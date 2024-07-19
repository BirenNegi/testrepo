using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewDrawingPage : SOSPage
    {

#region Members
        private DrawingInfo drawingInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (drawingInfo == null)
                return null;

            tempNode.ParentNode.Title = drawingInfo.DrawingType.Name + " Drawings";
            tempNode.ParentNode.Url += "?Type=" + drawingInfo.Type + "&ProjectId=" + drawingInfo.Project.IdStr + "&DrawingTypeId=" + drawingInfo.DrawingType.IdStr;

            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + drawingInfo.Project.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = drawingInfo.Project.Name + (drawingInfo.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + drawingInfo.Project.IdStr;

            return currentNode;
        }

        private void BindRevisions()
        {
            gvRevisions.DataSource = drawingInfo.DrawingRevisions;
            gvRevisions.DataBind();
        }

        private void BindTransmittals()
        {
            gvTransmittals.DataSource = drawingInfo.Transmittals;
            gvTransmittals.DataBind();
        }

        private void BindDrawing()
        {
            if (drawingInfo.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            lnkAddRevision.NavigateUrl = "~/Modules/Projects/EditDrawingRevision.aspx?DrawingId=" + drawingInfo.IdStr;

            lblName.Text = UI.Utils.SetFormString(drawingInfo.Name);
            lblType.Text = UI.Utils.SetFormString(drawingInfo.DrawingType.Name);
            lblDescription.Text = UI.Utils.SetFormString(drawingInfo.Description);

            foreach (TradeInfo tradeInfo in drawingInfo.Trades)
                tvTrades.Nodes[0].ChildNodes.Add(new TreeNode(tradeInfo.Name, "", "", "~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeInfo.IdStr, ""));

            BindRevisions();
            BindTransmittals();
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterDrawingId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewDrawing);
                parameterDrawingId = Utils.CheckParameter("DrawingId");
                drawingInfo = tradesController.GetDeepDrawingWithTransmittalsAndTrades(Int32.Parse(parameterDrawingId));
                Core.Utils.CheckNullObject(drawingInfo, parameterDrawingId, "Drawing");
                drawingInfo.Project = projectsController.GetProject(drawingInfo.Project.Id);

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditDrawing))
                    {
                        if (processController.AllowUpdateDrawingsCurrentUser(drawingInfo.Project))
                        {
                            pnlEdit.Visible = true;
                            phAddRevision.Visible = true;

                            cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Drawing and All its Revisions?');");
                        }
                    }
                    BindDrawing();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditDrawing.aspx?DrawingId=" + drawingInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteDrawing(drawingInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect(String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", drawingInfo.Type, drawingInfo.Project.IdStr, drawingInfo.DrawingType.IdStr));
        }
#endregion
    
    }
}
