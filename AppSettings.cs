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
    FormSettings FS = null;

    public void ShowSettings( )
    {
      if ( FS == null ) FS = new FormSettings( this );
      FS.ShowDialog( );
    }


    #region Setting Properties


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

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "layout_my_joystick" )] // just a default
    public String MyMappingName
    {
      get { return ( String )this["MyMappingName"]; }
      set { this["MyMappingName"] = value; }
    }


    // Seetings Window

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS1
    {
      get { return ( String )this["IgnoreJS1"]; }
      set { this["IgnoreJS1"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS2
    {
      get { return ( String )this["IgnoreJS2"]; }
      set { this["IgnoreJS2"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS3
    {
      get { return ( String )this["IgnoreJS3"]; }
      set { this["IgnoreJS3"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS4
    {
      get { return ( String )this["IgnoreJS4"]; }
      set { this["IgnoreJS4"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS5
    {
      get { return ( String )this["IgnoreJS5"]; }
      set { this["IgnoreJS5"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS6
    {
      get { return ( String )this["IgnoreJS6"]; }
      set { this["IgnoreJS6"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS7
    {
      get { return ( String )this["IgnoreJS7"]; }
      set { this["IgnoreJS7"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String IgnoreJS8
    {
      get { return ( String )this["IgnoreJS8"]; }
      set { this["IgnoreJS8"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )] // empty
    public String UserSCPath
    {
      get { return ( String )this["UserSCPath"]; }
      set { this["UserSCPath"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )] // false
    public Boolean UserSCPathUsed
    {
      get { return ( Boolean )this["UserSCPathUsed"]; }
      set { this["UserSCPathUsed"] = value; }
    }


    #endregion


  }
}
