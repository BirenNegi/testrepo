using System;
using System.Web;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using SOS.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class ViewClaimPage : SOSPage
    {

#region Members
		private ClaimInfo claimInfo = null;
		private ClaimInfo previousClaim = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

			if (claimInfo == null)
				return null;

			tempNode.ParentNode.Url += "?ProjectId=" + claimInfo.Project.IdStr;

			tempNode.ParentNode.ParentNode.Title = claimInfo.Project.Name;
			tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + claimInfo.Project.IdStr;			

            return currentNode;
        }

		private void BindTree(XmlNode xmlNode, TreeView treeView)
		{
			treeView.Nodes.Clear();
			treeView.Nodes.Add(new TreeNode());
			Utils.AddNode(xmlNode, treeView.Nodes[0]);
			treeView.Visible = true;
		}

        private void AddCellHeader(HtmlTableRow row, int colSpan, String name, String style, String align)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.Attributes.Add("center", align);
            cell.InnerText = name;
            cell.ColSpan = colSpan;
            row.Cells.Add(cell);
        }

        private void AddCellName(HtmlTableRow row, String name, String style)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			cell.InnerText = name;
			row.Cells.Add(cell);
		}

        private void AddRowsHeader(HtmlTable table)
        {
            HtmlTableRow row;

            row = new HtmlTableRow();
            AddCellHeader(row, 2, "Trades", "lstHeaderTop", "left");
            AddCellHeader(row, 2, "Previous Claim", "lstHeaderTop", "left");
            AddCellHeader(row, 2, "Updates This Claim", "lstHeaderTop", "left");
            table.Rows.Add(row);

            row = new HtmlTableRow();
            AddCellHeader(row, 1, "Name", "lstHeader", "left");
            AddCellHeader(row, 1, "Total", "lstHeader", "center");
            AddCellHeader(row, 1, "%", "lstHeader", "center");
            AddCellHeader(row, 1, "Amount", "lstHeader", "center");
            AddCellHeader(row, 1, "%", "lstHeader", "center");
            AddCellHeader(row, 1, "Amount", "lstHeader", "center");
            table.Rows.Add(row);
        }

		private void AddCellAmount(HtmlTableRow row, Decimal? amount, String style)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			cell.Align = "Right";
			cell.InnerText = UI.Utils.SetFormDecimal(amount);
			row.Cells.Add(cell);
		}

		private void AddCellPercentage(HtmlTableRow row, Decimal? percentage, String style)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			cell.Align = "Right";
			cell.InnerText = UI.Utils.SetFormDecimalNoDecimals(percentage);
			row.Cells.Add(cell);
		}

		private void AddCellEmpty(HtmlTableRow row, String style)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			row.Cells.Add(cell);
		}

		private void AddRowSeparator(HtmlTable table)
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell = new HtmlTableCell();

			cell.Attributes.Add("class", "lstHeaderTop");
			cell.ColSpan = 6;
			cell.InnerText = "Variations";
			row.Cells.Add(cell);
			table.Rows.Add(row);
		}

		private void AddRowTotalTrades(HtmlTable table, Decimal? totalTradesProject)
		{
			HtmlTableRow row = new HtmlTableRow();

			AddCellName(row, "Total Trades", "lstHeader");
			AddCellAmount(row, totalTradesProject, "lstHeader");
			AddCellEmpty(row, "lstHeader");

			if (previousClaim != null)
				AddCellAmount(row, previousClaim.TotalClaimTrades, "lstHeader");
			else
				AddCellEmpty(row, "lstHeader");

			AddCellEmpty(row, "lstHeader");
			AddCellAmount(row, claimInfo.TotalClaimTrades, "lstHeader");
			table.Rows.Add(row);
		}

		private void AddRowTotalVariations(HtmlTable table, Decimal? totalVariationsProject)
		{
			HtmlTableRow row = new HtmlTableRow();

			AddCellName(row, "Total Variations", "lstHeader");
			AddCellAmount(row, totalVariationsProject, "lstHeader");
			AddCellEmpty(row, "lstHeader");

			if (previousClaim != null)
				AddCellAmount(row, previousClaim.TotalClaimVariations, "lstHeader");
			else
				AddCellEmpty(row, "lstHeader");

			AddCellEmpty(row, "lstHeader");
			AddCellAmount(row, claimInfo.TotalClaimVariations, "lstHeader");
			table.Rows.Add(row);
		}

		private void AddCellsTotals(HtmlTableRow row, Decimal? totalAmount, IClaimDetail previousClaimDetail, IClaimDetail claimDetail, String style)
		{
			Decimal? amount = null;
			Decimal? percentage = null;
			Decimal? previousAmout = null;
			Decimal? previousPercentage = null;

			if (previousClaimDetail != null)
			{
				previousAmout = previousClaimDetail.Amount;   // Is never null
				if (totalAmount != null && totalAmount != 0)
					previousPercentage = (previousAmout / totalAmount) * 100;
			}

			if (claimDetail != null)
			{
				amount = claimDetail.Amount;   // Is never null
				if (totalAmount != null && totalAmount != 0)
					percentage = (amount / totalAmount) * 100;
			}

			AddCellPercentage(row, previousPercentage, style);
			AddCellAmount(row, previousAmout, style);

			if (previousAmout != null && amount != null && previousAmout == amount)
			{
				AddCellEmpty(row, style);
				AddCellEmpty(row, style);
			}
			else
			{
				AddCellPercentage(row, percentage, style);
				AddCellAmount(row, amount, style);
			}
		}

		private void BindClaim()
		{
			ProcessController processController = ProcessController.GetInstance();
			HtmlTableRow row;
			String currentStyle;
			ClaimTradeInfo claimTradeInfo;
			ClaimTradeInfo previousClaimTradeInfo;
			ClaimVariationInfo claimVariationInfo;
			ClaimVariationInfo previousClaimVariationInfo;
			Decimal totalProjectTrades = 0;
			Decimal totalProjectVariations = 0;

			tblClaim.Rows.Clear();
			AddRowsHeader(tblClaim);

			if (Security.ViewAccess(Security.userActions.EditClaim) && processController.AllowEditCurrentUser(claimInfo))
				pnlEdit.Visible = true;

			//#--- to disable edit claim and to visible Final Payments coments box


			PlFinalPaymentComments.Visible = false;
			if (claimInfo.Process.Steps[4].Type == ProcessStepInfo.StepTypeClaimInvoiceFinalApproval && claimInfo.Process.Steps[4].Status == "Approved")
			{
				pnlEdit.Visible = false;

				if (claimInfo.Process.Steps.Count > 5)
				{
					PlFinalPaymentComments.Visible = true;

					if (claimInfo.Process.Steps[5].Status == "Approved" && claimInfo.Process.Steps[6].Status != "Approved")
						TxtFPComments.Text = UI.Utils.SetFormString(claimInfo.Process.Steps[5].Comments);

					else if (claimInfo.Process.Steps[6].Status == "Approved") {
						TxtFPComments.Text = UI.Utils.SetFormString(claimInfo.Process.Steps[6].Comments);
						TxtFPComments.ReadOnly = true;
					}
				}

			}


			//#---
			// DS20231028 >>>
			if (claimInfo.Process.Steps.Count > 0)
			{
				if (claimInfo.Process.Steps[claimInfo.Process.Steps.Count - 1].Status == "Approved")
				{
					int UserId = (int)Web.Utils.GetCurrentUserId();
					if (UserId != 1)
					{
                        sfsBackupFile1.Disabled = true;
                        sfsBackupFile2.Disabled = true;
                        btnSave1.Enabled = false;
                        btnSave2.Enabled = false;
					}
				}
			}


			lblNumber.Text = UI.Utils.SetFormInteger(claimInfo.Number);
			lblWriteDate.Text = UI.Utils.SetFormDate(claimInfo.WriteDate);
			lblDraftApprovalDate.Text = UI.Utils.SetFormDate(claimInfo.DraftApprovalDate);
			lblInternalApprovalDate.Text = UI.Utils.SetFormDate(claimInfo.InternalApprovalDate);
			lblApprovalDate.Text = UI.Utils.SetFormDate(claimInfo.ApprovalDate);
			lblDueDate.Text = UI.Utils.SetFormDate(claimInfo.DueDate);
			lblClientDueDate.Text = UI.Utils.SetFormDate(claimInfo.ClientDueDate);
            lblAdjustmentNoteName.Text = UI.Utils.SetFormString(claimInfo.AdjustmentNoteName);
            lblAdjustmentNoteAmount.Text = UI.Utils.SetFormDecimal(claimInfo.AdjustmentNoteAmount);
            sfsBackupFile1.FilePath = UI.Utils.SetFormString(claimInfo.BackupFile1);                // DS20231023
            sfsBackupFile1.Path = UI.Utils.SetFormString(claimInfo.Project.AttachmentsFolder);      // DS20231023
            sfsBackupFile2.FilePath = UI.Utils.SetFormString(claimInfo.BackupFile2);                // DS20231023
            sfsBackupFile2.Path = UI.Utils.SetFormString(claimInfo.Project.AttachmentsFolder);      // DS20231023

            if (claimInfo.Project.ClientTrades != null)
			{
				currentStyle = "";
				previousClaimTradeInfo = null;
				foreach (ClientTradeInfo clientTrade in claimInfo.Project.ClientTrades)
				{
					if (previousClaim != null)
						previousClaimTradeInfo = previousClaim.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

					claimTradeInfo = claimInfo.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

					if (previousClaimTradeInfo != null || claimTradeInfo != null)
					{
						currentStyle = currentStyle == "lstItemWrap" ? "lstAltItemWrap" : "lstItemWrap";
						row = new HtmlTableRow();

						AddCellName(row, clientTrade.Name, currentStyle);
						AddCellAmount(row, clientTrade.Amount, currentStyle);
						AddCellsTotals(row, clientTrade.Amount, previousClaimTradeInfo, claimTradeInfo, currentStyle);

						tblClaim.Rows.Add(row);

						if (clientTrade.Amount != null)
							totalProjectTrades += (Decimal)clientTrade.Amount;
					}
				}
			}

			AddRowTotalTrades(tblClaim, totalProjectTrades);

			AddRowSeparator(tblClaim);

			if (claimInfo.Project.ClientVariations != null)
			{
				currentStyle = "";
				previousClaimVariationInfo = null;
				foreach (ClientVariationInfo clientVariation in claimInfo.Project.ClientVariations)
				{

                   

                    if (previousClaim != null)
                     previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation); });

                    //#----21/06/2016------issue: Not displaying "Prvious claim" for the new revised claim amount for clientvariations which has parent variation --------------------------------

                    if (previousClaimVariationInfo == null && clientVariation.ParentClientVariation != null && previousClaim != null)
                    {
                        previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation.ParentClientVariation); });
                    }

                    //#-----21/06/2016------// 


                    claimVariationInfo = claimInfo.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.TheParentClientVariation.Equals(clientVariation.TheParentClientVariation); });




                    if (previousClaimVariationInfo != null || claimVariationInfo != null)
					{
						currentStyle = currentStyle == "lstItemWrap" ? "lstAltItemWrap" : "lstItemWrap";
						row = new HtmlTableRow();

						AddCellName(row, clientVariation.NumberAndRevisionName + " - " + clientVariation.Name, currentStyle);
						AddCellAmount(row, clientVariation.TotalAmount, currentStyle);
						AddCellsTotals(row, clientVariation.TotalAmount, previousClaimVariationInfo, claimVariationInfo, currentStyle);

						tblClaim.Rows.Add(row);

						if (clientVariation.TotalAmount != null)
							totalProjectVariations += (Decimal)clientVariation.TotalAmount;
					}
				}
			}

			AddRowTotalVariations(tblClaim, totalProjectVariations);

            if (claimInfo.IsDraftApproved)
            {
                lnkViewClaim.NavigateUrl = "~/Modules/Projects/ShowClaim.aspx?ClaimId=" + claimInfo.IdStr;
                phViewClaim.Visible = true;
            }

            //#--To display "Claim "after CA draft approval---
            if (claimInfo.Process.Steps[0].ActualDate != null)
            {
                lnkViewClaim.NavigateUrl = "~/Modules/Projects/ShowClaim.aspx?ClaimId=" + claimInfo.IdStr;
                phViewClaim.Visible = true;
            }
            else
            { phViewClaim.Visible = false; }

            //#--


            XmlDocument xmlDocument = ProjectsController.GetInstance().CheckClaim(claimInfo);
            if (xmlDocument.DocumentElement != null)
            {
                BindTree(xmlDocument.DocumentElement.ChildNodes[0], TreeViewMissingFields);
                pnlErrors.Visible = true;
            }

            ProcessManagerClaim.StepId = null;

            BindApproval();
            BindClientVariations();
        }




      


		private void BindApproval()
		{
            ProcessManagerClaim.Process = claimInfo.Process;
            ProcessManagerClaim.ApproveClicked += new System.EventHandler(cmdApprove_Click);
            ProcessManagerClaim.ReverseClicked += new System.EventHandler(cmdReverse_Click);

            ProcessManagerClaim.BindApproval();
        }

        private void BindClientVariations()
        {
            if (claimInfo.IsLastClaim & claimInfo.Project.ClientVariations != null && claimInfo.Project.ClientVariations.Count > 0)
            {
                Decimal? totalLastClaim = claimInfo.Total;

                if ((totalLastClaim != null && (Decimal)totalLastClaim >= (Decimal)claimInfo.Project.ContractAmountPlusVariations))
                {
                    List<ClientVariationInfo> clientVariationInfoList = new List<ClientVariationInfo>();

                    foreach (ClientVariationInfo clientVariationInfo in claimInfo.Project.ClientVariations)
                        if (!clientVariationInfo.IsApproved && !clientVariationInfo.IsCancel)
                            clientVariationInfoList.Add(clientVariationInfo);

                    if (clientVariationInfoList.Count > 0)
                    {
                        gvClientVariations.DataSource = clientVariationInfoList;
                        gvClientVariations.DataBind();

                        pnlClientVariations.Visible = true;
                    }

                    TitleBar1.Info = "Final";
                }
            }
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
			String parameterClaimId;
			ProjectsController projectsController = ProjectsController.GetInstance();
			ProcessController processController = ProcessController.GetInstance();
            ClientVariationInfo clientVariationInfo = new ClientVariationInfo();

			try
			{
				Security.CheckAccess(Security.userActions.ViewClaim);
				parameterClaimId = Request.Params["ClaimId"];

				claimInfo = projectsController.GetClaimWithTradesAndVariations(Int32.Parse(parameterClaimId));


				Core.Utils.CheckNullObject(claimInfo, parameterClaimId, "Project Claim");
				claimInfo.Project = projectsController.GetProjectWithClaimsTradesAndVariations(claimInfo.Project.Id);

                if (claimInfo.ClaimTrades != null)
                    foreach (ClaimTradeInfo claimTrade in claimInfo.ClaimTrades)
                        claimTrade.ClientTrade = claimInfo.Project.ClientTrades.Find(delegate(ClientTradeInfo clientTradeInfoInList) { return clientTradeInfoInList.Equals(claimTrade.ClientTrade); });

                if (claimInfo.ClaimVariations != null)
                    foreach (ClaimVariationInfo claimVariation in claimInfo.ClaimVariations)
                        claimVariation.ClientVariation = claimInfo.Project.ClientVariations.Find(delegate(ClientVariationInfo clientVariationInfoInList) { return clientVariationInfoInList.TheParentClientVariation.Equals(claimVariation.ClientVariation.TheParentClientVariation); });

				previousClaim = projectsController.GetPreviousClaim(claimInfo);

                claimInfo.IsLastClaim = claimInfo.Project.Claims.Count > 0 && claimInfo.Project.Claims[claimInfo.Project.Claims.Count - 1].Equals(claimInfo);

                claimInfo.Project.Claims = new List<ClaimInfo>();
                claimInfo.Project.Claims.Add(claimInfo);
                claimInfo.Project.Claims.Add(previousClaim);

                if (claimInfo.Process != null)
					claimInfo.Process.Project = claimInfo.Project;

                if (!Page.IsPostBack)
				{

                    BindClaim();

                }

                else
                {

                    claimInfo.BackupFile1 = sfsBackupFile1.FilePath;                // DS20231023
                    claimInfo.BackupFile2 = sfsBackupFile2.FilePath;                // DS20231023
 

                    BindApproval();
                }
                sflBackupFile1.FilePath = sfsBackupFile1.FilePath;
                sflBackupFile1.BasePath = claimInfo.Project.AttachmentsFolder;
                sflBackupFile1.PageLink = String.Format("~/Modules/Projects/ShowClaimFile.aspx?FileType={0}&ClaimId={1}", "BackupFile1", claimInfo.IdStr);
                sflBackupFile2.FilePath = sfsBackupFile2.FilePath;
                sflBackupFile2.BasePath = claimInfo.Project.AttachmentsFolder;
                sflBackupFile2.PageLink = String.Format("~/Modules/Projects/ShowClaimFile.aspx?FileType={0}&ClaimId={1}", "BackupFile2", claimInfo.IdStr);
            }
			catch (Exception Ex)
			{
				Utils.ProcessPageLoadException(this, Ex);
			}			
        }

		protected void cmdApprove_Click(object sender, EventArgs e)
		{
			ProcessController processController = ProcessController.GetInstance();

			try
			{
				ProcessStepInfo currentStep = claimInfo.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerClaim.StepId; });

                //#----
                if (currentStep.Type == ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByFC || currentStep.Type == ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByPM)
                {
                    if (currentStep.TargetDate >= DateTime.Now)
                    { }
                    currentStep.Comments = TxtFPComments.Text;

                }
                //#----

                processController.ExecuteProcessStep(currentStep, ProcessManagerClaim.Comments);





                //#--- To add Final Claim Payment processsteps

                if (currentStep.Type != null && (currentStep.Type != ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByFC || currentStep.Type != ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByPM))
                {
                    if (claimInfo.Total != null && claimInfo.Project.ContractAmountPlusVariations != null)
                        if (claimInfo.Total == claimInfo.Project.ContractAmountPlusVariations && currentStep.Type == ProcessStepInfo.StepTypeClaimInvoiceFinalApproval && claimInfo.IsApproved && currentStep.Role == "FC" && claimInfo.Process.Steps[4].Status=="Approved")
                        {
                            // Create new claim  Process Steps  which will be approved by PM and FC  after the Project's (Practical completion date + Defects liability day)

                            ProjectsController projController = ProjectsController.GetInstance();
                            projController.AddFinalPaymentProcessSteps(claimInfo);



                        }

                }
                //#--- To add Final Claim Payment processsteps





                BindClaim();


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
				ProcessStepInfo currentStep = claimInfo.Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerClaim.StepId; });
				processController.ReverseProcessStep(currentStep, ProcessManagerClaim.Comments);

                BindClaim();
			}
			catch (Exception Ex)
			{
				Utils.ProcessPageLoadException(this, Ex);
			}
		}		
		
		protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
				ProjectsController.GetInstance().DeleteClaim(claimInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

			Response.Redirect("~/Modules/Projects/ListClaims.aspx?ProjectId=" + claimInfo.Project.IdStr);
        }
        //cmdSaveDoc_Click
        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditClaim.aspx?ClaimId=" + claimInfo.IdStr);
        }
        protected void cmdSaveDoc_Click(object sender, EventArgs e)   // DS20231023 >>>
        {
            ProjectsController projController = ProjectsController.GetInstance();
            claimInfo.BackupFile1 = sfsBackupFile1.FilePath;                // DS20231023
            claimInfo.BackupFile2 = sfsBackupFile2.FilePath;                // DS20231023
            sflBackupFile1.FilePath = sfsBackupFile1.FilePath;
            sflBackupFile1.BasePath = claimInfo.Project.AttachmentsFolder;
            sflBackupFile1.PageLink = String.Format("~/Modules/Projects/ShowClaimFile.aspx?FileType={0}&ClaimId={1}", "BackupFile1", claimInfo.IdStr);
            sflBackupFile2.FilePath = sfsBackupFile2.FilePath;
            sflBackupFile2.BasePath = claimInfo.Project.AttachmentsFolder;
            sflBackupFile2.PageLink = String.Format("~/Modules/Projects/ShowClaimFile.aspx?FileType={0}&ClaimId={1}", "BackupFile2", claimInfo.IdStr);
            projController.UpdateClaimBackupFiles(claimInfo);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        #endregion

    }
}
