using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportCVsPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindForm()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem(String.Empty, String.Empty));

            if (businessUnitInfoList != null)
                foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                    ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));
        }

        private void BindReport()
        {
            if (ddlBusinessUnit.SelectedValue != String.Empty)
            {
                ProjectsController projectsController = ProjectsController.GetInstance();
                List<ProjectInfo> projectInfoList = null;
                List<ProjectInfo> projectInfoListWithCVs = new List<ProjectInfo>();
                List<ReportParameter> reportParameters = new List<ReportParameter>();

                repCVs.Reset();

                if (ddlProjects.SelectedValue == "Active")
                {
                    projectInfoList = projectsController.ListActiveProjects(new BusinessUnitInfo((int?)Int32.Parse(ddlBusinessUnit.SelectedValue)));
                    reportParameters.Add(new ReportParameter("FilterInfo", "Active projects in " + ddlBusinessUnit.SelectedItem.Text));
                }
                else
                {
                    projectInfoList = new List<ProjectInfo>();
                    projectInfoList.Add(new ProjectInfo(int.Parse(ddlProjects.SelectedValue)));
                }

                foreach (ProjectInfo projectInfo in projectInfoList)
                    projectInfoListWithCVs.Add(projectsController.GetProjectWithClientVariations(projectInfo.Id));

                repCVs.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\CVsSummary.rdlc";
                repCVs.LocalReport.SetParameters(reportParameters);
                repCVs.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ProjectInfo", projectInfoListWithCVs));
                repCVs.DataBind();
                repCVs.Visible = true;
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
                    repCVs.Visible = false;
                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            String businessUnitId = ddlBusinessUnit.SelectedValue;

            if (businessUnitId != String.Empty)
            {
                List<ProjectInfo> projectInfoList = ProjectsController.GetInstance().ListActiveProjects((new BusinessUnitInfo((int?)Int32.Parse(businessUnitId))));

                ddlProjects.Items.Clear();
                ddlProjects.Items.Add(new ListItem("All Active", "Active"));

                if (projectInfoList != null)
                    foreach (ProjectInfo projectInfo in projectInfoList)
                        ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
            }
            else
            {
                ddlProjects.Items.Clear();
            }
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
