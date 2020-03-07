using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Devices
{
  public class DeviceList : List<DeviceCls>
  {
    /// <summary>
    /// Dumps the know GameDevices
    /// </summary>
    /// <returns></returns>
    public string DumpDevices()
    {
      this.Clear( );
      if ( AppSettings.Instance.DetectGamepad && ( DeviceInst.GamepadRef != null ) ) {
        this.Add( DeviceInst.GamepadRef );
      }
      this.AddRange( DeviceInst.JoystickListRef );
      this.Add( DeviceInst.MouseRef );

      string ret = $"DXInput Device Listing:\n";
      foreach(var dev in this ) {
        ret += $"\t{(dev.DevClass+":").PadRight(10)}{dev.DevName}\n\t - Product: {dev.DevGUID} - Instance: {{{dev.DevInstanceGUID}}}\n";
      }
      return ret;
    }
  }
}
