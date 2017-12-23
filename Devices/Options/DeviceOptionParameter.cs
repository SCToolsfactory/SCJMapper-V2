


using System;

namespace SCJMapper_V2.Devices.Options
{
  public class DeviceOptionParameter : ICloneable
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private string m_deviceName = "";
    private string m_deviceGUID = ""; // identify the beast!
    private string m_deviceClass = "";
    private int m_deviceNumber = 0;
    private string m_cmdCtrl = "";        // x, y, rotz ...
    private string m_doID = "";

    private string m_action = "";  // v_pitch ..  assigned if known only

    private bool m_deadzoneEnabled = false;  // default
    private string m_deadzone = "0.000";

    private bool m_saturationSupported = false; // supported for Joystick only
    private bool m_saturationEnabled = false;  // default
    private string m_saturation = "1.000";


    /// <summary>
    /// Clone this object
    /// </summary>
    /// <returns>A deep Clone of this object</returns>
    public object Clone()
    {
      var dop = (DeviceOptionParameter)this.MemberwiseClone( );
      // more objects to deep copy

      return dop;
    }

    /// <summary>
    /// Check clone against This
    /// </summary>
    /// <param name="clone"></param>
    /// <returns>True if the clone is identical but not a shallow copy</returns>
    internal bool CheckClone( DeviceOptionParameter clone )
    {
      bool ret = true;
      // object vars first
      ret &= ( this.m_deviceName == clone.m_deviceName ); // immutable string - shallow copy is OK
      ret &= ( this.m_deviceGUID == clone.m_deviceGUID ); // immutable string - shallow copy is OK
      ret &= ( this.m_deviceClass == clone.m_deviceClass );// immutable string - shallow copy is OK
      ret &= ( this.m_deviceNumber == clone.m_deviceNumber );
      ret &= ( this.m_cmdCtrl == clone.m_cmdCtrl ); // immutable string - shallow copy is OK
      ret &= ( this.m_doID == clone.m_doID ); // immutable string - shallow copy is OK
      ret &= ( this.m_action == clone.m_action ); // immutable string - shallow copy is OK
      ret &= ( this.m_deadzoneEnabled == clone.m_deadzoneEnabled );
      ret &= ( this.m_deadzone == clone.m_deadzone ); // immutable string - shallow copy is OK
      ret &= ( this.m_saturationSupported == clone.m_saturationSupported );
      ret &= ( this.m_saturationEnabled == clone.m_saturationEnabled );
      ret &= ( this.m_saturation == clone.m_saturation ); // immutable string - shallow copy is OK
      return ret;
    }



    /// <summary>
    /// cTor : empty
    /// </summary>
    public DeviceOptionParameter()
    {
    }

    /// <summary>
    /// cTor with content
    /// </summary>
    /// <param name="deviceName">The device name</param>
    /// <param name="cmdCtrl">The command e.g. x,y, rotz etc</param>
    /// <param name="dz">The deadzone value as string (empty string disables)</param>
    /// <param name="sa">The saturation value as string (empty string disables)</param>
    public DeviceOptionParameter( DeviceCls device, string cmdCtrl, string dz, string sa )
    {
      m_deviceClass = device.DevClass;
      m_deviceName = device.DevName;
      m_deviceGUID = device.DevInstanceGUID;
      m_deviceNumber = device.DevInstance;

      m_doID = Deviceoptions.DevOptionID( m_deviceClass, m_deviceName, cmdCtrl );
      m_cmdCtrl = cmdCtrl;

      m_deadzone = "0.000";
      m_deadzoneEnabled = false;
      if ( !string.IsNullOrEmpty( dz ) ) {
        m_deadzone = dz;
        m_deadzoneEnabled = true;
      }

      m_saturationSupported = false;
      m_saturation = "1.000";
      m_saturationEnabled = false;
      if ( Devices.Joystick.JoystickCls.IsDeviceClass( m_deviceClass ) ) {
        m_saturationSupported = true;
        if ( !string.IsNullOrEmpty( sa ) ) {
          m_saturation = sa;
          m_saturationEnabled = true;
        }
      }
    }

    #region Properties


    public string DevClass
    {
      get { return m_deviceClass; }
    }

    public string DevName
    {
      get { return m_deviceName; }
    }

    /// <summary>
    /// Returns the dx device number of the item
    /// </summary>
    public int DevInstance
    {
      get { return m_deviceNumber; }
    }

    public string DevInstanceGUID
    {
      get { return m_deviceGUID; }
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
        return ( m_deadzoneEnabled && ( m_deadzone != "0.000" ) );
      }
      set { m_deadzoneEnabled = value; }
    }

    public string Deadzone
    {
      get { return m_deadzone; }
      set { m_deadzone = value; }
    }

    public bool SaturationSupported
    {
      get {
        return m_saturationSupported;
      }
      set { m_saturationSupported = value; }
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
    public string Deviceoptions_toXML()
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
        if ( SaturationSupported && SaturationUsed ) tmp += string.Format( "\t\t<option input=\"{0}\" saturation=\"{1}\" />\n", m_cmdCtrl, m_saturation );
        tmp += string.Format( "\t</deviceoptions>\n \n" );
      }
      return tmp;
    }

  }
}
