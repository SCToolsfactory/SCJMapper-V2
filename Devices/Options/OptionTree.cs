using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using SCJMapper_V2.SC;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Keyboard;
using System.Collections;

namespace SCJMapper_V2.Devices.Options
{
  /// <summary>
  ///   Maintains Options - something like:
  ///
  ///	<options type="joystick" instance="1">
  ///		<!-- Make all piloting input linear -->
  ///		<pilot exponent="1" />
  ///	</options>  
  /// 	
  /// <options  type="joystick"  instance="2"  >
  ///	  <flight_move_strafe_longitudinal  invert="1"  />
  /// </options>
  ///	
  /// [type] : set to shared, mouse, xboxpad, or joystick  (NOTE the CIG type used is keyboard .. for the mouse....(Grrr) - we use MOUSE
  /// [instance] : set to the device number; js1=1, js2=2, etc
  /// [optiongroup] : set to what group the option should affect (for available groups see default actionmap)
  /// [option] : instance, sensitivity, exponent, nonlinearity *instance is a bug that will be fixed to 'invert' in the future
  /// [value] : for invert use 0/1; for others use 0.0 to 2.0
  /// 
  /// options are given per deviceClass and instance - it seems
  /// 	</summary>
  public class OptionTree : ICloneable
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );


    // Support only one set of independent options (string storage)
    List<string> m_stringOptions = new List<string>( );

    #region Clonable support

    // bag for all tuning items - key is the option name
    CloneableDictionary<string, DeviceTuningParameter> m_tuning = new CloneableDictionary<string, DeviceTuningParameter>( );

    /// <summary>
    /// Clone this object
    /// </summary>
    /// <returns>A deep Clone of this object</returns>
    public object Clone()
    {
      var ot = (OptionTree)this.MemberwiseClone( );
      // more objects to deep copy
      ot.m_stringOptions = new List<string>( m_stringOptions );
      if ( this.m_tuning != null ) ot.m_tuning = (CloneableDictionary<string, DeviceTuningParameter>)this.m_tuning.Clone( );

      return ot;
    }

    /// <summary>
    /// Check clone against This
    /// </summary>
    /// <param name="clone"></param>
    /// <returns>True if the clone is identical but not a shallow copy</returns>
    internal bool CheckClone( OptionTree clone )
    {
      bool ret = true;
      // object vars first
      ret &= ( !object.ReferenceEquals( this.m_stringOptions, clone.m_stringOptions ) );  // shall not be the same object !!

      // check m_tuning Dictionary
      ret &= ( this.m_tuning.Count == clone.m_tuning.Count );
      if ( ret ) {
        for ( int i = 0; i < this.m_tuning.Count; i++ ) {
          ret &= ( this.m_tuning.ElementAt( i ).Key == clone.m_tuning.ElementAt( i ).Key );

          ret &= ( !object.ReferenceEquals( this.m_tuning.ElementAt( i ).Value, clone.m_tuning.ElementAt( i ).Value ) );  // shall not be the same object !!
          ret &= ( this.m_tuning.ElementAt( i ).Value.CheckClone( clone.m_tuning.ElementAt( i ).Value ) );
        }
      }
      return ret;
    }

    #endregion



    // ctor
    public OptionTree( DeviceCls device )
    {
      if ( m_profileOptions.Count( ) == 0 ) {
        log.Error( "cTor OptionTree - profile not yet read" );
      }

      // get options for the device class
      var devOpts = m_profileOptions.Where( x => x.DeviceClass == device.DevClass );
      foreach (ProfileOptionRec rec in devOpts ) {
        m_tuning.Add( rec.OptName, new DeviceTuningParameter( rec.OptName, device ) );
      }
    }


    public int Count
    {
      get { return ( m_stringOptions.Count + 1 ); }
    }

    public void ResetDynamicItems()
    {
      foreach ( KeyValuePair<string, DeviceTuningParameter> kv in m_tuning ) {
        DeviceTuningParameter item = kv.Value;
        if ( item != null ) {
          item.ResetDynamicItems( );
        }
      }
    }

    // provide access to Tuning items


    /// <summary>
    /// Returns a tuning item for the asked option
    /// </summary>
    /// <param name="optionName">The option to get</param>
    /// <returns>A DeviceTuning item or null if it does not exist</returns>
    public DeviceTuningParameter TuningItem( string optionName )
    {
      if ( m_tuning.ContainsKey( optionName ) )
        return m_tuning[optionName];
      else
        return null;
    }



    private string[] FormatXml( string xml )
    {
      try {
        XDocument doc = XDocument.Parse( xml );
        return doc.ToString( ).Split( new string[] { string.Format( "\n" ) }, StringSplitOptions.RemoveEmptyEntries );
      }
      catch ( Exception ) {
        return new string[] { xml };
      }
    }

    /// <summary>
    /// Dump the Options as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public string toXML()
    {
      string r = "";

      // and dump the contents of plain string options
      foreach ( string x in m_stringOptions ) {

        if ( !string.IsNullOrWhiteSpace( x ) ) {
          foreach ( string line in FormatXml( x ) ) {
            r += string.Format( "\t{0}", line );
          }
        }

        r += string.Format( "\n" );
      }

      // dump Tuning 
      foreach ( KeyValuePair<string, DeviceTuningParameter> kv in m_tuning ) {
        r += kv.Value.Options_toXML( );
      }

      return r;
    }



    /// <summary>
    /// Read an Options from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public bool fromXML( XElement options )
    {
      /* 
       * This can be a lot of the following options
       * try to do our best....
       * 
       *  <options type="joystick" instance="1">
            <pilot_rot_moveyaw instance="1" sensitivity="0.8" exponent="1.2" />
          </options>  

       * 
           <options type="joystick" instance="1">
              <flight>
                <nonlinearity_curve>
                  <point in="0.1"  out="0.001"/>
                  <point in="0.25"  out="0.02"/>
                  <point in="0.5"  out="0.1"/>
                  <point in="0.75"  out="0.125"/>
                  <point in="0.85"  out="0.15"/>
                  <point in="0.90"  out="0.175"/>
                  <point in="0.925"  out="0.25"/>
                  <point in="0.94"  out="0.45"/>
                  <point in="0.95"  out="0.75"/>
                </nonlinearity_curve>
              </flight>
            </options>

       */
      string instance = (string)options.Attribute( "instance" ); // mandadory
      string type = (string)options.Attribute( "type" ); // mandadory
      if ( !int.TryParse( instance, out int nInstance ) ) nInstance = 0; // get the one from the map if given (else it's a map error...)

      string devClass = DeviceCls.DeviceClass; // the generic one
      if ( !string.IsNullOrEmpty(type)) devClass = type; // get the one from the map if given (else it's a map error...)

      // mouse arrives as type keyboard - fix it to deviceClass mouse here
      if ( KeyboardCls.IsDeviceClass( devClass ) )
        devClass = MouseCls.DeviceClass;

      // check if the profile contains the one we found in the map - then parse the element
      foreach ( XElement item in options.Elements( ) ) {
        if ( m_profileOptions.Contains( new ProfileOptionRec( ) { DeviceClass = devClass, OptName = item.Name.LocalName } ) ) {
          m_tuning[item.Name.LocalName].Options_fromXML( item, devClass, int.Parse( instance ) );
        }
      }

      return true;
    }

    #region static - Profile Handling

    public class ProfileOptionRec : IEquatable<ProfileOptionRec>
    {
      public string DeviceClass { get; set; }
      public string OptGroup { get; set; }
      public string OptName { get; set; }
      public bool ShowCurve { get; set; }
      public bool ShowInvert { get; set; }
      public ProfileOptionRec()
      {
        DeviceClass = DeviceCls.DeviceClass; OptGroup = ""; OptName = ""; ShowCurve = false; ShowInvert = false;
      }
      // same class and name means records match
      public bool Equals( ProfileOptionRec other )
      {
        return ( ( this.DeviceClass == other.DeviceClass ) && ( this.OptName == other.OptName ) );
      }
    }

    private static List<ProfileOptionRec> m_profileOptions = new List<ProfileOptionRec>( );
    /// <summary>
    /// Returns a list of ProfileOptions found in the defaultProfile
    /// </summary>
    public static IList<ProfileOptionRec> ProfileOptions { get => m_profileOptions; }

    /// <summary>
    /// Clears the stored optiontree items from the profile
    ///   must be cleared before re-reading them from profile
    /// </summary>
    public static void InitOptionReader()
    {
      m_profileOptions = new List<ProfileOptionRec>( );
    }

    /// <summary>
    /// Reads optiongroup nodes
    ///  the tree is composed of such nodes - we dive recursively
    /// </summary>
    /// <param name="optiongroupIn">The optiongroup to parse</param>
    /// <param name="devClass">The deviceclass it belongs to</param>
    /// <returns>True if OK</returns>
    private static bool ReadOptiongroup( XElement optiongroupIn, string devClass, string optGroup )
    {
      bool retVal = true;

      // collect content and process further groups
      string name = (string)optiongroupIn.Attribute( "name" );
      string uiLabel = (string)optiongroupIn.Attribute( "UILabel" );
      if ( string.IsNullOrEmpty( uiLabel ) )
        uiLabel = name; // subst if not found in Action node
      SCUiText.Instance.Add( name, uiLabel ); // Register item for translation

      // further groups
      IEnumerable<XElement> optiongroups = from x in optiongroupIn.Elements( )
                                           where ( x.Name == "optiongroup" )
                                           select x;
      foreach ( XElement optiongroup in optiongroups ) {
        retVal &= ReadOptiongroup( optiongroup, devClass, name ); // current is the group if we dive one down
      }
      // murks.. determine if it is a terminal node, then get items
      if ( optiongroups.Count() == 0 ) {
        ProfileOptionRec optRec = new ProfileOptionRec { DeviceClass = devClass, OptGroup=optGroup, OptName = name }; // create a new one
        // override props if they arrive in the node
        string attr = (string)optiongroupIn.Attribute( "UIShowCurve" );
        if ( !string.IsNullOrEmpty( attr ) ) {
          if ( int.TryParse( attr, out int showCurve ) ) {
            optRec.ShowCurve = ( showCurve == 1 );
          }
        }
        attr = (string)optiongroupIn.Attribute( "UIShowInvert" );
        if ( !string.IsNullOrEmpty( attr ) ) {
          if ( int.TryParse( attr, out int showInvert ) ) {
            optRec.ShowInvert = ( showInvert == 1 );
          }
        }
        if ( optRec.ShowCurve || optRec.ShowInvert)
          m_profileOptions.Add( optRec ); // add only if something is to tweak..
      }
      return retVal;
    }

    /// <summary>
    /// Collects items from the profile optiontree
    /// </summary>
    /// <param name="optiontree">The optiontree node</param>
    /// <returns>True if OK</returns>
    public static bool fromProfileXML( XElement optiontree )
    {
      /*
	      //<optiontree  type="keyboard"  name="root"  UIShowInvert="-1"  UIShowSensitivity="-1"  UISensitivityMin="0.01"  UISensitivityMax="6.25"  >
	      //	<optiongroup  name="master"  UILabel="@ui_COMasterSensitivity"  UIShowSensitivity="0"  UIShowInvert="0"  UIShowCurve="0"  >
	      //		<optiongroup  name="mouse_curves"  UILabel="@ui_COMasterSensitivityCurvesMouse"  UIShowCurve="-1"  UIShowSensitivity="0"  UIShowInvert="0"  >
       */
      log.Debug( "fromProfileXML - Entry" );

      bool retVal = true;

      string devClass = (string)optiontree.Attribute( "type" );
      // mouse arrives as type keyboard - fix it to deviceClass mouse here
      if ( KeyboardCls.IsDeviceClass( devClass ) )
        devClass = MouseCls.DeviceClass;

      IEnumerable<XElement> optiongroups = from x in optiontree.Elements( )
                                           where ( x.Name == "optiongroup" )
                                           select x;
      foreach ( XElement optiongroup in optiongroups ) {
        retVal &= ReadOptiongroup( optiongroup, devClass, "master" );
      }

      return retVal;
    }

    #endregion

  }
}
