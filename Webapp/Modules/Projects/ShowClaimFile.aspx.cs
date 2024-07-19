using System;

using SOS.Core;

namespace SOS.Web  // DS20231023 >>>>>
{
    public partial class ShowClaimFile : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ClaimInfo ClaimInfo;
            String parameterClaimId;
            String parameterFileType;

            try
            {
                Security.CheckAccess(Security.userActions.ViewClientVariation);
                parameterFileType = Utils.CheckParameter("FileType");
                parameterClaimId = Utils.CheckParameter("ClaimId");
                ClaimInfo = projectsController.GetClaim(Int32.Parse(parameterClaimId));
                ClaimInfo.Project = projectsController.GetProject(Int32.Parse(ClaimInfo.ProjectIdStr));
                Core.Utils.CheckNullObject(ClaimInfo, parameterClaimId, "Claim");

                switch (parameterFileType)
                {
                    case "BackupFile1":
                        Utils.SendFile(ClaimInfo.Project.AttachmentsFolder, ClaimInfo.BackupFile1);
                        break;
                    case "BackupFile2":
                        Utils.SendFile(ClaimInfo.Project.AttachmentsFolder, ClaimInfo.BackupFile2);
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
