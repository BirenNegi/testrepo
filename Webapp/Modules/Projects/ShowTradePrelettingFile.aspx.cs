using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowTradePrelettingFilePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            TradeInfo tradeInfo;
            String parameterTradeId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetTrade(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");
                Utils.SendFile(tradeInfo.Project.AttachmentsFolder, tradeInfo.PrelettingFile);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
