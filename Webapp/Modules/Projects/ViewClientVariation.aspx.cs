using System;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewClientVariationPage : SOSPage
    {

        #region Members
        private ClientVariationInfo clientVariationInfo;
        private ClientVariationItemInfo clientVariationItemInfo;
        private ClientVariationItemInfo newClientVariationItemInfo;
        private ClientVariationTradeInfo clientVariationTradeInfo;
        private ClientVariationTradeInfo newClientVariationTradeInfo;
        private List<IBudget> projectBudget;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (clientVariationInfo == null)
                return null;

            //#--TV--tempNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Client Variation";
            tempNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : clientVariationInfo is TenantVariationInfo ? "Tenant Variation" : "Client Variation";//----TV---#

            //#----TV-- tempNode.ParentNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Accounts" : "Client Variations";
            tempNode.ParentNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Accounts" : clientVariationInfo is TenantVariationInfo ? "Tenant Variation" : "Client Variations";//----TV---#


            tempNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr + "&Type=" + clientVariationInfo.Type;

            tempNode.ParentNode.ParentNode.Title = clientVariationInfo.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr;

            return currentNode;
        }

        private HtmlTableCell EmptyCell(String style)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.InnerHtml = "&nbsp;";
            cell.Align = "Center";

            return cell;
        }

        private HtmlTableCell LastCell(int colSpan = 2)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.InnerHtml = "&nbsp;";
            cell.ColSpan = colSpan;

            return cell;
        }

        private HtmlTableCell NumberCell(String style, int number)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.InnerText = number.ToString();
            cell.Attributes.Add("class", style);
            cell.Align = "Center";

            return cell;
        }

        private HtmlTableCell TextCell(String style, String theText, String alignment)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.Align = alignment;
            cell.InnerText = theText;

            return cell;
        }

        protected HtmlTableCell DecimalCell(String style, Decimal? amount)
        {
            HtmlTableCell cell = new HtmlTableCell();

            if (amount < 0)
                style = style + "Negative";

            cell.Attributes.Add("class", style);
            cell.Align = "Right";

            cell.InnerText = UI.Utils.SetFormDecimal(amount);
            return cell;
        }

        private HtmlTableCell EditCell(String style, String url)
        {
            HyperLink hyperLink = new HyperLink();
            HtmlTableCell cell = new HtmlTableCell();

            cell.Attributes.Add("class", style);
            hyperLink.CssClass = "frmLink";
            hyperLink.ToolTip = "Edit";
            hyperLink.ImageUrl = "~/Images/IconEdit.gif";
            hyperLink.NavigateUrl = url;
            cell.Controls.Add(hyperLink);

            return cell;
        }

        private HtmlTableCell TotalCell(String style, String idStr)
        {
            Label label = new Label();
            HtmlTableCell cell = new HtmlTableCell();

            label.ID = idStr;

            cell.Attributes.Add("class", style);
            cell.Align = "Right";
            cell.Controls.Add(label);

            return cell;
        }

        private HtmlTableCell DeleteCell(String style, String idStr, String message, EventHandler eventHandler)
        {
            HtmlAnchor htmlAnchor = new HtmlAnchor();
            HtmlTableCell cell = new HtmlTableCell();
            Image image = new Image();

            cell.Attributes.Add("class", style);
            htmlAnchor.Attributes.Add("class", "lstLink");
            htmlAnchor.Attributes.Add("title", "Delete");
            htmlAnchor.Attributes.Add("href", idStr);
            htmlAnchor.Attributes.Add("onclick", "javascript:return confirm('" + message + "');");
            htmlAnchor.ServerClick += eventHandler;
            image.ImageUrl = "~/Images/IconDelete.gif";
            htmlAnchor.Controls.Add(image);
            cell.Controls.Add(htmlAnchor);

            return cell;
        }

        //----San-060223------------------------
        private HtmlTableCell MoveUPCell(String style, String idStr, String message, EventHandler eventHandler)
        {
            HtmlAnchor htmlAnchor = new HtmlAnchor();
            HtmlTableCell cell = new HtmlTableCell();
            Image image = new Image();

            cell.Attributes.Add("class", style);
            htmlAnchor.Attributes.Add("class", "lstLink");
            htmlAnchor.Attributes.Add("title", "Move Up");
            htmlAnchor.Attributes.Add("href", idStr);
            // htmlAnchor.Attributes.Add("onclick", "javascript:return confirm('" + message + "');");
            htmlAnchor.ServerClick += eventHandler;
            image.ImageUrl = "~/Images/IconUp.gif";
            htmlAnchor.Controls.Add(image);
            cell.Controls.Add(htmlAnchor);

            return cell;
        }

        private HtmlTableCell MoveDownCell(String style, String idStr, String message, EventHandler eventHandler)
        {
            HtmlAnchor htmlAnchor = new HtmlAnchor();
            HtmlTableCell cell = new HtmlTableCell();
            Image image = new Image();

            cell.Attributes.Add("class", style);
            htmlAnchor.Attributes.Add("class", "lstLink");
            htmlAnchor.Attributes.Add("title", "Move Down");
            htmlAnchor.Attributes.Add("href", idStr);
            // htmlAnchor.Attributes.Add("onclick", "javascript:return confirm('" + message + "');");
            htmlAnchor.ServerClick += eventHandler;
            image.ImageUrl = "~/Images/IconDown.gif";
            htmlAnchor.Controls.Add(image);
            cell.Controls.Add(htmlAnchor);

            return cell;
        }


        //----San-060223------------------------


        private HtmlTableCell TextBoxCell(String style, String id, String width, Int32 rows, Boolean withValidator)
        {
            HtmlTableCell cell = new HtmlTableCell();
            TextBox textBox = new TextBox();

            cell.Attributes.Add("class", style);

            textBox.ID = id;
            textBox.Width = new Unit(width);
            textBox.Rows = rows;

            if (rows > 1)
                textBox.TextMode = TextBoxMode.MultiLine;

            if (withValidator)
            {
                CompareValidator compareValidator = new CompareValidator();
                compareValidator.ControlToValidate = textBox.ClientID;
                compareValidator.ErrorMessage = "Invalid number!<br />";
                compareValidator.CssClass = "frmError";
                compareValidator.Display = ValidatorDisplay.Dynamic;
                compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
                compareValidator.Type = ValidationDataType.Currency;

                cell.Controls.Add(compareValidator);
            }

            cell.Controls.Add(textBox);

            return cell;
        }

        private HtmlTableCell TradesCodesCell(String style, String id, String currentTradeCode, List<TradeTemplateInfo> tradeTemplateInfoList)
        {
            HtmlTableCell cell = new HtmlTableCell();
            DropDownList dropDownList = new DropDownList();

            cell.Attributes.Add("class", style);
            dropDownList.ID = id;

            //#--
            tradeTemplateInfoList.Sort((x, y) => x.Trade.Code.CompareTo(y.Trade.Code));
            //#--

            // create trade codes list with all trade templates
            dropDownList.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
            { //#--- dropDownList.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName, tradeTemplateInfo.TradeCode));
              //#---  to display -------Code - TrdaeName - JobType---- -
                dropDownList.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " - " + tradeTemplateInfo.TradeName + " - " + tradeTemplateInfo.Trade.JobTypeName, tradeTemplateInfo.TradeCode));

            }
            // remove trades already in use except the current one if exist
            foreach (ClientVariationTradeInfo clientVariationTradeInfo in clientVariationInfo.Trades)
                if (currentTradeCode == null || !currentTradeCode.Equals(clientVariationTradeInfo.TradeCode))
                    dropDownList.Items.Remove(dropDownList.Items.FindByValue(clientVariationTradeInfo.TradeCode));

            if (currentTradeCode == null)
                if (dropDownList.Items.FindByValue(currentTradeCode) != null)
                    dropDownList.SelectedValue = currentTradeCode;

            cell.Controls.Add(dropDownList);

            return cell;
        }

        private HtmlTableCell UpdateCell(String style, String id, EventHandler eventHandler, int colSpan = 2)
        {
            Button button = new Button();
            HtmlTableCell cell = new HtmlTableCell();

            cell.Attributes.Add("class", style);
            cell.ColSpan = colSpan;
            cell.Align = "Center";

            button = new Button();
            button.ID = id;
            button.Text = "Update";
            button.Click += eventHandler;

            cell.Controls.Add(button);

            return cell;
        }

        private void BindTree(XmlNode xmlNode, TreeView treeView)
        {
            treeView.Nodes.Clear();
            treeView.Nodes.Add(new TreeNode());
            Utils.AddNode(xmlNode, treeView.Nodes[0]);
            treeView.Visible = true;
        }

        private HtmlTableCell AddCell(String style, String id, EventHandler eventHandler, int colSpan = 2)
        {
            Button button = new Button();
            HtmlTableCell cell = new HtmlTableCell();

            cell.Attributes.Add("class", style);
            cell.ColSpan = colSpan;
            cell.Align = "Center";

            button = new Button();
            button.ID = id;
            button.Text = "Add";
            button.Click += eventHandler;

            cell.Controls.Add(button);

            return cell;
        }

        private void CreateForm()
        {
            HtmlTableRow row;
            String currStyle = null;
            String tradeCodeAndName;
            TradeTemplateInfo tradeTemplateInfo;
            List<TradeTemplateInfo> tradeTemplateInfoList;
            ClientVariationTradeInfo clientVariationTradeInBudget;
            Boolean tradeInUse = false;
            Boolean atLeastOnetradeInUse = false;

            // list of items
            foreach (ClientVariationItemInfo clientVariationItem in clientVariationInfo.Items)
            {
                currStyle = currStyle == "lstItemWrap" ? "lstAltItemWrap" : "lstItemWrap";

                row = new HtmlTableRow();

                // an item is being edited
                if (clientVariationItem.Equals(clientVariationItemInfo))
                {
                    row.Cells.Add(NumberCell(currStyle, tblItems.Rows.Count - 1));
                    row.Cells.Add(TextBoxCell(currStyle, "txtDescription", "420px", 3, false));
                    row.Cells.Add(TextBoxCell(currStyle, "txtAmountItem", "100px", 1, true));
                    row.Cells.Add(UpdateCell(currStyle, "cmdUpdateItem", new System.EventHandler(cmdUpdateItem_Click)));
                }
                else
                {
                    row.Cells.Add(NumberCell(currStyle, tblItems.Rows.Count - 1));
                    row.Cells.Add(TextCell(currStyle, UI.Utils.SetFormString(clientVariationItem.Description), "Left"));
                    row.Cells.Add(DecimalCell(currStyle, clientVariationItem.Amount));

                    // the variation is editable
                    if (newClientVariationItemInfo != null)
                    {
                        row.Cells.Add(EditCell(currStyle, String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}&ClientVariationItemId={1}", clientVariationInfo.IdStr, clientVariationItem.IdStr)));
                        row.Cells.Add(DeleteCell(currStyle, clientVariationItem.IdStr, String.Format("Delete Item {0}?", (tblItems.Rows.Count - 1).ToString()), new System.EventHandler(lnkDeleteItem_Click)));
                        //---San---- //----San-060223------------------------
                        row.Cells.Add(MoveUPCell(currStyle, clientVariationItem.IdStr, String.Format("Move Up {0}?", (tblItems.Rows.Count - 1).ToString()), new System.EventHandler(lnkMoveUpItem_Click)));
                        row.Cells.Add(MoveDownCell(currStyle, clientVariationItem.IdStr, String.Format("Move Down{0}?", (tblItems.Rows.Count - 1).ToString()), new System.EventHandler(lnkMoveDownItem_Click)));
                        //----San-060223------------------------

                    }
                }

                tblItems.Rows.Add(row);
            }

            // form to add new item
            if (newClientVariationItemInfo != null)
            {
                row = new HtmlTableRow();

                row.Cells.Add(EmptyCell("lstBlankItem"));
                row.Cells.Add(TextBoxCell("lstBlankItem", "txtNewDescription", "420px", 3, false));
                row.Cells.Add(TextBoxCell("lstBlankItem", "txtNewAmountItem", "100px", 1, true));
                row.Cells.Add(AddCell("lstBlankItem", "cmdAddItem", new System.EventHandler(cmdAddItem_Click)));

                tblItems.Rows.Add(row);
            }

            // total items row
            row = new HtmlTableRow();

            row.Cells.Add(EmptyCell("lstHeader"));
            row.Cells.Add(TextCell("lstHeader", "Total:", "Right"));
            row.Cells.Add(TotalCell("lstHeader", "lblTotalItems"));

            if (newClientVariationItemInfo != null)
            {
                row.Cells.Add(LastCell());
                tblItems.Rows[1].Cells.Add(LastCell());
            }

            tblItems.Rows.Add(row);





            // list of trades     -------------------------------------------------------------

            tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
            foreach (ClientVariationTradeInfo clientVariationTrade in clientVariationInfo.Trades)
            {
                currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                row = new HtmlTableRow();

                // a trade is being edited
                if (clientVariationTrade.Equals(clientVariationTradeInfo))
                {
                    row.Cells.Add(NumberCell(currStyle, tblTrades.Rows.Count));
                    row.Cells.Add(TradesCodesCell(currStyle, "ddlTradeCode", clientVariationTrade.TradeCode, tradeTemplateInfoList));
                    row.Cells.Add(TextBoxCell(currStyle, "txtAmountTrade", "100px", 1, true));
                    row.Cells.Add(UpdateCell(currStyle, "cmdUpdateTrade", new System.EventHandler(cmdUpdateTrade_Click), 3));
                }
                else
                {
                    tradeTemplateInfo = tradeTemplateInfoList.Find(delegate (TradeTemplateInfo tradeTemplateInfoInList) { return tradeTemplateInfoInList.TradeCode.Equals(clientVariationTrade.TradeCode); });
                    tradeCodeAndName = tradeTemplateInfo != null ? tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName : null;

                    row.Cells.Add(NumberCell(currStyle, tblTrades.Rows.Count));
                    row.Cells.Add(TextCell(currStyle, UI.Utils.SetFormString(tradeCodeAndName), "Left"));
                    row.Cells.Add(DecimalCell(currStyle, clientVariationTrade.Amount));

                    //Only the active CVs are in the budget.
                    clientVariationTradeInBudget = (ClientVariationTradeInfo)projectBudget.Find(pb => pb.Equals(clientVariationTrade));

                    if (clientVariationTradeInBudget != null)
                    {
                        row.Cells.Add(DecimalCell(currStyle, clientVariationTradeInBudget.BudgetAmountInitial.Value));
                        tradeInUse = projectBudget.Any(pb => clientVariationTrade.Equals(pb.BudgetProvider));
                    }
                    else
                        row.Cells.Add(TextCell(currStyle, "N/A", "Center"));

                    // the variation is editable
                    if (newClientVariationTradeInfo != null)
                    {
                        row.Cells.Add(EditCell(currStyle, String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}&ClientVariationTradeId={1}", clientVariationInfo.IdStr, clientVariationTrade.IdStr)));

                        if (!tradeInUse)
                        {
                            row.Cells.Add(DeleteCell(currStyle, clientVariationTrade.IdStr, String.Format("Delete Trade {0}?", tradeCodeAndName), new System.EventHandler(lnkDeleteTrade_Click)));
                        }
                        else
                        {
                            row.Cells.Add(EmptyCell(currStyle));
                            atLeastOnetradeInUse = true;
                        }
                    }
                }

                tblTrades.Rows.Add(row);
            }




            // form to add new trade
            if (newClientVariationTradeInfo != null)
            {
                row = new HtmlTableRow();

                row.Cells.Add(EmptyCell("lstBlankItem"));
                row.Cells.Add(TradesCodesCell("lstBlankItem", "ddlNewTradeCode", null, tradeTemplateInfoList));
                row.Cells.Add(TextBoxCell("lstBlankItem", "txtNewAmountTrade", "100px", 1, true));
                row.Cells.Add(AddCell("lstBlankItem", "cmdAddTrade", new System.EventHandler(cmdAddTrade_Click), 3));

                tblTrades.Rows.Add(row);
            }

            // total trades row
            row = new HtmlTableRow();

            row.Cells.Add(EmptyCell("lstHeader"));
            row.Cells.Add(TextCell("lstHeader", "Total:", "Right"));
            row.Cells.Add(TotalCell("lstHeader", "lblTotalTrades"));
            row.Cells.Add(EmptyCell("lstHeader"));

            if (newClientVariationItemInfo != null)
            {
                row.Cells.Add(LastCell(3));
                tblTrades.Rows[0].Cells.Add(LastCell());
            }

            tblTrades.Rows.Add(row);

            if (atLeastOnetradeInUse)
                phCancel.Visible = false;
        }

        private void BindApproval()
        {
            ProcessManagerCV.Process = clientVariationInfo.Process;
            ProcessManagerCV.Enabled = clientVariationInfo.IsLastSubClientVariation && !clientVariationInfo.IsCancel;
            ProcessManagerCV.ApproveClicked += new System.EventHandler(cmdApprove_Click);
            ProcessManagerCV.ReverseClicked += new System.EventHandler(cmdReverse_Click);

            ProcessManagerCV.BindApproval();
        }

        private void BindVariationItems()
        {
            if (clientVariationItemInfo != null)
            {
                ((TextBox)Utils.FindControlRecursive(tblItems, "txtDescription")).Text = UI.Utils.SetFormString(clientVariationItemInfo.Description);
                ((TextBox)Utils.FindControlRecursive(tblItems, "txtAmountItem")).Text = UI.Utils.SetFormEditDecimal(clientVariationItemInfo.Amount);
                ((TextBox)Utils.FindControlRecursive(tblItems, "txtAmountItem")).Focus();
            }
            else
                if (newClientVariationItemInfo != null)
                ((TextBox)Utils.FindControlRecursive(tblItems, "txtNewDescription")).Focus();
        }

        private void BindVariationTrades()
        {
            if (clientVariationTradeInfo != null)
            {
                if (clientVariationTradeInfo.TradeCode != null)
                    ((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlTradeCode")).SelectedValue = UI.Utils.SetFormString(clientVariationTradeInfo.TradeCode);

                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmountTrade")).Text = UI.Utils.SetFormEditDecimal(clientVariationTradeInfo.Amount);
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmountTrade")).Focus();
            }
        }

        private void BindVariationTotals()
        {
            ((Label)Utils.FindControlRecursive(tblItems, "lblTotalItems")).Text = UI.Utils.SetFormDecimal(clientVariationInfo.TotalAmount);
            ((Label)Utils.FindControlRecursive(tblTrades, "lblTotalTrades")).Text = UI.Utils.SetFormDecimal(clientVariationInfo.TotalTrades);

            if (clientVariationInfo.ItemsMinusTrades != null)
                if (clientVariationInfo.ItemsMinusTrades != 0)
                {
                    lblDifference.Text = UI.Utils.SetFormDecimal(clientVariationInfo.ItemsMinusTrades);
                    pnlTotals.Visible = true;
                }
        }

        private void BindClientVariation()
        {
            ProcessController processController = ProcessController.GetInstance();

            if (clientVariationInfo is SeparateAccountInfo)
            {
                SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                TitleBar1.Title = "Separate Account";
                Title = "Separate Account";

                pnlVerbalApprovalDate.Visible = false;

                cmdCancel.OnClientClick = "javascript:return confirm('Cancel Separate Account');";
                cmdRestore.OnClientClick = "javascript:return confirm('Restore Separate Account');";
                lnkViewClientVariation.Text = "Separate Account";

                if (separateAccountInfo.WorksCompleted)
                {
                    lnkInvoice.NavigateUrl = "~/Modules/Projects/ShowSeparateAccountInvoice.aspx?ClientVariationId=" + clientVariationInfo.IdStr;
                    lnkInvoice.Visible = true;

                    phInvoice.Visible = true;

                    lblInvoiceNumber.Text = UI.Utils.SetFormInteger(separateAccountInfo.InvoiceNumber);
                    lblInvoiceDate.Text = UI.Utils.SetFormDate(separateAccountInfo.InvoiceDate);
                    lblInvoiceSentDate.Text = UI.Utils.SetFormDate(separateAccountInfo.InvoiceSentDate);
                    lblInvoiceDueDate.Text = UI.Utils.SetFormDate(separateAccountInfo.InvoiceDueDate);
                    lblInvoicePaidDate.Text = UI.Utils.SetFormDate(separateAccountInfo.InvoicePaidDate);
                }
                else
                {
                    phInvoice.Visible = false;
                }

                phUseSecondPrincipal.Visible = true;
                sbvUseSecondPrincipal.Checked = separateAccountInfo.UseSecondPrincipal;
            }


            //----#--TV----
            else if (clientVariationInfo is TenantVariationInfo)
            {
                TitleBar1.Title = "Tenant Variation";
                Title = "Tenant Variation";

                cmdCancel.OnClientClick = "javascript:return confirm('Cancel Tenant Variation');";
                cmdRestore.OnClientClick = "javascript:return confirm('Restore Tenant Variation');";

                lnkViewClientVariation.Text = "Tenant Variation";

                pnlVerbalApprovalDate.Visible = true;
                phInvoice.Visible = false;
                phUseSecondPrincipal.Visible = false;
            }
            //-----TV---

            else
            {
                TitleBar1.Title = "Client Variation";
                Title = "Client Variation";

                cmdCancel.OnClientClick = "javascript:return confirm('Cancel Client Variation');";
                cmdRestore.OnClientClick = "javascript:return confirm('Restore Client Variation');";

                lnkViewClientVariation.Text = "Client Variation";

                pnlVerbalApprovalDate.Visible = true;
                phInvoice.Visible = false;
                phUseSecondPrincipal.Visible = false;
            }

            if (clientVariationInfo.IsLastSubClientVariation)
            {
                if (!clientVariationInfo.IsTheParentClientVariation)
                {
                    ddlSubClientVariations.Items.Add(new ListItem(clientVariationInfo.TheParentClientVariation.RevisionName, clientVariationInfo.TheParentClientVariation.IdStr));

                    foreach (ClientVariationInfo subClientVariationInfo in clientVariationInfo.TheParentClientVariation.SubClientVariations)
                        ddlSubClientVariations.Items.Add(new ListItem(subClientVariationInfo.RevisionName, subClientVariationInfo.IdStr));

                    ddlSubClientVariations.SelectedIndex = clientVariationInfo.TheParentClientVariation.SubClientVariations.Count;

                    phSubClientVariations.Visible = true;
                }

                if (clientVariationInfo.IsCancel)
                {
                    phCancelDate.Visible = true;
                    lblCancelDate.Text = UI.Utils.SetFormDate(clientVariationInfo.TheParentClientVariation.CancelDate);
                }
            }
            else
            {
                lnkLastSubClientVariation.NavigateUrl = "~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.LastSubClientVariation.IdStr;
                phLastSubClientVariation.Visible = true;
            }

            // if the client variation is editable shows the Edit and Delete buttons
            if (newClientVariationItemInfo != null)
            {
                pnlEdit.Visible = true;
                cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Client Variation?');");
            }

            lblNumber.Text = UI.Utils.SetFormInteger(clientVariationInfo.Number);
            lblRevisionName.Text = UI.Utils.SetFormString(clientVariationInfo.RevisionName);
            lblStatus.Text = UI.Utils.SetFormString(clientVariationInfo.Status);
            lblName.Text = UI.Utils.SetFormString(clientVariationInfo.Name);
            lblWriteDate.Text = UI.Utils.SetFormDate(clientVariationInfo.WriteDate);
            lblVerbalApprovalDate.Text = UI.Utils.SetFormDate(clientVariationInfo.VerbalApprovalDate);
            lblApprovalDate.Text = UI.Utils.SetFormDate(clientVariationInfo.ApprovalDate);
            lblQuotesFileName.Text = UI.Utils.SetFormString(clientVariationInfo.QuotesFile);
            lblBackupFileName.Text = UI.Utils.SetFormString(clientVariationInfo.BackupFile);
            lblClientApprovalFileName.Text = UI.Utils.SetFormString(clientVariationInfo.ClientApprovalFile);
            sbvShowCostDetails.Checked = clientVariationInfo.ShowCostDetails;
            lblComments.Text = UI.Utils.SetFormString(clientVariationInfo.Comments);

            sflQuotesFile.FilePath = clientVariationInfo.QuotesFile;
            sflQuotesFile.BasePath = clientVariationInfo.Project.AttachmentsFolder;
            sflQuotesFile.PageLink = String.Format("~/Modules/Projects/ShowClientVariationFile.aspx?FileType={0}&ClientVariationId={1}", ClientVariationInfo.FileTypeQuotes, clientVariationInfo.IdStr);

            sflBackupFile.FilePath = clientVariationInfo.BackupFile;
            sflBackupFile.BasePath = clientVariationInfo.Project.AttachmentsFolder;
            sflBackupFile.PageLink = String.Format("~/Modules/Projects/ShowClientVariationFile.aspx?FileType={0}&ClientVariationId={1}", ClientVariationInfo.FileTypeBackup, clientVariationInfo.IdStr);

            sflClientApprovalFile.FilePath = clientVariationInfo.ClientApprovalFile;
            sflClientApprovalFile.BasePath = clientVariationInfo.Project.AttachmentsFolder;
            sflClientApprovalFile.PageLink = String.Format("~/Modules/Projects/ShowClientVariationFile.aspx?FileType={0}&ClientVariationId={1}", ClientVariationInfo.FileTypeClientApproval, clientVariationInfo.IdStr);

            if (clientVariationInfo.IsInternallyApproved)
            {
                phViewClientVariation.Visible = true;
                lnkViewClientVariation.NavigateUrl = "~/Modules/Projects/ShowClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr;
            }

            XmlDocument xmlDocument = ProjectsController.GetInstance().CheckClientVariation(clientVariationInfo);
            if (xmlDocument.DocumentElement != null)
            {
                if (xmlDocument.DocumentElement.ChildNodes.Count == 2)
                {
                    BindTree(xmlDocument.DocumentElement.ChildNodes[0], TreeViewMissingFields);
                    BindTree(xmlDocument.DocumentElement.ChildNodes[1], TreeViewErrors);
                }
                else
                    if (xmlDocument.DocumentElement.ChildNodes[0].Name == "Fields")
                    BindTree(xmlDocument.DocumentElement.ChildNodes[0], TreeViewMissingFields);
                else
                    BindTree(xmlDocument.DocumentElement.ChildNodes[0], TreeViewErrors);

                pnlErrors.Visible = true;
            }

            ProcessManagerCV.StepId = null;

            BindApproval();
            BindVariationItems();
            BindVariationTrades();
            BindVariationTotals();
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewClientVariation);
                String parameterClientVariationId = Utils.CheckParameter("ClientVariationId");
                clientVariationInfo = projectsController.GetClientVariationWithItemsAndTrades(Int32.Parse(parameterClientVariationId));
                Core.Utils.CheckNullObject(clientVariationInfo, parameterClientVariationId, "Client Variation");

                clientVariationInfo.TheParentClientVariation.SubClientVariations = projectsController.GetClientVariations(clientVariationInfo.TheParentClientVariation);
                projectsController.SetRevisionNames(clientVariationInfo);

                clientVariationInfo.Project = projectsController.GetProject(clientVariationInfo.Project.Id);
                clientVariationInfo.Project.ClientVariations = new List<ClientVariationInfo>();
                clientVariationInfo.Project.ClientVariations.Add(clientVariationInfo);

                projectBudget = projectsController.GetProjectBudget(clientVariationInfo.Project, sbiTrades.IncludeAllCVSA, sbiTrades.IncludeAllOVO);

                if (clientVariationInfo.Process != null)
                    clientVariationInfo.Process.Project = clientVariationInfo.Project;

                if (Security.ViewAccess(Security.userActions.ViewClientVariation))
                {
                    if (clientVariationInfo.IsLastSubClientVariation)
                    {
                        if (processController.AllowEditCurrentUser(clientVariationInfo))
                        {
                            if (!clientVariationInfo.IsCancel)
                            {
                                newClientVariationItemInfo = new ClientVariationItemInfo(clientVariationInfo);
                                newClientVariationTradeInfo = new ClientVariationTradeInfo(clientVariationInfo);

                                String parameterClientVariationItemId = Request.Params["ClientVariationItemId"];
                                if (parameterClientVariationItemId != null)
                                    clientVariationItemInfo = clientVariationInfo.Items.Find(delegate (ClientVariationItemInfo clientVariationItemInfoInList) { return clientVariationItemInfoInList.Equals(new ClientVariationItemInfo(Int32.Parse(parameterClientVariationItemId))); });

                                String parameterClientVariationTradeId = Request.Params["ClientVariationTradeId"];
                                if (parameterClientVariationTradeId != null)
                                    clientVariationTradeInfo = clientVariationInfo.Trades.Find(delegate (ClientVariationTradeInfo clientVariationTradeInfoInList) { return clientVariationTradeInfoInList.Equals(new ClientVariationTradeInfo(Int32.Parse(parameterClientVariationTradeId))); });

                                if (clientVariationInfo.IsInternallyApproved)
                                {
                                    phSubClientVariation.Visible = true;
                                }

                                phCancel.Visible = true;
                            }
                            else
                            {
                                phRestore.Visible = true;
                            }
                        }
                    }
                }

                CreateForm();

                if (!Page.IsPostBack)
                    BindClientVariation();
                else
                    BindApproval();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ddlSubClientVariations_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + ddlSubClientVariations.SelectedValue);
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteClientVariation(clientVariationInfo.TheParentClientVariation);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + clientVariationInfo.Project.IdStr + "&Type=" + clientVariationInfo.Type);
        }

        protected void cmdSubClientVariation_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + ProjectsController.GetInstance().MakeRevision(clientVariationInfo).IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().CancelClientVariation(clientVariationInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdRestore_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().RestoreClientVariation(clientVariationInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = clientVariationInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerCV.StepId; });
                processController.ExecuteProcessStep(currentStep, ProcessManagerCV.Comments);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdReverse_Click(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                ProcessStepInfo currentStep = clientVariationInfo.Process.Steps.Find(delegate (ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == ProcessManagerCV.StepId; });
                processController.ReverseProcessStep(currentStep, ProcessManagerCV.Comments);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void lnkDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteClientVariationItem(new ClientVariationItemInfo(Int32.Parse(((HtmlAnchor)sender).HRef)));
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        //San---------------060223----------------

        protected void lnkMoveUpItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().ChangeClientVariationItemDisplayOrder(new ClientVariationItemInfo(Int32.Parse(((HtmlAnchor)sender).HRef)), true);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }


        protected void lnkMoveDownItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().ChangeClientVariationItemDisplayOrder(new ClientVariationItemInfo(Int32.Parse(((HtmlAnchor)sender).HRef)), false);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }


        //San---------------060223----------------
        protected void cmdUpdateItem_Click(object sender, EventArgs e)
        {
            try
            {
                clientVariationItemInfo.Description = UI.Utils.GetFormString(((TextBox)Utils.FindControlRecursive(tblItems, "txtDescription")).Text);
                clientVariationItemInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblItems, "txtAmountItem")).Text);

                if (clientVariationItemInfo.Description == null)
                    throw new Exception("The field Description must have a value.");

                ProjectsController.GetInstance().UpdateClientVariationItem(clientVariationItemInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                newClientVariationItemInfo.Description = UI.Utils.GetFormString(((TextBox)Utils.FindControlRecursive(tblItems, "txtNewDescription")).Text);
                newClientVariationItemInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblItems, "txtNewAmountItem")).Text);

                if (newClientVariationItemInfo.Description == null)
                    throw new Exception("The field Description must have a value.");

                ProjectsController.GetInstance().AddClientVariationItem(newClientVariationItemInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void lnkDeleteTrade_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteClientVariationTrade(new ClientVariationTradeInfo(Int32.Parse(((HtmlAnchor)sender).HRef)));
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdUpdateTrade_Click(object sender, EventArgs e)
        {
            try
            {
                clientVariationTradeInfo.TradeCode = ((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlTradeCode")).SelectedValue;
                clientVariationTradeInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmountTrade")).Text);

                if (clientVariationTradeInfo.TradeCode == null)
                    throw new Exception("The field Trade Code must have a value.");

                ProjectsController.GetInstance().UpdateClientVariationTrade(clientVariationTradeInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdAddTrade_Click(object sender, EventArgs e)
        {
            try
            {
                newClientVariationTradeInfo.TradeCode = ((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlNewTradeCode")).SelectedValue;
                newClientVariationTradeInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtNewAmountTrade")).Text);

                if (newClientVariationTradeInfo.TradeCode == null)
                    throw new Exception("The field Trade Code must have a value.");

                ProjectsController.GetInstance().AddClientVariationTrade(newClientVariationTradeInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }
        #endregion

    }
}
