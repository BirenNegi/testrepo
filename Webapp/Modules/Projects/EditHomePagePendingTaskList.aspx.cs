using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using SOS.Core;

namespace SOS.Web
{
    public partial class EditHomePagePendingTaskList : System.Web.UI.Page
    {
        #region Private Methods
        private void BuildForm()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();
            List<ProjectInfo> projectInfoList = projectsController.GetActiveProjectsForEmployee(currentUser);
           




            if (projectInfoList.Count == 0)
                lblProjectsInfo.Visible = true;
            else
            {

                dpdProjectList.DataSource = projectInfoList;
                dpdProjectList.DataTextField = "Name";
                dpdProjectList.DataValueField = "Id";
                dpdProjectList.DataBind();
                dpdProjectList.Items.Add("Select");
                dpdProjectList.SelectedIndex = dpdProjectList.Items.Count-1;
            }
        }
      


        protected void BindGrid(ProjectInfo projectInfo)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            Dictionary<int, List<ProcessStepInfo>> pendingProcessSteps;
            Dictionary<int, List<EmployeeInfo>> selectableEmployees;
            Dictionary<int, EmployeeInfo> selectedUsers;
            Dictionary<String, String> dictionaryRoles;
            List<ProcessStepInfo> processStepInfoList;

            EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();

            List<ProjectInfo> projectInfoList = projectsController.GetActiveProjectsForEmployee(currentUser);

            
            String rolesScript = String.Empty;
            String rolesInfo = hidRolesInfo.Value;
            String baseClientId = hidRolesInfo.ClientID.Substring(0, hidRolesInfo.ClientID.Length - hidRolesInfo.ID.Length);


            projectsController.InitializeHolidays();

            projectInfo = projectInfoList.Find(o => o.Id == projectInfo.Id);
           foreach (ProjectInfo projectInfo1 in projectInfoList)
                projectsController.GetProjectPeopleNames(projectInfo1);

           processController.GetEmpoyeesAndRoles(projectInfoList, (EmployeeInfo)Web.Utils.GetCurrentUser(), rolesInfo, out selectedUsers, out selectableEmployees, out dictionaryRoles);

            List<ProjectInfo> projectList = new List<ProjectInfo>();

            projectList.Add(projectInfoList.Find(o => o.Id == projectInfo.Id));
           
            // selectedUsers.Add(projectList[0].Id.Value, projectList[0].ConstructionManager); selectableEmployees[projectList[0].Id.Value].GroupBy(c => c.Id,(key, c) => c.FirstOrDefault())

            Dictionary<int, List<EmployeeInfo>> selectedUsersList = new Dictionary<int, List<EmployeeInfo>>();
            selectedUsersList.Add(projectList[0].Id.Value, (selectableEmployees[projectList[0].Id.Value].FindAll(a => a.Id != null).GroupBy(i=>i.Id.Value).Select(c1 => c1.First()).ToList()));

            pendingProcessSteps = processController.GetPendingSteps(projectList, selectedUsersList, null,null,true);

            if (selectableEmployees[projectInfo.Id.Value].Count > 0)

            {
                    processStepInfoList = pendingProcessSteps[projectInfo.Id.Value];
                    gvComparison.DataSource = processStepInfoList.FindAll(o => o.ProcessName == "Comparisons");
                    gvComparison.DataBind();

                    gvContract.DataSource = processStepInfoList.FindAll(o => o.ProcessName == "Contracts");
                    gvContract.DataBind();

                    gvVariationOrders.DataSource= processStepInfoList.FindAll(o => o.ProcessName == "Variation Orders") ;
                    gvVariationOrders.DataBind();

                    gvClientVariations.DataSource = processStepInfoList.FindAll(o => o.ProcessName == "Client Variations").OrderBy(o => o.Process.Project.ClientVariations[0].Number);
                    gvClientVariations.DataBind();

                    gvSeparateAccounts.DataSource = processStepInfoList.FindAll(o => o.ProcessName == "Separate Accounts").OrderBy(o => o.Process.Project.ClientVariations[0].Number);
                    gvSeparateAccounts.DataBind();

                    gvClaims.DataSource = processStepInfoList.FindAll(o => o.ProcessName == "Claims");
                    gvClaims.DataBind();

                   
            }



        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {   if(!IsPostBack) { 
                     Security.CheckAccess(Security.userActions.ViewAdminReports);   //Access only for CM and  above
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
            if (dpdProjectList.SelectedValue != null)
            {
                ProjectInfo projectInfo = new ProjectInfo(int.Parse(dpdProjectList.SelectedItem.Value.ToString()));
                BindGrid(projectInfo);
            }

        }

        protected void chkHide_CheckedChanged(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            CheckBox cbHide = (CheckBox)sender;

            GridViewRow parentRow = cbHide.NamingContainer as GridViewRow;
            int processId = int.Parse(parentRow.Cells[0].Text);
            bool HideorShow = cbHide.Checked;

            processController.UpdateProcessHideorShow(processId, HideorShow);
            
        }
    }
}