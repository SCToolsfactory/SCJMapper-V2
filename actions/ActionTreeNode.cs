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

    /// <summary>
    /// Composes the ActionText from items
    /// </summary>
    /// <param name="action">The action (profile notation)</param>
    /// <param name="cmd">The command applied</param>
    /// <param name="modified">True if it represents a modified item</param>
    /// <returns>A composed string</returns>
    public static string ComposeNodeActionText( string action, string cmd, bool modified = false )
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
    protected static void DecompNodeActionText( string nodeText, out string action, out string cmd )
    {
      action = ""; cmd = "";
      string[] e = nodeText.Split( new char[] { RegDiv, ModDiv }, StringSplitOptions.RemoveEmptyEntries );
      if ( e.Length > 1 ) {
        action = e[0].TrimEnd( );
        if ( e[1].Trim( ) == DeviceCls.DisabledInput ) {
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
    public static string ActionFromActionText( string nodeText )
    {
      ActionTreeNode.DecompNodeActionText( nodeText, out string action, out string cmd );
      return action;
    }

    /// <summary>
    /// Returns the command part from a node text
    /// i.e.  v_pitch - js1_x returns js1_x
    /// </summary>
    /// <param name="nodeText">The node text in 'action - command' notation</param>
    /// <returns>the command part or an empty string</returns>
    public static string CommandFromActionText( string nodeText )
    {
      ActionTreeNode.DecompNodeActionText( nodeText, out string action, out string cmd );
      return cmd;
    }


    #endregion


    // Object defs

    /// <summary>
    /// cTor: empty node
    /// </summary>
    public ActionTreeNode()
      : base( )
    {
    }

    /// <summary>
    /// cTor: just create a node without added functionality  
    ///    NOTE: must fill properties to work
    /// </summary>
    /// <param name="text">The Text element of the Node</param>
    public ActionTreeNode( string text )
    {
      this.Text = text;
    }

    /// <summary>
    /// cTor: just create a node without added functionality  
    ///    NOTE: must fill properties to work
    /// </summary>
    /// <param name="text">The Text element of the Node</param>
    /// <param name="children">The child list</param>
    public ActionTreeNode( string text, ActionTreeNode[] children )
      : base( text, children )
    {
    }

    /// <summary>
    /// cTor: Copy src to new node
    /// </summary>
    /// <param name="srcNode"></param>
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
      this.m_actionText = srcNode.m_actionText;

      // these are changing while using it
      this.Update( srcNode );
    }

    // our own properties
    protected string m_actionText = ""; // contains the former Text property of the TreeNode
    private string m_action = "";       // i.e.  v_pitch
    protected string m_command = "";     // i.e.  js2_button3
    protected bool m_modified = false;  // any modifier applied? (ActivationMode)
    private Act.ActionDevice m_actionDevice = Act.ActionDevice.AD_Unknown;

    /// <summary>
    /// Crates the translated node text for display
    /// </summary>
    /// <returns>A composed string</returns>
    protected string ComposeNodeText()
    {
      string tAction = SC.SCUiText.Instance.Text( m_action );

      if ( string.IsNullOrEmpty( m_command ) ) {
        return tAction;                                                            // v_eject
      }
      else if ( string.IsNullOrEmpty( tAction ) ) {
        return m_command;                                                               // js1_button1
      }
      else {
        if ( m_modified )
          return string.Format( "{0} {2} {1} {3}", tAction, m_command, RegDiv, ModDiv ); // v_eject - js1_button1 #
        else
          return string.Format( "{0} {2} {1}", tAction, m_command, RegDiv );             // v_eject - js1_button1
      }
    }

    /// <summary>
    /// Update this node from the other node
    ///  applies dynamic props only 
    /// </summary>
    /// <param name="other">The node to update from</param>
    public void Update( ActionTreeNode other )
    {
      // TreeNode props
      this.BackColor = other.BackColor;
      this.ToolTipText = other.ToolTipText;

      // own props
      this.Command = other.Command;
      this.Modified = other.Modified;
      this.Action = other.Action; //? does this fill all the rest properly?
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
        if ( this.Level == 2 ) {
          this.Action = ( this.Parent as ActionTreeNode ).Action;
        }
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
    /// Property ActionText (action in profile notation i.e. v_something) of an Action node
    /// </summary>
    public string ActionText { get => m_actionText; }

    /// <summary>
    /// Property Action of an Action node
    /// </summary>
    public string Action
    {
      get { return m_action; }
      set {
        m_action = value;
        // may need to update too
        this.ToolTipText = m_action;
        m_actionText = ActionTreeNode.ComposeNodeActionText( m_action, m_command, m_modified );
        base.Text = ComposeNodeText( );
      }
    }

    /// <summary>
    /// Property Command of an Action node
    /// </summary>
    public string Command
    {
      get { return m_command; }
      set {
        m_command = value;
        // may need to update too
        m_actionText = ActionTreeNode.ComposeNodeActionText( m_action, m_command, m_modified );
        base.Text = ComposeNodeText( );
      }
    }

    /// <summary>
    /// Property Modified of an Action node
    /// </summary>
    public bool Modified
    {
      get { return m_modified; }
      set {
        m_modified = value;
        // may need to update too
        m_actionText = ActionTreeNode.ComposeNodeActionText( m_action, m_command, m_modified );
        base.Text = ComposeNodeText( );
      }
    }

    /// <summary>
    /// Property ActionDevice of an Action node
    /// </summary>
    public Act.ActionDevice ActionDevice { get => m_actionDevice; set => m_actionDevice = value; }

    /// <summary>
    /// Return true if the content of Action - Command contains the match string
    /// </summary>
    /// <param name="match">A string to match</param>
    /// <returns>True if the node contains the match</returns>
    public bool Contains( string match )
    {
      string m = match.ToLowerInvariant( );
      if ( m_actionText.Contains( m ) || this.Text.ToLowerInvariant().Contains( m ) ) return true;
      return false;
    }

    /// <summary>
    /// Return true if the content of Action - Command contains the match string
    /// </summary>
    /// <param name="match">A string to match</param>
    /// <returns>True if the node contains the match</returns>
    public bool ContainsCtrl( string match )
    {
      string m = match.ToLowerInvariant( );
      if ( m_command.Contains( m ) ) return true;
      return false;
    }

    /// <summary>
    /// Return true if the content of Action - Command contains the match string
    /// </summary>
    /// <param name="match">A string to match</param>
    /// <returns>True if the node contains the match</returns>
    public bool ContainsAction( string match )
    {
      string m = match.ToLowerInvariant( );
      if ( m_action.Contains( m ) ) return true;
      return false;
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
