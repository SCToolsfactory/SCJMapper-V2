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

    /// <summary>
    /// Returns the name of the Personal Program folder in My Documents
    /// Creates the folder if needed
    /// </summary>
    /// <returns>Path to the Personal Program directory</returns>
    static public String UserDir
    {
      get
      {
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
      return Path.Combine( UserDir, mapName + ".xml" );
    }



  }
}
