using System;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class CreateContractPage : SOSPage
    {

#region Members
        private TradeInfo tradeInfo = null;
        private ProcessStepInfo processStepInfo = null;
        private ContractTemplateInfo contractTemplateInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeInfo == null)
                return null;

            tempNode.ParentNode.Title = tradeInfo.Name;
            tempNode.ParentNode.Url += "?TradeId=" + tradeInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeInfo.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeInfo.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
        }

        private void FormToObjects()
        {
            tradeInfo.Contract.CheckQuotes = chkQuotes.Checked;
            tradeInfo.Contract.CheckWinningQuote = chkWinningQuote.Checked;
            tradeInfo.Contract.CheckComparison = chkComparison.Checked;
            tradeInfo.Contract.CheckCheckList = chkCheckList.Checked;
            tradeInfo.Contract.CheckPrelettingMinutes = chkPrelettingMinutes.Checked;
            tradeInfo.Contract.CheckAmendments = chkAmendments.Checked;
            tradeInfo.Contract.CheckRetentionReq = chkRetentionReq.Checked;              // DS20231108
            tradeInfo.Contract.Comments = UI.Utils.GetFormString(txtComments.Text);

            if (contractTemplateInfo != null)
                if (ddlTemplate.SelectedValue == "Standard")
                    tradeInfo.Contract.Template = contractTemplateInfo.StandardTemplate;
                else
                    tradeInfo.Contract.Template = contractTemplateInfo.SimplifiedTemplate;
            else
                tradeInfo.Contract.Template = ContractsController.TagEditableOpen + "NoTemplate " + ContractsController.TagTitle + "No Template" + ContractsController.TagEditableEnd + "No template defined." + ContractsController.TagEditableClose;
        }
#endregion        

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            String parameterTradeId;
            String parameterProcessStepId;

            try
            {
                Security.CheckAccess(Security.userActions.EditContract);
                
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetDeepTradeActive(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");

                parameterProcessStepId = Utils.CheckParameter("ProcessStepId");
                processStepInfo = tradeInfo.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == Int32.Parse(parameterProcessStepId); });
                Core.Utils.CheckNullObject(processStepInfo, parameterProcessStepId, "Process step");

                tradeInfo.Project = projectsController.GetProjectWithDrawingsActive(tradeInfo.Project.Id);
                tradeInfo.Project.Trades = new List<TradeInfo>();
                tradeInfo.Project.Trades.Add(tradeInfo);
                tradeInfo.Process.Project = tradeInfo.Project;

                tradeInfo.Contract = new ContractInfo();
                tradeInfo.Contract.Trade = tradeInfo;
                tradeInfo.Contract.WriteDate = DateTime.Today;
                tradeInfo.Contract.GoodsServicesTax = Decimal.Parse(Web.Utils.GetConfigListItemValue("Global", "Settings", "GST"));
                contractTemplateInfo = contractsController.GetContractTemplate(tradeInfo);

                processController.CheckEditCurrentUser(tradeInfo.Process);

                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void valCheckList_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (chkQuotes.Checked && chkWinningQuote.Checked && chkComparison.Checked && chkCheckList.Checked && chkPrelettingMinutes.Checked && chkAmendments.Checked) || (UI.Utils.GetFormString(txtComments.Text) != null);
        }
        
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();

                    XmlDocument xmlDocument = ContractsController.GetInstance().CheckTradeForTemplate(tradeInfo, tradeInfo.Contract.Template);
                    if (xmlDocument.DocumentElement != null)
                    {
                        TreeView1.Nodes.Clear();
                        TreeView1.Nodes.Add(new TreeNode());
                        Utils.AddNode(xmlDocument.DocumentElement, TreeView1.Nodes[0]);
                        pnlErrors.Visible = true;
                        
                        return;
                    }

                    ContractsController.GetInstance().BuildContract(tradeInfo);
                    ProcessController.GetInstance().ExecuteProcessStep(processStepInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeInfo.IdStr);
        }
#endregion

    }
}
