using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.IO;
using Microsoft.VisualBasic;
using PdfSharp.Pdf.Content.Objects;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web.ModelBinding;
using System.Runtime.InteropServices.ComTypes;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Net.Mail;


namespace SOS.Core
{
    public sealed class SiteOrdersController : Controller
    {

        #region Members
        private String parameterProjectId = "";
        private String parameterOrderId = "";
        private ProjectInfo projectInfo = null;
        private ProjectsController projectsController = ProjectsController.GetInstance();
        #endregion

        private static SiteOrdersController instance;

        private SiteOrdersController()
        {
        }
        public Byte[] GenerateSiteOrderReport(SiteOrderInfo SiteOrderInfo, ProjectInfo ProjectInfo, SubContractorInfo SubContractorInfo, PeopleInfo ForemanPeopleInfo, PeopleInfo ContactPeopleInfo, PeopleInfo GivenByPeopleInfo, string BudgetName, ProjectTradesInfo ProjectTradesInfo)
        {
            // DS20230822 
            LocalReport localReport = new LocalReport();
            string Letterhead = "";
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + "templates\\lh\\";
            //Server.MapPath(string.Format("~/Files/{0}", fileName));
            //Content Type and Header.
            string fileName = filePath + "LH_" + ProjectInfo.BusinessUnitName + ".txt";
            if (!File.Exists(fileName)) fileName = filePath + "LH_ALL.txt";
            if (File.Exists(fileName)) Letterhead = File.ReadAllText(fileName);
            string SiteOrder = "D" + Int32.Parse(SiteOrderInfo.IdStr).ToString("000000");
            string ProjectNo = ProjectInfo.Year.ToString() + "-" + ProjectInfo.Number.ToString();
            String OrderRef = "";                           //DS20230311   
            //SiteOrderInfo.Title = SiteOrderInfo.Title.Replace(",", " ");    // DS20230822
            if (SiteOrderInfo.Items.Count == 0)
            {
                string Notes = SiteOrderInfo.Notes.Replace("\r\n", "\n");
                string[] SNotes = (SiteOrderInfo.Notes).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);   //DS20230309
                for (int i = 0; i < SNotes.Length; i++)
                {
                    SiteOrderDetailInfo SOD = new SiteOrderDetailInfo();
                    SOD.Id = (int)Data.Utils.GetDBInt32(i);
                    SOD.Title = Data.Utils.GetDBString(SNotes[i]);
                    SOD.Qty = null;
                    //SOD.UM = Data.Utils.GetDBString(0);
                    //SOD.Price = (Decimal)Data.Utils.GetDBDecimal(0);
                    //SOD.Amount = (Decimal)Data.Utils.GetDBDecimal(0);
                    SiteOrderInfo.Items.Add(SOD);
                    //Console.WriteLine("{0}: {1}", i, splitted[i]);
                }
                SiteOrderInfo.Notes = "";
                //parameters.Add(UI.Utils.GetFormString(Search[0]));   //DS20230307
            }
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            BudgetName = BudgetName + " - " + SiteOrderInfo.OrderCode + " " + SiteOrderInfo.TypeInfo;   //DS202317
            if (SiteOrderInfo.SubContractorId == 0)
            {
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
                reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SiteOrderInfo.Name)));
                // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
                reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
                reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
                // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
                reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
                reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SiteOrderInfo.Street)));
                reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SiteOrderInfo.Locality + ", " + SiteOrderInfo.State + ", " + SiteOrderInfo.PostalCode)));
                // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
                // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));

                reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
                reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
                reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
                reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(SiteOrderInfo.Contact)));
                // DS20230304 
                reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
                reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
                reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
            }
            else
            {
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
                reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SubContractorInfo.Name)));
                // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
                reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
                reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
                // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
                reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
                reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SubContractorInfo.Street)));
                reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SubContractorInfo.Locality + ", " + SubContractorInfo.State + ", " + SubContractorInfo.PostalCode)));
                // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
                // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));
                reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
                reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
                reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
                reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
                if (ContactPeopleInfo == null)
                {
                    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString("")));
                }
                else
                {
                    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(ContactPeopleInfo.Name)));
                }
                // DS20230304 
                reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
                //reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                if (GivenByPeopleInfo == null)
                {
                    reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString("")));
                }
                else
                {
                    reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                }

                reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
                String WO = "";
                if (!ProjectInfo.ContractTrades.Equals(null))
                {

                 foreach (TradeInfo tradeInfo in ProjectInfo.ContractTrades)       // DS20230411 >>
                   {
                       if (SiteOrderInfo.SubContractorId == tradeInfo.SelectedSubContractor.Id)
                       {
                            WO = "-" + tradeInfo.WorkOrderNumber.ToString();
                           break;
                       }
                   }
                }
                OrderRef = "SUBCONTRACTOR REF: (" + ProjectInfo.Number.ToString() + ")" + WO;

                // DS20230411 <<  !data.Equals(null)

            }
            reportParameters.Add(new ReportParameter("OrderRef", UI.Utils.SetFormString(OrderRef)));    // DS20230411
            string Draft = "";
            if (SiteOrderInfo.isOrderApproved == 0) { Draft = "Draft"; }
            //if (SiteOrderInfo.Status == "AT") { Draft = "Draft"; }
            Draft = "";       
            reportParameters.Add(new ReportParameter("Draft", UI.Utils.SetFormString(Draft)));    // DS20230411
            // localReport.ReportPath = Web.Utils.ReportsPath + "\\SiteOrderMAT.rdlc";  //DS20231010
            localReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "\\SiteOrderMAT.rdlc";  //DS20231010
            localReport.DataSources.Add(new ReportDataSource("SOS_Core_SiteOrderDetailInfo", SiteOrderInfo.Items));
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());

                throw new Exception("Generating Site Order Report");

            }



        }

        public Byte[] GenerateSiteOrderReportIns(SiteOrderInfo SiteOrderInfo, ProjectInfo ProjectInfo, SubContractorInfo SubContractorInfo, PeopleInfo ForemanPeopleInfo, PeopleInfo ContactPeopleInfo, PeopleInfo GivenByPeopleInfo, string BudgetName, ProjectTradesInfo ProjectTradesInfo)
        {
            // DS20230822
            LocalReport localReport = new LocalReport();
            string Letterhead = "";
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + "templates\\lh\\";
            //Server.MapPath(string.Format("~/Files/{0}", fileName));
            //Content Type and Header.
            string fileName = filePath + "LH_" + ProjectInfo.BusinessUnitName + ".txt";
            if (!File.Exists(fileName)) fileName = filePath + "LH_ALL.txt";
            if (File.Exists(fileName)) Letterhead = File.ReadAllText(fileName);
            string SiteOrder = "D" + Int32.Parse(SiteOrderInfo.IdStr).ToString("000000");
            string ProjectNo = ProjectInfo.Year.ToString() + "-" + ProjectInfo.Number.ToString();
            String OrderRef = "";                           //DS20230311   
            if (SiteOrderInfo.Items.Count == 0)
            {
                string Notes = SiteOrderInfo.Notes.Replace("\r\n", "\n");
                string[] SNotes = (SiteOrderInfo.Notes).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);   //DS20230309
                for (int i = 0; i < SNotes.Length; i++)
                {
                    SiteOrderDetailInfo SOD = new SiteOrderDetailInfo();
                    SOD.Id = (int)Data.Utils.GetDBInt32(i);
                    SOD.Title = Data.Utils.GetDBString(SNotes[i]);
                    SOD.Qty = null;
                    //SOD.UM = Data.Utils.GetDBString(0);
                    //SOD.Price = (Decimal)Data.Utils.GetDBDecimal(0);
                    //SOD.Amount = (Decimal)Data.Utils.GetDBDecimal(0);
                    SiteOrderInfo.Items.Add(SOD);
                    //Console.WriteLine("{0}: {1}", i, splitted[i]);
                }
                SiteOrderInfo.Notes = "";
                //parameters.Add(UI.Utils.GetFormString(Search[0]));   //DS20230307
            }
            BudgetName = BudgetName + " - " + SiteOrderInfo.OrderCode + " " + SiteOrderInfo.TypeInfo;   //DS202317
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            //SiteOrderInfo.Title = SiteOrderInfo.Title.Replace(",", " ");    // DS20230822
            if (SiteOrderInfo.SubContractorId == 0)
            {
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
                reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SiteOrderInfo.Name)));
                // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
                reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
                reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
                // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
                reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title)));
                //reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString("DEMO").ToString()));
                reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SiteOrderInfo.Street)));
                reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SiteOrderInfo.Locality + ", " + SiteOrderInfo.State + ", " + SiteOrderInfo.PostalCode)));
                // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
                // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));

                reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
                reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
                reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
                reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(SiteOrderInfo.Contact)));
                // DS20230304 
                reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
                reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
                reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
            }
            else
            {
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
                reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SubContractorInfo.Name)));
                // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
                reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
                reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
                // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
                reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title)));
                reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SubContractorInfo.Street)));
                reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SubContractorInfo.Locality + ", " + SubContractorInfo.State + ", " + SubContractorInfo.PostalCode)));
                // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
                // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));
                reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
                reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
                reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
                reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
            }
                if (ContactPeopleInfo == null)
                {
                    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString("")));
                }
                else
                {
                    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(ContactPeopleInfo.Name)));
                }
                // DS20230304 

                reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
                //reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                if (GivenByPeopleInfo == null)
                {
                    reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString("")));
                }
                else
                {
                    reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                }

                reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
                String WO = "";
                if (ProjectTradesInfo != null)
                {
                    BudgetName = ProjectTradesInfo.Code + " - " + ProjectTradesInfo.Name;   //DS20230822
                }

                //if (ProjectInfo.ContractTrades != null)   //DS20230822
                //{

                //    foreach (TradeInfo tradeInfo in ProjectInfo.ContractTrades)       // DS20230411 >>
                //    {
                //        if (SiteOrderInfo.SubContractorId == tradeInfo.SelectedSubContractor.Id)
                //        {
                //            WO = "-" + tradeInfo.WorkOrderNumber.ToString();
                //            break;
                //        }
                //    }
                //}
                OrderRef = "SUBCONTRACTOR REF: (" + ProjectInfo.Number.ToString() + ")" + WO;
                // DS20230411 <<

            reportParameters.Add(new ReportParameter("OrderRef", UI.Utils.SetFormString(OrderRef)));    // DS20230411
            string Draft = "";
            if (SiteOrderInfo.isOrderApproved == 0) { Draft = "Draft"; }
            //if (SiteOrderInfo.Status == "AT") { Draft = "Draft"; }
            Draft = "";
            reportParameters.Add(new ReportParameter("Draft", UI.Utils.SetFormString(Draft)));    // DS20230411
            //localReport.ReportPath = Web.Utils.ReportsPath + "\\SiteOrderINS.rdlc";//DS20231010
            localReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "\\SiteOrderINS.rdlc";  //DS20231010
            localReport.DataSources.Add(new ReportDataSource("SOS_Core_SiteOrderDetailInfo", SiteOrderInfo.Items));
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());

                throw new Exception("Generating Site Order Report");

            }

            int xx = 1;

        }



        //public Byte[] GenerateSiteOrderReportIns(SiteOrderInfo SiteOrderInfo, ProjectInfo ProjectInfo, SubContractorInfo SubContractorInfo, PeopleInfo ForemanPeopleInfo, PeopleInfo ContactPeopleInfo, PeopleInfo GivenByPeopleInfo)
        //{

        //    LocalReport localReport = new LocalReport();


        //    List<ReportParameter> reportParameters = new List<ReportParameter>();
        //    if (SiteOrderInfo.SubContractorId == 0)
        //    {
        //        reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
        //        reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SiteOrderInfo.Name)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
        //        reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(SiteOrderInfo.ProjectId.ToString())));
        //        reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
        //        // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
        //        reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SiteOrderInfo.Street)));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SiteOrderInfo.Locality + ", " + SiteOrderInfo.State + ", " + SiteOrderInfo.PostalCode)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));
        //        reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
        //        reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrderInfo.IdStr)));
        //        reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
        //        reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
        //        reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(SiteOrderInfo.Contact)));
        //        reportParameters.Add(new ReportParameter("VariationId", UI.Utils.SetFormString(SiteOrderInfo.VariationID.ToString())));
        //        // DS20230304 
        //        reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
        //    }
        //    else 
        //    {   
        //    reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
        //    reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SubContractorInfo.Name)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
        //        reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(SiteOrderInfo.ProjectId.ToString())));
        //    reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
        //        // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
        //        reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
        //    reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SubContractorInfo.Street)));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SubContractorInfo.Locality + ", " + SubContractorInfo.State + ", " + SubContractorInfo.PostalCode)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));
        //        reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
        //    reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrderInfo.IdStr)));
        //    reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
        //    reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
        //    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(ContactPeopleInfo.Name)));
        //    reportParameters.Add(new ReportParameter("VariationId", UI.Utils.SetFormString(SiteOrderInfo.VariationID.ToString())));
        //        // DS20230304 
        //        reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
        //    }

        //    localReport.ReportPath = Web.Utils.ReportsPath + "\\SiteOrderINS.rdlc";
        //    localReport.DataSources.Add(new ReportDataSource("SOS_Core_SiteOrderDetailInfo", SiteOrderInfo.Items));
        //    localReport.EnableExternalImages = true;
        //    localReport.SetParameters(reportParameters);

        //    try
        //    {
        //        return UI.Utils.RdlcToPdf(localReport);
        //    }
        //    catch (Exception Ex)
        //    {
        //        Utils.LogError(Ex.ToString());

        //        throw new Exception("Generating Site Order Report");

        //    }

        public Byte[] GenerateSiteOrderReportHir(SiteOrderInfo SiteOrderInfo, ProjectInfo ProjectInfo, SubContractorInfo SubContractorInfo, PeopleInfo ForemanPeopleInfo, PeopleInfo ContactPeopleInfo, PeopleInfo GivenByPeopleInfo, string BudgetName, ProjectTradesInfo ProjectTradesInfo)
        {
            // DS20230822
            LocalReport localReport = new LocalReport();
            string Letterhead = "";
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + "templates\\lh\\";
            //Server.MapPath(string.Format("~/Files/{0}", fileName));
            //Content Type and Header.
            string fileName = filePath + "LH_" + ProjectInfo.BusinessUnitName + ".txt";
            if (!File.Exists(fileName)) fileName = filePath + "LH_ALL.txt";
            if (File.Exists(fileName)) Letterhead = File.ReadAllText(fileName);
            string SiteOrder = "D" + Int32.Parse(SiteOrderInfo.IdStr).ToString("000000");
            string ProjectNo = ProjectInfo.Year.ToString() + "-" + ProjectInfo.Number.ToString();
            //SiteOrderInfo.Title = SiteOrderInfo.Title.Replace(",", " ");    // DS20230822
            BudgetName = BudgetName + " - " + SiteOrderInfo.OrderCode + " " + SiteOrderInfo.TypeInfo;   //DS202317
            String OrderRef = "";                           //DS20230311   
            if (SiteOrderInfo.Items.Count == 0)
            {
                string Notes = SiteOrderInfo.Notes.Replace("\r\n", "\n");
                string[] SNotes = (SiteOrderInfo.Notes).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);   //DS20230309
                for (int i = 0; i < SNotes.Length; i++)
                {
                    SiteOrderDetailInfo SOD = new SiteOrderDetailInfo();
                    SOD.Id = (int)Data.Utils.GetDBInt32(i);
                    SOD.Title = Data.Utils.GetDBString(SNotes[i]);
                    SOD.Qty = null;
                    //SOD.UM = Data.Utils.GetDBString(0);
                    //SOD.Price = (Decimal)Data.Utils.GetDBDecimal(0);
                    //SOD.Amount = (Decimal)Data.Utils.GetDBDecimal(0);
                    SiteOrderInfo.Items.Add(SOD);
                    //Console.WriteLine("{0}: {1}", i, splitted[i]);
                }
                SiteOrderInfo.Notes = "";
                //parameters.Add(UI.Utils.GetFormString(Search[0]));   //DS20230307
            }
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            if (SiteOrderInfo.SubContractorId == 0)
            {
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
                reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SiteOrderInfo.Name)));
                // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
                reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
                reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
                // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
                reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
                reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SiteOrderInfo.Street)));
                reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SiteOrderInfo.Locality + ", " + SiteOrderInfo.State + ", " + SiteOrderInfo.PostalCode)));
                // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
                // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));

                reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
                reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
                reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
                reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(SiteOrderInfo.Contact)));
                // DS20230304 
                reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
                reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
                reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));

            }
            else
            {
                reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
                reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SubContractorInfo.Name)));
                // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
                reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
                reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
                // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
                reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
                reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SubContractorInfo.Street)));
                reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SubContractorInfo.Locality + ", " + SubContractorInfo.State + ", " + SubContractorInfo.PostalCode)));
                // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
                // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));
                reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
                reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
                reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
                reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
                reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
                if (ContactPeopleInfo == null)
                {
                    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString("")));
                }
                else
                {
                    reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(ContactPeopleInfo.Name)));
                }
                // DS20230304 
                reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
                //reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                if (GivenByPeopleInfo == null)
                {
                    reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString("")));
                }
                else
                {
                    reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
                }

                reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
                String WO = "";
                if (ProjectInfo.ContractTrades != null)
                {

                    foreach (TradeInfo tradeInfo in ProjectInfo.ContractTrades)       // DS20230411 >>
                    {
                        if (SiteOrderInfo.SubContractorId == tradeInfo.SelectedSubContractor.Id)
                        {
                            WO = "-" + tradeInfo.WorkOrderNumber.ToString();
                            break;
                        }
                    }
                }
                OrderRef = "SUBCONTRACTOR REF: (" + ProjectInfo.Number.ToString() + ")" + WO;                // DS20230411 <<

            }
            reportParameters.Add(new ReportParameter("DateStart", UI.Utils.SetFormDate(SiteOrderInfo.DateStart)));
            reportParameters.Add(new ReportParameter("DateEnd", UI.Utils.SetFormDate(SiteOrderInfo.DateEnd)));
            reportParameters.Add(new ReportParameter("OrderRef", UI.Utils.SetFormString(OrderRef)));    // DS20230411            reportParameters.Add(new ReportParameter("DateStart", UI.Utils.SetFormDate(SiteOrderInfo.DateStart)));
            string Draft = "";
            if (SiteOrderInfo.isOrderApproved == 0) { Draft = "Draft"; }
            //if (SiteOrderInfo.Status == "AT") { Draft = "Draft"; }
            Draft = "";
            reportParameters.Add(new ReportParameter("Draft", UI.Utils.SetFormString(Draft)));    // DS20230411
            // localReport.ReportPath = Web.Utils.ReportsPath + "\\SiteOrderHIR.rdlc";  //DS20231010
            localReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "\\SiteOrderHIR.rdlc";  //DS20231010
            // localReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportsPath"].ToString() + "\\SiteOrderHIR.rdlc";  //DS20231010
            localReport.DataSources.Add(new ReportDataSource("SOS_Core_SiteOrderDetailInfo", SiteOrderInfo.Items));
            localReport.EnableExternalImages = true;
            localReport.SetParameters(reportParameters);

            try
            {
                return UI.Utils.RdlcToPdf(localReport);
            }
            catch (Exception Ex)
            {
                Utils.LogError(Ex.ToString());

                throw new Exception("Generating Equipment Hire Report");

            }



        }

        //}
        //public Byte[] GenerateSiteOrderReportHir(SiteOrderInfo SiteOrderInfo, ProjectInfo ProjectInfo, SubContractorInfo SubContractorInfo, PeopleInfo ForemanPeopleInfo, PeopleInfo ContactPeopleInfo, PeopleInfo GivenByPeopleInfo)
        //{

        //    LocalReport localReport = new LocalReport();


        //    List<ReportParameter> reportParameters = new List<ReportParameter>();
        //    if (SiteOrderInfo.SubContractorId == 0)
        //    {
        //        reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
        //        reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SiteOrderInfo.Name)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
        //        reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
        //        reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
        //        // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
        //        reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SiteOrderInfo.Street)));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SiteOrderInfo.Locality + ", " + SiteOrderInfo.State + ", " + SiteOrderInfo.PostalCode)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));

        //        reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
        //        reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
        //        reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
        //        reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
        //        reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(SiteOrderInfo.Contact)));
        //        // DS20230304 
        //        reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
        //        reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
        //        reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));
        //        reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
        //    }
        //    else
        //    {
        //        reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(ProjectInfo.Name)));
        //        reportParameters.Add(new ReportParameter("SubContractorName", UI.Utils.SetFormString(SubContractorInfo.Name)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("ContactPH", UI.Utils.SetFormString(SiteOrderInfo.ContactPhone)));
        //        reportParameters.Add(new ReportParameter("ProjectId", UI.Utils.SetFormString(ProjectNo)));
        //        reportParameters.Add(new ReportParameter("OrderDate", UI.Utils.SetFormDate(SiteOrderInfo.OrderDate)));
        //        // DS20230306 reportParameters.Add(new ReportParameter("DeliveryFee", UI.Utils.SetFormEditDecimal(SiteOrderInfo.DeliveryFee)));
        //        reportParameters.Add(new ReportParameter("ProjectTrade", UI.Utils.SetFormString(SiteOrderInfo.Title.ToString())));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress1", UI.Utils.SetFormString(SubContractorInfo.Street)));
        //        reportParameters.Add(new ReportParameter("SubContractorAddress2", UI.Utils.SetFormString(SubContractorInfo.Locality + ", " + SubContractorInfo.State + ", " + SubContractorInfo.PostalCode)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("GST", UI.Utils.SetFormEditDecimal(SiteOrderInfo.GST)));
        //        // DS20230304 reportParameters.Add(new ReportParameter("Total", UI.Utils.SetFormEditDecimal(SiteOrderInfo.Total)));
        //        reportParameters.Add(new ReportParameter("SubTotal", UI.Utils.SetFormEditDecimal(SiteOrderInfo.SubTotal)));
        //        reportParameters.Add(new ReportParameter("OrderNo", UI.Utils.SetFormString(SiteOrder)));
        //        reportParameters.Add(new ReportParameter("ForemanPH", UI.Utils.SetFormString(ForemanPeopleInfo.Phone)));
        //        reportParameters.Add(new ReportParameter("ForemanName", UI.Utils.SetFormString(ForemanPeopleInfo.Name)));
        //        reportParameters.Add(new ReportParameter("Letterhead", UI.Utils.SetFormString(Letterhead)));
        //        if (ContactPeopleInfo == null)
        //        {
        //            reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString("")));
        //        }
        //        else
        //        {
        //            reportParameters.Add(new ReportParameter("Contact", UI.Utils.SetFormString(ContactPeopleInfo.Name)));
        //        }
        //        // DS20230304 
        //        reportParameters.Add(new ReportParameter("Info", UI.Utils.SetFormString(SiteOrderInfo.Notes)));
        //        //reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
        //        if (GivenByPeopleInfo == null)
        //        {
        //            reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString("")));
        //        }
        //        else
        //        {
        //            reportParameters.Add(new ReportParameter("GivenByName", UI.Utils.SetFormString(GivenByPeopleInfo.Name)));
        //        }

        //        reportParameters.Add(new ReportParameter("BudgetName", UI.Utils.SetFormString(BudgetName)));

        //    }

        //    localReport.ReportPath = Web.Utils.ReportsPath + "\\SiteOrderHIR.rdlc";
        //    localReport.DataSources.Add(new ReportDataSource("SOS_Core_SiteOrderDetailInfo", SiteOrderInfo.Items));
        //    localReport.EnableExternalImages = true;
        //    localReport.SetParameters(reportParameters);

        //    try
        //    {
        //        return UI.Utils.RdlcToPdf(localReport);
        //    }
        //    catch (Exception Ex)
        //    {
        //        Utils.LogError(Ex.ToString());

        //        throw new Exception("Generating Site Order Report");

        //    }



        //}
        public static SiteOrdersController GetInstance()
        {
            if (instance == null)
            {
                return new SiteOrdersController();
            }
            return instance;
        }

        #region SiteOrderApprovals
        public int ApproveDraftOrder(SiteOrderInfo SiteOrderInfo)
        {
            SiteOrderApprovalsInfo SiteOrderApprovalsInfo = SiteOrderApprovalsGetCurrent((int)SiteOrderInfo.Id);
            if (SiteOrderApprovalsInfo != null)
            {
                int UserId = (int)Web.Utils.GetCurrentUserId();
                if (UserId == SiteOrderApprovalsInfo.AssignedPeopleId && SiteOrderApprovalsInfo.Process == "DOS")
                {
                    SiteOrderApprovalsSearchInfo SOA = null;
                    SOA = new SiteOrderApprovalsSearchInfo();
                    SOA.Id = SiteOrderApprovalsInfo.Id;
                    SOA.OrderId = SiteOrderApprovalsInfo.OrderId;
                    SOA.isApprovalCurrent = 0;
                    SOA.Status = "A";
                    UpdateSiteOrderApprovalStatus(SOA);
                    if (SiteOrderInfo.Typ == "Hir")
                    {
                        int rc = (int)AddSiteOrderApprovalsHireProcess(SiteOrderInfo);
                    }
                    else
                    {
                        int rc = (int)AddSiteOrderApprovalsProcess(SiteOrderInfo);
                    }
                    return 1;

                }

            }
            return 0;
        }
        public SiteOrderApprovalsInfo SiteOrderApprovalsGetCurrent(int OrderId)
        {
            SiteOrderApprovalsInfo SSA = new SiteOrderApprovalsInfo();
            IDataReader dr = null;
            List<SiteOrderApprovalsInfo> SiteOrderApprovalsInfoList = new List<SiteOrderApprovalsInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(OrderId);

            try
            {
                dr = Data.DataProvider.GetInstance().SiteOrderApprovalsGetCurrent(parameters.ToArray());
                //parameters.ToArray()
                // bool AllowEditSet = false;
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                   
                    SSA.Id = Data.Utils.GetDBInt32(dr["ApprovalID"]);
                    SSA.OrderId = (int)Data.Utils.GetDBInt32(dr["OrderID"]);
                    SSA.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    SSA.Title = Data.Utils.GetDBString(dr["Title"]);
                    SSA.AssignedPeopleId = (int)Data.Utils.GetDBInt32(dr["AssignedPeopleId"]);
                    SSA.AssignedDate = (DateTime)Data.Utils.GetDBDateTime(dr["AssignedDate"]);
                    SSA.RoleId = Data.Utils.GetDBString(dr["RoleID"]);
                    SSA.Process = Data.Utils.GetDBString(dr["Process"]);
                    SSA.ApprovalStatus = Data.Utils.GetDBString(dr["ApprovalStatus"]);
                    SSA.isApprovalCurrent = (int)Data.Utils.GetDBInt32(dr["isApprovalCurrent"]);
                    return SSA;

                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Docs from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SSA;
        }
        public void UpdateSiteOrderApprovalStatus(SiteOrderApprovalsSearchInfo SiteOrderApprovalsSearchInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(SiteOrderApprovalsSearchInfo);
            parameters.Add(SiteOrderApprovalsSearchInfo.Id);
            parameters.Add(SiteOrderApprovalsSearchInfo.OrderId);
            parameters.Add(SiteOrderApprovalsSearchInfo.isApprovalCurrent);
            parameters.Add(SiteOrderApprovalsSearchInfo.Status);
            parameters.Add(SiteOrderApprovalsSearchInfo.Reason);
            parameters.Add(SiteOrderApprovalsSearchInfo.ModifiedDate);
            parameters.Add(SiteOrderApprovalsSearchInfo.ModifiedBy);
            Data.DataProvider.GetInstance().UpdateSiteOrderApprovalStatus(parameters.ToArray());
            try
            {


                //#--
                //if (siteOrderInfo != null)
                //{

                //    Data.DataProvider.GetInstance().DeleteSubContractorBusinessUnitList(siteOrderInfo.Id);


                //    foreach (BusinessUnitInfo bUnit in siteOrderInfo.BusinessUnitslist)
                //    {
                //        List<Object> parameters1 = new List<Object>();
                //        parameters1.Add(subContractorInfo.Id);
                //        parameters1.Add(bUnit.Id);
                //        Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                //    }
                //} //#---

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updation Site Order in database");
            }
        }
        public int SearchSiteOrderApprovalsCount(int UserId)
        {
            IDataReader dr = null;
            List<SiteOrderApprovalsSearchInfo> SiteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsSearchInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(UserId);
            int CNT = 0;

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrderApprovalsCount(parameters.ToArray());
                //parameters.ToArray()
                // bool AllowEditSet = false;
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderApprovalsSearchInfo SSA = new SiteOrderApprovalsSearchInfo();
                    CNT = (int)Data.Utils.GetDBInt32(dr["CNT"]);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Approvals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return CNT;
        }
        public List<SiteOrderApprovalsSearchInfo> SearchSiteOrderApprovalsAll(int UserId)
        {
            IDataReader dr = null;
            List<SiteOrderApprovalsSearchInfo> SiteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsSearchInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(UserId);

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrderApprovalsAll(parameters.ToArray());
                //parameters.ToArray()
               // bool AllowEditSet = false;
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderApprovalsSearchInfo SSA = new SiteOrderApprovalsSearchInfo();
                    SSA.Id = Data.Utils.GetDBInt32(dr["ApprovalID"]);
                    SSA.OrderId = (int)Data.Utils.GetDBInt32(dr["OrderID"]);
                    SSA.OrderTitle = Data.Utils.GetDBString(dr["OrderTitle"]);
                    SSA.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    SSA.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);
                    SSA.Title = Data.Utils.GetDBString(dr["Title"]);
                    SSA.AssignedTo = Data.Utils.GetDBString(dr["AssignedTo"]);
                    SSA.AssignedPeopleId = (int)Data.Utils.GetDBInt32(dr["AssignedPeopleId"]);
                    SSA.Reason = Data.Utils.GetDBString(dr["Reason"]); // DS20230304
                    SSA.BusinessUnitName = Data.Utils.GetDBString(dr["BusinessUnitName"]); // DS20230620

                    if (dr["ApprovedBy"] == DBNull.Value)
                    {
                        SSA.ApprovedBy = "";
                    }
                    else
                    {
                        SSA.ApprovedBy = Data.Utils.GetDBString(dr["ApprovedBy"]);
                    }

                    if (dr["AssignedDate"] == DBNull.Value)
                    {
                        SSA.AssignedDate = "";
                    }
                    else
                    {
                        DateTime tmpDate = (DateTime)Data.Utils.GetDBDateTime(dr["AssignedDate"]);
                        SSA.AssignedDate = tmpDate.ToString("dd/MM/yyyy hh:mm");
                    }
                    if (dr["ApprovedDate"] == DBNull.Value)
                    {
                        SSA.ApprovedDate = "";
                    }
                    else
                    {
                        DateTime tmpDate = (DateTime)Data.Utils.GetDBDateTime(dr["ApprovedDate"]);
                        SSA.ApprovedDate = tmpDate.ToString("dd/MM/yyyy hh:mm");
                    }
                    SSA.Status = Data.Utils.GetDBString(dr["ApprovalStatus"]);
                    SSA.StatusDescription = "";
                    if (SSA.Status == "A")
                    {
                        SSA.StatusDescription = "Approved";
                        SSA.Approve = true;
                    }
                    if (SSA.Status == "R")
                    {
                        SSA.StatusDescription = "Rejected";
                        SSA.Reject = true;
                    }

                    if (SSA.Status == "R") SSA.StatusDescription = "Rejected";
                    if (SSA.Status == "C") SSA.StatusDescription = "Canceled";
                    //
                    // Set First Edit
                    //
                    SSA.isApprovalCurrent = (int)Data.Utils.GetDBInt32(dr["isApprovalCurrent"]);
                    //if (SSA.Status == "" & AllowEditSet == false)
                    //{
                    //    if (SSA.AssignedPeopleId == (int)Web.Utils.GetCurrentUserId()) SSA.AllowEdit = true;
                    //    AllowEditSet = true;
                    //}
                    //else
                    //{
                    //    SSA.AllowEdit = false;
                    //}
                    SSA.SubTotal = (decimal)Data.Utils.GetDBDecimal(dr["SubTotal"]);
                    SSA.OrderTyp = Data.Utils.GetDBString(dr["OrderTyp"]);
                    if (SSA.OrderTyp == "Mat") SSA.OrderTyp = "MO";
                    if (SSA.OrderTyp == "Hir") SSA.OrderTyp = "EH";
                    if (SSA.OrderTyp == "Ins") SSA.OrderTyp = "SI";
                    SSA.OrderDateEnd = (DateTime)Data.Utils.GetDBDateTime(dr["OrderDateEnd"]);
                    if (SSA.OrderTyp == "EH")
                    {
                        SSA.RowColor = "Orange";
                        if (DateTime.Now.AddDays(7) > SSA.OrderDateEnd)
                        {
                            SSA.RowColor = "Red";
                        }
                    }
                    else
                    {
                        SSA.RowColor = "White";
                    }
                    if (SSA.SubTotal == 0)
                    {
                        SSA.ValueLimit = "Z";
                    }
                    else
                    {
                        if (SSA.SubTotal > 3000)
                        {
                            SSA.ValueLimit = "L";
                        }
                        else
                        { 
                            SSA.ValueLimit = "";
                        }
                    }
                    SSA.DocCount = (int)Data.Utils.GetDBInt32(dr["DocCount"]);   // DS20230710
                    SiteOrderApprovalsSearchInfoList.Add(SSA);
                  
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Docs from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SiteOrderApprovalsSearchInfoList;
        }
        public int SiteOrderGetApprovalStatus(int OrderId)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();
            parameters.Add(OrderId);
            int Approved = 0;
            try
            {
                dr = Data.DataProvider.GetInstance().SiteOrderApprovalStatus(parameters.ToArray());
                //parameters.ToArray()
                // bool AllowEditSet = false;
              
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderApprovalsSearchInfo SSA = new SiteOrderApprovalsSearchInfo();
                    Approved = (int)Data.Utils.GetDBInt32(dr["Approved"]);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Approvals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return Approved;
        }
        // 
        // Get Site Orders Awaiting Email  DS20231010
        //
        public List<SiteOrderApprovalsInfo> SearchSiteOrderApprovalsEmailList()
        {
            IDataReader dr = null;
            List<SiteOrderApprovalsInfo> SiteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsInfo>();
            List<Object> parameters = new List<Object>();

            try
            {
                dr = Data.DataProvider.GetInstance().SiteOrderApprovalsEmailList(parameters.ToArray());
                //parameters.ToArray()
                // bool AllowEditSet = false;
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderApprovalsInfo SSA = new SiteOrderApprovalsInfo();
                    SSA.Id = Data.Utils.GetDBInt32(dr["ApprovalID"]);
                    SSA.OrderId = (int)Data.Utils.GetDBInt32(dr["OrderID"]);
                    SSA.Reason = Data.Utils.GetDBString(dr["Reason"]);
                    SSA.CreatedBy = (int)Data.Utils.GetDBInt32(dr["CreatedPeopleId"]);
                    SSA.CreatedDate = (DateTime)Data.Utils.GetDBDateTime(dr["CreatedDate"]);
                    SiteOrderApprovalsSearchInfoList.Add(SSA);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Approvals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SiteOrderApprovalsSearchInfoList;
        }
        // 
        // Daily Maintenance  DS20231025
        //
        public void MaintenanceDaily()
        {
            List<SiteOrderApprovalsInfo> SiteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsInfo>();
            List<Object> parameters = new List<Object>();
            try
            {
                Data.DataProvider.GetInstance().MaintenanceDaily(parameters.ToArray());

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Maintenance Daily Error");
            }
            finally
            {
            }

            return;
        }
        // 
        // Get Site Orders Awaiting Email  DS20231010
        //
        public void SiteOrderApprovalsUpdateReason(SiteOrderApprovalsInfo SOA)
        {
            IDataReader dr = null;
            List<SiteOrderApprovalsInfo> SiteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(SOA.Id);
            parameters.Add(SOA.OrderId);
            parameters.Add(SOA.Reason);
            parameters.Add(SOA.CreatedDate);
            parameters.Add(SOA.CreatedBy);
            try
            {
                dr = Data.DataProvider.GetInstance().SiteOrderApprovalsUpdateReason(parameters.ToArray());
 
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating SiteOrder Approvals reason");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return ;
        }
        public List<SiteOrderApprovalsSearchInfo> SearchSiteOrderApprovals(int OrderId)
        {
            IDataReader dr = null;
            List<SiteOrderApprovalsSearchInfo> SiteOrderApprovalsSearchInfoList = new List<SiteOrderApprovalsSearchInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(OrderId);

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrderApprovals(parameters.ToArray());
                //parameters.ToArray()
                // bool AllowEditSet = false;
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderApprovalsSearchInfo SSA = new SiteOrderApprovalsSearchInfo();
                    SSA.Id = Data.Utils.GetDBInt32(dr["ApprovalID"]);
                    SSA.OrderId = (int)Data.Utils.GetDBInt32(dr["OrderID"]);
                    SSA.OrderTitle = Data.Utils.GetDBString(dr["OrderTitle"]);
                    SSA.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    SSA.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);
                    SSA.Title = Data.Utils.GetDBString(dr["Title"]);
                    SSA.Process = Data.Utils.GetDBString(dr["Process"]);  // DS20231128
                    SSA.AssignedTo = Data.Utils.GetDBString(dr["AssignedTo"]);
                    SSA.AssignedPeopleId = (int)Data.Utils.GetDBInt32(dr["AssignedPeopleId"]);
                    if (dr["ApprovedBy"] == DBNull.Value)
                    {
                        SSA.ApprovedBy = "";
                    }
                    else
                    {
                        SSA.ApprovedBy = Data.Utils.GetDBString(dr["ApprovedBy"]);
                    }

                    if (dr["AssignedDate"] == DBNull.Value)
                    {
                        SSA.AssignedDate = "";
                    }
                    else
                    {
                        DateTime tmpDate = (DateTime)Data.Utils.GetDBDateTime(dr["AssignedDate"]);
                        SSA.AssignedDate = tmpDate.ToString("dd/MM/yyyy hh:mm");
                    }
                    if (dr["ApprovedDate"] == DBNull.Value)
                    {
                        SSA.ApprovedDate = "";
                    }
                    else
                    {
                        DateTime tmpDate = (DateTime)Data.Utils.GetDBDateTime(dr["ApprovedDate"]);
                        SSA.ApprovedDate = tmpDate.ToString("dd/MM/yyyy hh:mm");
                    }
                    SSA.Status = Data.Utils.GetDBString(dr["ApprovalStatus"]);
                    SSA.StatusDescription = "";
                    if (SSA.Status == "A")
                    {
                        SSA.StatusDescription = "Approved";
                        SSA.Approve = true;
                    }
                    if (SSA.Status == "R")
                    {
                        SSA.StatusDescription = "Rejected";
                        SSA.Reject = true;
                    }

                    if (SSA.Status == "R") SSA.StatusDescription = "Rejected";
                    if (SSA.Status == "C") SSA.StatusDescription = "Canceled";
                    //
                    // Set First Edit
                    //
                    SSA.isApprovalCurrent = (int)Data.Utils.GetDBInt32(dr["isApprovalCurrent"]);
                    SSA.Reason = Data.Utils.GetDBString(dr["Reason"]);  
                    SSA.SubTotal = (decimal)Data.Utils.GetDBDecimal(dr["SubTotal"]);
                    SSA.OrderTyp = Data.Utils.GetDBString(dr["OrderTyp"]);
                    if (SSA.OrderTyp == "Mat") SSA.OrderTyp = "MO";
                    if (SSA.OrderTyp == "Hir") SSA.OrderTyp = "EH";
                    if (SSA.OrderTyp == "Ins") SSA.OrderTyp = "SI";
                    SSA.OrderDateEnd = (DateTime)Data.Utils.GetDBDateTime(dr["OrderDateEnd"]);
                    if (SSA.OrderTyp == "EH")
                    {
                        SSA.RowColor = "Orange";
                        if (SSA.OrderDateEnd.AddDays(7) > DateTime.Now)
                        {
                            SSA.RowColor = "Red";
                        }
                    }
                    else
                    {
                        SSA.RowColor = "White";
                    }
                    if (SSA.SubTotal == 0)
                    {
                        SSA.ValueLimit = "Z";
                    }
                    else
                    {
                        if (SSA.SubTotal > 3000)
                        {
                            SSA.ValueLimit = "L";
                        }
                        else
                        {
                            SSA.ValueLimit = "";
                        }
                    }
                    SiteOrderApprovalsSearchInfoList.Add(SSA);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Approvals from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SiteOrderApprovalsSearchInfoList;
        }
        #endregion
        public int SearchSiteOrdersUserCount(String strSearch, String strSiteOrderType, bool chkHistory)
        {
            List<String> BU = GetBusinessUnits();       //DS20230627
            List<String> UserSearch = new List<String>();
            List<String> BUSearch = new List<String>();
            List<Object> parameters = new List<Object>();   //DS20230307
            int UserId = (int)Web.Utils.GetCurrentUserId();
            string[] Search = (strSearch + ",,,").Split(',');   //DS20230627
            //foreach (String SearchItem in Search)
            //{
            //    if (BU.Contains(SearchItem.ToUpper()) == true && SearchItem != "") { BUSearch.Add(SearchItem); } else { UserSearch.Add(SearchItem); }
            //}
            foreach (String SearchItem in Search)    //DS20230824 >>>
            {
                string SearchParm = SearchItem;
                if (SearchItem.StartsWith("D") & SearchParm.Length > 1)
                {
                    String SearchPrefix = SearchParm.Substring(1, SearchParm.Length - 1);
                    long number1 = 0;
                    bool canConvert = long.TryParse(SearchPrefix, out number1);
                    if (canConvert == true) { SearchParm = number1.ToString(); }
                }
                if (BU.Contains(SearchParm.ToUpper()) == true && SearchParm != "")
                {
                    BUSearch.Add(SearchParm);
                }
                else
                {
                    UserSearch.Add(SearchParm);
                }
            }
            BUSearch.Add("");
            int ShowHistory = 0;
            if (chkHistory == true) { ShowHistory = 1; }
            parameters.Add(UserId);
            parameters.Add(UI.Utils.GetFormString(UserSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[1]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[2]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(BUSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(strSiteOrderType));
            parameters.Add(ShowHistory);

            try
            {
                return Data.DataProvider.GetInstance().SearchSiteOrdersCountUser(parameters.ToArray());
                // Data.DataProvider.GetInstance().SearchSiteOrdersCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders Count from database.");
            }
        }

        public List<SiteOrderInfo> SearchSiteOrdersUser(int startRowIndex, int maximumRows, String orderBy, String strSearch, String strSiteOrderType, bool chkHistory)
        {
            List<String> BU = GetBusinessUnits();       //DS20230627
            List<String> UserSearch = new List<String>();
            List<String> BUSearch = new List<String>();
            List<Object> parameters = new List<Object>();   //DS20230307
            int UserId = (int)Web.Utils.GetCurrentUserId();
            string[] Search = (strSearch + ",,,").Split(',');   //DS20230627
            //foreach (String SearchItem in Search)
            //{
            //    if (BU.Contains(SearchItem.ToUpper()) == true && SearchItem != "") { BUSearch.Add(SearchItem); } else { UserSearch.Add(SearchItem); }
            //}
            foreach (String SearchItem in Search)    //DS20230824 >>>
            {
                string SearchParm = SearchItem;
                if (SearchItem.StartsWith("D") & SearchParm.Length > 1)
                {
                    String SearchPrefix = SearchParm.Substring(1, SearchParm.Length - 1);
                    long number1 = 0;
                    bool canConvert = long.TryParse(SearchPrefix, out number1);
                    if (canConvert == true) { SearchParm = number1.ToString(); }
                }
                if (BU.Contains(SearchParm.ToUpper()) == true && SearchParm != "")
                {
                    BUSearch.Add(SearchParm);
                }
                else
                {
                    UserSearch.Add(SearchParm);
                }
            }
            BUSearch.Add("");
            int ShowHistory = 0;
            if (chkHistory == true) { ShowHistory = 1; }
            IDataReader dr = null;
            List<SiteOrderInfo> siteOrderInfoList = new List<SiteOrderInfo>();
            // parameterProjectId = Web.Utils.CheckParameter("ProjectId");

            parameters.Add(UserId);
            parameters.Add(UI.Utils.GetFormString(UserSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[1]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[2]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(BUSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(strSiteOrderType));
            parameters.Add(ShowHistory);
            parameters.Add(orderBy);
            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            //parameters.Add(UI.Utils.GetFormString("1"));
            //parameters.Add(UI.Utils.GetFormString(strSearch));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrdersUser(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderInfo SOI = new SiteOrderInfo();
                    SOI.Id = Data.Utils.GetDBInt32(dr["ID"]);
                    SOI.Title = Data.Utils.GetDBString(dr["Title"]);
                    SOI.Status = Data.Utils.GetDBString(dr["Status"]);
                    SOI.Typ = Data.Utils.GetDBString(dr["Type"]);
                    SOI.OrderDate = Data.Utils.GetDBDateTime(dr["Date"]);
                    SOI.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    SOI.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);
                    SOI.SubContractorName = Data.Utils.GetDBString(dr["SubContractorName"]);
                    SOI.ForemanName = Data.Utils.GetDBString(dr["ForemanName"]);
                    SOI.GivenByName = Data.Utils.GetDBString(dr["GivenByName"]);                // DS20230304
                    SOI.SubTotal = Data.Utils.GetDBDecimal(dr["SubTotal"]);                // DS20230304
                    SOI.DateStart = Data.Utils.GetDBDateTime(dr["DateStart"]);
                    SOI.DateEnd = Data.Utils.GetDBDateTime(dr["DateEnd"]);
                    SOI.BusinessUnitName = Data.Utils.GetDBString(dr["BusinessUnitName"]);  // 20230620
                    if (SOI.Typ == "Hir")
                    {
                        SOI.RowColor = "Orange";
                        if (DateTime.Now.AddDays(7) > SOI.DateEnd)
                        {
                            SOI.RowColor = "Red";
                        }
                    }
                    else
                    {
                        SOI.RowColor = "White";
                    }
                    SOI.DocCount = (int)Data.Utils.GetDBInt32(dr["DocCount"]);   // DS20230710
                    siteOrderInfoList.Add(SOI);
                    //   siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                }
                // siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello2", Status = "B", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello3", Status = "C", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello4", Status = "D", Type = "MAT", Date = DateTime.Now });
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return siteOrderInfoList;
        }
        public int SearchSiteOrdersCount(String strSearch, String strSiteOrderType,bool chkHistory)
        {
            int ShowHistory = 0;
            if (chkHistory == true) { ShowHistory = 1; }
            string[] Search = (strSearch + ",,,").Split(',');   //DS20230627
            List<Object> parameters = new List<Object>();

            parameters.Add(Web.Utils.CheckParameter("ProjectId"));
            parameters.Add(UI.Utils.GetFormString(Search[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(Search[1]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(Search[2]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(strSiteOrderType));
            parameters.Add(ShowHistory);
            try
            {
                return Data.DataProvider.GetInstance().SearchSiteOrdersCount(parameters.ToArray());
              // Data.DataProvider.GetInstance().SearchSiteOrdersCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders Count from database.");
            }
        }
        public int SearchSiteOrdersAllCount(String strSearch, String strSiteOrderType, bool chkHistory)
        {
            List<String> BU = GetBusinessUnits();       //DS20230627
            List<String> UserSearch = new List<String>();
            List<String> BUSearch = new List<String>();
            List<Object> parameters = new List<Object>();   //DS20230307
            int UserId = (int)Web.Utils.GetCurrentUserId();
            string[] Search = (strSearch + ",,,").Split(',');   //DS20230627
            //foreach (String SearchItem in Search)
            //{
            //    if (BU.Contains(SearchItem.ToUpper()) == true && SearchItem != "") { BUSearch.Add(SearchItem); } else { UserSearch.Add(SearchItem); }
            //}
            foreach (String SearchItem in Search)    //DS20230824 >>>
            {
                string SearchParm = SearchItem;
                if (SearchItem.StartsWith("D") & SearchParm.Length > 1)
                {
                    String SearchPrefix = SearchParm.Substring(1, SearchParm.Length - 1);
                    long number1 = 0;
                    bool canConvert = long.TryParse(SearchPrefix, out number1);
                    if (canConvert == true) { SearchParm = number1.ToString(); }
                }
                if (BU.Contains(SearchParm.ToUpper()) == true && SearchParm != "")
                {
                    BUSearch.Add(SearchParm);
                }
                else
                {
                    UserSearch.Add(SearchParm);
                }
            }
            BUSearch.Add("");
            int ShowHistory = 0;
            if (chkHistory == true) { ShowHistory = 1; }
            parameters.Add("");
            //parameters.Add(UI.Utils.GetFormString(strSearch));   //DS20230307
            parameters.Add(UI.Utils.GetFormString(UserSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[1]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[2]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(BUSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(strSiteOrderType));
            parameters.Add(ShowHistory);

            try
            {
                return Data.DataProvider.GetInstance().SearchSiteOrdersCountAll(parameters.ToArray());
                // Data.DataProvider.GetInstance().SearchSiteOrdersCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders Count from database.");
            }
        }
        #region SiteOrderDetail
        //public SiteOrderInfo CreateSiteOrderDetail(IDataReader dr)
        //{
        //    return CreateSiteOrderDetail(dr, null);
        //}
        /// <summary>
        /// Search for SubContractors in the database 
        /// </summary>
        public List<SiteOrderDetailInfo> GetSiteOrderDetails(int OrderId)
        {
            IDataReader dr = null;
            List<SiteOrderDetailInfo> SiteOrderDetailInfoList = new List<SiteOrderDetailInfo>();
            List<Object> parameters = new List<Object>();
            //parameterOrderId = (int)UI.Utils.GetFormInteger(OrderId);
           // parameterOrderId = Web.Utils.CheckParameter("OrderID");
            parameters.Add(OrderId);
            //parameters.Add(maximumRows);
            //parameters.Add(UI.Utils.GetFormString("1"));
            //parameters.Add(UI.Utils.GetFormString(strSearch));

            try
            {
                dr = Data.DataProvider.GetInstance().GetSiteOrderDetails(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderDetailInfo SOD = new SiteOrderDetailInfo();
                    SOD.Id = (int)Data.Utils.GetDBInt32(dr["ItemId"]);
                    SOD.Title = Data.Utils.GetDBString(dr["Title"]);
                    SOD.Date = (DateTime)Data.Utils.GetDBDateTime(dr["Date"]);
                    SOD.Qty = (Decimal)Data.Utils.GetDBDecimal(dr["Qty"]);
                    SOD.ETA = (DateTime)Data.Utils.GetDBDateTime(dr["ETA"]);
                    SOD.UM = Data.Utils.GetDBString(dr["UM"]);
                    SOD.Price = (Decimal)Data.Utils.GetDBDecimal(dr["Price"]);
                    SOD.Amount = (Decimal)Data.Utils.GetDBDecimal(dr["Amount"]);
                    SOD.Status = Data.Utils.GetDBString(dr["Status"]);
                    SOD.CreatedBy = (int)Data.Utils.GetDBInt32(dr["CreatedPeopleId"]);
                    SOD.ModifiedBy = (int)Data.Utils.GetDBInt32(dr["ModifiedPeopleId"]);
                    SOD.ModifiedDate = (DateTime)Data.Utils.GetDBDateTime(dr["ModifiedDate"]);
                    SOD.CreatedDate = (DateTime)Data.Utils.GetDBDateTime(dr["CreatedDate"]);    
                    SiteOrderDetailInfoList.Add(SOD);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Item Info from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SiteOrderDetailInfoList;
        }
        public SiteOrderDetailInfo GetSiteOrderDetail(int ItemID)
        {
            IDataReader dr = null;
            SiteOrderDetailInfo SOD = new SiteOrderDetailInfo();
            List<Object> parameters = new List<Object>();
            parameters.Add(ItemID);

            try
            {
                dr = Data.DataProvider.GetInstance().GetSiteOrderDetail(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SOD.Id = (int)Data.Utils.GetDBInt32(dr["ItemId"]);
                    SOD.Title = Data.Utils.GetDBString(dr["Title"]);
                    SOD.Date = (DateTime)Data.Utils.GetDBDateTime(dr["Date"]);
                    SOD.Qty = (Decimal)Data.Utils.GetDBDecimal(dr["Qty"]);
                    SOD.ETA = (DateTime)Data.Utils.GetDBDateTime(dr["ETA"]);
                    SOD.UM = Data.Utils.GetDBString(dr["UM"]);
                    SOD.Price = (Decimal)Data.Utils.GetDBDecimal(dr["Price"]);
                    SOD.Amount = (Decimal)Data.Utils.GetDBDecimal(dr["Amount"]);
                    SOD.Status = Data.Utils.GetDBString(dr["Status"]);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Item Info from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SOD;
        }
        public SiteOrderDetailInfo InitializeSiteOrderDetail(int OrderId)
        {
            SiteOrderDetailInfo SOD;
            SOD = new SiteOrderDetailInfo();
            SOD.Id = 0;
            SOD.OrderID = OrderId;
            SOD.Title = "";
            SOD.Date = DateTime.Today;
            SOD.Qty = 0;
            SOD.ETA = DateTime.Today;
            SOD.UM = "EA";
            SOD.Price = 0;
            SOD.Amount = 0;
            SOD.Status = "";
           

            return SOD;
        }
        public int? AddSiteOrderDetail(SiteOrderDetailInfo SOD)
        {
            int? ItemId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(SOD);
            parameters.Add(SOD.OrderID);
            parameters.Add(SOD.Title);
            parameters.Add(SOD.Date);
            parameters.Add(SOD.Qty);
            parameters.Add(SOD.ETA);
            parameters.Add(SOD.UM);
            parameters.Add(SOD.Price);
            parameters.Add(SOD.Amount);
            parameters.Add(SOD.Status);
            parameters.Add(SOD.CreatedDate);
            parameters.Add(SOD.CreatedBy);

            try
            {
                ItemId = Data.DataProvider.GetInstance().AddSiteOrderDetail(parameters.ToArray());

                //#--
                //if (subContractorId != null && siteOrderInfo.BusinessUnitslist.Count>0)
                //{  
                //    foreach (BusinessUnitInfo bUnit in subContractorInfo.BusinessUnitslist) {
                //     List<Object> parameters1 = new List<Object>();
                //        parameters1.Add(subContractorId);
                //        parameters1.Add(bUnit.Id);
                //    Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                //}
                //} //#---
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Site Order Detail in database");
            }

            //
            // UPDATE ORDER TOTALS
            //
            parameters.Clear();
            parameters.Add(SOD.OrderID);
            try
            {
                Data.DataProvider.GetInstance().SumSiteOrders(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Site Order Summary in database");
            }
            return ItemId;
        }

        /// <summary>
        /// Adds or updates a Subcontractor
        /// </summary>
        public int? AddUpdateSiteOrderDetail(SiteOrderDetailInfo SOD)
        {
            if (SOD != null)
            {
                if (SOD.Id != null)
                {
                    UpdateSiteOrderDetail(SOD);
                    return SOD.Id;
                }
                else
                {
                    return AddSiteOrderDetail(SOD);
                }
            }
            else
            {
                return null;
            }
        }
        public void UpdateSiteOrderDetail(SiteOrderDetailInfo SOD)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(SOD);
            parameters.Add(SOD.Id);
            parameters.Add(SOD.OrderID);
            parameters.Add(SOD.Title);
            parameters.Add(SOD.Date);
            parameters.Add(SOD.Qty);
            parameters.Add(SOD.ETA);
            parameters.Add(SOD.UM);
            parameters.Add(SOD.Price);
            parameters.Add(SOD.Amount);
            parameters.Add(SOD.Status);
            parameters.Add(SOD.ModifiedDate);
            parameters.Add(SOD.ModifiedBy);
            try
            {
                Data.DataProvider.GetInstance().UpdateSiteOrderDetail(parameters.ToArray());


                //#--
                //if (siteOrderInfo != null)
                //{

                //    Data.DataProvider.GetInstance().DeleteSubContractorBusinessUnitList(siteOrderInfo.Id);


                //    foreach (BusinessUnitInfo bUnit in siteOrderInfo.BusinessUnitslist)
                //    {
                //        List<Object> parameters1 = new List<Object>();
                //        parameters1.Add(subContractorInfo.Id);
                //        parameters1.Add(bUnit.Id);
                //        Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                //    }
                //} //#---

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updation Site Order Detail in database");
            }
            //
            // UPDATE ORDER TOTALS
            //
            parameters.Clear();
            parameters.Add(SOD.OrderID);
            try
            {
                Data.DataProvider.GetInstance().SumSiteOrders(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Site Order Summary in database");
            }
        }
        #endregion
        #region SiteOrderDocs
     //   public int SearchSiteOrderDocsCount(String strSearch, String strOrderId)
       public int SearchSiteOrderDocsCount(String strSearch)
        {
            List<Object> parameters = new List<Object>();

            parameters.Add(UI.Utils.GetFormString(strSearch));
            //   parameters.Add((int)UI.Utils.GetFormInteger(strOrderId));
            return 4;
            try
            {
                 return Data.DataProvider.GetInstance().SearchSiteOrderDocsCount(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders Count from database.");
            }
        }
        public int? AddSiteOrderDoc(SiteOrderDocInfo SiteOrderDocInfo)
        {
            int? siteOrderDocId = null;
            List<Object> parameters = new List<Object>();

            // SetCreateInfo(SiteOrderDocInfo); //DS20231010   BATCH MOBILE TRANSMISSION
            SiteOrderDocInfo.CreatedDate = DateTime.Now;   //DS20231010   BATCH MOBILE TRANSMISSION
            
            parameters.Add(SiteOrderDocInfo.ProjectId);
            parameters.Add(SiteOrderDocInfo.OrderId);
            parameters.Add(SiteOrderDocInfo.DocTitle);
            parameters.Add(SiteOrderDocInfo.DocName);
            parameters.Add(SiteOrderDocInfo.DocNameOrig);
            parameters.Add(SiteOrderDocInfo.Status);
            parameters.Add(SiteOrderDocInfo.CreatedDate);
            parameters.Add(SiteOrderDocInfo.CreatedBy);

            try
            {
                siteOrderDocId = Data.DataProvider.GetInstance().AddSiteOrderDoc(parameters.ToArray());

                //#--
                //if (subContractorId != null && siteOrderInfo.BusinessUnitslist.Count>0)
                //{  
                //    foreach (BusinessUnitInfo bUnit in subContractorInfo.BusinessUnitslist) {
                //     List<Object> parameters1 = new List<Object>();
                //        parameters1.Add(subContractorId);
                //        parameters1.Add(bUnit.Id);
                //    Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                //}
                //} //#---
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating SiteOrderDoc in database");
            }

            return siteOrderDocId;
        }
        public int? DeleteSiteOrderDoc(int DocId)
        {
            List<Object> parameters = new List<Object>();

           
            parameters.Add(DocId);

            try
            {
                Data.DataProvider.GetInstance().DeleteSiteOrderDoc(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleteing SiteOrderDoc in database");
            }

            return 0;
        }
        public int? DeleteSiteOrderDetail(int ItemId, int OrderId)
        {
            List<Object> parameters = new List<Object>();


            parameters.Add(ItemId);
            parameters.Add(OrderId);

            try
            {
                Data.DataProvider.GetInstance().DeleteSiteOrderDetail(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Deleting SiteOrderItem in database");
            }
            parameters.Clear();
            parameters.Add(OrderId);

            try
            {
                Data.DataProvider.GetInstance().SumSiteOrders(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Summarising SiteOrder in database");
            }

            return 0;
        }


        /// <summary>
        /// Search for Project Trades in the database DS20230821
        /// </summary>
        public ProjectTradesInfo GetProjectTrades(int ProjectID,String Code)
        {
            IDataReader dr = null;
            ProjectTradesInfo ProjectTradesInfo = new ProjectTradesInfo();
            List<Object> parameters = new List<Object>();
            parameters.Add(ProjectID);
            parameters.Add(Code);

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectTrades(parameters.ToArray());
                //parameters.ToArray()
                if (dr.Read())
                {
                    ProjectTradesInfo PTI = new ProjectTradesInfo();
                    PTI.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    PTI.Code = Data.Utils.GetDBString(dr["Code"]);
                    PTI.Name = Data.Utils.GetDBString(dr["Name"]);
                    return PTI;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            // }
            return null;
        }
        /// <summary>
        /// Search for Project Trades in the database DS20230821
        /// </summary>
        public List<ProjectTradesInfo> GetProjectTradesAll(int ProjectID)
        {
            IDataReader dr = null;
            List<ProjectTradesInfo> ProjectTradesInfoList = new List<ProjectTradesInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(ProjectID);

            try
            {
                dr = Data.DataProvider.GetInstance().GetProjectTradesAll(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    ProjectTradesInfo PTI = new ProjectTradesInfo();
                    PTI.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    PTI.Code = Data.Utils.GetDBString(dr["Code"]);
                    PTI.Name = Data.Utils.GetDBString(dr["Name"]);
                    ProjectTradesInfoList.Add(PTI);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Project Trades from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            // }
            return ProjectTradesInfoList;
        }

        public SiteOrderDocInfo GetSiteOrderDoc(int DocID)
        {
            IDataReader dr = null;
            SiteOrderDocInfo SiteOrderDocInfo = new SiteOrderDocInfo();
            List<Object> parameters = new List<Object>();
            parameters.Add(DocID);

            try
            {
                dr = Data.DataProvider.GetInstance().GetSiteOrderDoc(parameters.ToArray());
                //parameters.ToArray()
                if (dr.Read())
                {
                    SiteOrderDocInfo SSI = new SiteOrderDocInfo();
                    SSI.Id = Data.Utils.GetDBInt32(dr["DocID"]);
                    SSI.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    SSI.OrderId = (int)Data.Utils.GetDBInt32(dr["DocID"]);
                    SSI.DocTitle = Data.Utils.GetDBString(dr["DocTitle"]);
                    SSI.DocName = Data.Utils.GetDBString(dr["DocName"]);
                    SSI.Status = Data.Utils.GetDBString(dr["Status"]);
                    SSI.SharePointID = Data.Utils.GetDBString(dr["SharePointID"]);
                    SSI.isMobileUpload = (int)Data.Utils.GetDBInt32(dr["isMobileUpload"]);
                    return SSI;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Doc from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            // }
            return null;
        }
        /// <summary>
        /// Search for SubContractors in the database 
        /// </summary>
        public List<SiteOrderDocSearchInfo> SearchSiteOrderDocs(int startRowIndex, int maximumRows, String orderBy, String strSearch)
        {
            IDataReader dr = null;
            List<SiteOrderDocSearchInfo> siteOrderDocSearchInfoList = new List<SiteOrderDocSearchInfo>();
            List<Object> parameters = new List<Object>();
            parameterOrderId = "1";
            parameterOrderId = Web.Utils.CheckParameter("OrderID");
            parameters.Add(parameterOrderId);
            //parameters.Add(maximumRows);
            //parameters.Add(UI.Utils.GetFormString("1"));
            //parameters.Add(UI.Utils.GetFormString(strSearch));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrderDocs(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderDocSearchInfo SSI = new SiteOrderDocSearchInfo();
                    SSI.Id = Data.Utils.GetDBInt32(dr["DocID"]);
                    SSI.DocTitle = Data.Utils.GetDBString(dr["DocTitle"]);
                    SSI.DocName = Data.Utils.GetDBString(dr["DocName"]);
                    SSI.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    SSI.DocDate = (DateTime)Data.Utils.GetDBDateTime(dr["ModifiedDate"]);
                    siteOrderDocSearchInfoList.Add(SSI);
                    //   siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Docs from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return siteOrderDocSearchInfoList;
        }
        public List<SiteOrderDocInfo> SiteOrderMobileDocsSearch(int OrderId)
        {
            IDataReader dr = null;
            List<SiteOrderDocInfo> SiteOrderDocInfoList = new List<SiteOrderDocInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(OrderId);

            try
            {
                dr = Data.DataProvider.GetInstance().SiteOrderMobileDocsSearch(parameters.ToArray());
                parameters.ToArray();
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderDocInfo SSI = new SiteOrderDocInfo();
                    SSI.Id = Data.Utils.GetDBInt32(dr["DocID"]);
                    SSI.DocTitle = Data.Utils.GetDBString(dr["DocTitle"]);
                    SSI.DocName = Data.Utils.GetDBString(dr["DocName"]);
                    SSI.isMobileUpload = (int)Data.Utils.GetDBInt32(dr["isMobileUpload"]);
                    SiteOrderDocInfoList.Add(SSI);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Docs from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return SiteOrderDocInfoList;
        }
        public void SiteOrderDocsRename(int DocId, string DocName, string DocNameOrig, int isMobileUpload)
        {
            List<Object> parameters = new List<Object>();
            parameters.Add(DocId);
            parameters.Add(DocName);
            parameters.Add(DocNameOrig);
            parameters.Add(isMobileUpload);

            try
            {
                Data.DataProvider.GetInstance().SiteOrderDocsRename(parameters.ToArray());

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Renaming Site Order Docs from database");
            }
            finally
            {
               
            }

            return ;
        }


        public List<SiteOrderDocSearchInfo> GetSiteOrderDocs(int OrderId)
        {
            IDataReader dr = null;
            List<SiteOrderDocSearchInfo> siteOrderDocSearchInfoList = new List<SiteOrderDocSearchInfo>();
            List<Object> parameters = new List<Object>();
            parameters.Add(OrderId);

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrderDocs(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderDocSearchInfo SSI = new SiteOrderDocSearchInfo();
                    SSI.Id = Data.Utils.GetDBInt32(dr["DocID"]);
                    SSI.DocTitle = Data.Utils.GetDBString(dr["DocTitle"]);
                    SSI.DocName = Data.Utils.GetDBString(dr["DocName"]);
                    SSI.LastName = Data.Utils.GetDBString(dr["LastName"]);
                    SSI.DocDate = (DateTime)Data.Utils.GetDBDateTime(dr["ModifiedDate"]);
                    siteOrderDocSearchInfoList.Add(SSI);
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order Docs from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return siteOrderDocSearchInfoList;
        }
        #endregion
        #region SiteOrders
        public List<String> GetBusinessUnits()  //DS20230627
        {
            List<String> BU = new List<String>();
            IDataReader dr = null;
            try
            {

                dr = Data.DataProvider.GetInstance().GetBusinessUnits();
                while (dr.Read())
                {
                    BU.Add(Data.Utils.GetDBString(dr["Name"]).ToUpper());
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Business Uits from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return BU;
        }
        public List<SiteOrderInfo> SearchSiteOrdersAll(int startRowIndex, int maximumRows, String orderBy, String strSearch, String strSiteOrderType, bool chkHistory)  //DS20230627
        {
            List<String> BU = GetBusinessUnits();
            List<String> UserSearch = new List<String>();
            List<String> BUSearch = new List<String>();
            int ShowHistory = 0;
            if (chkHistory == true) { ShowHistory = 1; }
            IDataReader dr = null;
            List<SiteOrderInfo> siteOrderInfoList = new List<SiteOrderInfo>();
            List<Object> parameters = new List<Object>();
            // parameterProjectId = Web.Utils.CheckParameter("ProjectId");
            string[] Search = (strSearch + ",,,").Split(',');   //DS20230627
            foreach (String SearchItem in Search)    //DS20230824 >>>
            {
                string SearchParm = SearchItem;
                if (SearchItem.StartsWith("D") & SearchParm.Length > 1)
                {
                    String SearchPrefix = SearchParm.Substring(1, SearchParm.Length - 1);
                    long number1 = 0;
                    bool canConvert = long.TryParse(SearchPrefix, out number1);
                    if (canConvert == true) { SearchParm = number1.ToString(); }
                }
                if (BU.Contains(SearchParm.ToUpper()) == true && SearchParm != "") 
                {
                    BUSearch.Add(SearchParm);
                }
                else 
                {
                     UserSearch.Add(SearchParm);
                }
            }
            BUSearch.Add("");
            int UserId = (int)Web.Utils.GetCurrentUserId();
            parameters.Add("");
            //parameters.Add(UI.Utils.GetFormString(strSearch));   //DS20230307
            parameters.Add(UI.Utils.GetFormString(UserSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[1]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(UserSearch[2]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(BUSearch[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(strSiteOrderType));
            parameters.Add(ShowHistory);
            parameters.Add(orderBy);
            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            //parameters.Add(UI.Utils.GetFormString("1"));
            //parameters.Add(UI.Utils.GetFormString(strSearch));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrdersAll(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                    SiteOrderInfo SOI = new SiteOrderInfo();
                    SOI.Id = Data.Utils.GetDBInt32(dr["ID"]);
                    SOI.Title = Data.Utils.GetDBString(dr["Title"]);
                    SOI.Status = Data.Utils.GetDBString(dr["Status"]);
                    SOI.Typ = Data.Utils.GetDBString(dr["Type"]);
                    SOI.OrderDate = Data.Utils.GetDBDateTime(dr["Date"]);
                    SOI.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
                    SOI.ProjectName = Data.Utils.GetDBString(dr["ProjectName"]);
                    SOI.Name = Data.Utils.GetDBString(dr["Name"]);
                    SOI.Street = Data.Utils.GetDBString(dr["Street"]);
                    SOI.Locality = Data.Utils.GetDBString(dr["Locality"]);
                    SOI.State = Data.Utils.GetDBString(dr["State"]);
                    SOI.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);
                    SOI.SubContractorName = Data.Utils.GetDBString(dr["SubContractorName"]);
                    SOI.ForemanName = Data.Utils.GetDBString(dr["ForemanName"]);
                    SOI.SubTotal = Data.Utils.GetDBDecimal(dr["SubTotal"]);
                    SOI.DateStart = Data.Utils.GetDBDateTime(dr["DateStart"]);
                    SOI.DateEnd = Data.Utils.GetDBDateTime(dr["DateEnd"]);
                    SOI.BusinessUnitName = Data.Utils.GetDBString(dr["BusinessUnitName"]);  // 20230620
                    SOI.GivenByName = Data.Utils.GetDBString(dr["GivenByName"]);  // 20230905
                    if (SOI.Typ == "Hir")
                    {
                        SOI.RowColor = "Orange";
                        if (DateTime.Now.AddDays(7) > SOI.DateEnd)
                        {
                            SOI.RowColor = "Red";
                        }
                    }
                    else
                    {
                        SOI.RowColor = "White";
                    }
                    SOI.DocCount = (int)Data.Utils.GetDBInt32(dr["DocCount"]);   // DS20230710
                    siteOrderInfoList.Add(SOI);
                    //   siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                }
                // siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello2", Status = "B", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello3", Status = "C", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello4", Status = "D", Type = "MAT", Date = DateTime.Now });
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return siteOrderInfoList;
        }
        /// <summary>
        /// Search for SubContractors in the database 
        /// </summary>
        public List<SiteOrderInfo> SearchSiteOrders(int startRowIndex, int maximumRows, String orderBy, String strSearch, String strSiteOrderType, bool chkHistory)
        {
            List<Object> parameters = new List<Object>();   //DS20230307
            int UserId = (int)Web.Utils.GetCurrentUserId();
            string[] Search = (strSearch + ",,,").Split(',');   //DS20230627
            int ShowHistory = 0;
            if (chkHistory == true) {ShowHistory = 1; }
            IDataReader dr = null;
            List<SiteOrderInfo> siteOrderInfoList = new List<SiteOrderInfo>();
            parameterProjectId = Web.Utils.CheckParameter("ProjectId");
            projectInfo = projectsController.GetProjectWithRFIs(Int32.Parse(parameterProjectId));
            parameters.Add(parameterProjectId);
            parameters.Add(UI.Utils.GetFormString(Search[0]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(Search[1]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(Search[2]));   //DS20230627
            parameters.Add(UI.Utils.GetFormString(strSiteOrderType));
            parameters.Add(ShowHistory);
            parameters.Add(orderBy);
            parameters.Add(startRowIndex);
            parameters.Add(maximumRows);
            //parameters.Add(UI.Utils.GetFormString("1"));
            //parameters.Add(UI.Utils.GetFormString(strSearch));

            try
            {
                dr = Data.DataProvider.GetInstance().SearchSiteOrders(parameters.ToArray());
                //parameters.ToArray()
                while (dr.Read())
                {
                    // siteOrderInfoList.Add(this.GetSiteOrder(Data.Utils.GetDBInt32(dr["OrderId"])));projectInfo.IdStr
                                        SiteOrderInfo SOI = new SiteOrderInfo();
                    SOI.Id = Data.Utils.GetDBInt32(dr["ID"]);
                    SOI.Title = Data.Utils.GetDBString(dr["Title"]);
                    SOI.Status = Data.Utils.GetDBString(dr["Status"]);
                    SOI.Typ = Data.Utils.GetDBString(dr["Type"]);
                    SOI.OrderDate = Data.Utils.GetDBDateTime(dr["Date"]);
                    SOI.DateStart = Data.Utils.GetDBDateTime(dr["DateStart"]);
                    SOI.DateEnd = Data.Utils.GetDBDateTime(dr["DateEnd"]);
                    SOI.Name = Data.Utils.GetDBString(dr["Name"]);
                    SOI.Street = Data.Utils.GetDBString(dr["Street"]);
                    SOI.Locality = Data.Utils.GetDBString(dr["Locality"]);
                    SOI.State = Data.Utils.GetDBString(dr["State"]);
                    SOI.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);
                    SOI.SubContractorName = Data.Utils.GetDBString(dr["SubContractorName"]);
                    SOI.ForemanName = Data.Utils.GetDBString(dr["ForemanName"]);
                    SOI.GivenByName = Data.Utils.GetDBString(dr["GivenByName"]);   // DS20240301
                    SOI.SubTotal = (decimal)Data.Utils.GetDBDecimal(dr["SubTotal"]);
                    if (SOI.Typ == "Hir")
                    {
                        SOI.RowColor = "Orange";
                        if (DateTime.Now.AddDays(7) > SOI.DateEnd)
                        {
                            SOI.RowColor = "Red";
                        }
                    }
                    else
                    {
                        SOI.RowColor = "White";
                    }
                    SOI.DocCount = (int)Data.Utils.GetDBInt32(dr["DocCount"]);   // DS20230710
                    siteOrderInfoList.Add(SOI);
                 //   siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                }
               // siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello", Status = "A", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello2", Status = "B", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello3", Status = "C", Type = "MAT", Date = DateTime.Now });
                //siteOrderInfoList.Add(new SiteOrderInfo() { Title = "Hello4", Status = "D", Type = "MAT", Date = DateTime.Now });
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return siteOrderInfoList;
        }
  
        /// <summary>
        /// Get all SubContractors names for the specified business unit form the database
        /// </summary>
        public List<SiteOrderInfo> ListSiteOrders(BusinessUnitInfo businessUnitInfo)
        {
            IDataReader dr = null;
            SiteOrderInfo siteOrderInfo = null;
            List<SiteOrderInfo> siteOrderInfoList = new List<SiteOrderInfo>();
            List<Object> parameters = new List<Object>();

            parameters.Add(GetId(siteOrderInfo));

            try
            {
                dr = Data.DataProvider.GetInstance().ListSiteOrders(parameters.ToArray());
                while (dr.Read())
                {
                    siteOrderInfo = new SiteOrderInfo(Data.Utils.GetDBInt32(dr["OrderId"]));
                    //siteOrderInfo.ShortName = Data.Utils.GetDBString(dr["ShortName"]);

                    siteOrderInfoList.Add(siteOrderInfo);
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Orders from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return siteOrderInfoList;
        }

        /// <summary>
        /// Get all SubContractors names form the database
        /// </summary>
        public List<SiteOrderInfo> ListSiteOrders()
        {
            return ListSiteOrders(null);
        }

        /// <summary>
        /// Creates a Subcontractor from a dr
        /// </summary>
        public SiteOrderInfo CreateSiteOrder(IDataReader dr, Dictionary<int, BusinessUnitInfo> businessUnitDictionary)
        {
            SiteOrderInfo siteOrderInfo = new SiteOrderInfo((int)Data.Utils.GetDBInt32(dr["OrderID"])); ;

     //       siteOrderInfo.Type = Data.Utils.GetDBString(dr["Type"]);
     //       siteOrderInfo.Title = Data.Utils.GetDBString(dr["Title"]);
     //       siteOrderInfo.OrderDate = Data.Utils.GetDBDateTime(dr["Date"]);
     //       siteOrderInfo.Status = Data.Utils.GetDBString(dr["Status"]);
            
       //     siteOrderInfo.Id = (int)Data.Utils.GetDBInt32(dr["OrderID"]);
            siteOrderInfo.Title = Data.Utils.GetDBString(dr["Title"]);
            siteOrderInfo.ProjectId = (int)Data.Utils.GetDBInt32(dr["ProjectId"]);
            siteOrderInfo.SubContractorId = (int)Data.Utils.GetDBInt32(dr["SubContractorId"]);
	        siteOrderInfo.Typ = Data.Utils.GetDBString(dr["Typ"]);
            siteOrderInfo.OrderDate = Data.Utils.GetDBDateTime(dr["OrderDate"]);
            siteOrderInfo.Status = Data.Utils.GetDBString(dr["Status"]);
	        siteOrderInfo.DateStart = Data.Utils.GetDBDateTime(dr["DateStart"]);
            siteOrderInfo.ForemanID = (int)Data.Utils.GetDBInt32(dr["ForemanID"]);
            siteOrderInfo.DateEnd = Data.Utils.GetDBDateTime(dr["DateEnd"]);
            siteOrderInfo.ItemsTotal = (decimal)Data.Utils.GetDBDecimal(dr["ItemsTotal"]);
            // DS20230306 siteOrderInfo.DeliveryFee = (decimal)Data.Utils.GetDBDecimal(dr["DeliveryFee"]);
            siteOrderInfo.SubTotal = (decimal)Data.Utils.GetDBDecimal(dr["SubTotal"]);
            // DS20230304 siteOrderInfo.GST = (decimal)Data.Utils.GetDBDecimal(dr["GST"]);
            // DS20230304 siteOrderInfo.Total = (decimal)Data.Utils.GetDBDecimal(dr["Total"]);
            siteOrderInfo.Contact = Data.Utils.GetDBString(dr["Contact"]);
            // DS20230304 siteOrderInfo.ContactPhone = Data.Utils.GetDBString(dr["ContactPhone"]);
            siteOrderInfo.PeriodType = Data.Utils.GetDBString(dr["PeriodType"]);
            siteOrderInfo.PaymentStatus = Data.Utils.GetDBString(dr["PaymentStatus"]);
            siteOrderInfo.Email = Data.Utils.GetDBString(dr["Email"]);
            siteOrderInfo.VariationID = (int)Data.Utils.GetDBInt32(dr["VariationID"]);
            siteOrderInfo.ContactPeopleId = (int)Data.Utils.GetDBInt32(dr["ContactPeopleId"]);
            siteOrderInfo.Notes = Data.Utils.GetDBString(dr["Notes"]);
            // DS20230304 siteOrderInfo.GSTRate = (decimal)Data.Utils.GetDBDecimal(dr["GSTRate"]);
            siteOrderInfo.Name = Data.Utils.GetDBString(dr["Name"]);
            siteOrderInfo.Street = Data.Utils.GetDBString(dr["Street"]);
            siteOrderInfo.Locality = Data.Utils.GetDBString(dr["Locality"]);
            siteOrderInfo.State = Data.Utils.GetDBString(dr["State"]);
            siteOrderInfo.PostalCode = Data.Utils.GetDBString(dr["PostalCode"]);
            siteOrderInfo.ABN = Data.Utils.GetDBString(dr["ABN"]);
            siteOrderInfo.isOrderApproved = (int)Data.Utils.GetDBInt32(dr["isOrderApproved"]);
            siteOrderInfo.GivenByPeopleId = (int)Data.Utils.GetDBInt32(dr["GivenByPeopleId"]);  // DS20230304
            siteOrderInfo.isMobile = (int)Data.Utils.GetDBInt32(dr["isMobile"]);  // DS20230304
            siteOrderInfo.Comments = Data.Utils.GetDBString(dr["Comments"]);  // DS20230304
            siteOrderInfo.TypeInfo = Data.Utils.GetDBString(dr["TypeInfo"]);  // DS20230304
            siteOrderInfo.TradesCode = Data.Utils.GetDBString(dr["TradesCode"]);  // DS20230308
            if (Data.Utils.GetDBString(dr["OrderCode"]) == null)
            {

            }
            else
            {
                siteOrderInfo.OrderCode = Data.Utils.GetDBString(dr["OrderCode"]);
            }
            //AssignAuditInfo(subContractorInfo, dr);

            //if (dr["BusinessUnitId"] != DBNull.Value)
            //    subContractorInfo.BusinessUnit = ContractsController.GetInstance().CreateBusinessUnit(Data.Utils.GetDBInt32(dr["BusinessUnitId"]), businessUnitDictionary);

            //TODO Read Items from DB
            //siteOrderInfo.Items.Add(new SiteOrderItemInfo()
            //{
            //    Item = "Item 1",
            //    Quantity = 1,
            //    Price = 100,
            //    UM = "EA",
            //    Amount = 100
            //});
            //siteOrderInfo.Items.Add(new SiteOrderItemInfo()
            //{
            //    Item = "Item 2",
            //    Quantity = 2,
            //    Price = 200,
            //    UM = "EA",
            //    Amount = 400
            //});
            //siteOrderInfo.Items.Add(new SiteOrderItemInfo()
            //{
            //    Item = "Item 3",
            //    Quantity = 3,
            //    Price = 300,
            //    UM = "EA",
            //    Amount = 900

            //});

            return siteOrderInfo;
        }

        /// <summary>
        /// Creates a Subcontractor from a dr
        /// </summary>
        public SiteOrderInfo CreateSiteOrder(IDataReader dr)
        {
            return CreateSiteOrder(dr, null);
        }

        /// <summary>
        /// Creates a CreateSiteOrder object from a dr or retrieve it from the dictionary
        /// </summary>
        public SiteOrderInfo CreateSiteOrder(Object dbId, Dictionary<int, SiteOrderInfo> siteOrdersDictionary)
        {
            int Id = Data.Utils.GetDBInt32(dbId).Value;

            if (siteOrdersDictionary == null)
                return GetInstance().GetSiteOrder(Id);
            else if (siteOrdersDictionary.ContainsKey(Id))
                return siteOrdersDictionary[Id];
            else
                return null;
        }

        /// <summary>
        /// Get a SubContractor from persistent storage
        /// </summary>
        public SiteOrderInfo GetSiteOrder(int? OrderId)
        {
            IDataReader dr = null;
            List<Object> parameters = new List<Object>();

            parameters.Add(OrderId);
            try
            {
                dr = Data.DataProvider.GetInstance().GetSiteOrder(parameters.ToArray());
                if (dr.Read())
                    return CreateSiteOrder(dr);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Getting Site Order from database");
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }

            return null;
        }
        //public SiteOrderInfo GetSiteOrderDetailss(int? OrderId)
        //{
        //    IDataReader dr = null;
        //    List<Object> parameters = new List<Object>();

        //    parameters.Add(OrderId);
        //    try
        //    {
        //        dr = Data.DataProvider.GetInstance().GetSiteOrderDetailss(parameters.ToArray());
        //        if (dr.Read())
        //            return CreateSiteOrderDetail(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.LogError(ex.ToString());
        //        throw new Exception("Getting Site Order from database");
        //    }
        //    finally
        //    {
        //        if (dr != null)
        //            dr.Close();
        //    }

        //    return null;
        //}
        ///// <summary>
        ///// Gets a Subcontractor with its contacts
        ///// </summary>
        //public SiteOrderInfo GetSiteOrderDeep(int? subContractorId)
        //{
        //    SubContractorInfo subContractorInfo = GetSubContractor(subContractorId);
        //    if (subContractorInfo != null) { 
        //        subContractorInfo.Contacts = GetContacts(subContractorInfo);
        //        //#-----
        //         subContractorInfo.BusinessUnitslist= GetSubcontractorBusinessUnitList(subContractorInfo);   //#-----------
        //     }
        //    return subContractorInfo;
        //}

        /// <summary>
        /// Updates a SubContractor in the database
        /// </summary>
        /// <param name="subContractorInfo">The SubContractor to update</param>
        public void UpdateSiteOrder(SiteOrderInfo SiteOrderInfo)
        {
            List<Object> parameters = new List<Object>();

            SetModifiedInfo(SiteOrderInfo);
            parameters.Add(SiteOrderInfo.Id);
            parameters.Add(SiteOrderInfo.Title);
            parameters.Add(SiteOrderInfo.ProjectId);
            parameters.Add(SiteOrderInfo.SubContractorId);
            parameters.Add(SiteOrderInfo.Typ);
            parameters.Add(SiteOrderInfo.OrderDate);
            parameters.Add(SiteOrderInfo.Status);
            parameters.Add(SiteOrderInfo.DateStart);
            parameters.Add(SiteOrderInfo.ForemanID);
            parameters.Add(SiteOrderInfo.DateEnd);
            parameters.Add(SiteOrderInfo.ItemsTotal);
            // DS20230306 parameters.Add(SiteOrderInfo.DeliveryFee);
            parameters.Add(SiteOrderInfo.SubTotal);
            // DS20230304 parameters.Add(SiteOrderInfo.GST);
            // DS20230304 parameters.Add(SiteOrderInfo.Total);
            parameters.Add(SiteOrderInfo.Contact);
            // DS20230304 parameters.Add(SiteOrderInfo.ContactPhone);
            parameters.Add(SiteOrderInfo.PeriodType);
            parameters.Add(SiteOrderInfo.PaymentStatus);
            parameters.Add(SiteOrderInfo.VariationID);
            parameters.Add(SiteOrderInfo.Email);
            parameters.Add(SiteOrderInfo.ContactPeopleId);
            parameters.Add(SiteOrderInfo.OrderCode);
            parameters.Add(SiteOrderInfo.Notes);
            // DS20230304 parameters.Add(SiteOrderInfo.GSTRate);
            parameters.Add(SiteOrderInfo.Name);
            parameters.Add(SiteOrderInfo.Street);
            parameters.Add(SiteOrderInfo.Locality);
            parameters.Add(SiteOrderInfo.State);
            parameters.Add(SiteOrderInfo.PostalCode);
            parameters.Add(SiteOrderInfo.ABN);
            parameters.Add(SiteOrderInfo.GivenByPeopleId);// DS20230304
            parameters.Add(SiteOrderInfo.isMobile);       // DS20230304
            parameters.Add(SiteOrderInfo.Comments);       // DS20230304
            parameters.Add(SiteOrderInfo.TypeInfo);       // DS20230304
            parameters.Add(SiteOrderInfo.TradesCode);     // DS20230307
            parameters.Add(SiteOrderInfo.ModifiedDate);
            parameters.Add(SiteOrderInfo.ModifiedBy);

            try
            {
                Data.DataProvider.GetInstance().UpdateSiteOrder(parameters.ToArray());


                //#--
                //if (siteOrderInfo != null)
                //{

                //    Data.DataProvider.GetInstance().DeleteSubContractorBusinessUnitList(siteOrderInfo.Id);


                //    foreach (BusinessUnitInfo bUnit in siteOrderInfo.BusinessUnitslist)
                //    {
                //        List<Object> parameters1 = new List<Object>();
                //        parameters1.Add(subContractorInfo.Id);
                //        parameters1.Add(bUnit.Id);
                //        Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                //    }
                //} //#---

            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updation Site Order in database");
            }
        }
        public SiteOrderInfo InitializeSiteOrder(int ProjectId,int SubContractorId,int FormanId)
        {
            SiteOrderInfo SiteOrderInfo;
            SiteOrderInfo = new SiteOrderInfo();
            SiteOrderInfo.Title = "";
            SiteOrderInfo.ProjectId = ProjectId;
            SiteOrderInfo.SubContractorId = SubContractorId;
            SiteOrderInfo.Typ = "";
            SiteOrderInfo.OrderDate = DateTime.Today;
            SiteOrderInfo.Status = "";
            SiteOrderInfo.DateStart = DateTime.Today;
            SiteOrderInfo.ForemanID = FormanId; 
            SiteOrderInfo.DateEnd = DateTime.Today;
            SiteOrderInfo.ItemsTotal = 0;
            // DS20230306 SiteOrderInfo.DeliveryFee = 0;
            SiteOrderInfo.SubTotal = 0;
            // DS20230304 SiteOrderInfo.GST = 0;  // DS20230304
            // DS20230304 SiteOrderInfo.Total = 0; // DS20230304
            SiteOrderInfo.Contact = "";
            // DS20230304 SiteOrderInfo.ContactPhone = ""; // DS20230304
            SiteOrderInfo.PeriodType = "M";
            SiteOrderInfo.PaymentStatus = "";
            SiteOrderInfo.OrderCode = "";
            SiteOrderInfo.Notes = "";
            // DS20230304 SiteOrderInfo.GSTRate = 0; // DS20230304
            SiteOrderInfo.ContactPeopleId = 0;
            SiteOrderInfo.Name = "";
            SiteOrderInfo.Street = "";
            SiteOrderInfo.Locality = "";
            SiteOrderInfo.State = "";
            SiteOrderInfo.PostalCode = "";
            SiteOrderInfo.ABN = "";
            SiteOrderInfo.Email = "";
            SiteOrderInfo.GivenByPeopleId = (int)Web.Utils.GetCurrentUserId(); // DS20230304
            SiteOrderInfo.isMobile = 0;        // DS20230304
            SiteOrderInfo.Comments = "";       // DS20230304
            SiteOrderInfo.TypeInfo = "";       // DS20230304
            SiteOrderInfo.TradesCode = "";       // DS20230307
            SiteOrderInfo.CreatedDate = DateTime.Today;
            SiteOrderInfo.ModifiedDate = DateTime.Today;

            return SiteOrderInfo;
        }
        /// <summary>
        /// Adds a SubContractor to the database
        /// </summary>
        /// <param name="subContractorInfo">The SubContractor to add</param>
        public int? AddSiteOrder(SiteOrderInfo SiteOrderInfo)
        {
            int? OrderId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(SiteOrderInfo);
            parameters.Add(SiteOrderInfo.Title);
            parameters.Add(SiteOrderInfo.ProjectId);
            parameters.Add(SiteOrderInfo.SubContractorId);  
            parameters.Add(SiteOrderInfo.Typ);
            parameters.Add(SiteOrderInfo.OrderDate);
            parameters.Add(SiteOrderInfo.Status);
            parameters.Add(SiteOrderInfo.DateStart);
            parameters.Add(SiteOrderInfo.ForemanID);
            parameters.Add(SiteOrderInfo.DateEnd);
            parameters.Add(SiteOrderInfo.ItemsTotal);
            // DS20230306 parameters.Add(SiteOrderInfo.DeliveryFee);
            parameters.Add(SiteOrderInfo.SubTotal);
            // DS20230304 parameters.Add(SiteOrderInfo.GST);
            // DS20230304 parameters.Add(SiteOrderInfo.Total);
            parameters.Add(SiteOrderInfo.Contact);
            // DS20230304 parameters.Add(SiteOrderInfo.ContactPhone);
            parameters.Add(SiteOrderInfo.PeriodType);
            parameters.Add(SiteOrderInfo.PaymentStatus);
            parameters.Add(SiteOrderInfo.VariationID);
            parameters.Add(SiteOrderInfo.Email);
            parameters.Add(SiteOrderInfo.ContactPeopleId);
            parameters.Add(SiteOrderInfo.OrderCode);
            parameters.Add(SiteOrderInfo.Notes);
            // DS20230304 parameters.Add(SiteOrderInfo.GSTRate);
            parameters.Add(SiteOrderInfo.Name);
            parameters.Add(SiteOrderInfo.Street);
            parameters.Add(SiteOrderInfo.Locality);
            parameters.Add(SiteOrderInfo.State);
            parameters.Add(SiteOrderInfo.PostalCode);
            parameters.Add(SiteOrderInfo.ABN);
            parameters.Add(SiteOrderInfo.GivenByPeopleId);// DS20230304
            parameters.Add(SiteOrderInfo.isMobile);       // DS20230304
            parameters.Add(SiteOrderInfo.Comments);       // DS20230304
            parameters.Add(SiteOrderInfo.TypeInfo);       // DS20230304
            parameters.Add(SiteOrderInfo.TradesCode);       // DS20230307
            parameters.Add(SiteOrderInfo.CreatedDate);
            parameters.Add(SiteOrderInfo.CreatedBy);
            try
            {
                OrderId = Data.DataProvider.GetInstance().AddSiteOrder(parameters.ToArray());
                
                //#--
                //if (subContractorId != null && siteOrderInfo.BusinessUnitslist.Count>0)
                //{  
                //    foreach (BusinessUnitInfo bUnit in subContractorInfo.BusinessUnitslist) {
                //     List<Object> parameters1 = new List<Object>();
                //        parameters1.Add(subContractorId);
                //        parameters1.Add(bUnit.Id);
                //    Data.DataProvider.GetInstance().AddBusinessUnitsToSubContractor(parameters1.ToArray());
                //}
               //} //#---
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Updating Subcontractor in database");
            }

            return OrderId;
        }
        public int? AddSiteOrderApprovalsProcess(SiteOrderInfo SiteOrderInfo)
        {
            int? OrderId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(SiteOrderInfo);
            parameters.Add(SiteOrderInfo.Id);
            parameters.Add(SiteOrderInfo.CreatedDate);
            parameters.Add(SiteOrderInfo.CreatedBy);

            try
            {
                OrderId = Data.DataProvider.GetInstance().AddSiteOrderApprovalsProcess(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Site Order Approvals in database");
            }

            return 0;
        }
        public int? AddSiteOrderApprovalsHireProcess(SiteOrderInfo SiteOrderInfo)
        {
            int? OrderId = null;
            List<Object> parameters = new List<Object>();

            SetCreateInfo(SiteOrderInfo);
            parameters.Add(SiteOrderInfo.Id);
            parameters.Add(SiteOrderInfo.CreatedDate);
            parameters.Add(SiteOrderInfo.CreatedBy);

            try
            {
                OrderId = Data.DataProvider.GetInstance().AddSiteOrderApprovalsHireProcess(parameters.ToArray());
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Adding Site Order Approvals in database");
            }

            return 0;
        }
        /// <summary>
        /// Adds or updates a Subcontractor
        /// </summary>
        public int? AddUpdateSiteOrder(SiteOrderInfo siteOrderInfo)
        {
            if (siteOrderInfo != null)
            {
                if (siteOrderInfo.Id != null)
                {
                    UpdateSiteOrder(siteOrderInfo);
                    return siteOrderInfo.Id;
                }
                else
                {
                    return AddSiteOrder(siteOrderInfo);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a Subcontractor from persistent storage
        /// </summary>
        public void DeleteSiteOrder(SiteOrderInfo siteOrderInfo)
        {
            try
            {
                Data.DataProvider.GetInstance().DeleteSiteOrder(siteOrderInfo.Id);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Removing Site Order from database");
            }
        }
        #endregion
        public string SendMobileOrders() //DS20231010
        {
            String LogFolder = "D:/SOS/Prod/Logfiles/";
            DateTime LogDate = DateTime.Now;

            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            List<SiteOrderApprovalsInfo> SOA = SiteOrdersController.SearchSiteOrderApprovalsEmailList();
            foreach(SiteOrderApprovalsInfo SOASingle in SOA)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(LogFolder, "SOSBatch.log"), true))
                { outputFile.WriteLine(LogDate.ToString() + " Sending Order " + SOASingle.OrderId.ToString() ); }
                string Recipient = "";
                bool isError = false;
                try
                {
                     Recipient = SendSubmisionNotification(SOASingle.OrderId.ToString(), (int)SOASingle.CreatedBy);
                }
                catch (Exception ex)
                {
                    Recipient = "Send Error Order " + SOASingle.OrderId.ToString() + " " + ex.Message;
                    isError = true;
                }
                if(isError == true)
                {
                    SOASingle.Reason = "Send Error";
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(LogFolder, "SOSBatch.log"), true))
                    { outputFile.WriteLine(LogDate.ToString() + " Sent Order Error " + SOASingle.OrderId.ToString() + " to " + Recipient); }
                }
                else
                {
                    SOASingle.Reason = "Sent";
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(LogFolder, "SOSBatch.log"), true))
                    { outputFile.WriteLine(LogDate.ToString() + " Sent Order " + SOASingle.OrderId.ToString() + " to " + Recipient); }
                }
                SiteOrdersController.SiteOrderApprovalsUpdateReason(SOASingle);
            }

            return "";
        }
        //public string MaintenanceDaily() //DS20231010
        //{
        //    String LogFolder = "D:/SOS/Prod/Logfiles/";
        //    DateTime LogDate = DateTime.Now;

        //    SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
        //    SiteOrdersController.SiteOrderApprovalsUpdateReason(SOASingle);

        //    String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
        //    string filePath = @DocFolder + saveDir;
        //    string TargetFileName = SiteOrderInfo.IdStr + "_" + DocID.ToString() + ".pdf";
        //    string FullName = filePath + TargetFileName;
        //    if (System.IO.Directory.Exists(filePath) == false)   // DS20230921
        //    {
        //        System.IO.Directory.CreateDirectory(filePath);
        //    }
        //    System.IO.File.WriteAllBytes(FullName, attachment);
        //    return "";
        //}
        public string SendSubmisionNotification(string parameterSiteOrderId,int SenderUserId)  //DS20231010
        {
            SiteOrderDocInfo SOD = new SiteOrderDocInfo();
            SiteOrderInfo SiteOrderInfo = null;
            ProjectInfo ProjectInfo = null;
            SubContractorInfo SubContractorInfo = null;
            PeopleInfo ForemanPeopleInfo = null;
            PeopleInfo EmailPeopleInfo = null;
            ProjectTradesInfo ProjectTradesInfo = null; //DS20230822
            PeopleInfo ContactPeopleInfo = null;
            PeopleInfo GivenByPeopleInfo = null;        // DS20230304
            PeopleInfo LoginPeopleInfo = null;        // DS20230329
            List<PeopleInfo> DummyPeopleInfoList = new List<PeopleInfo>() ;
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            SubContractorsController SubContractorsController = SubContractorsController.GetInstance();
            PeopleController PeopleController = PeopleController.GetInstance();

            //       try
            //       {
            // Security.CheckAccess(Security.userActions.ViewSiteOrder);     DS20231010 ### REMOVED FOR MOBILE PRINTING ###
            
            SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(parameterSiteOrderId));
            ProjectTradesInfo = SiteOrdersController.GetProjectTrades(SiteOrderInfo.ProjectId, SiteOrderInfo.TradesCode);  //DS20230822
            SiteOrderInfo.isOrderApproved = SiteOrdersController.SiteOrderGetApprovalStatus(Int32.Parse(parameterSiteOrderId));
            ProjectInfo = ProjectsController.GetProjectWithTradesParticipations(SiteOrderInfo.ProjectId);
            SiteOrderInfo.Items = SiteOrdersController.GetSiteOrderDetails(Int32.Parse(parameterSiteOrderId));
            SubContractorInfo = SubContractorsController.GetSubContractor(SiteOrderInfo.SubContractorId);
            EmailPeopleInfo = new ContactInfo(Data.Utils.GetDBInt32(SiteOrderInfo.SubContractorId)); 
            ForemanPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.ForemanID);
            GivenByPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.GivenByPeopleId);
            ContactPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.ContactPeopleId);// DS20230304
           // LoginPeopleInfo = PeopleController.GetPersonById((int)Web.Utils.GetCurrentUserId());  // DS20231010
            LoginPeopleInfo = PeopleController.GetPersonById(SenderUserId);  // DS20231010
            SiteOrderDocSearchInfo SODS = new SiteOrderDocSearchInfo();  // DS20230906 <<<
            EmailPeopleInfo.FirstName = SiteOrderInfo.Contact;
            EmailPeopleInfo.LastName = "";
            EmailPeopleInfo.Email = SiteOrderInfo.Email;
            // ContactInfo contactInfo = (ContactInfo)Web.Utils.GetCurrentUser();
            string BudgetName = "";

            List<TradeTemplateInfo> tradeTemplates = TradesController.GetInstance().GetTradeTemplatesFromCode(SiteOrderInfo.TradesCode);
            foreach (TradeTemplateInfo TradeTemplate in tradeTemplates) // DS20230308
            {
                BudgetName = TradeTemplate.TradeCode + " - " + TradeTemplate.TradeDescription;
            }
            Byte[] attachment = null;
            if (SiteOrderInfo.Typ == "Mat") { attachment = GenerateSiteOrderReport(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName,ProjectTradesInfo); }// DS2023822
            if (SiteOrderInfo.Typ == "Hir") { attachment = GenerateSiteOrderReportHir(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName,ProjectTradesInfo); }// DS2023822
            if (SiteOrderInfo.Typ == "Ins") { attachment = GenerateSiteOrderReportIns(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, ProjectTradesInfo); }// DS2023822
            // DS20230906 >>> write the pdf to diretory
            // 1 ADD SiteOrderDoc
            //
            SOD.ProjectId = SiteOrderInfo.ProjectId;
            SOD.OrderId = (int)SiteOrderInfo.Id;
            SOD.DocTitle = "PDF Sent to provider";    // DS20231012
            SOD.DocName = "SiteOrder_" + SiteOrderInfo.IdStr + ".pdf";
            SOD.DocNameOrig = "SiteOrder_" + SiteOrderInfo.IdStr + ".pdf";
            SOD.CreatedBy = LoginPeopleInfo.Id;
            SOD.CreatedDate = DateTime.Today;
            SOD.Status = "CUR";
            int DocID = (int)SiteOrdersController.GetInstance().AddSiteOrderDoc(SOD);
            //
            // 2 Save PDF   
            string saveDir = @"P" + SiteOrderInfo.ProjectId.ToString() + "\\";
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + saveDir;
            string TargetFileName = SiteOrderInfo.IdStr + "_" + DocID.ToString() + ".pdf";
            string FullName = filePath + TargetFileName;
            if(System.IO.Directory.Exists(filePath) == false)   // DS20230921
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            System.IO.File.WriteAllBytes(FullName, attachment);
            // DS20230906 <<< write the pdf to diretory
            Byte[] attachmentTerms = SOS.UI.Utils.getSiteOrderTerms(SiteOrderInfo.Typ, ProjectInfo.BusinessUnitName);

            String attachmentName = String.Format("Site Order_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr);
            String attachmentNameTerms = String.Format("Site Order_{0}_{1}_{2}_TERMS.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr);

            String subject = String.Format("Submission - {0} - {1} - {2}", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr);
            //
            // SAVE TERMS PDF DS20231012  >>>
            //
            if (SiteOrderInfo.SubContractorId == 0)
            {
                SOD.ProjectId = SiteOrderInfo.ProjectId;
                SOD.OrderId = (int)SiteOrderInfo.Id;
                SOD.DocTitle = "PDF Terms Sent to provider";    // DS20231012
                SOD.DocName = "SiteOrder_" + SiteOrderInfo.IdStr + "_TERMS.pdf";
                SOD.DocNameOrig = "SiteOrder_" + SiteOrderInfo.IdStr + "_TERMS.pdf";
                SOD.CreatedBy = LoginPeopleInfo.Id;
                SOD.CreatedDate = DateTime.Today;
                SOD.Status = "CUR";
                DocID = (int)SiteOrdersController.GetInstance().AddSiteOrderDoc(SOD);
                DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
                filePath = @DocFolder + saveDir;
                TargetFileName = SiteOrderInfo.IdStr + "_" + DocID.ToString() + ".pdf";
                FullName = filePath + TargetFileName;
                System.IO.File.WriteAllBytes(FullName, attachmentTerms);
            }
            // SAVE TERMS PDF DS20231012  <<<
            string OrderName = SiteOrderInfo.Name;
            if (SubContractorInfo != null) OrderName = SubContractorInfo.Name;
            string ConfirmStatus = "Created";
            if (SiteOrderInfo.isOrderApproved == 1) ConfirmStatus = "Approved";
            String message = "";
            //String message = "" +
            //   "The following Material Site Order has been submitted." + "<br />" +
            //   "<br />" +
            //   "Project: <b>" + UI.Utils.SetFormString(ProjectInfo.Name) + "</b><br />" +
            //   "Order: <b>" + UI.Utils.SetFormString(SiteOrderInfo.Title) + "</b><br />" +
            //   "Subcontractor: <b>" + UI.Utils.SetFormString(OrderName) + "</b><br />" +
            //   "Date/Time: <b>" + UI.Utils.SetFormDateTime(SiteOrderInfo.OrderDate) + "</b><br />" +
            //   "Order amount: <b>" + UI.Utils.SetFormDecimal(SiteOrderInfo.SubTotal) + "</b><br />" +

            //   "Order Status: <b>" + UI.Utils.SetFormString(ConfirmStatus) + "</b><br />" +

            //   "<br />" +
            //   "<i>SOS++</i><br />";
            message = UI.Utils.getSiteOrderEmail(SiteOrderInfo.Typ);
            message = message.Replace("$PROJECTNAME$", ProjectInfo.Name);
            message = message.Replace("$PROJECTTITLE$", UI.Utils.SetFormString(SiteOrderInfo.Title));
            message = message.Replace("$FORMANNAME$", UI.Utils.SetFormString(ForemanPeopleInfo.Name));
            message = message.Replace("$SUBCONTRACTORNAME$", UI.Utils.SetFormString(OrderName));
            message = message.Replace("$ORDERDATE$", UI.Utils.SetFormDateTime(SiteOrderInfo.OrderDate));
            message = message.Replace("$SUBTOTAL$", UI.Utils.SetFormDecimal(SiteOrderInfo.SubTotal));
            message = message.Replace("$ORDERSTATUS$", UI.Utils.SetFormString(ConfirmStatus));
            message = message.Replace("\r\n", "");
            //$PROJECTNAME$
            //$PROJECTTITLE$
            //$FORMANNAME$
            //$SUBCONTRACTORNAME$
            //$ORDERDATE$
            //$SUBTOTAL$
            //$ORDERSTATUS$
            string EmailTest = ConfigurationManager.AppSettings["EmailTest"].ToString();
            if (EmailTest != "")
            {
                //EmailPeopleInfo.Email = EmailTest;  DS20230329
                EmailPeopleInfo.Email = LoginPeopleInfo.Email;  // DS20230329
                if (SiteOrderInfo.SubContractorId == 0)
                {
                    Utils.SendEmailSiteOrder(LoginPeopleInfo, DummyPeopleInfoList, subject, message, attachment, attachmentName, attachmentTerms, attachmentNameTerms);
                 }
                else
                {
                    Utils.SendEmailSiteOrder(LoginPeopleInfo, DummyPeopleInfoList, subject, message, attachment, attachmentName, null, null);
                }
                return LoginPeopleInfo.Email;  // DS20230329
            }
            else
            {
                if (ContactPeopleInfo == null)   
                {
                 EmailPeopleInfo.Email = SiteOrderInfo.Email;
                 
                }
                else
                {
                    if (SiteOrderInfo.Email == null)  // DS20230729 >>>
                    {
                        EmailPeopleInfo.Email = ContactPeopleInfo.Email;
                    }
                    else
                    {
                        if (SiteOrderInfo.Email == "")
                        {
                            EmailPeopleInfo.Email = ContactPeopleInfo.Email;
                        }
                        else
                        {
                            EmailPeopleInfo.Email = SiteOrderInfo.Email;
                        }
                    }                                // DS20230729 <<<

                }
                DummyPeopleInfoList.Clear();
                DummyPeopleInfoList.Add(LoginPeopleInfo);
                if (SiteOrderInfo.SubContractorId == 0)  // DS20231012  SendEmailSiteOrder
                {
                    Utils.SendEmailSiteOrder(EmailPeopleInfo, DummyPeopleInfoList, subject, message, attachment, attachmentName, attachmentTerms, attachmentNameTerms);
                 }
                else
                {
                    Utils.SendEmailSiteOrder(EmailPeopleInfo, DummyPeopleInfoList, subject, message, attachment, attachmentName, null, null);
                }

                return EmailPeopleInfo.Email;  // DS20230329
            }


        }

    }

}
