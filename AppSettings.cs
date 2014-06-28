using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Drawing;

namespace SCJMapper_V2
{
  sealed class AppSettings : ApplicationSettingsBase
  {
    // Control bound settings
    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "1000, 900" )]
    public Size FormSize
    {
      get { return ( Size )this["FormSize"]; }
      set { this["FormSize"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "10, 10" )]
    public Point FormLocation
    {
      get { return ( Point )this["FormLocation"]; }
      set { this["FormLocation"] = value; }
    }

    // User Config Settings
    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "defaultProfile" )] // from Game Bundle
    public String DefProfileName
    {
      get { return ( String )this["DefProfileName"]; }
      set { this["DefProfileName"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "layout_joystick_spacesim" )] // from Game Bundle
    public String DefMappingName
    {
      get { return ( String )this["DefMappingName"]; }
      set { this["DefMappingName"] = value; }
    }



  }
}
