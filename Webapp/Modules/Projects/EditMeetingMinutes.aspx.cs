using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;


namespace SOS.Web
{
    public partial class EditMeetingMinutes : SOSPage
    {
        #region Members
        private MeetingMinutesInfo meetingInfo = null;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (meetingInfo == null)
                return null;

            tempNode.ParentNode.Url += "?ProjectId=" + meetingInfo.Project.IdStr;

            tempNode.ParentNode.ParentNode.Title = meetingInfo.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + meetingInfo.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (meetingInfo.Id == null)
            {
                TitleBar.Title = "Adding Meeting Minutes";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }

            //lblNumber.Text = UI.Utils.SetFormInteger(meetingInfo.Number);
            txtTypeNumber.Text = UI.Utils.SetFormInteger(meetingInfo.TypeNumber);
            Txtsubject.Text = UI.Utils.SetFormString(meetingInfo.Subject);
            sdrRaiseDate.Date = meetingInfo.MeetingDate;
            txtTime.Text = meetingInfo.MeetingTime;
            txtLocation.Text = meetingInfo.Location;
            txtAttendees.Text = meetingInfo.Attendees;
            sfsReferenceFile.FilePath = meetingInfo.ReferenceFile;
            sfsReferenceFile.Path = meetingInfo.Project.AttachmentsFolder;

        }

        private void FormToObjects()
        {
            meetingInfo.TypeNumber = UI.Utils.GetFormInteger(txtTypeNumber.Text);
            meetingInfo.Subject = UI.Utils.GetFormString(Txtsubject.Text);
            meetingInfo.MeetingDate = sdrRaiseDate.Date;
            meetingInfo.MeetingTime = txtTime.Text;
            meetingInfo.Location = txtLocation.Text;
            meetingInfo.ReferenceFile = sfsReferenceFile.FilePath;
            meetingInfo.Attendees = txtAttendees.Text;
            
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterMeetingId;
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.EditMeetingMinutes);
                parameterMeetingId = Request.Params["MeetingId"];
                if (parameterMeetingId == null)
                {
                    String parameterProjectId = Utils.CheckParameter("ProjectId");
                    ProjectInfo projectInfo = projectsController.GetProjectWithMeetings(Int32.Parse(parameterProjectId));
                    Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");
                    meetingInfo = projectsController.InitializeMeeting(projectInfo);
                }
                else
                {
                    meetingInfo = projectsController.GetMeetingById(Int32.Parse(parameterMeetingId));
                    Core.Utils.CheckNullObject(meetingInfo, parameterMeetingId, "Meeting Minutes");
                    meetingInfo.Project = projectsController.GetProjectWithMeetings(meetingInfo.Project.Id);
                }

                processController.CheckEditCurrentUser(meetingInfo);

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
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    meetingInfo.Id = ProjectsController.GetInstance().AddUpdateMeeting(meetingInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewMeetingMinutes.aspx?MeetingId=" + meetingInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (meetingInfo.Id == null)
                Response.Redirect("~/Modules/Projects/ListMeetingMinutes.aspx?ProjectId=" + meetingInfo.Project.IdStr);
            else
                Response.Redirect("~/Modules/Projects/ViewMeetingMinutes.aspx?MeetingId=" + meetingInfo.IdStr);
        }

        #endregion


    }
}