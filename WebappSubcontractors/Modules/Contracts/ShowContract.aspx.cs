using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowContractPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            ContractInfo contractInfo = null;
            String parameterContractId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterContractId = Utils.CheckParameterWithSection("ContractId");
                contractInfo = contractsController.GetContractWithModifications(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");
                contractInfo.Trade = tradesController.GetTradeWithParticipations(contractInfo.Trade.Id);
                contractsController.CheckViewCurrentUser(contractInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(UI.Utils.HtmlToPDF(contractsController.BuildContractWithModifications(contractInfo), contractsController.GetTemplateFooterText(contractInfo.Template)), "Contract");
        }
#endregion
        
    }
}