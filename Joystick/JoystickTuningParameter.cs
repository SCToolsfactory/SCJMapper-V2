using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SCJMapper_V2
{
  /// <summary>
  /// set of parameters to tune the Joystick
  /// </summary>
  public class JoystickTuningParameter
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private String m_command = "";  // v_pitch - js1_x ..
    private String m_cmdCtrl = "";  // x, y, rotz ...
    private int m_jsN = 0;   // jsN

    String m_option = ""; // the option name (level where it applies)

    private String m_deviceName = "";

    private bool   m_deadzoneEnabled = false;  // default
    private String m_deadzone = "0.000";

    private bool   m_senseEnabled = false;  // default
    private String m_sense = "1.00";

    private bool   m_invertEnabled = false;  // default

    private bool   m_expEnabled = false;  // default
    private String m_exponent = "1.000";

    private bool   m_ptsEnabled = false;  // default
    private List<String> m_PtsIn = new List<String>( );
    private List<String> m_PtsOut = new List<String>( );

    private JoystickList m_joystickList = null;
    private JoystickCls m_js = null;

    public JoystickTuningParameter( JoystickList jsList )
    {
      m_joystickList = jsList;
    }

    #region Properties

    public JoystickCls JsDevice
    {
      get { return m_js; }
    }


    public int JsN
    {
      get { return m_jsN; }
      set
      {
        m_jsN = value;
        m_js = m_joystickList.Find_jsN( m_jsN );
        m_deviceName = m_js.DevName;
      }
    }

    public String DeviceName
    {
      get { return m_deviceName; }
      set { m_deviceName = value; }
    }

    public String Command
    {
      get { return m_command; }
      set { m_command = value; DecomposeCommand( ); }
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


    /// <summary>
    /// Derive values from a command (e.g. v_pitch - js1_x)
    /// </summary>
    private void DecomposeCommand( )
    {
      // pobulate from input
      String[] e = Command.Split( new Char[] { '-' } );  // v_pitch - js1_x
      m_cmdCtrl = ""; m_jsN = 0;
      if ( e.Length > 1 ) {
        // get parts
        m_cmdCtrl = e[1].Trim( ).Split( new Char[] { '_' } )[1]; //js1_x -> x
        m_jsN = JoystickCls.JSNum( e[1].Trim( ) ); // get the right Joystick from jsN
        m_js = m_joystickList.Find_jsN( m_jsN );
        m_deviceName = m_js.DevName;
        m_option = String.Format( "pilot_move_{0}", m_cmdCtrl ); // update from Command
      }
    }


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


    /// <summary>
    /// Format an XML -options- node from the tuning contents
    /// </summary>
    /// <returns>The XML string or an empty string</returns>
    public String Options_toXML( )
    {
      if ( ( m_senseEnabled || m_expEnabled || m_invertEnabled || m_ptsEnabled ) == false ) return ""; // not used

      String tmp = "";
      tmp += String.Format( "\t<options type=\"joystick\" instance=\"{0}\">\n", m_jsN.ToString( ) );
      tmp += String.Format( "\t\t<{0} ", m_option );

      if ( InvertUsed ) {
        tmp += String.Format( "invert=\"1\" " );
      }
      if ( SensitivityUsed ) {
        tmp += String.Format( "sensitivity=\"{0}\" ", m_sense );
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
        tmp += String.Format( "exponent=\"{0}\" /> \n", m_exponent );
      }
      else {
        // neither exp or curve
        tmp += String.Format( "exponent=\"1.00\" /> \n" );// CIG get to default expo 2.something if not set to 1 here
      }

      tmp += String.Format( "\t</options>\n \n" );

      return tmp;
    }


    /// <summary>
    /// Read the options from the XML
    ///  can get only the 3 ones for Move X,Y,RotZ right now
    /// </summary>
    /// <param name="reader">A prepared XML reader</param>
    /// <param name="instance">the Josyticj instance number</param>
    /// <returns></returns>
    public Boolean Options_fromXML( XmlReader reader, int instance )
    {
      String invert = "";
      String sensitivity = "";
      String exponent = "";
      String instance_inv = "";

      m_option = reader.Name;
      JsN = instance;

      // derive from pilot_move_x || pilot_move_rotx (nothing bad should arrive here)
      String[] e = m_option.ToLowerInvariant( ).Split( new char[] { '_' } );
      if ( e.Length >= 2 ) m_cmdCtrl = e[2];


      if ( reader.HasAttributes ) {
        invert = reader["invert"];
        if ( !String.IsNullOrWhiteSpace( invert ) ) {
          m_invertEnabled = false;
          if ( invert == "1" ) m_invertEnabled = true;
        }

        instance_inv = reader["instance"]; // CIG wrong attr name ?!
        if ( !String.IsNullOrWhiteSpace( instance_inv ) ) {
          m_invertEnabled = false;
          if ( instance_inv == "1" ) m_invertEnabled = true;
        }

        sensitivity = reader["sensitivity"];
        if ( !String.IsNullOrWhiteSpace( sensitivity ) ) {
          m_sense = sensitivity;
          m_senseEnabled = true;
        }

        exponent = reader["exponent"];
        if ( !String.IsNullOrWhiteSpace( exponent ) ) {
          m_exponent = exponent;
          m_expEnabled = true;
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
