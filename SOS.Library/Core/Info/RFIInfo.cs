using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class RFIInfo : Info
    {

#region Constants
        public const String StatusNew = "N";
        public const String StatusSent = "S";
        public const String StatusReplied = "R";
        public const String StatusCancel = "C";
        public const String FileTypeReference = "REF";
        public const String FileTypeResponse = "RES";
#endregion

#region Private Members
#endregion

#region Constructors
        public RFIInfo() 
        {
        }

        public RFIInfo(int? RFIId)
        {
            Id = RFIId;
        }
#endregion

#region Public properties
        public int? Number { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public String Status { get; set; }
        public DateTime? RaiseDate { get; set; }
        public DateTime? TargetAnswerDate { get; set; }
        public DateTime? ActualAnswerDate { get; set; }
        public String ReferenceFile { get; set; }
        public String ClientResponseFile { get; set; }
        public String ClientResponseSummary { get; set; }

        public PeopleInfo Signer { get; set; }
        public ProjectInfo Project { get; set; }

        
        public Boolean IsOverdue
        {
            get
            {
                return TargetAnswerDate != null && ActualAnswerDate == null && ((DateTime)TargetAnswerDate).CompareTo(DateTime.Now) < 0;
            }
        }
#endregion

    }
}
