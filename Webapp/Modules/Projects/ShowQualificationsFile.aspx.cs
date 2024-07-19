using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;

using Client = SOS.FileTransferService.Client;



namespace SOS.Web
{
    public partial class ShowQualificationsFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Byte[] fileData = null;
            String fileName = null;
            try
            {
              //  Security.CheckAccess(Security.userActions.ViewParticipationSubContractor);
               
                String imagePath =  Utils.CheckParameter("ImagePath") ;
                fileName = Utils.CheckParameter("ImageName");

                if(imagePath!=string.Empty)
                fileData = Client.Utils.GetFileData(imagePath);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendFile(fileData,fileName);
        }
    }
}