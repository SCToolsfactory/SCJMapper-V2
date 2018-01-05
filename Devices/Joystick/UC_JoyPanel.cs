using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Devices.Joystick
{
  public partial class UC_JoyPanel : UserControl
  {
    public UC_JoyPanel( )
    {
      InitializeComponent( );
    }

    private int m_jsAssignment = 0; // na


    #region Strings

    /// <summary>
    /// Return s only if length is max 12 (some crazy names may kill the layout...)
    /// </summary>
    /// <param name="s">string to return if length less than 12</param>
    /// <param name="d">string to return if s length more than 12</param>
    /// <returns>A string</returns>
    private string Chk( string s, string d )
    {
      if ( s.Length > 12 ) return s.Substring(0,12) ; else return s;
    }


    public string Caption
    {
      set { gbxBoxCap.Text = value; }
    }

    public string X
    {
      set { lbl1X.Text = value; }
    }

    public string Xname
    {
      set { lX.Text = Chk( value, lX.Text ); }
    }

    public string Y
    {
      set { lbl1Y.Text = value; }
    }

    public string Yname
    {
      set { lY.Text = Chk( value, lY.Text ); }
    }

    public string Z
    {
      set { lbl1Z.Text = value; }
    }

    public string Zname
    {
      set { lZ.Text = Chk( value, lZ.Text ); }
    }

    public string Xr
    {
      set { lbl1Xr.Text = value; }
    }

    public string Xrname
    {
      set { lXr.Text = Chk( value, lXr.Text ); }
    }

    public string Yr
    {
      set { lbl1Yr.Text = value; }
    }

    public string Yrname
    {
      set { lYr.Text = Chk( value, lYr.Text ); }
    }

    public string Zr
    {
      set { lbl1Zr.Text = value; }
    }

    public string Zrname
    {
      set { lZr.Text = Chk( value, lZr.Text ); }
    }

    public string S1
    {
      set { lbl1S0.Text = value; }
    }

    public string S1name
    {
      set { lS0.Text = Chk( value, lS0.Text ); }
    }

    public string S2
    {
      set { lbl1S1.Text = value; }
    }

    public string S2name
    {
      set { lS1.Text = Chk( value, lS1.Text ); }
    }

    public string H1
    {
      set { lbl1Hat0.Text = value; }
    }

    public string H1name
    {
      set { lH0.Text = Chk( value, lH0.Text ); }
    }

    public string H2
    {
      set { lbl1Hat1.Text = value; }
    }

    public string H2name
    {
      set { lH1.Text = Chk( value, lH1.Text ); }
    }

    public string H3
    {
      set { lbl1Hat2.Text = value; }
    }

    public string H3name
    {
      set { lH2.Text = Chk( value, lH2.Text ); }
    }

    public string H4
    {
      set { lbl1Hat3.Text = value; }
    }

    public string H4name
    {
      set { lH3.Text = Chk( value, lH3.Text ); }
    }

    public string Button
    {
      set { lbl1Buttons.Text = value; }
    }


    public string nButtons
    {
      set { lblnButtons.Text = value; }
    }

    public string nAxis
    {
      set { lblnAxis.Text = value; }
    }

    public string nPOVs
    {
      set { lblnPOVs.Text = value; }
    }


    #endregion


    #region Enables

    public bool Xe
    {
      set { lbl1X.Enabled = value; lX.Enabled = value; }
    }

    public bool Ye
    {
      set { lbl1Y.Enabled = value; lY.Enabled = value; }
    }

    public bool Ze
    {
      set { lbl1Z.Enabled = value; lZ.Enabled = value; }
    }

    public bool Xre
    {
      set { lbl1Xr.Enabled = value; lXr.Enabled = value; }
    }

    public bool Yre
    {
      set { lbl1Yr.Enabled = value; lYr.Enabled = value; }
    }

    public bool Zre
    {
      set { lbl1Zr.Enabled = value; lZr.Enabled = value; }
    }

    public bool S1e
    {
      set { lbl1S0.Enabled = value; lS0.Enabled = value; }
    }

    public bool S2e
    {
      set { lbl1S1.Enabled = value; lS1.Enabled = value; }
    }

    public bool H1e
    {
      set { lbl1Hat0.Enabled = value; lH0.Enabled = value; }
    }

    public bool H2e
    {
      set { lbl1Hat1.Enabled = value; lH1.Enabled = value; }
    }

    public bool H3e
    {
      set { lbl1Hat2.Enabled = value; lH2.Enabled = value; }
    }

    public bool H4e
    {
      set { lbl1Hat3.Enabled = value; lH3.Enabled = value; }
    }

    public bool Buttone
    {
      set { lbl1Buttons.Enabled = value; lB.Enabled = value; }
    }

    #endregion


    #region jsAsignment

    /// <summary>
    /// jsN assignment
    /// Note: this is supposed to be either 1..4 for assigned ones or 0 for unassigned ones
    /// </summary>
    public int JsAssignment
    {
      get { return m_jsAssignment; }
      set
      {
        m_jsAssignment = value;
        if ( ( m_jsAssignment >= 1 ) && ( m_jsAssignment <= JoystickCls.JSnum_MAX ) ) {
          lblJsAssignment.Text = string.Format( "js{0}", m_jsAssignment );
        }
        else {
          lblJsAssignment.Text = "not assigned";
        }
      }
    }

    public string JsName
    {
      get { return JoystickCls.JSTag(m_jsAssignment) + "_"; }
    }

    #endregion
  }
}
