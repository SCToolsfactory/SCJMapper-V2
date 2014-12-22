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
      foreach ( JoystickCls js in m_Joystick ) js.FinishDX( );
      m_Joystick.Clear( );

      if ( disposing && ( components != null ) ) {
        components.Dispose( );
      }
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
      this.btDumpList = new System.Windows.Forms.Button();
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
      this.btJsKbd = new System.Windows.Forms.Button();
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.btBlend = new System.Windows.Forms.Button();
      this.lblLastJ = new System.Windows.Forms.TextBox();
      this.cbxThrottle = new System.Windows.Forms.CheckBox();
      this.btFind = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.btClear = new System.Windows.Forms.Button();
      this.lblAction = new System.Windows.Forms.Label();
      this.btAssign = new System.Windows.Forms.Button();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.cmAddDel = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tsiAddBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tdiDelBinding = new System.Windows.Forms.ToolStripMenuItem();
      this.tc1 = new System.Windows.Forms.TabControl();
      this.tabJS1 = new System.Windows.Forms.TabPage();
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
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.btJSTuning = new System.Windows.Forms.Button();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.btSettings = new System.Windows.Forms.Button();
      this.btJsReassign = new System.Windows.Forms.Button();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.btSaveMyMapping = new System.Windows.Forms.Button();
      this.btLoadMyMapping = new System.Windows.Forms.Button();
      this.txMappingName = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
      this.cbxShowJoystick = new System.Windows.Forms.CheckBox();
      this.cbxShowGamepad = new System.Windows.Forms.CheckBox();
      this.cbxShowKeyboard = new System.Windows.Forms.CheckBox();
      this.cbxShowMappedOnly = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txFilter = new System.Windows.Forms.TextBox();
      this.btClearFilter = new System.Windows.Forms.Button();
      this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
      this.cbxInvFlightPitch = new System.Windows.Forms.CheckBox();
      this.cbxInvAimPitch = new System.Windows.Forms.CheckBox();
      this.cbxInvViewPitch = new System.Windows.Forms.CheckBox();
      this.cbxInvFlightYaw = new System.Windows.Forms.CheckBox();
      this.cbxInvAimYaw = new System.Windows.Forms.CheckBox();
      this.cbxInvViewYaw = new System.Windows.Forms.CheckBox();
      this.cbxInvFlightRoll = new System.Windows.Forms.CheckBox();
      this.cbxInvThrottle = new System.Windows.Forms.CheckBox();
      this.cbxInvStrafeVert = new System.Windows.Forms.CheckBox();
      this.cbxInvStrafeLat = new System.Windows.Forms.CheckBox();
      this.cbxInvStrafeLon = new System.Windows.Forms.CheckBox();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.tsDDbtProfiles = new System.Windows.Forms.ToolStripDropDownButton();
      this.tsBtReset = new System.Windows.Forms.ToolStripDropDownButton();
      this.resetDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.resetEmptyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.tsDDbtMappings = new System.Windows.Forms.ToolStripDropDownButton();
      this.tsBtLoad = new System.Windows.Forms.ToolStripDropDownButton();
      this.defaultsLoadAndGrabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.resetLoadAndGrabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadAndGrabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.btDumpLog = new System.Windows.Forms.Button();
      this.UC_JoyPanel = new SCJMapper_V2.UC_JoyPanel();
      this.cmCopyPaste.SuspendLayout();
      this.panel2.SuspendLayout();
      this.cmAddDel.SuspendLayout();
      this.tc1.SuspendLayout();
      this.tabJS1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tlpanel.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btDumpList
      // 
      this.btDumpList.Location = new System.Drawing.Point(3, 33);
      this.btDumpList.Name = "btDumpList";
      this.btDumpList.Size = new System.Drawing.Size(120, 24);
      this.btDumpList.TabIndex = 24;
      this.btDumpList.Text = "Dump List-->";
      this.btDumpList.UseVisualStyleBackColor = true;
      this.btDumpList.Click += new System.EventHandler(this.btDumpList_Click);
      // 
      // rtb
      // 
      this.rtb.AcceptsTab = true;
      this.rtb.ContextMenuStrip = this.cmCopyPaste;
      this.rtb.DetectUrls = false;
      this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rtb.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtb.Location = new System.Drawing.Point(676, 81);
      this.rtb.Name = "rtb";
      this.rtb.Size = new System.Drawing.Size(372, 529);
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
      this.tsiCopy.Text = "Copy";
      this.tsiCopy.Click += new System.EventHandler(this.tsiCopy_Click);
      // 
      // tsiPaste
      // 
      this.tsiPaste.Name = "tsiPaste";
      this.tsiPaste.Size = new System.Drawing.Size(169, 22);
      this.tsiPaste.Text = "Paste";
      this.tsiPaste.Click += new System.EventHandler(this.tsiPaste_Click);
      // 
      // tsiPReplace
      // 
      this.tsiPReplace.Name = "tsiPReplace";
      this.tsiPReplace.Size = new System.Drawing.Size(169, 22);
      this.tsiPReplace.Text = "Paste (Replace all)";
      this.tsiPReplace.Click += new System.EventHandler(this.tsiPReplace_Click);
      // 
      // tsiSelAll
      // 
      this.tsiSelAll.Name = "tsiSelAll";
      this.tsiSelAll.Size = new System.Drawing.Size(169, 22);
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
      this.tsiOpen.Text = "Open...";
      this.tsiOpen.Click += new System.EventHandler(this.tsiOpen_Click);
      // 
      // tsiSaveAs
      // 
      this.tsiSaveAs.Name = "tsiSaveAs";
      this.tsiSaveAs.Size = new System.Drawing.Size(169, 22);
      this.tsiSaveAs.Text = "Save as...";
      this.tsiSaveAs.Click += new System.EventHandler(this.tsiSaveAs_Click);
      // 
      // btGrab
      // 
      this.btGrab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btGrab.Location = new System.Drawing.Point(171, 3);
      this.btGrab.Name = "btGrab";
      this.btGrab.Size = new System.Drawing.Size(120, 24);
      this.btGrab.TabIndex = 19;
      this.btGrab.Text = "<-- Grab XML";
      this.btGrab.UseVisualStyleBackColor = true;
      this.btGrab.Click += new System.EventHandler(this.btGrab_Click);
      // 
      // btDump
      // 
      this.btDump.Location = new System.Drawing.Point(3, 3);
      this.btDump.Name = "btDump";
      this.btDump.Size = new System.Drawing.Size(120, 24);
      this.btDump.TabIndex = 20;
      this.btDump.Text = "Dump XML-->";
      this.btDump.UseVisualStyleBackColor = true;
      this.btDump.Click += new System.EventHandler(this.btDump_Click);
      // 
      // panel2
      // 
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.btJsKbd);
      this.panel2.Controls.Add(this.btBlend);
      this.panel2.Controls.Add(this.lblLastJ);
      this.panel2.Controls.Add(this.cbxThrottle);
      this.panel2.Controls.Add(this.btFind);
      this.panel2.Controls.Add(this.label7);
      this.panel2.Controls.Add(this.label6);
      this.panel2.Controls.Add(this.btClear);
      this.panel2.Controls.Add(this.lblAction);
      this.panel2.Controls.Add(this.btAssign);
      this.panel2.Location = new System.Drawing.Point(3, 358);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(289, 142);
      this.panel2.TabIndex = 17;
      // 
      // btJsKbd
      // 
      this.btJsKbd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btJsKbd.ImageKey = "J";
      this.btJsKbd.ImageList = this.IL;
      this.btJsKbd.Location = new System.Drawing.Point(104, 112);
      this.btJsKbd.Name = "btJsKbd";
      this.btJsKbd.Size = new System.Drawing.Size(79, 25);
      this.btJsKbd.TabIndex = 16;
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
      this.IL.Images.SetKeyName(4, "X");
      this.IL.Images.SetKeyName(5, "P");
      this.IL.Images.SetKeyName(6, "Z");
      this.IL.Images.SetKeyName(7, "Add");
      // 
      // btBlend
      // 
      this.btBlend.Location = new System.Drawing.Point(10, 112);
      this.btBlend.Name = "btBlend";
      this.btBlend.Size = new System.Drawing.Size(73, 25);
      this.btBlend.TabIndex = 15;
      this.btBlend.Text = "Blend";
      this.btBlend.UseVisualStyleBackColor = true;
      this.btBlend.Click += new System.EventHandler(this.btBlend_Click);
      // 
      // lblLastJ
      // 
      this.lblLastJ.Location = new System.Drawing.Point(52, 38);
      this.lblLastJ.Name = "lblLastJ";
      this.lblLastJ.Size = new System.Drawing.Size(222, 22);
      this.lblLastJ.TabIndex = 14;
      this.lblLastJ.Text = "...";
      this.lblLastJ.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lblLastJ_MouseClick);
      this.lblLastJ.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lblLastJ_KeyDown);
      this.lblLastJ.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lblLastJ_MouseDoubleClick);
      // 
      // cbxThrottle
      // 
      this.cbxThrottle.AutoSize = true;
      this.cbxThrottle.Location = new System.Drawing.Point(89, 72);
      this.cbxThrottle.Name = "cbxThrottle";
      this.cbxThrottle.Size = new System.Drawing.Size(66, 17);
      this.cbxThrottle.TabIndex = 13;
      this.cbxThrottle.Text = "Throttle";
      this.cbxThrottle.UseVisualStyleBackColor = true;
      // 
      // btFind
      // 
      this.btFind.Location = new System.Drawing.Point(201, 67);
      this.btFind.Name = "btFind";
      this.btFind.Size = new System.Drawing.Size(73, 25);
      this.btFind.TabIndex = 12;
      this.btFind.Text = "Find 1st.";
      this.btFind.UseVisualStyleBackColor = true;
      this.btFind.Click += new System.EventHandler(this.btFind_Click);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(7, 41);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(28, 13);
      this.label7.TabIndex = 3;
      this.label7.Text = "Ctrl.";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(7, 17);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(33, 13);
      this.label6.TabIndex = 3;
      this.label6.Text = "Cmd.";
      // 
      // btClear
      // 
      this.btClear.Location = new System.Drawing.Point(201, 112);
      this.btClear.Name = "btClear";
      this.btClear.Size = new System.Drawing.Size(73, 25);
      this.btClear.TabIndex = 2;
      this.btClear.Text = "Clear";
      this.btClear.UseVisualStyleBackColor = true;
      this.btClear.Click += new System.EventHandler(this.btClear_Click);
      // 
      // lblAction
      // 
      this.lblAction.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblAction.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAction.Location = new System.Drawing.Point(52, 17);
      this.lblAction.Name = "lblAction";
      this.lblAction.Size = new System.Drawing.Size(222, 20);
      this.lblAction.TabIndex = 1;
      this.lblAction.Text = "...";
      // 
      // btAssign
      // 
      this.btAssign.Location = new System.Drawing.Point(10, 67);
      this.btAssign.Name = "btAssign";
      this.btAssign.Size = new System.Drawing.Size(73, 25);
      this.btAssign.TabIndex = 0;
      this.btAssign.Text = "Assign";
      this.btAssign.UseVisualStyleBackColor = true;
      this.btAssign.Click += new System.EventHandler(this.btAssign_Click);
      // 
      // treeView1
      // 
      this.treeView1.ContextMenuStrip = this.cmAddDel;
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.HotTracking = true;
      this.treeView1.ImageKey = "Map";
      this.treeView1.ImageList = this.IL;
      this.treeView1.Location = new System.Drawing.Point(6, 81);
      this.treeView1.Name = "treeView1";
      this.tlpanel.SetRowSpan(this.treeView1, 2);
      this.treeView1.SelectedImageKey = "Selected";
      this.treeView1.Size = new System.Drawing.Size(364, 686);
      this.treeView1.TabIndex = 16;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      // 
      // cmAddDel
      // 
      this.cmAddDel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiAddBinding,
            this.tdiDelBinding});
      this.cmAddDel.Name = "cmAddDel";
      this.cmAddDel.Size = new System.Drawing.Size(159, 48);
      this.cmAddDel.Opening += new System.ComponentModel.CancelEventHandler(this.cmAddDel_Opening);
      // 
      // tsiAddBinding
      // 
      this.tsiAddBinding.Name = "tsiAddBinding";
      this.tsiAddBinding.Size = new System.Drawing.Size(158, 22);
      this.tsiAddBinding.Text = "Add Mapping";
      this.tsiAddBinding.Click += new System.EventHandler(this.tsiAddBinding_Click);
      // 
      // tdiDelBinding
      // 
      this.tdiDelBinding.Name = "tdiDelBinding";
      this.tdiDelBinding.Size = new System.Drawing.Size(158, 22);
      this.tdiDelBinding.Text = "Delete Mapping";
      this.tdiDelBinding.Click += new System.EventHandler(this.tdiDelBinding_Click);
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
      this.tc1.Size = new System.Drawing.Size(289, 349);
      this.tc1.TabIndex = 15;
      this.tc1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tc1_DrawItem);
      // 
      // tabJS1
      // 
      this.tabJS1.Controls.Add(this.UC_JoyPanel);
      this.tabJS1.Location = new System.Drawing.Point(4, 24);
      this.tabJS1.Name = "tabJS1";
      this.tabJS1.Padding = new System.Windows.Forms.Padding(3);
      this.tabJS1.Size = new System.Drawing.Size(281, 321);
      this.tabJS1.TabIndex = 0;
      this.tabJS1.Text = "Joystick 1";
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
      this.panel1.Location = new System.Drawing.Point(6, 6);
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
      this.linkLblReleases.Location = new System.Drawing.Point(672, 17);
      this.linkLblReleases.Name = "linkLblReleases";
      this.linkLblReleases.Size = new System.Drawing.Size(259, 13);
      this.linkLblReleases.TabIndex = 3;
      this.linkLblReleases.TabStop = true;
      this.linkLblReleases.Text = "For information and updates visit us @ Github ...";
      this.linkLblReleases.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblReleases_LinkClicked);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(353, 42);
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
      this.buttonExit.Location = new System.Drawing.Point(171, 51);
      this.buttonExit.Name = "buttonExit";
      this.buttonExit.Size = new System.Drawing.Size(120, 24);
      this.buttonExit.TabIndex = 13;
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
      this.tlpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 370F));
      this.tlpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
      this.tlpanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlpanel.Controls.Add(this.rtb, 2, 1);
      this.tlpanel.Controls.Add(this.panel1, 0, 0);
      this.tlpanel.Controls.Add(this.treeView1, 0, 1);
      this.tlpanel.Controls.Add(this.flowLayoutPanel1, 1, 1);
      this.tlpanel.Controls.Add(this.tableLayoutPanel1, 1, 2);
      this.tlpanel.Controls.Add(this.tableLayoutPanel2, 1, 3);
      this.tlpanel.Controls.Add(this.tableLayoutPanel3, 2, 3);
      this.tlpanel.Controls.Add(this.flowLayoutPanel2, 0, 3);
      this.tlpanel.Controls.Add(this.flowLayoutPanel3, 2, 2);
      this.tlpanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tlpanel.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tlpanel.Location = new System.Drawing.Point(0, 0);
      this.tlpanel.Name = "tlpanel";
      this.tlpanel.Padding = new System.Windows.Forms.Padding(3);
      this.tlpanel.RowCount = 5;
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 535F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
      this.tlpanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
      this.tlpanel.Size = new System.Drawing.Size(1054, 892);
      this.tlpanel.TabIndex = 25;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.tc1);
      this.flowLayoutPanel1.Controls.Add(this.panel2);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(376, 81);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(294, 529);
      this.flowLayoutPanel1.TabIndex = 22;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Controls.Add(this.btDump, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.btGrab, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.btDumpList, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.btDumpLog, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.btJSTuning, 0, 4);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(376, 616);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 5;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(294, 151);
      this.tableLayoutPanel1.TabIndex = 23;
      // 
      // btJSTuning
      // 
      this.btJSTuning.Location = new System.Drawing.Point(3, 123);
      this.btJSTuning.Name = "btJSTuning";
      this.btJSTuning.Size = new System.Drawing.Size(120, 24);
      this.btJSTuning.TabIndex = 17;
      this.btJSTuning.Text = "Device Tuning";
      this.btJSTuning.Click += new System.EventHandler(this.btJSTuning_Click);
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.Controls.Add(this.buttonExit, 1, 1);
      this.tableLayoutPanel2.Controls.Add(this.btSettings, 0, 1);
      this.tableLayoutPanel2.Controls.Add(this.btJsReassign, 0, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(376, 773);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(294, 78);
      this.tableLayoutPanel2.TabIndex = 24;
      // 
      // btSettings
      // 
      this.btSettings.Location = new System.Drawing.Point(3, 51);
      this.btSettings.Name = "btSettings";
      this.btSettings.Size = new System.Drawing.Size(120, 24);
      this.btSettings.TabIndex = 14;
      this.btSettings.Text = "Settings...";
      this.btSettings.Click += new System.EventHandler(this.btSettings_Click);
      // 
      // btJsReassign
      // 
      this.btJsReassign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btJsReassign.Location = new System.Drawing.Point(3, 21);
      this.btJsReassign.Name = "btJsReassign";
      this.btJsReassign.Size = new System.Drawing.Size(120, 24);
      this.btJsReassign.TabIndex = 16;
      this.btJsReassign.Text = "Js Reassign...";
      this.btJsReassign.Click += new System.EventHandler(this.btJsReassign_Click);
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
      this.tableLayoutPanel3.Controls.Add(this.btSaveMyMapping, 1, 1);
      this.tableLayoutPanel3.Controls.Add(this.btLoadMyMapping, 0, 1);
      this.tableLayoutPanel3.Controls.Add(this.txMappingName, 1, 0);
      this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
      this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel3.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel3.Location = new System.Drawing.Point(676, 773);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 2;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(372, 78);
      this.tableLayoutPanel3.TabIndex = 25;
      // 
      // btSaveMyMapping
      // 
      this.btSaveMyMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btSaveMyMapping.Image = ((System.Drawing.Image)(resources.GetObject("btSaveMyMapping.Image")));
      this.btSaveMyMapping.Location = new System.Drawing.Point(164, 51);
      this.btSaveMyMapping.Name = "btSaveMyMapping";
      this.btSaveMyMapping.Size = new System.Drawing.Size(205, 24);
      this.btSaveMyMapping.TabIndex = 15;
      this.btSaveMyMapping.Text = "Dump and Save my Mapping";
      this.btSaveMyMapping.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.btSaveMyMapping.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btSaveMyMapping.Click += new System.EventHandler(this.btSaveMyMapping_Click);
      // 
      // btLoadMyMapping
      // 
      this.btLoadMyMapping.Location = new System.Drawing.Point(3, 51);
      this.btLoadMyMapping.Name = "btLoadMyMapping";
      this.btLoadMyMapping.Size = new System.Drawing.Size(120, 24);
      this.btLoadMyMapping.TabIndex = 14;
      this.btLoadMyMapping.Text = "Load my Mapping";
      this.btLoadMyMapping.Visible = false;
      this.btLoadMyMapping.Click += new System.EventHandler(this.btLoadMyMapping_Click);
      // 
      // txMappingName
      // 
      this.txMappingName.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.txMappingName.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
      this.txMappingName.Location = new System.Drawing.Point(135, 13);
      this.txMappingName.Name = "txMappingName";
      this.txMappingName.Size = new System.Drawing.Size(234, 22);
      this.txMappingName.TabIndex = 0;
      this.txMappingName.WordWrap = false;
      this.txMappingName.TextChanged += new System.EventHandler(this.txMappingName_TextChanged);
      // 
      // label1
      // 
      this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(41, 17);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(88, 13);
      this.label1.TabIndex = 16;
      this.label1.Text = "Mapping name:";
      // 
      // flowLayoutPanel2
      // 
      this.flowLayoutPanel2.Controls.Add(this.cbxShowJoystick);
      this.flowLayoutPanel2.Controls.Add(this.cbxShowGamepad);
      this.flowLayoutPanel2.Controls.Add(this.cbxShowKeyboard);
      this.flowLayoutPanel2.Controls.Add(this.cbxShowMappedOnly);
      this.flowLayoutPanel2.Controls.Add(this.label2);
      this.flowLayoutPanel2.Controls.Add(this.txFilter);
      this.flowLayoutPanel2.Controls.Add(this.btClearFilter);
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(6, 773);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new System.Drawing.Size(364, 78);
      this.flowLayoutPanel2.TabIndex = 26;
      // 
      // cbxShowJoystick
      // 
      this.cbxShowJoystick.AutoSize = true;
      this.cbxShowJoystick.Checked = true;
      this.cbxShowJoystick.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowJoystick.Location = new System.Drawing.Point(3, 3);
      this.cbxShowJoystick.Name = "cbxShowJoystick";
      this.cbxShowJoystick.Size = new System.Drawing.Size(65, 17);
      this.cbxShowJoystick.TabIndex = 0;
      this.cbxShowJoystick.Text = "Joystick";
      this.cbxShowJoystick.UseVisualStyleBackColor = true;
      this.cbxShowJoystick.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowGamepad
      // 
      this.cbxShowGamepad.AutoSize = true;
      this.cbxShowGamepad.Checked = true;
      this.cbxShowGamepad.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowGamepad.Location = new System.Drawing.Point(74, 3);
      this.cbxShowGamepad.Name = "cbxShowGamepad";
      this.cbxShowGamepad.Size = new System.Drawing.Size(75, 17);
      this.cbxShowGamepad.TabIndex = 1;
      this.cbxShowGamepad.Text = "Gamepad";
      this.cbxShowGamepad.UseVisualStyleBackColor = true;
      this.cbxShowGamepad.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowKeyboard
      // 
      this.cbxShowKeyboard.AutoSize = true;
      this.cbxShowKeyboard.Checked = true;
      this.cbxShowKeyboard.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxShowKeyboard.Location = new System.Drawing.Point(155, 3);
      this.cbxShowKeyboard.Name = "cbxShowKeyboard";
      this.cbxShowKeyboard.Size = new System.Drawing.Size(74, 17);
      this.cbxShowKeyboard.TabIndex = 1;
      this.cbxShowKeyboard.Text = "Keyboard";
      this.cbxShowKeyboard.UseVisualStyleBackColor = true;
      this.cbxShowKeyboard.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // cbxShowMappedOnly
      // 
      this.cbxShowMappedOnly.AutoSize = true;
      this.cbxShowMappedOnly.Location = new System.Drawing.Point(235, 3);
      this.cbxShowMappedOnly.Name = "cbxShowMappedOnly";
      this.cbxShowMappedOnly.Size = new System.Drawing.Size(94, 17);
      this.cbxShowMappedOnly.TabIndex = 1;
      this.cbxShowMappedOnly.Text = "Mapped only";
      this.cbxShowMappedOnly.UseVisualStyleBackColor = true;
      this.cbxShowMappedOnly.CheckedChanged += new System.EventHandler(this.cbxShowTreeOptions_CheckedChanged);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(3, 26);
      this.label2.Margin = new System.Windows.Forms.Padding(3);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(83, 24);
      this.label2.TabIndex = 27;
      this.label2.Text = "Action Filter:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txFilter
      // 
      this.txFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txFilter.Location = new System.Drawing.Point(92, 28);
      this.txFilter.Name = "txFilter";
      this.txFilter.Size = new System.Drawing.Size(120, 22);
      this.txFilter.TabIndex = 25;
      this.txFilter.WordWrap = false;
      this.txFilter.TextChanged += new System.EventHandler(this.txFilter_TextChanged);
      // 
      // btClearFilter
      // 
      this.btClearFilter.Location = new System.Drawing.Point(218, 26);
      this.btClearFilter.Name = "btClearFilter";
      this.btClearFilter.Size = new System.Drawing.Size(120, 24);
      this.btClearFilter.TabIndex = 26;
      this.btClearFilter.Text = "Clear Filter";
      this.btClearFilter.UseVisualStyleBackColor = true;
      this.btClearFilter.Click += new System.EventHandler(this.btClearFilter_Click);
      // 
      // flowLayoutPanel3
      // 
      this.flowLayoutPanel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.flowLayoutPanel3.Controls.Add(this.cbxInvFlightPitch);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvAimPitch);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvViewPitch);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvFlightYaw);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvAimYaw);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvViewYaw);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvFlightRoll);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvThrottle);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvStrafeVert);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvStrafeLat);
      this.flowLayoutPanel3.Controls.Add(this.cbxInvStrafeLon);
      this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flowLayoutPanel3.Location = new System.Drawing.Point(676, 616);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Size = new System.Drawing.Size(372, 151);
      this.flowLayoutPanel3.TabIndex = 27;
      // 
      // cbxInvFlightPitch
      // 
      this.cbxInvFlightPitch.Location = new System.Drawing.Point(3, 3);
      this.cbxInvFlightPitch.Name = "cbxInvFlightPitch";
      this.cbxInvFlightPitch.Size = new System.Drawing.Size(168, 18);
      this.cbxInvFlightPitch.TabIndex = 0;
      this.cbxInvFlightPitch.Text = "Inv. Flight Pitch";
      this.cbxInvFlightPitch.UseVisualStyleBackColor = true;
      // 
      // cbxInvAimPitch
      // 
      this.cbxInvAimPitch.Location = new System.Drawing.Point(3, 27);
      this.cbxInvAimPitch.Name = "cbxInvAimPitch";
      this.cbxInvAimPitch.Size = new System.Drawing.Size(168, 18);
      this.cbxInvAimPitch.TabIndex = 0;
      this.cbxInvAimPitch.Text = "Inv. Aim Pitch";
      this.cbxInvAimPitch.UseVisualStyleBackColor = true;
      // 
      // cbxInvViewPitch
      // 
      this.cbxInvViewPitch.Location = new System.Drawing.Point(3, 51);
      this.cbxInvViewPitch.Name = "cbxInvViewPitch";
      this.cbxInvViewPitch.Size = new System.Drawing.Size(168, 18);
      this.cbxInvViewPitch.TabIndex = 0;
      this.cbxInvViewPitch.Text = "Inv. View Pitch";
      this.cbxInvViewPitch.UseVisualStyleBackColor = true;
      // 
      // cbxInvFlightYaw
      // 
      this.cbxInvFlightYaw.Location = new System.Drawing.Point(3, 75);
      this.cbxInvFlightYaw.Name = "cbxInvFlightYaw";
      this.cbxInvFlightYaw.Size = new System.Drawing.Size(168, 18);
      this.cbxInvFlightYaw.TabIndex = 0;
      this.cbxInvFlightYaw.Text = "Inv. Flight Yaw";
      this.cbxInvFlightYaw.UseVisualStyleBackColor = true;
      // 
      // cbxInvAimYaw
      // 
      this.cbxInvAimYaw.Location = new System.Drawing.Point(3, 99);
      this.cbxInvAimYaw.Name = "cbxInvAimYaw";
      this.cbxInvAimYaw.Size = new System.Drawing.Size(168, 18);
      this.cbxInvAimYaw.TabIndex = 0;
      this.cbxInvAimYaw.Text = "Inv. Aim Yaw";
      this.cbxInvAimYaw.UseVisualStyleBackColor = true;
      // 
      // cbxInvViewYaw
      // 
      this.cbxInvViewYaw.Location = new System.Drawing.Point(3, 123);
      this.cbxInvViewYaw.Name = "cbxInvViewYaw";
      this.cbxInvViewYaw.Size = new System.Drawing.Size(168, 18);
      this.cbxInvViewYaw.TabIndex = 0;
      this.cbxInvViewYaw.Text = "Inv. View Yaw";
      this.cbxInvViewYaw.UseVisualStyleBackColor = true;
      // 
      // cbxInvFlightRoll
      // 
      this.cbxInvFlightRoll.Location = new System.Drawing.Point(177, 3);
      this.cbxInvFlightRoll.Name = "cbxInvFlightRoll";
      this.cbxInvFlightRoll.Size = new System.Drawing.Size(168, 18);
      this.cbxInvFlightRoll.TabIndex = 0;
      this.cbxInvFlightRoll.Text = "Inv. Flight Roll";
      this.cbxInvFlightRoll.UseVisualStyleBackColor = true;
      // 
      // cbxInvThrottle
      // 
      this.cbxInvThrottle.Location = new System.Drawing.Point(177, 27);
      this.cbxInvThrottle.Name = "cbxInvThrottle";
      this.cbxInvThrottle.Size = new System.Drawing.Size(168, 18);
      this.cbxInvThrottle.TabIndex = 0;
      this.cbxInvThrottle.Text = "Inv. Throttle";
      this.cbxInvThrottle.UseVisualStyleBackColor = true;
      // 
      // cbxInvStrafeVert
      // 
      this.cbxInvStrafeVert.Location = new System.Drawing.Point(177, 51);
      this.cbxInvStrafeVert.Name = "cbxInvStrafeVert";
      this.cbxInvStrafeVert.Size = new System.Drawing.Size(168, 18);
      this.cbxInvStrafeVert.TabIndex = 0;
      this.cbxInvStrafeVert.Text = "Inv. Strafe vertical";
      this.cbxInvStrafeVert.UseVisualStyleBackColor = true;
      // 
      // cbxInvStrafeLat
      // 
      this.cbxInvStrafeLat.Location = new System.Drawing.Point(177, 75);
      this.cbxInvStrafeLat.Name = "cbxInvStrafeLat";
      this.cbxInvStrafeLat.Size = new System.Drawing.Size(168, 18);
      this.cbxInvStrafeLat.TabIndex = 0;
      this.cbxInvStrafeLat.Text = "Inv. Strafe lateral";
      this.cbxInvStrafeLat.UseVisualStyleBackColor = true;
      // 
      // cbxInvStrafeLon
      // 
      this.cbxInvStrafeLon.Location = new System.Drawing.Point(177, 99);
      this.cbxInvStrafeLon.Name = "cbxInvStrafeLon";
      this.cbxInvStrafeLon.Size = new System.Drawing.Size(168, 18);
      this.cbxInvStrafeLon.TabIndex = 0;
      this.cbxInvStrafeLon.Text = "Inv. Strafe longitudinal";
      this.cbxInvStrafeLon.UseVisualStyleBackColor = true;
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.BackColor = System.Drawing.Color.DarkKhaki;
      this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(52, 25);
      this.toolStripStatusLabel2.Text = "Profiles:";
      // 
      // tsDDbtProfiles
      // 
      this.tsDDbtProfiles.AutoSize = false;
      this.tsDDbtProfiles.BackColor = System.Drawing.Color.DarkKhaki;
      this.tsDDbtProfiles.Image = ((System.Drawing.Image)(resources.GetObject("tsDDbtProfiles.Image")));
      this.tsDDbtProfiles.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsDDbtProfiles.Name = "tsDDbtProfiles";
      this.tsDDbtProfiles.Size = new System.Drawing.Size(250, 28);
      this.tsDDbtProfiles.Text = "Available Profiles";
      this.tsDDbtProfiles.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsDDbtProfiles_DropDownItemClicked);
      // 
      // tsBtReset
      // 
      this.tsBtReset.AutoSize = false;
      this.tsBtReset.BackColor = System.Drawing.Color.DarkKhaki;
      this.tsBtReset.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetDefaultsToolStripMenuItem,
            this.resetEmptyToolStripMenuItem});
      this.tsBtReset.Image = ((System.Drawing.Image)(resources.GetObject("tsBtReset.Image")));
      this.tsBtReset.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsBtReset.Name = "tsBtReset";
      this.tsBtReset.Size = new System.Drawing.Size(100, 28);
      this.tsBtReset.Text = "Reset...";
      this.tsBtReset.ToolTipText = "Reset with chosen options";
      // 
      // resetDefaultsToolStripMenuItem
      // 
      this.resetDefaultsToolStripMenuItem.BackColor = System.Drawing.Color.DarkKhaki;
      this.resetDefaultsToolStripMenuItem.Name = "resetDefaultsToolStripMenuItem";
      this.resetDefaultsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.resetDefaultsToolStripMenuItem.Text = "Reset defaults !";
      this.resetDefaultsToolStripMenuItem.Click += new System.EventHandler(this.resetDefaultsToolStripMenuItem_Click);
      // 
      // resetEmptyToolStripMenuItem
      // 
      this.resetEmptyToolStripMenuItem.BackColor = System.Drawing.Color.DarkKhaki;
      this.resetEmptyToolStripMenuItem.Name = "resetEmptyToolStripMenuItem";
      this.resetEmptyToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.resetEmptyToolStripMenuItem.Text = "Reset empty !";
      this.resetEmptyToolStripMenuItem.Click += new System.EventHandler(this.resetEmptyToolStripMenuItem_Click);
      // 
      // toolStripStatusLabel3
      // 
      this.toolStripStatusLabel3.AutoSize = false;
      this.toolStripStatusLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
      this.toolStripStatusLabel3.Size = new System.Drawing.Size(100, 25);
      this.toolStripStatusLabel3.Text = "                            ";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(63, 25);
      this.toolStripStatusLabel1.Text = "Mappings:";
      // 
      // tsDDbtMappings
      // 
      this.tsDDbtMappings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tsDDbtMappings.AutoSize = false;
      this.tsDDbtMappings.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.tsDDbtMappings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsDDbtMappings.Image = ((System.Drawing.Image)(resources.GetObject("tsDDbtMappings.Image")));
      this.tsDDbtMappings.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsDDbtMappings.Name = "tsDDbtMappings";
      this.tsDDbtMappings.Size = new System.Drawing.Size(250, 28);
      this.tsDDbtMappings.Text = "Available Mappings";
      this.tsDDbtMappings.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsDDbtMappings_DropDownItemClicked);
      // 
      // tsBtLoad
      // 
      this.tsBtLoad.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.tsBtLoad.AutoSize = false;
      this.tsBtLoad.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.tsBtLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultsLoadAndGrabToolStripMenuItem,
            this.resetLoadAndGrabToolStripMenuItem,
            this.loadAndGrabToolStripMenuItem,
            this.loadToolStripMenuItem});
      this.tsBtLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsBtLoad.Image")));
      this.tsBtLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsBtLoad.Name = "tsBtLoad";
      this.tsBtLoad.Size = new System.Drawing.Size(80, 28);
      this.tsBtLoad.Text = "Load...";
      // 
      // defaultsLoadAndGrabToolStripMenuItem
      // 
      this.defaultsLoadAndGrabToolStripMenuItem.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.defaultsLoadAndGrabToolStripMenuItem.Name = "defaultsLoadAndGrabToolStripMenuItem";
      this.defaultsLoadAndGrabToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
      this.defaultsLoadAndGrabToolStripMenuItem.Text = "Defaults, Load and Grab !";
      this.defaultsLoadAndGrabToolStripMenuItem.Click += new System.EventHandler(this.defaultsLoadAndGrabToolStripMenuItem_Click);
      // 
      // resetLoadAndGrabToolStripMenuItem
      // 
      this.resetLoadAndGrabToolStripMenuItem.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.resetLoadAndGrabToolStripMenuItem.Name = "resetLoadAndGrabToolStripMenuItem";
      this.resetLoadAndGrabToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
      this.resetLoadAndGrabToolStripMenuItem.Text = "Reset, Load and Grab !";
      this.resetLoadAndGrabToolStripMenuItem.Click += new System.EventHandler(this.resetLoadAndGrabToolStripMenuItem_Click);
      // 
      // loadAndGrabToolStripMenuItem
      // 
      this.loadAndGrabToolStripMenuItem.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.loadAndGrabToolStripMenuItem.Name = "loadAndGrabToolStripMenuItem";
      this.loadAndGrabToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
      this.loadAndGrabToolStripMenuItem.Text = "Load and Grab !";
      this.loadAndGrabToolStripMenuItem.Click += new System.EventHandler(this.loadAndGrabToolStripMenuItem_Click);
      // 
      // loadToolStripMenuItem
      // 
      this.loadToolStripMenuItem.BackColor = System.Drawing.Color.DarkSeaGreen;
      this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
      this.loadToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
      this.loadToolStripMenuItem.Text = "Load !";
      this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.tsDDbtProfiles,
            this.tsBtReset,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel1,
            this.tsDDbtMappings,
            this.tsBtLoad});
      this.statusStrip1.Location = new System.Drawing.Point(0, 862);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
      this.statusStrip1.ShowItemToolTips = true;
      this.statusStrip1.Size = new System.Drawing.Size(1054, 30);
      this.statusStrip1.TabIndex = 26;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // btDumpLog
      // 
      this.btDumpLog.Location = new System.Drawing.Point(3, 63);
      this.btDumpLog.Name = "btDumpLog";
      this.btDumpLog.Size = new System.Drawing.Size(120, 24);
      this.btDumpLog.TabIndex = 25;
      this.btDumpLog.Text = "Dump Log-->";
      this.btDumpLog.UseVisualStyleBackColor = true;
      this.btDumpLog.Click += new System.EventHandler(this.btDumpLog_Click);
      // 
      // UC_JoyPanel
      // 
      this.UC_JoyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.UC_JoyPanel.JsAssignment = 0;
      this.UC_JoyPanel.Location = new System.Drawing.Point(3, 3);
      this.UC_JoyPanel.Name = "UC_JoyPanel";
      this.UC_JoyPanel.Size = new System.Drawing.Size(275, 315);
      this.UC_JoyPanel.TabIndex = 0;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1054, 892);
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
      this.cmAddDel.ResumeLayout(false);
      this.tc1.ResumeLayout(false);
      this.tabJS1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tlpanel.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel3.ResumeLayout(false);
      this.tableLayoutPanel3.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.flowLayoutPanel2.PerformLayout();
      this.flowLayoutPanel3.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btDumpList;
    private System.Windows.Forms.RichTextBox rtb;
    private System.Windows.Forms.Button btGrab;
    private System.Windows.Forms.Button btDump;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btFind;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
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
    private UC_JoyPanel UC_JoyPanel;
    private System.Windows.Forms.TableLayoutPanel tlpanel;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.ToolStripDropDownButton tsDDbtProfiles;
    private System.Windows.Forms.ToolStripDropDownButton tsDDbtMappings;
    private System.Windows.Forms.ToolStripDropDownButton tsBtReset;
    private System.Windows.Forms.ToolStripMenuItem resetDefaultsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem resetEmptyToolStripMenuItem;
    private System.Windows.Forms.ToolStripDropDownButton tsBtLoad;
    private System.Windows.Forms.ToolStripMenuItem loadAndGrabToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem resetLoadAndGrabToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem defaultsLoadAndGrabToolStripMenuItem;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
    private System.Windows.Forms.Button btClearFilter;
    private System.Windows.Forms.TextBox txFilter;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button btSaveMyMapping;
    private System.Windows.Forms.Button btLoadMyMapping;
    private System.Windows.Forms.TextBox txMappingName;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.LinkLabel linkLblReleases;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.CheckBox cbxThrottle;
    private System.Windows.Forms.TextBox txRebind;
    private System.Windows.Forms.Button btSettings;
    private System.Windows.Forms.Button btJsReassign;
    private System.Windows.Forms.Button btJSTuning;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.TextBox lblLastJ;
    private System.Windows.Forms.Button btJsKbd;
    private System.Windows.Forms.Button btBlend;
    private System.Windows.Forms.Button btClip;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.CheckBox cbxShowJoystick;
    private System.Windows.Forms.CheckBox cbxShowGamepad;
    private System.Windows.Forms.CheckBox cbxShowKeyboard;
    private System.Windows.Forms.CheckBox cbxShowMappedOnly;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ContextMenuStrip cmAddDel;
    private System.Windows.Forms.ToolStripMenuItem tsiAddBinding;
    private System.Windows.Forms.ToolStripMenuItem tdiDelBinding;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    private System.Windows.Forms.CheckBox cbxInvFlightPitch;
    private System.Windows.Forms.CheckBox cbxInvFlightYaw;
    private System.Windows.Forms.CheckBox cbxInvAimPitch;
    private System.Windows.Forms.CheckBox cbxInvViewPitch;
    private System.Windows.Forms.CheckBox cbxInvAimYaw;
    private System.Windows.Forms.CheckBox cbxInvViewYaw;
    private System.Windows.Forms.CheckBox cbxInvFlightRoll;
    private System.Windows.Forms.CheckBox cbxInvStrafeVert;
    private System.Windows.Forms.CheckBox cbxInvStrafeLat;
    private System.Windows.Forms.CheckBox cbxInvStrafeLon;
    private System.Windows.Forms.CheckBox cbxInvThrottle;
    private System.Windows.Forms.Button btDumpLog;
  }
}

