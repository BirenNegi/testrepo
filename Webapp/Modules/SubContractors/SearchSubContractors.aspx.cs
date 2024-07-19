using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class SearchSubContractorsPage : System.Web.UI.Page
    {

        #region Public properties
        protected String GvSubContractorsSortExpression
        {
            get { return (String)ViewState["GvSubContractorsSortExpression"]; }
            set { ViewState["GvSubContractorsSortExpression"] = value; }
        }

        protected SortDirection GvSubContractorsSortDireccion
        {
            get { return (SortDirection)ViewState["GvSubContractorsSortDirection"]; }
            set { ViewState["GvSubContractorsSortDirection"] = value; }
        }
        #endregion

        #region Private Methods
        private void BindSearch()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            GvSubContractorsSortExpression = "Name";
            GvSubContractorsSortDireccion = SortDirection.Ascending;
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.SearchSubContractors);
                phAddNew.Visible = (Security.ViewAccess(Security.userActions.EditSubContractor));

                if (!Page.IsPostBack)
                {
                    BindSearch();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void odsSubContractors_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = SubContractorsController.GetInstance();
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSubContractors.PageIndex = 0;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            gvSubContractors.PageIndex = 0;
        }

        protected void gvSubContractors_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvSubContractors.PageIndex = 0;

            if (GvSubContractorsSortExpression == e.SortExpression)
                GvSubContractorsSortDireccion = Utils.ChangeSortDirection(GvSubContractorsSortDireccion);
            else
            {
                GvSubContractorsSortExpression = e.SortExpression;
                GvSubContractorsSortDireccion = SortDirection.Ascending;
            }

            e.SortDirection = GvSubContractorsSortDireccion;
        }

        protected void gvSubContractors_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvSubContractors, e, GvSubContractorsSortExpression, GvSubContractorsSortDireccion);
        }
        #endregion

    }
}
