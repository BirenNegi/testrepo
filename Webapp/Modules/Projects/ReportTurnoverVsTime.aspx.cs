using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;
using System.Data;

namespace SOS.Web
{
    public partial class ReportTurnoverVsTime : System.Web.UI.Page
    {

        #region Private Methods
        private void BindReport()
        {
            List<ReportParameter> reportParameterList = new List<ReportParameter>();

            Dictionary<int, List<ProcessStepInfo>> completedProcessSteps;
            Dictionary<int, List<EmployeeInfo>> selectedUsers = new Dictionary<int, List<EmployeeInfo>>();

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

           

          

            List<ProjectInfo> projectInfoList = sosFilterSelector.Projects;

            //projectsController.InitializeHolidays();

            foreach (ProjectInfo project in projectInfoList)
            {
                projectsController.GetProjectPeopleNames(project);
                selectedUsers.Add(project.Id.Value, sosFilterSelector.Employees);
            }

            DataTable Dt = projectsController.GetTurnoverVsTime(projectInfoList, selectedUsers, sosFilterSelector.StartDate, sosFilterSelector.EndDate);

            if (Dt.Rows.Count > 0)
            {

                reportParameterList.Add(new ReportParameter("FilterInfo", sosFilterSelector.FilterInfo, false));

                repTurnOver.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\TurnoverVsTime.rdlc";
                repTurnOver.LocalReport.SetParameters(reportParameterList);
                repTurnOver.LocalReport.DataSources.Clear();
                repTurnOver.LocalReport.DataSources.Add(new ReportDataSource("TurnoverVsTime",Dt));

                repTurnOver.DataBind();
                repTurnOver.Visible = true;
            }
            else
            {
                repTurnOver.Visible = false;
            }


        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewBudgetReports);

                if (!Page.IsPostBack)
                    repTurnOver.Visible = false;

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