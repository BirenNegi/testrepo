using System;
using System.Web;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewInvitationTemplatePage : System.Web.UI.Page
    {

#region Members
        private InvitationTemplateInfo invitationTemplateInfo = null;
#endregion

#region Private Methods
        private void BindInvitationTemplate()
        {
            litTemplate.Text = ContractsController.GetInstance().ViewTemplate(invitationTemplateInfo.Template);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterInvitationTemplateId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewInvitationTemplate);
                parameterInvitationTemplateId = Utils.CheckParameter("InvitationTemplateId");
                invitationTemplateInfo = TradesController.GetInstance().GetInvitationTemplate(Int32.Parse(parameterInvitationTemplateId));
                Core.Utils.CheckNullObject(invitationTemplateInfo, parameterInvitationTemplateId, "Invitation to Tender Template");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditInvitationTemplate))
                    {
                        cmdEditTop.Visible = true;
                    }
                    BindInvitationTemplate();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/EditInvitationTemplate.aspx?InvitationTemplateId=" + invitationTemplateInfo.IdStr);
        }
#endregion

    }
}
