using System;
using System.Configuration;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowEOTPage : System.Web.UI.Page
    {

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            EOTInfo eOTInfo = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterEOTId;
            String ParameterNodId;
            Byte[] pdfReport = null;

            try
            {
                /*---San---
                //Security.CheckAccess(Security.userActions.ViewEOT);
                //parameterEOTId = Utils.CheckParameter("EOTId");
                //eOTInfo = projectsController.GetEOT(Int32.Parse(parameterEOTId));
                //Core.Utils.CheckNullObject(eOTInfo, parameterEOTId, "EOT");
                //eOTInfo.Project = projectsController.GetProject(eOTInfo.Project.Id);

                //pdfReport = projectsController.GenerateEOTReport(eOTInfo);*/
              

                Security.CheckAccess(Security.userActions.ViewEOT);
                parameterEOTId = Utils.CheckParameter("EOTId");
                ParameterNodId = Utils.CheckParameter("NODID");
                if (ParameterNodId == "")
                    eOTInfo = projectsController.GetEOT(Int32.Parse(parameterEOTId));
                else
                    eOTInfo = projectsController.GetNODEOT(Int32.Parse(parameterEOTId), Int32.Parse(ParameterNodId));
                Core.Utils.CheckNullObject(eOTInfo, parameterEOTId, "EOT");
                eOTInfo.Project = projectsController.GetProject(eOTInfo.Project.Id);

                pdfReport = projectsController.GenerateEOTReport(eOTInfo);

             /*---San---*/

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "EOT");
        }
        #endregion

    }
}
