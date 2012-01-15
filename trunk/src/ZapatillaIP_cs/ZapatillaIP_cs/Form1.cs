using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ZapatillaIP_cs
{
    public partial class Form1 : Form
    {
        TcpClient tcpclnt;

        public Form1()
        {
            InitializeComponent();
            tcpclnt = new TcpClient();
            tcpclnt.Connect("139.229.65.214", 18008);
            Console.WriteLine("Connected");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("button1_Click");
            
            Console.WriteLine("Connecting.....");

            
            // use the ipaddress as in the server program


            
            //Console.Write("Enter the string to be transmitted : ");

            String str = Console.ReadLine();
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            //byte[] ba = asen.GetBytes(str);
            Console.WriteLine("Transmitting.....");

            //stm.Write(ba, 0, ba.Length);
            stm.WriteByte(49);
            Thread.Sleep(1000);
            stm.WriteByte(50);
/**
            byte[] bb = new byte[100];
            int k = stm.Read(bb, 0, 100);

            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(bb[i]));
            */
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            tcpclnt.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }
    }
}