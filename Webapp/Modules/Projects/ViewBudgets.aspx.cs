using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

//--To export  to PDF
//using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;
using System.IO;
using System.Data;
using SOS.Core;

namespace SOS.Web
{
    public partial class ViewBudgetsPage : SOSPage
    {

        #region Members
        private ProjectInfo projectInfo = null;
        private BudgetInfo budgetInfo = null;
        private BudgetInfo newBudgetInfo = null;
        private List<IBudget> projectBudget = null;
        private List<IBudget> projectBudgetFull = null;
        private List<TradeTemplateInfo> tradeTemplateInfoList = null;
        private StringDictionary tradeNamesDictionary = null;
        private Boolean viewSummary = false;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        protected HtmlTableCell DropDownCell(String id, Boolean isEdit = false)
        {
            HtmlTableCell cell = new HtmlTableCell();
            DropDownList dropDownList = new DropDownList();

            dropDownList.Items.Add(new ListItem("", ""));

            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                if (!projectInfo.Budgets.Any(pb => pb.TradeCode == tradeTemplateInfo.TradeCode) || (isEdit && tradeTemplateInfo.TradeCode == budgetInfo.TradeCode))
                    dropDownList.Items.Add(new ListItem(tradeTemplateInfo.TradeCodeAndName, tradeTemplateInfo.TradeCode));

            dropDownList.ID = id;

            cell.Controls.Add(dropDownList);

            return cell;
        }

        protected HtmlTableCell TextBoxCell(String id, String width)
        {
            HtmlTableCell cell = new HtmlTableCell();
            TextBox textBox = new TextBox();

            textBox.ID = id;
            textBox.Width = new Unit(width);

            CompareValidator compareValidator = new CompareValidator();
            compareValidator.ControlToValidate = textBox.ClientID;
            compareValidator.ErrorMessage = "Invalid number!<br />";
            compareValidator.CssClass = "frmError";
            compareValidator.Display = ValidatorDisplay.Dynamic;
            compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
            compareValidator.Type = ValidationDataType.Currency;

            cell.Controls.Add(compareValidator);

            cell.Controls.Add(textBox);

            return cell;
        }

        protected HtmlTableCell TextCell(String s)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.InnerText = UI.Utils.SetFormString(s);
            return cell;
        }

        protected HtmlTableCell DateCell(DateTime? date)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.NoWrap = true;
            cell.InnerText = UI.Utils.SetFormDate(date);
            return cell;
        }

        protected HtmlTableCell DateTimeCell(DateTime? date)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.NoWrap = true;
            cell.InnerText = UI.Utils.SetFormDateTime(date);
            return cell;
        }

        protected HtmlTableCell DecimalCell(Decimal d, Boolean addBorder = false)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Align = "Right";
            cell.NoWrap = true;

            if (addBorder)
                cell.Attributes.Add("style", "border:#999999 1px solid;");

            if (d < 0)
                cell.Attributes.Add("class", "frmError");

            //#---cell.InnerText = d != 0 || addBorder ? UI.Utils.SetFormEditDecimal((Decimal?)d) : "";

            cell.InnerText = d != 0 || addBorder ? UI.Utils.SetFormEditDecimal((Decimal?)d) : "0.00";
            //#---
            return cell;
        }

        protected HtmlTableCell EmptyCell()
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstBlankItem");
            return cell;
        }

        protected HtmlTableRow EditBudgetRow(String style, Decimal value, String idStr)
        {
            HtmlTableRow row;
            HtmlTableCell cell;

            Button button;
            Image image;

            hidSelectedId.Value = idStr;

            row = new HtmlTableRow();
            row.Attributes.Add("class", style);

            row.Cells.Add(DropDownCell("ddlCode", true));
            row.Cells.Add(TextBoxCell("txtAmount", "100px"));
            row.Cells.Add(DecimalCell(value));

            cell = new HtmlTableCell();
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

        protected HtmlTableRow ViewBudgetRow(BudgetInfo budget, String style)
        {
            HtmlTableRow row;
            HtmlTableCell cell;
            HtmlAnchor htmlAnchor;
            Image image;

            row = new HtmlTableRow();
            row.Attributes.Add("class", style);

            row.Cells.Add(TextCell(budget.CodeAndName));
            row.Cells.Add(DecimalCell(budget.BudgetAmount));

            if (viewSummary)
                row.Cells.Add(DecimalCell(projectBudgetFull.Find(pb => pb.Equals(budget)).BudgetAmountInitial.Value));




            if (newBudgetInfo != null)
            {
                cell = new HtmlTableCell();
                htmlAnchor = new HtmlAnchor();
                htmlAnchor.ID = "lnkEdit_" + budget.IdStr;
                htmlAnchor.Attributes.Add("class", "lstLink");
                htmlAnchor.Attributes.Add("title", "Edit");
                htmlAnchor.Attributes.Add("href", budget.IdStr);
                htmlAnchor.ServerClick += new System.EventHandler(lnkEdit_Click);
                image = new Image();
                image.ImageUrl = "~/Images/IconEdit.gif";
                htmlAnchor.Controls.Add(image);
                cell.Controls.Add(htmlAnchor);
                row.Cells.Add(cell);

                cell = new HtmlTableCell();

                if (!projectBudget.Any(pb => budget.Equals(pb.BudgetProvider)))
                {
                    htmlAnchor = new HtmlAnchor();
                    htmlAnchor.ID = "lnkDelete_" + budget.IdStr;
                    htmlAnchor.Attributes.Add("class", "lstLink");
                    htmlAnchor.Attributes.Add("title", "Delete");
                    htmlAnchor.Attributes.Add("href", budget.IdStr);
                    htmlAnchor.Attributes.Add("onClick", String.Format("javascript:return confirm('Delete from budget trade: {0}?');", budget.Code));
                    htmlAnchor.ServerClick += new System.EventHandler(lnkDelete_Click);
                    image = new Image();
                    image.ImageUrl = "~/Images/IconDelete.gif";
                    htmlAnchor.Controls.Add(image);
                    cell.Controls.Add(htmlAnchor);
                }

                row.Cells.Add(cell);
            }

            return row;
        }

        protected HtmlTableRow AddBudgetRow()
        {
            HtmlTableRow row;
            HtmlTableCell cell;
            Button button;

            row = new HtmlTableRow();

            row.Cells.Add(DropDownCell("ddlNewCode"));
            row.Cells.Add(TextBoxCell("txtNewAmount", "100px"));

            cell = new HtmlTableCell();
            cell.Align = "Center";
            cell.ColSpan = viewSummary ? 3 : 2;
            button = new Button();
            button.ID = "cmdAdd";
            button.Text = "Add";
            button.Click += new System.EventHandler(cmdAdd_Click);
            cell.Controls.Add(button);
            row.Cells.Add(cell);

            tblTrades.Rows.Add(row);

            return row;
        }

        protected HtmlTableRow TradeBudgetRow(IBudget iBudget, Decimal balance, String style)
        {
            HtmlTableRow row;

            row = new HtmlTableRow();
            row.Attributes.Add("class", style);

            row.Cells.Add(DateCell(iBudget.BudgetDate));
            row.Cells.Add(TextCell(iBudget.BudgetType.ToString()));
            row.Cells.Add(TextCell(iBudget.BudgetName));
            row.Cells.Add(DecimalCell(iBudget.BudgetAmount));

            if (viewSummary)
                row.Cells.Add(DecimalCell(balance));

            tblTrades.Rows.Add(row);

            return row;
        }

        //#----Release 11 --Roled Back--Budget In and Budget Out 

        /* 

                protected HtmlTableRow TradeBudgetRowIn(IBudget iBudget, Decimal balance, String style)
                {
                    HtmlTableRow row;

                    row = new HtmlTableRow();
                    row.Attributes.Add("class", style);

                    row.Cells.Add(DateCell(iBudget.BudgetDate));
                    row.Cells.Add(TextCell(iBudget.BudgetType.ToString()));
                    row.Cells.Add(TextCell(iBudget.BudgetName));
                    row.Cells.Add(DecimalCell(iBudget.BudgetAmount));
                    row.Cells.Add(DecimalCell(iBudget.BudgetAmountInitial.Value));//----Current
                    //if (viewSummary)
                    //    row.Cells.Add(DecimalCell(balance));

                    tblTrades.Rows.Add(row);

                    return row;
                }
                //#----



                //#---
                protected HtmlTableRow TradeBudgetRowOut(IBudget iBudget, Decimal balance, String style)
                {
                    HtmlTableRow row;

                    row = new HtmlTableRow();
                    row.Attributes.Add("class", style);



                    row.Cells.Add(DateCell(iBudget.BudgetDate));// ------- date

                    row.Cells.Add(TextCell(iBudget.BudgetType.ToString()));//---Type

                    if (iBudget.BudgetType == BudgetType.Variation)  //------Description
                    {
                        string X = ((SOS.Core.VariationInfo)iBudget).SubcontractorShortName.ToString();
                        row.Cells.Add(TextCell(iBudget.BudgetName + "-" + X));// Variation Name- Subcontractor short name
                    }
                    else { row.Cells.Add(TextCell(iBudget.BudgetName.ToString()));
                    }



                    row.Cells.Add(TextCell(iBudget.BudgetProvider.BudgetType.ToString()));//---BudgetType

                    row.Cells.Add(TextCell(iBudget.BudgetProvider.TradeCode));////---BudgetCode

                    if (iBudget.BudgetType == BudgetType.Variation)//---Original
                    {
                      decimal dec = ((SOS.Core.VariationInfo)iBudget).BudgetAmount;
                      row.Cells.Add(DecimalCell(dec));
                    }
                    else
                    { 
                        row.Cells.Add(DecimalCell(iBudget.BudgetProvider.BudgetAmount));
                    }

                    row.Cells.Add(DecimalCell(iBudget.BudgetAmount));//---Contract Amount


                    if (iBudget.BudgetType == BudgetType.Variation)
                    { 
                        if (iBudget.BudgetAmountInitial == null) row.Cells.Add(DecimalCell(0));
                        else row.Cells.Add(DecimalCell(iBudget.BudgetAmountInitial.Value)); //---Allocation for Variation
                    }
                    if (iBudget.BudgetType == BudgetType.Contract)
                    {
                        if (iBudget.BudgetAmountAllowance == null) row.Cells.Add(DecimalCell(0));
                        else row.Cells.Add(DecimalCell(iBudget.BudgetAmountAllowance.Value)); //---Allocation for Contract

                    }
                    if (iBudget.BudgetWinLoss != null)
                        row.Cells.Add(DecimalCell(iBudget.BudgetWinLoss.Value)); //---- Win/Loss
                    else row.Cells.Add(DecimalCell(0));

                    row.Cells.Add(DecimalCell(iBudget.BudgetProvider.BudgetUnallocated.Value));//---Unallocated


                   // row.Cells.Add(DecimalCell(iBudget.BudgetAmountInitial.Value));//---Current

                    //if (iBudget.BudgetType == BudgetType.Variation)//---Current
                    //{
                    //    decimal Current = ((SOS.Core.VariationInfo)iBudget).BudgetAmountInitial.Value;
                    //    row.Cells.Add(DecimalCell(Current));
                    //}
                    //else
                    //{ 
                    //  row.Cells.Add(DecimalCell(iBudget.BudgetProvider.BudgetAmountTradeInitial.Value));//----Current
                    //}
                    //if (viewSummary)
                    //    row.Cells.Add(DecimalCell(balance));

                    tblTrades.Rows.Add(row);

                    return row;
                }


                */
        //#---




        protected void CreateBOQForm()
        {
            HtmlTableRow row;
            String currStyle = null;
            BudgetInfo budget;

            hidSelectedId.Value = String.Empty;

            while (tblTrades.Rows.Count > 1)
                tblTrades.Rows.RemoveAt(tblTrades.Rows.Count - 1);

            if (projectInfo.Budgets != null)
            {
                for (int i = 0; i < projectInfo.Budgets.Count; i++)
                {
                    budget = projectInfo.Budgets[i];

                    currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                    if (budgetInfo != null && budgetInfo.Id != null && budget.Equals(budgetInfo))
                        row = EditBudgetRow(currStyle, projectBudget.Find(pb => pb.Equals(budget)).BudgetAmountInitial.Value, budgetInfo.IdStr);
                    else
                        row = ViewBudgetRow(budget, currStyle);

                    tblTrades.Rows.Add(row);
                }
            }

            if (newBudgetInfo != null)
                tblTrades.Rows.Add(AddBudgetRow());
        }

        private void CreateSummaryReport(Boolean includeAllCVSA, Boolean includeAllOVO)
        {
            Decimal totalBOQ = 0;
            Decimal totalCV = 0;
            Decimal totalSA = 0;
            Decimal totalContracts = 0;
            Decimal totalVariations = 0;
            Decimal totalGeneral = 0;
            Decimal budgetToAdd;

            Decimal[] totals;
            SortedDictionary<String, Decimal[]> budgetSummary = new SortedDictionary<String, Decimal[]>();

            HtmlTableRow row;
            HtmlTableCell cell;
            String currStyle = null;

            foreach (IBudget iBudget in projectBudgetFull.Where(b => b.TradeCode != TradeInfo.marginTradeCode))
            {
                if (budgetSummary.ContainsKey(iBudget.TradeCode))
                {
                    totals = budgetSummary[iBudget.TradeCode];
                }
                else
                {
                    totals = new Decimal[6];

                    for (int i = 0; i <= 5; i++)
                        totals[i] = 0;

                    budgetSummary.Add(iBudget.TradeCode, totals);
                }

                if (iBudget.BudgetAmount != 0)
                {
                    budgetToAdd = 0;

                    if (iBudget.BudgetType == BudgetType.BOQ)
                    {
                        budgetToAdd = iBudget.BudgetAmount;

                        totals[0] += budgetToAdd;
                        totalBOQ += budgetToAdd;
                    }
                    else
                    {
                        if (iBudget.BudgetType == BudgetType.CV || iBudget.BudgetType == BudgetType.SA)
                        {
                            if (iBudget.BudgetInclude || includeAllCVSA)
                            {
                                budgetToAdd = iBudget.BudgetAmount;

                                if (iBudget.BudgetType == BudgetType.CV)
                                {
                                    totals[1] += budgetToAdd;
                                    totalCV += budgetToAdd;
                                }
                                else
                                {
                                    totals[2] += budgetToAdd;
                                    totalSA += budgetToAdd;
                                }
                            }
                        }
                        else if (iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation)
                        {
                            if (iBudget.BudgetInclude || includeAllOVO)
                            {
                                budgetToAdd = iBudget.BudgetAmount;

                                if (iBudget.BudgetType == BudgetType.Contract)
                                {
                                    totals[3] += budgetToAdd;
                                    totalContracts += budgetToAdd;
                                }
                                else
                                {
                                    totals[4] += budgetToAdd;
                                    totalVariations += budgetToAdd;
                                }
                            }
                        }
                    }

                    if (budgetToAdd != 0)
                    {
                        totals[5] += iBudget.BudgetAmount;
                        totalGeneral += iBudget.BudgetAmount;
                    }
                }
            }

            foreach (String tradeCode in budgetSummary.Keys)
            {
                if (budgetSummary[tradeCode][3] == 0 && budgetSummary[tradeCode][4] == 0)
                {
                    totalGeneral -= budgetSummary[tradeCode][5];
                    budgetSummary[tradeCode][5] = 0;
                }

                currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                row = new HtmlTableRow();
                row.Attributes.Add("class", currStyle);

                row.Cells.Add(TextCell(tradeCode));
                row.Cells.Add(TextCell(tradeNamesDictionary[tradeCode]));
                row.Cells.Add(DecimalCell(budgetSummary[tradeCode][0]));
                row.Cells.Add(DecimalCell(budgetSummary[tradeCode][1]));
                row.Cells.Add(DecimalCell(budgetSummary[tradeCode][2]));
                row.Cells.Add(DecimalCell(budgetSummary[tradeCode][3]));
                row.Cells.Add(DecimalCell(budgetSummary[tradeCode][4]));
                row.Cells.Add(DecimalCell(budgetSummary[tradeCode][5]));

                tblSummary.Rows.Add(row);
            }

            row = new HtmlTableRow();
            row.Attributes.Add("class", "lstFooter");

            cell = new HtmlTableCell();
            cell.Align = "Right";
            cell.InnerText = "Total";
            cell.Attributes.Add("class", "lstBlankItem");
            cell.ColSpan = 2;

            row.Cells.Add(cell);

            row.Cells.Add(DecimalCell(totalBOQ, true));
            row.Cells.Add(DecimalCell(totalCV, true));
            row.Cells.Add(DecimalCell(totalSA, true));
            row.Cells.Add(DecimalCell(totalContracts, true));
            row.Cells.Add(DecimalCell(totalVariations, true));
            row.Cells.Add(DecimalCell(totalGeneral, true));

            tblSummary.Rows.Add(row);


        }

        private void CreateTradeReport_OLD(String tradeCode, Boolean includeAllCVSA, Boolean includeAllOVO)
        {
            if (tradeCode != String.Empty)
            {
                Decimal balance = 0;
                String currStyle = null;

                projectBudgetFull.Sort(new BudgetComparer());
                //#--


                projectBudgetFull = projectBudgetFull.Where(X => X.TradeCode.ToString() == tradeCode).ToList();

                // projectBudgetFull = projectBudgetFull.Select(x=>x.Id).Distinct().ToList();//04/02/2022--

                // projectBudgetFull = projectBudgetFull.Distinct().ToList();//04/02/2022

                //projectBudgetFull = projectBudgetFull.Select(X=>X.BudgetAmount).Distinct().ToList();

                //#---
                int? Bproviderid = 0;

                int i = 0;
                foreach (IBudget iBudget in projectBudgetFull)
                {
                    i += 1;
                    if (iBudget.TradeCode == tradeCode

                        //&& iBudget.Id!= Bproviderid //San--04/02/2022

                        && (iBudget.BudgetInclude ||
                        ((iBudget.BudgetType == BudgetType.CV || iBudget.BudgetType == BudgetType.SA) && includeAllCVSA) ||
                        ((iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation) && includeAllOVO)))


                    {

                        currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                        balance += iBudget.BudgetAmount;
                        tblDetails.Rows.Add(TradeBudgetRow(iBudget, balance, currStyle));

                        Bproviderid = iBudget.Id;//San--04/02/2022


                        //iBudget.TradeCode=

                        //#----Release 11 --Rooled Back--Budget In and Budget Out 
                        /* 
                            //for Budget In
                            if (iBudget.BudgetType == BudgetType.BOQ || iBudget.BudgetType == BudgetType.CV || iBudget.BudgetType == BudgetType.SA)
                            {
                                tblDetailsIn.Rows.Add(TradeBudgetRowIn(iBudget, balance, currStyle));
                            }
                            //for Budget Out
                            else if (iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation)
                            {
                                if (ddlBudgetsurceType.SelectedValue == "All")
                                {
                                    tblDetailsOut.Rows.Add(TradeBudgetRowOut(iBudget, balance, currStyle));
                                }
                                else if (ddlBudgetsurceType.SelectedValue == iBudget.BudgetProvider.BudgetType.ToString())
                                {
                                    tblDetailsOut.Rows.Add(TradeBudgetRowOut(iBudget, balance, currStyle));
                                }
                            }
                        */
                        //#----

                    }
                }
            }
        }


        private void CreateTradeReport(String tradeCode, Boolean includeAllCVSA, Boolean includeAllOVO)
        {
            if (tradeCode != String.Empty)
            {
                Decimal balance = 0;
                String currStyle = null;

                projectBudgetFull.Sort(new BudgetComparer());
                //#--


                projectBudgetFull = projectBudgetFull.Where(X => X.TradeCode.ToString() == tradeCode).ToList();

                List<string> woTradeList = new List<string>();




                // projectBudgetFull = projectBudgetFull.Select(x=>x.Id).Distinct().ToList();//04/02/2022--

                // projectBudgetFull = projectBudgetFull.Distinct().ToList();//04/02/2022

                //projectBudgetFull = projectBudgetFull.Select(X=>X.BudgetAmount).Distinct().ToList();

                //#---
                int? Bproviderid = 0;

                int i = 0;
                foreach (IBudget iBudget in projectBudgetFull)
                {

                    if (iBudget.TradeCode == tradeCode

                        //&& iBudget.Id!= Bproviderid //San--04/02/2022

                        && (iBudget.BudgetInclude ||
                        ((iBudget.BudgetType == BudgetType.CV || iBudget.BudgetType == BudgetType.SA) && includeAllCVSA) ||
                        ((iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation) && includeAllOVO)))


                    {

                        bool flag = false;
                        TradeBudgetInfo trBudget = null;
                        VariationInfo vrBudget = null;
                        if (i != 0)
                            if (projectBudgetFull[i].BudgetType == BudgetType.Contract)
                                trBudget = (TradeBudgetInfo)projectBudgetFull[i];

                        if (trBudget != null)
                        {

                            var workNo = woTradeList.Find(X => X == trBudget.WorkOrderNumber);

                            if (workNo == null)
                            {
                                woTradeList.Add(trBudget.WorkOrderNumber);
                                flag = true;
                            }


                        }
                        else if (projectBudgetFull[i].BudgetType == BudgetType.Variation)
                        {
                            vrBudget = (VariationInfo)projectBudgetFull[i];
                            var workNo = woTradeList.Find(X => X == vrBudget.IdStr);

                            if (workNo == null)
                            {
                                woTradeList.Add(vrBudget.IdStr);
                                flag = true;
                            }


                        }
                        else
                        {
                            flag = true;

                        }


                        currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                        balance += iBudget.BudgetAmount;
                        if (flag)
                        {
                            tblDetails.Rows.Add(TradeBudgetRow(iBudget, balance, currStyle));
                        }

                        Bproviderid = iBudget.Id;//San--04/02/2022


                        i += 1;


                        //iBudget.TradeCode=

                        //#----Release 11 --Rooled Back--Budget In and Budget Out 
                        /* 
                            //for Budget In
                            if (iBudget.BudgetType == BudgetType.BOQ || iBudget.BudgetType == BudgetType.CV || iBudget.BudgetType == BudgetType.SA)
                            {
                                tblDetailsIn.Rows.Add(TradeBudgetRowIn(iBudget, balance, currStyle));
                            }
                            //for Budget Out
                            else if (iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation)
                            {
                                if (ddlBudgetsurceType.SelectedValue == "All")
                                {
                                    tblDetailsOut.Rows.Add(TradeBudgetRowOut(iBudget, balance, currStyle));
                                }
                                else if (ddlBudgetsurceType.SelectedValue == iBudget.BudgetProvider.BudgetType.ToString())
                                {
                                    tblDetailsOut.Rows.Add(TradeBudgetRowOut(iBudget, balance, currStyle));
                                }
                            }
                        */
                        //#----

                    }
                }
            }
        }






        //private decimal getBalance(String tradeCode, Boolean includeAllCVSA, Boolean includeAllOVO)
        //{   Decimal balance = 0;

        //    if (tradeCode != String.Empty)
        //    {
        //        projectBudgetFull.Sort(new BudgetComparer());
        //        //#--
        //        projectBudgetFull = projectBudgetFull.Distinct().ToList();//#---

        //        int i = 0;
        //        foreach (IBudget iBudget in projectBudgetFull)
        //        {
        //            i += 1;
        //            if (iBudget.TradeCode == tradeCode && (iBudget.BudgetInclude ||
        //                ((iBudget.BudgetType == BudgetType.CV || iBudget.BudgetType == BudgetType.SA) && includeAllCVSA) ||
        //                ((iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation) && includeAllOVO)))
        //            {

        //                balance += iBudget.BudgetAmount;

        //            }
        //        }
        //    }
        //     return balance;

        //}




        private void BindDetail()
        {
            SortedDictionary<String, String> tradeCodes = new SortedDictionary<String, String>();
            String selectedTradeCode = ddlTrades.SelectedValue;

            tradeCodes.Add(String.Empty, "");

            foreach (IBudget iBudget in projectBudgetFull)
                if (!tradeCodes.ContainsKey(iBudget.TradeCode))
                    tradeCodes.Add(iBudget.TradeCode, iBudget.TradeCode + " - " + tradeNamesDictionary[iBudget.TradeCode]);

            ddlTrades.DataSource = tradeCodes;
            ddlTrades.DataValueField = "Key";
            ddlTrades.DataTextField = "Value";
            ddlTrades.DataBind();

            if (tradeCodes.ContainsKey(selectedTradeCode))
                ddlTrades.SelectedValue = selectedTradeCode;
        }

        private void BindBOQ()
        {
            if (budgetInfo != null)
            {
                ((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlCode")).SelectedValue = UI.Utils.SetFormString(budgetInfo.Code);
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmount")).Text = UI.Utils.SetFormEditDecimal(budgetInfo.Amount);
            }
        }

        private void ReloadBudget()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            projectInfo.Budgets = projectsController.GetBudgets(projectInfo);

            if (viewSummary)
            {
                projectBudget = projectsController.GetProjectBudget(projectInfo, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO);
                projectBudgetFull = projectsController.GetProjectBudget(projectInfo, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO, true);
            }
            else
            {
                projectBudget = projectsController.GetProjectBudget(projectInfo, true, true);
                projectBudgetFull = projectsController.GetProjectBudget(projectInfo, true, true, true);
            }

            budgetInfo = null;

            CreateBOQForm();
            BindDetail();
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            String parameterProjectId;
            Int32 parameterPanel;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                parameterProjectId = Utils.CheckParameter("ProjectId");
                parameterPanel = Utils.CheckParameterInt32("Panel");

                projectInfo = projectsController.GetProjectWithBudgets(Int32.Parse(parameterProjectId));
                tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();

                viewSummary = processController.AllowViewBudgetBalanceCurrentUser(projectInfo);

                if (viewSummary)
                {
                    projectBudget = projectsController.GetProjectBudget(projectInfo, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO);
                    projectBudgetFull = projectsController.GetProjectBudget(projectInfo, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO, true);
                }
                else
                {
                    cellBalance.Visible = false;
                    projectBudget = projectsController.GetProjectBudget(projectInfo, true, true);
                    projectBudgetFull = projectsController.GetProjectBudget(projectInfo, true, true, true);
                }

                tradeNamesDictionary = new StringDictionary();
                foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                    tradeNamesDictionary.Add(tradeTemplateInfo.TradeCode, tradeTemplateInfo.TradeName);

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                pnlViewSummary.Visible = viewSummary;
                tdBalance.Visible = viewSummary;
                sbiBOQ.Visible = viewSummary;

                if (processController.AllowAddBudgetCurrentUser(projectInfo))
                {
                    newBudgetInfo = new BudgetInfo();
                    newBudgetInfo.Project = projectInfo;

                    if (hidSelectedId.Value != String.Empty)
                    {
                        budgetInfo = new BudgetInfo();
                        budgetInfo.Id = Int32.Parse(hidSelectedId.Value);
                        budgetInfo.Code = projectBudget.Find(pb => pb.Id == budgetInfo.Id).TradeCode;
                    }
                }

                cpe.Collapsed = parameterPanel == 0;

                // when the budget form is posted back create the form so the events get called.
                CreateBOQForm();

                if (!IsPostBack)
                    BindDetail();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //After all the events have been executed load the summary and detail forms. 
            //There could be Budget added, removed or updated that affect these forms
            //The BOQ form cannot be added here because the binding when edit needs to be called after creating the form
            CreateSummaryReport(sbiSummary.IncludeAllCVSA, sbiSummary.IncludeAllOVO);
            CreateTradeReport(ddlTrades.SelectedValue, sbiDetails.IncludeAllCVSA, sbiDetails.IncludeAllOVO);
            //#---
            BindExportGrid();
            //#---
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteBudget(new BudgetInfo(Int32.Parse(((HtmlAnchor)sender).HRef)));
                ReloadBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            Int32 budgetId;

            try
            {
                budgetId = Int32.Parse(((HtmlAnchor)sender).HRef);

                budgetInfo = projectsController.GetBudget(budgetId);
                budgetInfo.Project = projectInfo;

                CreateBOQForm();
                BindBOQ();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            budgetInfo.Code = UI.Utils.GetFormString(((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlCode")).SelectedValue);
            budgetInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmount")).Text);

            try
            {
                if (budgetInfo.Code == null)
                    throw new Exception("A trade must be selected.");

                if (budgetInfo.Amount == null)
                    budgetInfo.Amount = 0;

                foreach (BudgetInfo budget in projectInfo.Budgets)
                    if (!budget.Equals(budgetInfo))
                        if (budget.Code.Equals(budgetInfo.Code))
                            throw new Exception(String.Format("The trade: {0} already exist in the project budget.", budget.CodeAndName));

                ProjectsController.GetInstance().UpdateBudget(budgetInfo);
                ReloadBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            budgetInfo = null;
            CreateBOQForm();
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {

            ProjectsController projectsController = ProjectsController.GetInstance();

            newBudgetInfo.Code = UI.Utils.GetFormString(((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlNewCode")).SelectedValue);
            newBudgetInfo.Amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtNewAmount")).Text);

            try
            {
                if (newBudgetInfo.Code == null)
                    throw new Exception("A trade must be selected");

                if (newBudgetInfo.Amount == null)
                    newBudgetInfo.Amount = 0;

                foreach (BudgetInfo budget in projectInfo.Budgets)
                    if (budget.Code.Equals(newBudgetInfo.Code))
                        throw new Exception(String.Format("The trade: {0} is already in the budget.", budget.CodeAndName));

                // We can use this code to store the trade code balance
                // That requires to update the database Budgets table and corresponding Stored procs.
                //newBudgetInfo.BudgetAmountTradeInitial = 0;

                //foreach (IBudget iBudget in projectBudget)
                //    if (iBudget.TradeCode.Equals(newBudgetInfo.Code) && iBudget.BudgetInclude)
                //        newBudgetInfo.BudgetAmountTradeInitial += iBudget.BudgetAmount;

                projectsController.AddBudget(newBudgetInfo);
                ReloadBudget();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpload_Click(object sender, EventArgs e)
        {
            byte[] fileBytes;
            int fileLength;
            int numBytes = 0;

            if (Page.IsValid)
            {
                try
                {
                    fileLength = fileTrades.PostedFile.ContentLength;
                    fileBytes = new byte[fileLength];
                    numBytes = fileTrades.PostedFile.InputStream.Read(fileBytes, 0, fileTrades.PostedFile.ContentLength);

                    if (numBytes == fileLength)
                        ProjectsController.GetInstance().ImportTradeBudgets(projectInfo, fileTrades.PostedFile.FileName, new System.IO.MemoryStream(fileBytes));

                    ReloadBudget();
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }
            }
        }

        #endregion



        //#--To export Budget Summary to Excel//
        private void BindExportGrid()
        {
            DataTable dtBudget = new DataTable();
            dtBudget.Columns.Add("Trade");
            dtBudget.Columns.Add("Name");
            dtBudget.Columns.Add("BOQ");
            dtBudget.Columns.Add("CV");
            dtBudget.Columns.Add("SA");
            dtBudget.Columns.Add("Contracts");
            dtBudget.Columns.Add("Varaitions");
            dtBudget.Columns.Add("Balance");
            dtBudget.AcceptChanges();

            DataRow Dr = null;
            foreach (HtmlTableRow hRow in tblSummary.Rows)
            {

                Dr = dtBudget.NewRow();
                for (int i = 0; i < hRow.Cells.Count; i++)   // hRow.Cells.Count
                {
                    Dr[i] = hRow.Cells[i].InnerText;
                }
                dtBudget.Rows.Add(Dr);
            }
            dtBudget.AcceptChanges();

            if (dtBudget.Rows.Count > 0)
                dtBudget.Rows[0].Delete();

            gvExportBudget.DataSource = dtBudget;
            gvExportBudget.DataBind();
        }

        //#--To export Budget Summary to Excel//


    }
}
