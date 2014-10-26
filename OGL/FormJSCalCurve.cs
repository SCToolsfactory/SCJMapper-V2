using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using SCJMapper_V2.TextureLoaders;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace SCJMapper_V2
{
  public partial class FormJSCalCurve : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public System.Boolean Canceled { get; set; }

    private bool loaded = false;

    #region OGL Fields

    // Shader
    int VertexShaderObject, FragmentShaderObject, ProgramObject;

    // Textures
    const TextureUnit TMU0_Unit = TextureUnit.Texture0;
    const int TMU0_UnitInteger = 0;
    String TMU0_Filename = "";
    uint TMU0_Handle;
    TextureTarget TMU0_Target;

    private String[] SBFiles = { "graphics/SB_OutThere1.dds", "graphics/Skybox.dds", "graphics/SB_Canyon.dds", "graphics/SB_Shiodome.dds", "graphics/SB_Highway.dds", "graphics/SB_BigSight.dds" };
    // index into SBFiles
    const int  SB_OutThere1 = 0;
    const int  SB_Skybox = 1;
    const int  SB_Canyon = 2;
    const int  SB_Shiodome = 3;
    const int  SB_Highway = 4;
    const int  SB_BigSight = 5;


    #endregion internal Fields

    #region Handling Vars

    // timing
    private Int64 m_msElapsed = 0;
    private Int64 m_ticks = 0;
    private double DegPerMS = 360.0 / 3000.0;

    // location / acceleration
    private RK4Integrator m_flightModel = new RK4Integrator( );
    private double m_damping = 5000; // range is around 3000 .. 30000

    Label[] lblIn = null;
    Label[] lblOut = null;

    BezierSeries m_bSeries = new BezierSeries( );

    #endregion


    #region Form Handling

    public FormJSCalCurve( )
    {
      InitializeComponent( );

      log.Info( "Enter FormJSCalCurve" );

      // helpers
      lblIn = new Label[] { null, lblIn1, lblIn2, lblIn3, null, null };     // goes with PtNo 1..
      lblOut = new Label[] { null, lblOut1, lblOut2, lblOut3, lblOutSense, lblOutExponent }; // goes with PtNo 1..


      // add 5 points to the chart data series ( Zero, user1..3, max)
      for ( int i=0; i < 5; i++ ) {
        m_bSeries.BezierPoints.Add( new DataPoint( 0, 0 ) );
      }
      m_bSeries.ChartType = SeriesChartType.Line;
      m_bSeries.Name = "Curve";
      chart1.Series[0] = m_bSeries;
      // Create the Marker Series
      chart1.Series.Add( "Marker" );
      chart1.Series[1].ChartType = SeriesChartType.Point;
      chart1.Series[1].MarkerColor = Color.Orange;
      chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
      chart1.Series[1].Points.AddXY( 0, 0 );
      chart1.Series[1].Points.AddXY( 0.25, 0.25 );
      chart1.Series[1].Points.AddXY( 0.5, 0.5 );
      chart1.Series[1].Points.AddXY( 0.75, 0.75 );
      chart1.Series[1].Points.AddXY( 1.0, 1.0 );

      TMU0_Filename = SBFiles[SB_OutThere1]; // initial sky

      Canceled = true;
    }

    private void FormJSCalCurve_Load( object sender, EventArgs e )
    {
      rbHornet.Checked = true;
      // chain of triggers to maintain and format entries with default events...
      rbY.Checked = false;
      //back to default
      rbY.Checked = true;

      rbPtSense.Checked = false; // trigger value change..
      rbPtSense.Checked = true; // default
    }

    private void FormJSCalCurve_FormClosing( object sender, FormClosingEventArgs e )
    {

      if ( loaded ) {
        YawUpdateTuning( );
        PitchUpdateTuning( );
        RollUpdateTuning( );

        Application.Idle -= Application_Idle;
        GL.DeleteProgram( ProgramObject );
        GL.DeleteTexture( TMU0_Handle );
      }
    }

    #endregion


    #region YAW - Interaction

    private JoystickTuningParameter m_Ytuning = new JoystickTuningParameter( null );
    // live values
    private JoystickCls m_Yjs = null;
    private String m_liveYawCommand ="";
    private float m_liveYdeadzone = 0.0f;
    private float m_liveYsense = 1.0f;
    private float m_liveYexponent = 1.0f;
    private xyPoints m_liveYnonLinCurve = new xyPoints( 1000 );  // max val of Joystick Input

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    public JoystickTuningParameter YawTuning
    {
      get
      {
        YawUpdateTuning( );
        return m_Ytuning;
      }
      set
      {
        m_Ytuning = value;
        // populate from input
        lblYCmd.Text = m_Ytuning.Command;
        m_liveYawCommand = m_Ytuning.CommandCtrl;
        m_Yjs = m_Ytuning.JsDevice;
        log.Info( "FormJSCalCurve : Yaw Command is: " + value );

        cbxYinvert.Checked = m_Ytuning.InvertUsed;
        cbxYdeadzone.Checked = m_Ytuning.DeadzoneUsed;
        lblYdeadzone.Text = m_Ytuning.Deadzone;
        cbxYsense.Checked = m_Ytuning.SensitivityUsed;
        lblYsense.Text = m_Ytuning.Sensitivity;
        cbxYexpo.Checked = m_Ytuning.ExponentUsed;
        lblYexponent.Text = m_Ytuning.Exponent;
        cbxYpts.Checked = m_Ytuning.NonLinCurveUsed;
        if ( m_Ytuning.NonLinCurveUsed ) {
          lblYin1.Text = m_Ytuning.NonLinCurvePtsIn[0]; lblYin2.Text = m_Ytuning.NonLinCurvePtsIn[1]; lblYin3.Text = m_Ytuning.NonLinCurvePtsIn[2];
          lblYout1.Text = m_Ytuning.NonLinCurvePtsOut[0]; lblYout2.Text = m_Ytuning.NonLinCurvePtsOut[1]; lblYout3.Text = m_Ytuning.NonLinCurvePtsOut[2];
        }
        else {
          lblYin1.Text = "0.250"; lblYin2.Text = "0.500"; lblYin3.Text = "0.750";
          lblYout1.Text = "0.250"; lblYout2.Text = "0.500"; lblYout3.Text = "0.750";
        }
        // update live values
        m_liveYdeadzone = 1000.0f * float.Parse( lblYdeadzone.Text ); // scale for JS axis
        m_liveYsense = float.Parse( lblYsense.Text );
        m_liveYexponent = float.Parse( lblYexponent.Text );
        if ( m_liveYnonLinCurve != null ) {
          m_liveYnonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
        }
      }
    }

    private void YawUpdateTuning( )
    {
      // update from left area labels (xyUsed items are updated on change - so they are actual allready)
      m_Ytuning.Deadzone = lblYdeadzone.Text;
      m_Ytuning.Sensitivity = lblYsense.Text;
      m_Ytuning.Exponent = lblYexponent.Text;
      List<String> pts = new List<String>( );
      pts.Add( lblYin1.Text ); pts.Add( lblYin2.Text ); pts.Add( lblYin3.Text );
      m_Ytuning.NonLinCurvePtsIn = pts;
      pts = new List<String>( );
      pts.Add( lblYout1.Text ); pts.Add( lblYout2.Text ); pts.Add( lblYout3.Text );
      m_Ytuning.NonLinCurvePtsOut = pts;
    }

    #endregion


    #region PITCH - Interaction

    private JoystickTuningParameter m_Ptuning = new JoystickTuningParameter( null );
    // live values
    private JoystickCls m_Pjs = null;
    private String m_livePitchCommand ="";
    private float m_livePdeadzone = 0.0f;
    private float m_livePsense = 1.0f;
    private float m_livePexponent = 1.0f;
    private xyPoints m_livePnonLinCurve = new xyPoints( 1000 );  // max val of Joystick Input

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    public JoystickTuningParameter PitchTuning
    {
      get
      {
        // update 
        PitchUpdateTuning( );
        return m_Ptuning;
      }
      set
      {
        m_Ptuning = value;
        // populate from input
        lblPCmd.Text = m_Ptuning.Command;  // 
        m_livePitchCommand = m_Ptuning.CommandCtrl;
        m_Pjs = m_Ptuning.JsDevice;
        log.Info( "FormJSCalCurve : Pitch Command is: " + value );

        cbxPinvert.Checked = m_Ptuning.InvertUsed;
        cbxPdeadzone.Checked = m_Ptuning.DeadzoneUsed;
        lblPdeadzone.Text = m_Ptuning.Deadzone;
        cbxPsense.Checked = m_Ptuning.SensitivityUsed;
        lblPsense.Text = m_Ptuning.Sensitivity;
        cbxPexpo.Checked = m_Ptuning.ExponentUsed;
        lblPexponent.Text = m_Ptuning.Exponent;
        cbxPpts.Checked = m_Ptuning.NonLinCurveUsed;
        if ( m_Ptuning.NonLinCurveUsed ) {
          lblPin1.Text = m_Ptuning.NonLinCurvePtsIn[0]; lblPin2.Text = m_Ptuning.NonLinCurvePtsIn[1]; lblPin3.Text = m_Ptuning.NonLinCurvePtsIn[2];
          lblPout1.Text = m_Ptuning.NonLinCurvePtsOut[0]; lblPout2.Text = m_Ptuning.NonLinCurvePtsOut[1]; lblPout3.Text = m_Ptuning.NonLinCurvePtsOut[2];
        }
        else {
          lblPin1.Text = "0.250"; lblPin2.Text = "0.500"; lblPin3.Text = "0.750";
          lblPout1.Text = "0.250"; lblPout2.Text = "0.500"; lblPout3.Text = "0.750";
        }
        // update live values
        m_livePdeadzone = 1000.0f * float.Parse( lblPdeadzone.Text );
        m_livePsense = float.Parse( lblPsense.Text );
        m_livePexponent = float.Parse( lblPexponent.Text );
        if ( m_livePnonLinCurve != null ) {
          m_livePnonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
        }
      }
    }

    private void PitchUpdateTuning( )
    {
      // update from left area labels (xyUsed items are updated on change - so they are actual allready)
      m_Ptuning.Deadzone = lblPdeadzone.Text;
      m_Ptuning.Sensitivity = lblPsense.Text;
      m_Ptuning.Exponent = lblPexponent.Text;
      List<String> pts = new List<String>( );
      pts.Add( lblPin1.Text ); pts.Add( lblPin2.Text ); pts.Add( lblPin3.Text );
      m_Ptuning.NonLinCurvePtsIn = pts;
      pts = new List<String>( );
      pts.Add( lblPout1.Text ); pts.Add( lblPout2.Text ); pts.Add( lblPout3.Text );
      m_Ptuning.NonLinCurvePtsOut = pts;
    }

    #endregion


    #region ROLL - Interaction

    private JoystickTuningParameter m_Rtuning = new JoystickTuningParameter( null );
    // live values
    private JoystickCls m_Rjs = null;
    private String m_liveRollCommand;
    private float m_liveRdeadzone = 0.0f;
    private float m_liveRsense = 1.0f;
    private float m_liveRexponent = 1.0f;
    private xyPoints m_liveRnonLinCurve = new xyPoints( 1000 );  // max val of Joystick Input

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    public JoystickTuningParameter RollTuning
    {
      get
      {
        // update 
        RollUpdateTuning( );
        return m_Rtuning;
      }
      set
      {
        m_Rtuning = value;
        // populate from input
        lblRCmd.Text = m_Rtuning.Command;  // 
        m_liveRollCommand = m_Rtuning.CommandCtrl;
        m_Rjs = m_Rtuning.JsDevice;
        log.Info( "FormJSCalCurve : Roll Command is: " + value );

        cbxRinvert.Checked = m_Rtuning.InvertUsed;
        cbxRdeadzone.Checked = m_Rtuning.DeadzoneUsed;
        lblRdeadzone.Text = m_Rtuning.Deadzone;
        cbxRsense.Checked = m_Rtuning.SensitivityUsed;
        lblRsense.Text = m_Rtuning.Sensitivity;
        cbxRexpo.Checked = m_Rtuning.ExponentUsed;
        lblRexponent.Text = m_Rtuning.Exponent;
        cbxRpts.Checked = m_Rtuning.NonLinCurveUsed;
        if ( m_Rtuning.NonLinCurveUsed ) {
          lblRin1.Text = m_Rtuning.NonLinCurvePtsIn[0]; lblRin2.Text = m_Rtuning.NonLinCurvePtsIn[1]; lblRin3.Text = m_Rtuning.NonLinCurvePtsIn[2];
          lblRout1.Text = m_Rtuning.NonLinCurvePtsOut[0]; lblRout2.Text = m_Rtuning.NonLinCurvePtsOut[1]; lblRout3.Text = m_Rtuning.NonLinCurvePtsOut[2];
        }
        else {
          lblRin1.Text = "0.250"; lblRin2.Text = "0.500"; lblRin3.Text = "0.750";
          lblRout1.Text = "0.250"; lblRout2.Text = "0.500"; lblRout3.Text = "0.750";
        }
        // update live values
        m_liveRdeadzone = 1000.0f * float.Parse( lblRdeadzone.Text );
        m_liveRsense = float.Parse( lblRsense.Text );
        m_liveRexponent = float.Parse( lblRexponent.Text );
        if ( m_liveRnonLinCurve != null ) {
          m_liveRnonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
        }
      }
    }

    private void RollUpdateTuning( )
    {
      // update from left area labels (xyUsed items are updated on change - so they are actual allready)
      m_Rtuning.Deadzone = lblRdeadzone.Text;
      m_Rtuning.Sensitivity = lblRsense.Text;
      m_Rtuning.Exponent = lblRexponent.Text;
      List<String> pts = new List<String>( );
      pts.Add( lblRin1.Text ); pts.Add( lblRin2.Text ); pts.Add( lblRin3.Text );
      m_Rtuning.NonLinCurvePtsIn = pts;
      pts = new List<String>( );
      pts.Add( lblRout1.Text ); pts.Add( lblRout2.Text ); pts.Add( lblRout3.Text );
      m_Rtuning.NonLinCurvePtsOut = pts;
    }

    #endregion


    #region OGL Content

    private void LoadSkybox( )
    {
      TextureLoaderParameters.FlipImages = false;
      TextureLoaderParameters.MagnificationFilter = TextureMagFilter.Linear;
      TextureLoaderParameters.MinificationFilter = TextureMinFilter.Linear;
      TextureLoaderParameters.WrapModeS = TextureWrapMode.ClampToEdge;
      TextureLoaderParameters.WrapModeT = TextureWrapMode.ClampToEdge;
      TextureLoaderParameters.EnvMode = TextureEnvMode.Modulate;

      try {
        ImageDDS.LoadFromDisk( TMU0_Filename, out TMU0_Handle, out TMU0_Target );
        log.Info( "Loaded " + TMU0_Filename + " with handle " + TMU0_Handle + " as " + TMU0_Target );
      }
      catch ( Exception ex ) {
        log.Error( "Error loading DDS file:", ex );
      }

      log.Info( "End of Texture Loading. GL Error: " + GL.GetError( ) );

    }


    private void glControl1_Load( object sender, EventArgs e )
    {
      log.Info( "Enter glControl1_Load" );


      GL.ClearColor( Color.SkyBlue ); // Yey! .NET Colors can be used directly!
      SetupViewport( );
      Application.Idle += Application_Idle; // press TAB twice after +=

      // init time keeping
      m_ticks = DateTime.Now.Ticks;
      m_msElapsed = 0;


      ////////////////////////////////

      // Check for necessary capabilities:
      string extensions = GL.GetString( StringName.Extensions );
      if ( !GL.GetString( StringName.Extensions ).Contains( "GL_ARB_shading_language" ) ) {
        log.ErrorFormat( "glControl1_Load - This program requires OpenGL 2.0. Found {0}. Aborting.", GL.GetString( StringName.Version ).Substring( 0, 3 ) );

        throw new NotSupportedException( String.Format( "This program requires OpenGL 2.0. Found {0}. Aborting.",
            GL.GetString( StringName.Version ).Substring( 0, 3 ) ) );
      }

      if ( !extensions.Contains( "GL_ARB_texture_compression" ) ||
           !extensions.Contains( "GL_EXT_texture_compression_s3tc" ) ) {
        log.Error( "glControl1_Load - This program requires support for texture compression. Aborting." );

        throw new NotSupportedException( "This program requires support for texture compression. Aborting." );
      }

      #region GL State

      GL.ClearColor( 0f, 0f, 0f, 0f );

      GL.Disable( EnableCap.Dither );

      GL.Enable( EnableCap.CullFace );
      GL.FrontFace( FrontFaceDirection.Ccw );
      GL.PolygonMode( MaterialFace.Front, PolygonMode.Fill );

      #endregion GL State

      #region Shaders

      string LogInfo;

      // Load&Compile Vertex Shader
      // Thanks:  http://www.rioki.org/2013/03/07/glsl-skybox.html

      // GLSL for vertex shader.
      VertexShaderObject = GL.CreateShader( ShaderType.VertexShader );
      String vertSource = @"
              #extension GL_ARB_gpu_shader5 : enable
              void main()
              {
                  mat4 r = gl_ModelViewMatrix;
                  r[3][0] = 0.0;
                  r[3][1] = 0.0;
                  r[3][2] = 0.0;

                  vec4 v = inverse(r) * gl_ProjectionMatrixInverse * gl_Vertex;

                  gl_TexCoord[0] = v; 
                  gl_Position    = gl_Vertex;
              }            
            ";
      // compile shader
      compileShader( VertexShaderObject, vertSource );


      // GLSL for fragment shader.
      FragmentShaderObject = GL.CreateShader( ShaderType.FragmentShader );
      String fragSource = @"
              uniform samplerCube Skybox;

              void main()
              {
                  gl_FragColor = textureCube(Skybox, gl_TexCoord[0]);
              }
            ";
      // compile shader
      compileShader( FragmentShaderObject, fragSource );

      // Link the Shaders to a usable Program
      ProgramObject = GL.CreateProgram( );
      GL.AttachShader( ProgramObject, VertexShaderObject );
      GL.AttachShader( ProgramObject, FragmentShaderObject );

      // link it all together
      GL.LinkProgram( ProgramObject );

      // flag ShaderObjects for delete when not used anymore
      GL.DeleteShader( VertexShaderObject );
      GL.DeleteShader( FragmentShaderObject );

      int[] temp = new int[1];
      GL.GetProgram( ProgramObject, GetProgramParameterName.LinkStatus, out temp[0] );
      log.Info( "Linking Program (" + ProgramObject + ") " + ( ( temp[0] == 1 ) ? "succeeded." : "FAILED!" ) );
      if ( temp[0] != 1 ) {
        GL.GetProgramInfoLog( ProgramObject, out LogInfo );
        log.Error( "Program Log:\n" + LogInfo );
      }

      GL.GetProgram( ProgramObject, GetProgramParameterName.ActiveAttributes, out temp[0] );
      log.Info( "Program registered " + temp[0] + " Attributes." );
      log.Info( "End of Shader build. GL Error: " + GL.GetError( ) );

      #endregion Shaders

      #region Textures

      LoadSkybox( );

      #endregion Textures


      loaded = true;
    }


    private void glControl1_Paint( object sender, PaintEventArgs e )
    {
      if ( !loaded ) return;
      Render( );
    }


    private void glControl1_Resize( object sender, EventArgs e )
    {
      SetupViewport( );
      if ( m_bSeries != null ) m_bSeries.Invalidate( chart1 );
      else chart1.Invalidate( );
    }


    /// <summary>
    /// Helper method to avoid code duplication.
    /// Compiles a shader and prints results using Debug.WriteLine.
    /// </summary>
    /// <param name="shader">A shader object, gotten from GL.CreateShader.</param>
    /// <param name="source">The GLSL source to compile.</param>
    void compileShader( int shader, string source )
    {
      GL.ShaderSource( shader, source );
      GL.CompileShader( shader );

      string info;
      GL.GetShaderInfoLog( shader, out info );
      Trace.WriteLine( info );

      int compileResult;
      GL.GetShader( shader, ShaderParameter.CompileStatus, out compileResult );
      if ( compileResult != 1 ) {
        log.Error( "compileShader - Compile Error:" );
        log.Error( source );
      }
    }



    private void SetupViewport( )
    {
      int w = glControl1.Width;
      int h = glControl1.Height;

      GL.Viewport( 0, 0, w, h ); // Use all of the glControl painting area

      GL.MatrixMode( MatrixMode.Projection );
      Matrix4 p = Matrix4.CreatePerspectiveFieldOfView( MathHelper.PiOver4, w / ( float )h, 0.1f, 10.0f );
      GL.LoadMatrix( ref p );

      GL.MatrixMode( MatrixMode.Modelview );
      GL.LoadIdentity( );
    }

    // One render cycle - beware this should be fast...
    private void Render( )
    {
      if ( !loaded ) return;

      GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );

      GL.UseProgram( ProgramObject ); // use the compiled shader

      // interface with shader variables
      GL.Uniform1( GL.GetUniformLocation( ProgramObject, "Skybox" ), TMU0_UnitInteger );

      // get our own matrix
      GL.PushMatrix( );
      GL.LoadIdentity( );  // Reset and transform the matrix.

      // part of the 3D orientation - the rest is in the idle routine 
      Matrix4d rm = new Matrix4d( m_right.X, m_right.Y, m_right.Z, 0,
                                m_up.X, m_up.Y, m_up.Z, 0,
                                m_front.X, m_front.Y, m_front.Z, 0,
                                0, 0, 0, 1 );
      GL.MultMatrix( ref rm ); // transform

      // Enable/Disable features
      GL.PushAttrib( AttribMask.EnableBit );
      GL.DepthMask( false );
      GL.Disable( EnableCap.DepthTest );
      GL.Disable( EnableCap.Lighting );
      GL.Disable( EnableCap.Blend );

      // use the Skybox texture
      GL.ActiveTexture( TMU0_Unit );
      GL.BindTexture( TMU0_Target, TMU0_Handle );

      // Draw
      GL.Color3( 1f, 1f, 1f );  // Just in case we set all vertices to white.

      // draw one Quad only
      GL.Begin( PrimitiveType.Quads );
      {
        GL.Vertex3( -1.0, -1.0, 0.0 );
        GL.Vertex3( 1.0, -1.0, 0.0 );
        GL.Vertex3( 1.0, 1.0, 0.0 );
        GL.Vertex3( -1.0, 1.0, 0.0 );
      }
      GL.End( );

      // END Draw


      // Restore enable bits and matrix
      GL.PopAttrib( );
      GL.PopMatrix( );

      // finally show the contents
      glControl1.SwapBuffers( );
    }

    #endregion


    #region Joystick Input Handling

    /// <summary>
    /// Proper 3D camera aiming...
    /// Thanks: http://tutorialrandom.blogspot.ch/2012/08/how-to-rotate-in-3d-using-opengl-proper.html
    /// 
    /// </summary>
    Vector3d m_right = Vector3d.UnitX;
    Vector3d m_up = Vector3d.UnitY;
    Vector3d m_front = Vector3d.UnitZ;

    /// <summary>
    /// Calc the view vector - take care of changing axis orientations 
    /// </summary>
    /// <param name="x">Right-Left Direction</param>
    /// <param name="y">Up-Down Direction</param>
    /// <param name="z">RotLeft - RotRight Direction</param>
    private void rotDeg( Vector3d dir )
    {
      Matrix4d temp = Matrix4d.CreateRotationX( MathHelper.DegreesToRadians( dir.Y ) );// invert y-> x
      m_right = Vector3d.TransformVector( m_right, temp );
      m_up = Vector3d.TransformVector( m_up, temp );
      m_front = Vector3d.TransformVector( m_front, temp );

      temp = Matrix4d.CreateRotationY( MathHelper.DegreesToRadians( dir.X ) ); // invert x-> y
      m_right = Vector3d.TransformVector( m_right, temp );
      m_up = Vector3d.TransformVector( m_up, temp );
      m_front = Vector3d.TransformVector( m_front, temp );

      temp = Matrix4d.CreateRotationZ( MathHelper.DegreesToRadians( dir.Z ) );
      m_right = Vector3d.TransformVector( m_right, temp );
      m_up = Vector3d.TransformVector( m_up, temp );
      m_front = Vector3d.TransformVector( m_front, temp );
    }


    /// <summary>
    /// Handle user input while Idle
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Application_Idle( object sender, EventArgs e )
    {
      // no guard needed -- we hooked into the event in Load handler
      if ( glControl1.IsDisposed ) return;
      if ( glControl1.Context == null ) return;

      while ( glControl1.IsIdle ) {
        Vector3d m = Vector3d.Zero; ;
        // calculate the aim change while the user is handling the control (integrating the amount of control)
        Int64 newTick = DateTime.Now.Ticks;
        m_msElapsed = ( newTick - m_ticks ) / TimeSpan.TicksPerMillisecond;
        if ( m_msElapsed < 20 ) continue; //pace updates with 20 ms minimum (50fps max)

        m_ticks = newTick; // prep next run

        int i_x,i_y,i_z = 0; // Joystick Input
        // query the Josticks for the 3 controls
        m_Yjs.GetCmdData( m_liveYawCommand, out i_x ); // + = right
        m_Pjs.GetCmdData( m_livePitchCommand, out i_y ); // + = up
        m_Rjs.GetCmdData( m_liveRollCommand, out i_z ); // += twist right

        // apply the modifications of the control (deadzone, shape, sensitivity)
        int x = i_x; int y = i_y; int z = i_z; // retain real input as i_xyz
        m_flightModel.Velocity = Vector3d.Zero;

        // Yaw
        if ( m_Ytuning.DeadzoneUsed ) {
          int sx = Math.Sign( x );
          x = ( int )( Math.Abs( x ) - m_liveYdeadzone ); x = ( x < 0 ) ? 0 : x * sx; // DZ is subtracted from the input
        }
        {
          double fout = 0.0;
          if ( m_Ytuning.ExponentUsed ) {
            fout = Math.Pow( Math.Abs( x / 1000.0 ), m_liveYexponent ) * ( ( m_Ytuning.SensitivityUsed ) ? m_liveYsense : 1.0 ) * Math.Sign( x );
          }
          else if ( m_Ytuning.NonLinCurveUsed ) {
            fout = m_liveYnonLinCurve.EvalX( x ) * ( ( m_Ytuning.SensitivityUsed ) ? m_liveYsense : 1.0 );
          }
          else {
            fout = Math.Abs( x / 1000.0 ) * ( ( m_Ytuning.SensitivityUsed ) ? m_liveYsense : 1.0 ) * Math.Sign( x );
          }
          fout = ( fout > 1.0 ) ? 1.0 : fout; // safeguard against any overshoots
          // update in/out labels if active axis
          lblYInput.Text = ( i_x / 1000.0 ).ToString( "0.00" ); lblYOutput.Text = ( fout ).ToString( "0.00" );
          // calculate new direction vector
          m.X = ( ( m_Ytuning.InvertUsed ) ? -1 : 1 ) * ( ( !cbYuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
        }

        // Pitch
        if ( m_Ptuning.DeadzoneUsed ) {
          int sy = Math.Sign( y );
          y = ( int )( Math.Abs( y ) - m_livePdeadzone ); y = ( y < 0 ) ? 0 : y * sy;
        }
        {
          double fout = 0.0;
          if ( m_Ptuning.ExponentUsed ) {
            fout = Math.Pow( Math.Abs( y / 1000.0 ), m_livePexponent ) * ( ( m_Ptuning.SensitivityUsed ) ? m_livePsense : 1.0 ) * Math.Sign( y );
          }
          else if ( m_Ptuning.NonLinCurveUsed ) {
            fout = m_livePnonLinCurve.EvalX( y ) * ( ( m_Ptuning.SensitivityUsed ) ? m_livePsense : 1.0 );
          }
          else {
            fout = Math.Abs( y / 1000.0 ) * ( ( m_Ptuning.SensitivityUsed ) ? m_livePsense : 1.0 ) * Math.Sign( y );
          }
          fout = ( fout > 1.0 ) ? 1.0 : fout;
          lblPInput.Text = ( i_y / 1000.0 ).ToString( "0.00" ); lblPOutput.Text = ( fout ).ToString( "0.00" );
          m.Y = ( ( m_Ptuning.InvertUsed ) ? -1 : 1 ) * ( ( !cbPuse.Checked ) ? -fout : 0 ) * m_msElapsed * DegPerMS;
        }

        // Roll
        if ( m_Rtuning.DeadzoneUsed ) {
          int sz = Math.Sign( z );
          z = ( int )( Math.Abs( z ) - m_liveRdeadzone ); z = ( z < 0 ) ? 0 : z * sz;
        }
        {
          double fout = 0.0;
          if ( m_Rtuning.ExponentUsed ) {
            fout = Math.Pow( Math.Abs( z / 1000.0 ), m_liveRexponent ) * ( ( m_Rtuning.SensitivityUsed ) ? m_liveRsense : 1.0 ) * Math.Sign( z );
          }
          else if ( m_Rtuning.NonLinCurveUsed ) {
            fout = m_liveRnonLinCurve.EvalX( z ) * ( ( m_Rtuning.SensitivityUsed ) ? m_liveRsense : 1.0 );
          }
          else {
            fout = Math.Abs( z / 1000.0 ) * ( ( m_Rtuning.SensitivityUsed ) ? m_liveRsense : 1.0 ) * Math.Sign( z );
          }
          fout = ( fout > 1.0 ) ? 1.0 : fout;
          lblRInput.Text = ( i_z / 1000.0 ).ToString( "0.00" ); lblROutput.Text = ( fout ).ToString( "0.00" );
          m.Z = ( ( m_Rtuning.InvertUsed ) ? -1 : 1 ) * ( ( !cbRuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
        }

        // finalize
        m_flightModel.Velocity -= m; // new direction change vector
        if ( m_msElapsed > 1000 ) m_msElapsed = 1000; // safeguard against locking (moving the window)
        Vector3d deltaAngleV = m_flightModel.Integrate( ( double )m_msElapsed / 1000.0, m_damping, 100.0 ); // heuristic K and B ..

        // rotate the view along the input 
        // rotDeg( m );
        rotDeg( deltaAngleV );

        // render once more
        Render( );

      }//while
    }

    #endregion


    #region Event Handling

    #region turnspeed things

    private void rbAurora_CheckedChanged( object sender, EventArgs e )
    {
      slDamping.Value = 6;
      slTurnSpeed.Value = 10;
    }

    private void rb300i_CheckedChanged( object sender, EventArgs e )
    {
      slDamping.Value = 6;
      slTurnSpeed.Value = 6; // turns in 3 seconds 360deg
    }

    private void rbHornet_CheckedChanged( object sender, EventArgs e )
    {
      slDamping.Value = 6;
      slTurnSpeed.Value = 8;
    }

    private void slTurnSpeed_ValueChanged( object sender, EventArgs e )
    {
      // recalc the turning scale based on one full 360 deg sweep in the given time (sec)
      DegPerMS = 360.0 / ( slTurnSpeed.Value * 500.0 );
      lblTurnspeed.Text = ( slTurnSpeed.Value / 2.0 ).ToString( );
    }

    private void slDamping_ValueChanged( object sender, EventArgs e )
    {
      m_damping = ( slDamping.Maximum - slDamping.Value + 1 ) * 100.0; // 100 .. 1000
      lblDamping.Text = slDamping.Value.ToString( );
    }

    #endregion

    // Deadzone slider 00 .. 30 -> 0 .. 0.15

    private void tbDeadzone_ValueChanged( object sender, EventArgs e )
    {
      lblDeadzone.Text = ( tbDeadzone.Value / 200.0f ).ToString( "0.000" );
      float curDeadzone = 1000.0f * ( tbDeadzone.Value / 200.0f );  // % scaled to maxAxis

      if ( rbY.Checked == true ) {
        m_liveYdeadzone = curDeadzone;
        lblYdeadzone.Text = lblDeadzone.Text;
      }
      else if ( rbP.Checked == true ) {
        m_livePdeadzone = curDeadzone;
        lblPdeadzone.Text = lblDeadzone.Text;
      }
      else if ( rbR.Checked == true ) {
        m_liveRdeadzone = curDeadzone;
        lblRdeadzone.Text = lblDeadzone.Text;
      }
    }


    #region Active Axis Changed

    /// <summary>
    /// Make Yaw Active
    /// </summary>
    private void rbY_CheckedChanged( object sender, EventArgs e )
    {
      if ( rbY.Checked == true ) {
        // get Labels from left area (current)
        tbDeadzone.Value = ( int )( float.Parse( lblYdeadzone.Text ) * 200f ); // updates Text and live field too
        lblIn[1].Text = lblYin1.Text; lblIn[2].Text = lblYin2.Text; lblIn[3].Text = lblYin3.Text;
        lblOut[1].Text = lblYout1.Text; lblOut[2].Text = lblYout2.Text; lblOut[3].Text = lblYout3.Text;
        lblOut[4].Text = lblYsense.Text;
        lblOut[5].Text = lblYexponent.Text;

        // setup chart along the choosen parameter
        rbPtSense.Checked = true; // force back to sense (available for both..)
        UpdateChartItems( );
      }
    }


    /// <summary>
    /// Make Pitch Active
    /// </summary>
    private void rbP_CheckedChanged( object sender, EventArgs e )
    {
      if ( rbP.Checked == true ) {
        // get Labels from left area (current)
        tbDeadzone.Value = ( int )( float.Parse( lblPdeadzone.Text ) * 200f ); // updates Text and live field too
        lblIn[1].Text = lblPin1.Text; lblIn[2].Text = lblPin2.Text; lblIn[3].Text = lblPin3.Text;
        lblOut[1].Text = lblPout1.Text; lblOut[2].Text = lblPout2.Text; lblOut[3].Text = lblPout3.Text;
        lblOut[4].Text = lblPsense.Text;
        lblOut[5].Text = lblPexponent.Text;

        // setup chart along the choosen parameter
        rbPtSense.Checked = true; // force back to sense (available for both..)
        UpdateChartItems( );
      }
    }

    /// <summary>
    /// Make Roll Active
    /// </summary>
    private void rbR_CheckedChanged( object sender, EventArgs e )
    {
      if ( rbR.Checked == true ) {
        // get Labels from left area (current)
        tbDeadzone.Value = ( int )( float.Parse( lblRdeadzone.Text ) * 200f ); // updates Text and live field too
        lblIn[1].Text = lblRin1.Text; lblIn[2].Text = lblRin2.Text; lblIn[3].Text = lblRin3.Text;
        lblOut[1].Text = lblRout1.Text; lblOut[2].Text = lblRout2.Text; lblOut[3].Text = lblRout3.Text;
        lblOut[4].Text = lblRsense.Text;
        lblOut[5].Text = lblRexponent.Text;

        // setup chart along the choosen parameter
        rbPtSense.Checked = true; // force back to sense (available for both..)
        UpdateChartItems( );
      }
    }

    #endregion

    #region Charts section

    // Chart - move Pts

    /// <summary>
    /// Evaluate which tune parameter has the chart input
    /// </summary>
    private void EvalChartInput( )
    {
      m_hitPt = 0;
      if ( ( rbPt1.Enabled ) && ( rbPt1.Checked == true ) ) m_hitPt = 1;
      if ( ( rbPt2.Enabled ) && ( rbPt2.Checked == true ) ) m_hitPt = 2;
      if ( ( rbPt3.Enabled ) && ( rbPt3.Checked == true ) ) m_hitPt = 3;
      if ( ( rbPtSense.Enabled ) && ( rbPtSense.Checked == true ) ) m_hitPt = 4;
      if ( ( rbPtExponent.Enabled ) && ( rbPtExponent.Checked == true ) ) m_hitPt = 5;
    }


    /// <summary>
    /// Handle change of the mouse input within the chart
    /// </summary>
    private void rbPtAny_CheckedChanged( object sender, EventArgs e )
    {
      EvalChartInput( );
    }


    // handle mouse interaction with the chart

    int m_hitPt = 0;
    bool m_hitActive = false;
    int mX = 0; int mY = 0;

    /// <summary>
    /// Update the graph from changes of acitve label values
    /// </summary>
    private void UpdateChartItems( )
    {
      bool senseUsed = true;
      bool expUsed = true;
      bool ptsUsed = true;
      double sense;
      // see what is on display..
      if ( rbY.Checked == true ) {
        // Yaw
        senseUsed = ( m_Ytuning.SensitivityUsed == true );
        expUsed = ( m_Ytuning.ExponentUsed == true );
        ptsUsed = ( m_Ytuning.NonLinCurveUsed == true );
        chart1.BackColor = rbY.BackColor;
      }
      else if ( rbP.Checked == true ) {
        // Pitch
        senseUsed = ( m_Ptuning.SensitivityUsed == true );
        expUsed = ( m_Ptuning.ExponentUsed == true );
        ptsUsed = ( m_Ptuning.NonLinCurveUsed == true );
        chart1.BackColor = rbP.BackColor;
      }
      else {
        // Roll
        senseUsed = ( m_Rtuning.SensitivityUsed == true );
        expUsed = ( m_Rtuning.ExponentUsed == true );
        ptsUsed = ( m_Rtuning.NonLinCurveUsed == true );
        chart1.BackColor = rbR.BackColor;
      }

      // generic part
      rbPtSense.Enabled = senseUsed;
      rbPtExponent.Enabled = expUsed;
      rbPt1.Enabled = ptsUsed; rbPt2.Enabled = ptsUsed; rbPt3.Enabled = ptsUsed;
      EvalChartInput( );  // review active chart input

      sense = ( senseUsed ) ? double.Parse( lblOut[4].Text ) : 1.0; // use current or 1.0 if disabled 

      if ( expUsed ) {
        // Exp mode
        double expo = double.Parse( lblOut[5].Text );
        // dont touch zero Point
        m_bSeries.BezierPoints[1].SetValueXY( 0.25, sense * Math.Pow( 0.25, expo ) );
        m_bSeries.BezierPoints[2].SetValueXY( 0.5, sense * Math.Pow( 0.5, expo ) );
        m_bSeries.BezierPoints[3].SetValueXY( 0.75, sense * Math.Pow( 0.75, expo ) );
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, sense * 1.0 );
      }
      else if ( ptsUsed ) {
        // Pts mode
        // dont touch zero Point
        for ( int i=1; i <= 3; i++ ) {
          m_bSeries.BezierPoints[i].SetValueXY( float.Parse( lblIn[i].Text ), sense * float.Parse( lblOut[i].Text ) );
        }
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, sense * 1.0 );
      }
      else {
        // linear
        // dont touch zero Point
        m_bSeries.BezierPoints[1].SetValueXY( 0.25, sense * 0.25 );
        m_bSeries.BezierPoints[2].SetValueXY( 0.5, sense * 0.5 );
        m_bSeries.BezierPoints[3].SetValueXY( 0.75, sense * 0.75 );
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, sense * 1.0 );
      }
      // update markers from curve points
      chart1.Series[1].Points[1] = m_bSeries.BezierPoints[1];
      chart1.Series[1].Points[2] = m_bSeries.BezierPoints[2];
      chart1.Series[1].Points[3] = m_bSeries.BezierPoints[3];
      chart1.Series[1].Points[4] = m_bSeries.BezierPoints[4];

      m_bSeries.Invalidate( chart1 );
    }



    private void chartPoint_MouseDown( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      m_hitActive = true; // activate movement tracking
      mX = e.X; mY = e.Y; // save initial loc to get deltas
    }

    private void chartPoint_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      if ( m_hitActive ) {
        if ( m_hitPt < 1 ) {
          // nothing selected ...
        }
        else if ( m_hitPt <= 3 ) {
          // Pt1..3
          double newX = double.Parse( lblIn[m_hitPt].Text ) + ( e.X - mX ) * 0.001f; mX = e.X;
          newX = ( newX > 1.0f ) ? 1.0f : newX;
          newX = ( newX < 0.0f ) ? 0.0f : newX;
          lblIn[m_hitPt].Text = newX.ToString( "0.000" );

          double newY = double.Parse( lblOut[m_hitPt].Text ) + ( e.Y - mY ) * -0.001f; mY = e.Y;
          newY = ( newY > 1.0f ) ? 1.0f : newY;
          newY = ( newY < 0.0f ) ? 0.0f : newY;
          lblOut[m_hitPt].Text = newY.ToString( "0.000" );

          // update chart (Points[0] is zero)
          double sense = double.Parse( lblOut[4].Text );
          m_bSeries.BezierPoints[m_hitPt].SetValueXY( newX, sense * newY );
          // update markers from curve points
          chart1.Series[1].Points[m_hitPt] = m_bSeries.BezierPoints[m_hitPt];
        }

        else if ( m_hitPt == 4 ) {
          // sense 
          double newY = double.Parse( lblOut[m_hitPt].Text ) + ( e.Y - mY ) * -0.01f; mY = e.Y;
          newY = ( newY > 1.0f ) ? 1.0f : newY;
          newY = ( newY < 0.2f ) ? 0.2f : newY;
          lblOut[m_hitPt].Text = newY.ToString( "0.00" );

          // update chart (Points[0] is zero)
          // depends on Exp or Pt mode...
          if ( rbPtExponent.Enabled == true ) {
            // Exp mode
            double expo = double.Parse( lblOut[5].Text );
            m_bSeries.BezierPoints[1].SetValueXY( 0.25, newY * Math.Pow( 0.25, expo ) );
            m_bSeries.BezierPoints[2].SetValueXY( 0.5, newY * Math.Pow( 0.5, expo ) );
            m_bSeries.BezierPoints[3].SetValueXY( 0.75, newY * Math.Pow( 0.75, expo ) );
            m_bSeries.BezierPoints[4].SetValueXY( 1.0, newY * 1.0 );
          }
          else if ( rbPt1.Enabled && rbPt2.Enabled && rbPt3.Enabled ) { // TODO - this might be slow to check all rbs each time
            // Pts mode
            for ( int i=1; i <= 3; i++ ) {
              m_bSeries.BezierPoints[i].SetValueXY( float.Parse( lblIn[i].Text ), newY * float.Parse( lblOut[i].Text ) );
            }
            m_bSeries.BezierPoints[4].SetValueXY( 1.0, newY * 1.0 );
          }
          else {
            // neither expo nor pts -> linear only
            m_bSeries.BezierPoints[1].SetValueXY( 0.25, newY * 0.25 );
            m_bSeries.BezierPoints[2].SetValueXY( 0.5, newY * 0.5 );
            m_bSeries.BezierPoints[3].SetValueXY( 0.75, newY * 0.75 );
            m_bSeries.BezierPoints[4].SetValueXY( 1.0, newY * 1.0 );
          }
        }

        else if ( m_hitPt == 5 ) {
          // exponent
          double newY = double.Parse( lblOut[m_hitPt].Text ) + ( e.Y - mY ) * 0.01f; mY = e.Y;
          newY = ( newY > 3.0f ) ? 3.0f : newY;
          newY = ( newY < 0.5f ) ? 0.5f : newY;
          lblOut[m_hitPt].Text = newY.ToString( "0.00" );

          // update chart (Points[0] is zero)
          double sense = double.Parse( lblOut[4].Text );
          m_bSeries.BezierPoints[1].SetValueXY( 0.25, sense * Math.Pow( 0.25, newY ) );
          m_bSeries.BezierPoints[2].SetValueXY( 0.5, sense * Math.Pow( 0.5, newY ) );
          m_bSeries.BezierPoints[3].SetValueXY( 0.75, sense * Math.Pow( 0.75, newY ) );
        }

        // update markers from curve points
        chart1.Series[1].Points[1] = m_bSeries.BezierPoints[1];
        chart1.Series[1].Points[2] = m_bSeries.BezierPoints[2];
        chart1.Series[1].Points[3] = m_bSeries.BezierPoints[3];
        chart1.Series[1].Points[4] = m_bSeries.BezierPoints[4];

        m_bSeries.Invalidate( chart1 );

      }
    }

    private void chartPoint_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      m_hitActive = false;

      // update the rest of the fields from Entry

      if ( rbY.Checked == true ) {
        // left area labels
        lblYin1.Text = lblIn[1].Text; lblYin2.Text = lblIn[2].Text; lblYin3.Text = lblIn[3].Text;
        lblYout1.Text = lblOut[1].Text; lblYout2.Text = lblOut[2].Text; lblYout3.Text = lblOut[3].Text;
        lblYsense.Text = lblOut[4].Text;
        lblYexponent.Text = lblOut[5].Text;
        // update live values
        m_liveYsense = float.Parse( lblYsense.Text );
        m_liveYexponent = float.Parse( lblYexponent.Text );
        if ( m_liveYnonLinCurve != null ) {
          m_liveYnonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
        }
      }
      else if ( rbP.Checked == true ) {
        // left area labels
        lblPin1.Text = lblIn[1].Text; lblPin2.Text = lblIn[2].Text; lblPin3.Text = lblIn[3].Text;
        lblPout1.Text = lblOut[1].Text; lblPout2.Text = lblOut[2].Text; lblPout3.Text = lblOut[3].Text;
        lblPsense.Text = lblOut[4].Text;
        lblPexponent.Text = lblOut[5].Text;
        // update live values
        m_livePsense = float.Parse( lblPsense.Text );
        m_livePexponent = float.Parse( lblPexponent.Text );
        if ( m_livePnonLinCurve != null ) {
          m_livePnonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
        }
      }
      else if ( rbR.Checked == true ) {
        // left area labels
        lblRin1.Text = lblIn[1].Text; lblRin2.Text = lblIn[2].Text; lblRin3.Text = lblIn[3].Text;
        lblRout1.Text = lblOut[1].Text; lblRout2.Text = lblOut[2].Text; lblRout3.Text = lblOut[3].Text;
        lblRsense.Text = lblOut[4].Text;
        lblRexponent.Text = lblOut[5].Text;
        // update live values
        m_liveRsense = float.Parse( lblRsense.Text );
        m_liveRexponent = float.Parse( lblRexponent.Text );
        if ( m_liveRnonLinCurve != null ) {
          m_liveRnonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
        }
      }
    }
    #endregion

    #region Checked Invert Changed

    private void cbxYinvert_CheckedChanged( object sender, EventArgs e )
    {
      m_Ytuning.InvertUsed = false;
      if ( cbxYinvert.Checked == true ) {
        m_Ytuning.InvertUsed = true; // update storage
        rbY.Checked = true; // auto switch
      }
    }

    private void cbxPinvert_CheckedChanged( object sender, EventArgs e )
    {
      m_Ptuning.InvertUsed = false;
      if ( cbxPinvert.Checked == true ) {
        m_Ptuning.InvertUsed = true; // update storage
        rbP.Checked = true; // auto switch
      }
    }

    private void cbxRinvert_CheckedChanged( object sender, EventArgs e )
    {
      m_Rtuning.InvertUsed = false;
      if ( cbxRinvert.Checked == true ) {
        m_Rtuning.InvertUsed = true; // update storage
        rbR.Checked = true; // auto switch
      }
    }

    #endregion

    #region Checked Deadzone Changed

    private void cbxYdeadzone_CheckedChanged( object sender, EventArgs e )
    {
      m_Ytuning.DeadzoneUsed = false;
      if ( cbxYdeadzone.Checked == true ) {
        m_Ytuning.DeadzoneUsed = true; // update storage
        rbY.Checked = true; // auto switch
        if ( rbY.Checked == true ) tbDeadzone.Value = ( int )( float.Parse( lblYdeadzone.Text ) * 0.2f ); // go live
      }
    }

    private void cbxPdeadzone_CheckedChanged( object sender, EventArgs e )
    {
      m_Ptuning.DeadzoneUsed = false;
      if ( cbxPdeadzone.Checked == true ) {
        m_Ptuning.DeadzoneUsed = true; // update storage
        rbP.Checked = true; // auto switch
        if ( rbP.Checked == true ) tbDeadzone.Value = ( int )( float.Parse( lblPdeadzone.Text ) * 0.2f ); // go live
      }
    }

    private void cbxRdeadzone_CheckedChanged( object sender, EventArgs e )
    {
      m_Rtuning.DeadzoneUsed = false;
      if ( cbxRdeadzone.Checked == true ) {
        m_Rtuning.DeadzoneUsed = true; // update storage
        rbR.Checked = true; // auto switch
        if ( rbR.Checked == true ) tbDeadzone.Value = ( int )( float.Parse( lblRdeadzone.Text ) * 0.2f ); // go live
      }
    }

    #endregion

    #region Checked Sense Changed

    private void cbxYsense_CheckedChanged( object sender, EventArgs e )
    {
      m_Ytuning.SensitivityUsed = false;
      if ( cbxYsense.Checked == true ) {
        m_Ytuning.SensitivityUsed = true; // update storage
        rbY.Checked = true; // auto switch
        if ( rbY.Checked == true ) {
          lblOut[4].Text = lblYsense.Text; m_liveYsense = float.Parse( lblYsense.Text );  // go live
        }// go live
      }
      UpdateChartItems( );
    }

    private void cbxPsense_CheckedChanged( object sender, EventArgs e )
    {
      m_Ptuning.SensitivityUsed = false;
      if ( cbxPsense.Checked == true ) {
        m_Ptuning.SensitivityUsed = true; // update storage
        rbP.Checked = true; // auto switch
        if ( rbP.Checked == true ) {
          lblOut[4].Text = lblPsense.Text; m_livePsense = float.Parse( lblPsense.Text );  // go live
        }// go live
      }
      UpdateChartItems( );
    }

    private void cbxRsense_CheckedChanged( object sender, EventArgs e )
    {
      m_Rtuning.SensitivityUsed = false;
      if ( cbxRsense.Checked == true ) {
        m_Rtuning.SensitivityUsed = true; // update storage
        rbR.Checked = true; // auto switch
        if ( rbR.Checked == true ) {
          lblOut[4].Text = lblRsense.Text; m_liveRsense = float.Parse( lblRsense.Text );  // go live
        }// go live
      }
      UpdateChartItems( );
    }

    #endregion

    #region Checked Exponent Changed

    private void cbxYexpo_CheckedChanged( object sender, EventArgs e )
    {
      m_Ytuning.ExponentUsed = false;
      if ( cbxYexpo.Checked == true ) {
        m_Ytuning.ExponentUsed = true; // update storage
        cbxYpts.Checked = false;       // forced: either expo OR points
        rbY.Checked = true; // auto switch
        if ( rbY.Checked == true ) {
          lblOut[5].Text = lblYexponent.Text; m_liveYexponent = float.Parse( lblYexponent.Text );  // go live from left area fields
        }// go live
      }
      UpdateChartItems( );
    }

    private void cbxPexpo_CheckedChanged( object sender, EventArgs e )
    {
      m_Ptuning.ExponentUsed = false;
      if ( cbxPexpo.Checked == true ) {
        m_Ptuning.ExponentUsed = true; // update storage
        cbxPpts.Checked = false;       // forced: either expo OR points
        rbP.Checked = true; // auto switch
        if ( rbP.Checked == true ) {
          lblOut[5].Text = lblPexponent.Text; m_livePexponent = float.Parse( lblPexponent.Text ); // go live from left area fields
        }// go live
      }
      UpdateChartItems( );
    }

    private void cbxRexpo_CheckedChanged( object sender, EventArgs e )
    {
      m_Rtuning.ExponentUsed = false;
      if ( cbxRexpo.Checked == true ) {
        m_Rtuning.ExponentUsed = true; // update storage
        cbxRpts.Checked = false;       // forced: either expo OR points
        rbR.Checked = true; // auto switch
        if ( rbR.Checked == true ) {
          lblOut[5].Text = lblRexponent.Text; m_liveRexponent = float.Parse( lblRexponent.Text ); // go live from left area fields
        }// go live
      }
      UpdateChartItems( );
    }

    #endregion

    #region Checked Points Changed

    private void cbxYpts_CheckedChanged( object sender, EventArgs e )
    {
      m_Ytuning.NonLinCurveUsed = false;
      if ( cbxYpts.Checked == true ) {
        m_Ytuning.NonLinCurveUsed = true; // update storage
        cbxYexpo.Checked = false;       // forced: either expo OR points
        rbY.Checked = true; // auto switch
        if ( rbY.Checked == true ) {
          // go live from left area fields
          lblIn[1].Text = lblYin1.Text; lblIn[2].Text = lblYin2.Text; lblIn[3].Text = lblYin3.Text;
          lblOut[1].Text = lblYout1.Text; lblOut[2].Text = lblYout2.Text; lblOut[3].Text = lblYout3.Text;
          if ( m_liveYnonLinCurve != null ) {
            m_liveYnonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                  float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                  float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
          }
        }// go live
      }
      UpdateChartItems( );
    }

    private void cbxPpts_CheckedChanged( object sender, EventArgs e )
    {
      m_Ptuning.NonLinCurveUsed = false;
      if ( cbxPpts.Checked == true ) {
        m_Ptuning.NonLinCurveUsed = true; // update storage
        cbxPexpo.Checked = false;       // forced: either expo OR points
        rbP.Checked = true; // auto switch
        if ( rbP.Checked == true ) {
          // go live from left area fields
          lblIn[1].Text = lblPin1.Text; lblIn[2].Text = lblPin2.Text; lblIn[3].Text = lblPin3.Text;
          lblOut[1].Text = lblPout1.Text; lblOut[2].Text = lblPout2.Text; lblOut[3].Text = lblPout3.Text;
          if ( m_livePnonLinCurve != null ) {
            m_livePnonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                  float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                  float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
          }
        }// go live
      }
      UpdateChartItems( );
    }

    private void cbxRpts_CheckedChanged( object sender, EventArgs e )
    {
      m_Rtuning.NonLinCurveUsed = false;
      if ( cbxRpts.Checked == true ) {
        m_Rtuning.NonLinCurveUsed = true; // update storage
        cbxRexpo.Checked = false;       // forced: either expo OR points
        rbR.Checked = true; // auto switch
        if ( rbR.Checked == true ) {
          // go live from left area fields
          lblIn[1].Text = lblRin1.Text; lblIn[2].Text = lblRin2.Text; lblIn[3].Text = lblRin3.Text;
          lblOut[1].Text = lblRout1.Text; lblOut[2].Text = lblRout2.Text; lblOut[3].Text = lblRout3.Text;
          if ( m_liveRnonLinCurve != null ) {
            m_liveRnonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                  float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                  float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
          }
        }// go live
      }
      UpdateChartItems( );
    }

    #endregion

    #region Skybox Checked Changed

    private void rbOutThere1_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_OutThere1];
      LoadSkybox( );
    }

    private void rbOutThere2_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_Skybox];
      LoadSkybox( );
    }

    private void rbCanyon_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_Canyon];
      LoadSkybox( );
    }

    private void rbShiodome_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_Shiodome];
      LoadSkybox( );
    }

    private void rbHighway_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_Highway];
      LoadSkybox( );
    }

    private void rbBigSight_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_BigSight];
      LoadSkybox( );

    }

    #endregion


    private void btCopyToAllAxis_Click( object sender, EventArgs e )
    {
      // just copy to all labels
      lblYin1.Text = lblIn1.Text; lblYout1.Text = lblOut1.Text;
      lblYin2.Text = lblIn2.Text; lblYout2.Text = lblOut2.Text;
      lblYin3.Text = lblIn3.Text; lblYout3.Text = lblOut3.Text;

      lblPin1.Text = lblIn1.Text; lblPout1.Text = lblOut1.Text;
      lblPin2.Text = lblIn2.Text; lblPout2.Text = lblOut2.Text;
      lblPin3.Text = lblIn3.Text; lblPout3.Text = lblOut3.Text;

      lblRin1.Text = lblIn1.Text; lblRout1.Text = lblOut1.Text;
      lblRin2.Text = lblIn2.Text; lblRout2.Text = lblOut2.Text;
      lblRin3.Text = lblIn3.Text; lblRout3.Text = lblOut3.Text;
    }

    private void btDone_Click( object sender, EventArgs e )
    {
      // It ai setup as OK button - nothing here so far...
    }


    #endregion








  }
}
