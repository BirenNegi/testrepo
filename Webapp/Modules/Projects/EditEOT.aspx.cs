using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditEOTPage : SOSPage
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

        private void ObjectsToForm()
        {
			if (eOTInfo.Id == null)
			{
				TitleBar.Title = "Adding EOT";
				cmdUpdateTop.Text = "Save";
				cmdUpdateBottom.Text = "Save";
			}

            lblNumber.Text = UI.Utils.SetFormInteger(eOTInfo.Number);
            txtDaysClaimed.Text = UI.Utils.SetFormFloat(eOTInfo.DaysClaimed);
            txtDaysApproved.Text = UI.Utils.SetFormFloat(eOTInfo.DaysApproved);
            txtCostCode.Text = UI.Utils.SetFormString(eOTInfo.CostCode);
            sdrStartDate.Date = eOTInfo.StartDate;
            sdrEndDate.Date = eOTInfo.EndDate;
            sdrFirstNoticeDate.Date = eOTInfo.FirstNoticeDate;
            sdrWriteDate.Date = eOTInfo.WriteDate;
            sdrSendDate.Date = eOTInfo.SendDate;
            sdrApprovalDate.Date = eOTInfo.ApprovalDate;
            txtCause.Text = UI.Utils.SetFormString(eOTInfo.Cause);
            txtNature.Text = UI.Utils.SetFormString(eOTInfo.Nature);
            txtPeriod.Text = UI.Utils.SetFormString(eOTInfo.Period);
            txtWorks.Text = UI.Utils.SetFormString(eOTInfo.Works);
             //#---
            dpdType.SelectedValue = UI.Utils.SetFormString(eOTInfo.TypeofEot);
            sfsBackupFile.FilePath = eOTInfo.ClientBackuplFile;
            sfsBackupFile.Path = eOTInfo.Project.AttachmentsFolder;
             //#---

            sfsClientApprovalFile.FilePath = eOTInfo.ClientApprovalFile;
            sfsClientApprovalFile.Path = eOTInfo.Project.AttachmentsFolder;
        }

        private void FormToObjects()
        {
            eOTInfo.DaysClaimed = UI.Utils.GetFormFloat(txtDaysClaimed.Text);
            eOTInfo.DaysApproved = UI.Utils.GetFormFloat(txtDaysApproved.Text);
            eOTInfo.CostCode = UI.Utils.GetFormString(txtCostCode.Text);
            eOTInfo.StartDate = sdrStartDate.Date;
            eOTInfo.EndDate = sdrEndDate.Date;
            eOTInfo.FirstNoticeDate = sdrFirstNoticeDate.Date;
            eOTInfo.WriteDate = sdrWriteDate.Date;
            eOTInfo.SendDate = sdrSendDate.Date;
            eOTInfo.ApprovalDate = sdrApprovalDate.Date;
            eOTInfo.ClientApprovalFile = sfsClientApprovalFile.FilePath;
            eOTInfo.Cause = UI.Utils.GetFormString(txtCause.Text);
            eOTInfo.Nature = UI.Utils.GetFormString(txtNature.Text);
            eOTInfo.Period = UI.Utils.GetFormString(txtPeriod.Text);
            eOTInfo.Works = UI.Utils.GetFormString(txtWorks.Text);
             //#--
            eOTInfo.TypeofEot= UI.Utils.GetFormString(dpdType.SelectedItem.Value);
            eOTInfo.ClientBackuplFile = sfsBackupFile.FilePath;
            //#

        }
#endregion

#region Event Handlers
		protected void Page_Load(object sender, EventArgs e)
		{
            String parameterEOTId;
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.EditEOT);
                parameterEOTId = Request.Params["EOTId"];
                if (parameterEOTId == null)
                {
                    String parameterProjectId = Utils.CheckParameter("ProjectId");
                    ProjectInfo projectInfo = projectsController.GetProjectWithEOTs(Int32.Parse(parameterProjectId));
                    Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");
                    eOTInfo = projectsController.InitializeEOT(projectInfo);
                }
                else
                {
                    eOTInfo = projectsController.GetEOT(Int32.Parse(parameterEOTId));
                    Core.Utils.CheckNullObject(eOTInfo, parameterEOTId, "EOT");
                    eOTInfo.Project = projectsController.GetProjectWithEOTs(eOTInfo.Project.Id);
                }

                processController.CheckEditCurrentUser(eOTInfo);

              

                if (!Page.IsPostBack)
                    ObjectsToForm();


                //#---

                if (dpdType.SelectedValue == "NOD")
                {
                    txtDaysClaimed.Text = null;
                    txtDaysClaimed.ReadOnly = true;
                }
                else
                    txtDaysClaimed.ReadOnly = false;

                //#---

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
		
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    FormToObjects();
                    
                    //eOTInfo.SendDate = null;
					eOTInfo.Id = ProjectsController.GetInstance().AddUpdateEOT(eOTInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
				Response.Redirect("~/Modules/Projects/ViewEOT.aspx?EOTId=" + eOTInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
			if (eOTInfo.Id == null)
				Response.Redirect("~/Modules/Projects/ListEOTs.aspx?ProjectId=" + eOTInfo.Project.IdStr);
			else
				Response.Redirect("~/Modules/Projects/ViewEOT.aspx?EOTId=" + eOTInfo.IdStr);
		}
        #endregion

        protected void dpdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dpdType.SelectedValue == "NOD")
            {
                txtDaysClaimed.Text = null;
                txtDaysClaimed.ReadOnly = true;
            }
            else
                txtDaysClaimed.ReadOnly = false;
        }
    }
}