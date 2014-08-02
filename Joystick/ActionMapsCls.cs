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
    public static String[] ActionMaps = { "multiplayer", "singleplayer", "player", "spaceship_general", "spaceship_view", "spaceship_movement", "spaceship_targeting", "spaceship_weapons", "spaceship_missiles", 
                                "spaceship_defensive", "spaceship_auto_weapons", "spaceship_radar" , "spaceship_hud" , "IFCS_controls" };


    
    public String version { get; set; }
    // own additions for JS mapping - should not harm..
    public String js1 { get; set; }
    public String js2 { get; set; }
    public String js3 { get; set; }
    public String js4 { get; set; }
    public String js5 { get; set; }
    public String js6 { get; set; }
    public String js7 { get; set; }
    public String js8 { get; set; }

    /// <summary>
    /// ctor
    /// </summary>
    public ActionMapsCls( )
    {
      version = "0";
      js1 = ""; js2 = ""; js3 = ""; js4 = ""; js5 = ""; js6 = ""; js7 = ""; js8 = "";
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

      String r = String.Format( "<ActionMaps version=\"{0}\" ", version );
      if ( !String.IsNullOrEmpty( js1 ) ) r += String.Format( "js1=\"{0}\" ", js1 );
      if ( !String.IsNullOrEmpty( js2 ) ) r += String.Format( "js2=\"{0}\" ", js2 );
      if ( !String.IsNullOrEmpty( js3 ) ) r += String.Format( "js3=\"{0}\" ", js3 );
      if ( !String.IsNullOrEmpty( js4 ) ) r += String.Format( "js4=\"{0}\" ", js4 );
      if ( !String.IsNullOrEmpty( js5 ) ) r += String.Format( "js5=\"{0}\" ", js5 );
      if ( !String.IsNullOrEmpty( js6 ) ) r += String.Format( "js6=\"{0}\" ", js6 );
      if ( !String.IsNullOrEmpty( js7 ) ) r += String.Format( "js7=\"{0}\" ", js7 );
      if ( !String.IsNullOrEmpty( js8 ) ) r += String.Format( "js8=\"{0}\" ", js8 );
      r += String.Format( ">\n");
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
          js1 = reader["js1"];
          js2 = reader["js2"];
          js3 = reader["js3"];
          js4 = reader["js4"];
          js5 = reader["js5"];
          js6 = reader["js6"];
          js7 = reader["js7"];
          js8 = reader["js8"];
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
