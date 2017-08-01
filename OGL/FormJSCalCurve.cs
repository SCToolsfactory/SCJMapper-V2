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

using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

using SCJMapper_V2.OGL.TextureLoaders;
using SCJMapper_V2.Joystick;
using SCJMapper_V2.Options;

namespace SCJMapper_V2.OGL
{
  public partial class FormJSCalCurve : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public bool Canceled { get; set; }

    private bool loaded = false;

    #region OGL Fields

    // Shader
    int VertexShaderObject, FragmentShaderObject, ProgramObject;

    // Textures
    const TextureUnit TMU0_Unit = TextureUnit.Texture0;
    const int TMU0_UnitInteger = 0;
    string TMU0_Filename = "";
    uint TMU0_Handle;
    TextureTarget TMU0_Target;

    private string[] SBFiles = { "graphics/SB_OutThere1.dds", "graphics/SB_OutThere3.dds", "graphics/Skybox.dds", "graphics/SB_Canyon.dds",
                                "graphics/SB_Shiodome.dds", "graphics/SB_Highway.dds", "graphics/SB_BigSight.dds", "graphics/SB_LA_Helipad.dds", "graphics/SB_Sunset.dds" };
    // index into SBFiles
    const int SB_OutThere1 = 0;
    const int SB_OutThere3 = 1;
    const int SB_Skybox = 2;
    const int SB_Canyon = 3;
    const int SB_Shiodome = 4;
    const int SB_Highway = 5;
    const int SB_BigSight = 6;
    const int SB_LA_Helipad = 7;
    const int SB_Sunset = 8;


    #endregion internal Fields

    #region Handling Vars

    // timing
    private Int64 m_msElapsed = 0;
    private Int64 m_ticks = 0;
    private double DegPerMS = 360.0 / 3000.0;
    private const Int64 m_frameTime = 25; // max Frametime msec 1/ = fps
    // location / acceleration
    private RK4Integrator m_flightModel = new RK4Integrator( );
    private double m_damping = 5000; // range is around 3000 .. 30000

    Label[] lblIn = null;
    Label[] lblOut = null;

    BezierSeries m_bSeries = new BezierSeries( );

    bool m_isStrafe = false;  // Strafe or YPR

    #endregion


    #region Form Handling

    public FormJSCalCurve()
    {
      InitializeComponent( );

      log.Info( "Enter FormJSCalCurve" );

      // helpers
      lblIn = new Label[] { null, lblIn1, lblIn2, lblIn3, null, null };     // goes with PtNo 1..
      lblOut = new Label[] { null, lblOut1, lblOut2, lblOut3, lblOutExponent }; // goes with PtNo 1..

      // add 5 points to the chart data series ( Zero, user1..3, max)
      for ( int i = 0; i < 5; i++ ) {
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

      // sliders
      tbSlider.Maximum = Deviceoptions.DevOptSliderMax;
      tbSlider.TickFrequency = Deviceoptions.DevOptSliderTick;

      // OGL map
      TMU0_Filename = SBFiles[SB_OutThere1]; // initial sky

      Canceled = true;
    }

    private void FormJSCalCurve_Load( object sender, EventArgs e )
    {
      rbHornet.Checked = true;

      rbTuneYPR.Checked = false;
      rbTuneYPR.Checked = true;
      // chain of triggers to maintain and format entries with default events...
      rbY.Checked = false;
      //back to default
      rbY.Checked = true;

      rbPtDeadzone.Checked = false; // trigger value change..
      rbPtDeadzone.Checked = true; // default

      rbTuneYPR.Checked = true;
    }

    private void FormJSCalCurve_FormClosing( object sender, FormClosingEventArgs e )
    {

      if ( loaded ) {
        YawUpdateTuning( );
        PitchUpdateTuning( );
        RollUpdateTuning( );

        StrafeLatUpdateTuning( );
        StrafeVertUpdateTuning( );
        StrafeLonUpdateTuning( );

        Application.Idle -= Application_Idle;
        GL.DeleteProgram( ProgramObject );
        GL.DeleteTexture( TMU0_Handle );
      }
    }

    #endregion


    #region class LiveValues (internal class)

    private class LiveValues
    {
      // load live from TuningParameters
      public void Load( DeviceTuningParameter dp )
      {
        if ( dp != null ) {
          used = ( !string.IsNullOrEmpty( dp.NodeText ) );
          if ( !used ) return;

          nodetext = dp.NodeText;
          string[] e = nodetext.Split( new char[] { ActionTreeInputNode.RegDiv, ActionTreeInputNode.ModDiv }, StringSplitOptions.RemoveEmptyEntries );
          if ( e.Length > 0 )
            control = e[1].TrimEnd( );
          else
            control = dp.NodeText;
          command = dp.CommandCtrl;
          // the option data
          if ( dp.DeviceoptionRef == null ) {
            deadzoneUsed = false;
            saturationUsed = false;
          }
          else {
            deadzoneUsed = dp.DeviceoptionRef.DeadzoneUsed;
            deadzoneS = dp.DeviceoptionRef.Deadzone;
            saturationUsed = dp.DeviceoptionRef.SaturationUsed;
            saturationS = dp.DeviceoptionRef.Saturation;
          }
          invertUsed = dp.InvertUsed;
          exponentUsed = dp.ExponentUsed;
          exponentS = dp.Exponent;
          nonLinCurveUsed = dp.NonLinCurveUsed;
          if ( dp.NonLinCurveUsed ) {
            nonLinCurve.Curve( float.Parse( dp.NonLinCurvePtsIn[0] ), float.Parse( dp.NonLinCurvePtsOut[0] ),
                                   float.Parse( dp.NonLinCurvePtsIn[1] ), float.Parse( dp.NonLinCurvePtsOut[1] ),
                                   float.Parse( dp.NonLinCurvePtsIn[2] ), float.Parse( dp.NonLinCurvePtsOut[2] ) );
          }
          else {
            // dummy curve
            nonLinCurve.Curve( 0.25f, 0.25f, 0.5f, 0.5f, 0.75f, 0.75f );
          }
        }
      }

      // update the TuningParameters
      public void Update( ref DeviceTuningParameter dp )
      {
        if ( !used ) return;
        // don't return strings to control the device
        if ( dp.DeviceoptionRef == null ) {
          ; // nothing to update
        }
        else {
          dp.DeviceoptionRef.DeadzoneUsed = deadzoneUsed;
          dp.DeviceoptionRef.Deadzone = deadzoneS;
          dp.DeviceoptionRef.SaturationUsed = saturationUsed;
          dp.DeviceoptionRef.Saturation = saturationS;
        }
        dp.InvertUsed = invertUsed;
        dp.ExponentUsed = exponentUsed;
        dp.Exponent = exponentS;
        dp.NonLinCurveUsed = nonLinCurveUsed;
        List<string> pts = new List<string>( );
        pts.Add( nonLinCurve.Pt( 0 ).X.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 1 ).X.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 2 ).X.ToString( "0.000" ) );
        dp.NonLinCurvePtsIn = pts;
        pts = new List<string>( );
        pts.Add( nonLinCurve.Pt( 0 ).Y.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 1 ).Y.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 2 ).Y.ToString( "0.000" ) );
        dp.NonLinCurvePtsOut = pts;
      }

      // Context
      public bool used = false;
      public string nodetext = "";  // the node text
      public string control = ""; // the device control item e.g.  js2_x
      public string command = ""; // the control item used to get the XDevice Input

      // calc values
      private double MAX_DZ = 160.0; // avoid range issues and silly values..
      private double MIN_SAT = 200.0; // avoid range issues and silly values..
      private double m_range = 1000.0;
      private double m_sign = 1.0;

      /// <summary>
      /// Scales the input according to Deadzone and Saturation
      /// </summary>
      /// <param name="devIn">Int device input -1000 .. 0..1000</param>
      /// <returns>A double in the range -1.0 .. 0.0 .. 1.0</returns>
      public double ScaledOut( int devIn )
      {
        int di = Math.Abs( devIn );
        if ( di <= m_deadzone ) return 0.0;
        if ( di >= m_saturation ) return 1.0 * Math.Sign( devIn );

        double fout = ( di - m_deadzone ) / m_range; // 0 .. 1.0
        if ( exponentUsed ) {
          fout = Math.Pow( fout, exponent ) * Math.Sign( devIn );
        }
        else if ( nonLinCurveUsed ) {
          fout = nonLinCurve.EvalX( (int)( fout * 1000.0 * Math.Sign( devIn ) ) ); // gets a scaled signed value (-1 .. +1)
        }
        else {
          fout = fout * Math.Sign( devIn );
        }

        fout = ( fout > 1.0 ) ? 1.0 : fout;   // safeguard against any overshoots
        fout = ( fout < -1.0 ) ? -1.0 : fout; // safeguard against any overshoots
        return fout;
      }
      /// <summary>
      /// Applies the inversion if needed
      /// </summary>
      /// <param name="devOut">The new DevOut float 0.0 .. 1000.0</param>
      /// <returns>Applied inverted DevOut</returns>
      public double InvertedSign
      {
        get { return m_sign; }
      }

      // set values
      public bool m_invertUsed = false;
      public bool invertUsed { get { return m_invertUsed; } set { m_invertUsed = value; m_sign = m_invertUsed ? -1.0 : 1.0; } }

      public bool deadzoneUsed = false;
      private double m_deadzone = 0.0; // stores 1000 * set value
      public double deadzone { get { return m_deadzone; } set { m_deadzone = ( value > MAX_DZ ) ? MAX_DZ : value; m_range = m_saturation - m_deadzone; } }
      public string deadzoneS // get/set game value 0..1.000
      {
        get { return ( deadzone / 1000.0 ).ToString( "0.000" ); }
        set {
          double f;
          if ( double.TryParse( value, out f ) ) {
            deadzone = f * 1000.0;
          }
          else {
            deadzone = 0.0;
          }
        }
      }

      public bool saturationUsed = false;
      private double m_saturation = 1000.0;// stores 1000 * set value
      public double saturation { get { return m_saturation; } set { m_saturation = ( value < MIN_SAT ) ? MIN_SAT : value; m_range = m_saturation - m_deadzone; } }
      public string saturationS // get/set game value 0..1.000
      {
        get { return ( m_saturation / 1000.0 ).ToString( "0.000" ); }
        set {
          double f;
          if ( double.TryParse( value, out f ) ) {
            saturation = f * 1000.0;
          }
          else {
            saturation = 1000.0;
          }
        }
      }

      public bool exponentUsed = false;
      public double exponent = 1.0F;
      public string exponentS
      {
        get { return exponent.ToString( "0.000" ); }
        set {
          double f;
          if ( double.TryParse( value, out f ) ) {
            exponent = f;
          }
          else {
            exponent = 1.0;
          }
        }
      }

      public bool nonLinCurveUsed = false;
      public xyPoints nonLinCurve = new xyPoints( 1000 );  // max val of Joystick Input
    } // class LiveValues

    #endregion



    private Tuningoptions m_tuningOptions = null; // will get the current optiontree on call
    public Tuningoptions TuningOptions
    {
      get {
        return m_tuningOptions;
      }
      set {
        m_tuningOptions = value;
        if ( m_tuningOptions == null ) {
          log.Error( "- TuningOptions: m_tuningRef not assigned" );
          return;
        }
        YawTuning = m_tuningOptions.FirstTuningItem( "flight_move_yaw" );
        PitchTuning = m_tuningOptions.FirstTuningItem( "flight_move_pitch" );
        RollTuning = m_tuningOptions.FirstTuningItem( "flight_move_roll" );

        StrafeLatTuning = m_tuningOptions.FirstTuningItem( "flight_move_strafe_lateral" );
        StrafeVertTuning = m_tuningOptions.FirstTuningItem( "flight_move_strafe_vertical" );
        StrafeLonTuning = m_tuningOptions.FirstTuningItem( "flight_move_strafe_longitudinal" );
      }
    }

    // Generic Interaction Pattern
    // 
    // Assign TuningParameter (load Live data from Parameter)
    // Switch YPR - Strafe   (load live values into GUI)
    // Switch Axis           (load live values into working area)
    // Change Parameters     (live values are changed, GUI is updated)
    // Retrieve TuningParameter (update Parameter from Live data
    //
    #region YAW - Interaction

    private DeviceTuningParameter m_YawTuning = new DeviceTuningParameter( "flight_move_yaw" );
    private LiveValues m_liveYaw = new LiveValues( ); // live values

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    private DeviceTuningParameter YawTuning
    {
      get {
        YawUpdateTuning( );
        return m_YawTuning;
      }
      set {
        log.Info( "FormJSCalCurve : Yaw Command is: " + value );
        m_YawTuning = value;
        // update live values from input
        m_liveYaw.Load( m_YawTuning );
        // populate from input
        //YawUpdateGUIFromLiveValues( );
      }
    }

    // update the GUI from Live
    private void YawUpdateGUIFromLiveValues()
    {
      // updated the working area with Tuning parameters (obey live values)
      LiveValues lv = m_liveYaw;

      if ( !lv.used ) {
        pnlYaw.Visible = false; rbY.Visible = false;
        return;
      }
      pnlYaw.Visible = true; rbY.Visible = true;

      lblYCmd.Text = lv.control;
      lblYnt.Text = lv.nodetext;
      cbxYinvert.Checked = lv.invertUsed;
      lblYdeadzone.Text = lv.deadzoneS;
      cbxYdeadzone.Checked = lv.deadzoneUsed;
      lblYsat.Text = lv.saturationS;
      cbxYsat.Checked = lv.saturationUsed;
      lblYexponent.Text = lv.exponentS;
      cbxYexpo.Checked = lv.exponentUsed;
      lblYin1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblYout1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
      lblYin2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblYout2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
      lblYin3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblYout3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      cbxYpts.Checked = lv.nonLinCurveUsed;

      rbY.Checked = false; rbY.Checked = true;
    }

    // update Tuning from Live values
    private void YawUpdateTuning()
    {
      m_liveYaw.Update( ref m_YawTuning ); // update from live values
    }

    #endregion


    #region PITCH - Interaction

    private DeviceTuningParameter m_PitchTuning = new DeviceTuningParameter( "flight_move_pitch" );
    private LiveValues m_livePitch = new LiveValues( ); // live values

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    private DeviceTuningParameter PitchTuning
    {
      get {
        // update 
        PitchUpdateTuning( );
        return m_PitchTuning;
      }
      set {
        log.Info( "FormJSCalCurve : Pitch Command is: " + value );
        m_PitchTuning = value;
        // update live values from input
        m_livePitch.Load( m_PitchTuning );
        // populate from input
        //PitchUpdateGUIFromLiveValues( );
      }
    }

    // update the GUI from Live
    private void PitchUpdateGUIFromLiveValues()
    {
      // updated the working area with Tuning parameters (obey live values)
      LiveValues lv = m_livePitch;

      if ( !lv.used ) {
        pnlPitch.Visible = false; rbP.Visible = false;
        return;
      }
      pnlPitch.Visible = true; rbP.Visible = true;

      lblPCmd.Text = lv.control;
      lblPnt.Text = lv.nodetext;
      cbxPinvert.Checked = lv.invertUsed;
      lblPdeadzone.Text = lv.deadzoneS;
      cbxPdeadzone.Checked = lv.deadzoneUsed;
      lblPsat.Text = lv.saturationS;
      cbxPsat.Checked = lv.saturationUsed;
      lblPexponent.Text = lv.exponentS;
      cbxPexpo.Checked = lv.exponentUsed;
      lblPin1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblPout1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
      lblPin2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblPout2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
      lblPin3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblPout3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      cbxPpts.Checked = lv.nonLinCurveUsed;

      rbP.Checked = false; rbP.Checked = true;
    }

    private void PitchUpdateTuning()
    {
      m_livePitch.Update( ref m_PitchTuning ); // update from live values
    }

    #endregion


    #region ROLL - Interaction

    private DeviceTuningParameter m_RollTuning = new DeviceTuningParameter( "flight_move_roll" );
    private LiveValues m_liveRoll = new LiveValues( ); // live values

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    private DeviceTuningParameter RollTuning
    {
      get {
        // update 
        RollUpdateTuning( );
        return m_RollTuning;
      }
      set {
        log.Info( "FormJSCalCurve : Roll Command is: " + value );
        m_RollTuning = value;
        // update live values from input
        m_liveRoll.Load( m_RollTuning );
        // populate from input
        //RollUpdateGUIFromLiveValues( );
      }
    }

    // update the GUI from Live
    private void RollUpdateGUIFromLiveValues()
    {
      // updated the working area with Tuning parameters (obey live values)
      LiveValues lv = m_liveRoll;

      if ( !lv.used ) {
        pnlRoll.Visible = false; rbR.Visible = false;

        return;
      }
      pnlRoll.Visible = true; rbR.Visible = true;

      lblRCmd.Text = lv.control;
      lblRnt.Text = lv.nodetext;
      cbxRinvert.Checked = lv.invertUsed;
      lblRdeadzone.Text = lv.deadzoneS;
      cbxRdeadzone.Checked = lv.deadzoneUsed;
      lblRsat.Text = lv.saturationS;
      cbxRsat.Checked = lv.saturationUsed;
      lblRexponent.Text = lv.exponentS;
      cbxRexpo.Checked = lv.exponentUsed;
      lblRin1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblRout1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
      lblRin2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblRout2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
      lblRin3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblRout3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      cbxRpts.Checked = lv.nonLinCurveUsed;

      rbR.Checked = false; rbR.Checked = true;
    }

    private void RollUpdateTuning()
    {
      m_liveRoll.Update( ref m_RollTuning ); // update from live values
    }


    #endregion


    #region Strafe Lateral - Interaction (yaw GUI values)

    private DeviceTuningParameter m_StrafeLatTuning = new DeviceTuningParameter( "flight_move_strafe_lateral" );
    private LiveValues m_liveStrafeLat = new LiveValues( ); // live values

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    private DeviceTuningParameter StrafeLatTuning
    {
      get {
        // update 
        StrafeLatUpdateTuning( );
        return m_StrafeLatTuning;
      }
      set {
        log.Info( "FormJSCalCurve : Strafe Lateral Command is: " + value );
        m_StrafeLatTuning = value;
        // update live values from input
        m_liveStrafeLat.Load( m_StrafeLatTuning );
        // populate from input
        //StrafeLatUpdateGUIFromLiveValues( );
      }
    }

    // update the GUI from Live
    private void StrafeLatUpdateGUIFromLiveValues()
    {
      // updated the working area with Tuning parameters (obey live values)
      LiveValues lv = m_liveStrafeLat;

      if ( !lv.used ) {
        pnlYaw.Visible = false; rbY.Visible = false;

        return;
      }
      pnlYaw.Visible = true; rbY.Visible = true;

      lblYCmd.Text = lv.control;
      lblYnt.Text = lv.nodetext;
      cbxYinvert.Checked = lv.invertUsed;
      lblYdeadzone.Text = lv.deadzoneS;
      cbxYdeadzone.Checked = lv.deadzoneUsed;
      lblYsat.Text = lv.saturationS;
      cbxYsat.Checked = lv.saturationUsed;
      lblYexponent.Text = lv.exponentS;
      cbxYexpo.Checked = lv.exponentUsed;
      lblYin1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblYout1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
      lblYin2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblYout2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
      lblYin3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblYout3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      cbxYpts.Checked = lv.nonLinCurveUsed;

      rbY.Checked = false; rbY.Checked = true;
    }

    private void StrafeLatUpdateTuning()
    {
      m_liveStrafeLat.Update( ref m_StrafeLatTuning ); // update from live values
    }

    #endregion


    #region Strafe Vertical - Interaction (pitch GUI values)

    private DeviceTuningParameter m_StrafeVertTuning = new DeviceTuningParameter( "flight_move_strafe_vertical" );
    private LiveValues m_liveStrafeVert = new LiveValues( ); // live values

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    private DeviceTuningParameter StrafeVertTuning
    {
      get {
        // update 
        StrafeVertUpdateTuning( );
        return m_StrafeVertTuning;
      }
      set {
        log.Info( "FormJSCalCurve : Strafe Vertical Command is: " + value );
        m_StrafeVertTuning = value;
        // update live values from input
        m_liveStrafeVert.Load( m_StrafeVertTuning );
        // populate from input
        //StrafeVertUpdateGUIFromLiveValues( );
      }
    }

    // update the GUI from Live
    private void StrafeVertUpdateGUIFromLiveValues()
    {
      // updated the working area with Tuning parameters (obey live values)
      LiveValues lv = m_liveStrafeVert;

      if ( !lv.used ) {
        pnlPitch.Visible = false; rbP.Visible = false;

        return;
      }
      pnlPitch.Visible = true; rbP.Visible = true;

      lblPCmd.Text = lv.control;
      lblPnt.Text = lv.nodetext;
      cbxPinvert.Checked = lv.invertUsed;
      lblPdeadzone.Text = lv.deadzoneS;
      cbxPdeadzone.Checked = lv.deadzoneUsed;
      lblPsat.Text = lv.saturationS;
      cbxPsat.Checked = lv.saturationUsed;
      lblPexponent.Text = lv.exponentS;
      cbxPexpo.Checked = lv.exponentUsed;
      lblPin1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblPout1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
      lblPin2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblPout2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
      lblPin3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblPout3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      cbxPpts.Checked = lv.nonLinCurveUsed;

      rbP.Checked = false; rbP.Checked = true;
    }

    private void StrafeVertUpdateTuning()
    {
      m_liveStrafeVert.Update( ref m_StrafeVertTuning ); // update from live values
    }

    #endregion


    #region Strafe Longitudinal - Interaction (Roll GUI values)

    private DeviceTuningParameter m_StrafeLonTuning = new DeviceTuningParameter( "flight_move_strafe_longitudinal" );
    private LiveValues m_liveStrafeLon = new LiveValues( ); // live values

    /// <summary>
    /// Submit the tuning parameters
    /// </summary>
    /// 
    private DeviceTuningParameter StrafeLonTuning
    {
      get {
        // update 
        StrafeLonUpdateTuning( );
        return m_StrafeLonTuning;
      }
      set {
        log.Info( "FormJSCalCurve : Strafe Longitudinal Command is: " + value );
        m_StrafeLonTuning = value;
        // update live values from input
        m_liveStrafeLon.Load( m_StrafeLonTuning );
        // populate from input
        //StrafeLonUpdateGUIFromLiveValues( );
      }
    }

    // update the GUI from Live
    private void StrafeLonUpdateGUIFromLiveValues()
    {
      // updated the working area with Tuning parameters (obey live values)
      LiveValues lv = m_liveStrafeLon;

      if ( !lv.used ) {
        pnlRoll.Visible = false; rbR.Visible = false;

        return;
      }
      pnlRoll.Visible = true; rbR.Visible = true;

      lblRCmd.Text = lv.control;
      lblRnt.Text = lv.nodetext;
      cbxRinvert.Checked = lv.invertUsed;
      lblRdeadzone.Text = lv.deadzoneS;
      cbxRdeadzone.Checked = lv.deadzoneUsed;
      lblRsat.Text = lv.saturationS;
      cbxRsat.Checked = lv.saturationUsed;
      lblRexponent.Text = lv.exponentS;
      cbxRexpo.Checked = lv.exponentUsed;
      lblRin1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblRout1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
      lblRin2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblRout2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
      lblRin3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblRout3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      cbxRpts.Checked = lv.nonLinCurveUsed;

      rbR.Checked = false; rbR.Checked = true;
    }

    private void StrafeLonUpdateTuning()
    {
      m_liveStrafeLon.Update( ref m_StrafeLonTuning ); // update from live values
    }

    #endregion


    #region OGL Content

    private void LoadSkybox()
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
      } catch ( Exception ex ) {
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

        throw new NotSupportedException( string.Format( "This program requires OpenGL 2.0. Found {0}. Aborting.",
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
      string vertSource = @"
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
      string fragSource = @"
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



    private void SetupViewport()
    {
      int w = glControl1.Width;
      int h = glControl1.Height;

      GL.Viewport( 0, 0, w, h ); // Use all of the glControl painting area

      GL.MatrixMode( MatrixMode.Projection );
      Matrix4 p = Matrix4.CreatePerspectiveFieldOfView( MathHelper.PiOver4, w / (float)h, 0.1f, 10.0f );
      GL.LoadMatrix( ref p );

      GL.MatrixMode( MatrixMode.Modelview );
      GL.LoadIdentity( );
    }

    // One render cycle - beware this should be fast...
    private void Render()
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
    /// Sub Handler for Strafe
    /// </summary>
    Vector3d Idle_Strafe()
    {
      Vector3d m = Vector3d.Zero; ;

      bool lat = ( m_StrafeLatTuning != null ) && ( m_StrafeLatTuning.GameDevice != null );
      bool vert = ( m_StrafeVertTuning != null ) && ( m_StrafeVertTuning.GameDevice != null );
      bool lon = ( m_StrafeLonTuning != null ) && ( m_StrafeLonTuning.GameDevice != null );

      int i_x = 0, i_y = 0, i_z = 0; // Joystick Input
      int x = 0; int y = 0; int z = 0; // retain real input as i_xyz

      if ( lat ) m_StrafeLatTuning.GameDevice.GetCmdData( m_liveStrafeLat.command, out i_x ); // + = right
      if ( vert ) m_StrafeVertTuning.GameDevice.GetCmdData( m_liveStrafeVert.command, out i_y ); // + = up
      if ( lon ) m_StrafeLonTuning.GameDevice.GetCmdData( m_liveStrafeLon.command, out i_z ); // += twist right
      // apply the modifications of the control (deadzone, shape, sensitivity)
      x = i_x; y = i_y; z = i_z; // retain real input as i_xyz
      m_flightModel.Velocity = Vector3d.Zero;

      // Lateral
      if ( lat ) {
        double fout = m_liveStrafeLat.ScaledOut( x ); // 0 .. 1000.0
        lblYInput.Text = ( i_x / 1000.0 ).ToString( "0.00" ); lblYOutput.Text = ( fout ).ToString( "0.00" );
        // calculate new direction vector
        m.X = m_liveStrafeLat.InvertedSign * ( ( !cbYuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
      }

      // Vertical
      if ( vert ) {
        double fout = m_liveStrafeVert.ScaledOut( y ); // 0 .. 1000.0
        lblPInput.Text = ( i_y / 1000.0 ).ToString( "0.00" ); lblPOutput.Text = ( fout ).ToString( "0.00" );
        // calculate new direction vector
        m.Y = m_liveStrafeVert.InvertedSign * ( ( !cbPuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
      }

      // Longitudinal
      if ( lon ) {
        double fout = m_liveStrafeLon.ScaledOut( z ); // 0 .. 1000.0
        lblRInput.Text = ( i_z / 1000.0 ).ToString( "0.00" ); lblROutput.Text = ( fout ).ToString( "0.00" );
        // calculate new direction vector
        m.Z = m_liveStrafeLon.InvertedSign * ( ( !cbRuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
      }
      return m;
    }


    /// <summary>
    /// Sub Handler for YPR (Yaw, Pitch, Roll)
    /// </summary>
    Vector3d Idle_YPR()
    {
      Vector3d m = Vector3d.Zero; ;

      bool yaw = ( m_YawTuning != null ) && ( m_YawTuning.GameDevice != null );
      bool pitch = ( m_PitchTuning != null ) && ( m_PitchTuning.GameDevice != null );
      bool roll = ( m_RollTuning != null ) && ( m_RollTuning.GameDevice != null );

      int i_x = 0, i_y = 0, i_z = 0; // Joystick Input
      int x = 0; int y = 0; int z = 0; // retain real input as i_xyz

      if ( yaw ) m_YawTuning.GameDevice.GetCmdData( m_liveYaw.command, out i_x ); // + = right
      if ( pitch ) m_PitchTuning.GameDevice.GetCmdData( m_livePitch.command, out i_y ); // + = up
      if ( roll ) m_RollTuning.GameDevice.GetCmdData( m_liveRoll.command, out i_z ); // += twist right

      // apply the modifications of the control (deadzone, shape, sensitivity)
      x = i_x; y = i_y; z = i_z; // retain real input as i_xyz
      m_flightModel.Velocity = Vector3d.Zero;

      // Yaw
      if ( yaw ) {
        double fout = m_liveYaw.ScaledOut( x ); // 0 .. 1000.0
        lblYInput.Text = ( i_x / 1000.0 ).ToString( "0.00" ); lblYOutput.Text = ( fout ).ToString( "0.00" );
        // calculate new direction vector
        m.X = m_liveYaw.InvertedSign * ( ( !cbYuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
      }

      // Pitch
      if ( pitch ) {
        double fout = m_livePitch.ScaledOut( y ); // 0 .. 1000.0
        lblPInput.Text = ( i_y / 1000.0 ).ToString( "0.00" ); lblPOutput.Text = ( fout ).ToString( "0.00" );
        // calculate new direction vector
        m.Y = m_livePitch.InvertedSign * ( ( !cbPuse.Checked ) ? -fout : 0 ) * m_msElapsed * DegPerMS; // 20170801: fix - Must use inverted out value
      }

      // Roll
      if ( roll ) {
        double fout = m_liveRoll.ScaledOut( z ); // 0 .. 1000.0
        lblRInput.Text = ( i_z / 1000.0 ).ToString( "0.00" ); lblROutput.Text = ( fout ).ToString( "0.00" );
        // calculate new direction vector
        m.Z = m_liveRoll.InvertedSign * ( ( !cbRuse.Checked ) ? fout : 0 ) * m_msElapsed * DegPerMS;
      }

      return m;
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
        if ( m_msElapsed < m_frameTime ) {
          // lblDebug.Text = "F";
          continue; //pace updates the max frametime allowed
        }
        //lblDebug.Text = "R";

        // safeguard against locking (moving the window) so the integrator does not get crazy..
        // if deltatime gets too big we fake a regular cycle for this round
        if ( m_msElapsed > 200 ) m_msElapsed = m_frameTime;

        m_ticks = newTick; // prep next run

        // query the Joysticks for the 3 controls and fill the flight model vector
        if ( m_isStrafe ) {
          m = Idle_Strafe( );
        }
        else {
          m = Idle_YPR( );
        }

        // finalize
        m_flightModel.Velocity -= m; // new direction change vector
        Vector3d deltaAngleV = m_flightModel.Integrate( (double)m_msElapsed / 1000.0, m_damping, 85.0 ); // heuristic K and B ..

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
      slTurnSpeed.Value = 8; // turns in 4 seconds 360deg
    }

    private void rbHornet_CheckedChanged( object sender, EventArgs e )
    {
      slDamping.Value = 6;
      slTurnSpeed.Value = 12;
    }

    private void slTurnSpeed_ValueChanged( object sender, EventArgs e )
    {
      // recalc the turning scale based on one full 360 deg sweep in the given time (sec)
      DegPerMS = 360.0 / ( slTurnSpeed.Value * 500.0 ); // slider is 0.5 sec steps
      lblTurnspeed.Text = ( slTurnSpeed.Value / 2.0 ).ToString( );
    }

    private void slDamping_ValueChanged( object sender, EventArgs e )
    {
      m_damping = ( slDamping.Maximum - slDamping.Value + 1 ) * 100.0; // 100 .. 1000
      lblDamping.Text = slDamping.Value.ToString( );
    }

    #endregion

    #region Tune Kind Changed

    private void rbTuneYPR_CheckedChanged( object sender, EventArgs e )
    {
      if ( rbTuneYPR.Checked ) {
        m_isStrafe = false;
        lblYaw.Text = "Yaw"; lblLiveYaw.Text = "Yaw"; rbY.Text = "Yaw -->";
        lblPitch.Text = "Pitch"; lblLivePitch.Text = "Pitch"; rbP.Text = "Pitch -->";
        lblRoll.Text = "Roll"; lblLiveRoll.Text = "Roll"; rbR.Text = "Roll -->";
        tlpData.BackColor = rbTuneYPR.BackColor;
        pnlAxisSelector.BackColor = rbTuneYPR.BackColor;
        RollUpdateGUIFromLiveValues( );
        PitchUpdateGUIFromLiveValues( );
        YawUpdateGUIFromLiveValues( );
      }
    }

    private void rbTuneStrafe_CheckedChanged( object sender, EventArgs e )
    {
      if ( rbTuneStrafe.Checked ) {
        m_isStrafe = true;
        lblYaw.Text = "Lat"; lblLiveYaw.Text = "Lateral"; rbY.Text = "Lateral -->";
        lblPitch.Text = "Vert"; lblLivePitch.Text = "Vertical"; rbP.Text = "Vertical -->";
        lblRoll.Text = "Lon"; lblLiveRoll.Text = "Long."; rbR.Text = "Longitudinal -->";
        tlpData.BackColor = rbTuneStrafe.BackColor;
        pnlAxisSelector.BackColor = rbTuneStrafe.BackColor;
        StrafeLonUpdateGUIFromLiveValues( );
        StrafeVertUpdateGUIFromLiveValues( );
        StrafeLatUpdateGUIFromLiveValues( );
      }
    }

    #endregion

    #region Active Axis Changed -  - copy data from left labels into the working area

    /// <summary>
    /// Make Yaw Active - copy data from left labels into the working area
    /// </summary>
    private void rbY_CheckedChanged()
    {
      if ( rbY.Checked == true ) {
        // get Labels from left area (current)
        lblIn[1].Text = lblYin1.Text; lblIn[2].Text = lblYin2.Text; lblIn[3].Text = lblYin3.Text;
        lblOut[1].Text = lblYout1.Text; lblOut[2].Text = lblYout2.Text; lblOut[3].Text = lblYout3.Text;
        lblOut[4].Text = lblYexponent.Text;
        lblNodetext.Text = lblYnt.Text;
        // setup chart along the choosen parameter
        rbPtDeadzone.Enabled = cbxYdeadzone.Checked;
        rbPtSaturation.Enabled = cbxYsat.Checked;

        rbPtDeadzone.Checked = true;
        if ( rbPtDeadzone.Enabled ) {
          tbSlider.Value = Deviceoptions.DeadzoneToSlider( lblYdeadzone.Text );
        }
        else {
          rbPtSaturation.Checked = true;
          if ( rbPtSaturation.Enabled ) {
            tbSlider.Value = Deviceoptions.SaturationToSlider( lblYsat.Text );
          }
        }
        UpdateChartItems( );
      }
    }
    private void rbY_CheckedChanged( object sender, EventArgs e )
    {
      rbY_CheckedChanged( );
    }
    private void rbY_CheckUpdate()
    {
      rbY.Checked = false; rbY.Checked = true; // forces to check and to update in case of already checked
    }


    /// <summary>
    /// Make Pitch Active - copy data from left labels into the working area
    /// </summary>
    private void rbP_CheckedChanged()
    {
      if ( rbP.Checked == true ) {
        // get Labels from left area (current)
        lblIn[1].Text = lblPin1.Text; lblIn[2].Text = lblPin2.Text; lblIn[3].Text = lblPin3.Text;
        lblOut[1].Text = lblPout1.Text; lblOut[2].Text = lblPout2.Text; lblOut[3].Text = lblPout3.Text;
        lblOut[4].Text = lblPexponent.Text;
        lblNodetext.Text = lblPnt.Text;
        // setup chart along the choosen parameter
        rbPtDeadzone.Enabled = cbxPdeadzone.Checked;
        rbPtSaturation.Enabled = cbxPsat.Checked;

        rbPtDeadzone.Checked = true;
        if ( rbPtDeadzone.Enabled ) {
          tbSlider.Value = Deviceoptions.DeadzoneToSlider( lblPdeadzone.Text );
        }
        else {
          rbPtSaturation.Checked = true; // force back to sense (available for both..)
          if ( rbPtSaturation.Enabled ) {
            tbSlider.Value = Deviceoptions.SaturationToSlider( lblPsat.Text );
          }
        }
        UpdateChartItems( );
      }
    }
    private void rbP_CheckedChanged( object sender, EventArgs e )
    {
      rbP_CheckedChanged( );
    }
    private void rbP_CheckUpdate()
    {
      rbP.Checked = false; rbP.Checked = true; // forces to check and to update in case of already checked
    }

    /// <summary>
    /// Make Roll Active - copy data from left labels into the working area
    /// </summary>
    private void rbR_CheckedChanged()
    {
      if ( rbR.Checked == true ) {
        // get Labels from left area (current)
        lblIn[1].Text = lblRin1.Text; lblIn[2].Text = lblRin2.Text; lblIn[3].Text = lblRin3.Text;
        lblOut[1].Text = lblRout1.Text; lblOut[2].Text = lblRout2.Text; lblOut[3].Text = lblRout3.Text;
        lblOut[4].Text = lblRexponent.Text;
        lblNodetext.Text = lblRnt.Text;
        // setup chart along the choosen parameter
        rbPtDeadzone.Enabled = cbxRdeadzone.Checked;
        rbPtSaturation.Enabled = cbxRsat.Checked;

        rbPtDeadzone.Checked = true;
        if ( rbPtDeadzone.Enabled ) {
          tbSlider.Value = Deviceoptions.DeadzoneToSlider( lblRdeadzone.Text );
        }
        else {
          rbPtSaturation.Checked = true; // force back to sense (available for both..)
          if ( rbPtSaturation.Enabled ) {
            tbSlider.Value = Deviceoptions.SaturationToSlider( lblRsat.Text );
          }
        }
        UpdateChartItems( );
      }
    }
    private void rbR_CheckedChanged( object sender, EventArgs e )
    {
      rbR_CheckedChanged( );
    }
    private void rbR_CheckUpdate()
    {
      rbR.Checked = false; rbR.Checked = true; // forces to check and to update in case of already checked
    }

    // Interaction helper - changes panel on mouse up

    private void pnlYaw_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      rbY_CheckUpdate( );
    }

    private void pnlPitch_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      rbP_CheckUpdate( );
    }

    private void pnlRoll_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      rbR_CheckUpdate( );
    }

    #endregion

    #region Charts section

    // Chart - move Pts

    /// <summary>
    /// Evaluate which tune parameter has the chart input
    /// </summary>
    private void EvalChartInput()
    {
      m_hitPt = 0;
      if ( rbPt1.Enabled && rbPt1.Checked ) m_hitPt = 1;
      if ( rbPt2.Enabled && rbPt2.Checked ) m_hitPt = 2;
      if ( rbPt3.Enabled && rbPt3.Checked ) m_hitPt = 3;
      if ( rbPtExponent.Enabled && rbPtExponent.Checked ) m_hitPt = 4;

      if ( m_hitPt > 0 ) return;

      // slider fudge
      tbSlider.Enabled = false;
      if ( rbPtDeadzone.Enabled && rbPtDeadzone.Checked ) {
        tbSlider.Enabled = true;
        if ( rbY.Checked ) {
          tbSlider.Value = Deviceoptions.DeadzoneToSlider( lblYdeadzone.Text );
        }
        else if ( rbP.Checked ) {
          tbSlider.Value = Deviceoptions.DeadzoneToSlider( lblPdeadzone.Text );
        }
        else if ( rbR.Checked ) {
          tbSlider.Value = Deviceoptions.DeadzoneToSlider( lblRdeadzone.Text );
        }
      }
      if ( rbPtSaturation.Enabled && rbPtSaturation.Checked ) {
        tbSlider.Enabled = true;
        if ( rbY.Checked ) {
          tbSlider.Value = Deviceoptions.SaturationToSlider( lblYsat.Text );
        }
        else if ( rbP.Checked ) {
          tbSlider.Value = Deviceoptions.SaturationToSlider( lblPsat.Text );
        }
        else if ( rbR.Checked ) {
          tbSlider.Value = Deviceoptions.SaturationToSlider( lblRsat.Text );
        }
      }
      EvalSlider( );
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
    private void UpdateChartItems()
    {
      bool deadzoneUsed = true;
      bool satUsed = true;
      bool expUsed = true;
      bool ptsUsed = true;
      // see what is on display..
      if ( rbY.Checked == true ) {
        // Yaw
        if ( m_isStrafe ) {
          deadzoneUsed = ( m_liveStrafeLat.deadzoneUsed == true );
          satUsed = ( m_liveStrafeLat.saturationUsed == true );
          expUsed = ( m_liveStrafeLat.exponentUsed == true );
          ptsUsed = ( m_liveStrafeLat.nonLinCurveUsed == true );
          lblGraphDeadzone.Text = m_liveStrafeLat.deadzoneS;
          lblGraphSaturation.Text = m_liveStrafeLat.saturationS;

        }
        else {
          deadzoneUsed = ( m_liveYaw.deadzoneUsed == true );
          satUsed = ( m_liveYaw.saturationUsed == true );
          expUsed = ( m_liveYaw.exponentUsed == true );
          ptsUsed = ( m_liveYaw.nonLinCurveUsed == true );
          lblGraphDeadzone.Text = m_liveYaw.deadzoneS;
          lblGraphSaturation.Text = m_liveYaw.saturationS;
        }
        chart1.BackColor = rbY.BackColor;

      }
      else if ( rbP.Checked == true ) {
        // Pitch
        if ( m_isStrafe ) {
          deadzoneUsed = ( m_liveStrafeVert.deadzoneUsed == true );
          satUsed = ( m_liveStrafeVert.saturationUsed == true );
          expUsed = ( m_liveStrafeVert.exponentUsed == true );
          ptsUsed = ( m_liveStrafeVert.nonLinCurveUsed == true );
          lblGraphDeadzone.Text = m_liveStrafeVert.deadzoneS;
          lblGraphSaturation.Text = m_liveStrafeVert.saturationS;
        }
        else {
          deadzoneUsed = ( m_livePitch.deadzoneUsed == true );
          satUsed = ( m_livePitch.saturationUsed == true );
          expUsed = ( m_livePitch.exponentUsed == true );
          ptsUsed = ( m_livePitch.nonLinCurveUsed == true );
          lblGraphDeadzone.Text = m_livePitch.deadzoneS;
          lblGraphSaturation.Text = m_livePitch.saturationS;
        }
        chart1.BackColor = rbP.BackColor;

      }
      else {
        // Roll
        if ( m_isStrafe ) {
          deadzoneUsed = ( m_liveStrafeLon.deadzoneUsed == true );
          satUsed = ( m_liveStrafeLon.saturationUsed == true );
          expUsed = ( m_liveStrafeLon.exponentUsed == true );
          ptsUsed = ( m_liveStrafeLon.nonLinCurveUsed == true );
          lblGraphDeadzone.Text = m_liveStrafeLon.deadzoneS;
          lblGraphSaturation.Text = m_liveStrafeLon.saturationS;
        }
        else {
          deadzoneUsed = ( m_liveRoll.deadzoneUsed == true );
          satUsed = ( m_liveRoll.saturationUsed == true );
          expUsed = ( m_liveRoll.exponentUsed == true );
          ptsUsed = ( m_liveRoll.nonLinCurveUsed == true );
          lblGraphDeadzone.Text = m_liveRoll.deadzoneS;
          lblGraphSaturation.Text = m_liveRoll.saturationS;
        }
        chart1.BackColor = rbR.BackColor;
      }

      // generic part
      rbPtDeadzone.Enabled = deadzoneUsed;
      lblGraphDeadzone.Visible = deadzoneUsed;

      rbPtSaturation.Enabled = satUsed;
      lblGraphSaturation.Visible = satUsed;

      rbPtExponent.Enabled = expUsed;
      rbPt1.Enabled = ptsUsed; rbPt2.Enabled = ptsUsed; rbPt3.Enabled = ptsUsed;
      EvalChartInput( );  // review active chart input

      if ( !tbSlider.Enabled ) lblOutSlider.Text = "---";

      if ( expUsed ) {
        // Exp mode
        double expo = double.Parse( lblOut[4].Text );
        // dont touch zero Point
        m_bSeries.BezierPoints[1].SetValueXY( 0.25, Math.Pow( 0.25, expo ) );
        m_bSeries.BezierPoints[2].SetValueXY( 0.5, Math.Pow( 0.5, expo ) );
        m_bSeries.BezierPoints[3].SetValueXY( 0.75, Math.Pow( 0.75, expo ) );
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, 1.0 );

      }
      else if ( ptsUsed ) {
        // Pts mode
        // dont touch zero Point
        for ( int i = 1; i <= 3; i++ ) {
          m_bSeries.BezierPoints[i].SetValueXY( float.Parse( lblIn[i].Text ), float.Parse( lblOut[i].Text ) );
        }
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, 1.0 );

      }
      else {
        // linear
        // dont touch zero Point
        m_bSeries.BezierPoints[1].SetValueXY( 0.25, 0.25 );
        m_bSeries.BezierPoints[2].SetValueXY( 0.5, 0.5 );
        m_bSeries.BezierPoints[3].SetValueXY( 0.75, 0.75 );
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, 1.0 );
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
          m_bSeries.BezierPoints[m_hitPt].SetValueXY( newX, newY );
          // update markers from curve points
          chart1.Series[1].Points[m_hitPt] = m_bSeries.BezierPoints[m_hitPt];

        }
        else if ( m_hitPt == 4 ) {
          // Exponent
          double newY = double.Parse( lblOut[m_hitPt].Text ) + ( e.Y - mY ) * 0.01f; mY = e.Y;
          newY = ( newY > 3.0f ) ? 3.0f : newY;
          newY = ( newY < 0.5f ) ? 0.5f : newY;
          lblOut[m_hitPt].Text = newY.ToString( "0.00" );

          // update chart (Points[0] is zero)
          m_bSeries.BezierPoints[1].SetValueXY( 0.25, Math.Pow( 0.25, newY ) );
          m_bSeries.BezierPoints[2].SetValueXY( 0.5, Math.Pow( 0.5, newY ) );
          m_bSeries.BezierPoints[3].SetValueXY( 0.75, Math.Pow( 0.75, newY ) );
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
        lblYexponent.Text = lblOut[4].Text;
        // update live values
        if ( m_isStrafe ) {
          m_liveStrafeLat.exponentS = lblYexponent.Text;
          if ( m_liveStrafeLat.nonLinCurve != null ) {
            m_liveStrafeLat.nonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                  float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                  float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
          }
        }
        else {
          m_liveYaw.exponentS = lblYexponent.Text;
          if ( m_liveYaw.nonLinCurve != null ) {
            m_liveYaw.nonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                  float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                  float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
          }
        }

      }
      else if ( rbP.Checked == true ) {
        // left area labels
        lblPin1.Text = lblIn[1].Text; lblPin2.Text = lblIn[2].Text; lblPin3.Text = lblIn[3].Text;
        lblPout1.Text = lblOut[1].Text; lblPout2.Text = lblOut[2].Text; lblPout3.Text = lblOut[3].Text;
        lblPexponent.Text = lblOut[4].Text;
        // update live values
        if ( m_isStrafe ) {
          m_liveStrafeVert.exponentS = lblPexponent.Text;
          if ( m_liveStrafeVert.nonLinCurve != null ) {
            m_liveStrafeVert.nonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                  float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                  float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
          }
        }
        else {
          m_livePitch.exponentS = lblPexponent.Text;
          if ( m_livePitch.nonLinCurve != null ) {
            m_livePitch.nonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                  float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                  float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
          }
        }

      }
      else if ( rbR.Checked == true ) {
        // left area labels
        lblRin1.Text = lblIn[1].Text; lblRin2.Text = lblIn[2].Text; lblRin3.Text = lblIn[3].Text;
        lblRout1.Text = lblOut[1].Text; lblRout2.Text = lblOut[2].Text; lblRout3.Text = lblOut[3].Text;
        lblRexponent.Text = lblOut[4].Text;
        // update live values
        if ( m_isStrafe ) {
          m_liveStrafeLon.exponentS = lblRexponent.Text;
          if ( m_liveStrafeLon.nonLinCurve != null ) {
            m_liveStrafeLon.nonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                  float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                  float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
          }
        }
        else {
          m_liveRoll.exponentS = lblRexponent.Text;
          if ( m_liveRoll.nonLinCurve != null ) {
            m_liveRoll.nonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                  float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                  float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
          }
        }

      }
    }
    #endregion

    #region Checked Invert Changed

    private void cbxYinvert_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLat.invertUsed = false;
        if ( cbxYinvert.Checked == true ) {
          m_liveStrafeLat.invertUsed = true;
        }
      }
      else {
        m_liveYaw.invertUsed = false;
        if ( cbxYinvert.Checked == true ) {
          m_liveYaw.invertUsed = true;
        }
      }
      rbY_CheckUpdate( );
    }

    private void cbxPinvert_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeVert.invertUsed = false;
        if ( cbxPinvert.Checked == true ) {
          m_liveStrafeVert.invertUsed = true;
        }
      }
      else {
        m_livePitch.invertUsed = false;
        if ( cbxPinvert.Checked == true ) {
          m_livePitch.invertUsed = true;
        }
      }
      rbP_CheckUpdate( );
    }

    private void cbxRinvert_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLon.invertUsed = false;
        if ( cbxRinvert.Checked == true ) {
          m_liveStrafeLon.invertUsed = true;
        }
      }
      else {
        m_liveRoll.invertUsed = false;
        if ( cbxRinvert.Checked == true ) {
          m_liveRoll.invertUsed = true;
        }
      }
      rbR_CheckUpdate( );
    }

    #endregion

    #region Slider Value Changed (Deadzone / Saturation)

    // Deadzone slider   00 .. 40 -> 0 .. 0.160 ( 4 pt scale)
    // Saturation slider 00 .. 40 -> 0.2 .. 1.0 ( 20 pt scale)

    private void EvalSlider()
    {
      if ( rbPtDeadzone.Enabled && rbPtDeadzone.Checked ) {
        lblOutSlider.Text = Deviceoptions.DeadzoneFromSlider( tbSlider.Value ).ToString( "0.000" );
        float curDeadzone = 1000.0f * Deviceoptions.DeadzoneFromSlider( tbSlider.Value );  // % scaled to maxAxis

        if ( rbY.Checked == true ) {
          if ( m_isStrafe ) m_liveStrafeLat.deadzone = curDeadzone; else m_liveYaw.deadzone = curDeadzone;
          lblYdeadzone.Text = lblOutSlider.Text;
        }
        else if ( rbP.Checked == true ) {
          if ( m_isStrafe ) m_liveStrafeVert.deadzone = curDeadzone; else m_livePitch.deadzone = curDeadzone;
          lblPdeadzone.Text = lblOutSlider.Text;
        }
        else if ( rbR.Checked == true ) {
          if ( m_isStrafe ) m_liveStrafeLon.deadzone = curDeadzone; else m_liveRoll.deadzone = curDeadzone;
          lblRdeadzone.Text = lblOutSlider.Text;
        }
        lblGraphDeadzone.Text = lblOutSlider.Text;
      }
      else if ( rbPtSaturation.Enabled && rbPtSaturation.Checked ) {
        lblOutSlider.Text = Deviceoptions.SaturationFromSlider( tbSlider.Value ).ToString( "0.000" );
        float curSaturation = 1000.0f * Deviceoptions.SaturationFromSlider( tbSlider.Value );  // % scaled to maxAxis

        if ( rbY.Checked == true ) {
          if ( m_isStrafe ) m_liveStrafeLat.saturation = curSaturation; else m_liveYaw.saturation = curSaturation;
          lblYsat.Text = lblOutSlider.Text;
        }
        else if ( rbP.Checked == true ) {
          if ( m_isStrafe ) m_liveStrafeVert.saturation = curSaturation; else m_livePitch.saturation = curSaturation;
          lblPsat.Text = lblOutSlider.Text;
        }
        else if ( rbR.Checked == true ) {
          if ( m_isStrafe ) m_liveStrafeLon.saturation = curSaturation; else m_liveRoll.saturation = curSaturation;
          lblRsat.Text = lblOutSlider.Text;
        }
        lblGraphSaturation.Text = lblOutSlider.Text;
      }
    }

    private void tbSlider_ValueChanged( object sender, EventArgs e )
    {
      EvalSlider( );
    }

    #endregion

    #region Checked Deadzone Changed

    private void cbxYdeadzone_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLat.deadzoneUsed = false;
        if ( cbxYdeadzone.Checked == true ) {
          m_liveStrafeLat.deadzoneUsed = true;
        }
      }
      else {
        m_liveYaw.deadzoneUsed = false;
        if ( cbxYdeadzone.Checked == true ) {
          m_liveYaw.deadzoneUsed = true;
        }
      }
      rbY_CheckUpdate( );
      rbPtDeadzone.Checked = cbxYdeadzone.Checked;
      UpdateChartItems( );
    }

    private void cbxPdeadzone_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeVert.deadzoneUsed = false;
        if ( cbxPdeadzone.Checked == true ) {
          m_liveStrafeVert.deadzoneUsed = true;
        }
      }
      else {
        m_livePitch.deadzoneUsed = false;
        if ( cbxPdeadzone.Checked == true ) {
          m_livePitch.deadzoneUsed = true;
        }
      }
      rbP_CheckUpdate( );
      rbPtDeadzone.Checked = cbxPdeadzone.Checked;
      UpdateChartItems( );
    }

    private void cbxRdeadzone_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLon.deadzoneUsed = false;
        if ( cbxRdeadzone.Checked == true ) {
          m_liveStrafeLon.deadzoneUsed = true; // update storage
        }
      }
      else {
        m_liveRoll.deadzoneUsed = false;
        if ( cbxRdeadzone.Checked == true ) {
          m_liveRoll.deadzoneUsed = true; // update storage
        }
      }
      rbR_CheckUpdate( );
      rbPtDeadzone.Checked = cbxRdeadzone.Checked;
      UpdateChartItems( );
    }

    #endregion

    #region Checked Saturation Changed

    private void cbxYsense_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLat.saturationUsed = false;
        if ( cbxYsat.Checked == true ) {
          m_liveStrafeLat.saturationUsed = true;
        }
      }
      else {
        m_liveYaw.saturationUsed = false;
        if ( cbxYsat.Checked == true ) {
          m_liveYaw.saturationUsed = true;
        }
      }
      rbY_CheckUpdate( );
      rbPtSaturation.Checked = cbxYsat.Checked;
      UpdateChartItems( );
    }

    private void cbxPsense_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeVert.saturationUsed = false;
        if ( cbxPsat.Checked == true ) {
          m_liveStrafeVert.saturationUsed = true;
        }
      }
      else {
        m_livePitch.saturationUsed = false;
        if ( cbxPsat.Checked == true ) {
          m_livePitch.saturationUsed = true;
        }
      }
      rbP_CheckUpdate( );
      rbPtSaturation.Checked = cbxPsat.Checked;
      UpdateChartItems( );
    }

    private void cbxRsense_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLon.saturationUsed = false;
        if ( cbxRsat.Checked == true ) {
          m_liveStrafeLon.saturationUsed = true;
        }
      }
      else {
        m_liveRoll.saturationUsed = false;
        if ( cbxRsat.Checked == true ) {
          m_liveRoll.saturationUsed = true;
        }
      }
      rbR_CheckUpdate( );
      rbPtSaturation.Checked = cbxRsat.Checked;
      UpdateChartItems( );
    }

    #endregion

    #region Checked Exponent Changed

    private void cbxYexpo_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLat.exponentUsed = false;
        if ( cbxYexpo.Checked == true ) {
          m_liveStrafeLat.exponentUsed = true;
          cbxYpts.Checked = false;       // forced: either expo OR points
        }
      }
      else {
        m_liveYaw.exponentUsed = false;
        if ( cbxYexpo.Checked == true ) {
          m_liveYaw.exponentUsed = true;
          cbxYpts.Checked = false;       // forced: either expo OR points
        }
      }
      rbY_CheckUpdate( );
      UpdateChartItems( );
    }

    private void cbxPexpo_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeVert.exponentUsed = false;
        if ( cbxPexpo.Checked == true ) {
          m_liveStrafeVert.exponentUsed = true;
          cbxPpts.Checked = false;       // forced: either expo OR points
        }
      }
      else {
        m_livePitch.exponentUsed = false;
        if ( cbxPexpo.Checked == true ) {
          m_livePitch.exponentUsed = true;
          cbxPpts.Checked = false;       // forced: either expo OR points
        }
      }
      rbP_CheckUpdate( );
      UpdateChartItems( );
    }

    private void cbxRexpo_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLon.exponentUsed = false;
        if ( cbxRexpo.Checked == true ) {
          m_liveStrafeLon.exponentUsed = true;
          cbxRpts.Checked = false;       // forced: either expo OR points
        }
      }
      else {
        m_liveRoll.exponentUsed = false;
        if ( cbxRexpo.Checked == true ) {
          m_liveRoll.exponentUsed = true;
          cbxRpts.Checked = false;       // forced: either expo OR points
        }
      }
      rbR_CheckUpdate( );
      UpdateChartItems( );
    }

    #endregion

    #region Checked Points Changed

    private void cbxYpts_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLat.nonLinCurveUsed = false;
        if ( cbxYpts.Checked == true ) {
          m_liveStrafeLat.nonLinCurveUsed = true;
          cbxYexpo.Checked = false;       // forced: either expo OR points
        }
      }
      else {
        m_liveYaw.nonLinCurveUsed = false;
        if ( cbxYpts.Checked == true ) {
          m_liveYaw.nonLinCurveUsed = true;
          cbxYexpo.Checked = false;       // forced: either expo OR points
        }
      }
      rbY_CheckUpdate( );
      UpdateChartItems( );
    }

    private void cbxPpts_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeVert.nonLinCurveUsed = false;
        if ( cbxPpts.Checked == true ) {
          m_liveStrafeVert.nonLinCurveUsed = true;
          cbxPexpo.Checked = false;       // forced: either expo OR points
        }
      }
      else {
        m_livePitch.nonLinCurveUsed = false;
        if ( cbxPpts.Checked == true ) {
          m_livePitch.nonLinCurveUsed = true;
          cbxPexpo.Checked = false;       // forced: either expo OR points
        }
      }
      rbP_CheckUpdate( );
      UpdateChartItems( );
    }

    private void cbxRpts_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_isStrafe ) {
        m_liveStrafeLon.nonLinCurveUsed = false;
        if ( cbxRpts.Checked == true ) {
          m_liveStrafeLon.nonLinCurveUsed = true;
          cbxRexpo.Checked = false;       // forced: either expo OR points
        }
      }
      else {
        m_liveRoll.nonLinCurveUsed = false;
        if ( cbxRpts.Checked == true ) {
          m_liveRoll.nonLinCurveUsed = true;
          cbxRexpo.Checked = false;       // forced: either expo OR points
        }
      }
      rbR_CheckUpdate( );
      UpdateChartItems( );
    }

    #endregion

    #region Skybox Checked Changed

    private void rbOutThere1_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_OutThere1];
      LoadSkybox( );
    }

    private void rbOutThere3_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_OutThere3];
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

    private void rbHelipad_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_LA_Helipad];
      LoadSkybox( );
    }

    private void rbSkybox_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_Skybox];
      LoadSkybox( );
    }

    private void rbSunset_CheckedChanged( object sender, EventArgs e )
    {
      TMU0_Filename = SBFiles[SB_Sunset];
      LoadSkybox( );
    }

    #endregion



    private void btCopyToAllAxis_Click( object sender, EventArgs e )
    {
      lblYin1.Text = lblIn1.Text; lblYout1.Text = lblOut1.Text;
      lblYin2.Text = lblIn2.Text; lblYout2.Text = lblOut2.Text;
      lblYin3.Text = lblIn3.Text; lblYout3.Text = lblOut3.Text;
      if ( m_isStrafe ) {
        if ( m_liveStrafeLat.nonLinCurve != null ) {
          m_liveStrafeLat.nonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
        }
      }
      else {
        if ( m_liveYaw.nonLinCurve != null ) {
          m_liveYaw.nonLinCurve.Curve( float.Parse( lblYin1.Text ), float.Parse( lblYout1.Text ),
                                float.Parse( lblYin2.Text ), float.Parse( lblYout2.Text ),
                                float.Parse( lblYin3.Text ), float.Parse( lblYout3.Text ) );
        }
      }

      lblPin1.Text = lblIn1.Text; lblPout1.Text = lblOut1.Text;
      lblPin2.Text = lblIn2.Text; lblPout2.Text = lblOut2.Text;
      lblPin3.Text = lblIn3.Text; lblPout3.Text = lblOut3.Text;
      if ( m_isStrafe ) {
        if ( m_liveStrafeVert.nonLinCurve != null ) {
          m_liveStrafeVert.nonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
        }
      }
      else {
        if ( m_livePitch.nonLinCurve != null ) {
          m_livePitch.nonLinCurve.Curve( float.Parse( lblPin1.Text ), float.Parse( lblPout1.Text ),
                                float.Parse( lblPin2.Text ), float.Parse( lblPout2.Text ),
                                float.Parse( lblPin3.Text ), float.Parse( lblPout3.Text ) );
        }
      }

      lblRin1.Text = lblIn1.Text; lblRout1.Text = lblOut1.Text;
      lblRin2.Text = lblIn2.Text; lblRout2.Text = lblOut2.Text;
      lblRin3.Text = lblIn3.Text; lblRout3.Text = lblOut3.Text;
      if ( m_isStrafe ) {
        if ( m_liveStrafeLon.nonLinCurve != null ) {
          m_liveStrafeLon.nonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
        }
      }
      else {
        if ( m_liveRoll.nonLinCurve != null ) {
          m_liveRoll.nonLinCurve.Curve( float.Parse( lblRin1.Text ), float.Parse( lblRout1.Text ),
                                float.Parse( lblRin2.Text ), float.Parse( lblRout2.Text ),
                                float.Parse( lblRin3.Text ), float.Parse( lblRout3.Text ) );
        }
      }
    }

    private void btDone_Click( object sender, EventArgs e )
    {
      // It ai setup as OK button - nothing here so far...
    }


    #endregion








  }
}
