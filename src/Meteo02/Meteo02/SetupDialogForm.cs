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
            this.textBoxMaxHumidity.Text = ""+Properties.Settings.Default.maxHumidity;
            this.textBoxMaxWindSpeed.Text = "" + Properties.Settings.Default.maxWindSpeed_inKnots;
            this.textBoxMinDewPoint.Text = "" + (100 * Properties.Settings.Default.minDewPointDelta);
        }
    }
}