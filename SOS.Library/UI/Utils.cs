using System;
using System.Web;
using System.IO;
using System.Text;
using System.Configuration;

using Microsoft.Reporting.WebForms;

//using WebSupergoo.ABCpdf8;  //#-06/08/20
using WebSupergoo.ABCpdf11;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Xml;
using SOS.Core;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace SOS.UI
{
    public class Utils
    {

        #region Public Static Methods
        public static DateTime? GetFormDate(String strDate)
        {
            return strDate != null ? strDate.Trim() == "" ? null : (DateTime?)DateTime.Parse(strDate.Trim()) : null;
        }

        public static String GetFormString(String str)
        {
            return str != null ? str.Trim() == "" ? null : str.Trim() : null;
        }

        public static Int32? GetFormInteger(String str)
        {
            return str != null ? str.Trim() == "" ? null : (Int32?)Int32.Parse(str.Trim()) : null;
        }

        public static Decimal? GetFormDecimal(String str)
        {
            return str != null ? str.Trim() == "" ? null : (Decimal?)Decimal.Parse(str.Trim()) : null;
        }

        public static Decimal? GetFormPercentage(String str)
        {
            return str != null ? str.Trim() == "" ? null : (Decimal?)Math.Round(Decimal.Parse(str.Trim()) / 100, 4) : null;
        }

        public static float? GetFormFloat(String str)
        {
            return str != null ? str.Trim() == "" ? null : (float?)Math.Round(float.Parse(str.Trim()), 2) : null;
        }

        public static String SetFormString(String str)
        {
            return str == null ? String.Empty : str;
        }

        public static String SetFormStringMultiline(String str)
        {
            return str == null ? String.Empty : str.Replace("\n", "<br/>");
        }

        public static Boolean? GetFormYesNo(String str)
        {
            return str == "Y" ? true : str == "N" ? (Boolean?)false : null;
        }

        public static String SetFormDate(DateTime? date)
        {
            return date == null ? String.Empty : ((DateTime)date).ToShortDateString();
        }

        public static String SetFormDateTime(DateTime? date)
        {
            return date == null ? String.Empty : SetFormDate(date) + " " + ((DateTime)date).ToShortTimeString();
        }

        public static String SetFormInteger(int? number)
        {
            return number == null ? String.Empty : String.Format("{0:#,###0}", ((Int32)number));
        }

        public static String SetFormIntegerNoZero(int? number)
        {
            return number == null ? String.Empty : (Int32)number == 0 ? String.Empty : String.Format("{0:#,###0}", ((Int32)number));
        }

        public static String SetFormEditInteger(Decimal? number)
        {
            return number == null ? String.Empty : String.Format("{0:g}", ((Decimal)number));
        }

        public static String SetFormDecimal(Decimal? number)
        {
            return number == null ? String.Empty : String.Format("{0:c}", ((Decimal)number));
        }

        public static String SetFormFloat(float? number)
        {
            return number == null ? String.Empty : String.Format("{0:n2}", ((float)number));
        }

        public static String SetFormDecimalNoDecimals(Decimal? number)
        {
            return number == null ? String.Empty : String.Format("{0:0}", ((Decimal)number));
        }

        public static String SetFormEditDecimal(Decimal? number)
        {
            return number == null ? String.Empty : String.Format("{0:#,###0.00}", ((Decimal)number));
        }

        public static String SetFormPercentage(Decimal? number)
        {
            decimal fourDecimals;
            decimal twoDecimals;

            if (number != null)
            {
                fourDecimals = Math.Round((decimal)number, 4);
                twoDecimals = Math.Round((decimal)number, 2);

                if (fourDecimals == twoDecimals)
                    return String.Format("{0:0%}", twoDecimals);
                else
                    return String.Format("{0:0.00%}", fourDecimals);
            }
            else
            {
                return String.Empty;
            }
        }

        public static String SetFormEditPercentage(Decimal? number)
        {
            return number == null ? String.Empty : SetFormEditDecimal((Decimal)number * 100);
        }

        public static String SetFormYesNo(Boolean? boolean)
        {
            return boolean == null ? "?" : (Boolean)boolean ? "Y" : "N";
        }

        public static Boolean SetFormBoolean(Boolean? boolean)
        {
            return boolean == null ? false : (Boolean)boolean;
        }

        public static String SetFormURL(String str)
        {
            if (str != null)
            {
                if (str.IndexOf("http://") == -1)
                {
                    return "http://" + str;
                }
                else
                {
                    return str;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        public static String SetFormFileSize(long fileSize)
        {
            Double fileKB = (Double)fileSize / 1024;

            if (fileKB < 1024)
                return String.Format("{0:#,##0.00}", (fileKB)) + " KB";
            else
                return String.Format("{0:#,##0.00}", (fileKB / 1024)) + " MB";
        }

        public static String SetFormTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.Days >= 1)
                return timeSpan.Days + " " + (timeSpan.Days == 1 ? "day" : "days") + (timeSpan.Hours > 0 ? " " + timeSpan.Hours + " " + (timeSpan.Hours == 1 ? "hour" : "hours") : "");
            else if (timeSpan.Hours >= 1)
                return timeSpan.Hours + " " + (timeSpan.Hours == 1 ? "hour" : "hours") + (timeSpan.Minutes > 0 ? " " + timeSpan.Minutes + " " + (timeSpan.Minutes == 1 ? "minute" : "minutes") : "");
            else if (timeSpan.Minutes >= 1)
                return timeSpan.Minutes + " " + (timeSpan.Minutes == 1 ? "minute" : "minutes");
            else if (timeSpan.Seconds >= 1)
                return timeSpan.Seconds + " " + (timeSpan.Seconds == 1 ? "second" : "seconds");
            else
                return "-";
        }

        public static DateTime? DateLastMinute(DateTime? dateTime)
        {
            DateTime dateLastMinute;

            if (dateTime == null)
                return null;

            dateLastMinute = (DateTime)dateTime;

            return new DateTime(dateLastMinute.Year, dateLastMinute.Month, dateLastMinute.Day, 23, 59, 0);
        }

        public static String FileName(String filePath)
        {
            if (filePath != null)
            {
                String[] subPaths = filePath.Split('\\');
                return subPaths[subPaths.Length - 1];
            }

            return null;
        }

        public static String Path(String basePath, String filePath)
        {
            return filePath != null ? basePath != null ? basePath + "\\" + filePath : filePath : String.Empty;
        }

        public static String FullPath(String basePath, String filePath)
        {
            String documentsFolder = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();

            return filePath != null ? basePath != null ? documentsFolder + "\\" + basePath + "\\" + filePath : documentsFolder + "\\" + filePath : documentsFolder;
        }

        public static String PathQuotesFile(String basePath, String filePath)
        {
            String quotesFolder = ConfigurationManager.AppSettings["QuotesFilesFolder"].ToString();

            return filePath != null ? basePath != null ? basePath + "\\" + quotesFolder + "\\" + filePath : quotesFolder + "\\" + filePath : quotesFolder;
        }

        public static String FullPathQuotesFile(String basePath, String filePath)
        {
            String documentsFolder = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();
            String quotesFolder = ConfigurationManager.AppSettings["QuotesFilesFolder"].ToString();

            return filePath != null ? basePath != null ? documentsFolder + "\\" + basePath + "\\" + quotesFolder + "\\" + filePath : documentsFolder + "\\" + quotesFolder + "\\" + filePath : documentsFolder + "\\" + quotesFolder;
        }

        public static String TextToHtml(String text)
        {
            StringBuilder sb = new StringBuilder(text);

            sb.Replace("  ", " &nbsp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");

            StringReader sr = new StringReader(sb.ToString());
            StringWriter sw = new StringWriter();

            while (sr.Peek() > -1)
            {
                string temp = sr.ReadLine();
                sw.Write(temp + "<br />");
            }

            return sw.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Converts a number (0 .. 999) to text
        /// </summary>
        public static String NumberToText(UInt32 number)
        {
            String strhh;
            String strh1;
            String strh2;
            String strh3;
            String str = number.ToString();
            UInt32 h1 = number / 100;  // hundreds
            UInt32 h2 = number % 100;
            UInt32 h3 = h2 / 10;       // tens
            UInt32 h4 = h2 % 10;       // units

            switch (h1)
            {
                case 1: strh3 = "One Hundred"; break;
                case 2: strh3 = "Two Hundred"; break;
                case 3: strh3 = "Three Hundred"; break;
                case 4: strh3 = "Four Hundred"; break;
                case 5: strh3 = "Five Hundred"; break;
                case 6: strh3 = "Six Hundred"; break;
                case 7: strh3 = "Seven Hundred"; break;
                case 8: strh3 = "Eight Hundred"; break;
                case 9: strh3 = "Nine Hundred"; break;
                default: strh3 = ""; break;
            }
            switch (h3)
            {
                case 1: strh2 = "Ten"; break;
                case 2: strh2 = "Twenty"; break;
                case 3: strh2 = "Thirty"; break;
                case 4: strh2 = "Fourty"; break;
                case 5: strh2 = "Fifty"; break;
                case 6: strh2 = "Sixty"; break;
                case 7: strh2 = "Seventy"; break;
                case 8: strh2 = "Eighty"; break;
                case 9: strh2 = "Ninety"; break;
                default: strh2 = ""; break;
            }

            switch (h4)
            {
                case 1: strh1 = "One"; break;
                case 2: strh1 = "Two"; break;
                case 3: strh1 = "Three"; break;
                case 4: strh1 = "Four"; break;
                case 5: strh1 = "Five"; break;
                case 6: strh1 = "Six"; break;
                case 7: strh1 = "Seven"; break;
                case 8: strh1 = "Eight"; break;
                case 9: strh1 = "Nine"; break;
                default: strh1 = "Zero"; break;
            }

            //Eleven - Twelve - ... - ninetee
            if (strh2 == "Ten" && strh1 == "One")
            {
                strh1 = "";
                strh2 = "Eleven";
            }
            else if (strh2 == "Ten" && strh1 == "Two")
            {
                strh1 = "";
                strh2 = "Twelve";
            }
            else if (strh2 == "Ten" && strh1 == "Three")
            {
                strh1 = "";
                strh2 = "Thirteen";
            }
            else if (strh2 == "Ten" && strh1 == "Four")
            {
                strh1 = "";
                strh2 = "Fourteen";
            }
            else if (strh2 == "Ten" && strh1 == "Five")
            {
                strh1 = "";
                strh2 = "Fifteen";
            }
            else if (strh2 == "Ten" && strh1 == "Six")
            {
                strh1 = "";
                strh2 = "Sixteen";
            }
            else if (strh2 == "Ten" && strh1 == "Seven")
            {
                strh1 = "";
                strh2 = "Seventeen";
            }
            else if (strh2 == "Ten" && strh1 == "Eight")
            {
                strh1 = "";
                strh2 = "Eighteen";
            }
            else if (strh2 == "Ten" && strh1 == "Nine")
            {
                strh1 = "";
                strh2 = "Nineteen";
            }

            //special cases
            if (str.Length == 1)
                strhh = strh1;
            else if (strh1 == "Zero")
                strhh = strh3 + " " + strh2;
            else if (strh2 == "")
                strhh = strh3 + " " + strh2 + strh1;
            else if (str.Length == 2)
                strhh = strh3 + " " + strh2 + " " + strh1;
            else
                strhh = strh3 + " " + strh2 + " " + strh1;

            return strhh;
        }

        /// <summary>
        /// Converts a decimal 0 .. 999,999,999 to text
        /// </summary>
        public static String DecimalToText(Decimal amount)
        {
            String strDecimals;
            String strResult;
            String strDecimalsLabel;
            String strResutlLabel;

            if (amount > (Decimal)999999999.99 || amount < (Decimal)0)
                throw new Exception("Converting number to text. Valid range 0 to 999,999,999");

            String strAmount = Convert.ToString(Convert.ToDecimal(Math.Truncate(amount)));
            String strMult100 = Convert.ToString(Convert.ToDecimal(Math.Truncate(amount * 100)));

            if (strMult100.Length >= 2)
                strDecimals = NumberToText(UInt32.Parse(strMult100.Substring(strMult100.Length - 2, 2)));
            else
                strDecimals = NumberToText(UInt32.Parse(strMult100));

            if (strAmount.Length <= 3)
                strResult = NumberToText(UInt32.Parse(strAmount));
            else if (strAmount.Length <= 6)
            {
                String str1 = strAmount.Substring(0, strAmount.Length - 3);
                String str2 = strAmount.Substring(strAmount.Length - 3, 3);
                UInt32 Num1 = UInt32.Parse(str1);
                UInt32 Num2 = UInt32.Parse(str2);
                String subResult1 = NumberToText(Num1);
                String subResult2 = NumberToText(Num2);

                if (Num2 == 0)
                    strResult = subResult1 + " Thousand";
                else
                    strResult = subResult1 + " Thousand " + subResult2;
            }
            else if (strAmount.Length <= 9)
            {
                String str1 = strAmount.Substring(0, strAmount.Length - 6);
                String str2 = strAmount.Substring(strAmount.Length - 6, 3);
                String str3 = strAmount.Substring(strAmount.Length - 3, 3);
                UInt32 Num1 = UInt32.Parse(str1);
                UInt32 Num2 = UInt32.Parse(str2);
                UInt32 Num3 = UInt32.Parse(str3);
                String subResult1 = NumberToText(Num1);
                String subResult2 = NumberToText(Num2);
                String subResult3 = NumberToText(Num3);

                if (Num3 == 0)
                    if (Num2 == 0)
                        strResult = subResult1 + " Million";
                    else
                        strResult = subResult1 + " Million " + subResult2 + " " + "Thousand";
                else
                    if (Num2 == 0)
                    strResult = subResult1 + " Million " + subResult3;
                else
                    strResult = subResult1 + " Million " + subResult2 + " " + "Thousand " + subResult3;
            }
            else
                strResult = String.Empty;

            if (strResult == "One")
                strResutlLabel = "Dollar";
            else
                strResutlLabel = "Dollars";

            if (strDecimals == "One")
                strDecimalsLabel = "Cent";
            else
                strDecimalsLabel = "Cents";

            return (strResult + " " + strResutlLabel + " AND " + strDecimals + " " + strDecimalsLabel).ToUpper();
        }

        /// <summary>
        /// Converts HTML to PDF. 
        /// </summary>
        public static Byte[] HtmlToPDF(String htmlDoc, String footer)
        {
            String imagesUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "/Images";
            String defaultMedia = ConfigurationManager.AppSettings["defaultMedia"].ToString();


            Doc pdfDoc = new Doc();

            Int32 pdfId;

            htmlDoc = htmlDoc.Replace("../../Images", imagesUrl);
            pdfDoc.SetInfo(0, "License", @"X/VKS0cMn5FgsCJaaKuGb4z0LLRIQ4MYlq3wxL3
                                            FA0ojxkiVPH3rYMVWQ0lkwg8KCtYx4j5OuSAQr6I
                                            hQbB + xFItfGeZBnw92TFMO / XgBjbi1y7S5MlUFrj
                                            UWBKMcmImUL1oUMFb8wtwCFVZoTCQbGhYcSuWVW7
                                            qmqUR6D9AYuLEkoEsvCDi936EjbJciD01CzUO5TN
                                            89BzwIB4lU6hn8l1xjGH2KflZAcrhPl / RfOWcJxl
                                            I2w == ");
            //  pdfDoc.SetInfo(0, "License", "XeJREBodo / 8E43UDavmDZIv2LOJLPdtyprn4k / DzJkkN + XPcCXekP + IzByJH / 2QFBw ==");   // trial version

            // pdfDoc.SetInfo(0, "License", "719-253-057-515-4596-813 ABCpdf .NET Pro 7"); //--Old version 8

            try
            {

                pdfDoc.HtmlOptions.Engine = EngineType.Gecko;

                pdfDoc.MediaBox.String = defaultMedia;
                //pdfDoc.HtmlOptions.Timeout = 10000000; //ds20230806
                pdfDoc.Rect.Left = 10;
                pdfDoc.Rect.Width = 580;

                if (footer != null)
                {
                    pdfDoc.Rect.Bottom = 115;
                    pdfDoc.Rect.Height = 725;
                }
                else
                {
                    pdfDoc.Rect.Bottom = 10;
                    pdfDoc.Rect.Height = 830;
                }

                //Uncoment to see a grid or a frame on the pdf documents
                //pdfDoc.AddGrid();
                //spdfDoc.FrameRect();

                pdfDoc.AddPage();
                pdfId = pdfDoc.AddImageHtml(htmlDoc,true,203,true);

                pdfDoc.Rect.Bottom = 20;
                pdfDoc.Rect.Height = 800;

                for (int i = 1; i <= 100; i++)
                {
                    if (!pdfDoc.Chainable(pdfId))
                        break;

                    pdfDoc.Page = pdfDoc.AddPage();
                    pdfId = pdfDoc.AddImageToChain(pdfId);
                }

            //for (int i = 1; i <= pdfDoc.PageCount; i++)
            //{
            //    pdfDoc.PageNumber = i;
            //    pdfDoc.Flatten();
            //}

                // First page footer
                if (footer != null && footer.Length > 0)
                {
                    pdfDoc.PageNumber = 1;

                    pdfDoc.Rect.Bottom = 5;
                    pdfDoc.Rect.Height = 110;


                    pdfDoc.AddImageHtml(footer);

                }




                for (int i = 1; i <= pdfDoc.PageCount; i++)
                {
                   
                    pdfDoc.PageNumber = i;
                    //-----#-------
                    pdfDoc.AddText("\r\n \r\n" + "Page " + i + " of " + pdfDoc.PageCount);//-----#------

                    pdfDoc.Rect.String="100 30 550 80";
                    pdfDoc.HPos= 1.0;
                    pdfDoc.VPos = 0.5;
                    pdfDoc.FontSize = 6;


                    // pdfDoc.Flatten(); //-----#------
                }



            








            }

            catch (Exception Ex)
            {
                Core.Utils.LogError(Ex.ToString());
                //throw new Exception("HTML to PDF Report");
            }
            return pdfDoc.GetData();



                //for (int i = 1; i <= pdfDoc.PageCount; i++)
                //{
                //   
                //    pdfDoc.PageNumber = i;
                //    //-----#-------
                //    pdfDoc.AddText("\r\n \r\n" + "Page " + i + " of " + pdfDoc.PageCount);//-----#------

                //    pdfDoc.Rect.String="100 30 550 80";
                //    pdfDoc.HPos= 1.0;
                //    pdfDoc.VPos = 0.5;
                //    pdfDoc.FontSize = 6;


                    // pdfDoc.Flatten(); //-----#------
                //}

            //}

            //catch (Exception Ex)
            //{
            //    Core.Utils.LogError(Ex.ToString());
            //    throw new Exception("HTML to PDF Report");
            //}
            //return pdfDoc.GetData();
        }

        /// <summary>
        /// Returns pdf output from report
        /// </summary>
        public static Byte[] RdlcToPdf(LocalReport localReport) 
        {
            Byte[] pdfRenderOutput;
            String[] streams;
            Warning[] warnings;
            String deviceInfo;
            String mimetype;
            String encodign;
            String fileNameExtension;

            deviceInfo = "" +
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.25in</PageWidth>" +
            "  <PageHeight>11.75in</PageHeight>" +
            "  <MarginTop>0.25in</MarginTop>" +
            "  <MarginLeft>0.25in</MarginLeft>" +
            "  <MarginRight>0.25in</MarginRight>" +
            "  <MarginBottom>0.25in</MarginBottom>" +
            "</DeviceInfo>";

            localReport.EnableExternalImages = true;

            try
            {
                pdfRenderOutput = localReport.Render("PDF", deviceInfo, out mimetype, out encodign, out fileNameExtension, out streams, out warnings);
            }
            catch (Exception Ex)
            {
                Core.Utils.LogError(Ex.ToString());
                throw new Exception("Rendering Report");
            }

            return pdfRenderOutput;
        }
        /// <summary>
        ///         /// 
        /// </summary>
        /// <param name="thisPage"></param>
        /// <param name="pProjectId"></param>
        /// <param name="pOrderId"></param>
        /// <param name="pDocId"></param>
        /// <param name="pDocName"></param>
        /// <returns></returns>

        public static string showSiteOrderDoc(SOS.Web.SOSPage thisPage, string pProjectId, string pOrderId, string pDocId, String pDocName)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            SiteOrderDocInfo SOD = SiteOrdersController.GetSiteOrderDoc(Int32.Parse(pDocId));
            //File to be downloaded.
            string TargetFileName = pOrderId + "_" + pDocId;
            string[] Splits = pDocName.Split('.');
            if (Splits.Length > 1)
            {
                TargetFileName += "." + Splits[Splits.GetUpperBound(0)];
            }
            //Path of the File to be downloaded.  SiteOrdersSharepointFolder
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + "P" + pProjectId + "\\";
            if (SOD.isMobileUpload == 1 || SOD.isMobileUpload == 11)
            {
                DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersSharepointFolder"].ToString();
                filePath = @DocFolder + "\\";
                TargetFileName = SOD.DocName;
            } 
 
            //Server.MapPath(string.Format("~/Files/{0}", fileName));
            string FullName = filePath + TargetFileName;
            if (System.IO.File.Exists(FullName))
            {
                //
                // RESIZE TO PDF
                //
                 System.IO.FileStream fs = new System.IO.FileStream(FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] ar = new byte[(int)fs.Length];
                fs.Read(ar, 0, (int)fs.Length);
                fs.Close();
                //
                // RESIZE
                //
                if (SOD.isMobileUpload == 1 )
                {
                    string[] subs = SOD.DocName.Split('.');
                    string ext = subs[subs.Length - 1];
                    string extL = ext.ToLower();
                    string extFile = ext;
                    if (extL == "jpg" || extL == "tif" || extL == "png" || extL == "tiff")
                    {
                        PdfDocument document = new PdfDocument();
                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        double w = (double)page.Width;
                        double h = (double)page.Height;
                        System.Drawing.Image img2 = System.Drawing.Image.FromStream(new MemoryStream(ar));
                        if (img2.Width > img2.Height)   // DS20230725
                        {
                            img2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        int ImageScale = 1;
                        if (img2.Width > w * 2 || img2.Height > h * 2)     // DS20230725 * 2
                        {
                          ImageScale = (int)Convert.ToInt32(img2.Width / w / 1.5);  // DS20230725 * 2

                        }
                        System.Drawing.Image imgR = ResizeImage(img2, img2.Width / ImageScale, img2.Height / ImageScale);
                        MemoryStream ms1 = new MemoryStream();
                        imgR.Save(ms1, ImageFormat.Jpeg);

                        PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromStream(ms1);
                        double iw = img.PointWidth;
                        double ih = img.PointHeight;
                        if (w < iw)
                        {
                            ih = ih * (w / iw);
                            iw = w;
                        }
                        if (h < ih)
                        {
                            iw = iw * (h / ih);
                            ih = h;
                        }
                        MemoryStream ms2 = new MemoryStream();
                        gfx.DrawImage(img, 1, 1, iw, ih);
                        document.Save(ms2, true);
                        ar = ms2.ToArray();
                        SOD.DocName = SOD.DocName + ".pdf";
                        ms2.Close();
                        document.Close();

                    }
                }
                //
                // RESIZE TO PDF
                //

                //thisPage.Response.ContentType = "image/jpg";      DS2023-07-18  >>>
                //thisPage.Response.ContentType = "application/jpg";
                //thisPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + pDocName);

                ////Writing the File to Response Stream.
                //thisPage.Response.WriteFile(FullName);

                ////Flushing the Response.
                //thisPage.Response.Flush();
                //thisPage.Response.End();
                SOS.Web.Utils.SendFile(ar.ToArray(), SOD.DocName);
            }
            //Content Type and Header.
            return "";
        }
        public static string showSiteOrderDocs(SOS.Web.SOSPage thisPage, string pProjectId, string pOrderId, List<SiteOrderDocSearchInfo> SiteOrderDocSearchInfo)
        {
            // --------------
            SiteOrderInfo SiteOrderInfo = null;
            ProjectInfo ProjectInfo = null;
            SubContractorInfo SubContractorInfo = null;
            PeopleInfo ForemanPeopleInfo = null;
            PeopleInfo ContactPeopleInfo = null;
            PeopleInfo GivenByPeopleInfo = null;
            ProjectTradesInfo ProjectTradesInfo = null;
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            ProjectsController ProjectsController = ProjectsController.GetInstance();
            SubContractorsController SubContractorsController = SubContractorsController.GetInstance();
            PeopleController PeopleController = PeopleController.GetInstance();
            Byte[] pdfReport = null;
            Byte[] pdfTerms = null;

            //       try
            //       {
            Security.CheckAccess(Security.userActions.ViewSiteOrder);

            SiteOrderInfo = SiteOrdersController.GetSiteOrder(Int32.Parse(pOrderId));

            ProjectInfo = ProjectsController.GetProjectWithClientTrades(SiteOrderInfo.ProjectId);
            ProjectTradesInfo = SiteOrdersController.GetProjectTrades(SiteOrderInfo.ProjectId, SiteOrderInfo.TradesCode);  //DS20230822
            SiteOrderInfo.Items = SiteOrdersController.GetSiteOrderDetails(Int32.Parse(pOrderId));
            SubContractorInfo = SubContractorsController.GetSubContractor(SiteOrderInfo.SubContractorId);
            ForemanPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.ForemanID);
            ContactPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.ContactPeopleId);
            GivenByPeopleInfo = PeopleController.GetPersonById(SiteOrderInfo.GivenByPeopleId);

            SiteOrderInfo.isOrderApproved = SiteOrdersController.SiteOrderGetApprovalStatus(Int32.Parse(pOrderId));
            // 
            // GET PDF REPORT + TERMS
            //
            string BudgetName = "";

            //List<TradeTemplateInfo> tradeTemplates = TradesController.GetInstance().GetTradeTemplatesFromCode(SiteOrderInfo.TradesCode); //DS20230822
            //foreach (TradeTemplateInfo TradeTemplate in tradeTemplates) // DS20230308
            //{
            //    BudgetName = TradeTemplate.TradeCode + " - " + TradeTemplate.TradeDescription;
            //}
            if (ProjectTradesInfo != null)
            {
                BudgetName = ProjectTradesInfo.Code + " - " + ProjectTradesInfo.Name;   //DS20230822
            }
            // Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");
            if (SiteOrderInfo.Typ == "Mat")
            {
                pdfReport = SiteOrdersController.GenerateSiteOrderReport(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, ProjectTradesInfo);  //DS20230822
                //Utils.SendPdfData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
                //SOS.Web.Utils.SavePDFData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            }
            if (SiteOrderInfo.Typ == "Ins")
            {
                pdfReport = SiteOrdersController.GenerateSiteOrderReportIns(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, ProjectTradesInfo);//DS20230822
                //Utils.SendPdfData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
                //SOS.Web.Utils.SavePDFData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            }
            if (SiteOrderInfo.Typ == "Hir")
            {
                pdfReport = SiteOrdersController.GenerateSiteOrderReportHir(SiteOrderInfo, ProjectInfo, SubContractorInfo, ForemanPeopleInfo, ContactPeopleInfo, GivenByPeopleInfo, BudgetName, ProjectTradesInfo);//DS20230822
                // Utils.SendPdfData(pdfReport, String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
            }

            pdfTerms = SOS.UI.Utils.getSiteOrderTerms(SiteOrderInfo.Typ, ProjectInfo.BusinessUnitName);
            // --------------
            using (var compressedFileStream = new System.IO.MemoryStream())
            {
                //Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    // ADD ORDER = TERMS
                    //           var zipEntry = zipArchive.CreateEntry(String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));
                    //           var zipEntry2 = zipArchive.CreateEntry(String.Format("SiteOrder_{0}_{1}_{2}_TERMS.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr));

                    //Create a zip entry for ## ORDER ##
                    var zipEntryOrder = zipArchive.CreateEntry(GetFileCharacters(String.Format("SiteOrder_{0}_{1}_{2}.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr)));
                    //Get the stream of the attachment
                    using (var originalFileStream = new System.IO.MemoryStream(pdfReport))
                    {
                        using (var zipEntryStream = zipEntryOrder.Open())
                        {
                            //Copy the attachment stream to the zip entry stream
                            try { originalFileStream.CopyTo(zipEntryStream); }
                            catch (Exception)
                            { throw; }
                        }
                    }

                    //Create a zip entry for ## TERMS ##
                    if (SiteOrderInfo.SubContractorId == 0)
                    {
                       var zipEntryTerms = zipArchive.CreateEntry(GetFileCharacters(String.Format("SiteOrder_{0}_{1}_{2}_TERMS.pdf", ProjectInfo.Name, SiteOrderInfo.Title, SiteOrderInfo.IdStr)));
                    //Get the stream of the attachment
                       using (var originalFileStream = new System.IO.MemoryStream(pdfTerms))
                       {
                           using (var zipEntryStream = zipEntryTerms.Open())
                           {
                            //Copy the attachment stream to the zip entry stream
                            try { originalFileStream.CopyTo(zipEntryStream); }
                            catch (Exception)
                            { throw; }
                           }
                       }

                    }

                    foreach (var item in SiteOrderDocSearchInfo)
                    {
                        string TargetFileName = pOrderId + "_" + item.IdStr;
                        string[] Splits = item.DocName.Split('.');
                        SiteOrderDocInfo SOD = SiteOrdersController.GetSiteOrderDoc((int)item.Id);
                        if (Splits.Length > 1)
                        {
                            TargetFileName += "." + Splits[Splits.GetUpperBound(0)];
                        }
                        //Path of the File to be downloaded.
                        String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
                        string filePath = @DocFolder + "P" + pProjectId + "\\";
                        if (SOD.isMobileUpload == 1 || SOD.isMobileUpload == 11)
                        {
                            DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersSharepointFolder"].ToString();
                            filePath = @DocFolder + "\\";
                            TargetFileName = SOD.DocName;
                        }
                        string FullName = filePath + TargetFileName;
                        if (System.IO.File.Exists(FullName))
                        {
                            System.IO.FileStream fs = new System.IO.FileStream(FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                            byte[] ar = new byte[(int)fs.Length];
                            fs.Read(ar, 0, (int)fs.Length);
                            fs.Close();
                            //
                            // RESIZE
                            //
                            if (SOD.isMobileUpload == 1)
                            {  
                            string[] subs = item.DocName.Split('.');
                            string ext = subs[subs.Length - 1];
                            string extL = ext.ToLower();
                            string extFile = ext;
                            if (extL == "jpg" || extL == "tif" || extL == "png" || extL == "tiff")
                                {
                                    PdfDocument document = new PdfDocument();
                                    PdfPage page = document.AddPage();
                                    XGraphics gfx = XGraphics.FromPdfPage(page);
                                    double w = (double)page.Width;   // DS202307 IMAGE
                                    double h = (double)page.Height;   // DS202307 IMAGE
                                    System.Drawing.Image img2 = System.Drawing.Image.FromStream(new MemoryStream(ar));
                                    if (img2.Width > img2.Height)   // DS20230725
                                    {
                                        img2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                    }
                                    int ImageScale = 1;
                                    if (img2.Width > w * 2 || img2.Height > h * 2)     // DS20230725 * 2
                                    {
                                        ImageScale = (int)Convert.ToInt32(img2.Width / w / 1.5);  // DS20230725 * 2

                                    }
                                    System.Drawing.Image imgR = ResizeImage(img2, img2.Width / ImageScale, img2.Height / ImageScale);
                                    MemoryStream ms1 = new MemoryStream();
                                    imgR.Save(ms1, ImageFormat.Jpeg);

                                    PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromStream(ms1);
                                    double iw = img.PointWidth;
                                    double ih = img.PointHeight;
                                    if (w < iw)
                                    {
                                        ih = ih * (w / iw);
                                        iw = w;
                                    }
                                    if (h < ih)
                                    {
                                        iw = iw * (h / ih);
                                        ih = h;
                                    }
                                    MemoryStream ms2 = new MemoryStream();
                                    gfx.DrawImage(img, 1, 1, iw, ih);
                                    document.Save(ms2,true);
                                    ar = ms2.ToArray();
                                    item.DocName = item.DocName + ".pdf";
                                    ms2.Close();
                                    document.Close();
                                    
                                }
                            }

                                //Create a zip entry for each attachment
                                var zipEntry = zipArchive.CreateEntry(item.DocName);

                            //Get the stream of the attachment
                            using (var originalFileStream = new System.IO.MemoryStream(ar))
                            {
                                using (var zipEntryStream = zipEntry.Open())
                                {
                                    //Copy the attachment stream to the zip entry stream
                                    try
                                    {
                                        originalFileStream.CopyTo(zipEntryStream);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }


                                   
                                }
                            }
                        }
                    }
                }
                SOS.Web.Utils.SaveZIPData(compressedFileStream.ToArray(), String.Format("SiteOrder_{0}.zip", pOrderId));
            }
            return "";
        }
        //
        // Read Mobile Email Witing to Be Sent
        //
        public static string GetSiteOrderEmailToSend() // DS20231010
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            List<SiteOrderDocInfo> SODL = SiteOrdersController.SiteOrderMobileDocsSearch(0);
            int CNT = 0;
            foreach (SiteOrderDocInfo SOD in SODL)
            {
                CNT = CNT + 1;
                SOS.UI.Utils.ConvertSiteOrderDoc(SOD);
            }
            Console.WriteLine(CNT);
            return "";
        }

        //
        // Read Mobile Images -> Compress -> Convert to PDF
        //
        public static string ConvertSiteOrderDocs() // DS20231010
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
            List<SiteOrderDocInfo> SODL = SiteOrdersController.SiteOrderMobileDocsSearch(0);
            int CNT = 0;
            foreach (SiteOrderDocInfo SOD in SODL)
            {
                CNT = CNT + 1;
                SOS.UI.Utils.ConvertSiteOrderDoc(SOD);
            }
            Console.WriteLine(CNT);
            return "";
        }
        public static string ConvertSiteOrderDoc(SiteOrderDocInfo SOD)
        {
            SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
           // if (SOD.DocName.ToLower() == "1711c4e0-68a1-4602-819a-bdd42696b803.jpg")
           // {
                //File to be downloaded.
                string TargetFileName = "";
                string[] Splits = SOD.DocName.Split('.');
                bool isConverted = false;
            bool isError = false;
            if (Splits.Length > 1)
                {
                    TargetFileName += "." + Splits[Splits.GetUpperBound(0)];
                }
                //Path of the File to be downloaded.  SiteOrdersSharepointFolder
                String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersSharepointFolder"].ToString();
                string filePath = "";
                DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersSharepointFolder"].ToString();
                filePath = @DocFolder + "\\";
                TargetFileName = SOD.DocName;


                //Server.MapPath(string.Format("~/Files/{0}", fileName));
                string FullName = filePath + TargetFileName;
                if (System.IO.File.Exists(FullName))
                {

                    //
                    // RESIZE IF NEW FILE DOES NOT EXIST
                    //
                    string[] subs = SOD.DocName.Split('.');
                    string ext = subs[subs.Length - 1];
                    string extL = ext.ToLower();
                    string extFile = ext;
                    if (extL == "jpg" || extL == "tif" || extL == "png" || extL == "tiff")
                    {
                    try { 

                    if (System.IO.File.Exists(FullName + ".pdf") == false)
                        {
                            //
                            // RESIZE TO PDF
                            //
                            System.IO.FileStream fs = new System.IO.FileStream(FullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                            byte[] ar = new byte[(int)fs.Length];
                            fs.Read(ar, 0, (int)fs.Length);
                            PdfDocument document = new PdfDocument();
                            PdfPage page = document.AddPage();
                            XGraphics gfx = XGraphics.FromPdfPage(page);
                            double w = (double)page.Width;   // DS202307 IMAGE
                            double h = (double)page.Height;   // DS202307 IMAGE
                            System.Drawing.Image img2 = System.Drawing.Image.FromStream(new MemoryStream(ar));
                            if (img2.Width > img2.Height)   // DS20230725
                            {
                                img2.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            }
                            int ImageScale = 1;
                            if (img2.Width > w * 2 || img2.Height > h * 2)     // DS20230725 * 2
                            {
                                ImageScale = (int)Convert.ToInt32(img2.Width / w / 1.5);  // DS20230725 * 2

                            }
                            System.Drawing.Image imgR = ResizeImage(img2, img2.Width / ImageScale, img2.Height / ImageScale);
                            MemoryStream ms1 = new MemoryStream();
                            imgR.Save(ms1, ImageFormat.Jpeg);

                            PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromStream(ms1);
                            double iw = img.PointWidth;
                            double ih = img.PointHeight;
                            if (w < iw)
                            {
                                ih = ih * (w / iw);
                                iw = w;
                            }
                            if (h < ih)
                            {
                                iw = iw * (h / ih);
                                ih = h;
                            }
                            MemoryStream ms2 = new MemoryStream();
                            gfx.DrawImage(img, 1, 1, iw, ih);
                            document.Save(ms2, true);
                            ar = ms2.ToArray();
                            SOD.DocName = SOD.DocName + ".pdf";
                            ms2.Close();
                            //  document.Save(filePath + SOD.DocName);
                            document.Close();
                            //fs.Write(ar, 0, (int)ar.Length);
                            fs.Close();
                            System.IO.File.WriteAllBytes(FullName, ar); //ms1.ToArray()'


                            System.IO.File.Move(FullName, filePath + SOD.DocName);
                            isConverted = true;
                            // SOS.UI.Utils.ConvertSiteOrderDoc(SOD);
                            //SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
                            //fs = new System.IO.FileStream(filePath + SOD.DocName, System.IO.FileMode.Truncate, System.IO.FileAccess.ReadWrite);
                            //fs.Write(ar, 0, (int)ar.Length);
                            //fs.Close();
                        }
                  //  }
                         }
                    catch (Exception)
                    { isError = true; }
                }
                if (isConverted == true)
                {
                    int DocId = (int)SOD.Id;
                    SiteOrdersController.SiteOrderDocsRename(DocId, SOD.DocName, SOD.DocName, 11);
                }
                else
                {
                    if (isError == true)
                    {
                        int DocId = (int)SOD.Id;
                        SiteOrdersController.SiteOrderDocsRename(DocId, TargetFileName, TargetFileName, 111);
                    }
                }

                // write file back
            }
            //Content Type and Header.
            return "";
        }
        private static string GetFileCharacters(String InputString)
        {
            String Result = "";
            string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_ .";
            int i = 0;

            for (i = 0; i < InputString.Length; i++)
            {
                if (Chars.Contains(InputString.ElementAt(i)))
                {
                    Result += InputString.ElementAt(i);
                }
            }
            return Result;
        }
        public static byte[] getSiteOrderTerms(String pOrderType,String pBusinessUnitName)  // DS20231023
        {

            //File to be downloaded.
            //Path of the File to be downloaded.
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + "templates\\terms\\";
            string fileName = filePath + "TERMS_" + pOrderType + "_" + pBusinessUnitName + ".PDF";
            //Server.MapPath(string.Format("~/Files/{0}", fileName));
            if (!System.IO.File.Exists(fileName)) fileName = filePath + "TERMS_" + pOrderType + "_ALL.PDF";

            if (System.IO.File.Exists(fileName))
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] ar = new byte[(int)fs.Length];
                fs.Read(ar, 0, (int)fs.Length);
                fs.Close();
                return ar;
            }
            return null;
        }
        public static string getSiteOrderEmail(String pDocType)
        {

            string Letterhead = "";
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder + "templates\\email\\";
            //Server.MapPath(string.Format("~/Files/{0}", fileName));
            //Content Type and Header.
            string fileName = filePath + "EMAIL_" + pDocType + ".txt";
            if (!System.IO.File.Exists(fileName)) fileName = filePath + "EMAIL_ALL.txt";
            if (System.IO.File.Exists(fileName)) Letterhead = System.IO.File.ReadAllText(fileName);
            return Letterhead;

        }
        public static string UploadSiteOrderDoc(SOS.Web.SOSPage thisPage, System.Web.UI.WebControls.FileUpload FileUpload1, string pProjectId, string pOrderId, string pDocId, String pDocName)
        {
            // ================================
            string saveDir = @"P" + pProjectId + "\\";
            String DocFolder = System.Configuration.ConfigurationManager.AppSettings["SiteOrdersFolder"].ToString();
            string filePath = @DocFolder; // + "P" + parameterProjectId + "\\";
            string[] subs = FileUpload1.FileName.Split('.');
            string ext = subs[subs.Length - 1];
            string extL = ext.ToLower();
            string extFile = ext;

            if (extL == "jpg" || extL == "png" || extL == "tif" || extL == "tiff") extFile = "pdf";
            if (subs.Length > 1) subs[subs.Length - 1] = extFile;

            string DocName = string.Join(".", subs);

            string TargetFileName = pOrderId + "_" + pDocId + "." + extFile;
            //string savePath = appPath + saveDir + Server.HtmlEncode(FileUpload1.FileName);
            //FileUpload1.SaveAs(savePath);
            string savePath = filePath + saveDir;
            if (!Directory.Exists(savePath))
            {
                try
                {
                    DirectoryInfo di = Directory.CreateDirectory(savePath);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            if (Directory.Exists(savePath))
            {
                try
                {
                    if (extL == "jpg" || extL == "tif" || extL == "png" || extL == "tiff")
                    {
                        byte[] fileData = null;
                        // byte[] compressedData = null;
                        Stream fileStream = null;
                        int length = 0;

                        length = FileUpload1.PostedFile.ContentLength;
                        fileData = new byte[length + 1];
                        fileStream = FileUpload1.PostedFile.InputStream;
                        fileStream.Read(fileData, 0, length);
                        PdfDocument document = new PdfDocument();
                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        double w = (double)page.Width;   // DS202307 IMAGE
                        double h = (double)page.Height;   // DS202307 IMAGE

                        System.Drawing.Image img2 = System.Drawing.Image.FromStream(new MemoryStream(fileData));
                        int ImageScale = 1;
                        if (img2.Width > w || img2.Height > h)
                            {
                            ImageScale = (int)Convert.ToInt32(img2.Width / w);
                        }
                        System.Drawing.Image imgR = ResizeImage(img2, img2.Width / ImageScale, img2.Height / ImageScale);
                        MemoryStream ms1 = new MemoryStream();
                        imgR.Save(ms1, ImageFormat.Jpeg);

                        //XImage img = PdfSharp.Drawing.XBitmapImage.FromStream(new MemoryStream(fileData));
                        PdfSharp.Drawing.XImage img = PdfSharp.Drawing.XImage.FromStream(ms1);
                        //XImage img = PdfSharp.Drawing.XImage.FromStream(new MemoryStream(fileData));
                        //XImage img3 = new XImage.re(img2, new Size(img2.Width / 2, img2.Height / 2));

                         double iw = img.PointWidth;
                        double ih = img.PointHeight;


                        if (w < iw)
                        {
                            ih = ih * (w / iw);
                            iw = w;
                        }
                        if (h < ih)
                        {
                            iw = iw * (h / ih);
                            ih = h;
                        }

                        gfx.DrawImage(img, 1, 1, iw, ih);

                        document.Save(savePath + TargetFileName);

                    }
                    else
                    {
                        FileUpload1.SaveAs(savePath + TargetFileName);
                    }
                }
                catch (Exception)
                {

                    throw;
                }


                // =================================
         
            }
            return "";
        }
        private static System.Drawing.Image ResizeImage(System.Drawing.Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        public static String GetABNResultFromXML(string pXML)
        {
            char c2 = (char)13;
            string m = c2.ToString(); XmlDocument xmlDoc = new XmlDocument();
            string strXML = pXML.Replace("\r\n", m);
            string[] tresult = strXML.Split(m[0]);
            strXML = string.Join(m, tresult, 1, tresult.Length - 1);
            //"<?xml version=\"1.0\" encoding=\"utf-8\"?>
            StringReader sReader = new StringReader(strXML);
            System.Xml.XmlTextReader reader = new XmlTextReader(sReader);
            //string ABNName = LoadXMLSettings(Reader);
            reader.WhitespaceHandling = WhitespaceHandling.None; // ignore whitespace
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "exceptionDescription")
                        {
                            reader.Read();
                            string result = "ERROR:" + reader.Value;
                            reader.Close();

                            return result;
                        }
                        if (reader.Name == "mainName")
                        {
                            reader.Read();
                            reader.Read();
                            string result = reader.Value;
                            reader.Close();

                            return result;
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        break;
                }
                // Display all attributes.
                if (reader.HasAttributes)
                {
                    for (int i = 0; i < reader.AttributeCount; i++)
                    {
                        reader.MoveToNextAttribute();
                    }
                    // Attributes of a specific Element can be accessed
                    //     directly by name using:
                    String attribValue = reader.GetAttribute("version");
                }
            }
            return "ERROR:No response from ABN Registration site";
        }
        #endregion
    }
}
