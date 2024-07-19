using System;
using System.Web.UI;

namespace SOS.Web
{
    public partial class ErrorPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDescription.Text = Session["LastError"].ToString();
        }
    }
}