using System;
using System.IO;
using System.Configuration;

using SOS.Core;

namespace SOS.Web
{
    public partial class FileSelectControl : System.Web.UI.UserControl
    {

#region Private Membersiations;
        private String filePath;
        public String FileFolder { get; set; }
        public Boolean? Required { get; set; }
        public String Path { get; set; }
        public Boolean Disabled { get; set; }
#endregion

#region Public properties
        public String FilePath
        {
            get { return UI.Utils.GetFormString(inputFilePath.Value); }
            set
            {
                filePath = value;
                txtFileName.Text = UI.Utils.SetFormString(filePath);
                inputFilePath.Value = UI.Utils.SetFormString(filePath);
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String fileFolder = FileFolder == null ? Core.Utils.SelectFile : FileFolder;
                String path = Path == null ? String.Empty : Path;
                valFileName.Visible = Required == null ? false : (Boolean)Required;

                txtFileName.Text = UI.Utils.SetFormString(filePath);
                txtFileName.Attributes.Add("readonly", "true");
                inputFilePath.Value = UI.Utils.SetFormString(filePath);

                if (!Disabled)
                    cmdSelFile.NavigateUrl = Utils.PopupFile(this.Page, inputFilePath.ClientID, txtFileName.ClientID, fileFolder, path);
                else
                    cmdSelFile.ImageUrl = "~/images/IconSearchGrey.png";
            }
        }
#endregion

    }
}

