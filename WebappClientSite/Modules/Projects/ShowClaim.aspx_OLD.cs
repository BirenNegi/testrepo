using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowClaimPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ClaimInfo claimInfo = null;
            ClaimInfo previousClaim = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            String parameterClaimId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewClaim);
                parameterClaimId = Utils.CheckParameter("ClaimId");
                claimInfo = projectsController.GetClaimWithTradesAndVariations(Int32.Parse(parameterClaimId));
                Core.Utils.CheckNullObject(claimInfo, parameterClaimId, "Project Claim");
                claimInfo.Project = projectsController.GetProjectWithClaimsTradesAndVariations(claimInfo.Project.Id);

                if (claimInfo.ClaimTrades != null)
                    foreach (ClaimTradeInfo claimTrade in claimInfo.ClaimTrades)
                        claimTrade.ClientTrade = claimInfo.Project.ClientTrades.Find(delegate(ClientTradeInfo clientTradeInfoInList) { return clientTradeInfoInList.Equals(claimTrade.ClientTrade); });

                if (claimInfo.ClaimVariations != null)
                    foreach (ClaimVariationInfo claimVariation in claimInfo.ClaimVariations)
                        claimVariation.ClientVariation = claimInfo.Project.ClientVariations.Find(delegate(ClientVariationInfo clientVariationInfoInList) { return clientVariationInfoInList.TheParentClientVariation.Equals(claimVariation.ClientVariation.TheParentClientVariation); });

                previousClaim = projectsController.GetPreviousClaim(claimInfo);

                pdfReport = projectsController.GenerateClaimReport(claimInfo, previousClaim);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "Claim");
        }
#endregion

    }
}
