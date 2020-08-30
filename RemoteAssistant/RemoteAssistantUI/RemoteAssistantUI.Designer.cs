namespace RemoteAssistantUI
{
    partial class RemoteAssistantUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteAssistantUI));
            this.notifyIconControl = new System.Windows.Forms.NotifyIcon(this.components);
            this.lbl_connection_status = new System.Windows.Forms.Label();
            this.static_text = new System.Windows.Forms.Label();
            this.lbl_deviceInfo = new System.Windows.Forms.Label();
            this.edb_ConnectionStatus = new System.Windows.Forms.TextBox();
            this.edb_DeviceInfo = new System.Windows.Forms.TextBox();
            this.edb_OneTimePassword = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIconControl
            // 
            resources.ApplyResources(this.notifyIconControl, "notifyIconControl");
            // 
            // lbl_connection_status
            // 
            resources.ApplyResources(this.lbl_connection_status, "lbl_connection_status");
            this.lbl_connection_status.Name = "lbl_connection_status";
            // 
            // static_text
            // 
            resources.ApplyResources(this.static_text, "static_text");
            this.static_text.Name = "static_text";
            // 
            // lbl_deviceInfo
            // 
            resources.ApplyResources(this.lbl_deviceInfo, "lbl_deviceInfo");
            this.lbl_deviceInfo.Name = "lbl_deviceInfo";
            // 
            // edb_ConnectionStatus
            // 
            resources.ApplyResources(this.edb_ConnectionStatus, "edb_ConnectionStatus");
            this.edb_ConnectionStatus.Name = "edb_ConnectionStatus";
            this.edb_ConnectionStatus.ReadOnly = true;
            // 
            // edb_DeviceInfo
            // 
            resources.ApplyResources(this.edb_DeviceInfo, "edb_DeviceInfo");
            this.edb_DeviceInfo.Name = "edb_DeviceInfo";
            this.edb_DeviceInfo.ReadOnly = true;
            // 
            // edb_OneTimePassword
            // 
            resources.ApplyResources(this.edb_OneTimePassword, "edb_OneTimePassword");
            this.edb_OneTimePassword.Name = "edb_OneTimePassword";
            this.edb_OneTimePassword.ReadOnly = true;
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // RemoteAssistantUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.edb_OneTimePassword);
            this.Controls.Add(this.edb_DeviceInfo);
            this.Controls.Add(this.edb_ConnectionStatus);
            this.Controls.Add(this.lbl_deviceInfo);
            this.Controls.Add(this.static_text);
            this.Controls.Add(this.lbl_connection_status);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RemoteAssistantUI";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteAssistantUI_FormClosing);
            this.Load += new System.EventHandler(this.RemoteAssistantUI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIconControl;
        private System.Windows.Forms.Label lbl_connection_status;
        private System.Windows.Forms.Label static_text;
        private System.Windows.Forms.Label lbl_deviceInfo;
        private System.Windows.Forms.TextBox edb_ConnectionStatus;
        private System.Windows.Forms.TextBox edb_DeviceInfo;
        private System.Windows.Forms.TextBox edb_OneTimePassword;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}

