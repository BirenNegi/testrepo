using System;
using System.Collections.Generic;
using System.Linq;

namespace SOS.Core
{
    [Serializable]
    public class TradeInfo : Info, ISortable
    {

#region Constants
        public const Int32 FlagRed = 1;
        public const Int32 FlagGreen = 2;
        public const String marginTradeCode = "19.01.01";
#endregion
        
#region Private Members
        private String name;
        private String code;
        private bool? tenderRequired;
        private String description;
        private String scopeHeader;
        private String scopeFooter;
        private int? displayOrder;
        private int? daysFromPCD;
        private DateTime? invitationDate;
        private DateTime? dueDate;
        private DateTime? comparisonDueDate;
        private DateTime? contractDueDate;
        private String contractType;
        private DateTime? comparisonApprovalDate;
        private decimal? comparisonApprovalAmount;
        private DateTime? commencementDate;
        private DateTime? completionDate;
        private String workOrderNumber;
        private int? comparisonDueDays;
        private int? contractDueDays;
        private String quotesFile;
        private String prelettingFile;
        //#----------
        private String signedContractFile;

        //#---------

        private int? flag;

        private decimal? totalBudget;
        private decimal? totalSelectedQuote;
        private decimal? totalCompanyVariations;
        private decimal? totalDesignVariations;
        private decimal? totalSubbiesVariations;

        private ProjectInfo project;
        private JobTypeInfo jobType;
        private ProcessInfo process;
        private TradeTemplateInfo tradeTemplate;
        private ContractInfo contract;
        private EmployeeInfo contractsAdministrator;
        private EmployeeInfo projectManager;

        private List<DrawingTypeInfo> drawingTypes;
        private List<DrawingInfo> drawings;
        private List<TradeItemCategoryInfo> itemCategories;
        private List<TradeParticipationInfo> tradeParticipations;
        private List<AddendumInfo> addendums;
        private List<TradeBudgetInfo> tradeBudgets;
#endregion

#region Constructors
        public TradeInfo()
        {
        }

        public TradeInfo(int? tradeId)
        {
            Id = tradeId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Code
        {
            get { return code; }
            set { code = value; }
        }

        public bool? TenderRequired
        {
            get { return tenderRequired; }
            set { tenderRequired = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String ScopeHeader
        {
            get { return scopeHeader; }
            set { scopeHeader = value; }
        }

        public String ScopeFooter
        {
            get { return scopeFooter; }
            set { scopeFooter = value; }
        }

        public int? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public int? DaysFromPCD
        {
            get { return daysFromPCD; }
            set { daysFromPCD = value; }
        }

        public DateTime? InvitationDate
        {
            get { return invitationDate; }
            set { invitationDate = value; }
        }

        public DateTime? DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public DateTime? ComparisonDueDate
        {
            get { return comparisonDueDate; }
            set { comparisonDueDate = value; }
        }

        public DateTime? ContractDueDate
        {
            get { return contractDueDate; }
            set { contractDueDate = value; }
        }

        public String ContractType
        {
            get { return contractType; }
            set { contractType = value; }
        }

        public DateTime? ComparisonApprovalDate
        {
            get { return comparisonApprovalDate; }
            set { comparisonApprovalDate = value; }
        }

        public decimal? ComparisonApprovalAmount
        {
            get { return comparisonApprovalAmount; }
            set { comparisonApprovalAmount = value; }
        }

        public DateTime? CommencementDate
        {
            get { return commencementDate; }
            set { commencementDate = value; }
        }

        public DateTime? CompletionDate
        {
            get { return completionDate; }
            set { completionDate = value; }
        }

        public String WorkOrderNumber
        {
            get { return workOrderNumber; }
            set { workOrderNumber = value; }
        }

        public int? ComparisonDueDays
        {
            get { return comparisonDueDays; }
            set { comparisonDueDays = value; }
        }

        public int? ContractDueDays
        {
            get { return contractDueDays; }
            set { contractDueDays = value; }
        }

        public String QuotesFile
        {
            get { return quotesFile; }
            set { quotesFile = value; }
        }

        public String PrelettingFile
        {
            get { return prelettingFile; }
            set { prelettingFile = value; }
        }


        //#----------------------

        public String SignedContractFile
        {
            get { return signedContractFile; }
            set { signedContractFile = value; }
        }

        //#----------------------


        public int? Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public decimal? TotalBudget
        {
            get { return totalBudget; }
            set { totalBudget = value; }
        }

        public decimal? TotalSelectedQuote
        {
            get { return totalSelectedQuote; }
            set { totalSelectedQuote = value; }
        }

        public decimal? TotalWinLoss
        {
            get
            {
                if (totalBudget != null && totalSelectedQuote != null && TotalCompanyVariations != null && TotalDesignVariations != null)
                    return totalBudget - totalSelectedQuote - TotalCompanyVariations - TotalDesignVariations;

                return null;
            }
        }

        public decimal? TotalCompanyVariations
        {
            get { return totalCompanyVariations; }
            set { totalCompanyVariations = value; }
        }

        public decimal? TotalDesignVariations
        {
            get { return totalDesignVariations; }
            set { totalDesignVariations = value; }
        }

        public decimal? TotalSubbiesVariations
        {
            get { return totalSubbiesVariations; }
            set { totalSubbiesVariations = value; }
        }

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }
        
        public JobTypeInfo JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }

        public ProcessInfo Process
        {
            get { return process; }
            set { process = value; }
        }

        public TradeTemplateInfo TradeTemplate
        {
            get { return tradeTemplate; }
            set { tradeTemplate = value; }
        }

        public ContractInfo Contract
        {
            get { return contract; }
            set { contract = value; }
        }

        public ProcessInfo ContractProcess
        {
            get { return contract != null ? contract.Process : null; }
        }

        public EmployeeInfo ContractsAdministrator
        {
            get { return contractsAdministrator; }
            set { contractsAdministrator = value; }
        }

        public EmployeeInfo ProjectManager
        {
            get { return projectManager; }
            set { projectManager = value; }
        }

        public List<DrawingTypeInfo> DrawingTypes
        {
            get { return drawingTypes; }
            set { drawingTypes = value; }
        }
        
        /// <summary>
        /// Drawings that are exculded from the project
        /// </summary>
        public List<DrawingInfo> Drawings
        {
            get { return drawings; }
            set { drawings = value; }
        }

        public List<TradeItemCategoryInfo> ItemCategories
        {
            get { return itemCategories; }
            set { itemCategories = value; }
        }

        public List<TradeParticipationInfo> Participations
        {
            get { return tradeParticipations; }
            set { tradeParticipations = value; }
        }

        public List<AddendumInfo> Addendums
        {
            get { return addendums; }
            set { addendums = value; }
        }

        public List<TradeBudgetInfo> TradeBudgets
        {
            get { return tradeBudgets; }
            set { tradeBudgets = value; }
        }

        /// <summary>
        /// Total budget for comparison
        /// </summary>
        public Decimal TotalBudgetAllowance
        {
            get
            {
                Decimal totalBudgetAllowance = 0;

                if (tradeBudgets != null)
                    foreach (TradeBudgetInfo tradeBudgetInfo in tradeBudgets)
                        totalBudgetAllowance += tradeBudgetInfo.BudgetAmountAllowance == null ? 0 : tradeBudgetInfo.BudgetAmountAllowance.Value;

                return totalBudgetAllowance;
            }
        }

        /// <summary>
        /// Total non negative budget original
        /// </summary>
        public Decimal TotalNonNegativeBudgetAmountInitial
        {
            get
            {
                Decimal? nonNegativeBudgetAmountInitial;
                Decimal totalNonNegativeBudgetAmountInitial = 0;

                if (tradeBudgets != null)
                {
                    foreach (TradeBudgetInfo tradeBudgetInfo in tradeBudgets)
                    {
                        nonNegativeBudgetAmountInitial = tradeBudgetInfo.BudgetAmountInitial != null && tradeBudgetInfo.BudgetAmountInitial.Value < 0 ? 0 : tradeBudgetInfo.BudgetAmountInitial;
                        totalNonNegativeBudgetAmountInitial += nonNegativeBudgetAmountInitial == null ? 0 : nonNegativeBudgetAmountInitial.Value;
                    }
                }

                return totalNonNegativeBudgetAmountInitial;
            }
        }

        /// <summary>
        /// Return the list of different trade codes for all trade budgets
        /// </summary>
        public Dictionary<String, Decimal> TradeCodes
        {
            get
            {
                Dictionary<String, Decimal> tradeCodes = new Dictionary<String, Decimal>();

                if (TradeBudgets != null)
                    foreach (TradeBudgetInfo tradeBudgetInfo in TradeBudgets)
                        if (tradeCodes.ContainsKey(tradeBudgetInfo.TradeCode))
                            tradeCodes[tradeBudgetInfo.TradeCode] += tradeBudgetInfo.BudgetAmount;
                        else
                            tradeCodes.Add(tradeBudgetInfo.TradeCode, tradeBudgetInfo.BudgetAmount);

                return tradeCodes;
            }
        }

        /// <summary>
        /// Total budget for contract.
        /// </summary>
        public Decimal TotalBudgetAmount
        {
            get
            {
                Decimal totalBudgetAmount = 0;

                if (tradeBudgets != null)
                {
                    foreach (TradeBudgetInfo tradeBudgetInfo in tradeBudgets)
                    {
                        totalBudgetAmount += tradeBudgetInfo.Amount == null ? 0 : tradeBudgetInfo.Amount.Value;
                    }
                }

                return totalBudgetAmount;
            }
        }

        public Boolean IsUsingBudgetModule
        {
            get { return tradeBudgets != null && tradeBudgets.Count > 0; }
        }

        public String JobTypeName
        {
            get { return JobType == null ? null : JobType.Name; }
        }

        public List<DrawingInfo> IncludedDrawings
        {
            get
            {
                List<DrawingInfo> includedDrawings = new List<DrawingInfo>();

                if (Project.Drawings != null)
                    foreach (DrawingInfo drawingInfo in Project.Drawings)
                        if (DrawingTypes.Find(delegate(DrawingTypeInfo drawingTypeInfoInList) { return drawingTypeInfoInList.Equals(drawingInfo.DrawingType); }) != null)
                            if (Drawings.Find(delegate(DrawingInfo drawingInfoInList) { return drawingInfoInList.Equals(drawingInfo); }) == null)
                                includedDrawings.Add(drawingInfo);

                return includedDrawings;
            }
        }

        public List<DrawingInfo> IncludedDrawingsActive
        {
            get
            {
                List<DrawingInfo> includedDrawings = new List<DrawingInfo>();

                if (Project.Drawings != null)
                    foreach (DrawingInfo drawingInfo in Project.Drawings)
                        if (drawingInfo.IsActive)
                            if (DrawingTypes.Find(delegate(DrawingTypeInfo drawingTypeInfoInList) { return drawingTypeInfoInList.Equals(drawingInfo.DrawingType); }) != null)
                                if (Drawings.Find(delegate(DrawingInfo drawingInfoInList) { return drawingInfoInList.Equals(drawingInfo); }) == null)
                                    includedDrawings.Add(drawingInfo);

                return includedDrawings;
            }
        }

        public List<DrawingInfo> IncludedDrawingsProposal
        {
            get
            {
                List<DrawingInfo> includedDrawings = new List<DrawingInfo>();

                if (Project.Drawings != null)
                    foreach (DrawingInfo drawingInfo in Project.Drawings)
                        if (drawingInfo.IsProposal)
                            if (DrawingTypes.Find(delegate(DrawingTypeInfo drawingTypeInfoInList) { return drawingTypeInfoInList.Equals(drawingInfo.DrawingType); }) != null)
                                if (Drawings.Find(delegate(DrawingInfo drawingInfoInList) { return drawingInfoInList.Equals(drawingInfo); }) == null)
                                    includedDrawings.Add(drawingInfo);

                return includedDrawings;
            }
        }

        public List<TradeParticipationInfo> SubcontractorsParticipations
        {
            get {
                List<TradeParticipationInfo> subcontractorsParticipations = new List<TradeParticipationInfo>();

                foreach (TradeParticipationInfo tradeParticipationInfo in Participations)
                    if (tradeParticipationInfo.SubContractor != null)
                        subcontractorsParticipations.Add(tradeParticipationInfo);

                return subcontractorsParticipations; 
            }
        }

        public List<TradeParticipationInfo> SubcontractorsActiveParticipations
        {
            get
            {
                List<TradeParticipationInfo> subcontractorsActiveParticipations = new List<TradeParticipationInfo>();

                foreach (TradeParticipationInfo tradeParticipationInfo in ActiveParticipations)
                    if (tradeParticipationInfo.SubContractor != null)
                        subcontractorsActiveParticipations.Add(tradeParticipationInfo);

                return subcontractorsActiveParticipations;
            }
        }

        public List<TradeParticipationInfo> SubcontractorsProposalParticipations
        {
            get
            {
                List<TradeParticipationInfo> subcontractorsProposalParticipations = new List<TradeParticipationInfo>();

                foreach (TradeParticipationInfo tradeParticipationInfo in ProposalParticipations)
                    if (tradeParticipationInfo.SubContractor != null)
                        subcontractorsProposalParticipations.Add(tradeParticipationInfo);

                return subcontractorsProposalParticipations;
            }
        }

        public TradeParticipationInfo BudgetParticipation
        {
            get
            {
                if (Participations != null)
                    foreach (TradeParticipationInfo tradeParticipationInfo in Participations)
                        if (tradeParticipationInfo.SubContractor == null)
                            return tradeParticipationInfo;

                return null;
            }
        }

        public TradeParticipationInfo SelectedParticipation
        {
            get
            {
                if (Participations != null)
                    foreach (TradeParticipationInfo tradeParticipationInfo in Participations)
                        if (tradeParticipationInfo.Rank != null)
                            if (tradeParticipationInfo.Rank == 1)
                                if (!tradeParticipationInfo.IsPulledOut)
                                    return tradeParticipationInfo;

                return null;
            }
        }

        public Boolean AllRanksAssigned
        {
            get
            {
                List<TradeParticipationInfo> subcontractorsParticipations = SubcontractorsParticipations;

                if (subcontractorsParticipations != null)
                    foreach (TradeParticipationInfo tradeParticipationInfo in subcontractorsParticipations)
                        if (!tradeParticipationInfo.IsPulledOut)
                            if (tradeParticipationInfo.Rank == null)
                                return false;

                return true;
            }
        }

        public List<TradeParticipationInfo> ActiveParticipations
        {
            get
            {
                List<TradeParticipationInfo> activeParticipations = new List<TradeParticipationInfo>();

                if (Participations != null)
                    foreach (TradeParticipationInfo tradeParticipationInfo in Participations)
                        if (tradeParticipationInfo.IsActive)
                            activeParticipations.Add(tradeParticipationInfo);

                return activeParticipations;
            }
        }

        public List<TradeParticipationInfo> ProposalParticipations
        {
            get
            {
                List<TradeParticipationInfo> proposalParticipations = new List<TradeParticipationInfo>();

                if (Participations != null)
                    foreach (TradeParticipationInfo tradeParticipationInfo in Participations)
                        if (tradeParticipationInfo.IsProposal)
                            proposalParticipations.Add(tradeParticipationInfo);

                return proposalParticipations;
            }
        }

        public SubContractorInfo SelectedSubContractor
        {
            get
            {
                TradeParticipationInfo selectedParticipation = SelectedParticipation;
                return selectedParticipation != null ? selectedParticipation.SubContractor : null;
            }
        }

        public String SelectedSubContractorName
        {
            get
            {
                SubContractorInfo selectedSubContractor = SelectedSubContractor;
                return selectedSubContractor != null ? selectedSubContractor.Name : String.Empty;
            }
        }

        public String SelectedSubContractorShortName
        {
            get
            {
                SubContractorInfo selectedSubContractor = SelectedSubContractor;
                return selectedSubContractor != null ? selectedSubContractor.ShortName : String.Empty;
            }
        }

        public bool ComparisonApproved
        {
            get
            {
                return ComparisonApprovalDate != null;
            }
        }

        public bool ContractCreated
        {
            get
            {
                return (Contract != null);
            }
        }

        public bool ContractApproved
        {
            get
            {
                return (Contract != null && Contract.IsApproved);
            }
        }

        public Int32? NumSubContracts
        {
            get
            {
                if (Contract == null)
                    return null;
                else
                    return Contract.NumSubContracts;
            }
        }

        public String ContractIdStr
        {
            get
            {
                if (Contract == null)
                    return null;
                else
                    return Contract.IdStr;
            }
        }

        public String ProjectIdStr
        {
            get { return Project != null ? Project.IdStr : null; }
        }

        public String ProjectName
        {
            get { return Project != null ? Project.Name : null; }
        }

        public String ProjectNumber
        {
            get { return Project != null ? Project.FullNumber : null; }
        }

        public String BusinessUnitName
        {
            get { return Project != null ? Project.BusinessUnitName : null; }
        }

        public String BusinessUnitIdStr
        {
            get { return Project != null ? Project.BusinessUnitIdStr : null; }
        }

        public int? ProjectManagerId
        {
            get { return ProjectManager != null ? ProjectManager.Id : null; }
        }

        public int? ContractsAdministratorId
        {
            get { return ContractsAdministrator != null ? ContractsAdministrator.Id : null; }
        }
#endregion
    
    }
}
