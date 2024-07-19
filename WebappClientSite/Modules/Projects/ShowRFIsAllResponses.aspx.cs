using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SOS.Core;

namespace SOS.Web
{
    public partial class ShowRFIsAllResponses : System.Web.UI.Page
    {
        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            RFIInfo rFIInfo = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterRFIId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewRFI);
                parameterRFIId = Utils.CheckParameter("RFIId");
                rFIInfo = projectsController.GetRFI(Int32.Parse(parameterRFIId));
                Core.Utils.CheckNullObject(rFIInfo, parameterRFIId, "RFI");
                rFIInfo.Project = projectsController.GetProject(rFIInfo.Project.Id);

                

                pdfReport = projectsController.GenerateRFIReportWithResponses(rFIInfo);
              
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "RFI");
        }
        #endregion
    }
}