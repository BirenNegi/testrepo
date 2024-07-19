using System;
using System.Web.UI.WebControls;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class SearchSiteOrdersUser : SOSPage
    {
        //public partial class SearchSiteOrders : System.Web.UI.Page
        private SiteOrderInfo SiteOrderInfo = null;
        #region Public properties
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.ParentNode.Title = "My Site Orders";
           
            return currentNode;
        }
        protected String GvSiteOrdersSortExpression
        {
            get { return (String)ViewState["GvSiteOrdersSortExpression"]; }
            set { ViewState["GvSiteOrdersSortExpression"] = value; }
        }

        protected SortDirection GvSiteOrdersSortDireccion
        {
            get { return (SortDirection)ViewState["GvSiteOrdersSortDirection"]; }
            set { ViewState["GvSiteOrdersSortDirection"] = value; }
        }
        protected void chkHistory_Click(object sender, EventArgs e)
        {
            gvSiteOrders.PageIndex = 0;
        }
        #endregion

        #region Members
        private ProjectInfo projectInfo;
        #endregion

        #region Private Methods
        private void BindSearch()
        {
            ddlSiteOrderType.Items.Add(new ListItem("All", String.Empty));
            ddlSiteOrderType.Items.Add(new ListItem("Mat", "Mat"));
            ddlSiteOrderType.Items.Add(new ListItem("Ins", "Ins"));
            ddlSiteOrderType.Items.Add(new ListItem("Hir", "Hir"));

            GvSiteOrdersSortExpression = "Name";
            GvSiteOrdersSortDireccion = SortDirection.Ascending;
        }
        #endregion
        String parameterProjectId;
        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            try
            {
                Security.CheckAccess(Security.userActions.SearchSiteOrders);
                gvSiteOrders.Columns[2].Visible = false;
                gvSiteOrders.Columns[5].Visible = false;
                gvSiteOrders.Columns[6].Visible = false;
                gvSiteOrders.Columns[7].Visible = false;
                //parameterProjectId = Request.Params["ProjectId"];
                //projectInfo = projectsController.GetProjectWithClaimsDeepTradesAndVariations(Int32.Parse(parameterProjectId));

                //lnkAddNew.NavigateUrl = "~/Modules/SiteOrders/EditSiteOrder.aspx?ProJectId=" + parameterProjectId + "&OrderTyp=Mat";
                int UserId = (int)Web.Utils.GetCurrentUserId();
                int ApprovalsCnt = SiteOrdersController.SearchSiteOrderApprovalsCount(UserId);
                lblApprovals.Text = "my Approvals(" + ApprovalsCnt.ToString() + ")";
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

        protected void odsSiteOrders_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SiteOrdersController.GetInstance();
        }

        protected void ddlSiteOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSiteOrders.PageIndex = 0;
        }
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Core/Home.aspx");
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            gvSiteOrders.PageIndex = 0;
        }
        protected void gvSiteOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridViewRow row = e.Row;
                SiteOrderInfo SIO = (SiteOrderInfo)e.Row.DataItem;
                if (SIO != null)
                {
                    if (SIO.RowColor != "")
                    {
                        if (SIO.RowColor == "Red")
                        {
                            row.BackColor = System.Drawing.Color.Red;

                        }
                        else
                        {
                            if (SIO.RowColor == "Orange") { row.BackColor = System.Drawing.Color.Orange; }
                        }
                    }
                    else
                    {
                        //  row.BackColor = System.Drawing.Color.LightSeaGreen;
                        row.BackColor = System.Drawing.Color.White;
                    }
                }
            }
            catch
            {

            }

        }
        protected void gvSiteOrders_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvSiteOrders.PageIndex = 0;

            if (GvSiteOrdersSortExpression == e.SortExpression)
                GvSiteOrdersSortDireccion = Utils.ChangeSortDirection(GvSiteOrdersSortDireccion);
            else
            {
                GvSiteOrdersSortExpression = e.SortExpression;
                GvSiteOrdersSortDireccion = SortDirection.Ascending;
            }

            e.SortDirection = GvSiteOrdersSortDireccion;
        }

        protected void gvSiteOrders_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvSiteOrders, e, GvSiteOrdersSortExpression, GvSiteOrdersSortDireccion);
        }
        //cmdApprovals_Click
        protected void cmdViewOrder_Click(object sender, EventArgs e)
        {
            LinkButton lnkbtn = sender as LinkButton;
            //   if (projectInfo.Id == null)
            if (!Page.IsPostBack)
            {
                parameterProjectId = Request.Params["ProjectId"];
            }
            string arguments = lnkbtn.CommandArgument.ToString();
            string[] args = arguments.Split(';');
            string OrderId = args[0];
            string OrderTyp = args[1];
            string ProjectId = args[2];
            Session["SiteOrderNav"] = "SearchSiteOrdersUser.aspx";
            if (OrderTyp == "Mat") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + ProjectId + "&OrderId=" + OrderId + "&OrderTyp=Mat");
            if (OrderTyp == "Ins") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + ProjectId + "&OrderId=" + OrderId + "&OrderTyp=Ins");
            if (OrderTyp == "Hir") Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderHire.aspx?ProjectId=" + ProjectId + "&OrderId=" + OrderId + "&OrderTyp=Hir");
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
 
        protected void cmdApprovals_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            int UserId = (int)Web.Utils.GetCurrentUserId();
            Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx?UserId=" + UserId.ToString() + "&OrderTyp=Mat");
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
            SiteOrderInfo = siteOrdersController.GetSiteOrder(Int32.Parse(OrderId));
            SiteOrderInfo.Docs = siteOrdersController.GetSiteOrderDocs(Int32.Parse(OrderId));

            if (SiteOrderInfo.Docs.Count == 0)   // DS20231031
            {
                lblMessage.Text = "  -  No documents found for order!";

            }
            else
            {
                string result = SOS.UI.Utils.showSiteOrderDocs(this, SiteOrderInfo.ProjectId.ToString(), OrderId, SiteOrderInfo.Docs);

            }
        }

        #endregion

    }
}
