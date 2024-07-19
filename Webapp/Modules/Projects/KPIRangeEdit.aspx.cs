using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SOS.Core;

namespace SOS.Web
{
    public partial class KPIRangeEdit : System.Web.UI.Page
    {
        KPIRangeInfo KPIrange = null;
        KPIPointsInfo KpiPoints = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            TitleBar.Title = "Edit KPI Color Range";
            if (!IsPostBack)
            {
                try
                {
                    Security.CheckAccess(Security.userActions.KPIRangeEdit);
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }
            }
        }


        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
                KPIrange = new KPIRangeInfo();
                KPIrange.Id = (int)GridView1.DataKeys[e.RowIndex].Value;  //int.Parse(row.Cells[1].Text.ToString());
                KPIrange.MinRange = int.Parse(((TextBox)(GridView1.Rows[e.RowIndex].FindControl("TxtMin"))).Text);
                KPIrange.MaxRange = int.Parse(((TextBox)(GridView1.Rows[e.RowIndex].FindControl("TxtMax"))).Text);
                KPIrange.TargetValue= int.Parse(((TextBox)(GridView1.Rows[e.RowIndex].FindControl("TxtTarget"))).Text);

                if (Page.IsValid)
                {
                    ProjectsController.GetInstance().UpdateKPIRange(KPIrange);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/KPIRangeEdit.aspx");




        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)GridView2.Rows[e.RowIndex];
                KpiPoints = new KPIPointsInfo();
                KpiPoints.Id = int.Parse(row.Cells[1].Text.ToString());
                KpiPoints.minPoints = int.Parse(((TextBox)(GridView2.Rows[e.RowIndex].FindControl("TxtminPoints"))).Text);
                KpiPoints.Points = int.Parse(((TextBox)(GridView2.Rows[e.RowIndex].FindControl("TxtPoints"))).Text);
               


                if (Page.IsValid)
                {
                    ProjectsController.GetInstance().UpdateKPIPoints(KpiPoints);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Projects/KPIRangeEdit.aspx");




        }
    }
}