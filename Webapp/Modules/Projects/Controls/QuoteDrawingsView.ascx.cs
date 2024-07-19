using System;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class QuoteDrawingsViewControl : System.Web.UI.UserControl
    {

#region Members
        private TradeParticipationInfo tradeParticipation = null;
#endregion

#region Public properties
        public TradeParticipationInfo TradeParticipation
        {
            get { return tradeParticipation; }
            set { tradeParticipation = value; }
        } 

        public Boolean RePaint
        {
            set
            {
                if (value && tradeParticipation != null)
                    BindQuoteDrawings();
            }
        }
#endregion

#region Private Methods
        private void BindQuoteDrawings()
        {
            HtmlTable table;
            HtmlTableRow row;
            HtmlTableCell cell;

            table = new HtmlTable();

            table.CellPadding = 4;
            table.CellSpacing = 2;

            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.InnerText = "Drawing";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.InnerText = "Revision";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstHeader");
            cell.Attributes.Add("align", "center");
            cell.InnerText = "Date";
            row.Cells.Add(cell);

            table.Rows.Add(row);
            
            if (TradeParticipation.QuoteParticipation.Transmittal != null && TradeParticipation.QuoteParticipation.Transmittal.TransmittalRevisions != null)
            {
                String currStyle = String.Empty;
                String cssStyle = String.Empty;
                String strRevisionName;
                String strRevisionDate;
                TransmittalRevisionInfo transmittalRevisionInfoQuote;
                TransmittalRevisionInfo transmittalRevisionInfo;
                List<DrawingInfo> includedDrawings = TradeParticipation.Trade.IncludedDrawings;

                foreach (DrawingInfo drawingInfo in includedDrawings)
                {
                    row = new HtmlTableRow();

                    currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.InnerText = UI.Utils.SetFormString(drawingInfo.Name);
                    row.Cells.Add(cell);

                    transmittalRevisionInfoQuote = TradeParticipation.QuoteParticipation.Transmittal.TransmittalRevisions.Find(delegate(TransmittalRevisionInfo TransmittalRevisionInfoInList) { return drawingInfo.Equals(TransmittalRevisionInfoInList.Drawing); });

                    if (transmittalRevisionInfoQuote != null)
                    {
                        transmittalRevisionInfo = TradeParticipation.Transmittal.TransmittalRevisions.Find(delegate(TransmittalRevisionInfo TransmittalRevisionInfoInList) { return drawingInfo.Equals(TransmittalRevisionInfoInList.Drawing); });

                        if (transmittalRevisionInfo != null)
                            if (transmittalRevisionInfoQuote.Revision.Equals(transmittalRevisionInfo.Revision))
                                if (transmittalRevisionInfoQuote.Revision.Equals(drawingInfo.LastRevision))
                                    cssStyle = "lstTextNormal";
                                else
                                    cssStyle = "lstTextGrayedOut";
                            else
                                cssStyle = "lstTextHighlighted";
                        else
                            cssStyle = "lstTextHighlighted";

                        strRevisionName = "<span class='" + cssStyle + "'>" + UI.Utils.SetFormString(transmittalRevisionInfoQuote.RevisionName) + "</span>";
                        strRevisionDate = "<span class='" + cssStyle + "'>" + UI.Utils.SetFormDate(transmittalRevisionInfoQuote.RevisionDate) + "</span>";
                    }
                    else
                    {
                        strRevisionName = String.Empty;
                        strRevisionDate = String.Empty;
                    }

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.InnerHtml = strRevisionName;
                    row.Cells.Add(cell);

                    cell = new HtmlTableCell();
                    cell.Attributes.Add("class", currStyle);
                    cell.Align = "center";
                    cell.InnerHtml = strRevisionDate;
                    row.Cells.Add(cell);

                    table.Rows.Add(row);
                }
            }

            phQuoteDrawings.Controls.Add(table);
        }
#endregion

    }
}
