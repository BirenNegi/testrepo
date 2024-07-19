using System;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListHolidaysPage : System.Web.UI.Page
    {

#region Private Methods
        private void bindHolidays()
        {
            gvHolidays.DataSource = ProjectsController.GetInstance().GetHolidays();
            gvHolidays.DataBind();
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ListHolidays);

                if (Security.ViewAccess(Security.userActions.EditHolidays))
                {
                    phAddNew.Visible = true;
                    gvHolidays.Columns[0].Visible = true;
                }

                if (!Page.IsPostBack)
                {
                    bindHolidays();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void butAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (sdrHolidayDate.Date != null)
                {
                    ProjectsController.GetInstance().AddHoliday((DateTime)sdrHolidayDate.Date);
                    sdrHolidayDate.Date = null;
                    bindHolidays();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvHolidays_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            DateTime dateTime = (DateTime)gvHolidays.DataKeys[e.RowIndex].Value;
            ProjectsController.GetInstance().DeleteHoliday(dateTime);
            bindHolidays();
        }

        protected void gvHolidays_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvHolidays.PageIndex = e.NewPageIndex;
            bindHolidays();
        }
#endregion

    }
}
