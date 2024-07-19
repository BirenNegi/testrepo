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
    public partial class PhotoGallery1 : SOSPage
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
                { }
                #region st
                string path = Server.MapPath("~/ProjectImages/" + projectInfo.Name).ToString();
                string[] filesindirectory = Directory.GetFiles(path);
                int i = 0;
                Image img;
                Panel pnl;
               


                foreach (string file in filesindirectory)
                {   
                    //<img src = '../../ProjectImages/" + projectInfo.Name+"/" + Path.GetFileName(file) + @"'style='width:100%'/> 
                    //i += 1;
                    //pnl = new Panel();
                    //pnl.ID = "Pnl" + i.ToString();


                    img = new Image();
                    img.ImageUrl = "../../ProjectImages/" + projectInfo.Name + "/" + Path.GetFileName(file);
                    img.Width = Unit.Percentage(100);
                    //img.Attributes.Add("style", "display:block");//block   none
                    img.CssClass = "mySlides";

                    //pnl.CssClass = "mySlides";
                    //pnl.Controls.Add(img);

                    Maindiv.Controls.Add(img);

                    //Panel1.Controls.Add(img);
                    //i += 1;
                    //literaldiv = @"<div class='mySlides fade'>
                    //<div class='numbertext'>" + i.ToString() + "/" + filesindirectory.Count().ToString() + @" </div>
                    //<img src='" + img.ImageUrl + @"'/>               
                    //<div class='text'>Caption Text</div>
                    //</div>";
                    //string Literal = "<div class='text'>" + Path.GetFileName(file) + "</div>";
                    //Maindiv.Controls.Add(new LiteralControl(Literal));

                }
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "showSlides(1)", true);
                //ClientScript.RegisterStartupScript(GetType(), "hiya", "alert('hi!')", true);
                //}
                #endregion



            }

            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

        }
    }
}