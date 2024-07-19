using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

using ProcessStatus = SOS.Core.ProcessStatus;

namespace SOS.Web
{
    public partial class EditProjectFilesPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            sfsMaintenanceManualFile.FilePath = projectInfo.MaintenanceManualFile;
            sfsMaintenanceManualFile.Path = projectInfo.AttachmentsFolder;

            sfsPostProjectReviewFile.FilePath = projectInfo.PostProjectReviewFile;
            sfsPostProjectReviewFile.Path = projectInfo.AttachmentsFolder;
        }

        private void FormToObjects()
        {
            projectInfo.MaintenanceManualFile = sfsMaintenanceManualFile.FilePath;
            projectInfo.PostProjectReviewFile = sfsPostProjectReviewFile.FilePath;
        }
#endregion

#region Public Methods
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterProjectId = Request.Params["ProjectId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditProject);
                projectInfo = ProjectsController.GetInstance().GetProject(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (!ProcessController.GetInstance().AllowEditFilesCurrentUser(projectInfo))
                    throw new SecurityException();

                if (!Page.IsPostBack)
                    ObjectsToForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();

                    projectsController.UpdateProjectFiles(projectInfo);
                    Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
#endregion

    }
}