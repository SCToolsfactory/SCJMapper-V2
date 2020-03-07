using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2.Layout
{
  // The Json file for a controller

  /*
       {
         "InputDevice" : { 
         "InputType": "Joystick",
         "DeviceName": "SaitekX55Joystick",
         "DeviceProdGuid": "{6dfb1c7b-7808-4f43-9dec-e9a4d83fcf4f}",
         "MapImage": "x55-joystick.jpg",
         "Controls": [
               { "Input": "B1", "Type": "Digital", "X": 2044, "Y": 604, "Width": 642, "Height": 108, "Cmt": "Primary trigger" },
               ...
             ]
          }
       }

  */
  [DataContract]
  class DeviceFile
  {
    [DataMember( IsRequired = true )]
    public string MapName { get; set; }
    [DataMember( IsRequired = true )]
    public string MapImage { get; set; } // The map Image
    [DataMember]
    public List<Device> InputDevices { get; set; } = new List<Device>( );

    /// <summary>
    /// Create all possible ShapeItems for this Device
    /// </summary>
    public void CreateShapes()
    {
      for ( int i = 0; i < InputDevices.Count; i++ ) {
        InputDevices[i].CreateShapes( );
      }
    }

    /// <summary>
    /// Find a Control entry with the given product guid and input command
    /// </summary>
    /// <param name="pGuid">the Device Prduct GUID</param>
    /// <param name="item">the Item (device property)</param>
    /// <returns>The found Control or Null</returns>
    public Control FindItem( string pGuid, string item )
    {
      if ( string.IsNullOrEmpty( pGuid ) ) return null;
      if ( string.IsNullOrEmpty( item ) ) return null;

      for ( int i = 0; i < InputDevices.Count; i++ ) {
        if ( InputDevices[i].DevicePIDVID.Contains( pGuid.Substring(1,8).ToLowerInvariant( ) ) ) {
          return InputDevices[i].FindItem( item );
        }
      }
      return null;
    }

  }

  [DataContract]
  class Device
  {
    [DataMember]
    public string InputType { get; set; } // Joystick, Throttle, Pedal, Gamepad, Keyboard, Other
    [DataMember]
    public string DeviceName { get; set; } // The device name
    [DataMember]
    public List<string> DeviceProdGuid { get; set; } = new List<string>( ); // The device product GUIDs as read by DirecInput
    [DataMember]
    public List<Control> Controls { get; set; } = new List<Control>( );// The list of Controls supported (see below)

    // non Json

    /// <summary>
    /// returns the PID VID part of the GUID (seems how this is composed in Win)
    /// </summary>
    public List<string> DevicePIDVID
    {
      get {
        var ret = new List<string>( );
        foreach ( string s in DeviceProdGuid ) {
          string pv = s.Substring( 1, 8 ).ToLowerInvariant(); // this is "{12345678-0000-0000 etc}
          ret.Add( pv );
        }
        return ret;
      }
    }

    /// <summary>
    /// Find a Control entry with the given input command
    /// </summary>
    /// <param name="item">the Item (device property)</param>
    /// <returns>The found Control or Null</returns>
    public Control FindItem( string item )
    {
      for ( int i = 0; i < Controls.Count; i++ ) {
        if ( Controls[i].Input == item ) {
          return Controls[i];
        }
      }
      return null;
    }


    /// <summary>
    /// Create all possible ShapeItems for this Device
    /// </summary>
    public void CreateShapes()
    {
      for ( int i = 0; i < Controls.Count; i++ ) {
        Controls[i].CreateShapes( );
      }
    }

  }

  /// <summary>
  /// One Device Input (command)
  /// </summary>
  [DataContract]
  class Control
  {
    [DataMember]
    public string Input { get; set; } = ""; // buttonN, hatN_up,_right,_down,_left, [rot]xyz, sliderN (CryInput notification)
    [DataMember]
    public string Type { get; set; } = "";  // "" or Analog or Digital
    [DataMember]
    public int X { get; set; } = 0;         // X label pos (left=0)
    [DataMember]
    public int Y { get; set; } = 0;         // Y label pos (top=0)
    [DataMember]
    public int Width { get; set; } = 600;   // Label field width
    [DataMember]
    public int Height { get; set; } = 54;   // Label field height
    [DataMember]
    public string Cmt { get; set; }         // Comment


    // non Json

    /// <summary>
    /// A queue with all available text fields
    ///  Take one and Insert the Display text and then add it to the DisplayList
    ///  If exhausted - well bad luck..
    /// </summary>
    public Queue<ShapeItem> ShapeItems = null;

    // Base layout values to get enough fields and still have readable text
    private const float c_baseFontSize = 16F;

    /// <summary>
    /// Create all possible ShapeItems for this Control
    /// </summary>
    public void CreateShapes()
    {
      // this is a bit messy...
      // have to allocate a number of Rectangles to draw into but the layout rects are very different in size..
      this.ShapeItems = new Queue<ShapeItem>( ); // get rid of previous ones
      // create a reference font 
      int baseHeight = MapProps.MapFont.Height;
      int baseWidth = MapProps.MapFont.Height * 12; // Lets see if this is good or needs adjustment

      // live values from base
      int nCols = Width / baseWidth;
      int nLines = Height / baseHeight;

      for ( int l = 0; l < nLines; l++ ) {
        for ( int c = 0; c < nCols; c++ ) {
          var sh = new ShapeItem {
            X = X + c * baseWidth,
            Y = Y + l * baseHeight,
            Width = baseWidth,
            Height = baseHeight
          };
          ShapeItems.Enqueue( sh );
        }
      }
    }


  }
}
