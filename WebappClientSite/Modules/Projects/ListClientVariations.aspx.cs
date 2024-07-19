using System;
using System.Web;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListClientVariationsPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo;
        private String cVtype;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.Title = cVtype == ClientVariationInfo.VariationTypeClient ? "Client Variations" : "Separate Accounts";

            tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        private void BindForm()
        {
            if (cVtype == ClientVariationInfo.VariationTypeClient)
            {
                Title = "Client Variations";
                TitleBar1.Title = "Client Variations";
                gvClientVariations.Columns[11].HeaderText = "CV";
            }
            else
            {
                Title = "Separate Accounts";
                TitleBar1.Title = "Separate Accounts";
                gvClientVariations.Columns[6].Visible = false;
                gvClientVariations.Columns[11].HeaderText = "SA";
            }

            lnkAddNew.NavigateUrl = "~/Modules/Projects/EditClientVariation.aspx?ProjectId=" + projectInfo.IdStr + "&Type=" + cVtype;
            //San--As per alex it should display only submited,Approved,To be approved status variations
            
            gvClientVariations.DataSource = projectInfo.ClientVariations.FindAll(x => x.Status !="To be Issued");// .Find(x => x.Status != ClientVariationInfo.StatusTobeIssued && x.Status!=ClientVariationInfo.StatusCancelled);
            gvClientVariations.DataBind();
        }

        protected String InfoStatus(ClientVariationInfo clientVariationInfo)
        {
            ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(clientVariationInfo.Process);
            return processStepInfo != null ? processStepInfo.Name : String.Empty;
        }

        protected String DateStatus(ClientVariationInfo clientVariationInfo)
        {
            ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(clientVariationInfo.Process);
            return processStepInfo != null ? UI.Utils.SetFormDate(processStepInfo.ActualDate) : String.Empty;
        }

        protected String LinkClientVariation(ClientVariationInfo clientVariationInfo)
        {
            if (clientVariationInfo.IsInternallyApproved)
                return "~/Modules/Projects/ShowClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr;
            else
                return null;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            
            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                String parameterProjectId = Utils.CheckParameter("ProjectId");

                String parameterType = Utils.CheckParameter("Type");
                cVtype = parameterType;
                if (cVtype != ClientVariationInfo.VariationTypeClient && cVtype != ClientVariationInfo.VariationTypeSeparateAccounts)
                    throw new Exception("Invalid Client Variation Type.");

                projectInfo = projectsController.GetProjectWithClientVariations(Int32.Parse(parameterProjectId), parameterType);

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                //if (processController.AllowAddClientVariationCurrentUser(projectInfo))
                //    phAddNew.Visible = true;
                
                if (!Page.IsPostBack)
                    BindForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
       
    }
}