using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditParticipationPage : SOSPage
    {

#region Members
        private TradeParticipationInfo tradeParticipation = null;  // Quote participation
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeParticipation == null)
                return null;

            tempNode.ParentNode.Title = tradeParticipation.ComparisonParticipation.ProjectName + " (" + tradeParticipation.ComparisonParticipation.TradeName + ")";
            tempNode.ParentNode.Url += "?ParticipationId=" + tradeParticipation.ComparisonParticipation.IdStr;

            return currentNode;
        }

        private void CreateForm()
        {
            String scriptSelectLatestDrawings = String.Empty;
            String currStyle;
            HtmlTableRow row;
            HtmlTableCell cell;
            DropDownList dropDownList;
            ListItem listItem;
            List<DrawingInfo> includedDrawings = tradeParticipation.Trade.IncludedDrawings;
            Dictionary<int?, DrawingRevisionInfo> drawingDictionary = new Dictionary<int?, DrawingRevisionInfo>();

            // Creates dictionary with the drawings used in the invitation to tender
            if (tradeParticipation.ComparisonParticipation.Transmittal != null && tradeParticipation.ComparisonParticipation.Transmittal.TransmittalRevisions != null)
                foreach (TransmittalRevisionInfo transmittalRevisionInfo in tradeParticipation.ComparisonParticipation.Transmittal.TransmittalRevisions)
                    drawingDictionary.Add(transmittalRevisionInfo.Drawing.Id, transmittalRevisionInfo.Revision);

            currStyle = "";
            foreach (DrawingInfo drawingInfo in includedDrawings)
            {
                row = new HtmlTableRow();

                currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", currStyle);
                cell.InnerText = UI.Utils.SetFormString(drawingInfo.Name);
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", currStyle);

                dropDownList = new DropDownList();
                dropDownList.ID = "ddlDrawing_" + drawingInfo.IdStr;

                dropDownList.Items.Add(new ListItem(String.Empty, String.Empty));

                if (drawingDictionary.ContainsKey(drawingInfo.Id))
                    if (drawingDictionary[drawingInfo.Id].Equals(drawingInfo.LastRevision))
                    {
                        listItem = new ListItem(UI.Utils.SetFormString(drawingInfo.LastRevisionNumber) + " (" + UI.Utils.SetFormDate(drawingInfo.LastRevisionDate) + ")", drawingInfo.LastRevisionIdStr);
                        listItem.Attributes.Add("class", "lstTextNormal");
                        dropDownList.Items.Add(listItem);
                    }
                    else
                        if (drawingInfo.DrawingRevisions != null)
                        {
                            foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
                                if (drawingDictionary[drawingInfo.Id].Equals(drawingRevisionInfo))
                                {
                                    listItem = new ListItem(UI.Utils.SetFormString(drawingRevisionInfo.Number) + " (" + UI.Utils.SetFormDate(drawingRevisionInfo.RevisionDate) + ")", drawingRevisionInfo.IdStr);
                                    listItem.Attributes.Add("class", "lstTextGrayedOut");
                                    dropDownList.Items.Add(listItem);
                                }
                                else if (drawingRevisionInfo.RevisionDate != null && drawingDictionary[drawingInfo.Id].RevisionDate != null && drawingRevisionInfo.RevisionDate >= drawingDictionary[drawingInfo.Id].RevisionDate)
                                {
                                    listItem = new ListItem(UI.Utils.SetFormString(drawingRevisionInfo.Number) + " (" + UI.Utils.SetFormDate(drawingRevisionInfo.RevisionDate) + ")", drawingRevisionInfo.IdStr);
                                    listItem.Attributes.Add("class", "lstTextHighlighted");
                                    dropDownList.Items.Add(listItem);
                                }
                        }
                        else
                            dropDownList = null;
                else
                    if (drawingInfo.LastRevision != null)
                    {
                        listItem = new ListItem(UI.Utils.SetFormString(drawingInfo.LastRevisionNumber) + " (" + UI.Utils.SetFormDate(drawingInfo.LastRevisionDate) + ")", drawingInfo.LastRevisionIdStr);
                        listItem.Attributes.Add("class", "lstTextHighlighted");
                        dropDownList.Items.Add(listItem);
                    }
                    else
                        dropDownList = null;

                row.Cells.Add(cell);
                tblDrawings.Rows.Add(row);

                if (dropDownList != null)
                {
                    cell.Controls.Add(dropDownList);
                    scriptSelectLatestDrawings = scriptSelectLatestDrawings + String.Format("document.getElementById('{0}').value={1}\r", dropDownList.ClientID, drawingInfo.LastRevisionIdStr);
                }
            }

            if (scriptSelectLatestDrawings != String.Empty)
            {
                scriptSelectLatestDrawings = "" +
                "<script language='JavaScript'>\r" +
                "function SelectLatestDrawings() {\r" +
                "   if (confirm('Assign Latest Revision to All Drawings?')) {\r" +
                scriptSelectLatestDrawings +
                "   }\r" +
                "}\r" +
                "</script>\r";

                // No need to check for script already register
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "SelectLatestDrawings", scriptSelectLatestDrawings);

                cmdSelectLatestDrawings.OnClientClick = "SelectLatestDrawings(); return false;";
            }

            EditComparison1.TradeParticipation = tradeParticipation;
        }

        private void ObjectsToForm()
        {
            DropDownList dropDownList;
            List<DrawingInfo> includedDrawings = tradeParticipation.Trade.IncludedDrawings;
            TransmittalRevisionInfo transmittalRevisionInfo;

            txtQuoteAmount.Text = UI.Utils.SetFormEditDecimal(tradeParticipation.Amount);
            txtComments.Text = UI.Utils.SetFormString(tradeParticipation.Comments);

            if (tradeParticipation.Transmittal != null && tradeParticipation.Transmittal.TransmittalRevisions != null)
                foreach (DrawingInfo drawingInfo in includedDrawings)
                {
                    dropDownList = (DropDownList)Utils.FindControlRecursive(tblDrawings, "ddlDrawing_" + drawingInfo.IdStr);

                    if (dropDownList != null)
                    {
                        transmittalRevisionInfo = tradeParticipation.Transmittal.TransmittalRevisions.Find(delegate(TransmittalRevisionInfo TransmittalRevisionInfoInList) { return drawingInfo.Equals(TransmittalRevisionInfoInList.Drawing); });

                        if (transmittalRevisionInfo != null)
                            dropDownList.SelectedValue = transmittalRevisionInfo.RevisionIdStr;
                    }
                }
        }

        private void FormToObjects()
        {
            DropDownList dropDownList;
            List<DrawingInfo> includedDrawings = tradeParticipation.Trade.IncludedDrawings;

            tradeParticipation = EditComparison1.TradeParticipation;
            tradeParticipation.Amount = UI.Utils.GetFormDecimal(txtQuoteAmount.Text);
            tradeParticipation.Comments = UI.Utils.GetFormString(txtComments.Text);

            if (tradeParticipation.Transmittal == null)
            {
                tradeParticipation.Transmittal = new TransmittalInfo();
                tradeParticipation.Transmittal.Type = tradeParticipation.Type;
            }

            tradeParticipation.Transmittal.TransmittalRevisions = new List<TransmittalRevisionInfo>();

            foreach (DrawingInfo drawingInfo in includedDrawings)
            {
                dropDownList = (DropDownList)Utils.FindControlRecursive(tblDrawings, "ddlDrawing_" + drawingInfo.IdStr);

                if (dropDownList != null && dropDownList.SelectedValue != String.Empty)
                    tradeParticipation.Transmittal.TransmittalRevisions.Add(new TransmittalRevisionInfo(1, new DrawingRevisionInfo(Int32.Parse(dropDownList.SelectedValue)), tradeParticipation.Transmittal));
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            String parameterParticipationId;

            try
            {
                Security.CheckAccess(Security.userActions.EditParticipationSubContractor);
                parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipation = tradesController.GetDeepTradeParticipationWithQuoteAndTransmittal(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipation, parameterParticipationId, "Trade Participation");

                if (tradeParticipation.ComparisonParticipation == null)
                    throw new Exception("Comparison participation does not exist");

                tradeParticipation.ComparisonParticipation = tradesController.GetTradeParticipationWithTransmittal(tradeParticipation.ComparisonParticipation.Id);
                tradeParticipation.ComparisonParticipation.QuoteParticipation = tradeParticipation;

                tradeParticipation.Trade = tradesController.GetTradeWithDrawingsAndItems(tradeParticipation.Trade.Id);
                tradeParticipation.Trade.Project = tradeParticipation.IsActive ? projectsController.GetProjectWithDrawingsActive(tradeParticipation.Trade.Project.Id) : projectsController.GetProjectWithDrawingsProposal(tradeParticipation.Trade.Project.Id);

                contractsController.CheckEditCurrentUser(tradeParticipation.ComparisonParticipation);

                CreateForm();

                if (!Page.IsPostBack)
                    ObjectsToForm();
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
                TradesController tradesController = TradesController.GetInstance();
                ProjectsController projectsController = ProjectsController.GetInstance();

                if (Page.IsValid)
                {
                    FormToObjects();

                    tradesController.UpdateTradeParticipationItems(tradeParticipation);
                    projectsController.AddUpdateTransmittal(tradeParticipation.Transmittal);
                    tradesController.UpdateTradeParticipation(tradeParticipation);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect(String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", tradeParticipation.ComparisonParticipation.IdStr));
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/Modules/Projects/ViewParticipationSubContractor.aspx?ParticipationId={0}", tradeParticipation.ComparisonParticipation.IdStr));
        }
#endregion

    }
}
