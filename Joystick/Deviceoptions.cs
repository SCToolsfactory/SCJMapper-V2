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

    DeviceDeadzoneParameter m_deadzoneX = null;
    DeviceDeadzoneParameter m_deadzoneY = null;
    DeviceDeadzoneParameter m_deadzoneZ = null;

    // ctor
    public Deviceoptions( Options options )
    {
      m_deadzoneX = new DeviceDeadzoneParameter();
      m_deadzoneY = new DeviceDeadzoneParameter( );
      m_deadzoneZ = new DeviceDeadzoneParameter( );
    }


    public int Count
    {
      get { return ( m_stringOptions.Count + ( ( m_deadzoneX != null ) ? 1 : 0 ) + ( ( m_deadzoneY != null ) ? 1 : 0 ) + ( ( m_deadzoneZ != null ) ? 1 : 0 ) ); }
    }

    // provide access to Sense items

    /// <summary>
    /// Returns the Z-sensitivity item
    /// </summary>
    public DeviceDeadzoneParameter DeadzoneX
    {
      get { return m_deadzoneX; }
    }
    /// <summary>
    /// Returns the Z-sensitivity item
    /// </summary>
    public DeviceDeadzoneParameter DeadzoneY
    {
      get { return m_deadzoneY; }
    }
    /// <summary>
    /// Returns the Z-sensitivity item
    /// </summary>
    public DeviceDeadzoneParameter DeadzoneZ
    {
      get { return m_deadzoneZ; }
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
      r += m_deadzoneX.Deviceoptions_toXML( );
      r += m_deadzoneY.Deviceoptions_toXML( );
      r += m_deadzoneZ.Deviceoptions_toXML( );

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
                  if ( String.IsNullOrWhiteSpace( m_deadzoneX.CommandCtrl ) ) m_deadzoneX.CommandCtrl = input; // if no options have been given...
                  if ( String.IsNullOrWhiteSpace( m_deadzoneX.DeviceName ) ) m_deadzoneX.DeviceName = name; // if no devicename has been given...
                  float testF;
                  if ( float.TryParse( deadzone, out testF ) ) { // check for valid number in string
                    m_deadzoneX.DeadzoneUsed = true; m_deadzoneX.Deadzone = deadzone;
                  }
                  else {
                    m_deadzoneX.DeadzoneUsed = false; m_deadzoneX.Deadzone = "0.00";
                  }
                }
                else if ( input.ToLowerInvariant( ).EndsWith("y") ) {
                  if ( String.IsNullOrWhiteSpace( m_deadzoneY.CommandCtrl ) ) m_deadzoneY.CommandCtrl = input; // if no options have been given...
                  if ( String.IsNullOrWhiteSpace( m_deadzoneY.DeviceName ) ) m_deadzoneY.DeviceName = name; // if no devicename has been given...
                  float testF;
                  if ( float.TryParse( deadzone, out testF ) ) { // check for valid number in string
                  m_deadzoneY.DeadzoneUsed = true; m_deadzoneY.Deadzone = deadzone;
                  }
                  else {
                    m_deadzoneY.DeadzoneUsed = false; m_deadzoneY.Deadzone = "0.00";
                  }
                }
                else if ( input.ToLowerInvariant( ).EndsWith( "z" ) ) {
                  if ( String.IsNullOrWhiteSpace( m_deadzoneZ.CommandCtrl )) m_deadzoneZ.CommandCtrl=input; // if no options have been given...
                  if ( String.IsNullOrWhiteSpace( m_deadzoneZ.DeviceName ) ) m_deadzoneZ.DeviceName = name; // if no devicename has been given...
                  float testF;
                  if ( float.TryParse( deadzone, out testF ) ) { // check for valid number in string
                  m_deadzoneZ.DeadzoneUsed = true; m_deadzoneZ.Deadzone = deadzone;
                  }
                  else {
                    m_deadzoneZ.DeadzoneUsed = false; m_deadzoneZ.Deadzone = "0.00";
                  }
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
