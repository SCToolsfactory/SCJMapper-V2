namespace SCJMapper_V2.Devices.Monitor
{
  partial class UC_Toggle
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Toggle));
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.lblContent = new System.Windows.Forms.Label();
      this.ucLed = new SCJMapper_V2.Devices.Monitor.UC_LED();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.ucLed);
      this.flowLayoutPanel1.Controls.Add(this.lblContent);
      this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flowLayoutPanel1.ForeColor = System.Drawing.Color.Gainsboro;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
      this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(64, 95);
      this.flowLayoutPanel1.TabIndex = 0;
      // 
      // lblContent
      // 
      this.lblContent.Dock = System.Windows.Forms.DockStyle.Top;
      this.lblContent.Location = new System.Drawing.Point(0, 40);
      this.lblContent.Margin = new System.Windows.Forms.Padding(0);
      this.lblContent.Name = "lblContent";
      this.lblContent.Size = new System.Drawing.Size(62, 51);
      this.lblContent.TabIndex = 1;
      this.lblContent.Text = "label1";
      this.lblContent.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // ucLed
      // 
      this.ucLed.BackColor = System.Drawing.Color.Transparent;
      this.ucLed.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucLed.BackgroundImage")));
      this.ucLed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.ucLed.Dock = System.Windows.Forms.DockStyle.Left;
      this.ucLed.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ucLed.Location = new System.Drawing.Point(0, 0);
      this.ucLed.Margin = new System.Windows.Forms.Padding(0);
      this.ucLed.Name = "ucLed";
      this.ucLed.RectShape = false;
      this.ucLed.Size = new System.Drawing.Size(62, 40);
      this.ucLed.Switch = false;
      this.ucLed.TabIndex = 0;
      // 
      // UC_Toggle
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.flowLayoutPanel1);
      this.Name = "UC_Toggle";
      this.Size = new System.Drawing.Size(64, 95);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private UC_LED ucLed;
    private System.Windows.Forms.Label lblContent;
  }
}
