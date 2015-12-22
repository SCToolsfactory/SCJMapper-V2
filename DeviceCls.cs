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
    public const String DeviceClass ="UNDEF";
    public const String DeviceID = "NA0_"; 
    static public Boolean IsUndefined( String deviceClass )
    {
      return ( deviceClass == DeviceClass );
    }
    
    public const String BlendedInput = "~"; // internal used only
    static public Boolean IsBlendedInput( String input ) { return ( input == BlendedInput ); }

    
    static public Boolean IsDeviceClass( String deviceClass ) { return false; }
    static public String DeviceClassFromInput( String input ) { return DeviceClass; }
    static public Boolean DevMatch( String devInput ) { return false; }

    public abstract String DevClass { get; }
    public abstract String DevName { get; }
    public abstract System.Drawing.Color MapColor { get; }

    public abstract Boolean Activated { get; set; }
    public virtual void FinishDX( ) { }
    public virtual void ApplySettings( ) { }

    public abstract String GetLastChange( );
    public abstract void GetCmdData( String cmd, out int data );
    public abstract void GetData( );

    static public String toXML( String blendedInput )
    {
      return blendedInput.Replace( BlendedInput, " " ); // must make spaces (tilde is for internal processing only)
    }
    static public String fromXML( String blendedInput )
    {
      return blendedInput.Replace( " ", BlendedInput ); // must make tilde (spaces is for external processing only)
    }

  }
}
