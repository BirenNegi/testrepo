using System;

namespace SOS.Core
{
    [Serializable]
    public class SiteOrderApprovalsSearchInfo : Info
    {
        #region Public properties
        public int OrderId { get; set; }
        public string OrderTitle { get; set; }
        public string OrderTyp { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public int AssignedPeopleId { get; set; }
        public string AssignedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public Boolean Approve { get; set; }
        public Boolean Reject { get; set; }
        public string Reason { get; set; }   // DS20230304
        public Decimal SubTotal { get; set; }   // DS20230314
        public int isApprovalCurrent { get; set; }
        public DateTime OrderDateEnd {get; set; }
        public string RowColor { get; set; }
        public string ValueLimit { get; set; }
        public string BusinessUnitName { get; set; }   // DS20230620
        public string Process { get; set; }  // DS20231128

        public int DocCount { get; set; }  // DS20230710
        #endregion

        public SiteOrderApprovalsSearchInfo()
        {
        }
#region Constructors
        public SiteOrderApprovalsSearchInfo(int? ApprovalId)
        {
            Id = ApprovalId;
        }
#endregion
    }
}
