using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.DirectInput;
using System.Windows.Forms;
using SharpDX;
using System.Reflection;
using System.Text.RegularExpressions;

using SCJMapper_V2.Common;

namespace SCJMapper_V2.Devices.Mouse
{
  /// <summary>
  /// Handles one Mouse device as DXInput device
  /// In addition provide some static tools to handle Mouse props here in one place
  /// </summary>
  public class MouseCls : DeviceCls
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    #region Static Items

    public new const string DeviceClass = "mouse";   // the device name used throughout this app
    public new const string DeviceID = "mo1_";
    static public int RegisteredDevices = 0;  // devices add here once they are created (though will not decrement as they are not deleted)

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
    static public System.Drawing.Color MouseColor()
    {
      return MyColors.MouseColor;
    }


    /// <summary>
    /// Returns true if the devicename is a joystick
    /// </summary>
    /// <param name="deviceClass"></param>
    /// <returns></returns>
    static new public bool IsDeviceClass( string deviceClass )
    {
      return ( deviceClass == DeviceClass );
    }

    /// <summary>
    /// Return this deviceClass if the input string starts with mo1_
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
    /// <param name="input">A mouse input</param>
    /// <returns>DevInput</returns>
    static public new string DevInput( string input )
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
    static public new bool DevMatch( string devInput )
    {
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
      return ( cLower.EndsWith( "_maxis_x" ) || cLower.EndsWith( "_maxis_y" ) );
    }

    /// <summary>
    /// Reformat the input from AC1 style to AC2 style
    /// </summary>
    /// <param name="input">The AC1 input string</param>
    /// <returns>An AC2 style input string</returns>
    static public string FromAC1( string input )
    {
      // input is something like a mouse1 (TODO compositions like lctrl+mouse1 ??)
      // try easy: add mo1_ at the beginning
      string retVal = input.Replace( " ", "" );
      if ( IsDisabledInput( input ) ) return input;

      return "mo1_" + retVal;
    }


    /// <summary>
    /// Format the various parts to a valid ctrl entry
    /// </summary>
    /// <param name="input">The input by the user</param>
    /// <param name="modifiers">Modifiers to be applied</param>
    /// <returns></returns>
    static public string MakeCtrl( string input, string modifiers )
    {
      return DeviceID + modifiers + input;
    }

    #endregion

    private SharpDX.DirectInput.Mouse m_device;
    private MouseState m_state = new MouseState( );
    private MouseState m_prevState = new MouseState( );

    private IntPtr m_hwnd;
    private bool m_activated = false;

    private string m_lastItem = "";
    private int m_senseLimit = 150; // axis jitter avoidance...

    /// <summary>
    /// Return the device instance number (which is always 1)
    /// </summary>
    public override int XmlInstance { get { return 1; } } // const 1 for mouse
    /// <summary>
    /// Return the DX device instance number (which is always 0)
    /// </summary>
    public override int DevInstance { get { return 0; } }
    /// <summary>
    /// The DeviceClass of this instance
    /// </summary>
    public override string DevClass { get { return MouseCls.DeviceClass; } }
    /// <summary>
    /// The JS ProductName property
    /// </summary>
    public override string DevName { get { return "Mouse"; } } // no props in directX
    /// <summary>
    /// The ProductGUID property
    /// </summary>
    public override string DevGUID { get { return $"{{{m_device.Information.ProductGuid.ToString( )}}}"; } } // @@@ tbd 
    /// <summary>
    /// The JS Instance GUID for multiple device support (VJoy gets 2 of the same name)
    /// </summary>
    public override string DevInstanceGUID { get { return m_device.Information.InstanceGuid.ToString( ); } }

    /// <summary>
    /// Returns the mapping color for this device
    /// </summary>
    public override System.Drawing.Color MapColor
    {
      get { return MyColors.MouseColor; }
    }


    public override bool Activated
    {
      get { return Activated_low; }
      set { Activated_low = value; }
    }

    private bool Activated_low
    {
      get { return m_activated; }
      set {
        m_activated = value;
        if ( m_activated == false ) m_device.Unacquire( ); // explicitely if not longer active
      }
    }

    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    public MouseCls( SharpDX.DirectInput.Mouse device, IntPtr hwnd )
    {
      log.DebugFormat( "MouseCls cTor - Entry with {0}", device.Information.ProductName );

      m_device = device;
      m_hwnd = hwnd;
      Activated_low = false;

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

      Activated_low = true;
    }

    /// <summary>
    /// Returns the number of buttons
    /// </summary>
    public int NumberOfButtons { get { return m_state.Buttons.Length; } }

    // Property Mapping from DXinput to CryEngine string
    private Dictionary<string, string> m_axiesDx2Cry = new Dictionary<string, string>( )
      {
          {"X","maxis_x"},
          {"Y","maxis_y"},
          {"Z","mwheel_"},
        };

    /// <summary>
    /// returns the currently available input string
    ///  (does not retrieve new data but uses what was collected by GetData())
    ///  NOTE: for Mouse when multiple inputs are available the sequence is 
    ///    axis > button > hat > slider (wher prio is max itemNum > min itemNum)
    /// </summary>
    /// <returns>An input string or an empty string if no input is available</returns>
    public override string GetCurrentInput()
    {
      string currentChange = "";

      // get axis
      foreach ( KeyValuePair<string, string> entry in m_axiesDx2Cry ) {
        PropertyInfo axisProperty = typeof( MouseState ).GetProperty( entry.Key );
        if ( DidAxisChange2( (int)axisProperty.GetValue( m_state, null ), (int)axisProperty.GetValue( m_prevState, null ), true ) ) {
          currentChange = entry.Value;
          if ( entry.Key == "Z" ) currentChange += "down";
        }
        else if ( DidAxisChange2( (int)axisProperty.GetValue( m_state, null ), (int)axisProperty.GetValue( m_prevState, null ), false ) ) {
          currentChange = entry.Value;
          if ( entry.Key == "Z" ) currentChange += "up";
        }
      }
      // get prio button
      bool[] buttons = m_state.Buttons;
      bool[] prevButtons = m_prevState.Buttons;
      for ( int bi = 0; bi < buttons.Length; bi++ ) {
        if ( buttons[bi] )
          currentChange = "mouse" + ( bi + 1 ).ToString( );
      }

      return currentChange;
    }

    /// <summary>
    /// Find the last change the user did on that device
    /// maxis_x, maxis_y, maxis_z, mwheel_up, mwheel_down, mouse1, mouse2, mouse3, mouse4, mouse5, possibly mouseN
    /// Modifiers: NO modifiers found in defaultProfile...
    /// Z-axis, typically a wheel. If the mouse does not have a z-axis, the value is 0. 
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public override string GetLastChange()
    {
      // get changed axis
      foreach ( KeyValuePair<string, string> entry in m_axiesDx2Cry ) {
        PropertyInfo axisProperty = typeof( MouseState ).GetProperty( entry.Key );
        if ( DidAxisChange2( (int)axisProperty.GetValue( m_state, null ), (int)axisProperty.GetValue( m_prevState, null ), true ) ) {
          m_lastItem = entry.Value;
          if ( entry.Key == "Z" ) m_lastItem += "down";
        }
        else if ( DidAxisChange2( (int)axisProperty.GetValue( m_state, null ), (int)axisProperty.GetValue( m_prevState, null ), false ) ) {
          m_lastItem = entry.Value;
          if ( entry.Key == "Z" ) m_lastItem += "up";
        }
      }

      // get new button
      bool[] buttons = m_state.Buttons;
      bool[] prevButtons = m_prevState.Buttons;
      for ( int bi = 0; bi < buttons.Length; bi++ ) {
        if ( buttons[bi] && ( buttons[bi] != prevButtons[bi] ) )
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


    System.Drawing.Rectangle m_targetRect = Screen.PrimaryScreen.Bounds;
    /// <summary>
    /// Fudge - must have a target rectangle to scale the mouse input into the target
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetRectForCmdData( System.Drawing.Rectangle target )
    {
      m_targetRect = target;
    }
    /// <summary>
    /// Collect the current data from the device (using WinForms.Cursor)
    /// </summary>
    public override void GetCmdData( string cmd, out int data )
    {
      System.Drawing.Point cPt = Cursor.Position;
      // somewhere on all screens 
      if ( m_targetRect.Contains( cPt ) ) {
        cPt = cPt - new System.Drawing.Size( m_targetRect.X, m_targetRect.Y ); // move the point relative to the target rect origin
        switch ( cmd ) {
          case "maxis_x": data = (int)( 2000 * cPt.X / m_targetRect.Width ) - 1000; break; // data should be -1000..1000
          case "maxis_y": data = -1 * ( (int)( 2000 * cPt.Y / m_targetRect.Height ) - 1000 ); break; // data should be -1000..1000
          default: data = 0; break;
        }
      }
      else {
        data = 0;
      }

      System.Diagnostics.Debug.Print( string.Format( "C:({0})-T({1})({2}) - data: {3}",
        Cursor.Position.ToString( ),
        m_targetRect.Location.ToString( ), m_targetRect.Size.ToString( ),
        data.ToString( ) ) );
    }


    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public override void GetData()
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

  }
}
