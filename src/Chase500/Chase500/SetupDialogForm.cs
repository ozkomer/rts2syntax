using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;

namespace ASCOM.Chase500
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            this.cbxNorth.SelectedIndex = Properties.Settings.Default.NorthOpen;
            this.cbxSouth.SelectedIndex = Properties.Settings.Default.SouthOpen;
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            Properties.Settings.Default.Host = this.tbHost.Text;
            Properties.Settings.Default.Port = this.nudPort.Value;
            Properties.Settings.Default.NorthOpen = (ushort) this.cbxNorth.SelectedIndex;
            Properties.Settings.Default.SouthOpen = (ushort) this.cbxSouth.SelectedIndex;

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

        private void tbHost_TextChanged(object sender, EventArgs e)
        {
            FeedbackModificacion();
        }

        private void FeedbackModificacion()
        {
            StringBuilder sb;
            sb = new StringBuilder();
            sb.Append ("(*) ");
            sb.Append (Properties.Settings.Default.DriverName);
            this.Text = sb.ToString();
        }

        private void nudPort_ValueChanged(object sender, EventArgs e)
        {
            FeedbackModificacion();
        }

        private void cbxNorth_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeedbackModificacion();
        }

        private void cbxSouth_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeedbackModificacion();
        }
    }
}