using SharpDX;
using SharpDX.DirectInput;
using System;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  /// <summary>
  /// Handles one JS device as DXInput device
  /// In addition provide some static tools to handle JS props here in one place
  /// Also owns the GUI i.e. the user control that shows all values
  /// </summary>
  class JoystickCls
  {
    private   Joystick m_device;

    private   JoystickState m_state = new JoystickState( );
    private   JoystickState m_prevState = new JoystickState( );

    private   Control m_hwnd;
    private int m_numPOVs = 0;      // static counter for UpdateControls
    private int m_sliderCount = 0;  // static counter for UpdateControls
    private String m_lastItem = "";

    private UC_JoyPanel m_jPanel = null; // the GUI panel


    /// <summary>
    /// Returns a CryEngine compatible hat direction
    /// </summary>
    /// <param name="value">The Hat value</param>
    /// <returns>The direction string</returns>
    private String HatDir( int value )
    {
      // Hats have a 360deg -> 36000 value reporting
      if ( value == 0 ) return "up";
      if ( value == 9000 ) return "right";
      if ( value == 18000 ) return "down";
      if ( value == 27000 ) return "left";
      return "";
    }

    /// <summary>
    /// Returns properly formatted jsn_ string
    /// </summary>
    /// <param name="jsNum">The JS number</param>
    /// <returns>The formatted JS name for the CryEngine XML</returns>
    static public String JSTag( int jsNum )
    {
      return "js" + jsNum.ToString( ) + "_";
    }


    /// <summary>
    /// Extract the JS number from a JS string
    /// </summary>
    /// <param name="jsTag">The JS string</param>
    /// <returns>The JS number</returns>
    static public int JSNum( String jsTag )
    {
      int retNum=0;
      if ( !String.IsNullOrEmpty( jsTag ) ) {
        int.TryParse( jsTag.Substring( 2, 1 ), out retNum );
      }
      return retNum;
    }

    /// <summary>
    /// The povides the JS ProductName property
    /// </summary>
    public String DevName { get { return m_device.Properties.ProductName; } }
    public int AxisCount { get { return m_device.Capabilities.AxeCount; } }
    public int ButtonCount { get { return m_device.Capabilities.ButtonCount; } }
    public int POVCount { get { return m_device.Capabilities.PovCount; } }


    /// <summary>
    /// ctor and init
    /// </summary>
    /// <param name="device">A DXInput device</param>
    /// <param name="hwnd">The WinHandle of the main window</param>
    /// <param name="panel">The respective JS panel to show the properties</param>
    public JoystickCls( Joystick device, Control hwnd, UC_JoyPanel panel )
    {
      m_device = device;
      m_hwnd = hwnd;
      m_jPanel = panel;

      // Set BufferSize in order to use buffered data.
      m_device.Properties.BufferSize = 128;

      m_jPanel.Caption = DevName;
      m_jPanel.nAxis = AxisCount.ToString( );
      m_jPanel.nButtons = ButtonCount.ToString( );
      m_jPanel.nPOVs = POVCount.ToString( );

      // Set the data format to the c_dfDIJoystick pre-defined format.
      //m_device.SetDataFormat( DeviceDataFormat.Joystick );
      // Set the cooperative level for the device.
      m_device.SetCooperativeLevel( m_hwnd, CooperativeLevel.Exclusive | CooperativeLevel.Foreground );
      // Enumerate all the objects on the device.
      foreach ( DeviceObjectInstance d in m_device.GetObjects( ) ) {
        // For axes that are returned, set the DIPROP_RANGE property for the
        // enumerated axis in order to scale min/max values.
        if ( ( 0 != ( d.ObjectId.Flags & DeviceObjectTypeFlags.Axis ) ) ) {
          // Set the range for the axis.
          m_device.Properties.Range = new InputRange( -1000, +1000 );
        }
        // Update the controls to reflect what objects the device supports.
        UpdateControls( d );
      }

    }

    /// <summary>
    /// Shutdown device access
    /// </summary>
    public void FinishDX( )
    {
      if ( null != m_device )
        m_device.Unacquire( );
    }


    /// <summary>
    /// Enable the properties that are supported by the device
    /// </summary>
    /// <param name="d"></param>
    private void UpdateControls( DeviceObjectInstance d )
    {
      // Set the UI to reflect what objects the joystick supports.
      if ( ObjectGuid.XAxis == d.ObjectType ) {
        m_jPanel.Xe = true;
        m_jPanel.Xname = d.Name + ":";
      }
      if ( ObjectGuid.YAxis == d.ObjectType ) {
        m_jPanel.Ye = true;
        m_jPanel.Yname = d.Name + ":";
      }
      if ( ObjectGuid.ZAxis == d.ObjectType ) {
        m_jPanel.Ze = true;
        m_jPanel.Zname = d.Name + ":";
      }
      if ( ObjectGuid.RxAxis == d.ObjectType ) {
        m_jPanel.Xre = true;
        m_jPanel.Xrname = d.Name + ":";
      }
      if ( ObjectGuid.RyAxis == d.ObjectType ) {
        m_jPanel.Yre = true;
        m_jPanel.Yrname = d.Name + ":";
      }
      if ( ObjectGuid.RzAxis == d.ObjectType ) {
        m_jPanel.Zre = true;
        m_jPanel.Zrname = d.Name + ":";
      }
      if ( ObjectGuid.Slider == d.ObjectType ) {
        switch ( m_sliderCount++ ) {
          case 0:
            m_jPanel.S1e = true;
            m_jPanel.S1name = d.Name + ":";
            break;

          case 1:
            m_jPanel.S2e = true;
            m_jPanel.S2name = d.Name + ":";
            break;
        }
      }
      if ( ObjectGuid.PovController == d.ObjectType ) {
        switch ( m_numPOVs++ ) {
          case 0:
            m_jPanel.H1e = true;
            m_jPanel.H1name = d.Name + ":";
            break;

          case 1:
            m_jPanel.H2e = true;
            m_jPanel.H2name = d.Name + ":";
            break;

          case 2:
            m_jPanel.H3e = true;
            m_jPanel.H3name = d.Name + ":";
            break;

          case 3:
            m_jPanel.H4e = true;
            m_jPanel.H4name = d.Name + ":";
            break;
        }
      }
    }

    /// <summary>
    /// Find the last change the user did on that device
    /// </summary>
    /// <returns>The last action as CryEngine compatible string</returns>
    public String GetLastChange( )
    {
      if ( m_state.X != m_prevState.X ) m_lastItem = "x";
      if ( m_state.Y != m_prevState.Y ) m_lastItem = "y";
      if ( m_state.Z != m_prevState.Z ) m_lastItem = "throttlez"; // this is not z because it usually maps the throttle 

      if ( m_state.RotationX != m_prevState.RotationX ) m_lastItem = "rotx";
      if ( m_state.RotationY != m_prevState.RotationY ) m_lastItem = "roty";
      if ( m_state.RotationZ != m_prevState.RotationZ ) m_lastItem = "rotz";

      int[] slider = m_state.Sliders;
      int[] pslider = m_prevState.Sliders;
      if ( slider[0] != pslider[0] ) m_lastItem = "slider1";
      if ( slider[1] != pslider[1] ) m_lastItem = "slider2";

      int[] pov = m_state.PointOfViewControllers;
      int[] ppov = m_prevState.PointOfViewControllers;
      if ( pov[0] >= 0 ) if ( pov[0] != ppov[0] ) m_lastItem = "hat1_" + HatDir( pov[0] );
      if ( pov[1] >= 0 ) if ( pov[1] != ppov[1] ) m_lastItem = "hat2_" + HatDir( pov[0] );
      if ( pov[2] >= 0 ) if ( pov[2] != ppov[2] ) m_lastItem = "hat3_" + HatDir( pov[0] );
      if ( pov[3] >= 0 ) if ( pov[3] != ppov[3] ) m_lastItem = "hat4_" + HatDir( pov[0] );

      bool[] buttons = m_state.Buttons;
      for ( int bi=0; bi < buttons.Length; bi++ ) {
        if ( buttons[bi] ) m_lastItem = "button" + ( bi + 1 ).ToString( );
      }
      return m_lastItem;
    }

    /// <summary>
    /// Show the current props in the GUI
    /// </summary>
    private void UpdateUI( )
    {
      // This function updated the UI with joystick state information.
      string strText = null;

      m_jPanel.X = m_state.X.ToString( );
      m_jPanel.Y = m_state.Y.ToString( );
      m_jPanel.Z = m_state.Z.ToString( );

      m_jPanel.Xr = m_state.RotationX.ToString( );
      m_jPanel.Yr = m_state.RotationY.ToString( );
      m_jPanel.Zr = m_state.RotationZ.ToString( );


      int[] slider = m_state.Sliders;

      m_jPanel.S1 = slider[0].ToString( );
      m_jPanel.S2 = slider[1].ToString( );

      int[] pov = m_state.PointOfViewControllers;

      m_jPanel.H1 = pov[0].ToString( );
      m_jPanel.H2 = pov[1].ToString( );
      m_jPanel.H3 = pov[2].ToString( );
      m_jPanel.H4 = pov[3].ToString( );

      // Fill up text with which buttons are pressed
      bool[] buttons = m_state.Buttons;

      int button = 0;
      foreach ( bool b in buttons ) {
        if ( b )
          strText += ( button + 1 ).ToString( "00 " ); // buttons are 1 based
        button++;
      }
      m_jPanel.Button = strText;
    }


    /// <summary>
    /// Collect the current data from the device
    /// </summary>
    public void GetData( )
    {
      // Make sure there is a valid device.
      if ( null == m_device )
        return;

      // Poll the device for info.
      try {
        m_device.Poll( );
      }
      catch ( SharpDXException e ) {
        if ( ( e.ResultCode == ResultCode.NotAcquired ) || ( e.ResultCode == ResultCode.InputLost ) ) {
          // Check to see if either the app needs to acquire the device, or
          // if the app lost the device to another process.
          try {
            // Acquire the device.
            m_device.Acquire( );
          }
          catch ( SharpDXException ) {
            // Failed to acquire the device.
            // This could be because the app doesn't have focus.
            return;
          }
        }
      }


      // Get the state of the device - retaining the previous state to find the lates change
      m_prevState = m_state;
      try { m_state = m_device.GetCurrentState( ); }
      // Catch any exceptions. None will be handled here, 
      // any device re-aquisition will be handled above.  
      catch ( SharpDXException ) {
        return;
      }

      UpdateUI( ); // and update the GUI
    }



  }
}
