using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains the complete ActionMaps - something like:
  ///   
  /// <ActionMaps version="0" >
  /// 	<actionmap name="spaceship_view">
  ///		<action name="v_view_cycle_fwd">
  ///			<rebind device="joystick" input="js2_button2" />
  ///		</action>
  ///		...
  ///			</actionmap>	
  /// </ActionMaps>
  /// </summary>
  class ActionMapsCls : List<ActionMapCls>
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    // actionmap names to gather (do we need them to be cofigurable ??)
    public static String[] ActionMaps = { "multiplayer", "singleplayer", "player", "flycam", "spaceship_general", "spaceship_view", "spaceship_movement", "spaceship_targeting", "spaceship_turret", 
                                          "spaceship_weapons", "spaceship_missiles", "spaceship_defensive", "spaceship_auto_weapons", "spaceship_radar" , "spaceship_hud" , "IFCS_controls" };


    
    public String version { get; set; }

    // own additions for JS mapping - should not harm..
    private String[] m_js;
    private String[] m_GUIDs;

    /// <summary>
    /// get/set jsN assignment (use 0-based index i.e. js1 -> [0])
    /// </summary>
    public String[] jsN
    {
      get { return m_js; }
    }

    /// <summary>
    /// get/set jsN GUID assignment (use 0-based index i.e. js1GUID -> [0])
    /// </summary>
    public String[] jsNGUID
    {
      get { return m_GUIDs; }
    }


    /// <summary>
    /// ctor
    /// </summary>
    public ActionMapsCls( )
    {
      version = "0";

      // create the Joystick assignments
      Array.Resize( ref m_js, JoystickCls.JSnum_MAX + 1 );
      Array.Resize( ref m_GUIDs, JoystickCls.JSnum_MAX + 1 );
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        m_js[i] = ""; m_GUIDs[i] = "";
      }
    }


    /// <summary>
    /// Copy return all ActionMaps while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The ActionMaps copy with reassigned input</returns>
    public ActionMapsCls ReassignJsN( Dictionary<int, int> newJsList )
    {
      ActionMapsCls newMaps = new ActionMapsCls( );
      // full copy from 'this' 
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        newMaps.jsN[i] = this.jsN[i]; newMaps.jsNGUID[i] = this.jsNGUID[i]; 
      }

      foreach ( ActionMapCls am in this ) {
        newMaps.Add( am.ReassignJsN( newJsList ) );
      }

      return newMaps;
    }

    /// <summary>
    /// Merge the given Map with this Map
    /// new ones are ignored - we don't learn from XML input for the time beeing
    /// </summary>
    /// <param name="newAcm"></param>
    private void Merge( ActionMapCls newAcm )
    {
      log.Debug( "Merge - Entry" );

      // do we find an actionmap like the new one in our list ?
      ActionMapCls ACM = this.Find( delegate( ActionMapCls acm ) {
        return acm.name == newAcm.name;
      } );
      if ( ACM == null ) {
        ; // this.Add( newAcm ); // no, add new
      }
      else {
        ACM.Merge( newAcm ); // yes, merge it
      }
    }

    
    /// <summary>
    /// Dump the ActionMaps as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      log.Debug( "toXML - Entry" );

      String r = String.Format( "<ActionMaps version=\"{0}\" \n", version );
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        if ( !String.IsNullOrEmpty( jsN[i] ) ) r += String.Format( "\tjs{0}=\"{1}\" ", i+1, jsN[i] );
        if ( !String.IsNullOrEmpty( jsNGUID[i] ) ) r += String.Format( "js{0}G=\"{1}\" \n", i + 1, jsNGUID[i] );
      }

      r += String.Format( ">\n" );
      foreach ( ActionMapCls amc in this ) {
        r += String.Format( "{0}\n", amc.toXML( ) );
      }
      r += String.Format( "</ActionMaps>\n" );
      return r;
    }



    /// <summary>
    /// Read an ActionMaps from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
    {
      log.Debug( "fromXML - Entry" );

      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );


      reader.Read( );

      if ( reader.Name == "ActionMaps" ) {
        if ( reader.HasAttributes ) {
          version = reader["version"];
          // get the joystick mapping if there is one
          for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
            jsN[i] = reader[String.Format( "js{0}", i + 1 )];
            jsNGUID[i] = reader[String.Format( "js{0}G", i + 1 )];
          }
        }
        else {
          return false;
        }
      }

      reader.Read( ); // move to next element

      String x = reader.ReadOuterXml( );
      while ( !String.IsNullOrEmpty( x ) ) {
        ActionMapCls acm = new ActionMapCls( );
        if ( acm.fromXML( x ) ) {
          this.Merge( acm ); // merge list
        }
        x = reader.ReadOuterXml( );
      }
      return true;
    }

  }
}
