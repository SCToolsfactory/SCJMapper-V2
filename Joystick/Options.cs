using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains an Options - something like:
  ///
  ///	<options type="joystick" instance="1">
  ///		<!-- Make all piloting input linear -->
  ///		<pilot exponent="1" />
  ///	</options>  
  /// 	
  /// [type] : set to shared, keyboard, xboxpad, or joystick
  /// [instance] : set to the device number; js1=1, js2=2, etc
  /// [optiongroup] : set to what group the option should affect (for available groups see default actionmap)
  /// [option] : instance, sensitivity, exponent, nonlinearity *instance is a bug that will be fixed to 'invert' in the future
  /// [value] : for invert use 0/1; for others use 0.0 to 2.0
  /// 
  /// 	</summary>
  public class Options
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    List<String> m_stringOptions = new List<String>( );
    DeviceTuningParameter m_tuningP = null; // pitch
    DeviceTuningParameter m_tuningY = null; // yaw
    DeviceTuningParameter m_tuningR = null; // roll

    // ctor
    public Options( JoystickList jsList )
    {
      m_tuningP = new DeviceTuningParameter( );
      m_tuningY = new DeviceTuningParameter( );
      m_tuningR = new DeviceTuningParameter( );
    }

    public int Count
    {
      get { return ( m_stringOptions.Count + ( ( m_tuningP != null ) ? 1 : 0 ) + ( ( m_tuningY != null ) ? 1 : 0 ) + ( ( m_tuningR != null ) ? 1 : 0 ) ); }
    }


    // provide access to Tuning items

    /// <summary>
    /// Returns the Pitch-Tuning item
    /// </summary>
    public DeviceTuningParameter TuneP
    {
      get { return m_tuningP; }
    }
    /// <summary>
    /// Returns the Yaw-Tuning item
    /// </summary>
    public DeviceTuningParameter TuneY
    {
      get { return m_tuningY; }
    }
    /// <summary>
    /// Returns the Roll-Tuning item
    /// </summary>
    public DeviceTuningParameter TuneR
    {
      get { return m_tuningR; }
    }


    private String[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new String[] { String.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      }
      catch ( Exception ) {
        return new String[] { xml };
      }
    }

    /// <summary>
    /// Dump the Options as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = "";

      // and dump the contents of plain string options
      foreach ( String x in m_stringOptions ) {

        if ( !String.IsNullOrWhiteSpace( x ) ) {
          foreach ( String line in FormatXml( x ) ) {
            r += String.Format( "\t{0}", line );
          }
        }

        r += String.Format( "\n" );
      }

      // dump Tuning 
      r += m_tuningP.Options_toXML( );
      r += m_tuningY.Options_toXML( );
      r += m_tuningR.Options_toXML( );

      return r;
    }



    /// <summary>
    /// Read an Options from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
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

      String type = "";
      String instance = "";

      if ( reader.HasAttributes ) {
        type = reader["type"];
        if ( !(( type.ToLowerInvariant( ) == "joystick") || (type.ToLowerInvariant( ) == "xboxpad") ) ) {
          // save as plain text
          if ( !m_stringOptions.Contains( xml ) ) m_stringOptions.Add( xml );
          return true;
        }
        // further on..
        instance = reader["instance"];

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

          if ( reader.Name == "flight_move_pitch" ) {
            m_tuningP.Options_fromXML( reader, type, int.Parse( instance ) );
          }
          else if ( reader.Name == "flight_move_yaw" ) {
            m_tuningY.Options_fromXML( reader, type, int.Parse( instance ) );
          }
          else if ( reader.Name == "flight_move_roll" ) {
            m_tuningR.Options_fromXML( reader, type, int.Parse( instance ) );
          }

          else {
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
