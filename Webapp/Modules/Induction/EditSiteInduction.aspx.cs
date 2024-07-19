using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using SOS.Core;

namespace SOS.Web
{
    public partial class EditSiteInduction : SOSPage
    {
        #region Private Members
        private SiteInductionInfo siteInductionInfo = null;
        private InductionController inductionController = null;
        int? parameterProjectId = null;
        private ProjectInfo projectInfo = null;
        #endregion

        #region Private Methods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {

            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode.ParentNode;
            
            if (projectInfo == null)
                return null;

             tempNode.Title = projectInfo.Name ;
            tempNode.Url += "?ProjectId=" + projectInfo.IdStr; 

            return currentNode;
        }


        private void ObjectsToForm()
        {
            siteInductionInfo.Documents = inductionController.GetInductionDocuments(siteInductionInfo.Type, parameterProjectId);
            siteInductionInfo.YesNoQAs = inductionController.GetInductionYesNoQAs(siteInductionInfo.Type, parameterProjectId);
            siteInductionInfo.OptinalQAs = inductionController.GetInductionOptinalQAs(siteInductionInfo.Type, parameterProjectId);
            siteInductionInfo.Note = inductionController.GetInductionNote(siteInductionInfo.Type, parameterProjectId);



            if (siteInductionInfo.Documents != null)
                gvSiteInductionDocuments.DataSource = siteInductionInfo.Documents;
            gvSiteInductionDocuments.DataBind();

            if (siteInductionInfo.OptinalQAs != null)
                gvOptlQA.DataSource = siteInductionInfo.OptinalQAs;
            gvOptlQA.DataBind();

            if (siteInductionInfo.YesNoQAs != null)
                gvYesNoQA.DataSource = siteInductionInfo.YesNoQAs;
            gvYesNoQA.DataBind();

            if (siteInductionInfo.Note != null)
                  txtNote.Text = siteInductionInfo.Note.Note;
        }
        #endregion

        #region Event Handler
        protected void Page_Load(object sender, EventArgs e)
        {
            siteInductionInfo = new SiteInductionInfo();
            inductionController = InductionController.GetInstance();               

            try
            {

                Security.CheckAccess(Security.userActions.CreateProject);
                parameterProjectId = int.Parse(Utils.CheckParameter("ProjectId"));
                projectInfo = ProjectsController.GetInstance().GetProject(parameterProjectId);
                if (!IsPostBack)
                {
                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {

                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void btnDocsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (sfsSiteInductionDocs.FilePath != null)
                {
                    InductionDocumentsInfo inductionDocumentsInfo = new InductionDocumentsInfo();

                    inductionDocumentsInfo.FileName = Path.GetFileName(sfsSiteInductionDocs.FilePath);      //Using system.io
                    inductionDocumentsInfo.FilePath = sfsSiteInductionDocs.FilePath;
                    inductionDocumentsInfo.projectId = parameterProjectId; //new ProjectInfo(parameterProjectId);
                    inductionDocumentsInfo.Type = siteInductionInfo.Type;
                    inductionController.AddInductionDocuments(inductionDocumentsInfo);


                    Page.Response.Redirect(Page.Request.Url.ToString(), true);

                }
                else Page.Response.Write("<script>alert('Please  select file...'); </script>");

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }


        }

        protected void gvSiteInductionDocuments_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int Id = int.Parse(gvSiteInductionDocuments.Rows[e.RowIndex].Cells[0].Text);
            InductionDocumentsInfo GIinfo = new InductionDocumentsInfo(Id);
            inductionController.DeleteInductionDocuments(GIinfo);

            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void BtnOptlQA_Click(object sender, EventArgs e)
        {
            OptionalQAInfo optlQA = new OptionalQAInfo();

            try
            {
                if (TxtQuestion.Text != "" && txtA.Text != "" && txtB.Text != "" && txtC.Text != "" && txtD.Text != "" && dpdRightA.SelectedIndex > 0)
                {

                    optlQA.Question = TxtQuestion.Text;
                    optlQA.Opt1 = txtA.Text;
                    optlQA.Opt2 = txtB.Text;
                    optlQA.Opt3 = txtC.Text;
                    optlQA.Opt4 = txtD.Text;
                    optlQA.RightAnswer = dpdRightA.SelectedItem.Value;
                    optlQA.Type = siteInductionInfo.Type;
                    optlQA.projectId = parameterProjectId;// new ProjectInfo(parameterProjectId);

                    inductionController.AddInductionOptinalQA(optlQA);

                    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                }
                else Response.Write("<script>alert('Please enter and select values for all the fields in Optional Question and Answers section'); </script>");

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }


        }

        protected void gvOptlQA_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int Id = int.Parse(gvOptlQA.Rows[e.RowIndex].Cells[0].Text);
            OptionalQAInfo optlQAInfo = new OptionalQAInfo(Id);
            inductionController.DeleteInductionOptionalQA(optlQAInfo);

            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        protected void BtnYesNo_Click(object sender, EventArgs e)
        {
            YesNoQAInfo ynQAInfo = new YesNoQAInfo();
            try
            {
                if (TxtYesNo.Text != "")
                {
                    ynQAInfo.Question = TxtYesNo.Text;
                    ynQAInfo.Comments = TxtComments.Text;
                    ynQAInfo.Type = siteInductionInfo.Type;
                    ynQAInfo.projectId = parameterProjectId;// new ProjectInfo(parameterProjectId);
                    inductionController.AddInductionYesNoQA(ynQAInfo);

                    Page.Response.Redirect(Page.Request.Url.ToString(), true);

                }
                else Response.Write("<script>alert('Please enter a values for all the fields'); </script>");
            }

            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvYesNoQA_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int Id = int.Parse(gvYesNoQA.Rows[e.RowIndex].Cells[0].Text);
            YesNoQAInfo yesNoQAInfo = new YesNoQAInfo(Id);
            inductionController.DeleteInductionYesNoQA(yesNoQAInfo);

            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }



        protected void btnNote_Click(object sender, EventArgs e)
        {

            try
            {
                
                siteInductionInfo.Note = inductionController.GetInductionNote(siteInductionInfo.Type, parameterProjectId.Value);
               

                if (siteInductionInfo.Note.Id == null && txtNote.Text != "")
                {
                    InductionNoteInfo inNoteInfo = new InductionNoteInfo();
                    inNoteInfo.Note = txtNote.Text;
                    inNoteInfo.Type = siteInductionInfo.Type;
                    inNoteInfo.projectId = parameterProjectId;// new ProjectInfo(parameterProjectId);
                    inductionController.AddInductionNote(inNoteInfo);

                    Page.Response.Redirect(Page.Request.Url.ToString(), true);

                }

                else if (siteInductionInfo.Note.Id != null)
                {
                    siteInductionInfo.Note.Note = txtNote.Text;
                    inductionController.UpdateInductionNote(siteInductionInfo.Note);

                    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                }
                else Response.Write("<script>alert('Please enter a valid values...'); </script>");
            }

            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        #endregion



    }
}