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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.pictureBox1.ImageLocation = "http://www.das.uchile.cl/~jose/chase500/P1070498.JPG";
            this.pictureBox1.Location = new System.Drawing.Point(15, 43);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(355, 327);
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
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(111, 48);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Image = global::FitsMonitor.Properties.Resources.Play_1_Normal_icon;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::FitsMonitor.Properties.Resources.Stop_Normal_Red_icon;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(43, 7);
            this.textBoxUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(416, 22);
            this.textBoxUrl.TabIndex = 1;
            this.textBoxUrl.Text = "http://www.das.uchile.cl/~jose/chase500/P1070498.JPG";
            // 
            // bCopiarOffline
            // 
            this.bCopiarOffline.Location = new System.Drawing.Point(320, 350);
            this.bCopiarOffline.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCopiarOffline.Name = "bCopiarOffline";
            this.bCopiarOffline.Size = new System.Drawing.Size(152, 28);
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
            this.tabControl1.Location = new System.Drawing.Point(16, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(487, 414);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxUrl);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(479, 385);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sample";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bReadStatus);
            this.tabPage2.Controls.Add(this.labelLastDetectedValues);
            this.tabPage2.Controls.Add(this.dataGridViewATC02);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(479, 385);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Header";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bReadStatus
            // 
            this.bReadStatus.Location = new System.Drawing.Point(8, 7);
            this.bReadStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bReadStatus.Name = "bReadStatus";
            this.bReadStatus.Size = new System.Drawing.Size(100, 28);
            this.bReadStatus.TabIndex = 5;
            this.bReadStatus.Text = "Read Status";
            this.bReadStatus.UseVisualStyleBackColor = true;
            this.bReadStatus.Click += new System.EventHandler(this.bReadStatus_Click);
            // 
            // labelLastDetectedValues
            // 
            this.labelLastDetectedValues.AutoSize = true;
            this.labelLastDetectedValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastDetectedValues.Location = new System.Drawing.Point(143, 38);
            this.labelLastDetectedValues.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLastDetectedValues.Name = "labelLastDetectedValues";
            this.labelLastDetectedValues.Size = new System.Drawing.Size(163, 17);
            this.labelLastDetectedValues.TabIndex = 5;
            this.labelLastDetectedValues.Text = "Last Detected Values";
            // 
            // dataGridViewATC02
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewATC02.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewATC02.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewATC02.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnParametro,
            this.Column2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewATC02.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewATC02.Location = new System.Drawing.Point(8, 78);
            this.dataGridViewATC02.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewATC02.Name = "dataGridViewATC02";
            this.dataGridViewATC02.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewATC02.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewATC02.RowTemplate.Height = 24;
            this.dataGridViewATC02.Size = new System.Drawing.Size(460, 297);
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
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(479, 385);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "OffLine";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // bCheckRaDec
            // 
            this.bCheckRaDec.Location = new System.Drawing.Point(272, 159);
            this.bCheckRaDec.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCheckRaDec.Name = "bCheckRaDec";
            this.bCheckRaDec.Size = new System.Drawing.Size(141, 28);
            this.bCheckRaDec.TabIndex = 7;
            this.bCheckRaDec.Text = "Check Ra Dec";
            this.bCheckRaDec.UseVisualStyleBackColor = true;
            this.bCheckRaDec.Click += new System.EventHandler(this.bCheckRaDec_Click);
            // 
            // bCorrectNames
            // 
            this.bCorrectNames.Location = new System.Drawing.Point(48, 159);
            this.bCorrectNames.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bCorrectNames.Name = "bCorrectNames";
            this.bCorrectNames.Size = new System.Drawing.Size(120, 28);
            this.bCorrectNames.TabIndex = 6;
            this.bCorrectNames.Text = "CorrectNames";
            this.bCorrectNames.UseVisualStyleBackColor = true;
            this.bCorrectNames.Click += new System.EventHandler(this.bCorrectNames_Click);
            // 
            // labelSubFolderDeep
            // 
            this.labelSubFolderDeep.AutoSize = true;
            this.labelSubFolderDeep.Location = new System.Drawing.Point(268, 44);
            this.labelSubFolderDeep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSubFolderDeep.Name = "labelSubFolderDeep";
            this.labelSubFolderDeep.Size = new System.Drawing.Size(115, 17);
            this.labelSubFolderDeep.TabIndex = 5;
            this.labelSubFolderDeep.Text = "Sub Folder Deep";
            // 
            // numericUpDownSubFolderDeep
            // 
            this.numericUpDownSubFolderDeep.Location = new System.Drawing.Point(392, 42);
            this.numericUpDownSubFolderDeep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownSubFolderDeep.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownSubFolderDeep.Name = "numericUpDownSubFolderDeep";
            this.numericUpDownSubFolderDeep.Size = new System.Drawing.Size(53, 22);
            this.numericUpDownSubFolderDeep.TabIndex = 4;
            this.numericUpDownSubFolderDeep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bOfflineFolder
            // 
            this.bOfflineFolder.Location = new System.Drawing.Point(4, 38);
            this.bOfflineFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bOfflineFolder.Name = "bOfflineFolder";
            this.bOfflineFolder.Size = new System.Drawing.Size(64, 28);
            this.bOfflineFolder.TabIndex = 3;
            this.bOfflineFolder.Text = "Folder";
            this.bOfflineFolder.UseVisualStyleBackColor = true;
            this.bOfflineFolder.Click += new System.EventHandler(this.bOfflineFolder_Click);
            // 
            // tbOfflineFolder
            // 
            this.tbOfflineFolder.Location = new System.Drawing.Point(12, 6);
            this.tbOfflineFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbOfflineFolder.Name = "tbOfflineFolder";
            this.tbOfflineFolder.Size = new System.Drawing.Size(455, 22);
            this.tbOfflineFolder.TabIndex = 0;
            this.tbOfflineFolder.Text = "C:\\Users\\chase\\Documents\\ACP Astronomy\\Images\\20120719";
            this.tbOfflineFolder.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Size = new System.Drawing.Size(479, 385);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 444);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(533, 476);
            this.Name = "Form1";
            this.Text = "Fits Monitor";
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
    }
}

