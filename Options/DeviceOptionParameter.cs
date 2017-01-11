


namespace SCJMapper_V2.Options
{
  public class DeviceOptionParameter
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private string m_deviceName = "";
    private string m_cmdCtrl = "";        // x, y, rotz ...
    private string m_doID = "";

    private string m_action ="";  // v_pitch ..  assigned if known only

    private bool   m_deadzoneEnabled = false;  // default
    private string m_deadzone = "0.000";

    private bool   m_saturationEnabled = false;  // default
    private string m_saturation = "1.000";


    public DeviceOptionParameter( )
    {     
    }

    /// <summary>
    /// cTor with content
    /// </summary>
    /// <param name="deviceName">The device name</param>
    /// <param name="cmdCtrl">The command e.g. x,y, rotz etc</param>
    /// <param name="dz">The deadzone value as string (empty string disables)</param>
    /// <param name="sa">The saturation value as string (empty string disables)</param>
    public DeviceOptionParameter(string deviceName, string cmdCtrl, string dz, string sa )
    {
      m_deviceName = deviceName;
      m_doID = Deviceoptions.DevOptionID( deviceName, cmdCtrl );
      m_cmdCtrl = cmdCtrl;
      if ( string.IsNullOrEmpty( dz ) ) {
        m_deadzone = "0.000";
        m_deadzoneEnabled = false;
      } else {
        m_deadzone = dz;
        m_deadzoneEnabled = true;
      }
      if ( string.IsNullOrEmpty(sa)) {
        m_saturation = "1.000";
        m_saturationEnabled = false;
      } else {
        m_saturation = sa;
        m_saturationEnabled = true;
      }
    }

    #region Properties


    public string DeviceName
    {
      get { return m_deviceName; }
      set { m_deviceName = value; }
    }

    public string DoID
    {
      get { return m_doID; }
      set { m_doID = value; }
    }

    public string CommandCtrl
    {
      get { return m_cmdCtrl; }
      set { m_cmdCtrl = value; }
    }

    public string Action
    {
      get { return m_action; }
      set { m_action = value; }
    }


    public bool DeadzoneUsed
    {
      get {
        return ( m_deadzoneEnabled && ( m_deadzone != "0.0000" ) );
      }
      set { m_deadzoneEnabled = value; }
    }

    public string Deadzone
    {
      get { return m_deadzone; }
      set { m_deadzone = value; }
    }

    public bool SaturationUsed
    {
      get {
        return ( m_saturationEnabled && ( m_saturation != "1.000" ) );
      }
      set { m_saturationEnabled = value; }
    }

    public string Saturation
    {
      get { return m_saturation; }
      set { m_saturation = value; }
    }

    #endregion



    /// <summary>
    /// Format an XML -deviceoptions- node from the tuning contents
    /// </summary>
    /// <returns>The XML string or an empty string</returns>
    public string Deviceoptions_toXML( )
    {
      /*
           <deviceoptions name="Joystick - HOTAS Warthog">
            <!-- Reduce the deadzone -->
            <option input="x" deadzone="0.015" />
            <option input="y" deadzone="0.015" />	
            <option input="y" saturation="0.85" />	
          </deviceoptions>
       */
      string tmp = "";
      if ( DeadzoneUsed || SaturationUsed ) {
        tmp += string.Format( "\t<deviceoptions name=\"{0}\">\n", m_deviceName );
        if ( DeadzoneUsed ) tmp += string.Format( "\t\t<option input=\"{0}\" deadzone=\"{1}\" />\n", m_cmdCtrl, m_deadzone );
        if ( SaturationUsed ) tmp += string.Format( "\t\t<option input=\"{0}\" saturation=\"{1}\" />\n", m_cmdCtrl, m_saturation );
        tmp += string.Format( "\t</deviceoptions>\n \n" );
      }
      return tmp;
    }



  }
}
