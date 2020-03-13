using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// One Action Item for the Layout process
  /// </summary>
  class ActionItem
  {
    /// <summary>
    /// The Text Shown in the Map
    /// </summary>
    public string DispText { get; set; }
    /// <summary>
    /// The action map this item belongs to
    /// </summary>
    public string ActionMap { get; set; } = ""; // TODO may be set a color for this one later

    // Input Device Refs
    public string DeviceName { get; set; } = "";      // Device Name
    public string DeviceProdGuid { get; set; } = "";  // Device Product GUID
    public string InputType { get; set; } = "";       // K1, M1, Jn, G1 (keyb, mouse, joystick jsN number 1.., gamepad)
    // Command Input Ref - match required to find the display location
    public string ControlInput { get; set; } = "";    // buttonN, hatN_up,_right,_down,_left, [rot]xyz, sliderN (CryInput notification)

    /// <summary>
    /// Returnd the PID VID part of the device GUID in lowercase
    ///  or an empty string...
    /// </summary>
    public string DevicePidVid
    {
      get {
        if ( DeviceProdGuid.Length > 9 ) {
          return DeviceProdGuid.Substring( 1, 8 ).ToLowerInvariant( );
        }
        return DeviceProdGuid;
      }
    }

    public string InputTypeLetter { get => InputType.Substring( 0, 1 ); }
    public short InputTypeNumber { get => short.Parse( InputType.Substring( 1 ) ); } // cannot fail else we have a program error...

    /// <summary>
    /// Returns the Modifier for this item
    /// i.e. only modifiers
    /// </summary>
    public string Modifier
    {
      get {
        // input can be:  {modifier+}Input
        if ( !ControlInput.Contains( "+" ) ) return ""; // no modifier

        string[] e = ControlInput.Split( new char[] { '+' } );
        string mod = "";
        for ( int i = 0; i < e.Length - 1; i++ ) {
          mod += MapProps.ModS( e[i] );
        }
        return "(" + mod + ")";
      }
    }

    /// <summary>
    /// Returns the Main Control for this item
    /// i.e. no modifiers
    /// </summary>
    public string MainControl
    {
      get {
        // input can be:  {modifier+}Input
        if ( !ControlInput.Contains( "+" ) ) return ControlInput; // no modifier

        string[] e = ControlInput.Split( new char[] { '+' } );
        return e[e.Length - 1]; // last item
      }
    }

    /// <summary>
    /// Returns the DispText with Modding added
    /// </summary>
    public string ModdedDispText
    {
      get {
        return Modifier+DispText;
      }
    }


  }
}
