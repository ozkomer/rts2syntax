namespace Montura
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
            this.serialPortMontura = new System.IO.Ports.SerialPort(this.components);
            this.timerReadSerial = new System.Windows.Forms.Timer(this.components);
            this.buttonContinue = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxRA = new System.Windows.Forms.GroupBox();
            this.radioButtonRA_West = new System.Windows.Forms.RadioButton();
            this.radioButtonRA_East = new System.Windows.Forms.RadioButton();
            this.radioButtonRA_Home = new System.Windows.Forms.RadioButton();
            this.groupBoxDEC = new System.Windows.Forms.GroupBox();
            this.radioButtonDecHome = new System.Windows.Forms.RadioButton();
            this.groupBoxTelescope = new System.Windows.Forms.GroupBox();
            this.checkBoxInfrared = new System.Windows.Forms.CheckBox();
            this.labelTelescope = new System.Windows.Forms.Label();
            this.checkBoxInfrarojos = new System.Windows.Forms.CheckBox();
            this.timerTelescopio = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.groupBoxRA.SuspendLayout();
            this.groupBoxDEC.SuspendLayout();
            this.groupBoxTelescope.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPortMontura
            // 
            this.serialPortMontura.PortName = "COM7";
            // 
            // timerReadSerial
            // 
            this.timerReadSerial.Interval = 200;
            this.timerReadSerial.Tick += new System.EventHandler(this.timerReadSerial_Tick);
            // 
            // buttonContinue
            // 
            this.buttonContinue.Enabled = false;
            this.buttonContinue.Location = new System.Drawing.Point(335, 212);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(75, 23);
            this.buttonContinue.TabIndex = 8;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Airbag, Slews, Infrareds";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(104, 48);
            this.contextMenuStrip1.Text = "Salir";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Image = global::Montura.Properties.Resources.Play_1_Normal_icon;
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.abrirToolStripMenuItem.Text = "Show";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Image = global::Montura.Properties.Resources.Stop_Normal_Red_icon;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.salirToolStripMenuItem.Text = "Exit";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // groupBoxRA
            // 
            this.groupBoxRA.Controls.Add(this.radioButtonRA_West);
            this.groupBoxRA.Controls.Add(this.radioButtonRA_East);
            this.groupBoxRA.Controls.Add(this.radioButtonRA_Home);
            this.groupBoxRA.Location = new System.Drawing.Point(12, 105);
            this.groupBoxRA.Name = "groupBoxRA";
            this.groupBoxRA.Size = new System.Drawing.Size(398, 66);
            this.groupBoxRA.TabIndex = 9;
            this.groupBoxRA.TabStop = false;
            this.groupBoxRA.Text = "RA";
            // 
            // radioButtonRA_West
            // 
            this.radioButtonRA_West.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButtonRA_West.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonRA_West.Location = new System.Drawing.Point(6, 19);
            this.radioButtonRA_West.Name = "radioButtonRA_West";
            this.radioButtonRA_West.Size = new System.Drawing.Size(82, 41);
            this.radioButtonRA_West.TabIndex = 2;
            this.radioButtonRA_West.TabStop = true;
            this.radioButtonRA_West.Text = "West";
            this.radioButtonRA_West.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonRA_West.UseVisualStyleBackColor = false;
            // 
            // radioButtonRA_East
            // 
            this.radioButtonRA_East.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButtonRA_East.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonRA_East.Location = new System.Drawing.Point(310, 19);
            this.radioButtonRA_East.Name = "radioButtonRA_East";
            this.radioButtonRA_East.Size = new System.Drawing.Size(82, 41);
            this.radioButtonRA_East.TabIndex = 1;
            this.radioButtonRA_East.TabStop = true;
            this.radioButtonRA_East.Text = "East";
            this.radioButtonRA_East.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonRA_East.UseVisualStyleBackColor = false;
            // 
            // radioButtonRA_Home
            // 
            this.radioButtonRA_Home.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButtonRA_Home.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonRA_Home.Location = new System.Drawing.Point(164, 19);
            this.radioButtonRA_Home.Name = "radioButtonRA_Home";
            this.radioButtonRA_Home.Size = new System.Drawing.Size(82, 41);
            this.radioButtonRA_Home.TabIndex = 0;
            this.radioButtonRA_Home.TabStop = true;
            this.radioButtonRA_Home.Text = "Home";
            this.radioButtonRA_Home.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonRA_Home.UseVisualStyleBackColor = false;
            // 
            // groupBoxDEC
            // 
            this.groupBoxDEC.Controls.Add(this.radioButtonDecHome);
            this.groupBoxDEC.Location = new System.Drawing.Point(12, 177);
            this.groupBoxDEC.Name = "groupBoxDEC";
            this.groupBoxDEC.Size = new System.Drawing.Size(246, 61);
            this.groupBoxDEC.TabIndex = 10;
            this.groupBoxDEC.TabStop = false;
            this.groupBoxDEC.Text = "DEC";
            // 
            // radioButtonDecHome
            // 
            this.radioButtonDecHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButtonDecHome.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButtonDecHome.Location = new System.Drawing.Point(82, 10);
            this.radioButtonDecHome.Name = "radioButtonDecHome";
            this.radioButtonDecHome.Size = new System.Drawing.Size(82, 41);
            this.radioButtonDecHome.TabIndex = 1;
            this.radioButtonDecHome.TabStop = true;
            this.radioButtonDecHome.Text = "Home";
            this.radioButtonDecHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButtonDecHome.UseVisualStyleBackColor = false;
            // 
            // groupBoxTelescope
            // 
            this.groupBoxTelescope.Controls.Add(this.checkBoxInfrared);
            this.groupBoxTelescope.Controls.Add(this.labelTelescope);
            this.groupBoxTelescope.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTelescope.Name = "groupBoxTelescope";
            this.groupBoxTelescope.Size = new System.Drawing.Size(397, 87);
            this.groupBoxTelescope.TabIndex = 11;
            this.groupBoxTelescope.TabStop = false;
            this.groupBoxTelescope.Text = "Ascom Telescope";
            // 
            // checkBoxInfrared
            // 
            this.checkBoxInfrared.AutoSize = true;
            this.checkBoxInfrared.Checked = true;
            this.checkBoxInfrared.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInfrared.Location = new System.Drawing.Point(8, 64);
            this.checkBoxInfrared.Name = "checkBoxInfrared";
            this.checkBoxInfrared.Size = new System.Drawing.Size(98, 17);
            this.checkBoxInfrared.TabIndex = 1;
            this.checkBoxInfrared.Text = "Infrared Control";
            this.checkBoxInfrared.UseVisualStyleBackColor = true;
            // 
            // labelTelescope
            // 
            this.labelTelescope.AutoSize = true;
            this.labelTelescope.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTelescope.Location = new System.Drawing.Point(32, 32);
            this.labelTelescope.Name = "labelTelescope";
            this.labelTelescope.Size = new System.Drawing.Size(144, 19);
            this.labelTelescope.TabIndex = 0;
            this.labelTelescope.Text = "Ascom Telescope";
            // 
            // checkBoxInfrarojos
            // 
            this.checkBoxInfrarojos.AutoSize = true;
            this.checkBoxInfrarojos.Location = new System.Drawing.Point(329, 187);
            this.checkBoxInfrarojos.Name = "checkBoxInfrarojos";
            this.checkBoxInfrarojos.Size = new System.Drawing.Size(67, 17);
            this.checkBoxInfrarojos.TabIndex = 12;
            this.checkBoxInfrarojos.Text = "Infrareds";
            this.checkBoxInfrarojos.UseVisualStyleBackColor = true;
            this.checkBoxInfrarojos.CheckedChanged += new System.EventHandler(this.checkBoxInfrarojos_CheckedChanged);
            // 
            // timerTelescopio
            // 
            this.timerTelescopio.Enabled = true;
            this.timerTelescopio.Interval = 500;
            this.timerTelescopio.Tick += new System.EventHandler(this.timerTelescopio_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 240);
            this.Controls.Add(this.checkBoxInfrarojos);
            this.Controls.Add(this.groupBoxTelescope);
            this.Controls.Add(this.groupBoxRA);
            this.Controls.Add(this.buttonContinue);
            this.Controls.Add(this.groupBoxDEC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Airbag, Slews, Infrareds";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBoxRA.ResumeLayout(false);
            this.groupBoxDEC.ResumeLayout(false);
            this.groupBoxTelescope.ResumeLayout(false);
            this.groupBoxTelescope.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPortMontura;
        private System.Windows.Forms.Timer timerReadSerial;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxRA;
        private System.Windows.Forms.GroupBox groupBoxDEC;
        private System.Windows.Forms.RadioButton radioButtonRA_Home;
        private System.Windows.Forms.RadioButton radioButtonRA_West;
        private System.Windows.Forms.RadioButton radioButtonRA_East;
        private System.Windows.Forms.RadioButton radioButtonDecHome;
        private System.Windows.Forms.GroupBox groupBoxTelescope;
        private System.Windows.Forms.Label labelTelescope;
        private System.Windows.Forms.CheckBox checkBoxInfrared;
        private System.Windows.Forms.CheckBox checkBoxInfrarojos;
        private System.Windows.Forms.Timer timerTelescopio;
    }
}

