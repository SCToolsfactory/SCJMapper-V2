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
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );
    private static readonly AppSettings  appSettings = new AppSettings( );

    /// <summary>
    /// Try to locate the launcher under "App Paths"
    /// </summary>
    static private String SCLauncherFile1
    {
      get
      {
        log.Debug( "SCLauncherFile1 - Entry" );
        String scLauncher = ( String )Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths\StarCitizen Launcher.exe", "", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherFile1 - Found HKLM - AppPath - Launcher.exe" );
          if ( File.Exists( scLauncher ) ) {
            return scLauncher;
          }
          else {
            log.WarnFormat( "SCLauncherFile1 - file does not exist: {0}", scLauncher );
            return "";
          }
        }
        log.Warn( "SCLauncherFile1 - did not found HKLM - AppPath - Launcher.exe" );
        return "";
      }
    }

    /// <summary>
    /// Try to locate the launcher under "Uninstall"
    /// </summary>
    static private String SCLauncherFile2
    {
      get
      {
        log.Debug( "SCLauncherFile2 - Entry" );
        String scLauncher = ( String )Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StarCitizen", "DisplayIcon", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherFile2 - Found HKLM - Uninstall - StarCitizen" );
          if ( File.Exists( scLauncher ) ) {
            return scLauncher;
          }
          else {
            log.WarnFormat( "SCLauncherFile2 - file does not exist: {0}", scLauncher );
            return "";
          }
        }
        log.Warn( "SCLauncherFile2 - did not found HKLM - Uninstall - StarCitizen" );
        return "";
      }
    }


    /// <summary>
    /// Try to locate the launcher under "Uninstall"
    /// </summary>
    static private String SCLauncherFile3
    {
      get
      {
        log.Debug( "SCLauncherFile3 - Entry" );
        String scLauncher = ( String )Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Cloud Imperium Games\StarCitizen Launcher.exe", "", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherFile3 - Found HKLM - CIG - Launcher.exe" );
          if ( File.Exists( scLauncher ) ) {
            return scLauncher;
          }
          else {
            log.WarnFormat( "SCLauncherFile3 - file does not exist: {0}", scLauncher );
            return "";
          }
        }
        log.Warn( "SCLauncherFile3 - did not found HKLM - CIG - Launcher.exe" );
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
        log.Debug( "SCBasePath - Entry" );
        appSettings.Reload( ); // local instance - reload as it might be changed outside
        String scp = "";
        if ( appSettings.UserSCPathUsed ) {
          // User has priority
          scp = appSettings.UserSCPath;
          if ( !Directory.Exists( scp ) ) {
            log.WarnFormat( "SCBasePath - user defined folder does not exist: {0}", scp );
            return ""; // sorry path does not exist
          }
        }
        else {
          // start the registry search
          scp = SCLauncherFile1;
          if ( String.IsNullOrEmpty( scp ) ) {
            scp = SCLauncherFile2;
            if ( String.IsNullOrEmpty( scp ) ) {
              scp = SCLauncherFile3;
              if ( String.IsNullOrEmpty( scp ) ) {
                log.Warn( "SCBasePath - cannot find any valid SC path" );
                return "";  // sorry did not found a thing..
              }
            }
          }
          // found the launcher.exe file - path adjust
          scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen\Launcher"
          scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
        }

        return scp;
      }
    }



    /// <summary>
    /// Returns the SC installation path or ""
    /// </summary>
    static public String SCInstallPath
    {
      get
      {
        log.Debug( "SCInstallPath - Entry" );
        return SCBasePath;
      }
    }


    /// <summary>
    /// Returns the SC Client path  e.g.  "E:\G\StarCitizen\CitizenClient"
    /// </summary>
    static public String SCClientPath
    {
      get
      {
        log.Debug( "SCClientPath - Entry" );
        String scp = SCBasePath;
        if ( String.IsNullOrEmpty( scp ) ) return ""; // no valid one can be found
        //
        scp = Path.Combine( scp, "CitizenClient" );
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientDataPath - CitizenClient subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\Data"
    /// </summary>
    static public String SCClientDataPath
    {
      get
      {
        log.Debug( "SCClientDataPath - Entry" );
        String scp = SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return ""; // no valid one can be found
        //
        scp = Path.Combine( scp, "Data" );
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientDataPath - CitizenClient.Data subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\USER"
    /// </summary>
    static public String SCClientUSERPath
    {
      get
      {
        log.Debug( "SCClientUSERPath - Entry" );
        String scp = SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "USER" );
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientUSERPath - CitizenClient.USER subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\logs"
    /// </summary>
    static public String SCClientLogsPath
    {
      get
      {
        log.Debug( "SCClientLogsPath - Entry" );
        String scp = SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "logs" );
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientUSERPath - CitizenClient.logs subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path  e.g.  "E:\G\StarCitizen\CitizenClient\Data\Controls\Mappings"
    /// </summary>
    static public String SCClientMappingPath
    {
      get
      {
        log.Debug( "SCClientMappingPath - Entry" );
        String scp = SCClientUSERPath; // AC1.03 new here
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "Controls" );
        scp = Path.Combine( scp, "Mappings" );
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientMappingPath - CitizenClient.USER.Controls.Mappings subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC GameData.pak file path  e.g.  "E:\G\StarCitizen\CitizenClient\Data\GameData.pak"
    /// </summary>
    static public String SCGameData_pak
    {
      get
      {
        log.Debug( "SCGameData_pak - Entry" );
        String scp = SCClientDataPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "GameData.pak" );
        if ( File.Exists( scp ) ) return scp;

        log.WarnFormat( "SCGameData_pak - CitizenClient.Data.GameData.pak file does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC log file path to the latest logfile  e.g.  "E:\G\StarCitizen\CitizenClient\logs\2014-12-22_17-53-01_Log_0.log"
    /// </summary>
    static public String SCLastLog
    {
      get
      {
        log.Debug( "SCLastLog - Entry" );
        String scp = SCClientLogsPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        try {
          var files = Directory.EnumerateFiles( scp, "*.log", SearchOption.TopDirectoryOnly );
          DateTime newestT = DateTime.FromFileTime( 1 ); // very old...
          String   newestF = "";
          foreach ( String f in files ) {
            try {
              FileInfo finfo = new FileInfo( f );
              if ( finfo.LastWriteTime > newestT ) {
                newestF = f; newestT = finfo.LastWriteTime;
              }
            }
            catch {
            }
          }
          return newestF;
        }
        catch {
        }
        return "";
      }
    }


    /// <summary>
    /// Returns the relative path of DefaultProfile.xml
    /// </summary>
    static public String DefaultProfilePath_rel
    {
      get
      {
        log.Debug( "DefaultProfilePath_rel - Entry" );
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
        log.Debug( "DefaultProfileName - Entry" );
        return @"defaultProfile";
      }
    }


  }
}
