using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Table
{

  /// <summary>
  /// A driver custom event to callback for exposure
  /// </summary>
  public class EditRowEventArgs : EventArgs
  {
    public string Actionmap { get; private set; }
    public string Actionkey { get; private set; }
    public int Nodeindex { get; private set; }

    public EditRowEventArgs( string actionmap, string actionkey, int nodeindex )
    {
      Actionmap = actionmap;
      Actionkey = actionkey;
      Nodeindex = nodeindex;
    }
  }

  /// <summary>
  /// A driver custom event to callback for exposure
  /// </summary>
  public class UpdateEditEventArgs : EventArgs
  {
    public UpdateEditEventArgs( )
    {
    }
  }



}
