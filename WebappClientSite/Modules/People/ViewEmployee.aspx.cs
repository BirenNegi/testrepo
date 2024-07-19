using System;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class ViewEmployeePage : SOSPage
    {

#region Members
        private EmployeeInfo employeeInfo = null;
        private string projectId ;
        #endregion

        #region Private Methods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectId == String.Empty)
                return null;

           // tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectId;

            return currentNode;
        }

        private void bindEmployee()
        {
            lblFirstName.Text = UI.Utils.SetFormString(employeeInfo.FirstName);
            lblLastName.Text = UI.Utils.SetFormString(employeeInfo.LastName);
            //lblStreet.Text = UI.Utils.SetFormString(employeeInfo.Street);
            //lblLocality.Text = UI.Utils.SetFormString(employeeInfo.Locality);
            //lblState.Text = UI.Utils.SetFormString(employeeInfo.State);
            //lblPostalCode.Text = UI.Utils.SetFormString(employeeInfo.PostalCode);
            lblPhone.Text = UI.Utils.SetFormString(employeeInfo.Phone);
            lblFax.Text = UI.Utils.SetFormString(employeeInfo.Fax);
            lblMobile.Text = UI.Utils.SetFormString(employeeInfo.Mobile);
            //lblBusinessUnit.Text = UI.Utils.SetFormString(employeeInfo.BusinessUnitName);
            //booInactive.Checked = employeeInfo.Inactive;

            lnkEmail.Text = UI.Utils.SetFormString(employeeInfo.Email);
            lnkEmail.NavigateUrl = "mailto:" + UI.Utils.SetFormString(employeeInfo.Email);
            
            //lblPosition.Text = UI.Utils.SetFormString(employeeInfo.Position);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId;
            String signatureImage;

            try
            {
                Security.CheckAccess(Security.userActions.ViewEmployee);
                parameterPeopleId = Utils.CheckParameter("PeopleId");
                projectId = Utils.CheckParameter("ProjectId");
                employeeInfo = (EmployeeInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                Core.Utils.CheckNullObject(employeeInfo, parameterPeopleId, "Employee");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditEmployee))
                    {
                        cmdEditTop.Visible = true;
                        cmdDeleteTop.Visible = true;

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Employee?');");

                        if (Security.ViewAccess(Security.userActions.CreateEmployeeAccount))
                        {
                            phAccount.Visible = true;

                            if (employeeInfo.HasAccount)
                            {
                                phAccountInfo.Visible = true;
                                lblUserName.Text = UI.Utils.SetFormString(employeeInfo.Login);
                                lblLastLogin.Text = UI.Utils.SetFormDate(employeeInfo.LastLoginDate);
                                lblAccessLevel.Text = Web.Utils.GetConfigListItemName("Global", "EmployeeTypes", employeeInfo.UserType);
                                if (employeeInfo.Signature != null)
                                {
                                    signatureImage = "~/Images/Signatures/" + employeeInfo.Signature;
                                    if (System.IO.File.Exists(Server.MapPath(signatureImage)))
                                    {
                                        imgSignature.ImageUrl = signatureImage;
                                        imgSignature.Visible = true;
                                    }
                                    else
                                    {
                                        lblSignature.Text = "File '" + employeeInfo.Signature + "' not found.";
                                        lblSignature.Visible = true;
                                    }
                                }
                                else
                                {
                                    lblSignature.Text = "No Signature.";
                                    lblSignature.Visible = true;
                                }
                                cmdDeleteAccount.Attributes.Add("onClick", "javascript:return confirm('Delete Employee Account ?');");
                            }
                            else
                            {
                                phNoAccountInfo.Visible = true;
                            }
                        }
                    }
                    bindEmployee();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/EditEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                PeopleController.GetInstance().DeletePerson(employeeInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/People/SearchEmployees.aspx");
        }

        protected void cmdDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                employeeInfo.Login = null;
                employeeInfo.Password = null;
                employeeInfo.LastLoginDate = null;
                employeeInfo.Signature = null;
                PeopleController.GetInstance().UpdatePerson(employeeInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/People/ViewEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
        }

        protected void cmdCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/CreateAccountEmployee.aspx?PeopleId=" + employeeInfo.IdStr);
        }
#endregion

    }
}