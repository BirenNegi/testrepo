using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;

using ProcessInfo = SOS.Core.ProcessInfo;
namespace SOS.Web
{
    public partial class LoginPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)            
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["Inf"] == "SesErr")
                {
                    phSessionError.Visible = true;
                }
                else
                {
                    if (Request.Params["Inf"] == "SecErr")
                    {
                        phSecurityError.Visible = true;
                    }
                }
            }
        }

        protected void butLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                PeopleInfo peopleInfo = PeopleController.GetInstance().GetPerson(null, txtLogin.Text, null);

                if (peopleInfo == null)
                {
                    lblMessage.Text = "Invalid User Name or Password!";
                    lblMessage.Visible = true;
                }
                else
                {
                    if (peopleInfo.Password != txtPassword.Text)
                    {
                        lblMessage.Text = "Invalid User Name or Password!";
                        lblMessage.Visible = true;
                    }
                    else
                    {
                        lblMessage.Visible = false;

                        try
                        {
							Utils.LogInUser(peopleInfo);
                            Response.Redirect("~/Modules/Core/Home.aspx");
                        }
                        catch (Exception Ex)
                        {
                            lblMessage.Text = Ex.ToString();
                            lblMessage.Visible = true;
                        }
                    }
                }
            }
        }
#endregion

    }
}
