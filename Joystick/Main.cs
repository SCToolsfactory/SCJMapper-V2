//-----------------------------------------------------------------------------
// File: Main.cs
//
// Desc: The Joystick sample obtains and displays joystick data.
//
// Copyright (c) Microsoft Corporation. All rights reserved
//-----------------------------------------------------------------------------
using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX;
using System.Drawing;
using System.IO;

namespace Joystick
{


  public class MainClass : System.Windows.Forms.Form
  {

    #region Window control declarations

    private System.Windows.Forms.Button buttonExit;
    private System.Windows.Forms.Timer timer1;
    private Panel panel1;
    private TabControl tc1;
    private TabPage tabJS1;
    private TreeView treeView1;
    private Panel panel2;
    private Label lblLastJ;
    private Label lblAction;
    private System.Windows.Forms.Button btAssign;
    private Panel panel3;
    private ComboBox cbJs1;
    private Label label56;
    private Label label55;
    private Label label54;
    private ComboBox cbJs3;
    private ComboBox cbJs2;
    private Label label57;
    private System.Windows.Forms.Button btClear;
    private System.Windows.Forms.Button btDump;
    private RichTextBox rtb;
    private Label label3;
    private System.Windows.Forms.Button btGrab;
    private Label label4;
    private Label lblTitle;
    private Label label7;
    private Label label6;
    private Label label8;
    private ImageList IL;
    private UC_JoyPanel UC_JoyPanel;
    private ContextMenuStrip cmCopyPaste;
    private ToolStripMenuItem tsiCopy;
    private ToolStripMenuItem tsiPaste;
    private ToolStripMenuItem tsiSelAll;
    private ToolStripMenuItem tsiPReplace;
    private System.Windows.Forms.Button btReset;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem tsiOpen;
    private ToolStripMenuItem tsiSaveAs;
    private OpenFileDialog OFD;
    private SaveFileDialog SFD;
    private System.Windows.Forms.Button btFind;
    private System.Windows.Forms.Button btDumpList;
    private System.ComponentModel.IContainer components;


    #endregion


    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainClass));
      this.buttonExit = new System.Windows.Forms.Button();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.panel1 = new System.Windows.Forms.Panel();
      this.label8 = new System.Windows.Forms.Label();
      this.lblTitle = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.tc1 = new System.Windows.Forms.TabControl();
      this.tabJS1 = new System.Windows.Forms.TabPage();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.panel2 = new System.Windows.Forms.Panel();
      this.btFind = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.btClear = new System.Windows.Forms.Button();
      this.lblLastJ = new System.Windows.Forms.Label();
      this.lblAction = new System.Windows.Forms.Label();
      this.btAssign = new System.Windows.Forms.Button();
      this.panel3 = new System.Windows.Forms.Panel();
      this.label56 = new System.Windows.Forms.Label();
      this.label55 = new System.Windows.Forms.Label();
      this.label57 = new System.Windows.Forms.Label();
      this.label54 = new System.Windows.Forms.Label();
      this.cbJs3 = new System.Windows.Forms.ComboBox();
      this.cbJs2 = new System.Windows.Forms.ComboBox();
      this.cbJs1 = new System.Windows.Forms.ComboBox();
      this.btDump = new System.Windows.Forms.Button();
      this.rtb = new System.Windows.Forms.RichTextBox();
      this.cmCopyPaste = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.tsiCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiPaste = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiPReplace = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiSelAll = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tsiOpen = new System.Windows.Forms.ToolStripMenuItem();
      this.tsiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
      this.label3 = new System.Windows.Forms.Label();
      this.btGrab = new System.Windows.Forms.Button();
      this.btReset = new System.Windows.Forms.Button();
      this.OFD = new System.Windows.Forms.OpenFileDialog();
      this.SFD = new System.Windows.Forms.SaveFileDialog();
      this.btDumpList = new System.Windows.Forms.Button();
      this.UC_JoyPanel = new Joystick.UC_JoyPanel();
      this.panel1.SuspendLayout();
      this.tc1.SuspendLayout();
      this.tabJS1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.cmCopyPaste.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonExit
      // 
      this.buttonExit.Location = new System.Drawing.Point(577, 678);
      this.buttonExit.Name = "buttonExit";
      this.buttonExit.Size = new System.Drawing.Size(94, 24);
      this.buttonExit.TabIndex = 0;
      this.buttonExit.Text = "Exit";
      this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
      // 
      // timer1
      // 
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.label8);
      this.panel1.Controls.Add(this.lblTitle);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Location = new System.Drawing.Point(2, 1);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1109, 66);
      this.panel1.TabIndex = 2;
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
      // tc1
      // 
      this.tc1.Controls.Add(this.tabJS1);
      this.tc1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tc1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tc1.HotTrack = true;
      this.tc1.ItemSize = new System.Drawing.Size(62, 22);
      this.tc1.Location = new System.Drawing.Point(387, 73);
      this.tc1.Multiline = true;
      this.tc1.Name = "tc1";
      this.tc1.SelectedIndex = 0;
      this.tc1.Size = new System.Drawing.Size(289, 252);
      this.tc1.TabIndex = 3;
      this.tc1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tc1_DrawItem);
      // 
      // tabJS1
      // 
      this.tabJS1.Controls.Add(this.UC_JoyPanel);
      this.tabJS1.Location = new System.Drawing.Point(4, 26);
      this.tabJS1.Name = "tabJS1";
      this.tabJS1.Padding = new System.Windows.Forms.Padding(3);
      this.tabJS1.Size = new System.Drawing.Size(281, 222);
      this.tabJS1.TabIndex = 0;
      this.tabJS1.Text = "Joystick 1";
      // 
      // treeView1
      // 
      this.treeView1.HotTracking = true;
      this.treeView1.ImageKey = "Map";
      this.treeView1.ImageList = this.IL;
      this.treeView1.Location = new System.Drawing.Point(12, 73);
      this.treeView1.Name = "treeView1";
      this.treeView1.SelectedImageKey = "Selected";
      this.treeView1.Size = new System.Drawing.Size(369, 629);
      this.treeView1.TabIndex = 4;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
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
      // 
      // panel2
      // 
      this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel2.Controls.Add(this.btFind);
      this.panel2.Controls.Add(this.label7);
      this.panel2.Controls.Add(this.label6);
      this.panel2.Controls.Add(this.btClear);
      this.panel2.Controls.Add(this.lblLastJ);
      this.panel2.Controls.Add(this.lblAction);
      this.panel2.Controls.Add(this.btAssign);
      this.panel2.Location = new System.Drawing.Point(387, 471);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(289, 103);
      this.panel2.TabIndex = 5;
      // 
      // btFind
      // 
      this.btFind.Location = new System.Drawing.Point(190, 37);
      this.btFind.Name = "btFind";
      this.btFind.Size = new System.Drawing.Size(84, 20);
      this.btFind.TabIndex = 12;
      this.btFind.Text = "Find 1st.";
      this.btFind.UseVisualStyleBackColor = true;
      this.btFind.Click += new System.EventHandler(this.btFind_Click);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(7, 37);
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
      this.btClear.Location = new System.Drawing.Point(190, 67);
      this.btClear.Name = "btClear";
      this.btClear.Size = new System.Drawing.Size(94, 25);
      this.btClear.TabIndex = 2;
      this.btClear.Text = "Clear";
      this.btClear.UseVisualStyleBackColor = true;
      this.btClear.Click += new System.EventHandler(this.btClear_Click);
      // 
      // lblLastJ
      // 
      this.lblLastJ.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblLastJ.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLastJ.Location = new System.Drawing.Point(52, 37);
      this.lblLastJ.Name = "lblLastJ";
      this.lblLastJ.Size = new System.Drawing.Size(135, 20);
      this.lblLastJ.TabIndex = 1;
      this.lblLastJ.Text = "...";
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
      this.btAssign.Size = new System.Drawing.Size(94, 25);
      this.btAssign.TabIndex = 0;
      this.btAssign.Text = "Assign";
      this.btAssign.UseVisualStyleBackColor = true;
      this.btAssign.Click += new System.EventHandler(this.btAssign_Click);
      // 
      // panel3
      // 
      this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel3.Controls.Add(this.label56);
      this.panel3.Controls.Add(this.label55);
      this.panel3.Controls.Add(this.label57);
      this.panel3.Controls.Add(this.label54);
      this.panel3.Controls.Add(this.cbJs3);
      this.panel3.Controls.Add(this.cbJs2);
      this.panel3.Controls.Add(this.cbJs1);
      this.panel3.Location = new System.Drawing.Point(387, 331);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(289, 134);
      this.panel3.TabIndex = 6;
      // 
      // label56
      // 
      this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label56.Location = new System.Drawing.Point(7, 101);
      this.label56.Name = "label56";
      this.label56.Size = new System.Drawing.Size(39, 20);
      this.label56.TabIndex = 2;
      this.label56.Text = "js3";
      // 
      // label55
      // 
      this.label55.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label55.Location = new System.Drawing.Point(7, 74);
      this.label55.Name = "label55";
      this.label55.Size = new System.Drawing.Size(39, 20);
      this.label55.TabIndex = 2;
      this.label55.Text = "js2";
      // 
      // label57
      // 
      this.label57.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label57.Location = new System.Drawing.Point(7, 9);
      this.label57.Name = "label57";
      this.label57.Size = new System.Drawing.Size(267, 20);
      this.label57.TabIndex = 2;
      this.label57.Text = "SC-Device to Joystick Mapping";
      // 
      // label54
      // 
      this.label54.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label54.Location = new System.Drawing.Point(7, 47);
      this.label54.Name = "label54";
      this.label54.Size = new System.Drawing.Size(39, 20);
      this.label54.TabIndex = 2;
      this.label54.Text = "js1";
      // 
      // cbJs3
      // 
      this.cbJs3.DisplayMember = "js1";
      this.cbJs3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbJs3.FormattingEnabled = true;
      this.cbJs3.Items.AddRange(new object[] {
            "Joystick 1",
            "Joystick 2",
            "Joystick 3"});
      this.cbJs3.Location = new System.Drawing.Point(52, 98);
      this.cbJs3.MaxDropDownItems = 4;
      this.cbJs3.Name = "cbJs3";
      this.cbJs3.Size = new System.Drawing.Size(233, 25);
      this.cbJs3.TabIndex = 0;
      // 
      // cbJs2
      // 
      this.cbJs2.DisplayMember = "js1";
      this.cbJs2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbJs2.FormattingEnabled = true;
      this.cbJs2.Items.AddRange(new object[] {
            "Joystick 1",
            "Joystick 2",
            "Joystick 3"});
      this.cbJs2.Location = new System.Drawing.Point(52, 71);
      this.cbJs2.MaxDropDownItems = 4;
      this.cbJs2.Name = "cbJs2";
      this.cbJs2.Size = new System.Drawing.Size(233, 25);
      this.cbJs2.TabIndex = 0;
      // 
      // cbJs1
      // 
      this.cbJs1.DisplayMember = "js1";
      this.cbJs1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbJs1.FormattingEnabled = true;
      this.cbJs1.Items.AddRange(new object[] {
            "Joystick 1",
            "Joystick 2",
            "Joystick 3"});
      this.cbJs1.Location = new System.Drawing.Point(52, 44);
      this.cbJs1.MaxDropDownItems = 4;
      this.cbJs1.Name = "cbJs1";
      this.cbJs1.Size = new System.Drawing.Size(233, 25);
      this.cbJs1.TabIndex = 0;
      // 
      // btDump
      // 
      this.btDump.Location = new System.Drawing.Point(397, 589);
      this.btDump.Name = "btDump";
      this.btDump.Size = new System.Drawing.Size(94, 26);
      this.btDump.TabIndex = 8;
      this.btDump.Text = "Dump XML-->";
      this.btDump.UseVisualStyleBackColor = true;
      this.btDump.Click += new System.EventHandler(this.btDump_Click);
      // 
      // rtb
      // 
      this.rtb.AcceptsTab = true;
      this.rtb.BackColor = System.Drawing.Color.Ivory;
      this.rtb.ContextMenuStrip = this.cmCopyPaste;
      this.rtb.DetectUrls = false;
      this.rtb.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtb.Location = new System.Drawing.Point(682, 73);
      this.rtb.Name = "rtb";
      this.rtb.Size = new System.Drawing.Size(439, 608);
      this.rtb.TabIndex = 9;
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
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(797, 684);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(231, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Right click above to open the context menu";
      // 
      // btGrab
      // 
      this.btGrab.Location = new System.Drawing.Point(578, 589);
      this.btGrab.Name = "btGrab";
      this.btGrab.Size = new System.Drawing.Size(94, 26);
      this.btGrab.TabIndex = 8;
      this.btGrab.Text = "<-- Grab XML";
      this.btGrab.UseVisualStyleBackColor = true;
      this.btGrab.Click += new System.EventHandler(this.btGrab_Click);
      // 
      // btReset
      // 
      this.btReset.Location = new System.Drawing.Point(397, 678);
      this.btReset.Name = "btReset";
      this.btReset.Size = new System.Drawing.Size(94, 24);
      this.btReset.TabIndex = 11;
      this.btReset.Text = "Reset";
      this.btReset.Click += new System.EventHandler(this.btReset_Click);
      // 
      // OFD
      // 
      this.OFD.DefaultExt = "xml";
      this.OFD.FileName = "Open Map File";
      this.OFD.Filter = "Mapping files|*.xml|All files|*.*";
      this.OFD.ReadOnlyChecked = true;
      this.OFD.SupportMultiDottedExtensions = true;
      // 
      // SFD
      // 
      this.SFD.DefaultExt = "xml";
      this.SFD.Filter = "Mapping files|*.xml|Text files|*.txt|All files|*.*";
      this.SFD.SupportMultiDottedExtensions = true;
      // 
      // btDumpList
      // 
      this.btDumpList.Location = new System.Drawing.Point(398, 630);
      this.btDumpList.Name = "btDumpList";
      this.btDumpList.Size = new System.Drawing.Size(94, 26);
      this.btDumpList.TabIndex = 12;
      this.btDumpList.Text = "Dump List-->";
      this.btDumpList.UseVisualStyleBackColor = true;
      this.btDumpList.Click += new System.EventHandler(this.btDumpList_Click);
      // 
      // UC_JoyPanel
      // 
      this.UC_JoyPanel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UC_JoyPanel.Location = new System.Drawing.Point(6, 6);
      this.UC_JoyPanel.Name = "UC_JoyPanel";
      this.UC_JoyPanel.Size = new System.Drawing.Size(276, 195);
      this.UC_JoyPanel.TabIndex = 0;
      // 
      // MainClass
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 15);
      this.ClientSize = new System.Drawing.Size(1133, 712);
      this.Controls.Add(this.btDumpList);
      this.Controls.Add(this.btReset);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.rtb);
      this.Controls.Add(this.btGrab);
      this.Controls.Add(this.btDump);
      this.Controls.Add(this.panel3);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.treeView1);
      this.Controls.Add(this.tc1);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.buttonExit);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.Name = "MainClass";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "SC Joystick Mapper";
      this.Load += new System.EventHandler(this.MainClass_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tc1.ResumeLayout(false);
      this.tabJS1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.cmCopyPaste.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion


    ///<remarks>
    /// Holds the DXInput Joystick List
    ///</remarks>
    private List<JoystickCls> m_JS = new List<JoystickCls>( );
    ///<remarks>
    /// Holds the ActionTree that manages the TreeView and the action lists
    ///</remarks>
    private ActionTree m_AT = null;



    #region Main Form Handling

    
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main( string[] args )
    {
      // Create a new instance of 
      // the MainClass class.
      Application.Run( new MainClass( ) );
    }

    /// <summary>
    /// TO create the form and its GUI components
    /// </summary>
    public MainClass( )
    {
      try {
        // Load the icon from our resources
        System.Resources.ResourceManager resources = new System.Resources.ResourceManager( this.GetType( ) );
        this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
      }
      catch {
        ; // well...
      }
      //
      // Required for Windows Form Designer support.
      //
      InitializeComponent( );

      // some applic initialization 
      rtb.SelectionTabs = new int[] { 10, 20, 30, 40, 50, 60 }; // short tabs
      String version = Application.ProductVersion;  // get the version information
      lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." )+1 ) ); // get the first two elements
    }

    /// <summary>
    ///  Handle the load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainClass_Load( object sender, System.EventArgs e )
    {
      if ( !InitDirectInput( ) )
        Close( );

      timer1.Start( ); // this one polls the joysticks to show the props
    }

    /// <summary>
    /// need to shutdown the XInputs
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      timer1.Stop( );

      // Unacquire all DirectInput objects.
      foreach ( JoystickCls js in m_JS ) js.FinishDX( );

      if ( disposing ) {
        if ( components != null ) {
          components.Dispose( );
        }
      }
      base.Dispose( disposing );
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
      }
      catch ( Exception Ex ) {
        MessageBox.Show( Ex.Message.ToString( ), "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Information );

      }

    }

    #endregion

    /// <summary>
    /// Resets the Action Tree
    /// </summary>
    private void InitActionTree( )
    {
      // build TreeView and the ActionMaps
      m_AT = new ActionTree( );
      m_AT.Ctrl = treeView1;  // the ActionTree owns the TreeView control
      m_AT.LoadTree( );       // Init

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
      
      // scan the Input for attached devices
      foreach ( DeviceInstance instance in Manager.GetDevices( DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly ) ) {
        // Create the device interface
        Device jsDevice = new Device( instance.InstanceGuid );
        JoystickCls js = null;

        // we have the first tab made as reference so TabPage[0] already exists
        if ( tabs == 0 ) {
          // first panel - The Tab content exists already 
          js = new JoystickCls( jsDevice, this, UC_JoyPanel ); // does all device related activities for that particular item
        }
        else {
          // setup the further tab contents along the reference one in TabPage[0] (the control named UC_JoyPanel)
          tc1.TabPages.Add("Joystick " + (tabs+1).ToString());
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

      InitActionTree( );

      return true;
    }

    /// <summary>
    /// Create the jsN  Joystick string from mapping (or from the JS index above item 3)
    /// </summary>
    /// <returns></returns>
    private String JSStr( )
    {
      if ( (String)tc1.SelectedTab.Tag == ( string )cbJs1.SelectedItem ) return JoystickCls.JSTag( 1 );
      if ( ( String )tc1.SelectedTab.Tag == ( string )cbJs2.SelectedItem ) return JoystickCls.JSTag( 2 );
      if ( ( String )tc1.SelectedTab.Tag == ( string )cbJs3.SelectedItem ) return JoystickCls.JSTag( 3 );
      return JoystickCls.JSTag( tc1.SelectedIndex+1 ); // return the Joystick number
    }


    #region Event Handling


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

      btDump.BackColor = btClear.BackColor; // neutral again
      btGrab.BackColor = btClear.BackColor; // neutral again
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
      btGrab.BackColor = btClear.BackColor; // neutral again
      btDump.BackColor = btClear.BackColor; // neutral again
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

    private void btReset_Click( object sender, EventArgs e )
    {
      InitActionTree( ); // start over
    }

    private void tsiOpen_Click( object sender, EventArgs e )
    {

      if ( OFD.ShowDialog(this) == System.Windows.Forms.DialogResult.OK ) {
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

    
    #endregion




  }
}
