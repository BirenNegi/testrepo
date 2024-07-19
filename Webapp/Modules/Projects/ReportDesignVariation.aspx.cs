using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace SOS.Web
{
    public partial class ReportDesignVariation : System.Web.UI.Page
    {
        #region Private Methods
        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            BusinessUnitInfo businessUnitInfo = ddlBusinessUnit.SelectedValue != String.Empty ? new BusinessUnitInfo(UI.Utils.GetFormInteger(ddlBusinessUnit.SelectedValue)) : null;

            if (chkAll.Checked)
                projectInfoList = projectsController.ListProjects(businessUnitInfo);
            else
                projectInfoList = projectsController.ListActiveProjects(businessUnitInfo);

            ddlProjects.Items.Clear();
            ddlProjects.Items.Add(new ListItem("All", String.Empty));
            ddlProjects.Items.Add(new ListItem("All Active", "Active"));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));

            ddlProjects.SelectedValue = "Active";
        }

        private void BindSubContractorsList()
        {
            //BusinessUnitInfo businessUnitInfo = ddlBusinessUnit.SelectedValue != String.Empty ? new BusinessUnitInfo(UI.Utils.GetFormInteger(ddlBusinessUnit.SelectedValue)) : null;
            //List<SubContractorInfo> subContractorInfoList = SubContractorsController.GetInstance().ListSubContractors(businessUnitInfo);

            //ddlSubbies.Items.Clear();
            //ddlSubbies.Items.Add(new ListItem("All", String.Empty));
            //foreach (SubContractorInfo subContractorInfo in subContractorInfoList)
            //    ddlSubbies.Items.Add(new ListItem(subContractorInfo.ShortName, subContractorInfo.IdStr));
        }

        private void BindForm()
        {
            List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
            List<EmployeeInfo> employeeInfoList = PeopleController.GetInstance().GetEmployeesWithApprovalRoles();
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            ddlTrades.Items.Add(new ListItem("All", String.Empty));
            //--#   tradeTemplateInfoList.Sort();
            tradeTemplateInfoList.Sort((x, y) => x.TradeCode.CompareTo(y.TradeCode));  //--#
            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)

                ddlTrades.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName, tradeTemplateInfo.TradeCode));

            //ddlPerson.Items.Add(new ListItem("All", String.Empty));
            //foreach (EmployeeInfo employeeInfo in employeeInfoList)
            //    ddlPerson.Items.Add(new ListItem(employeeInfo.Name, employeeInfo.IdStr));

            BindProjectsList();
          //  BindSubContractorsList();
        }

        private void BindReport()
        {
            //   List<TradeInfo> tradeInfoList = TradesController.GetInstance().SearchTrades(UI.Utils.GetFormString(ddlProjects.SelectedValue), UI.Utils.GetFormString(ddlTrades.SelectedValue), UI.Utils.GetFormInteger(ddlSubbies.SelectedValue));

            List<ReportParameter> reportParameterList = new List<ReportParameter>();
            ProjectsController projectsController = ProjectsController.GetInstance();
            String filterInfo;
            String projectStatus = null;
            int? projectId = null;

            if (ddlProjects.SelectedValue == "Active")
                projectStatus = ProjectInfo.StatusActive;
            else if (ddlProjects.SelectedValue != String.Empty)
                projectId = UI.Utils.GetFormInteger(ddlProjects.SelectedValue);



            if (ddlProjects.SelectedValue == String.Empty)
                if (ddlBusinessUnit.SelectedValue == String.Empty)
                    filterInfo = "All projects.";
                else
                    filterInfo = ddlBusinessUnit.SelectedItem.Text + " projects.";
            else
                if (ddlProjects.SelectedValue == "Active")
                if (ddlBusinessUnit.SelectedValue == String.Empty)
                    filterInfo = "Active projects.";
                else
                    filterInfo = ddlBusinessUnit.SelectedItem.Text + " active projects.";
            else
                filterInfo = "Project: " + ddlProjects.SelectedItem.Text + ".";

            if (ddlTrades.SelectedValue != String.Empty)
                filterInfo = filterInfo + " " + "Trade: " + ddlTrades.SelectedItem.Text + ".";

            //if (ddlSubbies.SelectedValue != String.Empty)
            //    filterInfo = filterInfo + " " + "Subcontractor: " + ddlSubbies.SelectedItem.Text + ".";

            if (sdrStartDate.Date != null)
                filterInfo = filterInfo + " " + "From: " + sdrStartDate.Date.Value.ToString("dd/MM/yyyy") + ".";
            //if (ddlPerson.SelectedValue != String.Empty)
            //    filterInfo = filterInfo + " " + "Project Manager: " + ddlPerson.SelectedItem.Text + ".";

            reportParameterList.Add(new ReportParameter("FilterInfo", filterInfo, false));

            DataTable Dt = projectsController.GetDesignVariation(projectId, UI.Utils.GetFormInteger(ddlBusinessUnit.SelectedValue), UI.Utils.GetFormString(ddlTrades.SelectedValue), projectStatus, sdrStartDate.Date, sdrEndDate.Date);




            repDesignVariation.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\DesignVariation.rdlc";
            repDesignVariation.LocalReport.SetParameters(reportParameterList);
            repDesignVariation.LocalReport.DataSources.Clear();
            repDesignVariation.LocalReport.DataSources.Add(new ReportDataSource("DesignVariation", Dt));

            repDesignVariation.DataBind();
            repDesignVariation.Visible = true;
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
                    repDesignVariation.Visible = false;
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
            BindProjectsList();
            BindSubContractorsList();
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