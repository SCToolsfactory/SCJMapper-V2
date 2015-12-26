using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2
{

  /// <summary>
  /// Contains the ActivationMode from SC
  ///   can be:
  ///   "Default" using the defActivationMode setting of the profile 
  ///   any of the List of profileDefined ones
  /// </summary>
  class ActivationModes : List<string>
  {

    public const String Default = "Use Profile";

    /// <summary>
    /// cTor: create a default one - with one Default element preset
    /// </summary>
    public ActivationModes( )
    {
      this.Add( Default ); // this is always in..
    }

    /// <summary>
    /// cTor: create one with a first item only
    /// </summary>
    /// <param name="firstItem"></param>
    public ActivationModes( string firstItem )
    {
      this.Clear( );
      this.Add( firstItem );
    }


  }
}
