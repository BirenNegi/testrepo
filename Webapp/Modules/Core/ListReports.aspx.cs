using System;
using SOS.Core;

namespace SOS.Web
{
    public partial class ListReportsPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (Security.ViewAccess(Security.userActions.ViewAdminReports))
                {
                    pnlPendingTasks.Visible = true;
                    pnlCompletedTasks.Visible = true;
                    pnlActivitySummary.Visible = true;
                    pnlReportKPI.Visible = true;
                }

                if (Security.ViewAccess(Security.userActions.ViewBudgetReports))
                {
                    pnlReportWorkOrders.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion

    }
}
