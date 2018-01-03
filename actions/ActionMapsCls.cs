using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Data;

using SCJMapper_V2.SC;
using SCJMapper_V2.Table;
using SCJMapper_V2.Devices;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Devices.Options;
using System.Linq;
using System.Xml.Linq;

namespace SCJMapper_V2.Actions
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
    public static string[] ActionMaps = { };

    public static void LoadSupportedActionMaps( SCActionMapList aml )
    {
      // load actionmaps
      ActionMaps = aml.ActionMaps;
    }

    #endregion Static Part of ActionMaps

    private const string ACM_VERSION = "version=\"1\" optionsVersion=\"2\" rebindVersion=\"2\"";  //AC2  the FIXED version 

    private string version { get; set; }

    private UICustHeader m_uiCustHeader = null;

    private Tuningoptions m_tuningOptions = null;
    private Deviceoptions m_deviceOptions = null;

    // own additions for JS mapping - should not harm..
    private string[] m_js;
    private string[] m_GUIDs;

    /// <summary>
    /// get/set jsN assignment (use 0-based index i.e. js1 -> [0])
    /// </summary>
    public string[] jsN
    {
      get { return m_js; }
    }

    /// <summary>
    /// get/set jsN GUID assignment (use 0-based index i.e. js1GUID -> [0])
    /// </summary>
    public string[] jsNGUID
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
    public Tuningoptions TuningOptions { get { return m_tuningOptions; } }

    /// <summary>
    /// Returns the DeviceOptions containing the deadzones and saturation
    /// </summary>
    public Deviceoptions DeviceOptions { get { return m_deviceOptions; } }


    /// <summary>
    /// Copy return all ActionMaps while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionMaps copy with reassigned input</returns>
    public ActionMapsCls ReassignJsN( JsReassingList newJsList )
    {
      var newMaps = new ActionMapsCls( this ) {
        m_uiCustHeader = (UICustHeader)this.m_uiCustHeader.Clone( ),
        m_tuningOptions = (Tuningoptions)this.m_tuningOptions.Clone( ),
        m_deviceOptions = (Deviceoptions)this.m_deviceOptions.Clone( )
      };

      foreach ( ActionMapCls am in this ) {
        newMaps.Add( am.ReassignJsN( newJsList ) );  // creates the deep copy of the tree
      }
      // remap the tuning options
      newMaps.m_tuningOptions.ReassignJsN( newJsList );

      return newMaps;
    }

    /// <summary>
    /// cTor: private copy constructor
    /// </summary>
    /// <param name="other"></param>
    private ActionMapsCls( ActionMapsCls other )
    {
      this.version = other.version;
      this.m_js = other.m_js;
      this.m_GUIDs = other.m_GUIDs;
      // other ref objects are not populated here    
    }

    /// <summary>
    /// cTor: plain, initializes values
    /// </summary>
    public ActionMapsCls()
    {
      version = ACM_VERSION;

      // create the Joystick assignments
      Array.Resize( ref m_js, JoystickCls.JSnum_MAX + 1 );
      Array.Resize( ref m_GUIDs, JoystickCls.JSnum_MAX + 1 );
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        m_js[i] = ""; m_GUIDs[i] = "";
      }
      // create the default mapped optiontree
     // CreateNewOptions( );
    }

    /// <summary>
    /// Helper to create all needed objs
    /// </summary>
    public void CreateNewOptions()
    {
      // create options objs
      m_uiCustHeader = new UICustHeader( );
      m_tuningOptions = new Tuningoptions( );
      m_deviceOptions = new Deviceoptions( );
    }

    /// <summary>
    /// Merge the given Map with this Map
    /// new ones are ignored - we don't learn from XML input for the time beeing
    /// </summary>
    /// <param name="newAcm"></param>
    private void Merge( ActionMapCls newAcm )
    {
      // do we find an actionmap like the new one in our list ?
      ActionMapCls ACM = this.Find( delegate ( ActionMapCls acm ) {
        return acm.MapName == newAcm.MapName;
      } );
      if ( ACM == null ) {
        ; // this.Add( newAcm ); // no, add new
      }
      else {
        ACM.Merge( newAcm ); // yes, merge it
      }
    }


    /// <summary>
    /// Converts all maps into a DataSet
    /// </summary>
    /// <param name="dsa">The dataset to populate</param>
    public void ToDataSet( DS_ActionMaps dsa )
    {
      dsa.Clear( );
      if ( dsa.HasChanges( ) ) dsa.T_ActionMap.AcceptChanges( );

      int AMcount = 1;
      foreach ( ActionMapCls am in this ) {
        DS_ActionMaps.T_ActionMapRow amr = dsa.T_ActionMap.NewT_ActionMapRow( );
        string amShown = DS_ActionMap.ActionMapShown( SCUiText.Instance.Text( am.MapName), AMcount++ );

        amr.ID_ActionMap = amShown;
        dsa.T_ActionMap.AddT_ActionMapRow( amr );

        foreach ( ActionCls ac in am ) {
          int ilIndex = 0;
          while ( ac.InputList.Count > ilIndex ) {
            DS_ActionMaps.T_ActionRow ar = dsa.T_Action.NewT_ActionRow( );
            ar.ID_Action = DS_ActionMap.ActionID( am.MapName, ac.Key, ac.InputList[ilIndex].NodeIndex ); // make a unique key
            ar.AddBind = ( ilIndex > 0 ); // all but the first are addbinds
            ar.REF_ActionMap = amShown;
            ar.ActionName = ac.ActionName;
            ar.ActionText = SCUiText.Instance.Text( ac.ActionName );
            ar.Device = ac.Device;
            ar.Def_Binding = ac.DefBinding;
            ar.Def_Modifier = ac.DefActivationMode.Name;
            ar.Usr_Binding = ac.InputList[ilIndex].DevInput;
            ar.Usr_Modifier = ac.InputList[ilIndex].ActivationMode.Name;
            ar.Disabled = DeviceCls.IsDisabledInput( ac.InputList[ilIndex].Input );
            dsa.T_Action.AddT_ActionRow( ar );

            ilIndex++;
          }

        }// each Action
      }// each ActionMap

      // finally
      if ( dsa.HasChanges( ) )
        dsa.AcceptChanges( );
    }

    /// <summary>
    /// Update the dataset
    /// </summary>
    /// <param name="dsa">The dataset</param>
    /// <param name="actionID">The actionID to update from</param>
    public void UpdateDataSet( DS_ActionMaps dsa, string actionID )
    {
      foreach ( ActionMapCls am in this ) {
        DS_ActionMaps.T_ActionMapRow amr = dsa.T_ActionMap.NewT_ActionMapRow( );

        foreach ( ActionCls ac in am ) {
          int ilIndex = 0;
          while ( ac.InputList.Count > ilIndex ) {
            if ( actionID == DS_ActionMap.ActionID( am.MapName, ac.Key, ac.InputList[ilIndex].NodeIndex ) ) {
              DS_ActionMaps.T_ActionRow ar = dsa.T_Action.FindByID_Action( actionID );
              ar.Usr_Binding = ac.InputList[ilIndex].DevInput;
              ar.Usr_Modifier = ac.InputList[ilIndex].ActivationMode.Name;
              ar.Disabled = DeviceCls.IsDisabledInput( ac.InputList[ilIndex].Input );
              ar.AcceptChanges( );
              return;
            }
            ilIndex++;
          }
        }// each Action
      }// each ActionMap
    }

    /// <summary>
    /// Dump the ActionMaps as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public string toXML( string fileName )
    {
      log.Debug( "ActionMapsCls.toXML - Entry" );

      AppSettings appSettings = AppSettings.Instance; // shortcut only

      // *** HEADER  

      // handle the versioning of the actionmaps
      // AC2 do not longer support ignoreversion... enter the new fixed header
      string r = "<ActionMaps " + ACM_VERSION;
      r += string.Format( " profileName=\"{0}\" \n", fileName.Replace( SCMappings.c_MapStartsWith, "" ) ); //AC2 add profilename

      // now the devices (our addition)
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !string.IsNullOrEmpty( jsN[i] ) ) r += string.Format( "\tjs{0}=\"{1}\" ", i + 1, jsN[i] );
        if ( !string.IsNullOrEmpty( jsNGUID[i] ) ) r += string.Format( "js{0}G=\"{1}\" \n", i + 1, jsNGUID[i] );
      }

      // close the tag
      r += string.Format( ">\n" );

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
        if ( !string.IsNullOrEmpty( jsN[i] ) ) {
          dr.devType = JoystickCls.DeviceClass; dr.instNo = i + 1; m_uiCustHeader.AddInstances( dr );
        }
      }
      m_uiCustHeader.Label = fileName.Replace( SCMappings.c_MapStartsWith, "" ); // remove redundant part
      r += m_uiCustHeader.toXML( ) + string.Format( "\n" );

      // *** OPTIONS 
      foreach ( KeyValuePair<string, OptionTree> kv in m_tuningOptions ) {
        if ( kv.Value.Count > 0 ) r += kv.Value.toXML( ) + string.Format( "\n" );
      }

      // *** DEVICE OPTIONS
      if ( m_deviceOptions.Count > 0 ) r += m_deviceOptions.toXML( ) + string.Format( "\n" );

      // *** MODIFIERS
      if ( SC.Modifiers.Instance.UserCount > 0 ) r += SC.Modifiers.Instance.ToXML( ) + string.Format( "\n" );

      // *** ACTION MAPS
      foreach ( ActionMapCls amc in this ) {
        r += string.Format( "{0}\n", amc.toXML( ) );
      }
      r += string.Format( "</ActionMaps>\n" );

      // tidy up..
      return r.Replace( string.Format( "\n\n" ), string.Format( "\n" ) );
    }


    /// <summary>
    /// Read an ActionMaps from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public bool fromXML( string xml )
    {
      log.Debug( "ActionMapsCls.fromXML - Entry" );

      XmlReaderSettings settings = new XmlReaderSettings {
        ConformanceLevel = ConformanceLevel.Fragment,
        IgnoreWhitespace = true,
        IgnoreComments = true
      };
      using ( XmlReader reader = XmlReader.Create( new StringReader( xml ), settings ) ) {

        reader.MoveToContent( );
        if ( reader.EOF ) return false;

        if ( XNode.ReadFrom( reader ) is XElement el ) {

          // read the header element
          if ( el.Name.LocalName == "ActionMaps" ) {
            if ( el.HasAttributes ) {
              version = (string)el.Attribute( "version" );
              if ( version == "0" ) version = ACM_VERSION; // update from legacy to actual version 

              // get the joystick mapping if there is one
              for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
                jsN[i] = (string)el.Attribute( string.Format( "js{0}", i + 1 ) );
                jsNGUID[i] = (string)el.Attribute( string.Format( "js{0}G", i + 1 ) );
              }
            }
            else {
              return false;
            }
          }

          // now handle the js assignment from the map
          // Reset with the found mapping
          DeviceInst.JoystickListRef.ResetJsNAssignment( jsNGUID );
          // Only now create the default optiontree for this map, containing included joysticks and the gamepad
          CreateNewOptions( );


          // now read the CIG content of the map

          IEnumerable<XElement> actionmaps = from x in el.Elements( )
                                             where ( x.Name == "actionmap" )
                                             select x;
          foreach ( XElement actionmap in actionmaps ) {
            ActionMapCls acm = new ActionMapCls( );
            if ( acm.fromXML( actionmap ) ) {
              this.Merge( acm ); // merge list
            }
          }

          IEnumerable<XElement> custHeaders = from x in el.Elements( )
                                              where ( x.Name == UICustHeader.XmlName )
                                              select x;
          foreach ( XElement custHeader in custHeaders ) {
            m_uiCustHeader.fromXML( custHeader );
          }

          IEnumerable<XElement> deviceOptions = from x in el.Elements( )
                                                where ( x.Name == "deviceoptions" )
                                                select x;
          foreach ( XElement deviceOption in deviceOptions ) {
            m_deviceOptions.fromXML( deviceOption );
          }

          IEnumerable<XElement> options = from x in el.Elements( )
                                          where ( x.Name == "options" )
                                          select x;
          foreach ( XElement option in options ) {
            m_tuningOptions.fromXML( option );
          }

          IEnumerable<XElement> modifiers = from x in el.Elements( )
                                            where ( x.Name == "modifiers" )
                                            select x;
          foreach ( XElement modifier in modifiers ) {
            SC.Modifiers.Instance.FromXML( modifier );
          }
        }

      }
      return true;
    }

  }
}
