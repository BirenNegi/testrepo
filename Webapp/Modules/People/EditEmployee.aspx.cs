using System;

using SOS.Core;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SOS.Web
{
    public partial class EditEmployeePage : System.Web.UI.Page
    {

#region Members
        private EmployeeInfo employeeInfo = null;
#endregion
        
#region Private Methods
        private void ObjectsToForm()
        {
            if (employeeInfo.Id == null)
            {
                TitleBar.Title = "Adding Staff Member";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Staff Member";
            }

            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem("All", String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            txtEmail.Text = UI.Utils.SetFormString(employeeInfo.Email);
            txtFax.Text = UI.Utils.SetFormString(employeeInfo.Fax);
            txtFirstName.Text = UI.Utils.SetFormString(employeeInfo.FirstName);
            txtLastName.Text = UI.Utils.SetFormString(employeeInfo.LastName);
            txtLocality.Text = UI.Utils.SetFormString(employeeInfo.Locality);
            txtMobile.Text = UI.Utils.SetFormString(employeeInfo.Mobile);
            txtPhone.Text = UI.Utils.SetFormString(employeeInfo.Phone);
            txtPostalCode.Text = UI.Utils.SetFormString(employeeInfo.PostalCode);
            txtStreet.Text = UI.Utils.SetFormString(employeeInfo.Street);
            txtPosition.Text = UI.Utils.SetFormString(employeeInfo.Position);
            chkInactive.Checked = employeeInfo.IsInactive;

            if (employeeInfo.BusinessUnit != null)
                ddlBusinessUnit.SelectedValue = employeeInfo.BusinessUnit.IdStr;
            
            Utils.GetConfigListAddEmpty("Global", "States", ddlState, employeeInfo.State);
        }

        private void FormToObjects()
        {
            employeeInfo.Email = UI.Utils.GetFormString(txtEmail.Text);
            employeeInfo.Fax = UI.Utils.GetFormString(txtFax.Text);
            employeeInfo.FirstName = UI.Utils.GetFormString(txtFirstName.Text);
            employeeInfo.LastName = UI.Utils.GetFormString(txtLastName.Text);
            employeeInfo.Locality = UI.Utils.GetFormString(txtLocality.Text);
            employeeInfo.Mobile = UI.Utils.GetFormString(txtMobile.Text);
            employeeInfo.Phone = UI.Utils.GetFormString(txtPhone.Text);
            employeeInfo.PostalCode = UI.Utils.GetFormString(txtPostalCode.Text);
            employeeInfo.Street = UI.Utils.GetFormString(txtStreet.Text);
            employeeInfo.Position = UI.Utils.GetFormString(txtPosition.Text);
            employeeInfo.Inactive = chkInactive.Checked;

            employeeInfo.BusinessUnit = ddlBusinessUnit.SelectedValue != "" ? new BusinessUnitInfo(Convert.ToInt32(ddlBusinessUnit.SelectedValue)) : null;

            employeeInfo.State = UI.Utils.GetFormString(ddlState.SelectedValue);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId = Request.Params["PeopleId"];

            try {
                Security.CheckAccess(Security.userActions.EditEmployee);

                if (parameterPeopleId == null)
                {
                    employeeInfo = new EmployeeInfo();
                }
                else
                {
                    employeeInfo = (EmployeeInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                    Core.Utils.CheckNullObject(employeeInfo, parameterPeopleId, "Employee");
                }
                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try {
                if (Page.IsValid)
                {
                    FormToObjects();
                    employeeInfo.Id = PeopleController.GetInstance().AddUpdatePerson(employeeInfo);
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/People/ViewEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (employeeInfo.Id == null)
            {
                Response.Redirect("~/Modules/People/SearchEmployees.aspx");
            }
            else
            {
                Response.Redirect("~/Modules/People/ViewEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
            }
        }
#endregion

    }
}