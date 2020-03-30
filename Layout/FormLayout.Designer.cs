namespace SCJMapper_V2.Layout
{
  partial class FormLayout
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
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLayout));
      this.tlPanel = new System.Windows.Forms.TableLayoutPanel();
      this.chkLbActionGroups = new System.Windows.Forms.ListView();
      this.chkLbActionMaps = new System.Windows.Forms.ListView();
      this.pnlInput = new System.Windows.Forms.Panel();
      this.btClose = new System.Windows.Forms.Button();
      this.pnlDebug = new System.Windows.Forms.Panel();
      this.label2 = new System.Windows.Forms.Label();
      this.cbxJs4 = new System.Windows.Forms.ComboBox();
      this.btActivateDevices = new System.Windows.Forms.Button();
      this.cbxJs3 = new System.Windows.Forms.ComboBox();
      this.cbxJs2 = new System.Windows.Forms.ComboBox();
      this.cbxJs1 = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.btCreateDbgList = new System.Windows.Forms.Button();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.btLayout = new System.Windows.Forms.Button();
      this.cbxLayouts = new System.Windows.Forms.ComboBox();
      this.drawPanel = new System.Windows.Forms.Panel();
      this.gbxColors = new System.Windows.Forms.GroupBox();
      this.lblFontSize = new System.Windows.Forms.Label();
      this.tbFontSize = new System.Windows.Forms.TrackBar();
      this.lblLayoutNote = new System.Windows.Forms.Label();
      this.btAccept = new System.Windows.Forms.Button();
      this.lblTextColor = new System.Windows.Forms.Label();
      this.lblTest = new System.Windows.Forms.Label();
      this.lblBackColor = new System.Windows.Forms.Label();
      this.chkLbActionGroupsColor = new System.Windows.Forms.ListView();
      this.btCancel = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btSave = new System.Windows.Forms.Button();
      this.btColors = new System.Windows.Forms.Button();
      this.SFD = new System.Windows.Forms.SaveFileDialog();
      this.colDlg = new System.Windows.Forms.ColorDialog();
      this.tlPanel.SuspendLayout();
      this.pnlInput.SuspendLayout();
      this.pnlDebug.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.drawPanel.SuspendLayout();
      this.gbxColors.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tbFontSize)).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tlPanel
      // 
      this.tlPanel.ColumnCount = 2;
      this.tlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
      this.tlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlPanel.Controls.Add(this.chkLbActionGroups, 0, 1);
      this.tlPanel.Controls.Add(this.chkLbActionMaps, 0, 2);
      this.tlPanel.Controls.Add(this.pnlInput, 0, 0);
      this.tlPanel.Controls.Add(this.drawPanel, 1, 1);
      this.tlPanel.Controls.Add(this.panel1, 0, 3);
      this.tlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tlPanel.Location = new System.Drawing.Point(0, 0);
      this.tlPanel.Name = "tlPanel";
      this.tlPanel.RowCount = 4;
      this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
      this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
      this.tlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
      this.tlPanel.Size = new System.Drawing.Size(1209, 681);
      this.tlPanel.TabIndex = 0;
      // 
      // chkLbActionGroups
      // 
      this.chkLbActionGroups.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.chkLbActionGroups.CheckBoxes = true;
      this.chkLbActionGroups.Dock = System.Windows.Forms.DockStyle.Fill;
      this.chkLbActionGroups.FullRowSelect = true;
      this.chkLbActionGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.chkLbActionGroups.HideSelection = false;
      this.chkLbActionGroups.HoverSelection = true;
      this.chkLbActionGroups.LabelWrap = false;
      this.chkLbActionGroups.Location = new System.Drawing.Point(3, 109);
      this.chkLbActionGroups.MultiSelect = false;
      this.chkLbActionGroups.Name = "chkLbActionGroups";
      this.chkLbActionGroups.ShowGroups = false;
      this.chkLbActionGroups.Size = new System.Drawing.Size(174, 117);
      this.chkLbActionGroups.TabIndex = 5;
      this.chkLbActionGroups.UseCompatibleStateImageBehavior = false;
      this.chkLbActionGroups.View = System.Windows.Forms.View.Details;
      this.chkLbActionGroups.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkLbActionGroups_ItemCheck);
      // 
      // chkLbActionMaps
      // 
      this.chkLbActionMaps.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.chkLbActionMaps.CheckBoxes = true;
      this.chkLbActionMaps.Dock = System.Windows.Forms.DockStyle.Fill;
      this.chkLbActionMaps.FullRowSelect = true;
      this.chkLbActionMaps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.chkLbActionMaps.HideSelection = false;
      this.chkLbActionMaps.HoverSelection = true;
      this.chkLbActionMaps.LabelWrap = false;
      this.chkLbActionMaps.Location = new System.Drawing.Point(3, 232);
      this.chkLbActionMaps.MultiSelect = false;
      this.chkLbActionMaps.Name = "chkLbActionMaps";
      this.chkLbActionMaps.ShowGroups = false;
      this.chkLbActionMaps.ShowItemToolTips = true;
      this.chkLbActionMaps.Size = new System.Drawing.Size(174, 365);
      this.chkLbActionMaps.TabIndex = 6;
      this.chkLbActionMaps.UseCompatibleStateImageBehavior = false;
      this.chkLbActionMaps.View = System.Windows.Forms.View.Details;
      // 
      // pnlInput
      // 
      this.pnlInput.BackColor = System.Drawing.Color.DarkGray;
      this.tlPanel.SetColumnSpan(this.pnlInput, 2);
      this.pnlInput.Controls.Add(this.btClose);
      this.pnlInput.Controls.Add(this.pnlDebug);
      this.pnlInput.Controls.Add(this.pictureBox1);
      this.pnlInput.Controls.Add(this.btLayout);
      this.pnlInput.Controls.Add(this.cbxLayouts);
      this.pnlInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlInput.Location = new System.Drawing.Point(3, 3);
      this.pnlInput.Name = "pnlInput";
      this.pnlInput.Size = new System.Drawing.Size(1203, 100);
      this.pnlInput.TabIndex = 1;
      // 
      // btClose
      // 
      this.btClose.Image = ((System.Drawing.Image)(resources.GetObject("btClose.Image")));
      this.btClose.Location = new System.Drawing.Point(651, 16);
      this.btClose.Name = "btClose";
      this.btClose.Size = new System.Drawing.Size(80, 53);
      this.btClose.TabIndex = 8;
      this.btClose.UseVisualStyleBackColor = true;
      this.btClose.Click += new System.EventHandler(this.btClose_Click);
      // 
      // pnlDebug
      // 
      this.pnlDebug.BackColor = System.Drawing.Color.DarkGray;
      this.pnlDebug.Controls.Add(this.label2);
      this.pnlDebug.Controls.Add(this.cbxJs4);
      this.pnlDebug.Controls.Add(this.btActivateDevices);
      this.pnlDebug.Controls.Add(this.cbxJs3);
      this.pnlDebug.Controls.Add(this.cbxJs2);
      this.pnlDebug.Controls.Add(this.cbxJs1);
      this.pnlDebug.Controls.Add(this.label4);
      this.pnlDebug.Controls.Add(this.label3);
      this.pnlDebug.Controls.Add(this.label1);
      this.pnlDebug.Controls.Add(this.btCreateDbgList);
      this.pnlDebug.Dock = System.Windows.Forms.DockStyle.Right;
      this.pnlDebug.Location = new System.Drawing.Point(742, 0);
      this.pnlDebug.Name = "pnlDebug";
      this.pnlDebug.Size = new System.Drawing.Size(461, 100);
      this.pnlDebug.TabIndex = 7;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 76);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(22, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "Js4";
      // 
      // cbxJs4
      // 
      this.cbxJs4.FormattingEnabled = true;
      this.cbxJs4.Location = new System.Drawing.Point(31, 73);
      this.cbxJs4.Name = "cbxJs4";
      this.cbxJs4.Size = new System.Drawing.Size(243, 21);
      this.cbxJs4.Sorted = true;
      this.cbxJs4.TabIndex = 18;
      // 
      // btActivateDevices
      // 
      this.btActivateDevices.Location = new System.Drawing.Point(280, 3);
      this.btActivateDevices.Name = "btActivateDevices";
      this.btActivateDevices.Size = new System.Drawing.Size(74, 90);
      this.btActivateDevices.TabIndex = 17;
      this.btActivateDevices.Text = "Activate Changed Devices";
      this.btActivateDevices.UseVisualStyleBackColor = true;
      this.btActivateDevices.Click += new System.EventHandler(this.btActivateDevices_Click);
      // 
      // cbxJs3
      // 
      this.cbxJs3.FormattingEnabled = true;
      this.cbxJs3.Location = new System.Drawing.Point(31, 50);
      this.cbxJs3.Name = "cbxJs3";
      this.cbxJs3.Size = new System.Drawing.Size(243, 21);
      this.cbxJs3.Sorted = true;
      this.cbxJs3.TabIndex = 16;
      // 
      // cbxJs2
      // 
      this.cbxJs2.FormattingEnabled = true;
      this.cbxJs2.Location = new System.Drawing.Point(31, 27);
      this.cbxJs2.Name = "cbxJs2";
      this.cbxJs2.Size = new System.Drawing.Size(243, 21);
      this.cbxJs2.Sorted = true;
      this.cbxJs2.TabIndex = 15;
      // 
      // cbxJs1
      // 
      this.cbxJs1.FormattingEnabled = true;
      this.cbxJs1.Location = new System.Drawing.Point(31, 4);
      this.cbxJs1.Name = "cbxJs1";
      this.cbxJs1.Size = new System.Drawing.Size(243, 21);
      this.cbxJs1.Sorted = true;
      this.cbxJs1.TabIndex = 14;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 53);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(22, 13);
      this.label4.TabIndex = 13;
      this.label4.Text = "Js3";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 30);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(22, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Js2";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(22, 13);
      this.label1.TabIndex = 11;
      this.label1.Text = "Js1";
      // 
      // btCreateDbgList
      // 
      this.btCreateDbgList.Location = new System.Drawing.Point(378, 4);
      this.btCreateDbgList.Name = "btCreateDbgList";
      this.btCreateDbgList.Size = new System.Drawing.Size(74, 89);
      this.btCreateDbgList.TabIndex = 10;
      this.btCreateDbgList.Text = "Create Test Items List";
      this.btCreateDbgList.UseVisualStyleBackColor = true;
      this.btCreateDbgList.Click += new System.EventHandler(this.btCreateDbgList_Click);
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
      this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
      this.pictureBox1.Location = new System.Drawing.Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(82, 90);
      this.pictureBox1.TabIndex = 6;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
      // 
      // btLayout
      // 
      this.btLayout.Image = ((System.Drawing.Image)(resources.GetObject("btLayout.Image")));
      this.btLayout.Location = new System.Drawing.Point(431, 16);
      this.btLayout.Name = "btLayout";
      this.btLayout.Size = new System.Drawing.Size(80, 53);
      this.btLayout.TabIndex = 3;
      this.btLayout.UseVisualStyleBackColor = true;
      this.btLayout.Click += new System.EventHandler(this.btLayout_Click);
      // 
      // cbxLayouts
      // 
      this.cbxLayouts.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbxLayouts.FormattingEnabled = true;
      this.cbxLayouts.Location = new System.Drawing.Point(91, 30);
      this.cbxLayouts.Name = "cbxLayouts";
      this.cbxLayouts.Size = new System.Drawing.Size(334, 25);
      this.cbxLayouts.Sorted = true;
      this.cbxLayouts.TabIndex = 1;
      this.cbxLayouts.SelectedIndexChanged += new System.EventHandler(this.cbxLayouts_SelectedIndexChanged);
      // 
      // drawPanel
      // 
      this.drawPanel.AutoScroll = true;
      this.drawPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.drawPanel.Controls.Add(this.gbxColors);
      this.drawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.drawPanel.Location = new System.Drawing.Point(183, 109);
      this.drawPanel.Name = "drawPanel";
      this.tlPanel.SetRowSpan(this.drawPanel, 3);
      this.drawPanel.Size = new System.Drawing.Size(1023, 569);
      this.drawPanel.TabIndex = 2;
      // 
      // gbxColors
      // 
      this.gbxColors.BackColor = System.Drawing.Color.Gray;
      this.gbxColors.Controls.Add(this.lblFontSize);
      this.gbxColors.Controls.Add(this.tbFontSize);
      this.gbxColors.Controls.Add(this.lblLayoutNote);
      this.gbxColors.Controls.Add(this.btAccept);
      this.gbxColors.Controls.Add(this.lblTextColor);
      this.gbxColors.Controls.Add(this.lblTest);
      this.gbxColors.Controls.Add(this.lblBackColor);
      this.gbxColors.Controls.Add(this.chkLbActionGroupsColor);
      this.gbxColors.Controls.Add(this.btCancel);
      this.gbxColors.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.gbxColors.Location = new System.Drawing.Point(17, 16);
      this.gbxColors.Name = "gbxColors";
      this.gbxColors.Size = new System.Drawing.Size(534, 455);
      this.gbxColors.TabIndex = 0;
      this.gbxColors.TabStop = false;
      this.gbxColors.Text = "Layout Settings";
      this.gbxColors.Visible = false;
      // 
      // lblFontSize
      // 
      this.lblFontSize.AutoSize = true;
      this.lblFontSize.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFontSize.ForeColor = System.Drawing.Color.LightCyan;
      this.lblFontSize.Location = new System.Drawing.Point(428, 35);
      this.lblFontSize.Name = "lblFontSize";
      this.lblFontSize.Size = new System.Drawing.Size(37, 30);
      this.lblFontSize.TabIndex = 14;
      this.lblFontSize.Text = "16";
      // 
      // tbFontSize
      // 
      this.tbFontSize.LargeChange = 2;
      this.tbFontSize.Location = new System.Drawing.Point(248, 30);
      this.tbFontSize.Maximum = 22;
      this.tbFontSize.Minimum = 12;
      this.tbFontSize.Name = "tbFontSize";
      this.tbFontSize.Size = new System.Drawing.Size(160, 45);
      this.tbFontSize.SmallChange = 2;
      this.tbFontSize.TabIndex = 13;
      this.tbFontSize.TickFrequency = 2;
      this.tbFontSize.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.tbFontSize.Value = 16;
      this.tbFontSize.Scroll += new System.EventHandler(this.tbFontSize_Scroll);
      // 
      // lblLayoutNote
      // 
      this.lblLayoutNote.ForeColor = System.Drawing.Color.LightCyan;
      this.lblLayoutNote.Location = new System.Drawing.Point(248, 246);
      this.lblLayoutNote.Name = "lblLayoutNote";
      this.lblLayoutNote.Size = new System.Drawing.Size(247, 141);
      this.lblLayoutNote.TabIndex = 12;
      this.lblLayoutNote.Tag = "§";
      this.lblLayoutNote.Text = "Select an entry in the list and then click above \r\nto assign colors.\r\n\r\nNote: \r\nW" +
    "hite background is treated as Transparent\r\nwhile drawing items to the image.";
      // 
      // btAccept
      // 
      this.btAccept.Location = new System.Drawing.Point(229, 407);
      this.btAccept.Name = "btAccept";
      this.btAccept.Size = new System.Drawing.Size(129, 26);
      this.btAccept.TabIndex = 11;
      this.btAccept.Tag = "§";
      this.btAccept.Text = "Accept";
      this.btAccept.UseVisualStyleBackColor = true;
      this.btAccept.Click += new System.EventHandler(this.btAcceptColors_Click);
      // 
      // lblTextColor
      // 
      this.lblTextColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lblTextColor.Cursor = System.Windows.Forms.Cursors.Hand;
      this.lblTextColor.Location = new System.Drawing.Point(281, 181);
      this.lblTextColor.Name = "lblTextColor";
      this.lblTextColor.Size = new System.Drawing.Size(157, 32);
      this.lblTextColor.TabIndex = 8;
      this.lblTextColor.Text = "Text Color";
      this.lblTextColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lblTextColor.Click += new System.EventHandler(this.lblTextColor_Click);
      // 
      // lblTest
      // 
      this.lblTest.BackColor = System.Drawing.Color.Transparent;
      this.lblTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lblTest.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTest.Location = new System.Drawing.Point(234, 89);
      this.lblTest.Name = "lblTest";
      this.lblTest.Size = new System.Drawing.Size(245, 49);
      this.lblTest.TabIndex = 10;
      this.lblTest.Text = "Color Test Label";
      this.lblTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblBackColor
      // 
      this.lblBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.lblBackColor.Cursor = System.Windows.Forms.Cursors.Hand;
      this.lblBackColor.Location = new System.Drawing.Point(251, 162);
      this.lblBackColor.Name = "lblBackColor";
      this.lblBackColor.Size = new System.Drawing.Size(211, 71);
      this.lblBackColor.TabIndex = 9;
      this.lblBackColor.Text = "Background Color";
      this.lblBackColor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.lblBackColor.Click += new System.EventHandler(this.lblBackColor_Click);
      // 
      // chkLbActionGroupsColor
      // 
      this.chkLbActionGroupsColor.Activation = System.Windows.Forms.ItemActivation.OneClick;
      this.chkLbActionGroupsColor.FullRowSelect = true;
      this.chkLbActionGroupsColor.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.chkLbActionGroupsColor.HideSelection = false;
      this.chkLbActionGroupsColor.HoverSelection = true;
      this.chkLbActionGroupsColor.LabelWrap = false;
      this.chkLbActionGroupsColor.Location = new System.Drawing.Point(20, 30);
      this.chkLbActionGroupsColor.MultiSelect = false;
      this.chkLbActionGroupsColor.Name = "chkLbActionGroupsColor";
      this.chkLbActionGroupsColor.ShowGroups = false;
      this.chkLbActionGroupsColor.Size = new System.Drawing.Size(189, 291);
      this.chkLbActionGroupsColor.TabIndex = 7;
      this.chkLbActionGroupsColor.UseCompatibleStateImageBehavior = false;
      this.chkLbActionGroupsColor.View = System.Windows.Forms.View.Details;
      this.chkLbActionGroupsColor.ItemActivate += new System.EventHandler(this.chkLbActionGroupsColor_ItemActivate);
      // 
      // btCancel
      // 
      this.btCancel.Location = new System.Drawing.Point(377, 407);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(129, 26);
      this.btCancel.TabIndex = 6;
      this.btCancel.Tag = "§";
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCloseColors_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btSave);
      this.panel1.Controls.Add(this.btColors);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 603);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(174, 75);
      this.panel1.TabIndex = 7;
      // 
      // btSave
      // 
      this.btSave.Image = ((System.Drawing.Image)(resources.GetObject("btSave.Image")));
      this.btSave.Location = new System.Drawing.Point(3, 11);
      this.btSave.Name = "btSave";
      this.btSave.Size = new System.Drawing.Size(62, 54);
      this.btSave.TabIndex = 4;
      this.btSave.UseVisualStyleBackColor = true;
      this.btSave.Click += new System.EventHandler(this.btSave_Click);
      // 
      // btColors
      // 
      this.btColors.Image = global::SCJMapper_V2.Properties.Resources.Settings;
      this.btColors.Location = new System.Drawing.Point(109, 11);
      this.btColors.Name = "btColors";
      this.btColors.Size = new System.Drawing.Size(62, 54);
      this.btColors.TabIndex = 5;
      this.btColors.UseVisualStyleBackColor = true;
      this.btColors.Click += new System.EventHandler(this.btColors_Click);
      // 
      // SFD
      // 
      this.SFD.DefaultExt = "png";
      this.SFD.SupportMultiDottedExtensions = true;
      this.SFD.Title = "Save Map Image";
      // 
      // colDlg
      // 
      this.colDlg.AnyColor = true;
      this.colDlg.FullOpen = true;
      // 
      // FormLayout
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1209, 681);
      this.Controls.Add(this.tlPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(1225, 720);
      this.Name = "FormLayout";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Device Layout";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLayout_FormClosing);
      this.Load += new System.EventHandler(this.FormLayout_Load);
      this.tlPanel.ResumeLayout(false);
      this.pnlInput.ResumeLayout(false);
      this.pnlDebug.ResumeLayout(false);
      this.pnlDebug.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.drawPanel.ResumeLayout(false);
      this.gbxColors.ResumeLayout(false);
      this.gbxColors.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tbFontSize)).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tlPanel;
    private System.Windows.Forms.Panel pnlInput;
    private System.Windows.Forms.Button btLayout;
    private System.Windows.Forms.ComboBox cbxLayouts;
    private System.Windows.Forms.Panel drawPanel;
    private System.Windows.Forms.ListView chkLbActionMaps;
    private System.Windows.Forms.ListView chkLbActionGroups;
    private System.Windows.Forms.Button btSave;
    private System.Windows.Forms.SaveFileDialog SFD;
    private System.Windows.Forms.Button btColors;
    private System.Windows.Forms.GroupBox gbxColors;
    private System.Windows.Forms.Button btCancel;
    private System.Windows.Forms.Label lblTest;
    private System.Windows.Forms.ListView chkLbActionGroupsColor;
    private System.Windows.Forms.Label lblBackColor;
    private System.Windows.Forms.Label lblTextColor;
    private System.Windows.Forms.ColorDialog colDlg;
    private System.Windows.Forms.Button btAccept;
    private System.Windows.Forms.Label lblLayoutNote;
    private System.Windows.Forms.TrackBar tbFontSize;
    private System.Windows.Forms.Label lblFontSize;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Button btCreateDbgList;
    private System.Windows.Forms.Panel pnlDebug;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btClose;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ComboBox cbxJs1;
    private System.Windows.Forms.ComboBox cbxJs3;
    private System.Windows.Forms.ComboBox cbxJs2;
    private System.Windows.Forms.Button btActivateDevices;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbxJs4;
  }
}