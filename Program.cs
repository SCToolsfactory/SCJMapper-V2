using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

using log4net;
using log4net.Config;
using System.Globalization;
using System.Reflection;
//[assembly: log4net.Config.XmlConfigurator( Watch = true )]
[assembly: log4net.Config.XmlConfigurator( ConfigFile = "log4Net.config", Watch = true )]

namespace SCJMapper_V2
{
  static class Program
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger ( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    // not used as long as the culture change below works..
//    internal static CultureInfo cultUS = new CultureInfo("en-US", false); // use strict US formats (this is used for numbers only)
//    internal static NumberStyles nsNum = NumberStyles.Number;


    // Thanks.. http://blog.rastating.com/setting-default-currentculture-in-all-versions-of-net/
    static void SetDefaultCulture( CultureInfo culture )
    {
      // The CultureInfo class has two private static members named m_userDefaultCulture 
      // and m_userDefaultUICulture in versions prior to .NET 4.0; 
      // in 4.0 they are named s_userDefaultCulture and s_userDefaultUICulture.

      Type type = typeof(CultureInfo);

      try {
        type.InvokeMember( "s_userDefaultCulture",
                            BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                            null,
                            culture,
                            new object[] { culture } );

        type.InvokeMember( "s_userDefaultUICulture",
                            BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                            null,
                            culture,
                            new object[] { culture } );
      }
      catch { }

      try {
        type.InvokeMember( "m_userDefaultCulture",
                            BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                            null,
                            culture,
                            new object[] { culture } );

        type.InvokeMember( "m_userDefaultUICulture",
                            BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                            null,
                            culture,
                            new object[] { culture } );
      }
      catch { }
    }


    /* Log Levels Inc Priority
     * ALL 
     * DEBUG 
     * INFO 
     * WARN 
     * ERROR 
     * FATAL 
     * OFF
     * */
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main( )
    {
      // Log file setup
      log.InfoFormat( "\n" );
      log.InfoFormat( "SCJMapper_V2 - Started" );

      CultureInfo current = CultureInfo.CurrentCulture;
      CultureInfo modded = new CultureInfo( current.Name ); // that is the users locale
      CultureInfo us = new CultureInfo( "en-US" );
      modded.NumberFormat = us.NumberFormat;  // change the whole number format to US - should be safe ...
      // change the applications formatting to US (the dec point is essential here)
      SetDefaultCulture( modded ); // have to maintain number formats without tracking every piece of code with locales
      log.InfoFormat( "SCJMapper_V2 - Changed to US number formatting" );


      Application.EnableVisualStyles( );
      Application.SetCompatibleTextRenderingDefault( false );
      Application.Run( new MainForm( ) );

      log.InfoFormat( "SCJMapper_V2 - Ended\n" );
    }
  }
}
