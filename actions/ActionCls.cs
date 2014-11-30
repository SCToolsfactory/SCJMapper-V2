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
  /// </summary>
  class ActionCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );


    public enum ActionDevice
    {
      AD_Unknown = -1,
      AD_Joystick = 0,
      AD_Gamepad,
      AD_Keyboard,
    }

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


    // Class items

    public String key { get; set; }  // the key is the "Daction" formatted item (as we can have the same name multiple times)
    public String name { get; set; }
    public String device { get; set; }
    public String input { get; set; }
    public String defBinding { get; set; }  // the default binding
    public Boolean inverted { get; set; }
    public ActionDevice actionDevice { get; set; }
    /// <summary>
    /// ctor
    /// </summary>
    public ActionCls( )
    {
      device = JoystickCls.DeviceClass;
      key = "";
      name = "";
      input = "";
      defBinding = "";
      inverted = false;
      actionDevice = ActionDevice.AD_Unknown;
    }


    /// <summary>
    /// Copy return the action while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The action copy with reassigned input</returns>
    public ActionCls ReassignJsN( Dictionary<int, int> newJsList )
    {
      ActionCls newAc = new ActionCls( );
      // full copy from 'this'
      newAc.key = this.key;
      newAc.name = this.name;
      newAc.device = this.device;
      newAc.defBinding = this.defBinding;
      newAc.input = this.input;
      newAc.inverted = this.inverted;

      // reassign the jsX part for Joystick commands
      if ( JoystickCls.IsDeviceClass( this.device ) && JoystickCls.IsDeviceClass( newAc.device ) ) {
        int oldJsN = JoystickCls.JSNum( this.input );
        if ( JoystickCls.IsJSValid( oldJsN ) ) {
          if ( newJsList.ContainsKey( oldJsN ) ) newAc.input = JoystickCls.ReassignJSTag( this.input, newJsList[oldJsN] );
        }
      }

      return newAc;
    }



    /// <summary>
    /// Merge action is simply copying the new input control
    /// </summary>
    /// <param name="newAc"></param>
    public void Merge( ActionCls newAc )
    {
      input = newAc.input;
      inverted = newAc.inverted;
    }

    /// <summary>
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = "";
      if ( !String.IsNullOrEmpty( input ) ) {
        if ( inverted ) r = String.Format( "\t<action name=\"{0}\">\n\t\t\t<rebind device=\"{1}\" input=\"{2}\" invert=\"1\" />\n\t\t</action>\n", name, device, input );
        else r = String.Format( "\t<action name=\"{0}\">\n\t\t\t<rebind device=\"{1}\" input=\"{2}\" />\n\t\t</action>\n", name, device, input );
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
          // Move the reader back to the element node.
          reader.ReadStartElement( "action" );
        }
        else {
          return false;
        }
      }
      if ( reader.Name == "rebind" ) {
        if ( reader.HasAttributes ) {
          device = reader["device"];
          input = reader["input"];
          if ( ( input == JoystickCls.BlendedInput ) || ( input == GamepadCls.BlendedInput ) ) input = ""; // don't carry jsx_reserved or xi_reserved into the action
          key = DevID( device ) + name; // unique id of the action
          actionDevice = ADevice( device ); // get the enum of the input device
          String inv = reader["invert"];
          if ( String.IsNullOrWhiteSpace( inv ) ) {
            inverted = false;
          }
          else {
            inverted = ( inv == "1" ) ? true : false;
          }
          // Move the reader back to the element node.
          reader.ReadStartElement( "rebind" );
        }
      }
      else {
        return false;
      }
      return true;
    }


  }
}
