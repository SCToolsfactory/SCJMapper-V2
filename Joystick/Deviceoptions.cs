using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains an Deviceoptions - something like:
  ///
  ///	<deviceoptions name="Joystick - HOTAS Warthog">
  ///		<!-- Reduce the deadzone -->
  ///		<option input="x" deadzone="0.015" />
  ///		<option input="y" deadzone="0.015" />	
  ///	</deviceoptions>	
  ///	
  /// [device] : set to device name (name shown in Windows Game Controllers control panel), currently known names follow
  ///	Joystick - HOTAS Warthog
  ///	Saitek X52 Pro Flight Controller
  ///	
  ///	</summary>
  public class Deviceoptions
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    List<String> m_stringOptions = new List<String>( );
    Options m_options = null;
    DeviceTuningParameter m_tuningX = null;
    DeviceTuningParameter m_tuningY = null;
    DeviceTuningParameter m_tuningZ = null;

    // ctor
    public Deviceoptions( Options options )
    {
      m_options = options;
      m_tuningX = m_options.TuneX;
      m_tuningY = m_options.TuneY;
      m_tuningZ = m_options.TuneZ;
    }


    public int Count
    {
      get { return ( m_stringOptions.Count + ( ( m_tuningX != null ) ? 1 : 0 ) + ( ( m_tuningY != null ) ? 1 : 0 ) + ( ( m_tuningZ != null ) ? 1 : 0 ) ); }
    }


    private String[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new String[] { String.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      }
      catch ( Exception ) {
        return new String[] { xml };
      }
    }

    /// <summary>
    /// Dump the Deviceoptions as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = "";

      // and dump the contents of plain string options
      foreach ( String x in m_stringOptions ) {

        if ( !String.IsNullOrWhiteSpace( x ) ) {
          foreach ( String line in FormatXml( x ) ) {
            r += String.Format( "\t{0}", line );
          }
        }

        r += String.Format( "\n" );
      }

      // dump Tuning 
      r += m_tuningX.Deviceoptions_toXML( );
      r += m_tuningY.Deviceoptions_toXML( );
      r += m_tuningZ.Deviceoptions_toXML( );

      return r;
    }



    /// <summary>
    /// Read an Deviceoptions from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
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

      String name = "";

      if ( reader.HasAttributes ) {
        name = reader["name"];

        reader.Read( );
        // try to disassemble the items
        while ( !reader.EOF ) {
          if ( reader.Name == "option" ) {
            if ( reader.HasAttributes ) {
              String input = reader["input"];
              String deadzone = reader["deadzone"];
              if ( ! (String.IsNullOrWhiteSpace( input ) || String.IsNullOrWhiteSpace( deadzone )) ) {
                if ( input.ToLowerInvariant( ).EndsWith("x") ) {
                  if ( String.IsNullOrWhiteSpace( m_tuningX.CommandCtrl ) ) m_tuningX.CommandCtrl = input; // if no options have been given...
                  if ( string.IsNullOrWhiteSpace( m_tuningX.DeviceName ) ) m_tuningX.DeviceName = name; // if no devicename has been given...
                  m_tuningX.DeadzoneUsed = true; m_tuningX.Deadzone = deadzone;
                }
                else if ( input.ToLowerInvariant( ).EndsWith("y") ) {
                  if ( String.IsNullOrWhiteSpace( m_tuningY.CommandCtrl ) ) m_tuningY.CommandCtrl = input; // if no options have been given...
                  if ( string.IsNullOrWhiteSpace( m_tuningY.DeviceName ) ) m_tuningY.DeviceName = name; // if no devicename has been given...
                  m_tuningY.DeadzoneUsed = true; m_tuningY.Deadzone = deadzone;
                }
                else if ( input.ToLowerInvariant( ).EndsWith( "z" ) ) {
                  if ( String.IsNullOrWhiteSpace( m_tuningZ.CommandCtrl )) m_tuningZ.CommandCtrl=input; // if no options have been given...
                  if ( string.IsNullOrWhiteSpace( m_tuningZ.DeviceName ) ) m_tuningZ.DeviceName = name; // if no devicename has been given...
                  m_tuningZ.DeadzoneUsed = true; m_tuningZ.Deadzone = deadzone;
                }
                else {
                  //?? option node refers to unknown axis (not x,y,rotz)
                  log.ErrorFormat( "Deviceoptions.fromXML: option node refers to unknown axis {0}", input );
                }
              }
              else {
                //? option node has not the needed attributes
                log.ErrorFormat( "Deviceoptions.fromXML: option node has not the needed attributes" );
              }
            }
            else {
              //?? option node has NO attributes
              log.ErrorFormat( "Deviceoptions.fromXML: option node has NO attributes" );
            }
          }

          reader.Read( );
        }//while
      }
      else {
        //??
        if ( !m_stringOptions.Contains( xml ) ) m_stringOptions.Add( xml );
      }

      return true;
    }


  }
}
