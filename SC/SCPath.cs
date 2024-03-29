﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using SCJMapper_V2.Translation;

namespace SCJMapper_V2.SC
{
  /// <summary>
  /// Find the SC pathes and folders
  /// </summary>
  class SCPath
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private static bool m_hasInformed = false; // prevent msgbox chains..

    /// <summary>
    /// Try to locate the launcher from Alpha 3.0.0 public - e.g. E:\G\StarCitizen\RSI Launcher
    /// Alpha 3.6.0 PTU launcher 1.2.0 has the same entry (but PTU location changed)
    /// </summary>
    static private string SCLauncherDir6 {
      get {
        log.Debug( "SCLauncherDir6 - Entry" );
        string scLauncher = (string)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\81bfc699-f883-50c7-b674-2483b6baae23", "InstallLocation", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherDir6 - Found HKLM -InstallLocation" );
          if ( Directory.Exists( scLauncher ) ) {
            return scLauncher;
          }
          else {
            log.WarnFormat( "SCLauncherDir6 - directory does not exist: {0}", scLauncher );
            return "";
          }
        }
        log.Warn( "SCLauncherDir6 - did not found HKLM - InstallLocation" );
        return "";
      }
    }

    /// <summary>
    /// Try to locate the launcher from Alpha 3.0.0 PTU - e.g. E:\G\StarCitizen\RSI PTU Launcher
    /// </summary>
    static private string SCLauncherDir5 {
      get {
        log.Debug( "SCLauncherDir5 - Entry" );
        string scLauncher = (string)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\94a6df8a-d3f9-558d-bb04-097c192530b9", "InstallLocation", null );
        if ( scLauncher != null ) {
          log.Info( "SCLauncherDir5 - Found HKLM -InstallLocation (PTU)" );
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
    /// Checks if the base path is correct - i.e. the subfolders can be found
    /// Changed for Patcher 1.2.0 again
    /// </summary>
    /// <param name="basePath"></param>
    /// <returns>An empty string if OK - else the issue found</returns>
    static public string CheckSCBasePath( string basePath )
    {
      string issue = "";

      if ( string.IsNullOrEmpty( basePath ) ) {
        issue = Tx.Translate( "scpEmptyString" );  // string.Format( "There is no vaild path given (empty string)" );
        return issue; // no valid one can be found
      }

      if ( !Directory.Exists( basePath ) ) {
        issue = Tx.Translate( "scpInvalidPath" ); // string.Format( "There is no vaild path given (invalid directory)" );
        return issue; // no valid one can be found
      }
      // 20180321 New PTU 3.1 another change in setup path - Testing for PTU first 
      // 20190711 Lanuncher 1.2 - PTU has moved - change detection to find this one first.
      basePath = Path.Combine( basePath, "StarCitizen" );

      string scpX = "";
      scpX = Path.Combine( basePath, "PTU" );
      if ( Directory.Exists( scpX ) ) {
        return ""; // OK at least PTU folder exists - seems legit
      }
      else {
        // may be there is only LIVE ?
        scpX = Path.Combine( basePath, "LIVE" );
        if ( Directory.Exists( scpX ) ) {
          return ""; // OK LIVE folder exists - seems legit
        }
        // for now it failed
        issue = string.Format( Tx.Translate( "scpClientDirNotFound" ).Replace( "\\n", "\n" ), scpX );
        //"Cannot find the SC Client Directory !!\n\nTried to look for:\n{0} \n\nPlease adjust the path in Settings\n"
      }

      // last resort is old style PTU only
      // This would be pre 1.2 laucher and PTU only
      basePath += "PTU";  // makes it "StarCitizenPTU"
      if ( Directory.Exists( basePath ) ) {
        return ""; // OK at least old PTU folder exists - seems legit
      }

      return issue; // OK exit 
    }

    /// <summary>
    /// Returns the base SC install path from something like "E:\G\StarCitizen"
    /// </summary>
    static private string SCBasePath {
      get {
        log.Debug( "SCBasePath - Entry" );
        AppSettings.Instance.Reload( ); // local instance - reload as it might be changed outside
        string scp = "";

        // User setting has Prio
        if ( AppSettings.Instance.UserSCPathUsed ) {
          scp = AppSettings.Instance.UserSCPath;
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
          string issue = string.Format( Tx.Translate( "scpClientDirNotFound" ).Replace( "\\n", "\n" ), scp );

          if ( !m_hasInformed )
            System.Windows.Forms.MessageBox.Show( issue, Tx.Translate( "setMsgBox" ),
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
          m_hasInformed = true;
          return ""; // sorry path does not exist

        }
        else {
          // start the registry search - sequence  5..1 to get the newest method first

          scp = SCLauncherDir6; // 3.0 Public Launcher
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          scp = SCLauncherDir5; // 3.0 PTU Launcher
#if DEBUG
          //***************************************
          //scp = ""; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
          //***************************************
#endif
          if ( !string.IsNullOrEmpty( scp ) ) {
            scp = Path.GetDirectoryName( scp );  // "E:\G\StarCitizen"
            return scp;
          }

          // nothing found
          log.Warn( "SCBasePath - cannot find any valid SC path" );
          // Issue a warning here to let the user know
          string issue = Tx.Translate( "scpAutoPathFailed" ).Replace( "\\n", "\n" );
          //string.Format( "Cannot find the SC Installation Path !!\nUse Settings to provide the path manually" );

          if ( !m_hasInformed )
            System.Windows.Forms.MessageBox.Show( issue, Tx.Translate( "setMsgBox" ),
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
          m_hasInformed = true;
        }
        return "";  // sorry did not found a thing..
      }// get

    }



    /// <summary>
    /// Returns the SC installation path or ""
    /// </summary>
    static public string SCInstallPath {
      get {
        log.Debug( "SCInstallPath - Entry" );
        return SCBasePath;
      }
    }



    /// <summary>
    /// Returns the SC Client path  
    /// SC 3.0.0: search path like  E:\G\StarCitizen\StarCitizen\LIVE 
    /// </summary>
    static public string SCClientPath {
      get {
        log.Debug( "SCClientPath - Entry" );
        string scp = SCBasePath;
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        string issue = "";

        if ( string.IsNullOrEmpty( scp ) ) return ""; // no valid one can be found


        // 20180321 New PTU 3.1 another change in setup path - Testing for PTU first 
        // 20190711 Lanuncher 1.2 - PTU has moved - change detection to find this one first.
        if ( AppSettings.Instance.UsePTU ) {
          if ( Directory.Exists( Path.Combine( scp, @"StarCitizen\PTU" ) ) ) {
            scp = Path.Combine( scp, @"StarCitizen\PTU" );
          }
          else if ( Directory.Exists( Path.Combine( scp, @"StarCitizenPTU\LIVE" ) ) ) {
            // this would be old style
            scp = Path.Combine( scp, @"StarCitizenPTU\LIVE" );
          }
        }
        else {
          // no PTU ..
          scp = Path.Combine( scp, @"StarCitizen\LIVE" );
        }
        if ( Directory.Exists( scp ) ) return scp; // EXIT Success

        log.WarnFormat( "SCClientPath - StarCitizen\\LIVE or PTU subfolder does not exist: {0}", scp );
        // Issue a warning here to let the user know
        issue = string.Format( Tx.Translate( "scpClientDirNotFound" ).Replace( "\\n", "\n" ), scp );
        //"Cannot find the SC Client Directory !!\n\nTried to look for:\n{0} \n\nPlease adjust the path in Settings\n"
        // Issue a warning here to let the user know
        if ( !m_hasInformed ) System.Windows.Forms.MessageBox.Show( issue, Tx.Translate( "setMsgBox" ), System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
        m_hasInformed = true;

        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 3.0: E:\G\StarCitizen\StarCitizen\LIVE\Data
    /// </summary>
    static public string SCClientDataPath {
      get {
        log.Debug( "SCClientDataPath - Entry" );
        string scp = SCClientPath;
        if ( string.IsNullOrEmpty( scp ) ) return ""; // no valid one can be found

        scp = Path.Combine( scp, "Data" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientDataPath - StarCitizen\\LIVE\\Data subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 3.0: E:\G\StarCitizen\StarCitizen\LIVE\USER
    /// AC 3.13: E:\G\StarCitizen\StarCitizen\LIVE\USER\Client\0
    /// </summary>
    static public string SCClientUSERPath {
      get {
        log.Debug( "SCClientUSERPath - Entry" );
        string scp = SCClientPath;
        if ( string.IsNullOrEmpty( scp ) ) return "";
        //
        string scpu = Path.Combine( scp, "USER", "Client", "0" ); // 20210404 new path
        if ( !Directory.Exists( scpu ) ) {
          scpu = Path.Combine( scp, "USER" ); // 20210404 old path
        }

#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scpu ) ) return scpu;

        log.WarnFormat( @"SCClientUSERPath - StarCitizen\\LIVE\\USER[\Client\0] subfolder does not exist: {0}", scpu );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public
    /// AC 3x: E:\G\StarCitizen\StarCitizen\LIVE or PTU
    /// </summary>
    static public string SCClientLogsPath {
      get {
        log.Debug( "SCClientLogsPath - Entry" );
        string scp = SCClientPath;
        if ( string.IsNullOrEmpty( scp ) ) return "";
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( "SCClientLogsPath - StarCitizen\\LIVE or PTU subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\USER\Controls\Mappings
    /// </summary>
    static public string SCClientMappingPath {
      get {
        log.Debug( "SCClientMappingPath - Entry" );
        string scp = SCClientUSERPath; // AC1.03 new here
        if ( string.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "Controls", "Mappings");
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( Directory.Exists( scp ) ) return scp;

        log.WarnFormat( @"SCClientMappingPath - StarCitizen\LIVE\USER\[Client\0\]Controls\Mappings subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC Data.p4k file path
    /// SC Alpha 3.0: E:\G\StarCitizen\StarCitizen\LIVE\Data.p4k (contains the binary XML now)
    /// </summary>
    static public string SCData_p4k {
      get {
        log.Debug( "SCDataXML_p4k - Entry" );
        string scp = SCClientPath;
        if ( string.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "Data.p4k" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( File.Exists( scp ) ) return scp;

        log.WarnFormat( @"SCData_p4k - StarCitizen\LIVE or PTU\Data\Data.p4k file does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC log file path to the latest logfile
    /// AC 1.1.6: E:\G\StarCitizen\StarCitizen\Public\Game.log  NOTE: 1.1.6 does not longer contain the needed entries .-((
    /// </summary>
    static public string SCLastLog {
      get {
        log.Debug( "SCLastLog - Entry" );
        string scp = SCClientLogsPath;
        if ( string.IsNullOrEmpty( scp ) ) return "";
        //
        try {
          var files = Directory.EnumerateFiles( scp, "*.log", SearchOption.TopDirectoryOnly );
          DateTime newestT = DateTime.FromFileTime( 1 ); // very old...
          string newestF = "";
          foreach ( string f in files ) {
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
    static public string DefaultProfilePath_rel {
      get {
        log.Debug( "DefaultProfilePath_rel - Entry" );
        return @"Libs\Config";
      }
    }

    /// <summary>
    /// Returns the name part of the DefaultProfile w/o extension...
    /// SC Alpha 2.2: still true .. but contains the binary XML now
    /// </summary>
    static public string DefaultProfileName {
      get {
        log.Debug( "DefaultProfileName - Entry" );
        return @"defaultProfile";
      }
    }

    /// <summary>
    /// Returns a summary of the detected pathes
    /// </summary>
    /// <returns>A formatted string</returns>
    static public string Summary( )
    {
      string ret = $"SC Path:\n";

      ret += $"  InstallPath:\t{SCInstallPath}\n";
      ret += $"  BasePath:\t\t{SCBasePath}\n";
      ret += $"  Client:\t\t{SCClientPath}\n";
      ret += $"  ClientData:\t\t{SCClientDataPath}\n";
      ret += $"  ClientUSER:\t\t{SCClientUSERPath}\n";
      ret += $"  ClientMapping:\t{SCClientMappingPath}\n";

      return ret;
    }



  }
}
