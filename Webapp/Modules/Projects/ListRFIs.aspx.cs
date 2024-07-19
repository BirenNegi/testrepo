using System;
using System.Web;
using System.Xml;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListRFIsPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo = null;
        private ProjectsController projectsController = ProjectsController.GetInstance();
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

        private void BindRFIs()
        {
            lnkAddRFI.NavigateUrl = "~/Modules/Projects/EditRFI.aspx?ProjectId=" + projectInfo.IdStr;

            gvRFIs.DataSource = projectInfo.RFIs;
            gvRFIs.DataBind();
        }

        protected Boolean IsComplete(RFIInfo rFIInfo)
        {
            return projectsController.CheckRFI(rFIInfo).DocumentElement == null;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterProjectId;
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ListRFIs);
                parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithRFIs(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (processController.AllowAddRFICurrentUser(projectInfo))
                    phAddNew.Visible = true;

                if (!Page.IsPostBack)
                    BindRFIs();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
