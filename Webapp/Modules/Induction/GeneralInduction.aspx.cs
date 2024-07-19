using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using SOS.Core;


namespace SOS.Web
{
    public partial class GeneralInduction : System.Web.UI.Page
    {
        #region Private Members
        private GeneralInductionInfo generalInductionInfo = null;
        private InductionController inductionController = null;

        #endregion

        #region Private Methods
        private void ObjectsToForm()
        {
            List<BusinessUnitInfo> businessUnitInfoList = ContractsController.GetInstance().GetBusinessUnits();

            dpdState.Items.Add(new ListItem(String.Empty, String.Empty));
            foreach (BusinessUnitInfo businessUnitInfo in businessUnitInfoList)
                dpdState.Items.Add(new ListItem(businessUnitInfo.Name, businessUnitInfo.IdStr));


            generalInductionInfo.Documents = inductionController.GetInductionDocuments(generalInductionInfo.Type);
            generalInductionInfo.YesNoQAs = inductionController.GetInductionYesNoQAs(generalInductionInfo.Type);
            generalInductionInfo.OptinalQAs = inductionController.GetInductionOptinalQAs(generalInductionInfo.Type);
            generalInductionInfo.Note = inductionController.GetInductionNote(generalInductionInfo.Type);

            if (generalInductionInfo.Documents != null)
            {
                gvGeneralInductionDocuments.DataSource = generalInductionInfo.Documents;
                gvGeneralInductionDocuments.DataBind();
            }

            if (generalInductionInfo.OptinalQAs != null)
            {
                gvOptlQA.DataSource = generalInductionInfo.OptinalQAs;
                gvOptlQA.DataBind();
                // gvOptlQA.Columns[1].ItemStyle.Width = 55;
                // gvOptlQA.Columns[1].ItemStyle.Wrap = true;
            }

            if (generalInductionInfo.YesNoQAs != null)
            {
                gvYesNoQA.DataSource = generalInductionInfo.YesNoQAs;
                gvYesNoQA.DataBind();
                //gvYesNoQA.Columns[1].ItemStyle.Width = 55;
               // gvYesNoQA.Columns[1].ItemStyle.Wrap = true;
            }
            if (generalInductionInfo.Note != null)
                txtNote.Text = generalInductionInfo.Note.Note.ToString();


        }


        private string getRightAnswer(string selectedValue)
        {
            if (selectedValue == "a")
                return txtA.Text;
            else if (selectedValue == "b")
                return txtB.Text;
            else if (selectedValue == "c")
                return txtC.Text;
            else
                return txtD.Text;
        }
        #endregion

        #region Event Handler
        protected void Page_Load(object sender, EventArgs e)
            {
                generalInductionInfo = new GeneralInductionInfo();   
                inductionController = InductionController.GetInstance();

                //gvOptlQA.Attributes.Add("style", "word-break:break-all; word-wrap:break-word");
                //gvYesNoQA.Attributes.Add("style", "word-break:break-all; word-wrap:break-word");
                //gvGeneralInductionDocuments.Attributes.Add("style", "word-break:break-all; word-wrap:break-word");

            try {

                    Security.CheckAccess(Security.userActions.CreateProject);
                

                    if (!IsPostBack)
                    { 
                        ObjectsToForm();
                    }
                }
                catch(Exception Ex)
                {

                    Utils.ProcessPageLoadException(this, Ex);
                }
            }

            protected void btnDocsAdd_Click(object sender, EventArgs e)
            {
                try
                { 
                        if (sfsGeneralInductionDocs.FilePath != null)
                        {
                            InductionDocumentsInfo inductionDocumentsInfo = new InductionDocumentsInfo();

                            inductionDocumentsInfo.FileName= Path.GetFileName(sfsGeneralInductionDocs.FilePath);      //Using system.io
                            inductionDocumentsInfo.FilePath = sfsGeneralInductionDocs.FilePath;
                            inductionDocumentsInfo.State = dpdState.SelectedItem.Text;
                            inductionDocumentsInfo.Version = txtVersion.Text;
                    inductionDocumentsInfo.projectId = null;// new ProjectInfo();
                            inductionDocumentsInfo.Type = generalInductionInfo.Type;
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

            protected void gvGeneralInductionDocuments_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
            {
                 int Id =int.Parse(gvGeneralInductionDocuments.Rows[e.RowIndex].Cells[0].Text);
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
                    optlQA.RightAnswer =getRightAnswer(dpdRightA.SelectedItem.Value);
                    optlQA.Type = generalInductionInfo.Type;
                    optlQA.projectId = null;// new ProjectInfo();

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
                        ynQAInfo.Type = generalInductionInfo.Type;
                        ynQAInfo.projectId = null;// new ProjectInfo();
                        inductionController.AddInductionYesNoQA(ynQAInfo);

                        ObjectsToForm();
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


        #endregion

         protected void btnNote_Click(object sender, EventArgs e)
        {
           
            try
            {
                generalInductionInfo.Note = inductionController.GetInductionNote(generalInductionInfo.Type);

                if (generalInductionInfo.Note.Id == null && txtNote.Text != "")
                {
                    InductionNoteInfo inNoteInfo = new InductionNoteInfo();
                    inNoteInfo.Note = txtNote.Text;
                    inNoteInfo.Type = generalInductionInfo.Type;
                    inNoteInfo.projectId = null;// new ProjectInfo();
                    inductionController.AddInductionNote(inNoteInfo);

                    Page.Response.Redirect(Page.Request.Url.ToString(), true);

                }

                else if (generalInductionInfo.Note.Id != null)
                {
                    generalInductionInfo.Note.Note = txtNote.Text;
                    inductionController.UpdateInductionNote(generalInductionInfo.Note);

                    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                }
                else Response.Write("<script>alert('Please enter a valid values...'); </script>");
            }

            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void gvOptlQA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //   ((Label)(e.Row.Cells[1].Controls[1])).Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
            //}
        }
    }
}