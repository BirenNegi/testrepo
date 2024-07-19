using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditParticipationItemPage : SOSPage
    {

#region Members
        private ParticipationItemInfo participationItemInfo = null;   // Quote participation item
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (participationItemInfo == null)
                return null;

            tempNode.ParentNode.Title = participationItemInfo.TradeParticipation.ComparisonParticipation.ProjectName + " (" + participationItemInfo.TradeParticipation.ComparisonParticipation.TradeName + ")";
            tempNode.ParentNode.Url += "?ParticipationId=" + participationItemInfo.TradeParticipation.ComparisonParticipation.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            lblCategory.Text = UI.Utils.SetFormString(participationItemInfo.TradeItem.TradeItemCategory.Name);
            lblItem.Text = UI.Utils.SetFormString(participationItemInfo.TradeItem.Name);
            lblUnits.Text = UI.Utils.SetFormString(participationItemInfo.TradeItem.Units);

            txtQuantity.Text = UI.Utils.SetFormString(participationItemInfo.Quantity);
            sbrIncluded.Checked = participationItemInfo.IsIncluded;
            txtAmount.Text = UI.Utils.SetFormEditDecimal(participationItemInfo.Amount);
            txtNotes.Text = UI.Utils.SetFormString(participationItemInfo.Notes);
        }

        private void FormToObjects()
        {
            participationItemInfo.Quantity = UI.Utils.GetFormString(txtQuantity.Text);
            participationItemInfo.IsIncluded = sbrIncluded.Checked;
            participationItemInfo.Amount = UI.Utils.GetFormDecimal(txtAmount.Text);
            participationItemInfo.Notes = UI.Utils.GetFormString(txtNotes.Text);

            if (participationItemInfo.Amount != null)
                participationItemInfo.Confirmed = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterParticipationItemId;

            try
            {
                Security.CheckAccess(Security.userActions.EditParticipationSubContractor);
                parameterParticipationItemId = Utils.CheckParameter("ParticipationItemId");
                participationItemInfo = tradesController.GetParticipationItem(Int32.Parse(parameterParticipationItemId));
                Core.Utils.CheckNullObject(participationItemInfo, parameterParticipationItemId, "Participation Item");
                participationItemInfo.TradeParticipation = tradesController.GetTradeParticipation(participationItemInfo.TradeParticipation.Id);

                if (participationItemInfo.TradeParticipation.ComparisonParticipation == null)
                    throw new Exception("Comparison participation does not exist");

                participationItemInfo.TradeItem = tradesController.GetTradeItem(participationItemInfo.TradeItem.Id);
                participationItemInfo.TradeParticipation.ComparisonParticipation = tradesController.GetTradeParticipation(participationItemInfo.TradeParticipation.ComparisonParticipation.Id);
                participationItemInfo.TradeParticipation.ComparisonParticipation.QuoteParticipation = participationItemInfo.TradeParticipation;

                contractsController.CheckEditCurrentUser(participationItemInfo.TradeParticipation.ComparisonParticipation);

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
                    TradesController.GetInstance().UpdateParticipationItem(participationItemInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect(String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", participationItemInfo.TradeParticipation.ComparisonParticipation.IdStr));
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", participationItemInfo.TradeParticipation.ComparisonParticipation.IdStr));
        }
#endregion

    }
}