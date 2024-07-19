using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportClaimsPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();

            if (chkAll.Checked)
                projectInfoList = projectsController.ListProjects();
            else
                projectInfoList = projectsController.ListActiveProjects();

            ddlProjects.Items.Clear();
            ddlProjects.Items.Add(new ListItem("All Active", "Active"));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));

            ddlProjects.SelectedValue = "Active";
        }

        private void BindForm()
        {
            BindProjectsList();
        }

        private void BindReport()
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ClaimInfo> claimInfoList = null;
            ProjectInfo projectInfo = null;
            String filterInfo = null;
            ClaimInfo previousClaim;

            repClaims.Reset();

            if (ddlProjects.SelectedValue == "Active")
            {
                List<ProjectInfo> projectInfoList = projectsController.ListActiveProjects();

                if (projectInfoList != null)
                    claimInfoList = new List<ClaimInfo>();

                    foreach (ProjectInfo project in projectInfoList)
                    {
                        projectInfo = projectsController.GetProjectWithClaimsDeep(project.Id);

                        if (projectInfo.Claims != null)
                        {
                            previousClaim = null;

                            foreach (ClaimInfo claimInfo in projectInfo.Claims)
                            {
                                claimInfo.Project = projectInfo;
                                claimInfo.PreviousClaim = previousClaim;
                                claimInfoList.Add(claimInfo);

                                previousClaim = claimInfo;
                            }
                        }
                    }

                filterInfo = "Active projects.";
            }
            else
            {
                projectInfo = projectsController.GetProjectWithClaimsDeep(Int32.Parse(ddlProjects.SelectedValue));
                claimInfoList = projectInfo.Claims;

                if (claimInfoList != null)
                {
                    previousClaim = null;

                    foreach (ClaimInfo claimInfo in claimInfoList)
                    {
                        claimInfo.Project = projectInfo;
                        claimInfo.PreviousClaim = previousClaim;

                        previousClaim = claimInfo;
                    }
                }

                filterInfo = "Project: " + ddlProjects.SelectedItem.Text + ".";
            }

            reportParameters.Add(new ReportParameter("FilterInfo", filterInfo, false));

            repClaims.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\Claims.rdlc"; ;
            repClaims.LocalReport.SetParameters(reportParameters);
            repClaims.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClaimInfo", claimInfoList));

            repClaims.DataBind();
            repClaims.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (!Page.IsPostBack)
                {
                    repClaims.Visible = false;
                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            BindProjectsList();
        }

        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
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
