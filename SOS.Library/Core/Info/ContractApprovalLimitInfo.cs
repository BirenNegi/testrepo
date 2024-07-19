using System;
using System.Collections.Generic;


namespace SOS.Core
{
    [Serializable]
  public  class ContractApprovalLimitInfo :Info
    {

        #region Constructors
        public ContractApprovalLimitInfo()
        {
        }

        public ContractApprovalLimitInfo(int? ApprovalId)
        {
            Id = ApprovalId;
        }
        #endregion


        #region Public properties
        public int Min { get; set; }
        public int Max { get; set; }
        public String Range {
            get { return Min.ToString() + "-" + Max.ToString(); }
        }

        public String WinOver5 { get; set; }
        public String Winlessthan5 { get; set; }
        public String Lossless5 { get; set; }
        public String Lossbtw5to50 { get; set; }
        public String LossOver50 { get; set; }
        #endregion

    }
}
