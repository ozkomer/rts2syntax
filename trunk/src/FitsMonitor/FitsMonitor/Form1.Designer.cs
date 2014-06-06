namespace FitsMonitor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.bCopiarOffline = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bReadStatus = new System.Windows.Forms.Button();
            this.labelLastDetectedValues = new System.Windows.Forms.Label();
            this.dataGridViewATC02 = new System.Windows.Forms.DataGridView();
            this.ColumnParametro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.bCheckRaDec = new System.Windows.Forms.Button();
            this.bCorrectNames = new System.Windows.Forms.Button();
            this.labelSubFolderDeep = new System.Windows.Forms.Label();
            this.numericUpDownSubFolderDeep = new System.Windows.Forms.NumericUpDown();
            this.bOfflineFolder = new System.Windows.Forms.Button();
            this.tbOfflineFolder = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.folderBrowserOffline = new System.Windows.Forms.FolderBrowserDialog();
            this.timerAtc02 = new System.Windows.Forms.Timer(this.components);
            this.fsWatchFits = new System.IO.FileSystemWatcher();
            this.timerRSync = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewATC02)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubFolderDeep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatchFits)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageLocation = "http://146.83.9.11/pictures/chase500night.JPG";
            this.pictureBox1.Location = new System.Drawing.Point(11, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(266, 266);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Fits Monitor";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Image = global::FitsMonitor.Properties.Resources.Play_1_Normal_icon;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::FitsMonitor.Properties.Resources.Stop_Normal_Red_icon;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(32, 6);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(313, 20);
            this.textBoxUrl.TabIndex = 1;
            this.textBoxUrl.Text = "http://146.83.9.11/pictures/chase500night.JPG";
            // 
            // bCopiarOffline
            // 
            this.bCopiarOffline.Location = new System.Drawing.Point(240, 284);
            this.bCopiarOffline.Name = "bCopiarOffline";
            this.bCopiarOffline.Size = new System.Drawing.Size(114, 23);
            this.bCopiarOffline.TabIndex = 2;
            this.bCopiarOffline.Text = "Start Offline Copy!";
            this.bCopiarOffline.UseVisualStyleBackColor = true;
            this.bCopiarOffline.Click += new System.EventHandler(this.bCopiarOffline_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(365, 336);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxUrl);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(357, 310);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sample";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bReadStatus);
            this.tabPage2.Controls.Add(this.labelLastDetectedValues);
            this.tabPage2.Controls.Add(this.dataGridViewATC02);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(357, 310);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Header";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bReadStatus
            // 
            this.bReadStatus.Location = new System.Drawing.Point(6, 6);
            this.bReadStatus.Name = "bReadStatus";
            this.bReadStatus.Size = new System.Drawing.Size(75, 23);
            this.bReadStatus.TabIndex = 5;
            this.bReadStatus.Text = "Read Status";
            this.bReadStatus.UseVisualStyleBackColor = true;
            this.bReadStatus.Click += new System.EventHandler(this.bReadStatus_Click);
            // 
            // labelLastDetectedValues
            // 
            this.labelLastDetectedValues.AutoSize = true;
            this.labelLastDetectedValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastDetectedValues.Location = new System.Drawing.Point(107, 31);
            this.labelLastDetectedValues.Name = "labelLastDetectedValues";
            this.labelLastDetectedValues.Size = new System.Drawing.Size(129, 13);
            this.labelLastDetectedValues.TabIndex = 5;
            this.labelLastDetectedValues.Text = "Last Detected Values";
            // 
            // dataGridViewATC02
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewATC02.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewATC02.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewATC02.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnParametro,
            this.Column2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewATC02.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewATC02.Location = new System.Drawing.Point(6, 63);
            this.dataGridViewATC02.Name = "dataGridViewATC02";
            this.dataGridViewATC02.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewATC02.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewATC02.RowTemplate.Height = 24;
            this.dataGridViewATC02.Size = new System.Drawing.Size(345, 241);
            this.dataGridViewATC02.TabIndex = 3;
            // 
            // ColumnParametro
            // 
            this.ColumnParametro.HeaderText = "Parameter";
            this.ColumnParametro.Name = "ColumnParametro";
            this.ColumnParametro.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Value";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.bCheckRaDec);
            this.tabPage3.Controls.Add(this.bCorrectNames);
            this.tabPage3.Controls.Add(this.labelSubFolderDeep);
            this.tabPage3.Controls.Add(this.numericUpDownSubFolderDeep);
            this.tabPage3.Controls.Add(this.bOfflineFolder);
            this.tabPage3.Controls.Add(this.bCopiarOffline);
            this.tabPage3.Controls.Add(this.tbOfflineFolder);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(357, 310);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "OffLine";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // bCheckRaDec
            // 
            this.bCheckRaDec.Location = new System.Drawing.Point(204, 129);
            this.bCheckRaDec.Name = "bCheckRaDec";
            this.bCheckRaDec.Size = new System.Drawing.Size(106, 23);
            this.bCheckRaDec.TabIndex = 7;
            this.bCheckRaDec.Text = "Check Ra Dec";
            this.bCheckRaDec.UseVisualStyleBackColor = true;
            this.bCheckRaDec.Click += new System.EventHandler(this.bCheckRaDec_Click);
            // 
            // bCorrectNames
            // 
            this.bCorrectNames.Location = new System.Drawing.Point(36, 129);
            this.bCorrectNames.Name = "bCorrectNames";
            this.bCorrectNames.Size = new System.Drawing.Size(90, 23);
            this.bCorrectNames.TabIndex = 6;
            this.bCorrectNames.Text = "CorrectNames";
            this.bCorrectNames.UseVisualStyleBackColor = true;
            this.bCorrectNames.Click += new System.EventHandler(this.bCorrectNames_Click);
            // 
            // labelSubFolderDeep
            // 
            this.labelSubFolderDeep.AutoSize = true;
            this.labelSubFolderDeep.Location = new System.Drawing.Point(201, 36);
            this.labelSubFolderDeep.Name = "labelSubFolderDeep";
            this.labelSubFolderDeep.Size = new System.Drawing.Size(87, 13);
            this.labelSubFolderDeep.TabIndex = 5;
            this.labelSubFolderDeep.Text = "Sub Folder Deep";
            // 
            // numericUpDownSubFolderDeep
            // 
            this.numericUpDownSubFolderDeep.Location = new System.Drawing.Point(294, 34);
            this.numericUpDownSubFolderDeep.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownSubFolderDeep.Name = "numericUpDownSubFolderDeep";
            this.numericUpDownSubFolderDeep.Size = new System.Drawing.Size(41, 20);
            this.numericUpDownSubFolderDeep.TabIndex = 4;
            this.numericUpDownSubFolderDeep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bOfflineFolder
            // 
            this.bOfflineFolder.Location = new System.Drawing.Point(3, 31);
            this.bOfflineFolder.Name = "bOfflineFolder";
            this.bOfflineFolder.Size = new System.Drawing.Size(48, 23);
            this.bOfflineFolder.TabIndex = 3;
            this.bOfflineFolder.Text = "Folder";
            this.bOfflineFolder.UseVisualStyleBackColor = true;
            this.bOfflineFolder.Click += new System.EventHandler(this.bOfflineFolder_Click);
            // 
            // tbOfflineFolder
            // 
            this.tbOfflineFolder.Location = new System.Drawing.Point(9, 5);
            this.tbOfflineFolder.Name = "tbOfflineFolder";
            this.tbOfflineFolder.Size = new System.Drawing.Size(342, 20);
            this.tbOfflineFolder.TabIndex = 0;
            this.tbOfflineFolder.Text = "C:\\Users\\chase\\Documents\\ACP Astronomy\\Images\\20120719";
            this.tbOfflineFolder.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(357, 310);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "ATC02";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // folderBrowserOffline
            // 
            this.folderBrowserOffline.ShowNewFolderButton = false;
            // 
            // timerAtc02
            // 
            this.timerAtc02.Enabled = true;
            this.timerAtc02.Interval = 15000;
            this.timerAtc02.Tick += new System.EventHandler(this.timerAtc02_Tick);
            // 
            // fsWatchFits
            // 
            this.fsWatchFits.EnableRaisingEvents = true;
            this.fsWatchFits.Filter = "*.fts";
            this.fsWatchFits.IncludeSubdirectories = true;
            this.fsWatchFits.Path = global::FitsMonitor.Properties.Settings.Default.WatchFolder;
            this.fsWatchFits.SynchronizingObject = this;
            this.fsWatchFits.Created += new System.IO.FileSystemEventHandler(this.fsWatchFits_Created);
            // 
            // timerRSync
            // 
            this.timerRSync.Enabled = global::FitsMonitor.Properties.Settings.Default.ZwickyTransferEnable;
            this.timerRSync.Interval = global::FitsMonitor.Properties.Settings.Default.RSyncTimer;
            this.timerRSync.Tick += new System.EventHandler(this.timerRSync_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 365);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(402, 392);
            this.Name = "Form1";
            this.Text = global::FitsMonitor.Properties.Settings.Default.version;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewATC02)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubFolderDeep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatchFits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.FileSystemWatcher fsWatchFits;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button bCopiarOffline;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button bOfflineFolder;
        private System.Windows.Forms.TextBox tbOfflineFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserOffline;
        private System.Windows.Forms.Label labelSubFolderDeep;
        private System.Windows.Forms.NumericUpDown numericUpDownSubFolderDeep;
        private System.Windows.Forms.DataGridView dataGridViewATC02;
        private System.Windows.Forms.Timer timerAtc02;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParametro;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label labelLastDetectedValues;
        private System.Windows.Forms.Button bCorrectNames;
        private System.Windows.Forms.Button bCheckRaDec;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button bReadStatus;
        private System.Windows.Forms.Timer timerRSync;
    }
}

