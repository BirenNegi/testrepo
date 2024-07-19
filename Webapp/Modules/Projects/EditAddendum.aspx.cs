using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditAddendumPage : SOSPage
    {

#region Members
        private AddendumInfo addendumInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (addendumInfo == null)
                return null;

            tempNode.ParentNode.Title = addendumInfo.Trade.Name;
            tempNode.ParentNode.Url += "?TradeId=" + addendumInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = addendumInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + addendumInfo.Trade.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (addendumInfo.Id == null)
            {
                TitleBar.Title = "Adding Trade Addendum";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }

            lblNumber.Text = UI.Utils.SetFormInteger(addendumInfo.Number);
            txtName.Text = UI.Utils.SetFormString(addendumInfo.Name);
            sdrAddendumDate.Date = addendumInfo.AddendumDate;
            txtDescription.Text = UI.Utils.SetFormString(addendumInfo.Description);
        }

        private void FormToObjects()
        {
            addendumInfo.Name = UI.Utils.GetFormString(txtName.Text);
            addendumInfo.AddendumDate = sdrAddendumDate.Date;
            addendumInfo.Description = UI.Utils.GetFormString(txtDescription.Text);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterAddendumId;

            try
            {
                Security.CheckAccess(Security.userActions.EditAddendum);
                parameterAddendumId = Request.Params["AddendumId"];

                if (parameterAddendumId == null)
                {
                    String parameterTradeId = Utils.CheckParameter("TradeId");
                    TradeInfo tradeInfo = tradesController.GetTradeWithAddendums(Int32.Parse(parameterTradeId));
                    Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");
                    addendumInfo = tradesController.InitializeAddendum(tradeInfo);
                }
                else
                {
                    addendumInfo = tradesController.GetAddendum(Int32.Parse(parameterAddendumId));
                    Core.Utils.CheckNullObject(addendumInfo, parameterAddendumId, "Trade Addendum");
                }

                processController.CheckEditCurrentUser(addendumInfo);

                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    addendumInfo.Id = TradesController.GetInstance().AddUpdateAddendum(addendumInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewAddendum.aspx?AddendumId=" + addendumInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (addendumInfo.Id == null)
                Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + addendumInfo.Trade.IdStr);
            else
                Response.Redirect("~/Modules/Projects/ViewAddendum.aspx?AddendumId=" + addendumInfo.IdStr);
        }
#endregion

    }
}