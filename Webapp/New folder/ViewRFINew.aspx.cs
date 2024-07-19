using System;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

using Client = SOS.FileTransferService.Client;



namespace SOS.Web
{
    public partial class ViewRFIPage : SOSPage
    {

#region Members
		private RFIInfo rFIInfo = null;
        private RFIsResponseInfo rFIResponse = null;
        private int SelectedRowindex;
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

            return currentNode;
        }

		private void BindTree(XmlNode xmlNode, TreeView treeView)
		{
			treeView.Nodes.Clear();
			treeView.Nodes.Add(new TreeNode());
			Utils.AddNode(xmlNode, treeView.Nodes[0]);
			treeView.Visible = true;
		}
		
		private void BindRFI()
		{
            ProcessController processController = ProcessController.GetInstance();

            //if (Security.ViewAccess(Security.userActions.EditRFI) && processController.AllowEditCurrentUser(rFIInfo))
            //{
            //    pnlEdit.Visible = true;
            //    phSendEmail.Visible = true;
            //}

            if (rFIInfo.Status == "R")
                BtnReply.Enabled = false;
            else
                BtnReply.Enabled = true;

            lblNumber.Text = UI.Utils.SetFormInteger(rFIInfo.Number);
            lblSubject.Text = UI.Utils.SetFormString(rFIInfo.Subject);
            lblRaiseDate.Text = UI.Utils.SetFormDate(rFIInfo.RaiseDate);
            lblTargetAnswerDate.Text = UI.Utils.SetFormDate(rFIInfo.TargetAnswerDate);
            lblActualAnswerDate.Text = UI.Utils.SetFormDate(rFIInfo.ActualAnswerDate);
            lblStatus.Text = UI.Utils.SetFormString(Utils.GetConfigListItemName("RFI", "Status", rFIInfo.Status));
            //lblReferenceFileName.Text = UI.Utils.SetFormString(rFIInfo.ReferenceFile);
            // lblClientResponseFile.Text = UI.Utils.SetFormString(rFIInfo.ClientResponseFile);



            if (rFIResponse != null) {
                txtDescription.Text = UI.Utils.SetFormString(rFIResponse.Responsemessage);
                if (rFIResponse.ResponseFolderPath.Length > 0)
                { 
                    attachmentrow.Visible = true;

                   listFiles(rFIResponse.ResponseFolderPath);

                }
                    
                else
                    attachmentrow.Visible = false;
            }
            else
            {
             txtDescription.Text = UI.Utils.SetFormString(rFIInfo.Description);
                attachmentrow.Visible = false;
            }

            HlReferenceFile.NavigateUrl= String.Format("~/Modules/Projects/ShowRFIFile.aspx?FileType={0}&RFIId={1}", RFIInfo.FileTypeReference, rFIInfo.IdStr);
                                    

            if (rFIInfo.Signer != null)
                lblSigner.Text = UI.Utils.SetFormString(rFIInfo.Signer.Name);

            XmlDocument xmlDocument = ProjectsController.GetInstance().CheckRFI(rFIInfo);
            if (xmlDocument.DocumentElement != null)
            {
                BindTree(xmlDocument.DocumentElement.ChildNodes[0], TreeViewMissingFields);
                pnlErrors.Visible = true;
                lblRFI.Visible = true;
                lblSendEmail.Visible = true;
            }
            else
            {
                lnkRFI.Visible = true;
                lnkRFI.NavigateUrl = "~/Modules/Projects/ShowRFI.aspx?RFIId=" + rFIInfo.IdStr;
                

                if (rFIInfo.Status != null && rFIInfo.Status == RFIInfo.StatusNew)
                    cmdSendEmail.Visible = true;
                else
                    lblSendEmail.Visible = true;
            }
        }


        private void listFiles(string path)
        {
            HyperLink Hl;
            Image Img;
            DirectoryInfo Di = new DirectoryInfo(Server.MapPath(path));
            if (Di.Exists)
            {
                foreach (FileInfo file in Di.GetFiles())
                {
                    Img = new Image();
                    Img.ImageUrl = "~//Images//IconDocument.gif";
                    Img.Height=15;
                    Img.Width = 16;
                    Hl = new HyperLink();
                    Hl.Text = file.Name;
                    Hl.Target = "_blank";
                    Hl.NavigateUrl = "~"+ path + "\\" + Path.GetFileName(file.FullName); //(new Uri(file.FullName)).ToString();
                    Hl.CssClass = "frmLink";
                    
                    AttachmentCell.Controls.Add(new LiteralControl("<br />")); 
                    AttachmentCell.Controls.Add(Img);
                    AttachmentCell.Controls.Add(new LiteralControl("&nbsp;"));
                    AttachmentCell.Controls.Add(Hl);
                    AttachmentCell.Controls.Add(new LiteralControl("<br />"));
                }
                AttachmentCell.Controls.Add(new LiteralControl("<br />"));
            }


        }


        private int HighlightSelectedRow(string ResponseId)
        {
            int i=0;
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (ResponseId == row.Cells[1].Text)
                {
                    break;
                }
                i++;
            }

           return i; 
        }

#endregion

        #region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
			String parameterRFIId, parameterResponseId;
			ProjectsController projectsController = ProjectsController.GetInstance();

			try
			{
				Security.CheckAccess(Security.userActions.ViewRFI);
                parameterRFIId = Utils.CheckParameter("RFIId");
				rFIInfo = projectsController.GetRFI(Int32.Parse(parameterRFIId));
				Core.Utils.CheckNullObject(rFIInfo, parameterRFIId, "RFI");
				rFIInfo.Project = projectsController.GetProject(rFIInfo.Project.Id);

                int row=0,responseid=0;
                if (HttpContext.Current.Request.Params["ResponseId"] != null)
                {    parameterResponseId = Utils.CheckParameter("ResponseId");
                     rFIResponse = projectsController.GetRFIResponse(Int32.Parse(parameterResponseId));
                     responseid =int.Parse(parameterResponseId);

                }

               
                //San-----
                SqlDataSource1.SelectCommand = @"
                        select P.FirstName +' '+P.LastName as [From],RaiseDate,RFIId,Number,ResponseNumber,ResponseId
                        from (  
                                    Select CreatedPeopleid, RaiseDate, RFIId, Number,'0' as ResponseNumber,'0' as ResponseId from RFIS  where RFIId = '" + parameterRFIId + @"' and Number = "+ rFIInfo.Number+ @"
                                    Union
                                    Select S.ResponseFrom,ResponseDate,R.RFIId,S.RFINumber,s.ResponseNumber,ResponseId 
                                    From  RFIs R  join [RFIsResponse] S on R.RFIId = S.RFIId and R.Number = S.RFINumber
                                    Where R.RFIId = '" + parameterRFIId + @"' and R.Number = " + rFIInfo.Number + @"
	                           )A
                         Inner Join people P on P.PeopleId = A.CreatedPeopleid
                         order by ResponseNumber desc";
                GridView1.DataSource = SqlDataSource1;
                GridView1.DataBind();
                lblSubject.Text = rFIInfo.Subject;


                //San----

                if (!Page.IsPostBack)
                {
                    BindRFI();
                    row = HighlightSelectedRow(responseid.ToString());
                    //GridView1.Rows[row].BorderColor = System.Drawing.Color.Red;
                    GridView1.Rows[row].CssClass = "SelectedRow";
                }



            }
			catch (Exception Ex)
			{
				Utils.ProcessPageLoadException(this, Ex);
			}			
        }

		protected void cmdEdit_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Modules/Projects/EditRFI.aspx?RFIId=" + rFIInfo.IdStr);
		}

        protected void cmdSendEmail_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                ProcessController.SendRFIToDistributionList(rFIInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ListRFIs.aspx?ProjectId=" + rFIInfo.Project.IdStr);
        }
        #endregion

        protected void BtnReply_Click(object sender, EventArgs e)
        {
             Response.Redirect("~/Modules/Projects/ReplyRFI.aspx?RFIId=" + rFIInfo.IdStr);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           SelectedRowindex = GridView1.SelectedRow.RowIndex;
            string ResponseId = GridView1.SelectedRow.Cells[1].Text;
            string RFIID = GridView1.SelectedRow.Cells[2].Text;
            //GridView1.SelectedRow.BackColor = System.Drawing.Color.LightGray;
            Response.Redirect("ViewRFI.aspx?RFIId=" + RFIID + "&ResponseId=" + ResponseId);
           
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
                       
            }
        }
    }
}
