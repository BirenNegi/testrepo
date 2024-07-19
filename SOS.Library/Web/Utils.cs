using System;
using System.Configuration;
using System.IO;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.IO.Compression;
using SOS.Core;

namespace SOS.Web
{
    public class Utils
    {

#region Public Static Methods
		/// <summary>
		/// Logs a user in
		/// </summary>
		public static void LogInUser(PeopleInfo peopleInfo)
		{
			try
			{
                //#--if (!(peopleInfo is EmployeeInfo) && !(peopleInfo is ContactInfo))

                if (!(peopleInfo is EmployeeInfo) && !(peopleInfo is ContactInfo) && !(peopleInfo is ClientContactInfo)) //#---
					throw new Exception("Invalid people type!");

				PeopleController.GetInstance().UpdatePersonLastLogin(peopleInfo);

                // Uncoment to instrument users logged it
				// Instrumentation.UpdateLoggedInUsers(true);

				HttpContext.Current.Session.Add("CurrentUser", peopleInfo);
			}
			catch (Exception ex)
			{
				SOS.Core.Utils.LogError(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}

		/// <summary>
        /// Returns current user
        /// </summary>
        public static PeopleInfo GetCurrentUser()
        {
            if (HttpContext.Current != null)
                return (PeopleInfo)HttpContext.Current.Session["CurrentUser"];
            else
            {
                LocalDataStoreSlot localDataStoreSlot = Thread.GetNamedDataSlot("CurrentUser");
                return (PeopleInfo)Thread.GetData(localDataStoreSlot);
            }
        }

		/// <summary>
		/// Logs the user out
		/// </summary>
		public static void LogOutUser()
		{
            // Uncoment to add instrumentation to users logged on
			// Instrumentation.UpdateLoggedInUsers(false);

			HttpContext.Current.Session["CurrentUser"] = null;
		}
		
		/// <summary>
        /// Returns true if the current user is an employee
        /// </summary>
        /// <returns></returns>
        public static Boolean IsCurrentUserEmployee()
        {
            PeopleInfo peopleInfo = GetCurrentUser();

            if (peopleInfo != null)
                if (peopleInfo is EmployeeInfo)
                    return true;

            return false;
        }

        /// <summary>
        /// returns true if the current user in a contact
        /// </summary>
        /// <returns></returns>
        public static Boolean IsCurrentUserContact()
        {
            PeopleInfo peopleInfo = GetCurrentUser();

            if (peopleInfo != null)
                if (peopleInfo is ContactInfo)
                    return true;

            return false;
        }



        /// <summary>
        /// #--returns true if the current user in a Clientcontact
        /// </summary>
        /// <returns></returns>
        public static Boolean IsCurrentUserClientContact()
        {
            PeopleInfo peopleInfo = GetCurrentUser();

            if (peopleInfo != null)
                if (peopleInfo is ClientContactInfo)
                    return true;

            return false;
        }
        //--#---


        /// <summary>
        /// Return current user ID
        /// </summary>
        public static int? GetCurrentUserId()
        {
            PeopleInfo peopleInfo = GetCurrentUser();
            return peopleInfo != null ? peopleInfo.Id : null;
        }

        /// <summary>
        /// Get a list of items from the config file
        /// </summary>
        public static void GetConfigList(String sectionName, String listId, ListControl list, String selectValue, Boolean codeOnly)
        {
            String xmlConfigFile = HttpContext.Current.Request.PhysicalApplicationPath + "Config\\SOS.Config";
            String xmlQuery = "/SOS/" + sectionName + "/List[@ID='" + listId + "']/*";
            ListItem selectedItem = null;

            try
            {
                XPathDocument document = new XPathDocument(xmlConfigFile);
                XPathNavigator navigator = document.CreateNavigator();
                XPathNodeIterator nodeIterator = navigator.Select(xmlQuery);

                list.Items.Clear();
                while (nodeIterator.MoveNext())
                    if (codeOnly)
                        list.Items.Add(new ListItem(nodeIterator.Current.GetAttribute("Value", string.Empty), nodeIterator.Current.GetAttribute("Value", string.Empty)));
                    else
                        list.Items.Add(new ListItem(nodeIterator.Current.GetAttribute("Name", string.Empty), nodeIterator.Current.GetAttribute("Value", string.Empty)));

                if (selectValue != null && selectValue != Core.Utils.SelectedValueOther)
                {
                    selectedItem = list.Items.FindByValue(selectValue);
                    if (selectedItem == null)
                        list.Items.Add(new ListItem(selectValue, selectValue));

                    list.SelectedValue = selectValue;
                }
            }
            catch (Exception Ex)
            {
                Core.Utils.LogError(Ex.ToString());
                throw new Exception("Accessing config file.");
            }

        }

        /// <summary>
        /// Get a list of items from the config file
        /// </summary>
        public static void GetConfigList(String sectionName, String listId, ListControl list, String selectValue)
        {
            GetConfigList(sectionName, listId, list, selectValue, false);
        }

        /// <summary>
        /// Get a list of items from the config file adding the option All
        /// </summary>
        public static void GetConfigListAddAll(String sectionName, String listId, ListControl list, String selectValue)
        {
            GetConfigList(sectionName, listId, list, selectValue);
            
            list.Items.Insert(0, new ListItem("All", ""));

            if (selectValue == null)
                list.SelectedIndex = 0;
        }

        /// <summary>
        /// Get a list of items from the config file adding the option empty
        /// </summary>
        public static void GetConfigListAddEmpty(String sectionName, String listId, ListControl list, String selectValue)
        {
            GetConfigList(sectionName, listId, list, selectValue);
           
            list.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            
            if (selectValue == null)
                list.SelectedIndex = 0;
        }

        /// <summary>
        /// Get a list of items from the config file adding the option empty
        /// </summary>
        public static void GetConfigListAddEmptyAddOther(String sectionName, String listId, ListControl list, String selectValue)
        {
            GetConfigList(sectionName, listId, list, selectValue);

            list.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            list.Items.Insert(list.Items.Count, new ListItem(Core.Utils.SelectedLabelOther, Core.Utils.SelectedValueOther));

            if (selectValue == null)
                list.SelectedIndex = 0;
            else
                if (selectValue == Core.Utils.SelectedValueOther)
                    list.SelectedIndex = list.Items.Count - 1;
        }

        /// <summary>
        /// Get a list of codes from the config file adding the option empty
        /// </summary>
        public static void GetConfigListCodeAddEmpty(String sectionName, String listId, ListControl list, String selectValue)
        {
            GetConfigList(sectionName, listId, list, selectValue, true);

            list.Items.Insert(0, new ListItem(String.Empty, String.Empty));

            if (selectValue == null)
                list.SelectedIndex = 0;
        }
        
        /// <summary>
        /// Gets a list item name from the config file
        /// </summary>
        public static String GetConfigListItemName(String sectionName, String listId, String selectValue)
        {
            String xmlConfigFile = HttpContext.Current.Request.PhysicalApplicationPath + "Config\\SOS.Config";
            String xmlQuery = "/SOS/" + sectionName + "/List[@ID='" + listId + "']/ListItem[@Value='" + selectValue + "']";

            if (selectValue == null)
                return String.Empty;
            else
                try
                {
                    XPathDocument document = new XPathDocument(xmlConfigFile);
                    XPathNavigator navigator = document.CreateNavigator();
                    XPathNodeIterator listItem = navigator.Select(xmlQuery);

                    if (listItem.MoveNext())
                        return listItem.Current.GetAttribute("Name", String.Empty);
                    else
                        return "Undefined";
                }
                catch (Exception Ex)
                {
                    Core.Utils.LogError(Ex.ToString());
                    throw new Exception("Accessing config file.");
                }
        }

        /// <summary>
        /// Gets a list value from the config file
        /// </summary>
        public static String GetConfigListItemValue(String sectionName, String listId, String selectName)
        {
            String xmlConfigFile = HttpContext.Current.Request.PhysicalApplicationPath + "Config\\SOS.Config";
            String xmlQuery = "/SOS/" + sectionName + "/List[@ID='" + listId + "']/ListItem[@Name='" + selectName + "']";

            if (selectName == null)
                return String.Empty;
            else
                try
                {
                    XPathDocument document = new XPathDocument(xmlConfigFile);
                    XPathNavigator navigator = document.CreateNavigator();
                    XPathNodeIterator listItem = navigator.Select(xmlQuery);

                    if (listItem.MoveNext())
                        return listItem.Current.GetAttribute("Value", String.Empty);
                    else
                        return "Undefined";
                }
                catch (Exception Ex)
                {
                    Core.Utils.LogError(Ex.ToString());
                    throw new Exception("Accessing config file.");
                }
        }

        /// <summary>
        /// Gets a list item name from the config file or use the other value
        /// </summary>
        public static String GetConfigListItemNameAndOther(String sectionName, String listId, String selectValue, String otherValue)
        {
            if (selectValue != null)
                if (selectValue == Core.Utils.SelectedValueOther)
                    if (otherValue != null && otherValue != String.Empty)
                        return otherValue;
                    else
                        return Core.Utils.SelectedLabelOther;
                else
                    return GetConfigListItemName(sectionName, listId, selectValue);
            else
                return null;
        }

        /// <summary>
        /// Check if an specified parameter exist in the Request object
        /// </summary>
        public static String CheckParameter(String paramName)
        {
            if (HttpContext.Current.Request.Params[paramName] == null)
                throw new Exception("Parameter " + paramName + " not specified.");

            return (String)HttpContext.Current.Request.Params[paramName];
        }

        /// <summary>
        /// Check if an specified parameter exist in the Rquest object
        /// </summary>
        public static String CheckParameterWithSection(String paramName)
        {
            String parameterValue = HttpContext.Current.Request.Params[paramName];
            Int32 posSection;

            if (parameterValue == null)
                throw new Exception("Parameter " + paramName + " not specified.");

            posSection = parameterValue.IndexOf("#");
            if (posSection > 0)
                return parameterValue.Substring(0, posSection);
            else
                return parameterValue;
        }

        /// <summary>
        /// Check if an specified parameter type Integer exist in the Rquest object
        /// </summary>
        public static Int32 CheckParameterInt32(String paramName)
        {
            try
            {
                return Convert.ToInt32(CheckParameter(paramName));
            }
            catch (Exception Ex)
            {
                Core.Utils.LogError(Ex.ToString());
                throw new Exception("Parameter " + paramName + " invalid.");
            }
        }

        /// <summary>
        /// Redirects to an error page if there is an exception
        /// </summary>
        public static void ProcessPageLoadException(Page page, Exception Ex)
        {
            if (Ex is SessionException)
            {
                HttpContext.Current.Server.Transfer("~/Modules/Core/Login.aspx?Inf=SesErr");
            }
            else
            {
                if (Ex is SecurityException)
                {
                    HttpContext.Current.Server.Transfer("~/Modules/Core/Login.aspx?Inf=SecErr");
                }
                else
                {
                    HttpContext.Current.Session["LastError"] = Ex.Message;
                    HttpContext.Current.Server.Transfer("~/Modules/Core/Error.aspx");
                }
            }
        }

        /// <summary>
        /// Display error message opening a control
        /// </summary>
        public static void ProcessControlLoadException(Control control, Exception Ex)
        {
            Panel panel = new Panel();
            Label label = new Label();
            Label labelDetalle = new Label();

            panel.CssClass = "frmFormErrMsg";
            label.CssClass = "frmError";
            labelDetalle.CssClass = "frmText";

            label.Text = String.Format("Error loading {0}. ", control.ID);
            labelDetalle.Text = Ex.Message;

            panel.Controls.Add(label);
            panel.Controls.Add(labelDetalle);

            control.Controls.Clear();
            control.Controls.Add(panel);
        }

        /// <summary>
        /// Returns the absolute Url with the application path
        /// </summary>
        public static String AbsoluteURL(String pageName)
        {
            Uri uri = HttpContext.Current.Request.Url;
            String appPath = HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath + "/";

            // Example:  [http]  [://]  [www.domain.com]  [:]  [123]  [/]  [appPath] [Page]
            return String.Format("{0}://{1}:{2}/{3}{4}", uri.Scheme, uri.Host, uri.Port, appPath, pageName);
        }
 
        /// <summary>
        /// Select Company popup
        /// </summary>
         public static String PopupCompany(Page page, String idField, String nameField, String businesUnitIdStr)
        {
            String strURL = AbsoluteURL("Modules/SubContractors/SelectSubContractor.aspx");
        String strScript;

            if (businesUnitIdStr != null)
                strURL = strURL + "?BusinessUnitId=" + businesUnitIdStr;

            if (!page.ClientScript.IsClientScriptBlockRegistered("CompanyPopup"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "var CompanyPopup_IDControlID;\r" +
                "var CompanyPopup_NameControID;\r" +
                "function CompanyPopupReceive(IDValue, NameValue) {\r" +
                "  document.getElementById(CompanyPopup_IDControlID).value = IDValue;\r" +
                "  document.getElementById(CompanyPopup_NameControlID).value = NameValue;\r" +
                "}\r\r" +
                "function CompanyPopupShow(IDControlID, NameControlID) {\r" +
                "  var winw = 640;\r" +
                "  var winh = 480;\r" +
                "  var winl = (screen.width - winw) / 2;\r" +
                "  var wint = (screen.height - winh) / 2;\r" +
                "  CompanyPopup_IDControlID = IDControlID;\r" +
                "  CompanyPopup_NameControlID = NameControlID;\r" +
                "  CompanyPopupWindow = window.open('" + strURL + "', 'WinCompanyPopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
                "  CompanyPopupWindow.focus();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CompanyPopup", strScript);
            }

            return "javascript:CompanyPopupShow('" + idField + "', '" + nameField + "')";
        }
//public static String PopupCompany(Page page, String idField, String nameField, String businesUnitIdStr) 
//{
//    String strURL = AbsoluteURL("Modules/SubContractors/SelectSubContractor.aspx");
//    String strScript; 

//    if (businesUnitIdStr != null)
//        strURL = strURL + "?BusinessUnitId=" + businesUnitIdStr;

//    if (!page.ClientScript.IsClientScriptBlockRegistered("CompanyPopup"))
//    {
//        strScript = "" +
//        "<script language='JavaScript'>\r" +
//        "var CompanyPopup_IDControlID;\r" +
//        "var CompanyPopup_NameControID;\r" +
//        "function CompanyPopupReceive(IDValue, NameValue) {\r" +
//        "  document.getElementById(CompanyPopup_IDControlID).value = IDValue;\r" +
//        "  document.getElementById(CompanyPopup_NameControlID).value = NameValue;\r" +
//        "  document.getElementById(CompanyPopup_NameControlID).txtSubContractorName_TextChanged();\r" +
//        "}\r\r" +
//        "function CompanyPopupShow(IDControlID, NameControlID) {\r" +
//        "  var winw = 640;\r" +
//        "  var winh = 480;\r" +
//        "  var winl = (screen.width - winw) / 2;\r" +
//        "  var wint = (screen.height - winh) / 2;\r" +
//        "  CompanyPopup_IDControlID = IDControlID;\r" +
//        "  CompanyPopup_NameControlID = NameControlID;\r" +
//        "  CompanyPopupWindow = window.open('" + strURL + "', 'WinCompanyPopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
//        "  CompanyPopupWindow.focus();\r" +
//        "}\r" +
//        "</script>\r\r";

//        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CompanyPopup", strScript);
//    }

//    return "javascript:CompanyPopupShow('" + idField + "', '" + nameField + "')";
//}
public static String PopupCompanySub(Page page, String idField, String nameField, String businesUnitIdStr, String ProjectIdStr)
        {
            String strURL = AbsoluteURL("Modules/SubContractors/SelectSubContractorProj.aspx");
            String strScript;
            char c = (char)34;
            string q = c.ToString();
            if (businesUnitIdStr != null)
                strURL = strURL + "?BusinessUnitId=" + businesUnitIdStr;
                strURL = strURL + "&ProjectId=" + ProjectIdStr;
            // __doPostBack('CalendarButton', '') 
            if (!page.ClientScript.IsClientScriptBlockRegistered("CompanyPopup"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "var CompanyPopup_IDControlID;\r" +
                "var CompanyPopup_NameControID;\r" +
                "function CompanyPopupReceive(IDValue, NameValue) {\r" +
                "  document.getElementById(CompanyPopup_IDControlID).value = IDValue;\r" +
                "  document.getElementById(CompanyPopup_NameControlID).value = NameValue;\r" +
                "__doPostBack('txtSubContractorName', '');\r" +
                //  "  document.getElementById(CompanyPopup_NameControlID).TextChanged = " + q + "txtSubContractorName_TextChanged()" + q + ";\r" +
                //"document.getElementById(CompanyPopup_NameControlID).ontextchanged = txtSubContractorName_TextChanged;\r" +
                //"  document.getElementById(CompanyPopup_NameControlID).ontextchanged = function(){ prompt('Hello world');  };\r" + 
                "}\r\r" +
                "function CompanyPopupShow(IDControlID, NameControlID) {\r" +
                "  var winw = 640;\r" +
                "  var winh = 480;\r" +
                "  var winl = (screen.width - winw) / 2;\r" +
                "  var wint = (screen.height - winh) / 2;\r" +
                "  CompanyPopup_IDControlID = IDControlID;\r" +
                "  CompanyPopup_NameControlID = NameControlID;\r" +
                "  CompanyPopupWindow = window.open('" + strURL + "', 'WinCompanyPopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
                "  CompanyPopupWindow.focus();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CompanyPopup", strScript);
            }

            return "javascript:CompanyPopupShow('" + idField + "', '" + nameField + "')";
        }
        //public static String PopupCompanySub(Page page, String idField, String nameField, String businesUnitIdStr, String ProjectIdStr)  // DS20230316
        //{
        //    String strURL = AbsoluteURL("Modules/SubContractors/SelectSubContractor.aspx");
        //    String strScript;
        //    char c = (char)34;
        //    string q = c.ToString();
        //    if (businesUnitIdStr != null)
        //        strURL = strURL + "?BusinessUnitId=" + businesUnitIdStr;
        //    if (ProjectIdStr != null)
        //        strURL = strURL + "&ProjectId=" + ProjectIdStr;
        //    // __doPostBack('CalendarButton', '') 
        //    if (!page.ClientScript.IsClientScriptBlockRegistered("CompanyPopup"))
        //    {
        //        strScript = "" +
        //        "<script language='JavaScript'>\r" +
        //        "var CompanyPopup_IDControlID;\r" +
        //        "var CompanyPopup_NameControID;\r" +
        //        "function CompanyPopupReceive(IDValue, NameValue) {\r" +
        //        "  document.getElementById(CompanyPopup_IDControlID).value = IDValue;\r" +
        //        "  document.getElementById(CompanyPopup_NameControlID).value = NameValue;\r" +
        //        "__doPostBack('txtSubContractorName', '');\r" +
        //        //  "  document.getElementById(CompanyPopup_NameControlID).TextChanged = " + q + "txtSubContractorName_TextChanged()" + q + ";\r" +
        //        //"document.getElementById(CompanyPopup_NameControlID).ontextchanged = txtSubContractorName_TextChanged;\r" +
        //        //"  document.getElementById(CompanyPopup_NameControlID).ontextchanged = function(){ prompt('Hello world');  };\r" + 
        //        "}\r\r" +
        //        "function CompanyPopupShow(IDControlID, NameControlID) {\r" +
        //        "  var winw = 640;\r" +
        //        "  var winh = 480;\r" +
        //        "  var winl = (screen.width - winw) / 2;\r" +
        //        "  var wint = (screen.height - winh) / 2;\r" +
        //        "  CompanyPopup_IDControlID = IDControlID;\r" +
        //        "  CompanyPopup_NameControlID = NameControlID;\r" +
        //        "  CompanyPopupWindow = window.open('" + strURL + "', 'WinCompanyPopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
        //        "  CompanyPopupWindow.focus();\r" +
        //        "}\r" +
        //        "</script>\r\r";

        //        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CompanyPopup", strScript);
        //    }

        //    return "javascript:CompanyPopupShow('" + idField + "', '" + nameField + "')";
        //}
        public static void LogError(string v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Java Script that returns the selected company to the opener window
        /// </summary>
        public static String PopupSendCompanySub(Page page, String idValue, String nameValue)
        {
            String strScript;

            if (!page.ClientScript.IsClientScriptBlockRegistered("CompanyPopupSend"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "function CompanyPopupSend(IDValue, NameValue) {\r" +
                "  window.opener.CompanyPopupReceive(IDValue, NameValue);\r" +
                "  window.close();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CompanyPopupSend", strScript);
            }
            return "javascript:CompanyPopupSend('" + idValue + "', '" + nameValue + "')";
        }
        /// <summary>
        /// Java Script that returns the selected company to the opener window
        /// </summary>
        public static String PopupSendCompany(Page page, String idValue, String nameValue)
        {
            String strScript;

            if (!page.ClientScript.IsClientScriptBlockRegistered("CompanyPopupSend"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "function CompanyPopupSend(IDValue, NameValue) {\r" +
                "  window.opener.CompanyPopupReceive(IDValue, NameValue);\r" +
                "  window.close();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "CompanyPopupSend", strScript);
            }
            return "javascript:CompanyPopupSend('" + idValue + "', '" + nameValue + "')";
        }
        /// <summary>
        /// Select People popup
        /// </summary>
        public static String PopupPeople(Page page, String idField, String nameField, String peopleType, BusinessUnitInfo businessUnitInfo, string SubContractorId = "")
        {
            String strURL;
            String strScript;
            String parameter;
            if (SubContractorId != "")
            {
                parameter = "?SubContractorId=" + SubContractorId;
            }
            else
            { 
            if (businessUnitInfo != null && businessUnitInfo.Id != null)
                parameter = "?BusinessUnitId=" + businessUnitInfo.IdStr;
            else
                parameter = String.Empty;
             }
            if (peopleType == PeopleInfo.PeopleTypeEmployee)
                strURL = AbsoluteURL("Modules/People/SelectEmployee.aspx" + parameter);
            //#---
            else if (peopleType == PeopleInfo.PeopleTypeClientContact)
                strURL = AbsoluteURL("Modules/People/SelectClientContact.aspx" + parameter);
            //#---
            else if (peopleType == PeopleInfo.PeopleTypeSubContractor)                              // DS202211
                strURL = AbsoluteURL("Modules/People/SelectClientContactSub.aspx" + parameter);
            //#--
            else
                strURL = AbsoluteURL("Modules/People/SelectContact.aspx" + parameter);

            if (!page.ClientScript.IsClientScriptBlockRegistered("PeoplePopup"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "var PeoplePopup_IDControlID;\r" +
                "var PeoplePopup_NameControID;\r" +
                "function PeoplePopupReceive(IDValue, NameValue) {\r" +
                "  document.getElementById(PeoplePopup_IDControlID).value = IDValue;\r" +
                "  document.getElementById(PeoplePopup_NameControlID).value = NameValue;\r" +
                "}\r\r" +
                "function PeoplePopupShow(IDControlID, NameControlID) {\r" +
                "  var winw = 720;\r" +
                "  var winh = 380;\r" +
                "  var winl = (screen.width - winw) / 2;\r" +
                "  var wint = (screen.height - winh) / 2;\r" +
                "  PeoplePopup_IDControlID = IDControlID;\r" +
                "  PeoplePopup_NameControlID = NameControlID;\r" +
                "  PeoplePopupWindow = window.open('" + strURL + "', 'WinPeoplePopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
                "  PeoplePopupWindow.focus();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "PeoplePopup", strScript);
            }

            return "javascript:PeoplePopupShow('" + idField + "', '" + nameField + "')";
        }

        public static String PopupPeopleSub(Page page, String idField, String nameField, String peopleType,string SubContractorId)
        {
            String strURL;
            String strScript;
            String parameter;
            char c = (char)34;
            string q = c.ToString();
            //if (businessUnitInfo != null && businessUnitInfo.Id != null)
            //    parameter = "?BusinessUnitId=" + businessUnitInfo.IdStr;
            //else
            //    parameter = String.Empty; document.getElementById(PeoplePopup_IDControlID).value
            parameter = "?SubContractorId=" + SubContractorId;
    //        parameter = "?SubContractorId='<%#(HiddenField)Page.PreviousPage.FindControl(" + q + "SubContractorId" + q + "%>)";
            //if (peopleType == PeopleInfo.PeopleTypeEmployee)
            //    strURL = AbsoluteURL("Modules/People/SelectEmployee.aspx" + parameter);
            ////#---
            //else if (peopleType == PeopleInfo.PeopleTypeClientContact) HttpUtility.UrlEncode(Eval("Name").ToString())
            //NavigateUrl='<%# string.Format("~/Details.aspx?Id={0}&Name={1}&Country={2}", HttpUtility.UrlEncode(Eval("Id").ToString()), HttpUtility.UrlEncode(Eval("Name").ToString()), HttpUtility.UrlEncode(Eval("Country").ToString())) %> '                    Text = "View Details" />
            strURL = AbsoluteURL("Modules/People/SelectClientContactSub.aspx" + parameter);
            //strURL = "<%#String.Format(" + q + "http://localhost:1202/Modules/People/SelectClientContactSub.aspx?SubContractorId={0}" + q + ", Eval(" + q + "txtSubContractorId" + q + "))%>'";
            // strURL = "'<%# string.Format(" + q + "~/Modules/People/SelectClientContactSub.aspx?SubContractorId={0}" + q + ", HttpUtility.UrlEncode(Eval(" + q + "txtSubContractorId" + q + ").ToString()) %> '";
            //strURL = "'<%# string.Format(" + q + "~/Modules/People/SelectClientContactSub.aspx?SubContractorId={0}" + q + ", Eval(" + q + "txtSubContractorId.value" + q + ").ToString()) %> '";
            //   onclick = '<%# String.Format("ShowOrderDetails(\"{0}\",\"{1}\",\"{2}\")",Eval("OrderCode"),Eval("CustomerName"),Eval("CustomerMobileNumber"))%>'
            //#--
            //else
            //    strURL = AbsoluteURL("Modules/People/SelectContact.aspx" + parameter);
            //document.getElementById("SubContracttorId").value
           // strURL = AbsoluteURL("Modules/People/SelectClientContactSub.aspx");
            if (!page.ClientScript.IsClientScriptBlockRegistered("PeoplePopupSub"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "var PeoplePopup_IDControlID;\r" +
                "var PeoplePopup_NameControID;\r" +
                "function PeoplePopupReceive(IDValue, NameValue) {\r" +
                "  document.getElementById(PeoplePopup_IDControlID).value = IDValue;\r" +
                "  document.getElementById(PeoplePopup_NameControlID).value = NameValue;\r" +
                "__doPostBack('txtSubContractorName', '');\r" +
                //"  document.getElementById('cmdCheck').click();\r" +

                "}\r\r" +
                "function PeoplePopupShow(IDControlID, NameControlID) {\r" +
                "  var winw = 720;\r" +
                "  var winh = 380;\r" +
                "  var winl = (screen.width - winw) / 2;\r" +
                "  var wint = (screen.height - winh) / 2;\r" +
                "  PeoplePopup_IDControlID = IDControlID;\r" +
                "  PeoplePopup_NameControlID = NameControlID;\r" +
                "  PeoplePopupWindow = window.open('" + strURL + "', 'WinPeoplePopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
//              "  PeoplePopupWindow = window.open('" + strURL + "', 'WinPeoplePopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
                "  PeoplePopupWindow.focus();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "PeoplePopupSub", strScript);
            }

            return "javascript:PeoplePopupShow('" + idField + "', '" + nameField + "')";
        }



        //#---to get all contacts from Subcontractors from the Current Project--Used to send Transmittals to related subbies
        public static String PopupPeopleFromProject(Page page, String idField, String nameField, String peopleType, TransmittalInfo transmittalInfo)
        {
            String strURL;
            String strScript;
            String parameter;
            
            if (transmittalInfo != null && transmittalInfo.Project.IdStr != null)
                parameter = "?projectId=" + transmittalInfo.Project.IdStr+"&TransmittalId="+ transmittalInfo.IdStr;
            else
                parameter = String.Empty;

            if (peopleType == PeopleInfo.PeopleTypeEmployee)
                strURL = AbsoluteURL("Modules/People/SelectEmployee.aspx" + parameter);
            else
                strURL = AbsoluteURL("Modules/People/SelectContactFromProject.aspx" + parameter);

            if (!page.ClientScript.IsClientScriptBlockRegistered("PeoplePopup"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "var PeoplePopup_IDControlID;\r" +
                "var PeoplePopup_NameControID;\r" +
                "function PeoplePopupReceive(IDValue, NameValue) {\r" +
                "  document.getElementById(PeoplePopup_IDControlID).value = IDValue;\r" +
                "  document.getElementById(PeoplePopup_NameControlID).value = NameValue;\r" +
                "}\r\r" +
                "function PeoplePopupShow(IDControlID, NameControlID) {\r" +
                "  var winw = 850;\r" +
                "  var winh = 400;\r" +
                "  var winl = (screen.width - winw) / 2;\r" +
                "  var wint = (screen.height - winh) / 2;\r" +
                "  PeoplePopup_IDControlID = IDControlID;\r" +
                "  PeoplePopup_NameControlID = NameControlID;\r" +
                "  PeoplePopupWindow = window.open('" + strURL + "', 'WinPeoplePopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
                "  PeoplePopupWindow.focus();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "PeoplePopup", strScript);
            }

            return "javascript:PeoplePopupShow('" + idField + "', '" + nameField + "')";
        }

        //#------

        /// <summary>
        /// Java Script that returns the selected person to the opener window
        /// </summary>
        public static String PopupSendPeopleSub(Page page, String idValue, String nameValue)
        {
            String strScript;

            if (!page.ClientScript.IsClientScriptBlockRegistered("PeoplePopupSend"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "function PeoplePopupSend(IDValue, NameValue) {\r" +
                "  window.opener.PeoplePopupReceive(IDValue, NameValue);\r" +
                "  window.close();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "PeoplePopupSend", strScript);
            }

            return "javascript:PeoplePopupSend('" + idValue + "', '" + nameValue + "')";
        }


        /// <summary>
        /// Java Script that returns the selected person to the opener window
        /// </summary>
        public static String PopupSendPeople(Page page, String idValue, String nameValue)
        {
            String strScript;

            if (!page.ClientScript.IsClientScriptBlockRegistered("PeoplePopupSend"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "function PeoplePopupSend(IDValue, NameValue) {\r" +
                "  window.opener.PeoplePopupReceive(IDValue, NameValue);\r" +
                "  window.close();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "PeoplePopupSend", strScript);
            }

            return "javascript:PeoplePopupSend('" + idValue + "', '" + nameValue + "')";
        }

        /// <summary>
        /// Select file popup
        /// </summary>
        public static String PopupFile(Page page, String idField, String nameField, String fileFolder, String path)
        {
            String strURL = AbsoluteURL("Modules/Projects/SelectFile.aspx?FileFolder=[F]&Path=[P]");
            String strScript;

            if (!page.ClientScript.IsClientScriptBlockRegistered("FilePopup"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "var FilePopup_IDControlID;\r" +
                "var FilePopup_NameControID;\r" +
                "function FilePopupReceive(IDValue, NameValue) {\r" +
                "  var strName = NameValue;\r" +
                "  document.getElementById(FilePopup_IDControlID).value = IDValue;\r" +
                "  document.getElementById(FilePopup_NameControlID).value = NameValue;\r" +
                "}\r\r" +
                "function FilePopupShow(IDControlID, NameControlID, FileFolder, Path) {\r" +
                "  var winw = 640;\r" +
                "  var winh = 480;\r" +
                "  var winl = (screen.width - winw) / 2;\r" +
                "  var wint = (screen.height - winh) / 2;\r" +
                "  var url = '" + strURL + "';\r" +
                "  FilePopup_IDControlID = IDControlID;\r" +
                "  FilePopup_NameControlID = NameControlID;\r" +
                "  FilePopupWindow = window.open(url.replace('[F]', FileFolder).replace('[P]', Path), 'WinFilePopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=0, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\r" +
                "  FilePopupWindow.focus();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "FilePopup", strScript);
            }

            return "javascript:FilePopupShow('" + idField + "', '" + nameField + "', '" + fileFolder + "', '" + path.Replace("\\", "\\\\") + "')";
        }

        /// <summary>
        /// Java Script that returns the selected file/folder to the opener window
        /// </summary>
        public static String PopupSendFile(Page page, String idValue, String nameValue)
        {
            String strScript;

            if (!page.ClientScript.IsClientScriptBlockRegistered("FilePopupSend"))
            {
                strScript = "" +
                "<script language='JavaScript'>\r" +
                "function FilePopupSend(IDValue, NameValue) {\r" +
                "  window.opener.FilePopupReceive(IDValue, NameValue);\r" +
                "  window.close();\r" +
                "}\r" +
                "</script>\r\r";

                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "FilePopupSend", strScript);
            }

            return "javascript:FilePopupSend('" + idValue + "', '" + nameValue + "')";
        }
        
        /// <summary>
        /// Returns the path of the edit image
        /// </summary>
        public static String GetEditImage()
        {
            return AbsoluteURL("Images/IconEdit.gif");
        }

        /// <summary>
        /// Finds a control recursively from a root control
        /// </summary>
        public static Control FindControlRecursive(Control root, String id)
        {
            if (root.ID == id)
                return root;

            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);
                if (t != null)
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Creates a link to open a page in a new window
        /// </summary>
        public static String CreatePopUp(Page opener, String strURL)
        {
            if (!opener.ClientScript.IsClientScriptBlockRegistered("SOSPopupWindow"))
            {
                String strScript = "" +
                "<script language='JavaScript'>\n" +
                "function SOSPopupShow(strURL) {\n" +
                "  var winw = 800;\n" +
                "  var winh = 600;\n" +
                "  var winl = (screen.width - winw) / 2;\n" +
                "  var wint = (screen.height - winh) / 2;\n" +
                "  PopupWindow = window.open(strURL, 'WinPopupWindow', 'toolbar=0, location=0, directories=0, status=0, menubar=1, scrollbars=1, resizable=1, width=' + winw + ', height=' + winh + ', left=' + winl + ', top=' + wint);\n" +
                "  PopupWindow.focus();\n" +
                "}\n" +
                "</script>\n";

                opener.Page.ClientScript.RegisterClientScriptBlock(opener.Page.GetType(), "SOSPopupWindow", strScript);
            }

            return "javascript:SOSPopupShow('" + AbsoluteURL(strURL) + "')";
        }

        /// <summary>
        /// Sends a byte array as "application/pdf" content type to the client
        /// </summary>
        public static void SendPdfData(Byte[] pdfData, String fileName)
        {
            HttpResponse response = HttpContext.Current.Response;

            response.Clear();
            response.Expires = 0;
            response.Buffer = true;
            response.ContentType = "application/pdf";
            response.AddHeader("content-length", pdfData.Length.ToString());
            response.AddHeader("content-disposition", "inline; filename=" + fileName + ".pdf");
            response.BinaryWrite(pdfData);
            response.End();           
        }
        public static void SavePDFData(Byte[] pdfData, String fileName)
        {
            HttpResponse response = HttpContext.Current.Response;

            response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
            response.ContentType = "application/octectstream";
            response.BinaryWrite(pdfData);
            response.End();


            //response.Clear();
            //response.Expires = 0;
            //response.Buffer = true;
            //response.ContentType = "application/pdf";
            //response.AddHeader("content-length", pdfData.Length.ToString());
            //response.AddHeader("content-disposition", "inline; filename=" + fileName + ".pdf");
            //response.BinaryWrite(pdfData);
            //response.End();
        }
        public static void SaveZIPData(Byte[] zippedFiles, String fileName)
        {
            HttpResponse Response = HttpContext.Current.Response;

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/x-compressed";
            Response.Charset = string.Empty;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(zippedFiles);
          //  Response.OutputStream.Flush();
           // Response.OutputStream.Close();
            Response.End();
        }

        /// <summary>
        /// Sends an stream as "application/x-unknown" content type to the client
        /// </summary>
        public static void SendFile(Byte[] fileData, String fileName)
        {
            HttpResponse response = HttpContext.Current.Response;

            response.Clear();
            response.Expires = 0;
            response.Buffer = true;
            response.ContentType = "application/x-unknown";
            response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            response.AddHeader("Content-Length", fileData.Length.ToString());
            response.BinaryWrite(fileData);
            response.Flush();
            response.Close();
        }

        /// <summary>
        /// Sends files as "application/x-unknown" content type to the client
        /// </summary>
        public static void SendFile(FileInfo fileInfo, String fileName)
        {
            HttpResponse response = HttpContext.Current.Response;
            String name = fileName != null ? fileName : fileInfo.Name;

            response.ClearContent();
            response.ClearHeaders();
            response.ContentType = "application/x-unknown";
            response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            response.AddHeader("Content-Length", fileInfo.Length.ToString());
            response.WriteFile(fileInfo.FullName);
            response.Flush();
            response.Close();
        }

        /// <summary>
        /// Sends files as "application/x-unknown" content type to the client
        /// </summary>
        public static void SendFile(FileInfo fileInfo)
        {
            SendFile(fileInfo, null);
        }

        /// <summary>
        /// Sends file with path from documents folder
        /// </summary>
        public static void SendFile(String path, String fileName)
        {
            SendFile(UI.Utils.FullPath(path, fileName));
        }

        /// <summary>
        /// Sends file from documents folder
        /// </summary>
        public static void SendFileWithName(String fullPath, String name)
        {
            FileInfo fileInfo;

            if (fullPath != null)
            {
                fileInfo = new FileInfo(fullPath);

                if(fileInfo.Exists)
                    SendFile(fileInfo, name);
                else
                    throw new Exception("File not found.");
            }
        }

        /// <summary>
        /// Sends file from documents folder
        /// </summary>
        public static void SendFile(String fullPath)
        {
            SendFileWithName(fullPath, null);
        }

        /// <summary>
        /// Adds a node to an Xml tree recursively
        /// </summary>
        public static void AddNode(XmlNode xmlNode, TreeNode treeNode)
        {
            treeNode.Text = xmlNode.Attributes["name"].Value;
            treeNode.SelectAction = TreeNodeSelectAction.None;
            treeNode.Expanded = xmlNode.Name == "root";

            if (xmlNode.HasChildNodes)
                for (int i = 0; i <= xmlNode.ChildNodes.Count - 1; i++)
                {
                    treeNode.ChildNodes.Add(new TreeNode());
                    AddNode(xmlNode.ChildNodes[i], treeNode.ChildNodes[i]);
                }
        }

        /// <summary>
        /// Return reports path
        /// </summary>
        public static String ReportsPath
        {
            get
            {
                return HttpContext.Current.Request.PhysicalApplicationPath + "Reports";
            }
        }

        public static object Strings { get; private set; }

        /// <summary>
        /// Changes sort direction
        /// </summary>
        public static SortDirection ChangeSortDirection(SortDirection sortDireccion)
        {
            return sortDireccion.Equals(SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }

        /// <summary>
        /// Sets the images for a sorted grid
        /// </summary>
        public static void SortedGridSetOrderImage(GridView gridView, GridViewRowEventArgs e, String sortExpression, SortDirection sortDirection)
        {
            Int32 numColumn = -1;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (DataControlField dataControlField in gridView.Columns)
                    if (dataControlField.SortExpression == sortExpression)
                        numColumn = gridView.Columns.IndexOf(dataControlField);

                if (numColumn != -1)
                {
                    Image image = new Image();
                    image.ImageUrl = sortDirection == SortDirection.Ascending ? "~/Images/IconUpDirection.png" : "~/Images/IconDownDirection.png";
                    image.Style.Add("padding-left", "5px");
                    e.Row.Cells[numColumn].Controls.Add(image);
                }
            }
        }

        /// <summary>
        /// Binds a a data table to a gridview
        /// </summary>
        public static void BindSortedGrid(GridView gridView, DataTable dataTable, String sortExpresion, SortDirection sortDirection)
        {
            DataView dataView = new DataView(dataTable);

            if (sortExpresion != null)
                if (sortDirection.Equals(SortDirection.Descending))
                    dataView.Sort = sortExpresion + " DESC";
                else
                    dataView.Sort = sortExpresion;

            gridView.DataSource = dataView;
            gridView.DataBind();
        }
        public static byte[] Compress(byte[] data)   //DS202211
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] data)   //DS202211
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        #endregion
        public string ShowSiteOrderDoc(string pDocId )
        {
            return "";

        }
    }
}
