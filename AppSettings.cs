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
    [DefaultSettingValueAttribute( "Reset empty" )] // defaults to one of the ResetMode Settings below
    public String ResetMode
    {
      get { return ( String )this["ResetMode"]; }
      set { this["ResetMode"] = value; }
    }

    // Application Settings
    [ApplicationScopedSetting( )]
    [DefaultSettingValueAttribute( "Reset empty" )]
    public String ResetModeEmpty
    {
      get { return ( String )this["ResetModeEmpty"]; }
      set { ; } // cannot be changed
    }

    [ApplicationScopedSetting( )]
    [DefaultSettingValueAttribute( "Reset defaults" )]
    public String ResetModeDefault
    {
      get { return ( String )this["ResetModeDefault"]; }
      set { ; } // cannot be changed
    }




  }
}
