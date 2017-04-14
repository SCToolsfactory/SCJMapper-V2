using SCJMapper_V2.Gamepad;
using SCJMapper_V2.Joystick;
using SCJMapper_V2.Keyboard;
using SCJMapper_V2.Mouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCJMapper_V2
{
  /// <summary>
  /// Collects and provides the device instances throughout the application
  ///  Remark: pls use the ..Ref properties unless assiging the instance
  /// </summary>
  public sealed class DeviceInst
  {
    ///<remarks>
    /// Holds the DXInput Joystick List
    ///</remarks>
    static private JoystickList m_Joystick = new JoystickList( );
    static public JoystickList JoystickListInst
    {
      get => m_Joystick;
    }
    /// <summary>
    /// Provides the JoystickList of instances found in this system
    /// </summary>
    static public JoystickList JoystickListRef
    {
      get => m_Joystick;
    }

    ///<remarks>
    /// Holds the DXInput Joystick List
    ///</remarks>
    static private JoystickCls m_curJoystick = null;
    static public JoystickCls JoystickInst
    {
      set => m_curJoystick = value;
    }
    /// <summary>
    /// Provides the 'current' Joystick instance
    /// </summary>
    static public JoystickCls JoystickRef
    {
      get => m_curJoystick;
    }

    ///<remarks>
    /// Holds the DXInput keyboard
    ///</remarks>
    static private GamepadCls m_Gamepad = null;
    static public GamepadCls GamepadInst
    {
      set => m_Gamepad = value;
    }
    /// <summary>
    /// Provides the first Gamepad instance found in this system
    /// </summary>
    static public GamepadCls GamepadRef
    {
      get => m_Gamepad;
    }

    ///<remarks>
    /// Holds the DXInput keyboard
    ///</remarks>
    static private KeyboardCls m_Keyboard = null;
    static public KeyboardCls KeyboardInst
    {
      set => m_Keyboard = value;
    }
    /// <summary>
    /// Provides the first Keyboard instance found in this system
    /// </summary>
    static public KeyboardCls KeyboardRef
    {
      get => m_Keyboard;
    }

    ///<remarks>
    /// Holds the DXInput mouse
    ///</remarks>
    static private MouseCls m_Mouse = null;
    static public MouseCls MouseInst
    {
      set => m_Mouse = value;
    }
    /// <summary>
    /// Provides the first Mouse instance found in this system
    /// </summary>
    static public MouseCls MouseRef
    {
      get => m_Mouse;
    }

  }
}
