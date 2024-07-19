using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class AddendumInfo : Info, IDocumentable
    {

#region Constructors
        public AddendumInfo() 
        {
        }

        public AddendumInfo(int? AddendumId)
        {
            Id = AddendumId;
        }
#endregion

#region Public properties
        public int? Number { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime? AddendumDate { get; set; }

        public TradeInfo Trade { get; set; }
        public AttachmentsGroupInfo AttachmentsGroup { set; get; }

        public String AttachmentsInfo
        {
            get { return AttachmentsGroup != null ? AttachmentsGroup.AttachmentsInfo : String.Empty; }
        }
#endregion

    }
}
