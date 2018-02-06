namespace SCJMapper_V2.Devices.Monitor
{
  partial class UC_LED
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_LED));
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // IL
      // 
      this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
      this.IL.TransparentColor = System.Drawing.Color.Transparent;
      this.IL.Images.SetKeyName(0, "LED-Round_off.png");
      this.IL.Images.SetKeyName(1, "LED-Round_red.png");
      this.IL.Images.SetKeyName(2, "LED-Round_green.png");
      this.IL.Images.SetKeyName(3, "LED-Round_blue.png");
      this.IL.Images.SetKeyName(4, "LED-Round_amber.png");
      this.IL.Images.SetKeyName(5, "LED-Rect_off.png");
      this.IL.Images.SetKeyName(6, "LED-Rect_red.png");
      this.IL.Images.SetKeyName(7, "LED-Rect_green.png");
      this.IL.Images.SetKeyName(8, "LED-Rect_blue.png");
      this.IL.Images.SetKeyName(9, "LED-Rect_amber.png");
      // 
      // UC_LED
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "UC_LED";
      this.Size = new System.Drawing.Size(128, 128);
      this.Load += new System.EventHandler(this.UC_LEDRound_Load);
      this.Click += new System.EventHandler(this.UC_LED_Click);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList IL;
  }
}
