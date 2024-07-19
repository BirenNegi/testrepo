using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditContactPage : System.Web.UI.Page
    {

#region Members
        private ContactInfo contactInfo = null;
#endregion
        
#region Private Methods
        private void ObjectsToForm()
        {
            if (contactInfo.Id == null)
            {
                TitleBar.Title = "Adding Contact";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Contact";
            }

            txtEmail.Text = UI.Utils.SetFormString(contactInfo.Email);
            txtFax.Text = UI.Utils.SetFormString(contactInfo.Fax);
            txtFirstName.Text = UI.Utils.SetFormString(contactInfo.FirstName);
            txtLastName.Text = UI.Utils.SetFormString(contactInfo.LastName);
            txtLocality.Text = UI.Utils.SetFormString(contactInfo.Locality);
            txtMobile.Text = UI.Utils.SetFormString(contactInfo.Mobile);
            txtPhone.Text = UI.Utils.SetFormString(contactInfo.Phone);
            txtPostalCode.Text = UI.Utils.SetFormString(contactInfo.PostalCode);
            txtStreet.Text = UI.Utils.SetFormString(contactInfo.Street);
            //#---
            txtPosition.Text= UI.Utils.SetFormString(contactInfo.Position);//#--

            //#--13/01/2020
             TxtEmergencyName.Text = UI.Utils.SetFormString(contactInfo.EmergencyContactName);
             TxtEmergencyNumber.Text= UI.Utils.SetFormString(contactInfo.EmergenctContactNumber);
             TxtAllergies.Text= UI.Utils.SetFormString(contactInfo.Allergies);
             sdrDOB.Date= contactInfo.DOB;
            //#--13/01/2020


            chkInactive.Checked = contactInfo.IsInactive;

            if (contactInfo.SubContractor != null)
            {
                txtCompanyId.Value = contactInfo.SubContractor.IdStr;
                txtCompanyName.Text = contactInfo.SubContractor.Name;
            }
            //#---
            else {

                txtCompanyId.Value = ((ContactInfo)Utils.GetCurrentUser()).SubContractor.IdStr;
                txtCompanyName.Text = ((ContactInfo)Utils.GetCurrentUser()).SubContractor.Name;
            }
             //#---
            Utils.GetConfigListAddEmpty("Global", "States", ddlState, contactInfo.State);

            //#--- cmdSelCompany.NavigateUrl = Utils.PopupCompany(this, txtCompanyId.ClientID, txtCompanyName.ClientID, null);
        }

        private void FormToObjects()
        {
            contactInfo.Email = UI.Utils.GetFormString(txtEmail.Text);
            contactInfo.Fax = UI.Utils.GetFormString(txtFax.Text);
            contactInfo.FirstName = UI.Utils.GetFormString(txtFirstName.Text);
            contactInfo.LastName = UI.Utils.GetFormString(txtLastName.Text);
            contactInfo.Locality = UI.Utils.GetFormString(txtLocality.Text);
            contactInfo.Mobile = UI.Utils.GetFormString(txtMobile.Text);
            contactInfo.Phone = UI.Utils.GetFormString(txtPhone.Text);
            contactInfo.PostalCode = UI.Utils.GetFormString(txtPostalCode.Text);
            contactInfo.Street = UI.Utils.GetFormString(txtStreet.Text);
            contactInfo.State = UI.Utils.GetFormString(ddlState.SelectedValue);
            //#--
              contactInfo.Position= UI.Utils.GetFormString(txtPosition.Text);
             
            //#--
            contactInfo.Inactive = chkInactive.Checked;

            contactInfo.SubContractor = new SubContractorInfo(((ContactInfo)Utils.GetCurrentUser()).SubContractor.Id);


            //#--13/01/2020
            contactInfo.DOB=sdrDOB.Date;
            contactInfo.Allergies = UI.Utils.GetFormString(TxtAllergies.Text);
            contactInfo.EmergencyContactName=  UI.Utils.GetFormString(TxtEmergencyName.Text);
            contactInfo.EmergenctContactNumber = UI.Utils.GetFormString(TxtEmergencyNumber.Text);
           //#--13/01/2020
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId = Request.Params["PeopleId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditContact);

                if (parameterPeopleId == null)
                {
                    contactInfo = new ContactInfo();
                }
                else
                {
                    contactInfo = (ContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                    if (contactInfo.SubContractor.Id != ((ContactInfo)Utils.GetCurrentUser()).SubContractor.Id)
                        Response.Redirect("~/Modules/Core/Login.aspx");
                    Core.Utils.CheckNullObject(contactInfo, parameterPeopleId, "Contact");
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
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    contactInfo.Id = PeopleController.GetInstance().AddUpdatePerson(contactInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/People/ViewContact.aspx?PeopleId=" + contactInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (contactInfo.Id == null)
                Response.Redirect("~/Modules/People/SearchContacts.aspx");
            else
                Response.Redirect("~/Modules/People/ViewContact.aspx?PeopleId=" + contactInfo.IdStr);
        }
       
        //#--- 15/01/2020
        protected void cmdAddOrEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/SubContractors/EditSubcontractorQualifications.aspx?PeopleId=" + contactInfo.IdStr);
        }

        //#---15/01/2020

        #endregion
    }
}