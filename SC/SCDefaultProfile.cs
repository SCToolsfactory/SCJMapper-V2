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

    static public String DefaultProfile( )
    {
      String retVal = "";

      // first try to get the SC defaultProfile from the GameData.pak
      retVal = ExtractDefaultProfile( );
      if ( String.IsNullOrEmpty( retVal ) ) {
        // second choice a defaultProfile.xml in the app dir distributed with the application ??? to be deleted ???
        if ( File.Exists( SCPath.DefaultProfileName ) ) {
          using ( StreamReader sr = new StreamReader( SCPath.DefaultProfileName ) ) {
            retVal = sr.ReadToEnd( );
          }
        }
        // last resort is the built in one
        else {
          retVal = SCJMapper_V2.Properties.Resources.defaultProfile;
        }
      }
      return retVal;
    }

    /// <summary>
    /// Extracts the file to the internal string
    /// </summary>
    static private String ExtractDefaultProfile( )
    {
      String retVal = "";
      if ( File.Exists( SCPath.SCGameData_pak ) ) {
        using ( ZipFile zip = ZipFile.Read( SCPath.SCGameData_pak ) ) {
          zip.CaseSensitiveRetrieval = false;
          ICollection<ZipEntry> gdpak = zip.SelectEntries( "name = " + SCPath.DefaultProfileName, SCPath.DefaultProfilePath_rel );
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
      }
      return retVal;
    }


  }
}
