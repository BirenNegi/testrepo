using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;
using System.IO;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SOS.Web
{
    public partial class SearchSiteOrderDocs : SOSPage  //System.Web.UI.Page
    {
        private SiteOrderInfo SiteOrderInfo = null;
        private SiteOrderDocInfo SOD = null;
        private ProjectInfo ProjectInfo = null;
        private PeopleInfo PeopleInfo = null;
        private String parameterProjectId;
        private String parameterOrderTyp;
        private String parameterOrderId;
        #region Public properties

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (ProjectInfo == null)
                return null;

            tempNode.ParentNode.Url += "?ProjectId=" + ProjectInfo.IdStr + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp;

            tempNode.ParentNode.ParentNode.Title = ProjectInfo.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + ProjectInfo.IdStr + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp;

            return currentNode;
        }

        protected String GvSiteOrderDocsSortExpression
        {
            get { return (String)ViewState["GvSiteOrderDocsSortExpression"]; }
            set { ViewState["GvSiteOrderDocsSortExpression"] = value; }
        }

        protected SortDirection GvSiteOrdersSortDirection
        {
            get { return (SortDirection)ViewState["GvSiteOrdersSortDirection"]; }
            set { ViewState["GvSiteOrdersSortDirection"] = value; }
        }
#endregion

#region Private Methods
        private void BindSearch()
        {

            GvSiteOrderDocsSortExpression = "DocTitle";
            GvSiteOrdersSortDirection = SortDirection.Ascending;
            gvSiteOrderDocs.DataSource = SiteOrderInfo.Docs;
            gvSiteOrderDocs.DataBind();

        }
        #endregion


        #region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
        SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
           PeopleController PeopleController = PeopleController.GetInstance();
            parameterProjectId = Request.Params["ProjectId"];
        parameterOrderId = Request.Params["OrderId"];
        parameterOrderTyp = Request.Params["OrderTyp"];
        ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
            if (parameterOrderTyp == "Mat") TitleBar1.Title = "Edit Material Order Documents - D" + Int32.Parse(parameterOrderId).ToString("000000");
            if (parameterOrderTyp == "Hir") TitleBar1.Title = "Equipment Hire Documents - D" + Int32.Parse(parameterOrderId).ToString("000000"); 
            if (parameterOrderTyp == "Ins") TitleBar1.Title = "Site Instruction Documents - D" + Int32.Parse(parameterOrderId).ToString("000000");
            lblMessage.Text = "Adding Documents: Select document, fill in document description and click Update Button";  
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            parameterProjectId = Request.Params["ProjectId"];
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            
            //Int32.Parse(parameterProjectId)
            try
            {
                Security.CheckAccess(Security.userActions.SearchSiteOrderDocs);
                //phAddNew.Visible = (Security.ViewAccess(Security.userActions.EditSubContractor));
                SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterOrderId));
                SiteOrderInfo.Docs = SiteOrdersController.GetSiteOrderDocs(Int32.Parse(parameterOrderId));
                if (!Page.IsPostBack)
                {
                    BindSearch();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void odsSiteOrderDocs_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SiteOrdersController.GetInstance();
        }

        protected void ddlSiteOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSiteOrderDocs.PageIndex = 0;
        }

        //protected void cmdSearch_Click(object sender, EventArgs e)
        //{
        //    gvSiteOrderDocs.PageIndex = 0;
        //}

        protected void gvSiteOrderDocs_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvSiteOrderDocs.PageIndex = 0;

            if (GvSiteOrderDocsSortExpression == e.SortExpression)
                GvSiteOrdersSortDirection = Utils.ChangeSortDirection(GvSiteOrdersSortDirection);
            else
            {
                GvSiteOrderDocsSortExpression = e.SortExpression;
                GvSiteOrdersSortDirection = SortDirection.Ascending;
            }

            e.SortDirection = GvSiteOrdersSortDirection;
        }

        protected void gvSiteOrderDocs_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            GvSiteOrdersSortDirection = SortDirection.Ascending;
            Utils.SortedGridSetOrderImage(gvSiteOrderDocs, e, GvSiteOrderDocsSortExpression, GvSiteOrdersSortDirection);
        }


        #endregion
        private void FormToObjects()
        {
            // = (int)UI.Utils.GetFormInteger(parameterProjectId);
            SOD = new SiteOrderDocInfo();

            SOD.ProjectId = (int)UI.Utils.GetFormInteger(parameterProjectId); //(int)UI.Utils.GetFormInteger(parameterProjectId);
            SOD.DocTitle = txtDocName.Text;
            SOD.OrderId = (int)UI.Utils.GetFormInteger(parameterOrderId);
            SOD.DocName = "NONE";
            SOD.DocNameOrig = "NONE";
            SOD.Status = "CUR";
  
        }
        void gv_RowCreated(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnUpdate = new ImageButton();
                btnUpdate.Click += cmdShowDoc_Click;
                TableCell tc = new TableCell();

                tc.Controls.Add(btnUpdate);
                e.Row.Cells.Add(tc);
            }
        }
        protected void gvSiteOrderDocs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
 
            }
        }
        protected void cmdDeleteDoc_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;
                string arguments = lnkbtn.CommandArgument.ToString();
                string[] args = arguments.Split(';');
                if (args.Length > 1)
                {

                string DocId = args[0];
                string DocName = args[1];
                string[] Splits = DocName.Split('.');
                string parameterOrderId = Request.Params["OrderId"];
                string parameterOrderTyp = Request.Params["OrderTyp"];
                string parameterProjectId = Request.Params["ProjectId"];
                string appPath = Request.PhysicalApplicationPath;
                //File to be downloaded.
                string TargetFileName = parameterOrderId + "_" + DocId;
                if (Splits.Length > 1)
                {
                    TargetFileName += "." + Splits[Splits.GetUpperBound(0)];
                }
                //Path of the File to be downloaded.
                String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
                string filePath = @DocFolder + "P" + parameterProjectId + "\\";
                //Server.MapPath(string.Format("~/Files/{0}", fileName));
                 string savePath = filePath;
                //Content Type and Header.
                if (File.Exists(savePath + TargetFileName))
                {
                    try
                    {
                        //DirectoryInfo di = Directory.CreateDirectory(savePath);

                        File.Delete(savePath + TargetFileName);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    int Result = (int)SiteOrdersController.GetInstance().DeleteSiteOrderDoc((int)UI.Utils.GetFormInteger(DocId));
                    Response.Redirect("~/Modules/SiteOrders/SearchSiteOrderDocs.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);

                    }
                }
            }
        }
        //cmdDownload_Click
        protected void cmdDownload_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;
                //Get the value passed from gridview
                string arguments = lnkbtn.CommandArgument.ToString();
                string[] args = arguments.Split(';');
                string DocId = args[0];
                string DocName = args[1];
                string parameterOrderId = Request.Params["OrderId"];
                string parameterOrderTyp = Request.Params["OrderTyp"];
                string parameterProjectId = Request.Params["ProjectId"];
                string result = SOS.UI.Utils.showSiteOrderDoc(this, parameterProjectId, parameterOrderId, DocId, DocName);
            }
            //
        }
        protected void cmdShowDoc_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;
                //Get the value passed from gridview
                string arguments = lnkbtn.CommandArgument.ToString();
                string[] args = arguments.Split(';');
                string DocId = args[0];
                string DocName = args[1];
                string parameterOrderId = Request.Params["OrderId"];
                string parameterOrderTyp = Request.Params["OrderTyp"];
                string parameterProjectId = Request.Params["ProjectId"];
                string result = SOS.UI.Utils.showSiteOrderDoc(this, parameterProjectId, parameterOrderId, DocId, DocName);
            }
            //
        }
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            parameterProjectId = Request.Params["ProjectId"];
            if (parameterOrderId == null)
            {
                Response.Redirect("~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId);

            }
            else
            {
                switch (parameterOrderTyp)
                {
                    case "Mat":
                        Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                        break;
                    case "Ins":
                        Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                        break;
                    case "Hir":
                        Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderHire.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                        break;
                    default:
                        Response.Redirect("~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId);
                        break;
                }
               
            }
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdAddNew_Click(object sender, EventArgs e)
        {
            SaveItem(sender, e,true);
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            SaveItem(sender, e,false);
        }
        protected void SaveItem(object sender, EventArgs e,bool ExitMode )
        {
            if (Page.IsValid)
            {
                PeopleController PeopleController = PeopleController.GetInstance();
                if (txtDocName.Text == "")
                {
                    lblMessage.Text = "Document Name is Mandatory!";
                    return;
                }

                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["parameterOrderTyp"];
                parameterProjectId = Request.Params["ProjectId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                string saveDir = @"P" + parameterProjectId + "\\";
                String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
                string filePath = @DocFolder; // + "P" + parameterProjectId + "\\";
                if (FileUpload1.HasFile)
                {
                    FormToObjects();
                    string[] subs = FileUpload1.FileName.Split('.');
                    string ext = subs[subs.Length - 1];
                    string extL = ext.ToLower();
                    string extFile = ext;

                    if (extL == "jpg" || extL == "png" || extL == "tif" || extL == "tiff") extFile = "pdf";
                    if (subs.Length > 1) subs[subs.Length - 1] = extFile;

                    SOD.DocName = string.Join(".", subs);
                    SOD.CreatedBy = (int)Web.Utils.GetCurrentUserId();   // DS20231031
                    int DocID = (int)SiteOrdersController.GetInstance().AddSiteOrderDoc(SOD);

                    string TargetFileName = parameterOrderId + "_" + DocID.ToString() + "." + extFile;
                    string savePath = filePath + saveDir;
                    string result = SOS.UI.Utils.UploadSiteOrderDoc(this, FileUpload1, parameterProjectId, parameterOrderId, DocID.ToString(), SOD.DocName);
                    SiteOrderDocSearchInfo SODS = new SiteOrderDocSearchInfo();
                    int PeopleId = (int)Web.Utils.GetCurrentUserId();
                    PeopleInfo = PeopleController.GetPersonById((int)Web.Utils.GetCurrentUserId());
                    SODS.DocName = SOD.DocName;
                    SODS.DocTitle = SOD.DocTitle;
                    SODS.DocDate = DateTime.Today;
                    SODS.LastName = PeopleInfo.LastName;
                    SODS.Id = DocID;
                    SODS.CreatedBy = (int)Web.Utils.GetCurrentUserId();   // DS20231031
                    SiteOrderInfo.Docs.Add(SODS);
                    if (ExitMode == true)
                    {
                    switch (parameterOrderTyp)
                       {
                        case "Mat":
                            Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                            break;
                        case "Ins":
                            Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                            break;
                        case "Hir":
                            Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderHire.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                            break;
                        default:
                            Response.Redirect("~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId);
                            break;
                       }

                    }
                }
                else
                {
                    lblMessage.Text = "Document / File is Mandatory!";
                    return;
                }
                BindSearch();
                //Response.Redirect("~/Modules/SiteOrders/SearchSiteOrderDocs.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            }

        }

    }


}
