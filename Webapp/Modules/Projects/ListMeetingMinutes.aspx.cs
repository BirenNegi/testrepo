using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListMeetingMinutes : SOSPage
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

        private void BindMeetingMinutes()
        {
            lnkAddMeetings.NavigateUrl = "~/Modules/Projects/EditMeetingMinutes.aspx?ProjectId=" + projectInfo.IdStr;

            gvMeetings.DataSource = projectInfo.MeetingMinutesList;
            gvMeetings.DataBind();
        }

        protected Boolean IsComplete(MeetingMinutesInfo meetingInfo)
        {
            return meetingInfo.ReferenceFile==null?false:true;
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterProjectId;
            ProcessController processController = ProcessController.GetInstance();
            
            try
            {
                Security.CheckAccess(Security.userActions.ListMeetingMinutes);
                parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithMeetings(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (processController.AllowAddMeetingMinutesCurrentUser(projectInfo))
                    phAddNew.Visible = true;

                if (!Page.IsPostBack)
                    BindMeetingMinutes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        #endregion
    }
}