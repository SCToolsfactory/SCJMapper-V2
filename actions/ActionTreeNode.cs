using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  /// <summary>
  /// Our TreeNode - inherits a regular one and adds some functionality
  /// </summary>
  class ActionTreeNode : TreeNode
  {

    #region Static items

    public const char RegDiv = '-';
    public const char ModDiv = '#';

    // Handle all text label composition and extraction here

    public static String ComposeNodeText( String action, String cmd, Boolean modified = false )
    {
      if ( String.IsNullOrEmpty( cmd ) ) {
        return action;                                                            // v_eject
      }
      else if ( String.IsNullOrEmpty( action ) ) {
        return cmd;                                                               // js1_button1
      }
      else {
        if ( modified )
          return string.Format( "{0} {2} {1} {3}", action, cmd, RegDiv, ModDiv ); // v_eject - js1_button1 #
        else
          return string.Format( "{0} {2} {1}", action, cmd, RegDiv );             // v_eject - js1_button1
      }
    }


    public static void DecompNodeText( String nodeText, out String action, out String cmd )
    {
      action = ""; cmd = "";
      String[] e = nodeText.Split( new char[] { RegDiv, ModDiv }, StringSplitOptions.RemoveEmptyEntries );
      if ( e.Length > 1 ) {
        action = e[0].TrimEnd( );
        if ( e[1].Trim() == DeviceCls.BlendedInput ) {
          cmd = e[1];
        }
        else {
          cmd = e[1].Trim( );
        }
      }
      else if ( e.Length > 0 ) {
        action = e[0].TrimEnd( );
        cmd = "";
        // consider if the single item is not an action but a command (from ActionTreeInputNode)
        // it is then starting with the tag $ (that must be removed)
        if ( action.StartsWith( "$" ) ) {
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
    public static String ActionFromNodeText( String nodeText )
    {
      String action, cmd;
      ActionTreeNode.DecompNodeText( nodeText, out action, out cmd );
      return action;
    }

    /// <summary>
    /// Returns the command part from a node text
    /// i.e.  v_pitch - js1_x returns js1_x
    /// </summary>
    /// <param name="nodeText">The node text in 'action - command' notation</param>
    /// <returns>the command part or an empty string</returns>
    public static String CommandFromNodeText( String nodeText )
    {
      String action, cmd;
      ActionTreeNode.DecompNodeText( nodeText, out action, out cmd );
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
    private String m_action = "";
    protected String m_command ="";
    protected bool m_modified = false; // any modifier applied? (ActivationMode)
    private ActionCls.ActionDevice m_actionDevice = ActionCls.ActionDevice.AD_Unknown;


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
    }



    public new String Text
    {
      get { return base.Text; }
      set
      {
        ActionTreeNode.DecompNodeText( value, out m_action, out m_command );
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }


    public String Action
    {
      get { return m_action; }
      set
      {
        m_action = value;
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }

    public String Command
    {
      get { return m_command; }
      set
      {
        m_command = value;
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }

    public ActionCls.ActionDevice ActionDevice
    {
      get { return m_actionDevice; }
      set
      {
        m_actionDevice = value;
      }
    }

    public Boolean Modified
    {
      get { return m_modified; }
      set
      {
        m_modified = value;
        base.Text = ActionTreeNode.ComposeNodeText( m_action, m_command, m_modified );
      }
    }


    public Boolean IsJoystickAction
    {
      get { return ( m_actionDevice == ActionCls.ActionDevice.AD_Joystick ); }
    }

    public Boolean IsGamepadAction
    {
      get { return ( m_actionDevice == ActionCls.ActionDevice.AD_Gamepad ); }
    }

    public Boolean IsKeyboardAction
    {
      get { return ( m_actionDevice == ActionCls.ActionDevice.AD_Keyboard ); }
    }

    public Boolean IsMouseAction  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
    {
      get { return ( m_actionDevice == ActionCls.ActionDevice.AD_Mouse ); }
    }

    public Boolean IsMappedAction
    {
      get {
        return !( string.IsNullOrEmpty( m_command ) || ActionCls.IsBlendedInput( m_command ) );
      }
    }


  }
}
