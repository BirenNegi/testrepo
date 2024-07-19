using System;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListRDOsPage : System.Web.UI.Page
    {

#region Private Methods
        private void bindRDOs()
        {
            gvRDOs.DataSource = ProjectsController.GetInstance().GetRDOs();
            gvRDOs.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListRDOs);

                if (Security.ViewAccess(Security.userActions.EditRDOs))
                {
                    phAddNew.Visible = true;
                    gvRDOs.Columns[0].Visible = true;
                }

                if (!Page.IsPostBack)
                {
                    bindRDOs();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void butAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (sdrRDODate.Date != null)
                {
                    ProjectsController.GetInstance().AddRDO((DateTime)sdrRDODate.Date);
                    sdrRDODate.Date = null;
                    bindRDOs();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvRDOs_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DateTime dateTime = (DateTime)gvRDOs.DataKeys[e.RowIndex].Value;
            ProjectsController.GetInstance().DeleteRDO(dateTime);
            bindRDOs();
        }

        protected void gvRDOs_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvRDOs.PageIndex = e.NewPageIndex;
            bindRDOs();
        }
#endregion

    }
}
