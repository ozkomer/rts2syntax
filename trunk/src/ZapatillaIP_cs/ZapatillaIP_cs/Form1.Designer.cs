namespace ZapatillaIP_cs
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPDU = new System.Windows.Forms.TabPage();
            this.groupBoxRelayArray = new System.Windows.Forms.GroupBox();
            this.buttonSwitchOff = new System.Windows.Forms.Button();
            this.buttonSwitchOn = new System.Windows.Forms.Button();
            this.checkBoxRelay16 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay15 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay14 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay13 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay12 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay11 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay10 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay9 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay5 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay6 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay7 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay8 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay4 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay3 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay2 = new System.Windows.Forms.CheckBox();
            this.checkBoxRelay1 = new System.Windows.Forms.CheckBox();
            this.buttonReadRelay = new System.Windows.Forms.Button();
            this.gbNetwork = new System.Windows.Forms.GroupBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPDU.SuspendLayout();
            this.groupBoxRelayArray.SuspendLayout();
            this.gbNetwork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPDU);
            this.tabControl1.Location = new System.Drawing.Point(17, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(608, 216);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.Visible = false;
            // 
            // tabPDU
            // 
            this.tabPDU.Controls.Add(this.groupBoxRelayArray);
            this.tabPDU.Location = new System.Drawing.Point(4, 22);
            this.tabPDU.Name = "tabPDU";
            this.tabPDU.Padding = new System.Windows.Forms.Padding(3);
            this.tabPDU.Size = new System.Drawing.Size(600, 190);
            this.tabPDU.TabIndex = 0;
            this.tabPDU.Text = "Power Distribution";
            this.tabPDU.UseVisualStyleBackColor = true;
            // 
            // groupBoxRelayArray
            // 
            this.groupBoxRelayArray.Controls.Add(this.buttonSwitchOff);
            this.groupBoxRelayArray.Controls.Add(this.buttonSwitchOn);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay16);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay15);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay14);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay13);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay12);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay11);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay10);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay9);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay5);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay6);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay7);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay8);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay4);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay3);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay2);
            this.groupBoxRelayArray.Controls.Add(this.checkBoxRelay1);
            this.groupBoxRelayArray.Controls.Add(this.buttonReadRelay);
            this.groupBoxRelayArray.Location = new System.Drawing.Point(11, 8);
            this.groupBoxRelayArray.Name = "groupBoxRelayArray";
            this.groupBoxRelayArray.Size = new System.Drawing.Size(499, 149);
            this.groupBoxRelayArray.TabIndex = 0;
            this.groupBoxRelayArray.TabStop = false;
            this.groupBoxRelayArray.Text = "16 Relays";
            // 
            // buttonSwitchOff
            // 
            this.buttonSwitchOff.Location = new System.Drawing.Point(9, 120);
            this.buttonSwitchOff.Name = "buttonSwitchOff";
            this.buttonSwitchOff.Size = new System.Drawing.Size(75, 23);
            this.buttonSwitchOff.TabIndex = 34;
            this.buttonSwitchOff.Text = "Switch Off";
            this.buttonSwitchOff.UseVisualStyleBackColor = true;
            this.buttonSwitchOff.Click += new System.EventHandler(this.buttonSwitchOff_Click);
            // 
            // buttonSwitchOn
            // 
            this.buttonSwitchOn.Location = new System.Drawing.Point(413, 120);
            this.buttonSwitchOn.Name = "buttonSwitchOn";
            this.buttonSwitchOn.Size = new System.Drawing.Size(75, 23);
            this.buttonSwitchOn.TabIndex = 33;
            this.buttonSwitchOn.Text = "Switch On";
            this.buttonSwitchOn.UseVisualStyleBackColor = true;
            this.buttonSwitchOn.Click += new System.EventHandler(this.buttonSwitchOn_Click);
            // 
            // checkBoxRelay16
            // 
            this.checkBoxRelay16.AutoSize = true;
            this.checkBoxRelay16.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay16Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay16.Location = new System.Drawing.Point(402, 93);
            this.checkBoxRelay16.Name = "checkBoxRelay16";
            this.checkBoxRelay16.Size = new System.Drawing.Size(86, 17);
            this.checkBoxRelay16.TabIndex = 32;
            this.checkBoxRelay16.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay16Label;
            this.checkBoxRelay16.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay15
            // 
            this.checkBoxRelay15.AutoSize = true;
            this.checkBoxRelay15.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay15Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay15.Location = new System.Drawing.Point(271, 93);
            this.checkBoxRelay15.Name = "checkBoxRelay15";
            this.checkBoxRelay15.Size = new System.Drawing.Size(86, 17);
            this.checkBoxRelay15.TabIndex = 31;
            this.checkBoxRelay15.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay15Label;
            this.checkBoxRelay15.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay14
            // 
            this.checkBoxRelay14.AutoSize = true;
            this.checkBoxRelay14.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay14Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay14.Location = new System.Drawing.Point(140, 93);
            this.checkBoxRelay14.Name = "checkBoxRelay14";
            this.checkBoxRelay14.Size = new System.Drawing.Size(86, 17);
            this.checkBoxRelay14.TabIndex = 30;
            this.checkBoxRelay14.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay14Label;
            this.checkBoxRelay14.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay13
            // 
            this.checkBoxRelay13.AutoSize = true;
            this.checkBoxRelay13.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay13Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay13.Location = new System.Drawing.Point(9, 93);
            this.checkBoxRelay13.Name = "checkBoxRelay13";
            this.checkBoxRelay13.Size = new System.Drawing.Size(97, 17);
            this.checkBoxRelay13.TabIndex = 29;
            this.checkBoxRelay13.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay13Label;
            this.checkBoxRelay13.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay12
            // 
            this.checkBoxRelay12.AutoSize = true;
            this.checkBoxRelay12.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay12Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay12.Location = new System.Drawing.Point(402, 70);
            this.checkBoxRelay12.Name = "checkBoxRelay12";
            this.checkBoxRelay12.Size = new System.Drawing.Size(62, 17);
            this.checkBoxRelay12.TabIndex = 28;
            this.checkBoxRelay12.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay12Label;
            this.checkBoxRelay12.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay11
            // 
            this.checkBoxRelay11.AutoSize = true;
            this.checkBoxRelay11.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay11Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay11.Location = new System.Drawing.Point(271, 70);
            this.checkBoxRelay11.Name = "checkBoxRelay11";
            this.checkBoxRelay11.Size = new System.Drawing.Size(100, 17);
            this.checkBoxRelay11.TabIndex = 27;
            this.checkBoxRelay11.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay11Label;
            this.checkBoxRelay11.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay10
            // 
            this.checkBoxRelay10.AutoSize = true;
            this.checkBoxRelay10.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay10Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay10.Location = new System.Drawing.Point(140, 70);
            this.checkBoxRelay10.Name = "checkBoxRelay10";
            this.checkBoxRelay10.Size = new System.Drawing.Size(57, 17);
            this.checkBoxRelay10.TabIndex = 26;
            this.checkBoxRelay10.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay10Label;
            this.checkBoxRelay10.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay9
            // 
            this.checkBoxRelay9.AutoSize = true;
            this.checkBoxRelay9.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay9Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay9.Location = new System.Drawing.Point(9, 70);
            this.checkBoxRelay9.Name = "checkBoxRelay9";
            this.checkBoxRelay9.Size = new System.Drawing.Size(48, 17);
            this.checkBoxRelay9.TabIndex = 25;
            this.checkBoxRelay9.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay9Label;
            this.checkBoxRelay9.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay5
            // 
            this.checkBoxRelay5.AutoSize = true;
            this.checkBoxRelay5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay5Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay5.Location = new System.Drawing.Point(9, 47);
            this.checkBoxRelay5.Name = "checkBoxRelay5";
            this.checkBoxRelay5.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay5.TabIndex = 24;
            this.checkBoxRelay5.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay5Label;
            this.checkBoxRelay5.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay6
            // 
            this.checkBoxRelay6.AutoSize = true;
            this.checkBoxRelay6.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay6Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay6.Location = new System.Drawing.Point(140, 47);
            this.checkBoxRelay6.Name = "checkBoxRelay6";
            this.checkBoxRelay6.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay6.TabIndex = 23;
            this.checkBoxRelay6.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay6Label;
            this.checkBoxRelay6.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay7
            // 
            this.checkBoxRelay7.AutoSize = true;
            this.checkBoxRelay7.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay7Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay7.Location = new System.Drawing.Point(271, 47);
            this.checkBoxRelay7.Name = "checkBoxRelay7";
            this.checkBoxRelay7.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay7.TabIndex = 22;
            this.checkBoxRelay7.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay7Label;
            this.checkBoxRelay7.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay8
            // 
            this.checkBoxRelay8.AutoSize = true;
            this.checkBoxRelay8.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay8Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay8.Location = new System.Drawing.Point(402, 47);
            this.checkBoxRelay8.Name = "checkBoxRelay8";
            this.checkBoxRelay8.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay8.TabIndex = 21;
            this.checkBoxRelay8.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay8Label;
            this.checkBoxRelay8.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay4
            // 
            this.checkBoxRelay4.AutoSize = true;
            this.checkBoxRelay4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay4Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay4.Location = new System.Drawing.Point(402, 24);
            this.checkBoxRelay4.Name = "checkBoxRelay4";
            this.checkBoxRelay4.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay4.TabIndex = 20;
            this.checkBoxRelay4.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay4Label;
            this.checkBoxRelay4.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay3
            // 
            this.checkBoxRelay3.AutoSize = true;
            this.checkBoxRelay3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay3Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay3.Location = new System.Drawing.Point(271, 24);
            this.checkBoxRelay3.Name = "checkBoxRelay3";
            this.checkBoxRelay3.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay3.TabIndex = 19;
            this.checkBoxRelay3.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay3Label;
            this.checkBoxRelay3.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay2
            // 
            this.checkBoxRelay2.AutoSize = true;
            this.checkBoxRelay2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay2Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay2.Location = new System.Drawing.Point(140, 24);
            this.checkBoxRelay2.Name = "checkBoxRelay2";
            this.checkBoxRelay2.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay2.TabIndex = 18;
            this.checkBoxRelay2.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay2Label;
            this.checkBoxRelay2.UseVisualStyleBackColor = true;
            // 
            // checkBoxRelay1
            // 
            this.checkBoxRelay1.AutoSize = true;
            this.checkBoxRelay1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay1Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxRelay1.Location = new System.Drawing.Point(9, 24);
            this.checkBoxRelay1.Name = "checkBoxRelay1";
            this.checkBoxRelay1.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRelay1.TabIndex = 17;
            this.checkBoxRelay1.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay1Label;
            this.checkBoxRelay1.UseVisualStyleBackColor = true;
            // 
            // buttonReadRelay
            // 
            this.buttonReadRelay.Location = new System.Drawing.Point(223, 120);
            this.buttonReadRelay.Name = "buttonReadRelay";
            this.buttonReadRelay.Size = new System.Drawing.Size(75, 23);
            this.buttonReadRelay.TabIndex = 16;
            this.buttonReadRelay.Text = "Read";
            this.buttonReadRelay.UseVisualStyleBackColor = true;
            this.buttonReadRelay.Click += new System.EventHandler(this.buttonReadRelay_Click);
            // 
            // gbNetwork
            // 
            this.gbNetwork.Controls.Add(this.buttonConnect);
            this.gbNetwork.Controls.Add(this.numericUpDown1);
            this.gbNetwork.Controls.Add(this.label2);
            this.gbNetwork.Controls.Add(this.tbxHost);
            this.gbNetwork.Controls.Add(this.label1);
            this.gbNetwork.Location = new System.Drawing.Point(26, 8);
            this.gbNetwork.Name = "gbNetwork";
            this.gbNetwork.Size = new System.Drawing.Size(450, 49);
            this.gbNetwork.TabIndex = 2;
            this.gbNetwork.TabStop = false;
            this.gbNetwork.Text = "Network";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(369, 14);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ZapatillaIP_cs.Properties.Settings.Default, "port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.Enabled = false;
            this.numericUpDown1.Location = new System.Drawing.Point(246, 17);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(69, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = global::ZapatillaIP_cs.Properties.Settings.Default.port;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tcp Port";
            // 
            // tbxHost
            // 
            this.tbxHost.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "ipAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbxHost.Location = new System.Drawing.Point(41, 16);
            this.tbxHost.Name = "tbxHost";
            this.tbxHost.ReadOnly = true;
            this.tbxHost.Size = new System.Drawing.Size(137, 20);
            this.tbxHost.TabIndex = 1;
            this.tbxHost.Text = global::ZapatillaIP_cs.Properties.Settings.Default.ipAddress;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 294);
            this.Controls.Add(this.gbNetwork);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Power Distribution Unit";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPDU.ResumeLayout(false);
            this.groupBoxRelayArray.ResumeLayout(false);
            this.groupBoxRelayArray.PerformLayout();
            this.gbNetwork.ResumeLayout(false);
            this.gbNetwork.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPDU;
        private System.Windows.Forms.GroupBox groupBoxRelayArray;
        private System.Windows.Forms.GroupBox gbNetwork;
        private System.Windows.Forms.TextBox tbxHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonReadRelay;
        private System.Windows.Forms.CheckBox checkBoxRelay5;
        private System.Windows.Forms.CheckBox checkBoxRelay6;
        private System.Windows.Forms.CheckBox checkBoxRelay7;
        private System.Windows.Forms.CheckBox checkBoxRelay8;
        private System.Windows.Forms.CheckBox checkBoxRelay4;
        private System.Windows.Forms.CheckBox checkBoxRelay3;
        private System.Windows.Forms.CheckBox checkBoxRelay2;
        private System.Windows.Forms.CheckBox checkBoxRelay1;
        private System.Windows.Forms.CheckBox checkBoxRelay14;
        private System.Windows.Forms.CheckBox checkBoxRelay13;
        private System.Windows.Forms.CheckBox checkBoxRelay12;
        private System.Windows.Forms.CheckBox checkBoxRelay11;
        private System.Windows.Forms.CheckBox checkBoxRelay10;
        private System.Windows.Forms.CheckBox checkBoxRelay9;
        private System.Windows.Forms.CheckBox checkBoxRelay16;
        private System.Windows.Forms.CheckBox checkBoxRelay15;
        private System.Windows.Forms.Button buttonSwitchOff;
        private System.Windows.Forms.Button buttonSwitchOn;
        private System.Windows.Forms.Button buttonConnect;
    }
}

