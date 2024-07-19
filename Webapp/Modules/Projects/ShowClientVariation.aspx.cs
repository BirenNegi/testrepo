using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowClientVariationPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientVariationInfo clientVariationInfo = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterClientVariationId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewClientVariation);
                parameterClientVariationId = Utils.CheckParameter("ClientVariationId");
                clientVariationInfo = projectsController.GetClientVariationWithItemsAndTrades(Int32.Parse(parameterClientVariationId));
                Core.Utils.CheckNullObject(clientVariationInfo, parameterClientVariationId, "Client Variation");
                clientVariationInfo.Project = projectsController.GetProject(clientVariationInfo.Project.Id);

                clientVariationInfo.TheParentClientVariation.SubClientVariations = projectsController.GetClientVariations(clientVariationInfo.TheParentClientVariation);
                projectsController.SetRevisionNames(clientVariationInfo);

                pdfReport = projectsController.GenerateClientVariationReport(clientVariationInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "ClientVariation");
        }
#endregion

    }
}
