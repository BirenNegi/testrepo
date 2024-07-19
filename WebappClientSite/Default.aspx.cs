using System;


namespace SOS.Web
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           Response.Redirect("~/Modules/Core/Login.aspx");
        }
    }
}