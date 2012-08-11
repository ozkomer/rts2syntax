namespace ASCOM.OrbitATC02
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbStepSize = new System.Windows.Forms.TextBox();
            this.cbRefreshFirmwareInfo = new System.Windows.Forms.CheckBox();
            this.nudRefreshStatusPeriod = new System.Windows.Forms.NumericUpDown();
            this.cbRefreshStatus = new System.Windows.Forms.CheckBox();
            this.nudBaudRate = new System.Windows.Forms.NumericUpDown();
            this.tbCommPort = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefreshStatusPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(339, 92);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.CmdOkClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(339, 122);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancelClick);
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.OrbitATC02.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(350, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Comm Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Baud Rate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Step Size [microns]";
            // 
            // tbStepSize
            // 
            this.tbStepSize.Location = new System.Drawing.Point(115, 52);
            this.tbStepSize.Name = "tbStepSize";
            this.tbStepSize.Size = new System.Drawing.Size(62, 20);
            this.tbStepSize.TabIndex = 9;
            // 
            // cbRefreshFirmwareInfo
            // 
            this.cbRefreshFirmwareInfo.AutoSize = true;
            this.cbRefreshFirmwareInfo.Checked = global::ASCOM.OrbitATC02.Properties.Settings.Default.refreshFirmwareInfo;
            this.cbRefreshFirmwareInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRefreshFirmwareInfo.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.OrbitATC02.Properties.Settings.Default, "refreshFirmwareInfo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbRefreshFirmwareInfo.Location = new System.Drawing.Point(15, 104);
            this.cbRefreshFirmwareInfo.Name = "cbRefreshFirmwareInfo";
            this.cbRefreshFirmwareInfo.Size = new System.Drawing.Size(129, 17);
            this.cbRefreshFirmwareInfo.TabIndex = 12;
            this.cbRefreshFirmwareInfo.Text = "Refresh Firmware Info";
            this.cbRefreshFirmwareInfo.UseVisualStyleBackColor = true;
            // 
            // nudRefreshStatusPeriod
            // 
            this.nudRefreshStatusPeriod.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.OrbitATC02.Properties.Settings.Default, "refreshStatusTimer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudRefreshStatusPeriod.Location = new System.Drawing.Point(166, 80);
            this.nudRefreshStatusPeriod.Name = "nudRefreshStatusPeriod";
            this.nudRefreshStatusPeriod.Size = new System.Drawing.Size(54, 20);
            this.nudRefreshStatusPeriod.TabIndex = 11;
            this.nudRefreshStatusPeriod.Value = global::ASCOM.OrbitATC02.Properties.Settings.Default.refreshStatusTimer;
            // 
            // cbRefreshStatus
            // 
            this.cbRefreshStatus.AutoSize = true;
            this.cbRefreshStatus.Checked = global::ASCOM.OrbitATC02.Properties.Settings.Default.refreshStatus;
            this.cbRefreshStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRefreshStatus.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.OrbitATC02.Properties.Settings.Default, "refreshStatus", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbRefreshStatus.Location = new System.Drawing.Point(15, 81);
            this.cbRefreshStatus.Name = "cbRefreshStatus";
            this.cbRefreshStatus.Size = new System.Drawing.Size(145, 17);
            this.cbRefreshStatus.TabIndex = 10;
            this.cbRefreshStatus.Text = "Refresh Status [seconds]";
            this.cbRefreshStatus.UseVisualStyleBackColor = true;
            this.cbRefreshStatus.CheckedChanged += new System.EventHandler(this.cbRefreshStatus_CheckedChanged);
            // 
            // nudBaudRate
            // 
            this.nudBaudRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.OrbitATC02.Properties.Settings.Default, "BaudRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudBaudRate.Increment = new decimal(new int[] {
            2400,
            0,
            0,
            0});
            this.nudBaudRate.Location = new System.Drawing.Point(207, 12);
            this.nudBaudRate.Maximum = new decimal(new int[] {
            115200,
            0,
            0,
            0});
            this.nudBaudRate.Minimum = new decimal(new int[] {
            2400,
            0,
            0,
            0});
            this.nudBaudRate.Name = "nudBaudRate";
            this.nudBaudRate.Size = new System.Drawing.Size(120, 20);
            this.nudBaudRate.TabIndex = 7;
            this.nudBaudRate.Value = global::ASCOM.OrbitATC02.Properties.Settings.Default.BaudRate;
            // 
            // tbCommPort
            // 
            this.tbCommPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.OrbitATC02.Properties.Settings.Default, "CommPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbCommPort.Location = new System.Drawing.Point(76, 12);
            this.tbCommPort.Name = "tbCommPort";
            this.tbCommPort.Size = new System.Drawing.Size(46, 20);
            this.tbCommPort.TabIndex = 5;
            this.tbCommPort.Text = global::ASCOM.OrbitATC02.Properties.Settings.Default.CommPort;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 155);
            this.Controls.Add(this.cbRefreshFirmwareInfo);
            this.Controls.Add(this.nudRefreshStatusPeriod);
            this.Controls.Add(this.cbRefreshStatus);
            this.Controls.Add(this.tbStepSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudBaudRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbCommPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.OrbitATC02.Properties.Settings.Default, "DriverName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = global::ASCOM.OrbitATC02.Properties.Settings.Default.DriverName;
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefreshStatusPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaudRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCommPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudBaudRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStepSize;
        private System.Windows.Forms.CheckBox cbRefreshStatus;
        private System.Windows.Forms.NumericUpDown nudRefreshStatusPeriod;
        private System.Windows.Forms.CheckBox cbRefreshFirmwareInfo;
    }
}