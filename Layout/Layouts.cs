using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  class Layouts : List<DeviceLayout>
  {
    /// <summary>
    /// cTor: Collect all Layouts in the distribution folder
    /// </summary>
    public Layouts()
    {
      this.Clear( );
      if ( !Directory.Exists( TheUser.LayoutsDir ) ) return;

      var jsons = Directory.EnumerateFiles( TheUser.LayoutsDir, "*.json" );
      foreach ( var f in jsons ) {
        var devLayout = new DeviceLayout {
          Filename = f,
          DeviceController = ControllerJson.FromJson( f )
        };
        if ( devLayout.DeviceController != null ) {
          this.Add( devLayout );
        }
      }
    }


  }
}
