using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

using SCJMapper_V2.CryXMLlib;

namespace SCJMapper_V2.SC
{

  /// <summary>
  /// Finds and returns the DefaultProfile from SC GameData.pak
  /// it is located in GameData.pak \Libs\Config
  /// </summary>
  class SCDefaultProfile
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    /// <summary>
    /// Ref to the used defaultProfile to inform the user about it.
    /// </summary>
    public static String UsedDefProfile = "n.a.";

    /// <summary>
    /// Returns a list of files found that match 'defaultProfile*.xml'
    /// 20151220BM: return only the single defaultProfile name
    /// </summary>
    /// <returns>A list of filenames - can be empty</returns>
    static public String DefaultProfileName
    {
      get { return "defaultProfile.xml"; }
    }

    /// <summary>
    /// Returns the sought default profile as string from various locations
    /// SC Alpha 2.2: Have to find the new one in E:\G\StarCitizen\StarCitizen\Public\Data\DataXML.pak (contains the binary XML now)
    /// </summary>
    /// <param name="defaultProfileName">The filename of the profile to be extracted </param>
    /// <returns>A string containing the file contents</returns>
    static public String DefaultProfile( String defaultProfileName )
    {
      log.Debug( "DefaultProfile - Entry" );

      String retVal = "";

        // first choice a defaultProfile.xml in the app dir distributed with the application ??? to be deleted ???
      if ( File.Exists( SCPath.DefaultProfileName ) ) {
        using ( StreamReader sr = new StreamReader( SCPath.DefaultProfileName ) ) {
          retVal = sr.ReadToEnd( );
          UsedDefProfile = "AppDirectory defaultProfile";
          log.InfoFormat( "- Use {0}", UsedDefProfile );
          
          return retVal; // EXIT
        }
      }

      // second try to get the SC defaultProfile from the GameData.pak
      retVal = ExtractDefaultBinProfile( defaultProfileName );
      if ( !String.IsNullOrEmpty( retVal ) ) {
        UsedDefProfile = "DataXML defaultProfile";
        log.InfoFormat( "- Use {0}", UsedDefProfile );
        return retVal; // EXIT
      }

      // third try to get the SC defaultProfile from the GameData.pak
      retVal = ExtractDefaultProfile( defaultProfileName );
      if ( !String.IsNullOrEmpty( retVal ) ) {
        UsedDefProfile = "GamePack defaultProfile";
        log.InfoFormat( "- Use {0}", UsedDefProfile );
        return retVal; // EXIT
      }

      // last resort is the built in one
      retVal = SCJMapper_V2.Properties.Resources.defaultProfile;
      UsedDefProfile = "App Resource defaultProfile";
      log.InfoFormat( "- Use {0}", UsedDefProfile );
      return retVal; // EXIT
    }


    /// <summary>
    /// Zip Extracts the file to a string
    /// SC Alpha 2.2: Have to find the new one in E:\G\StarCitizen\StarCitizen\Public\Data\DataXML.pak (contains the binary XML now)
    /// </summary>
    static private String ExtractDefaultBinProfile( String defaultProfileName )
    {
      log.Debug( "ExtractDefaultBinProfile - Entry" );

      String retVal = "";
      if ( File.Exists( SCPath.SCDataXML_pak ) ) {
        using ( ZipFile zip = ZipFile.Read( SCPath.SCDataXML_pak ) ) {
          zip.CaseSensitiveRetrieval = false;
          try {
            ICollection<ZipEntry> gdpak = zip.SelectEntries( "name = " + "'" + defaultProfileName + "'", SCPath.DefaultProfilePath_rel );
            if ( gdpak != null ) {
              try {
                MemoryStream mst = new MemoryStream( );
                gdpak.FirstOrDefault( ).Extract( mst );
                // use the binary XML reader
                CryXmlNodeRef ROOT = null;
                CryXmlBinReader.EResult readResult =  CryXmlBinReader.EResult.Error;
                CryXmlBinReader cbr = new CryXmlBinReader( );

                ROOT = cbr.LoadFromBuffer( mst.ToArray( ), out readResult );
                if ( readResult == CryXmlBinReader.EResult.Success ) {
                  XmlTree tree = new XmlTree( );
                  tree.BuildXML( ROOT );
                  retVal = tree.XML_string;
                }
                else {
                  log.ErrorFormat( "  Error in CryXmlBinReader: {0}", cbr.GetErrorDescription() );
                  retVal = ""; // clear any remanents
                }
              }
              catch {
                retVal = ""; // clear any remanents
              }
            }
          }
          catch ( Exception ex ) {
            log.Error( "  Unexpected ", ex );
          }

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
            log.Error( "  Unexpected", ex );
          }

        }
      }
      return retVal;
    }


  }
}
