using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;

namespace SCJMapper_V2
{
  public sealed class AppConfiguration : ConfigurationSection
  {
    // The collection (property bag) that contains the section properties.

    private static ConfigurationPropertyCollection _Properties;

    // The jsSenseLimit property.
    private static readonly ConfigurationProperty _jsSenseLimit =
              new ConfigurationProperty( "jsSenseLimit", typeof( int ), ( int )150, ConfigurationPropertyOptions.None );

    // ctor
    public AppConfiguration( )
    {
      // initialization
      _Properties = new ConfigurationPropertyCollection( );
      _Properties.Add( _jsSenseLimit );
    }


    protected override ConfigurationPropertyCollection Properties
    {
      get
      {
        return _Properties;
      }
    }

    [IntegerValidator( MinValue = 1, MaxValue = 1000, ExcludeRange = false )]
    public int jsSenseLimit
    {
      get
      {
        return ( int )this["jsSenseLimit"];
      }
      set
      {
        this["jsSenseLimit"] = value;
      }
    }



    /// <summary>
    /// Provide access to configuration props
    /// </summary>
    public class AppConfig
    {
      static private AppConfiguration GetAppSection( )
      {
        try {
          AppConfiguration appConfiguration = ConfigurationManager.GetSection( "AppConfiguration" ) as AppConfiguration;
          if ( appConfiguration == null )
            Console.WriteLine( "Failed to load AppConfiguration Section." );
          else {
            return appConfiguration;
          }

        }
        catch ( ConfigurationErrorsException err ) {
          Console.WriteLine( err.ToString( ) );
        }
        return null;
      }


      /// <summary>
      /// The axis detection sense limit
      /// </summary>
      static public int jsSenseLimit
      {
        get
        {
          AppConfiguration s = GetAppSection( );
          if ( s != null ) return s.jsSenseLimit;
          else return 150; // default if things go wrong...
        }
      }
    }


  }
}
