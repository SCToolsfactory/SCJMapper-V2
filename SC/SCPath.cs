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
    /// Try to locate the launcher from Alpha 3.0.0 public - e.g. E:\G\StarCitizen\RSI Launcher
    /// </summary>
    static private String SCLauncherDir6
    {
      get {
        log.Debug( "SCLauncherDir6 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\81bfc699-f883-50c7-b674-2483b6baae23", "InstallLocation", null );
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
    static private String SCLauncherDir5
    {
      get {
        log.Debug( "SCLauncherDir5 - Entry" );
        String scLauncher = (String)Registry.GetValue( @"HKEY_LOCAL_MACHINE\SOFTWARE\94a6df8a-d3f9-558d-bb04-097c192530b9", "InstallLocation", null );
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
    /// Returns the base SC install path from something like "E:\G\StarCitizen"
    /// </summary>
    static private String SCBasePath
    {
      get {
        log.Debug( "SCBasePath - Entry" );
        appSettings.Reload( ); // local instance - reload as it might be changed outside
        String scp = "";

        // User setting has Prio
        if ( appSettings.UserSCPathUsed ) {
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
    /// SC 3.0.0: search path like  E:\G\StarCitizen\StarCitizen\LIVE 
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
        // SC 3.0 try LIVE 
        scpX = Path.Combine( scp, "LIVE" );
        if ( Directory.Exists( scpX ) ) return scpX;

        // Issue a warning here to let the user know
        issue = string.Format( "Cannot find the SC Client Path !!\n\n" +
        "Tried to look for:\n" +
        "{0}\\LIVE \n" +
        "The program cannot load or save in GameFolders\n\n" +
        "Please submit a bug report, adding your complete SC game folder structure", scp );

        log.WarnFormat( "SCClientPath - StarCitizen\\Live subfolder does not exist: {0}", scp );

        // Issue a warning here to let the user know
        if ( !hasInformed ) System.Windows.Forms.MessageBox.Show( issue, "Cannot find SC Client Path !!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation );
        hasInformed = true;

        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 3.0: E:\G\StarCitizen\StarCitizen\LIVE\Data
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

        log.WarnFormat( "SCClientDataPath - StarCitizen\\LIVE\\Data subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC ClientData path
    /// AC 3.0: E:\G\StarCitizen\StarCitizen\LIVE\USER
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

        log.WarnFormat( "SCClientUSERPath - StarCitizen\\LIVE\\USER subfolder does not exist: {0}", scp );
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

        log.WarnFormat( "SCClientLogsPath - StarCitizen\\LIVE subfolder does not exist: {0}", scp );
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

        log.WarnFormat( "SCClientMappingPath - StarCitizen\\LIVE\\USER\\Controls\\Mappings subfolder does not exist: {0}", scp );
        return "";
      }
    }


    /// <summary>
    /// Returns the SC Data.p4k file path
    /// SC Alpha 3.0: E:\G\StarCitizen\StarCitizen\LIVE\Data.p4k (contains the binary XML now)
    /// </summary>
    static public String SCData_p4k
    {
      get {
        log.Debug( "SCDataXML_p4k - Entry" );
        String scp =  SCClientPath;
        if ( String.IsNullOrEmpty( scp ) ) return "";
        //
        scp = Path.Combine( scp, "Data.p4k" );
#if DEBUG
        //***************************************
        // scp += "X"; // TEST not found (COMMENT OUT FOR PRODUCTIVE BUILD)
        //***************************************
#endif
        if ( File.Exists( scp ) ) return scp;

        log.WarnFormat( "SCData_p4k - StarCitizen\\Public\\Data\\Data.p4k file does not exist: {0}", scp );
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
