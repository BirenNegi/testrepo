using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;
using System.Data;//#---
using SOS.Core;
using static iTextSharp.text.pdf.AcroFields;

namespace SOS.Web
{
    //
    // DS20240122
    //
    public partial class ReportTVsProjectPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();

            if (chkAll.Checked)
                projectInfoList = projectsController.ListProjects();
            else
                projectInfoList = projectsController.ListActiveProjects();

            ddlProjects.Items.Clear();
            ddlProjects.Items.Add(new ListItem(String.Empty, String.Empty));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
        }

        private void BindForm()
        {
            BindProjectsList();
        }

        private void BindReport()
        {

            //#-----
            DateTime? dateissued;
            DataTable dt = new DataTable();

            //#-------


            ProjectsController projectsController = ProjectsController.GetInstance();
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            ProjectInfo projectInfo;
            String projectId;

            if (ddlProjects.SelectedValue != String.Empty)
            {
                projectId = ddlProjects.SelectedValue;
                projectInfo = projectsController.GetProjectWithTenantVariations(Int32.Parse(projectId));

                if (projectInfo != null)
                {
                    reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(projectInfo.Name)));
                    reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(projectInfo.FullNumber)));

                    repCVsProject.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\TVs.rdlc";
                    repCVsProject.LocalReport.SetParameters(reportParameters);
                    repCVsProject.LocalReport.DataSources.Clear();

                    //San   //if (projectInfo.ClientVariations != null)
                    //San  //    repCVsProject.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", projectInfo.ClientVariations));


                    //#--------------------------------
                    if (projectInfo.ClientVariations != null)
                    {

                        dt.Columns.Add("NumberAndRevisionName");
                        dt.Columns.Add("Name");
                        dt.Columns.Add("WriteDate");
                        dt.Columns.Add("TotalAmount");
                        dt.Columns.Add("Status");
                        dt.Columns.Add("VerbalApprovalDate");
                        dt.Columns.Add("ApprovalDate");

                        dt.Columns["WriteDate"].DataType = typeof(DateTime);
                        dt.Columns["VerbalApprovalDate"].DataType= typeof(DateTime);
                        dt.Columns["ApprovalDate"].DataType= typeof(DateTime);
                        dt.Columns["TotalAmount"].DataType = typeof(decimal);

                        foreach (var item in projectInfo.ClientVariations)
                        {
                                if (item.Process.Steps[2].Role == "CM" && item.Process.Steps[2].Status == "Approved")
                            {
                                dateissued = item.Process.Steps[2].ActualDate;
                            }
                            else { dateissued = null; }


                            //----San------11/01/23---------------------------------

                            if (item.Status=="To be Issued")
                            {
                                dt.Rows.Add(item.NumberAndRevisionName, item.Name, dateissued, null, item.Status, item.VerbalApprovalDate, item.ApprovalDate);

                            }
                            else
                            //----San------11/01/23---------------------------------
                            //if (item.Status.ToUpper() == "TO BE APPROVED" || item.Status.ToUpper() == "TO BE ISSUED")   // DS20230912 >>>
                            //{
                            //    dt.Rows.Add(item.NumberAndRevisionName, item.Name, dateissued, null, item.Status, item.VerbalApprovalDate, item.ApprovalDate);

                            //}
                            //else
                            {                   // DS20231030                                                                       // DS20230912 <<<
                                dt.Rows.Add(item.NumberAndRevisionName, item.Name, dateissued, item.TotalAmount, item.Status, item.VerbalApprovalDate, item.ApprovalDate);

                            }

                        }

                        repCVsProject.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", dt));


                    }





                    //#---------------------------------------------


                    repCVsProject.DataBind();
                    repCVsProject.Visible = true;
                }
            }
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
                    repCVsProject.Visible = false;
                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            BindProjectsList();
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
