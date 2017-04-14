using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace SCJMapper_V2.Joystick
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
  public class UICustHeader : ICloneable
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    List<string> m_stringOptions = new List<string>( );

    public struct DevRec : ICloneable
    {
      public string devType;
      public int    instNo;

      public object Clone()
      {
        var dr = (DevRec)this.MemberwiseClone( );

        return dr;
      }

      /// <summary>
      /// Check clone against This
      /// </summary>
      /// <param name="clone"></param>
      /// <returns>True if the clone is identical but not a shallow copy</returns>
      public bool CheckClone(DevRec clone)
      {
        bool ret = true;
        ret &= ( this.devType == clone.devType ); // immutable string - shallow copy is OK
        ret &= ( this.instNo == clone.instNo ); // value type
        return ret;
      }
    }
    List<DevRec> m_devInstances = new List<DevRec>( );

    private string m_label = "";
    private string m_description = "";
    private string m_image = "";


    /// <summary>
    /// Clone this object
    /// </summary>
    /// <returns>A deep Clone of this object</returns>
    public object Clone( )
    {
      var uic = (UICustHeader)this.MemberwiseClone();
      // more objects to deep copy
      uic.m_devInstances = m_devInstances.Select( x => ( DevRec )x.Clone( ) ).ToList( );

#if DEBUG
      // check cloned item
      System.Diagnostics.Debug.Assert( CheckClone( uic ) );
#endif
      return uic;
    }

    /// <summary>
    /// Check clone against This
    /// </summary>
    /// <param name="clone"></param>
    /// <returns>True if the clone is identical but not a shallow copy</returns>
    private bool CheckClone (UICustHeader clone )
    {
      bool ret = true;
      ret &= ( this.m_stringOptions == clone.m_stringOptions ); // immutable string list - shallow copy is OK
      ret &= ( this.m_label == clone.m_label ); // immutable string - shallow copy is OK
      ret &= ( this.m_description == clone.m_description ); // immutable string - shallow copy is OK
      ret &= ( this.m_image == clone.m_image ); // immutable string - shallow copy is OK

      ret &= ( this.m_devInstances.Count == clone.m_devInstances.Count );
      if (ret) {
        for ( int i = 0; i < this.m_devInstances.Count; i++ ) {
          ret &= ( this.m_devInstances[i].CheckClone( clone.m_devInstances[i] ) );
        }
      }
      return ret;
    }


    public int Count
    {
      get { return ( m_stringOptions.Count + m_devInstances.Count ); }
    }

    public string Label
    {
      get { return m_label; }
      set { m_label = value; }
    }

    public void ClearInstances( )
    {
      m_devInstances.Clear( );
    }
    public void AddInstances( DevRec dr )
    {
      m_devInstances.Add( dr );
    }


    private string[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new string[] { string.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      }
      catch ( Exception ) {
        return new string[] { xml };
      }
    }

    /// <summary>
    /// Dump the CustomisationUIHeader as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public string toXML( )
    {
      /*
       <CustomisationUIHeader label="sdsd" description="" image="">
        <devices>
         <keyboard instance="1"/>
         <mouse instance="1"/>
         <joystick instance="1"/>
         <joystick instance="2"/>
        </devices>
        <categories>
         <category label="@ui_CCSpaceFlight"/>
        </categories>
       </CustomisationUIHeader>
      */


      string r = "";

      r += string.Format( "\t<CustomisationUIHeader label=\"{0}\" description=\"{1}\" image=\"{2}\">\n", m_label, m_description, m_image );
      if ( m_devInstances.Count > 0 ) {
        r += string.Format( "\t\t<devices>\n" );

        foreach ( DevRec dr in m_devInstances ) {
          r += string.Format( "\t\t\t<{0} instance=\"{1}\"/>\n", dr.devType, dr.instNo.ToString( ) );
        }

        r += string.Format( "\t\t</devices>\n" );
      }
      // CIG adds them to export - so can we ...
      r += string.Format( "\t\t<categories>\n" );
      r += string.Format( "\t\t<category label=\"@ui_CCSpaceFlight\"/>\n" );
      r += string.Format( "\t\t</categories>\n" );

      r += string.Format( "\t</CustomisationUIHeader>\n" );

      // and dump the plain contents if needed
      foreach ( string x in m_stringOptions ) {
        if ( !string.IsNullOrWhiteSpace( x ) ) {
          foreach ( string line in FormatXml( x ) ) {
            r += string.Format( "\t{0}", line );
          }
        }
      }
      r += string.Format( "\n" );
      return r;
    }



    /// <summary>
    /// Read an CustomisationUIHeader from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    private Boolean Instance_fromXML( XmlReader reader )
    {
      reader.Read( );

      while ( !reader.EOF ) {
        string devType = reader.Name;
        string instance = reader["instance"];
        int instNo = 0;
        if ( !string.IsNullOrWhiteSpace( instance ) ) {
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
    public Boolean fromXML( string xml )
    {
      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      if ( reader.HasAttributes ) {
        m_label = reader["label"];
        m_description = reader["description"];
        if ( string.IsNullOrEmpty( m_description ) ) m_description = "@ui_JoystickDefaultDesc";
        m_image = reader["image"];
        if ( string.IsNullOrEmpty( m_image ) ) m_image = "JoystickDefault";

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
          else if ( reader.Name.ToLowerInvariant( ) == "categories" ) {
            reader.ReadInnerXml( );
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
