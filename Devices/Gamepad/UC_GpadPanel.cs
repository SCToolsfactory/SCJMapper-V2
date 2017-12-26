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
    /// <param name="s">string to return if length less than 12</param>
    /// <param name="d">string to return if s length more than 12</param>
    /// <returns>A string</returns>
    private string Chk( string s, string d )
    {
      if ( s.Length > 12 ) return d; else return s;
    }


    public string Caption
    {
      set { gBoxCap.Text = value; }
    }

    public string DPad
    {
      set { iDPad.Text = value; }
    }

    public string TStickXL
    {
      set { iTStickXL.Text = value; }
    }

    public string TStickYL
    {
      set { iTStickYL.Text = value; }
    }

    public string TStickBtL
    {
      set { iTStickBtL.Text = value; }
    }

    public string TStickXR
    {
      set { iTStickXR.Text = value; }
    }

    public string TStickYR
    {
      set { iTStickYR.Text = value; }
    }

    public string TStickBtR
    {
      set { iTStickBtR.Text = value; }
    }

    public string TriggerL
    {
      set { iTrigL.Text = value; }
    }

    public string TriggerR
    {
      set { iTrigR.Text = value; }
    }

    public string ShoulderL
    {
      set { iShL.Text = value; }
    }

    public string ShoulderR
    {
      set { iShR.Text = value; }
    }

    public string Start
    {
      set { iBtStart.Text = value; }
    }

    public string Back
    {
      set { iBtBack.Text = value; }
    }

    public string Button
    {
      set { lbl1Buttons.Text = value; }
    }

    // Caps

    public string nButtons
    {
      set { lblnButtons.Text = value; }
    }

    public string nDPads
    {
      set { lblnDPad.Text = value; }
    }

    public string nTSticks
    {
      set { lblnTSticks.Text = value; }
    }

    public string nTriggers
    {
      set { lblnTriggers.Text = value; }
    }


    #endregion


    #region Enables

    public bool DPadE
    {
      set { iDPad.Enabled = value; lDPad.Enabled = value; }
    }

    public bool TStickLE
    {
      set { iTStickXL.Enabled = value; iTStickYL.Enabled = value; iTStickBtL.Enabled = true; lTStickL.Enabled = value; }
    }

    public bool TStickRE
    {
      set { iTStickXR.Enabled = value; iTStickYR.Enabled = value; iTStickBtR.Enabled = true; lTStickR.Enabled = value; }
    }

    public bool TriggerLE
    {
      set { iTrigL.Enabled = value; lTrigL.Enabled = value; }
    }

    public bool TriggerRE
    {
      set { iTrigR.Enabled = value; lTrigR.Enabled = value; }
    }

    public bool ShoulderLE
    {
      set { iShL.Enabled = value; lH0.Enabled = value; }
    }

    public bool ShoulderRE
    {
      set { iShR.Enabled = value; lH1.Enabled = value; }
    }

    public bool StartE
    {
      set { iBtStart.Enabled = value; lH2.Enabled = value; }
    }

    public bool BackE
    {
      set { iBtBack.Enabled = value; lH3.Enabled = value; }
    }

    public bool ButtonE
    {
      set { lbl1Buttons.Enabled = value; lB.Enabled = value; }
    }

    #endregion


 
  }
}
