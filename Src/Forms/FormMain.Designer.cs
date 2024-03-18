namespace RobotManager.Forms {
    partial class FormMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.GroupBox ServerManagerSettings;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabServer = new System.Windows.Forms.TabPage();
            this.TabManage = new System.Windows.Forms.TabPage();
            this.TabDebug = new System.Windows.Forms.TabPage();
            ServerManagerSettings = new System.Windows.Forms.GroupBox();
            this.Tabs.SuspendLayout();
            this.TabServer.SuspendLayout();
            this.TabManage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerManagerSettings
            // 
            ServerManagerSettings.Dock = System.Windows.Forms.DockStyle.Top;
            ServerManagerSettings.Location = new System.Drawing.Point(7, 3);
            ServerManagerSettings.Name = "ServerManagerSettings";
            ServerManagerSettings.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            ServerManagerSettings.Size = new System.Drawing.Size(1328, 100);
            ServerManagerSettings.TabIndex = 0;
            ServerManagerSettings.TabStop = false;
            ServerManagerSettings.Text = "Server Config";
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabServer);
            this.Tabs.Controls.Add(this.TabManage);
            this.Tabs.Controls.Add(this.TabDebug);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.Padding = new System.Drawing.Point(10, 5);
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(956, 551);
            this.Tabs.TabIndex = 0;
            // 
            // TabServer
            // 
            this.TabServer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.TabServer.Controls.Add(ServerManagerSettings);
            this.TabServer.Location = new System.Drawing.Point(4, 28);
            this.TabServer.Name = "TabServer";
            this.TabServer.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.TabServer.Size = new System.Drawing.Size(1342, 577);
            this.TabServer.TabIndex = 0;
            this.TabServer.Text = "Server Config";
            // 
            // TabManage
            // 
            this.TabManage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.TabManage.Location = new System.Drawing.Point(4, 28);
            this.TabManage.Name = "TabManage";
            this.TabManage.Padding = new System.Windows.Forms.Padding(3);
            this.TabManage.Size = new System.Drawing.Size(948, 519);
            this.TabManage.TabIndex = 1;
            this.TabManage.Text = "Overview / Manage";
            // 
            // TabDebug
            // 
            this.TabDebug.BackColor = System.Drawing.SystemColors.ControlLight;
            this.TabDebug.Location = new System.Drawing.Point(4, 28);
            this.TabDebug.Name = "TabDebug";
            this.TabDebug.Size = new System.Drawing.Size(1342, 577);
            this.TabDebug.TabIndex = 2;
            this.TabDebug.Text = "Debug";
            // 
            // playfieldOverview1
            // 

            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 551);
            this.Controls.Add(this.Tabs);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(550, 590);
            this.Name = "FormMain";
            this.Text = "Robot Manager";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Tabs.ResumeLayout(false);
            this.TabServer.ResumeLayout(false);
            this.TabManage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabServer;
        private System.Windows.Forms.TabPage TabManage;
        private System.Windows.Forms.TabPage TabDebug;
    }
}

