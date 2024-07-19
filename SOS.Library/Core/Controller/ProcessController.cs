using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Transactions;
using System.Configuration;
using System.Collections.Generic;

using System.Web;

using SOS.Data;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SOS.Core
{
    public sealed class ProcessController : Controller
    {

        #region Private Members
        private static ProcessController instance;
        #endregion

        #region Private Methods
        private ProcessController()
        {
        }

        private Boolean IsDateInRange(DateTime? theDate, DateTime? startDate, DateTime? endDate)
        {
            if (theDate == null)
            {
                return startDate == null && endDate == null;
            }
            else
            {
                if (startDate != null)
                {
                    if (endDate != null)
                    {
                        return theDate >= startDate && theDate <= endDate;
                    }
                    else
                    {
                        return theDate >= startDate;
                    }
                }
                else
                {
                    if (endDate != null)
                    {
                        return theDate <= endDate;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        #endregion

        #region Public Methods

        public static ProcessController GetInstance()
        {
            if (instance == null)
                instance = new ProcessController();

            return instance;
        }

        #region Process Methods
        /// <summary>
        /// Creates a ProcessInfo object from a dr
        /// </summary>
        public ProcessInfo CreateProcess(IDataReader dr)
        {
            ProcessInfo processInfo = new ProcessInfo(Data.Utils.GetDBInt32(dr["ProcessId"]));

            processInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            processInfo.StepComparisonApproval = Data.Utils.GetDBString(dr["StepComparisonApproval"]);
            processInfo.StepContractApproval = Data.Utils.GetDBString(dr["StepContractApproval"]);

            AssignAuditInfo(processInfo, dr);

            return processInfo;
        }

        /// <summary>
        /// Creates a ProcessInfo object from a dr or retrieve it from the dictionary
        /// </summary>
        public ProcessInfo CreateProcess(Object dbId, Dictionary<int, ProcessInfo> processesDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (processesDictionary == null)
                return GetInstance().GetDeepProcess(Id);
            else if (processesDictionary.ContainsKey(Id))
                return processesDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Creates a ProcessTemplateInfo object from a dr
        /// </summary>
        public ProcessTemplateInfo CreateProcessTemplate(IDataReader dr)
        {
            ProcessTemplateInfo processTemplateInfo = new ProcessTemplateInfo(Data.Utils.GetDBInt32(dr["ProcessId"]));

            processTemplateInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            processTemplateInfo.ProcessType = Data.Utils.GetDBString(dr["TemplateType"]);

            if (dr["JobTypeId"] != DBNull.Value)
                processTemplateInfo.JobType = new JobTypeInfo(Data.Utils.GetDBInt32(dr["JobTypeId"]));

            AssignAuditInfo(processTemplateInfo, dr);

            return processTemplateInfo;
        }

        /// <summary>
        /// Gets a process from database
        /// </summary>
        public ProcessInfo GetProcess(int? processInfoId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcess(processInfoId);
                if (dr.Read())
                    return CreateProcess(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Gets all the process templates
        /// </summary>
        public List<ProcessTemplateInfo> GetProcessTemplates()
        {
            IDataReader dr = null;
            List<ProcessTemplateInfo> processTemplateInfoList = new List<ProcessTemplateInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessTemplates();
                while (dr.Read())
                    processTemplateInfoList.Add(CreateProcessTemplate(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process Templates from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return processTemplateInfoList;
        }

        /// <summary>
        /// Gets all the process templates for an specified unit
        /// </summary>
        public List<ProcessTemplateInfo> GetProcessTemplates(BusinessUnitInfo businessUnitInfo)
        {
            IDataReader dr = null;
            List<ProcessTemplateInfo> processTemplateInfoList = new List<ProcessTemplateInfo>();

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessTemplatesForBusinessUnit(businessUnitInfo.Id);
                while (dr.Read())
                    processTemplateInfoList.Add(CreateProcessTemplate(dr));
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process Templates for Business Unit from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return processTemplateInfoList;
        }

        /// <summary>
        /// Gets a process template by type.
        /// </summary>
        private ProcessInfo GetDeepProcessTemplate(String type)
        {
            IDataReader dr = null;
            ProcessInfo processInfo = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessTemplatesByType(type);

                if (dr.Read())
                {
                    processInfo = CreateProcess(dr);
                    processInfo.Steps = GetProcessSteps(processInfo);
                }

                if (dr.Read())
                    throw new Exception("There is more than one Process Template for type " + type + ".");
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process Template for type " + type + " from database.");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return processInfo;
        }


        /// <summary>
        /// Gets a process template for Client Variations.
        /// </summary>
        public ProcessInfo GetDeepProcessTemplateForClientVariations(ClientVariationInfo clientVariationInfo)
        {
            return GetDeepProcessTemplate(clientVariationInfo.ProcessType);
        }

        /// <summary>
        /// Gets a process template for Claims.
        /// </summary>
        public ProcessInfo GetDeepProcessTemplateForClaims()
        {
            return GetDeepProcessTemplate(ProcessTemplateInfo.ProcessTypeClaim);
        }

        /// <summary>
        /// Gets a process from database based on a Business Unit, Job type and process type.
        /// The parameters can not be null
        /// </summary>
        public ProcessInfo GetProcessTemplate(BusinessUnitInfo businessUnitInfo, JobTypeInfo jobTypeInfo, String templateType)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(businessUnitInfo.Id);
            parameters.Add(jobTypeInfo.Id);
            parameters.Add(templateType);

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessTemplate(parameters.ToArray());
                if (dr.Read())
                    return CreateProcess(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a process with steps from database
        /// </summary>
        public ProcessInfo GetDeepProcess(int? processId)
        {
            ProcessInfo processInfo = GetProcess(processId);
            processInfo.Steps = GetProcessSteps(processInfo);
            processInfo.Reversals = GetReversals(processInfo);

            if (processInfo.Reversals != null && processInfo.Steps != null)
                foreach (ReversalInfo reversal in processInfo.Reversals)
                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        if (reversal.ProcessStep.Equals(processStep))
                        {
                            processStep.Reversals.Add(reversal);
                            break;
                        }

            return processInfo;
        }

        /// <summary>
        /// Get a process with steps and trade project people from database
        /// </summary>
        public ProcessInfo GetDeepProcessWithProjectPeople(TradeInfo tradeInfo)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessInfoForTrade(tradeInfo.Id);
                if (dr.Read())
                {
                    if (dr["ProcessId"] != DBNull.Value)
                    {
                        ProcessInfo processInfo = GetDeepProcess(Data.Utils.GetDBInt32(dr["ProcessId"]));
                        processInfo.Project = ProjectsController.GetInstance().GetProjectPeople(Data.Utils.GetDBInt32(dr["ProjectId"]), Data.Utils.GetDBInt32(dr["PMPeopleId"]), Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                        return processInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Trade process from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a process with steps and contract project people from database
        /// </summary>
        public ProcessInfo GetDeepProcessWithProjectPeople(ContractInfo contractInfo)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessInfoForContract(contractInfo.Id);
                if (dr.Read())
                {
                    if (dr["ProcessId"] != DBNull.Value)
                    {
                        ProcessInfo processInfo = GetDeepProcess(Data.Utils.GetDBInt32(dr["ProcessId"]));
                        processInfo.Project = ProjectsController.GetInstance().GetProjectPeople(Data.Utils.GetDBInt32(dr["ProjectId"]), Data.Utils.GetDBInt32(dr["PMPeopleId"]), Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                        return processInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Contract process from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a ClientVariation process with steps and project people from database
        /// </summary>
        public ProcessInfo GetDeepProcessWithProjectPeople(ClientVariationInfo clientVariationInfo)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessInfoForClientVariation(clientVariationInfo.Id);
                if (dr.Read())
                {
                    if (dr["ProcessId"] != DBNull.Value)
                    {
                        ProcessInfo processInfo = GetDeepProcess(Data.Utils.GetDBInt32(dr["ProcessId"]));
                        processInfo.Project = ProjectsController.GetInstance().GetProjectPeople(Data.Utils.GetDBInt32(dr["ProjectId"]));

                        return processInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Client Variation process from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a Claim process with steps and project people from database
        /// </summary>
        public ProcessInfo GetDeepProcessWithProjectPeople(ClaimInfo claimInfo)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessInfoForClaim(claimInfo.Id);
                if (dr.Read())
                {
                    if (dr["ProcessId"] != DBNull.Value)
                    {
                        ProcessInfo processInfo = GetDeepProcess(Data.Utils.GetDBInt32(dr["ProcessId"]));
                        processInfo.Project = ProjectsController.GetInstance().GetProjectPeople(Data.Utils.GetDBInt32(dr["ProjectId"]));

                        return processInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Claim process from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Get a process template with steps from database
        /// </summary>
        public ProcessInfo GetDeepProcessTemplate(BusinessUnitInfo businessUnitInfo, JobTypeInfo jobTypeInfo, String templateType)
        {
            ProcessInfo processInfo = GetProcessTemplate(businessUnitInfo, jobTypeInfo, templateType);

            if (processInfo != null)
                processInfo.Steps = GetProcessSteps(processInfo);

            return processInfo;
        }

        /// <summary>
        /// Adds a Process to the database
        /// </summary>
        public int? AddProcess(ProcessInfo processInfo)
        {
            int? processId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(processInfo);

            parameters.Add(processInfo.Name);
            parameters.Add(processInfo.StepComparisonApproval);
            parameters.Add(processInfo.StepContractApproval);

            parameters.Add(processInfo.CreatedDate);
            parameters.Add(processInfo.CreatedBy);

            try
            {
                processId = Data.DataProvider.GetInstance().AddProcess(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Process to database");
            }

            return processId;
        }

        /// <summary>
        /// Removes a Process from persistent storage
        /// </summary>
        public void DeleteProcess(ProcessInfo processInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteProcess(processInfo.Id);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());
                throw new Exception("Removing Process from database");
            }
        }

        /// <summary>
        /// Updates a Process Comparison approval step in the database
        /// </summary>
        public void UpdateProcessStepComparisonApproval(ProcessInfo processInfo, String stepComparisonApproval)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(processInfo.Id);
            parameters.Add(stepComparisonApproval);

            try
            {
                Data.DataProvider.GetInstance().UpdateProcessStepComparisonApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Process Comparison Approval Step in database");
            }
        }
        /// <summary>
        /// Updates a Process Comparison approval step in the database  DS2024-03-21
        /// </summary>
        public void UpdateProcessStepContractApproval(ProcessInfo processInfo, String stepContractApproval)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(processInfo.Id);
            parameters.Add(stepContractApproval);

            try
            {
                Data.DataProvider.GetInstance().UpdateProcessStepContractApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Process Contract Approval Step in database");
            }
        }

        //#---14-11-2018
        public void UpdateProcessHideorShow(int ProcessId, bool HideorShow)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(ProcessId);
            parameters.Add(HideorShow);

            try
            {
                Data.DataProvider.GetInstance().UpdateProcessHideorShow(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Process Comparison Hide or Show in database");
            }
        }
        //#---14-11-2018



        #endregion

        #region Process Step Methods
        /// <summary>
        /// Creates a ProcessStepInfo object from a dr
        /// </summary>
        public ProcessStepInfo CreateProcessStep(IDataReader dr, Dictionary<int, PeopleInfo> peopleDictionary)
        {
            ProcessStepInfo processStepInfo = new ProcessStepInfo(Data.Utils.GetDBInt32(dr["ProcessStepId"]));

            processStepInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            processStepInfo.Type = Data.Utils.GetDBString(dr["Type"]);
            processStepInfo.Role = Data.Utils.GetDBString(dr["UserType"]);
            processStepInfo.NumDays = Data.Utils.GetDBInt32(dr["NumDays"]);
            processStepInfo.Skip = Data.Utils.GetDBBoolean(dr["Skip"]);
            processStepInfo.TargetDate = Data.Utils.GetDBDateTime(dr["TargetDate"]);
            processStepInfo.ActualDate = Data.Utils.GetDBDateTime(dr["ActualDate"]);
            processStepInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

            if (dr["ProcessId"] != DBNull.Value)
                processStepInfo.Process = new ProcessInfo(Data.Utils.GetDBInt32(dr["ProcessId"]));

            if (dr["AssignedPeopleId"] != DBNull.Value)
                processStepInfo.AssignedTo = PeopleController.GetInstance().CreatePerson(dr["AssignedPeopleId"], peopleDictionary);

            if (dr["ApprovedPeopleId"] != DBNull.Value)
                processStepInfo.ApprovedBy = PeopleController.GetInstance().CreatePerson(dr["ApprovedPeopleId"], peopleDictionary);

            AssignAuditInfo(processStepInfo, dr);

            return processStepInfo;
        }

        /// <summary>
        /// Creates a ProcessStepInfo object from a dr
        /// </summary>
        public ProcessStepInfo CreateProcessStep(IDataReader dr)
        {
            return CreateProcessStep(dr, null);
        }

        /// <summary>
        /// Gets a ProcessStep from database
        /// </summary>
        public ProcessStepInfo GetProcessStep(int? ProcessStepInfoId)
        {
            IDataReader dr = null;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessStep(ProcessStepInfoId);
                if (dr.Read())
                    return CreateProcessStep(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process Step from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }

        /// <summary>
        /// Gets all the Process Steps for a process from database
        /// </summary>
        public List<ProcessStepInfo> GetProcessSteps(ProcessInfo processInfo)
        {
            IDataReader dr = null;
            int order = 0;
            List<ProcessStepInfo> processStepInfoList = new List<ProcessStepInfo>();
            ProcessStepInfo processStepInfo;

            try
            {
                dr = Data.DataProvider.GetInstance().GetProcessStepsByProcess(processInfo.Id);
                while (dr.Read())
                {
                    processStepInfo = CreateProcessStep(dr);
                    processStepInfo.Order = ++order;
                    processStepInfo.Process = processInfo;
                    processStepInfo.Reversals = new List<ReversalInfo>();
                    processStepInfoList.Add(processStepInfo);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Process Steps from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return processStepInfoList;
        }

        /// <summary>
        /// Get pending current steps for one selected user per project
        /// </summary>
        public Dictionary<int, List<ProcessStepInfo>> GetPendingSteps(List<ProjectInfo> projectInfoList, Dictionary<int, EmployeeInfo> selectedUsers)
        {
            Dictionary<int, List<EmployeeInfo>> selectedUsersList = new Dictionary<int, List<EmployeeInfo>>();

            foreach (ProjectInfo projectInfo in projectInfoList)
                selectedUsersList.Add(projectInfo.Id.Value, new List<EmployeeInfo>() { selectedUsers[projectInfo.Id.Value] });

            return GetPendingSteps(projectInfoList, selectedUsersList, null, null, true);
        }

        /// <summary>
        /// Get pending steps for multiple projects. projectsController must have initialized holidays
        /// </summary>
        public Dictionary<int, List<ProcessStepInfo>> GetPendingSteps(List<ProjectInfo> projectInfoList, Dictionary<int, List<EmployeeInfo>> selectedUsers, DateTime? startDate, DateTime? endDate, bool currentStepOnly)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps = new Dictionary<int, List<ProcessStepInfo>>();
            Dictionary<int, ProcessInfo> processInfoDictionary = new Dictionary<int, ProcessInfo>();
            Dictionary<int, ProjectInfo> projectsInfoDictionary = new Dictionary<int, ProjectInfo>();

            Dictionary<int, int> numActiveClaims = new Dictionary<int, int>();
            Dictionary<int, int> numClientTrades = new Dictionary<int, int>();
            Dictionary<int, int> numClientVariations = new Dictionary<int, int>();

            List<String> rolesPlayList = null;
            HashSet<String> rolesPlayHashSet = null;
            Dictionary<int, List<String>> rolesPlayOneProject;
            Dictionary<int, Dictionary<int, List<String>>> rolesPlayAllProjects = new Dictionary<int, Dictionary<int, List<String>>>();

            Dictionary<int, List<TradeParticipationInfo>> tradeParticipations = new Dictionary<int, List<TradeParticipationInfo>>();

            List<ProcessInfo> processInfoList = new List<ProcessInfo>();
            List<Object> parameters = new List<Object>();
            List<ProcessStepInfo> processStepInfoList;
            List<ProcessStepInfo> processStepInfoList1;
            List<EmployeeInfo> projectUsers;

            EmployeeInfo creatorEmployee;
            TradeParticipationInfo tradeParticipationInfo;
            ProcessInfo processInfo;
            ProcessStepInfo processStepInfo;
            ReversalInfo reversalInfo;

            IDataReader dr = null;

            StringBuilder ids = new StringBuilder();
            StringBuilder userIds = new StringBuilder();
            StringBuilder userTypes = new StringBuilder();

            DateTime? secondBusinessDay;
            DateTime dueDate;

            int projectId;
            int processId;
            int userId;
            int processStepId;
            int currentProjectId;
            int currentProcessId;
            int currentProcessStepId;
            int? reversalId;

            int? ProcessHide;//#---13-11-2018

            Boolean isPM;
            Boolean isCA;

            secondBusinessDay = projectsController.AddbusinessDaysToDate(DateTime.Now, 2);
            projectsController.InitializeHolidays();

            ids.Append("<ProjectsIds>");
            userIds.Append("<ProjectsUserIds>");
            userTypes.Append("<ProjectsUserTypes>");

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;
                pendingProcessSteps.Add(projectId, new List<ProcessStepInfo>());
                tradeParticipations.Add(projectId, new List<TradeParticipationInfo>());
                projectsInfoDictionary.Add(projectId, projectInfo);

                ids.Append("<ProjectId><Id>").Append(projectInfo.IdStr).Append("</Id></ProjectId>");

                projectUsers = selectedUsers[projectId];

                // find all the roles that the selected users play in the project
                if (projectUsers != null)
                {
                    // this hashset contains all the roles for all the users specified for this project
                    rolesPlayHashSet = new HashSet<String>();
                    rolesPlayOneProject = new Dictionary<int, List<String>>();
                    rolesPlayAllProjects.Add(projectId, rolesPlayOneProject);

                    foreach (EmployeeInfo employeeInfo in projectUsers)
                    {
                        // If an employeeInfo doest not have Id we are getting all the pending steps for the role
                        // If this is the case we will not have more than one employee for the project so Id = 0 is not used more than once
                        if (employeeInfo.Id != null)
                        {
                            rolesPlayList = GetAllRolesPlay(employeeInfo, projectInfo);
                            userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(employeeInfo.IdStr).Append("</PeopleId></ProjectUserId>");
                            userId = employeeInfo.Id.Value;
                        }
                        else
                        {
                            rolesPlayList = new List<string>();
                            rolesPlayList.Add(employeeInfo.Role);
                            userId = 0;
                        }

                        rolesPlayOneProject.Add(userId, rolesPlayList);

                        foreach (String role in rolesPlayList)
                            if (!rolesPlayHashSet.Contains(role))
                                rolesPlayHashSet.Add(role);
                    }

                    foreach (String role in rolesPlayHashSet)
                        userTypes.Append("<ProjectUserType><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><UserType>").Append(role).Append("</UserType></ProjectUserType>");
                }
            }




            ids.Append("</ProjectsIds>");
            userIds.Append("</ProjectsUserIds>");
            userTypes.Append("</ProjectsUserTypes>");

            parameters.Add(ids.ToString());
            parameters.Add(userTypes.ToString());
            parameters.Add(userIds.ToString());
            parameters.Add(startDate);
            parameters.Add(endDate);

            try
            {

                dr = Data.DataProvider.GetInstance().GetPendingSteps(parameters.ToArray());

                //GetPendingQuotes
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    tradeParticipationInfo = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));
                    tradeParticipationInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeParticipationInfo.Trade.Project = new ProjectInfo(projectId);
                    tradeParticipationInfo.SubContractor = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"]));

                    tradeParticipationInfo.QuoteDueDate = Data.Utils.GetDBDateTime(dr["QuoteDueDate"]);
                    tradeParticipationInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);
                    tradeParticipationInfo.SubContractor.Name = Data.Utils.GetDBString(dr["SubContractorName"]);
                    tradeParticipationInfo.SubContractor.ShortName = Data.Utils.GetDBString(dr["SubContractorShortName"]);

                    if (dr["PMPeopleId"] != DBNull.Value) tradeParticipationInfo.Trade.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) tradeParticipationInfo.Trade.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    if (IsDateInRange(tradeParticipationInfo.QuoteDueDate, null, secondBusinessDay))
                        tradeParticipations[projectId].Add(tradeParticipationInfo);
                }


                //Processes with steps. Contains all the processed that has at least one pending step in the specified date range
                dr.NextResult();
                currentProcessId = 0;
                currentProcessStepId = 0;
                processInfo = null;
                processStepInfo = null;
                while (dr.Read())
                {
                    processId = Data.Utils.GetDBInt32(dr["ProcessId"]).Value;
                    processStepId = Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value;
                    reversalId = Data.Utils.GetDBInt32(dr["ReversalId"]);
                    ProcessHide = dr["Hide"] != null ? dr["Hide"].ToString() != "" ? Data.Utils.GetDBInt32(dr["Hide"]) : 0 : 0;       // //#---13/11/2018

                    if (processId != currentProcessId)
                    {
                        currentProcessId = processId;

                        processInfo = new ProcessInfo(processId);
                        processInfo.Name = Data.Utils.GetDBString(dr["ProcessName"]);
                        processInfo.Steps = new List<ProcessStepInfo>();
                        //#---13/11/2018
                        processInfo.Hide = ProcessHide;
                        //#---13/11/2018

                        processInfoDictionary.Add(processId, processInfo);
                    }

                    if (processStepId != currentProcessStepId)
                    {
                        currentProcessStepId = processStepId;

                        processStepInfo = new ProcessStepInfo(processStepId);
                        processStepInfo.Reversals = new List<ReversalInfo>();
                        processStepInfo.Name = Data.Utils.GetDBString(dr["ProcessStepName"]);
                        processStepInfo.TargetDate = Data.Utils.GetDBDateTime(dr["TargetDate"]);
                        processStepInfo.ActualDate = Data.Utils.GetDBDateTime(dr["ActualDate"]);
                        processStepInfo.Skip = Data.Utils.GetDBBoolean(dr["Skip"]);
                        processStepInfo.Role = Data.Utils.GetDBString(dr["UserType"]);
                        processStepInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

                        processStepInfo.Process = processInfo;

                        processInfo.Steps.Add(processStepInfo);
                    }

                    if (reversalId != null)
                    {
                        reversalInfo = new ReversalInfo(reversalId);

                        reversalInfo.ReversalDate = Data.Utils.GetDBDateTime(dr["ReverseDate"]);
                        reversalInfo.ReversalNote = Data.Utils.GetDBString(dr["ReverseNote"]);

                        reversalInfo.ReversalBy = new EmployeeInfo();
                        reversalInfo.ReversalBy.FirstName = Data.Utils.GetDBString(dr["ReversedByFirstName"]);
                        reversalInfo.ReversalBy.LastName = Data.Utils.GetDBString(dr["ReversedByLastName"]);

                        processStepInfo.Reversals.Add(reversalInfo);
                    }
                }




                // Trades
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value], processInfo.Project.Trades[0]);

                    processInfoList.Add(processInfo);
                }




                // Contracts and Subcontracts (Variations)
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);
                    processInfo.Project.Trades[0].Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                    if (Data.Utils.GetDBInt32(dr["ParentContractId"]) != null)
                    {
                        processInfo.Project.Trades[0].Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));
                        processInfo.Project.Trades[0].Contract.Description = Data.Utils.GetDBString(dr["Description"]);
                    }

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value], processInfo.Project.Trades[0]);

                    processInfoList.Add(processInfo);
                }



                // Client Variations
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new ClientVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }

                // Tenant Variations  DS20240213 >>>
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new TenantVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }
                // Tenant Variations  DS20240213 <<<

                // Separate accounts
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new SeparateAccountInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }

                // Claims
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Claims = new List<ClaimInfo>();
                    processInfo.Project.Claims.Add(new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"])));
                    processInfo.Project.Claims[0].Number = Data.Utils.GetDBInt32(dr["Number"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }



                foreach (ProcessInfo process in processInfoList)
                {
                    projectId = process.Project.Id.Value;


                    if (currentStepOnly)
                    {
                        processStepInfoList1 = new List<ProcessStepInfo>();

                        processStepInfo = GetCurrentStep(process);

                        if (processStepInfo != null)
                            processStepInfoList1.Add(processStepInfo);
                    }
                    else
                    {
                        processStepInfoList1 = GetPendingSteps(process);
                    }

                    foreach (ProcessStepInfo processStep in processStepInfoList1)
                    {
                        foreach (EmployeeInfo employee in selectedUsers[projectId])
                        {
                            if (employee.Id != null && processStep.AssignedTo != null && employee.Equals(processStep.AssignedTo))
                            {
                                pendingProcessSteps[projectId].Add(processStep);
                                break;
                            }

                            if (employee.Id == null && processStep.AssignedTo == null && employee.Role == processStep.Role)
                            {
                                pendingProcessSteps[projectId].Add(processStep);
                                break;
                            }
                        }
                    }
                }




                // Missing Signed Contract file

                //SAN------Signed contracts file link missing

                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;

                while (dr.Read())
                {

                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();


                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));

                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    processInfo.Project.Trades[0].Type = Data.Utils.GetDBString(dr["TradeType"]);

                    processInfo.Name = "Missing Signed Contract File";


                    //processInfo.Project.Trades[0].Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                    //if (Data.Utils.GetDBInt32(dr["ParentContractId"]) != null)
                    //{
                    //    processInfo.Project.Trades[0].Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));
                    //   // processInfo.Project.Trades[0].Contract.Description = Data.Utils.GetDBString(dr["Description"]);
                    //}


                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["CAPeopleId"] != DBNull.Value && dr["TradeType"] != DBNull.Value)
                    {
                        string Rolee = "";
                        if (dr["TradeType"].ToString() == "Design")
                        {
                            Rolee = "DC";
                        }
                        else { Rolee = "CA"; }

                        processInfo.Steps[0].AssignedTo = GetRoleAssignee(Rolee, projectsInfoDictionary[processInfo.Project.Id.Value]); //new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                        processInfo.Steps[0].Name = Rolee + " - Upload Signed Contract file";

                        ////
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);


                }



                //SAN----Signed contracts file link missing









                // Transmittals
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.Transmittals = new List<TransmittalInfo>();
                    processInfo.Project.Transmittals.Add(new TransmittalInfo());
                    processInfo.Project.Transmittals[0].Contact = new ContactInfo();
                    processInfo.Project.Transmittals[0].Contact.SubContractor = new SubContractorInfo();

                    processInfo.Project.Transmittals[0].Id = Data.Utils.GetDBInt32(dr["TransmittalId"]);
                    processInfo.Project.Transmittals[0].TransmittalType = Data.Utils.GetDBString(dr["TransmittalType"]);
                    processInfo.Project.Transmittals[0].TransmittalTypeOther = Data.Utils.GetDBString(dr["TransmittalTypeOther"]);
                    processInfo.Project.Transmittals[0].TransmittalNumber = Data.Utils.GetDBInt32(dr["TransmittalNumber"]);
                    processInfo.Project.Transmittals[0].TransmissionDate = Data.Utils.GetDBDateTime(dr["TransmissionDate"]);
                    processInfo.Project.Transmittals[0].Contact.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    processInfo.Project.Transmittals[0].Contact.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    processInfo.Project.Transmittals[0].Contact.SubContractor.Name = Data.Utils.GetDBString(dr["Name"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["CreatorPeopleId"] != DBNull.Value)
                    {
                        creatorEmployee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CreatorPeopleId"]));
                        creatorEmployee.FirstName = Data.Utils.GetDBString(dr["CreatorFirstName"]);
                        creatorEmployee.LastName = Data.Utils.GetDBString(dr["CreatorLastName"]);
                        creatorEmployee.LastLoginDate = Data.Utils.GetDBDateTime(dr["CreatorLastLogin"]);

                        processInfo.Steps[0].AssignedTo = creatorEmployee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }



                //RFIs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.RFIs = new List<RFIInfo>();
                    processInfo.Project.RFIs.Add(new RFIInfo());

                    processInfo.Project.RFIs[0].Id = Data.Utils.GetDBInt32(dr["RFIId"]);
                    processInfo.Project.RFIs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.RFIs[0].Subject = Data.Utils.GetDBString(dr["Subject"]);
                    processInfo.Project.RFIs[0].Status = Data.Utils.GetDBString(dr["Status"]);
                    processInfo.Project.RFIs[0].RaiseDate = Data.Utils.GetDBDateTime(dr["RaiseDate"]);
                    processInfo.Project.RFIs[0].TargetAnswerDate = Data.Utils.GetDBDateTime(dr["TargetAnswerDate"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["PeopleId"] != DBNull.Value)
                    {
                        creatorEmployee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                        creatorEmployee.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                        creatorEmployee.LastName = Data.Utils.GetDBString(dr["LastName"]);
                        creatorEmployee.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);

                        processInfo.Steps[0].AssignedTo = creatorEmployee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //EOTs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.EOTs = new List<EOTInfo>();
                    processInfo.Project.EOTs.Add(new EOTInfo());

                    processInfo.Project.EOTs[0].Id = Data.Utils.GetDBInt32(dr["EOTId"]);
                    processInfo.Project.EOTs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.EOTs[0].Cause = Data.Utils.GetDBString(dr["Cause"]);
                    processInfo.Project.EOTs[0].StartDate = Data.Utils.GetDBDateTime(dr["StartDate"]);
                    processInfo.Project.EOTs[0].EndDate = Data.Utils.GetDBDateTime(dr["EndDate"]);
                    processInfo.Project.EOTs[0].FirstNoticeDate = Data.Utils.GetDBDateTime(dr["FirstNoticeDate"]);
                    processInfo.Project.EOTs[0].WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);

                    processInfo.Steps[0].Process = processInfo;
                    processInfo.Steps[0].AssignedTo = GetRoleAssignee(EmployeeInfo.TypeContractsAdministrator, projectsInfoDictionary[projectId]);

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //Number of Claims
                dr.NextResult();
                while (dr.Read())
                {
                    numActiveClaims.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClaims"]).Value);
                }

                //Number of Client trades an Client variations
                dr.NextResult();
                while (dr.Read())
                {
                    numClientTrades.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClientTrades"]).Value);
                    numClientVariations.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClientVariations"]).Value);
                }

                // Add special pending tasks
                foreach (ProjectInfo projectInfo in projectInfoList)
                {
                    projectId = projectInfo.Id.Value;
                    projectUsers = selectedUsers[projectId];
                    processStepInfoList = pendingProcessSteps[projectId];

                    foreach (EmployeeInfo employeeInfo in projectUsers)
                    {
                        rolesPlayOneProject = rolesPlayAllProjects[projectId];
                        userId = employeeInfo.Id == null ? 0 : employeeInfo.Id.Value;

                        isPM = rolesPlayOneProject[userId].Contains(EmployeeInfo.TypeProjectManager);
                        isCA = rolesPlayOneProject[userId].Contains(EmployeeInfo.TypeContractsAdministrator);

                        if (isPM)
                        {
                            // A Claim needs to be added if:
                            // 1. No active claim
                            // 2. The project contract amount has been specified
                            // 3. There are client trades or client variations (something to claim) in the project
                            // 4. The client trades match the contract amount
                            // 5. the last claim is not claiming 100%
                            // 6. If date range specified duedate is project commencement date
                            if (projectInfo.ContractAmount != null && (Decimal)projectInfo.ContractAmount > 0 &&
                                numActiveClaims.ContainsKey(projectId) && numActiveClaims[projectId] == 0 &&
                                (numClientTrades[projectId] > 0 || numClientVariations[projectId] > 0))
                            {
                                projectInfo.Claims = projectsController.GetClaimsWithTradesAndVariations(projectInfo);
                                projectInfo.ClientTrades = projectsController.GetClientTrades(projectInfo);
                                projectInfo.ClientVariations = projectsController.GetClientVariationsLastRevisions(projectInfo);

                                Decimal? totalClientTrades = projectInfo.TotalClientTrades;
                                Decimal? totalLastClaim = projectInfo.TotalLastClaim;

                                if (totalClientTrades != null && (Decimal)totalClientTrades == (Decimal)projectInfo.ContractAmount &&
                                   totalLastClaim != null && (Decimal)totalLastClaim < (Decimal)projectInfo.ContractAmountPlusVariations &&
                                   IsDateInRange(projectInfo.CommencementDate, startDate, endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Project.Claims = new List<ClaimInfo>();
                                    processInfo.Project.Claims.Add(new ClaimInfo());

                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }

                            if (projectInfo.CompletionDate != null)
                            {
                                //Maintenance manuals task due some days before completion date
                                //start showing some days before completion date
                                dueDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysToShowManualLink);
                                if (projectInfo.MaintenanceManualFile == null && IsDateInRange(dueDate, startDate, endDate == null ? DateTime.Now : endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].Name = "Maintenance manuals";
                                    processInfo.Steps[0].TargetDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysDueManualLink);
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }

                                // Post project review due some days after completion date
                                // start showing on completion date
                                dueDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysToShowProjectReview);
                                if (projectInfo.PostProjectReviewFile == null && IsDateInRange(dueDate, startDate, endDate == null ? DateTime.Now : endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].Name = "Post project review";
                                    processInfo.Steps[0].TargetDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysDueProjectReview);
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }
                        }

                        // Quotes reminers
                        if (isCA)
                        {
                            foreach (TradeParticipationInfo tradeParticipation in tradeParticipations[projectId])
                            {
                                // If it's a CA check to see if the trade has a CA if so it must be this employee
                                if ((tradeParticipation.Trade.Project.ContractsAdministrator == null) || employeeInfo.Id == null || (tradeParticipation.Trade.Project.ContractsAdministrator.Equals(employeeInfo)))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Project = new ProjectInfo();
                                    processInfo.Project.Trades = new List<TradeInfo>();
                                    processInfo.Project.Trades.Add(new TradeInfo());
                                    processInfo.Project.Trades[0].Participations = new List<TradeParticipationInfo>();
                                    processInfo.Project.Trades[0].Participations.Add(new TradeParticipationInfo());
                                    processInfo.Project.Trades[0].Participations[0].SubContractor = new SubContractorInfo();

                                    processInfo.Project.Trades[0].Name = tradeParticipation.Trade.Name;
                                    processInfo.Project.Trades[0].Participations[0].Id = tradeParticipation.Id;
                                    processInfo.Project.Trades[0].Participations[0].QuoteDate = tradeParticipation.QuoteDueDate;
                                    processInfo.Project.Trades[0].Participations[0].SubContractor.ShortName = tradeParticipation.SubContractor.ShortName;

                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }
                        }
                    }

                    foreach (ProcessStepInfo processStep in processStepInfoList)
                    {
                        reversalInfo = processStep.PendingReversal;

                        if (reversalInfo != null)
                            processStep.Comments = "Reversed on: " + UI.Utils.SetFormDateTime(reversalInfo.ReversalDate) + " By: " + reversalInfo.ReversalByName + ". " + reversalInfo.ReversalNote;
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Pending Process Steps from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return pendingProcessSteps;
        }





        //#----------For ActivitySummary Report

        public Dictionary<int, List<ProcessStepInfo>> GetPendingStepsForActivityReport(List<ProjectInfo> projectInfoList, Dictionary<int, List<EmployeeInfo>> selectedUsers, DateTime? startDate, DateTime? endDate, bool currentStepOnly)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps = new Dictionary<int, List<ProcessStepInfo>>();
            Dictionary<int, ProcessInfo> processInfoDictionary = new Dictionary<int, ProcessInfo>();
            Dictionary<int, ProjectInfo> projectsInfoDictionary = new Dictionary<int, ProjectInfo>();

            Dictionary<int, int> numActiveClaims = new Dictionary<int, int>();
            Dictionary<int, int> numClientTrades = new Dictionary<int, int>();
            Dictionary<int, int> numClientVariations = new Dictionary<int, int>();

            List<String> rolesPlayList = null;
            HashSet<String> rolesPlayHashSet = null;
            Dictionary<int, List<String>> rolesPlayOneProject;
            Dictionary<int, Dictionary<int, List<String>>> rolesPlayAllProjects = new Dictionary<int, Dictionary<int, List<String>>>();

            Dictionary<int, List<TradeParticipationInfo>> tradeParticipations = new Dictionary<int, List<TradeParticipationInfo>>();

            List<ProcessInfo> processInfoList = new List<ProcessInfo>();
            List<Object> parameters = new List<Object>();
            List<ProcessStepInfo> processStepInfoList;
            List<ProcessStepInfo> processStepInfoList1;
            List<EmployeeInfo> projectUsers;

            EmployeeInfo creatorEmployee;
            TradeParticipationInfo tradeParticipationInfo;
            ProcessInfo processInfo;
            ProcessStepInfo processStepInfo;
            ReversalInfo reversalInfo;

            IDataReader dr = null;

            StringBuilder ids = new StringBuilder();
            StringBuilder userIds = new StringBuilder();
            StringBuilder userTypes = new StringBuilder();

            DateTime? secondBusinessDay;
            DateTime dueDate;

            int projectId;
            int processId;
            int userId;
            int processStepId;
            int currentProjectId;
            int currentProcessId;
            int currentProcessStepId;
            int? reversalId;

            Boolean isPM;
            Boolean isCA;

            secondBusinessDay = projectsController.AddbusinessDaysToDate(DateTime.Now, 2);
            projectsController.InitializeHolidays();

            ids.Append("<ProjectsIds>");
            userIds.Append("<ProjectsUserIds>");
            userTypes.Append("<ProjectsUserTypes>");

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;
                pendingProcessSteps.Add(projectId, new List<ProcessStepInfo>());
                tradeParticipations.Add(projectId, new List<TradeParticipationInfo>());
                projectsInfoDictionary.Add(projectId, projectInfo);

                ids.Append("<ProjectId><Id>").Append(projectInfo.IdStr).Append("</Id></ProjectId>");

                projectUsers = selectedUsers[projectId];

                // find all the roles that the selected users play in the project
                if (projectUsers != null)
                {
                    // this hashset contains all the roles for all the users specified for this project
                    rolesPlayHashSet = new HashSet<String>();
                    rolesPlayOneProject = new Dictionary<int, List<String>>();
                    rolesPlayAllProjects.Add(projectId, rolesPlayOneProject);

                    foreach (EmployeeInfo employeeInfo in projectUsers)
                    {
                        // If an employeeInfo doest not have Id we are getting all the pending steps for the role
                        // If this is the case we will not have more than one employee for the project so Id = 0 is not used more than once
                        if (employeeInfo.Id != null)
                        {
                            rolesPlayList = GetAllRolesPlay(employeeInfo, projectInfo);
                            userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(employeeInfo.IdStr).Append("</PeopleId></ProjectUserId>");
                            userId = employeeInfo.Id.Value;
                        }
                        else
                        {
                            rolesPlayList = new List<string>();
                            rolesPlayList.Add(employeeInfo.Role);
                            userId = 0;
                        }

                        rolesPlayOneProject.Add(userId, rolesPlayList);

                        foreach (String role in rolesPlayList)
                            if (!rolesPlayHashSet.Contains(role))
                                rolesPlayHashSet.Add(role);
                    }

                    foreach (String role in rolesPlayHashSet)
                        userTypes.Append("<ProjectUserType><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><UserType>").Append(role).Append("</UserType></ProjectUserType>");
                }
            }

            ids.Append("</ProjectsIds>");
            userIds.Append("</ProjectsUserIds>");
            userTypes.Append("</ProjectsUserTypes>");

            parameters.Add(ids.ToString());
            parameters.Add(userTypes.ToString());
            parameters.Add(userIds.ToString());
            parameters.Add(startDate);
            parameters.Add(endDate);

            try
            {

                dr = Data.DataProvider.GetInstance().GetPendingStepsForActivityReport(parameters.ToArray());

                //GetPendingQuotes
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    tradeParticipationInfo = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));
                    tradeParticipationInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeParticipationInfo.Trade.Project = new ProjectInfo(projectId);
                    tradeParticipationInfo.SubContractor = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"]));

                    tradeParticipationInfo.QuoteDueDate = Data.Utils.GetDBDateTime(dr["QuoteDueDate"]);
                    tradeParticipationInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);
                    tradeParticipationInfo.SubContractor.Name = Data.Utils.GetDBString(dr["SubContractorName"]);
                    tradeParticipationInfo.SubContractor.ShortName = Data.Utils.GetDBString(dr["SubContractorShortName"]);

                    if (dr["PMPeopleId"] != DBNull.Value) tradeParticipationInfo.Trade.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) tradeParticipationInfo.Trade.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    if (IsDateInRange(tradeParticipationInfo.QuoteDueDate, null, secondBusinessDay))
                        tradeParticipations[projectId].Add(tradeParticipationInfo);
                }

                //Processes with steps. Contains all the processed that has at least one pending step in the specified date range
                dr.NextResult();
                currentProcessId = 0;
                currentProcessStepId = 0;
                processInfo = null;
                processStepInfo = null;
                while (dr.Read())
                {
                    processId = Data.Utils.GetDBInt32(dr["ProcessId"]).Value;
                    processStepId = Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value;
                    reversalId = Data.Utils.GetDBInt32(dr["ReversalId"]);

                    if (processId != currentProcessId)
                    {
                        currentProcessId = processId;

                        processInfo = new ProcessInfo(processId);
                        processInfo.Name = Data.Utils.GetDBString(dr["ProcessName"]);
                        processInfo.Steps = new List<ProcessStepInfo>();

                        processInfoDictionary.Add(processId, processInfo);
                    }

                    if (processStepId != currentProcessStepId)
                    {
                        currentProcessStepId = processStepId;

                        processStepInfo = new ProcessStepInfo(processStepId);
                        processStepInfo.Reversals = new List<ReversalInfo>();
                        processStepInfo.Name = Data.Utils.GetDBString(dr["ProcessStepName"]);
                        processStepInfo.TargetDate = Data.Utils.GetDBDateTime(dr["TargetDate"]);
                        processStepInfo.ActualDate = Data.Utils.GetDBDateTime(dr["ActualDate"]);
                        processStepInfo.Skip = Data.Utils.GetDBBoolean(dr["Skip"]);
                        processStepInfo.Role = Data.Utils.GetDBString(dr["UserType"]);
                        processStepInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

                        processStepInfo.Process = processInfo;

                        processInfo.Steps.Add(processStepInfo);
                    }

                    if (reversalId != null)
                    {
                        reversalInfo = new ReversalInfo(reversalId);

                        reversalInfo.ReversalDate = Data.Utils.GetDBDateTime(dr["ReverseDate"]);
                        reversalInfo.ReversalNote = Data.Utils.GetDBString(dr["ReverseNote"]);

                        reversalInfo.ReversalBy = new EmployeeInfo();
                        reversalInfo.ReversalBy.FirstName = Data.Utils.GetDBString(dr["ReversedByFirstName"]);
                        reversalInfo.ReversalBy.LastName = Data.Utils.GetDBString(dr["ReversedByLastName"]);

                        processStepInfo.Reversals.Add(reversalInfo);
                    }
                }

                // Trades
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value], processInfo.Project.Trades[0]);

                    processInfoList.Add(processInfo);
                }

                // Contracts and Subcontracts (Variations)
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);
                    processInfo.Project.Trades[0].Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                    if (Data.Utils.GetDBInt32(dr["ParentContractId"]) != null)
                    {
                        processInfo.Project.Trades[0].Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));
                        processInfo.Project.Trades[0].Contract.Description = Data.Utils.GetDBString(dr["Description"]);
                    }

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value], processInfo.Project.Trades[0]);

                    processInfoList.Add(processInfo);
                }

                // Client Variations
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new ClientVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }

                // Separate accounts
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new SeparateAccountInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }

                // Claims
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Claims = new List<ClaimInfo>();
                    processInfo.Project.Claims.Add(new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"])));
                    processInfo.Project.Claims[0].Number = Data.Utils.GetDBInt32(dr["Number"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }




                //------------------Filtering ProcessSteps to next level  -----------------------------------------

                foreach (ProcessInfo process in processInfoList)
                {
                    projectId = process.Project.Id.Value;

                    if (currentStepOnly)
                    {
                        processStepInfoList1 = new List<ProcessStepInfo>();

                        processStepInfo = GetCurrentStep(process);

                        if (processStepInfo != null)
                            processStepInfoList1.Add(processStepInfo);
                    }
                    else
                    {
                        processStepInfoList1 = GetPendingSteps(process);
                    }

                    foreach (ProcessStepInfo processStep in processStepInfoList1)
                    {
                        foreach (EmployeeInfo employee in selectedUsers[projectId])
                        {
                            if (employee.Id != null && processStep.AssignedTo != null && employee.Equals(processStep.AssignedTo))
                            {
                                pendingProcessSteps[projectId].Add(processStep);
                                break;
                            }

                            if (employee.Id == null && processStep.AssignedTo == null && employee.Role == processStep.Role)
                            {
                                pendingProcessSteps[projectId].Add(processStep);
                                break;
                            }
                        }
                    }
                }





                // Transmittals
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.Transmittals = new List<TransmittalInfo>();
                    processInfo.Project.Transmittals.Add(new TransmittalInfo());
                    processInfo.Project.Transmittals[0].Contact = new ContactInfo();
                    processInfo.Project.Transmittals[0].Contact.SubContractor = new SubContractorInfo();

                    processInfo.Project.Transmittals[0].Id = Data.Utils.GetDBInt32(dr["TransmittalId"]);
                    processInfo.Project.Transmittals[0].TransmittalType = Data.Utils.GetDBString(dr["TransmittalType"]);
                    processInfo.Project.Transmittals[0].TransmittalTypeOther = Data.Utils.GetDBString(dr["TransmittalTypeOther"]);
                    processInfo.Project.Transmittals[0].TransmittalNumber = Data.Utils.GetDBInt32(dr["TransmittalNumber"]);
                    processInfo.Project.Transmittals[0].TransmissionDate = Data.Utils.GetDBDateTime(dr["TransmissionDate"]);
                    processInfo.Project.Transmittals[0].Contact.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    processInfo.Project.Transmittals[0].Contact.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    processInfo.Project.Transmittals[0].Contact.SubContractor.Name = Data.Utils.GetDBString(dr["Name"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["CreatorPeopleId"] != DBNull.Value)
                    {
                        creatorEmployee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CreatorPeopleId"]));
                        creatorEmployee.FirstName = Data.Utils.GetDBString(dr["CreatorFirstName"]);
                        creatorEmployee.LastName = Data.Utils.GetDBString(dr["CreatorLastName"]);
                        creatorEmployee.LastLoginDate = Data.Utils.GetDBDateTime(dr["CreatorLastLogin"]);

                        processInfo.Steps[0].AssignedTo = creatorEmployee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //RFIs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.RFIs = new List<RFIInfo>();
                    processInfo.Project.RFIs.Add(new RFIInfo());

                    processInfo.Project.RFIs[0].Id = Data.Utils.GetDBInt32(dr["RFIId"]);
                    processInfo.Project.RFIs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.RFIs[0].Subject = Data.Utils.GetDBString(dr["Subject"]);
                    processInfo.Project.RFIs[0].Status = Data.Utils.GetDBString(dr["Status"]);
                    processInfo.Project.RFIs[0].RaiseDate = Data.Utils.GetDBDateTime(dr["RaiseDate"]);
                    processInfo.Project.RFIs[0].TargetAnswerDate = Data.Utils.GetDBDateTime(dr["TargetAnswerDate"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["PeopleId"] != DBNull.Value)
                    {
                        creatorEmployee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                        creatorEmployee.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                        creatorEmployee.LastName = Data.Utils.GetDBString(dr["LastName"]);
                        creatorEmployee.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);

                        processInfo.Steps[0].AssignedTo = creatorEmployee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //EOTs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.EOTs = new List<EOTInfo>();
                    processInfo.Project.EOTs.Add(new EOTInfo());

                    processInfo.Project.EOTs[0].Id = Data.Utils.GetDBInt32(dr["EOTId"]);
                    processInfo.Project.EOTs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.EOTs[0].Cause = Data.Utils.GetDBString(dr["Cause"]);
                    processInfo.Project.EOTs[0].StartDate = Data.Utils.GetDBDateTime(dr["StartDate"]);
                    processInfo.Project.EOTs[0].EndDate = Data.Utils.GetDBDateTime(dr["EndDate"]);
                    processInfo.Project.EOTs[0].FirstNoticeDate = Data.Utils.GetDBDateTime(dr["FirstNoticeDate"]);
                    processInfo.Project.EOTs[0].WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);

                    processInfo.Steps[0].Process = processInfo;
                    processInfo.Steps[0].AssignedTo = GetRoleAssignee(EmployeeInfo.TypeContractsAdministrator, projectsInfoDictionary[projectId]);

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //Number of Claims
                dr.NextResult();
                while (dr.Read())
                {
                    numActiveClaims.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClaims"]).Value);
                }

                //Number of Client trades an Client variations
                dr.NextResult();
                while (dr.Read())
                {
                    numClientTrades.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClientTrades"]).Value);
                    numClientVariations.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClientVariations"]).Value);
                }

                // Add special pending tasks
                foreach (ProjectInfo projectInfo in projectInfoList)
                {
                    projectId = projectInfo.Id.Value;
                    projectUsers = selectedUsers[projectId];
                    processStepInfoList = pendingProcessSteps[projectId];

                    foreach (EmployeeInfo employeeInfo in projectUsers)
                    {
                        rolesPlayOneProject = rolesPlayAllProjects[projectId];
                        userId = employeeInfo.Id == null ? 0 : employeeInfo.Id.Value;

                        isPM = rolesPlayOneProject[userId].Contains(EmployeeInfo.TypeProjectManager);
                        isCA = rolesPlayOneProject[userId].Contains(EmployeeInfo.TypeContractsAdministrator);

                        if (isPM)
                        {
                            // A Claim needs to be added if:
                            // 1. No active claim
                            // 2. The project contract amount has been specified
                            // 3. There are client trades or client variations (something to claim) in the project
                            // 4. The client trades match the contract amount
                            // 5. the last claim is not claiming 100%
                            // 6. If date range specified duedate is project commencement date
                            if (projectInfo.ContractAmount != null && (Decimal)projectInfo.ContractAmount > 0 &&
                                numActiveClaims.ContainsKey(projectId) && numActiveClaims[projectId] == 0 &&
                                (numClientTrades[projectId] > 0 || numClientVariations[projectId] > 0))
                            {
                                projectInfo.Claims = projectsController.GetClaimsWithTradesAndVariations(projectInfo);
                                projectInfo.ClientTrades = projectsController.GetClientTrades(projectInfo);
                                projectInfo.ClientVariations = projectsController.GetClientVariationsLastRevisions(projectInfo);

                                Decimal? totalClientTrades = projectInfo.TotalClientTrades;
                                Decimal? totalLastClaim = projectInfo.TotalLastClaim;

                                if (totalClientTrades != null && (Decimal)totalClientTrades == (Decimal)projectInfo.ContractAmount &&
                                   totalLastClaim != null && (Decimal)totalLastClaim < (Decimal)projectInfo.ContractAmountPlusVariations &&
                                   IsDateInRange(projectInfo.CommencementDate, startDate, endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Project.Claims = new List<ClaimInfo>();
                                    processInfo.Project.Claims.Add(new ClaimInfo());

                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }

                            if (projectInfo.CompletionDate != null)
                            {
                                //Maintenance manuals task due some days before completion date
                                //start showing some days before completion date
                                dueDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysToShowManualLink);
                                if (projectInfo.MaintenanceManualFile == null && IsDateInRange(dueDate, startDate, endDate == null ? DateTime.Now : endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].Name = "Maintenance manuals";
                                    processInfo.Steps[0].TargetDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysDueManualLink);
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }

                                // Post project review due some days after completion date
                                // start showing on completion date
                                dueDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysToShowProjectReview);
                                if (projectInfo.PostProjectReviewFile == null && IsDateInRange(dueDate, startDate, endDate == null ? DateTime.Now : endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].Name = "Post project review";
                                    processInfo.Steps[0].TargetDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysDueProjectReview);
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }
                        }

                        // Quotes reminers
                        if (isCA)
                        {
                            foreach (TradeParticipationInfo tradeParticipation in tradeParticipations[projectId])
                            {
                                // If it's a CA check to see if the trade has a CA if so it must be this employee
                                if ((tradeParticipation.Trade.Project.ContractsAdministrator == null) || employeeInfo.Id == null || (tradeParticipation.Trade.Project.ContractsAdministrator.Equals(employeeInfo)))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Project = new ProjectInfo();
                                    processInfo.Project.Trades = new List<TradeInfo>();
                                    processInfo.Project.Trades.Add(new TradeInfo());
                                    processInfo.Project.Trades[0].Participations = new List<TradeParticipationInfo>();
                                    processInfo.Project.Trades[0].Participations.Add(new TradeParticipationInfo());
                                    processInfo.Project.Trades[0].Participations[0].SubContractor = new SubContractorInfo();

                                    processInfo.Project.Trades[0].Name = tradeParticipation.Trade.Name;
                                    processInfo.Project.Trades[0].Participations[0].Id = tradeParticipation.Id;
                                    processInfo.Project.Trades[0].Participations[0].QuoteDate = tradeParticipation.QuoteDueDate;
                                    processInfo.Project.Trades[0].Participations[0].SubContractor.ShortName = tradeParticipation.SubContractor.ShortName;

                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }
                        }
                    }

                    foreach (ProcessStepInfo processStep in processStepInfoList)
                    {
                        reversalInfo = processStep.PendingReversal;

                        if (reversalInfo != null)
                            processStep.Comments = "Reversed on: " + UI.Utils.SetFormDateTime(reversalInfo.ReversalDate) + " By: " + reversalInfo.ReversalByName + ". " + reversalInfo.ReversalNote;
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Pending Process Steps from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return pendingProcessSteps;
        }


        //#----------For ActivitySummary Report






        /// <summary>
        /// Get executed steps for multiple projects. It does not include Trade participations
        /// </summary>
        public Dictionary<int, List<ProcessStepInfo>> GetExecutedSteps(List<ProjectInfo> projectInfoList, Dictionary<int, List<EmployeeInfo>> selectedUsers, DateTime? startDate, DateTime? endDate)
        {
            Dictionary<int, ProcessStepInfo> processSteps = new Dictionary<int, ProcessStepInfo>();
            Dictionary<int, List<ProcessStepInfo>> projectsProcessSteps = new Dictionary<int, List<ProcessStepInfo>>();

            List<Object> parameters = new List<Object>();
            List<ProcessStepInfo> processStepInfoList;

            EmployeeInfo employee;
            ProcessInfo processInfo;
            ProcessStepInfo processStepInfo;
            ReversalInfo reversalInfo;

            IDataReader dr = null;

            StringBuilder projectIds = new StringBuilder();
            StringBuilder userIds = new StringBuilder();

            int projectId;
            int processStepId;
            int currentProjectId;
            int currentProcessStepId;
            int? reversalId;

            projectIds.Append("<ProjectsIds>");
            userIds.Append("<ProjectsUserIds>");

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;
                projectsProcessSteps.Add(projectId, new List<ProcessStepInfo>());

                projectIds.Append("<ProjectId><Id>").Append(projectInfo.IdStr).Append("</Id></ProjectId>");

                if (selectedUsers[projectId] != null)
                    foreach (EmployeeInfo employeeInfo in selectedUsers[projectId])
                        userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(employeeInfo.IdStr).Append("</PeopleId></ProjectUserId>");
            }

            projectIds.Append("</ProjectsIds>");
            userIds.Append("</ProjectsUserIds>");

            parameters.Add(projectIds.ToString());
            parameters.Add(userIds.ToString());
            parameters.Add(startDate);
            parameters.Add(endDate);

            try
            {
                dr = Data.DataProvider.GetInstance().GetExecutedSteps(parameters.ToArray());

                //Process steps
                currentProcessStepId = 0;
                processStepInfo = null;
                while (dr.Read())
                {
                    processStepId = Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value;
                    reversalId = Data.Utils.GetDBInt32(dr["ReversalId"]);

                    if (processStepId != currentProcessStepId)
                    {
                        currentProcessStepId = processStepId;

                        processStepInfo = new ProcessStepInfo(processStepId);
                        processStepInfo.Reversals = new List<ReversalInfo>();
                        processStepInfo.AssignedTo = new EmployeeInfo(Data.Utils.GetDBInt32(dr["AssignedPeopleId"]));
                        processStepInfo.ApprovedBy = new EmployeeInfo(Data.Utils.GetDBInt32(dr["ApprovedPeopleId"]));
                        processStepInfo.Name = Data.Utils.GetDBString(dr["ProcessStepName"]);
                        processStepInfo.TargetDate = Data.Utils.GetDBDateTime(dr["TargetDate"]);
                        processStepInfo.ActualDate = Data.Utils.GetDBDateTime(dr["ActualDate"]);
                        processStepInfo.Skip = Data.Utils.GetDBBoolean(dr["Skip"]);
                        processStepInfo.Role = Data.Utils.GetDBString(dr["UserType"]);
                        processStepInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

                        processStepInfo.AssignedTo.FirstName = Data.Utils.GetDBString(dr["AssignedToFirstName"]);
                        processStepInfo.AssignedTo.LastName = Data.Utils.GetDBString(dr["AssignedToLastName"]);
                        processStepInfo.AssignedTo.LastLoginDate = Data.Utils.GetDBDateTime(dr["AssignedToUserLastLogin"]);

                        processStepInfo.ApprovedBy.FirstName = Data.Utils.GetDBString(dr["ApprovedByFirstName"]);
                        processStepInfo.ApprovedBy.LastName = Data.Utils.GetDBString(dr["ApprovedByLastName"]);
                        processStepInfo.ApprovedBy.LastLoginDate = Data.Utils.GetDBDateTime(dr["ApprovedByUserLastLogin"]);

                        processStepInfo.Process = new ProcessInfo(Data.Utils.GetDBInt32(dr["ProcessId"]));
                        processStepInfo.Process.Name = Data.Utils.GetDBString(dr["ProcessName"]);

                        processSteps.Add(processStepId, processStepInfo);
                    }

                    if (reversalId != null)
                    {
                        reversalInfo = new ReversalInfo(reversalId);

                        reversalInfo.ReversalDate = Data.Utils.GetDBDateTime(dr["ReverseDate"]);
                        reversalInfo.ReversalNote = Data.Utils.GetDBString(dr["ReverseNote"]);

                        reversalInfo.ReversalBy = new EmployeeInfo();
                        reversalInfo.ReversalBy.FirstName = Data.Utils.GetDBString(dr["ReversedByFirstName"]);
                        reversalInfo.ReversalBy.LastName = Data.Utils.GetDBString(dr["ReversedByLastName"]);

                        processStepInfo.Reversals.Add(reversalInfo);
                    }
                }

                // Trades
                dr.NextResult();
                while (dr.Read())
                {
                    processStepInfo = processSteps[Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value];

                    processInfo = processStepInfo.Process;
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    projectsProcessSteps[processInfo.Project.Id.Value].Add(processStepInfo);
                }

                // Contracts and Subcontracts (Variations)
                dr.NextResult();
                while (dr.Read())
                {
                    processStepInfo = processSteps[Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value];

                    processInfo = processStepInfo.Process;
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);
                    processInfo.Project.Trades[0].Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                    if (Data.Utils.GetDBInt32(dr["ParentContractId"]) != null)
                    {
                        processInfo.Project.Trades[0].Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));
                        processInfo.Project.Trades[0].Contract.Description = Data.Utils.GetDBString(dr["Description"]);
                    }

                    projectsProcessSteps[processInfo.Project.Id.Value].Add(processStepInfo);
                }

                // Client Variations
                dr.NextResult();
                while (dr.Read())
                {
                    processStepInfo = processSteps[Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value];

                    processInfo = processStepInfo.Process;
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new ClientVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    projectsProcessSteps[processInfo.Project.Id.Value].Add(processStepInfo);
                }

                // Separate accounts
                dr.NextResult();
                while (dr.Read())

                {
                    processStepInfo = processSteps[Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value];

                    processInfo = processStepInfo.Process;
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new SeparateAccountInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    projectsProcessSteps[processInfo.Project.Id.Value].Add(processStepInfo);
                }

                // Claims
                dr.NextResult();
                while (dr.Read())
                {
                    processStepInfo = processSteps[Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value];

                    processInfo = processStepInfo.Process;
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Claims = new List<ClaimInfo>();
                    processInfo.Project.Claims.Add(new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"])));
                    processInfo.Project.Claims[0].Number = Data.Utils.GetDBInt32(dr["Number"]);

                    projectsProcessSteps[processInfo.Project.Id.Value].Add(processStepInfo);
                }

                // Transmittals
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = projectsProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Transmittals = new List<TransmittalInfo>();
                    processInfo.Project.Transmittals.Add(new TransmittalInfo());
                    processInfo.Project.Transmittals[0].Contact = new ContactInfo();
                    processInfo.Project.Transmittals[0].Contact.SubContractor = new SubContractorInfo();

                    processInfo.Project.Transmittals[0].Id = Data.Utils.GetDBInt32(dr["TransmittalId"]);
                    processInfo.Project.Transmittals[0].TransmittalType = Data.Utils.GetDBString(dr["TransmittalType"]);
                    processInfo.Project.Transmittals[0].TransmittalTypeOther = Data.Utils.GetDBString(dr["TransmittalTypeOther"]);
                    processInfo.Project.Transmittals[0].TransmittalNumber = Data.Utils.GetDBInt32(dr["TransmittalNumber"]);
                    processInfo.Project.Transmittals[0].TransmissionDate = Data.Utils.GetDBDateTime(dr["TransmissionDate"]);
                    processInfo.Project.Transmittals[0].SentDate = Data.Utils.GetDBDateTime(dr["SentDate"]);
                    processInfo.Project.Transmittals[0].Contact.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    processInfo.Project.Transmittals[0].Contact.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    processInfo.Project.Transmittals[0].Contact.SubContractor.Name = Data.Utils.GetDBString(dr["Name"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["CreatorPeopleId"] != DBNull.Value)
                    {
                        employee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CreatorPeopleId"]));
                        employee.FirstName = Data.Utils.GetDBString(dr["CreatorFirstName"]);
                        employee.LastName = Data.Utils.GetDBString(dr["CreatorLastName"]);
                        employee.LastLoginDate = Data.Utils.GetDBDateTime(dr["CreatorLastLogin"]);

                        processInfo.Steps[0].ApprovedBy = employee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //RFIs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = projectsProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.RFIs = new List<RFIInfo>();
                    processInfo.Project.RFIs.Add(new RFIInfo());

                    processInfo.Project.RFIs[0].Id = Data.Utils.GetDBInt32(dr["RFIId"]);
                    processInfo.Project.RFIs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.RFIs[0].Subject = Data.Utils.GetDBString(dr["Subject"]);
                    processInfo.Project.RFIs[0].Status = Data.Utils.GetDBString(dr["Status"]);
                    processInfo.Project.RFIs[0].RaiseDate = Data.Utils.GetDBDateTime(dr["RaiseDate"]);
                    processInfo.Project.RFIs[0].TargetAnswerDate = Data.Utils.GetDBDateTime(dr["TargetAnswerDate"]);
                    processInfo.Project.RFIs[0].ActualAnswerDate = Data.Utils.GetDBDateTime(dr["ActualAnswerDate"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["PeopleId"] != DBNull.Value)
                    {
                        employee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                        employee.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                        employee.LastName = Data.Utils.GetDBString(dr["LastName"]);
                        employee.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);

                        processInfo.Steps[0].ApprovedBy = employee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //EOTs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = projectsProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.EOTs = new List<EOTInfo>();
                    processInfo.Project.EOTs.Add(new EOTInfo());

                    processInfo.Project.EOTs[0].Id = Data.Utils.GetDBInt32(dr["EOTId"]);
                    processInfo.Project.EOTs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.EOTs[0].Cause = Data.Utils.GetDBString(dr["Cause"]);
                    processInfo.Project.EOTs[0].StartDate = Data.Utils.GetDBDateTime(dr["StartDate"]);
                    processInfo.Project.EOTs[0].EndDate = Data.Utils.GetDBDateTime(dr["EndDate"]);
                    processInfo.Project.EOTs[0].FirstNoticeDate = Data.Utils.GetDBDateTime(dr["FirstNoticeDate"]);
                    processInfo.Project.EOTs[0].WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);
                    processInfo.Project.EOTs[0].ApprovalDate = Data.Utils.GetDBDateTime(dr["ApprovalDate"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["PeopleId"] != DBNull.Value)
                    {
                        employee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                        employee.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                        employee.LastName = Data.Utils.GetDBString(dr["LastName"]);
                        employee.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);

                        processInfo.Steps[0].ApprovedBy = employee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting executed Process Steps from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            foreach (ProcessStepInfo processStep in processSteps.Values)
            {
                reversalInfo = processStep.PendingReversal;

                if (reversalInfo != null)
                    processStep.Comments = "Reversed on: " + UI.Utils.SetFormDateTime(reversalInfo.ReversalDate) + " By: " + reversalInfo.ReversalByName + ". " + reversalInfo.ReversalNote;
            }

            return projectsProcessSteps;
        }

        /// <summary>
        /// For every project returns the list of employess the current user can supervise including him/her self
        /// </summary>
        public void GetEmpoyeesAndRoles(List<ProjectInfo> projectInfoList, EmployeeInfo currentUser, String rolesInfo, out Dictionary<int, EmployeeInfo> selectedUsers, out Dictionary<int, List<EmployeeInfo>> selectableEmployees, out Dictionary<String, String> dictionaryRoles)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            List<EmployeeInfo> employeeInfoList;
            List<EmployeeInfo> employeeInfoList1;
            List<EmployeeInfo> employeeInfoList2;
            List<String> rolesCanPlayList;

            EmployeeInfo selectedUser;
            EmployeeInfo roleAssignee;

            String[] roles;
            String[] idAndRole;

            int projectId;

            // rolesInfo string example: "1:CA,2:PM,3:MD,4:PM123,5:CA100"
            // this comes from the user selection in the home page. PM and CA specify the Id because a project can have mutiple CAs and PMs
            if (rolesInfo != String.Empty)
            {
                dictionaryRoles = new Dictionary<String, String>();
                roles = rolesInfo.Split(',');

                for (int i = 0; i < roles.Length; i++)
                {
                    idAndRole = roles[i].Split(':');
                    dictionaryRoles.Add(idAndRole[0], idAndRole[1]);
                }
            }
            else
            {
                dictionaryRoles = null;
            }

            selectableEmployees = new Dictionary<int, List<EmployeeInfo>>();
            selectedUsers = new Dictionary<int, EmployeeInfo>();

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;
                employeeInfoList = new List<EmployeeInfo>();
                selectableEmployees.Add(projectId, employeeInfoList);
                rolesCanPlayList = GetRolesCanPlay(currentUser, projectInfo);

                // roles in the project
                if (rolesCanPlayList.Count > 0)
                {
                    foreach (String roleName in rolesCanPlayList)
                    {
                        roleAssignee = GetRoleAssignee(roleName, projectInfo);

                        if (roleAssignee == null)
                        {
                            roleAssignee = new EmployeeInfo();
                            roleAssignee.Role = roleName;
                        }

                        employeeInfoList.Add(roleAssignee);

                        // If I can play the PM and I am a PM supervisor add all the PMs
                        if (roleName == EmployeeInfo.TypeProjectManager)
                        {
                            if (CanPlayRole(currentUser, EmployeeInfo.TypeConstructionsManager, projectInfo))
                            {
                                foreach (EmployeeInfo employee in projectInfo.TradesProjectManagers)
                                    if (!employee.Equals(projectInfo.ProjectManager))
                                        employeeInfoList.Add(employee);
                            }
                        }


                        // If I can play the CA role: if I am a PM just add the CAs that I supervise, if I am a PM supervisor add all the CAs
                        else if (roleName == EmployeeInfo.TypeContractsAdministrator)
                        {
                            if (CanPlayRole(currentUser, EmployeeInfo.TypeConstructionsManager, projectInfo))
                            {
                                foreach (EmployeeInfo employee in projectInfo.TradesContractsAdministrators)
                                    if (!employee.Equals(projectInfo.ContractsAdministrator))
                                        employeeInfoList.Add(employee);
                            }
                            else if (CanPlayRole(currentUser, EmployeeInfo.TypeProjectManager, projectInfo))
                            {
                                employeeInfoList1 = projectsController.GetProjectTradesCAs(projectInfo, projectInfo.ProjectManager);
                                employeeInfoList2 = projectsController.GetProjectTradesCAs(projectInfo);

                                foreach (EmployeeInfo employee in employeeInfoList1)
                                    if (!employee.Equals(projectInfo.ContractsAdministrator))
                                        employeeInfoList.Add(employee);

                                foreach (EmployeeInfo employee in employeeInfoList2)
                                    if (!employee.Equals(projectInfo.ContractsAdministrator))
                                        employeeInfoList.Add(employee);
                            }
                        }
                    }
                }

                // I can still play a role if I am a tade PM
                roleAssignee = projectInfo.TradesProjectManagers.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Equals(currentUser); });

                if (roleAssignee != null)
                {
                    // Add me if I am a trade PM and I am not in the list and add all my CAs
                    if (employeeInfoList.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Id.Equals(roleAssignee.Id) && employeeInfoInList.Role.Equals(roleAssignee.Role); }) == null)
                        employeeInfoList.Add(roleAssignee);

                    employeeInfoList1 = projectsController.GetProjectTradesCAs(projectInfo, roleAssignee);

                    foreach (EmployeeInfo employee in employeeInfoList1)
                        if (employeeInfoList.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Id.Equals(employee.Id) && employeeInfoInList.Role.Equals(employee.Role); }) == null)
                            employeeInfoList.Add(employee);
                }

                // I can still play a role if I am a tade CA
                roleAssignee = projectInfo.TradesContractsAdministrators.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Equals(currentUser); });
                if (roleAssignee != null)
                    if (employeeInfoList.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Id.Equals(roleAssignee.Id) && employeeInfoInList.Role.Equals(roleAssignee.Role); }) == null)
                        employeeInfoList.Add(roleAssignee);



                if (employeeInfoList.Count > 0)
                {
                    selectedUser = new EmployeeInfo();

                    // If an employee is already selected, keep that selection, if not, select the first employee in the list which should be me
                    if (dictionaryRoles != null)
                        if (dictionaryRoles.ContainsKey(projectInfo.IdStr))
                        {
                            // CAa and PMs have the role name plus the Id
                            if (dictionaryRoles[projectInfo.IdStr].Length > EmployeeInfo.LengthRoleName)
                            {
                                selectedUser.Role = dictionaryRoles[projectInfo.IdStr].Substring(0, EmployeeInfo.LengthRoleName);
                                selectedUser.Id = int.Parse(dictionaryRoles[projectInfo.IdStr].Substring(EmployeeInfo.LengthRoleName));
                            }
                            else
                            {
                                selectedUser.Role = dictionaryRoles[projectInfo.IdStr];
                                selectedUser.Id = null;
                            }
                        }
                        else
                            selectedUser = employeeInfoList[0];
                    else
                        selectedUser = employeeInfoList[0];

                    selectedUsers.Add(projectId, selectedUser);
                }
            }
        }

        /// <summary>
        /// Updates a process target dates based on an final date
        /// </summary>
        public void UpdateProcessDates(ProjectsController projectsController, ProcessInfo processInfo, DateTime? finalDate)
        {
            Int32 numDays = 0;
            Int32 tempDays = 0;

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    tempDays = 1;
                    numDays = 1;
                    for (Int32 i = processInfo.Steps.Count - 1; i >= 0; i--)
                    {
                        if (processInfo.Steps[i].ActualDate == null)
                        {
                            processInfo.Steps[i].TargetDate = projectsController.AddbusinessDaysToDate(finalDate, -numDays);
                            UpdateProcessStep(processInfo.Steps[i]);

                            if (tempDays == 2)
                            {
                                tempDays = 1;
                                numDays++;
                            }
                            else
                                tempDays++;
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating process dates in database");
            }
        }

        /// <summary>
        /// Updates a ProcessStep in the database
        /// </summary>
        public void UpdateProcessStep(ProcessStepInfo ProcessStepInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(ProcessStepInfo);

            parameters.Add(ProcessStepInfo.Id);
            parameters.Add(ProcessStepInfo.Role);
            parameters.Add(ProcessStepInfo.NumDays);
            parameters.Add(ProcessStepInfo.Skip);
            parameters.Add(ProcessStepInfo.TargetDate);
            parameters.Add(ProcessStepInfo.ActualDate);
            parameters.Add(ProcessStepInfo.Comments);
            parameters.Add(GetId(ProcessStepInfo.AssignedTo));
            parameters.Add(GetId(ProcessStepInfo.ApprovedBy));

            parameters.Add(ProcessStepInfo.ModifiedDate);
            parameters.Add(ProcessStepInfo.ModifiedBy);

            try
            {

                Data.DataProvider.GetInstance().UpdateProcessStep(parameters.ToArray());



            }

            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating ProcessStep in database----" + ex.ToString());
            }
        }

        /// <summary>
        /// Updates a ProcessStep Skip in the database
        /// </summary>
        public void UpdateProcessStepSkip(ProcessStepInfo ProcessStepInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(ProcessStepInfo.Id);
            parameters.Add(ProcessStepInfo.Skip);

            try
            {
                Data.DataProvider.GetInstance().UpdateProcessStepSkip(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating ProcessStep Skip in database");
            }
        }



        ///#----- Updates a  ProcessStep Skip,StepContractApproval for a Contract in the database

        public void UpdateProcessStepSkipStepContractApproval(ProcessStepInfo ProcessStepInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(ProcessStepInfo.Id);
            parameters.Add(ProcessStepInfo.Skip);
            parameters.Add(ProcessStepInfo.Process.StepContractApproval);

            try
            {
                Data.DataProvider.GetInstance().UpdateProcessStepSkipStepContractApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating ProcessStep Skip,StepContractApproval in database");
            }
        }
        ///#----- Updates a ProcessStep Skip,StepContractApproval in the database


        ///#----- Updates a  ProcessStep Skip,StepComparisonApproval for a Comparison in the database

        public void UpdateProcessStepSkipStepComparisonApproval(ProcessStepInfo ProcessStepInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(ProcessStepInfo.Id);
            parameters.Add(ProcessStepInfo.Skip);
            parameters.Add(ProcessStepInfo.Process.StepComparisonApproval);

            try
            {
                Data.DataProvider.GetInstance().UpdateProcessStepSkipStepComparisonApproval(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating ProcessStep Skip,StepComparisonApproval in database");
            }
        }
        ///#----- Updates a ProcessStep Skip,StepContractApproval in the database

        /// <summary>
        /// Adds a ProcessStep to the database
        /// </summary>
        public int? AddProcessStep(ProcessStepInfo processStepInfo)
        {
            int? ProcessStepId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(processStepInfo);

            parameters.Add(GetId(processStepInfo.Process));
            parameters.Add(processStepInfo.Type);
            parameters.Add(processStepInfo.Name);
            parameters.Add(processStepInfo.Role);
            parameters.Add(processStepInfo.NumDays);
            parameters.Add(processStepInfo.Skip);
            parameters.Add(processStepInfo.TargetDate);
            //#---
            parameters.Add(processStepInfo.Comments);
            //#---
            parameters.Add(processStepInfo.CreatedDate);
            parameters.Add(processStepInfo.CreatedBy);

            try
            {
                ProcessStepId = Data.DataProvider.GetInstance().AddProcessStep(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding ProcessStep to database");
            }

            return ProcessStepId;
        }

        /// <summary>
        /// Returns the assignee for a role in a project
        /// </summary>
        public EmployeeInfo GetRoleAssignee(String roleName, ProjectInfo projectInfo)
        {
            switch (roleName)
            {
                case EmployeeInfo.TypeBudgetAdministrator:
                    return projectInfo.BudgetAdministrator;

                case EmployeeInfo.TypeFinancialController:
                    return projectInfo.FinancialController;

                case EmployeeInfo.TypeUnitManager:
                    return projectInfo.UnitManager;

                case EmployeeInfo.TypeDirectorAuthorizacion:
                    return projectInfo.DirectorAuthorization;

                //#---COM-
                case EmployeeInfo.TypeCommercialManager:
                    return projectInfo.CommercialManager;
                //#----


                case EmployeeInfo.TypeManagingDirector:
                    return projectInfo.ManagingDirector;

                case EmployeeInfo.TypeConstructionsManager:
                    return projectInfo.ConstructionManager;

                case EmployeeInfo.TypeDesignManager:
                    return projectInfo.DesignManager;

                case EmployeeInfo.TypeProjectManager:
                    return projectInfo.ProjectManager;

                case EmployeeInfo.TypeDesignCoordinator:
                    return projectInfo.DesignCoordinator;

                case EmployeeInfo.TypeContractsAdministrator:
                    return projectInfo.ContractsAdministrator;

                //#--JCA--
                case EmployeeInfo.TypeJuniorContractsAdministrator:
                    return projectInfo.JuniorContractsAdministrator;
                //#--JCA--

                case EmployeeInfo.TypeContractsAdministrator3:
                    return projectInfo.ContractsAdministrator3;

                case EmployeeInfo.TypeContractsAdministrator4:
                    return projectInfo.ContractsAdministrator4;

                case EmployeeInfo.TypeContractsAdministrator5:
                    return projectInfo.ContractsAdministrator5;

                case EmployeeInfo.TypeContractsAdministrator6:
                    return projectInfo.ContractsAdministrator6;


                default: return null;
            }
        }

        /// <summary>
        /// Returns the assignee for a role in a project
        /// </summary>
        public EmployeeInfo GetRoleAssignee(String roleName, ProjectInfo projectInfo, TradeInfo tradeInfo)
        {
            if (roleName == EmployeeInfo.TypeContractsAdministrator && projectInfo.TradesContractsAdministratorsByTrade.ContainsKey(tradeInfo.Id.Value))
                return projectInfo.TradesContractsAdministratorsByTrade[tradeInfo.Id.Value];

            if (roleName == EmployeeInfo.TypeProjectManager && projectInfo.TradesProjectManagersByTrade.ContainsKey(tradeInfo.Id.Value))
                return projectInfo.TradesProjectManagersByTrade[tradeInfo.Id.Value];

            return GetRoleAssignee(roleName, projectInfo);
        }

        /// <summary>
        /// Returns the assignee for a process step according to the role and the project
        /// </summary>
        public EmployeeInfo GetStepAssignee(ProcessStepInfo processStepInfo)
        {
            return GetRoleAssignee(processStepInfo.Role, processStepInfo.Process.Project);
        }

        /// <summary>
        /// Returns the role of an employee in a project
        /// </summary>
        public String GetRoleName(EmployeeInfo employeeInfo, ProjectInfo projectInfo)
        {
            if (projectInfo.BudgetAdministrator != null)
                if (projectInfo.BudgetAdministrator.Equals(employeeInfo))
                    return EmployeeInfo.TypeBudgetAdministrator;

            if (projectInfo.FinancialController != null)
                if (projectInfo.FinancialController.Equals(employeeInfo))
                    return EmployeeInfo.TypeFinancialController;

            if (projectInfo.UnitManager != null)
                if (projectInfo.UnitManager.Equals(employeeInfo))
                    return EmployeeInfo.TypeUnitManager;

            if (projectInfo.DirectorAuthorization != null)
                if (projectInfo.DirectorAuthorization.Equals(employeeInfo))
                    return EmployeeInfo.TypeDirectorAuthorizacion;

            if (projectInfo.ManagingDirector != null)
                if (projectInfo.ManagingDirector.Equals(employeeInfo))
                    return EmployeeInfo.TypeManagingDirector;

            if (projectInfo.ConstructionManager != null)
                if (projectInfo.ConstructionManager.Equals(employeeInfo))
                    return EmployeeInfo.TypeConstructionsManager;

            if (projectInfo.ProjectManager != null)
                if (projectInfo.ProjectManager.Equals(employeeInfo))
                    return EmployeeInfo.TypeProjectManager;

            if (projectInfo.ContractsAdministrator != null)
                if (projectInfo.ContractsAdministrator.Equals(employeeInfo))
                    return EmployeeInfo.TypeContractsAdministrator;

            if (projectInfo.DesignManager != null)
                if (projectInfo.DesignManager.Equals(employeeInfo))
                    return EmployeeInfo.TypeDesignManager;

            if (projectInfo.DesignCoordinator != null)
                if (projectInfo.DesignCoordinator.Equals(employeeInfo))
                    return EmployeeInfo.TypeDesignCoordinator;

            //#----For---CO--- COMMercialManager and JCA 

            if (projectInfo.CommercialManager != null)
                if (projectInfo.CommercialManager.Equals(employeeInfo))
                    return EmployeeInfo.TypeCommercialManager;

            if (projectInfo.JuniorContractsAdministrator != null)
                if (projectInfo.JuniorContractsAdministrator.Equals(employeeInfo))
                    return EmployeeInfo.TypeJuniorContractsAdministrator;

            if (projectInfo.ContractsAdministrator3 != null)
                if (projectInfo.ContractsAdministrator3.Equals(employeeInfo))
                    return EmployeeInfo.TypeContractsAdministrator3;

            if (projectInfo.ContractsAdministrator4 != null)
                if (projectInfo.ContractsAdministrator4.Equals(employeeInfo))
                    return EmployeeInfo.TypeContractsAdministrator4;


            if (projectInfo.ContractsAdministrator5 != null)
                if (projectInfo.ContractsAdministrator5.Equals(employeeInfo))
                    return EmployeeInfo.TypeContractsAdministrator5;

            if (projectInfo.ContractsAdministrator6 != null)
                if (projectInfo.ContractsAdministrator6.Equals(employeeInfo))
                    return EmployeeInfo.TypeContractsAdministrator6;


            //#-----


            return null;
        }

        /// <summary>
        /// See if an employee has the rights to play an specified role in a project
        /// </summary>
        public bool CanPlayRole(EmployeeInfo employeeInfo, String role, ProjectInfo projectInfo)
        {
            switch (role)
            {
                case EmployeeInfo.TypeBudgetAdministrator:
                    return employeeInfo.Equals(projectInfo.BudgetAdministrator);

                case EmployeeInfo.TypeFinancialController:
                    return employeeInfo.Equals(projectInfo.FinancialController);

                case EmployeeInfo.TypeUnitManager:
                    return employeeInfo.Equals(projectInfo.UnitManager);

                case EmployeeInfo.TypeDirectorAuthorizacion:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization);



                case EmployeeInfo.TypeManagingDirector:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector);




                // #--- new role added
                case EmployeeInfo.TypeCommercialManager:
                    return
                            employeeInfo.Equals(projectInfo.UnitManager) ||
                            employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                            employeeInfo.Equals(projectInfo.ManagingDirector) ||
                            employeeInfo.Equals(projectInfo.CommercialManager);

                //#---



                case EmployeeInfo.TypeConstructionsManager:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //#--
                           employeeInfo.Equals(projectInfo.CommercialManager) ||//#----
                           employeeInfo.Equals(projectInfo.ConstructionManager);


                case EmployeeInfo.TypeDesignManager:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //#--
                           employeeInfo.Equals(projectInfo.CommercialManager) ||//#----
                           employeeInfo.Equals(projectInfo.DesignManager);


                case EmployeeInfo.TypeProjectManager:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //#--
                           employeeInfo.Equals(projectInfo.CommercialManager) ||//#----
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager);


                case EmployeeInfo.TypeDesignCoordinator:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //#--
                           employeeInfo.Equals(projectInfo.CommercialManager) ||//#----
                           employeeInfo.Equals(projectInfo.DesignManager) ||
                           employeeInfo.Equals(projectInfo.DesignCoordinator);


                case EmployeeInfo.TypeContractsAdministrator:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           employeeInfo.Equals(projectInfo.CommercialManager) ||//#----COM----
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager) ||
                           employeeInfo.Equals(projectInfo.JuniorContractsAdministrator) ||//#--JCA---
                           employeeInfo.Equals(projectInfo.ContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator3) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator4) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator5) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator6);


                //SAN--JCA------------------------------
                case EmployeeInfo.TypeJuniorContractsAdministrator:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //employeeInfo.Equals(projectInfo.CommercialManager) ||
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.JuniorContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator3) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator4) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator5) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator6);

                //SAN--JCA-------------------------------------


                case EmployeeInfo.TypeContractsAdministrator3:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //employeeInfo.Equals(projectInfo.CommercialManager) ||
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.JuniorContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator3) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator4) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator5) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator6);


                case EmployeeInfo.TypeContractsAdministrator4:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //employeeInfo.Equals(projectInfo.CommercialManager) ||
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.JuniorContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator3) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator4) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator5) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator6);

                case EmployeeInfo.TypeContractsAdministrator5:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //employeeInfo.Equals(projectInfo.CommercialManager) ||
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.JuniorContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator3) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator4) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator5) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator6);

                case EmployeeInfo.TypeContractsAdministrator6:
                    return employeeInfo.Equals(projectInfo.UnitManager) ||
                           employeeInfo.Equals(projectInfo.DirectorAuthorization) ||
                           employeeInfo.Equals(projectInfo.ManagingDirector) ||
                           //employeeInfo.Equals(projectInfo.CommercialManager) ||
                           employeeInfo.Equals(projectInfo.ConstructionManager) ||
                           employeeInfo.Equals(projectInfo.ProjectManager) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.JuniorContractsAdministrator) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator3) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator4) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator5) ||
                           employeeInfo.Equals(projectInfo.ContractsAdministrator6);



                default:
                    return false;
            }
        }

        /// <summary>
        /// Get a list of roles an employee can play in a project
        /// </summary>
        public List<String> GetRolesCanPlay(EmployeeInfo employeeInfo, ProjectInfo projectInfo)
        {
            List<String> rolesList = new List<String>();

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeBudgetAdministrator, projectInfo))
                rolesList.Add(EmployeeInfo.TypeBudgetAdministrator);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeFinancialController, projectInfo))
                rolesList.Add(EmployeeInfo.TypeFinancialController);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeUnitManager, projectInfo))
                rolesList.Add(EmployeeInfo.TypeUnitManager);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeDirectorAuthorizacion, projectInfo))
                rolesList.Add(EmployeeInfo.TypeDirectorAuthorizacion);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeManagingDirector, projectInfo))
                rolesList.Add(EmployeeInfo.TypeManagingDirector);

            //#-----New role COM
            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeCommercialManager, projectInfo))
                rolesList.Add(EmployeeInfo.TypeCommercialManager);
            //#-----New role COM

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeConstructionsManager, projectInfo))
                rolesList.Add(EmployeeInfo.TypeConstructionsManager);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeDesignManager, projectInfo))
                rolesList.Add(EmployeeInfo.TypeDesignManager);



            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeProjectManager, projectInfo))
                rolesList.Add(EmployeeInfo.TypeProjectManager);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeDesignCoordinator, projectInfo))
                rolesList.Add(EmployeeInfo.TypeDesignCoordinator);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeContractsAdministrator, projectInfo))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator);



            //# ----JCA----
            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeJuniorContractsAdministrator, projectInfo))
                rolesList.Add(EmployeeInfo.TypeJuniorContractsAdministrator);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeContractsAdministrator3, projectInfo))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator3);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeContractsAdministrator4, projectInfo))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator4);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeContractsAdministrator5, projectInfo))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator5);

            if (CanPlayRole(employeeInfo, EmployeeInfo.TypeContractsAdministrator6, projectInfo))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator6);

            //#----JCA-----
            return rolesList;
        }

        /// <summary>
        /// Get a list of roles an employee plays in a project
        /// </summary>
        public List<String> GetRolesPlay(EmployeeInfo employeeInfo, ProjectInfo projectInfo)
        {
            List<String> rolesList = new List<String>();

            if (employeeInfo.Equals(projectInfo.BudgetAdministrator))
                rolesList.Add(EmployeeInfo.TypeBudgetAdministrator);

            if (employeeInfo.Equals(projectInfo.FinancialController))
                rolesList.Add(EmployeeInfo.TypeFinancialController);

            if (employeeInfo.Equals(projectInfo.UnitManager))
                rolesList.Add(EmployeeInfo.TypeUnitManager);

            //#---new Role --COM
            if (employeeInfo.Equals(projectInfo.CommercialManager))
                rolesList.Add(EmployeeInfo.TypeCommercialManager);
            //#---new Role --COM

            if (employeeInfo.Equals(projectInfo.DirectorAuthorization))
                rolesList.Add(EmployeeInfo.TypeDirectorAuthorizacion);

            if (employeeInfo.Equals(projectInfo.ManagingDirector))
                rolesList.Add(EmployeeInfo.TypeManagingDirector);

            if (employeeInfo.Equals(projectInfo.ConstructionManager))
                rolesList.Add(EmployeeInfo.TypeConstructionsManager);

            if (employeeInfo.Equals(projectInfo.DesignManager))
                rolesList.Add(EmployeeInfo.TypeDesignManager);

            if (employeeInfo.Equals(projectInfo.ProjectManager))
                rolesList.Add(EmployeeInfo.TypeProjectManager);

            if (employeeInfo.Equals(projectInfo.DesignCoordinator))
                rolesList.Add(EmployeeInfo.TypeDesignCoordinator);

            if (employeeInfo.Equals(projectInfo.ContractsAdministrator))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator);
            //#----JCA---
            if (employeeInfo.Equals(projectInfo.JuniorContractsAdministrator))
                rolesList.Add(EmployeeInfo.TypeJuniorContractsAdministrator);
            //#----JCA---

            if (employeeInfo.Equals(projectInfo.ContractsAdministrator3))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator3);

            if (employeeInfo.Equals(projectInfo.ContractsAdministrator4))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator4);

            if (employeeInfo.Equals(projectInfo.ContractsAdministrator5))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator5);

            if (employeeInfo.Equals(projectInfo.ContractsAdministrator6))
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator6);

            return rolesList;
        }

        /// <summary>
        /// Get a list of roles an employee plays in a project trades
        /// </summary>
        public List<String> GetTradesRolesPlay(EmployeeInfo employeeInfo, ProjectInfo projectInfo)
        {
            List<String> rolesList = new List<String>();

            if (projectInfo.TradesProjectManagers.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Equals(employeeInfo); }) != null)
                rolesList.Add(EmployeeInfo.TypeProjectManager);

            if (projectInfo.TradesContractsAdministrators.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Equals(employeeInfo); }) != null)
                rolesList.Add(EmployeeInfo.TypeContractsAdministrator);

            return rolesList;
        }

        /// <summary>
        /// Get a list of roles an employee plays in a project or trades
        /// </summary>
        public List<String> GetAllRolesPlay(EmployeeInfo employeeInfo, ProjectInfo projectInfo)
        {
            List<String> rolesList = new List<String>();
            List<String> rolesProjectList = GetRolesPlay(employeeInfo, projectInfo);
            List<String> rolesTradesList = GetTradesRolesPlay(employeeInfo, projectInfo);

            foreach (String role in rolesProjectList)
                rolesList.Add(role);

            foreach (String role in rolesTradesList)
                if (!rolesList.Contains(role))
                    rolesList.Add(role);

            return rolesList;
        }

        /// <summary>
        /// Checks if the current user has approval rights on a process step
        /// The step can not be already executed
        /// The current user must exist, be an employee and can play the role assigned to the step in the project
        /// The previous step must be already approved
        /// </summary>
        public bool AllowApprovalCurrentUser(ProcessStepInfo processStepInfo)
        {
            ProcessStepInfo previousStep;
            PeopleInfo user;

            if (processStepInfo.ActualDate == null && !processStepInfo.SkipStep)
            {
                user = Web.Utils.GetCurrentUser();

                if (user != null)
                    if (user is EmployeeInfo)
                        if (CanPlayRole((EmployeeInfo)user, processStepInfo.Role, processStepInfo.Process.Project))
                        {
                            previousStep = GetPreviousProcessStep(processStepInfo);
                            return previousStep == null ? true : previousStep.ActualDate != null;
                        }
            }

            return false;
        }

        /// <summary>
        /// Checks if the current user has permission to approve a process step
        /// </summary>
        public bool PermissionApprovalCurrentUser(ProcessStepInfo processStepInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();
            return user != null && user is EmployeeInfo && CanPlayRole((EmployeeInfo)user, processStepInfo.Role, processStepInfo.Process.Project);
        }

        /// <summary>
        /// Checks if the current user has reversal rights on a process step
        /// The step has to be already executed
        /// The current user must exist, be an employee and can play the role assigned to the step in the project
        /// </summary>
        public bool AllowReversalCurrentUser(ProcessStepInfo processStepInfo)
        {
            PeopleInfo user;

            if (processStepInfo.ActualDate != null)
            {
                user = Web.Utils.GetCurrentUser();

                if (user != null)
                    if (user is EmployeeInfo)
                        if (CanPlayRole((EmployeeInfo)user, processStepInfo.Role, processStepInfo.Process.Project))
                            return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the current user can update information based on a process 
        /// </summary>
        public bool AllowEditCurrentUser(ProcessInfo processInfo)
        {
            ProcessStepInfo processStepInfo;
            PeopleInfo user;

            // If the user is the admin can edit
            user = Web.Utils.GetCurrentUser();
            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;

            // If there is no approval process there is no restriction on updating
            if (processInfo == null)
                return true;

            // Can edit only if there is a current step and it can be approved by the current user
            processStepInfo = GetCurrentStep(processInfo);

            //  to give access to BA    --Temporarly
            if (processInfo.Project.BudgetAdministrator != null)
                if (processInfo.Project.BudgetAdministrator.Equals(user))
                    return true;


            if (processStepInfo != null)
                return AllowApprovalCurrentUser(processStepInfo);

            return false;
        }

        /// Checks if the current user can update information Contact
        //#---23-01-2020
        public bool AllowEditCurrentUser(ContactInfo contactInfo)
        {

            PeopleInfo user;

            // If the user is the admin can edit
            user = Web.Utils.GetCurrentUser();
            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;
            // #--If the user is the Subcontractor admin can edit                                                                         
            if (user.UserType == ContactInfo.SubcontractorAdmin)
                return true;
            // If the user is the VC employee 
            if (user.UserType == PeopleInfo.PeopleTypeEmployee)
                return true;

            return false;
        }

        //#---23-01-2020

        public bool AllowEditTradeItemCategoryCurrentUser(TradeInfo tradeInfo)  // DS20240206
        {
            ProcessInfo processinfo = GetDeepProcessWithProjectPeople(tradeInfo);
            ProjectInfo projectInfo = ProjectsController.GetInstance().GetProject(tradeInfo.Project.Id);
            PeopleInfo user = Web.Utils.GetCurrentUser();
            if (processinfo.Project.ProjectManager.Id != null && processinfo.Project.ProjectManager.Id == user.Id) { return true; }
            if (processinfo.Project.ContractsAdministrator.Id != null && processinfo.Project.ContractsAdministrator.Id == user.Id) { return true; }
            if (projectInfo.ContractsAdministrator.Id != null && projectInfo.ContractsAdministrator.Id == user.Id) { return true; }
            return false;
        }


        /// <summary>
        /// Checks if the current user can update information based on a trade 
        /// </summary>
        public bool AllowEditCurrentUser(TradeInfo tradeInfo)
        {
            return AllowEditCurrentUser(GetDeepProcessWithProjectPeople(tradeInfo));
        }

        /// <summary>
        /// Checks if the current user can update information based on a contrat
        /// </summary>
        public bool AllowEditCurrentUser(ContractInfo contractInfo)
        {
            return AllowEditCurrentUser(GetDeepProcessWithProjectPeople(contractInfo));
        }

        /// Checks if the current user can update information based on a client variation
        /// </summary>
        public bool AllowEditCurrentUser(ClientVariationInfo clientVariationInfo)
        {
            return AllowEditCurrentUser(GetDeepProcessWithProjectPeople(clientVariationInfo));
        }

        /// Checks if the current user can update information based on a claim
        /// </summary>
        public bool AllowEditCurrentUser(ClaimInfo claimInfo)
        {
            return AllowEditCurrentUser(GetDeepProcessWithProjectPeople(claimInfo));
        }

        /// Checks if the current user can update information based on a RFI
        /// </summary>
        public bool AllowEditCurrentUser(RFIInfo rFIInfo)
        {
            return AllowAddRFICurrentUser(rFIInfo.Project);
        }


        //#----
        /// Checks if the current user can update information based on a RFI
        /// </summary>
        public bool AllowEditCurrentUser(MeetingMinutesInfo meetingInfo)
        {
            return AllowAddMeetingMinutesCurrentUser(meetingInfo.Project);
        }
        //#----


        /// Checks if the current user can update information based on a EOT
        /// </summary>
        public bool AllowEditCurrentUser(EOTInfo eOTInfo)
        {
            return AllowAddEOTCurrentUser(eOTInfo.Project);
        }

        /// Checks if the current user can update information based on a Addendum
        /// </summary>
        public bool AllowEditCurrentUser(AddendumInfo addendumInfo)
        {
            return AllowAddAddendumCurrentUser(addendumInfo.Trade.Project);
        }

        /// Checks if the current user can update information based on a Client Trade
        /// </summary>
        public bool AllowEditCurrentUser(ClientTradeInfo clientTradeInfo)
        {
            return AllowAddClientTradeCurrentUser(clientTradeInfo.Project);
        }

        /// Checks if the current user can view the Budget balance
        /// </summary>
        public bool AllowViewBudgetBalanceCurrentUser(ProjectInfo projectInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeManagingDirector, projectInfo) ||
                CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeBudgetAdministrator, projectInfo) ||
                CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeFinancialController, projectInfo) ||
                CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeConstructionsManager, projectInfo) ||
                CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeProjectManager, projectInfo))

                return true;

            return false;
        }

        /// <summary>
        /// Checks if the current user can update information on a project
        /// </summary>
        public bool AllowEditCurrentUser(ProjectInfo projectInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeManagingDirector, projectInfo))
                return true;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeConstructionsManager, projectInfo))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the current user can update information on a site order
        /// </summary>
        public bool AllowEditCurrentUser(SiteOrderInfo siteOrderInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;

            //if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeManagingDirector, siteOrderInfo))
            //    return true;

            //if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeConstructionsManager, siteOrderInfo))
            //    return true;

            return false;
        }

        /// <summary>
        /// Checks if the current user can update files information on a project
        /// </summary>
        public bool AllowEditFilesCurrentUser(ProjectInfo projectInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeProjectManager, projectInfo))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the current user can play any of the project roles
        /// </summary>
        public bool IsCurrentUserInAnyProjectRole(ProjectInfo projectInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user == null)
                return false;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeContractsAdministrator, projectInfo))
                return true;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeDesignCoordinator, projectInfo))
                return true;
            //#-- to allow BA to ADD/Edit/Delete
            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeBudgetAdministrator, projectInfo))
                return true;
            //---#
            if (projectInfo.TradesProjectManagers != null && projectInfo.TradesProjectManagers.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Equals(user); }) != null)
                return true;

            if (projectInfo.TradesContractsAdministrators != null && projectInfo.TradesContractsAdministrators.Find(delegate (EmployeeInfo employeeInfoInList) { return employeeInfoInList.Equals(user); }) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the current user can add trades to a project
        /// </summary>
        public bool AllowAddTradeCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        /// <summary>
        /// Checks if the current user can add client variations to a project
        /// </summary>
        public bool AllowAddClientVariationCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        /// <summary>
        /// Checks if the current user can add RFIs to a project
        /// </summary>
        public bool AllowAddRFICurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        //#----
        /// <summary>
        /// Checks if the current user can add MeetingMinutes to a project
        /// </summary>
        public bool AllowAddMeetingMinutesCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        //#---




        /// <summary>
        /// Checks if the current user can add EOTs to a project
        /// </summary>
        public bool AllowAddEOTCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        /// <summary>
        /// Checks if the current user can add Addendums to any project trade
        /// </summary>
        public bool AllowAddAddendumCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        /// <summary>
        /// Checks if the current user can add Client Trades to a project
        /// </summary>
        public bool AllowAddClientTradeCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        /// <summary>
        /// Checks if the current user can add budget to a project
        /// </summary>
        public bool AllowAddBudgetCurrentUser(ProjectInfo projectInfo)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            if (user.UserType == EmployeeInfo.TypeAdmin)
                return true;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeBudgetAdministrator, projectInfo))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the current user can update drawings information on a project
        /// </summary>
        public bool AllowUpdateDrawingsCurrentUser(ProjectInfo projectInfo)
        {
            return IsCurrentUserInAnyProjectRole(projectInfo);
        }

        /// <summary>
        /// Checks if the current user can add claims to the project.
        /// </summary>
        public bool AllowAddClaim(ProjectInfo projectInfo, out String infoAllow)
        {
            PeopleInfo user = Web.Utils.GetCurrentUser();

            Decimal? totalClientTrades = projectInfo.TotalClientTrades;
            Decimal? totalLastClaim = projectInfo.TotalLastClaim;

            infoAllow = null;

            if (CanPlayRole((EmployeeInfo)user, EmployeeInfo.TypeContractsAdministrator, projectInfo))
                if (projectInfo.ContractAmount == null || (Decimal)projectInfo.ContractAmount == 0)
                    infoAllow = "Contract amount not specifed";
                else
                    if (totalClientTrades == null || (Decimal)totalClientTrades == 0)
                    infoAllow = "No client trades specified";
                else
                        if ((Decimal)totalClientTrades != (Decimal)projectInfo.ContractAmount)
                    infoAllow = "Client trades don't match contract amount";
                else
                            if ((totalLastClaim != null && (Decimal)totalLastClaim >= (Decimal)projectInfo.ContractAmountPlusVariations))
                    infoAllow = "100% Claimed";
                else
                    return true;

            return false;
        }

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(ProcessInfo processInfo)
        {
            if (!AllowEditCurrentUser(processInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(TradeInfo tradeInfo)
        {
            if (!AllowEditCurrentUser(tradeInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(ContractInfo contractInfo)
        {
            if (!AllowEditCurrentUser(contractInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(ClientVariationInfo clientVariationInfo)
        {
            if (!AllowEditCurrentUser(clientVariationInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(ClaimInfo claimInfo)
        {
            if (!AllowEditCurrentUser(claimInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(RFIInfo rFIInfo)
        {
            if (!AllowEditCurrentUser(rFIInfo))
                throw new SecurityException();
        }


        //#---

        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(MeetingMinutesInfo meetingInfo)
        {
            if (!AllowEditCurrentUser(meetingInfo))
                throw new SecurityException();
        }

        //#---



        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(EOTInfo eOTInfo)
        {
            if (!AllowEditCurrentUser(eOTInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update information
        /// </summary>
        public void CheckEditCurrentUser(AddendumInfo addendumInfo)
        {
            if (!AllowEditCurrentUser(addendumInfo.Trade))
                throw new SecurityException();
        }


        public Dictionary<int, List<ProcessStepInfo>> GetPendingStepsForCA(List<ProjectInfo> projectInfoList, Dictionary<int, List<EmployeeInfo>> selectedUsers, DateTime? startDate, DateTime? endDate, bool currentStepOnly)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps = new Dictionary<int, List<ProcessStepInfo>>();
            Dictionary<int, ProcessInfo> processInfoDictionary = new Dictionary<int, ProcessInfo>();
            Dictionary<int, ProjectInfo> projectsInfoDictionary = new Dictionary<int, ProjectInfo>();

            Dictionary<int, int> numActiveClaims = new Dictionary<int, int>();
            Dictionary<int, int> numClientTrades = new Dictionary<int, int>();
            Dictionary<int, int> numClientVariations = new Dictionary<int, int>();

            List<String> rolesPlayList = null;
            HashSet<String> rolesPlayHashSet = null;
            Dictionary<int, List<String>> rolesPlayOneProject;
            Dictionary<int, Dictionary<int, List<String>>> rolesPlayAllProjects = new Dictionary<int, Dictionary<int, List<String>>>();

            Dictionary<int, List<TradeParticipationInfo>> tradeParticipations = new Dictionary<int, List<TradeParticipationInfo>>();

            List<ProcessInfo> processInfoList = new List<ProcessInfo>();
            List<Object> parameters = new List<Object>();
            List<ProcessStepInfo> processStepInfoList;
            List<ProcessStepInfo> processStepInfoList1;
            List<EmployeeInfo> projectUsers;

            EmployeeInfo creatorEmployee;
            TradeParticipationInfo tradeParticipationInfo;
            ProcessInfo processInfo;
            ProcessStepInfo processStepInfo;
            ReversalInfo reversalInfo;

            IDataReader dr = null;

            StringBuilder ids = new StringBuilder();
            StringBuilder userIds = new StringBuilder();
            StringBuilder userTypes = new StringBuilder();

            DateTime? secondBusinessDay;
            DateTime dueDate;

            int projectId;
            int processId;
            int userId;
            int processStepId;
            int currentProjectId;
            int currentProcessId;
            int currentProcessStepId;
            int? reversalId;

            int? ProcessHide;//#---13-11-2018

            Boolean isPM;
            Boolean isCA;

            secondBusinessDay = projectsController.AddbusinessDaysToDate(DateTime.Now, 2);
            projectsController.InitializeHolidays();

            ids.Append("<ProjectsIds>");
            userIds.Append("<ProjectsUserIds>");
            userTypes.Append("<ProjectsUserTypes>");

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                projectId = projectInfo.Id.Value;
                pendingProcessSteps.Add(projectId, new List<ProcessStepInfo>());
                tradeParticipations.Add(projectId, new List<TradeParticipationInfo>());
                projectsInfoDictionary.Add(projectId, projectInfo);

                ids.Append("<ProjectId><Id>").Append(projectInfo.IdStr).Append("</Id></ProjectId>");

                projectUsers = selectedUsers[projectId];

                // find all the roles that the selected users play in the project
                if (projectUsers != null)
                {
                    // this hashset contains all the roles for all the users specified for this project
                    rolesPlayHashSet = new HashSet<String>();
                    rolesPlayOneProject = new Dictionary<int, List<String>>();
                    rolesPlayAllProjects.Add(projectId, rolesPlayOneProject);

                    foreach (EmployeeInfo employeeInfo in projectUsers)
                    {
                        // If an employeeInfo doest not have Id we are getting all the pending steps for the role
                        // If this is the case we will not have more than one employee for the project so Id = 0 is not used more than once
                        if (employeeInfo.Id != null)
                        {
                            rolesPlayList = GetAllRolesPlay(employeeInfo, projectInfo);
                            userIds.Append("<ProjectUserId><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><PeopleId>").Append(employeeInfo.IdStr).Append("</PeopleId></ProjectUserId>");
                            userId = employeeInfo.Id.Value;
                        }
                        else
                        {
                            rolesPlayList = new List<string>();
                            rolesPlayList.Add(employeeInfo.Role);
                            userId = 0;
                        }

                        rolesPlayOneProject.Add(userId, rolesPlayList);

                        foreach (String role in rolesPlayList)
                            if (!rolesPlayHashSet.Contains(role))
                                rolesPlayHashSet.Add(role);
                    }

                    foreach (String role in rolesPlayHashSet)
                        userTypes.Append("<ProjectUserType><ProjectId>").Append(projectInfo.IdStr).Append("</ProjectId><UserType>").Append(role).Append("</UserType></ProjectUserType>");
                }
            }




            ids.Append("</ProjectsIds>");
            userIds.Append("</ProjectsUserIds>");
            userTypes.Append("</ProjectsUserTypes>");

            parameters.Add(ids.ToString());
            parameters.Add(userTypes.ToString());
            parameters.Add(userIds.ToString());
            parameters.Add(startDate);
            parameters.Add(endDate);

            try
            {

                dr = Data.DataProvider.GetInstance().GetPendingSteps(parameters.ToArray());

                //GetPendingQuotes
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    tradeParticipationInfo = new TradeParticipationInfo(Data.Utils.GetDBInt32(dr["TradeParticipationId"]));
                    tradeParticipationInfo.Trade = new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"]));
                    tradeParticipationInfo.Trade.Project = new ProjectInfo(projectId);
                    tradeParticipationInfo.SubContractor = new SubContractorInfo(Data.Utils.GetDBInt32(dr["SubContractorId"]));

                    tradeParticipationInfo.QuoteDueDate = Data.Utils.GetDBDateTime(dr["QuoteDueDate"]);
                    tradeParticipationInfo.Trade.Name = Data.Utils.GetDBString(dr["TradeName"]);
                    tradeParticipationInfo.SubContractor.Name = Data.Utils.GetDBString(dr["SubContractorName"]);
                    tradeParticipationInfo.SubContractor.ShortName = Data.Utils.GetDBString(dr["SubContractorShortName"]);

                    if (dr["PMPeopleId"] != DBNull.Value) tradeParticipationInfo.Trade.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) tradeParticipationInfo.Trade.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    if (IsDateInRange(tradeParticipationInfo.QuoteDueDate, null, secondBusinessDay))
                        tradeParticipations[projectId].Add(tradeParticipationInfo);
                }


                //Processes with steps. Contains all the processed that has at least one pending step in the specified date range
                dr.NextResult();
                currentProcessId = 0;
                currentProcessStepId = 0;
                processInfo = null;
                processStepInfo = null;
                while (dr.Read())
                {
                    processId = Data.Utils.GetDBInt32(dr["ProcessId"]).Value;
                    processStepId = Data.Utils.GetDBInt32(dr["ProcessStepId"]).Value;
                    reversalId = Data.Utils.GetDBInt32(dr["ReversalId"]);
                    ProcessHide = dr["Hide"] != null ? dr["Hide"].ToString() != "" ? Data.Utils.GetDBInt32(dr["Hide"]) : 0 : 0;       // //#---13/11/2018

                    if (processId != currentProcessId)
                    {
                        currentProcessId = processId;

                        processInfo = new ProcessInfo(processId);
                        processInfo.Name = Data.Utils.GetDBString(dr["ProcessName"]);
                        processInfo.Steps = new List<ProcessStepInfo>();
                        //#---13/11/2018
                        processInfo.Hide = ProcessHide;
                        //#---13/11/2018

                        processInfoDictionary.Add(processId, processInfo);
                    }

                    if (processStepId != currentProcessStepId)
                    {
                        currentProcessStepId = processStepId;

                        processStepInfo = new ProcessStepInfo(processStepId);
                        processStepInfo.Reversals = new List<ReversalInfo>();
                        processStepInfo.Name = Data.Utils.GetDBString(dr["ProcessStepName"]);
                        processStepInfo.TargetDate = Data.Utils.GetDBDateTime(dr["TargetDate"]);
                        processStepInfo.ActualDate = Data.Utils.GetDBDateTime(dr["ActualDate"]);
                        processStepInfo.Skip = Data.Utils.GetDBBoolean(dr["Skip"]);
                        processStepInfo.Role = Data.Utils.GetDBString(dr["UserType"]);
                        processStepInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);

                        processStepInfo.Process = processInfo;

                        processInfo.Steps.Add(processStepInfo);
                    }

                    if (reversalId != null)
                    {
                        reversalInfo = new ReversalInfo(reversalId);

                        reversalInfo.ReversalDate = Data.Utils.GetDBDateTime(dr["ReverseDate"]);
                        reversalInfo.ReversalNote = Data.Utils.GetDBString(dr["ReverseNote"]);

                        reversalInfo.ReversalBy = new EmployeeInfo();
                        reversalInfo.ReversalBy.FirstName = Data.Utils.GetDBString(dr["ReversedByFirstName"]);
                        reversalInfo.ReversalBy.LastName = Data.Utils.GetDBString(dr["ReversedByLastName"]);

                        processStepInfo.Reversals.Add(reversalInfo);
                    }
                }




                // Trades
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value], processInfo.Project.Trades[0]);

                    processInfoList.Add(processInfo);
                }




                // Contracts and Subcontracts (Variations)
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));
                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);
                    processInfo.Project.Trades[0].Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                    if (Data.Utils.GetDBInt32(dr["ParentContractId"]) != null)
                    {
                        processInfo.Project.Trades[0].Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));
                        processInfo.Project.Trades[0].Contract.Description = Data.Utils.GetDBString(dr["Description"]);
                    }

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value], processInfo.Project.Trades[0]);

                    processInfoList.Add(processInfo);
                }



                // Client Variations
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new ClientVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }
                // Tenant Variations  DS20240213 >>>
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new TenantVariationInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }
                // Tenant Variations  DS20240213 <<<

                // Separate accounts
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                    processInfo.Project.ClientVariations.Add(new SeparateAccountInfo(Data.Utils.GetDBInt32(dr["ClientVariationId"])));
                    processInfo.Project.ClientVariations[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.ClientVariations[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }

                // Claims
                dr.NextResult();
                while (dr.Read())
                {
                    processInfo = processInfoDictionary[Data.Utils.GetDBInt32(dr["ProcessId"]).Value];
                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Claims = new List<ClaimInfo>();
                    processInfo.Project.Claims.Add(new ClaimInfo(Data.Utils.GetDBInt32(dr["ClaimId"])));
                    processInfo.Project.Claims[0].Number = Data.Utils.GetDBInt32(dr["Number"]);

                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    foreach (ProcessStepInfo processStep in processInfo.Steps)
                        processStep.AssignedTo = GetRoleAssignee(processStep.Role, projectsInfoDictionary[processInfo.Project.Id.Value]);

                    processInfoList.Add(processInfo);
                }



                foreach (ProcessInfo process in processInfoList)
                {
                    projectId = process.Project.Id.Value;


                    if (currentStepOnly)
                    {
                        processStepInfoList1 = new List<ProcessStepInfo>();

                        processStepInfo = GetCurrentStep(process);

                        if (processStepInfo != null)
                            processStepInfoList1.Add(processStepInfo);
                    }
                    else
                    {
                        processStepInfoList1 = GetPendingSteps(process);
                    }

                    foreach (ProcessStepInfo processStep in processStepInfoList1)
                    {
                        foreach (EmployeeInfo employee in selectedUsers[projectId])
                        {
                            if (employee.Id != null && processStep.AssignedTo != null && employee.Equals(processStep.AssignedTo))
                            {
                                pendingProcessSteps[projectId].Add(processStep);
                                break;
                            }

                            if (employee.Id == null && processStep.AssignedTo == null && employee.Role == processStep.Role)
                            {
                                pendingProcessSteps[projectId].Add(processStep);
                                break;
                            }
                        }
                    }
                }




                // Missing Signed Contract file

                //SAN------Signed contracts file link missing

                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;

                while (dr.Read())
                {

                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();


                    processInfo.Project = new ProjectInfo(Data.Utils.GetDBInt32(dr["ProjectId"]));
                    processInfo.Project.Trades = new List<TradeInfo>();
                    processInfo.Project.Trades.Add(new TradeInfo(Data.Utils.GetDBInt32(dr["TradeId"])));

                    processInfo.Project.Trades[0].Name = Data.Utils.GetDBString(dr["Name"]);

                    processInfo.Project.Trades[0].Type = Data.Utils.GetDBString(dr["TradeType"]);

                    processInfo.Name = "Missing Signed Contract File";


                    //processInfo.Project.Trades[0].Contract = new ContractInfo(Data.Utils.GetDBInt32(dr["ContractId"]));

                    //if (Data.Utils.GetDBInt32(dr["ParentContractId"]) != null)
                    //{
                    //    processInfo.Project.Trades[0].Contract.ParentContract = new ContractInfo(Data.Utils.GetDBInt32(dr["ParentContractId"]));
                    //   // processInfo.Project.Trades[0].Contract.Description = Data.Utils.GetDBString(dr["Description"]);
                    //}


                    if (dr["PMPeopleId"] != DBNull.Value) processInfo.Project.ProjectManager = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PMPeopleId"]));
                    if (dr["CAPeopleId"] != DBNull.Value) processInfo.Project.ContractsAdministrator = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["CAPeopleId"] != DBNull.Value && dr["TradeType"] != DBNull.Value)
                    {
                        string Rolee = "";
                        if (dr["TradeType"].ToString() == "Design")
                        {
                            Rolee = "DC";
                        }
                        else { Rolee = "CA"; }

                        processInfo.Steps[0].AssignedTo = GetRoleAssignee(Rolee, projectsInfoDictionary[processInfo.Project.Id.Value]); //new EmployeeInfo(Data.Utils.GetDBInt32(dr["CAPeopleId"]));

                        processInfo.Steps[0].Name = Rolee + " - Upload Signed Contract file";

                        ////
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);


                }



                //SAN----Signed contracts file link missing









                // Transmittals
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.Transmittals = new List<TransmittalInfo>();
                    processInfo.Project.Transmittals.Add(new TransmittalInfo());
                    processInfo.Project.Transmittals[0].Contact = new ContactInfo();
                    processInfo.Project.Transmittals[0].Contact.SubContractor = new SubContractorInfo();

                    processInfo.Project.Transmittals[0].Id = Data.Utils.GetDBInt32(dr["TransmittalId"]);
                    processInfo.Project.Transmittals[0].TransmittalType = Data.Utils.GetDBString(dr["TransmittalType"]);
                    processInfo.Project.Transmittals[0].TransmittalTypeOther = Data.Utils.GetDBString(dr["TransmittalTypeOther"]);
                    processInfo.Project.Transmittals[0].TransmittalNumber = Data.Utils.GetDBInt32(dr["TransmittalNumber"]);
                    processInfo.Project.Transmittals[0].TransmissionDate = Data.Utils.GetDBDateTime(dr["TransmissionDate"]);
                    processInfo.Project.Transmittals[0].Contact.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                    processInfo.Project.Transmittals[0].Contact.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    processInfo.Project.Transmittals[0].Contact.SubContractor.Name = Data.Utils.GetDBString(dr["Name"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["CreatorPeopleId"] != DBNull.Value)
                    {
                        creatorEmployee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["CreatorPeopleId"]));
                        creatorEmployee.FirstName = Data.Utils.GetDBString(dr["CreatorFirstName"]);
                        creatorEmployee.LastName = Data.Utils.GetDBString(dr["CreatorLastName"]);
                        creatorEmployee.LastLoginDate = Data.Utils.GetDBDateTime(dr["CreatorLastLogin"]);

                        processInfo.Steps[0].AssignedTo = creatorEmployee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }



                //RFIs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.RFIs = new List<RFIInfo>();
                    processInfo.Project.RFIs.Add(new RFIInfo());

                    processInfo.Project.RFIs[0].Id = Data.Utils.GetDBInt32(dr["RFIId"]);
                    processInfo.Project.RFIs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.RFIs[0].Subject = Data.Utils.GetDBString(dr["Subject"]);
                    processInfo.Project.RFIs[0].Status = Data.Utils.GetDBString(dr["Status"]);
                    processInfo.Project.RFIs[0].RaiseDate = Data.Utils.GetDBDateTime(dr["RaiseDate"]);
                    processInfo.Project.RFIs[0].TargetAnswerDate = Data.Utils.GetDBDateTime(dr["TargetAnswerDate"]);

                    processInfo.Steps[0].Process = processInfo;

                    if (dr["PeopleId"] != DBNull.Value)
                    {
                        creatorEmployee = new EmployeeInfo(Data.Utils.GetDBInt32(dr["PeopleId"]));
                        creatorEmployee.FirstName = Data.Utils.GetDBString(dr["FirstName"]);
                        creatorEmployee.LastName = Data.Utils.GetDBString(dr["LastName"]);
                        creatorEmployee.LastLoginDate = Data.Utils.GetDBDateTime(dr["UserLastLogin"]);

                        processInfo.Steps[0].AssignedTo = creatorEmployee;
                    }

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //EOTs
                dr.NextResult();
                currentProjectId = 0;
                processStepInfoList = null;
                while (dr.Read())
                {
                    projectId = Data.Utils.GetDBInt32(dr["ProjectId"]).Value;

                    if (projectId != currentProjectId)
                    {
                        currentProjectId = projectId;
                        processStepInfoList = pendingProcessSteps[projectId];
                    }

                    processInfo = new ProcessInfo();
                    processInfo.Steps = new List<ProcessStepInfo>();
                    processInfo.Steps.Add(new ProcessStepInfo());
                    processInfo.Project = new ProjectInfo();
                    processInfo.Project.EOTs = new List<EOTInfo>();
                    processInfo.Project.EOTs.Add(new EOTInfo());

                    processInfo.Project.EOTs[0].Id = Data.Utils.GetDBInt32(dr["EOTId"]);
                    processInfo.Project.EOTs[0].Number = Data.Utils.GetDBInt32(dr["Number"]);
                    processInfo.Project.EOTs[0].Cause = Data.Utils.GetDBString(dr["Cause"]);
                    processInfo.Project.EOTs[0].StartDate = Data.Utils.GetDBDateTime(dr["StartDate"]);
                    processInfo.Project.EOTs[0].EndDate = Data.Utils.GetDBDateTime(dr["EndDate"]);
                    processInfo.Project.EOTs[0].FirstNoticeDate = Data.Utils.GetDBDateTime(dr["FirstNoticeDate"]);
                    processInfo.Project.EOTs[0].WriteDate = Data.Utils.GetDBDateTime(dr["WriteDate"]);

                    processInfo.Steps[0].Process = processInfo;
                    processInfo.Steps[0].AssignedTo = GetRoleAssignee(EmployeeInfo.TypeContractsAdministrator, projectsInfoDictionary[projectId]);

                    processStepInfoList.Add(processInfo.Steps[0]);
                }

                //Number of Claims
                dr.NextResult();
                while (dr.Read())
                {
                    numActiveClaims.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClaims"]).Value);
                }

                //Number of Client trades an Client variations
                dr.NextResult();
                while (dr.Read())
                {
                    numClientTrades.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClientTrades"]).Value);
                    numClientVariations.Add(Data.Utils.GetDBInt32(dr["ProjectId"]).Value, Data.Utils.GetDBInt32(dr["NumClientVariations"]).Value);
                }

                // Add special pending tasks
                foreach (ProjectInfo projectInfo in projectInfoList)
                {
                    projectId = projectInfo.Id.Value;
                    projectUsers = selectedUsers[projectId];
                    processStepInfoList = pendingProcessSteps[projectId];

                    foreach (EmployeeInfo employeeInfo in projectUsers)
                    {
                        rolesPlayOneProject = rolesPlayAllProjects[projectId];
                        userId = employeeInfo.Id == null ? 0 : employeeInfo.Id.Value;

                        isPM = rolesPlayOneProject[userId].Contains(EmployeeInfo.TypeProjectManager);
                        isCA = rolesPlayOneProject[userId].Contains(EmployeeInfo.TypeContractsAdministrator);

                        if (isPM)
                        {
                            // A Claim needs to be added if:
                            // 1. No active claim
                            // 2. The project contract amount has been specified
                            // 3. There are client trades or client variations (something to claim) in the project
                            // 4. The client trades match the contract amount
                            // 5. the last claim is not claiming 100%
                            // 6. If date range specified duedate is project commencement date
                            if (projectInfo.ContractAmount != null && (Decimal)projectInfo.ContractAmount > 0 &&
                                numActiveClaims.ContainsKey(projectId) && numActiveClaims[projectId] == 0 &&
                                (numClientTrades[projectId] > 0 || numClientVariations[projectId] > 0))
                            {
                                projectInfo.Claims = projectsController.GetClaimsWithTradesAndVariations(projectInfo);
                                projectInfo.ClientTrades = projectsController.GetClientTrades(projectInfo);
                                projectInfo.ClientVariations = projectsController.GetClientVariationsLastRevisions(projectInfo);

                                Decimal? totalClientTrades = projectInfo.TotalClientTrades;
                                Decimal? totalLastClaim = projectInfo.TotalLastClaim;

                                if (totalClientTrades != null && (Decimal)totalClientTrades == (Decimal)projectInfo.ContractAmount &&
                                   totalLastClaim != null && (Decimal)totalLastClaim < (Decimal)projectInfo.ContractAmountPlusVariations &&
                                   IsDateInRange(projectInfo.CommencementDate, startDate, endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Project.Claims = new List<ClaimInfo>();
                                    processInfo.Project.Claims.Add(new ClaimInfo());

                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }

                            if (projectInfo.CompletionDate != null)
                            {
                                //Maintenance manuals task due some days before completion date
                                //start showing some days before completion date
                                dueDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysToShowManualLink);
                                if (projectInfo.MaintenanceManualFile == null && IsDateInRange(dueDate, startDate, endDate == null ? DateTime.Now : endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].Name = "Maintenance manuals";
                                    processInfo.Steps[0].TargetDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysDueManualLink);
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }

                                // Post project review due some days after completion date
                                // start showing on completion date
                                dueDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysToShowProjectReview);
                                if (projectInfo.PostProjectReviewFile == null && IsDateInRange(dueDate, startDate, endDate == null ? DateTime.Now : endDate))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Project = new ProjectInfo(projectInfo.Id);
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].Name = "Post project review";
                                    processInfo.Steps[0].TargetDate = projectInfo.CompletionDate.Value.AddDays(ProjectInfo.DaysDueProjectReview);
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }
                        }

                        // Quotes reminers
                        if (isCA)
                        {
                            foreach (TradeParticipationInfo tradeParticipation in tradeParticipations[projectId])
                            {
                                // If it's a CA check to see if the trade has a CA if so it must be this employee
                                if ((tradeParticipation.Trade.Project.ContractsAdministrator == null) || employeeInfo.Id == null || (tradeParticipation.Trade.Project.ContractsAdministrator.Equals(employeeInfo)))
                                {
                                    processInfo = new ProcessInfo();
                                    processInfo.Steps = new List<ProcessStepInfo>();
                                    processInfo.Steps.Add(new ProcessStepInfo());
                                    processInfo.Project = new ProjectInfo();
                                    processInfo.Project.Trades = new List<TradeInfo>();
                                    processInfo.Project.Trades.Add(new TradeInfo());
                                    processInfo.Project.Trades[0].Participations = new List<TradeParticipationInfo>();
                                    processInfo.Project.Trades[0].Participations.Add(new TradeParticipationInfo());
                                    processInfo.Project.Trades[0].Participations[0].SubContractor = new SubContractorInfo();

                                    processInfo.Project.Trades[0].Name = tradeParticipation.Trade.Name;
                                    processInfo.Project.Trades[0].Participations[0].Id = tradeParticipation.Id;
                                    processInfo.Project.Trades[0].Participations[0].QuoteDate = tradeParticipation.QuoteDueDate;
                                    processInfo.Project.Trades[0].Participations[0].SubContractor.ShortName = tradeParticipation.SubContractor.ShortName;

                                    processInfo.Steps[0].Process = processInfo;
                                    processInfo.Steps[0].AssignedTo = employeeInfo;

                                    processStepInfoList.Add(processInfo.Steps[0]);
                                }
                            }
                        }
                    }

                    foreach (ProcessStepInfo processStep in processStepInfoList)
                    {
                        reversalInfo = processStep.PendingReversal;

                        if (reversalInfo != null)
                            processStep.Comments = "Reversed on: " + UI.Utils.SetFormDateTime(reversalInfo.ReversalDate) + " By: " + reversalInfo.ReversalByName + ". " + reversalInfo.ReversalNote;
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Pending Process Steps from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return pendingProcessSteps;
        }












        /// <summary>
        /// Throws and exception if the current user can not update information based on a process 
        /// </summary>
        public void CheckEditCurrentUser(ClientTradeInfo clientTradeInfo)
        {
            if (!AllowEditCurrentUser(clientTradeInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Throws and exception if the current user can not update a projects drowings
        /// </summary>
        public void CheckUpdateDrawingsCurrentUser(ProjectInfo projectInfo)
        {
            if (!AllowUpdateDrawingsCurrentUser(projectInfo))
                throw new SecurityException();
        }

        /// <summary>
        /// Determines if a step can be executed according to its type.
        /// </summary>
        public String CheckProcessStep(ProcessStepInfo processStepInfo)
        {
            XmlDocument xmlDocument;
            FileInfo fileInfo;

            switch (processStepInfo.Type)
            {
                case ProcessStepInfo.StepTypeComparisonCA:
                case ProcessStepInfo.StepTypeComparisonPM:
                case ProcessStepInfo.StepTypeComparisonCM:
                //#----COM approval process
                case ProcessStepInfo.StepTypeComparisonCO:
                //#----COM approval process
                case ProcessStepInfo.StepTypeComparisonDA:
                case ProcessStepInfo.StepTypeComparisonUM:
                case ProcessStepInfo.StepTypeCreateContract:
                    xmlDocument = TradesController.GetInstance().CheckTrade(processStepInfo.Process.Project.Trades[0], null, false);
                    if (xmlDocument.DocumentElement != null)
                        return "Comparison incomplete";
                    break;

                case ProcessStepInfo.StepTypeContractDC:
                case ProcessStepInfo.StepTypeContractDM:
                case ProcessStepInfo.StepTypeContractCA:
                case ProcessStepInfo.StepTypeContractPM:
                case ProcessStepInfo.StepTypeContractCM:
                //#----COM approval process
                case ProcessStepInfo.StepTypeContractCO:
                //#----COM approval process
                case ProcessStepInfo.StepTypeContractMD:
                    if (processStepInfo.Process.Project.Trades[0].Contract.IsSubContract)
                    {
                        xmlDocument = ContractsController.GetInstance().CheckTradeForTemplate(processStepInfo.Process.Project.Trades[0], processStepInfo.Process.Project.Trades[0].Contract.Template);
                        if (xmlDocument.DocumentElement != null)
                            return "Incomplete data";

                        if (processStepInfo.Process.Project.Trades[0].Contract.QuotesFile == null)
                            return "Quotes file not specified";

                        fileInfo = new FileInfo(UI.Utils.FullPath(processStepInfo.Process.Project.Trades[0].Contract.Trade.Project.AttachmentsFolder, processStepInfo.Process.Project.Trades[0].Contract.QuotesFile));
                        if (!fileInfo.Exists)
                            return "Quotes file does not exist";
                    }
                    break;

                case ProcessStepInfo.StepTypeClientVariationCA:
                case ProcessStepInfo.StepTypeClientVariationPM:
                case ProcessStepInfo.StepTypeClientVariationInternalApproval:
                case ProcessStepInfo.StepTypeClientVariationClientVerbalApproval:
                case ProcessStepInfo.StepTypeClientVariationClientFinalApproval:
                case ProcessStepInfo.StepTypeClientVariationWorksCompleted:
                case ProcessStepInfo.StepTypeClientVariationSendInvoice:
                case ProcessStepInfo.StepTypeClientVariationInvoicePaid:
                    xmlDocument = ProjectsController.GetInstance().CheckClientVariation(processStepInfo.Process.Project.ClientVariations[0]);
                    if (xmlDocument.DocumentElement != null)
                        return "Incomplete data";
                    break;

                case ProcessStepInfo.StepTypeClaimDraftCA:
                case ProcessStepInfo.StepTypeClaimDraftApproval:
                case ProcessStepInfo.StepTypeClaimInvoiceCA:
                case ProcessStepInfo.StepTypeClaimInvoiceInternalApproval:
                case ProcessStepInfo.StepTypeClaimInvoiceFinalApproval:
                    xmlDocument = ProjectsController.GetInstance().CheckClaim(processStepInfo.Process.Project.Claims[0]);
                    if (xmlDocument.DocumentElement != null)
                        return "Incomplete data";
                    break;
            }

            switch (processStepInfo.Type)
            {
                case ProcessStepInfo.StepTypeCreateContract:
                    if (processStepInfo.Process.Project.Trades[0].SelectedParticipation.Contact == null)
                        return "Winning quote contact not specified";

                    if (processStepInfo.Process.Project.Trades[0].PrelettingFile == null)
                        return "Order Letting file not specified";

                    fileInfo = new FileInfo(UI.Utils.FullPath(processStepInfo.Process.Project.Trades[0].Project.AttachmentsFolder, processStepInfo.Process.Project.Trades[0].PrelettingFile));
                    if (!fileInfo.Exists)
                        return "Order Letting file does not exist";

                    break;

                case ProcessStepInfo.StepTypeClientVariationClientVerbalApproval:
                case ProcessStepInfo.StepTypeClientVariationClientFinalApproval:
                    if (processStepInfo.Process.Project.ClientVariations[0].InternalApprovalDate == null)
                        return "Intenal approval not recorded.";
                    break;

                case ProcessStepInfo.StepTypeClaimInvoiceCA:
                case ProcessStepInfo.StepTypeClaimInvoiceInternalApproval:
                    if (!processStepInfo.Process.Project.Claims[0].IsDraftApproved)
                        return "Draft approval not recorded.";
                    break;

                case ProcessStepInfo.StepTypeClaimInvoiceFinalApproval:
                    if (!processStepInfo.Process.Project.Claims[0].IsInternallyApproved)
                        return "Invoice approval not recorded.";
                    break;
            }

            return null;
        }

        /// <summary>
        /// Sends an email to the next process step responsible
        /// </summary>
        public void SendNextStepEMail(ProcessStepInfo processStepInfo)
        {
            EmployeeInfo stepAssignee = GetStepAssignee(processStepInfo);
            String subject;
            String body;
            String contextInfo;

            if (stepAssignee != null)
            {
                if (processStepInfo.Process.Project.Trades != null)
                {
                    subject = "Approval pending (" + processStepInfo.Process.Project.Name + "/" + processStepInfo.Process.Project.Trades[0].Name + ")";
                    contextInfo = "Trade: <b>" + processStepInfo.Process.Project.Trades[0].Name + "</b><br />";
                }
                else
                    if (processStepInfo.Process.Project.Claims != null)
                {
                    subject = "Approval pending (" + processStepInfo.Process.Project.Name + "/Claim " + processStepInfo.Process.Project.Claims[0].Number.ToString() + ")";
                    contextInfo = "Progress Claim: <b>" + processStepInfo.Process.Project.Claims[0].Number.ToString() + "</b><br />";
                }
                else
                {
                    if (processStepInfo.Process.Project.ClientVariations != null)
                    {
                        String cVTypeName = processStepInfo.Process.Project.ClientVariations[0] is SeparateAccountInfo ? "Separate Account" : "Client Variation";

                        subject = "Approval pending (" + processStepInfo.Process.Project.Name + "/" + processStepInfo.Process.Project.ClientVariations[0].Type + " " + processStepInfo.Process.Project.ClientVariations[0].Number.ToString() + ")";
                        contextInfo = cVTypeName + ": <b>" + processStepInfo.Process.Project.ClientVariations[0].Name + "</b><br />";
                    }
                    else
                    {
                        subject = "Approval pending (" + processStepInfo.Process.Project.Name + ")";
                        contextInfo = "Unknown context<br />";
                    }
                }

                body = stepAssignee.FirstName + "</b>, you have a pending approval task to execute for: <br />" +
                       "<br />" +
                       "Project: <b>" + processStepInfo.Process.Project.Name + "</b><br />" +
                       contextInfo +
                       "Process: <b>" + processStepInfo.Process.Name + "</b><br />" +
                       "Step: <b>" + processStepInfo.Name + "</b><br />" +
                       "<br />" +
                       "<br />" +
                       "<i>SOS Server<br />Vaughan Constructions</i><br />";  //DS20231214

                Utils.SendEmail(stepAssignee, subject, body);
            }
        }

        /// <summary>
        /// Sends the contract to a subcontractor
        /// </summary>
        public static void SendContractToSubcontractor(TradeInfo tradeInfo)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            TradeParticipationInfo selectedParticipation = tradeInfo.SelectedParticipation;
            String message;
            String subject = tradeInfo.Project.Name + " - " + tradeInfo.Name + " - " + tradeInfo.Contract.TypeName + " " + tradeInfo.Contract.ContractNumber;


            if (tradeInfo.Contract.TypeName == "Contract")
            {
                message = "" +
                  "Dear " + selectedParticipation.Contact.FirstName + ",<br />" +
                  "<br />" +
                  "Find attached " + tradeInfo.Contract.TypeName + " for the above mentioned project." +
                  "<br />" +
                  "Please sign copy (i.e. [A] Sign (and have witnessed) the front page, AND [B] Initial all subsequent pages)  and return original hard copy to Vaughan Constructions Melbourne office (Att: Accounts).<br />" +
                  "If you have any queries in relation to the contents of the " + tradeInfo.Contract.TypeName + ", please contact the Project Manager.<br />" +
                  "<br />" +
                  "<br />" +
                  "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";
            }
            else
            {
                message = "" +
                    "Dear " + selectedParticipation.Contact.FirstName + ",<br />" +
                    "<br />" +
                    "Find attached " + tradeInfo.Contract.TypeName + " for the above mentioned project." +
                    "<br />" +

                    "If you have invoiced for these works before now, you will need to resubmit invoice to accountspayable@vaughans.com.au" +//San--14/06/2022
                    " and reference this variation number." + // DS20240403
                     "<br />" + "<br />" +
                     "Any queries in relation to the contents of the " + tradeInfo.Contract.TypeName + ", please contact the Project Manager.<br />" +
                    "<br />" +
                    "<br />" +
                    "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            }





            Byte[] attachment = UI.Utils.HtmlToPDF(contractsController.BuildContractWithModifications(tradeInfo.Contract), contractsController.GetTemplateFooterText(tradeInfo.Contract.Template));
            String attachmentName = tradeInfo.Contract.TypeName.Replace(" ", "") + "_(" + tradeInfo.Project.Number + ")-" + tradeInfo.WorkOrderNumber + ".pdf";

            Utils.SendEmail(selectedParticipation.Contact, subject, message, attachment, attachmentName);


            //#---- To Send email to CA and PM and Forman----


            if (tradeInfo.Project.ContractsAdministrator != null)
            {
                if (tradeInfo.Project.ContractsAdministrator.Email != null)
                    //throw new Exception("ContractsAdministrator's email not specified");
                    Utils.SendEmail(tradeInfo.Project.ContractsAdministrator, subject, message, attachment, attachmentName);
            }
            if (tradeInfo.Project.ProjectManager != null)
            {
                if (tradeInfo.Project.ProjectManager.Email != null)
                    // throw new Exception("ProjectManager's email not specified");

                    Utils.SendEmail(tradeInfo.Project.ProjectManager, subject, message, attachment, attachmentName);
            }




            //-----------------------To Foreman Send contracts without amount information---------------------------------

            if (tradeInfo.Project.Foreman != null && tradeInfo.Contract.TypeName == "Contract" && tradeInfo.JobTypeName != "Supply") //Send contracts without amount information
            {
                if (tradeInfo.Project.Foreman.Email != null)
                {  //throw new Exception("Foreman's email not specified");

                    String Foremancontract = contractsController.BuildContractWithModifications(tradeInfo.Contract);
                    Foremancontract = Foremancontract.Replace("<!-- Regarding -->", " ");
                    int len = Foremancontract.IndexOf("Contract Sum");
                    int len2 = Foremancontract.IndexOf("Regarding");
                    string X = "", Y = "";
                    if (len > -1)
                        X = Foremancontract.Substring(0, len);
                    if (len2 > -1)
                        Y = Foremancontract.Substring(len2);

                    string custom = @"</td><td></td></tr></tbody></table> </td><td></td></tr></tbody></table> </td></tr>   <tr><td><table><tbody><tr><td class='SupSection'>";


                    string NewForemancontract = X + custom + Y;


                    Byte[] attachment1 = UI.Utils.HtmlToPDF(NewForemancontract, contractsController.GetTemplateFooterText(tradeInfo.Contract.Template));
                    String attachmentName1 = tradeInfo.Contract.TypeName.Replace(" ", "") + "_(" + tradeInfo.Project.Number + ")-" + tradeInfo.WorkOrderNumber + ".pdf";

                    message = "" +
                    "Dear " + tradeInfo.Project.Foreman.Name + ",<br />" +
                    "<br />" +
                    "Find attached " + tradeInfo.Contract.TypeName + " for the above mentioned project. <br />" +
                    "If you have any queries in relation to the contents of the " + tradeInfo.Contract.TypeName + ", please contact the Project Manager.<br />" +
                    "<br />" +
                    "<br />" +
                    "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";


                    Utils.SendEmail(tradeInfo.Project.Foreman, subject, message, attachment1, attachmentName1);

                }

            }

            // #-----------------------To send Foreman--Variations Order Email  -------------------- 
            if (tradeInfo.Project.Foreman != null && tradeInfo.Contract.TypeName != "Contract")    // For Variation type
            {
                if (tradeInfo.Project.Foreman.Email != null)
                {
                    String Foremancontract = contractsController.BuildContractWithModifications(tradeInfo.Contract);
                    Foremancontract = Foremancontract.Replace("<!-- Regarding -->", " ");

                    string dX = "$" + (tradeInfo.Contract.Variations[0].Amount.Value).ToString("#,##0.00");

                    Foremancontract = Foremancontract.Replace(dX, " ");

                    int len = Foremancontract.IndexOf("THIS VARIATION ORDER");
                    int len2 = Foremancontract.IndexOf("CODE");
                    string X = "", Y = "";
                    if (len > -1)
                        X = Foremancontract.Substring(0, len);
                    if (len2 > -1)
                        Y = Foremancontract.Substring(len2);

                    string custom = @"<br /><br /><br /><br />";


                    string NewForemancontract = X + custom + Y;


                    Byte[] attachment1 = UI.Utils.HtmlToPDF(NewForemancontract, contractsController.GetTemplateFooterText(tradeInfo.Contract.Template));
                    String attachmentName1 = tradeInfo.Contract.TypeName.Replace(" ", "") + "_(" + tradeInfo.Project.Number + ")-" + tradeInfo.WorkOrderNumber + ".pdf";

                    message = "" +
                    "Dear " + tradeInfo.Project.Foreman.Name + ",<br />" +
                    "<br />" +
                    "Find attached " + tradeInfo.Contract.TypeName + " for the above mentioned project. <br />" +
                    "If you have any queries in relation to the contents of the " + tradeInfo.Contract.TypeName + ", please contact the Project Manager.<br />" +
                    "<br />" +
                    "<br />" +
                    "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";


                    Utils.SendEmail(tradeInfo.Project.Foreman, subject, message, attachment1, attachmentName1);


                }
                // throw new Exception("ProjectManager's email not specified");

                //Utils.SendEmail(tradeInfo.Project.ProjectManager, subject, message, attachment, attachmentName);
            }

            //#---- To Send email to CA and PM and Foreman----


        }

        /// <summary>
        /// Sends the contract summary to the site
        /// </summary>
        public static void SendContractSummaryToSite(TradeInfo tradeInfo)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            EmployeeInfo employeeInfo = new EmployeeInfo();

            employeeInfo.FirstName = "SOS";
            employeeInfo.LastName = "System";
            employeeInfo.Email = ConfigurationManager.AppSettings["EmailFax"].ToString();

            String subject = tradeInfo.Project.Fax;

            String message = "" +
               "Attention: " + tradeInfo.Project.Foreman.Name + ",<br />" +
               "<br />" +
               tradeInfo.Contract.TypeName + ": <b>" + tradeInfo.Contract.ContractNumber + "</b><br />" +
               "Trade: <b>" + tradeInfo.Code + " " + tradeInfo.Name + "</b><br />" +
               "Project: <b>" + tradeInfo.Project.Name + "</b><br />";

            Byte[] attachment = UI.Utils.HtmlToPDF(contractsController.BuildContractSummaryWithModifications(tradeInfo.Contract), contractsController.GetTemplateFooterText(tradeInfo.Contract.Template));
            String attachmentName = tradeInfo.Contract.TypeName.Replace(" ", "") + "_(" + tradeInfo.Project.Number + ")-" + tradeInfo.WorkOrderNumber + ".pdf";

            Utils.SendEmail(employeeInfo, subject, message, attachment, attachmentName);
        }

        /// <summary>
        /// Sends the contract apporval notification to accounts
        /// </summary>
        public static void SendContractNotificationToAccounts(TradeInfo tradeInfo)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            EmployeeInfo employeeInfo = new EmployeeInfo();

            employeeInfo.FirstName = "Accounts";
            employeeInfo.LastName = "";
            employeeInfo.Email = ConfigurationManager.AppSettings["EmailAccounts"].ToString();

            String subject = tradeInfo.Contract.TypeName + " " + tradeInfo.Contract.ContractNumber + " Approved";
            String strRetentionReq = ""; // DS20231108
            if (tradeInfo.Contract.CheckRetentionReq != null)    // DS20231108  >>>
                if (tradeInfo.Contract.CheckRetentionReq == true)
                {
                    { 
                strRetentionReq = "  <tr bgcolor='#DDDDDD'>" +
                         "    <td>Retention Required</td>" +
                         "    <td>Yes</td>" +
                         "  </tr>";
                    }
                }
            //We could use templates for emails as well. For that reason it has no problem using a trade variable as a string 
            //like below in contractsController.GetTradeVariable(tradeInfo, "TradesList")
            String body = "A " + tradeInfo.Contract.TypeName + " has been approved.<br />" +
                          "<br />" +
                          "<table>" +
                          "  <tr bgcolor='#EEEEEE'>" +
                          "    <td>Project Name:</td>" +
                          "    <td>" + tradeInfo.Project.Name + "</td>" +
                          "  </tr>" +
                          "  <tr bgcolor='#DDDDDD'>" +
                          "    <td>Project Number:</td>" +
                          "    <td>" + tradeInfo.Project.Number + "</td>" +
                          "  </tr>" +
                          "  <tr bgcolor='#EEEEEE'>" +
                          "    <td valign='top'>Trade:</td>" +
                          "    <td><b>" + tradeInfo.Code + " " + tradeInfo.Name + "</b><br/>" + tradeInfo.Contract.TypeName + " Amount: $ " + UI.Utils.SetFormEditDecimal(tradesController.GetQuoteTotal(tradeInfo.SelectedParticipation)) + "</td>" +
                          "  <tr bgcolor='#DDDDDD'>" +
                          "    <td valign='top'>Budget:</td>" +
                          "    <td>" + contractsController.GetTradeVariable(tradeInfo, "TradesList") + "</td>" +
                          "  </tr>" +
                          strRetentionReq +  // DS20231108
                          "</table>" +
                          "<br />" +
                          "<i>SOS Server<br />Vaughan Constructions</i><br />";  // DS20231214

            Byte[] attachment = UI.Utils.HtmlToPDF(contractsController.BuildContractWithModifications(tradeInfo.Contract), contractsController.GetTemplateFooterText(tradeInfo.Contract.Template));
            String attachmentName = tradeInfo.Contract.TypeName.Replace(" ", "") + "_(" + tradeInfo.Project.Number + ")-" + tradeInfo.WorkOrderNumber + ".pdf";

            Utils.SendEmail(employeeInfo, subject, body, attachment, attachmentName);
        }








        /// <summary>
        /// Sends the client variation to the specified person
        /// </summary>
        public static void SendClientVariation(ClientVariationInfo clientVariationInfo, PeopleInfo peopleInfo)
        {
            String cVTypeName = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Variation";

            if (clientVariationInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = clientVariationInfo.Project.Name + " - " + cVTypeName + " " + clientVariationInfo.NumberAndRevisionName;

            String message = "" +
               "Dear " + peopleInfo.FirstName + ",<br />" +
               "<br />" +
               "Please find attached " + cVTypeName + " No " + clientVariationInfo.NumberAndRevisionName + " (" + clientVariationInfo.Name + ") for your approval.<br />" +
               "<br />" +
               "<br />" +
               "<i>" + clientVariationInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateClientVariationReport(clientVariationInfo);

            String attachmentName = ("Project_" + clientVariationInfo.Project.Number + "_" + clientVariationInfo.Type + "_" + clientVariationInfo.NumberAndRevisionName) + ".pdf";

            if (clientVariationInfo.BackupFile != null)
            {
                FileInfo fileInfo = new FileInfo(UI.Utils.FullPath(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.BackupFile));

                if (fileInfo.Exists)
                {
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, new List<FileInfo> { fileInfo });
                }
                else
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            }
            else
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
        }

        //#--------------To send client Variation email copy to CAand PM--------------------------------------------

        /// <summary>
        /// Sends the client variation to the specified person
        /// </summary>
        public static void SendClientVariationTo_CAandPM(ClientVariationInfo clientVariationInfo, PeopleInfo peopleInfo)
        {
            //String ClientContactName = "";
            String cVTypeName = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Variation";

            if (clientVariationInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            //if (clientVariationInfo.Project.ClientContact != null)
            //    ClientContactName = clientVariationInfo.Project.ClientContact.FirstName;



            String subject = clientVariationInfo.Project.Name + " - " + cVTypeName + " " + clientVariationInfo.NumberAndRevisionName;

            String message = "" +
               //"Dear " + ClientContactName 
               "Hi" + ",<br />" +
               "<br />" +
               "Please find attached " + cVTypeName + " No " + clientVariationInfo.NumberAndRevisionName + " (" + clientVariationInfo.Name + ") for your approval.<br />" +
               "<br />" +
               "<br />" +
               "<i>" + clientVariationInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateClientVariationReport(clientVariationInfo);

            String attachmentName = ("Project_" + clientVariationInfo.Project.Number + "_" + clientVariationInfo.Type + "_" + clientVariationInfo.NumberAndRevisionName) + ".pdf";

            if (clientVariationInfo.BackupFile != null)
            {
                FileInfo fileInfo = new FileInfo(UI.Utils.FullPath(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.BackupFile));

                if (fileInfo.Exists)
                {
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, new List<FileInfo> { fileInfo });
                }
                else
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            }
            else
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
        }



        //#--------------To send client Variation email copy to CAand PM--------------------------------------------









        /// <summary>
        /// Sends the separate account invoice to the specified person
        /// </summary>
        public static void SendSeparateAccountInvoice(SeparateAccountInfo separateAccountInfo, PeopleInfo peopleInfo)
        {
            if (separateAccountInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = separateAccountInfo.Project.Name + " - Invoice " + UI.Utils.SetFormInteger(separateAccountInfo.InvoiceNumber);

            String message = "" +
               "Dear " + peopleInfo.FirstName + ",<br />" +
               "<br />" +
               "Please find attached Invoice No " + separateAccountInfo.InvoiceNumber + "<br />" +
               "<br />" +
               "<br />" +
               "<i>" + separateAccountInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateSeparateAccountInvoiceReport(separateAccountInfo);
            String attachmentName = ("Project_" + separateAccountInfo.Project.Number + "_Invoice_" + UI.Utils.SetFormInteger(separateAccountInfo.InvoiceNumber)) + ".pdf";
            Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
        }




        //#--------------To send  separate account invoice email copy to CAand PM--------------------------------------------

        /// <summary>
        /// Sends the separate account invoice to the specified person
        /// </summary>
        public static void SendSeparateAccountInvoiceTo_CAandPM(SeparateAccountInfo separateAccountInfo, PeopleInfo peopleInfo)
        {
            if (separateAccountInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = separateAccountInfo.Project.Name + " - Invoice " + UI.Utils.SetFormInteger(separateAccountInfo.InvoiceNumber);

            String message = "" +
              // "Dear " + separateAccountInfo.Project.ClientContact.FirstName + ",<br />" +
              "Hi" +
               "<br />" +
               "Please find attached Invoice No " + separateAccountInfo.InvoiceNumber + "<br />" +
               "<br />" +
               "<br />" +
               "<i>" + separateAccountInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateSeparateAccountInvoiceReport(separateAccountInfo);
            String attachmentName = ("Project_" + separateAccountInfo.Project.Number + "_Invoice_" + UI.Utils.SetFormInteger(separateAccountInfo.InvoiceNumber)) + ".pdf";
            Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
        }


        //#--------------To send  separate account invoice email copy to CAand PM--------------------------------------------



        /// <summary>
        /// Sends the client variation to the distribution list
        /// </summary>
        public static void SendClientVariationToDistributionList(ClientVariationInfo clientVariationInfo)
        {
            #region old
            /*
            //bool sendToClientContact;
            //bool sendToClientContact1;
            //bool sendToClientContact2;
            //bool sendToSuperintendent;
            //bool sendToQuantitySurveyor;

            //if (clientVariationInfo is SeparateAccountInfo)
            //{
            //    sendToClientContact = clientVariationInfo.Project.SendSAToClientContact;
            //    sendToClientContact1 = clientVariationInfo.Project.SendSAToClientContact1;
            //    sendToClientContact2 = clientVariationInfo.Project.SendSAToClientContact2;
            //    sendToSuperintendent = clientVariationInfo.Project.SendSAToSuperintendent;
            //    sendToQuantitySurveyor = clientVariationInfo.Project.SendSAToQuantitySurveyor;
            //}
            //else
            //{
            //    sendToClientContact = clientVariationInfo.Project.SendCVToClientContact;
            //    sendToClientContact1 = clientVariationInfo.Project.SendCVToClientContact1;
            //    sendToClientContact2 = clientVariationInfo.Project.SendCVToClientContact2;
            //    sendToSuperintendent = clientVariationInfo.Project.SendCVToSuperintendent;
            //    sendToQuantitySurveyor = clientVariationInfo.Project.SendCVToQuantitySurveyor;
            //}

            //if (sendToClientContact)
            //{
            //    if (clientVariationInfo.Project.ClientContact.Email == null)
            //        throw new Exception("Client contact's email not specified");

            //    SendClientVariation(clientVariationInfo, clientVariationInfo.Project.ClientContact);
            //}

            //if (sendToClientContact1)
            //{
            //    if (clientVariationInfo.Project.ClientContact1.Email == null)
            //        throw new Exception("Client contact 1's email not specified");

            //    SendClientVariation(clientVariationInfo, clientVariationInfo.Project.ClientContact1);
            //}

            //if (sendToClientContact2)
            //{
            //    if (clientVariationInfo.Project.ClientContact2.Email == null)
            //        throw new Exception("Client contact 2's email not specified");

            //    SendClientVariation(clientVariationInfo, clientVariationInfo.Project.ClientContact2);
            //}

            //if (sendToSuperintendent)
            //{
            //    if (clientVariationInfo.Project.Superintendent.Email == null)
            //        throw new Exception("Superintendent's email not specified");

            //    SendClientVariation(clientVariationInfo, clientVariationInfo.Project.Superintendent);
            //}

            //if (sendToQuantitySurveyor)
            //{
            //    if (clientVariationInfo.Project.Superintendent.Email == null)
            //        throw new Exception("Quantity surveyor's email not specified");

            //    SendClientVariation(clientVariationInfo, clientVariationInfo.Project.QuantitySurveyor);
            //}
            */
            #endregion
            //# --- New distribution list
            if (clientVariationInfo is SeparateAccountInfo)
            {
                foreach (ClientContactInfo clientContact in clientVariationInfo.Project.ClientContactList)
                {
                    if (clientContact.SendSAs.Value && clientContact.Email != null)
                    {
                        SendClientVariation(clientVariationInfo, clientContact);
                    }
                }
            }
            else
            {
                foreach (ClientContactInfo clientContact in clientVariationInfo.Project.ClientContactList)
                {
                    if (clientContact.SendCVs.Value && clientContact.Email != null)
                    {
                        SendClientVariation(clientVariationInfo, clientContact);
                    }
                }

            }

            //#------To send CM approval Notification to CA and PM 
            if (clientVariationInfo.Project.ContractsAdministrator != null)
            {
                if (clientVariationInfo.Project.ContractsAdministrator.Email == null)
                    throw new Exception("ContractsAdministrator's email not specified");

                SendClientVariationTo_CAandPM(clientVariationInfo, clientVariationInfo.Project.ContractsAdministrator);
            }


            if (clientVariationInfo.Project.ProjectManager != null)
            {
                if (clientVariationInfo.Project.ProjectManager.Email == null)
                    throw new Exception("ProjectManager's email not specified");

                SendClientVariationTo_CAandPM(clientVariationInfo, clientVariationInfo.Project.ProjectManager);
            }
            //#------

        }








        /// <summary>
        /// Sends the client variation to the distribution list. It has to be a SeparateAccoutInfo objet
        /// </summary>
        public static void SendSeparateAccountInvoiceToDistributionList(SeparateAccountInfo separateAccountInfo)
        {


            //#----
            #region Old
            /*
              
             * 
             if (separateAccountInfo.IsSecondPrincipal)
            {
                if (separateAccountInfo.Project.SecondPrincipal.Email == null)
                    throw new Exception("Second principal contact's email not specified");

                SendSeparateAccountInvoice(separateAccountInfo, separateAccountInfo.Project.SecondPrincipal);
            }


            if (separateAccountInfo.Project.SendSAToClientContact)
            {
                if (separateAccountInfo.Project.ClientContact.Email == null)
                    throw new Exception("Client contact's email not specified");

                SendSeparateAccountInvoice(separateAccountInfo, separateAccountInfo.Project.ClientContact);
            }

            if (separateAccountInfo.Project.SendSAToClientContact1)
            {
                if (separateAccountInfo.Project.ClientContact1.Email == null)
                    throw new Exception("Client contact 1's email not specified");

                SendSeparateAccountInvoice(separateAccountInfo, separateAccountInfo.Project.ClientContact1);
            }

            if (separateAccountInfo.Project.SendSAToClientContact2)
            {
                if (separateAccountInfo.Project.ClientContact2.Email == null)
                    throw new Exception("Client contact 2's email not specified");

                SendSeparateAccountInvoice(separateAccountInfo, separateAccountInfo.Project.ClientContact2);
            }

            if (separateAccountInfo.Project.SendSAToSuperintendent)
            {
                if (separateAccountInfo.Project.Superintendent.Email == null)
                    throw new Exception("Superintendent's email not specified");

                SendSeparateAccountInvoice(separateAccountInfo, separateAccountInfo.Project.Superintendent);
            }

            if (separateAccountInfo.Project.SendSAToQuantitySurveyor)
            {
                if (separateAccountInfo.Project.Superintendent.Email == null)
                    throw new Exception("Quantity surveyor's email not specified");

                SendSeparateAccountInvoice(separateAccountInfo, separateAccountInfo.Project.QuantitySurveyor);
            }
            */
            #endregion


            foreach (ClientContactInfo clientContact in separateAccountInfo.Project.ClientContactList)
            {
                if (clientContact.SendSAs.Value && clientContact.Email != null)
                {
                    SendSeparateAccountInvoice(separateAccountInfo, clientContact);
                }
            }
            //#----


            //#------To send CM approval Notification to CA and PM 
            if (separateAccountInfo.Project.ContractsAdministrator != null)
            {

                if (separateAccountInfo.Project.ContractsAdministrator.Email == null)
                    throw new Exception("ContractsAdministrator's email not specified");

                SendSeparateAccountInvoiceTo_CAandPM(separateAccountInfo, separateAccountInfo.Project.ContractsAdministrator);
            }

            if (separateAccountInfo.Project.ProjectManager != null)
            {
                if (separateAccountInfo.Project.ProjectManager.Email == null)
                    throw new Exception("ProjectManager's email not specified");

                SendSeparateAccountInvoiceTo_CAandPM(separateAccountInfo, separateAccountInfo.Project.ProjectManager);
            }
            //#------




        }

        /// <summary>
        /// Sends the client variation codes to accounts
        /// </summary>
        public static void SendClientVariationCodesToAccounts(ClientVariationInfo clientVariationInfo)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            EmployeeInfo employeeInfo = new EmployeeInfo();
            StringBuilder codesTable = new StringBuilder();
            String cVTypeName = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Client Variation";

            employeeInfo.FirstName = "Accounts";
            employeeInfo.LastName = "";
            employeeInfo.Email = ConfigurationManager.AppSettings["EmailAccounts"].ToString();

            codesTable.Append("<table cellpadding='4' cellspacing='0' border='1'>");
            codesTable.Append("  <tr>");
            codesTable.Append("    <td align='center'><b>Trade</b></td>");
            codesTable.Append("    <td align='center'><b>Amount($)</b></td>");
            codesTable.Append("  </tr>");

            if (clientVariationInfo.Trades != null)
                foreach (ClientVariationTradeInfo clientVariationTradeInfo in clientVariationInfo.Trades)
                {
                    codesTable.Append("<tr>");
                    codesTable.Append("  <td align='center'>").Append(clientVariationTradeInfo.TradeCode).Append("</td>");
                    codesTable.Append("  <td align='Right'>").Append(UI.Utils.SetFormDecimal(clientVariationTradeInfo.Amount)).Append("</td>");
                    codesTable.Append("</tr>");
                }

            codesTable.Append("</table>");

            String subject = clientVariationInfo.Project.Name + "/" + clientVariationInfo.Type + " " + clientVariationInfo.NumberAndRevisionName;

            String body = "A " + cVTypeName + " has beed approved.<br />" +
                          "<br />" +
                          "Project Name: <b>" + clientVariationInfo.Project.Name + "</b><br />" +
                          "Project Number: <b>" + clientVariationInfo.Project.FullNumber + "</b><br />" +
                          cVTypeName + " Name: <b>" + clientVariationInfo.Name + "</b><br />" +
                          cVTypeName + " Number: <b>" + clientVariationInfo.NumberAndRevisionName + "</b><br />" +
                          "<br />" +
                          "Codes:<br />" +
                          codesTable.ToString() +
                          "<br />" +
                          "<i>SOS Server<br />Vaughan Constructions</i><br />";    //DS20231214

            Byte[] attachment = projectsController.GenerateClientVariationReport(clientVariationInfo);

            String attachmentName = ("Project_" + clientVariationInfo.Project.Number + "_" + clientVariationInfo.Type + "_" + clientVariationInfo.NumberAndRevisionName) + ".pdf";

            Utils.SendEmail(employeeInfo, subject, body, attachment, attachmentName);
        }

        /// <summary>
        /// Sends claim to the specified person
        /// </summary>
        public static void SendClaim(ClaimInfo claimInfo, ClaimInfo previousClaim, PeopleInfo peopleInfo, Boolean isFinal)
        {
            String fullNumber = isFinal ? "Tax Invoice " + claimInfo.Project.FullNumber + " PC " + UI.Utils.SetFormInteger(claimInfo.Number) + " in respect of claim No " + UI.Utils.SetFormInteger(claimInfo.Number) : "Progress Claim " + claimInfo.Project.FullNumber + " PC " + UI.Utils.SetFormInteger(claimInfo.Number) + " for review and approval";

            if (claimInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = claimInfo.Project.Name + " - " + fullNumber;

            String message = "" +
               "Dear " + peopleInfo.FirstName + ",<br />" +
               "<br />" +
               "Please find attached " + fullNumber + "." +
               "<br />" +
               "<br />" +
               "<i>" + claimInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateClaimReport(claimInfo, previousClaim);

            String attachmentName = ("Project_" + claimInfo.Project.Number + "_ProgressClaim_" + claimInfo.Number.ToString()) + ".pdf";
            if(claimInfo.BackupFile1 == null && claimInfo.BackupFile2 == null)   // DS20240117 >>>
            {
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            }
            else
            {
                List<FileInfo> fileInfoList = new List<FileInfo>();
                if (claimInfo.BackupFile1 != null) { fileInfoList.Add(new FileInfo(UI.Utils.FullPath(claimInfo.Project.AttachmentsFolder, claimInfo.BackupFile1))); }
                if (claimInfo.BackupFile2 != null) { fileInfoList.Add(new FileInfo(UI.Utils.FullPath(claimInfo.Project.AttachmentsFolder, claimInfo.BackupFile2))); }
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, fileInfoList);
            }   // DS20240117 <<<
        }


        //#--------------To send  Claim email copy to CAand PM--------------------------------------------
        public static void SendClaimTo_CAandPM(ClaimInfo claimInfo, ClaimInfo previousClaim, PeopleInfo peopleInfo, Boolean isFinal)
        {
            String fullNumber = isFinal ? "Tax Invoice " + claimInfo.Project.FullNumber + " PC " + UI.Utils.SetFormInteger(claimInfo.Number) + " in respect of claim No " + UI.Utils.SetFormInteger(claimInfo.Number) : "Draft Claim " + claimInfo.Project.FullNumber + " PC " + UI.Utils.SetFormInteger(claimInfo.Number) + " for review and approval";

            if (claimInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = claimInfo.Project.Name + " - " + fullNumber;

            String message = "" +
                // "Dear " + claimInfo.Project.ClientContact.FirstName
                "Hi"
               + ",<br />" +
               "<br />" +
               "Please find attached " + fullNumber + "." +
               "<br />" +
               "<br />" +
               "<i>" + claimInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateClaimReport(claimInfo, previousClaim);

            String attachmentName = ("Project_" + claimInfo.Project.Number + "_ProgressClaim_" + claimInfo.Number.ToString()) + ".pdf";

            // Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            if (claimInfo.BackupFile1 == null && claimInfo.BackupFile2 == null)   // DS20240117 >>>
            {
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            }
            else
            {
                List<FileInfo> fileInfoList = new List<FileInfo>();
                if (claimInfo.BackupFile1 != null) { fileInfoList.Add(new FileInfo(UI.Utils.FullPath(claimInfo.Project.AttachmentsFolder, claimInfo.BackupFile1))); }
                if (claimInfo.BackupFile2 != null) { fileInfoList.Add(new FileInfo(UI.Utils.FullPath(claimInfo.Project.AttachmentsFolder, claimInfo.BackupFile2))); }
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, fileInfoList);
            }   // DS20240117 <<<

        }

        //#--------




        /// <summary>
        /// Sends claim to the distribution list
        /// </summary>
        public static void SendClaimToDistributionList(ClaimInfo claimInfo, ClaimInfo previousClaim, Boolean isFinal)
        {
            #region OLd
            /*
            if (claimInfo.Project.SendPCToClientContact)
            {
                if (claimInfo.Project.ClientContact.Email == null)
                    throw new Exception("Client contact's email not specified");

                SendClaim(claimInfo, previousClaim, claimInfo.Project.ClientContact, isFinal);
            }

            if (claimInfo.Project.SendPCToClientContact1)
            {
                if (claimInfo.Project.ClientContact1.Email == null)
                    throw new Exception("Client contact 1's email not specified");

                SendClaim(claimInfo, previousClaim, claimInfo.Project.ClientContact1, isFinal);
            }

            if (claimInfo.Project.SendPCToClientContact2)
            {
                if (claimInfo.Project.ClientContact2.Email == null)
                    throw new Exception("Client contact 2's email not specified");

                SendClaim(claimInfo, previousClaim, claimInfo.Project.ClientContact2, isFinal);
            }

            if (claimInfo.Project.SendPCToSuperintendent)
            {
                if (claimInfo.Project.Superintendent.Email == null)
                    throw new Exception("Superintendent's email not specified");

                SendClaim(claimInfo, previousClaim, claimInfo.Project.Superintendent, isFinal);
            }

            if (claimInfo.Project.SendPCToQuantitySurveyor)
            {
                if (claimInfo.Project.Superintendent.Email == null)
                    throw new Exception("Quantity surveyor's email not specified");

                SendClaim(claimInfo, previousClaim, claimInfo.Project.QuantitySurveyor, isFinal);
            }
            */
            #endregion

            foreach (ClientContactInfo clientContact in claimInfo.Project.ClientContactList)
            {
                if (clientContact.SendClaims.Value && clientContact.Email != null)
                {
                    SendClaim(claimInfo, previousClaim, clientContact, isFinal);
                }
            }



            //#--

            //#------To send CM approval Notification to CA and PM 

            if (claimInfo.Project.ContractsAdministrator != null)
            {
                if (claimInfo.Project.ContractsAdministrator.Email == null)
                    throw new Exception("ContractsAdministrator's email not specified");

                SendClaimTo_CAandPM(claimInfo, previousClaim, claimInfo.Project.ContractsAdministrator, isFinal);
            }

            if (claimInfo.Project.ProjectManager != null)
            {
                if (claimInfo.Project.ProjectManager.Email == null)
                    throw new Exception("ProjectManager's email not specified");

                SendClaimTo_CAandPM(claimInfo, previousClaim, claimInfo.Project.ProjectManager, isFinal);
            }
            //#------




        }


        #region RFIs

        /// <summary>
        /// Sends RFI to the specified person
        /// </summary>
        public static void SendRFI(RFIInfo rFIInfo, PeopleInfo peopleInfo)
        {
            String fullNumber = rFIInfo.Project.FullNumber + " - " + UI.Utils.SetFormInteger(rFIInfo.Number);

            if (rFIInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = rFIInfo.Project.Name + " RFI No " + fullNumber;

            //#--- To send an email as a cuurent user name

            ////String message = "" +
            ////   "Dear " + peopleInfo.FirstName + ",<br />" +
            ////   "<br />" +
            ////   "Please find attached RFI No " + fullNumber + "." +
            ////   "<br />" +
            ////   "<br />" +
            ////   "<i>" + rFIInfo.Project.ProjectManager.Name + "</i><br />" +
            ////   "<i>Project Manager</i><br />" +
            ////   "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";


            PeopleInfo curentuser = Web.Utils.GetCurrentUser();

            String message = "" +
               "Dear " + peopleInfo.FirstName + ",<br />" +
               "<br />" +
               "Please find attached RFI No " + fullNumber + "." +
               "<br />" +
               "<br />" +
               "<i>" + curentuser.Name + "</i><br />" +
               "<i> Phone:" + curentuser.Phone + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";


            //#----



            Byte[] attachment = projectsController.GenerateRFIReport(rFIInfo);

            String attachmentName = ("Project_" + rFIInfo.Project.Number + "_RFI_" + rFIInfo.Number.ToString()) + ".pdf";

            if (rFIInfo.ReferenceFile != null)
            {
                FileInfo fileInfo = new FileInfo(UI.Utils.FullPath(rFIInfo.Project.AttachmentsFolder, rFIInfo.ReferenceFile));

                if (fileInfo.Exists)
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, new List<FileInfo> { fileInfo });
                else
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            }
            else
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);

            rFIInfo.Status = RFIInfo.StatusSent;

            ProjectsController.GetInstance().UpdateRFIStatus(rFIInfo);
        }






        //#----To send RFI emailCopy to CA ans PM-------
        /// <summary>
        /// Sends RFI to the specified person
        /// </summary>
        public static void SendRFI_CAandPM(RFIInfo rFIInfo, PeopleInfo peopleInfo)
        {
            String fullNumber = rFIInfo.Project.FullNumber + " - " + UI.Utils.SetFormInteger(rFIInfo.Number);

            if (rFIInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = rFIInfo.Project.Name + " RFI No " + fullNumber;

            String message = "" +
               // "Dear " + rFIInfo.Project.ClientContact.FirstName 
               "Hi" + ",<br />" +
               "<br />" +
               "Please find attached RFI No " + fullNumber + "." +
               "<br />" +
               "<br />" +
               "<i>" + peopleInfo.Name + "</i><br />" +
               "<i> Phone:" + peopleInfo.Phone + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateRFIReport(rFIInfo);

            String attachmentName = ("Project_" + rFIInfo.Project.Number + "_RFI_" + rFIInfo.Number.ToString()) + ".pdf";

            if (rFIInfo.ReferenceFile != null)
            {
                FileInfo fileInfo = new FileInfo(UI.Utils.FullPath(rFIInfo.Project.AttachmentsFolder, rFIInfo.ReferenceFile));

                if (fileInfo.Exists)
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, new List<FileInfo> { fileInfo });
                else
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);
            }
            else
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);

            rFIInfo.Status = RFIInfo.StatusSent;

            ProjectsController.GetInstance().UpdateRFIStatus(rFIInfo);
        }




        //#----To send RFI emailCopy to CA ans PM-------




        /// <summary>
        /// Sends RFI to the distribution list
        /// </summary>
        public static void SendRFIToDistributionList(RFIInfo rFIInfo)
        {
            #region Old
            /*
            if (rFIInfo.Project.SendRFIToClientContact)
            {
                if (rFIInfo.Project.ClientContact.Email == null)
                    throw new Exception("Client contact's email not specified");

                SendRFI(rFIInfo, rFIInfo.Project.ClientContact);
            }

            if (rFIInfo.Project.SendRFIToClientContact1)
            {
                if (rFIInfo.Project.ClientContact1.Email == null)
                    throw new Exception("Client contact 1's email not specified");

                SendRFI(rFIInfo, rFIInfo.Project.ClientContact1);
            }

            if (rFIInfo.Project.SendRFIToClientContact2)
            {
                if (rFIInfo.Project.ClientContact2.Email == null)
                    throw new Exception("Client contact 2's email not specified");

                SendRFI(rFIInfo, rFIInfo.Project.ClientContact2);
            }

            if (rFIInfo.Project.SendRFIToSuperintendent)
            {
                if (rFIInfo.Project.Superintendent.Email == null)
                    throw new Exception("Superintendent's email not specified");

                SendRFI(rFIInfo, rFIInfo.Project.Superintendent);
            }

            if (rFIInfo.Project.SendRFIToQuantitySurveyor)
            {
                if (rFIInfo.Project.Superintendent.Email == null)
                    throw new Exception("Quantity surveyor's email not specified");

                SendRFI(rFIInfo, rFIInfo.Project.QuantitySurveyor);
            }

            */

            #endregion

            foreach (ClientContactInfo clientContact in rFIInfo.Project.ClientContactList)
            {
                if (clientContact.SendRFIs.Value && clientContact.Email != null)
                {
                    SendRFI(rFIInfo, clientContact);
                }
            }

            //#---
            // # --- Alex asked to change it to send the copy of email to the person who sent this

            PeopleInfo pplInfo = Web.Utils.GetCurrentUser();
            if (pplInfo != null)
                if (pplInfo.Email != null)
                    SendRFI_CAandPM(rFIInfo, pplInfo);


            /* 
              

            //#------To send CM approval Notification to CA and PM 

            if(rFIInfo.Project.ContractsAdministrator!=null)
            { 
                if (rFIInfo.Project.ContractsAdministrator.Email == null)
                throw new Exception("ContractsAdministrator's email not specified");

                
            }

            if (rFIInfo.Project.ProjectManager != null)
            { 
                 if (rFIInfo.Project.ProjectManager.Email == null)
                throw new Exception("ProjectManager's email not specified");

                SendRFI_CAandPM(rFIInfo, rFIInfo.Project.ProjectManager);
            }
            //#------

           */


        }



        public static void SendRFIsResponse(RFIsResponseInfo rFIsResponseInfo, PeopleInfo peopleInfo, List<FileInfo> FileList)
        {
            String fullNumber = rFIsResponseInfo.RFI.Project.FullNumber + " - " + UI.Utils.SetFormInteger(rFIsResponseInfo.RFI.Number);

            if (rFIsResponseInfo.RFI.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = rFIsResponseInfo.RFI.Project.Name + " RFI No " + fullNumber + "-" + rFIsResponseInfo.ResponseNumber;


            PeopleInfo curentuser = Web.Utils.GetCurrentUser();

            String message = "" +
               "Dear " + peopleInfo.FirstName + ",<br />" +
               "<br />" +
               "<br />" +
               "Please find the below response for the RFI No " + fullNumber + "." +
               "<br />" +
               "<br />" +
                  rFIsResponseInfo.Responsemessage.ToString() +
                "<br /> <br />" + "Please login to our client portal to respond  http://clientsos.vaughans.com.au " +
                "<br />" +
                "<br />" +
               "<i>" + curentuser.Name + "</i><br />" +
               "<i> Phone:" + curentuser.Phone + "</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            if (rFIsResponseInfo.ResponseFolderPath != null)
            {
                if (rFIsResponseInfo.ResponseFolderPath.Length > 0)
                {
                    if (FileList.Count > 0)
                        Utils.SendEmail(peopleInfo, subject, message, null, null, FileList);
                    else
                        Utils.SendEmail(peopleInfo, subject, message);
                }
            }
            else
                Utils.SendEmail(peopleInfo, subject, message);

            //rFIInfo.Status = RFIInfo.StatusSent;

            //ProjectsController.GetInstance().UpdateRFIStatus(rFIInfo);
        }


        public static void SendRFIsResponseToDistributionList(RFIsResponseInfo rFIsResponseInfo, List<FileInfo> FileList)
        {


            foreach (ClientContactInfo clientContact in rFIsResponseInfo.RFI.Project.ClientContactList)
            {
                if (clientContact.SendRFIs.Value && clientContact.Email != null)
                {
                    SendRFIsResponse(rFIsResponseInfo, clientContact, FileList);
                }
            }

            if (rFIsResponseInfo.RFI.Project.ContractsAdministrator != null)
                SendRFIsResponse(rFIsResponseInfo, rFIsResponseInfo.RFI.Project.ContractsAdministrator, FileList);

            if (rFIsResponseInfo.RFI.Project.ProjectManager != null)
                SendRFIsResponse(rFIsResponseInfo, rFIsResponseInfo.RFI.Project.ProjectManager, FileList);

            //--- If Current user is not CA or PM or not in Distribution list then Send a copy of RFI to Him to   ------

            PeopleInfo CreatedpplInfo = PeopleController.GetInstance().GetPersonById(rFIsResponseInfo.RFI.CreatedBy);

            PeopleInfo pplInfo = Web.Utils.GetCurrentUser();

            if (CreatedpplInfo.Equals(pplInfo))
            {
                if (pplInfo != null)
                    if (pplInfo.Email != null && pplInfo.Type == PeopleInfo.PeopleTypeEmployee)
                        if (!pplInfo.Equals(rFIsResponseInfo.RFI.Project.ProjectManager) && !pplInfo.Equals(rFIsResponseInfo.RFI.Project.ContractsAdministrator))
                            SendRFIsResponse(rFIsResponseInfo, pplInfo, FileList);

            }
            else
            {
                if (CreatedpplInfo != null)
                    if (CreatedpplInfo.Email != null && CreatedpplInfo.Type == PeopleInfo.PeopleTypeEmployee)
                        if (!CreatedpplInfo.Equals(rFIsResponseInfo.RFI.Project.ProjectManager) && !CreatedpplInfo.Equals(rFIsResponseInfo.RFI.Project.ContractsAdministrator))
                            SendRFIsResponse(rFIsResponseInfo, CreatedpplInfo, FileList);


            }

        }


        #endregion






        /// <summary>
        /// Sends EOT to the specified person.
        /// </summary>
        public static void SendEOT(EOTInfo eOTInfo, PeopleInfo peopleInfo)
        {
            String fullNumber = eOTInfo.Project.FullNumber + " - " + UI.Utils.SetFormInteger(eOTInfo.Number);

            if (eOTInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = eOTInfo.Project.Name + " EOT No " + fullNumber;

            String message = "" +
               "Dear " + peopleInfo.FirstName + ",<br />" +
               "<br />" +
               "Please find attached EOT No " + fullNumber + "." +
               "<br />" +
               "<br />" +
               "<i>" + eOTInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateEOTReport(eOTInfo);
            String attachmentName = ("Project_" + eOTInfo.Project.Number + "_EOT_" + eOTInfo.Number.ToString()) + ".pdf";

            //#----
            if (eOTInfo.ClientBackuplFile != null)
            {
                FileInfo fileInfo = new FileInfo(UI.Utils.FullPath(eOTInfo.Project.AttachmentsFolder, eOTInfo.ClientBackuplFile));

                if (fileInfo.Exists)
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, new List<FileInfo> { fileInfo });
            }
            else
                //#---
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);

            eOTInfo.SendDate = DateTime.Now;
            ProjectsController.GetInstance().UpdateEOTSendDate(eOTInfo);
        }




        //#--------------To send  EOT email copy to CAand PM--------------------------------------------

        /// <summary>
        /// Sends EOT to the specified person.
        /// </summary>
        public static void SendEOT_CAandPM(EOTInfo eOTInfo, PeopleInfo peopleInfo)
        {
            String fullNumber = eOTInfo.Project.FullNumber + " - " + UI.Utils.SetFormInteger(eOTInfo.Number);

            if (eOTInfo.Project.ProjectManager == null)
                throw new Exception("Project manager not specified.");

            ProjectsController projectsController = ProjectsController.GetInstance();

            String subject = eOTInfo.Project.Name + " EOT No " + fullNumber;

            String message = "" +
              // "Dear " + eOTInfo.Project.ClientContact.FirstName
              "Hi" + ",<br />" +
               "<br />" +
               "Please find attached EOT No " + fullNumber + "." +
               "<br />" +
               "<br />" +
               "<i>" + eOTInfo.Project.ProjectManager.Name + "</i><br />" +
               "<i>Project Manager</i><br />" +
               "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";

            Byte[] attachment = projectsController.GenerateEOTReport(eOTInfo);
            String attachmentName = ("Project_" + eOTInfo.Project.Number + "_EOT_" + eOTInfo.Number.ToString()) + ".pdf";
            //#----
            if (eOTInfo.ClientBackuplFile != null)
            {
                FileInfo fileInfo = new FileInfo(UI.Utils.FullPath(eOTInfo.Project.AttachmentsFolder, eOTInfo.ClientBackuplFile));

                if (fileInfo.Exists)
                    Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName, new List<FileInfo> { fileInfo });
            }
            else
                //#---
                Utils.SendEmail(peopleInfo, subject, message, attachment, attachmentName);

            eOTInfo.SendDate = DateTime.Now;
            ProjectsController.GetInstance().UpdateEOTSendDate(eOTInfo);
        }

        //#--------------To send  EOT email copy to CAand PM--------------------------------------------


        /// <summary>
        /// Sends EOT to the distribution list
        /// </summary>
        public static void SendEOTToDistributionList(EOTInfo eOTInfo)
        {
            #region Old
            /*
            if (eOTInfo.Project.SendEOTToClientContact)
            {
                if (eOTInfo.Project.ClientContact.Email == null)
                    throw new Exception("Client contact's email not specified");

                SendEOT(eOTInfo, eOTInfo.Project.ClientContact);
            }

            if (eOTInfo.Project.SendEOTToClientContact1)
            {
                if (eOTInfo.Project.ClientContact1.Email == null)
                    throw new Exception("Client contact 1's email not specified");

                SendEOT(eOTInfo, eOTInfo.Project.ClientContact1);
            }

            if (eOTInfo.Project.SendEOTToClientContact2)
            {
                if (eOTInfo.Project.ClientContact2.Email == null)
                    throw new Exception("Client contact 2's email not specified");

                SendEOT(eOTInfo, eOTInfo.Project.ClientContact2);
            }

            if (eOTInfo.Project.SendEOTToSuperintendent)
            {
                if (eOTInfo.Project.Superintendent.Email == null)
                    throw new Exception("Superintendent's email not specified");

                SendEOT(eOTInfo, eOTInfo.Project.Superintendent);
            }

            if (eOTInfo.Project.SendEOTToQuantitySurveyor)
            {
                if (eOTInfo.Project.Superintendent.Email == null)
                    throw new Exception("Quantity surveyor's email not specified");

                SendEOT(eOTInfo, eOTInfo.Project.QuantitySurveyor);
            }

            */
            #endregion

            foreach (ClientContactInfo clientContact in eOTInfo.Project.ClientContactList)
            {
                if (clientContact.SendEOTs.Value && clientContact.Email != null)
                {
                    SendEOT(eOTInfo, clientContact);
                }
            }

            //#------To send CM approval Notification to CA and PM 
            if (eOTInfo.Project.ContractsAdministrator != null)
            {
                if (eOTInfo.Project.ContractsAdministrator.Email == null)
                    throw new Exception("ContractsAdministrator's email not specified");

                SendEOT_CAandPM(eOTInfo, eOTInfo.Project.ContractsAdministrator);
            }


            if (eOTInfo.Project.ProjectManager != null)
            {
                if (eOTInfo.Project.ProjectManager.Email == null)
                    throw new Exception("ProjectManager's email not specified");

                SendEOT_CAandPM(eOTInfo, eOTInfo.Project.ProjectManager);
            }
            //#------





        }







        /// <summary>
        /// Sends an email when overbudget happens after comparison approval
        /// </summary>
        public void SendOverbudgetEmail(TradeInfo tradeInfo)
        {
            // #-----AS per Alex direction blocked the SendOverbudgetEmail

            /*   
             *   
             *                        

            ProcessStepInfo processStepInfo = tradeInfo.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeComparisonDA); });

            if (processStepInfo != null && processStepInfo.SkipStep)
            {
                decimal winningQuote = TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation);
                decimal budget = (decimal)tradeInfo.BudgetParticipation.Amount;
                decimal overBudget = winningQuote - budget;

                if (overBudget > (decimal)tradeInfo.Project.BusinessUnit.TradeOverbudgetApproval)
                {
                    if (tradeInfo.Project.DirectorAuthorization == null)
                        throw new Exception("Sending over budget notification. Director authorization role not set in project " + tradeInfo.Project.Name);

                    String subject = "Over budget (" + UI.Utils.SetFormString(tradeInfo.Project.Name) + " - " + UI.Utils.SetFormString(tradeInfo.Name) + ")";

                    String body = "The following trade was let over budget as a result of amendments made to the comparison after the order letting meeting: <br />" +
                                  "<br />" +
                                  "Business unit: <b>" + UI.Utils.SetFormString(tradeInfo.Project.BusinessUnitName) + "</b><br />" +
                                  "Project: <b>" + UI.Utils.SetFormString(tradeInfo.Project.Name) + "</b><br />" +
                                  "Trade: <b>" + UI.Utils.SetFormString(tradeInfo.Name) + "</b><br />" +
                                  "Subcontractor: <b>" + UI.Utils.SetFormString(tradeInfo.SelectedSubContractorShortName) + "</b><br />" +
                                  "<br />" +
                                  "Over budget amount: <b>" + UI.Utils.SetFormDecimal(overBudget) + "</b><br />" +
                                  "Quote at comparison approval: <b>" + UI.Utils.SetFormDecimal(tradeInfo.ComparisonApprovalAmount) + "</b><br />" +
                                  "Quote at present: <b>" + UI.Utils.SetFormDecimal(winningQuote) + "</b><br />" +
                                  "Trade budget: <b>" + UI.Utils.SetFormDecimal(budget) + "</b><br />" +
                                  "<br />" +
                                  "<i>SOS Server</i><br />";

                    Utils.SendEmail(tradeInfo.Project.DirectorAuthorization, subject, body);



                    //#---- To Send email to CA and PM----
                    if (tradeInfo.Project.ContractsAdministrator!=null)
                    { 
                        if (tradeInfo.Project.ContractsAdministrator.Email == null)
                            throw new Exception("ContractsAdministrator's email not specified");
                         Utils.SendEmail(tradeInfo.Project.ContractsAdministrator, subject, body);
                    }
                    if(tradeInfo.Project.ProjectManager!=null)
                    { 
                    if (tradeInfo.Project.ProjectManager.Email == null)
                        throw new Exception("ProjectManager's email not specified");

                    Utils.SendEmail(tradeInfo.Project.ProjectManager, subject, body);
                    }
                    //#---- To Send email to CA and PM----



                }
            }


         */ // #-----AS per Alex direction blocked the SendOverbudgetEmail


        }





        //#----Added new Role and new approval process CA/PM/CM/COM/UM/DA based on Trade amount COM/UM/DA will approve

        public void UpdateContractProcessSteps(TradeInfo tradeInfo)
        {


            decimal TradeQuote = TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation);

            List<ContractApprovalLimitInfo> ContractApprovalLimitList = ContractsController.GetInstance().GetContractApprovalLimits();

            decimal WinLosspercent = 0;

            bool WinStatus = false;
            decimal TradeAllocation = tradeInfo.TotalBudgetAllowance;
            decimal winloss = (tradeInfo.BudgetParticipation.Amount.Value - TradeQuote);

            if (TradeAllocation == 0) { TradeAllocation = 1; }  // to calculate  WinLosspercent, we can't devide anything by zero

            WinLosspercent = (100 * winloss) / TradeAllocation;
            if (winloss >= 0)
                WinStatus = true;
            else
                WinLosspercent = WinLosspercent * (-1);



            if (TradeQuote <= ContractApprovalLimitList[0].Max)   //<=20k
            {
                #region <=20k    
                if (WinStatus)// win
                {
                    if (WinLosspercent >= 5) //CA Approves   // by default CA/PM/CM -- need to skip PM and CM approval
                    {
                        // to skip CM approval
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = true;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCA;   //---CONCA
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }
                        // to skip PM approval
                        processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractPM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = true;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCA; //CONCA
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }



                    }
                    else //PM Approves
                    {
                        // to skip CM approval
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = true;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractPM;   //---CONPM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }
                    }

                }
                else //Loss
                {

                    if (WinLosspercent <= 50)
                    {
                        //CM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;   //---CONCM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                    }
                    else  //COM Approves
                    {
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }
                    }

                }
                #endregion

            }



            else if (TradeQuote <= ContractApprovalLimitList[1].Max)
            {
                #region <=100k    
                if (WinStatus)// win  //PM Approves
                {
                    //TO Skip CM approval
                    ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                    if (processStepInfo != null)
                    {
                        processStepInfo.Skip = true;
                        processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractPM;   //---CONPM
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                    }


                }
                else //Loss
                {

                    if (WinLosspercent <= 5)
                    {

                        //CM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;   //---CONCM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }


                    }
                    else if (WinLosspercent > 5 && WinLosspercent <= 50)
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                    }
                    else //MD
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }


                        //UM Approves
                        processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;   //---CONUM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }


                    }

                }
                #endregion
            }
            else if (TradeQuote <= ContractApprovalLimitList[2].Max)
            {
                #region <=500k    
                if (WinStatus)// win
                {

                    if (WinLosspercent >= 5)//PM Approves
                    {
                        //Skip CM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = true;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractPM;   //---CONPM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                    }
                    else
                    {
                        //CM Approves
                    }


                }
                else //Loss
                {

                    if (WinLosspercent <= 5) //COM Approves
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                    }
                    else //MD   approves
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                        //UM Approves
                        processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }



                    }

                }
                #endregion
            }
            else if (TradeQuote <= ContractApprovalLimitList[3].Max)
            {
                #region <=1000k    
                if (WinStatus)// win
                {

                    if (WinLosspercent >= 5)
                    {
                        //CM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;   //---CONCM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                    }
                    else   //COM Approves
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }
                    }


                }
                else //Loss  //UM Approves
                {

                    //COM Approves
                    ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                    if (processStepInfo != null)
                    {
                        processStepInfo.Skip = false;
                        processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                    }

                    //UM Approves
                    processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                    if (processStepInfo != null)
                    {
                        processStepInfo.Skip = false;
                        processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                    }




                }
                #endregion
            }
            else if (TradeQuote >= ContractApprovalLimitList[4].Min)
            {
                #region>1000k    
                if (WinStatus)// win
                {

                    if (WinLosspercent >= 5) //COM Approves
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;   //---CONCO
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                    }
                    else  //MD
                    {
                        //COM Approves
                        ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }

                        //UM Approves
                        processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                        if (processStepInfo != null)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        }





                    }


                }
                else //Loss //MD
                {
                    //COM Approves
                    ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                    if (processStepInfo != null)
                    {
                        processStepInfo.Skip = false;
                        processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                    }

                    //UM Approves
                    processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                    if (processStepInfo != null)
                    {
                        processStepInfo.Skip = false;
                        processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;   //---CONUM
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                    }

                }
                #endregion
            }




            #region OLd 
            /*
             // For COM Apptroval---
             if (tradeInfo.Project.BusinessUnit.TradeComAmountApproval != null)
             {
                 ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });


                 if (processStepInfo != null)

                     if (TradeQuote > tradeInfo.Project.BusinessUnit.TradeComAmountApproval)
                     {
                         processStepInfo.Skip = false;
                         processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;
                         ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                     }
                     else
                     {
                         processStepInfo.Skip = true;
                         processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;
                         ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                     }
             }


             // For UM Apptroval---
             if (tradeInfo.Project.BusinessUnit.TradeAmountApproval != null)
             {
                 ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });


                 if (processStepInfo != null)

                     if (TradeQuote > tradeInfo.Project.BusinessUnit.TradeAmountApproval)
                     {
                         processStepInfo.Skip = false;
                         processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;
                         ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                     }
                     else
                     {
                         processStepInfo.Skip = true;
                         processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;
                         ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                     }
             }

             // For DA Apptroval---
             if (tradeInfo.Project.BusinessUnit.TradeDAAmountApproval != null)
             {
                 ProcessStepInfo processStepInfo = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractDA); });


                 if (processStepInfo != null)

                     if (TradeQuote > tradeInfo.Project.BusinessUnit.TradeDAAmountApproval)
                     {
                         processStepInfo.Skip = false;
                         processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractDA;
                         ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                     }
                     else
                     {
                         processStepInfo.Skip = true;
                         ProcessController.GetInstance().UpdateProcessStepSkip(processStepInfo);
                     }
             }
                */
            #endregion  Old

        }

        //#----






        //#----Added new Role and  process approval condition and new approval process CA/PM/CM/DM/COM/UM/DA based on variation Type and  amount
        public void UpdateVariationContractProcessSteps(VariationInfo variInfo)
        {
            ContractInfo contractInfo = variInfo.Contract;
            TradeInfo tradeInfo = variInfo.Contract.Trade;
            contractInfo.Process = GetDeepProcessWithProjectPeople(contractInfo);
            contractInfo = ContractsController.GetInstance().GetContractWithVariations(contractInfo.Id);
            //contractInfo.Trade.Project = ProjectsController.GetInstance().GetDeepProject(contractInfo.Trade.Project.Id);
            contractInfo.Trade.Project = ProjectsController.GetInstance().GetProject(contractInfo.Trade.Project.Id);

            bool dmFlag = false, coFlag = false, umFlag = false, daFlag = false;
            bool umFlag2 = false;              //DS2023-12-20
            foreach (VariationInfo varInfo in contractInfo.Variations)
            {
                // For DM Approval
                ProcessStepInfo processStepInfo = contractInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractDM); });
                if (processStepInfo != null && !dmFlag)
                {
                    if (varInfo.Type == "DV")
                    {
                        processStepInfo.Skip = false;
                        if (processStepInfo.Process.StepContractApproval == ProcessStepInfo.StepTypeContractCM)
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractDM;

                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        dmFlag = true;
                    }
                    else
                    {
                        if (!coFlag)
                        {
                            processStepInfo.Skip = true;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                            //  dmFlag = true;  
                        }
                    }
                }


                //For COM Approval
                if (varInfo.Type == "DV" || varInfo.Type == "V" || varInfo.Type == "CV" || varInfo.Type == "SA" || varInfo.Type == "TV" || varInfo.Type == "BOQ" || varInfo.Type == "BC")
                {
                    processStepInfo = contractInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractCO); });
                    if (processStepInfo != null && !coFlag)
                    {
                        if (varInfo.BudgetWinLoss < 0)
                        {
                            processStepInfo.Skip = false;
                            if (processStepInfo.Process.StepContractApproval != ProcessStepInfo.StepTypeContractDA)
                                processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO;

                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                            coFlag = true;

                        }
                        else
                        {
                            processStepInfo.Skip = true;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM;
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                            // coFlag = true;
                        }
                    }
                }


                //For UM and DA Approval

                decimal? varUMDAapprovalAmt = contractInfo.Trade.Project.BusinessUnit.VariationUMDAOverApproval;

                decimal? varUMBOQVCVDVAmt = contractInfo.Trade.Project.BusinessUnit.VariationUMBOQVCVDVApproval;  // DS20231205
                
                //----UM------
                processStepInfo = contractInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                if (processStepInfo != null && !umFlag)
                {
                    if (varInfo.Type == "DV" || varInfo.Type == "V" || varInfo.Type == "BC")
                    {
                        if (varUMDAapprovalAmt != null && varInfo.Amount > varUMDAapprovalAmt)
                        {
                            processStepInfo.Skip = false;
                            if (processStepInfo.Process.StepContractApproval != ProcessStepInfo.StepTypeContractDA)
                                processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;

                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                            umFlag = true;

                        }
                    }
                    if (varInfo.Type == "DV" || varInfo.Type == "V" || varInfo.Type == "CV" || varInfo.Type == "BOQ")  // DS20231205
                    {
                        if (varUMBOQVCVDVAmt != null && varInfo.Amount > varUMBOQVCVDVAmt)   //DS20240405 ?????? Ask Will Absolute Amount + Director
                        {
                           processStepInfo.Skip = false; 
                           //  if (processStepInfo.Process.StepContractApproval != ProcessStepInfo.StepTypeContractDA)   //DS2024-03-28
                           //     processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;     //DS2024-03-28

                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                            umFlag = true;
                            umFlag2 = true;  //DS2023-12-20
                        }
                    }
                    if (!umFlag)
                    {
                        processStepInfo.Skip = true;
                        if (coFlag) { processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO; } else { processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM; }
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        // umFlag = true;
                    }

                }



                //----DA------
                processStepInfo = contractInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractDA); });
                if (processStepInfo != null && !daFlag)
                {
                    if (varInfo.Type == "DV" || varInfo.Type == "V" || varInfo.Type == "BC")
                    {
                        if (varUMDAapprovalAmt != null && varInfo.Amount > varUMDAapprovalAmt)
                        {
                            processStepInfo.Skip = false;
                            processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractDA;
                            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                            daFlag = true;
                        }
                    }
                    if (!daFlag)
                    {
                        processStepInfo.Skip = true;
                        if (coFlag) 
                        {   processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCO; } 
                        else 
                        {  processStepInfo.Process.StepContractApproval = ProcessStepInfo.StepTypeContractCM; }     
                        ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfo);
                        // daFlag = true;
                    }
                }

            }
            //DS2023-12-20   >>>
            if (umFlag2)
            {
                ProcessStepInfo processStepInfoUM = contractInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
                if (processStepInfoUM != null)
                {
                    if (processStepInfoUM.Process.StepContractApproval == ProcessStepInfo.StepTypeContractCM || processStepInfoUM.Process.StepContractApproval == ProcessStepInfo.StepTypeContractCO)     //DS2024-03-28  ADDED CO
                    {
                        processStepInfoUM.Process.StepContractApproval = ProcessStepInfo.StepTypeContractUM;
                         ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfoUM);  //ds20240328
                    }
                }
            }
            //else
            //{
            //    ProcessStepInfo processStepInfoDM = contractInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Type.Equals(ProcessStepInfo.StepTypeContractUM); });
            //    if (processStepInfoDM != null)
            //    {
            //        if (processStepInfoDM.Process.StepContractApproval == ProcessStepInfo.StepTypeContractCM || processStepInfoDM.Process.StepContractApproval == ProcessStepInfo.StepTypeContractCO)     //DS2024-03-28  ADDED CO
            //        {
            //            processStepInfoDM.Process.StepContractApproval = ProcessStepInfo.StepTypeContractDM;
            //            ProcessController.GetInstance().UpdateProcessStepSkipStepContractApproval(processStepInfoDM);  //ds20240328
            //        }
            //    }
            //}
            //DS2023-12-20   <<<
        }

        //#----






        /// <summary>
        /// Executes a process step according to its type
        /// </summary>
        public void ExecuteProcessStep(ProcessStepInfo processStepInfo)
        {
            ExecuteProcessStep(processStepInfo, null);
        }

        /// <summary>
        /// Executes a process step according to its type
        /// </summary>
        public void ExecuteProcessStep(ProcessStepInfo processStepInfo, String comments)
        {
            String checkStepInfo = CheckProcessStep(processStepInfo);
            ProcessStepInfo nextProcessStep = GetNextProcessStep(processStepInfo);
            ReversalInfo reversal;

            if (!AllowApprovalCurrentUser(processStepInfo))
                throw new Exception("Unauthorized access. You can not approve step " + processStepInfo.Name + " for process " + processStepInfo.Process.Name + " in project " + processStepInfo.Process.Project.Name);

            if (checkStepInfo != null)
                throw new Exception("The step: " + processStepInfo.Name + " can not be executed. " + checkStepInfo);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                switch (processStepInfo.Type)
                {
                    case ProcessStepInfo.StepTypeGenerateWorkOrder:
                        processStepInfo.Process.Project.Trades[0].WorkOrderNumber = ContractsController.GetInstance().CreateWorkOrder(processStepInfo.Process.Project.BusinessUnit);
                        TradesController.GetInstance().SetTradeWorkOrder(processStepInfo.Process.Project.Trades[0]);
                        break;

                    case ProcessStepInfo.StepTypeCreateContract://#--This step called when CA clicks on create Contract--CRECO               
                        SendOverbudgetEmail(processStepInfo.Process.Project.Trades[0]);
                        processStepInfo.Process.Project.Trades[0].Contract.Id = ContractsController.GetInstance().AddContract(processStepInfo.Process.Project.Trades[0].Contract);

                        //#-----Added new Role and new approval process CA/PM/CM/COM/UM/DA based on Trade amount COM/UM/DA will approve
                        if (processStepInfo.Process.Project.Trades[0].JobTypeName == "Construction")
                        {
                            UpdateContractProcessSteps(processStepInfo.Process.Project.Trades[0]);
                        }
                        //#-----


                        TradesController.GetInstance().SetTradeContract(processStepInfo.Process.Project.Trades[0]);
                        ProjectsController.GetInstance().UpdateTradeBudgets(processStepInfo.Process.Project.Trades[0]);

                        if (nextProcessStep == null)
                            if (processStepInfo.Process.Project.Trades[0].Contract.Process != null)
                                if (processStepInfo.Process.Project.Trades[0].Contract.Process.Steps != null)
                                    if (processStepInfo.Process.Project.Trades[0].Contract.Process.Steps.Count > 0)
                                    {
                                        processStepInfo.Process.Project.Trades[0].Contract.Process.Project = processStepInfo.Process.Project;
                                        nextProcessStep = processStepInfo.Process.Project.Trades[0].Contract.Process.Steps[0];
                                    }

                        break;
                        // DS20231211
                    case ProcessStepInfo.StepTypeClientVariationInternalApproval:    //# ----- This step is called when CM appoves Client variation and Seperate Account Variation
                        bool isCVSUMActive = false;
                        if (nextProcessStep != null)
                        {
                            if (nextProcessStep.Type == "CVSUM" || nextProcessStep.Skip == false)
                            {
                                isCVSUMActive = true;
                            }
                        }
                        if (isCVSUMActive == false)
                        {
                            ProjectsController.GetInstance().UpdateClientVariationInternalApproval(processStepInfo.Process.Project.ClientVariations[0]);
                            SendClientVariationToDistributionList(processStepInfo.Process.Project.ClientVariations[0]);
                        }
                        break;
                    case ProcessStepInfo.StepTypeClientVariationUM:    //# ----- This step is called when UM appoves Client variation and Seperate Account Variation
                        ProjectsController.GetInstance().UpdateClientVariationInternalApproval(processStepInfo.Process.Project.ClientVariations[0]);
                        SendClientVariationToDistributionList(processStepInfo.Process.Project.ClientVariations[0]);
                        break;

                    case ProcessStepInfo.StepTypeClientVariationClientVerbalApproval: //# ----- This step is called when CA appoves Record Verbal  Approval in Client variation
                        ProjectsController.GetInstance().UpdateClientVariationVerbalApproval(processStepInfo.Process.Project.ClientVariations[0]);
                        break;

                    case ProcessStepInfo.StepTypeClientVariationClientFinalApproval://# ----- This step is called when CA appoves  Record Final Approval in Client Vatriations/Seperate Account variations and sends email to accountId for finace side
                        ProjectsController.GetInstance().UpdateClientVariationFinalApproval(processStepInfo.Process.Project.ClientVariations[0]);
                        SendClientVariationCodesToAccounts(processStepInfo.Process.Project.ClientVariations[0]);
                        break;

                    case ProcessStepInfo.StepTypeClientVariationWorksCompleted:
                        ProjectsController.GetInstance().UpdateSeparateAccountWorksCompleted((SeparateAccountInfo)processStepInfo.Process.Project.ClientVariations[0]);
                        break;

                    case ProcessStepInfo.StepTypeClientVariationSendInvoice:
                        ProjectsController.GetInstance().UpdateSeparateAccountInvoiceSent((SeparateAccountInfo)processStepInfo.Process.Project.ClientVariations[0]);
                        SendSeparateAccountInvoiceToDistributionList((SeparateAccountInfo)processStepInfo.Process.Project.ClientVariations[0]);
                        break;

                    case ProcessStepInfo.StepTypeClaimDraftApproval://# ----- This step is called when PM approves"Draft Approval" in Claims
                        ProjectsController.GetInstance().UpdateClaimDraftApproval(processStepInfo.Process.Project.Claims[0]);
                        SendClaimToDistributionList(processStepInfo.Process.Project.Claims[0], processStepInfo.Process.Project.Claims[1], false);
                        break;

                    case ProcessStepInfo.StepTypeClaimInvoiceInternalApproval://# ----- This step is called when PM approves"Invoice Approval" in Claims
                        ProjectsController.GetInstance().UpdateClaimInternalApproval(processStepInfo.Process.Project.Claims[0]);
                        break;

                    case ProcessStepInfo.StepTypeClaimInvoiceFinalApproval://# ----- This step is called when FC approves"Invoice Approval" in Claims
                        ProjectsController.GetInstance().UpdateClaimApproval(processStepInfo.Process.Project.Claims[0]);
                        SendClaimToDistributionList(processStepInfo.Process.Project.Claims[0], processStepInfo.Process.Project.Claims[1], true);
                        break;

                    default:
                        break;
                }



                if (processStepInfo.Process.StepComparisonApproval != null)
                    if (processStepInfo.Process.StepComparisonApproval == processStepInfo.Type)//# ----- This step is called When process step type= COMCM,COMCO,COMUM,COMDA
                    {
                        TradesController tradesController = TradesController.GetInstance();

                        processStepInfo.Process.Project.Trades[0].ComparisonApprovalDate = DateTime.Today;
                        processStepInfo.Process.Project.Trades[0].ComparisonApprovalAmount = tradesController.GetQuoteTotal(processStepInfo.Process.Project.Trades[0].SelectedParticipation);
                        tradesController.SetTradeComparisonApprovalInfo(processStepInfo.Process.Project.Trades[0]);
                    }





                if (processStepInfo.Process.StepContractApproval != null)
                    if (processStepInfo.Process.StepContractApproval == processStepInfo.Type)//# ----- This step is called when a Contract approved by "StepContractApproval" it may be CONCM ,CONCOM,CONUM or CONDA
                    {
                        processStepInfo.Process.Project.Trades[0].Contract.ApprovalDate = DateTime.Today;
                        ContractsController.GetInstance().SetContractApprovalDate(processStepInfo.Process.Project.Trades[0].Contract);

                        if (processStepInfo.Process.Project.Trades[0].SelectedParticipation.Contact.Email == null)
                            throw new Exception("Subcontractor contact does not have email address.");

                        if (processStepInfo.Process.Project.Fax == null)
                            throw new Exception("Project does not have a site fax number.");

                        if (processStepInfo.Process.Project.Foreman == null)
                            throw new Exception("Project does not have a Foreman.");

                        if (processStepInfo.Process.Project.Trades[0].Contract.IsSubContract && processStepInfo.Process.Project.Trades[0].Contract.Variations != null)
                        {
                            IBudget budgetProvider;
                            ContractsController contractsController = ContractsController.GetInstance();
                            List<IBudget> projectBudget = ProjectsController.GetInstance().GetProjectBudget(processStepInfo.Process.Project, false, false);

                            foreach (VariationInfo variationInfo in processStepInfo.Process.Project.Trades[0].Contract.Variations)
                            {
                                budgetProvider = projectBudget.Find(pb => pb.Equals(variationInfo.BudgetProvider));

                                if (budgetProvider != null)
                                {
                                    //#--variationInfo.BudgetAmountInitial = budgetProvider.BudgetAmountInitial;
                                    variationInfo.BudgetAmountTradeInitial = budgetProvider.BudgetAmountTradeInitial;

                                    contractsController.UpdateVariation(variationInfo);
                                }
                            }
                        }

                        SendContractToSubcontractor(processStepInfo.Process.Project.Trades[0]);

                        SendContractSummaryToSite(processStepInfo.Process.Project.Trades[0]);

                        SendContractNotificationToAccounts(processStepInfo.Process.Project.Trades[0]);
                    }

                processStepInfo.ActualDate = DateTime.Now;
                processStepInfo.ApprovedBy = new EmployeeInfo(Web.Utils.GetCurrentUserId());
                processStepInfo.AssignedTo = GetStepAssignee(processStepInfo);

                reversal = processStepInfo.PendingReversal;

                if (reversal != null)
                {
                    reversal.ReplyBy = Web.Utils.GetCurrentUser();
                    reversal.ReplyDate = DateTime.Now;
                    reversal.ReplyNote = comments;

                    UpdateReversal(reversal);
                }

                UpdateProcessStep(processStepInfo);

                scope.Complete();
            }

            //#--if over budget >9,999 Send email notification estimator head
            if (processStepInfo.Role == "UM" && processStepInfo.Process.Name == "Comparison CA/PM/CM/COM/UM/DA")
            {
                if ((processStepInfo.Process.Project.Trades[0].TradeBudgets[0].BudgetWinLoss) > processStepInfo.Process.Project.BusinessUnit.TradeUMOverbudgetApproval)
                {
                    NotifyEstimatingDirector(processStepInfo);
                }

            }
            //#--if over budget >9,999 Send email notification estimator head



            if (nextProcessStep != null)
                SendNextStepEMail(nextProcessStep);
        }



        //#--ifTrade over budget >9,999 Send emailnotification to Estimating Director
        public void NotifyEstimatingDirector(ProcessStepInfo processStepInfo)
        {
            EmployeeInfo stepAssignee = processStepInfo.Process.Project.BusinessUnit.EstimatingDirector;
            String subject;
            String body;
            String contextInfo;

            if (stepAssignee != null)
            {
                if (processStepInfo.Process.Project.Trades != null)
                { }
                subject = "Trade over budget notification -(" + processStepInfo.Process.Project.Name + "/" + processStepInfo.Process.Project.Trades[0].Name + ")";
                contextInfo = "Trade: <b>" + processStepInfo.Process.Project.Trades[0].Name + "</b><br />";


                body = stepAssignee.FirstName + "</b>, a comparison, that is over budget, has just been approved.<br />" + "  Please find the over budgeted trade info: <br />" +
                       "<br />" +
                       "Project: <b>" + processStepInfo.Process.Project.Name + "</b><br />" +
                       "Trade: <b>" + processStepInfo.Process.Project.Trades[0].Name + "</b><br />" +
                       "Step: <b>" + processStepInfo.Name + "</b><br />" +
                       "Comparison: <b>" + processStepInfo.Process.Project.Trades[0].TradeBudgets[0].BudgetAmountAllowance.Value.ToString("c") + "</b><br />" +
                       "Contract: <b>" + processStepInfo.Process.Project.Trades[0].TradeBudgets[0].Amount.Value.ToString("c") + "</b><br />" +
                       "Budget Win/Loss: <b>" + processStepInfo.Process.Project.Trades[0].TradeBudgets[0].BudgetWinLoss.Value.ToString("c") + "</b><br />" +
                       "<br />" +
                       "<br />" +
                       "<i>SOS Server<br />Vaughan Constructions</i><br />";  //DS20231214

                Utils.SendEmail(stepAssignee, subject, body);
            }
        }









        /// <summary>
        /// Returns the name of the Assignee that executed or will execute a step
        /// </summary>
        public String GetStepAssigneeName(ProcessStepInfo processStepInfo)
        {
            if (processStepInfo.AssignedTo != null)
                return processStepInfo.AssignedTo.Name;
            else
            {
                EmployeeInfo stepAssignee = GetStepAssignee(processStepInfo);
                return stepAssignee != null ? stepAssignee.Name : "?";
            }
        }

        /// <summary>
        /// Returns the step pending for approval. The first one that does not have an actual date and is not skipped
        /// </summary>
        public ProcessStepInfo GetCurrentStep(ProcessInfo processInfo)
        {
            return processInfo.Steps.Find(delegate (ProcessStepInfo processStepInfo) { return processStepInfo.ActualDate == null && !processStepInfo.SkipStep; });
        }

        public List<ProcessStepInfo> GetPendingSteps(ProcessInfo processInfo)
        {
            List<ProcessStepInfo> pendingSteps = new List<ProcessStepInfo>();

            foreach (ProcessStepInfo processStepInfo in processInfo.Steps)
                if (processStepInfo.ActualDate == null && !processStepInfo.SkipStep)
                    pendingSteps.Add(processStepInfo);

            return pendingSteps;
        }

        /// <summary>
        /// Returns the process previous step of a process step. The process can not be null and must have at least one step.
        /// </summary>
        public ProcessStepInfo GetPreviousProcessStep(ProcessStepInfo processStepInfo)
        {
            ProcessStepInfo previousStep = null;
            Int32 i = -1;

            foreach (ProcessStepInfo currentStep in processStepInfo.Process.Steps)
            {
                if (currentStep.Equals(processStepInfo))
                {
                    if (previousStep != null)
                    {
                        while (previousStep.SkipStep && i >= 0)
                        {
                            previousStep = processStepInfo.Process.Steps[i];
                            i--;
                        }

                        if (i < 0)
                            previousStep = null;
                    }

                    return previousStep;
                }
                else
                    previousStep = currentStep;

                i++;
            }

            throw new Exception("Process Step " + processStepInfo.Name + " not found in process " + processStepInfo.Process.Name);
        }

        /// <summary>
        /// Returns the last executed step
        /// </summary>
        public ProcessStepInfo GetLastStep(ProcessInfo processInfo)
        {
            if (processInfo != null && processInfo.Steps.Count > 0)
            {
                ProcessStepInfo pendingStep = GetCurrentStep(processInfo);

                if (pendingStep != null)
                    return GetPreviousProcessStep(pendingStep);
                else
                   //#---07/11/2019-- return processInfo.Steps[processInfo.Steps.Count - 1];
                   if (processInfo.StepContractApproval != null)
                    return processInfo.Steps.Find(X => X.Type.Contains(processInfo.StepContractApproval));
                else return processInfo.Steps[processInfo.Steps.Count - 1];
                //#---07/11/2019-- 
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the process next step of a process step. The process can not be null and must have at least one step.
        /// </summary>
        public ProcessStepInfo GetNextProcessStep(ProcessStepInfo processStepInfo)
        {
            ProcessStepInfo nextProcessStep = null;

            Int32 posCurrent = processStepInfo.Process.Steps.IndexOf(processStepInfo);
            if (posCurrent == -1)
                throw new Exception("Process Step " + processStepInfo.Name + " not found in process " + processStepInfo.Process.Name);
            else
                while (posCurrent < processStepInfo.Process.Steps.Count - 1)
                    if (!processStepInfo.Process.Steps[posCurrent + 1].SkipStep)
                    {
                        nextProcessStep = processStepInfo.Process.Steps[posCurrent + 1];
                        nextProcessStep.Process = processStepInfo.Process;
                        break;
                    }
                    else
                        posCurrent++;

            return nextProcessStep;
        }



        /// <summary>
        /// Determines if a process is reversible to an specified previous step
        /// Checks the Step Passed Along with Following Step incase it is Conditional
        /// </summary>
        public Boolean IsReversible(ProcessStepInfo processStepInfo,ProcessStepInfo FollowingProcessStepInfo) //ds20231212
        {
            Boolean isReversible = false;

            if (processStepInfo != null)
                if (AllowReversalCurrentUser(processStepInfo))
                {
                    isReversible = true;

                    if (processStepInfo.Process.StepContractApproval != null)
                        if (processStepInfo.Process.StepContractApproval == processStepInfo.Type && FollowingProcessStepInfo.Type != ProcessStepInfo.StepTypeContractUM)   // ds20231218
                            isReversible = false;

                    if (processStepInfo.Process.StepContractApproval != null)
                        if (processStepInfo.Process.StepContractApproval == ProcessStepInfo.StepTypeContractUM && processStepInfo.Type == ProcessStepInfo.StepTypeContractUM)   // ds20231220
                            isReversible = false;

                    if (isReversible)
                        if (processStepInfo.Process.StepComparisonApproval != null)
                            if (processStepInfo.Process.StepComparisonApproval == processStepInfo.Type)
                                isReversible = false;

                    if (isReversible)
                        if (processStepInfo.Type == ProcessStepInfo.StepTypeGenerateWorkOrder ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeCreateContract ||
                            //processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationInternalApproval ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationClientVerbalApproval ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationClientFinalApproval ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationWorksCompleted ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationSendInvoice ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeClaimDraftApproval ||
                            processStepInfo.Type == ProcessStepInfo.StepTypeClaimInvoiceInternalApproval ||
                            (processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationInternalApproval && (FollowingProcessStepInfo.Type != ProcessStepInfo.StepTypeClientVariationUM && FollowingProcessStepInfo.Type != ProcessStepInfo.StepTypeContractUM)))  // DS20231208
                             //(CurrentProcessStepInfo.Type == ProcessStepInfo.StepTypeClientVariationUM && processStepInfo.Type == ProcessStepInfo.StepTypeClientVariationInternalApproval))  //ds20231212


                            isReversible = false;
                }

                //#---when contract type construction and one of  its variation type is Design  to display Reverse btn
                else if (processStepInfo.Process.Steps.Count == 7 && processStepInfo.Process.Steps[3].Skip == false && processStepInfo.Process.Steps[3].Status == "Pending" && processStepInfo.Process.Steps[2].Status == "Approved")
                {
                    isReversible = true;

                }


            //#

            return isReversible;
        }

        /// <summary>
        /// Moves a process to a previous step
        /// </summary>
        //#---   public void ReverseProcessStep(ProcessStepInfo processStepInfo, String reason)

        //#----Set optional Parameter to to send trade name in email notification//--#
        public void ReverseProcessStep(ProcessStepInfo processStepInfo, String reason, string TradeName = "")//#-- optional Tradename parameter passed
        {
            ProcessStepInfo previousStep;
            ReversalInfo reversal;

            //must be able to execute current step to reverse the process
            if (!AllowApprovalCurrentUser(processStepInfo))
                throw new Exception("Unauthorized access. You can not approve step " + processStepInfo.Name + " for process " + processStepInfo.Process.Name + " in project " + processStepInfo.Process.Project.Name);

            previousStep = GetPreviousProcessStep(processStepInfo);

            //#--Send email notification to Previous approver



            String subject = "Reversal process step notification - " + processStepInfo.Process.Project.Name;

            String message = "" +
               "Dear " + previousStep.ApprovedByName + ",<br />" +
               "<br />" +
               "The below process step is reversed." + "<br />" +
               "<br />" + "Process step: " + previousStep.Name +
              "<br />" + "Process Name: " + processStepInfo.Process.Name +                  // previousStep.ProcessName//
               "<br />" + "Project Name : " + processStepInfo.Process.Project.Name +
               "<br />" + "Reason: " + reason;
            if (TradeName.Length > 0)
            { message += "<br />" + "Trade Name: " + TradeName; }

            message += "<br />" + "Reversed by: " + processStepInfo.StepAssigneeName +

            "<br />" +
            "<br />" +
            "<i>SOS Administrator</i><br />" +
            "<i>" + Web.Utils.GetConfigListItemValue("Global", "Settings", "CompanyLongName") + "</i><br />";


            Utils.SendEmail(previousStep.ApprovedBy, subject, message);

            //#--Send email notification to Previous approver 

            if (IsReversible(previousStep, processStepInfo) )  //DS20231212
            {
                reversal = new ReversalInfo();
                reversal.ProcessStep = previousStep;
                reversal.ReversalBy = Web.Utils.GetCurrentUser();
                reversal.ReversalDate = DateTime.Now;
                reversal.ReversalNote = reason;

                previousStep.ActualDate = null;
                previousStep.AssignedTo = null;
                previousStep.ApprovedBy = null;
                previousStep.Reversals.Add(reversal);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    reversal.Id = AddReversal(reversal);
                    UpdateProcessStep(previousStep);
                    scope.Complete();
                }

            }
            else
            {
                throw new Exception("The process step: " + processStepInfo.Name + " for process " + processStepInfo.Process.Name + " is not reversible.");
            }
        }
        #endregion

        #region Reversal Methods
        /// <summary>
        /// Creates a ReversalInfo object from a dr and dictionary
        /// </summary>
        public ReversalInfo CreateReversal(IDataReader dr, Dictionary<int, PeopleInfo> peopleDictionary)
        {
            ReversalInfo reversalInfo = new ReversalInfo(Data.Utils.GetDBInt32(dr["ReversalId"]));

            reversalInfo.ReversalNote = Data.Utils.GetDBString(dr["ReverseNote"]);
            reversalInfo.ReplyNote = Data.Utils.GetDBString(dr["ReplyNote"]);
            reversalInfo.ReversalDate = Data.Utils.GetDBDateTime(dr["ReverseDate"]);
            reversalInfo.ReplyDate = Data.Utils.GetDBDateTime(dr["ReplyDate"]);

            reversalInfo.ProcessStep = new ProcessStepInfo(Data.Utils.GetDBInt32(dr["ProcessStepId"]));

            if (dr["ReversePeopleId"] != DBNull.Value)
                reversalInfo.ReversalBy = PeopleController.GetInstance().CreatePerson(dr["ReversePeopleId"], peopleDictionary);

            if (dr["ReplyPeopleId"] != DBNull.Value)
                reversalInfo.ReplyBy = PeopleController.GetInstance().CreatePerson(dr["ReplyPeopleId"], peopleDictionary);

            return reversalInfo;
        }

        /// <summary>
        /// Creates a ReversalInfo object from a dr
        /// </summary>
        public ReversalInfo CreateReversal(IDataReader dr)
        {
            return CreateReversal(dr, null);
        }

        /// <summary>
        /// Gets all the Reversals for a process from database
        /// </summary>
        public List<ReversalInfo> GetReversals(ProcessInfo processInfo)
        {
            IDataReader dr = null;
            List<ReversalInfo> ReversalInfoList = new List<ReversalInfo>();
            ReversalInfo reversalInfo;

            try
            {
                dr = Data.DataProvider.GetInstance().GetReversalsByProcess(processInfo.Id);
                while (dr.Read())
                {
                    reversalInfo = CreateReversal(dr);
                    reversalInfo.ProcessStep.Process = processInfo;

                    ReversalInfoList.Add(reversalInfo);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Reversals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return ReversalInfoList;
        }

        /// <summary>
        /// Adds a Reversal to the database
        /// </summary>
        public int? AddReversal(ReversalInfo ReversalInfo)
        {
            int? ReversalId = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(ReversalInfo.ProcessStep.Id);
            parameters.Add(ReversalInfo.ReversalBy.Id);
            parameters.Add(ReversalInfo.ReversalDate);
            parameters.Add(ReversalInfo.ReversalNote);

            try
            {
                ReversalId = Data.DataProvider.GetInstance().AddReversal(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Reversal to database");
            }

            return ReversalId;
        }

        /// <summary>
        /// Updates a Reversal in the database
        /// </summary>
        public void UpdateReversal(ReversalInfo ReversalInfo)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(ReversalInfo.Id);
            parameters.Add(ReversalInfo.ReplyBy.Id);
            parameters.Add(ReversalInfo.ReplyDate);
            parameters.Add(ReversalInfo.ReplyNote);

            try
            {
                Data.DataProvider.GetInstance().UpdateReversal(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Reversal in database");
            }
        }
        #endregion

        #endregion

    }
}
