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
    public partial class Photogallery3 : SOSPage
    {
        #region Members
        private ProjectInfo projectInfo = null;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.Photogallery);
                string parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = ProjectsController.GetInstance().GetProject(Int32.Parse(parameterProjectId));


                if (!IsPostBack)
                {
                    #region st
                    string path = Server.MapPath("~/ProjectImages/" + projectInfo.Name).ToString();
                   // string path = Server.MapPath("~/ProjectImages/SanTest").ToString();
                    string[] filesindirectory = Directory.GetFiles(path);
                    
                    Image img;
                    //Panel pnl;
                    //int i = 0;


                    foreach (string file in filesindirectory)
                    {
                        img = new Image();
                        img.ImageUrl = "../../ProjectImages/" + projectInfo.Name + "/" + Path.GetFileName(file);
                        //img.ImageUrl = "../../ProjectImages/SanTest/" + Path.GetFileName(file);
                        slides.Controls.Add(img);
                    }
                
                    #endregion
                    }


            }

            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

        }





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

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Utils.LogOutUser();
            Response.Redirect("~/Modules/Core/Login.aspx");
        }
    }
}