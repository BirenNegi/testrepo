using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditRFIPage : SOSPage
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

        private void ObjectsToForm()
        {
			if (rFIInfo.Id == null)
			{
				TitleBar.Title = "Adding RFI";
				cmdUpdateTop.Text = "Save";
				cmdUpdateBottom.Text = "Save";
			}

            lblNumber.Text = UI.Utils.SetFormInteger(rFIInfo.Number);
            
            txtSubject.Text = UI.Utils.SetFormString(rFIInfo.Subject);
            txtDescription.Text = UI.Utils.SetFormString(rFIInfo.Description);
            txtClientResponseSummary.Text = UI.Utils.SetFormString(rFIInfo.ClientResponseSummary);

            sdrRaiseDate.Date = rFIInfo.RaiseDate;
            sdrTargetAnswerDate.Date = rFIInfo.TargetAnswerDate;
            sdrActualAnswerDate.Date = rFIInfo.ActualAnswerDate;
            
            Utils.GetConfigListAddEmpty("RFI", "Status", ddlStatus, rFIInfo.Status);

            sfsReferenceFile.FilePath = rFIInfo.ReferenceFile;
            sfsReferenceFile.Path = rFIInfo.Project.AttachmentsFolder;

            sfsClientResponseFile.FilePath = rFIInfo.ClientResponseFile;
            sfsClientResponseFile.Path = rFIInfo.Project.AttachmentsFolder;

            if (rFIInfo.Signer != null)
                lblSigner.Text = UI.Utils.SetFormString(rFIInfo.Signer.Name);
		}

        private void FormToObjects()
        {
            rFIInfo.Subject = UI.Utils.GetFormString(txtSubject.Text);
            rFIInfo.Description = UI.Utils.GetFormString(txtDescription.Text);
            rFIInfo.ClientResponseSummary = UI.Utils.GetFormString(txtClientResponseSummary.Text);

            rFIInfo.RaiseDate = sdrRaiseDate.Date;
            rFIInfo.TargetAnswerDate = sdrTargetAnswerDate.Date;
            rFIInfo.ActualAnswerDate = sdrActualAnswerDate.Date;

            rFIInfo.ReferenceFile = sfsReferenceFile.FilePath;
            rFIInfo.ClientResponseFile = sfsClientResponseFile.FilePath;

            rFIInfo.Status = UI.Utils.GetFormString(ddlStatus.SelectedValue);
		}
#endregion

#region Event Handlers
		protected void Page_Load(object sender, EventArgs e)
		{
            String parameterRFIId;
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                Security.CheckAccess(Security.userActions.EditRFI);
                parameterRFIId = Request.Params["RFIId"];
                if (parameterRFIId == null)
                {
                    String parameterProjectId = Utils.CheckParameter("ProjectId");
                    ProjectInfo projectInfo = projectsController.GetProjectWithRFIs(Int32.Parse(parameterProjectId));
                    Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");
                    rFIInfo = projectsController.InitializeRFI(projectInfo);
                }
                else
                {
                    rFIInfo = projectsController.GetRFI(Int32.Parse(parameterRFIId));
                    Core.Utils.CheckNullObject(rFIInfo, parameterRFIId, "RFI");
                    rFIInfo.Project = projectsController.GetProjectWithRFIs(rFIInfo.Project.Id);
                }

                processController.CheckEditCurrentUser(rFIInfo);

                if (!Page.IsPostBack)
                    ObjectsToForm();
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
					rFIInfo.Id = ProjectsController.GetInstance().AddUpdateRFI(rFIInfo);
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                //#--Response.Redirect("~/Modules/Projects/ViewRFI.aspx?RFIId=" + rFIInfo.IdStr);
                Response.Redirect("~/Modules/Projects/ViewRFINew.aspx?RFIId=" + rFIInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
			if (rFIInfo.Id == null)
				Response.Redirect("~/Modules/Projects/ListRFIs.aspx?ProjectId=" + rFIInfo.Project.IdStr);
			else
                //#--Response.Redirect("~/Modules/Projects/ViewRFI.aspx?RFIId=" + rFIInfo.IdStr);
                Response.Redirect("~/Modules/Projects/ViewRFINew.aspx?RFIId=" + rFIInfo.IdStr);  //#--
        }

#endregion

    }
}