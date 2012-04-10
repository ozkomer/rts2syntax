using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Atc02
{
    public partial class Form1 : Form
    {
        //                                     0123456789
        public readonly String OPENREM      = "OPENREM   ";
        public readonly String CLOSEREM     = "CLOSEREM  ";
        public readonly String READSETT     = "READSETT  ";
        public readonly String UPDATEPC     = "UPDATEPC  ";
        public readonly String SETFAN       = "SETFAN ";
        public readonly String SETBFL       = "BFL ";
        public readonly String FINDOPTIMA   = "FINDOPTIMA";
        
        
        public Form1()
        {
            InitializeComponent();
        }

        private void bConnect_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                return;
            }
            Console.WriteLine("Conectando.");
            try
            {
                serialPort1.Open();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Puerta Serial en uso.");
            }
            
            if (serialPort1.IsOpen)
            {
                Console.WriteLine("Conectado.");
                groupBox1.Enabled = true;
                bDisconnect.Enabled = true;
                bConnect.Enabled = false;
            }
        }

        private void bDisconnect_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                return;
            }
            Console.WriteLine("Desconectando.");
            serialPort1.Close();
            if (!serialPort1.IsOpen)
            {
                Console.WriteLine("Desconectado.");
                groupBox1.Enabled = false;
                bConnect.Enabled = true;
                bDisconnect.Enabled = false;
            }
        }

        private void bOpenRem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("->" + OPENREM);
            serialPort1.Write(OPENREM);
            LeerSerial(10);
        }

        private void bReadSet_Click(object sender, EventArgs e)
        {
            Console.WriteLine("->" + READSETT);
            serialPort1.Write(READSETT);
            LeerSerial(130);
        }

        private void bUpdatePC_Click(object sender, EventArgs e)
        {
            Console.WriteLine("->" + UPDATEPC);
            serialPort1.Write(UPDATEPC);
            LeerSerial(100);
        }

        private void LeerSerial(int cantCaracteres)
        {
            Console.WriteLine("<---LeerSerial--->");
            Console.Write("Esperando respuesta ");

            while (serialPort1.BytesToRead < cantCaracteres)
            {
                Console.Write(".");
                System.Threading.Thread.Sleep(200);
            }
            Console.WriteLine(".");
            System.Threading.Thread.Sleep(200);
            Console.WriteLine(serialPort1.ReadExisting());
            Console.WriteLine("<---LeerSerial--->");
        }

        private void bCloseRemote_Click(object sender, EventArgs e)
        {
            Console.WriteLine("->" + CLOSEREM);
            serialPort1.Write(CLOSEREM);
            LeerSerial(10);
        }

        private void bSetFan_Click(object sender, EventArgs e)
        {
            StringBuilder comandoFan;
            comandoFan = new StringBuilder();
            comandoFan.Append(SETFAN);
            comandoFan.Append(trackBarFan.Value.ToString("000"));
            Console.WriteLine("->" + comandoFan);
            serialPort1.Write(comandoFan.ToString());
            LeerSerial(10);
        }

        private void trackBarFan_Scroll(object sender, EventArgs e)
        {
            StringBuilder leyendaFan;
            leyendaFan = new StringBuilder();
            leyendaFan.Append("Set Fan:");
            leyendaFan.Append(trackBarFan.Value);
            bSetFan.Text = leyendaFan.ToString();
            //Console.WriteLine(comandoFan + " " + comandoFan.Length);
        }

        private void bSetBFL_Click(object sender, EventArgs e)
        {
            StringBuilder comandoBFL;
            comandoBFL = new StringBuilder();
            comandoBFL.Append(SETBFL);
            comandoBFL.Append(nudBFL.Value.ToString("000.00"));
            Console.WriteLine("->" + comandoBFL);
            serialPort1.Write(comandoBFL.ToString());
            LeerSerial(10);
        }

        private void nudBFL_ValueChanged(object sender, EventArgs e)
        {
            StringBuilder leyendaBFL;
            leyendaBFL = new StringBuilder();
            leyendaBFL.Append("Set BFL:");
            //leyendaBFL.Append(SETBFL);
            leyendaBFL.Append(nudBFL.Value.ToString("000.00"));
            bSetBFL.Text = leyendaBFL.ToString();
            Console.WriteLine(leyendaBFL + " " + leyendaBFL.Length);
        }

        private void bFindOptimal_Click(object sender, EventArgs e)
        {
            Console.WriteLine("->" + FINDOPTIMA);
            serialPort1.Write(FINDOPTIMA);
            LeerSerial(10);
        }
    }
}
