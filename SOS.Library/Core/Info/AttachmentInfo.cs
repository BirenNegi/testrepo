using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class AttachmentInfo : Info
    {

#region Constants
        public const String File = "FIL";
        public const String Quotes = "QUO";
        public const String Preletting = "PRL";
        public const String Backup = "BAK";
        public const String Approval = "APP";
        public const String Drawing = "DWG";
#endregion

#region Constructors
        public AttachmentInfo() 
        {
        }

        public AttachmentInfo(int? AttachmentId)
        {
            Id = AttachmentId;
        }
#endregion

#region Public properties
        public String Name { get; set; }
        public String Description { get; set; }
        public String FilePath { get; set; }
        public DateTime? AttachDate { get; set; }
        public AttachmentsGroupInfo AttachmentsGroup { get; set; }
#endregion

    }
}
