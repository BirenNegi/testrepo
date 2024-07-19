using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditClientTradeTypePage : System.Web.UI.Page
    {

#region Members
        private ClientTradeTypeInfo clientTradeTypeInfo = null;
#endregion

#region Private Methods
        private void ObjectsToForm()
        {
            if (clientTradeTypeInfo.Id == null)
            {
                TitleBar.Title = "Adding Client Trade Type";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Client Trade Type";
            }

            txtName.Text = UI.Utils.SetFormString(clientTradeTypeInfo.Name);
        }

        private void FormToObjects()
        {
            clientTradeTypeInfo.Name = UI.Utils.GetFormString(txtName.Text);
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parameterClientTradeTypeId = Request.Params["ClientTradeTypeId"];

            try {
                Security.CheckAccess(Security.userActions.EditClientTradeType);

                if (parameterClientTradeTypeId == null)
                    clientTradeTypeInfo = new ClientTradeTypeInfo();
                else
                {
                    clientTradeTypeInfo = TradesController.GetInstance().GetClientTradeType(Int32.Parse(parameterClientTradeTypeId));
                    Core.Utils.CheckNullObject(clientTradeTypeInfo, parameterClientTradeTypeId, "Client Trade Type");
                }

                if (!Page.IsPostBack)
                    ObjectsToForm();
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
                    clientTradeTypeInfo.Id = TradesController.GetInstance().AddUpdateClientTradeType(clientTradeTypeInfo);
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Trades/ListClientTradeTypes.aspx");
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Trades/ListClientTradeTypes.aspx");
        }
#endregion

    }
}