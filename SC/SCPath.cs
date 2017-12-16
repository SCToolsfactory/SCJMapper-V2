using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace SCJMapper_V2.SC
{
  /// <summary>
  /// Find the SC pathes and folders
  /// </summary>
  class SCPath
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );
    private static readonly AppSettings appSettings = new AppSettings( );

    private static bool hasInformed = false; // prevent msgbox chains..

    /// <summary>
    /// Try to locate the launcher under "App Paths"
    /// </summary>
    static private String SCLauncherFile1
    {
      get {
        log.Debug( "SCLauncherFile1 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths\StarCitizen Launcher.exe", "", null );
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
      get {
        log.Debug( "SCLauncherFile2 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StarCitizen", "DisplayIcon", null );
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
      get {
        log.Debug( "SCLauncherFile3 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Cloud Imperium Games\StarCitizen Launcher.exe", "", null );
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
    /// Try to locate the launcher from Alpha 1.1.6  - e.g. E:\G\StarCitizen\CIGLauncher.exe
    /// </summary>
    static private String SCLauncherFile4
    {
      get {
        log.Debug( "SCLauncherFile4 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\CIGLauncher.exe", "", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherFile4 - Found HKCU - CIGLauncher.exe" );
          if ( File.Exists( scLauncher ) ) {
            return scLauncher;
          }
          else {
            log.WarnFormat( "SCLauncherFile4 - file does not exist: {0}", scLauncher );
            return "";
          }
        }
        log.Warn( "SCLauncherFile4 - did not found HKCU - CIGLauncher.exe" );
        return "";
      }
    }

    /// <summary>
    /// Try to locate the launcher from Alpha 3.0.0 PTU - e.g. E:\G\StarCitizen\RSI PTU Launcher
    /// </summary>
    static private String SCLauncherDir5
    {
      get {
        log.Debug( "SCLauncherDir5 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\94a6df8a-d3f9-558d-bb04-097c192530b9", "InstallLocation", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherDir5 - Found HKLM -InstallLocation" );
          if ( Directory.Exists( scLauncher ) ) {
            return scLauncher;
          }
          else {
            log.WarnFormat( "SCLauncherDir5 - directory does not exist: {0}", scLauncher );
            return "";
          }
        }
        log.Warn( "SCLauncherDir5 - did not found HKLM - InstallLocation" );
        return "";
      }
    }

    // one more would be here



    /// <summary>
    /// Returns the base SC install path from something like "E:\G\StarCitizen\Launcher\StarCitizenLauncher.exe"
    /// </summary>
    static private String SCBasePath
    {
      get {
        log.Debug( "SCBasePath - Entry" );
        appSettings.Reload( ); // local instance - reload as it might be changed outside
        String scp = "";

        if ( appSettings.UserSCPathUsed ) {
          // User has priority
          scp = appSettings.UserSCPath;
          log.InfoFormat( "SCBasePath - user defined folder given: {0}", scp );
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( Directory.Exists( scp ) ) {
            return scp;
          }

          // not found
          log.WarnFormat( "SCBasePath - user defined folder does not exist: {0}", scp );

          string issue = string.Format( "Cannot find the user defined SC Installation Path ({0})!!\n\n" +
                                        "Enter the folder where CIGLauncher.exe is located", scp );

          if ( !hasInformed )
            System.Windows.Forms.MessageBox.Show( issue, "Cannot find the user defined SC Installation Path !!",
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
          hasInformed = true;
          return ""; // sorry path does not exist

        }
        else {
          // start the registry search - sequence  5..1 to get the newest method first

          scp = SCLauncherDir5;
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            // AC 1.1.6 path OK - this one needs no adjustments anymore but removing the filename
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          scp = SCLauncherFile4;
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            // AC 1.1.6 path OK - this one needs no adjustments anymore but removing the filename
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          scp = SCLauncherFile3;
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            // found the launcher.exe file - path adjust for the old subdir (may be remove path find 1..3 later)
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen\Launcher"
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          scp = SCLauncherFile2;
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            // found the launcher.exe file - path adjust for the old subdir (may be remove path find 1..3 later)
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen\Launcher"
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          scp = SCLauncherFile1;
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            // found the launcher.exe file - path adjust for the old subdir (may be remove path find 1..3 later)
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen\Launcher"
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          // nothing found
          log.Warn( "SCBasePath - cannot find any valid SC path" );
          // Issue a warning here to let the user know
          string issue = string.Format( "Cannot find the SC Installation Path !!\n\n" +
                    "Use Settings to provide the path manually (don't forget to Check the Box left of the path to use it)\n\n" +
                    "Enter the folder where CIGLauncher.exe is located" );

          if ( !hasInformed )
            System.Windows.Forms.MessageBox.Show( issue, "Cannot find SC Installation Path !!",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
          hasInformed = true;
        }
        return "";  // sorry did not found a thing..
      }// get

    }



    /// <summary>
    /// Returns the SC installation path or ""
    /// </summary>
    static public String SCInstallPath
    {
      get {
        log.Debug( "SCInstallPath - Entry" );
        return SCBasePath;
      }
    }



    /// <summary>
    /// Returns the SC Client path  
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public
    /// SC 2x: alternatively use PTU path  E:\G\StarCitizen\StarCitizen\Test
    /// SC 2.2.2: alternatively search path in  E:\G\StarCitizen\StarCitizen\Live (don't know but this was mentioned in CIGs relnotes lately)
    /// </summary>
    static public String SCClientPath
    {
      get {
        log.Debug( "SCClientPath - Entry" );
        String scp = SCBasePath;
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        string issue = "";

        if ( String.IsNullOrEmpty( scp ) ) return ""; // no valid one can be found
                                                      //
        scp = Path.Combine( scp, "StarCitizen" );
        string scpX = "";
        // regular game folder
        scpX = Path.Combine( scp, "Public" );
        if ( Directory.Exists( scpX ) ) return scpX;

        // SC 2.2.2+ did not found it so try Live now
        scpX = Path.Combine( scp, "LIVE" );
        if ( Directory.Exists( scpX ) ) return scpX;

        // Issue a warning here to let the user know
        issue = string.Format( "Cannot find the SC Client Path !!\n\n" +
        "Tried to look for:\n" +
        "{0}\\Public or \n" +
        "{0}\\LIVE \n" +
        "The program cannot load or save in GameFolders\n\n" +
        "Please submit a bug report, adding your complete SC game folder structure", scp );

        log.WarnFormat( "SCClientPath - StarCitizen\\Public, StarCitizen\\Live subfolder does not exist: {0}", scp );

        // Issue a warning here to let the user know
        if ( !hasInformed ) System.Windows.Forms.MessageBox.Show( issue, "Cannot find SC Client Path !!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
        hasInformed = true;

        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\Data
    /// </summary>
    static public String SCClientDataPath
    {
      get {
        log.Debug( "SCClientDataPath - Entry" );
        String scp = SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return ""; // no valid one can be found

        scp = Path.Combine( scp, "Data" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientDataPath - StarCitizen\\Public\\Data subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\USER
    /// </summary>
    static public String SCClientUSERPath
    {
      get {
        log.Debug( "SCClientUSERPath - Entry" );
        String scp = SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "USER" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientUSERPath - StarCitizen\\Public\\USER subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public
    /// </summary>
    static public String SCClientLogsPath
    {
      get {
        log.Debug( "SCClientLogsPath - Entry" );
        String scp = SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientLogsPath - StarCitizen\\Public subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\USER\Controls\Mappings
    /// </summary>
    static public String SCClientMappingPath
    {
      get {
        log.Debug( "SCClientMappingPath - Entry" );
        String scp = SCClientUSERPath; // AC1.03 new here
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "Controls" );
        scp = Path.Combine( scp, "Mappings" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientMappingPath - StarCitizen\\Public\\USER\\Controls\\Mappings subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC GameData.pak file path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\Data\GameData.pak
    /// </summary>
    static public String SCGameData_pak
    {
      get {
        log.Debug( "SCGameData_pak - Entry" );
        String scp = SCClientDataPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "GameData.pak" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( File.Exists( scp ) ) return scp;

        log.WarnFormat( "SCGameData_pak - StarCitizen\\Public\\Data\\GameData.pak file does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC DataXML.pak file path
    /// SC Alpha 2.2: E:\G\StarCitizen\StarCitizen\Public\Data\DataXML.pak (contains the binary XML now)
    /// </summary>
    static public String SCDataXML_pak
    {
      get {
        log.Debug( "SCDataXML_pak - Entry" );
        String scp = SCClientDataPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "DataXML.pak" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( File.Exists( scp ) ) return scp;

        log.WarnFormat( "SCDataXML_pak - StarCitizen\\Public\\Data\\DataXML.pak file does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC log file path to the latest logfile
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\Game.log  NOTE: 1.1.6 does not longer contain the needed entries .-((
    /// </summary>
    static public String SCLastLog
    {
      get {
        log.Debug( "SCLastLog - Entry" );
        String scp = SCClientLogsPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        try {
          var files = Directory.EnumerateFiles( scp, "*.log", SearchOption.TopDirectoryOnly );
          DateTime newestT = DateTime.FromFileTime( 1 ); // very old...
          String newestF = "";
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
    /// SC Alpha 2.2: still true .. but contains the binary XML now
    /// </summary>
    static public String DefaultProfilePath_rel
    {
      get {
        log.Debug( "DefaultProfilePath_rel - Entry" );
        return @"Libs\Config";
      }
    }

    /// <summary>
    /// Returns the name part of the DefaultProfile w/o extension...
    /// SC Alpha 2.2: still true .. but contains the binary XML now
    /// </summary>
    static public String DefaultProfileName
    {
      get {
        log.Debug( "DefaultProfileName - Entry" );
        return @"defaultProfile";
      }
    }


  }
}
