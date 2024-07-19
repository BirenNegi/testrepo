using System;

namespace SOS.Reports
{
    public class TaskTotal
    {

#region Public properties
        public String BusinessUnitIdStr { get; set; }
        public String BusinessUnitName { get; set; }

        public String ProjectIdStr { get; set; }
        public String ProjectName { get; set; }

        public String EmployeeIdStr { get; set; }
        public String EmployeeName { get; set; }
        public DateTime? EmployeeLastLogin { get; set; }

        public int ComparisonsTotalPending { get; set; }
        public int ComparisonsTotalApproved { get; set; }
        public int ContractsTotalPending { get; set; }
        public int ContractsTotalApproved { get; set; }
        public int ClientVariationsTotalPending { get; set; }
        public int ClientVariationsTotalApproved { get; set; }
        public int SeparateAccountsTotalPending { get; set; }
        public int SeparateAccountsTotalApproved { get; set; }
        public int ClaimsTotalPending { get; set; }
        public int ClaimsTotalApproved { get; set; }
        public int TransmittalsTotalPending { get; set; }
        public int TransmittalsTotalApproved { get; set; }
        public int RFIsTotalPending { get; set; }
        public int RFIsTotalApproved { get; set; }
        public int EOTsTotalPending { get; set; }
        public int EOTsTotalApproved { get; set; }
#endregion

#region Public Methods
#endregion

    }
}
