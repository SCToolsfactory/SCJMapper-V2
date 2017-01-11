using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCJMapper_V2
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
    
    public const string BlendedInput = "~"; // internal used only
    static public bool IsBlendedInput( string input ) { return ( input == BlendedInput ); }

    
    static public bool IsDeviceClass( string deviceClass ) { return false; }
    static public string DeviceClassFromInput( string input ) { return DeviceClass; }
    static public string DevInput( string input ) { return input; }
    static public bool DevMatch( string devInput ) { return false; }

    public abstract string DevClass { get; }
    public abstract string DevName { get; }
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
      return blendedInput.Replace( BlendedInput, " " ); // must make spaces (tilde is for internal processing only)
    }
    static public string toXMLBlendExtension( string blendedInput )
    {
      return (IsBlendedInput(blendedInput) ? string.Format( "multiTap=\"1\"") : "" ); // blending needs to overwrite potential multitaps (2+)
    }
    static public string fromXML( string blendedInput )
    {
      return blendedInput.Replace( " ", BlendedInput ); // must make tilde (spaces is for external processing only)
    }

  }
}
