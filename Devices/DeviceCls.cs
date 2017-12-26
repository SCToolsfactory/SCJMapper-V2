using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public abstract System.Drawing.Color MapColor { get; }
    public virtual List<string> AnalogCommands { get { return new List<string>( ); }  } // just return an empty one if not implemented

    public abstract bool Activated { get; set; }
    public virtual void FinishDX( ) { }
    public virtual void ApplySettings( ) { }

    public abstract string GetLastChange( );
    public abstract void GetCmdData( string cmd, out int data );
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
