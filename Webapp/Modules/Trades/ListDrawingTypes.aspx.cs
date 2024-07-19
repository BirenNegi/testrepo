using System;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListDrawingTypesPage : System.Web.UI.Page
    {

#region Private Methods
        private void bindDrawingTypes()
        {
            gvDrawingTypes.DataSource = TradesController.GetInstance().GetDrawingTypes();
            gvDrawingTypes.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListDrawingTypes);

                if (Security.ViewAccess(Security.userActions.EditDrawingType))
                {
                    phAddNew.Visible = true;
                    gvDrawingTypes.Columns[0].Visible = true;
                    gvDrawingTypes.Columns[1].Visible = true;
                }

                if (!Page.IsPostBack)
                    bindDrawingTypes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvDrawingTypes_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
           int DrawingTypeId = (int)gvDrawingTypes.DataKeys[e.RowIndex].Value;
           TradesController.GetInstance().DeleteDrawingType(new DrawingTypeInfo(DrawingTypeId));
           bindDrawingTypes();
        }
#endregion

    }
}
