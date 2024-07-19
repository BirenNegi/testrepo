using System;
using System.Web.UI.WebControls;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class SubbieMasterPage : System.Web.UI.MasterPage
    {

#region Event Handlers
        protected void Page_Load(object sender, System.EventArgs e)
        {
            ContactInfo contactInfo = (ContactInfo)Utils.GetCurrentUser();
            if (Utils.GetCurrentUser() != null)
            {
                lnkUser.Text = contactInfo.Name;
                lnkUser.NavigateUrl = "~/Modules/Core/ChangePassword.aspx";
                lblSubcontractor.Text = contactInfo.SubContractorName;
            }

            if (SiteMap.CurrentNode != null)
                switch (SiteMap.CurrentNode.ResourceKey)
                {
                    case "Projects": menuMain.Items[0].Selected = true; break;
                    case "Help": menuMain.Items[1].Selected = true; break;
                }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
			Utils.LogOutUser();
			Response.Redirect("~/Modules/Core/Login.aspx");
        }
    }
#endregion

}