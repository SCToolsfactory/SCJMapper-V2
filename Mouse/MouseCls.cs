using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.DirectInput;
using System.Windows.Forms;
using SharpDX;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SCJMapper_V2
{
  /// <summary>
  /// Handles one Mouse device as DXInput device
  /// In addition provide some static tools to handle Mouse props here in one place
  /// </summary>
  public class MouseCls : DeviceCls
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );
    private static readonly AppSettings  appSettings = new AppSettings( );

    #region Static Items

    public new const String DeviceClass = "mouse";   // the device name used throughout this app
    public new const String DeviceID = "mo1_";
    static public int RegisteredDevices = 0;  // devices add here once they are created (though will not decrement as they are not deleted)

    public new const String BlendedInput = DeviceID + DeviceCls.BlendedInput;
    static public new Boolean IsBlendedInput( String input )
    {
      if ( input == BlendedInput ) return true;
      return false;
    }


    /// <summary>
    /// Returns the currently valid color
    /// </summary>
    /// <returns>A color</returns>
    static public System.Drawing.Color MouseColor( )
    {
      return MyColors.MouseColor;
    }


    /// <summary>
    /// Returns true if the devicename is a joystick
    /// </summary>
    /// <param name="deviceClass"></param>
    /// <returns></returns>
    static new public Boolean IsDeviceClass( String deviceClass )
    {
      return ( deviceClass == DeviceClass );
    }

    /// <summary>
    /// Return this deviceClass if the input string starts with mo1_
    /// </summary>
    /// <param name="devInput"></param>
    /// <returns></returns>
    static public new String DeviceClassFromInput( String devInput )
    {
      if ( DevMatch( devInput ) )
        return DeviceClass; // this 
      else
        return DeviceCls.DeviceClass; // unknown
    }

    /// <summary>
    /// Create a DevInput string if the input does look like not having a device ID
    /// </summary>
    /// <param name="input">A mouse input</param>
    /// <returns>DevInput</returns>
    static public new String DevInput( String input )
    {
      if ( DevMatch( input ) )
        return input; // already
      else
        return DeviceID + input;
    }

    /// <summary>
    /// Returns true if the input matches this device
    /// </summary>
    /// <param name="devInput">A devInput string</param>
    /// <returns>True for a match</returns>
    static public new Boolean DevMatch( String devInput )
    {
      return devInput.StartsWith( DeviceID );
    }


    /// <summary>
    /// Reformat the input from AC1 style to AC2 style
    /// </summary>
    /// <param name="input">The AC1 input string</param>
    /// <returns>An AC2 style input string</returns>
    static public String FromAC1( String input )
    {
      // input is something like a mouse1 (TODO compositions like lctrl+mouse1 ??)
      // try easy: add mo1_ at the beginning
      String retVal = input.Replace(" ","");
      if ( IsBlendedInput( input ) ) return input;

      return "mo1_" + retVal;
    }


    /// <summary>
    /// Format the various parts to a valid ctrl entry
    /// </summary>
    /// <param name="input">The input by the user</param>
    /// <param name="modifiers">Modifiers to be applied</param>
    /// <returns></returns>
    static public String MakeCtrl( String input, String modifiers )
    {
      return DeviceID + modifiers + input;
    }

    #endregion

    private Mouse m_device;
    private MouseState m_state = new MouseState( );
    private MouseState m_prevState = new MouseState( );

    private Control m_hwnd;
    private bool m_activated = false;

    private String m_lastItem = "";
    private int m_senseLimit = 150; // axis jitter avoidance...

    /// <summary>
    /// The DeviceClass of this instance
    /// </summary>
    public override String DevClass { get { return MouseCls.DeviceClass; } }
    /// <summary>
    /// The JS ProductName property
    /// </summary>
    public override String DevName { get { return m_device.Properties.ProductName; } }
    /// <summary>
    /// The JS Instance GUID for multiple device support (VJoy gets 2 of the same name)
    /// </summary>
    public String DevInstanceGUID { get { return m_device.Information.InstanceGuid.ToString( ); } }

    /// <summary>
    /// Returns the mapping color for this device
    /// </summary>
    public override System.Drawing.Color MapColor
    {
      get { return MyColors.MouseColor; }
    }


    public override Boolean Activated
    {
      get { return m_activated; }
      set
      {
        m_activated = value;
        if ( m_activated == false ) m_device.Unacquire( ); // explicitely if not longer active
      }
    }

    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    public MouseCls( Mouse device, Control hwnd )
    {
      log.DebugFormat( "MouseCls cTor - Entry with {0}", device.Information.ProductName );

      m_device = device;
      m_hwnd = hwnd;
      Activated = false;

      m_senseLimit = AppConfiguration.AppConfig.msSenseLimit; // can be changed in the app.config file if it is still too little

      // Set BufferSize in order to use buffered data.
      m_device.Properties.BufferSize = 128;

      log.Debug( "Get Mouse Object" );
      try {
        // Set the data format to the c_dfDIJoystick pre-defined format.
        //m_device.SetDataFormat( DeviceDataFormat.Joystick );
        // Set the cooperative level for the device.
        m_device.SetCooperativeLevel( m_hwnd, CooperativeLevel.NonExclusive | CooperativeLevel.Background );
        // Enumerate all the objects on the device.
      }
      catch ( Exception ex ) {
        log.Error( "Get Mouse Object failed", ex );
      }

      MouseCls.RegisteredDevices++;

      Activated = true;
    }



    public void Deactivate( )
    {
      this.Activated = false;
    }
    public void Activate( )
    {
      this.Activated = true;
    }

    public int NumberOfButtons { get { return m_state.Buttons.Length; } }


    /// <summary>
    /// Find the last change the user did on that device
    /// maxis_x, maxis_y, maxis_z, mwheel_up, mwheel_down, mouse1, mouse2, mouse3, mouse4, mouse5, possibly mouseN
    /// Modifiers: NO modifiers found in defaultProfile...
    /// Z-axis, typically a wheel. If the mouse does not have a z-axis, the value is 0. 
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public override String GetLastChange( )
    {
      // TODO: Expand this out into a joystick class (see commit for details)
      Dictionary<string, string> axies = new Dictionary<string, string>( )
        {
          {"X","maxis_x"},
          {"Y","maxis_y"},
          {"Z","mwheel_"},
        };

      foreach ( KeyValuePair<string, string> entry in axies ) {
        PropertyInfo axisProperty = typeof( MouseState ).GetProperty( entry.Key );

        if ( DidAxisChange2( ( int )axisProperty.GetValue( this.m_state, null ), ( int )axisProperty.GetValue( this.m_prevState, null ), true ) ) {
          this.m_lastItem = entry.Value;
          if ( entry.Key == "Z" ) this.m_lastItem += "down";
        }
        else if ( DidAxisChange2( ( int )axisProperty.GetValue( this.m_state, null ), ( int )axisProperty.GetValue( this.m_prevState, null ), false ) ) {
          this.m_lastItem = entry.Value;
          if ( entry.Key == "Z" ) this.m_lastItem += "up";
        }
      }

      bool[] buttons = m_state.Buttons;
      bool[] prevButtons = m_prevState.Buttons;
      for ( int bi = 0; bi < buttons.Length; bi++ ) {
        if ( buttons[bi] && buttons[bi] != prevButtons[bi] )
          m_lastItem = "mouse" + ( bi + 1 ).ToString( );
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
    private bool DidAxisChange2( int current, int prev, bool posAxis )
    {
      // determine if the axis drifts more than x units to account for bounce
      // old-new/old
      if ( current == prev )
        return false;
      int change = Math.Abs( prev - current );
      // if the axis has changed more than x units to it's last value
      if ( posAxis && ( current - prev ) > 0 )
        return change > 2 ? true : false;
      else if ( ( !posAxis ) && ( current - prev ) < 0 )
        return change > 2 ? true : false;
      else
        return false;
    }



    /// <summary>
    /// Collect the current data from the device (DUMMY for Mouse)
    /// </summary>
    public override void GetCmdData( String cmd, out int data )
    {
      // Make sure there is a valid device.
      data = 0;
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

    }
















    // mwheel_up, mwheel_down

    public static String MouseCmd( MouseEventArgs e )
    {
      String mbs = "";
      switch ( e.Button ) {
        case MouseButtons.Left: {
            mbs = "mouse1";
            break;
          }
        case MouseButtons.Middle: {
            mbs = "mouse3";
            break;
          }
        case MouseButtons.Right: {
            mbs = "mouse2";
            break;
          }
        case MouseButtons.XButton1: {
            mbs = "mouse4";
            break;
          }
        case MouseButtons.XButton2: {
            mbs = "mouse5";
            break;
          }
      }
      return mbs;
    }

  }
}
