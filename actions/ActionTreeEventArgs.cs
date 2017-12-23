using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  /// Event class to automate selection,
  /// returns the selected items in the ActionTree
  /// </summary>
  public class ActionTreeEventArgs : EventArgs
  {
    public string SelectedAction { get; set; }
    public string SelectedCtrl { get; set; }

    public ActionTreeEventArgs(string action, string ctrl )
    {
      SelectedAction = action;
      SelectedCtrl = ctrl;
    }
  }

}
