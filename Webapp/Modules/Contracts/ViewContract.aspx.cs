using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewContractPage : SOSPage
    {

#region Members
        private ContractInfo contractInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (contractInfo == null)
                return null;

            tempNode.ParentNode.Title = contractInfo.Trade.Name;
            tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?TradeId=" + contractInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = contractInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.Url = tempNode.ParentNode.ParentNode.Url + "?ProjectId=" + contractInfo.Trade.Project.IdStr;

            return currentNode;
        }

        private void BindContract()
        {
            ContractsController contractsController = ContractsController.GetInstance();
            List<String> editableSections = contractsController.GetTemplateEditables(contractInfo.Template);
            StringBuilder stringBuilder = new StringBuilder(contractInfo.Template);
            Boolean isEditable = ProcessController.GetInstance().AllowEditCurrentUser(contractInfo);
            ContractModificationInfo contractModification;
            TreeNode treeNode;
            String sectionName;
            String originalText;
            String newText;
            Int32 i;
            Int32 j = 0;

            originalText = contractsController.GetTemplateFooterFull(contractInfo.Template);
            if (originalText != null)
                stringBuilder.Replace(originalText, "<span class='TemplateFooter'>" + contractsController.GetTemplateFooterText(contractInfo.Template) + "</span>");

            originalText = contractsController.GetTemplateFinancialFull(contractInfo.Template);
            if (originalText != null)
                stringBuilder.Replace(originalText, "<span class='TemplateFinancial'>" + contractsController.GetTemplateFinancialText(contractInfo.Template) + "</span>");

            originalText = contractsController.GetTemplateTermsFull(contractInfo.Template);
            if (originalText != null)
                stringBuilder.Replace(originalText, "<span class='TemplateTerms'>" + contractsController.GetTemplateTermsText(contractInfo.Template) + "</span>");
          
            foreach (String editableSection in editableSections)
            {
              
                sectionName = contractsController.GetTemplateEditableName(editableSection);
                contractModification = contractInfo.ContractModifications.Find(delegate(ContractModificationInfo contractModificationFind) {return contractModificationFind.SectionName == sectionName; });
                originalText = contractsController.GetTemplateEditableFullSection(contractInfo.Template, editableSection);
                newText = contractModification != null ? contractModification.SectionText : contractsController.GetTemplateEditableTextSection(contractInfo.Template, editableSection);
                //#---12/11/2018
                if (newText.Contains("\r\n"))
                {
                    newText = newText.Replace(System.Environment.NewLine, "<br />");
                }
                //#--12/11/2018

                if (isEditable)
                    //#---stringBuilder.Replace(originalText, "<a name='" + sectionName + "'></a><span class='TemplateEditable'>&nbsp;<a href='" + "EditContract.aspx?ContractId=" + contractInfo.IdStr + "&SectionName=" + sectionName + "'><img src='" + Web.Utils.GetEditImage() + "' alt='" + contractsController.GetTemplateEditableTitle(editableSection) + "' border='0'></a>&nbsp;" + newText + "</span>");
                    
                    //#--- to allow to edit contract amount to only Admin access
                    if (sectionName == "VariableContractValue" || sectionName == "VariableContractGST" || sectionName == "VariableContractTotal" || sectionName == "VariableContratTotalWords")
                    {
                        if (Security.IsAdmin())
                            stringBuilder.Replace(originalText, "<a name='" + sectionName + "'></a><span class='TemplateEditable'>&nbsp;<a href='" + "EditContract.aspx?ContractId=" + contractInfo.IdStr + "&SectionName=" + sectionName + "'><img src='" + Web.Utils.GetEditImage() + "' alt='" + contractsController.GetTemplateEditableTitle(editableSection) + "' border='0'></a>&nbsp;" + newText + "</span>");
                        else 
                           stringBuilder.Replace(originalText, "<span class='TemplateEditable'>" + newText + "</span>");
                    }
                    else
                    {   stringBuilder.Replace(originalText, "<a name='" + sectionName + "'></a><span class='TemplateEditable'>&nbsp;<a href='" + "EditContract.aspx?ContractId=" + contractInfo.IdStr + "&SectionName=" + sectionName + "'><img src='" + Web.Utils.GetEditImage() + "' alt='" + contractsController.GetTemplateEditableTitle(editableSection) + "' border='0'></a>&nbsp;" + newText + "</span>");
                    }
                //---#
                else
                    stringBuilder.Replace(originalText, "<span class='TemplateEditable'>" + newText + "</span>");
                //if (sectionName == "VariablePaymentTerms")
                //{
                //    int AA = 0;
                //    AA += 1;
                //}
                    if (contractModification != null)
                {
                    TreeView1.Nodes[0].ChildNodes.Add(new TreeNode(contractsController.GetTemplateEditableTitle(editableSection)));
                    TreeView1.Nodes[0].ChildNodes[j].Expanded = false;
                    TreeView1.Nodes[0].ChildNodes[j].NavigateUrl = "~/Modules/Contracts/ViewContractSection.aspx?ContractId=" + contractInfo.IdStr + "&SectionName=" + contractModification.SectionName;

                    i = contractInfo.ContractModifications.IndexOf(contractModification);
                    while (i < contractInfo.ContractModifications.Count && contractInfo.ContractModifications[i].SectionName == sectionName)
                    {
                        treeNode = new TreeNode();
                        treeNode.Text = UI.Utils.SetFormDateTime(contractInfo.ContractModifications[i].CreatedDate) + " " + contractInfo.ContractModifications[i].Employee.Name;
                        treeNode.SelectAction = TreeNodeSelectAction.None;

                        TreeView1.Nodes[0].ChildNodes[j].ChildNodes.Add(treeNode);
                        i++;
                    }
                    j++;
                }
            }

            if (TreeView1.Nodes[0].ChildNodes.Count > 0)
                phChanges.Visible = true;

            if (contractInfo.IsApproved)
            {
                lnkPrint.NavigateUrl = "~/Modules/Contracts/ShowContract.aspx?ContractId=" + contractInfo.IdStr;
                lnkPrint.Visible=true;
            }

            litContract.Text = stringBuilder.ToString();

            if (!contractInfo.IsSubContract)
            {
                pnlCheckList.Visible = true;

                sbvQuotes.Checked = contractInfo.CheckQuotes;
                sbvWinningQuote.Checked = contractInfo.CheckWinningQuote;
                sbvComparison.Checked = contractInfo.CheckComparison;
                sbvCheckList.Checked = contractInfo.CheckCheckList;
                sbvPrelettingMinutes.Checked = contractInfo.CheckPrelettingMinutes;
                sbvAmendments.Checked = contractInfo.CheckAmendments;
                lblComments.Text = UI.Utils.SetFormString(contractInfo.Comments);
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterContractId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewContract);
                parameterContractId = Utils.CheckParameterWithSection("ContractId");
                contractInfo = ContractsController.GetInstance().GetContractWithModificationsAndVariations(Int32.Parse(parameterContractId));
                Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");

                if (!Page.IsPostBack)
                {
                    BindContract();
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