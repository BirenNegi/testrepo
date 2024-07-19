using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowSeparateAccountInvoicePage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            SeparateAccountInfo separateAccountInfo = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterClientVariationId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewClientVariation);
                parameterClientVariationId = Utils.CheckParameter("ClientVariationId");
                separateAccountInfo = (SeparateAccountInfo)projectsController.GetClientVariationWithItemsAndTrades(Int32.Parse(parameterClientVariationId));
                Core.Utils.CheckNullObject(separateAccountInfo, parameterClientVariationId, "separate Account");
                separateAccountInfo.Project = projectsController.GetProject(separateAccountInfo.Project.Id);

                separateAccountInfo.TheParentClientVariation.SubClientVariations = projectsController.GetClientVariations(separateAccountInfo.TheParentClientVariation);
                projectsController.SetRevisionNames(separateAccountInfo);

                pdfReport = projectsController.GenerateSeparateAccountInvoiceReport(separateAccountInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "SeparateAccountInvoice");
        }
#endregion

    }
}
