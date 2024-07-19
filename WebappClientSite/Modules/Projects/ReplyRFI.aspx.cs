using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

            rFIsRespnseInfo.ResponseFolderPath = RFIResponsePath;

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
            string RFIsResponsePath=null, Projpath=null, RFINumberPath = null;
            ProjectsController projectController = ProjectsController.GetInstance();
            

            ResponseNumber = projectController.GetRFIsResponseNumber(rFIInfo);
            try
            {
                if (FileUpload1.HasFile)
                {         //check for ProjectFolder exist under RFIsResponse Folder if not ctreate itby projectnumber

                    RFIsResponsePath = (WebConfigurationManager.AppSettings["RFIsResponseFolder"]).ToString();
                    Projpath = RFIsResponsePath + "\\" + rFIInfo.Project.Number.ToString();
                    RFINumberPath = Projpath + "\\" + rFIInfo.Number.ToString();
                    RFIResponsePath = RFINumberPath + "\\" + ResponseNumber.ToString();
                }

                //Save data into the RFIsREsponse table  
                FormToObjects();
                rFIsRespnseInfo.Id=projectController.AddRFIsResponse(rFIsRespnseInfo);

                if (FileUpload1.HasFile)
                {

                    if (!Directory.Exists (Server.MapPath(Projpath)))
                        Directory.CreateDirectory(Server.MapPath(Projpath));

                    if (!Directory.Exists(Server.MapPath(RFINumberPath)))
                        Directory.CreateDirectory(Server.MapPath(RFINumberPath));

                    if (!Directory.Exists(Server.MapPath(RFIResponsePath)))
                        Directory.CreateDirectory(Server.MapPath(RFIResponsePath));

                    foreach (var file in FileUpload1.PostedFiles)
                    {
                        file.SaveAs(Server.MapPath(RFIResponsePath + "\\" + Path.GetFileName(file.FileName)));
                        SaveFiletoDatabase(file);
                    }
               }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

           
            //send email
           // ProjectsController projectsController = ProjectsController.GetInstance();
            try
            {
                List<FileInfo> FileList = new List<FileInfo>();
                if (rFIsRespnseInfo.ResponseFolderPath.Length > 0)
                {                      
                    DirectoryInfo Di = new DirectoryInfo(Server.MapPath(rFIsRespnseInfo.ResponseFolderPath));
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



    }
}