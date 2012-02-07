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
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(238, 139);
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
            this.cmdCancel.Location = new System.Drawing.Point(12, 139);
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
            this.picASCOM.Location = new System.Drawing.Point(249, 39);
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
            this.label2.Location = new System.Drawing.Point(8, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(278, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chase 500 CTIO Database.";
            // 
            // labelMaxWindSpeed
            // 
            this.labelMaxWindSpeed.AutoSize = true;
            this.labelMaxWindSpeed.Location = new System.Drawing.Point(13, 39);
            this.labelMaxWindSpeed.Name = "labelMaxWindSpeed";
            this.labelMaxWindSpeed.Size = new System.Drawing.Size(124, 13);
            this.labelMaxWindSpeed.TabIndex = 5;
            this.labelMaxWindSpeed.Text = "Max Wind Speed [knots]";
            // 
            // labelMinDewPointDelta
            // 
            this.labelMinDewPointDelta.AutoSize = true;
            this.labelMinDewPointDelta.Location = new System.Drawing.Point(14, 65);
            this.labelMinDewPointDelta.Name = "labelMinDewPointDelta";
            this.labelMinDewPointDelta.Size = new System.Drawing.Size(123, 13);
            this.labelMinDewPointDelta.TabIndex = 6;
            this.labelMinDewPointDelta.Text = "Min Dew Point Delta [°c]";
            // 
            // labelMaxHumidity
            // 
            this.labelMaxHumidity.AutoSize = true;
            this.labelMaxHumidity.Location = new System.Drawing.Point(13, 91);
            this.labelMaxHumidity.Name = "labelMaxHumidity";
            this.labelMaxHumidity.Size = new System.Drawing.Size(87, 13);
            this.labelMaxHumidity.TabIndex = 7;
            this.labelMaxHumidity.Text = "Max Humidity [%]";
            // 
            // textBoxMaxWindSpeed
            // 
            this.textBoxMaxWindSpeed.Location = new System.Drawing.Point(143, 36);
            this.textBoxMaxWindSpeed.Name = "textBoxMaxWindSpeed";
            this.textBoxMaxWindSpeed.ReadOnly = true;
            this.textBoxMaxWindSpeed.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxWindSpeed.TabIndex = 8;
            // 
            // textBoxMinDewPoint
            // 
            this.textBoxMinDewPoint.Location = new System.Drawing.Point(143, 62);
            this.textBoxMinDewPoint.Name = "textBoxMinDewPoint";
            this.textBoxMinDewPoint.ReadOnly = true;
            this.textBoxMinDewPoint.Size = new System.Drawing.Size(100, 20);
            this.textBoxMinDewPoint.TabIndex = 9;
            // 
            // textBoxMaxHumidity
            // 
            this.textBoxMaxHumidity.Location = new System.Drawing.Point(143, 88);
            this.textBoxMaxHumidity.Name = "textBoxMaxHumidity";
            this.textBoxMaxHumidity.ReadOnly = true;
            this.textBoxMaxHumidity.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxHumidity.TabIndex = 10;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 180);
            this.Controls.Add(this.textBoxMaxHumidity);
            this.Controls.Add(this.textBoxMinDewPoint);
            this.Controls.Add(this.textBoxMaxWindSpeed);
            this.Controls.Add(this.labelMaxHumidity);
            this.Controls.Add(this.labelMinDewPointDelta);
            this.Controls.Add(this.labelMaxWindSpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
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
    }
}