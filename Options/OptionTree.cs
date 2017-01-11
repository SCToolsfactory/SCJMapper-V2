using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using SCJMapper_V2.Joystick;

namespace SCJMapper_V2.Options
{
  /// <summary>
  ///   Maintains an Options - something like:
  ///
  ///	<options type="joystick" instance="1">
  ///		<!-- Make all piloting input linear -->
  ///		<pilot exponent="1" />
  ///	</options>  
  /// 	
  /// <options  type="joystick"  instance="2"  >
  ///	  <flight_move_strafe_longitudinal  invert="1"  />
  /// </options>
  ///	
  /// [type] : set to shared, keyboard, xboxpad, or joystick
  /// [instance] : set to the device number; js1=1, js2=2, etc
  /// [optiongroup] : set to what group the option should affect (for available groups see default actionmap)
  /// [option] : instance, sensitivity, exponent, nonlinearity *instance is a bug that will be fixed to 'invert' in the future
  /// [value] : for invert use 0/1; for others use 0.0 to 2.0
  /// 
  /// options are given per deviceClass and instance - it seems
  /// 	</summary>
  public class OptionTree
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    // Support only one set of independent options (string storage)
    List<string> m_stringOptions = new List<string>( );

    // bag for all tuning items - key is the option name
    Dictionary<string,DeviceTuningParameter> m_tuning = new Dictionary<string, DeviceTuningParameter>();

    // ctor
    public OptionTree( )
    {
      m_tuning.Add( "flight_move_pitch", new DeviceTuningParameter( "flight_move_pitch" ) );
      m_tuning.Add( "flight_move_yaw", new DeviceTuningParameter( "flight_move_yaw" ) );
      m_tuning.Add( "flight_move_roll", new DeviceTuningParameter( "flight_move_roll" ) );
      m_tuning.Add( "flight_move_strafe_vertical", new DeviceTuningParameter( "flight_move_strafe_vertical" ) );
      m_tuning.Add( "flight_move_strafe_lateral", new DeviceTuningParameter( "flight_move_strafe_lateral" ) );
      m_tuning.Add( "flight_move_strafe_longitudinal", new DeviceTuningParameter( "flight_move_strafe_longitudinal" ) );

      m_tuning.Add( "flight_throttle_abs", new DeviceTuningParameter( "flight_throttle_abs" ) );
      m_tuning.Add( "flight_throttle_rel", new DeviceTuningParameter( "flight_throttle_rel" ) );

      m_tuning.Add( "flight_aim_pitch", new DeviceTuningParameter( "flight_aim_pitch" ) );
      m_tuning.Add( "flight_aim_yaw", new DeviceTuningParameter( "flight_aim_yaw" ) );

      m_tuning.Add( "flight_view_pitch", new DeviceTuningParameter( "flight_view_pitch" ) );
      m_tuning.Add( "flight_view_yaw", new DeviceTuningParameter( "flight_view_yaw" ) );

      m_tuning.Add( "turret_aim_pitch", new DeviceTuningParameter( "turret_aim_pitch" ) );
      m_tuning.Add( "turret_aim_yaw", new DeviceTuningParameter( "turret_aim_yaw" ) );

      // Gamepad specific
      /* Cannot find out to what actions they relate to -  left alone for now
      m_tuning.Add( "fps_view_pitch", new DeviceTuningParameter( "fps_view_pitch" ) );
      m_tuning.Add( "fps_view_yaw", new DeviceTuningParameter( "fps_view_yaw" ) );

      m_tuning.Add( "fps_move_lateral", new DeviceTuningParameter( "fps_move_lateral" ) );
      m_tuning.Add( "fps_move_longitudinal", new DeviceTuningParameter( "fps_move_longitudinal" ) );

      m_tuning.Add( "mgv_view_pitch", new DeviceTuningParameter( "mgv_view_pitch" ) );
      m_tuning.Add( "mgv_view_yaw", new DeviceTuningParameter( "mgv_view_yaw" ) );
      */
    }


    public int Count
    {
      get { return ( m_stringOptions.Count + 1 ); }
    }


    // provide access to Tuning items

    /// <summary>
    /// Returns a tuning item for the asked option
    /// </summary>
    /// <param name="optionName">The option to get</param>
    /// <returns>A DeviceTuning item or null if it does not exist</returns>
    public DeviceTuningParameter TuningItem( string optionName )
    {
      if ( m_tuning.ContainsKey( optionName ) )
        return m_tuning[optionName];
      else
        return null;
    }

    private string[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new string[] { string.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      } catch ( Exception ) {
        return new string[] { xml };
      }
    }

    /// <summary>
    /// Dump the Options as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public string toXML( )
    {
      string r = "";

      // and dump the contents of plain string options
      foreach ( string x in m_stringOptions ) {

        if ( !string.IsNullOrWhiteSpace( x ) ) {
          foreach ( string line in FormatXml( x ) ) {
            r += string.Format( "\t{0}", line );
          }
        }

        r += string.Format( "\n" );
      }

      // dump Tuning 
      foreach ( KeyValuePair<string, DeviceTuningParameter> kv in m_tuning ) {
        r += kv.Value.Options_toXML( );
      }

      return r;
    }



    /// <summary>
    /// Read an Options from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( string xml )
    {
      /* 
       * This can be a lot of the following options
       * try to do our best....
       * 
       *  <options type="joystick" instance="1">
            <pilot_rot_moveyaw instance="1" sensitivity="0.8" exponent="1.2" />
          </options>  

       * 
           <options type="joystick" instance="1">
              <flight>
                <nonlinearity_curve>
                  <point in="0.1"  out="0.001"/>
                  <point in="0.25"  out="0.02"/>
                  <point in="0.5"  out="0.1"/>
                  <point in="0.75"  out="0.125"/>
                  <point in="0.85"  out="0.15"/>
                  <point in="0.90"  out="0.175"/>
                  <point in="0.925"  out="0.25"/>
                  <point in="0.94"  out="0.45"/>
                  <point in="0.95"  out="0.75"/>
                </nonlinearity_curve>
              </flight>
            </options>

       */
      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      string type = "";
      string instance = ""; int nInstance = 0;

      if ( reader.HasAttributes ) {
        type = reader["type"];
        if ( !( ( type.ToLowerInvariant( ) == "joystick" ) || ( type.ToLowerInvariant( ) == "xboxpad" ) ) ) {
          // save as plain text
          if ( !m_stringOptions.Contains( xml ) ) m_stringOptions.Add( xml );
          return true;
        }
        // further on..
        instance = reader["instance"];
        if ( !int.TryParse( instance, out nInstance ) ) nInstance = 0;

        reader.Read( );
        // try to disassemble the items
        /*
         * <flight> instance="0/1" sensitivity="n.nn" exponent="n.nn"  (instance should be invert)
         *   <flight_move>
         *     <flight_move_pitch>  
         *     <flight_move_yaw>  
         *     <flight_move_roll>  
         *     <flight_move_strafe_vertical>  
         *     <flight_move_strafe_lateral>  
         *     <flight_move_strafe_longitudinal>  
         *   <flight_throttle> invert="0/1"
         *     <flight_throttle_abs>
         *     <flight_throttle_rel>
         *   <flight_aim>
         *       <flight_aim_pitch>  
         *       <flight_aim_yaw>  
         *   <flight_view>
         *       <flight_view_pitch>  
         *       <flight_view_yaw>  
         *   
         * 
                <nonlinearity_curve>
                  <point in="0.1"  out="0.001"/>
         *          ..
                </nonlinearity_curve>
         * 
         * 
         * 
         */
        while ( !reader.EOF ) {

          if ( reader.Name.ToLowerInvariant( ) == "flight_move_pitch" ) {
            m_tuning["flight_move_pitch"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_move_yaw" ) {
            m_tuning["flight_move_yaw"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_move_roll" ) {
            m_tuning["flight_move_roll"].Options_fromXML( reader, type, int.Parse( instance ) );

          } else if ( reader.Name.ToLowerInvariant( ) == "flight_move_strafe_vertical" ) {
            m_tuning["flight_move_strafe_vertical"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_move_strafe_lateral" ) {
            m_tuning["flight_move_strafe_lateral"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_move_strafe_longitudinal" ) {
            m_tuning["flight_move_strafe_longitudinal"].Options_fromXML( reader, type, int.Parse( instance ) );

          } else if ( reader.Name.ToLowerInvariant( ) == "flight_throttle_abs" ) {
            m_tuning["flight_throttle_abs"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_throttle_rel" ) {
            m_tuning["flight_throttle_rel"].Options_fromXML( reader, type, int.Parse( instance ) );

          } else if ( reader.Name.ToLowerInvariant( ) == "flight_aim_pitch" ) {
            m_tuning["flight_aim_pitch"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_aim_yaw" ) {
            m_tuning["flight_aim_yaw"].Options_fromXML( reader, type, int.Parse( instance ) );

          } else if ( reader.Name.ToLowerInvariant( ) == "flight_view_pitch" ) {
            m_tuning["flight_view_pitch"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.Name.ToLowerInvariant( ) == "flight_view_yaw" ) {
            m_tuning["flight_view_yaw"].Options_fromXML( reader, type, int.Parse( instance ) );
          } else if ( reader.NodeType != XmlNodeType.EndElement ) {
            //??
            log.InfoFormat( "Options.fromXML: unknown node - {0} - stored as is", reader.Name );
            if ( !m_stringOptions.Contains( xml ) ) m_stringOptions.Add( xml );
          }

          reader.Read( );
        }

      }
      return true;
    }



  }
}
