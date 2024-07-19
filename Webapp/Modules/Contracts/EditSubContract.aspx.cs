using System;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditSubContractPage : SOSPage
    {

#region Members
        private ContractInfo subContract;
        private ContractTemplateInfo contractTemplateInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (subContract == null)
                return null;

            tempNode.ParentNode.Url += "?ContractId=" + subContract.ParentContract.IdStr;

            tempNode.ParentNode.ParentNode.Url += "?ContractId=" + subContract.ParentContract.IdStr;
            
            tempNode.ParentNode.ParentNode.ParentNode.Title = subContract.Trade.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?TradeId=" + subContract.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Title = subContract.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + subContract.Trade.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (subContract.Id == null)
            {
                TitleBar.Title = "Adding Variation Order";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Variation Order";
            }

            lblSubcontractor.Text = UI.Utils.GetFormString(subContract.Trade.SelectedSubContractorName);
            lblNumber.Text = UI.Utils.SetFormInteger(subContract.SubcontractNumber);

            txtGoodsServicesTax.Text = UI.Utils.SetFormEditPercentage(subContract.GoodsServicesTax);
            txtTitle.Text = UI.Utils.GetFormString(subContract.Description);
            txtSiteInstruction.Text = UI.Utils.SetFormString(subContract.SiteInstruction);
            sdrSiteInstructionDate.Date = subContract.SiteInstructionDate;
            txtSubContractorReference.Text = UI.Utils.SetFormString(subContract.SubContractorReference);
            sdrSubContractorReferenceDate.Date = subContract.SubContractorReferenceDate;
            txtComments.Text = UI.Utils.SetFormString(subContract.Comments);

            sfsQuotesFile.FilePath = subContract.QuotesFile;
            sfsQuotesFile.Path = subContract.Trade.Project.AttachmentsFolder;

            chkOrigContractDur.Checked = UI.Utils.SetFormBoolean(subContract.CheckOrigContractDur);    // DS20240321
            sdrStartDate.Date = subContract.StartDate;    // DS20240321
            sdrFinishDate.Date = subContract.FinishDate;  // DS20240321
        }

        private void FormToObjects()
        {
            subContract.GoodsServicesTax = UI.Utils.GetFormPercentage(txtGoodsServicesTax.Text);
            subContract.Description = UI.Utils.GetFormString(txtTitle.Text);
            subContract.SiteInstruction = UI.Utils.GetFormString(txtSiteInstruction.Text);
            subContract.SiteInstructionDate = sdrSiteInstructionDate.Date;
            subContract.SubContractorReference = UI.Utils.GetFormString(txtSubContractorReference.Text);
            subContract.SubContractorReferenceDate = sdrSubContractorReferenceDate.Date;
            subContract.Comments = UI.Utils.GetFormString(txtComments.Text);

            subContract.QuotesFile = sfsQuotesFile.FilePath;
            if (chkOrigContractDur.Checked)  {subContract.CheckOrigContractDur = true;} else { subContract.CheckOrigContractDur = false; }  // DS20240321
            subContract.StartDate = sdrStartDate.Date;    // DS20240321
            subContract.FinishDate = sdrFinishDate.Date;  // DS20240321

            if (contractTemplateInfo != null)
                subContract.Template = contractTemplateInfo.VariationTemplate;
            else
                subContract.Template = ContractsController.TagEditableOpen + "NoTemplate " + ContractsController.TagTitle + "No Template" + ContractsController.TagEditableEnd + "No template defined." + ContractsController.TagEditableClose;
        }
#endregion        

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterSubContractId = Request.Params["SubContractId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditContract);

                if (parameterSubContractId == null)
                {
                    String parameterContractId = Utils.CheckParameter("ContractId");
                    ContractInfo contractInfo = contractsController.GetContractWithSubContracts(Int32.Parse(parameterContractId));
                    Core.Utils.CheckNullObject(contractInfo, parameterContractId, "Contract");

                    subContract = new ContractInfo();
                    subContract.WriteDate = DateTime.Now;
                    subContract.GoodsServicesTax = Decimal.Parse(Web.Utils.GetConfigListItemValue("Global", "Settings", "GST"));
                    subContract.SubcontractNumber = contractInfo.NextSubContractNumber;

                    subContract.ParentContract = contractInfo;
                    subContract.Trade = tradesController.GetTradeWithParticipations(contractInfo.Trade.Id);
                    subContract.Trade.Project = projectsController.GetProject(contractInfo.Trade.Project.Id);

                    contractTemplateInfo = ContractsController.GetInstance().GetContractTemplate(subContract.Trade);
                }
                else
                {
                    subContract = contractsController.GetContract(Int32.Parse(parameterSubContractId));
                    Core.Utils.CheckNullObject(subContract, parameterSubContractId, "Variation Order");
                    subContract.Trade = tradesController.GetTradeWithParticipations(subContract.Trade.Id);

                    processController.CheckEditCurrentUser(subContract);
                }

                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }
                else
                {
                }
                if (chkOrigContractDur.Checked == true)
                {
                    sdrStartDate.Enabled = false;
                    sdrFinishDate.Enabled = false;
                    sdrStartDate.Date = null;
                    sdrFinishDate.Date = null;
                    subContract.StartDate = null;    // DS20240321
                    subContract.FinishDate = null;  // DS20240321
                }
                else
                {
                    sdrStartDate.Enabled = true;
                    sdrFinishDate.Enabled = true;
             }

                }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void chkOrigContractDur_Changed(object sender, EventArgs e)
        {
            if (chkOrigContractDur.Checked == true)
            {
                sdrStartDate.Enabled = false;
                sdrFinishDate.Enabled = false;
                sdrStartDate.Date = null;
                sdrFinishDate.Date = null;
                subContract.StartDate = null;    // DS20240321
                subContract.FinishDate = null;  // DS20240321
            }
            else
            {
                sdrStartDate.Enabled = true;
                sdrFinishDate.Enabled = true;
            }
        }
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    subContract.Id = ContractsController.GetInstance().AddUpdateContract(subContract);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/ViewSubContract.aspx?SubContractId=" + subContract.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (subContract.Id != null)
                Response.Redirect("~/Modules/Contracts/ViewSubContract.aspx?SubContractId=" + subContract.IdStr);
            else
                Response.Redirect("~/Modules/Contracts/ListSubContracts.aspx?ContractId=" + subContract.ParentContract.IdStr);
        }
#endregion

    }
}
