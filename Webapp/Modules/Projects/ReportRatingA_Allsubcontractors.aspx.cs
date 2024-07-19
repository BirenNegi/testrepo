using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Microsoft.Reporting.WebForms;
using SOS.Core;
using System.Data;

namespace SOS.Web
{
    public partial class ReportRatingA_Allsubcontractors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (!Page.IsPostBack)
                {
                    RvRatingA.Visible = true;
                    BindReport();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }


        }



        private void BindReport()
        {

           

            TradesController tradeController = TradesController.GetInstance();
            DataTable dt = new DataTable();

         
            dt = tradeController.GetAllRatingASubContractors();

            if (dt.Rows.Count > 0)
            {
                RvRatingA.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\RatingA_AllsubContractors.rdlc";//RatingAALL

                if (RvRatingA.LocalReport.DataSources.Count > 0)
                {
                    RvRatingA.LocalReport.DataSources.Clear();

                }

                RvRatingA.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
                RvRatingA.DataBind();
                RvRatingA.Visible = true;

            }
            else { RvRatingA.Visible = false; }


        }






    }
}