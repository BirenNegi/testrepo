using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class EditSiteOrderDetailInst : SOSPage  //System.Web.UI.Page
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
 
            GvSiteOrdersSortExpression = "Name";
            GvSiteOrdersSortDireccion = SortDirection.Ascending;
        }
        #endregion

        
        protected void Page_Init(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            parameterProjectId = Request.Params["ProjectId"];
            parameterOrderId = Request.Params["OrderId"];
            parameterItemId = Request.Params["ItemId"];
            lblMessage.ForeColor = System.Drawing.Color.Blue;

            ProjectInfo = ProjectsController.GetProject(Int32.Parse(parameterProjectId));
            TitleBar.Title = "Site Instruction Item -  D" + Int32.Parse(parameterOrderId).ToString("000000");
            if (parameterItemId == null)
            {
                 SOD = SiteOrdersController.InitializeSiteOrderDetail(Int32.Parse(parameterOrderId));
               // cmdUpdateBottom.Text = "Add";
               // cmdUpdateTop.Text = "Add";
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
            //SOD.Qty = 1;
            //SOD.UM = "EA";
            //SOD.Qty = 1;
            //SOD.Price = (decimal)UI.Utils.GetFormDecimal(txtAmount.Text);
            //SOD.Amount = (decimal)UI.Utils.GetFormDecimal(txtAmount.Text);
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
            //SOD.Amount = SOD.Qty * SOD.Price;
            //txtAmount.Text = UI.Utils.SetFormEditDecimal(SOD.Amount);

            txtQuantity.Text = UI.Utils.SetFormEditDecimal(SOD.Qty);
            txtUM.Text = SOD.UM;
            txtPrice.Text = UI.Utils.SetFormEditDecimal(SOD.Price);
            SOD.Amount = SOD.Qty * SOD.Price;
            txtAmount.Text = UI.Utils.SetFormEditDecimal(SOD.Amount);
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            SaveItem(sender, e, false);
        }
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            SaveItem(sender, e, true);
        }
        protected void SaveItem(object sender, EventArgs e, bool isExit) 
            {
                //   if (projectInfo.Id == null)

                if (Page.IsValid)
                {
                parameterProjectId = Request.Params["ProjectId"];
                parameterOrderId = Request.Params["OrderId"];
                parameterOrderTyp = Request.Params["OrderTyp"];
                parameterItemId = Request.Params["ItemId"];
                decimal dd;
                if (decimal.TryParse(txtAmount.Text, out dd) == false)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Amount is not a number!";
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

 
                if (parameterItemId == null)
                    {
                        SOD.OrderID = (int)UI.Utils.GetFormInteger(parameterOrderId);
                        SOD.Id = SiteOrdersController.GetInstance().AddSiteOrderDetail(SOD);
                        parameterItemId = SOD.IdStr;

                    if (isExit == true)
                    {
                        //
                        Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    }
                    else
                    {
                        Response.Redirect("~/Modules/SiteOrders/EditSiteOrderDetailInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    }
                }
                else
                    {
                    SOD.OrderID = (int)UI.Utils.GetFormInteger(parameterOrderId);
                    SOD.Id = (int)UI.Utils.GetFormInteger(parameterItemId);
                    SiteOrdersController.GetInstance().AddUpdateSiteOrderDetail(SOD);
                    // DS20221122   Response.Redirect("~/Modules/SiteOrders/SearchSiteOrderDetailInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
                    Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);

                }

            }
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
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
            }

        }
        protected void cmdCancel_Click(object sender, EventArgs e)
            {
            parameterOrderId = Request.Params["OrderId"];
            parameterOrderTyp = Request.Params["OrderTyp"];
            if (parameterItemId == null)
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
            }
            else
            {
                Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?OrderId=" + parameterOrderId + "&ProjectId=" + parameterProjectId + "&OrderTyp=" + parameterOrderTyp);
            }
                
             }

       }
}
