using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListDrawingsPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo = null;
        private DrawingTypeInfo drawingTypeInfo = null;
        private String drawingType = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = drawingTypeInfo.Name + " Drawings";

            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = projectInfo.Name + (drawingType == Info.TypeProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        private void BindDrawings()
        {
            if (drawingType == Info.TypeProposal)
                pnlProposal.CssClass = "PanelProposal";

            TitleBar1.Title = drawingTypeInfo.Name + " Drawings";
            lnkAddDrawing.NavigateUrl = String.Format("~/Modules/Projects/EditDrawing.aspx?Type={0}&ProjectId={1}&DrawingTypeId={2}", drawingType, projectInfo.IdStr, drawingTypeInfo.IdStr);
            gvDrawings.DataSource = TradesController.GetInstance().GetDrawings(projectInfo, drawingTypeInfo);
            gvDrawings.DataBind();
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterProjectId;
            String parameterDrawingTypeId;

            try
            {
                Security.CheckAccess(Security.userActions.ListDrawings);

                drawingType = Utils.CheckParameter("Type");
                parameterProjectId = Utils.CheckParameter("ProjectId");
                parameterDrawingTypeId = Utils.CheckParameter("DrawingTypeId");

                projectInfo = projectsController.GetProject(Int32.Parse(parameterProjectId));
                drawingTypeInfo = tradesController.GetDrawingType(Int32.Parse(parameterDrawingTypeId));

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");
                Core.Utils.CheckNullObject(drawingTypeInfo, parameterDrawingTypeId, "Drawing Type");

                if (drawingType == Info.TypeActive)
                    projectInfo.Drawings = tradesController.GetDeepDrawingsActive(projectInfo);
                else if (drawingType == Info.TypeProposal)
                    projectInfo.Drawings = tradesController.GetDeepDrawingsProposal(projectInfo);
                else
                    throw new Exception("Invalid Drawing type");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditDrawing))
                        if (processController.AllowUpdateDrawingsCurrentUser(projectInfo))
                            phAddDrawing.Visible = true;

                    BindDrawings();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}