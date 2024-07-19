using System;
using System.Web;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditDrawingPage : SOSPage
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

        private void ObjectsToForm()
        {
            if (drawingInfo.IsProposal)
                pnlProposal.CssClass = "PanelProposal";

            if (drawingInfo.Id == null)
            {
                TitleBar.Title = "Adding Drawing and First Revision";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";

                phFirstRevision.Visible = true;
            }
            else
            {
                TitleBar.Title = "Updating Drawing";
            }

            txtName.Text = UI.Utils.SetFormString(drawingInfo.Name);
            txtDescription.Text = UI.Utils.SetFormString(drawingInfo.Description);
            lblType.Text = UI.Utils.SetFormString(drawingInfo.DrawingType.Name);
        }

        private void FormToObjects()
        {
            drawingInfo.Name = UI.Utils.GetFormString(txtName.Text);
            drawingInfo.Description = UI.Utils.GetFormString(txtDescription.Text);

            if (UI.Utils.SetFormString(txtNumber.Text) != String.Empty)
            {
                DrawingRevisionInfo drawingRevisionInfo = new DrawingRevisionInfo();

                drawingRevisionInfo.Drawing = drawingInfo;
                drawingRevisionInfo.Number = UI.Utils.GetFormString(txtNumber.Text);
                drawingRevisionInfo.RevisionDate = sdrDate.Date;
                drawingRevisionInfo.Comments = UI.Utils.GetFormString(txtComments.Text);

                drawingInfo.DrawingRevisions = new List<DrawingRevisionInfo>();
                drawingInfo.DrawingRevisions.Add(drawingRevisionInfo);
            }
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
                Security.CheckAccess(Security.userActions.EditDrawing);                
                parameterDrawingId = Request.Params["DrawingId"];
                if (parameterDrawingId == null)
                {
                    drawingInfo = new DrawingInfo();

                    String parameterType = Utils.CheckParameter("Type");
                    String parameterProjectId = Utils.CheckParameter("ProjectId");
                    String parameterDrawingTypeId = Utils.CheckParameter("DrawingTypeId");

                    drawingInfo.Type = parameterType;
                    drawingInfo.Project = projectsController.GetProject(Int32.Parse(parameterProjectId));
                    drawingInfo.DrawingType = tradesController.GetDrawingType(Int32.Parse(parameterDrawingTypeId));

                    Core.Utils.CheckNullObject(drawingInfo.Project, parameterProjectId, "Project");
                    Core.Utils.CheckNullObject(drawingInfo.DrawingType, parameterDrawingTypeId, "Drawing Type");
                }
                else
                {
                    drawingInfo = tradesController.GetDrawing(Int32.Parse(parameterDrawingId));
                    Core.Utils.CheckNullObject(drawingInfo, parameterDrawingId, "Drawing");
                    drawingInfo.Project = projectsController.GetProject(drawingInfo.Project.Id);
                }

                processController.CheckUpdateDrawingsCurrentUser(drawingInfo.Project);

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
            TradesController tradesController = TradesController.GetInstance();
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();

                    if (drawingInfo.Id == null)
                    {
                        drawingInfo.Id = tradesController.AddDrawing(drawingInfo);
                        if (drawingInfo.DrawingRevisions != null)
                            tradesController.AddDrawingRevision(drawingInfo.DrawingRevisions[0]);
                    }
                    else
                        tradesController.UpdateDrawing(drawingInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (drawingInfo.Id == null)
                Response.Redirect(String.Format("~/Modules/Projects/ListDrawings.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", drawingInfo.Type, drawingInfo.Project.IdStr, drawingInfo.DrawingType.IdStr));
            else
                Response.Redirect("~/Modules/Projects/ViewDrawing.aspx?DrawingId=" + drawingInfo.IdStr);
        }
#endregion

    }
}
