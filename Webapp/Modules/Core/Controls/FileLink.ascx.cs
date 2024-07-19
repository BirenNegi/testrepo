using System;
using System.IO;
using System.Configuration;

using SOS.Core;

namespace SOS.Web
{
    public partial class FileLinkControl : System.Web.UI.UserControl
    {

#region Public properties
        public String FileTitle { get; set; }
        public String FilePath { get; set; }
        public String BasePath { get; set; }
        public String PageLink { get; set; }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            FileInfo fileInfo;

            if (FilePath != null)
            {
                fileInfo = new FileInfo(UI.Utils.FullPath(BasePath, FilePath));

                if (fileInfo.Exists)
                {
                    lnkFile.NavigateUrl = PageLink;
                    lnkFile.Text = FileTitle;
                    lnkFile.CssClass = "frmLink";
                    lnkFile.Visible = true;
                    lblFile.Visible = false;
                }
                else
                {
                    lblFile.Text = FileTitle + " (Not found)";
                    lblFile.CssClass = "frmError";
                    lblFile.Visible = true;
                    lnkFile.Visible = false;
                }
            }
            else
            {
                lblFile.Text = "No " + FileTitle;
                //#----lblFile.CssClass = "frmText";

                lblFile.CssClass = "frmError";//#-------to make it Red
                lblFile.Visible = true;
                lnkFile.Visible = false;
            }
        }
    }
#endregion

}
