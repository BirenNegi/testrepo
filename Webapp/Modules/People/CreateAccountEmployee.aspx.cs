using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class CreateAccountEmployeePage : SOSPage
    {

#region Members
        private EmployeeInfo employeeInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (employeeInfo == null)
                return null;

            tempNode.ParentNode.Title = employeeInfo.Name;
            tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?PeopleId=" + employeeInfo.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            lblFirstName.Text = UI.Utils.SetFormString(employeeInfo.FirstName);
            lblLastName.Text = UI.Utils.SetFormString(employeeInfo.LastName);
            txtEmail.Text = UI.Utils.SetFormString(employeeInfo.Email);
            txtSignature.Text = UI.Utils.SetFormString(employeeInfo.Signature);

            Utils.GetConfigList("Global", "EmployeeTypes", ddlAccessLevel, null);
        }

        private void FormToObjects()
        {
            employeeInfo.Email = UI.Utils.GetFormString(txtEmail.Text);
            employeeInfo.Login = UI.Utils.GetFormString(txtUserName.Text);
            employeeInfo.Signature = UI.Utils.GetFormString(txtSignature.Text);
            employeeInfo.UserType = UI.Utils.GetFormString(ddlAccessLevel.SelectedValue);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId = Request.Params["PeopleId"];
            try
            {
                Security.CheckAccess(Security.userActions.CreateEmployeeAccount);
                parameterPeopleId = Utils.CheckParameter("PeopleId");
                employeeInfo = (EmployeeInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                Core.Utils.CheckNullObject(employeeInfo, parameterPeopleId, "Employee");

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
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    employeeInfo.Password = Security.GeneratePassword();
                    PeopleController.GetInstance().UpdatePerson(employeeInfo);
                    Core.Utils.SendPassword(employeeInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/People/ViewEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/ViewEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
        }
#endregion

    }
}