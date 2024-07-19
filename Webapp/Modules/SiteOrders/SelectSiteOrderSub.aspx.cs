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
    public partial class SelectSiteOrderSub : SOSPage  //System.Web.UI.Page
    {
        private SiteOrderInfo SiteOrderInfo = null;

        #region Public properties

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;
            return currentNode;
        }
#endregion

#region Private Methods

        #endregion


        #region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            lblMessage.Text = "Enter Site Order and Click View";
            lblMessage.ForeColor = Color.DarkBlue;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            //Int32.Parse(parameterProjectId)
            try
            {
               // Security.CheckAccess(Security.userActions.SelectSiteOrderSub);
                Security.CheckAccess(Security.userActions.EditSiteOrderApprovals);
                if (!Page.IsPostBack)
                {
                 }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }


         #endregion
 
        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
                 SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(txtOrderId.Text));
                if (SiteOrderInfo == null)
                {
                    lblMessage.Text = "Site Order not found!";
                    lblMessage.ForeColor = Color.DarkBlue;
                }
                else
                {
                    int UserId = (int)Web.Utils.GetCurrentUserId();
                    if (SiteOrderInfo.SubContractorId != UserId && SiteOrderInfo.ForemanID != UserId)
                    {
                        lblMessage.Text = "Site Order not linked to you as Forman / SubContractor!";
                        lblMessage.ForeColor = Color.Red;
                    }
                    else
                    {
                    switch (SiteOrderInfo.Typ)
                    {
                        case "Mat":
                            Response.Redirect("~/Modules/SiteOrders/ViewSiteOrder.aspx?ProjectId=" + SiteOrderInfo.ProjectId.ToString() + "&OrderId=" + SiteOrderInfo.IdStr + "&OrderTyp=" + SiteOrderInfo.Typ + "&ReadOnly=1");
                            break;
                        case "Ins":
                            Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderInst.aspx?ProjectId=" + SiteOrderInfo.ProjectId.ToString() + "&OrderId=" + SiteOrderInfo.IdStr + "&OrderTyp=" + SiteOrderInfo.Typ + "&ReadOnly=1");
                            break;
                        case "Hir":
                            Response.Redirect("~/Modules/SiteOrders/ViewSiteOrderHire.aspx?ProjectId=" + SiteOrderInfo.ProjectId.ToString() + "&OrderId=" + SiteOrderInfo.IdStr + "&OrderTyp=" + SiteOrderInfo.Typ + "&ReadOnly=1");
                            break;
                        default:
                        break;
                    }

                }
            }

            //
        }
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Core/Home.aspx");
        }
 

    }


}
