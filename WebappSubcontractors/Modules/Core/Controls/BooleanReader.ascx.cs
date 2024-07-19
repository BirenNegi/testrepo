using System;

namespace SOS.Web
{
    public partial class BooleanReaderControl : System.Web.UI.UserControl
    {

#region Public properties
        public bool? Checked
        {
            set
            {
                if (value != null)
                    if ((bool)value)
                        radbutBoolean.SelectedValue = "Y";
                    else
                        radbutBoolean.SelectedValue = "N";
                else
                    radbutBoolean.SelectedValue = "?";
            }
            get
            {
                if (radbutBoolean.SelectedValue == "Y")
                    return true;
                else
                    if (radbutBoolean.SelectedValue == "N")
                        return false;
                    else
                        return null;
            }
        }

        public bool? NotChecked
        {
            get { return Checked == null ? null : !Checked; }
        }
#endregion

    }
}