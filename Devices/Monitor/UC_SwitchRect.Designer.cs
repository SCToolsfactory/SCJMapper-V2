namespace SCJMapper_V2.Devices.Monitor
{
  partial class UC_SwitchRect
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_SwitchRect));
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // IL
      // 
      this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
      this.IL.TransparentColor = System.Drawing.Color.Transparent;
      this.IL.Images.SetKeyName(0, "OFF");
      this.IL.Images.SetKeyName(1, "ON");
      // 
      // UC_SwitchRect
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Transparent;
      this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.CausesValidation = false;
      this.DoubleBuffered = true;
      this.Margin = new System.Windows.Forms.Padding(0);
      this.Name = "UC_SwitchRect";
      this.Size = new System.Drawing.Size(128, 128);
      this.Load += new System.EventHandler(this.UC_SwitchRect_Load);
      this.Click += new System.EventHandler(this.UC_SwitchRect_Click);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ImageList IL;
  }
}
