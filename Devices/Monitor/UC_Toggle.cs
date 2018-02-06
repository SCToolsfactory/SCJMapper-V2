using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCJMapper_V2.Devices.Monitor
{
  public partial class UC_Toggle : UserControl
  {
    public UC_Toggle()
    {
      InitializeComponent( );

      ucLed.RectShape = true;
    }


    public bool Switch { get => ucLed.Switch; set => ucLed.Switch = value; }
    public string Label { get => lblContent.Text; set => lblContent.Text = value; }

    public void ToggleState()
    {
      ucLed.Switch = !ucLed.Switch;
    }

  }
}
