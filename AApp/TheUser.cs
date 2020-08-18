using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SCJMapper_V2
{
  /// <summary>
  /// Provides some items that are user related - packed in one place..
  /// </summary>
  class TheUser
  {

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    // distinguish for some stuff
    public static bool UsesPTU { get; set; }

    private static bool hasWriteAccessToFolder( string folderPath )
    {
      try {
        // Attempt to get a list of security permissions from the folder. 
        // This will raise an exception if the path is read only or do not have access to view the permissions. 
        System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl( folderPath );
        return true;
      }
      catch ( UnauthorizedAccessException ) {
        return false;
      }
    }

    /// <summary>
    /// Returns the name of the Personal Program folder in My Documents (depends on PTU use...)
    /// Creates the folder if needed
    /// </summary>
    /// <returns>Path to the Personal Program directory</returns>
    static public string UserDir
    {
      get {
        log.Debug( "UserDir - Entry" );
        string docPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.Personal ), Application.ProductName );
        if ( !Directory.Exists( docPath ) ) Directory.CreateDirectory( docPath );
        if ( UsesPTU ) {
          docPath = Path.Combine( docPath, "PTU" );
          if ( !Directory.Exists( docPath ) ) Directory.CreateDirectory( docPath );
        }
        return docPath;
      }
    }

    /// <summary>
    /// The directory to store the assets (depends on PTU use...)
    /// </summary>
    static public string FileStoreDir
    {
      get {
        log.Debug( "FileStoreDir - Entry" );
        string docPath = AppDir;
        // fallback
        if ( !hasWriteAccessToFolder( docPath ) )
          docPath = UserDir;
        if ( UsesPTU )
          return Path.Combine( docPath, "PTU_Storage" );
        else
          return Path.Combine( docPath, "Storage" );
      }
    }

    
    /// <summary>
    /// The application directory
    /// </summary>
    static public string AppDir { get => Path.GetDirectoryName( Application.ExecutablePath); }


    /// <summary>
    /// Returns the mapping file name + path into our user dir
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>A fully qualified filename</returns>
    static public string MappingFileName( string mapName )
    {
      log.Debug( "MappingFileName - Entry" );

      return Path.Combine( UserDir, mapName + ".xml" );
    }

    /// <summary>
    /// Returns the Layout Groups Json file name + path into our user dir
    /// </summary>
    /// <returns>A fully qualified filename</returns>
    static public string LayoutJsonFileName( )
    {
      log.Debug( "LayoutJsonFileName - Entry" );

      return Path.Combine( UserDir, "LayoutGroups.json" );
    }


    /// <summary>
    /// Returns the mapping file name + path into our user dir
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>A fully qualified filename</returns>
    static public string MappingCsvFileName( string mapName )
    {
      log.Debug( "MappingCsvFileName - Entry" );

      return Path.Combine( UserDir, mapName + ".csv" );
    }


    /// <summary>
    /// Returns the mapping file name + path into our user dir
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>A fully qualified filename</returns>
    static public string MappingXmlFileName( string mapName )
    {
      log.Debug( "MappingXmlFileName - Entry" );

      return Path.Combine( UserDir, mapName + ".scjm.xml" );
    }

    /// <summary>
    /// Returns the mapping file name + path into our user dir
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>A fully qualified filename</returns>
    static public string MappingJsonFileName( string mapName )
    {
      log.Debug( "MappingJsonFileName - Entry" );

      return Path.Combine( UserDir, mapName + ".scjm.json" );
    }


    /// <summary>
    /// Create a backupfile from the given file
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    static public void BackupMappingFile( string mapName )
    {
      log.Debug( "BackupMappingFile - Entry" );

      string mf = MappingFileName( mapName );
      if ( File.Exists( mf ) ) File.Copy( mf, mf + ".backup", true );
    }

    /// <summary>
    /// Graphics folder name in the Application directory
    /// </summary>
    static public string GraphicsDir => "graphics";

    /// <summary>
    /// Graphics\Layouts folder name in the Application directory
    /// </summary>
    static public string LayoutsDir => Path.Combine(GraphicsDir, "layouts");


  }
}
