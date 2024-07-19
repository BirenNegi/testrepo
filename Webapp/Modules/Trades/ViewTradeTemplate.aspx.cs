using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewTradeTemplatePage : SOSPage
    {

#region Members
        private TradeTemplateInfo tradeTemplateInfo = null;
        private List<SubContractorInfo> ViewSubcontractors = new List<SubContractorInfo>();

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
        private void BindSearch()      // DS20321108
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));
        }
        private void BindTradeTemplate()
        {
            lnkAddItemCategory.NavigateUrl = "~/Modules/Trades/EditTradeItemCategory.aspx?TradeId=" + tradeTemplateInfo.Trade.IdStr;

            sbvIsStandard.Checked = tradeTemplateInfo.IsStandard;
            sbvTenderRequired.Checked = tradeTemplateInfo.Trade.TenderRequired;

            lblName.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.Name);
            lblCode.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.Code);
            lblDescription.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.Description);
            lblDaysFromPCD.Text = UI.Utils.SetFormInteger(tradeTemplateInfo.Trade.DaysFromPCD);
            txtScopeHeader.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.ScopeHeader);
            txtScopeFooter.Text = UI.Utils.SetFormString(tradeTemplateInfo.Trade.ScopeFooter);
            lblJobType.Text = tradeTemplateInfo.Trade.JobType.Name;

            BindItemCategories();
            BindDrawingTypes();
            BindDefaultSubContractors();
        }

        private void BindItemCategories()
        {
            gvItemCategories.DataSource = tradeTemplateInfo.Trade.ItemCategories;
            gvItemCategories.DataBind();
        }

        private void BindDrawingTypes()
        {
            List<DrawingTypeInfo> drawingTypeInfoList = TradesController.GetInstance().GetDrawingTypes();
            
            ddlDrawingTypes.Items.Clear();
            
            foreach (DrawingTypeInfo drawingTypeInfo in drawingTypeInfoList)
                ddlDrawingTypes.Items.Add(new ListItem(drawingTypeInfo.Name,drawingTypeInfo.IdStr));

            foreach (DrawingTypeInfo drawingTypeInfo in tradeTemplateInfo.Trade.DrawingTypes)
                ddlDrawingTypes.Items.Remove(new ListItem(drawingTypeInfo.Name, drawingTypeInfo.IdStr));

            butAddDrawingType.Enabled = ddlDrawingTypes.Items.Count != 0;

            gvDrawingTypes.DataSource = tradeTemplateInfo.Trade.DrawingTypes;
            gvDrawingTypes.DataBind();
        }

        private void BindDefaultSubContractors()
        {
            string parameterTradeTemplateId = Utils.CheckParameter("TradeTemplateId");
            tradeTemplateInfo = TradesController.GetInstance().GetDeepTradeTemplate(Int32.Parse(parameterTradeTemplateId));
            ViewSubcontractors.Clear();

            if (ddlBusinessUnit.SelectedValue == "")
            {
                //                ViewSubcontractors = tradeTemplateInfo.DefaultSubContractors;
                foreach (SubContractorInfo SubContractorInfo in tradeTemplateInfo.DefaultSubContractors)
                {
                    ViewSubcontractors.Add(SubContractorInfo);
                }

            }
            else
            {
                // ViewSubcontractors.Clear;
                foreach (SubContractorInfo SubContractorInfo in tradeTemplateInfo.DefaultSubContractors)
                {
                    if (SubContractorInfo.BusinessUnit.IdStr == ddlBusinessUnit.SelectedValue)
                    {
                        ViewSubcontractors.Add(SubContractorInfo);
                    }
                    
                }
                    
                ViewSubcontractors = ViewSubcontractors;
            }
            gvSubContractors.DataSource = ViewSubcontractors;
            gvSubContractors.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterTradeTemplateId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTradeTemplate);
                parameterTradeTemplateId = Utils.CheckParameter("TradeTemplateId");

                tradeTemplateInfo = TradesController.GetInstance().GetDeepTradeTemplate(Int32.Parse(parameterTradeTemplateId));

                Core.Utils.CheckNullObject(tradeTemplateInfo, parameterTradeTemplateId, "Trade Template");

                if (tradeTemplateInfo.Trade == null)
                    tradeTemplateInfo.Trade = new TradeInfo();

                if (tradeTemplateInfo.Trade.JobType == null)
                    tradeTemplateInfo.Trade.JobType = new JobTypeInfo();

                cmdSelSubContractor.NavigateUrl = Utils.PopupCompany(this, txtSubContractorId.ClientID, txtSubContractorName.ClientID, null);

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTradeTemplate))
                    {
                        cmdEditTop.Visible = true;
                        cmdDeleteTop.Visible = true;

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Template?');");

                        phAddItemCategory.Visible = true;
                        gvItemCategories.Columns[4].Visible = true;
                        gvItemCategories.Columns[5].Visible = true;

                        phAddDrawingTypes.Visible = true;
                        gvDrawingTypes.Columns[0].Visible = true;

                        phAddSubContractor.Visible = true;
                        gvSubContractors.Columns[1].Visible = true;
                    }
                    BindSearch();    // DS20321108
                    BindTradeTemplate();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 1;
            i = i + 1;
            BindTradeTemplate();
        }
        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/EditTradeTemplate.aspx?TradeTemplateId=" + tradeTemplateInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteTradeTemplate(tradeTemplateInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Trades/ListTradeTemplates.aspx");
        }

        protected void butAddDrawingType_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();

            if (ddlDrawingTypes.SelectedItem != null)
            {
                tradesController.AddTradeDrawingType(tradeTemplateInfo.Trade, new DrawingTypeInfo(Int32.Parse(ddlDrawingTypes.SelectedItem.Value)));
                tradeTemplateInfo.Trade.DrawingTypes = tradesController.GetDrawingTypes(tradeTemplateInfo.Trade);
                BindDrawingTypes();
            }
        }

        protected void gvDrawingTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            try
            {
                int DrawingTypeId = (int)gvDrawingTypes.DataKeys[e.RowIndex].Value;
                tradesController.DeleteTradeDrawingType(tradeTemplateInfo.Trade, (new DrawingTypeInfo(DrawingTypeId)));
                tradeTemplateInfo.Trade.DrawingTypes = tradesController.GetDrawingTypes(tradeTemplateInfo.Trade);
                BindDrawingTypes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void butAddSubContractor_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            SubContractorInfo subContractorInfo;

            try
            {
                if (txtSubContractorId.Value != String.Empty)
                {
                    subContractorInfo = new SubContractorInfo(Int32.Parse(txtSubContractorId.Value));

                    if (tradeTemplateInfo.DefaultSubContractors.Find(delegate(SubContractorInfo subContractorInfoInList) { return subContractorInfoInList.Id == subContractorInfo.Id; }) == null)
                    {
                        tradesController.AddTradeTemplateSubContractor(tradeTemplateInfo, subContractorInfo);
                        tradeTemplateInfo.DefaultSubContractors = tradesController.GetTradeTemplateSubcontractors(tradeTemplateInfo);
                        BindDefaultSubContractors();
                    }
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvSubContractors_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();

            try
            {
                int SubContractorId = (int)gvSubContractors.DataKeys[e.RowIndex].Value;
                tradesController.DeleteTradeTemplateSubContractor(tradeTemplateInfo, (new SubContractorInfo(SubContractorId)));
                tradeTemplateInfo.DefaultSubContractors = tradesController.GetTradeTemplateSubcontractors(tradeTemplateInfo);
                BindDefaultSubContractors();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvItemCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            int? tradeItemCategoryId = (int?)gvItemCategories.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
            TradeItemCategoryInfo tradeItemCategoryInfo = tradeTemplateInfo.Trade.ItemCategories.Find(delegate(TradeItemCategoryInfo tradeItemCategoryInfoInList) { return tradeItemCategoryInfoInList.Id == tradeItemCategoryId; });

            if (e.CommandName == "MoveUp")
                tradesController.ChangeDisplayOrderTradeItemCategory(tradeTemplateInfo.Trade.ItemCategories, tradeItemCategoryInfo, true);
            else if (e.CommandName == "MoveDown")
                tradesController.ChangeDisplayOrderTradeItemCategory(tradeTemplateInfo.Trade.ItemCategories, tradeItemCategoryInfo, false);

            tradeTemplateInfo.Trade.ItemCategories = tradesController.GetTradeItemCategories(tradeTemplateInfo.Trade);
            BindItemCategories();
        }
       
        //#---
        protected void gvSubContractors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                if (tradeTemplateInfo.DefaultSubContractors[i].BusinessUnitslist.Count > 0)
                    foreach (BusinessUnitInfo Bunit in tradeTemplateInfo.DefaultSubContractors[i].BusinessUnitslist)
                    {
                        e.Row.Cells[7].Text += Bunit.Name + ", ";
                    }

            }
        }

        //#---
       #endregion
    }
}