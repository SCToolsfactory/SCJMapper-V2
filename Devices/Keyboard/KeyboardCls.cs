using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.DirectInput;
using System.Windows.Forms;
using SharpDX;
using System.Text.RegularExpressions;

using SCJMapper_V2.Common;

namespace SCJMapper_V2.Devices.Keyboard
{
  /// <summary>
  /// Handles one Keyboard device as DXInput device
  /// In addition provide some static tools to handle KBD props here in one place
  /// </summary>
  public class KeyboardCls : DeviceCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    #region Static Items

    public new const string DeviceClass = "keyboard";  // the device name used throughout this app
    public new const string DeviceID = "kb1_";
    static public int RegisteredDevices = 0;  // devices add here once they are created (though will not decrement as they are not deleted)

    public const string ClearMods = "escape";

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
    static public System.Drawing.Color KbdColor( )
    {
      return MyColors.KeyboardColor;
    }


    /// <summary>
    /// Returns true if the devicename is a joystick
    /// </summary>
    /// <param name="deviceClass"></param>
    /// <returns></returns>
    static public new bool IsDeviceClass( string deviceClass )
    {
      return ( deviceClass == DeviceClass );
    }

    /// <summary>
    /// Return this deviceClass if the input string starts with kb1_
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
    /// <param name="input">A keyboard input</param>
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
    /// Reformat the input from AC1 style to AC2 style
    /// </summary>
    /// <param name="input">The AC1 input string</param>
    /// <returns>An AC2 style input string</returns>
    static public string FromAC1( string input )
    {
      // input is something like a letter or a composition like lctrl+c 
      // try easy: add kb1_ at the beginning and before any +; first remove spaces
      string retVal = input.Replace(" ","");
      if ( IsDisabledInput( input ) ) return input;

      int plPos = retVal.IndexOf("+");
      while (plPos>0) {
        retVal.Insert( plPos + 1, "kb1_" );
        plPos = retVal.IndexOf("+", plPos+1);
      }
      return "kb1_" + retVal;
    }

    // See also SC keybinding_localization.xml

    // space, tab, semicolon, apostrophe, insert, left, right, up, down, home, pgup, pgdown, end, backspace
    // lbracket, rbracket,  np_0, np_1.., np_period, np_divide f1.., equal, minus, slash, comma, enter, backslash, equals, 
    // capslock
    // Modifiers: lalt, ralt, lctrl, rctrl (e.g. ralt+l, lshift+lctrl+1, lalt+lctrl+1)

    /// <summary>
    /// Translate the DX Keypressed list into SC keycode string
    /// </summary>
    /// <param name="pressedKeys">The list of pressed DX keys</param>
    /// <returns>The SC keycode string</returns>
    public static string DXKeyboardCmd( List<Key> pressedKeys, bool modAndKey )
    {
      string altMod = "";
      string shiftMod = "";
      string ctrlMod = "";
      string key = "";

      foreach ( Key k in pressedKeys ) {
        switch ( ( int )k ) {
          // handle modifiers first
          case ( int )Key.LeftAlt: altMod += "lalt+"; break;
          case ( int )Key.RightAlt: altMod += "ralt+"; break;
          case ( int )Key.LeftShift: shiftMod += "lshift+"; break;
          case ( int )Key.RightShift: shiftMod += "rshift+"; break;
          case ( int )Key.LeftControl: ctrlMod += "lctrl+"; break;
          case ( int )Key.RightControl: ctrlMod += "rctrl+"; break;

          // function keys first - modifier ??
          case (int)Key.F1: key += "f1+"; break;
          case (int)Key.F2: key += "f2+"; break;
          case (int)Key.F3: key += "f3+"; break;
          case (int)Key.F4: key += "f4+"; break;
          case (int)Key.F5: key += "f5+"; break;
          case (int)Key.F6: key += "f6+"; break;
          case (int)Key.F7: key += "f7+"; break;
          case (int)Key.F8: key += "f8+"; break;
          case (int)Key.F9: key += "f9+"; break;
          case (int)Key.F10: key += "f10+"; break;
          case (int)Key.F11: key += "f11+"; break;
          case (int)Key.F12: key += "f12+"; break;
          case (int)Key.F13: key += "f13+"; break;
          case (int)Key.F14: key += "f14+"; break;
          case (int)Key.F15: key += "f15+"; break;

          // all keys where the DX name does not match the SC name
          // Numpad
          case ( int )Key.NumberLock: key += "numlock+"; break;
          case ( int )Key.Divide: key += "np_divide+"; break;
          case ( int )Key.Multiply: key += "np_multiply+"; break;
          case ( int )Key.Subtract: key += "np_subtract+"; break;
          case ( int )Key.Add: key += "np_add+"; break;
          case ( int )Key.Decimal: key += "np_period+"; break;
          case ( int )Key.NumberPadEnter: key += "np_enter+"; break;
          case ( int )Key.NumberPad0: key += "np_0+"; break;
          case ( int )Key.NumberPad1: key += "np_1+"; break;
          case ( int )Key.NumberPad2: key += "np_2+"; break;
          case ( int )Key.NumberPad3: key += "np_3+"; break;
          case ( int )Key.NumberPad4: key += "np_4+"; break;
          case ( int )Key.NumberPad5: key += "np_5+"; break;
          case ( int )Key.NumberPad6: key += "np_6+"; break;
          case ( int )Key.NumberPad7: key += "np_7+"; break;
          case ( int )Key.NumberPad8: key += "np_8+"; break;
          case ( int )Key.NumberPad9: key += "np_9+"; break;
          // Digits
          case ( int )Key.D0: key += "0+"; break;
          case ( int )Key.D1: key += "1+"; break;
          case ( int )Key.D2: key += "2+"; break;
          case ( int )Key.D3: key += "3+"; break;
          case ( int )Key.D4: key += "4+"; break;
          case ( int )Key.D5: key += "5+"; break;
          case ( int )Key.D6: key += "6+"; break;
          case ( int )Key.D7: key += "7+"; break;
          case ( int )Key.D8: key += "8+"; break;
          case ( int )Key.D9: key += "9+"; break;
          // navigation
          case ( int )Key.Insert: key += "insert+"; break;
          case ( int )Key.Home: key += "home+"; break;
          case ( int )Key.Delete: key += "delete+"; break;
          case ( int )Key.End: key += "end+"; break;
          case ( int )Key.PageUp: key += "pgup+"; break;
          case ( int )Key.PageDown: key += "pgdown+"; break;
          case ( int )Key.PrintScreen: key += "print+"; break;
          case ( int )Key.ScrollLock: key += "scrolllock+"; break;
          case ( int )Key.Pause: key += "pause+"; break;
          // Arrows
          case ( int )Key.Up: key += "up+"; break;
          case ( int )Key.Down: key += "down+"; break;
          case ( int )Key.Left: key += "left+"; break;
          case ( int )Key.Right: key += "right+"; break;
          // non letters
          case ( int )Key.Escape: key += "escape+"; break;
          case ( int )Key.Minus: key += "minus+"; break;
          case ( int )Key.Equals: key += "equals+"; break;
          case ( int )Key.Grave: key += ""; break; // "grave+"; break; // reserved for Console 
          case ( int )Key.Underline: key += "underline+"; break;
          case ( int )Key.Back: key += "backspace+"; break;
          case ( int )Key.Tab: key += "tab+"; break;
          case ( int )Key.LeftBracket: key += "lbracket+"; break;
          case ( int )Key.RightBracket: key += "rbracket+"; break;
          case ( int )Key.Return: key += "enter+"; break;
          case ( int )Key.Capital: key += "capslock+"; break;
          case ( int )Key.Colon: key += "colon+"; break;
          case ( int )Key.Backslash: key += "backslash+"; break;
          case ( int )Key.Comma: key += "comma+"; break;
          case ( int )Key.Period: key += "period+"; break;
          case ( int )Key.Slash: key += "slash+"; break;
          case ( int )Key.Space: key += "space+"; break;
          case ( int )Key.Semicolon: key += "semicolon+"; break;
          case ( int )Key.Apostrophe: key += "apostrophe+"; break;

          // all where the lowercase DX name matches the SC name
          default:
            if ( ( ( int )k >= ( int )Key.Q ) && ( ( int )k <= ( int )Key.P ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...

            else if ( ( ( int )k >= ( int )Key.A ) && ( ( int )k <= ( int )Key.L ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...

            else if ( ( ( int )k >= ( int )Key.Z ) && ( ( int )k <= ( int )Key.M ) )
              key += k.ToString( ).ToLowerInvariant( ) + "+"; // ranges are based on the enum values...

            else { } // no other ones handled
            break;
        }

      }//for
      if ( modAndKey ) {
        key = altMod + shiftMod + ctrlMod + key;
      }
      else {
        // mods only OR space to kill mods
        if ( !key.Contains( ClearMods ) ) key = altMod + shiftMod + ctrlMod;
      }

      return key.TrimEnd( new char[] { '+' } );  // return killing the last +
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

    private SharpDX.DirectInput.Keyboard m_device;
    private KeyboardState m_state = new KeyboardState( );

    private Control m_hwnd;
    private bool m_activated = false;


    /// <summary>
    /// Return the device instance number (which is always 1)
    /// </summary>
    public override int XmlInstance { get { return 1; } } // const for keyboard
    /// <summary>
    /// Return the DX device instance number (which is always 0)
    /// </summary>
    public override int DevInstance { get { return 0; } }
    /// <summary>
    /// The DeviceClass of this instance
    /// </summary>
    public override string DevClass { get { return KeyboardCls.DeviceClass; } }
    /// <summary>
    /// The JS ProductName property
    /// </summary>
    public override string DevName { get { return m_device.Properties.ProductName; } }
    /// <summary>
    /// The JS Instance GUID for multiple device support (VJoy gets 2 of the same name)
    /// </summary>
    public override string DevInstanceGUID { get { return m_device.Information.InstanceGuid.ToString( ); } }

    /// <summary>
    /// Returns the mapping color for this device
    /// </summary>
    public override System.Drawing.Color MapColor
    {
      get { return MyColors.KeyboardColor; }
    }


    public override bool Activated
    {
      get { return Activated_low; }
      set { Activated_low = value; }
    }

    private bool Activated_low
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
    public KeyboardCls( SharpDX.DirectInput.Keyboard device, Control hwnd )
    {
      log.DebugFormat( "KeyboardCls ctor - Entry with {0}", device.Information.ProductName );

      m_device = device;
      m_hwnd = hwnd;
      Activated_low = false;

      // Set BufferSize in order to use buffered data.
      m_device.Properties.BufferSize = 128;

      log.Debug( "Get KBD Object" );
      try {
        // Set the data format to the c_dfDIJoystick pre-defined format.
        //m_device.SetDataFormat( DeviceDataFormat.Joystick );
        // Set the cooperative level for the device.
        m_device.SetCooperativeLevel( m_hwnd, CooperativeLevel.NonExclusive | CooperativeLevel.Background );
        // Enumerate all the objects on the device.
      }
      catch ( Exception ex ) {
        log.Error( "Get Keyboard Object failed", ex );
      }

      KeyboardCls.RegisteredDevices++;

      Activated_low = true;
    }



    public void Deactivate( )
    {
      this.Activated = false;
    }
    public void Activate( )
    {
      this.Activated = true;
    }

    /// <summary>
    /// Find the last change the user did on that device
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public override string GetLastChange( )
    {
      return DXKeyboardCmd( m_state.PressedKeys, true );
    }


    /// <summary>
    /// Find the last change the user did on that device
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public string GetLastChange( bool modAndKey )
    {
      return DXKeyboardCmd( m_state.PressedKeys, modAndKey );
    }


    /// <summary>
    /// Collect the current data from the device (DUMMY for Kbd)
    /// </summary>
    public override void GetCmdData( string cmd, out int data )
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
      try { m_state = m_device.GetCurrentState( ); }
      // Catch any exceptions. None will be handled here, 
      // any device re-aquisition will be handled above.  
      catch ( SharpDXException ) {
        return;
      }

    }


  }
}
