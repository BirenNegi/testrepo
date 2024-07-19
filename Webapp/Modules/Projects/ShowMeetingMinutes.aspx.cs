using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowMeetingMinutes : System.Web.UI.Page
    {
        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            MeetingMinutesInfo meetingInfo = new MeetingMinutesInfo();



            //String parameterFileType;
            String parameterMeetingId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewMeetingMinutes);
               // parameterFileType = Utils.CheckParameter("FileType");
                parameterMeetingId = Utils.CheckParameter("MeetingId");
                meetingInfo = projectsController.GetMeetingById(Int32.Parse(parameterMeetingId));
                Core.Utils.CheckNullObject(meetingInfo, parameterMeetingId, "MeetingMinutes");

                   if(meetingInfo.ReferenceFile!=null)
                        Utils.SendFile(meetingInfo.Project.AttachmentsFolder, meetingInfo.ReferenceFile);



                //San--14/01/2021---  FOR CLIENT SOS--------------------------------------- 

               // if (meetingInfo.ReferenceFile != null){
                //Byte[] fileData = null;
                //fileData = Client.Utils.GetFileData(meetingInfo.Project.AttachmentsFolder + "\\" + meetingInfo.ReferenceFile);
                //Utils.SendFile(fileData, meetingInfo.ReferenceFile);  }


                    //San--14/01/2021






            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        #endregion

    }
}