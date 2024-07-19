using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Transactions;
using System.Collections.Generic;

using SOS.Data;
using System.Diagnostics.Contracts;

namespace SOS.Core
{
    public sealed class ContractsController : Controller
    {

#region Private Members
        private static ContractsController instance;
#endregion

#region Public Constants
        public const String TagVariable = "Variable";
        public const String TagVariableOpen = "[SOS:Variable:";
        public const String TagVariableClose = "/]";

        public const String TagEditableOpen = "[SOS:Editable:";
        public const String TagEditableEnd = "]";
        public const String TagEditableClose = "[/SOS:Editable]";

        public const String TagFooterOpen = "[SOS:Footer]";
        public const String TagFooterClose = "[/SOS:Footer]";

        public const String TagFinancialOpen = "[SOS:Financial]";
        public const String TagFinancialClose = "[/SOS:Financial]";

        public const String TagTermsOpen = "[SOS:Terms]";
        public const String TagTermsClose = "[/SOS:Terms]";
        
        public const String TagTitle = "Title:";
        public const Int32 TagMaxSize = 64;
#endregion

#region Private Constants
        private const String VariableToday = "Today";
        private const String VariableOrderNumber = "OrderNumber";
        private const String VariableProjectName = "ProjectName";
        private const String VariableProjectNumber = "ProjectNumber";
        private const String VariableJobNumber = "JobNumber";
        private const String VariableProjectYear = "ProjectYear";
        private const String VariableProjectSuburb = "ProjectSuburb";
        //#--
        private const String VariableProjectAddress = "ProjectAddress";
        private const String VariableProjectState = "ProjectState";
        private const String VariablePrincipalABN = "PrincipalABN";
        //#--

        private const String VariableProjectCommencementDate = "ProjectCommencementDate";
        private const String VariableProjectCompletionDate = "ProjectCompletionDate";
        private const String VariableCASignature = "CASignature";
        private const String VariablePMSignature = "PMSignature";
        private const String VariableCMSignature = "CMSignature";
        private const String VariableDCSignature = "DCSignature";
        private const String VariableDMSignature = "DMSignature";
        private const String VariableCAName = "CAName";
        private const String VariablePMName = "PMName";
        private const String VariableCMName = "CMName";
        private const String VariableDCName = "DCName";
        private const String VariableDMName = "DMName";
        private const String VariableCAPhone = "CAPhone";
        private const String VariableCAFax = "CAFax";
        private const String VariableCAEmail = "CAEmail";
        private const String VariableTradeName = "TradeName";
        private const String VariableTradeCode = "TradeCode";
        private const String VariableContractValue = "ContractValue";
        private const String VariableContractGST = "ContractGST";
        private const String VariableContractTotal = "ContractTotal";
        private const String VariableContratTotalWords = "ContratTotalWords";
        private const String VariableContractDate = "ContractDate";
        private const String VariableScopeOfWorks = "ScopeOfWorks";
        private const String VariableSiteAddress = "SiteAddress";
        private const String VariableSiteManager = "SiteManager";
        private const String VariableSitePhone = "SitePhone";
        private const String VariableDrawings = "Drawings";
        private const String VariableDrawingsSummary = "DrawingsSummary";
        private const String VariableCommencementDate = "CommencementDate";
        private const String VariableCompletionDate = "CompletionDate";
        private const String VariableDueDate = "DueDate";
        private const String VariablePaymentTerms = "PaymentTerms";     // DS2023-10-02
        private const String VariableDefectsLiability = "DefectsLiability";
        private const String VariableLiquidatedDamages = "LiquidatedDamages";
        private const String VariableSiteAllowances = "SiteAllowances";
        private const String VariableRetention = "Retention";
        private const String VariableRetentionToCertification = "RetentionToCertification";
        private const String VariableRetentionToLdp = "RetentionToLdp";
        private const String VariableInterest = "Interest";
        private const String VariablePreLettingDate = "PreLettingDate";
        private const String VariablePrincipal = "Principal";
        private const String VariableSubcontractorName = "SubcontractorName";
        private const String VariableSubcontractorABN = "SubcontractorABN";
        //#----
        private const String VariableSubcontractorACN = "SubcontractorACN";
        private const String VariableSubcontractorLiceneceNumber = "LiceneceNumber";
        private const String VariableSubcontractorEmail = "SubcontractorEmail";
        private const String VariableProjectLawOfSubcontract = "LawOfSubcontract";
        //#----
        private const String VariableSubcontractorAddress = "SubcontractorAddress";
        private const String VariableSubcontractorPhone = "SubcontractorPhone";
        private const String VariableSubcontractorFax = "SubcontractorFax";
        private const String VariableSubcontractorContact = "SubcontractorContact";
        private const String VariableSubcontractorContactPhone = "SubcontractorContactPhone";
        private const String VariableSubcontractorContactMobile = "SubcontractorContactMobile";
        private const String VariableSubcontractorContactEmail = "SubcontractorContactEmail";
        private const String VariableParticipationDueDate = "ParticipationDueDate";
        private const String VariableVariationOrder = "VariationOrder";
        private const String VariableSiteInstruction = "SiteInstruction";
        private const String VariableSiteInstructionDate = "SiteInstructionDate";
        private const String VariableSubcontractorReference = "SubcontractorReference";
        private const String VariableSubcontractorReferenceDate = "SubcontractorReferenceDate";
        private const String VariableVariationOrderTitle = "VariationOrderTitle";
        private const String VariableVariations = "Variations";
        private const String VariableVariationsTotal = "VariationsTotal";
        private const String VariableVariationsGST = "VariationsGST";
        private const String VariableVariationsTotalPlusGST = "VariationsTotalPlusGST";
        private const String VariableVariationsTrades = "VariationsTrades";
        private const String VariableTradesList = "TradesList";
        private const String VariableStartDate = "StartDate";       // DS2024-03-25
        private const String VariableFinishDate = "FinishDate";     // DS2024-03-25
        #endregion

        #region Private Methods
        private ContractsController()
        {
        }
#endregion

#region Public Methods

        public static ContractsController GetInstance()
        {
            if (instance == null)
                instance = new ContractsController();

            return instance;
        }

#region Job Type Methods
        /// <summary>
        /// Creates a Job Type from a dr
        /// </summary>
        public JobTypeInfo CreateJobType(IDataReader dr)
        {
            JobTypeInfo jobTypeInfo = new JobTypeInfo(Data.Utils.GetDBInt32(dr["JobTypeId"]));
            jobTypeInfo.Name = Data.Utils.GetDBString(dr["Name"]);

            AssignAuditInfo(jobTypeInfo, dr);

            return jobTypeInfo;
        }

        /// <summary>
        /// Creates a Job Type object from a dictionary
        /// </summary>
        public JobTypeInfo CreateJobType(Object dbId, Dictionary<int, JobTypeInfo> jobTypesDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (jobTypesDictionary == null)
                return GetInstance().GetJobType(Id);
            else if (jobTypesDictionary.ContainsKey(Id))
                return jobTypesDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Get a JobType from persistent storage
        /// </summary>
        public JobTypeInfo GetJobType(int? jobTypeId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetJobType(jobTypeId);
                if (dr.Read())
                    return CreateJobType(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Job Type from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get all the JobTypes from persistent storage sorted by name
        /// </summary>
        public List<JobTypeInfo> GetJobTypes()
        {
            IDataReader dr = null;
            List<JobTypeInfo> jobTypeInfoList = new List<JobTypeInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetJobTypes();
                while (dr.Read())
                    jobTypeInfoList.Add(CreateJobType(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Job Types from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return jobTypeInfoList;
        }

        /// <summary>
        /// Updates a JobType in the database
        /// </summary>
        public void UpdateJobType(JobTypeInfo jobTypeInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(jobTypeInfo);

            parameters.Add(jobTypeInfo.Id);
            parameters.Add(jobTypeInfo.Name);

            parameters.Add(jobTypeInfo.ModifiedDate);
            parameters.Add(jobTypeInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateJobType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating JobType in database");
            }
        }

        /// <summary>
        /// Sets the approval date
        /// </summary>
        public void SetContractApprovalDate(ContractInfo contractInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(contractInfo.Id);
            parameters.Add(contractInfo.ApprovalDate);

            try
            {
                Data.DataProvider.GetInstance().SetContractApprovalDate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Setting Contract Approval Date in database");
            }
        }
        
        /// <summary>
        /// Adds a JobType to the database
        /// </summary>
        public int? AddJobType(JobTypeInfo jobTypeInfo)
        {
            int? jobTypeId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(jobTypeInfo);

            parameters.Add(jobTypeInfo.Name);

            parameters.Add(jobTypeInfo.CreatedDate);
            parameters.Add(jobTypeInfo.CreatedBy);

            try
            {
                jobTypeId = Data.DataProvider.GetInstance().AddJobType(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding JobType to database");
            }

            return jobTypeId;
        }

        /// <summary>
        /// Adds or updates a JobType
        /// </summary>
        public int? AddUpdateJobType(JobTypeInfo jobTypeInfo)
        {
            if (jobTypeInfo != null)
            {
                if (jobTypeInfo.Id != null)
                {
                    UpdateJobType(jobTypeInfo);
                    return jobTypeInfo.Id;
                }
                else
                {
                    return AddJobType(jobTypeInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a JobType from persistent storage
        /// </summary>
        public void DeleteJobType(JobTypeInfo jobTypeInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteJobType(jobTypeInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing JobType from database. It might be in use.");
            }
        }
#endregion

#region WorkOrders Methods
        /// <summary>
        /// Creates a new workr order
        /// </summary>
        public String CreateWorkOrder(BusinessUnitInfo businessUnitInfo)
        {
            int? workOrderId = null;
            String strWorkOrderId = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(DateTime.Now);
            parameters.Add(Web.Utils.GetCurrentUserId());

            try
            {
                workOrderId = Data.DataProvider.GetInstance().AddWorkOrder(parameters.ToArray());
                strWorkOrderId = workOrderId.ToString();
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Creating Work Order in database");
            }

            return strWorkOrderId;
        }
#endregion

#region Business Unit Methods
        /// <summary>
        /// Creates a Business Unit from a dr
        /// </summary>
        public BusinessUnitInfo CreateBusinessUnit(IDataReader dr, Dictionary<int, PeopleInfo> peopleDictionary)
        {
            BusinessUnitInfo businessUnitInfo = new BusinessUnitInfo(Data.Utils.GetDBInt32(dr["BusinessUnitId"]));            

            businessUnitInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            businessUnitInfo.ProjectNumberFormat = Data.Utils.GetDBString(dr["ProjectNumberFormat"]);
            businessUnitInfo.TradeOverbudgetApproval = Data.Utils.GetDBDecimal(dr["TradeOverbudgetApproval"]);
            businessUnitInfo.TradeAmountApproval = Data.Utils.GetDBDecimal(dr["TradeAmountApproval"]);
            //#---
            businessUnitInfo.TradeComAmountApproval = Data.Utils.GetDBDecimal(dr["TradeComAmountApproval"]);
            businessUnitInfo.TradeComOverBudget = Data.Utils.GetDBInt32(dr["TradeComOverBudget"]);
            businessUnitInfo.TradeDAAmountApproval = Data.Utils.GetDBDecimal(dr["TradeDAAmountApproval"]);
            businessUnitInfo.TradeUMOverbudgetApproval = Data.Utils.GetDBDecimal(dr["TradeUMOverbudgetApproval"]);
            businessUnitInfo.VariationUMDAOverApproval = Data.Utils.GetDBDecimal(dr["VariationUMDAOverAmtApproval"]);
            if (dr["EDPeopleId"] != DBNull.Value)
                businessUnitInfo.EstimatingDirector = (EmployeeInfo)PeopleController.GetInstance().CreatePerson(dr["EDPeopleId"], peopleDictionary);

            //#--

            businessUnitInfo.ClaimSpecialNote = Data.Utils.GetDBString(dr["ClaimSpecialNote"]);
            businessUnitInfo.VariationSepAccUMApproval = Data.Utils.GetDBDecimal(dr["VariationSepAccUMApproval"]);   //DS20231005
            businessUnitInfo.VariationUMBOQVCVDVApproval = Data.Utils.GetDBDecimal(dr["VariationUMBOQVCVDVApproval"]);   //DS20231124
            AssignAuditInfo(businessUnitInfo, dr);

            if (dr["UMPeopleId"] != DBNull.Value) 
                businessUnitInfo.UnitManager = (EmployeeInfo)PeopleController.GetInstance().CreatePerson(dr["UMPeopleId"], peopleDictionary);

            return businessUnitInfo;
        }

        /// <summary>
        /// Creates a Business Unit from a dr
        /// </summary>
        public BusinessUnitInfo CreateBusinessUnit(IDataReader dr)
        {
            return CreateBusinessUnit(dr, null);
        }

        /// <summary>
        /// Creates a CreateBusinessUnit object from a dr or retrieve it from the dictionary
        /// </summary>
        public BusinessUnitInfo CreateBusinessUnit(Object dbId, Dictionary<int, BusinessUnitInfo> businessUnitDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (businessUnitDictionary == null)
                return GetInstance().GetBusinessUnit(Id);
            else if (businessUnitDictionary.ContainsKey(Id))
                return businessUnitDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Get a Business Unit from persistent storage
        /// </summary>
        public BusinessUnitInfo GetBusinessUnit(int? businessUnitId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetBusinessUnit(businessUnitId);
                if (dr.Read())
                    return CreateBusinessUnit(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Business Unit from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Gets a business unit including its process templates
        /// </summary>
        public BusinessUnitInfo GetDeepBusinessUnit(int? businessUnitId)
        {
            BusinessUnitInfo businessUnitInfo = GetBusinessUnit(businessUnitId);
            businessUnitInfo.ProcessTemplates = ProcessController.GetInstance().GetProcessTemplates(businessUnitInfo);
            return businessUnitInfo;
        }

        /// <summary>
        /// Get all the Business Units from persistent storage sorted by name
        /// </summary>
        public List<BusinessUnitInfo> GetBusinessUnits()
        {
            IDataReader dr = null;
            List<BusinessUnitInfo> BusinessUnitInfoList = new List<BusinessUnitInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetBusinessUnits();
                while (dr.Read())
                    BusinessUnitInfoList.Add(CreateBusinessUnit(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Business Unit from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return BusinessUnitInfoList;
        }

        /// <summary>
        /// Updates a Business Unit in the database
        /// </summary>
        public void UpdateBusinessUnit(BusinessUnitInfo businessUnitInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(businessUnitInfo);

            parameters.Add(businessUnitInfo.Id);
            parameters.Add(businessUnitInfo.Name);
            parameters.Add(businessUnitInfo.ProjectNumberFormat);
            parameters.Add(GetId(businessUnitInfo.UnitManager));
            parameters.Add(businessUnitInfo.TradeOverbudgetApproval);
            parameters.Add(businessUnitInfo.TradeAmountApproval);
            //#---
            parameters.Add(businessUnitInfo.TradeComAmountApproval);
            parameters.Add(businessUnitInfo.TradeComOverBudget);
            parameters.Add(businessUnitInfo.TradeDAAmountApproval);
            parameters.Add(businessUnitInfo.TradeUMOverbudgetApproval);
            parameters.Add(businessUnitInfo.VariationUMDAOverApproval);
            parameters.Add(GetId(businessUnitInfo.EstimatingDirector));
            //#--

            parameters.Add(businessUnitInfo.ClaimSpecialNote);
            parameters.Add(businessUnitInfo.VariationSepAccUMApproval);    //DS20231005
            parameters.Add(businessUnitInfo.VariationUMBOQVCVDVApproval);    //DS20231124
            parameters.Add(businessUnitInfo.ModifiedDate);
            parameters.Add(businessUnitInfo.ModifiedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateBusinessUnit(parameters.ToArray());

                    Data.DataProvider.GetInstance().DeleteProcessTemplates(businessUnitInfo.Id);

                    foreach (ProcessTemplateInfo processTemplateInfo in businessUnitInfo.ProcessTemplates)
                    {
                        parameters.Clear();

                        parameters.Add(businessUnitInfo.Id);
                        parameters.Add(GetId(processTemplateInfo.JobType));
                        parameters.Add(processTemplateInfo.Id);

                        Data.DataProvider.GetInstance().AddProcessTemplate(parameters.ToArray());
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Business Unit in database");
            }
        }

        /// <summary>
        /// Adds a Business Unit to the database
        /// </summary>
        public int? AddBusinessUnit(BusinessUnitInfo businessUnitInfo)
        {
            int? BusinessUnitId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(businessUnitInfo);

            parameters.Add(businessUnitInfo.Name);
            parameters.Add(businessUnitInfo.ProjectNumberFormat);
            parameters.Add(GetId(businessUnitInfo.UnitManager));
            parameters.Add(businessUnitInfo.TradeOverbudgetApproval);
            parameters.Add(businessUnitInfo.TradeAmountApproval);
            //#-----
                parameters.Add(businessUnitInfo.TradeComAmountApproval);
                parameters.Add(businessUnitInfo.TradeComOverBudget);
                parameters.Add(businessUnitInfo.TradeDAAmountApproval);
                parameters.Add(businessUnitInfo.TradeUMOverbudgetApproval);
                parameters.Add(businessUnitInfo.VariationUMDAOverApproval);
                parameters.Add(GetId(businessUnitInfo.EstimatingDirector));
            //#---
            parameters.Add(businessUnitInfo.ClaimSpecialNote);
            parameters.Add(businessUnitInfo.VariationSepAccUMApproval);    //DS20231005
            parameters.Add(businessUnitInfo.VariationUMBOQVCVDVApproval);    //DS20231124
            parameters.Add(businessUnitInfo.CreatedDate);
            parameters.Add(businessUnitInfo.CreatedBy);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    BusinessUnitId = Data.DataProvider.GetInstance().AddBusinessUnit(parameters.ToArray());

                    foreach (ProcessTemplateInfo processTemplateInfo in businessUnitInfo.ProcessTemplates)
                    {
                        parameters.Clear();

                        parameters.Add(BusinessUnitId);
                        parameters.Add(GetId(processTemplateInfo.JobType));
                        parameters.Add(processTemplateInfo.Id);

                        Data.DataProvider.GetInstance().AddProcessTemplate(parameters.ToArray());
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Business Unit to database");
            }

            return BusinessUnitId;
        }

        /// <summary>
        /// Adds or updates a BusinessUnit
        /// </summary>
        public int? AddUpdateBusinessUnit(BusinessUnitInfo BusinessUnitInfo)
        {
            if (BusinessUnitInfo != null)
            {
                if (BusinessUnitInfo.Id != null)
                {
                    UpdateBusinessUnit(BusinessUnitInfo);
                    return BusinessUnitInfo.Id;
                }
                else
                {
                    return AddBusinessUnit(BusinessUnitInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a BusinessUnit from persistent storage
        /// </summary>
        public void DeleteBusinessUnit(BusinessUnitInfo BusinessUnitInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteBusinessUnit(BusinessUnitInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Business Unit from database. It might be in use.");
            }
        }
        #endregion

        //#------
        #region Contract Approval Limit Methods


          public void UpdateContractApprovalLimit(ContractApprovalLimitInfo CALInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(CALInfo);

            parameters.Add(CALInfo.Id);
            parameters.Add(CALInfo.Min);
            parameters.Add(CALInfo.Max);
            parameters.Add(CALInfo.ModifiedBy);
            parameters.Add(CALInfo.ModifiedDate);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    Data.DataProvider.GetInstance().UpdateContractApprovalLimit(parameters.ToArray());

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Contract Approval Limit in database");
            }




        }


        public ContractApprovalLimitInfo CreateContractApprovalLimit(IDataReader dr )
        {
            ContractApprovalLimitInfo contractApprovalLimit = new ContractApprovalLimitInfo(Data.Utils.GetDBInt32(dr["Id"]));
            contractApprovalLimit.Min = Data.Utils.GetDBInt32(dr["min"]).Value;
            contractApprovalLimit.Max = Data.Utils.GetDBInt32(dr["max"]).Value;

            contractApprovalLimit.WinOver5 = Data.Utils.GetDBString(dr["WinOver5"]);
            contractApprovalLimit.Winlessthan5 = Data.Utils.GetDBString(dr["Winlessthan5"]);

            contractApprovalLimit.Lossless5 = Data.Utils.GetDBString(dr["Losslessthan5"]);
            contractApprovalLimit.Lossbtw5to50 = Data.Utils.GetDBString(dr["Lossbtw5to50"]);
            contractApprovalLimit.LossOver50 = Data.Utils.GetDBString(dr["LossOver50"]);
          
            return contractApprovalLimit;
        }

        public List<ContractApprovalLimitInfo> GetContractApprovalLimits()
        {
            IDataReader dr = null;
            List<ContractApprovalLimitInfo> ContractApprovalLimitList = new List<ContractApprovalLimitInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetContractApprovalLimits();
                while (dr.Read())
                    ContractApprovalLimitList.Add(CreateContractApprovalLimit(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract Approval Limits from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return ContractApprovalLimitList;
        }

        #endregion
        //#----


        #region Contracts Templates Methods
        /// <summary>
        /// Creates a Contract Template from a dr
        /// </summary>
        public ContractTemplateInfo CreateContractTemplate(IDataReader dr)
        {
            ContractTemplateInfo contractTemplateInfo = new ContractTemplateInfo(Data.Utils.GetDBInt32(dr["ContractTemplateId"]));

            contractTemplateInfo.StandardTemplate = Data.Utils.GetDBString(dr["StandardTemplate"]);
            contractTemplateInfo.SimplifiedTemplate = Data.Utils.GetDBString(dr["SimplifiedTemplate"]);
            contractTemplateInfo.VariationTemplate = Data.Utils.GetDBString(dr["VariationTemplate"]);

            AssignAuditInfo(contractTemplateInfo, dr);

            if (dr["JobTypeId"] != DBNull.Value) 
                contractTemplateInfo.JobType = GetJobType(Data.Utils.GetDBInt32(dr["JobTypeId"]));
            
            if (dr["BusinessUnitId"] != DBNull.Value)
                contractTemplateInfo.BusinessUnit = GetBusinessUnit(Data.Utils.GetDBInt32(dr["BusinessUnitId"]));

            return contractTemplateInfo;
        }

        /// <summary>
        /// Get a contract template from persisten storage
        /// </summary>
        public ContractTemplateInfo GetContractTemplate(int? ContractTemplateId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetContractTemplate(ContractTemplateId);
                if (dr.Read())
                    return CreateContractTemplate(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract Template from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            return null;
        }

        /// <summary>
        /// Get a contract template from persisten storage
        /// </summary>
        public ContractTemplateInfo GetContractTemplate(TradeInfo tradeInfo)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetContractTemplate(tradeInfo.JobType.Id, tradeInfo.Project.BusinessUnit.Id);
                if (dr.Read())
                    return CreateContractTemplate(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract Template from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            return null;
        }

        /// <summary>
        /// Get all the contract templates from persistent storage
        /// </summary>
        public List<ContractTemplateInfo> GetContractTemplates()
        {
            IDataReader dr = null;
            List<ContractTemplateInfo> contractTemplateInfoList = new List<ContractTemplateInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetContractTemplates();
                while (dr.Read())
                    contractTemplateInfoList.Add(CreateContractTemplate(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract Templates from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return contractTemplateInfoList;
        }

        /// <summary>
        /// Updates a contract template in the database
        /// </summary>
        public void UpdateContractTemplate(ContractTemplateInfo contractTemplateInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(contractTemplateInfo);

            parameters.Add(contractTemplateInfo.Id);
            parameters.Add(contractTemplateInfo.StandardTemplate);
            parameters.Add(contractTemplateInfo.SimplifiedTemplate);
            parameters.Add(contractTemplateInfo.VariationTemplate);

            parameters.Add(contractTemplateInfo.ModifiedDate);
            parameters.Add(contractTemplateInfo.ModifiedBy);
            try
            {
                Data.DataProvider.GetInstance().UpdateContractTemplate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Contract Template in database");
            }
        }

        /// <summary>
        /// Adds a contract template to the database
        /// </summary>
        public int? AddContractTemplate(ContractTemplateInfo contractTemplateInfo)
        {
            int? contractTemplateId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(contractTemplateInfo);
            
            parameters.Add(GetId(contractTemplateInfo.JobType));
            parameters.Add(GetId(contractTemplateInfo.BusinessUnit));
            parameters.Add(contractTemplateInfo.StandardTemplate);
            parameters.Add(contractTemplateInfo.SimplifiedTemplate);
            parameters.Add(contractTemplateInfo.VariationTemplate);

            parameters.Add(contractTemplateInfo.CreatedDate);
            parameters.Add(contractTemplateInfo.CreatedBy);

            try
            {
                contractTemplateId = Data.DataProvider.GetInstance().AddContractTemplate(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Contract Template to database");
            }
            return contractTemplateId;
        }

        /// <summary>
        /// Adds or Updates a Contract Template in the database
        /// </summary>
        public int? AddUpdateContractTemplate(ContractTemplateInfo contractTemplateInfo)
        {
            if (contractTemplateInfo != null)
            {
                if (contractTemplateInfo.Id != null)
                {
                    UpdateContractTemplate(contractTemplateInfo);
                    return contractTemplateInfo.Id;
                }
                else
                {
                    return AddContractTemplate(contractTemplateInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Contract Template from persistent storage
        /// </summary>
        public void DeleteContractTemplate(ContractTemplateInfo ContractTemplateInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteContractTemplate(ContractTemplateInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Contract Template from database");
            }
        }

        /// <summary>
        /// Validates a template format. Returns error found or null if no error.
        /// </summary>
        public String ValidateTemplate(String template)
        {
            List<String> templateEditables = new List<String>();
            String lastValidEditable = "None";
            String lastValidVariable = "None";
            String errorMsg = null;
            Int32 numEditable = 0;
            Int32 numVariable = 0;
            Int32 templateLegth;
            Int32 currPos = 0;
            Int32 openPos;
            Int32 endPos;
            Int32 tagPos;
            Int32 nextPos;

            if (template != null)
            {
                templateLegth = template.Length;
                while (currPos < templateLegth && errorMsg == null)
                {
                    tagPos = template.IndexOf(TagEditableOpen, currPos);
                    if (tagPos >= 0)
                    {
                        openPos = tagPos;
                        currPos = tagPos + TagEditableOpen.Length;
                        tagPos = template.IndexOf(TagTitle, currPos);
                        if (tagPos >= 0 && tagPos - currPos < TagMaxSize)
                        {
                            currPos = tagPos + TagTitle.Length;
                            tagPos = template.IndexOf(TagEditableEnd, currPos);
                            if (tagPos >= 0 && tagPos - currPos < TagMaxSize)
                            {
                                endPos = tagPos;
                                currPos = tagPos + TagEditableEnd.Length;
                                tagPos = template.IndexOf(TagEditableClose, currPos);
                                nextPos = template.IndexOf(TagEditableOpen, currPos);
                                if (tagPos >= 0 && (nextPos == -1 || nextPos > tagPos))
                                {
                                    currPos = tagPos + TagEditableClose.Length;
                                    lastValidEditable = template.Substring(openPos, endPos - openPos + 1);

                                    numEditable = numEditable + 1;
                                }
                                else
                                    errorMsg = "Close tag '" + TagEditableClose + "' not found on editable section No: " + Convert.ToString(numEditable + 1);
                            }
                            else
                                errorMsg = "End tag '" + TagEditableEnd + "' not found on editable section No: " + Convert.ToString(numEditable + 1);
                        }
                        else
                            errorMsg = "Title tag '" + TagTitle + "' not found on editable section No: " + Convert.ToString(numEditable + 1);
                    }
                    else
                        currPos = templateLegth;
                }

                if (errorMsg != null)
                    errorMsg = errorMsg + ". Last valid editable section: " + lastValidEditable;
                else
                {
                    currPos = 0;
                    while (currPos < templateLegth && errorMsg == null)
                    {
                        {
                            tagPos = template.IndexOf(TagVariableOpen, currPos);
                            if (tagPos >= 0)
                            {
                                openPos = tagPos;
                                currPos = tagPos + TagVariableOpen.Length;
                                tagPos = template.IndexOf(TagTitle, currPos);
                                if (tagPos >= 0 && tagPos - currPos < TagMaxSize)
                                {
                                    currPos = tagPos + TagTitle.Length;
                                    tagPos = template.IndexOf(TagVariableClose, currPos);
                                    if (tagPos >= 0 && tagPos - currPos < TagMaxSize)
                                    {
                                        currPos = tagPos + TagVariableClose.Length;
                                        lastValidVariable = template.Substring(openPos, currPos - openPos);

                                        numVariable = numVariable + 1;
                                    }
                                    else
                                        errorMsg = "Close tag '" + TagVariableClose + "' not found for variable No: " + Convert.ToString(numVariable + 1);
                                }
                                else
                                    errorMsg = "Title tag '" + TagTitle + "' not found for variable No: " + Convert.ToString(numVariable + 1);
                            }
                            else
                                currPos = templateLegth;
                        }
                    }

                    if (errorMsg != null)
                        errorMsg = errorMsg + ". Last valid variable section: " + lastValidVariable;
                }

                if (errorMsg == null)
                {
                    templateEditables = GetTemplateEditables(template);
                    for (int i = 0; i < templateEditables.Count - 1; i++)
                    {
                        for (int j = i + 1; j < templateEditables.Count; j++)
                        {
                            if (GetTemplateEditableName(templateEditables[i]) == GetTemplateEditableName(templateEditables[j]))
                            {
                                return "Duplicated editable section name: " + templateEditables[i];
                            }
                        }
                    }
                }
            }

            return errorMsg;
        }

        /// <summary>
        /// Returns an array list of all editables section in a template
        /// </summary>
        public List<String> GetTemplateEditables(String template)
        {
            List<String> templateEditables = new List<String>();
            Int32 templateLegth;
            Int32 currPos = 0;
            Int32 openPos;
            Int32 endPos;
            Int32 tagNameAndTitleMaxSize = 2 * TagMaxSize;
                        
            if (template != null)
            {
                templateLegth = template.Length;
                while (currPos < templateLegth)
                {
                    openPos = template.IndexOf(TagEditableOpen, currPos);
                    if (openPos >= 0)
                    {
                        endPos = template.IndexOf(TagEditableEnd, openPos + TagEditableOpen.Length);
                        if (endPos >= 0 && endPos - openPos < tagNameAndTitleMaxSize)
                        {
                            currPos = endPos + TagEditableEnd.Length;
                            templateEditables.Add(template.Substring(openPos, currPos - openPos));
                        }
                        else
                        {
                            currPos = templateLegth;
                        }
                    }
                    else
                    {
                        currPos = templateLegth;
                    }
                }
            }

            return templateEditables;
        }

        /// <summary>
        /// Returns an array list of all variables section in a template
        /// </summary>
        public List<String> GetTemplateVariables(String template)
        {
            List<String> templateVariables = new List<String>();
            Int32 tagNameAndTitleMaxSize = 2 * TagMaxSize;
            Int32 templateLegth;
            Int32 currPos = 0;
            Int32 openPos;
            Int32 endPos;

            if (template != null)
            {
                templateLegth = template.Length;
                while (currPos < templateLegth)
                {
                    openPos = template.IndexOf(TagVariableOpen, currPos);
                    if (openPos >= 0)
                    {
                        endPos = template.IndexOf(TagVariableClose, openPos + TagVariableOpen.Length);
                        if (endPos >= 0 && endPos - openPos < tagNameAndTitleMaxSize)
                        {
                            currPos = endPos + TagVariableClose.Length;
                            templateVariables.Add(template.Substring(openPos, currPos - openPos));
                        }
                        else
                        {
                            currPos = templateLegth;
                        }
                    }
                    else
                    {
                        currPos = templateLegth;
                    }
                }
            }

            return templateVariables;
        }

        /// <summary>
        /// Gets the Name from an editable section.[SOS:Editable:AdminCharges Title:Administration Charges]
        /// </summary>
        public String GetTemplateEditableName(String editableSection)
        {
            Int32 titlePos = editableSection.IndexOf(TagTitle);
            if (titlePos >= 0)
                return editableSection.Substring(TagEditableOpen.Length, titlePos - TagEditableOpen.Length - 1).Trim();
            else
                return null;
        }

        /// <summary>
        /// Gets the Title from an editable section.
        /// </summary>
        public String GetTemplateEditableTitle(String editableSection)
        {
            Int32 titlePos = editableSection.IndexOf(TagTitle);
            if (titlePos >= 0)
            {
                Int32 endPos = editableSection.IndexOf(TagEditableEnd, titlePos + TagTitle.Length);
                if (endPos >= 0)
                    return editableSection.Substring(titlePos + TagTitle.Length, endPos - titlePos - TagTitle.Length);
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the Title from an editable section name.
        /// </summary>
        public String GetTemplateEditableTitle(List<String> editableSections, String sectionName)
        {
            foreach (String editableSection in editableSections)
                if (GetTemplateEditableName(editableSection) == sectionName)
                    return GetTemplateEditableTitle(editableSection);

            return String.Empty;
        }
               
        /// <summary>
        /// Gets the Name from an variable section.
        /// </summary>
        public String GetTemplateVariableName(String variableSection)
        {
            Int32 titlePos = variableSection.IndexOf(TagTitle);
            if (titlePos >= 0)
                return variableSection.Substring(TagVariableOpen.Length, titlePos - TagVariableOpen.Length - 1).Trim();
            else
                return null;
        }

        /// <summary>
        /// Gets the Title from an variable section.
        /// </summary>
        public String GetTemplateVariableTitle(String variableSection)
        {
            Int32 titlePos = variableSection.IndexOf(TagTitle);
            if (titlePos >= 0)
            {
                Int32 endPos = variableSection.IndexOf(TagVariableClose, titlePos + TagTitle.Length);
                if (endPos >= 0)
                    return variableSection.Substring(titlePos + TagTitle.Length, endPos - titlePos - TagTitle.Length);
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the whole section corresponding to the editableSection
        /// </summary>
        public String GetTemplateEditableFullSection(String template, String editableSection)
        {
            String fullSection = null;
            Int32 openPos;
            Int32 endPos;

            openPos = template.IndexOf(editableSection);
            if (openPos >= 0)
            {
                endPos = template.IndexOf(TagEditableClose, openPos + editableSection.Length);
                if (endPos >= 0)
                    fullSection = template.Substring(openPos, endPos - openPos + TagEditableClose.Length);
            }

            return fullSection;
        }

        /// <summary>
        /// Gets the text of an editable section
        /// </summary>
        public String GetTemplateEditableTextSection(String template, String editableSection)
        {
            String textSection = null;
            Int32 openPos;
            Int32 endPos;

            openPos = template.IndexOf(editableSection);
            if (openPos >= 0)
            {
                endPos = template.IndexOf(TagEditableClose, openPos + editableSection.Length);
                if (endPos >= 0)
                    textSection = template.Substring(openPos + editableSection.Length, endPos - openPos - editableSection.Length);
            }

            return textSection;
        }

        /// <summary>
        /// Gets a full section from a template
        /// </summary>
        public String GetTemplateSectionFull(String template, String tagOpen, String tagClose)
        {
            String fullSection = null;
            Int32 openPos;
            Int32 endPos;

            openPos = template.IndexOf(tagOpen);
            if (openPos >= 0)
            {
                endPos = template.IndexOf(tagClose, openPos + tagOpen.Length);
                if (endPos >= 0)
                    fullSection = template.Substring(openPos, endPos - openPos + tagClose.Length);
            }

            return fullSection;
        }

        /// <summary>
        /// Gets the section text part from a template
        /// </summary>
        public String GetTemplateSectionText(String template, String tagOpen, String tagClose)
        {
            String textSection = null;
            Int32 openPos;
            Int32 endPos;

            openPos = template.IndexOf(tagOpen);
            if (openPos >= 0)
            {
                endPos = template.IndexOf(tagClose, openPos + tagOpen.Length);
                if (endPos >= 0)
                    textSection = template.Substring(openPos + tagOpen.Length, endPos - openPos - tagOpen.Length);
            }

            return textSection;
        }

        /// <summary>
        /// Gets the footer from a template
        /// </summary>
        public String GetTemplateFooterFull(String template)
        {
            return GetTemplateSectionFull(template, TagFooterOpen, TagFooterClose);
        }

        /// <summary>
        /// Gets the financial information section from a template
        /// </summary>
        public String GetTemplateFinancialFull(String template)
        {
            return GetTemplateSectionFull(template, TagFinancialOpen, TagFinancialClose);
        }

        /// <summary>
        /// Gets the terms information section from a template
        /// </summary>
        public String GetTemplateTermsFull(String template)
        {
            return GetTemplateSectionFull(template, TagTermsOpen, TagTermsClose);
        }

        /// <summary>
        /// Gets the footer text part from a template
        /// </summary>
        public String GetTemplateFooterText(String template)
        {
            return GetTemplateSectionText(template, TagFooterOpen, TagFooterClose);
        }

        /// <summary>
        /// Gets the financial text part from a template
        /// </summary>
        public String GetTemplateFinancialText(String template)
        {
            return GetTemplateSectionText(template, TagFinancialOpen, TagFinancialClose);
        }

        /// <summary>
        /// Gets the terms text part from a template
        /// </summary>
        public String GetTemplateTermsText(String template)
        {
            return GetTemplateSectionText(template, TagTermsOpen, TagTermsClose);
        }

        /// <summary>
        /// Returns a string to view a template
        /// </summary>
        public String ViewTemplate(String template)
        {
            if (template == null)
                return String.Empty;

            List<String> templateEditables = GetTemplateEditables(template);
            List<String> templateVariables = GetTemplateVariables(template);
            String templateFooterFull = GetTemplateFooterFull(template);
            String templateFooterText = GetTemplateFooterText(template);
            String templateFinancialFull = GetTemplateFinancialFull(template);
            String templateFinancialText = GetTemplateFinancialText(template);
            String templateTermsFull = GetTemplateTermsFull(template);
            String templateTermsText = GetTemplateTermsText(template);

            StringBuilder stringBuilder = new StringBuilder(template);

            String openEditable = "<span class='TemplateEditable'>&nbsp;<img src='" + Web.Utils.GetEditImage() + "' alt='{0}'>&nbsp;";
            String closeEditable = "&nbsp;</span>";
            String openVariable = "<span class='TemplateVariable'>&nbsp;";
            String closeVariable = "&nbsp;</span>";
            String openFooter = "<span class='TemplateFooter'>";
            String closeFooter = "</span>";
            String openFinancial = "<span class='TemplateFinancial'>";
            String closeFinancial = "</span>";
            String openTerms = "<span class='TemplateTerms'>";
            String closeTerms = "</span>";

            stringBuilder.Replace(TagEditableClose, closeEditable);

            if (templateFooterFull != null)
                stringBuilder.Replace(templateFooterFull, openFooter + templateFooterText + closeFooter);

            if (templateFinancialFull != null)
                stringBuilder.Replace(templateFinancialFull, openFinancial + templateFinancialText + closeFinancial);

            if (templateTermsFull != null)
                stringBuilder.Replace(templateTermsFull, openTerms + templateTermsText + closeTerms);

            foreach (String templateEditable in templateEditables)
                stringBuilder.Replace(templateEditable, openEditable.Replace("{0}", GetTemplateEditableTitle(templateEditable)));

            foreach (String templateVariable in templateVariables)
                stringBuilder.Replace(templateVariable, openVariable + GetTemplateVariableTitle(templateVariable) + closeVariable);
            
            return stringBuilder.ToString();
        }
#endregion

        #region Contract Methods
        /// <summary>
        /// Creates a Contract from a dr and dictionaries
        /// </summary>
        public ContractInfo CreateContract(IDataReader dr, Dictionary<int, ProcessInfo> processesDictionary)
        {
            ContractInfo contractInfo = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

            contractInfo.Template = Data.Utils.GetDBString(dr["Template"]);
            contractInfo.Number = Data.Utils.GetDBString(dr["Number"]);
            contractInfo.WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);
            contractInfo.ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);
            contractInfo.CheckQuotes = Data.Utils.GetDBBoolean(dr["CheckQuotes"]);
            contractInfo.CheckWinningQuote = Data.Utils.GetDBBoolean(dr["CheckWinningQuote"]);
            contractInfo.CheckComparison = Data.Utils.GetDBBoolean(dr["CheckComparison"]);
            contractInfo.CheckCheckList = Data.Utils.GetDBBoolean(dr["CheckCheckList"]);
            contractInfo.CheckPrelettingMinutes = Data.Utils.GetDBBoolean(dr["CheckPrelettingMinutes"]);
            contractInfo.CheckAmendments = Data.Utils.GetDBBoolean(dr["CheckAmendments"]);
            contractInfo.SubcontractNumber = Data.Utils.GetDBInt32(dr["SubcontractNumber"]);
            contractInfo.SiteInstruction = Data.Utils.GetDBString(dr["SiteInstruction"]);
            contractInfo.SiteInstructionDate = Data.Utils.GetDBDateTime(dr["SiteInstructionDate"]);
            contractInfo.SubContractorReference = Data.Utils.GetDBString(dr["SubContractorReference"]);
            contractInfo.SubContractorReferenceDate = Data.Utils.GetDBDateTime(dr["SubContractorReferenceDate"]);
            contractInfo.GoodsServicesTax = Data.Utils.GetDBDecimal(dr["GoodsServicesTax"]);
            contractInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            contractInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);
            contractInfo.QuotesFile = Data.Utils.GetDBString(dr["QuotesFile"]);
            contractInfo.CheckOrigContractDur = Data.Utils.GetDBBoolean(dr["CheckOrigContractDur"]);   // DS20240321
            contractInfo.StartDate = Data.Utils.GetDBDateTime(dr["StartDate"]);                        // DS20240321
            contractInfo.FinishDate = Data.Utils.GetDBDateTime(dr["FinishDate"]);                      // DS20240321

            AssignAuditInfo(contractInfo, dr);

            if (dr["TradeId"] != DBNull.Value)
            {
                contractInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                contractInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                if (dr["ProjectId"] != DBNull.Value)
                {
                    contractInfo.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    contractInfo.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    contractInfo.Trade.Project.AttachmentsFolder = Data.Utils.GetDBString(dr["ProjectAttachmentsFolder"]);
                }
            }

            if (dr["ParentContractId"] != DBNull.Value)
                contractInfo.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));

            if (dr["ProcessId"] != DBNull.Value)
                contractInfo.Process = ProcessController.GetInstance().CreateProcess(dr["ProcessId"], processesDictionary);

            return contractInfo;
        }

        /// <summary>
        /// Creates a ProcessInfo object from a dictionary
        /// </summary>
        public ContractInfo CreateContract(Object dbId, Dictionary<int, ContractInfo> contractsDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (contractsDictionary == null)
                return GetInstance().GetContract(Id);
            else if (contractsDictionary.ContainsKey(Id))
                return contractsDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Creates a Contract from a dr
        /// </summary>
        public ContractInfo CreateContract(IDataReader dr)
        {
            return CreateContract(dr, null);
        }
        
        /// <summary>
        /// Get a contract from persistent storage
        /// </summary>
        public ContractInfo GetContract(int? ContractId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetContract(ContractId);
                if (dr.Read())
                    return CreateContract(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract Info from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get Contracts for the specified Trade from persistent storage
        /// </summary>
        public List<ContractInfo> GetContracts(TradeInfo tradeInfo)
        {
            IDataReader dr = null;
            List<ContractInfo> contractInfoList = new List<ContractInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetContracts(tradeInfo.Id);
                while (dr.Read())
                    contractInfoList.Add(CreateContract(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contracts for Trade from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return contractInfoList;
        }

        /// <summary>
        /// Get Subcontracts for the specified Contract from persistent storage
        /// </summary>
        public List<ContractInfo> GetSubContracts(ContractInfo contractInfo)
        {
            IDataReader dr = null;
            List<ContractInfo> contractInfoList = new List<ContractInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetSubContracts(contractInfo.Id);
                while (dr.Read())
                    contractInfoList.Add(CreateContract(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting SubContracts from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return contractInfoList;
        }

        /// <summary>
        /// Get Subcontracts with variations for the specified Contract from persistent storage
        /// </summary>
        public List<ContractInfo> GetSubContractsWithVariations(ContractInfo contractInfo)
        {
            List<ContractInfo> contractInfoList = GetSubContracts(contractInfo);

            foreach (ContractInfo contract in contractInfoList)
                contract.Variations = GetVariations(contract);

            return contractInfoList;
        }

        /// <summary>
        /// Get a Contract including its Modifications
        /// </summary>
        public ContractInfo GetContractWithModifications(int? ContractId)
        {
            ContractInfo contractInfo = GetContract(ContractId);
            contractInfo.ContractModifications = GetContractModifications(contractInfo);
            return contractInfo;
        }

        /// <summary>
        /// Get a Contract including its Variations
        /// </summary>
        public ContractInfo GetContractWithVariations(int? ContractId)
        {
            ContractInfo contractInfo = GetContract(ContractId);
            contractInfo.Variations = GetVariations(contractInfo);
            return contractInfo;
        }

        /// <summary>
        /// Get a Contract including its Modifications and Variations
        /// </summary>
        public ContractInfo GetContractWithModificationsAndVariations(int? ContractId)
        {
            ContractInfo contractInfo = GetContract(ContractId);
            contractInfo.ContractModifications = GetContractModifications(contractInfo);
            contractInfo.Variations = GetVariations(contractInfo);
            return contractInfo;
        }

        /// <summary>
        /// Get a Contract including its Subcontracts
        /// </summary>
        public ContractInfo GetContractWithSubContracts(int? ContractId)
        {
            ContractInfo contractInfo = GetContract(ContractId);
            contractInfo.Subcontracts = GetSubContracts(contractInfo);
            return contractInfo;
        }

        /// <summary>
        /// Get a Contract including its Subcontracts
        /// </summary>
        public ContractInfo GetContractWithSubContractsAndVariations(int? ContractId)
        {
            ContractInfo contractInfo = GetContract(ContractId);
            contractInfo.Subcontracts = GetSubContractsWithVariations(contractInfo);
            return contractInfo;
        }

        /// <summary>
        /// Get a Contract including its Subcontracts
        /// </summary>
        public ContractInfo GetContractWithModificationsSubContractsAndVariations(int? ContractId)
        {
            ContractInfo contractInfo = GetContract(ContractId);
            contractInfo.ContractModifications = GetContractModifications(contractInfo);
            contractInfo.Subcontracts = GetSubContractsWithVariations(contractInfo);
            return contractInfo;
        }

        /// <summary>
        /// Updates a contract in the database
        /// </summary>
        public void UpdateContract(ContractInfo contractInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(contractInfo);

            parameters.Add(contractInfo.Id);
            parameters.Add(contractInfo.Number);
            parameters.Add(contractInfo.WriteDate);
            parameters.Add(contractInfo.ApprovalDate);
            parameters.Add(contractInfo.CheckQuotes);
            parameters.Add(contractInfo.CheckWinningQuote);
            parameters.Add(contractInfo.CheckComparison);
            parameters.Add(contractInfo.CheckCheckList);
            parameters.Add(contractInfo.CheckPrelettingMinutes);
            parameters.Add(contractInfo.CheckAmendments);
            parameters.Add(contractInfo.SubcontractNumber);
            parameters.Add(contractInfo.SiteInstruction);
            parameters.Add(contractInfo.SiteInstructionDate);
            parameters.Add(contractInfo.SubContractorReference);
            parameters.Add(contractInfo.SubContractorReferenceDate);
            parameters.Add(contractInfo.GoodsServicesTax);
            parameters.Add(contractInfo.Description);
            parameters.Add(contractInfo.Comments);
            parameters.Add(contractInfo.QuotesFile);
            parameters.Add(contractInfo.StartDate);              // DS20240321
            parameters.Add(contractInfo.FinishDate);             // DS20240321
            parameters.Add(contractInfo.CheckOrigContractDur);   // DS20240321
            parameters.Add(contractInfo.ModifiedDate);
            parameters.Add(contractInfo.ModifiedBy);
            try
            {
                Data.DataProvider.GetInstance().UpdateContract(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Contract in database");
            }
        }

        /// <summary>
        /// Updates a contract general info in the database
        /// </summary>
        public void UpdateContractGeneralInfo(ContractInfo contractInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(contractInfo.Id);
            parameters.Add(contractInfo.GoodsServicesTax);

            try
            {
                Data.DataProvider.GetInstance().UpdateContractGeneralInfo(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Contract general info in database");
            }
        }

        /// <summary>
        /// Updates a contract's template in the database
        /// </summary>
        public void UpdateTemplateInContract(ContractInfo contractInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(contractInfo.Id);
            parameters.Add(contractInfo.Template);

            try
            {
                Data.DataProvider.GetInstance().UpdateTemplateInContract(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Contract's template in database");
            }
        }

        /// <summary>
        /// Adds a contract to the database
        /// </summary>
        public int?     AddContract(ContractInfo contractInfo)
        {
            int? contractId = null;
            List<Object> parameters = new List<Object>();
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessInfo processInfo;
            DateTime? dateTime;

            SetCreateInfo(contractInfo);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    processInfo = processController.GetDeepProcessTemplate(contractInfo.Trade.Project.BusinessUnit, contractInfo.Trade.JobType, ProcessTemplateInfo.ProcessTypeContract);

                    if (processInfo != null)
                    {
                        processInfo.Id = processController.AddProcess(processInfo);

                        dateTime = DateTime.Today;
                        projectsController.InitializeHolidays();

                        foreach (ProcessStepInfo processStepInfo in processInfo.Steps)
                        {
                            dateTime = projectsController.AddbusinessDaysToDate(dateTime, 1);
                            processStepInfo.TargetDate = dateTime;
                            processStepInfo.Id = processController.AddProcessStep(processStepInfo);
                        }

                        contractInfo.Process = processInfo;
                    }

                    parameters.Add(GetId(contractInfo.Trade));
                    parameters.Add(GetId(contractInfo.Process));
                    parameters.Add(GetId(contractInfo.ParentContract));
                    parameters.Add(contractInfo.Template);
                    parameters.Add(contractInfo.Number);
                    parameters.Add(contractInfo.WriteDate);
                    parameters.Add(contractInfo.CheckQuotes);
                    parameters.Add(contractInfo.CheckWinningQuote);
                    parameters.Add(contractInfo.CheckComparison);
                    parameters.Add(contractInfo.CheckCheckList);
                    parameters.Add(contractInfo.CheckPrelettingMinutes);
                    parameters.Add(contractInfo.CheckAmendments);
                    parameters.Add(contractInfo.SubcontractNumber);
                    parameters.Add(contractInfo.SiteInstruction);
                    parameters.Add(contractInfo.SiteInstructionDate);
                    parameters.Add(contractInfo.SubContractorReference);
                    parameters.Add(contractInfo.SubContractorReferenceDate);
                    parameters.Add(contractInfo.GoodsServicesTax);
                    parameters.Add(contractInfo.Description);
                    parameters.Add(contractInfo.Comments);
                    parameters.Add(contractInfo.QuotesFile);
                    parameters.Add(contractInfo.StartDate);              // DS20240321
                    parameters.Add(contractInfo.FinishDate);             // DS20240321
                    parameters.Add(contractInfo.CheckOrigContractDur);   // DS20240321
                    //parameters.Add(contractInfo.CheckRetentionReq);   //DS20231108

                    parameters.Add(contractInfo.CreatedDate);
                    parameters.Add(contractInfo.CreatedBy);

                    contractId = Data.DataProvider.GetInstance().AddContract(parameters.ToArray());

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Contract to database");
            }
            return contractId;
        }

        /// <summary>
        /// Adds or Updates a Contract in the database
        /// </summary>
        public int? AddUpdateContract(ContractInfo contractInfo)
        {
            if (contractInfo != null)
            {
                if (contractInfo.Id != null)
                {
                    UpdateContract(contractInfo);
                    return contractInfo.Id;
                }
                else
                {
                    return AddContract(contractInfo);
                }
            }
            else
            {
                return null;
            }
        }










        /// <summary>
        /// Remove a Contract from persistent storage
        /// </summary>
        public void DeleteContract(ContractInfo contractInfo)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    // Remove subcontracts with variations
                    if (contractInfo.Subcontracts != null)
                        foreach (ContractInfo subContract in contractInfo.Subcontracts)
                            DeleteContract(subContract);
                    
                    Data.DataProvider.GetInstance().DeleteContract(contractInfo.Id);

                    // Remove process with steps
                    if (contractInfo.Process != null)
                        ProcessController.GetInstance().DeleteProcess(contractInfo.Process);

                    scope.Complete();
                }
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Contract from database");
            }
        }

        /// <summary>
        /// Creates a new missing field message as an xml node
        /// </summary>
        private void AddXmlError(XmlDocument xmlDocument, XmlElement xmlElementVariable, String fieldName)
        {
            XmlElement xmlElementItem;

            xmlElementItem = xmlDocument.CreateElement("item", null);
            xmlElementItem.SetAttribute("name", fieldName);
            xmlElementVariable.AppendChild(xmlElementItem);
        }

        /// <summary>
        /// Checks a trade to see if it has all the information required according to a template
        /// </summary>
        public XmlDocument CheckTradeForTemplate(TradeInfo tradeInfo, String template)
        {
            TradeParticipationInfo selectedParticipation = tradeInfo.SelectedParticipation;
            SubContractorInfo selectedSubContractor = tradeInfo.SelectedSubContractor;

            List<String> templateVariables = GetTemplateVariables(template);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement xmlElementRoot;
            XmlElement xmlElementVariable;

            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, "no"));
            xmlElementRoot = xmlDocument.CreateElement("root", null);
            xmlElementRoot.SetAttribute("name", "Missing fields");

            foreach (String templateVariable in templateVariables)
            {
                xmlElementVariable = xmlDocument.CreateElement("variable", null);
                xmlElementVariable.SetAttribute("name", GetTemplateVariableTitle(templateVariable));
                
                switch (GetTemplateVariableName(templateVariable))
                {
                    case VariableOrderNumber:
                        if (tradeInfo.WorkOrderNumber == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Work Order Number");
                        break;

                    case VariableProjectName:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Name");
                        break;

                    case VariableProjectNumber:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Number == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Number");
                        break;

                    case VariableJobNumber:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Number == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Number");
                        else if (tradeInfo.Project.Year == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Year");
                        break;
                    
                    case VariableProjectYear:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Year == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Year");
                        break;

                    case VariableProjectSuburb:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Locality == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Suburb");
                        break;

                    case VariableProjectCommencementDate:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.CommencementDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Commencement Date");
                        break;

                    case VariableProjectCompletionDate:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.CompletionDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Completion Date");
                        break;

                    case VariableCASignature:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ContractsAdministrator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator");
                        else if (tradeInfo.Project.ContractsAdministrator.Signature == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator Signature");
                        break;

                    case VariablePMSignature:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ProjectManager == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Manager");
                        else if (tradeInfo.Project.ProjectManager.Signature == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Manager Signature");
                        break;

                    case VariableCMSignature:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ConstructionManager == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Constructions Manager");
                        else if (tradeInfo.Project.ConstructionManager.Signature == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Constructions Manager Signature");
                        break;

                    case VariableDCSignature:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.DesignCoordinator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Coordinator");
                        else if (tradeInfo.Project.DesignCoordinator.Signature == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Coordinator Signature");
                        break;

                    case VariableDMSignature:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.DesignManager == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Manager");
                        else if (tradeInfo.Project.DesignManager.Signature == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Manager Signature");
                        break;

                    case VariableCAName:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ContractsAdministrator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator");
                        else if (tradeInfo.Project.ContractsAdministrator.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator Name");
                        break;

                    case VariablePMName:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ProjectManager == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Manager");
                        else if (tradeInfo.Project.ProjectManager.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project Manager Name");
                        break;

                    case VariableCMName:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ConstructionManager == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Constructions Manager");
                        else if (tradeInfo.Project.ConstructionManager.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Constructions Manager Name");
                        break;

                    case VariableDCName:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.DesignCoordinator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Coordinator");
                        else if (tradeInfo.Project.DesignCoordinator.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Coordinator Name");
                        break;

                    case VariableDMName:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.DesignManager == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Manager");
                        else if (tradeInfo.Project.DesignManager.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Design Manager Name");
                        break;

                    case VariableCAPhone:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ContractsAdministrator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator");
                        else if (tradeInfo.Project.ContractsAdministrator.Phone == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator Phone");
                        break;

                    case VariableCAFax:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ContractsAdministrator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator");
                        else if (tradeInfo.Project.ContractsAdministrator.Fax == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator Fax");
                        break;

                    case VariableCAEmail:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.ContractsAdministrator == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator");
                        else if (tradeInfo.Project.ContractsAdministrator.Email == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contracts Administrator Email");
                        break;

                    case VariableTradeName:
                        if (tradeInfo.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Trade Name");
                        break;

                    case VariableTradeCode:
                        if (tradeInfo.Code == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Trade Code");
                        break;

                    case VariableContractValue:
                    case VariableContractGST:
                    case VariableContractTotal:
                    case VariableContratTotalWords:
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.Amount == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Quote");
                        break;

                    case VariableContractDate:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.WriteDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract Write Date");
                        break;

                    case VariableScopeOfWorks:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        break;

                    case VariableSiteAddress:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else
                        {
                            if (tradeInfo.Project.Street == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Project Street");
                            if (tradeInfo.Project.Locality == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Project Suburb");
                            if (tradeInfo.Project.State == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Project State");
                            if (tradeInfo.Project.PostalCode == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Project Postal Code");
                        }
                        break;

                    case VariableSiteManager:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Foreman == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Foreman");
                        else if (tradeInfo.Project.Foreman.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Foreman Name");
                        break;

                    case VariableSitePhone:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Foreman == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Foreman");
                        else if (tradeInfo.Project.Foreman.Mobile == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Foreman Mobil");
                        break;

                    case VariableDrawings:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        break;

                    case VariableDrawingsSummary:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        break;

                    case VariableCommencementDate:
                        if (tradeInfo.CommencementDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Commencement Date");
                        break;

                    case VariableCompletionDate:
                        if (tradeInfo.CompletionDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Completion Date");
                        break;

                    case VariableDueDate:
                        //#---if (tradeInfo.DueDate == null)
                        //#--    AddXmlError(xmlDocument, xmlElementVariable, "Due Date"); break;
                        //#--Passing Subcontractor due date  in place of Trade due date 
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.QuoteDueDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Quote Due Date");
                        break;
                    //#---

                    case VariableDefectsLiability:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.DefectsLiability == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Defects Liability");
                        break;

                    case VariableLiquidatedDamages:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.LiquidatedDamages == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Liquidated Damages");
                        break;

                    case VariableSiteAllowances:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.SiteAllowances == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Site Allowances");
                        break;

                    case VariableRetention:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Retention == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Retention Amount");
                        break;

                    case VariableRetentionToCertification:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.RetentionToCertification == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Retention to Certification");
                        break;

                    case VariableRetentionToLdp:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.RetentionToDLP == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Retention to LDP");
                        break;

                    case VariableInterest:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Interest == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Interest");
                        break;

                    case VariablePrincipal:
                        if (tradeInfo.Project == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Project");
                        else if (tradeInfo.Project.Principal == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Principal");
                        break;

                    case VariableSubcontractorName:
                        if (selectedSubContractor == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedSubContractor.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Name");
                        break;

                    case VariableSubcontractorABN:
                        if (selectedSubContractor == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedSubContractor.Abn == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor ABN");
                        break;

                    case VariableSubcontractorAddress:
                        if (selectedSubContractor == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else
                        {
                            if (selectedSubContractor.Street == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Street");
                            if (selectedSubContractor.Locality == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Suburb");
                            if (selectedSubContractor.State == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor State");
                            if (selectedSubContractor.PostalCode == null)
                                AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Postal Code");
                        }
                        break;

                    case VariableSubcontractorPhone:
                        if (selectedSubContractor == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedSubContractor.Phone == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Phone");
                        break;

                    case VariableSubcontractorFax:
                        if (selectedSubContractor == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedSubContractor.Fax == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Fax");
                        break;
                    case VariablePaymentTerms:     //DS2023-10-02
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Payment Terms");
                        break;


                    case VariableSubcontractorContact:
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.Contact == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact");
                        else if (selectedParticipation.Contact.Name == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact Name");
                        break;

                    case VariableSubcontractorContactPhone:
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.Contact == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact");
                        else if (selectedParticipation.Contact.Phone == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact Phone");
                        break;

                    case VariableSubcontractorContactMobile:
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.Contact == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact");
                        else if (selectedParticipation.Contact.Mobile == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact Mobile Phone");
                        break;

                    case VariableSubcontractorContactEmail:
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.Contact == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact");
                        else if (selectedParticipation.Contact.Email == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Contact Email");
                        break;

                    case VariableParticipationDueDate:
                        if (selectedParticipation == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor");
                        else if (selectedParticipation.QuoteDueDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Selected Subcontractor Quote Due Date");
                        break;

                    case VariableVariationOrder:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.SubcontractNumber == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Variation Order Number");
                        break;

                    case VariableSiteInstruction:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.SiteInstruction == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Site Instruction");
                        break;

                    case VariableSiteInstructionDate:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.SiteInstructionDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Site Instruction Date");
                        break;
                    case VariableStartDate:    // DS2024-04-15
                        bool CheckOrigContractDur = false;
                        if (tradeInfo.Contract.CheckOrigContractDur != null && tradeInfo.Contract.CheckOrigContractDur == true)
                            CheckOrigContractDur = true;
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.StartDate == null && CheckOrigContractDur == false)
                            AddXmlError(xmlDocument, xmlElementVariable, "Start Date");
                        break;
                    case VariableFinishDate:    // DS2024-04-15
                        bool CheckOrigContractDur1 = false;
                        if (tradeInfo.Contract.CheckOrigContractDur != null && tradeInfo.Contract.CheckOrigContractDur == true)
                            CheckOrigContractDur1 = true;
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.FinishDate == null && CheckOrigContractDur1 == false)
                            AddXmlError(xmlDocument, xmlElementVariable, "Finish Date");
                        break;

                    case VariableSubcontractorReference:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.SubContractorReference == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Subcontractor Reference");
                        break;
                    
                    case VariableSubcontractorReferenceDate:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.SubContractorReferenceDate == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Subcontractor Reference Date");
                        break;
                    
                    case VariableVariationOrderTitle:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.Description == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Title");
                        break;
                    
                    case VariableVariations:
                    case VariableVariationsTotal:
                    case VariableVariationsGST:
                    case VariableVariationsTotalPlusGST:
                    case VariableVariationsTrades:
                        if (tradeInfo.Contract == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Contract");
                        else if (tradeInfo.Contract.Variations == null)
                            AddXmlError(xmlDocument, xmlElementVariable, "Variations");
                        else if (tradeInfo.Contract.Variations.Count == 0)
                            AddXmlError(xmlDocument, xmlElementVariable, "Variations");
                        break;

                    case VariableTradesList:
                        if (tradeInfo.TradeBudgets == null || tradeInfo.TradeBudgets.Count == 0)
                            AddXmlError(xmlDocument, xmlElementVariable, "Budget trades");
                        break;
                }

                if (xmlElementVariable.HasChildNodes)
                    xmlElementRoot.AppendChild(xmlElementVariable);
            }

            if (xmlElementRoot.HasChildNodes)
                xmlDocument.AppendChild(xmlElementRoot);

            return xmlDocument;
        }

        /// <summary>
        /// Replaces a template variable by its value
        /// </summary>
        public String GetTradeVariable(TradeInfo tradeInfo, String variableName)
        {
            Int32 i;
            String sectionValue;
            String drawingsValue;

            switch (variableName)
            {
                case VariableToday: return UI.Utils.SetFormDate(DateTime.Today);
                case VariableOrderNumber: return tradeInfo.WorkOrderNumber;
                case VariableProjectName: return tradeInfo.Project.Name;
                case VariableProjectNumber: return tradeInfo.Project.Number;
                case VariableJobNumber: return tradeInfo.Project.FullNumber;
                case VariableProjectYear: return tradeInfo.Project.Year;
                case VariableProjectSuburb: return tradeInfo.Project.Locality;
                //#---
                case VariableProjectAddress: return tradeInfo.Project.Street +"  " + tradeInfo.Project.Locality + "  "+tradeInfo.Project.State;
                case VariableProjectState:   return tradeInfo.Project.State;
                //#--
                case VariableProjectCommencementDate: return UI.Utils.SetFormDate(tradeInfo.Project.CommencementDate);
                case VariableProjectCompletionDate: return UI.Utils.SetFormDate(tradeInfo.Project.CompletionDate);
                case VariableCASignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.ContractsAdministrator.Signature + "' width='180' heigth='60'>";
                case VariablePMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.ProjectManager.Signature + "' width='180' heigth='60'>";

                //#--- Witness Signature replaced by Finance Controler(CM-->FC)

                //case VariableCMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.ConstructionManager.Signature + "' width='180' heigth='60'>";
                case VariableCMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.FinancialController.Signature + "' width='180' heigth='60'>";

                //#---
                  

                case VariableDCSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.DesignCoordinator.Signature + "' width='180' heigth='60'>";
                case VariableDMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.DesignManager.Signature + "' width='180' heigth='60'>";


                case VariableCAName: return tradeInfo.Project.ContractsAdministrator.Name;
                case VariablePMName: return tradeInfo.Project.ProjectManager.Name;

                //#--- Witness Name replaced by Finance Controler   (CM-->FC)
                // case VariableCMName: return tradeInfo.Project.ConstructionManager.Name;
                   case VariableCMName: return tradeInfo.Project.FinancialController.Name;

                //#--- 

                case VariableDCName: return tradeInfo.Project.DesignCoordinator.Name;
                case VariableDMName: return tradeInfo.Project.DesignManager.Name;
                case VariableCAPhone: return tradeInfo.Project.ContractsAdministrator.Phone;
                case VariableCAFax: return tradeInfo.Project.ContractsAdministrator.Fax;
                case VariableCAEmail: return tradeInfo.Project.ContractsAdministrator.Email;
                case VariableTradeName: return tradeInfo.Name;
                case VariableTradeCode: return tradeInfo.Code;
                case VariableContractValue: return UI.Utils.SetFormEditDecimal(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation));
                case VariableContractGST: return UI.Utils.SetFormEditDecimal(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation) * tradeInfo.Contract.GoodsServicesTax);
                case VariableContractTotal: return UI.Utils.SetFormEditDecimal(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation) * (1 + tradeInfo.Contract.GoodsServicesTax));
                case VariableContratTotalWords: return UI.Utils.DecimalToText(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation));
                case VariableContractDate: return UI.Utils.SetFormDate(tradeInfo.Contract.WriteDate);
                case VariableScopeOfWorks: return UI.Utils.TextToHtml(TradesController.GetInstance().CreateScopeOfWorks(tradeInfo));
                case VariableSiteAddress: return tradeInfo.Project.Street + "<br />" + tradeInfo.Project.Locality + "," + tradeInfo.Project.State + " " + tradeInfo.Project.PostalCode;
                case VariableSiteManager: return tradeInfo.Project.Foreman.Name;
                case VariableSitePhone: return tradeInfo.Project.Foreman.Mobile;

                case VariableDrawings:
                    sectionValue = String.Empty;

                    if (tradeInfo.DrawingTypes != null)
                    {
                        foreach (DrawingTypeInfo drawingTypeInfo in tradeInfo.DrawingTypes)
                        {
                            drawingsValue = String.Empty;
                            foreach (DrawingInfo drawingInfo in tradeInfo.IncludedDrawings)
                                if (drawingInfo.DrawingType.Equals(drawingTypeInfo))
                                    drawingsValue += drawingInfo.Name + "-" + drawingInfo.LastRevisionNumber + ", ";

                            if (drawingsValue != String.Empty)
                            {
                                sectionValue += "<tr>\n" +
                                                "  <td class='Text'>" + drawingTypeInfo.Name + ":</td>\n" +
                                                "  <td class='Text'>" + drawingsValue.Substring(0, drawingsValue.Length - 2) + "</td>\n" +
                                                "</tr>\n";
                            }
                        }

                        if (sectionValue != String.Empty)
                            sectionValue = "<table>\n" + sectionValue + "</table>\n";
                    }

                    return sectionValue;

                case VariableDrawingsSummary:

                    sectionValue = String.Empty;

                    if (tradeInfo.SelectedParticipation.Transmittal != null && tradeInfo.SelectedParticipation.Transmittal.TransmittalRevisions != null)
                        foreach (TransmittalRevisionInfo transmittalRevisionInfo in tradeInfo.SelectedParticipation.Transmittal.TransmittalRevisions)
                            sectionValue += transmittalRevisionInfo.DrawingName + "-" + transmittalRevisionInfo.RevisionName + ", ";
                    else
                        foreach (DrawingInfo drawingInfo in tradeInfo.IncludedDrawings)
                            sectionValue += drawingInfo.Name + "-" + drawingInfo.LastRevisionNumber + ", ";

                    if (sectionValue != String.Empty)
                        sectionValue = sectionValue.Substring(0, sectionValue.Length - 2);

                    return sectionValue;

                case VariableCommencementDate: return UI.Utils.SetFormDate(tradeInfo.CommencementDate);
                case VariableCompletionDate: return UI.Utils.SetFormDate(tradeInfo.CompletionDate);

                //#--case VariableDueDate: return UI.Utils.SetFormDate(tradeInfo.DueDate);
                case VariableDueDate: return UI.Utils.SetFormDate(tradeInfo.SelectedParticipation.QuoteDueDate);//#---- to display Selected Participation's due date
                case VariablePaymentTerms: return tradeInfo.SelectedParticipation.PaymentTerms;  // DS2023-10-02
                case VariableDefectsLiability: return UI.Utils.SetFormInteger(tradeInfo.Project.DefectsLiability);
                case VariableLiquidatedDamages: return tradeInfo.Project.LiquidatedDamages;
                case VariableSiteAllowances: return tradeInfo.Project.SiteAllowances;
                case VariableRetention: return tradeInfo.Project.Retention;
                case VariableRetentionToCertification: return tradeInfo.Project.RetentionToCertification;
                case VariableRetentionToLdp: return tradeInfo.Project.RetentionToDLP;
                case VariableInterest: return tradeInfo.Project.Interest;
                case VariablePreLettingDate: return String.Empty;
                case VariablePrincipal: return tradeInfo.Project.Principal;
                    //#---
                case VariablePrincipalABN: return tradeInfo.Project.PrincipalABN;     //#---
                case VariableSubcontractorName: return tradeInfo.SelectedSubContractor.Name;
                case VariableSubcontractorABN: return tradeInfo.SelectedSubContractor.Abn;
                //#---
                case VariableSubcontractorACN: return tradeInfo.SelectedSubContractor.ACN;
                case VariableSubcontractorLiceneceNumber: return tradeInfo.SelectedSubContractor.LicenceNumber;
                case VariableSubcontractorEmail: return (tradeInfo.Participations.Find(x => x.IsAwarded == true)).Contact.Email;
                case VariableProjectLawOfSubcontract: return tradeInfo.Project.LawOfSubcontract;
                //#---
                case VariableSubcontractorAddress: return tradeInfo.SelectedSubContractor.Street + "<br />" + tradeInfo.SelectedSubContractor.Locality + "," + tradeInfo.SelectedSubContractor.State + " " + tradeInfo.SelectedSubContractor.PostalCode;
                case VariableSubcontractorPhone: return tradeInfo.SelectedSubContractor.Phone;
                case VariableSubcontractorFax: return tradeInfo.SelectedSubContractor.Fax;
                case VariableSubcontractorContact: return tradeInfo.SelectedParticipation.Contact.Name;
                case VariableSubcontractorContactPhone: return tradeInfo.SelectedParticipation.Contact.Phone;
                case VariableSubcontractorContactMobile: return tradeInfo.SelectedParticipation.Contact.Mobile;
                case VariableSubcontractorContactEmail: return tradeInfo.SelectedParticipation.Contact.Email;
                case VariableParticipationDueDate: return UI.Utils.SetFormDate(tradeInfo.SelectedParticipation.QuoteDueDate);
                case VariableVariationOrder: return UI.Utils.SetFormInteger(tradeInfo.Contract.SubcontractNumber);
                case VariableSiteInstruction: return tradeInfo.Contract.SiteInstruction;
                case VariableSiteInstructionDate: return UI.Utils.SetFormDate(tradeInfo.Contract.SiteInstructionDate);
                case VariableSubcontractorReference: return tradeInfo.Contract.SubContractorReference;
                case VariableSubcontractorReferenceDate: return UI.Utils.SetFormDate(tradeInfo.Contract.SubContractorReferenceDate);
                case VariableVariationOrderTitle: return tradeInfo.Contract.Description;
                case VariableStartDate: return UI.Utils.SetFormDate(tradeInfo.Contract.StartDate);   // DS2024-03-25
                case VariableFinishDate: return UI.Utils.SetFormDate(tradeInfo.Contract.FinishDate);   // DS2024-03-25
                case VariableVariations:
                    i = 0;
                    sectionValue = "<table width='100%'>";

                    foreach (VariationInfo variationInfo in tradeInfo.Contract.Variations)
                    {
                        sectionValue += "<tr>\n" +
                                        "  <td class='Text' align='center' valign='top'>" + UI.Utils.SetFormInteger(++i) + "</td>\n" +
                                        "  <td class='Text' align='left'><b>" + variationInfo.Header + "</b><br />" + variationInfo.Description + ":</td>\n" +
                                        "  <td class='Text' align='right' valign='bottom'>" + UI.Utils.SetFormDecimal(variationInfo.Amount) + "</td>\n" +
                                        "</tr>\n";
                    }

                    return sectionValue + "</table>";

                case VariableVariationsTotal: return UI.Utils.SetFormDecimal(tradeInfo.Contract.TotalVariations);
                case VariableVariationsGST: return UI.Utils.SetFormDecimal(tradeInfo.Contract.TotalVariations * tradeInfo.Contract.GoodsServicesTax);
                case VariableVariationsTotalPlusGST: return UI.Utils.SetFormDecimal(tradeInfo.Contract.TotalVariations * (1 + tradeInfo.Contract.GoodsServicesTax));

                case VariableVariationsTrades:
                    List<VariationInfo> variationInfoList = new List<VariationInfo>();
                    VariationInfo variationNew;
                    Boolean isInList;

                    foreach (VariationInfo variationInfo in tradeInfo.Contract.Variations)
                    {
                        isInList = false;

                        foreach (VariationInfo variation in variationInfoList)
                        {
                            if (variationInfo.Number == null && variation.Number == null)
                                isInList = variationInfo.TradeCode == variation.TradeCode && variationInfo.Type == variation.Type;
                            else
                                isInList = variationInfo.TradeCode == variation.TradeCode && variationInfo.Type == variation.Type && variationInfo.Number == variation.Number;

                            if (isInList)
                            {
                                variation.Amount = variation.Amount + variationInfo.Amount;
                                break;
                            }
                        }

                        if (!isInList)
                        {
                            variationNew = new VariationInfo();

                            variationNew.TradeCode = variationInfo.TradeCode;
                            variationNew.Type = variationInfo.Type;
                            variationNew.Number = variationInfo.Number;
                            //#---11/10/2019
                            variationNew.BudgetProvider = variationInfo.BudgetProvider;
                            //#---11/10/2019
                            variationNew.Amount = variationInfo.Amount;

                            variationInfoList.Add(variationNew);
                        }
                    }

                    sectionValue = "";

                    foreach (VariationInfo variationInfo in variationInfoList)
                        if (variationInfo.Number != null)
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " " + variationInfo.Number + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";
                        //#---11/10/2019
                        else if(variationInfo.Type=="CV" && variationInfo.BudgetProvider != null)
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " - " + variationInfo.BudgetProvider.BudgetName + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";
                         //#---11/10/2019


                        //#--03/05/2022
                        else if (variationInfo.Type == "SA" && variationInfo.BudgetProvider != null)
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " - " + variationInfo.BudgetProvider.BudgetName + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";

                        //#--03/05/2022

                        else
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";

                    return sectionValue;

                case VariableTradesList:

                    sectionValue = String.Empty;
                    String budgetName;

                    foreach (TradeBudgetInfo tradeBudgetInfo in tradeInfo.TradeBudgets)
                    {
                        if (tradeBudgetInfo.BudgetProvider.BudgetType == BudgetType.BOQ)
                            budgetName = tradeBudgetInfo.BudgetProvider.BudgetType.ToString();
                        else
                            budgetName = String.Format("{0} {1}", tradeBudgetInfo.BudgetProvider.BudgetType.ToString(), tradeBudgetInfo.BudgetProvider.BudgetName);

                            sectionValue += "<tr>\n" +
                                            "  <td class='Text' nowrap>" + budgetName + "</td>\n" +
                                            "  <td class='Text' nowrap>" + tradeBudgetInfo.TradeCode + "</td>\n" +
                                            "  <td class='Text' align='right' nowrap>" + UI.Utils.SetFormDecimal(tradeBudgetInfo.Amount) + "</td>\n" +
                                            "</tr>\n";
                    }

                    if (sectionValue != String.Empty)
                        sectionValue = "<table cellpadding='2' cellspacing='0' style='border:solid; border-width:1px; border-color:#AAAAAA'>\n" + sectionValue + "</table>\n";

                    return sectionValue;

                default: return null;
            }
        }

        /// <summary>
        /// Merges a template making the variables editable sections
        /// </summary>
        public String MergeTemplateEditatle(TradeInfo tradeInfo, String template)
        {
            return MergeTemplate(tradeInfo, template, "Edit");
        }

        /// <summary>
        /// Merges a template
        /// </summary>
        public String MergeTemplateView(TradeInfo tradeInfo, String template)
        {
            return MergeTemplate(tradeInfo, template, "View");
        }

        /// <summary>
        /// Merges a template to print, gets the footer if exist
        /// </summary>
        public String MergeTemplatePrint(TradeInfo tradeInfo, String template)
        {
            return MergeTemplate(tradeInfo, template, "Print");
        }

        /// <summary>
        /// Replaces the variables by its values and can make them editable sections with the same name 
        /// The trade must have all its information, project, drawings, etc. there must be a selected participation
        /// </summary>
        public String MergeTemplate(TradeInfo tradeInfo, String template, String conversionType)
        {
            StringBuilder stringBuilder = new StringBuilder(template);
            List<String> templateVariables;
            String sectionValue;
            String sectionInfo;

            if (CheckTradeForTemplate(tradeInfo, template).DocumentElement != null)
                throw new Exception("There are missing fields in the trade.");

            templateVariables = GetTemplateVariables(template);

            foreach (String templateVariable in templateVariables)
            {
                sectionValue = GetTradeVariable(tradeInfo, GetTemplateVariableName(templateVariable));

                if (conversionType == "Edit") 
                    sectionInfo = TagEditableOpen + TagVariable + GetTemplateVariableName(templateVariable) + " " + TagTitle + GetTemplateVariableTitle(templateVariable) + TagEditableEnd + sectionValue + TagEditableClose;
                else
                    sectionInfo = sectionValue;

                stringBuilder.Replace(templateVariable, sectionInfo);
            }

            if (conversionType == "Edit")
                return stringBuilder.ToString();
            else
            {
                String fullFooter = GetTemplateFooterFull(template);
                String textFooter = GetTemplateFooterText(template);

                if (conversionType == "View")
                {
                    if (fullFooter != null)
                        stringBuilder.Replace(fullFooter, "<span class='TemplateFooter'>" + textFooter + "</span>");
                }
                else if (conversionType == "Print")
                {
                    if (fullFooter != null)
                        stringBuilder.Replace(fullFooter, "");
                }

                return stringBuilder.ToString();
            }
        }
        

        public String GetTradeVariable_Old(TradeInfo tradeInfo, String variableName)
        {
            Int32 i;
            String sectionValue;
            String drawingsValue;

            switch (variableName)
            {
                case VariableToday: return UI.Utils.SetFormDate(DateTime.Today);
                case VariableOrderNumber: return tradeInfo.WorkOrderNumber;
                case VariableProjectName: return tradeInfo.Project.Name;
                case VariableProjectNumber: return tradeInfo.Project.Number;
                case VariableJobNumber: return tradeInfo.Project.FullNumber;
                case VariableProjectYear: return tradeInfo.Project.Year;
                case VariableProjectSuburb: return tradeInfo.Project.Locality;
                //#---
                case VariableProjectAddress: return tradeInfo.Project.Street + "  " + tradeInfo.Project.Locality + "  " + tradeInfo.Project.State;
                case VariableProjectState: return tradeInfo.Project.State;
                //#--
                case VariableProjectCommencementDate: return UI.Utils.SetFormDate(tradeInfo.Project.CommencementDate);
                case VariableProjectCompletionDate: return UI.Utils.SetFormDate(tradeInfo.Project.CompletionDate);
                case VariableCASignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.ContractsAdministrator.Signature + "' width='180' heigth='60'>";
                case VariablePMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.ProjectManager.Signature + "' width='180' heigth='60'>";

                //#--- Witness Signature replaced by Finance Controler(CM-->FC)

                //case VariableCMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.ConstructionManager.Signature + "' width='180' heigth='60'>";
                case VariableCMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.FinancialController.Signature + "' width='180' heigth='60'>";

                //#---


                case VariableDCSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.DesignCoordinator.Signature + "' width='180' heigth='60'>";
                case VariableDMSignature: return "<img alt='Signature' src='../../Images/Signatures/" + tradeInfo.Project.DesignManager.Signature + "' width='180' heigth='60'>";


                case VariableCAName: return tradeInfo.Project.ContractsAdministrator.Name;
                case VariablePMName: return tradeInfo.Project.ProjectManager.Name;

                //#--- Witness Name replaced by Finance Controler   (CM-->FC)
                // case VariableCMName: return tradeInfo.Project.ConstructionManager.Name;
                case VariableCMName: return tradeInfo.Project.FinancialController.Name;

                //#--- 

                case VariableDCName: return tradeInfo.Project.DesignCoordinator.Name;
                case VariableDMName: return tradeInfo.Project.DesignManager.Name;
                case VariableCAPhone: return tradeInfo.Project.ContractsAdministrator.Phone;
                case VariableCAFax: return tradeInfo.Project.ContractsAdministrator.Fax;
                case VariableCAEmail: return tradeInfo.Project.ContractsAdministrator.Email;
                case VariableTradeName: return tradeInfo.Name;
                case VariableTradeCode: return tradeInfo.Code;
                case VariableContractValue: return UI.Utils.SetFormEditDecimal(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation));
                case VariableContractGST: return UI.Utils.SetFormEditDecimal(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation) * tradeInfo.Contract.GoodsServicesTax);
                case VariableContractTotal: return UI.Utils.SetFormEditDecimal(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation) * (1 + tradeInfo.Contract.GoodsServicesTax));
                case VariableContratTotalWords: return UI.Utils.DecimalToText(TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation));
                case VariableContractDate: return UI.Utils.SetFormDate(tradeInfo.Contract.WriteDate);
                case VariableScopeOfWorks: return UI.Utils.TextToHtml(TradesController.GetInstance().CreateScopeOfWorks(tradeInfo));
                case VariableSiteAddress: return tradeInfo.Project.Street + "<br />" + tradeInfo.Project.Locality + "," + tradeInfo.Project.State + " " + tradeInfo.Project.PostalCode;
                case VariableSiteManager: return tradeInfo.Project.Foreman.Name;
                case VariableSitePhone: return tradeInfo.Project.Foreman.Mobile;

                case VariableDrawings:
                    sectionValue = String.Empty;

                    if (tradeInfo.DrawingTypes != null)
                    {
                        foreach (DrawingTypeInfo drawingTypeInfo in tradeInfo.DrawingTypes)
                        {
                            drawingsValue = String.Empty;
                            foreach (DrawingInfo drawingInfo in tradeInfo.IncludedDrawings)
                                if (drawingInfo.DrawingType.Equals(drawingTypeInfo))
                                    drawingsValue += drawingInfo.Name + "-" + drawingInfo.LastRevisionNumber + ", ";

                            if (drawingsValue != String.Empty)
                            {
                                sectionValue += "<tr>\n" +
                                                "  <td class='Text'>" + drawingTypeInfo.Name + ":</td>\n" +
                                                "  <td class='Text'>" + drawingsValue.Substring(0, drawingsValue.Length - 2) + "</td>\n" +
                                                "</tr>\n";
                            }
                        }

                        if (sectionValue != String.Empty)
                            sectionValue = "<table>\n" + sectionValue + "</table>\n";
                    }

                    return sectionValue;

                case VariableDrawingsSummary:

                    sectionValue = String.Empty;

                    if (tradeInfo.SelectedParticipation.Transmittal != null && tradeInfo.SelectedParticipation.Transmittal.TransmittalRevisions != null)
                        foreach (TransmittalRevisionInfo transmittalRevisionInfo in tradeInfo.SelectedParticipation.Transmittal.TransmittalRevisions)
                            sectionValue += transmittalRevisionInfo.DrawingName + "-" + transmittalRevisionInfo.RevisionName + ", ";
                    else
                        foreach (DrawingInfo drawingInfo in tradeInfo.IncludedDrawings)
                            sectionValue += drawingInfo.Name + "-" + drawingInfo.LastRevisionNumber + ", ";

                    if (sectionValue != String.Empty)
                        sectionValue = sectionValue.Substring(0, sectionValue.Length - 2);

                    return sectionValue;

                case VariableCommencementDate: return UI.Utils.SetFormDate(tradeInfo.CommencementDate);
                case VariableCompletionDate: return UI.Utils.SetFormDate(tradeInfo.CompletionDate);

                //#--case VariableDueDate: return UI.Utils.SetFormDate(tradeInfo.DueDate);
                case VariableDueDate: return UI.Utils.SetFormDate(tradeInfo.SelectedParticipation.QuoteDueDate);//#---- to display Selected Participation's due date
                case VariablePaymentTerms: return tradeInfo.SelectedParticipation.PaymentTerms;//DS2023-10-02
                case VariableDefectsLiability: return UI.Utils.SetFormInteger(tradeInfo.Project.DefectsLiability);
                case VariableLiquidatedDamages: return tradeInfo.Project.LiquidatedDamages;
                case VariableSiteAllowances: return tradeInfo.Project.SiteAllowances;
                case VariableRetention: return tradeInfo.Project.Retention;
                case VariableRetentionToCertification: return tradeInfo.Project.RetentionToCertification;
                case VariableRetentionToLdp: return tradeInfo.Project.RetentionToDLP;
                case VariableInterest: return tradeInfo.Project.Interest;
                case VariablePreLettingDate: return String.Empty;
                case VariablePrincipal: return tradeInfo.Project.Principal;
                //#---
                case VariablePrincipalABN: return tradeInfo.Project.PrincipalABN;     //#---
                case VariableSubcontractorName: return tradeInfo.SelectedSubContractor.Name;
                case VariableSubcontractorABN: return tradeInfo.SelectedSubContractor.Abn;
                //#---
                case VariableSubcontractorACN: return tradeInfo.SelectedSubContractor.ACN;
                case VariableSubcontractorLiceneceNumber: return tradeInfo.SelectedSubContractor.LicenceNumber;
                case VariableSubcontractorEmail: return (tradeInfo.Participations.Find(x => x.IsAwarded == true)).Contact.Email;
                case VariableProjectLawOfSubcontract: return tradeInfo.Project.LawOfSubcontract;
                //#---
                case VariableSubcontractorAddress: return tradeInfo.SelectedSubContractor.Street + "<br />" + tradeInfo.SelectedSubContractor.Locality + "," + tradeInfo.SelectedSubContractor.State + " " + tradeInfo.SelectedSubContractor.PostalCode;
                case VariableSubcontractorPhone: return tradeInfo.SelectedSubContractor.Phone;
                case VariableSubcontractorFax: return tradeInfo.SelectedSubContractor.Fax;
                case VariableSubcontractorContact: return tradeInfo.SelectedParticipation.Contact.Name;
                case VariableSubcontractorContactPhone: return tradeInfo.SelectedParticipation.Contact.Phone;
                case VariableSubcontractorContactMobile: return tradeInfo.SelectedParticipation.Contact.Mobile;
                case VariableSubcontractorContactEmail: return tradeInfo.SelectedParticipation.Contact.Email;
                case VariableParticipationDueDate: return UI.Utils.SetFormDate(tradeInfo.SelectedParticipation.QuoteDueDate);
                case VariableVariationOrder: return UI.Utils.SetFormInteger(tradeInfo.Contract.SubcontractNumber);
                case VariableSiteInstruction: return tradeInfo.Contract.SiteInstruction;
                case VariableSiteInstructionDate: return UI.Utils.SetFormDate(tradeInfo.Contract.SiteInstructionDate);
                case VariableSubcontractorReference: return tradeInfo.Contract.SubContractorReference;
                case VariableSubcontractorReferenceDate: return UI.Utils.SetFormDate(tradeInfo.Contract.SubContractorReferenceDate);
                case VariableVariationOrderTitle: return tradeInfo.Contract.Description;
                case VariableStartDate: return UI.Utils.SetFormDate(tradeInfo.Contract.StartDate);   // DS2024-03-25
                case VariableFinishDate: return UI.Utils.SetFormDate(tradeInfo.Contract.FinishDate); // DS2024-03-25

                case VariableVariations:
                    i = 0;
                    sectionValue = "<table width='100%'>";

                    foreach (VariationInfo variationInfo in tradeInfo.Contract.Variations)
                    {
                        sectionValue += "<tr>\n" +
                                        "  <td class='Text' align='center' valign='top'>" + UI.Utils.SetFormInteger(++i) + "</td>\n" +
                                        "  <td class='Text' align='left'><b>" + variationInfo.Header + "</b><br />" + variationInfo.Description + ":</td>\n" +
                                        "  <td class='Text' align='right' valign='bottom'>" + UI.Utils.SetFormDecimal(variationInfo.Amount) + "</td>\n" +
                                        "</tr>\n";
                    }

                    return sectionValue + "</table>";

                case VariableVariationsTotal: return UI.Utils.SetFormDecimal(tradeInfo.Contract.TotalVariations);
                case VariableVariationsGST: return UI.Utils.SetFormDecimal(tradeInfo.Contract.TotalVariations * tradeInfo.Contract.GoodsServicesTax);
                case VariableVariationsTotalPlusGST: return UI.Utils.SetFormDecimal(tradeInfo.Contract.TotalVariations * (1 + tradeInfo.Contract.GoodsServicesTax));

                case VariableVariationsTrades:
                    List<VariationInfo> variationInfoList = new List<VariationInfo>();
                    VariationInfo variationNew;
                    Boolean isInList;

                    foreach (VariationInfo variationInfo in tradeInfo.Contract.Variations)
                    {
                        isInList = false;

                        foreach (VariationInfo variation in variationInfoList)
                        {
                            if (variationInfo.Number == null && variation.Number == null)
                                isInList = variationInfo.TradeCode == variation.TradeCode && variationInfo.Type == variation.Type;
                            else
                                isInList = variationInfo.TradeCode == variation.TradeCode && variationInfo.Type == variation.Type && variationInfo.Number == variation.Number;

                            if (isInList)
                            {
                                variation.Amount = variation.Amount + variationInfo.Amount;
                                break;
                            }
                        }

                        if (!isInList)
                        {
                            variationNew = new VariationInfo();

                            variationNew.TradeCode = variationInfo.TradeCode;
                            variationNew.Type = variationInfo.Type;
                            variationNew.Number = variationInfo.Number;
                            //#---11/10/2019
                            variationNew.BudgetProvider = variationInfo.BudgetProvider;
                            //#---11/10/2019
                            variationNew.Amount = variationInfo.Amount;

                            variationInfoList.Add(variationNew);
                        }
                    }

                    sectionValue = "";

                    foreach (VariationInfo variationInfo in variationInfoList)
                        if (variationInfo.Number != null)
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " " + variationInfo.Number + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";
                        //#---11/10/2019
                        else if (variationInfo.Type == "CV" && variationInfo.BudgetProvider != null)
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " - " + variationInfo.BudgetProvider.BudgetName + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";
                        //#---11/10/2019


                        //#--03/05/2022
                        else if (variationInfo.Type == "SA" && variationInfo.BudgetProvider != null)
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " - " + variationInfo.BudgetProvider.BudgetName + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";

                        //#--03/05/2022

                        else
                            sectionValue += variationInfo.TradeCode + " " + variationInfo.Type + " " + UI.Utils.SetFormDecimal(variationInfo.Amount) + "<br />";

                    return sectionValue;

                case VariableTradesList:

                    sectionValue = String.Empty;
                    String budgetName;

                    foreach (TradeBudgetInfo tradeBudgetInfo in tradeInfo.TradeBudgets)
                    {
                        if (tradeBudgetInfo.BudgetProvider.BudgetType == BudgetType.BOQ)
                            budgetName = tradeBudgetInfo.BudgetProvider.BudgetType.ToString();
                        else
                            budgetName = String.Format("{0} {1}", tradeBudgetInfo.BudgetProvider.BudgetType.ToString(), tradeBudgetInfo.BudgetProvider.BudgetName);

                        sectionValue += "<tr>\n" +
                                        "  <td class='Text' nowrap>" + budgetName + "</td>\n" +
                                        "  <td class='Text' nowrap>" + tradeBudgetInfo.TradeCode + "</td>\n" +
                                        "  <td class='Text' align='right' nowrap>" + UI.Utils.SetFormDecimal(tradeBudgetInfo.Amount) + "</td>\n" +
                                        "</tr>\n";
                    }

                    if (sectionValue != String.Empty)
                        sectionValue = "<table cellpadding='2' cellspacing='0' style='border:solid; border-width:1px; border-color:#AAAAAA'>\n" + sectionValue + "</table>\n";

                    return sectionValue;

                default: return null;
            }
        }








        //#---Draft Contract Template
        public String MergeDraftTemplate(TradeInfo tradeInfo, String template, String conversionType)
        {
            StringBuilder stringBuilder = new StringBuilder(template);
            List<String> templateVariables;
            String sectionValue;
            String sectionInfo;

            //if (CheckTradeForTemplate(tradeInfo, template).DocumentElement != null)
            //    throw new Exception("There are missing fields in the trade.");

            templateVariables = GetTemplateVariables(template);

            foreach (String templateVariable in templateVariables)
            {
                if (templateVariable.Contains("Project") || templateVariable.Contains("Trade"))  //|| templateVariable.Contains("Scope")
                {
                    sectionValue = GetTradeVariable(tradeInfo, GetTemplateVariableName(templateVariable));

                    //if (conversionType == "Edit")
                    //    sectionInfo = TagEditableOpen + TagVariable + GetTemplateVariableName(templateVariable) + " " + TagTitle + GetTemplateVariableTitle(templateVariable) + TagEditableEnd + sectionValue + TagEditableClose;
                    //else
                    sectionInfo = sectionValue;

                    stringBuilder.Replace(templateVariable, sectionInfo);
                }
            }

            if (conversionType == "Edit")
                return stringBuilder.ToString();
            else
            {
                String fullFooter = GetTemplateFooterFull(template);
                String textFooter = GetTemplateFooterText(template);

                if (conversionType == "View")
                {
                    if (fullFooter != null)
                        stringBuilder.Replace(fullFooter, "<span class='TemplateFooter'>" + textFooter + "</span>");
                }
                //else if (conversionType == "Print")
                //{
                //    if (fullFooter != null)
                //        stringBuilder.Replace(fullFooter, "");
                //}

                return stringBuilder.ToString();
            }
        }

        //#---Draft Contract Template




        /// <summary>
        /// Builds a contract based on the template
        /// </summary>
        public void BuildContract(TradeInfo tradeInfo)
        {
            tradeInfo.Contract.Template = MergeTemplateEditatle(tradeInfo, tradeInfo.Contract.Template);
        }

        /// <summary>
        /// Returns a contract latest version
        /// </summary>
        public String BuildContractWithModifications(ContractInfo contractInfo, Boolean IsSummary)
        {
            List<String> editableSections = GetTemplateEditables(contractInfo.Template);
            StringBuilder stringBuilder = new StringBuilder(contractInfo.Template);
            ContractModificationInfo contractModification;
            String originalText;
            String sectionName;
            String newText;

            originalText = GetTemplateFooterFull(contractInfo.Template);
            if (originalText != null)
                stringBuilder.Replace(originalText, "");

            originalText = GetTemplateFinancialFull(contractInfo.Template);
            newText = IsSummary ? "" : GetTemplateFinancialText(contractInfo.Template);
            if (originalText != null)
                stringBuilder.Replace(originalText, newText);

            originalText = GetTemplateTermsFull(contractInfo.Template);
            newText = IsSummary ? "" : GetTemplateTermsText(contractInfo.Template);
            if (originalText != null)
                stringBuilder.Replace(originalText, newText);


           


            foreach (String editableSection in editableSections)
            {
                sectionName = GetTemplateEditableName(editableSection);

                if (contractInfo.ContractModifications != null)
                    contractModification = contractInfo.ContractModifications.Find(delegate(ContractModificationInfo contractModificationFind) { return contractModificationFind.SectionName == sectionName; });
                else
                    contractModification = null;

                originalText = GetTemplateEditableFullSection(contractInfo.Template, editableSection);
                newText = contractModification != null ? contractModification.SectionText : GetTemplateEditableTextSection(contractInfo.Template, editableSection);
                //#---12/11/2018
               

                if (newText.Contains("\r\n"))
                {
                    newText = newText.Replace(System.Environment.NewLine, "<br />");
                }
                //#--12/11/2018
                stringBuilder.Replace(originalText, newText);
            }


            //#----to remove 'aë0$' from contract page
            stringBuilder.Replace('–', '-');
            stringBuilder.Replace('“', '"');
            stringBuilder.Replace('”', '"');
            stringBuilder.Replace('•', '.');
            stringBuilder.Replace("’", "'");
            //#--  to remove 'aë0$' from contract page

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns a contract latest version
        /// </summary>
        public String BuildContractWithModifications(ContractInfo contractInfo)
        {
            return BuildContractWithModifications(contractInfo, false);
        }

        /// <summary>
        /// Returns a contract latest version
        /// </summary>
        public String BuildContractSummaryWithModifications(ContractInfo contractInfo)
        {
            return BuildContractWithModifications(contractInfo, true);
        }

        /// <summary>
        /// Verifies if a contact has the right to see a contract. Current user must be a Contact and must have a subcontractor
        /// </summary>
        public Boolean AllowViewCurrentUser(ContractInfo contractInfo)
        {
            ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();
            return contactInfo.SubContractor.Equals(contractInfo.Trade.SelectedSubContractor) && contractInfo.IsApproved;
        }

        /// <summary>
        /// Throws and exception if the current user can not view information
        /// </summary>
        public void CheckViewCurrentUser(ContractInfo contractInfo)
        {
            if (!AllowViewCurrentUser(contractInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Verifies if a contact has the right to see a trade participation.
        /// </summary>
        public Boolean AllowViewCurrentUser(TradeParticipationInfo tradeParticipationInfo)
        {
            ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();
            return contactInfo.SubContractor.Equals(tradeParticipationInfo.SubContractor) && tradeParticipationInfo.IsVisible;
        }

        /// <summary>
        /// Throws and exception if the current user can not view information
        /// </summary>
        public void CheckViewCurrentUser(TradeParticipationInfo tradeParticipationInfo)
        {
            if (!AllowViewCurrentUser(tradeParticipationInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Verifies if a contact has the right to edit a trade participation.
        /// </summary>
        public Boolean AllowEditCurrentUser(TradeParticipationInfo tradeParticipationInfo)
        {
            ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();
            return contactInfo.SubContractor.Equals(tradeParticipationInfo.SubContractor) && tradeParticipationInfo.IsEditable;
        }

        /// <summary>
        /// Throws and exception if the current user can not edit information
        /// </summary>
        public void CheckEditCurrentUser(TradeParticipationInfo tradeParticipationInfo)
        {
            if (!AllowEditCurrentUser(tradeParticipationInfo))
                throw new SecurityException();
        }
#endregion

        #region Contracts Modifications Methods
        /// <summary>
        /// Creates a Contract Modification from a dr
        /// </summary>
        public ContractModificationInfo CreateContractModification(IDataReader dr)
        {
            ContractModificationInfo contractModificationInfo = new ContractModificationInfo(Data.Utils.GetDBInt32(dr["ContractModificationId"]));

            contractModificationInfo.SectionName = Data.Utils.GetDBString(dr["SectionName"]);
            contractModificationInfo.SectionText = Data.Utils.GetDBString(dr["SectionText"]);

            AssignAuditInfo(contractModificationInfo, dr);

            if (dr["ContractId"] != DBNull.Value)
            {
                contractModificationInfo.Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                if (dr["TradeId"] != DBNull.Value)
                {
                    contractModificationInfo.Contract.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    contractModificationInfo.Contract.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                    if (dr["ProjectId"] != DBNull.Value)
                    {
                        contractModificationInfo.Contract.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                        contractModificationInfo.Contract.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    }
                }
            }

            if (dr["CreatedPeopleId"] != DBNull.Value)
                contractModificationInfo.Employee = (EmployeeInfo)PeopleController.GetInstance().GetPersonById(Data.Utils.GetDBInt32(dr["CreatedPeopleId"]));

            return contractModificationInfo;
        }

        /// <summary>
        /// Get a contract modification from persistent storage
        /// </summary>
        public ContractModificationInfo GetContractModification(int? ContractModificationId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetContractModification(ContractModificationId);
                if (dr.Read())
                    return CreateContractModification(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract Modification from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get contract modifications for a contract from persistent storage
        /// </summary>
        public List<ContractModificationInfo> GetContractModifications(ContractInfo contractInfo)
        {
            IDataReader dr = null;
            List<ContractModificationInfo> contractModificationInfoList = new List<ContractModificationInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetContractModifications(contractInfo.Id);
                while (dr.Read())
                    contractModificationInfoList.Add(CreateContractModification(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Modifications for Contract from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return contractModificationInfoList;
        }

        /// <summary>
        /// Adds a contract modification to the database
        /// </summary>
        public int? AddContractModification(ContractModificationInfo contractModificationInfo)
        {
            int? contractModificationId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(contractModificationInfo);

            parameters.Add(GetId(contractModificationInfo.Contract));
            parameters.Add(contractModificationInfo.SectionName);
            parameters.Add(contractModificationInfo.SectionText);
            parameters.Add(contractModificationInfo.CreatedDate);
            parameters.Add(contractModificationInfo.CreatedBy);

            try
            {
                contractModificationId = Data.DataProvider.GetInstance().AddContractModification(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Contract Modification to database");
            }

            return contractModificationId;
        }
#endregion

#region Variations Methods
        /// <summary>
        /// Creates a Variation from a dr
        /// </summary>
        public VariationInfo CreateVariation(IDataReader dr)
        {
            IBudget iBudget;

            if (dr["BudgetId"] != DBNull.Value)
            {
                iBudget = new BudgetInfo(Data.Utils.GetDBInt32(dr["BudgetId"]));
            }
            else if (dr["ClientVariationTradeId"] != DBNull.Value)
            {
                iBudget = new ClientVariationTradeInfo(Data.Utils.GetDBInt32(dr["ClientVariationTradeId"]));
            }
            else
            {
                iBudget = null;
            }

            VariationInfo variationInfo = new VariationInfo(Data.Utils.GetDBInt32(dr["VariationId"]), iBudget);

            variationInfo.Header = Data.Utils.GetDBString(dr["Header"]);
            variationInfo.Description = Data.Utils.GetDBString(dr["Description"]);
            variationInfo.TradeCode = Data.Utils.GetDBString(dr["TradeCode"]);
            variationInfo.Type = Data.Utils.GetDBString(dr["Type"]);
            variationInfo.Amount = Data.Utils.GetDBDecimal(dr["Amount"]);
            variationInfo.Allowance = Data.Utils.GetDBDecimal(dr["Allowance"]);  // Same as BudgetAmountInitial and BudgetAmountAllowance
            variationInfo.BudgetAmountTradeInitial = Data.Utils.GetDBDecimal(dr["BudgetAmountTradeInitial"]);
            variationInfo.Number = Data.Utils.GetDBString(dr["Number"]);

            AssignAuditInfo(variationInfo, dr);

            if (dr["ContractId"] != DBNull.Value)
            {
                variationInfo.Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                if (dr["ParentContractId"] != DBNull.Value)
                    variationInfo.Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));

                if (dr["TradeId"] != DBNull.Value)
                {
                    variationInfo.Contract.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    variationInfo.Contract.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);

                    if (dr["ProjectId"] != DBNull.Value)
                    {
                        variationInfo.Contract.Trade.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                        variationInfo.Contract.Trade.Project.Name = Data.Utils.GetDBString(dr["ProjectName"]);
                    }
                }
            }

            return variationInfo;
        }

        /// <summary>
        /// Get a Variation from persistent storage
        /// </summary>
        /// 
        public VariationInfo GetVariation(int? VariationId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetVariation(VariationId);
                if (dr.Read())
                    return CreateVariation(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Variation from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get Variations for a contract from persistent storage
        /// </summary>
        public List<VariationInfo> GetVariations(ContractInfo contractInfo)
        {
            IDataReader dr = null;
            List<VariationInfo> variationInfoList = new List<VariationInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetVariations(contractInfo.Id);
                while (dr.Read())
                    variationInfoList.Add(CreateVariation(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Variations from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return variationInfoList;
        }

        /// <summary>
        /// Updates a variation in the database
        /// </summary>
        public void UpdateVariation(VariationInfo variationInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(variationInfo);

            parameters.Add(variationInfo.Id);
            parameters.Add(variationInfo.Header);
            parameters.Add(variationInfo.Description);
            parameters.Add(variationInfo.TradeCode);
            parameters.Add(variationInfo.Type);
            parameters.Add(variationInfo.Amount);
            parameters.Add(variationInfo.Allowance);
            parameters.Add(variationInfo.BudgetAmountTradeInitial);
            parameters.Add(variationInfo.Number);

            if (variationInfo.BudgetProvider is BudgetInfo)
            {
                parameters.Add(variationInfo.BudgetProvider.Id);
                parameters.Add(null);
            }
            else if (variationInfo.BudgetProvider is ClientVariationTradeInfo)
            {
                parameters.Add(null);
                parameters.Add(variationInfo.BudgetProvider.Id);
            }
            else
            {
                parameters.Add(null);
                parameters.Add(null);
            }

            parameters.Add(variationInfo.ModifiedDate);
            parameters.Add(variationInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateVariation(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Variation in database");
            }
        }

        /// <summary>
        /// Updates a variation ClientVariationTrade in the database
        /// </summary>
        public void UpdateVariationClientVariationTrade(Int32? oldClientVariationTradeId, Int32? newClientVariationTradeId)
        {
            List<Object> parameters = new List<Object>();
            VariationInfo variationInfo = new VariationInfo();

            SetModifiedInfo(variationInfo);

            parameters.Add(oldClientVariationTradeId);
            parameters.Add(newClientVariationTradeId);
            parameters.Add(variationInfo.ModifiedDate);
            parameters.Add(variationInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateVariationClientVariationTradeId(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Variation's Client Variation Trade in database");
            }
        }
        
        /// <summary>
        /// Adds a Variation to the database
        /// </summary>
        public int? AddVariation(VariationInfo variationInfo)
        {
            int? VariationId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(variationInfo);

            parameters.Add(GetId(variationInfo.Contract));
            parameters.Add(variationInfo.Header);
            parameters.Add(variationInfo.Description);
            parameters.Add(variationInfo.TradeCode);
            parameters.Add(variationInfo.Type);
            parameters.Add(variationInfo.Amount);
            parameters.Add(variationInfo.Allowance);
            parameters.Add(variationInfo.BudgetAmountTradeInitial);
            parameters.Add(variationInfo.Number);

            if (variationInfo.BudgetProvider is BudgetInfo)
            {
                parameters.Add(variationInfo.BudgetProvider.Id);
                parameters.Add(null);
            }
            else if (variationInfo.BudgetProvider is ClientVariationTradeInfo)
            {
                parameters.Add(null);
                parameters.Add(variationInfo.BudgetProvider.Id);
            }
            else
            {
                parameters.Add(null);
                parameters.Add(null);
            }

            parameters.Add(variationInfo.CreatedDate);
            parameters.Add(variationInfo.CreatedBy);

            try
            {
                VariationId = Data.DataProvider.GetInstance().AddVariation(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Variation to database");
            }

            return VariationId;
        }

        /// <summary>
        /// Adds or Updates a Variation in the database
        /// </summary>
        public int? AddUpdateVariation(VariationInfo variationInfo)
        {
            if (variationInfo != null)
            {
                if (variationInfo.Id != null)
                {
                    UpdateVariation(variationInfo);
                    return variationInfo.Id;
                }
                else
                {
                    return AddVariation(variationInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Variation from persistent storage
        /// </summary>
        public void DeleteVariation(VariationInfo variationInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteVariation(variationInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Variation from database");
            }
        }
#endregion

#endregion

    }
}
