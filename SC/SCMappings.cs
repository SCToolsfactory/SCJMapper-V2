using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SCJMapper_V2.SC
{
  /// <summary>
  /// Finds and returns the Mappings from SC Bundle
  /// it is located in the Mappings Path
  /// </summary>
  class SCMappings
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public const string c_MapStartsWith = "layout_";  // we only allow those mapping names
    private const string c_UserMapStartsWith = c_MapStartsWith + "my_";  // we only allow those mapping names
    private const string c_ExportedMapEndsWith = "_exported";  // we only allow those mapping names

    static private List<string> m_scMappings = new List<string>( );
    static private SCGameMaps m_scGameMaps = new SCGameMaps( ); // only one instance allowed... else we read it multiple times from the pak file

    /// <summary>
    /// Returns true if a mapping file exists
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>True if the file exists</returns>
    static public bool MappingFileExists( string mapName )
    {
      bool retVal = false;
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
    static public string MappingFileName( string mapName )
    {
      return Path.Combine( SCPath.SCClientMappingPath, mapName + ".xml" );
    }


    /// <summary>
    /// Returns true if a mapping name is considered a user mapping
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>True if it is a user mapping name</returns>
    static public bool IsUserMapping( string mapName )
    {
      return Path.GetFileNameWithoutExtension( mapName ).StartsWith( c_UserMapStartsWith );
    }

    /// <summary>
    /// Returns true if a mapping name is considered an exported mapping
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>True if it is an exported mapping name</returns>
    static public bool IsExportedMapping( string mapName )
    {
      return Path.GetFileNameWithoutExtension( mapName ).StartsWith( c_MapStartsWith )
          && Path.GetFileNameWithoutExtension( mapName ).EndsWith( c_ExportedMapEndsWith );
    }

    /// <summary>
    /// Check if we may use that name - we allow only names like "layout_my_XYZ" 
    /// </summary>
    /// <param name="mapName">A map name</param>
    /// <returns>True if valid</returns>
    static public bool IsValidMappingName( string mapName )
    {
      bool retVal = true; // for now
      retVal &= mapName.StartsWith( c_UserMapStartsWith );
      retVal &= ( mapName.IndexOfAny( new char[] { ' ', '\t', '\n', '\r', '\0' } ) < 0 ); // make sure we don't have spaces etc.
      return retVal;
    }


    static public void UpdateMappingNames()
    {
      if ( Directory.Exists( SCPath.SCClientMappingPath ) ) {
        m_scMappings.Clear( );
        m_scMappings = Directory.EnumerateFiles( SCPath.SCClientMappingPath ).ToList( );
        foreach ( KeyValuePair<string, string> kv in m_scGameMaps ) {
          m_scMappings.Insert( 0, kv.Key ); // insert before others
        }
      }
      else {
        log.Warn( "UpdateMappingNames - cannot find SC Mapping directory" );
      }
    }


    /// <summary>
    /// Returns a list of files found 
    /// </summary>
    /// <returns>A list of filenames - can be empty</returns>
    static public List<string> MappingNames
    {
      get {
        log.Debug( "MappingNames - Entry" );
        if ( m_scMappings.Count == 0 ) {
          UpdateMappingNames( );
        }
        return m_scMappings;
      }
    }

    /// <summary>
    /// Returns the sought mapping
    /// </summary>
    /// <param name="mappingName">The filename of the profile to be extracted</param>
    /// <returns>A string containing the file contents</returns>
    static public string Mapping( string mappingName )
    {
      string retVal = "";
      // first check for exported as they may start with user mapping pattern as well
      if ( IsExportedMapping( mappingName ) ) { 
        string mFile = Path.Combine( SCPath.SCClientMappingPath, ( mappingName + ".xml" ) );
        if ( File.Exists( mFile ) ) {
          using ( var sr = new StreamReader( mFile ) ) {
            retVal = sr.ReadToEnd( );
          }
        }
      }
      else if ( IsUserMapping( mappingName ) ) {
        string mFile = Path.Combine( SCPath.SCClientMappingPath, ( mappingName + ".xml" ) );
        if ( File.Exists( mFile ) ) {
          using ( var sr = new StreamReader( mFile ) ) {
            retVal = sr.ReadToEnd( );
          }
        }
      }
      else {
        // CIG provided game mapping
        if ( m_scGameMaps.ContainsKey( mappingName ) ) {
          retVal = m_scGameMaps[mappingName];
        }
      }
      return retVal;
    }

  }
}
