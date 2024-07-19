using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowInvitationPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            TradeParticipationInfo tradeParticipationInfo = null;
            String parameterParticipationId;
            String template = tradesController.GetInvitationTemplate(1).Template;
            Byte[] pdfData = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipation);
                parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));                
                tradeParticipationInfo.Rank = 1;
                tradeParticipationInfo.PulledOut = false;                
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");
                pdfData = UI.Utils.HtmlToPDF(contractsController.MergeTemplatePrint(tradeParticipationInfo.Trade, template), contractsController.GetTemplateFooterText(template));
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfData, "Invitation");
        }
#endregion
        
    }
}
