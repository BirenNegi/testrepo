using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditProjectTradeItemPage : SOSPage
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

        private void ObjectsToForm()
        {
            if (tradeItemInfo.Id == null)
            {
                TitleBar.Title = "Adding Trade Item";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Trade Item";
            }

            txtName.Text = UI.Utils.SetFormString(tradeItemInfo.Name);
            txtUnits.Text = UI.Utils.SetFormString(tradeItemInfo.Units);
            sbrRequiresQuantityCheck.Checked = tradeItemInfo.RequiresQuantityCheck;
            sbrRequiredInProposal.Checked = tradeItemInfo.RequiredInProposal;
            txtScope.Text = UI.Utils.SetFormString(tradeItemInfo.Scope);
        }

        private void FormToObjects()
        {
            tradeItemInfo.Name = UI.Utils.GetFormString(txtName.Text);
            tradeItemInfo.Units = UI.Utils.GetFormString(txtUnits.Text);
            tradeItemInfo.RequiresQuantityCheck = sbrRequiresQuantityCheck.Checked;
            tradeItemInfo.RequiredInProposal = sbrRequiredInProposal.Checked;
            tradeItemInfo.Scope = UI.Utils.GetFormString(txtScope.Text);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeItemId = Request.Params["TradeItemId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditTrade);

                if (parameterTradeItemId == null)
                {
                    tradeItemInfo = new TradeItemInfo();
                    tradeItemInfo.TradeItemCategory = tradesController.GetTradeItemCategory((int?)Utils.CheckParameterInt32("TradeItemCategoryId"));
                }
                else
                {
                    tradeItemInfo = tradesController.GetTradeItem(Int32.Parse(parameterTradeItemId));
                    Core.Utils.CheckNullObject(tradeItemInfo, parameterTradeItemId, "Trade Item");
                }

                tradeItemInfo.TradeItemCategory.Trade.Participations = tradesController.GetTradeParticipations(tradeItemInfo.TradeItemCategory.Trade);

                processController.CheckEditCurrentUser(tradeItemInfo.TradeItemCategory.Trade);

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
                    tradeItemInfo.Id = TradesController.GetInstance().AddUpdateTradeItem(tradeItemInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewProjectTradeItem.aspx?TradeItemId=" + tradeItemInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (tradeItemInfo.Id == null)
            {
                Response.Redirect("~/Modules/Projects/ViewProjectTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemInfo.TradeItemCategory.IdStr);
            }
            else
            {
                Response.Redirect("~/Modules/Projects/ViewProjectTradeItem.aspx?TradeItemId=" + tradeItemInfo.IdStr);
            }
        }
#endregion

    }
}