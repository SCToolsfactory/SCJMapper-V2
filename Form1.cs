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
    AppSettings m_AppSettings = new AppSettings( );

    ///<remarks>
    /// Holds the DXInput Joystick List
    ///</remarks>
    private List<JoystickCls> m_JS = new List<JoystickCls>( );
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


    /// <summary>
    ///  Handle the load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainForm_Load( object sender, System.EventArgs e )
    {
      // some applic initialization 
      // Assign Size property, since databinding to Size doesn't work well.

      this.Size = m_AppSettings.FormSize;
      this.Location = m_AppSettings.FormLocation;

      // XML RTB
      rtb.SelectionTabs = new int[] { 10, 20, 30, 40, 50, 60 }; // short tabs
      rtb.DragEnter += new DragEventHandler( rtb_DragEnter );
      rtb.DragDrop += new DragEventHandler( rtb_DragDrop );
      rtb.AllowDrop = true; // add Drop to rtb
      String version = Application.ProductVersion;  // get the version information
      lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // get the first two elements

      // load profiles
      foreach ( String s in SCDefaultProfile.DefaultProfileNames ) {
        tsDDbtProfiles.DropDownItems.Add( Path.GetFileNameWithoutExtension( s ) );
      }
      tsDDbtProfiles.Text = m_AppSettings.DefProfileName;

      // load ResetMode
      tsDDbtResetMode.DropDownItems.Add( m_AppSettings.ResetModeEmpty );
      tsDDbtResetMode.DropDownItems.Add( m_AppSettings.ResetModeDefault );
      tsDDbtResetMode.Text = m_AppSettings.ResetMode;

      // Init X things
      if ( !InitDirectInput( ) )
        Close( );

      // poll the XInput
      timer1.Start( ); // this one polls the joysticks to show the props
    }


    /// <summary>
    /// Handles the Exit button
    /// </summary>
    private void buttonExit_Click( object sender, System.EventArgs e )
    {
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
        MessageBox.Show( Ex.Message.ToString( ), "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Information );

      }

    }

    #endregion

    /// <summary>
    /// Resets the Action Tree
    /// </summary>
    private void InitActionTree( Boolean addDefaultBinding )
    {
      // build TreeView and the ActionMaps
      m_AT = new ActionTree( );
      m_AT.Ctrl = treeView1;  // the ActionTree owns the TreeView control
      m_AT.LoadTree( m_AppSettings.DefProfileName, addDefaultBinding );       // Init with default profile filepath

      // default JS to Joystick mapping - can be changed and reloaded from XML
      if ( tc1.TabCount > 0 ) { cbJs1.SelectedIndex = 0; m_AT.ActionMaps.js1 = cbJs1.Text; }
      if ( tc1.TabCount > 1 ) { cbJs2.SelectedIndex = 1; m_AT.ActionMaps.js2 = cbJs2.Text; }
      if ( tc1.TabCount > 2 ) { cbJs3.SelectedIndex = 2; m_AT.ActionMaps.js3 = cbJs3.Text; }
    }

    /// <summary>
    /// Aquire the DInput joystick devices
    /// </summary>
    /// <returns></returns>
    public bool InitDirectInput( )
    {
      // Enumerate joysticks in the system.
      int tabs = 0;
      cbJs1.Items.Clear( ); cbJs2.Items.Clear( ); cbJs3.Items.Clear( ); // JS dropdowns init

      // Initialize DirectInput
      var directInput = new DirectInput( );

      // scan the Input for attached devices
      foreach ( DeviceInstance instance in directInput.GetDevices( DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly ) ) {

        // Create the device interface
        Joystick jsDevice = new Joystick( directInput, instance.InstanceGuid );
        JoystickCls js = null;

        // we have the first tab made as reference so TabPage[0] already exists
        if ( tabs == 0 ) {
          // first panel - The Tab content exists already 
          js = new JoystickCls( jsDevice, this, UC_JoyPanel ); // does all device related activities for that particular item
        }
        else {
          // setup the further tab contents along the reference one in TabPage[0] (the control named UC_JoyPanel)
          tc1.TabPages.Add( "Joystick " + ( tabs + 1 ).ToString( ) );
          UC_JoyPanel uUC_JoyPanelNew = new UC_JoyPanel( );
          tc1.TabPages[tabs].Controls.Add( uUC_JoyPanelNew );
          uUC_JoyPanelNew.Size = UC_JoyPanel.Size;
          uUC_JoyPanelNew.Location = UC_JoyPanel.Location;
          js = new JoystickCls( jsDevice, this, uUC_JoyPanelNew ); // does all device related activities for that particular item
        }
        m_JS.Add( js ); // add to joystick list

        tc1.TabPages[tabs].Tag = js.DevName;  // used to find the tab via JS mapping
        tc1.TabPages[tabs].BackColor = MyColors.JColor[tabs]; // each tab has its own color
        cbJs1.Items.Add( js.DevName ); cbJs2.Items.Add( js.DevName ); cbJs3.Items.Add( js.DevName ); // populate DropDowns with the JS name

        // next tab
        tabs++;
        if ( tabs == 8 ) break; // cannot load more JSticks than predefined Tabs
      }
      /*
      // TEST CREATE ALL 8 TABS
      for ( int i=(tabs+1); i < 9; i++ ) {
        tc1.TabPages.Add( "Joystick " + i.ToString( ) );
      }
      */

      if ( tabs == 0 ) {
        MessageBox.Show( "Unable to create a joystick device. Program will exit.", "No joystick found" );
        return false;
      }

      // load the profile items from the XML
      InitActionTree( ( m_AppSettings.ResetMode == m_AppSettings.ResetModeDefault ) );

      return true;
    }

    /// <summary>
    /// Create the jsN  Joystick string from mapping (or from the JS index above item 3)
    /// </summary>
    /// <returns></returns>
    private String JSStr( )
    {
      if ( ( String )tc1.SelectedTab.Tag == ( string )cbJs1.SelectedItem ) return JoystickCls.JSTag( 1 );
      if ( ( String )tc1.SelectedTab.Tag == ( string )cbJs2.SelectedItem ) return JoystickCls.JSTag( 2 );
      if ( ( String )tc1.SelectedTab.Tag == ( string )cbJs3.SelectedItem ) return JoystickCls.JSTag( 3 );
      return JoystickCls.JSTag( tc1.SelectedIndex + 1 ); // return the Joystick number
    }


    #region Event Handling

    private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
    {
      m_AppSettings.FormSize = this.Size;
      m_AppSettings.FormLocation = this.Location;
      m_AppSettings.Save( );
    }


    private void timer1_Tick( object sender, System.EventArgs e )
    {
      foreach ( JoystickCls jsc in m_JS ) { jsc.GetData( ); }  // poll the devices
      lblLastJ.Text = JSStr( ) + m_JS[tc1.SelectedIndex].GetLastChange( ); // show last handled JS control
    }

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

    private void btAssign_Click( object sender, EventArgs e )
    {
      m_AT.UpdateSelectedItem( lblLastJ.Text );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }

    private void btClear_Click( object sender, EventArgs e )
    {

      m_AT.UpdateSelectedItem( "" );
      if ( m_AT.Dirty ) btDump.BackColor = MyColors.DirtyColor;
    }

    private void btDump_Click( object sender, EventArgs e )
    {
      rtb.Text = String.Format( "<!-- {0} - SC Joystick Mapping -->\n{1}", DateTime.Now, m_AT.ActionMaps.toXML( ) );

      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
    }

    private void btDumpList_Click( object sender, EventArgs e )
    {
      rtb.Text = String.Format( "-- {0} - SC Joystick Mapping --\n{1}", DateTime.Now, m_AT.ReportActions( ) );
    }

    private void btGrab_Click( object sender, EventArgs e )
    {
      m_AT.ActionMaps.fromXML( rtb.Text );
      m_AT.ReloadCtrl( );
      // JS mapping for the first 3 items can be changed and reloaded from XML
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js1 ) ) {
        int i = cbJs1.FindString( m_AT.ActionMaps.js1 );
        if ( i >= 0 ) cbJs1.SelectedIndex = i;
      }
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js2 ) ) {
        int i = cbJs2.FindString( m_AT.ActionMaps.js2 );
        if ( i >= 0 ) cbJs2.SelectedIndex = i;
      }
      if ( !String.IsNullOrEmpty( m_AT.ActionMaps.js3 ) ) {
        int i = cbJs3.FindString( m_AT.ActionMaps.js3 );
        if ( i >= 0 ) cbJs3.SelectedIndex = i;
      }
      btDump.BackColor = btClear.BackColor; btDump.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; btGrab.UseVisualStyleBackColor = btClear.UseVisualStyleBackColor; // neutral again
    }

    private void tsBtReset_ButtonClick( object sender, EventArgs e )
    {
      // start over and if chosen, load defaults from SC game
      InitActionTree( ( m_AppSettings.ResetMode == m_AppSettings.ResetModeDefault ) );
    }

    private void tsDDbtProfiles_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      tsDDbtProfiles.Text = e.ClickedItem.Text;
      m_AppSettings.DefProfileName = e.ClickedItem.Text;
      m_AppSettings.Save( );
     // InitActionTree( ( Settings.Default.ResetMode == Settings.Default.ResetModeDefault ) ); // start over
    }

    private void tsDDbtResetMode_DropDownItemClicked( object sender, ToolStripItemClickedEventArgs e )
    {
      tsDDbtResetMode.Text = e.ClickedItem.Text;
      m_AppSettings.ResetMode = e.ClickedItem.Text;
      m_AppSettings.Save( );
    }


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
        using ( StreamReader istr = new StreamReader( OFD.OpenFile( ) ) ) {
          rtb.Text = istr.ReadToEnd( ); // load the complete XML from the file into the textbox
          btGrab.BackColor = MyColors.DirtyColor;
        }
      }
    }

    private void tsiSaveAs_Click( object sender, EventArgs e )
    {
      if ( SFD.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
        using ( StreamWriter istr = new StreamWriter( SFD.OpenFile( ) ) ) {
          istr.Write( rtb.Text ); // just dump the whole XML text
        }
      }

    }

    private void btFind_Click( object sender, EventArgs e )
    {
      m_AT.FindCtrl( lblLastJ.Text ); // find the action for a Control (joystick input)
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
      if ( droppedFilenames.Length > 0 ) rtb.LoadFile( droppedFilenames[0], System.Windows.Forms.RichTextBoxStreamType.PlainText );
    }
    #endregion





  }
}
