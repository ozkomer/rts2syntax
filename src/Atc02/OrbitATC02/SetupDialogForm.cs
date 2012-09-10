using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ASCOM.OrbitATC02
{
    public partial class SetupDialogForm : Form
    {
        private static Properties.Settings settings = Properties.Settings.Default;
        public SetupDialogForm()
        {
            InitializeComponent();
            this.Text = settings.DriverName;
            this.tbStepSize.Text = ""+settings.StepSize;
        }

        private void CmdOkClick(object sender, EventArgs e)
        {

            settings.StepSize = Double.Parse(tbStepSize.Text);
            settings.StartUpSecondaryPosition = this.nudLastSecondary.Value;
            settings.StartUpSecondary = this.cbSecondaryPositionStartUp.Checked;
            settings.FocusServer = this.tbFocusServer.Text;
            settings.FocusPort = (int) this.nufFocusPort.Value;
            settings.Save();
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
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
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
            this.tbStepSize.Text = ""+Properties.Settings.Default.StepSize;
        }

        private void cbxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbRefreshStatus_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}