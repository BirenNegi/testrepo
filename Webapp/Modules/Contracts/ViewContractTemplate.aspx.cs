using System;
using System.Web;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewContractTemplatePage : System.Web.UI.Page
    {

#region Members
        private ContractTemplateInfo contractTemplateInfo = null;
        private String parameterContratTemplateType = null;
#endregion

#region Private Methods
        private void BindContractTemplate()
        {
            ContractsController contractsController = ContractsController.GetInstance();

            switch (parameterContratTemplateType)
            {
                case "Std":
                    lblContractType.Text = "Standard";
                    litTemplate.Text = contractsController.ViewTemplate(contractTemplateInfo.StandardTemplate);
                    break;
                case "Sim":
                    lblContractType.Text = "Simplified";
                    litTemplate.Text = contractsController.ViewTemplate(contractTemplateInfo.SimplifiedTemplate);
                    break;
                case "Var":
                    lblContractType.Text = "Variation";
                    litTemplate.Text = contractsController.ViewTemplate(contractTemplateInfo.VariationTemplate);
                    break;
                default:
                    lblContractType.Text = "Invalid Type";
                    break;
            }

            lblBusninessUnit.Text = UI.Utils.SetFormString(contractTemplateInfo.BusinessUnit.Name);
            lblJobType.Text = UI.Utils.SetFormString(contractTemplateInfo.JobType.Name);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterContractTemplateId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewContractTemplate);
                parameterContractTemplateId = Utils.CheckParameter("ContractTemplateId");
                parameterContratTemplateType = Utils.CheckParameter("Type");
                contractTemplateInfo = ContractsController.GetInstance().GetContractTemplate(Int32.Parse(parameterContractTemplateId));
                Core.Utils.CheckNullObject(contractTemplateInfo, parameterContractTemplateId, "Contract Template");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditContractTemplate))
                    {
                        cmdEditTop.Visible = true;
                        cmdDeleteTop.Visible = true;

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Contract Template ?');");
                    }
                    BindContractTemplate();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/EditContractTemplate.aspx?Type=" + parameterContratTemplateType + "&ContractTemplateId=" + contractTemplateInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                switch (parameterContratTemplateType)
                {
                    case "Std": contractTemplateInfo.StandardTemplate = null; break;
                    case "Sim": contractTemplateInfo.SimplifiedTemplate = null; break;
                    case "Var": contractTemplateInfo.VariationTemplate = null; break;
                }

                if (contractTemplateInfo.StandardTemplate == null && contractTemplateInfo.SimplifiedTemplate == null && contractTemplateInfo.VariationTemplate == null)
                {
                    ContractsController.GetInstance().DeleteContractTemplate(contractTemplateInfo);
                }
                else
                {
                    ContractsController.GetInstance().UpdateContractTemplate(contractTemplateInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Contracts/ListContractTemplates.aspx");
        }
#endregion

    }
}