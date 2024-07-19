using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class SearchParticipationsSubContractorPage : SOSPage
    {

#region Members
        private SubContractorInfo subContractorInfo = null;
#endregion


#region Public properties
        protected String GvParticipationsSortExpression
        {
            get { return (String)ViewState["GvParticipationsSortExpression"]; }
            set { ViewState["GvParticipationsSortExpression"] = value; }
        }

        protected SortDirection GvParticipationsSortDireccion
        {
            get { return (SortDirection)ViewState["GvParticipationsSortDirection"]; }
            set { ViewState["GvParticipationsSortDirection"] = value; }
        }
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            return currentNode;
        }

        private void BindSearch()
        {
            odsParticipations.SelectParameters["strSubContractorId"].DefaultValue = subContractorInfo.IdStr;

            ddlStatus.Items.Add(new ListItem("Active", ProjectInfo.StatusActive));
            ddlStatus.Items.Add(new ListItem("All", String.Empty));

            GvParticipationsSortExpression = "Name";
            GvParticipationsSortDireccion = SortDirection.Ascending;
        }

        protected String ClassActiveParticipation(Boolean hasActiveParticipation)
        {
            return hasActiveParticipation ? "lstTextGrayedOut" : String.Empty;
        }

        protected String MessageActiveParticipation(Boolean hasActiveParticipation)
        {
            return hasActiveParticipation ? "Not active" : String.Empty;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.SearchParticipationsSubContractor);
                subContractorInfo = ((ContactInfo)Web.Utils.GetCurrentUser()).SubContractor;

                if (Web.Utils.GetCurrentUser().UserType == "SE")
                    Response.Redirect("~/Modules/Core/Login.aspx") ;
                 
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

        protected void odsParticipations_Selecting(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = TradesController.GetInstance();
        }
        
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvParticipations.PageIndex = 0;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            gvParticipations.PageIndex = 0;
        }

        protected void gvParticipations_OnSorting(object sender, GridViewSortEventArgs e)
        {
            gvParticipations.PageIndex = 0;

            if (GvParticipationsSortExpression == e.SortExpression)
                GvParticipationsSortDireccion = Utils.ChangeSortDirection(GvParticipationsSortDireccion);
            else
            {
                GvParticipationsSortExpression = e.SortExpression;
                GvParticipationsSortDireccion = (e.SortExpression == "DueDate") ? SortDirection.Descending : SortDirection.Ascending;
            }

            e.SortDirection = GvParticipationsSortDireccion;
        }

        protected void gvParticipations_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Utils.SortedGridSetOrderImage(gvParticipations, e, GvParticipationsSortExpression, GvParticipationsSortDireccion);
        }
#endregion

    }
}