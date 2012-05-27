namespace DriverTest
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
            this.bConnect = new System.Windows.Forms.Button();
            this.bDisconnect = new System.Windows.Forms.Button();
            this.bSetPosition = new System.Windows.Forms.Button();
            this.tbPosition = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bConnect
            // 
            this.bConnect.Location = new System.Drawing.Point(29, 24);
            this.bConnect.Name = "bConnect";
            this.bConnect.Size = new System.Drawing.Size(75, 23);
            this.bConnect.TabIndex = 0;
            this.bConnect.Text = "Connect";
            this.bConnect.UseVisualStyleBackColor = true;
            this.bConnect.Click += new System.EventHandler(this.bConnect_Click);
            // 
            // bDisconnect
            // 
            this.bDisconnect.Location = new System.Drawing.Point(152, 27);
            this.bDisconnect.Name = "bDisconnect";
            this.bDisconnect.Size = new System.Drawing.Size(75, 23);
            this.bDisconnect.TabIndex = 1;
            this.bDisconnect.Text = "Disconnect";
            this.bDisconnect.UseVisualStyleBackColor = true;
            this.bDisconnect.Click += new System.EventHandler(this.bDisconnect_Click);
            // 
            // bSetPosition
            // 
            this.bSetPosition.Location = new System.Drawing.Point(24, 74);
            this.bSetPosition.Name = "bSetPosition";
            this.bSetPosition.Size = new System.Drawing.Size(75, 23);
            this.bSetPosition.TabIndex = 2;
            this.bSetPosition.Text = "Position";
            this.bSetPosition.UseVisualStyleBackColor = true;
            this.bSetPosition.Click += new System.EventHandler(this.bSetPosition_Click);
            // 
            // tbPosition
            // 
            this.tbPosition.Location = new System.Drawing.Point(105, 76);
            this.tbPosition.Name = "tbPosition";
            this.tbPosition.Size = new System.Drawing.Size(100, 20);
            this.tbPosition.TabIndex = 3;
            this.tbPosition.Text = "3500";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tbPosition);
            this.Controls.Add(this.bSetPosition);
            this.Controls.Add(this.bDisconnect);
            this.Controls.Add(this.bConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bConnect;
        private System.Windows.Forms.Button bDisconnect;
        private System.Windows.Forms.Button bSetPosition;
        private System.Windows.Forms.TextBox tbPosition;

    }
}

