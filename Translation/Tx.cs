using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCJMapper_V2.Properties;

namespace SCJMapper_V2.Translation
{
  internal static class Tx
  {
    /// <summary>
    /// used Culture for translations
    /// </summary>
    public static CultureInfo UsedCulture = CultureInfo.CurrentCulture;

    // extension method
    public static string Translate( this Control ctl )
    {
      return Strings.ResourceManager.GetString( ctl.Name, UsedCulture ) ?? String.Format( "**UNDEF**" );
    }
    public static string Translate( this ToolStripItem ctl )
    {
      return Strings.ResourceManager.GetString( ctl.Name, UsedCulture ) ?? String.Format( "**UNDEF**" );
    }

    /// <summary>
    /// Returns the localce string for the ressource ID
    /// </summary>
    /// <param name="ressourceID">A ressource ID</param>
    /// <returns>The localized text</returns>
    public static string Translate ( string ressourceID )
    {
      return Strings.ResourceManager.GetString( ressourceID, UsedCulture ) ?? String.Format( "**UNDEF**" );
    }


    /// <summary>
    /// Localizes a tree of controls - localize the one with Tag=§
    ///   using the extension method
    /// </summary>
    /// <param name="ctrl">The control to start with</param>
    /// <param name="cultureInfo"></param>
    public static void LocalizeControlTree( Object ctrl)
    {
      // children first..
      if ( ctrl is ToolStrip ) {
        foreach ( ToolStripItem ctl in ( ctrl as ToolStrip ).Items ) {
          LocalizeControlTree( ctl );
        }
      }
      else if ( ctrl is StatusStrip ) {
        foreach ( ToolStripItem ctl in ( ctrl as StatusStrip ).Items ) {
          LocalizeControlTree( ctl );
        }
      }
      else if ( ctrl is ToolStripDropDownButton ) {
        foreach ( ToolStripItem ctl in ( ctrl as ToolStripDropDownButton ).DropDownItems ) {
          LocalizeControlTree( ctl );
        }
      }
      if ( ctrl is ToolStripItem ) {
        // tx this 
        try {
          if ( ( ctrl as ToolStripItem ).Tag?.ToString( ) == "§" ) { // the translation Tag set in GUI designer
            ( ctrl as ToolStripItem ).Text = ( ctrl as ToolStripItem ).Translate( );
          }
        }
        catch { }
        return;
      }
      else {
        foreach ( Control ctl in (ctrl as Control).Controls ) {
          LocalizeControlTree( ctl );
        }
      }
      // tx this 
      try {
        if ( ( ctrl as Control ).Tag?.ToString( ) == "§" ) { // the translation Tag set in GUI designer
          ( ctrl as Control ).Text = ( ctrl as Control ).Translate( );
        }
      }
      catch { }
    }


  }
}
