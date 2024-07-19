using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using SOS.Core;
using System.IO;
using System.Web.Configuration;

namespace SOS.Web
{

      public partial class ReplyRFI : SOSPage
    {
        #region Members
        private RFIInfo rFIInfo = null;
        private RFIsResponseInfo rFIsRespnseInfo = null;
        int ResponseNumber = 0;
        string RFIResponsePath = "";
        #endregion

        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (rFIInfo == null)
                return null;

            tempNode.ParentNode.Url += "?ProjectId=" + rFIInfo.Project.IdStr;

            tempNode.ParentNode.ParentNode.Title = rFIInfo.Project.Name;

            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + rFIInfo.Project.IdStr;

            tempNode.Title = rFIInfo.Subject;
            tempNode.Url += "?RFId=" + rFIInfo.IdStr;
            return currentNode;
        }

        private void BindRFI()
        {

            
            ProcessController processController = ProcessController.GetInstance();

            //if (Security.ViewAccess(Security.userActions.EditRFI) && processController.AllowEditCurrentUser(rFIInfo))
            //{
            //    pnlEdit.Visible = true;
            //    phSendEmail.Visible = true;
            //}

            lblNumber.Text = UI.Utils.SetFormInteger(rFIInfo.Number);
            lblSubject.Text = UI.Utils.SetFormString(rFIInfo.Subject);
            lblRaiseDate.Text = UI.Utils.SetFormDate(rFIInfo.RaiseDate);
            lblTargetAnswerDate.Text = UI.Utils.SetFormDate(rFIInfo.TargetAnswerDate);
            lblActualAnswerDate.Text = UI.Utils.SetFormDate(rFIInfo.ActualAnswerDate);
            lblStatus.Text = UI.Utils.SetFormString(Utils.GetConfigListItemName("RFI", "Status", rFIInfo.Status));
            
        }

        private void FormToObjects()
        {
            rFIsRespnseInfo = new RFIsResponseInfo();
            rFIsRespnseInfo.ResponseNumber = ResponseNumber;
            rFIsRespnseInfo.RFI = rFIInfo;
            rFIsRespnseInfo.Responsemessage = UI.Utils.SetFormString(Txtmessage.Text);
            string rFIresponseFolder= (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();
            if(RFIResponsePath!="")
             rFIsRespnseInfo.ResponseFolderPath = "\\" + RFIResponsePath.Substring(RFIResponsePath.IndexOf("RFIsResponse"));

        }


        private void SaveFiletoDatabase(HttpPostedFile hpFile)
        {
            

            Stream FileStream = hpFile.InputStream;
            BinaryReader br = new BinaryReader(FileStream);
            byte[] fileData = br.ReadBytes((Int32)FileStream.Length);

            string FileName = Path.GetFileName(hpFile.FileName);
            string FileExtension = Path.GetExtension(FileName);
            string contentType = hpFile.ContentType;

            ProjectsController.GetInstance().AddRFIsResponseAttachments(rFIsRespnseInfo, FileName, FileExtension, fileData);
                                                                             
        }


        #endregion





        protected void Page_Load(object sender, EventArgs e)
        {
            FileUpload1.Attributes.Add("onchange", "displayFile();");

            String parameterRFIId;
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.ViewRFI);
                parameterRFIId = Utils.CheckParameter("RFIId");
                rFIInfo = projectsController.GetRFI(Int32.Parse(parameterRFIId));
                Core.Utils.CheckNullObject(rFIInfo, parameterRFIId, "RFI");
                rFIInfo.Project = projectsController.GetProject(rFIInfo.Project.Id);

                if (!Page.IsPostBack)
                   BindRFI();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string rFIsResponsePath=null, Projpath = null, RFINumberPath = null;

            ProjectsController projectController = ProjectsController.GetInstance();
            //Save  the attachments in server side

            ResponseNumber = projectController.GetRFIsResponseNumber(rFIInfo);

            if (FileUpload1.HasFile)
            {
                 rFIsResponsePath = (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();

                 Projpath = Server.MapPath(@"\\RFIsResponse\\" + rFIInfo.Project.Number.ToString());

                 RFINumberPath = Projpath + "\\" + rFIInfo.Number.ToString();

                RFIResponsePath = RFINumberPath + "\\" + ResponseNumber.ToString();
            }

            #region to Save RFIsREsponse
            //Save data into the RFIsREsponse table  
            FormToObjects();

            rFIsRespnseInfo.Id=projectController.AddRFIsResponse(rFIsRespnseInfo);

            #endregion


            #region  FileUpload1.HasFile and Save it to folder


            try
            {
                    //check for ProjectFolder exist under RFIsResponse Folder if not ctreate itby projectnumber

                    // string rFIsResponsePath = (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();      //string rFIsResponsePath = @"\\192.168.200.114\RFIsResponse";

                    // string Projpath = rFIsResponsePath + "\\" + rFIInfo.Project.Number.ToString();

               
                   
                        if (FileUpload1.HasFile)
                        {  
                                    
                                    DirectoryInfo Di = new DirectoryInfo(Projpath);

                                    if (!Di.Exists)      //Server.MapPath(Projpath) for clientsos
                                        Directory.CreateDirectory(Projpath);

                                    if (!Directory.Exists(RFINumberPath))     //Server.MapPath(RFINumberPath) for clientsos
                                        Directory.CreateDirectory(RFINumberPath);

                                    if (!Directory.Exists(RFIResponsePath)) //Server.MapPath(RFIResponsePath)for clientsos
                                        Directory.CreateDirectory(RFIResponsePath);

                                    foreach (var file in FileUpload1.PostedFiles)
                                    {
                                        file.SaveAs(RFIResponsePath + "\\" + Path.GetFileName(file.FileName)); // Server.MapPath   for clientsos

                                        SaveFiletoDatabase(file);
                                    }
                  
                         }

            }
            catch (Exception Ex)
            {
                Response.Write(Ex.ToString());
               // Utils.ProcessPageLoadException(this, Ex);
            }
            #endregion


         

            //send email
           // ProjectsController projectsController = ProjectsController.GetInstance();
            try
            {
                List<FileInfo> FileList = new List<FileInfo>();
                if (rFIsRespnseInfo.ResponseFolderPath!=null)
                {
                    // DirectoryInfo Di = new DirectoryInfo(Server.MapPath(rFIsRespnseInfo.ResponseFolderPath)); -- for client SOS


                   //string RFIsResponsePath = (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();


                    //WebClient request = new WebClient();
                    // request.Credentials = new NetworkCredential("VC-Web_01\\snayak", "Saanvi25");

                    //DirectoryInfo Di = new DirectoryInfo(RFIsResponsePath +(rFIsRespnseInfo.ResponseFolderPath.Replace("\\\\","\\"))); 
                    DirectoryInfo Di = new DirectoryInfo(RFIResponsePath);
                    if (Di.Exists)
                    {
                        foreach (FileInfo file in Di.GetFiles())
                        {
                            FileList.Add(file);
                        }

                    }
                }
                    
                ProcessController.SendRFIsResponseToDistributionList(rFIsRespnseInfo,FileList);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }


           Response.Redirect("~/Modules/Projects/ListRFIs.aspx?ProjectId=" + rFIInfo.Project.IdStr);


        }

        
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    string X = "----";

        //    try
        //    {

        //        string rFIsResponsePath = (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();

        //        //string rFIsResponsePath = @"\\192.168.200.114\RFIsResponse";
        //        X = "1";

        //        //string Projpath = rFIsResponsePath + "\\" + rFIInfo.Project.Number.ToString();
        //        string Projpath = "\\\\vc-web-01\\RFIsResponse" + "\\" + rFIInfo.Project.Number.ToString();

        //        //string RFINumberPath = Projpath + "\\" + rFIInfo.Number.ToString();

        //        X = "12";
        //        //WebClient request = new WebClient();
        //        //request.Credentials = new NetworkCredential("VC-Web_01\\snayak", "Saanvi25");
        //        X = "123";
        //        DirectoryInfo Di = new DirectoryInfo(Projpath);
        //        X = "1234";
        //        if (Di.Exists)      //Server.MapPath(Projpath) for clientsos
        //        {
        //            X = "aqaqqaq";
        //            Directory.CreateDirectory(Projpath + "\\SanTest");
        //            X = "AAAAAAAA";
        //        }
        //        else
        //        {

        //            Directory.CreateDirectory(Projpath);
        //            X = "BBBB";
        //        }

        //        X = "CCCC";

        //    }
        //    catch (Exception Ex)
        //    {
        //        Response.Write(X + "Error--->" + Ex.ToString());

        //    }
        //    Response.Write(X);


        //}

        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    string X = "----";

            

        //       // string rFIsResponsePath = (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();

              
        //     //   X = "1";
           
        //      //  string Projpath =("\\\\VC-web-01\\c$\\SOS\\Prod\\WebappClientSite\\RFIsResponse\\" + rFIInfo.Project.Number.ToString());

        //   try
        //     {
        //        if (FileUpload2.HasFiles) {


        //          //  FileUpload2.SaveAs("\\\\VC-web-01\\RFIsResponse\\" + FileUpload2.PostedFile.FileName.ToString());

        //            SaveFiletoDatabase(FileUpload2.PostedFile);
        //        }
               

               

        //    }
        //    catch (Exception Ex)
        //    {
        //        Response.Write( "----Error--->" + Ex.ToString());

        //    }
        //    Response.Write(X);


        //}



    }
}