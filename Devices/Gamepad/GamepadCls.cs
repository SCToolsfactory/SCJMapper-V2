using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.XInput;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

using SCJMapper_V2.Common;

namespace SCJMapper_V2.Devices.Gamepad
{
  /// <summary>
  /// Handles one JS device as DXInput device
  /// In addition provide some static tools to handle JS props here in one place
  /// Also owns the GUI i.e. the user control that shows all values
  /// </summary>
  public class GamepadCls : DeviceCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    #region Static Items

    public new const string DeviceClass = "xboxpad";  // the device name used throughout this app (3.5 still in user mapping ??)
    public const string DeviceClass_3_5 = "gamepad";  // defaultProfile from Alpha 3.5 onwards
    public new const string DeviceID = "xi1_";
    static public int RegisteredDevices = 0;
    public const string DevNameCIG = "Controller (Gamepad)"; // seems CIG names the Gamepad always like this - and not as the device replies
    public const string DevGUIDCIG = "{028E045E-0000-0000-0000-504944564944}"; // - Fixed for Gamepads  - just one of the many possible - 

    public const string JsUnknown = "xi_";
    public new const string DisabledInput = DeviceID + DeviceCls.DisabledInput;
    static public new bool IsDisabledInput( string input )
    {
      if ( input == DisabledInput ) return true;
      return false;
    }

    /// <summary>
    /// Returns the currently valid color
    /// </summary>
    /// <returns>A color</returns>
    static public System.Drawing.Color XiColor()
    {
      return MyColors.GamepadColor;
    }


    /// <summary>
    /// Returns true if the devicename is a gamepad
    /// </summary>
    /// <param name="deviceClass"></param>
    /// <returns></returns>
    static public new bool IsDeviceClass( string deviceClass )
    {
      return ( deviceClass == DeviceClass ) || ( deviceClass == DeviceClass_3_5 ); // handle Alpha 3.5+
    }

    /// <summary>
    /// Return this deviceClass if the input string contains something like xiN_
    /// </summary>
    /// <param name="devInput"></param>
    /// <returns></returns>
    static public new string DeviceClassFromInput( string devInput )
    {
      if ( DevMatch( devInput ) )
        return DeviceClass; // this
      else
        return DeviceCls.DeviceClass; // unknown
    }

    /// <summary>
    /// Create a DevInput string if the input does look like not having a device ID
    /// </summary>
    /// <param name="input">A gamepad input</param>
    /// <returns>DevInput</returns>
    static public new string DevInput( string input )
    {
      if ( DevMatch( input ) )
        return input; // already
      else
        return FromAC1( input );
    }

    /// <summary>
    /// Returns true if the input matches this device
    /// </summary>
    /// <param name="devInput">A devInput string</param>
    /// <returns>True for a match</returns>
    static public new bool DevMatch( string devInput )
    {
      if ( string.IsNullOrEmpty( devInput ) ) return false;
      return devInput.StartsWith( DeviceID );
    }

    /// <summary>
    /// Returns true if a command is an axis command 
    /// </summary>
    /// <param name="command">The command string</param>
    /// <returns>True if it is an axis command</returns>
    static public new bool IsAxisCommand( string command )
    {
      string cLower = command.ToLowerInvariant( );
      return ( cLower.EndsWith( "_thumblx" ) || cLower.EndsWith( "_thumbly" )
          || cLower.EndsWith( "_thumbrx" ) || cLower.EndsWith( "_thumbry" ) );
    }


    const string xil_pattern = @"^xi_thumb[lr][xy]$";
    static Regex rgx_xil = new Regex( xil_pattern, RegexOptions.IgnoreCase );
    /// <summary>
    /// returns true if the ctrl can be inverted - for now this is thumb[lr][xyz]
    /// </summary>
    /// <param name="control"></param>
    /// <returns></returns>
    static public bool CanInvert( string control )
    {
      return rgx_xil.IsMatch( control );
    }

    /// <summary>
    /// Reformat the input from AC1 style to AC2 style
    /// </summary>
    /// <param name="input">The AC1 input string</param>
    /// <returns>An AC2 style input string</returns>
    static public string FromAC1( string input )
    {
      if ( string.IsNullOrEmpty( input ) ) return "";

      // input is something like a xi_something or compositions like triggerl_btn+thumbrx 
      // try easy: add xi1_ at the beginning; if xi_start subst with xi1_
      string retVal = input.Replace( " ", "" );
      if ( IsDisabledInput( input ) ) return input;

      if ( retVal.StartsWith( "xi_" ) )
        retVal = retVal.Insert( 2, "1" );
      else
        retVal = "xi1_" + retVal;

      return retVal;
    }



    #endregion

    private Controller m_device;
    private string m_devName = DevNameCIG;
    private string m_devGUID = DevGUIDCIG;
    private Capabilities m_gpCaps = new Capabilities( );

    private State m_state = new State( );
    private State m_prevState = new State( );

    private string m_lastItem = "";
    private int m_senseLimit = 500; // axis jitter avoidance...
    private int m_thlx_zero = 0;
    private int m_thly_zero = 0;
    private int m_thrx_zero = 0;
    private int m_thry_zero = 0;

    private bool m_activated = false;

    private UC_GpadPanel m_gPanel = null; // the GUI panel
    internal int MyTabPageIndex = -1;


    /// <summary>
    /// Return the device instance number (which is always 1)
    /// </summary>
    public override int XmlInstance { get { return 1; } } // const for Gamepad
    /// <summary>
    /// Return the DX device instance number (which is always 0)
    /// </summary>
    public override int DevInstance { get { return 0; } }
    /// <summary>
    /// The DeviceClass of this instance
    /// </summary>
    public override string DevClass { get { return GamepadCls.DeviceClass; } }
    /// <summary>
    /// The Gamepad ProductName property
    /// </summary>
    public override string DevName { get { return m_devName; } }
    /// <summary>
    /// The ProductGUID property
    /// </summary>
    public override string DevGUID { get { return m_devGUID; } }

    public void SetDeviceName( string devName )
    {
      m_devName = DevNameCIG; // hard override ...
      m_gPanel.Caption = DevName;
    }

    /// <summary>
    /// The JS Instance GUID for multiple device support (VJoy gets 2 of the same name)
    /// </summary>
    public override string DevInstanceGUID { get { return "17809207-4663-4629-b5f8-26cc6afa0e70"; } } // artifical GUID - DX does not maintain one

    /// <summary>
    /// Returns the mapping color for this device
    /// </summary>
    public override System.Drawing.Color MapColor
    {
      get { return MyColors.GamepadColor; }
    }


    // Note: GP has deadzone on left and right thumb only
    /*
       <deviceoptions name="Controller (Gamepad)">
        <option input="thumbl" deadzone="0.21575999"/>
        <option input="thumbr" deadzone="0.22475"/>
       </deviceoptions>
     */
    public override List<string> AnalogCommands
    {
      get {
        List<string> cmds = new List<string>( );

        try {
          // Enumerate all the objects on the device.
          if ( ( m_gpCaps.Gamepad.LeftThumbX != 0 ) || ( m_gpCaps.Gamepad.LeftThumbY != 0 ) ) { cmds.Add( "thumbl" ); }
          if ( ( m_gpCaps.Gamepad.RightThumbX != 0 ) || ( m_gpCaps.Gamepad.RightThumbY != 0 ) ) { cmds.Add( "thumbr" ); }
        } catch ( Exception ex ) {
          log.Error( "AnalogCommands - Get Gamepad Objects failed", ex );
        }
        cmds.Sort( );
        return cmds;
      }
    }

    public override bool Activated { get => m_activated; set => m_activated = value; }

    private bool Bit( GamepadButtonFlags set, GamepadButtonFlags check )
    {
      Int32 s = (Int32)set; Int32 c = (Int32)check;
      return ( ( s & c ) == c );
    }


    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    /// <param name="panel">The respective JS panel to show the properties</param>
    public GamepadCls( Controller device, UC_GpadPanel panel, int tabIndex )
    {
      log.DebugFormat( "GamepadCls ctor - Entry with index {0}", device.ToString( ) );

      m_device = device;
      m_gPanel = panel;
      MyTabPageIndex = tabIndex;
      m_activated = false;

      m_senseLimit = AppConfiguration.AppConfig.gpSenseLimit; // can be changed in the app.config file if it is still too little

      // Set BufferSize in order to use buffered data.
      log.Debug( "Get GP Objects" );
      try {
        m_gpCaps = m_device.GetCapabilities( DeviceQueryType.Gamepad );
      } catch ( Exception ex ) {
        log.Error( "Get GamepadCapabilities failed", ex );
      }

      m_gPanel.Caption = m_devName;
      int n = 0;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.DPadDown ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.DPadLeft ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.DPadRight ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.DPadUp ) ) n++;
      m_gPanel.nDPads = n.ToString( );
      m_gPanel.DPadE = ( n > 0 );

      n = 0;
      if ( ( m_gpCaps.Gamepad.LeftThumbX != 0 ) || ( m_gpCaps.Gamepad.LeftThumbY != 0 ) || Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) { n++; m_gPanel.TStickLE = true; }
      if ( ( m_gpCaps.Gamepad.RightThumbX != 0 ) || ( m_gpCaps.Gamepad.RightThumbY != 0 ) || Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) { n++; m_gPanel.TStickRE = true; }
      m_gPanel.nTSticks = n.ToString( );

      n = 0;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.A ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.B ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.X ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.Y ) ) n++;
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.Start ) ) { n++; m_gPanel.StartE = true; }
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.Back ) ) { n++; m_gPanel.BackE = true; }
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.LeftShoulder ) ) { n++; m_gPanel.ShoulderLE = true; }
      if ( Bit( m_gpCaps.Gamepad.Buttons, GamepadButtonFlags.RightShoulder ) ) { n++; m_gPanel.ShoulderRE = true; }
      m_gPanel.nButtons = n.ToString( );

      n = 0;
      if ( m_gpCaps.Gamepad.LeftTrigger > 0 ) { n++; m_gPanel.TriggerLE = true; }
      if ( m_gpCaps.Gamepad.RightTrigger > 0 ) { n++; m_gPanel.TriggerRE = true; }
      m_gPanel.nTriggers = n.ToString( );

      m_gPanel.ButtonE = true; // what else ...

      ApplySettings_low( ); // get whatever is needed here from Settings

      GamepadCls.RegisteredDevices++;
      m_activated = true;
    }



    /// <summary>
    /// Shutdown device access
    /// </summary>
    public override void FinishDX()
    {
      log.DebugFormat( "Release Input device: {0}", m_device );
    }

    /// <summary>
    /// Tells the Joystick to re-read settings
    /// </summary>
    public override void ApplySettings()
    {
      ApplySettings_low( );
    }

    private void ApplySettings_low()
    {
      AppSettings.Instance.Reload( );
    }


    /// <summary>
    /// Returns true if a modifer button is pressed
    /// </summary>
    /// <returns></returns>
    private bool ModButtonPressed()
    {
      bool retVal = m_state.Gamepad.Buttons != GamepadButtonFlags.None;
      retVal = ( retVal || ( Math.Abs( (Int32)m_state.Gamepad.LeftTrigger ) > 0 ) );
      retVal = ( retVal || ( Math.Abs( (Int32)m_state.Gamepad.RightTrigger ) > 0 ) );
      return retVal;
    }

    /// <summary>
    /// Checks if all 4 buttons are pressed and then calibrates the thumbaxes
    /// </summary>
    private void CheckAndCalibrate( ref State state )
    {
      bool check = true;
      check &= Bit( state.Gamepad.Buttons, GamepadButtonFlags.A );
      check &= Bit( state.Gamepad.Buttons, GamepadButtonFlags.B );
      check &= Bit( state.Gamepad.Buttons, GamepadButtonFlags.X );
      check &= Bit( state.Gamepad.Buttons, GamepadButtonFlags.Y );

      if (check) {
        m_thlx_zero = state.Gamepad.LeftThumbX;
        m_thly_zero = state.Gamepad.LeftThumbY;
        m_thrx_zero = state.Gamepad.RightThumbX;
        m_thry_zero = state.Gamepad.RightThumbY;
      }

      ApplyCalibration( ref state );
    }

    /// <summary>
    /// Applies the calibration to the state
    /// and makes sure the values are still in range of +-32767
    /// </summary>
    private void ApplyCalibration( ref State state )
    {

      int val = 0; int sign = 1;
      sign = Math.Sign( (int)state.Gamepad.LeftThumbX - m_thlx_zero );
      val = Math.Abs( (int)state.Gamepad.LeftThumbX - m_thlx_zero );
      state.Gamepad.LeftThumbX = (short)( val > 32767 ? 32767 * sign : val * sign );

      sign = Math.Sign( (int)state.Gamepad.LeftThumbY - m_thly_zero );
      val = Math.Abs( (int)state.Gamepad.LeftThumbY - m_thly_zero );
      state.Gamepad.LeftThumbY = (short)( val > 32767 ? 32767 * sign : val * sign );

      sign = Math.Sign( (int)state.Gamepad.RightThumbX - m_thrx_zero );
      val = Math.Abs( (int)state.Gamepad.RightThumbX - m_thrx_zero );
      state.Gamepad.RightThumbX = (short)( val > 32767 ? 32767 * sign : val * sign );

      sign = Math.Sign( (int)state.Gamepad.RightThumbY - m_thry_zero );
      val = Math.Abs( (int)state.Gamepad.RightThumbY - m_thry_zero );
      state.Gamepad.RightThumbY = (short)( val > 32767 ? 32767 * sign : val * sign );
    }


    /// <summary>
    /// returns the currently available input string
    ///  (does not retrieve new data but uses what was collected by GetData())
    /// </summary>
    /// <returns>An input string or an empty string if no input is available</returns>
    public override string GetCurrentInput()
    {
      string currentInput = string.Empty;

      if ( ModButtonPressed( ) ) {
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) currentInput += "xi_thumblx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) currentInput += "xi_thumbly+";

        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) currentInput += "xi_thumbrx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) currentInput += "xi_thumbry+";

        if ( Math.Abs( (Int32)m_state.Gamepad.LeftTrigger ) > 0 ) currentInput += "xi_triggerl_btn+";
        if ( Math.Abs( (Int32)m_state.Gamepad.RightTrigger ) > 0 ) currentInput += "xi_triggerr_btn+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.A ) ) currentInput += "xi_a+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.B ) ) currentInput += "xi_b+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.X ) ) currentInput += "xi_x+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Y ) ) currentInput += "xi_y+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Start ) ) currentInput += "xi_start+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Back ) ) currentInput += "xi_back+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadDown ) ) currentInput += "xi_dpad_down+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadLeft ) ) currentInput += "xi_dpad_left+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadRight ) ) currentInput += "xi_dpad_right+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadUp ) ) currentInput += "xi_dpad_up+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftShoulder ) ) currentInput += "xi_shoulderl+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightShoulder ) ) currentInput += "xi_shoulderr+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) currentInput += "xi_thumbl+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) currentInput += "xi_thumbr+";
      }
      else {
        // no button -> only non button items will reported - single events
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) ) ) currentInput = "xi_thumblx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) ) ) currentInput = "xi_thumbly+";

        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) ) ) currentInput = "xi_thumbrx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) ) ) currentInput = "xi_thumbry+";

        if ( Math.Abs( (Int32)m_state.Gamepad.LeftTrigger ) > 0 ) currentInput = "xi_triggerl_btn+";
        if ( Math.Abs( (Int32)m_state.Gamepad.RightTrigger ) > 0 ) currentInput = "xi_triggerr_btn+";
      }

      return currentInput.TrimEnd( new char[] { '+' } );
    }


    /// <summary>
    /// Find the last change the user did on that device
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public override string GetLastChange( )
    {
      if ( ModButtonPressed( ) ) {
        m_lastItem = string.Empty;
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) m_lastItem += "xi_thumblx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) m_lastItem += "xi_thumbly+";

        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) m_lastItem += "xi_thumbrx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) m_lastItem += "xi_thumbry+";

        if ( Math.Abs( (Int32)m_state.Gamepad.LeftTrigger ) > 0 ) m_lastItem += "xi_triggerl_btn+";
        if ( Math.Abs( (Int32)m_state.Gamepad.RightTrigger ) > 0 ) m_lastItem += "xi_triggerr_btn+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.A ) ) m_lastItem += "xi_a+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.B ) ) m_lastItem += "xi_b+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.X ) ) m_lastItem += "xi_x+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Y ) ) m_lastItem += "xi_y+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Start ) ) m_lastItem += "xi_start+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Back ) ) m_lastItem += "xi_back+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadDown ) ) m_lastItem += "xi_dpad_down+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadLeft ) ) m_lastItem += "xi_dpad_left+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadRight ) ) m_lastItem += "xi_dpad_right+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.DPadUp ) ) m_lastItem += "xi_dpad_up+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftShoulder ) ) m_lastItem += "xi_shoulderl+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightShoulder ) ) m_lastItem += "xi_shoulderr+";

        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) m_lastItem += "xi_thumbl+";
        if ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) m_lastItem += "xi_thumbr+";
      }
      else {
        // no button -> only non button items will reported - single events
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) ) ) m_lastItem = "xi_thumblx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.LeftThumbY ) > Math.Abs( (Int32)m_state.Gamepad.LeftThumbX ) ) ) m_lastItem = "xi_thumbly+";

        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) ) ) m_lastItem = "xi_thumbrx+";
        if ( ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > m_senseLimit )
          && ( Math.Abs( (Int32)m_state.Gamepad.RightThumbY ) > Math.Abs( (Int32)m_state.Gamepad.RightThumbX ) ) ) m_lastItem = "xi_thumbry+";

        if ( Math.Abs( (Int32)m_state.Gamepad.LeftTrigger ) > 0 ) m_lastItem = "xi_triggerl_btn+";
        if ( Math.Abs( (Int32)m_state.Gamepad.RightTrigger ) > 0 ) m_lastItem = "xi_triggerr_btn+";
      }

      return m_lastItem.TrimEnd( new char[] { '+' } );
    }


    ///<summary>
    /// Figure out if an axis changed enough to consider it as a changed state
    /// The change is polled every 100ms (timer1) so the user has to change so much within that time
    /// Then an axis usually swings back when left alone - that is the real recording of a change.
    /// We know that the range is -32758 .. 32767 so we can judge absolute
    /// % relative is prone to small changes around 0 - which is likely the case with axes
    /// </summary>
    private bool DidAxisChange2( int current, int prev )
    {
      // determine if the axis drifts more than x units to account for bounce
      // old-new/old
      if ( current == prev )
        return false;
      int change = ( Math.Abs( current ) - Math.Abs( prev ) ) / 32;
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
    private void UpdateUI()
    {
      if ( Application.MessageLoop ) {

        // This function updated the UI with joystick state information.
        string strText = "";

        strText += ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.DPadDown ) > 0 ) ? "d" : " ";
        strText += ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft ) > 0 ) ? "l" : " ";
        strText += ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.DPadRight ) > 0 ) ? "r" : " ";
        strText += ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.DPadUp ) > 0 ) ? "u" : " ";
        m_gPanel.DPad = strText;

        m_gPanel.TStickXL = m_state.Gamepad.LeftThumbX.ToString( );
        m_gPanel.TStickYL = m_state.Gamepad.LeftThumbY.ToString( );
        m_gPanel.TStickBtL = ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb ) > 0 ) ? "pressed" : "_";
        m_gPanel.TStickXR = m_state.Gamepad.RightThumbX.ToString( );
        m_gPanel.TStickYR = m_state.Gamepad.RightThumbY.ToString( );
        m_gPanel.TStickBtR = ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.RightThumb ) > 0 ) ? "pressed" : "_";

        m_gPanel.TriggerL = m_state.Gamepad.LeftTrigger.ToString( );
        m_gPanel.TriggerR = m_state.Gamepad.RightTrigger.ToString( );

        m_gPanel.ShoulderL = ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder ) > 0 ) ? "pressed" : "_";
        m_gPanel.ShoulderR = ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder ) > 0 ) ? "pressed" : "_";

        m_gPanel.Start = ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.Start ) > 0 ) ? "pressed" : "_";
        m_gPanel.Back = ( ( m_state.Gamepad.Buttons & GamepadButtonFlags.Back ) > 0 ) ? "pressed" : "_";


        string buttons = "";
        buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.A ) ) ? "A" : "_";
        buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.B ) ) ? "B" : "_";
        buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.X ) ) ? "X" : "_";
        buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Y ) ) ? "Y" : "_";
        m_gPanel.Button = buttons;
      }
    }


    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public override void GetCmdData( string cmd, out int data )
    {
      // Make sure there is a valid device.
      if ( m_device == null ) {
        data = 0;
        return;
      }

      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;

      // Poll the device for info.
      try {
        m_state = m_device.GetState( );
        CheckAndCalibrate( ref m_state );

        switch ( cmd ) {
          case "thumblx": data = (int)( m_state.Gamepad.LeftThumbX / 32.767f ); break; // data should be -1000..1000
          case "thumbly": data = (int)( m_state.Gamepad.LeftThumbY / 32.767f ); break;
          case "thumbrx": data = (int)( m_state.Gamepad.RightThumbX / 32.767f ); break;
          case "thumbry": data = (int)( m_state.Gamepad.RightThumbY / 32.767f ); break;
          default: data = 0; break;
        }

      } catch ( SharpDXException e ) {
        log.Error( "Gamepad-GetData: Unexpected Poll Exception", e );
        data = 0;
        return;  // EXIT see ex code
      }
    }


    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public override void GetData()
    {
      // Make sure there is a valid device.
      if ( m_device == null )
        return;

      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;

      // Poll the device for info.
      try {
        m_state = m_device.GetState( );
        CheckAndCalibrate( ref m_state );
      } catch ( SharpDXException e ) {
        log.Error( "Gamepad-GetData: Unexpected Poll Exception", e );
        return;  // EXIT see ex code
      }
      UpdateUI( ); // and update the GUI
    }

  }
}
