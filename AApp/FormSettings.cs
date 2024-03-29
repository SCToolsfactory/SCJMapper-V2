﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SCJMapper_V2.Common;
using SCJMapper_V2.Actions;
using SCJMapper_V2.Translation;

namespace SCJMapper_V2
{
  partial class FormSettings : Form
  {

    List<CheckBox> m_checks = new List<CheckBox>( );
    List<Label> m_labels = new List<Label>( );

    public bool Canceled { get; set; }

    public string PasteString { get; set; } // used to copy, paste JS commands

    /// <summary>
    /// ctor - gets the owning class instance
    /// </summary>
    /// <param name="owner"></param>
    public FormSettings()
    {
      InitializeComponent( );
    }


    private void FormSettings_Load( object sender, EventArgs e )
    {
      // get them for enumeration
      m_checks.Clear( );
      m_checks.Add( chkHidden01 ); m_checks.Add( chkHidden02 ); m_checks.Add( chkHidden03 ); m_checks.Add( chkHidden04 ); m_checks.Add( chkHidden05 ); m_checks.Add( chkHidden06 );
      m_checks.Add( chkHidden07 ); m_checks.Add( chkHidden08 ); m_checks.Add( chkHidden09 ); m_checks.Add( chkHidden10 ); m_checks.Add( chkHidden11 ); m_checks.Add( chkHidden12 );

      m_labels.Clear( );
      m_labels.Add( lblColor01 ); m_labels.Add( lblColor02 ); m_labels.Add( lblColor03 ); m_labels.Add( lblColor04 ); m_labels.Add( lblColor05 ); m_labels.Add( lblColor06 );
      m_labels.Add( lblColor07 ); m_labels.Add( lblColor08 ); m_labels.Add( lblColor09 ); m_labels.Add( lblColor10 ); m_labels.Add( lblColor11 ); m_labels.Add( lblColor12 );

      // init GUI
      Tx.LocalizeControlTree( this );
      foreach ( var c in m_checks ) c.Text = Tx.Translate( "chkHideXX" );

      chkLbActionMaps.Items.Clear( );
      for ( int i = 0; i < ActionMapsCls.ActionMaps.Length; i++ ) {
        chkLbActionMaps.Items.Add( ActionMapsCls.ActionMaps[i] );
      }

      comboLanguage.Items.Clear( );
      comboLanguage.Items.AddRange( SC.SCUiText.Instance.LanguagesS.ToArray( ) );

      LoadSettings( );
    }


    // Save from app settings into actuals
    private void LoadSettings()
    {
      // SC path
      txSCPath.Text = AppSettings.Instance.UserSCPath;
      cbxUsePath.Checked = AppSettings.Instance.UserSCPathUsed;

      //Ignore Buttons
      txJS1.Text = AppSettings.Instance.IgnoreJS1;
      txJS2.Text = AppSettings.Instance.IgnoreJS2;
      txJS3.Text = AppSettings.Instance.IgnoreJS3;
      txJS4.Text = AppSettings.Instance.IgnoreJS4;
      txJS5.Text = AppSettings.Instance.IgnoreJS5;
      txJS6.Text = AppSettings.Instance.IgnoreJS6;
      txJS7.Text = AppSettings.Instance.IgnoreJS7;
      txJS8.Text = AppSettings.Instance.IgnoreJS8;
      txJS9.Text = AppSettings.Instance.IgnoreJS9;
      txJS10.Text = AppSettings.Instance.IgnoreJS10;
      txJS11.Text = AppSettings.Instance.IgnoreJS11;
      txJS12.Text = AppSettings.Instance.IgnoreJS12;

      // Hidden Joysticks (a comma separated list of numbers to hide)
      for ( int i = 0; i < m_checks.Count; i++ ) {
        m_checks[i].Checked = AppSettings.Instance.JSnHide.Contains( i.ToString( "D2" ) );
      }

      // JS Tab Colors
      string[] e = AppSettings.Instance.JSnColor.Split( new char[] { ',' } );
      for ( int i = 0; i < m_labels.Count; i++ ) {
        if ( i < e.Length ) { 
          if ( int.TryParse( e[i], out int colInt ) ) { 
            m_labels[i].BackColor = Color.FromArgb( colInt );
          }
          else {
            //invalid int... , use default
            m_labels[i].BackColor = MyColors.TabColor[i];
          }
        }
        else {
          // no color found, use default
          m_labels[i].BackColor = MyColors.TabColor[i];
        }
      }


      // Ignore actionmaps
      for ( int i = 0; i < chkLbActionMaps.Items.Count; i++ ) {
        if ( AppSettings.Instance.IgnoreActionmaps.Contains( "," + chkLbActionMaps.Items[i].ToString( ) + "," ) ) {
          chkLbActionMaps.SetItemChecked( i, true );
        }
        else {
          chkLbActionMaps.SetItemChecked( i, false ); // 20161223: fix checked items and Canceled
        }
      }

      // DetectGamepad
      cbxDetectGamepad.Checked = AppSettings.Instance.DetectGamepad;

      // Use PTU
      cbxPTU.Checked = AppSettings.Instance.UsePTU;

      // AutoTabXML
      cbxAutoTabXML.Checked = AppSettings.Instance.AutoTabXML;

      // Use CSV Listing
      cbxCSVListing.Checked = AppSettings.Instance.UseCSVListing;
      cbxListModifiers.Checked = AppSettings.Instance.ListModifiers;

      // Language
      comboLanguage.SelectedItem = AppSettings.Instance.UseLanguage;
      cbxTreeTips.Checked = AppSettings.Instance.ShowTreeTips;
    }


    // Save the current settings
    private void SaveSettings()
    {
      // SC path
      AppSettings.Instance.UserSCPath = txSCPath.Text;
      AppSettings.Instance.UserSCPathUsed = cbxUsePath.Checked;

      //Ignore Buttons
      AppSettings.Instance.IgnoreJS1 = txJS1.Text;
      AppSettings.Instance.IgnoreJS2 = txJS2.Text;
      AppSettings.Instance.IgnoreJS3 = txJS3.Text;
      AppSettings.Instance.IgnoreJS4 = txJS4.Text;
      AppSettings.Instance.IgnoreJS5 = txJS5.Text;
      AppSettings.Instance.IgnoreJS6 = txJS6.Text;
      AppSettings.Instance.IgnoreJS7 = txJS7.Text;
      AppSettings.Instance.IgnoreJS8 = txJS8.Text;
      AppSettings.Instance.IgnoreJS9 = txJS9.Text;
      AppSettings.Instance.IgnoreJS10 = txJS10.Text;
      AppSettings.Instance.IgnoreJS11 = txJS11.Text;
      AppSettings.Instance.IgnoreJS12 = txJS12.Text;

      // Hidden Joysticks (a comma separated list of numbers to hide)
      AppSettings.Instance.JSnHide = "";
      for ( int i = 0; i < m_checks.Count; i++ ) {
        AppSettings.Instance.JSnHide += ( m_checks[i].Checked ) ? i.ToString( "D2" ) : "";
        AppSettings.Instance.JSnHide += ",";
      }

      // JS Tab Colors
      AppSettings.Instance.JSnColor = "";
      for ( int i = 0; i < m_labels.Count; i++ ) {
        AppSettings.Instance.JSnColor += m_labels[i].BackColor.ToArgb().ToString( );
        AppSettings.Instance.JSnColor += ",";
      }



      // Ignore actionmaps
      string ignore = ",";
      for ( int i = 0; i < chkLbActionMaps.Items.Count; i++ ) {
        if ( chkLbActionMaps.GetItemCheckState( i ) == CheckState.Checked ) {
          ignore += chkLbActionMaps.Items[i].ToString( ) + ",";
        }
      }
      AppSettings.Instance.IgnoreActionmaps = ignore;

      // DetectGamepad
      if ( AppSettings.Instance.DetectGamepad != cbxDetectGamepad.Checked ) {
        MessageBox.Show( Tx.Translate( "setNote1" ), Tx.Translate( "setNote2" ), MessageBoxButtons.OK, MessageBoxIcon.Information );
      }
      AppSettings.Instance.DetectGamepad = cbxDetectGamepad.Checked;

      //// Use PTU
      if ( AppSettings.Instance.UsePTU != cbxPTU.Checked ) {
        MessageBox.Show( "Changing to / from PTU folders needs a restart of the application !!", "Settings Notification", MessageBoxButtons.OK, MessageBoxIcon.Information );
      }
      AppSettings.Instance.UsePTU = cbxPTU.Checked;

      // AutoTabXML
      AppSettings.Instance.AutoTabXML = cbxAutoTabXML.Checked;

      // Use CSV Listing
      AppSettings.Instance.UseCSVListing = cbxCSVListing.Checked;
      AppSettings.Instance.ListModifiers = cbxListModifiers.Checked;

      // Language
      AppSettings.Instance.UseLanguage = (string)comboLanguage.SelectedItem;
      AppSettings.Instance.ShowTreeTips = cbxTreeTips.Checked;

      AppSettings.Instance.Save( );
    }


    private void btDone_Click( object sender, EventArgs e )
    {
      if ( cbxUsePath.Checked ) {
        string issue = SC.SCPath.CheckSCBasePath( fbDlg.SelectedPath );
        if ( !string.IsNullOrEmpty( issue ) ) {
          MessageBox.Show( this, issue, Tx.Translate( "setMsgBox" ), MessageBoxButtons.OK );
          return;
        }
      }

      SaveSettings( );
      Canceled = false;
      this.Hide( );
    }

    private void btCancel_Click( object sender, EventArgs e )
    {
      LoadSettings( );
      Canceled = true;
      this.Hide( );
    }

    // allow only numbers, blanks and del
    private bool nonNumberEntered = false;

    private void txJS1_KeyDown( object sender, KeyEventArgs e )
    {
      nonNumberEntered = false;
      // Determine whether the keystroke is a number from the top of the keyboard.
      if ( e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9 ) {
        if ( e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9 ) {
          if ( e.KeyCode != Keys.Back ) {
            if ( e.KeyCode != Keys.Space ) {
              nonNumberEntered = true;
            }
          }
        }
      }
      //If shift key was pressed, it's not a number.
      if ( Control.ModifierKeys == Keys.Shift ) {
        nonNumberEntered = true;
      }
    }

    private void txJS1_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( nonNumberEntered == true ) {
        // Stop the character from being entered into the control since it is non-numerical.
        e.Handled = true;
      }
    }

    private void FormSettings_FormClosing( object sender, FormClosingEventArgs e )
    {
      SaveSettings( );
      if ( e.CloseReason == CloseReason.UserClosing ) {
        e.Cancel = true;
        this.Hide( );
      }
    }

    private void btChooseSCDir_Click( object sender, EventArgs e )
    {
      fbDlg.SelectedPath = txSCPath.Text;
      if ( fbDlg.ShowDialog( this ) == DialogResult.OK ) {
        txSCPath.Text = fbDlg.SelectedPath;

        string issue = SC.SCPath.CheckSCBasePath( fbDlg.SelectedPath );
        if ( !string.IsNullOrEmpty( issue ) ) {
          MessageBox.Show( this, issue, Tx.Translate( "setMsgBox" ), MessageBoxButtons.OK );
        }
      }
    }

    private void comboLanguage_SelectedIndexChanged( object sender, EventArgs e )
    {

    }

    private void cbxUsePath_CheckedChanged( object sender, EventArgs e )
    {
      if ( cbxUsePath.Checked ) {
        string issue = SC.SCPath.CheckSCBasePath( fbDlg.SelectedPath );
        if ( !string.IsNullOrEmpty( issue ) ) {
          MessageBox.Show( this, issue, Tx.Translate( "setMsgBox" ), MessageBoxButtons.OK );
        }
      }
    }

    private void lblColor01_Click( object sender, EventArgs e )
    {
      colDlg.Color = ( sender as Label ).BackColor;
      if ( colDlg.ShowDialog( this ) == DialogResult.OK ) {
        ( sender as Label ).BackColor = colDlg.Color;
      }
    }
  }
}
