using System;
using System.Web;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewContractSectionPage : SOSPage
    {

#region Members
        private ContractInfo contractInfo;
        private List<ContractModificationInfo> contractModificationInfoList = new List<ContractModificationInfo>();
        private String parameterSectionName;
        private String sectionTitle;
        private String originalText;
#endregion
        
#region Private Methods
        private void BindContractSection()
        {
            lblSection.Text = sectionTitle;
            litOrignialText.Text = originalText;
            lnkModify.NavigateUrl = "~/Modules/Contracts/EditContract.aspx?ContractId=" + contractInfo.IdStr + "&SectionName=" + parameterSectionName;

            repModifications.DataSource = contractModificationInfoList;
            repModifications.DataBind();
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
            tempNode.ParentNode.ParentNode.Url = tempNode.ParentNode.ParentNode.Url + "?TradeId=" + contractInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = contractInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url = tempNode.ParentNode.ParentNode.ParentNode.Url + "?ProjectId=" + contractInfo.Trade.Project.IdStr;

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
            Int32 i;
            
            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterContractId = Utils.CheckParameter("ContractId");
                parameterSectionName = Utils.CheckParameter("SectionName");
                contractInfo = contractsController.GetContractWithModificationsAndVariations(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");

                if (Security.ViewAccess(Security.userActions.EditContract))
                    if (processController.AllowEditCurrentUser(contractInfo))
                        lnkModify.Visible = true;

                editableSections = contractsController.GetTemplateEditables(contractInfo.Template);
                editableSection = editableSections.Find(delegate(String editableSectionFind) { return contractsController.GetTemplateEditableName(editableSectionFind) == parameterSectionName; });
                sectionTitle = contractsController.GetTemplateEditableTitle(editableSection);
                originalText = contractsController.GetTemplateEditableTextSection(contractInfo.Template, editableSection);
                contractModificationInfo = contractInfo.ContractModifications.Find(delegate(ContractModificationInfo contractModificationInfoFind) { return contractModificationInfoFind.SectionName == parameterSectionName; });
                if (contractModificationInfo != null)
                {
                    i = contractInfo.ContractModifications.IndexOf(contractModificationInfo);
                    while (i < contractInfo.ContractModifications.Count && contractInfo.ContractModifications[i].SectionName == parameterSectionName)
                    {
                        contractModificationInfoList.Add(contractInfo.ContractModifications[i]);
                        i++;
                    }
                }

                if (!Page.IsPostBack)
                {
                    BindContractSection();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
       
    }
}