using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class HomePage : System.Web.UI.Page
    {
        SiteOrdersController SiteOrdersController = SiteOrdersController.GetInstance();

        #region Private Methods
        protected void cmdApprovals_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            int UserId = (int)Web.Utils.GetCurrentUserId();
            Response.Redirect("~/Modules/SiteOrders/EditSiteOrderApprovalsAll.aspx?UserId=" + UserId.ToString());
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdSiteOrders_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            int UserId = (int)Web.Utils.GetCurrentUserId();
            Response.Redirect("~/Modules/SiteOrders/SearchSiteOrdersUser.aspx?UserId=" + UserId.ToString());
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdSiteOrdersAll_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            int UserId = (int)Web.Utils.GetCurrentUserId();
            Response.Redirect("~/Modules/SiteOrders/SearchSiteOrdersAll.aspx?UserId=" + UserId.ToString());
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        protected void cmdSiteOrdersSub_Click(object sender, EventArgs e)
        {
            //   if (projectInfo.Id == null)
            int UserId = (int)Web.Utils.GetCurrentUserId();
            Response.Redirect("~/Modules/SiteOrders/SelectSiteOrderSub.aspx?UserId=" + UserId.ToString());
            //   else
            //       Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        private String DueDaysStyle(int? numDays)
        {
            if (numDays != null)
                if ((Int32)numDays > 0)
                    return "lstItemError";

            return String.Empty;
        }

        private HtmlTableCell CellText(String className, String innerText)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", className);
            cell.VAlign = "Top";
            cell.InnerText = innerText;
            return cell;
        }

        private HtmlTableCell CellTextCenter(String className, String innerText)
        {
            HtmlTableCell cell = CellText(className, innerText);
            cell.Align = "Center";
            cell.VAlign = "Top";
            return cell;
        }

        private HtmlTableCell CellHtmlText(String className, String innerHtml)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", className);
            cell.Align = "Center";
            cell.InnerHtml = innerHtml;
            return cell;
        }

        private HtmlTableCell CellLink(String className, String linkURL, String linkTitle)
        {
            HtmlTableCell cell = new HtmlTableCell();
            HyperLink hyperLink = new HyperLink();

            hyperLink.Attributes.Add("class", "frmLinkSmall");
            hyperLink.Text = linkTitle;
            hyperLink.NavigateUrl = linkURL;
            cell.Attributes.Add("class", className);
            cell.VAlign = "Top";
            cell.Controls.Add(hyperLink);

            return cell;
        }

        private HtmlTableCell CellOpen(String className, String linkURL)
        {
            HtmlTableCell cell = new HtmlTableCell();
            HyperLink hyperLink = new HyperLink();

            hyperLink.ImageUrl = "~/Images/IconView.gif";
            hyperLink.ToolTip = "Open";
            hyperLink.NavigateUrl = linkURL;

            cell.Attributes.Add("class", className);
            cell.VAlign = "Top";
            cell.Controls.Add(hyperLink);

            return cell;
        }

        private Boolean IncludeProcessStepDetails(String currProcess)
        {
            return currProcess != ProcessInfo.ProcessTransmittal && currProcess != ProcessInfo.ProcessRFI && currProcess != ProcessInfo.ProcessEOT && currProcess != ProcessInfo.ProcessCreateClaim && currProcess != ProcessInfo.ProcessParticipation && currProcess != ProcessInfo.ProcessProject;
        }

        private HtmlTableRow HeaderRow(String currProcess)
        {
            HtmlTableRow row = new HtmlTableRow();

            switch (currProcess)
            {
                case ProcessInfo.ProcessProject:
                    row.Cells.Add(CellText("lstHeader", ""));
                    row.Cells.Add(CellText("lstHeader", "Project File"));
                    row.Cells.Add(CellText("lstHeader", "Target Date"));
                    row.Cells.Add(CellText("lstHeader", "Due Days"));
                    break;

                case ProcessInfo.ProcessClaim:
                    row.Cells.Add(CellText("lstHeader", "Number"));
                    break;

                case ProcessInfo.ProcessClientVariation:
                    row.Cells.Add(CellText("lstHeader", "Number"));
                    row.Cells.Add(CellText("lstHeader", "Name"));
                    break;
                case ProcessInfo.ProcessTenantVariation:             // DS20240213
                    row.Cells.Add(CellText("lstHeader", "Number"));
                    row.Cells.Add(CellText("lstHeader", "Name"));
                    break;

                case ProcessInfo.ProcessSeparateAccount:
                    row.Cells.Add(CellText("lstHeader", "Number"));
                    break;

                case ProcessInfo.ProcessComparison:
                case ProcessInfo.ProcessContract:
           //#---
                case ProcessInfo.ProcessMissingSignedContractFile:
          //#---
                    row.Cells.Add(CellText("lstHeader", "Trade"));
                    break;
             
                case ProcessInfo.ProcessVariation:
                    row.Cells.Add(CellText("lstHeader", "Trade"));
                    row.Cells.Add(CellText("lstHeader", "Title"));
                    break;

                case ProcessInfo.ProcessTransmittal:
                    row.Cells.Add(CellText("lstHeader", ""));
                    row.Cells.Add(CellText("lstHeader", "Subcontractor"));
                    row.Cells.Add(CellText("lstHeader", "Contact"));
                    row.Cells.Add(CellText("lstHeader", "Transmittal"));
                    break;

                case ProcessInfo.ProcessRFI:
                    row.Cells.Add(CellText("lstHeader", ""));
                    row.Cells.Add(CellText("lstHeader", "Number"));
                    row.Cells.Add(CellText("lstHeader", "Subject"));
                    row.Cells.Add(CellText("lstHeader", "Status"));
                    row.Cells.Add(CellText("lstHeader", "Raised date"));
                    row.Cells.Add(CellText("lstHeader", "Answer by Date"));
                    break;

                case ProcessInfo.ProcessEOT:
                    row.Cells.Add(CellText("lstHeader", ""));
                    row.Cells.Add(CellText("lstHeader", "Number"));
                    row.Cells.Add(CellText("lstHeader", "Cause"));
                    row.Cells.Add(CellText("lstHeader", "Delay Period"));
                    row.Cells.Add(CellText("lstHeader", "First Notice Date"));
                    row.Cells.Add(CellText("lstHeader", "Written Notice Date"));
                    break;

                case ProcessInfo.ProcessParticipation:
                    row.Cells.Add(CellText("lstHeader", ""));
                    row.Cells.Add(CellText("lstHeader", "Trade"));
                    row.Cells.Add(CellText("lstHeader", "Subcontractor"));
                    row.Cells.Add(CellText("lstHeader", "Due Date"));
                    break;
            }

            if (IncludeProcessStepDetails(currProcess))
            {
                row.Cells.Add(CellText("lstHeader", "Pending Task"));
                row.Cells.Add(CellText("lstHeader", "Target Date"));
                row.Cells.Add(CellText("lstHeader", "Due Days"));
                row.Cells.Add(CellText("lstHeader", "Comments"));
            }

            return row;
        }

        private HtmlTableRow DetailsRow(ProcessStepInfo processStepInfo, String currProcess, String currStyle)
        {
            HtmlTableRow row = new HtmlTableRow();

            switch (currProcess)
            {
                case ProcessInfo.ProcessProject:
                    row.Cells.Add(CellOpen(currStyle, String.Format("~/Modules/Projects/EditProjectFiles.aspx?ProjectId={0}", processStepInfo.Process.Project.IdStr)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Name)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.TargetDate)));
                    row.Cells.Add(CellHtmlText(currStyle, "<span class='" + DueDaysStyle(processStepInfo.DueDays) + "'>" + UI.Utils.SetFormInteger(processStepInfo.DueDays) + "</span>"));
                    break;

                case ProcessInfo.ProcessClaim:
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormInteger(processStepInfo.Process.Project.Claims[0].Number)));
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Projects/ViewClaim.aspx?ClaimId={0}", processStepInfo.Process.Project.Claims[0].IdStr), processStepInfo.Name));
                    break;

                case ProcessInfo.ProcessClientVariation:
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormInteger(processStepInfo.Process.Project.ClientVariations[0].Number)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.ClientVariations[0].Name)));
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", processStepInfo.Process.Project.ClientVariations[0].IdStr), processStepInfo.Name));
                    break;

                case ProcessInfo.ProcessTenantVariation:        // DS20240213
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormInteger(processStepInfo.Process.Project.ClientVariations[0].Number)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.ClientVariations[0].Name)));
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", processStepInfo.Process.Project.ClientVariations[0].IdStr), processStepInfo.Name));
                    break;

                case ProcessInfo.ProcessSeparateAccount:
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormInteger(processStepInfo.Process.Project.ClientVariations[0].Number)));
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId={0}", processStepInfo.Process.Project.ClientVariations[0].IdStr), processStepInfo.Name));
                    break;

                case ProcessInfo.ProcessComparison:
                case ProcessInfo.ProcessContract:
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Trades[0].Name)));
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Projects/ViewProjectTrade.aspx?TradeId={0}", processStepInfo.Process.Project.Trades[0].IdStr), processStepInfo.Name));
                    break;


                //#----
                case ProcessInfo.ProcessMissingSignedContractFile:
                

                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Trades[0].Name)));

                    
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Projects/ViewProjectTrade.aspx?TradeId={0}", processStepInfo.Process.Project.Trades[0].IdStr), processStepInfo.Name));  //"CA - Upload Signed Contract file"
                    break;
                //#---


                case ProcessInfo.ProcessVariation:
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Trades[0].Name)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Trades[0].Contract.Description)));
                    row.Cells.Add(CellLink(currStyle, String.Format("~/Modules/Contracts/ViewSubContract.aspx?SubContractId={0}", processStepInfo.Process.Project.Trades[0].Contract.IdStr), processStepInfo.Name));
                    break;

                case ProcessInfo.ProcessTransmittal:
                    row.Cells.Add(CellOpen(currStyle, String.Format("~/Modules/Projects/ViewTransmittal.aspx?TransmittalId={0}", processStepInfo.Process.Project.Transmittals[0].IdStr)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Transmittals[0].SubContractorName)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Transmittals[0].ContactName)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Transmittals[0].Name)));
                    break;

                case ProcessInfo.ProcessRFI:
                    row.Cells.Add(CellOpen(currStyle, String.Format("~/Modules/Projects/ViewRFI.aspx?RFIId={0}", processStepInfo.Process.Project.RFIs[0].IdStr)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormInteger(processStepInfo.Process.Project.RFIs[0].Number)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.RFIs[0].Subject)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.RFIs[0].Status)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.Process.Project.RFIs[0].RaiseDate)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.Process.Project.RFIs[0].TargetAnswerDate)));
                    break;

                case ProcessInfo.ProcessEOT:
                    row.Cells.Add(CellOpen(currStyle, String.Format("~/Modules/Projects/ViewEOT.aspx?EOTId={0}", processStepInfo.Process.Project.EOTs[0].IdStr)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormInteger(processStepInfo.Process.Project.EOTs[0].Number)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.EOTs[0].Cause)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.EOTs[0].DelayPeriod)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.Process.Project.EOTs[0].FirstNoticeDate)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.Process.Project.EOTs[0].WriteDate)));
                    break;

                case ProcessInfo.ProcessCreateClaim:
                    row.Cells.Add(CellOpen(currStyle, String.Format("~/Modules/Projects/ListClaims.aspx?ProjectId={0}", processStepInfo.Process.Project.IdStr)));
                    break;

                case ProcessInfo.ProcessParticipation:
                    row.Cells.Add(CellOpen(currStyle, String.Format("~/Modules/Projects/ViewParticipation.aspx?ParticipationId={0}", processStepInfo.Process.Project.Trades[0].Participations[0].IdStr)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Trades[0].Name)));
                    row.Cells.Add(CellText(currStyle, UI.Utils.SetFormString(processStepInfo.Process.Project.Trades[0].Participations[0].SubContractor.ShortName)));
                    row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.Process.Project.Trades[0].Participations[0].QuoteDate)));
                    break;
            }

            if (IncludeProcessStepDetails(currProcess))
            {
                row.Cells.Add(CellTextCenter(currStyle, UI.Utils.SetFormDate(processStepInfo.TargetDate)));
                row.Cells.Add(CellHtmlText(currStyle, "<span class='" + DueDaysStyle(processStepInfo.DueDays) + "'>" + UI.Utils.SetFormInteger(processStepInfo.DueDays) + "</span>"));
                row.Cells.Add(CellText(currStyle + "Wrap", UI.Utils.SetFormString(processStepInfo.Comments)));
            }

            return row;
        }

        private void BuildForm()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps;
            Dictionary<int, List<EmployeeInfo>> selectableEmployees;
            Dictionary<int, EmployeeInfo> selectedUsers;
            Dictionary<String, String> dictionaryRoles;
            List<ProcessStepInfo> processStepInfoList;
            EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();
            List<ProjectInfo> projectInfoList = projectsController.GetActiveProjectsForEmployee(currentUser);
            String currProcess;
            String currStyle;
            Boolean firstDisplayed = false;
            HtmlTable table = null;
            HtmlTable tableTemp = null;
            HtmlTableRow row;
            HtmlTableCell cell;
            DropDownList ddl;
            String rolesScript = String.Empty;
            String rolesInfo = hidRolesInfo.Value;
            String baseClientId = hidRolesInfo.ClientID.Substring(0, hidRolesInfo.ClientID.Length - hidRolesInfo.ID.Length);

            if (projectInfoList.Count == 0)
                lblProjectsInfo.Visible = true;
            else
            {
                projectsController.InitializeHolidays();

                foreach (ProjectInfo projectInfo in projectInfoList)
                    projectsController.GetProjectPeopleNames(projectInfo);

                processController.GetEmpoyeesAndRoles(projectInfoList, (EmployeeInfo)Web.Utils.GetCurrentUser(), rolesInfo, out selectedUsers, out selectableEmployees, out dictionaryRoles);
                pendingProcessSteps = processController.GetPendingSteps(projectInfoList, selectedUsers);
                
                foreach (ProjectInfo projectInfo in projectInfoList)
                {
                    if (selectableEmployees[projectInfo.Id.Value].Count > 0)
                    {
                        ddl = new DropDownList();
                        ddl.ID = "ddlRole" + projectInfo.IdStr;
                        ddl.AutoPostBack = true;

                        foreach (EmployeeInfo employee in selectableEmployees[projectInfo.Id.Value])
                            if (employee.Id != null)
                                ddl.Items.Add(new ListItem(employee.NameAndRole, employee.Role + employee.IdStr));
                            else
                                ddl.Items.Add(new ListItem(employee.Role, employee.Role));

                        if (dictionaryRoles != null && dictionaryRoles.ContainsKey(projectInfo.IdStr))
                                ddl.SelectedValue = dictionaryRoles[projectInfo.IdStr];

                        rolesScript = rolesScript + String.Format("\"{0}:\" + document.getElementById(\"{1}\").options[document.getElementById(\"{1}\").selectedIndex].value + \",\" + ", projectInfo.IdStr, baseClientId + ddl.ClientID);

                        processStepInfoList = pendingProcessSteps[projectInfo.Id.Value];

                        // See if the project has pending steps or not to add it to the corresponding table
                        if (processStepInfoList.Count > 0)
                        {
                            tableTemp = tblTasks;

                            if (!firstDisplayed)
                                firstDisplayed = true;
                            else
                            {
                                //Project separator
                                row = new HtmlTableRow();
                                cell = new HtmlTableCell();
                                cell.Attributes.Add("colspan", "3");
                                cell.InnerHtml = "<hr />";
                                row.Cells.Add(cell);
                                tblTasks.Rows.Add(row);
                            }
                        }
                        else
                            tableTemp = tblNoTasks;

                        //Project name and roles ddl
                        row = new HtmlTableRow();
                        cell = new HtmlTableCell();
                        cell.Attributes.Add("colspan", "3");
                        cell.Attributes.Add("class", "lstTitle");
                        cell.InnerText = projectInfo.Name + " ";
                        cell.Controls.Add(ddl);
                        row.Cells.Add(cell);
                        tableTemp.Rows.Add(row);

                        currStyle = String.Empty;
                        currProcess = String.Empty;

                         
                        foreach (ProcessStepInfo processStepInfo in processStepInfoList)
                        {
                            currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";


                            if(processStepInfo.Process.Hide!=1) { 
                            if (currProcess != processStepInfo.ProcessName)
                            {
                                currProcess = processStepInfo.ProcessName;

                                //Process Name
                                row = new HtmlTableRow();
                                cell = new HtmlTableCell();
                                row.Cells.Add(cell);
                                cell = new HtmlTableCell();
                                cell.Attributes.Add("colspan", "2");
                                cell.Attributes.Add("class", "lstSubTitle");
                                cell.InnerText = currProcess;
                                row.Cells.Add(cell);
                                tblTasks.Rows.Add(row);

                                //Process Table
                                row = new HtmlTableRow();
                                cell = new HtmlTableCell();
                                cell.Attributes.Add("style", "width:25px;");
                                row.Cells.Add(cell);
                                cell = new HtmlTableCell();
                                cell.Attributes.Add("style", "width:25px;");
                                row.Cells.Add(cell);
                                cell = new HtmlTableCell();
                                table = new HtmlTable();
                                table.CellPadding = 4;
                                table.CellSpacing = 1;
                                table.Rows.Add(HeaderRow(currProcess));
                                cell.Controls.Add(table);
                                row.Cells.Add(cell);
                                tblTasks.Rows.Add(row);
                            }

                            if (processStepInfo.TargetDate != null)
                                if (currProcess == ProcessInfo.ProcessProject)
                                    processStepInfo.DueDays = projectsController.NumCalendarDays(processStepInfo.TargetDate);
                                else
                                    processStepInfo.DueDays = projectsController.NumBusinessDays(processStepInfo.TargetDate);

                            table.Rows.Add(DetailsRow(processStepInfo, currProcess, currStyle));
                            }
                        }
                    }
                }
            }

            divNoTasks.Visible = tblNoTasks.Rows.Count > 0;

            if (rolesScript != String.Empty)
            {
                rolesScript = "" +
                    "<script type=\"text/javascript\">" +
                    "function SOS_SaveProjectsRoles() {" +
                    "document.getElementById(\"" + hidRolesInfo.ClientID + "\").value = " + rolesScript.Substring(0, rolesScript.Length - 9) + 
                    "}" +
                    "</script>";

                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SOS_SaveProjectsRoles", rolesScript);
                Page.ClientScript.RegisterOnSubmitStatement(Page.GetType(), "SOS_SaveProjectsRoles_Submit", "SOS_SaveProjectsRoles();");
            }
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewPendingTasks);
                BuildForm();
                int UserId = (int)Web.Utils.GetCurrentUserId();
                int ApprovalsCnt = SiteOrdersController.SearchSiteOrderApprovalsCount(UserId);
                if (ApprovalsCnt == 0)
                  {
                    cmdApprovals.Visible = false;
                    lblApprovals.Visible = false;
                }
                else
                {
                    lblApprovals.Text = " Site Order Approvals (" + ApprovalsCnt.ToString() + ")    ";
                }
                lblOrders.Text = " My Site Orders   ";
                lblOrdersAll.Text = " All Site Orders   ";
                //lblOrdersSub.Text = " SubContractor Site Order View   ";
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
    
    }
}
