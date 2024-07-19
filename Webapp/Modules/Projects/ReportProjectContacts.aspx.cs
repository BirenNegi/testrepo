using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ReportProjectContacts : System.Web.UI.Page
    {

        #region Private Methods
        private void BuildForm()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();
            List<ProjectInfo> projectInfoList = null;
            //if (currentUser.Id == 4066)
            //{
                projectInfoList = projectsController.GetActiveProjects(currentUser);
            //}
            //else
            //{
            //    projectInfoList = projectsController.GetActiveProjectsForEmployee(currentUser);
            //}
            
            if (projectInfoList.Count > 0)
            {

                dpdProjectList.DataSource = projectInfoList;
                dpdProjectList.DataTextField = "Name";
                dpdProjectList.DataValueField = "Id";
                dpdProjectList.DataBind();

                dpdProjectList.Items.Add("Select");
                dpdProjectList.SelectedIndex =0;
            }
        }
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //*  ----SAN-----09112023-------
                //if (!IsPostBack)
                //{
                //    EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();  // DS@0230823
                //                                                                      //if (currentUser.Id != 4066)
                //                                                                      //{
                //                                                                      //    Security.CheckAccess(Security.userActions.ViewAdminReports);   //Access only for CM and  above
                //                                                                      //}
                //      BuildForm();
                //}
               


               if (!IsPostBack)
                    {
                        Security.CheckAccess(Security.userActions.ViewAdminReports);   //Access only for CM and  above
                        BuildForm();
                    }

                //*  ----SAN-----09112023-------

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void dpdProjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpdProjectList.SelectedValue != "Select")
            {
                ReportViewer1.DataBind();
                this.ReportViewer1.LocalReport.Refresh();

                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.DataBind();
                //ProjectInfo projectInfo = new ProjectInfo(int.Parse(dpdProjectList.SelectedItem.Value.ToString()));
                // BindGrid(projectInfo);
            }

        }






    }
}