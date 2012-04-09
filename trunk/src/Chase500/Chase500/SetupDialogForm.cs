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
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            Properties.Settings.Default.Host = this.tbHost.Text;
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
    }
}