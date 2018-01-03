using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCJMapper_V2.Devices;

namespace SCJMapper_V2.Table
{
  public partial class FormTable : Form
  {
    public FormTable()
    {
      InitializeComponent( );

      DS_AMaps = new DS_ActionMaps( ); // init once for the lifetime of the App
      m_bSrc.DataSource = DS_AMaps;
      m_bSrc.DataMember = "T_Action";

      DGV.AutoGenerateColumns = true;
      DGV.DataSource = m_bSrc;
      DGV.MultiSelect = false;
      DGV.Columns["ID_Action"].Visible = false;
      DGV.AutoResizeColumns( DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader );

      // handle Edits (Blended Col is only allowed to be edited by the user)
      btUpdateFromEdit.Enabled = chkEditBlend.Checked;
      DGV.Columns["Usr_Binding"].ReadOnly = true;
      DGV.Columns["Usr_Modifier"].ReadOnly = true;
      DGV.Columns["ActionName"].Visible = false;
      DGV.ReadOnly = !chkEditBlend.Checked;
    }



    public event EventHandler<EditRowEventArgs> EditActionEvent;
    private void RaiseEditActionEvent( string actionmap, string actionkey, int nodeindex )
    {
      if ( EditActionEvent != null ) {
        EditActionEvent( this, new EditRowEventArgs( actionmap, actionkey, nodeindex ) );
      }
    }

    public event EventHandler<UpdateEditEventArgs> UpdateEditEvent;
    private void RaiseUpdateEditEvent()
    {
      if ( UpdateEditEvent != null ) {
        UpdateEditEvent( this, new UpdateEditEventArgs( ) );
      }
    }


    public Size LastSize { get; set; }
    public Point LastLocation { get; set; }
    public string LastColSize { get; set; }

    private BindingSource m_bSrc = new BindingSource( );

    /// <summary>
    /// Assign or retrieve the underlying DataSet
    /// </summary>
    public DS_ActionMaps DS_AMaps { get; private set; }



    public void SuspendDGV()
    {
      // add things to improve speed while re-loading the DataSet outside - nothing found so far
    }

    public void ResumeDGV()
    {
      // finish things to improve speed while re-loading the DataSet outside - nothing found so far
    }


    /// <summary>
    /// Populate the view from the dataset
    /// </summary>
    public void Populate()
    {
      //      DGV.SuspendLayout( );

      if ( !string.IsNullOrEmpty( LastColSize ) ) {
        string[] e = LastColSize.Split( new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
        for ( int i = 0; i < e.Length; i++ ) {
          if ( i < DGV.Columns.Count )
            DGV.Columns[i].Width = int.Parse( e[i] );
        }
      }
      DGV.AllowUserToResizeColumns = true;
      ComposeFilter( );
      //    DGV.ResumeLayout( );
    }

    /// <summary>
    /// Update a single row with given actionID (from DS_ActionMap)
    ///   Note: the dataset needs to be updated by the caller before
    /// </summary>
    /// <param name="actionID">ActionID of the row to update</param>
    public void UpdateRow( string actionID )
    {
      if ( string.IsNullOrEmpty( actionID ) ) return; // nothing to do
    }


    /// <summary>
    /// A row is set to be edited 
    ///   resolve ID and callback to owner 
    /// </summary>
    /// <param name="rowIndex"></param>
    private void EditRow( int rowIndex )
    {
      string id = DGV.Rows[rowIndex].Cells["ID_Action"].Value.ToString( );
      // we have nn-actionmap.actionkey.nodeindex
      string actionMap = DS_ActionMap.ActionMap( id );
      string actionKey = DS_ActionMap.ActionKey( id );
      int nodeIndex = DS_ActionMap.ActionCommandIndex( id );

      RaiseEditActionEvent( actionMap, actionKey, nodeIndex );
    }





    private void ComposeFilter()
    {
      // make sure we only add parts that are really used - else it is using too much time to resolve '*'
      string filter = "";
      if ( !string.IsNullOrEmpty( txFilterAction.Text ) ) {
        filter += string.Format( "(ActionName LIKE '*{0}*')", txFilterAction.Text );
      }
      if ( !string.IsNullOrEmpty( txFilterDefBinding.Text ) ) {
        if ( !string.IsNullOrEmpty( filter ) ) filter += " AND ";
        filter += string.Format( "(Def_Binding LIKE '*{0}*')", txFilterDefBinding.Text );
      }
      if ( !string.IsNullOrEmpty( txFilterUsrBinding.Text ) ) {
        if ( !string.IsNullOrEmpty( filter ) ) filter += " AND ";
        filter += string.Format( "(Usr_Binding LIKE '*{0}*')", txFilterUsrBinding.Text );
      }

      string deviceFilter = "";
      if ( ( chkJoystick.Checked == false ) && ( chkGamepad.Checked == false ) && ( chkMouse.Checked == false ) && ( chkKbd.Checked == false ) ) {
        // none checked means all
      }
      else {
        deviceFilter = "( Device='X'"
        + ( ( chkJoystick.Checked ) ? string.Format( " OR Device = 'joystick'" ) : "" )
        + ( ( chkGamepad.Checked ) ? string.Format( " OR Device = 'xboxpad'" ) : "" )
        + ( ( chkMouse.Checked ) ? string.Format( " OR Device = 'mouse'" ) : "" )
        + ( ( chkKbd.Checked ) ? string.Format( " OR Device = 'keyboard'" ) : "" )
        + " )";
      }

      if ( !string.IsNullOrEmpty( deviceFilter ) ) {
        if ( !string.IsNullOrEmpty( filter ) ) filter += " AND ";
        filter += deviceFilter;
      }

      m_bSrc.Filter = filter;
    }



    private void txFilterAction_TextChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }

    private void txFilterBinding_TextChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }

    private void txFilterUsrBinding_TextChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }


    private void btClrFilterAction_Click( object sender, EventArgs e )
    {
      txFilterAction.Text = "";
    }

    private void btClrFilterBinding_Click( object sender, EventArgs e )
    {
      txFilterDefBinding.Text = "";
    }

    private void btClrFilterUsrBinding_Click( object sender, EventArgs e )
    {
      txFilterUsrBinding.Text = "";
    }


    private void chkJoystick_CheckedChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }

    private void chkGamepad_CheckedChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }

    private void chkMouse_CheckedChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }

    private void chkKbd_CheckedChanged( object sender, EventArgs e )
    {
      ComposeFilter( );
    }

    private void FormTable_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( e.CloseReason == CloseReason.UserClosing ) {

        this.Hide( );
        e.Cancel = true;
      }
    }


    private void FormTable_LocationChanged( object sender, EventArgs e )
    {
      if ( this.WindowState == FormWindowState.Normal )
        LastLocation = this.Location;
    }

    private void FormTable_SizeChanged( object sender, EventArgs e )
    {
      if ( this.WindowState == FormWindowState.Normal )
        LastSize = this.Size;
    }

    private void DGV_ColumnWidthChanged( object sender, DataGridViewColumnEventArgs e )
    {
      if ( this.WindowState == FormWindowState.Normal ) {
        string setting = "";
        foreach ( DataGridViewColumn col in DGV.Columns ) {
          setting += string.Format( "{0};", col.Width.ToString( ) );
        }
        LastColSize = setting;
      }
    }



    private void DGV_RowHeaderMouseClick( object sender, DataGridViewCellMouseEventArgs e )
    {
      EditRow( e.RowIndex );
    }


    private void DGV_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e )
    {
      if ( e.ColumnIndex < 0 && e.RowIndex < 0 ) m_bSrc.Sort = ""; // revert any sorting
    }


    private void DGV_CellMouseDoubleClick( object sender, DataGridViewCellMouseEventArgs e )
    {
      if ( e.RowIndex < 0 ) return; // would be the header row

      EditRow( e.RowIndex );
    }


    private void btUpdateFromEdit_Click( object sender, EventArgs e )
    {
      RaiseUpdateEditEvent( ); // accept will be done after updating the main tree
    }

    private void chkEditBlend_CheckedChanged( object sender, EventArgs e )
    {
      // toggle the Edit mode of the Blend Column
      btUpdateFromEdit.Enabled = chkEditBlend.Checked;
      btBlendAll.Enabled = chkEditBlend.Checked;

      DGV.ReadOnly = !chkEditBlend.Checked;
    }

    private void btCancelEdit_Click( object sender, EventArgs e )
    {
      // Undo Edits so far
      DS_AMaps.RejectChanges( );
    }


    private void btBlendAll_Click( object sender, EventArgs e )
    {
      if ( !chkEditBlend.Checked ) return; // only if Edit is allowed

      foreach ( DataGridViewRow row in DGV.Rows ) {
        if ( string.IsNullOrEmpty( (string)row.Cells[DGV.Columns["Usr_Binding"].Index].Value ) )
          row.Cells[DGV.Columns["Disabled"].Index].Value = true;
      }
    }


    private void DGV_CurrentCellDirtyStateChanged( object sender, EventArgs e )
    {
      // Immediately handle changes of the Blended Checkbox - workaround the regular DGV behavior
      if ( !DGV.IsCurrentCellDirty ) return;
      if ( DGV.CurrentCell.ColumnIndex != DGV.Columns["Disabled"].Index ) return;

      // only if dirty and the Blended column ..
      // it still has it's previous value i.e. inverse the logic here - commit the value imediately
      if ( (bool)DGV.Rows[DGV.CurrentCell.RowIndex].Cells[DGV.Columns["Disabled"].Index].Value == false ) {
        DGV.Rows[DGV.CurrentCell.RowIndex].Cells[DGV.Columns["Disabled"].Index].Value = true; // toggle value - triggers the ValueChanged Event below
      }
      else {
        DGV.Rows[DGV.CurrentCell.RowIndex].Cells[DGV.Columns["Disabled"].Index].Value = false;
      }
      DGV.NotifyCurrentCellDirty( false ); // have set the value - so set not dirty anymore
    }

    private void DGV_CellValueChanged( object sender, DataGridViewCellEventArgs e )
    {
      // set the Usr_Binding only if Blended column items Changes
      if ( e.ColumnIndex != DGV.Columns["Disabled"].Index ) return;

      if ( (bool)DGV.Rows[e.RowIndex].Cells[DGV.Columns["Disabled"].Index].Value == true )
        DGV.Rows[e.RowIndex].Cells[DGV.Columns["Usr_Binding"].Index].Value = DeviceCls.DisabledInput;
      else
        DGV.Rows[e.RowIndex].Cells[DGV.Columns["Usr_Binding"].Index].Value = ""; // don't know anything else...

    }

  }//class


}
