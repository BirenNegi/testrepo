using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Configuration;

using ICSharpCode.SharpZipLib.Zip;

using SOS.FileTransferService.DataContracts;
using SOS.FileTransferService.FaultContracts;
using SOS.FileTransferService.ServiceContracts;
using SOS.FileTransferService.MessageContracts;

namespace SOS.FileTransferService.ServiceImplementation
{
    public class FileTransferService : IFileTransferService
    {

#region Constants
        private Int32 MaxBytes = 67000000;
        private Int32 CompressionLevel = 0;   // 0 - store only to 9 - means best compression
#endregion

        #region Public Methods
        /// <summary>
        /// Gets a file from
        /// </summary>
        public FileResponse GetFile(FileRequest fileRequest)
        {
            String documentsFolder = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();
            FileResponse fileResponse = new FileResponse();
            FileInfo fileInfo;

            if (fileRequest.FilePath != null && fileRequest.FilePath.Length >= 1)
            {
                if (fileRequest.FilePath.Length == 1)
                {
                    if (fileRequest.FilePath[0] != null && fileRequest.FilePath[0] != String.Empty)
                    {
                        try
                        {
                            fileInfo = new FileInfo(documentsFolder + "\\" + fileRequest.FilePath[0]);
                        }
                        catch (Exception ex)
                        {
                            throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                        }

                        fileResponse.MetaData = new FileMetaData();
                        fileResponse.MetaData.Exist = fileInfo.Exists;

                        if (fileResponse.MetaData.Exist)
                        {
                            fileResponse.MetaData.Name = fileInfo.Name;
                            fileResponse.MetaData.Extension = fileInfo.Extension;
                            fileResponse.MetaData.CreationTime = fileInfo.CreationTime;
                            fileResponse.MetaData.LastAccessTime = fileInfo.LastAccessTime;
                            fileResponse.MetaData.LastWriteTime = fileInfo.LastWriteTime;
                            fileResponse.MetaData.Length = fileInfo.Length;

                            if (fileRequest.MetaDataOnly)
                            {
                                fileResponse.data = null;
                            }
                            else
                            {
                                try
                                {
                                    fileResponse.data = File.ReadAllBytes(fileInfo.FullName);
                                }
                                catch (Exception ex)
                                {
                                    throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new FaultException<GenericFault>(new GenericFault(), "File not specified.");
                    }
                }
                else
                {
                    Int32 totalBytes = 0;
                    ZipEntry zipEntry;
                    Byte[] fileData;

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (ZipOutputStream zipOutputStream = new ZipOutputStream(memoryStream))
                        {
                            zipOutputStream.SetLevel(CompressionLevel);

                            for (int i = 0; i < fileRequest.FilePath.Length; i++)
                            {
                                if (fileRequest.FilePath[i] != null && fileRequest.FilePath[i] != String.Empty)
                                {
                                    try
                                    {
                                        fileInfo = new FileInfo(documentsFolder + "\\" + fileRequest.FilePath[i]);

                                        if (fileInfo.Exists)
                                        {
                                            fileData = File.ReadAllBytes(fileInfo.FullName);

                                            totalBytes = totalBytes + fileData.Length;
                                            if (totalBytes > MaxBytes)
                                                throw new Exception("Maximum file size reached.");

                                            zipEntry = new ZipEntry(fileInfo.Name);
                                            zipEntry.Size = fileData.Length;
                                            zipEntry.DateTime = fileInfo.LastWriteTime;
                                            zipEntry.CompressionMethod = CompressionMethod.Deflated;

                                            zipOutputStream.PutNextEntry(zipEntry);
                                            zipOutputStream.Write(fileData, 0, fileData.Length);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                                    }
                                }
                            }
                        }

                        fileResponse.data = memoryStream.ToArray();
                    }

                    fileResponse.MetaData = new FileMetaData();
                    fileResponse.MetaData.Exist = true;
                    fileResponse.MetaData.Name = "CompressedFile";
                    fileResponse.MetaData.Extension = ".zip";
                    fileResponse.MetaData.CreationTime = DateTime.Now;
                    fileResponse.MetaData.LastAccessTime = DateTime.Now;
                    fileResponse.MetaData.LastWriteTime = DateTime.Now;
                    fileResponse.MetaData.Length = fileResponse.data.Length;
                }
            }

            return fileResponse;
        }

        /// <summary>
        /// Puts a file
        /// </summary>
        public FileRequest PutFile(FileResponse fileResponse)
        {
            String documentsFolder = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();
            FileRequest fileRequest = null;
            FileStream fileStream = null;
            BinaryWriter binaryWriter = null;
            FileInfo fileInfo;

            if (fileResponse.MetaData.Name != null && fileResponse.MetaData.Name != String.Empty)
            {
                if (fileResponse.data != null && fileResponse.data.Length > 0)
                {
                    try
                    {
                        fileInfo = new FileInfo(String.Format("{0}\\{1}", documentsFolder, fileResponse.MetaData.Name));
                    }
                    catch (Exception ex)
                    {
                        throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                    }

                    if (!fileInfo.Exists)
                    {
                        try
                        {
                            fileRequest = new FileRequest();
                            fileRequest.FilePath = new String[] { fileInfo.FullName };

                            fileStream = new FileStream(fileInfo.FullName, FileMode.CreateNew);

                            binaryWriter = new BinaryWriter(fileStream);
                            binaryWriter.Write(fileResponse.data);
                            binaryWriter.Close();

                            fileStream.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                        }
                        finally
                        {
                            if (binaryWriter != null)
                                binaryWriter.Close();

                            if (fileStream != null)
                                fileStream.Close();
                        }
                    }
                    else
                    {
                        throw new FaultException<GenericFault>(new GenericFault(), "File already exist.");
                    }
                }
                else
                {
                    throw new FaultException<GenericFault>(new GenericFault(), "File has not content.");
                }
            }
            else
            {
                throw new FaultException<GenericFault>(new GenericFault(), "File not specified.");
            }

            return fileRequest;
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        public void DeleteFile(FileRequest fileRequest)
        {
            String documentsFolder = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();
            FileInfo fileInfo;

            if (fileRequest.FilePath != null && fileRequest.FilePath.Length == 1 && fileRequest.FilePath[0] != null && fileRequest.FilePath[0] != String.Empty)
            {
                try
                {
                    fileInfo = new FileInfo(String.Format("{0}\\{1}", documentsFolder, fileRequest.FilePath[0]));
                }
                catch (Exception ex)
                {
                    throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                }

                if (fileInfo.Exists)
                {
                    try
                    {
                        fileInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        throw new FaultException<GenericFault>(new GenericFault(), ex.ToString());
                    }
                }
                else
                {
                    throw new FaultException<GenericFault>(new GenericFault(), "File not found.");
                }
            }
            else
            {
                throw new FaultException<GenericFault>(new GenericFault(), "File not specified.");
            }
        }
#endregion

    }
}
