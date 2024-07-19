using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SOS.Core;
using Microsoft.Reporting.WebForms;

namespace SOS.Web
{
    public partial class ShowProjectSnapShot : System.Web.UI.Page
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

        private void BindReport()
        {

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProjectInfo projectInfo = null;
            List<ReportParameter> reportParameters = new List<ReportParameter>();
            String parameterProjectId;
            List<ProjectInfo> projectInfoList = new List<ProjectInfo>(); 
            try
            {
                if (ddlProjects.SelectedValue != String.Empty)
                {
                    parameterProjectId= ddlProjects.SelectedValue;

                    projectInfo = new ProjectInfo(Int32.Parse(parameterProjectId));


                    if (projectInfo != null)
                    {

                        projectInfo = projectsController.GetProject(projectInfo.Id);
                        projectInfo.EOTs = projectsController.GetEOTs(projectInfo);
                        projectInfo.RFIs = projectsController.GetRFIs(projectInfo);

                        repPROJECTSNAPSHOT.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\SnapShot.rdlc";
                      
                        repPROJECTSNAPSHOT.LocalReport.DataSources.Clear();

                        if (projectInfo.Name == null || projectInfo.FullNumber == null || projectInfo.CommencementDate == null
                          || projectInfo.CompletionDate == null)
                        {
                          throw new System.ArgumentException("Please enter the missing Project information like Name/Number/Commencement Date/Completion Date/Contract Amount... under General Information section");
                            //Response.Write("<Script language='javascript>alert('Please enter the missing information like Project Name/Number/Commencement Date/Completion Date/Contract Amount... under General Information section '); </Script>");
                            //  return;
                            
                        }
                        projectInfo.ClaimedEOTsCompletionDate = projectsController.GetProjectCompletionDateWithClaimedEOTs(projectInfo);
                        projectInfo.ApprovedEOTsCompletionDate = projectsController.GetProjectCompletionDateWithApprovedEOTs(projectInfo);
                        reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(projectInfo.Name)));
                        reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(projectInfo.FullNumber)));
                        reportParameters.Add(new ReportParameter("ProjectCommencementDate", UI.Utils.SetFormDate(projectInfo.CommencementDate)));
                        reportParameters.Add(new ReportParameter("ProjectCompletionDate", UI.Utils.SetFormDate(projectInfo.CompletionDate)));
                        reportParameters.Add(new ReportParameter("ProjectRevisedCompletioDate", UI.Utils.SetFormDate(projectInfo.ApprovedEOTsCompletionDate)));
                       
                        string EOtsclaimed= projectInfo.EOTs.FindAll(x => x.ApprovalDate != null).Sum(x => x.DaysClaimed).ToString();
                        string EotsApprovedDays = projectInfo.EOTs.FindAll(x => x.ApprovalDate != null).Sum(x => x.DaysApproved).ToString();
                        reportParameters.Add(new ReportParameter("EotsClaimeddays", UI.Utils.SetFormString(EOtsclaimed)));
                        reportParameters.Add(new ReportParameter("EotsApproveddays", UI.Utils.SetFormString(EotsApprovedDays)));

                        string RFIsReplied= projectInfo.RFIs.FindAll(x => x.Status == "R").Count.ToString();
                        string RFIsPending = projectInfo.RFIs.FindAll(x => x.Status != "R").Count.ToString();
                        string RFIsTotal= projectInfo.RFIs.Count.ToString();
                        reportParameters.Add(new ReportParameter("RFIsReplied", UI.Utils.SetFormString(RFIsReplied)));
                        reportParameters.Add(new ReportParameter("RFIsPending", UI.Utils.SetFormString(RFIsPending)));
                        reportParameters.Add(new ReportParameter("RFIsTotal", UI.Utils.SetFormString(RFIsTotal)));

                        double Duration = projectsController.GetProjectOriginalDuration(projectInfo);
                        if (projectInfo.ApprovedEOTsCompletionDate == null)
                            projectInfo.ApprovedEOTsCompletionDate = projectInfo.CompletionDate;

                        double RevisedDuration = projectsController.GetProjectWorkingDuration(projectInfo.CommencementDate.Value, projectInfo.ApprovedEOTsCompletionDate.Value, double.Parse(EotsApprovedDays));
                        double WorkingDaystoDate;
                        if (projectInfo.PracticalCompletionDate!=null)
                             WorkingDaystoDate = projectsController.GetProjectWorkingDuration(projectInfo.CommencementDate.Value, projectInfo.PracticalCompletionDate.Value, double.Parse("0"));
                        else
                            WorkingDaystoDate = projectsController.GetProjectWorkingDuration(projectInfo.CommencementDate.Value, DateTime.Today.Date, double.Parse(EotsApprovedDays));

                        double WorkingDaysBalance = projectsController.GetProjectWorkingDuration(DateTime.Today.Date, projectInfo.ApprovedEOTsCompletionDate.Value,0.0);

                        double PercentageWorkingDaystoDate=0.0, PercentageWorkingDaysBalance=0.0;

                        //if (WorkingDaystoDate >= Duration && projectInfo.PracticalCompletionDate!=null) { PercentageWorkingDaystoDate = 100; }
                       // else
                            if (Duration > 0)
                            { 
                                PercentageWorkingDaystoDate = (WorkingDaystoDate * 100) / (Duration) ;
                                PercentageWorkingDaysBalance = (WorkingDaysBalance * 100) / (Duration);
                             }

                        reportParameters.Add(new ReportParameter("Duration", UI.Utils.SetFormString(Duration.ToString())));
                        reportParameters.Add(new ReportParameter("WorkingDaystoDate", UI.Utils.SetFormString(WorkingDaystoDate.ToString())));
                        reportParameters.Add(new ReportParameter("WorkingDaysBalance", UI.Utils.SetFormString(WorkingDaysBalance.ToString())));
                        reportParameters.Add(new ReportParameter("PercentageWorkingDaystoDate", UI.Utils.SetFormString(PercentageWorkingDaystoDate.ToString("00.00"))));
                        reportParameters.Add(new ReportParameter("PercentageWorkingDaysBalance", UI.Utils.SetFormString(PercentageWorkingDaysBalance.ToString("00.00"))));

                        projectInfoList.Add(projectInfo);

                        repPROJECTSNAPSHOT.LocalReport.DataSources.Add(new ReportDataSource("ProjectSnapShot", projectsController.GetProjectSnapShot(projectInfo)));
                        repPROJECTSNAPSHOT.LocalReport.DataSources.Add(new ReportDataSource("CVSA", projectsController.GetProjectCVSA(projectInfo)));
                        repPROJECTSNAPSHOT.LocalReport.DataSources.Add(new ReportDataSource("TurnoverVsTime", projectsController.GetTurnoverVsTime(projectInfoList, null, null, null)));

                        if (projectInfo.PracticalCompletionDate!=null)
                            repPROJECTSNAPSHOT.LocalReport.DataSources.Add(new ReportDataSource("DurationVsClaim", projectsController.GetWorkingDaysInPercent(projectInfo.CommencementDate, projectInfo.ApprovedEOTsCompletionDate.Value, projectInfo.PracticalCompletionDate,projectInfo.Id)));

                        else if (WorkingDaystoDate >= Duration && projectInfo.PracticalCompletionDate == null)
                            repPROJECTSNAPSHOT.LocalReport.DataSources.Add(new ReportDataSource("DurationVsClaim", projectsController.GetWorkingDaysInPercent(projectInfo.CommencementDate, projectInfo.ApprovedEOTsCompletionDate.Value,DateTime.Now, projectInfo.Id)));
                        else
                            repPROJECTSNAPSHOT.LocalReport.DataSources.Add(new ReportDataSource("DurationVsClaim", projectsController.GetWorkingDaysInPercent(projectInfo.CommencementDate, projectInfo.ApprovedEOTsCompletionDate, DateTime.Now, projectInfo.Id)));


                        repPROJECTSNAPSHOT.LocalReport.SetParameters(reportParameters);

                       
                        repPROJECTSNAPSHOT.DataBind();
                        repPROJECTSNAPSHOT.Visible = true;
                    }


                   
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        #endregion


        #region eventHandler
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Security.CheckAccess(Security.userActions.ViewBudgetReports);

                if (!Page.IsPostBack)
                {
                    repPROJECTSNAPSHOT.Visible = false;
                    BindProjectsList();
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
            //try
            //{
                BindReport();
            //}
            //catch (Exception Ex)
            //{
            //    Utils.ProcessPageLoadException(this, Ex);
            //}
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Core/ListReports.aspx");
        }


        #endregion


    }
}