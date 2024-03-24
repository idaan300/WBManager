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
            this.BindAddress = new System.Windows.Forms.TextBox();
            this.Desc_BindIP = new System.Windows.Forms.Label();
            this.StartPort = new System.Windows.Forms.NumericUpDown();
            this.NumOfInstances = new System.Windows.Forms.NumericUpDown();
            this.Desc_StartPort = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartInstances = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ServerManagerSettings = new System.Windows.Forms.GroupBox();
            ServerManagerSettings.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.TabServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StartPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOfInstances)).BeginInit();
            this.SuspendLayout();
            // 
            // ServerManagerSettings
            // 
            ServerManagerSettings.Controls.Add(this.button1);
            ServerManagerSettings.Controls.Add(this.button2);
            ServerManagerSettings.Controls.Add(this.StartInstances);
            ServerManagerSettings.Controls.Add(this.label2);
            ServerManagerSettings.Controls.Add(this.Desc_StartPort);
            ServerManagerSettings.Controls.Add(this.NumOfInstances);
            ServerManagerSettings.Controls.Add(this.StartPort);
            ServerManagerSettings.Controls.Add(this.Desc_BindIP);
            ServerManagerSettings.Controls.Add(this.BindAddress);
            ServerManagerSettings.Dock = System.Windows.Forms.DockStyle.Top;
            ServerManagerSettings.Location = new System.Drawing.Point(7, 3);
            ServerManagerSettings.Name = "ServerManagerSettings";
            ServerManagerSettings.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            ServerManagerSettings.Size = new System.Drawing.Size(934, 136);
            ServerManagerSettings.TabIndex = 0;
            ServerManagerSettings.TabStop = false;
            ServerManagerSettings.Text = "Server Config";
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabServer);
            this.Tabs.Controls.Add(this.TabManage);
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
            this.TabServer.Size = new System.Drawing.Size(948, 519);
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
            // BindAddress
            // 
            this.BindAddress.Location = new System.Drawing.Point(79, 32);
            this.BindAddress.Name = "BindAddress";
            this.BindAddress.Size = new System.Drawing.Size(153, 23);
            this.BindAddress.TabIndex = 0;
            this.BindAddress.Text = "127.0.0.1";
            // 
            // Desc_BindIP
            // 
            this.Desc_BindIP.AutoSize = true;
            this.Desc_BindIP.Location = new System.Drawing.Point(10, 37);
            this.Desc_BindIP.Name = "Desc_BindIP";
            this.Desc_BindIP.Size = new System.Drawing.Size(63, 15);
            this.Desc_BindIP.TabIndex = 1;
            this.Desc_BindIP.Text = "Bind Addr:";
            // 
            // StartPort
            // 
            this.StartPort.Location = new System.Drawing.Point(79, 64);
            this.StartPort.Maximum = new decimal(new int[] {
            65635,
            0,
            0,
            0});
            this.StartPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StartPort.Name = "StartPort";
            this.StartPort.Size = new System.Drawing.Size(77, 23);
            this.StartPort.TabIndex = 2;
            this.StartPort.Value = new decimal(new int[] {
            9001,
            0,
            0,
            0});
            // 
            // NumOfInstances
            // 
            this.NumOfInstances.Location = new System.Drawing.Point(79, 95);
            this.NumOfInstances.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NumOfInstances.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumOfInstances.Name = "NumOfInstances";
            this.NumOfInstances.Size = new System.Drawing.Size(77, 23);
            this.NumOfInstances.TabIndex = 3;
            this.NumOfInstances.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // Desc_StartPort
            // 
            this.Desc_StartPort.AutoSize = true;
            this.Desc_StartPort.Location = new System.Drawing.Point(10, 67);
            this.Desc_StartPort.Name = "Desc_StartPort";
            this.Desc_StartPort.Size = new System.Drawing.Size(59, 15);
            this.Desc_StartPort.TabIndex = 4;
            this.Desc_StartPort.Text = "Start Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Instances";
            // 
            // StartInstances
            // 
            this.StartInstances.Location = new System.Drawing.Point(260, 23);
            this.StartInstances.Name = "StartInstances";
            this.StartInstances.Size = new System.Drawing.Size(75, 46);
            this.StartInstances.TabIndex = 6;
            this.StartInstances.Text = "Start";
            this.StartInstances.UseVisualStyleBackColor = true;
            this.StartInstances.Click += new System.EventHandler(this.StartInstances_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(260, 75);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 46);
            this.button2.TabIndex = 7;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(366, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 46);
            this.button1.TabIndex = 8;
            this.button1.Text = "Stop Read";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            ServerManagerSettings.ResumeLayout(false);
            ServerManagerSettings.PerformLayout();
            this.Tabs.ResumeLayout(false);
            this.TabServer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StartPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOfInstances)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabServer;
        private System.Windows.Forms.TabPage TabManage;
        private System.Windows.Forms.NumericUpDown StartPort;
        private System.Windows.Forms.Label Desc_BindIP;
        private System.Windows.Forms.TextBox BindAddress;
        private System.Windows.Forms.NumericUpDown NumOfInstances;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Desc_StartPort;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button StartInstances;
        private System.Windows.Forms.Button button1;
    }
}

