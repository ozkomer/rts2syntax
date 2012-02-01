using System;
using System.Collections.Generic;
using System.Text;

namespace Cupula
{
    public class Instruccion
    {
        ushort startAddres;
        ushort tipo;
        byte[] entrada;
        byte[] salida;
        String instruccion;

        /// <summary>
        /// w 16 135,0
        /// r 20
        /// </summary>
        /// <param name="instrccion"></param>
        public Instruccion(String instrccion)
        {
            this.instruccion = instrccion;
            String[] part;
            part = instrccion.Split((" ").ToCharArray());
            if (part[0] == "w") { tipo = 1; } else { tipo = 2; }
            startAddres = ushort.Parse(part[1]);
            if (tipo==2)
            {
                entrada = null;
                salida = new byte[2];
            }
            else
            {
                entrada = new byte[2];
                String[] data;
                data = part[2].Split((",").ToCharArray());
                entrada[0] = Byte.Parse(data[0]);
                entrada[1] = Byte.Parse(data[1]);
                salida = new byte[2];
            }
        }

        public String ejecutar (ModbusTCP.Master master)
        {
            if (tipo == 2)
            {
                master.ReadHoldingRegister(0, startAddres, 1,ref salida);

            }
            if (tipo == 1)
            {
                master.WriteSingleRegister(0,startAddres,entrada,ref salida);
            }
            StringBuilder respuesta;
            respuesta = new StringBuilder ();
            respuesta.Append(instruccion);
            respuesta.Append("-->");
            respuesta.Append(salida[0]);
            respuesta.Append(",");
            respuesta.Append(salida[1]);
            return respuesta.ToString();
        }
    }
}
