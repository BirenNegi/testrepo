using System;
using System.IO;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

using SOS.Core;

using SOS.FileTransferService.DataContracts;

using Client = SOS.FileTransferService.Client;

namespace SOS.Web
{
    public partial class ViewParticipationSubContractorPage : SOSPage
    {

#region Members
        private TradeParticipationInfo tradeParticipationInfo = null;
        private DataTable drawingsDataTable = null;
        protected String drawingsLinks = String.Empty;
        protected String drawingsSizes = String.Empty;
        protected String drawingsIds = String.Empty;
        protected String checkAllId = String.Empty;
        private Byte[] fileData = null;
#endregion

#region Public properties
        protected String GvTransmittalsSortExpression
        {
            get { return (String)ViewState["GvTransmittalsSortExpression"]; }
            set { ViewState["GvTransmittalsSortExpression"] = value; }
        }

        protected SortDirection GvTransmittalsSortDireccion
        {
            get { return (SortDirection)ViewState["GvTransmittalsSortDirection"]; }
            set { ViewState["GvTransmittalsSortDirection"] = value; }
        }

        protected String GvSubContractsSortExpression
        {
            get { return (String)ViewState["GvSubContractsSortExpression"]; }
            set { ViewState["GvSubContractsSortExpression"] = value; }
        }

        protected SortDirection GvSubContractsSortDireccion
        {
            get { return (SortDirection)ViewState["GvSubContractsSortDirection"]; }
            set { ViewState["GvSubContractsSortDirection"] = value; }
        }

        protected String GvDrawingsSortExpression
        {
            get { return (String)ViewState["GvDrawingsSortExpression"]; }
            set { ViewState["GvDrawingsSortExpression"] = value; }
        }

        protected SortDirection GvDrawingsSortDireccion
        {
            get { return (SortDirection)ViewState["GvDrawingsSortDirection"]; }
            set { ViewState["GvDrawingsSortDirection"] = value; }
        }

        protected DateTime? TimerControlDate
        {
            get { return (DateTime?)ViewState["TimerControlDate"]; }
            set { ViewState["TimerControlDate"] = value; }
        }
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeParticipationInfo == null)
                return null;

            tempNode.Title = tradeParticipationInfo.ProjectName + " (" + tradeParticipationInfo.TradeName + ")";

            return currentNode;
        }

        private void InitializeForm()
        {
            pnlMessage.Visible = false;
        }

        protected Boolean FormatFileLink(Object objIdStr, Object objFileSize)
        {
            return objIdStr != DBNull.Value && objFileSize != DBNull.Value && (Int64)objFileSize != 0;
        }

        protected String FormatFileSize(Object objIdStr, Object objFileSize)
        {
            if (objIdStr == DBNull.Value)
                return "";
            else
                if (objFileSize == DBNull.Value)
                    return "File not found";
                else
                    return SOS.UI.Utils.SetFormFileSize((long)objFileSize);
        }

        private void AddNode(XmlNode xmlNode, TreeNode treeNode)
        {
            if (xmlNode.HasChildNodes)
            {
                switch (xmlNode.Name)
                {
                    case "errors":
                        treeNode.Text = "Quote Check";
                        break;
                    case "error":
                        treeNode.Text = xmlNode.Attributes["text"].Value;
                        treeNode.SelectAction = TreeNodeSelectAction.None;
                        treeNode.Expanded = false;
                        break;
                }

                for (int i = 0; i <= xmlNode.ChildNodes.Count - 1; i++)
                {
                    treeNode.ChildNodes.Add(new TreeNode());
                    AddNode(xmlNode.ChildNodes[i], treeNode.ChildNodes[i]);
                }
            }
            else
            {
                switch (xmlNode.Name)
                {
                    case "error":
                        treeNode.Text = xmlNode.Attributes["text"].Value;
                        treeNode.SelectAction = TreeNodeSelectAction.None;
                        break;
                    case "item":
                        treeNode.Text = xmlNode.Attributes["name"].Value;
                        treeNode.SelectAction = TreeNodeSelectAction.None;
                        break;
                }
            }
        }
        
        private void BindDrawingRevision(DrawingRevisionInfo drawingRevisionInfo, DataTable dataTable, String cssClass)
        {
            long? fileSize;
            DataRow dataRow = null;
            FileMetaData fileMetaData = null;

            if (drawingRevisionInfo.File != null)
            {
                fileMetaData = Client.Utils.GetFileMetaData(UI.Utils.Path(tradeParticipationInfo.Trade.Project.AttachmentsFolder, drawingRevisionInfo.File));

                if (fileMetaData.Exist)
                    fileSize = fileMetaData.Length;
                else
                    fileSize = null;
            }
            else
                fileSize = null;

            dataRow = dataTable.NewRow();

            dataRow["IdStr"] = drawingRevisionInfo.Drawing.IdStr;
            dataRow["DrawingType"] = drawingRevisionInfo.Drawing.DrawingType.Name;
            dataRow["Name"] = drawingRevisionInfo.Drawing.Name;
            dataRow["LastRevisionIdStr"] = drawingRevisionInfo.IdStr;
            dataRow["LastRevisionNumber"] = drawingRevisionInfo.Number;
            dataRow["CssClass"] = cssClass;

            if (drawingRevisionInfo.Id != null) dataRow["Id"] = drawingRevisionInfo.Id;
            if (drawingRevisionInfo.RevisionDate != null) dataRow["LastRevisionDate"] = drawingRevisionInfo.RevisionDate;
            if (fileSize != null) dataRow["LastRevisionFileSize"] = fileSize;

            dataTable.Rows.Add(dataRow);
        }

        private void BindDrawings()
        {
            List<DrawingInfo> filteredDrawings;

            String sortExpresion = GvDrawingsSortExpression;
            SortDirection sortDirection = GvDrawingsSortDireccion;
            Dictionary<int?, DrawingRevisionInfo> drawingDictionary = new Dictionary<int?, DrawingRevisionInfo>();
            List<DrawingInfo> includedDrawings = tradeParticipationInfo.Trade.IncludedDrawings;

            drawingsDataTable = new DataTable();

            if (tradeParticipationInfo.IsClosed && !tradeParticipationInfo.IsAwarded)
            {
                List<DrawingRevisionInfo> filteredDrawingRevisions;
                filteredDrawings = new List<DrawingInfo>();

                foreach (DrawingInfo drawingInfo in includedDrawings)
                {
                    if (drawingInfo.DrawingRevisions != null)
                    {
                        filteredDrawingRevisions = new List<DrawingRevisionInfo>();

                        foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
                            if (drawingRevisionInfo.RevisionDate == null || drawingRevisionInfo.RevisionDate <= tradeParticipationInfo.QuoteDueDate)
                                filteredDrawingRevisions.Add(drawingRevisionInfo);

                        if (filteredDrawingRevisions.Count > 0)
                        {
                            drawingInfo.DrawingRevisions = filteredDrawingRevisions;
                            filteredDrawings.Add(drawingInfo);
                        }
                    }
                }
            }
            else
                filteredDrawings = includedDrawings;

            if (tradeParticipationInfo.Transmittal != null && tradeParticipationInfo.Transmittal.TransmittalRevisions != null)
                foreach (TransmittalRevisionInfo transmittalRevisionInfo in tradeParticipationInfo.Transmittal.TransmittalRevisions)
                    drawingDictionary.Add(transmittalRevisionInfo.Drawing.Id, transmittalRevisionInfo.Revision);

            drawingsDataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
            drawingsDataTable.Columns.Add("IdStr", System.Type.GetType("System.String"));
            drawingsDataTable.Columns.Add("DrawingType", System.Type.GetType("System.String"));
            drawingsDataTable.Columns.Add("Name", System.Type.GetType("System.String"));
            drawingsDataTable.Columns.Add("LastRevisionIdStr", System.Type.GetType("System.String"));
            drawingsDataTable.Columns.Add("LastRevisionNumber", System.Type.GetType("System.String"));
            drawingsDataTable.Columns.Add("LastRevisionDate", System.Type.GetType("System.DateTime"));
            drawingsDataTable.Columns.Add("LastRevisionFileSize", System.Type.GetType("System.Int64"));
            drawingsDataTable.Columns.Add("CssClass", System.Type.GetType("System.String"));
            drawingsDataTable.PrimaryKey = new DataColumn[] { drawingsDataTable.Columns[0] };

            foreach (DrawingInfo drawingInfo in filteredDrawings)
            {
                if (tradeParticipationInfo.Transmittal == null)
                {
                    if (drawingInfo.LastRevision != null)
                    {
                        BindDrawingRevision(drawingInfo.LastRevision, drawingsDataTable, "lstTextNormal");
                    }
                }
                else
                {
                    if (drawingDictionary.ContainsKey(drawingInfo.Id))
                    {
                        if (drawingDictionary[drawingInfo.Id].Equals(drawingInfo.LastRevision))
                        {
                            drawingInfo.LastRevision.Drawing = drawingInfo;
                            BindDrawingRevision(drawingInfo.LastRevision, drawingsDataTable, "lstTextNormal");
                        }
                        else
                        {
                            if (drawingInfo.DrawingRevisions != null)
                            {
                                foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
                                {
                                    if (drawingDictionary[drawingInfo.Id].Equals(drawingRevisionInfo))
                                    {
                                        drawingRevisionInfo.Drawing = drawingInfo;
                                        BindDrawingRevision(drawingRevisionInfo, drawingsDataTable, "lstTextGrayedOut");
                                    }
                                    else if (drawingRevisionInfo.RevisionDate != null && drawingDictionary[drawingInfo.Id].RevisionDate != null && drawingRevisionInfo.RevisionDate >= drawingDictionary[drawingInfo.Id].RevisionDate)
                                    {
                                        drawingRevisionInfo.Drawing = drawingInfo;
                                        BindDrawingRevision(drawingRevisionInfo, drawingsDataTable, "lstTextHighlighted");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (drawingInfo.LastRevision != null)
                        {
                            drawingInfo.LastRevision.Drawing = drawingInfo;
                            BindDrawingRevision(drawingInfo.LastRevision, drawingsDataTable, "lstTextHighlighted");
                        }
                    }
                }
            }

            ClearSelectedDrawings();
            GridPageSizeDrawings.Visible = drawingsDataTable.Rows.Count > 0;
            Utils.BindSortedGrid(gvDrawings, drawingsDataTable, sortExpresion, sortDirection);
            RegisterScriptDrawings();
        }

        private void BindTransmittals()
        {
            String sortExpresion = GvTransmittalsSortExpression;
            SortDirection sortDirection = GvTransmittalsSortDireccion;
            DataTable dataTable = new DataTable();
            DataRow dataRow = null;

            dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("IdStr", System.Type.GetType("System.String"));
            dataTable.Columns.Add("Number", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("Description", System.Type.GetType("System.String"));
            dataTable.Columns.Add("TransmissionDate", System.Type.GetType("System.DateTime"));
            dataTable.Columns.Add("NumDrawings", System.Type.GetType("System.Int32"));

            if (tradeParticipationInfo.Trade.Project.Transmittals !=null)
                foreach (TransmittalInfo transmittalInfo in tradeParticipationInfo.Trade.Project.Transmittals)
                    if (transmittalInfo.IsSent)
                    {
                        dataRow = dataTable.NewRow();

                        dataRow["IdStr"] = transmittalInfo.IdStr;
                        dataRow["Number"] = transmittalInfo.TransmittalNumber;
                        dataRow["Description"] = transmittalInfo.TransmittalType != null ? Web.Utils.GetConfigListItemNameAndOther("Transmittals", "TransmittalType", transmittalInfo.TransmittalType, transmittalInfo.TransmittalTypeOther) : "No Type";

                        if (transmittalInfo.Id != null) dataRow["Id"] = (Int32)transmittalInfo.Id;
                        if (transmittalInfo.TransmissionDate != null) dataRow["TransmissionDate"] = (DateTime)transmittalInfo.TransmissionDate;
                        if (transmittalInfo.NumDrawings != null) dataRow["NumDrawings"] = (Int32)transmittalInfo.NumDrawings;

                        dataTable.Rows.Add(dataRow);
                    }

            GridPageSizeTransmittals.Visible = dataTable.Rows.Count > 0;
            Utils.BindSortedGrid(gvTransmittals, dataTable, sortExpresion, sortDirection);
        }

        private void BindSubContracts()
        {
            String sortExpresion = GvSubContractsSortExpression;
            SortDirection sortDirection = GvSubContractsSortDireccion;
            DataTable dataTable = new DataTable();
            DataRow dataRow = null;
            List<ContractInfo> approvedSubContracts = null;

            dataTable.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("IdStr", System.Type.GetType("System.String"));
            dataTable.Columns.Add("Description", System.Type.GetType("System.String"));
            dataTable.Columns.Add("WriteDate", System.Type.GetType("System.DateTime"));
            dataTable.Columns.Add("SiteInstruction", System.Type.GetType("System.String"));
            dataTable.Columns.Add("SubContractorReference", System.Type.GetType("System.String"));
            dataTable.Columns.Add("NumVariations", System.Type.GetType("System.Int32"));
            dataTable.Columns.Add("TotalVariations", System.Type.GetType("System.Decimal"));

            if (tradeParticipationInfo.Trade.Contract != null && tradeParticipationInfo.Trade.Contract.IsApproved)
            {
                approvedSubContracts = tradeParticipationInfo.Trade.Contract.ApprovedSubcontracts;

                foreach (ContractInfo contractInfo in approvedSubContracts)
                {
                    dataRow = dataTable.NewRow();

                    dataRow["IdStr"] = contractInfo.IdStr;
                    dataRow["Description"] = contractInfo.Description;
                    dataRow["SiteInstruction"] = contractInfo.SiteInstruction;
                    dataRow["SubContractorReference"] = contractInfo.SubContractorReference;

                    if (contractInfo.Id != null) dataRow["Id"] = (Int32)contractInfo.Id;
                    if (contractInfo.WriteDate != null) dataRow["WriteDate"] = (DateTime)contractInfo.WriteDate;
                    if (contractInfo.NumVariations != null) dataRow["NumVariations"] = (Int32)contractInfo.NumVariations;
                    if (contractInfo.TotalVariations != null) dataRow["TotalVariations"] = (Decimal)contractInfo.TotalVariations;

                    dataTable.Rows.Add(dataRow);
                }
            }

            GridPageSizeSubContracts.Visible = dataTable.Rows.Count > 0;
            Utils.BindSortedGrid(gvSubContracts, dataTable, sortExpresion, sortDirection);
        }

        private void BindQuote()
        {
            lblStatusName.Text = UI.Utils.SetFormString(tradeParticipationInfo.StatusNameSubcontractor);
            lblQuoteDate.Text = UI.Utils.SetFormDateTime(tradeParticipationInfo.QuoteDate);

            if (tradeParticipationInfo.Status == TradeParticipationInfo.StatusEnum.Invited || tradeParticipationInfo.Status == TradeParticipationInfo.StatusEnum.InvitedOnLine || tradeParticipationInfo.Status == TradeParticipationInfo.StatusEnum.Tendering)
                if (!tradeParticipationInfo.IsClosed)
                {
                    TimerControlDate = tradeParticipationInfo.QuoteDueDate;
                    tmrClosing.Enabled = true;

                    lblClosing.Text = UI.Utils.SetFormTimeSpan(tradeParticipationInfo.ClosingTime);
                }
                else
                {
                    tmrClosing.Enabled = false;
                    lblClosing.Text = "Closed";
                }
            else
            {
                tmrClosing.Enabled = false;
                lblClosing.Text = String.Empty;
            }

            if (tradeParticipationInfo.QuoteParticipation != null)
            {
                lnkPrint.NavigateUrl = "~/Modules/Projects/ShowQuote.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;
                phPrintQuote.Visible = true;

                if (tradeParticipationInfo.IsEditable)
                {
                    XmlDocument xmlDocument = TradesController.GetInstance().CheckParticipation(tradeParticipationInfo.QuoteParticipation);
                    if (xmlDocument.DocumentElement != null)
                    {
                        AddNode(xmlDocument.DocumentElement, TreeView1.Nodes[0]);

                        cmdSubmitQuote.Enabled = false;
                        cmdSubmitQuote.OnClientClick = String.Empty;
                    }
                    else
                    {
                        cmdSubmitQuote.Enabled = true;
                        cmdSubmitQuote.OnClientClick = "javascript:return confirm('Submit quote ?');";
                    }

                    lnkEditQuote.Enabled = true;
                    lnkEditQuote.NavigateUrl = "~/Modules/Projects/EditParticipation.aspx?ParticipationId=" + tradeParticipationInfo.QuoteParticipation.IdStr;
                }
                else
                {
                    cmdSubmitQuote.Enabled = false;
                    cmdSubmitQuote.OnClientClick = String.Empty;

                    lnkEditQuote.Enabled = false;
                    lnkEditQuote.NavigateUrl = String.Empty;
                }

                ViewComparison1.TradeParticipation = tradeParticipationInfo.QuoteParticipation;
                ViewComparison1.ComparisonType = tradeParticipationInfo.Type;
                ViewComparison1.AllowEdit = tradeParticipationInfo.IsEditable;
                ViewComparison1.RePaint = true;
                
                txtComments.Text = UI.Utils.SetFormString(tradeParticipationInfo.QuoteParticipation.Comments);

                ViewQuoteDrawings1.TradeParticipation = tradeParticipationInfo;
                ViewQuoteDrawings1.RePaint = true;
            }
            else
            {
                pnlQuote.Visible = false;
                phOnlineQuote.Visible = true;
            }
        }

        private void BindParticipation()
        {
            ContractsController contractsController = ContractsController.GetInstance();

            lblInvitationDate.Text = UI.Utils.SetFormDate(tradeParticipationInfo.InvitationDate);
            lblQuoteDueDate.Text = UI.Utils.SetFormDateTime(tradeParticipationInfo.QuoteDueDate);

            if (tradeParticipationInfo.Trade.Project.ContractsAdministrator != null)
            {
                lnkCA.Text = UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.ContractsAdministrator.Name);
                lnkCA.NavigateUrl = "mailto:" + UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.ContractsAdministrator.Email) + "?subject=" + UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.Name) + " - " + UI.Utils.SetFormString(tradeParticipationInfo.Trade.Name);
                lblCAPhone.Text = UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.ContractsAdministrator.Phone);
            }

            if (tradeParticipationInfo.Trade.Project.ProjectManager != null)
            {
                lnkPM.Text = UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.ProjectManager.Name);
                lnkPM.NavigateUrl = "mailto:" + UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.ProjectManager.Email) + "?subject=" + UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.Name) + " - " + UI.Utils.SetFormString(tradeParticipationInfo.Trade.Name);
                lblPMPhone.Text = UI.Utils.SetFormString(tradeParticipationInfo.Trade.Project.ProjectManager.Phone);
            }

            if (tradeParticipationInfo.IsAwarded && tradeParticipationInfo.Trade.ContractApproved)
            {
                phContract.Visible = true;
                lnkContract.NavigateUrl = "~/Modules/Contracts/ShowContract.aspx?ContractId=" + tradeParticipationInfo.Trade.Contract.IdStr;

                GvSubContractsSortExpression = "WriteDate";
                GvSubContractsSortDireccion = SortDirection.Descending;

                BindSubContracts();
            }

            if (tradeParticipationInfo.QuoteFile != null)
            {
                pnlViewQuoteFile.Visible = true;
                pnlUploadQuoteFile.Visible = false;
                spnRemoveQuoteFile.Visible = tradeParticipationInfo.IsEditable;

                FileMetaData fileMetaData = Client.Utils.GetFileMetaData(UI.Utils.PathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tradeParticipationInfo.QuoteFileName));

                if (fileMetaData.Exist)
                {
                    lnkViewQuoteFile.Text = tradeParticipationInfo.QuoteFile;
                    lnkViewQuoteFile.NavigateUrl = "~/Modules/Projects/ShowQuoteFile.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;

                    lblFileSize.Text = "(" + UI.Utils.SetFormFileSize(fileMetaData.Length) + ")";
                }
                else
                {
                    lblFileSize.Text = tradeParticipationInfo.QuoteFile + " (File not found)";
                }
            }
            else
            {
                pnlViewQuoteFile.Visible = false;

                if (tradeParticipationInfo.IsEditable)
                {
                    pnlUploadQuoteFile.Visible = true;
                    lnkUploadQuoteFile.NavigateUrl = "~/Modules/Projects/UploadQuoteFile.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;
                }
            }
            
            lnkInvitation.NavigateUrl = "~/Modules/Projects/ShowInvitation.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;
            lnkCheckList.NavigateUrl = "~/Modules/Projects/ShowCheckList.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;

            GvTransmittalsSortExpression = "TransmissionDate";
            GvTransmittalsSortDireccion = SortDirection.Descending;
            BindTransmittals();

            GvDrawingsSortExpression = "LastRevisionDate";
            GvDrawingsSortDireccion = SortDirection.Descending;
            BindDrawings();

            if (tradeParticipationInfo.QuoteParticipation != null)
            {
                phEditQuote.Visible = tradeParticipationInfo.IsEditable;
                phSubmitQuote.Visible = tradeParticipationInfo.IsEditable;
            }
            else
            {
                phEditQuote.Visible = false;
                phSubmitQuote.Visible = false;
            }

            /*cpeQuote.Collapsed = !tradeParticipationInfo.IsSubmittable;*/

            lblSizeError.Text = "Exceedes maximum allowed (" + UI.Utils.SetFormFileSize(SOS.FileTransferService.Client.Utils.MaxBytes) + ")";

            BindQuote();
        }


        private void ReBindParticipation()
        {
            GridPageSizeDrawings.GridView = gvDrawings;
            GridPageSizeDrawings.NumRecordsSelected += new System.EventHandler(GridPageSizeDrawings_OnNumRecordsSelected);

            GridPageSizeTransmittals.GridView = gvTransmittals;
            GridPageSizeTransmittals.NumRecordsSelected += new System.EventHandler(GridPageSizeTransmittals_OnNumRecordsSelected);

            GridPageSizeSubContracts.GridView = gvSubContracts;
            GridPageSizeSubContracts.NumRecordsSelected += new System.EventHandler(GridPageSizeSubContracts_OnNumRecordsSelected);
        }

        private void BindTimer()
        {
            if (TimerControlDate != null)
                if ((DateTime)TimerControlDate > DateTime.Now)
                    lblClosing.Text = UI.Utils.SetFormTimeSpan(((DateTime)TimerControlDate).Subtract(DateTime.Now));
                else
                    Response.Redirect(Request.Url.ToString(), true);
        }

        private void ClearSelectedDrawings()
        {
            drawingsLinks = String.Empty;
            drawingsSizes = String.Empty;
            drawingsIds = String.Empty;
        }

        private void RegisterScriptDrawings() 
        {
            String strScript = String.Format(@"
                <script language=""javascript"" type=""text/javascript"">
                var maxSize = {0};
                var arrayLinks = new Array({1});
                var arraySizes = new Array({2});
                var arrayIds = new Array({3});
                var lnkDownloadId = ""#{4}"";
                var lblErrorId = ""#{5}"";
                var lblSelectedId = ""#{6}"";
                var lblSizeId = ""#{7}"";
                var checkAllId = ""#{8}"";
                var lnkDownloadURL = ""{9}"";
                var totalSize;
                var numFiles;
                var Ids;
                var IsChecked;

                $(lnkDownloadId).click(function() {{ alert(""Please wait while your download starts. It can take up to a minute after clicking Ok.""); }});

                function UpdateDownloadInfo()
                {{
                    totalSize = 0;
                    numFiles = 0;
                    Ids = """";
                    for (var i = 0; i < arrayLinks.length; i++)
                    {{
                        if($(arrayLinks[i]).prop(""checked""))
                        {{
                            Ids = Ids == """" ? arrayIds[i] : Ids + "","" + arrayIds[i];
                            totalSize = totalSize + arraySizes[i];
                            numFiles++;
                        }}
                    }}

                    if (numFiles > 1)
                    {{
                        if (totalSize <= maxSize)
                        {{
                            $(lnkDownloadId).prop(""href"", lnkDownloadURL + Ids);
                            $(lnkDownloadId).prop(""title"", ""Download"");
                            $(lnkDownloadId).show(""slow"")
                            $(lblErrorId).hide(""slow"");
                        }}
                        else
                        {{
                            $(lnkDownloadId).hide(""slow"");
                            $(lblErrorId).show(""slow"");
                        }}
                        
                        $(lblSelectedId).text(numFiles);
                        $(lblSizeId).text(FormatFileSize(totalSize));
                    }}
                    else
                    {{
                        $(lnkDownloadId).hide(""slow"");
                        $(lblErrorId).hide(""slow"");
                        $(lblSelectedId).text("""");
                        $(lblSizeId).text("""");
                        $(lnkDownloadId).prop(""href"", """");
                        $(lnkDownloadId).prop(""title"", """");
                    }}
                }}

                function SelectAllDrawings()
                {{
                    IsChecked = $(checkAllId).prop(""checked"");

                    for (var i = 0; i < arrayLinks.length; i++) 
                        $(arrayLinks[i]).prop(""checked"", IsChecked);

                    UpdateDownloadInfo();
                }}

                function FormatFileSize(fileSize)
                {{
                    var fileSizeKb = fileSize / 1024;
                    return fileSizeKb < 1024 ? fileSizeKb.toFixed(2) + "" KB"" : (fileSizeKb / 1024).toFixed(2) + "" MB"";
                }}

                function InitializeDownloadInfo()
                {{
                    $(lnkDownloadId).hide();
                    $(lblErrorId).hide();
                }}
                </script>",
                SOS.FileTransferService.Client.Utils.MaxBytes.ToString(),
                drawingsLinks,
                drawingsSizes,
                drawingsIds,
                lnkDownloadAll.ClientID,
                lblSizeError.ClientID,
                lblSelected.ClientID,
                lblSize.ClientID,
                checkAllId,
                String.Format("ShowDrawingRevision.aspx?ParticipationId={0}&DrawingRevisionIds=", tradeParticipationInfo.IdStr));

            ScriptManager.RegisterStartupScript(this, this.GetType(), "DrawingsDownload", strScript, false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DrawingsDownloadInitialize", "InitializeDownloadInfo();", true);
        }

        private List<String> GetFileNames(String drawingRevisionIds)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<String> fileNames = new List<String>();
            String[] Ids = drawingRevisionIds.Split(',');
            DrawingRevisionInfo drawingRevisionInfo;
            String fileName;

            for (int i = 0; i < Ids.Length; i++)
            {
                drawingRevisionInfo = tradesController.GetDrawingRevision(Int32.Parse(Ids[i]));
                Core.Utils.CheckNullObject(drawingRevisionInfo, Ids[i], "Drawing Revision");
                projectsController.CheckViewCurrentUser(drawingRevisionInfo, tradeParticipationInfo);

                fileName = UI.Utils.Path(drawingRevisionInfo.Drawing.Project.AttachmentsFolder, drawingRevisionInfo.File);
                fileNames.Add(fileName);
            }

            return fileNames;
        }

        public void StartSendFile(String drawingRevisionIds)
        {
            List<String> fileNames = GetFileNames(drawingRevisionIds);
            IAsyncResult iAsyncResult = Client.Utils.StartGetFileData(fileNames.ToArray());
            Session["MethodCallInfo"] = iAsyncResult;
        }

        public Boolean EndSendFile(List<String> fileNames)
        {
            Boolean isCompleted;

            if (Session["MethodCallInfo"] != null)
            {
                IAsyncResult iAsyncResult = (IAsyncResult)Session["MethodCallInfo"];
                if (iAsyncResult.IsCompleted)
                {
                    isCompleted = true;

                    try
                    {
                        Byte[] fileData = Client.Utils.EndGetFileData(iAsyncResult);

                        // Do something like: 
                        // Remove message waiting for response and send the file (this will be done on javascript when this method returns true)
                        // Utils.SendFile(fileData, responseFileName);
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        Session["MethodCallInfo"] = null;
                    }
                }
                else
                {
                    isCompleted = false;
                }
            }
            else
            {
                isCompleted = true;
            }

            return isCompleted;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {

            


            String postBackControl = Request.Params.Get("__EVENTTARGET");

            if (postBackControl != null && postBackControl == tmrClosing.UniqueID)
                BindTimer();
            else
            {
                TradesController tradesController = TradesController.GetInstance();
                ProjectsController projectsController = ProjectsController.GetInstance();
                ContractsController contractsController = ContractsController.GetInstance();
                ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();

                try
                {
                    Security.CheckAccess(Security.userActions.ViewParticipationSubContractor);
                    String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                    tradeParticipationInfo = tradesController.GetDeepTradeParticipationWithQuoteAndTransmittal(Int32.Parse(parameterParticipationId));
                    Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Participation");
                    contractsController.CheckViewCurrentUser(tradeParticipationInfo);

                    if (!tradeParticipationInfo.IsOpen)
                    {
                        tradeParticipationInfo.OpenDate = DateTime.Now;
                        tradesController.UpdateTradeParticipationOpenDate(tradeParticipationInfo);
                    }

                    tradeParticipationInfo.Trade = tradesController.GetTradeWithDrawingsAndItems(tradeParticipationInfo.Trade.Id);

                    if (tradeParticipationInfo.Trade.Contract != null)
                        tradeParticipationInfo.Trade.Contract = contractsController.GetContractWithModificationsSubContractsAndVariations(tradeParticipationInfo.Trade.Contract.Id);

                    if (tradeParticipationInfo.IsActive)
                    {
                        tradeParticipationInfo.Trade.Project = projectsController.GetProjectWithDrawingsActive(tradeParticipationInfo.Trade.Project.Id);
                        tradeParticipationInfo.Trade.Project.Transmittals = projectsController.GetTransmittalsWithRevisionsActive(tradeParticipationInfo.Trade.Project, contactInfo.SubContractor);
                    }
                    else
                    {
                        tradeParticipationInfo.Trade.Project = projectsController.GetProjectWithDrawingsProposal(tradeParticipationInfo.Trade.Project.Id);
                        tradeParticipationInfo.Trade.Project.Transmittals = projectsController.GetTransmittalsWithRevisionsProposal(tradeParticipationInfo.Trade.Project, contactInfo.SubContractor);
                    }

                    if (tradeParticipationInfo.Trade.ContractsAdministrator != null)
                        tradeParticipationInfo.Trade.Project.ContractsAdministrator = tradeParticipationInfo.Trade.ContractsAdministrator;

                    if (tradeParticipationInfo.Trade.ProjectManager != null)
                        tradeParticipationInfo.Trade.Project.ProjectManager = tradeParticipationInfo.Trade.ProjectManager;

                    if (tradeParticipationInfo.QuoteParticipation != null)
                    {
                        tradeParticipationInfo.QuoteParticipation.Trade = tradeParticipationInfo.Trade;
                        tradeParticipationInfo.Trade.Participations = new List<TradeParticipationInfo>();
                        tradeParticipationInfo.Trade.Participations.Add(tradeParticipationInfo.QuoteParticipation);
                        tradeParticipationInfo.QuoteParticipation.QuoteFile = tradeParticipationInfo.QuoteFile;
                    }

                    InitializeForm();

                    if (!Page.IsPostBack)
                        BindParticipation();

                    ReBindParticipation();
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (fileData != null)
            {
                Utils.SendFile(fileData, "CompressedFile.zip");
            }
        }

        protected void cmdSubmitQuote_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().SubmitTradeParticipation(tradeParticipationInfo);
                pnlMessage.Visible = true;
                BindQuote();
               

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
                
            }
        }

     

        protected void gvDrawings_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDrawings.PageIndex = e.NewPageIndex;
            BindDrawings();
        }

        protected void gvDrawings_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvDrawings.PageIndex = 0;

            if (GvDrawingsSortExpression == e.SortExpression)
                GvDrawingsSortDireccion = Utils.ChangeSortDirection(GvDrawingsSortDireccion);
            else
            {
                GvDrawingsSortExpression = e.SortExpression;
                GvDrawingsSortDireccion = GvDrawingsSortExpression == "LastRevisionDate" ? SortDirection.Descending : SortDirection.Ascending;
            }

            BindDrawings();
        }

        protected void gvDrawings_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvDrawings, e, GvDrawingsSortExpression, GvDrawingsSortDireccion);
        }

        protected void gvDrawings_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            CheckBox chkSelect;
            DataRow dataRow;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                chkSelect = e.Row.FindControl("chkSelectAll") as CheckBox;
                checkAllId = chkSelect.ClientID;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                chkSelect = e.Row.FindControl("chkSelect") as CheckBox;

                if (chkSelect.Visible)
                {
                    dataRow = drawingsDataTable.Rows.Find(DataBinder.Eval(e.Row.DataItem, "ID"));

                    drawingsLinks = drawingsLinks == String.Empty ? "\"#" + chkSelect.ClientID + "\"" : drawingsLinks + ",\"#" + chkSelect.ClientID + "\"";
                    drawingsSizes = drawingsSizes == String.Empty ? dataRow["LastRevisionFileSize"].ToString() : drawingsSizes + "," + dataRow["LastRevisionFileSize"].ToString();
                    drawingsIds = drawingsIds == String.Empty ? "\"" + dataRow["LastRevisionIdStr"] + "\"" : drawingsIds + ",\"" + dataRow["LastRevisionIdStr"] + "\"";
                }
            }
        }

        protected void GridPageSizeDrawings_OnNumRecordsSelected(object sender, EventArgs e)
        {
            BindDrawings();
        }

        protected void gvTransmittals_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTransmittals.PageIndex = e.NewPageIndex;
            BindTransmittals();
        }

        protected void gvTransmittals_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvTransmittals.PageIndex = 0;

            if (GvTransmittalsSortExpression == e.SortExpression)
                GvTransmittalsSortDireccion = Utils.ChangeSortDirection(GvTransmittalsSortDireccion);
            else
            {
                GvTransmittalsSortExpression = e.SortExpression;
                GvTransmittalsSortDireccion = GvTransmittalsSortExpression == "TransmissionDate" ? SortDirection.Descending : SortDirection.Ascending;
            }

            BindTransmittals();
        }

        protected void gvTransmittals_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvTransmittals, e, GvTransmittalsSortExpression, GvTransmittalsSortDireccion);
        }

        protected void GridPageSizeTransmittals_OnNumRecordsSelected(object sender, EventArgs e)
        {
            BindTransmittals();
        }

        protected void gvSubContracts_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSubContracts.PageIndex = e.NewPageIndex;
            BindSubContracts();
        }

        protected void gvSubContracts_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvSubContracts.PageIndex = 0;

            if (GvSubContractsSortExpression == e.SortExpression)
                GvSubContractsSortDireccion = Utils.ChangeSortDirection(GvSubContractsSortDireccion);
            else
            {
                GvSubContractsSortExpression = e.SortExpression;
                GvSubContractsSortDireccion = GvSubContractsSortExpression == "WriteDate" ? SortDirection.Descending : SortDirection.Ascending;
            }

            BindSubContracts();
        }

        protected void gvSubContracts_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvSubContracts, e, GvSubContractsSortExpression, GvSubContractsSortDireccion);
        }

        protected void GridPageSizeSubContracts_OnNumRecordsSelected(object sender, EventArgs e)
        {
            BindSubContracts();
        }

        protected void lnkRemoveQuoteFile_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            String tmpFileName = tradeParticipationInfo.QuoteFileName;
            FileMetaData fileMetaData;

            try
            {
                tradeParticipationInfo.QuoteFile = null;
                tradeParticipationInfo.QuoteParticipation.QuoteFile = null;
                tradesController.UpdateTradeParticipationQuoteFile(tradeParticipationInfo);

                fileMetaData = Client.Utils.GetFileMetaData(UI.Utils.PathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tmpFileName));
                
                if (fileMetaData.Exist)
                    Client.Utils.DeleteFile(UI.Utils.PathQuotesFile(tradeParticipationInfo.Trade.Project.AttachmentsFolder, tmpFileName));

                BindParticipation();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
