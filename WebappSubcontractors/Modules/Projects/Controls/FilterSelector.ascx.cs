using System;
using System.Xml;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;
using SOS.Reports;

namespace SOS.Web
{
    public partial class FilterSelectorControl : System.Web.UI.UserControl
    {

#region Members
        private Boolean showDates = false;
        private Boolean showOptions = false;
        private Boolean showEmployees = false;
        private Boolean showTradesRange = false;
        private System.Web.UI.WebControls.WebControl actionControl = null;
#endregion

#region Public properties
        public Boolean ShowDates
        {
            set { showDates = value; }
        }

        public Boolean ShowOptions
        {
            set { showOptions = value; }
        }

        public Boolean ShowEmployees
        {
            set { showEmployees = value; }
        }

        public Boolean ShowTradesRange
        {
            set { showTradesRange = value; }
        }

        public System.Web.UI.WebControls.WebControl ActionControl
        {
            set
            {
                actionControl = value;
            }
        }

        public List<ProjectInfo> Projects
        {
            get
            {
                ProjectInfo project = Project;
                return project != null ? new List<ProjectInfo> { project } : ProjectsController.GetInstance().ListProjects(CombinedStatus, BusinessUnit);
            }
        }

        public List<EmployeeInfo> Employees
        {
            get
            {
                EmployeeInfo employee = Employee;
                return employee != null ? new List<EmployeeInfo> { employee } : PeopleController.GetInstance().ListEmployees(SelectedEmployeeStatus, Project, CombinedStatus, BusinessUnit);
            }
        }

        public DateTime? StartDate
        {
            get
            {
                if (showDates)
                {
                    DateTime? tmpDateTime = sdrStartDate.Date;

                    if (tmpDateTime != null)
                    {
                        return new DateTime(tmpDateTime.Value.Year, tmpDateTime.Value.Month, tmpDateTime.Value.Day, 0, 0, 0);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public DateTime? EndDate
        {
            get
            {
                if (showDates)
                {
                    DateTime? tmpDateTime = sdrEndDate.Date;

                    if (tmpDateTime != null)
                    {
                        return new DateTime(tmpDateTime.Value.Year, tmpDateTime.Value.Month, tmpDateTime.Value.Day, 23, 59, 59);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public String StartTrade
        {
            get
            {
                if (showTradesRange)
                {
                    return ddlTradesStart.SelectedValue != String.Empty ? ddlTradesStart.SelectedValue : null;
                }
                else
                {
                    return null;
                }
            }
        }

        public String EndTrade
        {
            get
            {
                if (showTradesRange)
                {
                    return ddlTradesEnd.SelectedValue != String.Empty ? ddlTradesEnd.SelectedValue : null;
                }
                else
                {
                    return null;
                }
            }
        }

        public String FilterInfo
        {
            get
            {
                String filterInfo;

                BusinessUnitInfo businessUnit = BusinessUnit;
                ProjectInfo project = Project;
                EmployeeInfo employee = Employee;

                filterInfo = "BU: " + (businessUnit != null ? businessUnit.Name : "All") + ". ";
                filterInfo = filterInfo + "Project: " + (CombinedStatus == ProjectInfo.combinedStatus.One ? project.Name : ddlProjects.SelectedItem.Text) + ". ";

                if (showEmployees)
                {
                    filterInfo = filterInfo + "Employee: " + (employee != null ? employee.Name : ddlEmployee.SelectedItem.Text) + ". ";
                }

                if (showTradesRange)
                {
                    if (StartTrade != null)
                    {
                        if (EndTrade != null)
                        {
                            if (StartTrade == EndTrade)
                            {
                                filterInfo = filterInfo + "Trade: " + StartTrade + ". ";
                            }
                            else
                            {
                                filterInfo = filterInfo + "Trades from: " + StartTrade + " to: " + EndTrade + ". ";
                            }
                        }
                        else
                        {
                            filterInfo = filterInfo + "Trades greater than or equal to: " + StartTrade + ". ";
                        }
                    }
                    else
                    {
                        if (EndTrade != null)
                        {
                            filterInfo = filterInfo + "Trades lower than or equal: " + EndTrade + ". ";
                        }
                        else
                        {
                            filterInfo = filterInfo + "All trades. ";
                        }
                    }
                }

                if (showDates)
                {
                    if (StartDate != null)
                    {
                        if (EndDate != null)
                        {
                            filterInfo = filterInfo + "From: " + UI.Utils.SetFormDate(StartDate) + " to: " + UI.Utils.SetFormDate(EndDate) + ". ";
                        }
                        else
                        {
                            filterInfo = filterInfo + "From: " + UI.Utils.SetFormDate(StartDate) + ". ";
                        }
                    }
                    else
                    {
                        if (EndDate != null)
                        {
                            filterInfo = filterInfo + "To: " + UI.Utils.SetFormDate(EndDate) + ". ";
                        }
                    }
                }

                if (showOptions)
                {
                    if (!OnlyNextStep)
                    {
                        filterInfo = filterInfo + "All tasks. ";
                    }
                }

                return filterInfo.TrimEnd();
            }
        }

        public Boolean OnlyNextStep
        {
            get
            {
                if (showOptions)
                {
                    return rbuOptions.SelectedValue == "Pending";
                }
                else
                {
                    return true;
                }
            }
        }
#endregion

#region private Methods
        private void CheckIsValid()
        {
            Boolean isValidDate = StartDate != null && EndDate != null ? EndDate >= StartDate : true;
            Boolean isValidTrade = StartTrade != null && EndTrade != null ? EndTrade.CompareTo(StartTrade) >= 0 : true;

            if (showDates)
                pnlErrorDate.Visible = !isValidDate;

            if (showTradesRange)
                pnlErrorTradesRange.Visible = !isValidTrade;

            if (actionControl != null)
                actionControl.Enabled = isValidDate && isValidTrade;
        }

        public void ReBindProjectsList()
        {
            for (int i = 0; i <= 4; i++)
                ddlProjects.Items[i].Attributes.Add("Style", "color:#0000FF");
        }

        public void ReBindEmpoyeeList()
        {
            for (int i = 0; i <= 10; i++)
                ddlEmployee.Items[i].Attributes.Add("Style", "color:#0000FF");
        }
#endregion

#region Public Methods
        public void ReBindLists()
        {
            ReBindProjectsList();

            if (showEmployees)
                ReBindEmpoyeeList();
        }
#endregion

#region Private Properties
        public Boolean IsValid
        {
            get
            {
                return StartDate != null && EndDate != null ? EndDate >= StartDate : true;
            }
        }

        private ProjectInfo.combinedStatus CombinedStatus
        {
            get
            {
                String projectId = ddlProjects.SelectedValue;

                switch (projectId)
                {
                    case "A": return ProjectInfo.combinedStatus.All; ;
                    case "B": return ProjectInfo.combinedStatus.Active;
                    case "C": return ProjectInfo.combinedStatus.Complete;
                    case "D": return ProjectInfo.combinedStatus.CompleteWithTasks;
                    case "E": return ProjectInfo.combinedStatus.ActiveAndCompleteWithTasks;
                    default: return ProjectInfo.combinedStatus.One;
                }
            }
        }

        private String SelectedEmployeeStatus
        {
            get
            {
                String employeeId = ddlEmployee.SelectedValue;

                switch (employeeId)
                {
                    case "A":
                        return null;

                    case EmployeeInfo.TypeBudgetAdministrator:
                    case EmployeeInfo.TypeConstructionsManager:
                    case EmployeeInfo.TypeContractsAdministrator:
                    case EmployeeInfo.TypeDesignCoordinator:
                    case EmployeeInfo.TypeDesignManager:
                    case EmployeeInfo.TypeDirectorAuthorizacion:
                    case EmployeeInfo.TypeFinancialController:
                    case EmployeeInfo.TypeManagingDirector:
                    case EmployeeInfo.TypeProjectManager:
                    case EmployeeInfo.TypeUnitManager:
                        return employeeId;

                    default: return "One";
                }
            }
        }

        private BusinessUnitInfo BusinessUnit
        {
            get
            {
                BusinessUnitInfo businessUnit = null;
                String businessUnitId = ddlBusinessUnit.SelectedValue;

                if (businessUnitId != String.Empty)
                {
                    businessUnit = new BusinessUnitInfo((int?)Int32.Parse(businessUnitId));
                    businessUnit.Name = ddlBusinessUnit.SelectedItem.Text;
                }

                return businessUnit;
            }
        }

        private ProjectInfo Project
        {
            get
            {
                ProjectInfo project = null;

                if (CombinedStatus == ProjectInfo.combinedStatus.One)
                {
                    project = ProjectsController.GetInstance().GetProject((int?)Int32.Parse(ddlProjects.SelectedValue));
                    project.Name = ddlProjects.SelectedItem.Text;
                }

                return project;
            }
        }

        private EmployeeInfo Employee
        {
            get
            {
                EmployeeInfo employee = null;

                if (SelectedEmployeeStatus == "One")
                {
                    employee = (EmployeeInfo)PeopleController.GetInstance().GetPersonById((int?)Int32.Parse(ddlEmployee.SelectedValue));
                }

                return employee;
            }
        }
#endregion

#region Private Methods
        private void BindForm()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));

            if (businessUnitInfoList != null)
                foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                    ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            if (showTradesRange)
            {
                List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();

                tradeTemplateInfoList.Sort((x, y) => x.TradeCode.CompareTo(y.TradeCode));

                ddlTradesStart.Items.Add(new ListItem(String.Empty, String.Empty));
                ddlTradesEnd.Items.Add(new ListItem(String.Empty, String.Empty));

                foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                {
                    ddlTradesStart.Items.Add(new ListItem(tradeTemplateInfo.TradeCode, tradeTemplateInfo.TradeCode));
                    ddlTradesEnd.Items.Add(new ListItem(tradeTemplateInfo.TradeCode, tradeTemplateInfo.TradeCode));
                }
            }

            pnlDates.Visible = showDates;
            pnlOptions.Visible = showOptions;
            pnlEmployees.Visible = showEmployees;
            pnlTradesRange.Visible = showTradesRange;

            BindProjectsList();
        }

        private void BindProjectsList()
        {
            if (CombinedStatus != ProjectInfo.combinedStatus.One)
            {
                while (ddlProjects.Items.Count > 5)
                    ddlProjects.Items.RemoveAt(5);

                List<ProjectInfo> projects = Projects;

                if (projects != null)
                    foreach (ProjectInfo project in projects)
                        ddlProjects.Items.Add(new ListItem(project.Name, project.IdStr));
            }

            if (showEmployees)
                BindEmployeesList();

            ReBindProjectsList();
        }

        private void BindEmployeesList()
        {
            if (Employee == null)
            {
                while (ddlEmployee.Items.Count > 11)
                    ddlEmployee.Items.RemoveAt(11);

                List<EmployeeInfo> employees = Employees;

                if (employees != null)
                    foreach (EmployeeInfo employee in employees)
                        ddlEmployee.Items.Add(new ListItem(employee.Name, employee.IdStr));
            }

            ReBindEmpoyeeList();
        }

        private void wireEvents()
        {
            if (showDates)
            {
                sdrStartDate.dateChanged += new EventHandler(sdrStartDate_DateChanged);
                sdrEndDate.dateChanged += new EventHandler(sdrEndDate_DateChanged);
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            wireEvents();

            if (!Page.IsPostBack)
                BindForm();
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProjects.SelectedValue = "B";
            BindProjectsList();
        }

        protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProjectsList();
        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEmployeesList();
        }

        protected void sdrStartDate_DateChanged(object sender, EventArgs e)
        {
            CheckIsValid();
            ReBindLists();
        }

        protected void sdrEndDate_DateChanged(object sender, EventArgs e)
        {
            CheckIsValid();
            ReBindLists();
        }

        protected void ddlTradesStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckIsValid();
            ReBindLists();
        }

        protected void ddlTradesEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckIsValid();
            ReBindLists();
        }
#endregion

    }
}
