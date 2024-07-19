using System;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListJobTypes : System.Web.UI.Page
    {

#region Private Methods
        private void bindJobTypes()
        {
            gvJobTypes.DataSource = ContractsController.GetInstance().GetJobTypes();
            gvJobTypes.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListJobTypes);

                if (Security.ViewAccess(Security.userActions.EditJobType))
                {
                    phAddNew.Visible = true;
                    gvJobTypes.Columns[0].Visible = true;
                    gvJobTypes.Columns[1].Visible = true;
                }

                if (!Page.IsPostBack)
                {
                    bindJobTypes();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvJobTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                int jobTypeId = (int)gvJobTypes.DataKeys[e.RowIndex].Value;
                ContractsController.GetInstance().DeleteJobType(new JobTypeInfo(jobTypeId));
                bindJobTypes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
