using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class SelectSubContractorPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindSearch(String parameterBusinessUnitId)
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            if (parameterBusinessUnitId != null)
                ddlBusinessUnit.SelectedValue = parameterBusinessUnitId;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterBusinessUnitId;
            String parameterProjectId;
            try
            {
                Security.CheckAccess(Security.userActions.SelectSubContractor);
                lnkNoneSelected.NavigateUrl = Utils.PopupSendCompany(this, "", "");
                parameterBusinessUnitId = Request.Params["BusinessUnitId"];
                parameterProjectId = Request.Params["ProjectId"];
                //strProject.Text = parameterProjectId;
               // strProject.Visible = false;
                if (!Page.IsPostBack)
                {
                    BindSearch(parameterBusinessUnitId);
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
            GridViewSubContractors.PageIndex = 0;
        }

        protected void cmdSearch_Click(object sender, EventArgs e)
        {
            GridViewSubContractors.PageIndex = 0;
        }
#endregion

    }
}
