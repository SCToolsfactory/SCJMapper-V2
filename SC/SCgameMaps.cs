using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCJMapper_V2.CryXMLlib;
using SCJMapper_V2.p4kFile;

namespace SCJMapper_V2.SC
{
  /// <summary>
  /// Maintains the game mappings contained in the p4k file
  /// </summary>
  public class SCGameMaps : SortedList<string, string>
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public SCGameMaps()
    {
      foreach ( string fileKey in SCFiles.Instance.MapFiles ) {
        this.Add( Path.GetFileNameWithoutExtension( fileKey ), SCFiles.Instance.MapFile( fileKey ) );
      }
    }


  }
}
