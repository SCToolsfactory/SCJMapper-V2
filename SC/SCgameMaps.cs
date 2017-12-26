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
  public class SCGameMaps: SortedList<string, string>
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public SCGameMaps()
    {
      if ( File.Exists( SCPath.SCData_p4k ) ) {
        try {
          var PD = new p4kDirectory( );
          IList<p4kFile.p4kFile> fileList = PD.ScanDirectoryContaining( SCPath.SCData_p4k, @"Data\Libs\Config\Mappings\layout_" );

          foreach (p4kFile.p4kFile file in fileList ) {
            byte[] fContent = PD.GetFile( SCPath.SCData_p4k, file );

            // use the binary XML reader
            CryXmlNodeRef ROOT = null;
            CryXmlBinReader.EResult readResult = CryXmlBinReader.EResult.Error;
            CryXmlBinReader cbr = new CryXmlBinReader( );
            ROOT = cbr.LoadFromBuffer( fContent, out readResult );
            if ( readResult == CryXmlBinReader.EResult.Success ) {
              XmlTree tree = new XmlTree( );
              tree.BuildXML( ROOT );
              this.Add( Path.GetFileNameWithoutExtension(file.Filename), tree.XML_string );
            }
            else {
              log.ErrorFormat( "SCGameMaps - Error in CryXmlBinReader: {0}", cbr.GetErrorDescription( ) );
            }
          }
        }
        catch ( Exception ex ) {
          log.Error( "SCGameMaps - Unexpected ", ex );
        }

      }

    }


  }
}
