using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCJMapper_V2.Layout
{
  // The Json file for LayoutGroups

  /*
   {
      "_comment" : "Layout Groups File (leave group names alone, change only the contained groups)",
      "SpaceFlight"       :[ "spaceship_general", "spaceship_view", "spaceship_movement", "spaceship_docking", "spaceship_power", "IFCS_controls"],
      "SpaceDefensive"    :[ "spaceship_defensive" ],
      "SpaceTargeting"    :[ "spaceship_targeting", "spaceship_target_hailing", "spaceship_scanning", "spaceship_ping", "spaceship_radar", "spaceship_targeting_advanced" ],
      "SpaceWeapons"      :[ "turret_main", "spaceship_weapons", "spaceship_missiles", "spaceship_auto_weapons" ],
      "SpaceMining"       :[ "spaceship_mining" ],
      "Player"            :[ "default", "prone", "player", "player_choice", "player_emotes", "player_input_optical_tracking" ],
      "EVA"               :[ "zero_gravity_eva" ],
      "Vehicle"           :[ "vehicle_general", "vehicle_driver" ],
      "Lights"            :[ "lights_controller" ],
      "Interaction"       :[ "spaceship_hud", "ui_textfield", "ui_notification", "player_choice_interaction_mode" ],
      "Spectator"         :[ "spectator", "flycam", "view_director_mode" ],
      "Others"            :[ "server_renderer" ]
    }

  */

  /// <summary>
  /// The LaoutGroups File
  /// </summary>
  [DataContract]
  class LayoutGroups
  {
    [DataMember( IsRequired = false )]
    public string _comment { get; set; }
    [DataMember( IsRequired = true )]
    public List<string> SpaceFlight { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> SpaceDefensive { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> SpaceTargeting { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> SpaceWeapons { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> SpaceMining { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> Player { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> EVA { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> Vehicle { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> Lights { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> Interaction { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> Spectator { get; set; } = new List<string>( );
    [DataMember( IsRequired = true )]
    public List<string> Others { get; set; } = new List<string>( );

    // non Json

  }
}
