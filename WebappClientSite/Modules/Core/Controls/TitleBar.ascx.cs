using System;
using System.Web.UI;

namespace SOS.Web
{
    public partial class TitleBarControl : UserControl
    {

#region Public properties
        public String Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public String Info
        {
            get { return lblInfo.Text; }
            set { lblInfo.Text = value; }
        }
#endregion

    }
}
