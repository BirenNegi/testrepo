using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditJobTypePage : System.Web.UI.Page
    {

#region Members
        private JobTypeInfo jobTypeInfo = null;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            if (jobTypeInfo.Id == null)
            {
                TitleBar.Title = "Adding Job Type";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Job Type";
            }

            txtName.Text = UI.Utils.SetFormString(jobTypeInfo.Name);
        }

        private void FormToObjects()
        {
            jobTypeInfo.Name = UI.Utils.GetFormString(txtName.Text);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterJobTypeId = Request.Params["JobTypeId"];

            try {
                Security.CheckAccess(Security.userActions.EditJobType);

                if (parameterJobTypeId == null)
                {
                    jobTypeInfo = new JobTypeInfo();
                }
                else
                {
                    jobTypeInfo = ContractsController.GetInstance().GetJobType(Int32.Parse(parameterJobTypeId));
                    Core.Utils.CheckNullObject(jobTypeInfo, parameterJobTypeId, "Job Type");
                }
                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try {
                if (Page.IsValid)
                {
                    FormToObjects();
                    jobTypeInfo.Id = ContractsController.GetInstance().AddUpdateJobType(jobTypeInfo);
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/ListJobTypes.aspx");
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/ListJobTypes.aspx");
        }
#endregion

    }
}