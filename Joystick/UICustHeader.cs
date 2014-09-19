using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace SCJMapper_V2
{
  /// <summary>
  ///   Maintains an CustomisationUIHeader - something like:
  ///
  /// 	<CustomisationUIHeader device="joystick" label="JoystickTMWarthog" description="@ui_JoystickTMWarthogDesc" image="JoystickTMWarthog" />
  /// 	
  /// 	</summary>
  class UICustHeader : List<String>
  {

    private String[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new String[] { String.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      }
      catch ( Exception ) {
        return new String[] { xml };
      }
    }

    /// <summary>
    /// Dump the CustomisationUIHeader as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public String toXML( )
    {
      String r = "";

      // and dump the contents
      foreach ( String x in this ) {
        if ( !String.IsNullOrWhiteSpace( x ) ) {
          foreach ( String line in FormatXml( x ) ) {
            r += String.Format( "\t{0}", line );
          }
        }
      }
      r += String.Format( "\n" );
      return r;
    }



    /// <summary>
    /// Read an CustomisationUIHeader from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public Boolean fromXML( String xml )
    {
      if ( !this.Contains( xml ) ) this.Add( xml );
      return true;
    }




  }
}
