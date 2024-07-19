using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using System.IO;

namespace SOS.Web
{
    public partial class Gallery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/ProjectImages/ALDI - Dandenong - WH & DC/"));
                List<ListItem> files = new List<ListItem>();
                foreach (string filePath in filePaths)
                {
                    files.Add(new ListItem(Path.GetFileName(filePath), ResolveUrl("~/ProjectImages/ALDI - Dandenong - WH & DC/" + Path.GetFileName(filePath))));
                }
                rptImages.DataSource = files;
                rptImages.DataBind();
            }
        }


      





    }
}