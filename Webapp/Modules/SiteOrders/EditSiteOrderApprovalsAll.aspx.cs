using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;
using System.IO;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using iTextSharp.text;

namespace SOS.Web
{
    public partial class EditSiteOrderApprovalsAll : SOSPage  //System.Web.UI.Page
    {
        private SiteOrderDocInfo SOD = null;
        private SiteOrderInfo SOI = null;
        private ProjectInfo ProjectInfo = null;
        private String parameterProjectId;
        private String parameterOrderTyp;
        private String parameterOrderId;
        private String parameterUserId;
        private List<SiteOrderApprovalsSearchInfo> siteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsSearchInfo>();
        private SiteOrderApprovalsSearchInfo SOA = null;
        private int c_ApprovedByPeopleId = 4 + 1;
        private int c_StatusDescription = 6 + 1;
        private int c_isApprovalCurrent = 7 + 1;
        private int c_Id = 8 + 1;
        private int c_AssignedPeopleId = 9 + 1;
        private int c_OrderId = 10 + 1;
        private int c_OrderTyp = 11 + 1;
        private int c_ValueLimit = 13 + 1;
        private int c_Reason = 15 + 1;
        private int c_OrderDateEnd = 17 + 1;
        private int c_RowColor = 18 + 1;
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

        protected String GvSiteOrderApprovalsSortExpression
        {
            get { return (String)ViewState["GvSiteOrderApprovalsSortExpression"]; }
            set { ViewState["GvSiteOrderApprovalsSortExpression"] = value; }
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
            GvSiteOrderApprovalsSortExpression = "Title";
            GvSiteOrdersSortDirection = SortDirection.Ascending;
            gvSiteOrderApprovals.DataSource = siteOrderApprovalsSearchInfoList;
            gvSiteOrderApprovals.DataBind();


            gvSiteOrderApprovals.Columns[c_isApprovalCurrent].Visible = false;
            gvSiteOrderApprovals.Columns[c_Id].Visible = false;
            gvSiteOrderApprovals.Columns[c_AssignedPeopleId].Visible = false;
            gvSiteOrderApprovals.Columns[c_OrderId].Visible = true;
            gvSiteOrderApprovals.Columns[c_OrderDateEnd].Visible = false;
            gvSiteOrderApprovals.Columns[c_RowColor].Visible = false;
           // gvSiteOrderApprovals.Columns[c_ValueLimit].Visible = false;
        }
        #endregion


        #region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
    {
        SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
        ProjectsController ProjectsController = ProjectsController.GetInstance();
        parameterProjectId = Request.Params["ProjectId"];
        parameterOrderId = Request.Params["OrderId"];
        parameterOrderTyp = Request.Params["OrderTyp"];

     //   ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
            int UserId = (int)Web.Utils.GetCurrentUserId();
            if (!Page.IsPostBack)
            {
                siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovalsAll(UserId);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            parameterProjectId = Request.Params["ProjectId"];
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            
            //Int32.Parse(parameterProjectId)
            try
            {
                Security.CheckAccess(Security.userActions.EditSiteOrderApprovals);
                //phAddNew.Visible = (Security.ViewAccess(Security.userActions.EditSubContractor));

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

        protected void odsSiteOrderApprovals_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SiteOrdersController.GetInstance();
        }

        protected void ddlSiteOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSiteOrderApprovals.PageIndex = 0;
        }

        //protected void cmdSearch_Click(object sender, EventArgs e)
        //{
        //    gvSiteOrderApprovals.PageIndex = 0;
        //}

        protected void gvSiteOrderApprovals_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvSiteOrderApprovals.PageIndex = 0;

            if (GvSiteOrderApprovalsSortExpression == e.SortExpression)
                GvSiteOrdersSortDirection = Utils.ChangeSortDirection(GvSiteOrdersSortDirection);
            else
            {
                GvSiteOrderApprovalsSortExpression = e.SortExpression;
                GvSiteOrdersSortDirection = SortDirection.Ascending;
            }

            e.SortDirection = GvSiteOrdersSortDirection;
        }

        protected void gvSiteOrderApprovals_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            GvSiteOrdersSortDirection = SortDirection.Ascending;
            Utils.SortedGridSetOrderImage(gvSiteOrderApprovals, e, GvSiteOrderApprovalsSortExpression, GvSiteOrdersSortDirection);
        }


        #endregion
  
        void gv_RowCreated(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnUpdate = new ImageButton();
                //btnUpdate.Click += cmdShowDoc_Click;
                TableCell tc = new TableCell();

                tc.Controls.Add(btnUpdate);
                e.Row.Cells.Add(tc);
            }
        }
        protected void gvSiteOrderApprovals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {

                    string tx = e.Row.Cells[c_StatusDescription].Text;
                    string tp = e.Row.Cells[c_OrderTyp].Text;
                    //DateTime OrderDateEnd = datetime.Parse(e.Row.Cells[c_OrderDateEnd].Text));
                    string isApprovalCurrent = e.Row.Cells[c_isApprovalCurrent].Text;
                    int AssignedPeopleId = int.Parse(e.Row.Cells[c_AssignedPeopleId].Text);
                    int OrderId = int.Parse(e.Row.Cells[c_OrderId].Text.Substring(1));
                    if (tx != null)
                    {
                        if (tx == "Approved") ((CheckBox)e.Row.FindControl("Approve")).Checked = true;
                        if (tx == "Rejected") ((CheckBox)e.Row.FindControl("Reject")).Checked = true;

                    }
                    if (isApprovalCurrent != null)
                    {
                        if (isApprovalCurrent == "1")
                        {
                            int UserId = (int)Web.Utils.GetCurrentUserId();
                            if (UserId == AssignedPeopleId) 
                            {
                                ((CheckBox)e.Row.FindControl("Approve")).Enabled = true;
                                ((CheckBox)e.Row.FindControl("Reject")).Enabled = true;
                            }
                            else
                            {
                                ((CheckBox)e.Row.FindControl("Approve")).Enabled = false;
                                ((CheckBox)e.Row.FindControl("Reject")).Enabled = false;
                            }
                            GridViewRow row = e.Row;
                            String RowColor = e.Row.Cells[c_RowColor].Text;
                            if (RowColor != "")
                            {
                                if (RowColor == "Red")
                                {
                                    row.BackColor = System.Drawing.Color.Red;

                                }
                                else
                                {
                                    if (RowColor == "Orange") { row.BackColor = System.Drawing.Color.Orange; }
                                }
                            }
                            else
                            {
                               //  row.BackColor = System.Drawing.Color.LightSeaGreen;
                                row.BackColor = System.Drawing.Color.White;
                            }
                        }
                        if (isApprovalCurrent == "0") ((CheckBox)e.Row.FindControl("Approve")).Enabled = false;
                        if (isApprovalCurrent == "0") ((CheckBox)e.Row.FindControl("Reject")).Enabled = false;

                    }
                        
                }
                catch
                {

                }

            }
        }
        protected void chkApproveChanged(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
                parameterUserId = Request.Params["UserId"];
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterProjectId = Request.Params["ProjectId"];
                CheckBox chk = sender as CheckBox;
                if (chk.Checked == true)
                {

                    //Get the value passed from gridview
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);

                    SiteOrderInfo SOI = SiteOrdersController.GetSiteOrder(int.Parse(row.Cells[c_OrderId].Text.Substring(1)));
                    List<SiteOrderDetailInfo> SOD = SiteOrdersController.GetSiteOrderDetails(int.Parse(row.Cells[c_OrderId].Text.Substring(1)));//DS20240304
                    string[] Notes = (SOI.Notes).Split(Environment.NewLine.ToCharArray());   //DS20230307
                                                                                             
                    if (SOD.Count == 0 & Notes[0] == "")   //DS20240304
                    {
                        lblMessage.Text = "Error: Order has no line items or Order Description!";
                        return;
                    }





                    //if (SOI.SubTotal == 0)
                    //{
                    //    lblMessage.Text = "Error: Order has no value!";
                    //    return;
                    //}
                    //if (SOI.Typ == "Ins" && SOI.SubContractorId != 0 && SOI.VariationID == 0)
                    //{
                    //    lblMessage.Text = "Error: Variation Order Missing for Order";
                    //    return;
                    //}
                    string id = row.Cells[c_Id].Text; 
                //string yourvalue = cb1;
                SOA = new SiteOrderApprovalsSearchInfo();
                SOA.Id = int.Parse(id);
                SOA.OrderId = int.Parse(row.Cells[c_OrderId].Text.Substring(1));
                SOA.isApprovalCurrent = 0;
                SOA.Status = "A";
                    SOA.Reason = row.Cells[c_Reason].Text;
                SiteOrdersController.UpdateSiteOrderApprovalStatus(SOA);
                if (SOI.Typ == "Hir")
                    {
                        int rc = (int)SiteOrdersController.GetInstance().AddSiteOrderApprovalsHireProcess(SOI);
                    }
                else
                    {
                        int rc = (int)SiteOrdersController.GetInstance().AddSiteOrderApprovalsProcess(SOI);
                    }
                    // siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                if (parameterOrderId == null)
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx");
                    }
                else
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                    }
                }

            }

        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            parameterProjectId = Request.Params["ProjectId"];
            if (parameterOrderId == null)
            {
                Response.Redirect("~/Modules/Core/Home.aspx");
            }
            else
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            }
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void chkRejectChanged(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterProjectId = Request.Params["ProjectId"];
                CheckBox chk = sender as CheckBox;
                string parameter = Request["__EVENTARGUMENT"];
                if (chk.Checked == true)
                {

                    //Get the value passed from gridview
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);

                    //GridViewRow row = chk.NamingContainer;
                    //int index = row.RowIndex;

                    //string cb1 = row.Cells[0].Text;
                    //string id = (string)System.Web.UI.DataBinder.Eval(row, "Title");
                    string id = row.Cells[c_Id].Text;
                    //string yourvalue = cb1;
                    SOA = new SiteOrderApprovalsSearchInfo();
                    SOA.Id = int.Parse(id);
                    SOA.OrderId = int.Parse(row.Cells[c_OrderId].Text.Substring(1));
                    SOA.isApprovalCurrent = 1;
                    SOA.Status = "R";
                    SOA.Reason = parameter;
                    SiteOrdersController.UpdateSiteOrderApprovalStatus(SOA);
                    // siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                    if (parameterOrderId == null)
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);

                    }
                }
                else
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    string id = row.Cells[c_Id].Text;
                    //string yourvalue = cb1;
                    SOA = new SiteOrderApprovalsSearchInfo();
                    SOA.Id = int.Parse(id);
                    SOA.OrderId = int.Parse(row.Cells[c_OrderId].Text.Substring(1));
                    SOA.isApprovalCurrent = 1;
                    SOA.Status = "";
                    SOA.Reason = "";
                    SiteOrdersController.UpdateSiteOrderApprovalStatus(SOA);
                    // siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                    if (parameterOrderId == null)
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);

                    }

                }
                //scree
                //SiteOrdersController.UpdateSiteOrderApprovalStatus()
                //siteOrdersController.(Int32.Parse(parameterOrderId)); cmdPrintOrder_Click

            }


        }
        protected void cmdViewOrder_Click(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            LinkButton lnkbtn = sender as LinkButton;
            //   if (projectInfo.Id == null)
            if (!Page.IsPostBack)
            {
                parameterProjectId = Request.Params["ProjectId"];
            }
            string arguments = lnkbtn.CommandArgument.ToString();
            string[] args = arguments.Split(';');
            string OrderId = args[2];
            string ProjectId = args[1];
            Session["SiteOrderNav"] = "EditSiteOrderApprovalsAll.aspx";

            SiteOrderInfo SOI = SiteOrdersController.GetSiteOrder(int.Parse(OrderId));
            if (SOI.Typ == "Mat") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + ProjectId + "&OrderId=" + OrderId + "&OrderTyp=Mat");
            if (SOI.Typ == "Ins") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + ProjectId + "&OrderId=" + OrderId + "&OrderTyp=Ins");
            if (SOI.Typ == "Hir") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderHire.aspx?ProjectId=" + ProjectId + "&OrderId=" + OrderId + "&OrderTyp=Hir");
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdPrintOrder_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = sender as LinkButton;
            string arguments = lnkbtn.CommandArgument.ToString();
            string[] args = arguments.Split(';');
            string OrderId = args[0];
            //Response.Redirect($"~/Modules/SiteOrders/ShowSiteOrderPage.aspx?OrderId={OrderId}&Docs=ALL");
            SiteOrdersController siteOrdersController = SiteOrdersController.GetInstance();
            //SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(OrderId));
            SOI = siteOrdersController.GetSiteOrder(Int32.Parse(OrderId));
            SOI.Docs = siteOrdersController.GetSiteOrderDocs(Int32.Parse(OrderId));
            string result = SOS.UI.Utils.showSiteOrderDocs(this, SOI.ProjectId.ToString(), OrderId, SOI.Docs);
        }
        private int GetColumnIndexByName(GridViewRow row, string v)
        {
            throw new NotImplementedException();
        }
   
    }
}
