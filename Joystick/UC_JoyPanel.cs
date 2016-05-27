using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Joystick
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
    /// <param name="s">String to return if length less than 12</param>
    /// <param name="d">String to return if s length more than 12</param>
    /// <returns>A string</returns>
    private String Chk( String s, String d )
    {
      if ( s.Length > 12 ) return s.Substring(0,12) ; else return s;
    }


    public String Caption
    {
      set { gBoxCap.Text = value; }
    }

    public String X
    {
      set { lbl1X.Text = value; }
    }

    public String Xname
    {
      set { lX.Text = Chk( value, lX.Text ); }
    }

    public String Y
    {
      set { lbl1Y.Text = value; }
    }

    public String Yname
    {
      set { lY.Text = Chk( value, lY.Text ); }
    }

    public String Z
    {
      set { lbl1Z.Text = value; }
    }

    public String Zname
    {
      set { lZ.Text = Chk( value, lZ.Text ); }
    }

    public String Xr
    {
      set { lbl1Xr.Text = value; }
    }

    public String Xrname
    {
      set { lXr.Text = Chk( value, lXr.Text ); }
    }

    public String Yr
    {
      set { lbl1Yr.Text = value; }
    }

    public String Yrname
    {
      set { lYr.Text = Chk( value, lYr.Text ); }
    }

    public String Zr
    {
      set { lbl1Zr.Text = value; }
    }

    public String Zrname
    {
      set { lZr.Text = Chk( value, lZr.Text ); }
    }

    public String S1
    {
      set { lbl1S0.Text = value; }
    }

    public String S1name
    {
      set { lS0.Text = Chk( value, lS0.Text ); }
    }

    public String S2
    {
      set { lbl1S1.Text = value; }
    }

    public String S2name
    {
      set { lS1.Text = Chk( value, lS1.Text ); }
    }

    public String H1
    {
      set { lbl1Hat0.Text = value; }
    }

    public String H1name
    {
      set { lH0.Text = Chk( value, lH0.Text ); }
    }

    public String H2
    {
      set { lbl1Hat1.Text = value; }
    }

    public String H2name
    {
      set { lH1.Text = Chk( value, lH1.Text ); }
    }

    public String H3
    {
      set { lbl1Hat2.Text = value; }
    }

    public String H3name
    {
      set { lH2.Text = Chk( value, lH2.Text ); }
    }

    public String H4
    {
      set { lbl1Hat3.Text = value; }
    }

    public String H4name
    {
      set { lH3.Text = Chk( value, lH3.Text ); }
    }

    public String Button
    {
      set { lbl1Buttons.Text = value; }
    }


    public String nButtons
    {
      set { lblnButtons.Text = value; }
    }

    public String nAxis
    {
      set { lblnAxis.Text = value; }
    }

    public String nPOVs
    {
      set { lblnPOVs.Text = value; }
    }


    #endregion


    #region Enables

    public Boolean Xe
    {
      set { lbl1X.Enabled = value; lX.Enabled = value; }
    }

    public Boolean Ye
    {
      set { lbl1Y.Enabled = value; lY.Enabled = value; }
    }

    public Boolean Ze
    {
      set { lbl1Z.Enabled = value; lZ.Enabled = value; }
    }

    public Boolean Xre
    {
      set { lbl1Xr.Enabled = value; lXr.Enabled = value; }
    }

    public Boolean Yre
    {
      set { lbl1Yr.Enabled = value; lYr.Enabled = value; }
    }

    public Boolean Zre
    {
      set { lbl1Zr.Enabled = value; lZr.Enabled = value; }
    }

    public Boolean S1e
    {
      set { lbl1S0.Enabled = value; lS0.Enabled = value; }
    }

    public Boolean S2e
    {
      set { lbl1S1.Enabled = value; lS1.Enabled = value; }
    }

    public Boolean H1e
    {
      set { lbl1Hat0.Enabled = value; lH0.Enabled = value; }
    }

    public Boolean H2e
    {
      set { lbl1Hat1.Enabled = value; lH1.Enabled = value; }
    }

    public Boolean H3e
    {
      set { lbl1Hat2.Enabled = value; lH2.Enabled = value; }
    }

    public Boolean H4e
    {
      set { lbl1Hat3.Enabled = value; lH3.Enabled = value; }
    }

    public Boolean Buttone
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
          lblJsAssignment.Text = String.Format( "js{0}", m_jsAssignment );
        }
        else {
          lblJsAssignment.Text = "not assigned";
        }
      }
    }

    public String JsName
    {
      get { return JoystickCls.JSTag(m_jsAssignment) + "_"; }
    }

    #endregion
  }
}
