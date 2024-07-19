using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewTradeItemPage : SOSPage
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
            tempNode.ParentNode.ParentNode.Url += "?TradeTemplateId=" + tradeItemInfo.TradeItemCategory.Trade.TradeTemplate.IdStr;
            
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
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterTradeItemId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTradeTemplate);
                parameterTradeItemId = Utils.CheckParameter("TradeItemId");
                tradeItemInfo = TradesController.GetInstance().GetTradeItem(Int32.Parse(parameterTradeItemId));

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTradeTemplate))
                    {
                        cmdEditTop.Visible = true;
                        cmdDeleteTop.Visible = true;

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Item ?');");
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
            Response.Redirect("~/Modules/Trades/EditTradeItem.aspx?TradeItemId=" + tradeItemInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteTradeItem(tradeItemInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Trades/ViewTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemInfo.TradeItemCategory.IdStr);
        }
#endregion

    }
}
