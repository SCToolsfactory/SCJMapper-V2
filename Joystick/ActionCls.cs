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
    // Static items to have this mapping in only one place

    /// <summary>
    /// Returns the Device ID i.e. the single letter to tag a device
    /// </summary>
    /// <param name="device">The device name from the CryFile</param>
    /// <returns>The single UCase device ID letter</returns>
    static public String DevID( String device )
    {
      switch ( device.ToLower( ) ) {
        case "keyboard": return "K";
        case "joystick": return "J";
        case "xboxpad": return "X";
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
        case "K": return "keyboard";
        case "J": return "joystick";
        case "X": return "xboxpad";
        case "P": return "ps3pad";
        default: return "unknown";
      }
    }


    // Class items

    public String key { get; set; }  // the key is the "Daction" formatted item (as we can have the same name multiple times)
    public String name { get; set; }
    public String device { get; set; }
    public String input { get; set; }

    /// <summary>
    /// ctor
    /// </summary>
    public ActionCls( )
    {
      device = "joystick";  // what else ??
    }

    /// <summary>
    /// Merge action is simply copying the new input control
    /// </summary>
    /// <param name="newAc"></param>
    public void Merge(ActionCls newAc)
    {
      input = newAc.input;
    }

    /// <summary>
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = "";
      if (! String.IsNullOrEmpty(input) ) r = String.Format( "\t<action name=\"{0}\">\n\t\t\t<rebind device=\"{1}\" input=\"{2}\" />\n\t\t</action>\n", name, device, input );
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
      XmlReader reader = XmlReader.Create(new StringReader( xml ), settings);

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
          key = DevID( device ) + name; // unique id of the action
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
