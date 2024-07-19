using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SOS.Core;
using Microsoft.Reporting.WebForms;
using SOS.Reports;
using System.Data;



namespace SOS.Web
{
    public partial class Report_Contracts_MissingSignedContractFiles : System.Web.UI.Page
    {

        #region private methods

        private void BindForm()
        {
            BindBusinessUnit();
            BindProjectsList();       
           
        }

        private void BindBusinessUnit()
        {

            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            
            ddlBusinessUnit.Items.Add(new ListItem(String.Empty, String.Empty));

            if (businessUnitInfoList != null)
                foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                    ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

        }

        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
             ProjectsController projectsController = ProjectsController.GetInstance();

            //////if (chkAll.Checked)
            //////    projectInfoList = projectsController.ListProjects();
            //////else

            projectInfoList = projectsController.ListActiveProjects();

            DdlProjectName.Items.Clear();
            DdlProjectName.Items.Add(new ListItem(String.Empty, String.Empty));

            if (ddlBusinessUnit.SelectedValue != String.Empty)
                projectInfoList = projectInfoList.FindAll(x => x.BusinessUnitIdStr == ddlBusinessUnit.SelectedValue);

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    DdlProjectName.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
        }


        private void BindReport()
        {
            
            ProjectsController pjcontroller = ProjectsController.GetInstance();
            DataTable dt= new DataTable();
            dt = pjcontroller.GetContractsList_MissingSignedContractFile(DdlProjectName.SelectedValue, ddlBusinessUnit.SelectedItem.Text);
            if (dt.Rows.Count > 0)
            {
                RVContracts.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\SignedContractFile.rdlc";

                if (RVContracts.LocalReport.DataSources.Count > 0)
                {
                    RVContracts.LocalReport.DataSources.Clear();

                }

                RVContracts.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                RVContracts.DataBind();
                RVContracts.Visible = true;

            }
            else { RVContracts.Visible = false; }
            


        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            BindForm();
        }

        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            BindReport();
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Core/ListReports.aspx");
        }
    }
}