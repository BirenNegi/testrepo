using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class SearchProjectsPage : System.Web.UI.Page
    {

#region Public properties
        protected String GvProjectsSortExpression
        {
            get { return (String)ViewState["GvProjectsSortExpression"]; }
            set { ViewState["GvProjectsSortExpression"] = value; }
        }

        protected SortDirection GvProjectsSortDireccion
        {
            get { return (SortDirection)ViewState["GvProjectsSortDirection"]; }
            set { ViewState["GvProjectsSortDirection"] = value; }
        }
#endregion

#region Private Methods
        private void BindSearch()
        {
            EmployeeInfo employeeInfo = (EmployeeInfo)Web.Utils.GetCurrentUser();

            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            Utils.GetConfigListAddAll("Global", "ProjectStatus", ddlStatus, null);
            ddlStatus.SelectedValue = ProjectInfo.StatusActive;

            if (employeeInfo.BusinessUnit != null)
                ddlBusinessUnit.SelectedValue = employeeInfo.BusinessUnit.IdStr;

            GvProjectsSortExpression = "Name";
            GvProjectsSortDireccion = SortDirection.Ascending;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.SearchProjects);
                phAddNew.Visible = Security.ViewAccess(Security.userActions.CreateProject);

                if (!Page.IsPostBack)
                    BindSearch();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void odsProjects_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = ProjectsController.GetInstance();
        } 

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvProjects.PageIndex = 0;
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvProjects.PageIndex = 0;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            gvProjects.PageIndex = 0;
        }

        protected void gvProjects_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvProjects.PageIndex = 0;

            if (GvProjectsSortExpression == e.SortExpression)
                GvProjectsSortDireccion = Utils.ChangeSortDirection(GvProjectsSortDireccion);
            else
            {
                GvProjectsSortExpression = e.SortExpression;
                GvProjectsSortDireccion = (e.SortExpression == "CommencementDate" || e.SortExpression == "CompletionDate") ? SortDirection.Descending : SortDirection.Ascending;
            }

            e.SortDirection = GvProjectsSortDireccion;
        }

        protected void gvProjects_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvProjects, e, GvProjectsSortExpression, GvProjectsSortDireccion);
        }
#endregion

    }
}