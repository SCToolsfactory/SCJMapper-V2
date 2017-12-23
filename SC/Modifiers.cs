using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SCJMapper_V2.SC
{
  public class Modifier
  {
    public string Name = "";
    public bool DefaultProfile = false;

    public Modifier( string name, bool defaultProfile = false )
    {
      Name = name;
      DefaultProfile = defaultProfile;
    }

    public string ToXML()
    {
      string r = string.Format( "\t\t<mod input =\"{0}\" />\n", Name );
      return r;
    }

    public static bool operator ==( Modifier a, Modifier b )
    {
      // If both are null, or both are same instance, return true.
      if ( ReferenceEquals( a, b ) ) {
        return true;
      }

      // If one is null, but not both, return false.
      if ( ( (object)a == null ) || ( (object)b == null ) ) {
        return false;
      }

      // Return true if the fields match:
      return a.Equals( b );
    }

    public static bool operator !=( Modifier a, Modifier b )
    {
      return !( a == b );
    }

    public override bool Equals( System.Object obj )
    {
      // If parameter is null return false.
      if ( obj == null ) {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      Modifier p = obj as Modifier;
      if ( (System.Object)p == null ) {
        return false;
      }

      // Return true if the fields match:
      return ( this.Equals( p ) );
    }

    /// <summary>
    /// Returns true if they are the same
    /// </summary>
    /// <param name="p">ActivationMode to compare with</param>
    /// <returns>True if both are the same, else false</returns>
    public bool Equals( Modifier p )
    {
      // If parameter is null return false:
      if ( (object)p == null ) {
        return false;
      }

      // Return true if the fields match:
      return ( Name == p.Name );
    }


    public override int GetHashCode()
    {
      return Name.GetHashCode( ) ^ this.GetHashCode( );
    }
  }


  /// <summary>
  /// Contains the ActivationMode from SC
  ///   can be:
  ///   "Default" using the defActivationMode setting of the profile 
  ///   any of the List of profileDefined ones
  /// </summary>
  public sealed class Modifiers : List<Modifier>
  {
    private static readonly Modifiers instance = new Modifiers( );


    public static Modifiers Instance
    {
      get {
        return instance;
      }
    }

    /// <summary>
    /// cTor: Empty - hidden
    /// </summary>
    private Modifiers() { }

    /// <summary>
    /// Returns the list of ActivationMode names
    /// </summary>
    /// <returns>A list of names</returns>
    public IList<string> Names
    {
      get {
        List<string> retVal = new List<string>( );
        foreach ( Modifier am in this ) retVal.Add( am.Name );
        return retVal;
      }
    }

    /// <summary>
    /// Returns the number of users added modifiers
    /// </summary>
    public int UserCount
    {
      get {
        int cnt = 0;
        foreach ( Modifier m in this ) {
          if ( !m.DefaultProfile )
            cnt++;
        }
        return cnt;
      }
    }

    /// <summary>
    /// Read the modifier node from profile or ActionMaps
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="defProfile"></param>
    /// <returns></returns>
    public bool FromXML( string xml, bool defProfile = false )
    {
      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      try {
        reader.ReadToFollowing( "modifiers" );
        reader.ReadToDescendant( "mod" );
        do {
          if ( reader.NodeType == XmlNodeType.EndElement ) {
            reader.Read( );
            break; // finished
          }
          string name = reader["input"];
          if ( !string.IsNullOrEmpty( name ) ) {
            var m = new Modifier( name, defProfile );
            if ( !Contains( m ) )
              Add( m );
          }
        } while ( reader.Read( ) );

        return true;
      }
      catch ( Exception ex ) {
        // get any exceptions from reading
        return false;
      }

    }


    /// <summary>
    /// Returns the XML string of Non Profile items
    /// </summary>
    /// <returns>An XML string</returns>
    public string ToXML()
    {
      if ( UserCount <= 0 ) return "";

      string r = string.Format( "\t<modifiers>\n" );
      foreach ( Modifier m in this ) {
        if ( !m.DefaultProfile )
          r += m.ToXML( );
      }
      r += string.Format( "\t</modifiers>\n\n" );
      return r;
    }

  }
}
