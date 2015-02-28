using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains the complete ActionMaps - something like:
  ///   
  /// <ActionMaps version="0" >
  /// 	<actionmap name="spaceship_view">
  ///		<action name="v_view_cycle_fwd">
  ///			<rebind device="joystick" input="js2_button2" />
  ///		</action>
  ///		...
  ///			</actionmap>	
  /// </ActionMaps>
  /// </summary>
  class ActionMapsCls : List<ActionMapCls>
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    #region Static Part of ActionMaps

    // actionmap names to gather (do we need them to be cofigurable ??)
    public static String[] ActionMaps = { };
    public static void LoadSupportedActionMaps( )
    {
      // load actionmaps
      String acm = AppConfiguration.AppConfig.scActionmaps;
      ActionMaps = acm.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
    }

    #endregion Static Part of ActionMaps

    private const String ACM_VERSION = "1"; // the default version 

    private String version { get; set; }
    private String ignoreversion { get; set; }

    private JoystickList m_joystickList = null;
    private UICustHeader m_uiCustHeader = null;
    private Options m_options = null;
    private Deviceoptions m_deviceOptions = null;

    private List<CheckBox> m_invertCB = null; // Options owns and handles all Inversions

    // own additions for JS mapping - should not harm..
    private String[] m_js;
    private String[] m_GUIDs;

    /// <summary>
    /// get/set jsN assignment (use 0-based index i.e. js1 -> [0])
    /// </summary>
    public String[] jsN
    {
      get { return m_js; }
    }

    /// <summary>
    /// get/set jsN GUID assignment (use 0-based index i.e. js1GUID -> [0])
    /// </summary>
    public String[] jsNGUID
    {
      get { return m_GUIDs; }
    }

    /// <summary>
    /// Clears a read but not longer known entry
    /// </summary>
    public void Clear_jsEntry(int index)
    {
      m_js[index] = "";
      m_GUIDs[index] = "";
    }

    // provide access to Tuning items of the Options obj to the owner

    /// <summary>
    /// Returns the X-Tuning item
    /// </summary>
    public DeviceTuningParameter TuningP
    {
      get { return m_options.TuneP; }
    }
    /// <summary>
    /// Returns the Y-Tuning item
    /// </summary>
    public DeviceTuningParameter TuningY
    {
      get { return m_options.TuneY; }
    }
    /// <summary>
    /// Returns the Z-Tuning item
    /// </summary>
    public DeviceTuningParameter TuningR
    {
      get { return m_options.TuneR; }
    }

    /// <summary>
    /// Returns the X-Sensitivity item
    /// </summary>
    public DeviceDeadzoneParameter DeadzoneX
    {
      get { return m_deviceOptions.DeadzoneX; }
    }
    /// <summary>
    /// Returns the Y-Sensitivity item
    /// </summary>
    public DeviceDeadzoneParameter DeadzoneY
    {
      get { return m_deviceOptions.DeadzoneY; }
    }
    /// <summary>
    /// Returns the Z-Sensitivity item
    /// </summary>
    public DeviceDeadzoneParameter DeadzoneZ
    {
      get { return m_deviceOptions.DeadzoneZ; }
    }

    /// <summary>
    /// Returns the Options item
    /// </summary>
    public Options Options
    {
      get { return m_options; }
    }

    /// <summary>
    /// Assign the GUI Invert Checkboxes for further handling
    /// </summary>
    public List<CheckBox> InvertCheckList
    {
      set { 
        m_invertCB = value;
        m_options.InvertCheckList = m_invertCB;
      }
    }



    /// <summary>
    /// ctor
    /// </summary>
    public ActionMapsCls( JoystickList jsList )
    {
      version = ACM_VERSION;
      m_joystickList = jsList; // have to save this for Reassign

      // create the Joystick assignments
      Array.Resize( ref m_js, JoystickCls.JSnum_MAX + 1 );
      Array.Resize( ref m_GUIDs, JoystickCls.JSnum_MAX + 1 );
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        m_js[i] = ""; m_GUIDs[i] = "";
      }

      CreateNewOptions( );

      LoadSupportedActionMaps( ); // get them from config
    }


    private void CreateNewOptions( )
    {
      // create options objs
      m_uiCustHeader = new UICustHeader( );
      m_options = new Options( m_joystickList );
      m_deviceOptions = new Deviceoptions( m_options );
    }

    /// <summary>
    /// Copy return all ActionMaps while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionMaps copy with reassigned input</returns>
    public ActionMapsCls ReassignJsN( JsReassingList newJsList )
    {
      ActionMapsCls newMaps = new ActionMapsCls( m_joystickList );
      // full copy from 'this' 

      newMaps.m_uiCustHeader = this.m_uiCustHeader;
      newMaps.m_deviceOptions = this.m_deviceOptions;
      newMaps.m_options = this.m_options;

      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        newMaps.jsN[i] = this.jsN[i]; newMaps.jsNGUID[i] = this.jsNGUID[i];
      }

      foreach ( ActionMapCls am in this ) {
        newMaps.Add( am.ReassignJsN( newJsList ) );
      }

      //m_options.ReassignJsN( newJsList );

      return newMaps;
    }

    /// <summary>
    /// Merge the given Map with this Map
    /// new ones are ignored - we don't learn from XML input for the time beeing
    /// </summary>
    /// <param name="newAcm"></param>
    private void Merge( ActionMapCls newAcm )
    {
      log.Debug( "Merge - Entry" );

      // do we find an actionmap like the new one in our list ?
      ActionMapCls ACM = this.Find( delegate( ActionMapCls acm ) {
        return acm.name == newAcm.name;
      } );
      if ( ACM == null ) {
        ; // this.Add( newAcm ); // no, add new
      }
      else {
        ACM.Merge( newAcm ); // yes, merge it
      }
    }


    /// <summary>
    /// Dump the ActionMaps as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( String fileName )
    {
      log.Debug( "toXML - Entry" );

      AppSettings  appSettings = new AppSettings( );

      // handle the versioning of the actionmaps
      String r = "<ActionMaps ";
      if ( !String.IsNullOrEmpty( ignoreversion ) ) {
        r += String.Format( "ignoreVersion=\"{0}\" \n", ignoreversion );
      }
      else if ( appSettings.ForceIgnoreversion ) {
        ignoreversion = "1"; // preset if missing
        r += String.Format( "ignoreVersion=\"{0}\" \n", ignoreversion );
      }
      else if ( !String.IsNullOrEmpty( version ) ) {
        r += String.Format( "version=\"{0}\" \n", version );
      }
      else {
        version = ACM_VERSION; // preset if missing
        r += String.Format( "version=\"{0}\" \n", version );
      }

      // now the devices (our addition)
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( jsN[i] ) ) r += String.Format( "\tjs{0}=\"{1}\" ", i + 1, jsN[i] );
        if ( !String.IsNullOrEmpty( jsNGUID[i] ) ) r += String.Format( "js{0}G=\"{1}\" \n", i + 1, jsNGUID[i] );
      }

      // close the tag
      r += String.Format( ">\n" );

      // and dump the option contents
      if ( m_uiCustHeader.Count > 0 ) {
        // prepare with new data
        m_uiCustHeader.ClearInstances( );
        UICustHeader.DevRec dr = new UICustHeader.DevRec( );
        for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
          if ( !String.IsNullOrEmpty( jsN[i] ) ) {
            dr.devType = "joystick"; dr.instNo = i + 1;
            m_uiCustHeader.AddInstances( dr );
          }
        }
        m_uiCustHeader.Label = fileName.Replace( SCMappings.c_MapStartsWith, "" ); // remove redundant part
        r += m_uiCustHeader.toXML( ) + String.Format( "\n" );
      }
      if ( m_options.Count > 0 ) r += m_options.toXML( ) + String.Format( "\n" );
      if ( m_deviceOptions.Count > 0 ) r += m_deviceOptions.toXML( ) + String.Format( "\n" );

      // finally the action maps
      foreach ( ActionMapCls amc in this ) {
        r += String.Format( "{0}\n", amc.toXML( ) );
      }
      r += String.Format( "</ActionMaps>\n" );
      return r;
    }


    /// <summary>
    /// Read an ActionMaps from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
    {
      log.Debug( "fromXML - Entry" );

      CreateNewOptions( ); // Reset those options...
      m_options.InvertCheckList = m_invertCB;
      m_options.ResetInverter( ); // have to reset when reading a new mapping

      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );


      reader.Read( );

      if ( reader.Name == "ActionMaps" ) {
        if ( reader.HasAttributes ) {
          version = reader["version"];
          if ( version == "0" ) version = ACM_VERSION; // update from legacy to actual version 

          ignoreversion = reader["ignoreVersion"]; // could be either / or ..

          // get the joystick mapping if there is one
          for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
            jsN[i] = reader[String.Format( "js{0}", i + 1 )];
            jsNGUID[i] = reader[String.Format( "js{0}G", i + 1 )];
          }
        }
        else {
          return false;
        }
      }

      reader.Read( ); // move to next element

      // could be actionmap OR (AC 0.9) deviceoptions OR options

      while ( !reader.EOF ) { //!String.IsNullOrEmpty( x ) ) {

        if ( reader.Name == "actionmap" ) {
          String x = reader.ReadOuterXml( );
          ActionMapCls acm = new ActionMapCls( );
          if ( acm.fromXML( x ) ) {
            this.Merge( acm ); // merge list
          }
        }
        else if ( reader.Name == "CustomisationUIHeader" ) {
          String x = reader.ReadOuterXml( );
          m_uiCustHeader.fromXML( x );
        }
        else if ( reader.Name == "deviceoptions" ) {
          String x = reader.ReadOuterXml( );
          m_deviceOptions.fromXML( x );
        }
        else if ( reader.Name == "options" ) {
          String x = reader.ReadOuterXml( );
          m_options.fromXML( x );
        }
        else {
          reader.Read( );
        }

      }
      return true;
    }



  }
}
