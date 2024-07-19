using System;

namespace SOS.Web
{
    public partial class DateReaderControl : System.Web.UI.UserControl
    {

#region Public Events
        public event System.EventHandler dateChanged;
#endregion 

#region Public properties
        public DateTime? Date
        {
            get
            {
                if (Enabled)
                    return UI.Utils.GetFormDate(txtCalendar.Text);
                else
                    return actCalendar.SelectedDate;
            }
            set { actCalendar.SelectedDate = value; }
        }

        public Boolean Enabled
        {
            get { return txtCalendar.Enabled; }
            set
            {
                txtCalendar.Enabled = value;
                imgCalendar.Visible = value;
            }
        }
#endregion

#region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (dateChanged != null)
                txtCalendar.AutoPostBack = true;
        }

        protected void txtCalendar_TextChanged(object sender, EventArgs e)
        {
            if (dateChanged != null)
                dateChanged(this, new EventArgs());
        }
#endregion

    }
}
