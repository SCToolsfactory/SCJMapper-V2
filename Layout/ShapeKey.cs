using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCJMapper_V2.Common;
using SCJMapper_V2.Devices.Keyboard;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// Key Input 
  /// contains a text to display at a position within a rectangle
  /// </summary>
  class ShapeKey : ShapeItem
  {
    /// <summary>
    /// The SCGameKey for this Command
    /// Only a single one - NO modifiers here
    /// </summary>
    public string SCGameKey { get; set; }

    /// <summary>
    /// The DX GameKey for this Command
    /// </summary>
    public SharpDX.DirectInput.Key DXGameKey
    {
      get {
        return KeyboardCls.FromSCKeyboardCmd( SCGameKey );
      }
    }

    /// <summary>
    /// The Windows Virtual GameKey for this Command
    /// </summary>
    public VirtualKey WinVirtualKey
    {
      get {
        return (VirtualKey)WinApi.MapVirtualKeyEx( (uint)DXGameKey, WinApi.VirtualKeyMapType.MAPVK_VSC_TO_VK_EX, IntPtr.Zero );
      }
    }

    /// <summary>
    /// Indicates that the Key symbol needs to be drawn
    /// </summary>
    public bool IsSymbolShape { get; set; } = false; // default

    /// <summary>
    /// GetRoundRectPath
    /// Credit: licensed under The Code Project Open License (CPOL)
    /// https://www.codeproject.com/Articles/27228/A-class-for-creating-round-rectangles-in-GDI-with
    ///  This function uses the AddArc method for defining the rounded rectangle path.
    ///  The first workaround handles the special case where the radius is 10. 
    ///  It offsets the arc's rectangle and increases its size at a strategic point. 
    ///  I don’t have a good theory for why this works or why it is only needed for a radius of 10.
    /// </summary>
    private void GetRoundRectPath( ref GraphicsPath pPath, Rectangle r, int dia )
    {
      // diameter can't exceed width or height
      if ( dia > r.Width ) dia = r.Width;
      if ( dia > r.Height ) dia = r.Height;

      // define a corner 
      var Corner = new Rectangle( r.X, r.Y, dia, dia );
      pPath.Reset( ); // begin path    
      // top left
      pPath.AddArc( Corner, 180, 90 );
      // tweak needed for radius of 10 (dia of 20)
      if ( dia == 20 ) {
        Corner.Width += 1;
        Corner.Height += 1;
        r.Width -= 1; r.Height -= 1;
      }
      // top right
      Corner.X += ( r.Width - dia - 1 );
      pPath.AddArc( Corner, 270, 90 );
      // bottom right
      Corner.Y += ( r.Height - dia - 1 );
      pPath.AddArc( Corner, 0, 90 );
      // bottom left
      Corner.X -= ( r.Width - dia - 1 );
      pPath.AddArc( Corner, 90, 90 );
      // end path
      pPath.CloseFigure( );
    }

    /// <summary>
    /// DrawRoundRect
    /// Credit: licensed under The Code Project Open License (CPOL)
    /// https://www.codeproject.com/Articles/27228/A-class-for-creating-round-rectangles-in-GDI-with
    ///  This function draws a rounded rectangle using the passed rectangle, radius, pen color, and pen width.
    ///  The second workaround involves using a pen width of 1 and drawing “width” number of rectangles, 
    ///  decrementing the size of the rect each time.That alone is insufficient, because it will leave 
    ///  holes at the corners. Instead, this deflates only the x, draws the rect, then deflates the y, and draws again.
    /// </summary>
    private void DrawRoundRect( Graphics pGraphics, Rectangle r, Color color, int radius, int width )
    {
      int dia = 2 * radius;

      // set to pixel mode
      var oldPageUnit = pGraphics.PageUnit;
      pGraphics.PageUnit = GraphicsUnit.Pixel;

      // define the pen
      var pen = new Pen( color, 1 );
      pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

      // get the corner path
      var path = new GraphicsPath( );
      // get path
      GetRoundRectPath( ref path, r, dia );
      // draw the round rect
      pGraphics.DrawPath( pen, path );
      // if width > 1
      for ( int i = 1; i < width; i++ ) {
        r.Inflate( -1, 0 ); // left stroke
        GetRoundRectPath( ref path, r, dia ); // get the path
        pGraphics.DrawPath( pen, path );  // draw the round rect
        r.Inflate( 0, -1 ); // up stroke
        GetRoundRectPath( ref path, r, dia ); // get the path
        pGraphics.DrawPath( pen, path );  // draw the round rect
      }
      // restore page unit
      pGraphics.PageUnit = oldPageUnit;
    }

    /// <summary>
    /// Draw a key
    /// </summary>
    /// <param name="g"></param>
    /// <param name="location"></param>
    /// <param name="key"></param>
    private void DrawKey( Graphics g, Rectangle drawRect, string key )
    {
      var printSize =Size.Add( Size.Ceiling( g.MeasureString( key, MapProps.MapFont ) ), new Size(18,18)); // get the surounding box for the Text 
      var rect = new Rectangle( drawRect.Location, printSize );
      rect.Offset( 0, ( drawRect.Height - printSize.Height ) / 2 ); // try to find the middle by shifting the drawing
      if ( rect.Width < rect.Height ) rect.Width = rect.Height; // minimum with
      DrawRoundRect( g, rect, MapProps.KbdSymbolPen.Color, 7, 3 );
      rect.Inflate( -5, -5 );
      DrawRoundRect( g, rect, MapProps.KbdSymbolPen.Color, 7, 3 );
      rect.Inflate( -2, -2 );
      g.DrawString( key, MapProps.MapFont, MapProps.KbdSymbolBrush, rect ); // write into the rectangle
    }

    #region IShape Implementation

    /// <summary>
    /// Draws the shape 
    /// </summary>
    public override void DrawShape( Graphics g )
    {
      // Key Symbol left of the Text Location
      if ( IsValid ) {
        var symbolRect = Rectangle;
        symbolRect.Offset( -120, 0 ); // TODO  get a proper left offset rather than static (Should be left aligned though..)
        symbolRect.Width = 120;
        string key = WinApi.KbdScanCodeToVK((uint)DXGameKey); // might work....
        if (IsSymbolShape) DrawKey( g, symbolRect, key );
      }
      // draw the text
      base.DrawShape( g );
    }

    #endregion

  }
}
