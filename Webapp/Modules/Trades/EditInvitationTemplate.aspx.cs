using System;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditInvitationTemplatePage : System.Web.UI.Page
    {

#region Members
        private InvitationTemplateInfo invitationTemplateInfo = null;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            htmlEditor.Content = UI.Utils.SetFormString(invitationTemplateInfo.Template);
        }

        private void FormToObjects()
        {
            invitationTemplateInfo.Template = UI.Utils.GetFormString(htmlEditor.Content);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterInvitationTemplateId;

            try
            {
                Security.CheckAccess(Security.userActions.EditInvitationTemplate);
                parameterInvitationTemplateId = Request.Params["InvitationTemplateId"];
                invitationTemplateInfo = TradesController.GetInstance().GetInvitationTemplate(Int32.Parse(parameterInvitationTemplateId));
                Core.Utils.CheckNullObject(invitationTemplateInfo, parameterInvitationTemplateId, "Invitation to Tender Template");

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
                    TradesController.GetInstance().UpdateInvitationTemplate(invitationTemplateInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Trades/ViewInvitationTemplate.aspx?InvitationTemplateId=" + invitationTemplateInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/ViewInvitationTemplate.aspx?InvitationTemplateId=" + invitationTemplateInfo.IdStr);
        }

        protected void valTemplate_Validate(object source, ServerValidateEventArgs value)
        {
            String errTemplate = ContractsController.GetInstance().ValidateTemplate(htmlEditor.Content);

            if (errTemplate == null)
            {
                value.IsValid = true;
            }
            else
            {
                value.IsValid = false;
                valTemplate.Text = "Error: " + errTemplate;
            }
        }
#endregion

    }
}
