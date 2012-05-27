using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ASCOM.OrbitATC02;

namespace DriverTest
{
    public partial class Form1 : Form
    {
        Focuser focus;
        public Form1()
        {
            InitializeComponent();
            focus = new Focuser();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            focus.Connected = true;
            Console.WriteLine("focus.Connected=" + focus.Connected);
        }

        private void bDisconnect_Click(object sender, EventArgs e)
        {
            focus.Connected = false;
            Console.WriteLine("focus.Connected=" + focus.Connected);
        }

        private void bSetPosition_Click(object sender, EventArgs e)
        {

            bSetPosition.Text = "Position=" + focus.Position;
            focus.Move( Int32.Parse(tbPosition.Text));
            while(focus.IsMoving)
            {

                System.Threading.Thread.Sleep(200);
            }
            bSetPosition.Text = "Position=" + focus.Position;
        }

    }
}
