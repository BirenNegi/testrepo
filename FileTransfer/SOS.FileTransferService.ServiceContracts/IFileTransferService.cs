using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using SOS.FileTransferService.MessageContracts;
using SOS.FileTransferService.FaultContracts;

namespace SOS.FileTransferService.ServiceContracts
{
    [ServiceContract]
    public interface IFileTransferService
    {

#region Public Methods
        [OperationContract]
        [FaultContract(typeof(GenericFault))]
        FileResponse GetFile(FileRequest fileRequest);

        [OperationContract]
        [FaultContract(typeof(GenericFault))]
        FileRequest PutFile(FileResponse fileResponse);

        [OperationContract]
        [FaultContract(typeof(GenericFault))]
        void DeleteFile(FileRequest fileRequest);
#endregion

    }
}
