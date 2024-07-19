using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClaimInfo : Info
    {

#region Constants
        public const String StatusIssued = "Issued";
        public const String StatusDraft = "Draft";
        public const String StatusInvoiced = "Invoice";
#endregion

#region Private Members
        private int? number;
        private DateTime? writeDate;
        private DateTime? draftApprovalDate;
        private DateTime? internalApprovalDate;
        private DateTime? approvalDate;
        private DateTime? dueDate;
        private DateTime? clientDueDate;
        private decimal? goodsServicesTax;
        private decimal? adjustmentNoteAmount;
        private String adjustmentNoteName;
        private Boolean isLastClaim = false;
        private string backupFile1;  // DS20231023
        private string backupFile2;  // DS20231023
        private ProjectInfo project;
        private ProcessInfo process;
        private ClaimInfo previousClaim;

        private List<ClaimTradeInfo> claimTrades;
        private List<ClaimVariationInfo> claimVariations;
#endregion

#region Constructors
        public ClaimInfo() 
        {
        }

        public ClaimInfo(int? ClaimId)
        {
            Id = ClaimId;
        }
#endregion

#region Public properties
        public int? Number
        {
            get { return number; }
            set { number = value; }
        }

        public DateTime? WriteDate
        {
            get { return writeDate; }
            set { writeDate = value; }
        }

        public DateTime? DraftApprovalDate
        {
            get { return draftApprovalDate; }
            set { draftApprovalDate = value; }
        }

        public DateTime? InternalApprovalDate
        {
            get { return internalApprovalDate; }
            set { internalApprovalDate = value; }
        }

        public DateTime? ApprovalDate
        {
            get { return approvalDate; }
            set { approvalDate = value; }
        }

        public DateTime? DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public DateTime? ClientDueDate
        {
            get { return clientDueDate; }
            set { clientDueDate = value; }
        }

        public decimal? GoodsServicesTax
        {
            get { return goodsServicesTax; }
            set { goodsServicesTax = value; }
        }

        public decimal? AdjustmentNoteAmount
        {
            get { return adjustmentNoteAmount; }
            set { adjustmentNoteAmount = value; }
        }

        public String AdjustmentNoteName
        {
            get { return adjustmentNoteName; }
            set { adjustmentNoteName = value; }
        }

        public Boolean IsLastClaim
        {
            get { return isLastClaim; }
            set { isLastClaim = value; }
        }
        public String BackupFile1             // DS20231023 >>>
        {
            get { return backupFile1; }
            set { backupFile1 = value; }
        }
        public String BackupFile2
        {
            get { return backupFile2; }
            set { backupFile2 = value; }
        }                                      // DS20231023 <<<

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        public ProcessInfo Process
        {
            get { return process; }
            set { process = value; }
        }

        public ClaimInfo PreviousClaim
        {
            get { return previousClaim; }
            set { previousClaim = value; }
        }

        public List<ClaimTradeInfo> ClaimTrades
        {
            get { return claimTrades; }
            set { claimTrades = value; }
        }

        public List<ClaimVariationInfo> ClaimVariations
        {
            get { return claimVariations; }
            set { claimVariations = value; }
        }

		public Decimal? TotalClaimTrades
		{
			get
			{
				Decimal? totalClaimTrades = null;

				if (ClaimTrades != null)
				{
					totalClaimTrades = 0;
					foreach (ClaimTradeInfo claimTradeInfo in ClaimTrades)
						if (claimTradeInfo.Amount != null)
							totalClaimTrades += (Decimal)claimTradeInfo.Amount;
				}

				return totalClaimTrades;
			}
		}

		public Decimal? TotalClaimVariations
		{
			get
			{
				Decimal? totalClaimVariations = null;

				if (ClaimVariations != null)
				{
					totalClaimVariations = 0;
					foreach (ClaimVariationInfo claimVariationInfo in ClaimVariations)
						if (claimVariationInfo.Amount != null)
							totalClaimVariations += (Decimal)claimVariationInfo.Amount;
				}

				return totalClaimVariations;
			}
		}

		public Decimal? Total
		{
            get
            {
                Decimal? totalClaimTrades = TotalClaimTrades;
                Decimal? totalClaimVariations = TotalClaimVariations;

                if (totalClaimTrades == null && totalClaimVariations == null)
                    return null;
                else
                    return Utils.GetDecimalValue(totalClaimTrades) + Utils.GetDecimalValue(totalClaimVariations);
            }
		}

        public decimal? TotalGST
        {
            get
            {
                Decimal? total = Total;

                if (total != null)
                    if (GoodsServicesTax != null)
                        return Math.Round((Decimal)total * (Decimal)GoodsServicesTax, 2);

                return null;
            }
        }

        public decimal? TotalAmountPlusGST
        {
            get
            {
                Decimal? total = Total;
                Decimal? totalGST = TotalGST;

                if (total != null)
                    if (totalGST != null)
                        return (Decimal)total + (Decimal)totalGST;

                return null;
            }
        }

        public decimal? TotalThisClaim
        {
            get
            {
                if (previousClaim == null)
                    return Total;
                else
                {
                    decimal? totalThis = Total;
                    decimal? totalPrevious = PreviousClaim.Total;

                    if (totalThis != null)
                        if (totalPrevious != null)
                            return (Decimal)totalThis - (Decimal)totalPrevious;
                        else
                            return totalThis;
                    else
                        return null;
                }
            }
        }

        public Boolean IsDraftApproved
        {
            get { return DraftApprovalDate != null; }
        }

        public Boolean IsInternallyApproved
        {
            get { return InternalApprovalDate != null; }
        }

        public Boolean IsApproved
        {
            get { return ApprovalDate != null; }
        }

        public String Status
        {
            get
            {
                if (IsApproved) return StatusInvoiced;
                if (IsDraftApproved) return StatusDraft;

                return StatusIssued;
            }
        }

        public String ProjectName
        {
            get { return Project != null ? Project.Name : null; }
        }

        public String ProjectNumber
        {
            get { return Project != null ? Project.FullNumber : null; }
        }

        public String ProjectIdStr
        {
            get { return Project != null ? Project.IdStr : null; }
        }
#endregion

    }
}
