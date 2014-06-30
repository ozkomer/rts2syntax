using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Chase500DB
{
    public class DbToRTML23
    {
        private chase500DataSetTableAdapters.plansTableAdapter taPlan;
        private chase500DataSetTableAdapters.projectsTableAdapter taProject;
        private chase500DataSetTableAdapters.observationsTableAdapter taObservation;
        private chase500DataSetTableAdapters.imagesetsTableAdapter taImageSet;
        private chase500DataSetTableAdapters.filtersTableAdapter taFilter;
        private chase500DataSetTableAdapters.constraintsTableAdapter taConstraints;

        /// <summary>
        /// El objeto a convertir en XML (cumppliente con la XSD de rtml2.3)
        /// </summary>
        private RTML rtml;

        private RTMLContact contacto;
        private int projectID;
        private String rtmlFilename;

        // Cache con la lista de Filtros
        chase500DataSet.filtersDataTable dtFilter;
            

        public DbToRTML23(int _projectID, String _rtmlFilename)
        {
            taPlan = new Chase500DB.chase500DataSetTableAdapters.plansTableAdapter();
            taProject = new Chase500DB.chase500DataSetTableAdapters.projectsTableAdapter();
            taObservation = new Chase500DB.chase500DataSetTableAdapters.observationsTableAdapter();
            taImageSet = new Chase500DB.chase500DataSetTableAdapters.imagesetsTableAdapter();
            taFilter = new Chase500DB.chase500DataSetTableAdapters.filtersTableAdapter();
            taConstraints = new Chase500DB.chase500DataSetTableAdapters.constraintsTableAdapter();

            // Cargamos el cache con la lista de filtros
            dtFilter = taFilter.GetData();

            rtml = new RTML();
            rtml.version = 2.3F;
            this.projectID = _projectID;
            this.rtmlFilename = _rtmlFilename;
        }

        private chase500DataSet.filtersRow getFiltro(int filterId)
        {
            Console.WriteLine("getFiltro-->" + filterId);
            foreach (chase500DataSet.filtersRow fila in dtFilter)
            {
                if (fila.id == filterId) { return fila; }
            }
            return null;
        }



        
        /// <summary>
        /// Barre la lista de plans obteniendo bloques de 30 registros.
        /// 
        /// </summary> 
        public void FillRTML()
        {
            this.setContact();
            chase500DataSet.observationsDataTable dtObservation;
            chase500DataSet.projectsDataTable dtProject;
            chase500DataSet.observationsRow rowObservation;
            chase500DataSet.projectsRow rowProject;


            //dtProject = taProject.GetData();
            //rowProject = null;
            //foreach (chase500DataSet.projectsRow rowProj in dtProject.Rows)
            //{
            //    Console.WriteLine(rowProj.id + "--" + rowProj.name);
            //    if (rowProj.id == 3)
            //    {
            //        rowProject = rowProj;
            //        break;
            //    }
            //}

            List<RTMLRequest> listaRequest;
            listaRequest = new List<RTMLRequest>();
            Console.WriteLine("START:FillRTML");


            int firstRow;
            firstRow = 0;
            Boolean bloqueConDatos;
            bloqueConDatos  = true;
            while (bloqueConDatos)
            {
                chase500DataSet.plansDataTable dtPlan;

                // Aqui se lee un bloque de 30 registros
                Boolean timeout = false;
                dtPlan = null;
                do
                {
                    try
                    {
                        dtPlan = taPlan.GetDataByProjectIdLimit(this.projectID, firstRow);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("taPlan.GetDataByProjectIdLimit-> Timeout, reintentando.");
                        timeout = true;
                    }         
                } while (timeout);

               
                firstRow += 30; // en la siguiente vuelta leeremos los siguiente 30 registros
                Console.WriteLine("dtPlan.Rows.Count=" + dtPlan.Rows.Count);
                bloqueConDatos = (dtPlan.Rows.Count>0);
                if (dtPlan.Rows.Count==0) {
                    Console.WriteLine("Bloque sin datos, abandonando loop 'bloqueConDatos'");
                    break;
                }
                foreach (chase500DataSet.plansRow rowPlan in dtPlan.Rows)
                {
                    if (rowPlan.enabled)
                    {
                        dtObservation = taObservation.GetDataByPlanId(rowPlan.id);
                        rowObservation = (chase500DataSet.observationsRow)dtObservation.Rows[0];
                        RTMLRequest request;
                        request = new RTMLRequest();
                        request.ID = rowPlan.name;
                        request.UserName = contacto.User;
                        request.Description = rowPlan.description;
                        request.Reason = ("monitor=" + rowPlan.resubmit);

                        request.Schedule = getConstraints(rowObservation.constraint_id, rowPlan.priority);

                        dtProject = taProject.GetById(rowPlan.project_id);
                        rowProject = (chase500DataSet.projectsRow)dtProject.Rows[0];

                        request.Project = rowProject.name;
                        RTMLRequestTarget target;
                        target = new RTMLRequestTarget();
                        target.Name = rowObservation.description;
                        target.Coordinates = getCoordenadas(rowObservation);
                        request.Target = new RTMLRequestTarget[1];
                        request.Target[0] = target;
                        target.Picture = getPictures(rowObservation.id);

                        listaRequest.Add(request);
                    }
                }
            }
            rtml.Request = listaRequest.ToArray();
            Console.WriteLine("END:FillRTML");
        }

        private RTMLRequestSchedule getConstraints(int constraintsID, int priority)
        {
            RTMLRequestSchedule schedule;
            schedule = new RTMLRequestSchedule();
            schedule.PrioritySpecified = true;
            schedule.Priority = (sbyte)priority;
            chase500DataSet.constraintsDataTable dtConstraints;
            chase500DataSet.constraintsRow filaSchedule;
            dtConstraints = taConstraints.GetDataById(constraintsID);
            filaSchedule = (chase500DataSet.constraintsRow)dtConstraints.Rows[0];
            schedule.HorizonSpecified = filaSchedule.horizonEnable;
            schedule.Horizon = filaSchedule.horizon;
            if (filaSchedule.hourAngleEnable)
            {
                RTMLRequestScheduleHourAngleRange hourAngle;
                hourAngle = new RTMLRequestScheduleHourAngleRange();
                hourAngle.East = filaSchedule.eastLimit;
                hourAngle.West = filaSchedule.westLimit;
                schedule.HourAngleRange = hourAngle;
            }
            if (filaSchedule.moonAvoidEnable)
            {
                RTMLRequestScheduleMoon luna;
                luna = new RTMLRequestScheduleMoon();
                luna.Distance = filaSchedule.distance;
                luna.Width = filaSchedule.width;
                schedule.Moon = luna;
            }
            return schedule;
        }

        private RTMLRequestTargetPicture[] getPictures(int observationRowID)
        {
            List<RTMLRequestTargetPicture> listaRespuesta;
            listaRespuesta = new List<RTMLRequestTargetPicture>();
            chase500DataSet.imagesetsDataTable dtImageSet;

            RTMLRequestTargetPicture requestTargetPicture;
            bool timeOut;
            timeOut = false;
            dtImageSet = null;
            do
            {
                try
                {
                    dtImageSet = taImageSet.GetDataByObservationId(observationRowID);
                }
                catch (Exception)
                {
                    timeOut = true;
                }
            } while (timeOut);

            chase500DataSet.filtersRow filtroOptico;
            foreach (chase500DataSet.imagesetsRow filaImage in dtImageSet.Rows)
            {
                if (filaImage.enabled)
                {
                    requestTargetPicture = new RTMLRequestTargetPicture();
                    requestTargetPicture.countSpecified = true;
                    filtroOptico = getFiltro(filaImage.filter_id);
                    requestTargetPicture.Name = filtroOptico.position.ToString();
                    requestTargetPicture.ExposureTime = filaImage.exposure;
                    requestTargetPicture.Filter = filtroOptico.name;
                    requestTargetPicture.count = (uint)filaImage.repeat;
                    listaRespuesta.Add(requestTargetPicture);
                }
            }
            return listaRespuesta.ToArray();
        }



        private RTMLRequestTargetCoordinates getCoordenadas(chase500DataSet.observationsRow rowObs)
        {
            RTMLRequestTargetCoordinates targetCoordinates;
            StringBuilder strRA;
            StringBuilder strDEC;
            targetCoordinates = new RTMLRequestTargetCoordinates();
            strRA = new StringBuilder();
            strDEC = new StringBuilder();

            strRA.Append(rowObs.rahh);
            strRA.Append(":");
            strRA.Append(rowObs.ramm);
            strRA.Append(":");
            strRA.Append(rowObs.rass);

            if (rowObs.decp)
            {
                strDEC.Append("+");
            }
            else
            {
                strDEC.Append("-");
            }

            strDEC.Append(rowObs.decdd);
            strDEC.Append(":");
            strDEC.Append(rowObs.decmm);
            strDEC.Append(":");
            strDEC.Append(rowObs.decss);


            ASCOM.Utilities.Util util;
            util = new ASCOM.Utilities.Util();

            targetCoordinates.RightAscension = util.HMSToDegrees(strRA.ToString());
            targetCoordinates.Declination = util.DMSToDegrees(strDEC.ToString());

            return targetCoordinates;
        }



        private void setContact()
        {
            contacto = new RTMLContact();
            contacto.Email = "jmaza@das.uchile.cl";
            contacto.User = "Jose Maza";
            rtml.Contact = contacto;
        }

        public void saveRTML()
        {
            XmlSerializer serializer;
            serializer = new XmlSerializer(typeof(RTML));
            Console.WriteLine("START:saveRTML");
            using (Stream stream = new FileStream( this.rtmlFilename, FileMode.Create))
            {
                XmlWriter writer;
                //writer = new XmlTextWriter(stream, Encoding.Unicode);
                writer = new XmlTextWriter(stream, Encoding.Default);
                serializer.Serialize(writer, this.rtml);
                writer.Close();
            }
            Console.WriteLine("END:saveRTML");
        }
    }
}
