using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditTradeTemplatePage : SOSPage
    {

#region Members
        private TradeTemplateInfo tradeTemplateInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeTemplateInfo == null)
                return null;

            tempNode.Title = tradeTemplateInfo.Trade.Name;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            List<JobTypeInfo> jobTypes = ContractsController.GetInstance().GetJobTypes();

            ddlJobTypes.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (JobTypeInfo jobType in jobTypes)
                ddlJobTypes.Items.Add(new ListItem(jobType.Name, jobType.IdStr));

            if (tradeTemplateInfo.Id == null)
            {
                TitleBar.Title = "Adding Trade Template";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Trade Template";
            }
            
            sbrIsStandard.Checked = tradeTemplateInfo.IsStandard;
            sbrTenderRequired.Checked = tradeTemplateInfo.Trade.TenderRequired;

            txtName.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.Name);
            txtCode.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.Code);
            txtDescription.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.Description);
            txtDaysFromPCD.Text = UI.Utils.SetFormInteger(tradeTemplateInfo.Trade.DaysFromPCD);
            txtScopeHeader.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.ScopeHeader);
            txtScopeFooter.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.ScopeFooter);
            ddlJobTypes.SelectedValue = tradeTemplateInfo.Trade.JobType.IdStr;
        }

        private void FormToObjects()
        {
            tradeTemplateInfo.IsStandard = sbrIsStandard.Checked;

            tradeTemplateInfo.Trade.TenderRequired = sbrTenderRequired.Checked;
            tradeTemplateInfo.Trade.Name = UI.Utils.GetFormString(txtName.Text);
            tradeTemplateInfo.Trade.Code = UI.Utils.GetFormString(txtCode.Text);
            tradeTemplateInfo.Trade.Description = UI.Utils.GetFormString(txtDescription.Text);
            tradeTemplateInfo.Trade.DaysFromPCD = UI.Utils.GetFormInteger(txtDaysFromPCD.Text);
            tradeTemplateInfo.Trade.ScopeHeader = UI.Utils.GetFormString(txtScopeHeader.Text);
            tradeTemplateInfo.Trade.ScopeFooter = UI.Utils.GetFormString(txtScopeFooter.Text);

            if (ddlJobTypes.SelectedValue != String.Empty)
                tradeTemplateInfo.Trade.JobType.Id = Int32.Parse(ddlJobTypes.SelectedValue);
            else
                tradeTemplateInfo.Trade.JobType = null;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterTradeTemplateId = Request.Params["TradeTemplateId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditTradeTemplate);

                if (parameterTradeTemplateId == null)
                {
                    tradeTemplateInfo = new TradeTemplateInfo();
                    tradeTemplateInfo.IsStandard = false;
                }
                else
                {
                    tradeTemplateInfo = TradesController.GetInstance().GetTradeTemplate(Int32.Parse(parameterTradeTemplateId));
                    Core.Utils.CheckNullObject(tradeTemplateInfo, parameterTradeTemplateId, "Trade Template");
                }

                if (tradeTemplateInfo.Trade == null)
                    tradeTemplateInfo.Trade = new TradeInfo();

                if (tradeTemplateInfo.Trade.JobType == null)
                    tradeTemplateInfo.Trade.JobType = new JobTypeInfo();

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
                    tradeTemplateInfo.Id = TradesController.GetInstance().AddUpdateTradeTemplate(tradeTemplateInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Trades/ViewTradeTemplate.aspx?TradeTemplateId=" + tradeTemplateInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (tradeTemplateInfo.Id == null)
                Response.Redirect("~/Modules/Trades/ListTradeTemplates.aspx");
            else
                Response.Redirect("~/Modules/Trades/ViewTradeTemplate.aspx?TradeTemplateId=" + tradeTemplateInfo.IdStr);
        }
#endregion

    }
}