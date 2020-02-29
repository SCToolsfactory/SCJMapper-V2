namespace SCJMapper_V2
{
  partial class AboutBox
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
      this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.labelProductName = new System.Windows.Forms.Label();
      this.labelVersion = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.labelCompanyName = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.tableLayoutPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel
      // 
      this.tableLayoutPanel.ColumnCount = 2;
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
      this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
      this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
      this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
      this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
      this.tableLayoutPanel.Controls.Add(this.label1, 1, 4);
      this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 6;
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.Size = new System.Drawing.Size(754, 265);
      this.tableLayoutPanel.TabIndex = 0;
      this.tableLayoutPanel.UseWaitCursor = true;
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.logoPictureBox.Image = global::SCJMapper_V2.Properties.Resources.Cassini_Logo2_s;
      this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
      this.logoPictureBox.Name = "logoPictureBox";
      this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
      this.logoPictureBox.Size = new System.Drawing.Size(371, 259);
      this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.logoPictureBox.TabIndex = 12;
      this.logoPictureBox.TabStop = false;
      this.logoPictureBox.UseWaitCursor = true;
      // 
      // labelProductName
      // 
      this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelProductName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelProductName.Location = new System.Drawing.Point(383, 0);
      this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new System.Drawing.Size(368, 17);
      this.labelProductName.TabIndex = 19;
      this.labelProductName.Text = "Product Name";
      this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.labelProductName.UseWaitCursor = true;
      // 
      // labelVersion
      // 
      this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelVersion.Location = new System.Drawing.Point(383, 26);
      this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelVersion.Name = "labelVersion";
      this.labelVersion.Size = new System.Drawing.Size(368, 17);
      this.labelVersion.TabIndex = 0;
      this.labelVersion.Text = "Version";
      this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.labelVersion.UseWaitCursor = true;
      // 
      // labelCopyright
      // 
      this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCopyright.Location = new System.Drawing.Point(383, 52);
      this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new System.Drawing.Size(368, 17);
      this.labelCopyright.TabIndex = 21;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.labelCopyright.UseWaitCursor = true;
      // 
      // labelCompanyName
      // 
      this.labelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCompanyName.Location = new System.Drawing.Point(383, 78);
      this.labelCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelCompanyName.Name = "labelCompanyName";
      this.labelCompanyName.Size = new System.Drawing.Size(368, 17);
      this.labelCompanyName.TabIndex = 22;
      this.labelCompanyName.Text = "Company Name";
      this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.labelCompanyName.UseWaitCursor = true;
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(380, 104);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(371, 132);
      this.label1.TabIndex = 23;
      this.label1.Text = "Loading Game Resources ...";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // timer1
      // 
      this.timer1.Enabled = true;
      // 
      // AboutBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(772, 283);
      this.ControlBox = false;
      this.Controls.Add(this.tableLayoutPanel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "About ...";
      this.TopMost = true;
      this.UseWaitCursor = true;
      this.tableLayoutPanel.ResumeLayout(false);
      this.tableLayoutPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    private System.Windows.Forms.PictureBox logoPictureBox;
    private System.Windows.Forms.Label labelProductName;
    private System.Windows.Forms.Label labelVersion;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.Label labelCompanyName;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Timer timer1;
  }
}
