using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class SearchContactsPage : System.Web.UI.Page
    {

#region Public properties
        protected String GvPeopleSortExpression
        {
            get { return (String)ViewState["GvPeopleSortExpression"]; }
            set { ViewState["GvPeopleSortExpression"] = value; }
        }

        protected SortDirection GvPeopleSortDireccion
        {
            get { return (SortDirection)ViewState["GvPeopleSortDirection"]; }
            set { ViewState["GvPeopleSortDirection"] = value; }
        }
#endregion

#region Private Methods
        private void BindSearch()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            GvPeopleSortExpression = "FullName";
            GvPeopleSortDireccion = SortDirection.Ascending;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.SearchContacts);

                if (Security.ViewAccess(Security.userActions.EditContact))
                {
                    phAddNew.Visible = true;
                    gvPeople.Columns[8].Visible = true;
                    gvPeople.Columns[9].Visible = true;
                }

                if (!Page.IsPostBack)
                    BindSearch();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void odsContacts_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = PeopleController.GetInstance();
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            gvPeople.PageIndex = 0;
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPeople.PageIndex = 0;
        }

        protected void chkInactive_OnCheckedChanged(object sender, EventArgs e)
        {
            gvPeople.PageIndex = 0;
        }

        protected void gvPeople_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvPeople.PageIndex = 0;

            if (GvPeopleSortExpression == e.SortExpression)
                GvPeopleSortDireccion = Utils.ChangeSortDirection(GvPeopleSortDireccion);
            else
            {
                GvPeopleSortExpression = e.SortExpression;
                GvPeopleSortDireccion = e.SortExpression == "LastLogin" ? SortDirection.Descending : SortDirection.Ascending;
            }

            e.SortDirection = GvPeopleSortDireccion;
        }

        protected void gvPeople_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvPeople, e, GvPeopleSortExpression, GvPeopleSortDireccion);
        }
#endregion

    }
}
