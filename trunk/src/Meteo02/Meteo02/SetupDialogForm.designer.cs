namespace ASCOM.Meteo02
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelMaxWindSpeed = new System.Windows.Forms.Label();
            this.labelMinDewPointDelta = new System.Windows.Forms.Label();
            this.labelMaxHumidity = new System.Windows.Forms.Label();
            this.textBoxMaxWindSpeed = new System.Windows.Forms.TextBox();
            this.textBoxMinDewPoint = new System.Windows.Forms.TextBox();
            this.textBoxMaxHumidity = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.rbWeatherDB = new System.Windows.Forms.RadioButton();
            this.gbSafeUnsafeLogic = new System.Windows.Forms.GroupBox();
            this.rbPromptWebSite = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbSafeUnsafeLogic.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(522, 406);
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
            this.cmdCancel.Location = new System.Drawing.Point(12, 406);
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
            this.picASCOM.Image = global::ASCOM.Meteo02.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(533, 12);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(278, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chase 500 CTIO Database.";
            // 
            // labelMaxWindSpeed
            // 
            this.labelMaxWindSpeed.AutoSize = true;
            this.labelMaxWindSpeed.Location = new System.Drawing.Point(11, 42);
            this.labelMaxWindSpeed.Name = "labelMaxWindSpeed";
            this.labelMaxWindSpeed.Size = new System.Drawing.Size(124, 13);
            this.labelMaxWindSpeed.TabIndex = 5;
            this.labelMaxWindSpeed.Text = "Max Wind Speed [knots]";
            // 
            // labelMinDewPointDelta
            // 
            this.labelMinDewPointDelta.AutoSize = true;
            this.labelMinDewPointDelta.Location = new System.Drawing.Point(12, 68);
            this.labelMinDewPointDelta.Name = "labelMinDewPointDelta";
            this.labelMinDewPointDelta.Size = new System.Drawing.Size(123, 13);
            this.labelMinDewPointDelta.TabIndex = 6;
            this.labelMinDewPointDelta.Text = "Min Dew Point Delta [°c]";
            // 
            // labelMaxHumidity
            // 
            this.labelMaxHumidity.AutoSize = true;
            this.labelMaxHumidity.Location = new System.Drawing.Point(11, 94);
            this.labelMaxHumidity.Name = "labelMaxHumidity";
            this.labelMaxHumidity.Size = new System.Drawing.Size(87, 13);
            this.labelMaxHumidity.TabIndex = 7;
            this.labelMaxHumidity.Text = "Max Humidity [%]";
            // 
            // textBoxMaxWindSpeed
            // 
            this.textBoxMaxWindSpeed.Location = new System.Drawing.Point(141, 39);
            this.textBoxMaxWindSpeed.Name = "textBoxMaxWindSpeed";
            this.textBoxMaxWindSpeed.ReadOnly = true;
            this.textBoxMaxWindSpeed.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxWindSpeed.TabIndex = 8;
            // 
            // textBoxMinDewPoint
            // 
            this.textBoxMinDewPoint.Location = new System.Drawing.Point(141, 65);
            this.textBoxMinDewPoint.Name = "textBoxMinDewPoint";
            this.textBoxMinDewPoint.ReadOnly = true;
            this.textBoxMinDewPoint.Size = new System.Drawing.Size(100, 20);
            this.textBoxMinDewPoint.TabIndex = 9;
            // 
            // textBoxMaxHumidity
            // 
            this.textBoxMaxHumidity.Location = new System.Drawing.Point(141, 91);
            this.textBoxMaxHumidity.Name = "textBoxMaxHumidity";
            this.textBoxMaxHumidity.ReadOnly = true;
            this.textBoxMaxHumidity.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxHumidity.TabIndex = 10;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 74);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(569, 326);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxMaxHumidity);
            this.tabPage1.Controls.Add(this.textBoxMinDewPoint);
            this.tabPage1.Controls.Add(this.labelMaxWindSpeed);
            this.tabPage1.Controls.Add(this.textBoxMaxWindSpeed);
            this.tabPage1.Controls.Add(this.labelMinDewPointDelta);
            this.tabPage1.Controls.Add(this.labelMaxHumidity);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(561, 300);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Weather Database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.webBrowser1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(561, 300);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Prompt WebSite";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowNavigation = false;
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Location = new System.Drawing.Point(6, 6);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(532, 280);
            this.webBrowser1.TabIndex = 5;
            this.webBrowser1.Url = new System.Uri("http://skynet.unc.edu/live/", System.UriKind.Absolute);
            // 
            // rbWeatherDB
            // 
            this.rbWeatherDB.AutoSize = true;
            this.rbWeatherDB.Checked = true;
            this.rbWeatherDB.Location = new System.Drawing.Point(10, 19);
            this.rbWeatherDB.Name = "rbWeatherDB";
            this.rbWeatherDB.Size = new System.Drawing.Size(115, 17);
            this.rbWeatherDB.TabIndex = 12;
            this.rbWeatherDB.TabStop = true;
            this.rbWeatherDB.Text = "Weather Database";
            this.rbWeatherDB.UseVisualStyleBackColor = true;
            // 
            // gbSafeUnsafeLogic
            // 
            this.gbSafeUnsafeLogic.Controls.Add(this.rbPromptWebSite);
            this.gbSafeUnsafeLogic.Controls.Add(this.rbWeatherDB);
            this.gbSafeUnsafeLogic.Location = new System.Drawing.Point(12, 12);
            this.gbSafeUnsafeLogic.Name = "gbSafeUnsafeLogic";
            this.gbSafeUnsafeLogic.Size = new System.Drawing.Size(261, 56);
            this.gbSafeUnsafeLogic.TabIndex = 13;
            this.gbSafeUnsafeLogic.TabStop = false;
            this.gbSafeUnsafeLogic.Text = "Safe Unsafe Logic";
            // 
            // rbPromptWebSite
            // 
            this.rbPromptWebSite.AutoSize = true;
            this.rbPromptWebSite.Location = new System.Drawing.Point(157, 19);
            this.rbPromptWebSite.Name = "rbPromptWebSite";
            this.rbPromptWebSite.Size = new System.Drawing.Size(99, 17);
            this.rbPromptWebSite.TabIndex = 13;
            this.rbPromptWebSite.TabStop = true;
            this.rbPromptWebSite.Text = "PromptWebSite";
            this.rbPromptWebSite.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 439);
            this.Controls.Add(this.gbSafeUnsafeLogic);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chase 500 Weather";
            this.Load += new System.EventHandler(this.SetupDialogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.gbSafeUnsafeLogic.ResumeLayout(false);
            this.gbSafeUnsafeLogic.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelMaxWindSpeed;
        private System.Windows.Forms.Label labelMinDewPointDelta;
        private System.Windows.Forms.Label labelMaxHumidity;
        private System.Windows.Forms.TextBox textBoxMaxWindSpeed;
        private System.Windows.Forms.TextBox textBoxMinDewPoint;
        private System.Windows.Forms.TextBox textBoxMaxHumidity;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RadioButton rbWeatherDB;
        private System.Windows.Forms.GroupBox gbSafeUnsafeLogic;
        private System.Windows.Forms.RadioButton rbPromptWebSite;
    }
}