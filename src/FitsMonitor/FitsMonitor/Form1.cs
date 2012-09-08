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
using ASCOM.DriverAccess;

namespace FitsMonitor
{
    public partial class Form1 : Form
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));

        /// <summary>
        /// Acceso abreviado a los settings
        /// </summary>
        private static FitsMonitor.Properties.Settings settings = FitsMonitor.Properties.Settings.Default;

        //private DateTime lastResponseTimeStamp;

        //private double bfl;
        //private int focstep;
        //private double priTemp;
        //private double secTemp;
        //private double ambTemp;
        //private int setFan;
        Atc02Xml atc02Status;

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Start FitsMonitor.");
            InitializeComponent();
            WinScpTransfer.HostName = settings.Host;
            WinScpTransfer.UserName = settings.Username;
            WinScpTransfer.SshHostKey = settings.SshHostKey;            
            //this.lastResponseTimeStamp = DateTime.MinValue;
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
                logger.Warn("archivo descartado, debe estar en carpeta de profundidad " + settings.FolderTotalDepth + ".");
                return;
            }
            if (fullPath.Contains(settings.DiscardFilePattern))
            {
                Console.WriteLine("archivo descartado, por poseer el patron " + settings.DiscardFilePattern + ".");
                return;
            }
            CompletaFitsHeader(fullPath);
            //StringBuilder remotePath;
            //String directorioRemoto;
            String remoteFilename;
            //remotePath = new StringBuilder();

            //remotePath.Append();
            //archivoRemoto.Append("/");
            //String fecha;
            //fecha = ruta[6];
            //remotePath.Append(fecha);
            //directorioRemoto = remotePath.ToString();
            //remotePath.Append("/");
            remoteFilename = (ruta[ruta_size - 1]);//.Replace(".fts", ".fits"));
            //Console.WriteLine("--->" + remotePath.ToString());

            url = new StringBuilder();
            url.Append("http://www.das.uchile.cl/~chase500/images/jpg/");
            url.Append(remoteFilename.Substring(0, settings.JpgFilenameLength).Replace('p','+'));
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
            if (!LecturaFresca())
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

        ///// <summary>
        ///// Metodo en Hebra.
        ///// Codigo Modificado a partir de un ejemplo de "tail" hallado en:
        ///// http://www.codeproject.com/Articles/5854/Tail-utility-for-windows
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void backgroundWorkerATC02_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    String fileName;
        //    StringBuilder respuesta;
        //    respuesta = new StringBuilder();
        //    fileName = e.Argument as String;
            
        //    ///while (true)
        //   //{
        //    Boolean salir;
        //    salir = false;
        //        try
        //        {
        //            using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
        //            {
        //                //start at the end of the file
        //                long lastMaxOffset = reader.BaseStream.Length;

        //                while (!salir)
        //                {
        //                    System.Threading.Thread.Sleep(100);
        //                    if (backgroundWorkerATC02.CancellationPending)
        //                    {
        //                        salir = true;
        //                        reader.Close();
        //                        break;
        //                    }
        //                    //if the file size has not changed, idle
        //                    if (reader.BaseStream.Length == lastMaxOffset)
        //                    {
        //                        //e.Result = respuesta.ToString();
        //                        continue;
        //                    }

        //                    //seek to the last max offset
        //                    reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

        //                    //read out of the file until the EOF
        //                    string line = "";
        //                    while ((line = reader.ReadLine()) != null)
        //                    {
                                
        //                        if (!(respuesta.ToString().Contains(line)))
        //                        {
        //                            //Console.WriteLine(line);
        //                            respuesta.AppendLine(line);
        //                        }
        //                        // Capturado un cambio en el archivo, lo reportamos
        //                        if (respuesta.ToString().Contains("DEWPO"))
        //                        {
        //                            e.Result = respuesta.ToString();
        //                            salir = true;
        //                            reader.Close();
        //                            break;
        //                        }
        //                    }
        //                    //update the last max offset
        //                    //lastMaxOffset = reader.BaseStream.Position;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    //}
        //}

        //private void backgroundWorkerATC02_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{

        //}

        //private void backgroundWorkerATC02_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Console.WriteLine("<backgroundWorkerATC02_RunWorkerCompleted>");
        //    String atc02Log;
        //    atc02Log = (String)e.Result;
        //    Console.WriteLine(atc02Log);
        //    analizaATC02(atc02Log);
        //    Console.WriteLine("</backgroundWorkerATC02_RunWorkerCompleted>");
        //    while (this.backgroundWorkerATC02.IsBusy)
        //    {
        //        System.Threading.Thread.Sleep(500);
        //        Console.Write(".");
        //    }
        //    Console.WriteLine("$");
        //    if (!this.backgroundWorkerATC02.IsBusy)
        //    {
        //        logger.Info("Re-Iniciando monitoreo en archivo:" + settings.AtcLogFile);
        //        this.backgroundWorkerATC02.RunWorkerAsync(settings.AtcLogFile);
        //    }
        //}

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


        /// <summary>
        /// Revisa si la última fecha leida por el ATC02 es cercana
        /// a la fecha actual de el sistema.
        /// 
        /// Si la fecha es cercana, este metodo permitira ingresar la informacion
        /// leida del Officina Stellare ATC02 a los archivos .fits
        /// </summary>
        /// <returns></returns>
        private Boolean LecturaFresca()
        {
            Boolean respuesta;
            respuesta = false;
            TimeSpan vejezLectura; // Diferencia de tiempo entre el presente y la ultima lectura del ATC02
            vejezLectura = DateTime.Now.Subtract(this.atc02Status.Timestamp);
            if (vejezLectura.TotalSeconds < 120)
            {
                respuesta = true;
            }
            else
            {
                logger.Warn("Ultima lectura, desfasada c/r al presente por " + vejezLectura.TotalSeconds + " segundos.");
            }
            return respuesta;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.MostrarVentana();
        }

        /// <summary>
        /// Envia el comando "GetXmlStatus" al driver ATC02 de Orbit.
        /// y actualiza la estructura de datos atc02Status
        /// </summary>
        private void refreshATC02XmlStatus()
        {
            String xmlStatus;
            xmlStatus = LeeArchivo(settings.Atc02StatusXmlFilePath);
            Console.WriteLine("xmlStatus=");
            Console.WriteLine(xmlStatus);
            this.atc02Status = new Atc02Xml(xmlStatus);
            if (LecturaFresca())
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
            //this.refreshATC02XmlStatus();
            //if (LecturaFresca())
            //{
            //    this.BackColor = Color.LightGreen;
            //    this.Text = "Fits Monitor, ATC02 ok";
            //}
            //else
            //{
            //    logger.Warn("ATC02 log outdated");
            //    this.BackColor = Color.Pink;
            //    this.Text = "Fits Monitor, check ATC02 (power/driver/log)";
            //}
            //this.analizaATC02();
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

        //private void fsWatchOfficinaStelare_Created(object sender, FileSystemEventArgs e)
        //{
        //    fsVigilaOfficinaStelare(e);
        //}

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

        private void fileSystemWatcherAtc02XML_Changed(object sender, FileSystemEventArgs e)
        {
            this.refreshATC02XmlStatus();
        }



    }
}
