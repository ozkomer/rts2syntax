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

namespace FitsMonitor
{
    public partial class Form1 : Form
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(Form1));
        
        /// <summary>
        /// Acceso abreviado a los settings
        /// </summary>
        private static FitsMonitor.Properties.Settings settings = FitsMonitor.Properties.Settings.Default;

        private DateTime lastResponseTimeStamp;

        private double bfl;
        private int focstep;
        private double priTemp;
        private double secTemp;
        private double ambTemp;
        private int setFan;

        public Form1()
        {
            XmlConfigurator.Configure();
            logger.Info("Start FitsMonitor.");
            InitializeComponent();
            WinScpTransfer.HostName = settings.Host;
            WinScpTransfer.UserName = settings.Username;
            WinScpTransfer.SshHostKey = settings.SshHostKey;            
            this.fsWatchOfficinaStelare.NotifyFilter = (NotifyFilters.Size | NotifyFilters.CreationTime);
            if (!this.backgroundWorkerATC02.IsBusy)
            {
                logger.Info("Iniciando monitoreo en archivo:" + settings.AtcLogFile);
                this.backgroundWorkerATC02.RunWorkerAsync(settings.AtcLogFile);
            }
            this.lastResponseTimeStamp = DateTime.MinValue;
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
            StringBuilder remotePath;
            String directorioRemoto;
            String remoteFilename;
            remotePath = new StringBuilder();

            remotePath.Append(settings.RemoteBasePath);
            //archivoRemoto.Append("/");
            String fecha;
            fecha = ruta[6];
            remotePath.Append(fecha);
            directorioRemoto = remotePath.ToString();
            //remotePath.Append("/");
            remoteFilename = (ruta[ruta_size - 1].Replace(".fts", ".fits"));
            Console.WriteLine("--->" + remotePath.ToString());

            url = new StringBuilder();
            url.Append("http://www.das.uchile.cl/~chase500/images/jpg/");
            url.Append(remoteFilename.Substring(0, settings.JpgFilenameLength));
            url.Append(".jpg");
            //url=http://www.das.uchile.cl/~chase500/images/jpg/Images.jpg
            Console.WriteLine("url=" + url.ToString());
            this.pictureBox1.ImageLocation = url.ToString();
            this.textBoxUrl.Text = url.ToString();
            FileTransfer.WinScpTransfer.Upload(fullPath, directorioRemoto, remoteFilename);
        }

        /// <summary>
        /// Modifica el header de un archivo .fits, previo al envio 
        /// del archivo a otro servidor.
        /// </summary>
        /// <param name="fullPath"></param>
        public void CompletaFitsHeader(String fitsFullPath)
        {
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

                hcFocStep.Value = ("" + this.focstep);
                hcPriTemp.Value = ("" + this.priTemp);
                hcSecTemp.Value = ("" + this.secTemp);
                hcAmbTemp.Value = ("" + this.ambTemp);
                hcSetFan.Value  = ("" + this.setFan);
                hdu.Header.Rewrite();
            }
            fitsFile.Close();
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

        /// <summary>
        /// Este metodo se encarga de actualizar el setting del AtcLogFile.
        /// AtcLogFile solo se modifica aquí. Por eso, es que despues de modificar 
        /// se invoca un settings.Save();
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fsWatchOfficinaStelare_Changed(object sender, FileSystemEventArgs e)
        {
            fsVigilaOfficinaStelare(e);
        }

        private void fsVigilaOfficinaStelare(FileSystemEventArgs e)
        {
            if (e.FullPath != settings.AtcLogFile)
            {
                if (this.backgroundWorkerATC02.IsBusy)
                {
                    logger.Info("Cancelando monitoreo en archivo:" + settings.AtcLogFile);
                    this.backgroundWorkerATC02.CancelAsync();
                }
                settings.AtcLogFile = e.FullPath;
                logger.Info("Nuevo archivo de logging es:" + settings.AtcLogFile);

                settings.Save();
                System.Threading.Thread.Sleep(500);

                Console.WriteLine("#");
                if (!this.backgroundWorkerATC02.IsBusy)
                {
                    logger.Info("Reiniciando monitoreo en archivo:" + settings.AtcLogFile);
                    this.backgroundWorkerATC02.RunWorkerAsync(settings.AtcLogFile);
                }
            }
        }

        private void bLogFile_Click(object sender, EventArgs e)
        {

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

        /// <summary>
        /// Metodo en Hebra.
        /// Codigo Modificado a partir de un ejemplo de "tail" hallado en:
        /// http://www.codeproject.com/Articles/5854/Tail-utility-for-windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerATC02_DoWork(object sender, DoWorkEventArgs e)
        {
            String fileName;
            StringBuilder respuesta;
            respuesta = new StringBuilder();
            fileName = e.Argument as String;
            
            ///while (true)
           //{
            Boolean salir;
            salir = false;
                try
                {
                    using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        //start at the end of the file
                        long lastMaxOffset = reader.BaseStream.Length;

                        while (!salir)
                        {
                            System.Threading.Thread.Sleep(100);
                            if (backgroundWorkerATC02.CancellationPending)
                            {
                                salir = true;
                                reader.Close();
                                break;
                            }
                            //if the file size has not changed, idle
                            if (reader.BaseStream.Length == lastMaxOffset)
                            {
                                //e.Result = respuesta.ToString();
                                continue;
                            }

                            //seek to the last max offset
                            reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

                            //read out of the file until the EOF
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {
                                
                                if (!(respuesta.ToString().Contains(line)))
                                {
                                    //Console.WriteLine(line);
                                    respuesta.AppendLine(line);
                                }
                                // Capturado un cambio en el archivo, lo reportamos
                                if (respuesta.ToString().Contains("DEWPO"))
                                {
                                    e.Result = respuesta.ToString();
                                    salir = true;
                                    reader.Close();
                                    break;
                                }
                            }
                            //update the last max offset
                            //lastMaxOffset = reader.BaseStream.Position;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            //}
        }

        private void backgroundWorkerATC02_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorkerATC02_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("<backgroundWorkerATC02_RunWorkerCompleted>");
            String atc02Log;
            atc02Log = (String)e.Result;
            Console.WriteLine(atc02Log);
            analizaATC02(atc02Log);
            Console.WriteLine("</backgroundWorkerATC02_RunWorkerCompleted>");
            while (this.backgroundWorkerATC02.IsBusy)
            {
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine("$");
            if (!this.backgroundWorkerATC02.IsBusy)
            {
                logger.Info("Re-Iniciando monitoreo en archivo:" + settings.AtcLogFile);
                this.backgroundWorkerATC02.RunWorkerAsync(settings.AtcLogFile);
            }
        }

        private void analizaATC02(String atc02Log)
        {
            if (atc02Log == null)
            {
                return;
            }
            String[] linea;
            linea = atc02Log.Split(("\n").ToCharArray());
            int CantLineas;
            int partLength;
            CantLineas = linea.Length;
            String[] part;
            String[] fila;
            String textoFechaAtc;
            String lastPart; // Texto despues del último espacio de la línea.
            int addOrder; //Permite examinar el log en el orden esperado de las lineas con datos.
            addOrder = 0;
            this.dataGridViewATC02.Rows.Clear();
            for (int i = 0; i < CantLineas; i++)
            {
                fila = new String[2];
                part = linea[i].Split((" ").ToCharArray());
                partLength = part.Length;
                lastPart = part[partLength - 1];
                if ((addOrder==0) && (linea[i].Contains("Response received:")))
                {
                    fila[0] = "Ultima respuesta";
                    textoFechaAtc = linea[i].Substring(0,17);
                    Console.WriteLine("textoFechaAtc="+textoFechaAtc);
                    lastResponseTimeStamp = DateTime.MinValue;
                    try
                    {
                        lastResponseTimeStamp = DateTime.ParseExact(textoFechaAtc, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (FormatException exc)
                    {
                        logger.Error("exc=" + exc.Message);
                    }

                    StringBuilder textoFecha;
                    textoFecha = new StringBuilder();
                    textoFecha.Append(lastResponseTimeStamp.ToLongDateString());
                    textoFecha.Append(" ");
                    textoFecha.Append(lastResponseTimeStamp.ToShortTimeString());

                    Console.WriteLine("timestamp=" + textoFecha.ToString());
                    fila[1] = textoFecha.ToString();
                    addOrder++;
                }
                if ((addOrder == 1) && (linea[i].Contains("SETFAN")))
                {
                    fila[0] = "SETFAN";
                    this.setFan = Int32.Parse(lastPart);
                    fila[1] = this.setFan.ToString();
                    addOrder++;
                }
                if ((addOrder == 2) && (linea[i].Contains("PRITE")))
                {
                    fila[0] = "PRITE";                    
                    this.priTemp = Double.Parse(lastPart);
                    fila[1] = this.priTemp.ToString();
                    addOrder++;
                }
                if ((addOrder==3) && (linea[i].Contains("SECTE")))
                {
                    fila[0] = "SECTE";
                    this.secTemp = Double.Parse(lastPart);
                    fila[1] = this.secTemp.ToString();
                    addOrder++;
                }
                if ((addOrder == 4) && (linea[i].Contains("BFL")))
                {
                    fila[0] = "BFL";
                    this.bfl = Double.Parse(lastPart);
                    fila[1] = this.bfl.ToString();
                    this.dataGridViewATC02.Rows.Add(fila);
                    fila[0] = "FOCSTEP";
                    this.focstep =BflToFocSetp(this.bfl);
                    fila[1] = this.focstep.ToString();
                    addOrder++;
                }
                if ((addOrder == 5) && (linea[i].Contains("AMBTE")))
                {
                    fila[0] = "AMBTE";
                    this.ambTemp = Double.Parse(lastPart);
                    fila[1] = this.ambTemp.ToString();
                    addOrder++;
                }                
                //fila[0] = part[0];
                //fila[1] = part[part.Length - 1];
                if ((fila[0]!=null) && (fila[0].Length>0))
                {
                    this.dataGridViewATC02.Rows.Add(fila);
                }
            }
        }

        /// <summary>
        /// Ecuación propia del sistema de enfoque del Chase500
        /// que convierte el BFL en los Steps (Para Stepper Motors)
        /// que cuenta el firmware para desplazar el espejo secundario
        /// </summary>
        /// <param name="FocStep"></param>
        /// <returns></returns>
        public static int BflToFocSetp(Double FocStep)
        {
            int respuesta;
            respuesta = (int) (100.0 * (FocStep - 130.0));
            return respuesta;
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
            vejezLectura = DateTime.Now.Subtract(lastResponseTimeStamp);
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
        /// ATC02 a traves del driver Officina Stellare genera un log.
        /// 
        /// El archivo de log es actualizado cada 30 segundos.
        /// 
        /// Una lectura se considera fresca si la última lectura del log
        /// difiere a lo máximo en 2 minutos con respecto a la hora del sistema operativo.
        /// 
        /// Un timer de esta aplicación, cada 15 segundos evalua la
        /// frescura de la última lectura del log realizada por esta aplicación.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerAtc02_Tick(object sender, EventArgs e)
        {
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

        private void fsWatchOfficinaStelare_Created(object sender, FileSystemEventArgs e)
        {
            fsVigilaOfficinaStelare(e);
        }

    }
}
