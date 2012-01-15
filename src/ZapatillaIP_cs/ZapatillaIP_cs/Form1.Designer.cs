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
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPDU = new System.Windows.Forms.TabPage();
            this.groupBoxRelayArray = new System.Windows.Forms.GroupBox();
            this.buttonRelay16 = new System.Windows.Forms.Button();
            this.buttonRelay15 = new System.Windows.Forms.Button();
            this.buttonRelay14 = new System.Windows.Forms.Button();
            this.buttonRelay13 = new System.Windows.Forms.Button();
            this.buttonRelay12 = new System.Windows.Forms.Button();
            this.buttonRelay11 = new System.Windows.Forms.Button();
            this.buttonRelay10 = new System.Windows.Forms.Button();
            this.buttonRelay9 = new System.Windows.Forms.Button();
            this.buttonRelay8 = new System.Windows.Forms.Button();
            this.buttonRelay7 = new System.Windows.Forms.Button();
            this.buttonRelay6 = new System.Windows.Forms.Button();
            this.buttonRelay5 = new System.Windows.Forms.Button();
            this.buttonRelay4 = new System.Windows.Forms.Button();
            this.buttonRelay3 = new System.Windows.Forms.Button();
            this.buttonRelay2 = new System.Windows.Forms.Button();
            this.buttonRelay1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPDU.SuspendLayout();
            this.groupBoxRelayArray.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPDU);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(17, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(608, 216);
            this.tabControl1.TabIndex = 1;
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
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay16);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay15);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay14);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay13);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay12);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay11);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay10);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay9);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay8);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay7);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay6);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay5);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay4);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay3);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay2);
            this.groupBoxRelayArray.Controls.Add(this.buttonRelay1);
            this.groupBoxRelayArray.Location = new System.Drawing.Point(11, 8);
            this.groupBoxRelayArray.Name = "groupBoxRelayArray";
            this.groupBoxRelayArray.Size = new System.Drawing.Size(426, 149);
            this.groupBoxRelayArray.TabIndex = 0;
            this.groupBoxRelayArray.TabStop = false;
            this.groupBoxRelayArray.Text = "16 Relays";
            // 
            // buttonRelay16
            // 
            this.buttonRelay16.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay16Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay16.Location = new System.Drawing.Point(298, 95);
            this.buttonRelay16.Name = "buttonRelay16";
            this.buttonRelay16.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay16.TabIndex = 15;
            this.buttonRelay16.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay16Label;
            this.buttonRelay16.UseVisualStyleBackColor = true;
            this.buttonRelay16.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay15
            // 
            this.buttonRelay15.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay15Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay15.Location = new System.Drawing.Point(205, 95);
            this.buttonRelay15.Name = "buttonRelay15";
            this.buttonRelay15.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay15.TabIndex = 14;
            this.buttonRelay15.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay15Label;
            this.buttonRelay15.UseVisualStyleBackColor = true;
            this.buttonRelay15.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay14
            // 
            this.buttonRelay14.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay14Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay14.Location = new System.Drawing.Point(112, 95);
            this.buttonRelay14.Name = "buttonRelay14";
            this.buttonRelay14.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay14.TabIndex = 13;
            this.buttonRelay14.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay14Label;
            this.buttonRelay14.UseVisualStyleBackColor = true;
            this.buttonRelay14.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay13
            // 
            this.buttonRelay13.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay13Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay13.Location = new System.Drawing.Point(19, 95);
            this.buttonRelay13.Name = "buttonRelay13";
            this.buttonRelay13.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay13.TabIndex = 12;
            this.buttonRelay13.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay13Label;
            this.buttonRelay13.UseVisualStyleBackColor = true;
            this.buttonRelay13.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay12
            // 
            this.buttonRelay12.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay12Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay12.Location = new System.Drawing.Point(298, 70);
            this.buttonRelay12.Name = "buttonRelay12";
            this.buttonRelay12.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay12.TabIndex = 11;
            this.buttonRelay12.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay12Label;
            this.buttonRelay12.UseVisualStyleBackColor = true;
            this.buttonRelay12.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay11
            // 
            this.buttonRelay11.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay11Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay11.Location = new System.Drawing.Point(205, 70);
            this.buttonRelay11.Name = "buttonRelay11";
            this.buttonRelay11.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay11.TabIndex = 10;
            this.buttonRelay11.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay11Label;
            this.buttonRelay11.UseVisualStyleBackColor = true;
            this.buttonRelay11.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay10
            // 
            this.buttonRelay10.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay10Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay10.Location = new System.Drawing.Point(112, 70);
            this.buttonRelay10.Name = "buttonRelay10";
            this.buttonRelay10.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay10.TabIndex = 9;
            this.buttonRelay10.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay10Label;
            this.buttonRelay10.UseVisualStyleBackColor = true;
            this.buttonRelay10.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay9
            // 
            this.buttonRelay9.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay9Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay9.Location = new System.Drawing.Point(19, 70);
            this.buttonRelay9.Name = "buttonRelay9";
            this.buttonRelay9.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay9.TabIndex = 8;
            this.buttonRelay9.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay9Label;
            this.buttonRelay9.UseVisualStyleBackColor = true;
            this.buttonRelay9.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay8
            // 
            this.buttonRelay8.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay8Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay8.Location = new System.Drawing.Point(298, 45);
            this.buttonRelay8.Name = "buttonRelay8";
            this.buttonRelay8.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay8.TabIndex = 7;
            this.buttonRelay8.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay8Label;
            this.buttonRelay8.UseVisualStyleBackColor = true;
            this.buttonRelay8.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay7
            // 
            this.buttonRelay7.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay7Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay7.Location = new System.Drawing.Point(205, 45);
            this.buttonRelay7.Name = "buttonRelay7";
            this.buttonRelay7.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay7.TabIndex = 6;
            this.buttonRelay7.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay7Label;
            this.buttonRelay7.UseVisualStyleBackColor = true;
            this.buttonRelay7.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay6
            // 
            this.buttonRelay6.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay6Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay6.Location = new System.Drawing.Point(112, 45);
            this.buttonRelay6.Name = "buttonRelay6";
            this.buttonRelay6.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay6.TabIndex = 5;
            this.buttonRelay6.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay6Label;
            this.buttonRelay6.UseVisualStyleBackColor = true;
            this.buttonRelay6.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay5
            // 
            this.buttonRelay5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay5Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay5.Location = new System.Drawing.Point(19, 45);
            this.buttonRelay5.Name = "buttonRelay5";
            this.buttonRelay5.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay5.TabIndex = 4;
            this.buttonRelay5.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay5Label;
            this.buttonRelay5.UseVisualStyleBackColor = true;
            this.buttonRelay5.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay4
            // 
            this.buttonRelay4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay4Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay4.Location = new System.Drawing.Point(298, 20);
            this.buttonRelay4.Name = "buttonRelay4";
            this.buttonRelay4.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay4.TabIndex = 3;
            this.buttonRelay4.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay4Label;
            this.buttonRelay4.UseVisualStyleBackColor = true;
            this.buttonRelay4.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay3
            // 
            this.buttonRelay3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay3Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay3.Location = new System.Drawing.Point(205, 20);
            this.buttonRelay3.Name = "buttonRelay3";
            this.buttonRelay3.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay3.TabIndex = 2;
            this.buttonRelay3.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay3Label;
            this.buttonRelay3.UseVisualStyleBackColor = true;
            this.buttonRelay3.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay2
            // 
            this.buttonRelay2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay2Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay2.Location = new System.Drawing.Point(112, 20);
            this.buttonRelay2.Name = "buttonRelay2";
            this.buttonRelay2.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay2.TabIndex = 1;
            this.buttonRelay2.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay2Label;
            this.buttonRelay2.UseVisualStyleBackColor = true;
            this.buttonRelay2.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // buttonRelay1
            // 
            this.buttonRelay1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZapatillaIP_cs.Properties.Settings.Default, "relay1Label", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buttonRelay1.Location = new System.Drawing.Point(19, 20);
            this.buttonRelay1.Name = "buttonRelay1";
            this.buttonRelay1.Size = new System.Drawing.Size(87, 19);
            this.buttonRelay1.TabIndex = 0;
            this.buttonRelay1.Text = global::ZapatillaIP_cs.Properties.Settings.Default.relay1Label;
            this.buttonRelay1.UseVisualStyleBackColor = true;
            this.buttonRelay1.Click += new System.EventHandler(this.buttonRelay_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(600, 190);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 294);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPDU.ResumeLayout(false);
            this.groupBoxRelayArray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPDU;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxRelayArray;
        private System.Windows.Forms.Button buttonRelay1;
        private System.Windows.Forms.Button buttonRelay2;
        private System.Windows.Forms.Button buttonRelay3;
        private System.Windows.Forms.Button buttonRelay5;
        private System.Windows.Forms.Button buttonRelay4;
        private System.Windows.Forms.Button buttonRelay6;
        private System.Windows.Forms.Button buttonRelay7;
        private System.Windows.Forms.Button buttonRelay8;
        private System.Windows.Forms.Button buttonRelay9;
        private System.Windows.Forms.Button buttonRelay10;
        private System.Windows.Forms.Button buttonRelay13;
        private System.Windows.Forms.Button buttonRelay12;
        private System.Windows.Forms.Button buttonRelay11;
        private System.Windows.Forms.Button buttonRelay16;
        private System.Windows.Forms.Button buttonRelay15;
        private System.Windows.Forms.Button buttonRelay14;
    }
}

