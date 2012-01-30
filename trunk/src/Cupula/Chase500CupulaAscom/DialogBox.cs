using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chase500CupulaAscom
{
    public partial class DialogBox : Form
    {
        private Chase500Ascom chas;
        public DialogBox(Chase500Ascom chas)
        {
            InitializeComponent();
            this.chas = chas;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.chas.CommandString("HOST " + this.textBoxHost.Text,true);
            this.chas.CommandString("PORT " + this.numericUpDown1.Value, true);
        }
    }
}