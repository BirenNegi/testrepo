using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowEOTFilePage : System.Web.UI.Page
    {
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            EOTInfo EOTInfo = new EOTInfo();
            String parameterEOTId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewEOT);
                parameterEOTId = Utils.CheckParameter("EOTId");
                EOTInfo = projectsController.GetEOT(Int32.Parse(parameterEOTId));
                Core.Utils.CheckNullObject(EOTInfo, parameterEOTId, "EOT");

                //#---
                String parameterFileType= Utils.CheckParameter("FileType");
                if(parameterFileType=="BackupFile")
                    Utils.SendFile(EOTInfo.Project.AttachmentsFolder, EOTInfo.ClientBackuplFile);
                else
                //#---
                Utils.SendFile(EOTInfo.Project.AttachmentsFolder, EOTInfo.ClientApprovalFile);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
