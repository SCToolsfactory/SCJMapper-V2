using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCJMapper_V2.Actions;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Translation;

namespace SCJMapper_V2.Devices.Monitor
{
  /// <summary>
  /// Allows continuos monitoring of Device Events
  /// NOTE: Disable any other device reporting while this form is open..
  /// </summary>
  internal partial class FormDeviceMonitor : Form
  {
    private bool m_monitor = false;
    private DeviceMonitoring m_dx = null;
    private ActionTree m_atRef = null;
    private DeviceMonitoring.DxDeviceStates m_prevStates = new DeviceMonitoring.DxDeviceStates( );
    private List<TextBox> m_jTx = new List<TextBox>( );

    public ActionTree ActionTree { set => m_atRef = value; }

    public FormDeviceMonitor()
    {
      InitializeComponent( );
    }



    private void FormDeviceMonitor_Load( object sender, EventArgs e )
    {
      Tx.LocalizeControlTree( this );
      lblKeyboard.Text = Tx.Translate( "xKeyboard" );
      lblMouse.Text = Tx.Translate( "xMouse" );
      lblGamepad.Text = Tx.Translate( "xGamepad" );
      lblJ_00.Text = Tx.Translate( "xJoystick" ) + "-0";
      lblJ_01.Text = Tx.Translate( "xJoystick" ) + "-1";
      lblJ_02.Text = Tx.Translate( "xJoystick" ) + "-2";
      lblJ_03.Text = Tx.Translate( "xJoystick" ) + "-3";
      lblJ_04.Text = Tx.Translate( "xJoystick" ) + "-4";
      lblJ_05.Text = Tx.Translate( "xJoystick" ) + "-5";
      lblJ_06.Text = Tx.Translate( "xJoystick" ) + "-6";
      lblJ_07.Text = Tx.Translate( "xJoystick" ) + "-7";
      lblJ_08.Text = Tx.Translate( "xJoystick" ) + "-8";
      lblJ_09.Text = Tx.Translate( "xJoystick" ) + "-9";
      lblJ_10.Text = Tx.Translate( "xJoystick" ) + "-10";
      lblJ_11.Text = Tx.Translate( "xJoystick" ) + "-11";

      // get enumerated access for the joy text boxes
      m_jTx.Add( txJoystick00 ); m_jTx.Add( txJoystick01 ); m_jTx.Add( txJoystick02 ); m_jTx.Add( txJoystick03 ); m_jTx.Add( txJoystick04 ); m_jTx.Add( txJoystick05 );
      m_jTx.Add( txJoystick06 ); m_jTx.Add( txJoystick07 ); m_jTx.Add( txJoystick08 ); m_jTx.Add( txJoystick09 ); m_jTx.Add( txJoystick10 ); m_jTx.Add( txJoystick11 );

      // start with List
      chkReport.Checked = true;

      // Dx stuff
      m_dx = new DeviceMonitoring( );
      m_dx.DxDeviceEvent += M_dx_DxDeviceEvent;
      // start with monitoring OFF
      m_monitor = false;
      UpdateMonitor( );
    }

    private Queue<DeviceMonitoring.DxDeviceStates> m_dxQueue = new Queue<DeviceMonitoring.DxDeviceStates>( );

    // can be called from non UI thread
    private void M_dx_DxDeviceEvent( object sender, DxDeviceEventArgs e )
    {
      m_dxQueue.Enqueue( e.DeviceStates );
      this.Invoke( (MethodInvoker)delegate {
        UpdateDeviceLabels( );
      } );
    }

    private void FormDeviceMonitor_FormClosing( object sender, FormClosingEventArgs e )
    {
      m_dx?.ShutMonitoring( );
      m_dx = null;
      Thread.Sleep( 200 ); // should allow to get all settled..
    }

    private void btTogMonitor_Click( object sender, EventArgs e )
    {
      m_monitor = !m_monitor;
      UpdateMonitor( );
    }


    private void UpdateMonitor()
    {
      if ( m_monitor ) {
        btTogMonitor.ImageKey = "Monitoring";
        m_dx.ReportEvents = true;
      }
      else {
        btTogMonitor.ImageKey = "NotMonitoring";
        m_dx.ReportEvents = false;
      }
    }

    /// <summary>
    /// Update the GUI elements in the Form Thread
    /// </summary>
    private void UpdateDeviceLabels()
    {
      // process all received events
      while ( m_dxQueue.Count > 0 ) {
        var states = m_dxQueue.Dequeue( );

        // Keyboard
        if ( !string.IsNullOrEmpty( states.KeyboardIn.Input ) ) {
          txKeyboard.Text = states.KeyboardIn.Input;
          UpdateAssignmentList( states.KeyboardIn.Input );
        }
        else {
        //  txKeyboard.Text = "";
        }
        // Mouse
        if ( !string.IsNullOrEmpty( states.MouseIn.Input ) ) {
          if ( chkMonitorMouse.Checked ) {
            txMouse.Text = states.MouseIn.Input;
            UpdateAssignmentList( states.MouseIn.Input );
          }
          else {

            if ( states.MouseIn.IsAxis == false ) {
              // only non axis..
              txMouse.Text = states.MouseIn.Input;
              UpdateAssignmentList( states.MouseIn.Input );
            }
          }
        }
        else {
         // txMouse.Text = "";
        }
        // Gamepad
        if ( !string.IsNullOrEmpty( states.GamepadIn.Input ) ) {
          txGamepad.Text = states.GamepadIn.Input;
          UpdateAssignmentList( states.GamepadIn.Input );
        }
        else {
         // txGamepad.Text = "";
        }

        // all Joysticks
        for ( int jsDev = 0; jsDev < m_jTx.Count; jsDev++ ) {
          if ( !string.IsNullOrEmpty( states.JoystickIn[jsDev].Input ) ) {
            m_jTx[jsDev].Text = states.JoystickIn[jsDev].Input;
            UpdateAssignmentList( states.JoystickIn[jsDev].Input );
          }
          else {
           // m_jTx[jsDev].Text = "";
          }
        }
      }
    }

    private void UpdateAssignmentList( string devInput )
    {
      if ( string.IsNullOrEmpty( devInput ) ) return;

      var actions = m_atRef.GetAllActions( devInput );

      if ( chkReport.Checked ) {
        // show list
        var RTF = new RTF.RTFformatter { RColor = SCJMapper_V2.RTF.RTFformatter.ERColor.ERC_Gainsborow };
        m_atRef.ListAllActionsRTF( devInput, RTF, true );
        // have to check if throttle is used and if - add those to the list
        string altDevInput = JoystickCls.MakeThrottle( devInput, true );
        if ( altDevInput != devInput ) {
          m_atRef.ListAllActionsRTF( altDevInput, RTF, true );
        }
        lbxOther.Rtf = RTF.RTFtext;
      }
      else {
        lbxOther.Text = "";
      }

    }

  }
}
