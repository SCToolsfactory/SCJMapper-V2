using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// The shape drawing/interaction interface
  /// </summary>
  interface IShape
  {
    bool IsValid { get; }

    /// <summary>
    /// Draws the shape 
    /// </summary>
    void DrawShape( Graphics g );

    /// <summary>
    /// Sets X,Y from Mouse location - shape is centered
    /// </summary>
    /// <param name="loc"></param>
    void SetMouseLocation( Point loc );

    /// <summary>
    /// Returns true if the item contains the location
    /// </summary>
    /// <param name="location">A location point</param>
    /// <returns>True if the location is with the item area</returns>
    bool HitTest( Point location );

    /// <summary>
    /// Offset of click location vs. middle of the rectangle 
    /// to move it seamlessly
    /// </summary>
    /// <param name="location">Click location</param>
    /// <returns>Movement offset</returns>
    Point ClickOffset( Point location );

  }
}
