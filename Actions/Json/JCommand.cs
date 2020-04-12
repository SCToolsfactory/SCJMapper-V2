using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace SCJMapper_V2.Actions.Json
{
  [DataContract]
  class JCommand
  {
    /*
          { "input" : "command" }, 
     */
    [DataMember( Name = "input" )]
    public string Input { get; set; } = "";

  }
}
