using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains an actionmap - something like:
  ///   
  /// <actionmap name="spaceship_view">
  /// 		<action name="v_view_cycle_fwd">
  /// 			<rebind device="joystick" input="js2_button2" />
	///     </action>
  /// 		<action name="v_view_dynamic_focus_toggle">
  /// 			<rebind device="joystick" input="js2_button25" />
  /// 		</action>		
  /// 	</actionmap>
  /// </summary>
  class ActionMapCls : List<ActionCls>
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public String name { get; set; }

    /// <summary>
    /// Merge the given Map with this Map
    /// new ones are ignored - we don't learn from XML input for the time beeing
    /// </summary>
    /// <param name="newAcm"></param>
    public void Merge( ActionMapCls newAcm )
    {
      // do we find all actions in the new list that are like the new ones in our list ?
      foreach ( ActionCls newAc in newAcm ) {
        ActionCls AC = this.Find( delegate( ActionCls ac ) {
          return ac.key == newAc.key;
        } );
        if ( AC == null ) {
          ; //  this.Add( newAc ); // no, add it
        }
        else {
          AC.Merge( newAc ); // yes, merge it
        }
      }
    }


    /// <summary>
    /// Dump the actionmap as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = String.Format( "\t<actionmap name=\"{0}\">\n", name );
      foreach ( ActionCls ac in this ) {
        String x =  ac.toXML( );
        if ( !String.IsNullOrEmpty(x) ) r += String.Format( "\t{0}", x);
      }
      r += String.Format( "\t</actionmap>\n");
      return r;
    }


    /// <summary>
    /// Read an actionmap from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
    {
      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      if ( reader.Name == "actionmap" ) {
        if ( reader.HasAttributes ) {
          name = reader["name"];
        }
        else {
          return false;
        }
      }

      reader.Read( ); // move to next element

      String x = reader.ReadOuterXml( );
      while ( ! String.IsNullOrEmpty(x)  ) {
        ActionCls ac = new ActionCls( );
        if ( ac.fromXML( x ) ) {
          this.Add( ac ); // add to list
        }
        x=reader.ReadOuterXml();
      }
      return true;
    }

  }
}
