using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportPurchaseSchedulePage : System.Web.UI.Page
    {

#region Private Methods
        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();

            if (chkAll.Checked)
                projectInfoList = projectsController.ListProjects();
            else
                projectInfoList = projectsController.ListActiveProjects();

            ddlProjects.Items.Clear();
          //  ddlProjects.Items.Add(new ListItem("All", String.Empty));
           // ddlProjects.Items.Add(new ListItem("All Active", "Active"));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));

            ddlProjects.SelectedValue = "Active";
        }

        private void BindForm()
        {
            List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
            List<SubContractorInfo> subContractorInfoList = SubContractorsController.GetInstance().ListSubContractors();
            List<EmployeeInfo> employeeInfoList = PeopleController.GetInstance().GetEmployeesWithApprovalRoles();

            ddlTrades.Items.Add(new ListItem("All", String.Empty));
            //--San   tradeTemplateInfoList.Sort();
            tradeTemplateInfoList.Sort((x, y) => x.TradeCode.CompareTo(y.TradeCode));  //--San

            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                ddlTrades.Items.Add(new ListItem(tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName, tradeTemplateInfo.TradeCode));

            ddlSubbies.Items.Add(new ListItem("All", String.Empty));
            foreach (SubContractorInfo subContractorInfo in subContractorInfoList)
                ddlSubbies.Items.Add(new ListItem(subContractorInfo.ShortName, subContractorInfo.IdStr));

            ddlPerson.Items.Add(new ListItem("All", String.Empty));
            foreach (EmployeeInfo employeeInfo in employeeInfoList)
                ddlPerson.Items.Add(new ListItem(employeeInfo.Name, employeeInfo.IdStr));

            BindProjectsList();
        }

        private void IncludeRecord(ContractInfo contractInfo, List<ContractInfo> contractInfoList, EmployeeInfo selectedEmployee, ProcessStepInfo lastStep, ProcessStepInfo currentStep, TradeInfo tradeInfo, ProcessController processController, ProjectsController projectsController)
        {
            EmployeeInfo roleAssignee = null;

            if (currentStep != null)
            {
                //#-- roleAssignee = processController.GetRoleAssignee(currentStep.Role, tradeInfo.Project);


                //#--- to get right "Person to Action" in Purchasing schedule report
                if (currentStep.Role == "CA" && tradeInfo.ContractsAdministrator != null)
                {
                    roleAssignee = tradeInfo.ContractsAdministrator;
                }
                else if (currentStep.Role == "PM" && tradeInfo.ProjectManager != null)
                {
                    roleAssignee = tradeInfo.ProjectManager;
                }
                else
                {
                   roleAssignee = processController.GetRoleAssignee(currentStep.Role, tradeInfo.Project);
                }
                //#--- to get right "Person to Action" in Purchasing schedule report

            }
            if (selectedEmployee == null || selectedEmployee.Equals(roleAssignee))
            {
                contractInfo.Trade = tradeInfo;

                if (lastStep != null)
                {
                    contractInfo.CurrentStatusName = lastStep.Name;
                    contractInfo.CurrentStatusDate = lastStep.ActualDate;
                }

                if (currentStep != null)
                {
                    contractInfo.PendingTaskName = currentStep.Name;
                    contractInfo.PendingTaskDueDate = currentStep.TargetDate;
                    contractInfo.PendingTaskDueDays = projectsController.NumBusinessDays(currentStep.TargetDate);

                    if (roleAssignee != null)
                        contractInfo.PendingTaskPersonName = roleAssignee.Name;
                }

                contractInfoList.Add(contractInfo);
            }
        }

        private ContractInfo UpdateRecord(ContractInfo contractInfo, TradeInfo tradeInfo, ProcessStepInfo lastStep, ProcessStepInfo currentStep, ProcessController processController)
        {

            return contractInfo;
        }

        public class reportContractInfo
        {
            // Auto-Initialized properties  
            public string ProjectName { get; set; }
            public string TradeName { get; set; }
            public string Code { get; set; }
            public string SubcontractorName { get; set; }
            public string SubcontractNumber { get; set; }
            public string CurrentStatusName { get; set; }
            public string CurrentStatusDate { get; set; }

            public string PendingTask { get; set; }
            public string PersonToAction { get; set; }
            public string DueDate { get; set; }
            public string DueDays { get; set; }
        }

        private void BindReport()
        {
            String filterInfo = null;
            ProcessStepInfo lastStep = null;
            ProcessStepInfo currentStep = null;
            EmployeeInfo selectedEmployee = null;
            Boolean includeSubContracts = chkSubcontracts.Checked;
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ContractInfo> contractInfoList = new List<ContractInfo>();
            List<TradeInfo> tradeInfoList = TradesController.GetInstance().SearchTrades(UI.Utils.GetFormString(ddlProjects.SelectedValue), UI.Utils.GetFormString(ddlTrades.SelectedValue), UI.Utils.GetFormInteger(ddlSubbies.SelectedValue));
            Dictionary<String, ProjectInfo> projectsDictionary = new Dictionary<String, ProjectInfo>();
            List<ReportParameter> reportParameterList = new List<ReportParameter>();

            projectsController.InitializeHolidays();

            if (ddlPerson.SelectedValue != String.Empty)
                selectedEmployee = (EmployeeInfo) PeopleController.GetInstance().GetPersonById(UI.Utils.GetFormInteger(ddlPerson.SelectedValue));

            foreach (TradeInfo tradeInfo in tradeInfoList)
            {
                if (!projectsDictionary.ContainsKey(tradeInfo.Project.IdStr))
                    projectsDictionary.Add(tradeInfo.Project.IdStr, projectsController.GetProject(tradeInfo.Project.Id));

                tradeInfo.Project = projectsDictionary[tradeInfo.Project.IdStr];
            }

            foreach (TradeInfo tradeInfo in tradeInfoList)
            {
                if (tradeInfo.Contract != null)
                {
                    lastStep = processController.GetLastStep(tradeInfo.Contract.Process);
                    if (lastStep == null)
                        lastStep = processController.GetLastStep(tradeInfo.Process);

                    currentStep = processController.GetCurrentStep(tradeInfo.Contract.Process);
                }
                else
                {
                    lastStep = processController.GetLastStep(tradeInfo.Process);
                    currentStep = processController.GetCurrentStep(tradeInfo.Process);
                }

                IncludeRecord(new ContractInfo(), contractInfoList, selectedEmployee, lastStep, currentStep, tradeInfo, processController, projectsController);
                               
                if (includeSubContracts)
                {
                    if (tradeInfo.Contract != null)
                    {
                        foreach (ContractInfo subContract in tradeInfo.Contract.Subcontracts)
                        {
                            lastStep = processController.GetLastStep(subContract.Process);
                            currentStep = processController.GetCurrentStep(subContract.Process);

                            IncludeRecord(subContract, contractInfoList, selectedEmployee, lastStep, currentStep, tradeInfo, processController, projectsController);
                        }
                    }
                }
            }

            if (ddlProjects.SelectedValue == String.Empty)
                filterInfo = "All projects.";
            else
                if (ddlProjects.SelectedValue == "Active")
                    filterInfo = "Active projects.";
                else
                    filterInfo = "Project: " + ddlProjects.SelectedItem.Text + ".";

            if (ddlTrades.SelectedValue != String.Empty)
                filterInfo = filterInfo + " " + "Trade: " + ddlTrades.SelectedItem.Text + ".";

            if (ddlSubbies.SelectedValue != String.Empty)
                filterInfo = filterInfo + " " + "Subcontractor: " + ddlSubbies.SelectedItem.Text + ".";

            if (ddlPerson.SelectedValue != String.Empty)
                filterInfo = filterInfo + " " + "Person to act: " + ddlPerson.SelectedItem.Text + ".";

            reportParameterList.Add(new ReportParameter("FilterInfo", filterInfo, false));

            repPurchaseSchedule.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\PurchasingSchedule.rdlc"; ;
            repPurchaseSchedule.LocalReport.SetParameters(reportParameterList);
            repPurchaseSchedule.LocalReport.DataSources.Clear();

            

      

        List<reportContractInfo> lstRptContract = new List<reportContractInfo>();

            //contractInfoList.ConvertAll(x => x.ToString());
            //reportList = contractInfoList.ConvertAll<string>();


            foreach(ContractInfo cInfo in contractInfoList)
            {
                reportContractInfo rpt = new reportContractInfo();
                rpt.ProjectName = cInfo.Trade.ProjectName;
                rpt.TradeName = cInfo.Trade.Name;
                rpt.SubcontractorName = cInfo.Trade.SelectedSubContractorName;
                rpt.SubcontractNumber = cInfo.SubcontractNumber.ToString();
                rpt.Code = cInfo.Trade.Code;
                rpt.CurrentStatusName = cInfo.CurrentStatusName;
                rpt.CurrentStatusDate = cInfo.CurrentStatusDate.ToString(); 
                rpt.PendingTask = cInfo.PendingTaskName;
                rpt.PersonToAction = cInfo.PendingTaskPersonName;
                rpt.DueDate = cInfo.PendingTaskDueDate.ToString();
                rpt.DueDays = cInfo.PendingTaskDueDays.ToString();

                lstRptContract.Add(rpt);

            }

            repPurchaseSchedule.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ContractInfo", lstRptContract));
            repPurchaseSchedule.DataBind();
            repPurchaseSchedule.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (!Page.IsPostBack)
                {
                    repPurchaseSchedule.Visible = false;
                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            BindProjectsList();
        }

        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                BindReport();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
             Response.Redirect("~/Modules/Core/ListReports.aspx");
        }
#endregion

    }
}
