using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class EditSiteOrderDetail : SOSPage  //System.Web.UI.Page
    {
        private SiteOrderDetailInfo SOD = null;
        private SiteOrderInfo SOI = null;
        private ProjectInfo ProjectInfo = null;
        private String parameterProjectId;
        private String parameterOrderId;
        private String parameterOrderTyp;
        private String parameterItemId;
        #region Public properties
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
        #endregion

        #region Private Methods
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
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + ProjectInfo.IdStr;

            return currentNode;
        }
        private void BindSearch()
        {
      //      ddlSiteOrderType.Items.Add(new ListItem("All", String.Empty));
      //      ddlSiteOrderType.Items.Add(new ListItem("Mat", "Mat"));
      //      ddlSiteOrderType.Items.Add(new ListItem("Inst", "Inst"));
      //      ddlSiteOrderType.Items.Add(new ListItem("Hire", "Hire"));

            GvSiteOrdersSortExpression = "Name";
            GvSiteOrdersSortDireccion = SortDirection.Ascending;
        }
        #endregion

        
        protected void Page_Init(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            lblMessage.ForeColor = System.Drawing.Color.Blue;

            parameterProjectId = Request.Params["ProjectId"];
            parameterOrderId = Request.Params["OrderId"];
            parameterItemId = Request.Params["ItemId"];
            ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
            TitleBar.Title = "Material Order Item - D" + Int32.Parse(parameterOrderId).ToString("000000"); ;
            if (parameterItemId == null)
            {
                SOD = SiteOrdersController.InitializeSiteOrderDetail(Int32.Parse(parameterOrderId));
                cmdUpdateBottom.Text = "Save & Exit";
                cmdUpdateTop.Text = "Save & Exit";
                cmdDeleteBottom.Visible = false;
                cmdDeleteTop.Visible = false;
            }
            else
            {
                SOD = SiteOrdersController.GetSiteOrderDetail(Int32.Parse(parameterItemId));
            }
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            try
            {
                Security.CheckAccess(Security.userActions.SearchSiteOrders);

                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                    SOI = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterOrderId));
                    SOI.Items = SiteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterOrderId));
                    gvSiteOrderItems.DataSource = SOI.Items;
                    gvSiteOrderItems.DataBind();
                }
                else
                {
                }
                        
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        private void FormToObjects()
        {
            // = (int)UI.Utils.GetFormInteger(parameterProjectId);
            SOD = new SiteOrderDetailInfo();
            SOD.Title = txtTitle.Text;
            SOD.Qty = (decimal)UI.Utils.GetFormDecimal(txtQuantity.Text);
            SOD.UM = txtUM.Text;
            SOD.Price = (decimal)UI.Utils.GetFormDecimal(txtPrice.Text);
            SOD.Amount = SOD.Qty * SOD.Price;
            SOD.Date = DateTime.Today;
            SOD.ETA = DateTime.Today;
            SOD.Status = "NEW";
        }
        private void ObjectsToForm()
        {
            if (SOD.Id == null)
            {
                TitleBar.Title = "Adding Site Order";
                cmdUpdateTop.Text = "Save & Exit";
                cmdUpdateBottom.Text = "Save & Exit";
            }
            txtTitle.Text = SOD.Title;
            txtQuantity.Text = UI.Utils.SetFormEditDecimal(SOD.Qty);
            txtUM.Text = SOD.UM;
            txtPrice.Text = UI.Utils.SetFormEditDecimal(SOD.Price);
            SOD.Amount = SOD.Qty * SOD.Price;
            txtAmount.Text = UI.Utils.SetFormEditDecimal(SOD.Amount);
        }
        protected void saveItem(object sender, EventArgs e, bool isExit)
        {
            if (Page.IsValid)
            {
                parameterProjectId = Request.Params["ProjectId"];
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterItemId = Request.Params["ItemId"];
                decimal dd;
                if (decimal.TryParse(txtQuantity.Text, out dd) == false)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Quantity is not a number!";
                    return;
                }
                if (decimal.TryParse(txtPrice.Text, out dd) == false)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Price is not a number!";
                    return;
                }


                if (txtTitle.Text == "")
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Description is Mandatory!";
                    return;
                }
                FormToObjects();
                //if (SOD.Amount == 0)
                //{
                //    lblMessage.ForeColor = System.Drawing.Color.Red;
                //    lblMessage.Text = "Amount may not be ZERO!";
                //    return;
                //}
                if (SOD.Qty == 0)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Quantity may not be ZERO!";
                    return;
                }
 
                if (parameterItemId == null)
                {
                    SOD.OrderID = (int)UI.Utils.GetFormInteger(parameterOrderId);
                    SOD.Id = SiteOrdersController.GetInstance().AddSiteOrderDetail(SOD);
                    parameterItemId = SOD.IdStr;
                    if (isExit == true)
                    {

                        //
                        Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    }
                    else
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetail.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    }
                }
                else
                {
                    SOD.OrderID = (int)UI.Utils.GetFormInteger(parameterOrderId);
                    SOD.Id = (int)UI.Utils.GetFormInteger(parameterItemId);
                    SiteOrdersController.GetInstance().AddUpdateSiteOrderDetail(SOD);
                    //  DS20221122 Response.Redirect("~/Modules/SiteOrders/SearchSiteOrderDetail.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    if (isExit == true)
                    {
                        Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    }
                    else
                    {

                    }
                }

            }
        }
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            saveItem(sender, e, true);
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            saveItem(sender, e, false);
        }
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
                parameterProjectId = Request.Params["ProjectId"];
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterItemId = Request.Params["ItemId"];
                FormToObjects();
                if (parameterItemId != null)
                {
                SOD.Id = (int)UI.Utils.GetFormInteger(parameterItemId);
                SOD.OrderID = (int)UI.Utils.GetFormInteger(parameterOrderId);
                SiteOrdersController.GetInstance().DeleteSiteOrderDetail((int)SOD.Id, SOD.OrderID);
                parameterItemId = SOD.IdStr;
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                }

        }
        protected void cmdCancel_Click(object sender, EventArgs e)
            {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];

            if (parameterItemId == null)
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
            }
            else
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
            }
            }

       }
}
