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
    DeviceTuningParameter m_tuningX = null;
    DeviceTuningParameter m_tuningY = null;
    DeviceTuningParameter m_tuningZ = null;

    // ctor
    public Options( JoystickList jsList )
    {
      m_tuningX = new DeviceTuningParameter(  ); // can be x or rotx
      m_tuningY = new DeviceTuningParameter(  ); // can be y or roty
      m_tuningZ = new DeviceTuningParameter(  ); // can be z or rotz
    }

    public int Count
    {
      get { return ( m_stringOptions.Count + ( ( m_tuningX != null ) ? 1 : 0 ) + ( ( m_tuningY != null ) ? 1 : 0 ) + ( ( m_tuningZ != null ) ? 1 : 0 ) ); }
    }


    // provide access to Tuning items

    /// <summary>
    /// Returns the X-Tuning item
    /// </summary>
    public DeviceTuningParameter TuneX
    {
      get { return m_tuningX; }
    }
    /// <summary>
    /// Returns the Y-Tuning item
    /// </summary>
    public DeviceTuningParameter TuneY
    {
      get { return m_tuningY; }
    }
    /// <summary>
    /// Returns the Z-Tuning item
    /// </summary>
    public DeviceTuningParameter TuneZ
    {
      get { return m_tuningZ; }
    }

    /*
    /// <summary>
    /// reassign the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list (old,new)</param>
    public void ReassignJsN( Dictionary<int, int> newJsList )
    {
      foreach ( KeyValuePair<int,int> kv in newJsList ) {
        if ( m_tuningX.JsN == kv.Key ) m_tuningX.JsN = kv.Value;
        if ( m_tuningY.JsN == kv.Key ) m_tuningY.JsN = kv.Value;
        if ( m_tuningZ.JsN == kv.Key ) m_tuningZ.JsN = kv.Value;
      }
    }
    */

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
      r += m_tuningX.Options_toXML( );
      r += m_tuningY.Options_toXML( );
      r += m_tuningZ.Options_toXML( );

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

       * 	 <options type="joystick" instance="1">
		          <!-- Make all main stick piloting input linear -->
		          <pilot_move_main exponent="1" />
	          </options>
       * 
           <options type="joystick" instance="1">
		          <pilot>
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
		          </pilot>
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
         * <pilot> instance="0/1" sensitivity="n.nn" exponent="n.nn"  (instance should be invert)
         *   <pilot_move>
         *     <pilot_move_main> 
         *       <pilot_move_x>  
         *       <pilot_move_y>  
         *       <pilot_move_z>  
         *     <pilot_move_rot>
         *       <pilot_move_rotx>  
         *       <pilot_move_roty>  
         *       <pilot_move_rotz>  
         *     <pilot_move_sliders>
         *       <pilot_move_slider1>
         *       <pilot_move_slider2>
         *   <pilot_throttle> invert="0/1"
         *   <pilot_aim>
         *     <pilot_aim_main>
         *       <pilot_aim_x>  
         *       <pilot_aim_y>  
         *     <pilot_aim_rot>
         *       <pilot_aim_rotz>  
         *   <pilot_view>
         *     <pilot_view_main>
         *       <pilot_view_x>  
         *       <pilot_view_y>  
         *     <pilot_view_rot>
         *       <pilot_view_rotz>  
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

          if ( reader.Name == "pilot_move_x" || reader.Name == "pilot_move_rotx" ) {
            m_tuningX.Options_fromXML( reader, type, int.Parse( instance ) );
          }
          else if ( reader.Name == "pilot_move_y" || reader.Name == "pilot_move_roty" ) {
            m_tuningY.Options_fromXML( reader, type, int.Parse( instance ) );
          }
          else if ( reader.Name == "pilot_move_z" || reader.Name == "pilot_move_rotz" ) {
            m_tuningZ.Options_fromXML( reader, type, int.Parse( instance ) );
          }
          // fixed - get pitch as X and Yaw as Y
          else if ( reader.Name == "pilot_movepitch" ) {
            m_tuningX.Options_fromXML( reader, type, int.Parse( instance ) );
          }
          else if ( reader.Name == "pilot_moveyaw" ) {
            m_tuningY.Options_fromXML( reader, type, int.Parse( instance ) );
          }

          else if ( reader.Name == "pilot_throttle" ) {
            // supports invert
            //jtp.Options_fromXML( reader, int.Parse( instance ) );
            log.InfoFormat( "Options.fromXML: pilot_throttle node not yet supported" );
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
