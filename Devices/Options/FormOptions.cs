using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.OGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using SCJMapper_V2.Actions;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.SC;
using SCJMapper_V2.Translation;
using SCJMapper_V2.Common;

namespace SCJMapper_V2.Devices.Options
{
  public partial class FormOptions : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private Label[] lblIn = null;
    private Label[] lblOut = null;

    // Col Index of the ListView items
    private const int LV_DevCtrl = 1;
    private const int LV_Saturation = LV_DevCtrl + 1;
    private const int LV_Deadzone = LV_Saturation + 1;
    private const int LV_Invert = LV_Deadzone + 1;
    private const int LV_Expo = LV_Invert + 1;
    private const int LV_Pt1 = LV_Expo + 1;
    private const int LV_Pt2 = LV_Pt1 + 1;
    private const int LV_Pt3 = LV_Pt2 + 1;

    // allow access by index - index is tabpage index
    private TabPage[] tabs = null;      // Tag = DeviceName
    private ListView[] lviews = null;  // Tag = ToID

    // search the LV with the corresponding Tag
    private ListView FindLV( string toID )
    {
      foreach ( TabPage tp in tabC.TabPages ) {
        if ( toID == (string)tp.Controls["LV"].Tag ) {
          return (ListView)tp.Controls["LV"];
        }
      }
      return null;
    }

    // search the LV with the corresponding Tag
    private ListView FindLVbyGUID( string guid )
    {
      foreach ( TabPage tp in tabC.TabPages ) {
        if ( guid == (string)tp.Tag ) {
          return (ListView)tp.Controls["LV"];
        }
      }
      return null;
    }

    private BezierSeries m_bSeries = new BezierSeries( );

    private enum ESubItems
    {
      EControl = 0,
      EInverted,
      EExponent,
      EPt1,
      EPt2,
      EPt3,

      ESubItems_LAST
    }

    public Size LastSize { get; set; }
    public Point LastLocation { get; set; }

    private Tuningoptions m_tuningRef = null; // will get the current optiontree on call
    public Tuningoptions TuningOptions { get { return m_tuningRef; } set { m_tuningRef = value; } }

    private Deviceoptions m_devOptRef = null; // will get the current device options on call
    public Deviceoptions DeviceOptions { get { return m_devOptRef; } set { m_devOptRef = value; } }

    private DeviceList m_devListRef = null; // will get the current devices on call
    public DeviceList Devicelist
    {
      get { return m_devListRef; }
      set {
        m_devListRef = value;
      }
    }


    #region Form handling

    public FormOptions()
    {
      InitializeComponent( );

      log.Info( "cTor - Entry" );

#if DEBUG
      btDebugStop.Visible = true;
#endif

      // helpers
      lblIn = new Label[] { null, lblLiveIn1, lblLiveIn2, lblLiveIn3, null, null };     // goes with PtNo 1..
      lblOut = new Label[] { null, lblLiveOut1, lblLiveOut2, lblLiveOut3, lblLiveOutExponent }; // goes with PtNo 1..

      // add 5 points to the chart data series ( Zero, user1..3, max)
      for ( int i = 0; i < 5; i++ ) {
        m_bSeries.BezierPoints.Add( new DataPoint( 0, 0 ) );
      }
      m_bSeries.ChartType = SeriesChartType.Line;
      m_bSeries.Name = "Curve";
      chart1.Series[0] = m_bSeries;
      // Create the Marker Series
      chart1.Series.Add( "Marker" );
      chart1.Series[1].ChartType = SeriesChartType.Point;
      chart1.Series[1].MarkerColor = Color.Orange;
      chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
      chart1.Series[1].Points.AddXY( 0, 0 );
      chart1.Series[1].Points.AddXY( 0.25, 0.25 );
      chart1.Series[1].Points.AddXY( 0.5, 0.5 );
      chart1.Series[1].Points.AddXY( 0.75, 0.75 );
      chart1.Series[1].Points.AddXY( 1.0, 1.0 );

      // sliders
      tbDeadzone.Maximum = Deviceoptions.DevOptSliderMax;
      tbDeadzone.TickFrequency = Deviceoptions.DevOptSliderTick;
      tbSaturation.Maximum = Deviceoptions.DevOptSliderMax;
      tbSaturation.TickFrequency = Deviceoptions.DevOptSliderTick;

      log.Debug( "cTor - Exit" );
    }

    private void FormOptions_Load( object sender, EventArgs e )
    {
      log.Debug( "Load - Entry" );

      // Assign Size property - check if on screen, else use defaults
      if ( Commons.IsOnScreen( new Rectangle( AppSettings.Instance.FormOptionsLocation, AppSettings.Instance.FormOptionsSize ) ) ) {
        this.Size = AppSettings.Instance.FormOptionsSize;
        this.Location = AppSettings.Instance.FormOptionsLocation;
      }

      Tx.LocalizeControlTree( this );
      // localization with generic IDs
      cbxUseDeadzone.Text = Tx.Translate( "xDeadzone" );
      cbxUseSaturation.Text = Tx.Translate( "xSaturation" );
      lblInverted.Text = Tx.Translate( "xInvert" );
      cbxLiveInvert.Text = Tx.Translate( "xYes" );
      cbxLiveInvertForced.Text = Tx.Translate( "xForced" );
      rbUseExpo.Text = Tx.Translate( "xExponent" );
      rbUsePts.Text = Tx.Translate( "xCurve" );
      rbUseNone.Text = Tx.Translate( "xNone" );
      rbLivePt1.Text = Tx.Translate( "xPoint1" );
      rbLivePt2.Text = Tx.Translate( "xPoint2" );
      rbLivePt3.Text = Tx.Translate( "xPoint3" );
      // END OF localization with generic IDs


      DeviceTabsSetup( );

      PrepOptionsTab( );

      TabChangeHandling( ); // select the first LV item

      log.Debug( "Load - Exit" );
    }

    private void FormOptions_LocationChanged( object sender, EventArgs e )
    {
      if ( this.WindowState == FormWindowState.Normal )
        LastLocation = this.Location;

    }

    private void FormOptions_SizeChanged( object sender, EventArgs e )
    {
      if ( this.WindowState == FormWindowState.Normal )
        LastSize = this.Size;
    }


    private void FormOptions_FormClosing( object sender, FormClosingEventArgs e )
    {
      log.Debug( "FormClosing - Entry" );
      if ( this.WindowState== FormWindowState.Normal ) {
        AppSettings.Instance.FormOptionsLocation = this.Location;
        AppSettings.Instance.FormOptionsSize = this.Size;
      }

      // have to carry on current edits - NO ListView SelectionChange Event happens 
      try {
        if ( ( (ListView)tabC.SelectedTab.Controls["LV"] ).SelectedItems.Count > 0 ) {
          // we push the current one back to tuning and the list view
          UpdateLiveTuning( );
          UpdateLiveDevOption( );
        }
      }
      catch {
        ;
      }

      log.Info( "Closed now" );
    }

    #endregion


    #region Setup

    // setup all device tabs
    private void DeviceTabsSetup()
    {
      tabs = new TabPage[] { };
      lviews = new ListView[] { };

      tabC.TabPages.Clear( );
      for ( int idx = 0; idx < m_devListRef.Count; idx++ ) {
        if ( m_devListRef[idx].XmlInstance > 0 ) {
          // only with mapped devices
          ListView lview = new ListView( );
          Array.Resize( ref lviews, lviews.Length + 1 ); lviews[lviews.Length - 1] = lview;
          // copied from Designer.cs
          lview.Dock = DockStyle.Fill;
          lview.Location = new Point( 3, 3 );
          lview.Name = "LV";
          lview.Size = new Size( 650, 629 );
          lview.TabIndex = 0;
          lview.UseCompatibleStateImageBehavior = false;
          lview.View = View.Details;
          lview.SelectedIndexChanged += new EventHandler( this.lvOptionTree_SelectedIndexChanged );

          lview.Tag = Tuningoptions.TuneOptionIDfromJsN( m_devListRef[idx].DevClass, m_devListRef[idx].XmlInstance );
          //m_devListRef[idx].DevInstanceGUID; // find an LV

          TabPage tab = new TabPage( m_devListRef[idx].DevName );
          Array.Resize( ref tabs, tabs.Length + 1 ); tabs[tabs.Length - 1] = tab;

          // copied from Designer.cs
          tab.Controls.Add( lview );
          tab.Location = new Point( 4, 22 );
          tab.Name = "Tab";
          tab.Padding = new Padding( 3 );
          tab.Size = new Size( 656, 635 );
          tab.TabIndex = 0;
          tab.UseVisualStyleBackColor = true;

          tab.Tag = m_devListRef[idx].DevInstanceGUID;    // find a device Tab
          tabC.TabPages.Add( tab );

          DeviceTabSetup( tabC.TabPages.Count - 1 ); // last added
        }
      }
    }

    // setup one device tab with index
    private void DeviceTabSetup( int tabIndex )
    {
      if ( tabIndex < 0 ) return;

      //TabPage tab = tabs[tabIndex];
      ListView lview = lviews[tabIndex];

      ListViewSetup( lview );

      OptionTreeSetup( lview, (string)tabs[tabIndex].Tag );

    }

    // Basic list view setup
    private void ListViewSetup( ListView lview )
    {
      if ( lview == null ) return;

      lview.Clear( );
      lview.View = View.Details;
      lview.LabelEdit = false;
      lview.AllowColumnReorder = false;
      lview.FullRowSelect = true;
      lview.GridLines = true;
      lview.CheckBoxes = false;
      lview.MultiSelect = false;
      lview.HideSelection = false;

      string instText = " - " + Tx.Translate("xInstance") +" =" + Tuningoptions.XmlInstanceFromID( (string)lview.Tag );
      lview.Columns.Add( Tx.Translate( "xOption" ) + " " + instText, 180, HorizontalAlignment.Left );
      lview.Columns.Add( Tx.Translate( "xDevControl"), 80, HorizontalAlignment.Left );
      lview.Columns.Add( Tx.Translate( "xSaturation"), 80, HorizontalAlignment.Center );
      lview.Columns.Add(Tx.Translate( "xDeadzone"), 80, HorizontalAlignment.Center );
      lview.Columns.Add( Tx.Translate( "xInvert"), 50, HorizontalAlignment.Center );
      lview.Columns.Add( Tx.Translate( "xExponent"), 50, HorizontalAlignment.Center );
      lview.Columns.Add( Tx.Translate( "xPoint1"), 90, HorizontalAlignment.Center );
      lview.Columns.Add( Tx.Translate( "xPoint2" ), 90, HorizontalAlignment.Center );
      lview.Columns.Add( Tx.Translate( "xPoint3" ), 90, HorizontalAlignment.Center );

      lview.ShowGroups = true;
    }

    // make this in one contained function - easier to maintain
    private void ListViewItemSetup( ListViewItem lvi )
    {
      lvi.SubItems.Add( "" ); // dev control
      lvi.SubItems.Add( "" ); lvi.SubItems.Add( "" ); // saturation + deadzone
      lvi.SubItems.Add( "" ); lvi.SubItems.Add( "" ); // invert + expo
      lvi.SubItems.Add( "" ); lvi.SubItems.Add( "" ); lvi.SubItems.Add( "" ); // 3 points
    }


    // Setup the listview for a particular Device Tab
    private void OptionTreeSetup( ListView lview, string devGUID )
    {
      log.Debug( "OptionTreeSetup - Entry" );
      if ( m_tuningRef == null ) {
        log.Error( "- OptionTreeSetup: m_tuningRef not assigned" );
        return;
      }
      if ( m_devOptRef == null ) {
        log.Error( "- OptionTreeSetup: m_devOptRef not assigned" );
        return;
      }

      DeviceTuningParameter tuning = null;
      string option = "";
      ListViewGroup lvg = null;
      ListViewItem lvi;
      List<string> devNamesDone = new List<string>( );
      string devClass = DeviceCls.DeviceClass;

      // scan for DeviceOptions to be added for this device(GUID)
      foreach ( KeyValuePair<string, DeviceOptionParameter> kv in m_devOptRef ) {
        if ( kv.Value.DevInstanceGUID == devGUID ) {
          devClass = kv.Value.DevClass;
          if ( !devNamesDone.Contains( kv.Value.DevName ) ) {
            lvg = new ListViewGroup(Tx.Translate( "xDeviceOptions" )); lview.Groups.Add( lvg );
            devNamesDone.Add( kv.Value.DevName );
          }
          lvi = new ListViewItem( kv.Value.CommandCtrl, lvg ) { Name = kv.Value.DoID };
          lview.Items.Add( lvi );
          ListViewItemSetup( lvi );
          UpdateLvDevOptionFromLiveValues( kv.Value );
        }
      }

      if ( DeviceCls.IsUndefined(devClass) ) {
        // still undefined - solve here
        if ( devGUID == DeviceInst.GamepadRef?.DevInstanceGUID )
          devClass = GamepadCls.DeviceClass;
        else if ( devGUID == DeviceInst.MouseRef?.DevInstanceGUID )
          devClass = MouseCls.DeviceClass;
      }

      int lvCount = 0;
      // then the functions
      string group = "";
      foreach ( var pOpt in OptionTree.ProfileOptions.Where( x => x.DeviceClass == devClass ) ) {
        if ( pOpt.OptGroup != group ) {
          group = pOpt.OptGroup;
          lvg = new ListViewGroup( lvCount++.ToString( ), SCUiText.Instance.Text( group ) ); lview.Groups.Add( lvg );
        }

        option = pOpt.OptName; tuning = m_tuningRef.TuningItem( (string)lview.Tag, option ); m_live.Load( tuning );
        if ( m_live.used ) {
          lvi = new ListViewItem( SCUiText.Instance.Text( option ), lvg ) { Name = option };
          lvi.Name = option; lview.Items.Add( lvi ); ListViewItemSetup( lvi );
          UpdateLvOptionFromLiveValues( m_live );
        }
      }
      log.Debug( "OptionTreeSetup - Exit" );
    }


    #endregion

    private void button2_Click( object sender, EventArgs e )
    {
      ; // break
    }

    #region class LiveValues (internal class)

    private class LiveValues
    {
      // load live from TuningParameters
      public void Load( DeviceTuningParameter dp )
      {
        Reset( );
        if ( dp != null ) {
          optionName = dp.OptionName;
          used = true; // always

          isTuningItem = true;
          isDevOptionItem = false;

          gameDeviceRef = dp.GameDevice;
          nodetext = dp.NodeText;
          if ( !string.IsNullOrEmpty( dp.NodeText ) ) {
            string[] e = nodetext.Split( new char[] { ActionTreeInputNode.RegDiv, ActionTreeInputNode.ModDiv }, StringSplitOptions.RemoveEmptyEntries );
            if ( e.Length > 0 )
              control = e[1].TrimEnd( );
            else
              control = dp.NodeText;
          }
          else if ( gameDeviceRef != null ) {
            //control = gameDeviceRef.DevName;
          }
          else {
            control = "n.a.";
          }
          command = dp.CommandCtrl;

          // the device option data if available
          if ( dp.DeviceoptionRef != null ) {
            isDevOptionItem = true;

            deadzoneUsed = dp.DeviceoptionRef.DeadzoneUsed;
            deadzoneS = dp.DeviceoptionRef.Deadzone;

            saturationSupported = dp.DeviceoptionRef.SaturationSupported;
            saturationUsed = dp.DeviceoptionRef.SaturationUsed;
            saturationS = dp.DeviceoptionRef.Saturation;
          }
          else {
            deadzoneUsed = false;
            saturationSupported = false;
            saturationUsed = false;
          }

          // the tuning data
          invertForced = dp.InvertForced;
          invertUsed = dp.InvertUsed;
          exponentUsed = dp.ExponentUsed;
          exponentS = dp.Exponent;
          nonLinCurveUsed = dp.NonLinCurveUsed;
          if ( dp.NonLinCurveUsed ) {
            nonLinCurve.Curve( float.Parse( dp.NonLinCurvePtsIn[0] ), float.Parse( dp.NonLinCurvePtsOut[0] ),
                                   float.Parse( dp.NonLinCurvePtsIn[1] ), float.Parse( dp.NonLinCurvePtsOut[1] ),
                                   float.Parse( dp.NonLinCurvePtsIn[2] ), float.Parse( dp.NonLinCurvePtsOut[2] ) );
          }
          else {
            // dummy curve
            nonLinCurve.Curve( 0.25f, 0.25f, 0.5f, 0.5f, 0.75f, 0.75f );
          }
        }
      }

      // load live from DevOptions
      public void Load( DeviceOptionParameter dp )
      {
        Reset( );
        if ( dp != null ) {
          optionName = dp.CommandCtrl;
          used = true;

          isTuningItem = false;
          isDevOptionItem = true;

          nodetext = dp.CommandCtrl;
          control = dp.CommandCtrl;
          command = dp.CommandCtrl;
          // the device option data
          deadzoneUsed = dp.DeadzoneUsed;
          deadzoneS = dp.Deadzone;

          saturationSupported = dp.SaturationSupported;
          saturationUsed = dp.SaturationUsed;
          saturationS = dp.Saturation;

          // tuning data is not used here
          invertForced = false;
          invertUsed = false;
          exponentUsed = false;
          nonLinCurveUsed = false;
        }
      }

      // update the TuningParameters
      public void Update( ref DeviceTuningParameter dp )
      {
        if ( !used ) return;
        // don't return strings to control the device
        /* This is preassigned now - delete if finished
        if ( AcceptGameDevice ) {
          dp.GameDevice = gameDeviceRef;
        }
        */
        dp.InvertForced = invertForced;
        dp.InvertUsed = invertUsed;

        // update device options
        if ( dp.DeviceoptionRef != null ) {
          dp.DeviceoptionRef.DeadzoneUsed = deadzoneUsed;
          dp.DeviceoptionRef.Deadzone = deadzoneS;
          if ( saturationSupported ) {
            dp.DeviceoptionRef.SaturationUsed = saturationUsed;
            dp.DeviceoptionRef.Saturation = saturationS;
          }
          else {
            dp.DeviceoptionRef.SaturationUsed = false;
          }
        }
        // update tuning
        dp.ExponentUsed = exponentUsed;
        dp.Exponent = exponentS;
        dp.NonLinCurveUsed = nonLinCurveUsed;
        List<string> pts = new List<string>( );
        pts.Add( nonLinCurve.Pt( 0 ).X.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 1 ).X.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 2 ).X.ToString( "0.000" ) );
        dp.NonLinCurvePtsIn = pts;
        pts = new List<string>( );
        pts.Add( nonLinCurve.Pt( 0 ).Y.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 1 ).Y.ToString( "0.000" ) ); pts.Add( nonLinCurve.Pt( 2 ).Y.ToString( "0.000" ) );
        dp.NonLinCurvePtsOut = pts;
      }

      // update the DeviceOptions
      public void Update( ref DeviceOptionParameter dp )
      {
        if ( !used ) return;
        // don't return strings to control the device
        dp.DeadzoneUsed = deadzoneUsed;
        dp.Deadzone = deadzoneS;
        if ( saturationSupported ) {
          dp.SaturationUsed = saturationUsed;
          dp.Saturation = saturationS;
        }
        else {
          dp.SaturationUsed = false;
        }
      }

      // reset the live parameters
      public void Reset()
      {
        used = false;
        nodetext = ""; control = ""; command = "";
        m_range = 1000.0; m_sign = 1.0;
        invertForced = false; invertUsed = false;
        deadzoneUsed = false; deadzone = 0.0;
        saturationSupported = false; saturationUsed = false; saturation = 1000.0;
        exponentUsed = false; exponent = 1.0;
        nonLinCurveUsed = false;
      }

      // Context
      public bool used = false;
      public string optionName = "";

      public string nodetext = "";  // the node text
      public string control = ""; // the device control item e.g.  js2_x
      public string command = ""; // the control item used to get the XDevice Input
      public string devOptCommand { get { return command.Replace( "throttle", "" ); } } // have to get rid of throttle for devOptions..
      public bool AcceptGameDevice { get { return string.IsNullOrEmpty( nodetext ); } } // this is how we do it..
      public DeviceCls gameDeviceRef = null;

      public bool isTuningItem = false;
      public bool isDevOptionItem = false;

      // calc values
      private const double MAX_DZ = 160.0; // avoid range issues and silly values..
      private const double MIN_SAT = 200.0; // avoid range issues and silly values..
      private double m_range = 1000.0;
      private double m_sign = 1.0;


      // set values
      public bool m_invertForced = false;
      public bool invertForced { get { return m_invertForced; } set { m_invertForced = value; } }
      public bool m_invertUsed = false;
      public bool invertUsed { get { return m_invertUsed; } set { m_invertUsed = value; m_sign = m_invertUsed ? -1.0 : 1.0; } }
      public string invertS
      {
        get {
          if ( invertUsed )
            return "yes";
          else
            return "no";
        }
        set {
          if ( value == "yes" )
            m_invertUsed = true;
          else
            m_invertUsed = false;

          m_sign = m_invertUsed ? -1.0 : 1.0;
        }
      }

      public bool deadzoneUsed = false;
      private double m_deadzone = 0.0; // stores 1000 * set value
      public double deadzone { get { return m_deadzone; } set { m_deadzone = ( value > MAX_DZ ) ? MAX_DZ : value; m_range = m_saturation - m_deadzone; } }
      public string deadzoneS // get/set game value 0..1.000
      {
        get { return ( deadzone / 1000.0 ).ToString( "0.000" ); }
        set {
          double f;
          if ( double.TryParse( value, out f ) ) {
            deadzone = f * 1000.0;
          }
          else {
            deadzone = 0.0;
          }
        }
      }

      public bool saturationSupported = false;
      public bool saturationUsed = false;
      private double m_saturation = 1000.0;// stores 1000 * set value
      public double saturation { get { return m_saturation; } set { m_saturation = ( value < MIN_SAT ) ? MIN_SAT : value; m_range = m_saturation - m_deadzone; } }
      public string saturationS // get/set game value 0..1.000
      {
        get { return ( m_saturation / 1000.0 ).ToString( "0.000" ); }
        set {
          double f;
          if ( double.TryParse( value, out f ) ) {
            saturation = f * 1000.0;
          }
          else {
            saturation = 1000.0;
          }
        }
      }

      public bool exponentUsed = false;
      public double exponent = 1.0F;
      public string exponentS
      {
        get {
          if ( exponentUsed )
            return exponent.ToString( "0.000" );
          else
            return "1.000";
        }
        set {
          double f;
          if ( double.TryParse( value, out f ) ) {
            exponent = f;
          }
          else {
            exponent = 1.0;
          }
        }
      }

      public bool nonLinCurveUsed = false;
      public xyPoints nonLinCurve = new xyPoints( 1000 );  // max val of Joystick Input

      /// <summary>
      /// returns the Point string value
      /// </summary>
      /// <param name="ptIndex">The IN point (1..3)</param>
      /// <returns>A formatted string</returns>
      public string PtInS( int ptIndex )
      {
        if ( !nonLinCurveUsed )
          return ( 0.25 * ptIndex ).ToString( "0.000" );
        else
          return nonLinCurve.Pt( ptIndex - 1 ).X.ToString( "0.000" );
      }
      /// <summary>
      /// returns the Point string value
      /// </summary>
      /// <param name="ptIndex">The OUT point (1..3)</param>
      /// <returns>A formatted string</returns>
      public string PtOutS( int ptIndex )
      {
        if ( !nonLinCurveUsed )
          return ( 0.25 * ptIndex ).ToString( "0.000" );
        else
          return nonLinCurve.Pt( ptIndex - 1 ).Y.ToString( "0.000" );
      }
      /// <summary>
      /// returns the Point string value pair
      /// </summary>
      /// <param name="ptIndex">The IN / OUT point (1..3)</param>
      /// <returns>A formatted string</returns>
      public string PtS( int ptIndex )
      {
        if ( !nonLinCurveUsed )
          return ( 0.25 * ptIndex ).ToString( "0.000" ) + " / " + ( 0.25 * ptIndex ).ToString( "0.000" );
        else
          return nonLinCurve.Pt( ptIndex - 1 ).X.ToString( "0.000" ) + " / " + nonLinCurve.Pt( ptIndex - 1 ).Y.ToString( "0.000" );
      }
    } // class LiveValues

    #endregion

    private LiveValues m_live = new LiveValues( );
    private DeviceTuningParameter m_liveTuning = null;
    private DeviceOptionParameter m_liveDevOption = null;

    // handles a Tab change - selecting the first item and make it visible etc.
    private void TabChangeHandling()
    {
      // select the first item in the LV control of the tab
      if ( tabC.SelectedTab.Controls.ContainsKey( "LV" ) ) {

        if ( ( (ListView)tabC.SelectedTab.Controls["LV"] ).Items.Count > 0 )
          ( (ListView)tabC.SelectedTab.Controls["LV"] ).Items[0].Selected = true;
        // select the LV itself
        tabC.SelectedTab.Controls["LV"].Select( );

        UpdateGUIFromLiveValues( m_live );
      }
    }

    // update the GUI from Live
    private void UpdateGUIFromLiveValues( LiveValues lv )
    {
      log.Debug( "UpdateGUIFromLiveValues - Entry" );
      // guard input mess handling...
      m_updatingPts++;

      if ( !lv.used ) {
        // reset
        pnlOptionInput.Visible = false;
        pnlDevOptionInput.Visible = false;

        //        pnlOptionInput.Enabled = false;
        lblLiveNodetext.Text = "---";
        cbxLiveInvert.Checked = false;
        lblLiveOutDeadzone.Text = "0.000"; cbxUseDeadzone.Checked = false; tbDeadzone.Enabled = false;
        lblLiveOutSaturation.Text = "1.000"; cbxUseSaturation.Checked = false; tbSaturation.Enabled = false; cbxUseSaturation.Enabled = false;
        lblLiveOutExponent.Text = "1.000"; rbLivePtExponent.Checked = false;
        lblLiveIn1.Text = "0.250"; lblLiveOut1.Text = "0.250"; lblLiveIn2.Text = "0.500"; lblLiveOut2.Text = "0.500"; lblLiveIn3.Text = "0.750"; lblLiveOut3.Text = "0.750";
        rbLivePt1.Checked = false; rbLivePt2.Checked = false; rbLivePt3.Checked = false;
        rbUseNone.Checked = true;
        cbxLiveInvert.Enabled = false;

        m_updatingPts--; // end guard
        log.Debug( "UpdateGUIFromLiveValues - Exit 'not used'" );
        return; // EXIT
      }

      // get values from Live storage
      pnlOptionInput.Visible = lv.isTuningItem;
      pnlDevOptionInput.Visible = lv.isDevOptionItem;
      //      pnlDevOptionInput.Visible = !lv.AcceptGameDevice; // cannot assign DevOptions to Tuning parameters without Action (will just dumped the Option only)

      lblLiveNodetext.Text = lv.nodetext;


      if ( lv.deadzoneUsed ) lblLiveOutDeadzone.Text = lv.deadzoneS;
      cbxUseDeadzone.Checked = lv.deadzoneUsed;

      if ( lv.saturationSupported && lv.saturationUsed ) lblLiveOutSaturation.Text = lv.saturationS;
      cbxUseSaturation.Enabled = lv.saturationSupported;
      cbxUseSaturation.Checked = ( lv.saturationSupported && lv.saturationUsed );

      cbxLiveInvert.Enabled = true;
      cbxLiveInvert.Checked = lv.invertUsed;
      cbxLiveInvertForced.Enabled = true;
      cbxLiveInvertForced.Checked = lv.invertForced;

      rbUseNone.Checked = true; // init - we will see later if it changes (guarded - so no sideeffects from Checked Events)
      if ( lv.exponentUsed ) lblLiveOutExponent.Text = lv.exponentS;
      rbUseExpo.Checked = lv.exponentUsed;  // Update to used one - if so..
      rbLivePtExponent.Checked = lv.exponentUsed;

      if ( lv.nonLinCurveUsed ) {
        lblLiveIn1.Text = lv.nonLinCurve.Pt( 0 ).X.ToString( "0.000" ); lblLiveOut1.Text = lv.nonLinCurve.Pt( 0 ).Y.ToString( "0.000" );
        lblLiveIn2.Text = lv.nonLinCurve.Pt( 1 ).X.ToString( "0.000" ); lblLiveOut2.Text = lv.nonLinCurve.Pt( 1 ).Y.ToString( "0.000" );
        lblLiveIn3.Text = lv.nonLinCurve.Pt( 2 ).X.ToString( "0.000" ); lblLiveOut3.Text = lv.nonLinCurve.Pt( 2 ).Y.ToString( "0.000" );
      }
      rbUsePts.Checked = lv.nonLinCurveUsed; // Update to used one - if so..
      rbLivePt1.Checked = lv.nonLinCurveUsed; rbLivePt2.Checked = false; rbLivePt3.Checked = false; // mark Pt1 to start with

      m_updatingPts--; // end guard

      log.Debug( "UpdateGUIFromLiveValues - Exit" );
    }


    private void UpdateLvOptionFromLiveValues( LiveValues lval )
    {
      log.Debug( "UpdateLvOptionFromLiveValues - Entry" );
      if ( lval.gameDeviceRef == null )
        return; // should finally not happen..

      // find the row entry
      ListView lv = FindLVbyGUID( lval.gameDeviceRef.DevInstanceGUID );
      if ( lv == null ) return; // happens at startup when not all are created yet

      if ( !lv.Items.ContainsKey( lval.optionName ) ) {
        log.Error( "ERROR: UpdateLvOptionFromLiveValues - Did not found Option: " + lval.optionName );
        log.Debug( "UpdateLvOptionFromLiveValues - Exit 'not used'" );
        return;
      }

      ListViewItem lvi = lv.Items[lval.optionName];
      if ( !lval.used ) {
        // leave alone.. for next time enabling it
        lvi.SubItems[LV_DevCtrl].Text = m_live.control; // js4_x
        lvi.SubItems[LV_Invert].Text = "---"; lvi.SubItems[LV_Expo].Text = "---"; // inverted .. expo
        lvi.SubItems[4].Text = "--- / ---"; lvi.SubItems[5].Text = "--- / ---"; lvi.SubItems[6].Text = "--- / ---"; // pt1..3
      }
      else {
        lvi.SubItems[LV_DevCtrl].Text = m_live.control; // js4_x
        lvi.SubItems[LV_Invert].Text = m_live.invertS;
        if ( m_live.exponentUsed )
          lvi.SubItems[LV_Expo].Text = m_live.exponentS; // inverted .. expo
        else
          lvi.SubItems[LV_Expo].Text = "---"; // inverted .. expo
        if ( m_live.nonLinCurveUsed ) {
          lvi.SubItems[LV_Pt1].Text = m_live.PtS( 1 ); lvi.SubItems[LV_Pt2].Text = m_live.PtS( 2 ); lvi.SubItems[LV_Pt3].Text = m_live.PtS( 3 ); // pt1..3
        }
        else {
          lvi.SubItems[LV_Pt1].Text = "--- / ---"; lvi.SubItems[LV_Pt2].Text = "--- / ---"; lvi.SubItems[LV_Pt3].Text = "--- / ---"; // pt1..3
        }
      }

      log.Debug( "UpdateLvOptionFromLiveValues - Exit" );
    }


    private void UpdateLvDevOptionFromLiveValues( DeviceOptionParameter dp )
    {
      log.Debug( "UpdateLvDevOptionFromLiveValues - Entry" );
      ListView lv = FindLVbyGUID( dp.DevInstanceGUID );
      if ( !lv.Items.ContainsKey( dp.DoID ) ) {
        log.Error( "ERROR: UpdateLvDevOptionFromLiveValues - Did not found Option: " + dp.DoID );
        log.Debug( "UpdateLvDevOptionFromLiveValues - Exit 'not used'" );
        return;
      }

      ListViewItem lvi = lv.Items[dp.DoID];
      lvi.SubItems[LV_DevCtrl].Text = SCUiText.Instance.Text( dp.Action ); // Action 
      if ( dp.SaturationSupported && dp.SaturationUsed )
        lvi.SubItems[LV_Saturation].Text = dp.Saturation; // saturation
      else if ( dp.SaturationSupported )
        lvi.SubItems[LV_Saturation].Text = "---";
      else
        lvi.SubItems[LV_Saturation].Text = "n.a.";

      if ( dp.DeadzoneUsed )
        lvi.SubItems[LV_Deadzone].Text = dp.Deadzone; // deadzone
      else
        lvi.SubItems[LV_Deadzone].Text = "---";

      log.Debug( "UpdateLvDevOptionFromLiveValues - Exit" );
    }


    #region Charts section

    // Chart - move Pts

    /// <summary>
    /// Evaluate which tune parameter has the chart input
    /// </summary>
    private void EvalChartInput()
    {
      log.Debug( "EvalChartInput - Entry" );

      m_hitPt = 0;
      if ( rbUsePts.Checked && rbLivePt1.Enabled && rbLivePt1.Checked ) m_hitPt = 1;
      if ( rbUsePts.Checked && rbLivePt2.Enabled && rbLivePt2.Checked ) m_hitPt = 2;
      if ( rbUsePts.Checked && rbLivePt3.Enabled && rbLivePt3.Checked ) m_hitPt = 3;
      if ( rbUseExpo.Checked && rbLivePtExponent.Enabled && rbLivePtExponent.Checked ) m_hitPt = 4;

      //      if ( m_hitPt > 0 ) return;

      // slider fudge
      tbDeadzone.Enabled = false;
      if ( cbxUseDeadzone.Enabled && cbxUseDeadzone.Checked ) {
        tbDeadzone.Enabled = true;
        tbDeadzone.Value = Deviceoptions.DeadzoneToSlider( lblLiveOutDeadzone.Text );
      }

      tbSaturation.Enabled = false;
      if ( cbxUseSaturation.Enabled && cbxUseSaturation.Checked ) {
        tbSaturation.Enabled = true;
        tbSaturation.Value = Deviceoptions.SaturationToSlider( lblLiveOutSaturation.Text );
      }
      EvalSlider( );

      log.Debug( "EvalChartInput - Exit" );
    }


    private int m_updatingPts = 0;
    /// <summary>
    /// Handle change of the mouse input within the chart
    /// </summary>
    private void rbPtAny_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_updatingPts > 0 ) return;
      // start guard
      m_updatingPts++;

      //Depending on the selected radio button change the inputs..

      // prev state here
      if ( m_live.exponentUsed ) {
        if ( rbUseExpo.Checked ) {
          ; // just nothing
        }
        else if ( rbUsePts.Checked ) {
          // switch to NonLin
          rbLivePtExponent.Enabled = false; rbLivePtExponent.Checked = false;
          m_live.exponentUsed = false;
          rbLivePt1.Enabled = true; rbLivePt2.Enabled = true; rbLivePt3.Enabled = true;
          rbLivePt1.Checked = true; // start with the first one
          m_live.nonLinCurveUsed = true;
        }
        else {
          //Switch to none
          rbLivePtExponent.Enabled = false; rbLivePtExponent.Checked = false;
          m_live.exponentUsed = false;
        }

      }
      else if ( m_live.nonLinCurveUsed ) {
        if ( rbUseExpo.Checked ) {
          // switch to expo
          rbLivePt1.Enabled = false; rbLivePt2.Enabled = false; rbLivePt3.Enabled = false;
          rbLivePt1.Checked = false; rbLivePt2.Checked = false; rbLivePt3.Checked = false;
          m_live.nonLinCurveUsed = false;
          rbLivePtExponent.Enabled = true; rbLivePtExponent.Checked = true;
          m_live.exponentUsed = true;
        }
        else if ( rbUsePts.Checked ) {
          ; // just nothing
        }
        else {
          //Switch to none
          rbLivePt1.Enabled = false; rbLivePt2.Enabled = false; rbLivePt3.Enabled = false;
          rbLivePt1.Checked = false; rbLivePt2.Checked = false; rbLivePt3.Checked = false;
          m_live.nonLinCurveUsed = false;
        }

      }
      else {
        // prev was None
        if ( rbUseExpo.Checked ) {
          // switch to expo
          rbLivePtExponent.Enabled = true; rbLivePtExponent.Checked = true;
          m_live.exponentUsed = true;
        }
        else if ( rbUsePts.Checked ) {
          // switch to NonLin
          rbLivePt1.Enabled = true; rbLivePt2.Enabled = true; rbLivePt3.Enabled = true;
          rbLivePt1.Checked = true; // start with the first one
          m_live.nonLinCurveUsed = true;
        }
        else {
          //Switch to none
          ; // just nothing
        }
      }

      // EvalChartInput( );

      UpdateChartItems( );
      // end guard
      m_updatingPts--;
    }


    // handle mouse interaction with the chart

    int m_hitPt = 0;
    bool m_hitActive = false;
    int mX = 0; int mY = 0;

    /// <summary>
    /// Update the graph from changes of acitve label values
    /// </summary>
    private void UpdateChartItems()
    {
      log.Debug( "UpdateChartItems - Entry" );

      bool deadzoneUsed = true;
      bool satUsed = true;
      bool satSupp = true;
      bool expUsed = true;
      bool ptsUsed = true;

      // see what is on display..
      if ( m_live == null ) {

        return;
      }
      deadzoneUsed = m_live.deadzoneUsed;
      satSupp = m_live.saturationSupported;
      satUsed = ( satSupp && m_live.saturationUsed );
      expUsed = m_live.exponentUsed;
      ptsUsed = m_live.nonLinCurveUsed;
      lblGraphDeadzone.Text = m_live.deadzoneS;
      lblGraphSaturation.Text = m_live.saturationS;

      // generic part
      lblGraphDeadzone.Visible = deadzoneUsed;
      lblGraphSaturation.Visible = satUsed;


      rbLivePtExponent.Enabled = expUsed;
      rbLivePt1.Enabled = ptsUsed; rbLivePt2.Enabled = ptsUsed; rbLivePt3.Enabled = ptsUsed;
      EvalChartInput( );  // review active chart input

      if ( !tbDeadzone.Enabled ) lblLiveOutDeadzone.Text = "0.000";
      if ( !tbSaturation.Enabled ) lblLiveOutSaturation.Text = "1.000";

      if ( expUsed ) {
        // Exp mode
        double expo = double.Parse( lblOut[4].Text );
        // dont touch zero Point
        m_bSeries.BezierPoints[1].SetValueXY( 0.25, Math.Pow( 0.25, expo ) );
        m_bSeries.BezierPoints[2].SetValueXY( 0.5, Math.Pow( 0.5, expo ) );
        m_bSeries.BezierPoints[3].SetValueXY( 0.75, Math.Pow( 0.75, expo ) );
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, 1.0 );

      }
      else if ( ptsUsed ) {
        // Pts mode
        // dont touch zero Point
        for ( int i = 1; i <= 3; i++ ) {
          m_bSeries.BezierPoints[i].SetValueXY( float.Parse( lblIn[i].Text ), float.Parse( lblOut[i].Text ) );
        }
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, 1.0 );

      }
      else {
        // linear
        // dont touch zero Point
        m_bSeries.BezierPoints[1].SetValueXY( 0.25, 0.25 );
        m_bSeries.BezierPoints[2].SetValueXY( 0.5, 0.5 );
        m_bSeries.BezierPoints[3].SetValueXY( 0.75, 0.75 );
        m_bSeries.BezierPoints[4].SetValueXY( 1.0, 1.0 );
      }
      // update markers from curve points
      chart1.Series[1].Points[1] = m_bSeries.BezierPoints[1];
      chart1.Series[1].Points[2] = m_bSeries.BezierPoints[2];
      chart1.Series[1].Points[3] = m_bSeries.BezierPoints[3];
      chart1.Series[1].Points[4] = m_bSeries.BezierPoints[4];

      m_bSeries.Invalidate( chart1 );

      log.Debug( "UpdateChartItems - Exit" );
    }



    private void chartPoint_MouseDown( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      m_hitActive = true; // activate movement tracking
      mX = e.X; mY = e.Y; // save initial loc to get deltas
    }

    private void chartPoint_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      if ( m_hitActive ) {
        if ( m_hitPt < 1 ) {
          // nothing selected ...
        }
        else if ( m_hitPt <= 3 ) {
          // Pt1..3
          double newX = double.Parse( lblIn[m_hitPt].Text ) + ( e.X - mX ) * 0.001f; mX = e.X;
          newX = ( newX > 1.0f ) ? 1.0f : newX;
          newX = ( newX < 0.0f ) ? 0.0f : newX;
          lblIn[m_hitPt].Text = newX.ToString( "0.000" );

          double newY = double.Parse( lblOut[m_hitPt].Text ) + ( e.Y - mY ) * -0.001f; mY = e.Y;
          newY = ( newY > 1.0f ) ? 1.0f : newY;
          newY = ( newY < 0.0f ) ? 0.0f : newY;
          lblOut[m_hitPt].Text = newY.ToString( "0.000" );

          // update chart (Points[0] is zero)
          m_bSeries.BezierPoints[m_hitPt].SetValueXY( newX, newY );
          // update markers from curve points
          chart1.Series[1].Points[m_hitPt] = m_bSeries.BezierPoints[m_hitPt];

        }
        else if ( m_hitPt == 4 ) {
          // Exponent
          double newY = double.Parse( lblOut[m_hitPt].Text ) + ( e.Y - mY ) * 0.01f; mY = e.Y;
          newY = ( newY > 3.0f ) ? 3.0f : newY;
          newY = ( newY < 0.5f ) ? 0.5f : newY;
          lblOut[m_hitPt].Text = newY.ToString( "0.000" );

          // update chart (Points[0] is zero)
          m_bSeries.BezierPoints[1].SetValueXY( 0.25, Math.Pow( 0.25, newY ) );
          m_bSeries.BezierPoints[2].SetValueXY( 0.5, Math.Pow( 0.5, newY ) );
          m_bSeries.BezierPoints[3].SetValueXY( 0.75, Math.Pow( 0.75, newY ) );
        }

        // update markers from curve points
        chart1.Series[1].Points[1] = m_bSeries.BezierPoints[1];
        chart1.Series[1].Points[2] = m_bSeries.BezierPoints[2];
        chart1.Series[1].Points[3] = m_bSeries.BezierPoints[3];
        chart1.Series[1].Points[4] = m_bSeries.BezierPoints[4];

        m_bSeries.Invalidate( chart1 );

      }
    }

    private void chartPoint_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      m_hitActive = false;

      // update live values
      m_live.exponentS = lblLiveOutExponent.Text;
      if ( m_live.nonLinCurve != null ) {
        m_live.nonLinCurve.Curve( float.Parse( lblLiveIn1.Text ), float.Parse( lblLiveOut1.Text ),
                              float.Parse( lblLiveIn2.Text ), float.Parse( lblLiveOut2.Text ),
                              float.Parse( lblLiveIn3.Text ), float.Parse( lblLiveOut3.Text ) );
      }
    }
    #endregion


    #region Slider Value Changed (Deadzone / Saturation)

    private void tbSlider_ValueChanged( object sender, EventArgs e )
    {
      EvalSlider( );
    }

    /// <summary>
    /// Update Live from Slider Value
    /// </summary>
    private void EvalSlider()
    {
      lblLiveOutDeadzone.Text = Deviceoptions.DeadzoneFromSlider( tbDeadzone.Value ).ToString( "0.000" );
      if ( cbxUseDeadzone.Enabled && cbxUseDeadzone.Checked ) {
        float curDeadzone = 1000.0f * Deviceoptions.DeadzoneFromSlider( tbDeadzone.Value );  // % scaled to maxAxis
        m_live.deadzone = curDeadzone;
        lblGraphDeadzone.Text = lblLiveOutDeadzone.Text;

      }
      lblLiveOutSaturation.Text = Deviceoptions.SaturationFromSlider( tbSaturation.Value ).ToString( "0.000" );
      if ( cbxUseSaturation.Enabled && cbxUseSaturation.Checked ) {
        float curSaturation = 1000.0f * Deviceoptions.SaturationFromSlider( tbSaturation.Value );  // % scaled to maxAxis
        m_live.saturation = curSaturation;
        lblGraphSaturation.Text = lblLiveOutSaturation.Text;
      }
    }
    #endregion

    #region Checked Invert Changed

    private void cbxInvert_CheckedChanged( object sender, EventArgs e )
    {
      m_live.invertUsed = false;
      if ( cbxLiveInvert.Checked == true ) {
        m_live.invertUsed = true;
      }
    }

    private void cbxLiveInvertForced_CheckedChanged( object sender, EventArgs e )
    {
      m_live.invertForced = false;
      if ( cbxLiveInvertForced.Checked == true ) {
        m_live.invertForced = true;
      }
    }

    #endregion

    #region Checked Deadzone Changed

    private void cbxUseDeadzone_CheckedChanged( object sender, EventArgs e )
    {
      m_live.deadzoneUsed = false;
      if ( cbxUseDeadzone.Checked == true ) {
        tbDeadzone.Enabled = true;
        m_live.deadzoneUsed = true;
      }
      else {
        tbDeadzone.Enabled = false;
        tbDeadzone.Value = tbDeadzone.Minimum;
      }
      EvalChartInput( );
      UpdateGUIFromLiveValues( m_live );
    }

    #endregion

    #region Checked Saturation Changed

    private void cbxUseSaturation_CheckedChanged( object sender, EventArgs e )
    {
      m_live.saturationUsed = false;
      if ( cbxUseSaturation.Checked == true ) {
        tbSaturation.Enabled = true;
        m_live.saturationUsed = true;
      }
      else {
        tbSaturation.Enabled = false;
        tbSaturation.Value = tbSaturation.Maximum;
      }
      EvalChartInput( );
      UpdateGUIFromLiveValues( m_live );
    }

    #endregion



    /// <summary>
    /// Update the last Tuning item and the ListView Item from Live
    /// </summary>
    private void UpdateLiveTuning()
    {
      if ( m_liveTuning != null ) {
        m_live.Update( ref m_liveTuning );
        UpdateLvOptionFromLiveValues( m_live );
        m_liveTuning = null;
      }
    }

    private void UpdateLiveDevOption()
    {
      if ( m_liveDevOption != null ) {
        m_live.Update( ref m_liveDevOption );
        UpdateLvDevOptionFromLiveValues( m_liveDevOption );
        m_liveDevOption = null;
      }
    }

    private void PrepOptionsTab()
    {
      pnlOptionInput.Visible = true;
      if ( lvOptionTree.Items.Count > 0 )
        lvOptionTree.Items[0].Selected = true;
    }


    // get the Live Item updated
    private void lvOptionTree_SelectedIndexChanged( object sender, EventArgs e )
    {
      log.Debug( "lvOptionTree_SelectedIndexChanged - Entry" );
      try {
        // before loading a new one we push the current one back to tuning and the list view
        UpdateLiveTuning( );
        UpdateLiveDevOption( );

        if ( ( sender as ListView ).SelectedItems.Count > 0 ) {
          ListViewItem lvi = ( sender as ListView ).SelectedItems[0];
          // get the associated parameter - either DevOptions for the first part or Tuning for 2nd and on
          m_liveTuning = m_tuningRef.TuningItem( (string)( sender as ListView ).Tag, lvi.Name );
          if ( m_liveTuning == null ) {
            // try devOptions only
            if ( m_devOptRef.ContainsKey( lvi.Name ) ) {
              m_liveDevOption = m_devOptRef[lvi.Name];
              m_live.Load( m_liveDevOption );
            }
            else {
              m_liveDevOption = null;
            }

          }
          else {
            // valid tuning item
            m_liveDevOption = m_liveTuning.DeviceoptionRef; // also connect the DevOptions here 
            m_live.Load( m_liveTuning );
          }
        }
        else {
          // selection got empty - update prev selection
          m_liveDevOption = null;
          m_liveTuning = null;
          m_live.Load( m_liveTuning );
        }

        UpdateGUIFromLiveValues( m_live );
        UpdateChartItems( );
      }
      catch {
        ;
      }
      log.Debug( "lvOptionTree_SelectedIndexChanged - Exit" );
    }


    // handles Selecting AND Deselecting
    private void tabC_Selecting( object sender, TabControlCancelEventArgs e )
    {
      log.Debug( "tabC_Selecting - Entry" );
      if ( e.Action == TabControlAction.Deselecting ) {
        // before leaving the Tab we push the current one back to tuning and the list view
        UpdateLiveTuning( );
        UpdateLiveDevOption( );
        m_liveTuning = null;
        m_live.Reset( );
      }
      else if ( e.Action == TabControlAction.Selecting ) {
        PrepOptionsTab( );
        if ( e.TabPage != null ) {
          TabChangeHandling( );
        }
      }

      e.Cancel = false; // let it change
      log.Debug( "tabC_Selecting - Exit" );
    }

    private void btExit_Click( object sender, EventArgs e )
    {
      // It ai setup as OK button - nothing here so far...
    }

  }
}
