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
using System.Globalization;
using nom.tam.fits;
using System.Net.Sockets;
using System.Net;
using AtcXml;

namespace FitsMonitor
{
    public partial class Form1 : Form
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));

        /// <summary>
        /// Acceso abreviado a los settings
        /// </summary>
        private static FitsMonitor.Properties.Settings settings = FitsMonitor.Properties.Settings.Default;

        private Atc02Xml atc02Status;

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Start FitsMonitor.");
            InitializeComponent();
            WinScpTransfer.HostName = settings.Host;
            WinScpTransfer.UserName = settings.Username;
            WinScpTransfer.SshHostKey = settings.SshHostKey;            
        }

        /// <summary>
        /// Cada vez que MaximDL crea un nuevo archivo fits, este metodo captura la ruta de este archivo
        /// y comienza a procesarlo (copiarlo a zwicky).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fsWatchFits_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            Copiar(e.FullPath);
        }

        /// <summary>
        /// Copia un archivo desde baade hacia zwicky.
        /// Ademas busca el correspondiente archivo jpg para desplegarlo en el picturebox.
        /// </summary>
        /// <param name="fullPath">Ruta del archivo de origen</param>
        public void Copiar(String fullPath)
        {
            // Url del archivo Sample en el sitio de Jose Maza
            StringBuilder url;
            // Componentes del fullPath, separadas por el caracter '/'
            String[] ruta;
            // Cantidad de componentes del arreglo ruta
            int ruta_size;

            ruta = fullPath.Split(("" + Path.DirectorySeparatorChar).ToCharArray());
            ruta_size = ruta.Length;
            if (ruta_size != settings.FolderTotalDepth)
            {
                logger.Info("archivo descartado, debe estar en carpeta de profundidad " + settings.FolderTotalDepth + ".");
                return;
            }
            if (fullPath.Contains(settings.DiscardFilePattern))
            {
                logger.Info("archivo descartado, por poseer el patron " + settings.DiscardFilePattern + ".");
                return;
            }
            CompletaFitsHeader(fullPath);
            String remoteFilename;
            remoteFilename = (ruta[ruta_size - 1]);
            String localFolder;
            localFolder = ruta[ruta_size - 2];
            url = new StringBuilder();
            url.Append("http://www.das.uchile.cl/~chase500/images/jpg/");
            url.Append(localFolder.Substring(0, settings.JpgFilenameLength).Replace('p', '+'));
            url.Append(".jpg");
            //url=http://www.das.uchile.cl/~chase500/images/jpg/Images.jpg
            Console.WriteLine("url=" + url.ToString());
            this.pictureBox1.ImageLocation = url.ToString();
            this.textBoxUrl.Text = url.ToString();
            FileTransfer.WinScpTransfer.Upload(fullPath, settings.RemoteBasePath, remoteFilename);
        }

        /// <summary>
        /// Modifica el header de un archivo .fits, previo al envio 
        /// del archivo a otro servidor.
        /// </summary>
        /// <param name="fullPath"></param>
        public void CompletaFitsHeader(String fitsFullPath)
        {
            this.refreshATC02XmlStatus();
            if (!this.atc02Status.IsFresh())
            {
                logger.Info("ATC02 sin datos recientes, no se actualizara Archivo fits '"+fitsFullPath+"'");
                return;
            }
            DateTime Ahora;
            Ahora = DateTime.Now;
            Fits fitsFile;

            fitsFile = new Fits(fitsFullPath);

            BasicHDU hdu;
            hdu = fitsFile.ReadHDU();
            HeaderCard hcDATE_OBS;// [ISO 8601] UTC date/time of exposure start   
            HeaderCard hcEXPOSURE;// [sec] Duration of exposure

            hcDATE_OBS = hdu.Header.FindCard("DATE-OBS");
            hcEXPOSURE = hdu.Header.FindCard("EXPOSURE");
            DateTime dateObs;
            dateObs = DateTime.Parse(hcDATE_OBS.Value);//// [ISO 8601] UTC date/time of exposure start
            Double exposure;// [sec] Duration of exposure
            exposure = Double.Parse(hcEXPOSURE.Value);

            TimeSpan vejezFits; // Tiempo Transcurrido entre termino de exposición y el presente
            vejezFits = Ahora.Subtract(dateObs);
            double vejezSegundos;
            vejezSegundos = ((vejezFits.TotalSeconds) - exposure);
            logger.Info("Vejez archivo FITS="+vejezSegundos+"[segundos].");
            // Si la vejez del archiv Fits es menor a veinte segundos
            // entonces se modifica el encabezado del fits
            // con la información del ATC02
            if (vejezSegundos < 20)
            {
                HeaderCard hcFocStep;
                HeaderCard hcPriTemp;
                HeaderCard hcSecTemp;
                HeaderCard hcAmbTemp;
                HeaderCard hcSetFan;
                logger.Info("Actualizando archivo FITS:" + fitsFullPath);
                hcFocStep = hdu.Header.FindCard("FOCSTEP");
                hcPriTemp = hdu.Header.FindCard("PRITEMP");
                hcSecTemp = hdu.Header.FindCard("SECTEMP");
                hcAmbTemp = hdu.Header.FindCard("AMBTEMP");
                hcSetFan = hdu.Header.FindCard("SETFAN");

                hcFocStep.Value = ("" + Atc02Xml.BflToFocSetp( this.atc02Status.FocusPosition));
                hcPriTemp.Value = ("" + this.atc02Status.PrimaryTemperature);
                hcSecTemp.Value = ("" + this.atc02Status.SecondaryTemperature);
                hcAmbTemp.Value = ("" + this.atc02Status.AmbientTemperature);
                hcSetFan.Value  = ("" + this.atc02Status.FanPower);
                hdu.Header.Rewrite();
            }
            fitsFile.Close();
            // Permitimos al sistema que guarde los cambios en el archivo fits.
            System.Threading.Thread.Sleep(10000);
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
            arista = (Math.Min(formSize.Width, formSize.Height));
            this.tabControl1.Size = new Size(arista-28, arista-57);
            this.pictureBox1.Size = new Size(arista-127, arista-127);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ConfirmarClose();
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
            DialogResult result;
            StringBuilder mensaje;
            mensaje = new StringBuilder ();
            mensaje.Append("Comenzara la copia de todos los archivos bajo la carpeta:\n\n");
            mensaje.Append(tbOfflineFolder.Text);
            mensaje.Append("\n\n");
            mensaje.Append("Se buscarán archivos hasta en ");
            mensaje.Append(this.numericUpDownSubFolderDeep.Value);
            mensaje.Append(" nivel de profundidad.");
            result = MessageBox.Show(mensaje.ToString(), "Copia ssh OffLine", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.tabPage3.BackColor = Color.Yellow;
                int deep;
                logger.Debug("bCopiarOffline_Click");
                deep = (int)numericUpDownSubFolderDeep.Value;
                DirectoryInfo dInfo;
                dInfo = new DirectoryInfo(tbOfflineFolder.Text);
                ExploraCarpeta(dInfo, deep);
                this.tabPage3.BackColor = Color.LightGray;
            }
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

                fInfo = carpeta.GetFiles();

                foreach (FileInfo archivo in fInfo)
                {
                    if ((archivo.Extension == ".fits") ||
                        (archivo.Extension == ".fts"))
                    {
                        this.Copiar(archivo.FullName);
                    }
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



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bOfflineFolder_Click(object sender, EventArgs e)
        {
            DirectoryInfo directorio;

            directorio = new DirectoryInfo(this.tbOfflineFolder.Text);
            if (directorio.Exists)
            {

                folderBrowserOffline.SelectedPath = this.tbOfflineFolder.Text;
            }
            DialogResult dResult;
            dResult = folderBrowserOffline.ShowDialog();
            if (dResult == DialogResult.OK)
            {
                this.tbOfflineFolder.Text = folderBrowserOffline.SelectedPath;
            }
        }

        private void analizaATC02()
        {
            String[] fila;
            this.dataGridViewATC02.Rows.Clear();

                fila = new String[2];
                    fila[0] = "Ultima respuesta";
                    StringBuilder textoFecha;
                    textoFecha = new StringBuilder();

            
                    textoFecha.Append(this.atc02Status.Timestamp.ToLongDateString());
                    textoFecha.Append(" ");
                    textoFecha.Append(this.atc02Status.Timestamp.ToShortTimeString());

                    Console.WriteLine("timestamp=" + textoFecha.ToString());
                    fila[1] = textoFecha.ToString();
                    agregaFila(fila);

                    fila[0] = "SETFAN";
                    fila[1] = this.atc02Status.FanPower.ToString();
                    agregaFila(fila);

                    fila[0] = "PRITE";
                    fila[1] = this.atc02Status.PrimaryTemperature.ToString();
                    agregaFila(fila);

                    fila[0] = "SECTE";
                    fila[1] = this.atc02Status.SecondaryTemperature.ToString();
                    agregaFila(fila);

                    fila[0] = "BFL";
                    fila[1] = this.atc02Status.FocusPosition.ToString();
                    agregaFila(fila);

                    fila[0] = "FOCSTEP";
                    fila[1] = Atc02Xml.BflToFocSetp( this.atc02Status.FocusPosition).ToString();
                    agregaFila(fila);

                    fila[0] = "AMBTE";
                    fila[1] = this.atc02Status.AmbientTemperature.ToString();
                    agregaFila(fila);
        }

        private void agregaFila(String[] fila)
        {
            if ((fila[0] != null) && (fila[0].Length > 0))
            {
                try
                {
                    this.dataGridViewATC02.Rows.Add(fila);
                }
                catch (InvalidOperationException ioe)
                {
                    logger.Error(ioe.Message);
                    logger.Error(fila.ToString());
                }
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }

        private String EnviaMensaje(String mensaje)
        {
            String respuesta;
            respuesta = null;
            TcpClient client = new TcpClient();

            IPEndPoint serverEndPoint;
            IPAddress focusServer;
            focusServer = IPAddress.Parse(settings.FocusServer);
            
            serverEndPoint = new IPEndPoint(focusServer, settings.FocusPort);

            try
            {
                client.Connect(serverEndPoint);
            }
            catch (SocketException)
            {
                //MessageBox.Show("No se encuentra el Focus Server, revise si está en ejecución", "Error de Comunicación");
                return "Error de Comunicacion";
            }

            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(mensaje);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
            byte[] bufferIn;
            bufferIn = new byte[800];
            int bytesRead;
            bytesRead = clientStream.Read(bufferIn, 0, 800);
            respuesta = encoder.GetString(bufferIn, 0, 800);
            Console.WriteLine("-----------");
            Console.WriteLine(respuesta);
            Console.WriteLine("-----------");
            client.Close();
            return respuesta;
        }

        /// <summary>
        /// Envia el comando "GetXmlStatus" al driver ATC02 de Orbit.
        /// y actualiza la estructura de datos atc02Status
        /// </summary>
        private void refreshATC02XmlStatus()
        {
            String xmlStatus;
            xmlStatus = EnviaMensaje("GetXmlStatus");
            Console.WriteLine("xmlStatus=");
            Console.WriteLine(xmlStatus);
                this.atc02Status = new Atc02Xml(xmlStatus);

            if (this.atc02Status.IsFresh())
            {
                this.BackColor = Color.LightGreen;
                this.Text = "Fits Monitor, ATC02 ok";
            }
            else
            {
                logger.Warn("ATC02 log outdated");
                this.BackColor = Color.Pink;
                this.Text = "Fits Monitor, check ATC02 (power/driver/log)";
            }
            this.analizaATC02();
        }

        public static String LeeArchivo(String ruta)
        {
            String respuesta;
            // Read the file as one string.
            System.IO.StreamReader myFile =
               new System.IO.StreamReader(ruta);
            respuesta = myFile.ReadToEnd();

            myFile.Close();
            return respuesta;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerAtc02_Tick(object sender, EventArgs e)
        {
            this.refreshATC02XmlStatus();
        }

        private void bCorrectNames_Click(object sender, EventArgs e)
        {
            this.fsWatchFits.EnableRaisingEvents = false;
            DirectoryInfo dInfo;
            dInfo = new DirectoryInfo(tbOfflineFolder.Text);
            this.tabPage3.BackColor = Color.LightBlue;
            this.tabPage3.Refresh();
            logger.Info("bCorrectNames_Click:");
            exploraCorrectNames(dInfo);
            this.tabPage3.BackColor = Color.LightGray;
        }

        /// <summary>
        /// Explora la carpeta, en el orden requerido para que
        /// - las correcciones en nombres de archivo sean previas a.
        /// - las correcciones en nombres de carpetas.
        /// </summary>
        /// <param name="carpeta"></param>
        private void exploraCorrectNames(DirectoryInfo carpeta)
        {
            logger.Info("Explorando:"+carpeta.FullName);
            DirectoryInfo[] subCarpetas;
            FileInfo[] archivos;
            subCarpetas = carpeta.GetDirectories();
            
            archivos = carpeta.GetFiles();

            foreach (FileInfo archivoFits in archivos)
            {
                correctFits(archivoFits);
            }
            foreach (DirectoryInfo subFolder in subCarpetas)
            {
                exploraCorrectNames(subFolder); // Llamada recursiva
            }
            foreach (DirectoryInfo subFolder in subCarpetas)
            {
                correctFolderName(subFolder);
            }
        }

        private void exploraRevisaRaDec(DirectoryInfo carpeta)
        {
            //logger.Info("Explorando:" + carpeta.FullName);
            DirectoryInfo[] subCarpetas;
            FileInfo[] archivos;
            subCarpetas = carpeta.GetDirectories();

            archivos = carpeta.GetFiles();

            foreach (FileInfo archivoFits in archivos)
            {
                revisaRaDecFits(archivoFits);
            }
            foreach (DirectoryInfo subFolder in subCarpetas)
            {
                exploraRevisaRaDec(subFolder); // Llamada recursiva
            }
        }

        public static String[] EntreComillas(String texto)
        {
            char[] comilla;
            comilla = "'".ToCharArray();
            String[] part;
            part = texto.Split(comilla);
            return part;
        }

/// <summary>
/// Revisa las coordenadas de un archivo "fits". 
/// Dejando intacta la ruta (carpeta) original.
/// 
/// Avisa por pantalla:
/// - Si las coordenadas del filename no coinciden con las del fits.
/// 
/// </summary>
/// <param name="archivo"></param>
private void revisaRaDecFits(FileInfo archivo)
{
    //logger.Info("correctFits:archivo=" + archivo.Name);
    String ruta;
    String shortFileName;
    String extension;
    String fullFilename;

    ruta = archivo.DirectoryName;
    shortFileName = archivo.Name;
    extension = archivo.Extension;
    fullFilename = archivo.FullName;

        ///////////////
        if ((extension == ".fts") || (extension == ".fits"))
        {
            Fits fitsFile;
            String strObject;
            String strObjectRA;
            String strObjectDEC;
            fitsFile = new Fits(fullFilename);
            BasicHDU hdu;
            hdu = fitsFile.ReadHDU();
            HeaderCard hcOBJECT;// Target object name  <---- Asi esta comentado en MaximDL
            HeaderCard hcOBJCTRA;//  [hms J2000] Target right ascension  <---- Asi esta comentado en MaximDL
            HeaderCard hcOBJCTDEC;//  [dms +N J2000] Target declination  <---- Asi esta comentado en MaximDL
            hcOBJECT = hdu.Header.FindCard("OBJECT");//OBJECT  	= 'F1910178-615834'
            hcOBJCTRA = hdu.Header.FindCard("OBJCTRA");//OBJCTRA 	= '19 10 17.80' 
            hcOBJCTDEC = hdu.Header.FindCard("OBJCTDEC");//OBJCTDEC	= '-61 58 34.0'
            strObject = hcOBJECT.Value;
            strObjectRA = null;
            strObjectDEC = null;
            if (hcOBJCTRA != null) { strObjectRA = hcOBJCTRA.Value; }
            if (hcOBJCTDEC != null) { strObjectDEC = hcOBJCTDEC.Value; }
            fitsFile.Close();
            if ((strObjectRA != null) && (strObject.StartsWith("F")))
            {
                StringBuilder radec;
                //strObjectRA = EntreComillas(strObjectRA)[1];
                radec = new StringBuilder();
                radec.Append("F");
                radec.Append(strObjectRA.Replace(" ", "").Replace(".","").Substring(0, 7));
                //if (!strObjectDEC.StartsWith("-"))
                //{
                //    radec.Append("p");
                //}
                radec.Append(strObjectDEC.Replace(" ", "").Replace(".", "").Replace("+", "p").Substring(0, 7));
                StringBuilder comparacion;
                comparacion = new StringBuilder();
                comparacion.Append(strObject);
                comparacion.Append("==");
                comparacion.Append(radec);
                comparacion.Append(" --> ");
                Boolean match;
                match = strObject.Equals(radec.ToString());
                comparacion.Append(match);
                Console.WriteLine(comparacion.ToString());
                if (!match)
                {
                    StringBuilder mensaje;
                    mensaje = new StringBuilder();
                    mensaje.Append(comparacion.ToString());
                    mensaje.Append("\trm\t");
                    mensaje.Append(shortFileName);
                    mensaje.Append("\t");
                    mensaje.Append(fullFilename);
                    Console.WriteLine(mensaje.ToString());
                }
            }
        }

        ///////////////    
}


        /// <summary>
        /// Corrige el nombre un archivo "fits". 
        /// Dejando intacta la ruta (carpeta) original.
        /// 
        /// Además si es un archivo fits, corrige el contenido del campo
        /// object en la misma lógica que la corrección del nombre del archivo.
        /// 
        /// Primero se corrige el contenido del archivo y luego el nombre.
        /// </summary>
        /// <param name="archivo"></param>
        private void correctFits(FileInfo archivo)
        {
            logger.Info("correctFits:archivo=" + archivo.Name);
            String ruta;
            String oldFileName;
            String newFileName;
            String extension;
            String oldFullFilename;
            String newFullFilename;

            Boolean requiereRename; //True solo si se detecta que es pertinente un rename
            requiereRename = false;

            ruta = archivo.DirectoryName;
            oldFileName = archivo.Name;
            extension = archivo.Extension;
            newFileName = archivo.Name;
            oldFullFilename = archivo.FullName;
            
            if (extension == ".fts")
            {
                requiereRename = true;
                logger.Info(";.fts->.fits");
                newFileName = newFileName.Replace(".fts", ".fits");
            }
            if (oldFileName.Contains("+"))
            {
                requiereRename = true;
                logger.Info(";'+'->'p'");
                newFileName = newFileName.Replace("+", "p");
                ///////////////
                if ((extension == ".fts") || (extension == ".fits"))
                {
                    Fits fitsFile;
                    String strObject; // El string que hay que corregir.
                    String strCorrectedObject;
                    fitsFile = new Fits(oldFullFilename);
                    BasicHDU hdu;
                    hdu = fitsFile.ReadHDU();
                    HeaderCard hcOBJECT;// Target object name  <---- Asi esta comentado en MaximDL
                    hcOBJECT = hdu.Header.FindCard("OBJECT");
                    strObject = hcOBJECT.Value;
                    if (strObject.Contains("+"))
                    {
                        strCorrectedObject = strObject.Replace("+", "p");
                        hcOBJECT.Value = strCorrectedObject;
                    }
                    hdu.Header.Rewrite();
                    fitsFile.Close();
                }

                ///////////////
            }
            if (requiereRename)
            {

                newFullFilename = ruta + Path.DirectorySeparatorChar + newFileName;

                logger.Info(";"+oldFullFilename+"->"+newFullFilename);
                try
                {
                    archivo.MoveTo(newFullFilename);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }
            }

            //Console.WriteLine("#");
        }

        /// <summary>
        /// Corrige el nombre el último tramo de una carpeta.
        /// Dejando intacta la ruta padre de la carpeta.
        /// </summary>
        /// <param name="carpeta"></param>
        private void correctFolderName(DirectoryInfo carpeta)
        {
            String ruta;
            String nombreCarpeta;
            String nuevoNombreCarpeta;
            String oldFolderName;
            String newFolderName;
            oldFolderName = carpeta.FullName;
            ruta = carpeta.Parent.FullName;
            nombreCarpeta = carpeta.Name;

            logger.Info("correctFolderName:" + oldFolderName);
            if (carpeta.Name.Contains("+"))
            {
                logger.Info(";'+'->'p'");
                nuevoNombreCarpeta = nombreCarpeta.Replace("+", "p");
                newFolderName = ruta + Path.DirectorySeparatorChar + nuevoNombreCarpeta;
                logger.Info(";" + oldFolderName + "->" + newFolderName);
                try
                {
                    carpeta.MoveTo(newFolderName);
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }
            }
            
            //if (carpeta.Name.Contains("_"))
            //{
            //    Console.Write(";'_'->''");
            //}
            //Console.WriteLine("#");
        }


        private void bCheckRaDec_Click(object sender, EventArgs e)
        {
            this.fsWatchFits.EnableRaisingEvents = false;
            DirectoryInfo dInfo;
            dInfo = new DirectoryInfo(tbOfflineFolder.Text);
            this.tabPage3.BackColor = Color.LightBlue;
            this.tabPage3.Refresh();
            logger.Info("bCheckRaDec_Click:");
            exploraRevisaRaDec(dInfo);
            this.tabPage3.BackColor = Color.LightGray;
        }

        private void bReadStatus_Click(object sender, EventArgs e)
        {
            this.refreshATC02XmlStatus();
        }
    }
}
