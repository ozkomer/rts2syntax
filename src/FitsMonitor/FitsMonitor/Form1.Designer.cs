﻿namespace FitsMonitor
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
            this.dataGridViewATC02 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bLogFile = new System.Windows.Forms.Button();
            this.tbLogFile = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelSubFolderDeep = new System.Windows.Forms.Label();
            this.numericUpDownSubFolderDeep = new System.Windows.Forms.NumericUpDown();
            this.bOfflineFolder = new System.Windows.Forms.Button();
            this.tbOfflineFolder = new System.Windows.Forms.TextBox();
            this.fsWatchFits = new System.IO.FileSystemWatcher();
            this.fsWatchOfficinaStelare = new System.IO.FileSystemWatcher();
            this.folderBrowserOffline = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorkerATC02 = new System.ComponentModel.BackgroundWorker();
            this.timerAtc02 = new System.Windows.Forms.Timer(this.components);
            this.ColumnParametro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelLastDetectedLogFile = new System.Windows.Forms.Label();
            this.labelLastDetectedValues = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewATC02)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubFolderDeep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatchFits)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fsWatchOfficinaStelare)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageLocation = "http://www.das.uchile.cl/~jose/chase500/P1070498.JPG";
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
            this.showToolStripMenuItem.Image = global::FitsMonitor.Properties.Resources.Play_1_Normal_icon;
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::FitsMonitor.Properties.Resources.Stop_Normal_Red_icon;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(32, 6);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(313, 20);
            this.textBoxUrl.TabIndex = 1;
            this.textBoxUrl.Text = "http://www.das.uchile.cl/~jose/chase500/P1070498.JPG";
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
            this.tabPage2.Controls.Add(this.labelLastDetectedValues);
            this.tabPage2.Controls.Add(this.labelLastDetectedLogFile);
            this.tabPage2.Controls.Add(this.dataGridViewATC02);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.bLogFile);
            this.tabPage2.Controls.Add(this.tbLogFile);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(357, 310);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Header";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewATC02
            // 
            this.dataGridViewATC02.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewATC02.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnParametro,
            this.Column2});
            this.dataGridViewATC02.Location = new System.Drawing.Point(6, 106);
            this.dataGridViewATC02.Name = "dataGridViewATC02";
            this.dataGridViewATC02.ReadOnly = true;
            this.dataGridViewATC02.Size = new System.Drawing.Size(345, 181);
            this.dataGridViewATC02.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FitsMonitor.Properties.Settings.Default, "AtcLogFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(3, 57);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(345, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = global::FitsMonitor.Properties.Settings.Default.AtcLogFile;
            // 
            // bLogFile
            // 
            this.bLogFile.Location = new System.Drawing.Point(6, 6);
            this.bLogFile.Name = "bLogFile";
            this.bLogFile.Size = new System.Drawing.Size(75, 23);
            this.bLogFile.TabIndex = 1;
            this.bLogFile.Text = "Log Folder";
            this.bLogFile.UseVisualStyleBackColor = true;
            this.bLogFile.Click += new System.EventHandler(this.bLogFile_Click);
            // 
            // tbLogFile
            // 
            this.tbLogFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FitsMonitor.Properties.Settings.Default, "OfficinaStellareLog", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbLogFile.Location = new System.Drawing.Point(87, 8);
            this.tbLogFile.Name = "tbLogFile";
            this.tbLogFile.Size = new System.Drawing.Size(264, 20);
            this.tbLogFile.TabIndex = 0;
            this.tbLogFile.Text = global::FitsMonitor.Properties.Settings.Default.OfficinaStellareLog;
            // 
            // tabPage3
            // 
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
            // labelSubFolderDeep
            // 
            this.labelSubFolderDeep.AutoSize = true;
            this.labelSubFolderDeep.Location = new System.Drawing.Point(6, 37);
            this.labelSubFolderDeep.Name = "labelSubFolderDeep";
            this.labelSubFolderDeep.Size = new System.Drawing.Size(87, 13);
            this.labelSubFolderDeep.TabIndex = 5;
            this.labelSubFolderDeep.Text = "Sub Folder Deep";
            // 
            // numericUpDownSubFolderDeep
            // 
            this.numericUpDownSubFolderDeep.Location = new System.Drawing.Point(110, 35);
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
            this.bOfflineFolder.Location = new System.Drawing.Point(306, 3);
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
            this.tbOfflineFolder.Size = new System.Drawing.Size(273, 20);
            this.tbOfflineFolder.TabIndex = 0;
            this.tbOfflineFolder.Text = "C:\\Users\\chase\\Documents\\ACP Astronomy\\Images";
            this.tbOfflineFolder.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            // fsWatchOfficinaStelare
            // 
            this.fsWatchOfficinaStelare.EnableRaisingEvents = true;
            this.fsWatchOfficinaStelare.Filter = "*.log";
            this.fsWatchOfficinaStelare.NotifyFilter = System.IO.NotifyFilters.Size;
            this.fsWatchOfficinaStelare.Path = global::FitsMonitor.Properties.Settings.Default.OfficinaStellareLog;
            this.fsWatchOfficinaStelare.SynchronizingObject = this;
            this.fsWatchOfficinaStelare.Changed += new System.IO.FileSystemEventHandler(this.fsWatchOfficinaStelare_Changed);
            // 
            // folderBrowserOffline
            // 
            this.folderBrowserOffline.ShowNewFolderButton = false;
            // 
            // backgroundWorkerATC02
            // 
            this.backgroundWorkerATC02.WorkerSupportsCancellation = true;
            this.backgroundWorkerATC02.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerATC02_DoWork);
            this.backgroundWorkerATC02.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerATC02_RunWorkerCompleted);
            this.backgroundWorkerATC02.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerATC02_ProgressChanged);
            // 
            // timerAtc02
            // 
            this.timerAtc02.Enabled = true;
            this.timerAtc02.Interval = 15000;
            this.timerAtc02.Tick += new System.EventHandler(this.timerAtc02_Tick);
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
            // labelLastDetectedLogFile
            // 
            this.labelLastDetectedLogFile.AutoSize = true;
            this.labelLastDetectedLogFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastDetectedLogFile.Location = new System.Drawing.Point(102, 36);
            this.labelLastDetectedLogFile.Name = "labelLastDetectedLogFile";
            this.labelLastDetectedLogFile.Size = new System.Drawing.Size(136, 13);
            this.labelLastDetectedLogFile.TabIndex = 4;
            this.labelLastDetectedLogFile.Text = "Last Detected Log File";
            // 
            // labelLastDetectedValues
            // 
            this.labelLastDetectedValues.AutoSize = true;
            this.labelLastDetectedValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastDetectedValues.Location = new System.Drawing.Point(109, 85);
            this.labelLastDetectedValues.Name = "labelLastDetectedValues";
            this.labelLastDetectedValues.Size = new System.Drawing.Size(129, 13);
            this.labelLastDetectedValues.TabIndex = 5;
            this.labelLastDetectedValues.Text = "Last Detected Values";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 355);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(402, 393);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.fsWatchOfficinaStelare)).EndInit();
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
        private System.Windows.Forms.Button bLogFile;
        private System.Windows.Forms.TextBox tbLogFile;
        private System.IO.FileSystemWatcher fsWatchOfficinaStelare;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button bOfflineFolder;
        private System.Windows.Forms.TextBox tbOfflineFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserOffline;
        private System.ComponentModel.BackgroundWorker backgroundWorkerATC02;
        private System.Windows.Forms.Label labelSubFolderDeep;
        private System.Windows.Forms.NumericUpDown numericUpDownSubFolderDeep;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridViewATC02;
        private System.Windows.Forms.Timer timerAtc02;
        private System.Windows.Forms.Label labelLastDetectedLogFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParametro;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label labelLastDetectedValues;
    }
}
