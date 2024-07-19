using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;

namespace SOS.Web
{
    public partial class ReportProjectSnapshotPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindReport()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ReportParameter> reportParameterList = new List<ReportParameter>();

            List<ProjectInfo> projectInfoList = sosFilterSelector.Projects;

            projectsController.InitializeHolidays();

            foreach (ProjectInfo project in projectInfoList)
            {
                projectsController.GetProjectPeopleNames(project);
                //ToDo: selectedUsers.Add(project.Id.Value, sosFilterSelector.Employees);
            }

            foreach (ProjectInfo project in projectInfoList)
            {




            }



            repProjectSnapshot.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\.rdlc"; ;
            repProjectSnapshot.LocalReport.SetParameters(reportParameterList);
            repProjectSnapshot.LocalReport.DataSources.Clear();

            repProjectSnapshot.DataBind();
            repProjectSnapshot.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewBudgetReports);

                if (!Page.IsPostBack)
                    repProjectSnapshot.Visible = false;

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
