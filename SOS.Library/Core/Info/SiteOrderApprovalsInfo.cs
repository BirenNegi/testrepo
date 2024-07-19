using System;

namespace SOS.Core
{
    [Serializable]
    public class SiteOrderApprovalsInfo : Info
    {
        #region Public properties
        public int OrderId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public int AssignedPeopleId { get; set; }
        public DateTime AssignedDate { get; set; }
        public int ApprovedPeopleId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string RoleId { get; set; }
        public string Process { get; set; }
        public string ApprovalStatus { get; set; }
        public int Seq { get; set; }
        public int isApprovalCurrent { get; set; }
        public string Reason { get; set; }            // DS20230301
        #endregion

        public SiteOrderApprovalsInfo()
        {
        }
#region Constructors
        public SiteOrderApprovalsInfo(int? ApprovalId)
        {
            Id = ApprovalId;
        }
#endregion
    }
}
