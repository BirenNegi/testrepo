using System;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowTransmittalsPage : System.Web.UI.Page
    {

#region Members
        List<TransmittalInfo> TransmittalInfoList = new List<TransmittalInfo>();
        private ProjectInfo projectInfo = null;
#endregion

#region Private Methods
        private TransmittalInfo CopyTransmittal(TransmittalInfo transmittalInfo, ContactInfo contactInfo)
        {
            TransmittalInfo transmittal = new TransmittalInfo(transmittalInfo.Id);
            transmittal.Contact = contactInfo;
            return transmittal;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            PeopleController peopleController = PeopleController.GetInstance();
            List<TransmittalInfo> transmittalInfoList = new List<TransmittalInfo>();
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            LocalReport localReport = new LocalReport();
            String parameterProjectId;
            Byte[] pdfReport = null;
            Int32 transmittalId = 0;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                parameterProjectId = Utils.CheckParameter("ProjectId");
                
                projectInfo = projectsController.GetProjectWithTransmittals(Int32.Parse(parameterProjectId));
                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                foreach (TransmittalInfo transmittalInfo in projectInfo.Transmittals)
                {
                    transmittalInfo.Contacts = projectsController.GetTransmittalContacts(transmittalInfo);

                    transmittalInfoList.Add(transmittalInfo);

                    foreach (ContactInfo contactInfo in transmittalInfo.Contacts)
                        transmittalInfoList.Add(CopyTransmittal(transmittalInfo,contactInfo));

                    if ((Boolean)transmittalInfo.SendClientContact && projectInfo.ClientContact != null)
                        transmittalInfoList.Add(CopyTransmittal(transmittalInfo, peopleController.ConvertClientContactToContact(projectInfo.ClientContact)));

                    if ((Boolean)transmittalInfo.SendClientContact1 && projectInfo.ClientContact1 != null)
                        transmittalInfoList.Add(CopyTransmittal(transmittalInfo, peopleController.ConvertClientContactToContact(projectInfo.ClientContact1)));

                    if ((Boolean)transmittalInfo.SendClientContact2 && projectInfo.ClientContact2 != null)
                        transmittalInfoList.Add(CopyTransmittal(transmittalInfo, peopleController.ConvertClientContactToContact(projectInfo.ClientContact2)));

                    if ((Boolean)transmittalInfo.SendQuantitySurveyor && projectInfo.QuantitySurveyor != null)
                        transmittalInfoList.Add(CopyTransmittal(transmittalInfo, peopleController.ConvertClientContactToContact(projectInfo.QuantitySurveyor)));

                    if ((Boolean)transmittalInfo.SendSuperintendent && projectInfo.Superintendent != null)
                        transmittalInfoList.Add(CopyTransmittal(transmittalInfo, peopleController.ConvertClientContactToContact(projectInfo.Superintendent)));
                }

                foreach (TransmittalInfo transmittalInfo in transmittalInfoList)
                    if ((Int32)transmittalInfo.Id == transmittalId)
                        transmittalInfo.Id = null;
                    else
                        transmittalId = (Int32)transmittalInfo.Id;

                reportParameters.Add(new ReportParameter("ProjectName", projectInfo.Name));
                reportParameters.Add(new ReportParameter("JobNo", UI.Utils.SetFormString(projectInfo.FullNumber)));                

                localReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\Transmittals.rdlc";

                // Uncomment to load the assembly
                // localReport.ExecuteReportInCurrentAppDomain(AppDomain.CurrentDomain.Evidence);
                // localReport.AddTrustedCodeModuleInCurrentAppDomain("SOS.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

                localReport.DataSources.Add(new ReportDataSource("TransmittalInfo", transmittalInfoList));
                localReport.SetParameters(reportParameters);

                pdfReport = UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "Transmittals");
        }
#endregion

    }
}
