using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewParticipationPage : SOSPage
    {

#region Members
        private TradeParticipationInfo tradeParticipationInfo = null;
        private int? tmpRank = null;
        private Boolean? tmpPulledOut = null;
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

        private void BindCopyQuote()
        {
            if (!tradeParticipationInfo.IsEmpty)
            {
                cmdCopyQuote.Enabled = false;
                cmdCopyQuote.OnClientClick = String.Empty;
            }
        }

        private void BindFileLinks()
        {
            if (tradeParticipationInfo.QuoteFile != null)
            {
                sflQuoteFile.FilePath = UI.Utils.PathQuotesFile(null, tradeParticipationInfo.QuoteFileName);
                sflQuoteFile.BasePath = tradeParticipationInfo.Trade.Project.AttachmentsFolder;
                sflQuoteFile.PageLink = String.Format("~/Modules/Projects/ShowParticipationQuoteFile.aspx?ParticipationId={0}", tradeParticipationInfo.IdStr);
            }
        }

        private void BindParticipation()
        {
            ContractsController contractsController = ContractsController.GetInstance();
            String template = TradesController.GetInstance().GetInvitationTemplate(1).Template;
            XmlDocument xmlDocument;

            if (tradeParticipationInfo.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            lblInvitationDate.Text = UI.Utils.SetFormDate(tradeParticipationInfo.InvitationDate);
            lblStatus.Text = UI.Utils.SetFormString(tradeParticipationInfo.StatusName);
            lblDueDate.Text = UI.Utils.SetFormDate(tradeParticipationInfo.QuoteDueDate);
            lblQuoteAmount.Text = UI.Utils.SetFormDecimal(tradeParticipationInfo.Amount);
            lblQuoteDate.Text = UI.Utils.SetFormDate(tradeParticipationInfo.QuoteDate);
            lblReminderDate.Text = UI.Utils.SetFormDate(tradeParticipationInfo.ReminderDate);
            lblRank.Text = UI.Utils.SetFormInteger(tradeParticipationInfo.Rank);
            lblComments.Text = UI.Utils.SetFormString(tradeParticipationInfo.Comments);
            ddlPaymentTerms.SelectedItem.Value = UI.Utils.SetFormString(tradeParticipationInfo.PaymentTerms);  // DS20230921
            //#---
            lblInternalComments.Text = UI.Utils.SetFormString(tradeParticipationInfo.InternalComments);
            lblSafetyRating.Text= UI.Utils.SetFormString(tradeParticipationInfo.safetyRisk);
            //#---

            //San-30-01-2019--To display ShortName   ---

            //lnkSubcontractor.Text = UI.Utils.SetFormString(tradeParticipationInfo.SubContractor.Name);
            lnkSubcontractor.Text = UI.Utils.SetFormString(tradeParticipationInfo.SubContractor.ShortNameOrName);

            //San-30-01-2019--To display ShortName   ---

            lnkSubcontractor.NavigateUrl = "~/Modules/SubContractors/ViewSubContractor.aspx?SubContractorId=" + tradeParticipationInfo.SubContractor.IdStr;




            lnkInvitation.NavigateUrl = "~/Modules/Projects/ViewInvitation.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;

            if (tradeParticipationInfo.Contact != null)
            {
                lnkContact.Text = UI.Utils.SetFormString(tradeParticipationInfo.Contact.Name);
                lnkContact.NavigateUrl = "~/Modules/People/ViewContact.aspx?PeopleId=" + tradeParticipationInfo.Contact.IdStr;
            }

            tradeParticipationInfo.Rank = 1;
            tradeParticipationInfo.PulledOut = false;
            xmlDocument = contractsController.CheckTradeForTemplate(tradeParticipationInfo.Trade, template);
            tradeParticipationInfo.Rank = tmpRank;
            tradeParticipationInfo.PulledOut = tmpPulledOut;

            if (tradeParticipationInfo.CanSendInvitation)
                if (tradeParticipationInfo.IsClosed)
                    cmdSendByEmail.OnClientClick = "javascript:alert('The tendering is already closed'); return false;";
                else
                    if (tradeParticipationInfo.QuoteDueDate == null)
                    cmdSendByEmail.OnClientClick = "javascript:alert('Due date must be provided'); return false;";
                else
                        if (xmlDocument.DocumentElement != null)
                    cmdSendByEmail.OnClientClick = "javascript:alert('There are missing fields'); return false;";
                else
                    cmdSendByEmail.OnClientClick = "javascript:return confirm('Send invitation by email ?');";
            
            //#---Resend invitation to Subi
            else if (tradeParticipationInfo.Status == TradeParticipationInfo.StatusEnum.InvitedOnLine || tradeParticipationInfo.Status == TradeParticipationInfo.StatusEnum.Invited)
            {
                cmdSendByEmail.OnClientClick = "javascript:return confirm('Send invitation again by email ?');";

            }
            //#-- Resend invitation to Subi

            else
                cmdSendByEmail.OnClientClick = "javascript:alert('Cannot send when status is: " + tradeParticipationInfo.StatusName + "'); return false;";

           

            if (tradeParticipationInfo.CanSendReminder)
                if (tradeParticipationInfo.IsClosed)
                    cmdSendReminder.OnClientClick = "javascript:alert('The tendering is already closed'); return false;";
                else
                    if (tradeParticipationInfo.IsReminded)
                        cmdSendReminder.OnClientClick = "javascript:alert('A reminder has already been sent'); return false;";
                    else
                        cmdSendReminder.OnClientClick = "javascript:return confirm('Send quote reminder by email ?');";
            else
                cmdSendReminder.OnClientClick = "javascript:alert('Cannot send when status is: " + tradeParticipationInfo.StatusName + "'); return false;";
            
            if (xmlDocument.DocumentElement != null)
            {
                TreeViewCheck.Nodes.Clear();
                TreeViewCheck.Nodes.Add(new TreeNode());
                Utils.AddNode(xmlDocument.DocumentElement, TreeViewCheck.Nodes[0]);
                TreeViewCheck.ExpandAll();

                pnlErrors.Visible = true;
            }

            if (tradeParticipationInfo.IsSubmitted && tradeParticipationInfo.QuoteParticipation != null)
            {
                lnkPrint.NavigateUrl = "~/Modules/Projects/ShowQuote.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;

                tradeParticipationInfo.QuoteParticipation.Trade = tradeParticipationInfo.Trade;
                tradeParticipationInfo.Trade.Participations = new List<TradeParticipationInfo>();
                tradeParticipationInfo.Trade.Participations.Add(tradeParticipationInfo.QuoteParticipation);
                
                ViewComparison1.TradeParticipation = tradeParticipationInfo.QuoteParticipation;
                ViewComparison1.ComparisonType = tradeParticipationInfo.Type;
                ViewComparison1.AllowEdit = false;
                ViewComparison1.RePaint = true;

                lblQuoteAmountQuote.Text = UI.Utils.SetFormDecimal(tradeParticipationInfo.QuoteParticipation.Amount);
                txtCommentsQuote.Text = UI.Utils.SetFormString(tradeParticipationInfo.QuoteParticipation.Comments);

                ViewQuoteDrawings1.TradeParticipation = tradeParticipationInfo;
                ViewQuoteDrawings1.RePaint = true;

                pnlQuote.Visible = true;
            }

            BindCopyQuote();
            BindFileLinks();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipation);
                String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetDeepTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");

                tmpRank = tradeParticipationInfo.Rank;
                tmpPulledOut = tradeParticipationInfo.PulledOut;

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditParticipation))
                    {
                        if (processController.AllowEditCurrentUser(tradeParticipationInfo.Trade))
                        {
                            pnlEdit.Visible = true;
                            pnlCopyQuote.Visible = true;

                            cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Subcontractor ?');");
                        }
                    }

                    BindParticipation();
                }

                BindFileLinks();

                pnlMessage.Visible = false;
                pnlCopyMessage.Visible = false;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditParticipation.aspx?ParticipationId=" + tradeParticipationInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteTradeParticipation(tradeParticipationInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeParticipationInfo.Trade.IdStr);
        }

        protected void cmdSendByEmail_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                tradeParticipationInfo.Rank = 1;
                tradeParticipationInfo.PulledOut = false;
                projectsController.SendInvitacion(tradeParticipationInfo, tmpRank, tmpPulledOut);
                BindParticipation();
                lblMessage.Text = "Invitation to tender sent.";
                pnlMessage.Visible = true;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdSendReminder_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                projectsController.SendInvitacionReminder(tradeParticipationInfo);
                BindParticipation();
                lblMessage.Text = "Quote submission reminder sent.";
                pnlMessage.Visible = true;
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCopyQuote_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();

            try
            {
                List<String> notFoudList = new List<string>();

                tradesController.CopyTradeParticipationQuoteToComparison(tradeParticipationInfo, ref notFoudList);
                tradeParticipationInfo = tradesController.GetDeepTradeParticipationWithTradeAndProject(tradeParticipationInfo.Id);                
                
                lnkComparison.NavigateUrl = String.Format("~/Modules/ViewComparisonrojects/ViewComparison.aspx?Type={0}&TradeId={1}", tradeParticipationInfo.Type, tradeParticipationInfo.Trade.IdStr);
                pnlCopyMessage.Visible = true;

                if (notFoudList.Count > 1)
                {
                    repErrorsCopy.DataSource = notFoudList;
                    repErrorsCopy.DataBind();
                    pnlCopyErrors.Visible = true;
                }
                else
                    pnlCopyErrors.Visible = false;

                BindParticipation();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}

