using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ASCOM.OrbitATC02
{
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            this.Text = Properties.Settings.Default.DriverName;
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            Properties.Settings.Default.CommPort = tbCommPort.Text;
            Properties.Settings.Default.BaudRate = nudBaudRate.Value;
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

        }

        private void cbxBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}