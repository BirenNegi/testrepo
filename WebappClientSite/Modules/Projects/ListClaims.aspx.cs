using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ListClaimsPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo;
        private String infoAllow;
        private Decimal? totalClientTradesProject;
        private Decimal? totalClientVariationsProject;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        private void BindForm()
        {
            lnkAddNew.NavigateUrl = "~/Modules/Projects/EditClaim.aspx?ProjectId=" + projectInfo.IdStr;
            //TitleBar1.Info = infoAllow;

            gvClaims.DataSource = projectInfo.Claims.FindAll(x=>x.Status=="Invoice");
            gvClaims.DataBind();
        }

        protected String InfoStatus(ClaimInfo claimInfo)
        {
			if (claimInfo.Process != null)
			{
				ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(claimInfo.Process);
				return processStepInfo != null ? processStepInfo.Name : String.Empty;
			}
			else
			{
				return "No process";
			}
        }

        protected String DateStatus(ClaimInfo claimInfo)
        {
			if (claimInfo.Process != null)
			{
				ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(claimInfo.Process);
				return processStepInfo != null ? UI.Utils.SetFormDate(processStepInfo.ActualDate) : String.Empty;
			}
			else
			{
				return "No Process";
			}
        }

        protected String LinkClaim(ClaimInfo claimInfo)
        {
            if (claimInfo.IsInternallyApproved)
                return "~/Modules/Projects/ShowClaim.aspx?ClaimId=" + claimInfo.IdStr;
            else
                return null;
        }

        public String ClientTradesPercentage(ClaimInfo claimInfo)
        {
            Decimal? totalClaimTrades = claimInfo.TotalClaimTrades;
            Decimal? clientTradesPercentage = null;

            if (totalClientTradesProject != null && totalClientTradesProject != 0 && totalClaimTrades != null)
            {
                clientTradesPercentage = (totalClaimTrades / totalClientTradesProject) * 100;

                if (clientTradesPercentage > 99 && clientTradesPercentage < 100)
                    clientTradesPercentage = 99;

                return UI.Utils.SetFormDecimalNoDecimals(clientTradesPercentage);
            }

            return String.Empty;
        }

        public String ClientVariationsPercentage(ClaimInfo claimInfo)
        {
            Decimal? totalClaimVariations = claimInfo.TotalClaimVariations;
            Decimal? clientVariationsPercentage = null;

            if (totalClientVariationsProject != null && totalClientVariationsProject != 0 && totalClaimVariations != null)
            {
                clientVariationsPercentage = (totalClaimVariations / totalClientVariationsProject) * 100;

                if (clientVariationsPercentage > 99 && clientVariationsPercentage < 100)
                    clientVariationsPercentage = 99;

                return UI.Utils.SetFormDecimalNoDecimals(clientVariationsPercentage);
            }

            return String.Empty;
        }
#endregion

#region Event Handlers
		protected void gvClaims_RowCreated(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.Header)
			{
				GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
				TableCell cell;

				cell = new TableCell();
				cell.Attributes.Add("class", "lstHeaderTop");
				cell.ColumnSpan = 1;
				row.Cells.Add(cell);
				
				cell = new TableCell();
				cell.Attributes.Add("class", "lstHeaderTop");
				cell.Attributes.Add("align", "center");
				cell.Text = "Dates";
				cell.ColumnSpan = 3;
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.Attributes.Add("class", "lstHeaderTop");
				cell.Attributes.Add("align", "center");
				cell.Text = "Amounts";
				cell.ColumnSpan = 7;
				row.Cells.Add(cell);

				//cell = new TableCell();
				//cell.Attributes.Add("class", "lstHeaderTop");
				//cell.Attributes.Add("align", "center");
				//cell.Text = "Process";
				//cell.ColumnSpan = 2;
				//row.Cells.Add(cell);

    //            cell = new TableCell();
    //            cell.Attributes.Add("class", "lstHeaderTop");
    //            row.Cells.Add(cell);

                ((Table)gvClaims.Controls[0]).Rows.Add(row);
			}
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            
            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                String parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithClaimsDeepTradesAndVariations(Int32.Parse(parameterProjectId));
                totalClientTradesProject = projectInfo.TotalClientTrades;
                totalClientVariationsProject = projectInfo.TotalClientVariations;

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                //if (processController.AllowAddClaim(projectInfo, out infoAllow))
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