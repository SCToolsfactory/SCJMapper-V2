using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SCJMapper_V2.Common;
using SCJMapper_V2.Devices;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Our TreeNode - inherits a regular TreeNode one and adds some functionality
  /// </summary>
  class ActionTreeNode : TreeNode
  {

    #region Static items

    public const char RegDiv = '-'; // action, cmd separator tag
    public const char ModDiv = '#'; // modified tag
    public const string AddDiv = "$"; // addbind tag

    // Handle all text label composition and extraction here

    public static string ComposeNodeText( string action, string cmd, bool modified = false )
    {
      if ( string.IsNullOrEmpty( cmd ) ) {
        return action;                                                            // v_eject
      }
      else if ( string.IsNullOrEmpty( action ) ) {
        return cmd;                                                               // js1_button1
      }
      else {
        if ( modified )
          return string.Format( "{0} {2} {1} {3}", action, cmd, RegDiv, ModDiv ); // v_eject - js1_button1 #
        else
          return string.Format( "{0} {2} {1}", action, cmd, RegDiv );             // v_eject - js1_button1
      }
    }

    /// <summary>
    /// Decompose from node.Text to the individual parts
    /// e.g.  v_eject - js1_button1 # =>  v_eject, js1_button1
    /// </summary>
    /// <param name="nodeText">The node Text</param>
    /// <param name="action">The action</param>
    /// <param name="cmd">The device command</param>
    public static void DecompNodeText( string nodeText, out string action, out string cmd )
    {
      action = ""; cmd = "";
      string[] e = nodeText.Split( new char[] { RegDiv, ModDiv }, StringSplitOptions.RemoveEmptyEntries );
      if ( e.Length > 1 ) {
        action = e[0].TrimEnd( );
        if ( e[1].Trim() == DeviceCls.DisabledInput ) {
          cmd = e[1];
        }
        else {
          cmd = e[1].Trim( );
        }
      }
      else if ( e.Length > 0 ) {
        // action part only - i.e. not bound node
        action = e[0].TrimEnd( );
        cmd = "";
        // consider if the single item is not an action but a command (from ActionTreeInputNode)
        // it is then starting with the tag $ (that must be removed)
        if ( action.StartsWith( AddDiv ) ) {
          cmd = action.Substring( 1 );
          action = "";
        }

      }
    }


    /// <summary>
    /// Returns the action part from a node text
    /// i.e.  v_pitch - js1_x returns v_pitch
    /// </summary>
    /// <param name="nodeText">The node text in 'action - command' notation</param>
    /// <returns>the action part or an empty string</returns>
    public static string ActionFromNodeText( string nodeText )
    {
      ActionTreeNode.DecompNodeText( nodeText, out string action, out string cmd );
      return action;
    }

    /// <summary>
    /// Returns the command part from a node text
    /// i.e.  v_pitch - js1_x returns js1_x
    /// </summary>
    /// <param name="nodeText">The node text in 'action - command' notation</param>
    /// <returns>the command part or an empty string</returns>
    public static string CommandFromNodeText( string nodeText )
    {
      ActionTreeNode.DecompNodeText( nodeText, out string action, out string cmd );
      return cmd;
    }

    #endregion


    // Object defs

    // ctor
    public ActionTreeNode( )
      : base( )
    {
    }

    // ctor
    public ActionTreeNode( ActionTreeNode srcNode )
      : base( )
    {
      if ( srcNode == null ) return;
      // properties set once for a node
      this.Name = srcNode.Name;
      this.Text = srcNode.Text;
      this.ForeColor = srcNode.ForeColor;
      this.NodeFont = srcNode.NodeFont;
      this.ImageKey = srcNode.ImageKey;
      this.Tag = srcNode.Tag;
      this.m_action = srcNode.m_action;
      this.m_actionDevice = srcNode.m_actionDevice;

      // these are changing while using it
      this.Update( srcNode );
    }

    // ctor
    public ActionTreeNode( string text )
    {
      this.Text = text;
    }

    // ctor
    public ActionTreeNode( string text, ActionTreeNode[] children )
      : base( text, children )
    {
    }

    // our own properties
    private string m_action = "";
    protected string m_command ="";
    protected bool m_modified = false; // any modifier applied? (ActivationMode)
    private Act.ActionDevice m_actionDevice = Act.ActionDevice.AD_Unknown;


    /// <summary>
    /// Update this node from the other node
    ///  applies dynamic props only 
    /// </summary>
    /// <param name="other">The node to update from</param>
    public void Update( ActionTreeNode other )
    {
      this.BackColor = other.BackColor;
      this.Command = other.Command;
      this.Modified = other.Modified;
      this.Action = other.Action; //?????????????????
    }

    /// <summary>
    /// Apply an update from the action to the treenode
    /// First apply to the GUI tree where the selection happend then copy it over to master tree
    /// </summary>
    /// <param name="action">The action that carries the update</param>
    public void UpdateAction( ActionCommandCls actionCmd )
    {
      //log.Debug( "UpdateNodeFromAction - Entry" );
      if ( actionCmd == null ) return;

      // input is either "" or a valid mapping or a blended mapping
      if ( string.IsNullOrEmpty( actionCmd.Input ) ) {
        // new unmapped
        this.Command = ""; this.BackColor = MyColors.UnassignedColor;
        if ( this.Level == 2 ) this.Action = "UNDEF"; // apply UNDEF - 20160525 fix addbind not showing UNDEF if assigned
      }
      // blended mapped ones - can only get a Blend Background
      else if ( actionCmd.Input == DeviceCls.DisabledInput ) {
        this.Command = actionCmd.DevInput; this.BackColor = MyColors.BlendedColor;
      }
      else {
        // mapped ( regular ones )
        this.Command = actionCmd.DevInput;
        // background is along the input 
        this.BackColor = Act.DeviceColor( actionCmd.DevInput );
      }
      this.Modified = !actionCmd.DefaultActivationMode; // apply modifier visual
    }



    /// <summary>
    /// Property Text of an Action node
    /// </summary>
    public new string Text
    {
      get { return base.Text; }
      set
      {
        ActionTreeNode.DecompNodeText( value, out m_action, out m_command );
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }


    /// <summary>
    /// Property Action of an Action node
    /// </summary>
    public string Action
    {
      get { return m_action; }
      set
      {
        m_action = value;
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }

    /// <summary>
    /// Property Command of an Action node
    /// </summary>
    public string Command
    {
      get { return m_command; }
      set
      {
        m_command = value;
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }

    /// <summary>
    /// Property ActionDevice of an Action node
    /// </summary>
    public Act.ActionDevice ActionDevice
    {
      get { return m_actionDevice; }
      set
      {
        m_actionDevice = value;
      }
    }

    /// <summary>
    /// Property Modified of an Action node
    /// </summary>
    public bool Modified
    {
      get { return m_modified; }
      set
      {
        m_modified = value;
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }


    /// <summary>
    /// Property IsJoystickAction of an Action node
    /// </summary>
    public bool IsJoystickAction
    {
      get { return ( m_actionDevice == Act.ActionDevice.AD_Joystick ); }
    }

    /// <summary>
    /// Property IsGamepadAction of an Action node
    /// </summary>
    public bool IsGamepadAction
    {
      get { return ( m_actionDevice == Act.ActionDevice.AD_Gamepad ); }
    }

    /// <summary>
    /// Property IsKeyboardAction of an Action node
    /// </summary>
    public bool IsKeyboardAction
    {
      get { return ( m_actionDevice == Act.ActionDevice.AD_Keyboard ); }
    }

    /// <summary>
    /// Property IsMouseAction of an Action node
    /// </summary>
    public bool IsMouseAction  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
    {
      get { return ( m_actionDevice == Act.ActionDevice.AD_Mouse ); }
    }

    /// <summary>
    /// Returns true if the action is mapped
    /// </summary>
    public bool IsMappedAction
    {
      get {
        return !( string.IsNullOrEmpty( m_command ) || Act.IsDisabledInput( m_command ) );
      }
    }

    /// <summary>
    /// Returns true if the action is disabled
    /// </summary>
    public bool IsDisabledAction
    {
      get {
        return Act.IsDisabledInput( m_command );
      }
    }


  }
}
