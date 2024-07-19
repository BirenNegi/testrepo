using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text.RegularExpressions;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;
using SOS.Core;

namespace SOS.Web
{
    public partial class ShowSiteOrderPage : System.Web.UI.Page
    {

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z0-9_]");
            SiteOrderInfo SiteOrderInfo = null;
            ProjectInfo ProjectInfo = null;
            SubContractorInfo SubContractorInfo = null;
            PeopleInfo ForemanPeopleInfo = null;
            PeopleInfo ContactPeopleInfo = null;
            PeopleInfo GivenByPeopleInfo = null;
            BusinessUnitInfo BusinessUnitInfo = null; ;
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            SubContractorsController SubContractorsController = SubContractorsController.GetInstance();
            PeopleController PeopleController = PeopleController.GetInstance();
            ProjectTradesInfo projectTradesInfo = null;  //DS20230822
            Byte[] pdfReport = null;
            Byte[] pdfTerms = null;

            //       try
            //       {
            Security.CheckAccess(Security.userActions.ViewSiteOrder);
            String parameterSiteOrderId = Utils.CheckParameter("OrderId");

            SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterSiteOrderId));
            SiteOrderInfo.isOrderApproved = SiteOrdersController.SiteOrderGetApprovalStatus(Int32.Parse(parameterSiteOrderId));
            // ProjectInfo = ProjectsController.GetProject(SiteOrderInfo.ProjectId); // DS20230411
            ProjectInfo = ProjectsController.GetProjectWithTradesParticipations(SiteOrderInfo.ProjectId); // DS20230411
            SiteOrderInfo.Items = SiteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterSiteOrderId));
            SubContractorInfo = SubContractorsController.GetSubContractor(SiteOrderInfo.SubContractorId);
            ForemanPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.ForemanID);
            ContactPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.ContactPeopleId);
            GivenByPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.GivenByPeopleId);
            projectTradesInfo = SiteOrdersController.GetInstance().GetProjectTrades(SiteOrderInfo.ProjectId, SiteOrderInfo.TradesCode);//DS20230822
            

            // DS20230307
            string BudgetName = "";
            if (projectTradesInfo != null) //DS20230822;
            {
                BudgetName = projectTradesInfo.Code + " - " + projectTradesInfo.Name;
            }
            //List<TradeTemplateInfo> tradeTemplates = TradesController.GetInstance().GetTradeTemplatesFromCode(SiteOrderInfo.TradesCode);
            //foreach (TradeTemplateInfo TradeTemplate in tradeTemplates) // DS20230308
            //{
            //    BudgetName = TradeTemplate.TradeCode + " - " + TradeTemplate.TradeDescription;
            //}
            // Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");
            if (SiteOrderInfo.Typ == "Mat")
            {
                pdfReport = SiteOrdersController.GenerateSiteOrderReport(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, projectTradesInfo);
                //Utils.SendPdfData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
                //SOS.Web.Utils.SavePDFData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            }
            if (SiteOrderInfo.Typ == "Ins")
            {
                pdfReport = SiteOrdersController.GenerateSiteOrderReportIns(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, projectTradesInfo);
                //Utils.SendPdfData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
                //SOS.Web.Utils.SavePDFData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            }
            if (SiteOrderInfo.Typ == "Hir")
            {
                pdfReport = SiteOrdersController.GenerateSiteOrderReportHir(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, projectTradesInfo);
                // Utils.SendPdfData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            }


            //SOS.Web.Utils.SavePDFData(pdfTerms, String.Format("SiteOrder_{0}_{1}_{2}_TERMS.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            SiteOrderInfo.Title = Regex.Replace(SiteOrderInfo.Title, "[^a-zA-Z0-9]", String.Empty);  // 20230824
            using (var compressedFileStream = new System.IO.MemoryStream())
            {
                //Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    //Create a zip entry for each attachment
                     var zipEntry = zipArchive.CreateEntry(String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));

                    //Get the stream of the attachment
                    using (var originalFileStream = new System.IO.MemoryStream(pdfReport))
                    {
                        using (var zipEntryStream = zipEntry.Open())
                        {
                            //Copy the attachment stream to the zip entry stream
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }
                    if (SiteOrderInfo.SubContractorId == 0)
                    {
                        pdfTerms = SOS.UI.Utils.getSiteOrderTerms(SiteOrderInfo.Typ,ProjectInfo.BusinessUnitName);  //DS20231023

                        var zipEntry2 = zipArchive.CreateEntry(String.Format("SiteOrder_{0}_{1}_{2}_TERMS.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));

                       //Get the stream of the attachment
                       using (var originalFileStream = new System.IO.MemoryStream(pdfTerms))
                       {
                           using (var zipEntryStream = zipEntry2.Open())
                           {
                            //Copy the attachment stream to the zip entry stream
                            originalFileStream.CopyTo(zipEntryStream);
                           }
                       }
                    }
                }
                SOS.Web.Utils.SaveZIPData(compressedFileStream.ToArray(), String.Format("SiteOrder_{0}_{1}_{2}.zip", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
                //sendOutZIP(compressedFileStream.ToArray(), "FileName.zip");



            }

            //        }
            //        catch (Exception Ex)
            //        {
            //            Utils.ProcessPageLoadException(this, Ex);
            //        }


        }
        #endregion

    }
}
