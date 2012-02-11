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
            SpPerfChart.ChartPen chartPen13 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen14 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen15 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen16 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen17 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen18 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen19 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen20 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen21 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen22 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen23 = new SpPerfChart.ChartPen();
            SpPerfChart.ChartPen chartPen24 = new SpPerfChart.ChartPen();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.serialPortMontura = new System.IO.Ports.SerialPort(this.components);
            this.timerReadSerial = new System.Windows.Forms.Timer(this.components);
            this.perfChartRA_x = new SpPerfChart.PerfChart();
            this.labelRA_X = new System.Windows.Forms.Label();
            this.labelRA_Y = new System.Windows.Forms.Label();
            this.perfChartRA_Y = new SpPerfChart.PerfChart();
            this.labelRA_Z = new System.Windows.Forms.Label();
            this.perfChartRA_Z = new SpPerfChart.PerfChart();
            this.labelAlpha = new System.Windows.Forms.Label();
            this.progressBarRisk = new System.Windows.Forms.ProgressBar();
            this.buttonContinue = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPortMontura
            // 
            this.serialPortMontura.PortName = "COM7";
            // 
            // timerReadSerial
            // 
            this.timerReadSerial.Interval = 250;
            this.timerReadSerial.Tick += new System.EventHandler(this.timerReadSerial_Tick);
            // 
            // perfChartRA_x
            // 
            this.perfChartRA_x.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.perfChartRA_x.Location = new System.Drawing.Point(16, 26);
            this.perfChartRA_x.Name = "perfChartRA_x";
            this.perfChartRA_x.PerfChartStyle.AntiAliasing = true;
            chartPen13.Color = System.Drawing.Color.Black;
            chartPen13.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen13.Width = 1F;
            this.perfChartRA_x.PerfChartStyle.AvgLinePen = chartPen13;
            this.perfChartRA_x.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.DarkGreen;
            this.perfChartRA_x.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.DarkGreen;
            chartPen14.Color = System.Drawing.Color.Black;
            chartPen14.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen14.Width = 1F;
            this.perfChartRA_x.PerfChartStyle.ChartLinePen = chartPen14;
            chartPen15.Color = System.Drawing.Color.Black;
            chartPen15.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen15.Width = 1F;
            this.perfChartRA_x.PerfChartStyle.HorizontalGridPen = chartPen15;
            this.perfChartRA_x.PerfChartStyle.ShowAverageLine = true;
            this.perfChartRA_x.PerfChartStyle.ShowHorizontalGridLines = true;
            this.perfChartRA_x.PerfChartStyle.ShowVerticalGridLines = true;
            chartPen16.Color = System.Drawing.Color.Black;
            chartPen16.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen16.Width = 1F;
            this.perfChartRA_x.PerfChartStyle.VerticalGridPen = chartPen16;
            this.perfChartRA_x.ScaleMode = SpPerfChart.ScaleMode.Relative;
            this.perfChartRA_x.Size = new System.Drawing.Size(274, 100);
            this.perfChartRA_x.TabIndex = 0;
            this.perfChartRA_x.TimerInterval = 100;
            this.perfChartRA_x.TimerMode = SpPerfChart.TimerMode.Disabled;
            // 
            // labelRA_X
            // 
            this.labelRA_X.AutoSize = true;
            this.labelRA_X.Location = new System.Drawing.Point(16, 3);
            this.labelRA_X.Name = "labelRA_X";
            this.labelRA_X.Size = new System.Drawing.Size(35, 13);
            this.labelRA_X.TabIndex = 1;
            this.labelRA_X.Text = "RA_X";
            // 
            // labelRA_Y
            // 
            this.labelRA_Y.AutoSize = true;
            this.labelRA_Y.Location = new System.Drawing.Point(16, 138);
            this.labelRA_Y.Name = "labelRA_Y";
            this.labelRA_Y.Size = new System.Drawing.Size(35, 13);
            this.labelRA_Y.TabIndex = 3;
            this.labelRA_Y.Text = "RA_Y";
            // 
            // perfChartRA_Y
            // 
            this.perfChartRA_Y.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.perfChartRA_Y.Location = new System.Drawing.Point(16, 161);
            this.perfChartRA_Y.Name = "perfChartRA_Y";
            this.perfChartRA_Y.PerfChartStyle.AntiAliasing = true;
            chartPen17.Color = System.Drawing.Color.Black;
            chartPen17.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen17.Width = 1F;
            this.perfChartRA_Y.PerfChartStyle.AvgLinePen = chartPen17;
            this.perfChartRA_Y.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.DarkGreen;
            this.perfChartRA_Y.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.DarkGreen;
            chartPen18.Color = System.Drawing.Color.Black;
            chartPen18.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen18.Width = 1F;
            this.perfChartRA_Y.PerfChartStyle.ChartLinePen = chartPen18;
            chartPen19.Color = System.Drawing.Color.Black;
            chartPen19.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen19.Width = 1F;
            this.perfChartRA_Y.PerfChartStyle.HorizontalGridPen = chartPen19;
            this.perfChartRA_Y.PerfChartStyle.ShowAverageLine = true;
            this.perfChartRA_Y.PerfChartStyle.ShowHorizontalGridLines = true;
            this.perfChartRA_Y.PerfChartStyle.ShowVerticalGridLines = true;
            chartPen20.Color = System.Drawing.Color.Black;
            chartPen20.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen20.Width = 1F;
            this.perfChartRA_Y.PerfChartStyle.VerticalGridPen = chartPen20;
            this.perfChartRA_Y.ScaleMode = SpPerfChart.ScaleMode.Relative;
            this.perfChartRA_Y.Size = new System.Drawing.Size(274, 100);
            this.perfChartRA_Y.TabIndex = 2;
            this.perfChartRA_Y.TimerInterval = 100;
            this.perfChartRA_Y.TimerMode = SpPerfChart.TimerMode.Disabled;
            // 
            // labelRA_Z
            // 
            this.labelRA_Z.AutoSize = true;
            this.labelRA_Z.Location = new System.Drawing.Point(16, 275);
            this.labelRA_Z.Name = "labelRA_Z";
            this.labelRA_Z.Size = new System.Drawing.Size(35, 13);
            this.labelRA_Z.TabIndex = 5;
            this.labelRA_Z.Text = "RA_Z";
            // 
            // perfChartRA_Z
            // 
            this.perfChartRA_Z.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.perfChartRA_Z.Location = new System.Drawing.Point(16, 298);
            this.perfChartRA_Z.Name = "perfChartRA_Z";
            this.perfChartRA_Z.PerfChartStyle.AntiAliasing = true;
            chartPen21.Color = System.Drawing.Color.Black;
            chartPen21.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen21.Width = 1F;
            this.perfChartRA_Z.PerfChartStyle.AvgLinePen = chartPen21;
            this.perfChartRA_Z.PerfChartStyle.BackgroundColorBottom = System.Drawing.Color.DarkGreen;
            this.perfChartRA_Z.PerfChartStyle.BackgroundColorTop = System.Drawing.Color.DarkGreen;
            chartPen22.Color = System.Drawing.Color.Black;
            chartPen22.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen22.Width = 1F;
            this.perfChartRA_Z.PerfChartStyle.ChartLinePen = chartPen22;
            chartPen23.Color = System.Drawing.Color.Black;
            chartPen23.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen23.Width = 1F;
            this.perfChartRA_Z.PerfChartStyle.HorizontalGridPen = chartPen23;
            this.perfChartRA_Z.PerfChartStyle.ShowAverageLine = true;
            this.perfChartRA_Z.PerfChartStyle.ShowHorizontalGridLines = true;
            this.perfChartRA_Z.PerfChartStyle.ShowVerticalGridLines = true;
            chartPen24.Color = System.Drawing.Color.Black;
            chartPen24.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            chartPen24.Width = 1F;
            this.perfChartRA_Z.PerfChartStyle.VerticalGridPen = chartPen24;
            this.perfChartRA_Z.ScaleMode = SpPerfChart.ScaleMode.Relative;
            this.perfChartRA_Z.Size = new System.Drawing.Size(274, 100);
            this.perfChartRA_Z.TabIndex = 4;
            this.perfChartRA_Z.TimerInterval = 100;
            this.perfChartRA_Z.TimerMode = SpPerfChart.TimerMode.Disabled;
            // 
            // labelAlpha
            // 
            this.labelAlpha.AutoSize = true;
            this.labelAlpha.Location = new System.Drawing.Point(6, 3);
            this.labelAlpha.Name = "labelAlpha";
            this.labelAlpha.Size = new System.Drawing.Size(44, 13);
            this.labelAlpha.TabIndex = 6;
            this.labelAlpha.Text = "Angle ~";
            // 
            // progressBarRisk
            // 
            this.progressBarRisk.BackColor = System.Drawing.Color.LightSalmon;
            this.progressBarRisk.Location = new System.Drawing.Point(9, 19);
            this.progressBarRisk.Name = "progressBarRisk";
            this.progressBarRisk.Size = new System.Drawing.Size(100, 23);
            this.progressBarRisk.Step = 20;
            this.progressBarRisk.TabIndex = 7;
            this.progressBarRisk.Value = 20;
            // 
            // buttonContinue
            // 
            this.buttonContinue.Enabled = false;
            this.buttonContinue.Location = new System.Drawing.Point(115, 19);
            this.buttonContinue.Name = "buttonContinue";
            this.buttonContinue.Size = new System.Drawing.Size(75, 23);
            this.buttonContinue.TabIndex = 8;
            this.buttonContinue.Text = "Continue";
            this.buttonContinue.UseVisualStyleBackColor = true;
            this.buttonContinue.Click += new System.EventHandler(this.buttonContinue_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(324, 452);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelAlpha);
            this.tabPage1.Controls.Add(this.buttonContinue);
            this.tabPage1.Controls.Add(this.progressBarRisk);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(316, 426);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Risk Level";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.perfChartRA_x);
            this.tabPage2.Controls.Add(this.labelRA_X);
            this.tabPage2.Controls.Add(this.perfChartRA_Y);
            this.tabPage2.Controls.Add(this.labelRA_Y);
            this.tabPage2.Controls.Add(this.labelRA_Z);
            this.tabPage2.Controls.Add(this.perfChartRA_Z);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(316, 426);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Details";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 474);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.Text = "Accelerometers";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPortMontura;
        private System.Windows.Forms.Timer timerReadSerial;
        private SpPerfChart.PerfChart perfChartRA_x;
        private System.Windows.Forms.Label labelRA_X;
        private System.Windows.Forms.Label labelRA_Y;
        private SpPerfChart.PerfChart perfChartRA_Y;
        private System.Windows.Forms.Label labelRA_Z;
        private SpPerfChart.PerfChart perfChartRA_Z;
        private System.Windows.Forms.Label labelAlpha;
        private System.Windows.Forms.ProgressBar progressBarRisk;
        private System.Windows.Forms.Button buttonContinue;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

