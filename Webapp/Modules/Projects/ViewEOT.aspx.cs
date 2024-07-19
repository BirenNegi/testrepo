using System;
using System.Xml;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewEOTPage : SOSPage
    {

#region Members
		private EOTInfo eOTInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

			if (eOTInfo == null)
				return null;

			tempNode.ParentNode.Url += "?ProjectId=" + eOTInfo.Project.IdStr;

			tempNode.ParentNode.ParentNode.Title = eOTInfo.Project.Name;
			tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + eOTInfo.Project.IdStr;			

            return currentNode;
        }

		private void BindTree(XmlNode xmlNode, TreeView treeView)
		{
			treeView.Nodes.Clear();
			treeView.Nodes.Add(new TreeNode());
			Utils.AddNode(xmlNode, treeView.Nodes[0]);
			treeView.Visible = true;
		}
		
		private void BindEOT()
		{
            ProcessController processController = ProcessController.GetInstance();

            if (Security.ViewAccess(Security.userActions.EditEOT) && processController.AllowEditCurrentUser(eOTInfo))
            {
                pnlEdit.Visible = true;
                phSendEmail.Visible = true;
            }

			lblNumber.Text = UI.Utils.SetFormInteger(eOTInfo.Number);
            lblDaysClaimed.Text = UI.Utils.SetFormFloat(eOTInfo.DaysClaimed);
            lblDaysApproved.Text = UI.Utils.SetFormFloat(eOTInfo.DaysApproved);
            lblCostCode.Text = UI.Utils.SetFormString(eOTInfo.CostCode);
            lblStartDate.Text = UI.Utils.SetFormDate(eOTInfo.StartDate);
            lblEndDate.Text = UI.Utils.SetFormDate(eOTInfo.EndDate);
            lblFirstNoticeDate.Text = UI.Utils.SetFormDate(eOTInfo.FirstNoticeDate);
            lblWriteDate.Text = UI.Utils.SetFormDate(eOTInfo.WriteDate);
            lblSendDate.Text = UI.Utils.SetFormDate(eOTInfo.SendDate);
            
            lblApprovalDate.Text = UI.Utils.SetFormDate(eOTInfo.ApprovalDate);
            //#----
            lblCliecntBackupFile.Text = UI.Utils.SetFormString(eOTInfo.ClientBackuplFile); 
            lblEotType.Text= UI.Utils.SetFormString(eOTInfo.TypeofEot);
            //#---

            lblClientApprovalFile.Text = UI.Utils.SetFormString(eOTInfo.ClientApprovalFile);
            lblCause.Text = UI.Utils.SetFormString(eOTInfo.Cause);
            txtNature.Text = UI.Utils.SetFormString(eOTInfo.Nature);
            txtPeriod.Text = UI.Utils.SetFormString(eOTInfo.Period);
            txtWorks.Text = UI.Utils.SetFormString(eOTInfo.Works);

            sflClientApprovalFile.FilePath = eOTInfo.ClientApprovalFile;
            sflClientApprovalFile.BasePath = eOTInfo.Project.AttachmentsFolder;
            //#----sflClientApprovalFile.PageLink = String.Format("~/Modules/Projects/ShowEOTFile.aspx?EOTId={0}", eOTInfo.IdStr);
            sflClientApprovalFile.PageLink = String.Format("~/Modules/Projects/ShowEOTFile.aspx?EOTId={0}&FileType=ApprovedFile", eOTInfo.IdStr);
            sflClientBackupFile.FilePath = eOTInfo.ClientBackuplFile;
            sflClientBackupFile.BasePath = eOTInfo.Project.AttachmentsFolder;
            sflClientBackupFile.PageLink = String.Format("~/Modules/Projects/ShowEOTFile.aspx?EOTId={0}&FileType=BackupFile", eOTInfo.IdStr);
            //#--



            XmlDocument xmlDocument = ProjectsController.GetInstance().CheckEOT(eOTInfo);
            if (xmlDocument.DocumentElement != null)
            {
                BindTree(xmlDocument.DocumentElement.ChildNodes[0], TreeViewMissingFields);
                pnlErrors.Visible = true;
                lblEOT.Visible = true;
                lblSendEmail.Visible = true;
            }
            else
            {
                if (eOTInfo.DaysApproved > 0) { 
                lnkEOT.NavigateUrl = "~/Modules/Projects/ShowEOT.aspx?EOTId=" + eOTInfo.IdStr + "&NODID="+null;
                lnkEOT.Visible = true;
                }
                if (eOTInfo.SendDate == null)
                    cmdSendEmail.Visible = true;
                else
                    lblSendEmail.Visible = true;
                 //#----
                 //  If EOT has NodEots then lblnod is visible with heading  NOD if the daysApproved is N ull else as EOT
                 //#---



            }
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
			String parameterEOTId;
			ProjectsController projectsController = ProjectsController.GetInstance();

			try
			{
				Security.CheckAccess(Security.userActions.ViewEOT);
                parameterEOTId = Utils.CheckParameter("EOTId");
				eOTInfo = projectsController.GetEOT(Int32.Parse(parameterEOTId));
				Core.Utils.CheckNullObject(eOTInfo, parameterEOTId, "EOT");
				eOTInfo.Project = projectsController.GetProject(eOTInfo.Project.Id);

				if (!Page.IsPostBack)
					BindEOT();
			}
			catch (Exception Ex)
			{
				Utils.ProcessPageLoadException(this, Ex);
			}
        }

		protected void cmdEdit_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Modules/Projects/EditEOT.aspx?EOTId=" + eOTInfo.IdStr);
		}

        protected void cmdSendEmail_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                ProcessController.SendEOTToDistributionList(eOTInfo);
                //--SAN--If NOD(Notice of Delay)or EOT(extension Of Time)  store in database
                eOTInfo.SendDate= DateTime.Now;
                ProjectsController.GetInstance().AddUpdateNODEOT(eOTInfo);
                //---#


            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ListEOTs.aspx?ProjectId=" + eOTInfo.Project.IdStr);
        }
#endregion
    
    }
}
