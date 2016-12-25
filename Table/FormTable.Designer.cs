namespace SCJMapper_V2.Table
{
  partial class FormTable
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
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTable));
      this.DGV = new System.Windows.Forms.DataGridView();
      this.txFilterAction = new System.Windows.Forms.TextBox();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.panel2 = new System.Windows.Forms.Panel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btBlendAll = new System.Windows.Forms.Button();
      this.btCancelEdit = new System.Windows.Forms.Button();
      this.chkEditBlend = new System.Windows.Forms.CheckBox();
      this.btUpdateFromEdit = new System.Windows.Forms.Button();
      this.btClrFilterUsrBinding = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.txFilterUsrBinding = new System.Windows.Forms.TextBox();
      this.btClrFilterDefBinding = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.txFilterDefBinding = new System.Windows.Forms.TextBox();
      this.btClrFilterAction = new System.Windows.Forms.Button();
      this.chkKbd = new System.Windows.Forms.CheckBox();
      this.chkMouse = new System.Windows.Forms.CheckBox();
      this.chkGamepad = new System.Windows.Forms.CheckBox();
      this.chkJoystick = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.dS_ActionMaps = new SCJMapper_V2.Table.DS_ActionMaps();
      this.dSActionMapsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dS_ActionMaps)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dSActionMapsBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // DGV
      // 
      this.DGV.AllowUserToAddRows = false;
      this.DGV.AllowUserToDeleteRows = false;
      this.DGV.AllowUserToOrderColumns = true;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.DGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.DGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
      this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DGV.Location = new System.Drawing.Point(3, 3);
      this.DGV.Name = "DGV";
      this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV.Size = new System.Drawing.Size(438, 230);
      this.DGV.TabIndex = 0;
      this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseClick);
      this.DGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseDoubleClick);
      this.DGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
      this.DGV.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.DGV_ColumnWidthChanged);
      this.DGV.CurrentCellDirtyStateChanged += new System.EventHandler(this.DGV_CurrentCellDirtyStateChanged);
      this.DGV.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_RowHeaderMouseClick);
      // 
      // txFilterAction
      // 
      this.txFilterAction.Location = new System.Drawing.Point(93, 6);
      this.txFilterAction.Name = "txFilterAction";
      this.txFilterAction.Size = new System.Drawing.Size(109, 22);
      this.txFilterAction.TabIndex = 1;
      this.txFilterAction.TextChanged += new System.EventHandler(this.txFilterAction_TextChanged);
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
      this.tableLayoutPanel1.Controls.Add(this.DGV, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(744, 286);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // panel2
      // 
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(447, 239);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(294, 44);
      this.panel2.TabIndex = 2;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(447, 3);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 209F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(294, 230);
      this.tableLayoutPanel2.TabIndex = 3;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btBlendAll);
      this.panel1.Controls.Add(this.btCancelEdit);
      this.panel1.Controls.Add(this.chkEditBlend);
      this.panel1.Controls.Add(this.btUpdateFromEdit);
      this.panel1.Controls.Add(this.btClrFilterUsrBinding);
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.txFilterUsrBinding);
      this.panel1.Controls.Add(this.btClrFilterDefBinding);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.txFilterDefBinding);
      this.panel1.Controls.Add(this.btClrFilterAction);
      this.panel1.Controls.Add(this.chkKbd);
      this.panel1.Controls.Add(this.chkMouse);
      this.panel1.Controls.Add(this.chkGamepad);
      this.panel1.Controls.Add(this.chkJoystick);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.txFilterAction);
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(288, 202);
      this.panel1.TabIndex = 1;
      // 
      // btBlendAll
      // 
      this.btBlendAll.Enabled = false;
      this.btBlendAll.Location = new System.Drawing.Point(183, 122);
      this.btBlendAll.Name = "btBlendAll";
      this.btBlendAll.Size = new System.Drawing.Size(97, 42);
      this.btBlendAll.TabIndex = 14;
      this.btBlendAll.Text = "Disable all Unmapped";
      this.btBlendAll.UseVisualStyleBackColor = true;
      this.btBlendAll.Click += new System.EventHandler(this.btBlendAll_Click);
      // 
      // btCancelEdit
      // 
      this.btCancelEdit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btCancelEdit.ForeColor = System.Drawing.Color.DarkOliveGreen;
      this.btCancelEdit.Location = new System.Drawing.Point(183, 170);
      this.btCancelEdit.Name = "btCancelEdit";
      this.btCancelEdit.Size = new System.Drawing.Size(97, 28);
      this.btCancelEdit.TabIndex = 13;
      this.btCancelEdit.Text = "Undo Edits";
      this.btCancelEdit.UseVisualStyleBackColor = true;
      this.btCancelEdit.Click += new System.EventHandler(this.btCancelEdit_Click);
      // 
      // chkEditBlend
      // 
      this.chkEditBlend.AutoSize = true;
      this.chkEditBlend.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.chkEditBlend.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkEditBlend.ForeColor = System.Drawing.Color.Firebrick;
      this.chkEditBlend.Location = new System.Drawing.Point(76, 135);
      this.chkEditBlend.Name = "chkEditBlend";
      this.chkEditBlend.Size = new System.Drawing.Size(87, 17);
      this.chkEditBlend.TabIndex = 12;
      this.chkEditBlend.Text = "Edit Disable";
      this.chkEditBlend.UseVisualStyleBackColor = true;
      this.chkEditBlend.CheckedChanged += new System.EventHandler(this.chkEditBlend_CheckedChanged);
      // 
      // btUpdateFromEdit
      // 
      this.btUpdateFromEdit.Enabled = false;
      this.btUpdateFromEdit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btUpdateFromEdit.ForeColor = System.Drawing.Color.Firebrick;
      this.btUpdateFromEdit.Location = new System.Drawing.Point(10, 169);
      this.btUpdateFromEdit.Name = "btUpdateFromEdit";
      this.btUpdateFromEdit.Size = new System.Drawing.Size(97, 30);
      this.btUpdateFromEdit.TabIndex = 12;
      this.btUpdateFromEdit.Text = "Accept Edits";
      this.btUpdateFromEdit.UseVisualStyleBackColor = true;
      this.btUpdateFromEdit.Click += new System.EventHandler(this.btUpdateFromEdit_Click);
      // 
      // btClrFilterUsrBinding
      // 
      this.btClrFilterUsrBinding.Location = new System.Drawing.Point(222, 60);
      this.btClrFilterUsrBinding.Name = "btClrFilterUsrBinding";
      this.btClrFilterUsrBinding.Size = new System.Drawing.Size(58, 23);
      this.btClrFilterUsrBinding.TabIndex = 11;
      this.btClrFilterUsrBinding.Text = "Clear";
      this.btClrFilterUsrBinding.UseVisualStyleBackColor = true;
      this.btClrFilterUsrBinding.Click += new System.EventHandler(this.btClrFilterUsrBinding_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(7, 65);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(80, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Usr Bind Filter";
      // 
      // txFilterUsrBinding
      // 
      this.txFilterUsrBinding.Location = new System.Drawing.Point(93, 62);
      this.txFilterUsrBinding.Name = "txFilterUsrBinding";
      this.txFilterUsrBinding.Size = new System.Drawing.Size(109, 22);
      this.txFilterUsrBinding.TabIndex = 9;
      this.txFilterUsrBinding.TextChanged += new System.EventHandler(this.txFilterUsrBinding_TextChanged);
      // 
      // btClrFilterDefBinding
      // 
      this.btClrFilterDefBinding.Location = new System.Drawing.Point(222, 32);
      this.btClrFilterDefBinding.Name = "btClrFilterDefBinding";
      this.btClrFilterDefBinding.Size = new System.Drawing.Size(58, 23);
      this.btClrFilterDefBinding.TabIndex = 8;
      this.btClrFilterDefBinding.Text = "Clear";
      this.btClrFilterDefBinding.UseVisualStyleBackColor = true;
      this.btClrFilterDefBinding.Click += new System.EventHandler(this.btClrFilterBinding_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 37);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(81, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Def Bind Filter";
      // 
      // txFilterDefBinding
      // 
      this.txFilterDefBinding.Location = new System.Drawing.Point(93, 34);
      this.txFilterDefBinding.Name = "txFilterDefBinding";
      this.txFilterDefBinding.Size = new System.Drawing.Size(109, 22);
      this.txFilterDefBinding.TabIndex = 6;
      this.txFilterDefBinding.TextChanged += new System.EventHandler(this.txFilterBinding_TextChanged);
      // 
      // btClrFilterAction
      // 
      this.btClrFilterAction.Location = new System.Drawing.Point(222, 4);
      this.btClrFilterAction.Name = "btClrFilterAction";
      this.btClrFilterAction.Size = new System.Drawing.Size(58, 23);
      this.btClrFilterAction.TabIndex = 5;
      this.btClrFilterAction.Text = "Clear";
      this.btClrFilterAction.UseVisualStyleBackColor = true;
      this.btClrFilterAction.Click += new System.EventHandler(this.btClrFilterAction_Click);
      // 
      // chkKbd
      // 
      this.chkKbd.AutoSize = true;
      this.chkKbd.Location = new System.Drawing.Point(222, 93);
      this.chkKbd.Name = "chkKbd";
      this.chkKbd.Size = new System.Drawing.Size(46, 17);
      this.chkKbd.TabIndex = 4;
      this.chkKbd.Text = "Kbd";
      this.chkKbd.UseVisualStyleBackColor = true;
      this.chkKbd.CheckedChanged += new System.EventHandler(this.chkKbd_CheckedChanged);
      // 
      // chkMouse
      // 
      this.chkMouse.AutoSize = true;
      this.chkMouse.Location = new System.Drawing.Point(158, 93);
      this.chkMouse.Name = "chkMouse";
      this.chkMouse.Size = new System.Drawing.Size(61, 17);
      this.chkMouse.TabIndex = 4;
      this.chkMouse.Text = "Mouse";
      this.chkMouse.UseVisualStyleBackColor = true;
      this.chkMouse.CheckedChanged += new System.EventHandler(this.chkMouse_CheckedChanged);
      // 
      // chkGamepad
      // 
      this.chkGamepad.AutoSize = true;
      this.chkGamepad.Location = new System.Drawing.Point(80, 93);
      this.chkGamepad.Name = "chkGamepad";
      this.chkGamepad.Size = new System.Drawing.Size(75, 17);
      this.chkGamepad.TabIndex = 4;
      this.chkGamepad.Text = "Gamepad";
      this.chkGamepad.UseVisualStyleBackColor = true;
      this.chkGamepad.CheckedChanged += new System.EventHandler(this.chkGamepad_CheckedChanged);
      // 
      // chkJoystick
      // 
      this.chkJoystick.AutoSize = true;
      this.chkJoystick.Location = new System.Drawing.Point(10, 93);
      this.chkJoystick.Name = "chkJoystick";
      this.chkJoystick.Size = new System.Drawing.Size(65, 17);
      this.chkJoystick.TabIndex = 3;
      this.chkJoystick.Text = "Joystick";
      this.chkJoystick.UseVisualStyleBackColor = true;
      this.chkJoystick.CheckedChanged += new System.EventHandler(this.chkJoystick_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(69, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Action Filter";
      // 
      // dS_ActionMaps
      // 
      this.dS_ActionMaps.DataSetName = "Table.DS_ActionMaps";
      this.dS_ActionMaps.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
      // 
      // dSActionMapsBindingSource
      // 
      this.dSActionMapsBindingSource.DataSource = this.dS_ActionMaps;
      this.dSActionMapsBindingSource.Position = 0;
      // 
      // FormTable
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(744, 286);
      this.Controls.Add(this.tableLayoutPanel1);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(760, 320);
      this.Name = "FormTable";
      this.Text = "Actiontree as Table";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTable_FormClosing);
      this.LocationChanged += new System.EventHandler(this.FormTable_LocationChanged);
      this.SizeChanged += new System.EventHandler(this.FormTable_SizeChanged);
      ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dS_ActionMaps)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dSActionMapsBindingSource)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView DGV;
    private System.Windows.Forms.BindingSource dSActionMapsBindingSource;
    private DS_ActionMaps dS_ActionMaps;
    private System.Windows.Forms.TextBox txFilterAction;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btClrFilterDefBinding;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txFilterDefBinding;
    private System.Windows.Forms.Button btClrFilterAction;
    private System.Windows.Forms.CheckBox chkKbd;
    private System.Windows.Forms.CheckBox chkMouse;
    private System.Windows.Forms.CheckBox chkGamepad;
    private System.Windows.Forms.CheckBox chkJoystick;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btClrFilterUsrBinding;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txFilterUsrBinding;
    private System.Windows.Forms.Button btUpdateFromEdit;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.CheckBox chkEditBlend;
    private System.Windows.Forms.Button btCancelEdit;
    private System.Windows.Forms.Button btBlendAll;
  }
}