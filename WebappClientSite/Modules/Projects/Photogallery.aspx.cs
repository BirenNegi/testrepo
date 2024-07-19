using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using System.IO;
using SOS.Core;

namespace SOS.Web
{
    public partial class Photogallery : SOSPage
    {

        #region Members
        private ProjectInfo projectInfo = null;
      
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showSlides(1)", true);
            //ClientScript.RegisterStartupScript(GetType(), "hiya", "alert('hi!')", true);

            return currentNode;
        }

        private void BindImages()
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/ProjectImages/ALDI - Dandenong - WH & DC/"));
            List<ListItem> files = new List<ListItem>();
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                files.Add(new ListItem(fileName, "ProjectImages/ALDI - Dandenong - WH & DC/" + fileName));
            }
            //Repeater1.DataSource = files;
            //Repeater1.DataBind();

        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Utils.LogOutUser();
            Response.Redirect("~/Modules/Core/Login.aspx");
        }

        private void BindTreeview()
        {

            List<ProjectImage> projectImageList;

            projectImageList = ProjectsController.GetInstance().GetProjectImages(projectInfo);



            if (projectImageList.Count > 0)
            {
                // DirectoryInfo rootInfo = new DirectoryInfo("");
                TreeNode ProjectNode = new TreeNode();

                ProjectNode.Text = projectImageList[0].ProjectName.ToString();
                TreeView1.Nodes.Add(ProjectNode);


                foreach (ProjectImage img in projectImageList)
                {
                    TreeNode imgNode = new TreeNode();
                    imgNode.Text = img.ImageName;
                    imgNode.Value = img.IdStr; //Convert.ToBase64String(img.ImageData);
                    //imgNode.SelectAction = TreeNodeSelectAction.None;
                    if (img.FolderName != null)
                    {

                        TreeNode ParentNode = FindNode(ProjectNode, img.FolderName);

                        if (ParentNode == null)
                        {
                            ParentNode = new TreeNode();
                            ParentNode.Text = img.FolderName;
                            ParentNode.SelectAction = TreeNodeSelectAction.None;
                            //ParentNode.Value = img.Parent;

                            ParentNode.ChildNodes.Add(imgNode);
                            ProjectNode.ChildNodes.Add(ParentNode);
                        }
                        else
                        {

                            ParentNode.ChildNodes.Add(imgNode);

                        }

                    }

                    else
                        ProjectNode.ChildNodes.Add(imgNode);
                }
                ProjectNode.CollapseAll();
            }


        }

        private TreeNode FindNode(TreeNode ParentNode, string matchText)
        {
            foreach (TreeNode node in ParentNode.ChildNodes)
            {
                if (node.Text == matchText)
                {
                    return node;
                }

            }
            return (TreeNode)null;
        }

        #endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Security.CheckAccess(Security.userActions.Photogallery);
                string parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = ProjectsController.GetInstance().GetProject(Int32.Parse(parameterProjectId));


                if (!IsPostBack)
                {


                    BindTreeview();

                    if (Image1.ImageUrl == null || Image1.ImageUrl == String.Empty)
                        Image1.Visible = false;


                    #region slider
                    /*
                    string path = Server.MapPath("~/ProjectImages/" + projectInfo.Name).ToString();
                    // string path = Server.MapPath("~/ProjectImages/SanTest").ToString();
                    string[] filesindirectory = Directory.GetFiles(path);

                    Image img;
                   


                    foreach (string file in filesindirectory)
                    {
                        img = new Image();
                        img.ImageUrl = "../../ProjectImages/" + projectInfo.Name + "/" + Path.GetFileName(file);
                        //img.ImageUrl = "../../ProjectImages/SanTest/" + Path.GetFileName(file);
                        slides.Controls.Add(img);
                    }
                       */
                    #endregion




                }





                ClientContactInfo clientContactInfo = (ClientContactInfo)Utils.GetCurrentUser();
                if (Utils.GetCurrentUser() != null)
                {
                    lnkUser.Text = clientContactInfo.Name;
                    lnkUser.NavigateUrl = "~/Modules/Core/ChangePassword.aspx";
                    //lblSubcontractor.Text = clientContactInfo.Name;
                }

                if (SiteMap.CurrentNode != null)
                    switch (SiteMap.CurrentNode.ResourceKey)
                    {
                        case "Projects": menuMain.Items[0].Selected = true; break;
                        case "Help": menuMain.Items[1].Selected = true; break;
                    }




            }

            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }















       

        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode clickedNode = this.TreeView1.SelectedNode;
            TreeView1.SelectedNodeStyle.BackColor = System.Drawing.Color.LightBlue;

            if (clickedNode.Parent != null)
            {
                ProjectImage pImage = ProjectsController.GetInstance().GetProjectImageById(int.Parse(clickedNode.Value.ToString()));
                Image1.ImageUrl = "data:Image/png;base64,"+Convert.ToBase64String(pImage.ImageData);


            }

            if (Image1.ImageUrl == null || Image1.ImageUrl == String.Empty)
                Image1.Visible = false;
            else Image1.Visible = true;


        }

        #endregion




    }
}