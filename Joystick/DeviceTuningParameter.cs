using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace SCJMapper_V2.Joystick
{
  /// <summary>
  /// set of parameters to tune the Joystick
  /// </summary>
  public class DeviceTuningParameter
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private String m_action = "";       // v_pitch
    private String m_cmdCtrl = "";      // js1_x, js1_y, js1_rotz ...
    private String m_type = "";         // joystick OR xboxpad
    private int m_devInstanceNo = -1;   // jsN - instance in XML

    String m_option = ""; // the option name (level where it applies)

    private String m_deviceName = "";

    private bool   m_senseEnabled = false;  // default
    private String m_sense = "1.00";

    private bool   m_expEnabled = false;  // default
    private String m_exponent = "1.000";

    private bool   m_ptsEnabled = false;  // default
    private List<String> m_PtsIn = new List<String>( );
    private List<String> m_PtsOut = new List<String>( );

    private bool   m_invertEnabled = false; // default

    private DeviceCls m_device = null;

    private DeviceDeadzoneParameter m_deadzone = null;

    public DeviceTuningParameter( )
    {
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
        else if ( Gamepad.GamepadCls.IsDeviceClass( m_device.DevClass ) ) {
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

    public String Action
    {
      get { return m_action; }
      set { m_action = value; DecomposeCommand( ); }
    }

    public String CommandCtrl
    {
      get { return m_cmdCtrl; }
      set { m_cmdCtrl = value; }
    }


    public DeviceDeadzoneParameter Deadzone
    {
      get { return m_deadzone; }
      set
      {
        m_deadzone = value;
        if ( m_deadzone != null ) {
          m_deadzone.DeviceName = DeviceName;     // must know too
          m_deadzone.CommandCtrl = CommandCtrl;   // must know too
        }
      }
    }

    public bool SensitivityUsed
    {
      get { return m_senseEnabled; }
      set { m_senseEnabled = value; }
    }

    public String Sensitivity
    {
      get { return m_sense; }
      set { m_sense = value; }
    }

    public bool InvertUsed
    {
      get { return m_invertEnabled; }
      set { m_invertEnabled = value; }
    }

    public bool ExponentUsed
    {
      get { return m_expEnabled; }
      set { m_expEnabled = value; }
    }

    public String Exponent
    {
      get { return m_exponent; }
      set { m_exponent = value; }
    }


    public bool NonLinCurveUsed
    {
      get { return m_ptsEnabled; }
      set { m_ptsEnabled = value; }
    }

    public List<String> NonLinCurvePtsIn
    {
      get { return m_PtsIn; }
      set { m_PtsIn = value; }
    }
    public List<String> NonLinCurvePtsOut
    {
      get { return m_PtsOut; }
      set { m_PtsOut = value; }
    }

    #endregion

    // reset some defaults
    public void Reset( )
    {
      GameDevice = null;
      Deadzone = null;
      Action = "";
    }


    /// <summary>
    /// Derive values from a command (e.g. v_pitch - js1_x)
    /// </summary>
    private void DecomposeCommand( )
    {
      // populate from input
      // something like "v_pitch - js1_x" OR "v_pitch - xi_thumbl" OR "v_pitch - ximod+xi_thumbl+xi_mod"
      String cmd = ActionTreeNode.CommandFromNodeText( Action );
      String action = ActionTreeNode.ActionFromNodeText( Action );
      m_cmdCtrl = "";
      if ( !String.IsNullOrWhiteSpace( cmd ) ) {
        // decomp gamepad entries - could have modifiers so check for contains...
        if ( cmd.Contains( "xi_thumblx" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumblx";
          m_deviceName = m_device.DevName;
          if ( action.Contains( "pitch" ) ) m_option = String.Format( "flight_move_pitch" );
          else m_option = String.Format( "flight_move_yaw" );
        }
        else if ( cmd.Contains( "xi_thumbly" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumbly";
          m_deviceName = m_device.DevName;
          if ( action.Contains( "pitch" ) ) m_option = String.Format( "flight_move_pitch" );
          else m_option = String.Format( "flight_move_yaw" );
        }
        else if ( cmd.Contains( "xi_thumbrx" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumbrx";
          m_deviceName = m_device.DevName;
          if ( action.Contains( "pitch" ) ) m_option = String.Format( "flight_move_pitch" );
          else m_option = String.Format( "flight_move_yaw" );
        }
        else if ( cmd.Contains( "xi_thumbry" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumbry";
          m_deviceName = m_device.DevName;
          if ( action.Contains( "pitch" ) ) m_option = String.Format( "flight_move_pitch" );
          else m_option = String.Format( "flight_move_yaw" );
        }
        // assume joystick
        else {
          // get parts
          m_cmdCtrl = JoystickCls.ActionFromJsCommand( cmd ); //js1_x -> x; js2_rotz -> rotz
          m_deviceName = m_device.DevName;
          if ( action.Contains( "pitch" ) ) m_option = String.Format( "flight_move_pitch" );
          else if ( action.Contains( "yaw" ) ) m_option = String.Format( "flight_move_yaw" );
          else m_option = String.Format( "flight_move_roll" );
        }
      }
    }


    /// <summary>
    /// Format an XML -options- node from the tuning contents
    /// </summary>
    /// <returns>The XML string or an empty string</returns>
    public String Options_toXML( )
    {
      if ( ( SensitivityUsed || ExponentUsed || InvertUsed || NonLinCurveUsed ) == false ) return ""; // not used

      String tmp = "";
      tmp += String.Format( "\t<options type=\"{0}\" instance=\"{1}\">\n", m_type, m_devInstanceNo.ToString( ) );
      tmp += String.Format( "\t\t<{0} ", m_option );

      if ( InvertUsed ) {
        tmp += String.Format( "invert=\"1\" " );
      }
      if ( SensitivityUsed ) {
        tmp += String.Format( "sensitivity=\"{0}\" ", Sensitivity );
      }
      if ( NonLinCurveUsed ) {
        // add exp to avoid merge of things...
        tmp += String.Format( "exponent=\"1.00\" > \n" ); // CIG get to default expo 2.something if not set to 1 here
        tmp += String.Format( "\t\t\t<nonlinearity_curve>\n" );
        tmp += String.Format( "\t\t\t\t<point in=\"{0}\"  out=\"{1}\"/>\n", m_PtsIn[0], m_PtsOut[0] );
        tmp += String.Format( "\t\t\t\t<point in=\"{0}\"  out=\"{1}\"/>\n", m_PtsIn[1], m_PtsOut[1] );
        tmp += String.Format( "\t\t\t\t<point in=\"{0}\"  out=\"{1}\"/>\n", m_PtsIn[2], m_PtsOut[2] );
        tmp += String.Format( "\t\t\t</nonlinearity_curve>\n" );
        tmp += String.Format( "\t\t</{0}> \n", m_option );
      }
      else if ( ExponentUsed ) {
        // only exp used
        tmp += String.Format( "exponent=\"{0}\" /> \n", Exponent );
      }
      else {
        // neither exp or curve
        tmp += String.Format( " /> \n" );// nothing...
      }

      tmp += String.Format( "\t</options>\n \n" );

      return tmp;
    }


    /// <summary>
    /// Read the options from the XML
    ///  can get only the 3 ones for Move Pitch, Yaw, Roll right now
    /// </summary>
    /// <param name="reader">A prepared XML reader</param>
    /// <param name="instance">the Joystick instance number</param>
    /// <returns></returns>
    public Boolean Options_fromXML( XmlReader reader, String type, int instance )
    {
      m_type = type;

      String invert = "";
      String sensitivity = "";
      String exponent = "";

      m_option = reader.Name;
      m_devInstanceNo = instance;

      // derive from flight_move_pitch || flight_move_yaw || flight_move_roll (nothing bad should arrive here)
      String[] e = m_option.ToLowerInvariant( ).Split( new char[] { '_' } );
      if ( e.Length > 2 ) m_cmdCtrl = e[2]; // TODO - see if m_cmdCtrl is needed to be derived here


      if ( reader.HasAttributes ) {
        invert = reader["invert"];
        if ( !String.IsNullOrWhiteSpace( invert ) ) {
          InvertUsed = false;
          if ( invert == "1" ) InvertUsed = true;
        }

        sensitivity = reader["sensitivity"];
        if ( !String.IsNullOrWhiteSpace( sensitivity ) ) {
          Sensitivity = sensitivity;
          SensitivityUsed = true;
        }

        exponent = reader["exponent"];
        if ( !String.IsNullOrWhiteSpace( exponent ) ) {
          Exponent = exponent;
          ExponentUsed = true;
        }
      }
      // we may have a nonlin curve...
      reader.Read( );
      if ( !reader.EOF ) {
        if ( reader.Name.ToLowerInvariant( ) == "nonlinearity_curve" ) {
          m_PtsIn.Clear( ); m_PtsOut.Clear( ); // reset pts

          reader.Read( );
          while ( !reader.EOF ) {
            String ptIn = "";
            String ptOut = "";
            if ( reader.Name.ToLowerInvariant( ) == "point" ) {
              if ( reader.HasAttributes ) {
                ptIn = reader["in"];
                ptOut = reader["out"];
                m_PtsIn.Add( ptIn ); m_PtsOut.Add( ptOut ); m_ptsEnabled = true;
              }
            }
            reader.Read( );
          }//while
          // sanity check - we've have to have 3 pts  here - else we subst
          // add 2nd
          if ( m_PtsIn.Count < 2 ) {
            m_PtsIn.Add( "0.5" ); m_PtsOut.Add( "0.5" );
            log.Info( "Options_fromXML: got only one nonlin point, added (0.5|0.5)" );
          }
          // add 3rd
          if ( m_PtsIn.Count < 3 ) {
            m_PtsIn.Add( "0.75" ); m_PtsOut.Add( "0.75" );
            log.Info( "Options_fromXML: got only two nonlin points, added (0.75|0.75)" );
          }
        }
      }

      return true;
    }


  }
}
