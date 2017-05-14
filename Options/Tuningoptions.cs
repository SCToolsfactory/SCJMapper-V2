using SCJMapper_V2.Gamepad;
using SCJMapper_V2.Joystick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SCJMapper_V2.Options
{
  public class Tuningoptions : CloneableDictionary<string, OptionTree>, ICloneable
  {

    #region Static parts

    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private static char ID_Delimiter = '⁞';

    // Translate toID  keys to the proper OptionTree.. (reassing stuff)
    // index is the DevNumber (DX enum) - value is the JsN/XmlInstance from reassign
    private static int[] m_jsMap = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; // Max joysticks

    private static string TuneOptionID( string deviceClass, int dxNumber )
    {
      return string.Format( "{0}{1}{2}", deviceClass, ID_Delimiter, dxNumber.ToString( ) );
    }


    /// <summary>
    /// Create a tuningOptionID from devClass and xmlInstance (jsN for joysticks, 1 for gamepad)
    /// </summary>
    /// <param name="deviceClass">Joysstick or Gamepad class name</param>
    /// <param name="instance">The xml instance number or 1 for gamepad</param>
    /// <returns>An ID or a dummy ID if the instance is not found</returns>
    public static string TuneOptionIDfromJsN( string deviceClass, int instance )
    {
      // only search for joysticks
      if ( JoystickCls.IsDeviceClass( deviceClass ) ) {
        for ( int i = 0; i < m_jsMap.Length; i++ ) {
          if ( m_jsMap[i] == instance ) {
            return string.Format( "{0}{1}{2}", deviceClass, ID_Delimiter, i.ToString( ) );
          }
        }
        // not found return
        return string.Format( "{0}{1}{2}", deviceClass, ID_Delimiter, -1 ); // will not be found in the collection

      }
      else {
        // gamepad
        return string.Format( "{0}{1}{2}", deviceClass, ID_Delimiter, instance );
      }
    }


    // translate a ToID built from JsN into the internal collection key
    private static string ToIDfromJsToID( string toIDjs )
    {
      string deviceClass = ClassFromID( toIDjs );
      // only search for joysticks
      if ( JoystickCls.IsDeviceClass( deviceClass ) ) {
        string[] e = toIDjs.Split( ID_Delimiter );
        if ( e.Length > 1 ) {
          int i = int.Parse( e[1] );
          return TuneOptionIDfromJsN( e[0], i );
        }
        else
          return "";
      }
      else {
        // gamepad
        return toIDjs;
      }
    }


    /// <summary>
    /// Returns the instance part of an ID
    /// </summary>
    /// <param name="TO_ID">A tuningOptionID</param>
    /// <returns>The instance part</returns>
    public static int InstanceFromID( string TO_ID )
    {
      string[] e = TO_ID.Split( ID_Delimiter );
      if ( e.Length > 1 )
        return int.Parse( e[1] );
      else
        return 0;
    }

    /// <summary>
    /// Returns the xml instance (jsN for Joysticks) part of an ID
    /// </summary>
    /// <param name="TO_ID">A tuningOptionID</param>
    /// <returns>The xml instance part</returns>
    public static int XmlInstanceFromID( string TO_ID )
    {
      int inst = 0;
      string[] e = TO_ID.Split( ID_Delimiter );
      if ( e.Length > 1 ) {
        string deviceClass = ClassFromID( TO_ID );
        inst = int.Parse( e[1] );
        if ( JoystickCls.IsDeviceClass( deviceClass ) ) {
          if ( inst >= 0 ) return m_jsMap[inst];
          else return inst;
        }
        else {
          //Gamepad
          return inst;
        }
      }
      else {
        return -1;
      }
    }

    /// <summary>
    /// Returns the device class part of an ID
    /// </summary>
    /// <param name="TO_ID">A tuningOptionID</param>
    /// <returns>The device class part</returns>
    public static string ClassFromID( string TO_ID )
    {
      string[] e = TO_ID.Split( ID_Delimiter );
      if ( e.Length > 0 )
        return e[0];
      else
        return DeviceCls.DeviceClass;
    }

    #endregion


    /// <summary>
    /// Clone this object
    /// </summary>
    /// <returns>A deep Clone of this object</returns>
    public override object Clone()
    {
      var to = new Tuningoptions( (CloneableDictionary<string, OptionTree>)base.Clone( ) );
      // more objects to deep copy

#if DEBUG
      // check cloned item
      System.Diagnostics.Debug.Assert( CheckClone( to ) );
#endif
      return to;
    }

    /// <summary>
    /// Check clone against This
    /// </summary>
    /// <param name="clone"></param>
    /// <returns>True if the clone is identical but not a shallow copy</returns>
    private bool CheckClone( Tuningoptions clone )
    {
      bool ret = true;
      // object vars first

      // check THIS Dictionary
      ret &= ( this.Count == clone.Count );
      if ( ret ) {
        for ( int i = 0; i < this.Count; i++ ) {
          ret &= ( this.ElementAt( i ).Key == clone.ElementAt( i ).Key );

          ret &= ( !object.ReferenceEquals( this.ElementAt( i ).Value, clone.ElementAt( i ).Value ) );  // shall not be the same object !!
          ret &= ( this.ElementAt( i ).Value.CheckClone( clone.ElementAt( i ).Value ) ); // sub check
        }
      }
      return ret;
    }


    /// <summary>
    /// cTor:  Copy - Initializes the tuning options with the given one
    /// </summary>
    /// <param name="init"></param>
    private Tuningoptions( CloneableDictionary<string, OptionTree> init )
    {
      foreach ( KeyValuePair<string, OptionTree> kvp in init ) {
        this.Add( kvp.Key, kvp.Value );
      }
    }

    /// <summary>
    /// ctor: Create tuning options for each device
    ///       access via ToID
    /// </summary>
    public Tuningoptions()
    {
      // init reassign map
      m_jsMap = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }; // Max joysticks

      // create all Options for all devices found (they may or may no be used)
      foreach ( JoystickCls js in DeviceInst.JoystickListRef ) {
        string toid = TuneOptionID( JoystickCls.DeviceClass, js.DevInstance ); // initial is the XInput enumeration

        if ( !this.ContainsKey( toid ) ) {
          this.Add( toid, new OptionTree( js ) ); // init with disabled defaults
                                                  // update map
          m_jsMap[js.DevInstance] = js.XmlInstance;
        }
        else {
          log.WarnFormat( "cTor - Joystick DO_ID {0} exists (likely a duplicate device name", toid );
        }
      }

      // add gamepad if there is any
      if ( DeviceInst.GamepadRef != null ) {
        string toid = TuneOptionID( GamepadCls.DeviceClass, 1 );// const - 
        if ( !this.ContainsKey( toid ) ) {
          this.Add( toid, new OptionTree( DeviceInst.GamepadRef ) ); // init with disabled defaults
        }
        else {
          log.WarnFormat( "cTor - Gamepad DO_ID {0} exists", toid );
        }
      }
    }

    /// <summary>
    /// Reset all items that will be assigned dynamically while scanning the actions
    /// </summary>
    public void ResetDynamicItems()
    {
      foreach ( KeyValuePair<string, OptionTree> kv in this ) {
        OptionTree item = kv.Value;
        if ( item != null ) {
          item.ResetDynamicItems( ); ;
        }
      }
    }


    /// <summary>
    /// Remaps the tuning options (jsN numbers)
    /// </summary>
    /// <param name="newJsList">A reassign list</param>
    public void ReassignJsN( JsReassingList newJsList )
    {
      // reassign the N part for Joystick commands
      int[] jsMapNew = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // Max joysticks - all n.a. assigned

      // walk the complete list - it is along the DX enum - so copy should work
      for ( int i = 0; i < newJsList.Count; i++ ) {
        jsMapNew[i] = newJsList[i].newJs;
      }
      m_jsMap = (int[])jsMapNew.Clone( ); // make it live
    }

    /// <summary>
    /// Returns a DevTuning item from the sought parameter
    /// as the device is not given it looks for the first one that implements it
    /// Note: this prefers devices that are listed early (usually a gamepad is before the joysticks)
    /// </summary>
    /// <param name="option">The tuning parameter</param>
    /// <returns>The sought item or null if not found</returns>
    public DeviceTuningParameter FirstTuningItem( string option )
    {
      foreach ( KeyValuePair<string, OptionTree> kv in this ) {
        DeviceTuningParameter item = kv.Value.TuningItem( option );
        if ( ( item != null ) && !string.IsNullOrEmpty( item.NodeText ) )
          return item;
      }
      // not found in any device
      return null;
    }

    /// <summary>
    /// Returns a DevTuning item from the sought parameters
    /// </summary>
    /// <param name="toID">A Tuningoption ID (device)</param>
    /// <param name="option">The tuning parameter</param>
    /// <returns>The sought item or null if not found</returns>
    public DeviceTuningParameter TuningItem( string toID, string option )
    {
      if ( this.ContainsKey( toID ) ) {
        return this[toID].TuningItem( option );
      }
      return null;
    }

    /// <summary>
    /// Returns the OptionTree for a ToID or null - if not found
    /// </summary>
    /// <param name="toID">A ToID</param>
    /// <returns>The OptionTree or null if not found</returns>
    public OptionTree OptionTreeFromToID( string toID )
    {
      if ( this.ContainsKey( toID ) ) {
        return this[toID];
      }
      return null;
    }


    /// <summary>
    /// Read an Options from XML - do some sanity check
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public bool fromXML( string xml )
    {
      /* 
       * This can be a lot of the following options
       * try to do our best....
       * 
       *  <options type="joystick" instance="1">
       *  ..
          </options>  

       * 
           <options type="joystick" instance="1">
           ..
           </options>

       */
      XmlReaderSettings settings = new XmlReaderSettings( );
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      settings.IgnoreWhitespace = true;
      settings.IgnoreComments = true;
      XmlReader reader = XmlReader.Create( new StringReader( xml ), settings );

      reader.Read( );

      string type = "";
      string instance = ""; int nInstance = 0;

      if ( reader.HasAttributes ) {
        instance = reader["instance"];
        if ( !int.TryParse( instance, out nInstance ) ) nInstance = 0;

        type = reader["type"];
        // now dispatch to the instance to capture the content
        if ( type.ToLowerInvariant( ) == "joystick" ) {
          string toID = TuneOptionIDfromJsN( JoystickCls.DeviceClass, nInstance );
          // now this might not be availabe if devices have been changed
          if ( this.ContainsKey( toID ) ) {
            this[toID].fromXML( xml );
          }
          else {
            log.InfoFormat( "Read XML Options - joystick instance {0} is not available - dropped this content", nInstance );
          }
        }
        else if ( type.ToLowerInvariant( ) == "xboxpad" ) {
          string toID = TuneOptionID( GamepadCls.DeviceClass, nInstance );
          if ( this.ContainsKey( toID ) ) {// 20170513: bugfix if gamepad is in the XML but not connected right now - ignore
            this[toID].fromXML( xml );
          }
          else {
            log.InfoFormat( "Read XML Options - xboxpad instance {0} is not available - dropped this content", nInstance );
          }
        }

      }
      return true;
    }

  }
}
