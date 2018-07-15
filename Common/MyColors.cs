using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SCJMapper_V2.Common
{
  /// <summary>
  /// Provide the colors used 
  /// </summary>
  class MyColors
  {
    static public readonly Color[] InitColor = { Color.LightGreen, Color.LightBlue, Color.Khaki, Color.LightSalmon,
                                                 Color.Beige, Color.Yellow, Color.MintCream, Color.LightCyan,
                                                 Color.MistyRose, Color.Wheat, Color.Plum, Color.Pink };
    static public readonly Color GamepadColorDefault = Color.SandyBrown; // Must not be one of the Joysticks!! some cruel algo depends on it beeing different...

    static public Color[] TabColor = ( Color[] )InitColor.Clone( ); // the gamepad tab will get reassinged to Gamepad color on Init

    /// <summary>
    /// Colormap of the Joystick assignment - use (JsN-1) as index
    /// </summary>
    static public Color[] JsMapColor = ( Color[] )TabColor.Clone( ); // maps only Joystick colors

    /// <summary>
    /// Joystick color of the JsN assigned joystick instance
    /// </summary>
    /// <param name="jsN">The jsN number of the joystick</param>
    /// <returns>The (tab) color of this item</returns>
    static public Color JsColor(int jsN ) { return JsMapColor[jsN - 1]; }

    /// <summary>
    /// The Gamepad color
    /// </summary>
    static public Color GamepadColor = GamepadColorDefault;

    static public Color KeyboardColor = Color.Lavender;
    static public Color MouseColor = Color.Moccasin;
    static public Color BlendedColor = Color.LightGray;
    static public Color UnassignedColor = Color.White;

    static public Color DirtyColor = Color.Tomato;

    static public Color SuccessColor = Color.GreenYellow;
    static public Color ValidColor = Color.White;
    static public Color InvalidColor = Color.Tomato;
    static public Color ErrorColor = Color.Gold;

    static public Color ProfileColor = Color.DarkKhaki;
    static public Color MappingColor = Color.DarkSeaGreen;

  }
}
