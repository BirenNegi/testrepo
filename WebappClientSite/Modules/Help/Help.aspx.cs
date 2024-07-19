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
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewSubContractorHelp);

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

        protected void cmdSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Page.IsValid)
                //{
                //    String question = UI.Utils.GetFormString(txtComment.Text);

                //    if (question != null)
                //    {
                //        TradesController.GetInstance().SubmitQuestion(question);
                //        txtComment.Text = String.Empty;
                //        pnlMessage.Visible = true;
                //    }
                //}
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        #endregion
    }

}