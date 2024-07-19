using System;
using System.Web;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewHelpPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void BindForm()
        {
            cpeIntro.Collapsed = true;
            cpeLogin.Collapsed = true;
            cpeNavigation.Collapsed = true;
            cpeSubcontractors.Collapsed = true;
            cpeStaff.Collapsed = true;
            cpeTaskList.Collapsed = true;
            cpeProject.Collapsed = true;
            cpeManageProjects.Collapsed = true;
            cpeContractManager.Collapsed = true;
            cpePurchasingSchedule.Collapsed = true;
            cpeDrawingRegister.Collapsed = true;
            cpeTransmittals.Collapsed = true;
            cpeProjectTrade.Collapsed = true;
            cpeItemCategories.Collapsed = true;
            cpeApproval.Collapsed = true;
            cpeSubbies.Collapsed = true;
            cpeDrawingTypes.Collapsed = true;
            cpeDrawings.Collapsed = true;
            cpeComparison.Collapsed = true;
            cpeVariations.Collapsed = true;
            cpeAdmin.Collapsed = true;
            cpeTradeTemplates.Collapsed = true;
            cpeContractTemplates.Collapsed = true;
            cpeBusinessUnits.Collapsed = true;
            cpeInstallation.Collapsed = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewHelp);

                if (!Page.IsPostBack)
                {
                    BindForm();
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