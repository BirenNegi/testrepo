using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditDrawingTypePage : System.Web.UI.Page
    {

#region Members
        private DrawingTypeInfo drawingTypeInfo = null;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            if (drawingTypeInfo.Id == null)
            {
                TitleBar.Title = "Adding Drawing Type";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Drawing Type";
            }

            txtName.Text = UI.Utils.SetFormString(drawingTypeInfo.Name);
        }

        private void FormToObjects()
        {
            drawingTypeInfo.Name = UI.Utils.GetFormString(txtName.Text);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterDrawingTypeId = Request.Params["DrawingTypeId"];

            try {
                Security.CheckAccess(Security.userActions.EditDrawingType);

                if (parameterDrawingTypeId == null)
                {
                    drawingTypeInfo = new DrawingTypeInfo();
                }
                else
                {
                    drawingTypeInfo = TradesController.GetInstance().GetDrawingType(Int32.Parse(parameterDrawingTypeId));
                    Core.Utils.CheckNullObject(drawingTypeInfo, parameterDrawingTypeId, "Drawing Type");
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
                    drawingTypeInfo.Id = TradesController.GetInstance().AddUpdateDrawingType(drawingTypeInfo);
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Trades/ListDrawingTypes.aspx");
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/ListDrawingTypes.aspx");
        }
#endregion

    }
}