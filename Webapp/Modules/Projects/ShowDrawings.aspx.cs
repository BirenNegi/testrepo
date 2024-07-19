using System;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowDrawingsPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProjectInfo projectInfo = null;
            String parameterType = null;
            String parameterProjectId;
            LocalReport localReport = new LocalReport();
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);

                parameterType = Utils.CheckParameter("Type");
                parameterProjectId = Utils.CheckParameter("ProjectId");

                if (parameterType == Info.TypeActive)
                    projectInfo = projectsController.GetProjectWithDrawingsActive(Int32.Parse(parameterProjectId));
                else if (parameterType == Info.TypeProposal)
                    projectInfo = projectsController.GetProjectWithDrawingsProposal(Int32.Parse(parameterProjectId));
                else
                    throw new Exception("Invalid Drawing type");

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                reportParameters.Add(new ReportParameter("ProjectName", projectInfo.Name));
                reportParameters.Add(new ReportParameter("JobNo", UI.Utils.SetFormString(projectInfo.FullNumber)));
                reportParameters.Add(new ReportParameter("DrawingTypes", UI.Utils.SetFormString(parameterType)));

                localReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\DrawingsLastRevisions.rdlc";
                localReport.DataSources.Add(new ReportDataSource("DrawingInfo", projectInfo.Drawings));
                localReport.SetParameters(reportParameters);

                pdfReport = UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "Drawings");
        }
#endregion

    }
}
