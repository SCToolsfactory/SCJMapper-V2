using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// Controller Input 
  /// contains a text to display at a position within a rectangle
  /// </summary>
  class ShapeItem : IShape
  {
    /// <summary>
    /// The Text Shown in the Map
    /// </summary>
    public string DispText { get; set; }

    /// <summary>
    /// Location Left
    /// </summary>
    public int X { get; set; } = 0;
    /// <summary>
    /// Location Top
    /// </summary>
    public int Y { get; set; } = 0;
    /// <summary>
    /// Width
    /// </summary>
    public int Width { get; set; } = 0;
    /// <summary>
    /// Height
    /// </summary>
    public int Height { get; set; } = 0;

    public Point Location
    {
      get { return new Point( X, Y ); }
      set { X = value.X; Y = value.Y; }
    }
    public Size Size
    {
      get { return new Size( Width, Height ); }
      set { Width = value.Width; Height = value.Height; }
    }
    public Rectangle Rectangle
    {
      get { return new Rectangle( X, Y, Width, Height ); }
      set { X = value.X; Y = value.Y; Width = value.Width; Height = value.Height; }
    }

    public bool IsValid { get => !string.IsNullOrEmpty( DispText ); }

    private Brush m_textBrush = Brushes.Black;
    private Color m_textColor = Color.DarkBlue;

    private Brush m_backBrush = Brushes.White;
    private Color m_backColor = Color.White;

    /// <summary>
    /// Set the Textcolor
    /// </summary>
    public Color TextColor
    {
      get => m_textColor;
      set {
        m_textColor = value;
        m_textBrush.Dispose( );
        m_textBrush = new SolidBrush( m_textColor ); // set the text brush as well
      }
    }

    /// <summary>
    /// Set the Textcolor
    /// </summary>
    public Color BackColor
    {
      get => m_backColor;
      set {
        m_backColor = value;
        m_backBrush.Dispose( );
        m_backBrush = new SolidBrush( m_backColor ); // set the text brush as well
      }
    }

    /// <summary>
    /// Returns the drawn text size for this item
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public SizeF MeasureShape( Graphics g )
    {
      return g.MeasureString( DispText, MapProps.MapFont );
    }

    #region IShape Implementation

    /// <summary>
    /// Draws the shape 
    /// </summary>
    public void DrawShape( Graphics g )
    {
      if ( IsValid ) {
        if ( m_backColor!= Color.White ) {
          g.FillRectangle( m_backBrush, Rectangle );
        }
        g.DrawString( DispText, MapProps.MapFont, m_textBrush, Rectangle ); // write into the rectangle
      }
    }

    /// <summary>
    /// Sets X,Y from Mouse location - shape is centered
    /// </summary>
    /// <param name="loc"></param>
    public void SetMouseLocation( Point loc )
    {
    }

    /// <summary>
    /// Returns true if the item contains the location
    /// </summary>
    /// <param name="location">A location point</param>
    /// <returns>True if the location is with the item area</returns>
    public bool HitTest( Point location )
    {
      return new Rectangle( X, Y, Width, Height ).Contains( location );
    }

    /// <summary>
    /// Offset of click location vs. middle of the rectangle 
    /// to move it seamlessly
    /// </summary>
    /// <param name="location">Click location</param>
    /// <returns>Movement offset</returns>
    public Point ClickOffset( Point location )
    {
      return new Point( -( location.X - X - Width / 2 ),
                        -( location.Y - Y - Height / 2 ) );
    }

    #endregion

  }
}
