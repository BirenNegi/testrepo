using System;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;
using SOS.Web;

namespace SOS.Web
{
    public partial class ComparisonViewControl : System.Web.UI.UserControl
    {

#region Members
        private Boolean allowEdit = false;
        private String comparisonType = null;
        private TradeParticipationInfo tradeParticipation = null;
#endregion

#region Public properties
        public TradeParticipationInfo TradeParticipation
        {
            get { return tradeParticipation; }
            set { tradeParticipation = value; }
        }

        public Boolean AllowEdit
        {
            get { return allowEdit; }
            set { allowEdit = value; }
        }

        public String ComparisonType
        {
            get { return comparisonType; }
            set { comparisonType = value; }
        }

        public Boolean RePaint
        {
            set
            {
                if (value && tradeParticipation != null)
                    BindTradeParticipation();
            }
        }
#endregion

#region Private Methods
        private void BindTradeParticipation()
        {
            TradesController tradesController = TradesController.GetInstance();
            ParticipationItemInfo participationItemInfo;
            HtmlTable table;
            HtmlTableRow row;
            HtmlTableCell cell;
            Label label;
            String currStyle;
            Int32 numRows = 4;
            String strSpan = Convert.ToString((TradeParticipation.Trade.Participations.Count) * 3 + 1); ;

            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in TradeParticipation.Trade.ItemCategories)
            {
                numRows++;

                if (comparisonType == Info.TypeActive)
                    numRows = numRows + tradeItemCategoryInfo.TradeItems.Count;
                else
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        if (tradeItemInfo.IsRequiredInProposal)
                            numRows++;
            }

            table = new HtmlTable();

            row = new HtmlTableRow();
            TradeParticipationInfo budgetParticipation = null;   //#--
            // Header
            if (TradeParticipation.Id == null)
            {
                 budgetParticipation = TradeParticipation.Trade.Participations.Count > 0 ? TradeParticipation.Trade.Participations[0] : null;

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

                // Subcontractor names and totals
                foreach (TradeParticipationInfo tradeParticipationInfo in TradeParticipation.Trade.Participations)
                {



                    cell = new HtmlTableCell();
                    cell.Attributes.Add("rowspan", numRows.ToString());
                    cell.Attributes.Add("class", "lstLine");
                    cell.Attributes.Add("align", "center");
                    cell.InnerHtml = "<img src='./../../Images/1x1.gif' width='1'>";
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("colspan", "2");
                    //#--cell.Attributes.Add("class", "lstHeaderTop");   //#---
                    cell.Attributes.Add("align", "center");
                    cell.InnerText = budgetParticipation != null && budgetParticipation.Equals(tradeParticipationInfo) ? "Budget/BOQ" : tradeParticipationInfo.SubContractor.ShortNameOrName;
                    //#---
                    if (tradeParticipationInfo.IsPulledOut != false) { cell.Attributes.Add("class", "lstHeaderTopPulledout"); } else { cell.Attributes.Add("class", "lstHeaderTop"); } 
                     //#---
                    
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", "lstHeaderTop");
                    cell.Attributes.Add("align", "right");
                    cell.InnerText = UI.Utils.SetFormDecimal(tradeParticipationInfo.Amount);
                    //#---
                    if (tradeParticipationInfo.IsPulledOut != false) { cell.Attributes.Add("class", "lstHeaderTopPulledout"); } else { cell.Attributes.Add("class", "lstHeaderTop"); }
                    //#---
                    row.Cells.Add(cell);
                }
            }
            else
            {
                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", "5");
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "right");
                cell.InnerText = "Base Quote";
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstHeaderTop");
                cell.Attributes.Add("align", "right");
                cell.InnerText = UI.Utils.SetFormDecimal(TradeParticipation.Amount);
                row.Cells.Add(cell);  
            }

            table.Rows.Add(row);

            // Labels Qty, Y/N, $
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

            foreach (TradeParticipationInfo tradeParticipationInfo in TradeParticipation.Trade.Participations)
            {
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
               

               
            }

            table.Rows.Add(row);

            // Trade item category
            foreach (TradeItemCategoryInfo tradeItemCategoryInfo in TradeParticipation.Trade.ItemCategories)
            {
                String editParticipationURL = Utils.AbsoluteURL("Modules/Projects/EditParticipationItem.aspx");
                row = new HtmlTableRow();

                cell = new HtmlTableCell();
                cell.InnerText = tradeItemCategoryInfo.ShortDescription;
                cell.Attributes.Add("colspan", "2");
                cell.Attributes.Add("class", "frmSubSubTitle");
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", strSpan);
                cell.Attributes.Add("class", "frmText");
                row.Cells.Add(cell);

                table.Rows.Add(row);

                // Trade Items
                if (tradeItemCategoryInfo.TradeItems != null)
                {
                    currStyle = "";
                    foreach (TradeItemInfo tradeItemInfo in tradeItemCategoryInfo.TradeItems)
                        if (comparisonType == Info.TypeActive || tradeItemInfo.IsRequiredInProposal)
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

                                   //SAN---- cell.InnerText = tradeItemInfo.Name + " *";

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

                            string BQuantity="", BYesNo="", BAmount="";
                            foreach (TradeParticipationInfo tradeParticipationInfo in TradeParticipation.Trade.Participations)
                            {
                                participationItemInfo = tradesController.GetParticipationItem(tradeParticipationInfo, tradeItemInfo);

                                if (participationItemInfo == null)
                                    throw new Exception("The participation item does not exist.");

                                //#---to heighlight un maching columns with Budget/BOQ
                                if(tradeParticipationInfo.Equals(budgetParticipation))
                                {
                                     BQuantity = participationItemInfo.Quantity;

                                    BYesNo = UI.Utils.SetFormYesNo(participationItemInfo.IsIncluded).ToString();
                                    BAmount = UI.Utils.SetFormDecimal(participationItemInfo.Amount);


                                }
                                //#---to heighlight un maching columns with Budget/BOQ


                                cell = new HtmlTableCell();
                                cell.Attributes.Add("class", currStyle);
                                cell.Attributes.Add("align", "center");
                                cell.InnerText = participationItemInfo.Quantity;

                                if (tradeItemInfo.RequiresQuantityCheck != null && (Boolean)tradeItemInfo.RequiresQuantityCheck && !tradeParticipationInfo.IsPulledOut && participationItemInfo.Quantity == null)
                                {
                                  cell.Attributes.Add("class", "sanBorder");
                                }

                                //#---to heighlight un maching columns with Budget/BOQ     ---! tradeParticipationInfo.IsPulledOut &&
                                if (!tradeParticipationInfo.Equals(budgetParticipation) && !tradeParticipationInfo.IsPulledOut && participationItemInfo.Quantity != null && participationItemInfo.Quantity != BQuantity)
                                {
                                    cell.Attributes.Add("class", "sBorder");
                                }
                                //#---to heighlight un maching columns with Budget/BOQ

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

                                 if (allowEdit)
                                     cell.Attributes.Add("onclick", "javascript:window.location.href='" + editParticipationURL + "?ParticipationItemId=" + participationItemInfo.IdStr + "'");

                                 cell.Attributes.Add("align", "center");
                                 cell.InnerText = UI.Utils.SetFormYesNo(participationItemInfo.IsIncluded);

                                 //#---to heighlight un maching columns with Budget/BOQ
                                 if (!tradeParticipationInfo.Equals(budgetParticipation) && !tradeParticipationInfo.IsPulledOut && UI.Utils.SetFormYesNo(participationItemInfo.IsIncluded) != BYesNo &&  participationItemInfo.Amount==null)
                                 {
                                     cell.Attributes.Add("class", "sanBorder");
                                 }
                                 //#---to heighlight un maching columns with Budget/BOQ


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
                                 cell.Attributes.Add("class", currStyle);
                                 cell.Attributes.Add("align", "right");
                                 cell.Controls.Add(label);
                                 row.Cells.Add(cell);
                                 

                            }   //----for each

                            table.Rows.Add(row);
                        }
                }
            }

            // Totals
            if (TradeParticipation.Id == null)
            {
                row = new HtmlTableRow();

                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", "3");
                cell.Attributes.Add("class", "lstHeaderBig");
                cell.Attributes.Add("align", "right");
                cell.InnerText = "Total Comparison";

                row.Cells.Add(cell);

                foreach (TradeParticipationInfo tradeParticipationInfo in TradeParticipation.Trade.Participations)
                {
                    cell = new HtmlTableCell();
                    cell.Attributes.Add("colspan", "3");
                    cell.Attributes.Add("class", "lstHeaderBig");
                    cell.Attributes.Add("align", "right");

                    if (tradeParticipationInfo.IsPulledOut)
                        cell.InnerText = "Pulled out";

                    else
                        cell.InnerText = UI.Utils.SetFormDecimal(tradesController.GetComparisonTotal(tradeParticipationInfo));

                    row.Cells.Add(cell);
                }

                table.Rows.Add(row);
            }

            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.Attributes.Add("colspan", "3");
            cell.Attributes.Add("class", "lstHeaderTopBig");
            cell.Attributes.Add("align", "right");
            cell.InnerText = "Total Quote";
            row.Cells.Add(cell);

            foreach (TradeParticipationInfo tradeParticipationInfo in TradeParticipation.Trade.Participations)
            {
                cell = new HtmlTableCell();
                cell.Attributes.Add("colspan", "3");
               // cell.Attributes.Add("class", "lstHeaderTopBig");
                cell.Attributes.Add("align", "right");

                if (tradeParticipationInfo.IsPulledOut)
                {
                    cell.InnerText = "Pulled out";
                    cell.Attributes.Add("class", "lstHeaderTopPulledout");
                }
                else
                {
                    cell.InnerText = UI.Utils.SetFormDecimal(tradesController.GetQuoteTotal(tradeParticipationInfo));
                    cell.Attributes.Add("class", "lstHeaderTopBig");
                }
                row.Cells.Add(cell);
            }

            table.Rows.Add(row);
            phComparison.Controls.Add(table);
        }
#endregion

    }
}
