using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCJMapper_V2.Actions;

namespace SCJMapper_V2.SCJServer
{
  /// <summary>
  /// Create SCJoyServer Commands from the selected item
  /// </summary>
  class SCJScmd
  {
    /*
     Should be something like:

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

	Keyboard:
		Key:      { "K": {"VKcodeEx": "keyName", "VKcode": n, "Mode": "p|r|t|s|d", "Modifier": "mod", "Delay": msec, "Ext1": "extstr", "Ext2": "extstr", "Ext3": "extstr" } }  
					- VKcodeEx "s" either a number n=> 1..255 or a WinUser VK_.. literal (see separate Reference file)
					- VKcode n=> 1..255 WinUser VK_.. (see separate Reference file)
						if both are found the VKcodeEx item gets priority and the VKcode element is ignored
						if none is found the command is ignored
					- Mode optional - either of the chars (see below)
					- Modifier optional - a set of codes (see below)

		- Mode:     [mode]      (p)ress, (r)elease, (t)ap, (s)hort tap, (d)ouble tap           (default=tap - short tap is a tap with almost no delay)
		- Modifier: [mod[&mod]] (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt, (ls)hift, (rs)hift   (default=none - concat modifiers with & char)
		- Delay:    [delay]      nnnn  milliseconds, optional for Tap and Double Tap           (default=150 VJCommand.DEFAULT_DELAY, max 20_000 msec)     
		- JNo:      [joyNumber] >0 Joystick number                                             (default=1)
		- Ext1.3:   [string]    Optional user extensions that must be handled by the caller

      e.g.
      { "K": {"VKcodeEx": "VK_B", "Mode": "t", "Modifier": "lc"}}
      { "B": {"Index": 21 }}
     */


    private static string GetKeyCommand( ActionTreeNode node )
    {
      string fullPath = node.FullPath;
      string action = node.Action;
      string command = node.Command.Replace("kb1_",""); // needs some fiddling..

      // kb1_ralt+l
      string[] e = command.Split( new char[] { '+' } );
      // a number of modifiers and the key char
      string key = KbdCodes.FromSCKeyboardCmd( e[e.Length-1] );
      string mod = "";
      for ( int i= 0; i<e.Length-1; i++ ) {
        mod += KbdCodes.FromSCKeyboardMod( e[i] );
      }
      if ( !string.IsNullOrEmpty( mod ) ) mod = mod.Substring( 0, mod.Length - 1 ); // kill last &

      string cmd = $"{fullPath}\"\n  {{ \"K\": {{ \"VKCodeEx\": \"{key}\", \"Mode\": \"t\", \"Modifier\": \"{mod}\" }}}}\n";
      return cmd;
    }

    private static string GetJoyCommand( ActionTreeNode node )
    {
      string fullPath = node.FullPath;
      string command = node.Command;
      string action = Devices.Joystick.JoystickCls.ActionFromJsCommand(command);
      int jsN = Devices.Joystick.JoystickCls.JSNum( command );

      string cmd = "";
      string jscmd = JoystickCodes.FromJsAction( action, jsN );
      if ( !string.IsNullOrEmpty( jscmd ) ) {
        cmd = $"{fullPath}\"\n  {jscmd}\n";
      }
      return cmd;
    }

    /// <summary>
    /// Create a command for SCJoyServer use
    /// </summary>
    /// <param name="node">The clicked node</param>
    /// <returns>A string</returns>
    public static string GetCommand( TreeNode node )
    {
      if ( node is ActionTreeNode ) {
        var aNode = node as ActionTreeNode;
        if ( !aNode.IsMappedAction ) return "";
        if ( aNode.IsDisabledAction ) return "";

        if ( aNode.IsKeyboardAction ) {
          return GetKeyCommand( aNode );
        }else if ( aNode.IsJoystickAction ) {
          return GetJoyCommand( aNode );
        }
        else {
          return "";
        }
      }
      return "";
    }

  }
}
