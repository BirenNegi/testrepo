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
    public partial class ReportSubcontractorsMissingInsurance : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (!Page.IsPostBack)
                {
                    RvSubInsurance.Visible = false;

                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

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



           // BindProjectsList();

        }


        private void BindReport()
        {

            //string Project = ddlProjects.SelectedItem.Value;
            string BusinessUnit = ddlBusinessUnit.SelectedItem.Value;
            string Subcontractor = ddlSubbies.SelectedItem.Value;

            SubContractorsController SubController = SubContractorsController.GetInstance();
            DataTable dt = new DataTable();

            dt = SubController.GetSubContractorsMissingInsuranceLink(BusinessUnit, Subcontractor);

            if (dt.Rows.Count > 0)
            {
                RvSubInsurance.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\SubcontractorMissingInsuranceLinks.rdlc";

                if (RvSubInsurance.LocalReport.DataSources.Count > 0)
                {
                    RvSubInsurance.LocalReport.DataSources.Clear();

                }

                RvSubInsurance.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                RvSubInsurance.DataBind();
                RvSubInsurance.Visible = true;

            }
            else { RvSubInsurance.Visible = false; }







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