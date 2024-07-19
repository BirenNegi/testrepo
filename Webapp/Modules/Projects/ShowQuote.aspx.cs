using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowQuotePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradeParticipationInfo tradeParticipationInfo = null;
            TradesController tradesController = TradesController.GetInstance();
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipation);
                String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetDeepTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");

                pdfReport = tradesController.GenerateQuoteReport(tradeParticipationInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, String.Format("Quote_{0}_{1}_{2}.pdf", tradeParticipationInfo.ProjectName, tradeParticipationInfo.TradeName, tradeParticipationInfo.SubcontractorShortName));
        }
#endregion

    }
}
