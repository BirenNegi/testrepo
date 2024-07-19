using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using SOS.Core;
using System.Drawing;

namespace SOS.Web
{
    public partial class DashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BuildForm();
            


        }


        private void BuildForm()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            EmployeeInfo currentUser = (EmployeeInfo)Utils.GetCurrentUser();
            List<ProjectInfo> projectInfoList = projectsController.GetActiveProjectsForEmployee(currentUser);

            //processController.GetEmpoyeesAndRoles(projectInfoList, (EmployeeInfo)Web.Utils.GetCurrentUser(), rolesInfo, out selectedUsers, out selectableEmployees, out dictionaryRoles);
            //pendingProcessSteps = processController.GetPendingSteps(projectInfoList, selectedUsers);

            HtmlTable table;
            HtmlTableRow row;
            HtmlTableCell cell;
            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                table = new HtmlTable();
                table.Attributes.Add("class", "ProjectThumbnails");
                //1st Row
                row = new HtmlTableRow();
                cell = CellHtmlHeaderText("lstHeader", projectInfo.Number + "-" + projectInfo.Name);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                //2nd Row


            }
            HyperLink1.ToolTip = "Manager: xyz\n\nCOntractAdmin:PQR\nDate:12/02/2016 \n\nDate:12/02/2016\nPending Tasks:18\nAddress:223 Bodmin Court Truganina\nBusiness Unit:Melbourne";

            ImageButton1.ToolTip = "Manager: xyz\n\nCOntractAdmin:PQR\nDate:12/02/2016 \n\nDate:12/02/2016\nPending Tasks:18\nAddress:223 Bodmin Court Truganina\nBusiness Unit:Melbourne";
            
        }




        private HtmlTableCell CellHtmlHeaderText(String className, String innerHtml)
        {
            HtmlTableCell cell = new HtmlTableCell();
            HyperLink hLink = new HyperLink();
            hLink.Text = innerHtml;
            hLink.NavigateUrl = "";
            hLink.ToolTip= "Manager: xyz\n\nCOntractAdmin:PQR\nDate:12/02/2016 \n\nDate:12/02/2016\nPending Tasks:18\nAddress:223 Bodmin Court Truganina\nBusiness Unit:Melbourne";

            cell.Attributes.Add("class", className);
            cell.Align = "Center";
            //cell.InnerHtml = innerHtml;
            cell.ColSpan = 2;
            cell.Controls.Add(hLink);
            return cell;
        }


        private HtmlTableCell CellHtmlImage(String className, String innerHtml)
        {
            HtmlTableCell cell = new HtmlTableCell();
            HtmlImage Img = new HtmlImage();
            Img.Src = "~/Images\building.gif";
            cell.Attributes.Add("class", className);
            cell.Align = "Center";
            cell.InnerHtml = innerHtml;
            return cell;
        }

        private HtmlTableCell CellHtmlFooterText(String className, String innerHtml)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", className);
            cell.Align = "Center";
            cell.InnerHtml = innerHtml;
            cell.ColSpan = 2;
            return cell;
        }


    }
}