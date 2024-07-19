using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditComparisonPage : SOSPage
    {

#region Members
        private TradeParticipationInfo tradeParticipation = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeParticipation.Trade == null)
                return null;

            tempNode.ParentNode.Title = tradeParticipation.Trade.Name;
            tempNode.ParentNode.Url += "?TradeId=" + tradeParticipation.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeParticipation.Trade.Project.Name + (tradeParticipation.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeParticipation.Trade.Project.IdStr;

            return currentNode;
        }

        private void CreateForm()
        {
            if (tradeParticipation.Equals(tradeParticipation.Trade.BudgetParticipation) && tradeParticipation.Trade.IsUsingBudgetModule)
                EditComparison1.CanEditValues = false;

            EditComparison1.TradeParticipation = tradeParticipation;
        }

        private void ObjectsToForm()
        {
            if (tradeParticipation.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            CheckComparison1.Trade = tradeParticipation.Trade;
            CheckComparison1.TradeParticipation = tradeParticipation;
            CheckComparison1.TradeParticipationType = tradeParticipation.Type;
        }

        private void FormToObjects()
        {
            tradeParticipation = EditComparison1.TradeParticipation;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterParticipationId;

            try
            {
                Security.CheckAccess(Security.userActions.EditTrade);

                parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipation = tradesController.GetDeepTradeParticipation(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipation, parameterParticipationId, "Trade Participation");

                if (tradeParticipation.IsActive)
                    tradeParticipation.Trade = tradesController.GetTradeWithItemsAndParticipationsActive(tradeParticipation.Trade.Id);
                else
                    tradeParticipation.Trade = tradesController.GetTradeWithItemsAndParticipationsProposal(tradeParticipation.Trade.Id);

                tradeParticipation.Trade.Project = projectsController.GetProject(tradeParticipation.Trade.Project.Id);

                tradeParticipation.Trade.TradeBudgets = tradesController.GetTradeBudgets(tradeParticipation.Trade);
                tradesController.SetBudgetParticipationAmount(tradeParticipation.Trade);

                processController.CheckEditCurrentUser(tradeParticipation.Trade);

                CreateForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (!Page.IsPostBack)
            {
                ObjectsToForm();
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    TradesController.GetInstance().UpdateTradeParticipationItems(tradeParticipation);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect(String.Format("~/Modules/Projects/ViewComparison.aspx?Type={0}&TradeId={1}", tradeParticipation.Type, tradeParticipation.Trade.IdStr));
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/Modules/Projects/ViewComparison.aspx?Type={0}&TradeId={1}", tradeParticipation.Type, tradeParticipation.Trade.IdStr));
        }
#endregion

    }
}
