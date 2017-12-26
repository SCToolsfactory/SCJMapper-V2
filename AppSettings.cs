using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  sealed class AppSettings : ApplicationSettingsBase, IDisposable
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

    public void Dispose( bool disposing )
    {
      if ( disposing ) {
        // dispose managed resources
        if ( FS != null ) FS.Dispose( );
      }
      // free native resources
    }

    public void Dispose()
    {
      Dispose( true );
      GC.SuppressFinalize( this );
    }


    /// <summary>
    /// Show the Settings Dialog
    /// </summary>
    public DialogResult ShowSettings( string pasteString )
    {
      if ( FS == null ) FS = new FormSettings( this );
      FS.PasteString = pasteString; // propagate joyinput
      FS.ShowDialog( );
      return ( FS.Canceled ) ? DialogResult.Cancel : DialogResult.OK;
    }


    #region Setting Properties

    // manages Upgrade
    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )] 
    public bool FirstRun
    {
      get { return ( bool )this["FirstRun"]; }
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
    [DefaultSettingValueAttribute( "layout_joystick_spacesim" )] // from Game Bundle
    public string DefMappingName
    {
      get { return ( string )this["DefMappingName"]; }
      set { this["DefMappingName"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "layout_my_joystick" )] // just a default
    public string MyMappingName
    {
      get { return ( string )this["MyMappingName"]; }
      set { this["MyMappingName"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public bool ShowJoystick
    {
      get { return ( bool )this["ShowJoystick"]; }
      set { this["ShowJoystick"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public bool ShowGamepad
    {
      get { return ( bool )this["ShowGamepad"]; }
      set { this["ShowGamepad"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public bool ShowKeyboard
    {
      get { return ( bool )this["ShowKeyboard"]; }
      set { this["ShowKeyboard"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public bool ShowMouse  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
    {
      get { return ( bool )this["ShowMouse"]; }
      set { this["ShowMouse"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool ShowMapped
    {
      get { return ( bool )this["ShowMapped"]; }
      set { this["ShowMapped"] = value; }
    }


    // Seetings Window

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS1
    {
      get { return ( string )this["IgnoreJS1"]; }
      set { this["IgnoreJS1"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS2
    {
      get { return ( string )this["IgnoreJS2"]; }
      set { this["IgnoreJS2"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS3
    {
      get { return ( string )this["IgnoreJS3"]; }
      set { this["IgnoreJS3"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS4
    {
      get { return ( string )this["IgnoreJS4"]; }
      set { this["IgnoreJS4"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS5
    {
      get { return ( string )this["IgnoreJS5"]; }
      set { this["IgnoreJS5"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS6
    {
      get { return ( string )this["IgnoreJS6"]; }
      set { this["IgnoreJS6"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS7
    {
      get { return ( string )this["IgnoreJS7"]; }
      set { this["IgnoreJS7"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS8
    {
      get { return ( string )this["IgnoreJS8"]; }
      set { this["IgnoreJS8"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS9
    {
      get { return ( string )this["IgnoreJS9"]; }
      set { this["IgnoreJS9"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS10
    {
      get { return ( string )this["IgnoreJS10"]; }
      set { this["IgnoreJS10"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS11
    {
      get { return ( string )this["IgnoreJS11"]; }
      set { this["IgnoreJS11"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string IgnoreJS12
    {
      get { return ( string )this["IgnoreJS12"]; }
      set { this["IgnoreJS12"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string UserSCPath
    {
      get { return ( string )this["UserSCPath"]; }
      set { this["UserSCPath"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool UserSCPathUsed
    {
      get { return ( bool )this["UserSCPathUsed"]; }
      set { this["UserSCPathUsed"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( ",default,multiplayer,singleplayer,player,flycam,vehicle_driver," )] // empty  Note: comma separated list, must have a comma at the begining and the end (to find 'player' on its own...)
    public string IgnoreActionmaps
    {
      get { return ( string )this["IgnoreActionmaps"]; }
      set { this["IgnoreActionmaps"] = value; }
    }


    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool DetectGamepad
    {
      get { return ( bool )this["DetectGamepad"]; }
      set { this["DetectGamepad"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool UsePTU
    {
      get { return false; } // ( bool )this["UsePTU"]; } no longer used
      set { this["UsePTU"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool UseCSVListing
    {
      get { return ( bool )this["UseCSVListing"]; }
      set { this["UseCSVListing"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool ListModifiers
    {
      get { return ( bool )this["ListModifiers"]; }
      set { this["ListModifiers"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public bool AutoTabXML
    {
      get { return (bool)this["AutoTabXML"]; }
      set { this["AutoTabXML"] = value; }
    }

    

    //**** Form Table

    // Control bound settings
    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "1000, 900" )]
    public Size FormTableSize
    {
      get { return ( Size )this["FormTableSize"]; }
      set { this["FormTableSize"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "760, 320" )]
    public Point FormTableLocation
    {
      get { return ( Point )this["FormTableLocation"]; }
      set { this["FormTableLocation"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public string FormTableColumnWidth
    {
      get { return ( string )this["FormTableColumnWidth"]; }
      set { this["FormTableColumnWidth"] = value; }
    }



    #endregion


  }
}
