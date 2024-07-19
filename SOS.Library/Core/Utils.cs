using System;
using System.IO;
using System.Xml;
using System.Net.Mail;
using System.Net.Mime;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Generic;
using System.Net;

namespace SOS.Core
{
    public static class Utils
    {

#region Constants
        public const String SelectedValueOther = "OT";
        public const String SelectedLabelOther = "Other";
        public const String SelectFile = "File";
        public const String SelectFolder = "Folder";
#endregion

#region Public Static Methods
        /// <summary>
        /// Writes an error message to the PC log
        /// </summary>
        public static void LogError(String errMsg)
        {
            if (!EventLog.SourceExists("SOSApp"))
                EventLog.CreateEventSource("SOSApp", "Application");

            EventLog.WriteEntry("SOSApp", errMsg, EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes all the inner exceptions to the PC log
        /// </summary>
        public static void LogException(Exception exception)
        {
            while (exception != null)
            {
                LogError(exception.ToString());
                exception = exception.InnerException;
            }
        }

        /// <summary>
        /// Writes an information message to the PC log
        /// </summary>
        public static void LogInformation(String errMsg)
        {
            if (!EventLog.SourceExists("SOSApp"))
                EventLog.CreateEventSource("SOSApp", "Application");

            EventLog.WriteEntry("SOSApp", errMsg, EventLogEntryType.Information);
        }

        /// <summary>
        /// Checks if an object is null and throws an exception
        /// </summary>
        public static void CheckNullObject(Object obj, String parameterId, String objectType)
        {
            if (obj == null)
            {
                throw new Exception(objectType + " with Id: " + parameterId + " not found.");
            }
        }

        public static Decimal GetDecimalValue(Decimal? theValue)
        {
            return theValue == null ? 0 : (Decimal)theValue;
        }

        public static void addAttachment(MailMessage mailMessage, Byte[] attachmentData, String attachmentName)
        {
            if (attachmentData != null)
            {
                Attachment attachment;
                MemoryStream ms = new System.IO.MemoryStream();

                ms.Write(attachmentData, 0, attachmentData.Length);
                ms.Position = 0;

                attachment = new Attachment(ms, attachmentName, MediaTypeNames.Application.Pdf);

                attachment.ContentDisposition.CreationDate = DateTime.Now;
                attachment.ContentDisposition.ModificationDate = DateTime.Now;
                attachment.ContentDisposition.ReadDate = DateTime.Now;

                mailMessage.Attachments.Add(attachment);
            }
        }       

        /// <summary>
        /// Sends an email with attachment data, files and list of CC
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfoFrom, PeopleInfo peopleInfo, List<PeopleInfo> peopleInfoList, String subject, String message, Byte[] attachmentData1, String attachmentName1, Byte[] attachmentData2, String attachmentName2, List<FileInfo> fileInfoList)
        {
            MailMessage mailMessage;
            MailAddress mailAddressFrom;
            MailAddress mailAddressTo;
            Attachment attachment;
            String emailFrom;

            String emailSmtpServer = ConfigurationManager.AppSettings["EmailSmtpServer"].ToString();
            String emailRedirect = ConfigurationManager.AppSettings["EmailRedirect"].ToString();
            SmtpClient smtpClient = new SmtpClient(emailSmtpServer);
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("vaughanconstructions@vaughans.com.au", "yHgRDr54325");
            smtpClient.Timeout = 600000;
//          System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(768 | 3072);
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(768 | 3072);

            if (peopleInfoFrom != null)
            {
                emailFrom = peopleInfoFrom.Name;
                mailAddressFrom = new MailAddress(peopleInfoFrom.Email, peopleInfoFrom.Name);
            }
            else
            {
                emailFrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                mailAddressFrom = new MailAddress(emailFrom, "Vaughan Constructions");   // DS20231212
            }

            if (emailRedirect != String.Empty)
                mailAddressTo = new MailAddress(emailRedirect, peopleInfo.Name + " (Redirected)");
            else if (peopleInfo == null)
                throw new Exception("Cannot send email. People record is null");
            else if (peopleInfo.Email == null)
                throw new Exception("Cannot send email. Email is null in people record: " + peopleInfo.Name + ".");
            else
                mailAddressTo = new MailAddress(peopleInfo.Email, peopleInfo.Name);

            mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            //#--
            //mailMessage.Body = message
            mailMessage.Body = message.Replace("\r\n","<br />");
            //#----


            if (peopleInfoList != null)
                foreach (PeopleInfo peopleInfoCC in peopleInfoList)
                    if (emailRedirect != String.Empty)
                        mailMessage.CC.Add(new MailAddress(emailRedirect, peopleInfoCC.Name));
                    else
                        if (peopleInfoCC.Email != null)
                            mailMessage.CC.Add(new MailAddress(peopleInfoCC.Email, peopleInfoCC.Name));

            addAttachment(mailMessage, attachmentData1, attachmentName1);
            addAttachment(mailMessage, attachmentData2, attachmentName2);

            if (fileInfoList != null)
            {
                foreach (FileInfo fileInfo in fileInfoList)
                {
                    attachment = new Attachment(fileInfo.FullName, MediaTypeNames.Application.Octet);

                    attachment.ContentDisposition.CreationDate = fileInfo.CreationTime;
                    attachment.ContentDisposition.ModificationDate = fileInfo.LastWriteTime;
                    attachment.ContentDisposition.ReadDate = fileInfo.LastAccessTime;
                    attachment.ContentDisposition.Size = fileInfo.Length;
                    attachment.Name = fileInfo.Name;

                    mailMessage.Attachments.Add(attachment);
                }
            }

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error Sending Email.");
            }
        }
        public static void SendEmailSiteOrder(PeopleInfo peopleInfoFrom, PeopleInfo peopleInfo, List<PeopleInfo> peopleInfoList, String subject, String message, Byte[] attachmentData1, String attachmentName1, Byte[] attachmentData2, String attachmentName2, List<FileInfo> fileInfoList)
        {
            MailMessage mailMessage;
            MailAddress mailAddressFrom;
            MailAddress mailAddressTo;
            Attachment attachment;
            String emailFrom;

            String emailSmtpServer = ConfigurationManager.AppSettings["EmailSmtpServer"].ToString();
            String emailRedirect = ConfigurationManager.AppSettings["EmailRedirect"].ToString();
            SmtpClient smtpClient = new SmtpClient(emailSmtpServer);
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("vaughanconstructions@vaughans.com.au", "yHgRDr54325");
            smtpClient.Timeout = 600000;
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(768 | 3072);

            if (peopleInfoFrom != null)
            {
                emailFrom = peopleInfoFrom.Name;
                mailAddressFrom = new MailAddress(peopleInfoFrom.Email, peopleInfoFrom.Name);
            }
            else
            {
                emailFrom = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                mailAddressFrom = new MailAddress(emailFrom, "SOS Server<br />Vaughan Constructions");   //DS20231214
            }

            if (emailRedirect != String.Empty)
                mailAddressTo = new MailAddress(emailRedirect, peopleInfo.Name);
            else
            if (peopleInfo == null)
                throw new Exception("Cannot send email. People record is null");
            else if (peopleInfo.Email == null)
                throw new Exception("Cannot send email. Email is null in people record: " + peopleInfo.Name + ".");
            else
                mailAddressTo = new MailAddress(peopleInfo.Email, peopleInfo.Name);

            mailMessage = new MailMessage(mailAddressFrom, mailAddressTo);
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;

            //#--
            //mailMessage.Body = message
            mailMessage.Body = message.Replace("\r\n", "<br />");
            //#----


            if (peopleInfoList != null)
                foreach (PeopleInfo peopleInfoCC in peopleInfoList)
                    if (emailRedirect != String.Empty)
                        mailMessage.CC.Add(new MailAddress(emailRedirect, peopleInfoCC.Name));
                    else
                        if (peopleInfoCC.Email != null)
                        mailMessage.CC.Add(new MailAddress(peopleInfoCC.Email, peopleInfoCC.Name));

            addAttachment(mailMessage, attachmentData1, attachmentName1);
            addAttachment(mailMessage, attachmentData2, attachmentName2);

            if (fileInfoList != null)
            {
                foreach (FileInfo fileInfo in fileInfoList)
                {
                    attachment = new Attachment(fileInfo.FullName, MediaTypeNames.Application.Octet);

                    attachment.ContentDisposition.CreationDate = fileInfo.CreationTime;
                    attachment.ContentDisposition.ModificationDate = fileInfo.LastWriteTime;
                    attachment.ContentDisposition.ReadDate = fileInfo.LastAccessTime;
                    attachment.ContentDisposition.Size = fileInfo.Length;
                    attachment.Name = fileInfo.Name;

                    mailMessage.Attachments.Add(attachment);
                }
            }

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex.ToString());
                throw new Exception("Error Sending Email.");
            }
        }
        /// <summary>
        /// Sends an email with atachment data and files with CC
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfo, List<PeopleInfo> peopleInfoList, String subject, String message, Byte[] attachmentData, String attachmentName, List<FileInfo> fileInfoList)
        {
            SendEmail(null, peopleInfo, peopleInfoList, subject, message, attachmentData, attachmentName, null, null, fileInfoList);
        }

        /// <summary>
        /// Sends an email with atachment data and files
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfo, String subject, String message, Byte[] attachmentData, String attachmentName, List<FileInfo> fileInfoList)
        {
            SendEmail(null, peopleInfo, null, subject, message, attachmentData, attachmentName, null, null, fileInfoList);
        }
        /// <summary>
        /// Sends an email with two atachment data and files
        /// </summary>
        public static void SendEmailSiteOrder(PeopleInfo peopleInfo, List<PeopleInfo> peopleInfoList, String subject, String message, Byte[] attachmentData1, String attachmentName1, Byte[] attachmentData2, String attachmentName2)
        {
            SendEmail(null, peopleInfo, peopleInfoList, subject, message, attachmentData1, attachmentName1, attachmentData2, attachmentName2, null);
        }


        /// <summary>
        /// Sends an email with two atachment data and files
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfo, List<PeopleInfo> peopleInfoList, String subject, String message, Byte[] attachmentData1, String attachmentName1, Byte[] attachmentData2, String attachmentName2)
        {
            SendEmail(null, peopleInfo, peopleInfoList, subject, message, attachmentData1, attachmentName1, attachmentData2, attachmentName2, null);
        }

        /// <summary>
        /// Sends an email with atachment data
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfo, String subject, String message, Byte[] attachmentData, String attachmentName)
        {
            SendEmail(null, peopleInfo, null, subject, message, attachmentData, attachmentName, null, null, null);
        }

        /// <summary>
        /// Sends and email with no attachment
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfo, String subject, String message)
        {
            SendEmail(null, peopleInfo, null, subject, message, null, null, null, null, null);
        }

        /// <summary>
        /// Sends and email to support account
        /// </summary>
        public static void SendEmailToSupport(PeopleInfo peopleInfo, String subject, String message)
        {
            EmployeeInfo employeeInfo = new EmployeeInfo();

            employeeInfo.FirstName= "Tech";
            employeeInfo.LastName = "Support";
            employeeInfo.Email = ConfigurationManager.AppSettings["EmailSupport"].ToString();

            SendEmail(peopleInfo, employeeInfo, null, subject, message, null, null, null, null, null);
        }

        /// <summary>
        /// Sends and email with no attachment
        /// </summary>
        public static void SendEmail(PeopleInfo peopleInfoFrom, PeopleInfo peopleInfo, List<PeopleInfo> peopleInfoList, String subject, String message)
        {
            SendEmail(peopleInfoFrom, peopleInfo, peopleInfoList, subject, message, null, null, null, null, null);
        }

        /// <summary>
        /// Send the password remainder email
        /// </summary>
        public static void SendPasswordReminder(PeopleInfo peopleInfo)
        {
            String subject = "Your SOS Account Information";

            String message = "" +
               peopleInfo.FirstName + ", here is you password reminder:<br />" +
               "<br />" +
               "Login: <b>" + peopleInfo.Login + "</b><br />" +
               "Password: <b>" + peopleInfo.Password + "</b><br />" +
               "<br />" +
               "<br />" +
               "<i>SOS Server</i><br />";

            SendEmail(peopleInfo, subject, message);
        }

        /// <summary>
        /// Send the new account email to a user
        /// </summary>
        public static void SendPassword(PeopleInfo peopleInfo)
        {
            String url = peopleInfo is EmployeeInfo ? ConfigurationManager.AppSettings["BaseUrl"].ToString() : ConfigurationManager.AppSettings["SubcontractorUrl"].ToString();

            String subject = "New SOS Account";

            String message = "" +
               peopleInfo.FirstName + ", a new account has been created for you in SOS++:<br />" +
               "<br />" +
               "Login: <b>" + peopleInfo.Login + "</b><br />" +
               "Password: <b>" + peopleInfo.Password + "</b><br />" +
               "URL: <a href='" + url + "'>" + url + "</a><br />" +
               "<br />" +
               "You can change your password clicking on your name on the top right corner after you log in.<br />" +
               "<br />" +
               "<br />" +
               "<i>SOS Server</i><br />";

            //#-----If People  Type Client --CL

            if (peopleInfo.Type == "CL")
            {
               message = "" +
               peopleInfo.FirstName + ", a new account has been created for you in SOS++:<br />" +
               "<br />" +
               "Login: <b>" + peopleInfo.Login + "</b><br />" +
               "Password: <b>" + peopleInfo.Password + "</b><br />" +
               "URL: <a href='https://Clientsos.vaughans.com.au'>https://Clientsos.vaughans.com.au</a><br />" +
               "<br />" +
               "You can change your password clicking on your name on the top right corner after you log in.<br />" +
               "<br />" +
               "<br />" +
               "<i>SOS Server</i><br />";

            }


            SendEmail(peopleInfo, subject, message);
        }

        /// <summary>
        /// Adds a node containing an error description to a root element on an XML document
        /// </summary>
        public static void AddErrorMessageNode(XmlDocument xmlDocument, XmlElement xmlElementRoot, String msgError)
        {
            XmlElement xmlElementError = xmlDocument.CreateElement("Error", null);
            xmlElementError.SetAttribute("name", msgError);
            xmlElementRoot.AppendChild(xmlElementError);
        }

        /// <summary>
        /// Adds a node containing the name of a missing field to a root element on an XML document
        /// </summary>
        public static void AddMissingFieldNode(Object fieldInfo, XmlDocument xmlDocument, XmlElement xmlElementRoot, String fieldName)
        {
            if (fieldInfo == null)
                AddErrorMessageNode(xmlDocument, xmlElementRoot, fieldName);
        }

        public static DateTime NextMonthlySequenceThe15(DateTime startDate, int sequenceNumber)
        {
            DateTime tmpDate = startDate.AddMonths(sequenceNumber - 1);
            return new DateTime(tmpDate.Year, tmpDate.Month, 15);

        }

        public static DateTime NextMonthlySequenceLastDay(DateTime startDate, int sequenceNumber)
        {
            return startDate.AddMonths(sequenceNumber).AddDays(-1);
        }

        public static DateTime NextSemiMonthlySequenceThe15(DateTime startDate, int sequenceNumber)
        {
            DateTime tmpDate = startDate.AddMonths((int)(sequenceNumber / 2));
            return new DateTime(tmpDate.Year, tmpDate.Month, 15);

        }

        public static DateTime NextSemiMonthlySequenceLastDay(DateTime startDate, int sequenceNumber)
        {
            return startDate.AddMonths((int)(sequenceNumber / 2) + 1).AddDays(-1);
        }

        public static Decimal? AddValues(Decimal? total, Decimal? item, Boolean isSubtract = false)
        {
            if (total.HasValue)
            {
                if (item.HasValue)
                {
                    return isSubtract ? total.Value - item.Value : total.Value + item.Value;
                }
                else
                {
                    return total.Value;
                }
            }
            else
            {
                if (item.HasValue)
                {
                    return isSubtract ? - item.Value : item.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        public static Decimal AddValues(Decimal? total, Decimal itemValue)
        {
            return total.HasValue ? total.Value + itemValue : itemValue;
        }
#endregion

    }
}
