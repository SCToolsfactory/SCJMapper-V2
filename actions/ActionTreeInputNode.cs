using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Our INPUT TreeNode - inherits a regular one and adds some functionality
  /// 
  /// contains the input command  i.e.  - js2_button3 OR ! js1_x  (MODs applies at the very beginning of the string)
  /// </summary>
  class ActionTreeInputNode : ActionTreeNode
  {

    #region Static items


    // Handle all text label composition and extraction here

    /// <summary>
    /// Returns a the cmd with standard modifier if modified == true
    /// </summary>
    /// <param name="cmd">Any string</param>
    /// <param name="modified">Bool true if a modifier shall be added</param>
    /// <returns>The string with added Modifier if requested</returns>
    public static string ComposeNodeText( string cmd, bool modified = false )
    {
      if ( string.IsNullOrEmpty( cmd ) ) {
        return "";
      }
      else {
        if ( modified )
          return string.Format( "{0} {1}", cmd, ActionTreeNode.ModDiv ); // js1_button1 #
        else
          return string.Format( "{0}", cmd );                            // js1_button1
      }
    }

    /// <summary>
    /// Returns the cmd part of a string like "cmd - anything #" 
    /// </summary>
    /// <param name="nodeText">A nodetext string like "cmd - anything #"</param>
    /// <param name="cmd">contains the cmd part if delimiters are present - else returns the input</param>
    public static void DecompNodeText( string nodeText, out string cmd )
    {
      string[] e = nodeText.Split( new char[] { RegDiv, ModDiv }, StringSplitOptions.RemoveEmptyEntries );
      if ( e.Length > 0 ) 
        cmd = e[0].TrimEnd( );
      else
        cmd = nodeText;
    }


    /// <summary>
    /// Returns the command part from a node text
    /// i.e.  "v_pitch - js1_x" returns v_pitch
    /// </summary>
    /// <param name="nodeText">The node text in 'action - command' notation</param>
    /// <returns>the command part or an empty string</returns>
    public new static string CommandFromNodeText( string nodeText )
    {
      string cmd;
      ActionTreeInputNode.DecompNodeText( nodeText, out cmd );
      return cmd;
    }

    #endregion


    // Object defs

    // ctor
    public ActionTreeInputNode( )
      : base( )
    {
    }

    // ctor
    public ActionTreeInputNode( ActionTreeInputNode srcNode )
      : base( srcNode )
    {
      if ( srcNode == null ) return;
      /*
      this.Name = srcNode.Name;
      this.Text = srcNode.Text;
      this.BackColor = srcNode.BackColor;
      this.ForeColor = srcNode.ForeColor;
      this.NodeFont = srcNode.NodeFont;
      this.ImageKey = srcNode.ImageKey;
      this.Tag = srcNode.Tag;
      this.m_command = srcNode.m_command;
       */
    }

    // ctor
    public ActionTreeInputNode( string text )
      : base ( text )
    {
      //this.Text = text;
    }

    // ctor
    public ActionTreeInputNode( string text, ActionTreeInputNode[] children )
      : base( text, children )
    {
    }


    //private string m_command ="";

    public new string Text
    {
      get { return base.Text; }
      set
      {
        ActionTreeInputNode.DecompNodeText( value, out m_command );
        base.Text = ActionTreeInputNode.ComposeNodeText( "$" + m_command, m_modified ); // tag for the node processing
      }
    }


    public new string Command
    {
      get { return m_command; }
      set
      {
        m_command = value;
        Text = ActionTreeInputNode.ComposeNodeText( m_command, m_modified ); // compose - later it will be decomposed again
      }
    }

  }
}
