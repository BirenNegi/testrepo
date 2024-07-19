using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewTradeItemCategoryPage : SOSPage
    {

#region Members
        private TradeItemCategoryInfo tradeItemCategoryInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeItemCategoryInfo == null)
                return null;

            tempNode.Title = tradeItemCategoryInfo.Name;
            tempNode.ParentNode.Title = tradeItemCategoryInfo.Trade.Name;
            tempNode.ParentNode.Url += "?TradeTemplateId=" + tradeItemCategoryInfo.Trade.TradeTemplate.IdStr;

            return currentNode;
        }

        private void BindTradeItems()
        {
            gvTradeItems.DataSource = tradeItemCategoryInfo.TradeItems;
            gvTradeItems.DataBind();
        }

        private void BindTradeItemCategory()
        {
            lnkAddTradeItem.NavigateUrl = "~/Modules/Trades/EditTradeItem.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr;

            lblName.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.Name);
            lblShortDescription.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.ShortDescription);
            lblLongDescription.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.LongDescription);

            BindTradeItems();
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterTradeItemCategoryId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTradeTemplate);
                parameterTradeItemCategoryId = Utils.CheckParameter("TradeItemCategoryId");
                tradeItemCategoryInfo = TradesController.GetInstance().GetDeepTradeItemCategory(Int32.Parse(parameterTradeItemCategoryId));

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTradeTemplate))
                    {
                        cmdEditTop.Visible = true;
                        cmdDeleteTop.Visible = true;

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Item Category and all its Items?');");

                        phAddTradeItem.Visible = true;
                        gvTradeItems.Columns[5].Visible = true;
                        gvTradeItems.Columns[6].Visible = true;
                    }
                    BindTradeItemCategory();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/EditTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteTradeItemCategory(tradeItemCategoryInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Trades/ViewTradeTemplate.aspx?TradeTemplateId=" + tradeItemCategoryInfo.Trade.TradeTemplate.IdStr);
        }

        protected void gvTradeItems_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            int? tradeItemId = (int?)gvTradeItems.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
            TradeItemInfo tradeItemInfo = tradeItemCategoryInfo.TradeItems.Find(delegate(TradeItemInfo tradeItemInfoInList) { return tradeItemInfoInList.Id == tradeItemId; });

            if (e.CommandName == "MoveUp")
                tradesController.ChangeDisplayOrderTradeItem(tradeItemCategoryInfo.TradeItems, tradeItemInfo, true);
            else if (e.CommandName == "MoveDown")
                tradesController.ChangeDisplayOrderTradeItem(tradeItemCategoryInfo.TradeItems, tradeItemInfo, false);

            tradeItemCategoryInfo.TradeItems = tradesController.GetTradeItems(tradeItemCategoryInfo);
            BindTradeItems();
        }
#endregion
    
    }
}
