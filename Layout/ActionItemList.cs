using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// A list of ActionItems
  /// </summary>
  class ActionItemList : List<ActionItem>
  {
    /// <summary>
    /// Get the Devices contained in the List
    /// </summary>
    public List<string> Devices
    {
      get {
        var list = new List<string>( );
        foreach ( var si in this ) {
          if ( !list.Contains( si.DeviceName ) ) {
            list.Add( si.DeviceName );
          }
        }
        return list;
      }
    }
  }
}
