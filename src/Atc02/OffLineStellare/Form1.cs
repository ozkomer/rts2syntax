using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libOfflineLog;

namespace OffLineStellare
{
    public partial class Form1 : Form
    {
        OfficinaStellareLog officinaStellareLog;

        public Form1()
        {
            InitializeComponent();
            this.officinaStellareLog = null;
        }

        private void bOpen_Click(object sender, EventArgs e)
        {           
             this.openFileDialog1.ShowDialog(this);
             String archivo;
             archivo = this.openFileDialog1.FileName;
             this.officinaStellareLog = new OfficinaStellareLog(archivo);
             this.officinaStellareLog.analizar();
             Console.WriteLine(this.officinaStellareLog.ToString());
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
