using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditProjectTradeItemCategoryPage : SOSPage
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
            tempNode.ParentNode.Url += "?TradeId=" + tradeItemCategoryInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeItemCategoryInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeItemCategoryInfo.Trade.Project.IdStr;

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
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeItemCategoryId = Request.Params["TradeItemCategoryId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditTrade);
                
                if (parameterTradeItemCategoryId == null)
                {
                    tradeItemCategoryInfo = new TradeItemCategoryInfo();
                    tradeItemCategoryInfo.Trade = tradesController.GetTrade((int?)Utils.CheckParameterInt32("TradeId"));
                }
                else
                {
                    tradeItemCategoryInfo = tradesController.GetTradeItemCategory(Int32.Parse(parameterTradeItemCategoryId));
                    Core.Utils.CheckNullObject(tradeItemCategoryInfo, parameterTradeItemCategoryId, "Trade Item Category");
                }

                processController.CheckEditCurrentUser(tradeItemCategoryInfo.Trade);

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
                Response.Redirect("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (tradeItemCategoryInfo.Id == null)
            {
                Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeItemCategoryInfo.Trade.IdStr);
            }
            else
            {
                Response.Redirect("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr);
            }
        }
#endregion

    }
}
