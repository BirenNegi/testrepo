using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditSubContractorPage : System.Web.UI.Page
    {

#region Members
        private SubContractorInfo subContractorInfo = null;
#endregion
        
#region Private Methods
        private void ObjectsToForm()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            //#---To sort BZ units
            businessUnitInfoList.Sort((x, y) => x.Name.CompareTo(y.Name));
            //#---

            ddlBusinessUnit.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList) { 
                ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));
                //#--
                ddlBusinessUnitList.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));    //#--

              }



            if (subContractorInfo.Id == null)
            {
                TitleBar.Title = "Adding Subcontractor";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Subcontractor";
            }

            txtName.Text = UI.Utils.SetFormString(subContractorInfo.Name);
            txtShortName.Text = UI.Utils.SetFormString(subContractorInfo.ShortName);
            txtStreet.Text = UI.Utils.SetFormString(subContractorInfo.Street);
            txtLocality.Text = UI.Utils.SetFormString(subContractorInfo.Locality);
            txtPostalCode.Text = UI.Utils.SetFormString(subContractorInfo.PostalCode);
            txtPhone.Text = UI.Utils.SetFormString(subContractorInfo.Phone);
            txtFax.Text = UI.Utils.SetFormString(subContractorInfo.Fax);
            txtAbn.Text = UI.Utils.SetFormString(subContractorInfo.Abn);
            txtAccount.Text = UI.Utils.SetFormString(subContractorInfo.Account);
            txtWebsite.Text = UI.Utils.SetFormString(subContractorInfo.Website);

            //#---
            txtACN.Text= UI.Utils.SetFormString(subContractorInfo.ACN);
            txtLicenceNumber.Text= UI.Utils.SetFormString(subContractorInfo.LicenceNumber);

            if (UI.Utils.SetFormString(subContractorInfo.PrequalifiedForm).Length>0)
            {
                lblPrequalification.Text = (UI.Utils.SetFormString(subContractorInfo.PrequalifiedForm));
                FileUpload1.Width=86;
               // string Fpath = UI.Utils.SetFormString(subContractorInfo.PrequalifiedForm);
               // Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "AddvalueControl('" + Fpath + "');", true);
            }
            // sfsMaintenanceManualFile.FilePath=UI.Utils.SetFormString(subContractorInfo.PrequalifiedForm);

            if (UI.Utils.SetFormString(subContractorInfo.PublicLiabilityInsurance).Length > 0)
            {
                lblPublicliability.Text = (UI.Utils.SetFormString(subContractorInfo.PublicLiabilityInsurance));
                FuPublicliability.Width = 86;
            }

            if (subContractorInfo.DCContractor!=null && subContractorInfo.DCContractor.Value == true)
                chkDc.Checked = true; 
            else
                chkDc.Checked = false;


            if (UI.Utils.SetFormString(subContractorInfo.WorkCoverInsurance).Length > 0)
            {
                lblWorkcover.Text = (UI.Utils.SetFormString(subContractorInfo.WorkCoverInsurance));
                FuWorkcover.Width = 86;
            }


            if (UI.Utils.SetFormString(subContractorInfo.ProfessionalIndemnityInsurance).Length > 0)
            {
                lblProfessionalIndemnity.Text = (UI.Utils.SetFormString(subContractorInfo.ProfessionalIndemnityInsurance));
                FuPIinsurance.Width = 86;
            }

            //#---

            txtComments.Text = UI.Utils.SetFormString(subContractorInfo.Comments);

            if (subContractorInfo.BusinessUnit != null)
                ddlBusinessUnit.SelectedValue = subContractorInfo.BusinessUnit.IdStr;

            Utils.GetConfigListAddEmpty("Global", "States", ddlState, subContractorInfo.State);


            //#---
            if (subContractorInfo.BusinessUnitslist != null)
                foreach (BusinessUnitInfo bUnit in subContractorInfo.BusinessUnitslist)
                {
                    for(int i=0;i<=ddlBusinessUnitList.Items.Count;i++)
                    {
                        if (bUnit.IdStr == ddlBusinessUnitList.Items[i].Value)
                        {ddlBusinessUnitList.Items[i].Selected = true;
                        break;
                        }
                    }

                }

            //#---



        }

        private void FormToObjects()
        {
            subContractorInfo.Name = UI.Utils.GetFormString(txtName.Text);
            subContractorInfo.ShortName = UI.Utils.GetFormString(txtShortName.Text);
            subContractorInfo.Street = UI.Utils.GetFormString(txtStreet.Text);
            subContractorInfo.Locality = UI.Utils.GetFormString(txtLocality.Text);
            subContractorInfo.PostalCode = UI.Utils.GetFormString(txtPostalCode.Text);
            subContractorInfo.Phone = UI.Utils.GetFormString(txtPhone.Text);
            subContractorInfo.Fax = UI.Utils.GetFormString(txtFax.Text);
            subContractorInfo.Abn = UI.Utils.GetFormString(txtAbn.Text);
            subContractorInfo.Account = UI.Utils.GetFormString(txtAccount.Text);
            subContractorInfo.Website = UI.Utils.GetFormString(txtWebsite.Text);
            //#---

            subContractorInfo.ACN = UI.Utils.GetFormString(txtACN.Text);
            subContractorInfo.LicenceNumber = UI.Utils.GetFormString(txtLicenceNumber.Text);

            if (FileUpload1.FileName.Length > 0) {

                string fpath = FileUpload1.PostedFile.FileName;
                ////fpath = System.IO.Path.GetDirectoryName(FileUpload1.PostedFile.FileName);
                subContractorInfo.PrequalifiedForm = fpath;      //System.IO.Path.GetFullPath(FileUpload1.FileName);    //sfsMaintenanceManualFile.FilePath; 
            }

            if (FuPublicliability.FileName.Length > 0)
            {
                string Ppath = FuPublicliability.PostedFile.FileName;
                subContractorInfo.PublicLiabilityInsurance = Ppath;
            }

            if (FuWorkcover.FileName.Length > 0)
            {
                string Wpath = FuWorkcover.PostedFile.FileName;
                subContractorInfo.WorkCoverInsurance = Wpath;
            }

            if (chkDc.Checked)
            { subContractorInfo.DCContractor = true; }
            else 
            { subContractorInfo.DCContractor = false; }

            if (FuPIinsurance.FileName.Length > 0 && chkDc.Checked)
            {
                string Ppath = FuPIinsurance.PostedFile.FileName;
                subContractorInfo.ProfessionalIndemnityInsurance = Ppath;
            }

            subContractorInfo.BusinessUnitslist = new List<BusinessUnitInfo>();
            foreach (ListItem item in ddlBusinessUnitList.Items)
                if (item.Selected == true)
                    subContractorInfo.BusinessUnitslist.Add(ContractsController.GetInstance().GetBusinessUnit(Convert.ToInt32(item.Value)));

            //#---

            subContractorInfo.Comments = UI.Utils.GetFormString(txtComments.Text);

            subContractorInfo.BusinessUnit = ddlBusinessUnit.SelectedValue != "" ? new BusinessUnitInfo(Convert.ToInt32(ddlBusinessUnit.SelectedValue)) : null;

            subContractorInfo.State = UI.Utils.GetFormString(ddlState.SelectedValue);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {

            FileUpload1.Attributes.Add("onchange", "PreQualificationchange('MainContent_FileUpload1');");//onclick
            FuPublicliability.Attributes.Add("onchange", "PreQualificationchange('MainContent_FuPublicliability');");
            FuWorkcover.Attributes.Add("onchange", "PreQualificationchange('MainContent_FuWorkcover');");

            String parameterSubContractorId = Request.Params["SubContractorId"];

            try {
                Security.CheckAccess(Security.userActions.EditSubContractor);

                if (parameterSubContractorId == null)
                {
                    subContractorInfo = new SubContractorInfo();
                }
                else
                {
                    subContractorInfo = SubContractorsController.GetInstance().GetSubContractor(Int32.Parse(parameterSubContractorId));
                    Core.Utils.CheckNullObject(subContractorInfo, parameterSubContractorId, "SubContractor");
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
            try {
                if (Page.IsValid)
                {
                    FormToObjects();
                    subContractorInfo.Id = SubContractorsController.GetInstance().AddUpdateSubContractor(subContractorInfo);
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/SubContractors/ViewSubContractor.aspx?SubContractorId=" + subContractorInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            if (subContractorInfo.Id == null)
            {
                Response.Redirect("~/Modules/SubContractors/SearchSubContractors.aspx");
            }
            else
            {
                Response.Redirect("~/Modules/SubContractors/ViewSubContractor.aspx?SubContractorId=" + subContractorInfo.IdStr);
            }
        }
        #endregion

        
    }
}