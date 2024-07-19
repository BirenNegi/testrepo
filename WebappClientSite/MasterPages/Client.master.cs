using System;
using System.Web.UI.WebControls;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class ClientMasterPage : System.Web.UI.MasterPage
    {

#region Event Handlers
        protected void Page_Load(object sender, System.EventArgs e)
        {   string ProjectId = Request.QueryString["ProjectId"];
            if (!IsPostBack)
            { 
            
            ClientContactInfo clientContactInfo = (ClientContactInfo)Utils.GetCurrentUser();
                    if (Utils.GetCurrentUser() != null)
                    {
                        lnkUser.Text = clientContactInfo.Name.ToUpper();
                        lnkUser.NavigateUrl = "~/Modules/Core/ChangePassword.aspx";
                       // lblSubcontractor.Text = clientContactInfo.Name;
                    }
            }
            if (SiteMap.CurrentNode != null)
                switch (SiteMap.CurrentNode.ResourceKey)
                {
                    case "Projects": menuMain.Items[0].Selected = true;
                        menuMain.Items[0].NavigateUrl = "~/Modules/Projects/ClientProjectDetails.aspx?ProjectId="+ ProjectId;
                        break;
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