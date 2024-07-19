using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ContractInfo : Info
    {

#region Private Members
        private String template;
        private String number;
        private DateTime? writeDate;
        private DateTime? approvalDate;
        private int? subcontractNumber;
        private decimal? goodsServicesTax;
        private String siteInstruction;
        private DateTime? siteInstructionDate;
        private String subContractorReference;
        private DateTime? subContractorReferenceDate;
        private String description;
        private String comments;
        private bool? checkQuotes;
        private bool? checkWinningQuote;
        private bool? checkComparison;
        private bool? checkCheckList;
        private bool? checkPrelettingMinutes;
        private bool? checkAmendments;
        private bool? checkRetentionReq;  // DS20231108
        private String quotesFile;

        private String currentStatusName;
        private DateTime? currentStatusDate;
        private String pendingTaskName;
        private String pendingTaskPersonName;
        private DateTime? pendingTaskDueDate;
        private int? pendingTaskDueDays;

        private DateTime? startDate;  // DS20240320
        private DateTime? finishDate;  // DS20240320
        private bool? checkOrigContractDur;  // DS20240320

        private ContractInfo parentContract;
        private TradeInfo trade;
        private ProcessInfo process;
        
        private List<ContractModificationInfo> contractModifications;
        private List<ContractInfo> subcontracts;
        private List<VariationInfo> variations;
#endregion

#region Constructors
        public ContractInfo() 
        {
        }

        public ContractInfo(int? contractId)
        {
            Id = contractId;
        }
#endregion

#region Public properties
        public String Template
        {
            get { return template; }
            set { template = value; }
        }

        public String Number
        {
            get { return number; }
            set { number = value; }
        }

        public DateTime? WriteDate
        {
            get { return writeDate; }
            set { writeDate = value; }
        }

        public DateTime? ApprovalDate
        {
            get { return approvalDate; }
            set { approvalDate = value; }
        }

        public int? SubcontractNumber
        {
            get { return subcontractNumber; }
            set { subcontractNumber = value; }
        }

        public decimal? GoodsServicesTax
        {
            get { return goodsServicesTax; }
            set { goodsServicesTax = value; }
        }

        public String SiteInstruction
        {
            get { return siteInstruction; }
            set { siteInstruction = value; }
        }

        public DateTime? SiteInstructionDate
        {
            get { return siteInstructionDate; }
            set { siteInstructionDate = value; }
        }

        public String SubContractorReference
        {
            get { return subContractorReference; }
            set { subContractorReference = value; }
        }

        public DateTime? SubContractorReferenceDate
        {
            get { return subContractorReferenceDate; }
            set { subContractorReferenceDate = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public bool? CheckQuotes
        {
            get { return checkQuotes; }
            set { checkQuotes = value; }
        }

        public bool? CheckWinningQuote
        {
            get { return checkWinningQuote; }
            set { checkWinningQuote = value; }
        }

        public bool? CheckComparison
        {
            get { return checkComparison; }
            set { checkComparison = value; }
        }

        public bool? CheckCheckList
        {
            get { return checkCheckList; }
            set { checkCheckList = value; }
        }

        public bool? CheckPrelettingMinutes
        {
            get { return checkPrelettingMinutes; }
            set { checkPrelettingMinutes = value; }
        }

        public bool? CheckAmendments
        {
            get { return checkAmendments; }
            set { checkAmendments = value; }
        }
        public bool? CheckRetentionReq    // DS20231108
        {
            get { return checkRetentionReq; }
            set { checkRetentionReq = value; }
        }

        public String QuotesFile
        {
            get { return quotesFile; }
            set { quotesFile = value; }
        }

        public String CurrentStatusName
        {
            get { return currentStatusName; }
            set { currentStatusName = value; }
        }

        public DateTime? CurrentStatusDate
        {
            get { return currentStatusDate; }
            set { currentStatusDate = value; }
        }

        public String PendingTaskName
        {
            get { return pendingTaskName; }
            set { pendingTaskName = value; }
        }

        public String PendingTaskPersonName
        {
            get { return pendingTaskPersonName; }
            set { pendingTaskPersonName = value; }
        }

        public DateTime? PendingTaskDueDate
        {
            get { return pendingTaskDueDate; }
            set { pendingTaskDueDate = value; }
        }

        public int? PendingTaskDueDays
        {
            get { return pendingTaskDueDays; }
            set { pendingTaskDueDays = value; }
        }

        public ContractInfo ParentContract
        {
            get { return parentContract; }
            set { parentContract = value; }
        }
        public DateTime? StartDate  // DS20240320 >>>
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime? FinishDate
        {
            get { return finishDate; }
            set { finishDate = value; }
        }
        public bool? CheckOrigContractDur
        {
            get { return checkOrigContractDur; }
            set { checkOrigContractDur = value; }
        }                                          // DS20240320
        public TradeInfo Trade
        {
            get { return trade; }
            set { trade = value; }
        }

        public ProcessInfo Process
        {
            get { return process; }
            set { process = value; }
        }

        public List<ContractModificationInfo> ContractModifications
        {
            get { return contractModifications; }
            set { contractModifications = value; }
        }

        public List<ContractInfo> Subcontracts
        {
            get { return subcontracts; }
            set { subcontracts = value; }
        }

        public List<ContractInfo> ApprovedSubcontracts
        {
            get {
                List<ContractInfo> approvedSubcontracts = new List<ContractInfo>();

                if (Subcontracts != null)
                    foreach (ContractInfo contractInfo in Subcontracts)
                        if (contractInfo.IsApproved)
                            approvedSubcontracts.Add(contractInfo);

                return approvedSubcontracts; 
            }
        }

        public List<VariationInfo> Variations
        {
            get { return variations; }
            set { variations = value; }
        }


        public Boolean IsSubContract
        {
            get
            {
                return ParentContract != null;
            }
        }

        public Boolean IsApproved
        {
            get
            {
                return approvalDate != null;
            }
        }

        public Int32? NumVariations
        {
            get
            {
                if (Variations == null)
                    return null;
                else
                    return Variations.Count;            
            }
        }

        public Int32? NumSubContracts
        {
            get
            {
                if (Subcontracts == null)
                    return null;
                else
                    return Subcontracts.Count;
            }
        }

        public decimal? TotalVariations
        {
            get
            {
                decimal? totalVariations = 0;

                if (Variations != null)
                    foreach (VariationInfo variationInfo in Variations)
                        totalVariations = totalVariations + variationInfo.Amount;

                return totalVariations;
            }
        }

        public decimal? TotalAllocations
        {
            get
            {
                decimal? totalAllocations = 0;

                if (Variations != null)
                    foreach (VariationInfo variationInfo in Variations)
                        if (variationInfo.Allowance.HasValue)
                            totalAllocations = totalAllocations + variationInfo.Allowance.Value;

                return totalAllocations;
            }
        }

        public decimal? TotalWinLoss
        {
            get
            {
                decimal? totalWinLoss = 0;

                if (Variations != null)
                    foreach (VariationInfo variationInfo in Variations)
                        if (variationInfo.BudgetWinLoss.HasValue)
                            totalWinLoss = totalWinLoss + variationInfo.BudgetWinLoss.Value;

                return totalWinLoss;
            }
        }

        public decimal? TotalCompanyVariations
        {
            get
            {
                decimal? totalCompanyVariations = 0;

                if (Variations != null)
                    foreach (VariationInfo variationInfo in Variations)
                        if (variationInfo.Type == VariationInfo.VariationTypeCompany)
                            totalCompanyVariations = totalCompanyVariations + variationInfo.Amount;

                return totalCompanyVariations;
            }
        }

        public decimal? TotalDesignVariations
        {
            get
            {
                decimal? totalDesignVariations = 0;

                if (Variations != null)
                    foreach (VariationInfo variationInfo in Variations)
                        if (variationInfo.Type == VariationInfo.VariationTypeDesign)
                            totalDesignVariations = totalDesignVariations + variationInfo.Amount;

                return totalDesignVariations;
            }
        }

        public decimal? TotalSubbiesVariations
        {
            get
            {
                decimal? totalSubbiesVariations = 0;

                if (Variations != null)
                    foreach (VariationInfo variationInfo in Variations)
                        if (variationInfo.Type != VariationInfo.VariationTypeCompany && variationInfo.Type != VariationInfo.VariationTypeDesign)
                            totalSubbiesVariations = totalSubbiesVariations + variationInfo.Amount;

                return totalSubbiesVariations;
            }
        }

        public int NextSubContractNumber
        {
            get
            {
                if (Subcontracts != null)
                    if (Subcontracts.Count > 0)
                        if (Subcontracts[Subcontracts.Count - 1].subcontractNumber != null)
                            return (int)Subcontracts[Subcontracts.Count - 1].subcontractNumber + 1;

                return 1;
            }
        }

        public String TypeName
        {
            get { 
                return IsSubContract ? "Variation Order" : "Contract";
            }
        }

        public String ContractNumber
        {
            get
            {
                if (IsSubContract)
                    return "(" + Trade.Project.Number + ")-" + Trade.WorkOrderNumber + "/" + SubcontractNumber.ToString();
                else
                    return "(" + Trade.Project.Number + ")-" + Trade.WorkOrderNumber;
            }
        }

        public String SubcontractorShortName
        {
            get
            {
                return ParentContract != null ? ParentContract.SubcontractorShortName : Trade.SelectedSubContractorShortName;
            }
        }

        public String WorkOrderNumber
        {
            get
            {
                return ParentContract != null ? ParentContract.WorkOrderNumber : Trade.WorkOrderNumber;
            }
        }



        public String ProjectName{
            get { return Trade?.Project?.Name; }
        }
#endregion

    }
}
