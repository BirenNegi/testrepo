using System;
using System.IO;
using System.ServiceModel;

using SOS.FileTransferService.DataContracts;

namespace SOS.FileTransferService.MessageContracts
{
    [MessageContract]
    public class FileResponse
    {
        [MessageBodyMember]
        public String Login { get; set; }

        [MessageBodyMember]
        public String Password { get; set; }

        [MessageHeader]
        public FileMetaData MetaData;

        [MessageBodyMember]
        public Byte[] data;
    }
}
