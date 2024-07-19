using System;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditVariationPage : SOSPage
    {

        #region Members
        private VariationInfo variationInfo;
        private List<IBudget> projectBudget;
        private Boolean useBudget = true;
        private List<IBudget> SanprojectBudget;//#---


        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (variationInfo == null)
                return null;

            tempNode.ParentNode.Url += "?SubContractId=" + variationInfo.Contract.IdStr;

            tempNode.ParentNode.ParentNode.Url += "?ContractId=" + variationInfo.Contract.ParentContract.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ContractId=" + variationInfo.Contract.ParentContract.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Title = variationInfo.Contract.Trade.Name;
            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Url += "?TradeId=" + variationInfo.Contract.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.Title = variationInfo.Contract.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + variationInfo.Contract.Trade.Project.IdStr;

            return currentNode;
        }

        private void BindVariationType()
        {
            if (variationInfo.Id == null)
            {
                if (variationInfo.Type == VariationInfo.VariationTypeBackCharge)
                {
                    List<TradeInfo> tradeInfoList = TradesController.GetInstance().GetTradesBasic(variationInfo.Contract.Trade.Project);

                    ddlRelatedTrade.Items.Add(new ListItem("", ""));

                    foreach (TradeInfo tradeInfo in tradeInfoList)
                        if (tradeInfo.ContractApproved)
                            if (tradeInfo.Code != variationInfo.Contract.Trade.Code)
                                if (tradeInfo.Code.Substring(0, 2) == variationInfo.Contract.Trade.Code.Substring(0, 2))
                                    ddlRelatedTrade.Items.Add(new ListItem(tradeInfo.Code + " " + tradeInfo.Name, tradeInfo.IdStr));

                    foreach (TradeInfo tradeInfo in tradeInfoList)
                        if (tradeInfo.ContractApproved)
                            if (tradeInfo.Code != variationInfo.Contract.Trade.Code)
                                if (tradeInfo.Code.Substring(0, 2) != variationInfo.Contract.Trade.Code.Substring(0, 2))
                                    ddlRelatedTrade.Items.Add(new ListItem(tradeInfo.Code + " " + tradeInfo.Name, tradeInfo.IdStr));

                    valRealtedTrade.Enabled = true;
                }
                else
                {
                    ddlRelatedTrade.Items.Clear();
                    valRealtedTrade.Enabled = false;
                }
            }

            if (useBudget)
            {
                ddlBudgetSource.Items.Clear();

                if (variationInfo.Type != null)
                {
                    ddlBudgetSource.Items.Add(new ListItem(String.Empty, String.Empty));
                    foreach (IBudget iBudget in projectBudget)
                        //---SAN---TV---if ((variationInfo.IsBOQ && iBudget.BudgetType == BudgetType.BOQ) || (variationInfo.IsCV && iBudget.BudgetType == BudgetType.CV) || (variationInfo.IsSA && iBudget.BudgetType == BudgetType.SA))

                        if ((variationInfo.IsBOQ && iBudget.BudgetType == BudgetType.BOQ) || (variationInfo.IsCV && iBudget.BudgetType == BudgetType.CV) || (variationInfo.IsSA && iBudget.BudgetType == BudgetType.SA) || (variationInfo.IsTV && iBudget.BudgetType == BudgetType.TV))  //---SAN--TV
                            ddlBudgetSource.Items.Add(new ListItem(iBudget.TradeCode + " - " + iBudget.BudgetName, iBudget.Id.Value.ToString()));
                }

                valBudgetSource.Enabled = variationInfo.Type != VariationInfo.VariationTypeBackCharge;

                BindVariationBudget();
            }
            else
            {
                if (variationInfo.Type != null)
                {
                    ddlType.SelectedValue = variationInfo.Type;

                    if (variationInfo.Type == VariationInfo.VariationTypeClient)
                    {
                        if (variationInfo.Number != null)
                        {
                            ddlCVNumber.SelectedValue = variationInfo.Number;

                            if (ddlCVNumber.SelectedValue == String.Empty)
                            {
                                ddlCVNumber.Items.Add(new ListItem(variationInfo.Number, variationInfo.Number));
                                ddlCVNumber.SelectedValue = variationInfo.Number;
                            }
                        }

                        ddlCVNumber.Visible = true;
                    }
                    else if (variationInfo.Type == VariationInfo.VariationTypeSeparateAccounts)
                    {
                        if (variationInfo.Number != null)
                        {
                            ddlSANumber.SelectedValue = variationInfo.Number;

                            if (ddlSANumber.SelectedValue == String.Empty)
                            {
                                ddlSANumber.Items.Add(new ListItem(variationInfo.Number, variationInfo.Number));
                                ddlSANumber.SelectedValue = variationInfo.Number;
                            }
                        }

                        ddlSANumber.Visible = true;
                    }

                    //---TV----
                    else if (variationInfo.Type == VariationInfo.VariationTypeTenant)
                    {
                        if (variationInfo.Number != null)
                        {
                            ddlTVNumber.SelectedValue = variationInfo.Number;

                            if (ddlTVNumber.SelectedValue == String.Empty)
                            {
                                ddlTVNumber.Items.Add(new ListItem(variationInfo.Number, variationInfo.Number));
                                ddlTVNumber.SelectedValue = variationInfo.Number;
                            }
                        }

                        ddlTVNumber.Visible = true;
                    }

                    //---TV----


                    else
                    {
                        txtNumber.Text = UI.Utils.SetFormString(variationInfo.Number);
                        txtNumber.Visible = true;
                    }

                }
                else
                {
                    lblNumber.Visible = true;
                }
            }
        }

        private void BindVariationBudget()
        {
            IBudget budgetProvider;
            IBudget SanbudgetProvider;//#---


            if (variationInfo.BudgetProvider != null)
            {
                budgetProvider = projectBudget.Find(pb => pb.Equals(variationInfo.BudgetProvider));
                //#--
                SanbudgetProvider = SanprojectBudget.Find(pb => pb.Equals(variationInfo.BudgetProvider));
                //#--


                lblBudgetCurrent.Text = UI.Utils.SetFormEditDecimal(budgetProvider.BudgetAmountInitial);



                //#---
                lblBudgetOriginal.Text = UI.Utils.SetFormEditDecimal(variationInfo.BudgetAmountInitial);


                //#--lblBudgetunallocated.Text= UI.Utils.SetFormEditDecimal(budgetProvider.BudgetUnallocated);


                //#--
                if (budgetProvider.BudgetUnallocated < 0)
                    lblBudgetunallocated.Text = "0.00";
                else
                    lblBudgetunallocated.Text = UI.Utils.SetFormEditDecimal(budgetProvider.BudgetUnallocated);


                lblUnallocated.Text = UI.Utils.SetFormEditDecimal(SanbudgetProvider.BudgetUnallocated);

                //#---


                /* Uncoment to show the trade codes table
                lblTradeBudgetCurrent.Text = UI.Utils.SetFormEditDecimal(budgetProvider.BudgetAmountTradeInitial);
                lblTradeBudgetOriginal.Text = UI.Utils.SetFormEditDecimal(variationInfo.BudgetAmountTradeInitial);
                */

                lblBudgetCurrent.ForeColor = budgetProvider.BudgetAmountInitial != null && budgetProvider.BudgetAmountInitial.Value < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lblBudgetOriginal.ForeColor = variationInfo.BudgetAmountInitial != null && variationInfo.BudgetAmountInitial.Value < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lblBudgetunallocated.ForeColor = budgetProvider.BudgetUnallocated != null && budgetProvider.BudgetUnallocated.Value < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;

                /* Uncoment to show the trade codes table
                lblTradeBudgetCurrent.ForeColor =  budgetProvider.BudgetAmountTradeInitial != null && budgetProvider.BudgetAmountTradeInitial.Value < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lblTradeBudgetOriginal.ForeColor = variationInfo.BudgetAmountTradeInitial != null && variationInfo.BudgetAmountTradeInitial.Value < 0 ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                */
            }
            else
            {
                lblBudgetCurrent.Text = String.Empty;
                lblBudgetOriginal.Text = String.Empty;
                lblBudgetunallocated.Text = String.Empty;
                //#--
                lblUnallocated.Text = String.Empty;//#---

                /* Uncoment to show the trade codes table
                lblTradeBudgetCurrent.Text = String.Empty;
                lblTradeBudgetOriginal.Text = String.Empty;
                */
            }
        }

        private void BindVariationSource()
        {
            if (ddlBudgetSource.SelectedValue != String.Empty)
            {
                variationInfo.BudgetProvider = variationInfo.IsBOQ ? (IBudget)(new BudgetInfo(UI.Utils.GetFormInteger(ddlBudgetSource.SelectedValue))) : (IBudget)(new ClientVariationTradeInfo(UI.Utils.GetFormInteger(ddlBudgetSource.SelectedValue)));
            }
            else
            {
                variationInfo.BudgetProvider = null;
            }
        }

        private void ObjectsToForm()
        {
            TradesController tradesController = TradesController.GetInstance();
            List<TradeTemplateInfo> tradeTemplateInfoList = new List<TradeTemplateInfo>();

            if (variationInfo.Id == null)
            {
                TitleBar.Title = "Adding Variation";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
                TitleBar.Title = "Updating Variation";

            txtHeader.Text = UI.Utils.SetFormString(variationInfo.Header);
            txtDescription.Text = UI.Utils.SetFormString(variationInfo.Description);
            txtAmount.Text = UI.Utils.SetFormEditDecimal(variationInfo.Amount);
            txtAllowance.Text = UI.Utils.SetFormEditDecimal(variationInfo.Allowance);

            Utils.GetConfigListAddEmpty("Global", "VariationTypes", ddlType, null);

            if (useBudget)
            {
                BindVariationType();
                BindVariationBudget();

                if (variationInfo.Type != null)
                    ddlType.SelectedValue = variationInfo.Type;

                if (variationInfo.BudgetProvider != null)
                    ddlBudgetSource.SelectedValue = variationInfo.BudgetProvider.Id.Value.ToString();
            }
            else
            {
                tradeTemplateInfoList = tradesController.GetTradeTemplates();
                ddlTrade.Items.Add(new ListItem("", ""));
                ddlTrade.Items.Add(new ListItem(variationInfo.Contract.Trade.Code + " " + variationInfo.Contract.Trade.Name, variationInfo.Contract.Trade.Code));

                foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                    if (tradeTemplateInfo.TradeCode != variationInfo.Contract.Trade.Code)
                        if (tradeTemplateInfo.TradeCode.Substring(0, 2) == variationInfo.Contract.Trade.Code.Substring(0, 2))
                            ddlTrade.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName, tradeTemplateInfo.TradeCode));

                foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                    if (tradeTemplateInfo.TradeCode != variationInfo.Contract.Trade.Code)
                        if (tradeTemplateInfo.TradeCode.Substring(0, 2) != variationInfo.Contract.Trade.Code.Substring(0, 2))
                            ddlTrade.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName, tradeTemplateInfo.TradeCode));

                ddlCVNumber.Items.Add(new ListItem(String.Empty, String.Empty));
                foreach (ClientVariationInfo clientVariationInfo in variationInfo.Contract.Trade.Project.ClientVariations)
                    ddlCVNumber.Items.Add(new ListItem("CV " + UI.Utils.SetFormInteger(clientVariationInfo.Number) + " " + clientVariationInfo.Name, clientVariationInfo.Number.ToString()));

                ddlSANumber.Items.Add(new ListItem(String.Empty, String.Empty));
                foreach (ClientVariationInfo clientVariationInfo in variationInfo.Contract.Trade.Project.SeparateAccounts)
                    ddlSANumber.Items.Add(new ListItem("SA " + UI.Utils.SetFormInteger(clientVariationInfo.Number) + " " + clientVariationInfo.Name, clientVariationInfo.Number.ToString()));


                //--#--TV---

                ddlTVNumber.Items.Add(new ListItem(String.Empty, String.Empty));
                foreach (ClientVariationInfo clientVariationInfo in variationInfo.Contract.Trade.Project.TenantVariations)
                    ddlTVNumber.Items.Add(new ListItem("TV " + UI.Utils.SetFormInteger(clientVariationInfo.Number) + " " + clientVariationInfo.Name, clientVariationInfo.Number.ToString()));


                //--#--TV---



                if (variationInfo.TradeCode != null)
                    ddlTrade.SelectedValue = variationInfo.TradeCode;
            }
        }

        private void FormToObjects()
        {
            String variationType = UI.Utils.GetFormString(ddlType.SelectedValue);

            variationInfo.Header = UI.Utils.GetFormString(txtHeader.Text);
            variationInfo.Description = UI.Utils.GetFormString(txtDescription.Text);
            variationInfo.Amount = UI.Utils.GetFormDecimal(txtAmount.Text);
            variationInfo.Allowance = UI.Utils.GetFormDecimal(txtAllowance.Text);
            variationInfo.Type = variationType;

            if (useBudget)
            {
                // When using budget module a budget source must be selected unless the variation type is backcharge
                if (ddlBudgetSource.SelectedValue != String.Empty)
                {
                    IBudget selectedBudgetProvider = variationInfo.IsBOQ ? (IBudget)(new BudgetInfo(UI.Utils.GetFormInteger(ddlBudgetSource.SelectedValue))) : (IBudget)(new ClientVariationTradeInfo(UI.Utils.GetFormInteger(ddlBudgetSource.SelectedValue)));
                    IBudget projectBudgetProvider = projectBudget.Find(pb => pb.Equals(selectedBudgetProvider));

                    variationInfo.BudgetProvider = projectBudgetProvider;
                }
                else
                {
                    variationInfo.TradeCode = variationInfo.Contract.Trade.Code;
                    variationInfo.BudgetProvider = null;
                }
            }
            else
            {
                variationInfo.TradeCode = UI.Utils.GetFormString(ddlTrade.SelectedValue);

                switch (variationType)
                {
                    case VariationInfo.VariationTypeClient:
                        variationInfo.Number = UI.Utils.GetFormString(ddlCVNumber.SelectedValue);
                        break;
                    case VariationInfo.VariationTypeSeparateAccounts:
                        variationInfo.Number = UI.Utils.GetFormString(ddlSANumber.SelectedValue);
                        break;

                    //---#-------TV---
                    case VariationInfo.VariationTypeTenant:
                        variationInfo.Number = UI.Utils.GetFormString(ddlTVNumber.SelectedValue);
                        break;
                    //-----#-------

                    default:
                        variationInfo.Number = UI.Utils.GetFormString(txtNumber.Text);
                        break;
                }
            }
        }










        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterVariationId = Request.Params["VariationId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditContact);

                if (parameterVariationId != null)
                {
                    variationInfo = contractsController.GetVariation(Int32.Parse(parameterVariationId));
                    Core.Utils.CheckNullObject(variationInfo, parameterVariationId, "Variation");

                    useBudget = variationInfo.TradeCode == null;
                }
                else
                {
                    String parameterSubContractId = Utils.CheckParameter("SubContractId");
                    ContractInfo contractInfo = contractsController.GetContract(Int32.Parse(parameterSubContractId));
                    Core.Utils.CheckNullObject(contractInfo, parameterSubContractId, "Variation Order");

                    variationInfo = new VariationInfo();
                    variationInfo.Contract = contractInfo;
                }

                variationInfo.Contract.Trade = tradesController.GetTrade(variationInfo.Contract.Trade.Id);
                processController.CheckEditCurrentUser(variationInfo.Contract);

                if (useBudget)
                {
                    projectBudget = projectsController.GetProjectBudget(variationInfo.Contract.Trade.Project, sbiTrade.IncludeAllCVSA, sbiTrade.IncludeAllOVO);
                    phBudget.Visible = true;
                    sbiTrade.IncludeClicked += new EventHandler(sbiTrade_IncludeClicked);
                    //#---
                    SanprojectBudget = projectsController.GetProjectBudget(variationInfo.Contract.Trade.Project, false, false);
                    //#---
                }
                else
                {
                    variationInfo.Contract.Trade.Project.ClientVariations = projectsController.GetClientVariationsLastRevisions(variationInfo.Contract.Trade.Project);
                    variationInfo.Contract.Trade.Project.SeparateAccounts = projectsController.GetSeparateAccountsLastRevisions(variationInfo.Contract.Trade.Project);
                    variationInfo.Contract.Trade.Project.TenantVariations = projectsController.GetTenantVariationsLastRevisions(variationInfo.Contract.Trade.Project);

                    phTrade.Visible = true;
                    phCVNumber.Visible = true;
                }

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

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            String variationType = UI.Utils.GetFormString(ddlType.SelectedValue);

            ddlCVNumber.Visible = false;
            ddlSANumber.Visible = false;
            ddlTVNumber.Visible = false;  //--#-------TV  
            txtNumber.Visible = false;
            lblNumber.Visible = false;

            ddlCVNumber.SelectedValue = String.Empty;
            ddlSANumber.SelectedValue = String.Empty;
            ddlTVNumber.SelectedValue = String.Empty;//--#-------TV

            txtNumber.Text = String.Empty;

            if (variationInfo.Type == null || variationInfo.Type != variationType)
                variationInfo.Number = null;

            variationInfo.Type = variationType;
            variationInfo.BudgetProvider = null;

            BindVariationType();
        }

        protected void ddlBudgetSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            variationInfo.Type = UI.Utils.GetFormString(ddlType.SelectedValue);

            BindVariationSource();
            BindVariationBudget();
        }

        protected void sbiTrade_IncludeClicked(object sender, EventArgs e)
        {
            variationInfo.Type = UI.Utils.GetFormString(ddlType.SelectedValue);

            BindVariationSource();
            BindVariationBudget();
        }

        protected void valBudgetSource_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlBudgetSource.SelectedValue != String.Empty || ddlType.SelectedValue == VariationInfo.VariationTypeBackCharge;
        }

        protected void valRealtedTrade_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlType.SelectedValue == VariationInfo.VariationTypeBackCharge)
                if (ddlRelatedTrade.SelectedValue == String.Empty)
                    args.IsValid = false;
                else
                    args.IsValid = (useBudget && ddlRelatedTrade.SelectedValue != variationInfo.Contract.Trade.Code) || (!useBudget && ddlRelatedTrade.SelectedValue != ddlTrade.SelectedValue);
            else
                args.IsValid = true;
        }

        protected void valBOQAllowance_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (useBudget)
            {


                if (ddlType.SelectedValue != VariationInfo.VariationTypeBackCharge)
                    if (UI.Utils.GetFormDecimal(txtAllowance.Text) == null)
                    {
                        valBOQAllowance.ErrorMessage = "Required field.<br />";
                        args.IsValid = false;
                    }

                    //#----

                    else if (UI.Utils.GetFormDecimal(txtAllowance.Text) != null && UI.Utils.GetFormDecimal(lblUnallocated.Text) != null)
                    {
                        if (UI.Utils.GetFormDecimal(txtAllowance.Text) > 0 && UI.Utils.GetFormDecimal(lblUnallocated.Text) <= 0)
                        {
                            valBOQAllowance.ErrorMessage = "Unallocated budget is <= 0.<br />";
                            args.IsValid = false;
                        }

                        else if (UI.Utils.GetFormDecimal(lblUnallocated.Text) > 0 && UI.Utils.GetFormDecimal(txtAllowance.Text) > UI.Utils.GetFormDecimal(lblUnallocated.Text))
                        {
                            valBOQAllowance.ErrorMessage = "Must be <= $" + UI.Utils.GetFormDecimal(lblUnallocated.Text) + ".<br />";
                            args.IsValid = false;
                        }

                    }
                    //#----

                    else
                        args.IsValid = true;
                else
                    if (UI.Utils.GetFormDecimal(txtAllowance.Text) != null)
                {
                    valBOQAllowance.ErrorMessage = "Not Required for back charge.<br />";
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;

            }// if (useBudget)

            else
            {
                if (ddlType.SelectedValue == VariationInfo.VariationTypeBillOfQuantities)
                    if (UI.Utils.GetFormDecimal(txtAllowance.Text) == null)
                    {
                        valBOQAllowance.ErrorMessage = "Required field.<br />";
                        args.IsValid = false;
                    }
                    else
                        args.IsValid = true;
                else
                    if (UI.Utils.GetFormDecimal(txtAllowance.Text) != null)
                {
                    valBOQAllowance.ErrorMessage = "Only for BOQ.<br />";
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;

            }

        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();

            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();

                    if (variationInfo.Id == null)
                    {
                        variationInfo.Id = contractsController.AddVariation(variationInfo);

                        if (variationInfo.Type == VariationInfo.VariationTypeBackCharge)
                        {
                            ProjectsController projectsController = ProjectsController.GetInstance();
                            TradesController tradesController = TradesController.GetInstance();

                            ContractInfo subContract = new ContractInfo();
                            VariationInfo newVariationInfo = new VariationInfo();
                            TradeInfo tradeInfo = tradesController.GetTrade(Int32.Parse(ddlRelatedTrade.SelectedValue));
                            tradeInfo.Project = projectsController.GetProject(tradeInfo.Project.Id);
                            ContractTemplateInfo contractTemplateInfo = contractsController.GetContractTemplate(tradeInfo);

                            tradeInfo.Contract.Subcontracts = contractsController.GetSubContracts(tradeInfo.Contract);

                            subContract.WriteDate = DateTime.Now;

                            subContract.SubcontractNumber = tradeInfo.Contract.NextSubContractNumber;
                            subContract.ParentContract = tradeInfo.Contract;
                            subContract.Trade = tradeInfo;

                            subContract.GoodsServicesTax = variationInfo.Contract.GoodsServicesTax;
                            subContract.Description = variationInfo.Contract.Description;
                            subContract.SiteInstruction = variationInfo.Contract.SiteInstruction;
                            subContract.SiteInstructionDate = variationInfo.Contract.SiteInstructionDate;
                            subContract.SubContractorReference = variationInfo.Contract.SubContractorReference;
                            subContract.SubContractorReferenceDate = variationInfo.Contract.SubContractorReferenceDate;

                            if (contractTemplateInfo != null)
                                subContract.Template = contractTemplateInfo.VariationTemplate;
                            else
                                subContract.Template = ContractsController.TagEditableOpen + "NoTemplate " + ContractsController.TagTitle + "No Template" + ContractsController.TagEditableEnd + "No template defined." + ContractsController.TagEditableClose;

                            subContract.Id = contractsController.AddContract(subContract);

                            newVariationInfo.Header = variationInfo.Header;
                            newVariationInfo.Description = variationInfo.Description;
                            newVariationInfo.Amount = -variationInfo.Amount;
                            newVariationInfo.TradeCode = variationInfo.TradeCode;
                            newVariationInfo.Number = variationInfo.Number;
                            newVariationInfo.Type = variationInfo.Type;

                            newVariationInfo.Contract = subContract;

                            newVariationInfo.Id = contractsController.AddVariation(newVariationInfo);
                        }
                    }
                    else
                    {
                        contractsController.UpdateVariation(variationInfo);
                    }
                    //#---Added new variation  process approval condition and new approval process CA/PM/CM/DM/COM/UM/DA based on variation Type and  amount

                    if (variationInfo.Contract.Trade.JobTypeName == "Construction")
                    {
                        ContractInfo contractInfo = variationInfo.Contract;
                        ProcessController.GetInstance().UpdateVariationContractProcessSteps(variationInfo);
                    }
                    //#----

                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/ViewSubContract.aspx?SubContractId=" + variationInfo.Contract.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/ViewSubContract.aspx?SubContractId=" + variationInfo.Contract.IdStr);
        }
        #endregion

    }
}
