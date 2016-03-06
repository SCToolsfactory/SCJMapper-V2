using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2
{
  /// <summary>
  /// Maintains one ActionCommand 
  /// AC2 style input is used i.e. with device tag in front
  /// commands are a built from as devID_input where ..
  ///   devID: jsN, mo1, xi1, kb1, thumbl_down and modified ones: ralt+button1 (modifier+deviceinput)
  ///   input:  x, mouse1, r,  and ~ as internal blend (defined in DeviceCls)
  /// </summary>
  public class ActionCommandCls
  {

    /// <summary>
    /// The Input commands used incl. modifiers (mod+command)
    /// </summary>
    public String Input { get; set; }     // AC2 style: input command name AC2  e.g. x, mouse1, r, "~" to blend
    /// <summary>
    /// The device ID of the device (jsN, mo1, kb1, xi1)
    /// </summary>
    public String DevID { get; set; }     // the device ID (jsN, mo1, xi1, kb1) 

    /// <summary>
    /// The applied ActivationMode for this command
    /// </summary>
    public ActivationMode ActivationMode { get; set; }  // "" or one of the defined ActivationModes

    /// <summary>
    /// Returns true if default ActivationMode is set
    /// </summary>
    public Boolean DefaultActivationMode { get { return ActivationMode == ActivationMode.Default;  } }

      /// <summary>
    /// The complete input string  (devID_input)
    /// Assuming internally blended ones only (i.e. no space blends contained)
    /// Can derive if a device tag is given
    /// </summary>
    public String DevInput
    {
      get
      {
        if ( string.IsNullOrEmpty( Input ) )
          return Input; // no Input - return empty
        else if ( string.IsNullOrEmpty( DevID ) )
          return Input; // no devID - return input only
        else
          return String.Format( "{0}_{1}", DevID, Input ); // fully qualified only if both exist
      }
      set
      {
        // decompose the deviceInput into parts
        if ( string.IsNullOrEmpty( value ) ) {  // no Input - insert input empty
          Input = ""; // empty one
        }
        else if ( value.IndexOf( "_" ) == 3 ) { // fully qualified only if both exist single digit number
          DevID = value.Substring( 0, 3 );
          Input = value.Substring( 4 );
        }
        else if ( value.IndexOf( "_" ) == 4 ) { // fully qualified only if both exist 2 digit number
          DevID = value.Substring( 0, 4 );
          Input = value.Substring( 5 );
        }
        else {  // no device - insert input empty
          // treat as input only
          Input = value;
        }
      }
    }


    /// <summary>
    /// The index of the visible child node (-1 -> shown in ActionNode)
    /// </summary>
    public int NodeIndex { get; set; }              // index of the vis treenode


    // ctor
    public ActionCommandCls( )
    {
      // init with something to debug if needed
      Input = "UNDEF";
      DevID = "NA0";
      ActivationMode = new ActivationMode( ActivationMode.Default );
      NodeIndex = -1;
    }

    // ctor
    public ActionCommandCls( ActionCommandCls other )
    {
      Input = other.Input;
      DevID = other.DevID;
      ActivationMode = new ActivationMode( other.ActivationMode );
      NodeIndex = other.NodeIndex;
    }

    // ctor
    public ActionCommandCls( String devInp )
    {
      DevInput = devInp;
      ActivationMode = new ActivationMode( ActivationMode.Default );
      NodeIndex = -1;
    }

    // ctor
    public ActionCommandCls( String devInp, int nodeIx )
    {
      DevInput = devInp;
      ActivationMode = new ActivationMode( ActivationMode.Default );
      NodeIndex = nodeIx;
    }

    // ctor
    public ActionCommandCls( String dev, String inp )
    {
      Input = inp;
      DevID = dev;
      ActivationMode = new ActivationMode( ActivationMode.Default );
      NodeIndex = -1;
    }

    // ctor
    public ActionCommandCls( String dev, String inp, int nodeIx )
    {
      Input = inp;
      DevID = dev;
      ActivationMode = new ActivationMode( ActivationMode.Default );
      NodeIndex = nodeIx;
    }


    /// <summary>
    /// Copy return the action while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The action copy with reassigned input</returns>
    public ActionCommandCls ReassignJsN( JsReassingList newJsList )
    {
      // full copy from 'this'
      ActionCommandCls newAc = new ActionCommandCls( this );

      // reassign the jsX part for Joystick commands
      if ( this.DevID.StartsWith( "js" ) ) {
        int oldJsN = JoystickCls.JSNum( this.DevID );
        if ( JoystickCls.IsJSValid( oldJsN ) ) {
          if ( newJsList.ContainsOldJs( oldJsN ) ) newAc.DevID = JoystickCls.ReassignJSTag( this.DevID, newJsList.newJsFromOldJs( oldJsN ) );
        }
      }

      return newAc;
    }

    /// <summary>
    /// Strange behavior of SC - needs a proper multitap to accept ActivationModes
    /// </summary>
    /// <param name="actMode"></param>
    /// <returns></returns>
    private String MutitapFudge(  ActivationMode actMode )
    {
      if ( actMode.IsDoubleTap ) {
        return "multiTap = \"2\"";
      }
      else {
        return "multiTap = \"1\"";
      }
    }

    /// <summary>
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = "";
      if ( !String.IsNullOrEmpty( Input ) ) {
        // regular - apply XML formatting to internally blended items
        r += String.Format( "input=\"{0}_{1}\" {2} ", DevID, DeviceCls.toXML( Input ), DeviceCls.toXMLBlendExtension(Input) ); // add multitap override if needed
        if ( ! ActivationMode.Equals( ActivationMode.Default ) ) {
          r += String.Format( "ActivationMode=\"{0}\" {1} ", ActivationMode.Name, MutitapFudge(ActivationMode) );
        }
      }
      r += String.Format( " />\n" );

      return r;
    }

  }
}
