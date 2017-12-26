using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SCJMapper_V2.Common;

namespace SCJMapper_V2.Devices.Joystick
{
  public class JoystickList : List<JoystickCls>, IDisposable
  {
    #region Static Parts

    /// <summary>
    /// Reassigns the mapping color based on the jsAssignment list given
    /// i.e. prepare the mapping colors for a given jsN assignment
    /// </summary>
    /// <param name="newJsList">List of 0.. tabs where the value is the jsN number </param>
    static public void ReassignJsColor( List<int> newJsList )
    {
      // the default colors are aligned with the tabs - the tabs color is never changed but the jsN may
      // i.e. if the first Tab is assigned as js2 then the second MapColor must get the color of the first Tab
      int idx = 0;
      foreach ( int i in newJsList ) {
        if ( MyColors.TabColor[idx] == MyColors.GamepadColor ) {
          ;  // skip the gamepad for joystick coloring
        } else {
          // walk through the tabs
          if ( i > 0 ) {
            // this is the jsN for the tab indexed (make it 0 based)
            MyColors.JsMapColor[i - 1] = MyColors.TabColor[idx];
          }
        }
        idx++;
      }
    }

    static private Color DeviceColor( int dxnumber )
    {
      int devNumber = 0; // this runs asynch due to the gamepad tab somewhere inbetween..
      for ( int mapIndex = 0; mapIndex < MyColors.TabColor.Length; mapIndex++ ) {
        if ( MyColors.TabColor[mapIndex] == MyColors.GamepadColor ) {
          ; // skip the gamepad for joystick coloring
        } else if ( dxnumber == devNumber) {
          return MyColors.TabColor[mapIndex];
        } else {
          devNumber++;// not found but advance the device
        }
      }
      return Color.Pink; // error but we should see the pink...
    }

    #endregion

    private FormReassign FR = null;

    public JsReassingList JsReassingList { get; set; } // index - oldJs, newJs
    public List<int> NewJsList { get; set; }  // index is this[idx]


    protected virtual void Dispose( bool disposing )
    {
      if ( disposing ) {
        // dispose managed resources
        if ( FR != null ) FR.Dispose( );
      }
      // free native resources
    }

    public void Dispose()
    {
      Dispose( true );
      GC.SuppressFinalize( this );
    }



    /// <summary>
    /// Deactivate all joysticks 
    /// </summary>
    public void Deactivate( )
    {
      foreach ( JoystickCls j in this ) j.Activated = false;
    }

    /// <summary>
    /// Activate all joysticks
    /// </summary>
    public void Activate( )
    {
      foreach ( JoystickCls j in this ) j.Activated = true;
    }

    /// <summary>
    /// Show the jsN Reassign Dialog
    /// </summary>
    public DialogResult ShowReassign( )
    {
      if ( FR == null ) {
        FR = new FormReassign( this );
        JsReassingList = new JsReassingList( ); // used in ReassignJsN
        NewJsList = new List<int>( );
      }
      FR.ShowDialog( );
      if ( FR.Canceled == false ) {
        int jIdx = 0;
        // update the new js indication in the tabs
        foreach ( JoystickCls js in this ) {
          js.JSAssignment = NewJsList[jIdx++];
          if ( js.XmlInstance > 0 )
            MyColors.JsMapColor[js.XmlInstance - 1] = DeviceColor( js.DevInstance );
        }
      }
      return ( FR.Canceled ) ? DialogResult.Cancel : DialogResult.OK;
    }


    /// <summary>
    /// Reset the Js Assingment to the new mapping provided
    ///   index of the map is the jsNumber (0 based - i.e. js1 ==> index 0)
    /// </summary>
    public void ResetJsNAssignment( string[] jsNGUID )
    {
      ClearJsNAssignment( );
      // for all supported jsN
      for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
        JoystickCls js = null;
        if ( !string.IsNullOrEmpty( jsNGUID[i] ) ) js = Find_jsInstance( jsNGUID[i] );
        if ( js != null ) {
          js.JSAssignment = i + 1; // i is 0 based ; jsN is 1 based
          if ( js.XmlInstance > 0 )
            MyColors.JsMapColor[js.XmlInstance - 1] = DeviceColor( js.DevInstance );
        }
      }
    }

    /// <summary>
    /// Set JsN to zero
    /// </summary>
    public void ClearJsNAssignment( )
    {
      int devNum = 0;
      foreach ( JoystickCls js in this ) {
        js.JSAssignment = 0;
        MyColors.JsMapColor[devNum] = DeviceColor( devNum );
        devNum++;
      }
    }


    /// <summary>
    /// Returns the Joystick instance for the given jsN 
    /// </summary>
    /// <param name="n">The JsN</param>
    /// <returns>The instance or null if not found</returns>
    public JoystickCls Find_jsN( int n )
    {
      foreach ( JoystickCls j in this ) {
        if ( j.JSAssignment == n ) return j;
      }
      return null;
    }


    /// <summary>
    /// Returns the Joystick instance for the given device name
    /// </summary>
    /// <param name="n">The device name</param>
    /// <returns>The instance or null if not found</returns>
    public JoystickCls Find_jsDev( string devName )
    {
      foreach ( JoystickCls j in this ) {
        if ( j.DevName == devName ) return j;
      }
      return null;
    }


    /// <summary>
    /// Returns the Joystick instance for the given device instance GUID
    /// </summary>
    /// <param name="n">The instance GUID</param>
    /// <returns>The instance or null if not found</returns>
    public JoystickCls Find_jsInstance( string instGUID )
    {
      foreach ( JoystickCls j in this ) {
        if ( j.DevInstanceGUID == instGUID ) return j;
      }
      return null;
    }


  }
}
