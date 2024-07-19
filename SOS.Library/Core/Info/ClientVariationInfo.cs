using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClientVariationInfo : Info
    {

        #region Constants
        public const String VariationTypeClient = "CV";
        public const String VariationTypeSeparateAccounts = "SA";
        public const String VariationTypeTenant = "TV";
        public const String StatusCancelled = "Cancelled";
        public const String StatusPaid = "Paid";
        public const String StatusInvoiced = "Invoiced";
        public const String StatusWorksCompleted = "Works Completed";
        public const String StatusApproved = "Approved";
        public const String StatusVerballyApproved = "Verbally Approved";
        public const String StatusToBeApproved = "To be Approved";
        public const String StatusTobeIssued = "To be Issued";
        public const String FileTypeQuotes = "QF";
        public const String FileTypeBackup = "BF";
        public const String FileTypeClientApproval = "CF";
        #endregion

        #region Private Members
        private String name;
        private int? number;
        private decimal? goodsServicesTax;
        private DateTime? writeDate;
        private DateTime? internalApprovalDate;
        private DateTime? approvalDate;
        private DateTime? verbalApprovalDate;
        private DateTime? cancelDate;
        private String quotesFile;
        private String backupFile;
        private String clientApprovalFile;
        private Boolean? hideCostDetails;
        private String revisionName;
        private String comments;

        private ProjectInfo project;
        private ProcessInfo process;
        private ClientVariationInfo parentClientVariation;
        private VariationInfo variation;

        private List<ClientVariationItemInfo> items;
        private List<ClientVariationTradeInfo> trades;
        private List<ClientVariationInfo> subClientVariations;
        #endregion

        #region Constructors
        public ClientVariationInfo()
        {
            Type = ClientVariationInfo.VariationTypeClient;
        }

        public ClientVariationInfo(int? clientVariationId)
        {
            Type = ClientVariationInfo.VariationTypeClient;
            Id = clientVariationId;
        }
        #endregion

        #region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int? Number
        {
            get { return number; }
            set { number = value; }
        }

        public decimal? GoodsServicesTax
        {
            get { return goodsServicesTax; }
            set { goodsServicesTax = value; }
        }

        public DateTime? InternalApprovalDate
        {
            get { return internalApprovalDate; }
            set { internalApprovalDate = value; }
        }

        public DateTime? WriteDate
        {
            get { return writeDate; }
            set { writeDate = value; }
        }

        public DateTime? VerbalApprovalDate
        {
            get { return verbalApprovalDate; }
            set { verbalApprovalDate = value; }
        }

        public DateTime? CancelDate
        {
            get { return TheParentClientVariation.cancelDate; }
            set { TheParentClientVariation.cancelDate = value; }
        }

        public DateTime? ApprovalDate
        {
            get { return approvalDate; }
            set { approvalDate = value; }
        }

        public String QuotesFile
        {
            get { return quotesFile; }
            set { quotesFile = value; }
        }

        public String BackupFile
        {
            get { return backupFile; }
            set { backupFile = value; }
        }

        public String ClientApprovalFile
        {
            get { return clientApprovalFile; }
            set { clientApprovalFile = value; }
        }

        public Boolean? HideCostDetails
        {
            get { return hideCostDetails; }
            set { hideCostDetails = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public Boolean? ShowCostDetails
        {
            get { return hideCostDetails == null ? null : !HideCostDetails; }
        }

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

        public ClientVariationInfo ParentClientVariation
        {
            get { return parentClientVariation; }
            set { parentClientVariation = value; }
        }

        public VariationInfo Variation
        {
            get { return variation; }
            set { variation = value; }
        }

        public List<ClientVariationItemInfo> Items
        {
            get { return items; }
            set { items = value; }
        }

        public List<ClientVariationTradeInfo> Trades
        {
            get { return trades; }
            set { trades = value; }
        }

        public List<ClientVariationInfo> SubClientVariations
        {
            get { return subClientVariations; }
            set { subClientVariations = value; }
        }

        public String RevisionName
        {
            get { return revisionName; }
            set { revisionName = value; }
        }

        public String NumberAndRevisionName
        {
            get
            {
                if (Number != null)
                    if (RevisionName != null)
                        return Number.ToString() + RevisionName;
                    else
                        return Number.ToString();
                else
                    if (RevisionName != null)
                    return "?" + RevisionName;
                else
                    return null;
            }
        }

        public virtual String ProcessType
        {
            get { return ProcessTemplateInfo.ProcessTypeClientVariation; }
        }

        public Boolean IsSubClienVariation
        {
            get { return ParentClientVariation != null; }
        }

        public Boolean IsApproved
        {
            get { return ApprovalDate != null; }
        }

        public Boolean IsVerballyApproved
        {
            get { return VerbalApprovalDate != null; }
        }

        public Boolean IsInternallyApproved
        {
            get { return InternalApprovalDate != null; }
        }

        public Boolean IsLastSubClientVariation
        {
            get { return this.Equals(LastSubClientVariation); }
        }

        public Boolean IsCancel
        {
            get { return CancelDate != null; }
        }

        public virtual String Status
        {
            get
            {
                if (IsCancel)
                    return StatusCancelled;
                else if (IsApproved)
                    return StatusApproved;
                else if (IsVerballyApproved)
                    return StatusVerballyApproved;
                else if (IsInternallyApproved)
                    return StatusToBeApproved;
                else
                    return StatusTobeIssued;
            }
        }

        public int StatusOrder
        {
            get
            {
                switch (Status)
                {
                    case StatusTobeIssued: return 10;
                    case StatusToBeApproved: return 20;
                    case StatusVerballyApproved: return 30;
                    case StatusApproved: return 40;
                    case StatusCancelled: return 50;
                    default: return 100;
                }
            }
        }

        public Boolean IsTheParentClientVariation
        {
            get { return this.ParentClientVariation == null; }
        }

        public ClientVariationInfo TheParentClientVariation
        {
            get
            {
                if (ParentClientVariation == null)
                    return this;
                else
                    return ParentClientVariation;
            }
        }

        public int? NumSubClientVariations
        {
            get { return SubClientVariations == null ? null : (int?)SubClientVariations.Count; }
        }

        public decimal? TotalAmount
        {
            get
            {
                if (Status == StatusCancelled)
                    return null;
                else
                    if (Items == null)
                    return null;
                else
                {
                    Decimal totalAmount = 0;

                    foreach (ClientVariationItemInfo clientVariationItemInfo in Items)
                        if (clientVariationItemInfo.Amount != null)
                            totalAmount += (Decimal)clientVariationItemInfo.Amount;

                    return totalAmount;
                }
            }
        }

        public decimal? TotalTrades
        {
            get
            {
                if (Trades == null)
                    return null;
                else
                {
                    Decimal totalTrades = 0;

                    foreach (ClientVariationTradeInfo clientVariationTradeInfo in Trades)
                        if (clientVariationTradeInfo.Amount != null)
                            totalTrades += (Decimal)clientVariationTradeInfo.Amount;

                    return totalTrades;
                }
            }
        }

        public decimal? TotalGST
        {
            get
            {
                Decimal? totalAmount = TotalAmount;

                if (totalAmount != null)
                    if (GoodsServicesTax != null)
                        return Math.Round((Decimal)totalAmount * (Decimal)GoodsServicesTax, 2);

                return null;
            }
        }

        public decimal? TotalAmountPlusGST
        {
            get
            {
                Decimal? totalAmount = TotalAmount;
                Decimal? totalGST = TotalGST;

                if (totalAmount != null)
                    if (totalGST != null)
                        return (Decimal)totalAmount + (Decimal)totalGST;

                return null;
            }
        }

        public decimal? ItemsMinusTrades
        {
            get
            {
                Decimal? totalAmount = TotalAmount;
                Decimal? totalTrades = TotalTrades;

                if (totalAmount != null)
                    if (totalTrades != null)
                        return totalAmount - totalTrades;

                return null;
            }
        }

        public ClientVariationInfo LastSubClientVariation
        {
            get
            {
                if (TheParentClientVariation.SubClientVariations == null || TheParentClientVariation.SubClientVariations.Count == 0)
                    return TheParentClientVariation;
                else
                    return TheParentClientVariation.SubClientVariations[TheParentClientVariation.SubClientVariations.Count - 1];
            }
        }

        public String ProjectName
        {
            get
            {
                return Project != null ? Project.Name : null;
            }
        }

        public String ProjectNumber
        {
            get
            {
                return Project != null ? Project.FullNumber : null;
            }
        }

        public String ProjectPrincipal
        {
            get
            {
                return Project != null ? Project.Principal : null;
            }
        }

        public String ProjectManager
        {
            get
            {
                return Project != null || Project.ProjectManager != null ? Project.ProjectManager.Name : null;
            }
        }
        #endregion

    }
}
