using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditClientVariationPage : SOSPage
    {

        #region Members
        private ClientVariationInfo clientVariationInfo = null;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (clientVariationInfo == null)
                return null;

            //#---- tempNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Client Variation";

            tempNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : clientVariationInfo is TenantVariationInfo ? "Tenant Variation" : "Client Variation";
            // #-----

            tempNode.ParentNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Accounts" : clientVariationInfo is TenantVariationInfo ? "Tenant Variation" : "Client Variations";//---#

            tempNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr + "&Type=" + clientVariationInfo.Type;

            tempNode.ParentNode.ParentNode.Title = clientVariationInfo.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            if (clientVariationInfo.Id == null)
            {
                //#--TitleBar.Title = clientVariationInfo is SeparateAccountInfo ? "Adding Separate Account" :"Adding Client Variation";

                TitleBar.Title = clientVariationInfo is SeparateAccountInfo ? "Adding Separate Account" : clientVariationInfo is TenantVariationInfo ? "Adding Tenant Varation" : "Adding Client Variation"; //--#
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                //----#---TitleBar.Title = clientVariationInfo is SeparateAccountInfo ? "Updating Separate Account" : "Updating Client Variation";
                TitleBar.Title = clientVariationInfo is SeparateAccountInfo ? "Updating Separate Account" : clientVariationInfo is TenantVariationInfo ? "Updating Tenant Variation" : "Updating Client Variation";//---#
            }

            if (clientVariationInfo is SeparateAccountInfo)
            {
                SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                Title = "Separate Account";

                if (separateAccountInfo.WorksCompleted)
                {
                    phInvoice.Visible = true;

                    txtInvoiceNumber.Text = UI.Utils.SetFormEditInteger(separateAccountInfo.InvoiceNumber);
                    lblInvoiceSentDate.Text = UI.Utils.SetFormDate(separateAccountInfo.InvoiceSentDate);
                    sdrInvoiceDate.Date = separateAccountInfo.InvoiceDate;
                    sdrInvoiceDueDate.Date = separateAccountInfo.InvoiceDueDate;
                    sdrInvoicePaidDate.Date = separateAccountInfo.InvoicePaidDate;
                }
                else
                {
                    phInvoice.Visible = false;
                }

                phUseSecondPrincipal.Visible = true;
                sbrUseSecondPrincipal.Checked = separateAccountInfo.UseSecondPrincipal;
            }

            //---#---Tenant Variation
            else if (clientVariationInfo is TenantVariationInfo)
            {
                TenantVariationInfo tenantVariationInfo = (TenantVariationInfo)clientVariationInfo;

                Title = "Tenant Variation";
                phInvoice.Visible = false;
                phUseSecondPrincipal.Visible = false;
            }
            //---#---
            else
            {
                Title = "Client Variation";
                phInvoice.Visible = false;
                phUseSecondPrincipal.Visible = false;
            }

            lblNumber.Text = UI.Utils.SetFormInteger(clientVariationInfo.Number);
            txtName.Text = UI.Utils.SetFormString(clientVariationInfo.Name);
            sbrShowCostDetails.Checked = clientVariationInfo.ShowCostDetails;
            txtComments.Text = UI.Utils.SetFormString(clientVariationInfo.Comments);

            sfsQuotesFile.FilePath = clientVariationInfo.QuotesFile;
            sfsQuotesFile.Path = clientVariationInfo.Project.AttachmentsFolder;

            sfsBackupFile.FilePath = clientVariationInfo.BackupFile;
            sfsBackupFile.Path = clientVariationInfo.Project.AttachmentsFolder;

            sfsClientApprovalFile.FilePath = clientVariationInfo.ClientApprovalFile;
            sfsClientApprovalFile.Path = clientVariationInfo.Project.AttachmentsFolder;
        }

        private void FormToObjects()
        {
            clientVariationInfo.Name = UI.Utils.GetFormString(txtName.Text);
            clientVariationInfo.QuotesFile = sfsQuotesFile.FilePath;
            clientVariationInfo.BackupFile = sfsBackupFile.FilePath;
            clientVariationInfo.ClientApprovalFile = sfsClientApprovalFile.FilePath;
            clientVariationInfo.HideCostDetails = sbrShowCostDetails.NotChecked;
            clientVariationInfo.Comments = UI.Utils.GetFormString(txtComments.Text);

            if (clientVariationInfo is SeparateAccountInfo)
            {
                SeparateAccountInfo separateAccountInfo = (SeparateAccountInfo)clientVariationInfo;

                if (separateAccountInfo.WorksCompleted)
                {
                    separateAccountInfo.InvoiceNumber = UI.Utils.GetFormInteger(txtInvoiceNumber.Text);
                    separateAccountInfo.InvoiceDate = sdrInvoiceDate.Date;
                    separateAccountInfo.InvoiceDueDate = sdrInvoiceDueDate.Date;
                    separateAccountInfo.InvoicePaidDate = sdrInvoicePaidDate.Date;
                }

                separateAccountInfo.UseSecondPrincipal = sbrUseSecondPrincipal.Checked;
            }
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            String parameterClientVariationId;

            try
            {
                Security.CheckAccess(Security.userActions.EditClientVariation);
                parameterClientVariationId = Request.Params["ClientVariationId"];
                if (parameterClientVariationId == null)
                {
                    String parameterProjectId = Utils.CheckParameter("ProjectId");
                    String parameterType = Utils.CheckParameter("Type");

                    //#---20/09/2023-----TV 
                    if (parameterType != ClientVariationInfo.VariationTypeClient && parameterType != ClientVariationInfo.VariationTypeSeparateAccounts && parameterType != ClientVariationInfo.VariationTypeTenant)
                        throw new Exception("Invalid Type.");
                    //#---20/09/2023-----TV
                    clientVariationInfo = projectsController.GetClientVariationObject(parameterType);
                    clientVariationInfo.GoodsServicesTax = Decimal.Parse(Web.Utils.GetConfigListItemValue("Global", "Settings", "GST"));
                    clientVariationInfo.WriteDate = DateTime.Today;
                    clientVariationInfo.Project = projectsController.GetProject(Int32.Parse(parameterProjectId));

                    Core.Utils.CheckNullObject(clientVariationInfo.Project, parameterProjectId, "Project");

                    projectsController.SetNextClientVariationNumber(clientVariationInfo);
                }
                else
                {
                    clientVariationInfo = projectsController.GetClientVariation(Int32.Parse(parameterClientVariationId));
                    Core.Utils.CheckNullObject(clientVariationInfo, parameterClientVariationId, "Client Variation");
                    clientVariationInfo.Project = projectsController.GetProject(clientVariationInfo.Project.Id);
                }

                processController.CheckEditCurrentUser(clientVariationInfo);

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
                    clientVariationInfo.Id = ProjectsController.GetInstance().AddUpdateClientVariation(clientVariationInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (clientVariationInfo.Id == null)
                Response.Redirect("~/Modules/Projects/ListClientVariations.aspx?ProjectId=" + clientVariationInfo.Project.IdStr + "&Type=" + clientVariationInfo.Type);
            else
                Response.Redirect("~/Modules/Projects/ViewClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr);
        }
        #endregion

    }
}
