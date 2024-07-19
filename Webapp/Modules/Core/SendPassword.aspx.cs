using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class SendPasswordPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void butEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmail.Text.Trim() == String.Empty)
                {
                    lblError.Text = "Required field!<br />";
                    lblError.Visible = true;
                }
                else
                {
                    PeopleInfo peopleInfo = PeopleController.GetInstance().GetPersonByEmail(txtEmail.Text.Trim());
                    if (peopleInfo == null)
                    {
                        lblError.Text = "Email address not found!<br />";
                        lblError.Visible = true;
                    }
                    else
                    {
                        Core.Utils.SendPasswordReminder(peopleInfo);
                        pnlForm.Visible = false;
                        pnlMessage.Visible = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                Core.Utils.LogError(Ex.ToString());
                lblError.Text = "Error sending email. Please try again later.<br />";
                lblError.Visible = true;
            }
        }
#endregion

    }
}