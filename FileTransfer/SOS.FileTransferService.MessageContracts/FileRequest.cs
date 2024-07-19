using System;
using System.ServiceModel;

namespace SOS.FileTransferService.MessageContracts
{
    [MessageContract]
    public class FileRequest
    {

#region Public properties
        [MessageBodyMember]
        public String Login { get; set; }

        [MessageBodyMember]
        public String Password { get; set; }

        [MessageBodyMember]
        public String[] FilePath { get; set; }

        [MessageBodyMember]
        public Boolean MetaDataOnly { get; set; }
#endregion

    }
}
