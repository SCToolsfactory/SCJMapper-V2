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
    public DialogResult ShowSettings( String pasteString )
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
    [DefaultSettingValueAttribute( "True" )]
    public Boolean ShowJoystick
    {
      get { return ( Boolean )this["ShowJoystick"]; }
      set { this["ShowJoystick"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public Boolean ShowGamepad
    {
      get { return ( Boolean )this["ShowGamepad"]; }
      set { this["ShowGamepad"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public Boolean ShowKeyboard
    {
      get { return ( Boolean )this["ShowKeyboard"]; }
      set { this["ShowKeyboard"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "True" )]
    public Boolean ShowMouse  // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
    {
      get { return ( Boolean )this["ShowMouse"]; }
      set { this["ShowMouse"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean ShowMapped
    {
      get { return ( Boolean )this["ShowMapped"]; }
      set { this["ShowMapped"] = value; }
    }


    // Seetings Window

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS1
    {
      get { return ( String )this["IgnoreJS1"]; }
      set { this["IgnoreJS1"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS2
    {
      get { return ( String )this["IgnoreJS2"]; }
      set { this["IgnoreJS2"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS3
    {
      get { return ( String )this["IgnoreJS3"]; }
      set { this["IgnoreJS3"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS4
    {
      get { return ( String )this["IgnoreJS4"]; }
      set { this["IgnoreJS4"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS5
    {
      get { return ( String )this["IgnoreJS5"]; }
      set { this["IgnoreJS5"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS6
    {
      get { return ( String )this["IgnoreJS6"]; }
      set { this["IgnoreJS6"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS7
    {
      get { return ( String )this["IgnoreJS7"]; }
      set { this["IgnoreJS7"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS8
    {
      get { return ( String )this["IgnoreJS8"]; }
      set { this["IgnoreJS8"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS9
    {
      get { return ( String )this["IgnoreJS9"]; }
      set { this["IgnoreJS9"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS10
    {
      get { return ( String )this["IgnoreJS10"]; }
      set { this["IgnoreJS10"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS11
    {
      get { return ( String )this["IgnoreJS11"]; }
      set { this["IgnoreJS11"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String IgnoreJS12
    {
      get { return ( String )this["IgnoreJS12"]; }
      set { this["IgnoreJS12"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "" )]
    public String UserSCPath
    {
      get { return ( String )this["UserSCPath"]; }
      set { this["UserSCPath"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean UserSCPathUsed
    {
      get { return ( Boolean )this["UserSCPathUsed"]; }
      set { this["UserSCPathUsed"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( ",default,multiplayer,singleplayer,player,flycam,vehicle_driver," )] // empty  Note: comma separated list, must have a comma at the begining and the end (to find 'player' on its own...)
    public String IgnoreActionmaps
    {
      get { return ( String )this["IgnoreActionmaps"]; }
      set { this["IgnoreActionmaps"] = value; }
    }


    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean DetectGamepad
    {
      get { return ( Boolean )this["DetectGamepad"]; }
      set { this["DetectGamepad"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean UsePTU
    {
      get { return false; } // ( Boolean )this["UsePTU"]; } no longer used
      set { this["UsePTU"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean UseCSVListing
    {
      get { return ( Boolean )this["UseCSVListing"]; }
      set { this["UseCSVListing"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean ListModifiers
    {
      get { return ( Boolean )this["ListModifiers"]; }
      set { this["ListModifiers"] = value; }
    }

    [UserScopedSettingAttribute( )]
    [DefaultSettingValueAttribute( "False" )]
    public Boolean AutoTabXML
    {
      get { return (Boolean)this["AutoTabXML"]; }
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
