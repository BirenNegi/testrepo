using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditProjectTradePage : SOSPage
    {

#region Members
        private TradeInfo tradeInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeInfo == null)
                return null;

            tempNode.Title = tradeInfo.Name;

            tempNode.ParentNode.Title = tradeInfo.Project.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + tradeInfo.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            ProcessController processController = ProcessController.GetInstance();

            lblName.Text = UI.Utils.SetFormString(tradeInfo.Name);
            lblCode.Text = UI.Utils.SetFormString(tradeInfo.Code);
            lblJobType.Text = tradeInfo.JobType.Name;
            sbvTenderRequired.Checked = tradeInfo.TenderRequired;
            lblDescription.Text = UI.Utils.SetFormString(tradeInfo.Description);
            txtDaysFromPCD.Text = UI.Utils.SetFormInteger(tradeInfo.DaysFromPCD);
            txtScopeHeader.Text = UI.Utils.SetFormString(tradeInfo.ScopeHeader);
            txtScopeFooter.Text = UI.Utils.SetFormString(tradeInfo.ScopeFooter);
            sdrInvitationDate.Date = tradeInfo.InvitationDate;
            sdrQuotesDueDate.Date = tradeInfo.DueDate;
            sdrComparisonDueDate.Date = tradeInfo.ComparisonDueDate;
            sdrContractDueDate.Date = tradeInfo.ContractDueDate;
            sdrCommencementDate.Date = tradeInfo.CommencementDate;

            sdrCompletionDate.Date = tradeInfo.CompletionDate;
            lblWorkOrder.Text = UI.Utils.SetFormString(tradeInfo.WorkOrderNumber);

            sfsQuotesFile.FilePath = tradeInfo.QuotesFile;
            sfsQuotesFile.Path = tradeInfo.Project.AttachmentsFolder;

            sfsPrelettingFile.FilePath = tradeInfo.PrelettingFile;
            sfsPrelettingFile.Path = tradeInfo.Project.AttachmentsFolder;

            if (tradeInfo.ProjectManager != null)
            {
                txtPMId.Value = tradeInfo.ProjectManager.IdStr;
                txtPM.Text = tradeInfo.ProjectManager.Name;
            }

            if (tradeInfo.ContractsAdministrator != null)
            {
                txtCAId.Value = tradeInfo.ContractsAdministrator.IdStr;
                txtCA.Text = tradeInfo.ContractsAdministrator.Name;
            }

            if (processController.CanPlayRole((EmployeeInfo)Web.Utils.GetCurrentUser(), EmployeeInfo.TypeProjectManager, tradeInfo.Project))
                sdrComparisonDueDate.Enabled = true;

            if (tradeInfo.Flag == null)
                rbNoFlag.Checked = true;
            else if ((Int32)tradeInfo.Flag == TradeInfo.FlagRed)
                rbRedFlag.Checked = true;
            else if ((Int32)tradeInfo.Flag == TradeInfo.FlagGreen)
                rbGreenFlag.Checked = true;

            cmdSelPM.NavigateUrl = Utils.PopupPeople(this, txtPMId.ClientID, txtPM.ClientID, PeopleInfo.PeopleTypeEmployee, tradeInfo.Project.BusinessUnit);
            cmdSelCA.NavigateUrl = Utils.PopupPeople(this, txtCAId.ClientID, txtCA.ClientID, PeopleInfo.PeopleTypeEmployee, tradeInfo.Project.BusinessUnit);
        }

        private void FormToObjects()
        {
            ProcessController processController = ProcessController.GetInstance();

            tradeInfo.DaysFromPCD = UI.Utils.GetFormInteger(txtDaysFromPCD.Text);
            tradeInfo.ScopeHeader = UI.Utils.GetFormString(txtScopeHeader.Text);
            tradeInfo.ScopeFooter = UI.Utils.GetFormString(txtScopeFooter.Text);
            tradeInfo.InvitationDate = sdrInvitationDate.Date;
            tradeInfo.DueDate = sdrQuotesDueDate.Date;
            tradeInfo.ContractDueDate = sdrContractDueDate.Date;
            tradeInfo.CommencementDate = sdrCommencementDate.Date;
            tradeInfo.CompletionDate = sdrCompletionDate.Date;

            tradeInfo.ProjectManager = txtPMId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtPMId.Value)) : null;
            tradeInfo.ContractsAdministrator = txtCAId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtCAId.Value)) : null;

            tradeInfo.QuotesFile = sfsQuotesFile.FilePath;
            tradeInfo.PrelettingFile = sfsPrelettingFile.FilePath;

            if (rbRedFlag.Checked)
                tradeInfo.Flag = TradeInfo.FlagRed;
            else if (rbGreenFlag.Checked)
                tradeInfo.Flag = TradeInfo.FlagGreen;
            else
                tradeInfo.Flag = null;

            if (sdrComparisonDueDate.Enabled)
                tradeInfo.ComparisonDueDate = sdrComparisonDueDate.Date;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeId;

            try
            {
                Security.CheckAccess(Security.userActions.EditTrade);
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetTradeWithParticipations(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");
                tradeInfo.Project = projectsController.GetProject(tradeInfo.Project.Id);
                processController.CheckEditCurrentUser(tradeInfo);

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
                    tradeInfo.Id = TradesController.GetInstance().AddUpdateTrade(tradeInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (tradeInfo.Id == null)
                Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + tradeInfo.Project.IdStr);
            else
                Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeInfo.IdStr);
        }
 #endregion

    }
}
