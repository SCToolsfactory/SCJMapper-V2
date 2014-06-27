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
    private Boolean m_valid = false;
    public Boolean IsValid { get { return m_valid; } }

    private String m_fileName = "";
    public String DefaultProfileName { get { return m_fileName; } }

    /// <summary>
    /// Extracts the file to destPath
    /// the subpath will be retained
    /// </summary>
    /// <param name="destPath">Destination path to extract to</param>
    private void ExtractDefaultProfile( String destPath )
    {
      String scp = SCPath.SCClientDataPath;
      if ( String.IsNullOrEmpty( scp ) ) return; // sorry did not work

      using ( ZipFile zip = ZipFile.Read( SCPath.SCGameData_pak ) ) {
        zip.CaseSensitiveRetrieval = false;

        ICollection<ZipEntry> gdpak = zip.SelectEntries( "name = " + SCPath.DefaultProfileName, SCPath.DefaultProfilePath_rel );
        if ( gdpak != null ) {
          gdpak.FirstOrDefault( ).Extract( destPath, ExtractExistingFileAction.OverwriteSilently );
          m_fileName = Path.Combine( destPath, SCPath.DefaultProfilePath_rel );
          m_fileName = Path.Combine( m_fileName, SCPath.DefaultProfileName );
          m_valid = true;
        }
      }
    }


    /// <summary>
    /// Deletes the extracted version of the defaultProfile
    /// </summary>
    /// <param name="destPath">Destination path to delete from</param>
    public void ClearDefaultProfile( String destPath )
    {
      try {
        String p = Path.Combine( destPath, SCPath.DefaultProfilePath_rel );
        File.Delete( Path.Combine( p, SCPath.DefaultProfileName ) );
        m_fileName = "";
        m_valid = false;
      }
      catch { }
    }

    /// <summary>
    /// Retrieves the newest defaultProfile from GameData.pak
    /// It deletes the last retrieved one first
    /// </summary>
    /// <param name="destPath">Destination path to extract to</param>
    /// <returns>True if successfull; if false there is no old file left in the path</returns>
    public Boolean GetDefaultProfile( String destPath )
    {
      ClearDefaultProfile( destPath );
      ExtractDefaultProfile( destPath );
      return m_valid;
    }

  }
}
