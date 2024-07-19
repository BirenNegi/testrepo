using System;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class SelectFilePage : System.Web.UI.Page
    {

#region Members
        private String rootPath = null;
        private Boolean isFolder = false;
#endregion

#region Private Methods
        private void BindForm(String systemPath, String projectPath)
        {
            lblSystemPath.Text = systemPath;
            lblProjectPath.Text = projectPath;

            if (isFolder)
            {
                pnlFolder.Visible = true;
                pnlProjectPath.Visible = false;
                TitleBar1.Title = "Selecting Server Folder";
            }
            else
                TitleBar1.Title = "Selecting Server File";

            lnkNoneSelected.NavigateUrl = Utils.PopupSendFile(this, "", "");
        }

        private void BindTree(String CurrentPath)
        {
            String[] folders = Directory.GetDirectories(CurrentPath);
            String fileFullName;
            TreeNode treeNode;

            tvFiles.Nodes.Clear();

            treeNode = new TreeNode();
            treeNode.Value = CurrentPath;
            treeNode.SelectAction = TreeNodeSelectAction.None;
            treeNode.Expanded = true;
            tvFiles.Nodes.Add(treeNode);

            if (CurrentPath == rootPath)
                treeNode.Text = "\\";
            else
            {
                treeNode.Text = CurrentPath.Replace(rootPath, "");
                treeNode = new TreeNode("..", CurrentPath.Substring(0, CurrentPath.LastIndexOf("\\")), "~/Images/IconView.gif", "", "");
                tvFiles.Nodes[0].ChildNodes.Add(treeNode);
            }

            foreach (String s in folders)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(s);
                treeNode = new TreeNode(directoryInfo.Name, directoryInfo.FullName, "~/Images/IconView.gif", "", "");
                tvFiles.Nodes[0].ChildNodes.Add(treeNode);
            }

            if (isFolder)
            {
                if (CurrentPath != rootPath)
                {
                    fileFullName = CurrentPath.Replace(rootPath + "\\", "").Replace("\\", "\\\\");
                    lnkThisSelected.NavigateUrl = Utils.PopupSendFile(this, fileFullName, fileFullName);
                    lnkThisSelected.Visible = true;
                    lblThisSelected.Visible = false;
                }
                else
                {
                    lnkThisSelected.Visible = false;
                    lblThisSelected.Visible = true;
                }
            }
            else
            {
                String[] files = Directory.GetFiles(CurrentPath);

                foreach (String s in files)
                {
                    FileInfo fileInfo = new FileInfo(s);
                    fileFullName = fileInfo.FullName.Replace(rootPath + "\\", "").Replace("\\", "\\\\");
                    treeNode = new TreeNode(fileInfo.Name, fileInfo.FullName, "", Utils.PopupSendFile(this, fileFullName, fileFullName), "");
                    tvFiles.Nodes[0].ChildNodes.Add(treeNode);
                }
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            String parmeterFileFolder;
            String parameterPath;
            String systemPath;
            String projectPath;

            try
            {
                Security.CheckAccess(Security.userActions.SelectFile);

                parmeterFileFolder = Utils.CheckParameter("FileFolder");  //san---- its similar to Request.querystring["FileFolder"]
                parameterPath = Utils.CheckParameter("Path");

                isFolder = parmeterFileFolder == Core.Utils.SelectFolder;

                if (parameterPath != String.Empty)
                    projectPath = "\\" + parameterPath;
                else
                    projectPath = String.Empty;

                systemPath = ConfigurationManager.AppSettings["DocumentsFolder"].ToString();

                rootPath = systemPath + projectPath;

                if (!IsPostBack)
                {
                    BindForm(systemPath, projectPath);
                    BindTree(rootPath);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void tvFiles_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindTree(tvFiles.SelectedValue);
        }
#endregion

    }
}
