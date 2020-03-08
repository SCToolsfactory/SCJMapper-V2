using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCJMapper_V2.Devices.Gamepad;

namespace SCJMapper_V2.Layout
{
  /// <summary>
  /// Create an ActionItemList for Layout Debug
  ///   contains allmost all possible items ...
  /// </summary>
  class DbgActionItemList
  {
    private ActionItemList m_actionItems = new ActionItemList( );
    public ActionItemList DbgList { get => m_actionItems; }

    private ActionItem AI_Button( string aMap, short bNum, short jNum, string guid )
    {
      var ai = new ActionItem {
        ActionMap = aMap,
        ControlInput = $"button{bNum}",
        DeviceName = "Debug Controller",
        DeviceProdGuid = guid,
        DispText = $"Joy{jNum} - Button {bNum}",
        InputType = $"J{jNum}"
      };
      return ai;
    }

    private List<ActionItem> AI_POV( string aMap, short povNum, short jNum, string guid )
    {
      var ail = new List<ActionItem>( );
      var ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"hat{povNum}_up", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - POV {povNum} Up", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"hat{povNum}_right", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - POV {povNum} Right", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"hat{povNum}_down", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - POV {povNum} Down", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"hat{povNum}_left", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - POV {povNum} Left", InputType = $"J{jNum}"
      }; ail.Add( ai );
      return ail;
    }

    private List<ActionItem> AI_Analog( string aMap, short jNum, string guid )
    {
      var ail = new List<ActionItem>( );
      var ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"x", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - X - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"y", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - Y - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"z", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - Z - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"rotx", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - RotX - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"roty", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - RotY - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"rotz", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - RotZ - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"slider1", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - Slider 1 - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"slider2", DeviceName = "Debug Controller",
        DeviceProdGuid = guid, DispText = $"Joy{jNum} - Slider 2 - Axis", InputType = $"J{jNum}"
      }; ail.Add( ai );

      return ail;
    }

    private List<ActionItem> AI_Gamepad( string aMap )
    {
      var ail = new List<ActionItem>( );
      var ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"x", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button X", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"a", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button A", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"b", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button B", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"y", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Y", InputType = $"G1"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"shoulderl", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Shoulder Left", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"shoulderr", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Shoulder Right", InputType = $"G1"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"triggerl_btn", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Trigger Left", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"triggerr_btn", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Trigger Right", InputType = $"G1"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"back", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Back", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"start", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Start", InputType = $"G1"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"thumbl", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Thumb Left", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"thumbr", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Button Thumb Right", InputType = $"G1"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"dpad_up", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - POV Up", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"dpad_right", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - POV Right", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"dpad_down", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - POV Down", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"dpad_left", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - POV Left", InputType = $"G1"
      }; ail.Add( ai );

      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"thumblx", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - X - Axis Thumb Left", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"thumbly", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Y - Axis Thumb Left", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"thumbrx", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - X - Axis Thumb Right", InputType = $"G1"
      }; ail.Add( ai );
      ai = new ActionItem {
        ActionMap = aMap, ControlInput = $"thumbry", DeviceName = "Debug Controller",
        DeviceProdGuid = GamepadCls.DevGUIDCIG, DispText = $"GP - Y - Axis Thumb Right", InputType = $"G1"
      }; ail.Add( ai );

      return ail;
    }




    /// <summary>
    /// Create a debug list for all the guids provided
    /// </summary>
    /// <param name="guids">Array: At least one guid </param>
    public void CreateDebugList( string[] guids )
    {
      // create Joysticks js1..js4 for the provided GUIDs
      for ( int joy = 0; joy < guids.Length; joy++ ) {
        // Joy Buttons are in "spaceship_general"
        for ( int i = 1; i <= 32; i++ ) {
          m_actionItems.Add( AI_Button( "spaceship_general", (short)i, (short)( joy + 1 ), guids[joy] ) );
        }
        // Joy POVs are in "spaceship_view"
        for ( int i = 1; i <= 4; i++ ) {
          m_actionItems.AddRange( AI_POV( "spaceship_view", (short)i, (short)( joy + 1 ), guids[joy] ) );
        }
        // Joy Analogs are in "spaceship_movement"
        m_actionItems.AddRange( AI_Analog( "spaceship_movement", (short)( joy + 1 ), guids[joy] ) );
      }

      // Create Gamepad
      // Joy Analogs are in "spaceship_targeting"
      m_actionItems.AddRange( AI_Gamepad( "spaceship_targeting" ) );

      // Create Gamepad modified

      // Create Keyboard
    }


  }
}
