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
using SchedulerPost;


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
            int projectID;
            String projectName;
            projectName = this.cbxProject.GetItemText(this.cbxProject.SelectedItem);
            projectID = (int) this.cbxProject.SelectedValue;
            String archivoRTML;
            archivoRTML = this.crearDocumentoRtml(projectID, projectName);
            int idProjectACP;
            idProjectACP = this.getProjectId(projectName);
            this.eliminaPlanes(idProjectACP);
            this.uploadRTML(archivoRTML);
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Archivo RTML creado","DB to RTML2.3");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'chase500DataSet.projects' table. You can move, or remove it, as needed.
            this.projectsTableAdapter.Fill(this.chase500DataSet.projects);

        }


        /// <summary>
        /// Crea un documento RTML para un proyecto del Telescopio 500. Estos proyectos
        /// estan definidos en la tabla mysql "chase500.projects" de Zwicky.
        /// 
        /// Hay 4 posibilidades: 
        /// StandardFields
        /// FollowUp
        /// Search
        /// Candidato a Supernova        
        /// </summary>
        /// <param name="projectID">columna id en la tabla chase500.projects</param>
        /// <param name="projectName">columna name en la tabla chase500.projects</param>
        /// <returns>La ruta completa del archivo RTML creado.</returns>
        private String crearDocumentoRtml(int projectID, String projectName)
        {
            Console.WriteLine("projectID=" + projectID);
            String fileName;
            fileName = (projectName.Replace(" ", String.Empty) + ".xml");
            Console.WriteLine("fileName=" + fileName);

            StringBuilder archivoSalida;
            archivoSalida = new StringBuilder();
            archivoSalida.Append(@"C:\Users\Administrator\Desktop\rtml2.3\");
            archivoSalida.Append(fileName);
            String pathFileNameRtml;
            pathFileNameRtml = archivoSalida.ToString();
            Console.WriteLine("archivoSalida=" + pathFileNameRtml);
            DbToRTML23 dbToRTML;

            dbToRTML = new DbToRTML23(projectID, pathFileNameRtml);
            dbToRTML.FillRTML();
            dbToRTML.saveRTML();
            return pathFileNameRtml;
        }

        /// <summary>
        /// retorna el id en ACP de un proyecto.
        /// </summary>
        /// <param name="projectName">Nombre del proyecto</param>
        /// <returns>id segun ACP</returns>
        private int getProjectId(String projectName)
        {
            SchedulerStatus rtmlPost;
            rtmlPost = new SchedulerStatus(projectName);
            rtmlPost.readStatus();
            return rtmlPost.ProjectID;
        }

        private void eliminaPlanes(int idProjectACP)
        {
            System.Console.WriteLine("Eliminando Planes ACP projectID:" + idProjectACP);
            ProjectPlans pPlans;
            pPlans = new ProjectPlans(idProjectACP);
            pPlans.readStatus();
            List<Plan> listaPlanes;
            listaPlanes =  pPlans.ProjctPlans;
 
            foreach (Plan plan in listaPlanes)
            {
                eliminaPlan(plan);
            }      
        }

        private void eliminaPlan(Plan plan)
        {   
            System.Console.WriteLine("Eliminando Plan:" + plan.ToString());
            DeletePlan deletePlan;
            deletePlan = new DeletePlan(plan.Id);
            String respuesta;
            respuesta = deletePlan.sendPost();
            Console.WriteLine("respuesta=" + respuesta);            
        }

        public void uploadRTML(String archivoRTML)
        {
            UploadRTML rtmlPost;
            rtmlPost = new UploadRTML(true, archivoRTML);
            String webResponse;
            webResponse = rtmlPost.upload();
            Console.WriteLine("webResponse=" + webResponse);
            int largo = 0;
            if (webResponse != null)
            {
                largo = webResponse.Length;
            }
        }
            

    }
}
