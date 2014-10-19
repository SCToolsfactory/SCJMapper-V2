using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  sealed class AppSettings : ApplicationSettingsBase
  {
    FormSettings FS = null;

    public AppSettings( )
    {
      if ( this.FirstRun ) {
        // migrate the settings to the new version if the app runs the rist time
        try {
          this.Upgrade( );
        }
        catch { }
        this.FirstRun = false;
        this.Save( );
      }
    }

    /// <summary>
    /// Show the Settings Dialog
    /// </summary>
    public DialogResult ShowSettings( )
    {
      if ( FS == null ) FS = new FormSettings( this );
      FS.ShowDialog( );
      return ( FS.Canceled ) ? DialogResult.Cancel : DialogResult.OK;
    }


    #region Setting Properties

    // manages Upgrade
    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )] 
    public Boolean FirstRun
    {
      get { return ( Boolean )this["FirstRun"]; }
      set { this["FirstRun"] = value; }
    }


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

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )] // false
    public Boolean BlendUnmapped
    {
      get { return ( Boolean )this["BlendUnmapped"]; }
      set { this["BlendUnmapped"] = value; }
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

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( ",multiplayer,player,flycam,vehicle_driver,singleplayer," )] // empty  Note: comma separated list, must have a comma at the begining and the end (to find 'player' on its own...)
    public String IgnoreActionmaps
    {
      get { return ( String )this["IgnoreActionmaps"]; }
      set { this["IgnoreActionmaps"] = value; }
    }


    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )] // false
    public Boolean ForceIgnoreversion
    {
      get { return ( Boolean )this["ForceIgnoreversion"]; }
      set { this["ForceIgnoreversion"] = value; }
    }


    #endregion


  }
}
