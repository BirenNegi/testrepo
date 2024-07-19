using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowTransmittalPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            TransmittalInfo transmittalInfo = null;
            String parameterTransmittalId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTransmittal);
                parameterTransmittalId = Utils.CheckParameter("TransmittalId");
                transmittalInfo = projectsController.GetDeepTransmittal(Int32.Parse(parameterTransmittalId));
                Core.Utils.CheckNullObject(transmittalInfo, parameterTransmittalId, "Transmittal");
               //San-- projectsController.CheckViewCurrentUser(transmittalInfo); //San---
                transmittalInfo.Project = projectsController.GetProject(transmittalInfo.Project.Id);

                pdfReport = projectsController.GenerateTransmittalReport(transmittalInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "Transmittal");
        }
#endregion

    }
}
