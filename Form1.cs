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


namespace SCJMapper_V2
{
  public partial class MainForm : Form
  {
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod( ).DeclaringType );

    private const String c_GithubLink = @"https://github.com/SCToolsfactory/SCJMapper-V2/releases";

    private AppSettings m_AppSettings = new AppSettings( );

    ///<remarks>
    /// Holds the DXInput Joystick List
    ///</remarks>
    private JoystickList m_Joystick = new JoystickList( );

    ///<remarks>
    /// Holds the DXInput keyboard
    ///</remarks>
    private GamepadCls m_Gamepad = null;

    ///<remarks>
    /// Holds the DXInput keyboard
    ///</remarks>
    private KeyboardCls m_Keyboard = null;


    ///<remarks>
    /// Holds the ActionTree that manages the TreeView and the action lists
    ///</remarks>
    private ActionTree m_AT = null;

    private FormJSCalCurve JSCAL = null;


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
    private Boolean IsGamepadTab( TabPage page )
    {
      // catch if the Tag is not an int...
      try {
        return ( ( int )page.Tag == ID_GAMEPAD_TAB );
      }
      catch {
        return false;
      }
    }

    /// <summary>
    /// Detects and returns the current Input device
    /// </summary>
    private DeviceCls.InputKind InputMode
    {
      get
      {
        if ( m_keyIn ) {
          return DeviceCls.InputKind.Kbd;
        }
        else {
          if ( IsGamepadTab( tc1.SelectedTab ) ) {
            return DeviceCls.InputKind.Gamepad;
          }
          else {
            return DeviceCls.InputKind.Joystick;
          }
        }
      }
    }


    /// <summary>
    /// Get the current JsN String for the active device tab
    /// </summary>
    /// <returns>The jsN string - can be jsx, js1..jsN</returns>
    private String JSStr( )
    {
      UC_JoyPanel jp = ( UC_JoyPanel )( tc1.SelectedTab.Controls["UC_JoyPanel"] );
      return jp.JsName;
    }

    #endregion


    #region Main Form Handling


    public MainForm( )
    {

      try {
        // Load the icon from our resources
        System.Resources.ResourceManager resources = new System.Resources.ResourceManager( this.GetType( ) );
        this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
      }
      catch {
        ; // well...
      }

      InitializeComponent( );

    }

    private void MainForm_Deactivate( object sender, EventArgs e )
    {
      timer1.Enabled = false;
      m_Joystick.Deactivate( );
      m_Keyboard.Deactivate( );
    }

    private void MainForm_Activated( object sender, EventArgs e )
    {
      timer1.Enabled = true;
      m_Joystick.Activate( );
      m_Keyboard.Activate( );
    }


    private void LoadProfileDD( )
    {
      tsDDbtProfiles.DropDownItems.Clear( );
      foreach ( String s in SCDefaultProfile.DefaultProfileNames ) {
        tsDDbtProfiles.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ) );
      }
    }

    private void LoadMappingDD( )
    {
      SCMappings.UpdateMappingNames( );
      tsDDbtMappings.DropDownItems.Clear( );
      foreach ( String s in SCMappings.MappingNames ) {
        tsDDbtMappings.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ) );
      }
    }

    /// <summary>
    /// Indicates if the SC directory is a valid one
    /// </summary>
    private void SCFileIndication( )
    {
      if ( String.IsNullOrEmpty( SCPath.SCGameData_pak ) ) tsDDbtProfiles.BackColor = MyColors.InvalidColor;
      else tsDDbtProfiles.BackColor = MyColors.ProfileColor;

      if ( String.IsNullOrEmpty( SCPath.SCClientMappingPath ) ) tsDDbtMappings.BackColor = MyColors.InvalidColor;
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

      String version = Application.ProductVersion;  // get the version information
      lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // get the first two elements
      log.InfoFormat( "Application Version: {0}", version.ToString( ) );

      // tooltips where needed
      toolTip1.SetToolTip( this.linkLblReleases, c_GithubLink ); // allow to see where the link may head

      // XML RTB
      log.Debug( "Loading RTB" );
      rtb.SelectionTabs = new int[] { 10, 20, 30, 40, 50, 60 }; // short tabs
      rtb.DragEnter += new DragEventHandler( rtb_DragEnter );
      rtb.DragDrop += new DragEventHandler( rtb_DragDrop );
      rtb.AllowDrop = true; // add Drop to rtb

      // load profiles
      log.Debug( "Loading Profiles" );
      LoadProfileDD( );
      tsDDbtProfiles.Text = m_AppSettings.DefProfileName;

      // load mappings
      log.Debug( "Loading Mappings" );
      LoadMappingDD( );
      tsDDbtMappings.Text = m_AppSettings.DefMappingName;

      SCFileIndication( );

      // load other defaults
      log.Debug( "Loading Other" );
      txMappingName.Text = m_AppSettings.MyMappingName;
      SetRebindField( txMappingName.Text );

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
        m_AppSettings.MyMappingName = txMappingName.Text; // last used - persist
        txMappingName.BackColor = MyColors.SuccessColor;
      }
      else {
        log.WarnFormat( "Last used mapping not available ({0})", txMappingName.Text );
        txMappingName.BackColor = MyColors.ErrorColor;
      }

      // poll the XInput
      log.Debug( "Start XInput polling" );
      timer1.Start( ); // this one polls the joysticks to show the props
    }


    /// <summary>
    /// Handles the Exit button
    /// </summary>
    private void buttonExit_Click( object sender, System.EventArgs e )
    {
      log.Debug( "Shutting down now..." );
      Close( );
    }


    /// <summary>
    /// Fancy tab coloring with ownerdraw to paint the callout buttons
    /// </summary>
    private void tc1_DrawItem( object sender, System.Windows.Forms.DrawItemEventArgs e )
    {
      try {
        //This line of code will help you to change the apperance like size,name,style.
        Font f;
        //For background color
        Brush backBrush = new System.Drawing.SolidBrush( MyColors.TabColor[e.Index] );
        //For forground color
        Brush foreBrush = new SolidBrush( System.Drawing.Color.Black );


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
    private void InitActionTree( Boolean addDefaultBinding )
    {
      log.Debug( "InitActionTree - Entry" );

      // build TreeView and the ActionMaps
      m_AT = new ActionTree( m_AppSettings.BlendUnmapped, m_AppSettings.BlendUnmappedGP, m_Joystick );
      m_AT.Ctrl = treeView1;  // the ActionTree owns the TreeView control
      m_AT.IgnoreMaps = m_AppSettings.IgnoreActionmaps;
      m_AT.LoadTree( m_AppSettings.DefProfileName, addDefaultBinding );       // Init with default profile filepath

      // apply a default JS to Joystick mapping - can be changed and reloaded from XML mappings
      // must take care of Gamepads if there are (but we take care of one only...)

      int joyStickIndex = 0; // Joystick List Index
      for ( int deviceTabIndex=0; deviceTabIndex < JoystickCls.JSnum_MAX; deviceTabIndex++ ) {
        if ( tc1.TabPages.Count > deviceTabIndex ) {
          // valid Device Tab
          if ( IsGamepadTab( tc1.TabPages[deviceTabIndex] ) ) {
            ; // ignore gamepads
          }
          else if ( m_Joystick.Count > joyStickIndex ) {
            // there is a joystick device left..
            m_Joystick[joyStickIndex].JSAssignment = joyStickIndex + 1; // assign number 1..
            m_AT.ActionMaps.jsN[deviceTabIndex] = m_Joystick[joyStickIndex].DevName;
            m_AT.ActionMaps.jsNGUID[deviceTabIndex] = m_Joystick[joyStickIndex].DevInstanceGUID;
            joyStickIndex++;
          }
        }
      }
    }


    /// <summary>
    /// Aquire the DInput joystick devices
    /// </summary>
    /// <returns></returns>
    public bool InitDirectInput( )
    {
      log.Debug( "Entry" );

      // Enumerate joysticks in the system.
      int tabs = 0;
      SharpDX.XInput.UserIndex gpDeviceIndex = SharpDX.XInput.UserIndex.Any;

      try {
        // Initialize DirectInput
        log.Debug( "Instantiate DirectInput" );
        var directInput = new DirectInput( );

        log.Debug( "Get Keyboard device" );
        m_Keyboard = new KeyboardCls( new Keyboard( directInput ), this );

        // scan the Input for attached devices
        log.Debug( "Scan GameControl devices" );
        int nJs = 1; // number the Joystick Tabs
        foreach ( DeviceInstance instance in directInput.GetDevices( DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly ) ) {

          log.InfoFormat( "GameControl: #{0} Type:{1} Device:{2}", tabs, instance.Type.ToString( ), instance.ProductName );
          // Create the device interface
          log.Debug( "Create the device interface" );
          SharpDX.DirectInput.Joystick jsDevice = null;
          SharpDX.XInput.Controller gpDevice = null;
          JoystickCls js = null; GamepadCls gs = null;
          if ( m_AppSettings.DetectGamepad && ( instance.Usage == SharpDX.Multimedia.UsageId.GenericGamepad ) ) {
            // detect Gamepad only if the user wishes to do so
            for ( SharpDX.XInput.UserIndex i =  SharpDX.XInput.UserIndex.One; i < SharpDX.XInput.UserIndex.Four; i++ ) {
              gpDevice = new SharpDX.XInput.Controller( i );
              if ( gpDevice.IsConnected ) {
                log.InfoFormat( "Scan Input {0} for gamepad - {1}", i, gpDevice.GetCapabilities( SharpDX.XInput.DeviceQueryType.Gamepad ).ToString( ) );
                gpDeviceIndex = i;
                break;
              }
            }
          }
          else {
            jsDevice = new Joystick( directInput, instance.InstanceGuid );
            log.DebugFormat( "Create the device interface for: {0}", jsDevice.Information.ProductName );
          }

          // we have the first tab made as reference so TabPage[0] already exists
          if ( tabs == 0 ) {
            // first panel - The Tab content exists already 
            if ( gpDevice != null ) {
              log.Debug( "Add first Gamepad panel" );
              tc1.TabPages[0].Text = "Gamepad ";
              UC_GpadPanel uUC_GpadPanelNew = new UC_GpadPanel( ); tc1.TabPages[0].Controls.Add( uUC_GpadPanelNew );
              uUC_GpadPanelNew.Size = UC_JoyPanel.Size; uUC_GpadPanelNew.Location = UC_JoyPanel.Location;
              UC_JoyPanel.Enabled = false; UC_JoyPanel.Visible = false; // don't use this one 
              log.Debug( "Create Gamepad instance" );
              gs = new GamepadCls( gpDevice, uUC_GpadPanelNew, 0 ); // does all device related activities for that particular item
              gs.SetDeviceName( instance.ProductName );
            }
            else {
              log.Debug( "Add first Joystick panel" );
              log.Debug( "Create Joystick instance" );
              tc1.TabPages[tabs].Text = String.Format( "Joystick {0}", nJs++ );
              js = new JoystickCls( jsDevice, this, tabs + 1, UC_JoyPanel, 0 ); // does all device related activities for that particular item
            }
          }
          else {
            if ( gpDevice != null ) {
              log.Debug( "Add next Gamepad panel" );
              tc1.TabPages.Add( "Gamepad " );
              UC_GpadPanel uUC_GpadPanelNew = new UC_GpadPanel( ); tc1.TabPages[tabs].Controls.Add( uUC_GpadPanelNew );
              uUC_GpadPanelNew.Size = UC_JoyPanel.Size; uUC_GpadPanelNew.Location = UC_JoyPanel.Location;
              UC_JoyPanel.Enabled = false; UC_JoyPanel.Visible = false; // don't use this one 
              log.Debug( "Create Gamepad instance" );
              gs = new GamepadCls( gpDevice, uUC_GpadPanelNew, tabs ); // does all device related activities for that particular item
              gs.SetDeviceName( instance.ProductName );
            }
            else {
              log.Debug( "Add next Joystick panel" );
              // setup the further tab contents along the reference one in TabPage[0] (the control named UC_JoyPanel)
              tc1.TabPages.Add( String.Format( "Joystick {0}", nJs++ ) );
              UC_JoyPanel uUC_JoyPanelNew = new UC_JoyPanel( ); tc1.TabPages[tabs].Controls.Add( uUC_JoyPanelNew );
              uUC_JoyPanelNew.Size = UC_JoyPanel.Size; uUC_JoyPanelNew.Location = UC_JoyPanel.Location;
              log.Debug( "Create Joystick instance" );
              js = new JoystickCls( jsDevice, this, tabs + 1, uUC_JoyPanelNew, tabs ); // does all device related activities for that particular item
            }
          }

          if ( gpDevice != null ) {
            m_Gamepad = gs;
            SetGamepadTab( tc1.TabPages[tabs] );  // indicates the gamepad tab (murks..)
            MyColors.GamepadColor = MyColors.TabColor[tabs]; // save it for future use
          }
          else if ( js != null ) {
            m_Joystick.Add( js ); // add to joystick list
            tc1.TabPages[tabs].Tag = ( m_Joystick.Count - 1 );  // used to find the tab for polling
          }
          tc1.TabPages[tabs].BackColor = MyColors.TabColor[tabs]; // each tab has its own color

          // next tab
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

      }
      catch ( Exception ex ) {
        log.Debug( "InitDirectInput failed unexpectedly", ex );
        return false;
      }

      return true;
    }

    #endregion




    /// <summary>
    ///  Grab the rtb data and load them into config
    /// </summary>
    private void Grab( )
    {
      log.Debug( "Grab - Entry" );

      m_Joystick.ResetJsNAssignment( );
      m_AT.ActionMaps.fromXML( rtb.Text );
      // JS mapping for js1 .. js8 can be changed and reloaded from XML
      // note - unmapped ones remain what they were
      // This is includes similar procedures as reassigning of the jsN items
      JoystickCls j = null;

      m_Joystick.ClearJsNAssignment( );
      // for all supported jsN
      for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
        j = null;
        if ( !String.IsNullOrEmpty( m_AT.ActionMaps.jsNGUID[i] ) ) j = m_Joystick.Find_jsInstance( m_AT.ActionMaps.jsNGUID[i] );
        else if ( !String.IsNullOrEmpty( m_AT.ActionMaps.jsN[i] ) ) j = m_Joystick.Find_jsDev( m_AT.ActionMaps.jsN[i] );

        if ( j != null ) {
          m_AT.ActionMaps.jsNGUID[i] = j.DevInstanceGUID; // subst for missing one (version up etc.)
          j.JSAssignment = i + 1; // i is 0 based ; jsN is 1 based
        }
        else {
          // a valid but unknown GUID

          m_AT.ActionMaps.Clear_jsEntry( i );
        }
      }


      // maintain the new JsN assignment and update the colorlist
      List<int> newL = new List<int>( );
      foreach ( TabPage tp in tc1.TabPages ) {
        if ( IsGamepadTab( tp ) ) newL.Add( 0 );
        else newL.Add( m_Joystick[( int )tp.Tag].JSAssignment );
      }
      JoystickCls.ReassignJsColor( newL );

      m_AT.ReloadTreeView( ); // finally reload things into the tree

      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
    }


    /// <summary>
    /// Dump Config into rtb
    /// </summary>
    private void Dump( )
    {
      log.Debug( "Dump - Entry" );

      rtb.Text = String.Format( "<!-- {0} - SC Joystick Mapping -->\n{1}", DateTime.Now, m_AT.ActionMaps.toXML( ) );

      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
    }


    private void SetRebindField( String map )
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
      m_AppSettings.Save( );
    }


    private void timer1_Tick( object sender, System.EventArgs e )
    {
      if ( m_keyIn ) return; // allow keyboard / mouse input

      String ctrl = "";
      int jsIndex = ( int )tc1.SelectedTab.Tag; // gets the index into the JS list
      if ( jsIndex < 0 ) {
        m_Gamepad.GetData( );
        ctrl = m_Gamepad.GetLastChange( );
        timer1.Interval = 750; // allow more time to release buttons
      }
      else {
        m_Joystick[jsIndex].GetData( );  // poll the device
        ctrl = JSStr( ) + m_Joystick[jsIndex].GetLastChange( ); // show last handled JS control
        timer1.Interval = 100; // standard polling
      }

      lblLastJ.Text = ctrl;
      if ( JoystickCls.CanThrottle( ctrl ) ) {
        cbxThrottle.Enabled = true;
        cbxInvert.Enabled = true;
      }
      else if ( GamepadCls.CanInvert( ctrl ) ) {
        cbxInvert.Enabled = true;
      }
      else {
        cbxThrottle.Checked = false; cbxThrottle.Enabled = false;
        cbxInvert.Checked = false; cbxInvert.Enabled = false;
      }
    }


    // TreeView Events

    private void treeView1_AfterSelect( object sender, TreeViewEventArgs e )
    {
      if ( e.Node.Level == 1 ) {
        // actions cannot have a blank - if there is one it's mapped
        if ( e.Node.Text.IndexOf( " ", 0 ) > 0 ) {
          lblAction.Text = e.Node.Text.Substring( 0, e.Node.Text.IndexOf( " ", 0 ) ); // get only the action part as Cmd.
        }
        else {
          lblAction.Text = e.Node.Text;
        }
      }
    }


    // Show options

    private void cbxShowTreeOptions_CheckedChanged( object sender, EventArgs e )
    {
      m_AT.DefineShowOptions( cbxShowJoystick.Checked, cbxShowGamepad.Checked, cbxShowKeyboard.Checked, cbxShowMappedOnly.Checked );
      m_AT.ReloadTreeView( );
    }



    // Assign Panel Items

    private void btFind_Click( object sender, EventArgs e )
    {
      m_AT.FindAndSelectCtrl( JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked ) ); // find the action for a Control (joystick input)
    }

    private void btAssign_Click( object sender, EventArgs e )
    {
      m_AT.UpdateSelectedItem( JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked ), cbxInvert.Checked, InputMode );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }


    // General Area Items

    private void btClear_Click( object sender, EventArgs e )
    {

      m_AT.UpdateSelectedItem( "", false, InputMode );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }

    private void btDump_Click( object sender, EventArgs e )
    {
      Dump( );
    }

    private void btDumpList_Click( object sender, EventArgs e )
    {
      rtb.Text = String.Format( "-- {0} - SC Joystick Mapping --\n{1}", DateTime.Now, m_AT.ReportActions( ) );
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


    // Toolstrip Items

    private void tsBtReset_ButtonClick( object sender, EventArgs e )
    {
    }

    private void tsDDbtProfiles_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      tsDDbtProfiles.Text = e.ClickedItem.Text;
      m_AppSettings.DefProfileName = e.ClickedItem.Text; m_AppSettings.Save( );
      // InitActionTree( ( Settings.Default.ResetMode == Settings.Default.ResetModeDefault ) ); // start over
    }

    private void resetEmptyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // start over 
      InitActionTree( false );
      rtb.Text = "";
    }

    private void resetDefaultsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // start over and if chosen, load defaults from SC game
      InitActionTree( true );
      rtb.Text = "";
    }

    private void tsDDbtMappings_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      tsDDbtMappings.Text = e.ClickedItem.Text;
      m_AppSettings.DefMappingName = e.ClickedItem.Text; m_AppSettings.Save( );
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
    private void btLoadMyMapping_Click( object sender, EventArgs e )
    {
      if ( SCMappings.MappingFileExists( txMappingName.Text ) ) {
        rtb.LoadFile( SCMappings.MappingFileName( txMappingName.Text ), RichTextBoxStreamType.PlainText );
        InitActionTree( false );
        Grab( );
        m_AppSettings.MyMappingName = txMappingName.Text; // last used - persist
        txMappingName.BackColor = MyColors.SuccessColor;
      }
      else {
        txMappingName.BackColor = MyColors.ErrorColor;
      }
    }

    private void btSaveMyMapping_Click( object sender, EventArgs e )
    {
      Boolean cancel = false;
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
          m_AppSettings.MyMappingName = txMappingName.Text; // last used - persist
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
      System.Windows.Forms.Clipboard.SetText( txRebind.Text );
    }


    // Settings

    private void btSettings_Click( object sender, EventArgs e )
    {
      // have to stop polling while the Settings window is open
      timer1.Enabled = false;
      if ( m_AppSettings.ShowSettings( ) != System.Windows.Forms.DialogResult.Cancel ) {
        m_AppSettings.Reload( ); // must reload in case of any changes in the form
        // then reload the profile and mappings
        LoadProfileDD( );
        LoadMappingDD( );
        // indicates (in)valid folders
        SCFileIndication( );
        // now update the contents according to new settings
        foreach ( JoystickCls j in m_Joystick ) j.ApplySettings( ); // update Seetings
        m_AT.IgnoreMaps = m_AppSettings.IgnoreActionmaps;
        // and start over with an empty tree
        InitActionTree( false );
      }

      timer1.Enabled = true;
    }

    private void btJsReassign_Click( object sender, EventArgs e )
    {
      // have to stop polling while the Reassign window is open
      timer1.Enabled = false;
      if ( m_Joystick.ShowReassign( ) != System.Windows.Forms.DialogResult.Cancel ) {
        // copy the action tree while reassigning the jsN mappings from OLD to NEW
        ActionTree newTree = m_AT.ReassignJsN( m_Joystick.JsReassingList );

        // we have still the old assignment in the ActionMap - change it here (map does not know about the devices)
        JoystickCls j = null;
        // for all supported jsN devices
        for ( int i=0; i < JoystickCls.JSnum_MAX; i++ ) {
          j = m_Joystick.Find_jsN( i + 1 );
          if ( j != null ) {
            newTree.ActionMaps.jsN[i] = j.DevName; newTree.ActionMaps.jsNGUID[i] = j.DevInstanceGUID;
          }
          else {
            newTree.ActionMaps.jsN[i] = ""; newTree.ActionMaps.jsNGUID[i] = "";
          }
        }

        m_AT = newTree; // make it the valid one
        m_AT.ReloadTreeView( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }

      timer1.Enabled = true;
    }


    // Blend

    private void btBlend_Click( object sender, EventArgs e )
    {
      m_AT.UpdateSelectedItem( DeviceCls.BlendedInput, false, InputMode );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }

    // Joystick Tuning

    private void btJSTuning_Click( object sender, EventArgs e )
    {
      timer1.Enabled = false; // must be off while a modal window is shown, else DX gets crazy

      JSCAL = new FormJSCalCurve( );
      // get current mapping from ActionMaps
      String nodeText = "";

      // attach Yaw command
      DeviceCls dev =  null;
      String find = "";

      find = ActionTreeNode.ComposeNodeText( "v_yaw", ActionTreeNode.REG_MOD, "js" );
      nodeText = m_AT.FindText( "spaceship_movement", find ); // returns "" or a complete text ("action - command")
      if ( String.IsNullOrWhiteSpace( nodeText ) ) {
        find = ActionTreeNode.ComposeNodeText( "v_yaw", ActionTreeNode.INV_MOD, "js" );
        nodeText = m_AT.FindText( "spaceship_movement", find ); // find inverted ones too
      }
      if ( !String.IsNullOrWhiteSpace( nodeText ) ) {
        dev = m_Joystick.Find_jsN( JoystickCls.JSNum( ActionTreeNode.CommandFromNodeText( nodeText ) ) );
      }
      else {
        find = ActionTreeNode.ComposeNodeText( "v_yaw", ActionTreeNode.REG_MOD, "xi" );
        nodeText = m_AT.FindText( "spaceship_movement", find );
        if ( String.IsNullOrWhiteSpace( nodeText ) ) {
          find = ActionTreeNode.ComposeNodeText( "v_yaw", ActionTreeNode.INV_MOD, "xi" );
          nodeText = m_AT.FindText( "spaceship_movement", find ); // find inverted ones too
        }
        if ( !String.IsNullOrWhiteSpace( nodeText ) ) {
          dev = m_Gamepad;
        }
      }

      if ( dev != null ) {
        // JS commands that are supported
        if ( nodeText.ToLowerInvariant( ).EndsWith( "_x" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotx" ) ) {
          m_AT.ActionMaps.TuningX.GameDevice = dev;
          m_AT.ActionMaps.TuningX.ActionCommand = nodeText;
          JSCAL.YawTuning = m_AT.ActionMaps.TuningX;
        }
        else if ( nodeText.ToLowerInvariant( ).EndsWith( "_y" ) || nodeText.ToLowerInvariant( ).EndsWith( "_roty" ) ) {
          m_AT.ActionMaps.TuningY.GameDevice = dev;
          m_AT.ActionMaps.TuningY.ActionCommand = nodeText;
          JSCAL.YawTuning = m_AT.ActionMaps.TuningY;
        }
        else if ( nodeText.ToLowerInvariant( ).EndsWith( "_z" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotz" ) ) {
          m_AT.ActionMaps.TuningZ.GameDevice = dev;
          m_AT.ActionMaps.TuningZ.ActionCommand = nodeText;
          JSCAL.YawTuning = m_AT.ActionMaps.TuningZ;
        }
        // GP commands that are supported - X
        else if ( nodeText.ToLowerInvariant( ).Contains( "_thumblx" ) || nodeText.ToLowerInvariant( ).Contains( "_thumbrx" ) ) {
          m_AT.ActionMaps.TuningX.GameDevice = dev;
          m_AT.ActionMaps.TuningX.ActionCommand = nodeText;
          JSCAL.YawTuning = m_AT.ActionMaps.TuningX;
        }
      }

      // attach Pitch command
      dev = null;
      find = ActionTreeNode.ComposeNodeText( "v_pitch", ActionTreeNode.REG_MOD, "js" );
      nodeText = m_AT.FindText( "spaceship_movement", find ); // returns "" or a complete text ("action - command")
      if ( String.IsNullOrWhiteSpace( nodeText ) ) {
        find = ActionTreeNode.ComposeNodeText( "v_pitch", ActionTreeNode.INV_MOD, "js" );
        nodeText = m_AT.FindText( "spaceship_movement", find ); // find inverted ones too
      }
      if ( !String.IsNullOrWhiteSpace( nodeText ) ) {
        dev = m_Joystick.Find_jsN( JoystickCls.JSNum( ActionTreeNode.CommandFromNodeText( nodeText ) ) );
      }
      else {
        find = ActionTreeNode.ComposeNodeText( "v_pitch", ActionTreeNode.REG_MOD, "xi" );
        nodeText = m_AT.FindText( "spaceship_movement", find );
        if ( String.IsNullOrWhiteSpace( nodeText ) ) {
          find = ActionTreeNode.ComposeNodeText( "v_pitch", ActionTreeNode.INV_MOD, "xi" );
          nodeText = m_AT.FindText( "spaceship_movement", find ); // find inverted ones too
        }
        if ( !String.IsNullOrWhiteSpace( nodeText ) ) {
          dev = m_Gamepad;
        }
      }

      if ( dev != null ) {
        // JS commands that are supported
        if ( nodeText.ToLowerInvariant( ).EndsWith( "_x" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotx" ) ) {
          m_AT.ActionMaps.TuningX.GameDevice = dev;
          m_AT.ActionMaps.TuningX.ActionCommand = nodeText;
          JSCAL.PitchTuning = m_AT.ActionMaps.TuningX;
        }
        else if ( nodeText.ToLowerInvariant( ).EndsWith( "_y" ) || nodeText.ToLowerInvariant( ).EndsWith( "_roty" ) ) {
          m_AT.ActionMaps.TuningY.GameDevice = dev;
          m_AT.ActionMaps.TuningY.ActionCommand = nodeText;
          JSCAL.PitchTuning = m_AT.ActionMaps.TuningY;
        }
        else if ( nodeText.ToLowerInvariant( ).EndsWith( "_z" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotz" ) ) {
          m_AT.ActionMaps.TuningZ.GameDevice = dev;
          m_AT.ActionMaps.TuningZ.ActionCommand = nodeText;
          JSCAL.PitchTuning = m_AT.ActionMaps.TuningZ;
        }
        // GP commands that are supported - either Y
        else if ( nodeText.ToLowerInvariant( ).Contains( "_thumbly" ) || nodeText.ToLowerInvariant( ).Contains( "_thumbry" ) ) {
          m_AT.ActionMaps.TuningY.GameDevice = dev;
          m_AT.ActionMaps.TuningY.ActionCommand = nodeText;
          JSCAL.PitchTuning = m_AT.ActionMaps.TuningY;
        }
      }

      // attach Roll command - cannot use gamepad here
      dev = null;
      find = ActionTreeNode.ComposeNodeText( "v_roll", ActionTreeNode.REG_MOD, "js" );
      nodeText = m_AT.FindText( "spaceship_movement", find ); // returns "" or a complete text ("action - command")
      if ( String.IsNullOrWhiteSpace( nodeText ) ) {
        find = ActionTreeNode.ComposeNodeText( "v_roll", ActionTreeNode.INV_MOD, "js" );
        nodeText = m_AT.FindText( "spaceship_movement", find ); // find inverted ones too
      }
      if ( !String.IsNullOrWhiteSpace( nodeText ) ) {
        dev = m_Joystick.Find_jsN( JoystickCls.JSNum( ActionTreeNode.CommandFromNodeText( nodeText ) ) );
      }

      if ( dev != null ) {
        // JS commands that are supported
        if ( nodeText.ToLowerInvariant( ).EndsWith( "_x" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotx" ) ) {
          m_AT.ActionMaps.TuningX.GameDevice = dev;
          m_AT.ActionMaps.TuningX.ActionCommand = nodeText;
          JSCAL.RollTuning = m_AT.ActionMaps.TuningX;
        }
        else if ( nodeText.ToLowerInvariant( ).EndsWith( "_y" ) || nodeText.ToLowerInvariant( ).EndsWith( "_roty" ) ) {
          m_AT.ActionMaps.TuningY.GameDevice = dev;
          m_AT.ActionMaps.TuningY.ActionCommand = nodeText;
          JSCAL.RollTuning = m_AT.ActionMaps.TuningY;
        }
        else if ( nodeText.ToLowerInvariant( ).EndsWith( "_z" ) || nodeText.ToLowerInvariant( ).EndsWith( "_rotz" ) ) {
          m_AT.ActionMaps.TuningZ.GameDevice = dev;
          m_AT.ActionMaps.TuningZ.ActionCommand = nodeText;
          JSCAL.RollTuning = m_AT.ActionMaps.TuningZ;
        }
      }

      // run
      JSCAL.ShowDialog( );
      m_AT.Dirty = true;

      // get from dialog
      JSCAL = null; // get rid and create a new one next time..

      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      timer1.Enabled = true;
    }


    // Keyboard Input

    Boolean m_keyIn = false;

    // Right no a double click triggers the switch between JS and Mouse+Kbd
    private void lblLastJ_MouseDoubleClick( object sender, MouseEventArgs e )
    {
      m_keyIn = ( !m_keyIn );
      if ( m_keyIn ) {
        if ( m_Keyboard == null ) {
          m_keyIn = false;
          return;
        } // bail out ..

        lblLastJ.BackColor = MyColors.KeyboardColor;
        m_Keyboard.Activate( );
        m_Keyboard.GetData( ); // poll to aquire once
      }
      else {
        lblLastJ.BackColor = MyColors.ValidColor;
        m_Keyboard.Deactivate( );
      }

    }

    private void btJsKbd_Click( object sender, EventArgs e )
    {
      m_keyIn = ( !m_keyIn );
      if ( m_keyIn ) {
        if ( m_Keyboard == null ) {
          m_keyIn = false;
          btJsKbd.ImageKey = "J";
          return;
        } // bail out ..

        lblLastJ.BackColor = MyColors.KeyboardColor;
        btJsKbd.ImageKey = "K";
        lblLastJ.Focus( );
        m_Keyboard.Activate( );
        m_Keyboard.GetData( ); // poll to aquire once
      }
      else {
        lblLastJ.BackColor = MyColors.ValidColor;
        btJsKbd.ImageKey = "J";
        m_Keyboard.Deactivate( );
      }
    }

    // read mouse commands (TODO only buttons no movement so far)
    private void lblLastJ_MouseClick( object sender, MouseEventArgs e )
    {
      if ( !m_keyIn ) return;
      // capture mouse things
      lblLastJ.Text = MouseCls.MouseCmd( e );
    }

    // Key down triggers the readout via DX Input
    private void lblLastJ_KeyDown( object sender, KeyEventArgs e )
    {
      if ( m_keyIn ) {
        m_Keyboard.GetData( );
        lblLastJ.Text = m_Keyboard.GetLastChange( );
      }
      // don't spill the field with regular input
      e.SuppressKeyPress = true;
      e.Handled = true;
    }



    #endregion





  }
}
