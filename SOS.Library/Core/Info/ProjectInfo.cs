using System;
using System.Collections.Generic;
using System.Text;

namespace SOS.Core
{
    [Serializable]
    public class ProjectInfo : Info
    {

        #region Constants
        public const String StatusProposal = "PR";
        public const String StatusActive = "AT";
        public const String StatusComplete = "CP";
        public const String StatusCompleteWithTasks = "CT";
        public const String Forthnightly = "F";
        public const String Monthly = "M";

        public const int DaysToShowManualLink = -25;
        public const int DaysDueManualLink = -10;

        public const int DaysToShowProjectReview = 0;
        public const int DaysDueProjectReview = 15;

        public const String FileTypeManual = "MA";
        public const String FileTypeReview = "RE";

        public enum combinedStatus
        {
            One,
            All,
            Proposal,
            Active,
            Complete,
            CompleteWithTasks,
            ActiveAndCompleteWithTasks
        }
        #endregion

        #region Private Members
        private String name;
        private String number;
        private String year;
        private String street;
        private String locality;
        private String state;
        private String postalCode;
        private String fax;
        private String description;
        private int? defectsLiability;
        private String liquidatedDamages;
        private String siteAllowances;
        private String retention;
        private String retentionToCertification;
        private String retentionToDLP;
        private String interest;
        private String principal;
        private String principalABN;
        private String principal2;
        private String principal2ABN;
        private String specialClause;
        private String lawOfSubcontract;
        private String status;
        private DateTime? commencementDate;
        private DateTime? completionDate;


        //#---- 22/06/2016--
        private DateTime? claimedEOTsCompletionDate;
        private DateTime? approvedEOTsCompletionDate;


        //#---- 22/06/2016--


        private Decimal? contractAmount;
        private String paymentTerms;
        private String claimFrequency;
        private Decimal? waranty1Amount;
        private Decimal? waranty2Amount;
        private DateTime? waranty1Date;
        private DateTime? waranty2Date;
        private DateTime? practicalCompletionDate;
        private DateTime? firstClaimDueDate;
        private Byte[] distributionListInfo;
        private String deepZoomUrl;
        private String attachmentsFolder;
        private String maintenanceManualFile;
        private String postProjectReviewFile;

        private EmployeeInfo managingDirector;
        private EmployeeInfo designCoordinator;
        private EmployeeInfo designManager;
        private EmployeeInfo contractsAdministrator;
        private EmployeeInfo projectManager;
        private EmployeeInfo constructionManager;
        private EmployeeInfo foreman;
        private EmployeeInfo financialController;
        private EmployeeInfo directorAuthorization;
        private EmployeeInfo budgetAdministrator;

        //#----new Role COM JCA
        private EmployeeInfo commercialManager;
        private EmployeeInfo juniorContractsAdministrator;

        private EmployeeInfo contractsAdministrator3;
        private EmployeeInfo contractsAdministrator4;
        private EmployeeInfo contractsAdministrator5;
        private EmployeeInfo contractsAdministrator6;

        //#---- site Address  
        private String siteaddress;
        private String sitesuburb;
        private String sitestate;
        private String sitepostalCode;

        //#---- Principal Address 
        private String principaladdress;
        private String principalsuburb;
        private String principalstate;
        private String principalpostalCode;

        //#----     



        private ClientContactInfo clientContact;
        private ClientContactInfo clientContact1;
        private ClientContactInfo clientContact2;
        private ClientContactInfo superintendent;
        private ClientContactInfo quantitySurveyor;
        private ClientContactInfo secondPrincipal;
        private BusinessUnitInfo businessUnit;
        //#---
        private List<ClientContactInfo> clientContactList; //---Distributionlist and Websiteaccesslist
        private List<MeetingMinutesInfo> meetingMinutesList;// ---List of Meetings

        //#--
        private List<DrawingInfo> drawings;
        private List<TransmittalInfo> transmittals;
        private List<TradeInfo> trades;
        private List<ClientTradeInfo> clientTrades;
        private List<BudgetInfo> budgets;
        private List<ClientVariationInfo> clientVariations;
        private List<ClientVariationInfo> separateAccounts;
        private List<ClientVariationInfo> tenantVariations;//#--TV----
        private List<ClaimInfo> claims;
        private List<RFIInfo> rFIs;
        private List<EOTInfo> eOTs;

        private Dictionary<int, EmployeeInfo> tradesContractsAdministratorsByTrade;
        private List<EmployeeInfo> tradesContractsAdministrators;

        private List<EmployeeInfo> tradesProjectManagers;
        private Dictionary<int, EmployeeInfo> tradesProjectManagersByTrade;
        #endregion

        #region Constructors
        public ProjectInfo()
        {
        }

        public ProjectInfo(int? projectId)
        {
            Id = projectId;
        }
        #endregion

        #region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Number
        {
            get { return number; }
            set { number = value; }
        }

        public String Year
        {
            get { return year; }
            set { year = value; }
        }

        public String Street
        {
            get { return street; }
            set { street = value; }
        }

        public String Locality
        {
            get { return locality; }
            set { locality = value; }
        }

        public String State
        {
            get { return state; }
            set { state = value; }
        }

        public String PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public String Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int? DefectsLiability
        {
            get { return defectsLiability; }
            set { defectsLiability = value; }
        }

        public String LiquidatedDamages
        {
            get { return liquidatedDamages; }
            set { liquidatedDamages = value; }
        }

        public String SiteAllowances
        {
            get { return siteAllowances; }
            set { siteAllowances = value; }
        }

        public String Retention
        {
            get { return retention; }
            set { retention = value; }
        }

        public String RetentionToCertification
        {
            get { return retentionToCertification; }
            set { retentionToCertification = value; }
        }

        public String RetentionToDLP
        {
            get { return retentionToDLP; }
            set { retentionToDLP = value; }
        }

        public String Interest
        {
            get { return interest; }
            set { interest = value; }
        }

        public String Principal
        {
            get { return principal; }
            set { principal = value; }
        }

        public String PrincipalABN
        {
            get { return principalABN; }
            set { principalABN = value; }
        }

        public String Principal2
        {
            get { return principal2; }
            set { principal2 = value; }
        }

        public String Principal2ABN
        {
            get { return principal2ABN; }
            set { principal2ABN = value; }
        }

        public String SpecialClause
        {
            get { return specialClause; }
            set { specialClause = value; }
        }
        //#-----

        //#-----SiteAddress
        public String Siteaddress
        {
            get { return siteaddress; }
            set { siteaddress = value; }
        }

        public String SiteSuburb
        {
            get { return sitesuburb; }
            set { sitesuburb = value; }
        }

        public String SiteState
        {
            get { return sitestate; }
            set { sitestate = value; }
        }

        public String SitePostalCode
        {
            get { return sitepostalCode; }
            set { sitepostalCode = value; }
        }
        //#-----SiteAddress


        //---------------Principal Address----

        public String Principaladdress
        {
            get { return principaladdress; }
            set { principaladdress = value; }
        }

        public String PrincipalSuburb
        {
            get { return principalsuburb; }
            set { principalsuburb = value; }
        }

        public String PrincipalState
        {
            get { return principalstate; }
            set { principalstate = value; }
        }

        public String PrincipalPostalCode
        {
            get { return principalpostalCode; }
            set { principalpostalCode = value; }
        }

        //#-----







        //#-----












        public String LawOfSubcontract
        {
            get { return lawOfSubcontract; }
            set { lawOfSubcontract = value; }
        }

        public String AccountName { get; set; }

        public String AccountNumber { get; set; }

        public String BSB { get; set; }



        //#-----


        public String Status
        {
            get { return status; }
            set { status = value; }
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

        public Decimal? ContractAmount
        {
            get { return contractAmount; }
            set { contractAmount = value; }
        }

        public String PaymentTerms
        {
            get { return paymentTerms; }
            set { paymentTerms = value; }
        }

        public String ClaimFrequency
        {
            get { return claimFrequency; }
            set { claimFrequency = value; }
        }

        public Decimal? Waranty1Amount
        {
            get { return waranty1Amount; }
            set { waranty1Amount = value; }
        }

        public DateTime? Waranty1Date
        {
            get { return waranty1Date; }
            set { waranty1Date = value; }
        }

        public Decimal? Waranty2Amount
        {
            get { return waranty2Amount; }
            set { waranty2Amount = value; }
        }

        public DateTime? Waranty2Date
        {
            get { return waranty2Date; }
            set { waranty2Date = value; }
        }

        public DateTime? PracticalCompletionDate
        {
            get { return practicalCompletionDate; }
            set { practicalCompletionDate = value; }
        }

        //#----22/06/2016----------To display copletiondate-including claimed and Approved EOTs-------

        public DateTime? ClaimedEOTsCompletionDate
        {
            get { return claimedEOTsCompletionDate; }
            set { claimedEOTsCompletionDate = value; }
        }

        public DateTime? ApprovedEOTsCompletionDate
        {
            get { return approvedEOTsCompletionDate; }
            set { approvedEOTsCompletionDate = value; }
        }

        //#----22/06/2016------------------





        public DateTime? FirstClaimDueDate
        {
            get { return firstClaimDueDate; }
            set { firstClaimDueDate = value; }
        }

        public String DeepZoomUrl
        {
            get { return deepZoomUrl; }
            set { deepZoomUrl = value; }
        }

        public String AttachmentsFolder
        {
            get { return attachmentsFolder; }
            set { attachmentsFolder = value; }
        }

        public String MaintenanceManualFile
        {
            get { return maintenanceManualFile; }
            set { maintenanceManualFile = value; }
        }

        public String PostProjectReviewFile
        {
            get { return postProjectReviewFile; }
            set { postProjectReviewFile = value; }
        }

        public List<EmployeeInfo> TradesContractsAdministrators
        {
            get { return tradesContractsAdministrators; }
            set { tradesContractsAdministrators = value; }
        }

        public Dictionary<int, EmployeeInfo> TradesContractsAdministratorsByTrade
        {
            get { return tradesContractsAdministratorsByTrade; }
            set { tradesContractsAdministratorsByTrade = value; }
        }

        public List<EmployeeInfo> TradesProjectManagers
        {
            get { return tradesProjectManagers; }
            set { tradesProjectManagers = value; }
        }

        public Dictionary<int, EmployeeInfo> TradesProjectManagersByTrade
        {
            get { return tradesProjectManagersByTrade; }
            set { tradesProjectManagersByTrade = value; }
        }

        public String PMInitials
        {
            get { return ProjectManager != null ? ProjectManager.Initials : null; }
        }

        public String CMInitials
        {
            get { return ConstructionManager != null ? ConstructionManager.Initials : null; }
        }

        public String MDInitials
        {
            get { return ManagingDirector != null ? ManagingDirector.Initials : null; }
        }

        public String ClientContactInitials
        {
            get { return ClientContact != null ? ClientContact.Initials : null; }
        }

        public String ClientContact1Initials
        {
            get { return ClientContact1 != null ? ClientContact1.Initials : null; }
        }

        public String ClientContact2Initials
        {
            get { return ClientContact2 != null ? ClientContact2.Initials : null; }
        }

        public String SuperintendentInitials
        {
            get { return Superintendent != null ? Superintendent.Initials : null; }
        }

        public String QuantitySurveyorInitials
        {
            get { return QuantitySurveyor != null ? QuantitySurveyor.Initials : null; }
        }

        public String DistributionListEOTInitials
        {
            get
            {
                StringBuilder distributionListInitials = new StringBuilder(16);
                String disListInitials;

                if (PMInitials != null)
                    distributionListInitials.Append(PMInitials);

                if (CMInitials != null)
                    distributionListInitials.Append(",").Append(CMInitials);

                if (MDInitials != null)
                    distributionListInitials.Append(",").Append(MDInitials);

                if (SendEOTToClientContact && ClientContactInitials != null)
                    distributionListInitials.Append(",").Append(ClientContactInitials);

                if (SendEOTToClientContact1 && ClientContact1Initials != null)
                    distributionListInitials.Append(",").Append(ClientContact1Initials);

                if (SendEOTToClientContact2 && ClientContact2Initials != null)
                    distributionListInitials.Append(",").Append(ClientContact2Initials);

                if (SendEOTToSuperintendent && SuperintendentInitials != null)
                    distributionListInitials.Append(",").Append(SuperintendentInitials);

                if (SendEOTToQuantitySurveyor && QuantitySurveyorInitials != null)
                    distributionListInitials.Append(",").Append(QuantitySurveyorInitials);

                disListInitials = distributionListInitials.ToString();

                if (disListInitials != null && disListInitials.Substring(0, 1) == ",")
                    disListInitials = disListInitials.Substring(1, disListInitials.Length - 1);

                return disListInitials;
            }
        }

        public String DistributionListInfo
        {
            get
            {
                if (distributionListInfo == null)
                    return null;
                else
                {
                    Char[] c = { (Char)distributionListInfo[0], (Char)distributionListInfo[1], (Char)distributionListInfo[2], (Char)distributionListInfo[3], (Char)distributionListInfo[4] };
                    return new String(c);
                }
            }
            set
            {
                if (value != null && value.Length == 5)
                {
                    distributionListInfo = new Byte[5];
                    for (int i = 0; i < value.Length; i++)
                        distributionListInfo[i] = (Byte)value[i];
                }
                else
                    distributionListInfo = null;

            }
        }

        public EmployeeInfo DesignCoordinator
        {
            get { return designCoordinator; }
            set { designCoordinator = value; }
        }

        public EmployeeInfo DesignManager
        {
            get { return designManager; }
            set { designManager = value; }
        }

        public EmployeeInfo ManagingDirector
        {
            get { return managingDirector; }
            set { managingDirector = value; }
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

        public EmployeeInfo ConstructionManager
        {
            get { return constructionManager; }
            set { constructionManager = value; }
        }

        public EmployeeInfo Foreman
        {
            get { return foreman; }
            set { foreman = value; }
        }

        public EmployeeInfo FinancialController
        {
            get { return financialController; }
            set { financialController = value; }
        }

        public EmployeeInfo DirectorAuthorization
        {
            get { return directorAuthorization; }
            set { directorAuthorization = value; }
        }

        public EmployeeInfo BudgetAdministrator
        {
            get { return budgetAdministrator; }
            set { budgetAdministrator = value; }
        }

        //#-------New Role COM and JCA
        public EmployeeInfo CommercialManager
        {
            get { return commercialManager; }
            set { commercialManager = value; }
        }

        public EmployeeInfo JuniorContractsAdministrator
        {
            get { return juniorContractsAdministrator; }
            set { juniorContractsAdministrator = value; }
        }


        public EmployeeInfo ContractsAdministrator3
        {
            get { return contractsAdministrator3; }
            set { contractsAdministrator3 = value; }
        }

        public EmployeeInfo ContractsAdministrator4
        {
            get { return contractsAdministrator4; }
            set { contractsAdministrator4 = value; }
        }

        public EmployeeInfo ContractsAdministrator5
        {
            get { return contractsAdministrator5; }
            set { contractsAdministrator5 = value; }
        }

        public EmployeeInfo ContractsAdministrator6
        {
            get { return contractsAdministrator6; }
            set { contractsAdministrator6 = value; }
        }

        //#-------


        public ClientContactInfo ClientContact
        {
            get { return clientContact; }
            set { clientContact = value; }
        }

        public ClientContactInfo ClientContact1
        {
            get { return clientContact1; }
            set { clientContact1 = value; }
        }

        public ClientContactInfo ClientContact2
        {
            get { return clientContact2; }
            set { clientContact2 = value; }
        }

        public ClientContactInfo Superintendent
        {
            get { return superintendent; }
            set { superintendent = value; }
        }

        public ClientContactInfo QuantitySurveyor
        {
            get { return quantitySurveyor; }
            set { quantitySurveyor = value; }
        }

        public ClientContactInfo SecondPrincipal
        {
            get { return secondPrincipal; }
            set { secondPrincipal = value; }
        }

        public BusinessUnitInfo BusinessUnit
        {
            get { return businessUnit; }
            set { businessUnit = value; }
        }
        //#---
        public List<ClientContactInfo> ClientContactList
        {
            get { return clientContactList; }
            set { clientContactList = value; }
        }

        public List<MeetingMinutesInfo> MeetingMinutesList
        {
            get { return meetingMinutesList; }
            set { meetingMinutesList = value; }
        }



        //#---

        public List<DrawingInfo> Drawings
        {
            get { return drawings; }
            set { drawings = value; }
        }

        public List<DrawingInfo> DrawingsActive
        {
            get
            {
                if (Drawings != null)
                {
                    List<DrawingInfo> drawingsActive = new List<DrawingInfo>();

                    foreach (DrawingInfo drawing in Drawings)
                        if (drawing.IsActive)
                            drawingsActive.Add(drawing);

                    return drawingsActive;
                }
                else
                    return null;
            }
        }

        public List<DrawingInfo> DrawingsProposal
        {
            get
            {
                if (Drawings != null)
                {
                    List<DrawingInfo> drawingsProposal = new List<DrawingInfo>();

                    foreach (DrawingInfo drawing in Drawings)
                        if (drawing.IsProposal)
                            drawingsProposal.Add(drawing);

                    return drawingsProposal;
                }
                else
                    return null;
            }
        }

        public List<TransmittalInfo> Transmittals
        {
            get { return transmittals; }
            set { transmittals = value; }
        }

        public List<TradeInfo> Trades
        {
            get { return trades; }
            set { trades = value; }
        }

        public List<ClientTradeInfo> ClientTrades
        {
            get { return clientTrades; }
            set { clientTrades = value; }
        }

        public List<BudgetInfo> Budgets
        {
            get { return budgets; }
            set { budgets = value; }
        }

        public List<ClientVariationInfo> ClientVariations
        {
            get { return clientVariations; }
            set { clientVariations = value; }
        }

        public List<ClientVariationInfo> SeparateAccounts
        {
            get { return separateAccounts; }
            set { separateAccounts = value; }
        }
        //#---TV---
        public List<ClientVariationInfo> TenantVariations
        {
            get { return tenantVariations; }
            set { tenantVariations = value; }
        }
        //#---TV---

        public List<ClaimInfo> Claims
        {
            get { return claims; }
            set { claims = value; }
        }

        public List<RFIInfo> RFIs
        {
            get { return rFIs; }
            set { rFIs = value; }
        }

        public List<EOTInfo> EOTs
        {
            get { return eOTs; }
            set { eOTs = value; }
        }

        public EmployeeInfo UnitManager
        {
            get { return BusinessUnit != null ? BusinessUnit.UnitManager : null; }
        }

        public String ContractsAdministratorName
        {
            get
            {
                if (ContractsAdministrator != null)
                    return ContractsAdministrator.Name;
                else
                    return null;
            }
        }

        public String ProjectManagerName
        {
            get
            {
                if (ProjectManager != null)
                    return ProjectManager.Name;
                else
                    return null;
            }
        }

        public String ConstructionManagerName
        {
            get
            {
                if (ConstructionManager != null)
                    return ConstructionManager.Name;
                else
                    return null;
            }
        }

        public String BusinessUnitIdStr
        {
            get { return BusinessUnit == null ? null : BusinessUnit.IdStr; }
        }

        public String BusinessUnitName
        {
            get { return BusinessUnit == null ? null : BusinessUnit.Name; }
        }

        public List<TradeInfo> TenderTrades
        {
            get
            {
                List<TradeInfo> tenderTrades = new List<TradeInfo>();

                foreach (TradeInfo tradeInfo in Trades)
                    if (tradeInfo.SelectedParticipation == null)
                        tenderTrades.Add(tradeInfo);

                return tenderTrades;
            }
        }

        public List<TradeInfo> ContractTrades
        {
            get
            {
                List<TradeInfo> contractTrades = new List<TradeInfo>();
                if (Trades != null)  //DS20240301
                { 
                foreach (TradeInfo tradeInfo in Trades)
                    if (tradeInfo.SelectedParticipation != null)
                        contractTrades.Add(tradeInfo);
                }
                return contractTrades;

            }
        }

        public String FullNumber
        {
            get
            {

                //#---if (BusinessUnit != null)

                //#--
                if (BusinessUnit != null && Year != null && Number != null)
                    //#----


                    if (BusinessUnit.ProjectNumberFormat != null)
                        switch (BusinessUnit.ProjectNumberFormat)
                        {
                            //#---case "P-Y": return (Number != null ? Number : "") + "-" + (Year != null ? Year : "");


                            //#--
                            case "P-Y":
                                if (int.Parse(Year) == 16 && int.Parse(Number) > 600)
                                {
                                    return (Year != null ? Year : "") + "-" + (Number != null ? Number : "");
                                }
                                else if (int.Parse(Year) > 16)
                                {
                                    return (Year != null ? Year : "") + "-" + (Number != null ? Number : "");
                                }
                                else
                                {
                                    return (Number != null ? Number : "") + "-" + (Year != null ? Year : "");
                                }
                            //#--

                            case "Y-P": return (Year != null ? Year : "") + "-" + (Number != null ? Number : "");
                        }

                return String.Empty;
            }
        }

        public String StatusName
        {
            get
            {
                if (Status == null)
                    return null;
                else
                    switch (Status)
                    {
                        case StatusProposal: return "Proposal";
                        case StatusActive: return "Active";
                        case StatusComplete: return "Complete";
                        case StatusCompleteWithTasks: return "Complete with tasks";
                        default: return "???";
                    }
            }
        }

        public Boolean IsStatusProposal
        {
            get { return Status == StatusProposal; }
        }

        public Decimal? TotalClientTrades
        {
            get
            {
                if (ClientTrades != null)
                {
                    Decimal totalClientTrades = 0;

                    foreach (ClientTradeInfo clientTradeInfo in ClientTrades)
                        if (clientTradeInfo.Amount != null)
                            totalClientTrades += (Decimal)clientTradeInfo.Amount;

                    return totalClientTrades;
                }

                return null;
            }
        }

        public Decimal? TotalClientVariations
        {
            get
            {
                Decimal? totalAmount;

                if (ClientVariations != null)
                {
                    Decimal? totalClientVariations = 0;

                    foreach (ClientVariationInfo clientVariationInfo in ClientVariations)
                    {
                        totalAmount = clientVariationInfo.TotalAmount;

                        if (totalAmount != null)
                            totalClientVariations += (Decimal)totalAmount;
                    }

                    return totalClientVariations;
                }

                return null;
            }
        }

        public Decimal? TotalLastClaim
        {
            get { return Claims != null && Claims.Count != 0 ? Claims[Claims.Count - 1].Total : null; }
        }

        public DateTime? DateLastClaim
        {
            get { return Claims != null && Claims.Count != 0 ? Claims[Claims.Count - 1].ApprovalDate : null; }
        }

        public Decimal? ContractAmountMinusClientTrades
        {
            get
            {
                Decimal? totalClientTrades = TotalClientTrades;

                if (ContractAmount != null)
                    if (totalClientTrades != null)
                        return ContractAmount - totalClientTrades;

                return null;
            }
        }

        public Decimal? ContractAmountPlusVariations
        {
            get
            {
                Decimal? totalClientVariations = TotalClientVariations;

                if (ContractAmount != null)
                    if (totalClientVariations != null)
                        return (Decimal)ContractAmount + (Decimal)totalClientVariations;
                    else
                        return (Decimal)ContractAmount;

                return null;
            }
        }

        public DateTime? ExperiationDateMaintenancePeriod
        {
            get
            {
                if (PracticalCompletionDate != null)
                    if (DefectsLiability != null)
                        return ((DateTime)practicalCompletionDate).AddDays((double)DefectsLiability);

                return null;
            }
        }




        public Boolean SendCVToClientContact
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[0] >> 4) & 1) == 1); }
        }

        public Boolean SendSAToClientContact
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[0] >> 3) & 1) == 1); }
        }

        public Boolean SendPCToClientContact
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[0] >> 2) & 1) == 1); }
        }

        public Boolean SendRFIToClientContact
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[0] >> 1) & 1) == 1); }
        }

        public Boolean SendEOTToClientContact
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[0] >> 0) & 1) == 1); }
        }

        public Boolean SendCVToClientContact1
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[1] >> 4) & 1) == 1); }
        }

        public Boolean SendSAToClientContact1
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[1] >> 3) & 1) == 1); }
        }

        public Boolean SendPCToClientContact1
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[1] >> 2) & 1) == 1); }
        }

        public Boolean SendRFIToClientContact1
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[1] >> 1) & 1) == 1); }
        }

        public Boolean SendEOTToClientContact1
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[1] >> 0) & 1) == 1); }
        }

        public Boolean SendCVToClientContact2
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[2] >> 4) & 1) == 1); }
        }

        public Boolean SendSAToClientContact2
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[2] >> 3) & 1) == 1); }
        }

        public Boolean SendPCToClientContact2
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[2] >> 2) & 1) == 1); }
        }

        public Boolean SendRFIToClientContact2
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[2] >> 1) & 1) == 1); }
        }

        public Boolean SendEOTToClientContact2
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[2] >> 0) & 1) == 1); }
        }

        public Boolean SendCVToSuperintendent
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[3] >> 4) & 1) == 1); }
        }

        public Boolean SendSAToSuperintendent
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[3] >> 3) & 1) == 1); }
        }

        public Boolean SendPCToSuperintendent
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[3] >> 2) & 1) == 1); }
        }

        public Boolean SendRFIToSuperintendent
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[3] >> 1) & 1) == 1); }
        }

        public Boolean SendEOTToSuperintendent
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[3] >> 0) & 1) == 1); }
        }

        public Boolean SendCVToQuantitySurveyor
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[4] >> 4) & 1) == 1); }
        }

        public Boolean SendSAToQuantitySurveyor
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[4] >> 3) & 1) == 1); }
        }

        public Boolean SendPCToQuantitySurveyor
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[4] >> 2) & 1) == 1); }
        }

        public Boolean SendRFIToQuantitySurveyor
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[4] >> 1) & 1) == 1); }
        }

        public Boolean SendEOTToQuantitySurveyor
        {
            get { return (distributionListInfo != null) && (((distributionListInfo[4] >> 0) & 1) == 1); }
        }







        private int? NumCVStatus(String status)
        {
            if (ClientVariations != null)
            {
                int numCVStatus = 0;

                foreach (ClientVariationInfo clientVariationInfo in ClientVariations)
                    if (clientVariationInfo.Status == status)
                        numCVStatus++;

                return numCVStatus;
            }
            else
            {
                return null;
            }
        }

        private Decimal? TotCVStatus(String status)
        {
            if (ClientVariations != null)
            {
                Decimal totCVStatus = 0;
                Decimal? totCV;

                foreach (ClientVariationInfo clientVariationInfo in ClientVariations)
                    if (clientVariationInfo.Status == status)
                    {
                        totCV = clientVariationInfo.TotalAmount;

                        if (totCV != null)
                            totCVStatus += (Decimal)totCV;
                    }

                return totCVStatus;
            }
            else
            {
                return null;
            }
        }

        public int? NumCVCancelled
        {
            get { return NumCVStatus(ClientVariationInfo.StatusCancelled); }
        }

        public int? NumCVApproved
        {
            get { return NumCVStatus(ClientVariationInfo.StatusApproved); }
        }

        public int? NumCVVerballyApproved
        {
            get { return NumCVStatus(ClientVariationInfo.StatusVerballyApproved); }
        }

        public int? NumCVToBeApproved
        {
            get { return NumCVStatus(ClientVariationInfo.StatusToBeApproved); }
        }

        public int? NumCVTobeIssued
        {
            get { return NumCVStatus(ClientVariationInfo.StatusTobeIssued); }
        }

        public Decimal? TotCVCancelled
        {
            get { return TotCVStatus(ClientVariationInfo.StatusCancelled); }
        }

        public Decimal? TotCVApproved
        {
            get { return TotCVStatus(ClientVariationInfo.StatusApproved); }
        }

        public Decimal? TotCVVerballyApproved
        {
            get { return TotCVStatus(ClientVariationInfo.StatusVerballyApproved); }
        }

        public Decimal? TotCVToBeApproved
        {
            get { return TotCVStatus(ClientVariationInfo.StatusToBeApproved); }
        }

        public Decimal? TotCVTobeIssued
        {
            get { return TotCVStatus(ClientVariationInfo.StatusTobeIssued); }
        }

        public Boolean IsEmptyDrawingsActvie
        {
            get
            {
                return DrawingsActive == null || DrawingsActive.Count == 0;
            }
        }








        #endregion

    }
}
