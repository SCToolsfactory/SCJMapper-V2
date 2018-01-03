using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.IO;

using SCJMapper_V2.Common;
using SCJMapper_V2.SC;
using SCJMapper_V2.Devices;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;
using System.Xml.Linq;

namespace SCJMapper_V2.Actions
{
  /// <summary>
  ///   Maintains an action - something like:
  ///   
  /// 		<action name="v_view_cycle_fwd">
  /// 					<rebind device="joystick" input="js2_button2" />
  /// 				</action>
  /// 				
  /// 
  /// AC1.0
  /// <action name="v_roll">
  ///   <rebind device="joystick" input="js1_rotz" />
  ///   <addbind device="joystick" input="js2_x" />
  /// </action>
  /// 
  /// AC1.1
  /// <action name="v_roll">
  ///   <rebind device="joystick" input="rctrl+js1_rotz" />
  /// </action>
  /// 
  /// AC2.0
  /// <action name="v_roll">
  ///   <rebind input="js1_rotz" />  // jsN, moN, kbN (gamepad ?)
  ///   <addbind input="js1_rotz" />  // jsN, moN, kbN (gamepad ?) still possible together with rebind?
  /// </action>
  /// 
  /// </summary>
  public class ActionCls
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    public string Key { get; set; }                       // the key is the "Daction" formatted item (as we can have the same name multiple times)
    public string ActionName { get; set; }                // the plain action name e.g. v_yaw
    public Act.ActionDevice ActionDevice { get; set; }    // the enum of the device
    public string Device { get; set; }                    // name of the device (uses DeviceClass)
    public string DefBinding { get; set; }                // the default binding
    public ActivationMode DefActivationMode { get; set; } // the default binding ActivationMode
    public List<ActionCommandCls> InputList { get; set; } // regular bind is the 0-element, addbinds are added to the list

    /// <summary>
    /// Clone this object
    /// </summary>
    /// <returns>A deep Clone of this object</returns>
    private ActionCls MyClone()
    {
      ActionCls newAc = (ActionCls)this.MemberwiseClone( );
      // more objects to deep copy
      newAc.DefActivationMode = (ActivationMode)this.DefActivationMode.Clone( );
      newAc.InputList = this.InputList.Select( x => (ActionCommandCls)x.Clone( ) ).ToList( );

      return newAc;
    }


    /// <summary>
    /// Copy return the action while reassigning the JsN Tag
    /// </summary>
    /// <param name="newJsList">The JsN reassign list</param>
    /// <returns>The action copy with reassigned input</returns>
    public ActionCls ReassignJsN( JsReassingList newJsList )
    {
      ActionCls newAc = this.MyClone( );
      // creates a copy of the list with reassigned jsN devs
      newAc.InputList.Clear( ); // get rid of cloned list
      foreach ( ActionCommandCls acc in InputList ) {
        newAc.InputList.Add( acc.ReassignJsN( newJsList ) ); // creates the deep copy of the list
      }

      return newAc;
    }

    /// <summary>
    /// ctor
    /// </summary>
    public ActionCls()
    {
      Key = "";
      ActionDevice = Act.ActionDevice.AD_Unknown;
      Device = JoystickCls.DeviceClass;
      ActionName = "";
      DefBinding = "";
      DefActivationMode = ActivationMode.Default;
      InputList = new List<ActionCommandCls>( ); // empty list
    }

    /// <summary>
    /// Creates and adds the inputCommand list with given input string
    /// AC2 style input is used i.e. with device tag in front
    ///   apply given ActivationMode - can be "~" to indicate DONT APPLY 
    /// </summary>
    /// <param name="devInput"></param>
    /// <returns>Returns the ActionCommand created</returns>
    public ActionCommandCls AddCommand( string devInput, ActivationMode activationMode )
    {
      ActionCommandCls acc = new ActionCommandCls( devInput, InputList.Count - 1 ); // starts from -1 ...
      acc.ActivationMode = new ActivationMode( activationMode );
      InputList.Add( acc );
      return acc;
    }

    /// <summary>
    /// Add an ActionCommand with Input at nodeindex
    ///   apply default ActivationMode
    /// </summary>
    /// <param name="devInput">The input to apply</param>
    /// <param name="index">The nodeindex</param>
    /// <returns>The created ActionCommand</returns>
    public ActionCommandCls AddCommand( string devInput, int index )
    {
      ActionCommandCls acc = new ActionCommandCls( devInput, index ) {
        ActivationMode = new ActivationMode( ActivationMode.Default )
      };
      InputList.Add( acc );
      return acc;
    }

    /// <summary>
    /// Delete an ActionCommand with nodeindex
    /// </summary>
    /// <param name="index">The nodeindex</param>
    public void DelCommand( int index )
    {
      int removeIt = -1;

      for ( int i = 0; i < InputList.Count; i++ ) {
        if ( InputList[i].NodeIndex == index ) removeIt = i;
        if ( InputList[i].NodeIndex > index ) InputList[i].NodeIndex -= 1; // reorder trailing ones
      }
      if ( removeIt >= 0 ) InputList.RemoveAt( removeIt );
    }

    /// <summary>
    /// Merge action is simply copying the new input control
    /// </summary>
    /// <param name="newAc"></param>
    public void Merge( ActionCls newAc )
    {
      this.InputList.Clear( );
      foreach ( ActionCommandCls acc in newAc.InputList ) {
        this.InputList.Add( acc );
      }
    }

    /// <summary>
    /// Updates an actionCommand with a new input (command)
    /// </summary>
    /// <param name="devInput">The input command</param>
    /// <param name="accIndex">The input index to update</param>
    public void UpdateCommandFromInput( string devInput, int accIndex ) // ActionCommandCls actionCmd )
    {
      //log.Debug( "UpdateCommandFromInput - Entry" );
      if ( accIndex < 0 ) return;
      // Apply the input to the ActionTree
      this.InputList[accIndex].DevInput = Act.DisableInput( devInput, this.ActionDevice );
      if ( Act.IsDisabledInput( this.InputList[accIndex].DevInput ) || string.IsNullOrEmpty( devInput ) ) {
        this.InputList[accIndex].ActivationMode = new ActivationMode( ActivationMode.Default ); // reset activation mode if the input is empty
      }
    }

    /// <summary>
    /// Find an ActionCommand with input in an Action
    /// </summary>
    /// <param name="input">The input</param>
    /// <returns>An actionCommand or null if not found</returns>
    public ActionCommandCls FindActionInputObject( string devInput )
    {
      log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCommandCls acc = null;
      acc = this.InputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.DevInput == devInput; } );
      if ( acc == null ) {
        log.Error( "FindActionInputObject - Action Input not found in Action" );
        return null;  // ERROR - Action Input not found in tree
      }
      return acc;
    }

    /// <summary>
    /// Find an ActionCommand with index in an Action
    /// </summary>
    /// <param name="index">The input index to find</param>
    /// <returns>An actionCommand or null if not found</returns>
    public ActionCommandCls FindActionInputObject( int index )
    {
      log.Debug( "FindActionInputObject - Entry" );
      // Apply the input to the ActionTree
      ActionCommandCls acc = null;
      acc = this.InputList.Find( delegate ( ActionCommandCls _ACC ) { return _ACC.NodeIndex == index; } );
      if ( acc == null ) {
        log.Error( "FindActionInputObject - Action Input not found in Action" );
        return null;  // ERROR - Action Input not found in tree
      }
      return acc;
    }

    /// <summary>
    /// Dump the action as partial XML nicely formatted
    /// </summary>
    /// <returns>the action as XML fragment</returns>
    public string toXML()
    {
      string r = ""; string
      bindCmd = "rebind"; // first entry is rebind
      if ( InputList.Count > 0 ) {
        if ( !string.IsNullOrEmpty( InputList[0].Input ) ) {
          r = string.Format( "\t<action name=\"{0}\">\n", ActionName );
          foreach ( ActionCommandCls acc in InputList ) {
            if ( !string.IsNullOrEmpty( acc.Input ) ) {
              r += string.Format( "\t\t\t<{0} {1}", bindCmd, acc.toXML( ) ); // 20151220BM: format for AC2 style 
              bindCmd = "addbind"; // switch to addbind
            }
          }
          r += string.Format( "\t\t</action>\n" );
        }
      }
      return r;
    }

    /// <summary>
    /// Read an action from XML - do some sanity checks
    /// </summary>
    /// <param name="xml">the XML action fragment</param>
    /// <returns>True if an action was decoded</returns>
    public bool fromXML( XElement actionNode )
    {
      ActionName = (string)actionNode.Attribute( "name" ); // mandadory
      foreach ( XElement bindingNode in actionNode.Nodes( ) ) {
        string binding = bindingNode.Name.ToString( );
        string input = "", actModeName = "", multi = "";
        input = (string)bindingNode.Attribute( "input" ); // mandadory
        if ( string.IsNullOrEmpty( input ) ) input = "";
        actModeName = (string)bindingNode.Attribute( "ActivationMode" );
        multi = (string)bindingNode.Attribute( "multiTap" );
        string device = (string)bindingNode.Attribute( "device" );
        //process
        input = DeviceCls.fromXML( input ); // move from external to internal blend
        if ( !string.IsNullOrEmpty( device ) ) {
          // AC1 style - need to reformat mouse and keyboard according to AC2 style now
          if ( KeyboardCls.IsDeviceClass( device ) ) input = KeyboardCls.FromAC1( input );
          else if ( MouseCls.IsDeviceClass( device ) ) input = MouseCls.FromAC1( input );
          else if ( GamepadCls.IsDeviceClass( device ) ) input = GamepadCls.FromAC1( input );
        }
        Device = Act.DeviceClassFromInput( input );
        ActivationMode actMode = null;
        if ( !string.IsNullOrEmpty( actModeName ) ) {
          actMode = ActivationModes.Instance.ActivationModeByName( actModeName ); // should be a valid ActivationMode for this action
        }
        else {
          actMode = new ActivationMode( ActivationMode.Default ); // no specific name given, use default
          if ( !string.IsNullOrEmpty( multi ) ) {
            actMode.MultiTap = int.Parse( multi ); // modify with given multiTap
          }
        }
        if ( binding == "rebind" ) {
          Key = Act.DevTag( Device ) + ActionName; // unique id of the action
          ActionDevice = Act.ADevice( Device ); // get the enum of the input device
        }
        AddCommand( input, actMode );
      }//foreach
      return true;
    }

  }
}
