using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2
{
  public class DeviceDeadzoneParameter
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private String m_actionCommand = "";  // v_pitch - js1_x ..
    private String m_cmdCtrl = "";        // x, y, rotz ...
    private String m_type = "";           // joystick OR xboxpad
    private int m_devInstanceNo = -1;     // jsN - instance in XML

    String m_option = ""; // the option name (level where it applies)

    private String m_deviceName = "";

    private bool   m_deadzoneEnabled = false;  // default
    private String m_deadzone = "0.000";

    private DeviceCls m_device = null;

    public DeviceDeadzoneParameter( )
    {
    }

    #region Properties

    public DeviceCls GameDevice
    {
      get { return m_device; }
      set { m_device = value;
        m_type = "";
        m_devInstanceNo = -1;
        if ( JoystickCls.IsDeviceClass( m_device.DevClass ) ) {
          m_type = m_device.DevClass;
          m_devInstanceNo = (m_device as JoystickCls).JSAssignment;
        }
        else if ( GamepadCls.IsDeviceClass( m_device.DevClass ) ) {
          m_type = m_device.DevClass;
          m_devInstanceNo = 1; // supports ONE gamepad
        }
       }
    }


    public int DevInstanceNo
    {
      get { return m_devInstanceNo; }
    }

    public String DeviceName
    {
      get { return m_deviceName; }
      set { m_deviceName = value; }
    }

    public String CommandCtrl
    {
      get { return m_cmdCtrl; }
      set { m_cmdCtrl = value; }
    }


    public bool DeadzoneUsed
    {
      get { return m_deadzoneEnabled; }
      set { m_deadzoneEnabled = value; }
    }

    public String Deadzone
    {
      get { return m_deadzone; }
      set { m_deadzone = value; }
    }

    #endregion


    /// <summary>
    /// Format an XML -deviceoptions- node from the tuning contents
    /// </summary>
    /// <returns>The XML string or an empty string</returns>
    public String Deviceoptions_toXML( )
    {
      /*
	         <deviceoptions name="Joystick - HOTAS Warthog">
		        <!-- Reduce the deadzone -->
		        <option input="x" deadzone="0.015" />
		        <option input="y" deadzone="0.015" />	
	        </deviceoptions>
       */
      String tmp = "";
      if ( m_deadzoneEnabled ) {
        tmp += String.Format( "\t<deviceoptions name=\"{0}\">\n", m_deviceName );
        tmp += String.Format( "\t\t<option input=\"{0}\" deadzone=\"{1}\" />\n", m_cmdCtrl, m_deadzone );
        tmp += String.Format( "\t</deviceoptions>\n \n" );
      }
      return tmp;
    }


  
  }
}
