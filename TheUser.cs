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

    /// <summary>
    /// Returns the name of the Personal Program folder in My Documents
    /// Creates the folder if needed
    /// </summary>
    /// <returns>Path to the Personal Program directory</returns>
    static public String UserDir
    {
      get
      {
        log.Debug( "UserDir - Entry" );
        String docPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.Personal ), Application.ProductName);
        if ( !Directory.Exists( docPath ) ) Directory.CreateDirectory( docPath );
        return docPath;
      }
    }


    /// <summary>
    /// Returns the mapping file name + path into our user dir
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    /// <returns>A fully qualified filename</returns>
    static public String MappingFileName( String mapName )
    {
      log.Debug( "MappingFileName - Entry" );

      return Path.Combine( UserDir, mapName + ".xml" );
    }


    /// <summary>
    /// Create a backupfile from the given file
    /// </summary>
    /// <param name="mapName">The mapping name</param>
    static public void BackupMappingFile( String mapName )
    {
      log.Debug( "BackupMappingFile - Entry" );

      String mf = MappingFileName( mapName );
      if ( File.Exists( mf ) ) File.Copy( mf, mf + ".backup", true );
    }

  }
}
