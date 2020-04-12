using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SharpDX.DirectInput;

namespace SCJMapper_V2.Devices.Monitor
{
  /// <summary>
  /// Monitors the DirectX devices and reports Events to update the caller
  /// </summary>
  public class DeviceMonitoring
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( MethodBase.GetCurrentMethod( ).DeclaringType );

    public event EventHandler<DxDeviceEventArgs> DxDeviceEvent;
    // call when the items are known.
    private void DeviceStateUpdated( DxDeviceStates action )
    {
      if ( m_reportEvents ) {
        DxDeviceEvent?.Invoke( this, new DxDeviceEventArgs( action ) );
      }
    }


    private Thread m_monitoringThread = null;

    // Thread polling
    private static int m_pollInterval_ms = 50; //**** Take care...
    public int PollIntervall
    {
      get => m_pollInterval_ms;
      set {
        m_pollInterval_ms = value;
        m_pollInterval_ms = ( m_pollInterval_ms < 10 ) ? 10 : m_pollInterval_ms; // limit minimum to 10 ms
      }
    }

    private static bool m_reportEvents = false;
    public bool ReportEvents
    {
      get => m_reportEvents;
      set {
        m_reportEvents = value;
        if ( m_reportEvents ) {
          DeviceInst.GamepadRef?.Activate( );
          DeviceInst.KeyboardRef?.Activate( );
          DeviceInst.MouseRef?.Activate( );
          DeviceInst.JoystickListRef.Activate( );
        }
        else {
          DeviceInst.GamepadRef?.Deactivate( );
          DeviceInst.KeyboardRef?.Deactivate( );
          DeviceInst.MouseRef?.Deactivate( );
          DeviceInst.JoystickListRef.Deactivate( );
        }
      }
    }

    /// <summary>
    /// cTor: Enable continuous monitoring
    /// </summary>
    public DeviceMonitoring()
    {
      log.Debug( "DeviceMonitoring.cTor - Entry" );

      // save the Activated States
      DeviceInst.GamepadRef?.PushActiveState( );
      DeviceInst.KeyboardRef?.PushActiveState( );
      DeviceInst.MouseRef?.PushActiveState( );
      DeviceInst.JoystickListRef.PushActiveState( );

      var dxMonitorThread = new DxMonitorThread( this );
      m_monitoringThread = new Thread( dxMonitorThread.Run );
      m_monitoringThread.Start( );

    }

    public void ShutMonitoring()
    {
      m_reportEvents = false;

      // recover the Activated States
      DeviceInst.GamepadRef?.PopActiveState( );
      DeviceInst.KeyboardRef?.PopActiveState( );
      DeviceInst.MouseRef?.PopActiveState( );
      DeviceInst.JoystickListRef.PopActiveState( );

      m_monitoringThread?.Abort( );
    }

    public class DxDeviceReport
    {
      public string Input = "";
      public bool IsAxis = false; // true if an Axis is reported
      public bool Pressed = false; // true if activated (buttons, keys)
    }

    /// <summary>
    /// Reporting structure for DX devices
    /// </summary>
    public class DxDeviceStates
    {
      // Keyboard
      public DxDeviceReport KeyboardIn = new DxDeviceReport( ) { Input = "", IsAxis = false };
      // Mouse
      public DxDeviceReport MouseIn = new DxDeviceReport( ) { Input = "", IsAxis = false };
      // Gamepad
      public DxDeviceReport GamepadIn = new DxDeviceReport( ) { Input = "", IsAxis = false };
      // Joysticks
      public DxDeviceReport[] JoystickIn = new DxDeviceReport[12] { new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
                                                                    new DxDeviceReport(){ Input="", IsAxis=false },
      };

      public string Modifier = "";  // mod from Kbd


      // state management
      string m_prevKbdMod = string.Empty;
      string m_prevKbd = string.Empty;
      bool m_kbdEmpty = false;

      string m_prevMouse = string.Empty;
      string m_prevMouseMod = string.Empty;
      bool m_mouseEmpty = false;

      string m_prevGamepad = string.Empty;
      string m_prevGamepadMod = string.Empty;
      bool m_gamepadEmpty = false;

      string[] m_prevJoystick = new string[12] { "", "", "", "", "", "", "", "", "", "", "", "" };
      string[] m_prevJoystickMod = new string[12] { "", "", "", "", "", "", "", "", "", "", "", "" };
      bool[] m_joystickEmpty = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };
      int m_timer = 3000 / m_pollInterval_ms;// 3sec

      /// <summary>
      /// This should assign a new input or an empty string for each item 
      /// </summary>
      public void Update()
      {
        bool anyInput = false;

        // keyboard input will be empty on release where other stay on their last input...
        Modifier = DeviceInst.KeyboardRef?.GetLastChange( false ); // mod only
        if ( !string.IsNullOrEmpty( Modifier ) ) {
          Modifier += "+";
        }

        string input = string.Empty;

        KeyboardIn.Input = string.Empty;
        input = DeviceInst.KeyboardRef?.GetCurrentInput( ); // key only
        KeyboardIn.Pressed = false; // to start with
        if ( !string.IsNullOrEmpty( input ) ) {
          KeyboardIn.Pressed = true;
          // still pressed or newly pressed
          if ( m_prevKbd == input ) {
            // still pressed
            if ( ( m_prevKbdMod != Modifier ) || m_kbdEmpty ) {
              // but mod changed; or newly the same- report
              KeyboardIn.Input = KeyboardCls.DevInput( Modifier + input );
              m_prevKbdMod = Modifier;
              anyInput = true;
            }
          }
          else {
            // new input
            KeyboardIn.Input = KeyboardCls.DevInput( Modifier + input );
            m_prevKbd = input;
            m_prevKbdMod = Modifier;
            anyInput = true;
          }
        }
        m_kbdEmpty = string.IsNullOrEmpty( input );

        MouseIn.Input = string.Empty;
        input = DeviceInst.MouseRef?.GetCurrentInput( );
        MouseIn.Pressed = false;
        if ( !string.IsNullOrEmpty( input ) ) {
          MouseIn.Pressed = true;
          // still pressed or newly pressed
          if ( m_prevMouse == input ) {
            // still pressed
            if ( ( m_prevMouseMod != Modifier ) || m_mouseEmpty ) {
              // but mod changed - report
              MouseIn.Input = MouseCls.DevInput( Modifier + input );
              m_prevMouseMod = Modifier;
              anyInput = true;
            }
          }
          else {
            // new input
            MouseIn.Input = MouseCls.DevInput( Modifier + input );
            m_prevMouse = input;
            m_prevMouseMod = Modifier;
            MouseIn.IsAxis = MouseCls.IsAxisCommand( MouseIn.Input );
            anyInput = true;
          }
        }
        m_mouseEmpty = string.IsNullOrEmpty( input );

        GamepadIn.Input = string.Empty;
        input = DeviceInst.GamepadRef?.GetCurrentInput( );
        GamepadIn.Pressed = false;
        if ( !string.IsNullOrEmpty( input ) ) {
          GamepadIn.Pressed = true;
          // still pressed or newly pressed
          if ( m_prevGamepad == input ) {
            // still pressed
            if ( ( m_prevGamepadMod != Modifier ) || m_gamepadEmpty ) {
              // but mod changed - report
              GamepadIn.Input = GamepadCls.DevInput( Modifier + input );
              m_prevGamepadMod = Modifier;
              anyInput = true;
            }
          }
          else {
            // new input
            GamepadIn.Input = GamepadCls.DevInput( Modifier + input );
            m_prevGamepad = input;
            m_prevGamepadMod = Modifier;
            GamepadIn.IsAxis = GamepadCls.IsAxisCommand( GamepadIn.Input );
            anyInput = true;
          }
        }
        m_gamepadEmpty = string.IsNullOrEmpty( input );

        // 
        foreach ( var js in DeviceInst.JoystickListRef ) {
          JoystickIn[js.DevInstance].Input = string.Empty; // indicates no change
          input = js.GetCurrentInput( );   // we get either a code or an empty string if released
          JoystickIn[js.DevInstance].Pressed = false;
          if ( !string.IsNullOrEmpty( input ) ) {
            JoystickIn[js.DevInstance].Pressed = true;
            // still pressed or newly pressed
            if ( m_prevJoystick[js.DevInstance] == input ) {
              // still pressed
              if ( ( m_prevJoystickMod[js.DevInstance] != Modifier ) || m_joystickEmpty[js.DevInstance] ) {
                // but mod changed - report
                JoystickIn[js.DevInstance].Input = JoystickCls.DevInput( Modifier + input, js.JSAssignment );
                m_prevJoystickMod[js.DevInstance] = Modifier;
                anyInput = true;
              }
            }
            else {
              // new input
              JoystickIn[js.DevInstance].Input = JoystickCls.DevInput( Modifier + input, js.JSAssignment );
              m_prevJoystick[js.DevInstance] = input;
              m_prevJoystickMod[js.DevInstance] = Modifier;
              JoystickIn[js.DevInstance].IsAxis = JoystickCls.IsAxisCommand( JoystickIn[js.DevInstance].Input );
              anyInput = true;
            }
          }
          m_joystickEmpty[js.DevInstance] = string.IsNullOrEmpty( input );
        }

        if ( anyInput ) {
          m_timer = 3000 / m_pollInterval_ms; // for any Input - wait again 3sec before sending modifier alone as input 
        }
        else {
          m_timer = ( m_timer < 0 ) ? 0 : m_timer - 1; // decrement and hold at 0
          // check if it is time to send the modifier as input
          if ( !string.IsNullOrEmpty( Modifier ) && ( m_timer <= 0 ) )
            KeyboardIn.Input = KeyboardCls.DevInput( DeviceInst.KeyboardRef?.GetLastChange( false ) );
        }

      }
    }


    private static DxDeviceStates m_deviceStates = new DxDeviceStates( );
    public DxDeviceStates GetState { get => m_deviceStates; }


    class DxMonitorThread
    {
      private DeviceMonitoring m_context = null;
      public DxMonitorThread( DeviceMonitoring context )
      {
        m_context = context;
      }
      /// <summary>
      /// Thread routine to scan DxDevices
      /// </summary>
      public void Run()
      {
        while ( true ) {
          if ( m_reportEvents ) {
            if ( ( DeviceInst.KeyboardRef != null ) && DeviceInst.KeyboardRef.Activated ) DeviceInst.KeyboardRef.GetData( );
            if ( ( DeviceInst.MouseRef != null ) && DeviceInst.MouseRef.Activated ) DeviceInst.MouseRef.GetData( );
            if ( ( DeviceInst.GamepadRef != null ) && DeviceInst.GamepadRef.Activated ) DeviceInst.GamepadRef.GetData( );
            foreach ( var js in DeviceInst.JoystickListRef ) {
              if ( js.Activated ) js.GetData( );
            }
            // fill device states
            m_deviceStates.Update( );
            m_context?.DeviceStateUpdated( m_deviceStates );
          }

          try {
            Thread.Sleep( m_pollInterval_ms );
          }
          catch {
            break;
          }
        }
      }
    }

  }
}
