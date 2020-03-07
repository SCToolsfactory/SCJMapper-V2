using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SCJMapper_V2.Layout.ActionGroups;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// Maintains the colormap for the Layout
  /// </summary>
  class MapProps
  {
    /// <summary>
    /// Save Color Settings for the Layout
    /// </summary>
    public struct ColorPair
    {
      public Color ForeColor;
      public Color BackColor;

      public ColorPair( Color fColor, Color bColor )
      {
        ForeColor = fColor;
        BackColor = bColor;
      }
    }

    private static int m_fontSize = 16; // maintained as int to support the TrackBar Value property)
    private static Font m_font = new Font( "Tahoma", m_fontSize ); // real fontsize will be scaled to float but not used outside

    // all known actionmaps with it's classification
    private static Dictionary<EGroup, ColorPair> m_amColors;

    /// <summary>
    /// cTor: Load Colors from AppSettings
    /// </summary>
    static MapProps()
    {
      AppSettings.Instance.Reload( );

      FontSize = AppSettings.Instance.LayoutFontSize; // also creates the MapFont property

      m_amColors = new Dictionary<EGroup, ColorPair> {
        { EGroup.SpaceFlight,     ConvertFromString(AppSettings.Instance.GroupColor_00) },
        { EGroup.SpaceDefensive,  ConvertFromString(AppSettings.Instance.GroupColor_01) },
        { EGroup.SpaceTargeting,  ConvertFromString(AppSettings.Instance.GroupColor_02) },
        { EGroup.SpaceWeapons,    ConvertFromString(AppSettings.Instance.GroupColor_03) },
        { EGroup.SpaceMining,     ConvertFromString(AppSettings.Instance.GroupColor_04) },
        { EGroup.Player,          ConvertFromString(AppSettings.Instance.GroupColor_05) },
        { EGroup.EVA,             ConvertFromString(AppSettings.Instance.GroupColor_06) },
        { EGroup.Vehicle,         ConvertFromString(AppSettings.Instance.GroupColor_07) },
        { EGroup.VehicleWeapons,  ConvertFromString(AppSettings.Instance.GroupColor_08) },
        { EGroup.Lights,          ConvertFromString(AppSettings.Instance.GroupColor_09) },
        { EGroup.Interaction,     ConvertFromString(AppSettings.Instance.GroupColor_10) },
        { EGroup.Spectator,       ConvertFromString(AppSettings.Instance.GroupColor_11) },
        { EGroup.Others,          ConvertFromString(AppSettings.Instance.GroupColor_12) }
      };
    }

    /// <summary>
    /// Save color map settings for the Layout
    /// </summary>
    public static void SaveToSettings()
    {
      AppSettings.Instance.LayoutFontSize = m_fontSize;

      AppSettings.Instance.GroupColor_00 = ConvertToString( m_amColors[EGroup.SpaceFlight] );
      AppSettings.Instance.GroupColor_01 = ConvertToString( m_amColors[EGroup.SpaceDefensive] );
      AppSettings.Instance.GroupColor_02 = ConvertToString( m_amColors[EGroup.SpaceTargeting] );
      AppSettings.Instance.GroupColor_03 = ConvertToString( m_amColors[EGroup.SpaceWeapons] );
      AppSettings.Instance.GroupColor_04 = ConvertToString( m_amColors[EGroup.SpaceMining] );
      AppSettings.Instance.GroupColor_05 = ConvertToString( m_amColors[EGroup.Player] );
      AppSettings.Instance.GroupColor_06 = ConvertToString( m_amColors[EGroup.EVA] );
      AppSettings.Instance.GroupColor_07 = ConvertToString( m_amColors[EGroup.Vehicle] );
      AppSettings.Instance.GroupColor_08 = ConvertToString( m_amColors[EGroup.VehicleWeapons] );
      AppSettings.Instance.GroupColor_09 = ConvertToString( m_amColors[EGroup.Lights] );
      AppSettings.Instance.GroupColor_10 = ConvertToString( m_amColors[EGroup.Interaction] );
      AppSettings.Instance.GroupColor_11 = ConvertToString( m_amColors[EGroup.Spectator] );
      AppSettings.Instance.GroupColor_12 = ConvertToString( m_amColors[EGroup.Others] );

      AppSettings.Instance.Save( );
    }

    /// <summary>
    /// Cheap serializing...
    /// Converts from a ColorPair to a string
    /// </summary>
    /// <param name="colPair">A ColorPair</param>
    /// <returns>A serialized string</returns>
    private static string ConvertToString( ColorPair colPair )
    {
      string f = TypeDescriptor.GetConverter( typeof( Color ) ).ConvertToInvariantString( colPair.ForeColor );
      string b = TypeDescriptor.GetConverter( typeof( Color ) ).ConvertToInvariantString( colPair.BackColor );
      return $"{f}|{b}";
    }
    /// <summary>
    /// Cheap deserializing...
    /// Converts from a string to a ColorPair
    /// </summary>
    /// <param name="colPairS">A serialized string</param>
    /// <returns>A ColorPair</returns>
    private static ColorPair ConvertFromString( string colPairS )
    {
      string[] e = colPairS.Split( new char[] { '|' } );
      if ( e.Length == 2 ) {
        var f = (Color)TypeDescriptor.GetConverter( typeof( Color ) ).ConvertFromInvariantString( e[0] );
        var b = (Color)TypeDescriptor.GetConverter( typeof( Color ) ).ConvertFromInvariantString( e[1] );
        return new ColorPair( f, b );
      }
      else {
        return new ColorPair( Color.Pink, Color.Transparent );
      }
    }

    // Handle Layout Font
    /// <summary>
    /// Returns the Display Font for the layout
    /// </summary>
    public static Font MapFont { get => m_font; }

    /// <summary>
    /// FontSize property
    /// creates a new MapFont property to use
    /// </summary>
    public static int FontSize
    {
      get => m_fontSize;
      set {
        m_fontSize = value;
        m_font = new Font( m_font.FontFamily, m_fontSize );
      }
    }

    // Handle Layout Colors

    public static void SetMapColor( EGroup eGroup, ColorPair colorPair )
    {
      m_amColors[eGroup] = colorPair;
    }

    public static void SetMapColor( EGroup eGroup, Color fcolor, Color bcolor )
    {
      m_amColors[eGroup] = new ColorPair( fcolor, bcolor );
    }

    public static void SetMapForeColor( EGroup eGroup, Color color )
    {
      var copy = m_amColors[eGroup];
      copy.ForeColor = color;
      m_amColors[eGroup] = copy;
    }

    public static void SetMapBackColor( EGroup eGroup, Color color )
    {
      var copy = m_amColors[eGroup];
      copy.BackColor = color;
      m_amColors[eGroup] = copy;
    }

    public static ColorPair MapColor( string actionmap )
    {
      var acls = ActionGroups.MapNameToGroup( actionmap );
      return m_amColors[acls];
    }


    /// <summary>
    /// Returns the text color for an actionmap
    /// </summary>
    /// <param name="actionmap"></param>
    public static Color MapForeColor( string actionmap )
    {
      var acls = ActionGroups.MapNameToGroup( actionmap );
      return m_amColors[acls].ForeColor;
    }

    public static Color MapBackColor( string actionmap )
    {
      var acls = ActionGroups.MapNameToGroup( actionmap );
      return m_amColors[acls].BackColor;
    }


    public static ColorPair GroupColor( EGroup eGroup )
    {
      return m_amColors[eGroup];
    }

    public static Color GroupForeColor( EGroup eGroup )
    {
      return m_amColors[eGroup].ForeColor;
    }

    public static Color GroupBackColor( EGroup eGroup )
    {
      return m_amColors[eGroup].BackColor;
    }

  }
}
