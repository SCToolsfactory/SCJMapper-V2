using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// defines the action grouping
  /// </summary>
  class ActionGroups
  {
    // logger
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    /// <summary>
    /// All actionmap groups
    /// </summary>
    public enum EGroup
    {
      SpaceFlight = 0,
      SpaceDefensive,
      SpaceTargeting,
      SpaceWeapons,
      SpaceMining,
      Player,
      EVA,
      Vehicle,
      //VehicleWeapons,  // removed in 3.10.0
      Lights,
      Interaction,
      Spectator,
      Others,
    }

    private static Dictionary<EGroup, List<string>> m_actionDict;

    /// <summary>
    /// cTor
    /// </summary>
    static ActionGroups()
    {
      m_actionDict = new Dictionary<EGroup, List<string>>( );

      // Try to read the user file
      var lg = LayoutGroupsJson.FromJson( TheUser.LayoutJsonFileName( ) );
      if ( lg != null ) {
        log.Info( "Layout-ActionGroups: use user provided file" );
        // read from the user file
        m_actionDict.Add( EGroup.SpaceFlight, lg.SpaceFlight );
        m_actionDict.Add( EGroup.SpaceTargeting, lg.SpaceTargeting );
        m_actionDict.Add( EGroup.SpaceMining, lg.SpaceMining );
        m_actionDict.Add( EGroup.SpaceWeapons, lg.SpaceWeapons );
        m_actionDict.Add( EGroup.SpaceDefensive, lg.SpaceDefensive );
        m_actionDict.Add( EGroup.Lights, lg.Lights );
        m_actionDict.Add( EGroup.Player, lg.Player );
        m_actionDict.Add( EGroup.EVA, lg.EVA );
        m_actionDict.Add( EGroup.Vehicle, lg.Vehicle );
        //m_actionDict.Add( EGroup.VehicleWeapons, lg.VehicleWeapons ); // removed in 3.10.0
        m_actionDict.Add( EGroup.Interaction, lg.Interaction );
        m_actionDict.Add( EGroup.Spectator, lg.Spectator );
        m_actionDict.Add( EGroup.Others, lg.Others );
      }
      else {
        log.Info( "Layout-ActionGroups: user provided file not available, use internal backup mapping" );
        // use the built in backup assignment (as of 3.10.0/1
        // Define which maps belongs to which group
        var x = new List<string>( ) { "spaceship_general", "spaceship_view", "spaceship_movement", "spaceship_docking", "spaceship_power", "IFCS_controls" };
        m_actionDict.Add( EGroup.SpaceFlight, x );

        x = new List<string>( ) { "spaceship_targeting", "spaceship_target_hailing", "spaceship_scanning", "spaceship_ping", "spaceship_radar", "spaceship_targeting_advanced" };
        m_actionDict.Add( EGroup.SpaceTargeting, x );

        x = new List<string>( ) { "spaceship_mining", "tractor_beam" };
        m_actionDict.Add( EGroup.SpaceMining, x );

        x = new List<string>( ) { "turret_main", "spaceship_weapons", "spaceship_missiles", "spaceship_auto_weapons" };
        m_actionDict.Add( EGroup.SpaceWeapons, x );

        x = new List<string>( ) { "spaceship_defensive" };
        m_actionDict.Add( EGroup.SpaceDefensive, x );

        x = new List<string>( ) { "lights_controller" };
        m_actionDict.Add( EGroup.Lights, x );

        x = new List<string>( ) { "default", "player", "prone", "hacking", "player_choice", "player_emotes", "player_input_optical_tracking" };
        m_actionDict.Add( EGroup.Player, x );

        x = new List<string>( ) { "zero_gravity_eva" };
        m_actionDict.Add( EGroup.EVA, x );

        x = new List<string>( ) { "vehicle_general", "vehicle_driver" };
        m_actionDict.Add( EGroup.Vehicle, x );
        //x = new List<string>( ) { "vehicle_gunner" }; // removed in 3.10.0
        //m_actionDict.Add( EGroup.VehicleWeapons, x ); // removed in 3.10.0
        // 3.13 add "player_choice_interaction_mode"
        x = new List<string>( ) { "spaceship_hud", "ui_textfield", "ui_notification", "player_choice_interaction_mode" };
        m_actionDict.Add( EGroup.Interaction, x );

        x = new List<string>( ) { "spectator", "flycam", "view_director_mode" };
        m_actionDict.Add( EGroup.Spectator, x );

        x = new List<string>( ) { "server_renderer" };
        m_actionDict.Add( EGroup.Others, x );
      }
    }

    /// <summary>
    /// Return the names of the groups
    /// </summary>
    /// <returns></returns>
    public static List<string> ActionGroupNames()
    {
      return Enum.GetNames( typeof( EGroup ) ).ToList( );
    }

    /// <summary>
    /// Returns the list of actionmaps of a group
    /// </summary>
    /// <param name="eClass"></param>
    /// <returns></returns>
    public static List<string> ActionmapNames( EGroup eClass )
    {
      return m_actionDict[eClass];
    }

    /// <summary>
    /// Returns the group from the actionmap
    /// </summary>
    /// <param name="actionmap"></param>
    /// <returns></returns>
    public static EGroup MapNameToGroup( string actionmap )
    {
      foreach ( var kv in m_actionDict ) {
        if ( kv.Value.Contains( actionmap ) )
          return kv.Key;
      }
      return EGroup.Others;
    }

    /// <summary>
    /// Returns the group from the actiongroup name
    /// </summary>
    /// <param name="actiongroup"></param>
    /// <returns></returns>
    public static EGroup GroupNameToGroup( string actiongroup )
    {
      foreach ( var kv in m_actionDict ) {
        if ( kv.Key.ToString( ) == actiongroup )
          return kv.Key;
      }
      return EGroup.Others;
    }



  }
}
