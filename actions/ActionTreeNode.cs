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

    public const char REG_MOD = '-';
    public const char INV_MOD = '!';


    // Handle all text label composition and extraction here

    public static String ComposeNodeText( String action, char mod, String cmd )
    {
      if ( String.IsNullOrEmpty( cmd ) ) {
        return action;
      }
      else {
        return action + " " + mod + " " + cmd;
      }
    }


    public static void DecompNodeText( String nodeText, out String action, out char mod, out String cmd )
    {
      action = ""; cmd = ""; mod = ( nodeText.Contains( INV_MOD ) ) ? INV_MOD : REG_MOD;
      String[] e = nodeText.Split( new char[] { REG_MOD, INV_MOD }, StringSplitOptions.RemoveEmptyEntries );
      if ( e.Length > 1 ) {
        action = e[0].Trim( );
        cmd = e[1].Trim( );
      }
      else if ( e.Length > 0 ) {
        action = e[0].Trim( );
        cmd = "";
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
      String action, cmd; char mod;
      DecompNodeText( nodeText, out action, out mod, out cmd );
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
      String action, cmd; char mod;
      DecompNodeText( nodeText, out action, out mod, out cmd );
      return cmd;
    }

    /// <summary>
    /// Returns the invert modifier of the command part from a node text
    /// i.e.  v_pitch - js1_x returns false v_pitch ! js1_x  returns true
    /// </summary>
    /// <param name="nodeText">The node text in 'action - command' notation</param>
    /// <returns>True if there is a command and if it contains an inverter else false</returns>
    public static Boolean CommandInvertFromNodeText( String nodeText )
    {
      String action, cmd; char mod;
      DecompNodeText( nodeText, out action, out mod, out cmd );
      return ( mod == INV_MOD );
    }

    #endregion



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
      this.m_modifier = srcNode.m_modifier;
    }

    public ActionTreeNode( string text )
    {
      this.Text = text;
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
    private char m_modifier = REG_MOD;

    public new String Text
    {
      get { return base.Text; }
      set
      {
        DecompNodeText( value, out m_action, out m_modifier, out m_command );
        base.Text = ComposeNodeText( m_action, m_modifier, m_command );
      }
    }


    public String Action
    {
      get { return m_action; }
      set
      {
        m_action = value;
        base.Text = ComposeNodeText( m_action, m_modifier, m_command );
      }
    }

    public String Command
    {
      get { return m_command; }
      set
      {
        m_command = value;
        base.Text = ComposeNodeText( m_action, m_modifier, m_command );
      }
    }

    public Boolean InvertCommand
    {
      get { return ( m_modifier == INV_MOD ); }
      set
      {
        m_modifier = ( value ) ? INV_MOD : REG_MOD;
        base.Text = ComposeNodeText( m_action, m_modifier, m_command );
      }
    }



  }
}
