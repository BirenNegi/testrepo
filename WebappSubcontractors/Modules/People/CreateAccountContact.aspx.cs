using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class CreateAccountContactPage : SOSPage
    {

#region Members
        private ContactInfo contactInfo = null;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (contactInfo == null)
                return null;

            tempNode.ParentNode.Title = contactInfo.Name;
            tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?PeopleId=" + contactInfo.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            lblFirstName.Text = UI.Utils.SetFormString(contactInfo.FirstName);
            lblLastName.Text = UI.Utils.SetFormString(contactInfo.LastName);
            txtEmail.Text = UI.Utils.SetFormString(contactInfo.Email);

            Utils.GetConfigList("Global", "ContactTypes", ddlAccessLevel, null);
        }

        private void FormToObjects()
        {
            contactInfo.Email = UI.Utils.GetFormString(txtEmail.Text);
            contactInfo.Login = UI.Utils.GetFormString(txtUserName.Text);
            contactInfo.UserType = UI.Utils.GetFormString(ddlAccessLevel.SelectedValue);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId = Request.Params["PeopleId"];
            try
            {
                Security.CheckAccess(Security.userActions.CreateContactAccount);
                parameterPeopleId = Utils.CheckParameter("PeopleId");
                contactInfo = (ContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);

                if (contactInfo.SubContractor.Id != ((ContactInfo)Utils.GetCurrentUser()).SubContractor.Id)
                    Response.Redirect("~/Modules/Core/Login.aspx");

                Core.Utils.CheckNullObject(contactInfo, parameterPeopleId, "Contact");

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
                    contactInfo.Password = Security.GeneratePassword();
                    PeopleController.GetInstance().UpdatePerson(contactInfo);
                    Core.Utils.SendPassword(contactInfo);
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
            Response.Redirect("~/Modules/People/ViewContact.aspx?PeopleId=" + contactInfo.IdStr);
        }
#endregion

    }
}