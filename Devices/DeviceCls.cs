using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SCJMapper_V2.Devices
{

  /// <summary>
  /// Basic device as DXInput device
  /// </summary>
  public abstract class DeviceCls
  {
    public const string DeviceClass ="UNDEF";
    public const string DeviceID = "NA0_"; 
    static public bool IsUndefined( string deviceClass )
    {
      return ( deviceClass == DeviceClass );
    }
    
    public const string DisabledInput = "~"; // internal used only
    static public bool IsDisabledInput( string input ) { return ( input == DisabledInput ); }

    
    static public bool IsDeviceClass( string deviceClass ) { return false; }
    static public string DeviceClassFromInput( string input ) { return DeviceClass; }
    static public string DevInput( string input ) { return input; }
    static public bool DevMatch( string devInput ) { return false; }

    static public bool IsAxisCommand( string command ) { return false; }

    /// <summary>
    /// Return the CIG instance number (which is the jsN number) - 1 based
    /// </summary>
    public abstract int XmlInstance { get; } // holds the CIG instance to be used throughout
    /// <summary>
    /// The DeviceClass of this instance
    /// </summary>
    public abstract string DevClass { get; }
    /// <summary>
    /// The DX ProductName property
    /// </summary>
    public abstract string DevName { get; }

    /// <summary>
    /// The DX instance number of the object (from enum) - 0 based 
    /// </summary>
    public abstract int DevInstance { get; }
    /// <summary>
    /// The DX GUID of the device
    /// </summary>
    public abstract string DevInstanceGUID { get; }

    /// <summary>
    /// The TabPage associated with the device (GP and JS only)
    /// </summary>
    public TabPage TabPage { get; set; } // added to handle hiding..
    public bool Hidden { get; set; } // added to handle hiding..

    public abstract System.Drawing.Color MapColor { get; }
    public virtual List<string> AnalogCommands { get { return new List<string>( ); }  } // just return an empty one if not implemented

    public abstract bool Activated { get; set; }
    public void Deactivate() { this.Activated = false; }
    public void Activate() { this.Activated = true; }

    private Stack<bool> m_activeState = new Stack<bool>( );
    /// <summary>
    ///  pushes the Activated state on a stack
    /// </summary>
    public void PushActiveState()
    {
      m_activeState.Push( Activated );
    }
    /// <summary>
    /// Pop the Activated state from stack
    /// </summary>
    public void PopActiveState()
    {
      if ( m_activeState.Count > 0 )
        Activated = m_activeState.Pop( );
    }


    public virtual void FinishDX( ) { }
    public virtual void ApplySettings( ) { }

    /// <summary>
    /// returns the currently available input string
    ///  (does not retrieve new data but uses what was collected by GetData())
    /// </summary>
    /// <returns>An input string or an empty string if no input is available</returns>
    public abstract string GetCurrentInput( );

    /// <summary>
    /// Find the last change the user did on that device
    ///  either new or from persistence
    /// </summary>
    /// <returns>An input string</returns>
    public abstract string GetLastChange( );

    /// <summary>
    /// Returns the data for the requested input
    /// Retrieves a new set of data from DX
    /// </summary>
    /// <param name="cmd">The input where the value is requested for</param>
    /// <param name="data">The value for the input</param>
    public abstract void GetCmdData( string cmd, out int data );

    /// <summary>
    /// Retrieves the input data from DX
    /// </summary>
    public abstract void GetData( );

    static public string toXML( string blendedInput )
    {
      return blendedInput.Replace( DisabledInput, " " ); // must make spaces (tilde is for internal processing only)
    }
    static public string toXMLBlendExtension( string blendedInput )
    {
      return (IsDisabledInput(blendedInput) ? string.Format( "multiTap=\"1\"") : "" ); // blending needs to overwrite potential multitaps (2+)
    }
    static public string fromXML( string blendedInput )
    {
      return blendedInput.Replace( " ", DisabledInput ); // must make tilde (spaces is for external processing only)
    }

  }
}
