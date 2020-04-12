using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCJMapper_V2.Actions.Json
{
  [DataContract]
  class JAction
  {
    /*
      { "action": "aname", "action_tx": "translated_name", 
        "rebind": [
          { "input" : "command" }, 
          { "input" : "command" }
        ] 
      },
     
     */
    [DataMember( Name = "action_tx" )]
    public string ActionTX { get; set; } = "";
    [DataMember( Name = "action" )]
    public string Action { get; set; } = "";
    [DataMember( Name = "rebind" )]
    public List<JCommand> Rebind { get; set; } = new List<JCommand>();
  }
}
