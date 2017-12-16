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

using SCJMapper_V2.SC;
using SCJMapper_V2.Table;
using SCJMapper_V2.Keyboard;
using SCJMapper_V2.Mouse;
using SCJMapper_V2.Gamepad;
using SCJMapper_V2.Joystick;
using SCJMapper_V2.Options;

namespace SCJMapper_V2
{
  public partial class MainForm : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private const string c_GithubLink = @"https://github.com/SCToolsfactory/SCJMapper-V2/releases";

    private AppSettings m_AppSettings = new AppSettings( );
    private bool m_appLoading = true; // used to detect if we are loading (or running)

    // keyboard modifier handling variables
    private string m_persistentMods = "";
    private const int c_modifierTime = 3500; // msec time before a modifier times out and will be removed
    private int m_modifierTimeout = 0;

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
      } catch {
        return false;
      }
    }

    /// <summary>
    /// Detects and returns the current Input device
    /// </summary>
    private ActionCls.ActionDevice InputMode
    {
      get {
        // take care of the sequence.. mouse overrides key but both override joy and game
        if ( m_mouseIn ) {   // 20151220BM: add mouse device (from AC 2.0 defaultProfile usage)
          return ActionCls.ActionDevice.AD_Mouse;
        }
        else if ( m_keyIn ) {
          return ActionCls.ActionDevice.AD_Keyboard;
        }
        else {
          if ( IsGamepadTab( tc1.SelectedTab ) ) {
            return ActionCls.ActionDevice.AD_Gamepad;
          }
          else {
            return ActionCls.ActionDevice.AD_Joystick;
          }
        }
      }
    }

    /// <summary>
    /// Get the current JsN string for the active device tab
    /// </summary>
    /// <returns>The jsN string - can be jsx, js1..jsN</returns>
    private string JSStr()
    {
      UC_JoyPanel jp = (UC_JoyPanel)( tc1.SelectedTab.Controls["UC_JoyPanel"] );
      return jp.JsName;
    }

    // tab index for the tcXML control
    private enum EATabXML
    {
      Tab_XML = 0,
      Tab_Assignment,
    }

    private void AutoTabXML_Assignment( EATabXML tab )
    {
      if ( m_AppSettings.AutoTabXML ) {
        if ( tcXML.SelectedIndex != (int)tab ) {
          tcXML.SelectedTab = tcXML.TabPages[(int)tab];
          if ( tab == EATabXML.Tab_Assignment )
            lblLastJ.Select( ); // select again as when changing the Tabs
        }
      }
    }

    private void UpdateDDMapping(string mapName )
    {
      tsDDbtMappings.Text = mapName;
      m_AppSettings.DefMappingName = mapName; m_AppSettings.Save( );
    }


    #endregion

    #region Main Form Handling


    public MainForm()
    {

      try {
        // Load the icon from our resources
        System.Resources.ResourceManager resources = new System.Resources.ResourceManager( this.GetType( ) );
        this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      } catch {
        ; // well...
      }

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


    private void LoadMappingDD()
    {
      SCMappings.UpdateMappingNames( );
      tsDDbtMappings.DropDownItems.Clear( );
      foreach ( string s in SCMappings.MappingNames ) {
        tsDDbtMappings.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ) );
      }
    }

    /// <summary>
    /// Indicates if the SC directory is a valid one
    /// </summary>
    private void SCFileIndication()
    {
      if ( string.IsNullOrEmpty( SCPath.SCClientMappingPath ) ) tsDDbtMappings.BackColor = MyColors.InvalidColor;
      else tsDDbtMappings.BackColor = MyColors.MappingColor;
    }


    /// <summary>
    ///  Handle the load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_Load( object sender, System.EventArgs e )
    {
      log.Debug( "MainForm_Load - Entry" );

      // some applic initialization 
      // Assign Size property, since databinding to Size doesn't work well.

      this.Size = m_AppSettings.FormSize;
      this.Location = m_AppSettings.FormLocation;

      string version = Application.ProductVersion;  // get the version information
      // BETA VERSION; TODO -  comment out if not longer
      //lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // PRODUCTION
      lblTitle.Text += " - V " + version + " beta"; // BETA

      log.InfoFormat( "Application Version: {0}", version.ToString( ) );

      // tooltips where needed
      toolTip1.SetToolTip( this.linkLblReleases, c_GithubLink ); // allow to see where the link may head

      // XML RTB
      log.Debug( "Loading RTB" );
      rtb.SelectionTabs = new int[] { 10, 20, 30, 40, 50, 60 }; // short tabs
      rtb.DragEnter += new DragEventHandler( rtb_DragEnter );
      rtb.DragDrop += new DragEventHandler( rtb_DragDrop );
      rtb.AllowDrop = true; // add Drop to rtb

      // load mappings
      log.Debug( "Loading Mappings" );
      LoadMappingDD( );
      tsDDbtMappings.Text = m_AppSettings.DefMappingName;

      SCFileIndication( );

      // load other defaults
      log.Debug( "Loading Other" );
      txMappingName.Text = m_AppSettings.MyMappingName;
      SetRebindField( txMappingName.Text );
      foreach ( ToolStripDropDownItem d in tsDDbtMappings.DropDownItems ) {
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
        m_AppSettings.MyMappingName = txMappingName.Text; m_AppSettings.Save( );// last used - persist
        txMappingName.BackColor = MyColors.SuccessColor;
      }
      else {
        log.WarnFormat( "Last used mapping not available ({0})", txMappingName.Text );
        txMappingName.BackColor = MyColors.ErrorColor;
      }

      // load Mouse menu strip
      if ( DeviceInst.MouseRef != null ) {
        for ( int i = 0; i < DeviceInst.MouseRef.NumberOfButtons; i++ ) {
          ToolStripMenuItem ts = new ToolStripMenuItem( "Button " + ( i + 1 ).ToString( ), null, new EventHandler( tmeItem_Click ) );
          ts.Tag = ( i + 1 ).ToString( );
          cmMouseEntry.Items.Add( ts );
        }
      }


      // load show checkboxes
      cbxShowJoystick.Checked = m_AppSettings.ShowJoystick;
      cbxShowGamepad.Checked = m_AppSettings.ShowGamepad;
      cbxShowKeyboard.Checked = m_AppSettings.ShowKeyboard;
      cbxShowMouse.Checked = m_AppSettings.ShowMouse;
      cbxShowMappedOnly.Checked = m_AppSettings.ShowMapped;

      // init current Joystick
      int jsIndex = (int)tc1.SelectedTab.Tag; // gets the index into the JS list
      if ( jsIndex >= 0 ) DeviceInst.JoystickInst = DeviceInst.JoystickListRef[jsIndex];

      // init PTU folder usage sign
      lblPTU.Visible = false; // m_AppSettings.UsePTU;  no longer used
      if ( m_AppSettings.UsePTU ) log.Debug( "Using PTU Folders" );

      // Auto Tab XML
      cbxAutoTabXML.Checked = m_AppSettings.AutoTabXML;

      // poll the XInput
      log.Debug( "Start XInput polling" );
      timer1_Tick( null,null );

      timer1.Start( ); // this one polls the joysticks to show the props

      // Select XML tab to start with 
      AutoTabXML_Assignment( EATabXML.Tab_XML );

      m_appLoading = false; // no longer
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
      // init current Joystick
      int jsIndex = (int)tc1.SelectedTab.Tag; // gets the index into the JS list
      if ( jsIndex >= 0 )
        DeviceInst.JoystickInst = DeviceInst.JoystickListRef[jsIndex];
      else
        DeviceInst.JoystickInst = null;
    }

    /// <summary>
    /// Fancy tab coloring with ownerdraw to paint the callout buttons
    /// </summary>
    private void tc1_DrawItem( object sender, DrawItemEventArgs e )
    {
      try {
        //This line of code will help you to change the apperance like size,name,style.
        Font f;
        //For background color
        Brush backBrush = new SolidBrush( MyColors.TabColor[e.Index] );
        //For forground color
        Brush foreBrush = new SolidBrush( Color.Black );


        //This construct will hell you to deside which tab page have current focus
        //to change the style.
        if ( e.Index == this.tc1.SelectedIndex ) {
          //This line of code will help you to change the apperance like size,name,style.
          f = new Font( e.Font, FontStyle.Bold | FontStyle.Bold );
          f = new Font( e.Font, FontStyle.Bold );

          Rectangle tabRect = tc1.Bounds;
          Region tabRegion = new Region( tabRect );
          Rectangle TabItemRect = new Rectangle( 0, 0, 0, 0 );
          for ( int nTanIndex = 0; nTanIndex < tc1.TabCount; nTanIndex++ ) {
            TabItemRect = Rectangle.Union( TabItemRect, tc1.GetTabRect( nTanIndex ) );
          }
          tabRegion.Exclude( TabItemRect );
          e.Graphics.FillRegion( backBrush, tabRegion );
        }
        else {
          f = e.Font;
          foreBrush = new SolidBrush( e.ForeColor );
        }

        //To set the alignment of the caption.
        string tabName = this.tc1.TabPages[e.Index].Text;
        StringFormat sf = new StringFormat( );
        sf.Alignment = StringAlignment.Center;

        //Thsi will help you to fill the interior portion of
        //selected tabpage.
        e.Graphics.FillRectangle( backBrush, e.Bounds );
        Rectangle r = e.Bounds;
        r = new Rectangle( r.X, r.Y + 3, r.Width, r.Height - 3 );
        e.Graphics.DrawString( tabName, f, foreBrush, r, sf );

        sf.Dispose( );
        if ( e.Index == this.tc1.SelectedIndex ) {
          f.Dispose( );
          backBrush.Dispose( );
        }
        else {
          backBrush.Dispose( );
          foreBrush.Dispose( );
        }
      } catch ( Exception Ex ) {
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
      log.DebugFormat( "InitActionTree - New AT: {0}", m_AT.GetHashCode().ToString() );

      m_AT.NodeSelectedEvent += M_AT_NodeSelectedEvent; // connect the Event

      m_AT.Ctrl = treeView1;  // the ActionTree owns the TreeView control
      m_AT.IgnoreMaps = m_AppSettings.IgnoreActionmaps;
      // provide the display items (init)
      m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
      // Init with default profile filepath
      m_AT.LoadProfileTree( SCDefaultProfile.DefaultProfileName, addDefaultBinding );
      lblProfileUsed.Text = SCDefaultProfile.UsedDefProfile; // SCA 2.2 show used profile

      // Activation Update
      tdiCbxActivation.Items.Clear( );
      tdiCbxActivation.Items.AddRange( ActivationModes.Instance.Names.ToArray( ) );
      tdiCbxActivation.SelectedIndex = 0;

      // apply a default JS to Joystick mapping - can be changed and reloaded from XML mappings
      // must take care of Gamepads if there are (but we take care of one only...)
      //@@@@@@@
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
            m_AT.ActionMaps.jsNGUID[deviceTabIndex] = DeviceInst.JoystickListRef[joyStickIndex].DevInstanceGUID;
            joyStickIndex++;
          }
        }
      }
      m_AT.FilterTree( txFilter.Text );
    }



    // Helper: collect the joysticks here
    struct myDxJoystick
    {
      public SharpDX.DirectInput.Joystick js;
      public string prodName;
    }

    /// <summary>
    /// Aquire the DInput joystick devices
    /// </summary>
    /// <returns></returns>
    public bool InitDirectInput()
    {
      log.Debug( "Entry" );

      // Enumerate gamepads in the system.
      SharpDX.XInput.UserIndex gpDeviceIndex = SharpDX.XInput.UserIndex.Any;

      // Initialize DirectInput
      log.Debug( "Instantiate DirectInput" );
      var directInput = new DirectInput( );

      try {
        log.Debug( "Get Keyboard device" );
        DeviceInst.KeyboardInst = new KeyboardCls( new SharpDX.DirectInput.Keyboard( directInput ), this );

        log.Debug( "Get Mouse device" );
        DeviceInst.MouseInst = new MouseCls( new SharpDX.DirectInput.Mouse( directInput ), this );

      } catch ( Exception ex ) {
        log.Debug( "InitDirectInput phase 1 failed unexpectedly", ex );
        return false;
      }


      List<myDxJoystick> dxJoysticks = new List<myDxJoystick>( );
      SharpDX.XInput.Controller dxGamepad = null;

      try {
        // scan the Input for attached devices
        log.Debug( "Scan GameControl devices" );
        foreach ( DeviceInstance instance in directInput.GetDevices( DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly ) ) {
          log.InfoFormat( "GameControl: Type:{0} Device:{1}", instance.Type.ToString( ), instance.ProductName );
          // Create the device interface
          log.Debug( "Create the device interface" );
          if ( m_AppSettings.DetectGamepad && ( instance.Usage == SharpDX.Multimedia.UsageId.GenericGamepad ) ) {
            // detect Gamepad only if the user wishes to do so
            for ( SharpDX.XInput.UserIndex i = SharpDX.XInput.UserIndex.One; i < SharpDX.XInput.UserIndex.Four; i++ ) {
              dxGamepad = new SharpDX.XInput.Controller( i );
              if ( dxGamepad.IsConnected ) {
                log.InfoFormat( "Scan Input {0} for gamepad - {1}", i, dxGamepad.GetCapabilities( SharpDX.XInput.DeviceQueryType.Gamepad ).ToString( ) );
                gpDeviceIndex = i;
                break; // get only the first one
              }
            }
          }
          else {
            myDxJoystick myJs = new myDxJoystick( );
            myJs.js = new SharpDX.DirectInput.Joystick( directInput, instance.InstanceGuid );
            myJs.prodName = instance.ProductName;
            dxJoysticks.Add( myJs );
            log.DebugFormat( "Create the device interface for: {0}", myJs.prodName );
          }
        }
      } catch ( Exception ex ) {
        log.Debug( "InitDirectInput phase 2 failed unexpectedly", ex );
        return false;
      }


      int tabs = 0;
      // make the GP the first device if there is one.
      if ( dxGamepad != null ) {
        log.Debug( "Add first Gamepad panel" );
        tc1.TabPages[tabs].Text = "Gamepad ";
        UC_GpadPanel uUC_GpadPanelNew = new UC_GpadPanel( ); tc1.TabPages[tabs].Controls.Add( uUC_GpadPanelNew );
        uUC_GpadPanelNew.Size = UC_JoyPanel.Size; uUC_GpadPanelNew.Location = UC_JoyPanel.Location;
        UC_JoyPanel.Enabled = false; UC_JoyPanel.Visible = false; // don't use this one 
        log.Debug( "Create Gamepad instance" );
        DeviceInst.GamepadInst = new GamepadCls( dxGamepad, uUC_GpadPanelNew, tabs ); // does all device related activities for that particular item
        DeviceInst.GamepadRef.SetDeviceName( GamepadCls.DevNameCIG ); // this is fixed ...
        tc1.TabPages[tabs].ToolTipText = string.Format( "{0}\n{1}", DeviceInst.GamepadRef.DevName, " " );
        toolTip1.SetToolTip( tc1.TabPages[tabs], tc1.TabPages[tabs].ToolTipText );

        SetGamepadTab( tc1.TabPages[tabs] );  // indicates the gamepad tab (murks..)
        MyColors.TabColor[tabs] = MyColors.GamepadColor; // save it for future use of tab coloring (drawing)
        tc1.TabPages[tabs].BackColor = MyColors.TabColor[tabs];

        tabs++; // next tab
      }

      // do all joysticks
      int nJs = 0; // number the Joystick Tabs
      foreach ( myDxJoystick myJs in dxJoysticks ) {
        // we have the first tab made as reference so TabPage[0] already exists
        JoystickCls js = null; UC_JoyPanel uUC_JoyPanelNew = null;
        if ( tabs == 0 ) {
          // first panel - The Tab content exists already 
          log.Debug( "Add first Joystick panel" );
          uUC_JoyPanelNew = UC_JoyPanel;
        }
        else {
          log.Debug( "Add next Joystick panel" );
          // setup the further tab contents along the reference one in TabPage[0] (the control named UC_JoyPanel)
          tc1.TabPages.Add( "" );  // numbering is 1 based for the user
          uUC_JoyPanelNew = new UC_JoyPanel( ); tc1.TabPages[tabs].Controls.Add( uUC_JoyPanelNew );
          uUC_JoyPanelNew.Size = UC_JoyPanel.Size; uUC_JoyPanelNew.Location = UC_JoyPanel.Location;
          //uUC_JoyPanelNew.Dock = UC_JoyPanel.Dock; uUC_JoyPanelNew.Anchor = UC_JoyPanel.Anchor;
          //uUC_JoyPanelNew.AutoScaleMode = UC_JoyPanel.AutoScaleMode; uUC_JoyPanelNew.AutoSize = UC_JoyPanel.AutoSize;

        }
        // common part
        tc1.TabPages[tabs].Text = string.Format( "Joystick {0}", nJs + 1 ); // numbering is 1 based for the user
        log.Debug( "Create Joystick instance " + nJs.ToString( ) );
        js = new JoystickCls( myJs.js, this, nJs, uUC_JoyPanelNew, tabs ); // does all device related activities for that particular item
        DeviceInst.JoystickListRef.Add( js ); // add to joystick list
        tc1.TabPages[tabs].ToolTipText = string.Format( "{0}\n{1}", js.DevName, js.DevInstanceGUID );
        toolTip1.SetToolTip( tc1.TabPages[tabs], tc1.TabPages[tabs].ToolTipText );
        tc1.TabPages[tabs].BackColor = MyColors.TabColor[tabs];
        tc1.TabPages[tabs].Tag = js.DevInstance;  //  used to find the tab for polling

        nJs++; // next joystick
        // next Joystick tab
        tabs++;
        if ( tabs >= JoystickCls.JSnum_MAX ) break; // cannot load more JSticks than predefined Tabs
      }

      log.DebugFormat( "Added {0} GameControl devices", tabs );

      if ( tabs == 0 ) {
        log.Warn( "Unable to find and/or create any joystick devices." );
        MessageBox.Show( "Unable to create a joystick device. Program will exit.", "No joystick found", MessageBoxButtons.OK, MessageBoxIcon.Information );
        return false;
      }

      // load the profile items from the XML
      log.Debug( "Init ActionTree" );
      InitActionTree( true );

      return true;
    }

    #endregion




    /// <summary>
    ///  Grab the rtb data and load them into config
    /// </summary>
    private void Grab()
    {
      log.Debug( "Grab - Entry" );

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
      } catch {
        ; // just ignore
      }
      UpdateTable( );
    }


    /// <summary>
    /// Dump Config into rtb
    /// </summary>
    private void Dump()
    {
      log.Debug( "Dump - Entry" );

      AutoTabXML_Assignment( EATabXML.Tab_XML );

      rtb.Text = string.Format( "<!-- {0} - SC Joystick Mapping - {1} -->\n{2}", DateTime.Now, txMappingName.Text, m_AT.toXML( txMappingName.Text ) );

      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
    }


    private void SetRebindField( string map )
    {
      txRebind.Text = "pp_rebindkeys " + map;
    }


    #region Event Handling

    // Form Events

    private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
    {
      log.Debug( "MainForm_FormClosing - Entry" );

      m_AppSettings.FormSize = this.Size;
      m_AppSettings.FormLocation = this.Location;

      if ( FTAB != null ) {
        m_AppSettings.FormTableLocation = FTAB.LastLocation;
        m_AppSettings.FormTableSize = FTAB.LastSize;
        m_AppSettings.FormTableColumnWidth = FTAB.LastColSize;

        FTAB.Close( );
        FTAB = null;
      }

      m_AppSettings.Save( );
    }


    // polls the devices to get the latest update
    private void timer1_Tick( object sender, System.EventArgs e )
    {
      // Handle Kbd modifier timeout for joystick
      m_modifierTimeout -= timer1.Interval;  // decrement timeout
      if ( m_modifierTimeout < 0 ) m_modifierTimeout = 0; // prevent undeflow after long time not using modifiers


      if ( m_keyIn || tc1.SelectedTab.Tag == null ) return; // don't handle those

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
      btMakeMod.Enabled = JoystickCls.ValidModifier( ctrl );

    }


    // TreeView Events

    private void treeView1_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
    {
      if ( e.Button == MouseButtons.Right ) {
        treeView1.SelectedNode = e.Node; // trigger ActionTree events..
      }
    }

    // Action Tree Event - manages the de/selection of a node
    private void M_AT_NodeSelectedEvent( object sender, ActionTreeEventArgs e )
    {
      lblAction.Text = e.SelectedAction;
      lblAssigned.Text = e.SelectedCtrl;
    }


    // Show options

    private void cbxShowTreeOptions_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_AT == null ) return; // on init
      m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMouse.Checked, cbxShowMappedOnly.Checked );
      m_AT.ReloadTreeView( );

      if ( m_appLoading ) return; // don't assign while loading defaults
      m_AppSettings.ShowJoystick = cbxShowJoystick.Checked; m_AppSettings.ShowGamepad = cbxShowGamepad.Checked;
      m_AppSettings.ShowKeyboard = cbxShowKeyboard.Checked; m_AppSettings.ShowMouse = cbxShowMouse.Checked;
      m_AppSettings.ShowMapped = cbxShowMappedOnly.Checked;
    }



    // Assign Panel Items

    private void btFind_Click( object sender, EventArgs e )
    {
      m_AT.FindAndSelectCtrl( JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked ) , ""); // find the action for a Control (joystick input)
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
      if ( m_AT.CanBlendBinding ) {
        m_AT.BlendBinding( );
        UpdateTableSelectedItem( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }
      else MySounds.PlayCannot( );
    }

    private void btClear_Click( object sender, EventArgs e )
    {
      log.Debug( "btClear_Click" );
      if ( m_AT.CanClearBinding || m_AT.CanBlendBinding ) {
        m_AT.ClearBinding( );
        UpdateTableSelectedItem( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }
      else MySounds.PlayCannot( );
    }

    //TODO
    // possibly obsolete - we dont support to make own modifiers - button is invisible
    private void btMakeMod_Click( object sender, EventArgs e )
    {
    }
    // possibly obsolete - we dont support to make own modifiers
    private void Cbx_CheckedChanged( object sender, EventArgs e )
    {
    }


    // General Area Items

    private void btDump_Click( object sender, EventArgs e )
    {
      Dump( );
    }

    private void btDumpList_Click( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_XML );

      if ( m_AppSettings.UseCSVListing )
        rtb.Text = string.Format( "-- {0} - SC Joystick Mapping --\n{1}", DateTime.Now, m_AT.ReportActionsCSV( m_AppSettings.ListModifiers ) );
      else
        rtb.Text = string.Format( "-- {0} - SC Joystick Mapping --\n{1}", DateTime.Now, m_AT.ReportActions( ) );

    }

    private void btDumpLog_Click( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_XML );

      rtb.Text = string.Format( "-- {0} - SC Joystick AC Log Controller Detection --\n{1}", DateTime.Now, SCLogExtract.ExtractLog( ) );
    }

    private void btDumpProfile_Click( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_XML );

      rtb.Text = SCDefaultProfile.DefaultProfile( SCDefaultProfile.DefaultProfileName );
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
      m_AppSettings.AutoTabXML = cbxAutoTabXML.Checked; m_AppSettings.Save( );
    }

    // Toolstrip Items

    private void tsBtReset_ButtonClick( object sender, EventArgs e )
    {
    }

    private void resetEmptyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // start over 
      InitActionTree( false );
      rtb.Text = "";
      UpdateTable( );
    }

    private void resetDefaultsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // start over and if chosen, load defaults from SC game
      InitActionTree( true );
      rtb.Text = "";
      UpdateTable( );
    }

    private void tsDDbtMappings_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      UpdateDDMapping( e.ClickedItem.Text );
    }


    private void loadToolStripMenuItem_Click( object sender, EventArgs e )
    {
      rtb.Text = SCMappings.Mapping( m_AppSettings.DefMappingName );
      if ( SCMappings.IsUserMapping( m_AppSettings.DefMappingName ) ) {
        txMappingName.Text = m_AppSettings.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      btGrab.BackColor = MyColors.DirtyColor;
      txMappingName.BackColor = MyColors.ValidColor;
    }

    private void loadAndGrabToolStripMenuItem_Click( object sender, EventArgs e )
    {
      rtb.Text = SCMappings.Mapping( m_AppSettings.DefMappingName );
      Grab( );
      if ( SCMappings.IsUserMapping( m_AppSettings.DefMappingName ) ) {
        txMappingName.Text = m_AppSettings.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      btDump.BackColor = MyColors.DirtyColor;
      txMappingName.BackColor = MyColors.ValidColor;
    }

    private void resetLoadAndGrabToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // start over 
      InitActionTree( false );
      rtb.Text = SCMappings.Mapping( m_AppSettings.DefMappingName );
      if ( SCMappings.IsUserMapping( m_AppSettings.DefMappingName ) ) {
        txMappingName.Text = m_AppSettings.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      Grab( );
      txMappingName.BackColor = MyColors.ValidColor;
    }

    private void defaultsLoadAndGrabToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // start over 
      InitActionTree( true );
      rtb.Text = SCMappings.Mapping( m_AppSettings.DefMappingName );
      Grab( );
      if ( SCMappings.IsUserMapping( m_AppSettings.DefMappingName ) ) {
        txMappingName.Text = m_AppSettings.DefMappingName;
        SetRebindField( txMappingName.Text );
      }
      btDump.BackColor = MyColors.DirtyColor;
      txMappingName.BackColor = MyColors.ValidColor;
    }


    // Context Menu Items

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
      if ( OFD.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
        rtb.LoadFile( OFD.FileName, RichTextBoxStreamType.PlainText );
        btGrab.BackColor = MyColors.DirtyColor;
      }
    }

    private void tsiSaveAs_Click( object sender, EventArgs e )
    {
      if ( SFD.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
        rtb.SaveFile( SFD.FileName, RichTextBoxStreamType.PlainText );
      }
    }

    // Node Menu
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
        tdiAssignBinding.Text = "Assign: " + JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked );
      }
      tdiAssignBinding.Visible = m_AT.CanAssignBinding; any2 = any2 || m_AT.CanAssignBinding; // Assign
      tdiBlendBinding.Visible = m_AT.CanBlendBinding; any2 = any2 || m_AT.CanBlendBinding; // Blend
      tdiClearBinding.Visible = m_AT.CanClearBinding; any2 = any2 || m_AT.CanClearBinding; // Clear

      tdiAddBinding.Visible = m_AT.CanAddBinding; any3 = any3 || m_AT.CanAddBinding; // Add
      tdiDelBinding.Visible = m_AT.CanDelBinding; any3 = any3 || m_AT.CanDelBinding; // Del


      // handle activation modes - there is a default one and the list of choosable ones
      // there is no further decision on can or cannot - any(2) is enough to know
      tdiCbxActivation.Visible = false;
      ActivationModes am = m_AT.ActivationModeSelectedItem( );
      // have to fudge around with a descriptive text here
      if ( am[0] == ActivationMode.Default )
        tdiTxDefActivationMode.Text = string.Format( "Profile: {0}", "no ActivationMode" ); // show the default element
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

    // after user entry of the context menu - see if one has changed the ActivationMode
    private void cmAddDel_Closed( object sender, ToolStripDropDownClosedEventArgs e )
    {
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

    // XML load and save
    private void btSaveMyMapping_Click( object sender, EventArgs e )
    {
      bool cancel = false;

      AutoTabXML_Assignment( EATabXML.Tab_XML );

      if ( SCMappings.IsValidMappingName( txMappingName.Text ) ) {
        Dump( );
        if ( SCMappings.MappingFileExists( txMappingName.Text ) ) {
          cancel = ( MessageBox.Show( "File exists, shall we overwrite ?", "Save XML", MessageBoxButtons.YesNo ) == System.Windows.Forms.DialogResult.No );
        }
        if ( !cancel ) {
          rtb.SaveFile( SCMappings.MappingFileName( txMappingName.Text ), RichTextBoxStreamType.PlainText );
          TheUser.BackupMappingFile( txMappingName.Text ); // backup copy of the old one
          rtb.SaveFile( TheUser.MappingFileName( txMappingName.Text ), RichTextBoxStreamType.PlainText ); // also save the new one in the user space
          SetRebindField( txMappingName.Text );

          // get the new one into the list
          LoadMappingDD( );
          UpdateDDMapping( txMappingName.Text );
          m_AppSettings.MyMappingName = txMappingName.Text; m_AppSettings.Save( );// last used - persist
          txMappingName.BackColor = MyColors.SuccessColor;
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

    // Hyperlink

    private void linkLblReleases_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
    {
      this.linkLblReleases.LinkVisited = true;
      System.Diagnostics.Process.Start( c_GithubLink );
    }

    private void btClip_Click( object sender, EventArgs e )
    {
      Clipboard.SetText( txRebind.Text );
    }


    // Settings

    private void btSettings_Click( object sender, EventArgs e )
    {
      // have to stop polling while the Settings window is open
      timer1.Enabled = false;
      if ( m_AppSettings.ShowSettings( "" ) != System.Windows.Forms.DialogResult.Cancel ) {
        m_AppSettings.Reload( ); // must reload in case of any changes in the form
        // then reload the profile and mappings
        LoadMappingDD( );
        // indicates (in)valid folders
        SCFileIndication( );

        // now update the contents according to new settings
        foreach ( JoystickCls j in DeviceInst.JoystickListRef ) j.ApplySettings( ); // update Seetings
        m_AT.IgnoreMaps = m_AppSettings.IgnoreActionmaps;
        // and start over with an empty tree
        InitActionTree( false );
        UpdateTable( );
      }

      timer1.Enabled = true;
    }

    private void btJsReassign_Click( object sender, EventArgs e )
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
          j = DeviceInst.JoystickListRef.Find_jsN( i + 1 );
          if ( j != null ) {
            newTree.ActionMaps.jsN[i] = j.DevName; newTree.ActionMaps.jsNGUID[i] = j.DevInstanceGUID;
          }
          else {
            newTree.ActionMaps.jsN[i] = ""; newTree.ActionMaps.jsNGUID[i] = "";
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


    // Joystick Tuning

    private void cbxInv_XY_MouseClick( object sender, MouseEventArgs e )
    {
      m_AT.Dirty = true;
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }


    /// <summary>
    /// Updates Gamedevice, Nodetext for one Tuning (Option) item from current assignment
    /// </summary>
    /// <param name="optionName">THe option to handle</param>
    /// <param name="action">The corresponding action</param>
    /// <param name="actionmap">The actionmap to search for the action</param>
    private void UpdateOptionItem( string optionName, string action, string actionmap )
    {
      // get current mapping from ActionMaps
      string nodeText = "";

      // attach Yaw command
      DeviceTuningParameter tuning = null;
      DeviceCls dev = null;
      string find = "";

      // find action item for Joysticks
      find = ActionTreeNode.ComposeNodeText( action, "js" );
      nodeText = m_AT.FindText( actionmap, find ); // returns "" or a complete text ("action - command")
      if ( !string.IsNullOrWhiteSpace( nodeText ) ) {
        if ( !ActionCls.IsBlendedInput( ActionTreeNode.CommandFromNodeText( nodeText ) ) ) {
          dev = DeviceInst.JoystickListRef.Find_jsN( JoystickCls.JSNum( ActionTreeNode.CommandFromNodeText( nodeText ) ) );
          if ( dev != null ) {
            // find the tuning item of the action
            string toID = Tuningoptions.TuneOptionIDfromJsN( JoystickCls.DeviceClass, dev.XmlInstance );
            OptionTree ot = m_AT.ActionMaps.TuningOptions.OptionTreeFromToID( toID );
            if ( ot != null ) tuning = ot.TuningItem( optionName );  // set defaults
          }
        }
      }

      if ( dev == null ) {
        // nothing found? find action item for GPads
        find = ActionTreeNode.ComposeNodeText( action, "xi" );
        nodeText = m_AT.FindText( actionmap, find );
        if ( !string.IsNullOrWhiteSpace( nodeText ) ) {
          if ( !ActionCls.IsBlendedInput( ActionTreeNode.CommandFromNodeText( nodeText ) ) ) {
            dev = DeviceInst.GamepadRef;
            if ( dev != null ) {
              // find the tuning item of the action
              string toID = Tuningoptions.TuneOptionIDfromJsN( GamepadCls.DeviceClass, dev.XmlInstance );
              OptionTree ot = m_AT.ActionMaps.TuningOptions.OptionTreeFromToID( toID );
              if ( ot != null ) tuning = ot.TuningItem( optionName );  // set defaults
            }
          }
        }
      }
      // dev might be null here if no device for the action was found
      // tuning might be null here if no tuningitem for the device action was found (which should not happen !!)
      if ( ( dev != null ) && ( tuning == null ) ) {
        log.ErrorFormat( "UpdateOptionItem - Tuning item for device not found - dev: {0} - option: {1}", dev.DevName, optionName );
        return; // ERROR EXIT
      }

      if ( dev != null ) {
        // having a device and a tuning item here
        // JS commands that are supported
        if ( nodeText.ToLowerInvariant( ).EndsWith( "_x" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotx" ) || nodeText.ToLowerInvariant( ).EndsWith( "_throttlex" )
          || nodeText.ToLowerInvariant( ).EndsWith( "_y" ) || nodeText.ToLowerInvariant( ).EndsWith( "_roty" ) || nodeText.ToLowerInvariant( ).EndsWith( "_throttley" )
          || nodeText.ToLowerInvariant( ).EndsWith( "_Z" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotz" ) || nodeText.ToLowerInvariant( ).EndsWith( "_throttlez" )
          || nodeText.ToLowerInvariant( ).EndsWith( "_slider1" ) || nodeText.ToLowerInvariant( ).EndsWith( "_slider2" ) ) {
          // update dynamic properties
          string doID = Deviceoptions.DevOptionID( dev.DevClass, dev.DevName, nodeText );
          if ( m_AT.ActionMaps.DeviceOptions.ContainsKey( doID ) ) {
            tuning.AssignDynamicItems( dev, m_AT.ActionMaps.DeviceOptions[doID], nodeText );
          }
          else {
            tuning.AssignDynamicItems( dev, null, nodeText );
          }
        }
        // GP commands that are supported
        else if ( nodeText.ToLowerInvariant( ).Contains( "_thumblx" ) || nodeText.ToLowerInvariant( ).Contains( "_thumbrx" )
               || nodeText.ToLowerInvariant( ).Contains( "_thumbly" ) || nodeText.ToLowerInvariant( ).Contains( "_thumbry" ) ) {
          // update dynamic properties
          tuning.GameDevice = dev;
          tuning.NodeText = nodeText;
          string doID = Deviceoptions.DevOptionID( dev.DevClass, dev.DevName, nodeText );
          if ( m_AT.ActionMaps.DeviceOptions.ContainsKey( doID ) ) {
            tuning.AssignDynamicItems( dev, m_AT.ActionMaps.DeviceOptions[doID], nodeText );
          }
          else {
            tuning.AssignDynamicItems( dev, null, nodeText );
          }
        }
      }
      else if ( tuning != null && tuning.DevInstanceNo > 0 ) {
        // a device was assigned but the action is not mapped
        // try to find the gamedevice here ??
        if ( JoystickCls.IsDeviceClass( tuning.DeviceClass ) ) {
          tuning.AssignDynamicItems( DeviceInst.JoystickListRef.Find_jsN( tuning.DevInstanceNo ), null, "" );
        }
        else if ( GamepadCls.IsDeviceClass( tuning.DeviceClass ) ) {
          tuning.AssignDynamicItems( DeviceInst.GamepadRef, null, "" );
        }
      }

    }

    /// <summary>
    /// Get the assigned controls for some commands used in Tuning Yaw,Pitch,Roll and the Strafe ones
    /// Connect deviceOption if known
    /// </summary>
    private void UpdateTuningItems()
    {
      // cleanup - Actions will be assigned new in below calls
      m_AT.ActionMaps.DeviceOptions.ResetDynamicItems( );
      m_AT.ActionMaps.TuningOptions.ResetDynamicItems( );

      // get current mapping from ActionMaps
      UpdateOptionItem( "flight_move_pitch", "v_pitch", "spaceship_movement" );
      UpdateOptionItem( "flight_move_yaw", "v_yaw", "spaceship_movement" );
      UpdateOptionItem( "flight_move_roll", "v_roll", "spaceship_movement" );

      UpdateOptionItem( "flight_move_strafe_vertical", "v_strafe_vertical", "spaceship_movement" );
      UpdateOptionItem( "flight_move_strafe_lateral", "v_strafe_lateral", "spaceship_movement" );
      UpdateOptionItem( "flight_move_strafe_longitudinal", "v_strafe_longitudinal", "spaceship_movement" );
    }


    /// <summary>
    /// Get the assigned controls for other Options - if available...
    /// </summary>
    private void UpdateMoreOptionItems()
    {
      // get current mapping from ActionMaps
      UpdateOptionItem( "flight_throttle_abs", "v_throttle_abs", "spaceship_movement" );
      UpdateOptionItem( "flight_throttle_rel", "v_throttle_rel", "spaceship_movement" );

      UpdateOptionItem( "flight_aim_pitch", "v_aim_pitch", "spaceship_targeting" );
      UpdateOptionItem( "flight_aim_yaw", "v_aim_yaw", "spaceship_targeting" );

      UpdateOptionItem( "flight_view_pitch", "v_view_pitch", "spaceship_view" );
      UpdateOptionItem( "flight_view_yaw", "v_view_yaw", "spaceship_view" );

      UpdateOptionItem( "turret_aim_pitch", "v_aim_pitch", "spaceship_turret" );
      UpdateOptionItem( "turret_aim_yaw", "v_aim_yaw", "spaceship_turret" );
    }
    

    private void btJSTuning_Click( object sender, EventArgs e )
    {
      timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy

      JSCAL = new OGL.FormJSCalCurve( );

      // Have to attach here to capture the currently valid settings
      UpdateTuningItems( );
      // run
      JSCAL.TuningOptions = m_AT.ActionMaps.TuningOptions;
      JSCAL.ShowDialog( this );
      m_AT.Dirty = true;

      JSCAL = null; // get rid and create a new one next time..

      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      timer1.Enabled = true;
    }


    private void btOptions_Click( object sender, EventArgs e )
    {
      timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy

      FormOptions OPT = new FormOptions( );

      // Have to attach here to capture the currently valid settings
      UpdateTuningItems( );
      UpdateMoreOptionItems( );

      DeviceList devlist = new DeviceList( );
      if ( m_AppSettings.DetectGamepad && ( DeviceInst.GamepadRef != null ) ) {
        devlist.Add( DeviceInst.GamepadRef );
      }
      devlist.AddRange( DeviceInst.JoystickListRef );

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


    // Keyboard Input

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
          lblLastJ.Text = DeviceInst.KeyboardRef.GetLastChange( true );
          m_mouseIn = false; // clear on kbd input
        }
        // also maintain persistent mods
        UpdateModifiers( );
      }
      // don't spill the field with regular input
      e.SuppressKeyPress = true;
      e.Handled = true;
    }

    // text of input has changed
    private void lblLastJ_TextChanged( object sender, EventArgs e )
    {
      AutoTabXML_Assignment( EATabXML.Tab_Assignment );

      string devInput = ActionCls.DevInput( lblLastJ.Text, InputMode );
      RTF.RTFformatter RTF = new RTF.RTFformatter( );
      m_AT.FindAllActionsRTF( devInput, RTF );
      // have to check if throttle is used and if - add those to the list
      string altDevInput = JoystickCls.MakeThrottle( devInput, true );
      if ( altDevInput != devInput ) {
        m_AT.FindAllActionsRTF( altDevInput, RTF );
      }
      lbxOther.Rtf = RTF.RTFtext;
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

    // Mouse Input

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

      int btNum = 0;
      if ( int.TryParse( (string)ts.Tag, out btNum ) ) {
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
      }

      lblLastJ.Text = ctrl;
    }

    #endregion


    // Called when the table must be rebuild
    private void UpdateTable()
    {
      // only if needed
      if ( ( FTAB != null ) && FTAB.Visible ) {
        FTAB.SuspendDGV( );
        m_AT.ActionMaps.toDataSet( FTAB.DS_AMaps );
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
        m_AT.ActionMaps.updateDataSet( FTAB.DS_AMaps, actionID );
        // FTAB.UpdateRow( actionID );  seems not needed...
      }
    }

    // Show the Table Window
    private void btTable_Click( object sender, EventArgs e )
    {
      bool created = false;
      if ( FTAB == null ) {
        FTAB = new FormTable( );
        FTAB.EditActionEvent += FTAB_EditActionEvent;
        FTAB.UpdateEditEvent += FTAB_UpdateEditEvent;
        created = true;
      }

      if ( FTAB.Visible ) {
        m_AppSettings.FormTableSize = FTAB.LastSize;
        m_AppSettings.FormTableLocation = FTAB.LastLocation;
        m_AppSettings.FormTableColumnWidth = FTAB.LastColSize;
        FTAB.Hide( );

      }
      else {
        FTAB.Show( );

        if ( created ) {
          FTAB.Size = m_AppSettings.FormTableSize;
          FTAB.Location = m_AppSettings.FormTableLocation;
          FTAB.LastColSize = m_AppSettings.FormTableColumnWidth;
        }
        // reload the data to display
        UpdateTable( );
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

  }
}
