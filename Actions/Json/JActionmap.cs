using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCJMapper_V2.Actions.Json
{
  [DataContract]
  class JActionmap
  {
    /*
    {
      "actionmap": "amapname", "actionmap_tx": "translated_amapname", 
       "actions": [
        ]
    }
     
     */

    [DataMember( Name = "actionmap_tx" )]
    public string ActionmapTX { get; set; }
    [DataMember( Name = "actionmap" )]
    public string Actionmap { get; set; } = "";
    [DataMember( Name = "actions" )]
    public List<JAction> Actions { get; set; } = new List<JAction>( );

  }
}
