using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditParticipationItemPage : SOSPage
    {

#region Members
        private ParticipationItemInfo participationItemInfo  = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (participationItemInfo == null)
                return null;

            tempNode.ParentNode.Url += "?Type=" + participationItemInfo.TradeParticipation.Type + "&TradeId=" + participationItemInfo.TradeItem.TradeItemCategory.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = participationItemInfo.TradeItem.TradeItemCategory.Trade.Name;
            tempNode.ParentNode.ParentNode.Url += "?TradeId=" + participationItemInfo.TradeItem.TradeItemCategory.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = participationItemInfo.TradeItem.TradeItemCategory.Trade.Project.Name + (participationItemInfo.TradeParticipation.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + participationItemInfo.TradeItem.TradeItemCategory.Trade.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (participationItemInfo.TradeParticipation.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            if (participationItemInfo.TradeParticipation.SubContractor != null)
                lblSubcontractor.Text = UI.Utils.SetFormString(participationItemInfo.TradeParticipation.SubContractor.Name);

            lblCategory.Text = UI.Utils.SetFormString(participationItemInfo.TradeItem.TradeItemCategory.Name);
            lblItem.Text = UI.Utils.SetFormString(participationItemInfo.TradeItem.Name);
            lblUnits.Text = UI.Utils.SetFormString(participationItemInfo.TradeItem.Units);

            txtQuantity.Text = UI.Utils.SetFormString(participationItemInfo.Quantity);
            sbrIncluded.Checked = participationItemInfo.IsIncluded;
            txtAmount.Text = UI.Utils.SetFormEditDecimal(participationItemInfo.Amount);
            chkConirmed.Checked = UI.Utils.SetFormBoolean(participationItemInfo.Confirmed);
            txtNotes.Text = UI.Utils.SetFormString(participationItemInfo.Notes);
        }

        private void FormToObjects()
        {
            participationItemInfo.Quantity = UI.Utils.GetFormString(txtQuantity.Text);
            participationItemInfo.IsIncluded = sbrIncluded.Checked;
            participationItemInfo.Amount = UI.Utils.GetFormDecimal(txtAmount.Text);
            participationItemInfo.Notes = UI.Utils.GetFormString(txtNotes.Text);

            if (participationItemInfo.Amount == null)
                participationItemInfo.Confirmed = null;
            else
                participationItemInfo.Confirmed = chkConirmed.Checked;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterParticipationItemId;

            try
            {
                Security.CheckAccess(Security.userActions.EditTrade);
                parameterParticipationItemId = Utils.CheckParameter("ParticipationItemId");
                participationItemInfo = tradesController.GetParticipationItem(Int32.Parse(parameterParticipationItemId));
                Core.Utils.CheckNullObject(participationItemInfo, parameterParticipationItemId, "Participation Item");
                participationItemInfo.TradeItem = tradesController.GetTradeItem(participationItemInfo.TradeItem.Id);
                participationItemInfo.TradeParticipation.Trade = tradesController.GetTradeWithParticipations(participationItemInfo.TradeItem.TradeItemCategory.Trade.Id);
                participationItemInfo.TradeParticipation.Trade.Project = projectsController.GetProject(participationItemInfo.TradeItem.TradeItemCategory.Trade.Project.Id);

                processController.CheckEditCurrentUser(participationItemInfo.TradeItem.TradeItemCategory.Trade);

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
                    TradesController.GetInstance().UpdateParticipationItemAndProcess(participationItemInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect(String.Format("~/Modules/Projects/ViewComparison.aspx?Type={0}&TradeId={1}", participationItemInfo.TradeParticipation.Type, participationItemInfo.TradeItem.TradeItemCategory.Trade.IdStr));
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/Modules/Projects/ViewComparison.aspx?Type={0}&TradeId={1}", participationItemInfo.TradeParticipation.Type, participationItemInfo.TradeItem.TradeItemCategory.Trade.IdStr));
        }
#endregion

    }
}