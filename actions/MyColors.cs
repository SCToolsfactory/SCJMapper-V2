using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SCJMapper_V2
{
  /// <summary>
  /// Provide the colors used 
  /// </summary>
  class MyColors
  {
    static public Color[] TabColor = { Color.LightGreen, Color.LightBlue, Color.Khaki, Color.LightSalmon,
                                       Color.Beige, Color.Yellow, Color.Plum, Color.MintCream,
                                       Color.LightCyan, Color.MistyRose, Color.Wheat, Color.Pink };
    static public Color GamepadColorDefault = Color.Fuchsia; // will be changed on init (else we see pink..)

    static public Color[] MapColor = ( System.Drawing.Color[] )TabColor.Clone( );
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

    static public void Reset( )
    {
      MapColor = ( System.Drawing.Color[] )TabColor.Clone( );
      GamepadColor = GamepadColorDefault;
    }

  }
}
