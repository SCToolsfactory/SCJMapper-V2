using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Our INPUT TreeNode - inherits from ActionTreeNode  - serves as a distinct type in the ActionTree for Inputs
  /// 
  /// </summary>
  class ActionTreeInputNode : ActionTreeNode
  {

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

  }
}
