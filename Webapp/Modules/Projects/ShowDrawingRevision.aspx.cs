using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowDrawingRevisionPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            DrawingRevisionInfo drawingRevisionInfo;
            String parameterDrawingRevisionId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProjectsDrawings);
                parameterDrawingRevisionId = Utils.CheckParameter("DrawingRevisionId");
                drawingRevisionInfo = tradesController.GetDrawingRevision(Int32.Parse(parameterDrawingRevisionId));
                Core.Utils.CheckNullObject(drawingRevisionInfo, parameterDrawingRevisionId, "Drawing Revision");
                Utils.SendFile(drawingRevisionInfo.Drawing.Project.AttachmentsFolder, drawingRevisionInfo.File);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
