using System;
using System.Web.UI;

namespace SOS.Web
{
    public partial class ErrorMessage : UserControl
    {

#region Public properties
        public String Description
        {
            set
            { 
                lblDescription.Text = value;
                phError.Visible = true;
            }
        }
#endregion

    }
}
