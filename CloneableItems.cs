using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2
{
  public class CloneableDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : ICloneable
  {
    public virtual object Clone( )
    {
      CloneableDictionary<TKey, TValue> clone = new CloneableDictionary<TKey, TValue>();
      foreach ( KeyValuePair<TKey, TValue> kvp in this ) {
        clone.Add( kvp.Key, ( TValue )kvp.Value.Clone( ) );
      }
      return clone;
    }
  }

  public class CloneableList<TValue> : List<TValue> where TValue : ICloneable
  {
    public virtual CloneableList<TValue> Clone( )
    {
      CloneableList<TValue> clone = new CloneableList<TValue>();
      foreach ( TValue kvp in this ) {
        clone.Add( ( TValue )kvp.Clone( ) );
      }
      return clone;
    }
  }

}
