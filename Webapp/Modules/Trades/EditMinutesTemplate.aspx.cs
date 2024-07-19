using System;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditMinutesTemplatePage : System.Web.UI.Page
    {

#region Members
        private MinutesTemplateInfo minutesTemplateInfo = null;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            htmlEditor.Content = UI.Utils.SetFormString(minutesTemplateInfo.Template);
        }

        private void FormToObjects()
        {
            minutesTemplateInfo.Template = UI.Utils.GetFormString(htmlEditor.Content);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterMinutesTemplateId;

            try
            {
                Security.CheckAccess(Security.userActions.EditMinutesTemplate);
                parameterMinutesTemplateId = Request.Params["MinutesTemplateId"];
                minutesTemplateInfo = TradesController.GetInstance().GetMinutesTemplate(Int32.Parse(parameterMinutesTemplateId));
                Core.Utils.CheckNullObject(minutesTemplateInfo, parameterMinutesTemplateId, "Order Letting Minutes Template");

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
                    TradesController.GetInstance().UpdateMinutesTemplate(minutesTemplateInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Trades/ViewMinutesTemplate.aspx?MinutesTemplateId=" + minutesTemplateInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/ViewMinutesTemplate.aspx?MinutesTemplateId=" + minutesTemplateInfo.IdStr);
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
