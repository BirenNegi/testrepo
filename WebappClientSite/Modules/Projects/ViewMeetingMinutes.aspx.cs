using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;
using SOS.Core;

namespace SOS.Web
{
    public partial class ViewMeetingMinutes :SOSPage
    {

        #region Members
        private  MeetingMinutesInfo meetingInfo = null;
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

        private void BindTree(XmlNode xmlNode, TreeView treeView)
        {
            treeView.Nodes.Clear();
            treeView.Nodes.Add(new TreeNode());
            Utils.AddNode(xmlNode, treeView.Nodes[0]);
            treeView.Visible = true;
        }

        private void BindMeeting()
        {
            ProcessController processController = ProcessController.GetInstance();

            if (Security.ViewAccess(Security.userActions.EditMeetingMinutes) && processController.AllowEditCurrentUser(meetingInfo))
            {
                pnlEdit.Visible = true;
                //phSendEmail.Visible = true;
            }

            //lblNumber.Text = UI.Utils.SetFormInteger(meetingInfo.Number);
            lblTypleNumber.Text= UI.Utils.SetFormInteger(meetingInfo.TypeNumber);
            lblSubject.Text = UI.Utils.SetFormString(meetingInfo.Subject);
            lblRaiseDate.Text = UI.Utils.SetFormDate(meetingInfo.MeetingDate);
            lblTime.Text = UI.Utils.SetFormString(meetingInfo.MeetingTime);
            lblLocation.Text = UI.Utils.SetFormString(meetingInfo.Location);
            lblAttendees.Text = UI.Utils.SetFormString(meetingInfo.Attendees);
            lblReferenceFileName.Text = UI.Utils.SetFormString(meetingInfo.ReferenceFile);
            

           
        }
        #endregion


        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parametermeetingId;
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewMeetingMinutes);
                parametermeetingId = Utils.CheckParameter("MeetingId");
                meetingInfo = projectsController.GetMeetingById(Int32.Parse(parametermeetingId));
                Core.Utils.CheckNullObject(meetingInfo, parametermeetingId, "MeetingMinutes");
                meetingInfo.Project = projectsController.GetProject(meetingInfo.Project.Id);

                if (!Page.IsPostBack)
                    BindMeeting();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditMeetingMinutes.aspx?MeetingId=" + meetingInfo.IdStr);
        }


        #endregion










    }
}