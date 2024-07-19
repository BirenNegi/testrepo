using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportWorkOrdersPage : System.Web.UI.Page
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
            BindProjectsList();
        }

        private void BindReport()
        {
            String filterInfo = null;
            TradesController tradesController = TradesController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ReportParameter> reportParameterList = new List<ReportParameter>();
            Dictionary<String, ProjectInfo> projectsDictionary = new Dictionary<String, ProjectInfo>();
            List<TradeInfo> tradeInfoList = tradesController.SearchWorkOrders(UI.Utils.GetFormString(ddlProjects.SelectedValue));

            foreach (TradeInfo tradeInfo in tradeInfoList)
            {
                if (!projectsDictionary.ContainsKey(tradeInfo.Project.IdStr))
                    projectsDictionary.Add(tradeInfo.Project.IdStr, projectsController.GetProject(tradeInfo.Project.Id));

                tradeInfo.Project = projectsDictionary[tradeInfo.Project.IdStr];

                tradeInfo.TotalBudget = tradeInfo.BudgetParticipation != null ? tradesController.GetQuoteTotal(tradeInfo.BudgetParticipation) : 0;
                tradeInfo.TotalSelectedQuote = tradeInfo.SelectedParticipation != null ? tradesController.GetQuoteTotal(tradeInfo.SelectedParticipation) : 0;

                tradeInfo.TotalCompanyVariations = 0;
                tradeInfo.TotalDesignVariations = 0;
                tradeInfo.TotalSubbiesVariations = 0;
                if (tradeInfo.Contract != null)
                    foreach (ContractInfo subContract in tradeInfo.Contract.Subcontracts)
                    {
                        tradeInfo.TotalCompanyVariations = tradeInfo.TotalCompanyVariations + subContract.TotalCompanyVariations;
                        tradeInfo.TotalDesignVariations = tradeInfo.TotalDesignVariations + subContract.TotalDesignVariations;
                        tradeInfo.TotalSubbiesVariations = tradeInfo.TotalSubbiesVariations + subContract.TotalSubbiesVariations;                        
                    }
            }

            if (ddlProjects.SelectedValue == String.Empty)
                filterInfo = "All projects.";
            else
                if (ddlProjects.SelectedValue == "Active")
                    filterInfo = "Active projects.";
                else
                    filterInfo = "Project: " + ddlProjects.SelectedItem.Text + ".";

            reportParameterList.Add(new ReportParameter("FilterInfo", filterInfo, false));

            repWorkOrders.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\WorkOrders.rdlc"; ;
            repWorkOrders.LocalReport.SetParameters(reportParameterList);
            repWorkOrders.LocalReport.DataSources.Clear();
            repWorkOrders.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_TradeInfo", tradeInfoList));

            repWorkOrders.DataBind();
            repWorkOrders.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewAdminReports);

                if (!Page.IsPostBack)
                {
                    repWorkOrders.Visible = false;
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
