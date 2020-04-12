using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Actions.Json
{
  [DataContract]
  class JExport
  {
    /*
          {
            "_comment": "comment", 
            "maps" : [
             ]
           }
     
     */
    [DataMember( Name = "_comment" )]
    public string Comment { get; set; } = "";
    [DataMember( Name = "maps" )]
    public List<JActionmap> Maps { get; set; } = new List<JActionmap>( );

    // non Json serializing

    public bool WriteToFile( string filename )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( JExport ) );
        using ( var tw = File.Create( filename ) ) {
          jsonSerializer.WriteObject( tw, this );
        }
      }
#pragma warning disable CS0168 // Variable is declared but never used
      catch ( Exception e ) {
#pragma warning restore CS0168 // Variable is declared but never used
        return false;
      }
      return true;
    }

  }
}
