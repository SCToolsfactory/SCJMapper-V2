using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// A list of ActionItems
  /// </summary>
  class ActionItemList : List<ActionItem>
  {
    public class Tracker
    {
      public string PidVid { get; set; } = "";

      private short m_lowN = 100;
      private short m_highN = 0;

      public short LowN { get => m_lowN; }
      public short HighN { get => m_highN; }

      /// <summary>
      /// Add a JsN number and keep track of low and high
      /// </summary>
      /// <param name="n"></param>
      public void AddNumber( short n )
      {
        m_lowN = ( n < m_lowN ) ? n : m_lowN;
        m_highN = ( n > m_highN ) ? n : m_highN;
      }
    }


    private Dictionary<string, Tracker> m_trackers = new Dictionary<string, Tracker>( );

    private Tracker GetTracker( string pidVid )
    {
      if ( m_trackers.ContainsKey( pidVid ) ) {
        return m_trackers[pidVid];
      }
      return null;
    }

    /// <summary>
    /// Returns true if the jsN number is the lower one 
    /// </summary>
    /// <param name="pidVid">Device pidvid</param>
    /// <param name="jsN">The JsN number</param>
    /// <returns></returns>
    public bool IsFirstInstance( string pidVid, short jsN )
    {
      var tracker = GetTracker( pidVid );
      if ( tracker == null ) return true;
      return ( jsN == tracker.LowN );
    }

    /// <summary>
    /// Keep track of JsNs while adding stuff
    /// </summary>
    /// <param name="actionItem"></param>
    public new void Add( ActionItem actionItem )
    {
      base.Add( actionItem );

      if ( actionItem.InputTypeLetter != "J" ) return; // track only Joysticks

      if ( !m_trackers.ContainsKey( actionItem.DevicePidVid ) ) {
        // newly seen device
        m_trackers.Add( actionItem.DevicePidVid, new Tracker( ) { PidVid = actionItem.DevicePidVid } );
      }
      m_trackers[actionItem.DevicePidVid].AddNumber( actionItem.InputTypeNumber );
    }



    /// <summary>
    /// Get the Devices contained in the List
    /// </summary>
    public Dictionary<int, string> Devices
    {
      get {
        var list = new Dictionary<int, string>( );
        foreach ( var si in this ) {
          if ( !list.ContainsKey( si.InputTypeNumber ) ) {
            list.Add( si.InputTypeNumber, si.DeviceName );
          }
        }
        return list;
      }
    }

    /// <summary>
    /// Get the Joystick Devices contained in the List
    /// </summary>
    public Dictionary<int, string> JsDevices
    {
      get {
        var list = new Dictionary<int, string>( );
        foreach ( var si in this ) {
          if (si.InputTypeLetter == "J" ) {
            if ( !list.ContainsKey( si.InputTypeNumber ) ) {
              list.Add( si.InputTypeNumber, si.DeviceName );
            }
          }
        }
        return list;
      }
    }

    /// <summary>
    /// Override the current JS guids with the given one
    /// </summary>
    /// <param name="guids">A list of GUIDs</param>
    public void OverrideJsDevices (List<string> guids )
    {
      for (int aiidx=0; aiidx<this.Count; aiidx++ ) {
        // Joystick only
        var ai = this[aiidx];
        if ( ai.InputTypeLetter == "J" ) {
          if ( guids.Count>= ai.InputTypeNumber ) {
            this[aiidx].DeviceProdGuid = guids[ai.InputTypeNumber - 1]; // the guid list is 0 based, jsN is 1 based..
          }
        }
      }
    }

  }
}
