using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class SelectClientContact : System.Web.UI.Page
    {
        #region Members
        private BusinessUnitInfo businessUnitInfo = null;
        #endregion

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
            foreach (BusinessUnitInfo businessUnit in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnit.Name, businessUnit.IdStr));

            if (businessUnitInfo != null)
                ddlBusinessUnit.SelectedValue = businessUnitInfo.IdStr;

            GvPeopleSortExpression = "FullName";
            GvPeopleSortDireccion = SortDirection.Ascending;

            lnkNoneSelected.NavigateUrl = Utils.PopupSendPeople(this, "", "");
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterBusinessUnitId = null;//Request.Params["BusinessUnitId"];

            try
            {
                Security.CheckAccess(Security.userActions.SelectContact);

                if (parameterBusinessUnitId != null)
                    businessUnitInfo = new BusinessUnitInfo(Int32.Parse(parameterBusinessUnitId));

                if (!Page.IsPostBack)
                    BindSearch();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void odsPeople_Selecting(object sender, ObjectDataSourceEventArgs e)
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
                GvPeopleSortDireccion = SortDirection.Ascending;
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