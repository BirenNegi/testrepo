using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;

namespace SOS.Web
{
    public partial class ReportBidsChartPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindForm()
        {
            List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlTrades.Items.Add(new ListItem("", String.Empty));
            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                ddlTrades.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName, tradeTemplateInfo.IdStr));

            ddlBusinessUnit.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));




            ddlTradeParticipationType.Items.Add(new ListItem("Projects", Info.TypeActive));
            ddlTradeParticipationType.Items.Add(new ListItem("Proposals", Info.TypeProposal));
        }

        private void BindReport()
        {
            String tradeTemplateId = ddlTrades.SelectedValue;
            String businessUnitId = ddlBusinessUnit.SelectedValue;
            String tradeParticipationType = ddlTradeParticipationType.SelectedValue;
            //#--
            DateTime? FromDate = sdrStartDate.Date;     //#--

            TradeTemplateInfo tradeTemplateInfo = null;
            BusinessUnitInfo businessUnitInfo = null;
            List<BidsChart> bidsChartList = new List<BidsChart>();
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            List<ReportParameter> reportParameters = new List<ReportParameter>();



            if (tradeTemplateId != String.Empty && businessUnitId != String.Empty)
            {
                tradeTemplateInfo = tradesController.GetTradeTemplate(Int32.Parse(tradeTemplateId));
                businessUnitInfo = contractsController.GetBusinessUnit(Int32.Parse(businessUnitId));

                Core.Utils.CheckNullObject(tradeTemplateInfo, tradeTemplateId, "Trade Template");
                Core.Utils.CheckNullObject(businessUnitInfo, businessUnitId, "Business Unit");

                //#--bidsChartList = tradesController.GetBidsChart(tradeTemplateInfo, businessUnitInfo, tradeParticipationType);
                bidsChartList = tradesController.GetBidsChart(tradeTemplateInfo, businessUnitInfo, tradeParticipationType, sdrStartDate.Date);
                //#--
                reportParameters.Add(new ReportParameter("TradeName", tradeTemplateInfo.TradeName));
                reportParameters.Add(new ReportParameter("BusinessUnitName", businessUnitInfo.Name));
                reportParameters.Add(new ReportParameter("DatesInfo", "All Years"));

                repBidsChart.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\BidsChart.rdlc";
                repBidsChart.LocalReport.SetParameters(reportParameters);
                repBidsChart.LocalReport.DataSources.Clear();
                repBidsChart.LocalReport.DataSources.Add(new ReportDataSource("BidsChart", bidsChartList));
                        
                repBidsChart.DataBind();
                repBidsChart.Visible = true;
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
                    repBidsChart.Visible = false;
                    BindForm();
                }
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
