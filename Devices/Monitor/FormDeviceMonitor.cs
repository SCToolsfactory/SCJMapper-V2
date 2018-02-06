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

    private class TogWrap
    {
      public UC_Toggle Toggle = null;
      public string Cmd = "";
      public string Label = "";

      public TogWrap( UC_Toggle tog, string cmd, string lbl )
      {
        Toggle = tog;
        Cmd = cmd;
        Label = lbl;
        Toggle.Label = Label;
      }
    }

    private Dictionary<string, TogWrap> m_toggles = new Dictionary<string, TogWrap>( );

    private enum Togs
    {
      Freelook
    }

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

      // get Toggles
      var t = new TogWrap( uC_Toggle1, "v_view_freelook_mode", "Freelook" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle2, "v_ifcs_toggle_esp", "ESP" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle3, "v_toggle_landing_system", "Landing System" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle4, "v_toggle_weapon_gimbal_lock", "Gimbal Lock Tgt" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle5, "v_target_toggle_pinned_focused", "Pin focused" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle6, "v_power_toggle_group_1", "Pwr Shields" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle7, "v_power_toggle_group_2", "Pwr Weapons" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle8, "v_power_toggle_group_3", "Pwr Drive" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle9, "v_power_toggle", "Power" ); m_toggles.Add( t.Cmd, t );
      t = new TogWrap( uC_Toggle10, "v_toggle_running_lights", "Lights" ); m_toggles.Add( t.Cmd, t );


      m_dx = new DeviceMonitoring( );
      m_dx.DxDeviceEvent += M_dx_DxDeviceEvent;
      // start with monitoring OFF
      m_monitor = false;
      UpdateMonitor( );
    }

    // can be called from non UI thread
    private void M_dx_DxDeviceEvent( object sender, DxDeviceEventArgs e )
    {
      this.Invoke( (MethodInvoker)delegate {
        UpdateDeviceLabels( e.DeviceStates );
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
    private void UpdateDeviceLabels( DeviceMonitoring.DxDeviceStates states )
    {
      if ( !string.IsNullOrEmpty( states.KeyboardIn.Input ) ) {
        txKeyboard.Text = states.KeyboardIn.Input;
        UpdateAssignmentList( states.KeyboardIn.Input );
      }
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
      if ( !string.IsNullOrEmpty( states.GamepadIn.Input ) ) {
        txGamepad.Text = states.GamepadIn.Input;
        UpdateAssignmentList( states.GamepadIn.Input );
      }
      int jsDev = 0;
      if ( !string.IsNullOrEmpty( states.JoystickIn[jsDev].Input ) ) {
        txJoystick00.Text = states.JoystickIn[jsDev].Input;
        UpdateAssignmentList( states.JoystickIn[jsDev].Input );
      }
      jsDev = 1;
      if ( !string.IsNullOrEmpty( states.JoystickIn[jsDev].Input ) ) {
        txJoystick01.Text = states.JoystickIn[jsDev].Input;
        UpdateAssignmentList( states.JoystickIn[jsDev].Input );
      }
      jsDev = 2;
      if ( !string.IsNullOrEmpty( states.JoystickIn[jsDev].Input ) ) {
        txJoystick02.Text = states.JoystickIn[jsDev].Input;
        UpdateAssignmentList( states.JoystickIn[jsDev].Input );
      }
      jsDev = 3;
      if ( !string.IsNullOrEmpty( states.JoystickIn[jsDev].Input ) ) {
        txJoystick03.Text = states.JoystickIn[jsDev].Input;
        UpdateAssignmentList( states.JoystickIn[jsDev].Input );
      }
    }

    private void UpdateAssignmentList( string devInput )
    {
      if ( string.IsNullOrEmpty( devInput ) ) return;

      var actions = m_atRef.GetAllActions( devInput );
      // cheap
      foreach ( var t in m_toggles ) {
        if ( actions.Contains( t.Key ) ) {
          t.Value.Toggle.ToggleState( );
        }
      }
      if ( chkReport.Checked ) {
        // show list
        RTF.RTFformatter RTF = new RTF.RTFformatter { RColor = SCJMapper_V2.RTF.RTFformatter.ERColor.ERC_Gainsborow };
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
