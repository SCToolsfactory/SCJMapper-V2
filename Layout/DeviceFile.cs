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
      "MapName" : "T.16000M Joystick (right)",
      "MapImage" : "T16000M.jpg",
      "InputDevices" :[
      { 
        "InputType": "J1",
        "DeviceName": "T16000M",
        "DeviceProdGuid": ["{B10A044F-0000-0000-0000-504944564944}"],
         "Controls": [
               { "Input": "B1", "Type": "Digital", "X": 2044, "Y": 604, "Width": 642, "Height": 108, "Cmt": "Primary trigger" },
               ...
        ]
      }
      ]
    }

  */

  /// <summary>
  /// The Device Mapping File
  /// </summary>
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
    /// Create all possible ShapeItems for this Mapping File
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
    /// <param name="pidVid">the Device Prduct PID VID string in lowecase</param>
    /// <param name="input">the Item (device property)</param>
    /// <param name="firstInstance">If true it looks for the InputTypeNumber 1 (InputType="x1") else for the next</param>
    /// <returns>The found Control or Null</returns>
    public Control FindItem( string pidVid, string input, bool firstInstance )
    {
      if ( string.IsNullOrEmpty( pidVid ) ) return null;
      if ( string.IsNullOrEmpty( input ) ) return null;

      for ( int i = 0; i < InputDevices.Count; i++ ) {
        if ( InputDevices[i].DevicePIDVID.Contains( pidVid ) ) { // can have multiple PID VIDs for a device (alternates)
          // returns if we are asked for the first instance and it is the first one (default)
          if ( firstInstance && ( InputDevices[i].InputTypeNumber == 1 ) ) {
            return InputDevices[i].FindItem( input );
          }
          else if (!firstInstance && InputDevices[i].InputTypeNumber > 1 ) {
            return InputDevices[i].FindItem( input ); // not first and J2.. - return any other (more than 2 not supported)
          }
        }
      }
      return null;
    }

  }

  /// <summary>
  /// One Game Device 
  /// </summary>
  [DataContract]
  class Device
  {
    [DataMember( IsRequired = true )]
    public string InputType { get; set; } // Joystick, Throttle, Pedal, Gamepad, Keyboard, Other
    [DataMember( IsRequired = true )]
    public string DeviceName { get; set; } // The device name
    [DataMember( IsRequired = true )]
    public List<string> DeviceProdGuid { get; set; } = new List<string>( ); // The device product GUIDs as read by DirecInput
    [DataMember]
    public List<Control> Controls { get; set; } = new List<Control>( );// The list of Controls supported (see below)

    // non Json

    public string InputTypeLetter { get => InputType.Substring( 0, 1 ); }
    public int InputTypeNumber
    {
      get {
        if ( InputType.Length > 1 ) {
          if ( int.TryParse( InputType.Substring( 1 ), out int num ) ) {
            return num;
          }
        }
        return 1; //default
      }
    }

    /// <summary>
    /// returns the PID VID part of the GUID (seems how this is composed in Win)
    /// </summary>
    public List<string> DevicePIDVID
    {
      get {
        var ret = new List<string>( );
        foreach ( string s in DeviceProdGuid ) {
          string pv = s.Substring( 1, 8 ).ToLowerInvariant( ); // this is "{12345678-0000-0000 etc}
          ret.Add( pv );
        }
        return ret;
      }
    }

    /// <summary>
    /// Find a Control entry with the given input command
    /// </summary>
    /// <param name="input">the Item (device property)</param>
    /// <returns>The found Control or Null</returns>
    public Control FindItem( string input )
    {
      // input can be:  {modifier+}Input
      string[] e = input.Split( new char[] { '+' } );
      string effInput = e[e.Length - 1]; // last item is the real input
      for ( int i = 0; i < Controls.Count; i++ ) {
        if ( input == Controls[i].Input ) {
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
    [DataMember( IsRequired = true )]
    public string Input { get; set; } = ""; // buttonN, hatN_up,_right,_down,_left, [rot]xyz, sliderN (CryInput notification)
    [DataMember]
    public string Type { get; set; } = "";  // "" or Analog or Digital
    [DataMember( IsRequired = true )]
    public int X { get; set; } = 0;         // X label pos (left=0)
    [DataMember( IsRequired = true )]
    public int Y { get; set; } = 0;         // Y label pos (top=0)
    [DataMember( IsRequired = true )]
    public int Width { get; set; } = 600;   // Label field width
    [DataMember]
    public int Height { get; set; } = 54;   // Label field height
    [DataMember]
    public string Cmt { get; set; } = "";   // Comment


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
