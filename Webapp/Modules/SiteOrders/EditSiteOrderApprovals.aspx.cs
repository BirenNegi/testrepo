using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;
using System.IO;
using System.Drawing;
using Microsoft.Reporting.WebForms;
namespace SOS.Web
{
    public partial class EditSiteOrderApprovals : SOSPage  //System.Web.UI.Page
    {
        private SiteOrderInfo SOI = null;
        private ProjectInfo ProjectInfo = null;
        private String parameterProjectId;
        private String parameterOrderTyp;
        private String parameterOrderId;
        private List<SiteOrderApprovalsSearchInfo> siteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsSearchInfo>();
        private SiteOrderApprovalsSearchInfo SOA = null;
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
           gvSiteOrderApprovals.Columns[8 + 1].Visible = false;
           gvSiteOrderApprovals.Columns[9 + 1].Visible = false;
           gvSiteOrderApprovals.Columns[10 + 1].Visible = false;
          //  gvSiteOrderApprovals.Columns[12].Visible = false;
          //  gvSiteOrderApprovals.Columns[13].Visible = false;
        }
        #endregion


        #region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
    {
            string rsn = reason1.Text;
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
        ProjectsController ProjectsController = ProjectsController.GetInstance();
        parameterProjectId = Request.Params["ProjectId"];
        parameterOrderId = Request.Params["OrderId"];
        parameterOrderTyp = Request.Params["OrderTyp"];
        ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
        SOI = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterOrderId));
            if (parameterOrderTyp == "Mat") OrderTyp.Value = "MO";
            if (parameterOrderTyp == "Hir") OrderTyp.Value = "EH";
            if (parameterOrderTyp == "Ins") OrderTyp.Value = "SI";
            Limit.Value = "";
            if (SOI.SubTotal > 3000) Limit.Value = "Y";
            if (!Page.IsPostBack)
   {

                siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
            }
            if (parameterOrderTyp == "Mat") TitleBar1.Title = "Edit Material Order Approvals - D" + Int32.Parse(parameterOrderId).ToString("000000"); ;
        if (parameterOrderTyp == "Hir") TitleBar1.Title = "Equipment Hire Approvals - D" + Int32.Parse(parameterOrderId).ToString("000000"); ;
        if (parameterOrderTyp == "Ins") TitleBar1.Title = "Site Instruction Approvals - D" + Int32.Parse(parameterOrderId).ToString("000000"); ;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            parameterProjectId = Request.Params["ProjectId"];
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            string rsn = HiddenReason.Value;
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
 
                 try
                {
                    
                    //int index = GetColumnIndexByName(e.Row, "myDataField");
                    string tx = e.Row.Cells[5].Text;
                    string isApprovalCurrent = e.Row.Cells[8 + 1].Text;
                    int AssignedPeopleId = int.Parse(e.Row.Cells[10 + 1].Text);
                    if (tx != null)
                    {
                        if (tx == "Approved") ((CheckBox)e.Row.FindControl("Approve")).Checked = true;
                        if (tx == "Rejected") ((CheckBox)e.Row.FindControl("Reject")).Checked = true;

                    }
                    if (isApprovalCurrent != null)
                    {
                        if (isApprovalCurrent == "1" || tx == "Rejected") 
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
                            row.BackColor = Color.LightSeaGreen;
                        }
                        if (isApprovalCurrent == "0")
                        {
                            if (tx != "Rejected") ((CheckBox)e.Row.FindControl("Approve")).Enabled = false;
                        }
                        if (isApprovalCurrent == "0")
                        {
                            if (tx != "Rejected") ((CheckBox)e.Row.FindControl("Reject")).Enabled = false;
                        }

                    }
                        
                }
                catch
                {


                }

        }
        protected void chkApproveChanged(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterProjectId = Request.Params["ProjectId"];
                CheckBox chk = sender as CheckBox;
                if (chk.Checked == true)
                    {

                //Get the value passed from gridview
                 GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    SiteOrderInfo SOI = SiteOrdersController.GetSiteOrder(int.Parse(parameterOrderId));
                    List<SiteOrderDetailInfo> SOD = SiteOrdersController.GetSiteOrderDetails(int.Parse(parameterOrderId));//DS20240304
                    string[] Notes = (SOI.Notes).Split(Environment.NewLine.ToCharArray());   //DS20230307
                    //if (SOI.SubTotal == 0)
                    //{
                    //    lblMessage.Text = "Error: Order has no value!";
                    //    return;
                    //}
//                    if (SOI.ItemsTotal == 0 & Notes[0] == "")  // DS20240304
                    if (SOD.Count == 0 & Notes[0] == "")   //DS20240304
                        {
                            lblMessage.Text = "Error: Order has no line items or Order Description!";
                        return;
                    }
//                    if (SOI.Typ == "Ins" && SOI.SubContractorId != 0 && SOI.VariationID == 0)
//                    {
//                        lblMessage.Text = "Error: Variation Order Missing for Order!";
 //                       return;
 //                   }

                    string id = row.Cells[9 + 1].Text;
                //string yourvalue = cb1;
                SOA = new SiteOrderApprovalsSearchInfo();
                SOA.Id = int.Parse(id);
                SOA.OrderId = int.Parse(parameterOrderId);
                SOA.isApprovalCurrent = 0;
                SOA.Status = "A";
                SiteOrdersController.UpdateSiteOrderApprovalStatus(SOA);
                    // siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                    if (SOI.Typ == "Hir")
                    {
                        int rc = (int)SiteOrdersController.GetInstance().AddSiteOrderApprovalsHireProcess(SOI);
                    }
                    else
                    {
                        int rc = (int)SiteOrdersController.GetInstance().AddSiteOrderApprovalsProcess(SOI);
                    }
                    Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovals.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);

                }
                //scree
                //SiteOrdersController.UpdateSiteOrderApprovalStatus()
                //siteOrdersController.(Int32.Parse(parameterOrderId));
            }

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
                if (parameterOrderTyp == "Mat") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                if (parameterOrderTyp == "Hir") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderHire.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
                if (parameterOrderTyp == "Ins") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
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
                string rsn = reason1.Text;
                string parameter = Request["__EVENTARGUMENT"];

                if (chk.Checked == true)
                {
                   // string parm = e.ToString;
                    //Get the value passed from gridview
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    //GridViewRow row = chk.NamingContainer;
                    //int index = row.RowIndex;

                    //string cb1 = row.Cells[0].Text;
                    //string id = (string)System.Web.UI.DataBinder.Eval(row, "Title");
                    string id = row.Cells[9 + 1].Text;
                    string rr = reason1.Text;
                    //string yourvalue = cb1;
                    SOA = new SiteOrderApprovalsSearchInfo();
                    SOA.Id = int.Parse(id);
                    SOA.OrderId = int.Parse(parameterOrderId);
                    SOA.isApprovalCurrent = 1;
                    SOA.Status = "R";
                    SOA.Reason = parameter;
                    SiteOrdersController.UpdateSiteOrderApprovalStatus(SOA);
                    // siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                    Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovals.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);

                }
                else
                {

                    //Get the value passed from gridview
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    //GridViewRow row = chk.NamingContainer;
                    //int index = row.RowIndex;

                    //string cb1 = row.Cells[0].Text;
                    //string id = (string)System.Web.UI.DataBinder.Eval(row, "Title");
                    string id = row.Cells[9 + 1].Text;
                    //string yourvalue = cb1;
                    SOA = new SiteOrderApprovalsSearchInfo();
                    SOA.Id = int.Parse(id);
                    SOA.OrderId = int.Parse(parameterOrderId);
                    SOA.isApprovalCurrent = 1;
                    SOA.Status = "";
                    SiteOrdersController.UpdateSiteOrderApprovalStatus(SOA);
                    // siteOrderApprovalsSearchInfoList = SiteOrdersController.SearchSiteOrderApprovals(Int32.Parse(parameterOrderId));
                    Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovals.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);

                }
                //scree
                //SiteOrdersController.UpdateSiteOrderApprovalStatus()
                //siteOrdersController.(Int32.Parse(parameterOrderId));
            }

        }
        private int GetColumnIndexByName(GridViewRow row, string v)
        {
            throw new NotImplementedException();
        }



    }
}
