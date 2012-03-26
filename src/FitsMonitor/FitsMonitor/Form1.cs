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
            Copiar(e);
        }

        public void Copiar(FileSystemEventArgs e)
        {
            String fullPath;
            StringBuilder url;
            String[] ruta;
            int ruta_size;
            fullPath = e.FullPath;
            ruta = fullPath.Split(("" + Path.DirectorySeparatorChar).ToCharArray());
            ruta_size = ruta.Length;
            if ((!(fullPath.Contains("RAW"))) ||
                    (ruta_size != 9)
                )
            {
                Console.WriteLine("archivo no es un RAW fits, skipping");
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
            /*
            #region  Abrir sesion
            wscpSession = new Session();
            SessionOptions sesionOptions;
            sesionOptions = new SessionOptions();

            sesionOptions.HostName = tbHost.Text;
            sesionOptions.UserName = tbUser.Text;
            //sesionOptions.Password = tbPassword.Text;
            sesionOptions.Protocol = Protocol.Sftp;
            sesionOptions.PortNumber = 22;
            sesionOptions.SshHostKey = tbSshHostKey.Text;
            wscpSession.Open(sesionOptions);
            #endregion

            WinSCP.RemoteDirectoryInfo remoteDirInfo;
            if (wscpSession.Opened)
            {
                Console.WriteLine("Sesion Iniciada.");
                remoteDirInfo = null;
                try
                {
                    remoteDirInfo = wscpSession.ListDirectory(directorioRemoto);
                }
                catch (WinSCP.SessionRemoteException sre)
                {
                    Console.WriteLine("sre=" + sre.Message);
                }
                if ((remoteDirInfo == null) ||
                     (remoteDirInfo.Files == null) ||
                     (remoteDirInfo.Files.Count == 0))
                {
                    wscpSession.ExecuteCommand("mkdir " + directorioRemoto);
                }

                TransferOptions transferOptions;
                transferOptions = new TransferOptions();
                transferOptions.PreserveTimestamp = true;
                transferOptions.TransferMode = TransferMode.Binary;
                TransferOperationResult transferResult;
                transferResult = wscpSession.PutFiles(fullPath, archivoRemoto.ToString(), false, transferOptions);
                Console.WriteLine("transferResult.IsSuccess=" + transferResult.IsSuccess);
                foreach (OperationResultBase orb in transferResult.Failures)
                {
                    Console.WriteLine("error-->" + orb.ToString());
                }
                wscpSession.Dispose();
                Console.WriteLine("Sesion Finalizada.");
            }
             */ 
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

        private void fsWatchFits_Changed(object sender, FileSystemEventArgs e)
        {

        }
    }
}
