using System;
using System.Web;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewMinutesTemplatePage : System.Web.UI.Page
    {

#region Members
        private MinutesTemplateInfo minutesTemplateInfo = null;
#endregion

#region Private Methods
        private void BindMinutesTemplate()
        {
            ContractsController contractsController = ContractsController.GetInstance();

            List<String> templateEditables = contractsController.GetTemplateEditables(minutesTemplateInfo.Template);
            List<String> templateVariables = contractsController.GetTemplateVariables(minutesTemplateInfo.Template);

            StringBuilder stringBuilder = new StringBuilder(minutesTemplateInfo.Template);

            String openEditable = "<span class='TemplateEditable'>&nbsp;<img src='" + Web.Utils.GetEditImage() + "' alt='{0}'>&nbsp;";
            String closeEditable = "&nbsp;</span>";
            String openVariable = "<span class='TemplateVariable'>&nbsp;";
            String closeVariable = "&nbsp;</span>";

            stringBuilder.Replace(ContractsController.TagEditableClose, closeEditable);
            foreach (String templateEditable in templateEditables)
                stringBuilder.Replace(templateEditable, openEditable.Replace("{0}", contractsController.GetTemplateEditableTitle(templateEditable)));

            foreach (String templateVariable in templateVariables)
                stringBuilder.Replace(templateVariable, openVariable + contractsController.GetTemplateVariableTitle(templateVariable) + closeVariable);

            litTemplate.Text = stringBuilder.ToString();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterMinutesTemplateId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewMinutesTemplate);
                parameterMinutesTemplateId = Utils.CheckParameter("MinutesTemplateId");
                minutesTemplateInfo = TradesController.GetInstance().GetMinutesTemplate(Int32.Parse(parameterMinutesTemplateId));
                Core.Utils.CheckNullObject(minutesTemplateInfo, parameterMinutesTemplateId, "Order Letting Minutes Template");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditMinutesTemplate))
                    {
                        cmdEditTop.Visible = true;
                    }
                    BindMinutesTemplate();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/EditMinutesTemplate.aspx?MinutesTemplateId=" + minutesTemplateInfo.IdStr);
        }
#endregion

    }
}
