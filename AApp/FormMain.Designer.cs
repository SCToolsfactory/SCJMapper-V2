namespace SCJMapper_V2
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      timer1.Stop( );
      // Unacquire all DirectInput objects.
      foreach ( Devices.Joystick.JoystickCls js in Devices.DeviceInst.JoystickListRef ) js.FinishDX( );
      Devices.DeviceInst.JoystickListRef.Clear( );

      if ( disposing && ( components != null ) ) components.Dispose( );
      if ( disposing && ( m_AT != null ) ) m_AT.Dispose( );

      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.rtb = new System.Windows.Forms.RichTextBox();
      this.cmCopyPaste = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tsiCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiPaste = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiPReplace = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiSelAll = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tsiOpen = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.btGrab = new System.Windows.Forms.Button();
      this.btDump = new System.Windows.Forms.Button();
      this.panel2 = new System.Windows.Forms.Panel();
      this.lblMapping = new System.Windows.Forms.Label();
      this.lblAssigned = new System.Windows.Forms.Label();
      this.btJsKbd = new System.Windows.Forms.Button();
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.btBlend = new System.Windows.Forms.Button();
      this.lblLastJ = new System.Windows.Forms.TextBox();
      this.cmMouseEntry = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tmeK_Tab = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
      this.tmeXAxis = new System.Windows.Forms.ToolStripMenuItem();
      this.tmeYAxis = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.tmeWUp = new System.Windows.Forms.ToolStripMenuItem();
      this.tmeWDown = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.cbxThrottle = new System.Windows.Forms.CheckBox();
      this.btFind = new System.Windows.Forms.Button();
      this.lblDevCtrl = new System.Windows.Forms.Label();
      this.lblSelected = new System.Windows.Forms.Label();
      this.btClear = new System.Windows.Forms.Button();
      this.lblAction = new System.Windows.Forms.Label();
      this.btAssign = new System.Windows.Forms.Button();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.cmAddDel = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tdiCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiExpandAll = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiSGroup1 = new System.Windows.Forms.ToolStripSeparator();
      this.tdiAssignBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiBlendBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiClearBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiSGroup2 = new System.Windows.Forms.ToolStripSeparator();
      this.tdiAddBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiDelBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiSGroup3 = new System.Windows.Forms.ToolStripSeparator();
      this.tdiTxDefActivationMode = new System.Windows.Forms.ToolStripTextBox();
      this.tdiCbxActivation = new System.Windows.Forms.ToolStripComboBox();
      this.tdiSGroup4 = new System.Windows.Forms.ToolStripSeparator();
      this.tdiAddMod1 = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiAddMod2 = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiAddMod3 = new System.Windows.Forms.ToolStripMenuItem();
      this.tc1 = new System.Windows.Forms.TabControl();
      this.tabJS1 = new System.Windows.Forms.TabPage();
      this.UC_JoyPanel = new SCJMapper_V2.Devices.Joystick.UC_JoyPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btClip = new System.Windows.Forms.Button();
      this.txRebind = new System.Windows.Forms.TextBox();
      this.linkLblReleases = new System.Windows.Forms.LinkLabel();
      this.label8 = new System.Windows.Forms.Label();
      this.lblTitle = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.buttonExit = new System.Windows.Forms.Button();
      this.OFD = new System.Windows.Forms.OpenFileDialog();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.SFD = new System.Windows.Forms.SaveFileDialog();
      this.tlpanel = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.lblPTU = new System.Windows.Forms.Label();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.txMappingName = new System.Windows.Forms.TextBox();
      this.lblMappingname = new System.Windows.Forms.Label();
      this.btSaveMyMapping = new System.Windows.Forms.Button();
      this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
      this.btClearFilter = new System.Windows.Forms.Button();
      this.lblActionFilter = new System.Windows.Forms.Label();
      this.cbxShowMappedOnly = new System.Windows.Forms.CheckBox();
      this.cbxShowMouse = new System.Windows.Forms.CheckBox();
      this.cbxShowKeyboard = new System.Windows.Forms.CheckBox();
      this.cbxShowJoystick = new System.Windows.Forms.CheckBox();
      this.cbxShowGamepad = new System.Windows.Forms.CheckBox();
      this.txFilter = new System.Windows.Forms.TextBox();
      this.tcXML = new System.Windows.Forms.TabControl();
      this.tPageDump = new System.Windows.Forms.TabPage();
      this.tPageOther = new System.Windows.Forms.TabPage();
      this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
      this.cbxAutoTabXML = new System.Windows.Forms.CheckBox();
      this.lbxOther = new System.Windows.Forms.RichTextBox();
      this.tsLblProfile = new System.Windows.Forms.ToolStripStatusLabel();
      this.tsLblSupportedProfile = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.tslblProfileUsed = new System.Windows.Forms.ToolStripStatusLabel();
      this.tsLblSupport = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.msBtLoad = new System.Windows.Forms.ToolStripDropDownButton();
      this.meResetDefaults = new System.Windows.Forms.ToolStripMenuItem();
      this.meResetEmpty = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.msBtDump = new System.Windows.Forms.ToolStripDropDownButton();
      this.meDumpMappingList = new System.Windows.Forms.ToolStripMenuItem();
      this.meDumpLogfile = new System.Windows.Forms.ToolStripMenuItem();
      this.meDumpDefaultProfile = new System.Windows.Forms.ToolStripMenuItem();
      this.meDumpActiontreeAsXML = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.msBtShow = new System.Windows.Forms.ToolStripDropDownButton();
      this.meShowToggleTable = new System.Windows.Forms.ToolStripMenuItem();
      this.meShowOptionsDialog = new System.Windows.Forms.ToolStripMenuItem();
      this.meShowDeviceTuningDialog = new System.Windows.Forms.ToolStripMenuItem();
      this.meShowDeviceMonitoringDialog = new System.Windows.Forms.ToolStripMenuItem();
      this.meShowLayoutDialog = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this.msBtConfig = new System.Windows.Forms.ToolStripDropDownButton();
      this.meSettingsDialog = new System.Windows.Forms.ToolStripMenuItem();
      this.meJsReassignDialog = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
      this.msBtLoadMap = new System.Windows.Forms.ToolStripDropDownButton();
      this.meDefaultsLoadAndGrab = new System.Windows.Forms.ToolStripMenuItem();
      this.meResetLoadAndGrab = new System.Windows.Forms.ToolStripMenuItem();
      this.meLoadAndGrab = new System.Windows.Forms.ToolStripMenuItem();
      this.meLoad = new System.Windows.Forms.ToolStripMenuItem();
      this.msSelectMapping = new System.Windows.Forms.ToolStripDropDownButton();
      this.tsLblMappings = new System.Windows.Forms.ToolStripLabel();
      this.IL2 = new System.Windows.Forms.ImageList(this.components);
      this.cmCopyPaste.SuspendLayout();
      this.panel2.SuspendLayout();
      this.cmMouseEntry.SuspendLayout();
      this.cmAddDel.SuspendLayout();
      this.tc1.SuspendLayout();
      this.tabJS1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tlpanel.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.tableLayoutPanel4.SuspendLayout();
      this.tableLayoutPanel5.SuspendLayout();
      this.tcXML.SuspendLayout();
      this.tPageDump.SuspendLayout();
      this.tPageOther.SuspendLayout();
      this.tableLayoutPanel6.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // rtb
      // 
      this.rtb.AcceptsTab = true;
      this.rtb.ContextMenuStrip = this.cmCopyPaste;
      this.rtb.DetectUrls = false;
      this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rtb.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtb.Location = new System.Drawing.Point(3, 3);
      this.rtb.Name = "rtb";
      this.rtb.Size = new System.Drawing.Size(338, 608);
      this.rtb.TabIndex = 21;
      this.rtb.Text = "";
      this.rtb.WordWrap = false;
      // 
      // cmCopyPaste
      // 
      this.cmCopyPaste.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiCopy,
            this.tsiPaste,
            this.tsiPReplace,
            this.tsiSelAll,
            this.toolStripSeparator1,
            this.tsiOpen,
            this.tsiSaveAs});
      this.cmCopyPaste.Name = "cmCopyPaste";
      this.cmCopyPaste.Size = new System.Drawing.Size(170, 142);
      // 
      // tsiCopy
      // 
      this.tsiCopy.Name = "tsiCopy";
      this.tsiCopy.Size = new System.Drawing.Size(169, 22);
      this.tsiCopy.Tag = "§";
      this.tsiCopy.Text = "Copy";
      this.tsiCopy.Click += new System.EventHandler(this.tsiCopy_Click);
      // 
      // tsiPaste
      // 
      this.tsiPaste.Name = "tsiPaste";
      this.tsiPaste.Size = new System.Drawing.Size(169, 22);
      this.tsiPaste.Tag = "§";
      this.tsiPaste.Text = "Paste";
      this.tsiPaste.Click += new System.EventHandler(this.tsiPaste_Click);
      // 
      // tsiPReplace
      // 
      this.tsiPReplace.Name = "tsiPReplace";
      this.tsiPReplace.Size = new System.Drawing.Size(169, 22);
      this.tsiPReplace.Tag = "§";
      this.tsiPReplace.Text = "Paste (Replace all)";
      this.tsiPReplace.Click += new System.EventHandler(this.tsiPReplace_Click);
      // 
      // tsiSelAll
      // 
      this.tsiSelAll.Name = "tsiSelAll";
      this.tsiSelAll.Size = new System.Drawing.Size(169, 22);
      this.tsiSelAll.Tag = "§";
      this.tsiSelAll.Text = "Select All";
      this.tsiSelAll.Click += new System.EventHandler(this.tsiSelAll_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
      // 
      // tsiOpen
      // 
      this.tsiOpen.Name = "tsiOpen";
      this.tsiOpen.Size = new System.Drawing.Size(169, 22);
      this.tsiOpen.Tag = "§";
      this.tsiOpen.Text = "Open...";
      this.tsiOpen.Click += new System.EventHandler(this.tsiOpen_Click);
      // 
      // tsiSaveAs
      // 
      this.tsiSaveAs.Name = "tsiSaveAs";
      this.tsiSaveAs.Size = new System.Drawing.Size(169, 22);
      this.tsiSaveAs.Tag = "§";
      this.tsiSaveAs.Text = "Save as...";
      this.tsiSaveAs.Click += new System.EventHandler(this.tsiSaveAs_Click);
      // 
      // btGrab
      // 
      this.btGrab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btGrab.Image = global::SCJMapper_V2.Properties.Resources.LArrow;
      this.btGrab.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btGrab.Location = new System.Drawing.Point(181, 3);
      this.btGrab.Name = "btGrab";
      this.btGrab.Size = new System.Drawing.Size(120, 50);
      this.btGrab.TabIndex = 19;
      this.btGrab.Tag = "§";
      this.btGrab.Text = "Grab XML";
      this.btGrab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btGrab.UseVisualStyleBackColor = true;
      this.btGrab.Click += new System.EventHandler(this.btGrab_Click);
      // 
      // btDump
      // 
      this.btDump.Image = global::SCJMapper_V2.Properties.Resources.RArrow;
      this.btDump.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btDump.Location = new System.Drawing.Point(3, 3);
      this.btDump.Name = "btDump";
      this.btDump.Size = new System.Drawing.Size(120, 50);
      this.btDump.TabIndex = 20;
      this.btDump.Tag = "§";
      this.btDump.Text = "Dump XML";
      this.btDump.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btDump.UseVisualStyleBackColor = true;
      this.btDump.Click += new System.EventHandler(this.btDump_Click);
      // 
      // panel2
      // 
      this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.lblMapping);
      this.panel2.Controls.Add(this.lblAssigned);
      this.panel2.Controls.Add(this.btJsKbd);
      this.panel2.Controls.Add(this.btBlend);
      this.panel2.Controls.Add(this.lblLastJ);
      this.panel2.Controls.Add(this.cbxThrottle);
      this.panel2.Controls.Add(this.btFind);
      this.panel2.Controls.Add(this.lblDevCtrl);
      this.panel2.Controls.Add(this.lblSelected);
      this.panel2.Controls.Add(this.btClear);
      this.panel2.Controls.Add(this.lblAction);
      this.panel2.Controls.Add(this.btAssign);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel2.Location = new System.Drawing.Point(3, 376);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(298, 164);
      this.panel2.TabIndex = 17;
      // 
      // lblMapping
      // 
      this.lblMapping.AutoSize = true;
      this.lblMapping.Location = new System.Drawing.Point(4, 41);
      this.lblMapping.Name = "lblMapping";
      this.lblMapping.Size = new System.Drawing.Size(54, 13);
      this.lblMapping.TabIndex = 18;
      this.lblMapping.Tag = "§";
      this.lblMapping.Text = "Mapping";
      // 
      // lblAssigned
      // 
      this.lblAssigned.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblAssigned.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAssigned.Location = new System.Drawing.Point(89, 40);
      this.lblAssigned.Name = "lblAssigned";
      this.lblAssigned.Size = new System.Drawing.Size(199, 20);
      this.lblAssigned.TabIndex = 17;
      this.lblAssigned.Text = "...";
      // 
      // btJsKbd
      // 
      this.btJsKbd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btJsKbd.ImageKey = "J";
      this.btJsKbd.ImageList = this.IL;
      this.btJsKbd.Location = new System.Drawing.Point(107, 134);
      this.btJsKbd.Name = "btJsKbd";
      this.btJsKbd.Size = new System.Drawing.Size(79, 25);
      this.btJsKbd.TabIndex = 16;
      this.btJsKbd.Tag = "§";
      this.btJsKbd.Text = "JS / Kbd";
      this.btJsKbd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btJsKbd.UseVisualStyleBackColor = true;
      this.btJsKbd.Click += new System.EventHandler(this.btJsKbd_Click);
      // 
      // IL
      // 
      this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
      this.IL.TransparentColor = System.Drawing.Color.Transparent;
      this.IL.Images.SetKeyName(0, "Map");
      this.IL.Images.SetKeyName(1, "Selected");
      this.IL.Images.SetKeyName(2, "J");
      this.IL.Images.SetKeyName(3, "K");
      this.IL.Images.SetKeyName(4, "M");
      this.IL.Images.SetKeyName(5, "X");
      this.IL.Images.SetKeyName(6, "P");
      this.IL.Images.SetKeyName(7, "Z");
      this.IL.Images.SetKeyName(8, "Add");
      // 
      // btBlend
      // 
      this.btBlend.Location = new System.Drawing.Point(9, 134);
      this.btBlend.Name = "btBlend";
      this.btBlend.Size = new System.Drawing.Size(92, 25);
      this.btBlend.TabIndex = 16;
      this.btBlend.Tag = "§";
      this.btBlend.Text = "Disable";
      this.btBlend.UseVisualStyleBackColor = true;
      this.btBlend.Click += new System.EventHandler(this.btBlend_Click);
      // 
      // lblLastJ
      // 
      this.lblLastJ.CausesValidation = false;
      this.lblLastJ.ContextMenuStrip = this.cmMouseEntry;
      this.lblLastJ.Location = new System.Drawing.Point(89, 74);
      this.lblLastJ.Name = "lblLastJ";
      this.lblLastJ.Size = new System.Drawing.Size(199, 22);
      this.lblLastJ.TabIndex = 14;
      this.lblLastJ.Text = "...";
      this.lblLastJ.TextChanged += new System.EventHandler(this.lblLastJ_TextChanged);
      this.lblLastJ.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lblLastJ_KeyDown);
      // 
      // cmMouseEntry
      // 
      this.cmMouseEntry.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmeK_Tab,
            this.toolStripSeparator7,
            this.tmeXAxis,
            this.tmeYAxis,
            this.toolStripSeparator3,
            this.tmeWUp,
            this.tmeWDown,
            this.toolStripSeparator4});
      this.cmMouseEntry.Name = "cmMouseEntry";
      this.cmMouseEntry.Size = new System.Drawing.Size(173, 132);
      this.cmMouseEntry.Opening += new System.ComponentModel.CancelEventHandler(this.cmMouseEntry_Opening);
      // 
      // tmeK_Tab
      // 
      this.tmeK_Tab.Name = "tmeK_Tab";
      this.tmeK_Tab.Size = new System.Drawing.Size(172, 22);
      this.tmeK_Tab.Tag = "K_Tab";
      this.tmeK_Tab.Text = "Kbd - TAB";
      this.tmeK_Tab.Click += new System.EventHandler(this.tmeItem_Click);
      // 
      // toolStripSeparator7
      // 
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new System.Drawing.Size(169, 6);
      // 
      // tmeXAxis
      // 
      this.tmeXAxis.Name = "tmeXAxis";
      this.tmeXAxis.Size = new System.Drawing.Size(172, 22);
      this.tmeXAxis.Tag = "X";
      this.tmeXAxis.Text = "X-Axis (horizontal)";
      this.tmeXAxis.Click += new System.EventHandler(this.tmeItem_Click);
      // 
      // tmeYAxis
      // 
      this.tmeYAxis.Name = "tmeYAxis";
      this.tmeYAxis.Size = new System.Drawing.Size(172, 22);
      this.tmeYAxis.Tag = "Y";
      this.tmeYAxis.Text = "Y-Axis (vertical)";
      this.tmeYAxis.Click += new System.EventHandler(this.tmeItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(169, 6);
      // 
      // tmeWUp
      // 
      this.tmeWUp.Name = "tmeWUp";
      this.tmeWUp.Size = new System.Drawing.Size(172, 22);
      this.tmeWUp.Tag = "U";
      this.tmeWUp.Text = "Wheel Up";
      this.tmeWUp.Click += new System.EventHandler(this.tmeItem_Click);
      // 
      // tmeWDown
      // 
      this.tmeWDown.Name = "tmeWDown";
      this.tmeWDown.Size = new System.Drawing.Size(172, 22);
      this.tmeWDown.Tag = "D";
      this.tmeWDown.Text = "Wheel Down";
      this.tmeWDown.Click += new System.EventHandler(this.tmeItem_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(169, 6);
      // 
      // cbxThrottle
      // 
      this.cbxThrottle.AutoSize = true;
      this.cbxThrottle.Location = new System.Drawing.Point(107, 108);
      this.cbxThrottle.Name = "cbxThrottle";
      this.cbxThrottle.Size = new System.Drawing.Size(67, 17);
      this.cbxThrottle.TabIndex = 13;
      this.cbxThrottle.Tag = "§";
      this.cbxThrottle.Text = "Throttle";
      this.cbxThrottle.UseVisualStyleBackColor = true;
      // 
      // btFind
      // 
      this.btFind.Location = new System.Drawing.Point(196, 103);
      this.btFind.Name = "btFind";
      this.btFind.Size = new System.Drawing.Size(92, 25);
      this.btFind.TabIndex = 12;
      this.btFind.Tag = "§";
      this.btFind.Text = "Find 1st.";
      this.btFind.UseVisualStyleBackColor = true;
      this.btFind.Click += new System.EventHandler(this.btFind_Click);
      // 
      // lblDevCtrl
      // 
      this.lblDevCtrl.AutoSize = true;
      this.lblDevCtrl.Location = new System.Drawing.Point(4, 77);
      this.lblDevCtrl.Name = "lblDevCtrl";
      this.lblDevCtrl.Size = new System.Drawing.Size(50, 13);
      this.lblDevCtrl.TabIndex = 3;
      this.lblDevCtrl.Tag = "§";
      this.lblDevCtrl.Text = "Dev Ctrl.";
      // 
      // lblSelected
      // 
      this.lblSelected.AutoSize = true;
      this.lblSelected.Location = new System.Drawing.Point(4, 18);
      this.lblSelected.Name = "lblSelected";
      this.lblSelected.Size = new System.Drawing.Size(50, 13);
      this.lblSelected.TabIndex = 3;
      this.lblSelected.Tag = "§";
      this.lblSelected.Text = "Selected";
      // 
      // btClear
      // 
      this.btClear.Location = new System.Drawing.Point(196, 134);
      this.btClear.Name = "btClear";
      this.btClear.Size = new System.Drawing.Size(92, 25);
      this.btClear.TabIndex = 2;
      this.btClear.Tag = "§";
      this.btClear.Text = "Clear";
      this.btClear.UseVisualStyleBackColor = true;
      this.btClear.Click += new System.EventHandler(this.btClear_Click);
      // 
      // lblAction
      // 
      this.lblAction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblAction.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAction.Location = new System.Drawing.Point(89, 17);
      this.lblAction.Name = "lblAction";
      this.lblAction.Size = new System.Drawing.Size(199, 20);
      this.lblAction.TabIndex = 1;
      this.lblAction.Text = "...";
      // 
      // btAssign
      // 
      this.btAssign.Location = new System.Drawing.Point(9, 103);
      this.btAssign.Name = "btAssign";
      this.btAssign.Size = new System.Drawing.Size(92, 25);
      this.btAssign.TabIndex = 15;
      this.btAssign.Tag = "§";
      this.btAssign.Text = "Assign";
      this.btAssign.UseVisualStyleBackColor = true;
      this.btAssign.Click += new System.EventHandler(this.btAssign_Click);
      // 
      // treeView1
      // 
      this.treeView1.ContextMenuStrip = this.cmAddDel;
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.FullRowSelect = true;
      this.treeView1.HotTracking = true;
      this.treeView1.ImageKey = "Map";
      this.treeView1.ImageList = this.IL;
      this.treeView1.Location = new System.Drawing.Point(6, 126);
      this.treeView1.Name = "treeView1";
      this.tlpanel.SetRowSpan(this.treeView1, 2);
      this.treeView1.SelectedImageKey = "Selected";
      this.treeView1.Size = new System.Drawing.Size(374, 640);
      this.treeView1.TabIndex = 16;
      this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
      // 
      // cmAddDel
      // 
      this.cmAddDel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tdiCollapseAll,
            this.tdiExpandAll,
            this.tdiSGroup1,
            this.tdiAssignBinding,
            this.tdiBlendBinding,
            this.tdiClearBinding,
            this.tdiSGroup2,
            this.tdiAddBinding,
            this.tdiDelBinding,
            this.tdiSGroup3,
            this.tdiTxDefActivationMode,
            this.tdiCbxActivation,
            this.tdiSGroup4,
            this.tdiAddMod1,
            this.tdiAddMod2,
            this.tdiAddMod3});
      this.cmAddDel.Name = "cmAddDel";
      this.cmAddDel.Size = new System.Drawing.Size(261, 457);
      this.cmAddDel.Opening += new System.ComponentModel.CancelEventHandler(this.cmAddDel_Opening);
      // 
      // tdiCollapseAll
      // 
      this.tdiCollapseAll.ForeColor = System.Drawing.Color.MediumBlue;
      this.tdiCollapseAll.Name = "tdiCollapseAll";
      this.tdiCollapseAll.Size = new System.Drawing.Size(260, 22);
      this.tdiCollapseAll.Tag = "§";
      this.tdiCollapseAll.Text = "Collapse to selected";
      this.tdiCollapseAll.Click += new System.EventHandler(this.tdiCollapseAll_Click);
      // 
      // tdiExpandAll
      // 
      this.tdiExpandAll.ForeColor = System.Drawing.Color.MediumBlue;
      this.tdiExpandAll.Name = "tdiExpandAll";
      this.tdiExpandAll.Size = new System.Drawing.Size(260, 22);
      this.tdiExpandAll.Tag = "§";
      this.tdiExpandAll.Text = "Expand all Mappings";
      this.tdiExpandAll.Click += new System.EventHandler(this.tdiExpandAll_Click);
      // 
      // tdiSGroup1
      // 
      this.tdiSGroup1.Name = "tdiSGroup1";
      this.tdiSGroup1.Size = new System.Drawing.Size(257, 6);
      // 
      // tdiAssignBinding
      // 
      this.tdiAssignBinding.Name = "tdiAssignBinding";
      this.tdiAssignBinding.Size = new System.Drawing.Size(260, 22);
      this.tdiAssignBinding.Tag = "§";
      this.tdiAssignBinding.Text = "Assign Mapping";
      this.tdiAssignBinding.Click += new System.EventHandler(this.tdiAssignBinding_Click);
      // 
      // tdiBlendBinding
      // 
      this.tdiBlendBinding.Name = "tdiBlendBinding";
      this.tdiBlendBinding.Size = new System.Drawing.Size(260, 22);
      this.tdiBlendBinding.Tag = "§";
      this.tdiBlendBinding.Text = "Disable Mapping";
      this.tdiBlendBinding.Click += new System.EventHandler(this.tdiBlendBinding_Click);
      // 
      // tdiClearBinding
      // 
      this.tdiClearBinding.Name = "tdiClearBinding";
      this.tdiClearBinding.Size = new System.Drawing.Size(260, 22);
      this.tdiClearBinding.Tag = "§";
      this.tdiClearBinding.Text = "Clear Mapping";
      this.tdiClearBinding.Click += new System.EventHandler(this.tdiClearBinding_Click);
      // 
      // tdiSGroup2
      // 
      this.tdiSGroup2.Name = "tdiSGroup2";
      this.tdiSGroup2.Size = new System.Drawing.Size(257, 6);
      // 
      // tdiAddBinding
      // 
      this.tdiAddBinding.Name = "tdiAddBinding";
      this.tdiAddBinding.Size = new System.Drawing.Size(260, 22);
      this.tdiAddBinding.Tag = "§";
      this.tdiAddBinding.Text = "Add Mapping";
      this.tdiAddBinding.Click += new System.EventHandler(this.tsiAddBinding_Click);
      // 
      // tdiDelBinding
      // 
      this.tdiDelBinding.Name = "tdiDelBinding";
      this.tdiDelBinding.Size = new System.Drawing.Size(260, 22);
      this.tdiDelBinding.Tag = "§";
      this.tdiDelBinding.Text = "Delete Mapping";
      this.tdiDelBinding.Click += new System.EventHandler(this.tdiDelBinding_Click);
      // 
      // tdiSGroup3
      // 
      this.tdiSGroup3.Name = "tdiSGroup3";
      this.tdiSGroup3.Size = new System.Drawing.Size(257, 6);
      // 
      // tdiTxDefActivationMode
      // 
      this.tdiTxDefActivationMode.BackColor = System.Drawing.Color.PapayaWhip;
      this.tdiTxDefActivationMode.Name = "tdiTxDefActivationMode";
      this.tdiTxDefActivationMode.ReadOnly = true;
      this.tdiTxDefActivationMode.Size = new System.Drawing.Size(200, 23);
      this.tdiTxDefActivationMode.Text = "Default ActMode";
      // 
      // tdiCbxActivation
      // 
      this.tdiCbxActivation.AutoSize = false;
      this.tdiCbxActivation.DropDownHeight = 140;
      this.tdiCbxActivation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
      this.tdiCbxActivation.DropDownWidth = 160;
      this.tdiCbxActivation.IntegralHeight = false;
      this.tdiCbxActivation.Items.AddRange(new object[] {
            "None"});
      this.tdiCbxActivation.MaxDropDownItems = 10;
      this.tdiCbxActivation.Name = "tdiCbxActivation";
      this.tdiCbxActivation.Size = new System.Drawing.Size(200, 180);
      this.tdiCbxActivation.Click += new System.EventHandler(this.tdiCbxActivation_Click);
      // 
      // tdiSGroup4
      // 
      this.tdiSGroup4.Name = "tdiSGroup4";
      this.tdiSGroup4.Size = new System.Drawing.Size(257, 6);
      // 
      // tdiAddMod1
      // 
      this.tdiAddMod1.Name = "tdiAddMod1";
      this.tdiAddMod1.Size = new System.Drawing.Size(260, 22);
      this.tdiAddMod1.Tag = "0";
      this.tdiAddMod1.Text = "Mod:";
      this.tdiAddMod1.Visible = false;
      this.tdiAddMod1.Click += new System.EventHandler(this.tdiAddMod_Click);
      // 
      // tdiAddMod2
      // 
      this.tdiAddMod2.Name = "tdiAddMod2";
      this.tdiAddMod2.Size = new System.Drawing.Size(260, 22);
      this.tdiAddMod2.Tag = "1";
      this.tdiAddMod2.Text = "Mod:";
      this.tdiAddMod2.Visible = false;
      this.tdiAddMod2.Click += new System.EventHandler(this.tdiAddMod_Click);
      // 
      // tdiAddMod3
      // 
      this.tdiAddMod3.Name = "tdiAddMod3";
      this.tdiAddMod3.Size = new System.Drawing.Size(260, 22);
      this.tdiAddMod3.Tag = "2";
      this.tdiAddMod3.Text = "Mod:";
      this.tdiAddMod3.Visible = false;
      this.tdiAddMod3.Click += new System.EventHandler(this.tdiAddMod_Click);
      // 
      // tc1
      // 
      this.tc1.Controls.Add(this.tabJS1);
      this.tc1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tc1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tc1.HotTrack = true;
      this.tc1.ItemSize = new System.Drawing.Size(62, 20);
      this.tc1.Location = new System.Drawing.Point(3, 3);
      this.tc1.Multiline = true;
      this.tc1.Name = "tc1";
      this.tc1.SelectedIndex = 0;
      this.tc1.ShowToolTips = true;
      this.tc1.Size = new System.Drawing.Size(298, 367);
      this.tc1.TabIndex = 15;
      this.tc1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tc1_DrawItem);
      this.tc1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tc1_Selected);
      // 
      // tabJS1
      // 
      this.tabJS1.Controls.Add(this.UC_JoyPanel);
      this.tabJS1.Location = new System.Drawing.Point(4, 24);
      this.tabJS1.Name = "tabJS1";
      this.tabJS1.Padding = new System.Windows.Forms.Padding(3);
      this.tabJS1.Size = new System.Drawing.Size(290, 339);
      this.tabJS1.TabIndex = 0;
      this.tabJS1.Text = "Joystick 1";
      // 
      // UC_JoyPanel
      // 
      this.UC_JoyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.UC_JoyPanel.JsAssignment = 0;
      this.UC_JoyPanel.Location = new System.Drawing.Point(3, 3);
      this.UC_JoyPanel.Name = "UC_JoyPanel";
      this.UC_JoyPanel.Size = new System.Drawing.Size(284, 333);
      this.UC_JoyPanel.TabIndex = 0;
      // 
      // panel1
      // 
      this.tlpanel.SetColumnSpan(this.panel1, 3);
      this.panel1.Controls.Add(this.btClip);
      this.panel1.Controls.Add(this.txRebind);
      this.panel1.Controls.Add(this.linkLblReleases);
      this.panel1.Controls.Add(this.label8);
      this.panel1.Controls.Add(this.lblTitle);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(6, 51);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1042, 66);
      this.panel1.TabIndex = 14;
      // 
      // btClip
      // 
      this.btClip.Image = global::SCJMapper_V2.Properties.Resources.Notes;
      this.btClip.Location = new System.Drawing.Point(984, 8);
      this.btClip.Name = "btClip";
      this.btClip.Size = new System.Drawing.Size(52, 55);
      this.btClip.TabIndex = 55;
      this.btClip.UseVisualStyleBackColor = true;
      this.btClip.Click += new System.EventHandler(this.btClip_Click);
      // 
      // txRebind
      // 
      this.txRebind.Location = new System.Drawing.Point(603, 39);
      this.txRebind.Name = "txRebind";
      this.txRebind.Size = new System.Drawing.Size(361, 22);
      this.txRebind.TabIndex = 4;
      this.txRebind.Text = "pp_rebindkeys";
      // 
      // linkLblReleases
      // 
      this.linkLblReleases.AutoSize = true;
      this.linkLblReleases.Location = new System.Drawing.Point(612, 20);
      this.linkLblReleases.Name = "linkLblReleases";
      this.linkLblReleases.Size = new System.Drawing.Size(259, 13);
      this.linkLblReleases.TabIndex = 3;
      this.linkLblReleases.TabStop = true;
      this.linkLblReleases.Tag = "§";
      this.linkLblReleases.Text = "For information and updates visit us @ Github ...";
      this.linkLblReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblReleases_LinkClicked);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(247, 42);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(58, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "by Cassini";
      // 
      // lblTitle
      // 
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTitle.Location = new System.Drawing.Point(226, 8);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(185, 25);
      this.lblTitle.TabIndex = 1;
      this.lblTitle.Text = "SC Joystick Mapper";
      // 
      // label4
      // 
      this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
      this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.label4.Location = new System.Drawing.Point(0, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(220, 66);
      this.label4.TabIndex = 0;
      // 
      // buttonExit
      // 
      this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonExit.Location = new System.Drawing.Point(181, 51);
      this.buttonExit.Name = "buttonExit";
      this.buttonExit.Size = new System.Drawing.Size(120, 24);
      this.buttonExit.TabIndex = 13;
      this.buttonExit.Tag = "§";
      this.buttonExit.Text = "Exit";
      this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
      // 
      // OFD
      // 
      this.OFD.DefaultExt = "xml";
      this.OFD.FileName = "Open Map File";
      this.OFD.Filter = "Mapping files|*.xml|All files|*.*";
      this.OFD.ReadOnlyChecked = true;
      this.OFD.SupportMultiDottedExtensions = true;
      // 
      // timer1
      // 
      this.timer1.Interval = 150;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // SFD
      // 
      this.SFD.DefaultExt = "xml";
      this.SFD.Filter = "Mapping files|*.xml|Text files|*.txt|All files|*.*";
      this.SFD.SupportMultiDottedExtensions = true;
      // 
      // tlpanel
      // 
      this.tlpanel.ColumnCount = 3;
      this.tlpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 380F));
      this.tlpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 310F));
      this.tlpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlpanel.Controls.Add(this.panel1, 0, 1);
      this.tlpanel.Controls.Add(this.treeView1, 0, 2);
      this.tlpanel.Controls.Add(this.tableLayoutPanel1, 1, 3);
      this.tlpanel.Controls.Add(this.tableLayoutPanel2, 1, 4);
      this.tlpanel.Controls.Add(this.tableLayoutPanel3, 2, 4);
      this.tlpanel.Controls.Add(this.tableLayoutPanel4, 1, 2);
      this.tlpanel.Controls.Add(this.tableLayoutPanel5, 0, 4);
      this.tlpanel.Controls.Add(this.tcXML, 2, 2);
      this.tlpanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tlpanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tlpanel.Location = new System.Drawing.Point(0, 0);
      this.tlpanel.Name = "tlpanel";
      this.tlpanel.Padding = new System.Windows.Forms.Padding(3);
      this.tlpanel.RowCount = 6;
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
      this.tlpanel.Size = new System.Drawing.Size(1054, 891);
      this.tlpanel.TabIndex = 25;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.btGrab, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.btDump, 0, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(386, 692);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(304, 74);
      this.tableLayoutPanel1.TabIndex = 23;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.Controls.Add(this.buttonExit, 1, 1);
      this.tableLayoutPanel2.Controls.Add(this.lblPTU, 1, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(386, 772);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(304, 78);
      this.tableLayoutPanel2.TabIndex = 24;
      // 
      // lblPTU
      // 
      this.lblPTU.AutoSize = true;
      this.lblPTU.BackColor = System.Drawing.Color.SandyBrown;
      this.lblPTU.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblPTU.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPTU.Location = new System.Drawing.Point(155, 0);
      this.lblPTU.Name = "lblPTU";
      this.lblPTU.Size = new System.Drawing.Size(146, 48);
      this.lblPTU.TabIndex = 14;
      this.lblPTU.Text = "Using PTU";
      this.lblPTU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lblPTU.Visible = false;
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
      this.tableLayoutPanel3.Controls.Add(this.txMappingName, 1, 0);
      this.tableLayoutPanel3.Controls.Add(this.lblMappingname, 0, 0);
      this.tableLayoutPanel3.Controls.Add(this.btSaveMyMapping, 0, 1);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(696, 772);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 2;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(352, 78);
      this.tableLayoutPanel3.TabIndex = 25;
      // 
      // txMappingName
      // 
      this.txMappingName.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.txMappingName.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
      this.txMappingName.Location = new System.Drawing.Point(115, 13);
      this.txMappingName.Name = "txMappingName";
      this.txMappingName.Size = new System.Drawing.Size(234, 22);
      this.txMappingName.TabIndex = 0;
      this.txMappingName.WordWrap = false;
      this.txMappingName.TextChanged += new System.EventHandler(this.txMappingName_TextChanged);
      // 
      // lblMappingname
      // 
      this.lblMappingname.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.lblMappingname.AutoSize = true;
      this.lblMappingname.Location = new System.Drawing.Point(21, 17);
      this.lblMappingname.Name = "lblMappingname";
      this.lblMappingname.Size = new System.Drawing.Size(88, 13);
      this.lblMappingname.TabIndex = 16;
      this.lblMappingname.Tag = "§";
      this.lblMappingname.Text = "Mapping name:";
      // 
      // btSaveMyMapping
      // 
      this.btSaveMyMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel3.SetColumnSpan(this.btSaveMyMapping, 2);
      this.btSaveMyMapping.Image = ((System.Drawing.Image)(resources.GetObject("btSaveMyMapping.Image")));
      this.btSaveMyMapping.Location = new System.Drawing.Point(115, 51);
      this.btSaveMyMapping.Name = "btSaveMyMapping";
      this.btSaveMyMapping.Size = new System.Drawing.Size(234, 24);
      this.btSaveMyMapping.TabIndex = 15;
      this.btSaveMyMapping.Tag = "§";
      this.btSaveMyMapping.Text = "Dump and Save my Mapping";
      this.btSaveMyMapping.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btSaveMyMapping.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btSaveMyMapping.Click += new System.EventHandler(this.btSaveMyMapping_Click);
      // 
      // tableLayoutPanel4
      // 
      this.tableLayoutPanel4.ColumnCount = 1;
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel4.Controls.Add(this.panel2, 0, 1);
      this.tableLayoutPanel4.Controls.Add(this.tc1, 0, 0);
      this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
      this.tableLayoutPanel4.Location = new System.Drawing.Point(386, 126);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      this.tableLayoutPanel4.RowCount = 2;
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
      this.tableLayoutPanel4.Size = new System.Drawing.Size(304, 543);
      this.tableLayoutPanel4.TabIndex = 28;
      // 
      // tableLayoutPanel5
      // 
      this.tableLayoutPanel5.ColumnCount = 5;
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
      this.tableLayoutPanel5.Controls.Add(this.btClearFilter, 4, 1);
      this.tableLayoutPanel5.Controls.Add(this.lblActionFilter, 0, 1);
      this.tableLayoutPanel5.Controls.Add(this.cbxShowMappedOnly, 4, 0);
      this.tableLayoutPanel5.Controls.Add(this.cbxShowMouse, 3, 0);
      this.tableLayoutPanel5.Controls.Add(this.cbxShowKeyboard, 2, 0);
      this.tableLayoutPanel5.Controls.Add(this.cbxShowJoystick, 0, 0);
      this.tableLayoutPanel5.Controls.Add(this.cbxShowGamepad, 1, 0);
      this.tableLayoutPanel5.Controls.Add(this.txFilter, 2, 1);
      this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 772);
      this.tableLayoutPanel5.Name = "tableLayoutPanel5";
      this.tableLayoutPanel5.RowCount = 2;
      this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.33333F));
      this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.66667F));
      this.tableLayoutPanel5.Size = new System.Drawing.Size(374, 78);
      this.tableLayoutPanel5.TabIndex = 29;
      // 
      // btClearFilter
      // 
      this.btClearFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.btClearFilter.Location = new System.Drawing.Point(299, 50);
      this.btClearFilter.Name = "btClearFilter";
      this.btClearFilter.Size = new System.Drawing.Size(70, 23);
      this.btClearFilter.TabIndex = 26;
      this.btClearFilter.Tag = "§";
      this.btClearFilter.Text = "Clear Filter";
      this.btClearFilter.UseVisualStyleBackColor = true;
      this.btClearFilter.Click += new System.EventHandler(this.btClearFilter_Click);
      // 
      // lblActionFilter
      // 
      this.lblActionFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.tableLayoutPanel5.SetColumnSpan(this.lblActionFilter, 2);
      this.lblActionFilter.Location = new System.Drawing.Point(3, 50);
      this.lblActionFilter.Margin = new System.Windows.Forms.Padding(3);
      this.lblActionFilter.Name = "lblActionFilter";
      this.lblActionFilter.Size = new System.Drawing.Size(83, 23);
      this.lblActionFilter.TabIndex = 27;
      this.lblActionFilter.Tag = "§";
      this.lblActionFilter.Text = "Action Filter:";
      this.lblActionFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // cbxShowMappedOnly
      // 
      this.cbxShowMappedOnly.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.cbxShowMappedOnly.AutoSize = true;
      this.cbxShowMappedOnly.Location = new System.Drawing.Point(299, 14);
      this.cbxShowMappedOnly.Name = "cbxShowMappedOnly";
      this.cbxShowMappedOnly.Size = new System.Drawing.Size(69, 17);
      this.cbxShowMappedOnly.TabIndex = 1;
      this.cbxShowMappedOnly.Tag = "§";
      this.cbxShowMappedOnly.Text = "Mapped";
      this.cbxShowMappedOnly.UseVisualStyleBackColor = true;
      this.cbxShowMappedOnly.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowMouse
      // 
      this.cbxShowMouse.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.cbxShowMouse.AutoSize = true;
      this.cbxShowMouse.Location = new System.Drawing.Point(225, 14);
      this.cbxShowMouse.Name = "cbxShowMouse";
      this.cbxShowMouse.Size = new System.Drawing.Size(61, 17);
      this.cbxShowMouse.TabIndex = 28;
      this.cbxShowMouse.Tag = "§";
      this.cbxShowMouse.Text = "Mouse";
      this.cbxShowMouse.UseVisualStyleBackColor = true;
      this.cbxShowMouse.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowKeyboard
      // 
      this.cbxShowKeyboard.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.cbxShowKeyboard.AutoSize = true;
      this.cbxShowKeyboard.Checked = true;
      this.cbxShowKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowKeyboard.Location = new System.Drawing.Point(151, 14);
      this.cbxShowKeyboard.Name = "cbxShowKeyboard";
      this.cbxShowKeyboard.Size = new System.Drawing.Size(53, 17);
      this.cbxShowKeyboard.TabIndex = 1;
      this.cbxShowKeyboard.Tag = "§";
      this.cbxShowKeyboard.Text = "Keyb.";
      this.cbxShowKeyboard.UseVisualStyleBackColor = true;
      this.cbxShowKeyboard.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowJoystick
      // 
      this.cbxShowJoystick.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.cbxShowJoystick.AutoSize = true;
      this.cbxShowJoystick.Checked = true;
      this.cbxShowJoystick.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowJoystick.Location = new System.Drawing.Point(3, 14);
      this.cbxShowJoystick.Name = "cbxShowJoystick";
      this.cbxShowJoystick.Size = new System.Drawing.Size(65, 17);
      this.cbxShowJoystick.TabIndex = 0;
      this.cbxShowJoystick.Tag = "§";
      this.cbxShowJoystick.Text = "Joystick";
      this.cbxShowJoystick.UseVisualStyleBackColor = true;
      this.cbxShowJoystick.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowGamepad
      // 
      this.cbxShowGamepad.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.cbxShowGamepad.AutoSize = true;
      this.cbxShowGamepad.Checked = true;
      this.cbxShowGamepad.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowGamepad.Location = new System.Drawing.Point(77, 14);
      this.cbxShowGamepad.Name = "cbxShowGamepad";
      this.cbxShowGamepad.Size = new System.Drawing.Size(68, 17);
      this.cbxShowGamepad.TabIndex = 1;
      this.cbxShowGamepad.Tag = "§";
      this.cbxShowGamepad.Text = "Gamepad";
      this.cbxShowGamepad.UseVisualStyleBackColor = true;
      this.cbxShowGamepad.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // txFilter
      // 
      this.txFilter.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.tableLayoutPanel5.SetColumnSpan(this.txFilter, 2);
      this.txFilter.Location = new System.Drawing.Point(151, 50);
      this.txFilter.Name = "txFilter";
      this.txFilter.Size = new System.Drawing.Size(138, 22);
      this.txFilter.TabIndex = 25;
      this.txFilter.WordWrap = false;
      this.txFilter.TextChanged += new System.EventHandler(this.txFilter_TextChanged);
      // 
      // tcXML
      // 
      this.tcXML.Controls.Add(this.tPageDump);
      this.tcXML.Controls.Add(this.tPageOther);
      this.tcXML.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tcXML.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tcXML.Location = new System.Drawing.Point(696, 126);
      this.tcXML.Name = "tcXML";
      this.tlpanel.SetRowSpan(this.tcXML, 2);
      this.tcXML.SelectedIndex = 0;
      this.tcXML.Size = new System.Drawing.Size(352, 640);
      this.tcXML.TabIndex = 30;
      // 
      // tPageDump
      // 
      this.tPageDump.Controls.Add(this.rtb);
      this.tPageDump.Location = new System.Drawing.Point(4, 22);
      this.tPageDump.Name = "tPageDump";
      this.tPageDump.Padding = new System.Windows.Forms.Padding(3);
      this.tPageDump.Size = new System.Drawing.Size(344, 614);
      this.tPageDump.TabIndex = 1;
      this.tPageDump.Tag = "§";
      this.tPageDump.Text = "Dumps (XML, Logs etc.)";
      this.tPageDump.UseVisualStyleBackColor = true;
      // 
      // tPageOther
      // 
      this.tPageOther.BackColor = System.Drawing.Color.Gainsboro;
      this.tPageOther.Controls.Add(this.tableLayoutPanel6);
      this.tPageOther.Location = new System.Drawing.Point(4, 22);
      this.tPageOther.Name = "tPageOther";
      this.tPageOther.Padding = new System.Windows.Forms.Padding(3);
      this.tPageOther.Size = new System.Drawing.Size(344, 614);
      this.tPageOther.TabIndex = 0;
      this.tPageOther.Tag = "§";
      this.tPageOther.Text = "All Mappings";
      // 
      // tableLayoutPanel6
      // 
      this.tableLayoutPanel6.ColumnCount = 1;
      this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel6.Controls.Add(this.cbxAutoTabXML, 0, 0);
      this.tableLayoutPanel6.Controls.Add(this.lbxOther, 0, 1);
      this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
      this.tableLayoutPanel6.Name = "tableLayoutPanel6";
      this.tableLayoutPanel6.RowCount = 2;
      this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel6.Size = new System.Drawing.Size(338, 608);
      this.tableLayoutPanel6.TabIndex = 0;
      // 
      // cbxAutoTabXML
      // 
      this.cbxAutoTabXML.AutoSize = true;
      this.cbxAutoTabXML.Location = new System.Drawing.Point(3, 3);
      this.cbxAutoTabXML.Name = "cbxAutoTabXML";
      this.cbxAutoTabXML.Size = new System.Drawing.Size(233, 17);
      this.cbxAutoTabXML.TabIndex = 2;
      this.cbxAutoTabXML.Tag = "§";
      this.cbxAutoTabXML.Text = "Switch XML/Mapping tab automatically";
      this.cbxAutoTabXML.UseVisualStyleBackColor = true;
      this.cbxAutoTabXML.CheckedChanged += new System.EventHandler(this.cbxAutoTabXML_CheckedChanged);
      // 
      // lbxOther
      // 
      this.lbxOther.BackColor = System.Drawing.Color.WhiteSmoke;
      this.lbxOther.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.lbxOther.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbxOther.Location = new System.Drawing.Point(3, 33);
      this.lbxOther.Name = "lbxOther";
      this.lbxOther.Size = new System.Drawing.Size(332, 572);
      this.lbxOther.TabIndex = 3;
      this.lbxOther.Text = "";
      this.lbxOther.WordWrap = false;
      // 
      // tsLblProfile
      // 
      this.tsLblProfile.BackColor = System.Drawing.Color.DarkKhaki;
      this.tsLblProfile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tsLblProfile.Name = "tsLblProfile";
      this.tsLblProfile.Size = new System.Drawing.Size(47, 17);
      this.tsLblProfile.Tag = "§";
      this.tsLblProfile.Text = "Profile:";
      // 
      // tsLblSupportedProfile
      // 
      this.tsLblSupportedProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsLblSupportedProfile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tsLblSupportedProfile.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
      this.tsLblSupportedProfile.Name = "tsLblSupportedProfile";
      this.tsLblSupportedProfile.Size = new System.Drawing.Size(839, 17);
      this.tsLblSupportedProfile.Spring = true;
      this.tsLblSupportedProfile.Tag = "§";
      this.tsLblSupportedProfile.Text = " Support:";
      this.tsLblSupportedProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLblProfile,
            this.tslblProfileUsed,
            this.tsLblSupportedProfile,
            this.tsLblSupport});
      this.statusStrip1.Location = new System.Drawing.Point(0, 869);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
      this.statusStrip1.ShowItemToolTips = true;
      this.statusStrip1.Size = new System.Drawing.Size(1054, 22);
      this.statusStrip1.TabIndex = 26;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // tslblProfileUsed
      // 
      this.tslblProfileUsed.Name = "tslblProfileUsed";
      this.tslblProfileUsed.Size = new System.Drawing.Size(69, 17);
      this.tslblProfileUsed.Text = "used profile";
      // 
      // tsLblSupport
      // 
      this.tsLblSupport.Name = "tsLblSupport";
      this.tsLblSupport.Size = new System.Drawing.Size(74, 17);
      this.tsLblSupport.Text = "tsLblSupport";
      // 
      // toolStrip1
      // 
      this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msBtLoad,
            this.toolStripSeparator2,
            this.msBtDump,
            this.toolStripSeparator5,
            this.msBtShow,
            this.toolStripSeparator6,
            this.msBtConfig,
            this.toolStripSeparator8,
            this.msBtLoadMap,
            this.msSelectMapping,
            this.tsLblMappings});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.MinimumSize = new System.Drawing.Size(0, 40);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1054, 40);
      this.toolStrip1.Stretch = true;
      this.toolStrip1.TabIndex = 27;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // msBtLoad
      // 
      this.msBtLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meResetDefaults,
            this.meResetEmpty});
      this.msBtLoad.Image = global::SCJMapper_V2.Properties.Resources.Home;
      this.msBtLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.msBtLoad.Name = "msBtLoad";
      this.msBtLoad.Size = new System.Drawing.Size(89, 37);
      this.msBtLoad.Tag = "§";
      this.msBtLoad.Text = "Reset...";
      this.msBtLoad.ToolTipText = "Reset the action tree";
      // 
      // meResetDefaults
      // 
      this.meResetDefaults.Image = global::SCJMapper_V2.Properties.Resources.RSI;
      this.meResetDefaults.Name = "meResetDefaults";
      this.meResetDefaults.Size = new System.Drawing.Size(169, 38);
      this.meResetDefaults.Tag = "§";
      this.meResetDefaults.Text = "Reset defaults !";
      this.meResetDefaults.Click += new System.EventHandler(this.meResetDefaults_Click);
      // 
      // meResetEmpty
      // 
      this.meResetEmpty.Image = global::SCJMapper_V2.Properties.Resources.NPad;
      this.meResetEmpty.Name = "meResetEmpty";
      this.meResetEmpty.Size = new System.Drawing.Size(169, 38);
      this.meResetEmpty.Tag = "§";
      this.meResetEmpty.Text = "Reset empty !";
      this.meResetEmpty.Click += new System.EventHandler(this.meResetEmpty_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 40);
      // 
      // msBtDump
      // 
      this.msBtDump.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meDumpMappingList,
            this.meDumpLogfile,
            this.meDumpDefaultProfile,
            this.meDumpActiontreeAsXML});
      this.msBtDump.Image = global::SCJMapper_V2.Properties.Resources.Info;
      this.msBtDump.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.msBtDump.Name = "msBtDump";
      this.msBtDump.Size = new System.Drawing.Size(94, 37);
      this.msBtDump.Tag = "§";
      this.msBtDump.Text = "Dump...";
      this.msBtDump.ToolTipText = "Get additional information";
      // 
      // meDumpMappingList
      // 
      this.meDumpMappingList.Name = "meDumpMappingList";
      this.meDumpMappingList.Size = new System.Drawing.Size(206, 22);
      this.meDumpMappingList.Tag = "§";
      this.meDumpMappingList.Text = "Dump Mapping List";
      this.meDumpMappingList.Click += new System.EventHandler(this.meDumpMappingList_Click);
      // 
      // meDumpLogfile
      // 
      this.meDumpLogfile.Name = "meDumpLogfile";
      this.meDumpLogfile.Size = new System.Drawing.Size(206, 22);
      this.meDumpLogfile.Tag = "§";
      this.meDumpLogfile.Text = "Dump Logfile";
      this.meDumpLogfile.Click += new System.EventHandler(this.meDumpLogfile_Click);
      // 
      // meDumpDefaultProfile
      // 
      this.meDumpDefaultProfile.Name = "meDumpDefaultProfile";
      this.meDumpDefaultProfile.Size = new System.Drawing.Size(206, 22);
      this.meDumpDefaultProfile.Tag = "§";
      this.meDumpDefaultProfile.Text = "Dump DefaultProfile";
      this.meDumpDefaultProfile.Click += new System.EventHandler(this.meDumpDefaultProfile_Click);
      // 
      // meDumpActiontreeAsXML
      // 
      this.meDumpActiontreeAsXML.Name = "meDumpActiontreeAsXML";
      this.meDumpActiontreeAsXML.Size = new System.Drawing.Size(206, 22);
      this.meDumpActiontreeAsXML.Tag = "§";
      this.meDumpActiontreeAsXML.Text = "Dump Actiontree as XML";
      this.meDumpActiontreeAsXML.Click += new System.EventHandler(this.meDumpActiontreeAsXML_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(6, 40);
      // 
      // msBtShow
      // 
      this.msBtShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meShowToggleTable,
            this.meShowOptionsDialog,
            this.meShowDeviceTuningDialog,
            this.meShowDeviceMonitoringDialog,
            this.meShowLayoutDialog});
      this.msBtShow.Image = global::SCJMapper_V2.Properties.Resources.Monitor;
      this.msBtShow.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.msBtShow.Name = "msBtShow";
      this.msBtShow.Size = new System.Drawing.Size(90, 37);
      this.msBtShow.Tag = "§";
      this.msBtShow.Text = "Show...";
      this.msBtShow.ToolTipText = "Show Options, Tuning and Tables";
      // 
      // meShowToggleTable
      // 
      this.meShowToggleTable.Name = "meShowToggleTable";
      this.meShowToggleTable.Size = new System.Drawing.Size(250, 22);
      this.meShowToggleTable.Tag = "§";
      this.meShowToggleTable.Text = "Show Toggle Table...";
      this.meShowToggleTable.Click += new System.EventHandler(this.meShowToggleTable_Click);
      // 
      // meShowOptionsDialog
      // 
      this.meShowOptionsDialog.Name = "meShowOptionsDialog";
      this.meShowOptionsDialog.Size = new System.Drawing.Size(250, 22);
      this.meShowOptionsDialog.Tag = "§";
      this.meShowOptionsDialog.Text = "Show Options Dialog...";
      this.meShowOptionsDialog.Click += new System.EventHandler(this.meShowOptionsDialog_Click);
      // 
      // meShowDeviceTuningDialog
      // 
      this.meShowDeviceTuningDialog.Name = "meShowDeviceTuningDialog";
      this.meShowDeviceTuningDialog.Size = new System.Drawing.Size(250, 22);
      this.meShowDeviceTuningDialog.Tag = "§";
      this.meShowDeviceTuningDialog.Text = "Show Device Tuning Dialog...";
      this.meShowDeviceTuningDialog.Click += new System.EventHandler(this.meShowDeviceTuningDialog_Click);
      // 
      // meShowDeviceMonitoringDialog
      // 
      this.meShowDeviceMonitoringDialog.Name = "meShowDeviceMonitoringDialog";
      this.meShowDeviceMonitoringDialog.Size = new System.Drawing.Size(250, 22);
      this.meShowDeviceMonitoringDialog.Tag = "§";
      this.meShowDeviceMonitoringDialog.Text = "Show Device Monitoring Dialog...";
      this.meShowDeviceMonitoringDialog.Click += new System.EventHandler(this.meShowDeviceMonitoringDialog_Click);
      // 
      // meShowLayoutDialog
      // 
      this.meShowLayoutDialog.Name = "meShowLayoutDialog";
      this.meShowLayoutDialog.Size = new System.Drawing.Size(250, 22);
      this.meShowLayoutDialog.Tag = "§";
      this.meShowLayoutDialog.Text = "Show Layout Dialog...";
      this.meShowLayoutDialog.Click += new System.EventHandler(this.meShowLayoutDialog_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(6, 40);
      // 
      // msBtConfig
      // 
      this.msBtConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meSettingsDialog,
            this.meJsReassignDialog});
      this.msBtConfig.Image = global::SCJMapper_V2.Properties.Resources.Settings;
      this.msBtConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.msBtConfig.Name = "msBtConfig";
      this.msBtConfig.Size = new System.Drawing.Size(97, 37);
      this.msBtConfig.Tag = "§";
      this.msBtConfig.Text = "Config...";
      this.msBtConfig.ToolTipText = "Configure the program";
      // 
      // meSettingsDialog
      // 
      this.meSettingsDialog.Name = "meSettingsDialog";
      this.meSettingsDialog.Size = new System.Drawing.Size(178, 22);
      this.meSettingsDialog.Tag = "§";
      this.meSettingsDialog.Text = "Settings Dialog...";
      this.meSettingsDialog.Click += new System.EventHandler(this.meSettingsDialog_Click);
      // 
      // meJsReassignDialog
      // 
      this.meJsReassignDialog.Name = "meJsReassignDialog";
      this.meJsReassignDialog.Size = new System.Drawing.Size(178, 22);
      this.meJsReassignDialog.Tag = "§";
      this.meJsReassignDialog.Text = "Js Reassign Dialog...";
      this.meJsReassignDialog.Click += new System.EventHandler(this.meJsReassignDialog_Click);
      // 
      // toolStripSeparator8
      // 
      this.toolStripSeparator8.Name = "toolStripSeparator8";
      this.toolStripSeparator8.Size = new System.Drawing.Size(6, 40);
      // 
      // msBtLoadMap
      // 
      this.msBtLoadMap.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.msBtLoadMap.AutoSize = false;
      this.msBtLoadMap.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.msBtLoadMap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.meDefaultsLoadAndGrab,
            this.meResetLoadAndGrab,
            this.meLoadAndGrab,
            this.meLoad});
      this.msBtLoadMap.Image = global::SCJMapper_V2.Properties.Resources.Folder;
      this.msBtLoadMap.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.msBtLoadMap.Name = "msBtLoadMap";
      this.msBtLoadMap.Size = new System.Drawing.Size(150, 37);
      this.msBtLoadMap.Tag = "§";
      this.msBtLoadMap.Text = "Load...";
      this.msBtLoadMap.ToolTipText = "Load a map with options...";
      // 
      // meDefaultsLoadAndGrab
      // 
      this.meDefaultsLoadAndGrab.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.meDefaultsLoadAndGrab.Name = "meDefaultsLoadAndGrab";
      this.meDefaultsLoadAndGrab.Size = new System.Drawing.Size(206, 22);
      this.meDefaultsLoadAndGrab.Tag = "§";
      this.meDefaultsLoadAndGrab.Text = "Defaults, Load and Grab !";
      this.meDefaultsLoadAndGrab.Click += new System.EventHandler(this.meDefaultsLoadAndGrab_Click);
      // 
      // meResetLoadAndGrab
      // 
      this.meResetLoadAndGrab.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.meResetLoadAndGrab.Name = "meResetLoadAndGrab";
      this.meResetLoadAndGrab.Size = new System.Drawing.Size(206, 22);
      this.meResetLoadAndGrab.Tag = "§";
      this.meResetLoadAndGrab.Text = "Reset, Load and Grab !";
      this.meResetLoadAndGrab.Click += new System.EventHandler(this.meResetLoadAndGrab_Click);
      // 
      // meLoadAndGrab
      // 
      this.meLoadAndGrab.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.meLoadAndGrab.Name = "meLoadAndGrab";
      this.meLoadAndGrab.Size = new System.Drawing.Size(206, 22);
      this.meLoadAndGrab.Tag = "§";
      this.meLoadAndGrab.Text = "Load and Grab !";
      this.meLoadAndGrab.Click += new System.EventHandler(this.meLoadAndGrab_Click);
      // 
      // meLoad
      // 
      this.meLoad.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.meLoad.Name = "meLoad";
      this.meLoad.Size = new System.Drawing.Size(206, 22);
      this.meLoad.Tag = "§";
      this.meLoad.Text = "Load !";
      this.meLoad.Click += new System.EventHandler(this.meLoad_Click);
      // 
      // msSelectMapping
      // 
      this.msSelectMapping.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.msSelectMapping.AutoSize = false;
      this.msSelectMapping.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.msSelectMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.msSelectMapping.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.msSelectMapping.Image = ((System.Drawing.Image)(resources.GetObject("msSelectMapping.Image")));
      this.msSelectMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.msSelectMapping.Name = "msSelectMapping";
      this.msSelectMapping.Size = new System.Drawing.Size(240, 37);
      this.msSelectMapping.Text = "Available Mappings";
      this.msSelectMapping.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.msSelectMapping.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.msSelectMapping_DropDownItemClicked);
      // 
      // tsLblMappings
      // 
      this.tsLblMappings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tsLblMappings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tsLblMappings.Name = "tsLblMappings";
      this.tsLblMappings.Size = new System.Drawing.Size(69, 37);
      this.tsLblMappings.Tag = "§";
      this.tsLblMappings.Text = "Mappings:  ";
      // 
      // IL2
      // 
      this.IL2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL2.ImageStream")));
      this.IL2.TransparentColor = System.Drawing.Color.Transparent;
      this.IL2.Images.SetKeyName(0, "User");
      this.IL2.Images.SetKeyName(1, "Locked");
      this.IL2.Images.SetKeyName(2, "RSI");
      this.IL2.Images.SetKeyName(3, "Exported");
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(1054, 891);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.tlpanel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(1070, 930);
      this.Name = "MainForm";
      this.Text = "SC Joystick Mapper";
      this.Activated += new System.EventHandler(this.MainForm_Activated);
      this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.cmCopyPaste.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.cmMouseEntry.ResumeLayout(false);
      this.cmAddDel.ResumeLayout(false);
      this.cmAddDel.PerformLayout();
      this.tc1.ResumeLayout(false);
      this.tabJS1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tlpanel.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      this.tableLayoutPanel4.ResumeLayout(false);
      this.tableLayoutPanel5.ResumeLayout(false);
      this.tableLayoutPanel5.PerformLayout();
      this.tcXML.ResumeLayout(false);
      this.tPageDump.ResumeLayout(false);
      this.tPageOther.ResumeLayout(false);
      this.tableLayoutPanel6.ResumeLayout(false);
      this.tableLayoutPanel6.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.RichTextBox rtb;
    private System.Windows.Forms.Button btGrab;
    private System.Windows.Forms.Button btDump;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btFind;
    private System.Windows.Forms.Label lblDevCtrl;
    private System.Windows.Forms.Label lblSelected;
    private System.Windows.Forms.Button btClear;
    private System.Windows.Forms.Label lblAction;
    private System.Windows.Forms.Button btAssign;
    private System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.TabControl tc1;
    private System.Windows.Forms.TabPage tabJS1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button buttonExit;
    private System.Windows.Forms.OpenFileDialog OFD;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.ContextMenuStrip cmCopyPaste;
    private System.Windows.Forms.ToolStripMenuItem tsiCopy;
    private System.Windows.Forms.ToolStripMenuItem tsiPaste;
    private System.Windows.Forms.ToolStripMenuItem tsiPReplace;
    private System.Windows.Forms.ToolStripMenuItem tsiSelAll;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem tsiOpen;
    private System.Windows.Forms.ToolStripMenuItem tsiSaveAs;
    private System.Windows.Forms.SaveFileDialog SFD;
    private System.Windows.Forms.ImageList IL;
    private Devices.Joystick.UC_JoyPanel UC_JoyPanel;
    private System.Windows.Forms.TableLayoutPanel tlpanel;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.ToolStripStatusLabel tsLblProfile;
    private System.Windows.Forms.ToolStripStatusLabel tsLblSupportedProfile;
    private System.Windows.Forms.Button btClearFilter;
    private System.Windows.Forms.TextBox txFilter;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button btSaveMyMapping;
    private System.Windows.Forms.TextBox txMappingName;
    private System.Windows.Forms.Label lblMappingname;
    private System.Windows.Forms.LinkLabel linkLblReleases;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.CheckBox cbxThrottle;
    private System.Windows.Forms.TextBox txRebind;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.TextBox lblLastJ;
    private System.Windows.Forms.Button btJsKbd;
    private System.Windows.Forms.Button btBlend;
    private System.Windows.Forms.Button btClip;
    private System.Windows.Forms.CheckBox cbxShowJoystick;
    private System.Windows.Forms.CheckBox cbxShowGamepad;
    private System.Windows.Forms.CheckBox cbxShowKeyboard;
    private System.Windows.Forms.CheckBox cbxShowMappedOnly;
    private System.Windows.Forms.Label lblActionFilter;
    private System.Windows.Forms.ContextMenuStrip cmAddDel;
    private System.Windows.Forms.ToolStripMenuItem tdiAddBinding;
    private System.Windows.Forms.ToolStripMenuItem tdiDelBinding;
    private System.Windows.Forms.ToolStripSeparator tdiSGroup2;
    private System.Windows.Forms.ToolStripMenuItem tdiBlendBinding;
    private System.Windows.Forms.ToolStripMenuItem tdiClearBinding;
    private System.Windows.Forms.ToolStripMenuItem tdiAssignBinding;
    private System.Windows.Forms.CheckBox cbxShowMouse;
    private System.Windows.Forms.ContextMenuStrip cmMouseEntry;
    private System.Windows.Forms.ToolStripMenuItem tmeXAxis;
    private System.Windows.Forms.ToolStripMenuItem tmeYAxis;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem tmeWUp;
    private System.Windows.Forms.ToolStripMenuItem tmeWDown;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripSeparator tdiSGroup3;
    private System.Windows.Forms.ToolStripMenuItem tdiAddMod1;
    private System.Windows.Forms.ToolStripMenuItem tdiAddMod2;
    private System.Windows.Forms.ToolStripMenuItem tdiAddMod3;
    private System.Windows.Forms.ToolStripComboBox tdiCbxActivation;
    private System.Windows.Forms.ToolStripSeparator tdiSGroup4;
    private System.Windows.Forms.ToolStripTextBox tdiTxDefActivationMode;
    private System.Windows.Forms.ToolStripMenuItem tmeK_Tab;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.ToolStripMenuItem tdiCollapseAll;
    private System.Windows.Forms.ToolStripSeparator tdiSGroup1;
    private System.Windows.Forms.Label lblMapping;
    private System.Windows.Forms.Label lblAssigned;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    private System.Windows.Forms.TabControl tcXML;
    private System.Windows.Forms.TabPage tPageOther;
    private System.Windows.Forms.TabPage tPageDump;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
    private System.Windows.Forms.CheckBox cbxAutoTabXML;
    private System.Windows.Forms.RichTextBox lbxOther;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripStatusLabel tslblProfileUsed;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripDropDownButton msSelectMapping;
    private System.Windows.Forms.ToolStripLabel tsLblMappings;
    private System.Windows.Forms.ToolStripDropDownButton msBtLoadMap;
    private System.Windows.Forms.ToolStripMenuItem meDefaultsLoadAndGrab;
    private System.Windows.Forms.ToolStripMenuItem meResetLoadAndGrab;
    private System.Windows.Forms.ToolStripMenuItem meLoadAndGrab;
    private System.Windows.Forms.ToolStripMenuItem meLoad;
    private System.Windows.Forms.ImageList IL2;
    private System.Windows.Forms.ToolStripDropDownButton msBtLoad;
    private System.Windows.Forms.ToolStripMenuItem meResetDefaults;
    private System.Windows.Forms.ToolStripMenuItem meResetEmpty;
    private System.Windows.Forms.ToolStripMenuItem meDumpMappingList;
    private System.Windows.Forms.ToolStripMenuItem meDumpLogfile;
    private System.Windows.Forms.ToolStripMenuItem meDumpDefaultProfile;
    private System.Windows.Forms.ToolStripMenuItem meDumpActiontreeAsXML;
    private System.Windows.Forms.ToolStripDropDownButton msBtShow;
    private System.Windows.Forms.ToolStripMenuItem meShowToggleTable;
    private System.Windows.Forms.ToolStripMenuItem meShowOptionsDialog;
    private System.Windows.Forms.ToolStripMenuItem meShowDeviceTuningDialog;
    private System.Windows.Forms.ToolStripDropDownButton msBtConfig;
    private System.Windows.Forms.ToolStripMenuItem meSettingsDialog;
    private System.Windows.Forms.ToolStripMenuItem meJsReassignDialog;
    private System.Windows.Forms.ToolStripStatusLabel tsLblSupport;
    private System.Windows.Forms.ToolStripDropDownButton msBtDump;
    private System.Windows.Forms.ToolStripMenuItem tdiExpandAll;
    private System.Windows.Forms.ToolStripMenuItem meShowDeviceMonitoringDialog;
    private System.Windows.Forms.Label lblPTU;
    private System.Windows.Forms.ToolStripMenuItem meShowLayoutDialog;
  }
}

