using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2.SC
{
  /// <summary>
  /// Defines a single activation mode 
  /// </summary>
  public class ActivationMode
  {
    private const string c_DefaultActModeName =  "Use Profile";

    /// <summary>
    /// The default ActivationMode
    /// </summary>
    static public ActivationMode Default = new ActivationMode( c_DefaultActModeName );

    /// <summary>
    /// Returns true if the given name matches the default name
    /// </summary>
    /// <param name="activationName">Name to test</param>
    /// <returns>True if the name matches the default name</returns>
    static public Boolean IsDefault( string activationName )
    {
      return ( activationName == Default.Name );
    }


    /// <summary>
    /// The name of the ActivationMode
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Number of 'multitaps' defined (1..)
    /// </summary>
    public int MultiTap { get; set; }


    /// <summary>
    /// cTor: empty constructor
    /// </summary>
    public ActivationMode( )
    {
      this.Name = Default.Name;
      this.MultiTap = Default.MultiTap;
    }

    /// <summary>
    /// cTor: copy constructor
    /// </summary>
    /// <param name="other"></param>
    public ActivationMode( ActivationMode other )
    {
      this.Name = other.Name;
      this.MultiTap = other.MultiTap;
    }

    /// <summary>
    /// cTor: with init arguments, try to find the multiTap from the Instance list
    /// </summary>
    /// <param name="name">The Name</param>
    public ActivationMode( string name )
    {
      Name = name;
      // if the name is default - multitap is valid else it should be 0 and is given by the mode list
      if ( Name == c_DefaultActModeName ) {
        MultiTap = 1;
      }
      else {
        // try to get the one from the list (if the list is available..)
        try {
          MultiTap = ActivationModes.Instance.MultiTapFor( Name );
        }
        catch {
          // not in list - set to 1
          MultiTap = 1;
        }
      }
    }

    /// <summary>
    /// cTor: with init arguments
    /// </summary>
    /// <param name="name">The Name</param>
    /// <param name="mTaps">The multiTap number</param>
    public ActivationMode( string name, int mTaps )
    {
      Name = name;
      MultiTap = mTaps;
    }

    /// <summary>
    /// Returns true if multiTap is more than 1
    /// </summary>
    public Boolean IsDoubleTap { get { return ( MultiTap > 1 ); } }



    public static bool operator ==( ActivationMode a, ActivationMode b )
    {
      // If both are null, or both are same instance, return true.
      if ( System.Object.ReferenceEquals( a, b ) ) {
        return true;
      }

      // If one is null, but not both, return false.
      if ( ( ( object )a == null ) || ( ( object )b == null ) ) {
        return false;
      }

      // Return true if the fields match:
      return a.Equals( b );
    }

    public static bool operator !=( ActivationMode a, ActivationMode b )
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
      ActivationMode p = obj as ActivationMode;
      if ( ( System.Object )p == null ) {
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
    public bool Equals( ActivationMode p )
    {
      // If parameter is null return false:
      if ( ( object )p == null ) {
        return false;
      }

      // Return true if the fields match:
      return ( Name == p.Name ) && ( MultiTap == p.MultiTap );
    }


    public override int GetHashCode( )
    {
      return Name.GetHashCode( ) ^ MultiTap.GetHashCode( );
    }

  }// end class



  /// <summary>
  /// Contains the ActivationMode from SC
  ///   can be:
  ///   "Default" using the defActivationMode setting of the profile 
  ///   any of the List of profileDefined ones
  /// </summary>
  public sealed class ActivationModes : List<ActivationMode>
  {
    private static readonly ActivationModes instance = new ActivationModes( ActivationMode.Default );


    public static ActivationModes Instance
    {
      get
      {
        return instance;
      }
    }


    /// <summary>
    /// cTor: Empty - hidden
    /// </summary>
    private ActivationModes( ) { }

    /// <summary>
    /// cTor: create one with a first item only
    /// </summary>
    /// <param name="firstItem"></param>
    public ActivationModes( ActivationMode firstItem )
    {
      this.Clear( );
      this.Add( firstItem );
    }


    /// <summary>
    /// cTor: create one with a default and an attached one
    /// </summary>
    /// <param name="defaultMode">The default ActivationMode</param>
    /// <param name="attachedMode">The attached ActivationMode</param>
    public ActivationModes( ActivationMode defaultMode, ActivationMode attachedMode )
    {
      this.Clear( );
      this.Add( defaultMode );
      this.Add( attachedMode );
    }


    /// <summary>
    /// Returns the multitap number for an item
    /// </summary>
    /// <param name="actModeName"></param>
    /// <returns></returns>
    public int MultiTapFor( string actModeName )
    {
      try {
        ActivationMode fAm = this.Find( am => am.Name == actModeName );
        return fAm.MultiTap;
      }
      catch {
        return 1;
      }
    }


    /// <summary>
    /// Returns the list of ActivationMode names
    /// </summary>
    /// <returns>A list of names</returns>
    public List<string> Names
    {
      get
      {
        List<string>retVal = new List<string>();
        foreach ( ActivationMode am in this ) retVal.Add( am.Name );
        return retVal;
      }
    }


    /// <summary>
    /// Returns a new instance from given name from the list 
    /// Or Default if not found
    /// </summary>
    /// <param name="actModeName">The activationMode to get</param>
    /// <returns>A new ActivationMode instance</returns>
    public ActivationMode ActivationModeByName( string actModeName )
    {
      try {
        ActivationMode fAm = this.Find( am => am.Name == actModeName );
        return new ActivationMode( fAm );
      }
      catch {
        return new ActivationMode( );
      }
    }




  }
}
