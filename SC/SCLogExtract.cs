using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SCJMapper_V2.SC
{
  public class SCLogExtract
  {


    static private String ExtractValuableInfo( String inLine )
    {
      String retVal = "";
      string l = inLine.ToLowerInvariant( );

      // detect what we would like to find..
      if ( l.StartsWith( "log started" ) ) return String.Format("\t{0}\n", inLine);
      if ( l.StartsWith( "executable:" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "productversion" ) ) return String.Format( "\t{0}\n", inLine );


      if ( l.StartsWith( "windows:" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "current display mode" ) ) return String.Format( "\t{0}\n", inLine );

      if ( l.Contains( "physical memory" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "--- dedicated video memory" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "- final rating" ) ) return String.Format( "\t{0}\n", inLine );

      if ( l.Contains( "64 bit" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.Contains( "keyboard" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.Contains( "display mode" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "- connected" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.StartsWith( "reset controls" ) ) return String.Format( "\t{0}\n", inLine );
      if ( l.Contains( "enjoy" ) ) return String.Format( "\t{0}\n", inLine );

      return retVal;
    }


    static public String ExtractLog( )
    {
      String content = String.Format( "\n\n" );

      String fname = SCPath.SCLastLog;
      if ( String.IsNullOrEmpty( fname ) ) {
        return String.Format( "\nCould not find a logfile\n" );
      }
      // first choice a defaultProfile.xml in the app dir distributed with the application ??? to be deleted ???
      if ( File.Exists( fname ) ) {
        using ( StreamReader sr = new StreamReader( fname ) ) {
          while ( !sr.EndOfStream ) {
            content += ExtractValuableInfo( sr.ReadLine( ) );
          }
        }
      }
      return content += String.Format( "\n\n" ); ; ;
    }


  }
}
