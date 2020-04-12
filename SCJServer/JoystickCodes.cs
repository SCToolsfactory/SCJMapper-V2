using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.SCJServer
{
  /// <summary>
  /// Translates Joystick stuff
  /// </summary>
  class JoystickCodes
  {
    /*
	    Joystick:
	    Axis:     { "A": {"Direction": "X|Y|Z", "Value": number, "JNo": j, "Ext1": "extstr", "Ext2": "extstr", "Ext3": "extstr"} }
				    - number => 0..1000 (normalized)
    
	    RotAxis:  { "R": {"Direction": "X|Y|Z", "Value": number, "JNo": j, "Ext1": "extstr", "Ext2": "extstr", "Ext3": "extstr"} }
				    - number => 0..1000 (normalized)
    
	    Slider:   { "S": {"Index": 1|2, "Value": number, "JNo": j, "Ext1": "extstr", "Ext2": "extstr", "Ext3": "extstr"} }
				    - number => 0..1000 (normalized)
    
	    POV:      { "P": {"Index": 1|2|3|4, "Direction": "c | u | r | d | l", "JNo": j, "Ext1": "extstr", "Ext2": "extstr", "Ext3": "extstr" } }   
				    - Index n=> 1..MaxPOV (setup of vJoy, max = 4)
				    - Direction either of the chars (center (released), up, right, donw, left)

	    Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay": msec, "JNo": j, "Ext1": "extstr", "Ext2": "extstr", "Ext3": "extstr" } } 
				    - Button Index n => 1..VJ_MAXBUTTON (setup of vJoy - SC supports 60)
				    - Mode optional - either of the chars (see below)

		    - Mode:     [mode]      (p)ress, (r)elease, (t)ap, (s)hort tap, (d)ouble tap           (default=tap - short tap is a tap with almost no delay)
		    - Delay:    [delay]      nnnn  milliseconds, optional for Tap and Double Tap           (default=150 VJCommand.DEFAULT_DELAY, max 20_000 msec)     
		    - JNo:      [joyNumber] >0 Joystick number                                             (default=1)
     
     */


    public static string FromJsAction( string jsAction, int jsN )
    {
      /*
        button38
        y
        rotz
        slider1
        hat1_down
      */
      if ( jsAction.StartsWith( "button" ) ) {
        if ( int.TryParse( jsAction.Replace( "button", "" ), out int bNum ) ) {
          return $"{{ \"B\":{{ \"Index\": {bNum}, \"Mode\": \"t\", \"JNo\": {jsN} }} }}";
        }
        else return "";
      }
      if ( jsAction.StartsWith( "rot" ) ) {
        string dir = jsAction.Replace( "rot", "" );
        return $"{{ \"R\":{{ \"Direction\": {dir.ToUpperInvariant( )}, \"Value\": NNN, \"JNo\": {jsN} }} }}";
      }
      if ( jsAction.StartsWith( "slider" ) ) {
        if ( int.TryParse( jsAction.Replace( "slider", "" ), out int bNum ) ) {
          return $"{{ \"S\":{{ \"Index\": {bNum}, \"Value\": NNN, \"JNo\": {jsN} }} }}";
        }
        else return "";
      }
      if ( jsAction.StartsWith( "hat" ) ) {
        string[] e = jsAction.Split( new char[] { '_' } );
        if ( int.TryParse( e[0].Replace( "hat", "" ), out int bNum ) ) {
          return $"{{ \"P\":{{ \"Index\": {bNum}, \"Direction\": \"{e[1].Substring( 0, 1 )}\", \"JNo\": {jsN} }} }}\n"
               + $"  {{ \"P\":{{ \"Index\": {bNum}, \"Direction\": \"c\", \"JNo\": {jsN} }} }}"; // add the center one as well
        }
        else return "";
      }
      // remains Axis x,y,z
      if ( jsAction == "x" || jsAction == "y" || jsAction == "z" ) {
        return $"{{ \"A\":{{ \"Direction\": {jsAction.ToUpperInvariant( )}, \"Value\": NNN, \"JNo\": {jsN} }} }}";
      }
      return "";
    }

  }
}
