using System;
using System.Web;
using System.Xml;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListEOTsPage : SOSPage
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

        private void BindEOTs()
        {
            lnkAddEOT.NavigateUrl = "~/Modules/Projects/EditEOT.aspx?ProjectId=" + projectInfo.IdStr;

            gvEOTs.DataSource = projectInfo.EOTs;
            gvEOTs.DataBind();
        }

        protected Boolean IsComplete(EOTInfo eOTInfo)
        {
            return projectsController.CheckEOT(eOTInfo).DocumentElement == null;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterProjectId;
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ListEOTs);
                parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithEOTs(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (processController.AllowAddEOTCurrentUser(projectInfo))
                    phAddNew.Visible = true;

                if (!Page.IsPostBack)
                    BindEOTs();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
