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

    // The gpSenseLimit property.
    private static readonly ConfigurationProperty _gpSenseLimit =
      new ConfigurationProperty( "gpSenseLimit", typeof( int ), ( int )500, ConfigurationPropertyOptions.None );

    // The msSenseLimit property. (Mouse)
    private static readonly ConfigurationProperty _msSenseLimit =
      new ConfigurationProperty( "msSenseLimit", typeof( int ), ( int )150, ConfigurationPropertyOptions.None );

    // The scActionmaps property.
    private static String _defaultActionmaps = "multiplayer,singleplayer,invite,player,flycam,vehicle_general,vehicle_driver,vehicle_gunner"
                + ",spaceship_general,spaceship_view,spaceship_movement,spaceship_targeting,spaceship_turret,spaceship_weapons,spaceship_missiles"
                + ",spaceship_defensive,spaceship_auto_weapons,spaceship_power,spaceship_radar,spaceship_hud,zero_gravity_general,zero_gravity_eva,IFCS_controls";
    private static readonly ConfigurationProperty _scActionmaps = 
      new ConfigurationProperty( "scActionmaps", typeof( String ), (String)_defaultActionmaps, ConfigurationPropertyOptions.None );

    // ctor
    public AppConfiguration( )
    {
      // initialization
      _Properties = new ConfigurationPropertyCollection( );
      _Properties.Add( _jsSenseLimit );
      _Properties.Add( _gpSenseLimit );
      _Properties.Add( _msSenseLimit );
      _Properties.Add( _scActionmaps );
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

    [IntegerValidator( MinValue = 1, MaxValue = 32000, ExcludeRange = false )]
    public int gpSenseLimit
    {
      get
      {
        return ( int )this["gpSenseLimit"];
      }
      set
      {
        this["gpSenseLimit"] = value;
      }
    }

    [IntegerValidator( MinValue = 1, MaxValue = 32000, ExcludeRange = false )]
    public int msSenseLimit
    {
      get
      {
        return ( int )this["msSenseLimit"];
      }
      set
      {
        this["msSenseLimit"] = value;
      }
    }

    [StringValidator( InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 10, MaxLength = 500 )]
    public String scActionmaps
    {
      get
      {
        return ( String )this["scActionmaps"];
      }
      set
      {
        this["scActionmaps"] = value;
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
      /// The Joystick axis detection sense limit
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

      /// <summary>
      /// The Gamepad axis detection sense limit
      /// </summary>
      static public int gpSenseLimit
      {
        get
        {
          AppConfiguration s = GetAppSection( );
          if ( s != null ) return s.gpSenseLimit;
          else return 500; // default if things go wrong...
        }
      }

      /// <summary>
      /// The Mouse axis detection sense limit
      /// </summary>
      static public int msSenseLimit
      {
        get
        {
          AppConfiguration s = GetAppSection( );
          if ( s != null ) return s.msSenseLimit;
          else return 150; // default if things go wrong...
        }
      }

      /// <summary>
      /// The actionmaps supported
      /// </summary>
      static public String scActionmaps
      {
        get
        {
          AppConfiguration s = GetAppSection( );
          if ( s != null ) {
            // get rid of blanks and CR,LFs from the config file
            String t = s.scActionmaps.Replace( " ", "" ).Replace( String.Format( "\n" ), "" ).Replace( String.Format( "\r" ), "" );
            return t;
          }
          else return _defaultActionmaps; // default if things go wrong...
        }
      }
    }


  }
}
