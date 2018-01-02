using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Our INPUT TreeNode - inherits a ActionTreeNode one and adds some functionality
  /// 
  /// contains the input command  i.e.  - js2_button3 OR ! js1_x  (MODs applies at the very beginning of the string)
  /// </summary>
  class ActionTreeInputNode : ActionTreeNode
  {

    #region Static items


    // Handle all text label composition and extraction here

    ///// <summary>
    ///// Returns a the cmd with standard modifier if modified == true
    ///// </summary>
    ///// <param name="cmd">Any string</param>
    ///// <param name="modified">Bool true if a modifier shall be added</param>
    ///// <returns>The string with added Modifier if requested</returns>
    //private static string ComposeNodeText( string cmd, bool modified = false )
    //{
    //  if ( string.IsNullOrEmpty( cmd ) ) {
    //    return "";
    //  }
    //  else {
    //    if ( modified )
    //      return string.Format( "{0} {1}", cmd, ActionTreeNode.ModDiv ); // js1_button1 #
    //    else
    //      return string.Format( "{0}", cmd );                            // js1_button1
    //  }
    //}

    ///// <summary>
    ///// Returns the cmd part of a string like "cmd - anything #" 
    ///// </summary>
    ///// <param name="nodeText">A nodetext string like "cmd - anything #"</param>
    ///// <param name="cmd">contains the cmd part if delimiters are present - else returns the input</param>
    //private static void DecompNodeText( string nodeText, out string cmd )
    //{
    //  string[] e = nodeText.Split( new char[] { RegDiv, ModDiv }, StringSplitOptions.RemoveEmptyEntries );
    //  if ( e.Length > 0 ) 
    //    cmd = e[0].TrimEnd( );
    //  else
    //    cmd = nodeText;
    //}


    ///// <summary>
    ///// Returns the command part from a node text
    ///// i.e.  "v_pitch - js1_x" returns v_pitch
    ///// </summary>
    ///// <param name="nodeText">The node text in 'action - command' notation</param>
    ///// <returns>the command part or an empty string</returns>
    //private new static string CommandFromNodeText( string nodeText )
    //{
    //  ActionTreeInputNode.DecompNodeText( nodeText, out string cmd );
    //  return cmd;
    //}

    #endregion


    // Object defs

    /// <summary>
    /// cTor: empty
    /// </summary>
    public ActionTreeInputNode( )
      : base( )
    {
    }

    /// <summary>
    /// cTor: just create a node without added functionality  
    ///    NOTE: must fill properties to work
    /// </summary>
    /// <param name="text">The Text element of the Node</param>
    public ActionTreeInputNode( string text )
      : base ( text )
    {
    }

    /// <summary>
    /// cTor: just create a node without added functionality  
    ///    NOTE: must fill properties to work
    /// </summary>
    /// <param name="text">The Text element of the Node</param>
    /// <param name="children">The child list</param>
    public ActionTreeInputNode( string text, ActionTreeInputNode[] children )
      : base( text, children )
    {
    }

    /// <summary>
    /// cTor: Copy src to this new node
    /// </summary>
    /// <param name="srcNode">Src node to copy from</param>
    public ActionTreeInputNode( ActionTreeInputNode srcNode )
      : base( srcNode )
    {
    }


    ///// <summary>
    ///// Property Text of an Input node
    ///// </summary>
    //public new string Text
    //{
    //  get { return base.Text; }
    //  set
    //  {
    //    ActionTreeInputNode.DecompNodeText( value, out m_command );
    //    m_actionText = ActionTreeInputNode.ComposeNodeText( AddDiv + m_command, m_modified ); // tag for the node processing
    //    base.Text = m_actionText; // TODO
    //  }
    //}


    //public new string Command
    ///// <summary>
    ///// Property Command of an Input node
    ///// </summary>
    //{
    //  get { return m_command; }
    //  set
    //  {
    //    m_command = value;
    //    m_actionText = ActionTreeInputNode.ComposeNodeText( m_command, m_modified ); // compose - later it will be decomposed again
    //    base.Text = m_actionText; // TODO
    //  }
    //}

  }
}
