using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.SCJServer
{
  /// <summary>
  /// Convert from game key codes to VK_ codes (WinUser names for the server)
  /// </summary>
  class KbdCodes
  {
    public static string FromSCKeyboardMod( string scMod )
    {
      if ( scMod == "lalt" ) return "la&";
      if ( scMod == "ralt" ) return "ra&";
      if ( scMod == "lctrl" ) return "lc&";
      if ( scMod == "rctrl" ) return "rc&";
      if ( scMod == "lshift" ) return "ls&";
      if ( scMod == "rshift" ) return "rd&";
      return "";
    }

    /// <summary>
    /// Converts from SC command to VK_ command
    /// </summary>
    /// <param name="scKey">A single SC game keyname</param>
    /// <returns>The VK_Code of this key</returns>
    static public string FromSCKeyboardCmd( string scKey )
    {
      switch ( scKey ) {
        // handle modifiers first
        case "lalt": return "VK_LALT";
        case "ralt": return "VK_RALT";
        case "lshift": return "VK_LSHIFT";
        case "rshift": return "VK_RSHIFT";
        case "lctrl": return "VK_LCONTROL";
        case "rctrl": return "VK_RCONTROL";

        // function keys first 
        case "f1": return "VK_F1";
        case "f2": return "VK_F2";
        case "f3": return "VK_F3";
        case "f4": return "VK_F4";
        case "f5": return "VK_F5";
        case "f6": return "VK_F6";
        case "f7": return "VK_F7";
        case "f8": return "VK_F8";
        case "f9": return "VK_F9";
        case "f10": return "VK_F10";
        case "f11": return "VK_F11";
        case "f12": return "VK_F12";
        case "f13": return "VK_F13";
        case "f14": return "VK_F14";
        case "f15": return "VK_F15";

        // all keys where the DX name does not match the SC name
        // Numpad
        case "numlock": return "VK_NUMLOCK";
        case "np_divide": return "VK_NP_DIVIDE";
        case "np_multiply": return "VK_NP_MULTIPLY";
        case "np_subtract": return "VK_NP_SUBTRACT";
        case "np_add": return "VK_NP_ADD";
        case "np_period": return "VK_NP_PERIOD";
        case "np_enter": return "VK_NP_ENTER";
        case "np_0": return "VK_NP_0";
        case "np_1": return "VK_NP_1";
        case "np_2": return "VK_NP_2";
        case "np_3": return "VK_NP_3";
        case "np_4": return "VK_NP_4";
        case "np_5": return "VK_NP_5";
        case "np_6": return "VK_NP_6";
        case "np_7": return "VK_NP_7";
        case "np_8": return "VK_NP_8";
        case "np_9": return "VK_NP_9";
        // Digits
        case "0": return "VK_0";
        case "1": return "VK_1";
        case "2": return "VK_2";
        case "3": return "VK_3";
        case "4": return "VK_4";
        case "5": return "VK_5";
        case "6": return "VK_6";
        case "7": return "VK_7";
        case "8": return "VK_8";
        case "9": return "VK_9";
        // letters - the easy way..
        case "a": return "VK_A";
        case "b": return "VK_B";
        case "c": return "VK_C";
        case "d": return "VK_D";
        case "e": return "VK_E";
        case "f": return "VK_F";
        case "g": return "VK_G";
        case "h": return "VK_H";
        case "i": return "VK_I";
        case "j": return "VK_J";
        case "k": return "VK_K";
        case "l": return "VK_L";
        case "m": return "VK_M";
        case "n": return "VK_N";
        case "o": return "VK_O";
        case "p": return "VK_P";
        case "q": return "VK_Q";
        case "r": return "VK_R";
        case "s": return "VK_S";
        case "t": return "VK_T";
        case "u": return "VK_U";
        case "v": return "VK_V";
        case "w": return "VK_W";
        case "x": return "VK_X";
        case "y": return "VK_Y";
        case "z": return "VK_Z";
        // navigation
        case "insert": return "VK_INSERT";
        case "home": return "VK_HOME";
        case "delete": return "VK_DELETE";
        case "end": return "VK_END";
        case "pgup": return "VK_PGUP";
        case "pgdown": return "VK_PGDN";
        case "print": return "VK_PRINTSCREEN";
        case "scrolllock": return "VK_SCROLLOCK";
        case "pause": return "VK_PAUSE";
        // Arrows
        case "up": return "VK_UPARROW";
        case "down": return "VK_DOWNARROW";
        case "left": return "VK_LEFTARROW";
        case "right": return "VK_RIGHTARROW";
        // non letters
        case "escape": return "VK_ESCAPE";
        case "minus": return "VK_MINUS";
        case "equals": return "VK_EQUALS";
        case "backspace": return "VK_BACK";
        case "tab": return "VK_TAB";
        case "lbracket": return "VK_LBRACKET";
        case "rbracket": return "VK_RBRACKET";
        case "enter": return "VK_RETURN";
        case "capslock": return "VK_CAPSLOCK";
        case "backslash": return "VK_BACKSLASH";
        case "comma": return "VK_COMMA";
        case "period": return "VK_PERIOD";
        case "slash": return "VK_SLASH";
        case "space": return "VK_SPACE";
        case "semicolon": return "VK_SEMICOLON";
        case "apostrophe": return "VK_APOSTROPHE";

        // all where the lowercase DX name matches the SC name
        default:
          return "";
      }
    }

  }

}

