using SharpDX;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SCJMapper_V2
{
  /// <summary>
  /// Handles one JS device as DXInput device
  /// In addition provide some static tools to handle JS props here in one place
  /// Also owns the GUI i.e. the user control that shows all values
  /// </summary>
  class JoystickCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );
    private static readonly AppSettings  appSettings = new AppSettings( );

    #region Static Items

    public const String DeviceName = "joystick";  // the device name used throughout this app
    static private int JSnum_UNKNOWN = 0;

    /// <summary>
    /// Returns true if the devicename is a joystick
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    static public Boolean IsJoystick( String device )
    {
      return ( device == DeviceName );
    }


    /// <summary>
    /// Returns properly formatted jsn_ string
    /// </summary>
    /// <param name="jsNum">The JS number</param>
    /// <returns>The formatted JS name for the CryEngine XML</returns>
    static public String JSTag( int jsNum )
    {
      if ( IsJSValid( jsNum ) ) return "js" + jsNum.ToString( ) + "_";
      else return "";
    }


    /// <summary>
    /// Extract the JS number from a JS string
    /// </summary>
    /// <param name="jsTag">The JS string</param>
    /// <returns>The JS number</returns>
    static public int JSNum( String jsTag )
    {
      int retNum = JSnum_UNKNOWN;
      if ( !String.IsNullOrEmpty( jsTag ) ) {
        int.TryParse( jsTag.Substring( 2, 1 ), out retNum );
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
      return ( jsNum > JSnum_UNKNOWN );
    }

    const string js_pattern = @"^js\d_[xyz]$";
    static Regex rgx_js = new Regex( js_pattern, RegexOptions.IgnoreCase );
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
      if ( rgx_js.IsMatch( control ) ) {
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
      return rgx_js.IsMatch( control ) || rgx_jsr.IsMatch( control );
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
    private int m_joystickNumber = 0;
    private bool[] m_ignoreButtons;

    private UC_JoyPanel m_jPanel = null; // the GUI panel


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
    /// The povides the JS ProductName property
    /// </summary>
    public String DevName { get { return m_device.Properties.ProductName; } }
    public int AxisCount { get { return m_device.Capabilities.AxeCount; } }
    public int ButtonCount { get { return m_device.Capabilities.ButtonCount; } }
    public int POVCount { get { return m_device.Capabilities.PovCount; } }


    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    /// <param name="panel">The respective JS panel to show the properties</param>
    public JoystickCls( Joystick device, Control hwnd, int joystickNum, UC_JoyPanel panel )
    {
      log.DebugFormat( "ctor - Entry with {0}", device.Information.ProductName );

      m_device = device;
      m_hwnd = hwnd;
      m_joystickNumber = joystickNum;
      m_jPanel = panel;

      m_senseLimit = AppConfiguration.AppConfig.jsSenseLimit; // can be changed in the app.config file if it is still too little

      // Set BufferSize in order to use buffered data.
      m_device.Properties.BufferSize = 128;

      m_jPanel.Caption = DevName;
      m_jPanel.nAxis = AxisCount.ToString( );
      m_jPanel.nButtons = ButtonCount.ToString( );
      m_jPanel.nPOVs = POVCount.ToString( );

      m_ignoreButtons = new bool[m_state.Buttons.Length]; 
      ResetIgnoreButtons( );

      log.Debug( "Get JS Objects" );
      try {
        // Set the data format to the c_dfDIJoystick pre-defined format.
        //m_device.SetDataFormat( DeviceDataFormat.Joystick );
        // Set the cooperative level for the device.
        m_device.SetCooperativeLevel( m_hwnd, CooperativeLevel.Exclusive | CooperativeLevel.Foreground );
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
    }



    /// <summary>
    /// Shutdown device access
    /// </summary>
    public void FinishDX( )
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
    public void ApplySettings( )
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
    public String GetLastChange( )
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
    public void GetData( )
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

      UpdateUI( ); // and update the GUI
    }



  }
}
