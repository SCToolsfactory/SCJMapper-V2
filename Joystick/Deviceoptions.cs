using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace SCJMapper_V2.Joystick
{
  /// <summary>
  ///   Maintains an Deviceoptions - something like:
  ///
  ///	<deviceoptions name="Joystick - HOTAS Warthog">
  ///		<!-- Reduce the deadzone -->
  ///		<option input="x" deadzone="0.015" />
  ///		<option input="y" deadzone="0.015" />	
  ///		<option input="y" saturation="0.85" />	
  ///	</deviceoptions>	
  ///	
  /// [device] : set to device name (name shown in Windows Game Controllers control panel), currently known names follow
  ///	Joystick - HOTAS Warthog
  ///	Saitek X52 Pro Flight Controller
  ///	
  ///	</summary>
  public class Deviceoptions : Dictionary<string, DeviceOptionParameter>
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private static char ID_Delimiter = '⁞';
    public static string DevOptionID( string devName, string cmdCtrl )
    {
      string cmd = cmdCtrl.Trim();
      if ( cmd.Contains( "_" ) ) {
        int l = cmd.LastIndexOf("_");
        cmd = cmd.Substring( l + 1 ); // assuming it is never the last one..
      }
      return string.Format( "{0}{1}{2}", devName, ID_Delimiter, cmd );
    }

    List<string> m_stringOptions = new List<string>( );


    // ctor
    public Deviceoptions( Options options )
    {
    }


    new public int Count
    {
      get { return ( m_stringOptions.Count + base.Count ); }
    }

    // provide access to Sense items

    private string[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new string[] { string.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      } catch ( Exception ) {
        return new string[] { xml };
      }
    }

    /// <summary>
    /// Dump the Deviceoptions as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public string toXML( )
    {
      string r = "";

      // and dump the contents of plain string options
      foreach ( string x in m_stringOptions ) {

        if ( !string.IsNullOrWhiteSpace( x ) ) {
          foreach ( string line in FormatXml( x ) ) {
            r += string.Format( "\t{0}", line );
          }
        }

        r += string.Format( "\n" );
      }

      // dump Tuning 
      foreach ( KeyValuePair<string, DeviceOptionParameter> kv in this ) {
        r += kv.Value.Deviceoptions_toXML( );
      }

      return r;
    }



    /// <summary>
    /// Read an Deviceoptions from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( string xml )
    {
      /* 
       * This can be a lot of the following options
       * try to do our best....
       * 
       * 	<deviceoptions name="Joystick - HOTAS Warthog">
            <!-- Reduce the deadzone -->
            <option input="x" deadzone="0.015" />
            <option input="y" deadzone="0.015" />	
          </deviceoptions>
       * 
      */

      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      string name = "";

      if ( reader.HasAttributes ) {
        name = reader["name"];

        reader.Read( );
        // try to disassemble the items
        while ( !reader.EOF ) {
          if ( reader.Name.ToLowerInvariant( ) == "option" ) {
            if ( reader.HasAttributes ) {
              string input = reader["input"];
              string deadzone = reader["deadzone"];
              string saturation = reader["saturation"];
              if ( !string.IsNullOrWhiteSpace( input ) ) {
                string doID = DevOptionID(name, input);
                if ( !string.IsNullOrWhiteSpace( deadzone ) ) {
                  float testF;
                  if ( !float.TryParse( deadzone, out testF ) ) { // check for valid number in string
                    deadzone = "0.00";
                  }
                  if ( !this.ContainsKey( doID ) ) {
                    this.Add( doID, new DeviceOptionParameter( name, input, deadzone, saturation ) );
                  }else {
                    // add deadzone value tp existing
                    this[doID].Deadzone = deadzone;
                  }
                }
                if ( !string.IsNullOrWhiteSpace( saturation ) ) {
                  float testF;
                  if ( !float.TryParse( saturation, out testF ) ) { // check for valid number in string
                    saturation = "1.00";
                  }
                  if ( !this.ContainsKey( doID ) ) {
                    this.Add( doID, new DeviceOptionParameter( name, input, deadzone, saturation ) );
                  } else {
                    // add saturation value tp existing
                    this[doID].Saturation = saturation;
                  }
                }
              } else {
                //? option node has not the needed attributes
                log.ErrorFormat( "Deviceoptions.fromXML: option node has not the needed attributes" );
              }
            } else {
              //?? option node has NO attributes
              log.ErrorFormat( "Deviceoptions.fromXML: option node has NO attributes" );
            }
          }

          reader.Read( );
        }//while
      } else {
        //??
        if ( !m_stringOptions.Contains( xml ) ) m_stringOptions.Add( xml );
      }

      return true;
    }


  }
}
