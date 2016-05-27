using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SCJMapper_V2.Joystick
{
  /// <summary>
  /// Maintain actionmap Modifiers
  /// </summary>
  class Modifiers : List<string>
  {
   
    /// <summary>
    /// Returns a properly formatted Modifier entry
    /// </summary>
    /// <returns></returns>
    public String toXML()
    {
      string r = "";
      r += string.Format( "\t<modifiers>\n" );
      foreach ( string s in this ) {
        r += string.Format( "\t\t<mod input=\"{0}\" />\n", s );
      }
      r += string.Format( "\t</modifiers>\n" );

      return r;
    }


    /// <summary>
    /// Read an Modifier entry from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    private Boolean Instance_fromXML( XmlReader reader )
    {
      reader.Read( );

      while ( !reader.EOF ) {
        if ( reader.Name.ToLowerInvariant() == "mod" ) {
          String input = reader["input"];
          if ( !String.IsNullOrWhiteSpace( input ) ) {
            this.Add( input );
          }
        }
        reader.Read( );
        if ( reader.NodeType == XmlNodeType.EndElement ) break; // expect end of <modifiers> here
      }//while

      return true;
    }


    /// <summary>
    /// Reads the Modifier entry from an action map file
    /// </summary>
    public Boolean fromXML( String xml )
    {
      /*
        <!-- Key modifiers -->
        <modifiers>
          <mod input="js1_button3" />
        </modifiers>

      */
      this.Clear( );

      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      // try to disassemble the items
      while ( !reader.EOF ) {

        if ( reader.Name.ToLowerInvariant( ) == "modifiers" ) {
          Instance_fromXML( reader );
        }

        reader.Read( );
        if ( reader.NodeType == XmlNodeType.EndElement ) break; // expect end of <modifiers> here
      }

      return true;

    }

  }
}
