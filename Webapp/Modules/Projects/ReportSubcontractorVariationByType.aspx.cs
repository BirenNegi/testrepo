using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Reporting.WebForms;
using SOS.Core;
using System.Data;

namespace SOS.Web
{
    public partial class ReportSubcontractorVariationByType : System.Web.UI.Page
    {
        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();

            if (chkAll.Checked)
                projectInfoList = projectsController.ListProjects();
            else

                projectInfoList = projectsController.ListActiveProjects();

            ddlProjects.Items.Clear();
            //ddlProjects.Items.Add(new ListItem("All", String.Empty));
            //ddlProjects.Items.Add(new ListItem("All Active", "Active"));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));

            //ddlProjects.SelectedValue = "Active";
        }

        private void BindForm()
        {
            ////List<SubContractorInfo> subContractorInfoList = SubContractorsController.GetInstance().ListSubContractors();

            ////ddlSubbies.Items.Add(new ListItem("All", String.Empty));
            ////foreach (SubContractorInfo subContractorInfo in subContractorInfoList)
            ////    ddlSubbies.Items.Add(new ListItem(subContractorInfo.ShortName, subContractorInfo.IdStr));


            ////List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ////ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));

            ////if (businessUnitInfoList != null)
            ////    foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
            ////        ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));



            BindProjectsList();

        }


        private void BindReport()
        {

            string Project = ddlProjects.SelectedItem.Value;
            //string BusinessUnit = ddlBusinessUnit.SelectedItem.Value;
            //string Subcontractor = ddlSubbies.SelectedItem.Value;

            SubContractorsController SubController = SubContractorsController.GetInstance();
            DataTable dt = new DataTable();

            dt = SubController.GetSubContractorsVariationsByType(Project);

            if (dt.Rows.Count > 0)
            {
                RvSubVar.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\SubcontractorVariationByType.rdlc";

                if (RvSubVar.LocalReport.DataSources.Count > 0)
                {
                    RvSubVar.LocalReport.DataSources.Clear();

                }

                RvSubVar.LocalReport.DataSources.Add(new ReportDataSource("DatasetSubbiVariationByType", dt));
                RvSubVar.DataBind();
                RvSubVar.Visible = true;

            }
            else { RvSubVar.Visible = false; }







        }


        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (!Page.IsPostBack)
                {
                    RvSubVar.Visible = false;

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




    }
}