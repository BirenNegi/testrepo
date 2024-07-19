using System;
using System.Web.UI.WebControls;
using System.Web;
using SOS.Core;

namespace SOS.Web
{
    public partial class UserMasterPage : System.Web.UI.MasterPage
    {

#region Members
        private EmployeeInfo employeeInfo = null;
#endregion
        
#region Private Methods
        private void BindForm() 
        {
            if (Utils.GetCurrentUser() != null)
            {
                lnkUser.Text = Utils.GetCurrentUser().Name;
                lnkUser.NavigateUrl = "~/Modules/Core/ChangePassword.aspx";

                if (employeeInfo.UserType == EmployeeInfo.TypeAdmin)
                    menuMain.Items[4].ChildItems.Add(new MenuItem("Files Links", "Files Links", "", "~/Modules/Projects/UpdateFilesLinks.aspx"));
            }

            if (SiteMap.CurrentNode != null)
                switch (SiteMap.CurrentNode.ResourceKey)
                {
                    case "Projects": menuMain.Items[0].Selected = true; break;
                    case "People": menuMain.Items[1].Selected = true; break;
                    case "Subcontractors": menuMain.Items[2].Selected = true; break;
                    case "Reports": menuMain.Items[3].Selected = true; break;
                    case "Admin": menuMain.Items[4].Selected = true; break;
                    case "Help": menuMain.Items[5].Selected = true; break;
                }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, System.EventArgs e)
        {
            employeeInfo = (EmployeeInfo)Utils.GetCurrentUser();

            if (!IsPostBack)
                BindForm();
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
			Utils.LogOutUser();
            Response.Redirect("~/Modules/Core/Login.aspx");
        }
#endregion

    }
}