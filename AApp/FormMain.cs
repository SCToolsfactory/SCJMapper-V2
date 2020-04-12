using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SharpDX.DirectInput;
using System.IO;

using SCJMapper_V2.Common;
using SCJMapper_V2.Actions;
using SCJMapper_V2.SC;
using SCJMapper_V2.Table;
using SCJMapper_V2.Devices;
using SCJMapper_V2.Devices.Keyboard;
using SCJMapper_V2.Devices.Mouse;
using SCJMapper_V2.Devices.Gamepad;
using SCJMapper_V2.Devices.Joystick;
using SCJMapper_V2.Devices.Options;
using SCJMapper_V2.Devices.Monitor;
using SCJMapper_V2.Translation;
using System.Threading;
using SCJMapper_V2.Layout;

namespace SCJMapper_V2
{
  public partial class MainForm : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private const string c_GithubLink = @"https://github.com/SCToolsfactory/SCJMapper-V2/releases";

    private bool m_appLoading = true; // used to detect if we are loading (or running)

    // keyboard modifier handling variables
    private string m_persistentMods = "";
    private const int c_modifierTime = 3500; // msec time before a modifier times out and will be removed
    private int m_modifierTimeout = 0;
    private bool m_dumpSCJScommands = false; // allow dumping SCJoyServer Commands into the XML Pane while true


    ///<remarks>
    /// Holds the ActionTree that manages the TreeView and the action lists
    ///</remarks>
    private ActionTree m_AT = null;


    ///<remarks>
    /// Holds the Tuning Form
    ///</remarks>
    private OGL.FormJSCalCurve JSCAL = null;

    ///<remarks>
    /// Holds the Table Form
    ///</remarks>
    private FormTable FTAB = null;

    #region Tools section

    // Means to identify the Gamepad TabPage 
    // (the TAG is used as Int for JS as well - so don't change the ID type used)
    private const int ID_GAMEPAD_TAB = -99;
    /// <summary>
    /// Identify the Tab as Gamepad tab
    /// </summary>
    /// <param name="page">The tab page</param>
    private void SetGamepadTab( TabPage page )
    {
      page.Tag = ID_GAMEPAD_TAB;
    }
    /// <summary>
    /// Returns true if the tabPage is the Gamepad Page
    /// </summary>
    /// <param name="page">The tab page</param>
    /// <returns>True if it is the Gamepad Tab</returns>
    private bool IsGamepadTab( TabPage page )
    {
      // catch if the Tag is not an int...
      try {
        return ( (int)page.Tag == ID_GAMEPAD_TAB );
      }
      catch {
        return false;
      }
    }

    /// <summary>
    /// Detects and returns the current Input device
    /// </summary>
    private Act.ActionDevice InputMode
    {
      get {
        // take care of the sequence.. mouse overrides key but both override joy and game
        if ( m_mouseIn ) {   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
          return Act.ActionDevice.AD_Mouse;
        }
        else if ( m_keyIn ) {
          return Act.ActionDevice.AD_Keyboard;
        }
        else {
          if ( tc1.SelectedTab != null ) {
            if ( IsGamepadTab( tc1.SelectedTab ) ) {
              return Act.ActionDevice.AD_Gamepad;
            }
            else {
              return Act.ActionDevice.AD_Joystick;
            }
          }
          return Act.ActionDevice.AD_Unknown;
        }
      }
    }


    /// <summary>
    /// Get the current JsN string for the active device tab
    /// </summary>
    /// <returns>The jsN string - can be jsx, js1..jsN</returns>
    private string JSStr()
    {
      UC_JoyPanel jp = (UC_JoyPanel)( tc1.SelectedTab?.Controls["UC_JoyPanel"] );
      return jp?.JsName;
    }

    // tab index for the tcXML control
    private enum EATabXML
    {
      Tab_XML = 0,
      Tab_Assignment,
    }

    private void AutoTabXML_Assignment( EATabXML tab )
    {
      if ( AppSettings.Instance.AutoTabXML ) {
        if ( tcXML.SelectedIndex != (int)tab ) {
          tcXML.SelectedTab = tcXML.TabPages[(int)tab];
          if ( tab == EATabXML.Tab_Assignment )
            lblLastJ.Select( ); // select again as when changing the Tabs
        }
      }
    }

    private void UpdateDDMapping( string mapName )
    {
      msSelectMapping.Text = mapName;
      AppSettings.Instance.DefMappingName = mapName; AppSettings.Instance.Save( );
    }

    /// <summary>
    /// Indicates if the SC directory is a valid one
    /// </summary>
    private void SCFileIndication()
    {
      if ( string.IsNullOrEmpty( SCPath.SCClientMappingPath ) ) msSelectMapping.BackColor = MyColors.InvalidColor;
      else msSelectMapping.BackColor = MyColors.MappingColor;
    }


    /// <summary>
    /// Returns true if the JS with index (0...) is hidden in AppSettings
    /// </summary>
    /// <param name="jsIndex">The JS index (0...)</param>
    /// <returns>True if hidden</returns>
    private bool IsTabPageHidden( int jsIndex )
    {
      return ( AppSettings.Instance.JSnHide.Contains( jsIndex.ToString( "D2" ) ) );
    }

    // contains the index into the color map of this particular device
    // JS are 0..n, GP is GPtab index, not used is -1
    private int[] m_tabMap = new int[12] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    // manage TabPage visibility
    private void ShowTabPages()
    {
      // only JS devices can be hidden
      foreach ( var dev in DeviceInst.JoystickListRef ) {
        dev.Hidden = IsTabPageHidden( dev.DevInstance );
      }
      tc1.SuspendLayout( );
      // reload all pages from the dev instance
      tc1.TabPages.Clear( );
      // rebuild tab map for visible JS devices
      m_tabMap = new int[12] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
      int i = 0;
      foreach ( var dev in DeviceInst.JoystickListRef ) {
        if ( !dev.Hidden ) {
          tc1.TabPages.Add( dev.TabPage );
          m_tabMap[i++] = dev.DevInstance;
        }
      }
      if ( DeviceInst.GamepadRef != null ) {
        tc1.TabPages.Add( DeviceInst.GamepadRef.TabPage );
        m_tabMap[i++] = DeviceInst.GamepadRef.MyTabPageIndex;
      }
      // select the first tab if one is available
      if ( tc1.TabPages.Count > 0 )
        tc1.SelectedTab = tc1.TabPages[0];

      tc1.ResumeLayout( );
    }


    /// <summary>
    /// Returns the assigned color of the Joystick from Settings
    /// </summary>
    /// <param name="jsIndex">The JS index (0...)</param>
    /// <returns>An Argb Color</returns>
    private Color JsColorSetting( int jsIndex )
    {
      // read JS Tab Colors
      string[] e = AppSettings.Instance.JSnColor.Split( new char[] { ',' } );
      if ( jsIndex < e.Length ) {
        if ( int.TryParse( e[jsIndex], out int colInt ) ) {
          return Color.FromArgb( colInt );
        }
        else {
          //invalid int... , use default
          return MyColors.TabColor[jsIndex];
        }
      }
      else {
        // no color found, use default
        return MyColors.TabColor[jsIndex];
      }
    }

    /// <summary>
    /// Update the TabPage Colors from Settings (only Joystick colors)
    /// </summary>
    private void UpdateTabPageColors()
    {
      // re-load TabPage colors for each JS device
      foreach ( var dev in DeviceInst.JoystickListRef ) {
        MyColors.TabColor[dev.DevInstance] = JsColorSetting( dev.DevInstance );
        dev.TabPage.BackColor = MyColors.TabColor[dev.DevInstance];
      }
    }

    #endregion

    #region Main Form Handling
    public void splash()
    {
      Application.Run( new AboutBox( ) );
    }

    private Thread SplashT = null;

    public MainForm()
    {

      try {
        // Load the icon from our resources
        var resources = new System.Resources.ResourceManager( this.GetType( ) );
        this.Icon = ( (Icon)( resources.GetObject( "$this.Icon" ) ) );
      }
      catch {
        ; // well...
      }

      // Splash screen
      SplashT = new Thread( new ThreadStart( splash ) );
      SplashT.Start( );

      InitializeComponent( );

    }

    private void MainForm_Deactivate( object sender, EventArgs e )
    {
      timer1.Enabled = false;
      if ( DeviceInst.JoystickListRef != null ) DeviceInst.JoystickListRef.Deactivate( );
      if ( DeviceInst.KeyboardRef != null ) DeviceInst.KeyboardRef.Deactivate( );
    }

    private void MainForm_Activated( object sender, EventArgs e )
    {
      timer1.Enabled = true;
      if ( DeviceInst.JoystickListRef != null ) DeviceInst.JoystickListRef.Activate( );
      if ( DeviceInst.KeyboardRef != null ) DeviceInst.KeyboardRef.Activate( );
    }


    private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
    {
      log.Debug( "MainForm_FormClosing - Entry" );

      // don't record minimized, maximized forms
      if ( this.WindowState == FormWindowState.Normal ) {
        AppSettings.Instance.FormSize = this.Size;
        AppSettings.Instance.FormLocation = this.Location;
      }

      if ( FTAB != null ) {
        AppSettings.Instance.FormTableLocation = FTAB.LastLocation;
        AppSettings.Instance.FormTableSize = FTAB.LastSize;
        AppSettings.Instance.FormTableColumnWidth = FTAB.LastColSize;

        FTAB.Close( );
        FTAB = null;
      }

      AppSettings.Instance.Save( );
    }


    private void LoadMappingDD()
    {
      SCMappings.UpdateMappingNames( );
      msSelectMapping.DropDownItems.Clear( );
      foreach ( string s in SCMappings.MappingNames ) {
        if ( SCMappings.IsExportedMapping( s ) ) {
          msSelectMapping.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ), IL2.Images["Exported"] );
        }
        else if ( !SCMappings.IsUserMapping( s ) ) {
          msSelectMapping.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ), IL2.Images["RSI"] );
        }
        else {
          msSelectMapping.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ), IL2.Images["User"] );
        }
      }
    }

    /// <summary>
    ///  Handle the load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_Load( object sender, System.EventArgs e )
    {
      log.Debug( "MainForm_Load - Entry" );

      // 20190711 - this needs to be done before updating the Pack...
      // init PTU folder usage at the very start
      if ( AppSettings.Instance.UsePTU ) log.Debug( "Using PTU Folders" );
      lblPTU.Visible = AppSettings.Instance.UsePTU;
      TheUser.UsesPTU = AppSettings.Instance.UsePTU;

      SCFiles.Instance.UpdatePack( ); // update game files

      Tx.LocalizeControlTree( this );
      Tx.LocalizeControlTree( cmCopyPaste );
      Tx.LocalizeControlTree( cmAddDel );
      msBtLoad.ToolTipText = Tx.Translate( msBtLoad.Name + "_TT" );
      msBtDump.ToolTipText = Tx.Translate( msBtDump.Name + "_TT" );
      msBtShow.ToolTipText = Tx.Translate( msBtShow.Name + "_TT" );
      msBtConfig.ToolTipText = Tx.Translate( msBtConfig.Name + "_TT" );
      msBtLoadMap.ToolTipText = Tx.Translate( msBtLoadMap.Name + "_TT" );

      // some applic initialization 
      // Assign Size property - check if on screen, else use defaults
      if ( Commons.IsOnScreen( new Rectangle( AppSettings.Instance.FormLocation, AppSettings.Instance.FormSize ) ) ) {
        this.Size = AppSettings.Instance.FormSize;
        this.Location = AppSettings.Instance.FormLocation;
      }

      string version = Application.ProductVersion;  // get the version information
      // BETA VERSION; TODO -  comment out if not longer
      //lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // PRODUCTION
      lblTitle.Text += " - V " + version + " beta"; // BETA

      log.InfoFormat( "Application Version: {0}", version.ToString( ) );

      // tooltips where needed
      toolTip1.SetToolTip( this.linkLblReleases, c_GithubLink ); // allow to see where the link may head

      tsLblSupport.Text = "profile version = \"1\" optionsVersion = \"2\" rebindVersion = \"2\"";

      // XML RTB
      log.Debug( "Loading RTB" );
      rtb.SelectionTabs = new int[] { 10, 20, 30, 40, 50, 60 }; // short tabs
      rtb.DragEnter += new DragEventHandler( rtb_DragEnter );
      rtb.DragDrop += new DragEventHandler( rtb_DragDrop );
      rtb.AllowDrop = true; // add Drop to rtb

      // load languages
      SCUiText.Instance.Language = SCUiText.Languages.profile;
      if ( Enum.TryParse( AppSettings.Instance.UseLanguage, out SCUiText.Languages lang ) ) {
        SCUiText.Instance.Language = lang;
      }
      treeView1.ShowNodeToolTips = AppSettings.Instance.ShowTreeTips;

      // load mappings
      log.Debug( "Loading Mappings" );
      LoadMappingDD( );
      msSelectMapping.Text = AppSettings.Instance.DefMappingName;

      SCFileIndication( );

      // load TabPage colors 
      for ( int i = 0; i < MyColors.TabColor.Length; i++ ) {
        MyColors.TabColor[i] = JsColorSetting( i );
      }

      // load other defaults
      log.Debug( "Loading Other" );
      txMappingName.Text = AppSettings.Instance.MyMappingName;
      SetRebindField( txMappingName.Text );
      foreach ( ToolStripDropDownItem d in msSelectMapping.DropDownItems ) {
        if ( d.Text == txMappingName.Text ) {
          UpdateDDMapping( txMappingName.Text );
          break;
        }
      }

      // Init X things
      log.Debug( "Loading DirectX" );
      if ( !InitDirectInput( ) ) {
        log.Fatal( "Initializing DirectXInput failed" );
        MessageBox.Show( "Initializing DirectXInput failed - program exits now", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information );
        Close( );
      }

      log.Debug( "Loading last used mapping" );
      if ( SCMappings.MappingFileExists( txMappingName.Text ) ) {
        rtb.LoadFile( SCMappings.MappingFileName( txMappingName.Text ), RichTextBoxStreamType.PlainText );
        InitActionTree( false );
        Grab( );
        AppSettings.Instance.MyMappingName = txMappingName.Text; AppSettings.Instance.Save( );// last used - persist
        txMappingName.BackColor = MyColors.SuccessColor;
      }
      else {
        log.WarnFormat( "Last used mapping not available ({0})", txMappingName.Text );
        txMappingName.BackColor = MyColors.ErrorColor;
      }

      // load Mouse menu strip
      if ( DeviceInst.MouseRef != null ) {
        for ( int i = 0; i < DeviceInst.MouseRef.NumberOfButtons; i++ ) {
          var ts = new ToolStripMenuItem( "Button " + ( i + 1 ).ToString( ), null, new EventHandler( tmeItem_Click ) );
          ts.Tag = ( i + 1 ).ToString( );
          cmMouseEntry.Items.Add( ts );
        }
      }

      // load show checkboxes
      cbxShowJoystick.Checked = AppSettings.Instance.ShowJoystick;
      cbxShowGamepad.Checked = AppSettings.Instance.ShowGamepad;
      cbxShowKeyboard.Checked = AppSettings.Instance.ShowKeyboard;
      cbxShowMouse.Checked = AppSettings.Instance.ShowMouse;
      cbxShowMappedOnly.Checked = AppSettings.Instance.ShowMapped;

      // now update the contents according to new settings
      foreach ( JoystickCls j in DeviceInst.JoystickListRef ) j.ApplySettings( );

      // init current Joystick
      int jsIndex = -1;
      if ( tc1.SelectedTab != null )
        jsIndex = (int)tc1.SelectedTab.Tag; // gets the index into the JS list
      if ( jsIndex >= 0 ) DeviceInst.JoystickInst = DeviceInst.JoystickListRef[jsIndex];

      // Auto Tab XML
      cbxAutoTabXML.Checked = AppSettings.Instance.AutoTabXML;

      // poll the XInput
      log.Debug( "Start XInput polling" );
      timer1_Tick( null, null );

      timer1.Start( ); // this one polls the joysticks to show the props

      // Select XML tab to start with 
      AutoTabXML_Assignment( EATabXML.Tab_XML );

      m_appLoading = false; // no longer

      SplashT.Abort( );
    }


    /// <summary>
    /// Handles the Exit button
    /// </summary>
    private void buttonExit_Click( object sender, EventArgs e )
    {
      log.Debug( "Shutting down now..." );
      Close( );
    }


    // TAB Control Events

    private void tc1_Selected( object sender, TabControlEventArgs e )
    {
      if ( tc1.SelectedTab == null ) {
        DeviceInst.JoystickInst = null;
      }
      else {
        // init current Joystick
        int jsIndex = (int)tc1.SelectedTab.Tag; // gets the index into the JS list
        if ( jsIndex >= 0 )
          DeviceInst.JoystickInst = DeviceInst.JoystickListRef[jsIndex];
        else
          DeviceInst.JoystickInst = null;
      }
    }

    /// <summary>
    /// Fancy tab coloring with ownerdraw to paint the callout buttons
    /// </summary>
    private void tc1_DrawItem( object sender, DrawItemEventArgs e )
    {
      try {
        // get the BG color from the current TabColor Map.
        // as some devices can be hidden, use m_tabMap to find the 'real' index rather than using the tab index (this tabMap is updated on changes in Settings)
        // GP should be always the last JS +1 
        // -1 indicates - not used

        if ( m_tabMap[e.Index] < 0 )
          return; // not used tab - should not happen..

        Font f;
        Brush backBrush = new SolidBrush( MyColors.TabColor[m_tabMap[e.Index]] );
        Brush foreBrush = new SolidBrush( Color.Black );


        //The draw call sends all tabs to draw, the selected one needs to be with Bold font
        if ( e.Index == this.tc1.SelectedIndex ) {
          f = new Font( e.Font, FontStyle.Bold );
          /*
          Rectangle tabRect = tc1.Bounds;
          Region tabRegion = new Region( tabRect );
          Rectangle TabItemRect = new Rectangle( 0, 0, 0, 0 );
          for ( int nTanIndex = 0; nTanIndex < tc1.TabCount; nTanIndex++ ) {
            TabItemRect = Rectangle.Union( TabItemRect, tc1.GetTabRect( nTanIndex ) );
          }
          tabRegion.Exclude( TabItemRect );
          //e.Graphics.FillRegion( backBrush, tabRegion );
          */
        }
        else {
          f = e.Font;
        }

        //To set the alignment of the caption.
        string tabName = this.tc1.TabPages[e.Index].Text;
        StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };

        //This will help you to fill the interior portion of selected tabpage.
        e.Graphics.FillRectangle( backBrush, e.Bounds );
        Rectangle r = e.Bounds;
        r = new Rectangle( r.X, r.Y + 3, r.Width, r.Height - 3 );
        e.Graphics.DrawString( tabName, f, foreBrush, r, sf );

        sf.Dispose( );
        if ( e.Index == this.tc1.SelectedIndex ) {
          f.Dispose( ); // we created this one
        }
        backBrush.Dispose( );
        foreBrush.Dispose( );
      }
      catch ( Exception Ex ) {
        log.Error( "Ex DrawItem", Ex );
        MessageBox.Show( Ex.Message.ToString( ), "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Information );
      }

    }

    #endregion

    #region Initializations

    /// <summary>
    /// Resets the Action Tree
    /// </summary>
    private void InitActionTree( bool addDefaultBinding )
    {
      log.Debug( "InitActionTree - Entry" );

      // build TreeView and the ActionMaps
      if ( m_AT != null ) {
        m_AT.NodeSelectedEvent -= M_AT_NodeSelectedEvent; // disconnect the Event
        m_AT.Dispose( );
      }

      m_AT = new ActionTree( );
      log.DebugFormat( "InitActionTree - New AT: {0}", m_AT.GetHashCode( ).ToString( ) );

      m_AT.NodeSelectedEvent += M_AT_NodeSelectedEvent; // connect the Event

      m_AT.Ctrl = treeView1;  // the ActionTree owns the TreeView control
      m_AT.IgnoreMaps = AppSettings.Instance.IgnoreActionmaps;
      // provide the display items (init)
      m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
      // Init with default profile filepath
      m_AT.LoadProfileTree( addDefaultBinding );
      tslblProfileUsed.Text = SCDefaultProfile.UsedDefProfile; // SCA 2.2 show used profile

      // Activation Update
      tdiCbxActivation.Items.Clear( );
      tdiCbxActivation.Items.AddRange( ActivationModes.Instance.Names.ToArray( ) );
      tdiCbxActivation.SelectedIndex = 0;

      // apply a default JS to Joystick mapping - can be changed and reloaded from XML mappings
      // must take care of Gamepads if there are (but we take care of one only...)
      int joyStickIndex = 0; // Joystick List Index
      for ( int deviceTabIndex = 0; deviceTabIndex < JoystickCls.JSnum_MAX; deviceTabIndex++ ) {
        if ( tc1.TabPages.Count > deviceTabIndex ) {
          // valid Device Tab
          if ( IsGamepadTab( tc1.TabPages[deviceTabIndex] ) ) {
            ; // ignore gamepads
          }
          else if ( DeviceInst.JoystickListRef.Count > joyStickIndex ) {
            // there is a joystick device left..
            DeviceInst.JoystickListRef[joyStickIndex].JSAssignment = joyStickIndex + 1; // assign number 1..
            m_AT.ActionMaps.jsN[deviceTabIndex] = DeviceInst.JoystickListRef[joyStickIndex].DevName;
            m_AT.ActionMaps.jsN_instGUID[deviceTabIndex] = DeviceInst.JoystickListRef[joyStickIndex].DevInstanceGUID;
            m_AT.ActionMaps.jsN_prodGUID[deviceTabIndex] = DeviceInst.JoystickListRef[joyStickIndex].DevGUID;
            joyStickIndex++;
          }
        }
      }
      m_AT.FilterTree( txFilter.Text );
    }

    // Helper: collect the joysticks here
    struct myDxJoystick
    {
      public Joystick js;
      public string prodName;
    }

    /// <summary>
    /// Aquire the DInput joystick devices
    /// </summary>
    /// <returns></returns>
    public bool InitDirectInput()
    {
      log.Debug( "InitDirectInput - Entry" );

      // Enumerate gamepads in the system.
      SharpDX.XInput.UserIndex gpDeviceIndex = SharpDX.XInput.UserIndex.Any;

      // Initialize DirectInput
      log.Debug( "  - Instantiate DirectInput" );
      var directInput = new DirectInput( );

      try {
        log.Debug( "  - Get Keyboard device" );
        DeviceInst.KeyboardInst = new KeyboardCls( new Keyboard( directInput ), this.Handle );

        log.Debug( "  - Get Mouse device" );
        DeviceInst.MouseInst = new MouseCls( new Mouse( directInput ), this.Handle );

      }
      catch ( Exception ex ) {
        log.Debug( "  *** InitDirectInput phase 1 failed unexpectedly", ex );
        return false;
      }

      // init devices
      List<myDxJoystick> dxJoysticks = new List<myDxJoystick>( );
      SharpDX.XInput.Controller dxGamepad = null;

      // load from DirectX
      try {
        // scan the Input for attached devices
        log.Debug( "  - Scan GameControl devices" );
        foreach ( DeviceInstance instance in directInput.GetDevices( DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly ) ) {
          log.InfoFormat( "  - GameControl: Type:{0} Device:{1}", instance.Type.ToString( ), instance.ProductName );
          // Create the device interface
          log.Debug( "  - Create the device interface" );
          if ( AppSettings.Instance.DetectGamepad && ( instance.Usage == SharpDX.Multimedia.UsageId.GenericGamepad ) ) {
            // detect Gamepad only if the user wishes to do so
            for ( SharpDX.XInput.UserIndex i = SharpDX.XInput.UserIndex.One; i < SharpDX.XInput.UserIndex.Four; i++ ) {
              dxGamepad = new SharpDX.XInput.Controller( i );
              if ( dxGamepad.IsConnected ) {
                log.InfoFormat( "  - Scan Input {0} for gamepad - {1}", i, dxGamepad.GetCapabilities( SharpDX.XInput.DeviceQueryType.Gamepad ).ToString( ) );
                gpDeviceIndex = i;
                break; // get only the first one
              }
            }
          }
          else {
            myDxJoystick myJs = new myDxJoystick { js = new Joystick( directInput, instance.InstanceGuid ), prodName = instance.ProductName };
            dxJoysticks.Add( myJs );
            log.DebugFormat( "  - Create the device interface for: {0}", myJs.prodName );
          }
        }
      }
      catch ( Exception ex ) {
        log.Debug( "  *** InitDirectInput phase 2 failed unexpectedly", ex );
        return false;
      }

      // Create the TabPages
      int tabs = 0;

      // do all joysticks
      int nJs = 0; // number the Joystick Tabs
      foreach ( myDxJoystick myJs in dxJoysticks ) {
        // we have the first tab made as reference so TabPage[0] already exists
        JoystickCls js = null;
        UC_JoyPanel uUC_JoyPanelNew = null;
        if ( tabs == 0 ) {
          // first panel - The Tab content exists already 
          log.Debug( "  - Add first Joystick panel" );
          uUC_JoyPanelNew = UC_JoyPanel;
        }
        else {
          log.Debug( "  - Add next Joystick panel" );
          // setup the further tab contents along the reference one in TabPage[0] (the control named UC_JoyPanel)
          tc1.TabPages.Add( "" );
          uUC_JoyPanelNew = new UC_JoyPanel( ); tc1.TabPages[tabs].Controls.Add( uUC_JoyPanelNew );
          Tx.LocalizeControlTree( uUC_JoyPanelNew );
          uUC_JoyPanelNew.Size = UC_JoyPanel.Size; uUC_JoyPanelNew.Location = UC_JoyPanel.Location;
        }
        // common part
        log.Debug( "  - Create Joystick instance " + nJs.ToString( ) );
        // does all device related activities for that particular item
        js = new JoystickCls( myJs.js, this, nJs, uUC_JoyPanelNew, tabs ) { TabPage = tc1.TabPages[tabs] };
        DeviceInst.JoystickListRef.Add( js ); // add to joystick list
        js.TabPage.Text = string.Format( "{0} {1}", Tx.Translate( "xJoystick" ), nJs + 1 ); // numbering is 1 based for the user
        js.TabPage.ToolTipText = string.Format( "{0}\n{1}", js.DevName, js.DevInstanceGUID );
        toolTip1.SetToolTip( js.TabPage, js.TabPage.ToolTipText );
        js.TabPage.Tag = js.DevInstance;  //  used to find the tab for polling
        js.TabPage.BackColor = MyColors.TabColor[tabs];
        js.Hidden = IsTabPageHidden( js.DevInstance );

        nJs++; // next joystick
        // next Joystick tab
        tabs++;
        if ( tabs >= JoystickCls.JSnum_MAX ) {
          log.Debug( "  - Number of Device tabs reached MAX, cannot add more devices" );
          break; // cannot load more JSticks than predefined Tabs
        }
      }

      // make the GP the LAST device if there is one.
      if ( ( tabs < JoystickCls.JSnum_MAX ) && ( dxGamepad != null ) ) {
        log.Debug( "  - Add Gamepad panel" );
        if ( tabs > 0 ) {
          tc1.TabPages.Add( "" );
          log.Debug( "  - Add Gamepad as next panel" );
        }
        tc1.TabPages[tabs].Text = Tx.Translate( "xGamepad" ) + " ";
        UC_GpadPanel uUC_GpadPanelNew = new UC_GpadPanel( ); tc1.TabPages[tabs].Controls.Add( uUC_GpadPanelNew );
        Tx.LocalizeControlTree( uUC_GpadPanelNew );

        uUC_GpadPanelNew.Size = UC_JoyPanel.Size; uUC_GpadPanelNew.Location = UC_JoyPanel.Location;
        if ( tabs == 0 ) {
          UC_JoyPanel.Enabled = false; UC_JoyPanel.Visible = false; // don't use this one 
        }
        log.Debug( "  - Create Gamepad instance" );
        DeviceInst.GamepadInst = new GamepadCls( dxGamepad, uUC_GpadPanelNew, tabs ); // does all device related activities for that particular item
        DeviceInst.GamepadRef.SetDeviceName( GamepadCls.DevNameCIG ); // this is fixed ...
        DeviceInst.GamepadRef.TabPage = tc1.TabPages[tabs];

        DeviceInst.GamepadRef.TabPage.ToolTipText = string.Format( "{0}\n{1}", DeviceInst.GamepadRef.DevName, " " );
        toolTip1.SetToolTip( DeviceInst.GamepadRef.TabPage, DeviceInst.GamepadRef.TabPage.ToolTipText );

        SetGamepadTab( DeviceInst.GamepadRef.TabPage );  // indicates the gamepad tab (murks..)
        MyColors.TabColor[tabs] = MyColors.GamepadColor; // save it for future use of tab coloring (drawing)
        DeviceInst.GamepadRef.TabPage.BackColor = MyColors.TabColor[tabs];
        DeviceInst.GamepadRef.Hidden = false; // always visible

        tabs++; // next tab
      }
      log.DebugFormat( "  - Added {0} GameControl devices", tabs );

      if ( tabs == 0 ) {
        log.Warn( "  - Unable to find and/or create any joystick devices." );
        MessageBox.Show( "Unable to create a joystick device. Program will exit.", "No joystick found", MessageBoxButtons.OK, MessageBoxIcon.Information );
        return false;
      }

      // manage visibility of Tabs
      ShowTabPages( );

      // load the profile items from the XML
      log.Debug( "  - End of, InitActionTree now" );
      InitActionTree( true );

      return true;
    }

    #endregion

    #region Tree Handling

    /// <summary>
    ///  Grab the rtb data and load them into config
    /// </summary>
    private void Grab()
    {
      log.Debug( "Grab - Entry" );
      m_dumpSCJScommands = false; // disable this one

      m_AT.ActionMaps.fromXML( rtb.Text );

      // Collect modifiers - simply overwrite existing ones as we deal with THIS file now
      tdiAddMod1.Visible = false; tdiAddMod2.Visible = false; tdiAddMod3.Visible = false; // make context menu invisible
      tdiAddMod1.Text = ""; tdiAddMod2.Text = ""; tdiAddMod3.Text = ""; // and clear
      /*
      if ( m_AT.ActionMaps.Modifiers.Count > 2 ) {
        tdiAddMod3.Text = string.Format( "MOD: {0}", m_AT.ActionMaps.Modifiers[2] ); tdiAddMod3.Visible = true;
        // make a new one
        CheckBox cbx = new CheckBox(); cbx.Text = m_AT.ActionMaps.Modifiers[2]; cbx.Checked = true;
        cbx.CheckedChanged += Cbx_CheckedChanged;
        flpExtensions.Controls.Add( cbx );
      }
      if ( m_AT.ActionMaps.Modifiers.Count > 1 ) {
        tdiAddMod2.Text = string.Format( "MOD: {0}", m_AT.ActionMaps.Modifiers[1] ); tdiAddMod2.Visible = true;
        // make a new one
        CheckBox cbx = new CheckBox(); cbx.Text = m_AT.ActionMaps.Modifiers[1]; cbx.Checked = true;
        cbx.CheckedChanged += Cbx_CheckedChanged;
        flpExtensions.Controls.Add( cbx );
      }
      if ( m_AT.ActionMaps.Modifiers.Count > 0 ) {
        tdiAddMod1.Text = string.Format( "MOD: {0}", m_AT.ActionMaps.Modifiers[0] ); tdiAddMod1.Visible = true;
        // make a new one
        CheckBox cbx = new CheckBox(); cbx.Text = m_AT.ActionMaps.Modifiers[0]; cbx.Checked = true;
        cbx.CheckedChanged += Cbx_CheckedChanged;
        flpExtensions.Controls.Add( cbx );
      }
      */

      m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
      m_AT.ReloadTreeView( ); // finally reload things into the tree

      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again

      // get the text into the view
      try {
        rtb.ScrollToCaret( );
      }
      catch {
        ; // just ignore
      }
      UpdateTable( );
      UpdateAssignmentList( );
    }


    /// <summary>
    /// Dump Config into rtb
    /// </summary>
    private void Dump()
    {
      log.Debug( "Dump - Entry" );
      m_dumpSCJScommands = false; // disable this one

      AutoTabXML_Assignment( EATabXML.Tab_XML );

      rtb.Text = string.Format( "<!-- {0} - SC Joystick Mapping - {1} -->\n{2}", DateTime.Now, txMappingName.Text, m_AT.toXML( txMappingName.Text ) );

      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
    }


    private void SetRebindField( string map )
    {
      txRebind.Text = "pp_rebindkeys " + map;
    }

    #endregion

    #region Event Handling

    // *** Timer Events

    // polls the devices to get the latest update
    private void timer1_Tick( object sender, EventArgs e )
    {
      // Handle Kbd modifier timeout for joystick
      m_modifierTimeout -= timer1.Interval;  // decrement timeout
      if ( m_modifierTimeout < 0 ) m_modifierTimeout = 0; // prevent undeflow after long time not using modifiers

      if ( m_keyIn || tc1.SelectedTab?.Tag == null ) return; // don't handle those

      string ctrl = "";
      if ( DeviceInst.JoystickRef == null ) {
        // no active joystick - may be a gamepad
        if ( DeviceInst.GamepadRef != null ) {
          // poll Gamepad if active
          DeviceInst.GamepadRef.GetData( );
          ctrl = DeviceInst.GamepadRef.GetLastChange( );
          timer1.Interval = 750; // allow more time to release buttons [msec]
        }
      }
      else {
        // poll active Joystick
        DeviceInst.JoystickRef.GetData( );  // poll the device
        // add keyboard modifier - if there are ..
        if ( DeviceInst.KeyboardRef == null ) {
          // no keyboard => no modifier 
          ctrl = JSStr( ) + DeviceInst.JoystickRef.GetLastChange( ); // show last handled JS control
        }
        else {
          UpdateModifiers( );   // get the last keyboard modifer to compose the command, also handles the modifier lifetime
          ctrl = JSStr( ) + m_persistentMods + DeviceInst.JoystickRef.GetLastChange( ); // show last handled JS control
        }
        timer1.Interval = 150; // standard polling [msec]
      }

      lblLastJ.Text = ctrl;

      // Handle Throttle checkbox
      if ( JoystickCls.CanThrottle( ctrl ) ) {
        cbxThrottle.Enabled = true;
      }
      else {
        cbxThrottle.Checked = false; cbxThrottle.Enabled = false;
      }
      // Update joystick modifiers - not currently used
      //btMakeMod.Enabled = JoystickCls.ValidModifier( ctrl );

    }


    // *** TreeView Events

    private void treeView1_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
    {
      if ( e.Button == MouseButtons.Right ) {
        treeView1.SelectedNode = e.Node; // trigger ActionTree events..
      }
    }

    private void treeView1_NodeMouseDoubleClick( object sender, TreeNodeMouseClickEventArgs e )
    {
      if ( !m_dumpSCJScommands ) return; // disabled

      if ( e.Button == MouseButtons.Left ) {
        if ( e.Node.Level > 0 ) {
          string cmd = SCJServer.SCJScmd.GetCommand( e.Node );
          if ( !string.IsNullOrEmpty( cmd ) ) {
            rtb.Text += $"{cmd}\n";
          }
        }
      }
    }

    // Action Tree Event - manages the de/selection of a node
    private void M_AT_NodeSelectedEvent( object sender, ActionTreeEventArgs e )
    {
      lblAction.Text = SCUiText.Instance.Text( e.SelectedAction );
      lblAssigned.Text = e.SelectedCtrl;
    }


    // *** Show options

    private void cbxShowTreeOptions_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_AT == null ) return; // on init
      m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
      m_AT.ReloadTreeView( );

      if ( m_appLoading ) return; // don't assign while loading defaults
      AppSettings.Instance.ShowJoystick = cbxShowJoystick.Checked; AppSettings.Instance.ShowGamepad = cbxShowGamepad.Checked;
      AppSettings.Instance.ShowKeyboard = cbxShowKeyboard.Checked; AppSettings.Instance.ShowMouse = cbxShowMouse.Checked;
      AppSettings.Instance.ShowMapped = cbxShowMappedOnly.Checked;
    }



    // *** Assign Panel Items

    private void btFind_Click( object sender, EventArgs e )
    {

      m_AT.FindAndSelectCtrl( JoystickCls.MakeThrottle( Act.DevInput( lblLastJ.Text, InputMode ), cbxThrottle.Checked ), "" ); // find the action for a Control (joystick input)
    }

    private void btAssign_Click( object sender, EventArgs e )
    {
      log.Debug( "btAssign_Click" );
      if ( m_AT.UpdateSelectedItem( JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked ), InputMode, true ) ) {
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
        UpdateTableSelectedItem( );
      }
      else MySounds.PlayNotfound( );
    }

    private void btBlend_Click( object sender, EventArgs e )
    {
      log.Debug( "btBlend_Click" );
      if ( m_AT.CanDisableBinding ) {
        m_AT.DisableBinding( );
        UpdateTableSelectedItem( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }
      else MySounds.PlayCannot( );
    }

    private void btClear_Click( object sender, EventArgs e )
    {
      log.Debug( "btClear_Click" );
      if ( m_AT.CanClearBinding || m_AT.CanDisableBinding ) {
        m_AT.ClearBinding( );
        UpdateTableSelectedItem( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }
      else MySounds.PlayCannot( );
    }

    // *** General Area Items

    private void btDump_Click( object sender, EventArgs e )
    {
      Dump( );
    }

    private void btGrab_Click( object sender, EventArgs e )
    {
      Grab( );
    }

    private void btClearFilter_Click( object sender, EventArgs e )
    {
      txFilter.Text = "";
    }

    private void txFilter_TextChanged( object sender, EventArgs e )
    {
      m_AT.FilterTree( txFilter.Text );
    }

    private void cbxAutoTabXML_CheckedChanged( object sender, EventArgs e )
    {
      AppSettings.Instance.AutoTabXML = cbxAutoTabXML.Checked; AppSettings.Instance.Save( );
    }

    // *** Toolstrip Items

    private void meResetDefaults_Click( object sender, EventArgs e )
    {
      m_dumpSCJScommands = false; // disable this one

      // start over and if chosen, load defaults from SC game
      InitActionTree( true );
      rtb.Text = "";
      UpdateTable( );
      UpdateAssignmentList( );
    }

    private void meResetEmpty_Click( object sender, EventArgs e )
    {
      m_dumpSCJScommands = false; // disable this one

      // start over 
      cbxShowMappedOnly.Checked = false; // else it might get empty..
      InitActionTree( false );
      rtb.Text = "";
      UpdateTable( );
      UpdateAssignmentList( );
    }


    private void meDumpMappingList_Click( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_XML );

      if ( AppSettings.Instance.UseCSVListing )
        rtb.Text = string.Format( "-- {0} - SC Joystick Mapping --\n{1}", DateTime.Now, m_AT.ReportActionsCSV( AppSettings.Instance.ListModifiers ) );
      else
        rtb.Text = string.Format( "-- {0} - SC Joystick Mapping --\n{1}", DateTime.Now, m_AT.ReportActions( ) );
    }

    private void meDumpLogfile_Click( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_XML );
      rtb.Text = $"-- {DateTime.Now} - SC Joystick AC Path and Logfile --\n";
      var devList = new DeviceList( );
      rtb.Text += $"\n{devList.DumpDevices( )}\n{SCPath.Summary( )}\n{SCLogExtract.ExtractLog( )}";
      devList = null;
    }

    private void meDumpDefaultProfile_Click( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_XML );
      rtb.Text = SCDefaultProfile.DefaultProfile( );
    }

    private void meDumpActiontreeAsXML_Click( object sender, EventArgs e )
    {
      Dump( );
    }

    // *** Dialogs

    // Show the Table Window
    private void meShowToggleTable_Click( object sender, EventArgs e )
    {
      bool created = false;
      if ( FTAB == null ) {
        FTAB = new FormTable( );
        FTAB.EditActionEvent += FTAB_EditActionEvent;
        FTAB.UpdateEditEvent += FTAB_UpdateEditEvent;
        created = true;
      }

      if ( FTAB.Visible ) {
        AppSettings.Instance.FormTableSize = FTAB.LastSize;
        AppSettings.Instance.FormTableLocation = FTAB.LastLocation;
        AppSettings.Instance.FormTableColumnWidth = FTAB.LastColSize;
        FTAB.Hide( );
      }
      else {
        FTAB.Show( );
        if ( created ) {
          FTAB.LastColSize = AppSettings.Instance.FormTableColumnWidth;
        }
        // reload the data to display
        UpdateTable( );
      }
    }

    private void meShowOptionsDialog_Click( object sender, EventArgs e )
    {
      timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy

      FormOptions OPT = new FormOptions( );

      // Have to attach here to capture the currently valid settings
      // cleanup - Actions will be assigned new in below calls
      m_AT.ActionMaps.DeviceOptions.ResetDynamicItems( );
      m_AT.ActionMaps.TuningOptions.ResetDynamicItems( );

      UpdateAllTuningItems( JoystickCls.DeviceClass );
      UpdateAllTuningItems( GamepadCls.DeviceClass );
      UpdateAllTuningItems( MouseCls.DeviceClass );

      DeviceList devlist = new DeviceList( );
      if ( AppSettings.Instance.DetectGamepad && ( DeviceInst.GamepadRef != null ) ) {
        devlist.Add( DeviceInst.GamepadRef );
      }
      devlist.AddRange( DeviceInst.JoystickListRef );
      devlist.Add( DeviceInst.MouseRef );

      OPT.TuningOptions = m_AT.ActionMaps.TuningOptions;
      OPT.DeviceOptions = m_AT.ActionMaps.DeviceOptions;
      OPT.Devicelist = devlist;

      OPT.ShowDialog( this );
      m_AT.Dirty = true;

      OPT = null; // get rid and create a new one next time..
      devlist = null;

      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      timer1.Enabled = true;
    }

    private void meShowDeviceTuningDialog_Click( object sender, EventArgs e )
    {
      timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy

      JSCAL = new OGL.FormJSCalCurve( );

      // Have to attach here to capture the currently valid settings
      // cleanup - Actions will be assigned new in below calls
      m_AT.ActionMaps.DeviceOptions.ResetDynamicItems( );
      m_AT.ActionMaps.TuningOptions.ResetDynamicItems( );

      UpdateTuningItems( );

      // run
      JSCAL.TuningOptions = m_AT.ActionMaps.TuningOptions;
      JSCAL.ShowDialog( this );
      m_AT.Dirty = true;

      JSCAL = null; // get rid and create a new one next time..

      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      timer1.Enabled = true;
    }

    private void meShowDeviceMonitoringDialog_Click( object sender, EventArgs e )
    {
      timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy
      var MONITOR = new FormDeviceMonitor { ActionTree = m_AT };
      MONITOR.ShowDialog( this );
      MONITOR = null; // get rid and create a new one next time..
      timer1.Enabled = true;
    }

    private void meShowLayoutDialog_Click( object sender, EventArgs e )
    {
      // sanity check for the layout folder
      if ( !Directory.Exists( TheUser.LayoutsDir ) ) {
        MessageBox.Show( this, $"Layout folder is missing - should be: {TheUser.LayoutsDir}" );
      }
      else {
        timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy
        var LAYOUT = new FormLayout { ActionList = m_AT.ReportActionsSItemText( ) };
        LAYOUT.ShowDialog( this );
        LAYOUT = null; // get rid and create a new one next time..
        timer1.Enabled = true;
      }
    }

    private void meDumpSCJoyServerCommands_Click( object sender, EventArgs e )
    {
      // clear the XML pane and allow double clicks to create commands from the action
      rtb.Clear( );
      m_dumpSCJScommands = true; // enable this one
      rtb.Text = "Doubleclick items in the Action Tree to create commands\n\n";

    }

    // *** Settings

    private void meSettingsDialog_Click( object sender, EventArgs e )
    {
      // have to stop polling while the Settings window is open
      timer1.Enabled = false;
      if ( AppSettings.Instance.ShowSettings( "" ) != DialogResult.Cancel ) {
        AppSettings.Instance.Reload( ); // must reload in case of any changes in the form

        // Hide JS Tabs as needed
        ShowTabPages( );
        // update JS colors
        UpdateTabPageColors( );

        // then reload the profile and mappings
        LoadMappingDD( );
        // indicates (in)valid folders
        SCFileIndication( );
        // change language if needed
        if ( Enum.TryParse( AppSettings.Instance.UseLanguage, out SCUiText.Languages lang ) ) {
          SCUiText.Instance.Language = lang;
        }
        treeView1.ShowNodeToolTips = AppSettings.Instance.ShowTreeTips;

        // now update the contents according to new settings
        foreach ( JoystickCls j in DeviceInst.JoystickListRef ) j.ApplySettings( ); // update Seetings
        m_AT.IgnoreMaps = AppSettings.Instance.IgnoreActionmaps;
        // and start over with an empty tree
        InitActionTree( false );
        UpdateTable( );
      }
      timer1.Enabled = true;
    }

    private void meJsReassignDialog_Click( object sender, EventArgs e )
    {
      // have to stop polling while the Reassign window is open
      timer1.Enabled = false;
      if ( DeviceInst.JoystickListRef.ShowReassign( ) != DialogResult.Cancel ) {
        // copy the action tree while reassigning the jsN mappings from OLD to NEW
        ActionTree newTree = m_AT.ReassignJsN( DeviceInst.JoystickListRef.JsReassingList );

        // we have still the old assignment in the ActionMap - change it here (map does not know about the devices)
        JoystickCls j = null;
        // for all supported jsN devices
        for ( int i = 0; i < JoystickCls.JSnum_MAX; i++ ) {
          j = DeviceInst.JoystickListRef.Find_InstanceForjsN( i + 1 );
          if ( j != null ) {
            newTree.ActionMaps.jsN[i] = j.DevName;
            newTree.ActionMaps.jsN_instGUID[i] = j.DevInstanceGUID;
            newTree.ActionMaps.jsN_prodGUID[i] = j.DevGUID;
          }
          else {
            newTree.ActionMaps.jsN[i] = ""; newTree.ActionMaps.jsN_instGUID[i] = ""; newTree.ActionMaps.jsN_prodGUID[i] = "";
          }
        }

        m_AT.NodeSelectedEvent -= M_AT_NodeSelectedEvent; // disconnect the Event
        m_AT = newTree; // make it the valid one
        m_AT.NodeSelectedEvent += M_AT_NodeSelectedEvent; // reconnect the Event

        m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
        m_AT.ReloadTreeView( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }

      timer1.Enabled = true;
    }

    // *** Load maps

    private void msSelectMapping_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      UpdateDDMapping( e.ClickedItem.Text );
    }

    private void meDefaultsLoadAndGrab_Click( object sender, EventArgs e )
    {
      m_dumpSCJScommands = false; // disable this one

      // start over 
      InitActionTree( true );
      rtb.Text = SCMappings.Mapping( AppSettings.Instance.DefMappingName );
      Grab( );
      if ( SCMappings.IsUserMapping( AppSettings.Instance.DefMappingName ) ) {
        txMappingName.Text = AppSettings.Instance.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      btDump.BackColor = MyColors.DirtyColor;
      txMappingName.BackColor = MyColors.ValidColor;
    }

    private void meResetLoadAndGrab_Click( object sender, EventArgs e )
    {
      m_dumpSCJScommands = false; // disable this one

      // start over 
      InitActionTree( false );
      rtb.Text = SCMappings.Mapping( AppSettings.Instance.DefMappingName );
      if ( SCMappings.IsUserMapping( AppSettings.Instance.DefMappingName ) ) {
        txMappingName.Text = AppSettings.Instance.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      Grab( );
      txMappingName.BackColor = MyColors.ValidColor;
    }

    private void meLoadAndGrab_Click( object sender, EventArgs e )
    {
      m_dumpSCJScommands = false; // disable this one

      rtb.Text = SCMappings.Mapping( AppSettings.Instance.DefMappingName );
      Grab( );
      if ( SCMappings.IsUserMapping( AppSettings.Instance.DefMappingName ) ) {
        txMappingName.Text = AppSettings.Instance.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      btDump.BackColor = MyColors.DirtyColor;
      txMappingName.BackColor = MyColors.ValidColor;
    }

    private void meLoad_Click( object sender, EventArgs e )
    {
      m_dumpSCJScommands = false; // disable this one

      rtb.Text = SCMappings.Mapping( AppSettings.Instance.DefMappingName );
      if ( SCMappings.IsUserMapping( AppSettings.Instance.DefMappingName ) ) {
        txMappingName.Text = AppSettings.Instance.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      btGrab.BackColor = MyColors.DirtyColor;
      txMappingName.BackColor = MyColors.ValidColor;
    }


    // *** Context Menu Items

    // RTB Menu
    private void tsiCopy_Click( object sender, EventArgs e )
    {
      rtb.Focus( );
      if ( rtb.SelectionLength > 0 ) rtb.Copy( );
    }

    private void tsiPaste_Click( object sender, EventArgs e )
    {
      rtb.Focus( );
      rtb.Paste( DataFormats.GetFormat( DataFormats.UnicodeText ) );
      btGrab.BackColor = MyColors.DirtyColor;
    }

    private void tsiSelAll_Click( object sender, EventArgs e )
    {
      rtb.Focus( );
      rtb.SelectAll( );
    }

    private void tsiPReplace_Click( object sender, EventArgs e )
    {
      rtb.Focus( );
      rtb.SelectAll( );
      rtb.Paste( DataFormats.GetFormat( DataFormats.UnicodeText ) );
      btGrab.BackColor = MyColors.DirtyColor;
    }

    private void tsiOpen_Click( object sender, EventArgs e )
    {
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        rtb.LoadFile( OFD.FileName, RichTextBoxStreamType.PlainText );
        btGrab.BackColor = MyColors.DirtyColor;
      }
    }

    private void tsiSaveAs_Click( object sender, EventArgs e )
    {
      if ( SFD.ShowDialog( this ) == DialogResult.OK ) {
        rtb.SaveFile( SFD.FileName, RichTextBoxStreamType.PlainText );
      }
    }

    // *** Node Menu
    private ActivationMode m_prevActivationMode = new ActivationMode( ActivationMode.Default );


    private void cmAddDel_Opening( object sender, CancelEventArgs e )
    {
      // note: the right click selected the node
      ContextMenuStrip cts = ( sender as ContextMenuStrip );
      bool any2 = false;    // Group 2
      bool any3 = false;    // Group 3
      bool any4 = false;    // Group 4

      m_prevActivationMode = ActivationMode.Default; // switch Closing handling OFF in case we don't show anything

      if ( m_AT.CanAssignBinding ) {
        tdiAssignBinding.Text = Tx.Translate( tdiAssignBinding ) + ": " + JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked );
      }
      tdiAssignBinding.Visible = m_AT.CanAssignBinding; any2 = any2 || m_AT.CanAssignBinding; // Assign
      tdiBlendBinding.Visible = m_AT.CanDisableBinding; any2 = any2 || m_AT.CanDisableBinding; // Blend
      tdiClearBinding.Visible = m_AT.CanClearBinding; any2 = any2 || m_AT.CanClearBinding; // Clear

      tdiAddBinding.Visible = m_AT.CanAddBinding; any3 = any3 || m_AT.CanAddBinding; // Add
      tdiDelBinding.Visible = m_AT.CanDelBinding; any3 = any3 || m_AT.CanDelBinding; // Del

      // handle activation modes - there is a default one and the list of choosable ones
      // there is no further decision on can or cannot - any(2) is enough to know
      tdiCbxActivation.Visible = false;
      ActivationModes am = m_AT.ActivationModeSelectedItem( );
      // have to fudge around with a descriptive text here
      if ( am[0] == ActivationMode.Default )
        tdiTxDefActivationMode.Text = string.Format( "Profile: {0}", Tx.Translate( tdiTxDefActivationMode ) ); // show the default element
      else
        tdiTxDefActivationMode.Text = string.Format( "Profile: {0}", am[0].Name ); // show the default element

      if ( any2 && m_AT.IsMappedAction ) {
        m_prevActivationMode = am[1]; // this is the selected one
        tdiCbxActivation.Visible = true;
        any4 = true;
        tdiCbxActivation.Text = m_prevActivationMode.Name;
      }

      tdiSGroup1.Visible = any2; // separator
      tdiSGroup2.Visible = any3; // separator
      tdiSGroup3.Visible = any4; // separator
      tdiTxDefActivationMode.Visible = any4;

      e.Cancel = false; // !( any2 || any3 );
    }

    // Collapses all but the selected node or the part where it is in
    private void tdiCollapseAll_Click( object sender, EventArgs e )
    {
      TreeNode selNodeActionMap = treeView1.SelectedNode;
      TreeNode selNodeParent = selNodeActionMap;
      // see if we have a parent..
      if ( selNodeActionMap.Level > 1 )
        selNodeParent = selNodeActionMap.Parent;

      treeView1.CollapseAll( );
      selNodeParent.Expand( );
      treeView1.SelectedNode = selNodeActionMap;
      treeView1.SelectedNode.EnsureVisible( );
    }

    private void tdiExpandAll_Click( object sender, EventArgs e )
    {
      treeView1.ExpandAll( );
      treeView1.SelectedNode.EnsureVisible( );
    }


    private void tdiCbxActivation_Click( object sender, EventArgs e )
    {
      cmAddDel.Close( ToolStripDropDownCloseReason.ItemClicked );
      if ( !string.IsNullOrEmpty( m_prevActivationMode.Name ) && ( m_prevActivationMode.Name != (string)tdiCbxActivation.SelectedItem ) ) {
        tdiCbxActivation.Text = (string)tdiCbxActivation.SelectedItem;
        // seems to have changed - evaluate
        // it is either one of the ActivationModes, or profile default
        m_AT.UpdateActivationModeSelectedItem( tdiCbxActivation.Text );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
        m_prevActivationMode = ActivationMode.Default; // reset prev entry for next edit
      }
    }

    private void tdiAssignBinding_Click( object sender, EventArgs e )
    {
      btAssign_Click( sender, e );
    }

    private void tdiBlendBinding_Click( object sender, EventArgs e )
    {
      btBlend_Click( sender, e );
    }

    private void tdiClearBinding_Click( object sender, EventArgs e )
    {
      btClear_Click( sender, e );
    }


    private void tsiAddBinding_Click( object sender, EventArgs e )
    {
      // note: the right click selected the node
      log.Debug( "tsiAddBinding_Click" );
      m_AT.AddBinding( );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }

    private void tdiDelBinding_Click( object sender, EventArgs e )
    {
      // note: the right click selected the node
      log.Debug( "tdiDelBinding_Click" );
      m_AT.DelBinding( );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }

    // note: the right click selected the node
    private void tdiAddMod_Click( object sender, EventArgs e )
    {

    }



    // rtb drop xml file
    private void rtb_DragEnter( object sender, DragEventArgs e )
    {
      bool dropEnabled = true;
      if ( e.Data.GetDataPresent( DataFormats.FileDrop, true ) ) {
        string[] filenames =
                       e.Data.GetData( DataFormats.FileDrop, true ) as string[];

        foreach ( string filename in filenames ) {
          if ( System.IO.Path.GetExtension( filename ).ToUpperInvariant( ) != ".XML" ) {
            dropEnabled = false;
            break;
          }
        }
      }
      else {
        dropEnabled = false;
      }

      if ( dropEnabled ) {
        e.Effect = DragDropEffects.Copy;
      }
      else {
        e.Effect = DragDropEffects.None;
      }
    }

    private void rtb_DragDrop( object sender, DragEventArgs e )
    {
      // Loads the file into the control. 
      string[] droppedFilenames = e.Data.GetData( DataFormats.FileDrop, true ) as string[];
      if ( droppedFilenames.Length > 0 ) rtb.LoadFile( droppedFilenames[0], RichTextBoxStreamType.PlainText );
    }

    // *** XML load and save
    private void btSaveMyMapping_Click( object sender, EventArgs e )
    {
      bool cancel = false;

      AutoTabXML_Assignment( EATabXML.Tab_XML );

      if ( SCMappings.IsValidMappingName( txMappingName.Text ) ) {
        Dump( );
        if ( SCMappings.MappingFileExists( txMappingName.Text ) ) {
          cancel = ( MessageBox.Show( "File exists, shall we overwrite ?", "Save XML", MessageBoxButtons.YesNo ) == DialogResult.No );
        }
        if ( !cancel ) {
          rtb.SaveFile( SCMappings.MappingFileName( txMappingName.Text ), RichTextBoxStreamType.PlainText );
          TheUser.BackupMappingFile( txMappingName.Text ); // backup copy of the old one
          rtb.SaveFile( TheUser.MappingFileName( txMappingName.Text ), RichTextBoxStreamType.PlainText ); // also save the new one in the user space
          SetRebindField( txMappingName.Text );

          // get the new one into the list
          LoadMappingDD( );
          UpdateDDMapping( txMappingName.Text );
          AppSettings.Instance.MyMappingName = txMappingName.Text; AppSettings.Instance.Save( );// last used - persist
          txMappingName.BackColor = MyColors.SuccessColor;

          // autosave our XML for other activities
          string xmlList = $"<!-- {DateTime.Now} - SC Joystick Mapping ({txMappingName.Text}) --> \n{m_AT.ReportActionsXML( )}";
          using ( StreamWriter sw = File.CreateText( TheUser.MappingXmlFileName( txMappingName.Text ) ) ) {
            sw.Write( xmlList );
          }
          // autosave our Json for other activities
          var jexport = m_AT.ReportActionsJson( );
          jexport.Comment = $"{DateTime.Now} - SC Joystick Mapping ({txMappingName.Text})";
          jexport.WriteToFile( TheUser.MappingJsonFileName( txMappingName.Text ) );
        }
      }
      else {
        txMappingName.BackColor = MyColors.ErrorColor;
      }
    }

    private void txMappingName_TextChanged( object sender, EventArgs e )
    {
      if ( SCMappings.IsValidMappingName( txMappingName.Text ) ) {
        txMappingName.BackColor = MyColors.ValidColor;
      }
      else {
        txMappingName.BackColor = MyColors.InvalidColor;
      }
    }

    // *** Hyperlink

    private void linkLblReleases_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
    {
      this.linkLblReleases.LinkVisited = true;
      System.Diagnostics.Process.Start( c_GithubLink );
    }

    private void btClip_Click( object sender, EventArgs e )
    {
      Clipboard.SetText( txRebind.Text );
    }




    // *** Joystick Tuning

    private void cbxInv_XY_MouseClick( object sender, MouseEventArgs e )
    {
      m_AT.Dirty = true;
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }


    /// <summary>
    /// Updates Gamedevice, Nodetext for one Tuning (Option) item from current assignment
    /// </summary>
    /// <param name="deviceClass">The device class</param>
    /// <param name="optionName">The option to handle</param>
    /// <param name="action">The corresponding action</param>
    /// <param name="actionmap">The actionmap to search for the action</param>
    private bool UpdateTuningForDevice( string deviceClass, string optionName, string action, string actionmap )
    {
      DeviceTuningParameter tuning = null;
      DeviceCls dev = null;
      string match = "";
      string nodeText = "";

      if ( JoystickCls.IsDeviceClass( deviceClass ) ) {
        match = ActionTreeNode.ComposeNodeActionText( action, "js" );
      }
      else if ( GamepadCls.IsDeviceClass( deviceClass ) ) {
        match = ActionTreeNode.ComposeNodeActionText( action, "xi" );
      }
      else if ( MouseCls.IsDeviceClass( deviceClass ) ) {
        match = ActionTreeNode.ComposeNodeActionText( action + "_mouse", "mo" ); // CIG cannot decide on terminology rules at all...
      }

      nodeText = m_AT.FindText( actionmap, match ); // returns "" or a complete text ("action - command")
      // check for exit states
      if ( string.IsNullOrWhiteSpace( nodeText ) ) return false; // EXIT - no node assigned
      if ( Act.IsDisabledInput( ActionTreeNode.CommandFromActionText( nodeText ) ) ) return false; // EXIT disabled item

      // find the device for the action if it is an axis (analog command)
      string command = ActionTreeNode.CommandFromActionText( nodeText );
      if ( JoystickCls.IsAxisCommand( command ) ) {
        dev = DeviceInst.JoystickListRef.Find_InstanceForjsN( JoystickCls.JSNum( command ) );
      }
      else if ( GamepadCls.IsAxisCommand( command ) ) {
        dev = DeviceInst.GamepadRef;
      }
      else if ( MouseCls.IsAxisCommand( command ) ) {
        dev = DeviceInst.MouseRef;
      }
      // finally do the job..
      if ( dev != null ) {
        // find the tuning item of the action
        string toID = Tuningoptions.TuneOptionIDfromJsN( deviceClass, dev.XmlInstance );
        OptionTree ot = m_AT.ActionMaps.TuningOptions.OptionTreeFromToID( toID );
        if ( ot == null ) return false; // EXIT no optiontree for the device

        tuning = ot.TuningItem( optionName );  // set defaults
        if ( tuning == null ) return false; // EXIT no tuning item for the device

        string doID = Deviceoptions.DevOptionID( dev.DevClass, dev.DevName, nodeText );
        if ( m_AT.ActionMaps.DeviceOptions.ContainsKey( doID ) ) {
          tuning.AssignDynamicItems( dev, m_AT.ActionMaps.DeviceOptions[doID], nodeText );
        }
        else {
          tuning.AssignDynamicItems( dev, null, nodeText );
        }
      }
      return true;
    }

    /// <summary>
    /// Updates the option for the first device found only
    ///   Used for the Tuning Dialog, only one item can be tuned
    /// </summary>
    /// <param name="optionName">The option to handle</param>
    /// <param name="action">The corresponding action</param>
    /// <param name="actionmap">The actionmap to search for the action</param>
    private void UpdateTuningPrioritized( string optionName, string action, string actionmap )
    {
      bool retVal = UpdateTuningForDevice( JoystickCls.DeviceClass, optionName, action, actionmap );
      if ( !retVal ) retVal = UpdateTuningForDevice( GamepadCls.DeviceClass, optionName, action, actionmap );
      if ( !retVal ) retVal = UpdateTuningForDevice( MouseCls.DeviceClass, optionName, action, actionmap );
    }


    /// <summary>
    /// Get the assigned controls for some commands used in Tuning Yaw,Pitch,Roll and the Strafe ones
    /// Connect deviceOption if known
    /// </summary>
    private void UpdateTuningItems()
    {
      UpdateTuningPrioritized( "flight_move_pitch", "v_pitch", "spaceship_movement" );
      UpdateTuningPrioritized( "flight_move_yaw", "v_yaw", "spaceship_movement" );
      UpdateTuningPrioritized( "flight_move_roll", "v_roll", "spaceship_movement" );

      UpdateTuningPrioritized( "flight_move_strafe_vertical", "v_strafe_vertical", "spaceship_movement" );
      UpdateTuningPrioritized( "flight_move_strafe_lateral", "v_strafe_lateral", "spaceship_movement" );
      UpdateTuningPrioritized( "flight_move_strafe_longitudinal", "v_strafe_longitudinal", "spaceship_movement" );
    }


    /// <summary>
    /// Get the assigned controls for other Options - if available...
    /// </summary>
    private void UpdateAllTuningItems( string deviceClass )
    {
      UpdateTuningForDevice( deviceClass, "flight_move_pitch", "v_pitch", "spaceship_movement" );
      UpdateTuningForDevice( deviceClass, "flight_move_yaw", "v_yaw", "spaceship_movement" );
      UpdateTuningForDevice( deviceClass, "flight_move_roll", "v_roll", "spaceship_movement" );

      UpdateTuningForDevice( deviceClass, "flight_move_strafe_vertical", "v_strafe_vertical", "spaceship_movement" );
      UpdateTuningForDevice( deviceClass, "flight_move_strafe_lateral", "v_strafe_lateral", "spaceship_movement" );
      UpdateTuningForDevice( deviceClass, "flight_move_strafe_longitudinal", "v_strafe_longitudinal", "spaceship_movement" );

      UpdateTuningForDevice( deviceClass, "flight_view_pitch", "v_view_pitch", "spaceship_view" );
      UpdateTuningForDevice( deviceClass, "flight_view_yaw", "v_view_yaw", "spaceship_view" );

      UpdateTuningForDevice( deviceClass, "flight_throttle_abs", "v_throttle_abs", "spaceship_movement" );
      UpdateTuningForDevice( deviceClass, "flight_throttle_rel", "v_throttle_rel", "spaceship_movement" );

      UpdateTuningForDevice( deviceClass, "flight_aim_pitch", "v_aim_pitch", "spaceship_targeting" );
      UpdateTuningForDevice( deviceClass, "flight_aim_yaw", "v_aim_yaw", "spaceship_targeting" );

      UpdateTuningForDevice( deviceClass, "turret_aim_pitch", "v_aim_pitch", "spaceship_turret" );
      UpdateTuningForDevice( deviceClass, "turret_aim_yaw", "v_aim_yaw", "spaceship_turret" );

      UpdateTuningForDevice( deviceClass, "mgv_view_pitch", "v_view_pitch", "vehicle_general" );
      UpdateTuningForDevice( deviceClass, "mgv_view_yaw", "v_view_yaw", "vehicle_general" );
    }

    // *** Keyboard Input

    bool m_keyIn = false;
    bool m_mouseIn = false;

    private void btJsKbd_Click( object sender, EventArgs e )
    {
      m_keyIn = ( !m_keyIn );
      if ( m_keyIn ) {
        cbxThrottle.Checked = false; cbxThrottle.Enabled = false; // must be disabled..
        if ( DeviceInst.KeyboardRef == null ) {
          m_keyIn = false;
          btJsKbd.ImageKey = "J";
          return;
        } // bail out ..

        lblLastJ.BackColor = MyColors.KeyboardColor;
        btJsKbd.ImageKey = "K";
        lblLastJ.Focus( );
        DeviceInst.KeyboardRef.Activate( );
        DeviceInst.KeyboardRef.GetData( ); // poll to aquire once
      }
      else {
        m_mouseIn = false; // not longer
        lblLastJ.BackColor = MyColors.ValidColor;
        btJsKbd.ImageKey = "J";
        // m_Keyboard.Deactivate( );  // not longer with modifier mappings in AC 1.1
      }
    }


    // Key down triggers the readout via DX Input
    private void lblLastJ_KeyDown( object sender, KeyEventArgs e )
    {
      if ( m_keyIn ) {
        DeviceInst.KeyboardRef.GetData( );
        string modS = DeviceInst.KeyboardRef.GetLastChange( false ); // modifiers only
        string keyModS = DeviceInst.KeyboardRef.GetLastChange( true ); // modifiers+keyboard input
        // don't override modifiers when we are in mouse mode and the mod is the same and there is no kbd entry.... 
        if ( m_mouseIn && ( keyModS == modS ) && ( m_persistentMods == ( modS + "+" ) ) ) {
          ; // nothing here - 
        }
        else {
          m_mouseIn = false; // clear on kbd input - 20171226 must prepend text change
          lblLastJ.Text = DeviceInst.KeyboardRef.GetLastChange( true );
        }
        // also maintain persistent mods
        UpdateModifiers( );
      }
      // don't spill the field with regular input
      e.SuppressKeyPress = true;
      e.Handled = true;
    }

    private void UpdateAssignmentList()
    {
      string devInput = Act.DevInput( lblLastJ.Text, InputMode );
      RTF.RTFformatter RTF = new RTF.RTFformatter( );
      m_AT.ListAllActionsRTF( devInput, RTF );
      // have to check if throttle is used and if - add those to the list
      string altDevInput = JoystickCls.MakeThrottle( devInput, true );
      if ( altDevInput != devInput ) {
        m_AT.ListAllActionsRTF( altDevInput, RTF );
      }
      lbxOther.Rtf = RTF.RTFtext;
    }
    // text of input has changed
    private void lblLastJ_TextChanged( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_Assignment );
      UpdateAssignmentList( );
    }


    // maintain the global modifier store 
    private void UpdateModifiers()
    {
      if ( DeviceInst.KeyboardRef == null ) return;

      DeviceInst.KeyboardRef.GetData( );
      string modS = DeviceInst.KeyboardRef.GetLastChange( false );
      if ( !string.IsNullOrEmpty( modS ) ) {
        if ( modS.Contains( KeyboardCls.ClearMods ) ) {
          // allow to cancel modifiers
          m_persistentMods = ""; // kill persistent ones
        }
        else {
          m_persistentMods = modS + "+";
          m_modifierTimeout = c_modifierTime; // restart show interval
        }
      }
      else {
        if ( m_modifierTimeout <= 0 ) {
          m_persistentMods = ""; // modifier timed out
          m_mouseIn = false;
        }
      }
    }

    // *** Mouse Input

    private void cmMouseEntry_Opening( object sender, CancelEventArgs e )
    {
      if ( !m_keyIn ) e.Cancel = true;
    }


    // processes all mouse context menu and some unreachable KBD  item clicks
    private void tmeItem_Click( object sender, EventArgs e )
    {
      ToolStripMenuItem ts = (ToolStripMenuItem)sender;
      if ( string.IsNullOrEmpty( (string)ts.Tag ) ) return;

      string item = "";
      string device = MouseCls.DeviceClass;

      if ( int.TryParse( (string)ts.Tag, out int btNum ) ) {
        // got a button (most likely..)
        item = "mouse" + btNum.ToString( );
      }
      else if ( (string)ts.Tag == "X" )
        item = "maxis_x";
      else if ( (string)ts.Tag == "Y" )
        item = "maxis_y";
      else if ( (string)ts.Tag == "U" )
        item = "mwheel_up";
      else if ( (string)ts.Tag == "D" )
        item = "mwheel_down";
      else if ( (string)ts.Tag == "K_Tab" ) {
        item = "tab";
        device = KeyboardCls.DeviceClass;
      }

      string ctrl = "";
      // have to handle the two devices
      if ( MouseCls.IsDeviceClass( device ) ) {
        if ( DeviceInst.KeyboardRef == null ) {
          // no keyboard = no modifier 
          ctrl = MouseCls.MakeCtrl( item, "" ); // show last handled JS control
        }
        else {
          UpdateModifiers( );
          ctrl = MouseCls.MakeCtrl( item, m_persistentMods ); // show last handled JS control
        }
        m_mouseIn = true; // for this one only
      }
      else if ( KeyboardCls.IsDeviceClass( device ) ) {
        UpdateModifiers( );
        ctrl = KeyboardCls.MakeCtrl( item, m_persistentMods ); // show last handled JS control
        m_mouseIn = false;
      }

      lblLastJ.Text = ctrl;
    }

    #endregion

    #region DataTable Handling

    // Called when the table must be rebuild
    private void UpdateTable()
    {
      // only if needed
      if ( ( FTAB != null ) && FTAB.Visible ) {
        FTAB.SuspendDGV( );
        m_AT.ActionMaps.ToDataSet( FTAB.DS_AMaps );
        FTAB.ResumeDGV( );
        FTAB.Populate( );
      }
    }

    // Called when an entry has been modified
    private void UpdateTableSelectedItem()
    {
      // only if needed
      if ( ( FTAB != null ) && FTAB.Visible ) {
        string actionID = m_AT.SelectedActionID;
        m_AT.ActionMaps.UpdateDataSet( FTAB.DS_AMaps, actionID );
      }
    }

    // called when the user clicks Update from the Table Window
    private void FTAB_UpdateEditEvent( object sender, UpdateEditEventArgs e )
    {
      ActionTree newTree = m_AT.UpdateFromDataSet( FTAB.DS_AMaps );

      // returns a null if no changes have been found
      if ( newTree != null ) {
        m_AT.NodeSelectedEvent -= M_AT_NodeSelectedEvent; // disconnect the Event
        m_AT = newTree; // make it the valid one
        m_AT.NodeSelectedEvent += M_AT_NodeSelectedEvent; // reconnect the Event

        m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
        m_AT.ReloadTreeView( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }

    }

    // called when the user if the TAB form wants to edit a row
    private void FTAB_EditActionEvent( object sender, EditRowEventArgs e )
    {
      m_AT.FindAndSelectActionKey( e.Actionmap, e.Actionkey, e.Nodeindex );
    }


    #endregion


  }
}
