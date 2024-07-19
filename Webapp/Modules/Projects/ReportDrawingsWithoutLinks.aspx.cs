using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SOS.Core;

namespace SOS.Web
{
    public partial class ReportDrawingsWithoutLinks : System.Web.UI.Page
    {
        #region Private Methods
        private void BuildForm()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();
            List<ProjectInfo> projectInfoList = projectsController.GetActiveProjectsForEmployee(currentUser);

            if (projectInfoList.Count > 0)
            {

                dpdProjectList.DataSource = projectInfoList;
                dpdProjectList.DataTextField = "Name";
                dpdProjectList.DataValueField = "Id";
                dpdProjectList.DataBind();

                dpdProjectList.Items.Add("Select");
                dpdProjectList.SelectedIndex = 0;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Security.CheckAccess(Security.userActions.ViewDrawing);   //Access only for CM and  above
                    BuildForm();
                }
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
                RptDrawingWithoutLink.DataBind();
                this.RptDrawingWithoutLink.LocalReport.Refresh();

                RptDrawingWithoutLink.LocalReport.Refresh();
                RptDrawingWithoutLink.DataBind();
                //ProjectInfo projectInfo = new ProjectInfo(int.Parse(dpdProjectList.SelectedItem.Value.ToString()));
                // BindGrid(projectInfo);
            }

        }



    }
}