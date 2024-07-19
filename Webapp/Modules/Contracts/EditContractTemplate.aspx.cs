using System;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditContractTemplatePage : System.Web.UI.Page
    {

#region Members
        private ContractTemplateInfo contractTemplateInfo = null;
        private String parameterContratTemplateType = null;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            if (contractTemplateInfo.Id == null)
            {
                TitleBar.Title = "Adding Contract Template.";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Contract Template";
            }

            switch (parameterContratTemplateType)
            {
                case "Std":
                    lblContractType.Text = "Standard";
                    htmlEditor.Content = UI.Utils.SetFormString(contractTemplateInfo.StandardTemplate);
                    break;
                case "Sim":
                    lblContractType.Text = "Simplified";
                    htmlEditor.Content = UI.Utils.SetFormString(contractTemplateInfo.SimplifiedTemplate);
                    break;
                case "Var":
                    lblContractType.Text = "Variation";
                    htmlEditor.Content = UI.Utils.SetFormString(contractTemplateInfo.VariationTemplate);
                    break;
                default:
                    lblContractType.Text = "Invalid Type";
                    htmlEditor.Content = String.Empty;
                    break;
            }

            lblBusninessUnit.Text = UI.Utils.SetFormString(contractTemplateInfo.BusinessUnit.Name);
            lblJobType.Text = UI.Utils.SetFormString(contractTemplateInfo.JobType.Name);
        }

        private void FormToObjects()
        {
            switch (parameterContratTemplateType)
            {
                case "Std": contractTemplateInfo.StandardTemplate = UI.Utils.GetFormString(htmlEditor.Content); break;
                case "Sim": contractTemplateInfo.SimplifiedTemplate = UI.Utils.GetFormString(htmlEditor.Content); break;
                case "Var": contractTemplateInfo.VariationTemplate = UI.Utils.GetFormString(htmlEditor.Content); break;
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterContractTemplateId;
            ContractsController contractsController = ContractsController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.EditContractTemplate);

                parameterContractTemplateId = Request.Params["ContractTemplateId"];
                parameterContratTemplateType = Utils.CheckParameter("Type");

                if (parameterContractTemplateId == null)
                {
                    BusinessUnitInfo businessUnitInfo = contractsController.GetBusinessUnit((int?)Utils.CheckParameterInt32("BusinessUnitId"));
                    JobTypeInfo jobTypeInfo = contractsController.GetJobType((int?)Utils.CheckParameterInt32("JobTypeId"));

                    contractTemplateInfo = new ContractTemplateInfo();
                    contractTemplateInfo.BusinessUnit = businessUnitInfo;
                    contractTemplateInfo.JobType = jobTypeInfo;
                }
                else
                {
                    contractTemplateInfo = contractsController.GetContractTemplate(Int32.Parse(parameterContractTemplateId));
                    Core.Utils.CheckNullObject(contractTemplateInfo, parameterContractTemplateId, "Business Unit");
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
                    contractTemplateInfo.Id = ContractsController.GetInstance().AddUpdateContractTemplate(contractTemplateInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/ViewContractTemplate.aspx?Type=" + parameterContratTemplateType + "&ContractTemplateId=" + contractTemplateInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (contractTemplateInfo.Id == null)
            {
                Response.Redirect("~/Modules/Contracts/ListContractTemplates.aspx");
            }
            else
            {
                Response.Redirect("~/Modules/Contracts/ViewContractTemplate.aspx?Type=" + parameterContratTemplateType + "&ContractTemplateId=" + contractTemplateInfo.IdStr);
            }
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