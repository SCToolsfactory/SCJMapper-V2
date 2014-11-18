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

    public ActionTreeNode( )
      : base( )
    {
    }

    public ActionTreeNode( ActionTreeNode srcNode )
      : base( )
    {
      if ( srcNode == null ) return;
      this.Name = srcNode.Name;
      this.Text = srcNode.Text;
      this.BackColor = srcNode.BackColor;
      this.ForeColor = srcNode.ForeColor;
      this.NodeFont = srcNode.NodeFont;
      this.ImageKey = srcNode.ImageKey;
      this.Tag = srcNode.Tag;
      this.m_action = srcNode.m_action;
      this.m_command = srcNode.m_command;
    }

    public ActionTreeNode( string text )
      : base( text )
    {
    }

    public ActionTreeNode( string text, ActionTreeNode[] children )
      : base( text, children )
    {
    }


    /// <summary>
    /// Instantiates a copy of the node - copies only the needed properties
    /// </summary>
    /// <param name="srcNode">A source node</param>
    /// <returns>A new TreeNode</returns>
    private ActionTreeNode TNCopy( ActionTreeNode srcNode )
    {
      if ( srcNode == null ) return null;

      ActionTreeNode nn = new ActionTreeNode( );
      nn.Name = srcNode.Name;
      nn.Text = srcNode.Text;
      nn.BackColor = srcNode.BackColor;
      nn.ForeColor = srcNode.ForeColor;
      nn.NodeFont = srcNode.NodeFont;
      nn.ImageKey = srcNode.ImageKey;
      return nn;
    }


    private String m_action = "";
    private String m_command ="";

    public String Action
    {
      get { return m_action; }
      set
      {
        m_action = value;
        this.Text = ComposeNodeText( m_action, m_command );
      }
    }

    public String Command
    {
      get { return m_command; }
      set
      {
        m_command = value;
        this.Text = ComposeNodeText( m_action, m_command );
      }
    }


    // Handle all text label composition and extraction here

    private static String ComposeNodeText( String action, String cmd )
    {
      if ( String.IsNullOrEmpty( cmd ) ) {
        return action;
      }
      else {
        return action + " - " + cmd;
      }
    }


    private static void DecompNodeText( String nodeText, out String action, out String cmd )
    {
      action = ""; cmd = "";
      String[] e = nodeText.Split( new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries );
      if ( e.Length > 1 ) {
        action = e[0].Trim( );
        cmd = e[1].Trim( );
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
      DecompNodeText( nodeText, out action, out cmd );
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
      DecompNodeText( nodeText, out action, out cmd );
      return cmd;
    }

  }
}
