using System;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;
using System.Text;//#--


namespace SOS.Web
{
    public partial class ComparisonEditControl : System.Web.UI.UserControl
    {

#region Members
        private TradeParticipationInfo tradeParticipation = null;
        private Boolean canEditValues = true;
#endregion

#region Public properties
        public TradeParticipationInfo TradeParticipation
        {
            get
            {
                FormToObjects();
                return tradeParticipation;
            }
            set
            {
                tradeParticipation = value;
            }
        }

        public Boolean CanEditValues
        {
            set
            {
                canEditValues = value;
            }
        }
#endregion

#region Private Methods
        private void CreateForm()
        {
            TradesController tradesController = TradesController.GetInstance();
            ParticipationItemInfo participationItemInfo;
            RadioButtonList radioButtonList, radioButtonSan;
            TextBox textBox;
            CheckBox checkBox;
            HtmlTableRow row;
            HtmlTableCell cell;
            HtmlTable table;
            CompareValidator compareValidator;
            Label label;
            


            String currStyle;
            Int32 numRows = 4;

            // Count total rows
            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipation.Trade.ItemCategories)
            {
                numRows++;

                if (tradeParticipation.IsActive)
                    numRows = numRows + tradeItemCategoryInfo.TradeItems.Count;
                else
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        if (tradeItemInfo.IsRequiredInProposal)
                            numRows++;
            }

            TradeParticipationInfo budgetParticipation = null; 
            // Header
            table = new HtmlTable();

            if (tradeParticipation.ComparisonParticipation == null)
            {
                row = new HtmlTableRow();

                //San --- To display Budget/BOQ
                 budgetParticipation = tradeParticipation.Trade.Participations.Count > 0 ? tradeParticipation.Trade.Participations[0] : null;

                //#----- To display Budget/BOQ
                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", "2");
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "center");
                cell.InnerText = "Subcontractor";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "center");
                row.Cells.Add(cell);
                //#----
                /*
                 cell = new HtmlTableCell();
                 cell.Attributes.Add("rowspan", numRows.ToString());
                 cell.Attributes.Add("class", "lstLine");
                 cell.Attributes.Add("align", "center");
                 cell.InnerHtml = "<img src='./../../Images/1x1.gif' width='1'>";
                 row.Cells.Add(cell);



                 cell = new HtmlTableCell();
                 cell.Attributes.Add("class", "lstHeaderTop");
                 cell.Attributes.Add("align", "center");
                 row.Cells.Add(cell);
                  */


                //#---


                //#----- To display Budget/BOQ
                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", "2");
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "center");
                cell.InnerText = budgetParticipation != null  ? "Budget/BOQ" : null;



                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "right");
                cell.InnerText = UI.Utils.SetFormDecimal(budgetParticipation.Amount);

                row.Cells.Add(cell);

                //#----- To display Budget/BOQ









                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", "4");
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "center");
                cell.InnerText = tradeParticipation.SubContractor == null ? "Budget/BOQ" : tradeParticipation.SubContractor.ShortNameOrName;
                row.Cells.Add(cell);

                table.Rows.Add(row);
            }

            // Labels Qty, Y/N, $, C
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.Attributes.Add("colspan", "2");
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Trade Items";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Unit";
            row.Cells.Add(cell);

             //#----- To display Budget/BOQ
            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Qty";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Y/N";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "$";
            row.Cells.Add(cell);
            //#----- To display Budget/BOQ






            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Qty";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Y/N";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "$";
            row.Cells.Add(cell);

            if (tradeParticipation.ComparisonParticipation == null)
            {
                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeader");
                cell.Attributes.Add("align", "center");
                cell.InnerHtml = "<img src='./../../Images/IconConfirmed.gif'>";
                row.Cells.Add(cell);
            }

            table.Rows.Add(row);






            // Trade item category
            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipation.Trade.ItemCategories)
            {
                row = new HtmlTableRow();

                cell = new HtmlTableCell();
                cell.InnerText = tradeItemCategoryInfo.ShortDescription;
                cell.Attributes.Add("colspan", "2");
                cell.Attributes.Add("class", "frmSubSubTitle");
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
               
                cell.Attributes.Add("colspan", "5"); //#--- cell.Attributes.Add("colspan", "2");
                cell.Attributes.Add("class", "frmText");
                row.Cells.Add(cell);


                //#---------
               
                if(tradeItemCategoryInfo.ShortDescription !=null && tradeItemCategoryInfo.TradeItems != null && tradeItemCategoryInfo.TradeItems.Count>0)
                { 
                    radioButtonSan = new RadioButtonList();
                    radioButtonSan.ID = "yesno_" + tradeItemCategoryInfo.ShortDescription;
                    radioButtonSan.RepeatDirection = RepeatDirection.Horizontal;
                    radioButtonSan.Items.Add("Y");
                    radioButtonSan.Items.Add("N");
                  
                    foreach (ListItem Li in radioButtonSan.Items)
                    { Li.Attributes.Add("onclick", "javascript:Check(this);");
                    }

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("colspan", "3");         //#---   cell.Attributes.Add("colspan", "4");
                    cell.Attributes.Add("class", "frmSubSubTitle");
                    cell.Controls.Add(radioButtonSan);
                    row.Cells.Add(cell);
                }
                
                //#----------


                table.Rows.Add(row);

                // Trade Items
                if (tradeItemCategoryInfo.TradeItems != null)
                {
                    currStyle = "";
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        if (tradeParticipation.IsActive || tradeItemInfo.IsRequiredInProposal)
                        {
                            row = new HtmlTableRow();

                            currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", "frmText");
                            cell.InnerHtml = "&nbsp;&nbsp;";
                            row.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);

                            if (tradeItemInfo.RequiresQuantityCheck == null)
                                cell.InnerText = tradeItemInfo.Name;
                            else
                                if ((Boolean)tradeItemInfo.RequiresQuantityCheck)
                                //#---- cell.InnerText = tradeItemInfo.Name + " * ";
                                //#--
                                    cell.InnerHtml = tradeItemInfo.Name + " <span style='color:red;font-size:12px;'><Strong> * </Strong></span>";
                                //#--
                            else
                                cell.InnerText = tradeItemInfo.Name;

                            row.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);
                            cell.Attributes.Add("align", "center");
                            cell.InnerText = tradeItemInfo.Units;
                            row.Cells.Add(cell);


                            //#----- To display Budget/BOQ

                            #region  to display BOQ

                            participationItemInfo = tradesController.GetParticipationItem(budgetParticipation, tradeItemInfo);


                            if (participationItemInfo == null)
                                throw new Exception("The participation item does not exist.");

                                    cell = new HtmlTableCell();
                                    cell.Attributes.Add("class", "BOQlstItem");
                                    cell.Attributes.Add("align", "center");
                                    cell.InnerText = participationItemInfo.Quantity;

                                    if (tradeItemInfo.RequiresQuantityCheck != null && (Boolean)tradeItemInfo.RequiresQuantityCheck && participationItemInfo.Quantity == null)
                                    {
                                        cell.Attributes.Add("class", "sanBorder");
                                    }

                                    row.Cells.Add(cell);



                                     cell = new HtmlTableCell();

                                          if (currStyle == "lstItem")
                                                if (participationItemInfo.Notes != null)
                                                {
                                                    cell.Attributes.Add("class", "lstItemCursorNote");
                                                    cell.Attributes.Add("title", participationItemInfo.Notes);
                                                }
                                                else
                                                    cell.Attributes.Add("class", "lstItemCursor");
                                            else
                                                if (participationItemInfo.Notes != null)
                                            {
                                                cell.Attributes.Add("class", "lstAltItemCursorNote");
                                                cell.Attributes.Add("title", participationItemInfo.Notes);
                                            }
                                            else
                                                cell.Attributes.Add("class", "lstAltItemCursor");

                                            //#---if (allowEdit)
                                            //#---    cell.Attributes.Add("onclick", "javascript:window.location.href='" + editParticipationURL + "?ParticipationItemId=" + participationItemInfo.IdStr + "'");
                                                cell.Attributes.Add("class", "BOQlstItem");
                                                cell.Attributes.Add("align", "center");
                                                cell.InnerText = UI.Utils.SetFormYesNo(participationItemInfo.IsIncluded);
                                            row.Cells.Add(cell);




                                        label = new Label();
                                        label.Text = UI.Utils.SetFormDecimal(participationItemInfo.Amount);

                                        if (participationItemInfo.IsIncluded != null)
                                            if (participationItemInfo.Confirmed != null)
                                                if (!(Boolean)participationItemInfo.Confirmed)
                                                    label.CssClass = "lstItemAssumed";
                                                else
                                                    label.CssClass = "lstItemConfirmed";

                                        cell = new HtmlTableCell();
                                        cell.Attributes.Add("class", "BOQlstItem");
                                        cell.Attributes.Add("align", "right");
                                        cell.Controls.Add(label);
                                        row.Cells.Add(cell);


                           

                            #endregion


                            //#----- To display Budget/BOQ




                            participationItemInfo = tradesController.GetParticipationItem(tradeParticipation, tradeItemInfo);

                            if (participationItemInfo == null)
                                throw new Exception("The participation item does not exist.");

                            textBox = new TextBox();
                            textBox.Width = new Unit("75px");
                            textBox.ID = "qty_" + participationItemInfo.IdStr;
                                                     

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);
                            

                            //#------
                                //if (tradeItemInfo.RequiresQuantityCheck != null && (Boolean)tradeItemInfo.RequiresQuantityCheck)
                                //{
                                
                                    //cell.Controls.Add(new System.Web.UI.LiteralControl("<br />"));
                                    //RqfVal = new RequiredFieldValidator();
                                    //RqfVal.ControlToValidate = textBox.ClientID;
                                    //RqfVal.ErrorMessage = "Please enter Qty";
                                    //RqfVal.Display = ValidatorDisplay.Dynamic;
                                    //RqfVal.ForeColor = System.Drawing.Color.Red;
                                    //cell.Controls.Add(RqfVal);

                                //}
                            //#-----

                            cell.Controls.Add(textBox);
                            row.Cells.Add(cell);

                            radioButtonList = new RadioButtonList();
                            //#--- radioButtonList.ID = "yesno_"+ participationItemInfo.IdStr;

                            //#---
                            radioButtonList.ID = "yesno_"+tradeItemCategoryInfo.ShortDescription+"_" + participationItemInfo.IdStr;

                            //#---

                            radioButtonList.RepeatDirection = RepeatDirection.Horizontal;
                            radioButtonList.ToolTip = tradeItemCategoryInfo.ShortDescription + " / " + tradeItemInfo.Name;
                            radioButtonList.Items.Add(new ListItem("Y"));
                            radioButtonList.Items.Add(new ListItem("N"));
                            radioButtonList.Items.Add(new ListItem("?"));
                           


                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);
                            cell.Controls.Add(radioButtonList);
                            row.Cells.Add(cell);

                            textBox = new TextBox();
                            textBox.Width = new Unit("75px");
                            textBox.ID = "amount_" + participationItemInfo.IdStr;
                            textBox.Enabled = canEditValues;

                            compareValidator = new CompareValidator();
                            compareValidator.ControlToValidate = textBox.ClientID;
                            compareValidator.ErrorMessage = "Invalid number!<br />";
                            compareValidator.CssClass = "frmError";
                            compareValidator.Display = ValidatorDisplay.Dynamic;
                            compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
                            compareValidator.Type = ValidationDataType.Currency;

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);
                            cell.Controls.Add(compareValidator);
                            cell.Controls.Add(textBox);
                            row.Cells.Add(cell);

                            if (tradeParticipation.ComparisonParticipation == null)
                            {
                                checkBox = new CheckBox();
                                checkBox.ToolTip = "Confirmed";
                                checkBox.ID = "confirmed_" + participationItemInfo.IdStr;

                                cell = new HtmlTableCell();
                                cell.Attributes.Add("class", currStyle);
                                cell.Controls.Add(checkBox);
                                row.Cells.Add(cell);
                            }

                            table.Rows.Add(row);
                        }
                }
            }

            phComparison.Controls.Add(table);
        }

        private void ObjectsToForm()
        {
            CheckBox chkConfirmed;
            TextBox qtyTextBox;
            TextBox amountTextBox;
            RadioButtonList yesnoRadioButtonList;
            ParticipationItemInfo participationItemInfo;
            TradesController tradesController = TradesController.GetInstance();

            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipation.Trade.ItemCategories)
                if (tradeItemCategoryInfo.TradeItems != null)
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        if (tradeParticipation.IsActive || tradeItemInfo.IsRequiredInProposal)
                        {
                            participationItemInfo = tradesController.GetParticipationItem(tradeParticipation, tradeItemInfo);

                            qtyTextBox = (TextBox)Utils.FindControlRecursive(phComparison, "qty_" + participationItemInfo.IdStr);


                            //#--- yesnoRadioButtonList = (RadioButtonList)Utils.FindControlRecursive(phComparison, "yesno_" + participationItemInfo.IdStr);
                            //#--
                            yesnoRadioButtonList = (RadioButtonList)Utils.FindControlRecursive(phComparison, "yesno_"+ tradeItemCategoryInfo.ShortDescription + "_" + participationItemInfo.IdStr);


                            //#--

                            amountTextBox = (TextBox)Utils.FindControlRecursive(phComparison, "amount_" + participationItemInfo.IdStr);

                            qtyTextBox.Text = UI.Utils.SetFormString(participationItemInfo.Quantity);
                            yesnoRadioButtonList.SelectedValue = UI.Utils.SetFormYesNo(participationItemInfo.IsIncluded);
                            amountTextBox.Text = UI.Utils.SetFormEditDecimal(participationItemInfo.Amount);

                            if (tradeParticipation.ComparisonParticipation == null)
                            {
                                chkConfirmed = (CheckBox)Utils.FindControlRecursive(phComparison, "confirmed_" + participationItemInfo.IdStr);
                                chkConfirmed.Checked = UI.Utils.SetFormBoolean(participationItemInfo.Confirmed);
                            }
                        }
        }



        private void FormToObjects()
        {
            CheckBox chkConfirmed;
            TextBox qtyTextBox;
            TextBox amountTextBox;
            RadioButtonList yesnoRadioButtonList;
            ParticipationItemInfo participationItemInfo;
            TradesController tradesController = TradesController.GetInstance();

            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in tradeParticipation.Trade.ItemCategories)
                if (tradeItemCategoryInfo.TradeItems != null)
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        if (tradeParticipation.IsActive || tradeItemInfo.IsRequiredInProposal)
                        {
                            participationItemInfo = tradesController.GetParticipationItem(tradeParticipation, tradeItemInfo);

                            qtyTextBox = (TextBox)Utils.FindControlRecursive(phComparison, "qty_" + participationItemInfo.IdStr);
                           //#--- yesnoRadioButtonList = (RadioButtonList)Utils.FindControlRecursive(phComparison, "yesno_" + participationItemInfo.IdStr);
                           //#---
                            yesnoRadioButtonList = (RadioButtonList)Utils.FindControlRecursive(phComparison, "yesno_"+tradeItemCategoryInfo.ShortDescription + "_" + participationItemInfo.IdStr);
                            //#---
                            amountTextBox = (TextBox)Utils.FindControlRecursive(phComparison, "amount_" + participationItemInfo.IdStr);

                            participationItemInfo.Quantity = UI.Utils.GetFormString(qtyTextBox.Text);
                            participationItemInfo.IsIncluded = UI.Utils.GetFormYesNo(yesnoRadioButtonList.SelectedValue);
                            participationItemInfo.Amount = UI.Utils.GetFormDecimal(amountTextBox.Text);

                            if (participationItemInfo.Amount == null)
                                participationItemInfo.Confirmed = null;
                            else
                                if (tradeParticipation.ComparisonParticipation == null)
                                {
                                    chkConfirmed = (CheckBox)Utils.FindControlRecursive(phComparison, "confirmed_" + participationItemInfo.IdStr);
                                    participationItemInfo.Confirmed = chkConfirmed.Checked;
                                }
                                else
                                    participationItemInfo.Confirmed = true;
                        }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (tradeParticipation != null)
            {
                CreateForm();

                if (!Page.IsPostBack)
                    ObjectsToForm();
            }
        }
#endregion

    }
}
