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
            this.label3 = new System.Windows.Forms.Label();
            this.tbStepSize = new System.Windows.Forms.TextBox();
            this.cbSecondaryPositionStartUp = new System.Windows.Forms.CheckBox();
            this.nudLastSecondary = new System.Windows.Forms.NumericUpDown();
            this.lblFocusServer = new System.Windows.Forms.Label();
            this.tbFocusServer = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.nufFocusPort = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLastSecondary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nufFocusPort)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(339, 86);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Step Size [microns]";
            // 
            // tbStepSize
            // 
            this.tbStepSize.Location = new System.Drawing.Point(115, 6);
            this.tbStepSize.Name = "tbStepSize";
            this.tbStepSize.Size = new System.Drawing.Size(62, 20);
            this.tbStepSize.TabIndex = 9;
            // 
            // cbSecondaryPositionStartUp
            // 
            this.cbSecondaryPositionStartUp.AutoSize = true;
            this.cbSecondaryPositionStartUp.Checked = global::ASCOM.OrbitATC02.Properties.Settings.Default.StartUpSecondary;
            this.cbSecondaryPositionStartUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSecondaryPositionStartUp.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.OrbitATC02.Properties.Settings.Default, "lastSecondaryStartUp", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbSecondaryPositionStartUp.Location = new System.Drawing.Point(15, 51);
            this.cbSecondaryPositionStartUp.Name = "cbSecondaryPositionStartUp";
            this.cbSecondaryPositionStartUp.Size = new System.Drawing.Size(211, 17);
            this.cbSecondaryPositionStartUp.TabIndex = 13;
            this.cbSecondaryPositionStartUp.Text = "Use Last Secondary Position at Startup";
            this.cbSecondaryPositionStartUp.UseVisualStyleBackColor = true;
            // 
            // nudLastSecondary
            // 
            this.nudLastSecondary.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.OrbitATC02.Properties.Settings.Default, "lastSecondaryPosition", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudLastSecondary.Location = new System.Drawing.Point(232, 51);
            this.nudLastSecondary.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudLastSecondary.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudLastSecondary.Name = "nudLastSecondary";
            this.nudLastSecondary.Size = new System.Drawing.Size(81, 20);
            this.nudLastSecondary.TabIndex = 14;
            this.nudLastSecondary.Value = global::ASCOM.OrbitATC02.Properties.Settings.Default.StartUpSecondaryPosition;
            // 
            // lblFocusServer
            // 
            this.lblFocusServer.AutoSize = true;
            this.lblFocusServer.Location = new System.Drawing.Point(12, 92);
            this.lblFocusServer.Name = "lblFocusServer";
            this.lblFocusServer.Size = new System.Drawing.Size(70, 13);
            this.lblFocusServer.TabIndex = 15;
            this.lblFocusServer.Text = "Focus Server";
            // 
            // tbFocusServer
            // 
            this.tbFocusServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.OrbitATC02.Properties.Settings.Default, "FocusServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbFocusServer.Location = new System.Drawing.Point(88, 89);
            this.tbFocusServer.Name = "tbFocusServer";
            this.tbFocusServer.Size = new System.Drawing.Size(97, 20);
            this.tbFocusServer.TabIndex = 16;
            this.tbFocusServer.Text = global::ASCOM.OrbitATC02.Properties.Settings.Default.FocusServer;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(191, 92);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 17;
            this.lblPort.Text = "Port";
            // 
            // nufFocusPort
            // 
            this.nufFocusPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.OrbitATC02.Properties.Settings.Default, "FocusPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nufFocusPort.Location = new System.Drawing.Point(223, 90);
            this.nufFocusPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nufFocusPort.Name = "nufFocusPort";
            this.nufFocusPort.Size = new System.Drawing.Size(90, 20);
            this.nufFocusPort.TabIndex = 18;
            this.nufFocusPort.Value = global::ASCOM.OrbitATC02.Properties.Settings.Default.FocusPort;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 165);
            this.Controls.Add(this.nufFocusPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.tbFocusServer);
            this.Controls.Add(this.lblFocusServer);
            this.Controls.Add(this.nudLastSecondary);
            this.Controls.Add(this.cbSecondaryPositionStartUp);
            this.Controls.Add(this.tbStepSize);
            this.Controls.Add(this.label3);
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
            ((System.ComponentModel.ISupportInitialize)(this.nudLastSecondary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nufFocusPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStepSize;
        private System.Windows.Forms.CheckBox cbSecondaryPositionStartUp;
        private System.Windows.Forms.NumericUpDown nudLastSecondary;
        private System.Windows.Forms.Label lblFocusServer;
        private System.Windows.Forms.TextBox tbFocusServer;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown nufFocusPort;
    }
}