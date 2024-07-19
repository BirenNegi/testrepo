using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditParticipationPage : SOSPage
    {

#region Members
        private TradeParticipationInfo tradeParticipationInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeParticipationInfo == null)
                return null;

            tempNode.Title = tradeParticipationInfo.SubContractor.Name;

            tempNode.ParentNode.Title = tradeParticipationInfo.Trade.Name;
            tempNode.ParentNode.Url += "?TradeId=" + tradeParticipationInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeParticipationInfo.Trade.Project.Name + (tradeParticipationInfo.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeParticipationInfo.Trade.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (tradeParticipationInfo.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            ddlRank.Items.Add(new ListItem(String.Empty, String.Empty));
            for (int i = 1; i <= tradeParticipationInfo.Trade.SubcontractorsParticipations.Count; i++)
                ddlRank.Items.Add(new ListItem(i.ToString(), i.ToString()));

            ddlContact.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (ContactInfo contactInfo in tradeParticipationInfo.SubContractor.Contacts)
                ddlContact.Items.Add(new ListItem(contactInfo.Name, contactInfo.IdStr));

            lblSubcontractor.Text = UI.Utils.SetFormString(tradeParticipationInfo.SubContractor.Name);
            sdrInvitationDate.Date = tradeParticipationInfo.InvitationDate;
            sbrPulledOut.Checked = tradeParticipationInfo.PulledOut;
            sdrDueDate.Date = tradeParticipationInfo.QuoteDueDate;
            txtQuoteAmount.Text = UI.Utils.SetFormEditDecimal(tradeParticipationInfo.Amount);
            sdrQuoteDate.Date = tradeParticipationInfo.QuoteDate;
            sdrReminderDate.Date = tradeParticipationInfo.ReminderDate;
            txtComments.Text = UI.Utils.SetFormString(tradeParticipationInfo.Comments);
            ddlPaymentTerms.SelectedItem.Value = UI.Utils.SetFormString(tradeParticipationInfo.PaymentTerms);  // DS20230921
            //#-----

            txtinternalComments.Text= UI.Utils.SetFormString(tradeParticipationInfo.InternalComments);
            ddlSfetyRatings.SelectedValue = UI.Utils.SetFormString(tradeParticipationInfo.safetyRisk);
            //#----

            if (tradeParticipationInfo.Rank != null)
            {
                if (!ddlRank.Items.Contains(new ListItem(tradeParticipationInfo.Rank.ToString(), tradeParticipationInfo.Rank.ToString())))
                    ddlRank.Items.Add(new ListItem(tradeParticipationInfo.Rank.ToString(), tradeParticipationInfo.Rank.ToString()));

                ddlRank.SelectedValue = tradeParticipationInfo.Rank.ToString();
            }

            if (tradeParticipationInfo.Contact != null)
                ddlContact.SelectedValue = tradeParticipationInfo.Contact.IdStr;

            BindCheckComparison();
        }

        private void BindCheckComparison()
        {
            CheckComparison1.Trade = tradeParticipationInfo.Trade;
            CheckComparison1.TradeParticipationType = tradeParticipationInfo.Type;
            CheckComparison1.IgnoreRankAssignment = true;
        }

        private void FormToObjects()
        {
            TradeParticipationInfo tradeParticipation;

            tradeParticipationInfo.SubContractor.Name = UI.Utils.GetFormString(lblSubcontractor.Text);
            tradeParticipationInfo.InvitationDate = sdrInvitationDate.Date;
            tradeParticipationInfo.PulledOut = sbrPulledOut.Checked;
            tradeParticipationInfo.QuoteDueDate = UI.Utils.DateLastMinute(sdrDueDate.Date);
            tradeParticipationInfo.Amount = UI.Utils.GetFormDecimal(txtQuoteAmount.Text);
            tradeParticipationInfo.QuoteDate = sdrQuoteDate.Date;
            tradeParticipationInfo.ReminderDate = sdrReminderDate.Date;
            tradeParticipationInfo.Rank = UI.Utils.GetFormInteger(ddlRank.SelectedValue);
            tradeParticipationInfo.Comments = UI.Utils.GetFormString(txtComments.Text);
            //#----
            tradeParticipationInfo.InternalComments = UI.Utils.GetFormString(txtinternalComments.Text);
            tradeParticipationInfo.safetyRisk = UI.Utils.GetFormString(ddlSfetyRatings.SelectedItem.Value);
            //#---
            tradeParticipationInfo.PaymentTerms = UI.Utils.GetFormString(ddlPaymentTerms.SelectedItem.Value); // DS20230921

            tradeParticipationInfo.Contact = ddlContact.SelectedValue != String.Empty ? new ContactInfo(Int32.Parse(ddlContact.SelectedValue)) : null;

            tradeParticipation = tradeParticipationInfo.Trade.Participations.Find(delegate(TradeParticipationInfo tradeParticipationInfoInList) { return tradeParticipationInfoInList.Equals(tradeParticipationInfo); });
            tradeParticipationInfo.Trade.Participations[tradeParticipationInfo.Trade.Participations.IndexOf(tradeParticipation)] = tradeParticipationInfo;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterParticipationId;

            try
            {
                Security.CheckAccess(Security.userActions.EditParticipation);
                parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetDeepTradeParticipationWithContacts(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");

                tradeParticipationInfo.Trade = tradeParticipationInfo.IsActive ? tradesController.GetTradeWithItemsAndParticipationsActive(tradeParticipationInfo.Trade.Id) : tradesController.GetTradeWithItemsAndParticipationsProposal(tradeParticipationInfo.Trade.Id);
                tradeParticipationInfo.Trade.Project = projectsController.GetProject(tradeParticipationInfo.Trade.Project.Id);

                tradeParticipationInfo.Trade.TradeBudgets = tradesController.GetTradeBudgets(tradeParticipationInfo.Trade);
                tradesController.SetBudgetParticipationAmount(tradeParticipationInfo.Trade);

                processController.CheckEditCurrentUser(tradeParticipationInfo.Trade);

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

        protected void valRank_ServerValidate(object source, ServerValidateEventArgs args)
        {
            FormToObjects();

            BindCheckComparison();
            CheckComparison1.CheckComparision();

            args.IsValid = tradeParticipationInfo.IsPulledOut || tradeParticipationInfo.Rank == null || CheckComparison1.ComparisonOk;
            CheckComparison1.Visible = !args.IsValid;
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    TradesController.GetInstance().UpdateTradeParticipation(tradeParticipationInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewParticipation.aspx?ParticipationId=" + tradeParticipationInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/ViewParticipation.aspx?ParticipationId=" + tradeParticipationInfo.IdStr);
        }
#endregion

    }
}