using System;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewRFIPage : SOSPage
    {

#region Members
		private RFIInfo rFIInfo = null;
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

            if (Security.ViewAccess(Security.userActions.EditRFI) && processController.AllowEditCurrentUser(rFIInfo))
            {
                pnlEdit.Visible = true;
                phSendEmail.Visible = true;
            }

			lblNumber.Text = UI.Utils.SetFormInteger(rFIInfo.Number);
            lblSubject.Text = UI.Utils.SetFormString(rFIInfo.Subject);
            lblRaiseDate.Text = UI.Utils.SetFormDate(rFIInfo.RaiseDate);
            lblTargetAnswerDate.Text = UI.Utils.SetFormDate(rFIInfo.TargetAnswerDate);
            lblActualAnswerDate.Text = UI.Utils.SetFormDate(rFIInfo.ActualAnswerDate);
            lblStatus.Text = UI.Utils.SetFormString(Utils.GetConfigListItemName("RFI", "Status", rFIInfo.Status));
            lblReferenceFileName.Text = UI.Utils.SetFormString(rFIInfo.ReferenceFile);
            lblClientResponseFile.Text = UI.Utils.SetFormString(rFIInfo.ClientResponseFile);

            txtDescription.Text = UI.Utils.SetFormString(rFIInfo.Description);
            txtClientResponseSummary.Text = UI.Utils.SetFormString(rFIInfo.ClientResponseSummary);

            sflReferenceFile.FilePath = rFIInfo.ReferenceFile;
            sflReferenceFile.BasePath = rFIInfo.Project.AttachmentsFolder;
            sflReferenceFile.PageLink = String.Format("~/Modules/Projects/ShowRFIFile.aspx?FileType={0}&RFIId={1}", RFIInfo.FileTypeReference, rFIInfo.IdStr);

            sflClientResponseFile.FilePath = rFIInfo.ClientResponseFile;
            sflClientResponseFile.BasePath = rFIInfo.Project.AttachmentsFolder;
            sflClientResponseFile.PageLink = String.Format("~/Modules/Projects/ShowRFIFile.aspx?FileType={0}&RFIId={1}", RFIInfo.FileTypeResponse, rFIInfo.IdStr);

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
                lnkRFI.NavigateUrl = "~/Modules/Projects/ShowRFI.aspx?RFIId=" + rFIInfo.IdStr;
                lnkRFI.Visible = true;

                //#---
                if (rFIInfo.Status != null && rFIInfo.Status != RFIInfo.StatusNew)
                {
                    lnkNewView.NavigateUrl = "~/Modules/Projects/ViewRFINew.aspx?RFIId=" + rFIInfo.IdStr;// + "&ResponseId=0";
                    lnkNewView.Visible = true;
                }
                else lblNewView.Visible = true;

                //#----
                if (rFIInfo.Status != null && rFIInfo.Status == RFIInfo.StatusNew)
                    cmdSendEmail.Visible = true;
                else
                    lblSendEmail.Visible = true;
            }
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
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
    
    }
}
