using System;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SOS.Core;

namespace SOS.Web
{
    public partial class ViewSubContractPage : SOSPage
    {

#region Members
        private ContractInfo subContract;
        List<IBudget> projectBudget;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (subContract == null)
                return null;

            tempNode.ParentNode.Url += "?ContractId=" + subContract.ParentContract.IdStr;

            tempNode.ParentNode.ParentNode.Url += "?ContractId=" + subContract.ParentContract.IdStr;
            
            tempNode.ParentNode.ParentNode.ParentNode.Title = subContract.Trade.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?TradeId=" + subContract.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Title = subContract.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + subContract.Trade.Project.IdStr;

            return currentNode;
        }

        private void BindApproval()
        {
            ProcessManagerSubcontract.Process = subContract.Process;
            ProcessManagerSubcontract.ApproveClicked += new System.EventHandler(cmdApprove_Click);
            ProcessManagerSubcontract.ReverseClicked += new System.EventHandler(cmdReverse_Click);

            ProcessManagerSubcontract.BindApproval();
           

        }

        private void BindVariations()
        {
          
            gvVariations.DataSource = subContract.Variations;
            gvVariations.DataBind();
        }

        private void BindSubContract()
        {
            ProcessController processController = ProcessController.GetInstance();
            Boolean isEditable;

            if (Security.ViewAccess(Security.userActions.EditContact))
            {
                isEditable = processController.AllowEditCurrentUser(subContract);

                cmdEditTop.Visible = isEditable;
                cmdDeleteTop.Visible = isEditable;

                if (isEditable)
                    cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Variation Order ?');");

                phAddVariation.Visible = isEditable;

                gvVariations.Columns[0].Visible = isEditable;
                gvVariations.Columns[1].Visible = isEditable;
                //#----To disable Edit and Add variations after CM approval

                ProcessStepInfo processStepInfo = processController.GetCurrentStep(subContract.Process);
                if (processStepInfo != null && subContract.Process.Steps.Count>6)
                if(subContract.Process.Steps[2].Role=="CM" && subContract.Process.Steps[2].Status=="Approved")// && processStepInfo.Role != "CA" && processStepInfo.Role != "PM" && processStepInfo.Role != "CM" && processStepInfo.Role != "DC")
                {
                    gvVariations.Columns[0].Visible = false;
                    gvVariations.Columns[1].Visible = false;
                    phAddVariation.Visible = false;
                }

                //#---To disable Edit and Add variations after CM approval

            }

            lblSubcontractor.Text = UI.Utils.GetFormString(subContract.Trade.SelectedSubContractorName);
            lblNumber.Text = UI.Utils.SetFormInteger(subContract.SubcontractNumber);
            lblTitle.Text = UI.Utils.SetFormString(subContract.Description);
            lblGoodsServicesTax.Text = UI.Utils.SetFormPercentage(subContract.GoodsServicesTax);
            lblQuotesFileName.Text = UI.Utils.SetFormString(subContract.QuotesFile);
            lblSiteInstruction.Text = UI.Utils.SetFormString(subContract.SiteInstruction);
            lblSiteInstructionDate.Text = UI.Utils.SetFormDate(subContract.SiteInstructionDate);
            lblSubContractorReference.Text = UI.Utils.SetFormString(subContract.SubContractorReference);
            lblSubContractorReferenceDate.Text = UI.Utils.SetFormDate(subContract.SubContractorReferenceDate);
            lblComments.Text = UI.Utils.SetFormString(subContract.Comments);
            chkOrigContractDur.Enabled = false;   // DS20240321
            chkOrigContractDur.Checked = UI.Utils.SetFormBoolean(subContract.CheckOrigContractDur);    // DS20240321
            lblStartDate.Text = UI.Utils.SetFormDate(subContract.StartDate);    // DS20240321
            lblFinishDate.Text = UI.Utils.SetFormDate(subContract.FinishDate);  // DS20240321
            //chkOrigContractDur
            sflQuotesFile.FilePath = subContract.QuotesFile;
            sflQuotesFile.BasePath = subContract.Trade.Project.AttachmentsFolder;
            sflQuotesFile.PageLink = String.Format("~/Modules/Projects/ShowContractQuotesFile.aspx?ContractId={0}", subContract.IdStr);

            lnkAddVariation.NavigateUrl = "~/Modules/Contracts/EditVariation.aspx?SubContractId=" + subContract.IdStr;

            if (subContract.IsApproved)
            {
                phViewContract.Visible = true;
                lnkViewContract.NavigateUrl = "~/Modules/Contracts/ViewContract.aspx?ContractId=" + subContract.IdStr;
            }
            else
            {
                XmlDocument xmlDocument = ContractsController.GetInstance().CheckTradeForTemplate(subContract.Trade, subContract.Template);
                if (xmlDocument.DocumentElement != null)
                {
                    Utils.AddNode(xmlDocument.DocumentElement, TreeView1.Nodes[0]);
                    pnlErrors.Visible = true;
                    TreeView1.Nodes[0].Expanded = false;
                }
            }

            ProcessManagerSubcontract.StepId = null;

            BindApproval();
            BindVariations();
        }

        protected String TotalVariations()
        {
            return UI.Utils.SetFormDecimal(subContract.TotalVariations);
        }

        protected String TotalAllocations()
        {
            return UI.Utils.SetFormDecimal(subContract.TotalAllocations);
        }

        protected String TotalWinLoss()
        {
            return UI.Utils.SetFormDecimal(subContract.TotalWinLoss);
        }

        protected String StyleName(decimal? allowanceMinusAmount)
        {
            if (allowanceMinusAmount != null)
                if (allowanceMinusAmount < 0)
                    return "lstItemError";

            return String.Empty;
        }

        protected void AssignCVAllownaces()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ClientVariationInfo clientVariationInfo = null;
            int variationNumber;
            decimal tradeAllowance;

            if (subContract.Variations != null)
                foreach (VariationInfo variationInfo in subContract.Variations)
                    if (variationInfo.BudgetProvider == null && (variationInfo.Type == VariationInfo.VariationTypeClient || variationInfo.Type == VariationInfo.VariationTypeSeparateAccounts))
                    {
                        tradeAllowance = 0;

                        if (variationInfo.TradeCode != null && variationInfo.Number != null && int.TryParse(variationInfo.Number, out variationNumber))
                        {
                            if (variationInfo.Type == VariationInfo.VariationTypeClient)
                                clientVariationInfo = projectsController.GetClientVariationByNumberWithItemsAndTrades(variationNumber, subContract.Trade.Project, ClientVariationInfo.VariationTypeClient);
                            else
                                clientVariationInfo = projectsController.GetClientVariationByNumberWithItemsAndTrades(variationNumber, subContract.Trade.Project, ClientVariationInfo.VariationTypeSeparateAccounts);

                            if (clientVariationInfo != null && clientVariationInfo.Trades != null)
                                foreach (ClientVariationTradeInfo clientVariationTradeInfo in clientVariationInfo.Trades)
                                    if (clientVariationTradeInfo.TradeCode != null && clientVariationTradeInfo.TradeCode == variationInfo.TradeCode && clientVariationTradeInfo.Amount != null)
                                        tradeAllowance = tradeAllowance + (decimal)clientVariationTradeInfo.Amount;
                        }

                        variationInfo.CVAllowance = tradeAllowance;
                    }
                    else
                        variationInfo.CVAllowance = null;
        }

        protected String CVSAStatus(VariationInfo variationInfo)
        {
            int variationNumber;
            ClientVariationInfo clientVariationInfo = null;

            if (variationInfo.Type != null && (variationInfo.Type == VariationInfo.VariationTypeClient || variationInfo.Type == VariationInfo.VariationTypeSeparateAccounts))
            {
                if (variationInfo.BudgetProvider != null)
                {
                    clientVariationInfo = ((ClientVariationTradeInfo)variationInfo.BudgetProvider).ClientVariation;
                }
                else
                {
                    if (variationInfo.Number != null && int.TryParse(variationInfo.Number, out variationNumber))
                    {
                        if (variationInfo.Type == VariationInfo.VariationTypeClient)
                            clientVariationInfo = ProjectsController.GetInstance().GetClientVariationByNumber(variationNumber, variationInfo.Contract.Trade.Project, ClientVariationInfo.VariationTypeClient);
                        else
                            clientVariationInfo = ProjectsController.GetInstance().GetClientVariationByNumber(variationNumber, variationInfo.Contract.Trade.Project, ClientVariationInfo.VariationTypeSeparateAccounts);
                    }
                }
            }

            if (clientVariationInfo != null)
                return clientVariationInfo.Status;
            else
                return String.Empty;
        }

        protected String BudgetNameInfo(VariationInfo variationInfo)
        {
            int variationNumber;
            IBudget iBudget;

            if (variationInfo.BudgetProvider != null)
            {
                return variationInfo.BudgetProvider.BudgetName;
            }
            else
            {
                if (variationInfo.Type != null && (variationInfo.IsCV || variationInfo.IsSA))
                {
                    if (variationInfo.Number != null && int.TryParse(variationInfo.Number, out variationNumber))
                    {
                        return UI.Utils.SetFormInteger(variationNumber);
                    }
                    else
                    {
                        return variationInfo.Number;
                    }
                }
                else
                {
                    if (variationInfo.IsBOQ)
                    {
                        iBudget = projectBudget.Find(pb => pb.TradeCode == variationInfo.TradeCode);

                        if (iBudget != null)
                        {
                            return iBudget.BudgetName;
                        }
                        else
                        {
                            return String.Empty;
                        }
                    }
                    else
                    {
                        return "N/A";
                    }
                }
            }
        }

        protected String BudgetTypeInfo(VariationInfo variationInfo)
        {
            if (variationInfo.BudgetProvider != null)
            {
                return variationInfo.BudgetProvider.BudgetType.ToString();
            }
            else
            {
                if (variationInfo.Type != null)
                {
                    if (variationInfo.Type != VariationInfo.VariationTypeBackCharge)
                    {
                        return variationInfo.Type;
                    }
                    else
                    {
                        return "N/A";
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        protected String BudgetOriginalInfo(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? variationInfo.Contract.IsApproved ? SOS.UI.Utils.SetFormDecimal(variationInfo.BudgetAmountInitial) : String.Empty : "N/A";
        }

        protected String BudgetUnallocatedlInfo(VariationInfo variationInfo)
        {
            
            //#--return variationInfo.BudgetProvider != null ? variationInfo.Contract.IsApproved ? SOS.UI.Utils.SetFormDecimal(variationInfo.BudgetProvider.BudgetUnallocated) : String.Empty : "N/A";

            return variationInfo.BudgetProvider != null ? variationInfo.Contract.IsApproved ? variationInfo.BudgetProvider.BudgetUnallocated >0 ? SOS.UI.Utils.SetFormDecimal(variationInfo.BudgetProvider.BudgetUnallocated) : "$0.00" : String.Empty : "N/A";

            //#---

        }

        protected String TradeBudgetOriginalInfo(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? variationInfo.Contract.IsApproved ? SOS.UI.Utils.SetFormDecimal(variationInfo.BudgetAmountTradeInitial) : String.Empty : "N/A";
        }

        protected String TradeBudgetCurrentInfo(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? SOS.UI.Utils.SetFormDecimal(variationInfo.BudgetProvider.BudgetAmountTradeInitial) : "N/A";
        }

        protected String BudgetOriginalStyleName(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? StyleName(variationInfo.BudgetAmountInitial) : String.Empty;
        }

        protected String BudgetUnallocatedlStyleName(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? StyleName(variationInfo.BudgetProvider.BudgetUnallocated) : String.Empty;
        }

        protected String TradeBudgetOriginalStyleName(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? StyleName(variationInfo.BudgetAmountTradeInitial) : String.Empty;
        }

        protected String TradeBudgetCurrentStyleName(VariationInfo variationInfo)
        {
            return variationInfo.BudgetProvider != null ? StyleName(variationInfo.BudgetProvider.BudgetAmountTradeInitial) : String.Empty;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterSubContractId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterSubContractId = Utils.CheckParameter("SubContractId");
                subContract = contractsController.GetContractWithVariations(Int32.Parse(parameterSubContractId));
                Core.Utils.CheckNullObject(subContract, parameterSubContractId, "Variation Order");

                subContract.Trade = tradesController.GetDeepTradeWithBudget(subContract.Trade.Id);
                subContract.Trade.Contract = subContract;
                subContract.Trade.Project = projectsController.GetProject(subContract.Trade.Project.Id);
                subContract.Trade.Project.Trades = new List<TradeInfo>();
                subContract.Trade.Project.Trades.Add(subContract.Trade);
                subContract.Trade.Project.Drawings = tradesController.GetDeepDrawings(subContract.Trade.Project);

                projectBudget = projectsController.GetProjectBudget(subContract.Trade.Project, sbiTrade.IncludeAllCVSA, sbiTrade.IncludeAllOVO);
                sbiTrade.IncludeClicked += new EventHandler(sbiTrade_IncludeClicked);

                foreach (VariationInfo variationInfo in subContract.Variations)
                    if (variationInfo.BudgetProvider != null)
                        variationInfo.BudgetProvider = projectBudget.Find(pb => pb.Equals(variationInfo.BudgetProvider));

                if (subContract.Variations != null)
                    foreach (VariationInfo variationInfo in subContract.Variations)
                        variationInfo.Contract = subContract;

                AssignCVAllownaces();

                if (subContract.Trade.ProjectManager != null)
                    subContract.Trade.Project.ProjectManager = subContract.Trade.ProjectManager;

                if (subContract.Trade.ContractsAdministrator != null)
                    subContract.Trade.Project.ContractsAdministrator = subContract.Trade.ContractsAdministrator;

                if (subContract.Process != null)
                    subContract.Process.Project = subContract.Trade.Project;

                //#----Quote File link  was missing after approval
                if (subContract.QuotesFile != null)
                {
                    sflQuotesFile.FilePath = subContract.QuotesFile;
                    sflQuotesFile.BasePath = subContract.Trade.Project.AttachmentsFolder;
                    sflQuotesFile.PageLink = String.Format("~/Modules/Projects/ShowContractQuotesFile.aspx?ContractId={0}", subContract.IdStr);
                }
               //#----Quote File link  was missing after approval



                if (!Page.IsPostBack)
                    BindSubContract();
                else
                    BindApproval();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/EditSubcontract.aspx?SubContractId=" + subContract.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ContractsController.GetInstance().DeleteContract(subContract);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Contracts/ListSubContracts.aspx?ContractId=" + subContract.ParentContract.IdStr);
        }

        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = subContract.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerSubcontract.StepId; });

                if (currentStep.Process.StepContractApproval != null)
                    if (currentStep.Process.StepContractApproval == currentStep.Type)
                    {
                        ContractsController contractsController = ContractsController.GetInstance();
                        contractsController.BuildContract(subContract.Trade);
                        contractsController.UpdateTemplateInContract(subContract);
                    }

                processController.ExecuteProcessStep(currentStep, ProcessManagerSubcontract.Comments);

                subContract.Process = processController.GetDeepProcess(subContract.Process.Id);
                subContract.Process.Project = subContract.Trade.Project;

                BindSubContract();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdReverse_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = subContract.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerSubcontract.StepId; });
                //#--  processController.ReverseProcessStep(currentStep, ProcessManagerSubcontract.Comments)
               processController.ReverseProcessStep(currentStep, ProcessManagerSubcontract.Comments,subContract.Trade.Name+" - "+subContract.SubcontractNumber.ToString());//#---Passed new parameter Trade name
               subContract.Process = processController.GetDeepProcess(subContract.Process.Id);
               subContract.Process.Project = subContract.Trade.Project;

                BindSubContract();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvVariations_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                ContractsController contractsController = ContractsController.GetInstance();
                int VariationId = (int)gvVariations.DataKeys[e.RowIndex].Value;
                contractsController.DeleteVariation(new VariationInfo(VariationId));
                subContract.Variations = contractsController.GetVariations(subContract);
                AssignCVAllownaces();
                BindVariations();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void sbiTrade_IncludeClicked(object sender, EventArgs e)
        {
            BindVariations();
        }

        protected void gvVariations_DataBound(object sender, EventArgs e)
        {
            TableHeaderCell tableHeaderCell;
            GridView gridView = (GridView)sender;

            if (gridView.Controls.Count > 0)
            {
                Table table = (Table)gridView.Controls[0];
                GridViewRow gridViewRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.Text = String.Empty;
                tableHeaderCell.ColumnSpan = ProcessController.GetInstance().AllowEditCurrentUser(subContract) ? 5 : 3;
                gridViewRow.Cells.Add(tableHeaderCell);

                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 5;
                tableHeaderCell.CssClass = "lstHeader";
                tableHeaderCell.Text = "Budget Source";
                gridViewRow.Cells.Add(tableHeaderCell);

                /* Uncomment to show trade codes allocation
                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.ColumnSpan = 2;
                tableHeaderCell.CssClass = "lstHeader";
                tableHeaderCell.Text = "Revised Budget";
                gridViewRow.Cells.Add(tableHeaderCell);
                */

                tableHeaderCell = new TableHeaderCell();
                tableHeaderCell.Text = String.Empty;
                tableHeaderCell.ColumnSpan = 4;
                gridViewRow.Cells.Add(tableHeaderCell);

                table.Rows.AddAt(0, gridViewRow);
            }
        }
#endregion

    }
}
