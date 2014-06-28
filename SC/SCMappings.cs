using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SCJMapper_V2
{
  /// <summary>
  /// Finds and returns the Mappings from SC Bundle
  /// it is located in the Mappings Path
  /// </summary>
  class SCMappings
  {

    /// <summary>
    /// Returns a list of files found 
    /// </summary>
    /// <returns>A list of filenames - can be empty</returns>
    static public List<String> MappingNames
    {
      get
      {
        List<String> retVal = new List<String>( );
        if ( Directory.Exists( SCPath.SCClientMappingPath ) ) {
          retVal = ( List<String> )Directory.EnumerateFiles( SCPath.SCClientMappingPath ).ToList();
        }
        return retVal;
      }
    }

    /// <summary>
    /// Returns the sought default profile as string from GameData.pak
    /// </summary>
    /// <param name="defaultProfileName">The filename of the profile to be extracted</param>
    /// <returns>A string containing the file contents</returns>
    static public String Mapping( String mappingName )
    {
      String retVal = "";
      String mFile = Path.Combine( SCPath.SCClientMappingPath, ( mappingName + ".xml" ) );
      if ( File.Exists( mFile ) ) {
        using ( StreamReader sr = new StreamReader( mFile ) ) {
          retVal = sr.ReadToEnd( );
        }
      }

      return retVal;
    }

  }
}
