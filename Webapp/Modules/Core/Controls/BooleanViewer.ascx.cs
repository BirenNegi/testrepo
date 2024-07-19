using System;

namespace SOS.Web
{
    public partial class BooleanViewerControl : System.Web.UI.UserControl
    {

#region Public properties
        public bool? Checked
        {
            set
            {
                if (value == null)
                    imgBoolean.ImageUrl = "~/Images/1x1.gif";
                else
                    if ((bool)value)
                        imgBoolean.ImageUrl = "~/Images/IconChecked.gif";
                    else
                        imgBoolean.ImageUrl = "~/Images/IconUnchecked.gif";
            }
        }
#endregion

    }
}