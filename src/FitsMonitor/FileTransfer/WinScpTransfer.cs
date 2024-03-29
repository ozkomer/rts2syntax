﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinSCP;
using log4net;

namespace FileTransfer
{
    public class WinScpTransfer
    {
        private String hostName;
        private String userName;
        private String sshHostKey;
        private static readonly ILog logger = LogManager.GetLogger(typeof(WinScpTransfer));

        public WinScpTransfer(String hostName, String userName, String sshHostKey)
        {
            this.hostName = hostName;
            this.userName = userName;
            this.sshHostKey = sshHostKey;
        }

        public String HostName
        {
            get { return this.hostName; }
            set { this.hostName = value; }
        }

        public String UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public String SshHostKey
        {
            get { return this.sshHostKey; }
            set { this.sshHostKey = value; }
        }

        /// <summary>
        /// Upload the archivoLocal,
        /// if the remotePath doe'nt exist, creates it.        /// 
        /// </summary>
        /// <param name="archivoLocal">File to upload</param>
        /// <param name="remotePath">Remote Directory</param>
        /// <param name="remoteFilename">RemoteFilename</param>
        public void Upload(String archivoLocal, String remotePath, String remoteFilename)
        {
            logger.Info("archivoLocal=" + archivoLocal);
            logger.Info("remotePath=" + remotePath);
            logger.Info("remoteFilename=" + remoteFilename);
            
            #region  Abrir sesion
            Session wscpSession;
            wscpSession = new Session();
            SessionOptions sesionOptions;
            sesionOptions = new SessionOptions();

            sesionOptions.HostName = this.hostName;
            sesionOptions.UserName = this.userName;
            sesionOptions.Protocol = Protocol.Sftp;
            sesionOptions.PortNumber = 22;
            sesionOptions.SshHostKey = this.sshHostKey;
            try
            {
                wscpSession.Open(sesionOptions);
            }
            catch (WinSCP.SessionException e)
            {
                logger.Error(e.Message);
            }
            #endregion

            WinSCP.RemoteDirectoryInfo remoteDirInfo;
            if (wscpSession.Opened)
            {
                logger.Info("Sesion Iniciada.");
                #region Asegurar Carpeta Destino
                remoteDirInfo = null;
                try
                {
                    remoteDirInfo = wscpSession.ListDirectory(remotePath);
                }
                catch (WinSCP.SessionRemoteException sre)
                {
                    logger.Info("sre=" + sre.Message);
                }
                if ((remoteDirInfo == null) ||
                     (remoteDirInfo.Files == null) ||
                     (remoteDirInfo.Files.Count == 0))
                {
                    wscpSession.ExecuteCommand("mkdir " + remotePath);
                }
                #endregion
                #region Upload File
                TransferOptions transferOptions;
                transferOptions = new TransferOptions();
                transferOptions.PreserveTimestamp = true;
                transferOptions.TransferMode = TransferMode.Binary;
                TransferOperationResult transferResult;
                StringBuilder remotheFullPathFilename;
                remotheFullPathFilename = new StringBuilder();
                remotheFullPathFilename.Append(remotePath);
                remotheFullPathFilename.Append("/");
                remotheFullPathFilename.Append(remoteFilename);
                transferResult = wscpSession.PutFiles(archivoLocal, remotheFullPathFilename.ToString(), false, transferOptions);
                logger.Info("transferResult.IsSuccess=" + transferResult.IsSuccess);

                foreach (SessionRemoteException orb in transferResult.Failures)
                {
                    logger.Info("error-->" + orb.ToString());
                }
                #endregion
                wscpSession.Dispose();
                logger.Info("Sesion Finalizada.");
              
            }
        }
    }
}
