﻿namespace MeteoChase500DriverTestAplication
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.labelHumidity = new System.Windows.Forms.Label();
            this.labelPressure = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.labelWindSpeed = new System.Windows.Forms.Label();
            this.labelWindDir = new System.Windows.Forms.Label();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.labelDewPoint = new System.Windows.Forms.Label();
            this.listBoxRegistros = new System.Windows.Forms.ListBox();
            this.labelMaxHumidity = new System.Windows.Forms.Label();
            this.labelMinDewPoint = new System.Windows.Forms.Label();
            this.labelMaxWindSpeed = new System.Windows.Forms.Label();
            this.buttonSetup = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.labelAverageWindSpeed = new System.Windows.Forms.Label();
            this.toolTipKnotsHora = new System.Windows.Forms.ToolTip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonShowLog = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHumidity
            // 
            this.labelHumidity.AutoSize = true;
            this.labelHumidity.Location = new System.Drawing.Point(481, 17);
            this.labelHumidity.Name = "labelHumidity";
            this.labelHumidity.Size = new System.Drawing.Size(128, 13);
            this.labelHumidity.TabIndex = 0;
            this.labelHumidity.Text = "Humidity...........................";
            this.labelHumidity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPressure
            // 
            this.labelPressure.AutoSize = true;
            this.labelPressure.Location = new System.Drawing.Point(226, 22);
            this.labelPressure.Name = "labelPressure";
            this.labelPressure.Size = new System.Drawing.Size(48, 13);
            this.labelPressure.TabIndex = 1;
            this.labelPressure.Text = "Pressure";
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Location = new System.Drawing.Point(14, 46);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(67, 13);
            this.labelTemperature.TabIndex = 2;
            this.labelTemperature.Text = "Temperature";
            // 
            // labelWindSpeed
            // 
            this.labelWindSpeed.AutoSize = true;
            this.labelWindSpeed.Location = new System.Drawing.Point(226, 75);
            this.labelWindSpeed.Name = "labelWindSpeed";
            this.labelWindSpeed.Size = new System.Drawing.Size(66, 13);
            this.labelWindSpeed.TabIndex = 3;
            this.labelWindSpeed.Text = "Wind Speed";
            this.labelWindSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipKnotsHora.SetToolTip(this.labelWindSpeed, "Aplies for Speed, Average, and Max");
            // 
            // labelWindDir
            // 
            this.labelWindDir.AutoSize = true;
            this.labelWindDir.Location = new System.Drawing.Point(14, 75);
            this.labelWindDir.Name = "labelWindDir";
            this.labelWindDir.Size = new System.Drawing.Size(77, 13);
            this.labelWindDir.TabIndex = 4;
            this.labelWindDir.Text = "Wind Direction";
            // 
            // labelDateTime
            // 
            this.labelDateTime.AutoSize = true;
            this.labelDateTime.Location = new System.Drawing.Point(14, 17);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(56, 13);
            this.labelDateTime.TabIndex = 5;
            this.labelDateTime.Text = "Date Time";
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 30000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // labelDewPoint
            // 
            this.labelDewPoint.AutoSize = true;
            this.labelDewPoint.Location = new System.Drawing.Point(365, 46);
            this.labelDewPoint.Name = "labelDewPoint";
            this.labelDewPoint.Size = new System.Drawing.Size(218, 13);
            this.labelDewPoint.TabIndex = 6;
            this.labelDewPoint.Text = "Dew Point......................................................";
            this.labelDewPoint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listBoxRegistros
            // 
            this.listBoxRegistros.FormattingEnabled = true;
            this.listBoxRegistros.Location = new System.Drawing.Point(7, 119);
            this.listBoxRegistros.Name = "listBoxRegistros";
            this.listBoxRegistros.ScrollAlwaysVisible = true;
            this.listBoxRegistros.Size = new System.Drawing.Size(841, 433);
            this.listBoxRegistros.TabIndex = 7;
            // 
            // labelMaxHumidity
            // 
            this.labelMaxHumidity.AutoSize = true;
            this.labelMaxHumidity.Location = new System.Drawing.Point(615, 17);
            this.labelMaxHumidity.Name = "labelMaxHumidity";
            this.labelMaxHumidity.Size = new System.Drawing.Size(70, 13);
            this.labelMaxHumidity.TabIndex = 8;
            this.labelMaxHumidity.Text = "Max Humidity";
            this.labelMaxHumidity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMinDewPoint
            // 
            this.labelMinDewPoint.AutoSize = true;
            this.labelMinDewPoint.Location = new System.Drawing.Point(615, 46);
            this.labelMinDewPoint.Name = "labelMinDewPoint";
            this.labelMinDewPoint.Size = new System.Drawing.Size(104, 13);
            this.labelMinDewPoint.TabIndex = 9;
            this.labelMinDewPoint.Text = "Min Delta Dew Point";
            this.labelMinDewPoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMaxWindSpeed
            // 
            this.labelMaxWindSpeed.AutoSize = true;
            this.labelMaxWindSpeed.Location = new System.Drawing.Point(615, 75);
            this.labelMaxWindSpeed.Name = "labelMaxWindSpeed";
            this.labelMaxWindSpeed.Size = new System.Drawing.Size(89, 13);
            this.labelMaxWindSpeed.TabIndex = 10;
            this.labelMaxWindSpeed.Text = "Max Wind Speed";
            this.labelMaxWindSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTipKnotsHora.SetToolTip(this.labelMaxWindSpeed, "Aplies for Speed, Average, and Max");
            // 
            // buttonSetup
            // 
            this.buttonSetup.Location = new System.Drawing.Point(737, 12);
            this.buttonSetup.Name = "buttonSetup";
            this.buttonSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonSetup.TabIndex = 11;
            this.buttonSetup.Text = "Setup";
            this.buttonSetup.UseVisualStyleBackColor = true;
            this.buttonSetup.Click += new System.EventHandler(this.buttonSetup_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(368, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // labelAverageWindSpeed
            // 
            this.labelAverageWindSpeed.AutoSize = true;
            this.labelAverageWindSpeed.Location = new System.Drawing.Point(374, 75);
            this.labelAverageWindSpeed.Name = "labelAverageWindSpeed";
            this.labelAverageWindSpeed.Size = new System.Drawing.Size(109, 13);
            this.labelAverageWindSpeed.TabIndex = 14;
            this.labelAverageWindSpeed.Text = "Average Wind Speed";
            this.toolTipKnotsHora.SetToolTip(this.labelAverageWindSpeed, "Aplies for Speed, Average, and Max");
            // 
            // toolTipKnotsHora
            // 
            this.toolTipKnotsHora.ToolTipTitle = "All wind Speeds are in [Knots]";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Chase500 Meteorology Analisis.";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Image = global::MeteoChase500DriverTestAplication.Properties.Resources.Play_1_Normal_icon;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::MeteoChase500DriverTestAplication.Properties.Resources.Stop_Normal_Red_icon;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // buttonShowLog
            // 
            this.buttonShowLog.Location = new System.Drawing.Point(737, 70);
            this.buttonShowLog.Name = "buttonShowLog";
            this.buttonShowLog.Size = new System.Drawing.Size(75, 23);
            this.buttonShowLog.TabIndex = 15;
            this.buttonShowLog.Text = "Open Log";
            this.buttonShowLog.UseVisualStyleBackColor = true;
            this.buttonShowLog.Click += new System.EventHandler(this.buttonShowLog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 565);
            this.Controls.Add(this.buttonShowLog);
            this.Controls.Add(this.labelAverageWindSpeed);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonSetup);
            this.Controls.Add(this.labelMaxWindSpeed);
            this.Controls.Add(this.labelMinDewPoint);
            this.Controls.Add(this.labelMaxHumidity);
            this.Controls.Add(this.listBoxRegistros);
            this.Controls.Add(this.labelDewPoint);
            this.Controls.Add(this.labelDateTime);
            this.Controls.Add(this.labelWindDir);
            this.Controls.Add(this.labelWindSpeed);
            this.Controls.Add(this.labelTemperature);
            this.Controls.Add(this.labelPressure);
            this.Controls.Add(this.labelHumidity);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "CTIO Environmental";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHumidity;
        private System.Windows.Forms.Label labelPressure;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label labelWindSpeed;
        private System.Windows.Forms.Label labelWindDir;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Label labelDewPoint;
        private System.Windows.Forms.ListBox listBoxRegistros;
        private System.Windows.Forms.Label labelMaxHumidity;
        private System.Windows.Forms.Label labelMinDewPoint;
        private System.Windows.Forms.Label labelMaxWindSpeed;
        private System.Windows.Forms.Button buttonSetup;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelAverageWindSpeed;
        private System.Windows.Forms.ToolTip toolTipKnotsHora;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button buttonShowLog;
    }
}

