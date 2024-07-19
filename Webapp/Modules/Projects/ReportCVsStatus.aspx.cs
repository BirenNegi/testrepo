using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportCVsStatusPage : System.Web.UI.Page
    {

#region Private Methods
        private void BindForm()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            ddlBusinessUnit.Items.Add(new ListItem(String.Empty, String.Empty));

            if (businessUnitInfoList != null)
                foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                    ddlBusinessUnit.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));
        }

        private void BindReport()
        {
            if (ddlBusinessUnit.SelectedValue != String.Empty)
            {
                List<ReportParameter> reportParameters = new List<ReportParameter>();
                ProjectsController projectsController = ProjectsController.GetInstance();
                List<ClientVariationInfo> clientVariationInfoList = null;

                List<CvStatus> CvStatusList = null;

                repCVsStatus.Reset();

                if (ddlProjects.SelectedValue == "Active")
                {
                    List<ProjectInfo> projectInfoList = projectsController.ListActiveProjects(new BusinessUnitInfo((int?)Int32.Parse(ddlBusinessUnit.SelectedValue)));
                    ProjectInfo project = null;

                    if (projectInfoList != null)
                    {
                        clientVariationInfoList = new List<ClientVariationInfo>();

                        foreach (ProjectInfo projectInfo in projectInfoList)
                        {
                            project = projectsController.GetProjectWithClientVariations(projectInfo.Id);

                            if (project.ClientVariations != null)
                                foreach (ClientVariationInfo clientVariation in project.ClientVariations)
                                    clientVariationInfoList.Add(clientVariation);
                        }
                    }

                    reportParameters.Add(new ReportParameter("BusinessUnitName", ddlBusinessUnit.SelectedItem.Text));

                    repCVsStatus.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\CVsStatus.rdlc";
                }
                else
                {
                    ProjectInfo projectInfo = projectsController.GetProjectWithClientVariations((int?)Int32.Parse(ddlProjects.SelectedValue));

                    clientVariationInfoList = projectInfo.ClientVariations;
                    //----------------------------
                    CvStatusList = new List<CvStatus>();
                    CvStatus CV = null;

                    var  ReportCVlist = projectsController.GetCVStatusReportData(Int32.Parse(ddlProjects.SelectedValue));
                    for (int i = 0; i < ReportCVlist.Rows.Count; i++)
                    {
                         var Clv= clientVariationInfoList.Find(a => a.IdStr == (ReportCVlist.Rows[i][0]).ToString());
                        if(Clv!=null) { 
                        CV = new CvStatus();
                        CV.Id =int.Parse(ReportCVlist.Rows[i][0].ToString());
                        CV.Name = ReportCVlist.Rows[i][1].ToString();
                        CV.Number= int.Parse(ReportCVlist.Rows[i][2].ToString());
                        CV.NumberAndRevisionName = Clv.NumberAndRevisionName;
                        CV.WriteDate = Clv.WriteDate;
                        CV.VerbalApprovalDate = Clv.VerbalApprovalDate; //Data.Utils.GetDBDateTime(ReportCVlist.Rows[i][4]);
                        CV.InternalApprovalDate = Clv.InternalApprovalDate; //Data.Utils.GetDBDateTime(ReportCVlist.Rows[i][5]);
                        CV.ApprovalDate = Clv.ApprovalDate; //Data.Utils.GetDBDateTime(ReportCVlist.Rows[i][6]);
                        CV.Status = Clv.Status;
                        CV.StatusOrder = Clv.StatusOrder;
                        CV.Value =decimal.Parse(ReportCVlist.Rows[i]["Value"].ToString());
                        CV.Amount = decimal.Parse(ReportCVlist.Rows[i]["Amount"].ToString());
                        CV.Balance = decimal.Parse(ReportCVlist.Rows[i]["Balance"].ToString());
                        CV.ProjectName = Clv.ProjectName;
                        CV.ProjectNumber = Clv.ProjectNumber;

                        CvStatusList.Add(CV);
                        }
                    }

                    //--------------------------------- 

                    reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(projectInfo.Name)));
                    reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(projectInfo.FullNumber)));

                    repCVsStatus.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\CVsStatusByProject.rdlc";
                }

                 repCVsStatus.LocalReport.SetParameters(reportParameters);
                 repCVsStatus.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", clientVariationInfoList));
                //#-- 
                repCVsStatus.LocalReport.DataSources.Add(new ReportDataSource("CLientVariationStatus", CvStatusList));


                repCVsStatus.DataBind();
                repCVsStatus.Visible = true;
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
                    repCVsStatus.Visible = false;
                    BindForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            String businessUnitId = ddlBusinessUnit.SelectedValue;

            if (businessUnitId != String.Empty)
            {
                List<ProjectInfo> projectInfoList = ProjectsController.GetInstance().ListActiveProjects((new BusinessUnitInfo((int?)Int32.Parse(businessUnitId))));

               ddlProjects.Items.Clear();
               //#--  ddlProjects.Items.Add(new ListItem("All Active", "Active"));//#

                if (projectInfoList != null)
                    foreach (ProjectInfo projectInfo in projectInfoList)
                        ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
            }
            else
            {
                ddlProjects.Items.Clear();
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


        private class CvStatus
        {
            public int Id { get; set; }
            public string IdStr { get; set; }
            public string Name { get; set; } 
            public int Number { get; set; }
            public String NumberAndRevisionName { get; set; }
            public  String Status { get; set; }
            public int? StatusOrder { get; set; }
            public DateTime? WriteDate { get; set; } 
            public DateTime? InternalApprovalDate { get; set; }
            public DateTime? VerbalApprovalDate { get; set; }
            public DateTime? ApprovalDate { get; set; }
            public DateTime? CancelDate { get; set; }
            public decimal? Value { get; set; }
            public decimal? Amount { get; set; }
            public decimal? Balance { get; set; }
            public string ProjectName { get; set; }
            public string ProjectNumber { get; set; }
        }



    }
}
