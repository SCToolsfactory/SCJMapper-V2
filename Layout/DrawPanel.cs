using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// Subclassing the Panel to draw on it 
  /// Sets the graphics props to avoid flicker
  /// </summary>
  class DrawPanel : System.Windows.Forms.Panel
  {
    public DrawPanel()
    {
      this.SetStyle(
          System.Windows.Forms.ControlStyles.UserPaint |
          System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
          System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
          true );
    }

    public Image BackgroundImageResized
    {
      get => base.BackgroundImage;
      set {
        base.BackgroundImage = value;
        this.Size = base.BackgroundImage.Size; // autosize the panel to the image size
        this.Refresh( );

      }
    }

  }
}