

#define USE_DS_ACTIONMAPS


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Data;

using SCJMapper_V2.SC;
using SCJMapper_V2.Table;
using SCJMapper_V2.Keyboard;
using SCJMapper_V2.Mouse;
using SCJMapper_V2.Gamepad;
using SCJMapper_V2.Joystick;
using SCJMapper_V2.Options;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains the complete ActionMaps - something like:
  ///   
  /// <ActionMaps ActionMaps version="1" optionsVersion="2" rebindVersion="2" profileName="sdsd" > // AC2
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

    public static void LoadSupportedActionMaps( SCActionMapList aml )
    {
      // load actionmaps
      ActionMaps = aml.ActionMaps;
    }

    #endregion Static Part of ActionMaps

    private const String ACM_VERSION = "version=\"1\" optionsVersion=\"2\" rebindVersion=\"2\"";  //AC2  the FIXED version 

    private String version { get; set; }

    private JoystickList m_joystickList = null;
    private UICustHeader m_uiCustHeader = null;
    private OptionTree m_optionTree = null; // options are given per deviceClass and instance - it seems
    private Deviceoptions m_deviceOptions = null;
    private Modifiers m_modifiers = null;

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
    public void Clear_jsEntry( int index )
    {
      m_js[index] = "";
      m_GUIDs[index] = "";
    }

    // provide access to Tuning items of the Options obj to the owner

    /// <summary>
    /// Returns the device tuning items - the OptionTree
    /// </summary>
    public OptionTree OptionTree { get { return m_optionTree; } }


    /// <summary>
    /// Returns the DeviceOptions containing the deadzones
    /// </summary>
    public Deviceoptions DeviceOptions
    {
      get { return m_deviceOptions; }
    }


    /// <summary>
    /// Returns the assigned Modifiers
    /// </summary>
    public Modifiers Modifiers
    {
      get { return m_modifiers; }
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
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        m_js[i] = ""; m_GUIDs[i] = "";
      }

      CreateNewOptions( );

      //LoadSupportedActionMaps( ); // get them from config @@@@@@@@@
    }


    private void CreateNewOptions( )
    {
      // create options objs
      m_uiCustHeader = new UICustHeader( );
      m_optionTree = new OptionTree( );
      m_deviceOptions = new Deviceoptions( m_joystickList );
      m_modifiers = new Modifiers( );
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
      newMaps.m_optionTree = this.m_optionTree;
      newMaps.m_modifiers = this.m_modifiers;

      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
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
      } else {
        ACM.Merge( newAcm ); // yes, merge it
      }
    }


#if USE_DS_ACTIONMAPS  // see and (un)define on top of file to allow for editing the DataSet entities

    public void toDataSet( DS_ActionMaps dsa )
    {
      dsa.Clear( );
      if ( dsa.HasChanges( ) ) dsa.T_ActionMap.AcceptChanges( );

      int AMcount = 1;
      foreach ( ActionMapCls am in this ) {
        DS_ActionMaps.T_ActionMapRow amr =  dsa.T_ActionMap.NewT_ActionMapRow();
        string amShown = DS_ActionMap.ActionMapShown(am.name, AMcount++);

        amr.ID_ActionMap = amShown;
        dsa.T_ActionMap.AddT_ActionMapRow( amr );

        foreach ( ActionCls ac in am ) {
          int ilIndex = 0;
          while ( ac.inputList.Count > ilIndex ) {
            DS_ActionMaps.T_ActionRow ar =  dsa.T_Action.NewT_ActionRow();
            ar.ID_Action = DS_ActionMap.ActionID( am.name, ac.key, ac.inputList[ilIndex].NodeIndex ); // make a unique key
            ar.AddBind = ( ilIndex > 0 ); // all but the first are addbinds
            ar.REF_ActionMap = amShown;
            ar.ActionName = ac.name;
            ar.Device = ac.device;
            ar.Def_Binding = ac.defBinding;
            ar.Def_Modifier = ac.defActivationMode.Name;
            ar.Usr_Binding = ac.inputList[ilIndex].DevInput;
            ar.Usr_Modifier = ac.inputList[ilIndex].ActivationMode.Name;
            ar.Disabled = DeviceCls.IsBlendedInput( ac.inputList[ilIndex].Input );
            dsa.T_Action.AddT_ActionRow( ar );

            ilIndex++;
          }

        }// each Action
      }// each ActionMap

      // finally
      if ( dsa.HasChanges( ) )
        dsa.AcceptChanges( );
    }

    public void updateDataSet( DS_ActionMaps dsa, string actionID )
    {
      foreach ( ActionMapCls am in this ) {
        DS_ActionMaps.T_ActionMapRow amr =  dsa.T_ActionMap.NewT_ActionMapRow();

        foreach ( ActionCls ac in am ) {
          int ilIndex = 0;
          while ( ac.inputList.Count > ilIndex ) {
            if ( actionID == DS_ActionMap.ActionID( am.name, ac.key, ac.inputList[ilIndex].NodeIndex ) ) {
              DS_ActionMaps.T_ActionRow ar =dsa.T_Action.FindByID_Action(actionID);
              ar.Usr_Binding = ac.inputList[ilIndex].DevInput;
              ar.Usr_Modifier = ac.inputList[ilIndex].ActivationMode.Name;
              ar.Disabled = DeviceCls.IsBlendedInput( ac.inputList[ilIndex].Input );
              ar.AcceptChanges( );
              return;
            }
            ilIndex++;
          }
        }// each Action
      }// each ActionMap

    }

#endif



    /// <summary>
    /// Dump the ActionMaps as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( String fileName )
    {
      log.Debug( "ActionMapsCls.toXML - Entry" );

      AppSettings  appSettings = new AppSettings( );

      // *** HEADER  

      // handle the versioning of the actionmaps
      // AC2 do not longer support ignoreversion... enter the new fixed header
      String r = "<ActionMaps " + ACM_VERSION;
      r += String.Format( " profileName=\"{0}\" \n", fileName.Replace( SCMappings.c_MapStartsWith, "" ) ); //AC2 add profilename

      // now the devices (our addition)
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( jsN[i] ) ) r += String.Format( "\tjs{0}=\"{1}\" ", i + 1, jsN[i] );
        if ( !String.IsNullOrEmpty( jsNGUID[i] ) ) r += String.Format( "js{0}G=\"{1}\" \n", i + 1, jsNGUID[i] );
      }

      // close the tag
      r += String.Format( ">\n" );


      // *** CustomisationUIHeader

      // and dump the option contents - prepare with new data
      m_uiCustHeader.ClearInstances( );
      UICustHeader.DevRec dr = new UICustHeader.DevRec( );
      dr.devType = KeyboardCls.DeviceClass; dr.instNo = 1; m_uiCustHeader.AddInstances( dr );
      dr.devType = MouseCls.DeviceClass; dr.instNo = 1; m_uiCustHeader.AddInstances( dr );
      // do we use Gamepad ??
      if ( GamepadCls.RegisteredDevices > 0 ) {
        dr.devType = GamepadCls.DeviceClass; dr.instNo = 1; m_uiCustHeader.AddInstances( dr );
      }

      // all Joysticks
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( jsN[i] ) ) {
          dr.devType = JoystickCls.DeviceClass; dr.instNo = i + 1; m_uiCustHeader.AddInstances( dr );
        }
      }
      m_uiCustHeader.Label = fileName.Replace( SCMappings.c_MapStartsWith, "" ); // remove redundant part
      r += m_uiCustHeader.toXML( ) + String.Format( "\n" );

      // *** OPTIONS 
      if ( m_optionTree.Count > 0 ) r += m_optionTree.toXML( ) + String.Format( "\n" );

      // *** DEVICE OPTIONS
      if ( m_deviceOptions.Count > 0 ) r += m_deviceOptions.toXML( ) + String.Format( "\n" );

      // *** MODIFIERS
      if ( m_modifiers.Count > 0 ) r += m_modifiers.toXML( ) + String.Format( "\n" );

      // *** ACTION MAPS

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
      log.Debug( "ActionMapsCls.fromXML - Entry" );

      CreateNewOptions( ); // Reset those options...

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

          // get the joystick mapping if there is one
          for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
            jsN[i] = reader[String.Format( "js{0}", i + 1 )];
            jsNGUID[i] = reader[String.Format( "js{0}G", i + 1 )];
          }
        } else {
          return false;
        }
      }

      reader.Read( ); // move to next element

      // could be actionmap OR (AC 0.9) deviceoptions OR options

      while ( !reader.EOF ) { //!String.IsNullOrEmpty( x ) ) {

        if ( reader.Name.ToLowerInvariant( ) == "actionmap" ) {
          String x = reader.ReadOuterXml( );
          ActionMapCls acm = new ActionMapCls( );
          if ( acm.fromXML( x ) ) {
            this.Merge( acm ); // merge list
          }
        } else if ( reader.Name.ToLowerInvariant( ) == "customisationuiheader" ) {
          String x = reader.ReadOuterXml( );
          m_uiCustHeader.fromXML( x );
        } else if ( reader.Name.ToLowerInvariant( ) == "deviceoptions" ) {
          String x = reader.ReadOuterXml( );
          m_deviceOptions.fromXML( x );
        } else if ( reader.Name.ToLowerInvariant( ) == "options" ) {
          String x = reader.ReadOuterXml( );
          m_optionTree.fromXML( x );
        } else if ( reader.Name.ToLowerInvariant( ) == "modifiers" ) {
          String x = reader.ReadOuterXml( );
          m_modifiers.fromXML( x );
        } else {
          reader.Read( );
        }

      }
      return true;
    }



  }
}
