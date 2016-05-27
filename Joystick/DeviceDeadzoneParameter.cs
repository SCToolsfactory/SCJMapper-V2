using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2.Joystick
{
  public class DeviceDeadzoneParameter
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private String m_deviceName = "";
    private String m_cmdCtrl = "";        // x, y, rotz ...

    private bool   m_deadzoneEnabled = false;  // default
    private String m_deadzone = "0.000";


    public DeviceDeadzoneParameter( )
    {
    }

    #region Properties


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
