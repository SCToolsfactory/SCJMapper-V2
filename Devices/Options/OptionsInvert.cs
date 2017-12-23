using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Devices.Gamepad;

namespace SCJMapper_V2.Devices.Options
{
  public class OptionsInvert
  {

    public enum Inversions
    {
      flight_aim_pitch = 0, // used also as array index - MUST remain 0,
      flight_view_pitch,
      flight_aim_yaw,
      flight_view_yaw,
      flight_throttle,
//      flight_move_strafe_vertical,
//      flight_move_strafe_lateral,
//      flight_move_strafe_longitudinal,

      I_LAST // designates the last item for loop handling
    }

    public struct MappedActionRec
    {
      public MappedActionRec( String m, String a ) { Map = m; Action = a; }
      public String Map;
      public String Action;
    }

    // setup of mapped actions as of AC 1.0 (may need a change once in a while...)
    //Note: sequence is matched with the Enum above
    static public  MappedActionRec[] MappedActions = {
        new MappedActionRec("spaceship_targeting", "v_aim_pitch"),
        new MappedActionRec("spaceship_view", "v_view_pitch"),
        new MappedActionRec("spaceship_targeting", "v_aim_yaw"),
        new MappedActionRec("spaceship_view", "v_view_yaw"),
        new MappedActionRec("spaceship_movement", "v_throttle_abs"),
//        new MappedActionRec("spaceship_movement", "v_strafe_vertical"),
//        new MappedActionRec("spaceship_movement", "v_strafe_lateral"),
//        new MappedActionRec("spaceship_movement", "v_strafe_longitudinal"),
                                             };

    private CheckBox m_cbInvert = null;
    private DeviceCls m_device = null;
    private String m_type = "";         // joystick OR xboxpad
    private int m_devInstanceNo = -1;   // jsN - instance in XML

    String m_option = ""; // the option name (level where it applies)

    // ctor
    public OptionsInvert( Inversions item )
    {
      m_option = item.ToString();
    }


    #region Properties

    public DeviceCls GameDevice
    {
      get { return m_device; }
      set
      {
        m_device = value;
        m_type = "";
        m_devInstanceNo = -1;
        if ( m_device == null ) return; // got a null device

        if ( JoystickCls.IsDeviceClass( m_device.DevClass ) ) {
          m_type = m_device.DevClass;
          m_devInstanceNo = ( m_device as JoystickCls ).JSAssignment;
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

    public bool InvertUsed
    {
      get { return m_cbInvert.Checked; }
      set { m_cbInvert.Checked = value; }
    }

    public CheckBox CBInvert
    {
      set { m_cbInvert = value; }
    }

    #endregion

    public void Reset( )
    {
      m_cbInvert.Checked = false;
    }


    /// <summary>
    /// Format an XML -options- node from the tuning contents
    /// </summary>
    /// <returns>The XML string or an empty string</returns>
    public String Options_toXML( )
    {
      if ( InvertUsed == false ) return ""; // not used
      if ( DevInstanceNo <= 0 ) return "";  // not assigned or not valid...

      String tmp = "";
      tmp += String.Format( "\t<options type=\"{0}\" instance=\"{1}\">\n", m_type, m_devInstanceNo.ToString( ) );
      tmp += String.Format( "\t\t<{0} ", m_option );

      if ( InvertUsed ) { // leave this IF in - to allow to extend the code for sensitivity
        tmp += String.Format( "invert=\"1\" " );
      }
      tmp += String.Format( "/> \n" );// CIG get to default expo 2.something if not set to 1 here

      tmp += String.Format( "\t</options>\n \n" );

      return tmp;
    }


    /// <summary>
    /// Read the options from the XML
    /// </summary>
    /// <param name="reader">A prepared XML reader</param>
    /// <param name="instance">the Joystick instance number</param>
    /// <returns></returns>
    public Boolean Options_fromXML( XmlReader reader, String type, int instance )
    {
      m_type = type;

      String invert = "";
      m_option = reader.Name;
      m_devInstanceNo = instance;

      if ( reader.HasAttributes ) {
        invert = reader["invert"];
        if ( !String.IsNullOrWhiteSpace( invert ) ) {
          InvertUsed = false;
          if ( invert == "1" ) InvertUsed = true;
        }

      }
      reader.Read( );

      return true;
    }


  }
}
