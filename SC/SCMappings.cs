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
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public  const String c_MapStartsWith = "layout_";  // we only allow those mapping names
    private const String c_UserMapStartsWith =  c_MapStartsWith + "my_";  // we only allow those mapping names

    static private List<String> m_scMappings = new List<string>( );


    /// <summary>
    /// Returns true if a mapping file exists
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>True if the file exists</returns>
    static public Boolean MappingFileExists( String mapName )
    {
      Boolean retVal = false;
      if ( Directory.Exists( SCPath.SCClientMappingPath ) ) {
        retVal = File.Exists( Path.Combine( SCPath.SCClientMappingPath, mapName + ".xml" ) );
      }
      return retVal;
    }

    /// <summary>
    /// Returns the mapping file name + path into our user MAPPING dir (TODO possibly to change??)
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>A fully qualified filename</returns>
    static public String MappingFileName( String mapName )
    {
      return Path.Combine( SCPath.SCClientMappingPath, mapName + ".xml" );
    }


    /// <summary>
    /// Returns true if a mapping name is considered a user mapping
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>True if it is a user mapping name</returns>
    static public Boolean IsUserMapping( String mapName )
    {
      return mapName.StartsWith( c_UserMapStartsWith );
    }

    /// <summary>
    /// Check if we may use that name - we allow only names like "layout_my_XYZ" 
    /// </summary>
    /// <param name="mapName">A map name</param>
    /// <returns>True if valid</returns>
    static public Boolean IsValidMappingName( String mapName )
    {
      Boolean retVal = true; // for now
      retVal &= mapName.StartsWith( c_UserMapStartsWith );
      retVal &= ( mapName.IndexOfAny( new char[] { ' ', '\t', '\n', '\r', '\0' } ) < 0 ); // make sure we don't have spaces etc.
      return retVal;
    }


    static public void UpdateMappingNames( )
    {
      if ( Directory.Exists( SCPath.SCClientMappingPath ) ) {
        m_scMappings.Clear( );
        m_scMappings = ( List<String> )Directory.EnumerateFiles( SCPath.SCClientMappingPath ).ToList( );
      }
      else {
        log.Warn( "UpdateMappingNames - cannot find SC Mapping directory" );
      }
    }


    /// <summary>
    /// Returns a list of files found 
    /// </summary>
    /// <returns>A list of filenames - can be empty</returns>
    static public List<String> MappingNames
    {
      get
      {
        log.Debug( "MappingNames - Entry" );
        if ( m_scMappings.Count == 0 ) {
          UpdateMappingNames( );
        }
        return m_scMappings;
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
