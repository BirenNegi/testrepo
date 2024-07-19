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
    public partial class ReportPendingTasksPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindReport()
        {
            List<ReportParameter> reportParameterList = new List<ReportParameter>();

            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps;
            Dictionary<int, List<EmployeeInfo>> selectedUsers = new Dictionary<int, List<EmployeeInfo>>();

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            List<PendingTask> pendingTasksList = new List<PendingTask>();

            PendingTask pendingTask;
            String entityName;
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

            pendingProcessSteps = processController.GetPendingSteps(projectInfoList, selectedUsers, sosFilterSelector.StartDate, sosFilterSelector.EndDate, sosFilterSelector.OnlyNextStep);

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

            reportParameterList.Add(new ReportParameter("FilterInfo", sosFilterSelector.FilterInfo, false));

            repPendingTasks.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\PendingTasks.rdlc";
            repPendingTasks.LocalReport.SetParameters(reportParameterList);
            repPendingTasks.LocalReport.DataSources.Clear();
            repPendingTasks.LocalReport.DataSources.Add(new ReportDataSource("SOS_Reports_PendingTask", pendingTasksList));

            repPendingTasks.DataBind();
            repPendingTasks.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewBudgetReports);

                if (!Page.IsPostBack)
                    repPendingTasks.Visible = false;

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
