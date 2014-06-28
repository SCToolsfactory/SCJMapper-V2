using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace SCJMapper_V2
{
  /// <summary>
  /// Find the SC pathes and folders
  /// </summary>
  class SCPath
  {

    /// <summary>
    /// Try to locate the launcher under "App Paths"
    /// </summary>
    static private String SCLauncherPath1
    {
      get
      {
        String scpath = ( String )Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths\StarCitizen Launcher.exe", "", null );
        if ( scpath != null ) {
          return scpath;
        }
        return "";
      }
    }

    /// <summary>
    /// Try to locate the launcher under "Uninstall"
    /// </summary>
    static private String SCLauncherPath2
    {
      get
      {
        String scpath = ( String )Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StarCitizen", "DisplayIcon", null );
        if ( scpath != null ) {
          return scpath;
        }
        return "";
      }
    }


    /// <summary>
    /// Returns the base SC install path from something like "E:\G\StarCitizen\Launcher\StarCitizenLauncher.exe"
    /// </summary>
    static private String SCBasePath
    {
      get
      {
        String scp = SCLauncherPath1;
        if ( String.IsNullOrEmpty( scp ) ) {
          scp = SCLauncherPath2;
          if ( String.IsNullOrEmpty( scp ) ) {
            return "";  // sorry did not found a thing..
          }
        }
        // found the launcher
        scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen\Launcher"
        scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
        return scp;
      }
    }



    /// <summary>
    /// Returns the SC installation path
    /// </summary>
    static public String SCInstallPath
    {
      get
      {
        return SCBasePath;
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\Data"
    /// </summary>
    static public String SCClientDataPath
    {
      get
      {
        String scp = SCBasePath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "CitizenClient" );
        scp = Path.Combine( scp, "Data" );
        return scp;
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\USER"
    /// </summary>
    static public String SCClientUSERPath
    {
      get
      {
        String scp = SCBasePath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "CitizenClient" );
        scp = Path.Combine( scp, "USER" );
        return scp;
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\Data\Controls\Mappings"
    /// </summary>
    static public String SCClientMappingPath
    {
      get
      {
        String scp = SCClientDataPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "Controls" );
        scp = Path.Combine( scp, "Mappings" );
        return scp;
      }
    }


    /// <summary>
    /// Returns the SC GameData.pak file path  e.g.  "E:\G\StarCitizen\CitizenClient\Data\GameData.pak"
    /// </summary>
    static public String SCGameData_pak
    {
      get
      {
        String scp = SCClientDataPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "GameData.pak" );
        return scp;
      }
    }


    /// <summary>
    /// Returns the relative path of DefaultProfile.xml
    /// </summary>
    static public String DefaultProfilePath_rel
    {
      get
      {
        return @"Libs\Config";
      }
    }

    /// <summary>
    /// Returns the name part of the DefaultProfile w/o extension...
    /// </summary>
    static public String DefaultProfileName
    {
      get
      {
        return @"defaultProfile";
      }
    }


  }
}
