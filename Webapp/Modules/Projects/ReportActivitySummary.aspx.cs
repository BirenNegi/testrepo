using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;
using System.Linq;
using ProcessInfo = SOS.Core.ProcessInfo;

namespace SOS.Web
{
    public partial class ReportActivitySummaryPage : System.Web.UI.Page
    {

#region Private Methods
        private TaskTotal InitializeTaskTotal(Dictionary<int, Dictionary<int, TaskTotal>> taskTotalDictionary, ProjectInfo project, List<TaskTotal> taskTotalList, PeopleInfo peopleInfo)
        {
            TaskTotal taskTotal;

            if (!taskTotalDictionary.ContainsKey(project.Id.Value))
                taskTotalDictionary.Add(project.Id.Value, new Dictionary<int, TaskTotal>());

            if (!taskTotalDictionary[project.Id.Value].ContainsKey(peopleInfo.Id.Value))
            {
                taskTotal = new TaskTotal();

                taskTotal.ProjectIdStr = UI.Utils.SetFormString(project.IdStr);
                taskTotal.ProjectName = UI.Utils.SetFormString(project.Name);
                taskTotal.EmployeeIdStr = UI.Utils.SetFormString(peopleInfo.IdStr);
                taskTotal.EmployeeName = UI.Utils.SetFormString(peopleInfo.Name);
                taskTotal.EmployeeLastLogin = peopleInfo.LastLoginDate;

                if (project.BusinessUnit != null)
                {
                    taskTotal.BusinessUnitIdStr = project.BusinessUnit.IdStr;
                    taskTotal.BusinessUnitName = project.BusinessUnit.Name;
                }
                else
                {
                    taskTotal.BusinessUnitIdStr = "0";
                    taskTotal.BusinessUnitName = "Unknown";
                }

                taskTotalList.Add(taskTotal);
                taskTotalDictionary[project.Id.Value].Add(peopleInfo.Id.Value, taskTotal);
            }
            else
            {
                taskTotal = taskTotalDictionary[project.Id.Value][peopleInfo.Id.Value];
            }

            return taskTotal;
        }

        private void BindReport()
        {
            List<ReportParameter> reportParameterList = new List<ReportParameter>();

            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps;
            Dictionary<int, List<ProcessStepInfo>> execdutedProcessSteps;
            Dictionary<int, Dictionary<int, TaskTotal>> taskTotalDictionary = new Dictionary<int, Dictionary<int, TaskTotal>>();
            Dictionary<int, List<EmployeeInfo>> selectedUsers = new Dictionary<int, List<EmployeeInfo>>();
            StringDictionary processPeople = new StringDictionary();
            String processPerson;
            Boolean includeStep;

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            List<TaskTotal> taskTotalList = new List<TaskTotal>();

            TaskTotal taskTotal;

            String entityName;
            DateTime? actualDate;
            DateTime? targetDate;
            String task;
            int? dueDays;

            List<CompletedTask> completedTasksList = new List<CompletedTask>();
            CompletedTask completedTask;

            List<PendingTask> pendingTasksList = new List<PendingTask>();
            PendingTask pendingTask;

            List<ProjectInfo> projectInfoList = sosFilterSelector.Projects;
           
            projectsController.InitializeHolidays();

            foreach (ProjectInfo project in projectInfoList)
            {

                projectsController.GetProjectPeopleNames(project);
                selectedUsers.Add(project.Id.Value, sosFilterSelector.Employees);
                


            }

           
             pendingProcessSteps = processController.GetPendingStepsForActivityReport(projectInfoList, selectedUsers, sosFilterSelector.StartDate, sosFilterSelector.EndDate,false);

             execdutedProcessSteps = processController.GetExecutedSteps(projectInfoList, selectedUsers, sosFilterSelector.StartDate, sosFilterSelector.EndDate);

            #region Activity Report 


            // Adds up all the pending process steps
            foreach (ProjectInfo project in projectInfoList)
            {
                if (pendingProcessSteps.ContainsKey(project.Id.Value))
                {
                    processPeople.Clear();

                    foreach (ProcessStepInfo processStep in pendingProcessSteps[project.Id.Value])
                    {
                        // Include only a step per person per process
                        if (processStep.Process.Id == null)
                        {
                            includeStep = true;
                        }
                        else
                        {
                            processPerson = processStep.Process.IdStr + "_" + processStep.AssignedTo.IdStr;

                            if (processPeople.ContainsKey(processPerson))
                            {
                                includeStep = false;
                            }
                            else
                            {
                                processPeople.Add(processPerson, String.Empty);
                                includeStep = true;
                            }
                        }

                        if (includeStep)
                        {
                            taskTotal = InitializeTaskTotal(taskTotalDictionary, project, taskTotalList, processStep.AssignedTo);

                            switch (processStep.ProcessName)
                            {
                                case ProcessInfo.ProcessClaim: taskTotal.ClaimsTotalPending++; break;
                                case ProcessInfo.ProcessClientVariation: taskTotal.ClientVariationsTotalPending++; break;
                                case ProcessInfo.ProcessSeparateAccount: taskTotal.SeparateAccountsTotalPending++; break;
                                case ProcessInfo.ProcessComparison: taskTotal.ComparisonsTotalPending++; break;
                                case ProcessInfo.ProcessContract: taskTotal.ContractsTotalPending++; break;
                               // case ProcessInfo.ProcessVariation: taskTotal.ClientVariationsTotalPending++; break;
                                case ProcessInfo.ProcessTransmittal: taskTotal.TransmittalsTotalPending++; break;
                                case ProcessInfo.ProcessRFI: taskTotal.RFIsTotalPending++; break;
                                case ProcessInfo.ProcessEOT: taskTotal.EOTsTotalPending++; break;
                            }
                        }
                    }
                }
            }

            // Adds up all the approved process steps
            foreach (ProjectInfo project in projectInfoList)
            {
                if (execdutedProcessSteps.ContainsKey(project.Id.Value))
                {
                    processPeople.Clear();

                    foreach (ProcessStepInfo processStep in execdutedProcessSteps[project.Id.Value])
                    {
                        // Include only a step per person per process
                        if (processStep.Process.Id == null)
                        {
                            includeStep = true;
                        }
                        else
                        {
                            processPerson = processStep.Process.IdStr + "_" + processStep.ApprovedBy.IdStr;

                            if (processPeople.ContainsKey(processPerson))
                            {
                                includeStep = false;
                            }
                            else
                            {
                                processPeople.Add(processPerson, String.Empty);
                                includeStep = true;
                            }
                        }

                        if (includeStep)
                        {
                            taskTotal = InitializeTaskTotal(taskTotalDictionary, project, taskTotalList, processStep.ApprovedBy);

                            switch (processStep.ProcessName)
                            {
                                case ProcessInfo.ProcessClaim: taskTotal.ClaimsTotalApproved++; break;
                                case ProcessInfo.ProcessClientVariation: taskTotal.ClientVariationsTotalApproved++; break;
                                case ProcessInfo.ProcessSeparateAccount: taskTotal.SeparateAccountsTotalApproved++; break;
                                case ProcessInfo.ProcessComparison: taskTotal.ComparisonsTotalApproved++; break;
                                case ProcessInfo.ProcessContract: taskTotal.ContractsTotalApproved++; break;
                               // case ProcessInfo.ProcessVariation: taskTotal.ClientVariationsTotalApproved++; break;
                                case ProcessInfo.ProcessTransmittal: taskTotal.TransmittalsTotalApproved++; break;
                                case ProcessInfo.ProcessRFI: taskTotal.RFIsTotalApproved++; break;
                                case ProcessInfo.ProcessEOT: taskTotal.EOTsTotalApproved++; break;
                            }
                        }
                    }
                }
            }

            #endregion Activity report
            if (ChkBrkdown.Checked)
            { 
                //-------------Completed Task List------------------------
                #region Complited Task list
            

            foreach (ProjectInfo project in projectInfoList)
            {
                if (execdutedProcessSteps.ContainsKey(project.Id.Value))
                {
                    foreach (ProcessStepInfo processStep in execdutedProcessSteps[project.Id.Value])
                    {
                        completedTask = new CompletedTask();

                        if (project.BusinessUnit != null)
                        {
                            completedTask.BusinessUnitIdStr = project.BusinessUnit.IdStr;
                            completedTask.BusinessUnitName = project.BusinessUnit.Name;
                        }
                        else
                        {
                            completedTask.BusinessUnitIdStr = "0";
                            completedTask.BusinessUnitName = "Unknown";
                        }

                        completedTask.ProjectIdStr = UI.Utils.SetFormString(project.IdStr);
                        completedTask.ProjectName = UI.Utils.SetFormString(project.Name);
                        completedTask.EmployeeIdStr = UI.Utils.SetFormString(processStep.ApprovedBy.IdStr);
                        completedTask.EmployeeName = UI.Utils.SetFormString(processStep.ApprovedBy.Name);
                        completedTask.EntityType = UI.Utils.SetFormString(processStep.ProcessName);

                        entityName = null;
                        task = null;
                        actualDate = null;
                        targetDate = null;
                        dueDays = null;

                        switch (processStep.ProcessName)
                        {
                            case ProcessInfo.ProcessClaim: entityName = "No. " + UI.Utils.SetFormInteger(processStep.Process.Project.Claims[0].Number); break;
                            case ProcessInfo.ProcessClientVariation: entityName = UI.Utils.SetFormString(processStep.Process.Project.ClientVariations[0].Name) + " (" + UI.Utils.SetFormInteger(processStep.Process.Project.ClientVariations[0].Number) + ")"; break;
                            case ProcessInfo.ProcessSeparateAccount: entityName = "No. " + UI.Utils.SetFormInteger(processStep.Process.Project.ClientVariations[0].Number); break;
                            case ProcessInfo.ProcessComparison: entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name); break;
                            case ProcessInfo.ProcessContract: entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name); break;
                            case ProcessInfo.ProcessVariation: entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name) + ". " + UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Contract.Description); break;

                            case ProcessInfo.ProcessTransmittal:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.Transmittals[0].SubContractorName) + " - " + UI.Utils.SetFormString(processStep.Process.Project.Transmittals[0].Name);
                                actualDate = processStep.Process.Project.Transmittals[0].SentDate;
                                break;

                            case ProcessInfo.ProcessRFI:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.RFIs[0].Subject) + " (" + UI.Utils.SetFormInteger(processStep.Process.Project.RFIs[0].Number) + ") - " + processStep.Process.Project.RFIs[0].Status;
                                actualDate = processStep.Process.Project.RFIs[0].ActualAnswerDate;
                                targetDate = processStep.Process.Project.RFIs[0].TargetAnswerDate;
                                break;

                            case ProcessInfo.ProcessEOT:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.EOTs[0].Cause) + " (" + UI.Utils.SetFormInteger(processStep.Process.Project.EOTs[0].Number) + ")";
                                actualDate = processStep.Process.Project.EOTs[0].ApprovalDate;
                                break;
                        }

                        if (processStep.ProcessName != ProcessInfo.ProcessTransmittal && processStep.ProcessName != ProcessInfo.ProcessRFI && processStep.ProcessName != ProcessInfo.ProcessEOT)
                        {
                            task = processStep.Name;
                            actualDate = processStep.ActualDate;
                            targetDate = processStep.TargetDate;
                        }

                        if (targetDate != null)
                            dueDays = projectsController.NumBusinessDays(actualDate, targetDate);

                        completedTask.EntityName = entityName;
                        completedTask.Task = task;
                        completedTask.ActualDate = actualDate;
                        completedTask.DueDays = dueDays;

                        completedTasksList.Add(completedTask);
                    }
                }
            }

            #endregion Completed Tasklist
                
                //-------------Pending Task List--------------------------
                #region Pending Task List
              

            foreach (ProjectInfo project in projectInfoList)
            {
                if (pendingProcessSteps.ContainsKey(project.Id.Value))
                {
                    foreach (ProcessStepInfo processStep in pendingProcessSteps[project.Id.Value])
                    {
                        pendingTask = new PendingTask();

                        if (project.BusinessUnit != null)
                        {
                            pendingTask.BusinessUnitIdStr = project.BusinessUnit.IdStr;
                            pendingTask.BusinessUnitName = project.BusinessUnit.Name;
                        }
                        else
                        {
                            pendingTask.BusinessUnitIdStr = "0";
                            pendingTask.BusinessUnitName = "Unknown";
                        }

                        pendingTask.ProjectIdStr = UI.Utils.SetFormString(project.IdStr);
                        pendingTask.ProjectName = UI.Utils.SetFormString(project.Name);
                        pendingTask.EmployeeIdStr = UI.Utils.SetFormString(processStep.AssignedTo.IdStr);
                        pendingTask.EmployeeName = UI.Utils.SetFormString(processStep.AssignedTo.Name);
                        pendingTask.EntityType = UI.Utils.SetFormString(processStep.ProcessName);

                        entityName = null;
                        task = null;
                        targetDate = null;
                        dueDays = null;

                        switch (processStep.ProcessName)
                        {
                            case ProcessInfo.ProcessClaim: entityName = "No. " + UI.Utils.SetFormInteger(processStep.Process.Project.Claims[0].Number); break;
                            case ProcessInfo.ProcessClientVariation: entityName = UI.Utils.SetFormString(processStep.Process.Project.ClientVariations[0].Name) + " (" + UI.Utils.SetFormInteger(processStep.Process.Project.ClientVariations[0].Number) + ")"; break;
                            case ProcessInfo.ProcessSeparateAccount: entityName = "No. " + UI.Utils.SetFormInteger(processStep.Process.Project.ClientVariations[0].Number); break;
                            case ProcessInfo.ProcessComparison: entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name); break;
                            case ProcessInfo.ProcessContract: entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name); break;
                            case ProcessInfo.ProcessVariation: entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name) + ". " + UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Contract.Description); break;

                            case ProcessInfo.ProcessTransmittal:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.Transmittals[0].SubContractorName) + " - " + UI.Utils.SetFormString(processStep.Process.Project.Transmittals[0].Name);
                                targetDate = processStep.Process.Project.Transmittals[0].TransmissionDate;
                                break;

                            case ProcessInfo.ProcessRFI:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.RFIs[0].Subject) + " (" + UI.Utils.SetFormInteger(processStep.Process.Project.RFIs[0].Number) + ") - " + processStep.Process.Project.RFIs[0].Status;
                                targetDate = processStep.Process.Project.RFIs[0].TargetAnswerDate;
                                break;

                            case ProcessInfo.ProcessEOT:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.EOTs[0].Cause) + " (" + UI.Utils.SetFormInteger(processStep.Process.Project.EOTs[0].Number) + ")";
                                targetDate = processStep.Process.Project.EOTs[0].FirstNoticeDate;
                                break;

                            case ProcessInfo.ProcessCreateClaim:
                                entityName = "New Claim";
                                targetDate = DateTime.Now;
                                break;

                            case ProcessInfo.ProcessParticipation:
                                entityName = UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Name) + " - " + UI.Utils.SetFormString(processStep.Process.Project.Trades[0].Participations[0].SubContractor.ShortName);
                                targetDate = processStep.Process.Project.Trades[0].Participations[0].QuoteDate;
                                break;
                        }

                        if (processStep.ProcessName != ProcessInfo.ProcessTransmittal && processStep.ProcessName != ProcessInfo.ProcessRFI && processStep.ProcessName != ProcessInfo.ProcessEOT && processStep.ProcessName != ProcessInfo.ProcessCreateClaim && processStep.ProcessName != ProcessInfo.ProcessParticipation)
                        {
                            task = processStep.Name;
                            targetDate = processStep.TargetDate;
                            dueDays = projectsController.NumBusinessDays(targetDate);
                        }

                        pendingTask.EntityName = entityName;
                        pendingTask.Task = task;
                        pendingTask.TargetDate = targetDate;
                        pendingTask.DueDays = dueDays;

                        pendingTasksList.Add(pendingTask);
                    }
                }
            }


            #endregion Pending Tasklist
            }
            reportParameterList.Add(new ReportParameter("FilterInfo", sosFilterSelector.FilterInfo, false));

            repActivitySummary.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\ActivitySummary.rdlc";
            repActivitySummary.LocalReport.SetParameters(reportParameterList);
            repActivitySummary.LocalReport.DataSources.Clear();
            //#--
            taskTotalList = taskTotalList.FindAll(x => x.EmployeeLastLogin >= DateTime.Now.AddYears(-1));
            taskTotalList = taskTotalList.OrderBy(x=>x.EmployeeName).ToList(); //taskTotalList.Sort((x, y) => x.EmployeeName.CompareTo(y.EmployeeName));
            //#--
            repActivitySummary.LocalReport.DataSources.Add(new ReportDataSource("SOS_Reports_TaskTotal", taskTotalList));
            repActivitySummary.LocalReport.DataSources.Add(new ReportDataSource("SOS_Reports_CompletedTask", completedTasksList));
            repActivitySummary.LocalReport.DataSources.Add(new ReportDataSource("SOS_Reports_PendingTask", pendingTasksList));
           
            repActivitySummary.DataBind();
            repActivitySummary.Visible = true;
        }

     

        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewBudgetReports);

                if (!Page.IsPostBack)
                    repActivitySummary.Visible = false;

                sosFilterSelector.ActionControl = cmdGenerateReport;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                sosFilterSelector.ReBindLists();
                BindReport();
               

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
             Response.Redirect("~/Modules/Core/ListReports.aspx");
        }
#endregion

    }
}