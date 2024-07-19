using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SOS.Web
{
    public partial class BalanceInclude : System.Web.UI.UserControl
    {

#region Event handlers
        public event EventHandler IncludeClicked;
#endregion

#region Public properties
        public Boolean IncludeAllCVSA
        {
            get
            {
                return rblIncludeCVSA.SelectedValue == "All";
            }
        }

        public Boolean IncludeAllOVO
        {
            get
            {
                return rblIncludeOVO.SelectedValue == "All";
            }
        }
#endregion

#region Event Handlers
        protected void rblInclude_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IncludeClicked != null)
                IncludeClicked(this, new EventArgs());
        }
#endregion
                
    }
}
