using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using SCJMapper_V2.Actions;
using System.Xml.Linq;
using System.Linq;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Gamepad;

namespace SCJMapper_V2.SC
{
  /// <summary>
  /// should read the default profile - may be replacing the MappingVars once
  /// Reads the profile as is and creates a CSV Map (internal legacy format) for clients
  /// default bindings are read as AC1 style items i.e. not containing the device
  /// </summary>
  class DProfileReader
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public bool ValidContent { get; set; }

    // an action map and its actions
    class ProfileAction
    {
      public string Name { get; set; }    // the action name
      public string DevID { get; set; }  // the input method K,J,X,P
      private string m_defBinding = "";   // NOTE: this is AC1 style in the Profile - need to conver later when dumping out
      public string DefBinding { get { return m_defBinding; } set { m_defBinding = value; } }  // DONT! need to clean this one, found spaces...

      private ActivationMode m_defActivationMode = ActivationMode.Default;
      public ActivationMode DefActivationMode { get { return m_defActivationMode; } set { m_defActivationMode = value; } }

      public string KeyName
      { get { return DevID + Name; } } // prep for TreView usage - create a key from input+name
    }


    class ActionMap : List<ProfileAction>  // carries the action list
    {
      public string Name { get; set; } // the map name

      static int ContainsLoop( List<ProfileAction> list, string value )
      {
        for ( int i = 0; i < list.Count; i++ ) {
          if ( list[i].KeyName == value ) {
            return i;
          }
        }
        return -1;
      }

      public new int IndexOf( ProfileAction pact )
      {
        return ContainsLoop( this, pact.KeyName );
      }

      public new bool Contains( ProfileAction pact )
      {
        return ( ContainsLoop( this, pact.KeyName ) >= 0 );
      }

      public new void Add( ProfileAction pact )
      {
        // only get the latest ..
        if ( this.Contains( pact ) )
          this.RemoveAt( IndexOf( pact ) );

        base.Add( pact );
      }
    };


    Dictionary<string, ActionMap> m_aMap = null; // key would be the actionmap name
    ActionMap m_currentMap = null;



    // ****************** CLASS **********************

    /// <summary>
    /// cTor
    /// </summary>
    public DProfileReader()
    {
      ValidContent = false; // default
    }


    /// <summary>
    /// Returns the collected actionmaps as CSV (same format as MappingVars)
    /// i.e. one line per actionmap where the first element is the actionmap and following are actions;defaultBinding lead by the input Key in uppercase (JKXP)
    /// </summary>
    public string CSVMap
    {
      get {
        log.Debug( "DProfileReader.CSVMap - Entry" );

        string buf = "";
        foreach ( ActionMap am in m_aMap.Values ) {
          buf += am.Name + ";";
          foreach ( ProfileAction a in am ) {
            buf += a.KeyName + ";" + a.DefBinding + ";" + a.DefActivationMode.Name + ";" + a.DefActivationMode.MultiTap.ToString( ) + ";"; // add default binding + activation mode to the CSV
          }
          buf += string.Format( "\n" );
        }
        return buf;
      }
    }



    // INPUT processing of the xml nodes

    private void ActModeInput( ref ProfileAction pa, XElement xml )
    {
      string actModeName = (string)xml.Attribute( "ActivationMode" );
      string multiTap = (string)xml.Attribute( "multiTap" );
      if ( string.IsNullOrEmpty( actModeName ) ) {
        actModeName = pa.DefActivationMode.Name; // from store
      }
      if ( string.IsNullOrEmpty( multiTap ) ) {
        multiTap = ActivationModes.Instance.MultiTapFor( actModeName ).ToString( ); // given by the already collected items
      }
      pa.DefActivationMode = new ActivationMode( actModeName, int.Parse( multiTap ) ); // renew
    }

    private void JInput( ref ProfileAction pa, XElement xml, string input )
    {
      ActModeInput( ref pa, xml );
      if ( !string.IsNullOrEmpty( input ) ) {
        pa.DefBinding = input;
        if ( pa.DefBinding == " " )
          pa.DefBinding = JoystickCls.DisabledInput;
        else if ( !string.IsNullOrEmpty( pa.DefBinding ) )
          pa.DefBinding = "js1_" + pa.DefBinding; // extend with device for mapping use
      }
    }

    private void KInput( ref ProfileAction pa, XElement xml, string input )
    {
      ActModeInput( ref pa, xml );
      if ( !string.IsNullOrEmpty( input ) ) {
        pa.DefBinding = input;
        if ( pa.DefBinding == " " )
          pa.DefBinding = KeyboardCls.DisabledInput;
        else if ( !string.IsNullOrEmpty( pa.DefBinding ) )
          pa.DefBinding = "kb1_" + pa.DefBinding; // extend with device for mapping use
      }
    }

    private void MInput( ref ProfileAction pa, XElement xml, string input )
    {
      ActModeInput( ref pa, xml );
      if ( !string.IsNullOrEmpty( input ) ) {
        pa.DefBinding = input;
        if ( pa.DefBinding == " " )
          pa.DefBinding = MouseCls.DisabledInput;
        else if ( !string.IsNullOrEmpty( pa.DefBinding ) )
          pa.DefBinding = "mo1_" + pa.DefBinding; // extend with device for mapping use
      }
    }

    private void XInput( ref ProfileAction pa, XElement xml, string input )
    {
      ActModeInput( ref pa, xml );
      if ( !string.IsNullOrEmpty( input ) ) {
        pa.DefBinding = input;
        if ( pa.DefBinding == " " )
          pa.DefBinding = GamepadCls.DisabledInput;
        else if ( !string.IsNullOrEmpty( pa.DefBinding ) )
          pa.DefBinding = "xi1_" + pa.DefBinding; // extend with device for mapping use
      }
    }

    /// <summary>
    /// Read all from an Action XElement
    /// </summary>
    /// <param name="action">The action XElement</param>
    /// <returns>True if successfull</returns>
    private bool ReadAction( XElement action )
    {
      /* A variety exists here...
		    <action  name="v_eject_cinematic"  onPress="0"  onHold="1"  onRelease="1"  keyboard="ralt+l"  xboxpad="shoulderl+shoulderr+y"  joystick=" "  />
		    <action  name="v_exit"  onPress="1"  onHold="0"  onRelease="1"  multiTap="1"  multiTapBlock="1"  pressTriggerThreshold="1.0"  releaseTriggerThreshold="-1"  releaseTriggerDelay="0"  keyboard="f"  xboxpad="y"  UILabel="@ui_CIExit"  UIDescription="@ui_CIExitDesc"  >
			    <joystick  ActivationMode="press"  input=" "  />
		    </action>
		    <action  name="v_throttle_100"  onPress="1"  xboxpad=" "  joystick=" "  UILabel="@ui_CIThrottleMax"  UIDescription="@ui_CIThrottleMaxDesc"  >
			    <xboxpad  multiTap="2"  input="thumbl_up"  />
			    <keyboard  multiTap="2"  input=" "  />
		    </action>

        <action  name="v_target_deselect_component"  ActivationMode="delayed_press"  pressTriggerThreshold="0.75"  joystick=""  >
			    <keyboard  >
				    <inputdata  input="lbracket"  />
				    <inputdata  input="rbracket"  />
			    </keyboard>
		    </action>
       
       */
      log.Debug( "DProfileReader.ReadAction - Entry" );
      // a complete actionmap arrives here
      bool retVal = true;

      string name = (string)action.Attribute( "name" );

      // prep all kinds
      var jAC = new ProfileAction( ) { Name = name, DevID = Act.DevTag( JoystickCls.DeviceClass ) };
      var kAC = new ProfileAction( ) { Name = name, DevID = Act.DevTag( KeyboardCls.DeviceClass ) };
      var mAC = new ProfileAction( ) { Name = name, DevID = Act.DevTag( MouseCls.DeviceClass ) };
      var xAC = new ProfileAction( ) { Name = name, DevID = Act.DevTag( GamepadCls.DeviceClass ) };

      // process element items
      JInput( ref jAC, action, (string)action.Attribute( JoystickCls.DeviceClass ) );
      KInput( ref kAC, action, (string)action.Attribute( KeyboardCls.DeviceClass ) );
      MInput( ref mAC, action, (string)action.Attribute( MouseCls.DeviceClass ) );
      XInput( ref xAC, action, (string)action.Attribute( GamepadCls.DeviceClass ) );

      // then nested ones - they may or may not exist from the initial scan
      foreach ( XElement l1action in action.Elements( ) ) {
        // comes with the name of the device class
        switch ( l1action.Name.LocalName ) {
          case JoystickCls.DeviceClass: {
              JInput( ref jAC, l1action, (string)l1action.Attribute( "input" ) ); // may have attributed ones
              foreach ( XElement l2action in l1action.Elements( ) ) {
                if ( l2action.Name.LocalName == "inputdata" ) {
                  JInput( ref jAC, l2action, (string)l2action.Attribute( "input" ) ); // or in the inputdata nesting
                }
              }
            }
            break;

          case KeyboardCls.DeviceClass: {
              KInput( ref kAC, l1action, (string)l1action.Attribute( "input" ) ); // may have attributed ones
              foreach ( XElement l2action in l1action.Elements( ) ) {
                if ( l2action.Name.LocalName == "inputdata" ) {
                  KInput( ref kAC, l2action, (string)l2action.Attribute( "input" ) ); // or in the inputdata nesting
                }
              }
            }
            break;

          case MouseCls.DeviceClass: {
              MInput( ref mAC, l1action, (string)l1action.Attribute( "input" ) ); // may have attributed ones
              foreach ( XElement l2action in l1action.Elements( ) ) {
                if ( l2action.Name.LocalName == "inputdata" ) {
                  MInput( ref mAC, l2action, (string)l2action.Attribute( "input" ) ); // or in the inputdata nesting
                }
              }
            }
            break;

          case GamepadCls.DeviceClass: {
              XInput( ref xAC, l1action, (string)l1action.Attribute( "input" ) ); // may have attributed ones
              foreach ( XElement l2action in l1action.Elements( ) ) {
                if ( l2action.Name.LocalName == "inputdata" ) {
                  XInput( ref xAC, l2action, (string)l2action.Attribute( "input" ) ); // or in the inputdata nesting
                }
              }
            }
            break;

          default: break;
        }
      }

      if ( !string.IsNullOrEmpty( jAC.DefBinding ) ) m_currentMap.Add( jAC );  // finally add it to the current map if it was bound
      if ( !string.IsNullOrEmpty( kAC.DefBinding ) ) m_currentMap.Add( kAC );  // finally add it to the current map if it was bound
      if ( !string.IsNullOrEmpty( mAC.DefBinding ) ) m_currentMap.Add( mAC );  // finally add it to the current map if it was bound
      if ( !string.IsNullOrEmpty( xAC.DefBinding ) ) m_currentMap.Add( xAC );  // finally add it to the current map if it was bound

      return retVal;
    }


    /// <summary>
    /// Read all from an Actionmap XElement
    /// </summary>
    /// <param name="actionmap">The Actionmap XElement</param>
    /// <returns>True if successfull</returns>
    private bool ReadActionmap( XElement actionmap )
    {
      log.Debug( "DProfileReader.ReadActionmap - Entry" );
      // a complete actionmap arrives here
      bool retVal = true;

      // check for a valid one
      string mapName = (string)actionmap.Attribute( "name" ); // mandatory
      string item = Array.Find( ActionMapsCls.ActionMaps, delegate ( string sstr ) { return sstr == mapName; } );
      if ( !string.IsNullOrEmpty( item ) ) {
        // finally.... it is a valid actionmap
        m_currentMap = new ActionMap( );
        m_currentMap.Name = mapName;
        if ( !m_aMap.ContainsKey( mapName ) ) { //20170325 - fix multiple map names - don't add the second, third etc. (CIG bug..)
          m_aMap.Add( mapName, m_currentMap ); // add to our inventory
          IEnumerable<XElement> actions = from x in actionmap.Elements( )
                                          where ( x.Name == "action" )
                                          select x;
          foreach ( XElement action in actions ) {
            // one action
            retVal &= ReadAction( action );
          }
        }
        else {
          log.DebugFormat( "ReadActionmap: IGNORED duplicate map with name: {0}", mapName );
        }

      }
      return retVal;
    }


    /// <summary>
    /// Read all from ActivationMode XElement
    /// </summary>
    /// <param name="actmodes">The Activatiomodes XElement</param>
    /// <returns>True if successfull</returns>
    private bool ReadActivationModes( XElement actmodes )
    {
      /*
        //<ActivationModes  >
        //	<ActivationMode  name="tap"  onPress="0"  onHold="0"  onRelease="1"  multiTap="1"  multiTapBlock="1"  pressTriggerThreshold="-1"  releaseTriggerThreshold="0.25"  releaseTriggerDelay="0"  />
        ... 
        //</ActivationModes>
       */
      log.Debug( "DProfileReader.ReadActivationModes - Entry" );

      IEnumerable<XElement> activationmodes = from x in actmodes.Elements( )
                                              where ( x.Name == "ActivationMode" )
                                              select x;
      foreach ( XElement activationmode in activationmodes ) {
        string name = (string)activationmode.Attribute( "name" );
        string mTap = (string)activationmode.Attribute( "multiTap" );
        if ( !string.IsNullOrEmpty( name ) ) ActivationModes.Instance.Add( new ActivationMode( name, int.Parse( mTap ) ) );
      }
      return true;
    }

    /// <summary>
    /// Read the defaultProfile.xml - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action defaultProfile Content</param>
    /// <returns>True if an action was decoded</returns>
    public bool fromXML( string xml )
    {
      log.Debug( "DProfileReader.fromXML - Entry" );

      // make sure we have them loaded ( refactoring to get a singleton or so...)
      if ( ActionMapsCls.ActionMaps.Length == 0 ) ActionMapsCls.LoadSupportedActionMaps( ActionMapList( xml ) );

      // read the content of the xml
      XmlReaderSettings settings = new XmlReaderSettings {
        ConformanceLevel = ConformanceLevel.Fragment,
        IgnoreWhitespace = true,
        IgnoreComments = true
      };

      using ( XmlReader reader = XmlReader.Create( new StringReader( xml ), settings ) ) {
        m_aMap = new Dictionary<string, ActionMap>( );
        // init the activation modes singleton
        ActivationModes.Instance.Clear( );
        ActivationModes.Instance.Add( ActivationMode.Default );

        ValidContent = true; // init

        reader.MoveToContent( );
        if ( XNode.ReadFrom( reader ) is XElement el ) {

          IEnumerable<XElement> activationModes = from x in el.Elements( )
                                                  where ( x.Name == "ActivationModes" )
                                                  select x;
          foreach ( XElement activationMode in activationModes ) {
            ValidContent &= ReadActivationModes( activationMode );
          }


          Modifiers.Instance.Clear( );
          IEnumerable<XElement> modifiers = from x in el.Elements( )
                                            where ( x.Name == "modifiers" )
                                            select x;
          foreach ( XElement modifier in modifiers ) {
            ValidContent &= Modifiers.Instance.FromXML( modifier );
          }

          IEnumerable<XElement> actionmaps = from x in el.Elements( )
                                             where ( x.Name == "actionmap" )
                                             select x;
          foreach ( XElement actionmap in actionmaps ) {
            ValidContent &= ReadActionmap( actionmap );
          }

        }
      }
      return ValidContent;
    }

    /// <summary>
    /// Returns the acionmals contained in the profile xml string
    /// </summary>
    /// <param name="xml">A default profile XML string</param>
    /// <returns>A filled SCActionMapList object</returns>
    public SCActionMapList ActionMapList( string xml )
    {
      log.Debug( "DProfileReader.ActionMapList - Entry" );

      SCActionMapList aml = new SCActionMapList( );
      XmlReaderSettings settings = new XmlReaderSettings {
        ConformanceLevel = ConformanceLevel.Fragment,
        IgnoreWhitespace = true,
        IgnoreComments = true
      };
      using ( XmlReader reader = XmlReader.Create( new StringReader( xml ), settings ) ) {
        reader.MoveToContent( );
        if ( XNode.ReadFrom( reader ) is XElement el ) {
          IEnumerable<XElement> actionmaps = from x in el.Elements( )
                                             where ( x.Name == "actionmap" )
                                             select x;
          foreach ( XElement actionmap in actionmaps ) {
            aml.AddActionMap( (string)actionmap.Attribute("name") );
          }
        }
      }
      return aml;
    }

  }
}
