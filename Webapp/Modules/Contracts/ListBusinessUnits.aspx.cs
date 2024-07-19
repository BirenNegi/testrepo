using System;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListBusinessUnits : System.Web.UI.Page
    {

#region Private Methods
        private void bindBusinessUnits()
        {
            gvBusinessUnits.DataSource = ContractsController.GetInstance().GetBusinessUnits();
            gvBusinessUnits.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListBusinessUnits);

                if (Security.ViewAccess(Security.userActions.EditBusinessUnit))
                {
                    phAddNew.Visible = true;
                    gvBusinessUnits.Columns[0].Visible = true;
                    gvBusinessUnits.Columns[1].Visible = true;
                }

                if (!Page.IsPostBack)
                {
                    bindBusinessUnits();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvBusinessUnits_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            try
            {
                int BusinessUnitId = (int)gvBusinessUnits.DataKeys[e.RowIndex].Value;
                ContractsController.GetInstance().DeleteBusinessUnit(new BusinessUnitInfo(BusinessUnitId));
                bindBusinessUnits();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
