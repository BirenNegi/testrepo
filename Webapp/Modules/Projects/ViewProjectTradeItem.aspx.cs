using System;
using System.Web;
using System.Xml;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewProjectTradeItemPage : SOSPage
    {

#region Members
        private TradeItemInfo tradeItemInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeItemInfo == null)
                return null;

            tempNode.Title = tradeItemInfo.Name;

            tempNode.ParentNode.Title = tradeItemInfo.TradeItemCategory.Name;
            tempNode.ParentNode.Url += "?TradeItemCategoryId=" + tradeItemInfo.TradeItemCategory.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeItemInfo.TradeItemCategory.Trade.Name;
            tempNode.ParentNode.ParentNode.Url += "?TradeId=" + tradeItemInfo.TradeItemCategory.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = tradeItemInfo.TradeItemCategory.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeItemInfo.TradeItemCategory.Trade.Project.IdStr;
            
            return currentNode;
        }

        private void BindTradeItem()
        {
            lblName.Text = UI.Utils.SetFormString(tradeItemInfo.Name);
            lblUnits.Text = UI.Utils.SetFormString(tradeItemInfo.Units);
            sbvRequiresQuantityCheck.Checked = tradeItemInfo.RequiresQuantityCheck;
            sbvRequiredInProposal.Checked = tradeItemInfo.RequiredInProposal;
            txtScope.Text = UI.Utils.SetFormString(tradeItemInfo.Scope);
        }

        private void BindDeleteErrorsTree(XmlDocument xmlDocument, Boolean deleteError)
        {
            if (deleteError)
            {
                TreeViewDeleteErrors.Nodes.Clear();
                TreeViewDeleteErrors.Nodes.Add(new TreeNode());
                Utils.AddNode(xmlDocument.DocumentElement, TreeViewDeleteErrors.Nodes[0]);
                TreeViewDeleteErrors.ExpandAll();
                pnlDeleteErrors.Visible = true;
            }
            else
                pnlDeleteErrors.Visible = false;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeItemId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                parameterTradeItemId = Utils.CheckParameter("TradeItemId");
                tradeItemInfo = tradesController.GetTradeItem(Int32.Parse(parameterTradeItemId));
                Core.Utils.CheckNullObject(tradeItemInfo, parameterTradeItemId, "Trade Item");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTrade))
                    {
                        if (processController.AllowEditCurrentUser(tradeItemInfo.TradeItemCategory.Trade))
                        {
                            cmdEditTop.Visible = true;
                            cmdDeleteTop.Visible = true;

                            cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Item ?');");
                        }
                    }
                    BindTradeItem();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditProjectTradeItem.aspx?TradeItemId=" + tradeItemInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            Boolean deleteError = false;

            try
            {
                XmlDocument xmlDocument = tradesController.CheckTradeItemForDelete(tradeItemInfo);

                if (xmlDocument.DocumentElement != null)
                    deleteError = true;
                else
                    tradesController.DeleteTradeItem(tradeItemInfo);

                BindDeleteErrorsTree(xmlDocument, deleteError);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (!deleteError)
                Response.Redirect("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemInfo.TradeItemCategory.IdStr);
        }
#endregion

    }
}
