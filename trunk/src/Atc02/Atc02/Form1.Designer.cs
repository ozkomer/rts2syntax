namespace Atc02
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
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.bConnect = new System.Windows.Forms.Button();
            this.bDisconnect = new System.Windows.Forms.Button();
            this.bOpenRem = new System.Windows.Forms.Button();
            this.bReadSet = new System.Windows.Forms.Button();
            this.bUpdatePC = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudBFL = new System.Windows.Forms.NumericUpDown();
            this.bSetBFL = new System.Windows.Forms.Button();
            this.trackBarFan = new System.Windows.Forms.TrackBar();
            this.bSetFan = new System.Windows.Forms.Button();
            this.bCloseRemote = new System.Windows.Forms.Button();
            this.bFindOptimal = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBFL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFan)).BeginInit();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 2400;
            this.serialPort1.PortName = "COM3";
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(12, 12);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(75, 23);
            this.bConnect.TabIndex = 0;
            this.bConnect.Text = "Connect";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.bConnect_Click);
            // 
            // bDisconnect
            // 
            this.bDisconnect.Enabled = false;
            this.bDisconnect.Location = new System.Drawing.Point(93, 12);
            this.bDisconnect.Name = "bDisconnect";
            this.bDisconnect.Size = new System.Drawing.Size(75, 23);
            this.bDisconnect.TabIndex = 1;
            this.bDisconnect.Text = "Disconnect";
            this.bDisconnect.UseVisualStyleBackColor = true;
            this.bDisconnect.Click += new System.EventHandler(this.bDisconnect_Click);
            // 
            // bOpenRem
            // 
            this.bOpenRem.Location = new System.Drawing.Point(6, 19);
            this.bOpenRem.Name = "bOpenRem";
            this.bOpenRem.Size = new System.Drawing.Size(98, 23);
            this.bOpenRem.TabIndex = 2;
            this.bOpenRem.Text = "Open Remote";
            this.bOpenRem.UseVisualStyleBackColor = true;
            this.bOpenRem.Click += new System.EventHandler(this.bOpenRem_Click);
            // 
            // bReadSet
            // 
            this.bReadSet.Location = new System.Drawing.Point(144, 48);
            this.bReadSet.Name = "bReadSet";
            this.bReadSet.Size = new System.Drawing.Size(98, 23);
            this.bReadSet.TabIndex = 3;
            this.bReadSet.Text = "Read Set";
            this.bReadSet.UseVisualStyleBackColor = true;
            this.bReadSet.Click += new System.EventHandler(this.bReadSet_Click);
            // 
            // bUpdatePC
            // 
            this.bUpdatePC.Location = new System.Drawing.Point(6, 48);
            this.bUpdatePC.Name = "bUpdatePC";
            this.bUpdatePC.Size = new System.Drawing.Size(75, 23);
            this.bUpdatePC.TabIndex = 4;
            this.bUpdatePC.Text = "Update PC";
            this.bUpdatePC.UseVisualStyleBackColor = true;
            this.bUpdatePC.Click += new System.EventHandler(this.bUpdatePC_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bFindOptimal);
            this.groupBox1.Controls.Add(this.nudBFL);
            this.groupBox1.Controls.Add(this.bSetBFL);
            this.groupBox1.Controls.Add(this.trackBarFan);
            this.groupBox1.Controls.Add(this.bSetFan);
            this.groupBox1.Controls.Add(this.bCloseRemote);
            this.groupBox1.Controls.Add(this.bOpenRem);
            this.groupBox1.Controls.Add(this.bUpdatePC);
            this.groupBox1.Controls.Add(this.bReadSet);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 209);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Comandos";
            // 
            // nudBFL
            // 
            this.nudBFL.DecimalPlaces = 2;
            this.nudBFL.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudBFL.Location = new System.Drawing.Point(129, 122);
            this.nudBFL.Maximum = new decimal(new int[] {
            210,
            0,
            0,
            0});
            this.nudBFL.Minimum = new decimal(new int[] {
            130,
            0,
            0,
            0});
            this.nudBFL.Name = "nudBFL";
            this.nudBFL.Size = new System.Drawing.Size(69, 20);
            this.nudBFL.TabIndex = 10;
            this.nudBFL.Value = new decimal(new int[] {
            170,
            0,
            0,
            0});
            this.nudBFL.ValueChanged += new System.EventHandler(this.nudBFL_ValueChanged);
            // 
            // bSetBFL
            // 
            this.bSetBFL.Location = new System.Drawing.Point(6, 119);
            this.bSetBFL.Name = "bSetBFL";
            this.bSetBFL.Size = new System.Drawing.Size(117, 23);
            this.bSetBFL.TabIndex = 9;
            this.bSetBFL.Text = "Set BFL";
            this.bSetBFL.UseVisualStyleBackColor = true;
            this.bSetBFL.Click += new System.EventHandler(this.bSetBFL_Click);
            // 
            // trackBarFan
            // 
            this.trackBarFan.Location = new System.Drawing.Point(87, 77);
            this.trackBarFan.Maximum = 100;
            this.trackBarFan.Name = "trackBarFan";
            this.trackBarFan.Size = new System.Drawing.Size(155, 45);
            this.trackBarFan.TabIndex = 8;
            this.trackBarFan.TickFrequency = 10;
            this.trackBarFan.Scroll += new System.EventHandler(this.trackBarFan_Scroll);
            // 
            // bSetFan
            // 
            this.bSetFan.Location = new System.Drawing.Point(6, 90);
            this.bSetFan.Name = "bSetFan";
            this.bSetFan.Size = new System.Drawing.Size(75, 23);
            this.bSetFan.TabIndex = 7;
            this.bSetFan.Text = "Set Fan";
            this.bSetFan.UseVisualStyleBackColor = true;
            this.bSetFan.Click += new System.EventHandler(this.bSetFan_Click);
            // 
            // bCloseRemote
            // 
            this.bCloseRemote.Location = new System.Drawing.Point(144, 19);
            this.bCloseRemote.Name = "bCloseRemote";
            this.bCloseRemote.Size = new System.Drawing.Size(98, 23);
            this.bCloseRemote.TabIndex = 5;
            this.bCloseRemote.Text = "Close Remote";
            this.bCloseRemote.UseVisualStyleBackColor = true;
            this.bCloseRemote.Click += new System.EventHandler(this.bCloseRemote_Click);
            // 
            // bFindOptimal
            // 
            this.bFindOptimal.Location = new System.Drawing.Point(6, 148);
            this.bFindOptimal.Name = "bFindOptimal";
            this.bFindOptimal.Size = new System.Drawing.Size(75, 23);
            this.bFindOptimal.TabIndex = 11;
            this.bFindOptimal.Text = "Find Optimal";
            this.bFindOptimal.UseVisualStyleBackColor = true;
            this.bFindOptimal.Click += new System.EventHandler(this.bFindOptimal_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bDisconnect);
            this.Controls.Add(this.bConnect);
            this.Name = "Form1";
            this.Text = "ATC_02";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBFL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.Button bDisconnect;
        private System.Windows.Forms.Button bOpenRem;
        private System.Windows.Forms.Button bReadSet;
        private System.Windows.Forms.Button bUpdatePC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bCloseRemote;
        private System.Windows.Forms.Button bSetFan;
        private System.Windows.Forms.TrackBar trackBarFan;
        private System.Windows.Forms.Button bSetBFL;
        private System.Windows.Forms.NumericUpDown nudBFL;
        private System.Windows.Forms.Button bFindOptimal;
    }
}

