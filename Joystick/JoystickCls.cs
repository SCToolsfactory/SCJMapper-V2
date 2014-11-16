using SharpDX;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace SCJMapper_V2
{
  /// <summary>
  /// Handles one JS device as DXInput device
  /// In addition provide some static tools to handle JS props here in one place
  /// Also owns the GUI i.e. the user control that shows all values
  /// </summary>
  public class JoystickCls : DeviceCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );
    private static readonly AppSettings  appSettings = new AppSettings( );

    #region Static Items

    public new const String DeviceName = "joystick";  // the device name used throughout this app

    public const String JsUnknown = "jsx_";
    public new const String BlendedInput = "jsx_reserved";

    static private int JSnum_UNKNOWN = 0;
    static public int JSnum_MAX = 8;   // can only assign 4 jsN devices in SC


    /// <summary>
    /// Reassigns the mapping color based on the jsAssignment list given
    /// i.e. prepare the mapping colors for a given jsN assignment
    /// </summary>
    /// <param name="newJsList">List of 0.. tabs where the value is the jsN number </param>
    static public void ReassignJsColor( List<int> newJsList )
    {
      // the default colors are aligned with the tabs - the tabs color is never changed but the jsN may
      // i.e. if the first Tab is assigned as js2 then the second MapColor must get the color of the first Tab
      int idx = 0;
      foreach ( int i in newJsList ) {
        // walk through the tabs
        if ( i > 0 ) {
          // this is the jsN for the tab indexed (make it 0 based)
          MyColors.MapColor[i - 1] = MyColors.TabColor[idx];
        }
        idx++;
      }
    }

    /// <summary>
    /// Returns the currently valid color for a jsN assignment
    /// </summary>
    /// <param name="jsN">The jsN number of the command</param>
    /// <returns>A color</returns>
    static public System.Drawing.Color JsNColor( int jsN )
    {
      if ( jsN == JSnum_UNKNOWN ) return MyColors.BlendedColor;
      if ( jsN < 1 ) return MyColors.ErrorColor;
      if ( jsN > JoystickCls.JSnum_MAX ) return MyColors.ErrorColor;

      return MyColors.MapColor[jsN - 1]; // jsN is 1  based, color array is 0 based
    }


    /// <summary>
    /// Returns true if the devicename is a joystick
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    static public new Boolean IsDevice( String device )
    {
      return ( device == DeviceName );
    }


    /// <summary>
    /// Returns properly formatted jsn_ string (jsx_ if the input is UNKNOWN)
    /// </summary>
    /// <param name="jsNum">The JS number</param>
    /// <returns>The formatted JS name for the CryEngine XML</returns>
    static public String JSTag( int jsNum )
    {
      if ( IsJSValid( jsNum ) ) return "js" + jsNum.ToString( ) + "_";
      return JsUnknown;
    }


    /// <summary>
    /// Extract the JS number from a JS string (jsx_ returns 0 - UNKNOWN)
    /// </summary>
    /// <param name="jsTag">The JS string</param>
    /// <returns>The JS number</returns>
    static public int JSNum( String jsTag )
    {
      int retNum = JSnum_UNKNOWN;
      if ( !String.IsNullOrWhiteSpace( jsTag ) ) {
        if ( !int.TryParse( jsTag.Substring( 2, 1 ), out retNum ) ) {
          retNum = JSnum_UNKNOWN;
        }
      }
      return retNum;
    }

    /// <summary>
    /// Returns the validity of a JSnumber
    /// Done here to maintain the ownership of how things are done
    /// </summary>
    /// <param name="jsNum">The JS number</param>
    /// <returns>True if it is a valid one</returns>
    static public Boolean IsJSValid( int jsNum )
    {
      return ( jsNum > JSnum_UNKNOWN ) && ( jsNum <= JSnum_MAX );
    }

    const string js_pattern = @"^js\d_*";
    static Regex rgx_js = new Regex( js_pattern, RegexOptions.IgnoreCase );
    /// <summary>
    /// Returns true if the input starts with a valid jsN_ formatting
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    static public Boolean IsJsN( String input )
    {
      return rgx_js.IsMatch( input );
    }


    /// <summary>
    /// Returns an adjusted jsN_ tag with the new number
    /// </summary>
    /// <param name="input">An input directive</param>
    /// <param name="newJsN">the new JsN number</param>
    /// <returns>The modified js directive or the directive if no mod can be done</returns>
    static public String ReassignJSTag( String input, int newJsN )
    {
      if ( IsJsN( input ) ) {
        return input.Replace( input.Substring( 0, 4 ), JSTag( newJsN ) );
      }
      else {
        return input;
      }
    }



    const string jsl_pattern = @"^js\d_[xyz]$";
    static Regex rgx_jsl = new Regex( jsl_pattern, RegexOptions.IgnoreCase );
    const string jsr_pattern = @"^js\d_rot[xyz]$";
    static Regex rgx_jsr = new Regex( jsr_pattern, RegexOptions.IgnoreCase );
    /// <summary>
    /// Makes a throttle from the given ctrl
    /// accepts js#_(rot)[xyz] and returns js#_throttle[xyz]
    /// </summary>
    /// <param name="control"></param>
    /// <param name="makeIt"></param>
    /// <returns></returns>
    static public String MakeThrottle( String control, Boolean makeIt )
    {
      if ( makeIt == false ) return control;
      if ( control.Length < 5 ) return control;

      String retVal = control;
      if ( rgx_jsl.IsMatch( control ) ) {
        retVal = retVal.Insert( 4, "throttle" );
      }
      /* THIS IS WRONG.... don't know if rot can get a throttle...
      else if ( rgx_jsr.IsMatch( control ) ) {
        retVal = retVal.Remove( 4, 3 );  // remove rot
        retVal = retVal.Insert( 4, "throttle" );
      } 
      */
      return retVal;
    }

    /// <summary>
    /// returns true if the ctrl can be a throttle - for now this is js#_[xyz]
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    static public Boolean CanThrottle( String control )
    {
      return rgx_jsl.IsMatch( control ) || rgx_jsr.IsMatch( control );
    }


    #endregion

    private Joystick m_device;

    private JoystickState m_state = new JoystickState( );
    private JoystickState m_prevState = new JoystickState( );

    private Control m_hwnd;
    private int m_numPOVs = 0;      // static counter for UpdateControls
    private int m_sliderCount = 0;  // static counter for UpdateControls
    private String m_lastItem = "";
    private int m_senseLimit = 150; // axis jitter avoidance...
    private int m_joystickNumber = 0; // seq number of the enumerated joystick
    private bool[] m_ignoreButtons;
    private bool m_activated = false;

    private UC_JoyPanel m_jPanel = null; // the GUI panel
    internal int  MyTabPageIndex = -1;

    /// <summary>
    /// Returns a CryEngine compatible hat direction
    /// </summary>
    /// <param name="value">The Hat value</param>
    /// <returns>The direction string</returns>
    private String HatDir( int value )
    {
      // Hats have a 360deg -> 36000 value reporting
      if ( value == 0 ) return "up";
      if ( value == 9000 ) return "right";
      if ( value == 18000 ) return "down";
      if ( value == 27000 ) return "left";
      return "";
    }


    /// <summary>
    /// The JS ProductName property
    /// </summary>
    public override String DevName { get { return m_device.Properties.ProductName; } }
    /// <summary>
    /// The JS Instance GUID for multiple device support (VJoy gets 2 of the same name)
    /// </summary>
    public String DevInstanceGUID { get { return m_device.Information.InstanceGuid.ToString( ); } }
    /// <summary>
    /// The sequence number of the enumerated devices
    /// </summary>
    public int DevNumber { get { return m_joystickNumber; } }
    /// <summary>
    /// The assigned jsN number for this device
    /// </summary>
    public int JSAssignment
    {
      get { return m_jPanel.JsAssignment; }
      set { m_jPanel.JsAssignment = value; }
    }

    /// <summary>
    /// Returns the mapping color for this device
    /// </summary>
    public override System.Drawing.Color MapColor
    {
      get { return JsNColor( JSAssignment ); }
    }


    // device props
    public int AxisCount { get { return m_device.Capabilities.AxeCount; } }
    public int ButtonCount { get { return m_device.Capabilities.ButtonCount; } }
    public int POVCount { get { return m_device.Capabilities.PovCount; } }

    public override Boolean Activated 
    { 
      get { return m_activated;} 
      set { m_activated = value;
      if ( m_activated == false ) m_device.Unacquire( ); // explicitely if not longer active
      } 
    }


    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    /// <param name="panel">The respective JS panel to show the properties</param>
    public JoystickCls( Joystick device, Control hwnd, int joystickNum, UC_JoyPanel panel, int tabIndex )
    {
      log.DebugFormat( "JoystickCls ctor - Entry with {0}", device.Information.ProductName );

      m_device = device;
      m_hwnd = hwnd;
      m_joystickNumber = joystickNum;
      m_jPanel = panel;
      MyTabPageIndex = tabIndex;
      Activated = false;

      m_senseLimit = AppConfiguration.AppConfig.jsSenseLimit; // can be changed in the app.config file if it is still too little

      // Set BufferSize in order to use buffered data.
      m_device.Properties.BufferSize = 128;

      m_jPanel.Caption = DevName;
      m_jPanel.nAxis = AxisCount.ToString( );
      m_jPanel.nButtons = ButtonCount.ToString( );
      m_jPanel.nPOVs = POVCount.ToString( );
      m_jPanel.JsAssignment = 0; // default is no assignment

      m_ignoreButtons = new bool[m_state.Buttons.Length];
      ResetIgnoreButtons( );

      log.Debug( "Get JS Objects" );
      try {
        // Set the data format to the c_dfDIJoystick pre-defined format.
        //m_device.SetDataFormat( DeviceDataFormat.Joystick );
        // Set the cooperative level for the device.
        m_device.SetCooperativeLevel( m_hwnd, CooperativeLevel.NonExclusive | CooperativeLevel.Background );
        // Enumerate all the objects on the device.
        foreach ( DeviceObjectInstance d in m_device.GetObjects( ) ) {
          // For axes that are returned, set the DIPROP_RANGE property for the
          // enumerated axis in order to scale min/max values.
          if ( ( 0 != ( d.ObjectId.Flags & DeviceObjectTypeFlags.Axis ) ) ) {
            // Set the range for the axis.
            m_device.Properties.Range = new InputRange( -1000, +1000 );
          }
          // Update the controls to reflect what objects the device supports.
          UpdateControls( d );
        }
      }
      catch ( Exception ex ) {
        log.Error( "Get JS Objects failed", ex );
      }

      ApplySettings( ); // get whatever is needed here from Settings
      Activated = true; 
    }



    /// <summary>
    /// Shutdown device access
    /// </summary>
    public override void FinishDX( )
    {
      if ( null != m_device ) {
        log.DebugFormat( "Release DirectInput device: {0}", m_device.Information.ProductName );
        m_device.Unacquire( );
        m_device = null;
      }
    }


    private void ResetIgnoreButtons( )
    {
      for ( int i=0; i < m_ignoreButtons.Length; i++ ) m_ignoreButtons[i] = false;
    }

    /// <summary>
    /// Tells the Joystick to re-read settings
    /// </summary>
    public override void ApplySettings( )
    {
      appSettings.Reload( );

      ResetIgnoreButtons( );
      // read ignore buttons
      String igs = "";
      switch ( m_joystickNumber ) {
        case 1: igs = appSettings.IgnoreJS1; break;
        case 2: igs = appSettings.IgnoreJS2; break;
        case 3: igs = appSettings.IgnoreJS3; break;
        case 4: igs = appSettings.IgnoreJS4; break;
        case 5: igs = appSettings.IgnoreJS5; break;
        case 6: igs = appSettings.IgnoreJS6; break;
        case 7: igs = appSettings.IgnoreJS7; break;
        case 8: igs = appSettings.IgnoreJS8; break;
        default: break;
      }
      if ( String.IsNullOrWhiteSpace( igs ) ) return; // no setting - all allowed

      // read the ignore numbers
      String[] nums = igs.Split( ' ' );
      foreach ( String s in nums ) {
        int btNum = 0; // gets 1..n
        if ( int.TryParse( s, out btNum ) ) {
          if ( ( btNum > 0 ) && ( btNum <= m_ignoreButtons.Length ) ) {
            m_ignoreButtons[--btNum] = true; // zero indexed
          }
        }
      }

    }



    /// <summary>
    /// Enable the properties that are supported by the device
    /// </summary>
    /// <param name="d"></param>
    private void UpdateControls( DeviceObjectInstance d )
    {
      // Set the UI to reflect what objects the joystick supports.
      if ( ObjectGuid.XAxis == d.ObjectType ) {
        m_jPanel.Xe = true;
        m_jPanel.Xname = d.Name + ":";
      }
      if ( ObjectGuid.YAxis == d.ObjectType ) {
        m_jPanel.Ye = true;
        m_jPanel.Yname = d.Name + ":";
      }
      if ( ObjectGuid.ZAxis == d.ObjectType ) {
        m_jPanel.Ze = true;
        m_jPanel.Zname = d.Name + ":";
      }
      if ( ObjectGuid.RxAxis == d.ObjectType ) {
        m_jPanel.Xre = true;
        m_jPanel.Xrname = d.Name + ":";
      }
      if ( ObjectGuid.RyAxis == d.ObjectType ) {
        m_jPanel.Yre = true;
        m_jPanel.Yrname = d.Name + ":";
      }
      if ( ObjectGuid.RzAxis == d.ObjectType ) {
        m_jPanel.Zre = true;
        m_jPanel.Zrname = d.Name + ":";
      }
      if ( ObjectGuid.Slider == d.ObjectType ) {
        switch ( m_sliderCount++ ) {
          case 0:
            m_jPanel.S1e = true;
            m_jPanel.S1name = d.Name + ":";
            break;

          case 1:
            m_jPanel.S2e = true;
            m_jPanel.S2name = d.Name + ":";
            break;
        }
      }
      if ( ObjectGuid.PovController == d.ObjectType ) {
        switch ( m_numPOVs++ ) {
          case 0:
            m_jPanel.H1e = true;
            m_jPanel.H1name = d.Name + ":";
            break;

          case 1:
            m_jPanel.H2e = true;
            m_jPanel.H2name = d.Name + ":";
            break;

          case 2:
            m_jPanel.H3e = true;
            m_jPanel.H3name = d.Name + ":";
            break;

          case 3:
            m_jPanel.H4e = true;
            m_jPanel.H4name = d.Name + ":";
            break;
        }
      }
    }

    /// <summary>
    /// Find the last change the user did on that device
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public override String GetLastChange( )
    {
      // TODO: Expand this out into a joystick class (see commit for details)
      Dictionary<string, string> axies = new Dictionary<string, string>( )
		    {
			    {"X","x"},
			    {"Y","y"},
			    {"Z","z"}, 
			    {"RotationX","rotx"},
			    {"RotationY","roty"},
			    {"RotationZ","rotz"}
		    };

      foreach ( KeyValuePair<string, string> entry in axies ) {
        PropertyInfo axisProperty = typeof( JoystickState ).GetProperty( entry.Key );

        if ( DidAxisChange2( ( int )axisProperty.GetValue( this.m_state, null ), ( int )axisProperty.GetValue( this.m_prevState, null ) ) )
          this.m_lastItem = entry.Value;
      }

      int[] slider = m_state.Sliders;
      int[] pslider = m_prevState.Sliders;
      if ( DidAxisChange2( slider[0], pslider[0] ) ) m_lastItem = "slider1";
      if ( DidAxisChange2( slider[1], pslider[1] ) ) m_lastItem = "slider2";

      int[] pov = m_state.PointOfViewControllers;
      int[] ppov = m_prevState.PointOfViewControllers;
      if ( pov[0] >= 0 ) if ( pov[0] != ppov[0] ) m_lastItem = "hat1_" + HatDir( pov[0] );
      if ( pov[1] >= 0 ) if ( pov[1] != ppov[1] ) m_lastItem = "hat2_" + HatDir( pov[0] );
      if ( pov[2] >= 0 ) if ( pov[2] != ppov[2] ) m_lastItem = "hat3_" + HatDir( pov[0] );
      if ( pov[3] >= 0 ) if ( pov[3] != ppov[3] ) m_lastItem = "hat4_" + HatDir( pov[0] );

      bool[] buttons = m_state.Buttons;
      bool[] prevButtons = m_prevState.Buttons;
      for ( int bi = 0; bi < buttons.Length; bi++ ) {
        if ( m_ignoreButtons[bi] == false ) {
          if ( buttons[bi] && buttons[bi] != prevButtons[bi] )
            m_lastItem = "button" + ( bi + 1 ).ToString( );
        }
      }
      return m_lastItem;
    }


    ///<summary>
    /// Figure out if an axis changed enough to consider it as a changed state
    /// The change is polled every 100ms (timer1) so the user has to change so much within that time
    /// Then an axis usually swings back when left alone - that is the real recording of a change.
    /// We know that the range is -1000 .. 1000 so we can judge absolute
    /// % relative is prone to small changes around 0 - which is likely the case with axes
    /// </summary>
    private bool DidAxisChange2( int current, int prev )
    {
      // determine if the axis drifts more than x units to account for bounce
      // old-new/old
      if ( current == prev )
        return false;
      int change = Math.Abs( prev - current );
      // if the axis has changed more than x units to it's last value
      return change > m_senseLimit ? true : false;

    }

    ///<summary>
    /// Figure out if an axis changed enough to consider it as a changed state
    /// </summary>
    private bool DidAxisChange( int current, int prev )
    {
      // determine if the axis drifts more than x% to account for bounce
      // old-new/old
      if ( current == prev )
        return false;
      if ( prev == 0 )
        prev = 1;
      int changepct = Math.Abs( prev ) - Math.Abs( current ) / Math.Abs( prev );
      // if the axis has changed more than 70% relative to it's last value
      return changepct > 70 ? true : false;

    }

    /// <summary>
    /// Show the current props in the GUI
    /// </summary>
    private void UpdateUI( )
    {
      // This function updated the UI with joystick state information.
      string strText = null;

      m_jPanel.X = m_state.X.ToString( );
      m_jPanel.Y = m_state.Y.ToString( );
      m_jPanel.Z = m_state.Z.ToString( );

      m_jPanel.Xr = m_state.RotationX.ToString( );
      m_jPanel.Yr = m_state.RotationY.ToString( );
      m_jPanel.Zr = m_state.RotationZ.ToString( );


      int[] slider = m_state.Sliders;

      m_jPanel.S1 = slider[0].ToString( );
      m_jPanel.S2 = slider[1].ToString( );

      int[] pov = m_state.PointOfViewControllers;

      m_jPanel.H1 = pov[0].ToString( );
      m_jPanel.H2 = pov[1].ToString( );
      m_jPanel.H3 = pov[2].ToString( );
      m_jPanel.H4 = pov[3].ToString( );

      // Fill up text with which buttons are pressed
      bool[] buttons = m_state.Buttons;

      int button = 0;
      foreach ( bool b in buttons ) {
        if ( b )
          strText += ( button + 1 ).ToString( "00 " ); // buttons are 1 based
        button++;
      }
      m_jPanel.Button = strText;
    }


    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public void GetAxisData( out int x, out int y, out int rz )
    {
      x = 0; y = 0; rz = 0;

      // Make sure there is a valid device.
      if ( null == m_device )
        return;

      // Poll the device for info.
      try {
        m_device.Poll( );
      }
      catch ( SharpDXException e ) {
        if ( ( e.ResultCode == ResultCode.NotAcquired ) || ( e.ResultCode == ResultCode.InputLost ) ) {
          // Check to see if either the app needs to acquire the device, or
          // if the app lost the device to another process.
          try {
            // Acquire the device.
            m_device.Acquire( );
          }
          catch ( SharpDXException ) {
            // Failed to acquire the device. This could be because the app doesn't have focus.
            return;  // EXIT unaquired
          }
        }
        else {
          log.Error( "Unexpected Poll Exception", e );
          return;  // EXIT see ex code
        }
      }


      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;
      try { m_state = m_device.GetCurrentState( ); }
      // Catch any exceptions. None will be handled here, 
      // any device re-aquisition will be handled above.  
      catch ( SharpDXException ) {
        return;
      }

      x = m_state.X; y = m_state.Y; rz = m_state.RotationZ;
    }




    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public void GetCmdData( String cmd, out int data )
    {
      // TODO: Expand this out into a joystick class (see commit for details)
      Dictionary<string, string> axies = new Dictionary<string, string>( )
		    {
			    {"x","X"},
			    {"y","Y"},
			    {"z","Z"}, 
			    {"rotx","RotationX"},
			    {"roty","RotationY"},
			    {"rotz","RotationZ"}
		    };

      data = 0;

      // Make sure there is a valid device.
      if ( null == m_device )
        return;

      // Poll the device for info.
      try {
        m_device.Poll( );
      }
      catch ( SharpDXException e ) {
        if ( ( e.ResultCode == ResultCode.NotAcquired ) || ( e.ResultCode == ResultCode.InputLost ) ) {
          // Check to see if either the app needs to acquire the device, or
          // if the app lost the device to another process.
          try {
            // Acquire the device.
            m_device.Acquire( );
          }
          catch ( SharpDXException ) {
            // Failed to acquire the device. This could be because the app doesn't have focus.
            return;  // EXIT unaquired
          }
        }
        else {
          log.Error( "Unexpected Poll Exception", e );
          return;  // EXIT see ex code
        }
      }


      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;
      try { m_state = m_device.GetCurrentState( ); }
      // Catch any exceptions. None will be handled here, 
      // any device re-aquisition will be handled above.  
      catch ( SharpDXException ) {
        return;
      }

      try {
        PropertyInfo axisProperty = typeof( JoystickState ).GetProperty( axies[cmd] );
        data = ( int )axisProperty.GetValue( this.m_state, null );
      }
      catch {
        data = 0;
      }
    }




    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public override void GetData( )
    {
      // Make sure there is a valid device.
      if ( null == m_device )
        return;

      // Poll the device for info.
      try {
        m_device.Poll( );
      }
      catch ( SharpDXException e ) {
        if ( ( e.ResultCode == ResultCode.NotAcquired ) || ( e.ResultCode == ResultCode.InputLost ) ) {
          // Check to see if either the app needs to acquire the device, or
          // if the app lost the device to another process.
          try {
            // Acquire the device - if the (main)window is active
            if ( Activated ) m_device.Acquire( );
          }
          catch ( SharpDXException ) {
            // Failed to acquire the device. This could be because the app doesn't have focus.
            return;  // EXIT unaquired
          }
        }
        else {
          log.Error( "Unexpected Poll Exception", e );
          return;  // EXIT see ex code
        }
      }


      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;
      try { m_state = m_device.GetCurrentState( ); }
      // Catch any exceptions. None will be handled here, 
      // any device re-aquisition will be handled above.  
      catch ( SharpDXException ) {
        return;
      }

      UpdateUI( ); // and update the GUI
    }



  }
}
