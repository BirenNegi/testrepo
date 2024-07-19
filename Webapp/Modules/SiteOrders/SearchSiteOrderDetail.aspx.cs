using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;
using System.IO;

namespace SOS.Web
{
    public partial class SearchSiteOrderDetail : SOSPage  //System.Web.UI.Page
    {
        private SiteOrderInfo SiteOrderInfo = null;
        private ProjectInfo ProjectInfo = null;
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

 
#endregion

#region Private Methods
        private void BindSearch()
        {
            gvSiteOrderItems.DataSource = SiteOrderInfo.Items;
            gvSiteOrderItems.DataBind();
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
           ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
           SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterOrderId));
           SiteOrderInfo.Items = SiteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterOrderId));
            TitleBar1.Title = "View Material Order Items - " + parameterOrderId;
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
                Security.CheckAccess(Security.userActions.SearchSiteOrderDetail);
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

        protected void odsSiteOrderDetail_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SiteOrdersController.GetInstance();
        }

        protected void ddlSiteOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSiteOrderItems.PageIndex = 0;
        }

        //protected void cmdSearch_Click(object sender, EventArgs e)
        //{
        //    gvSiteOrderDocs.PageIndex = 0;
        //}

          protected void gvSiteOrderDetails_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
        }


        #endregion
        private void FormToObjects()
        {
 
        }
        void gv_RowCreated(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnUpdate = new ImageButton();
                btnUpdate.Click += cmdEditItem_Click;
                TableCell tc = new TableCell();

                tc.Controls.Add(btnUpdate);
                e.Row.Cells.Add(tc);
            }
        }
        protected void gvSiteOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
 
                LinkButton lnk = e.Row.FindControl("ShowDetail") as LinkButton;
                lnk.Click += new EventHandler(cmdEditItem_Click);
                ImageButton btnUpdate = (ImageButton)e.Row.FindControl("EditDetail");
                btnUpdate.ID = "btnEditDetail";
                btnUpdate.ImageUrl = "~/Images/IconView.gif";
                btnUpdate.ToolTip = "View";
                btnUpdate.CommandName = "ShowDoc";

            }
        }
        protected void cmdEditItem_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;

                //Get the value passed from gridview
                string arguments = lnkbtn.CommandArgument.ToString();
                string[] args = arguments.Split(';');
                string ItemId = args[0];
                string parameterOrderId = Request.Params["OrderId"];
                string parameterOrderTyp = Request.Params["OrderTyp"];
                string parameterProjectId = Request.Params["ProjectId"];
                if (Page.IsValid)
                {
                    Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetail.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp + "&ItemId=" + ItemId);
                }
            }

        }
        protected void cmdDeleteItem_Click(object sender, EventArgs e)
        {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            parameterProjectId = Request.Params["ProjectId"];
            if (Page.IsValid)
            {
                LinkButton lnkbtn = sender as LinkButton;

                //Get the value passed from gridview
                string arguments = lnkbtn.CommandArgument.ToString();
                string[] args = arguments.Split(';');
                string ItemId = args[0];
                string parameterOrderId = Request.Params["OrderId"];
                if (Page.IsValid)
                {
                    int Result = (int)SiteOrdersController.GetInstance().DeleteSiteOrderDetail((int)UI.Utils.GetFormInteger(ItemId), (int)UI.Utils.GetFormInteger(parameterOrderId));
                  
                    Response.Redirect("~/Modules/SiteOrders/SearchSiteOrderDetail.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
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
                Response.Redirect("~/Modules/SiteOrders/SearchSiteOrders.aspx?ProjectId=" + parameterProjectId);
            }
            else
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            }
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdAddItem_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["parameterOrderTyp"];
                parameterProjectId = Request.Params["ProjectId"];
                Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetail.aspx?ProjectId=" + parameterProjectId + "&OrderId=" + parameterOrderId + "&OrderTyp=" + parameterOrderTyp);
            }
        }
        
    }
}
