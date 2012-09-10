namespace PowerFocus
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewATC02 = new System.Windows.Forms.DataGridView();
            this.ColumnParameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bReadStatus = new System.Windows.Forms.Button();
            this.bSetFan = new System.Windows.Forms.Button();
            this.trackBarFan = new System.Windows.Forms.TrackBar();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewATC02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFan)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
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
            this.showToolStripMenuItem.Image = global::PowerFocus.Properties.Resources.Play_1_Normal_icon;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::PowerFocus.Properties.Resources.Stop_Normal_Red_icon;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataGridViewATC02
            // 
            this.dataGridViewATC02.AllowUserToAddRows = false;
            this.dataGridViewATC02.AllowUserToDeleteRows = false;
            this.dataGridViewATC02.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewATC02.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnParameter,
            this.ColumnValue});
            this.dataGridViewATC02.Location = new System.Drawing.Point(12, 41);
            this.dataGridViewATC02.Name = "dataGridViewATC02";
            this.dataGridViewATC02.Size = new System.Drawing.Size(259, 245);
            this.dataGridViewATC02.TabIndex = 1;
            // 
            // ColumnParameter
            // 
            this.ColumnParameter.HeaderText = "Parameter";
            this.ColumnParameter.Name = "ColumnParameter";
            this.ColumnParameter.ReadOnly = true;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            // 
            // bReadStatus
            // 
            this.bReadStatus.Location = new System.Drawing.Point(12, 12);
            this.bReadStatus.Name = "bReadStatus";
            this.bReadStatus.Size = new System.Drawing.Size(75, 23);
            this.bReadStatus.TabIndex = 2;
            this.bReadStatus.Text = "Read Status";
            this.bReadStatus.UseVisualStyleBackColor = true;
            this.bReadStatus.Click += new System.EventHandler(this.bReadStatus_Click);
            // 
            // bSetFan
            // 
            this.bSetFan.Location = new System.Drawing.Point(12, 322);
            this.bSetFan.Name = "bSetFan";
            this.bSetFan.Size = new System.Drawing.Size(75, 23);
            this.bSetFan.TabIndex = 3;
            this.bSetFan.Text = "Set Fan";
            this.bSetFan.UseVisualStyleBackColor = true;
            this.bSetFan.Click += new System.EventHandler(this.bSetFan_Click);
            // 
            // trackBarFan
            // 
            this.trackBarFan.LargeChange = 10;
            this.trackBarFan.Location = new System.Drawing.Point(93, 322);
            this.trackBarFan.Maximum = 100;
            this.trackBarFan.Name = "trackBarFan";
            this.trackBarFan.Size = new System.Drawing.Size(178, 45);
            this.trackBarFan.SmallChange = 5;
            this.trackBarFan.TabIndex = 4;
            this.trackBarFan.TickFrequency = 10;
            this.trackBarFan.Scroll += new System.EventHandler(this.trackBarFan_Scroll);
            // 
            // timerStatus
            // 
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 379);
            this.Controls.Add(this.trackBarFan);
            this.Controls.Add(this.bSetFan);
            this.Controls.Add(this.bReadStatus);
            this.Controls.Add(this.dataGridViewATC02);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Focus Server";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewATC02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewATC02;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.Button bReadStatus;
        private System.Windows.Forms.Button bSetFan;
        private System.Windows.Forms.TrackBar trackBarFan;
        private System.Windows.Forms.Timer timerStatus;
    }
}

