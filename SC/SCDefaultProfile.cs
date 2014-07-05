using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace SCJMapper_V2
{

  /// <summary>
  /// Finds and returns the DefaultProfile from SC GameData.pak
  /// it is located in GameData.pak \Libs\Config
  /// </summary>
  class SCDefaultProfile
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    /// <summary>
    /// Returns a list of files found that match 'defaultProfile*.xml'
    /// </summary>
    /// <returns>A list of filenames - can be empty</returns>
    static public List<String> DefaultProfileNames
    {
      get
      {
        log.Debug( "DefaultProfileNames - Entry" );
        List<String> retVal = new List<String>( );
        if ( File.Exists( SCPath.SCGameData_pak ) ) {
          using ( ZipFile zip = ZipFile.Read( SCPath.SCGameData_pak ) ) {
            try {
              zip.CaseSensitiveRetrieval = false;
              ICollection<ZipEntry> gdpak = zip.SelectEntries( "name = " + "'" + SCPath.DefaultProfileName + "*.xml'", SCPath.DefaultProfilePath_rel );
              if ( gdpak != null ) {
                foreach ( ZipEntry ze in gdpak ) {
                  retVal.Add( ze.FileName );
                }
              }
            }
            catch ( Exception ex ) {
              log.Error( "Unexpected", ex );
            }
          }
        }
        return retVal;
      }
    }

    /// <summary>
    /// Returns the sought default profile as string from GameData.pak
    /// </summary>
    /// <param name="defaultProfileName">The filename of the profile to be extracted</param>
    /// <returns>A string containing the file contents</returns>
    static public String DefaultProfile( String defaultProfileName )
    {
      log.Debug( "DefaultProfile - Entry" );

      String retVal = "";

      // first choice a defaultProfile.xml in the app dir distributed with the application ??? to be deleted ???
      if ( File.Exists( SCPath.DefaultProfileName + ".xml" ) ) {
        using ( StreamReader sr = new StreamReader( SCPath.DefaultProfileName + ".xml" ) ) {
          retVal = sr.ReadToEnd( );
          log.Info( "- Use AppDirectory defaultProfile" );
        }
      }
      else {
        // second try to get the SC defaultProfile from the GameData.pak
        retVal = ExtractDefaultProfile( defaultProfileName );
        if ( ! String.IsNullOrEmpty( retVal ) ) {
          log.Info( "- Use GamePack defaultProfile" );
        }
        else {
          // last resort is the built in one
          retVal = SCJMapper_V2.Properties.Resources.defaultProfile;
          log.Info( "- Use built in defaultProfile" );
        }
      }
      return retVal;
    }


    /// <summary>
    /// Zip Extracts the file to a string
    /// </summary>
    static private String ExtractDefaultProfile( String defaultProfileName )
    {
      log.Debug( "ExtractDefaultProfile - Entry" );

      String retVal = "";
      if ( File.Exists( SCPath.SCGameData_pak ) ) {
        using ( ZipFile zip = ZipFile.Read( SCPath.SCGameData_pak ) ) {
          zip.CaseSensitiveRetrieval = false;
          try {
            ICollection<ZipEntry> gdpak = zip.SelectEntries( "name = " + "'" + defaultProfileName + "'", SCPath.DefaultProfilePath_rel );
            if ( gdpak != null ) {
              try {
                MemoryStream mst = new MemoryStream( );
                gdpak.FirstOrDefault( ).Extract( mst );
                UTF8Encoding unc = new UTF8Encoding( );
                retVal = unc.GetString( mst.ToArray( ) );
              }
              catch {
                retVal = ""; // clear any remanents
              }
            }
          }
          catch ( Exception ex ) {
            log.Error( "Unexpected", ex );
          }

        }
      }
      return retVal;
    }


  }
}
