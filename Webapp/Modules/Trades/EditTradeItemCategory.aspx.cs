using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditTradeItemCategoryPage : SOSPage
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

        private void ObjectsToForm()
        {
            if (tradeItemCategoryInfo.Id == null)
            {
                TitleBar.Title = "Adding Trade Item Category";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Trade Item Category";
            }

            txtName.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.Name);
            txtShortDescription.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.ShortDescription);
            txtLongDescription.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.LongDescription);
        }

        private void FormToObjects()
        {
            tradeItemCategoryInfo.Name = UI.Utils.GetFormString(txtName.Text);
            tradeItemCategoryInfo.ShortDescription = UI.Utils.GetFormString(txtShortDescription.Text);
            tradeItemCategoryInfo.LongDescription = UI.Utils.GetFormString(txtLongDescription.Text);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterTradeItemCategoryId = Request.Params["TradeItemCategoryId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditTradeTemplate);
                
                if (parameterTradeItemCategoryId == null)
                {
                    tradeItemCategoryInfo = new TradeItemCategoryInfo();
                    tradeItemCategoryInfo.Trade = TradesController.GetInstance().GetTrade((int?)Utils.CheckParameterInt32("TradeId"));
                }
                else
                {
                    tradeItemCategoryInfo = TradesController.GetInstance().GetTradeItemCategory(Int32.Parse(parameterTradeItemCategoryId));
                    Core.Utils.CheckNullObject(tradeItemCategoryInfo, parameterTradeItemCategoryId, "Trade Item Category");
                }

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

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    tradeItemCategoryInfo.Id = TradesController.GetInstance().AddUpdateTradeItemCategory(tradeItemCategoryInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Trades/ViewTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (tradeItemCategoryInfo.Id == null)
                Response.Redirect("~/Modules/Trades/ViewTradeTemplate.aspx?TradeTemplateId=" + tradeItemCategoryInfo.Trade.TradeTemplate.IdStr);
            else
                Response.Redirect("~/Modules/Trades/ViewTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr);
        }
#endregion

    }
}
