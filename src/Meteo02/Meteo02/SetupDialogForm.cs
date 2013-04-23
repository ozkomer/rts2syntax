using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;


namespace ASCOM.Meteo02
{
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            if (this.rbWeatherDB.Checked)
            {
                Properties.Settings.Default.safeUnsafeLogic = 0;
            }
            if (this.rbPromptWebSite.Checked)
            {
                Properties.Settings.Default.safeUnsafeLogic = 1;
            }
            Properties.Settings.Default.Save();
            Dispose();
        }

        private void CmdCancelClick(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BrowseToAscom(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            this.textBoxMaxHumidity.Text = ""+ (100 * Properties.Settings.Default.maxHumidity)+" %";
            this.textBoxMaxWindSpeed.Text = "" + Properties.Settings.Default.maxWindSpeed_inKnots;
            this.textBoxMinDewPoint.Text = "" + Properties.Settings.Default.minDewPointDelta;
            switch (Properties.Settings.Default.safeUnsafeLogic)
            {
                case 0:
                    this.rbWeatherDB.Checked=true;
                    break;
                case 1:
                    this.rbPromptWebSite.Checked = true;
                    break;
            }
        }
    }
}