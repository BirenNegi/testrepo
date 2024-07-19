using System;
using System.Web;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListSubContractsPage : SOSPage
    {

#region Members
        private ContractInfo contractInfo;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (contractInfo == null)
                return null;

            tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?ContractId=" + contractInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = contractInfo.Trade.Name;
            tempNode.ParentNode.ParentNode.Url = tempNode.ParentNode.ParentNode.Url + "?TradeId=" + contractInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = contractInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url = tempNode.ParentNode.ParentNode.ParentNode.Url + "?ProjectId=" + contractInfo.Trade.Project.IdStr;

            return currentNode;
        }

        private void BindContract()
        {
            lnkAddNew.NavigateUrl = "~/Modules/Contracts/EditSubContract.aspx?ContractId=" + contractInfo.IdStr;
            TitleBar1.Info = contractInfo.Trade.SelectedSubContractorName;

            gvSubContracts.DataSource = contractInfo.Subcontracts;
            gvSubContracts.DataBind();
        }

        protected String InfoStatus(ContractInfo subContract)
        {
            ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(subContract.Process);

            if (processStepInfo != null)
                return processStepInfo.Name;
            else
                return String.Empty;
        }

        protected String DateStatus(ContractInfo subContract)
        {
            ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(subContract.Process);

            if (processStepInfo != null)
                return UI.Utils.SetFormDate(processStepInfo.ActualDate);
            else
                return String.Empty;
        }

        protected String LinkContract(ContractInfo subContract)
        {
            if (subContract.IsApproved)
                return "~/Modules/Contracts/ShowContract.aspx?ContractId=" + subContract.IdStr;
            else
                return null;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterContractId;
            
            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterContractId = Utils.CheckParameter("ContractId");
                contractInfo = contractsController.GetContractWithSubContractsAndVariations(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");
                contractInfo.Trade = tradesController.GetTradeWithParticipations(contractInfo.Trade.Id);

                if (Security.ViewAccess(Security.userActions.EditContract))
                    phAddNew.Visible = true;
                
                if (!Page.IsPostBack)
                    BindContract();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
       
    }
}