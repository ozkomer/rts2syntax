using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Collections;


namespace Chase500DB
{
    /// <summary>
    /// Codigo generado con los siguientes comandos de consola:
    /// cd C:\Users\chase\Documents\emaureir\src\Chase500DB\Chase500DB
    /// "C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin\xsd.exe"  /classes RTML-2.3.xsd
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bCrearDoc_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int projectID = (int) this.cbxProject.SelectedValue;
            Console.WriteLine("projectID=" + projectID);
            String fileName = (this.cbxProject.GetItemText(this.cbxProject.SelectedItem).Replace(" ",String.Empty)+".xml");
            Console.WriteLine("fileName=" + fileName);

            StringBuilder archivoSalida;
            archivoSalida = new StringBuilder();
            archivoSalida.Append(@"C:\Users\Administrator\Desktop\rtml2.3\");
            archivoSalida.Append(fileName);

            Console.WriteLine("archivoSalida=" + archivoSalida.ToString());
            DbToRTML23 dbToRTML;

            dbToRTML = new DbToRTML23(projectID, archivoSalida.ToString());
            dbToRTML.FillRTML();
            dbToRTML.saveRTML();
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Archivo RTML creado","DB to RTML2.3");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'chase500DataSet.projects' table. You can move, or remove it, as needed.
            this.projectsTableAdapter.Fill(this.chase500DataSet.projects);

        }
    }
}
