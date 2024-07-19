using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowContractQuotesFilePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ContractInfo contractInfo;
            String parameterContractId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterContractId = Utils.CheckParameter("ContractId");
                contractInfo = contractsController.GetContract(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");
                Utils.SendFile(contractInfo.Trade.Project.AttachmentsFolder, contractInfo.QuotesFile);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
