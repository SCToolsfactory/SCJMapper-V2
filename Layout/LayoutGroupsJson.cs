using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  class LayoutGroupsJson
  {
    // logger
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    /// <summary>
    /// Reads from a LayoutGroups file
    /// </summary>
    /// <param name="jFilename">The Json Filename</param>
    /// <returns>A LayoutGroups obj or null for errors</returns>
    public static LayoutGroups FromJson( string jFilename )
    {
      LayoutGroups c = null;
      if ( File.Exists( jFilename ) ) {
        using ( var ts = File.OpenRead( jFilename ) ) {
          c = FromJson( ts );
        }
      }
      else {
        log.Debug( $"LayoutGroups.FromJson: Userfile does not exist ({jFilename})" );
      }
      return c;
    }


    /// <summary>
    /// Reads from a LayoutGroups stream
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A LayoutGroups obj or null for errors</returns>
    public static LayoutGroups FromJson( Stream jStream )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( LayoutGroups ) );
        object objResponse = jsonSerializer.ReadObject( jStream );
        var jsonResults = objResponse as LayoutGroups;
        return jsonResults;
      }
      catch ( Exception e ) {
        log.Debug( $"LayoutGroups.FromJson: Serializer Exception ({e.Message})" );
        return null;
      }
    }


  }
}
