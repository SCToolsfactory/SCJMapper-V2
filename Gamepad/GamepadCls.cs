using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.XInput;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace SCJMapper_V2
{
  /// <summary>
  /// Handles one JS device as DXInput device
  /// In addition provide some static tools to handle JS props here in one place
  /// Also owns the GUI i.e. the user control that shows all values
  /// </summary>
  public class GamepadCls : DeviceCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );
    private static readonly AppSettings  appSettings = new AppSettings( );

    #region Static Items

    public new const String DeviceName = "xboxpad";  // the device name used throughout this app
    public const String JsUnknown = "xi_";
    public new const String BlendedInput = "xi_reserved";

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
    /// <param name="device"></param>
    /// <returns></returns>
    static public new Boolean IsDevice( String device )
    {
      return ( device == DeviceName );
    }

    /// <summary>
    /// Returns true if the input is an xi_ but not xi_reserved
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    static public Boolean IsXiValid( String input )
    {
      return (IsXi(input) && (String.Compare(input, BlendedInput)!= 0));
    }


    const string xi_pattern = @"^xi_*";
    static Regex rgx_xi = new Regex( xi_pattern, RegexOptions.IgnoreCase );
    /// <summary>
    /// Returns true if the input starts with a valid xi_ formatting
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    static public Boolean IsXi( String input )
    {
      return rgx_xi.IsMatch( input );
    }

    #endregion

    private Controller m_device;

    private Capabilities m_gpCaps = new Capabilities( );

    private State m_state = new State( );
    private State m_prevState = new State( );

    private String m_lastItem = "";
    private int m_senseLimit = 500; // axis jitter avoidance...
    private bool m_activated = false;

    private UC_GpadPanel m_gPanel = null; // the GUI panel
    internal int  MyTabPageIndex = -1;


    /// <summary>
    /// The JS ProductName property
    /// </summary>
    public override String DevName { get { return "Generic Gamepad"; } }
    /// <summary>

    /// <summary>
    /// Returns the mapping color for this device
    /// </summary>
    public override System.Drawing.Color MapColor
    {
      get { return MyColors.GamepadColor; }
    }

    public override Boolean Activated
    {
      get { return m_activated; }
      set
      {
        m_activated = value;
      }
    }

    private Boolean Bit( GamepadButtonFlags set, GamepadButtonFlags check )
    {
      Int32 s = ( Int32 )set; Int32 c = ( Int32 )check;
      return ( ( s & c ) == c );
    }


    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    /// <param name="panel">The respective JS panel to show the properties</param>
    public GamepadCls( SharpDX.XInput.Controller device, UC_GpadPanel panel, int tabIndex )
    {
      log.DebugFormat( "GamepadCls ctor - Entry with index {0}", device.ToString( ) );

      m_device = device;
      m_gPanel = panel;
      MyTabPageIndex = tabIndex;
      Activated = false;

      m_senseLimit = AppConfiguration.AppConfig.gpSenseLimit; // can be changed in the app.config file if it is still too little

      // Set BufferSize in order to use buffered data.
      log.Debug( "Get GP Objects" );
      try {
        m_gpCaps = m_device.GetCapabilities( DeviceQueryType.Gamepad );
      }
      catch ( Exception ex ) {
        log.Error( "Get GamepadCapabilities failed", ex );
      }

      m_gPanel.Caption = DevName;
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

      ApplySettings( ); // get whatever is needed here from Settings
      Activated = true;
    }



    /// <summary>
    /// Shutdown device access
    /// </summary>
    public override void FinishDX( )
    {
      log.DebugFormat( "Release Input device: {0}", m_device );
    }

    /// <summary>
    /// Tells the Joystick to re-read settings
    /// </summary>
    public override void ApplySettings( )
    {
      appSettings.Reload( );
    }


    /// <summary>
    /// Returns true if a modifer button is pressed
    /// </summary>
    /// <returns></returns>
    private Boolean ModButtonPressed( )
    {
      Boolean retVal =  m_state.Gamepad.Buttons != GamepadButtonFlags.None;
      retVal = ( retVal || ( Math.Abs( ( Int32 )m_state.Gamepad.LeftTrigger ) > 0 ) );
      retVal = ( retVal || ( Math.Abs( ( Int32 )m_state.Gamepad.RightTrigger ) > 0 ) );
      return retVal;
    }


    /// <summary>
    /// Find the last change the user did on that device
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public override String GetLastChange( )
    {
      if ( ModButtonPressed() ) {
        m_lastItem = "";
        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbX ) > m_senseLimit )
          && ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbX ) > Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbY ) ) 
          && !Bit(m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb) ) m_lastItem += "xi_thumblx+";
        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbY ) > m_senseLimit )
          && ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbY ) > Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbX ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.LeftThumb ) ) m_lastItem += "xi_thumbly+";

        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbX ) > m_senseLimit )
          && ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbX ) > Math.Abs( ( Int32 )m_state.Gamepad.RightThumbY ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) m_lastItem += "xi_thumbrx+";
        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbY ) > m_senseLimit )
          && ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbY ) > Math.Abs( ( Int32 )m_state.Gamepad.RightThumbX ) )
          && !Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.RightThumb ) ) m_lastItem += "xi_thumbry+";

        if ( Math.Abs( ( Int32 )m_state.Gamepad.LeftTrigger ) > 0 ) m_lastItem += "xi_triggerl_btn+";
        if ( Math.Abs( ( Int32 )m_state.Gamepad.RightTrigger ) > 0 ) m_lastItem += "xi_triggerr_btn+";

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
        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbX ) > m_senseLimit ) 
          && ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbX ) > Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbY )) ) m_lastItem = "xi_thumblx+";
        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbY ) > m_senseLimit ) 
          && ( Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbY ) > Math.Abs( ( Int32 )m_state.Gamepad.LeftThumbX )) ) m_lastItem = "xi_thumbly+";

        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbX ) > m_senseLimit )
          && ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbX ) > Math.Abs( ( Int32 )m_state.Gamepad.RightThumbY ) ) ) m_lastItem = "xi_thumbrx+";
        if ( ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbY ) > m_senseLimit ) 
          && ( Math.Abs( ( Int32 )m_state.Gamepad.RightThumbY ) > Math.Abs( ( Int32 )m_state.Gamepad.RightThumbX ) ) ) m_lastItem = "xi_thumbry+";

        if ( Math.Abs( ( Int32 )m_state.Gamepad.LeftTrigger ) > 0 ) m_lastItem = "xi_triggerl_btn+";
        if ( Math.Abs( ( Int32 )m_state.Gamepad.RightTrigger ) > 0 ) m_lastItem = "xi_triggerr_btn+";
      }

      return m_lastItem.TrimEnd( new char[] { '+' } ); ;
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
      int change = (Math.Abs( current ) - Math.Abs( prev ) ) / 32;
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


      String buttons = "";
      buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.A ) ) ? "A" : "_";
      buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.B ) ) ? "B" : "_";
      buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.X ) ) ? "X" : "_";
      buttons += ( Bit( m_state.Gamepad.Buttons, GamepadButtonFlags.Y ) ) ? "Y" : "_";
      m_gPanel.Button = buttons;
    }



    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public override void GetData( )
    {
      // Make sure there is a valid device.
      if ( m_device == null )
        return;

      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;

      // Poll the device for info.
      try {
        m_state = m_device.GetState( );
      }
      catch ( SharpDXException e ) {
        log.Error( "Gamepad-GetData: Unexpected Poll Exception", e );
        return;  // EXIT see ex code
      }
      UpdateUI( ); // and update the GUI
    }



  }
}
