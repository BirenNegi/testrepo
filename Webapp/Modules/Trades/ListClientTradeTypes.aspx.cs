using System;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListClientTradeTypesPage : System.Web.UI.Page
    {

#region Private Methods
        private void bindClientTradeTypes()
        {
            gvClientTradeTypes.DataSource = TradesController.GetInstance().GetClientTradeTypes();
            gvClientTradeTypes.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListClientTradeTypes);

                if (Security.ViewAccess(Security.userActions.EditClientTradeType))
                {
                    phAddNew.Visible = true;
                    gvClientTradeTypes.Columns[0].Visible = true;
                    gvClientTradeTypes.Columns[1].Visible = true;
                }

                if (!Page.IsPostBack)
                    bindClientTradeTypes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvClientTradeTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int clientTradeTypeId = (int)gvClientTradeTypes.DataKeys[e.RowIndex].Value;
            TradesController.GetInstance().DeleteClientTradeType(new ClientTradeTypeInfo(clientTradeTypeId));
            bindClientTradeTypes();
        }
#endregion

    }
}
