namespace SCJMapper_V2.OGL
{
  partial class FormJSCalCurve
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJSCalCurve));
      this.glControl1 = new OpenTK.GLControl();
      this.tlp = new System.Windows.Forms.TableLayoutPanel();
      this.tlpData = new System.Windows.Forms.TableLayoutPanel();
      this.panel2 = new System.Windows.Forms.Panel();
      this.pnlYaw = new System.Windows.Forms.Panel();
      this.lblYnt = new System.Windows.Forms.Label();
      this.cbxYinvert = new System.Windows.Forms.CheckBox();
      this.cbxYpts = new System.Windows.Forms.CheckBox();
      this.cbxYexpo = new System.Windows.Forms.CheckBox();
      this.cbxYsat = new System.Windows.Forms.CheckBox();
      this.cbxYdeadzone = new System.Windows.Forms.CheckBox();
      this.label25 = new System.Windows.Forms.Label();
      this.label24 = new System.Windows.Forms.Label();
      this.label23 = new System.Windows.Forms.Label();
      this.lblYout3 = new System.Windows.Forms.Label();
      this.lblYin3 = new System.Windows.Forms.Label();
      this.lblYout2 = new System.Windows.Forms.Label();
      this.lblYin2 = new System.Windows.Forms.Label();
      this.lblYout1 = new System.Windows.Forms.Label();
      this.lblYin1 = new System.Windows.Forms.Label();
      this.lblYsat = new System.Windows.Forms.Label();
      this.lblYexponent = new System.Windows.Forms.Label();
      this.lblYdeadzone = new System.Windows.Forms.Label();
      this.lblYCmd = new System.Windows.Forms.Label();
      this.lblYaw = new System.Windows.Forms.Label();
      this.pnlPitch = new System.Windows.Forms.Panel();
      this.lblPnt = new System.Windows.Forms.Label();
      this.cbxPinvert = new System.Windows.Forms.CheckBox();
      this.cbxPpts = new System.Windows.Forms.CheckBox();
      this.cbxPexpo = new System.Windows.Forms.CheckBox();
      this.cbxPsat = new System.Windows.Forms.CheckBox();
      this.cbxPdeadzone = new System.Windows.Forms.CheckBox();
      this.label26 = new System.Windows.Forms.Label();
      this.label27 = new System.Windows.Forms.Label();
      this.label28 = new System.Windows.Forms.Label();
      this.lblPout3 = new System.Windows.Forms.Label();
      this.lblPin3 = new System.Windows.Forms.Label();
      this.lblPout2 = new System.Windows.Forms.Label();
      this.lblPin2 = new System.Windows.Forms.Label();
      this.lblPout1 = new System.Windows.Forms.Label();
      this.lblPin1 = new System.Windows.Forms.Label();
      this.lblPsat = new System.Windows.Forms.Label();
      this.lblPexponent = new System.Windows.Forms.Label();
      this.lblPdeadzone = new System.Windows.Forms.Label();
      this.lblPCmd = new System.Windows.Forms.Label();
      this.lblPitch = new System.Windows.Forms.Label();
      this.pnlRoll = new System.Windows.Forms.Panel();
      this.lblRnt = new System.Windows.Forms.Label();
      this.cbxRinvert = new System.Windows.Forms.CheckBox();
      this.cbxRpts = new System.Windows.Forms.CheckBox();
      this.cbxRexpo = new System.Windows.Forms.CheckBox();
      this.cbxRsat = new System.Windows.Forms.CheckBox();
      this.cbxRdeadzone = new System.Windows.Forms.CheckBox();
      this.label35 = new System.Windows.Forms.Label();
      this.label36 = new System.Windows.Forms.Label();
      this.label37 = new System.Windows.Forms.Label();
      this.lblRout3 = new System.Windows.Forms.Label();
      this.lblRin3 = new System.Windows.Forms.Label();
      this.lblRout2 = new System.Windows.Forms.Label();
      this.lblRin2 = new System.Windows.Forms.Label();
      this.lblRout1 = new System.Windows.Forms.Label();
      this.lblRin1 = new System.Windows.Forms.Label();
      this.lblRsat = new System.Windows.Forms.Label();
      this.lblRexponent = new System.Windows.Forms.Label();
      this.lblRdeadzone = new System.Windows.Forms.Label();
      this.lblRCmd = new System.Windows.Forms.Label();
      this.lblRoll = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.panel9 = new System.Windows.Forms.Panel();
      this.label6 = new System.Windows.Forms.Label();
      this.lblDamping = new System.Windows.Forms.Label();
      this.slDamping = new System.Windows.Forms.TrackBar();
      this.panel1 = new System.Windows.Forms.Panel();
      this.label16 = new System.Windows.Forms.Label();
      this.lblTurnspeed = new System.Windows.Forms.Label();
      this.slTurnSpeed = new System.Windows.Forms.TrackBar();
      this.panel6 = new System.Windows.Forms.Panel();
      this.rbPtDeadzone = new System.Windows.Forms.RadioButton();
      this.lblGraphSaturation = new System.Windows.Forms.Label();
      this.lblGraphDeadzone = new System.Windows.Forms.Label();
      this.lblNodetext = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.cbRuse = new System.Windows.Forms.CheckBox();
      this.cbPuse = new System.Windows.Forms.CheckBox();
      this.cbYuse = new System.Windows.Forms.CheckBox();
      this.label11 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.lblROutput = new System.Windows.Forms.Label();
      this.lblRInput = new System.Windows.Forms.Label();
      this.lblPOutput = new System.Windows.Forms.Label();
      this.lblPInput = new System.Windows.Forms.Label();
      this.lblLiveRoll = new System.Windows.Forms.Label();
      this.lblLivePitch = new System.Windows.Forms.Label();
      this.btCopyToAllAxis = new System.Windows.Forms.Button();
      this.lblLiveYaw = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.lblYOutput = new System.Windows.Forms.Label();
      this.lblYInput = new System.Windows.Forms.Label();
      this.rbPtExponent = new System.Windows.Forms.RadioButton();
      this.rbPtSaturation = new System.Windows.Forms.RadioButton();
      this.rbPt3 = new System.Windows.Forms.RadioButton();
      this.rbPt2 = new System.Windows.Forms.RadioButton();
      this.rbPt1 = new System.Windows.Forms.RadioButton();
      this.label33 = new System.Windows.Forms.Label();
      this.label32 = new System.Windows.Forms.Label();
      this.lblOut3 = new System.Windows.Forms.Label();
      this.lblIn3 = new System.Windows.Forms.Label();
      this.lblOut2 = new System.Windows.Forms.Label();
      this.lblIn2 = new System.Windows.Forms.Label();
      this.lblOut1 = new System.Windows.Forms.Label();
      this.lblIn1 = new System.Windows.Forms.Label();
      this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.lblOutSlider = new System.Windows.Forms.Label();
      this.lblOutExponent = new System.Windows.Forms.Label();
      this.tbSlider = new System.Windows.Forms.TrackBar();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.rb300 = new System.Windows.Forms.RadioButton();
      this.rbHornet = new System.Windows.Forms.RadioButton();
      this.rbAurora = new System.Windows.Forms.RadioButton();
      this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
      this.btDone = new System.Windows.Forms.Button();
      this.panel8 = new System.Windows.Forms.Panel();
      this.rbSunset = new System.Windows.Forms.RadioButton();
      this.rbOutThere3 = new System.Windows.Forms.RadioButton();
      this.rbSkybox = new System.Windows.Forms.RadioButton();
      this.rbOutThere1 = new System.Windows.Forms.RadioButton();
      this.rbBigSight = new System.Windows.Forms.RadioButton();
      this.rbHighway = new System.Windows.Forms.RadioButton();
      this.rbHelipad = new System.Windows.Forms.RadioButton();
      this.rbShiodome = new System.Windows.Forms.RadioButton();
      this.rbCanyon = new System.Windows.Forms.RadioButton();
      this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
      this.pnlAxisSelector = new System.Windows.Forms.Panel();
      this.rbP = new System.Windows.Forms.RadioButton();
      this.rbY = new System.Windows.Forms.RadioButton();
      this.rbR = new System.Windows.Forms.RadioButton();
      this.panel10 = new System.Windows.Forms.Panel();
      this.rbTuneStrafe = new System.Windows.Forms.RadioButton();
      this.rbTuneYPR = new System.Windows.Forms.RadioButton();
      this.tlp.SuspendLayout();
      this.tlpData.SuspendLayout();
      this.pnlYaw.SuspendLayout();
      this.pnlPitch.SuspendLayout();
      this.pnlRoll.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel9.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.slDamping)).BeginInit();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.slTurnSpeed)).BeginInit();
      this.panel6.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.tbSlider)).BeginInit();
      this.flowLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.panel8.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.pnlAxisSelector.SuspendLayout();
      this.panel10.SuspendLayout();
      this.SuspendLayout();
      // 
      // glControl1
      // 
      this.glControl1.BackColor = System.Drawing.Color.Black;
      this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.glControl1.Location = new System.Drawing.Point(153, 3);
      this.glControl1.Name = "glControl1";
      this.glControl1.Size = new System.Drawing.Size(1028, 610);
      this.glControl1.TabIndex = 0;
      this.glControl1.VSync = false;
      this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
      this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
      this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
      // 
      // tlp
      // 
      this.tlp.ColumnCount = 2;
      this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
      this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlp.Controls.Add(this.tlpData, 0, 0);
      this.tlp.Controls.Add(this.tableLayoutPanel1, 1, 1);
      this.tlp.Controls.Add(this.glControl1, 1, 0);
      this.tlp.Controls.Add(this.flowLayoutPanel2, 0, 1);
      this.tlp.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tlp.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tlp.Location = new System.Drawing.Point(0, 0);
      this.tlp.Name = "tlp";
      this.tlp.RowCount = 2;
      this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
      this.tlp.Size = new System.Drawing.Size(1184, 916);
      this.tlp.TabIndex = 1;
      // 
      // tlpData
      // 
      this.tlpData.BackColor = System.Drawing.Color.Gold;
      this.tlpData.ColumnCount = 1;
      this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tlpData.Controls.Add(this.panel2, 0, 0);
      this.tlpData.Controls.Add(this.pnlYaw, 0, 1);
      this.tlpData.Controls.Add(this.pnlPitch, 0, 3);
      this.tlpData.Controls.Add(this.pnlRoll, 0, 5);
      this.tlpData.Dock = System.Windows.Forms.DockStyle.Top;
      this.tlpData.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tlpData.Location = new System.Drawing.Point(3, 3);
      this.tlpData.Name = "tlpData";
      this.tlpData.RowCount = 8;
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 155F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 155F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 155F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
      this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tlpData.Size = new System.Drawing.Size(144, 610);
      this.tlpData.TabIndex = 2;
      // 
      // panel2
      // 
      this.panel2.BackgroundImage = global::SCJMapper_V2.Properties.Resources.YPR;
      this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(3, 3);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(138, 114);
      this.panel2.TabIndex = 0;
      // 
      // pnlYaw
      // 
      this.pnlYaw.BackColor = System.Drawing.Color.PowderBlue;
      this.pnlYaw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pnlYaw.Controls.Add(this.lblYnt);
      this.pnlYaw.Controls.Add(this.cbxYinvert);
      this.pnlYaw.Controls.Add(this.cbxYpts);
      this.pnlYaw.Controls.Add(this.cbxYexpo);
      this.pnlYaw.Controls.Add(this.cbxYsat);
      this.pnlYaw.Controls.Add(this.cbxYdeadzone);
      this.pnlYaw.Controls.Add(this.label25);
      this.pnlYaw.Controls.Add(this.label24);
      this.pnlYaw.Controls.Add(this.label23);
      this.pnlYaw.Controls.Add(this.lblYout3);
      this.pnlYaw.Controls.Add(this.lblYin3);
      this.pnlYaw.Controls.Add(this.lblYout2);
      this.pnlYaw.Controls.Add(this.lblYin2);
      this.pnlYaw.Controls.Add(this.lblYout1);
      this.pnlYaw.Controls.Add(this.lblYin1);
      this.pnlYaw.Controls.Add(this.lblYsat);
      this.pnlYaw.Controls.Add(this.lblYexponent);
      this.pnlYaw.Controls.Add(this.lblYdeadzone);
      this.pnlYaw.Controls.Add(this.lblYCmd);
      this.pnlYaw.Controls.Add(this.lblYaw);
      this.pnlYaw.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlYaw.Location = new System.Drawing.Point(3, 123);
      this.pnlYaw.Name = "pnlYaw";
      this.pnlYaw.Size = new System.Drawing.Size(138, 149);
      this.pnlYaw.TabIndex = 2;
      // 
      // lblYnt
      // 
      this.lblYnt.AutoSize = true;
      this.lblYnt.Location = new System.Drawing.Point(107, 17);
      this.lblYnt.Name = "lblYnt";
      this.lblYnt.Size = new System.Drawing.Size(20, 13);
      this.lblYnt.TabIndex = 23;
      this.lblYnt.Text = "NT";
      this.lblYnt.Visible = false;
      // 
      // cbxYinvert
      // 
      this.cbxYinvert.AutoSize = true;
      this.cbxYinvert.Location = new System.Drawing.Point(6, 17);
      this.cbxYinvert.Name = "cbxYinvert";
      this.cbxYinvert.Size = new System.Drawing.Size(55, 17);
      this.cbxYinvert.TabIndex = 22;
      this.cbxYinvert.Text = "Invert";
      this.cbxYinvert.UseVisualStyleBackColor = true;
      this.cbxYinvert.CheckedChanged += new System.EventHandler(this.cbxYinvert_CheckedChanged);
      // 
      // cbxYpts
      // 
      this.cbxYpts.AutoSize = true;
      this.cbxYpts.Location = new System.Drawing.Point(6, 91);
      this.cbxYpts.Name = "cbxYpts";
      this.cbxYpts.Size = new System.Drawing.Size(15, 14);
      this.cbxYpts.TabIndex = 21;
      this.cbxYpts.UseVisualStyleBackColor = true;
      this.cbxYpts.CheckedChanged += new System.EventHandler(this.cbxYpts_CheckedChanged);
      // 
      // cbxYexpo
      // 
      this.cbxYexpo.AutoSize = true;
      this.cbxYexpo.Location = new System.Drawing.Point(6, 71);
      this.cbxYexpo.Name = "cbxYexpo";
      this.cbxYexpo.Size = new System.Drawing.Size(75, 17);
      this.cbxYexpo.TabIndex = 20;
      this.cbxYexpo.Text = "Exponent";
      this.cbxYexpo.UseVisualStyleBackColor = true;
      this.cbxYexpo.CheckedChanged += new System.EventHandler(this.cbxYexpo_CheckedChanged);
      // 
      // cbxYsat
      // 
      this.cbxYsat.AutoSize = true;
      this.cbxYsat.Location = new System.Drawing.Point(6, 53);
      this.cbxYsat.Name = "cbxYsat";
      this.cbxYsat.Size = new System.Drawing.Size(80, 17);
      this.cbxYsat.TabIndex = 19;
      this.cbxYsat.Text = "Saturation";
      this.cbxYsat.UseVisualStyleBackColor = true;
      this.cbxYsat.CheckedChanged += new System.EventHandler(this.cbxYsense_CheckedChanged);
      // 
      // cbxYdeadzone
      // 
      this.cbxYdeadzone.AutoSize = true;
      this.cbxYdeadzone.Location = new System.Drawing.Point(6, 35);
      this.cbxYdeadzone.Name = "cbxYdeadzone";
      this.cbxYdeadzone.Size = new System.Drawing.Size(78, 17);
      this.cbxYdeadzone.TabIndex = 18;
      this.cbxYdeadzone.Text = "Deadzone";
      this.cbxYdeadzone.UseVisualStyleBackColor = true;
      this.cbxYdeadzone.CheckedChanged += new System.EventHandler(this.cbxYdeadzone_CheckedChanged);
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(37, 127);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(23, 13);
      this.label25.TabIndex = 17;
      this.label25.Text = "Pt3";
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(37, 109);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(23, 13);
      this.label24.TabIndex = 16;
      this.label24.Text = "Pt2";
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(37, 91);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(23, 13);
      this.label23.TabIndex = 15;
      this.label23.Text = "Pt1";
      // 
      // lblYout3
      // 
      this.lblYout3.AutoSize = true;
      this.lblYout3.Location = new System.Drawing.Point(99, 127);
      this.lblYout3.Name = "lblYout3";
      this.lblYout3.Size = new System.Drawing.Size(28, 13);
      this.lblYout3.TabIndex = 14;
      this.lblYout3.Text = "0.75";
      // 
      // lblYin3
      // 
      this.lblYin3.AutoSize = true;
      this.lblYin3.Location = new System.Drawing.Point(59, 127);
      this.lblYin3.Name = "lblYin3";
      this.lblYin3.Size = new System.Drawing.Size(28, 13);
      this.lblYin3.TabIndex = 13;
      this.lblYin3.Text = "0.75";
      // 
      // lblYout2
      // 
      this.lblYout2.AutoSize = true;
      this.lblYout2.Location = new System.Drawing.Point(99, 109);
      this.lblYout2.Name = "lblYout2";
      this.lblYout2.Size = new System.Drawing.Size(22, 13);
      this.lblYout2.TabIndex = 12;
      this.lblYout2.Text = "0.5";
      // 
      // lblYin2
      // 
      this.lblYin2.AutoSize = true;
      this.lblYin2.Location = new System.Drawing.Point(59, 109);
      this.lblYin2.Name = "lblYin2";
      this.lblYin2.Size = new System.Drawing.Size(22, 13);
      this.lblYin2.TabIndex = 11;
      this.lblYin2.Text = "0.5";
      // 
      // lblYout1
      // 
      this.lblYout1.AutoSize = true;
      this.lblYout1.Location = new System.Drawing.Point(99, 91);
      this.lblYout1.Name = "lblYout1";
      this.lblYout1.Size = new System.Drawing.Size(28, 13);
      this.lblYout1.TabIndex = 10;
      this.lblYout1.Text = "0.25";
      // 
      // lblYin1
      // 
      this.lblYin1.AutoSize = true;
      this.lblYin1.Location = new System.Drawing.Point(59, 91);
      this.lblYin1.Name = "lblYin1";
      this.lblYin1.Size = new System.Drawing.Size(28, 13);
      this.lblYin1.TabIndex = 9;
      this.lblYin1.Text = "0.25";
      // 
      // lblYsat
      // 
      this.lblYsat.AutoSize = true;
      this.lblYsat.Location = new System.Drawing.Point(99, 54);
      this.lblYsat.Name = "lblYsat";
      this.lblYsat.Size = new System.Drawing.Size(34, 13);
      this.lblYsat.TabIndex = 8;
      this.lblYsat.Text = "1.000";
      // 
      // lblYexponent
      // 
      this.lblYexponent.AutoSize = true;
      this.lblYexponent.Location = new System.Drawing.Point(99, 72);
      this.lblYexponent.Name = "lblYexponent";
      this.lblYexponent.Size = new System.Drawing.Size(34, 13);
      this.lblYexponent.TabIndex = 6;
      this.lblYexponent.Text = "1.000";
      // 
      // lblYdeadzone
      // 
      this.lblYdeadzone.AutoSize = true;
      this.lblYdeadzone.Location = new System.Drawing.Point(99, 36);
      this.lblYdeadzone.Name = "lblYdeadzone";
      this.lblYdeadzone.Size = new System.Drawing.Size(34, 13);
      this.lblYdeadzone.TabIndex = 4;
      this.lblYdeadzone.Text = "0.000";
      // 
      // lblYCmd
      // 
      this.lblYCmd.AutoSize = true;
      this.lblYCmd.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblYCmd.Location = new System.Drawing.Point(37, 1);
      this.lblYCmd.Name = "lblYCmd";
      this.lblYCmd.Size = new System.Drawing.Size(47, 12);
      this.lblYCmd.TabIndex = 2;
      this.lblYCmd.Text = "Command";
      // 
      // lblYaw
      // 
      this.lblYaw.AutoSize = true;
      this.lblYaw.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblYaw.Location = new System.Drawing.Point(2, 0);
      this.lblYaw.Name = "lblYaw";
      this.lblYaw.Size = new System.Drawing.Size(29, 13);
      this.lblYaw.TabIndex = 1;
      this.lblYaw.Text = "Yaw";
      // 
      // pnlPitch
      // 
      this.pnlPitch.BackColor = System.Drawing.Color.Salmon;
      this.pnlPitch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pnlPitch.Controls.Add(this.lblPnt);
      this.pnlPitch.Controls.Add(this.cbxPinvert);
      this.pnlPitch.Controls.Add(this.cbxPpts);
      this.pnlPitch.Controls.Add(this.cbxPexpo);
      this.pnlPitch.Controls.Add(this.cbxPsat);
      this.pnlPitch.Controls.Add(this.cbxPdeadzone);
      this.pnlPitch.Controls.Add(this.label26);
      this.pnlPitch.Controls.Add(this.label27);
      this.pnlPitch.Controls.Add(this.label28);
      this.pnlPitch.Controls.Add(this.lblPout3);
      this.pnlPitch.Controls.Add(this.lblPin3);
      this.pnlPitch.Controls.Add(this.lblPout2);
      this.pnlPitch.Controls.Add(this.lblPin2);
      this.pnlPitch.Controls.Add(this.lblPout1);
      this.pnlPitch.Controls.Add(this.lblPin1);
      this.pnlPitch.Controls.Add(this.lblPsat);
      this.pnlPitch.Controls.Add(this.lblPexponent);
      this.pnlPitch.Controls.Add(this.lblPdeadzone);
      this.pnlPitch.Controls.Add(this.lblPCmd);
      this.pnlPitch.Controls.Add(this.lblPitch);
      this.pnlPitch.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlPitch.Location = new System.Drawing.Point(3, 284);
      this.pnlPitch.Name = "pnlPitch";
      this.pnlPitch.Size = new System.Drawing.Size(138, 149);
      this.pnlPitch.TabIndex = 3;
      // 
      // lblPnt
      // 
      this.lblPnt.AutoSize = true;
      this.lblPnt.Location = new System.Drawing.Point(107, 17);
      this.lblPnt.Name = "lblPnt";
      this.lblPnt.Size = new System.Drawing.Size(20, 13);
      this.lblPnt.TabIndex = 34;
      this.lblPnt.Text = "NT";
      this.lblPnt.Visible = false;
      // 
      // cbxPinvert
      // 
      this.cbxPinvert.AutoSize = true;
      this.cbxPinvert.Location = new System.Drawing.Point(6, 17);
      this.cbxPinvert.Name = "cbxPinvert";
      this.cbxPinvert.Size = new System.Drawing.Size(55, 17);
      this.cbxPinvert.TabIndex = 33;
      this.cbxPinvert.Text = "Invert";
      this.cbxPinvert.UseVisualStyleBackColor = true;
      this.cbxPinvert.CheckedChanged += new System.EventHandler(this.cbxPinvert_CheckedChanged);
      // 
      // cbxPpts
      // 
      this.cbxPpts.AutoSize = true;
      this.cbxPpts.Location = new System.Drawing.Point(6, 91);
      this.cbxPpts.Name = "cbxPpts";
      this.cbxPpts.Size = new System.Drawing.Size(15, 14);
      this.cbxPpts.TabIndex = 32;
      this.cbxPpts.UseVisualStyleBackColor = true;
      this.cbxPpts.CheckedChanged += new System.EventHandler(this.cbxPpts_CheckedChanged);
      // 
      // cbxPexpo
      // 
      this.cbxPexpo.AutoSize = true;
      this.cbxPexpo.Location = new System.Drawing.Point(6, 71);
      this.cbxPexpo.Name = "cbxPexpo";
      this.cbxPexpo.Size = new System.Drawing.Size(75, 17);
      this.cbxPexpo.TabIndex = 31;
      this.cbxPexpo.Text = "Exponent";
      this.cbxPexpo.UseVisualStyleBackColor = true;
      this.cbxPexpo.CheckedChanged += new System.EventHandler(this.cbxPexpo_CheckedChanged);
      // 
      // cbxPsat
      // 
      this.cbxPsat.AutoSize = true;
      this.cbxPsat.Location = new System.Drawing.Point(6, 53);
      this.cbxPsat.Name = "cbxPsat";
      this.cbxPsat.Size = new System.Drawing.Size(80, 17);
      this.cbxPsat.TabIndex = 30;
      this.cbxPsat.Text = "Saturation";
      this.cbxPsat.UseVisualStyleBackColor = true;
      this.cbxPsat.CheckedChanged += new System.EventHandler(this.cbxPsense_CheckedChanged);
      // 
      // cbxPdeadzone
      // 
      this.cbxPdeadzone.AutoSize = true;
      this.cbxPdeadzone.Location = new System.Drawing.Point(6, 35);
      this.cbxPdeadzone.Name = "cbxPdeadzone";
      this.cbxPdeadzone.Size = new System.Drawing.Size(78, 17);
      this.cbxPdeadzone.TabIndex = 29;
      this.cbxPdeadzone.Text = "Deadzone";
      this.cbxPdeadzone.UseVisualStyleBackColor = true;
      this.cbxPdeadzone.CheckedChanged += new System.EventHandler(this.cbxPdeadzone_CheckedChanged);
      // 
      // label26
      // 
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(37, 126);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(23, 13);
      this.label26.TabIndex = 28;
      this.label26.Text = "Pt3";
      // 
      // label27
      // 
      this.label27.AutoSize = true;
      this.label27.Location = new System.Drawing.Point(37, 108);
      this.label27.Name = "label27";
      this.label27.Size = new System.Drawing.Size(23, 13);
      this.label27.TabIndex = 27;
      this.label27.Text = "Pt2";
      // 
      // label28
      // 
      this.label28.AutoSize = true;
      this.label28.Location = new System.Drawing.Point(37, 90);
      this.label28.Name = "label28";
      this.label28.Size = new System.Drawing.Size(23, 13);
      this.label28.TabIndex = 26;
      this.label28.Text = "Pt1";
      // 
      // lblPout3
      // 
      this.lblPout3.AutoSize = true;
      this.lblPout3.Location = new System.Drawing.Point(99, 126);
      this.lblPout3.Name = "lblPout3";
      this.lblPout3.Size = new System.Drawing.Size(28, 13);
      this.lblPout3.TabIndex = 25;
      this.lblPout3.Text = "0.75";
      // 
      // lblPin3
      // 
      this.lblPin3.AutoSize = true;
      this.lblPin3.Location = new System.Drawing.Point(59, 126);
      this.lblPin3.Name = "lblPin3";
      this.lblPin3.Size = new System.Drawing.Size(28, 13);
      this.lblPin3.TabIndex = 24;
      this.lblPin3.Text = "0.75";
      // 
      // lblPout2
      // 
      this.lblPout2.AutoSize = true;
      this.lblPout2.Location = new System.Drawing.Point(99, 108);
      this.lblPout2.Name = "lblPout2";
      this.lblPout2.Size = new System.Drawing.Size(22, 13);
      this.lblPout2.TabIndex = 23;
      this.lblPout2.Text = "0.5";
      // 
      // lblPin2
      // 
      this.lblPin2.AutoSize = true;
      this.lblPin2.Location = new System.Drawing.Point(59, 108);
      this.lblPin2.Name = "lblPin2";
      this.lblPin2.Size = new System.Drawing.Size(22, 13);
      this.lblPin2.TabIndex = 22;
      this.lblPin2.Text = "0.5";
      // 
      // lblPout1
      // 
      this.lblPout1.AutoSize = true;
      this.lblPout1.Location = new System.Drawing.Point(99, 90);
      this.lblPout1.Name = "lblPout1";
      this.lblPout1.Size = new System.Drawing.Size(28, 13);
      this.lblPout1.TabIndex = 21;
      this.lblPout1.Text = "0.25";
      // 
      // lblPin1
      // 
      this.lblPin1.AutoSize = true;
      this.lblPin1.Location = new System.Drawing.Point(59, 90);
      this.lblPin1.Name = "lblPin1";
      this.lblPin1.Size = new System.Drawing.Size(28, 13);
      this.lblPin1.TabIndex = 20;
      this.lblPin1.Text = "0.25";
      // 
      // lblPsat
      // 
      this.lblPsat.AutoSize = true;
      this.lblPsat.Location = new System.Drawing.Point(99, 54);
      this.lblPsat.Name = "lblPsat";
      this.lblPsat.Size = new System.Drawing.Size(34, 13);
      this.lblPsat.TabIndex = 10;
      this.lblPsat.Text = "1.000";
      // 
      // lblPexponent
      // 
      this.lblPexponent.AutoSize = true;
      this.lblPexponent.Location = new System.Drawing.Point(99, 72);
      this.lblPexponent.Name = "lblPexponent";
      this.lblPexponent.Size = new System.Drawing.Size(34, 13);
      this.lblPexponent.TabIndex = 8;
      this.lblPexponent.Text = "1.000";
      // 
      // lblPdeadzone
      // 
      this.lblPdeadzone.AutoSize = true;
      this.lblPdeadzone.Location = new System.Drawing.Point(99, 36);
      this.lblPdeadzone.Name = "lblPdeadzone";
      this.lblPdeadzone.Size = new System.Drawing.Size(34, 13);
      this.lblPdeadzone.TabIndex = 5;
      this.lblPdeadzone.Text = "0.000";
      // 
      // lblPCmd
      // 
      this.lblPCmd.AutoSize = true;
      this.lblPCmd.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPCmd.Location = new System.Drawing.Point(37, 1);
      this.lblPCmd.Name = "lblPCmd";
      this.lblPCmd.Size = new System.Drawing.Size(47, 12);
      this.lblPCmd.TabIndex = 3;
      this.lblPCmd.Text = "Command";
      // 
      // lblPitch
      // 
      this.lblPitch.AutoSize = true;
      this.lblPitch.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPitch.Location = new System.Drawing.Point(2, 0);
      this.lblPitch.Name = "lblPitch";
      this.lblPitch.Size = new System.Drawing.Size(33, 13);
      this.lblPitch.TabIndex = 2;
      this.lblPitch.Text = "Pitch";
      // 
      // pnlRoll
      // 
      this.pnlRoll.BackColor = System.Drawing.Color.LightGreen;
      this.pnlRoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pnlRoll.Controls.Add(this.lblRnt);
      this.pnlRoll.Controls.Add(this.cbxRinvert);
      this.pnlRoll.Controls.Add(this.cbxRpts);
      this.pnlRoll.Controls.Add(this.cbxRexpo);
      this.pnlRoll.Controls.Add(this.cbxRsat);
      this.pnlRoll.Controls.Add(this.cbxRdeadzone);
      this.pnlRoll.Controls.Add(this.label35);
      this.pnlRoll.Controls.Add(this.label36);
      this.pnlRoll.Controls.Add(this.label37);
      this.pnlRoll.Controls.Add(this.lblRout3);
      this.pnlRoll.Controls.Add(this.lblRin3);
      this.pnlRoll.Controls.Add(this.lblRout2);
      this.pnlRoll.Controls.Add(this.lblRin2);
      this.pnlRoll.Controls.Add(this.lblRout1);
      this.pnlRoll.Controls.Add(this.lblRin1);
      this.pnlRoll.Controls.Add(this.lblRsat);
      this.pnlRoll.Controls.Add(this.lblRexponent);
      this.pnlRoll.Controls.Add(this.lblRdeadzone);
      this.pnlRoll.Controls.Add(this.lblRCmd);
      this.pnlRoll.Controls.Add(this.lblRoll);
      this.pnlRoll.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlRoll.Location = new System.Drawing.Point(3, 445);
      this.pnlRoll.Name = "pnlRoll";
      this.pnlRoll.Size = new System.Drawing.Size(138, 149);
      this.pnlRoll.TabIndex = 4;
      // 
      // lblRnt
      // 
      this.lblRnt.AutoSize = true;
      this.lblRnt.Location = new System.Drawing.Point(107, 17);
      this.lblRnt.Name = "lblRnt";
      this.lblRnt.Size = new System.Drawing.Size(20, 13);
      this.lblRnt.TabIndex = 38;
      this.lblRnt.Text = "NT";
      this.lblRnt.Visible = false;
      // 
      // cbxRinvert
      // 
      this.cbxRinvert.AutoSize = true;
      this.cbxRinvert.Location = new System.Drawing.Point(6, 17);
      this.cbxRinvert.Name = "cbxRinvert";
      this.cbxRinvert.Size = new System.Drawing.Size(55, 17);
      this.cbxRinvert.TabIndex = 37;
      this.cbxRinvert.Text = "Invert";
      this.cbxRinvert.UseVisualStyleBackColor = true;
      this.cbxRinvert.CheckedChanged += new System.EventHandler(this.cbxRinvert_CheckedChanged);
      // 
      // cbxRpts
      // 
      this.cbxRpts.AutoSize = true;
      this.cbxRpts.Location = new System.Drawing.Point(6, 91);
      this.cbxRpts.Name = "cbxRpts";
      this.cbxRpts.Size = new System.Drawing.Size(15, 14);
      this.cbxRpts.TabIndex = 36;
      this.cbxRpts.UseVisualStyleBackColor = true;
      this.cbxRpts.CheckedChanged += new System.EventHandler(this.cbxRpts_CheckedChanged);
      // 
      // cbxRexpo
      // 
      this.cbxRexpo.AutoSize = true;
      this.cbxRexpo.Location = new System.Drawing.Point(6, 71);
      this.cbxRexpo.Name = "cbxRexpo";
      this.cbxRexpo.Size = new System.Drawing.Size(75, 17);
      this.cbxRexpo.TabIndex = 35;
      this.cbxRexpo.Text = "Exponent";
      this.cbxRexpo.UseVisualStyleBackColor = true;
      this.cbxRexpo.CheckedChanged += new System.EventHandler(this.cbxRexpo_CheckedChanged);
      // 
      // cbxRsat
      // 
      this.cbxRsat.AutoSize = true;
      this.cbxRsat.Location = new System.Drawing.Point(6, 53);
      this.cbxRsat.Name = "cbxRsat";
      this.cbxRsat.Size = new System.Drawing.Size(80, 17);
      this.cbxRsat.TabIndex = 34;
      this.cbxRsat.Text = "Saturation";
      this.cbxRsat.UseVisualStyleBackColor = true;
      this.cbxRsat.CheckedChanged += new System.EventHandler(this.cbxRsense_CheckedChanged);
      // 
      // cbxRdeadzone
      // 
      this.cbxRdeadzone.AutoSize = true;
      this.cbxRdeadzone.Location = new System.Drawing.Point(6, 35);
      this.cbxRdeadzone.Name = "cbxRdeadzone";
      this.cbxRdeadzone.Size = new System.Drawing.Size(78, 17);
      this.cbxRdeadzone.TabIndex = 33;
      this.cbxRdeadzone.Text = "Deadzone";
      this.cbxRdeadzone.UseVisualStyleBackColor = true;
      this.cbxRdeadzone.CheckedChanged += new System.EventHandler(this.cbxRdeadzone_CheckedChanged);
      // 
      // label35
      // 
      this.label35.AutoSize = true;
      this.label35.Location = new System.Drawing.Point(37, 126);
      this.label35.Name = "label35";
      this.label35.Size = new System.Drawing.Size(23, 13);
      this.label35.TabIndex = 28;
      this.label35.Text = "Pt3";
      // 
      // label36
      // 
      this.label36.AutoSize = true;
      this.label36.Location = new System.Drawing.Point(37, 108);
      this.label36.Name = "label36";
      this.label36.Size = new System.Drawing.Size(23, 13);
      this.label36.TabIndex = 27;
      this.label36.Text = "Pt2";
      // 
      // label37
      // 
      this.label37.AutoSize = true;
      this.label37.Location = new System.Drawing.Point(37, 90);
      this.label37.Name = "label37";
      this.label37.Size = new System.Drawing.Size(23, 13);
      this.label37.TabIndex = 26;
      this.label37.Text = "Pt1";
      // 
      // lblRout3
      // 
      this.lblRout3.AutoSize = true;
      this.lblRout3.Location = new System.Drawing.Point(99, 126);
      this.lblRout3.Name = "lblRout3";
      this.lblRout3.Size = new System.Drawing.Size(28, 13);
      this.lblRout3.TabIndex = 25;
      this.lblRout3.Text = "0.75";
      // 
      // lblRin3
      // 
      this.lblRin3.AutoSize = true;
      this.lblRin3.Location = new System.Drawing.Point(59, 126);
      this.lblRin3.Name = "lblRin3";
      this.lblRin3.Size = new System.Drawing.Size(28, 13);
      this.lblRin3.TabIndex = 24;
      this.lblRin3.Text = "0.75";
      // 
      // lblRout2
      // 
      this.lblRout2.AutoSize = true;
      this.lblRout2.Location = new System.Drawing.Point(99, 108);
      this.lblRout2.Name = "lblRout2";
      this.lblRout2.Size = new System.Drawing.Size(22, 13);
      this.lblRout2.TabIndex = 23;
      this.lblRout2.Text = "0.5";
      // 
      // lblRin2
      // 
      this.lblRin2.AutoSize = true;
      this.lblRin2.Location = new System.Drawing.Point(59, 108);
      this.lblRin2.Name = "lblRin2";
      this.lblRin2.Size = new System.Drawing.Size(22, 13);
      this.lblRin2.TabIndex = 22;
      this.lblRin2.Text = "0.5";
      // 
      // lblRout1
      // 
      this.lblRout1.AutoSize = true;
      this.lblRout1.Location = new System.Drawing.Point(99, 90);
      this.lblRout1.Name = "lblRout1";
      this.lblRout1.Size = new System.Drawing.Size(28, 13);
      this.lblRout1.TabIndex = 21;
      this.lblRout1.Text = "0.25";
      // 
      // lblRin1
      // 
      this.lblRin1.AutoSize = true;
      this.lblRin1.Location = new System.Drawing.Point(59, 90);
      this.lblRin1.Name = "lblRin1";
      this.lblRin1.Size = new System.Drawing.Size(28, 13);
      this.lblRin1.TabIndex = 20;
      this.lblRin1.Text = "0.25";
      // 
      // lblRsat
      // 
      this.lblRsat.AutoSize = true;
      this.lblRsat.Location = new System.Drawing.Point(99, 54);
      this.lblRsat.Name = "lblRsat";
      this.lblRsat.Size = new System.Drawing.Size(34, 13);
      this.lblRsat.TabIndex = 10;
      this.lblRsat.Text = "1.000";
      // 
      // lblRexponent
      // 
      this.lblRexponent.AutoSize = true;
      this.lblRexponent.Location = new System.Drawing.Point(99, 72);
      this.lblRexponent.Name = "lblRexponent";
      this.lblRexponent.Size = new System.Drawing.Size(34, 13);
      this.lblRexponent.TabIndex = 8;
      this.lblRexponent.Text = "1.000";
      // 
      // lblRdeadzone
      // 
      this.lblRdeadzone.AutoSize = true;
      this.lblRdeadzone.Location = new System.Drawing.Point(99, 36);
      this.lblRdeadzone.Name = "lblRdeadzone";
      this.lblRdeadzone.Size = new System.Drawing.Size(34, 13);
      this.lblRdeadzone.TabIndex = 6;
      this.lblRdeadzone.Text = "0.000";
      // 
      // lblRCmd
      // 
      this.lblRCmd.AutoSize = true;
      this.lblRCmd.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRCmd.Location = new System.Drawing.Point(37, 1);
      this.lblRCmd.Name = "lblRCmd";
      this.lblRCmd.Size = new System.Drawing.Size(47, 12);
      this.lblRCmd.TabIndex = 4;
      this.lblRCmd.Text = "Command";
      // 
      // lblRoll
      // 
      this.lblRoll.AutoSize = true;
      this.lblRoll.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRoll.Location = new System.Drawing.Point(2, 0);
      this.lblRoll.Name = "lblRoll";
      this.lblRoll.Size = new System.Drawing.Size(27, 13);
      this.lblRoll.TabIndex = 2;
      this.lblRoll.Text = "Roll";
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.79447F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.20553F));
      this.tableLayoutPanel1.Controls.Add(this.panel9, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
      this.tableLayoutPanel1.Controls.Add(this.panel6, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 3);
      this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(153, 619);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 4;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(1028, 294);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // panel9
      // 
      this.panel9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel9.Controls.Add(this.label6);
      this.panel9.Controls.Add(this.lblDamping);
      this.panel9.Controls.Add(this.slDamping);
      this.panel9.Location = new System.Drawing.Point(624, 63);
      this.panel9.Name = "panel9";
      this.panel9.Size = new System.Drawing.Size(401, 54);
      this.panel9.TabIndex = 7;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.Location = new System.Drawing.Point(273, 8);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(54, 13);
      this.label6.TabIndex = 12;
      this.label6.Text = "damping";
      // 
      // lblDamping
      // 
      this.lblDamping.AutoSize = true;
      this.lblDamping.Location = new System.Drawing.Point(273, 29);
      this.lblDamping.Name = "lblDamping";
      this.lblDamping.Size = new System.Drawing.Size(13, 13);
      this.lblDamping.TabIndex = 3;
      this.lblDamping.Text = "1";
      // 
      // slDamping
      // 
      this.slDamping.LargeChange = 1;
      this.slDamping.Location = new System.Drawing.Point(3, 3);
      this.slDamping.Minimum = 1;
      this.slDamping.Name = "slDamping";
      this.slDamping.Size = new System.Drawing.Size(264, 45);
      this.slDamping.TabIndex = 2;
      this.slDamping.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.slDamping.Value = 4;
      this.slDamping.ValueChanged += new System.EventHandler(this.slDamping_ValueChanged);
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.Controls.Add(this.label16);
      this.panel1.Controls.Add(this.lblTurnspeed);
      this.panel1.Controls.Add(this.slTurnSpeed);
      this.panel1.Location = new System.Drawing.Point(624, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(401, 54);
      this.panel1.TabIndex = 4;
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label16.Location = new System.Drawing.Point(273, 8);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(93, 13);
      this.label16.TabIndex = 12;
      this.label16.Text = "sec per 360° turn";
      // 
      // lblTurnspeed
      // 
      this.lblTurnspeed.AutoSize = true;
      this.lblTurnspeed.Location = new System.Drawing.Point(273, 29);
      this.lblTurnspeed.Name = "lblTurnspeed";
      this.lblTurnspeed.Size = new System.Drawing.Size(13, 13);
      this.lblTurnspeed.TabIndex = 3;
      this.lblTurnspeed.Text = "1";
      // 
      // slTurnSpeed
      // 
      this.slTurnSpeed.LargeChange = 1;
      this.slTurnSpeed.Location = new System.Drawing.Point(3, 3);
      this.slTurnSpeed.Maximum = 30;
      this.slTurnSpeed.Minimum = 1;
      this.slTurnSpeed.Name = "slTurnSpeed";
      this.slTurnSpeed.Size = new System.Drawing.Size(264, 45);
      this.slTurnSpeed.TabIndex = 2;
      this.slTurnSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.slTurnSpeed.Value = 6;
      this.slTurnSpeed.ValueChanged += new System.EventHandler(this.slTurnSpeed_ValueChanged);
      // 
      // panel6
      // 
      this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.panel6.BackColor = System.Drawing.Color.WhiteSmoke;
      this.panel6.Controls.Add(this.rbPtDeadzone);
      this.panel6.Controls.Add(this.lblGraphSaturation);
      this.panel6.Controls.Add(this.lblGraphDeadzone);
      this.panel6.Controls.Add(this.lblNodetext);
      this.panel6.Controls.Add(this.label12);
      this.panel6.Controls.Add(this.cbRuse);
      this.panel6.Controls.Add(this.cbPuse);
      this.panel6.Controls.Add(this.cbYuse);
      this.panel6.Controls.Add(this.label11);
      this.panel6.Controls.Add(this.label10);
      this.panel6.Controls.Add(this.lblROutput);
      this.panel6.Controls.Add(this.lblRInput);
      this.panel6.Controls.Add(this.lblPOutput);
      this.panel6.Controls.Add(this.lblPInput);
      this.panel6.Controls.Add(this.lblLiveRoll);
      this.panel6.Controls.Add(this.lblLivePitch);
      this.panel6.Controls.Add(this.btCopyToAllAxis);
      this.panel6.Controls.Add(this.lblLiveYaw);
      this.panel6.Controls.Add(this.label4);
      this.panel6.Controls.Add(this.lblYOutput);
      this.panel6.Controls.Add(this.lblYInput);
      this.panel6.Controls.Add(this.rbPtExponent);
      this.panel6.Controls.Add(this.rbPtSaturation);
      this.panel6.Controls.Add(this.rbPt3);
      this.panel6.Controls.Add(this.rbPt2);
      this.panel6.Controls.Add(this.rbPt1);
      this.panel6.Controls.Add(this.label33);
      this.panel6.Controls.Add(this.label32);
      this.panel6.Controls.Add(this.lblOut3);
      this.panel6.Controls.Add(this.lblIn3);
      this.panel6.Controls.Add(this.lblOut2);
      this.panel6.Controls.Add(this.lblIn2);
      this.panel6.Controls.Add(this.lblOut1);
      this.panel6.Controls.Add(this.lblIn1);
      this.panel6.Controls.Add(this.chart1);
      this.panel6.Controls.Add(this.lblOutSlider);
      this.panel6.Controls.Add(this.lblOutExponent);
      this.panel6.Controls.Add(this.tbSlider);
      this.panel6.Location = new System.Drawing.Point(3, 3);
      this.panel6.Name = "panel6";
      this.tableLayoutPanel1.SetRowSpan(this.panel6, 4);
      this.panel6.Size = new System.Drawing.Size(579, 288);
      this.panel6.TabIndex = 5;
      // 
      // rbPtDeadzone
      // 
      this.rbPtDeadzone.AutoSize = true;
      this.rbPtDeadzone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbPtDeadzone.Location = new System.Drawing.Point(14, 72);
      this.rbPtDeadzone.Name = "rbPtDeadzone";
      this.rbPtDeadzone.Size = new System.Drawing.Size(81, 19);
      this.rbPtDeadzone.TabIndex = 54;
      this.rbPtDeadzone.Text = "Deadzone";
      this.rbPtDeadzone.UseVisualStyleBackColor = true;
      this.rbPtDeadzone.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // lblGraphSaturation
      // 
      this.lblGraphSaturation.AutoSize = true;
      this.lblGraphSaturation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblGraphSaturation.Location = new System.Drawing.Point(531, 266);
      this.lblGraphSaturation.Name = "lblGraphSaturation";
      this.lblGraphSaturation.Size = new System.Drawing.Size(34, 15);
      this.lblGraphSaturation.TabIndex = 53;
      this.lblGraphSaturation.Text = "0.000";
      // 
      // lblGraphDeadzone
      // 
      this.lblGraphDeadzone.AutoSize = true;
      this.lblGraphDeadzone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblGraphDeadzone.Location = new System.Drawing.Point(252, 266);
      this.lblGraphDeadzone.Name = "lblGraphDeadzone";
      this.lblGraphDeadzone.Size = new System.Drawing.Size(34, 15);
      this.lblGraphDeadzone.TabIndex = 52;
      this.lblGraphDeadzone.Text = "0.000";
      // 
      // lblNodetext
      // 
      this.lblNodetext.AutoSize = true;
      this.lblNodetext.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNodetext.Location = new System.Drawing.Point(11, 3);
      this.lblNodetext.Name = "lblNodetext";
      this.lblNodetext.Size = new System.Drawing.Size(16, 13);
      this.lblNodetext.TabIndex = 51;
      this.lblNodetext.Text = "...";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label12.Location = new System.Drawing.Point(213, 233);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(16, 45);
      this.label12.TabIndex = 50;
      this.label12.Text = "O\r\nF\r\nF\r\n";
      // 
      // cbRuse
      // 
      this.cbRuse.AutoSize = true;
      this.cbRuse.Location = new System.Drawing.Point(192, 267);
      this.cbRuse.Name = "cbRuse";
      this.cbRuse.Size = new System.Drawing.Size(15, 14);
      this.cbRuse.TabIndex = 49;
      this.cbRuse.UseVisualStyleBackColor = true;
      // 
      // cbPuse
      // 
      this.cbPuse.AutoSize = true;
      this.cbPuse.Location = new System.Drawing.Point(192, 248);
      this.cbPuse.Name = "cbPuse";
      this.cbPuse.Size = new System.Drawing.Size(15, 14);
      this.cbPuse.TabIndex = 48;
      this.cbPuse.UseVisualStyleBackColor = true;
      // 
      // cbYuse
      // 
      this.cbYuse.AutoSize = true;
      this.cbYuse.Location = new System.Drawing.Point(192, 228);
      this.cbYuse.Name = "cbYuse";
      this.cbYuse.Size = new System.Drawing.Size(15, 14);
      this.cbYuse.TabIndex = 47;
      this.cbYuse.UseVisualStyleBackColor = true;
      // 
      // label11
      // 
      this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label11.Location = new System.Drawing.Point(5, 216);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(246, 3);
      this.label11.TabIndex = 46;
      this.label11.Text = " ";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label10.Location = new System.Drawing.Point(15, 225);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(13, 60);
      this.label10.TabIndex = 45;
      this.label10.Text = "L\r\ni\r\nv\r\ne";
      // 
      // lblROutput
      // 
      this.lblROutput.AutoSize = true;
      this.lblROutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblROutput.Location = new System.Drawing.Point(142, 266);
      this.lblROutput.Name = "lblROutput";
      this.lblROutput.Size = new System.Drawing.Size(34, 15);
      this.lblROutput.TabIndex = 44;
      this.lblROutput.Text = "0.000";
      // 
      // lblRInput
      // 
      this.lblRInput.AutoSize = true;
      this.lblRInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRInput.Location = new System.Drawing.Point(89, 266);
      this.lblRInput.Name = "lblRInput";
      this.lblRInput.Size = new System.Drawing.Size(34, 15);
      this.lblRInput.TabIndex = 43;
      this.lblRInput.Text = "0.000";
      // 
      // lblPOutput
      // 
      this.lblPOutput.AutoSize = true;
      this.lblPOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPOutput.Location = new System.Drawing.Point(142, 247);
      this.lblPOutput.Name = "lblPOutput";
      this.lblPOutput.Size = new System.Drawing.Size(34, 15);
      this.lblPOutput.TabIndex = 42;
      this.lblPOutput.Text = "0.000";
      // 
      // lblPInput
      // 
      this.lblPInput.AutoSize = true;
      this.lblPInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPInput.Location = new System.Drawing.Point(89, 247);
      this.lblPInput.Name = "lblPInput";
      this.lblPInput.Size = new System.Drawing.Size(34, 15);
      this.lblPInput.TabIndex = 41;
      this.lblPInput.Text = "0.000";
      // 
      // lblLiveRoll
      // 
      this.lblLiveRoll.AutoSize = true;
      this.lblLiveRoll.BackColor = System.Drawing.Color.LightGreen;
      this.lblLiveRoll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveRoll.Location = new System.Drawing.Point(32, 266);
      this.lblLiveRoll.Name = "lblLiveRoll";
      this.lblLiveRoll.Size = new System.Drawing.Size(46, 15);
      this.lblLiveRoll.TabIndex = 40;
      this.lblLiveRoll.Text = "R-Axis:";
      // 
      // lblLivePitch
      // 
      this.lblLivePitch.AutoSize = true;
      this.lblLivePitch.BackColor = System.Drawing.Color.Salmon;
      this.lblLivePitch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLivePitch.Location = new System.Drawing.Point(32, 247);
      this.lblLivePitch.Name = "lblLivePitch";
      this.lblLivePitch.Size = new System.Drawing.Size(45, 15);
      this.lblLivePitch.TabIndex = 39;
      this.lblLivePitch.Text = "P-Axis:";
      // 
      // btCopyToAllAxis
      // 
      this.btCopyToAllAxis.Location = new System.Drawing.Point(192, 159);
      this.btCopyToAllAxis.Name = "btCopyToAllAxis";
      this.btCopyToAllAxis.Size = new System.Drawing.Size(57, 44);
      this.btCopyToAllAxis.TabIndex = 38;
      this.btCopyToAllAxis.Text = "Copy to all axis";
      this.btCopyToAllAxis.UseVisualStyleBackColor = true;
      this.btCopyToAllAxis.Click += new System.EventHandler(this.btCopyToAllAxis_Click);
      // 
      // lblLiveYaw
      // 
      this.lblLiveYaw.AutoSize = true;
      this.lblLiveYaw.BackColor = System.Drawing.Color.PowderBlue;
      this.lblLiveYaw.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLiveYaw.Location = new System.Drawing.Point(32, 228);
      this.lblLiveYaw.Name = "lblLiveYaw";
      this.lblLiveYaw.Size = new System.Drawing.Size(45, 15);
      this.lblLiveYaw.TabIndex = 37;
      this.lblLiveYaw.Text = "Y-Axis:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(310, 272);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(193, 13);
      this.label4.TabIndex = 36;
      this.label4.Text = "Select an option then click and drag";
      // 
      // lblYOutput
      // 
      this.lblYOutput.AutoSize = true;
      this.lblYOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblYOutput.Location = new System.Drawing.Point(142, 228);
      this.lblYOutput.Name = "lblYOutput";
      this.lblYOutput.Size = new System.Drawing.Size(34, 15);
      this.lblYOutput.TabIndex = 35;
      this.lblYOutput.Text = "0.000";
      // 
      // lblYInput
      // 
      this.lblYInput.AutoSize = true;
      this.lblYInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblYInput.Location = new System.Drawing.Point(89, 228);
      this.lblYInput.Name = "lblYInput";
      this.lblYInput.Size = new System.Drawing.Size(34, 15);
      this.lblYInput.TabIndex = 34;
      this.lblYInput.Text = "0.000";
      // 
      // rbPtExponent
      // 
      this.rbPtExponent.AutoSize = true;
      this.rbPtExponent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbPtExponent.Location = new System.Drawing.Point(14, 108);
      this.rbPtExponent.Name = "rbPtExponent";
      this.rbPtExponent.Size = new System.Drawing.Size(81, 19);
      this.rbPtExponent.TabIndex = 33;
      this.rbPtExponent.Text = "Exponent:";
      this.rbPtExponent.UseVisualStyleBackColor = true;
      this.rbPtExponent.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // rbPtSaturation
      // 
      this.rbPtSaturation.AutoSize = true;
      this.rbPtSaturation.Checked = true;
      this.rbPtSaturation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbPtSaturation.Location = new System.Drawing.Point(101, 72);
      this.rbPtSaturation.Name = "rbPtSaturation";
      this.rbPtSaturation.Size = new System.Drawing.Size(83, 19);
      this.rbPtSaturation.TabIndex = 32;
      this.rbPtSaturation.TabStop = true;
      this.rbPtSaturation.Text = "Saturation";
      this.rbPtSaturation.UseVisualStyleBackColor = true;
      this.rbPtSaturation.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // rbPt3
      // 
      this.rbPt3.AutoSize = true;
      this.rbPt3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbPt3.Location = new System.Drawing.Point(14, 194);
      this.rbPt3.Name = "rbPt3";
      this.rbPt3.Size = new System.Drawing.Size(67, 19);
      this.rbPt3.TabIndex = 31;
      this.rbPt3.Text = "Point 3:";
      this.rbPt3.UseVisualStyleBackColor = true;
      this.rbPt3.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // rbPt2
      // 
      this.rbPt2.AutoSize = true;
      this.rbPt2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbPt2.Location = new System.Drawing.Point(14, 171);
      this.rbPt2.Name = "rbPt2";
      this.rbPt2.Size = new System.Drawing.Size(67, 19);
      this.rbPt2.TabIndex = 30;
      this.rbPt2.Text = "Point 2:";
      this.rbPt2.UseVisualStyleBackColor = true;
      this.rbPt2.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // rbPt1
      // 
      this.rbPt1.AutoSize = true;
      this.rbPt1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbPt1.Location = new System.Drawing.Point(14, 148);
      this.rbPt1.Name = "rbPt1";
      this.rbPt1.Size = new System.Drawing.Size(67, 19);
      this.rbPt1.TabIndex = 29;
      this.rbPt1.Text = "Point 1:";
      this.rbPt1.UseVisualStyleBackColor = true;
      this.rbPt1.CheckedChanged += new System.EventHandler(this.rbPtAny_CheckedChanged);
      // 
      // label33
      // 
      this.label33.AutoSize = true;
      this.label33.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label33.Location = new System.Drawing.Point(135, 130);
      this.label33.Name = "label33";
      this.label33.Size = new System.Drawing.Size(46, 15);
      this.label33.TabIndex = 28;
      this.label33.Text = "OUT(y)";
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label32.Location = new System.Drawing.Point(89, 130);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(35, 15);
      this.label32.TabIndex = 27;
      this.label32.Text = "IN(x)";
      // 
      // lblOut3
      // 
      this.lblOut3.AutoSize = true;
      this.lblOut3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOut3.Location = new System.Drawing.Point(142, 196);
      this.lblOut3.Name = "lblOut3";
      this.lblOut3.Size = new System.Drawing.Size(28, 15);
      this.lblOut3.TabIndex = 23;
      this.lblOut3.Text = "0.75";
      // 
      // lblIn3
      // 
      this.lblIn3.AutoSize = true;
      this.lblIn3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblIn3.Location = new System.Drawing.Point(89, 196);
      this.lblIn3.Name = "lblIn3";
      this.lblIn3.Size = new System.Drawing.Size(28, 15);
      this.lblIn3.TabIndex = 22;
      this.lblIn3.Text = "0.75";
      // 
      // lblOut2
      // 
      this.lblOut2.AutoSize = true;
      this.lblOut2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOut2.Location = new System.Drawing.Point(142, 173);
      this.lblOut2.Name = "lblOut2";
      this.lblOut2.Size = new System.Drawing.Size(22, 15);
      this.lblOut2.TabIndex = 21;
      this.lblOut2.Text = "0.5";
      // 
      // lblIn2
      // 
      this.lblIn2.AutoSize = true;
      this.lblIn2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblIn2.Location = new System.Drawing.Point(89, 173);
      this.lblIn2.Name = "lblIn2";
      this.lblIn2.Size = new System.Drawing.Size(22, 15);
      this.lblIn2.TabIndex = 20;
      this.lblIn2.Text = "0.5";
      // 
      // lblOut1
      // 
      this.lblOut1.AutoSize = true;
      this.lblOut1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOut1.Location = new System.Drawing.Point(142, 150);
      this.lblOut1.Name = "lblOut1";
      this.lblOut1.Size = new System.Drawing.Size(28, 15);
      this.lblOut1.TabIndex = 19;
      this.lblOut1.Text = "0.25";
      // 
      // lblIn1
      // 
      this.lblIn1.AutoSize = true;
      this.lblIn1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblIn1.Location = new System.Drawing.Point(89, 150);
      this.lblIn1.Name = "lblIn1";
      this.lblIn1.Size = new System.Drawing.Size(28, 15);
      this.lblIn1.TabIndex = 18;
      this.lblIn1.Text = "0.25";
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
      this.chart1.Location = new System.Drawing.Point(258, 7);
      this.chart1.Name = "chart1";
      series1.ChartArea = "ChartArea1";
      series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
      series1.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
      series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
      series1.Name = "Series1";
      this.chart1.Series.Add(series1);
      this.chart1.Size = new System.Drawing.Size(301, 262);
      this.chart1.TabIndex = 16;
      this.chart1.Text = "chart1";
      this.chart1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartPoint_MouseDown);
      this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartPoint_MouseMove);
      this.chart1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chartPoint_MouseUp);
      // 
      // lblOutSlider
      // 
      this.lblOutSlider.AutoSize = true;
      this.lblOutSlider.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOutSlider.Location = new System.Drawing.Point(174, 34);
      this.lblOutSlider.Name = "lblOutSlider";
      this.lblOutSlider.Size = new System.Drawing.Size(34, 15);
      this.lblOutSlider.TabIndex = 13;
      this.lblOutSlider.Text = "0.000";
      // 
      // lblOutExponent
      // 
      this.lblOutExponent.AutoSize = true;
      this.lblOutExponent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOutExponent.Location = new System.Drawing.Point(112, 110);
      this.lblOutExponent.Name = "lblOutExponent";
      this.lblOutExponent.Size = new System.Drawing.Size(34, 15);
      this.lblOutExponent.TabIndex = 9;
      this.lblOutExponent.Text = "0.000";
      // 
      // tbSlider
      // 
      this.tbSlider.Location = new System.Drawing.Point(3, 21);
      this.tbSlider.Maximum = 40;
      this.tbSlider.Name = "tbSlider";
      this.tbSlider.Size = new System.Drawing.Size(165, 45);
      this.tbSlider.TabIndex = 0;
      this.tbSlider.TickFrequency = 5;
      this.tbSlider.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.tbSlider.ValueChanged += new System.EventHandler(this.tbSlider_ValueChanged);
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flowLayoutPanel1.Controls.Add(this.rb300);
      this.flowLayoutPanel1.Controls.Add(this.rbHornet);
      this.flowLayoutPanel1.Controls.Add(this.rbAurora);
      this.flowLayoutPanel1.Location = new System.Drawing.Point(624, 123);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(401, 91);
      this.flowLayoutPanel1.TabIndex = 3;
      // 
      // rb300
      // 
      this.rb300.AutoSize = true;
      this.rb300.Checked = true;
      this.rb300.Image = global::SCJMapper_V2.Properties.Resources._300i1;
      this.rb300.Location = new System.Drawing.Point(3, 3);
      this.rb300.Name = "rb300";
      this.rb300.Size = new System.Drawing.Size(114, 48);
      this.rb300.TabIndex = 2;
      this.rb300.TabStop = true;
      this.rb300.UseVisualStyleBackColor = true;
      this.rb300.CheckedChanged += new System.EventHandler(this.rb300i_CheckedChanged);
      // 
      // rbHornet
      // 
      this.rbHornet.AutoSize = true;
      this.rbHornet.Image = global::SCJMapper_V2.Properties.Resources.hornet;
      this.rbHornet.Location = new System.Drawing.Point(123, 3);
      this.rbHornet.Name = "rbHornet";
      this.rbHornet.Size = new System.Drawing.Size(114, 50);
      this.rbHornet.TabIndex = 0;
      this.rbHornet.UseVisualStyleBackColor = true;
      this.rbHornet.CheckedChanged += new System.EventHandler(this.rbHornet_CheckedChanged);
      // 
      // rbAurora
      // 
      this.rbAurora.AutoSize = true;
      this.rbAurora.Image = global::SCJMapper_V2.Properties.Resources.aurora;
      this.rbAurora.Location = new System.Drawing.Point(243, 3);
      this.rbAurora.Name = "rbAurora";
      this.rbAurora.Size = new System.Drawing.Size(114, 50);
      this.rbAurora.TabIndex = 1;
      this.rbAurora.UseVisualStyleBackColor = true;
      this.rbAurora.CheckedChanged += new System.EventHandler(this.rbAurora_CheckedChanged);
      // 
      // tableLayoutPanel3
      // 
      this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.66029F));
      this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.33971F));
      this.tableLayoutPanel3.Controls.Add(this.btDone, 1, 0);
      this.tableLayoutPanel3.Controls.Add(this.panel8, 0, 0);
      this.tableLayoutPanel3.Location = new System.Drawing.Point(607, 220);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel3.Size = new System.Drawing.Size(418, 71);
      this.tableLayoutPanel3.TabIndex = 6;
      // 
      // btDone
      // 
      this.btDone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btDone.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btDone.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btDone.Location = new System.Drawing.Point(290, 3);
      this.btDone.Name = "btDone";
      this.btDone.Size = new System.Drawing.Size(125, 65);
      this.btDone.TabIndex = 28;
      this.btDone.Text = "Done";
      this.btDone.UseVisualStyleBackColor = true;
      this.btDone.Click += new System.EventHandler(this.btDone_Click);
      // 
      // panel8
      // 
      this.panel8.Controls.Add(this.rbSunset);
      this.panel8.Controls.Add(this.rbOutThere3);
      this.panel8.Controls.Add(this.rbSkybox);
      this.panel8.Controls.Add(this.rbOutThere1);
      this.panel8.Controls.Add(this.rbBigSight);
      this.panel8.Controls.Add(this.rbHighway);
      this.panel8.Controls.Add(this.rbHelipad);
      this.panel8.Controls.Add(this.rbShiodome);
      this.panel8.Controls.Add(this.rbCanyon);
      this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel8.Location = new System.Drawing.Point(3, 3);
      this.panel8.Name = "panel8";
      this.panel8.Size = new System.Drawing.Size(281, 65);
      this.panel8.TabIndex = 4;
      // 
      // rbSunset
      // 
      this.rbSunset.AutoSize = true;
      this.rbSunset.Location = new System.Drawing.Point(205, 35);
      this.rbSunset.Name = "rbSunset";
      this.rbSunset.Size = new System.Drawing.Size(60, 17);
      this.rbSunset.TabIndex = 7;
      this.rbSunset.Text = "Sunset";
      this.rbSunset.UseVisualStyleBackColor = true;
      this.rbSunset.CheckedChanged += new System.EventHandler(this.rbSunset_CheckedChanged);
      // 
      // rbOutThere3
      // 
      this.rbOutThere3.AutoSize = true;
      this.rbOutThere3.Location = new System.Drawing.Point(32, 18);
      this.rbOutThere3.Name = "rbOutThere3";
      this.rbOutThere3.Size = new System.Drawing.Size(84, 17);
      this.rbOutThere3.TabIndex = 6;
      this.rbOutThere3.Text = "Out there 3";
      this.rbOutThere3.UseVisualStyleBackColor = true;
      this.rbOutThere3.CheckedChanged += new System.EventHandler(this.rbOutThere3_CheckedChanged);
      // 
      // rbSkybox
      // 
      this.rbSkybox.AutoSize = true;
      this.rbSkybox.Location = new System.Drawing.Point(32, 35);
      this.rbSkybox.Name = "rbSkybox";
      this.rbSkybox.Size = new System.Drawing.Size(83, 17);
      this.rbSkybox.TabIndex = 5;
      this.rbSkybox.Text = "Skybox.dds";
      this.rbSkybox.UseVisualStyleBackColor = true;
      this.rbSkybox.CheckedChanged += new System.EventHandler(this.rbSkybox_CheckedChanged);
      // 
      // rbOutThere1
      // 
      this.rbOutThere1.AutoSize = true;
      this.rbOutThere1.Checked = true;
      this.rbOutThere1.Location = new System.Drawing.Point(32, 1);
      this.rbOutThere1.Name = "rbOutThere1";
      this.rbOutThere1.Size = new System.Drawing.Size(84, 17);
      this.rbOutThere1.TabIndex = 4;
      this.rbOutThere1.TabStop = true;
      this.rbOutThere1.Text = "Out there 1";
      this.rbOutThere1.UseVisualStyleBackColor = true;
      this.rbOutThere1.CheckedChanged += new System.EventHandler(this.rbOutThere1_CheckedChanged);
      // 
      // rbBigSight
      // 
      this.rbBigSight.AutoSize = true;
      this.rbBigSight.Location = new System.Drawing.Point(205, 18);
      this.rbBigSight.Name = "rbBigSight";
      this.rbBigSight.Size = new System.Drawing.Size(72, 17);
      this.rbBigSight.TabIndex = 3;
      this.rbBigSight.Text = "Big Sight";
      this.rbBigSight.UseVisualStyleBackColor = true;
      this.rbBigSight.CheckedChanged += new System.EventHandler(this.rbBigSight_CheckedChanged);
      // 
      // rbHighway
      // 
      this.rbHighway.AutoSize = true;
      this.rbHighway.Location = new System.Drawing.Point(205, 1);
      this.rbHighway.Name = "rbHighway";
      this.rbHighway.Size = new System.Drawing.Size(70, 17);
      this.rbHighway.TabIndex = 2;
      this.rbHighway.Text = "Highway";
      this.rbHighway.UseVisualStyleBackColor = true;
      this.rbHighway.CheckedChanged += new System.EventHandler(this.rbHighway_CheckedChanged);
      // 
      // rbHelipad
      // 
      this.rbHelipad.AutoSize = true;
      this.rbHelipad.Location = new System.Drawing.Point(122, 35);
      this.rbHelipad.Name = "rbHelipad";
      this.rbHelipad.Size = new System.Drawing.Size(80, 17);
      this.rbHelipad.TabIndex = 1;
      this.rbHelipad.Text = "LA Helipad";
      this.rbHelipad.UseVisualStyleBackColor = true;
      this.rbHelipad.CheckedChanged += new System.EventHandler(this.rbHelipad_CheckedChanged);
      // 
      // rbShiodome
      // 
      this.rbShiodome.AutoSize = true;
      this.rbShiodome.Location = new System.Drawing.Point(122, 18);
      this.rbShiodome.Name = "rbShiodome";
      this.rbShiodome.Size = new System.Drawing.Size(77, 17);
      this.rbShiodome.TabIndex = 1;
      this.rbShiodome.Text = "Shiodome";
      this.rbShiodome.UseVisualStyleBackColor = true;
      this.rbShiodome.CheckedChanged += new System.EventHandler(this.rbShiodome_CheckedChanged);
      // 
      // rbCanyon
      // 
      this.rbCanyon.AutoSize = true;
      this.rbCanyon.Location = new System.Drawing.Point(122, 1);
      this.rbCanyon.Name = "rbCanyon";
      this.rbCanyon.Size = new System.Drawing.Size(64, 17);
      this.rbCanyon.TabIndex = 0;
      this.rbCanyon.Text = "Canyon";
      this.rbCanyon.UseVisualStyleBackColor = true;
      this.rbCanyon.CheckedChanged += new System.EventHandler(this.rbCanyon_CheckedChanged);
      // 
      // flowLayoutPanel2
      // 
      this.flowLayoutPanel2.Controls.Add(this.pnlAxisSelector);
      this.flowLayoutPanel2.Controls.Add(this.panel10);
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 619);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new System.Drawing.Size(144, 294);
      this.flowLayoutPanel2.TabIndex = 3;
      // 
      // pnlAxisSelector
      // 
      this.pnlAxisSelector.BackColor = System.Drawing.Color.Gold;
      this.pnlAxisSelector.Controls.Add(this.rbP);
      this.pnlAxisSelector.Controls.Add(this.rbY);
      this.pnlAxisSelector.Controls.Add(this.rbR);
      this.pnlAxisSelector.Location = new System.Drawing.Point(0, 0);
      this.pnlAxisSelector.Margin = new System.Windows.Forms.Padding(0);
      this.pnlAxisSelector.Name = "pnlAxisSelector";
      this.pnlAxisSelector.Size = new System.Drawing.Size(144, 157);
      this.pnlAxisSelector.TabIndex = 3;
      // 
      // rbP
      // 
      this.rbP.BackColor = System.Drawing.Color.Salmon;
      this.rbP.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbP.Location = new System.Drawing.Point(14, 58);
      this.rbP.Name = "rbP";
      this.rbP.Size = new System.Drawing.Size(111, 42);
      this.rbP.TabIndex = 6;
      this.rbP.Text = "Pitch -->";
      this.rbP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.rbP.UseVisualStyleBackColor = false;
      this.rbP.CheckedChanged += new System.EventHandler(this.rbP_CheckedChanged);
      // 
      // rbY
      // 
      this.rbY.BackColor = System.Drawing.Color.PowderBlue;
      this.rbY.Checked = true;
      this.rbY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbY.Location = new System.Drawing.Point(15, 10);
      this.rbY.Name = "rbY";
      this.rbY.Size = new System.Drawing.Size(111, 42);
      this.rbY.TabIndex = 5;
      this.rbY.TabStop = true;
      this.rbY.Text = "Yaw -->";
      this.rbY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.rbY.UseVisualStyleBackColor = false;
      this.rbY.CheckedChanged += new System.EventHandler(this.rbY_CheckedChanged);
      // 
      // rbR
      // 
      this.rbR.BackColor = System.Drawing.Color.LightGreen;
      this.rbR.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbR.Location = new System.Drawing.Point(15, 106);
      this.rbR.Name = "rbR";
      this.rbR.Size = new System.Drawing.Size(111, 42);
      this.rbR.TabIndex = 7;
      this.rbR.Text = "Roll -->";
      this.rbR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.rbR.UseVisualStyleBackColor = false;
      this.rbR.CheckedChanged += new System.EventHandler(this.rbR_CheckedChanged);
      // 
      // panel10
      // 
      this.panel10.Controls.Add(this.rbTuneStrafe);
      this.panel10.Controls.Add(this.rbTuneYPR);
      this.panel10.Location = new System.Drawing.Point(3, 160);
      this.panel10.Name = "panel10";
      this.panel10.Size = new System.Drawing.Size(138, 128);
      this.panel10.TabIndex = 4;
      // 
      // rbTuneStrafe
      // 
      this.rbTuneStrafe.BackColor = System.Drawing.Color.SandyBrown;
      this.rbTuneStrafe.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbTuneStrafe.Location = new System.Drawing.Point(12, 68);
      this.rbTuneStrafe.Name = "rbTuneStrafe";
      this.rbTuneStrafe.Size = new System.Drawing.Size(111, 42);
      this.rbTuneStrafe.TabIndex = 0;
      this.rbTuneStrafe.Text = "Tune Strafe";
      this.rbTuneStrafe.UseVisualStyleBackColor = false;
      this.rbTuneStrafe.CheckedChanged += new System.EventHandler(this.rbTuneStrafe_CheckedChanged);
      // 
      // rbTuneYPR
      // 
      this.rbTuneYPR.BackColor = System.Drawing.Color.Gold;
      this.rbTuneYPR.Checked = true;
      this.rbTuneYPR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rbTuneYPR.Location = new System.Drawing.Point(12, 20);
      this.rbTuneYPR.Name = "rbTuneYPR";
      this.rbTuneYPR.Size = new System.Drawing.Size(111, 42);
      this.rbTuneYPR.TabIndex = 0;
      this.rbTuneYPR.TabStop = true;
      this.rbTuneYPR.Text = "Tune YPR";
      this.rbTuneYPR.UseVisualStyleBackColor = false;
      this.rbTuneYPR.CheckedChanged += new System.EventHandler(this.rbTuneYPR_CheckedChanged);
      // 
      // FormJSCalCurve
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1184, 916);
      this.Controls.Add(this.tlp);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(1200, 950);
      this.Name = "FormJSCalCurve";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Joystick Tuning";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJSCalCurve_FormClosing);
      this.Load += new System.EventHandler(this.FormJSCalCurve_Load);
      this.tlp.ResumeLayout(false);
      this.tlpData.ResumeLayout(false);
      this.pnlYaw.ResumeLayout(false);
      this.pnlYaw.PerformLayout();
      this.pnlPitch.ResumeLayout(false);
      this.pnlPitch.PerformLayout();
      this.pnlRoll.ResumeLayout(false);
      this.pnlRoll.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.panel9.ResumeLayout(false);
      this.panel9.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.slDamping)).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.slTurnSpeed)).EndInit();
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.tbSlider)).EndInit();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.panel8.ResumeLayout(false);
      this.panel8.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.pnlAxisSelector.ResumeLayout(false);
      this.panel10.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private OpenTK.GLControl glControl1;
    private System.Windows.Forms.TableLayoutPanel tlp;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TrackBar slTurnSpeed;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.RadioButton rbHornet;
    private System.Windows.Forms.RadioButton rbAurora;
    private System.Windows.Forms.RadioButton rb300;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lblTurnspeed;
    private System.Windows.Forms.TableLayoutPanel tlpData;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel pnlYaw;
    private System.Windows.Forms.Label lblYaw;
    private System.Windows.Forms.Panel pnlPitch;
    private System.Windows.Forms.Label lblPitch;
    private System.Windows.Forms.Panel pnlRoll;
    private System.Windows.Forms.Label lblRoll;
    private System.Windows.Forms.Label lblYCmd;
    private System.Windows.Forms.Label lblPCmd;
    private System.Windows.Forms.Label lblRCmd;
    private System.Windows.Forms.Panel panel6;
    private System.Windows.Forms.TrackBar tbSlider;
    private System.Windows.Forms.RadioButton rbR;
    private System.Windows.Forms.RadioButton rbP;
    private System.Windows.Forms.RadioButton rbY;
    private System.Windows.Forms.Label lblYdeadzone;
    private System.Windows.Forms.Label lblPdeadzone;
    private System.Windows.Forms.Label lblRdeadzone;
    private System.Windows.Forms.Label lblOutExponent;
    private System.Windows.Forms.Label lblYexponent;
    private System.Windows.Forms.Label lblPexponent;
    private System.Windows.Forms.Label lblRexponent;
    private System.Windows.Forms.Label lblYsat;
    private System.Windows.Forms.Label lblPsat;
    private System.Windows.Forms.Label lblRsat;
    private System.Windows.Forms.Label lblOutSlider;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.Label lblYout3;
    private System.Windows.Forms.Label lblYin3;
    private System.Windows.Forms.Label lblYout2;
    private System.Windows.Forms.Label lblYin2;
    private System.Windows.Forms.Label lblYout1;
    private System.Windows.Forms.Label lblYin1;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Label lblPout3;
    private System.Windows.Forms.Label lblPin3;
    private System.Windows.Forms.Label lblPout2;
    private System.Windows.Forms.Label lblPin2;
    private System.Windows.Forms.Label lblPout1;
    private System.Windows.Forms.Label lblPin1;
    private System.Windows.Forms.Label label35;
    private System.Windows.Forms.Label label36;
    private System.Windows.Forms.Label label37;
    private System.Windows.Forms.Label lblRout3;
    private System.Windows.Forms.Label lblRin3;
    private System.Windows.Forms.Label lblRout2;
    private System.Windows.Forms.Label lblRin2;
    private System.Windows.Forms.Label lblRout1;
    private System.Windows.Forms.Label lblRin1;
    private System.Windows.Forms.Panel pnlAxisSelector;
    private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    private System.Windows.Forms.RadioButton rbPt3;
    private System.Windows.Forms.RadioButton rbPt2;
    private System.Windows.Forms.RadioButton rbPt1;
    private System.Windows.Forms.Label label33;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label lblOut3;
    private System.Windows.Forms.Label lblIn3;
    private System.Windows.Forms.Label lblOut2;
    private System.Windows.Forms.Label lblIn2;
    private System.Windows.Forms.Label lblOut1;
    private System.Windows.Forms.Label lblIn1;
    private System.Windows.Forms.RadioButton rbPtExponent;
    private System.Windows.Forms.RadioButton rbPtSaturation;
    private System.Windows.Forms.CheckBox cbxYpts;
    private System.Windows.Forms.CheckBox cbxYexpo;
    private System.Windows.Forms.CheckBox cbxYsat;
    private System.Windows.Forms.CheckBox cbxYdeadzone;
    private System.Windows.Forms.CheckBox cbxPpts;
    private System.Windows.Forms.CheckBox cbxPexpo;
    private System.Windows.Forms.CheckBox cbxPsat;
    private System.Windows.Forms.CheckBox cbxPdeadzone;
    private System.Windows.Forms.CheckBox cbxRpts;
    private System.Windows.Forms.CheckBox cbxRexpo;
    private System.Windows.Forms.CheckBox cbxRsat;
    private System.Windows.Forms.CheckBox cbxRdeadzone;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button btDone;
    private System.Windows.Forms.CheckBox cbxYinvert;
    private System.Windows.Forms.CheckBox cbxPinvert;
    private System.Windows.Forms.CheckBox cbxRinvert;
    private System.Windows.Forms.Label lblYOutput;
    private System.Windows.Forms.Label lblYInput;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
    private System.Windows.Forms.Panel panel8;
    private System.Windows.Forms.RadioButton rbHighway;
    private System.Windows.Forms.RadioButton rbShiodome;
    private System.Windows.Forms.RadioButton rbCanyon;
    private System.Windows.Forms.Label lblLiveYaw;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.RadioButton rbBigSight;
    private System.Windows.Forms.Panel panel9;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label lblDamping;
    private System.Windows.Forms.TrackBar slDamping;
    private System.Windows.Forms.Button btCopyToAllAxis;
    private System.Windows.Forms.Label lblROutput;
    private System.Windows.Forms.Label lblRInput;
    private System.Windows.Forms.Label lblPOutput;
    private System.Windows.Forms.Label lblPInput;
    private System.Windows.Forms.Label lblLiveRoll;
    private System.Windows.Forms.Label lblLivePitch;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.CheckBox cbRuse;
    private System.Windows.Forms.CheckBox cbPuse;
    private System.Windows.Forms.CheckBox cbYuse;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.RadioButton rbSkybox;
    private System.Windows.Forms.RadioButton rbOutThere1;
    private System.Windows.Forms.Panel panel10;
    private System.Windows.Forms.RadioButton rbTuneStrafe;
    private System.Windows.Forms.RadioButton rbTuneYPR;
    private System.Windows.Forms.Label lblNodetext;
    private System.Windows.Forms.Label lblYnt;
    private System.Windows.Forms.Label lblPnt;
    private System.Windows.Forms.Label lblRnt;
    private System.Windows.Forms.RadioButton rbOutThere3;
    private System.Windows.Forms.RadioButton rbHelipad;
    private System.Windows.Forms.RadioButton rbSunset;
    private System.Windows.Forms.Label lblGraphSaturation;
    private System.Windows.Forms.Label lblGraphDeadzone;
    private System.Windows.Forms.RadioButton rbPtDeadzone;
  }
}