using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class ChangePasswordPage : SOSPage
    {

#region Members
        private PeopleInfo peopleInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            lblFirstName.Text = UI.Utils.SetFormString(peopleInfo.FirstName);
            lblLastName.Text = UI.Utils.SetFormString(peopleInfo.LastName);
            lblEmail.Text = UI.Utils.SetFormString(peopleInfo.Email);
        }

        private void FormToObjects()
        {
            peopleInfo.Password = txtNewPassword.Text;
        }
#endregion

#region Event Handlers
        void Page_PreInit(object sender, System.EventArgs args)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ChangePassword);
                peopleInfo = Utils.GetCurrentUser();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ChangePassword);
                peopleInfo = Utils.GetCurrentUser();

                if (!Page.IsPostBack)
                    ObjectsToForm();
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
                    PeopleController.GetInstance().UpdatePerson(peopleInfo);
                    pnlForm.Visible = false;
                    pnlMessage.Visible = true;
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            // Response.Redirect("~/Modules/Projects/SearchParticipationsSubContractor.aspx");
            Response.Redirect("~/Modules/Projects/ClientProjects.aspx", false);
        }

        protected void valCurrentPassword2_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = txtCurrentPassword.Text == peopleInfo.Password;
        }

        protected void valConfirmation2_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = txtNewPassword.Text == txtConfirmation.Text;
        }
#endregion

    }
}