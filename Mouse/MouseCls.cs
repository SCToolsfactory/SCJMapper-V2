using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  public class MouseCls
  {
    // mwheel_up, mwheel_down


    public static String MouseCmd(MouseEventArgs e )
    {
      String mbs = "";
      switch ( e.Button ) {
        case MouseButtons.Left: {
          mbs = "mouse1";
            break;
          }
        case MouseButtons.Middle: {
            mbs = "mouse3";
            break;
          }
        case MouseButtons.Right: {
            mbs = "mouse2";
            break;
          }
        case MouseButtons.XButton1: {
            mbs = "mouse4";
            break;
          }
        case MouseButtons.XButton2: {
            mbs = "mouse5";
            break;
          }
      }
      return mbs;
    }

  }
}
