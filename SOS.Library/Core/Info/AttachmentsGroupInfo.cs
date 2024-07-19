using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class AttachmentsGroupInfo : Info
    {

#region Constructors
        public AttachmentsGroupInfo() 
        {
        }

        public AttachmentsGroupInfo(int? AttachmentsGroupId)
        {
            Id = AttachmentsGroupId;
        }
#endregion

#region Public properties
        public List<AttachmentInfo> Attachments { get; set; }

        public String AttachmentsInfo
        {
            get { return Attachments != null ? UI.Utils.SetFormIntegerNoZero(Attachments.Count) : String.Empty; }
        }
#endregion

    }
}
