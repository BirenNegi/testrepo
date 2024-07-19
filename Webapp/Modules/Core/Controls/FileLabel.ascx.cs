using System;
using System.IO;
using System.Configuration;

using SOS.Core;

namespace SOS.Web
{
    public partial class FileLabelControl : System.Web.UI.UserControl
    {

#region Public properties
        public DrawingRevisionInfo DrawingRevision
        {
            set
            {
                FileInfo fileInfo;

                if (value != null && value.File != null)
                {
                    fileInfo = new FileInfo(UI.Utils.FullPath(value.Drawing.Project.AttachmentsFolder, value.File));

                    if (fileInfo.Exists)
                    {
                        lnkFile.NavigateUrl = String.Format("~/Modules/Projects/ShowDrawingRevision.aspx?DrawingRevisionId={0}", value.IdStr);
                        lnkFile.Text = UI.Utils.SetFormString(value.File);

                        lblSize.Text = "(" + UI.Utils.SetFormFileSize(fileInfo.Length) + ")";

                        lnkFile.Visible = true;
                        lblSize.Visible = true;
                    }
                    else
                    {
                        lblFile.Text = UI.Utils.SetFormString(value.File);

                        lblSize.CssClass = "frmError";
                        lblSize.Text = "(Not found)";

                        lblFile.Visible = true;
                        lblSize.Visible = true;
                    }
                }
            }

        }
#endregion

    }
}