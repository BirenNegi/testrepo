using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ProcessInfo = SOS.Core.ProcessInfo;
using Microsoft.Reporting.WebForms;
using System.Data;
using SOS.Core;
using SOS.Reports;


namespace SOS.Web
{
    public partial class ReportKPIAnalysis : System.Web.UI.Page
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

          
            Dictionary<int, List<EmployeeInfo>> selectedUsers = new Dictionary<int, List<EmployeeInfo>>();

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
           
            List<ProjectInfo> projectInfoList = sosFilterSelector.Projects;
          

            foreach (ProjectInfo project in projectInfoList)
            {
                projectsController.GetProjectPeopleNames(project);
                selectedUsers.Add(project.Id.Value, sosFilterSelector.Employees);
            }


            DataTable Dt = projectsController.GetKPIAnalysis(projectInfoList, selectedUsers, sosFilterSelector.StartDate, sosFilterSelector.EndDate);

            if (Dt.Rows.Count > 0)
            {
                               
                reportParameterList.Add(new ReportParameter("FilterInfo", sosFilterSelector.FilterInfo, false));

                RepKPIAnalysis.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\KPIAnalysisNew.rdlc";
                RepKPIAnalysis.LocalReport.SetParameters(reportParameterList);
                RepKPIAnalysis.LocalReport.DataSources.Clear();
                RepKPIAnalysis.LocalReport.DataSources.Add(new ReportDataSource("KPINew", Dt));

                RepKPIAnalysis.DataBind();
                RepKPIAnalysis.Visible = true;
            }
            else
            {
                RepKPIAnalysis.Visible = false;
            }


        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewAdminReports);

                if (!Page.IsPostBack)
                    RepKPIAnalysis.Visible = false;

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