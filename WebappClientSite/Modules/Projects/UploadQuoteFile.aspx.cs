using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

using Client = SOS.FileTransferService.Client;

namespace SOS.Web
{
    public partial class UploadQuoteFilePage : SOSPage
    {

#region Private Constants
        private const int MaxFileSize = 4194304;
#endregion

#region Members
        private TradeParticipationInfo tradeParticipationInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeParticipationInfo == null)
                return null;

            tempNode.ParentNode.Title = tradeParticipationInfo.ProjectName + " (" + tradeParticipationInfo.TradeName + ")";
            tempNode.ParentNode.Url += "?ParticipationId=" + tradeParticipationInfo.IdStr;

            return currentNode;
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipationSubContractor);
                String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Participation");
                contractsController.CheckViewCurrentUser(tradeParticipationInfo);

                if (!tradeParticipationInfo.IsEditable)
                    throw new Exception("The Trade Participation is not editable");
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ServerValidation(object source, ServerValidateEventArgs args)
        {
            int fileLength = fileQuote.PostedFile.ContentLength;

            if (fileLength == 0)
            {
                args.IsValid = false;
                valQuotefile.ErrorMessage = "The file can't be empty.";
            }
            else if (fileLength > MaxFileSize)
            {
                args.IsValid = false;
                valQuotefile.ErrorMessage = "The maximum file size is: " + UI.Utils.SetFormFileSize(MaxFileSize);
            }
            else
            {
                args.IsValid = true;
                valQuotefile.ErrorMessage = String.Empty;
            }
        }

        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            byte[] fileBytes;
            int fileLength;
            int numBytes = 0;

            if (Page.IsValid)
            {
                try
                {
                    fileLength = fileQuote.PostedFile.ContentLength;
                    fileBytes = new byte[fileLength];
                    numBytes = fileQuote.PostedFile.InputStream.Read(fileBytes, 0, fileQuote.PostedFile.ContentLength);

                    if (numBytes == fileLength)
                    {
                        tradeParticipationInfo.QuoteFile = UI.Utils.FileName(fileQuote.PostedFile.FileName);
                        tradesController.UpdateTradeParticipationQuoteFile(tradeParticipationInfo);
                        Client.Utils.PutFile(UI.Utils.PathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tradeParticipationInfo.QuoteFileName), null, fileBytes);
                    }
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }

                Response.Redirect(String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", tradeParticipationInfo.IdStr));
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", tradeParticipationInfo.IdStr));
        }
#endregion

    }
}
