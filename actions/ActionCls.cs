using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using SCJMapper_V2.SC;
using SCJMapper_V2.Keyboard;
using SCJMapper_V2.Mouse;
using SCJMapper_V2.Gamepad;
using SCJMapper_V2.Joystick;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains an action - something like:
  ///   
  /// 		<action name="v_view_cycle_fwd">
  /// 					<rebind device="joystick" input="js2_button2" />
  /// 				</action>
  /// 				
  /// 
  /// AC1.0
  /// <action name="v_roll">
  ///   <rebind device="joystick" input="js1_rotz" />
  ///   <addbind device="joystick" input="js2_x" />
  /// </action>
  /// 
  /// AC1.1
  /// <action name="v_roll">
  ///   <rebind device="joystick" input="rctrl+js1_rotz" />
  /// </action>
  /// 
  /// AC2.0
  /// <action name="v_roll">
  ///   <rebind input="js1_rotz" />  // jsN, moN, kbN (gamepad ?)
  ///   <addbind input="js1_rotz" />  // jsN, moN, kbN (gamepad ?) still possible together with rebind?
  /// </action>
  /// 
  /// </summary>
  public class ActionCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    /// <summary>
    /// Device Enums
    /// </summary>
    public enum ActionDevice
    {
      AD_Unknown = -1,
      AD_Joystick = 0,
      AD_Gamepad,
      AD_Keyboard,
      AD_Mouse,   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
    }

    #region Static Items

    /// <summary>
    /// Return the Device Enum from a DeviceClass string
    /// </summary>
    /// <param name="deviceClass">Device Class string</param>
    /// <returns>Device Enum</returns>
    static public ActionDevice ADevice( String deviceClass )
    {
      switch ( deviceClass.ToLower( ) ) {
        case KeyboardCls.DeviceClass: return ActionDevice.AD_Keyboard;
        case JoystickCls.DeviceClass: return ActionDevice.AD_Joystick;
        case GamepadCls.DeviceClass: return ActionDevice.AD_Gamepad;
        case MouseCls.DeviceClass: return ActionDevice.AD_Mouse;   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        case "ps3pad": return ActionDevice.AD_Gamepad;
        default: return ActionDevice.AD_Unknown;
      }
    }

    // Static items to have this mapping in only one place

    /// <summary>
    /// Returns the Device Tag i.e. the single letter to mark a device in Actions
    /// </summary>
    /// <param name="device">The device name from the defaultProfile</param>
    /// <returns>The single UCase device Tag letter</returns>
    static public String DevTag( String device )
    {
      switch ( device.ToLower( ) ) {
        case KeyboardCls.DeviceClass: return "K";
        case JoystickCls.DeviceClass: return "J";
        case GamepadCls.DeviceClass: return "X";
        case MouseCls.DeviceClass: return "M";  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        case "ps3pad": return "P";
        default: return "Z";
      }
    }

    /// <summary>
    /// Returns the Device name from the Device Tag
    /// </summary>
    /// <param name="device">The single UCase device Tag letter</param>
    /// <returns>The device name from the defaultProfile</returns>
    static public String DeviceClassFromTag( String devTag )
    {
      switch ( devTag ) {
        case "K": return KeyboardCls.DeviceClass;
        case "J": return JoystickCls.DeviceClass;
        case "X": return GamepadCls.DeviceClass;
        case "M": return MouseCls.DeviceClass;  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
        case "P": return "ps3pad";
        default: return "unknown";
      }
    }


    /// <summary>
    /// Try to derive the device class from the devInput string (mo1_, kb1_, xi1_, jsN_)
    /// </summary>
    /// <param name="devInput">The input command string dev_input format</param>
    /// <returns>A proper DeviceClass string</returns>
    static public String DeviceClassFromInput( String devInput )
    {
      String deviceClass = DeviceCls.DeviceClass;

      deviceClass = JoystickCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      deviceClass = GamepadCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      deviceClass = KeyboardCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      deviceClass = MouseCls.DeviceClassFromInput( devInput );
      if ( !DeviceCls.IsUndefined( deviceClass ) ) return deviceClass;
      // others..
      return deviceClass;
    }

    /// <summary>
    /// Returns the ActionDevice from a deviceID (a trailing _ is added if not there)
    /// </summary>
    /// <param name="devID">DeviceID</param>
    /// <returns>The ActionDevice</returns>
    static public ActionDevice ADeviceFromDevID( string devID )
    {
      string val = devID;
      if ( !devID.EndsWith( "_" ) ) val += "_";
      return ADevice( DeviceClassFromInput( val ) );
    }

    /// <summary>
    /// Returns the ActionDevice from the devInput string (mo1_, kb1_, xi1_, jsN_)
    /// </summary>
    /// <param name="devInput">The input command string dev_input format</param>
    /// <returns>The ActionDevice</returns>
    static public ActionDevice ADeviceFromInput( string devInput )
    {
      return ADevice( DeviceClassFromInput( devInput ) );
    }

    /// <summary>
    /// Query the devices if the input is blended
    /// </summary>
    /// <param name="input">The input command</param>
    /// <returns>True if blended input</returns>
    static public Boolean IsBlendedInput( String input )
    {
      Boolean blendedInput = false;

      blendedInput = DeviceCls.IsBlendedInput( input ); // generic
      if ( blendedInput ) return blendedInput;

      blendedInput = JoystickCls.IsBlendedInput( input );
      if ( blendedInput ) return blendedInput;
      blendedInput = GamepadCls.IsBlendedInput( input );
      if ( blendedInput ) return blendedInput;
      blendedInput = KeyboardCls.IsBlendedInput( input );
      if ( blendedInput ) return blendedInput;
      blendedInput = MouseCls.IsBlendedInput( input );
      if ( blendedInput ) return blendedInput;
      // others..
      return blendedInput;
    }

    /// <summary>
    /// Blend the input using the device specific format of the input is generic Blind
    /// </summary>
    /// <param name="input">An input (generic blend or a valid command)</param>
    /// <param name="aDevice">A valid device</param>
    /// <returns>A device blend or the original input if it was not a blend</returns>
    static public String BlendInput( String input, ActionDevice aDevice )
    {
      if ( DeviceCls.IsBlendedInput( input ) ) {
        // was generic blind
        switch ( aDevice ) {
          case ActionDevice.AD_Gamepad: return GamepadCls.BlendedInput;
          case ActionDevice.AD_Joystick: return JoystickCls.BlendedInput;
          case ActionDevice.AD_Keyboard: return KeyboardCls.BlendedInput;
          case ActionDevice.AD_Mouse: return MouseCls.BlendedInput;
          default: return "";
        }
      }
      else {
        return input; // just return
      }
    }

    /// <summary>
    /// Extends the input to a device input if not already done
    /// </summary>
    /// <param name="input">An input</param>
    /// <param name="aDevice">The ActionDevice</param>
    /// <returns>A valid devInput (dev_input) format</returns>
    static public String DevInput( string input, ActionDevice aDevice )
    {
      switch ( aDevice ) {
        case ActionDevice.AD_Gamepad: return GamepadCls.DevInput( input );
        case ActionDevice.AD_Joystick: return JoystickCls.DevInput( input );
        case ActionDevice.AD_Keyboard: return KeyboardCls.DevInput( input );
        case ActionDevice.AD_Mouse: return MouseCls.DevInput( input );
        default: return input;
      }
    }

    /// <summary>
    /// Return the color of a device
    /// </summary>
    /// <param name="devInput">The devinput (determine JS colors)</param>
    /// <param name="aDevice">The ActionDevice</param>
    /// <returns>The device color</returns>
    static public System.Drawing.Color DeviceColor( string devInput )
    {
      // background is along the input 
      ActionDevice aDevice = ADeviceFromInput( devInput);
      switch ( aDevice ) {
        case ActionDevice.AD_Gamepad: return GamepadCls.XiColor( );
        case ActionDevice.AD_Joystick: {
            int jNum = JoystickCls.JSNum( devInput ); // need to know which JS 
            return JoystickCls.JsNColor( jNum );
          }
        case ActionDevice.AD_Keyboard: return KeyboardCls.KbdColor( );
        case ActionDevice.AD_Mouse: return MouseCls.MouseColor( );
        default: return MyColors.UnassignedColor;
      }
    }


    #endregion


    // ****************  Class items **********************

    public String key { get; set; }                 // the key is the "Daction" formatted item (as we can have the same name multiple times)
    public String name { get; set; }                // the plain action name e.g. v_yaw
    public ActionDevice actionDevice { get; set; }  // the enum of the device
    public String device { get; set; }              // name of the device (uses DeviceClass)
    public String defBinding { get; set; }          // the default binding
    public ActivationMode defActivationMode { get; set; }   // the default binding ActivationMode
    public List<ActionCommandCls> inputList { get; set; } // regular bind is the 0-element, addbinds are added to the list

    /// <summary>
    /// ctor
    /// </summary>
    public ActionCls( )
    {
      key = "";
      actionDevice = ActionDevice.AD_Unknown;
      device = JoystickCls.DeviceClass;
      name = "";
      defBinding = "";
      defActivationMode = ActivationMode.Default;
      inputList = new List<ActionCommandCls>( ); // empty list
    }


    /// <summary>
    /// Copy return the action while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The action copy with reassigned input</returns>
    public ActionCls ReassignJsN( JsReassingList newJsList )
    {
      ActionCls newAc = new ActionCls( );
      // full copy from 'this'
      newAc.key = this.key;
      newAc.actionDevice = this.actionDevice;
      newAc.device = this.device;
      newAc.name = this.name;
      newAc.defBinding = this.defBinding;
      newAc.defActivationMode = this.defActivationMode;

      // creates a copy of the list with reassigned jsN devs
      foreach ( ActionCommandCls acc in inputList ) {
        newAc.inputList.Add( acc.ReassignJsN( newJsList ) );
      }

      return newAc;
    }


    /// <summary>
    /// Creates and adds the inputCommand list with given input string
    /// AC2 style input is used i.e. with device tag in front
    ///   apply given ActivationMode - can be "~" to indicate DONT APPLY 
    /// </summary>
    /// <param name="devInput"></param>
    /// <returns>Returns the ActionCommand created</returns>
    public ActionCommandCls AddCommand( String devInput, ActivationMode activationMode )
    {
      ActionCommandCls acc = new ActionCommandCls( devInput, inputList.Count - 1 ); // starts from -1 ...
      acc.ActivationMode = new ActivationMode( activationMode );
      inputList.Add( acc );
      return acc;
    }

    /// <summary>
    /// Add an ActionCommand with Input at nodeindex
    ///   apply default ActivationMode
    /// </summary>
    /// <param name="devInput"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public ActionCommandCls AddCommand( String devInput, int index )
    {
      ActionCommandCls acc = new ActionCommandCls( devInput, index );
      acc.ActivationMode = new ActivationMode( ActivationMode.Default );
      inputList.Add( acc );
      return acc;
    }

    public void DelCommand( int index )
    {
      int removeIt = -1;

      for ( int i = 0; i < inputList.Count; i++ ) {
        if ( inputList[i].NodeIndex == index ) removeIt = i;
        if ( inputList[i].NodeIndex > index ) inputList[i].NodeIndex -= 1; // reorder trailing ones
      }
      if ( removeIt >= 0 ) inputList.RemoveAt( removeIt );
    }

    
    /// <summary>
    /// Merge action is simply copying the new input control
    /// </summary>
    /// <param name="newAc"></param>
    public void Merge( ActionCls newAc )
    {
      this.inputList.Clear( );
      foreach ( ActionCommandCls acc in newAc.inputList ) {
        this.inputList.Add( acc );
      }
    }



    /// <summary>
    /// Updates an actionCommand with a new input (command)
    /// </summary>
    /// <param name="devInput">The input command</param>
    public void UpdateCommandFromInput( String devInput, int accIndex ) // ActionCommandCls actionCmd )
    {
      //log.Debug( "UpdateCommandFromInput - Entry" );
      if ( accIndex < 0 ) return;
      // Apply the input to the ActionTree
      this.inputList[accIndex].DevInput = BlendInput( devInput, this.actionDevice );
      if ( IsBlendedInput( this.inputList[accIndex].DevInput ) || string.IsNullOrEmpty(devInput) ) {
        this.inputList[accIndex].ActivationMode = new ActivationMode( ActivationMode.Default ); // reset activation mode if the input is empty
      }
    }

    /// <summary>
    /// Find an ActionCommand with input in an Action
    /// </summary>
    /// <param name="input">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    public ActionCommandCls FindActionInputObject( String devInput )
    {
      log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCommandCls acc = null;
      acc = this.inputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.DevInput == devInput; } );
      if ( acc == null ) {
        log.Error( "FindActionInputObject - Action Input not found in Action" );
        return null;  // ERROR - Action Input not found in tree
      }
      return acc;
    }


    /// <summary>
    /// Find an ActionCommand with index in an Action
    /// </summary>
    /// <param name="input">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    public ActionCommandCls FindActionInputObject( int index )
    {
      log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCommandCls acc = null;
      acc = this.inputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.NodeIndex == index; } );
      if ( acc == null ) {
        log.Error( "FindActionInputObject - Action Input not found in Action" );
        return null;  // ERROR - Action Input not found in tree
      }
      return acc;
    }


    /// <summary>
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = ""; String
      bindCmd = "rebind";
      if ( inputList.Count > 0 ) {
        if ( !String.IsNullOrEmpty( inputList[0].Input ) ) {
          r = String.Format( "\t<action name=\"{0}\">\n", name );
          foreach ( ActionCommandCls acc in inputList ) {
            if ( !String.IsNullOrEmpty( acc.Input ) ) {
              // r += String.Format( "\t\t\t<{0} device=\"{1}\" {2}", bindCmd, device, acc.toXML( ) ); // OLD style
              r += String.Format( "\t\t\t<{0} {1}", bindCmd, acc.toXML( ) ); // 20151220BM: format for AC2 style 
              bindCmd = "addbind"; // switch to addbind
            }
          }
          r += String.Format( "\t\t</action>\n" );
        }
      }

      return r;
    }

    /// <summary>
    /// Read an action from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
    {
      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      if ( reader.Name.ToLowerInvariant( ) == "action" ) {
        if ( reader.HasAttributes ) {
          name = reader["name"];
          reader.ReadStartElement( "action" ); // Checks that the current content node is an element with the given Name and advances the reader to the next node
        }
        else {
          return false;
        }
      }
      do {
        // support AC2 and AC1 i.e. without and with device attribute 
        if ( reader.Name.ToLowerInvariant( ) == "rebind" ) {
          if ( reader.HasAttributes ) {
            device = reader["device"];
            String input = reader["input"];
            if ( String.IsNullOrEmpty( input ) ) return false; // ERROR exit
            input = DeviceCls.fromXML( input ); // move from external to internal blend
            if ( String.IsNullOrEmpty( device ) ) {
              // AC2 style - derive the device (Device.DeviceClass)
              device = DeviceClassFromInput( input );
            }
            else {
              // AC1 style - need to reformat mouse and keyboard according to AC2 style now
              if ( KeyboardCls.IsDeviceClass( device ) ) input = KeyboardCls.FromAC1( input );
              else if ( MouseCls.IsDeviceClass( device ) ) input = MouseCls.FromAC1( input );
              else if ( GamepadCls.IsDeviceClass( device ) ) input = GamepadCls.FromAC1( input );
            }
            //first find an ActivationMode if there is - applies to all actions
            // this can be an Activation Mode OR a multitap
            // if there is an activationMode - copy the one from our List
            // if no ActivationMode is given, create one with multitap 1 or may be 2...
            string actModeName = reader["ActivationMode"];
            ActivationMode actMode = null;
            if ( ! string.IsNullOrEmpty( actModeName ) ) {
              actMode = ActivationModes.Instance.ActivationModeByName( actModeName ); // should be a valid ActivationMode for this action
            }
            else {
              actMode = new ActivationMode( ActivationMode.Default ); // no specific name given, use default
              string multiTap = reader["multiTap"];
              if ( !string.IsNullOrEmpty( multiTap ) ) {
                actMode.MultiTap = int.Parse(multiTap); // modify with given multiTap
              }
            }

            key = DevTag( device ) + name; // unique id of the action
            actionDevice = ADevice( device ); // get the enum of the input device

            AddCommand( input, actMode );
            // advances the reader to the next node
            reader.ReadStartElement( "rebind" );
          }
        }
        else if ( reader.Name.ToLowerInvariant( ) == "addbind" ) {
          if ( reader.HasAttributes ) {
            device = reader["device"];
            String input = reader["input"];
            if ( String.IsNullOrEmpty( input ) ) return false; // ERROR exit
            input = DeviceCls.fromXML( input ); // move from external to internal blend
            if ( String.IsNullOrEmpty( device ) ) {
              // AC2 style - derive the device (Device.DeviceClass)
              device = DeviceClassFromInput( input );
            }
            else {
              // AC1 style - need to reformat according to AC2 style now
              if ( KeyboardCls.IsDeviceClass( device ) ) input = KeyboardCls.FromAC1( input );
              else if ( MouseCls.IsDeviceClass( device ) ) input = MouseCls.FromAC1( input );
              else if ( GamepadCls.IsDeviceClass( device ) ) input = GamepadCls.FromAC1( input );
            }
            //first find an ActivationMode if there is - applies to all actions
            // this can be an Activation Mode OR a multitap
            // if there is an activationMode - copy the one from our List
            // if no ActivationMode is given, create one with multitap 1 or may be 2...
            string actModeName = reader["ActivationMode"];
            ActivationMode actMode = null;
            if ( !string.IsNullOrEmpty( actModeName ) ) {
              actMode = ActivationModes.Instance.ActivationModeByName( actModeName ); // should be a valid ActivationMode for this action
            }
            else {
              actMode = new ActivationMode( ActivationMode.Default ); // no specific name given, use default
              string multiTap = reader["multiTap"];
              if ( !string.IsNullOrEmpty( multiTap ) ) {
                actMode.MultiTap = int.Parse( multiTap ); // modify with given multiTap
              }
            }
            AddCommand( input, actMode );
            // advances the reader to the next node
            reader.ReadStartElement( "addbind" );
          }
        }
        else {
          return false;
        }
      } while ( reader.Name == "addbind" );
      return true;
    }


  }
}
