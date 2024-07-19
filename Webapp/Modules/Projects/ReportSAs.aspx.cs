using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportSAsPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindForm()
        {
            List<String> statusList = new List<string>();

            ddlStatus.Items.Add(new ListItem("", String.Empty));
            ddlStatus.Items.Add(new ListItem("To be issued", ClientVariationInfo.StatusTobeIssued));
            ddlStatus.Items.Add(new ListItem("To be approved", ClientVariationInfo.StatusToBeApproved));
            ddlStatus.Items.Add(new ListItem("Approved", ClientVariationInfo.StatusApproved));
            ddlStatus.Items.Add(new ListItem("Works completed", ClientVariationInfo.StatusWorksCompleted));
            ddlStatus.Items.Add(new ListItem("Invoiced", ClientVariationInfo.StatusInvoiced));
            ddlStatus.Items.Add(new ListItem("Paid", ClientVariationInfo.StatusPaid));
            ddlStatus.Items.Add(new ListItem("Cancelled", ClientVariationInfo.StatusCancelled));


            BindProjectsList();//#-------------


        }




        //#-----------------------------------

        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();

            //////if (chkAll.Checked)
            //////    projectInfoList = projectsController.ListProjects();
            //////else

           projectInfoList = projectsController.ListActiveProjects();

            DdlProjectName.Items.Clear();
            DdlProjectName.Items.Add(new ListItem(String.Empty, String.Empty));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    DdlProjectName.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
        }



        //#---------------------------------


        private void BindReport()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            List<ClientVariationInfo> clientVariationInfoList;
           // List<ClientVariationInfo> FilteredclientVariation;
            String filterInfo;

            if (sdrStartDate.Date != null)
                if (sdrEndDate.Date != null)
                    filterInfo = "Created between " + UI.Utils.SetFormDate(sdrStartDate.Date) + " and " + UI.Utils.SetFormDate(sdrEndDate.Date);
                else
                    filterInfo = "Created after " + UI.Utils.SetFormDate(sdrStartDate.Date);
            else
                if (sdrEndDate.Date != null)
                    filterInfo = "Created before " + UI.Utils.SetFormDate(sdrEndDate.Date);
                else
                    filterInfo = "All records";

            clientVariationInfoList = projectsController.GetClientVariationsTotals(sdrStartDate.Date, sdrEndDate.Date, ClientVariationInfo.VariationTypeSeparateAccounts, UI.Utils.GetFormString(ddlStatus.SelectedValue));

            reportParameters.Add(new ReportParameter("FilterInfo", filterInfo));

            repSAs.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\SeparateAccounts.rdlc";
            repSAs.LocalReport.SetParameters(reportParameters);
            repSAs.LocalReport.DataSources.Clear();

            //#--------

            if (DdlProjectName.SelectedItem.Value != String.Empty)
            {
                filterInfo += " ProjectName: " + DdlProjectName.SelectedItem.Text;
                List<ClientVariationInfo> FilteredclientVariations = clientVariationInfoList.FindAll(X => X.ProjectName == DdlProjectName.SelectedItem.Text);

                repSAs.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", FilteredclientVariations));
            }
            else
            {

                repSAs.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", clientVariationInfoList));


            }

            //#------

            //#-------//// repSAs.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", clientVariationInfoList));


            repSAs.DataBind();

            repSAs.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewReports);

                if (!Page.IsPostBack)
                {
                    repSAs.Visible = false;
                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                BindReport();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
             Response.Redirect("~/Modules/Core/ListReports.aspx");
        }
#endregion

    }
}
