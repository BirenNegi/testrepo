using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowParticipationQuoteFilePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipation);
                String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                TradeParticipationInfo tradeParticipationInfo = TradesController.GetInstance().GetTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");

                Utils.SendFileWithName(UI.Utils.FullPathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tradeParticipationInfo.QuoteFileName), tradeParticipationInfo.QuoteFile);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
