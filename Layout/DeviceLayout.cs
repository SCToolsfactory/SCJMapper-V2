using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCJMapper_V2.Properties;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// One Device Layout (from Json description)
  /// </summary>
  class DeviceLayout
  {
    public DeviceFile DeviceController { get; set; } = new DeviceFile( );
    public string Filename { get; set; } = "";

    /// <summary>
    /// Returns the Display string for this object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return DeviceController.MapName;
    }

    /// <summary>
    /// Returns the background image from the appropriate folder
    /// </summary>
    /// <param name="imageFilename">The image filename</param>
    /// <returns>An image or a dummy one if it does not exist</returns>
    public Image Image
    {
      get {
        string fn = this.DeviceController.MapImage;
        if ( !File.Exists( fn ) ) {
          fn = Path.Combine( TheUser.LayoutsDir, fn ); // if it is not found use the default path
        }
        if ( File.Exists( fn ) ) {
          return Image.FromFile( fn );
        }
        else {
          return Resources.page_notdefined;
        }

      }
    }

  }
}
