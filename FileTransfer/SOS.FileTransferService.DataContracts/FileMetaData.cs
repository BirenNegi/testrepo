using System;
using System.Runtime.Serialization;

namespace SOS.FileTransferService.DataContracts
{
    [DataContract]
    public class FileMetaData
    {
        [DataMember]
        public Boolean Exist { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Extension { get; set; }

        [DataMember]
        public DateTime CreationTime { get; set; }

        [DataMember]
        public DateTime LastAccessTime { get; set; }

        [DataMember]
        public DateTime LastWriteTime { get; set; }

        [DataMember]
        public long Length { get; set; }
    }
}
