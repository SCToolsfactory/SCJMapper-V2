using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Devices.Gamepad
{
  public partial class UC_GpadPanel : UserControl
  {
    public UC_GpadPanel( )
    {
      InitializeComponent( );
    }
    #region Strings

    /// <summary>
    /// Return s only if length is max 12 (some crazy names may kill the layout...)
    /// </summary>
    /// <param name="s">String to return if length less than 12</param>
    /// <param name="d">String to return if s length more than 12</param>
    /// <returns>A string</returns>
    private String Chk( String s, String d )
    {
      if ( s.Length > 12 ) return d; else return s;
    }


    public String Caption
    {
      set { gBoxCap.Text = value; }
    }

    public String DPad
    {
      set { iDPad.Text = value; }
    }

    public String TStickXL
    {
      set { iTStickXL.Text = value; }
    }

    public String TStickYL
    {
      set { iTStickYL.Text = value; }
    }

    public String TStickBtL
    {
      set { iTStickBtL.Text = value; }
    }

    public String TStickXR
    {
      set { iTStickXR.Text = value; }
    }

    public String TStickYR
    {
      set { iTStickYR.Text = value; }
    }

    public String TStickBtR
    {
      set { iTStickBtR.Text = value; }
    }

    public String TriggerL
    {
      set { iTrigL.Text = value; }
    }

    public String TriggerR
    {
      set { iTrigR.Text = value; }
    }

    public String ShoulderL
    {
      set { iShL.Text = value; }
    }

    public String ShoulderR
    {
      set { iShR.Text = value; }
    }

    public String Start
    {
      set { iBtStart.Text = value; }
    }

    public String Back
    {
      set { iBtBack.Text = value; }
    }

    public String Button
    {
      set { lbl1Buttons.Text = value; }
    }

    // Caps

    public String nButtons
    {
      set { lblnButtons.Text = value; }
    }

    public String nDPads
    {
      set { lblnDPad.Text = value; }
    }

    public String nTSticks
    {
      set { lblnTSticks.Text = value; }
    }

    public String nTriggers
    {
      set { lblnTriggers.Text = value; }
    }


    #endregion


    #region Enables

    public Boolean DPadE
    {
      set { iDPad.Enabled = value; lDPad.Enabled = value; }
    }

    public Boolean TStickLE
    {
      set { iTStickXL.Enabled = value; iTStickYL.Enabled = value; iTStickBtL.Enabled = true; lTStickL.Enabled = value; }
    }

    public Boolean TStickRE
    {
      set { iTStickXR.Enabled = value; iTStickYR.Enabled = value; iTStickBtR.Enabled = true; lTStickR.Enabled = value; }
    }

    public Boolean TriggerLE
    {
      set { iTrigL.Enabled = value; lTrigL.Enabled = value; }
    }

    public Boolean TriggerRE
    {
      set { iTrigR.Enabled = value; lTrigR.Enabled = value; }
    }

    public Boolean ShoulderLE
    {
      set { iShL.Enabled = value; lH0.Enabled = value; }
    }

    public Boolean ShoulderRE
    {
      set { iShR.Enabled = value; lH1.Enabled = value; }
    }

    public Boolean StartE
    {
      set { iBtStart.Enabled = value; lH2.Enabled = value; }
    }

    public Boolean BackE
    {
      set { iBtBack.Enabled = value; lH3.Enabled = value; }
    }

    public Boolean ButtonE
    {
      set { lbl1Buttons.Enabled = value; lB.Enabled = value; }
    }

    #endregion


 
  }
}
