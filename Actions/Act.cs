using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SCJMapper_V2.Common;
using SCJMapper_V2.Devices;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Common Device Handling items for Action Classes
  /// </summary>
  public class Act
  {
    /// <summary>
    /// Device Enums to be used within Action Classes
    /// </summary>
    public enum ActionDevice
    {
      AD_Unknown = -1,
      AD_Joystick = 0,
      AD_Gamepad,
      AD_Keyboard,
      AD_Mouse,   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
    }

    // Static items to have this device mappings in only one place

    #region Static Items

    // device naming:
    //    ADevice  - an enum from ActionDevice (above) used internally by the Action classes
    //    DevTag   - one letter leader (K,J,X...) used to Key items

    //    DeviceClass - defined in class DeviceCls and inheritors (name used in XMLs)
    //    Input, devInput - the part of the input used in XML that defines the device e.g.  jsN_, mo1_, kb1_, xi1_

    /// <summary>
    /// Return the Device Enum from a DeviceClass string
    /// </summary>
    /// <param name="deviceClass">Device Class string</param>
    /// <returns>Device Enum</returns>
    static public ActionDevice ADevice( string deviceClass )
    {
      switch ( deviceClass.ToLower( ) ) {
        case KeyboardCls.DeviceClass: return ActionDevice.AD_Keyboard;
        case JoystickCls.DeviceClass: return ActionDevice.AD_Joystick;
        case GamepadCls.DeviceClass: return ActionDevice.AD_Gamepad;
        case MouseCls.DeviceClass: return ActionDevice.AD_Mouse;   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        case "ps3pad": return ActionDevice.AD_Gamepad;
        default: return ActionDevice.AD_Unknown;
      }
    }

    /// <summary>
    /// Returns the Device Tag i.e. the single letter to mark a device in Actions
    /// </summary>
    /// <param name="device">The device name from the defaultProfile</param>
    /// <returns>The single UCase device Tag letter</returns>
    static public string DevTag( string device )
    {
      switch ( device.ToLower( ) ) {
        case KeyboardCls.DeviceClass: return "K";
        case JoystickCls.DeviceClass: return "J";
        case GamepadCls.DeviceClass: return "X";
        case MouseCls.DeviceClass: return "M";  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        case "ps3pad": return "P";
        default: return "Z";
      }
    }

    /// <summary>
    /// Returns the Device name from the Device Tag
    /// </summary>
    /// <param name="device">The single UCase device Tag letter</param>
    /// <returns>The device name from the defaultProfile</returns>
    static public string DeviceClassFromTag( string devTag )
    {
      switch ( devTag ) {
        case "K": return KeyboardCls.DeviceClass;
        case "J": return JoystickCls.DeviceClass;
        case "X": return GamepadCls.DeviceClass;
        case "M": return MouseCls.DeviceClass;  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        case "P": return "ps3pad";
        default: return "unknown";
      }
    }


    /// <summary>
    /// Try to derive the device class from the devInput string (mo1_, kb1_, xi1_, jsN_)
    /// </summary>
    /// <param name="devInput">The input command string dev_input format</param>
    /// <returns>A proper DeviceClass string</returns>
    static public string DeviceClassFromInput( string devInput )
    {
      string deviceClass = DeviceCls.DeviceClass;

      deviceClass = JoystickCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      deviceClass = GamepadCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      deviceClass = KeyboardCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      deviceClass = MouseCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      // others..
      return deviceClass;
    }

    /// <summary>
    /// Returns the ActionDevice from a deviceID (a trailing _ is added if not there)
    /// </summary>
    /// <param name="devID">DeviceID</param>
    /// <returns>The ActionDevice</returns>
    static public ActionDevice ADeviceFromDevID( string devID )
    {
      string val = devID;
      if ( !devID.EndsWith( "_" ) ) val += "_";
      return ADevice( DeviceClassFromInput( val ) );
    }

    /// <summary>
    /// Returns the ActionDevice from the devInput string (mo1_, kb1_, xi1_, jsN_)
    /// </summary>
    /// <param name="devInput">The input command string dev_input format</param>
    /// <returns>The ActionDevice</returns>
    static public ActionDevice ADeviceFromInput( string devInput )
    {
      return ADevice( DeviceClassFromInput( devInput ) );
    }

    /// <summary>
    /// Query the devices if the input is disabled
    /// </summary>
    /// <param name="input">The input command</param>
    /// <returns>True if disabled input</returns>
    static public bool IsDisabledInput( string input )
    {
      bool disabledInput = false;

      disabledInput = DeviceCls.IsDisabledInput( input ); // generic
      if ( disabledInput ) return disabledInput;

      disabledInput = JoystickCls.IsDisabledInput( input );
      if ( disabledInput ) return disabledInput;
      disabledInput = GamepadCls.IsDisabledInput( input );
      if ( disabledInput ) return disabledInput;
      disabledInput = KeyboardCls.IsDisabledInput( input );
      if ( disabledInput ) return disabledInput;
      disabledInput = MouseCls.IsDisabledInput( input );
      if ( disabledInput ) return disabledInput;
      // others..
      return disabledInput;
    }

    /// <summary>
    /// Disable the input for a specific device - input is a generic Disabled Input
    /// </summary>
    /// <param name="input">An input (generic disable or a valid command)</param>
    /// <param name="aDevice">A valid device</param>
    /// <returns>A device diabled or the original input if it was not a disabled</returns>
    static public string DisableInput( string input, ActionDevice aDevice )
    {
      if ( DeviceCls.IsDisabledInput( input ) ) {
        // was generic blind - return a device specific disabled input
        switch ( aDevice ) {
          case ActionDevice.AD_Gamepad: return GamepadCls.DisabledInput;
          case ActionDevice.AD_Joystick: return JoystickCls.DisabledInput;
          case ActionDevice.AD_Keyboard: return KeyboardCls.DisabledInput;
          case ActionDevice.AD_Mouse: return MouseCls.DisabledInput;
          default: return "";
        }
      }
      else {
        return input; // not disabled - just return the input
      }
    }

    /// <summary>
    /// Extends the input to a device input if not already done
    /// </summary>
    /// <param name="input">An input</param>
    /// <param name="aDevice">The ActionDevice</param>
    /// <returns>A valid devInput (dev_input) format</returns>
    static public string DevInput( string input, ActionDevice aDevice )
    {
      switch ( aDevice ) {
        case ActionDevice.AD_Gamepad: return GamepadCls.DevInput( input );
        case ActionDevice.AD_Joystick: return JoystickCls.DevInput( input );
        case ActionDevice.AD_Keyboard: return KeyboardCls.DevInput( input );
        case ActionDevice.AD_Mouse: return MouseCls.DevInput( input );
        default: return input;
      }
    }

    /// <summary>
    /// Return the color of a device
    /// </summary>
    /// <param name="devInput">The devinput (determine JS colors)</param>
    /// <param name="aDevice">The ActionDevice</param>
    /// <returns>The device color</returns>
    static public System.Drawing.Color DeviceColor( string devInput )
    {
      // background is along the input 
      ActionDevice aDevice = ADeviceFromInput( devInput );
      switch ( aDevice ) {
        case ActionDevice.AD_Gamepad: return GamepadCls.XiColor( );
        case ActionDevice.AD_Joystick: return JoystickCls.JsNColor( JoystickCls.JSNum( devInput ) );// need to know which JS 
        case ActionDevice.AD_Keyboard: return KeyboardCls.KbdColor( );
        case ActionDevice.AD_Mouse: return MouseCls.MouseColor( );
        default: return MyColors.UnassignedColor;
      }
    }


    #endregion

  }
}
