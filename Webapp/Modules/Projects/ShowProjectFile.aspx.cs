using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowProjectFilePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProjectInfo projectInfo;
            String parameterProjectId;
            String parameterFileType;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                parameterFileType = Utils.CheckParameter("FileType");
                parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProject(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                switch (parameterFileType)
                {
                    case ProjectInfo.FileTypeManual:
                        Utils.SendFile(projectInfo.AttachmentsFolder, projectInfo.MaintenanceManualFile);
                        break;
                    case ProjectInfo.FileTypeReview:
                        Utils.SendFile(projectInfo.AttachmentsFolder, projectInfo.PostProjectReviewFile);
                        break;
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
