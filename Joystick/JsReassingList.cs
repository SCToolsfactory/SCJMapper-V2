using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2
{
  public struct JsReassingR
  {
    public JsReassingR( int oldJ, int newJ ) { oldJs = oldJ; newJs = newJ; }
    public int oldJs;
    public int newJs;
  }

  public class JsReassingList : List<JsReassingR>
  {

    public Boolean ContainsOldJs( int oldJs )
    {
      foreach ( JsReassingR jr in this ) {
        if ( jr.oldJs == oldJs ) return true;
      }
      return false;
    }

    public int indexOfOldJs( int oldJs )
    {
      for ( int i=0; i < this.Count; i++ ) {
        if ( this[i].oldJs == oldJs ) return i;
      }
      return -1;
    }

    public int newJsFromOldJs( int oldJs )
    {
      foreach ( JsReassingR jr in this ) {
        if ( jr.oldJs == oldJs ) return jr.newJs;
      }
      return 0;
    }

    public void Add( int oldJs, int newJs )
    {
      JsReassingR rec = new JsReassingR( oldJs, newJs );
      this.Add( rec );
    }


  }
}
