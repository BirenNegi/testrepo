using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Linq;

using SOS.Core;

namespace SOS.Web
{
    public partial class TradeBudgetControl : UserControl, ICallbackEventHandler
    {
        [Serializable]
        public class ControlResponse
        {
            public String ResponseType { get; set; }
            public String TradeCode { get; set; }
            public String Balance { get; set; }
            public String ContractBalance { get; set; }
            public String TradeCodeBalance { get; set; }
            public String ContractTradeCodeBalance { get; set; }
            public String Unallocated { get; set; }
            public StringDictionary BudgetSources = new StringDictionary();
        }

#region Members
        private List<IBudget> projectBudget;
        private TradeBudgetInfo tradeBudgetInfo = null;
        private TradeBudgetInfo newTradeBudgetInfo = null;
        private TradeInfo trade = null;
        private Boolean isEditable = false;
        private String selectedType = null;
        private String selectedId = null;
#endregion

#region Public properties
        public TradeInfo Trade
        {
            set { trade = value; }
        }

        public Boolean IsEditable
        {
            set { isEditable = value; }
        }

        public event EventHandler Updated;
#endregion
        
#region Public methods
        public void ReBind()
        {
            newTradeBudgetInfo = isEditable ? new TradeBudgetInfo(trade, null) : null;
            tradeBudgetInfo = null;
            CreateForm();
        }
#endregion

#region Private Methods
        protected HtmlTableCell DropDownTypeCell(String style, String id)
        {
            HtmlTableCell cell = new HtmlTableCell();
            DropDownList dropDownList = new DropDownList();

            dropDownList.ID = id;
            dropDownList.Items.Add(new ListItem("", ""));
            dropDownList.Items.Add(new ListItem(BudgetType.BOQ.ToString(), BudgetType.BOQ.ToString()));
            dropDownList.Items.Add(new ListItem(BudgetType.CV.ToString(), BudgetType.CV.ToString()));
            dropDownList.Items.Add(new ListItem(BudgetType.SA.ToString(), BudgetType.SA.ToString()));

            cell.Attributes.Add("class", style);
            cell.Controls.Add(dropDownList);

            return cell;
        }

        protected HtmlTableCell DropDownNameCell(String style, String id)
        {
            HtmlTableCell cell = new HtmlTableCell();
            DropDownList dropDownList = new DropDownList();

            dropDownList.ID = id;

            cell.Attributes.Add("class", style);
            cell.Controls.Add(dropDownList);

            return cell;
        }

        protected HtmlTableCell TextBoxCell(String style, String id, String width, Decimal? currentValue, Decimal? maxAmount = null)
        {
            HtmlTableCell cell = new HtmlTableCell();
            TextBox textBox = new TextBox();
            RangeValidator rangeValidator;
            CompareValidator compareValidator;

            cell.Attributes.Add("class", style);

            textBox.ID = id;
            textBox.Width = new Unit(width);
            textBox.Text = UI.Utils.SetFormEditDecimal(currentValue);

            compareValidator = new CompareValidator();
            compareValidator.ControlToValidate = textBox.ClientID;
            compareValidator.CssClass = "frmError";
            compareValidator.Display = ValidatorDisplay.Dynamic;
            compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
            compareValidator.Type = ValidationDataType.Currency;

            cell.Controls.Add(compareValidator);

            if (maxAmount != null)
            {
                rangeValidator = new RangeValidator();
                rangeValidator.ControlToValidate = textBox.ClientID;
                rangeValidator.ErrorMessage = "Must be <= " + maxAmount + "<br />";
                rangeValidator.CssClass = "frmError";
                rangeValidator.Display = ValidatorDisplay.Dynamic;
                rangeValidator.Type = ValidationDataType.Currency;
                rangeValidator.MinimumValue = "0";
                rangeValidator.MaximumValue = maxAmount != null ? String.Format("{0:#,###0.00}", maxAmount) : "0";

                cell.Controls.Add(rangeValidator);
            }

            cell.Controls.Add(textBox);

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

        protected HtmlTableCell TextCell(String style, String text)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.InnerText = UI.Utils.SetFormString(text);
            return cell;
        }

        protected HtmlTableCell EmptyCell()
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstBlankItem");
            return cell;
        }

        protected HtmlTableRow AddBudgetRow(String style, Decimal? maxAmount = null)
        {
            HtmlTableRow row;
            HtmlTableCell cell;
            Button button;
            DropDownList dropDownListBudgetType;
            DropDownList dropDownListBudgetSource;
            Label lblUnallocated;
            Label lblTradeCode;
            Label lblContractBalance;

            /* Uncoment to show trade code balance
            Label lblTradeCodeBalance;
            Label lblContractTradeCodeBalance;
             */

            /* Uncoment to show budget source balance
            Label lblBalance;
             */

            TextBox textBoxComparisonAmount;
            TextBox textBoxContractAmount;
            CustomValidator customValidator;
            CompareValidator compareValidator;
            HiddenField hiddenField;

            String currStyle = style;

            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";
            dropDownListBudgetType = new DropDownList();
            dropDownListBudgetType.ID = "ddlAddBudgetType";
            dropDownListBudgetType.AutoPostBack = false;

            dropDownListBudgetType.Items.AddRange(
                    new ListItem[4] { new ListItem("", ""), 
                        new ListItem("BOQ", BudgetType.BOQ.ToString()), 
                        new ListItem("CV", BudgetType.CV.ToString()), 
                        new ListItem("SA", BudgetType.SA.ToString()) });

            cell.Controls.Add(dropDownListBudgetType);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";

            dropDownListBudgetSource = new DropDownList();
            dropDownListBudgetSource.ID = "ddlAddBudgetSource";
            dropDownListBudgetSource.Items.Add(new ListItem("", ""));
            cell.Controls.Add(dropDownListBudgetSource);

            hiddenField = new HiddenField();
            hiddenField.ID = "hidAddBudgetSource";
            cell.Controls.Add(hiddenField);

            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";
            lblTradeCode = new Label();
            lblTradeCode.ID = "lblTradeCode";
            cell.Controls.Add(lblTradeCode);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";
            lblContractBalance = new Label();
            lblContractBalance.ID = "lblContractBalance";
            cell.Controls.Add(lblContractBalance);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";

            lblUnallocated = new Label();
            lblUnallocated.ID = "lblUnallocated";
            cell.Controls.Add(lblUnallocated);
            row.Cells.Add(cell);

            /* Uncoment to show budget source balance
            lblBalance = new Label();
            lblBalance.ID = "lblBalance";
            cell.Controls.Add(lblBalance);
            row.Cells.Add(cell);
            */

            /* Uncoment to show trade code balance
            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";
            lblContractTradeCodeBalance = new Label();
            lblContractTradeCodeBalance.ID = "lblContractTradeCodeBalance";
            cell.Controls.Add(lblContractTradeCodeBalance);
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";
            lblTradeCodeBalance = new Label();
            lblTradeCodeBalance.ID = "lblTradeCodeBalance";
            cell.Controls.Add(lblTradeCodeBalance);
            row.Cells.Add(cell);
            */

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);

            textBoxComparisonAmount = new TextBox();
            textBoxComparisonAmount.Width = new Unit(65);
            textBoxComparisonAmount.ID = "textBoxComparisonAmount";

            compareValidator = new CompareValidator();
            compareValidator.ControlToValidate = textBoxComparisonAmount.ClientID;
            compareValidator.ErrorMessage = "Invalid number!<br />";
            compareValidator.CssClass = "frmError";
            compareValidator.Display = ValidatorDisplay.Dynamic;
            compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
            compareValidator.Type = ValidationDataType.Currency;

            customValidator = new CustomValidator();
            customValidator.ID = "valAdd";
            customValidator.ControlToValidate = textBoxComparisonAmount.ClientID;
            customValidator.ErrorMessage = "Invalid value!<br />";
            customValidator.CssClass = "frmError";
            customValidator.Display = ValidatorDisplay.Dynamic;

            customValidator.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(valAdd_ServerValidate);

            cell.Controls.Add(customValidator);
            cell.Controls.Add(compareValidator);
            cell.Controls.Add(textBoxComparisonAmount);

            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);

            textBoxContractAmount = new TextBox();
            textBoxContractAmount.Width = new Unit(65);
            textBoxContractAmount.ID = "textBoxContractAmount";

            compareValidator = new CompareValidator();
            compareValidator.ControlToValidate = textBoxContractAmount.ClientID;
            compareValidator.ErrorMessage = "Invalid number!<br />";
            compareValidator.CssClass = "frmError";
            compareValidator.Display = ValidatorDisplay.Dynamic;
            compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
            compareValidator.Type = ValidationDataType.Currency;

            cell.Controls.Add(compareValidator);
            cell.Controls.Add(textBoxContractAmount);

            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", currStyle);
            cell.Align = "Center";
            cell.ColSpan = 2;
            button = new Button();
            button.Enabled = false;
            button.ID = "cmdAdd";
            button.Text = "Add";
            button.Click += new System.EventHandler(cmdAdd_Click);
            cell.Controls.Add(button);
            row.Cells.Add(cell);

            tblTrades.Rows.Add(row);

            String addButtonScript = @"
                    function CheckAddButton() {
                        if ($('#" + textBoxComparisonAmount.ClientID + @"').val() == '') {
                            $('#" + button.ClientID + @"').attr('disabled', true);
                        } else {
                            $('#" + button.ClientID + @"').attr('disabled', false);
                        }
                    }";

            // This method gets the call back reference to do the Asyn call
            String callServerScript = @"
                    function CallServer(arg, context) {"
                + Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveDataFromServer", "") + @"
                    }";

            /* Use these scripts to show the trade code balance
            String sendDataToServerScript = @"
                    function SendDataToServer(operation) {
                        var strSelection;
                        var strId;

                        $('#" + lblTradeCode.ClientID + @"').text('Loading...');
                        $('#" + lblUnallocated.ClientID + @"').empty();
                        $('#" + lblContractBalance.ClientID + @"').empty();
                        $('#" + lblTradeCodeBalance.ClientID + @"').empty();
                        $('#" + lblContractTradeCodeBalance.ClientID + @"').empty();
                        $('#" + textBoxComparisonAmount.ClientID + @"').val('');
                        $('#" + textBoxComparisonAmount.ClientID + @"').hide();
                        CheckAddButton();

                        if (operation == 'selectType') {
                            $('#" + dropDownListBudgetSource.ClientID + @"').empty();
                            $('#" + dropDownListBudgetSource.ClientID + @"').hide();

                            strSelection = $('#" + dropDownListBudgetType.ClientID + @"').val();
                        } else {
                            strId = $('#" + dropDownListBudgetSource.ClientID + @"').val();
                            strSelection = $('#" + dropDownListBudgetType.ClientID + @"').val() + ',' + strId;
                            $('#" + hiddenField.ClientID + @"').val(strId);
                        }
                       
                        CallServer(strSelection, '');
                    }";

            String receiveDataFromServercript = @"
                    function ReceiveDataFromServer(arg, context) {
                        var objResponse = jQuery.parseJSON(arg);

                        if (objResponse.ResponseType == 'selectType') {
                            $('#" + lblTradeCode.ClientID + @"').text('');

                            var dropDownListBudgetSource = document.getElementById('" + dropDownListBudgetSource.ClientID + @"');

                            dropDownListBudgetSource.options.add(new Option('', ''));

                            $.each(objResponse.BudgetSources, function(i, budgetSource) {
                                dropDownListBudgetSource.options.add(new Option(budgetSource.Value, budgetSource.Key));
                            });

                            $('#" + dropDownListBudgetSource.ClientID + @"').show();
                        } else {
                            $('#" + lblTradeCode.ClientID + @"').text(objResponse.TradeCode);
                            $('#" + lblUnallocated.ClientID + @"').text(objResponse.Unallocated);
                            $('#" + lblContractBalance.ClientID + @"').text(objResponse.ContractBalance);
                            $('#" + lblTradeCodeBalance.ClientID + @"').text(objResponse.TradeCodeBalance);
                            $('#" + lblContractTradeCodeBalance.ClientID + @"').text(objResponse.ContractTradeCodeBalance);
                        }

                        $('#" + textBoxComparisonAmount.ClientID + @"').show();
                        CheckAddButton();
                    }";             
            */

            // This java script method creates a string conatining the selected information and sends it asynchronously to the web server
            String sendDataToServerScript = @"
                    function SendDataToServer(operation) {
                        var strSelection;
                        var strId;

                        $('#" + lblTradeCode.ClientID + @"').text('Loading...');
                        $('#" + lblUnallocated.ClientID + @"').empty();
                        $('#" + lblContractBalance.ClientID + @"').empty();
                        $('#" + textBoxComparisonAmount.ClientID + @"').val('');
                        $('#" + textBoxComparisonAmount.ClientID + @"').hide();
                        CheckAddButton();

                        if (operation == 'selectType') {
                            $('#" + dropDownListBudgetSource.ClientID + @"').empty();
                            $('#" + dropDownListBudgetSource.ClientID + @"').hide();

                            strSelection = $('#" + dropDownListBudgetType.ClientID + @"').val();
                        } else {
                            strId = $('#" + dropDownListBudgetSource.ClientID + @"').val();
                            strSelection = $('#" + dropDownListBudgetType.ClientID + @"').val() + ',' + strId;
                            $('#" + hiddenField.ClientID + @"').val(strId);
                        }
                       
                        CallServer(strSelection, '');
                    }";

            String receiveDataFromServercript = @"
                    function ReceiveDataFromServer(arg, context) {
                        var objResponse = jQuery.parseJSON(arg);

                        if (objResponse.ResponseType == 'selectType') {
                            $('#" + lblTradeCode.ClientID + @"').text('');

                            var dropDownListBudgetSource = document.getElementById('" + dropDownListBudgetSource.ClientID + @"');

                            dropDownListBudgetSource.options.add(new Option('', ''));

                            $.each(objResponse.BudgetSources, function(i, budgetSource) {
                                dropDownListBudgetSource.options.add(new Option(budgetSource.Value, budgetSource.Key));
                            });

                            $('#" + dropDownListBudgetSource.ClientID + @"').show();
                        } else {
                            $('#" + lblTradeCode.ClientID + @"').text(objResponse.TradeCode);
                            $('#" + lblUnallocated.ClientID + @"').text(objResponse.Unallocated);
                            $('#" + lblContractBalance.ClientID + @"').text(objResponse.ContractBalance);
                        }

                        $('#" + textBoxComparisonAmount.ClientID + @"').show();
                        CheckAddButton();
                    }";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callServerScript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SendDataToServer", sendDataToServerScript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ReceiveDataFromServer", receiveDataFromServercript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddButton", addButtonScript, true);

            dropDownListBudgetType.Attributes.Add("onchange", "SendDataToServer('selectType')");
            dropDownListBudgetSource.Attributes.Add("onchange", "SendDataToServer('selectSource')");
            textBoxComparisonAmount.Attributes.Add("onkeyup ", "CheckAddButton()");

            tblTrades.Rows.Add(row);

            return row;
        }

        protected HtmlTableRow EditBudgetRow(TradeBudgetInfo tradeBudget, IBudget budgetProvider, String style)
        {
            HtmlTableRow row;
            HtmlTableCell cell;
            Button button;
            Image image;
            String idKey = String.Empty;

            row = new HtmlTableRow();
            row.Attributes.Add("class", style);

            hidSelectedId.Value = String.Format("{0}_{1}", budgetProvider.BudgetType, budgetProvider.Id.Value.ToString());

            row.Cells.Add(TextCell(style, budgetProvider.BudgetType.ToString()));
            row.Cells.Add(TextCell(style, budgetProvider.BudgetName));
            row.Cells.Add(TextCell(style, budgetProvider.TradeCode));
            row.Cells.Add(DecimalCell(style, tradeBudget.BudgetAmountInitial));
            row.Cells.Add(DecimalCell(style, budgetProvider.BudgetUnallocated));

            /* Uncoment to show trade code balance
            row.Cells.Add(DecimalCell(style, tradeBudget.BudgetAmountTradeInitial));
            row.Cells.Add(DecimalCell(style, budgetProvider.BudgetAmountTradeInitial));
            */

            row.Cells.Add(TextBoxCell(style, "txtBudgetAmountAllowance", "65", tradeBudget.BudgetAmountAllowance, budgetProvider.BudgetAmount));
            row.Cells.Add(TextBoxCell(style, "txtBudgetAmount", "65", tradeBudget.Amount));

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.NoWrap = true;
            cell.Align = "Center";
            cell.ColSpan = 2;

            button = new Button();
            button.ID = "cmdUpdate";
            button.Text = "Update";
            button.Click += new System.EventHandler(cmdUpdate_Click);

            cell.Controls.Add(button);

            image = new Image();
            image.ImageUrl = "~/Images/1x1.gif";
            image.Width = 8;
            cell.Controls.Add(image);

            button = new Button();
            button.ID = "cmdCancel";
            button.Text = "Cancel";
            button.Click += new System.EventHandler(cmdCancel_Click);
            cell.Controls.Add(button);

            row.Cells.Add(cell);

            return row;
        }

        protected HtmlTableRow ViewBudgetRow(TradeBudgetInfo tradeBudget, IBudget budgetProvider, String style)
        {
            HtmlTableRow row;
            HtmlTableCell cell;
            HtmlAnchor htmlAnchor;
            Image image;
            String idKey;

            /* Uncoment to show budget source balance
            Decimal? nonNegativeBudgetProviderBudgetAmountInitial = budgetProvider.BudgetAmountInitial != null && budgetProvider.BudgetAmountInitial.Value < 0 ? 0 : budgetProvider.BudgetAmountInitial; ;
            */

            Decimal? nonNegativeTradeBudgetBudgetAmountInitial = tradeBudget.BudgetAmountInitial != null && tradeBudget.BudgetAmountInitial.Value < 0 ? 0 : tradeBudget.BudgetAmountInitial;
            Decimal? nonNegativeBudgetProviderBudgetUnallocated = budgetProvider.BudgetUnallocated != null && budgetProvider.BudgetUnallocated.Value < 0 ? 0 : budgetProvider.BudgetUnallocated; ;

            row = new HtmlTableRow();
            row.Attributes.Add("class", style);

            row.Cells.Add(TextCell(style, budgetProvider.BudgetType.ToString()));
            row.Cells.Add(TextCell(style, budgetProvider.BudgetName));
            row.Cells.Add(TextCell(style, budgetProvider.TradeCode));

            /* Uncoment to show budget source balance
            row.Cells.Add(DecimalCell(style, nonNegativeBudgetProviderBudgetAmountInitial));
            */

            row.Cells.Add(DecimalCell(style, nonNegativeTradeBudgetBudgetAmountInitial));
            row.Cells.Add(DecimalCell(style, nonNegativeBudgetProviderBudgetUnallocated));

            /* Uncoment to show trade code balance
            row.Cells.Add(DecimalCell(style, tradeBudget.BudgetAmountTradeInitial));
            row.Cells.Add(DecimalCell(style, budgetProvider.BudgetAmountTradeInitial));
            */

            row.Cells.Add(DecimalCell(style, tradeBudget.BudgetAmountAllowance));
            row.Cells.Add(DecimalCell(style, tradeBudget.Amount));
            row.Cells.Add(DecimalCell(style, tradeBudget.BudgetWinLoss));

            if (newTradeBudgetInfo != null)
            {
                idKey = String.Format("{0}_{1}", budgetProvider.BudgetType, budgetProvider.Id.Value.ToString());

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", style);

                cell = new HtmlTableCell();
                htmlAnchor = new HtmlAnchor();
                htmlAnchor.ID = "lnkEdit_" + idKey;
                htmlAnchor.Attributes.Add("class", "lstLink");
                htmlAnchor.Attributes.Add("title", "Edit");
                htmlAnchor.Attributes.Add("href", idKey);
                htmlAnchor.ServerClick += new System.EventHandler(lnkEdit_Click);
                image = new Image();
                image.ImageUrl = "~/Images/IconEdit.gif";
                htmlAnchor.Controls.Add(image);
                cell.Controls.Add(htmlAnchor);
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", style);

                htmlAnchor = new HtmlAnchor();
                htmlAnchor.ID = "lnkDelete_" + idKey;
                htmlAnchor.Attributes.Add("class", "lstLink");
                htmlAnchor.Attributes.Add("title", "Delete");
                htmlAnchor.Attributes.Add("href", idKey);
                htmlAnchor.Attributes.Add("onclick", String.Format("javascript:return confirm('Delete budget: {0} ?');", tradeBudget.BudgetProvider.BudgetName));
                htmlAnchor.ServerClick += new System.EventHandler(lnkDelete_Click);
                image = new Image();
                image.ImageUrl = "~/Images/IconDelete.gif";
                htmlAnchor.Controls.Add(image);
                cell.Controls.Add(htmlAnchor);

                row.Cells.Add(cell);
            }

            return row;
        }

        protected HtmlTableRow ViewTradeCodeRow(String tradeCode, Decimal amount, String style)
        {
            HtmlTableRow row = new HtmlTableRow();

            row.Attributes.Add("class", style);
            row.Cells.Add(TextCell(style, tradeCode));

            if (amount != 0)
                row.Cells.Add(DecimalCell(style, amount));

            return row;
        }

        protected HtmlTableRow TotalBudgetRow()
        {
            HtmlTableRow row;
            HtmlTableCell cell;
            IBudget budgetProvider;

            /* Uncomment to show budget provider balance
            Decimal? nonNegativeBudgetProviderBudgetAmountInitial;
            Decimal totalNonNegativeBudgetProviderBudgetAmountInitial = 0;
            */

            Decimal? nonNegativeBudgetProviderBudgetUnallocated;
            Decimal totalNonNegativeBudgetProviderBudgetUnallocated = 0;
            Decimal? totalWinLoss = null;

            if (trade.TradeBudgets != null)
                foreach (TradeBudgetInfo tradeBudgetInfo in trade.TradeBudgets)
                {
                    budgetProvider = projectBudget.Find(pb => pb.Equals(tradeBudgetInfo.BudgetProvider));

                    /* Uncomment to show budget provider balance
                    nonNegativeBudgetProviderBudgetAmountInitial = budgetProvider.BudgetAmountInitial != null && budgetProvider.BudgetAmountInitial.Value < 0 ? 0 : budgetProvider.BudgetAmountInitial;
                    totalNonNegativeBudgetProviderBudgetAmountInitial += nonNegativeBudgetProviderBudgetAmountInitial == null ? 0 : nonNegativeBudgetProviderBudgetAmountInitial.Value;
                    */

                    nonNegativeBudgetProviderBudgetUnallocated = budgetProvider.BudgetUnallocated != null && budgetProvider.BudgetUnallocated.Value < 0 ? 0 : budgetProvider.BudgetUnallocated;
                    totalNonNegativeBudgetProviderBudgetUnallocated += nonNegativeBudgetProviderBudgetUnallocated == null ? 0 : nonNegativeBudgetProviderBudgetUnallocated.Value;

                    if (tradeBudgetInfo.BudgetWinLoss.HasValue)
                    {
                        if (totalWinLoss == null)
                            totalWinLoss = tradeBudgetInfo.BudgetWinLoss.Value;
                        else
                            totalWinLoss = totalWinLoss.Value + tradeBudgetInfo.BudgetWinLoss.Value;
                    }
                }

            row = new HtmlTableRow();

            cell = new HtmlTableCell();

            cell.ColSpan = 3;
            cell.Align = "Right";
            cell.InnerText = "Total:";
            cell.Attributes.Add("class", "lstHeader");

            row.Cells.Add(cell);
            row.Cells.Add(DecimalCell("lstHeader", trade.TotalNonNegativeBudgetAmountInitial));

            /* Uncomment to show budget provider balance total
            row.Cells.Add(DecimalCell("lstHeader", totalNonNegativeBudgetProviderBudgetAmountInitial));
            */

            row.Cells.Add(DecimalCell("lstHeader", totalNonNegativeBudgetProviderBudgetUnallocated));
            row.Cells.Add(DecimalCell("lstHeader", trade.TotalBudgetAllowance));
            row.Cells.Add(DecimalCell("lstHeader", trade.TotalBudgetAmount));
            row.Cells.Add(DecimalCell("lstHeader", totalWinLoss));

            cell = new HtmlTableCell();
            cell.ColSpan = 2;
            cell.Attributes.Add("class", "lstHeader");

            row.Cells.Add(cell);

            return row;
        }

        protected void CreateForm()
        {
            Dictionary<String, Decimal> tradeCodes = trade.TradeCodes;
            String currStyle = "lstAltItem";
            IBudget budgetProvider;

            hidSelectedId.Value = String.Empty;
            lblBudget.Text = String.Empty;
            pnlOldBudget.Visible = false;

            while (tblTrades.Rows.Count > 2)
                tblTrades.Rows.RemoveAt(tblTrades.Rows.Count - 1);

            // Uncoment to show the trade codes table
            // while (tblCodes.Rows.Count > 2)
            //     tblCodes.Rows.RemoveAt(tblCodes.Rows.Count - 1);

            if (trade.IsUsingBudgetModule)
            {
                foreach (TradeBudgetInfo tradeBudget in trade.TradeBudgets)
                {
                    budgetProvider = projectBudget.Find(pb => pb.Equals(tradeBudget.BudgetProvider));
                    currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                    if (tradeBudgetInfo != null && tradeBudget.BudgetProvider.Equals(tradeBudgetInfo.BudgetProvider))
                        tblTrades.Rows.Add(EditBudgetRow(tradeBudget, budgetProvider, currStyle));
                    else
                        tblTrades.Rows.Add(ViewBudgetRow(tradeBudget, budgetProvider, currStyle));
                }

                currStyle = "lstAltItem";

                // Uncoment to show the trade codes table
                // foreach (String tradeCode in tradeCodes.Keys)
                // {
                //     currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                //     tblCodes.Rows.Add(ViewTradeCodeRow(tradeCode, -tradeCodes[tradeCode], currStyle));
                // }
            }
            else
            {
                // The trade budget before was the budget participation Amount
                TradeParticipationInfo budgetParticipation = trade.BudgetParticipation;

                if (budgetParticipation != null && budgetParticipation.Amount != null)
                {
                    pnlOldBudget.Visible = true;
                    lblBudget.Text = UI.Utils.SetFormDecimal(budgetParticipation.Amount);
                }
            }

            if (newTradeBudgetInfo != null)
            {
                currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                tblTrades.Rows.Add(AddBudgetRow(currStyle));
            }

            tblTrades.Rows.Add(TotalBudgetRow());
        }

        private void BindForm()
        {
            if (tradeBudgetInfo != null)
            {
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtBudgetAmountAllowance")).Text = UI.Utils.SetFormEditDecimal(tradeBudgetInfo.BudgetAmountAllowance);
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtBudgetAmount")).Text = UI.Utils.SetFormEditDecimal(tradeBudgetInfo.Amount);
            }
        }

        private void ReloadForm()
        {
            trade.TradeBudgets = TradesController.GetInstance().GetTradeBudgets(trade);
            projectBudget = ProjectsController.GetInstance().GetProjectBudget(trade.Project, sbiTrade.IncludeAllCVSA, sbiTrade.IncludeAllOVO);

            tradeBudgetInfo = null;

            CreateForm();

            Control_Update();
        }
#endregion
        
#region Event Handlers
        protected void Control_Update()
        {
            if (Updated != null)
                Updated(this, new EventArgs());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                if (trade != null)
                {
                    projectBudget = projectsController.GetProjectBudget(trade.Project, sbiTrade.IncludeAllCVSA, sbiTrade.IncludeAllOVO);

                    if (isEditable)
                    {
                        newTradeBudgetInfo = new TradeBudgetInfo(trade, null);

                        if (hidSelectedId.Value != String.Empty)
                        {
                            String[] IdInfo = hidSelectedId.Value.Split('_');

                            IBudget budgetProvider = projectsController.CreateBudgetObject(IdInfo[0]);
                            budgetProvider.Id = Int32.Parse(IdInfo[1]);

                            tradeBudgetInfo = new TradeBudgetInfo(trade, budgetProvider);
                        }
                    }

                    CreateForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessControlLoadException(this, Ex);
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();

            String combinedKey = ((HtmlAnchor)sender).HRef;
            String[] IdInfo = combinedKey.Split('_');

            try
            {
                IBudget budgetProvider = projectsController.CreateBudgetObject(IdInfo[0]);
                budgetProvider.Id = Int32.Parse(IdInfo[1]);

                tradesController.DeleteTradeBudget(new TradeBudgetInfo(trade, budgetProvider));
                ReloadForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessControlLoadException(this, Ex);
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();

            String combinedKey = ((HtmlAnchor)sender).HRef;
            String[] IdInfo = combinedKey.Split('_');

            try
            {
                IBudget budgetProvider = projectsController.CreateBudgetObject(IdInfo[0]);
                budgetProvider.Id = Int32.Parse(IdInfo[1]);

                tradeBudgetInfo = tradesController.GetTradeBudget(trade.Id, budgetProvider);

                CreateForm();
                BindForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessControlLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ProjectsController projectsController = ProjectsController.GetInstance();
                TradesController tradesController = TradesController.GetInstance();

                tradeBudgetInfo.BudgetAmountAllowance = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtBudgetAmountAllowance")).Text);
                tradeBudgetInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtBudgetAmount")).Text);

                try
                {
                    tradesController.UpdateTradeBudget(tradeBudgetInfo);
                    ReloadForm();
                }
                catch (Exception Ex)
                {
                    Utils.ProcessControlLoadException(this, Ex);
                }
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            tradeBudgetInfo = null;
            CreateForm();
        }

        protected void valAdd_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            Decimal? amountEntered;

            String budgetType = UI.Utils.GetFormString(((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlAddBudgetType")).Text);
            IBudget budgetProvider = ProjectsController.GetInstance().CreateBudgetObject(budgetType);
            String budgetIdStr = UI.Utils.GetFormString(((HiddenField)Utils.FindControlRecursive(tblTrades, "hidAddBudgetSource")).Value);
            budgetProvider.Id = UI.Utils.GetFormInteger(budgetIdStr);

            IBudget iBudgetSelected = projectBudget.Find(pb => pb.BudgetType == budgetProvider.BudgetType && pb.Id == budgetProvider.Id);

            amountEntered = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "textBoxComparisonAmount")).Text);

            if (amountEntered != null && amountEntered <= iBudgetSelected.BudgetUnallocated)
                args.IsValid = true;
            else
            {
                ((CustomValidator)source).ErrorMessage = "Must be <= " + UI.Utils.SetFormDecimal(iBudgetSelected.BudgetUnallocated) + "<br />";
                args.IsValid = false;
            }
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ProjectsController projectsController = ProjectsController.GetInstance();
                TradesController tradesController = TradesController.GetInstance();

                String budgetType = UI.Utils.GetFormString(((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlAddBudgetType")).Text);
                String budgetIdStr = UI.Utils.GetFormString(((HiddenField)Utils.FindControlRecursive(tblTrades, "hidAddBudgetSource")).Value);

                IBudget budgetProvider = projectsController.CreateBudgetObject(budgetType);
                budgetProvider.Id = UI.Utils.GetFormInteger(budgetIdStr);

                newTradeBudgetInfo.BudgetProvider = budgetProvider;
                newTradeBudgetInfo.BudgetAmountAllowance = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "textBoxComparisonAmount")).Text);
                newTradeBudgetInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "textBoxContractAmount")).Text);

                try
                {
                    foreach (TradeBudgetInfo tradeBudget in trade.TradeBudgets)
                        if (tradeBudget.BudgetProvider.Equals(newTradeBudgetInfo.BudgetProvider))
                            throw new Exception(String.Format("The budget provider: {0} is already included.", tradeBudget.BudgetProvider.BudgetName));

                    tradesController.AddTradeBudget(newTradeBudgetInfo);
                    ReloadForm();
                }
                catch (Exception Ex)
                {
                    Utils.ProcessControlLoadException(this, Ex);
                }
            }
        }

        public void RaiseCallbackEvent(String eventArgument)
        {
            String[] parameters = eventArgument.Split(',');

            selectedType = parameters[0];
            selectedId = parameters.Length == 2 ? parameters[1] : null;
        }

        public String GetCallbackResult()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            ControlResponse controlResponse = new ControlResponse();

            // When a budget type was selected, populate all the available budget sources for that type
            if (selectedId == null)
            {
                controlResponse.ResponseType = "selectType";
                controlResponse.BudgetSources = new StringDictionary();

                foreach (IBudget iBudget in projectBudget)
                {
                    if (iBudget.BudgetType.ToString() == selectedType && !trade.TradeBudgets.Any(tb => tb.BudgetProvider.Equals(iBudget)))
                    {
                        if (iBudget is BudgetInfo)
                        {
                            controlResponse.BudgetSources.Add(iBudget.Id.Value.ToString(), iBudget.BudgetName);
                        }
                        else
                        {
                            controlResponse.BudgetSources.Add(iBudget.Id.Value.ToString(), iBudget.BudgetName + " - " + iBudget.TradeCode);
                        }
                    }
                }
            }
            else   // When a budget source is selected show current balances
            {
                IBudget iBudgetSelected;
                BudgetType budgetTypeSelected;

                controlResponse.ResponseType = "selectSource";

                if (selectedType == BudgetType.CV.ToString())
                    budgetTypeSelected = BudgetType.CV;
                else if (selectedType == BudgetType.SA.ToString())
                    budgetTypeSelected = BudgetType.SA;
                else
                    budgetTypeSelected = BudgetType.BOQ;

                iBudgetSelected = projectBudget.Find(pb => pb.BudgetType == budgetTypeSelected && pb.Id.Value.ToString() == selectedId);

                controlResponse.TradeCode = iBudgetSelected.TradeCode;
                controlResponse.Balance = UI.Utils.SetFormEditDecimal(iBudgetSelected.BudgetAmountInitial);
                controlResponse.Unallocated = UI.Utils.SetFormEditDecimal(iBudgetSelected.BudgetUnallocated);
                controlResponse.TradeCodeBalance = UI.Utils.SetFormEditDecimal(iBudgetSelected.BudgetAmountTradeInitial);
            }

            return jsonSerializer.Serialize(controlResponse);
        }
#endregion

    }
}
