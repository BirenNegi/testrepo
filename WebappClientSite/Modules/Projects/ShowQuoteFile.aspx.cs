using System;
using System.IO;
using System.ServiceModel;

using SOS.Core;

using Client = SOS.FileTransferService.Client;

namespace SOS.Web
{
    public partial class ShowQuoteFilePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradeParticipationInfo tradeParticipationInfo = null;
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            Byte[] fileData = null;
            String fileName = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipationSubContractor);
                String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Participation");
                contractsController.CheckViewCurrentUser(tradeParticipationInfo);

                fileName = UI.Utils.PathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tradeParticipationInfo.QuoteFileName);
                fileData = Client.Utils.GetFileData(fileName);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendFile(fileData, tradeParticipationInfo.QuoteFile);
        }
#endregion

    }
}
