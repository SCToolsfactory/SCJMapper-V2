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
  public partial class UC_SwitchRect : UserControl
  {
    public UC_SwitchRect()
    {
      InitializeComponent( );

    }

    private void UC_SwitchRect_Load( object sender, EventArgs e )
    {
      this.BackgroundImage = IL.Images[( m_switch ) ? "ON" : "OFF"];
    }

    private bool m_switch = false;
    /// <summary>
    /// Set the switch and its appearance
    /// </summary>
    public bool Switch { get => m_switch; set { m_switch = value; BackgroundImage = IL.Images[( m_switch ) ? "ON" : "OFF"]; } }

    // toggle
    private void UC_SwitchRect_Click( object sender, EventArgs e )
    {
      Switch = !m_switch;
    }

  }
}
