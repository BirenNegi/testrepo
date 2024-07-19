using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SOS.Core;
using System.Configuration;
using System.IO;

using SOS.FileTransferService.DataContracts;
using Client = SOS.FileTransferService.Client;

namespace SOS.Web
{
    public partial class EditSubcontractorQualifications: SOSPage
    {

        #region Members
        private ContactInfo contactInfo = null;
        private QualificationInfo qualificationInfo = null;
        #endregion


        #region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {

            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (contactInfo == null)
                return null;


            tempNode.ParentNode.Title = contactInfo.Name;
            tempNode.ParentNode.Url += "?peopleId=" + contactInfo.IdStr;

          

            return currentNode;

        }

        private void ObjectsToForm()
        {
            ProcessController processController = ProcessController.GetInstance();
            Boolean isEditable = false;
            if (Security.ViewAccess(Security.userActions.EditContact))
            {
                isEditable = processController.AllowEditCurrentUser(contactInfo);

            }
            gvQualifications.Columns[4].Visible = isEditable;
            gvQualifications.Columns[5].Visible = isEditable;

            BindQualifications();
        }

        private void BindQualifications()
        {
            if (contactInfo.Qualifications != null)
            {
                gvQualifications.DataSource = contactInfo.Qualifications;
                gvQualifications.DataBind();
                 
            }

        }



        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {

                  

            ClientScript.RegisterClientScriptInclude(this.GetType(), "myScript", "Style/gvAJAX/popup.js");
            String parameterPeopleId;

            try
            {
                Security.CheckAccess(Security.userActions.EditContact);
                parameterPeopleId = Utils.CheckParameter("PeopleId");
                contactInfo = (ContactInfo)PeopleController.GetInstance().GetPerson(Int32.Parse(parameterPeopleId), null, null);

                if (contactInfo.SubContractor.Id != ((ContactInfo)Utils.GetCurrentUser()).SubContractor.Id)
                    Response.Redirect("~/Modules/Core/Login.aspx");

                contactInfo.Qualifications = PeopleController.GetInstance().GetQualificationsByContactId(Int32.Parse(parameterPeopleId));
                Core.Utils.CheckNullObject(contactInfo, parameterPeopleId, "Contact");
                if (!Page.IsPostBack)
                {
                  
                    ObjectsToForm();
                }

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

        }

        protected void gvQualifications_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            gvQualifications.EditIndex = -1;
            BindQualifications();
        }

        protected void gvQualifications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
               
                FileMetaData fileMetaData;

                if (e.CommandName == "RemoveFile")
                {
                        int qualificationId = int.Parse(e.CommandArgument.ToString());
                        if (contactInfo.Qualifications != null)
                            qualificationInfo = contactInfo.Qualifications.Find(x => x.Id == qualificationId);

                        int index = gvQualifications.EditIndex;

                        try
                            {
                                if (qualificationInfo != null)
                                { 
                                    fileMetaData = Client.Utils.GetFileMetaData(qualificationInfo.imagePath);

                                    if (fileMetaData.Exist)
                                        Client.Utils.DeleteFile(qualificationInfo.imagePath);

                                    qualificationInfo.imageName = "";
                                    qualificationInfo.imagePath = "";
                                    PeopleController.GetInstance().UpdateQualification(qualificationInfo);
                                    BindQualifications();
                                }
                            }
                            catch (Exception Ex)
                            {
                                Utils.ProcessPageLoadException(this, Ex);
                            }
                 }

            if (e.CommandName == "DownloadFile")
            {
                int qualificationId = int.Parse(e.CommandArgument.ToString());
                if (contactInfo.Qualifications != null)
                    qualificationInfo = contactInfo.Qualifications.Find(x => x.Id == qualificationId);

                int index = gvQualifications.EditIndex;
                try
                        {
                            if (qualificationInfo != null)
                            {
                                fileMetaData = Client.Utils.GetFileMetaData(qualificationInfo.imagePath);
                                if (fileMetaData.Exist)
                                    Response.Redirect("~/Modules/Projects/ShowQualificationsFile.aspx?ImagePath=" + qualificationInfo.imagePath+"&ImageName="+qualificationInfo.imageName,false);

                            }
                        }
                        catch (Exception Ex)
                        {
                            Utils.ProcessPageLoadException(this, Ex);
                        }
            }

            if (e.CommandName == "AddQualification")
            {

                byte[] fileBytes;
                int fileLength;
                int numBytes = 0;
                string QualificationsFolderPath = "";
                string SubcontractorFilePath = "";
                GridViewRow frow = gvQualifications.FooterRow;
                qualificationInfo = new QualificationInfo();

                try { 
                        qualificationInfo.qualificationName = ((TextBox)(frow.Cells[1].FindControl("TxtNewQualificationName"))).Text.ToString();
                        qualificationInfo.cardNumber = ((TextBox)(frow.Cells[2].FindControl("TxtNewcardNumber"))).Text.ToString();
                        qualificationInfo.expiryDate = UI.Utils.GetFormDate(((TextBox)(frow.Cells[3].FindControl("TxtNewexpiryDate"))).Text.ToString());

                        qualificationInfo.contactInfo=new ContactInfo(contactInfo.Id);
                        
                        FileUpload fileUploader = frow.FindControl("FileloaderFooter") as FileUpload;
                        fileLength = fileUploader.PostedFile.ContentLength;

                        if (fileLength > 0)
                        {
                            fileBytes = new byte[fileLength];
                            numBytes = fileUploader.PostedFile.InputStream.Read(fileBytes, 0, fileUploader.PostedFile.ContentLength);
                            QualificationsFolderPath = ConfigurationManager.AppSettings["SubcontractorsQualifications"].ToString();
                            SubcontractorFilePath = "\\" + contactInfo.SubContractorName + "_" + contactInfo.IdStr + "_";

                            Client.Utils.PutFile(QualificationsFolderPath + SubcontractorFilePath + fileUploader.PostedFile.FileName, null, fileBytes);

                            qualificationInfo.imageName = fileUploader.PostedFile.FileName;
                            qualificationInfo.imagePath = QualificationsFolderPath + SubcontractorFilePath + fileUploader.PostedFile.FileName;
                        }
                        else
                        {
                            qualificationInfo.imageName = "";
                            qualificationInfo.imagePath = "";
                        }
                        qualificationInfo.Id = PeopleController.GetInstance().AddQualification(qualificationInfo);
                    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }

            }

        }

        protected void gvQualifications_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvQualifications.Rows[e.RowIndex];
            int? Id = (int?)gvQualifications.DataKeys[Convert.ToInt32(e.RowIndex)].Value;
            qualificationInfo = contactInfo.Qualifications.Find(x => x.Id == Id);

            if (qualificationInfo != null)
                PeopleController.GetInstance().DeleteQualification(qualificationInfo);

            Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }

        protected void gvQualifications_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvQualifications.EditIndex = e.NewEditIndex;
            BindQualifications();

           
        }

        protected void gvQualifications_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            byte[] fileBytes;
            int fileLength=0;
            int numBytes = 0;
            string QualificationsFolderPath = "";
            string SubcontractorFilePath = "";

          try
            {
                if (Page.IsValid)
                {
                    GridViewRow row = (GridViewRow)gvQualifications.Rows[e.RowIndex];
                    int? Id = (int?)gvQualifications.DataKeys[Convert.ToInt32(e.RowIndex)].Value;

                    qualificationInfo = contactInfo.Qualifications.Find(x=>x.Id==Id);

                    qualificationInfo.qualificationName = UI.Utils.GetFormString(((TextBox)(row.Cells[1].FindControl("TxtUpdateQualificationName"))).Text).ToString();
                    qualificationInfo.cardNumber = ((TextBox)(row.Cells[2].FindControl("TxtUpdatecardNumber"))).Text.ToString();
                    qualificationInfo.expiryDate = UI.Utils.GetFormDate(((TextBox)(row.Cells[3].FindControl("TxtUpdateexpiryDate"))).Text);

                    //HtmlInputFile fileLoader = row.FindControl("Fileloader")as HtmlInputFile;
                    //fileLength = fileLoader.PostedFile.ContentLength;

                    FileUpload fileUploader = row.FindControl("Fileloader") as FileUpload;

                    if (fileUploader.Visible)
                    { 
                        fileLength = fileUploader.PostedFile.ContentLength;


                        if (fileLength > 0)
                        {
                            fileBytes = new byte[fileLength];
                            numBytes = fileUploader.PostedFile.InputStream.Read(fileBytes, 0, fileUploader.PostedFile.ContentLength);
                            QualificationsFolderPath = ConfigurationManager.AppSettings["SubcontractorsQualifications"].ToString();
                            SubcontractorFilePath = "\\" + contactInfo.SubContractorName + "_" + contactInfo.IdStr + "_";


                            Client.Utils.PutFile(QualificationsFolderPath + SubcontractorFilePath + fileUploader.PostedFile.FileName, null, fileBytes);



                            qualificationInfo.imageName = fileUploader.PostedFile.FileName;
                            qualificationInfo.imagePath = QualificationsFolderPath + SubcontractorFilePath + fileUploader.PostedFile.FileName;


                        }
                        else
                        {

                            qualificationInfo.imageName = "";
                            qualificationInfo.imagePath = "";
                        }
                    }
                    PeopleController.GetInstance().UpdateQualification(qualificationInfo);

                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
         
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

       
    }
}