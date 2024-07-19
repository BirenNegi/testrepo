using System;
using System.IO;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewProjectTradePage : SOSPage
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

            tempNode.ParentNode.Title = tradeInfo.Project.Name + (tradeInfo.Project.IsStatusProposal ? " (Proposal)" : ""); ;
            tempNode.ParentNode.Url += "?ProjectId=" + tradeInfo.Project.IdStr;

            return currentNode;
        }

        private void BindTrade()
        {
            ProcessController processController = ProcessController.GetInstance();
            Boolean isEditable = false;
            String deleteMessage;

            if (tradeInfo.Project.IsStatusProposal)
            {
                cpe1.Collapsed = false;
                pnlStatusActive.Visible = false;
            }

            if (Security.ViewAccess(Security.userActions.EditTrade))
            {
                isEditable = processController.AllowEditCurrentUser(tradeInfo.Process);

                if (isEditable)
                {
                    if (tradeInfo.Contract != null)
                        if (tradeInfo.Contract.NumSubContracts > 0)
                            deleteMessage = ", Contract and " + tradeInfo.Contract.NumSubContracts + " Variation Orders ";
                        else
                            deleteMessage = " and Contract ";
                    else
                        deleteMessage = " ";

                    cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade" + deleteMessage + "from the Project?');");
                }

                cmdEditTop.Visible = isEditable;
                cmdDeleteTop.Visible = isEditable;

                phAddItemCategory.Visible = isEditable;
                gvItemCategories.Columns[4].Visible = isEditable;
                gvItemCategories.Columns[5].Visible = isEditable;

                gvParticipantsProposal.Columns[1].Visible = isEditable && tradeInfo.Project.Status == ProjectInfo.StatusActive;

                phAddDrawingTypes.Visible = isEditable;
                gvDrawingTypes.Columns[0].Visible = isEditable;

                phAddParticipant.Visible = isEditable;

                phAddParticipantProposal.Visible = isEditable;

                cmdDrawings.Visible = isEditable;
                cmdDrawingsProposal.Visible = isEditable;

                phAddAddendum.Visible = isEditable;
            }

            if (tradeInfo.ContractApproved)
                TitleBar1.Info = "Contract Approved. <b>" + tradeInfo.SelectedSubContractorName + "</b>";
            else
                if (tradeInfo.SelectedParticipation != null)
                TitleBar1.Info = "Contract to be Raised. " + tradeInfo.SelectedSubContractorName;
            else
                TitleBar1.Info = "Contract to be Raised.";

            //#--23-11-2018--  lnkAddItemCategory.NavigateUrl = "~/Modules/Projects/EditProjectTradeItemCategory.aspx?TradeId=" + tradeInfo.IdStr;

            lnkAddItemCategory.NavigateUrl = "~/Modules/Projects/EditItemCategories_Items_ScopeOfWork.aspx?TradeId=" + tradeInfo.IdStr;

            //#--23-11-2018--

            lnkComparison.NavigateUrl = String.Format("~/Modules/Projects/ViewComparison.aspx?Type={0}&TradeId={1}", Info.TypeActive, tradeInfo.IdStr);
            lnkComparisonProposal.NavigateUrl = String.Format("~/Modules/Projects/ViewComparison.aspx?Type={0}&TradeId={1}", Info.TypeProposal, tradeInfo.IdStr);

            lnkCheckList.NavigateUrl = String.Format("~/Modules/Projects/ShowCheckList.aspx?Type={0}&TradeId={1}", Info.TypeActive, tradeInfo.IdStr);
            lnkCheckListProposal.NavigateUrl = String.Format("~/Modules/Projects/ShowCheckList.aspx?Type={0}&TradeId={1}", Info.TypeProposal, tradeInfo.IdStr);

            lnkAddAddendum.NavigateUrl = "~/Modules/Projects/EditAddendum.aspx?TradeId=" + tradeInfo.IdStr;

            lblName.Text = UI.Utils.SetFormString(tradeInfo.Name);
            lblJobType.Text = tradeInfo.JobType.Name;
            lblCode.Text = UI.Utils.SetFormString(tradeInfo.Code);
            sbvTenderRequired.Checked = tradeInfo.TenderRequired;
            lblDescription.Text = UI.Utils.SetFormString(tradeInfo.Description);
            lblDaysFromPCD.Text = UI.Utils.SetFormInteger(tradeInfo.DaysFromPCD);
            txtScopeHeader.Text = UI.Utils.SetFormString(tradeInfo.ScopeHeader);
            txtScopeFooter.Text = UI.Utils.SetFormString(tradeInfo.ScopeFooter);
            lblQuotesFileName.Text = UI.Utils.SetFormString(tradeInfo.QuotesFile);
            lblPrelettingFileName.Text = UI.Utils.SetFormString(tradeInfo.PrelettingFile);

            //#-----Signed Contract File

            lblSignedContractFile.Text= UI.Utils.SetFormString(tradeInfo.SignedContractFile);

            //#------------------

            lblInvitationDate.Text = UI.Utils.SetFormDate(tradeInfo.InvitationDate);
            lblQuotesDueDate.Text = UI.Utils.SetFormDate(tradeInfo.DueDate);
            lblComparisonDueDate.Text = UI.Utils.SetFormDate(tradeInfo.ComparisonDueDate);
            lblContractDueDate.Text = UI.Utils.SetFormDate(tradeInfo.ContractDueDate);
            lblCommencementDate.Text = UI.Utils.SetFormDate(tradeInfo.CommencementDate);
            lblCompletionDate.Text = UI.Utils.SetFormDate(tradeInfo.CompletionDate);
            lblWorkOrder.Text = UI.Utils.SetFormString(tradeInfo.WorkOrderNumber);

            if (tradeInfo.ProjectManager != null)
            {
                lnkPM.Text = UI.Utils.SetFormString(tradeInfo.ProjectManager.Name);
                lnkPM.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + tradeInfo.ProjectManager.IdStr;
            }

            if (tradeInfo.ContractsAdministrator != null)
            {
                lnkCA.Text = UI.Utils.SetFormString(tradeInfo.ContractsAdministrator.Name);
                lnkCA.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + tradeInfo.ContractsAdministrator.IdStr;
            }

            if (tradeInfo.ComparisonApproved)
                if (tradeInfo.ComparisonApprovalAmount != null)
                    if (tradeInfo.SelectedParticipation != null)
                        if ((decimal)tradeInfo.ComparisonApprovalAmount != TradesController.GetInstance().GetQuoteTotal(tradeInfo.SelectedParticipation))
                            lblCheckComparisonAmount.Text = ">> Winning quote amount modified after comparison approval <<";

            if (tradeInfo.Contract != null)
            {
                phContract.Visible = true;
                lnkContract.NavigateUrl = "~/Modules/Contracts/ViewContract.aspx?ContractId=" + tradeInfo.Contract.IdStr;

                //#---to display Foreman Contract
                //if (tradeInfo.ContractApproved)
                //{
                //    phForemanContract.Visible = true;
                //    lnkForemanContract.NavigateUrl = "~/Modules/Contracts/ShowForemanContract.aspx?ContractId=" + tradeInfo.Contract.IdStr;
                //}
                //#-------------

            }


            //#--16-01-2019 -To display DraftContract
            if (tradeInfo.Contract == null && tradeInfo.JobTypeName=="Construction")
            {
                phDraftContract.Visible = true;
                lnkDraftContract.NavigateUrl = "~/Modules/Contracts/ViewDraftContract.aspx?TradeId=" + tradeInfo.IdStr;

            }
            //#--16-01-2019

            if (tradeInfo.WorkOrderNumber != null)
            {
                phPreletting.Visible = true;
                lnkPreletting.NavigateUrl = "~/Modules/Projects/ViewMinutes.aspx?TradeId=" + tradeInfo.IdStr;
            }

            if (tradeInfo.ContractApproved)
            {
                phVariations.Visible = true;
                lnkVariations.NavigateUrl = "~/Modules/Contracts/ListSubContracts.aspx?ContractId=" + tradeInfo.Contract.IdStr;
            }

            //#---22/11/23------------------//

            lblApprovedComparison.Visible = false;
            if (tradeInfo.ComparisonApproved)
            { 
                lblApprovedComparison.Visible = true;  
                lblApprovedComparison.Text=" Comparison Approved Amount  : "+UI.Utils.SetFormDecimal(tradeInfo.ComparisonApprovalAmount).ToString()+"  and  Date : "+ UI.Utils.SetFormDate(tradeInfo.ComparisonApprovalDate);

            }

            //#---22/11/23------------------//

            if (tradeInfo.Flag != null)
                if (tradeInfo.Flag.Value == TradeInfo.FlagRed)
                    imgRedFlag.Visible = true;
                else if (tradeInfo.Flag.Value == TradeInfo.FlagGreen)
                    imgGreenFlag.Visible = true;

            BindItemCategories();
            BindFileLinks();
            BindDrawingTypes();
            BindAddendums();
            BindParticipants();
            BindParticipantsProposal();
        }

        private void BindFileLinks()
        {
            sflQuotesFile.FilePath = tradeInfo.QuotesFile;
            sflQuotesFile.BasePath = tradeInfo.Project.AttachmentsFolder;
            sflQuotesFile.PageLink = String.Format("~/Modules/Projects/ShowTradeQuotesFile.aspx?TradeId={0}", tradeInfo.IdStr);

            sflPrelettingFile.FilePath = tradeInfo.PrelettingFile;
            sflPrelettingFile.BasePath = tradeInfo.Project.AttachmentsFolder;
            sflPrelettingFile.PageLink = String.Format("~/Modules/Projects/ShowTradePrelettingFile.aspx?TradeId={0}", tradeInfo.IdStr);

            sflSignedContractFile.FilePath = tradeInfo.SignedContractFile;
            sflSignedContractFile.BasePath = tradeInfo.Project.AttachmentsFolder;
            sflSignedContractFile.PageLink = String.Format("~/Modules/Projects/ShowTradeSignedContractFile.aspx?TradeId={0}", tradeInfo.IdStr);

        }



        //#---------Subcontractor's Signed Contract File--------
        private void ChkSignedContractFile()
        {
            //TradesController tradesController = TradesController.GetInstance();
            //tradesController.GetDeepTrade(tradeInfo.Id);

            TblSignedContract.Visible = false; // its block or table to update the signed contract file

            sfsSignedContractFile.Path= tradeInfo.Project.AttachmentsFolder; //File uploader control to update the signed contract fil

            sflSignedContractFile.Visible = true; // its link to download the signed contract file

            foreach (ProcessStepInfo prStep in tradeInfo.Contract.Process.Steps)
            {

                //prStep.Role == "CM" condition removed bcz  CA,PM  can also approve the contract //----if (prStep.Role == "CM" && prStep.Status == "Approved" && tradeInfo.SignedContractFile == null)
                if ( prStep.Status == "Approved" && tradeInfo.SignedContractFile == null)
                {
                    TblSignedContract.Visible = true;
                    sflSignedContractFile.Visible = true; // its link to download the signed contract filefile

                }
                else if (tradeInfo.Project.Trades[0].JobTypeName=="Design" && prStep.Role == "DM" && prStep.Status == "Approved" && tradeInfo.SignedContractFile == null)
                {

                    TblSignedContract.Visible = true;
                    sflSignedContractFile.Visible = true; // its link to download the signed contract filefile
                }

                if (Security.IsAdmin() && tradeInfo.SignedContractFile != null)
                {
                    sfsSignedContractFile.FilePath = UI.Utils.SetFormString(tradeInfo.SignedContractFile);
                    TblSignedContract.Visible = true;
                    sflSignedContractFile.Visible = true;
                }

               
            }




        }

        //#---------


        private void BindApproval()
        {
            ProcessManagerTrade.StepId = null;
            ProcessManagerTrade.Process = tradeInfo.Process;
            ProcessManagerTrade.TradeIdStr = tradeInfo.IdStr;
            ProcessManagerTrade.ApproveClicked += new System.EventHandler(cmdApprove_Click);
            ProcessManagerTrade.ReverseClicked += new System.EventHandler(cmdReverse_Click);

            ProcessManagerTrade.BindApproval(); // Calling method in ProcessManager.ascx.cs
        }

        private void BindApprovalContract()
        {
            if (tradeInfo.Contract != null)
            {
                ProcessManagerContract.StepId = null;
                ProcessManagerContract.Process = tradeInfo.Contract.Process;
                ProcessManagerContract.ApproveClicked += new System.EventHandler(cmdApproveContract_Click);
                ProcessManagerContract.ReverseClicked += new System.EventHandler(cmdReverseContract_Click);

                ProcessManagerContract.BindApproval();
            }
        }

        private void BindBudget()
        {
            tradeBudget.Trade = tradeInfo;
            tradeBudget.Updated += new System.EventHandler(budget_Update);
            tradeBudget.IsEditable = Security.ViewAccess(Security.userActions.EditTrade) && ProcessController.GetInstance().AllowEditCurrentUser(tradeInfo.ContractProcess) && !tradeInfo.ContractCreated;
        }

        private void ReBindBudget()
        {
            tradeBudget.IsEditable = Security.ViewAccess(Security.userActions.EditTrade) && ProcessController.GetInstance().AllowEditCurrentUser(tradeInfo.ContractProcess) && !tradeInfo.ContractCreated;
            tradeBudget.ReBind();
        }

        private void BindItemCategories()
        {
            gvItemCategories.DataSource = tradeInfo.ItemCategories;
            gvItemCategories.DataBind();
        }

        private void BindDrawingTypes()
        {
            List<DrawingTypeInfo> drawingTypeInfoList = TradesController.GetInstance().GetDrawingTypes();

            ddlDrawingTypes.Items.Clear();

            foreach (DrawingTypeInfo drawingTypeInfo in drawingTypeInfoList)
                ddlDrawingTypes.Items.Add(new ListItem(drawingTypeInfo.Name, drawingTypeInfo.IdStr));

            foreach (DrawingTypeInfo drawingTypeInfo in tradeInfo.DrawingTypes)
                ddlDrawingTypes.Items.Remove(new ListItem(drawingTypeInfo.Name, drawingTypeInfo.IdStr));

            butAddDrawingType.Enabled = ddlDrawingTypes.Items.Count != 0;

            gvDrawingTypes.DataSource = tradeInfo.DrawingTypes;
            gvDrawingTypes.DataBind();

            BindDrawings();
            tvDrawings.Nodes[0].Expanded = true;

            BindDrawingsProposal();
            tvDrawingsProposal.Nodes[0].Expanded = true;
        }

        private void BindParticipants()
        {
            gvParticipants.DataSource = tradeInfo.SubcontractorsActiveParticipations;
            gvParticipants.DataBind();
        }

        private void BindParticipantsProposal()
        {
            gvParticipantsProposal.DataSource = tradeInfo.SubcontractorsProposalParticipations;
            gvParticipantsProposal.DataBind();
        }

        private void BindAddendums()
        {
            gvAddendums.DataSource = tradeInfo.Addendums;
            gvAddendums.DataBind();
        }

        private void BindDrawings(String drawingType, TreeView tv)
        {
            TradesController tradesController = TradesController.GetInstance();
            List<DrawingInfo> drawingInfoList;
            DrawingInfo drawingInfoSearch;
            TreeNode treeNodeType;
            TreeNode treeNodeDrawing;
            TreeNode treeNodeRevision;

            tv.Nodes.Clear();
            tv.Nodes.Add(new TreeNode("Drawings"));
            tv.Nodes[0].SelectAction = TreeNodeSelectAction.None;
            tv.Nodes[0].Expanded = false;

            foreach (DrawingTypeInfo drawingTypeInfo in tradeInfo.DrawingTypes)
            {
                treeNodeType = new TreeNode(drawingTypeInfo.Name, "", "", String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", drawingType, tradeInfo.Project.IdStr, drawingTypeInfo.IdStr), "");
                treeNodeType.Expanded = false;
                drawingInfoList = tradesController.GetDrawings(tradeInfo.Project, drawingTypeInfo, drawingType);
                foreach (DrawingInfo drawingInfo in drawingInfoList)
                {
                    drawingInfoSearch = tradeInfo.Drawings.Find(delegate (DrawingInfo drawingInfoInList) { return drawingInfoInList.Equals(drawingInfo); });

                    treeNodeDrawing = new TreeNode(drawingInfo.Name, drawingInfo.IdStr, "", "~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingInfo.IdStr, "");
                    treeNodeDrawing.Expanded = false;
                    treeNodeDrawing.ShowCheckBox = true;
                    treeNodeDrawing.Checked = drawingInfoSearch == null;

                    foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
                    {
                        treeNodeRevision = new TreeNode(drawingRevisionInfo.Number + " / " + UI.Utils.SetFormDate(drawingRevisionInfo.RevisionDate), "", "", "~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + drawingRevisionInfo.IdStr, "");
                        treeNodeDrawing.ChildNodes.Add(treeNodeRevision);
                    }
                    treeNodeType.ChildNodes.Add(treeNodeDrawing);
                }

                tv.Nodes[0].ChildNodes.Add(treeNodeType);
            }
        }

        private void BindDrawings()
        {
            BindDrawings(Info.TypeActive, tvDrawings);
        }

        private void BindDrawingsProposal()
        {
            BindDrawings(Info.TypeProposal, tvDrawingsProposal);
        }

        private void BindCopyTree(XmlDocument xmlDocument)
        {
            if (xmlDocument.DocumentElement != null)
            {
                TreeViewCheckCopy.Nodes.Clear();
                TreeViewCheckCopy.Nodes.Add(new TreeNode());
                Utils.AddNode(xmlDocument.DocumentElement, TreeViewCheckCopy.Nodes[0]);
                TreeViewCheckCopy.ExpandAll();
                pnlCopyErrors.Visible = true;
            }
            else
                pnlCopyErrors.Visible = false;
        }

        protected void UpdateDrawings(String drawingType, TreeView tv)
        {
            TradesController tradesController = TradesController.GetInstance();
            List<DrawingInfo> drawingInfoList;
            bool IsChecked;
            bool IsExcluded;

            foreach (DrawingTypeInfo drawingTypeInfo in tradeInfo.DrawingTypes)
            {
                drawingInfoList = tradesController.GetDrawings(tradeInfo.Project, drawingTypeInfo, drawingType);
                foreach (DrawingInfo drawingInfo in drawingInfoList)
                {
                    IsChecked = false;
                    foreach (TreeNode treeNode in tv.CheckedNodes)
                        if (drawingInfo.IdStr == treeNode.Value)
                        {
                            IsChecked = true;
                            break;
                        }

                    IsExcluded = tradeInfo.Drawings.Find(delegate (DrawingInfo drawingInfoInList) { return drawingInfoInList.Equals(drawingInfo); }) != null;

                    if (IsChecked && IsExcluded)
                        tradesController.DeleteTradeDrawing(tradeInfo, drawingInfo);
                    else
                        if (!IsChecked && !IsExcluded)
                        tradesController.AddTradeDrawing(tradeInfo, drawingInfo);
                }
            }
        }

        protected String ComparisonTotal(TradeParticipationInfo tradeParticipationInfo)
        {
            if (tradeParticipationInfo.IsPulledOut)
                return string.Empty;

            if (tradeParticipationInfo.Amount == null)
                return string.Empty;

            return UI.Utils.SetFormDecimal(TradesController.GetInstance().GetComparisonTotal(tradeParticipationInfo));
        }

        protected String StyleNameSelectedParticipation(int? rank)
        {
            if (rank != null)
                if ((Int32)rank == 1)
                    return "lstItemBold";

            return String.Empty;
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            String parameterTradeId;
            // payment terms in TenderList DS20230922
            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetDeepTradeWithBudget(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");

                tradeInfo.Project = projectsController.GetProject(tradeInfo.Project.Id);
                tradeInfo.Project.Trades = new List<TradeInfo>();
                tradeInfo.Project.Trades.Add(tradeInfo);
                tradeInfo.Project.Drawings = tradesController.GetDeepDrawings(tradeInfo.Project);
                tradeInfo.Project.Budgets = projectsController.GetBudgets(tradeInfo.Project);


                if (tradeInfo.ProjectManager != null)
                    tradeInfo.Project.ProjectManager = tradeInfo.ProjectManager;

                if (tradeInfo.ContractsAdministrator != null)
                    tradeInfo.Project.ContractsAdministrator = tradeInfo.ContractsAdministrator;

                if (tradeInfo.Process != null)
                    tradeInfo.Process.Project = tradeInfo.Project;

                if (tradeInfo.Contract != null)
                {
                    tradeInfo.Contract.Trade = tradeInfo;
                    tradeInfo.Contract.ContractModifications = contractsController.GetContractModifications(tradeInfo.Contract);
                    tradeInfo.Contract.Subcontracts = contractsController.GetSubContracts(tradeInfo.Contract);

                    if (tradeInfo.Contract.Process != null)
                        tradeInfo.Contract.Process.Project = tradeInfo.Project;
                }

                if (tradeInfo.JobType == null)
                    tradeInfo.JobType = new JobTypeInfo();

                cmdSelParticipant.NavigateUrl = Utils.PopupCompany(this, txtParticipantId.ClientID, txtParticipantName.ClientID, tradeInfo.Project.BusinessUnit.IdStr);
                cmdSelParticipantProposal.NavigateUrl = Utils.PopupCompany(this, txtParticipantProposalId.ClientID, txtParticipantProposalName.ClientID, tradeInfo.Project.BusinessUnit.IdStr);

                if (!Page.IsPostBack)
                    BindTrade();

                BindFileLinks();

                tradeInfo.Participations = tradeInfo.ActiveParticipations;


                //#------------------
                if (tradeInfo.Contract != null)
                    if (!Page.IsPostBack && tradeInfo.Contract.Process != null && tradeInfo.Contract.Process.Steps.Count > 0)
                    {
                        ChkSignedContractFile();
                    }

                //#------------------------




                BindApproval();
                BindApprovalContract();
                BindBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }


        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditProjectTrade.aspx?TradeId=" + tradeInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteTrade(tradeInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + tradeInfo.Project.IdStr);
        }

        protected void butAddParticipant_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            TradeParticipationInfo tradeParticipationInfo;

            try
            {
                if (txtParticipantId.Value != String.Empty)
                {
                    tradeParticipationInfo = new TradeParticipationInfo();
                    tradeParticipationInfo.Type = Info.TypeActive;
                    tradeParticipationInfo.Trade = tradeInfo;
                    tradeParticipationInfo.SubContractor = SubContractorsController.GetInstance().GetSubContractorDeep(Int32.Parse(txtParticipantId.Value));

                    if (tradeInfo.SubcontractorsParticipations.Find(delegate (TradeParticipationInfo tradeParticipationInfoInList) { return tradeParticipationInfoInList.SubContractor.Id == tradeParticipationInfo.SubContractor.Id && tradeParticipationInfoInList.Type == tradeParticipationInfo.Type; }) == null)
                    {
                        tradesController.AddTradeParticipation(tradeParticipationInfo);
                        tradeInfo.Participations = tradesController.GetTradeParticipations(tradeInfo);
                        BindParticipants();
                    }

                    txtParticipantId.Value = String.Empty;
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void butAddParticipantProposal_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            TradeParticipationInfo tradeParticipationInfo;

            try
            {
                if (txtParticipantProposalId.Value != String.Empty)
                {
                    tradeParticipationInfo = new TradeParticipationInfo();
                    tradeParticipationInfo.Type = Info.TypeProposal;
                    tradeParticipationInfo.Trade = tradeInfo;
                    tradeParticipationInfo.SubContractor = SubContractorsController.GetInstance().GetSubContractorDeep(Int32.Parse(txtParticipantProposalId.Value));

                    //#--if (tradeInfo.SubcontractorsParticipations.Find(delegate (TradeParticipationInfo tradeParticipationInfoInList) { return tradeParticipationInfoInList.SubContractor.Id == tradeParticipationInfo.SubContractor.Id; }) == null)
                    if (tradeInfo.SubcontractorsParticipations.Find(delegate (TradeParticipationInfo tradeParticipationInfoInList) { return tradeParticipationInfoInList.SubContractor.Id == tradeParticipationInfo.SubContractor.Id && tradeParticipationInfoInList.Type == tradeParticipationInfo.Type; }) == null)
                    //--#  28/05/2019                  
                    {
                        tradesController.AddTradeParticipation(tradeParticipationInfo);
                        tradeInfo.Participations = tradesController.GetTradeParticipations(tradeInfo);
                        BindParticipantsProposal();
                    }

                    txtParticipantProposalId.Value = String.Empty;
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvParticipantsProposal_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Copy")
                {
                    TradesController tradesController = TradesController.GetInstance();
                    int? tradeParticipationId = (int?)gvParticipantsProposal.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                    XmlDocument xmlDocument = tradesController.CopyTradeParticipationToActive(tradeParticipationId);
                    tradeInfo.Participations = tradesController.GetTradeParticipations(tradeInfo);
                    BindCopyTree(xmlDocument);
                    BindParticipants();
                }
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
            TradeItemCategoryInfo tradeItemCategoryInfo = tradeInfo.ItemCategories.Find(delegate (TradeItemCategoryInfo tradeItemCategoryInfoInList) { return tradeItemCategoryInfoInList.Id == tradeItemCategoryId; });

            if (e.CommandName == "MoveUp")
                tradesController.ChangeDisplayOrderTradeItemCategory(tradeInfo.ItemCategories, tradeItemCategoryInfo, true);
            else if (e.CommandName == "MoveDown")
                tradesController.ChangeDisplayOrderTradeItemCategory(tradeInfo.ItemCategories, tradeItemCategoryInfo, false);

            tradeInfo.ItemCategories = tradesController.GetTradeItemCategories(tradeInfo);
            BindItemCategories();
        }


        //Called when Comparison approve btn clicked...
        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = tradeInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerTrade.StepId; });
                processController.ExecuteProcessStep(currentStep, ProcessManagerTrade.Comments);
                tradeInfo.Process = processController.GetDeepProcess(tradeInfo.Process.Id);
                tradeInfo.Process.Project = tradeInfo.Project;

                BindTrade();
                BindApproval();
                ReBindBudget();
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
                ProcessStepInfo currentStep = tradeInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerTrade.StepId; });
                //#-- processController.ReverseProcessStep(currentStep, ProcessManagerTrade.Comments);
                processController.ReverseProcessStep(currentStep, ProcessManagerTrade.Comments,tradeInfo.Name);//#--passed new parameter TradeName


                tradeInfo.Process = processController.GetDeepProcess(tradeInfo.Process.Id);
                tradeInfo.Process.Project = tradeInfo.Project;

                BindTrade();
                BindApproval();
                ReBindBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        /// <summary>
        /// The budget update can enable or disable the approval. For that reason when the budget is updated we update the approval process manager control
        /// </summary>
        protected void budget_Update(object sender, EventArgs e)
        {
            BindApproval();
        }



        //Called when Contract approve btn clicked...
        protected void cmdApproveContract_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerContract.StepId; });
                processController.ExecuteProcessStep(currentStep, ProcessManagerContract.Comments);
                tradeInfo.Contract.Process = processController.GetDeepProcess(tradeInfo.Contract.Process.Id);
                tradeInfo.Contract.Process.Project = tradeInfo.Project;

                BindTrade();
                BindApprovalContract();
                ReBindBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdReverseContract_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = tradeInfo.Contract.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerContract.StepId; });
                //#-- processController.ReverseProcessStep(currentStep, ProcessManagerContract.Comments);
                processController.ReverseProcessStep(currentStep, ProcessManagerContract.Comments,tradeInfo.Name);//#--- on reversal 
                tradeInfo.Contract.Process = processController.GetDeepProcess(tradeInfo.Contract.Process.Id);
                tradeInfo.Contract.Process.Project = tradeInfo.Project;

                BindTrade();
                BindApprovalContract();
                ReBindBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void butAddDrawingType_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();

            if (ddlDrawingTypes.SelectedItem != null)
            {
                tradesController.AddTradeDrawingType(tradeInfo, new DrawingTypeInfo(Int32.Parse(ddlDrawingTypes.SelectedItem.Value)));
                tradeInfo.DrawingTypes = tradesController.GetDrawingTypes(tradeInfo);
                BindDrawingTypes();
            }
        }

        protected void gvDrawingTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            try
            {
                int DrawingTypeId = (int)gvDrawingTypes.DataKeys[e.RowIndex].Value;
                tradesController.DeleteTradeDrawingType(tradeInfo, (new DrawingTypeInfo(DrawingTypeId)));
                tradeInfo.DrawingTypes = tradesController.GetDrawingTypes(tradeInfo);
                BindDrawingTypes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }


        //#---- Update Subcontractor's SignedContractFile path
        protected void btnSignedContractFile_Click(object sender,EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            try
            { 
                tradeInfo.SignedContractFile = sfsSignedContractFile.FilePath.ToString();
                tradesController.UpdateTradeSignedContractFile(tradeInfo);
                Response.Redirect(Request.RawUrl);// To refresh the page
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }



        //#--------

        protected void cmdDrawings_Click(object sender, EventArgs e)
        {
            UpdateDrawings(Info.TypeActive, tvDrawings);
        }

        protected void cmdDrawingsProposal_Click(object sender, EventArgs e)
        {
            UpdateDrawings(Info.TypeProposal, tvDrawingsProposal);
        }
#endregion

    }
}
