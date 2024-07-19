using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewSubContractorPage : System.Web.UI.Page
    {

#region Members
        private SubContractorInfo subContractorInfo = null;
#endregion

#region Private Methods
        private void bindSubContractor()
        {
            lblName.Text = UI.Utils.SetFormString(subContractorInfo.Name);
            lblBusiniessUnit.Text = UI.Utils.SetFormString(subContractorInfo.BusinessUnitName);
            lblShortName.Text = UI.Utils.SetFormString(subContractorInfo.ShortName);
            lblStreet.Text = UI.Utils.SetFormString(subContractorInfo.Street);
            lblLocality.Text = UI.Utils.SetFormString(subContractorInfo.Locality);
            lblState.Text = UI.Utils.SetFormString(subContractorInfo.State);
            lblPostalCode.Text = UI.Utils.SetFormString(subContractorInfo.PostalCode);
            lblPhone.Text = UI.Utils.SetFormString(subContractorInfo.Phone);
            lblFax.Text = UI.Utils.SetFormString(subContractorInfo.Fax);
            lblAbn.Text = UI.Utils.SetFormString(subContractorInfo.Abn);
            lblAccount.Text = UI.Utils.SetFormString(subContractorInfo.Account);

            //#---
            lblACN.Text = UI.Utils.SetFormString(subContractorInfo.ACN);
            lblLicenceNumber.Text = UI.Utils.SetFormString(subContractorInfo.LicenceNumber);
            lblPrequalification.Text= UI.Utils.SetFormString(subContractorInfo.PrequalifiedForm);
            lblPublicliabilityInsurance.Text= UI.Utils.SetFormString(subContractorInfo.PublicLiabilityInsurance);
            lblWorkCoverInsurance.Text= UI.Utils.SetFormString(subContractorInfo.WorkCoverInsurance);

            if (subContractorInfo.DCContractor!=null && subContractorInfo.DCContractor.Value == true)
                chkDC.Checked = true;
            else
                chkDC.Checked = false;

            lblProfessionalIndemnity.Text= UI.Utils.SetFormString(subContractorInfo.ProfessionalIndemnityInsurance);
            //#--

            lblComments.Text = UI.Utils.SetFormString(subContractorInfo.Comments);

            lnkWebsite.Text = UI.Utils.SetFormString(subContractorInfo.Website);
            lnkWebsite.NavigateUrl = UI.Utils.SetFormURL(subContractorInfo.Website);
            lnkWebsite.Target = "_blank";

           //#----
            if (subContractorInfo.BusinessUnitslist.Count > 0)
            {
                String OutherUnits =String.Empty;
               foreach (BusinessUnitInfo bunitInfo in subContractorInfo.BusinessUnitslist)
                {
                    OutherUnits += bunitInfo.Name + ",";
                }

                lblBusinesUnitList.Text = OutherUnits;
            }
            //#----
            GridViewContacts.DataSource = subContractorInfo.Contacts.FindAll(x=>x.UserType!="SE");
            GridViewContacts.DataBind();
            //#----
            GridViewEmployees.DataSource = subContractorInfo.Contacts.FindAll(x => x.UserType == "SE");
            GridViewEmployees.DataBind();
            //#----
        }
        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterSubContractorId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewSubContractor);
                parameterSubContractorId = Utils.CheckParameter("SubContractorId");
                subContractorInfo = SubContractorsController.GetInstance().GetSubContractorDeep(Int32.Parse(parameterSubContractorId));
                Core.Utils.CheckNullObject(subContractorInfo, parameterSubContractorId, "SubContractor");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditSubContractor))
                    {
                        cmdEditTop.Visible = true;
                        cmdDeleteTop.Visible = true;

                        cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Subcontractor?');");
                    }
                    bindSubContractor();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/SubContractors/EditSubContractor.aspx?SubContractorId=" + subContractorInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SubContractorsController.GetInstance().DeleteSubContractor(subContractorInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/SubContractors/SearchSubContractors.aspx");
        }
#endregion

    }
}