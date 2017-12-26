using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SCJMapper_V2.SC
{
  public class SCLogExtract
  {


    static private string ExtractValuableInfo( string inLine )
    {
      string retVal = "";
      string l = inLine.ToLowerInvariant( );

      // detect what we would like to find..
      if ( l.StartsWith( "log started" ) ) return string.Format("\t{0}\n", inLine);
      if ( l.StartsWith( "executable:" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "productversion" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "host cpu" ) ) return string.Format( "\t{0}\n", inLine );


      if ( l.StartsWith( "windows:" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "current display mode" ) ) return string.Format( "\t{0}\n", inLine );

      if ( l.Contains( "physical memory" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "--- dedicated video memory" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "- final rating" ) ) return string.Format( "\t{0}\n", inLine );

      if ( l.Contains( "64 bit" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.Contains( "keyboard" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.Contains( "display mode" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "- connected" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "reset controls" ) ) return string.Format( "\t{0}\n", inLine );
      if ( l.Contains( "enjoy" ) ) return string.Format( "\t{0}\n", inLine );

      return retVal;
    }


    static public string ExtractLog( )
    {
      string content = string.Format( "\n\n" );

      string fname = SCPath.SCLastLog;
      if ( string.IsNullOrEmpty( fname ) ) {
        return string.Format( "\nCould not find a logfile\n" );
      }
      // first choice a defaultProfile.xml in the app dir distributed with the application ??? to be deleted ???
      if ( File.Exists( fname ) ) {
        using ( StreamReader sr = new StreamReader( fname ) ) {
          while ( !sr.EndOfStream ) {
            content += ExtractValuableInfo( sr.ReadLine( ) );
          }
        }
      }
      return content += string.Format( "\n\n" ); ; ;
    }


  }
}
