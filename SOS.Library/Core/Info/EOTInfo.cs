using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class EOTInfo : Info
    {

        #region Constants

        public const String TypeNod = "NOD";
        public const String TypeEot = "EOT";
        #endregion

        #region Private Members
        #endregion

        #region Constructors
        public EOTInfo() 
        {
        }

        public EOTInfo(int? EOTId)
        {
            Id = EOTId;
        }
#endregion

#region Public properties
        public int? Number { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? FirstNoticeDate { get; set; }
        public DateTime? WriteDate { get; set; }
        public DateTime? SendDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public float? DaysClaimed { get; set; }
        public float? DaysApproved { get; set; }
        public String Cause { get; set; }
        public String Nature { get; set; }
        public String Period { get; set; }
        public String Works { get; set; }
        public String CostCode { get; set; }
        public String Status { get; set; }
        public String ClientApprovalFile { get; set; }
          //#--
        public String ClientBackuplFile { get; set; }
        public String TypeofEot { get; set; }

        //#---

        public ProjectInfo Project { get; set; }

        public String DelayPeriod
        {
            get { return UI.Utils.SetFormDate(StartDate) + " - " + UI.Utils.SetFormDate(EndDate); }
        }

        public bool IsClaim
        {
            get { return DaysClaimed != null; }
        }

        
#endregion

    }
}
