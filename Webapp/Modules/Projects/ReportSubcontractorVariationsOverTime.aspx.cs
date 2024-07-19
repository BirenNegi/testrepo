using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using SOS.Reports;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SOS.Web
{
    public partial class ReportSubcontractorVariationsOverTime : System.Web.UI.Page
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
            ddlProjects.Items.Add(new ListItem("All", String.Empty));
            ddlProjects.Items.Add(new ListItem("All Active", "Active"));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));

            ddlProjects.SelectedValue = "Active";
        }

        private void BindForm()
        {
            List<SubContractorInfo> subContractorInfoList = SubContractorsController.GetInstance().ListSubContractors();

            ddlSubbies.Items.Add(new ListItem("All", String.Empty));
            foreach (SubContractorInfo subContractorInfo in subContractorInfoList)
                ddlSubbies.Items.Add(new ListItem(subContractorInfo.ShortName, subContractorInfo.IdStr));


            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));

            if (businessUnitInfoList != null)
                foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                    ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));



            BindProjectsList();

        }
        private void BindReport()
        {

            string Project = ddlProjects.SelectedItem.Value;
            string BusinessUnit = ddlBusinessUnit.SelectedItem.Value;
            string Subcontractor = ddlSubbies.SelectedItem.Value;

            SubContractorsController SubController = SubContractorsController.GetInstance();
            DataTable dt = new DataTable();

            dt = SubController.GetSubContractorsVariations(Project, BusinessUnit, Subcontractor);

            if (dt.Rows.Count > 0)
            {
                repVariationsOverTime.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\SubcontractorVariationsOverTime.rdlc";

                if (repVariationsOverTime.LocalReport.DataSources.Count > 0)
                {
                    repVariationsOverTime.LocalReport.DataSources.Clear();

                }

                repVariationsOverTime.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                repVariationsOverTime.DataBind();
                repVariationsOverTime.Visible = true;

            }
            else { repVariationsOverTime.Visible = false; }

        
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewAdminReports);

                if (!Page.IsPostBack)
                    repVariationsOverTime.Visible = false;

                BindForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        
        protected void chkAll_CheckedChanged(object sender, EventArgs e)    // DS20230729
        {
        }
            protected void cmdGenerateReport_Click(object sender, EventArgs e)
            {
                try
                {
              //  sosFilterSelector.ReBindLists();
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