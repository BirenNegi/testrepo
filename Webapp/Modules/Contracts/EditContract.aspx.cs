using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditContractPage : SOSPage
    {

#region Members
        private ContractInfo contractInfo;
        private String parameterSectionName;
        private String sectionTitle;
        private String currentText;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            lblProject.Text = UI.Utils.SetFormString(contractInfo.Trade.Project.Name);
            lblTrade.Text = UI.Utils.SetFormString(contractInfo.Trade.Name);
            lblSection.Text = UI.Utils.SetFormString(sectionTitle);
            htmlEditor.Content = UI.Utils.SetFormString(currentText);
        }
#endregion

#region Event Handlers
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (contractInfo == null)
                return null;

            tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?ContractId=" + contractInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = contractInfo.Trade.Name;
            tempNode.ParentNode.ParentNode.Url += "?TradeId=" + contractInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = contractInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + contractInfo.Trade.Project.IdStr;

            return currentNode;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ContractModificationInfo contractModificationInfo;
            List<String> editableSections;
            String parameterContractId;
            String editableSection;

            try
            {
                Security.CheckAccess(Security.userActions.EditContract);
                parameterContractId = Utils.CheckParameter("ContractId");
                parameterSectionName = Utils.CheckParameter("SectionName");
                contractInfo = contractsController.GetContractWithModificationsAndVariations(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");
                processController.CheckEditCurrentUser(contractInfo);
                
                editableSections = contractsController.GetTemplateEditables(contractInfo.Template);
                editableSection = editableSections.Find(delegate(String editableSectionFind) { return contractsController.GetTemplateEditableName(editableSectionFind) == parameterSectionName; });
                contractModificationInfo = contractInfo.ContractModifications.Find(delegate(ContractModificationInfo contractModificationInfoFind) { return contractModificationInfoFind.SectionName == parameterSectionName; });
                currentText = contractModificationInfo != null ? contractModificationInfo.SectionText : contractsController.GetTemplateEditableTextSection(contractInfo.Template, editableSection);
                sectionTitle = contractsController.GetTemplateEditableTitle(editableSection);

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
                    if (htmlEditor.Content != currentText)
                    {
                        ContractModificationInfo contractModificationInfo = new ContractModificationInfo();

                        contractModificationInfo.Contract = contractInfo;
                        contractModificationInfo.SectionName = parameterSectionName;
                        contractModificationInfo.SectionText = htmlEditor.Content;

                        ContractsController.GetInstance().AddContractModification(contractModificationInfo);
                    }
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/ViewContract.aspx?ContractId=" + contractInfo.IdStr + "#" + parameterSectionName);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/ViewContract.aspx?ContractId=" + contractInfo.IdStr + "#" + parameterSectionName);
        }
#endregion
    
    }
}
