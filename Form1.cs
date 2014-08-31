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
    private JoystickList m_JS = new JoystickList( );

    ///<remarks>
    /// Holds the ActionTree that manages the TreeView and the action lists
    ///</remarks>
    private ActionTree m_AT = null;


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
      cbxBlendUnmapped.Checked = m_AppSettings.BlendUnmapped;

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
        Brush backBrush = new System.Drawing.SolidBrush( MyColors.JColor[e.Index] );
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

    /// <summary>
    /// Resets the Action Tree
    /// </summary>
    private void InitActionTree( Boolean addDefaultBinding )
    {
      log.Debug( "InitActionTree - Entry" );

      // build TreeView and the ActionMaps
      m_AT = new ActionTree( cbxBlendUnmapped.Checked );
      m_AT.Ctrl = treeView1;  // the ActionTree owns the TreeView control
      m_AT.IgnoreMaps = m_AppSettings.IgnoreActionmaps;
      m_AT.LoadTree( m_AppSettings.DefProfileName, addDefaultBinding );       // Init with default profile filepath

      // default JS to Joystick mapping - can be changed and reloaded from XML
      if ( m_JS.Count > 0 ) { m_JS[0].JSAssignment = 1; m_AT.ActionMaps.js1 = m_JS[0].DevName; m_AT.ActionMaps.js1GUID = m_JS[0].DevInstanceGUID; }
      if ( m_JS.Count > 1 ) { m_JS[1].JSAssignment = 2; m_AT.ActionMaps.js2 = m_JS[1].DevName; m_AT.ActionMaps.js2GUID = m_JS[1].DevInstanceGUID; }
      if ( m_JS.Count > 2 ) { m_JS[2].JSAssignment = 0; } // unmapped ones go with default 0
      if ( m_JS.Count > 3 ) { m_JS[3].JSAssignment = 0; }
      if ( m_JS.Count > 4 ) { m_JS[4].JSAssignment = 0; }
      if ( m_JS.Count > 5 ) { m_JS[5].JSAssignment = 0; }
      if ( m_JS.Count > 6 ) { m_JS[6].JSAssignment = 0; }
      if ( m_JS.Count > 7 ) { m_JS[7].JSAssignment = 0; }
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

      try {
        // Initialize DirectInput
        log.Debug( "Instantiate DirectInput" );
        var directInput = new DirectInput( );

        // scan the Input for attached devices
        log.Debug( "Scan GameControl devices" );
        foreach ( DeviceInstance instance in directInput.GetDevices( DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly ) ) {

          // Create the device interface
          log.Debug( "Create the device interface" );
          SharpDX.DirectInput.Joystick jsDevice = new Joystick( directInput, instance.InstanceGuid );
          JoystickCls js = null;
          log.DebugFormat( "Create the device interface for: {0}", jsDevice.Information.ProductName );

          // we have the first tab made as reference so TabPage[0] already exists
          if ( tabs == 0 ) {
            // first panel - The Tab content exists already 
            log.Debug( "Add first Joystick panel" );
            js = new JoystickCls( jsDevice, this, tabs + 1, UC_JoyPanel, tc1.TabPages[0] ); // does all device related activities for that particular item
          }
          else {
            log.Debug( "Add next Joystick panel" );
            // setup the further tab contents along the reference one in TabPage[0] (the control named UC_JoyPanel)
            tc1.TabPages.Add( "Joystick " + ( tabs + 1 ).ToString( ) );
            UC_JoyPanel uUC_JoyPanelNew = new UC_JoyPanel( );
            tc1.TabPages[tabs].Controls.Add( uUC_JoyPanelNew );
            uUC_JoyPanelNew.Size = UC_JoyPanel.Size;
            uUC_JoyPanelNew.Location = UC_JoyPanel.Location;
            log.Debug( "Create Joystick instance" );
            js = new JoystickCls( jsDevice, this, tabs + 1, uUC_JoyPanelNew, tc1.TabPages[tabs] ); // does all device related activities for that particular item
          }
          m_JS.Add( js ); // add to joystick list

          tc1.TabPages[tabs].Tag = js.DevName;  // used to find the tab via JS mapping
          tc1.TabPages[tabs].BackColor = MyColors.JColor[tabs]; // each tab has its own color

          // next tab
          tabs++;
          if ( tabs == 8 ) break; // cannot load more JSticks than predefined Tabs
        }
        log.DebugFormat( "Added {0} GameControl devices", tabs );

        /*
        // TEST CREATE ALL 8 TABS
        for ( int i=(tabs+1); i < 9; i++ ) {
          tc1.TabPages.Add( "Joystick " + i.ToString( ) );
        }
        */

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



    /// <summary>
    /// Get the current JsTag for the active device tab
    /// </summary>
    /// <returns></returns>
    private String JSStr( )
    {
      UC_JoyPanel jp = ( UC_JoyPanel )( tc1.SelectedTab.Controls["UC_JoyPanel"] );
      return jp.JsName;
    }


    /// <summary>
    ///  Grab the rtb data and load them into config
    /// </summary>
    private void Grab( )
    {
      log.Debug( "Grab - Entry" );

      m_JS.ResetJsNAssignment( );
      m_AT.ActionMaps.fromXML( rtb.Text );
      // JS mapping for js1 .. js4 can be changed and reloaded from XML
      // note - unmapped ones remain what they were
      // This is includes similar procedures as reassigning of the jsN items
      JoystickCls j = null;
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js1GUID ) ) {
        j = m_JS.Find_jsInstance( m_AT.ActionMaps.js1GUID );
      }
      else if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js1 ) ) {
        j = m_JS.Find_jsDev( m_AT.ActionMaps.js1 );
      }
      if ( j != null ) {
        m_AT.ActionMaps.js1GUID = j.DevInstanceGUID; // subst for missing one (version up etc.)
        j.JSAssignment = 1;
      }

      j = null; ;
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js2GUID ) ) {
        j = m_JS.Find_jsInstance( m_AT.ActionMaps.js2GUID );
      }
      else if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js2 ) ) {
        j = m_JS.Find_jsDev( m_AT.ActionMaps.js2 );
      }
      if ( j != null ) {
        m_AT.ActionMaps.js2GUID = j.DevInstanceGUID; // subst for missing one (version up etc.)
        j.JSAssignment = 2;
      }

      j = null; ;
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js3GUID ) ) {
        j = m_JS.Find_jsInstance( m_AT.ActionMaps.js3GUID );
      }
      else if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js3 ) ) {
        j = m_JS.Find_jsDev( m_AT.ActionMaps.js3 );
      }
      if ( j != null ) {
        m_AT.ActionMaps.js3GUID = j.DevInstanceGUID; // subst for missing one (version up etc.)
        j.JSAssignment = 3;
      }

      j = null; ;
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js4GUID ) ) {
        j = m_JS.Find_jsInstance( m_AT.ActionMaps.js4GUID );
      }
      else if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js4 ) ) {
        j = m_JS.Find_jsDev( m_AT.ActionMaps.js4 );
      }
      if ( j != null ) {
        m_AT.ActionMaps.js4GUID = j.DevInstanceGUID; // subst for missing one (version up etc.)
        j.JSAssignment = 4;
      }

      // maintain the new JsN assignment and update the colorlist
      List<int> newL = new List<int>( );
      foreach ( JoystickCls jj in m_JS ) {
        newL.Add(jj.JSAssignment);
      }
      JoystickCls.ReassignJsColor( newL );

      m_AT.ReloadCtrl( ); // finally reload things into the tree

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
      foreach ( JoystickCls jsc in m_JS ) { jsc.GetData( ); }  // poll the devices
      String ctrl =  JSStr( ) + m_JS[tc1.SelectedIndex].GetLastChange( ); // show last handled JS control
      lblLastJ.Text = ctrl;
      if ( JoystickCls.CanThrottle( ctrl ) ) {
        cbxThrottle.Enabled = true;
      }
      else {
        cbxThrottle.Checked = false;
        cbxThrottle.Enabled = false;
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

    // Assign Panel Items

    private void btFind_Click( object sender, EventArgs e )
    {
      m_AT.FindCtrl( JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked ) ); // find the action for a Control (joystick input)
    }

    private void btAssign_Click( object sender, EventArgs e )
    {
      m_AT.UpdateSelectedItem( JoystickCls.MakeThrottle( lblLastJ.Text, cbxThrottle.Checked ) );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }


    // General Area Items

    private void btClear_Click( object sender, EventArgs e )
    {

      m_AT.UpdateSelectedItem( "" );
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
        foreach ( JoystickCls j in m_JS ) j.ApplySettings( ); // update Seetings
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
      if ( m_JS.ShowReassign( ) != System.Windows.Forms.DialogResult.Cancel ) {
        // copy the action tree while reassigning the jsN mappings from OLD to NEW
        ActionTree newTree = m_AT.ReassignJsN( m_JS.JsReassingList );
        // we have still the old assignment in the ActionMap - change it here (map does not know about the devices)
        JoystickCls j = null;
        j = m_JS.Find_jsN( 1 );
        if ( j != null ) {
          newTree.ActionMaps.js1 = j.DevName; newTree.ActionMaps.js1GUID = j.DevInstanceGUID;
        }
        else {
          newTree.ActionMaps.js1 = ""; newTree.ActionMaps.js1GUID = "";
        }
        j = m_JS.Find_jsN( 2 );
        if ( j != null ) {
          newTree.ActionMaps.js2 = j.DevName; newTree.ActionMaps.js2GUID = j.DevInstanceGUID;
        }
        else {
          newTree.ActionMaps.js2 = ""; newTree.ActionMaps.js2GUID = "";
        }
        j = m_JS.Find_jsN( 3 );
        if ( j != null ) {
          newTree.ActionMaps.js3 = j.DevName; newTree.ActionMaps.js3GUID = j.DevInstanceGUID;
        }
        else {
          newTree.ActionMaps.js3 = ""; newTree.ActionMaps.js3GUID = "";
        }
        j = m_JS.Find_jsN( 4 );
        if ( j != null ) {
          newTree.ActionMaps.js4 = j.DevName; newTree.ActionMaps.js4GUID = j.DevInstanceGUID;
        }
        else {
          newTree.ActionMaps.js4 = ""; newTree.ActionMaps.js4GUID = "";
        }

        m_AT = newTree; // make it the valid one
        m_AT.ReloadCtrl( );
        if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
      }

      timer1.Enabled = true;
    }


    // Blend Unmapped

    private void cbxBlendUnmapped_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_AT != null ) {
        m_AT.BlendUnmapped = cbxBlendUnmapped.Checked;
        m_AT.ReloadCtrl( );
      }
      m_AppSettings.BlendUnmapped = cbxBlendUnmapped.Checked;

    }


    #endregion





  }
}
