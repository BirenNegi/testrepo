using System;
using System.Web;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using SOS.FileTransferService.DataContracts;

using Client = SOS.FileTransferService.Client;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListDrawingsTransmittalsPage : SOSPage
    {

        #region Members
        private ProjectInfo projectInfo = null;
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
        #endregion


        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = projectInfo.Name;

            tempNode.Title = "Drawings/Transmittals";

            tempNode.ParentNode.Title = projectInfo.Name + (projectInfo.IsStatusProposal ? " (Proposal)" : ""); ;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        //private void BindProject()
        //{
        //    TradesController tradesController = TradesController.GetInstance();
        //    List<DrawingTypeInfo> drawingTypeInfoList = tradesController.GetDrawingTypes();
        //    List<DrawingInfo> drawingInfoList = new List<DrawingInfo>();
        //    TreeNode treeNodeType;
        //    TreeNode treeNodeTypeProposal;
        //    TreeNode treeNodeDrawing;
        //    TreeNode treeNodeRevision;
        //    TreeNode treeNodeSubcontractor;
        //    TreeNode treeNodeTransmittal;
        //    String DeepZoomCode;

        //    if (projectInfo.IsStatusProposal)
        //    {
        //        //cpe1.Collapsed = false;
        //        pnlStatusActive.Visible = false;
        //    }

        //    if (!projectInfo.IsEmptyDrawingsActvie)
        //    {
        //        cmdCopyDrawings.Enabled = false;
        //        cmdCopyDrawings.OnClientClick = String.Empty;
        //    }

        //    foreach (DrawingTypeInfo drawingTypeInfo in drawingTypeInfoList)
        //    {
        //        treeNodeType = new TreeNode(drawingTypeInfo.Name, drawingTypeInfo.IdStr, "", String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", Info.TypeActive, projectInfo.IdStr, drawingTypeInfo.IdStr), "");
        //        treeNodeTypeProposal = new TreeNode(drawingTypeInfo.Name, drawingTypeInfo.IdStr, "", String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", Info.TypeProposal, projectInfo.IdStr, drawingTypeInfo.IdStr), "");

        //        treeNodeType.Expanded = false;
        //        treeNodeTypeProposal.Expanded = false;

        //        drawingInfoList = tradesController.GetDrawings(projectInfo, drawingTypeInfo);
        //        foreach (DrawingInfo drawingInfo in drawingInfoList)
        //        {
        //            treeNodeDrawing = new TreeNode(drawingInfo.Name + (drawingInfo.Description != null ? "-" + drawingInfo.Description : String.Empty), drawingInfo.IdStr, "", "~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingInfo.IdStr, "");
        //            treeNodeDrawing.Expanded = false;

        //            foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
        //            {
        //                treeNodeRevision = new TreeNode(drawingRevisionInfo.Number + (drawingRevisionInfo.RevisionDate != null ? " - " + UI.Utils.SetFormDate(drawingRevisionInfo.RevisionDate) : String.Empty), drawingRevisionInfo.IdStr, "", "~/Modules/Projects/ViewDrawingRevision.aspx?DrawingRevisionId=" + drawingRevisionInfo.IdStr, "");
        //                treeNodeDrawing.ChildNodes.Add(treeNodeRevision);
        //            }

        //            if (drawingInfo.IsActive)
        //                treeNodeType.ChildNodes.Add(treeNodeDrawing);
        //            else
        //                treeNodeTypeProposal.ChildNodes.Add(treeNodeDrawing);
        //        }

        //        tvDrawings.Nodes[0].ChildNodes.Add(treeNodeType);
        //        tvDrawingsProposal.Nodes[0].ChildNodes.Add(treeNodeTypeProposal);
        //    }

        //    lnkDrawings.NavigateUrl = String.Format("~/Modules/Projects/ShowDrawings.aspx?Type={0}&ProjectId={1}", Info.TypeActive, projectInfo.IdStr);
        //    lnkDrawingsProposal.NavigateUrl = String.Format("~/Modules/Projects/ShowDrawings.aspx?Type={0}&ProjectId={1}", Info.TypeProposal, projectInfo.IdStr);

        //    lnkTransmittals.NavigateUrl = String.Format("~/Modules/Projects/ShowTransmittals.aspx?Type={0}&ProjectId={1}", Info.TypeActive, projectInfo.IdStr);
        //    lnkTransmittalsProposal.NavigateUrl = String.Format("~/Modules/Projects/ShowTransmittals.aspx?Type={0}&ProjectId={1}", Info.TypeProposal, projectInfo.IdStr);

        //    lnkAddTransmittal.NavigateUrl = String.Format("~/Modules/Projects/EditTransmittal.aspx?Type={0}&ProjectId={1}", Info.TypeActive, projectInfo.IdStr);
        //    lnkAddTransmittalProposal.NavigateUrl = String.Format("~/Modules/Projects/EditTransmittal.aspx?Type={0}&ProjectId={1}", Info.TypeProposal, projectInfo.IdStr);

        //    int? currentSubcontractorId = 0;
        //    treeNodeSubcontractor = null;
        //    if (projectInfo.Transmittals != null)
        //    {
        //        foreach (TransmittalInfo transmittalInfo in projectInfo.Transmittals)
        //        {
        //            if (transmittalInfo.SubContractor == null)
        //            {
        //                transmittalInfo.Contact = new ContactInfo();
        //                transmittalInfo.Contact.SubContractor = new SubContractorInfo();
        //                transmittalInfo.Contact.SubContractor.Name = "No Subcontractor";
        //            }

        //            if (transmittalInfo.SubContractor.Id != currentSubcontractorId)
        //            {
        //                treeNodeSubcontractor = new TreeNode(transmittalInfo.SubContractor.Name);
        //                treeNodeSubcontractor.Expanded = false;
        //                treeNodeSubcontractor.SelectAction = TreeNodeSelectAction.None;

        //                if (transmittalInfo.IsActive)
        //                    tvTransmittals.Nodes[0].ChildNodes.Add(treeNodeSubcontractor);
        //                else
        //                    tvTransmittalsProposal.Nodes[0].ChildNodes.Add(treeNodeSubcontractor);

        //                currentSubcontractorId = transmittalInfo.SubContractor.Id;
        //            }

        //            treeNodeTransmittal = new TreeNode(transmittalInfo.Name, transmittalInfo.IdStr, "", "~/Modules/Projects/ViewTransmittal.aspx?TransmittalId=" + transmittalInfo.IdStr, "");
        //            treeNodeSubcontractor.ChildNodes.Add(treeNodeTransmittal);
        //        }
        //    }

        //    DeepZoomCode = ConfigurationManager.AppSettings["DeepZoomCode"].ToString();

        //    if (projectInfo.DeepZoomUrl != null)
        //    {
        //        pnlDeepZoom.Visible = true;

        //        if (DeepZoomCode == DrawingInfo.DeepZoomCodeLocal)
        //        {
        //            //actSeadragon.SourceUrl = "~/" + projectInfo.DeepZoomUrl;
        //            //actSeadragon.Visible = true;
        //        }
        //        else if (DeepZoomCode == DrawingInfo.DeepZoomCodeRemote)
        //        {
        //            litScriptDeepZoom.Text = "" +
        //            "<script type='text/javascript' src='http://seadragon.com/ajax/0.8/seadragon-min.js'></script>" +
        //            "<script type='text/javascript'>" +
        //            "var viewer = null;\r" +
        //            "function initDeepZoom() {\r" +
        //            "  viewer = new Seadragon.Viewer('" + divDeepZoom.ClientID + "');\r" +
        //            "  viewer.openDzi('../../" + projectInfo.DeepZoomUrl + "')\r" +
        //            "}\r" +
        //            "Seadragon.Utils.addEvent(window, 'load', initDeepZoom);" +
        //            "</script>\r";

        //            divDeepZoom.Visible = true;
        //        }
        //    }
        //    else
        //    {
        //        pnlDeepZoom.Visible = false;
        //    }
        //}


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

            if (projectInfo.Transmittals != null)
                foreach (TransmittalInfo transmittalInfo in projectInfo.Transmittals)
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

        private void BindDrawings()
        {
           

            String sortExpresion = GvDrawingsSortExpression;
            SortDirection sortDirection = GvDrawingsSortDireccion;
            List<DrawingRevisionInfo> filteredDrawingRevisions;
            List<DrawingInfo> filteredDrawings;
            drawingsDataTable = new DataTable();

            if (projectInfo.Drawings != null)
            {
                filteredDrawings = new List<DrawingInfo>();

                foreach (DrawingInfo drawingInfo in projectInfo.Drawings)
                {
                    if (drawingInfo.DrawingRevisions != null)
                    {
                        filteredDrawingRevisions = new List<DrawingRevisionInfo>();

                        foreach (DrawingRevisionInfo drawingRevisionInfo in drawingInfo.DrawingRevisions)
                           // if (drawingRevisionInfo.RevisionDate == null)
                                filteredDrawingRevisions.Add(drawingRevisionInfo);

                        if (filteredDrawingRevisions.Count > 0)
                        {
                            drawingInfo.DrawingRevisions = filteredDrawingRevisions;
                            filteredDrawings.Add(drawingInfo);
                        }
                    }
                }


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
                    drawingInfo.DrawingRevisions = TradesController.GetInstance().GetDrawingRevisions(drawingInfo);
                        if (drawingInfo.LastRevision != null)
                        {
                            BindDrawingRevision(drawingInfo.LastRevision, drawingsDataTable, "lstTextNormal");
                        }

                    }

                ClearSelectedDrawings();
                /*GridPageSizeDrawings.Visible = drawingsDataTable.Rows.Count > 0;*/
                Utils.BindSortedGrid(gvDrawings, drawingsDataTable, sortExpresion, sortDirection);
                RegisterScriptDrawings();




            }

        }


        private void ReBindDrawings()
        {
            GridPageSizeDrawings.GridView = gvDrawings;
            GridPageSizeDrawings.NumRecordsSelected += new System.EventHandler(GridPageSizeDrawings_OnNumRecordsSelected);

            GridPageSizeTransmittals.GridView = gvTransmittals;
            GridPageSizeTransmittals.NumRecordsSelected += new System.EventHandler(GridPageSizeTransmittals_OnNumRecordsSelected);
        }


        private void BindDrawingRevision(DrawingRevisionInfo drawingRevisionInfo, DataTable dataTable, String cssClass)
        {
            long? fileSize;
            DataRow dataRow = null;
            FileMetaData fileMetaData = null;

            if (drawingRevisionInfo.File != null)
            {
                fileMetaData = Client.Utils.GetFileMetaData(UI.Utils.Path(projectInfo.AttachmentsFolder, drawingRevisionInfo.File));

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
/*---San--
               function UpdateDownloadInfo()
                {{
                    

                    totalSize = 0;
                    numFiles = 0;
                    Ids = """";
                    for (var i = 0; i < arrayLinks.length; i++)
                    {{

                        if($(arrayLinks[i]).prop(checked))
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
*/
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
                String.Format("ShowDrawingRevision.aspx?ProjectId={0}&DrawingRevisionIds=", projectInfo.IdStr));

            ScriptManager.RegisterStartupScript(this, this.GetType(), "DrawingsDownload", strScript, false);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DrawingsDownloadInitialize", "InitializeDownloadInfo();", true);
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


        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                String parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithDrawingsAndTransmittals(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.ViewProject))
                    {
                        
                    }
                    GvDrawingsSortExpression = "LastRevisionDate";
                    GvDrawingsSortDireccion = SortDirection.Descending;


                    GvTransmittalsSortExpression = "TransmissionDate";
                    GvTransmittalsSortDireccion = SortDirection.Descending;


                    BindDrawings();
                    BindTransmittals();
                   // BindProject();
                }
                ReBindDrawings();
                
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




        protected void cmdCopyDrawings_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().CopyDrawingRegister(projectInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect(String.Format("~/Modules/Projects/ListDrawingsTransmittals.aspx?ProjectId={0}", projectInfo.IdStr));
        }



#endregion

    }
}
