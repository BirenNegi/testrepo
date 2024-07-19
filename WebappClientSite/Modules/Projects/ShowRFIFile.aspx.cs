using System;

using SOS.Core;

using System.Collections.Generic;
using Client = SOS.FileTransferService.Client;


namespace SOS.Web
{
    public partial class ShowRFIFilePage : System.Web.UI.Page
    {
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            RFIInfo rFIInfo = new RFIInfo();
            String parameterFileType;
            String parameterRFIId;

            Byte[] fileData = null;
            String fileName = null;
            List<String> fileNames = new List<string>();
            try
            {
                Security.CheckAccess(Security.userActions.ViewRFI);
                parameterFileType = Utils.CheckParameter("FileType");
                parameterRFIId = Utils.CheckParameter("RFIId");
                rFIInfo = projectsController.GetRFI(Int32.Parse(parameterRFIId));
                Core.Utils.CheckNullObject(rFIInfo, parameterRFIId, "RFI");

                switch (parameterFileType)
                {


                    case RFIInfo.FileTypeReference:
                        //San---------------------------
                        fileName = UI.Utils.Path(rFIInfo.Project.AttachmentsFolder, rFIInfo.ReferenceFile);
                        fileNames.Add(fileName);

                        fileData = Client.Utils.GetFileData(fileName);
                        Utils.SendFile(fileData, rFIInfo.ReferenceFile);
                        break;
                        //San--------------------------------
                    case RFIInfo.FileTypeResponse:
                        Utils.SendFile(rFIInfo.Project.AttachmentsFolder, rFIInfo.ClientResponseFile);
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
