using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportEOTsPage : System.Web.UI.Page
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
            ddlProjects.Items.Add(new ListItem(String.Empty, String.Empty));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
        }

        private void BindForm()
        {
            BindProjectsList();
        }

        private void BindReport()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            ProjectInfo projectInfo;
            String projectId;

            if (ddlProjects.SelectedValue != String.Empty)
            {
                projectId = ddlProjects.SelectedValue;
                projectInfo = projectsController.GetProjectWithEOTs(Int32.Parse(projectId));

                if (projectInfo != null)
                {
                    reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(projectInfo.Name)));
                    reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(projectInfo.FullNumber)));

                    repEOTs.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\EOTs.rdlc";
                    repEOTs.LocalReport.SetParameters(reportParameters);
                    repEOTs.LocalReport.DataSources.Clear();

                    if (projectInfo.EOTs != null)
                        repEOTs.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_EOTInfo", projectInfo.EOTs));

                    repEOTs.DataBind();
                    repEOTs.Visible = true;
                }
            }
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
                    repEOTs.Visible = false;
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
