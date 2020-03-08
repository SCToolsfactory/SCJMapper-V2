using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SCJMapper_V2.Actions;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;

namespace SCJMapper_V2.Devices.Options
{
  /// <summary>
  /// set of parameters to tune the Joystick
  /// </summary>
  public class DeviceTuningParameter : ICloneable
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );


    private string m_nodetext = "";     // v_pitch - js1_x
    private string m_action = "";       // v_pitch
    private string m_cmdCtrl = "";      // js1_x, js1_y, js1_rotz ...
    private string m_devClass = "";     // joystick OR xboxpad OR mouse
    private int m_devInstanceNo = -1;   // jsN - instance in XML

    string m_option = ""; // the option name (level where it applies)

    private string m_deviceName = "";
    private bool m_isStrafe = false;  // default

    private bool m_expEnabled = false;  // default
    private string m_exponent = "1.000";

    private bool m_ptsEnabled = false;  // default
    private List<string> m_PtsIn = new List<string>( );
    private List<string> m_PtsOut = new List<string>( );

    private bool m_invertForced = false; // default (forced writing)
    private bool m_invertEnabled = false; // default

    private DeviceCls m_deviceRef = null; // Ref

    private DeviceOptionParameter m_deviceoptionRef = null; // will be used only while editing the Options !!


    /// <summary>
    /// Clone this object
    /// </summary>
    /// <returns>A deep Clone of this object</returns>
    public object Clone()
    {
      var dt = (DeviceTuningParameter)this.MemberwiseClone( ); // self and all value types
      // more objects to deep copy
      // --> NO cloning as this Ref will be overwritten when editing
      dt.m_deviceoptionRef = null; // just reset

      return dt;
    }

    /// <summary>
    /// Check clone against This
    /// </summary>
    /// <param name="clone"></param>
    /// <returns>True if the clone is identical but not a shallow copy</returns>
    internal bool CheckClone( DeviceTuningParameter clone )
    {
      bool ret = true;
      // object vars first
      ret &= ( this.m_nodetext == clone.m_nodetext ); // immutable string - shallow copy is OK
      ret &= ( this.m_action == clone.m_action ); // immutable string - shallow copy is OK
      ret &= ( this.m_cmdCtrl == clone.m_cmdCtrl );// immutable string - shallow copy is OK
      ret &= ( this.m_devClass == clone.m_devClass ); // immutable string - shallow copy is OK
      ret &= ( this.m_devInstanceNo == clone.m_devInstanceNo );
      ret &= ( this.m_option == clone.m_option );
      ret &= ( this.m_deviceName == clone.m_deviceName );
      ret &= ( this.m_isStrafe == clone.m_isStrafe );
      ret &= ( this.m_expEnabled == clone.m_expEnabled );
      ret &= ( this.m_exponent == clone.m_exponent );
      ret &= ( this.m_ptsEnabled == clone.m_ptsEnabled );
      ret &= ( this.m_PtsIn == clone.m_PtsIn );
      ret &= ( this.m_PtsOut == clone.m_PtsOut );
      ret &= ( this.m_invertEnabled == clone.m_invertEnabled );
      ret &= ( this.m_invertForced == clone.m_invertForced );
      ret &= ( this.m_deviceRef == clone.m_deviceRef );

      // check m_deviceoptionRef
      // --> NO check as this is assigned and used only while editing the Options

      return ret;
    }



    public DeviceTuningParameter( string optName )
    {
      m_option = optName;
    }

    public DeviceTuningParameter( string optName, DeviceCls device )
    {
      m_option = optName;
      GameDevice = device;
    }

    #region Properties

    public DeviceCls GameDevice
    {
      get { return m_deviceRef; }
      set {
        m_deviceRef = value;
        m_devClass = "";
        m_devInstanceNo = -1;
        if ( m_deviceRef == null ) return; // got a null device

        m_devClass = m_deviceRef.DevClass;
        m_devInstanceNo = m_deviceRef.XmlInstance;
      }
    }


    public int DevInstanceNo
    {
      get { return m_devInstanceNo; }
    }

    public string DeviceName
    {
      get { return m_deviceName; }
      set { m_deviceName = value; }
    }

    public string DeviceClass
    {
      get { return m_devClass; }
    }


    public string OptionName
    {
      get { return m_option; }
    }

    public string NodeText
    {
      get { return m_nodetext; }
      set { m_nodetext = value; DecomposeCommand( ); }
    }

    public string CommandCtrl
    {
      get { return m_cmdCtrl; }
      set { m_cmdCtrl = value; }
    }

    public bool IsStrafeCommand
    {
      get { return m_isStrafe; }
      set { m_isStrafe = value; }
    }

    public DeviceOptionParameter DeviceoptionRef
    {
      get { return m_deviceoptionRef; }
      set {
        m_deviceoptionRef = value;
        if ( m_deviceoptionRef != null )
          m_deviceoptionRef.Action = m_action;
      }
    }

    public bool InvertUsed
    {
      get { return m_invertEnabled; }
      set { m_invertEnabled = value; }
    }

    /// <summary>
    /// Force writing of Inverted
    /// </summary>
    public bool InvertForced
    {
      get { return m_invertForced; }
      set { m_invertForced = value;}
    }

    public bool ExponentUsed
    {
      get { return m_expEnabled; }
      set { m_expEnabled = value; }
    }

    public string Exponent
    {
      get { return m_exponent; }
      set { m_exponent = value; }
    }


    public bool NonLinCurveUsed
    {
      get { return m_ptsEnabled; }
      set { m_ptsEnabled = value; }
    }

    public List<string> NonLinCurvePtsIn
    {
      get { return m_PtsIn; }
      set { m_PtsIn = value; }
    }
    public List<string> NonLinCurvePtsOut
    {
      get { return m_PtsOut; }
      set { m_PtsOut = value; }
    }

    #endregion

    /// <summary>
    /// Reset all items that will be assigned dynamically while scanning the actions
    /// - currently DeviceoptionRef, NodeText
    /// </summary>
    public void ResetDynamicItems()
    {
      // using the public property to ensure the complete processing of the assignment
      // GameDevice = null;
      DeviceoptionRef = null;
      NodeText = "";
    }

    public void AssignDynamicItems( DeviceCls dev, DeviceOptionParameter devOptionRef, string nodeText )
    {
      // using the public property to ensure the complete processing of the assignment
      NodeText = nodeText; // must be first - the content is used later for DeviceOptionRef assignment
      GameDevice = dev;
      DeviceoptionRef = devOptionRef;
    }

    /// <summary>
    /// Derive values from a command (e.g. v_pitch - js1_x)
    /// </summary>
    private void DecomposeCommand()
    {
      // populate from input
      // something like "v_pitch - js1_x" OR "v_pitch - xi_thumbl" OR "v_pitch - ximod+xi_thumbl+xi_mod"
      string cmd = ActionTreeNode.CommandFromActionText( NodeText );
      m_action = ActionTreeNode.ActionFromActionText( NodeText );
      m_cmdCtrl = "";
      if ( !string.IsNullOrWhiteSpace( cmd ) ) {
        // decomp gamepad entries - could have modifiers so check for contains...
        if ( cmd.Contains( "xi_thumblx" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumblx";
          m_deviceName = m_deviceRef.DevName;
        }
        else if ( cmd.Contains( "xi_thumbly" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumbly";
          m_deviceName = m_deviceRef.DevName;
        }
        else if ( cmd.Contains( "xi_thumbrx" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumbrx";
          m_deviceName = m_deviceRef.DevName;
        }
        else if ( cmd.Contains( "xi_thumbry" ) ) {
          // gamepad
          m_cmdCtrl = "xi_thumbry";
          m_deviceName = m_deviceRef.DevName;
        }
        else if ( cmd.Contains( "maxis_x" ) ) {
          // mouse
          m_cmdCtrl = "maxis_x";
          m_deviceName = m_deviceRef.DevName;
        }
        else if ( cmd.Contains( "maxis_y" ) ) {
          // mouse
          m_cmdCtrl = "maxis_y";
          m_deviceName = m_deviceRef.DevName;
        }
        // assume joystick
        else {
          // get parts
          m_cmdCtrl = JoystickCls.ActionFromJsCommand( cmd ); //js1_x -> x; js2_rotz -> rotz
          m_deviceName = m_deviceRef.DevName;
        }
      }
    }

    /// <summary>
    /// Rounds a string to 3 decimals (if it is a number..)
    /// </summary>
    /// <param name="valString">A value string</param>
    /// <returns>A rounded value string - or the string if not a number</returns>
    private string RoundString( string valString )
    {
      double d = 0;
      if ( ( !string.IsNullOrEmpty( valString ) ) && double.TryParse( valString, out d ) ) {
        return d.ToString( "0.000" );
      }
      else {
        return valString;
      }
    }

    /// <summary>
    /// Format an XML -options- node from the tuning contents
    /// </summary>
    /// <returns>The XML string or an empty string</returns>
    public string Options_toXML()
    {
      if ( ( /*SensitivityUsed ||*/ ExponentUsed || InvertForced || InvertUsed || NonLinCurveUsed ) == false ) return ""; // not used
      if ( DevInstanceNo < 1 ) return ""; // no device to assign it to..

      string tmp = "";

      // again we have to translate from internal deviceClass mouse to CIG type keyboard ...
      string type = m_devClass;
      if ( MouseCls.IsDeviceClass( type ) ) type = KeyboardCls.DeviceClass;

      tmp += $"\t<options type=\"{type}\" instance=\"{m_devInstanceNo.ToString( )}\" Product=\"{m_deviceRef.DevName + " " + m_deviceRef.DevGUID}\">\n";// 3.5 do we need Product here ??
      tmp += string.Format( "\t\t<{0} ", m_option );

      if ( InvertUsed ) {
        tmp += string.Format( "invert=\"1\" " ); // if used write it in all the times
      } else if ( InvertForced ) {
        tmp += string.Format( "invert=\"0\" " ); // disable only if requested
      }
      /*
      if ( SensitivityUsed ) {
        tmp += string.Format( "sensitivity=\"{0}\" ", Sensitivity );
      }
      */
      if ( NonLinCurveUsed ) {
        // add exp to avoid merge of things...
        tmp += string.Format( "exponent=\"1.00\" > \n" ); // CIG get to default expo 2.something if not set to 1 here
        tmp += string.Format( "\t\t\t<nonlinearity_curve>\n" );
        tmp += string.Format( "\t\t\t\t<point in=\"{0}\"  out=\"{1}\"/>\n", m_PtsIn[0], m_PtsOut[0] );
        tmp += string.Format( "\t\t\t\t<point in=\"{0}\"  out=\"{1}\"/>\n", m_PtsIn[1], m_PtsOut[1] );
        tmp += string.Format( "\t\t\t\t<point in=\"{0}\"  out=\"{1}\"/>\n", m_PtsIn[2], m_PtsOut[2] );
        tmp += string.Format( "\t\t\t</nonlinearity_curve>\n" );
        tmp += string.Format( "\t\t</{0}> \n", m_option );
      }
      else if ( ExponentUsed ) {
        // only exp used
        tmp += string.Format( "exponent=\"{0}\" /> \n", Exponent );
      }
      else {
        // neither exp or curve
        tmp += string.Format( " /> \n" );// nothing...
      }

      tmp += string.Format( "\t</options>\n \n" );

      return tmp;
    }


    /// <summary>
    /// Read the options from the XML
    ///  can get only the 3 ones for Move Pitch, Yaw, Roll right now
    /// </summary>
    /// <param name="reader">A prepared XML reader</param>
    /// <param name="instance">the Joystick instance number</param>
    /// <returns></returns>
    public bool Options_fromXML( XElement option, string deviceClass, int instance )
    {
      /*
		    <flight_move_pitch exponent="1.00" > 
			    <nonlinearity_curve>
				    <point in="0.182"  out="0.028"/>
				    <point in="0.629"  out="0.235"/>
				    <point in="0.895"  out="0.629"/>
			    </nonlinearity_curve>
		    </flight_move_pitch> 
             */
      m_devClass = deviceClass;
      m_devInstanceNo = instance;
      m_option = option.Name.LocalName;

      // derive from flight_move_pitch || flight_move_yaw || flight_move_roll (nothing bad should arrive here)
      string[] e = m_option.ToLowerInvariant( ).Split( new char[] { '_' } );
      if ( e.Length > 2 ) m_cmdCtrl = e[2]; // TODO - see if m_cmdCtrl is needed to be derived here

      string invert = (string)option.Attribute( "invert" );
      if ( !string.IsNullOrEmpty(invert) ) {
        InvertUsed = false;
        if ( invert == "1" ) InvertUsed = true;
        if ( invert == "0" ) {
          InvertUsed = false;
          InvertForced = true;
        }
      }

      string exponent = (string)option.Attribute( "exponent" );
      if ( !string.IsNullOrEmpty( exponent ) ) {
        Exponent = RoundString( exponent );
        ExponentUsed = true;
      }

      // we may have a nonlin curve...
      XElement nlc = option.Element( "nonlinearity_curve" );
      if ( nlc != null ) {
        m_PtsIn.Clear( ); m_PtsOut.Clear( ); // reset pts
        IEnumerable<XElement> points = from x in nlc.Elements( )
                                       where ( x.Name == "point" )
                                       select x;
        foreach ( XElement point in points ) {
          string ptIn = RoundString( (string)point.Attribute( "in" ) );
          string ptOut = RoundString( (string)point.Attribute( "out" ) );
          m_PtsIn.Add( ptIn ); m_PtsOut.Add( ptOut ); m_ptsEnabled = true;
        }
        ExponentUsed = false; // despite having the Expo=1.00 in the options - it is not used with nonlin curve
        Exponent = RoundString( "1.00" );
      }
      // sanity check - we've have to have 3 pts  here - else we subst
      // add 2nd
      if ( m_PtsIn.Count < 2 ) {
        m_PtsIn.Add( "0.500" ); m_PtsOut.Add( "0.500" );
        log.Info( "Options_fromXML: got only one nonlin point, added (0.5|0.5)" );
      }
      // add 3rd
      if ( m_PtsIn.Count < 3 ) {
        m_PtsIn.Add( "0.750" ); m_PtsOut.Add( "0.750" );
        log.Info( "Options_fromXML: got only two nonlin points, added (0.75|0.75)" );
      }

      return true;
    }

  }
}
