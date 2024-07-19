using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewProjectPage : SOSPage
    {

        #region Members
        private ProjectInfo projectInfo = null;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = projectInfo.Name + (projectInfo.IsStatusProposal ? " (Proposal)" : ""); ;

            return currentNode;
        }

        private ProcessStepInfo CurrentStep(TradeInfo tradeInfo)
        {
            ProcessController processController = ProcessController.GetInstance();
            ProcessStepInfo processStepInfo = null;

            if (tradeInfo.Contract != null)
            {
                processStepInfo = processController.GetLastStep(tradeInfo.Contract.Process);
                if (processStepInfo == null)
                {
                    processStepInfo = processController.GetLastStep(tradeInfo.Process);
                }
            }
            else
            {
                processStepInfo = processController.GetLastStep(tradeInfo.Process);
            }

            return processStepInfo;
        }

        protected String InfoStatus(TradeInfo tradeInfo)
        {
            ProcessStepInfo processStepInfo = CurrentStep(tradeInfo);

            if (processStepInfo != null)
                return processStepInfo.Name;
            else
                return String.Empty;
        }

        protected String DateStatus(TradeInfo tradeInfo)
        {
            ProcessStepInfo processStepInfo = CurrentStep(tradeInfo);

            if (processStepInfo != null)
                return UI.Utils.SetFormDate(processStepInfo.ActualDate);
            else
                return String.Empty;
        }

        protected String LinkContract(TradeInfo tradeInfo)
        {
            if (tradeInfo.ContractApproved)
                return "~/Modules/Contracts/ShowContract.aspx?ContractId=" + tradeInfo.Contract.IdStr;
            else
                return null;
        }

        protected String StyleName(int? numDays)
        {
            if (numDays != null)
                if ((Int32)numDays > 0)
                    return "lstItemError";

            return String.Empty;
        }

        protected String SetIconPath(int? flag)
        {
            if (flag == null)
                return "~/Images/IconView.gif";
            else if (flag.Value == TradeInfo.FlagRed)
                return "~/Images/RedFlag.png";
            else if (flag.Value == TradeInfo.FlagGreen)
                return "~/Images/GreenFlag.png";
            else
                return "~/Images/IconView.gif";
        }

        private void BindTradesContract()
        {
            List<TradeInfo> tradeInfoList = new List<TradeInfo>();

            foreach (TradeInfo tradeInfo in projectInfo.ContractTrades)
                if (ddlJobTypes.SelectedValue == String.Empty || ddlJobTypes.SelectedValue == tradeInfo.JobType.IdStr)
                    tradeInfoList.Add(tradeInfo);


            gvTradesContract.DataSource = tradeInfoList;
            gvTradesContract.DataBind();
        }

        private void BindTradesTender()
        {
            List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
            List<TradeParticipationInfo> tradeParticipationInfoList;
            ListItem listItem;
            HtmlTable table;
            HtmlTableRow row;
            HtmlTableCell cell;
            HyperLink hyperLink;
            Int32 maxSubbies = 0;
            String currStyle;

            ddlTradeTemplates.Items.Clear();
            //#--
            tradeTemplateInfoList.Sort((x, y) => x.Trade.Code.CompareTo(y.Trade.Code));
            //#--

            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                if (tradeTemplateInfo.Trade != null)
                    if (ddlJobTypes1.SelectedValue == String.Empty || ddlJobTypes1.SelectedValue == tradeTemplateInfo.Trade.JobType.IdStr)
                        //#--  ddlTradeTemplates.Items.Add(new ListItem(tradeTemplateInfo.Trade.Name, tradeTemplateInfo.IdStr));

                        //#--- to display -------Code-TrdaeName-JobType-----
                        ddlTradeTemplates.Items.Add(new ListItem(tradeTemplateInfo.Trade.Code + " - " + tradeTemplateInfo.Trade.Name + " - " + tradeTemplateInfo.Trade.JobTypeName, tradeTemplateInfo.IdStr));


            //#---


            foreach (TradeInfo tradeInfo in projectInfo.TenderTrades)
            {
                if (ddlJobTypes1.SelectedValue == String.Empty || ddlJobTypes1.SelectedValue == tradeInfo.JobType.IdStr)
                {
                    tradeParticipationInfoList = projectInfo.IsStatusProposal ? tradeInfo.SubcontractorsProposalParticipations : tradeInfo.SubcontractorsActiveParticipations;

                    //#--- listItem = ddlTradeTemplates.Items.FindByText(tradeInfo.Name);


                    //#---
                    listItem = ddlTradeTemplates.Items.FindByText(tradeInfo.Name + " - " + tradeInfo.Code + " - " + tradeInfo.JobTypeName);
                    //#---



                    if (tradeParticipationInfoList.Count > maxSubbies)
                        maxSubbies = tradeParticipationInfoList.Count;

                    if (listItem != null)
                        ddlTradeTemplates.Items.Remove(listItem);
                }
            }

            butAddTradeTemplate.Enabled = ddlTradeTemplates.Items.Count != 0;

            //Tender process manager
            table = new HtmlTable();
            table.CellPadding = 4;
            table.CellSpacing = 1;

            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Align = "Center";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Align = "Center";
            cell.InnerText = "Trade Name";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Align = "Center";
            cell.InnerText = "Code";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.InnerHtml = "Invitation<br />Date";
            cell.Align = "Center";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Align = "Center";
            cell.InnerHtml = "Closing<br />Date";
            row.Cells.Add(cell);

            if (!projectInfo.IsStatusProposal)
            {
                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Align = "Center";
                cell.InnerText = "Status";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Align = "Center";
                cell.InnerHtml = "Date";
                row.Cells.Add(cell);
            }

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Align = "Center";
            cell.InnerHtml = "Comparison<br />Due Date";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Align = "Center";
            cell.InnerHtml = "Due<br />Days";
            row.Cells.Add(cell);

            for (int i = 1; i <= maxSubbies; i++)
            {
                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeader");
                cell.Align = "Center";
                cell.InnerText = "Subbie " + i.ToString();
                row.Cells.Add(cell);
            }

            table.Rows.Add(row);

            currStyle = "";
            foreach (TradeInfo tradeInfo in projectInfo.TenderTrades)
            {
                if (ddlJobTypes1.SelectedValue == String.Empty || ddlJobTypes1.SelectedValue == tradeInfo.JobType.IdStr)
                {
                    tradeParticipationInfoList = projectInfo.IsStatusProposal ? tradeInfo.SubcontractorsProposalParticipations : tradeInfo.SubcontractorsActiveParticipations;

                    row = new HtmlTableRow();

                    currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                    hyperLink = new HyperLink();
                    hyperLink.ImageUrl = SetIconPath(tradeInfo.Flag);
                    hyperLink.ToolTip = "Open";
                    hyperLink.NavigateUrl = String.Format("~/Modules/Projects/ViewProjectTrade.aspx?TradeId={0}", tradeInfo.IdStr);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Controls.Add(hyperLink);
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.InnerText = tradeInfo.Name;
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Align = "Center";
                    cell.InnerText = tradeInfo.Code;
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Align = "Center";
                    cell.InnerText = UI.Utils.SetFormDate(tradeInfo.InvitationDate);
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Align = "Center";
                    cell.InnerText = UI.Utils.SetFormDate(tradeInfo.DueDate);
                    row.Cells.Add(cell);

                    if (!projectInfo.IsStatusProposal)
                    {
                        cell = new HtmlTableCell();
                        cell.Attributes.Add("class", currStyle);
                        cell.InnerText = InfoStatus(tradeInfo);
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        cell.Attributes.Add("class", currStyle);
                        cell.Align = "Center";
                        cell.InnerText = DateStatus(tradeInfo);
                        row.Cells.Add(cell);
                    }

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Align = "Center";
                    cell.InnerText = UI.Utils.SetFormDate(tradeInfo.ComparisonDueDate);
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Align = "Center";
                    cell.InnerHtml = "<span class='" + StyleName(tradeInfo.ComparisonDueDays) + "'>" + UI.Utils.SetFormInteger(tradeInfo.ComparisonDueDays) + "</span>";
                    row.Cells.Add(cell);

                    foreach (TradeParticipationInfo tradeParticipationInfo in tradeParticipationInfoList)
                    {
                        cell = new HtmlTableCell();
                        cell.Attributes.Add("class", currStyle);
                        cell.InnerText = tradeParticipationInfo.SubContractor.ShortNameOrName;
                        row.Cells.Add(cell);
                    }

                    for (int i = tradeParticipationInfoList.Count; i < maxSubbies; i++)
                    {
                        cell = new HtmlTableCell();
                        cell.Attributes.Add("class", currStyle);
                        row.Cells.Add(cell);
                    }

                    table.Rows.Add(row);
                }

                phTender.Controls.Add(table);
            }
        }

        private void BindProject()
        {
            List<JobTypeInfo> jobTypes = ContractsController.GetInstance().GetJobTypes();

            if (projectInfo.IsStatusProposal)
            {
                pnlStatusActive.Visible = false;
                tdProposal.Attributes.Add("class", "PanelProposal");
                tdProposal1.Attributes.Add("class", "PanelProposal");
            }

            lblName.Text = UI.Utils.SetFormString(projectInfo.Name);
            lblNumber.Text = UI.Utils.SetFormString(projectInfo.Number);
            lblYear.Text = UI.Utils.SetFormString(projectInfo.Year);
            lblBusiniessUnit.Text = UI.Utils.SetFormString(projectInfo.BusinessUnitName);
            lblStatus.Text = UI.Utils.GetFormString(projectInfo.StatusName);
            lblPrincipal.Text = UI.Utils.SetFormString(projectInfo.Principal);
            lblPrincipalABN.Text = UI.Utils.SetFormString(projectInfo.PrincipalABN);
            lblStreet.Text = UI.Utils.SetFormString(projectInfo.Street);
            lblLocality.Text = UI.Utils.SetFormString(projectInfo.Locality);
            lblState.Text = UI.Utils.SetFormString(projectInfo.State);
            lblPostalCode.Text = UI.Utils.SetFormString(projectInfo.PostalCode);
            lblPrincipal2.Text = UI.Utils.SetFormString(projectInfo.Principal2);
            lblPrincipal2ABN.Text = UI.Utils.SetFormString(projectInfo.Principal2ABN);
            lblFax.Text = UI.Utils.SetFormString(projectInfo.Fax);
            lblDefectsLiability.Text = UI.Utils.SetFormInteger(projectInfo.DefectsLiability);
            lblLiquidatedDamages.Text = UI.Utils.SetFormString(projectInfo.LiquidatedDamages);
            lblSiteAllowances.Text = UI.Utils.SetFormString(projectInfo.SiteAllowances);
            lblInterest.Text = UI.Utils.SetFormString(projectInfo.Interest);
            lblWaranty1Amount.Text = UI.Utils.SetFormDecimal(projectInfo.Waranty1Amount);
            lblWaranty1Date.Text = UI.Utils.SetFormDate(projectInfo.Waranty1Date);
            lblWaranty2Amount.Text = UI.Utils.SetFormDecimal(projectInfo.Waranty2Amount);
            lblWaranty2Date.Text = UI.Utils.SetFormDate(projectInfo.Waranty2Date);
            lblRetention.Text = UI.Utils.SetFormString(projectInfo.Retention);
            lblRetentionToCertification.Text = UI.Utils.SetFormString(projectInfo.RetentionToCertification);
            lblRetentionToDLP.Text = UI.Utils.SetFormString(projectInfo.RetentionToDLP);
            lblInterest.Text = UI.Utils.SetFormString(projectInfo.Interest);
            lblPracticalCompletionDate.Text = UI.Utils.SetFormDate(projectInfo.PracticalCompletionDate);
            lblCommencementDate.Text = UI.Utils.SetFormDate(projectInfo.CommencementDate);
            lblCompletionDate.Text = UI.Utils.SetFormDate(projectInfo.CompletionDate);


            //#----23/06/2016---------

            lblCompletionDateClaimedEOTs.Text = UI.Utils.SetFormDate(projectInfo.ClaimedEOTsCompletionDate);
            lblCompletionDateApprovedEOTs.Text = UI.Utils.SetFormDate(projectInfo.ApprovedEOTsCompletionDate);

            //#----23/06/2016---------

            lblFirstClaimDueDate.Text = UI.Utils.SetFormDate(projectInfo.FirstClaimDueDate);
            lblDeepZoomUrl.Text = UI.Utils.SetFormString(projectInfo.DeepZoomUrl);
            lblAttachmentsFolder.Text = UI.Utils.SetFormString(projectInfo.AttachmentsFolder);
            lblMaintenanceManualFile.Text = UI.Utils.SetFormString(projectInfo.MaintenanceManualFile);
            lblPostProjectReviewFile.Text = UI.Utils.SetFormString(projectInfo.PostProjectReviewFile);
            lblDescription.Text = UI.Utils.SetFormString(projectInfo.Description);
            lblSpecialClause.Text = UI.Utils.SetFormString(projectInfo.SpecialClause);
            //#---
            lblLawOfSubcontract.Text = UI.Utils.SetFormString(projectInfo.LawOfSubcontract);

            lblAccountName.Text = UI.Utils.SetFormString(projectInfo.AccountName);
            lblBSB.Text = UI.Utils.SetFormString(projectInfo.BSB);
            lblAccountNumber.Text = UI.Utils.SetFormString(projectInfo.AccountNumber);


            //#--SAN---Siteaddress--
            lblSiteaddress.Text = UI.Utils.SetFormString(projectInfo.Siteaddress);
            lblSiteSuburb.Text = UI.Utils.SetFormString(projectInfo.SiteSuburb);
            lblSiteState.Text = UI.Utils.SetFormString(projectInfo.SiteState);
            lblSitePostalCode.Text = UI.Utils.SetFormString(projectInfo.SitePostalCode);
            //#--SAN---Siteaddress--




            //#---SAN-Principal Address


            lblPrincipaladdress.Text = UI.Utils.SetFormString(projectInfo.Principaladdress);
            lblPrincipalSuburb.Text = UI.Utils.SetFormString(projectInfo.PrincipalSuburb);
            lblPrincipalState.Text = UI.Utils.SetFormString(projectInfo.PrincipalState);
            lblPrincipalPostalCode.Text = UI.Utils.SetFormString(projectInfo.PrincipalPostalCode);




            lblContractAmount.Text = UI.Utils.SetFormDecimal(projectInfo.ContractAmount);

            lblClientContactFirstName.Text = UI.Utils.SetFormString(projectInfo.ClientContact.FirstName);
            lblClientContactLastName.Text = UI.Utils.SetFormString(projectInfo.ClientContact.LastName);
            lblClientContactCompanyName.Text = UI.Utils.SetFormString(projectInfo.ClientContact.CompanyName);
            lblClientContactStreet.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Street);
            lblClientContactLocality.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Locality);
            lblClientContactState.Text = UI.Utils.SetFormString(projectInfo.ClientContact.State);
            lblClientContactPostalCode.Text = UI.Utils.SetFormString(projectInfo.ClientContact.PostalCode);
            lblClientContactEmail.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Email);
            lblClientContactPhone.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Phone);
            lblClientContactFax.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Fax);
            sbvClientContactCV.Checked = projectInfo.SendCVToClientContact;
            sbvClientContactSA.Checked = projectInfo.SendSAToClientContact;
            sbvClientContactPC.Checked = projectInfo.SendPCToClientContact;
            sbvClientContactRFI.Checked = projectInfo.SendRFIToClientContact;
            sbvClientContactEOT.Checked = projectInfo.SendEOTToClientContact;

            lblClientContact1FirstName.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.FirstName);
            lblClientContact1LastName.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.LastName);
            lblClientContact1CompanyName.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.CompanyName);
            lblClientContact1Street.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Street);
            lblClientContact1Locality.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Locality);
            lblClientContact1State.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.State);
            lblClientContact1PostalCode.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.PostalCode);
            lblClientContact1Email.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Email);
            lblClientContact1Phone.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Phone);
            lblClientContact1Fax.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Fax);
            sbvClientContact1CV.Checked = projectInfo.SendCVToClientContact1;
            sbvClientContact1SA.Checked = projectInfo.SendSAToClientContact1;
            sbvClientContact1PC.Checked = projectInfo.SendPCToClientContact1;
            sbvClientContact1RFI.Checked = projectInfo.SendRFIToClientContact1;
            sbvClientContact1EOT.Checked = projectInfo.SendEOTToClientContact1;

            lblClientContact2FirstName.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.FirstName);
            lblClientContact2LastName.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.LastName);
            lblClientContact2CompanyName.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.CompanyName);
            lblClientContact2Street.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Street);
            lblClientContact2Locality.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Locality);
            lblClientContact2State.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.State);
            lblClientContact2PostalCode.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.PostalCode);
            lblClientContact2Email.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Email);
            lblClientContact2Phone.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Phone);
            lblClientContact2Fax.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Fax);
            sbvClientContact2CV.Checked = projectInfo.SendCVToClientContact2;
            sbvClientContact2SA.Checked = projectInfo.SendSAToClientContact2;
            sbvClientContact2PC.Checked = projectInfo.SendPCToClientContact2;
            sbvClientContact2RFI.Checked = projectInfo.SendRFIToClientContact2;
            sbvClientContact2EOT.Checked = projectInfo.SendEOTToClientContact2;

            lblSIFirstName.Text = UI.Utils.SetFormString(projectInfo.Superintendent.FirstName);
            lblSILastName.Text = UI.Utils.SetFormString(projectInfo.Superintendent.LastName);
            lblSICompanyName.Text = UI.Utils.SetFormString(projectInfo.Superintendent.CompanyName);
            lblSIStreet.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Street);
            lblSILocality.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Locality);
            lblSIState.Text = UI.Utils.SetFormString(projectInfo.Superintendent.State);
            lblSIPostalCode.Text = UI.Utils.SetFormString(projectInfo.Superintendent.PostalCode);
            lblSIEmail.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Email);
            lblSIPhone.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Phone);
            lblSIFax.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Fax);
            sbvSICV.Checked = projectInfo.SendCVToSuperintendent;
            sbvSISA.Checked = projectInfo.SendSAToSuperintendent;
            sbvSIPC.Checked = projectInfo.SendPCToSuperintendent;
            sbvSIRFI.Checked = projectInfo.SendRFIToSuperintendent;
            sbvSIEOT.Checked = projectInfo.SendEOTToSuperintendent;

            lblQSFirstName.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.FirstName);
            lblQSLastName.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.LastName);
            lblQSCompanyName.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.CompanyName);
            lblQSStreet.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Street);
            lblQSLocality.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Locality);
            lblQSState.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.State);
            lblQSPostalCode.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.PostalCode);
            lblQSEmail.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Email);
            lblQSPhone.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Phone);
            lblQSFax.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Fax);
            sbvQSCV.Checked = projectInfo.SendCVToQuantitySurveyor;
            sbvQSSA.Checked = projectInfo.SendSAToQuantitySurveyor;
            sbvQSPC.Checked = projectInfo.SendPCToQuantitySurveyor;
            sbvQSRFI.Checked = projectInfo.SendRFIToQuantitySurveyor;
            sbvQSEOT.Checked = projectInfo.SendEOTToQuantitySurveyor;

            lblSecondPrincipalFirstName.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.FirstName);
            lblSecondPrincipalLastName.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.LastName);
            lblSecondPrincipalStreet.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Street);
            lblSecondPrincipalLocality.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Locality);
            lblSecondPrincipalState.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.State);
            lblSecondPrincipalPostalCode.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.PostalCode);
            lblSecondPrincipalEmail.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Email);
            lblSecondPrincipalPhone.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Phone);

            lblPaymentTerms.Text = UI.Utils.SetFormString(Utils.GetConfigListItemName("Global", "PaymentTerms", projectInfo.PaymentTerms));
            lblClaimFrequency.Text = UI.Utils.SetFormString(Utils.GetConfigListItemName("Global", "ClaimFrequency", projectInfo.ClaimFrequency));

            if (projectInfo.ManagingDirector != null)
            {
                lnkGM.Text = UI.Utils.SetFormString(projectInfo.ManagingDirector.Name);
                lnkGM.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ManagingDirector.IdStr;
            }

            if (projectInfo.ContractsAdministrator != null)
            {
                lnkCA.Text = UI.Utils.SetFormString(projectInfo.ContractsAdministrator.Name);
                lnkCA.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ContractsAdministrator.IdStr;
            }

            if (projectInfo.ProjectManager != null)
            {
                lnkPM.Text = UI.Utils.SetFormString(projectInfo.ProjectManager.Name);
                lnkPM.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ProjectManager.IdStr;
            }

            if (projectInfo.ConstructionManager != null)
            {
                lnkCM.Text = UI.Utils.SetFormString(projectInfo.ConstructionManager.Name);
                lnkCM.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ConstructionManager.IdStr;
            }

            if (projectInfo.DesignManager != null)
            {
                lnkDM.Text = UI.Utils.SetFormString(projectInfo.DesignManager.Name);
                lnkDM.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.DesignManager.IdStr;
            }

            if (projectInfo.DesignCoordinator != null)
            {
                lnkDC.Text = UI.Utils.SetFormString(projectInfo.DesignCoordinator.Name);
                lnkDC.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.DesignCoordinator.IdStr;
            }

            if (projectInfo.FinancialController != null)
            {
                lnkFC.Text = UI.Utils.SetFormString(projectInfo.FinancialController.Name);
                lnkFC.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.FinancialController.IdStr;
            }

            if (projectInfo.DirectorAuthorization != null)
            {
                lnkDA.Text = UI.Utils.SetFormString(projectInfo.DirectorAuthorization.Name);
                lnkDA.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.DirectorAuthorization.IdStr;
            }

            if (projectInfo.BudgetAdministrator != null)
            {
                lnkBA.Text = UI.Utils.SetFormString(projectInfo.BudgetAdministrator.Name);
                lnkBA.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.BudgetAdministrator.IdStr;
            }

            //#----New Role COM and JCA
            if (projectInfo.CommercialManager != null)
            {
                lnkCO.Text = UI.Utils.SetFormString(projectInfo.CommercialManager.Name);
                lnkCO.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.CommercialManager.IdStr;
            }

            if (projectInfo.JuniorContractsAdministrator != null)
            {
                lnkJC.Text = UI.Utils.SetFormString(projectInfo.JuniorContractsAdministrator.Name);
                lnkJC.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.JuniorContractsAdministrator.IdStr;
            }


            if (projectInfo.ContractsAdministrator3 != null)
            {
                lnkCA3.Text = UI.Utils.SetFormString(projectInfo.ContractsAdministrator3.Name);
                lnkCA3.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ContractsAdministrator3.IdStr;
            }


            if (projectInfo.ContractsAdministrator4 != null)
            {
                lnkCA4.Text = UI.Utils.SetFormString(projectInfo.ContractsAdministrator4.Name);
                lnkCA4.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ContractsAdministrator4.IdStr;
            }



            if (projectInfo.ContractsAdministrator5 != null)
            {
                lnkCA5.Text = UI.Utils.SetFormString(projectInfo.ContractsAdministrator5.Name);
                lnkCA5.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ContractsAdministrator5.IdStr;
            }



            if (projectInfo.ContractsAdministrator6 != null)
            {
                lnkCA6.Text = UI.Utils.SetFormString(projectInfo.ContractsAdministrator6.Name);
                lnkCA6.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.ContractsAdministrator6.IdStr;
            }





            //#-----


            if (projectInfo.Foreman != null)
            {
                lnkForeman.Text = UI.Utils.SetFormString(projectInfo.Foreman.Name);
                lnkForeman.NavigateUrl = "~/Modules/People/ViewEmployee.aspx?PeopleId=" + projectInfo.Foreman.IdStr;
            }

            lnkBudgets.NavigateUrl = "~/Modules/Projects/ViewBudgets.aspx?Panel=0&ProjectId=" + projectInfo.IdStr;
            lnkTradesBreakup.NavigateUrl = "~/Modules/Projects/ViewClientTrades.aspx?ProjectId=" + projectInfo.IdStr;
            lnkClientVariations.NavigateUrl = "~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + projectInfo.IdStr + "&Type=" + ClientVariationInfo.VariationTypeClient;

            lnkTenantAccounts.NavigateUrl = "~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + projectInfo.IdStr + "&Type=" + ClientVariationInfo.VariationTypeTenant;

            lnkSeparateAccounts.NavigateUrl = "~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + projectInfo.IdStr + "&Type=" + ClientVariationInfo.VariationTypeSeparateAccounts;
            lnkClaims.NavigateUrl = "~/Modules/Projects/ListClaims.aspx?ProjectId=" + projectInfo.IdStr;
            lnkRFIs.NavigateUrl = "~/Modules/Projects/ListRFIs.aspx?ProjectId=" + projectInfo.IdStr;
            lnkEOTs.NavigateUrl = "~/Modules/Projects/ListEOTs.aspx?ProjectId=" + projectInfo.IdStr;
            lnkDrawingsTransmittals.NavigateUrl = "~/Modules/Projects/ListDrawingsTransmittals.aspx?ProjectId=" + projectInfo.IdStr;

            //#---
            lnkManageClientAccess.NavigateUrl = "~/Modules/People/EditClientContactAccess.aspx?ProjectId=" + projectInfo.IdStr;
            lnkMeetingMinutes.NavigateUrl = "~/Modules/Projects/ListMeetingMinutes.aspx?ProjectId=" + projectInfo.IdStr;
            lnkSiteInduction.NavigateUrl = "~/Modules/Induction/EditSiteInduction.aspx?ProjectId=" + projectInfo.IdStr;
            lnkSiteOrders.NavigateUrl = "~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + projectInfo.IdStr;

            //#---


            ddlJobTypes.Items.Add(new ListItem("All", String.Empty));
            ddlJobTypes1.Items.Add(new ListItem("All", String.Empty));
            foreach (JobTypeInfo jobType in jobTypes)
            {
                ddlJobTypes.Items.Add(new ListItem(jobType.Name, jobType.IdStr));
                ddlJobTypes1.Items.Add(new ListItem(jobType.Name, jobType.IdStr));
            }

            sflMaintenanceManual.FilePath = projectInfo.MaintenanceManualFile;
            sflMaintenanceManual.BasePath = projectInfo.AttachmentsFolder;
            sflMaintenanceManual.PageLink = String.Format("~/Modules/Projects/ShowProjectFile.aspx?FileType={0}&ProjectId={1}", ProjectInfo.FileTypeManual, projectInfo.IdStr);

            sflPostProjectReview.FilePath = projectInfo.PostProjectReviewFile;
            sflPostProjectReview.BasePath = projectInfo.AttachmentsFolder;
            sflPostProjectReview.PageLink = String.Format("~/Modules/Projects/ShowProjectFile.aspx?FileType={0}&ProjectId={1}", ProjectInfo.FileTypeReview, projectInfo.IdStr);

            BindTradesTender();
            BindTradesContract();
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            Session["SiteOrderNav"] = "";  //DS20230413
            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                String parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithTradesParticipations(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");
                int tCount = projectInfo.ContractTrades.Count;
                projectsController.SetProjectDueDays(projectInfo);



                //#----22/06/2016---To display project completion date with  Claimed and Approved EOTS with considering Holidays and RDOs and Weekends---

                projectInfo.EOTs = projectsController.GetEOTs(projectInfo);

                projectInfo.ClaimedEOTsCompletionDate = projectsController.GetProjectCompletionDateWithClaimedEOTs(projectInfo);

                projectInfo.ApprovedEOTsCompletionDate = projectsController.GetProjectCompletionDateWithApprovedEOTs(projectInfo);

                //#----22/06/2016--------






                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.ViewProject))
                    {
                        if (processController.AllowAddTradeCurrentUser(projectInfo))
                            phAddTrade.Visible = true;

                        if (processController.AllowEditCurrentUser(projectInfo))
                        {
                            cmdEditTop.Visible = true;
                            cmdDeleteTop.Visible = true;

                            if (projectInfo.Trades.Count > 0)
                                cmdDeleteTop.Enabled = false;
                            else
                                cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Project ?');");
                        }
                    }

                    BindProject();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditProject.aspx?ProjectId=" + projectInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteProject(projectInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/SearchProjects.aspx");
        }

        protected void butAddTradeTemplate_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradeTemplateInfo tradeTemplateInfo;

            try
            {
                if (ddlTradeTemplates.SelectedItem != null)
                {
                    tradeTemplateInfo = tradesController.GetDeepTradeTemplate(Int32.Parse(ddlTradeTemplates.SelectedValue));
                    tradesController.CopyTradeTemplate(projectInfo, tradeTemplateInfo);

                    tradeTemplateInfo.Trade.Project = projectInfo;

                    if (tradeTemplateInfo.Trade.DaysFromPCD != null)
                    {
                        projectsController.InitializeHolidays();
                        projectsController.UpdateTradeDates(tradeTemplateInfo.Trade);
                    }

                    projectInfo.Trades = tradesController.GetDeepTrades(projectInfo);
                    BindTradesTender();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ddlJobTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTradesContract();
            BindTradesTender();
        }

        protected void ddlJobTypes1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTradesTender();
        }
        #endregion

    }
}

