using System;
using SOS.Core;
using System.Web;

namespace SOS.Web
{
    public partial class CreateClientContactAccount : SOSPage
    {
        #region Members
        private ClientContactInfo clientContactInfo = null;
        private String ProjectId;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;
            
            if (clientContactInfo == null)
                return null;

            tempNode.ParentNode.Title = clientContactInfo.Name;
            tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?PeopleId=" + clientContactInfo.IdStr;

            return currentNode;
            
        }

        private void ObjectsToForm()
        {
            lblFirstName.Text = UI.Utils.SetFormString(clientContactInfo.FirstName);
            lblLastName.Text = UI.Utils.SetFormString(clientContactInfo.LastName);
            txtEmail.Text = UI.Utils.SetFormString(clientContactInfo.Email);

            //Utils.GetConfigList("Global", "ContactTypes", ddlAccessLevel, null);
        }

        private void FormToObjects()
        {
            clientContactInfo.Email = UI.Utils.GetFormString(txtEmail.Text);
            clientContactInfo.Login = UI.Utils.GetFormString(txtUserName.Text);
           // clientContactInfo.UserType = UI.Utils.GetFormString(ddlAccessLevel.SelectedValue);
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterPeopleId = Request.Params["PeopleId"];
            ProjectId= Request.Params["ProjectId"].ToString();
            try
            {
                Security.CheckAccess(Security.userActions.CreateClientContactAccount);
                parameterPeopleId = Utils.CheckParameter("PeopleId");
                clientContactInfo = (ClientContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);
                Core.Utils.CheckNullObject(clientContactInfo, parameterPeopleId, "Contact");

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
                    clientContactInfo.Password = Security.GeneratePassword();
                    PeopleController.GetInstance().UpdatePerson(clientContactInfo);
                    Core.Utils.SendPassword(clientContactInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId=" + ProjectId);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/People/EditClientContactAccess.aspx?ProjectId=" + ProjectId);
        }
        #endregion
    }
}