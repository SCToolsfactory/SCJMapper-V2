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
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblLoadingData = new System.Windows.Forms.Label();
      this.gbxEditBindings = new System.Windows.Forms.GroupBox();
      this.chkEditDisabled = new System.Windows.Forms.CheckBox();
      this.btUpdateFromEdit = new System.Windows.Forms.Button();
      this.btDisableUnmapped = new System.Windows.Forms.Button();
      this.btCancelEdit = new System.Windows.Forms.Button();
      this.gbxFilters = new System.Windows.Forms.GroupBox();
      this.lblActionFilter = new System.Windows.Forms.Label();
      this.cbxShowJoystick = new System.Windows.Forms.CheckBox();
      this.cbxShowGamepad = new System.Windows.Forms.CheckBox();
      this.cbxShowMouse = new System.Windows.Forms.CheckBox();
      this.btClrFilterUsrBinding = new System.Windows.Forms.Button();
      this.cbxShowKeyboard = new System.Windows.Forms.CheckBox();
      this.lblUsrBindFilter = new System.Windows.Forms.Label();
      this.btClrFilterAction = new System.Windows.Forms.Button();
      this.txFilterUsrBinding = new System.Windows.Forms.TextBox();
      this.txFilterDefBinding = new System.Windows.Forms.TextBox();
      this.btClrFilterDefBinding = new System.Windows.Forms.Button();
      this.lblDefBindFilter = new System.Windows.Forms.Label();
      this.dS_ActionMaps = new SCJMapper_V2.Table.DS_ActionMaps();
      this.dSActionMapsBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gbxEditBindings.SuspendLayout();
      this.gbxFilters.SuspendLayout();
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
      this.DGV.Location = new System.Drawing.Point(303, 3);
      this.DGV.Name = "DGV";
      this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.DGV.Size = new System.Drawing.Size(438, 310);
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
      this.txFilterAction.Location = new System.Drawing.Point(109, 24);
      this.txFilterAction.Name = "txFilterAction";
      this.txFilterAction.Size = new System.Drawing.Size(106, 22);
      this.txFilterAction.TabIndex = 1;
      this.txFilterAction.TextChanged += new System.EventHandler(this.txFilterAction_TextChanged);
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.DGV, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 304F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(744, 316);
      this.tableLayoutPanel1.TabIndex = 2;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.lblLoadingData);
      this.panel1.Controls.Add(this.gbxEditBindings);
      this.panel1.Controls.Add(this.gbxFilters);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(294, 303);
      this.panel1.TabIndex = 1;
      // 
      // lblLoadingData
      // 
      this.lblLoadingData.AutoSize = true;
      this.lblLoadingData.BackColor = System.Drawing.SystemColors.Control;
      this.lblLoadingData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLoadingData.ForeColor = System.Drawing.Color.DarkRed;
      this.lblLoadingData.Location = new System.Drawing.Point(27, 282);
      this.lblLoadingData.Name = "lblLoadingData";
      this.lblLoadingData.Size = new System.Drawing.Size(85, 13);
      this.lblLoadingData.TabIndex = 17;
      this.lblLoadingData.Tag = "§";
      this.lblLoadingData.Text = "Loading data...";
      // 
      // gbxEditBindings
      // 
      this.gbxEditBindings.Controls.Add(this.chkEditDisabled);
      this.gbxEditBindings.Controls.Add(this.btUpdateFromEdit);
      this.gbxEditBindings.Controls.Add(this.btDisableUnmapped);
      this.gbxEditBindings.Controls.Add(this.btCancelEdit);
      this.gbxEditBindings.Location = new System.Drawing.Point(6, 163);
      this.gbxEditBindings.Name = "gbxEditBindings";
      this.gbxEditBindings.Size = new System.Drawing.Size(288, 107);
      this.gbxEditBindings.TabIndex = 16;
      this.gbxEditBindings.TabStop = false;
      this.gbxEditBindings.Tag = "§";
      this.gbxEditBindings.Text = "Edit";
      // 
      // chkEditDisabled
      // 
      this.chkEditDisabled.AutoSize = true;
      this.chkEditDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.chkEditDisabled.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkEditDisabled.ForeColor = System.Drawing.Color.Firebrick;
      this.chkEditDisabled.Location = new System.Drawing.Point(6, 21);
      this.chkEditDisabled.Name = "chkEditDisabled";
      this.chkEditDisabled.Size = new System.Drawing.Size(104, 17);
      this.chkEditDisabled.TabIndex = 12;
      this.chkEditDisabled.Tag = "§";
      this.chkEditDisabled.Text = "Edit \"Disabled\"";
      this.chkEditDisabled.UseVisualStyleBackColor = true;
      this.chkEditDisabled.CheckedChanged += new System.EventHandler(this.chkEditBlend_CheckedChanged);
      // 
      // btUpdateFromEdit
      // 
      this.btUpdateFromEdit.Enabled = false;
      this.btUpdateFromEdit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btUpdateFromEdit.ForeColor = System.Drawing.Color.Firebrick;
      this.btUpdateFromEdit.Location = new System.Drawing.Point(172, 59);
      this.btUpdateFromEdit.Name = "btUpdateFromEdit";
      this.btUpdateFromEdit.Size = new System.Drawing.Size(104, 39);
      this.btUpdateFromEdit.TabIndex = 12;
      this.btUpdateFromEdit.Tag = "§";
      this.btUpdateFromEdit.Text = "Accept Edits";
      this.btUpdateFromEdit.UseVisualStyleBackColor = true;
      this.btUpdateFromEdit.Click += new System.EventHandler(this.btUpdateFromEdit_Click);
      // 
      // btDisableUnmapped
      // 
      this.btDisableUnmapped.Enabled = false;
      this.btDisableUnmapped.Location = new System.Drawing.Point(9, 44);
      this.btDisableUnmapped.Name = "btDisableUnmapped";
      this.btDisableUnmapped.Size = new System.Drawing.Size(97, 42);
      this.btDisableUnmapped.TabIndex = 14;
      this.btDisableUnmapped.Tag = "§";
      this.btDisableUnmapped.Text = "Disable all Unmapped";
      this.btDisableUnmapped.UseVisualStyleBackColor = true;
      this.btDisableUnmapped.Click += new System.EventHandler(this.btBlendAll_Click);
      // 
      // btCancelEdit
      // 
      this.btCancelEdit.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btCancelEdit.ForeColor = System.Drawing.Color.DarkOliveGreen;
      this.btCancelEdit.Location = new System.Drawing.Point(172, 14);
      this.btCancelEdit.Name = "btCancelEdit";
      this.btCancelEdit.Size = new System.Drawing.Size(104, 39);
      this.btCancelEdit.TabIndex = 13;
      this.btCancelEdit.Tag = "§";
      this.btCancelEdit.Text = "Undo Edits";
      this.btCancelEdit.UseVisualStyleBackColor = true;
      this.btCancelEdit.Click += new System.EventHandler(this.btCancelEdit_Click);
      // 
      // gbxFilters
      // 
      this.gbxFilters.Controls.Add(this.lblActionFilter);
      this.gbxFilters.Controls.Add(this.txFilterAction);
      this.gbxFilters.Controls.Add(this.cbxShowJoystick);
      this.gbxFilters.Controls.Add(this.cbxShowGamepad);
      this.gbxFilters.Controls.Add(this.cbxShowMouse);
      this.gbxFilters.Controls.Add(this.btClrFilterUsrBinding);
      this.gbxFilters.Controls.Add(this.cbxShowKeyboard);
      this.gbxFilters.Controls.Add(this.lblUsrBindFilter);
      this.gbxFilters.Controls.Add(this.btClrFilterAction);
      this.gbxFilters.Controls.Add(this.txFilterUsrBinding);
      this.gbxFilters.Controls.Add(this.txFilterDefBinding);
      this.gbxFilters.Controls.Add(this.btClrFilterDefBinding);
      this.gbxFilters.Controls.Add(this.lblDefBindFilter);
      this.gbxFilters.Location = new System.Drawing.Point(3, 3);
      this.gbxFilters.Name = "gbxFilters";
      this.gbxFilters.Size = new System.Drawing.Size(288, 154);
      this.gbxFilters.TabIndex = 15;
      this.gbxFilters.TabStop = false;
      this.gbxFilters.Tag = "§";
      this.gbxFilters.Text = "Filters";
      // 
      // lblActionFilter
      // 
      this.lblActionFilter.AutoSize = true;
      this.lblActionFilter.Location = new System.Drawing.Point(6, 27);
      this.lblActionFilter.Name = "lblActionFilter";
      this.lblActionFilter.Size = new System.Drawing.Size(69, 13);
      this.lblActionFilter.TabIndex = 2;
      this.lblActionFilter.Tag = "§";
      this.lblActionFilter.Text = "Action Filter";
      // 
      // cbxShowJoystick
      // 
      this.cbxShowJoystick.AutoSize = true;
      this.cbxShowJoystick.Location = new System.Drawing.Point(9, 111);
      this.cbxShowJoystick.Name = "cbxShowJoystick";
      this.cbxShowJoystick.Size = new System.Drawing.Size(65, 17);
      this.cbxShowJoystick.TabIndex = 3;
      this.cbxShowJoystick.Tag = "§";
      this.cbxShowJoystick.Text = "Joystick";
      this.cbxShowJoystick.UseVisualStyleBackColor = true;
      this.cbxShowJoystick.CheckedChanged += new System.EventHandler(this.chkJoystick_CheckedChanged);
      // 
      // cbxShowGamepad
      // 
      this.cbxShowGamepad.AutoSize = true;
      this.cbxShowGamepad.Location = new System.Drawing.Point(133, 111);
      this.cbxShowGamepad.Name = "cbxShowGamepad";
      this.cbxShowGamepad.Size = new System.Drawing.Size(62, 17);
      this.cbxShowGamepad.TabIndex = 4;
      this.cbxShowGamepad.Tag = "§";
      this.cbxShowGamepad.Text = "Gamep";
      this.cbxShowGamepad.UseVisualStyleBackColor = true;
      this.cbxShowGamepad.CheckedChanged += new System.EventHandler(this.chkGamepad_CheckedChanged);
      // 
      // cbxShowMouse
      // 
      this.cbxShowMouse.AutoSize = true;
      this.cbxShowMouse.Location = new System.Drawing.Point(9, 134);
      this.cbxShowMouse.Name = "cbxShowMouse";
      this.cbxShowMouse.Size = new System.Drawing.Size(61, 17);
      this.cbxShowMouse.TabIndex = 4;
      this.cbxShowMouse.Tag = "§";
      this.cbxShowMouse.Text = "Mouse";
      this.cbxShowMouse.UseVisualStyleBackColor = true;
      this.cbxShowMouse.CheckedChanged += new System.EventHandler(this.chkMouse_CheckedChanged);
      // 
      // btClrFilterUsrBinding
      // 
      this.btClrFilterUsrBinding.Location = new System.Drawing.Point(221, 78);
      this.btClrFilterUsrBinding.Name = "btClrFilterUsrBinding";
      this.btClrFilterUsrBinding.Size = new System.Drawing.Size(58, 23);
      this.btClrFilterUsrBinding.TabIndex = 11;
      this.btClrFilterUsrBinding.Tag = "§";
      this.btClrFilterUsrBinding.Text = "Clear";
      this.btClrFilterUsrBinding.UseVisualStyleBackColor = true;
      this.btClrFilterUsrBinding.Click += new System.EventHandler(this.btClrFilterUsrBinding_Click);
      // 
      // cbxShowKeyboard
      // 
      this.cbxShowKeyboard.AutoSize = true;
      this.cbxShowKeyboard.Location = new System.Drawing.Point(133, 134);
      this.cbxShowKeyboard.Name = "cbxShowKeyboard";
      this.cbxShowKeyboard.Size = new System.Drawing.Size(46, 17);
      this.cbxShowKeyboard.TabIndex = 4;
      this.cbxShowKeyboard.Tag = "§";
      this.cbxShowKeyboard.Text = "Kbd";
      this.cbxShowKeyboard.UseVisualStyleBackColor = true;
      this.cbxShowKeyboard.CheckedChanged += new System.EventHandler(this.chkKbd_CheckedChanged);
      // 
      // lblUsrBindFilter
      // 
      this.lblUsrBindFilter.AutoSize = true;
      this.lblUsrBindFilter.Location = new System.Drawing.Point(6, 83);
      this.lblUsrBindFilter.Name = "lblUsrBindFilter";
      this.lblUsrBindFilter.Size = new System.Drawing.Size(80, 13);
      this.lblUsrBindFilter.TabIndex = 10;
      this.lblUsrBindFilter.Tag = "§";
      this.lblUsrBindFilter.Text = "Usr Bind Filter";
      // 
      // btClrFilterAction
      // 
      this.btClrFilterAction.Location = new System.Drawing.Point(221, 22);
      this.btClrFilterAction.Name = "btClrFilterAction";
      this.btClrFilterAction.Size = new System.Drawing.Size(58, 23);
      this.btClrFilterAction.TabIndex = 5;
      this.btClrFilterAction.Tag = "§";
      this.btClrFilterAction.Text = "Clear";
      this.btClrFilterAction.UseVisualStyleBackColor = true;
      this.btClrFilterAction.Click += new System.EventHandler(this.btClrFilterAction_Click);
      // 
      // txFilterUsrBinding
      // 
      this.txFilterUsrBinding.Location = new System.Drawing.Point(109, 80);
      this.txFilterUsrBinding.Name = "txFilterUsrBinding";
      this.txFilterUsrBinding.Size = new System.Drawing.Size(106, 22);
      this.txFilterUsrBinding.TabIndex = 9;
      this.txFilterUsrBinding.TextChanged += new System.EventHandler(this.txFilterUsrBinding_TextChanged);
      // 
      // txFilterDefBinding
      // 
      this.txFilterDefBinding.Location = new System.Drawing.Point(109, 52);
      this.txFilterDefBinding.Name = "txFilterDefBinding";
      this.txFilterDefBinding.Size = new System.Drawing.Size(106, 22);
      this.txFilterDefBinding.TabIndex = 6;
      this.txFilterDefBinding.TextChanged += new System.EventHandler(this.txFilterBinding_TextChanged);
      // 
      // btClrFilterDefBinding
      // 
      this.btClrFilterDefBinding.Location = new System.Drawing.Point(221, 50);
      this.btClrFilterDefBinding.Name = "btClrFilterDefBinding";
      this.btClrFilterDefBinding.Size = new System.Drawing.Size(58, 23);
      this.btClrFilterDefBinding.TabIndex = 8;
      this.btClrFilterDefBinding.Tag = "§";
      this.btClrFilterDefBinding.Text = "Clear";
      this.btClrFilterDefBinding.UseVisualStyleBackColor = true;
      this.btClrFilterDefBinding.Click += new System.EventHandler(this.btClrFilterBinding_Click);
      // 
      // lblDefBindFilter
      // 
      this.lblDefBindFilter.AutoSize = true;
      this.lblDefBindFilter.Location = new System.Drawing.Point(6, 55);
      this.lblDefBindFilter.Name = "lblDefBindFilter";
      this.lblDefBindFilter.Size = new System.Drawing.Size(81, 13);
      this.lblDefBindFilter.TabIndex = 7;
      this.lblDefBindFilter.Tag = "§";
      this.lblDefBindFilter.Text = "Def Bind Filter";
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
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(744, 316);
      this.Controls.Add(this.tableLayoutPanel1);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(760, 355);
      this.Name = "FormTable";
      this.Tag = "§";
      this.Text = "Actiontree as Table";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTable_FormClosing);
      this.Load += new System.EventHandler(this.FormTable_Load);
      this.LocationChanged += new System.EventHandler(this.FormTable_LocationChanged);
      this.SizeChanged += new System.EventHandler(this.FormTable_SizeChanged);
      ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.gbxEditBindings.ResumeLayout(false);
      this.gbxEditBindings.PerformLayout();
      this.gbxFilters.ResumeLayout(false);
      this.gbxFilters.PerformLayout();
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
    private System.Windows.Forms.Label lblDefBindFilter;
    private System.Windows.Forms.TextBox txFilterDefBinding;
    private System.Windows.Forms.Button btClrFilterAction;
    private System.Windows.Forms.CheckBox cbxShowKeyboard;
    private System.Windows.Forms.CheckBox cbxShowMouse;
    private System.Windows.Forms.CheckBox cbxShowGamepad;
    private System.Windows.Forms.CheckBox cbxShowJoystick;
    private System.Windows.Forms.Label lblActionFilter;
    private System.Windows.Forms.Button btClrFilterUsrBinding;
    private System.Windows.Forms.Label lblUsrBindFilter;
    private System.Windows.Forms.TextBox txFilterUsrBinding;
    private System.Windows.Forms.Button btUpdateFromEdit;
    private System.Windows.Forms.CheckBox chkEditDisabled;
    private System.Windows.Forms.Button btCancelEdit;
    private System.Windows.Forms.Button btDisableUnmapped;
    private System.Windows.Forms.GroupBox gbxFilters;
    private System.Windows.Forms.GroupBox gbxEditBindings;
    private System.Windows.Forms.Label lblLoadingData;
  }
}