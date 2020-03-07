using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// A display list of items to be drawn in the paint event
  /// </summary>
  class DisplayList : List<IShape>
  {

    /// <summary>
    /// Draw all items in the list
    /// </summary>
    /// <param name="g">Graphics context to draw to</param>
    public void DrawList( Graphics g )
    {
      foreach ( var item in this ) {
        item.DrawShape( g );
      }
    }


  }
}
