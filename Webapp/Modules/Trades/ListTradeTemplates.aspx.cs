using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListTradeTemplatesPage : System.Web.UI.Page
    {

#region Members
        private List<TradeTemplateInfo> tradeTemplates = null;
#endregion

#region Private Methods
        private void bindTradeTemplates()
        {
            gvTradesTemplates.DataSource = tradeTemplates;
            gvTradesTemplates.DataBind();
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListTradeTemplates);

                if (Security.ViewAccess(Security.userActions.EditTradeTemplate))
                {
                    phAddNew.Visible = true;
                    gvTradesTemplates.Columns[7].Visible = true;
                    gvTradesTemplates.Columns[8].Visible = true;
                }

                tradeTemplates = TradesController.GetInstance().GetTradeTemplates();

                if (!Page.IsPostBack)
                {
                    bindTradeTemplates();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvTradesTemplates_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            TradeInfo trade = null;
            TradesController tradesController = TradesController.GetInstance();
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();
            int? tradeTemplateId = (int?)gvTradesTemplates.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;

            foreach (TradeTemplateInfo tradeTemplate in tradeTemplates)
            {
                if (tradeTemplate.Trade != null)
                    tradeInfoList.Add(tradeTemplate.Trade);

                if (tradeTemplate.Id == tradeTemplateId)
                    trade = tradeTemplate.Trade;
            }

            if (e.CommandName == "MoveUp")
                tradesController.ChangeDisplayOrderTrade(tradeInfoList, trade, true);
            else if (e.CommandName == "MoveDown")
                tradesController.ChangeDisplayOrderTrade(tradeInfoList, trade, false);

            tradeTemplates = tradesController.GetTradeTemplates();
            bindTradeTemplates();
        }
#endregion

    }
}