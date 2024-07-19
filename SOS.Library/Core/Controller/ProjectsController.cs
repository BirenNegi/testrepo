using System;
using System.Data;
using System.Xml;
using System.Linq;
using System.IO;
using System.Transactions;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

using System.Text;
using Microsoft.Reporting.WebForms;
using Excel;

using SOS.Data;
using System.Security.Claims;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices.ComTypes;
using System.Collections;

namespace SOS.Core
{
    public sealed class ProjectsController : Controller
    {

        #region Private Constants
        private const int FirstAsciiCode = 65;
        private const int LastAsciiCode = 90;
        #endregion

        #region Private Members
        private static ProjectsController instance;

        private List<DateTime> holidays = null;
        private List<DateTime> RDOs = null;
        private delegate int? AddProjectDelegate(ProjectInfo projectInfo, PeopleInfo peopleInfo, ProcessStatus processStatus);
        private delegate Int32 UpdateProjectFilesDelegate(ProjectInfo projectInfo, String activePath, String archivePath, ProcessStatus processStatus);
        #endregion

        #region Private Methods
        private ProjectsController()
        {
        }
        #endregion

        #region Public Methods

        public static ProjectsController GetInstance()
        {
            if (instance == null)
                instance = new ProjectsController();

            return instance;
        }


        #region General Methods
        /// <summary>
        /// Change display order for client trades
        /// </summary>
        public void ChangeDisplayOrderClientTrade(List<ClientTradeInfo> clientTradeInfoList, ClientTradeInfo clientTradeInfo, bool moveUp)
        {
            TradesController tradesController = TradesController.GetInstance();

            List<ISortable> iSortableList = new List<ISortable>();
            foreach (ClientTradeInfo tmpClientTradeInfo in clientTradeInfoList)
                iSortableList.Add(tmpClientTradeInfo);

            ClientTradeInfo modifiedClientTradeInfo = (ClientTradeInfo)tradesController.ChangeDisplayOrder(iSortableList, clientTradeInfo, moveUp);
            if (modifiedClientTradeInfo != null)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        UpdateClientTrade(clientTradeInfo);
                        UpdateClientTrade(modifiedClientTradeInfo);

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.ToString());
                    throw new Exception("Changing Client Trade Display Order");
                }
            }
        }
        #endregion


        #region Holidays Methods
        /// <summary>
        /// Get all the Holidays from persistent storage
        /// </summary>
        public List<DateTime> GetHolidays()
        {
            IDataReader dr = null;
            List<DateTime> holidaysList = new List<DateTime>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetHolidays();
                while (dr.Read())
                    holidaysList.Add((DateTime)Data.Utils.GetDBDateTime(dr["HolidayDate"]));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Holidays from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return holidaysList;
        }

        /// <summary>
        /// Adds a Holiday to the database
        /// </summary>
        public void AddHoliday(DateTime dateTime)
        {
            try
            {
                Data.DataProvider.GetInstance().AddHoliday(dateTime);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Holiday to database. It might already exist.");
            }
        }

        /// <summary>
        /// Remove a Holiday from persistent storage
        /// </summary>
        public void DeleteHoliday(DateTime dateTime)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteHoliday(dateTime);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Holiday from database.");
            }
        }

        /// <summary>
        /// Intializes the system used to calculate business days
        /// </summary>
        public void InitializeHolidays()
        {
            holidays = GetHolidays();
            holidays.Sort();
        }
        #endregion


        #region RDOs Methods
        /// <summary>
        /// Get all the RDOs from persistent storage
        /// </summary>
        public List<DateTime> GetRDOs()
        {
            IDataReader dr = null;
            List<DateTime> RDOsList = new List<DateTime>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetRDOs();
                while (dr.Read())
                    RDOsList.Add((DateTime)Data.Utils.GetDBDateTime(dr["RDODate"]));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RDOs from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return RDOsList;
        }

        /// <summary>
        /// Adds a RDO to the database
        /// </summary>
        public void AddRDO(DateTime dateTime)
        {
            try
            {
                Data.DataProvider.GetInstance().AddRDO(dateTime);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding RDO to database. It might already exist.");
            }
        }

        /// <summary>
        /// Remove a RDO from persistent storage
        /// </summary>
        public void DeleteRDO(DateTime dateTime)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteRDO(dateTime);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing RDO from database.");
            }
        }

        /// <summary>
        /// Intializes the system used to calculate business days
        /// </summary>
        public void InitializeRDOs()
        {
            RDOs = GetRDOs();
            RDOs.Sort();
        }
        #endregion


        #region Project Methods
        /// <summary>
        /// Get projects names form the database based on status and businessunit
        /// </summary>
        public List<ProjectInfo> ListProjects(ProjectInfo.combinedStatus combinedStatus, BusinessUnitInfo businessUnit)
        {
            IDataReader dr = null;
            ProjectInfo projectInfo = null;
            Boolean include;
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>();
            List<ProjectInfo> projectInfoList1 = new List<ProjectInfo>();

            try
            {
                if (combinedStatus == ProjectInfo.combinedStatus.Active)
                    dr = Data.DataProvider.GetInstance().ListActiveProjects();
                else
                    dr = Data.DataProvider.GetInstance().ListProjects();

                while (dr.Read())
                {
                    projectInfo = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    projectInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    projectInfo.Status = Data.Utils.GetDBString(dr["Status"]);
                    projectInfo.ContractAmount = Data.Utils.GetDBDecimal(dr["ContractAmount"]);
                    projectInfo.CompletionDate = Data.Utils.GetDBDateTime(dr["CompletionDate"]);
                    projectInfo.MaintenanceManualFile = Data.Utils.GetDBString(dr["MaintenanceManualFile"]);
                    projectInfo.PostProjectReviewFile = Data.Utils.GetDBString(dr["PostProjectReviewFile"]);

                    if (dr["BusinessUnitId"] != DBNull.Value)
                    {
                        projectInfo.BusinessUnit = new BusinessUnitInfo(Data.Utils.GetDBInt32(dr["businessUnitId"]));
                        projectInfo.BusinessUnit.Name = Data.Utils.GetDBString(dr["BusinessUnitName"]);
                    }

                    if (businessUnit == null || (projectInfo.BusinessUnit != null && projectInfo.BusinessUnit.Equals(businessUnit)))
                        projectInfoList.Add(projectInfo);
                }

                if (combinedStatus != ProjectInfo.combinedStatus.All && combinedStatus != ProjectInfo.combinedStatus.Active)
                    foreach (ProjectInfo project in projectInfoList)
                    {
                        switch (combinedStatus)
                        {
                            case ProjectInfo.combinedStatus.Proposal: include = project.Status == ProjectInfo.StatusProposal; break;
                            case ProjectInfo.combinedStatus.Complete: include = project.Status == ProjectInfo.StatusComplete; break;
                            case ProjectInfo.combinedStatus.CompleteWithTasks: include = project.Status == ProjectInfo.StatusCompleteWithTasks; break;
                            case ProjectInfo.combinedStatus.ActiveAndCompleteWithTasks: include = project.Status == ProjectInfo.StatusActive || project.Status == ProjectInfo.StatusCompleteWithTasks; break;
                            default: include = false; break;
                        }

                        if (include)
                            projectInfoList1.Add(project);
                    }
                else
                    projectInfoList1 = projectInfoList;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Projects from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectInfoList1;
        }

        public List<ProjectInfo> ListProjects()
        {
            return ListProjects(ProjectInfo.combinedStatus.All, null);
        }

        public List<ProjectInfo> ListActiveProjects()
        {
            return ListProjects(ProjectInfo.combinedStatus.Active, null);
        }

        public List<ProjectInfo> ListProjects(BusinessUnitInfo businessUnit)
        {
            return ListProjects(ProjectInfo.combinedStatus.All, businessUnit);
        }

        public List<ProjectInfo> ListActiveProjects(BusinessUnitInfo businessUnit)
        {
            return ListProjects(ProjectInfo.combinedStatus.Active, businessUnit);
        }

        public int SearchProjectsCount(String strSearch, String strBusinessUnit, String strStatus)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            parameters.Add(UI.Utils.GetFormString(strStatus));
            try
            {
                return Data.DataProvider.GetInstance().SearchProjectCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Projects Count from database.");
            }
        }

        /// <summary>
        /// Search for projects in the database 
        /// </summary>
        public List<ProjectInfo> SearchProjects(int startRowIndex, int maximumRows, String orderBy, String strSearch, String strBusinessUnit, String strStatus)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>();

            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            parameters.Add(UI.Utils.GetFormString(orderBy));
            parameters.Add(UI.Utils.GetFormString(strSearch));
            parameters.Add(UI.Utils.GetFormInteger(strBusinessUnit));
            parameters.Add(UI.Utils.GetFormString(strStatus));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchProjects(parameters.ToArray());
                while (dr.Read())
                {
                    projectInfoList.Add(GetProject(Data.Utils.GetDBInt32(dr["ProjectId"])));
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Projects from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectInfoList;
        }

        /// <summary>
        /// Gets project trades from persistent storage
        /// </summary>
        public List<TradeInfo> GetProjectTrades(ProjectInfo projectInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTrades(projectInfo.Id);
                while (dr.Read())
                    tradeInfoList.Add(tradesController.CreateTrade(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Gets project trades with participations and contracts from persistent storage
        /// </summary>
        public List<TradeInfo> GetProjectTradesWithParticipationsAndSubcontracts(ProjectInfo projectInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            TradeInfo tradeInfo;
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTrades(projectInfo.Id);
                while (dr.Read())
                {
                    tradeInfo = tradesController.CreateTrade(dr);

                    if (tradeInfo.Contract != null)
                        tradeInfo.Contract.Subcontracts = contractsController.GetSubContracts(tradeInfo.Contract);

                    tradeInfo.Participations = tradesController.GetTradeParticipations(tradeInfo);

                    tradeInfoList.Add(tradeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Gets project trades with participations and contracts from persistent storage. Using optimized method
        /// </summary>
        public List<TradeInfo> GetProjectTradesWithParticipationsAndSubcontractsOptimized(ProjectInfo projectInfo)
        {
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();

            SubContractorsController subContractorsController = SubContractorsController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            PeopleController peopleController = PeopleController.GetInstance();

            Dictionary<int, TradeParticipationInfo> tradeParticipationsDictionary = new Dictionary<int, TradeParticipationInfo>();
            Dictionary<int, SubContractorInfo> subContractorsDictionary = new Dictionary<int, SubContractorInfo>();
            Dictionary<int, BusinessUnitInfo> businessUnitDictionary = new Dictionary<int, BusinessUnitInfo>();
            Dictionary<int, ProcessStepInfo> processStepsDictionary = new Dictionary<int, ProcessStepInfo>();
            Dictionary<int, ContractInfo> contractsDictionary = new Dictionary<int, ContractInfo>();
            Dictionary<int, ProcessInfo> processesDictionary = new Dictionary<int, ProcessInfo>();
            Dictionary<int, JobTypeInfo> jobTypesDictionary = new Dictionary<int, JobTypeInfo>();
            Dictionary<int, PeopleInfo> peopleDictionary = new Dictionary<int, PeopleInfo>();
            Dictionary<int, TradeInfo> tradesDictionary = new Dictionary<int, TradeInfo>();

            TradeParticipationInfo tradeParticipationInfo;
            SubContractorInfo subContractorInfo;
            ProcessStepInfo processStepInfo;
            ContractInfo contractInfo;
            ReversalInfo reversalInfo;
            JobTypeInfo jobTypeInfo;
            ProcessInfo processInfo;
            TradeInfo tradeInfo;

            IDataReader dr = null;
            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectAll(projectInfo.Id);

                // Job Types
                while (dr.Read())
                {
                    jobTypeInfo = contractsController.CreateJobType(dr);
                    jobTypesDictionary.Add(jobTypeInfo.Id.Value, jobTypeInfo);
                }

                // People not required
                // Business Units tot required

                // Subcontractors
                dr.NextResult();
                while (dr.Read())
                {
                    subContractorInfo = subContractorsController.CreateSubContractor(dr, businessUnitDictionary);
                    subContractorsDictionary.Add(subContractorInfo.Id.Value, subContractorInfo);
                }

                // Processes
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processController.CreateProcess(dr);
                    processesDictionary.Add(processInfo.Id.Value, processInfo);
                }

                dr.NextResult();
                processInfo = new ProcessInfo(0);
                while (dr.Read())
                {
                    processStepInfo = processController.CreateProcessStep(dr, peopleDictionary);
                    processStepsDictionary.Add(processStepInfo.Id.Value, processStepInfo);

                    if (processInfo.Id.Value != processStepInfo.Process.Id.Value)
                    {
                        processInfo = processesDictionary[processStepInfo.Process.Id.Value];
                        processInfo.Steps = new List<ProcessStepInfo>();
                    }

                    processInfo.Steps.Add(processStepInfo);
                    processStepInfo.Process = processInfo;
                }

                dr.NextResult();
                processStepInfo = new ProcessStepInfo(0);
                while (dr.Read())
                {
                    reversalInfo = processController.CreateReversal(dr, peopleDictionary);

                    if (processStepInfo.Id.Value != reversalInfo.ProcessStep.Id.Value)
                    {
                        processStepInfo = processStepsDictionary[reversalInfo.ProcessStep.Id.Value];
                        processStepInfo.Reversals = new List<ReversalInfo>();
                    }

                    processStepInfo.Reversals.Add(reversalInfo);
                    reversalInfo.ProcessStep = processStepInfo;
                }

                // Contracts
                dr.NextResult();
                while (dr.Read())
                {
                    contractInfo = contractsController.CreateContract(dr, processesDictionary);
                    contractsDictionary.Add(contractInfo.Id.Value, contractInfo);
                }

                foreach (ContractInfo subContract in contractsDictionary.Values)
                {
                    if (subContract.ParentContract != null)
                    {
                        contractInfo = contractsDictionary[subContract.ParentContract.Id.Value];

                        if (contractInfo.Subcontracts == null)
                            contractInfo.Subcontracts = new List<ContractInfo>();

                        contractInfo.Subcontracts.Add(subContract);
                    }
                }

                // Trades
                dr.NextResult();
                while (dr.Read())
                {
                    tradeInfo = tradesController.CreateTrade(dr, contractsDictionary, jobTypesDictionary, processesDictionary, peopleDictionary);
                    tradesDictionary.Add(tradeInfo.Id.Value, tradeInfo);
                    tradeInfoList.Add(tradeInfo);
                }

                //Trade Participations
                dr.NextResult();
                tradeInfo = new TradeInfo(0);
                while (dr.Read())
                {
                    tradeParticipationInfo = tradesController.CreateTradeParticipation(dr, subContractorsDictionary, peopleDictionary);
                    tradeParticipationsDictionary.Add(tradeParticipationInfo.Id.Value, tradeParticipationInfo);

                    if (tradeInfo.Id.Value != tradeParticipationInfo.Trade.Id.Value)
                    {
                        tradeInfo = tradesDictionary[tradeParticipationInfo.Trade.Id.Value];
                        tradeInfo.Participations = new List<TradeParticipationInfo>();
                    }

                    if (tradeParticipationInfo.ComparisonParticipation == null)
                        tradeInfo.Participations.Add(tradeParticipationInfo);
                }

                foreach (TradeParticipationInfo tradeParticipation in tradeParticipationsDictionary.Values)
                    if (tradeParticipation.ComparisonParticipation != null)
                        tradeParticipationsDictionary[tradeParticipation.ComparisonParticipation.Id.Value].QuoteParticipation = tradeParticipation;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Gets deep project trades from persistent storage
        /// </summary>
        public List<TradeInfo> GetDeepProjectTrades(ProjectInfo projectInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTrades(projectInfo.Id);
                while (dr.Read())
                    tradeInfoList.Add(tradesController.GetDeepTrade(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Gets a Project from persistent storage
        /// </summary>
        public ProjectInfo GetProject(int? ProjectId)
        {
            IDataReader dr = null;
            ProjectInfo projectInfo = null;
            PeopleController peopleController = PeopleController.GetInstance();

            try
            {
                dr = Data.DataProvider.GetInstance().GetProject(ProjectId);
                if (dr.Read())
                {
                    projectInfo = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    projectInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    projectInfo.Number = Data.Utils.GetDBString(dr["Number"]);
                    projectInfo.Year = Data.Utils.GetDBString(dr["Year"]);
                    projectInfo.Description = Data.Utils.GetDBString(dr["Description"]);
                    projectInfo.Street = Data.Utils.GetDBString(dr["Street"]);
                    projectInfo.Locality = Data.Utils.GetDBString(dr["Locality"]);
                    projectInfo.State = Data.Utils.GetDBString(dr["State"]);
                    projectInfo.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);
                    projectInfo.Fax = Data.Utils.GetDBString(dr["Fax"]);
                    projectInfo.DefectsLiability = Data.Utils.GetDBInt32(dr["DefectsLiability"]);
                    projectInfo.LiquidatedDamages = Data.Utils.GetDBString(dr["LiquidatedDamages"]);
                    projectInfo.SiteAllowances = Data.Utils.GetDBString(dr["SiteAllowances"]);
                    projectInfo.Retention = Data.Utils.GetDBString(dr["Retention"]);
                    projectInfo.RetentionToCertification = Data.Utils.GetDBString(dr["RetentionToCertification"]);
                    projectInfo.RetentionToDLP = Data.Utils.GetDBString(dr["RetentionToDLP"]);
                    projectInfo.Interest = Data.Utils.GetDBString(dr["Interest"]);
                    projectInfo.Principal = Data.Utils.GetDBString(dr["Principal"]);
                    projectInfo.PrincipalABN = Data.Utils.GetDBString(dr["PrincipalABN"]);
                    projectInfo.Principal2 = Data.Utils.GetDBString(dr["Principal2"]);
                    projectInfo.Principal2ABN = Data.Utils.GetDBString(dr["Principal2ABN"]);
                    projectInfo.SpecialClause = Data.Utils.GetDBString(dr["SpecialClause"]);
                    //#--
                    projectInfo.LawOfSubcontract = Data.Utils.GetDBString(dr["LawOfSubcontract"]);


                    projectInfo.AccountName = Data.Utils.GetDBString(dr["AccountName"]);
                    projectInfo.BSB = Data.Utils.GetDBString(dr["BSB"]);
                    projectInfo.AccountNumber = Data.Utils.GetDBString(dr["AccountNumber"]);

                    //#--
                    projectInfo.Status = Data.Utils.GetDBString(dr["Status"]);
                    projectInfo.CommencementDate = Data.Utils.GetDBDateTime(dr["CommencementDate"]);
                    projectInfo.CompletionDate = Data.Utils.GetDBDateTime(dr["CompletionDate"]);
                    projectInfo.DistributionListInfo = Data.Utils.GetDBString(dr["DistributionListInfo"]);
                    projectInfo.ContractAmount = Data.Utils.GetDBDecimal(dr["ContractAmount"]);
                    projectInfo.PaymentTerms = Data.Utils.GetDBString(dr["PaymentTerms"]);
                    projectInfo.ClaimFrequency = Data.Utils.GetDBString(dr["ClaimFrequency"]);
                    projectInfo.Waranty1Amount = Data.Utils.GetDBDecimal(dr["Waranty1Amount"]);
                    projectInfo.Waranty1Date = Data.Utils.GetDBDateTime(dr["Waranty1Date"]);
                    projectInfo.Waranty2Amount = Data.Utils.GetDBDecimal(dr["Waranty2Amount"]);
                    projectInfo.Waranty2Date = Data.Utils.GetDBDateTime(dr["Waranty2Date"]);
                    projectInfo.PracticalCompletionDate = Data.Utils.GetDBDateTime(dr["PracticalCompletionDate"]);
                    projectInfo.FirstClaimDueDate = Data.Utils.GetDBDateTime(dr["FirstClaimDueDate"]);


                    //#-------Site Address---San

                    projectInfo.Siteaddress = Data.Utils.GetDBString(dr["Siteaddress"]);
                    projectInfo.SiteSuburb = Data.Utils.GetDBString(dr["SiteSuburb"]);
                    projectInfo.SiteState = Data.Utils.GetDBString(dr["SiteState"]);
                    projectInfo.SitePostalCode = Data.Utils.GetDBString(dr["SitePostalCode"]);


                    //#----Site Address--San



                    //#-------Principal Address---San

                    projectInfo.Principaladdress = Data.Utils.GetDBString(dr["Principaladdress"]);
                    projectInfo.PrincipalSuburb = Data.Utils.GetDBString(dr["PrincipalSuburb"]);
                    projectInfo.PrincipalState = Data.Utils.GetDBString(dr["PrincipalState"]);
                    projectInfo.PrincipalPostalCode = Data.Utils.GetDBString(dr["PrincipalPostalCode"]);


                    //#----Principal Address--San






                    projectInfo.ClientContact = new ClientContactInfo();
                    projectInfo.ClientContact.FirstName = Data.Utils.GetDBString(dr["ClientContactFirstName"]);
                    projectInfo.ClientContact.LastName = Data.Utils.GetDBString(dr["ClientContactLastName"]);
                    projectInfo.ClientContact.Street = Data.Utils.GetDBString(dr["ClientContactStreet"]);
                    projectInfo.ClientContact.Locality = Data.Utils.GetDBString(dr["ClientContactLocality"]);
                    projectInfo.ClientContact.State = Data.Utils.GetDBString(dr["ClientContactState"]);
                    projectInfo.ClientContact.PostalCode = Data.Utils.GetDBString(dr["ClientContactPostalCode"]);
                    projectInfo.ClientContact.Phone = Data.Utils.GetDBString(dr["ClientContactPhone"]);
                    projectInfo.ClientContact.Fax = Data.Utils.GetDBString(dr["ClientContactFax"]);
                    projectInfo.ClientContact.Email = Data.Utils.GetDBString(dr["ClientContactEmail"]);
                    projectInfo.ClientContact.CompanyName = Data.Utils.GetDBString(dr["ClientContactCompanyName"]);

                    projectInfo.ClientContact1 = new ClientContactInfo();
                    projectInfo.ClientContact1.FirstName = Data.Utils.GetDBString(dr["ClientContact1FirstName"]);
                    projectInfo.ClientContact1.LastName = Data.Utils.GetDBString(dr["ClientContact1LastName"]);
                    projectInfo.ClientContact1.Street = Data.Utils.GetDBString(dr["ClientContact1Street"]);
                    projectInfo.ClientContact1.Locality = Data.Utils.GetDBString(dr["ClientContact1Locality"]);
                    projectInfo.ClientContact1.State = Data.Utils.GetDBString(dr["ClientContact1State"]);
                    projectInfo.ClientContact1.PostalCode = Data.Utils.GetDBString(dr["ClientContact1PostalCode"]);
                    projectInfo.ClientContact1.Phone = Data.Utils.GetDBString(dr["ClientContact1Phone"]);
                    projectInfo.ClientContact1.Fax = Data.Utils.GetDBString(dr["ClientContact1Fax"]);
                    projectInfo.ClientContact1.Email = Data.Utils.GetDBString(dr["ClientContact1Email"]);
                    projectInfo.ClientContact1.CompanyName = Data.Utils.GetDBString(dr["ClientContact1CompanyName"]);

                    projectInfo.ClientContact2 = new ClientContactInfo();
                    projectInfo.ClientContact2.FirstName = Data.Utils.GetDBString(dr["ClientContact2FirstName"]);
                    projectInfo.ClientContact2.LastName = Data.Utils.GetDBString(dr["ClientContact2LastName"]);
                    projectInfo.ClientContact2.Street = Data.Utils.GetDBString(dr["ClientContact2Street"]);
                    projectInfo.ClientContact2.Locality = Data.Utils.GetDBString(dr["ClientContact2Locality"]);
                    projectInfo.ClientContact2.State = Data.Utils.GetDBString(dr["ClientContact2State"]);
                    projectInfo.ClientContact2.PostalCode = Data.Utils.GetDBString(dr["ClientContact2PostalCode"]);
                    projectInfo.ClientContact2.Phone = Data.Utils.GetDBString(dr["ClientContact2Phone"]);
                    projectInfo.ClientContact2.Fax = Data.Utils.GetDBString(dr["ClientContact2Fax"]);
                    projectInfo.ClientContact2.Email = Data.Utils.GetDBString(dr["ClientContact2Email"]);
                    projectInfo.ClientContact2.CompanyName = Data.Utils.GetDBString(dr["ClientContact2CompanyName"]);

                    projectInfo.Superintendent = new ClientContactInfo();
                    projectInfo.Superintendent.FirstName = Data.Utils.GetDBString(dr["SuperintendentFirstName"]);
                    projectInfo.Superintendent.LastName = Data.Utils.GetDBString(dr["SuperintendentLastName"]);
                    projectInfo.Superintendent.Street = Data.Utils.GetDBString(dr["SuperintendentStreet"]);
                    projectInfo.Superintendent.Locality = Data.Utils.GetDBString(dr["SuperintendentLocality"]);
                    projectInfo.Superintendent.State = Data.Utils.GetDBString(dr["SuperintendentState"]);
                    projectInfo.Superintendent.PostalCode = Data.Utils.GetDBString(dr["SuperintendentPostalCode"]);
                    projectInfo.Superintendent.Phone = Data.Utils.GetDBString(dr["SuperintendentPhone"]);
                    projectInfo.Superintendent.Fax = Data.Utils.GetDBString(dr["SuperintendentFax"]);
                    projectInfo.Superintendent.Email = Data.Utils.GetDBString(dr["SuperintendentEmail"]);
                    projectInfo.Superintendent.CompanyName = Data.Utils.GetDBString(dr["SuperintendentCompanyName"]);


                    projectInfo.QuantitySurveyor = new ClientContactInfo();
                    projectInfo.QuantitySurveyor.FirstName = Data.Utils.GetDBString(dr["QuantitySurveyorFirstName"]);
                    projectInfo.QuantitySurveyor.LastName = Data.Utils.GetDBString(dr["QuantitySurveyorLastName"]);
                    projectInfo.QuantitySurveyor.Street = Data.Utils.GetDBString(dr["QuantitySurveyorStreet"]);
                    projectInfo.QuantitySurveyor.Locality = Data.Utils.GetDBString(dr["QuantitySurveyorLocality"]);
                    projectInfo.QuantitySurveyor.State = Data.Utils.GetDBString(dr["QuantitySurveyorState"]);
                    projectInfo.QuantitySurveyor.PostalCode = Data.Utils.GetDBString(dr["QuantitySurveyorPostalCode"]);
                    projectInfo.QuantitySurveyor.Phone = Data.Utils.GetDBString(dr["QuantitySurveyorPhone"]);
                    projectInfo.QuantitySurveyor.Fax = Data.Utils.GetDBString(dr["QuantitySurveyorFax"]);
                    projectInfo.QuantitySurveyor.Email = Data.Utils.GetDBString(dr["QuantitySurveyorEmail"]);
                    projectInfo.QuantitySurveyor.CompanyName = Data.Utils.GetDBString(dr["QuantitySurveyorCompanyName"]);

                    projectInfo.SecondPrincipal = new ClientContactInfo();
                    projectInfo.SecondPrincipal.FirstName = Data.Utils.GetDBString(dr["SecondPrincipalFirstName"]);
                    projectInfo.SecondPrincipal.LastName = Data.Utils.GetDBString(dr["SecondPrincipalLastName"]);
                    projectInfo.SecondPrincipal.Street = Data.Utils.GetDBString(dr["SecondPrincipalStreet"]);
                    projectInfo.SecondPrincipal.Locality = Data.Utils.GetDBString(dr["SecondPrincipalLocality"]);
                    projectInfo.SecondPrincipal.State = Data.Utils.GetDBString(dr["SecondPrincipalState"]);
                    projectInfo.SecondPrincipal.PostalCode = Data.Utils.GetDBString(dr["SecondPrincipalPostalCode"]);
                    projectInfo.SecondPrincipal.Phone = Data.Utils.GetDBString(dr["SecondPrincipalPhone"]);
                    projectInfo.SecondPrincipal.Fax = Data.Utils.GetDBString(dr["SecondPrincipalFax"]);
                    projectInfo.SecondPrincipal.Email = Data.Utils.GetDBString(dr["SecondPrincipalEmail"]);
                    projectInfo.SecondPrincipal.CompanyName = Data.Utils.GetDBString(dr["SecondPrincipalCompanyName"]);

                    projectInfo.DeepZoomUrl = Data.Utils.GetDBString(dr["DeepZoomUrl"]);
                    projectInfo.AttachmentsFolder = Data.Utils.GetDBString(dr["AttachmentsFolder"]);
                    projectInfo.MaintenanceManualFile = Data.Utils.GetDBString(dr["MaintenanceManualFile"]);
                    projectInfo.PostProjectReviewFile = Data.Utils.GetDBString(dr["PostProjectReviewFile"]);

                    AssignAuditInfo(projectInfo, dr);

                    if (dr["BusinessUnitId"] != DBNull.Value) projectInfo.BusinessUnit = ContractsController.GetInstance().GetBusinessUnit(Data.Utils.GetDBInt32(dr["BusinessUnitId"]));
                    if (dr["MDPeopleId"] != DBNull.Value) projectInfo.ManagingDirector = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["MDPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CAPeopleId"]));
                    if (dr["PMPeopleId"] != DBNull.Value) projectInfo.ProjectManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CMPeopleId"] != DBNull.Value) projectInfo.ConstructionManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CMPeopleId"]));
                    if (dr["ForemanPeopleId"] != DBNull.Value) projectInfo.Foreman = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["ForemanPeopleId"]));
                    if (dr["DMPeopleId"] != DBNull.Value) projectInfo.DesignManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["DMPeopleId"]));
                    if (dr["DCPeopleId"] != DBNull.Value) projectInfo.DesignCoordinator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["DCPeopleId"]));
                    if (dr["FCPeopleId"] != DBNull.Value) projectInfo.FinancialController = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["FCPeopleId"]));
                    if (dr["DAPeopleId"] != DBNull.Value) projectInfo.DirectorAuthorization = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["DAPeopleId"]));
                    if (dr["BAPeopleId"] != DBNull.Value) projectInfo.BudgetAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["BAPeopleId"]));
                    //#-----New Rolw COM and JCA
                    if (dr["COPeopleId"] != DBNull.Value) projectInfo.CommercialManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["COPeopleId"]));
                    if (dr["JCPeopleId"] != DBNull.Value) projectInfo.JuniorContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["JCPeopleId"]));

                    if (dr["CA3PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator3 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA3PeopleId"]));
                    if (dr["CA4PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator4 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA4PeopleId"]));
                    if (dr["CA5PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator5 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA5PeopleId"]));
                    if (dr["CA6PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator6 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA6PeopleId"]));



                    //#-----


                }

                dr.NextResult();
                projectInfo.TradesProjectManagers = new List<EmployeeInfo>();
                while (dr.Read())
                    projectInfo.TradesProjectManagers.Add((EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["PMPeopleId"])));

                dr.NextResult();
                projectInfo.TradesContractsAdministrators = new List<EmployeeInfo>();
                while (dr.Read())
                    projectInfo.TradesContractsAdministrators.Add((EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CAPeopleId"])));
                //#----
                dr.NextResult();
                projectInfo.ClientContactList = new List<ClientContactInfo>();
                while (dr.Read())
                {
                    ClientContactInfo clientContact = (ClientContactInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["ClientContactId"]));

                    if (clientContact != null)
                    {
                        clientContact.SendEOTs = Data.Utils.GetDBBoolean(dr["DistEOTs"]);
                        clientContact.SendRFIs = Data.Utils.GetDBBoolean(dr["DistRFIs"]);
                        clientContact.SendClaims = Data.Utils.GetDBBoolean(dr["DistClaims"]);
                        clientContact.SendSAs = Data.Utils.GetDBBoolean(dr["DistSaparateAccounts"]);
                        clientContact.SendCVs = Data.Utils.GetDBBoolean(dr["DistClientVariations"]);

                        clientContact.AttentionToEots = Data.Utils.GetDBBoolean(dr["AttentionEOT"]);
                        clientContact.AttentionToRFIs = Data.Utils.GetDBBoolean(dr["AttentionRFI"]);
                        clientContact.AttentionToClaims = Data.Utils.GetDBBoolean(dr["AttentionClaim"]);

                        clientContact.WebAccessToEOTs = Data.Utils.GetDBBoolean(dr["WebEots"]);
                        clientContact.WebAccessToRFIs = Data.Utils.GetDBBoolean(dr["WebRFIs"]);
                        clientContact.WebAccessToClaims = Data.Utils.GetDBBoolean(dr["WebClaims"]);
                        clientContact.WebAccessToSAs = Data.Utils.GetDBBoolean(dr["WebSeparateAccounts"]);
                        clientContact.WebAccessToCVs = Data.Utils.GetDBBoolean(dr["WebClientVariations"]);
                        clientContact.WebAccessToDocs = Data.Utils.GetDBBoolean(dr["WebDocs"]);
                        clientContact.WebAccessToPhotos = Data.Utils.GetDBBoolean(dr["WebPhotos"]);

                        projectInfo.ClientContactList.Add(clientContact);
                    }
                }
                //#---

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectInfo;
        }

        /// <summary>
        /// Gets the Project stakeholders
        /// </summary>
        public ProjectInfo GetProjectPeople(int? ProjectId, int? PMId, int? CAId)
        {
            IDataReader dr = null;
            ProjectInfo projectInfo = null;
            PeopleController peopleController = PeopleController.GetInstance();

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectPeople(ProjectId);
                if (dr.Read())
                {
                    projectInfo = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    projectInfo.BusinessUnit = new BusinessUnitInfo();

                    if (dr["MDPeopleId"] != DBNull.Value) projectInfo.ManagingDirector = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["MDPeopleId"]));
                    if (dr["CMPeopleId"] != DBNull.Value) projectInfo.ConstructionManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CMPeopleId"]));
                    if (dr["DMPeopleId"] != DBNull.Value) projectInfo.DesignManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["DMPeopleId"]));
                    // #---Com,JCA new role added
                    if (dr["COPeopleId"] != DBNull.Value) projectInfo.CommercialManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["COPeopleId"]));
                    if (dr["JCPeopleId"] != DBNull.Value) projectInfo.JuniorContractsAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["JCPeopleId"]));

                    if (dr["CA3PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator3 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA3PeopleId"]));
                    if (dr["CA4PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator4 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA4PeopleId"]));
                    if (dr["CA5PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator5 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA5PeopleId"]));
                    if (dr["CA6PeopleId"] != DBNull.Value) projectInfo.ContractsAdministrator6 = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CA6PeopleId"]));








                    // #---Com,JCA  new role added

                    if (dr["DCPeopleId"] != DBNull.Value) projectInfo.DesignCoordinator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["DCPeopleId"]));
                    if (dr["FCPeopleId"] != DBNull.Value) projectInfo.FinancialController = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["FCPeopleId"]));
                    if (dr["DAPeopleId"] != DBNull.Value) projectInfo.DirectorAuthorization = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["DAPeopleId"]));
                    if (dr["BAPeopleId"] != DBNull.Value) projectInfo.BudgetAdministrator = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["BAPeopleId"]));
                    if (dr["UMPeopleId"] != DBNull.Value) projectInfo.BusinessUnit.UnitManager = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["UMPeopleId"]));

                    projectInfo.ProjectManager = PMId != null ? (EmployeeInfo)peopleController.GetPersonById(PMId) : dr["PMPeopleId"] != DBNull.Value ? (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["PMPeopleId"])) : null;
                    projectInfo.ContractsAdministrator = CAId != null ? (EmployeeInfo)peopleController.GetPersonById(CAId) : dr["CAPeopleId"] != DBNull.Value ? (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CAPeopleId"])) : null;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project People from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectInfo;
        }

        /// <summary>
        /// Gets the Project stakeholders
        /// </summary>
        public ProjectInfo GetProjectPeople(int? ProjectId)
        {
            return GetProjectPeople(ProjectId, null, null);
        }

        /// <summary>
        /// Gets deep a Project from persistent storage
        /// </summary>
        public ProjectInfo GetDeepProject(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);

            projectInfo.Trades = GetDeepProjectTrades(projectInfo);
            projectInfo.Drawings = TradesController.GetInstance().GetDeepDrawings(projectInfo);

            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with Trades(no deep) from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithTradesParticipations(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Trades = GetProjectTradesWithParticipationsAndSubcontractsOptimized(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with ClienTrades
        /// </summary>
        public ProjectInfo GetProjectWithClientTrades(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.ClientTrades = GetClientTrades(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with Budgets
        /// </summary>
        public ProjectInfo GetProjectWithBudgets(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Budgets = GetBudgets(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with ClientVariations
        /// </summary>
        public ProjectInfo GetProjectWithClientVariations(int? ProjectId, String type)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.ClientVariations = GetClientVariationsLastRevisions(projectInfo, type);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with Tenant Variations   DS20240122 >>>
        /// </summary>
        public ProjectInfo GetProjectWithTenantVariations(int? ProjectId)
        {
            return GetProjectWithClientVariations(ProjectId, ClientVariationInfo.VariationTypeTenant);
        } //                                        DS20240122 <<<
        /// <summary>
        /// Gets a Project with ClientVariations
        /// </summary>
        public ProjectInfo GetProjectWithClientVariations(int? ProjectId)
        {
            return GetProjectWithClientVariations(ProjectId, ClientVariationInfo.VariationTypeClient);
        }

        /// <summary>
        /// Gets a Project with ClienTrades and ClientVariations
        /// </summary>
        public ProjectInfo GetProjectWithClientTradesAndVariations(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.ClientTrades = GetClientTrades(projectInfo);
            projectInfo.ClientVariations = GetClientVariationsLastRevisions(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with Claims
        /// </summary>
        public ProjectInfo GetProjectWithClaimsDeepTradesAndVariations(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Claims = GetClaimsWithTradesAndVariations(projectInfo);
            projectInfo.ClientTrades = GetClientTrades(projectInfo);
            projectInfo.ClientVariations = GetClientVariationsLastRevisions(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with Claims
        /// </summary>
        public ProjectInfo GetProjectWithClaimsDeep(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Claims = GetClaimsWithTradesAndVariations(projectInfo);
            return projectInfo;
        }

        /// <summary>
		/// Gets a Project with Claims
		/// </summary>
		public ProjectInfo GetProjectWithClaimsTradesAndVariations(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Claims = GetClaims(projectInfo);
            projectInfo.ClientTrades = GetClientTrades(projectInfo);
            projectInfo.ClientVariations = GetClientVariationsLastRevisions(projectInfo);
            projectInfo.TenantVariations = GetTenantVariationsLastRevisions(projectInfo);  //DS20240216
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with drawings from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithDrawings(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Drawings = TradesController.GetInstance().GetDeepDrawings(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with drawings from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithDrawingsAndTrades(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Drawings = TradesController.GetInstance().GetDeepDrawings(projectInfo);
            projectInfo.Trades = GetProjectTrades(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with active drawings from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithDrawingsActive(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Drawings = TradesController.GetInstance().GetDeepDrawingsActive(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with proposal drawings from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithDrawingsProposal(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Drawings = TradesController.GetInstance().GetDeepDrawingsProposal(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with transmittals from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithTransmittals(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.Transmittals = GetTransmittals(projectInfo);
            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with drawings and transmittals from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithDrawingsAndTransmittals(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);

            projectInfo.Drawings = TradesController.GetInstance().GetDrawingRevisions(projectInfo);
            projectInfo.Transmittals = GetTransmittals(projectInfo);

            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with RFIs from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithRFIs(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.RFIs = GetRFIs(projectInfo);

            if (projectInfo.RFIs != null)
                foreach (RFIInfo rFIInfo in projectInfo.RFIs)
                    rFIInfo.Project = projectInfo;

            return projectInfo;
        }

        /// <summary>
        /// Gets a Project with EOTs from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithEOTs(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.EOTs = GetEOTs(projectInfo);

            if (projectInfo.EOTs != null)
                foreach (EOTInfo eOTInfo in projectInfo.EOTs)
                    eOTInfo.Project = projectInfo;

            return projectInfo;
        }

        /// <summary>
        /// Gets a list of active projects where the Employee is playing a role in the approval processes
        /// </summary>
        public List<ProjectInfo> GetActiveProjectsForEmployee(EmployeeInfo employeeInfo)
        {
            IDataReader dr = null;
            ProjectInfo projectInfo = null;
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetActiveProjectsForEmployee(employeeInfo.Id);
                while (dr.Read())
                {
                    projectInfo = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    projectInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    projectInfo.ContractAmount = Data.Utils.GetDBDecimal(dr["ContractAmount"]);
                    projectInfo.CompletionDate = Data.Utils.GetDBDateTime(dr["CompletionDate"]);
                    projectInfo.MaintenanceManualFile = Data.Utils.GetDBString(dr["MaintenanceManualFile"]);
                    projectInfo.PostProjectReviewFile = Data.Utils.GetDBString(dr["PostProjectReviewFile"]);

                    projectInfoList.Add(projectInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting active projects for employee from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectInfoList;
        }
        /// <summary>
        /// Gets a list of active projects where the Employee is playing a role in the approval processes
        /// </summary>
        public List<ProjectInfo> GetActiveProjects(EmployeeInfo employeeInfo)  //DS20230823
        {
            IDataReader dr = null;
            ProjectInfo projectInfo = null;
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetActiveProjects(employeeInfo.Id);
                while (dr.Read())
                {
                    projectInfo = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    projectInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    projectInfo.ContractAmount = Data.Utils.GetDBDecimal(dr["ContractAmount"]);
                    projectInfo.CompletionDate = Data.Utils.GetDBDateTime(dr["CompletionDate"]);
                    projectInfo.MaintenanceManualFile = Data.Utils.GetDBString(dr["MaintenanceManualFile"]);
                    projectInfo.PostProjectReviewFile = Data.Utils.GetDBString(dr["PostProjectReviewFile"]);

                    projectInfoList.Add(projectInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting active projects for employee from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return projectInfoList;
        }
        /// <summary>
        /// Gets the roles names for a project
        /// </summary>
        public void GetProjectPeopleNames(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            EmployeeInfo employeeInfo;
            Dictionary<int, EmployeeInfo> tradesEmployees;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectsPeopleNames(projectInfo.Id);
                if (dr.Read())
                {
                    if (dr["MDPeopleId"] != DBNull.Value)
                    {
                        projectInfo.ManagingDirector = new EmployeeInfo(Data.Utils.GetDBInt32(dr["MDPeopleId"]));
                        projectInfo.ManagingDirector.FirstName = Data.Utils.GetDBString(dr["MDFirstName"]);
                        projectInfo.ManagingDirector.LastName = Data.Utils.GetDBString(dr["MDLastName"]);
                        projectInfo.ManagingDirector.LastLoginDate = Data.Utils.GetDBDateTime(dr["MDUserLastLogin"]);
                        projectInfo.ManagingDirector.Role = EmployeeInfo.TypeManagingDirector;
                    }

                    if (dr["CAPeopleId"] != DBNull.Value)
                    {
                        projectInfo.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));
                        projectInfo.ContractsAdministrator.FirstName = Data.Utils.GetDBString(dr["CAFirstName"]);
                        projectInfo.ContractsAdministrator.LastName = Data.Utils.GetDBString(dr["CALastName"]);
                        projectInfo.ContractsAdministrator.LastLoginDate = Data.Utils.GetDBDateTime(dr["CAUserLastLogin"]);
                        projectInfo.ContractsAdministrator.Role = EmployeeInfo.TypeContractsAdministrator;
                    }

                    if (dr["PMPeopleId"] != DBNull.Value)
                    {
                        projectInfo.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                        projectInfo.ProjectManager.FirstName = Data.Utils.GetDBString(dr["PMFirstName"]);
                        projectInfo.ProjectManager.LastName = Data.Utils.GetDBString(dr["PMLastName"]);
                        projectInfo.ProjectManager.LastLoginDate = Data.Utils.GetDBDateTime(dr["PMUserLastLogin"]);
                        projectInfo.ProjectManager.Role = EmployeeInfo.TypeProjectManager;
                    }

                    if (dr["CMPeopleId"] != DBNull.Value)
                    {
                        projectInfo.ConstructionManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CMPeopleId"]));
                        projectInfo.ConstructionManager.FirstName = Data.Utils.GetDBString(dr["CMFirstName"]);
                        projectInfo.ConstructionManager.LastName = Data.Utils.GetDBString(dr["CMLastName"]);
                        projectInfo.ConstructionManager.LastLoginDate = Data.Utils.GetDBDateTime(dr["CMUserLastLogin"]);
                        projectInfo.ConstructionManager.Role = EmployeeInfo.TypeConstructionsManager;
                    }

                    //#----COM----TypeCommercialManager --JCA--TypeJuniorContracts Administrator

                    if (dr["COPeopleId"] != DBNull.Value)
                    {
                        projectInfo.CommercialManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["COPeopleId"]));
                        projectInfo.CommercialManager.FirstName = Data.Utils.GetDBString(dr["COFirstName"]);
                        projectInfo.CommercialManager.LastName = Data.Utils.GetDBString(dr["COLastName"]);
                        projectInfo.CommercialManager.LastLoginDate = Data.Utils.GetDBDateTime(dr["COUserLastLogin"]);

                        projectInfo.CommercialManager.Role = EmployeeInfo.TypeCommercialManager;
                    }

                    if (dr["JCPeopleId"] != DBNull.Value)
                    {
                        projectInfo.JuniorContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["JCPeopleId"]));
                        projectInfo.JuniorContractsAdministrator.FirstName = Data.Utils.GetDBString(dr["JCFirstName"]);
                        projectInfo.JuniorContractsAdministrator.LastName = Data.Utils.GetDBString(dr["JCLastName"]);
                        projectInfo.JuniorContractsAdministrator.LastLoginDate = Data.Utils.GetDBDateTime(dr["JCUserLastLogin"]);

                        projectInfo.JuniorContractsAdministrator.Role = EmployeeInfo.TypeJuniorContractsAdministrator;
                    }

                    if (dr["CA3PeopleId"] != DBNull.Value)
                    {
                        projectInfo.ContractsAdministrator3 = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CA3PeopleId"]));
                        projectInfo.ContractsAdministrator3.FirstName = Data.Utils.GetDBString(dr["CA3FirstName"]);
                        projectInfo.ContractsAdministrator3.LastName = Data.Utils.GetDBString(dr["CA3LastName"]);
                        projectInfo.ContractsAdministrator3.LastLoginDate = Data.Utils.GetDBDateTime(dr["CA3UserLastLogin"]);
                        projectInfo.ContractsAdministrator3.Role = EmployeeInfo.TypeContractsAdministrator3;
                    }

                    if (dr["CA4PeopleId"] != DBNull.Value)
                    {
                        projectInfo.ContractsAdministrator4 = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CA4PeopleId"]));
                        projectInfo.ContractsAdministrator4.FirstName = Data.Utils.GetDBString(dr["CA4FirstName"]);
                        projectInfo.ContractsAdministrator4.LastName = Data.Utils.GetDBString(dr["CA4LastName"]);
                        projectInfo.ContractsAdministrator4.LastLoginDate = Data.Utils.GetDBDateTime(dr["CA4UserLastLogin"]);
                        projectInfo.ContractsAdministrator4.Role = EmployeeInfo.TypeContractsAdministrator4;
                    }

                    if (dr["CA5PeopleId"] != DBNull.Value)
                    {
                        projectInfo.ContractsAdministrator5 = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CA5PeopleId"]));
                        projectInfo.ContractsAdministrator5.FirstName = Data.Utils.GetDBString(dr["CA5FirstName"]);
                        projectInfo.ContractsAdministrator5.LastName = Data.Utils.GetDBString(dr["CA5LastName"]);
                        projectInfo.ContractsAdministrator5.LastLoginDate = Data.Utils.GetDBDateTime(dr["CA5UserLastLogin"]);
                        projectInfo.ContractsAdministrator5.Role = EmployeeInfo.TypeContractsAdministrator5;
                    }

                    if (dr["CA6PeopleId"] != DBNull.Value)
                    {
                        projectInfo.ContractsAdministrator6 = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CA6PeopleId"]));
                        projectInfo.ContractsAdministrator6.FirstName = Data.Utils.GetDBString(dr["CA6FirstName"]);
                        projectInfo.ContractsAdministrator6.LastName = Data.Utils.GetDBString(dr["CA6LastName"]);
                        projectInfo.ContractsAdministrator6.LastLoginDate = Data.Utils.GetDBDateTime(dr["CA6UserLastLogin"]);
                        projectInfo.ContractsAdministrator6.Role = EmployeeInfo.TypeContractsAdministrator6;
                    }


                    //#------COM and JCA

                    if (dr["DMPeopleId"] != DBNull.Value)
                    {
                        projectInfo.DesignManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["DMPeopleId"]));
                        projectInfo.DesignManager.FirstName = Data.Utils.GetDBString(dr["DMFirstName"]);
                        projectInfo.DesignManager.LastName = Data.Utils.GetDBString(dr["DMLastName"]);
                        projectInfo.DesignManager.LastLoginDate = Data.Utils.GetDBDateTime(dr["DMUserLastLogin"]);
                        projectInfo.DesignManager.Role = EmployeeInfo.TypeDesignManager;
                    }

                    if (dr["DCPeopleId"] != DBNull.Value)
                    {
                        projectInfo.DesignCoordinator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["DCPeopleId"]));
                        projectInfo.DesignCoordinator.FirstName = Data.Utils.GetDBString(dr["DCFirstName"]);
                        projectInfo.DesignCoordinator.LastName = Data.Utils.GetDBString(dr["DCLastName"]);
                        projectInfo.DesignCoordinator.LastLoginDate = Data.Utils.GetDBDateTime(dr["DCUserLastLogin"]);
                        projectInfo.DesignCoordinator.Role = EmployeeInfo.TypeDesignCoordinator;
                    }

                    if (dr["FCPeopleId"] != DBNull.Value)
                    {
                        projectInfo.FinancialController = new EmployeeInfo(Data.Utils.GetDBInt32(dr["FCPeopleId"]));
                        projectInfo.FinancialController.FirstName = Data.Utils.GetDBString(dr["FCFirstName"]);
                        projectInfo.FinancialController.LastName = Data.Utils.GetDBString(dr["FCLastName"]);
                        projectInfo.FinancialController.LastLoginDate = Data.Utils.GetDBDateTime(dr["FCUserLastLogin"]);
                        projectInfo.FinancialController.Role = EmployeeInfo.TypeFinancialController;
                    }

                    if (dr["DAPeopleId"] != DBNull.Value)
                    {
                        projectInfo.DirectorAuthorization = new EmployeeInfo(Data.Utils.GetDBInt32(dr["DAPeopleId"]));
                        projectInfo.DirectorAuthorization.FirstName = Data.Utils.GetDBString(dr["DAFirstName"]);
                        projectInfo.DirectorAuthorization.LastName = Data.Utils.GetDBString(dr["DALastName"]);
                        projectInfo.DirectorAuthorization.LastLoginDate = Data.Utils.GetDBDateTime(dr["DAUserLastLogin"]);
                        projectInfo.DirectorAuthorization.Role = EmployeeInfo.TypeDirectorAuthorizacion;
                    }

                    if (dr["BAPeopleId"] != DBNull.Value)
                    {
                        projectInfo.BudgetAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["BAPeopleId"]));
                        projectInfo.BudgetAdministrator.FirstName = Data.Utils.GetDBString(dr["BAFirstName"]);
                        projectInfo.BudgetAdministrator.LastName = Data.Utils.GetDBString(dr["BALastName"]);
                        projectInfo.BudgetAdministrator.LastLoginDate = Data.Utils.GetDBDateTime(dr["BAUserLastLogin"]);
                        projectInfo.BudgetAdministrator.Role = EmployeeInfo.TypeBudgetAdministrator;
                    }

                    if (dr["UMPeopleId"] != DBNull.Value)
                    {
                        if (projectInfo.BusinessUnit == null)
                            projectInfo.BusinessUnit = new BusinessUnitInfo();

                        projectInfo.BusinessUnit.UnitManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["UMPeopleId"]));
                        projectInfo.BusinessUnit.UnitManager.FirstName = Data.Utils.GetDBString(dr["UMFirstName"]);
                        projectInfo.BusinessUnit.UnitManager.LastName = Data.Utils.GetDBString(dr["UMLastName"]);
                        projectInfo.BusinessUnit.UnitManager.LastLoginDate = Data.Utils.GetDBDateTime(dr["UMUserLastLogin"]);
                        projectInfo.BusinessUnit.UnitManager.Role = EmployeeInfo.TypeUnitManager;
                    }
                }

                // Trade PMs
                dr.NextResult();
                projectInfo.TradesProjectManagers = new List<EmployeeInfo>();
                projectInfo.TradesProjectManagersByTrade = new Dictionary<int, EmployeeInfo>();
                tradesEmployees = new Dictionary<int, EmployeeInfo>();
                while (dr.Read())
                {
                    employeeInfo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    employeeInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    employeeInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    employeeInfo.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);
                    employeeInfo.Role = EmployeeInfo.TypeProjectManager;

                    projectInfo.TradesProjectManagersByTrade.Add(Convert.ToInt32(dr["TradeId"]), employeeInfo);

                    if (!employeeInfo.Equals(projectInfo.ProjectManager) && !tradesEmployees.ContainsKey(employeeInfo.Id.Value))
                        tradesEmployees.Add(employeeInfo.Id.Value, employeeInfo);

                    foreach (EmployeeInfo employee in tradesEmployees.Values)
                        projectInfo.TradesProjectManagers.Add(employeeInfo);
                }

                // Trade CAs
                dr.NextResult();
                projectInfo.TradesContractsAdministrators = new List<EmployeeInfo>();
                projectInfo.TradesContractsAdministratorsByTrade = new Dictionary<int, EmployeeInfo>();
                tradesEmployees = new Dictionary<int, EmployeeInfo>();
                while (dr.Read())
                {
                    employeeInfo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));
                    employeeInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    employeeInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    employeeInfo.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);
                    employeeInfo.Role = EmployeeInfo.TypeContractsAdministrator;

                    projectInfo.TradesContractsAdministratorsByTrade.Add(Convert.ToInt32(dr["TradeId"]), employeeInfo);

                    if (!employeeInfo.Equals(projectInfo.ContractsAdministrator) && !tradesEmployees.ContainsKey(employeeInfo.Id.Value))
                        tradesEmployees.Add(employeeInfo.Id.Value, employeeInfo);
                }

                foreach (EmployeeInfo employee in tradesEmployees.Values)
                    projectInfo.TradesContractsAdministrators.Add(employee);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting project people's info from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }

        /// <summary>
        /// Gets all the CAs for a PMs in the project trades
        /// </summary>
        public List<EmployeeInfo> GetProjectTradesCAs(ProjectInfo projectInfo, EmployeeInfo peopleInfo)
        {
            IDataReader dr = null;
            EmployeeInfo employeeInfo;
            List<Object> parameters = new List<Object>();
            List<EmployeeInfo> employeeInfoList = new List<EmployeeInfo>();

            parameters.Add(projectInfo.Id);
            parameters.Add(peopleInfo.Id);

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectTradesCAs(parameters.ToArray());
                if (dr.Read())
                {
                    employeeInfo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));

                    employeeInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    employeeInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    employeeInfo.Role = EmployeeInfo.TypeContractsAdministrator;

                    employeeInfoList.Add(employeeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting project trades CAs for PM info from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return employeeInfoList;
        }

        /// <summary>
        /// Gets all the CAs with no PM in the project trades
        /// </summary>
        public List<EmployeeInfo> GetProjectTradesCAs(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            EmployeeInfo employeeInfo;
            List<EmployeeInfo> employeeInfoList = new List<EmployeeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectTradesCAsWithNoPM(projectInfo.Id);
                if (dr.Read())
                {
                    employeeInfo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                    employeeInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    employeeInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);

                    employeeInfoList.Add(employeeInfo);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting project trades CAs info from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return employeeInfoList;
        }

        /// <summary>
        /// Gets a list of trades for a project and a contact
        /// </summary>
        public List<TradeInfo> GetTradesForContactAndProject(ContactInfo contactInfo, ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            TradesController tradesController = TradesController.GetInstance();

            if (contactInfo.SubContractor != null)
            {
                parameters.Add(projectInfo.Id);
                parameters.Add(contactInfo.SubContractor.Id);

                try
                {
                    dr = Data.DataProvider.GetInstance().GetTradesForSubcontractorAndProject(parameters.ToArray());
                    while (dr.Read())
                        tradeInfoList.Add(tradesController.GetTrade(Data.Utils.GetDBInt32(dr["TradeId"])));
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.ToString());
                    throw new Exception("Getting trades for contact and project from database");
                }
                finally
                {
                    if (dr != null)
                        dr.Close();
                }
            }

            return tradeInfoList;
        }

        /// <summary>
        /// Verifies if a person has the right to see a drawing revision.
        /// </summary>
        public Boolean AllowViewCurrentUser(DrawingRevisionInfo drawingRevisionInfo)
        {
            PeopleInfo peopleInfo = Web.Utils.GetCurrentUser();

            if (peopleInfo is EmployeeInfo)
                return true;

            //#----
            if (peopleInfo is ClientContactInfo)
            {
                if (((ClientContactInfo)peopleInfo).Projects.Find(x => x.Id.Equals(drawingRevisionInfo.Drawing.Project.Id)) != null)
                    return true;
            }

            //#---
            if (peopleInfo is ContactInfo)
            {
                ContactInfo contactInfo = (ContactInfo)peopleInfo;
                TradesController tradesController = TradesController.GetInstance();
                ProjectInfo projectInfo = GetProjectWithDrawingsAndTrades(drawingRevisionInfo.Drawing.Project.Id);

                foreach (TradeInfo tradeInfo in projectInfo.Trades)
                {
                    tradeInfo.Participations = tradesController.GetTradeParticipations(tradeInfo);

                    foreach (TradeParticipationInfo tradeParticipationInfo in tradeInfo.SubcontractorsParticipations)
                        if (tradeParticipationInfo.IsVisible && tradeParticipationInfo.SubContractor.Equals(contactInfo.SubContractor))
                        {
                            tradeInfo.Project = projectInfo;
                            tradeInfo.DrawingTypes = tradesController.GetDrawingTypes(tradeInfo);
                            tradeInfo.Drawings = tradesController.GetDrawings(tradeInfo);

                            if (tradeInfo.IncludedDrawings.Find(delegate (DrawingInfo drawingInfoInList) { return drawingRevisionInfo.Drawing.Equals(drawingInfoInList); }) != null)
                                return tradeParticipationInfo.IsAwarded || !tradeParticipationInfo.IsClosed ? true : drawingRevisionInfo.RevisionDate == null || drawingRevisionInfo.RevisionDate <= tradeParticipationInfo.QuoteDueDate;
                        }
                }
            }

            return false;
        }

        /// <summary>
        /// Verifies if a contact has the right to see a drawing revision within a trade participation
        /// </summary>
        public Boolean AllowViewCurrentUser(DrawingRevisionInfo drawingRevisionInfo, TradeParticipationInfo tradeParticipationInfo)
        {
            if (ContractsController.GetInstance().AllowViewCurrentUser(tradeParticipationInfo) && tradeParticipationInfo.Trade.IncludedDrawings.Find(delegate (DrawingInfo drawingInfoInList) { return drawingRevisionInfo.Drawing.Equals(drawingInfoInList); }) != null)
                return tradeParticipationInfo.IsAwarded || !tradeParticipationInfo.IsClosed ? true : drawingRevisionInfo.RevisionDate == null || drawingRevisionInfo.RevisionDate <= tradeParticipationInfo.QuoteDueDate;

            return false;
        }

        /// <summary>
        /// Throws and exception if the current user can not view information
        /// </summary>
        public void CheckViewCurrentUser(DrawingRevisionInfo drawingRevisionInfo)
        {
            if (!AllowViewCurrentUser(drawingRevisionInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not view information
        /// </summary>
        public void CheckViewCurrentUser(DrawingRevisionInfo drawingRevisionInfo, TradeParticipationInfo tradeParticipationInfo)
        {
            if (!AllowViewCurrentUser(drawingRevisionInfo, tradeParticipationInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Assign project info to parameters for Add and Update sprocs
        /// </summary>
        /// <param name="projectInfo"></param>
        /// <param name="parameters"></param>
        private void AssignProjecInfo(ProjectInfo projectInfo, List<Object> parameters)
        {
            parameters.Add(GetId(projectInfo.BusinessUnit));
            parameters.Add(projectInfo.Name);
            parameters.Add(projectInfo.Number);
            parameters.Add(projectInfo.Year);
            parameters.Add(projectInfo.Description);
            parameters.Add(projectInfo.Street);
            parameters.Add(projectInfo.Locality);
            parameters.Add(projectInfo.State);
            parameters.Add(projectInfo.PostalCode);
            parameters.Add(projectInfo.Fax);
            parameters.Add(GetId(projectInfo.ManagingDirector));
            parameters.Add(GetId(projectInfo.ContractsAdministrator));
            parameters.Add(GetId(projectInfo.ProjectManager));
            parameters.Add(GetId(projectInfo.ConstructionManager));
            parameters.Add(GetId(projectInfo.Foreman));
            parameters.Add(GetId(projectInfo.DesignManager));
            parameters.Add(GetId(projectInfo.DesignCoordinator));
            parameters.Add(GetId(projectInfo.FinancialController));
            parameters.Add(GetId(projectInfo.DirectorAuthorization));
            parameters.Add(GetId(projectInfo.BudgetAdministrator));

            //#----New Role COM and JCA
            parameters.Add(GetId(projectInfo.CommercialManager));
            parameters.Add(GetId(projectInfo.JuniorContractsAdministrator));

            parameters.Add(GetId(projectInfo.ContractsAdministrator3));
            parameters.Add(GetId(projectInfo.ContractsAdministrator4));
            parameters.Add(GetId(projectInfo.ContractsAdministrator5));
            parameters.Add(GetId(projectInfo.ContractsAdministrator6));
            //#---

            parameters.Add(projectInfo.DefectsLiability);
            parameters.Add(projectInfo.LiquidatedDamages);
            parameters.Add(projectInfo.SiteAllowances);
            parameters.Add(projectInfo.Retention);
            parameters.Add(projectInfo.RetentionToCertification);
            parameters.Add(projectInfo.RetentionToDLP);
            parameters.Add(projectInfo.Interest);
            parameters.Add(projectInfo.Principal);
            parameters.Add(projectInfo.PrincipalABN);
            parameters.Add(projectInfo.Principal2);
            parameters.Add(projectInfo.Principal2ABN);
            parameters.Add(projectInfo.SpecialClause);
            //#--
            parameters.Add(projectInfo.LawOfSubcontract);
            parameters.Add(projectInfo.AccountName);
            parameters.Add(projectInfo.BSB);
            parameters.Add(projectInfo.AccountNumber);

            //#--Site Address
            parameters.Add(projectInfo.Siteaddress);
            parameters.Add(projectInfo.SiteSuburb);
            parameters.Add(projectInfo.SiteState);
            parameters.Add(projectInfo.SitePostalCode);



            //#--Principal Address
            parameters.Add(projectInfo.Principaladdress);
            parameters.Add(projectInfo.PrincipalSuburb);
            parameters.Add(projectInfo.PrincipalState);
            parameters.Add(projectInfo.PrincipalPostalCode);




            //#--
            parameters.Add(projectInfo.Status);
            parameters.Add(projectInfo.CommencementDate);
            parameters.Add(projectInfo.CompletionDate);
            parameters.Add(projectInfo.DistributionListInfo);
            parameters.Add(projectInfo.ContractAmount);
            parameters.Add(projectInfo.PaymentTerms);
            parameters.Add(projectInfo.ClaimFrequency);
            parameters.Add(projectInfo.Waranty1Amount);
            parameters.Add(projectInfo.Waranty1Date);
            parameters.Add(projectInfo.Waranty2Amount);
            parameters.Add(projectInfo.Waranty2Date);
            parameters.Add(projectInfo.PracticalCompletionDate);
            parameters.Add(projectInfo.FirstClaimDueDate);

            parameters.Add(projectInfo.ClientContact.FirstName);
            parameters.Add(projectInfo.ClientContact.LastName);
            parameters.Add(projectInfo.ClientContact.Street);
            parameters.Add(projectInfo.ClientContact.Locality);
            parameters.Add(projectInfo.ClientContact.State);
            parameters.Add(projectInfo.ClientContact.PostalCode);
            parameters.Add(projectInfo.ClientContact.Phone);
            parameters.Add(projectInfo.ClientContact.Fax);
            parameters.Add(projectInfo.ClientContact.Email);
            parameters.Add(projectInfo.ClientContact.CompanyName);

            parameters.Add(projectInfo.ClientContact1.FirstName);
            parameters.Add(projectInfo.ClientContact1.LastName);
            parameters.Add(projectInfo.ClientContact1.Street);
            parameters.Add(projectInfo.ClientContact1.Locality);
            parameters.Add(projectInfo.ClientContact1.State);
            parameters.Add(projectInfo.ClientContact1.PostalCode);
            parameters.Add(projectInfo.ClientContact1.Phone);
            parameters.Add(projectInfo.ClientContact1.Fax);
            parameters.Add(projectInfo.ClientContact1.Email);
            parameters.Add(projectInfo.ClientContact1.CompanyName);

            parameters.Add(projectInfo.ClientContact2.FirstName);
            parameters.Add(projectInfo.ClientContact2.LastName);
            parameters.Add(projectInfo.ClientContact2.Street);
            parameters.Add(projectInfo.ClientContact2.Locality);
            parameters.Add(projectInfo.ClientContact2.State);
            parameters.Add(projectInfo.ClientContact2.PostalCode);
            parameters.Add(projectInfo.ClientContact2.Phone);
            parameters.Add(projectInfo.ClientContact2.Fax);
            parameters.Add(projectInfo.ClientContact2.Email);
            parameters.Add(projectInfo.ClientContact2.CompanyName);

            parameters.Add(projectInfo.Superintendent.FirstName);
            parameters.Add(projectInfo.Superintendent.LastName);
            parameters.Add(projectInfo.Superintendent.Street);
            parameters.Add(projectInfo.Superintendent.Locality);
            parameters.Add(projectInfo.Superintendent.State);
            parameters.Add(projectInfo.Superintendent.PostalCode);
            parameters.Add(projectInfo.Superintendent.Phone);
            parameters.Add(projectInfo.Superintendent.Fax);
            parameters.Add(projectInfo.Superintendent.Email);
            parameters.Add(projectInfo.Superintendent.CompanyName);

            parameters.Add(projectInfo.QuantitySurveyor.FirstName);
            parameters.Add(projectInfo.QuantitySurveyor.LastName);
            parameters.Add(projectInfo.QuantitySurveyor.Street);
            parameters.Add(projectInfo.QuantitySurveyor.Locality);
            parameters.Add(projectInfo.QuantitySurveyor.State);
            parameters.Add(projectInfo.QuantitySurveyor.PostalCode);
            parameters.Add(projectInfo.QuantitySurveyor.Phone);
            parameters.Add(projectInfo.QuantitySurveyor.Fax);
            parameters.Add(projectInfo.QuantitySurveyor.Email);
            parameters.Add(projectInfo.QuantitySurveyor.CompanyName);

            parameters.Add(projectInfo.SecondPrincipal.FirstName);
            parameters.Add(projectInfo.SecondPrincipal.LastName);
            parameters.Add(projectInfo.SecondPrincipal.Street);
            parameters.Add(projectInfo.SecondPrincipal.Locality);
            parameters.Add(projectInfo.SecondPrincipal.State);
            parameters.Add(projectInfo.SecondPrincipal.PostalCode);
            parameters.Add(projectInfo.SecondPrincipal.Phone);
            parameters.Add(projectInfo.SecondPrincipal.Fax);
            parameters.Add(projectInfo.SecondPrincipal.Email);
            parameters.Add(projectInfo.SecondPrincipal.CompanyName);

            parameters.Add(projectInfo.DeepZoomUrl);
            parameters.Add(projectInfo.AttachmentsFolder);
            parameters.Add(projectInfo.MaintenanceManualFile);
            parameters.Add(projectInfo.PostProjectReviewFile);
        }



        /// <summary>
        ///Get ProjectTrades With CAParticipations
        /// </summary>
        /// <param name="projectInfo"></param>
        /// <param name="parameters"></param>

        public List<TradeInfo> GetProjectTradesWithCAParticipations(ProjectInfo projectInfo)
        {
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();

            SubContractorsController subContractorsController = SubContractorsController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            PeopleController peopleController = PeopleController.GetInstance();

            Dictionary<int, TradeParticipationInfo> tradeParticipationsDictionary = new Dictionary<int, TradeParticipationInfo>();
            Dictionary<int, SubContractorInfo> subContractorsDictionary = new Dictionary<int, SubContractorInfo>();
            Dictionary<int, BusinessUnitInfo> businessUnitDictionary = new Dictionary<int, BusinessUnitInfo>();
            Dictionary<int, ProcessStepInfo> processStepsDictionary = new Dictionary<int, ProcessStepInfo>();
            Dictionary<int, ContractInfo> contractsDictionary = new Dictionary<int, ContractInfo>();
            Dictionary<int, ProcessInfo> processesDictionary = new Dictionary<int, ProcessInfo>();
            Dictionary<int, JobTypeInfo> jobTypesDictionary = new Dictionary<int, JobTypeInfo>();
            Dictionary<int, PeopleInfo> peopleDictionary = new Dictionary<int, PeopleInfo>();
            Dictionary<int, TradeInfo> tradesDictionary = new Dictionary<int, TradeInfo>();

            TradeParticipationInfo tradeParticipationInfo;
            SubContractorInfo subContractorInfo;
            ProcessStepInfo processStepInfo;
            ContractInfo contractInfo;
            ReversalInfo reversalInfo;
            JobTypeInfo jobTypeInfo;
            ProcessInfo processInfo;
            TradeInfo tradeInfo;

            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectAll(projectInfo.Id);

                // Job Types
                while (dr.Read())
                {
                    jobTypeInfo = contractsController.CreateJobType(dr);
                    jobTypesDictionary.Add(jobTypeInfo.Id.Value, jobTypeInfo);
                }

                // People not required
                // Business Units tot required

                // Subcontractors
                dr.NextResult();
                while (dr.Read())
                {
                    subContractorInfo = subContractorsController.CreateSubContractor(dr, businessUnitDictionary);
                    subContractorsDictionary.Add(subContractorInfo.Id.Value, subContractorInfo);
                }

                // Processes
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processController.CreateProcess(dr);
                    processesDictionary.Add(processInfo.Id.Value, processInfo);
                }

                dr.NextResult();
                processInfo = new ProcessInfo(0);
                while (dr.Read())
                {
                    processStepInfo = processController.CreateProcessStep(dr, peopleDictionary);
                    processStepsDictionary.Add(processStepInfo.Id.Value, processStepInfo);

                    if (processInfo.Id.Value != processStepInfo.Process.Id.Value)
                    {
                        processInfo = processesDictionary[processStepInfo.Process.Id.Value];
                        processInfo.Steps = new List<ProcessStepInfo>();
                    }

                    processInfo.Steps.Add(processStepInfo);
                    processStepInfo.Process = processInfo;
                }

                dr.NextResult();
                processStepInfo = new ProcessStepInfo(0);
                while (dr.Read())
                {
                    reversalInfo = processController.CreateReversal(dr, peopleDictionary);

                    if (processStepInfo.Id.Value != reversalInfo.ProcessStep.Id.Value)
                    {
                        processStepInfo = processStepsDictionary[reversalInfo.ProcessStep.Id.Value];
                        processStepInfo.Reversals = new List<ReversalInfo>();
                    }

                    processStepInfo.Reversals.Add(reversalInfo);
                    reversalInfo.ProcessStep = processStepInfo;
                }

                // Contracts
                dr.NextResult();
                while (dr.Read())
                {
                    contractInfo = contractsController.CreateContract(dr, processesDictionary);
                    contractsDictionary.Add(contractInfo.Id.Value, contractInfo);
                }

                foreach (ContractInfo subContract in contractsDictionary.Values)
                {
                    if (subContract.ParentContract != null)
                    {
                        contractInfo = contractsDictionary[subContract.ParentContract.Id.Value];

                        if (contractInfo.Subcontracts == null)
                            contractInfo.Subcontracts = new List<ContractInfo>();

                        contractInfo.Subcontracts.Add(subContract);
                    }
                }

                // Trades
                dr.NextResult();
                while (dr.Read())
                {
                    tradeInfo = tradesController.CreateTrade(dr, contractsDictionary, jobTypesDictionary, processesDictionary, peopleDictionary);
                    tradesDictionary.Add(tradeInfo.Id.Value, tradeInfo);
                    tradeInfoList.Add(tradeInfo);
                }

                //Trade Participations
                dr.NextResult();
                tradeInfo = new TradeInfo(0);
                while (dr.Read())
                {
                    tradeParticipationInfo = tradesController.CreateTradeParticipation(dr, subContractorsDictionary, peopleDictionary);
                    tradeParticipationsDictionary.Add(tradeParticipationInfo.Id.Value, tradeParticipationInfo);

                    if (tradeInfo.Id.Value != tradeParticipationInfo.Trade.Id.Value)
                    {
                        tradeInfo = tradesDictionary[tradeParticipationInfo.Trade.Id.Value];
                        tradeInfo.Participations = new List<TradeParticipationInfo>();
                    }

                    if (tradeParticipationInfo.ComparisonParticipation == null)
                        tradeInfo.Participations.Add(tradeParticipationInfo);
                }

                foreach (TradeParticipationInfo tradeParticipation in tradeParticipationsDictionary.Values)
                    if (tradeParticipation.ComparisonParticipation != null)
                        tradeParticipationsDictionary[tradeParticipation.ComparisonParticipation.Id.Value].QuoteParticipation = tradeParticipation;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }


        /// <summary>
        /// Creates the project folders
        /// </summary>
        public void CreateProjectFolders(ProjectInfo projectInfo)
        {
            String documentsFolder = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();
            String quotesFileFolder = ConfigurationManager.AppSettings["QuotesFilesFolder"].ToString();
            DirectoryInfo directoryInfo = null;

            if (projectInfo.AttachmentsFolder != null)
            {
                directoryInfo = new DirectoryInfo(String.Format("{0}\\{1}\\{2}", documentsFolder, projectInfo.AttachmentsFolder, quotesFileFolder));

                if (!directoryInfo.Exists)
                {
                    try
                    {
                        Directory.CreateDirectory(directoryInfo.FullName);
                    }
                    catch
                    {

                        throw new Exception("Creating project's quotes folder");
                    }
                }
            }
        }

        /// <summary>
        /// Updates a Project in the database
        /// </summary>
        public void UpdateProject(ProjectInfo projectInfo)
        {
            List<Object> parameters = new List<Object>();
            ProjectInfo originalProjectInfo;
            Boolean updateDates = false;

            try
            {
                SetModifiedInfo(projectInfo);

                parameters.Add(projectInfo.Id);

                AssignProjecInfo(projectInfo, parameters);

                parameters.Add(projectInfo.ModifiedDate);
                parameters.Add(projectInfo.ModifiedBy);

                originalProjectInfo = GetProject(projectInfo.Id);

                if (originalProjectInfo.CommencementDate == null)
                {
                    if (projectInfo.CommencementDate != null)
                        updateDates = true;
                }
                else
                    if (projectInfo.CommencementDate == null)
                    updateDates = true;
                else
                        if (!originalProjectInfo.CommencementDate.Value.Equals(projectInfo.CommencementDate.Value))
                    updateDates = true;

                if (updateDates)
                {
                    projectInfo.Trades = GetProjectTrades(projectInfo);
                    InitializeHolidays();

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
                    {
                        Data.DataProvider.GetInstance().UpdateProject(parameters.ToArray());
                        UpdateProjectDates(projectInfo);
                        scope.Complete();
                    }
                }
                else
                {
                    Data.DataProvider.GetInstance().UpdateProject(parameters.ToArray());
                }

             //    CreateProjectFolders(projectInfo);    DS2024-02-06
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Project in database");
            }
        }

        /// <summary>
        /// Updates a Project files in the database
        /// </summary>
        public void UpdateProjectFiles(ProjectInfo projectInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(projectInfo.Id);
            parameters.Add(projectInfo.MaintenanceManualFile);
            parameters.Add(projectInfo.PostProjectReviewFile);

            try
            {
                Data.DataProvider.GetInstance().UpdateProjectFiles(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Project files in database");
            }
        }

        /// <summary>
        /// Adds a Project to the database
        /// </summary>
        public int? AddProject(ProjectInfo projectInfo, PeopleInfo peopleInfo, ProcessStatus processStatus)
        {
            int? projectId = null;
            List<Object> parameters = new List<Object>();
            List<TradeTemplateInfo> tradeTemplateInfoList = new List<TradeTemplateInfo>();
            TradesController tradesController = TradesController.GetInstance();
            LocalDataStoreSlot localDataStoreSlot;

            try
            {
                localDataStoreSlot = Thread.GetNamedDataSlot(("CurrentUser"));
                if (localDataStoreSlot == null)
                    localDataStoreSlot = Thread.AllocateNamedDataSlot("CurrentUser");

                Thread.SetData(localDataStoreSlot, peopleInfo);

                processStatus.AddProcessStatusInfo("Reading trade templates.");
                tradeTemplateInfoList = tradesController.GetTradeTemplatesStandard(tradesController.GetDeepTradeTemplates());

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 5, 0)))
                {
                    SetCreateInfo(projectInfo);

                    AssignProjecInfo(projectInfo, parameters);

                    parameters.Add(projectInfo.CreatedDate);
                    parameters.Add(projectInfo.CreatedBy);

                    processStatus.AddProcessStatusInfo("Creating project.");
                    projectId = Data.DataProvider.GetInstance().AddProject(parameters.ToArray());
                    projectInfo.Id = projectId;

                    foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                    {
                        processStatus.AddProcessStatusInfo("Adding " + tradeTemplateInfo.Trade.Name + ".");
                        tradesController.CopyTradeTemplate(projectInfo, tradeTemplateInfo);
                    }

                    scope.Complete();
                }

                if (projectInfo.CommencementDate != null)
                {
                    processStatus.AddProcessStatusInfo("Updating dates.");
                    InitializeHolidays();
                    projectInfo.Trades = GetProjectTrades(projectInfo);
                    UpdateProjectDates(projectInfo);
                }

                processStatus.AddProcessStatusInfo("Creating project folders.");
                CreateProjectFolders(projectInfo);
                processStatus.CompleteProcess("Complete.");
            }
            catch (Exception ex)
            {
                processStatus.CompleteProcess("Error Adding project.");

                Utils.LogError(ex.ToString());
                throw new Exception("Adding Project to database");
            }
            finally
            {
                Thread.FreeNamedDataSlot("CurrentUser");
            }

            return projectId;
        }

        /// <summary>
        /// Initiates AddProject method execution asynchronously
        /// </summary>
        public IAsyncResult StartAddProject(ProjectInfo projectInfo)
        {
            AddProjectDelegate addProjectDelegate = new AddProjectDelegate(AddProject);
            ProcessStatus processStatus = new ProcessStatus();
            IAsyncResult iAsyncResult = addProjectDelegate.BeginInvoke(projectInfo, Web.Utils.GetCurrentUser(), processStatus, null, processStatus);

            return iAsyncResult;
        }

        /// <summary>
        /// Ends the invocation of AddProject method
        /// </summary>
        public int? EndAddProject(IAsyncResult iAsyncResult)
        {
            AsyncResult asyncResult = (AsyncResult)iAsyncResult;
            AddProjectDelegate addProjectDelegate = (AddProjectDelegate)asyncResult.AsyncDelegate;
            return addProjectDelegate.EndInvoke(asyncResult);
        }

        /// <summary>
        /// Sets the Comparison Due days and Contrat Due days for all project trades
        /// </summary>
        public void SetProjectDueDays(ProjectInfo projectInfo)
        {
            if (projectInfo.Trades != null)
            {
                InitializeHolidays();

                foreach (TradeInfo tradeInfo in projectInfo.Trades)
                    SetTradeDueDays(tradeInfo);
            }
        }

        /// <summary>
        /// Sets a trade's Comparison and Contrat Due days
        /// </summary>
        public void SetTradeDueDays(TradeInfo tradeInfo)
        {
            if (!tradeInfo.ComparisonApproved)
                tradeInfo.ComparisonDueDays = NumBusinessDays(tradeInfo.ComparisonDueDate);

            if (!tradeInfo.ContractApproved)
                tradeInfo.ContractDueDays = NumBusinessDays(tradeInfo.ContractDueDate);
        }

        /// <summary>
        /// Updates all the project dates based on the Project Commencement Date PCD
        /// </summary>
        public void UpdateProjectDates(ProjectInfo projectInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (TradeInfo tradeInfo in projectInfo.Trades)
                        if (tradeInfo.DaysFromPCD != null)
                        {
                            tradeInfo.Project = projectInfo;
                            UpdateTradeDates(tradeInfo);
                        }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating project dates in database");
            }
        }

        /// <summary>
        /// Updates all the trade dates based on the Project Commencement Date PCD.
        /// Trade must have DaysFromPCD not null.
        /// </summary>
        public void UpdateTradeDates(TradeInfo tradeInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            DateTime? initialDate;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    initialDate = AddbusinessDaysToDate(tradeInfo.Project.CommencementDate, (Int32)tradeInfo.DaysFromPCD);

                    tradeInfo.InvitationDate = AddbusinessDaysToDate(initialDate, Int32.Parse(Web.Utils.GetConfigListItemValue("Trades", "Settings", "DaysToInvitationDate")));
                    tradeInfo.DueDate = AddbusinessDaysToDate(initialDate, Int32.Parse(Web.Utils.GetConfigListItemValue("Trades", "Settings", "DaysToDueDate")));

                    if (!tradeInfo.ComparisonApproved)
                        tradeInfo.ComparisonDueDate = AddbusinessDaysToDate(initialDate, Int32.Parse(Web.Utils.GetConfigListItemValue("Trades", "Settings", "DaysToComparisonDueDate")));

                    if (!tradeInfo.ContractApproved)
                        tradeInfo.ContractDueDate = AddbusinessDaysToDate(initialDate, Int32.Parse(Web.Utils.GetConfigListItemValue("Trades", "Settings", "DaysToContractDueDate")));

                    tradesController.UpdateTradeDates(tradeInfo);

                    if (tradeInfo.Process != null)
                        processController.UpdateProcessDates(this, tradeInfo.Process, initialDate);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating trade dates in database");
            }
        }

        /// <summary>
        /// Adds or subtract a number of business days from a date and return the new date
        /// </summary>
        public DateTime? AddbusinessDaysToDate(DateTime? dateTime, Int32 numDays)
        {
            if (dateTime == null)
                return null;

            DateTime nextDate = (DateTime)dateTime;
            Int32 increment = numDays > 0 ? 1 : -1;
            Int32 currentDays = 0;

            numDays = Math.Abs(numDays);

            while (currentDays < numDays)
            {
                nextDate = nextDate.AddDays(increment);
                if (IsBusinessDay(nextDate))
                    currentDays++;
            }

            return nextDate;
        }

        /// <summary>
        /// Returns the number of business days between today and a date
        /// </summary>
        public int? NumBusinessDays(DateTime? fromDateTime, DateTime? toDateTime)
        {
            if (fromDateTime == null || toDateTime == null)
                return null;

            //remove the times from the dates
            fromDateTime = new DateTime(fromDateTime.Value.Year, fromDateTime.Value.Month, fromDateTime.Value.Day);
            toDateTime = new DateTime(toDateTime.Value.Year, toDateTime.Value.Month, toDateTime.Value.Day);

            Int32 numDays = 0;
            DateTime nextDate = (DateTime)fromDateTime;
            Int32 increment = nextDate < toDateTime ? 1 : -1;

            while (nextDate != toDateTime)
            {
                nextDate = nextDate.AddDays(increment);

                if (IsBusinessDay(nextDate))
                    numDays = numDays + increment;
            }

            return numDays;
        }

        /// <summary>
        /// Returns the number of business days between today and a date
        /// </summary>
        public int? NumBusinessDays(DateTime? dateTime)
        {
            return NumBusinessDays(dateTime, DateTime.Today);
        }

        /// <summary>
        /// Returns the number of calendar days between today and a date
        /// </summary>
        public int? NumCalendarDays(DateTime? dateTime)
        {
            if (dateTime == null)
                return null;
            else
                return DateTime.Today.Subtract(dateTime.Value).Days;
        }

        /// <summary>
        /// Checks if a date is a business day
        /// </summary>
        public Boolean IsBusinessDay(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
                return false;

            if (holidays.BinarySearch(dateTime) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Remove a Project from persistent storage
        /// </summary>
        public void DeleteProject(ProjectInfo projectInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteProject(projectInfo.Id);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Removing Project from database. In order to delete a project all its trades must be deleted.");
            }
        }

        /// <summary>
        /// Gets all the file names in a project from active to archive
        /// </summary>
        public List<Info> GetProjectFiles(int? ProjectId)
        {
            IDataReader dr = null;
            List<Info> infoList = new List<Info>();
            ClientVariationInfo clientVariationInfo;
            DrawingRevisionInfo drawingRevisionInfo;
            ContractInfo contractInfo;
            TradeInfo tradeInfo;
            EOTInfo eOTInfo;
            RFIInfo rFIInfo;

            try
            {
                dr = Data.DataProvider.GetInstance().GetFilesForProject(ProjectId);

                //Trades
                while (dr.Read())
                {
                    tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeInfo.QuotesFile = Data.Utils.GetDBString(dr["QuotesFile"]);
                    tradeInfo.PrelettingFile = Data.Utils.GetDBString(dr["PrelettingFile"]);

                    infoList.Add(tradeInfo);
                }

                //Contracts
                dr.NextResult();
                while (dr.Read())
                {
                    contractInfo = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));
                    contractInfo.QuotesFile = Data.Utils.GetDBString(dr["QuotesFile"]);

                    infoList.Add(contractInfo);
                }

                //ClientVariations
                dr.NextResult();
                while (dr.Read())
                {
                    clientVariationInfo = new ClientVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"]));
                    clientVariationInfo.QuotesFile = Data.Utils.GetDBString(dr["QuotesFile"]);
                    clientVariationInfo.BackupFile = Data.Utils.GetDBString(dr["BackupFile"]);
                    clientVariationInfo.ClientApprovalFile = Data.Utils.GetDBString(dr["ClientApprovalFile"]);

                    infoList.Add(clientVariationInfo);
                }

                //Drawing Revisions
                dr.NextResult();
                while (dr.Read())
                {
                    drawingRevisionInfo = new DrawingRevisionInfo(Data.Utils.GetDBInt32(dr["DrawingRevisionId"]));
                    drawingRevisionInfo.File = Data.Utils.GetDBString(dr["FilePath"]);

                    infoList.Add(drawingRevisionInfo);
                }

                //RFIs
                dr.NextResult();
                while (dr.Read())
                {
                    rFIInfo = new RFIInfo(Data.Utils.GetDBInt32(dr["RFIId"]));
                    rFIInfo.ReferenceFile = Data.Utils.GetDBString(dr["ReferenceFile"]);
                    rFIInfo.ClientResponseFile = Data.Utils.GetDBString(dr["ClientResponseFile"]);

                    infoList.Add(rFIInfo);
                }

                //EOT
                dr.NextResult();
                while (dr.Read())
                {
                    eOTInfo = new EOTInfo(Data.Utils.GetDBInt32(dr["EOTId"]));
                    eOTInfo.ClientApprovalFile = Data.Utils.GetDBString(dr["ClientApprovalFile"]);

                    infoList.Add(eOTInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting project files from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return infoList;
        }

        /// <summary>
        /// Updates the file paths from a list of info objects from the project
        /// </summary>
        public Int32 UpdateProjectFiles(ProjectInfo projectInfo, String activePath, String archivePath, ProcessStatus processStatus)
        {
            List<Info> infoList;
            List<Object> parameters = new List<Object>();
            Int32 currentRecord = 0;
            Int32 updatedFiles = 0;
            Boolean updateFiles;
            TradeInfo tradeInfo;
            ContractInfo contractInfo;
            ClientVariationInfo clientVariationInfo;
            DrawingRevisionInfo drawingRevisionInfo;
            RFIInfo rFIInfo;
            EOTInfo eOTInfo;

            processStatus.IsComplete = false;
            processStatus.PercentageCompletion = 0;

            try
            {
                infoList = GetProjectFiles(projectInfo.Id);

                Int32 totalRecords = infoList.Count;

                foreach (Info info in infoList)
                {
                    if (info is TradeInfo)
                    {
                        tradeInfo = (TradeInfo)info;

                        updateFiles = false;

                        if (tradeInfo.QuotesFile != null && tradeInfo.QuotesFile.StartsWith(activePath))
                        {
                            tradeInfo.QuotesFile = archivePath + tradeInfo.QuotesFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (tradeInfo.PrelettingFile != null && tradeInfo.PrelettingFile.StartsWith(activePath))
                        {
                            tradeInfo.PrelettingFile = archivePath + tradeInfo.PrelettingFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (updateFiles)
                        {
                            parameters.Clear();

                            parameters.Add(tradeInfo.Id);
                            parameters.Add(tradeInfo.QuotesFile);
                            parameters.Add(tradeInfo.PrelettingFile);

                            DataProvider.GetInstance().UpdateTradeFiles(parameters.ToArray());
                        }
                    }
                    else if (info is ContractInfo)
                    {
                        contractInfo = (ContractInfo)info;

                        updateFiles = false;

                        if (contractInfo.QuotesFile != null && contractInfo.QuotesFile.StartsWith(activePath))
                        {
                            contractInfo.QuotesFile = archivePath + contractInfo.QuotesFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (updateFiles)
                        {
                            parameters.Clear();

                            parameters.Add(contractInfo.Id);
                            parameters.Add(contractInfo.QuotesFile);

                            DataProvider.GetInstance().UpdateContractFiles(parameters.ToArray());
                        }
                    }
                    else if (info is ClientVariationInfo)
                    {
                        clientVariationInfo = (ClientVariationInfo)info;

                        updateFiles = false;

                        if (clientVariationInfo.QuotesFile != null && clientVariationInfo.QuotesFile.StartsWith(activePath))
                        {
                            clientVariationInfo.QuotesFile = archivePath + clientVariationInfo.QuotesFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (clientVariationInfo.BackupFile != null && clientVariationInfo.BackupFile.StartsWith(activePath))
                        {
                            clientVariationInfo.BackupFile = archivePath + clientVariationInfo.BackupFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (clientVariationInfo.ClientApprovalFile != null && clientVariationInfo.ClientApprovalFile.StartsWith(activePath))
                        {
                            clientVariationInfo.ClientApprovalFile = archivePath + clientVariationInfo.ClientApprovalFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (updateFiles)
                        {
                            parameters.Clear();

                            parameters.Add(clientVariationInfo.Id);
                            parameters.Add(clientVariationInfo.QuotesFile);
                            parameters.Add(clientVariationInfo.BackupFile);
                            parameters.Add(clientVariationInfo.ClientApprovalFile);

                            DataProvider.GetInstance().UpdateClientVariationFiles(parameters.ToArray());
                        }
                    }
                    else if (info is DrawingRevisionInfo)
                    {
                        drawingRevisionInfo = (DrawingRevisionInfo)info;

                        updateFiles = false;

                        if (drawingRevisionInfo.File != null && drawingRevisionInfo.File.StartsWith(activePath))
                        {
                            drawingRevisionInfo.File = archivePath + drawingRevisionInfo.File.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (updateFiles)
                        {
                            parameters.Clear();

                            parameters.Add(drawingRevisionInfo.Id);
                            parameters.Add(drawingRevisionInfo.File);

                            DataProvider.GetInstance().UpdateDrawingRevisionFiles(parameters.ToArray());
                        }
                    }
                    else if (info is RFIInfo)
                    {
                        rFIInfo = (RFIInfo)info;

                        updateFiles = false;

                        if (rFIInfo.ReferenceFile != null && rFIInfo.ReferenceFile.StartsWith(activePath))
                        {
                            rFIInfo.ReferenceFile = archivePath + rFIInfo.ReferenceFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (rFIInfo.ClientResponseFile != null && rFIInfo.ClientResponseFile.StartsWith(activePath))
                        {
                            rFIInfo.ClientResponseFile = archivePath + rFIInfo.ClientResponseFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (updateFiles)
                        {
                            parameters.Clear();

                            parameters.Add(rFIInfo.Id);
                            parameters.Add(rFIInfo.ReferenceFile);
                            parameters.Add(rFIInfo.ClientResponseFile);

                            DataProvider.GetInstance().UpdateRFIFiles(parameters.ToArray());
                        }
                    }
                    else if (info is EOTInfo)
                    {
                        eOTInfo = (EOTInfo)info;

                        updateFiles = false;

                        if (eOTInfo.ClientApprovalFile != null && eOTInfo.ClientApprovalFile.StartsWith(activePath))
                        {
                            eOTInfo.ClientApprovalFile = archivePath + eOTInfo.ClientApprovalFile.Substring(activePath.Length);
                            updateFiles = true;
                            updatedFiles++;
                        }

                        if (updateFiles)
                        {
                            parameters.Clear();

                            parameters.Add(eOTInfo.Id);
                            parameters.Add(eOTInfo.ClientApprovalFile);

                            DataProvider.GetInstance().UpdateEOTFiles(parameters.ToArray());
                        }
                    }

                    currentRecord++;
                    processStatus.PercentageCompletion = (Int32)Math.Round(((decimal)currentRecord / (decimal)totalRecords) * 100, 0);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating project files in the database");
            }

            processStatus.CompleteProcess(String.Empty);

            return updatedFiles;
        }

        /// <summary>
        /// Initiates the UpdateProjectFiles method
        /// </summary>
        public IAsyncResult StartUpdateProjectFiles(ProjectInfo projectInfo, String activePath, String archivePath)
        {
            UpdateProjectFilesDelegate updateProjectFilesDelegate = new UpdateProjectFilesDelegate(UpdateProjectFiles);
            ProcessStatus processStatus = new ProcessStatus();
            IAsyncResult iAsyncResult = updateProjectFilesDelegate.BeginInvoke(projectInfo, activePath, archivePath, processStatus, null, processStatus);

            return iAsyncResult;
        }

        /// <summary>
        /// Ends the UpdateProjectFiles method
        /// </summary>
        public Int32 EndUpdateProjectFiles(IAsyncResult iAsyncResult)
        {
            AsyncResult asyncResult = (AsyncResult)iAsyncResult;
            UpdateProjectFilesDelegate updateProjectFilesDelegate = (UpdateProjectFilesDelegate)asyncResult.AsyncDelegate;
            return updateProjectFilesDelegate.EndInvoke(asyncResult);
        }

        /// <summary>
        /// Copies the drawing register from proposal to active
        /// </summary>
        public void CopyDrawingRegister(ProjectInfo projectInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            List<DrawingInfo> drawingsProposal = projectInfo.DrawingsProposal;

            if (!projectInfo.IsEmptyDrawingsActvie)
                throw new Exception("The active drawing register must be empty");

            if (drawingsProposal != null)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        foreach (DrawingInfo drawing in drawingsProposal)
                        {
                            drawing.Type = DrawingInfo.TypeActive;
                            drawing.Id = tradesController.AddDrawing(drawing);

                            if (drawing.DrawingRevisions != null)
                                foreach (DrawingRevisionInfo drawingRevision in drawing.DrawingRevisions)
                                    tradesController.AddDrawingRevision(drawingRevision);
                        }

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex.ToString());
                    throw new Exception("Copying drawing register");
                }
            }
        }




        #endregion


        #region Transmittals Methods
        /// <summary>
        /// Creates a Transmittal from a dr
        /// </summary>
        public TransmittalInfo CreateTransmittal(IDataReader dr)
        {
            TransmittalInfo transmittalInfo = new TransmittalInfo(Data.Utils.GetDBInt32(dr["TransmittalId"]));

            transmittalInfo.Type = Data.Utils.GetDBString(dr["TransmittalTheType"]);
            transmittalInfo.TransmittalNumber = Data.Utils.GetDBInt32(dr["TransmittalNumber"]);
            transmittalInfo.TransmissionDate = Data.Utils.GetDBDateTime(dr["TransmissionDate"]);
            transmittalInfo.DeliveryMethod = Data.Utils.GetDBString(dr["DeliveryMethod"]);
            transmittalInfo.TransmittalType = Data.Utils.GetDBString(dr["TransmittalType"]);
            transmittalInfo.RequiredAction = Data.Utils.GetDBString(dr["RequiredAction"]);
            transmittalInfo.TransmittalTypeOther = Data.Utils.GetDBString(dr["TransmittalTypeOther"]);
            transmittalInfo.RequiredActionOther = Data.Utils.GetDBString(dr["RequiredActionOther"]);
            transmittalInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);
            transmittalInfo.SendClientContact = Data.Utils.GetDBBoolean(dr["SendClientContact"]);
            transmittalInfo.SendClientContact1 = Data.Utils.GetDBBoolean(dr["SendClientContact1"]);
            transmittalInfo.SendClientContact2 = Data.Utils.GetDBBoolean(dr["SendClientContact2"]);
            transmittalInfo.SendSuperintendent = Data.Utils.GetDBBoolean(dr["SendSuperintendent"]);
            transmittalInfo.SendQuantitySurveyor = Data.Utils.GetDBBoolean(dr["SendQuantitySurveyor"]);
            transmittalInfo.SentDate = Data.Utils.GetDBDateTime(dr["SentDate"]);

            AssignAuditInfo(transmittalInfo, dr);

            if (dr["ProjectId"] != DBNull.Value)
            {
                transmittalInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                transmittalInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                transmittalInfo.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
            }

            if (dr["ContactPeopleId"] != DBNull.Value)
            {
                transmittalInfo.Contact = (ContactInfo)PeopleController.GetInstance().GetPersonById(Data.Utils.GetDBInt32(dr["ContactPeopleId"]));
                if (transmittalInfo.Contact.SubContractor != null)
                    transmittalInfo.Contact.SubContractor = SubContractorsController.GetInstance().GetSubContractorDeep(transmittalInfo.Contact.SubContractor.Id);
            }

            return transmittalInfo;
        }

        /// <summary>
        /// Get a Transmittal from persistent storage
        /// </summary>
        public TransmittalInfo GetTransmittal(int? transmittalId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetTransmittal(transmittalId);
                if (dr.Read())
                    return CreateTransmittal(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Transmittal from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Creates a TransmittalRevision from a dr
        /// </summary>
        public TransmittalRevisionInfo CreateTransmittalRevision(IDataReader dr)
        {
            TransmittalRevisionInfo transmittalRevisionInfo = new TransmittalRevisionInfo();
            transmittalRevisionInfo.NumCopies = Data.Utils.GetDBInt32(dr["NumCopies"]);
            transmittalRevisionInfo.Revision = TradesController.GetInstance().CreateDrawingRevision(dr);
            transmittalRevisionInfo.Revision.Drawing.Description = Data.Utils.GetDBString(dr["DrawingDescription"]);

            if (dr["TransmittalId"] != DBNull.Value)
                transmittalRevisionInfo.Transmittal = new TransmittalInfo(Data.Utils.GetDBInt32(dr["TransmittalId"]));

            return transmittalRevisionInfo;
        }

        /// <summary>
        /// Returns the list of transmittal revisions for a transmittal
        /// </summary>
        public List<TransmittalRevisionInfo> GetTransmittalRevisions(TransmittalInfo transmittalInfo)
        {
            IDataReader dr = null;
            List<TransmittalRevisionInfo> transmittalRevisionList = new List<TransmittalRevisionInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTransmittalRevisionsByTransmittal(transmittalInfo.Id);
                while (dr.Read())
                    transmittalRevisionList.Add(CreateTransmittalRevision(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Transmittal Revisions from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return transmittalRevisionList;
        }

        /// <summary>
        /// Creates a TransmittalContact from a dr
        /// </summary>
        public ContactInfo CreateTransmittalContact(IDataReader dr)
        {
            ContactInfo contactInfo = new ContactInfo();

            contactInfo.Id = Data.Utils.GetDBInt32(dr["PeopleId"]);
            contactInfo.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
            contactInfo.LastName = Data.Utils.GetDBString(dr["LastName"]);
            contactInfo.Email = Data.Utils.GetDBString(dr["Email"]);

            contactInfo.Street = Data.Utils.GetDBString(dr["Street"]);
            contactInfo.Locality = Data.Utils.GetDBString(dr["Locality"]);
            contactInfo.State = Data.Utils.GetDBString(dr["State"]);
            contactInfo.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);

            if (dr["SubContractorId"] != DBNull.Value)
            {
                SubContractorInfo subContractorInfo = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"]));
                subContractorInfo.Name = Data.Utils.GetDBString(dr["SubContractorName"]);
                subContractorInfo.Street = Data.Utils.GetDBString(dr["SubContractorStreet"]);
                subContractorInfo.Locality = Data.Utils.GetDBString(dr["SubContractorLocality"]);
                subContractorInfo.State = Data.Utils.GetDBString(dr["SubContractorState"]);
                subContractorInfo.PostalCode = Data.Utils.GetDBString(dr["SubContractorPostalCode"]);

                contactInfo.SubContractor = subContractorInfo;

                if (contactInfo.Street == null && subContractorInfo.Street != null)
                {
                    contactInfo.Street = subContractorInfo.Street;
                    contactInfo.Locality = subContractorInfo.Locality;
                    contactInfo.State = subContractorInfo.State;
                    contactInfo.PostalCode = subContractorInfo.PostalCode;
                }
            }

            return contactInfo;
        }

        /// <summary>
        /// Returns the list of contacts for a transmittal
        /// </summary>
        public List<ContactInfo> GetTransmittalContacts(TransmittalInfo transmittalInfo)
        {
            IDataReader dr = null;
            List<ContactInfo> contactInfoList = new List<ContactInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetContactsByTransmittal(transmittalInfo.Id);
                while (dr.Read())
                    contactInfoList.Add(CreateTransmittalContact(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Transmittal contacts from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return contactInfoList;
        }





        /// <summary>
        /// Get a Transmittal with transmittal revisions and contacts from persistent storage.
        /// </summary>
        public TransmittalInfo GetDeepTransmittal(int? transmittalId)
        {
            TransmittalInfo transmittalInfo = GetTransmittal(transmittalId);
            transmittalInfo.TransmittalRevisions = GetTransmittalRevisions(transmittalInfo);
            transmittalInfo.Contacts = GetTransmittalContacts(transmittalInfo);
            //#--
            transmittalInfo.ClientContacts = GetTransmittalClientContacts(transmittalInfo);
            //#--
            return transmittalInfo;
        }

        /// <summary>
        /// Get a Transmittal with transmittal revisions and contacts from persistent storage.
        /// </summary>
        public TransmittalInfo GetDeepTransmittal(IDataReader dr)
        {
            TransmittalInfo transmittalInfo = CreateTransmittal(dr);
            transmittalInfo.TransmittalRevisions = GetTransmittalRevisions(transmittalInfo);
            transmittalInfo.Contacts = GetTransmittalContacts(transmittalInfo);
            //#--
            transmittalInfo.ClientContacts = GetTransmittalClientContacts(transmittalInfo);
            //#--

            return transmittalInfo;
        }


        //#--Returns the list of Clientcontacts for a transmittal
        public List<ClientContactInfo> GetTransmittalClientContacts(TransmittalInfo transmittalInfo)
        {
            IDataReader dr = null;
            List<ClientContactInfo> clientContactInfoList = new List<ClientContactInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientContactsByTransmittal(transmittalInfo.Id);
                while (dr.Read())
                    clientContactInfoList.Add((ClientContactInfo)PeopleController.GetInstance().GetPersonById(Data.Utils.GetDBInt32(dr["PeopleId"])));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Transmittal contacts from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientContactInfoList;
        }






        /// <summary>
        /// Get a Transmittal with transmittal revisions from persistent storage.
        /// </summary>
        public TransmittalInfo GetTransmittalWithRevisions(int? transmittalId)
        {
            TransmittalInfo transmittalInfo = GetTransmittal(transmittalId);
            transmittalInfo.TransmittalRevisions = GetTransmittalRevisions(transmittalInfo);
            return transmittalInfo;
        }

        /// <summary>
        /// Get a Transmittal with transmittal revisions from persistent storage.
        /// </summary>
        public TransmittalInfo GetTransmittalWithRevisions(IDataReader dr)
        {
            TransmittalInfo transmittalInfo = CreateTransmittal(dr);
            transmittalInfo.TransmittalRevisions = GetTransmittalRevisions(transmittalInfo);
            return transmittalInfo;
        }

        /// <summary>
        /// Get the Transmittals for the specified Project
        /// </summary>
        public List<TransmittalInfo> GetTransmittals(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<TransmittalInfo> transmittalInfoList = new List<TransmittalInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTransmittalsByProject(projectInfo.Id);
                while (dr.Read())
                    transmittalInfoList.Add(CreateTransmittal(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Transmittals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return transmittalInfoList;
        }

        /// <summary>
        /// Get the Transmittals with its drawing revisions for the specified Project
        /// </summary>
        public List<TransmittalInfo> GetTransmittalsWithRevisions(ProjectInfo projectInfo, SubContractorInfo subContractorInfo, String type)
        {
            IDataReader dr = null;
            TransmittalInfo transmittalInfo;
            List<Object> parameters = new List<Object>();
            List<TransmittalInfo> transmittalInfoList = new List<TransmittalInfo>();

            parameters.Add(type);
            parameters.Add(projectInfo.Id);
            parameters.Add(subContractorInfo.Id);

            try
            {
                dr = Data.DataProvider.GetInstance().GetTransmittalsByProjectAndSubContractor(parameters.ToArray());
                while (dr.Read())
                {
                    transmittalInfo = CreateTransmittal(dr);
                    transmittalInfo.TransmittalRevisions = GetTransmittalRevisions(transmittalInfo);

                    transmittalInfoList.Add(transmittalInfo);

                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Subcontractor Transmittals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return transmittalInfoList;
        }

        /// <summary>
        /// Get the Transmittals with its drawing revisions active for the specified Project
        /// </summary>
        public List<TransmittalInfo> GetTransmittalsWithRevisionsActive(ProjectInfo projectInfo, SubContractorInfo subContractorInfo)
        {
            return GetTransmittalsWithRevisions(projectInfo, subContractorInfo, Info.TypeActive);
        }

        /// <summary>
        /// Get the Transmittals with its drawing revisions proposal for the specified Project
        /// </summary>
        public List<TransmittalInfo> GetTransmittalsWithRevisionsProposal(ProjectInfo projectInfo, SubContractorInfo subContractorInfo)
        {
            return GetTransmittalsWithRevisions(projectInfo, subContractorInfo, Info.TypeProposal);
        }

        /// <summary>
        /// Get the Transmittals with transmittal revisions and contacts for the specified Project
        /// </summary>
        public List<TransmittalInfo> GetDeepTransmittals(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<TransmittalInfo> transmittalInfoList = new List<TransmittalInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetTransmittalsByProject(projectInfo.Id);
                while (dr.Read())
                    transmittalInfoList.Add(GetDeepTransmittal(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Transmittals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return transmittalInfoList;
        }

        /// <summary>
        /// Updates a Transmittal in the database
        /// </summary>
        public void UpdateTransmittal(TransmittalInfo transmittalInfo)
        {
            List<Object> parameters = new List<Object>();
            TransmittalInfo originalTransmittal = GetTransmittalWithRevisions(transmittalInfo.Id);
            Dictionary<Int32, TransmittalRevisionInfo> dictionaryRevisions = new Dictionary<Int32, TransmittalRevisionInfo>();
            Dictionary<Int32, TransmittalRevisionInfo> dictionaryOriginalRevisions = new Dictionary<Int32, TransmittalRevisionInfo>();

            foreach (TransmittalRevisionInfo transmittalRevision in transmittalInfo.TransmittalRevisions)
                dictionaryRevisions.Add((Int32)transmittalRevision.RevisionId, transmittalRevision);

            foreach (TransmittalRevisionInfo transmittalRevision in originalTransmittal.TransmittalRevisions)
                dictionaryOriginalRevisions.Add((Int32)transmittalRevision.RevisionId, transmittalRevision);

            SetModifiedInfo(transmittalInfo);

            parameters.Add(transmittalInfo.Id);
            parameters.Add(GetId(transmittalInfo.Contact));
            parameters.Add(GetId(transmittalInfo.SubContractor));
            parameters.Add(transmittalInfo.TransmissionDate);
            parameters.Add(transmittalInfo.DeliveryMethod);
            parameters.Add(transmittalInfo.TransmittalType);
            parameters.Add(transmittalInfo.RequiredAction);
            parameters.Add(transmittalInfo.TransmittalTypeOther);
            parameters.Add(transmittalInfo.RequiredActionOther);
            parameters.Add(transmittalInfo.Comments);
            parameters.Add(transmittalInfo.SendClientContact);
            parameters.Add(transmittalInfo.SendClientContact1);
            parameters.Add(transmittalInfo.SendClientContact2);
            parameters.Add(transmittalInfo.SendSuperintendent);
            parameters.Add(transmittalInfo.SendQuantitySurveyor);
            parameters.Add(transmittalInfo.SentDate);

            parameters.Add(transmittalInfo.ModifiedDate);
            parameters.Add(transmittalInfo.ModifiedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateTransmittal(parameters.ToArray());

                    foreach (TransmittalRevisionInfo transmittalRevision in transmittalInfo.TransmittalRevisions)
                        if (dictionaryOriginalRevisions.ContainsKey((Int32)transmittalRevision.RevisionId))
                            UpdateTransmittalRevision(transmittalRevision);
                        else
                            AddTransmittalRevision(transmittalRevision);

                    foreach (TransmittalRevisionInfo transmittalRevision in originalTransmittal.TransmittalRevisions)
                        if (!dictionaryRevisions.ContainsKey((Int32)transmittalRevision.RevisionId))
                            DeleteTransmittalRevision(transmittalRevision);

                    //#----To Add distributionList
                    if (transmittalInfo.Project != null)
                        foreach (ClientContactInfo clientContact in transmittalInfo.Project.ClientContactList)
                            if (clientContact.SendTransmittals == true)
                            {
                                parameters = new List<Object>();
                                parameters.Add(transmittalInfo.Id);
                                parameters.Add(clientContact.Id);
                                parameters.Add(transmittalInfo.Project.Id);

                                Data.DataProvider.GetInstance().AddTransmittalClientContact(parameters.ToArray());
                            }
                            else
                            {
                                parameters = new List<Object>();
                                parameters.Add(transmittalInfo.Id);
                                parameters.Add(clientContact.Id);
                                Data.DataProvider.GetInstance().DeleteTransmittalClientContact(parameters.ToArray());

                            }
                    //#---



                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Transmittal in database");
            }
        }

        /// <summary>
        /// Updates a Transmittal sent date in the database
        /// </summary>
        public void UpdateTransmittalSentDate(TransmittalInfo transmittalInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(transmittalInfo.Id);
            parameters.Add(transmittalInfo.SentDate);

            try
            {
                Data.DataProvider.GetInstance().UpdateTransmittalSentDate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Transmittal Date Sent in database");
            }
        }

        /// <summary>
        /// Adds a Transmittal to the database
        /// </summary>
        public int? AddTransmittal(TransmittalInfo transmittalInfo)
        {
            int? transmittalId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(transmittalInfo);

            parameters.Add(transmittalInfo.Type);
            parameters.Add(GetId(transmittalInfo.Project));
            parameters.Add(GetId(transmittalInfo.Contact));
            parameters.Add(GetId(transmittalInfo.SubContractor));
            parameters.Add(transmittalInfo.TransmissionDate);
            parameters.Add(transmittalInfo.DeliveryMethod);
            parameters.Add(transmittalInfo.TransmittalType);
            parameters.Add(transmittalInfo.RequiredAction);
            parameters.Add(transmittalInfo.TransmittalTypeOther);
            parameters.Add(transmittalInfo.RequiredActionOther);
            parameters.Add(transmittalInfo.Comments);
            parameters.Add(transmittalInfo.SendClientContact);
            parameters.Add(transmittalInfo.SendClientContact1);
            parameters.Add(transmittalInfo.SendClientContact2);
            parameters.Add(transmittalInfo.SendSuperintendent);
            parameters.Add(transmittalInfo.SendQuantitySurveyor);
            parameters.Add(transmittalInfo.SentDate);

            parameters.Add(transmittalInfo.CreatedDate);
            parameters.Add(transmittalInfo.CreatedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    transmittalId = transmittalInfo.Id = Data.DataProvider.GetInstance().AddTransmittal(parameters.ToArray());
                    foreach (TransmittalRevisionInfo transmittalRevision in transmittalInfo.TransmittalRevisions)
                        if (transmittalRevision.Revision != null)
                        {
                            transmittalRevision.Transmittal = transmittalInfo;
                            AddTransmittalRevision(transmittalRevision);
                        }
                    //#----To Add distributionList


                    if (transmittalInfo.Project != null)
                        foreach (ClientContactInfo clientContact in transmittalInfo.Project.ClientContactList)
                            if (clientContact.SendTransmittals == true)
                            {
                                parameters = new List<Object>();
                                parameters.Add(transmittalId);
                                parameters.Add(clientContact.Id);
                                parameters.Add(transmittalInfo.Project.Id);

                                Data.DataProvider.GetInstance().AddTransmittalClientContact(parameters.ToArray());
                            }

                    //#---

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Transmittal to database");
            }

            return transmittalId;
        }

        /// <summary>
        /// Adds or updates a Transmittal
        /// </summary>
        public int? AddUpdateTransmittal(TransmittalInfo transmittalInfo)
        {
            if (transmittalInfo != null)
            {
                if (transmittalInfo.Id != null)
                {
                    UpdateTransmittal(transmittalInfo);
                    return transmittalInfo.Id;
                }
                else
                {
                    return AddTransmittal(transmittalInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Transmittal from persistent storage
        /// </summary>
        public void DeleteTransmittal(TransmittalInfo transmittalInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteTransmittal(transmittalInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Transmittal from database");
            }
        }

        /// <summary>
        /// Add Revision to Transmittal
        /// </summary>
        public void AddTransmittalRevision(TransmittalRevisionInfo transmittalRevisionInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(transmittalRevisionInfo.Transmittal.Id);
            parameters.Add(transmittalRevisionInfo.Revision.Id);
            parameters.Add(transmittalRevisionInfo.NumCopies);

            try
            {
                Data.DataProvider.GetInstance().AddTransmittalRevision(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Transmittal Revision in database");
            }
        }

        /// <summary>
        /// Updates a TransmittalRevision
        /// </summary>
        public void UpdateTransmittalRevision(TransmittalRevisionInfo transmittalRevisionInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(transmittalRevisionInfo.Transmittal.Id);
            parameters.Add(transmittalRevisionInfo.Revision.Id);
            parameters.Add(transmittalRevisionInfo.NumCopies);

            try
            {
                Data.DataProvider.GetInstance().UpdateTransmittalRevision(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Transmittal Revision in database");
            }
        }

        /// <summary>
        /// Delete Revision from Transmittal
        /// </summary>
        public void DeleteTransmittalRevision(TransmittalRevisionInfo transmittalRevisionInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(transmittalRevisionInfo.Transmittal.Id);
            parameters.Add(transmittalRevisionInfo.Revision.Id);

            try
            {
                Data.DataProvider.GetInstance().DeleteTransmittalRevision(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleting Revision from Transmittal in database");
            }
        }

        /// <summary>
        /// Add Contact to Transmittal
        /// </summary>
        public void AddTransmittalContact(TransmittalInfo transmittalInfo, ContactInfo contactInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(transmittalInfo.Id);
            parameters.Add(contactInfo.Id);
            parameters.Add(GetId(contactInfo.SubContractor));

            try
            {
                Data.DataProvider.GetInstance().AddTransmittalContact(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Transmittal Contact in database");
            }
        }

        /// <summary>
        /// Delete Contact from Transmittal
        /// </summary>
        public void DeleteTransmittalContact(TransmittalInfo transmittalInfo, ContactInfo contactInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(transmittalInfo.Id);
            parameters.Add(contactInfo.Id);

            try
            {
                Data.DataProvider.GetInstance().DeleteTransmittalContact(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleting Contact from Transmittal in database");
            }
        }

        /// <summary>
        /// Generate the transmittal Report
        /// </summary>
        public Byte[] GenerateTransmittalReport(TransmittalInfo transmittalInfo)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            List<ContactInfo> contactInfoList = new List<ContactInfo>();
            PeopleController peopleController = PeopleController.GetInstance();
            ContactInfo contact = transmittalInfo.Contact;
            LocalReport localReport = new LocalReport();
            EmployeeInfo employeeInfo = null;
            String contactName = null;
            String subcontractorName = null;
            String Signature = null;
            String Street1 = null;
            String Street2 = null;

            employeeInfo = (EmployeeInfo)peopleController.GetPersonById(transmittalInfo.CreatedBy);
            Signature = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/" + UI.Utils.SetFormString(employeeInfo.Signature);

            //#------
            #region Old
            //foreach (ContactInfo contactInfo in transmittalInfo.Contacts)
            //    contactInfoList.Add(contactInfo);

            //if ((bool)transmittalInfo.SendClientContact)
            //    contactInfoList.Add(peopleController.ConvertClientContactToContact(transmittalInfo.Project.ClientContact));

            //if ((bool)transmittalInfo.SendClientContact1)
            //    contactInfoList.Add(peopleController.ConvertClientContactToContact(transmittalInfo.Project.ClientContact1));

            //if ((bool)transmittalInfo.SendClientContact2)
            //    contactInfoList.Add(peopleController.ConvertClientContactToContact(transmittalInfo.Project.ClientContact2));

            //if ((bool)transmittalInfo.SendSuperintendent)
            //    contactInfoList.Add(peopleController.ConvertClientContactToContact(transmittalInfo.Project.Superintendent));

            //if ((bool)transmittalInfo.SendQuantitySurveyor)
            //    contactInfoList.Add(peopleController.ConvertClientContactToContact(transmittalInfo.Project.QuantitySurveyor));
            #endregion


            if (transmittalInfo.ClientContacts != null)
                foreach (ClientContactInfo clientContactInfo in transmittalInfo.ClientContacts)
                {
                    //if (clientContactInfo.SendTransmittals != null)
                    //    if ((Boolean)clientContactInfo.SendTransmittals)

                    //   clientContactInfo.Email = "";
                    contactInfoList.Add(peopleController.ConvertClientContactToContact(clientContactInfo));
                }

            //#----



            if (contact == null && contactInfoList.Count > 0)
            {
                contact = contactInfoList[0];
                contactInfoList.Remove(contactInfoList[0]);
            }

            if (contact != null)
            {
                subcontractorName = contact.SubContractorName;
                contactName = contact.Name;
                Street1 = contact.Street;
                Street2 = contact.Locality + "," + contact.State + " " + contact.PostalCode;
            }

            reportParameters.Add(new ReportParameter("Type", UI.Utils.SetFormString(Web.Utils.GetConfigListItemNameAndOther("Transmittals", "TransmittalType", transmittalInfo.TransmittalType, transmittalInfo.TransmittalTypeOther))));
            reportParameters.Add(new ReportParameter("SentBy", UI.Utils.SetFormString(Web.Utils.GetConfigListItemName("Transmittals", "DeliveryMethod", transmittalInfo.DeliveryMethod))));
            reportParameters.Add(new ReportParameter("Action", UI.Utils.SetFormString(Web.Utils.GetConfigListItemNameAndOther("Transmittals", "RequiredAction", transmittalInfo.RequiredAction, transmittalInfo.RequiredActionOther))));
            reportParameters.Add(new ReportParameter("ToName", UI.Utils.SetFormString(subcontractorName)));
            reportParameters.Add(new ReportParameter("ToContact", UI.Utils.SetFormString(contactName)));
            reportParameters.Add(new ReportParameter("ToAddress", UI.Utils.SetFormString(Street1)));
            reportParameters.Add(new ReportParameter("ToCity", UI.Utils.SetFormString(Street2)));
            reportParameters.Add(new ReportParameter("Date", UI.Utils.SetFormDate(transmittalInfo.TransmissionDate)));
            reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(transmittalInfo.Project.Name)));
            reportParameters.Add(new ReportParameter("JobNo", UI.Utils.SetFormString(transmittalInfo.Project.FullNumber)));
            reportParameters.Add(new ReportParameter("Number", UI.Utils.SetFormInteger(transmittalInfo.TransmittalNumber)));
            reportParameters.Add(new ReportParameter("Comments", UI.Utils.SetFormString(transmittalInfo.Comments)));
            reportParameters.Add(new ReportParameter("SignatureName", UI.Utils.SetFormString(employeeInfo.Name)));
            reportParameters.Add(new ReportParameter("Signature", Signature));
            reportParameters.Add(new ReportParameter("ViewCopyTo", UI.Utils.SetFormYesNo(contactInfoList.Count > 0).ToString()));

            localReport.ReportPath = Web.Utils.ReportsPath + "\\Transmittal.rdlc";
            localReport.DataSources.Add(new ReportDataSource("TransmittalRevisionInfo", transmittalInfo.TransmittalRevisions));
            localReport.DataSources.Add(new ReportDataSource("ContactInfo", contactInfoList));
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating Transmittal Report");
            }
        }

        /// <summary>
        /// Generate the ChcekList Report
        /// </summary>
        public Byte[] GenerateCheckListReport(TradeInfo tradeInfo, String checkListType)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            List<TradeItemInfo> tradeItemInfoList = new List<TradeItemInfo>();
            LocalReport localReport = new LocalReport();

            if (tradeInfo.ItemCategories != null)
                foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeInfo.ItemCategories)
                    if (tradeItemCategoryInfo.TradeItems != null)
                        foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                            if (checkListType == Info.TypeActive || tradeItemInfo.IsRequiredInProposal)
                            {
                                tradeItemInfo.TradeItemCategory = tradeItemCategoryInfo;
                                tradeItemInfoList.Add(tradeItemInfo);
                            }

            reportParameters.Add(new ReportParameter("ProjectName", tradeInfo.Project.Name));
            reportParameters.Add(new ReportParameter("TradeName", tradeInfo.Name));

            localReport.ReportPath = Web.Utils.ReportsPath + "\\CheckList.rdlc";
            localReport.DataSources.Add(new ReportDataSource("TradeItemInfo", tradeItemInfoList));
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating Check List Report");
            }
        }

        /// <summary>
        /// Generate the ChcekList Report
        /// </summary>
        public Byte[] GenerateCheckListReport(TradeParticipationInfo tradeParticipationInfo)
        {
            return GenerateCheckListReport(tradeParticipationInfo.Trade, tradeParticipationInfo.Type);
        }

        /// <summary>
        /// Sends a transmittal to all recepients
        /// </summary>
        public void SendTransmittal(TransmittalInfo transmittalInfo)
        {
            PeopleController peopleController = PeopleController.GetInstance();
            EmployeeInfo employeeInfo = (EmployeeInfo)peopleController.GetPersonById(transmittalInfo.CreatedBy);

            String subject = transmittalInfo.Project.Name + " - Transmittal: " + UI.Utils.SetFormInteger(transmittalInfo.TransmittalNumber);

            String message = "" +
               "Please find attached transmittal: " + transmittalInfo.Name + "." +
               "<br />" +
               "<br />" +
               "<i>" + employeeInfo.Name + "</i><br />" +
               "<i>" + employeeInfo.Position + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            //#-----
            if (transmittalInfo.DeliveryMethod == "DLS")// Download from SOS
            {
                string actTransmittal = UI.Utils.SetFormString(Web.Utils.GetConfigListItemNameAndOther("Transmittals", "RequiredAction", transmittalInfo.RequiredAction, transmittalInfo.RequiredActionOther));

                message = "" +
                "Attached transmittal No. " + transmittalInfo.TransmittalNumberStr + " dated " + UI.Utils.SetFormDate(transmittalInfo.TransmissionDate) + " for " + actTransmittal +
               "<br /> Please log into our SOS system to download the documentation referred to in the above mentioned transmittal." +
               "<br />" +
               "<br /> SOS website link: https://sos.vaughans.com.au " +
               "<br />" +
               "<br />" +
               "<i>" + employeeInfo.Name + "</i><br />" +
               "<i>" + employeeInfo.Position + "</i><br />" +
               "<i>" + employeeInfo.Phone + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            }
            //#----





            Byte[] attachment = GenerateTransmittalReport(transmittalInfo);
            String attachmentName = ("Project_" + transmittalInfo.Project.Number + "_Transmittal_" + UI.Utils.SetFormInteger(transmittalInfo.TransmittalNumber)) + ".pdf";


            List<FileInfo> fileInfoList = new List<FileInfo>();
            FileInfo fileInfo;

            foreach (TransmittalRevisionInfo transmittalRevisionInfo in transmittalInfo.TransmittalRevisions)
            {
                if (transmittalRevisionInfo.Revision != null && transmittalRevisionInfo.Revision.File != null)
                {
                    fileInfo = new FileInfo(UI.Utils.FullPath(transmittalRevisionInfo.Revision.Drawing.Project.AttachmentsFolder, transmittalRevisionInfo.Revision.File));
                    if (fileInfo.Exists)
                        fileInfoList.Add(fileInfo);
                }
            }

            List<PeopleInfo> peopleInfoList = new List<PeopleInfo>();

            //#---
            #region Old
            /*
            //if ((Boolean)transmittalInfo.SendClientContact)
            //    peopleInfoList.Add(transmittalInfo.Project.ClientContact);

            //if ((Boolean)transmittalInfo.SendClientContact1)
            //    peopleInfoList.Add(transmittalInfo.Project.ClientContact1);

            //if ((Boolean)transmittalInfo.SendClientContact2)
            //    peopleInfoList.Add(transmittalInfo.Project.ClientContact2);

            //if ((Boolean)transmittalInfo.SendSuperintendent)
            //    peopleInfoList.Add(transmittalInfo.Project.Superintendent);

            //if ((Boolean)transmittalInfo.SendQuantitySurveyor)
            //    peopleInfoList.Add(transmittalInfo.Project.QuantitySurveyor);
            */
            #endregion
            foreach (ClientContactInfo clientContactInfo in transmittalInfo.ClientContacts)
                if (clientContactInfo.Email != null || clientContactInfo.Email != String.Empty)
                {
                    peopleInfoList.Add(clientContactInfo);
                }

            //#----

            foreach (ContactInfo contactInfo in transmittalInfo.Contacts)
                peopleInfoList.Add(contactInfo);

            if (transmittalInfo.Contact == null)
            {
                if (peopleInfoList.Count > 0)
                {
                    transmittalInfo.Contact = new ContactInfo();

                    transmittalInfo.Contact.Email = peopleInfoList[0].Email;
                    transmittalInfo.Contact.FirstName = peopleInfoList[0].FirstName;
                    transmittalInfo.Contact.LastName = peopleInfoList[0].LastName;

                    peopleInfoList.Remove(peopleInfoList[0]);
                }
                else
                    throw new Exception("No recipients have been selected");
            }


            //#-------- Alex want to send a copy of transmittal  to only the person who sent this transmittal

            if (employeeInfo.Email != null)
                peopleInfoList.Add(employeeInfo);

            /*
             //---Send copy of email to CA and PM  and It will Send only transmittals not any drawings or other attachments
             
              
            if (transmittalInfo.Project.ContractsAdministrator != null)
                if(transmittalInfo.Project.ContractsAdministrator.Email!=null)
                peopleInfoList.Add(transmittalInfo.Project.ContractsAdministrator);

            if(transmittalInfo.Project.ProjectManager != null)
                if(transmittalInfo.Project.ProjectManager.Email != null)
                peopleInfoList.Add(transmittalInfo.Project.ProjectManager);

            */


            if (transmittalInfo.DeliveryMethod == "DLS")// Download from SOS
            {
                fileInfoList.Clear();//It will Send only transmittals not any drawings
            }


            //#-------

            Utils.SendEmail(transmittalInfo.Contact, peopleInfoList, subject, message, attachment, attachmentName, fileInfoList);
        }

        /// <summary>
        /// Sends an invitation to tender
        /// </summary>
        public void SendInvitacion(TradeParticipationInfo tradeParticipationInfo, int? tmpRank, Boolean? tmpPulledOut)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            TransmittalInfo transmittalInfo = new TransmittalInfo();
            List<PeopleInfo> peopleInfoList = new List<PeopleInfo>();
            TradeParticipationInfo quoteParticipation = new TradeParticipationInfo();
            EmployeeInfo employeeInfo = tradeParticipationInfo.Trade.ContractsAdministrator != null ? tradeParticipationInfo.Trade.ContractsAdministrator : tradeParticipationInfo.Trade.Project.ContractsAdministrator;
            String template = tradesController.GetInvitationTemplate(1).Template;

            if (employeeInfo == null)
                throw new Exception("CA not specified.");

            peopleInfoList.Add(employeeInfo);

            String subject = String.Format("Tender Invitation - {0} - {1}", tradeParticipationInfo.Trade.Project.Name, tradeParticipationInfo.Trade.Name);

            String message = "" +
               "Dear " + tradeParticipationInfo.Contact.FirstName + ",<br />" +
               "<br />" +
               "Please find attached Invitation to Tender and Checklist for the project mentioned in the subject of this email as discussed." + "<br />" +
               "<br />" +
               "All the documentation relevant to this project is available in our website: <a href='https://sos.vaughans.com.au'>https://sos.vaughans.com.au</a>. " +
               "Please ensure the Checklist is completed online and submitted to us by the tender due date." + "<br />" +
               "<br />" +
               "Feel free to contact me with any questions regarding the tender documentation or our tender online system." + "<br />" +
               "<br />" +
               "Kind Regards,<br />" +
               "<i>" + employeeInfo.Name + "</i><br />" +
               "<i>" + employeeInfo.Position + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />" +
               "<i>" + employeeInfo.Phone + "</i><br />" +
               "<i><a:href='mailto:" + employeeInfo.Email + "'>" + employeeInfo.Email + "</a></i><br />";

            Byte[] invitation = UI.Utils.HtmlToPDF(contractsController.MergeTemplatePrint(tradeParticipationInfo.Trade, template), contractsController.GetTemplateFooterText(template));
            String invitationName = String.Format("TenderInvitation_{0}_{1}.pdf", tradeParticipationInfo.Trade.Project.Name, tradeParticipationInfo.Trade.Name);

            Byte[] checkList = GenerateCheckListReport(tradeParticipationInfo.Trade, tradeParticipationInfo.Type);
            String checkListName = String.Format("Checklist_{0}_{1}.pdf", tradeParticipationInfo.Trade.Project.Name, tradeParticipationInfo.Trade.Name);

            transmittalInfo.Type = tradeParticipationInfo.Type;
            transmittalInfo.TransmittalRevisions = new List<TransmittalRevisionInfo>();

            // Drawings on the trade must be only the ones based on the participation type because of the use of the template variable.
            foreach (DrawingInfo drawingInfo in tradeParticipationInfo.Trade.IncludedDrawings)
                transmittalInfo.TransmittalRevisions.Add(new TransmittalRevisionInfo(1, drawingInfo.LastRevision));

            tradeParticipationInfo.InvitationDate = DateTime.Now;
            tradeParticipationInfo.Transmittal = transmittalInfo;
            tradeParticipationInfo.Rank = tmpRank;
            tradeParticipationInfo.PulledOut = tmpPulledOut;
            quoteParticipation.Type = tradeParticipationInfo.Type;
            quoteParticipation.Trade = tradeParticipationInfo.Trade;
            quoteParticipation.ComparisonParticipation = tradeParticipationInfo;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    transmittalInfo.Id = AddTransmittal(transmittalInfo);
                    quoteParticipation.Id = tradesController.AddTradeParticipation(quoteParticipation);
                    tradesController.UpdateTradeParticipation(tradeParticipationInfo);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating invitation to tender");
            }

            Utils.SendEmail(tradeParticipationInfo.Contact, peopleInfoList, subject, message, invitation, invitationName, checkList, checkListName);

            tradeParticipationInfo.QuoteParticipation = quoteParticipation;
        }

        /// <summary>
        /// Sends a quote reminder
        /// </summary>
        public void SendInvitacionReminder(TradeParticipationInfo tradeParticipationInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            EmployeeInfo employeeInfo = tradeParticipationInfo.Trade.ContractsAdministrator != null ? tradeParticipationInfo.Trade.ContractsAdministrator : tradeParticipationInfo.Trade.Project.ContractsAdministrator;

            if (employeeInfo == null)
                throw new Exception("CA not specified.");

            String subject = String.Format("Quote submission reminder - {0} - {1}", tradeParticipationInfo.Trade.Project.Name, tradeParticipationInfo.Trade.Name);

            String message = "" +
               "Dear " + tradeParticipationInfo.Contact.FirstName + ",<br />" +
               "<br />" +
               "This is a reminder to submit your quote for the project in the subject. The due date is: <b>" + UI.Utils.SetFormDateTime(tradeParticipationInfo.QuoteDueDate) + "</b>." + "<br />" +
               "<br />" +
               "Feel free to contact me if you have any questions about the invitation to tender." + "<br />" +
               "<br />" +
               "Kind Regards,<br />" +
               "<i>" + employeeInfo.Name + "</i><br />" +
               "<i>" + employeeInfo.Position + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />" +
               "<i>" + employeeInfo.Phone + "</i><br />" +
               "<i><a:href='mailto:" + employeeInfo.Email + "'>" + employeeInfo.Email + "</a></i><br />";

            tradeParticipationInfo.ReminderDate = DateTime.Now;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    tradesController.UpdateTradeParticipationReminderDate(tradeParticipationInfo);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating trade participation reminder date");
            }

            Utils.SendEmail(employeeInfo, tradeParticipationInfo.Contact, new List<PeopleInfo>() { employeeInfo }, subject, message);
        }

        /// <summary>
        /// Checks transmittal for errors and missing fields
        /// </summary>
        public XmlDocument CheckTransmittal(TransmittalInfo transmittalInfo)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElement;
            XmlElement xmlElement1;
            XmlElement xmlElementErrors;
            FileInfo fileInfo;
            long totalFilesSize = 0;
            long maxFilesSize = long.Parse(ConfigurationManager.AppSettings["EmailMaxSize"].ToString());
            List<ClientContactInfo> clientContactInfoList = new List<ClientContactInfo>();

            xmlElementRoot = xmlDocument.CreateElement("root", null);
            xmlElementRoot.SetAttribute("name", "Transmittal Check");

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElement = xmlDocument.CreateElement("Fields", null);
            xmlElement.SetAttribute("name", "Missing fields");

            xmlElementErrors = xmlDocument.CreateElement("Errors", null);
            xmlElementErrors.SetAttribute("name", "Errors");

            Utils.AddMissingFieldNode(transmittalInfo.TransmittalType, xmlDocument, xmlElement, "Transmittal type");
            Utils.AddMissingFieldNode(transmittalInfo.DeliveryMethod, xmlDocument, xmlElement, "Delivery method");
            Utils.AddMissingFieldNode(transmittalInfo.RequiredAction, xmlDocument, xmlElement, "Required action");
            Utils.AddMissingFieldNode(transmittalInfo.TransmissionDate, xmlDocument, xmlElement, "Date");

            //SAN------------------------
            if (transmittalInfo.DeliveryMethod == "DLS" && (transmittalInfo.SendClientContact == true || transmittalInfo.SendClientContact1 == true || transmittalInfo.SendClientContact2 == true || transmittalInfo.SendQuantitySurveyor == true || transmittalInfo.SendSuperintendent == true))
            {
                Utils.AddMissingFieldNode(null, xmlDocument, xmlElement, "Delivery method 'Download from SOS' works only for Subcontractors,please clear the distribution list.");

            }
            //SAN---------------


            if (transmittalInfo.Contact != null)
            {
                Utils.AddMissingFieldNode(transmittalInfo.Contact.Email, xmlDocument, xmlElement, "Contact email");
                Utils.AddMissingFieldNode(transmittalInfo.SubContractor, xmlDocument, xmlElement, "Contact company");
            }

            //#---
            #region old
            /*
            if ((bool)transmittalInfo.SendClientContact)
            {
                Utils.AddMissingFieldNode(transmittalInfo.Project.ClientContact, xmlDocument, xmlElement, "Project client main contact");
                if (transmittalInfo.Project.ClientContact != null)
                {
                    clientContactInfoList.Add(transmittalInfo.Project.ClientContact);
                    Utils.AddMissingFieldNode(transmittalInfo.Project.ClientContact.Email, xmlDocument, xmlElement, "Project client main contact email");
                }
            }

            if ((bool)transmittalInfo.SendClientContact1)
            {
                Utils.AddMissingFieldNode(transmittalInfo.Project.ClientContact1, xmlDocument, xmlElement, "Project client Contact 1");
                if (transmittalInfo.Project.ClientContact1 != null)
                {
                    clientContactInfoList.Add(transmittalInfo.Project.ClientContact1);
                    Utils.AddMissingFieldNode(transmittalInfo.Project.ClientContact1.Email, xmlDocument, xmlElement, "Project client contact 1 email");
                }
            }

            if ((bool)transmittalInfo.SendClientContact2)
            {
                Utils.AddMissingFieldNode(transmittalInfo.Project.ClientContact2, xmlDocument, xmlElement, "Project client Contact 2");
                if (transmittalInfo.Project.ClientContact2 != null)
                {
                    clientContactInfoList.Add(transmittalInfo.Project.ClientContact2);
                    Utils.AddMissingFieldNode(transmittalInfo.Project.ClientContact2.Email, xmlDocument, xmlElement, "Project client contact 2 email");
                }
            }

            if ((bool)transmittalInfo.SendSuperintendent)
            {
                Utils.AddMissingFieldNode(transmittalInfo.Project.Superintendent, xmlDocument, xmlElement, "Project superintendent");
                if (transmittalInfo.Project.Superintendent != null)
                {
                    clientContactInfoList.Add(transmittalInfo.Project.Superintendent);
                    Utils.AddMissingFieldNode(transmittalInfo.Project.Superintendent.Email, xmlDocument, xmlElement, "Project superintendent email");
                }
            }

            if ((bool)transmittalInfo.SendQuantitySurveyor)
            {
                Utils.AddMissingFieldNode(transmittalInfo.Project.QuantitySurveyor, xmlDocument, xmlElement, "Project quantity surveyor");
                if (transmittalInfo.Project.QuantitySurveyor != null)
                {
                    clientContactInfoList.Add(transmittalInfo.Project.QuantitySurveyor);
                    Utils.AddMissingFieldNode(transmittalInfo.Project.QuantitySurveyor.Email, xmlDocument, xmlElement, "Project quantity surveyor email");
                }
            }

            */
            #endregion

            foreach (ClientContactInfo clientContactInfo in transmittalInfo.ClientContacts)
            {
                /////Utils.AddMissingFieldNode(clientContactInfo, xmlDocument, xmlElement, "Contact: " + clientContactInfo.Name + " company");
                Utils.AddMissingFieldNode(clientContactInfo.Email, xmlDocument, xmlElement, "Contact: " + clientContactInfo.Name + " email");
            }

            //#---

            foreach (ContactInfo contactInfo in transmittalInfo.Contacts)
            {
                Utils.AddMissingFieldNode(contactInfo.SubContractor, xmlDocument, xmlElement, "Contact: " + contactInfo.Name + " company");
                Utils.AddMissingFieldNode(contactInfo.Email, xmlDocument, xmlElement, "Contact: " + contactInfo.Name + " email");
            }

            if (transmittalInfo.TransmittalRevisions == null || transmittalInfo.TransmittalRevisions.Count == 0)
                Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Drawings");

            foreach (TransmittalRevisionInfo transmittalRevisionInfo in transmittalInfo.TransmittalRevisions)
            {
                xmlElement1 = xmlDocument.CreateElement("Revision" + transmittalRevisionInfo.IdStr, null);
                xmlElement1.SetAttribute("name", transmittalRevisionInfo.DrawingName + " - " + transmittalRevisionInfo.RevisionName);

                Utils.AddMissingFieldNode(transmittalRevisionInfo.NumCopies, xmlDocument, xmlElement1, "Number of copies");
                Utils.AddMissingFieldNode(transmittalRevisionInfo.Revision, xmlDocument, xmlElement1, "Revision");
                Utils.AddMissingFieldNode(transmittalRevisionInfo.RevisionFile, xmlDocument, xmlElement1, "Revision file");

                if (transmittalRevisionInfo.RevisionFile != null)
                {
                    fileInfo = new FileInfo(UI.Utils.FullPath(transmittalInfo.Project.AttachmentsFolder, transmittalRevisionInfo.RevisionFile));

                    if (fileInfo.Exists)
                        totalFilesSize = totalFilesSize + fileInfo.Length;
                    else
                        Utils.AddErrorMessageNode(xmlDocument, xmlElement1, "File: " + transmittalRevisionInfo.RevisionFile + " does not exist");
                }

                if (xmlElement1.HasChildNodes)
                    xmlElement.AppendChild(xmlElement1);
            }

            if (transmittalInfo.DeliveryMethod == TransmittalInfo.DeliveryMethodEmail && totalFilesSize > maxFilesSize)
                Utils.AddErrorMessageNode(xmlDocument, xmlElementErrors, "Drawing files size (" + UI.Utils.SetFormFileSize(totalFilesSize) + ") greater than maximum allowed (" + UI.Utils.SetFormFileSize(maxFilesSize) + ")");

            //#--if (transmittalInfo.Contact == null && clientContactInfoList.Count == 0 && transmittalInfo.Contacts.Count == 0)
            if (transmittalInfo.Contact == null && transmittalInfo.ClientContacts.Count == 0 && transmittalInfo.Contacts.Count == 0)  //#--
                Utils.AddErrorMessageNode(xmlDocument, xmlElementErrors, "No recipients selected.");

            if (xmlElement.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElement);

            if (xmlElementErrors.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElementErrors);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        /// <summary>
        /// Makes a copy of an existing transmittal
        /// </summary>
        public void CopyTransmittal(TransmittalInfo transmittalInfo)
        {
            try
            {
                SetCreateInfo(transmittalInfo);

                transmittalInfo.SentDate = null;
                transmittalInfo.TransmissionDate = null;
                transmittalInfo.SendClientContact = false;
                transmittalInfo.SendClientContact1 = false;
                transmittalInfo.SendClientContact2 = false;
                transmittalInfo.SendSuperintendent = false;
                transmittalInfo.SendQuantitySurveyor = false;
                transmittalInfo.Contact = null;

                transmittalInfo.Id = AddTransmittal(transmittalInfo);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Copying Transmittal.");
            }
        }

        /// <summary>
        /// Verifies if a contact has the right to see a transmittal. Current user must be a Contact and must have a subcontractor
        /// </summary>
        public Boolean AllowViewCurrentUser(TransmittalInfo transmittalInfo)
        {
            ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();
            return (transmittalInfo.Contact != null && transmittalInfo.Contact.SubContractor != null && transmittalInfo.Contact.SubContractor.Equals(contactInfo.SubContractor)) || (transmittalInfo.Contacts != null && transmittalInfo.Contacts.Find(delegate (ContactInfo contactInfoInList) { return contactInfo.SubContractor.Equals(contactInfoInList.SubContractor); }) != null);
        }

        /// <summary>
        /// Throws and exception if the current user can not view information
        /// </summary>
        public void CheckViewCurrentUser(TransmittalInfo transmittalInfo)
        {
            if (!AllowViewCurrentUser(transmittalInfo))
                throw new SecurityException();
        }
        #endregion


        #region ClientTrades Methods
        /// <summary>
        /// Creates a ClientTrade from a dr
        /// </summary>
        public ClientTradeInfo CreateClientTrade(IDataReader dr)
        {
            ClientTradeInfo clientTradeInfo = new ClientTradeInfo(Data.Utils.GetDBInt32(dr["ClientTradeId"]));

            clientTradeInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            clientTradeInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            clientTradeInfo.Claimed = Data.Utils.GetDBDecimal(dr["Claimed"]);
            clientTradeInfo.DisplayOrder = Data.Utils.GetDBInt32(dr["DisplayOrder"]);

            AssignAuditInfo(clientTradeInfo, dr);

            return clientTradeInfo;
        }

        /// <summary>
        /// Get a ClientTrade from persistent storage
        /// </summary>
        public ClientTradeInfo GetClientTrade(int? clientTradeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientTrade(clientTradeId);
                if (dr.Read())
                    return CreateClientTrade(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Trade from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the ClientTrades for the specified Project
        /// </summary>
        public List<ClientTradeInfo> GetClientTrades(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<ClientTradeInfo> clientTradeInfoList = new List<ClientTradeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientTradesByProject(projectInfo.Id);
                while (dr.Read())
                    clientTradeInfoList.Add(CreateClientTrade(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Client Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientTradeInfoList;
        }

        /// <summary>
        /// Updates a ClientTrade in the database
        /// </summary>
        public void UpdateClientTrade(ClientTradeInfo clientTradeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(clientTradeInfo);

            parameters.Add(clientTradeInfo.Id);
            parameters.Add(clientTradeInfo.Name);
            parameters.Add(clientTradeInfo.Amount);
            parameters.Add(clientTradeInfo.DisplayOrder);

            parameters.Add(clientTradeInfo.ModifiedDate);
            parameters.Add(clientTradeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateClientTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Trade in database");
            }
        }

        /// <summary>
        /// Adds a ClientTrade to the database
        /// </summary>
        public int? AddClientTrade(ClientTradeInfo clientTradeInfo)
        {
            int? clientTradeId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(clientTradeInfo);

            parameters.Add(GetId(clientTradeInfo.Project));
            parameters.Add(clientTradeInfo.Name);
            parameters.Add(clientTradeInfo.Amount);

            parameters.Add(clientTradeInfo.CreatedDate);
            parameters.Add(clientTradeInfo.CreatedBy);

            try
            {
                clientTradeId = Data.DataProvider.GetInstance().AddClientTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Client Trade to database");
            }

            return clientTradeId;
        }

        /// <summary>
        /// Adds or updates a ClientTrade
        /// </summary>
        public int? AddUpdateClientTrade(ClientTradeInfo clientTradeInfo)
        {
            if (clientTradeInfo != null)
            {
                if (clientTradeInfo.Id != null)
                {
                    UpdateClientTrade(clientTradeInfo);
                    return clientTradeInfo.Id;
                }
                else
                {
                    return AddClientTrade(clientTradeInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a ClientTrade from persistent storage
        /// </summary>
        public void DeleteClientTrade(ClientTradeInfo clientTradeInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteClientTrade(clientTradeInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Client Trade from database");
            }
        }
        #endregion


        #region Budget Methods
        /// <summary>
        /// Creates a new instance of a budget provicer object
        /// </summary>
        public IBudget CreateBudgetObject(String budgetType)
        {
            IBudget budgetSource;

            if (budgetType == BudgetType.BOQ.ToString())
                budgetSource = new BudgetInfo();
            else
            {
                budgetSource = new ClientVariationTradeInfo();
                ((ClientVariationTradeInfo)budgetSource).ClientVariation = budgetType == BudgetType.CV.ToString() ? new ClientVariationInfo() : new SeparateAccountInfo();
            }

            return budgetSource;
        }

        /// <summary>
        /// Creates a Budget from a dr
        /// </summary>
        public BudgetInfo CreateBudget(IDataReader dr)
        {
            BudgetInfo budgetInfo = new BudgetInfo(Data.Utils.GetDBInt32(dr["BudgetId"]));

            budgetInfo.Code = Data.Utils.GetDBString(dr["Code"]);
            budgetInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            budgetInfo.Name = Data.Utils.GetDBString(dr["Name"]);

            AssignAuditInfo(budgetInfo, dr);

            return budgetInfo;
        }

        /// <summary>
        /// Get a Budget from persistent storage
        /// </summary>
        public BudgetInfo GetBudget(int? budgetId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetBudget(budgetId);
                if (dr.Read())
                    return CreateBudget(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Budget from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Budgets for the specified Project
        /// </summary>
        public List<BudgetInfo> GetBudgets(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<BudgetInfo> budgetInfoList = new List<BudgetInfo>();
            BudgetInfo budgetInfo = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetBudgetsByProject(projectInfo.Id);
                while (dr.Read())
                {
                    budgetInfo = CreateBudget(dr);
                    budgetInfo.Project = projectInfo;
                    budgetInfoList.Add(budgetInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Budgets from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return budgetInfoList;
        }

        /// <summary>
        /// Updates a Budget in the database
        /// </summary>
        public void UpdateBudget(BudgetInfo budgetInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(budgetInfo);

            parameters.Add(budgetInfo.Id);
            parameters.Add(budgetInfo.Amount);

            parameters.Add(budgetInfo.ModifiedDate);
            parameters.Add(budgetInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateBudget(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Budget in database");
            }
        }

        /// <summary>
        /// Adds a Budget to the database
        /// </summary>
        public int? AddBudget(BudgetInfo budgetInfo)
        {
            int? budgetId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(budgetInfo);

            parameters.Add(GetId(budgetInfo.Project));
            parameters.Add(budgetInfo.Code);
            parameters.Add(budgetInfo.Amount);

            parameters.Add(budgetInfo.CreatedDate);
            parameters.Add(budgetInfo.CreatedBy);

            try
            {
                budgetId = Data.DataProvider.GetInstance().AddBudget(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Budget to database");
            }

            return budgetId;
        }

        /// <summary>
        /// Adds or updates a Budget
        /// </summary>
        public int? AddUpdateBudget(BudgetInfo budgetInfo)
        {
            if (budgetInfo != null)
            {
                if (budgetInfo.Id != null)
                {
                    UpdateBudget(budgetInfo);
                    return budgetInfo.Id;
                }
                else
                {
                    return AddBudget(budgetInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Budget from persistent storage
        /// </summary>
        public void DeleteBudget(BudgetInfo budgetInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteBudget(budgetInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Budget from database");
            }
        }

        private TradeInfo AssignTradeBudget(IDataReader dr)
        {
            TradeInfo tradeInfo;

            tradeInfo = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
            tradeInfo.Contract = new ContractInfo();
            tradeInfo.Participations = new List<TradeParticipationInfo>();
            tradeInfo.Participations.Add(new TradeParticipationInfo());
            tradeInfo.Participations[0].SubContractor = new SubContractorInfo();

            tradeInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            tradeInfo.Contract.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
            tradeInfo.WorkOrderNumber = Data.Utils.GetDBString(dr["WorkOrderNumber"]);
            tradeInfo.Participations[0].SubContractor.ShortName = Data.Utils.GetDBString(dr["ShortName"]);

            return tradeInfo;
        }

        private VariationInfo AssignVariation(IBudget budgetProvider, IDataReader dr)
        {
            VariationInfo variationInfo;

            variationInfo = new VariationInfo(Data.Utils.GetDBInt32(dr["VariationId"]), budgetProvider);
            variationInfo.Contract = new ContractInfo();
            variationInfo.Contract.ParentContract = new ContractInfo();
            variationInfo.Contract.ParentContract.Trade = new TradeInfo();
            variationInfo.Contract.ParentContract.Trade.Participations = new List<TradeParticipationInfo>();
            variationInfo.Contract.ParentContract.Trade.Participations.Add(new TradeParticipationInfo());
            variationInfo.Contract.ParentContract.Trade.Participations[0].Rank = 1;
            variationInfo.Contract.ParentContract.Trade.Participations[0].SubContractor = new SubContractorInfo();

            variationInfo.TradeCode = Data.Utils.GetDBString(dr["TradeCode"]);
            variationInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            //#---to callculate Unallocatedvalue
            if (variationInfo.BudgetProvider != null)
                variationInfo.Allowance = Data.Utils.GetDBDecimal(dr["Allowance"]);
            //#---
            variationInfo.Number = Data.Utils.GetDBString(dr["Number"]);
            variationInfo.Header = Data.Utils.GetDBString(dr["Header"]);
            variationInfo.Type = Data.Utils.GetDBString(dr["Type"]);
            variationInfo.Contract.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
            variationInfo.Contract.ParentContract.Trade.Participations[0].SubContractor.ShortName = Data.Utils.GetDBString(dr["ShortName"]);
            variationInfo.Contract.ParentContract.Trade.WorkOrderNumber = Data.Utils.GetDBString(dr["WorkOrderNumber"]);

            return variationInfo;
        }

        /// <summary>
        /// Returns a list of Ibudget for a project.
        /// BudgetAmountInitial and BudgetAmountTradeInitial are set with the balances for the budget providers and trade code
        /// </summary>
        /// <param name="project"></param>
        public List<IBudget> GetProjectBudget(ProjectInfo projectInfo, Boolean includeAllCVSA, Boolean includeAllOVO, Boolean includeExisting = false)
        {
            Dictionary<String, Decimal> tradeCodeBalanaces = new Dictionary<String, Decimal>();
            Dictionary<String, IBudget> budgetSources = new Dictionary<String, IBudget>();
            List<IBudget> budgetList = new List<IBudget>();
            IDataReader dr = null;
            BudgetInfo budgetInfo;
            ClientVariationInfo clientVariationInfo;
            ClientVariationTradeInfo clientVariationTradeInfo;
            TradeBudgetInfo tradeBudgeInfo;
            VariationInfo variationInfo;
            TradeInfo tradeInfo;
            String cvType;
            String budgetProviderId;
            Decimal amountToInclude;
            Decimal allowanceToInclude;

            try
            {
                // Project budget
                dr = Data.DataProvider.GetInstance().GetBudgetByProject(projectInfo.Id, includeExisting);
                while (dr.Read())
                {
                    budgetInfo = new BudgetInfo(Data.Utils.GetDBInt32(dr["BudgetId"]));

                    budgetInfo.Code = Data.Utils.GetDBString(dr["Code"]);
                    budgetInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
                    budgetInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    budgetInfo.CreatedDate = Data.Utils.GetDBDateTime(dr["CreatedDate"]);

                    budgetList.Add(budgetInfo);

                    budgetInfo.BudgetAmountInitial = budgetInfo.BudgetAmount;
                    budgetInfo.BudgetUnallocated = budgetInfo.BudgetAmountInitial;
                    budgetProviderId = budgetInfo.BudgetType.ToString() + budgetInfo.IdStr;
                    budgetSources.Add(budgetProviderId, budgetInfo);

                    tradeCodeBalanaces.Add(budgetInfo.TradeCode, budgetInfo.BudgetAmountInitial.Value);
                }



                // Client variations and Separate accounts
                dr.NextResult();
                while (dr.Read())
                {
                    cvType = Data.Utils.GetDBString(dr["Type"]);

                    clientVariationTradeInfo = new ClientVariationTradeInfo(Data.Utils.GetDBInt32(dr["ClientVariationTradeId"]));
                    clientVariationTradeInfo.TradeCode = Data.Utils.GetDBString(dr["TradeCode"]);
                    clientVariationTradeInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);

                    //San----TV---    clientVariationInfo = cvType == VariationInfo.VariationTypeClient ? new ClientVariationInfo() : new SeparateAccountInfo();

                    if (Data.Utils.GetDBString(dr["Type"]) == "CV") { clientVariationInfo = new ClientVariationInfo(); }
                    else if (Data.Utils.GetDBString(dr["Type"]) == "SA") { clientVariationInfo = new SeparateAccountInfo(); }
                    else if (Data.Utils.GetDBString(dr["Type"]) == "TV")
                    {
                        clientVariationInfo = new TenantVariationInfo();
                    }
                    else { clientVariationInfo = new ClientVariationInfo(); }

                    // clientVariationInfo = cvType == VariationInfo.VariationTypeClient ? new ClientVariationInfo():(VariationInfo.VariationTypeSeparateAccounts)? new SeparateAccountInfo(): new TenantVariationInfo();
                    //#---TV


                    clientVariationInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
                    clientVariationInfo.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);

                    if (dr["ParentClientVariationId"] != DBNull.Value)
                        clientVariationInfo.RevisionName = RevisionName(Data.Utils.GetDBInt32(dr["RevisionNumber"]));

                    clientVariationTradeInfo.ClientVariation = clientVariationInfo;

                    budgetList.Add(clientVariationTradeInfo);

                    clientVariationTradeInfo.BudgetAmountInitial = (clientVariationTradeInfo.BudgetInclude || includeAllCVSA) ? clientVariationTradeInfo.BudgetAmount : 0;
                    clientVariationTradeInfo.BudgetUnallocated = clientVariationTradeInfo.BudgetAmountInitial;
                    budgetProviderId = clientVariationTradeInfo.BudgetType.ToString() + clientVariationTradeInfo.Id.Value.ToString();
                    budgetSources.Add(budgetProviderId, clientVariationTradeInfo);

                    if (tradeCodeBalanaces.ContainsKey(clientVariationTradeInfo.TradeCode))
                        tradeCodeBalanaces[clientVariationTradeInfo.TradeCode] += clientVariationTradeInfo.BudgetAmountInitial.Value;
                    else
                        tradeCodeBalanaces.Add(clientVariationTradeInfo.TradeCode, clientVariationTradeInfo.BudgetAmountInitial.Value);
                }







                // Trades budget from BOQ
                dr.NextResult();
                while (dr.Read())
                {



                    budgetProviderId = BudgetType.BOQ.ToString() + Data.Utils.GetDBInt32(dr["BudgetId"]).Value.ToString();
                    if (!budgetSources.ContainsKey(budgetProviderId))
                        throw new Exception(String.Format("Budget provider BOQ with id : {0} not found", budgetProviderId));

                    budgetInfo = (BudgetInfo)budgetSources[budgetProviderId];

                    tradeInfo = AssignTradeBudget(dr);

                    tradeBudgeInfo = new TradeBudgetInfo(tradeInfo, budgetInfo);
                    tradeBudgeInfo.Amount = Data.Utils.GetDBDecimal(dr["BudgetAmount"]);
                    tradeBudgeInfo.BudgetAmountAllowance = Data.Utils.GetDBDecimal(dr["BudgetAmountAllowance"]);

                    budgetList.Add(tradeBudgeInfo);

                    if (tradeBudgeInfo.BudgetInclude || includeAllOVO)
                    {
                        amountToInclude = tradeBudgeInfo.BudgetAmount;
                        allowanceToInclude = tradeBudgeInfo.BudgetAmountAllowance.HasValue ? -tradeBudgeInfo.BudgetAmountAllowance.Value : 0;
                    }
                    else
                    {
                        amountToInclude = 0;
                        allowanceToInclude = 0;
                    }

                    tradeCodeBalanaces[budgetInfo.TradeCode] += amountToInclude;
                    budgetInfo.BudgetAmountInitial = budgetInfo.BudgetAmountInitial.Value + amountToInclude;
                    budgetInfo.BudgetUnallocated = budgetInfo.BudgetUnallocated.Value + allowanceToInclude;



                }






                // Trades budget from CV/SA
                dr.NextResult();
                while (dr.Read())
                {



                    budgetProviderId = Data.Utils.GetDBString(dr["Type"]) + Data.Utils.GetDBInt32(dr["ClientVariationTradeId"]);

                    if (!budgetSources.ContainsKey(budgetProviderId))
                        throw new Exception(String.Format("Budget provider CV/SA with id : {0} not found", budgetProviderId));

                    clientVariationTradeInfo = (ClientVariationTradeInfo)budgetSources[budgetProviderId];

                    tradeInfo = AssignTradeBudget(dr);

                    tradeBudgeInfo = new TradeBudgetInfo(tradeInfo, clientVariationTradeInfo);
                    tradeBudgeInfo.Amount = Data.Utils.GetDBDecimal(dr["BudgetAmount"]);
                    tradeBudgeInfo.BudgetAmountAllowance = Data.Utils.GetDBDecimal(dr["BudgetAmountAllowance"]);

                    budgetList.Add(tradeBudgeInfo);

                    if (tradeBudgeInfo.BudgetInclude || includeAllOVO)
                    {
                        amountToInclude = tradeBudgeInfo.BudgetAmount;
                        allowanceToInclude = tradeBudgeInfo.BudgetAmountAllowance.HasValue ? -tradeBudgeInfo.BudgetAmountAllowance.Value : 0;
                    }
                    else
                    {
                        amountToInclude = 0;
                        allowanceToInclude = 0;
                    }

                    tradeCodeBalanaces[clientVariationTradeInfo.TradeCode] += amountToInclude;
                    clientVariationTradeInfo.BudgetAmountInitial = clientVariationTradeInfo.BudgetAmountInitial.Value + amountToInclude;
                    clientVariationTradeInfo.BudgetUnallocated = clientVariationTradeInfo.BudgetUnallocated.Value + allowanceToInclude;
                }





                // Variations with no budget provider
                dr.NextResult();
                while (dr.Read())
                {
                    variationInfo = AssignVariation(null, dr);
                    budgetList.Add(variationInfo);
                    amountToInclude = (variationInfo.BudgetInclude || includeAllOVO) ? variationInfo.BudgetAmount : 0;

                    if (tradeCodeBalanaces.ContainsKey(variationInfo.TradeCode))
                        tradeCodeBalanaces[variationInfo.TradeCode] += amountToInclude;
                    else
                        tradeCodeBalanaces.Add(variationInfo.TradeCode, amountToInclude);
                }




                // Variations with trade budget budget provider
                dr.NextResult();
                while (dr.Read())
                {


                    budgetProviderId = BudgetType.BOQ.ToString() + Data.Utils.GetDBInt32(dr["BudgetId"]);

                    if (!budgetSources.ContainsKey(budgetProviderId))
                        throw new Exception(String.Format("Budget provider BOQ with id : {0} not found", budgetProviderId));

                    budgetInfo = (BudgetInfo)budgetSources[budgetProviderId];
                    variationInfo = AssignVariation(budgetInfo, dr);
                    budgetList.Add(variationInfo);

                    if (variationInfo.BudgetInclude || includeAllOVO)
                    {
                        amountToInclude = variationInfo.BudgetAmount;
                        allowanceToInclude = variationInfo.Allowance.HasValue ? -variationInfo.Allowance.Value : 0;
                    }
                    else
                    {
                        amountToInclude = 0;
                        allowanceToInclude = 0;
                    }

                    tradeCodeBalanaces[budgetInfo.TradeCode] += amountToInclude;
                    budgetInfo.BudgetAmountInitial = budgetInfo.BudgetAmountInitial.Value + amountToInclude;
                    budgetInfo.BudgetUnallocated = budgetInfo.BudgetUnallocated + allowanceToInclude;


                }





                // Variations with CV/SA budget budget provider
                dr.NextResult();
                while (dr.Read())
                {
                    budgetProviderId = Data.Utils.GetDBString(dr["ClientVariationType"]) + Data.Utils.GetDBInt32(dr["ClientVariationTradeId"]);

                    if (!budgetSources.ContainsKey(budgetProviderId))
                        throw new Exception(String.Format("Budget provider CV/SA with id : {0} not found", budgetProviderId));

                    clientVariationTradeInfo = (ClientVariationTradeInfo)budgetSources[budgetProviderId];
                    variationInfo = AssignVariation(clientVariationTradeInfo, dr);
                    budgetList.Add(variationInfo);

                    if (variationInfo.BudgetInclude || includeAllOVO)
                    {
                        amountToInclude = variationInfo.BudgetAmount;
                        allowanceToInclude = variationInfo.Allowance.HasValue ? -variationInfo.Allowance.Value : 0;
                    }
                    else
                    {
                        amountToInclude = 0;
                        allowanceToInclude = 0;
                    }

                    tradeCodeBalanaces[clientVariationTradeInfo.TradeCode] += amountToInclude;
                    clientVariationTradeInfo.BudgetAmountInitial = clientVariationTradeInfo.BudgetAmountInitial.Value + amountToInclude;
                    clientVariationTradeInfo.BudgetUnallocated = clientVariationTradeInfo.BudgetUnallocated + allowanceToInclude;
                }

                if (includeExisting)
                {
                    // for each trade we create a BudgetInfo object
                    dr.NextResult();
                    while (dr.Read())
                    {
                        budgetProviderId = "Trade" + Data.Utils.GetDBInt32(dr["TradeId"]).Value.ToString();
                        budgetInfo = new BudgetInfo();

                        budgetInfo.Code = Data.Utils.GetDBString(dr["Code"]);
                        budgetInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
                        budgetInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                        budgetInfo.CreatedDate = Data.Utils.GetDBDateTime(dr["CreatedDate"]);
                        budgetInfo.Adjustments = Data.Utils.GetDBDecimal(dr["Adjustments"]);

                        budgetList.Add(budgetInfo);

                        budgetInfo.BudgetAmountInitial = budgetInfo.BudgetAmount;
                        budgetInfo.BudgetUnallocated = budgetInfo.BudgetAmountInitial;

                        budgetSources.Add(budgetProviderId, budgetInfo);

                        if (tradeCodeBalanaces.ContainsKey(budgetInfo.TradeCode))
                            tradeCodeBalanaces[budgetInfo.TradeCode] += budgetInfo.BudgetAmountInitial.Value;
                        else
                            tradeCodeBalanaces.Add(budgetInfo.TradeCode, budgetInfo.BudgetAmountInitial.Value);
                    }




                    // For each contract we create a TradeBudgetInfo object
                    dr.NextResult();
                    while (dr.Read())
                    {
                        budgetProviderId = "Trade" + Data.Utils.GetDBInt32(dr["TradeId"]).Value.ToString();
                        budgetInfo = (BudgetInfo)budgetSources[budgetProviderId];

                        tradeInfo = AssignTradeBudget(dr);

                        tradeBudgeInfo = new TradeBudgetInfo(tradeInfo, budgetInfo);
                        tradeBudgeInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
                        tradeBudgeInfo.BudgetAmountAllowance = tradeBudgeInfo.Amount;

                        budgetList.Add(tradeBudgeInfo);

                        amountToInclude = (tradeBudgeInfo.BudgetInclude || includeAllOVO) ? tradeBudgeInfo.BudgetAmount : 0;

                        tradeCodeBalanaces[budgetInfo.TradeCode] += amountToInclude;
                        budgetInfo.BudgetAmountInitial = budgetInfo.BudgetAmountInitial.Value + amountToInclude;
                        budgetInfo.BudgetUnallocated = budgetInfo.BudgetUnallocated.Value + amountToInclude;
                    }





                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Budget from database. " + ex.Message);
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            // Assigns the trade code balance to all the IBudget objects
            foreach (IBudget iBudget in budgetList)
                iBudget.BudgetAmountTradeInitial = tradeCodeBalanaces[iBudget.TradeCode];

            return budgetList;
        }

        /// <summary>
        /// Updates all the trade tradebudgets
        /// Set the trade budget's initial budget and trade budget balance. The inicial budget balance used is the one based on approved budget.
        /// </summary>
        public void UpdateTradeBudgets(TradeInfo tradeInfo)
        {
            TradesController tradesController = TradesController.GetInstance();
            List<IBudget> projectBudget = GetProjectBudget(tradeInfo.Project, false, false);
            IBudget budgetProvider;

            if (tradeInfo.TradeBudgets != null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (TradeBudgetInfo tradeBudgetInfo in tradeInfo.TradeBudgets)
                    {
                        budgetProvider = projectBudget.Find(pb => pb.Equals(tradeBudgetInfo.BudgetProvider));

                        if (budgetProvider == null)
                        {
                            Utils.LogError(String.Format("Budget provider [Name: {0}, Type: {1}] not found", budgetProvider.BudgetName, budgetProvider.BudgetType));
                            throw new Exception("Updating trade budgets. Budget provider not found.");
                        }
                        else
                        {
                            tradeBudgetInfo.BudgetAmountInitial = budgetProvider.BudgetAmountInitial;
                            tradeBudgetInfo.BudgetAmountTradeInitial = budgetProvider.BudgetAmountTradeInitial;

                            tradesController.UpdateTradeBudget(tradeBudgetInfo);
                        }
                    }

                    scope.Complete();
                }
            }
        }

        /// <summary>
        /// Import a list of trade budgets from an excel file in a memroy stream
        /// </summary>
        public void ImportTradeBudgets(ProjectInfo projectInfo, String fileName, MemoryStream memoryStream)
        {
            IExcelDataReader excelDataReader;
            String tradeCode;
            Decimal tradeValue;
            String tradeValueStr;
            TradeTemplateInfo tradeTemplate;
            String processInfo = String.Empty;
            BudgetInfo budgetInfo;

            List<TradeTemplateInfo> tradeTemplates = TradesController.GetInstance().GetTradeTemplates();

            if (Path.GetExtension(fileName).ToLower() == ".xls")
            {
                excelDataReader = ExcelReaderFactory.CreateBinaryReader(memoryStream);
            }
            else if (Path.GetExtension(fileName).ToLower() == ".xlsx")
            {
                excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(memoryStream);
            }
            else
            {
                throw new Exception("File must be of type .xls or .xlsx");
            }

            DataSet dataSet = excelDataReader.AsDataSet();

            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count >= 2 && dataSet.Tables[0].Rows.Count >= 1)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        tradeCode = row[0] != null ? row[0].ToString().Trim() : null;

                        if (!String.IsNullOrEmpty(tradeCode))
                        {
                            if (tradeCode.Length > 6)
                                tradeCode = row[0].ToString().Trim().Substring(0, 6);

                            if (tradeCode.Length == 6)
                                tradeCode = String.Format("{0}.{1}.{2}", tradeCode.Substring(0, 2), tradeCode.Substring(2, 2), tradeCode.Substring(4, 2));

                            tradeTemplate = tradeTemplates.FirstOrDefault(tt => tt.TradeCode.Equals(tradeCode));

                            if (tradeTemplate != null)
                            {
                                tradeValueStr = row[1].ToString().Trim();

                                if (String.IsNullOrEmpty(tradeValueStr))
                                {
                                    tradeValue = 0;
                                }
                                else
                                {
                                    if (!Decimal.TryParse(tradeValueStr, out tradeValue))
                                    {
                                        throw new Exception(String.Format("Invalid amount: {0} for trade code: {1}.", tradeValueStr, tradeCode));
                                    }
                                }

                                if (!projectInfo.Budgets.Exists(b => b.TradeCode == tradeCode))
                                {
                                    budgetInfo = new BudgetInfo();
                                    budgetInfo.Project = projectInfo;
                                    budgetInfo.Code = tradeCode;
                                    budgetInfo.Amount = tradeValue;

                                    AddBudget(budgetInfo);

                                    projectInfo.Budgets.Add(budgetInfo);
                                }
                            }
                            else
                            {
                                throw new Exception(String.Format("The trade code: {0} doest not exist on the system.", tradeCode));
                            }
                        }
                    }

                    scope.Complete();
                }
            }
            else
            {
                throw new Exception("Invalid file.");
            }
        }
        #endregion


        #region Client Variations Methods
        /// <summary>
        /// Returns a client variation object based on the type
        /// </summary>
        public ClientVariationInfo GetClientVariationObject(String type, int? clientVariationId)
        {
            if (type == ClientVariationInfo.VariationTypeClient)
                return new ClientVariationInfo(clientVariationId);
            //---#---TV
            else if (type == ClientVariationInfo.VariationTypeTenant)
                return new TenantVariationInfo(clientVariationId);
            //---#----TV

            else
                return new SeparateAccountInfo(clientVariationId);
        }

        /// <summary>
        /// Returns a client variation object based on the type
        /// </summary>
        public ClientVariationInfo GetClientVariationObject(String type)
        {
            return GetClientVariationObject(type, null);
        }

        /// <summary>
        /// Creates a ClientVariation from a dr
        /// </summary>
        public ClientVariationInfo CreateClientVariation(IDataReader dr)
        {
            ClientVariationInfo clientVariationInfo = GetClientVariationObject(Data.Utils.GetDBString(dr["Type"]), Data.Utils.GetDBInt32(dr["ClientVariationId"]));

            clientVariationInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            clientVariationInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
            clientVariationInfo.GoodsServicesTax = Data.Utils.GetDBDecimal(dr["GoodsServicesTax"]);
            clientVariationInfo.WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);
            clientVariationInfo.InternalApprovalDate = Data.Utils.GetDBDateTime(dr["InternalApprovalDate"]);
            clientVariationInfo.VerbalApprovalDate = Data.Utils.GetDBDateTime(dr["VerbalApprovalDate"]);
            clientVariationInfo.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
            clientVariationInfo.QuotesFile = Data.Utils.GetDBString(dr["QuotesFile"]);
            clientVariationInfo.BackupFile = Data.Utils.GetDBString(dr["BackupFile"]);
            clientVariationInfo.ClientApprovalFile = Data.Utils.GetDBString(dr["ClientApprovalFile"]);
            clientVariationInfo.HideCostDetails = Data.Utils.GetDBBoolean(dr["HideCostDetails"]);
            clientVariationInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

            if (clientVariationInfo is SeparateAccountInfo)
            {
                SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                separateAccountInfo.InvoiceNumber = Data.Utils.GetDBInt32(dr["InvoiceNumber"]);
                separateAccountInfo.InvoiceDate = Data.Utils.GetDBDateTime(dr["InvoiceDate"]);
                separateAccountInfo.InvoiceSentDate = Data.Utils.GetDBDateTime(dr["InvoiceSentDate"]);
                separateAccountInfo.InvoiceDueDate = Data.Utils.GetDBDateTime(dr["InvoiceDueDate"]);
                separateAccountInfo.InvoicePaidDate = Data.Utils.GetDBDateTime(dr["InvoicePaidDate"]);
                separateAccountInfo.WorksCompletedDate = Data.Utils.GetDBDateTime(dr["WorksCompletedDate"]);
                separateAccountInfo.UseSecondPrincipal = Data.Utils.GetDBBoolean(dr["UseSecondPrincipal"]);
            }

            AssignAuditInfo(clientVariationInfo, dr);

            if (dr["ProjectId"] != DBNull.Value)
            {
                clientVariationInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                clientVariationInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                clientVariationInfo.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
            }

            if (dr["ParentClientVariationId"] != DBNull.Value)
            {
                clientVariationInfo.ParentClientVariation = GetClientVariationObject(Data.Utils.GetDBString(dr["Type"]), Data.Utils.GetDBInt32(dr["ParentClientVariationId"]));
                clientVariationInfo.CancelDate = Data.Utils.GetDBDateTime(dr["ParentCancelDate"]);
            }
            else
                clientVariationInfo.CancelDate = Data.Utils.GetDBDateTime(dr["CancelDate"]);

            if (dr["VariationId"] != DBNull.Value) clientVariationInfo.Variation = ContractsController.GetInstance().GetVariation(Data.Utils.GetDBInt32(dr["VariationId"]));
            if (dr["ProcessId"] != DBNull.Value) clientVariationInfo.Process = ProcessController.GetInstance().GetDeepProcess(Data.Utils.GetDBInt32(dr["ProcessId"]));

            return clientVariationInfo;
        }

        /// <summary>
        /// Get a Client Variation from persistent storage
        /// </summary>
        public ClientVariationInfo GetClientVariation(int? clientVariationId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariation(clientVariationId);
                if (dr.Read())
                    return CreateClientVariation(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a Client Variation last revision by number from persistent storage
        /// </summary>
        public ClientVariationInfo GetClientVariationByNumber(int? number, ProjectInfo projectInfo, String type)
        {
            List<Object> parameters = new List<Object>();
            ClientVariationInfo clientVariationInfo = null;
            IDataReader dr = null;
            IDataReader dr1 = null;
            int? clientVariationId;

            parameters.Add(number);
            parameters.Add(GetId(projectInfo));
            parameters.Add(type);

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationByNumber(parameters.ToArray());
                if (dr.Read())
                {
                    clientVariationInfo = CreateClientVariation(dr);

                    dr1 = Data.DataProvider.GetInstance().GetClientVariationMaxId(clientVariationInfo.Id);
                    if (dr1.Read())
                    {
                        clientVariationId = Data.Utils.GetDBInt32(dr1["MaxClientVariationId"]);

                        if (clientVariationId != null)
                            clientVariationInfo = GetClientVariation(clientVariationId);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation by number from database");
            }
            finally
            {
                if (dr1 != null)
                    dr1.Close();

                if (dr != null)
                    dr.Close();
            }

            return clientVariationInfo;
        }

        /// <summary>
        /// Get the Client Variations for the specified Project
        /// </summary>
        public List<ClientVariationInfo> GetClientVariations(ProjectInfo projectInfo, String type)
        {
            IDataReader dr = null;
            List<ClientVariationInfo> clientVariationInfoList = new List<ClientVariationInfo>();
            ClientVariationInfo clientVariationInfo;
            List<Object> parameters = new List<Object>();

            parameters.Add(projectInfo.Id);
            parameters.Add(type);

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationsByProject((parameters.ToArray()));
                while (dr.Read())
                {
                    clientVariationInfo = GetClientVariationWithSubClientVariations(Data.Utils.GetDBInt32(dr["ClientVariationId"]));

                    if (!clientVariationInfo.IsLastSubClientVariation)
                    {
                        clientVariationInfo.SubClientVariations[clientVariationInfo.SubClientVariations.Count - 1] = GetClientVariation(clientVariationInfo.LastSubClientVariation.Id);
                        clientVariationInfo.SubClientVariations[clientVariationInfo.SubClientVariations.Count - 1].ParentClientVariation = clientVariationInfo;
                    }

                    clientVariationInfo.LastSubClientVariation.Trades = GetClientVariationTrades(clientVariationInfo.LastSubClientVariation);
                    clientVariationInfo.LastSubClientVariation.Items = GetClientVariationItems(clientVariationInfo.LastSubClientVariation);

                    SetRevisionNames(clientVariationInfo);

                    clientVariationInfoList.Add(clientVariationInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Client Variations from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientVariationInfoList;
        }

        /// <summary>
        /// Get the Client Variations latest revisions for the specified Project
        /// </summary>
        public List<ClientVariationInfo> GetClientVariationsLastRevisions(ProjectInfo projectInfo, String type)
        {
            List<ClientVariationInfo> clientVariationInfoList = GetClientVariations(projectInfo, type);
            List<ClientVariationInfo> clientVariationInfoLastRevisionList = new List<ClientVariationInfo>();

            foreach (ClientVariationInfo clientVariationInfo in clientVariationInfoList)
                clientVariationInfoLastRevisionList.Add(clientVariationInfo.LastSubClientVariation);

            return clientVariationInfoLastRevisionList;
        }

        /// <summary>
        /// Get the Client Variations latest revisions for the specified Project
        /// </summary>
        public List<ClientVariationInfo> GetClientVariationsLastRevisions(ProjectInfo projectInfo)
        {
            return GetClientVariationsLastRevisions(projectInfo, ClientVariationInfo.VariationTypeClient);
        }

        /// <summary>
        /// Get the Client Variations type separate accounts latest revisions for the specified Project
        /// </summary>
        public List<ClientVariationInfo> GetSeparateAccountsLastRevisions(ProjectInfo projectInfo)
        {
            return GetClientVariationsLastRevisions(projectInfo, ClientVariationInfo.VariationTypeSeparateAccounts);
        }


        //#-----TV------
        /// <summary>
        /// Get the Client Variations type Tenant variation latest revisions for the specified Project
        /// </summary>
        public List<ClientVariationInfo> GetTenantVariationsLastRevisions(ProjectInfo projectInfo)
        {
            return GetClientVariationsLastRevisions(projectInfo, ClientVariationInfo.VariationTypeTenant);
        }

        //#-----TV------

        /// <summary>
        /// Get the Sub Client Variations for the specified Client Variation
        /// </summary>
        public List<ClientVariationInfo> GetClientVariations(ClientVariationInfo clientVariationInfo)
        {
            IDataReader dr = null;
            List<ClientVariationInfo> clientVariationInfoList = new List<ClientVariationInfo>();
            ClientVariationInfo subClientVariationInfo;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationsByClientVariation(clientVariationInfo.Id);
                while (dr.Read())
                {
                    subClientVariationInfo = CreateClientVariation(dr);
                    subClientVariationInfo.ParentClientVariation = clientVariationInfo;
                    clientVariationInfoList.Add(subClientVariationInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation Revisions from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientVariationInfoList;
        }

        /// <summary>
        /// Updates a Client Variation in the database
        /// </summary>
        public void UpdateClientVariation(ClientVariationInfo clientVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(clientVariationInfo);

            parameters.Add(clientVariationInfo.Id);
            parameters.Add(clientVariationInfo.Name);
            parameters.Add(clientVariationInfo.WriteDate);
            parameters.Add(clientVariationInfo.InternalApprovalDate);
            parameters.Add(clientVariationInfo.VerbalApprovalDate);
            parameters.Add(clientVariationInfo.ApprovalDate);
            parameters.Add(clientVariationInfo.QuotesFile);
            parameters.Add(clientVariationInfo.BackupFile);
            parameters.Add(clientVariationInfo.ClientApprovalFile);
            parameters.Add(clientVariationInfo.HideCostDetails);
            parameters.Add(clientVariationInfo.Comments);

            if (clientVariationInfo is SeparateAccountInfo)
            {
                SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                parameters.Add(separateAccountInfo.InvoiceNumber);
                parameters.Add(separateAccountInfo.InvoiceDate);
                parameters.Add(separateAccountInfo.InvoiceSentDate);
                parameters.Add(separateAccountInfo.InvoiceDueDate);
                parameters.Add(separateAccountInfo.InvoicePaidDate);
                parameters.Add(separateAccountInfo.WorksCompletedDate);
                parameters.Add(separateAccountInfo.UseSecondPrincipal);
            }
            else
            {
                parameters.Add(null);
                parameters.Add(null);
                parameters.Add(null);
                parameters.Add(null);
                parameters.Add(null);
                parameters.Add(null);
                parameters.Add(null);
            }

            parameters.Add(clientVariationInfo.ModifiedDate);
            parameters.Add(clientVariationInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariation(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation in database");
            }
        }

        /// <summary>
        /// Updates a Client Variation's Internal approval in the database
        /// </summary>
        public void UpdateClientVariationInternalApproval(ClientVariationInfo clientVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            if (clientVariationInfo.InternalApprovalDate != null)
                throw new Exception("The Internal Approval Date has already been set");
            else
            {
                clientVariationInfo.InternalApprovalDate = DateTime.Today;

                parameters.Add(clientVariationInfo.Id);
                parameters.Add(clientVariationInfo.InternalApprovalDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationInternalApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Internal Approval Date in database");
            }
        }

        /// <summary>
        /// Updates a Client Variation's Verbal approval in the database
        /// </summary>
        public void UpdateClientVariationVerbalApproval(ClientVariationInfo clientVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            if (clientVariationInfo.VerbalApprovalDate != null)
                throw new Exception("The Verbal Approval Date has already been set");
            else
            {
                clientVariationInfo.VerbalApprovalDate = DateTime.Today;

                parameters.Add(clientVariationInfo.Id);
                parameters.Add(clientVariationInfo.VerbalApprovalDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationVerbalApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Verbal Approval Date in database");
            }
        }

        /// <summary>
        /// Updates a Separate Account's InvoiceSentDate in the database
        /// </summary>
        public void UpdateSeparateAccountInvoiceSent(SeparateAccountInfo separateAccountInfo)
        {
            List<Object> parameters = new List<Object>();

            if (separateAccountInfo.InvoiceSentDate != null)
                throw new Exception("The Invoice Sent Date has already been set");
            else
            {
                separateAccountInfo.InvoiceSentDate = DateTime.Today;

                parameters.Add(separateAccountInfo.Id);
                parameters.Add(separateAccountInfo.InvoiceSentDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationInvoiceSent(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Invoice Sent in database");
            }
        }

        /// <summary>
        /// Updates a Separate Account's WorksCompletedDate in the database
        /// </summary>
        public void UpdateSeparateAccountWorksCompleted(SeparateAccountInfo separateAccountInfo)
        {
            List<Object> parameters = new List<Object>();

            if (separateAccountInfo.WorksCompletedDate != null)
                throw new Exception("The Works Completed Date has already been set");
            else
            {
                separateAccountInfo.WorksCompletedDate = DateTime.Today;

                parameters.Add(separateAccountInfo.Id);
                parameters.Add(separateAccountInfo.WorksCompletedDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationWorksCompleted(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Works Completed in database");
            }
        }

        /// <summary>
        /// Updates a Client Variation's Final approval in the database
        /// </summary>
        public void UpdateClientVariationFinalApproval(ClientVariationInfo clientVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            if (clientVariationInfo.ApprovalDate != null)
                throw new Exception("The Final Approval Date has already been set");
            else
            {
                clientVariationInfo.ApprovalDate = DateTime.Today;

                parameters.Add(clientVariationInfo.Id);
                parameters.Add(clientVariationInfo.ApprovalDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationFinalApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Final Approval Date in database");
            }
        }

        /// <summary>
        /// Updates a Client Variation's Cancel date in the database
        /// </summary>
        public void UpdateClientVariationCancel(ClientVariationInfo clientVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(clientVariationInfo.TheParentClientVariation.Id);
            parameters.Add(clientVariationInfo.CancelDate);

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationCancel(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Cancel Date in database");
            }
        }

        /// <summary>
        /// Cancel a client variation
        /// </summary>
        public void CancelClientVariation(ClientVariationInfo clientVariationInfo)
        {
            clientVariationInfo.CancelDate = DateTime.Today;
            UpdateClientVariationCancel(clientVariationInfo);
        }

        /// <summary>
        /// Restore a client variation
        /// </summary>
        public void RestoreClientVariation(ClientVariationInfo clientVariationInfo)
        {
            clientVariationInfo.CancelDate = null;
            UpdateClientVariationCancel(clientVariationInfo);
        }

        /// <summary>
        /// Assigns the next Client Variation Number
        /// </summary>
        public void SetNextClientVariationNumber(ClientVariationInfo clientVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(clientVariationInfo.Project.Id);
            parameters.Add(clientVariationInfo.Type);

            if (clientVariationInfo.ParentClientVariation == null)
                clientVariationInfo.Number = Data.DataProvider.GetInstance().GetClientVariationNumber(parameters.ToArray());
        }

        /// <summary>
        /// Adds a Client Variation to the database
        /// </summary>
        private int? AddClientVariation(ClientVariationInfo clientVariationInfo, ProcessInfo processInfo)
        {
            int? clientVariationId = null;
            List<Object> parameters = new List<Object>();
            ProcessController processController = ProcessController.GetInstance();
            DateTime? dateTime;

            SetCreateInfo(clientVariationInfo);

            try
            {
                SetNextClientVariationNumber(clientVariationInfo);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (processInfo != null)
                    {
                        processInfo.Id = processController.AddProcess(processInfo);

                        dateTime = DateTime.Today;
                        InitializeHolidays();

                        foreach (ProcessStepInfo processStepInfo in processInfo.Steps)
                        {
                            dateTime = AddbusinessDaysToDate(dateTime, (int)processStepInfo.NumDays);
                            processStepInfo.TargetDate = dateTime;
                            processStepInfo.Id = processController.AddProcessStep(processStepInfo);
                        }

                        clientVariationInfo.Process = processInfo;
                    }

                    parameters.Add(GetId(clientVariationInfo.ParentClientVariation));
                    parameters.Add(GetId(clientVariationInfo.Project));
                    parameters.Add(GetId(clientVariationInfo.Process));
                    parameters.Add(GetId(clientVariationInfo.Variation));
                    parameters.Add(clientVariationInfo.Type);
                    parameters.Add(clientVariationInfo.Name);
                    parameters.Add(clientVariationInfo.Number);
                    parameters.Add(clientVariationInfo.GoodsServicesTax);
                    parameters.Add(clientVariationInfo.WriteDate);
                    parameters.Add(clientVariationInfo.InternalApprovalDate);
                    parameters.Add(clientVariationInfo.VerbalApprovalDate);
                    parameters.Add(clientVariationInfo.ApprovalDate);
                    parameters.Add(clientVariationInfo.QuotesFile);
                    parameters.Add(clientVariationInfo.BackupFile);
                    parameters.Add(clientVariationInfo.ClientApprovalFile);
                    parameters.Add(clientVariationInfo.HideCostDetails);
                    parameters.Add(clientVariationInfo.Comments);

                    if (clientVariationInfo is SeparateAccountInfo)
                    {
                        SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                        parameters.Add(separateAccountInfo.InvoiceNumber);
                        parameters.Add(separateAccountInfo.InvoiceDate);
                        parameters.Add(separateAccountInfo.InvoiceSentDate);
                        parameters.Add(separateAccountInfo.InvoiceDueDate);
                        parameters.Add(separateAccountInfo.InvoicePaidDate);
                        parameters.Add(separateAccountInfo.WorksCompletedDate);
                        parameters.Add(separateAccountInfo.UseSecondPrincipal);
                    }
                    else
                    {
                        parameters.Add(null);
                        parameters.Add(null);
                        parameters.Add(null);
                        parameters.Add(null);
                        parameters.Add(null);
                        parameters.Add(null);
                        parameters.Add(null);
                    }

                    parameters.Add(clientVariationInfo.CreatedDate);
                    parameters.Add(clientVariationInfo.CreatedBy);

                    clientVariationId = Data.DataProvider.GetInstance().AddClientVariation(parameters.ToArray());

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Client Variation to database");
            }

            return clientVariationId;
        }

        /// <summary>
        /// Adds a Client Variation to the database
        /// </summary>
        public int? AddClientVariation(ClientVariationInfo clientVariationInfo)
        {
            ProcessInfo processInfo = null;

            try
            {
                processInfo = ProcessController.GetInstance().GetDeepProcessTemplateForClientVariations(clientVariationInfo);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting process template for client variation from database");
            }

            return AddClientVariation(clientVariationInfo, processInfo);
        }

        /// <summary>
        /// Adds or updates a Client Variation
        /// </summary>
        public int? AddUpdateClientVariation(ClientVariationInfo clientVariationInfo)
        {
            if (clientVariationInfo != null)
            {
                if (clientVariationInfo.Id != null)
                {
                    UpdateClientVariation(clientVariationInfo);
                    return clientVariationInfo.Id;
                }
                else
                {
                    return AddClientVariation(clientVariationInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Client Variation with process and subvariations from persistent storage
        /// </summary>
        public void DeleteClientVariation(ClientVariationInfo clientVariationInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (clientVariationInfo.SubClientVariations != null)
                        foreach (ClientVariationInfo subClientVariation in clientVariationInfo.SubClientVariations)
                            DeleteClientVariation(subClientVariation);

                    Data.DataProvider.GetInstance().DeleteClientVariation(clientVariationInfo.Id);

                    if (clientVariationInfo.Process != null)
                        ProcessController.GetInstance().DeleteProcess(clientVariationInfo.Process);

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Client Variation from database");
            }
        }

        /// <summary>
        /// Get a Client Variation with Items and trades from persistent storage
        /// </summary>
        public ClientVariationInfo GetClientVariationWithItemsAndTrades(int? clientVariationId)
        {
            ClientVariationInfo clientVariationInfo = GetClientVariation(clientVariationId);
            clientVariationInfo.Items = GetClientVariationItems(clientVariationInfo);
            clientVariationInfo.Trades = GetClientVariationTrades(clientVariationInfo);
            return clientVariationInfo;
        }

        /// <summary>
        /// Get a Client Variation by number with Items from persistent storage
        /// </summary>
        public ClientVariationInfo GetClientVariationByNumberWithItemsAndTrades(int? number, ProjectInfo projectInfo, String type)
        {
            ClientVariationInfo clientVariationInfo = GetClientVariationByNumber(number, projectInfo, type);

            if (clientVariationInfo != null)
            {
                clientVariationInfo.Items = GetClientVariationItems(clientVariationInfo);
                clientVariationInfo.Trades = GetClientVariationTrades(clientVariationInfo);
            }

            return clientVariationInfo;
        }

        /// <summary>
        /// Get a Client Variation with sub client variations from persistent storage
        /// </summary>
        public ClientVariationInfo GetClientVariationWithSubClientVariations(int? clientVariationId)
        {
            ClientVariationInfo clientVariationInfo = GetClientVariation(clientVariationId);
            clientVariationInfo.SubClientVariations = GetClientVariations(clientVariationInfo);
            return clientVariationInfo;
        }

        /// <summary>
        /// Checks a Client Variation for errors and missing fields
        /// </summary>
        public XmlDocument CheckClientVariation(ClientVariationInfo clientVariationInfo)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElement;
            FileInfo fileInfo;
            ProcessStepInfo currentStep = clientVariationInfo.Process != null ? ProcessController.GetInstance().GetCurrentStep(clientVariationInfo.Process) : null;

            xmlElementRoot = xmlDocument.CreateElement("root", null);
            xmlElementRoot.SetAttribute("name", "Variation Check");

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElement = xmlDocument.CreateElement("Fields", null);
            xmlElement.SetAttribute("name", "Missing Fields");

            Utils.AddMissingFieldNode(clientVariationInfo.Project.Name, xmlDocument, xmlElement, "Project name");
            Utils.AddMissingFieldNode(clientVariationInfo.Project.FullNumber, xmlDocument, xmlElement, "Project number");
            Utils.AddMissingFieldNode(clientVariationInfo.WriteDate, xmlDocument, xmlElement, "Write date");

            Utils.AddMissingFieldNode(clientVariationInfo.Project.ProjectManager, xmlDocument, xmlElement, "Project manager");
            if (clientVariationInfo.Project.ProjectManager != null)
            {
                Utils.AddMissingFieldNode(clientVariationInfo.Project.ProjectManager.Name, xmlDocument, xmlElement, "Project manager name");
                Utils.AddMissingFieldNode(clientVariationInfo.Project.ProjectManager.Signature, xmlDocument, xmlElement, "Project manager signature");
            }

            Utils.AddMissingFieldNode(clientVariationInfo.Project.ConstructionManager, xmlDocument, xmlElement, "Construction manager");
            if (clientVariationInfo.Project.ConstructionManager != null)
            {
                Utils.AddMissingFieldNode(clientVariationInfo.Project.ConstructionManager.Name, xmlDocument, xmlElement, "Construction manager name");
                Utils.AddMissingFieldNode(clientVariationInfo.Project.ConstructionManager.Signature, xmlDocument, xmlElement, "Construction manager signature");
            }

            Utils.AddMissingFieldNode(clientVariationInfo.Number, xmlDocument, xmlElement, "Number");
            Utils.AddMissingFieldNode(clientVariationInfo.Name, xmlDocument, xmlElement, "Name");
            Utils.AddMissingFieldNode(clientVariationInfo.TotalAmount, xmlDocument, xmlElement, "Total amount");
            Utils.AddMissingFieldNode(clientVariationInfo.TotalGST, xmlDocument, xmlElement, "Total GST");
            Utils.AddMissingFieldNode(clientVariationInfo.TotalAmountPlusGST, xmlDocument, xmlElement, "Grand total");
            Utils.AddMissingFieldNode(clientVariationInfo.QuotesFile, xmlDocument, xmlElement, "VC Quotes file");

            if (currentStep != null && currentStep.Type == ProcessStepInfo.StepTypeClientVariationClientFinalApproval)
                Utils.AddMissingFieldNode(clientVariationInfo.ClientApprovalFile, xmlDocument, xmlElement, "Client approval file");

            if (clientVariationInfo is SeparateAccountInfo)
            {
                SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                if (separateAccountInfo.WorksCompleted)
                {
                    Utils.AddMissingFieldNode(separateAccountInfo.InvoiceNumber, xmlDocument, xmlElement, "Invoice number");
                    Utils.AddMissingFieldNode(separateAccountInfo.InvoiceDate, xmlDocument, xmlElement, "Invoice date");
                    Utils.AddMissingFieldNode(separateAccountInfo.InvoiceDueDate, xmlDocument, xmlElement, "Invoice due date");
                }

                if (separateAccountInfo.InvoiceSent)
                    Utils.AddMissingFieldNode(separateAccountInfo.InvoicePaidDate, xmlDocument, xmlElement, "Invoice date paid");
            }

            if (clientVariationInfo.Items == null || clientVariationInfo.Items.Count == 0)
                Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Items");

            if (clientVariationInfo.Trades == null || clientVariationInfo.Trades.Count == 0)
                Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Trades");

            if (xmlElement.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElement);

            xmlElement = xmlDocument.CreateElement("Errors", null);
            xmlElement.SetAttribute("name", "Errors");

            if (clientVariationInfo.ItemsMinusTrades != null)
                if (clientVariationInfo.ItemsMinusTrades != 0)
                    Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Items and Trades amounts don't match");

            if (clientVariationInfo.QuotesFile != null)
            {
                fileInfo = new FileInfo(UI.Utils.FullPath(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.QuotesFile));
                if (!fileInfo.Exists)
                    Utils.AddErrorMessageNode(xmlDocument, xmlElement, "VC Quotes file does not exist");
            }

            if (currentStep != null && currentStep.Type == ProcessStepInfo.StepTypeClientVariationClientFinalApproval)
                if (clientVariationInfo.ClientApprovalFile != null)
                {
                    fileInfo = new FileInfo(UI.Utils.FullPath(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.ClientApprovalFile));
                    if (!fileInfo.Exists)
                        Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Client approval file does not exist");
                }

            if (xmlElement.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElement);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        /// <summary>
        /// Creates a new revision for a client variation
        /// </summary>
        public ClientVariationInfo MakeRevision(ClientVariationInfo clientVariationInfo)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            ClientVariationInfo childClientVariationInfo = null;
            ClientVariationItemInfo childClientVariationItemInfo;
            ClientVariationTradeInfo childClientVariationTradeInfo;
            ProcessInfo processInfo;

            childClientVariationInfo = GetClientVariationObject(clientVariationInfo.Type);

            childClientVariationInfo.ParentClientVariation = clientVariationInfo.TheParentClientVariation;
            childClientVariationInfo.WriteDate = DateTime.Today;

            childClientVariationInfo.Project = clientVariationInfo.Project;
            childClientVariationInfo.Number = clientVariationInfo.Number;
            childClientVariationInfo.Name = clientVariationInfo.Name;
            childClientVariationInfo.GoodsServicesTax = clientVariationInfo.GoodsServicesTax;
            childClientVariationInfo.QuotesFile = clientVariationInfo.QuotesFile;
            childClientVariationInfo.BackupFile = clientVariationInfo.BackupFile;
            childClientVariationInfo.ClientApprovalFile = clientVariationInfo.ClientApprovalFile;
            childClientVariationInfo.HideCostDetails = clientVariationInfo.HideCostDetails;
            childClientVariationInfo.Comments = clientVariationInfo.Comments;
            childClientVariationInfo.Variation = clientVariationInfo.Variation;

            childClientVariationInfo.Items = new List<ClientVariationItemInfo>();
            childClientVariationInfo.Trades = new List<ClientVariationTradeInfo>();

            try
            {
                processInfo = ProcessController.GetInstance().GetDeepProcessTemplateForClientVariations(clientVariationInfo);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    childClientVariationInfo.Id = AddClientVariation(childClientVariationInfo, processInfo);

                    foreach (ClientVariationItemInfo clientVariationItemInfo in clientVariationInfo.Items)
                    {
                        childClientVariationItemInfo = new ClientVariationItemInfo();

                        childClientVariationItemInfo.ClientVariation = childClientVariationInfo;
                        childClientVariationItemInfo.Amount = clientVariationItemInfo.Amount;
                        childClientVariationItemInfo.Description = clientVariationItemInfo.Description;

                        childClientVariationItemInfo.Id = AddClientVariationItem(childClientVariationItemInfo);
                    }

                    foreach (ClientVariationTradeInfo clientVariationTradeInfo in clientVariationInfo.Trades)
                    {
                        childClientVariationTradeInfo = new ClientVariationTradeInfo();

                        childClientVariationTradeInfo.ClientVariation = childClientVariationInfo;
                        childClientVariationTradeInfo.TradeCode = clientVariationTradeInfo.TradeCode;
                        childClientVariationTradeInfo.Amount = clientVariationTradeInfo.Amount;

                        childClientVariationTradeInfo.Id = AddClientVariationTrade(childClientVariationTradeInfo);

                        contractsController.UpdateVariationClientVariationTrade(clientVariationTradeInfo.Id, childClientVariationTradeInfo.Id);
                        tradesController.UpdateTradeBudgetClientVariationTrade(clientVariationTradeInfo.Id, childClientVariationTradeInfo.Id);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Creating Client Variation Revision in database");
            }

            return childClientVariationInfo;
        }

        /// <summary>
        /// Returns a list of Client Variations with totals between the specifed dates.
        /// Used for the Separate Accounts report.
        /// </summary>
        public List<ClientVariationInfo> GetClientVariationsTotals(DateTime? startDate, DateTime? endDate, String type, String status)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            ClientVariationInfo clientVariationInfo;
            List<ClientVariationInfo> ClientVariationInfoList = new List<ClientVariationInfo>();
            String principalName = String.Empty;
            int numChildren;

            parameters.Add(startDate);
            parameters.Add(endDate);
            parameters.Add(type);

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationsByDate(parameters.ToArray());

                while (dr.Read())
                {
                    clientVariationInfo = GetClientVariationObject(type, Data.Utils.GetDBInt32(dr["ClientVariationId"]));
                    clientVariationInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
                    clientVariationInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    clientVariationInfo.InternalApprovalDate = Data.Utils.GetDBDateTime(dr["InternalApprovalDate"]);
                    clientVariationInfo.VerbalApprovalDate = Data.Utils.GetDBDateTime(dr["VerbalApprovalDate"]);
                    clientVariationInfo.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
                    clientVariationInfo.CancelDate = Data.Utils.GetDBDateTime(dr["CancelDate"]);
                    clientVariationInfo.WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);

                    if (clientVariationInfo is SeparateAccountInfo)
                    {
                        SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                        separateAccountInfo.InvoiceDate = Data.Utils.GetDBDateTime(dr["InvoiceDate"]);
                        separateAccountInfo.InvoiceSentDate = Data.Utils.GetDBDateTime(dr["InvoiceSentDate"]);
                        separateAccountInfo.WorksCompletedDate = Data.Utils.GetDBDateTime(dr["WorksCompletedDate"]);
                        separateAccountInfo.InvoicePaidDate = Data.Utils.GetDBDateTime(dr["InvoicePaidDate"]);
                        separateAccountInfo.UseSecondPrincipal = Data.Utils.GetDBBoolean(dr["UseSecondPrincipal"]);

                        principalName = separateAccountInfo.IsSecondPrincipal ? Data.Utils.GetDBString(dr["Principal2"]) : Data.Utils.GetDBString(dr["Principal"]);
                    }
                    else
                    {
                        principalName = Data.Utils.GetDBString(dr["Principal"]);
                    }

                    numChildren = (int)Data.Utils.GetDBInt32(dr["NumChildren"]) - 1;
                    if (numChildren > 0)
                        clientVariationInfo.RevisionName = ((Char)(FirstAsciiCode + numChildren)).ToString();

                    clientVariationInfo.Project = new ProjectInfo();
                    clientVariationInfo.Project.Number = Data.Utils.GetDBString(dr["ProjectNumber"]);
                    clientVariationInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    clientVariationInfo.Project.Year = Data.Utils.GetDBString(dr["Year"]);
                    clientVariationInfo.Project.Principal = principalName;

                    clientVariationInfo.Project.BusinessUnit = new BusinessUnitInfo();
                    clientVariationInfo.Project.BusinessUnit.ProjectNumberFormat = Data.Utils.GetDBString(dr["ProjectNumberFormat"]);

                    clientVariationInfo.Project.ProjectManager = new EmployeeInfo();
                    clientVariationInfo.Project.ProjectManager.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    clientVariationInfo.Project.ProjectManager.LastName = Data.Utils.GetDBString(dr["LastName"]);

                    clientVariationInfo.Items = new List<ClientVariationItemInfo>();
                    clientVariationInfo.Items.Add(new ClientVariationItemInfo());
                    clientVariationInfo.Items[0].Amount = Data.Utils.GetDBDecimal(dr["Amount"]);

                    ClientVariationInfoList.Add(clientVariationInfo);
                }

                if (status != null)
                {
                    List<ClientVariationInfo> ClientVariationInfoFilterList = new List<ClientVariationInfo>();

                    foreach (ClientVariationInfo clientVariation in ClientVariationInfoList)
                        if (clientVariation.Status.Equals(status))
                            ClientVariationInfoFilterList.Add(clientVariation);

                    ClientVariationInfoList = ClientVariationInfoFilterList;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Separate Accounts by date from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return ClientVariationInfoList;
        }
        #endregion


        #region Client Variation Items Methods
        /// <summary>
        /// Creates a Client Variation Item from a dr
        /// </summary>
        public ClientVariationItemInfo CreateClientVariationItem(IDataReader dr)
        {
            ClientVariationItemInfo clientVariationItemInfo = new ClientVariationItemInfo(Data.Utils.GetDBInt32(dr["ClientVariationDetailId"]));

            clientVariationItemInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            clientVariationItemInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            clientVariationItemInfo.DisplayOrder = Data.Utils.GetDBInt32(dr["DisplayOrder"]);//-----San----

            AssignAuditInfo(clientVariationItemInfo, dr);

            if (dr["ClientVariationId"] != DBNull.Value)
            {
                clientVariationItemInfo.ClientVariation = GetClientVariationObject(Data.Utils.GetDBString(dr["ClientVariationType"]), Data.Utils.GetDBInt32(dr["ClientVariationId"]));
                clientVariationItemInfo.ClientVariation.Name = Data.Utils.GetDBString(dr["ClientVariationName"]);

                if (dr["ParentClientVariationId"] != DBNull.Value)
                    clientVariationItemInfo.ClientVariation.ParentClientVariation = GetClientVariationObject(Data.Utils.GetDBString(dr["ClientVariationType"]), Data.Utils.GetDBInt32(dr["ParentClientVariationId"]));

                if (dr["ProjectId"] != DBNull.Value)
                {
                    clientVariationItemInfo.ClientVariation.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    clientVariationItemInfo.ClientVariation.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                }
            }

            return clientVariationItemInfo;
        }

        /// <summary>
        /// Get a Client Variation Item from persistent storage
        /// </summary>
        public ClientVariationItemInfo GetClientVariationItem(int? clientVariationItemId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationItem(clientVariationItemId);
                if (dr.Read())
                    return CreateClientVariationItem(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation Item from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Client Variation Items for the specified Client Variation
        /// </summary>
        public List<ClientVariationItemInfo> GetClientVariationItems(ClientVariationInfo clientVariationInfo)
        {
            IDataReader dr = null;
            List<ClientVariationItemInfo> clientVariationItemInfoList = new List<ClientVariationItemInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationItemsByClientVariation(clientVariationInfo.Id);
                while (dr.Read())
                    clientVariationItemInfoList.Add(CreateClientVariationItem(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation Items from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientVariationItemInfoList;
        }

        /// <summary>
        /// Updates a Client Variation Item in the database
        /// </summary>
        public void UpdateClientVariationItem(ClientVariationItemInfo clientVariationItemInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(clientVariationItemInfo);

            parameters.Add(clientVariationItemInfo.Id);
            parameters.Add(clientVariationItemInfo.Amount);
            parameters.Add(clientVariationItemInfo.Description);

            parameters.Add(clientVariationItemInfo.ModifiedDate);
            parameters.Add(clientVariationItemInfo.ModifiedBy);
            parameters.Add(clientVariationItemInfo.DisplayOrder);//---San---

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationItem(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Item in database");
            }
            //
            // UPDATE CVSUM VISIBILITY
            //
            UpdateSkipCVSUM(clientVariationItemInfo);   // ds20231125
        }
        public void UpdateSkipCVSUM(ClientVariationItemInfo clientVariationItemInfo)
        {
            List<Object> parameters = new List<Object>();
            parameters.Add(clientVariationItemInfo.ClientVariation.Id);
            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationSkipCVSUM(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Item in database");
            }

        }
        /// <summary>
        /// Adds a Client Variation Item to the database
        /// </summary>
        public int? AddClientVariationItem(ClientVariationItemInfo clientVariationItemInfo)
        {
            int? clientVariationItemId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(clientVariationItemInfo);

            parameters.Add(GetId(clientVariationItemInfo.ClientVariation));
            parameters.Add(clientVariationItemInfo.Amount);
            parameters.Add(clientVariationItemInfo.Description);

            parameters.Add(clientVariationItemInfo.CreatedDate);
            parameters.Add(clientVariationItemInfo.CreatedBy);

            try
            {
                clientVariationItemId = Data.DataProvider.GetInstance().AddClientVariationItem(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Client Variation Item to database");
            }
            UpdateSkipCVSUM(clientVariationItemInfo);   // ds20231125
            return clientVariationItemId;
        }

        /// <summary>
        /// Adds or updates a Client Variation Item
        /// </summary>
        public int? AddUpdateClientVariationItem(ClientVariationItemInfo clientVariationItemInfo)
        {
            if (clientVariationItemInfo != null)
            {
                if (clientVariationItemInfo.Id != null)
                {
                    UpdateClientVariationItem(clientVariationItemInfo);
                    return clientVariationItemInfo.Id;
                }
                else
                {
                    return AddClientVariationItem(clientVariationItemInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Client Variation Item from persistent storage
        /// </summary>
        public void DeleteClientVariationItem(ClientVariationItemInfo clientVariationItemInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteClientVariationItem(clientVariationItemInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Client Variation Item from database");
            }
            UpdateSkipCVSUM(clientVariationItemInfo);   // ds20231125
        }


        //-----San----------------------------06022o23-----

        public void ChangeClientVariationItemDisplayOrder(ClientVariationItemInfo clientVariationItemInfo, bool moveUp)
        {
            try
            {

                clientVariationItemInfo = GetClientVariationItem(clientVariationItemInfo.Id);

                List<ClientVariationItemInfo> lstClientVariationItem = GetClientVariationItems(clientVariationItemInfo.ClientVariation);


                if (lstClientVariationItem.Count > 0)
                {
                    lstClientVariationItem.OrderBy(a => a.DisplayOrder);
                }
                //List<ISortable> iSortableList = new List<ISortable>();

                //foreach (ClientVariationItemInfo cvItem in lstClientVariationItem)
                //    iSortableList.Add((ISortable)cvItem);



                ClientVariationItemInfo modifiedCvItemInfo = (ClientVariationItemInfo)ChangeDisplayOrderCvItem(lstClientVariationItem, clientVariationItemInfo, moveUp);
                if (modifiedCvItemInfo != null)
                {
                    try
                    {
                        //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                        //{
                        UpdateClientVariationItem(clientVariationItemInfo);
                        UpdateClientVariationItem(modifiedCvItemInfo);

                        //    scope.Complete();
                        //}
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex.ToString());
                        throw new Exception("Changing CV Item Display Order");
                    }
                }






            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Changing CV Item Display Order");
            }
        }





        public ClientVariationItemInfo ChangeDisplayOrderCvItem(List<ClientVariationItemInfo> sortableList, ClientVariationItemInfo sortable, bool moveUp)
        {
            ClientVariationItemInfo modyObject = null;
            int? tmp;
            int currIndex = sortableList.FindIndex(a => a.Id == sortable.Id);

            if (currIndex != -1)
            {
                if (moveUp)
                {
                    if (currIndex != 0)
                        modyObject = sortableList[currIndex - 1];
                }
                else
                {
                    if (currIndex < sortableList.Count - 1)
                        modyObject = sortableList[currIndex + 1];
                }

                if (modyObject != null)
                {
                    tmp = modyObject.DisplayOrder;
                    modyObject.DisplayOrder = sortable.DisplayOrder;
                    sortable.DisplayOrder = tmp;
                }
            }

            return modyObject;
        }



        //-----San----------------------------06022o23-----


        /// <summary>
        /// returns a string corresponding to a revision name given the revision number: A, B, C, etc.
        /// </summary>
        /// <param name="revisionNumber"></param>
        /// <returns></returns>
        public String RevisionName(int? revisionNumber)
        {
            if (revisionNumber.HasValue)
            {
                int asciiCodeNumber = FirstAsciiCode + revisionNumber.Value - 1;
                return asciiCodeNumber <= LastAsciiCode ? ((Char)(asciiCodeNumber)).ToString() : "R" + (revisionNumber.ToString());
            }
            else
            {
                return "?";
            }
        }

        /// <summary>
        /// Set the revisions names for a Client Variation and its children
        /// </summary>
        public void SetRevisionNames(ClientVariationInfo clientVariationInfo)
        {
            if (clientVariationInfo.TheParentClientVariation.SubClientVariations == null || clientVariationInfo.TheParentClientVariation.SubClientVariations.Count == 0)
                clientVariationInfo.TheParentClientVariation.RevisionName = null;
            else
            {
                clientVariationInfo.TheParentClientVariation.RevisionName = String.Empty;

                int i = FirstAsciiCode;
                foreach (ClientVariationInfo subClientVariationInfo in clientVariationInfo.TheParentClientVariation.SubClientVariations)
                {
                    if (i <= LastAsciiCode)
                        subClientVariationInfo.RevisionName = ((Char)(i)).ToString();
                    else
                        subClientVariationInfo.RevisionName = "R" + (i.ToString());

                    if (subClientVariationInfo.Equals(clientVariationInfo))
                        clientVariationInfo.RevisionName = subClientVariationInfo.RevisionName;

                    i++;
                }
            }
        }
        #endregion


        #region Client Variation Trades Methods
        /// <summary>
        /// Creates a Client Variation Trade from a dr
        /// </summary>
        public ClientVariationTradeInfo CreateClientVariationTrade(IDataReader dr)
        {
            ClientVariationTradeInfo clientVariationTradeInfo = new ClientVariationTradeInfo(Data.Utils.GetDBInt32(dr["ClientVariationTradeId"]));

            clientVariationTradeInfo.TradeCode = Data.Utils.GetDBString(dr["TradeCode"]);
            clientVariationTradeInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);

            AssignAuditInfo(clientVariationTradeInfo, dr);

            if (dr["ClientVariationId"] != DBNull.Value)
            {
                clientVariationTradeInfo.ClientVariation = GetClientVariationObject(Data.Utils.GetDBString(dr["ClientVariationType"]), Data.Utils.GetDBInt32(dr["ClientVariationId"]));
                clientVariationTradeInfo.ClientVariation.Name = Data.Utils.GetDBString(dr["ClientVariationName"]);

                if (dr["ProjectId"] != DBNull.Value)
                {
                    clientVariationTradeInfo.ClientVariation.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    clientVariationTradeInfo.ClientVariation.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                }
            }

            return clientVariationTradeInfo;
        }

        /// <summary>
        /// Get a Client Variation Trade from persistent storage
        /// </summary>
        public ClientVariationTradeInfo GetClientVariationTrade(int? clientVariationTradeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationTrade(clientVariationTradeId);
                if (dr.Read())
                    return CreateClientVariationTrade(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation Trade from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Client Variation Trades for the specified Client Variation
        /// </summary>
        public List<ClientVariationTradeInfo> GetClientVariationTrades(ClientVariationInfo clientVariationInfo)
        {
            IDataReader dr = null;
            List<ClientVariationTradeInfo> clientVariationTradeInfoList = new List<ClientVariationTradeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClientVariationTradesByClientVariation(clientVariationInfo.Id);
                while (dr.Read())
                    clientVariationTradeInfoList.Add(CreateClientVariationTrade(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return clientVariationTradeInfoList;
        }

        /// <summary>
        /// Updates a Client Variation Trade in the database
        /// </summary>
        public void UpdateClientVariationTrade(ClientVariationTradeInfo clientVariationTradeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(clientVariationTradeInfo);

            parameters.Add(clientVariationTradeInfo.Id);
            parameters.Add(clientVariationTradeInfo.TradeCode);
            parameters.Add(clientVariationTradeInfo.Amount);

            parameters.Add(clientVariationTradeInfo.ModifiedDate);
            parameters.Add(clientVariationTradeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateClientVariationTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Client Variation Trade in database");
            }
        }

        /// <summary>
        /// Adds a Client Variation Trade to the database
        /// </summary>
        public int? AddClientVariationTrade(ClientVariationTradeInfo clientVariationTradeInfo)
        {
            int? clientVariationTradeId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(clientVariationTradeInfo);

            parameters.Add(GetId(clientVariationTradeInfo.ClientVariation));
            parameters.Add(clientVariationTradeInfo.TradeCode);
            parameters.Add(clientVariationTradeInfo.Amount);

            parameters.Add(clientVariationTradeInfo.CreatedDate);
            parameters.Add(clientVariationTradeInfo.CreatedBy);

            try
            {
                clientVariationTradeId = Data.DataProvider.GetInstance().AddClientVariationTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Client Variation Trade to database");
            }

            return clientVariationTradeId;
        }

        /// <summary>
        /// Adds or updates a Client Variation Trade
        /// </summary>
        public int? AddUpdateClientVariationTrade(ClientVariationTradeInfo clientVariationTradeInfo)
        {
            if (clientVariationTradeInfo != null)
            {
                if (clientVariationTradeInfo.Id != null)
                {
                    UpdateClientVariationTrade(clientVariationTradeInfo);
                    return clientVariationTradeInfo.Id;
                }
                else
                {
                    return AddClientVariationTrade(clientVariationTradeInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Client Variation Trade from persistent storage
        /// </summary>
        public void DeleteClientVariationTrade(ClientVariationTradeInfo clientVariationTradeInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteClientVariationTrade(clientVariationTradeInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Client Variation Trade from database");
            }
        }

        /// <summary>
        /// Generates the Client Variation Report
        /// </summary>
        public Byte[] GenerateClientVariationReport(ClientVariationInfo clientVariationInfo)
        {
            String SignaturePM = null;
            String SignatureCM = null;
            LocalReport localReport = new LocalReport();
            List<ReportParameter> reportParameters = new List<ReportParameter>();

            if (clientVariationInfo.Project.ProjectManager != null)
                if (clientVariationInfo.Project.ProjectManager.Signature != null)
                    SignaturePM = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/" + UI.Utils.SetFormString(clientVariationInfo.Project.ProjectManager.Signature);

            if (clientVariationInfo.Project.ConstructionManager != null)
                if (clientVariationInfo.Project.ConstructionManager.Signature != null)
                    SignatureCM = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/" + UI.Utils.SetFormString(clientVariationInfo.Project.ConstructionManager.Signature);

            reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(clientVariationInfo.Project.Name)));
            reportParameters.Add(new ReportParameter("ProjectNo", UI.Utils.SetFormString(clientVariationInfo.Project.FullNumber)));
            reportParameters.Add(new ReportParameter("ClientVariationNo", UI.Utils.SetFormString(clientVariationInfo.NumberAndRevisionName)));
            reportParameters.Add(new ReportParameter("IssueDate", UI.Utils.SetFormDate(clientVariationInfo.WriteDate)));
            reportParameters.Add(new ReportParameter("PM_Name", UI.Utils.SetFormString(clientVariationInfo.Project.ProjectManagerName)));
            reportParameters.Add(new ReportParameter("PM_Signature", SignaturePM));
            reportParameters.Add(new ReportParameter("CM_Name", UI.Utils.SetFormString(clientVariationInfo.Project.ConstructionManagerName)));
            reportParameters.Add(new ReportParameter("CM_Signature", SignatureCM));
            reportParameters.Add(new ReportParameter("HideCostDetails", UI.Utils.SetFormYesNo(clientVariationInfo.HideCostDetails)));
            reportParameters.Add(new ReportParameter("ClientVariationName", UI.Utils.SetFormString(clientVariationInfo.Name)));
            reportParameters.Add(new ReportParameter("TotalAmount", UI.Utils.SetFormEditDecimal(clientVariationInfo.TotalAmount)));
            reportParameters.Add(new ReportParameter("TotalGST", UI.Utils.SetFormEditDecimal(clientVariationInfo.TotalGST)));
            reportParameters.Add(new ReportParameter("TotalClientVariation", UI.Utils.SetFormEditDecimal(clientVariationInfo.TotalAmountPlusGST)));

            if (clientVariationInfo is SeparateAccountInfo)
            {
                if (clientVariationInfo.Project.BusinessUnitName == "QLD2")
                {
                    localReport.ReportPath = Web.Utils.ReportsPath + "\\SeparateAccountQLD.rdlc";
                }

                else
                {
                    localReport.ReportPath = Web.Utils.ReportsPath + "\\SeparateAccount.rdlc";
                }

            }

            else
            {
                if (clientVariationInfo.Project.BusinessUnitName == "QLD2")
                {
                    localReport.ReportPath = Web.Utils.ReportsPath + "\\ClientVariationQLD.rdlc";
                }

                else
                {
                    localReport.ReportPath = Web.Utils.ReportsPath + "\\ClientVariation.rdlc";
                }
            }

            localReport.DataSources.Add(new ReportDataSource("ClientVariationItemInfo", clientVariationInfo.Items));
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating Client Variation Report");
            }
        }

        /// <summary>
        /// Generates the Separate Account Invoice Report
        /// </summary>
        public Byte[] GenerateSeparateAccountInvoiceReport(SeparateAccountInfo separateAccountInfo)
        {
            LocalReport localReport = new LocalReport();
            List<ReportParameter> reportParameters = new List<ReportParameter>();

            reportParameters.Add(new ReportParameter("InvoiceDate", UI.Utils.SetFormDate(separateAccountInfo.InvoiceDate)));
            reportParameters.Add(new ReportParameter("InvoiceNumber", UI.Utils.SetFormEditInteger(separateAccountInfo.InvoiceNumber)));
            reportParameters.Add(new ReportParameter("PrincipalABN", UI.Utils.SetFormString(separateAccountInfo.PrincipalABN)));
            reportParameters.Add(new ReportParameter("PrincipalName", UI.Utils.SetFormString(separateAccountInfo.Principal)));
            //#--- 
            reportParameters.Add(new ReportParameter("ClientStreet", UI.Utils.SetFormString(separateAccountInfo.ContactStreet)));
            reportParameters.Add(new ReportParameter("ClientLocality", UI.Utils.SetFormString(separateAccountInfo.ContactLocality)));
            reportParameters.Add(new ReportParameter("ClientState", UI.Utils.SetFormString(separateAccountInfo.ContactState)));
            reportParameters.Add(new ReportParameter("ClientPostalCode", UI.Utils.SetFormString(separateAccountInfo.ContactPostalCode)));
            reportParameters.Add(new ReportParameter("ContactName", UI.Utils.SetFormString(separateAccountInfo.ContactName)));
            //#--
            reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(separateAccountInfo.Project.Name)));
            reportParameters.Add(new ReportParameter("ProjectNo", UI.Utils.SetFormString(separateAccountInfo.Project.FullNumber)));
            reportParameters.Add(new ReportParameter("Name", UI.Utils.SetFormString(separateAccountInfo.Name)));
            reportParameters.Add(new ReportParameter("Number", UI.Utils.SetFormString(separateAccountInfo.NumberAndRevisionName)));
            reportParameters.Add(new ReportParameter("Cost", UI.Utils.SetFormEditDecimal(separateAccountInfo.TotalAmount)));
            reportParameters.Add(new ReportParameter("GstPercentage", UI.Utils.SetFormPercentage(separateAccountInfo.GoodsServicesTax)));
            reportParameters.Add(new ReportParameter("GstAmount", UI.Utils.SetFormEditDecimal(separateAccountInfo.TotalGST)));
            reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(separateAccountInfo.TotalAmountPlusGST)));
            reportParameters.Add(new ReportParameter("DueDate", UI.Utils.SetFormDate(separateAccountInfo.InvoiceDueDate)));


            //#---
            //if (separateAccountInfo.Project.ClientContactList != null)
            //{
            //    foreach (ClientContactInfo clientContact in separateAccountInfo.Project.ClientContactList)
            //    {
            //        if (clientContact.AttentionToClaims != null)
            //            if ((Boolean)clientContact.AttentionToClaims)
            //            {                              
            //                reportParameters.Add(new ReportParameter("ClientStreet", UI.Utils.SetFormString(clientContact.Street)));
            //                reportParameters.Add(new ReportParameter("ClientLocality", UI.Utils.SetFormString(clientContact.Locality)));
            //                reportParameters.Add(new ReportParameter("ClientState", UI.Utils.SetFormString(clientContact.State)));
            //                reportParameters.Add(new ReportParameter("ClientPostalCode", UI.Utils.SetFormString(clientContact.PostalCode)));
            //                reportParameters.Add(new ReportParameter("ContactName", UI.Utils.SetFormString(clientContact.Name)));
            //                break;
            //            }
            //    }
            //}

            //#---




            //# --To get special note from BusinessUnit

            reportParameters.Add(new ReportParameter("SpecialNote", UI.Utils.SetFormString(separateAccountInfo.Project.BusinessUnit.ClaimSpecialNote)));
            //---#

            //#--To set Title Tax invoice or Adjustment note for Seperate Account Invoice report--#

            if (separateAccountInfo.TotalAmount.HasValue && separateAccountInfo.TotalAmount.Value > 0)
                reportParameters.Add(new ReportParameter("TitleInfo", "Tax Invoice"));
            else
                reportParameters.Add(new ReportParameter("TitleInfo", "Adjustment Note"));

            //#---

            localReport.ReportPath = Web.Utils.ReportsPath + "\\SeparateAccountInvoice.rdlc";

            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating Client Variation Invoice Report");
            }
        }

        /// <summary>
        /// Generate the Claim Report
        /// </summary>
        public Byte[] GenerateClaimReport(ClaimInfo claimInfo, ClaimInfo previousClaim)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            LocalReport localReport = new LocalReport();
            String SignatureMD = null;
            String previousClaimsNumbers = null;
            Decimal? finalAccount = null;
            Decimal? totalGST = null;
            Decimal? totalClaim = null;
            Decimal? totalClientTrades = 0;
            Decimal? totalClientVariations = 0;
            String warrantyInfo = null;
            String adjustmentInfo = null;

            Decimal? totalCurrent = claimInfo.Total;
            Decimal? totalPrevious = previousClaim != null ? previousClaim.Total : null;
            Decimal? totalProject = claimInfo.Project.ContractAmountPlusVariations;
            DateTime? experiationDateMaintenancePeriod = claimInfo.Project.ExperiationDateMaintenancePeriod;

            if (claimInfo.Project.ManagingDirector != null)
                if (claimInfo.Project.ManagingDirector.Signature != null)
                    SignatureMD = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/" + UI.Utils.SetFormString(claimInfo.Project.ManagingDirector.Signature);

            if (claimInfo.AdjustmentNoteName != null)
            {
                adjustmentInfo = ", Adj. " + claimInfo.AdjustmentNoteName;

                if (claimInfo.AdjustmentNoteAmount != null)
                    if (totalPrevious != null)
                        totalPrevious = (Decimal)totalPrevious.Value - (Decimal)claimInfo.AdjustmentNoteAmount;
                    else
                        totalPrevious = -(Decimal)claimInfo.AdjustmentNoteAmount;
            }

            if (totalCurrent != null)
                if (totalPrevious != null)
                    finalAccount = (Decimal)totalCurrent - (Decimal)totalPrevious;
                else
                    finalAccount = (Decimal)totalCurrent;

            if (finalAccount != null && claimInfo.GoodsServicesTax != null)
                totalGST = Math.Round((Decimal)((Decimal)finalAccount * (Decimal)claimInfo.GoodsServicesTax), 2);

            if (finalAccount != null && totalGST != null)
                totalClaim = (Decimal)finalAccount + (Decimal)totalGST;

            if (claimInfo.Number != null)
                if ((Int32)claimInfo.Number > 1)
                    if ((Int32)claimInfo.Number == 2)
                        previousClaimsNumbers = " 1";
                    else if ((Int32)claimInfo.Number == 3)
                        previousClaimsNumbers = " 1, 2";
                    else
                        previousClaimsNumbers = " 1 - " + UI.Utils.SetFormInteger((claimInfo.Number - 1));

            if (claimInfo.Project.ClientTrades != null)
                foreach (ClientTradeInfo clientTrade in claimInfo.Project.ClientTrades)
                {
                    ClaimTradeInfo claimTradeInfo = claimInfo.ClaimTrades.Find(delegate (ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });
                    if (claimTradeInfo != null)
                        if (clientTrade.Amount != null)
                            totalClientTrades += (Decimal)clientTrade.Amount;
                }

            if (claimInfo.Project.ClientVariations != null)
                foreach (ClientVariationInfo clientVariation in claimInfo.Project.ClientVariations)
                {
                    ClaimVariationInfo claimVariationInfo = claimInfo.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.TheParentClientVariation.Equals(clientVariation.TheParentClientVariation); });
                    if (claimVariationInfo != null)
                        if (clientVariation.TotalAmount != null)
                        {
                            if (claimVariationInfo.ClientVariation.Status.ToUpper() == "TO BE APPROVED" || claimVariationInfo.ClientVariation.Status.ToUpper() == "APPROVED" || claimVariationInfo.ClientVariation.Status.ToUpper() == "VERBALLY APPROVED" || claimVariationInfo.ClientVariation.Status.ToUpper() == "SUBMITTED")  //DS20230829
                            {                                                                                                                                                                                                                                                                  //  if (claimVariationInfo.ClientVariation.Status.ToUpper() != "TO BE ISSUED")  //DS20230829
                                if (claimVariationInfo.ClientVariation.Status.ToUpper() != "CANCELLED")  //DS20230829                                                                                                                                                                                                  //if (claimVariationInfo.ClientVariation.Status.ToUpper() != "TO BE ISSUED") 
                                {
                                    totalClientVariations += (Decimal)clientVariation.TotalAmount;
                                }
                            }
                        }
                }

            // Totals
            reportParameters.Add(new ReportParameter("ContractAmount", UI.Utils.SetFormDecimal(totalClientTrades)));
            reportParameters.Add(new ReportParameter("TotalTrades", UI.Utils.SetFormDecimal(totalClientTrades)));
            reportParameters.Add(new ReportParameter("TotalVariations", UI.Utils.SetFormDecimal(totalClientVariations)));
            reportParameters.Add(new ReportParameter("ClaimTrades", UI.Utils.SetFormDecimal(claimInfo.TotalClaimTrades)));
            reportParameters.Add(new ReportParameter("ClaimVariations", UI.Utils.SetFormDecimal(claimInfo.TotalClaimVariations)));
            reportParameters.Add(new ReportParameter("RevisedContractSum", UI.Utils.SetFormDecimal(totalClientTrades + totalClientVariations)));

            reportParameters.Add(new ReportParameter("ThisClaim", UI.Utils.SetFormDecimal(finalAccount)));
            reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormDecimal(totalGST)));
            reportParameters.Add(new ReportParameter("ThisClaimWithGST", UI.Utils.SetFormDecimal(totalClaim)));

            reportParameters.Add(new ReportParameter("PreviousClaimsNumbers", UI.Utils.SetFormString(previousClaimsNumbers)));
            reportParameters.Add(new ReportParameter("TotalPreviousClaim", UI.Utils.SetFormDecimal(totalPrevious)));
            reportParameters.Add(new ReportParameter("AdjustmentInfo", UI.Utils.SetFormString(adjustmentInfo)));

            reportParameters.Add(new ReportParameter("Principal", UI.Utils.SetFormString(claimInfo.Project.Principal)));
            reportParameters.Add(new ReportParameter("PrincipalABN", UI.Utils.SetFormString(claimInfo.Project.PrincipalABN)));


            //#------To get Attention client contact and Superintendent, surveyor
            #region

            //reportParameters.Add(new ReportParameter("ClientFax", UI.Utils.SetFormString(claimInfo.Project.ClientContact.Fax)));
            //reportParameters.Add(new ReportParameter("ClientStreet", UI.Utils.SetFormString(claimInfo.Project.ClientContact.Street)));
            //reportParameters.Add(new ReportParameter("ClientLocality", UI.Utils.SetFormString(claimInfo.Project.ClientContact.Locality)));
            //reportParameters.Add(new ReportParameter("ClientState", UI.Utils.SetFormString(claimInfo.Project.ClientContact.State)));
            //reportParameters.Add(new ReportParameter("ClientPostalCode", UI.Utils.SetFormString(claimInfo.Project.ClientContact.PostalCode)));




            //reportParameters.Add(new ReportParameter("ClientName", UI.Utils.SetFormString(claimInfo.Project.ClientContact.Name)));

            //reportParameters.Add(new ReportParameter("QtySurveyorName", UI.Utils.SetFormString(claimInfo.Project.QuantitySurveyor.Name)));
            //reportParameters.Add(new ReportParameter("QtySurveyorCompany", UI.Utils.SetFormString(claimInfo.Project.QuantitySurveyor.CompanyName)));
            //reportParameters.Add(new ReportParameter("QtySurveyorPhone", UI.Utils.SetFormString(claimInfo.Project.QuantitySurveyor.Phone)));

            //reportParameters.Add(new ReportParameter("SuperintendentName", UI.Utils.SetFormString(claimInfo.Project.Superintendent.Name)));
            //reportParameters.Add(new ReportParameter("SuperintendentCompany", UI.Utils.SetFormString(claimInfo.Project.Superintendent.CompanyName)));
            //reportParameters.Add(new ReportParameter("SuperintendentPhone", UI.Utils.SetFormString(claimInfo.Project.Superintendent.Phone)));
            #endregion
            //#---

            string clientName = "";
            if (claimInfo.Project.ClientContactList != null)
            {
                foreach (ClientContactInfo clientContact in claimInfo.Project.ClientContactList)
                {
                    if (clientContact.AttentionToClaims != null)
                        if ((Boolean)clientContact.AttentionToClaims)
                        {
                            clientName = UI.Utils.SetFormString(clientContact.Name);

                            reportParameters.Add(new ReportParameter("ClientFax", UI.Utils.SetFormString(clientContact.Fax)));

                            reportParameters.Add(new ReportParameter("ClientEmail", UI.Utils.SetFormString(clientContact.Email)));


                            reportParameters.Add(new ReportParameter("ClientStreet", UI.Utils.SetFormString(clientContact.Street)));
                            reportParameters.Add(new ReportParameter("ClientLocality", UI.Utils.SetFormString(clientContact.Locality)));
                            reportParameters.Add(new ReportParameter("ClientState", UI.Utils.SetFormString(clientContact.State)));
                            reportParameters.Add(new ReportParameter("ClientPostalCode", UI.Utils.SetFormString(clientContact.PostalCode)));

                        }
                    if (clientContact.Position == "Superintendent")
                    {
                        reportParameters.Add(new ReportParameter("SuperintendentName", UI.Utils.SetFormString(clientContact.Name)));
                        reportParameters.Add(new ReportParameter("SuperintendentCompany", UI.Utils.SetFormString(clientContact.CompanyName)));
                        reportParameters.Add(new ReportParameter("SuperintendentPhone", UI.Utils.SetFormString(clientContact.Phone)));
                    }
                    if (clientContact.Position == "Surveyor")
                    {
                        reportParameters.Add(new ReportParameter("QtySurveyorName", UI.Utils.SetFormString(clientContact.Name)));
                        reportParameters.Add(new ReportParameter("QtySurveyorCompany", UI.Utils.SetFormString(clientContact.CompanyName)));
                        reportParameters.Add(new ReportParameter("QtySurveyorPhone", UI.Utils.SetFormString(clientContact.Phone)));
                    }
                }

                reportParameters.Add(new ReportParameter("ClientName", UI.Utils.SetFormString(clientName)));

            }
            //#---



            // General Info
            reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(claimInfo.Project.Name)));
            reportParameters.Add(new ReportParameter("JobNo", UI.Utils.SetFormString(claimInfo.Project.FullNumber)));
            reportParameters.Add(new ReportParameter("ClaimNo", UI.Utils.SetFormInteger(claimInfo.Number)));
            reportParameters.Add(new ReportParameter("IssueDate", UI.Utils.SetFormDate(claimInfo.DueDate)));
            reportParameters.Add(new ReportParameter("ClientDueDate", UI.Utils.SetFormDate(claimInfo.ClientDueDate)));
            reportParameters.Add(new ReportParameter("PracticalCompletionDate", UI.Utils.SetFormDate(claimInfo.Project.PracticalCompletionDate)));
            reportParameters.Add(new ReportParameter("MaintenanceDate", UI.Utils.SetFormDate(experiationDateMaintenancePeriod)));

            //#-- Hardcoded as per Alex 

            reportParameters.Add(new ReportParameter("MD_Name", "Andrew Noble")); //#---UI.Utils.SetFormString(claimInfo.Project.ManagingDirector.Name)));
            reportParameters.Add(new ReportParameter("MD_Position", "Managing Director")); //#--UI.Utils.SetFormString(claimInfo.Project.ManagingDirector.Position)));
            SignatureMD = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/ANoble.gif";//#-- + UI.Utils.SetFormString(claimInfo.Project.ManagingDirector.Signature);
            reportParameters.Add(new ReportParameter("MD_Signature", SignatureMD));

            //#-- Hardcoded as per Alex 

            reportParameters.Add(new ReportParameter("ClaimTypeName", claimInfo.IsApproved ? "Final" : "Draft"));
            reportParameters.Add(new ReportParameter("SpecialNote", UI.Utils.SetFormString(claimInfo.Project.BusinessUnit.ClaimSpecialNote)));

            // Determine if is progress claim or final claim or Final Payment Claim
            if (totalCurrent != null && totalProject != null && (Decimal)totalCurrent == (Decimal)totalProject)
            {

                if (claimInfo.Project.Waranty1Amount != null &&
                            claimInfo.Project.Waranty1Date != null &&
                            experiationDateMaintenancePeriod != null &&
                            claimInfo.Project.Waranty1Date > claimInfo.Project.PracticalCompletionDate)
                {
                    warrantyInfo = "Would you please return the first Bank Guarantee for an amount of " +
                                   UI.Utils.SetFormDecimal(claimInfo.Project.Waranty1Amount) + ". " +
                                   "The second Bank Guarantee for an amount of " +
                                   UI.Utils.SetFormDecimal(claimInfo.Project.Waranty2Amount) + " " +
                                   "is due to be returned to us at the end of the maintenance period, which is the " +
                                   UI.Utils.SetFormDate(experiationDateMaintenancePeriod) + ".";
                }

                reportParameters.Add(new ReportParameter("WarrantyInfo", UI.Utils.SetFormString(warrantyInfo)));

                //#---12/03/24
                localReport.ReportPath = Web.Utils.ReportsPath + "\\FinalClaim.rdlc";

                //if (claimInfo.Project.BusinessUnitName == "QLD2")
                //{
                //    reportParameters.Add(new ReportParameter("AccountName", UI.Utils.SetFormString(claimInfo.Project.AccountName)));
                //    reportParameters.Add(new ReportParameter("BSB", UI.Utils.SetFormString(claimInfo.Project.BSB)));
                //    reportParameters.Add(new ReportParameter("AccountNumber", UI.Utils.SetFormString(claimInfo.Project.AccountNumber)));
                //    localReport.ReportPath = Web.Utils.ReportsPath + "\\FinalClaimQLD2.rdlc";

                //}
                //else
                //{
                //    localReport.ReportPath = Web.Utils.ReportsPath + "\\FinalClaim.rdlc";
                //}
                //#--12/03/24




                //#--- for FinalPayment claim

                if (claimInfo.Process.Steps.Count > 5)
                {

                    if (claimInfo.Process.Steps[6].Status == "Approved")
                    {
                        reportParameters.Add(new ReportParameter("Comments", UI.Utils.SetFormString(claimInfo.Process.Steps[6].Comments)));
                        reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(claimInfo.Project.FullNumber)));
                        reportParameters.Add(new ReportParameter("ClaimsNumbers", UI.Utils.SetFormString("1 - " + claimInfo.Number.ToString())));
                        reportParameters.Add(new ReportParameter("TotalPreviousClaimFinayPayment", UI.Utils.SetFormDecimal(claimInfo.Total)));

                        //#--12/03/24
                        //if(claimInfo.Project.BusinessUnitName=="QLD2")
                        //{
                        //    reportParameters.Add(new ReportParameter("AccountName", UI.Utils.SetFormString(claimInfo.Project.AccountName)));
                        //    reportParameters.Add(new ReportParameter("BSB", UI.Utils.SetFormString(claimInfo.Project.BSB)));
                        //    reportParameters.Add(new ReportParameter("AccountNumber", UI.Utils.SetFormString(claimInfo.Project.AccountNumber)));
                        //    localReport.ReportPath = Web.Utils.ReportsPath + "\\FinalPaymentClaimQLD.rdlc";

                        //}
                        //else  //#--12/03/24

                            localReport.ReportPath = Web.Utils.ReportsPath + "\\FinalPaymentClaim.rdlc";
                    }


                }
                //#-----


            }
            else
            {
                reportParameters.Add(new ReportParameter("TotalTradesAndVariations", UI.Utils.SetFormDecimal(totalCurrent)));

                // #-----8/11/2018   -- To display 2more columns---  percentage and value completed for month----

                if (claimInfo.Project.BusinessUnitName == "QLD2")//---14/12/2021
                {
                    reportParameters.Add(new ReportParameter("AccountName", UI.Utils.SetFormString(claimInfo.Project.AccountName)));
                    reportParameters.Add(new ReportParameter("BSB", UI.Utils.SetFormString(claimInfo.Project.BSB)));
                    reportParameters.Add(new ReportParameter("AccountNumber", UI.Utils.SetFormString(claimInfo.Project.AccountNumber)));

                    localReport.ReportPath = Web.Utils.ReportsPath + "\\ClaimQLD.rdlc";
                }
                else
                    localReport.ReportPath = Web.Utils.ReportsPath + "\\ClaimCopy.rdlc";// Claim.rdlc";



                #region ClaimTrade
                DataTable DtTrade = new DataTable();
                DtTrade.Columns.Add("Trade");
                DtTrade.Columns.Add("TradeTotal");
                DtTrade.Columns.Add("Completed");
                DtTrade.Columns.Add("Claimed");
                DtTrade.Columns.Add("MonthlyCompleted");
                DtTrade.Columns.Add("MonthlyClaimed");
                DataRow Dr;
                ClaimTradeInfo previousCtrade = null;
                ClaimTradeInfo claimTrade = null;
                decimal prviousPercentage = 0, prviousAmount = 0;
                //foreach (ClaimTradeInfo claimTrade in claimInfo.ClaimTrades)
                //{
                foreach (ClientTradeInfo clientTrade in claimInfo.Project.ClientTrades)
                {
                    claimTrade = claimInfo.ClaimTrades.Find(delegate (ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

                    if (previousClaim != null)
                        previousCtrade = previousClaim.ClaimTrades.Find(delegate (ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

                    if (previousCtrade != null || claimTrade != null)
                    {
                        Dr = DtTrade.NewRow();
                        if (claimTrade.ClientTradeName != null)  //#---11-02-2019
                            Dr["Trade"] = claimTrade.ClientTradeName.ToString();

                        if (claimTrade.ClientTradeAmount != null)  //#---11-02-2019
                            Dr["TradeTotal"] = claimTrade.ClientTradeAmount.Value;

                        if (claimTrade.ClientTradePercentaje != null)  //#---11-02-2019
                            Dr["Completed"] = claimTrade.ClientTradePercentaje.Value;

                        if (claimTrade.Amount != null)  //#---11-02-2019
                            Dr["Claimed"] = claimTrade.Amount.Value;

                        // ClaimTradeInfo previousCtrade = previousClaim.ClaimTrades.Find(x => x.ClientTrade.Id == claimTrade.ClientTrade.Id);

                        if (previousCtrade != null)
                            if (previousCtrade.Amount != null && claimTrade.ClientTradeAmount != null)
                            {
                                if (claimTrade.ClientTradeAmount.Value > 0)
                                    prviousPercentage = (previousCtrade.Amount.Value / claimTrade.ClientTradeAmount.Value);
                                prviousAmount = previousCtrade.Amount.Value;

                            }

                        Dr["MonthlyCompleted"] = (claimTrade.ClientTradePercentaje != null ? (claimTrade.ClientTradePercentaje.Value) : 0) - prviousPercentage;
                        Dr["MonthlyClaimed"] = (claimTrade.Amount != null ? claimTrade.Amount.Value : 0) - prviousAmount;

                        DtTrade.Rows.Add(Dr);

                        prviousPercentage = 0; prviousAmount = 0;
                    }
                }
                #endregion

                #region ClaimVariation

                DataTable DtClaimVariation = new DataTable();
                DtClaimVariation.Columns.Add("ClientVariationNumber");
                DtClaimVariation.Columns.Add("ClientVariationName");
                DtClaimVariation.Columns.Add("ClientVariationPercentaje");
                DtClaimVariation.Columns.Add("ClientVariationAmount");
                DtClaimVariation.Columns.Add("ClientVariationNumberAndRevisionName");
                DtClaimVariation.Columns.Add("Amount");
                DtClaimVariation.Columns.Add("MonthlyCompleted");
                DtClaimVariation.Columns.Add("MonthlyClaimed");
                DataRow Drow;
                decimal prviousPercentageA = 0, prviousAmountA = 0;
                ClaimVariationInfo previousClaimVariationInfo = null;
                ClaimVariationInfo claimVariationInfo = null;
                //List<ClaimVariationInfo> ClaimVariationInfoList = new List<ClaimVariationInfo>();   //DS20230829
                try
                {
                    foreach (ClientVariationInfo clientVariation in claimInfo.Project.ClientVariations)
                    {
                        if (previousClaim != null)
                            previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation); });

                        if (previousClaimVariationInfo == null && clientVariation.ParentClientVariation != null && previousClaim != null)
                        {
                            previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation.ParentClientVariation); });
                        }

                        claimVariationInfo = claimInfo.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.TheParentClientVariation.Equals(clientVariation.TheParentClientVariation); });

                        if (previousClaimVariationInfo != null || claimVariationInfo != null)
                        {
                            if (claimVariationInfo.ClientVariation.Status.ToUpper() != "CANCELLED")  //DS20230913                                                                                                                                                                                                  //if (claimVariationInfo.ClientVariation.Status.ToUpper() != "TO BE ISSUED") 
                            {
                                Drow = DtClaimVariation.NewRow();

                                Drow["ClientVariationNumber"] = claimVariationInfo.ClientVariationNumber != null ? claimVariationInfo.ClientVariationNumber.Value : 0;
                                Drow["ClientVariationName"] = claimVariationInfo.ClientVariationName.ToString();
                                Drow["ClientVariationNumberAndRevisionName"] = claimVariationInfo.ClientVariationNumberAndRevisionName.ToString();
                                //Drow["ClientVariationAmount"] = claimVariationInfo.ClientVariationAmount != null ? claimVariationInfo.ClientVariationAmount.Value : 0;
                                Drow["ClientVariationPercentaje"] = claimVariationInfo.ClientVariationPercentaje != null ? claimVariationInfo.ClientVariationPercentaje.Value : 0;
                                //“Approved-”, “To be approved-”, “Verbally Approved-”,  “Submitted-”- or “Cancelled” 
                                if (claimVariationInfo.ClientVariation.Status.ToUpper() == "APPROVED" || claimVariationInfo.ClientVariation.Status.ToUpper() == "TO BE APPROVED" || claimVariationInfo.ClientVariation.Status.ToUpper() == "VERBALLY APPROVED" || claimVariationInfo.ClientVariation.Status.ToUpper() == "SUBMITTED")  //DS20230829
                                                                                                                                                                                                                                                                                                                                       //                                if (claimVariationInfo.ClientVariation.Status.ToUpper() != "TO BE ISSUED")  //DS20230829
                                {
                                    decimal decAmount = claimVariationInfo.ClientVariationAmount != null ? claimVariationInfo.ClientVariationAmount.Value : 0; ;
                                    string strAmount = "$" + decAmount.ToString("###,##0.00");
                                    Drow["ClientVariationAmount"] = strAmount;

                                    decAmount = claimVariationInfo.Amount != null ? claimVariationInfo.Amount.Value : 0;
                                    strAmount = "$" + decAmount.ToString("###,##0.00");
                                    Drow["Amount"] = strAmount;
                                    //Drow["Amount"] = claimVariationInfo.Amount != null ? claimVariationInfo.Amount.Value : 0;
                                }
                                else

                                {
                                    Drow["ClientVariationAmount"] = "TBC";
                                    Drow["Amount"] = "$0.00";
                                }

                                if (previousClaimVariationInfo != null)
                                    if (previousClaimVariationInfo.Amount != null && claimVariationInfo.ClientVariationAmount != null)
                                    {
                                        if (claimVariationInfo.ClientVariationAmount.Value > 0)
                                            prviousPercentageA = (previousClaimVariationInfo.Amount.Value / claimVariationInfo.ClientVariationAmount.Value);
                                        prviousAmountA = previousClaimVariationInfo.Amount.Value;

                                    }

                                Drow["MonthlyCompleted"] = (claimVariationInfo.ClientVariationPercentaje != null ? (claimVariationInfo.ClientVariationPercentaje.Value) : 0) - prviousPercentageA;
                                Drow["MonthlyClaimed"] = (claimVariationInfo.Amount != null ? claimVariationInfo.Amount.Value : 0) - prviousAmountA;
                                DtClaimVariation.Rows.Add(Drow);
                                // ClaimVariationInfoList.Add(claimVariationInfo);   //DS20230829
                            }
                            prviousPercentageA = 0; prviousAmountA = 0;
                        }
                    }
                }
                catch (Exception ex)
                {

                    string X = ex.ToString();
                }

                #endregion

                localReport.DataSources.Add(new ReportDataSource("MonthlyClaimVariationInfo", DtClaimVariation));
                localReport.DataSources.Add(new ReportDataSource("MonthlyClaimTradeInfo", DtTrade));

                //#--- 8/11/2018
            }

            try
            {
                localReport.DataSources.Add(new ReportDataSource("ClaimVariationInfo", claimInfo.ClaimVariations));       //DS20230829
                // localReport.DataSources.Add(new ReportDataSource("ClaimVariationInfo", ClaimVariationInfoList));       //DS20230829
                localReport.DataSources.Add(new ReportDataSource("ClaimTradeInfo", claimInfo.ClaimTrades));
            }
            catch (Exception ex)
            {
                string X = ex.ToString();
            }



            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating Claim Report");
            }
        }

        /// <summary>
        /// Generate the RFI Report
        /// </summary>
        public Byte[] GenerateRFIReport(RFIInfo rFIInfo)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            LocalReport localReport = new LocalReport();
            String signature = null;
            String signer = null;

            if (rFIInfo.Signer != null)
            {
                signer = rFIInfo.Signer.Name;

                if (rFIInfo.Signer.Signature != null)
                    signature = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/" + UI.Utils.SetFormString(rFIInfo.Signer.Signature);
            }

            if (rFIInfo.Project != null)
            {
                reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(rFIInfo.Project.FullNumber)));
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(rFIInfo.Project.Name)));
                reportParameters.Add(new ReportParameter("ProjectPrincipal", UI.Utils.SetFormString(rFIInfo.Project.Principal)));
                reportParameters.Add(new ReportParameter("RFINumber", UI.Utils.SetFormInteger(rFIInfo.Number)));
                reportParameters.Add(new ReportParameter("DateRaised", UI.Utils.SetFormDate(rFIInfo.RaiseDate)));
                reportParameters.Add(new ReportParameter("TargetAnswerDate", UI.Utils.SetFormDate(rFIInfo.TargetAnswerDate)));
                reportParameters.Add(new ReportParameter("Subject", UI.Utils.SetFormString(rFIInfo.Subject)));
                reportParameters.Add(new ReportParameter("Description", UI.Utils.SetFormString(rFIInfo.Description)));
                reportParameters.Add(new ReportParameter("Signer", UI.Utils.SetFormString(signer)));
                reportParameters.Add(new ReportParameter("Signature", UI.Utils.SetFormString(signature)));

                //#----
                //if (rFIInfo.Project.ClientContact != null)
                //    reportParameters.Add(new ReportParameter("ClientContactName", UI.Utils.SetFormString(rFIInfo.Project.ClientContact.Name)));

                if (rFIInfo.Project.ClientContactList != null)
                {
                    foreach (ClientContactInfo clientContact in rFIInfo.Project.ClientContactList)
                    {
                        if ((Boolean)clientContact.AttentionToRFIs)
                        {
                            reportParameters.Add(new ReportParameter("ClientContactName", UI.Utils.SetFormString(clientContact.Name)));
                        }

                    }

                }
                //#---


            }
            if (rFIInfo.Project.BusinessUnitName == "QLD2")//---14/12/2021
                localReport.ReportPath = Web.Utils.ReportsPath + "\\RFIQLD.rdlc";
            else
                localReport.ReportPath = Web.Utils.ReportsPath + "\\RFI.rdlc";
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating RFI Report");
            }
        }



        //#----
        public Byte[] GenerateRFIReportWithResponses(RFIInfo rFIInfo)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            LocalReport localReport = new LocalReport();
            String signature = null;
            String signer = null;

            if (rFIInfo.Signer != null)
            {
                signer = rFIInfo.Signer.Name;

                if (rFIInfo.Signer.Signature != null)
                    signature = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images/Signatures/" + UI.Utils.SetFormString(rFIInfo.Signer.Signature);
            }

            if (rFIInfo.Project != null)
            {
                reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(rFIInfo.Project.FullNumber)));
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(rFIInfo.Project.Name)));
                reportParameters.Add(new ReportParameter("ProjectPrincipal", UI.Utils.SetFormString(rFIInfo.Project.Principal)));
                reportParameters.Add(new ReportParameter("RFINumber", UI.Utils.SetFormInteger(rFIInfo.Number)));
                reportParameters.Add(new ReportParameter("DateRaised", UI.Utils.SetFormDate(rFIInfo.RaiseDate)));
                reportParameters.Add(new ReportParameter("TargetAnswerDate", UI.Utils.SetFormDate(rFIInfo.TargetAnswerDate)));
                reportParameters.Add(new ReportParameter("Subject", UI.Utils.SetFormString(rFIInfo.Subject)));
                reportParameters.Add(new ReportParameter("Description", UI.Utils.SetFormString(rFIInfo.Description)));
                reportParameters.Add(new ReportParameter("Signer", UI.Utils.SetFormString(signer)));
                reportParameters.Add(new ReportParameter("Signature", UI.Utils.SetFormString(signature)));

                //#----
                //if (rFIInfo.Project.ClientContact != null)
                //    reportParameters.Add(new ReportParameter("ClientContactName", UI.Utils.SetFormString(rFIInfo.Project.ClientContact.Name)));

                if (rFIInfo.Project.ClientContactList != null)
                {
                    foreach (ClientContactInfo clientContact in rFIInfo.Project.ClientContactList)
                    {
                        if ((Boolean)clientContact.AttentionToRFIs)
                        {
                            reportParameters.Add(new ReportParameter("ClientContactName", UI.Utils.SetFormString(clientContact.Name)));
                        }

                    }

                }
                //#---


            }
            localReport.DataSources.Add(new ReportDataSource("RFIResponses", GetRFIsWithResponse(rFIInfo)));
            //if(rFIInfo.Project.BusinessUnitName==)
            localReport.ReportPath = Web.Utils.ReportsPath + "\\RFIwithResponses.rdlc";
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating RFI with Responses Report");
            }
        }

        //#---

        /// <summary>
        /// Generate the EOT Report
        /// </summary>
        public Byte[] GenerateEOTReport(EOTInfo eOTInfo)
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            LocalReport localReport = new LocalReport();
            String title = null;
            String DetailsDelay = null;
            //#--------
            //if (eOTInfo.IsClaim)
            //    title = "Notice of Delay and Claim for Extension of Time";
            //else
            //{ title = "Notice of Delay";

            if (eOTInfo.DaysClaimed != null || eOTInfo.DaysClaimed > 0)  // #--if (eOTInfo.IsClaim) --22082019
            {
                title = "Notice of Delay and Claim for Extension of Time";
                DetailsDelay = "Pursuant to the terms of the head contract, we hereby provide notification of a delay to the contract works and make claim for an Extension of time for Practical Completion, per the following:";
            }
            else
            {
                title = "Notice of Delay";
                DetailsDelay = "Pursuant to the terms of the head contract, we hereby provide notification of a delay to the contract works, per the following:";

            }

            //#----
            reportParameters.Add(new ReportParameter("Title", UI.Utils.SetFormString(title)));
            reportParameters.Add(new ReportParameter("DetailsDelay", UI.Utils.SetFormString(DetailsDelay)));
            reportParameters.Add(new ReportParameter("EOTNumber", UI.Utils.SetFormInteger(eOTInfo.Number)));
            reportParameters.Add(new ReportParameter("CostCode", UI.Utils.SetFormString(eOTInfo.CostCode)));
            reportParameters.Add(new ReportParameter("FirstNoticeDate", UI.Utils.SetFormDate(eOTInfo.FirstNoticeDate)));
            reportParameters.Add(new ReportParameter("WriteDate", UI.Utils.SetFormDate(eOTInfo.WriteDate)));
            reportParameters.Add(new ReportParameter("Cause", UI.Utils.SetFormString(eOTInfo.Cause)));
            reportParameters.Add(new ReportParameter("Nature", UI.Utils.SetFormString(eOTInfo.Nature)));
            reportParameters.Add(new ReportParameter("Period", UI.Utils.SetFormString(eOTInfo.Period)));
            reportParameters.Add(new ReportParameter("Works", UI.Utils.SetFormString(eOTInfo.Works)));

            if (eOTInfo.DaysClaimed != null)
                reportParameters.Add(new ReportParameter("Extension", UI.Utils.SetFormFloat(eOTInfo.DaysClaimed) + " days"));
            else
                reportParameters.Add(new ReportParameter("Extension", UI.Utils.SetFormString("TBA")));


            //#--22/08/2019 ---EOT 
            if (eOTInfo.SendDate != null)
                reportParameters.Add(new ReportParameter("DateofIssue", UI.Utils.SetFormDate(eOTInfo.SendDate)));
            //#--22/08/2019 ---EOT 



            if (eOTInfo.Project != null)
            {
                reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(eOTInfo.Project.FullNumber)));
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(eOTInfo.Project.Name)));
                reportParameters.Add(new ReportParameter("ProjectPrincipal", UI.Utils.SetFormString(eOTInfo.Project.Principal)));
                reportParameters.Add(new ReportParameter("Distribution", UI.Utils.SetFormString(eOTInfo.Project.DistributionListEOTInitials)));


                //#---- To add attention To EOT from Clientcontact list

                //if (eOTInfo.Project.ClientContact != null)
                //{
                //    reportParameters.Add(new ReportParameter("Fax", UI.Utils.SetFormString(eOTInfo.Project.ClientContact.Fax)));
                //    reportParameters.Add(new ReportParameter("Attention", UI.Utils.SetFormString(eOTInfo.Project.ClientContact.Name)));
                //}


                if (eOTInfo.Project.ClientContactList != null)
                {
                    foreach (ClientContactInfo clientContact in eOTInfo.Project.ClientContactList)
                    {
                        if ((Boolean)clientContact.AttentionToEots)
                        {
                            reportParameters.Add(new ReportParameter("Attention", UI.Utils.SetFormString(clientContact.Name)));
                            reportParameters.Add(new ReportParameter("Fax", UI.Utils.SetFormString(clientContact.Fax)));
                            reportParameters.Add(new ReportParameter("Email", UI.Utils.SetFormString(clientContact.Email))); //#---
                            break;
                        }
                    }
                }
                //#---



            }

            if (eOTInfo.Project.BusinessUnitName == "QLD2")//---14/12/2021
                localReport.ReportPath = Web.Utils.ReportsPath + "\\EOTQLD.rdlc";
            else
                localReport.ReportPath = Web.Utils.ReportsPath + "\\EOT.rdlc";

            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Generating EOT Report");
            }
        }
        #endregion


        #region Claims Methods
        /// <summary>
        /// Sets initial state of a new claim
        /// </summary>
        public ClaimInfo InitializeClaim(ProjectInfo projectInfo, ClaimInfo previousClaim)
        {
            ClaimInfo claimInfo;
            ClaimTradeInfo claimTradeInfo;
            ClaimTradeInfo previousClaimTradeInfo;
            ClaimVariationInfo claimVariationInfo;
            ClaimVariationInfo previousClaimVariationInfo;
            DateTime firstClaimDueDate;
            DateTime startDate;
            DateTime tmpDate;

            claimInfo = new ClaimInfo();
            claimInfo.Number = projectInfo.Claims != null ? projectInfo.Claims.Count + 1 : 1;
            claimInfo.WriteDate = DateTime.Today;
            claimInfo.GoodsServicesTax = Decimal.Parse(Web.Utils.GetConfigListItemValue("Global", "Settings", "GST"));
            claimInfo.ClaimTrades = new List<ClaimTradeInfo>();
            claimInfo.ClaimVariations = new List<ClaimVariationInfo>();
            claimInfo.Project = projectInfo;

            if (claimInfo.Project.FirstClaimDueDate != null & claimInfo.Project.ClaimFrequency != null)
            {
                firstClaimDueDate = (DateTime)claimInfo.Project.FirstClaimDueDate;
                startDate = new DateTime(firstClaimDueDate.Year, firstClaimDueDate.Month, 1);

                if (claimInfo.Project.ClaimFrequency == ProjectInfo.Monthly)
                    if (firstClaimDueDate.Day <= 15)
                        claimInfo.DueDate = Utils.NextMonthlySequenceThe15(startDate, (int)claimInfo.Number);
                    else
                        claimInfo.DueDate = Utils.NextMonthlySequenceLastDay(startDate, (int)claimInfo.Number);
                else
                    if (firstClaimDueDate.Day <= 15)
                    claimInfo.DueDate = (int)claimInfo.Number % 2 == 0 ? Utils.NextSemiMonthlySequenceLastDay(startDate, (int)claimInfo.Number) : Utils.NextSemiMonthlySequenceThe15(startDate, (int)claimInfo.Number);
                else
                    claimInfo.DueDate = (int)claimInfo.Number % 2 == 0 ? Utils.NextSemiMonthlySequenceThe15(startDate, (int)claimInfo.Number) : Utils.NextSemiMonthlySequenceLastDay(startDate, (int)claimInfo.Number);
            }

            if (claimInfo.DueDate != null && claimInfo.Project.PaymentTerms != null)
                if (claimInfo.Project.PaymentTerms == ProjectInfo.Monthly)
                    if (((DateTime)claimInfo.DueDate).Day == 15)
                        claimInfo.ClientDueDate = ((DateTime)claimInfo.DueDate).AddMonths(1);
                    else
                    {
                        tmpDate = (DateTime)claimInfo.DueDate;
                        claimInfo.ClientDueDate = new DateTime(tmpDate.Year, tmpDate.Month, 1).AddMonths(2).AddDays(-1);
                    }
                else
                    if (((DateTime)claimInfo.DueDate).Day == 15)
                {
                    tmpDate = (DateTime)claimInfo.DueDate;
                    claimInfo.ClientDueDate = new DateTime(tmpDate.Year, tmpDate.Month, 1).AddMonths(1).AddDays(-1);
                }
                else
                {
                    tmpDate = ((DateTime)claimInfo.DueDate).AddMonths(1);
                    claimInfo.ClientDueDate = new DateTime(tmpDate.Year, tmpDate.Month, 15);
                }

            if (projectInfo.ClientTrades != null)
                foreach (ClientTradeInfo clientTradeInfo in claimInfo.Project.ClientTrades)
                {
                    claimTradeInfo = new ClaimTradeInfo();
                    claimTradeInfo.Claim = claimInfo;
                    claimTradeInfo.ClientTrade = new ClientTradeInfo(clientTradeInfo.Id);

                    if (previousClaim != null)
                    {
                        previousClaimTradeInfo = previousClaim.ClaimTrades.Find(delegate (ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTradeInfo); });

                        if (previousClaimTradeInfo != null)
                            claimTradeInfo.Amount = previousClaimTradeInfo.Amount;
                        else
                            claimTradeInfo.Amount = 0;
                    }
                    else
                        claimTradeInfo.Amount = 0;

                    claimInfo.ClaimTrades.Add(claimTradeInfo);
                }

            if (claimInfo.Project.ClientVariations != null)
                foreach (ClientVariationInfo clientVariationInfo in claimInfo.Project.ClientVariations)
                {
                    claimVariationInfo = new ClaimVariationInfo();
                    claimVariationInfo.Claim = claimInfo;
                    claimVariationInfo.ClientVariation = new ClientVariationInfo(clientVariationInfo.Id);
                    claimVariationInfo.ClientVariation.ParentClientVariation = clientVariationInfo.ParentClientVariation;

                    if (previousClaim != null)
                    {
                        previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariationInfo); });


                        //#--21/06/2016------to get Prvious claim amount from ParentClientVariationId-//-------
                        if (previousClaimVariationInfo == null && clientVariationInfo.ParentClientVariation != null)
                            previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariationInfo.ParentClientVariation); });
                        //#--------------

                        if (previousClaimVariationInfo != null)
                            claimVariationInfo.Amount = previousClaimVariationInfo.Amount;
                        else
                            claimVariationInfo.Amount = 0;
                    }
                    else
                        claimVariationInfo.Amount = 0;

                    claimInfo.ClaimVariations.Add(claimVariationInfo);
                }

            return claimInfo;
        }

        /// <summary>
        /// Creates a Claim from a dr
        /// </summary>
        public ClaimInfo CreateClaim(IDataReader dr)
        {
            ClaimInfo claimInfo = new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"]));

            claimInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
            claimInfo.WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);
            claimInfo.DraftApprovalDate = Data.Utils.GetDBDateTime(dr["DraftApprovalDate"]);
            claimInfo.InternalApprovalDate = Data.Utils.GetDBDateTime(dr["InternalApprovalDate"]);
            claimInfo.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
            claimInfo.DueDate = Data.Utils.GetDBDateTime(dr["DueDate"]);
            claimInfo.ClientDueDate = Data.Utils.GetDBDateTime(dr["ClientDueDate"]);
            claimInfo.GoodsServicesTax = Data.Utils.GetDBDecimal(dr["GoodsServicesTax"]);
            claimInfo.AdjustmentNoteAmount = Data.Utils.GetDBDecimal(dr["AdjustmentNoteAmount"]);
            claimInfo.AdjustmentNoteName = Data.Utils.GetDBString(dr["AdjustmentNoteName"]);
            claimInfo.BackupFile1 = Data.Utils.GetDBString(dr["BackupFile1"]);  // DS20231023
            claimInfo.BackupFile2 = Data.Utils.GetDBString(dr["BackupFile2"]);  // DS20231023

            AssignAuditInfo(claimInfo, dr);

            if (dr["ProjectId"] != DBNull.Value)
            {
                claimInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                claimInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
            }

            if (dr["ProcessId"] != DBNull.Value) claimInfo.Process = ProcessController.GetInstance().GetDeepProcess(Data.Utils.GetDBInt32(dr["ProcessId"]));

            return claimInfo;
        }

        /// <summary>
        /// Get a Claim from persistent storage
        /// </summary>
        public ClaimInfo GetClaim(int? claimId)

        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClaim(claimId);
                if (dr.Read())
                    return CreateClaim(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Progress Claim from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Claims for the specified Project
        /// </summary>
        public List<ClaimInfo> GetClaims(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<ClaimInfo> claimInfoList = new List<ClaimInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClaimsByProject(projectInfo.Id);
                while (dr.Read())
                    claimInfoList.Add(CreateClaim(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Progress Claims from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return claimInfoList;
        }

        /// <summary>
        /// Get the Claims deep for the specified Project
        /// </summary>
        public List<ClaimInfo> GetClaimsWithTradesAndVariations(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            ClaimInfo claimInfo;
            List<ClaimInfo> claimInfoList = new List<ClaimInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClaimsByProject(projectInfo.Id);
                while (dr.Read())
                {
                    claimInfo = CreateClaim(dr);
                    claimInfo.ClaimTrades = GetClaimTrades(claimInfo);
                    claimInfo.ClaimVariations = GetClaimVariations(claimInfo);
                    claimInfoList.Add(claimInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Progress Claims with Trades and Variations from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return claimInfoList;
        }

        /// <summary>
        /// Updates a Claim in the database
        /// </summary>
        public void UpdateClaim(ClaimInfo claimInfo, ClaimInfo originalClaimInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(claimInfo);

            parameters.Add(claimInfo.Id);
            parameters.Add(claimInfo.WriteDate);
            parameters.Add(claimInfo.DueDate);
            parameters.Add(claimInfo.ClientDueDate);
            parameters.Add(claimInfo.GoodsServicesTax);
            parameters.Add(claimInfo.AdjustmentNoteAmount);
            parameters.Add(claimInfo.AdjustmentNoteName);

            parameters.Add(claimInfo.ModifiedDate);
            parameters.Add(claimInfo.ModifiedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateClaim(parameters.ToArray());

                    if ((claimInfo.Process != null) && (
                            (originalClaimInfo.DueDate == null && claimInfo.DueDate != null) ||
                            (originalClaimInfo.DueDate != null && claimInfo.DueDate == null) ||
                            (originalClaimInfo.DueDate != null && claimInfo.DueDate != null && originalClaimInfo.DueDate != claimInfo.DueDate)
                        ))
                    {
                        InitializeHolidays();
                        ProcessController.GetInstance().UpdateProcessDates(this, claimInfo.Process, claimInfo.DueDate);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim in database");
            }
        }

        /// <summary>
        /// Updates a claim including claim trades and claim variations
        /// </summary>
        public void UdateClaimWithTradesAndVariations(ClaimInfo claimInfo)
        {
            ClaimInfo originalClaim = new ClaimInfo();
            Dictionary<Int32, ClaimTradeInfo> dictionaryOriginalClaimTrades = new Dictionary<Int32, ClaimTradeInfo>();
            Dictionary<Int32, ClaimVariationInfo> dictionaryOriginalClaimVariations = new Dictionary<Int32, ClaimVariationInfo>();
            Dictionary<Int32, ClaimTradeInfo> dictionaryNewClaimTrades = new Dictionary<Int32, ClaimTradeInfo>();
            Dictionary<Int32, ClaimVariationInfo> dictionaryNewClaimVariations = new Dictionary<Int32, ClaimVariationInfo>();

            foreach (ClaimTradeInfo claimTrade in claimInfo.ClaimTrades)
                dictionaryNewClaimTrades.Add((Int32)claimTrade.ClientTrade.Id, claimTrade);

            foreach (ClaimVariationInfo claimVariation in claimInfo.ClaimVariations)
                dictionaryNewClaimVariations.Add((Int32)claimVariation.ClientVariation.Id, claimVariation);

            try
            {
                originalClaim = GetClaimWithTradesAndVariations(claimInfo.Id);

                foreach (ClaimTradeInfo claimTrade in originalClaim.ClaimTrades)
                    dictionaryOriginalClaimTrades.Add((Int32)claimTrade.ClientTrade.Id, claimTrade);

                foreach (ClaimVariationInfo claimVariation in originalClaim.ClaimVariations)
                    dictionaryOriginalClaimVariations.Add((Int32)claimVariation.ClientVariation.TheParentClientVariation.Id, claimVariation);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    UpdateClaim(claimInfo, originalClaim);

                    foreach (ClaimTradeInfo claimTrade in claimInfo.ClaimTrades)
                        if (dictionaryOriginalClaimTrades.ContainsKey((Int32)claimTrade.ClientTrade.Id))
                            UpdateClaimTrade(claimTrade);
                        else
                            AddClaimTrade(claimTrade);

                    foreach (ClaimTradeInfo claimTrade in originalClaim.ClaimTrades)
                        if (!dictionaryNewClaimTrades.ContainsKey((Int32)claimTrade.ClientTrade.Id))
                            DeleteClaimTrade(claimTrade);

                    foreach (ClaimVariationInfo claimVariation in claimInfo.ClaimVariations)
                        if (dictionaryOriginalClaimVariations.ContainsKey((Int32)claimVariation.ClientVariation.TheParentClientVariation.Id))
                            UpdateClaimVariation(claimVariation);
                        else
                            AddClaimVariation(claimVariation);

                    foreach (ClaimVariationInfo claimVariation in originalClaim.ClaimVariations)
                        if (!dictionaryNewClaimVariations.ContainsKey((Int32)claimVariation.ClientVariation.TheParentClientVariation.Id))
                            DeleteClaimVariation(claimVariation);

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating claim with trades an variations in the database");
            }
        }

        /// <summary>
        /// Adds a claim including claim trades and claim variations
        /// </summary>
        public int? AddClaimWithTradesAndVariations(ClaimInfo claimInfo)
        {
            int? claimId = null;
            ProcessInfo processInfo;

            try
            {
                processInfo = ProcessController.GetInstance().GetDeepProcessTemplateForClaims();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    claimInfo.Id = AddClaim(claimInfo, processInfo);

                    foreach (ClaimTradeInfo claimTrade in claimInfo.ClaimTrades)
                        AddClaimTrade(claimTrade);

                    foreach (ClaimVariationInfo claimVariation in claimInfo.ClaimVariations)
                        AddClaimVariation(claimVariation);

                    scope.Complete();

                    claimId = claimInfo.Id;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding claim with trades an variations in the database");
            }

            return claimId;
        }

        /// <summary>
        /// Updates a Claim's Draft Approval in the database
        /// </summary>
        public void UpdateClaimDraftApproval(ClaimInfo claimInfo)
        {
            List<Object> parameters = new List<Object>();

            if (claimInfo.DraftApprovalDate != null)
                throw new Exception("The Draft Approval Date has already been set");
            else
            {
                claimInfo.DraftApprovalDate = DateTime.Today;

                parameters.Add(claimInfo.Id);
                parameters.Add(claimInfo.DraftApprovalDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClaimDraftApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim Draft Approval Date in database");
            }
        }

        /// <summary>
        /// Updates a Claim's Internal Approval in the database
        /// </summary>
        public void UpdateClaimInternalApproval(ClaimInfo claimInfo)
        {
            List<Object> parameters = new List<Object>();

            if (claimInfo.InternalApprovalDate != null)
                throw new Exception("The Internal Approval Date has already been set");
            else
            {
                claimInfo.InternalApprovalDate = DateTime.Today;

                parameters.Add(claimInfo.Id);
                parameters.Add(claimInfo.InternalApprovalDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClaimInternalApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim Internal Approval Date in database");
            }
        }

        /// <summary>
        /// Updates a Claim's Approval in the database
        /// </summary>
        public void UpdateClaimApproval(ClaimInfo claimInfo)
        {
            List<Object> parameters = new List<Object>();

            if (claimInfo.ApprovalDate != null)
                throw new Exception("The Approval Date has already been set");
            else
            {
                claimInfo.ApprovalDate = DateTime.Today;

                parameters.Add(claimInfo.Id);
                parameters.Add(claimInfo.ApprovalDate);
            }

            try
            {
                Data.DataProvider.GetInstance().UpdateClaimApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim Approval Date in database");
            }
        }
        public void UpdateClaimBackupFiles(ClaimInfo claimInfo)
        {
            List<Object> parameters = new List<Object>();

            claimInfo.ApprovalDate = DateTime.Today;

            parameters.Add(claimInfo.Id);
            parameters.Add(claimInfo.BackupFile1);
            parameters.Add(claimInfo.BackupFile2);

            try
            {
                Data.DataProvider.GetInstance().UpdateClaimBackupFiles(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim Approval Date in database");
            }
        }
        /// <summary>
        /// Adds a Claim to the database. Claim must have a project
        /// </summary>
        private int? AddClaim(ClaimInfo claimInfo, ProcessInfo processInfo)
        {
            int? claimId = null;
            ProcessController processController = ProcessController.GetInstance();
            List<Object> parameters = new List<Object>();

            SetCreateInfo(claimInfo);

            try
            {
                InitializeHolidays();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (processInfo != null)
                    {
                        processInfo.Id = processController.AddProcess(processInfo);

                        foreach (ProcessStepInfo processStepInfo in processInfo.Steps)
                            processStepInfo.Id = processController.AddProcessStep(processStepInfo);

                        if (claimInfo.DueDate != null)
                            processController.UpdateProcessDates(this, processInfo, claimInfo.DueDate);

                        claimInfo.Process = processInfo;
                    }

                    parameters.Add(GetId(claimInfo.Project));
                    parameters.Add(GetId(claimInfo.Process));
                    parameters.Add(claimInfo.Number);
                    parameters.Add(claimInfo.WriteDate);
                    parameters.Add(claimInfo.DueDate);
                    parameters.Add(claimInfo.ClientDueDate);
                    parameters.Add(claimInfo.GoodsServicesTax);
                    parameters.Add(claimInfo.AdjustmentNoteAmount);
                    parameters.Add(claimInfo.AdjustmentNoteName);

                    parameters.Add(claimInfo.CreatedDate);
                    parameters.Add(claimInfo.CreatedBy);

                    claimId = Data.DataProvider.GetInstance().AddClaim(parameters.ToArray());

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Progress Claim to database");
            }

            return claimId;
        }

        /// <summary>
        /// Adds or updates a Claim
        /// </summary>
        public int? AddUpdateClaim(ClaimInfo claimInfo)
        {
            if (claimInfo != null)
            {
                if (claimInfo.Id != null)
                {
                    UdateClaimWithTradesAndVariations(claimInfo);
                    return claimInfo.Id;
                }
                else
                    return AddClaimWithTradesAndVariations(claimInfo);
            }
            else
                return null;
        }

        /// <summary>
        /// Remove a Claim from persistent storage
        /// </summary>
        public void DeleteClaim(ClaimInfo claimInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().DeleteClaim(claimInfo.Id);

                    if (claimInfo.Process != null)
                        ProcessController.GetInstance().DeleteProcess(claimInfo.Process);

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Progress Claim from database");
            }
        }

        /// <summary>
        /// Get a Claim with Trades and Variations from persistent storage
        /// </summary>
        public ClaimInfo GetClaimWithTradesAndVariations(int? claimId)
        {
            ClaimInfo claimInfo = GetClaim(claimId);

            claimInfo.ClaimTrades = GetClaimTrades(claimInfo);
            claimInfo.ClaimVariations = GetClaimVariations(claimInfo);

            return claimInfo;
        }






        //#----


        ///<Summary>
        ///To Add FInal PaymentClaim's process steps 
        ///<Summary>

        public int? AddFinalPaymentProcessSteps(ClaimInfo claiminfo)
        {
            DateTime Practicaldate = DateTime.Now;
            double Defactsliability = 0;
            int ID = 0;

            if (claiminfo.Project.PracticalCompletionDate != null)
                Practicaldate = claiminfo.Project.PracticalCompletionDate.Value;

            if (claiminfo.Project.DefectsLiability != null)
                Defactsliability = double.Parse(claiminfo.Project.DefectsLiability.Value.ToString());


            if (Practicaldate != null && Defactsliability > 0)
                Practicaldate = Practicaldate.AddDays(Defactsliability);
            try
            {

                ProcessController processController = ProcessController.GetInstance();
                ProcessStepInfo processStepInfo = new ProcessStepInfo();
                //---- CLFPP---PM - Final Payment Approval
                processStepInfo.Process = claiminfo.Process;
                processStepInfo.Type = ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByPM;
                processStepInfo.Name = "PM - Final Payment Approval";
                processStepInfo.Role = "PM";
                processStepInfo.NumDays = 1;
                processStepInfo.TargetDate = Practicaldate;
                processStepInfo.Comments = "You will be able to approve this on, or after the Target Date";
                processStepInfo.Id = processController.AddProcessStep(processStepInfo);

                //---- CLFPF---FC - Final Payment Approval

                processStepInfo = new ProcessStepInfo();
                processStepInfo.Process = claiminfo.Process;
                processStepInfo.Type = ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByFC;
                processStepInfo.Name = "FC - Final Payment Approval";
                processStepInfo.Role = "FC";
                processStepInfo.NumDays = 1;
                processStepInfo.TargetDate = Practicaldate;
                processStepInfo.Comments = "You will be able to approve this on, or after the Target Date";
                processStepInfo.Id = processController.AddProcessStep(processStepInfo);
                ID = processStepInfo.Id.Value;
            }

            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding ProgressSteps for Final Payment Claim to database");
            }

            return ID;

        }


        //#-------



        /// <summary>
        /// Return previous claim. ClaimInfo must have project with its claims. claimInfo can't be new
        /// </summary>
        public ClaimInfo GetPreviousClaim(ClaimInfo claimInfo)
        {
            ClaimInfo previousClaim = null;

            foreach (ClaimInfo claim in claimInfo.Project.Claims)
                if (claim.Equals(claimInfo))
                    break;
                else
                    previousClaim = claim;

            if (previousClaim != null)
                previousClaim = GetClaimWithTradesAndVariations(previousClaim.Id);

            return previousClaim;
        }

        /// <summary>
        /// Checks a Claim for errors and missing fields
        /// </summary>
        public XmlDocument CheckClaim(ClaimInfo claimInfo)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElement;

            xmlElementRoot = xmlDocument.CreateElement("root", null);
            xmlElementRoot.SetAttribute("name", "Claim Check");

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElement = xmlDocument.CreateElement("Fields", null);
            xmlElement.SetAttribute("name", "Missing Fields");

            Utils.AddMissingFieldNode(claimInfo.DueDate, xmlDocument, xmlElement, "Due date");
            Utils.AddMissingFieldNode(claimInfo.ClientDueDate, xmlDocument, xmlElement, "Client due date");
            Utils.AddMissingFieldNode(claimInfo.Project, xmlDocument, xmlElement, "Project");

            if (claimInfo.Project != null)
            {
                Utils.AddMissingFieldNode(claimInfo.Project.DefectsLiability, xmlDocument, xmlElement, "Defects liability days");

                if (claimInfo.Project.ClientTrades == null || claimInfo.Project.ClientTrades.Count == 0)
                    Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Client trades");

                if (claimInfo.Project.ManagingDirector == null)
                    Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Managing Director");
                else if (claimInfo.Project.ManagingDirector.Signature == null)
                    Utils.AddErrorMessageNode(xmlDocument, xmlElement, "Managing Director signature");

                Utils.AddMissingFieldNode(claimInfo.Project.ContractAmount, xmlDocument, xmlElement, "Contract amount");
                Utils.AddMissingFieldNode(claimInfo.Project.Name, xmlDocument, xmlElement, "Project name");
                Utils.AddMissingFieldNode(claimInfo.Project.Number, xmlDocument, xmlElement, "Project number");
                Utils.AddMissingFieldNode(claimInfo.Project.Principal, xmlDocument, xmlElement, "Project principal name");
                Utils.AddMissingFieldNode(claimInfo.Project.PrincipalABN, xmlDocument, xmlElement, "Project principal ABN");

                if (claimInfo.IsLastClaim)
                {
                    Decimal? totalLastClaim = claimInfo.Total;

                    if ((totalLastClaim != null && (Decimal)totalLastClaim >= (Decimal)claimInfo.Project.ContractAmountPlusVariations))
                    {
                        if (claimInfo.Project.Waranty1Amount != null)
                            Utils.AddMissingFieldNode(claimInfo.Project.Waranty1Date, xmlDocument, xmlElement, "Project warranty 1 expiry date");

                        if (claimInfo.Project.Waranty2Amount != null)
                            Utils.AddMissingFieldNode(claimInfo.Project.Waranty2Date, xmlDocument, xmlElement, "Project warranty 2 expiry date");

                        Utils.AddMissingFieldNode(claimInfo.Project.PracticalCompletionDate, xmlDocument, xmlElement, "Project practical completion date");
                        Utils.AddMissingFieldNode(claimInfo.Project.DefectsLiability, xmlDocument, xmlElement, "Project defects liability period");
                    }
                }

                //#--
                if (claimInfo.Project.ClientContactList != null)
                {

                    foreach (ClientContactInfo clientInfo in claimInfo.Project.ClientContactList)
                    {
                        if (clientInfo.SendClaims != null)
                            if (clientInfo.SendClaims.Value)
                            {
                                Utils.AddMissingFieldNode(clientInfo.Name, xmlDocument, xmlElement, "Contact name");
                                Utils.AddMissingFieldNode(clientInfo.Email, xmlDocument, xmlElement, clientInfo.Name + " Contact email");
                            }

                    }


                }

                //#---



                //Utils.AddMissingFieldNode(claimInfo.Project.ClientContact.Name, xmlDocument, xmlElement, "Main contact name");
                //Utils.AddMissingFieldNode(claimInfo.Project.ClientContact.Street, xmlDocument, xmlElement, "Main contact address");
                //Utils.AddMissingFieldNode(claimInfo.Project.ClientContact.Locality, xmlDocument, xmlElement, "Main contact suburb");
                //Utils.AddMissingFieldNode(claimInfo.Project.ClientContact.State, xmlDocument, xmlElement, "Main contact state");
                //Utils.AddMissingFieldNode(claimInfo.Project.ClientContact.PostalCode, xmlDocument, xmlElement, "Main contact postal code");

                //if (claimInfo.Project.SendPCToClientContact)
                //    Utils.AddMissingFieldNode(claimInfo.Project.ClientContact.Email, xmlDocument, xmlElement, "Main contact email");

                //if (claimInfo.Project.SendPCToClientContact1)
                //{
                //    Utils.AddMissingFieldNode(claimInfo.Project.ClientContact1.Name, xmlDocument, xmlElement, "Contact 1 name");
                //    Utils.AddMissingFieldNode(claimInfo.Project.ClientContact1.Email, xmlDocument, xmlElement, "Contact 1 email");
                //}

                //if (claimInfo.Project.SendPCToClientContact2)
                //{
                //    Utils.AddMissingFieldNode(claimInfo.Project.ClientContact2.Name, xmlDocument, xmlElement, "Contact 2 name");
                //    Utils.AddMissingFieldNode(claimInfo.Project.ClientContact2.Email, xmlDocument, xmlElement, "Contact 2 email");
                //}

                //if (claimInfo.Project.SendPCToQuantitySurveyor)
                //{
                //    Utils.AddMissingFieldNode(claimInfo.Project.QuantitySurveyor.Name, xmlDocument, xmlElement, "Quantity surveyor name");
                //    Utils.AddMissingFieldNode(claimInfo.Project.QuantitySurveyor.Email, xmlDocument, xmlElement, "Quantity surveyor email");
                //}

                //if (claimInfo.Project.SendPCToSuperintendent)
                //{
                //    Utils.AddMissingFieldNode(claimInfo.Project.Superintendent.Name, xmlDocument, xmlElement, "Superintendent name");
                //    Utils.AddMissingFieldNode(claimInfo.Project.Superintendent.Email, xmlDocument, xmlElement, "Superintendent email");
                //}

                //#---
            }

            if (xmlElement.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElement);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }
        #endregion


        #region Claim Variations Methods
        /// <summary>
        /// Creates a Claim Variation from a dr
        /// </summary>
        public ClaimVariationInfo CreateClaimVariation(IDataReader dr)
        {
            ClaimVariationInfo claimVariationInfo = new ClaimVariationInfo();

            claimVariationInfo.Claim = new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"]));
            claimVariationInfo.ClientVariation = new ClientVariationInfo();
            claimVariationInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            AssignAuditInfo(claimVariationInfo, dr);

            if (dr["ParentClientVariationId"] != DBNull.Value)
            {
                claimVariationInfo.ClientVariation.Id = Data.Utils.GetDBInt32(dr["MaxClientVariationId"]);
                claimVariationInfo.ClientVariation.ParentClientVariation = new ClientVariationInfo(Data.Utils.GetDBInt32(dr["ParentClientVariationId"]));
            }
            else
                claimVariationInfo.ClientVariation.Id = Data.Utils.GetDBInt32(dr["ClientVariationId"]);

            return claimVariationInfo;
        }

        /// <summary>
        /// Get the Claim Variations for the specified Claim
        /// </summary>
        public List<ClaimVariationInfo> GetClaimVariations(ClaimInfo claimInfo)
        {
            IDataReader dr = null;
            List<ClaimVariationInfo> claimVariationInfoList = new List<ClaimVariationInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClaimVariationsByClaim(claimInfo.Id);
                while (dr.Read())
                {
                    claimVariationInfoList.Add(CreateClaimVariation(dr));
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Progress Claim Variations for Claim from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return claimVariationInfoList;
        }

        /// <summary>
        /// Updates a Claim Variation in the database
        /// </summary>
        public void UpdateClaimVariation(ClaimVariationInfo claimVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(claimVariationInfo);

            parameters.Add(claimVariationInfo.Claim.Id);
            parameters.Add(claimVariationInfo.ClientVariation.TheParentClientVariation.Id);
            parameters.Add(claimVariationInfo.Amount);

            parameters.Add(claimVariationInfo.ModifiedDate);
            parameters.Add(claimVariationInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateClaimVariation(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim Variation in database");
            }
        }

        /// <summary>
        /// Adds a Claim Variation to the database
        /// </summary>
        public void AddClaimVariation(ClaimVariationInfo claimVariationInfo)
        {
            List<Object> parameters = new List<Object>();

            SetCreateInfo(claimVariationInfo);

            parameters.Add(claimVariationInfo.Claim.Id);
            parameters.Add(claimVariationInfo.ClientVariation.TheParentClientVariation.Id);
            parameters.Add(claimVariationInfo.Amount);

            parameters.Add(claimVariationInfo.CreatedDate);
            parameters.Add(claimVariationInfo.CreatedBy);

            try
            {
                Data.DataProvider.GetInstance().AddClaimVariation(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Progress Claim Variation to database");
            }
        }

        /// <summary>
        /// Adds or updates a Claim Variation
        /// </summary>
        public void AddUpdateClaimVariation(ClaimVariationInfo claimVariationInfo)
        {
            if (claimVariationInfo != null)
                if (claimVariationInfo.Id != null)
                    UpdateClaimVariation(claimVariationInfo);
                else
                    AddClaimVariation(claimVariationInfo);
        }

        /// <summary>
        /// Remove a Claim Variation from persistent storage
        /// </summary>
        public void DeleteClaimVariation(ClaimVariationInfo claimVariationInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteClaimVariation(claimVariationInfo.Claim.Id, claimVariationInfo.ClientVariation.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Progress Claim Variation from database");
            }
        }
        #endregion


        #region Claim Trades Methods
        /// <summary>
        /// Creates a Claim Trade from a dr
        /// </summary>
        public ClaimTradeInfo CreateClaimTrade(IDataReader dr)
        {
            ClaimTradeInfo claimTradeInfo = new ClaimTradeInfo();

            claimTradeInfo.Claim = new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"]));
            claimTradeInfo.ClientTrade = new ClientTradeInfo(Data.Utils.GetDBInt32(dr["ClientTradeId"]));

            claimTradeInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);

            AssignAuditInfo(claimTradeInfo, dr);

            return claimTradeInfo;
        }

        /// <summary>
        /// Get a Claim Trade from persistent storage
        /// </summary>
        public ClaimTradeInfo GetClaimTrade(int? claimId, int? claimTradeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetClaimTrade(claimId, claimTradeId);
                if (dr.Read())
                    return CreateClaimTrade(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Progress Claim Trade from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Claim Trades for the specified Claim
        /// </summary>
        public List<ClaimTradeInfo> GetClaimTrades(ClaimInfo claimInfo)
        {
            IDataReader dr = null;
            List<ClaimTradeInfo> claimTradeInfoList = new List<ClaimTradeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetClaimTradesByClaim(claimInfo.Id);
                while (dr.Read())
                    claimTradeInfoList.Add(CreateClaimTrade(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Progress Claim Trades for Progress Claim from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return claimTradeInfoList;
        }

        /// <summary>
        /// Updates a Claim Trade in the database
        /// </summary>
        public void UpdateClaimTrade(ClaimTradeInfo claimTradeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(claimTradeInfo);

            parameters.Add(claimTradeInfo.Claim.Id);
            parameters.Add(claimTradeInfo.ClientTrade.Id);
            parameters.Add(claimTradeInfo.Amount);

            parameters.Add(claimTradeInfo.ModifiedDate);
            parameters.Add(claimTradeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateClaimTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Progress Claim Trade in database");
            }
        }

        /// <summary>
        /// Adds a Claim Trade to the database
        /// </summary>
        public void AddClaimTrade(ClaimTradeInfo claimTradeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetCreateInfo(claimTradeInfo);

            parameters.Add(claimTradeInfo.Claim.Id);
            parameters.Add(claimTradeInfo.ClientTrade.Id);
            parameters.Add(claimTradeInfo.Amount);

            parameters.Add(claimTradeInfo.CreatedDate);
            parameters.Add(claimTradeInfo.CreatedBy);

            try
            {
                Data.DataProvider.GetInstance().AddClaimTrade(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Progress Claim Trade to database");
            }
        }

        /// <summary>
        /// Adds or updates a Claim Trade
        /// </summary>
        public void AddUpdateClaimTrade(ClaimTradeInfo claimTradeInfo)
        {
            if (claimTradeInfo != null)
                if (claimTradeInfo.Id != null)
                    UpdateClaimTrade(claimTradeInfo);
                else
                    AddClaimTrade(claimTradeInfo);
        }

        /// <summary>
        /// Remove a Claim Trade from persistent storage
        /// </summary>
        public void DeleteClaimTrade(ClaimTradeInfo claimTradeInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteClaimTrade(claimTradeInfo.Claim.Id, claimTradeInfo.ClientTrade.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Progress Claim Trade from database");
            }
        }
        #endregion


        #region RFIs methods
        /// <summary>
        /// Sets initial state of a new RFI
        /// </summary>
        public RFIInfo InitializeRFI(ProjectInfo projectInfo)
        {
            RFIInfo rFIInfo = new RFIInfo();
            Int32 daysToReply;

            rFIInfo.Project = projectInfo;
            rFIInfo.Status = RFIInfo.StatusNew;
            rFIInfo.RaiseDate = DateTime.Now;
            rFIInfo.Number = projectInfo.RFIs != null ? projectInfo.RFIs.Count + 1 : 1;

            InitializeHolidays();

            rFIInfo.CreatedBy = Web.Utils.GetCurrentUserId();
            if (rFIInfo.CreatedBy != null)
                rFIInfo.Signer = PeopleController.GetInstance().GetPersonById(rFIInfo.CreatedBy);

            try
            {
                daysToReply = Int32.Parse(Web.Utils.GetConfigListItemValue("RFI", "Settings", "DaysToResponse"));
                rFIInfo.TargetAnswerDate = AddbusinessDaysToDate(rFIInfo.RaiseDate, daysToReply);
            }
            catch (Exception ex)
            {
                Utils.LogError("Error casting daysToReply from confi file. " + ex.ToString());
            }

            return rFIInfo;
        }

        /// <summary>
        /// Creates an RFI from a dr
        /// </summary>
        public RFIInfo CreateRFI(IDataReader dr)
        {
            PeopleController peopleController = PeopleController.GetInstance();

            RFIInfo rFIInfo = new RFIInfo(Data.Utils.GetDBInt32(dr["RFIId"]));

            rFIInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
            rFIInfo.Subject = Data.Utils.GetDBString(dr["Subject"]);
            rFIInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            rFIInfo.Status = Data.Utils.GetDBString(dr["Status"]);
            rFIInfo.RaiseDate = Data.Utils.GetDBDateTime(dr["RaiseDate"]);
            rFIInfo.TargetAnswerDate = Data.Utils.GetDBDateTime(dr["TargetAnswerDate"]);
            rFIInfo.ActualAnswerDate = Data.Utils.GetDBDateTime(dr["ActualAnswerDate"]);
            rFIInfo.ReferenceFile = Data.Utils.GetDBString(dr["ReferenceFile"]);
            rFIInfo.ClientResponseFile = Data.Utils.GetDBString(dr["ClientResponseFile"]);
            rFIInfo.ClientResponseSummary = Data.Utils.GetDBString(dr["ClientResponseSummary"]);

            AssignAuditInfo(rFIInfo, dr);

            if (dr["CreatedPeopleId"] != DBNull.Value) rFIInfo.Signer = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CreatedPeopleId"]));

            if (dr["ProjectId"] != DBNull.Value)
            {
                rFIInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                rFIInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                rFIInfo.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
            }

            return rFIInfo;
        }

        /// <summary>
        /// Get an RFI from persistent storage
        /// </summary>
        public RFIInfo GetRFI(int? RFIId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetRFI(RFIId);
                if (dr.Read())
                    return CreateRFI(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RFI from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the RFI for the specified project
        /// </summary>
        public List<RFIInfo> GetRFIs(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<RFIInfo> rFIInfoList = new List<RFIInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetRFIsByProject(projectInfo.Id);
                while (dr.Read())
                    rFIInfoList.Add(CreateRFI(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RFIs for Project from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return rFIInfoList;
        }

        /// <summary>
        /// Updates a RFI in the database
        /// </summary>
        public void UpdateRFI(RFIInfo rFIInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(rFIInfo);

            parameters.Add(rFIInfo.Id);
            parameters.Add(rFIInfo.Subject);
            parameters.Add(rFIInfo.Description);
            parameters.Add(rFIInfo.Status);
            parameters.Add(rFIInfo.RaiseDate);
            parameters.Add(rFIInfo.TargetAnswerDate);
            parameters.Add(rFIInfo.ActualAnswerDate);
            parameters.Add(rFIInfo.ReferenceFile);
            parameters.Add(rFIInfo.ClientResponseFile);
            parameters.Add(rFIInfo.ClientResponseSummary);

            parameters.Add(rFIInfo.ModifiedDate);
            parameters.Add(rFIInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateRFI(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating RFI in database");
            }
        }

        /// <summary>
        /// Updates a RFI status in the database
        /// </summary>
        public void UpdateRFIStatus(RFIInfo rFIInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(rFIInfo.Id);
            parameters.Add(rFIInfo.Status);

            try
            {
                Data.DataProvider.GetInstance().UpdateRFIStatus(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating RFI status in database");
            }
        }

        /// <summary>
        /// Adds an RFI to the database
        /// </summary>
        public int? AddRFI(RFIInfo rFIInfo)
        {
            int? rFIId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(rFIInfo);

            parameters.Add(GetId(rFIInfo.Project));
            parameters.Add(rFIInfo.Number);
            parameters.Add(rFIInfo.Subject);
            parameters.Add(rFIInfo.Description);
            parameters.Add(rFIInfo.Status);
            parameters.Add(rFIInfo.RaiseDate);
            parameters.Add(rFIInfo.TargetAnswerDate);
            parameters.Add(rFIInfo.ActualAnswerDate);
            parameters.Add(rFIInfo.ReferenceFile);
            parameters.Add(rFIInfo.ClientResponseFile);
            parameters.Add(rFIInfo.ClientResponseSummary);

            parameters.Add(rFIInfo.CreatedDate);
            parameters.Add(rFIInfo.CreatedBy);

            try
            {
                rFIId = Data.DataProvider.GetInstance().AddRFI(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding RFI to database");
            }

            return rFIId;
        }

        /// <summary>
        /// Adds or updates a RFI
        /// </summary>
        public int? AddUpdateRFI(RFIInfo rFIInfo)
        {
            if (rFIInfo != null)
            {
                if (rFIInfo.Id != null)
                {
                    UpdateRFI(rFIInfo);
                    return rFIInfo.Id;
                }
                else
                    return AddRFI(rFIInfo);
            }
            else
                return null;
        }

        /// <summary>
        /// Remove an RFI from persistent storage
        /// </summary>
        public void DeleteRFI(RFIInfo rFIInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteRFI(rFIInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing RFI from database");
            }
        }

        /// <summary>
        /// Checks an RFI for errors and missing fields
        /// </summary>
        public XmlDocument CheckRFI(RFIInfo rFIInfo)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElement;

            xmlElementRoot = xmlDocument.CreateElement("root", null);
            xmlElementRoot.SetAttribute("name", "RFI Check");

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElement = xmlDocument.CreateElement("Fields", null);
            xmlElement.SetAttribute("name", "Missing fields");

            Utils.AddMissingFieldNode(rFIInfo.Number, xmlDocument, xmlElement, "Number");
            Utils.AddMissingFieldNode(rFIInfo.Subject, xmlDocument, xmlElement, "Subject");
            Utils.AddMissingFieldNode(rFIInfo.Description, xmlDocument, xmlElement, "Description");
            Utils.AddMissingFieldNode(rFIInfo.Status, xmlDocument, xmlElement, "Status");
            Utils.AddMissingFieldNode(rFIInfo.RaiseDate, xmlDocument, xmlElement, "Raise date");
            Utils.AddMissingFieldNode(rFIInfo.TargetAnswerDate, xmlDocument, xmlElement, "Answer by date");
            Utils.AddMissingFieldNode(rFIInfo.Signer, xmlDocument, xmlElement, "signer");

            if (rFIInfo.Signer != null)
            {
                Utils.AddMissingFieldNode(rFIInfo.Signer.Name, xmlDocument, xmlElement, "Signer name");
                Utils.AddMissingFieldNode(rFIInfo.Signer.Signature, xmlDocument, xmlElement, "Signer signature");
            }

            if (rFIInfo.Project != null)
            {
                Utils.AddMissingFieldNode(rFIInfo.Project.Number, xmlDocument, xmlElement, "Project number");
                Utils.AddMissingFieldNode(rFIInfo.Project.Principal, xmlDocument, xmlElement, "Project principal");

                //#--
                if (rFIInfo.Project.ClientContactList != null)
                {

                    foreach (ClientContactInfo clientInfo in rFIInfo.Project.ClientContactList)
                    {
                        if (clientInfo.SendRFIs != null)
                            if (clientInfo.SendRFIs.Value)
                            {
                                Utils.AddMissingFieldNode(clientInfo.Name, xmlDocument, xmlElement, "Contact name");
                                Utils.AddMissingFieldNode(clientInfo.Email, xmlDocument, xmlElement, clientInfo.Name + " Contact email");
                            }

                    }


                }

                //#--

                ////Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact, xmlDocument, xmlElement, "Main contact");

                ////if (rFIInfo.Project.ClientContact != null)
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact.Name, xmlDocument, xmlElement, "Main contact name");

                ////if (rFIInfo.Project.SendRFIToClientContact)
                ////{
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact.Email, xmlDocument, xmlElement, "Main contact email");
                ////}

                ////if (rFIInfo.Project.SendRFIToClientContact1)
                ////{
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact1.Name, xmlDocument, xmlElement, "Contact 1 name");
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact1.Email, xmlDocument, xmlElement, "Contact 1 email");
                ////}

                ////if (rFIInfo.Project.SendRFIToClientContact2)
                ////{
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact2.Name, xmlDocument, xmlElement, "Contact 2 name");
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.ClientContact2.Email, xmlDocument, xmlElement, "Contact 2 email");
                ////}

                ////if (rFIInfo.Project.SendRFIToQuantitySurveyor)
                ////{
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.QuantitySurveyor.Name, xmlDocument, xmlElement, "Quantity surveyor name");
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.QuantitySurveyor.Email, xmlDocument, xmlElement, "Quantity surveyor email");
                ////}

                ////if (rFIInfo.Project.SendRFIToSuperintendent)
                ////{
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.Superintendent.Name, xmlDocument, xmlElement, "Superintendent name");
                ////    Utils.AddMissingFieldNode(rFIInfo.Project.Superintendent.Email, xmlDocument, xmlElement, "Superintendent email");
                ////}

                //#----
            }

            if (xmlElement.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElement);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        //#----RFI RESPONSE count
        public int GetRFIsResponseNumber(RFIInfo rFIInfo)
        {
            int responseNumber;
            List<Object> parameters = new List<Object>();
            parameters.Add(rFIInfo.Id);
            parameters.Add(rFIInfo.Number);

            try
            {
                responseNumber = Data.DataProvider.GetInstance().GetRFIsResponseNumber(parameters.ToArray());

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RFIsResponseNumber  from database");
            }
            return responseNumber;
        }

        public int? AddRFIsResponse(RFIsResponseInfo rFIsResponseInfo)
        {
            int? rFIsResponseId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(rFIsResponseInfo);

            parameters.Add(rFIsResponseInfo.ResponseNumber);
            parameters.Add(rFIsResponseInfo.Responsemessage);
            parameters.Add(rFIsResponseInfo.ResponseFolderPath);
            parameters.Add(rFIsResponseInfo.ResponseFrom);
            parameters.Add(rFIsResponseInfo.RFI.Id);
            parameters.Add(rFIsResponseInfo.RFI.Number);

            parameters.Add(rFIsResponseInfo.CreatedDate);
            parameters.Add(rFIsResponseInfo.CreatedBy);

            try
            {
                rFIsResponseId = Data.DataProvider.GetInstance().AddRFIsResponse(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding RFIsResponse to database");
            }

            return rFIsResponseId;
        }


        /// <summary>
        /// Get an RFI from persistent storage
        /// </summary>
        public RFIsResponseInfo GetRFIResponse(int? RFIResponseId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetRFISResponse(RFIResponseId);
                if (dr.Read())
                    return CreateRFIResponse(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RFIResponse from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }


        public DataTable GetRFIsWithResponse(RFIInfo rFIInfo)
        {

            IDataReader dr = null;
            DataTable dt = new DataTable();

            try
            {
                dr = Data.DataProvider.GetInstance().GetRFISWithResponse(rFIInfo.Id);
                dt.Load(dr);
                if (dt.Rows.Count > 0)
                    return dt;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RFIResponse from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }



        /// <summary>
        /// Creates an RFI from a dr
        /// </summary>
        public RFIsResponseInfo CreateRFIResponse(IDataReader dr)
        {
            PeopleController peopleController = PeopleController.GetInstance();

            RFIsResponseInfo rFIResponseInfo = new RFIsResponseInfo(Data.Utils.GetDBInt32(dr["ResponseId"]));

            rFIResponseInfo.ResponseNumber = Data.Utils.GetDBInt32(dr["ResponseNumber"]);
            rFIResponseInfo.Responsemessage = Data.Utils.GetDBString(dr["Responsemessage"]);
            rFIResponseInfo.ResponseFrom = peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CreatedBy"]));
            rFIResponseInfo.ResponseFolderPath = Data.Utils.GetDBString(dr["ResponseFolderPath"]);
            if (dr["RFIId"] != DBNull.Value) { rFIResponseInfo.RFI = GetRFI(Data.Utils.GetDBInt32(dr["RFIId"])); }

            return rFIResponseInfo;

        }



        public int? AddRFIsResponseAttachments(RFIsResponseInfo rFIsResponseInfo, string FileName, string FileExtension, byte[] FileData)
        {
            int? rFIsResponseId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(rFIsResponseInfo);

            parameters.Add(rFIsResponseInfo.Id);
            parameters.Add(rFIsResponseInfo.RFI.Id);
            parameters.Add(FileName);
            parameters.Add(FileExtension);
            parameters.Add(FileData);



            parameters.Add(rFIsResponseInfo.CreatedDate);
            parameters.Add(rFIsResponseInfo.CreatedBy);

            try
            {
                rFIsResponseId = Data.DataProvider.GetInstance().AddRFIsResponseAttachments(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding RFIsResponse attachments to database");
            }

            return rFIsResponseId;
        }


        /// <summary>
        /// Get an RFI from persistent storage
        /// </summary>
        public List<RFIsResponseAttachmentInfo> GetRFIResponseAttachments(int? RFIResponseId)
        {
            IDataReader dr = null;
            List<RFIsResponseAttachmentInfo> lstresponseAttachments = new List<RFIsResponseAttachmentInfo>();
            RFIsResponseAttachmentInfo rFIsResponseAttachment;
            try
            {
                dr = Data.DataProvider.GetInstance().GetRFISResponseAttachments(RFIResponseId);
                while (dr.Read())
                {
                    rFIsResponseAttachment = new RFIsResponseAttachmentInfo();
                    rFIsResponseAttachment.Id = Data.Utils.GetDBInt32(dr["Id"].ToString());
                    rFIsResponseAttachment.FileName = Data.Utils.GetDBString(dr["FileName"].ToString());
                    rFIsResponseAttachment.FileExtension = Data.Utils.GetDBString(dr["FileType"].ToString());
                    rFIsResponseAttachment.FileData = ((byte[])dr["Filedata"]);
                    // rFIsResponseAttachment.RFIsResponse.Id= Data.Utils.GetDBInt32(dr["ResponseId"].ToString());


                    lstresponseAttachments.Add(rFIsResponseAttachment);

                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting RFIResponse attachments from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return lstresponseAttachments;
        }






        #endregion

        //#---
        #region ProjectImage methods

        public int? AddProjectImage(ProjectImage pImage)
        {
            int? pImageId;
            List<Object> parameters = new List<Object>();

            parameters.Add(pImage.ProjectId);
            parameters.Add(pImage.ProjectName);
            parameters.Add(pImage.ParentFolder);
            parameters.Add(pImage.FolderName);
            parameters.Add(pImage.ImageName);
            parameters.Add(pImage.ImageData);

            try
            {
                pImageId = Data.DataProvider.GetInstance().AddProjectImage(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding ProjectImage to database");
            }

            return pImageId;
        }

        public List<ProjectImage> GetProjectImages(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<ProjectImage> ImageList = new List<ProjectImage>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectImagesByProject(projectInfo.Id);
                while (dr.Read())
                {
                    ProjectImage pImage = new ProjectImage();
                    pImage.Id = Data.Utils.GetDBInt32(dr["Id"]);
                    pImage.ImageName = Data.Utils.GetDBString(dr["ImageName"]);
                    pImage.FolderName = Data.Utils.GetDBString(dr["FolderName"]);
                    pImage.ParentFolder = Data.Utils.GetDBString(dr["ParentFolder"]);
                    // pImage.ImageData = ((byte[])dr["ImageData"]);
                    pImage.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);

                    ImageList.Add(pImage);
                }


            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting Project Images for Project from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return ImageList;
        }


        public ProjectImage GetProjectImageById(int? Id)
        {
            IDataReader dr = null;
            ProjectImage pImage = null;


            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectImageById(Id);
                while (dr.Read())
                {
                    pImage = new ProjectImage();
                    pImage.Id = Data.Utils.GetDBInt32(dr["Id"]);
                    pImage.ImageName = Data.Utils.GetDBString(dr["ImageName"]);
                    pImage.FolderName = Data.Utils.GetDBString(dr["FolderName"]);
                    pImage.ParentFolder = Data.Utils.GetDBString(dr["ParentFolder"]);
                    pImage.ImageData = ((byte[])dr["ImageData"]);
                    pImage.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);


                }


            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on Getting Project Image by id from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return pImage;
        }

        #endregion


        #region MeetingMinutes

        /// <summary>
        /// Sets initial state of a new Meeting
        /// </summary>
        public MeetingMinutesInfo InitializeMeeting(ProjectInfo projectInfo)
        {
            MeetingMinutesInfo meetingInfo = new MeetingMinutesInfo();
            meetingInfo.Project = projectInfo;
            meetingInfo.Number = projectInfo.MeetingMinutesList != null ? projectInfo.MeetingMinutesList.Count + 1 : 1;
            meetingInfo.CreatedBy = Web.Utils.GetCurrentUserId();


            return meetingInfo;
        }

        /// <summary>
        /// Creates an Meeting from a dr
        /// </summary>
        public MeetingMinutesInfo CreateMeeting(IDataReader dr)
        {
            PeopleController peopleController = PeopleController.GetInstance();

            MeetingMinutesInfo meetingInfo = new MeetingMinutesInfo(Data.Utils.GetDBInt32(dr["MeetingId"]));

            meetingInfo.Number = Data.Utils.GetDBInt32(dr["MeetingNumber"]);
            meetingInfo.Subject = Data.Utils.GetDBString(dr["Subject"]);
            meetingInfo.Location = Data.Utils.GetDBString(dr["Location"]);
            meetingInfo.Attendees = Data.Utils.GetDBString(dr["Attendees"]);
            meetingInfo.MeetingDate = Data.Utils.GetDBDateTime(dr["MeetingDate"]);
            meetingInfo.MeetingTime = Data.Utils.GetDBString(dr["MeetingTime"]);
            meetingInfo.ReferenceFile = Data.Utils.GetDBString(dr["ReferenceFile"]);
            meetingInfo.TypeNumber = Data.Utils.GetDBInt32(dr["TypeNumber"]);

            // AssignAuditInfo(meetingInfo, dr);

            //if (dr["CreatedBy"] != DBNull.Value) meetingInfo.CreatedBy = (EmployeeInfo)peopleController.GetPersonById(Data.Utils.GetDBInt32(dr["CreatedPeopleId"]));

            if (dr["ProjectId"] != DBNull.Value)
            {
                meetingInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                meetingInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);

                meetingInfo.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
            }

            return meetingInfo;
        }







        /// <summary>
        /// Get an RFI from persistent storage
        /// </summary>
        public MeetingMinutesInfo GetMeetingById(int? MeetingId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetMeeting(MeetingId);
                if (dr.Read())
                    return CreateMeeting(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Meeting from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Gets a Project with Meetings from persistent storage
        /// </summary>
        public ProjectInfo GetProjectWithMeetings(int? ProjectId)
        {
            ProjectInfo projectInfo = GetProject(ProjectId);
            projectInfo.MeetingMinutesList = GetMeetings(projectInfo);

            if (projectInfo.MeetingMinutesList != null)
                foreach (MeetingMinutesInfo meetingInfo in projectInfo.MeetingMinutesList)
                    meetingInfo.Project = projectInfo;

            return projectInfo;
        }

        /// <summary>
        /// Get the Meetings for the specified project
        /// </summary>
        public List<MeetingMinutesInfo> GetMeetings(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<MeetingMinutesInfo> meetingsList = new List<MeetingMinutesInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetMeetingsByProject(projectInfo.Id);
                while (dr.Read())
                    meetingsList.Add(CreateMeeting(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Meetings for Project from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return meetingsList;
        }




        /// <summary>
        /// Updates a RFI in the database
        /// </summary>
        public void UpdateMeeting(MeetingMinutesInfo meetingInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(meetingInfo);

            parameters.Add(meetingInfo.Id);
            parameters.Add(meetingInfo.Subject);

            parameters.Add(meetingInfo.MeetingDate);
            parameters.Add(meetingInfo.MeetingTime);
            parameters.Add(meetingInfo.Location);
            parameters.Add(meetingInfo.ReferenceFile);
            parameters.Add(meetingInfo.Attendees);
            parameters.Add(meetingInfo.TypeNumber);

            parameters.Add(meetingInfo.ModifiedDate);
            parameters.Add(meetingInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateMeeting(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Meeting in database");
            }
        }



        /// <summary>
        /// Adds an RFI to the database
        /// </summary>
        public int? AddMeeting(MeetingMinutesInfo meetingInfo)
        {
            int? MeetingId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(meetingInfo);

            parameters.Add(GetId(meetingInfo.Project));
            parameters.Add(meetingInfo.Number);
            parameters.Add(meetingInfo.Subject);
            parameters.Add(meetingInfo.MeetingDate);
            parameters.Add(meetingInfo.MeetingTime);
            parameters.Add(meetingInfo.Location);
            parameters.Add(meetingInfo.Attendees);
            parameters.Add(meetingInfo.ReferenceFile);
            parameters.Add(meetingInfo.TypeNumber);

            parameters.Add(meetingInfo.CreatedDate);
            parameters.Add(meetingInfo.CreatedBy);

            try
            {
                MeetingId = Data.DataProvider.GetInstance().AddMeeting(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Meeting to database");
            }

            return MeetingId;
        }

        /// <summary>
        /// Adds or updates a RFI
        /// </summary>
        public int? AddUpdateMeeting(MeetingMinutesInfo meetingInfo)
        {
            if (meetingInfo != null)
            {
                if (meetingInfo.Id != null)
                {
                    UpdateMeeting(meetingInfo);
                    return meetingInfo.Id;
                }
                else
                    return AddMeeting(meetingInfo);
            }
            else
                return null;
        }



        /// <summary>
        /// Delete Meeting from  storage
        /// </summary>
        public void DeleteMeeting(MeetingMinutesInfo meetingInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteMeeting(meetingInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing meeting from database");
            }
        }





        #endregion
        //#--


        #region KPIranges
        public void UpdateKPIRange(KPIRangeInfo KPIRange)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(KPIRange);

            parameters.Add(KPIRange.Id);
            parameters.Add(KPIRange.TargetValue);
            parameters.Add(KPIRange.MinRange);
            parameters.Add(KPIRange.MaxRange);
            parameters.Add(KPIRange.ModifiedBy);
            parameters.Add(KPIRange.ModifiedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateKPIRange(parameters.ToArray());

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on updating KPIRange in database");
            }




        }

        public void UpdateKPIPoints(KPIPointsInfo KPIPoint)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(KPIPoint);

            parameters.Add(KPIPoint.Id);
            parameters.Add(KPIPoint.minPoints);
            parameters.Add(KPIPoint.Points);
            parameters.Add(KPIPoint.ModifiedBy);
            parameters.Add(KPIPoint.ModifiedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateKPIPoints(parameters.ToArray());

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on updating KPIRange in database");
            }




        }



        #endregion





        #region EOTs methods

        /// <summary>
        /// Sets initial state of a new EOT
        /// </summary>
        public EOTInfo InitializeEOT(ProjectInfo projectInfo)
        {
            EOTInfo eOTInfo = new EOTInfo();

            eOTInfo.Project = projectInfo;
            eOTInfo.WriteDate = DateTime.Now;
            eOTInfo.Number = projectInfo.EOTs != null ? projectInfo.EOTs.Count + 1 : 1;

            return eOTInfo;
        }

        /// <summary>
        /// Creates an EOT from a dr
        /// </summary>
        public EOTInfo CreateEOT(IDataReader dr)
        {
            PeopleController peopleController = PeopleController.GetInstance();

            EOTInfo eOTInfo = new EOTInfo(Data.Utils.GetDBInt32(dr["EOTId"]));

            eOTInfo.Number = Data.Utils.GetDBInt32(dr["Number"]);
            eOTInfo.StartDate = Data.Utils.GetDBDateTime(dr["StartDate"]);
            eOTInfo.EndDate = Data.Utils.GetDBDateTime(dr["EndDate"]);
            eOTInfo.FirstNoticeDate = Data.Utils.GetDBDateTime(dr["FirstNoticeDate"]);
            eOTInfo.WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);
            eOTInfo.SendDate = Data.Utils.GetDBDateTime(dr["SendDate"]);
            eOTInfo.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
            eOTInfo.DaysClaimed = Data.Utils.GetDBFloat(dr["DaysClaimed"]);
            eOTInfo.DaysApproved = Data.Utils.GetDBFloat(dr["DaysApproved"]);
            eOTInfo.Cause = Data.Utils.GetDBString(dr["Cause"]);
            eOTInfo.Nature = Data.Utils.GetDBString(dr["Nature"]);
            eOTInfo.Period = Data.Utils.GetDBString(dr["Period"]);
            eOTInfo.Works = Data.Utils.GetDBString(dr["Works"]);
            eOTInfo.CostCode = Data.Utils.GetDBString(dr["CostCode"]);
            eOTInfo.Status = Data.Utils.GetDBString(dr["Status"]);
            eOTInfo.ClientApprovalFile = Data.Utils.GetDBString(dr["ClientApprovalFile"]);
            //#---
            eOTInfo.ClientBackuplFile = Data.Utils.GetDBString(dr["ClientBackUpFile"]);
            eOTInfo.TypeofEot = Data.Utils.GetDBString(dr["EotType"]);  //#---


            AssignAuditInfo(eOTInfo, dr);

            if (dr["ProjectId"] != DBNull.Value)
            {
                eOTInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                eOTInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                eOTInfo.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
            }

            return eOTInfo;
        }

        /// <summary>
        /// Get an EOT from persistent storage
        /// </summary>
        public EOTInfo GetEOT(int? EOTId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetEOT(EOTId);
                if (dr.Read())
                    return CreateEOT(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting EOT from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }


        public EOTInfo GetNODEOT(int? EOTId, int? NODId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetNODEOT(EOTId, NODId);
                if (dr.Read())
                    return CreateEOT(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting EOT from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }


        /// <summary>
        /// Get the EOT for the specified project
        /// </summary>
        public List<EOTInfo> GetEOTs(ProjectInfo projectInfo)
        {
            IDataReader dr = null;
            List<EOTInfo> eOTInfoList = new List<EOTInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetEOTsByProject(projectInfo.Id);
                while (dr.Read())
                    eOTInfoList.Add(CreateEOT(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting EOTs for Project from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return eOTInfoList;
        }

        /// <summary>
        /// Updates an EOT in the database
        /// </summary>
        public void UpdateEOT(EOTInfo eOTInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(eOTInfo);

            parameters.Add(eOTInfo.Id);
            parameters.Add(eOTInfo.StartDate);
            parameters.Add(eOTInfo.EndDate);
            parameters.Add(eOTInfo.FirstNoticeDate);
            parameters.Add(eOTInfo.WriteDate);
            parameters.Add(eOTInfo.SendDate);
            parameters.Add(eOTInfo.ApprovalDate);
            parameters.Add(eOTInfo.DaysClaimed);
            parameters.Add(eOTInfo.DaysApproved);
            parameters.Add(eOTInfo.Cause);
            parameters.Add(eOTInfo.Nature);
            parameters.Add(eOTInfo.Period);
            parameters.Add(eOTInfo.Works);
            parameters.Add(eOTInfo.CostCode);
            parameters.Add(eOTInfo.Status);
            parameters.Add(eOTInfo.ClientApprovalFile);

            //#--
            parameters.Add(eOTInfo.TypeofEot);
            parameters.Add(eOTInfo.ClientBackuplFile);   //#---

            parameters.Add(eOTInfo.ModifiedDate);
            parameters.Add(eOTInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateEOT(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating EOT in database");
            }
        }

        /// <summary>
        /// Updates an EOT send date in the database
        /// </summary>
        public void UpdateEOTSendDate(EOTInfo eOTInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(eOTInfo.Id);
            parameters.Add(eOTInfo.SendDate);

            try
            {
                Data.DataProvider.GetInstance().UpdateEOTSendDate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating EOT Send Date in database");
            }
        }

        /// <summary>
        /// Adds an EOT to the database
        /// </summary>
        public int? AddEOT(EOTInfo eOTInfo)
        {
            int? eOTId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(eOTInfo);

            parameters.Add(GetId(eOTInfo.Project));
            parameters.Add(eOTInfo.Number);
            parameters.Add(eOTInfo.StartDate);
            parameters.Add(eOTInfo.EndDate);
            parameters.Add(eOTInfo.FirstNoticeDate);
            parameters.Add(eOTInfo.WriteDate);
            parameters.Add(eOTInfo.SendDate);
            parameters.Add(eOTInfo.ApprovalDate);
            parameters.Add(eOTInfo.DaysClaimed);
            parameters.Add(eOTInfo.DaysApproved);
            parameters.Add(eOTInfo.Cause);
            parameters.Add(eOTInfo.Nature);
            parameters.Add(eOTInfo.Period);
            parameters.Add(eOTInfo.Works);
            parameters.Add(eOTInfo.CostCode);
            parameters.Add(eOTInfo.Status);
            parameters.Add(eOTInfo.ClientApprovalFile);
            //#--
            parameters.Add(eOTInfo.TypeofEot);
            parameters.Add(eOTInfo.ClientBackuplFile);   //#---

            parameters.Add(eOTInfo.CreatedDate);
            parameters.Add(eOTInfo.CreatedBy);

            try
            {
                eOTId = Data.DataProvider.GetInstance().AddEOT(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding EOT to database");
            }

            return eOTId;
        }

        /// <summary>
        /// Adds or updates a EOT
        /// </summary>
        public int? AddUpdateEOT(EOTInfo eOTInfo)
        {
            if (eOTInfo != null)
            {
                if (eOTInfo.Id != null)
                {
                    UpdateEOT(eOTInfo);
                    return eOTInfo.Id;
                }
                else
                    return AddEOT(eOTInfo);
            }
            else
                return null;
        }


        //#----To add NODs
        public int? AddUpdateNODEOT(EOTInfo eOTInfo)
        {
            if (eOTInfo != null)
            {
                return AddNODEOT(eOTInfo);
            }
            else
                return null;
        }

        //#----To add NODs to database
        public int? AddNODEOT(EOTInfo eOTInfo)
        {
            int? nODId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(eOTInfo);
            parameters.Add(eOTInfo.Id);
            parameters.Add(GetId(eOTInfo.Project));
            parameters.Add(eOTInfo.Number);
            parameters.Add(eOTInfo.StartDate);
            parameters.Add(eOTInfo.EndDate);
            parameters.Add(eOTInfo.FirstNoticeDate);
            parameters.Add(eOTInfo.WriteDate);
            parameters.Add(eOTInfo.SendDate);
            parameters.Add(eOTInfo.ApprovalDate);
            parameters.Add(eOTInfo.DaysClaimed);
            parameters.Add(eOTInfo.DaysApproved);
            parameters.Add(eOTInfo.Cause);
            parameters.Add(eOTInfo.Nature);
            parameters.Add(eOTInfo.Period);
            parameters.Add(eOTInfo.Works);
            parameters.Add(eOTInfo.CostCode);
            parameters.Add(eOTInfo.Status);
            parameters.Add(eOTInfo.CreatedDate);
            parameters.Add(eOTInfo.CreatedBy);

            try
            {
                nODId = Data.DataProvider.GetInstance().AddNODEOT(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding NODEOTs to database");
            }

            return nODId;
        }





        /// <summary>
        /// Remove an EOT from persistent storage
        /// </summary>
        public void DeleteEOT(EOTInfo eOTInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteEOT(eOTInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing EOT from database");
            }
        }


        //#--------22/06/2016---
        //To display Project completion date including claimed EOTs and Including Approved EOTS-- 


        //Get project complitiondate including claimed EOTs and taking account of RDOs and Holidays
        public DateTime? GetProjectCompletionDateWithClaimedEOTs(ProjectInfo projectInfo)
        {
            double DaysClaimed = 0;
            DateTime? EOTsClaimedate = null; //DateTime.Now.AddYears(10);

            if (projectInfo.EOTs.Count > 0 && projectInfo.CompletionDate != null)
            {
                DateTime CalDate;

                CalDate = projectInfo.CompletionDate.Value;

                foreach (EOTInfo EOts in projectInfo.EOTs)
                {
                    if (EOts.DaysClaimed != null)
                        DaysClaimed = DaysClaimed + double.Parse(EOts.DaysClaimed.ToString());
                }

                CalDate = CalDate.AddDays(DaysClaimed);

                CalDate = CheckHolidaysWeekEndRDO(CalDate);

                if (CalDate != null)
                    EOTsClaimedate = CalDate;

            }

            return EOTsClaimedate;
        }


        //#--------22/06/2016---
        //Get project complitiondate including claimed EOTs and taking account of RDOs and Holidays
        public DateTime? GetProjectCompletionDateWithApprovedEOTs(ProjectInfo projectInfo)
        {
            double DaysClaimed = 0, TotalDaysClaimed = 0;
            DateTime? EOTsAppovedDate = null;  //DateTime.Now.AddYears(10);

            if (projectInfo.EOTs.Count > 0 && projectInfo.CompletionDate != null)
            {
                DateTime CalDate;
                CalDate = projectInfo.CompletionDate.Value;


                foreach (EOTInfo EOts in projectInfo.EOTs)
                {
                    if (EOts.DaysApproved != null && EOts.DaysApproved > 0)
                    {
                        DaysClaimed = DaysClaimed + double.Parse(EOts.DaysApproved.ToString());
                        TotalDaysClaimed += DaysClaimed;

                        for (int i = 1; i <= Math.Ceiling(EOts.DaysApproved.Value); i++)
                        {
                            CalDate = CalDate.AddDays(1);
                            CalDate = CheckHolidaysWeekEndRDO(CalDate);
                            DaysClaimed = 0;
                        }
                    }
                }

                //CalDate = CalDate.AddDays(DaysClaimed);

                //CalDate = CheckHolidaysWeekEndRDO(CalDate);

                if (CalDate != null)
                    EOTsAppovedDate = CalDate;
            }

            return EOTsAppovedDate;
        }



        //#--------22/06/2016---
        private DateTime CheckHolidaysWeekEndRDO(DateTime EOTsdate)
        {

            int i = 0;
            bool Holiday = true, RDOs = true, WeekendSat = true, WeekendSun = true;
            List<DateTime> sHoliday = GetHolidays();
            List<DateTime> sRDOs = GetRDOs();

            while (i < 3)
            {

                if (!CheckExistInHolidays(EOTsdate, sHoliday))
                {
                    EOTsdate = EOTsdate.AddDays(1);
                    Holiday = false;
                }
                else { Holiday = true; }

                if (!CheckExistInRDOs(EOTsdate, sRDOs))
                {
                    EOTsdate = EOTsdate.AddDays(1);
                    RDOs = false;
                }
                else { RDOs = true; }

                if (EOTsdate.DayOfWeek.ToString() == "Saturday")
                {
                    EOTsdate = EOTsdate.AddDays(2);
                    WeekendSat = false;
                }
                else { WeekendSat = true; }

                if (EOTsdate.DayOfWeek.ToString() == "Sunday")
                {
                    EOTsdate = EOTsdate.AddDays(1);
                    WeekendSun = false;
                }
                else { WeekendSun = true; }


                if (Holiday && RDOs && WeekendSat && WeekendSun)
                {
                    i = 4;
                }
                else { i = 2; }


            }


            return EOTsdate;

        }

        //#--------22/06/2016---
        private bool CheckExistInHolidays(DateTime dt, List<DateTime> sHolidays)
        {
            if (sHolidays.Exists(x => x.Date == dt.Date))
            {
                return false;
            }
            else { return true; }

        }

        //#--------22/06/2016---
        private bool CheckExistInRDOs(DateTime dt, List<DateTime> sRDO)
        {
            if (sRDO.Exists(x => x.Date == dt.Date))
            {
                return false;
            }
            else { return true; }

        }

        //#--------22/06/2016----- 


        //#--- Project workingdays -Weekends-Holidays-Rdos
        public int GetProjectOriginalDuration(ProjectInfo projectInfo)
        {
            int Projectduration = 0;

            if (projectInfo.CompletionDate.Value != null && projectInfo.CommencementDate.Value != null)
            {
                List<DateTime> sHoliday = GetHolidays();
                List<DateTime> sRDOs = GetRDOs();

                int cntHoliday = sHoliday.Count(x => x.Date >= projectInfo.CommencementDate.Value && x.Date <= projectInfo.CompletionDate.Value);

                int cntRDOs = sRDOs.Count(x => x.Date >= projectInfo.CommencementDate.Value && x.Date <= projectInfo.CompletionDate.Value);


                DateTime Startdate, Enddate;
                Startdate = projectInfo.CommencementDate.Value;
                Enddate = projectInfo.CompletionDate.Value;
                while (Startdate <= Enddate)
                {
                    if (Startdate.DayOfWeek != DayOfWeek.Saturday && Startdate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        ++Projectduration;
                    }
                    Startdate = Startdate.AddDays(1);
                }


                Projectduration = Projectduration - (cntHoliday + cntRDOs);
            }

            return Projectduration;
        }

        //#---Project workingdays -Weekends-Holidays-Rdos-EOTs
        public double GetProjectWorkingDuration(DateTime Startdate, DateTime Enddate, double ApprovedEots = 0.0)
        {
            double Projectduration = 0;

            if (Startdate != null && Enddate != null)
            {
                List<DateTime> sHoliday = GetHolidays();
                List<DateTime> sRDOs = GetRDOs();

                double cntHoliday = sHoliday.Count(x => x.Date >= Startdate && x.Date <= Enddate);

                double cntRDOs = sRDOs.Count(x => x.Date >= Startdate && x.Date <= Enddate);


                //DateTime Startdate, Enddate;
                //Startdate = projectInfo.CommencementDate.Value;
                //Enddate = projectInfo.CompletionDate.Value;
                while (Startdate <= Enddate)
                {
                    if (Startdate.DayOfWeek != DayOfWeek.Saturday && Startdate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        ++Projectduration;
                    }
                    Startdate = Startdate.AddDays(1);
                }


                Projectduration = Projectduration - (cntHoliday + cntRDOs + ApprovedEots);
            }

            return Projectduration;
        }




        /// <summary>
        /// Checks an EOT for errors and missing fields
        /// </summary>
        public XmlDocument CheckEOT(EOTInfo eOTInfo)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElement;

            xmlElementRoot = xmlDocument.CreateElement("root", null);
            xmlElementRoot.SetAttribute("name", "EOT Check");

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));

            xmlElement = xmlDocument.CreateElement("Fields", null);
            xmlElement.SetAttribute("name", "Missing fields");

            Utils.AddMissingFieldNode(eOTInfo.Number, xmlDocument, xmlElement, "Number");
            Utils.AddMissingFieldNode(eOTInfo.FirstNoticeDate, xmlDocument, xmlElement, "First notice date");
            Utils.AddMissingFieldNode(eOTInfo.CostCode, xmlDocument, xmlElement, "Costs");
            Utils.AddMissingFieldNode(eOTInfo.WriteDate, xmlDocument, xmlElement, "Write date");
            Utils.AddMissingFieldNode(eOTInfo.StartDate, xmlDocument, xmlElement, "Start date");
            Utils.AddMissingFieldNode(eOTInfo.EndDate, xmlDocument, xmlElement, "End date");
            Utils.AddMissingFieldNode(eOTInfo.Cause, xmlDocument, xmlElement, "Cause");
            Utils.AddMissingFieldNode(eOTInfo.Nature, xmlDocument, xmlElement, "Nature");
            Utils.AddMissingFieldNode(eOTInfo.Period, xmlDocument, xmlElement, "Period");
            Utils.AddMissingFieldNode(eOTInfo.Works, xmlDocument, xmlElement, "Works");

            if (eOTInfo.Project != null)
            {
                Utils.AddMissingFieldNode(eOTInfo.Project.Number, xmlDocument, xmlElement, "Project number");
                Utils.AddMissingFieldNode(eOTInfo.Project.Name, xmlDocument, xmlElement, "Project name");
                Utils.AddMissingFieldNode(eOTInfo.Project.ProjectManager, xmlDocument, xmlElement, "Project Manager");
                Utils.AddMissingFieldNode(eOTInfo.Project.ConstructionManager, xmlDocument, xmlElement, "Construction Manager");
                Utils.AddMissingFieldNode(eOTInfo.Project.ManagingDirector, xmlDocument, xmlElement, "Managing Director");
                //#--Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact, xmlDocument, xmlElement, "Main contact");


                //#--
                if (eOTInfo.Project.ClientContactList != null)
                {

                    foreach (ClientContactInfo clientInfo in eOTInfo.Project.ClientContactList)
                    {
                        if (clientInfo.SendEOTs != null)
                            if (clientInfo.SendEOTs.Value)
                            {
                                Utils.AddMissingFieldNode(clientInfo.Name, xmlDocument, xmlElement, "Contact name");
                                Utils.AddMissingFieldNode(clientInfo.Email, xmlDocument, xmlElement, clientInfo.Name + " Contact email");
                            }

                    }


                }

                //#---

                ////if (eOTInfo.Project.ClientContact != null)
                ////{
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact.Name, xmlDocument, xmlElement, "Main contact name");
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact.Email, xmlDocument, xmlElement, "Main contact email");
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact.Fax, xmlDocument, xmlElement, "Main contact fax");
                ////}

                ////if (eOTInfo.Project.SendEOTToClientContact1)
                ////{
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact1.Name, xmlDocument, xmlElement, "Contact 1 name");
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact1.Email, xmlDocument, xmlElement, "Contact 1 email");
                ////}

                ////if (eOTInfo.Project.SendEOTToClientContact2)
                ////{
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact2.Name, xmlDocument, xmlElement, "Contact 2 name");
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.ClientContact2.Email, xmlDocument, xmlElement, "Contact 2 email");
                ////}

                ////if (eOTInfo.Project.SendEOTToQuantitySurveyor)
                ////{
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.QuantitySurveyor.Name, xmlDocument, xmlElement, "Quantity surveyor name");
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.QuantitySurveyor.Email, xmlDocument, xmlElement, "Quantity surveyor email");
                ////}

                ////if (eOTInfo.Project.SendRFIToSuperintendent)
                ////{
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.Superintendent.Name, xmlDocument, xmlElement, "Superintendent name");
                ////    Utils.AddMissingFieldNode(eOTInfo.Project.Superintendent.Email, xmlDocument, xmlElement, "Superintendent email");
                ////}
                //#---

            }

            if (xmlElement.HasChildNodes)
                xmlElementRoot.AppendChild(xmlElement);

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }





        #endregion


        #region Attachments
        /// <summary>
        /// Creates an Attachment from a dr.
        /// </summary>
        public AttachmentInfo CreateAttachment(IDataReader dr)
        {
            PeopleController peopleController = PeopleController.GetInstance();

            AttachmentInfo attachmentInfo = new AttachmentInfo(Data.Utils.GetDBInt32(dr["AttachmentId"]));

            attachmentInfo.Type = Data.Utils.GetDBString(dr["Type"]);
            attachmentInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            attachmentInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            attachmentInfo.FilePath = Data.Utils.GetDBString(dr["FilePath"]);
            attachmentInfo.AttachDate = Data.Utils.GetDBDateTime(dr["AttachDate"]);

            AssignAuditInfo(attachmentInfo, dr);

            attachmentInfo.AttachmentsGroup = new AttachmentsGroupInfo(Data.Utils.GetDBInt32(dr["AttachmentsGroupId"]));

            return attachmentInfo;
        }

        /// <summary>
        /// Get an Attachment from persistent storage
        /// </summary>
        public AttachmentInfo GetAttachment(int? AttachmentId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetAttachment(AttachmentId);
                if (dr.Read())
                    return CreateAttachment(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Attachment from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get the Attachment for the specified attachments group
        /// </summary>
        public void GetAttachments(IDocumentable iDocumentable)
        {
            IDataReader dr = null;
            iDocumentable.AttachmentsGroup.Attachments = new List<AttachmentInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetAttachmentsByGroup(iDocumentable.AttachmentsGroup.Id);
                while (dr.Read())
                    iDocumentable.AttachmentsGroup.Attachments.Add(CreateAttachment(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Attachments Group from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }

        /// <summary>
        /// Updates an Attachment in the database
        /// </summary>
        public void UpdateAttachment(AttachmentInfo attachmentInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(attachmentInfo);

            parameters.Add(attachmentInfo.Id);
            parameters.Add(attachmentInfo.Name);
            parameters.Add(attachmentInfo.Description);
            parameters.Add(attachmentInfo.FilePath);
            parameters.Add(attachmentInfo.AttachDate);

            parameters.Add(attachmentInfo.ModifiedDate);
            parameters.Add(attachmentInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateAttachment(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Attachment in database");
            }
        }

        /// <summary>
        /// Adds an Attachment to the database
        /// </summary>
        public int? AddAttachment(AttachmentInfo attachmentInfo)
        {
            int? attachmentId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(attachmentInfo);

            parameters.Add(GetId(attachmentInfo.AttachmentsGroup));
            parameters.Add(attachmentInfo.Type);
            parameters.Add(attachmentInfo.Name);
            parameters.Add(attachmentInfo.Description);
            parameters.Add(attachmentInfo.FilePath);
            parameters.Add(attachmentInfo.AttachDate);

            parameters.Add(attachmentInfo.CreatedDate);
            parameters.Add(attachmentInfo.CreatedBy);

            try
            {
                attachmentId = Data.DataProvider.GetInstance().AddAttachment(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Attachment to database");
            }

            return attachmentId;
        }

        /// <summary>
        /// Adds or updates a Attachment
        /// </summary>
        public int? AddUpdateAttachment(AttachmentInfo attachmentInfo)
        {
            if (attachmentInfo != null)
            {
                if (attachmentInfo.Id != null)
                {
                    UpdateAttachment(attachmentInfo);
                    return attachmentInfo.Id;
                }
                else
                    return AddAttachment(attachmentInfo);
            }
            else
                return null;
        }

        /// <summary>
        /// Remove an Attachment from persistent storage
        /// </summary>
        public void DeleteAttachment(AttachmentInfo attachmentInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteAttachment(attachmentInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Attachment from database");
            }
        }
        #endregion


        #region Project Reports methods
        /// <summary>
        /// Get the purchasing schedule report
        /// </summary>
        public List<TradeInfo> GetTradesReport(int? projectId, int? subContractorId, int? peopleId, int? businessUnitId, String tradeCode, String projectSatus)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            TradeInfo tradeInfo;

            parameters.Add(projectId);
            parameters.Add(subContractorId);
            parameters.Add(peopleId);
            parameters.Add(businessUnitId);
            parameters.Add(tradeCode);
            parameters.Add(projectSatus);

            try
            {
                dr = Data.DataProvider.GetInstance().GetTradesReport(parameters.ToArray());
                while (dr.Read())
                {
                    tradeInfo = new TradeInfo();
                    tradeInfo.Contract = new ContractInfo();
                    tradeInfo.Project = new ProjectInfo();
                    tradeInfo.Project.BusinessUnit = new BusinessUnitInfo();
                    tradeInfo.Participations = new List<TradeParticipationInfo>();
                    tradeInfo.Participations.Add(new TradeParticipationInfo());

                    tradeInfo.Name = Data.Utils.GetDBString(dr["Name"]);
                    tradeInfo.Code = Data.Utils.GetDBString(dr["Code"]);
                    tradeInfo.ContractDueDate = Data.Utils.GetDBDateTime(dr["ContractDueDate"]);

                    tradeInfo.Contract.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);

                    tradeInfo.Project.Id = Data.Utils.GetDBInt32(dr["ProjectId"]);
                    tradeInfo.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    tradeInfo.Project.Number = Data.Utils.GetDBString(dr["ProjectNumber"]);
                    tradeInfo.Project.Year = Data.Utils.GetDBString(dr["ProjectYear"]);

                    tradeInfo.Project.BusinessUnit.Id = Data.Utils.GetDBInt32(dr["BusinessUnitId"]);
                    tradeInfo.Project.BusinessUnit.Name = Data.Utils.GetDBString(dr["BusinessUnitName"]);
                    tradeInfo.Project.BusinessUnit.ProjectNumberFormat = Data.Utils.GetDBString(dr["BusinessUnitProjectNumberFormat"]);

                    if (tradeInfo.Contract.ApprovalDate != null)
                    {
                        tradeInfo.Participations[0].Rank = 1;
                        tradeInfo.Participations[0].PulledOut = false;
                        tradeInfo.Participations[0].SubContractor = new SubContractorInfo();
                        tradeInfo.Participations[0].SubContractor.Name = Data.Utils.GetDBString(dr["SubcontractorShortName"]);
                    }

                    tradeInfoList.Add(tradeInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trades Report from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return tradeInfoList;
        }
        //#-----
        //To get the list of Contracts which dont have any uploaded signed contract file
        public DataTable GetContractsList_MissingSignedContractFile(String ProjectId, String BUsinessName)
        {

            List<Object> parameters = new List<Object>();
            DataTable dt = new DataTable();
            IDataReader dr = null;


            parameters.Add(ProjectId);
            parameters.Add(BUsinessName);
            try
            {
                dr = Data.DataProvider.GetInstance().GetMissingSignedContractFileContracts(parameters.ToArray());
                dt.Load(dr);

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Missing Signed Contract File Contracts from database");
            }

            return dt;
        }


        public DataTable GetCVStatusReportData(int ProjectId)
        {
            List<Object> parameters = new List<Object>();
            DataTable dt = new DataTable();
            IDataReader dr = null;


            parameters.Add(ProjectId);

            try
            {
                dr = Data.DataProvider.GetInstance().GetCVStatusReportData(parameters.ToArray());
                dt.Load(dr);

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Get CVStatusReportData from database");
            }

            return dt;

        }


        //#--Project Snapshot------
        public DataTable GetProjectSnapShot(ProjectInfo projectInfo)
        {
            IDataReader Dr = null;
            DataTable Ds = new DataTable();
            try
            {
                Dr = Data.DataProvider.GetInstance().GetProjectSnapShot(projectInfo.Id);
                if (Dr != null)
                    Ds.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting'Project Snap Shot from database");
            }



            return Ds;
        }


        public DataTable GetProjectCVSA(ProjectInfo projectInfo)
        {
            IDataReader Dr = null;
            DataTable Ds = new DataTable();
            try
            {
                Dr = Data.DataProvider.GetInstance().GetProjectCVSA(projectInfo.Id);
                if (Dr != null)
                    Ds.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting'Project CVSA from database");
            }



            return Ds;
        }



        //#--Report Turnover vS Time
        public DataTable GetTurnoverVsTime(List<ProjectInfo> projectInfoList, Dictionary<int, List<EmployeeInfo>> selectedUsers, DateTime? startDate, DateTime? endDate)
        {
            DataTable Dt = new DataTable();
            List<Object> parameters = new List<Object>();
            IDataReader Dr = null;



            StringBuilder projectIds = new StringBuilder();
            StringBuilder userIds = new StringBuilder();

            int projectId;


            projectIds.Append("<ProjectsIds>");
            userIds.Append("<ProjectsUserIds>");

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;


                projectIds.Append("<ProjectId><Id>").Append(projectInfo.IdStr).Append("</Id></ProjectId>");
                if (selectedUsers != null)
                {
                    if (selectedUsers[projectId] != null)
                        foreach (EmployeeInfo employeeInfo in selectedUsers[projectId])
                            userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(employeeInfo.IdStr).Append("</PeopleId></ProjectUserId>");
                }
                else if (selectedUsers == null)
                {
                    userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(projectInfo.ProjectManager.IdStr).Append("</PeopleId></ProjectUserId>");


                }
            }

            projectIds.Append("</ProjectsIds>");
            userIds.Append("</ProjectsUserIds>");

            parameters.Add(projectIds.ToString());
            parameters.Add(userIds.ToString());
            parameters.Add(startDate);
            parameters.Add(endDate);
            try
            {
                Dr = DataProvider.GetInstance().GetTurnOverVsTime(parameters.ToArray());
                if (Dr != null)
                    Dt.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on getting TurnoverVsTime report data from database");
            }

            return Dt;
        }



        //#--Report KPI Analysis
        public DataTable GetKPIAnalysis(List<ProjectInfo> projectInfoList, Dictionary<int, List<EmployeeInfo>> selectedUsers, DateTime? startDate, DateTime? endDate)
        {

            ProcessController processController = ProcessController.GetInstance();


            DataTable Dt = new DataTable();
            List<Object> parameters = new List<Object>();
            IDataReader Dr = null;


            List<String> rolesPlayList = null;
            HashSet<String> rolesPlayHashSet = null;

            StringBuilder projectIds = new StringBuilder();
            StringBuilder userIds = new StringBuilder();
            StringBuilder userTypes = new StringBuilder();



            int projectId;


            projectIds.Append("<ProjectsIds>");
            userIds.Append("<ProjectsUserIds>");
            userTypes.Append("<ProjectsUserTypes>");
            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;


                projectIds.Append("<ProjectId><Id>").Append(projectInfo.IdStr).Append("</Id></ProjectId>");
                if (selectedUsers != null)
                {
                    rolesPlayHashSet = new HashSet<String>();

                    if (selectedUsers[projectId] != null)
                        foreach (EmployeeInfo employeeInfo in selectedUsers[projectId])
                        {
                            userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(employeeInfo.IdStr).Append("</PeopleId></ProjectUserId>");

                            rolesPlayList = processController.GetAllRolesPlay(employeeInfo, projectInfo);

                            foreach (String role in rolesPlayList)
                                if (!rolesPlayHashSet.Contains(role))
                                    rolesPlayHashSet.Add(role);



                        }
                    foreach (String role in rolesPlayHashSet)
                        userTypes.Append("<ProjectUserType><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><UserType>").Append(role).Append("</UserType></ProjectUserType>");


                }
                else if (selectedUsers == null)
                {
                    userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(projectInfo.ProjectManager.IdStr).Append("</PeopleId></ProjectUserId>");


                }
            }

            projectIds.Append("</ProjectsIds>");
            userIds.Append("</ProjectsUserIds>");
            userTypes.Append("</ProjectsUserTypes>");

            parameters.Add(projectIds.ToString());
            parameters.Add(userTypes.ToString());
            parameters.Add(userIds.ToString());
            parameters.Add(startDate);
            parameters.Add(endDate);
            try
            {
                Dr = DataProvider.GetInstance().GetKPIAnalysis(parameters.ToArray());
                if (Dr != null)
                    Dt.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on getting KPI Analysis report data from database");
            }

            return Dt;
        }



        //# --Report Subcontractors Vv DV

        public DataTable GetSubcontractorsVvDV(int? projectId, int? subContractorId, int? businessUnitId, String tradeCode, String projectSatus, DateTime? startDate, DateTime? endDate)
        {
            DataTable Dt = new DataTable();
            IDataReader Dr = null;
            List<Object> parameters = new List<Object>();


            parameters.Add(projectId);
            parameters.Add(subContractorId);
            //parameters.Add(peopleId);
            parameters.Add(businessUnitId);
            parameters.Add(tradeCode);
            parameters.Add(projectSatus);
            parameters.Add(startDate);
            parameters.Add(endDate);

            try
            {
                Dr = DataProvider.GetInstance().GetSubcontractorsVvDV(parameters.ToArray());
                if (Dr != null)
                    Dt.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on getting GetSubcontractorsVvDV report data from database");
            }

            return Dt;

        }


        //#----


        //# --Report Subcontractors Vv DV

        public DataTable GetDesignVariation(int? projectId, int? businessUnitId, String tradeCode, String projectSatus, DateTime? startDate, DateTime? endDate)
        {
            DataTable Dt = new DataTable();
            IDataReader Dr = null;
            List<Object> parameters = new List<Object>();


            parameters.Add(projectId);
            //parameters.Add(subContractorId);
            //parameters.Add(peopleId);
            parameters.Add(businessUnitId);
            parameters.Add(tradeCode);
            parameters.Add(projectSatus);
            parameters.Add(startDate);
            parameters.Add(endDate);

            try
            {
                Dr = DataProvider.GetInstance().GetDesignVariation(parameters.ToArray());
                if (Dr != null)
                    Dt.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error on getting Design Variation report data from database");
            }

            return Dt;

        }


        //#----

        //#--DurationVsClaim
        public DataTable GetWorkingDaysInPercent(DateTime? StartDate, DateTime? RevisedDate, DateTime? practicalCompletionDate, int? ProjectId)
        {
            IDataReader Dr = null;
            DataTable Ds = new DataTable();
            List<Object> parameters = new List<Object>();

            parameters.Add(StartDate);
            parameters.Add(RevisedDate);
            parameters.Add(practicalCompletionDate);
            parameters.Add(ProjectId);
            try
            {
                Dr = Data.DataProvider.GetInstance().GetWorkingDaysInPercent(parameters.ToArray());
                if (Dr != null)
                    Ds.Load(Dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting'Project working days in percentage' from database");
            }



            return Ds;
        }
        //
        // Build TradeItemInfo List from Uploaded Excel Stream from Local Browser
        //
        public string TradeItemTemplateUpload(TradeItemCategoryInfo tradeItemCategory, System.Web.UI.WebControls.FileUpload FileUpload1)  //DS2023-11-10
        {
            //byte[] fileData = null;
            //// byte[] compressedData = null;
            //Stream fileStream = null;
            //int length = 0;

            //length = FileUpload1.PostedFile.ContentLength;
            //fileData = new byte[length + 1];
            //fileStream = FileUpload1.PostedFile.InputStream;
            //fileStream.Read(fileData, 0, length);
            List<TradeItemInfo> ListTradeItemInfo = tradeItemCategory.TradeItems;
            string Result = "Update Successfull!";
            byte[] fileData = null;
            Stream fileStream = null;
            int length = 0;
            int DisplayOrder = 0;

            foreach (TradeItemInfo tradeItemInfo in ListTradeItemInfo)
            {
                DisplayOrder = (int)tradeItemInfo.DisplayOrder;
            }

            length = FileUpload1.PostedFile.ContentLength;
            fileData = new byte[length + 1];
            fileStream = FileUpload1.PostedFile.InputStream;
            fileStream.Read(fileData, 0, length);
            List<List<string>> listExcel = GetExcelArray(fileData);
            //
            // VALIDATE
            //
            int Rows = listExcel.Count - 1;
            if (Rows <= 1) { return "No Data in Excel Sheet!"; }
            //
            List<string> listExccelH = listExcel[0];
            if (listExccelH.Count < 5) { return "Invalid Sheet Header - Please download!"; }
            if (listExccelH[0] != "Name") { return "Invalid Sheet Header - Please download!"; ; }
            if (listExccelH[1] != "Units") { return "Invalid Sheet Header - Please download!"; ; }
            if (listExccelH[2] != "ScopeOfWorks") { return "Invalid Sheet Header - Please download!"; ; }
            if (listExccelH[3] != "RequiresQuantityCheck") { return "Invalid Sheet Header - Please download!"; ; }
            if (listExccelH[4] != "RequiredInProposal") { return "Invalid Sheet Header - Please download!"; ; }
            int updates = 0;
            for (int Row = 1; Row <= Rows; Row++)
            {
                if (listExcel[Row].Count != 0)
                {
                    List<string> listExcelRow = listExcel[Row];
                    if (listExcelRow[0] != "")
                    {
                        if (listExcelRow.Count < 5) { return "Invalid Sheet Row " + Row.ToString() + " - Please download again!"; }
                        bool found = false;
                        foreach (TradeItemInfo tradeItemInfo in ListTradeItemInfo)
                        {
                            if (listExcelRow[0] == tradeItemInfo.Name) { found = true; }
                        }
                        if (found == false)
                        {
                            TradeItemInfo NewTradeItemInfo = new TradeItemInfo();
                            DisplayOrder++;
                            NewTradeItemInfo.RequiresQuantityCheck = false;
                            NewTradeItemInfo.RequiredInProposal = false;
                            NewTradeItemInfo.Name = listExcelRow[0];
                            NewTradeItemInfo.Units = listExcelRow[1];
                            NewTradeItemInfo.Scope = listExcelRow[2];
                            if (listExcelRow[3] == "Y") { NewTradeItemInfo.RequiresQuantityCheck = true; }
                            if (listExcelRow[4] == "Y") { NewTradeItemInfo.RequiredInProposal = true; }
                            NewTradeItemInfo.DisplayOrder = DisplayOrder;

                            NewTradeItemInfo.TradeItemCategory = TradesController.GetInstance().GetTradeItemCategory(tradeItemCategory.Id);
                            NewTradeItemInfo.TradeItemCategory.Trade.Participations = TradesController.GetInstance().GetTradeParticipations(NewTradeItemInfo.TradeItemCategory.Trade);
                            int Id = (int)TradesController.GetInstance().AddUpdateTradeItem(NewTradeItemInfo);
                            updates++;
                        }
                    }
                }
            }
            if (updates == 0) { return "Data already updated!"; }
            return updates.ToString() + " Items Imported!";

        }
        private void UpdateList(int ColNo, string ColValue, List<List<string>> Records)
        {
            List<string> Record = Records[Records.Count - 1];
            if (ColNo > Record.Count - 1)
            {
                Record.Add(ColValue);
            }
            else
            {
                Record[ColNo] = ColValue;
            }
            Records[Records.Count - 1] = Record;
        }
        public List<List<string>> GetExcelArray(byte[] data)
        {
            List<List<string>> listExcel = new List<List<string>>();
            try
            {
                string ColValue = "";
                bool isComplexField = false;
                int ColNo = 0;
                int RowNo = 0;
                listExcel.Add(new List<string> { });
                foreach (var b in data)
                {
                    byte[] byte1 = { b };
                    // Use Encoding.GetString to convert the byte array to a string
                    string resultString = Encoding.UTF8.GetString(data);
                    if (isComplexField == false)
                    {
                        switch (b)
                        {
                            case (byte)'\n':              // NEW ROW
                                UpdateList(ColNo, ColValue, listExcel);
                                RowNo += 1;
                                if (RowNo > 0)
                                {
                                    listExcel.Add(new List<string> { });
                                    ColValue = "";
                                    ColNo = 0;
                                }
                                break;
                            case (byte)'"':               // START OF SPECIAL FIELD
                                isComplexField = true;
                                break;
                            case (byte)'\r':              // IGNORE
                                break;
                            case (byte)',':               // NEW FIELD
                                isComplexField = false;
                                UpdateList(ColNo, ColValue, listExcel);
                                ColValue = "";
                                ColNo += 1;
                                break;
                            default:                     // COONCATENATE
                                ColValue += System.Text.Encoding.UTF8.GetString(byte1);
                                break;
                        }
                    }
                    else  // COMPLEX FIELD
                    {
                        if (b == '"')                        // END OF SPECIAL FIELD
                        {
                            isComplexField = false;
                        }
                        else
                        {
                            ColValue += Encoding.UTF8.GetString(byte1);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return listExcel;
        }

        static byte[] ConcatByteArrays(byte[] array1, byte[] array2)
        {
            byte[] newArray = new byte[array1.Length + array2.Length];
            Buffer.BlockCopy(array1, 0, newArray, 0, array1.Length);
            Buffer.BlockCopy(array2, 0, newArray, array1.Length, array2.Length);
            return newArray;
        }
        private string BuildCSVRow(String RowValue, String ColValue)
        {
            //  ’'
            if (ColValue == null) { ColValue = ""; }
            ColValue = ColValue.Replace('"', ',');
            ColValue = '"' + ColValue.Replace('’', '\'') + '"';
            if (RowValue == "")
            {
                return ColValue;
            }
            else
            {
                return RowValue + ',' + ColValue;
            }
        }
        //
        // Export TradeItemInfo List to Excel Stream Object -> Save to Local Browser 
        //
        public void TradeItemTemplateUpLoad(List<TradeItemInfo> TradeItemInfoList) //DS2023-11-10
        {
        }
        public void TradeItemTemplateDownLoad(List<TradeItemInfo> TradeItemInfoList, string ProjectName, string TradeName) //DS2023-11-10
        {
            try
            {
                // Your OpenXML code here
                // string filePath = @"d:\tmp\example.xlsx";
                // Create or edit Excel file
                // Your OpenXML code here
                //           MemoryStream stream1 = new MemoryStream();

                byte[] byteArray = new byte[0];
                string HeaderRow = "";
                HeaderRow = BuildCSVRow(HeaderRow, "Name");
                HeaderRow = BuildCSVRow(HeaderRow, "Units");
                HeaderRow = BuildCSVRow(HeaderRow, "ScopeOfWorks");
                HeaderRow = BuildCSVRow(HeaderRow, "RequiresQuantityCheck");
                HeaderRow = BuildCSVRow(HeaderRow, "RequiredInProposal");
                HeaderRow = HeaderRow + "\r\n";
                byteArray = ConcatByteArrays(byteArray, Encoding.UTF8.GetBytes(HeaderRow));
                if (TradeItemInfoList != null)
                {
                    foreach (TradeItemInfo tradeItemInfo in TradeItemInfoList)
                    {
                        string DataRow = "";
                        string RequiredInProposal = "N";
                        string RequiresQuantityCheck = "N";
                        if (tradeItemInfo.RequiredInProposal == true) { RequiredInProposal = "Y"; }
                        if (tradeItemInfo.RequiresQuantityCheck == true) { RequiresQuantityCheck = "Y"; }
                        Row row = new Row();
                        row.Append(BuildCell(tradeItemInfo.Name), BuildCell("Units"), BuildCell(tradeItemInfo.Scope), BuildCell(RequiresQuantityCheck), BuildCell(RequiredInProposal));
                        DataRow = BuildCSVRow(DataRow, tradeItemInfo.Name);
                        DataRow = BuildCSVRow(DataRow, tradeItemInfo.Units);
                        DataRow = BuildCSVRow(DataRow, tradeItemInfo.Scope);
                        DataRow = BuildCSVRow(DataRow, RequiresQuantityCheck);
                        DataRow = BuildCSVRow(DataRow, RequiredInProposal);
                        DataRow = DataRow + "\r\n";
                        byteArray = ConcatByteArrays(byteArray, Encoding.UTF8.GetBytes(DataRow));
                    }
                    string FileName = "TradeItemExport_" + ProjectName + "_" + TradeName + ".csv";
                    FileName = RemoveSpecialCharacters(FileName);
                    SOS.Web.Utils.SendFile(byteArray, FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            //string filePath = @"d:\tmp\example.xlsx";
            // Create or edit Excel file
        }
        static string RemoveSpecialCharacters(string filename)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string sanitizedFilename = new string(filename
                .Where(c => !invalidChars.Contains(c))
                .ToArray());

            return sanitizedFilename;
        }
        private Cell BuildCell(string strValue)
        {
            Cell cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(strValue)
            };
            return cell;
        }
        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;

            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));

                // Writer raw data                
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }

            return true;
        }
        #endregion


        //------------------------------------------------------------

        #endregion

    }
}
