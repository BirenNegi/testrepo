using System;

namespace SOS.Reports
{
    public class UserActivity
    {

#region Public properties
        public int ProjectId { get; set; }
        public String ProjectName { get; set; }

        public int UserId { get; set; }
        public String UserName { get; set; }

        public int ComparisonsApproved { get; set; }
        public int ComparisonsPending { get; set; }
        public int ContractsApproved { get; set; }
        public int ContractsPending { get; set; }
        public int ClientVariationsApproved { get; set; }
        public int ClientVariationsPending { get; set; }
        public int SeparateAccountsApproved { get; set; }
        public int SeparateAccountsPending { get; set; }
        public int ClaimsApproved { get; set; }
        public int ClaimsPending { get; set; }

        public int TransmittalsCreated { get; set; }
        public int TransmittalsFinalized { get; set; }
        public int RFIsCreated { get; set; }
        public int RFIsFinalized { get; set; }
        public int EOTsCreated { get; set; }
        public int EOTsFinalized { get; set; }
#endregion

#region Public Methods
#endregion

    }
}
