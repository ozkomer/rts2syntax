using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FileTransfer;
using log4net.Config;
using log4net;

namespace FitsMonitor
{
    public partial class Form1 : Form
    {
        private static FitsMonitor.Properties.Settings settings = FitsMonitor.Properties.Settings.Default;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Start FitsMonitor.");
            InitializeComponent();
            WinScpTransfer.HostName = settings.Host;
            WinScpTransfer.UserName = settings.Username;
            WinScpTransfer.SshHostKey = settings.SshHostKey;
        }

        private void fsWatchFits_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            Copiar(e.FullPath);
        }

        /// <summary>
        /// Copia un archivo desde baade hacia zwicky.
        /// </summary>
        /// <param name="fullPath">Ruta del archivo de origen</param>
        public void Copiar(String fullPath)
        {
            // Url del archivo en el sitio de Jose Maza
            StringBuilder url;
            // Componentes del fullPath, separadas por el caracter '/'
            String[] ruta;
            // Cantidad de componentes del arreglo ruta
            int ruta_size;

            ruta = fullPath.Split(("" + Path.DirectorySeparatorChar).ToCharArray());
            ruta_size = ruta.Length;
            if (ruta_size != settings.FolderTotalDepth)
            {
                logger.Warn("archivo descartado, debe estar en carpeta de profundidad " + settings.FolderTotalDepth + ".");
                return;
            }
            if (fullPath.Contains(settings.DiscardFilePattern)) 
            {
                Console.WriteLine("archivo descartado, por poseer el patron " + settings.DiscardFilePattern + ".");
                return;
            }
            StringBuilder remotePath;
            String directorioRemoto;
            String remoteFilename;
            remotePath = new StringBuilder();

            remotePath.Append( settings.RemoteBasePath);
            //archivoRemoto.Append("/");
            String fecha;
            fecha = ruta[6];
            remotePath.Append(fecha);
            directorioRemoto = remotePath.ToString();
            //remotePath.Append("/");
            remoteFilename = (ruta[ruta_size-1].Replace(".fts", ".fits"));
            Console.WriteLine("--->" + remotePath.ToString());

            url = new StringBuilder();
            url.Append("http://www.das.uchile.cl/~chase500/images/jpg/");
            url.Append(remoteFilename.Substring(4, 15));
            url.Append(".jpg");
            //url=http://www.das.uchile.cl/~chase500/images/jpg/Images.jpg
            Console.WriteLine("url=" + url.ToString());
            this.pictureBox1.ImageLocation = url.ToString();
            this.textBoxUrl.Text = url.ToString();
            FileTransfer.WinScpTransfer.Upload(fullPath, directorioRemoto, remoteFilename);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.Hide();
                return;
            }
            Size formSize;
            formSize = this.Size;
            int arista;
            arista = (Math.Min( formSize.Width,formSize.Height ) - 38);
            this.pictureBox1.Size = new Size(arista, arista);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ConfirmarClose();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.MostrarVentana();
        }

        /// <summary>
        /// Muestra la ventana de esta aplicacion.
        /// </summary>
        private void MostrarVentana()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private DialogResult ConfirmarClose()
        {
            DialogResult respuesta;
            respuesta = MessageBox.Show("Closing application. Continue?", "Fits Monitor.", MessageBoxButtons.YesNo);
            if (respuesta == DialogResult.Yes)
            {
                Application.Exit();
            }
            return respuesta;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                return;
            }
            DialogResult respuesta;
            respuesta = this.ConfirmarClose();
            if (respuesta == DialogResult.No)
            {

                e.Cancel = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void bCopiarOffline_Click(object sender, EventArgs e)
        {
            logger.Debug("bCopiarOffline_Click");
            DirectoryInfo dInfo;
            dInfo = new DirectoryInfo(settings.WatchFolder);
            ExploraCarpeta(dInfo, 0);
        }

        /// <summary>
        /// Metodo recursivo
        /// </summary>
        /// <param name="carpeta"></param>
        /// <param name="profundidad"></param>
        private void ExploraCarpeta(DirectoryInfo carpeta, int profundidad)
        {
            if (profundidad == 2)
            {
                FileInfo[] fInfo;
                
                fInfo = carpeta.GetFiles("*.fts");

                foreach (FileInfo archivo in fInfo)
                {
                    this.Copiar(archivo.FullName);
                    //logger.Debug(archivo.FullName);
                }
                return;
            }
            DirectoryInfo[] subcarpetas;
            subcarpetas = carpeta.GetDirectories();
            foreach (DirectoryInfo dInfo in subcarpetas)
            {
                if (profundidad < 2)
                {
                    ExploraCarpeta(dInfo, (profundidad + 1));
                }
            }
        }

    }
}
