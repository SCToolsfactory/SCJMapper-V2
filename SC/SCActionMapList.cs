using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.SC
{
  class SCActionMapList : SortedList<int, string>
  {
    private int NextID
    {
      get {
        return this.Count;
      }
    }

    public void AddActionMap( string amName )
    {
      if ( !this.ContainsValue( amName ) )
        this.Add( NextID, amName );
    }

    public string[] ActionMaps { get { return this.Values.ToArray( ); } }


  }
}
