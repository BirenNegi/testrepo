using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;

using ProcessInfo = SOS.Core.ProcessInfo;

namespace SOS.Web
{
    public partial class ReportCompletedTasksPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindReport()
        {
            List<ReportParameter> reportParameterList = new List<ReportParameter>();

            Dictionary<int, List<ProcessStepInfo>> completedProcessSteps;
            Dictionary<int, List<EmployeeInfo>> selectedUsers = new Dictionary<int, List<EmployeeInfo>>();

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            List<CompletedTask> completedTasksList = new List<CompletedTask>();

            CompletedTask completedTask;
            String entityName;
            DateTime? actualDate;
            DateTime? targetDate;
            String task;
            int? dueDays;

            List<ProjectInfo> projectInfoList = sosFilterSelector.Projects;

            projectsController.InitializeHolidays();

            foreach (ProjectInfo project in projectInfoList)
            {
                projectsController.GetProjectPeopleNames(project);
                selectedUsers.Add(project.Id.Value, sosFilterSelector.Employees);
            }

            completedProcessSteps = processController.GetExecutedSteps(projectInfoList, selectedUsers, sosFilterSelector.StartDate, sosFilterSelector.EndDate);

            foreach (ProjectInfo project in projectInfoList)
            {
                if (completedProcessSteps.ContainsKey(project.Id.Value))
                {
                    foreach (ProcessStepInfo processStep in completedProcessSteps[project.Id.Value])
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

            reportParameterList.Add(new ReportParameter("FilterInfo", sosFilterSelector.FilterInfo, false));

            repCompletedTasks.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\CompletedTasks.rdlc";
            repCompletedTasks.LocalReport.SetParameters(reportParameterList);
            repCompletedTasks.LocalReport.DataSources.Clear();
            repCompletedTasks.LocalReport.DataSources.Add(new ReportDataSource("SOS_Reports_CompletedTask", completedTasksList));

            repCompletedTasks.DataBind();
            repCompletedTasks.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewAdminReports);

                if (!Page.IsPostBack)
                    repCompletedTasks.Visible = false;

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
