using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

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
  /// </summary>
  public class ActionCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );


    public enum ActionDevice
    {
      AD_Unknown = -1,
      AD_Joystick = 0,
      AD_Gamepad,
      AD_Keyboard,
    }

    #region Static Items

    static public ActionDevice ADevice( String device )
    {
      switch ( device.ToLower( ) ) {
        case KeyboardCls.DeviceClass: return ActionDevice.AD_Keyboard;
        case JoystickCls.DeviceClass: return ActionDevice.AD_Joystick;
        case GamepadCls.DeviceClass: return ActionDevice.AD_Gamepad;
        case "ps3pad": return ActionDevice.AD_Gamepad;
        default: return ActionDevice.AD_Unknown;
      }
    }

    // Static items to have this mapping in only one place

    /// <summary>
    /// Returns the Device ID i.e. the single letter to tag a device
    /// </summary>
    /// <param name="device">The device name from the CryFile</param>
    /// <returns>The single UCase device ID letter</returns>
    static public String DevID( String device )
    {
      switch ( device.ToLower( ) ) {
        case KeyboardCls.DeviceClass: return "K";
        case JoystickCls.DeviceClass: return "J";
        case GamepadCls.DeviceClass: return "X";
        case "ps3pad": return "P";
        default: return "Z";
      }
    }

    /// <summary>
    /// Returns the Device name from the ID
    /// </summary>
    /// <param name="device">The single UCase device ID letter</param>
    /// <returns>The device name from the CryFile</returns>
    static public String DeviceFromID( String devID )
    {
      switch ( devID ) {
        case "K": return KeyboardCls.DeviceClass;
        case "J": return JoystickCls.DeviceClass;
        case "X": return GamepadCls.DeviceClass;
        case "P": return "ps3pad";
        default: return "unknown";
      }
    }

    #endregion


    // Class items

    public String key { get; set; }  // the key is the "Daction" formatted item (as we can have the same name multiple times)
    public String name { get; set; }                // the plain action name e.g. v_yaw
    public ActionDevice actionDevice { get; set; }  // the enum of the device
    public String device { get; set; }              // name of the device (uses DeviceClass)
    public String defBinding { get; set; }          // the default binding
    public List<ActionCommandCls> inputList { get; set; }

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

      foreach ( ActionCommandCls acc in inputList ) {
        newAc.inputList.Add( acc.ReassignJsN( newJsList ) );
      }

      return newAc;
    }


    public ActionCommandCls AddCommand( String input, int index )
    {
      ActionCommandCls acc = new ActionCommandCls( );
      acc.input = input; acc.nodeIndex = index;
      inputList.Add( acc );
      return acc;
    }

    public void DelCommand( int index )
    {
      int removeIt = -1;

      for ( int i = 0; i < inputList.Count; i++ ) {
        if ( inputList[i].nodeIndex == index ) removeIt = i;
        if ( inputList[i].nodeIndex > index ) inputList[i].nodeIndex -= 1; // reorder trailing ones
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
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = ""; String 
      bindCmd = "rebind";
      if ( inputList.Count > 0 ) {
        if ( !String.IsNullOrEmpty( inputList[0].input ) ) {
          r = String.Format( "\t<action name=\"{0}\">\n", name );
          foreach ( ActionCommandCls acc in inputList ) {
            if ( !String.IsNullOrEmpty( acc.input ) ) {
              r += String.Format( "\t\t\t<{0} device=\"{1}\" {2}", bindCmd, device, acc.toXML( ) );
              bindCmd = "addbind";
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

      if ( reader.Name == "action" ) {
        if ( reader.HasAttributes ) {
          name = reader["name"];
          reader.ReadStartElement( "action" ); // Checks that the current content node is an element with the given Name and advances the reader to the next node
        }
        else {
          return false;
        }
      }
      do {
        if ( reader.Name == "rebind" ) {
          if ( reader.HasAttributes ) {
            device = reader["device"];
            ActionCommandCls acc = new ActionCommandCls( );
            acc.input = reader["input"];
            if ( ( acc.input == JoystickCls.BlendedInput ) || ( acc.input == GamepadCls.BlendedInput ) ) acc.input = ""; // don't carry jsx_reserved or xi_reserved into the action
            key = DevID( device ) + name; // unique id of the action
            actionDevice = ADevice( device ); // get the enum of the input device
            inputList.Add( acc );
            // advances the reader to the next node
            reader.ReadStartElement( "rebind" );
          }
        }
        else if ( reader.Name == "addbind" ) {
          if ( reader.HasAttributes ) {
            device = reader["device"];
            ActionCommandCls acc = new ActionCommandCls( );
            acc.input = reader["input"];
            if ( ( acc.input == JoystickCls.BlendedInput ) || ( acc.input == GamepadCls.BlendedInput ) ) acc.input = ""; // don't carry jsx_reserved or xi_reserved into the action
            key = DevID( device ) + name; // unique id of the action
            actionDevice = ADevice( device ); // get the enum of the input device
            inputList.Add( acc );
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
