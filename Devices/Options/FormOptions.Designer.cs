namespace SCJMapper_V2.Devices.Options
{
  partial class FormOptions
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
      if ( disposing && ( components != null ) ) components.Dispose( );
      if ( disposing && ( m_bSeries != null ) ) m_bSeries.Dispose( );
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tabC = new System.Windows.Forms.TabControl();
      this.tabOptions = new System.Windows.Forms.TabPage();
      this.lvOptionTree = new System.Windows.Forms.ListView();
      this.tabDevOption = new System.Windows.Forms.TabPage();
      this.pnlOptionInput = new System.Windows.Forms.Panel();
      this.rbUsePts = new System.Windows.Forms.RadioButton();
      this.rbUseExpo = new System.Windows.Forms.RadioButton();
      this.rbUseNone = new System.Windows.Forms.RadioButton();
      this.panel2 = new System.Windows.Forms.Panel();
      this.rbLivePtExponent = new System.Windows.Forms.RadioButton();
      this.lblLiveOutExponent = new System.Windows.Forms.Label();
      this.lblLiveIn1 = new System.Windows.Forms.Label();
      this.lblLiveOut1 = new System.Windows.Forms.Label();
      this.cbxLiveInvert = new System.Windows.Forms.CheckBox();
      this.lblLiveIn2 = new System.Windows.Forms.Label();
      this.lblLiveOut2 = new System.Windows.Forms.Label();
      this.lblLiveIn3 = new System.Windows.Forms.Label();
      this.rbLivePt3 = new System.Windows.Forms.RadioButton();
      this.lblLiveOut3 = new System.Windows.Forms.Label();
      this.rbLivePt2 = new System.Windows.Forms.RadioButton();
      this.label32 = new System.Windows.Forms.Label();
      this.rbLivePt1 = new System.Windows.Forms.RadioButton();
      this.label33 = new System.Windows.Forms.Label();
      this.lblGraphSaturation = new System.Windows.Forms.Label();
      this.lblGraphDeadzone = new System.Windows.Forms.Label();
      this.lblLiveNodetext = new System.Windows.Forms.Label();
      this.lblChart = new System.Windows.Forms.Label();
      this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.pnlDevOptionInput = new System.Windows.Forms.Panel();
      this.cbxUseSaturation = new System.Windows.Forms.CheckBox();
      this.cbxUseDeadzone = new System.Windows.Forms.CheckBox();
      this.panel3 = new System.Windows.Forms.Panel();
      this.lblLiveOutSaturation = new System.Windows.Forms.Label();
      this.tbSaturation = new System.Windows.Forms.TrackBar();
      this.tbDeadzone = new System.Windows.Forms.TrackBar();
      this.lblLiveOutDeadzone = new System.Windows.Forms.Label();
      this.pnlPreview = new System.Windows.Forms.Panel();
      this.btDebugStop = new System.Windows.Forms.Button();
      this.panel4 = new System.Windows.Forms.Panel();
      this.btDone = new System.Windows.Forms.Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.tabC.SuspendLayout();
      this.tabOptions.SuspendLayout();
      this.pnlOptionInput.SuspendLayout();
      this.panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
      this.pnlDevOptionInput.SuspendLayout();
      this.panel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tbSaturation)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.tbDeadzone)).BeginInit();
      this.pnlPreview.SuspendLayout();
      this.panel4.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
      this.tableLayoutPanel1.Controls.Add(this.tabC, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.pnlOptionInput, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.pnlDevOptionInput, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.pnlPreview, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 3);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 4;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 516F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(984, 726);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // tabC
      // 
      this.tabC.Controls.Add(this.tabOptions);
      this.tabC.Controls.Add(this.tabDevOption);
      this.tabC.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabC.Location = new System.Drawing.Point(3, 3);
      this.tabC.Name = "tabC";
      this.tableLayoutPanel1.SetRowSpan(this.tabC, 3);
      this.tabC.SelectedIndex = 0;
      this.tabC.Size = new System.Drawing.Size(638, 670);
      this.tabC.TabIndex = 3;
      this.tabC.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabC_Selecting);
      this.tabC.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabC_Selecting);
      // 
      // tabOptions
      // 
      this.tabOptions.Controls.Add(this.lvOptionTree);
      this.tabOptions.Location = new System.Drawing.Point(4, 22);
      this.tabOptions.Name = "tabOptions";
      this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
      this.tabOptions.Size = new System.Drawing.Size(630, 644);
      this.tabOptions.TabIndex = 0;
      this.tabOptions.Text = "Flight Options";
      this.tabOptions.UseVisualStyleBackColor = true;
      // 
      // lvOptionTree
      // 
      this.lvOptionTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lvOptionTree.Location = new System.Drawing.Point(3, 3);
      this.lvOptionTree.Name = "lvOptionTree";
      this.lvOptionTree.Size = new System.Drawing.Size(624, 638);
      this.lvOptionTree.TabIndex = 0;
      this.lvOptionTree.UseCompatibleStateImageBehavior = false;
      this.lvOptionTree.View = System.Windows.Forms.View.Details;
      this.lvOptionTree.SelectedIndexChanged += new System.EventHandler(this.lvOptionTree_SelectedIndexChanged);
      // 
      // tabDevOption
      // 
      this.tabDevOption.Location = new System.Drawing.Point(4, 22);
      this.tabDevOption.Name = "tabDevOption";
      this.tabDevOption.Padding = new System.Windows.Forms.Padding(3);
      this.tabDevOption.Size = new System.Drawing.Size(630, 644);
      this.tabDevOption.TabIndex = 1;
      this.tabDevOption.Text = "Device Options";
      this.tabDevOption.UseVisualStyleBackColor = true;
      // 
      // pnlOptionInput
      // 
      this.pnlOptionInput.BackColor = System.Drawing.Color.WhiteSmoke;
      this.pnlOptionInput.Controls.Add(this.rbUsePts);
      this.pnlOptionInput.Controls.Add(this.rbUseExpo);
      this.pnlOptionInput.Controls.Add(this.rbUseNone);
      this.pnlOptionInput.Controls.Add(this.panel2);
      this.pnlOptionInput.Controls.Add(this.lblGraphSaturation);
      this.pnlOptionInput.Controls.Add(this.lblGraphDeadzone);
      this.pnlOptionInput.Controls.Add(this.lblLiveNodetext);
      this.pnlOptionInput.Controls.Add(this.lblChart);
      this.pnlOptionInput.Controls.Add(this.chart1);
      this.pnlOptionInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlOptionInput.Location = new System.Drawing.Point(647, 28);
      this.pnlOptionInput.Name = "pnlOptionInput";
      this.pnlOptionInput.Size = new System.Drawing.Size(334, 510);
      this.pnlOptionInput.TabIndex = 6;
      // 
      // rbUsePts
      // 
      this.rbUsePts.AutoSize = true;
      this.rbUsePts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbUsePts.Location = new System.Drawing.Point(13, 396);
      this.rbUsePts.Name = "rbUsePts";
      this.rbUsePts.Size = new System.Drawing.Size(58, 17);
      this.rbUsePts.TabIndex = 57;
      this.rbUsePts.Text = "Curve";
      this.rbUsePts.UseVisualStyleBackColor = true;
      this.rbUsePts.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // rbUseExpo
      // 
      this.rbUseExpo.AutoSize = true;
      this.rbUseExpo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbUseExpo.Location = new System.Drawing.Point(13, 354);
      this.rbUseExpo.Name = "rbUseExpo";
      this.rbUseExpo.Size = new System.Drawing.Size(78, 17);
      this.rbUseExpo.TabIndex = 57;
      this.rbUseExpo.Text = "Exponent";
      this.rbUseExpo.UseVisualStyleBackColor = true;
      this.rbUseExpo.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // rbUseNone
      // 
      this.rbUseNone.AutoSize = true;
      this.rbUseNone.Checked = true;
      this.rbUseNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbUseNone.Location = new System.Drawing.Point(13, 482);
      this.rbUseNone.Name = "rbUseNone";
      this.rbUseNone.Size = new System.Drawing.Size(55, 17);
      this.rbUseNone.TabIndex = 56;
      this.rbUseNone.TabStop = true;
      this.rbUseNone.Text = "None";
      this.rbUseNone.UseVisualStyleBackColor = true;
      this.rbUseNone.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.Color.Gainsboro;
      this.panel2.Controls.Add(this.rbLivePtExponent);
      this.panel2.Controls.Add(this.lblLiveOutExponent);
      this.panel2.Controls.Add(this.lblLiveIn1);
      this.panel2.Controls.Add(this.lblLiveOut1);
      this.panel2.Controls.Add(this.cbxLiveInvert);
      this.panel2.Controls.Add(this.lblLiveIn2);
      this.panel2.Controls.Add(this.lblLiveOut2);
      this.panel2.Controls.Add(this.lblLiveIn3);
      this.panel2.Controls.Add(this.rbLivePt3);
      this.panel2.Controls.Add(this.lblLiveOut3);
      this.panel2.Controls.Add(this.rbLivePt2);
      this.panel2.Controls.Add(this.label32);
      this.panel2.Controls.Add(this.rbLivePt1);
      this.panel2.Controls.Add(this.label33);
      this.panel2.Location = new System.Drawing.Point(103, 345);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(220, 144);
      this.panel2.TabIndex = 55;
      // 
      // rbLivePtExponent
      // 
      this.rbLivePtExponent.AutoSize = true;
      this.rbLivePtExponent.Checked = true;
      this.rbLivePtExponent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbLivePtExponent.Location = new System.Drawing.Point(9, 9);
      this.rbLivePtExponent.Name = "rbLivePtExponent";
      this.rbLivePtExponent.Size = new System.Drawing.Size(84, 19);
      this.rbLivePtExponent.TabIndex = 33;
      this.rbLivePtExponent.TabStop = true;
      this.rbLivePtExponent.Tag = "§";
      this.rbLivePtExponent.Text = "Exp. Value:";
      this.rbLivePtExponent.UseVisualStyleBackColor = true;
      this.rbLivePtExponent.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // lblLiveOutExponent
      // 
      this.lblLiveOutExponent.AutoSize = true;
      this.lblLiveOutExponent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveOutExponent.Location = new System.Drawing.Point(107, 11);
      this.lblLiveOutExponent.Name = "lblLiveOutExponent";
      this.lblLiveOutExponent.Size = new System.Drawing.Size(34, 15);
      this.lblLiveOutExponent.TabIndex = 9;
      this.lblLiveOutExponent.Text = "0.000";
      // 
      // lblLiveIn1
      // 
      this.lblLiveIn1.AutoSize = true;
      this.lblLiveIn1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveIn1.Location = new System.Drawing.Point(84, 51);
      this.lblLiveIn1.Name = "lblLiveIn1";
      this.lblLiveIn1.Size = new System.Drawing.Size(28, 15);
      this.lblLiveIn1.TabIndex = 18;
      this.lblLiveIn1.Text = "0.25";
      // 
      // lblLiveOut1
      // 
      this.lblLiveOut1.AutoSize = true;
      this.lblLiveOut1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveOut1.Location = new System.Drawing.Point(137, 51);
      this.lblLiveOut1.Name = "lblLiveOut1";
      this.lblLiveOut1.Size = new System.Drawing.Size(28, 15);
      this.lblLiveOut1.TabIndex = 19;
      this.lblLiveOut1.Text = "0.25";
      // 
      // cbxLiveInvert
      // 
      this.cbxLiveInvert.AutoSize = true;
      this.cbxLiveInvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbxLiveInvert.Location = new System.Drawing.Point(9, 120);
      this.cbxLiveInvert.Name = "cbxLiveInvert";
      this.cbxLiveInvert.Size = new System.Drawing.Size(59, 17);
      this.cbxLiveInvert.TabIndex = 54;
      this.cbxLiveInvert.Tag = "§";
      this.cbxLiveInvert.Text = "Invert";
      this.cbxLiveInvert.UseVisualStyleBackColor = true;
      this.cbxLiveInvert.CheckedChanged += new System.EventHandler(this.cbxInvert_CheckedChanged);
      // 
      // lblLiveIn2
      // 
      this.lblLiveIn2.AutoSize = true;
      this.lblLiveIn2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveIn2.Location = new System.Drawing.Point(84, 74);
      this.lblLiveIn2.Name = "lblLiveIn2";
      this.lblLiveIn2.Size = new System.Drawing.Size(22, 15);
      this.lblLiveIn2.TabIndex = 20;
      this.lblLiveIn2.Text = "0.5";
      // 
      // lblLiveOut2
      // 
      this.lblLiveOut2.AutoSize = true;
      this.lblLiveOut2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveOut2.Location = new System.Drawing.Point(137, 74);
      this.lblLiveOut2.Name = "lblLiveOut2";
      this.lblLiveOut2.Size = new System.Drawing.Size(22, 15);
      this.lblLiveOut2.TabIndex = 21;
      this.lblLiveOut2.Text = "0.5";
      // 
      // lblLiveIn3
      // 
      this.lblLiveIn3.AutoSize = true;
      this.lblLiveIn3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveIn3.Location = new System.Drawing.Point(84, 97);
      this.lblLiveIn3.Name = "lblLiveIn3";
      this.lblLiveIn3.Size = new System.Drawing.Size(28, 15);
      this.lblLiveIn3.TabIndex = 22;
      this.lblLiveIn3.Text = "0.75";
      // 
      // rbLivePt3
      // 
      this.rbLivePt3.AutoSize = true;
      this.rbLivePt3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbLivePt3.Location = new System.Drawing.Point(9, 95);
      this.rbLivePt3.Name = "rbLivePt3";
      this.rbLivePt3.Size = new System.Drawing.Size(67, 19);
      this.rbLivePt3.TabIndex = 31;
      this.rbLivePt3.Text = "Point 3:";
      this.rbLivePt3.UseVisualStyleBackColor = true;
      this.rbLivePt3.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // lblLiveOut3
      // 
      this.lblLiveOut3.AutoSize = true;
      this.lblLiveOut3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveOut3.Location = new System.Drawing.Point(137, 97);
      this.lblLiveOut3.Name = "lblLiveOut3";
      this.lblLiveOut3.Size = new System.Drawing.Size(28, 15);
      this.lblLiveOut3.TabIndex = 23;
      this.lblLiveOut3.Text = "0.75";
      // 
      // rbLivePt2
      // 
      this.rbLivePt2.AutoSize = true;
      this.rbLivePt2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbLivePt2.Location = new System.Drawing.Point(9, 72);
      this.rbLivePt2.Name = "rbLivePt2";
      this.rbLivePt2.Size = new System.Drawing.Size(67, 19);
      this.rbLivePt2.TabIndex = 30;
      this.rbLivePt2.Text = "Point 2:";
      this.rbLivePt2.UseVisualStyleBackColor = true;
      this.rbLivePt2.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label32.Location = new System.Drawing.Point(84, 31);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(35, 15);
      this.label32.TabIndex = 27;
      this.label32.Text = "IN(x)";
      // 
      // rbLivePt1
      // 
      this.rbLivePt1.AutoSize = true;
      this.rbLivePt1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbLivePt1.Location = new System.Drawing.Point(9, 49);
      this.rbLivePt1.Name = "rbLivePt1";
      this.rbLivePt1.Size = new System.Drawing.Size(67, 19);
      this.rbLivePt1.TabIndex = 29;
      this.rbLivePt1.Text = "Point 1:";
      this.rbLivePt1.UseVisualStyleBackColor = true;
      this.rbLivePt1.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // label33
      // 
      this.label33.AutoSize = true;
      this.label33.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label33.Location = new System.Drawing.Point(130, 31);
      this.label33.Name = "label33";
      this.label33.Size = new System.Drawing.Size(46, 15);
      this.label33.TabIndex = 28;
      this.label33.Text = "OUT(y)";
      // 
      // lblGraphSaturation
      // 
      this.lblGraphSaturation.AutoSize = true;
      this.lblGraphSaturation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblGraphSaturation.Location = new System.Drawing.Point(289, 295);
      this.lblGraphSaturation.Name = "lblGraphSaturation";
      this.lblGraphSaturation.Size = new System.Drawing.Size(34, 15);
      this.lblGraphSaturation.TabIndex = 53;
      this.lblGraphSaturation.Text = "0.000";
      // 
      // lblGraphDeadzone
      // 
      this.lblGraphDeadzone.AutoSize = true;
      this.lblGraphDeadzone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblGraphDeadzone.Location = new System.Drawing.Point(10, 295);
      this.lblGraphDeadzone.Name = "lblGraphDeadzone";
      this.lblGraphDeadzone.Size = new System.Drawing.Size(34, 15);
      this.lblGraphDeadzone.TabIndex = 52;
      this.lblGraphDeadzone.Text = "0.000";
      // 
      // lblLiveNodetext
      // 
      this.lblLiveNodetext.AutoSize = true;
      this.lblLiveNodetext.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveNodetext.ForeColor = System.Drawing.Color.ForestGreen;
      this.lblLiveNodetext.Location = new System.Drawing.Point(13, 321);
      this.lblLiveNodetext.Name = "lblLiveNodetext";
      this.lblLiveNodetext.Size = new System.Drawing.Size(16, 13);
      this.lblLiveNodetext.TabIndex = 51;
      this.lblLiveNodetext.Text = "...";
      // 
      // lblChart
      // 
      this.lblChart.Location = new System.Drawing.Point(50, 295);
      this.lblChart.Name = "lblChart";
      this.lblChart.Size = new System.Drawing.Size(233, 15);
      this.lblChart.TabIndex = 36;
      this.lblChart.Tag = "§";
      this.lblChart.Text = "Select an option then click and drag";
      this.lblChart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // chart1
      // 
      chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
      chartArea1.AxisX.IsMarginVisible = false;
      chartArea1.AxisX.LabelStyle.Enabled = false;
      chartArea1.AxisX.MajorGrid.Interval = 0.2D;
      chartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisX.MajorTickMark.Interval = 0.2D;
      chartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisX.Maximum = 1D;
      chartArea1.AxisX.Minimum = 0D;
      chartArea1.AxisX.MinorGrid.Interval = 0.1D;
      chartArea1.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisX.MinorTickMark.Interval = 0.1D;
      chartArea1.AxisX.MinorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisY.Crossing = -1.7976931348623157E+308D;
      chartArea1.AxisY.Interval = 5D;
      chartArea1.AxisY.IsMarginVisible = false;
      chartArea1.AxisY.LabelStyle.Enabled = false;
      chartArea1.AxisY.MajorGrid.Interval = 0.2D;
      chartArea1.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisY.MajorTickMark.Interval = 0.2D;
      chartArea1.AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
      chartArea1.AxisY.Maximum = 1D;
      chartArea1.AxisY.Minimum = 0D;
      chartArea1.Name = "ChartArea1";
      this.chart1.ChartAreas.Add(chartArea1);
      this.chart1.Location = new System.Drawing.Point(16, 3);
      this.chart1.Name = "chart1";
      series1.ChartArea = "ChartArea1";
      series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
      series1.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
      series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
      series1.Name = "Series1";
      this.chart1.Series.Add(series1);
      this.chart1.Size = new System.Drawing.Size(301, 295);
      this.chart1.TabIndex = 16;
      this.chart1.Text = "chart1";
      this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartPoint_MouseDown);
      this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartPoint_MouseMove);
      this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chartPoint_MouseUp);
      // 
      // pnlDevOptionInput
      // 
      this.pnlDevOptionInput.BackColor = System.Drawing.Color.WhiteSmoke;
      this.pnlDevOptionInput.Controls.Add(this.cbxUseSaturation);
      this.pnlDevOptionInput.Controls.Add(this.cbxUseDeadzone);
      this.pnlDevOptionInput.Controls.Add(this.panel3);
      this.pnlDevOptionInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlDevOptionInput.Location = new System.Drawing.Point(647, 544);
      this.pnlDevOptionInput.Name = "pnlDevOptionInput";
      this.pnlDevOptionInput.Size = new System.Drawing.Size(334, 129);
      this.pnlDevOptionInput.TabIndex = 7;
      // 
      // cbxUseSaturation
      // 
      this.cbxUseSaturation.AutoSize = true;
      this.cbxUseSaturation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbxUseSaturation.Location = new System.Drawing.Point(13, 76);
      this.cbxUseSaturation.Name = "cbxUseSaturation";
      this.cbxUseSaturation.Size = new System.Drawing.Size(84, 17);
      this.cbxUseSaturation.TabIndex = 57;
      this.cbxUseSaturation.Text = "Saturation";
      this.cbxUseSaturation.UseVisualStyleBackColor = true;
      this.cbxUseSaturation.CheckedChanged += new System.EventHandler(this.cbxUseSaturation_CheckedChanged);
      // 
      // cbxUseDeadzone
      // 
      this.cbxUseDeadzone.AutoSize = true;
      this.cbxUseDeadzone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbxUseDeadzone.Location = new System.Drawing.Point(13, 25);
      this.cbxUseDeadzone.Name = "cbxUseDeadzone";
      this.cbxUseDeadzone.Size = new System.Drawing.Size(83, 17);
      this.cbxUseDeadzone.TabIndex = 56;
      this.cbxUseDeadzone.Text = "Deadzone";
      this.cbxUseDeadzone.UseVisualStyleBackColor = true;
      this.cbxUseDeadzone.CheckedChanged += new System.EventHandler(this.cbxUseDeadzone_CheckedChanged);
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.Color.Gainsboro;
      this.panel3.Controls.Add(this.lblLiveOutSaturation);
      this.panel3.Controls.Add(this.tbSaturation);
      this.panel3.Controls.Add(this.tbDeadzone);
      this.panel3.Controls.Add(this.lblLiveOutDeadzone);
      this.panel3.Location = new System.Drawing.Point(103, 5);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(220, 109);
      this.panel3.TabIndex = 55;
      // 
      // lblLiveOutSaturation
      // 
      this.lblLiveOutSaturation.AutoSize = true;
      this.lblLiveOutSaturation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveOutSaturation.Location = new System.Drawing.Point(180, 71);
      this.lblLiveOutSaturation.Name = "lblLiveOutSaturation";
      this.lblLiveOutSaturation.Size = new System.Drawing.Size(34, 15);
      this.lblLiveOutSaturation.TabIndex = 15;
      this.lblLiveOutSaturation.Text = "0.000";
      // 
      // tbSaturation
      // 
      this.tbSaturation.Location = new System.Drawing.Point(9, 58);
      this.tbSaturation.Maximum = 40;
      this.tbSaturation.Name = "tbSaturation";
      this.tbSaturation.Size = new System.Drawing.Size(165, 45);
      this.tbSaturation.TabIndex = 14;
      this.tbSaturation.TickFrequency = 5;
      this.tbSaturation.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.tbSaturation.ValueChanged += new System.EventHandler(this.tbSlider_ValueChanged);
      // 
      // tbDeadzone
      // 
      this.tbDeadzone.Location = new System.Drawing.Point(9, 7);
      this.tbDeadzone.Maximum = 40;
      this.tbDeadzone.Name = "tbDeadzone";
      this.tbDeadzone.Size = new System.Drawing.Size(165, 45);
      this.tbDeadzone.TabIndex = 0;
      this.tbDeadzone.TickFrequency = 5;
      this.tbDeadzone.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.tbDeadzone.ValueChanged += new System.EventHandler(this.tbSlider_ValueChanged);
      // 
      // lblLiveOutDeadzone
      // 
      this.lblLiveOutDeadzone.AutoSize = true;
      this.lblLiveOutDeadzone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveOutDeadzone.Location = new System.Drawing.Point(180, 20);
      this.lblLiveOutDeadzone.Name = "lblLiveOutDeadzone";
      this.lblLiveOutDeadzone.Size = new System.Drawing.Size(34, 15);
      this.lblLiveOutDeadzone.TabIndex = 13;
      this.lblLiveOutDeadzone.Text = "0.000";
      // 
      // pnlPreview
      // 
      this.pnlPreview.Controls.Add(this.btDebugStop);
      this.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlPreview.Location = new System.Drawing.Point(3, 679);
      this.pnlPreview.Name = "pnlPreview";
      this.pnlPreview.Size = new System.Drawing.Size(638, 44);
      this.pnlPreview.TabIndex = 8;
      // 
      // btDebugStop
      // 
      this.btDebugStop.Location = new System.Drawing.Point(233, 3);
      this.btDebugStop.Name = "btDebugStop";
      this.btDebugStop.Size = new System.Drawing.Size(134, 27);
      this.btDebugStop.TabIndex = 2;
      this.btDebugStop.Text = "Debug Stop  only";
      this.btDebugStop.UseVisualStyleBackColor = true;
      this.btDebugStop.Visible = false;
      this.btDebugStop.Click += new System.EventHandler(this.button2_Click);
      // 
      // panel4
      // 
      this.panel4.AutoSize = true;
      this.panel4.Controls.Add(this.btDone);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel4.Location = new System.Drawing.Point(647, 679);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(334, 44);
      this.panel4.TabIndex = 9;
      // 
      // btDone
      // 
      this.btDone.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btDone.Location = new System.Drawing.Point(193, 8);
      this.btDone.Name = "btDone";
      this.btDone.Size = new System.Drawing.Size(136, 32);
      this.btDone.TabIndex = 0;
      this.btDone.Tag = "§";
      this.btDone.Text = "Done";
      this.btDone.UseVisualStyleBackColor = true;
      this.btDone.Click += new System.EventHandler(this.btExit_Click);
      // 
      // FormOptions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(984, 726);
      this.Controls.Add(this.tableLayoutPanel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(1000, 765);
      this.Name = "FormOptions";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Tag = "§";
      this.Text = "SCJMapper - Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOptions_FormClosing);
      this.Load += new System.EventHandler(this.FormOptions_Load);
      this.LocationChanged += new System.EventHandler(this.FormOptions_LocationChanged);
      this.SizeChanged += new System.EventHandler(this.FormOptions_SizeChanged);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tabC.ResumeLayout(false);
      this.tabOptions.ResumeLayout(false);
      this.pnlOptionInput.ResumeLayout(false);
      this.pnlOptionInput.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
      this.pnlDevOptionInput.ResumeLayout(false);
      this.pnlDevOptionInput.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tbSaturation)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.tbDeadzone)).EndInit();
      this.pnlPreview.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.ListView lvOptionTree;
    private System.Windows.Forms.Button btDebugStop;
    private System.Windows.Forms.TabControl tabC;
    private System.Windows.Forms.TabPage tabOptions;
    private System.Windows.Forms.TabPage tabDevOption;
    private System.Windows.Forms.ListView lviewlvDevOptions;
    private System.Windows.Forms.Panel pnlOptionInput;
    private System.Windows.Forms.RadioButton rbUsePts;
    private System.Windows.Forms.RadioButton rbUseExpo;
    private System.Windows.Forms.RadioButton rbUseNone;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.RadioButton rbLivePtExponent;
    private System.Windows.Forms.Label lblLiveOutExponent;
    private System.Windows.Forms.Label lblLiveIn1;
    private System.Windows.Forms.Label lblLiveOut1;
    private System.Windows.Forms.CheckBox cbxLiveInvert;
    private System.Windows.Forms.Label lblLiveIn2;
    private System.Windows.Forms.Label lblLiveOut2;
    private System.Windows.Forms.Label lblLiveIn3;
    private System.Windows.Forms.RadioButton rbLivePt3;
    private System.Windows.Forms.Label lblLiveOut3;
    private System.Windows.Forms.RadioButton rbLivePt2;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.RadioButton rbLivePt1;
    private System.Windows.Forms.Label label33;
    private System.Windows.Forms.Label lblGraphSaturation;
    private System.Windows.Forms.Label lblGraphDeadzone;
    private System.Windows.Forms.Label lblLiveNodetext;
    private System.Windows.Forms.Label lblChart;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    private System.Windows.Forms.Panel pnlDevOptionInput;
    private System.Windows.Forms.TrackBar tbDeadzone;
    private System.Windows.Forms.Label lblLiveOutDeadzone;
    private System.Windows.Forms.CheckBox cbxUseSaturation;
    private System.Windows.Forms.CheckBox cbxUseDeadzone;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Label lblLiveOutSaturation;
    private System.Windows.Forms.TrackBar tbSaturation;
    private System.Windows.Forms.Panel pnlPreview;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Button btDone;
  }
}