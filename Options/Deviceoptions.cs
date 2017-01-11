using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using SCJMapper_V2.Joystick;

namespace SCJMapper_V2.Options
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
    /// <summary>
    /// Create a DeviceOption ID from dev Name and the command
    /// </summary>
    /// <param name="devName">The game device name as retrieved from XInput</param>
    /// <param name="cmdCtrl">A device control that supports devOptions (all ananlog controls)</param>
    /// <returns></returns>
    public static string DevOptionID( string devName, string cmdCtrl )
    {
      // cmdCtrl can be anything 
      //    v_flight_throttle_abs - js1_throttlez
      //    v_strafe_longitudinal - js1_y
      //    v_strafe_longitudinal - js1_roty
      //    v_strafe_longitudinal - xi1_shoulderl+thumbly
      //    v_strafe_longitudinal - xi1_thumbly
      string cmd = cmdCtrl.Trim();
      // messy...
      if ( cmd.Contains( "throttle" ) ) cmd = cmd.Replace( "throttle", "" ); // this is not suitable for the devOption
      if ( cmd.Contains( "throttle" ) ) cmd = cmd.Replace( "thumbl", "" ); // this is not suitable for the devOption
      if ( cmd.Contains( "throttle" ) ) cmd = cmd.Replace( "thumbr", "" ); // this is not suitable for the devOption
      if ( cmd.Contains( "_" ) ) {
        int l = cmd.LastIndexOf("_");
        cmd = cmd.Substring( l + 1 ); // assuming it is never the last one..
      }
      if ( cmd.Contains( "+" ) ) {
        int l = cmd.LastIndexOf("+");
        cmd = cmd.Substring( l + 1 ); // assuming it is never the last one..
      }
      return string.Format( "{0}{1}{2}", devName, ID_Delimiter, cmd );
    }


    private List<string> m_stringOptions = new List<string>( ); // collected options from XML that are not parsed


    // ctor
    public Deviceoptions( JoystickList jsList )
    {
      // create all devOptions for all devices found (they may or may no be used)
      foreach ( JoystickCls js in jsList ) {
        foreach ( string input in js.AnalogCommands ) {
          string doid = DevOptionID(js.DevName, input);
          if ( ! this.ContainsKey(doid)) {
            this.Add( doid, new DeviceOptionParameter( js.DevName, input, "", "" ) ); // init with disabled defaults
          } else {
            log.WarnFormat( "cTor - DO_ID {0} exists (likely a duplicate device name e,g, vJoy ??)", doid );
          }
        }
      }
    }


    new public int Count
    {
      get { return ( m_stringOptions.Count + base.Count ); }
    }

    /// <summary>
    /// Reset all Action strings in the dictionary
    /// </summary>
    public void ResetActions( )
    {
      foreach (KeyValuePair<string, DeviceOptionParameter> kv in this ) {
        kv.Value.Action = "";
      }
    }


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
                    deadzone = "0.000";
                  }
                  if ( !this.ContainsKey( doID ) ) {
                    this.Add( doID, new DeviceOptionParameter( name, input, deadzone, saturation ) );
                  } else {
                    // add deadzone value tp existing
                    this[doID].DeadzoneUsed = true;
                    this[doID].Deadzone = deadzone;
                  }
                }
                if ( !string.IsNullOrWhiteSpace( saturation ) ) {
                  float testF;
                  if ( !float.TryParse( saturation, out testF ) ) { // check for valid number in string
                    saturation = "1.000";
                  }
                  if ( !this.ContainsKey( doID ) ) {
                    this.Add( doID, new DeviceOptionParameter( name, input, deadzone, saturation ) );
                  } else {
                    // add saturation value tp existing
                    this[doID].SaturationUsed = true;
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
