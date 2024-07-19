using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewContactPage : System.Web.UI.Page
    {

#region Members
        private ContactInfo contactInfo = null;
#endregion

#region Private Methods
        private void bindContact()
        {
            lblFirstName.Text = UI.Utils.SetFormString(contactInfo.FirstName);
            lblLastName.Text = UI.Utils.SetFormString(contactInfo.LastName);
            lblStreet.Text = UI.Utils.SetFormString(contactInfo.Street);
            lblLocality.Text = UI.Utils.SetFormString(contactInfo.Locality);
            lblState.Text = UI.Utils.SetFormString(contactInfo.State);
            lblPostalCode.Text = UI.Utils.SetFormString(contactInfo.PostalCode);
            lblPhone.Text = UI.Utils.SetFormString(contactInfo.Phone);
            lblFax.Text = UI.Utils.SetFormString(contactInfo.Fax);
            lblMobile.Text = UI.Utils.SetFormString(contactInfo.Mobile);

            lnkEmail.Text = UI.Utils.SetFormString(contactInfo.Email);
            lnkEmail.NavigateUrl = "mailto:" + UI.Utils.SetFormString(contactInfo.Email);
            //#---
            lblPosition.Text = UI.Utils.SetFormString(contactInfo.Position);

            LblDOB.Text = UI.Utils.SetFormDate(contactInfo.DOB).ToString();
            LblAlergies.Text = UI.Utils.SetFormString(contactInfo.Allergies);
            lblEmergencyContactName.Text = UI.Utils.SetFormString(contactInfo.EmergencyContactName);
            LblEmergencyContactNumber.Text = UI.Utils.SetFormString(contactInfo.EmergenctContactNumber);

            BindQualifications();
            //#--
            booInactive.Checked = contactInfo.Inactive;

            if (contactInfo.SubContractor != null)
            {
                lnkCompany.Text = contactInfo.SubContractor.Name;
                lnkCompany.NavigateUrl = "~/Modules/SubContractors/ViewSubContractor.aspx?SubContractorId=" + contactInfo.SubContractor.IdStr;
            }
        }
         //#--
        private void BindQualifications()
        {
            if (contactInfo.Qualifications != null)
            {
                gvQualifications.DataSource = contactInfo.Qualifications;
                gvQualifications.DataBind();

            }

        }
        //#--

        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterPeopleId = Utils.CheckParameter("PeopleId");
                contactInfo = (ContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                //#--04/02/20 


                if (contactInfo.SubContractor.Id != ((ContactInfo)Utils.GetCurrentUser()).SubContractor.Id)
                    Response.Redirect("~/Modules/Core/Login.aspx");
                else { 
                contactInfo.Qualifications = PeopleController.GetInstance().GetQualificationsByContactId(Int32.Parse(parameterPeopleId));
                //#--04/02/20
                Core.Utils.CheckNullObject(contactInfo, parameterPeopleId, "Contact");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditContact))
                    {
                        cmdEditTop.Visible = true;
                       // cmdDeleteTop.Visible = true;//#---

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Contact?');");

                        if (Security.ViewAccess(Security.userActions.CreateContactAccount))
                        {
                            phAccount.Visible = true;

                            if (contactInfo.HasAccount)
                            {
                                phAccountInfo.Visible = true;
                                lblUserName.Text = UI.Utils.SetFormString(contactInfo.Login);
                                lblLastLogin.Text = UI.Utils.SetFormDate(contactInfo.LastLoginDate);
                                lblAccessLevel.Text = Web.Utils.GetConfigListItemName("Global", "ContactTypes", contactInfo.UserType);
                                cmdDeleteAccount.Attributes.Add("onClick", "javascript:return confirm('Delete Contact Account ?');");
                            }
                            else
                            {
                                phNoAccountInfo.Visible = true;
                            }
                        }
                    }
                    bindContact();
                }
            }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/EditContact.aspx?PeopleId=" + contactInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    PeopleController.GetInstance().DeletePerson(contactInfo);
            //}
            //catch (Exception Ex)
            //{
            //    Utils.ProcessPageLoadException(this, Ex);
            //}

            //Response.Redirect("~/Modules/People/SearchContacts.aspx");
        }

        protected void cmdDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                contactInfo.Login = null;
                contactInfo.Password = null;
                contactInfo.LastLoginDate = null;
                PeopleController.GetInstance().UpdatePerson(contactInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/People/ViewContact.aspx?PeopleId=" + contactInfo.IdStr);
        }

        protected void cmdCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/CreateAccountContact.aspx?PeopleId=" + contactInfo.IdStr);
        }

        //#--- 15/01/2020
        protected void cmdAddOrEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/EditSubcontractorQualifications.aspx?PeopleId=" + contactInfo.IdStr);
        }

        //#---15/01/2020


        #endregion

    }
}