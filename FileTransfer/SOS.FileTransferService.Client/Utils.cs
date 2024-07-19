using System;
using System.IO;
using System.ServiceModel;
using System.Runtime.Remoting.Messaging;

using SOS.FileTransferService.DataContracts;
using SOS.FileTransferService.FaultContracts;

namespace SOS.FileTransferService.Client
{
    public class Utils
    {

#region Private Members
        private delegate Byte[] GetFileDataDelegate(String[] fileName);
#endregion

#region Constants
        public static Int32 MaxBytes = 67000000;
#endregion

#region Public methods
        /// <summary>
        /// Calls the FileTransferServices and return the file data
        /// </summary>
        private static void GetFile(String[] fileName, Boolean metaDataOnly, out FileMetaData fileMetaData, out Byte[] fileData)
        {
            FileTransferServiceClient fileTransferServiceClient = new FileTransferServiceClient();

            fileData = null;
            fileMetaData = null;
            String login = null;
            String password = null;

            try
            {
                fileMetaData = fileTransferServiceClient.GetFile(fileName, ref login, metaDataOnly, ref password, out fileData);
            }
            catch (TimeoutException timeoutException)
            {
                throw new Exception(timeoutException.ToString());
            }
            catch (FaultException<GenericFault> genericFault)
            {
                throw new Exception(genericFault.ToString());
            }
            catch (FaultException faultException)
            {
                throw new Exception(faultException.ToString());
            }
            catch (CommunicationException communicationException)
            {
                throw new Exception(communicationException.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            finally
            {
                fileTransferServiceClient.Close();
            }
        }

        /// <summary>
        /// Calls the FileTransferServices and return the file data
        /// </summary>
        private static void GetFile(String fileName, Boolean metaDataOnly, out FileMetaData fileMetaData, out Byte[] fileData)
        {
            GetFile(new String[] { fileName }, metaDataOnly, out fileMetaData, out fileData);
        }
        
        /// <summary>
        /// Gets file data
        /// </summary>
        public static Byte[] GetFileData(String[] fileName)
        {
            Byte[] fileData;
            FileMetaData fileMetaData;

            GetFile(fileName, false, out fileMetaData, out fileData);

            return fileData;
        }

        public static IAsyncResult StartGetFileData(String[] fileName)
        {
            GetFileDataDelegate getFileDataDelegate = new GetFileDataDelegate(GetFileData);
            IAsyncResult iAsyncResult = getFileDataDelegate.BeginInvoke(fileName, null, null);
            return iAsyncResult;
        }

        public static Byte[] EndGetFileData(IAsyncResult iAsyncResult)
        {
            AsyncResult asyncResult = (AsyncResult)iAsyncResult;
            GetFileDataDelegate getFileDataDelegate = (GetFileDataDelegate)asyncResult.AsyncDelegate;
            return getFileDataDelegate.EndInvoke(asyncResult);
        }

        /// <summary>
        /// Gets file data
        /// </summary>
        public static Byte[] GetFileData(String fileName)
        {
            return GetFileData(new String[] { fileName });
        }

        /// <summary>
        /// Gets file meta data
        /// </summary>
        public static FileMetaData GetFileMetaData(String fileName)
        {
            try
            {
                Byte[] fileData;
                FileMetaData fileMetaData;

                GetFile(fileName, true, out fileMetaData, out fileData);

                return fileMetaData;
            }
            catch (Exception ex)
            {
                string x = ex.ToString();
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Get file data and metadata
        /// </summary>
        public static void GetFileDataAndMetaData(String fileName, out FileMetaData fileMetaData, out Byte[] fileData)
        {
            GetFile(fileName, false, out fileMetaData, out fileData);
        }

        /// <summary>
        /// Calls the FileTransferServices to save the file
        /// </summary>
        public static void PutFile(String fileName, String fileExtension, byte[] data)
        {
            FileTransferServiceClient fileTransferServiceClient = new FileTransferServiceClient();

            String login = null;
            String password = null;
            Boolean metadataOnly;

            FileMetaData fileMetaData = new FileMetaData();
            fileMetaData.Name = fileName;
            fileMetaData.Extension = fileExtension;

            try
            {
                fileTransferServiceClient.PutFile(fileMetaData, ref login, ref password, data, out metadataOnly);
            }
            catch (TimeoutException timeoutException)
            {
                throw new Exception(timeoutException.ToString());
            }
            catch (FaultException<GenericFault> genericFault)
            {
                throw new Exception(genericFault.ToString());
            }
            catch (FaultException faultException)
            {
                throw new Exception(faultException.ToString());
            }
            catch (CommunicationException communicationException)
            {
                throw new Exception(communicationException.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            finally
            {
                fileTransferServiceClient.Close();
            }
        }

        /// <summary>
        /// Calls the FileTransferServices to delete a file
        /// </summary>
        public static void DeleteFile(String filePath)
        {
            FileTransferServiceClient fileTransferServiceClient = new FileTransferServiceClient();

            String login = null;
            String password = null;
            Boolean metaDataOnly = false;

            try
            {
                fileTransferServiceClient.DeleteFile(new String[] { filePath }, login, metaDataOnly, password);
            }
            catch (TimeoutException timeoutException)
            {
                throw new Exception(timeoutException.ToString());
            }
            catch (FaultException<GenericFault> genericFault)
            {
                throw new Exception(genericFault.ToString());
            }
            catch (FaultException faultException)
            {
                throw new Exception(faultException.ToString());
            }
            catch (CommunicationException communicationException)
            {
                throw new Exception(communicationException.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            finally
            {
                fileTransferServiceClient.Close();
            }
        }

        /// <summary>
        /// Determines if the .zip will be too large
        /// </summary>
        public Boolean IsFileTooLarge(String[] fileName)
        {
            if (fileName != null && fileName.Length > 0)
            {
                foreach (String s in fileName)
                {
                }
            }

            return false;
        }


        public static void PutSubcontractorFile(string FilePath, String fileName, String fileExtension, byte[] data)
        {
            FileTransferServiceClient fileTransferServiceClient = new FileTransferServiceClient();

            String login = null;
            String password = null;
            Boolean metadataOnly;

            FileMetaData fileMetaData = new FileMetaData();
            fileMetaData.Name = fileName;
            fileMetaData.Extension = fileExtension;

            try
            {
                bool exists = Directory.Exists(FilePath);
                if (!exists)
                    Directory.CreateDirectory(FilePath);

                fileTransferServiceClient.PutFile(fileMetaData, ref login, ref password, data, out metadataOnly);
            }
            catch (TimeoutException timeoutException)
            {
                throw new Exception(timeoutException.ToString());
            }
            catch (FaultException<GenericFault> genericFault)
            {
                throw new Exception(genericFault.ToString());
            }
            catch (FaultException faultException)
            {
                throw new Exception(faultException.ToString());
            }
            catch (CommunicationException communicationException)
            {
                throw new Exception(communicationException.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            finally
            {
                fileTransferServiceClient.Close();
            }
        }
#endregion

    }
}
