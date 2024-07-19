using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditClaimPage : SOSPage
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

		private void AddRowSeparator(HtmlTable table)
		{
			HtmlTableRow row = new HtmlTableRow();
			HtmlTableCell cell = new HtmlTableCell();

			row = new HtmlTableRow();
			cell.Attributes.Add("class", "lstHeaderTop");
			cell.ColSpan = 6;
			cell.InnerText = "Variations";
			row.Cells.Add(cell);
			table.Rows.Add(row);			
		}

		private void AddCellName(HtmlTableRow row, String name, String style)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			cell.InnerText = name;
			row.Cells.Add(cell);
		}

		private void AddCellAmount(HtmlTableRow row, Decimal? amount, String style)
		{
			HtmlTableCell cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			cell.Align = "Right";
			cell.InnerText = UI.Utils.SetFormEditDecimal(amount);
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

		private void AddCellTextAmount(HtmlTableRow row, Decimal? maxAmount, String style, String controlName)
		{
			TextBox textBox;
			HtmlTableCell cell;
			CompareValidator compareValidator;
			RangeValidator rangeValidator;			

			textBox = new TextBox();
			textBox.Width = new Unit("80px");
            textBox.Attributes.Add("Style", "text-align:right");
			textBox.ID = controlName;

			compareValidator = new CompareValidator();
			compareValidator.ControlToValidate = textBox.ClientID;
			compareValidator.ErrorMessage = "Invalid number!<br />";
			compareValidator.CssClass = "frmError";
			compareValidator.Display = ValidatorDisplay.Dynamic;
			compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
			compareValidator.Type = ValidationDataType.Currency;

			rangeValidator = new RangeValidator();
			rangeValidator.ControlToValidate = textBox.ClientID;
			rangeValidator.ErrorMessage = "Must be currency [<= " + maxAmount + "]<br />";
			rangeValidator.CssClass = "frmError";
			rangeValidator.Display = ValidatorDisplay.Dynamic;
			rangeValidator.Type = ValidationDataType.Currency;
            rangeValidator.MinimumValue = Decimal.MinValue.ToString();
            rangeValidator.MaximumValue = maxAmount != null ? String.Format("{0:#,###0.00}", maxAmount) : "0";

			cell = new HtmlTableCell();
			cell.Attributes.Add("class", style);
			cell.Controls.Add(compareValidator);
			cell.Controls.Add(rangeValidator);
			cell.Controls.Add(textBox);
			row.Cells.Add(cell);
		}

		private void AddCellTextPercentage(HtmlTableRow row, String style, String controlName)
		{
			TextBox textBox;
			HtmlTableCell cell;
			CompareValidator compareValidator;
			RangeValidator rangeValidator;
			
			textBox = new TextBox();
			textBox.Width = new Unit("30px");
            textBox.Attributes.Add("Style", "text-align:right");
			textBox.ID = controlName;

			compareValidator = new CompareValidator();
			compareValidator.ControlToValidate = textBox.ClientID;
			compareValidator.ErrorMessage = "Invalid number!<br />";
			compareValidator.CssClass = "frmError";
			compareValidator.Display = ValidatorDisplay.Dynamic;
			compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
			compareValidator.Type = ValidationDataType.Currency;

			rangeValidator = new RangeValidator();
			rangeValidator.ControlToValidate = textBox.ClientID;
			rangeValidator.ErrorMessage = "Must be integer [-100..100]<br />";
			rangeValidator.CssClass = "frmError";
			rangeValidator.Display = ValidatorDisplay.Dynamic;
			rangeValidator.Type = ValidationDataType.Integer;
			rangeValidator.MinimumValue = "-100";
			rangeValidator.MaximumValue = "100";

			cell = new HtmlTableCell();
			cell.Controls.Add(compareValidator);
			cell.Controls.Add(rangeValidator);
			cell.Attributes.Add("class", style);
			cell.Controls.Add(textBox);
			row.Cells.Add(cell);
		}

		private void CreateForm()
		{
			ProjectsController projectsController = ProjectsController.GetInstance();
			HtmlTableRow row;
			String currentStyle;
			Decimal? previousAmout;
			Decimal? previousPercentage;
			ClaimTradeInfo previousClaimTradeInfo;
			ClaimVariationInfo previousClaimVariationInfo;

			if (claimInfo.Project.ClientTrades != null)
			{
				currentStyle = "";
				foreach (ClientTradeInfo clientTrade in claimInfo.Project.ClientTrades)
				{
					row = new HtmlTableRow();
					currentStyle = currentStyle == "lstItemWrap" ? "lstAltItemWrap" : "lstItemWrap";

					AddCellName(row, clientTrade.Name, currentStyle);
					AddCellAmount(row, clientTrade.Amount, currentStyle);

					previousAmout = null;
					previousPercentage = null;
					if (previousClaim != null)
					{
						previousClaimTradeInfo = previousClaim.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

                        

						if (previousClaimTradeInfo != null)
						{
							previousAmout = previousClaimTradeInfo.Amount;

							if (clientTrade.Amount != null && previousAmout != null && clientTrade.Amount != 0)
								previousPercentage = (previousAmout / clientTrade.Amount) * 100;
						}
					}

					AddCellPercentage(row, previousPercentage, currentStyle);
					AddCellAmount(row, previousAmout, currentStyle);
					AddCellTextPercentage(row, currentStyle, "TradePercentage_" + clientTrade.IdStr);
					AddCellTextAmount(row, clientTrade.Amount, currentStyle, "TradeAmount_" + clientTrade.IdStr);

					tblClaim.Rows.Add(row);
				}
			}

			AddRowSeparator(tblClaim);
			
			if (claimInfo.Project.ClientVariations != null)
			{
				currentStyle = "";
				foreach (ClientVariationInfo clientVariation in claimInfo.Project.ClientVariations)
				{
					row = new HtmlTableRow();
					currentStyle = currentStyle == "lstItemWrap" ? "lstAltItemWrap" : "lstItemWrap";

					AddCellName(row, clientVariation.NumberAndRevisionName + " - " + clientVariation.Name, currentStyle);
					AddCellAmount(row, clientVariation.TotalAmount, currentStyle);

					previousAmout = null;
					previousPercentage = null;
					if (previousClaim != null)
					{
						previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.TheParentClientVariation.Equals(clientVariation.TheParentClientVariation); });

                       
                        if (previousClaimVariationInfo != null)
						{
							previousAmout = previousClaimVariationInfo.Amount;

							if (clientVariation.TotalAmount != null && previousAmout != null && clientVariation.TotalAmount != 0)
								previousPercentage = (previousAmout / clientVariation.TotalAmount) * 100;
						}
					}

					AddCellPercentage(row, previousPercentage, currentStyle);
					AddCellAmount(row, previousAmout, currentStyle);
					AddCellTextPercentage(row, currentStyle, "VariationPercentage_" + clientVariation.IdStr);
					AddCellTextAmount(row, clientVariation.TotalAmount, currentStyle, "VariationAmount_" + clientVariation.IdStr);

					tblClaim.Rows.Add(row);
				}
			}
		}

		private void SetAmountPercentageTextBoxes(String txtAmountId, String txtPercentageId, Decimal? totalAmount, IClaimDetail claimDetail, IClaimDetail previousClaimDetail)
		{
			Decimal? amount = claimDetail != null ? claimDetail.Amount : null;
			Decimal? percentage = null;

			if (amount != null)
			{
                if (previousClaimDetail == null || previousClaimDetail.Amount == null || previousClaimDetail.Amount != claimDetail.Amount)
                {
                    if (amount == 0)
                    {
                        amount = null;
                        percentage = 0;
                    }
                    else
                        if (totalAmount != null && totalAmount != 0)
                        percentage = (amount / totalAmount) * 100;

                    ((TextBox)Utils.FindControlRecursive(tblClaim, txtAmountId)).Text = UI.Utils.SetFormEditDecimal(amount);
                    ((TextBox)Utils.FindControlRecursive(tblClaim, txtPercentageId)).Text = UI.Utils.SetFormDecimalNoDecimals(percentage);
                }

                

            }
        }

		public Decimal? GetAmountFromTextBoxes(String txtAmountId, String txtPercentageId, Decimal? totalAmount, IClaimDetail claimDetail, IClaimDetail previousClaimDetail)
		{
			Decimal? amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblClaim, txtAmountId)).Text);

			if (amount == null)
			{
				Decimal? percentage = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblClaim, txtPercentageId)).Text);

				if (percentage == null)
				{
					if (previousClaimDetail != null && previousClaimDetail.Amount != null)
					{
						amount = previousClaimDetail.Amount;
					}
				}
				else
				{
					if (totalAmount != null)
					{
						amount = Math.Round((Decimal)(percentage * totalAmount) / 100, 2);
					}
				}
			}

			return amount;
		}

        private void ObjectsToForm()
        {
			ClaimTradeInfo claimTradeInfo;
			ClaimVariationInfo claimVariationInfo;
			ClaimTradeInfo previousClaimTradeInfo = null;
			ClaimVariationInfo previousClaimVariationInfo = null;

			if (claimInfo.Id == null)
			{
				TitleBar.Title = "Adding Claim";
				cmdUpdateTop.Text = "Save";
				cmdUpdateBottom.Text = "Save";
			}

            lblNumber.Text = UI.Utils.SetFormInteger(claimInfo.Number);
            
            lblDraftApprovalDate.Text = UI.Utils.SetFormDate(claimInfo.DraftApprovalDate);
            lblInternalApprovalDate.Text = UI.Utils.SetFormDate(claimInfo.InternalApprovalDate);
            lblApprovalDate.Text = UI.Utils.SetFormDate(claimInfo.ApprovalDate);

            sdrDueDate.Date = claimInfo.DueDate;
            sdrClientDueDate.Date = claimInfo.ClientDueDate;
            txtAdjustmentNoteName.Text = UI.Utils.SetFormString(claimInfo.AdjustmentNoteName);
            txtAdjustmentNoteAmount.Text = UI.Utils.SetFormEditDecimal(claimInfo.AdjustmentNoteAmount);

			if (claimInfo.Project.ClientTrades != null)
			{
				foreach (ClientTradeInfo clientTrade in claimInfo.Project.ClientTrades)
				{
					claimTradeInfo = claimInfo.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

					if (previousClaim != null)
						previousClaimTradeInfo = previousClaim.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });
					
					SetAmountPercentageTextBoxes("TradeAmount_" + clientTrade.IdStr, "TradePercentage_" + clientTrade.IdStr, clientTrade.Amount, claimTradeInfo, previousClaimTradeInfo);
				}
			}

			if (claimInfo.Project.ClientVariations != null)
			{
				foreach (ClientVariationInfo clientVariation in claimInfo.Project.ClientVariations)
				{
					claimVariationInfo = claimInfo.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation); });

					if (previousClaim != null)
						previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation); });

                    //#---21/06/2016----to get Prvious claim amount from ParentClientVariationId//
                    if (previousClaimVariationInfo == null && clientVariation.ParentClientVariation != null && previousClaim != null)
                    {
                        previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation.ParentClientVariation); });
                        //#---19/12/2016--in editmode for revised claim it was  not dispaying the amount 
                        claimVariationInfo = claimInfo.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation.ParentClientVariation); });
                        //#---19/12/2016--
                    }

                    //#---//

                    SetAmountPercentageTextBoxes("VariationAmount_" + clientVariation.IdStr, "VariationPercentage_" + clientVariation.IdStr, clientVariation.TotalAmount, claimVariationInfo, previousClaimVariationInfo);
				}
			}
		}

        private void FormToObjects()
        {
			ClaimTradeInfo claimTradeInfo;
			ClaimVariationInfo claimVariationInfo;
			ClaimTradeInfo previousClaimTradeInfo = null;
			ClaimVariationInfo previousClaimVariationInfo = null;
			Decimal? amount;

            claimInfo.DueDate = sdrDueDate.Date;
            claimInfo.ClientDueDate = sdrClientDueDate.Date;
            claimInfo.AdjustmentNoteName = UI.Utils.GetFormString(txtAdjustmentNoteName.Text);
            claimInfo.AdjustmentNoteAmount = UI.Utils.GetFormDecimal(txtAdjustmentNoteAmount.Text);

			if (claimInfo.Project.ClientTrades != null)
			{
				foreach (ClientTradeInfo clientTrade in claimInfo.Project.ClientTrades)
				{
					claimTradeInfo = claimInfo.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

					if (previousClaim != null)
						previousClaimTradeInfo = previousClaim.ClaimTrades.Find(delegate(ClaimTradeInfo claimTradeInfoInList) { return claimTradeInfoInList.ClientTrade.Equals(clientTrade); });

					amount = GetAmountFromTextBoxes("TradeAmount_" + clientTrade.IdStr, "TradePercentage_" + clientTrade.IdStr, clientTrade.Amount, claimTradeInfo, previousClaimTradeInfo);

					if (amount == null)
					{
						if (claimTradeInfo != null)
						{
							claimInfo.ClaimTrades.Remove(claimTradeInfo);
						}
					}
					else
					{
						if (claimTradeInfo != null)
						{
							claimTradeInfo.Amount = amount;
						}
						else
						{
							claimTradeInfo = new ClaimTradeInfo();
							claimTradeInfo.Amount = amount;
							claimTradeInfo.Claim = claimInfo;
							claimTradeInfo.ClientTrade = clientTrade;
							claimInfo.ClaimTrades.Add(claimTradeInfo);
						}
					}
				}
			}

			if (claimInfo.Project.ClientVariations != null)
			{
				foreach (ClientVariationInfo clientVariation in claimInfo.Project.ClientVariations)
				{
					claimVariationInfo = claimInfo.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation); });

					if (previousClaim != null)
						previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate(ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation); });

                    //#----21/06/2016-----to get Prvious claim amount from ParentClientVariationId-//

                    if (previousClaimVariationInfo == null && clientVariation.ParentClientVariation != null && previousClaim != null)
                    {
                        previousClaimVariationInfo = previousClaim.ClaimVariations.Find(delegate (ClaimVariationInfo claimVariationInfoInList) { return claimVariationInfoInList.ClientVariation.Equals(clientVariation.ParentClientVariation); });

                    }

                    //#------------//


                    amount = GetAmountFromTextBoxes("VariationAmount_" + clientVariation.IdStr, "VariationPercentage_" + clientVariation.IdStr, clientVariation.TotalAmount, claimVariationInfo, previousClaimVariationInfo);

					if (amount == null)
					{
						if (claimVariationInfo != null)
						{
							claimInfo.ClaimVariations.Remove(claimVariationInfo);
						}
					}
					else
					{
						if (claimVariationInfo != null)
						{
							claimVariationInfo.Amount = amount;
						}
						else
						{
							claimVariationInfo = new ClaimVariationInfo();
							claimVariationInfo.Amount = amount;
							claimVariationInfo.Claim = claimInfo;
							claimVariationInfo.ClientVariation = clientVariation;
							claimInfo.ClaimVariations.Add(claimVariationInfo);

						}
					}
				}
			}
		}
#endregion

#region Event Handlers
		protected void Page_Init(object sender, EventArgs e)
		{
			String parameterClaimId;
			ProjectsController projectsController = ProjectsController.GetInstance();
			ProcessController processController = ProcessController.GetInstance();
            Decimal? contractAmountPlusVariations;

			try
			{
				Security.CheckAccess(Security.userActions.EditClaim);
				parameterClaimId = Request.Params["ClaimId"];
				if (parameterClaimId == null)
				{
					String parameterProjectId = Utils.CheckParameter("ProjectId");
					ProjectInfo projectInfo = projectsController.GetProjectWithClaimsTradesAndVariations(Int32.Parse(parameterProjectId));
					Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                    if (projectInfo.Claims != null && projectInfo.Claims.Count > 0)
                    {
                        previousClaim = projectsController.GetClaimWithTradesAndVariations(projectInfo.Claims[projectInfo.Claims.Count - 1].Id);
                        contractAmountPlusVariations = projectInfo.ContractAmountPlusVariations;
                        if (contractAmountPlusVariations != null && previousClaim.Total >= (Decimal)contractAmountPlusVariations)
                            throw new Exception("A Claim can not be created. 100% has been claimed.");
                    }
                    else
                        previousClaim = null;

					claimInfo = projectsController.InitializeClaim(projectInfo, previousClaim);
				}
				else
				{
					claimInfo = projectsController.GetClaimWithTradesAndVariations(Int32.Parse(parameterClaimId));
					Core.Utils.CheckNullObject(claimInfo, parameterClaimId, "Project Claim");
					claimInfo.Project = projectsController.GetProjectWithClaimsTradesAndVariations(claimInfo.Project.Id);
					previousClaim = projectsController.GetPreviousClaim(claimInfo);
				}

				processController.CheckEditCurrentUser(claimInfo);

				CreateForm();
			}
			catch (Exception Ex)
			{
				Utils.ProcessPageLoadException(this, Ex);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
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
					claimInfo.Id = ProjectsController.GetInstance().AddUpdateClaim(claimInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
				Response.Redirect("~/Modules/Projects/ViewClaim.aspx?ClaimId=" + claimInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
			if (claimInfo.Id == null)
				Response.Redirect("~/Modules/Projects/ListClaims.aspx?ProjectId=" + claimInfo.Project.IdStr);
			else
				Response.Redirect("~/Modules/Projects/ViewClaim.aspx?ClaimId=" + claimInfo.IdStr);
		}
#endregion

    }
}