using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    private static string m_defProfileCached = ""; // cache...

    /// <summary>
    /// Ref to the used defaultProfile to inform the user about it.
    /// </summary>
    public static string UsedDefProfile = "n.a.";

    /// <summary>
    /// Returns a list of files found that match 'defaultProfile*.xml'
    /// 20151220BM: return only the single defaultProfile name
    /// </summary>
    /// <returns>A list of filenames - can be empty</returns>
    static public string DefaultProfileName
    {
      get { return "defaultProfile.xml"; }
    }

    /// <summary>
    /// Returns the sought default profile as string from various locations
    /// SC Alpha 2.2: Have to find the new one in E:\G\StarCitizen\StarCitizen\Public\Data\DataXML.pak (contains the binary XML now)
    /// </summary>
    /// <returns>A string containing the file contents</returns>
    static public string DefaultProfile()
    {
      log.Debug( "DefaultProfile - Entry" );

      string retVal = m_defProfileCached;
      if ( !string.IsNullOrEmpty( retVal ) ) return retVal; // Return cached defaultProfile

      // first choice a defaultProfile.xml in the app dir distributed with the application ??? to be deleted ???
      if ( File.Exists( DefaultProfileName ) ) { // 20170404 - use the given name, not the one from SCPATH...
        using ( StreamReader sr = new StreamReader( DefaultProfileName ) ) {
          retVal = sr.ReadToEnd( );
          UsedDefProfile = "AppDirectory defaultProfile";
          log.InfoFormat( "- Use {0}", UsedDefProfile );
          m_defProfileCached = retVal;
          return retVal; // EXIT
        }
      }

      // second try to get the SC defaultProfile ..\USER\defaultProfile.xml
      string patchProfile = Path.Combine( SCPath.SCClientUSERPath, DefaultProfileName );
      if ( File.Exists( patchProfile ) ) { // 20171126 PTU  patch location in ..\USER\defaultProfile.xml
        using ( StreamReader sr = new StreamReader( patchProfile ) ) {
          retVal = sr.ReadToEnd( );
          UsedDefProfile = "USER Directory defaultProfile.xml";
          log.InfoFormat( "- Use {0}", UsedDefProfile );
          m_defProfileCached = retVal;
          return retVal; // EXIT
        }
      }

      // third try to get the SC defaultProfile from the Data.p4k TODO
      retVal = SCFiles.Instance.DefaultProfile;
      if ( !string.IsNullOrEmpty( retVal ) ) {
        UsedDefProfile = "GamePack defaultProfile";
        log.InfoFormat( "- Use {0}", UsedDefProfile );
        m_defProfileCached = retVal;
        return retVal; // EXIT
      }

      // last resort is the built in one
      retVal = Properties.Resources.defaultProfile;
      UsedDefProfile = "App Resource defaultProfile";
      log.InfoFormat( "- Use {0}", UsedDefProfile );
      m_defProfileCached = retVal;
      return retVal; // EXIT
    }




  }
}
