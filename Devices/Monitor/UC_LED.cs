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
  public partial class UC_LED : UserControl
  {
    private const int c_RoundOffset = 0;
    private const int c_RectOffset = 5; // rect images start
    private int m_colorOffset = 0;

    private const int c_OFF = 4; // Amber
    private const int c_ON = 2; // Green

    public UC_LED()
    {
      InitializeComponent( );
    }

    private void UC_LEDRound_Load( object sender, EventArgs e )
    {
      this.BackgroundImage = IL.Images[m_colorOffset + ( ( m_switch ) ? c_ON : c_OFF )];
    }

    private bool m_switch = false;
    private bool m_rectShape = false;

    /// <summary>
    /// Set the switch and its appearance
    /// </summary>
    public bool Switch { get => m_switch; set { m_switch = value; BackgroundImage = IL.Images[m_colorOffset + ( ( m_switch ) ? c_ON : c_OFF )]; } }
    public void ON() { Switch = true; }
    public void OFF() { Switch = false; }

    /// <summary>
    /// Selects the shape (true=Rect; false=Round)
    /// </summary>
    public bool RectShape { get => m_rectShape; set { m_rectShape = value; m_colorOffset = ( m_rectShape ) ? c_RectOffset : c_RoundOffset; } }

    private void UC_LED_Click( object sender, EventArgs e )
    {
      Switch = !m_switch;
    }
  }
}
