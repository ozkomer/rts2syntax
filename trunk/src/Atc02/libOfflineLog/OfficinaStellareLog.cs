using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace libOfflineLog
{
    public class OfficinaStellareLog
    {
        String filename;
        List<OfficinaStellareRecord> lista;

        public OfficinaStellareLog(String archivo)
        {
            this.filename = archivo;
            lista = new List<OfficinaStellareRecord>();
        }

        public void analizar()
        {
            TextReader tr;
            String linea;
            tr = new StreamReader(this.filename);
            List<String> updatePC;
            updatePC = null;
            while ((linea = tr.ReadLine()) != null)
            {
                if ((updatePC==null) && (linea.Contains("'UPDATEPC")))
                {
                    //Console.WriteLine(linea);
                    updatePC = new List<string>();
                    if (linea.Trim().Length > 5)
                    {
                        updatePC.Add(linea);
                    }
                }
                else
                {
                    if (updatePC != null)
                    {
                        if ((linea.Trim().Length > 5) && (!linea.Contains("UPDATEPC"))) 
                        {
                            updatePC.Add(linea);
                        }
                        if (updatePC.Count == 12)
                        {
                            this.creaRegistro(updatePC);
                            updatePC = null;
                        }
                    }
                }

            }            
        }

        public void creaRegistro(List<String> lineas)
        {
            OfficinaStellareRecord registro;
            registro = new OfficinaStellareRecord();
            registro.Fecha = OfficinaStellareLog.getFecha(lineas[1]);
            registro.AmbientTemperature = getDouble(lineas[8]);
            registro.PrimaryTemperature = getDouble(lineas[4]);
            registro.SecondaryTemperature = getDouble(lineas[5]);
            registro.Bfl = getDouble(lineas[6]);
            registro.Pressure = getDouble(lineas[10]);
            registro.Relativehumidity = getDouble(lineas[9]);

            this.lista.Add(registro);
            //console.writeline("fecha->" + registro.fecha.ticks);
            //console.writeline("stabt ->" + lineas[2]);
            //console.writeline("setfan  ->" + lineas[3]);
            //console.writeline("prite ->" + registro.primarytemperature);
            //console.writeline("secte ->" + registro.secondarytemperature);
            //console.writeline("bfl ->" + registro.bfl);
            //console.writeline("shutter ->" + lineas[7]);
            //console.writeline("ambte ->" + registro.ambienttemperature);
            //console.writeline("humid ->" + registro.relativehumidity);
            //console.writeline("pres ->" + registro.pressure);
            //console.writeline("dewpo ->" + lineas[11]);
        }

        public override string ToString()
        {
            StringBuilder respuesta;
            respuesta = new StringBuilder();
            int largoLista;
            largoLista = this.lista.Count;
            for (int i = 0; i < largoLista; i++)
            {
                respuesta.AppendLine(this.lista[i].ToString());
            }
            return respuesta.ToString();
        }

        public static double getDouble (String linea)
        {
            double respuesta;
            String[] parte;
            int largoParte;
            parte = linea.Split((" ").ToCharArray());
            largoParte = parte.Length;
            respuesta = Double.Parse(parte[largoParte-1]);
            return respuesta;
        }

        public static DateTime getFecha(String linea)
        {
            String[] space;
            String[] yy_mm_dd;
            String[] hh_mm_ss;
            DateTime respuesta;
            
            space = linea.Split((" ").ToCharArray());
            yy_mm_dd = (space[0]).Split(("-").ToCharArray());
            hh_mm_ss = (space[1]).Split((":").ToCharArray());
            int YYYY, MM, DD, hh, mm, ss;
            YYYY = (2000 + Int32.Parse(yy_mm_dd[0]));
            MM = Int32.Parse(yy_mm_dd[1]);
            DD = Int32.Parse(yy_mm_dd[2]);
            hh = Int32.Parse(hh_mm_ss[0]);
            mm = Int32.Parse(hh_mm_ss[1]);
            ss = Int32.Parse(hh_mm_ss[2]);
            respuesta = new DateTime(YYYY, MM, DD, hh, mm, ss);

            return respuesta;            
        }
    }
}
