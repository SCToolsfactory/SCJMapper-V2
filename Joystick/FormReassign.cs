using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Joystick
{
  partial class FormReassign : Form
  {
    private readonly JoystickList m_owner = null; // owner class - access to settings
    private TextBox[] m_tb = new TextBox[] { null, null, null, null, null, null, null, null, null, null, null, null, };
    private ComboBox[] m_cb = new ComboBox[] { null, null, null, null, null, null, null, null, null, null, null, null, };

    
    public Boolean Canceled { get; set; }

    /// <summary>
    /// ctor - gets the owning class instance
    /// </summary>
    /// <param name="owner"></param>
    public FormReassign( JoystickList owner )
    {
      InitializeComponent( );
      m_owner = owner;

      m_tb[0] = txJS1; m_tb[1] = txJS2; m_tb[2] = txJS3; m_tb[3] = txJS4;
      m_tb[4] = txJS5; m_tb[5] = txJS6; m_tb[6] = txJS7; m_tb[7] = txJS8;
      m_tb[8] = txJS9; m_tb[9] = txJS10; m_tb[10] = txJS11; m_tb[11] = txJS12;

      m_cb[0] = cbxStick1; m_cb[1] = cbxStick2; m_cb[2] = cbxStick3; m_cb[3] = cbxStick4;
      m_cb[4] = cbxStick5; m_cb[5] = cbxStick6; m_cb[6] = cbxStick7; m_cb[7] = cbxStick8;
      m_cb[8] = cbxStick9; m_cb[9] = cbxStick10; m_cb[10] = cbxStick11; m_cb[11] = cbxStick12;
    }


    private void FormReassign_Load( object sender, EventArgs e )
    {
      int textIdx = 0;
      foreach ( JoystickCls j in m_owner ) {
        m_tb[textIdx++].Text = j.DevName;
      }

      LoadSettings( );
    }


    private void LoadSettings( )
    {
      int textIdx = 0;
      m_owner.JsReassingList.Clear( );
      m_owner.NewJsList.Clear( );
      foreach ( JoystickCls j in m_owner ) {
        m_cb[textIdx].SelectedIndex = ( j.JSAssignment <= JoystickCls.JSnum_MAX ) ? j.JSAssignment : 0;
        m_owner.NewJsList.Add( m_cb[textIdx].SelectedIndex );  // old for now - new depends on Leave Button
        textIdx++;
      }
    }


    private void SaveSettings( )
    {
      int textIdx = 0;
      m_owner.JsReassingList.Clear( );
      foreach ( JoystickCls j in m_owner ) {
        try {
          m_owner.JsReassingList.Add( m_owner.NewJsList[textIdx], m_cb[textIdx].SelectedIndex );
          m_owner.NewJsList[textIdx] = m_cb[textIdx].SelectedIndex; // save with new JsN
        }
        catch {
          ; // nothing - this is a double Old jsN (legacy without instances)
        }
        textIdx++;
      }
    }

    private Boolean IsOK( )
    {
      int[] jsx = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
      foreach ( ComboBox cb in m_cb ) {
        if ( cb.SelectedIndex>0) jsx[cb.SelectedIndex]++;
      }

      return ( ( jsx[1] < 2 ) && ( jsx[2] < 2 ) && ( jsx[3] < 2 ) && ( jsx[4] < 2 ) 
            && ( jsx[5] < 2 ) && ( jsx[6] < 2 ) && ( jsx[7] < 2 ) && ( jsx[8] < 2 )
            && ( jsx[9] < 2 ) && ( jsx[10] < 2 ) && ( jsx[11] < 2 ) && ( jsx[12] < 2 ) ); // each Js can be set only once
    }


    private void btDone_Click( object sender, EventArgs e )
    {
      if ( IsOK( ) ) {
        SaveSettings( );
        Canceled = false;
        this.Hide( );
      }
      else {
        MessageBox.Show( "the same jsN was assigned to more than one Joystick - either fix it or exit with Cancel", "Accept Reassignment", MessageBoxButtons.OK );
      }
    }

    private void btCancel_Click( object sender, EventArgs e )
    {
      LoadSettings( );
      Canceled = true;
      this.Hide( );
    }


    private void FormSettings_FormClosing( object sender, FormClosingEventArgs e )
    {
      SaveSettings( );
      if ( e.CloseReason == CloseReason.UserClosing ) {
        e.Cancel = true;
        this.Hide( );
      }
    }

  
  }
}
