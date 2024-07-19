using System;
using System.Web;
using System.Web.UI.WebControls;

namespace SOS.Web
{
    public partial class ComboAndTextReaderControl : System.Web.UI.UserControl
    {
 
#region Public properties
        public DropDownList dropDownList
        {
            get
            {
                return ddlControl;
            }
        }

        public String SelectedValue
        {

            get { return UI.Utils.GetFormString(ddlControl.SelectedValue); }
        }

        public String TextOther
        {
            get
            {
                if (ddlControl.SelectedValue == Core.Utils.SelectedValueOther)
                {
                    return UI.Utils.GetFormString(txtControl.Text);
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                txtControl.Text = UI.Utils.SetFormString(value);
            }
        }
#endregion
        public void BindForm()
        {
            String strScript = "" +
            "<script language='JavaScript'>\r" +
            "function ShowHideTxtControl(ddlControl, txtControl, divControl) {\r" +
            "  if (document.getElementById(ddlControl).value == 'OT') {\r" +
            "     document.getElementById(divControl).className = 'Visible';\r" +
            "     document.getElementById(txtControl).focus();\r" +
            "  } else {\r" +
            "     document.getElementById(divControl).className = 'Invisible';\r" +
            "  }\r" +
            "}\r" +
            "</script>\r";

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ShowHideTxtControl"))
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "ShowHideTxtControl", strScript);

            ddlControl.Attributes["onChange"] = "ShowHideTxtControl('" + ddlControl.ClientID + "','" + txtControl.ClientID + "','" + divControl.ClientID + "');";

            if (ddlControl.SelectedValue == Core.Utils.SelectedValueOther)
                divControl.Attributes["Class"] = "Visible";
            else
                divControl.Attributes["Class"] = "Invisible";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindForm();
        }
    }
}

