using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

using ProcessStatus = SOS.Core.ProcessStatus;

namespace SOS.Web
{
    public partial class EditProjectPage : SOSPage, ICallbackEventHandler
    {

        #region Members
        private ProjectInfo projectInfo = null;
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = projectInfo.Name;

            return currentNode;
        }

        private void ObjectsToForm()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));

            if (projectInfo.Id == null)
            {
                TitleBar.Title = "Adding Project";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";

                //--San------
                TxtAccountName.Text = "Vaughan Constructions PTY LTD";
                TxtBSB.Text = "083 004";
                TxtAccountNumber.Text = "51516 9569";

                //---San-------

            }
            else
            {
                TitleBar.Title = "Updating Project";
            }

            txtName.Text = UI.Utils.SetFormString(projectInfo.Name);
            txtNumber.Text = UI.Utils.SetFormString(projectInfo.Number);
            txtYear.Text = UI.Utils.SetFormString(projectInfo.Year);
            txtFax.Text = UI.Utils.SetFormString(projectInfo.Fax);
            txtDefectsLiability.Text = UI.Utils.SetFormInteger(projectInfo.DefectsLiability);
            txtLiquidatedDamages.Text = UI.Utils.SetFormString(projectInfo.LiquidatedDamages);
            txtSiteAllowances.Text = UI.Utils.SetFormString(projectInfo.SiteAllowances);
            txtRetention.Text = UI.Utils.SetFormString(projectInfo.Retention);
            txtRetentionToCertification.Text = UI.Utils.SetFormString(projectInfo.RetentionToCertification);
            txtRetentionToDLP.Text = UI.Utils.SetFormString(projectInfo.RetentionToDLP);
            txtInterest.Text = UI.Utils.SetFormString(projectInfo.Interest);
            txtPrincipal.Text = UI.Utils.SetFormString(projectInfo.Principal);
            txtPrincipalABN.Text = UI.Utils.SetFormString(projectInfo.PrincipalABN);
            txtStreet.Text = UI.Utils.SetFormString(projectInfo.Street);
            txtLocality.Text = UI.Utils.SetFormString(projectInfo.Locality);
            txtPostalCode.Text = UI.Utils.SetFormString(projectInfo.PostalCode);
            txtPrincipal2.Text = UI.Utils.SetFormString(projectInfo.Principal2);
            txtPrincipal2ABN.Text = UI.Utils.SetFormString(projectInfo.Principal2ABN);
            txtDeepZoomUrl.Text = UI.Utils.SetFormString(projectInfo.DeepZoomUrl);


            txtDescription.Text = UI.Utils.SetFormString(projectInfo.Description);
            txtSpecialClause.Text = UI.Utils.SetFormString(projectInfo.SpecialClause);
            //#---
            txtLawofSubcontract.Text = UI.Utils.SetFormString(projectInfo.LawOfSubcontract);

            if (projectInfo.Id != null)
            {
                TxtAccountName.Text = UI.Utils.SetFormString(projectInfo.AccountName);
                TxtBSB.Text = UI.Utils.SetFormString(projectInfo.BSB);
                TxtAccountNumber.Text = UI.Utils.SetFormString(projectInfo.AccountNumber);
            }
            //#---


            sdrCommencementDate.Date = projectInfo.CommencementDate;
            sdrCompletionDate.Date = projectInfo.CompletionDate;
            txtContractAmount.Text = UI.Utils.SetFormEditDecimal(projectInfo.ContractAmount);
            sdrFirstClaimDueDate.Date = projectInfo.FirstClaimDueDate;
            txtWaranty1Amount.Text = UI.Utils.SetFormEditDecimal(projectInfo.Waranty1Amount);
            sdrWaranty1Date.Date = projectInfo.Waranty1Date;
            txtWaranty2Amount.Text = UI.Utils.SetFormEditDecimal(projectInfo.Waranty2Amount);
            sdrWaranty2Date.Date = projectInfo.Waranty2Date;
            sdrPracticalCompletionDate.Date = projectInfo.PracticalCompletionDate;

            sfsAttachmentsFolder.FilePath = UI.Utils.SetFormString(projectInfo.AttachmentsFolder);
            sfsAttachmentsFolder.FileFolder = Core.Utils.SelectFolder;
            sfsAttachmentsFolder.Required = true;

            sfsMaintenanceManualFile.FilePath = projectInfo.MaintenanceManualFile;
            sfsMaintenanceManualFile.Path = projectInfo.AttachmentsFolder;

            sfsPostProjectReviewFile.FilePath = projectInfo.PostProjectReviewFile;
            sfsPostProjectReviewFile.Path = projectInfo.AttachmentsFolder;

            if (projectInfo.AttachmentsFolder == null)
            {
                sfsMaintenanceManualFile.Disabled = true;
                sfsMaintenanceManualFile.Disabled = true;
            }

            if (projectInfo.BusinessUnit != null)
                ddlBusinessUnit.SelectedValue = projectInfo.BusinessUnit.IdStr;

            if (projectInfo.ManagingDirector != null)
            {
                txtGMId.Value = projectInfo.ManagingDirector.IdStr;
                txtGM.Text = projectInfo.ManagingDirector.Name;
            }

            if (projectInfo.ContractsAdministrator != null)
            {
                txtCAId.Value = projectInfo.ContractsAdministrator.IdStr;
                txtCA.Text = projectInfo.ContractsAdministrator.Name;
            }

            if (projectInfo.ProjectManager != null)
            {
                txtPMId.Value = projectInfo.ProjectManager.IdStr;
                txtPM.Text = projectInfo.ProjectManager.Name;
            }

            if (projectInfo.ConstructionManager != null)
            {
                txtCMId.Value = projectInfo.ConstructionManager.IdStr;
                txtCM.Text = projectInfo.ConstructionManager.Name;
            }

            if (projectInfo.DesignManager != null)
            {
                txtDMId.Value = projectInfo.DesignManager.IdStr;
                txtDM.Text = projectInfo.DesignManager.Name;
            }

            if (projectInfo.DesignCoordinator != null)
            {
                txtDCId.Value = projectInfo.DesignCoordinator.IdStr;
                txtDC.Text = projectInfo.DesignCoordinator.Name;
            }

            if (projectInfo.FinancialController != null)
            {
                txtFCId.Value = projectInfo.FinancialController.IdStr;
                txtFC.Text = projectInfo.FinancialController.Name;
            }

            if (projectInfo.DirectorAuthorization != null)
            {
                txtDAId.Value = projectInfo.DirectorAuthorization.IdStr;
                txtDA.Text = projectInfo.DirectorAuthorization.Name;
            }

            if (projectInfo.BudgetAdministrator != null)
            {
                txtBAId.Value = projectInfo.BudgetAdministrator.IdStr;
                txtBA.Text = projectInfo.BudgetAdministrator.Name;
            }

            //#---------New Role COM and JCA added

            if (projectInfo.CommercialManager != null)
            {
                txtCOId.Value = projectInfo.CommercialManager.IdStr;
                txtCO.Text = projectInfo.CommercialManager.Name;
            }

            if (projectInfo.JuniorContractsAdministrator != null)
            {
                txtJCId.Value = projectInfo.JuniorContractsAdministrator.IdStr;
                txtJC.Text = projectInfo.JuniorContractsAdministrator.Name;
            }


            if (projectInfo.ContractsAdministrator3 != null)
            {
                TxtCA3Id.Value = projectInfo.ContractsAdministrator3.IdStr;
                TxtCA3.Text = projectInfo.ContractsAdministrator3.Name;
            }

            if (projectInfo.ContractsAdministrator4 != null)
            {
                TxtCA4Id.Value = projectInfo.ContractsAdministrator4.IdStr;
                TxtCA4.Text = projectInfo.ContractsAdministrator4.Name;
            }

            if (projectInfo.ContractsAdministrator5 != null)
            {
                TxtCA5Id.Value = projectInfo.ContractsAdministrator5.IdStr;
                TxtCA5.Text = projectInfo.ContractsAdministrator5.Name;
            }

            if (projectInfo.ContractsAdministrator6 != null)
            {
                TxtCA6Id.Value = projectInfo.ContractsAdministrator6.IdStr;
                TxtCA6.Text = projectInfo.ContractsAdministrator6.Name;
            }


            //#---------

            if (projectInfo.Foreman != null)
            {
                txtForemanId.Value = projectInfo.Foreman.IdStr;
                txtForeman.Text = projectInfo.Foreman.Name;
            }

            txtClientContactFirstName.Text = UI.Utils.SetFormString(projectInfo.ClientContact.FirstName);
            txtClientContactLastName.Text = UI.Utils.SetFormString(projectInfo.ClientContact.LastName);
            txtClientContactCompanyName.Text = UI.Utils.SetFormString(projectInfo.ClientContact.CompanyName);
            txtClientContactStreet.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Street);
            txtClientContactLocality.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Locality);
            txtClientContactPostalCode.Text = UI.Utils.SetFormString(projectInfo.ClientContact.PostalCode);
            txtClientContactEmail.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Email);
            txtClientContactPhone.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Phone);
            txtClientContactFax.Text = UI.Utils.SetFormString(projectInfo.ClientContact.Fax);
            chkClientContactCV.Checked = projectInfo.SendCVToClientContact;
            chkClientContactSA.Checked = projectInfo.SendSAToClientContact;
            chkClientContactPC.Checked = projectInfo.SendPCToClientContact;
            chkClientContactRFI.Checked = projectInfo.SendRFIToClientContact;
            chkClientContactEOT.Checked = projectInfo.SendEOTToClientContact;

            txtClientContact1FirstName.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.FirstName);
            txtClientContact1LastName.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.LastName);
            txtClientContact1CompanyName.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.CompanyName);
            txtClientContact1Street.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Street);
            txtClientContact1Locality.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Locality);
            txtClientContact1PostalCode.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.PostalCode);
            txtClientContact1Email.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Email);
            txtClientContact1Phone.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Phone);
            txtClientContact1Fax.Text = UI.Utils.SetFormString(projectInfo.ClientContact1.Fax);
            chkClientContact1CV.Checked = projectInfo.SendCVToClientContact1;
            chkClientContact1SA.Checked = projectInfo.SendSAToClientContact1;
            chkClientContact1PC.Checked = projectInfo.SendPCToClientContact1;
            chkClientContact1RFI.Checked = projectInfo.SendRFIToClientContact1;
            chkClientContact1EOT.Checked = projectInfo.SendEOTToClientContact1;

            txtClientContact2FirstName.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.FirstName);
            txtClientContact2LastName.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.LastName);
            txtClientContact2CompanyName.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.CompanyName);
            txtClientContact2Street.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Street);
            txtClientContact2Locality.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Locality);
            txtClientContact2PostalCode.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.PostalCode);
            txtClientContact2Email.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Email);
            txtClientContact2Phone.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Phone);
            txtClientContact2Fax.Text = UI.Utils.SetFormString(projectInfo.ClientContact2.Fax);
            chkClientContact2CV.Checked = projectInfo.SendCVToClientContact2;
            chkClientContact2SA.Checked = projectInfo.SendSAToClientContact2;
            chkClientContact2PC.Checked = projectInfo.SendPCToClientContact2;
            chkClientContact2RFI.Checked = projectInfo.SendRFIToClientContact2;
            chkClientContact2EOT.Checked = projectInfo.SendEOTToClientContact2;

            txtSIFirstName.Text = UI.Utils.SetFormString(projectInfo.Superintendent.FirstName);
            txtSILastName.Text = UI.Utils.SetFormString(projectInfo.Superintendent.LastName);
            txtSICompanyName.Text = UI.Utils.SetFormString(projectInfo.Superintendent.CompanyName);
            txtSIStreet.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Street);
            txtSILocality.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Locality);
            txtSIPostalCode.Text = UI.Utils.SetFormString(projectInfo.Superintendent.PostalCode);
            txtSIEmail.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Email);
            txtSIPhone.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Phone);
            txtSIFax.Text = UI.Utils.SetFormString(projectInfo.Superintendent.Fax);
            chkSICV.Checked = projectInfo.SendCVToSuperintendent;
            chkSISA.Checked = projectInfo.SendSAToSuperintendent;
            chkSIPC.Checked = projectInfo.SendPCToSuperintendent;
            chkSIRFI.Checked = projectInfo.SendRFIToSuperintendent;
            chkSIEOT.Checked = projectInfo.SendEOTToSuperintendent;

            txtQSFirstName.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.FirstName);
            txtQSLastName.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.LastName);
            txtQSCompanyName.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.CompanyName);
            txtQSStreet.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Street);
            txtQSLocality.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Locality);
            txtQSPostalCode.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.PostalCode);
            txtQSEmail.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Email);
            txtQSPhone.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Phone);
            txtQSFax.Text = UI.Utils.SetFormString(projectInfo.QuantitySurveyor.Fax);
            chkQSCV.Checked = projectInfo.SendCVToQuantitySurveyor;
            chkQSSA.Checked = projectInfo.SendSAToQuantitySurveyor;
            chkQSPC.Checked = projectInfo.SendPCToQuantitySurveyor;
            chkQSRFI.Checked = projectInfo.SendRFIToQuantitySurveyor;
            chkQSEOT.Checked = projectInfo.SendEOTToQuantitySurveyor;

            txtSecondPrincipalFirstName.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.FirstName);
            txtSecondPrincipalLastName.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.LastName);
            txtSecondPrincipalStreet.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Street);
            txtSecondPrincipalLocality.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Locality);
            txtSecondPrincipalPostalCode.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.PostalCode);
            txtSecondPrincipalEmail.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Email);
            txtSecondPrincipalPhone.Text = UI.Utils.SetFormString(projectInfo.SecondPrincipal.Phone);


            //#----------------------SITE ADDRESS

            Utils.GetConfigListAddEmpty("Global", "States", ddlSiteState, projectInfo.SiteState);
            txtSiteAddress.Text = UI.Utils.SetFormString(projectInfo.Siteaddress);
            txtSiteSuburb.Text = UI.Utils.SetFormString(projectInfo.SiteSuburb);
            txtSitePostalCode.Text = UI.Utils.SetFormString(projectInfo.SitePostalCode);

            //#----------------------SITE ADDRESS



            //#----------------------Principal ADDRESS

            Utils.GetConfigListAddEmpty("Global", "States", ddlPrState, projectInfo.PrincipalState);
            txtPrAddress.Text = UI.Utils.SetFormString(projectInfo.Principaladdress);
            txtPrSuburb.Text = UI.Utils.SetFormString(projectInfo.PrincipalSuburb);
            txtPrPostalcode.Text = UI.Utils.SetFormString(projectInfo.PrincipalPostalCode);

            //#----------------------Principal ADDRESS


            Utils.GetConfigListAddEmpty("Global", "States", ddlState, projectInfo.State);
            Utils.GetConfigListAddEmpty("Global", "ProjectStatus", ddlStatus, projectInfo.Status);
            Utils.GetConfigListAddEmpty("Global", "PaymentTerms", ddlPaymentTerms, projectInfo.PaymentTerms);
            Utils.GetConfigListAddEmpty("Global", "ClaimFrequency", ddlClaimFrequency, projectInfo.ClaimFrequency);

            Utils.GetConfigListCodeAddEmpty("Global", "States", ddlClientContactState, projectInfo.ClientContact.State);
            Utils.GetConfigListCodeAddEmpty("Global", "States", ddlClientContact1State, projectInfo.ClientContact1.State);
            Utils.GetConfigListCodeAddEmpty("Global", "States", ddlClientContact2State, projectInfo.ClientContact2.State);
            Utils.GetConfigListCodeAddEmpty("Global", "States", ddlSIState, projectInfo.Superintendent.State);
            Utils.GetConfigListCodeAddEmpty("Global", "States", ddlQSState, projectInfo.QuantitySurveyor.State);
            Utils.GetConfigListCodeAddEmpty("Global", "States", ddlSecondPrincipalState, projectInfo.SecondPrincipal.State);

            cmdSelGM.NavigateUrl = Utils.PopupPeople(this, txtGMId.ClientID, txtGM.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelCA.NavigateUrl = Utils.PopupPeople(this, txtCAId.ClientID, txtCA.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelPM.NavigateUrl = Utils.PopupPeople(this, txtPMId.ClientID, txtPM.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelCM.NavigateUrl = Utils.PopupPeople(this, txtCMId.ClientID, txtCM.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelDM.NavigateUrl = Utils.PopupPeople(this, txtDMId.ClientID, txtDM.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelDC.NavigateUrl = Utils.PopupPeople(this, txtDCId.ClientID, txtDC.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelFC.NavigateUrl = Utils.PopupPeople(this, txtFCId.ClientID, txtFC.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelDA.NavigateUrl = Utils.PopupPeople(this, txtDAId.ClientID, txtDA.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelBA.NavigateUrl = Utils.PopupPeople(this, txtBAId.ClientID, txtBA.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);

            //#---Added new role Commercial Manager and Junior Contracts Adminin
            cmdSelCO.NavigateUrl = Utils.PopupPeople(this, txtCOId.ClientID, txtCO.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            cmdSelJC.NavigateUrl = Utils.PopupPeople(this, txtJCId.ClientID, txtJC.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);

            CmdSelCA3.NavigateUrl = Utils.PopupPeople(this, TxtCA3Id.ClientID, TxtCA3.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            CmdSelCA4.NavigateUrl = Utils.PopupPeople(this, TxtCA4Id.ClientID, TxtCA4.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            CmdSelCA5.NavigateUrl = Utils.PopupPeople(this, TxtCA5Id.ClientID, TxtCA5.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);
            CmdSelCA6.NavigateUrl = Utils.PopupPeople(this, TxtCA6Id.ClientID, TxtCA6.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);





            //#---Added new role Commercial Manager
            cmdSelForeman.NavigateUrl = Utils.PopupPeople(this, txtForemanId.ClientID, txtForeman.ClientID, PeopleInfo.PeopleTypeEmployee, projectInfo.BusinessUnit);

            valDefectsLiability1.MaximumValue = Int32.MaxValue.ToString();
        }

        private void FormToObjects()
        {
            projectInfo.Name = UI.Utils.GetFormString(txtName.Text);
            projectInfo.Status = UI.Utils.GetFormString(ddlStatus.SelectedValue);
            projectInfo.Number = UI.Utils.GetFormString(txtNumber.Text);
            projectInfo.Year = UI.Utils.GetFormString(txtYear.Text);
            projectInfo.Fax = UI.Utils.GetFormString(txtFax.Text);
            projectInfo.DefectsLiability = UI.Utils.GetFormInteger(txtDefectsLiability.Text);
            projectInfo.LiquidatedDamages = UI.Utils.GetFormString(txtLiquidatedDamages.Text);
            projectInfo.SiteAllowances = UI.Utils.GetFormString(txtSiteAllowances.Text);
            projectInfo.Retention = UI.Utils.GetFormString(txtRetention.Text);
            projectInfo.RetentionToCertification = UI.Utils.GetFormString(txtRetentionToCertification.Text);
            projectInfo.RetentionToDLP = UI.Utils.GetFormString(txtRetentionToDLP.Text);
            projectInfo.Interest = UI.Utils.GetFormString(txtInterest.Text);
            projectInfo.Principal = UI.Utils.GetFormString(txtPrincipal.Text);
            projectInfo.PrincipalABN = UI.Utils.GetFormString(txtPrincipalABN.Text);
            projectInfo.Street = UI.Utils.GetFormString(txtStreet.Text);
            projectInfo.Locality = UI.Utils.GetFormString(txtLocality.Text);
            projectInfo.PostalCode = UI.Utils.GetFormString(txtPostalCode.Text);
            projectInfo.State = UI.Utils.GetFormString(ddlState.SelectedValue);
            projectInfo.Principal2 = UI.Utils.GetFormString(txtPrincipal2.Text);
            projectInfo.Principal2ABN = UI.Utils.GetFormString(txtPrincipal2ABN.Text);
            projectInfo.DeepZoomUrl = UI.Utils.GetFormString(txtDeepZoomUrl.Text);
            projectInfo.AttachmentsFolder = sfsAttachmentsFolder.FilePath;
            projectInfo.MaintenanceManualFile = sfsMaintenanceManualFile.FilePath;
            projectInfo.PostProjectReviewFile = sfsPostProjectReviewFile.FilePath;

            projectInfo.Description = UI.Utils.GetFormString(txtDescription.Text);
            projectInfo.SpecialClause = UI.Utils.GetFormString(txtSpecialClause.Text);

            //#-----SIte and Principla Address---San---

            projectInfo.Siteaddress = UI.Utils.GetFormString(txtSiteAddress.Text);
            projectInfo.SiteSuburb = UI.Utils.GetFormString(txtSiteSuburb.Text);
            projectInfo.SitePostalCode = UI.Utils.GetFormString(txtSitePostalCode.Text);
            projectInfo.SiteState = UI.Utils.GetFormString(ddlSiteState.SelectedValue);

            projectInfo.Principaladdress = UI.Utils.GetFormString(txtPrAddress.Text);
            projectInfo.PrincipalSuburb = UI.Utils.GetFormString(txtPrSuburb.Text);
            projectInfo.PrincipalPostalCode = UI.Utils.GetFormString(txtPrPostalcode.Text);
            projectInfo.PrincipalState = UI.Utils.GetFormString(ddlPrState.SelectedValue);



            //#-----SIte Address and Principla---San---

            //#---
            projectInfo.LawOfSubcontract = UI.Utils.GetFormString(txtLawofSubcontract.Text);

            projectInfo.AccountName = UI.Utils.GetFormString(TxtAccountName.Text);
            projectInfo.BSB = UI.Utils.GetFormString(TxtBSB.Text);
            projectInfo.AccountNumber = UI.Utils.GetFormString(TxtAccountNumber.Text);

            //#--

            projectInfo.CommencementDate = sdrCommencementDate.Date;
            projectInfo.CompletionDate = sdrCompletionDate.Date;
            projectInfo.ContractAmount = UI.Utils.GetFormDecimal(txtContractAmount.Text);
            projectInfo.PaymentTerms = UI.Utils.GetFormString(ddlPaymentTerms.SelectedValue);
            projectInfo.ClaimFrequency = UI.Utils.GetFormString(ddlClaimFrequency.SelectedValue);
            projectInfo.FirstClaimDueDate = sdrFirstClaimDueDate.Date;
            projectInfo.Waranty1Amount = UI.Utils.GetFormDecimal(txtWaranty1Amount.Text);
            projectInfo.Waranty1Date = sdrWaranty1Date.Date;
            projectInfo.Waranty2Amount = UI.Utils.GetFormDecimal(txtWaranty2Amount.Text);
            projectInfo.Waranty2Date = sdrWaranty2Date.Date;
            projectInfo.PracticalCompletionDate = sdrPracticalCompletionDate.Date;

            projectInfo.ClientContact.FirstName = UI.Utils.GetFormString(txtClientContactFirstName.Text);
            projectInfo.ClientContact.LastName = UI.Utils.GetFormString(txtClientContactLastName.Text);
            projectInfo.ClientContact.CompanyName = UI.Utils.GetFormString(txtClientContactCompanyName.Text);
            projectInfo.ClientContact.Street = UI.Utils.GetFormString(txtClientContactStreet.Text);
            projectInfo.ClientContact.Locality = UI.Utils.GetFormString(txtClientContactLocality.Text);
            projectInfo.ClientContact.State = UI.Utils.GetFormString(ddlClientContactState.SelectedValue);
            projectInfo.ClientContact.PostalCode = UI.Utils.GetFormString(txtClientContactPostalCode.Text);
            projectInfo.ClientContact.Email = UI.Utils.GetFormString(txtClientContactEmail.Text);
            projectInfo.ClientContact.Phone = UI.Utils.GetFormString(txtClientContactPhone.Text);
            projectInfo.ClientContact.Fax = UI.Utils.GetFormString(txtClientContactFax.Text);

            projectInfo.ClientContact1.FirstName = UI.Utils.GetFormString(txtClientContact1FirstName.Text);
            projectInfo.ClientContact1.LastName = UI.Utils.GetFormString(txtClientContact1LastName.Text);
            projectInfo.ClientContact1.CompanyName = UI.Utils.GetFormString(txtClientContact1CompanyName.Text);
            projectInfo.ClientContact1.Street = UI.Utils.GetFormString(txtClientContact1Street.Text);
            projectInfo.ClientContact1.Locality = UI.Utils.GetFormString(txtClientContact1Locality.Text);
            projectInfo.ClientContact1.State = UI.Utils.GetFormString(ddlClientContact1State.SelectedValue);
            projectInfo.ClientContact1.PostalCode = UI.Utils.GetFormString(txtClientContact1PostalCode.Text);
            projectInfo.ClientContact1.Email = UI.Utils.GetFormString(txtClientContact1Email.Text);
            projectInfo.ClientContact1.Phone = UI.Utils.GetFormString(txtClientContact1Phone.Text);
            projectInfo.ClientContact1.Fax = UI.Utils.GetFormString(txtClientContact1Fax.Text);

            projectInfo.ClientContact2.FirstName = UI.Utils.GetFormString(txtClientContact2FirstName.Text);
            projectInfo.ClientContact2.LastName = UI.Utils.GetFormString(txtClientContact2LastName.Text);
            projectInfo.ClientContact2.CompanyName = UI.Utils.GetFormString(txtClientContact2CompanyName.Text);
            projectInfo.ClientContact2.Street = UI.Utils.GetFormString(txtClientContact2Street.Text);
            projectInfo.ClientContact2.Locality = UI.Utils.GetFormString(txtClientContact2Locality.Text);
            projectInfo.ClientContact2.State = UI.Utils.GetFormString(ddlClientContact2State.SelectedValue);
            projectInfo.ClientContact2.PostalCode = UI.Utils.GetFormString(txtClientContact2PostalCode.Text);
            projectInfo.ClientContact2.Email = UI.Utils.GetFormString(txtClientContact2Email.Text);
            projectInfo.ClientContact2.Phone = UI.Utils.GetFormString(txtClientContact2Phone.Text);
            projectInfo.ClientContact2.Fax = UI.Utils.GetFormString(txtClientContact2Fax.Text);
            projectInfo.ClientContact2.Phone = UI.Utils.GetFormString(txtClientContact2Phone.Text);
            projectInfo.ClientContact2.Fax = UI.Utils.GetFormString(txtClientContact2Fax.Text);

            projectInfo.Superintendent.FirstName = UI.Utils.GetFormString(txtSIFirstName.Text);
            projectInfo.Superintendent.LastName = UI.Utils.GetFormString(txtSILastName.Text);
            projectInfo.Superintendent.CompanyName = UI.Utils.GetFormString(txtSICompanyName.Text);
            projectInfo.Superintendent.Street = UI.Utils.GetFormString(txtSIStreet.Text);
            projectInfo.Superintendent.Locality = UI.Utils.GetFormString(txtSILocality.Text);
            projectInfo.Superintendent.State = UI.Utils.GetFormString(ddlSIState.SelectedValue);
            projectInfo.Superintendent.PostalCode = UI.Utils.GetFormString(txtSIPostalCode.Text);
            projectInfo.Superintendent.Email = UI.Utils.GetFormString(txtSIEmail.Text);
            projectInfo.Superintendent.Phone = UI.Utils.GetFormString(txtSIPhone.Text);
            projectInfo.Superintendent.Fax = UI.Utils.GetFormString(txtSIFax.Text);

            projectInfo.QuantitySurveyor.FirstName = UI.Utils.GetFormString(txtQSFirstName.Text);
            projectInfo.QuantitySurveyor.LastName = UI.Utils.GetFormString(txtQSLastName.Text);
            projectInfo.QuantitySurveyor.CompanyName = UI.Utils.GetFormString(txtQSCompanyName.Text);
            projectInfo.QuantitySurveyor.Street = UI.Utils.GetFormString(txtQSStreet.Text);
            projectInfo.QuantitySurveyor.Locality = UI.Utils.GetFormString(txtQSLocality.Text);
            projectInfo.QuantitySurveyor.State = UI.Utils.GetFormString(ddlQSState.SelectedValue);
            projectInfo.QuantitySurveyor.PostalCode = UI.Utils.GetFormString(txtQSPostalCode.Text);
            projectInfo.QuantitySurveyor.Email = UI.Utils.GetFormString(txtQSEmail.Text);
            projectInfo.QuantitySurveyor.Phone = UI.Utils.GetFormString(txtQSPhone.Text);
            projectInfo.QuantitySurveyor.Fax = UI.Utils.GetFormString(txtQSFax.Text);

            projectInfo.SecondPrincipal.FirstName = UI.Utils.GetFormString(txtSecondPrincipalFirstName.Text);
            projectInfo.SecondPrincipal.LastName = UI.Utils.GetFormString(txtSecondPrincipalLastName.Text);
            projectInfo.SecondPrincipal.Street = UI.Utils.GetFormString(txtSecondPrincipalStreet.Text);
            projectInfo.SecondPrincipal.Locality = UI.Utils.GetFormString(txtSecondPrincipalLocality.Text);
            projectInfo.SecondPrincipal.State = UI.Utils.GetFormString(ddlSecondPrincipalState.SelectedValue);
            projectInfo.SecondPrincipal.PostalCode = UI.Utils.GetFormString(txtSecondPrincipalPostalCode.Text);
            projectInfo.SecondPrincipal.Email = UI.Utils.GetFormString(txtSecondPrincipalEmail.Text);
            projectInfo.SecondPrincipal.Phone = UI.Utils.GetFormString(txtSecondPrincipalPhone.Text);

            Char[] c = {
                (Char)((Int32)(chkClientContactCV.Checked ? 16 : 0) + (Int32)(chkClientContactSA.Checked ? 8 : 0) + (Int32)(chkClientContactPC.Checked ? 4 : 0) + (Int32)(chkClientContactRFI.Checked ? 2 : 0) + (Int32)(chkClientContactEOT.Checked ? 1 : 0)),
                (Char)((Int32)(chkClientContact1CV.Checked ? 16 : 0) + (Int32)(chkClientContact1SA.Checked ? 8 : 0) + (Int32)(chkClientContact1PC.Checked ? 4 : 0) + (Int32)(chkClientContact1RFI.Checked ? 2 : 0) + (Int32)(chkClientContact1EOT.Checked ? 1 : 0)),
                (Char)((Int32)(chkClientContact2CV.Checked ? 16 : 0) + (Int32)(chkClientContact2SA.Checked ? 8 : 0) + (Int32)(chkClientContact2PC.Checked ? 4 : 0) + (Int32)(chkClientContact2RFI.Checked ? 2 : 0) + (Int32)(chkClientContact2EOT.Checked ? 1 : 0)),
                (Char)((Int32)(chkSICV.Checked ? 16 : 0) + (Int32)(chkSISA.Checked ? 8 : 0) + (Int32)(chkSIPC.Checked ? 4 : 0) + (Int32)(chkSIRFI.Checked ? 2 : 0) + (Int32)(chkSIEOT.Checked ? 1 : 0)),
                (Char)((Int32)(chkQSCV.Checked ? 16 : 0) + (Int32)(chkQSSA.Checked ? 8 : 0) + (Int32)(chkQSPC.Checked ? 4 : 0) + (Int32)(chkQSRFI.Checked ? 2 : 0) + (Int32)(chkQSEOT.Checked ? 1 : 0))
            };

            projectInfo.DistributionListInfo = new String(c);

            projectInfo.BusinessUnit = ddlBusinessUnit.SelectedValue != "" ? new BusinessUnitInfo(Convert.ToInt32(ddlBusinessUnit.SelectedValue)) : null;

            projectInfo.ManagingDirector = txtGMId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtGMId.Value)) : null;
            projectInfo.ContractsAdministrator = txtCAId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtCAId.Value)) : null;
            projectInfo.ProjectManager = txtPMId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtPMId.Value)) : null;
            projectInfo.ConstructionManager = txtCMId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtCMId.Value)) : null;
            projectInfo.DesignManager = txtDMId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtDMId.Value)) : null;
            projectInfo.DesignCoordinator = txtDCId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtDCId.Value)) : null;
            projectInfo.FinancialController = txtFCId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtFCId.Value)) : null;
            projectInfo.DirectorAuthorization = txtDAId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtDAId.Value)) : null;
            projectInfo.BudgetAdministrator = txtBAId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtBAId.Value)) : null;
            //#-----New role Commercial Manger and Junior Contracts Administrator is added
            projectInfo.CommercialManager = txtCOId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtCOId.Value)) : null;
            projectInfo.JuniorContractsAdministrator = txtJCId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtJCId.Value)) : null;

            projectInfo.ContractsAdministrator3 = TxtCA3Id.Value != "" ? new EmployeeInfo(Convert.ToInt32(TxtCA3Id.Value)) : null;
            projectInfo.ContractsAdministrator4 = TxtCA4Id.Value != "" ? new EmployeeInfo(Convert.ToInt32(TxtCA4Id.Value)) : null;
            projectInfo.ContractsAdministrator5 = TxtCA5Id.Value != "" ? new EmployeeInfo(Convert.ToInt32(TxtCA5Id.Value)) : null;
            projectInfo.ContractsAdministrator6 = TxtCA6Id.Value != "" ? new EmployeeInfo(Convert.ToInt32(TxtCA6Id.Value)) : null;




            //#---
            projectInfo.Foreman = txtForemanId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtForemanId.Value)) : null;



        }

        private void CreateClientScripts()
        {
            pnlForm.Visible = false;
            pnlCreateInfo.Visible = true;

            ClientScriptManager clientScriptManager = Page.ClientScript;
            String callbackScript = @"function CallServer() {" + clientScriptManager.GetCallbackEventReference(this, "", "ReceiveServerData", "") + "}";
            String receiveServerDataScript = @"function ReceiveServerData(eventArgument, context) { ReceiveServerDataWithConfigutation(eventArgument, context, '" + ProcessStatus.END_PROCESS_CODE + "', '" + ProcessStatus.DETAILS_SEPARATOR + "', " + ProcessStatus.POLLING_INTERVAL.ToString() + ", " + ProcessStatus.REDIRECT_TIME.ToString() + ", 'txtProgress',  'divRedirect'); }";
            String startupScript = "CallServer();" + Environment.NewLine;

            clientScriptManager.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            clientScriptManager.RegisterClientScriptBlock(this.GetType(), "ReceiveServerData", receiveServerDataScript, true);
            clientScriptManager.RegisterStartupScript(this.GetType(), "CallServerFirstCall", startupScript, true);
        }

        private SOS.Core.ProcessStatus CheckProcessStatus()
        {
            ProcessStatus processStatus = null;

            if (Session["MethodCallInfo"] != null)
            {
                IAsyncResult iAsyncResult = (IAsyncResult)Session["MethodCallInfo"];
                ProcessStatus runningProcessStatus = (ProcessStatus)iAsyncResult.AsyncState;

                if (iAsyncResult.IsCompleted)
                {
                    processStatus = runningProcessStatus.GetStatusUpdate();

                    try
                    {
                        projectInfo.Id = ProjectsController.GetInstance().EndAddProject(iAsyncResult);
                        processStatus.AddProcessStatusInfo(ProcessStatus.END_PROCESS_CODE + Utils.AbsoluteURL("Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr));
                    }
                    catch (Exception)
                    {
                        processStatus.AddProcessStatusInfo(ProcessStatus.END_PROCESS_CODE + Utils.AbsoluteURL("Modules/Projects/SearchProjects.aspx"));
                    }
                    finally
                    {
                        Session["MethodCallInfo"] = null;
                        processStatus.IsComplete = true;
                    }
                }
                else
                {
                    processStatus = runningProcessStatus.GetStatusUpdate();
                }
            }

            return processStatus;
        }
        #endregion

        #region Public Methods
        public String GetCallbackResult()
        {
            ProcessStatus processStatus = CheckProcessStatus();

            if (processStatus != null)
                return processStatus.ProcessDetails;
            else
                return ProcessStatus.END_PROCESS_CODE;
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            // Nothing to do
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterProjectId = Request.Params["ProjectId"];

            try
            {
                if (parameterProjectId == null)
                {
                    Security.CheckAccess(Security.userActions.CreateProject);

                    projectInfo = new ProjectInfo();

                    projectInfo.ClientContact = new ClientContactInfo();
                    projectInfo.ClientContact1 = new ClientContactInfo();
                    projectInfo.ClientContact2 = new ClientContactInfo();
                    projectInfo.Superintendent = new ClientContactInfo();
                    projectInfo.QuantitySurveyor = new ClientContactInfo();
                    projectInfo.SecondPrincipal = new ClientContactInfo();
                }
                else
                {
                    Security.CheckAccess(Security.userActions.EditProject);

                    projectInfo = ProjectsController.GetInstance().GetProject(Int32.Parse(parameterProjectId));
                    Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");
                    if (!ProcessController.GetInstance().AllowEditCurrentUser(projectInfo))
                        throw new SecurityException();
                }

                if (!Page.IsPostBack)
                    ObjectsToForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();

                    if (projectInfo.Id != null)
                    {
                        projectsController.UpdateProject(projectInfo);
                        Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr, true);
                    }
                    else
                    {
                        IAsyncResult iAsyncResult = projectsController.StartAddProject(projectInfo);
                        Session["MethodCallInfo"] = iAsyncResult;

                        CreateClientScripts();
                    }
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (projectInfo.Id == null)
                Response.Redirect("~/Modules/Projects/SearchProjects.aspx");
            else
                Response.Redirect("~/Modules/Projects/ViewProject.aspx?ProjectId=" + projectInfo.IdStr);
        }
        #endregion

    }
}