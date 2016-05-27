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

      // detect what we would like to find..
      if ( inLine.StartsWith( "Log Started" ) ) return String.Format("\t{0}\n", inLine);
      if ( inLine.StartsWith( "Executable:" ) ) return String.Format( "\t{0}\n", inLine );
      if ( inLine.StartsWith( "ProductVersion" ) ) return String.Format( "\t{0}\n", inLine );

      if ( inLine.StartsWith( "Windows:" ) ) return String.Format( "\t{0}\n", inLine );
      if ( inLine.StartsWith( "Current display mode" ) ) return String.Format( "\t{0}\n", inLine );
      if ( inLine.Contains( "physical memory" ) ) return String.Format( "\t{0}\n", inLine );
      if ( inLine.StartsWith( "--- Dedicated video memory" ) ) return String.Format( "\t{0}\n", inLine );
      if ( inLine.StartsWith( "- Final rating" ) ) return String.Format( "\t{0}\n", inLine );

      if ( inLine.Contains( "> Creating" ) ) return String.Format( "\t{0}\n", inLine );


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
