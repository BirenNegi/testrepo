using SOS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SOS.UI;
using System.Threading;
using System.Configuration;
using System.IO;
//using SOS.Core;

namespace SOSBatch
{
    internal class clsSOSBatch
    {
        private static SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void Main(string[] args)
        {
            //
            // Send Mobile E-Mail
            //
            // String LogFolder = System.Configuration.ConfigurationManager.AppSettings["SOSBatchLogFile"].ToString();
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);
            String LogFolder = "D:/SOS/Prod/Logfiles/";
            DateTime LogDate = DateTime.Now;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(LogFolder, "SOSBatch.log"), true))
            {                outputFile.WriteLine(LogDate.ToString() + " SOSBatch Started");            }
            //string SendAddress = SiteOrdersController.SendSubmisionNotification("1",1);  // SiteOrderId  SenderUserId (User Logged in at time of Sending)
            while (1 == 1)
            {
                SiteOrdersController.SendMobileOrders();
                Thread.Sleep(15000);
                if(DateTime.Now.Hour == 5 && DateTime.Now.Minute == 0)
                {
                    SiteOrdersController.MaintenanceDaily();
                    Thread.Sleep(45000);
                    LogDate = DateTime.Now;
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(LogFolder, "SOSBatch.log"), true))
                    { outputFile.WriteLine(LogDate.ToString() + " MaintenanceDaily"); }
                }
            }

            //
            // Read Mobile Images -> Compress -> Convert to PDF
            //
            // SOS.UI.Utils.ConvertSiteOrderDocs();   // ### FUTURE ###
        }
        static void Main1(string[] args)
        {
        }
    }
}
