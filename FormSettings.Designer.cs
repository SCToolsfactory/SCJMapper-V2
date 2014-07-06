namespace SCJMapper_V2
{
  partial class FormSettings
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
      this.btDone = new System.Windows.Forms.Button();
      this.txSCPath = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.btChooseSCDir = new System.Windows.Forms.Button();
      this.cbxUsePath = new System.Windows.Forms.CheckBox();
      this.txJS1 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.txJS2 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txJS3 = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txJS4 = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.txJS5 = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.txJS6 = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.txJS7 = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.txJS8 = new System.Windows.Forms.TextBox();
      this.fbDlg = new System.Windows.Forms.FolderBrowserDialog();
      this.SuspendLayout();
      // 
      // btDone
      // 
      this.btDone.Location = new System.Drawing.Point(462, 319);
      this.btDone.Name = "btDone";
      this.btDone.Size = new System.Drawing.Size(93, 31);
      this.btDone.TabIndex = 1;
      this.btDone.Text = "Done";
      this.btDone.UseVisualStyleBackColor = true;
      this.btDone.Click += new System.EventHandler(this.btDone_Click);
      // 
      // txSCPath
      // 
      this.txSCPath.Location = new System.Drawing.Point(36, 280);
      this.txSCPath.Name = "txSCPath";
      this.txSCPath.Size = new System.Drawing.Size(479, 22);
      this.txSCPath.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(510, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Ignore Buttons - enter button numbers which should be ignored separated by spaces" +
    " (e.g. 24 25)";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(12, 264);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(330, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Path to the Star Citizen Installation (e.g. C:\\Games\\StarCitizen)";
      // 
      // btChooseSCDir
      // 
      this.btChooseSCDir.Location = new System.Drawing.Point(521, 280);
      this.btChooseSCDir.Name = "btChooseSCDir";
      this.btChooseSCDir.Size = new System.Drawing.Size(33, 22);
      this.btChooseSCDir.TabIndex = 4;
      this.btChooseSCDir.Text = " ... ";
      this.btChooseSCDir.UseVisualStyleBackColor = true;
      this.btChooseSCDir.Click += new System.EventHandler(this.btChooseSCDir_Click);
      // 
      // cbxUsePath
      // 
      this.cbxUsePath.AutoSize = true;
      this.cbxUsePath.Location = new System.Drawing.Point(15, 283);
      this.cbxUsePath.Name = "cbxUsePath";
      this.cbxUsePath.Size = new System.Drawing.Size(15, 14);
      this.cbxUsePath.TabIndex = 5;
      this.cbxUsePath.UseVisualStyleBackColor = true;
      // 
      // txJS1
      // 
      this.txJS1.Location = new System.Drawing.Point(72, 31);
      this.txJS1.Name = "txJS1";
      this.txJS1.Size = new System.Drawing.Size(482, 22);
      this.txJS1.TabIndex = 6;
      this.txJS1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 34);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Joystick 1";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 62);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(55, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Joystick 2";
      // 
      // txJS2
      // 
      this.txJS2.Location = new System.Drawing.Point(72, 59);
      this.txJS2.Name = "txJS2";
      this.txJS2.Size = new System.Drawing.Size(482, 22);
      this.txJS2.TabIndex = 8;
      this.txJS2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 90);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(55, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Joystick 3";
      // 
      // txJS3
      // 
      this.txJS3.Location = new System.Drawing.Point(72, 87);
      this.txJS3.Name = "txJS3";
      this.txJS3.Size = new System.Drawing.Size(482, 22);
      this.txJS3.TabIndex = 10;
      this.txJS3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 118);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(55, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "Joystick 4";
      // 
      // txJS4
      // 
      this.txJS4.Location = new System.Drawing.Point(72, 115);
      this.txJS4.Name = "txJS4";
      this.txJS4.Size = new System.Drawing.Size(482, 22);
      this.txJS4.TabIndex = 12;
      this.txJS4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(12, 146);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(55, 13);
      this.label7.TabIndex = 15;
      this.label7.Text = "Joystick 5";
      // 
      // txJS5
      // 
      this.txJS5.Location = new System.Drawing.Point(72, 143);
      this.txJS5.Name = "txJS5";
      this.txJS5.Size = new System.Drawing.Size(482, 22);
      this.txJS5.TabIndex = 14;
      this.txJS5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(12, 174);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(55, 13);
      this.label8.TabIndex = 17;
      this.label8.Text = "Joystick 6";
      // 
      // txJS6
      // 
      this.txJS6.Location = new System.Drawing.Point(72, 171);
      this.txJS6.Name = "txJS6";
      this.txJS6.Size = new System.Drawing.Size(482, 22);
      this.txJS6.TabIndex = 16;
      this.txJS6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(12, 202);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(55, 13);
      this.label9.TabIndex = 19;
      this.label9.Text = "Joystick 7";
      // 
      // txJS7
      // 
      this.txJS7.Location = new System.Drawing.Point(72, 199);
      this.txJS7.Name = "txJS7";
      this.txJS7.Size = new System.Drawing.Size(482, 22);
      this.txJS7.TabIndex = 18;
      this.txJS7.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(12, 230);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(55, 13);
      this.label10.TabIndex = 21;
      this.label10.Text = "Joystick 8";
      // 
      // txJS8
      // 
      this.txJS8.Location = new System.Drawing.Point(72, 227);
      this.txJS8.Name = "txJS8";
      this.txJS8.Size = new System.Drawing.Size(482, 22);
      this.txJS8.TabIndex = 20;
      this.txJS8.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txJS1_KeyDown);
      this.txJS8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txJS1_KeyPress);
      // 
      // fbDlg
      // 
      this.fbDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
      this.fbDlg.ShowNewFolderButton = false;
      // 
      // FormSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(566, 363);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.txJS8);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.txJS7);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.txJS6);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.txJS5);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.txJS4);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.txJS3);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.txJS2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.txJS1);
      this.Controls.Add(this.cbxUsePath);
      this.Controls.Add(this.btChooseSCDir);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txSCPath);
      this.Controls.Add(this.btDone);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormSettings";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Settings";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
      this.Load += new System.EventHandler(this.FormSettings_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btDone;
    private System.Windows.Forms.TextBox txSCPath;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btChooseSCDir;
    private System.Windows.Forms.CheckBox cbxUsePath;
    private System.Windows.Forms.TextBox txJS1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txJS2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txJS3;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txJS4;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txJS5;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox txJS6;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txJS7;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox txJS8;
    private System.Windows.Forms.FolderBrowserDialog fbDlg;
  }
}