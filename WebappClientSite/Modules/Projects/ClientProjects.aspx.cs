using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ClientProjects : SOSPage
    {

        #region Members
        private ClientContactInfo clientContactInfo = null;
        #endregion


        #region Private Methods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            return currentNode;
        }

        private void BuildProjectLinks()
        {
            if (clientContactInfo.Projects != null)
            {
                foreach (ProjectInfo projectInfo in clientContactInfo.Projects)
                {
                    Table tbl = new Table();
                    TableHeaderRow Thr = new TableHeaderRow();
                    TableRow Tr = new TableRow();
                    TableCell Td = new TableCell();
                    ImageButton img = new ImageButton();
                    HyperLink Hpl = new HyperLink();

                    tbl.CssClass = "ptTable";
                    //----1st Row ----
                    Tr.CssClass = "ptHeaderRow";
                    Td.ColumnSpan = 3;
                   
                    Hpl.Text = projectInfo.Name;
                    Hpl.ID = "pjName";
                    Hpl.NavigateUrl = "~/Modules/Projects/ClientProjectDetails.aspx?ProjectId="+projectInfo.IdStr;

                    Td.Controls.Add(Hpl);
                    Tr.Cells.Add(Td);
                    Tr.Style.Add("color","#036bba");//font - family:Verdana, Arial;font - weight:bold;font - size:8pt; ");
                    tbl.Rows.Add(Tr);
                    //----2nd Row----
                    Tr = new TableRow();
                        Td = new TableCell();
                        Td.CssClass = "ptImageCell";
                                img.ImageUrl = "../../Images/building.GIF";
                                img.CssClass = "ptImage";
                                //img.Attributes.Add("onclick()", "RedirectToNextPage();");
                        Td.Controls.Add(img);
                    Tr.Cells.Add(Td);

                        Td = new TableCell();
                    Td.CssClass = "ptRow";
                        Td.ColumnSpan = 2;
                        Td.Controls.Add(new LiteralControl("<br />"));

                    if (projectInfo.PostalCode == null)
                        projectInfo.PostalCode = "";

                        Td.Text = projectInfo.Principal + "<br />" + projectInfo.Street+ "<br />"+projectInfo.State+"   "+projectInfo.PostalCode.ToString();
                    Tr.Cells.Add(Td);
                    tbl.Rows.Add(Tr);
                    //---3rd Row---
                    Tr = new TableRow();
                    Tr.CssClass= "ptFooterRow";
                        Td = new TableCell();
                        Td.Width = new Unit("30%");
                    Tr.Cells.Add(Td);
                        Td = new TableCell();
                        Td.Width = new Unit("40%");
                        Td.Text = projectInfo.StatusName + "|" + projectInfo.FullNumber; //"<a href='~/Modules/Core/DashBoardDetails.aspx'>" + projectInfo.StatusName + "|" + projectInfo.FullNumber+"</a>";
                    Tr.Cells.Add(Td);
                        Td = new TableCell();
                        Td.Width = new Unit("30%");
                    Tr.Cells.Add(Td);
                    tbl.Rows.Add(Tr);
                    


                    //add to main table/row/cell
                    
                    td2.Controls.Add(new LiteralControl("<br /> <br />"));
                    td2.Controls.Add(tbl);

                }

            }

        }




        #endregion




        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ClientProjects);
                clientContactInfo = ((ClientContactInfo)Web.Utils.GetCurrentUser());

                if (clientContactInfo != null)
                    clientContactInfo.Projects = PeopleController.GetInstance().GetClientProjects(clientContactInfo.Id);

                if (!Page.IsPostBack)
                {
                    if (clientContactInfo.Projects.Count >1)
                        BuildProjectLinks();
                    else
                        Response.Redirect("~/Modules/Projects/ClientProjectDetails.aspx?ProjectId="+clientContactInfo.Projects[0].IdStr, false);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        #endregion

       
    }
}