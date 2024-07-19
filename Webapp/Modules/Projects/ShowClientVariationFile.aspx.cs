using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowClientVariationFilePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ClientVariationInfo clientVariationInfo;
            String parameterClientVariationId;
            String parameterFileType;

            try
            {
                Security.CheckAccess(Security.userActions.ViewClientVariation);
                parameterFileType = Utils.CheckParameter("FileType");
                parameterClientVariationId = Utils.CheckParameter("ClientVariationId");
                clientVariationInfo = projectsController.GetClientVariationWithItemsAndTrades(Int32.Parse(parameterClientVariationId));
                Core.Utils.CheckNullObject(clientVariationInfo, parameterClientVariationId, "Client Variation");

                switch (parameterFileType)
                {
                    case ClientVariationInfo.FileTypeQuotes:
                        Utils.SendFile(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.QuotesFile);
                        break;
                    case ClientVariationInfo.FileTypeBackup:
                        Utils.SendFile(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.BackupFile);
                        break;
                    case ClientVariationInfo.FileTypeClientApproval:
                        Utils.SendFile(clientVariationInfo.Project.AttachmentsFolder, clientVariationInfo.ClientApprovalFile);
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
