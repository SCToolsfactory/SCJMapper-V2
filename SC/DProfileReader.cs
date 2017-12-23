using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using SCJMapper_V2.Actions;

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

    private Stack<string> m_nodeNameStack = null; // element name stack - keeping track where we are

    // state for the parser
    enum EState
    {
      idle = 0,
      inActionMap,
    }
    private EState m_state = EState.idle;

    // an action map and its actions
    class ProfileAction
    {
      public string name { get; set; }    // the action name
      public string devID { get; set; }  // the input method K,J,X,P
      private string m_defBinding = "";   // NOTE: this is AC1 style in the Profile - need to conver later when dumping out
      public string defBinding { get { return m_defBinding; } set { m_defBinding = value; } }  // DONT! need to clean this one, found spaces...

      private ActivationMode m_defActivationMode = ActivationMode.Default;
      public ActivationMode defActivationMode { get { return m_defActivationMode; } set { m_defActivationMode = value; } }

      public string keyName
      { get { return devID + name; } } // prep for TreView usage - create a key from input+name
    }


    class ActionMap : List<ProfileAction>  // carries the action list
    {
      public string name { get; set; } // the map name

      static int ContainsLoop( List<ProfileAction> list, string value )
      {
        for ( int i = 0; i < list.Count; i++ ) {
          if ( list[i].keyName == value ) {
            return i;
          }
        }
        return -1;
      }

      public new int IndexOf( ProfileAction pact )
      {
        return ContainsLoop( this, pact.keyName );
      }

      public new bool Contains( ProfileAction pact )
      {
        return ( ContainsLoop( this, pact.keyName ) >= 0 );
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
          buf += am.name + ";";
          foreach ( ProfileAction a in am ) {
            buf += a.keyName + ";" + a.defBinding + ";" + a.defActivationMode.Name + ";" + a.defActivationMode.MultiTap.ToString( ) + ";"; // add default binding + activation mode to the CSV
          }
          buf += string.Format( "\n" );
        }
        return buf;
      }
    }


    /// <summary>
    /// Assumes to be in an action element
    /// retrieves the attributes and collects the various control=binding pairs
    /// </summary>
    /// <param name="xr">An XML reader @ StartElement</param>
    private void CollectActions( Dictionary<string, string> attr )
    {
      //first find an ActivationMode if there is - applies to all actions
      string actModeName = ActivationMode.Default.Name;
      string multiTap = "0";

      // this can be an Activation Mode OR a multitap
      // if there is an activationMode the multiTap remains 0
      // if no ActivationMode is given, multitap is 1 or may be 2...
      if ( attr.ContainsKey( "ActivationMode" ) ) {
        actModeName = attr["ActivationMode"];
        multiTap = ActivationModes.Instance.MultiTapFor( actModeName ).ToString( ); // given by the already collected items
      }
      else {
        // name remains default - we handle multiTaps only here
        multiTap = "1"; // default if not changed in the action to may be 2 or so..
        if ( attr.ContainsKey( "multiTap" ) ) {
          multiTap = attr["multiTap"];
        }
      }
      ActivationMode actMode = new ActivationMode( actModeName, int.Parse( multiTap ) ); // should be a valid ActivationMode for this action

      // we collect actions for each input ie for K,J,X and M
      if ( attr.ContainsKey( "joystick" ) ) {
        ProfileAction ac = new ProfileAction( );
        ac.name = attr["name"];
        ac.devID = "J";
        ac.defBinding = attr["joystick"];
        ac.defActivationMode = actMode;
        if ( ac.defBinding == " " ) {
          ac.defBinding = Devices.Joystick.JoystickCls.BlendedInput;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
        else if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
          ac.defBinding = "js1_" + ac.defBinding;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
      }

      if ( attr.ContainsKey( "keyboard" ) ) {
        ProfileAction ac = new ProfileAction( );
        ac.name = attr["name"];
        ac.devID = "K";
        ac.defBinding = attr["keyboard"];
        ac.defActivationMode = actMode;
        if ( ac.defBinding == " " ) {
          ac.defBinding = Devices.Keyboard.KeyboardCls.BlendedInput;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
        else if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
          ac.defBinding = "kb1_" + ac.defBinding;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
      }

      if ( attr.ContainsKey( "mouse" ) ) {   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        ProfileAction ac = new ProfileAction( );
        ac.name = attr["name"];
        ac.devID = "M";
        ac.defBinding = attr["mouse"];
        ac.defActivationMode = actMode;
        if ( ac.defBinding == " " ) {
          ac.defBinding = Devices.Mouse.MouseCls.BlendedInput;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
        else if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
          ac.defBinding = "mo1_" + ac.defBinding;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
      }

      if ( attr.ContainsKey( "xboxpad" ) ) {
        ProfileAction ac = new ProfileAction( );
        ac.name = attr["name"];
        ac.devID = "X";
        ac.defBinding = attr["xboxpad"];
        ac.defActivationMode = actMode;
        if ( ac.defBinding == " " ) {
          ac.defBinding = Devices.Gamepad.GamepadCls.BlendedInput;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
        else if ( !string.IsNullOrEmpty( ac.defBinding ) ) {
          ac.defBinding = "xi1_" + ac.defBinding;
          m_currentMap.Add( ac );  // finally add it to the current map if it was bound
        }
      }
      if ( attr.ContainsKey( "ps3pad" ) ) {
        // ignore
      }
    }


    /// <summary>
    /// Read one 'empty' XML element
    /// 
    /// <name [attr="" ..] />
    /// 
    /// </summary>
    /// <param name="xr">An XML reader @ StartElement</param>
    /// <returns>True if reading can continue</returns>
    private bool ReadEmptyElement( XmlReader xr )
    {
      Dictionary<string, string> attr = new Dictionary<string, string>( );
      string eName = xr.Name;
      switch ( xr.NodeType ) {
        case XmlNodeType.Element:
          //Console.Write( "<{0}", xr.Name );
          while ( xr.MoveToNextAttribute( ) ) {
            attr.Add( xr.Name, xr.Value );  // save the attributes
            //Console.Write( " {0}='{1}'", xr.Name, xr.Value );
          }
          if ( m_state == EState.inActionMap ) {
            // processing a valid action map - collect actions
            if ( eName.ToLower( ) == "action" ) {
              // this is an action.. - collect it
              CollectActions( attr );
            }
          }// if inmap
          //Console.Write( ">\n" );

          break;
        case XmlNodeType.Text:
          //Console.Write( xr.Value );
          break;
        case XmlNodeType.EndElement:
          //Console.Write( "</{0}>\n", xr.Name );
          break;
      }

      return true;
    }

    /// <summary>
    /// Reads an action sub element
    /// </summary>
    /// <param name="xr">An XML reader @ StartElement</param>
    private void ReadActionSub( XmlReader xr, string actionName, string device )
    {

      /*
          <action name = "v_throttle_100" onPress = "1" xboxpad = " " joystick = " " UILabel = "@ui_CIThrottleMax" UIDescription = "@ui_CIThrottleMaxDesc" >
            <keyboard multiTap = "2" input = "w" />
          </action >
        or
          <action name="v_brake" onPress="1" onHold="1" onRelease="1" keyboard="space" xboxpad="xi_shoulderl+xi_shoulderr">
            <joystick input="js2_button7" />        
            <joystick input="js2_button8" />
          </action>
        or
          <action name="v_hud_confirm" onPress="1" onRelease="1" xboxpad="xi_triggerL_btn+xi_a" joystick="js1_button19">
            <keyboard>
              <inputdata input="enter"/>
            </keyboard>
          </action>
        or
          <action name="ui_up" onPress="1" onHold="1" holdTriggerDelay="0.15" holdRepeatDelay="0.15" >
            <keyboard>
              <inputdata input="up" />
            </keyboard>
            <xboxpad>
              <inputdata input="dpad_up" />
              <inputdata input="thumbly" useAnalogCompare="1" analogCompareVal="0.5" analogCompareOp="GREATERTHAN" />
              <inputdata input="thumbry" useAnalogCompare="1" analogCompareVal="0.5" analogCompareOp="GREATERTHAN" />
            </xboxpad>
          </action>

      */
      bool done = false;
      do {
        xr.Read( ); // get next element
        Dictionary<string, string> attr = new Dictionary<string, string>( );
        // add what is not contained in the structure we are about to parse
        attr.Add( "name", actionName );  // actionName is in the outer element

        string eName = xr.Name; // this is either the device or inputdata if there are multiple entries

        // read attributes if any
        while ( xr.MoveToNextAttribute( ) ) {
          attr.Add( xr.Name, xr.Value );  // save the attributes
          //Console.Write( " {0}='{1}'", xr.Name, xr.Value );
        }
        xr.MoveToElement( ); // backup

        // Have to add the device, otherwise the following does not work..
        if ( attr.ContainsKey( "input" ) ) {
          if ( !string.IsNullOrEmpty( device ) ) {
            // device is given i.e. enclosing the input statements
            // 20170512 - some keyboard entries are listed with mouse input ... ?!?
            // we substitute and add such mouse entries - don't know what the game does later...
            string ip = attr["input"];
            if ( !string.IsNullOrEmpty( ip ) ) {
              if ( ip.StartsWith( "mouse" ) ) {
                attr.Add( "mouse", attr["input"] ); // override the device given
              }
              else {
                attr.Add( device, attr["input"] ); // if the device is given, use it
              }
            }
          }
          else {
            string ip = attr["input"];
            if ( !string.IsNullOrEmpty( ip ) ) {
              attr.Add( eName, ip ); // else it should be the eName element
            }
          }
        }

        // the element name is a control
        if ( xr.NodeType == XmlNodeType.EndElement ) {
          done = ( xr.Name == m_nodeNameStack.Peek( ) ); // EXIT if the end element matches the entry
        }
        else if ( xr.IsEmptyElement ) {
          // an attribute only element
          CollectActions( attr );
        }
        else {
          // one with subelements again
          m_nodeNameStack.Push( xr.Name ); // recursive .. push element name to terminate later (this is i.e. keyboard) 
          ReadActionSub( xr, actionName, xr.Name );
        }
      } while ( !done );

      m_nodeNameStack.Pop( ); // action is finished
    }

    /// <summary>
    /// Read one XML element 
    /// 
    /// <name attr="">
    ///   [ Xelement ]
    /// </name>
    /// 
    /// </summary>
    /// <param name="xr">An XML reader @ StartElement</param>
    /// <returns>True if reading can continue</returns>
    private bool ReadElement( XmlReader xr )
    {
      Dictionary<string, string> attr = new Dictionary<string, string>( );
      string eName = xr.Name;
      switch ( xr.NodeType ) {
        case XmlNodeType.Element:
          //Console.Write( "<{0}", xr.Name );
          while ( xr.MoveToNextAttribute( ) ) {
            attr.Add( xr.Name, xr.Value );  // save the attributes
            //Console.Write( " {0}='{1}'", xr.Name, xr.Value );
          }
          // now here we could have an actionmap start
          if ( m_state == EState.idle ) {
            if ( m_nodeNameStack.Peek( ).ToLower( ) == "actionmap" ) {
              // check for a valid one
              string mapName = attr["name"];
              string item = Array.Find( ActionMapsCls.ActionMaps, delegate ( string sstr ) { return sstr == mapName; } );
              if ( !string.IsNullOrEmpty( item ) ) {
                // finally.... it is a valid actionmap
                m_currentMap = new ActionMap( );
                m_currentMap.name = mapName;
                if ( !m_aMap.ContainsKey( mapName ) ) { //20170325 - fix multiple map names - don't add the second, third etc. (CIG bug..)
                  m_aMap.Add( mapName, m_currentMap ); // add to our inventory
                }
                else {
                  log.DebugFormat( "ReadElement: IGNORED duplicate map with name: {0}", mapName );
                }
                m_state = EState.inActionMap; // now we are in and processing the map
              }
            }
          }
          else if ( m_state == EState.inActionMap ) {
            // processing a valid action map - collect actions
            if ( eName.ToLower( ) == "action" ) {
              // this is an action.. - collect it
              CollectActions( attr );
              ReadActionSub( xr, attr["name"], "" ); // a non empty action element may have a sub element (but no device yet)
            }
          }
          //Console.Write( ">\n" );
          break;
        case XmlNodeType.Text:
          //Console.Write( xr.Value );
          break;
        case XmlNodeType.EndElement:
          //Console.Write( "</{0}>\n", xr.Name );
          break;
      }
      return true;
    }


    /// <summary>
    /// Read the xml part
    /// </summary>
    /// <param name="xr"></param>
    /// <returns></returns>
    private bool ReadXML( XmlReader xr )
    {
      log.Debug( "DProfileReader.ReadXML - Entry" );

      bool retVal = true;

      try {
        do {
          if ( xr.IsStartElement( ) ) {
            m_nodeNameStack.Push( xr.Name );
            if ( xr.IsEmptyElement ) {
              retVal = retVal && ReadEmptyElement( xr );
              m_nodeNameStack.Pop( ); // empty ones end inline
            }
            else {
              retVal = retVal && ReadElement( xr );
            }
          }
          else if ( xr.NodeType == XmlNodeType.EndElement ) {
            //Console.Write( "</{0}>\n", xr.Name );
            string exitElement = m_nodeNameStack.Pop( );
            if ( m_state == EState.inActionMap )
              if ( exitElement.ToLower( ) == "actionmap" ) m_state = EState.idle; // finished 
          }

        } while ( xr.Read( ) );

        if ( m_nodeNameStack.Count == 0 )
          return retVal && true;
        else
          return false;

      }
      catch ( Exception ex ) {
        // get any exceptions from reading
        log.Error( "DProfileReader.ReadXML - unexpected", ex );
        return false;
      }
    }

    /*
	//<ActivationModes  >
	//	<ActivationMode  name="tap"  onPress="0"  onHold="0"  onRelease="1"  multiTap="1"  multiTapBlock="1"  pressTriggerThreshold="-1"  releaseTriggerThreshold="0.25"  releaseTriggerDelay="0"  />
  ... 
	//</ActivationModes>
     */
    private bool ReadActivationModes( XmlReader xr )
    {
      log.Debug( "DProfileReader.ReadActivationModes - Entry" );

      try {
        xr.ReadToFollowing( "ActivationModes" );
        xr.ReadToDescendant( "ActivationMode" );
        do {
          if ( xr.NodeType == XmlNodeType.EndElement ) {
            xr.Read( );
            break; // finished
          }
          string name = xr["name"];
          string mTap = xr["multiTap"];
          if ( !string.IsNullOrEmpty( name ) ) ActivationModes.Instance.Add( new ActivationMode( name, int.Parse( mTap ) ) );
        } while ( xr.Read( ) );

        return true;
      }
      catch ( Exception ex ) {
        // get any exceptions from reading
        log.Error( "DProfileReader.ReadXML - unexpected", ex );
        return false;
      }
    }


    // Read modifiers

    //<modifiers>
    //  <mod input = "xi_shoulderl" />
    //  <mod input ="xi_triggerl_btn"  />
    //  <mod input = "xi_dpad_down" />
    //  <mod input ="xi_y"  />
    //  <mod input = "kb_z" />
    //  <mod input ="kb_f3"  />
    //  <mod input = "kb_lalt" />

    //  <mod input = "kb_lalt" />
    //  <mod input = "kb_f4"  />
    //  <mod input = "kb_mouse2" />

    //  <mod input = "xi_dpad_down" />
    //  <mod input = "xi_shoulderl"  />
    //  <mod input = "xi_triggerl_btn" />

    //</ modifiers >

    private bool ReadModifiers( XmlReader xr )
    {
      log.Debug( "DProfileReader.ReadModifiers - Entry" );

      try {
        xr.ReadToFollowing( "modifiers" );
        return Modifiers.Instance.FromXML( xr.ReadOuterXml(), true );
      }
      catch ( Exception ex ) {
        // get any exceptions from reading
        log.Error( "DProfileReader.ReadXML - unexpected", ex );
        return false;
      }
    }


    /// <summary>
    /// Read the defaultProfile.xml - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action defaultProfile Content</param>
    /// <returns>True if an action was decoded</returns>
    public bool fromXML( string xml )
    {
      log.Debug( "DProfileReader.fromXML - Entry" );

      if ( ActionMapsCls.ActionMaps.Length == 0 ) ActionMapsCls.LoadSupportedActionMaps( ActionMapList( xml ) ); // make sure we have them loaded ( refactoring to get a singleton or so...)

      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );


      m_nodeNameStack = new Stack<string>( );
      m_aMap = new Dictionary<string, ActionMap>( );
      // init the activation modes singleton
      ActivationModes.Instance.Clear( );
      ActivationModes.Instance.Add( ActivationMode.Default );

      ValidContent = true; // init
      reader.Read( );
      ValidContent &= ReadActivationModes( reader );

      Modifiers.Instance.Clear( );
      ValidContent &= ReadModifiers( reader );

      m_nodeNameStack.Push( "profile" ); // we are already in the XML now

      ValidContent &= ReadXML( reader );

      return ValidContent;
    }

    /// <summary>
    /// Returns the acionmals contained in the profile xml string
    /// </summary>
    /// <param name="xml">A default profile XML string</param>
    /// <returns>A filled SCActionMapList object</returns>
    public SCActionMapList ActionMapList( string xml )
    {
      SCActionMapList aml = new SCActionMapList( );

      log.Debug( "DProfileReader.ActionMapList - Entry" );

      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );
      if ( !reader.ReadToFollowing( "actionmap" ) ) return aml; // ERROR empty one..
      do {
        string attr = reader["name"];
        if ( !string.IsNullOrEmpty( attr ) )
          aml.AddActionMap( attr );
      } while ( reader.ReadToFollowing( "actionmap" ) );

      return aml;
    }

  }
}
