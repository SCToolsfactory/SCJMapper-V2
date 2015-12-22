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
  ///   Maintains an CustomisationUIHeader - something like:
  ///
  /// 	<CustomisationUIHeader device="joystick" label="JoystickTMWarthog" description="@ui_JoystickTMWarthogDesc" image="JoystickTMWarthog" />
  /// 	
  /// or 
  /// 
  ///	<CustomisationUIHeader label="JoystickSaitekX55" description="@ui_JoystickSaitekX55Desc" image="JoystickSaitekX55">
  ///		<Devices>
  ///			<joystick instance="1" />
  ///			<joystick instance="2" />
  ///		</Devices>
  ///	</CustomisationUIHeader>
  /// 	</summary>
  public class UICustHeader
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    List<String> m_stringOptions = new List<String>( );

    public struct DevRec
    {
      public String devType;
      public int    instNo;
    }
    List<DevRec> m_devInstances = new List<DevRec>( );

    private String m_label = "";
    private String m_description = "";
    private String m_image = "";


    public int Count
    {
      get { return ( m_stringOptions.Count + m_devInstances.Count ); }
    }

    public String Label
    {
      get { return m_label; }
      set { m_label = value; }
    }

    public void ClearInstances( )
    {
      m_devInstances.Clear();
    }
    public void AddInstances( DevRec dr )
    {
      m_devInstances.Add( dr );
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
    /// Dump the CustomisationUIHeader as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      /*
       <CustomisationUIHeader label="sdsd" description="" image="">
        <devices>
         <keyboard instance="1"/>
         <mouse instance="1"/>
         <joystick instance="1"/>
         <joystick instance="2"/>
        </devices>
        <categories />
       </CustomisationUIHeader>
      */


      String r = "";

      r += String.Format( "\t<CustomisationUIHeader label=\"{0}\" description=\"{1}\" image=\"{2}\">\n", m_label, m_description, m_image );
      if ( m_devInstances.Count > 0 ) {
        r += String.Format( "\t\t<devices>\n" );

        foreach ( DevRec dr in m_devInstances ) {
          r += String.Format( "\t\t\t<{0} instance=\"{1}\"/>\n", dr.devType, dr.instNo.ToString( ) );
        }

        r += String.Format( "\t\t</devices>\n" );
      }
      r += String.Format( "\t</CustomisationUIHeader>\n" );

      // and dump the plain contents if needed
      foreach ( String x in m_stringOptions ) {
        if ( !String.IsNullOrWhiteSpace( x ) ) {
          foreach ( String line in FormatXml( x ) ) {
            r += String.Format( "\t{0}", line );
          }
        }
      }
      r += String.Format( "\n" );
      return r;
    }



    /// <summary>
    /// Read an CustomisationUIHeader from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean Instance_fromXML( XmlReader reader )
    {
      reader.Read( );

      while ( !reader.EOF ) {
        String devType = reader.Name;
        String instance = reader["instance"];
        int instNo = 0;
        if ( !String.IsNullOrWhiteSpace( instance ) ) {
          if ( !int.TryParse( instance, out instNo ) ) {
            instNo = 0;
          }
          else {
            DevRec dr = new DevRec( );
            dr.devType = devType;
            dr.instNo = instNo;
            m_devInstances.Add( dr );
          }
        }
        reader.Read( );
        if ( reader.NodeType == XmlNodeType.EndElement ) break; // expect end of <Devices> here
      }//while

      return true;
    }


    /// <summary>
    /// Read an CustomisationUIHeader from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
    {
      // if ( !this.Contains( xml ) ) this.Add( xml );


      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      if ( reader.HasAttributes ) {
        m_label = reader["label"];
        m_description = reader["description"];
        if ( String.IsNullOrEmpty( m_description ) ) m_description = "@ui_JoystickDefaultDesc";
        m_image = reader["image"];
        if ( String.IsNullOrEmpty( m_image ) ) m_image = "JoystickDefault";

        reader.Read( );
        // try to disassemble the items
        /*
         *  <Devices>
         *  	<joystick instance="1" />
         *		<joystick instance="2" />
         *  </Devices>
         */
        while ( !reader.EOF ) {

          if ( reader.Name.ToLowerInvariant( ) == "devices" ) {
            Instance_fromXML( reader );
          }
          else {
            //??
            log.InfoFormat( "UICustHeader.fromXML: unknown node - {0} - stored as is", reader.Name );
            if ( !m_stringOptions.Contains( xml ) ) m_stringOptions.Add( xml );
          }

          reader.Read( );
          if ( reader.NodeType == XmlNodeType.EndElement ) break; // expect end of <CustomisationUIHeader> here
        }

      }

      return true;
    }




  }
}
