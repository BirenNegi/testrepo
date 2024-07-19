using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewDrawingRevisionPage : SOSPage
    {

#region Members
        private DrawingRevisionInfo drawingRevisionInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (drawingRevisionInfo == null)
                return null;

            tempNode.ParentNode.Title = drawingRevisionInfo.Drawing.Name;
            tempNode.ParentNode.Url += "?DrawingId=" + drawingRevisionInfo.Drawing.IdStr;

            tempNode.ParentNode.ParentNode.Title = drawingRevisionInfo.Drawing.DrawingType.Name + " Drawings";
            tempNode.ParentNode.ParentNode.Url += "?Type=" + drawingRevisionInfo.Drawing.Type + "&ProjectId=" + drawingRevisionInfo.Drawing.Project.IdStr + "&DrawingTypeId=" + drawingRevisionInfo.Drawing.DrawingType.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + drawingRevisionInfo.Drawing.Project.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Title = drawingRevisionInfo.Drawing.Project.Name + (drawingRevisionInfo.Drawing.IsProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + drawingRevisionInfo.Drawing.Project.IdStr;

            return currentNode;
        }

        private void BindDrawing()
        {
            if (drawingRevisionInfo.Drawing.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            lblNumber.Text = UI.Utils.SetFormString(drawingRevisionInfo.Number);
            lblDate.Text = UI.Utils.SetFormDate(drawingRevisionInfo.RevisionDate);
            lblComments.Text = UI.Utils.SetFormString(drawingRevisionInfo.Comments);
            sfrRevisionFile.DrawingRevision = drawingRevisionInfo;
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterDrawingRevisionId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewDrawing);
                parameterDrawingRevisionId = Utils.CheckParameter("DrawingRevisionId");
                drawingRevisionInfo = tradesController.GetDrawingRevision(Int32.Parse(parameterDrawingRevisionId));
                Core.Utils.CheckNullObject(drawingRevisionInfo, parameterDrawingRevisionId, "Drawing Revision");
                drawingRevisionInfo.Drawing.Project = projectsController.GetProject(drawingRevisionInfo.Drawing.Project.Id);

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditDrawing))
                    {
                        if (processController.AllowUpdateDrawingsCurrentUser(drawingRevisionInfo.Drawing.Project))
                        {
                            pnlEdit.Visible = true;
                            cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Drawing Revisions?');");
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
            Response.Redirect("~/Modules/Projects/EditDrawingRevision.aspx?DrawingRevisionId=" + drawingRevisionInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                TradesController.GetInstance().DeleteDrawingRevision(drawingRevisionInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingRevisionInfo.Drawing.IdStr);
        }
#endregion
    
    }
}
