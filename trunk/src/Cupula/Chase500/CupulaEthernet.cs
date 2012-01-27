using System;
using System.Collections.Generic;
using System.Text;
using ModbusTCP;

namespace Chase500
{
    public class CupulaEthernet
    {
        private Boolean[] zregJ1XT1;
        private Boolean[] zregO1XT1;


        public const long USEC_SEC = 1000000;
        public const long SLEEP_OPENING = 500;

        public const UInt16 ZC_NORTH_OPEN = 0x0001;
        public const UInt16 ZC_SOUTH_OPEN = 0x0010;

        public const UInt16 ZC_NORTH_OPEN_50 = 0x0002;
        public const UInt16 ZC_NORTH_CLOSE_50 = 0x0004;

        public const UInt16 ZC_SOUTH_OPEN_50 = 0x0020;
        public const UInt16 ZC_SOUTH_CLOSE_50 = 0x0040;

        public const UInt16 ZC_NORTH_CLOSE = 0x0000;
        public const UInt16 ZC_SOUTH_CLOSE = 0x0000;

        public const ushort ZREG_J1XT1 = 16;
        public const ushort ZREG_O1XT1 = 20;

        public const UInt16 ZS_SOUTH_OPEN  =  4;
        public const UInt16 ZS_SOUTH_50    =  5;
        public const UInt16 ZS_SOUTH_CLOSE =  6;
        public const UInt16 ZS_NORTH_OPEN  =  7;
        public const UInt16 ZS_NORTH_50    =  8;
        public const UInt16 ZS_NORTH_CLOSE =  9;


        public const UInt16 DOME_DOME_MASK = 0xffff;
        public const UInt16 DOME_CLOSING = 0xffff;

        public enum roof { Open, Half, Close };


        public const ushort OPEN = 0;
        public const ushort HALF = 1;
        public const ushort CLOSE = 2;

        /// <summary>
        /// Posicion definido por el usuario para la apertura del lado Norte.
        /// </summary>
        private ushort northRoof;

        /// <summary>
        /// Posicion definido por el usuario para la apertura del lado Sur.
        /// </summary>
        private ushort southRoof;

        private Master zelioConn;

        public CupulaEthernet()
        {
            zelioConn = new Master();
            zregJ1XT1 = new Boolean[16];
            zregO1XT1 = new Boolean[16];
            northRoof = CupulaEthernet.OPEN;
            southRoof = CupulaEthernet.OPEN;

            Console.WriteLine("zelioConn.connected=" + zelioConn.connected);
        }

        /// <summary>
        /// Posicion definido por el usuario para la apertura del lado Norte.
        /// </summary>
        public ushort NorthRoof
        {
            get { return this.northRoof; }
            set { this.northRoof = value; }
        }

        public ushort SouthRoof
        {
            get { return this.southRoof; }
            set { this.southRoof = value; }
        }



        public Master TcpSession
        {
            get { return this.zelioConn; }
            set { this.zelioConn = value; }
        }

        /// <summary>
        /// Abrir personalizado.
        /// </summary>
        public void abrir()
        {
            Console.WriteLine("Abrir");
            for (int i = 0; i < 16; i++)
            {
                if ((i == 3) || (i == 7))
                {
                    Zreg_J1XT1[i] = true;
                }
                else
                {
                    Zreg_J1XT1[i] = false;
                }
            }
            Write_ZREG_J1XT1();
            System.Threading.Thread.Sleep(500);
            Read_ZREG_O1XT1();
            int valor;
            valor = 0;
            // Lado Norte
            switch (northRoof)
            {
                case (CupulaEthernet.OPEN):
                    valor += 1;
                    break;
                case (CupulaEthernet.HALF):
                    if (Zreg_O1XT1[CupulaEthernet.ZS_NORTH_CLOSE])
                    {
                        valor += (1 << 1);
                    }
                    if (Zreg_O1XT1[CupulaEthernet.ZS_NORTH_OPEN])
                    {
                        valor += (1 << 2);
                    }
                    break;
                case (CupulaEthernet.CLOSE):
                    //valor += (1 << 3);
                    break;
            }
            switch (southRoof)
            {
                case (CupulaEthernet.OPEN):
                    valor += (1 << 4);
                    break;
                case (CupulaEthernet.HALF):
                    if (Zreg_O1XT1[CupulaEthernet.ZS_SOUTH_CLOSE])
                    {
                        valor += (1 << 5);
                    }
                    if (Zreg_O1XT1[CupulaEthernet.ZS_SOUTH_OPEN])
                    {
                        valor += (1 << 6);
                    }
                    break;
                case (CupulaEthernet.CLOSE):
                    //valor += (1 << 7);
                    break;
            }
            for (int i = 0; i < 16; i++)
            {
                Zreg_J1XT1[i] = ((valor % 2) == 1);
                valor /= 2;
            }
            Write_ZREG_J1XT1();
        }

        private Boolean[] Read_PLC(ushort startRegister, ushort cantBytes)
        {
            byte[] regs;
            regs = new byte[cantBytes];
            try
            {
                zelioConn.ReadHoldingRegister(0, startRegister, cantBytes, ref regs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            ulong total;
            total = 0;
            for (int i = 0; i < cantBytes; i++)
            {
                total = (total << (8 * i));
                total += regs[i];
            }
            Console.WriteLine("Read_PLC: total=" + total);
            Boolean[] respuesta;
            respuesta = new Boolean[8 * cantBytes];
            for (int i = 0; i < respuesta.Length; i++)
            {
                respuesta[i] = ((total % 2) == 1);
                total /= 2;
            }
            return respuesta;
        }

        /// <summary>
        /// Lee los 16 bits del primer "Control register".
        /// Ojo, solo son relevantes los primeros 8 bits
        /// </summary>
        public void Read_ZREG_J1XT1()
        {
            this.zregJ1XT1 = Read_PLC(ZREG_J1XT1, 2);
        }

        /// <summary>
        /// Envia al PLC los 16 bits del primer "Control Register".
        /// </summary>
        public void Write_ZREG_J1XT1()
        {
            byte[] valor;
            valor = new byte[2];
            int indice;
            for (int i = 0; i < 16; i++)
            {
                indice = (i / 8);
                if (zregJ1XT1[i])
                {
                    valor[indice] += (byte)(1 << (i - (indice * 8)));
                }                 
            }
            Console.WriteLine("valor[0]=" + valor[0] + "    valor[1]=" + valor[1]);            
            zelioConn.WriteSingleRegister(0,ZREG_J1XT1,valor);
        }

        /// <summary>
        /// Lee los 16 bits del primer "Ouput Register"
        /// </summary>
        public void Read_ZREG_O1XT1()
        {
            this.zregO1XT1 = Read_PLC(ZREG_O1XT1, 2);
        }

        public Boolean[] Zreg_J1XT1
        {
            get { return this.zregJ1XT1; }
            set { this.zregJ1XT1 = value; }
        }

        public Boolean[] Zreg_O1XT1
        {
            get { return this.zregO1XT1; }
            set { this.zregO1XT1 = value; }
        }


        /// <summary>
        /// Al invocar a esta funcion, los valores de zregO1XT1 deben estar frescos.
        /// </summary>
        /// <returns> 0 si el estado de apertura difiere del estado deseado para los techos norte y sur.
        /// 1 si el estado de apertura coincide solo para el techo norte.
        /// 2 si el estado de apertura coincide solo para el techo sur.
        /// 3 si el estado de apertura coincide para ambos techos.
        /// </returns>
        public int IsOpened()
        {
            int hits = 0;
            switch (this.northRoof)
            {
                case OPEN:
                    if (zregO1XT1[ZS_NORTH_OPEN])
                        hits |= 0x01;
                    break;
                case HALF:
                    if (zregO1XT1[ZS_NORTH_50])
                        hits |= 0x01;
                    break;
                case CLOSE:
                    if (zregO1XT1[ZS_NORTH_CLOSE])
                        hits |= 0x01;
                    break;
            }
            switch (this.southRoof)
            {
                case 0:
                    if (zregO1XT1[ZS_SOUTH_OPEN])
                        hits |= 0x02;
                    break;
                case 1:
                    if (zregO1XT1[ZS_SOUTH_50])
                        hits |= 0x02;
                    break;
                case 2:
                    if (zregO1XT1[ZS_SOUTH_CLOSE])
                        hits |= 0x02;
                    break;
            }

            //if (hits == 0x03)
            //    return -2;
            //return 0;
            return hits;
        }

        /// <summary>
        /// Al invocar a esta funcion, los valores de zregO1XT1 deben estar frescos.
        /// </summary>
        /// <returns></returns>
        public int IsClosed()
        {
            // check states of end switches..
            int hits = 0;
            if (zregO1XT1[ZS_SOUTH_CLOSE]) 
            {
                hits |= 0x01;
            }
            if (zregO1XT1[ZS_NORTH_CLOSE])
            {
                hits |= 0x02;
            }
            return hits;
        }

        /// <summary>
        /// Invocar cuando en la interfaz se escoja la opcion "Set Close".
        /// </summary>
        public void SetClose()
        {
            for (int i = 0; i < 8; i++)
            {
                if ((i == 3) || (i == 7))
                {
                    Zreg_J1XT1[i] = true;
                }
                else
                {
                    Zreg_J1XT1[i] = false;
                }
            }
        }
        //private UInt16 GetMask(Byte o, Byte ns)
        //{
        //    switch (o)
        //    {
        //        case 0:
        //            return (ns == 0 ? ZC_NORTH_OPEN : ZC_SOUTH_OPEN);
        //        case 1:
        //            {
        //                if (info())
        //                    return 0;
        //                UInt16 rc = 0;
        //                switch (ns)
        //                {
        //                    case 0:
        //                        if (northMiddle)
        //                        {
        //                            rc = ZC_NORTH_CLOSE_50;
        //                        }
        //                        else
        //                        {
        //                            rc = ZC_NORTH_OPEN_50;
        //                        }
        //                        break;
        //                    case 1:
        //                        if (southMiddle)
        //                        {
        //                            rc = ZC_SOUTH_CLOSE_50;
        //                        }
        //                        else
        //                        {
        //                            rc = ZC_SOUTH_OPEN_50;
        //                        }

        //                        break;
        //                }

        //                //zelioConn ->writeHoldingRegisterMask (ZREG_J2XT1, ZC_RESET_PASSED, ZC_RESET_PASSED);
        //                System.Threading.Thread.Sleep((int)SLEEP_OPENING);
        //                //zelioConn->writeHoldingRegisterMask (ZREG_J2XT1, ZC_RESET_PASSED, 0);
        //                return rc;
        //            }
        //        case 2:
        //            return ns == 0 ? ZC_NORTH_CLOSE : ZC_SOUTH_CLOSE;
        //        //	case 3:
        //        //      return ns == 0 ? ZC_NORTH_STOP : ZC_SOUTH_STOP; 
        //    }
        //    return 0;
        //}


        private void MatchOpenSetting()
        {
            UInt16 no;
            UInt16 so;
            //no = GetMask(this.openNorth, 0);
            //so = GetMask(this.openSouth, 1);
            byte[] command;
            command = new byte[2];
            command[0] = 0x0088;
            command[1] = 0x0000;
            zelioConn.WriteSingleRegister(0,ZREG_J1XT1, command);
            System.Threading.Thread.Sleep((int)SLEEP_OPENING);
            command[0] = 0x0011;
            zelioConn.WriteSingleRegister(0, ZREG_J1XT1, command);

            //zelioConn->writeHoldingRegisterMask(ZREG_J1XT1, ZC_MASK_OPEN_CLOSE, no | so);

        }


        public void StartOpen()
        {
            MatchOpenSetting();
            System.Threading.Thread.Sleep((int)SLEEP_OPENING);
            //info();
        }

        //------------------- Replica codigo RTS2 (Desde aqui hasta el final)------------
        /*
        private Boolean info()
        {
            Boolean respuesta;
            respuesta = true;
            return respuesta;
        }







        private ushort getState()
        {
            return 0xffff;

        }

        public int StartClose()
        {
            if ((getState() & DOME_DOME_MASK) == DOME_CLOSING)
                return 0;
            try
            {
                //zelioConn->writeHoldingRegisterMask(ZREG_J1XT1, ZC_MASK_OPEN_CLOSE, ZC_NORTH_CLOSE | ZC_SOUTH_CLOSE);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            // 20 minutes timeout..
            //setWeatherTimeout(1200, "closed, timeout for opening (to allow dissipate motor heat)");
            System.Threading.Thread.Sleep((int)SLEEP_OPENING);
            info();
            return 0;
        }


*/

    }
}
