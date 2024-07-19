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
    public partial class ReportTrade : System.Web.UI.Page
    {

        #region Private methods
        private void BindForm()
        {

            List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();

            tradeTemplateInfoList.Sort((x, y) => x.TradeCode.CompareTo(y.TradeCode));

            //DdlTradeCode.Items.Add(new ListItem(String.Empty, String.Empty));
            //ddlTradeName.Items.Add(new ListItem(String.Empty, String.Empty));

            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
            {
                DdlTradeCode.Items.Add(new ListItem(tradeTemplateInfo.TradeCode, tradeTemplateInfo.TradeCode));
                ddlTradeName.Items.Add(new ListItem(tradeTemplateInfo.TradeName, tradeTemplateInfo.TradeCode));
            }
        }


        private void BindReport()
        {
            
            
            string TradeCode = DdlTradeCode.SelectedItem.Value;
            TradesController tradeController = TradesController.GetInstance();
            DataTable dt = new DataTable();
            dt = tradeController.GetTrade_TradeItemCategories_TradeItem(TradeCode);

            if (dt.Rows.Count > 0)
            {
                RvTrade.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\Trades.rdlc";

                if (RvTrade.LocalReport.DataSources.Count > 0)
                {
                    RvTrade.LocalReport.DataSources.Clear();

                }

                RvTrade.LocalReport.DataSources.Add(new ReportDataSource("TradeDataSet", dt));
                RvTrade.DataBind();
                RvTrade.Visible = true;

            }
            else { RvTrade.Visible = false; }


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
                  RvTrade.Visible = false;
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

        

        

        protected void DdlTradeCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTradeName.SelectedValue = DdlTradeCode.SelectedItem.Value;
        }

        protected void ddlTradeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DdlTradeCode.SelectedValue = ddlTradeName.SelectedItem.Value;
        }
        
        
        
        #endregion



    }
}