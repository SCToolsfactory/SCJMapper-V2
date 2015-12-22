using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2
{
  /// <summary>
  /// Maintains one ActionCommand 
  /// AC2 style input is used i.e. with device tag in front
  /// </summary>
  public class ActionCommandCls
  {




    public String input { get; set; }               // AC2 style: input command name AC2  e.g. js1_x, mo1_button1, kb1_r, "kb1_ " to blend

    /// <summary>
    /// The index of the visible child node (-1 -> shown in ActionNode)
    /// </summary>
    public int nodeIndex { get; set; }              // index of the vis treenode


    // ctor
    public ActionCommandCls( )
    {
      input = "UNDEF";
      nodeIndex = -1;
    }


    /// <summary>
    /// Copy return the action while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The action copy with reassigned input</returns>
    public ActionCommandCls ReassignJsN( JsReassingList newJsList )
    {
      ActionCommandCls newAc = new ActionCommandCls( );
      // full copy from 'this'
      newAc.input = this.input;

      // reassign the jsX part for Joystick commands  (silly but rather fast comparison)
      if ( this.input.Contains( "js1_" ) || this.input.Contains( "js2_" ) || this.input.Contains( "js3_" ) || this.input.Contains( "js4_" )
        || this.input.Contains( "js5_" ) || this.input.Contains( "js6_" ) || this.input.Contains( "js7_" ) || this.input.Contains( "js8_" ) ) {
        int oldJsN = JoystickCls.JSNum( this.input );
        if ( JoystickCls.IsJSValid( oldJsN ) ) {
          if ( newJsList.ContainsOldJs( oldJsN ) ) newAc.input = JoystickCls.ReassignJSTag( this.input, newJsList.newJsFromOldJs(oldJsN) );
        }
      }

      return newAc;
    }


    /// <summary>
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( ActionCls.ActionDevice aDev )
    {
      String r = "";
      if ( String.IsNullOrEmpty( input ) ) {
        r = String.Format( " />\n" );  // actually an ERROR...
      }
      else {
        // regular - first get device dependend blending if needed then apply XML formatting to internally blended items
        r = String.Format( "input=\"{0}\" />\n", DeviceCls.toXML( ActionCls.BlendInput( input, aDev ) ) );
      }

      return r;
    }

  }
}
