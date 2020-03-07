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
    public string InputType { get; set; } = "";       // K, M, J, G (keyb, mouse, joystick, gamepad)
    // Command Input Ref - match required to find the display location
    public string ControlInput { get; set; } = "";    // buttonN, hatN_up,_right,_down,_left, [rot]xyz, sliderN (CryInput notification)

  }
}
