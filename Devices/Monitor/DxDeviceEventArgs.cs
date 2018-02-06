using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Devices.Monitor
{
  public class DxDeviceEventArgs
  {
    public DeviceMonitoring.DxDeviceStates DeviceStates { get; set; }
    public string SelectedCtrl { get; set; }

    public DxDeviceEventArgs( DeviceMonitoring.DxDeviceStates states )
    {
      DeviceStates = states;
    }
  }
}
