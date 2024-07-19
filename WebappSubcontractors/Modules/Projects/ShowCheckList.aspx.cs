using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Reporting.WebForms;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowCheckListPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            TradeParticipationInfo tradeParticipationInfo = null;
            String parameterParticipationId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipationSubContractor);
                parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = tradesController.GetTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Participation");
                contractsController.CheckViewCurrentUser(tradeParticipationInfo);
                pdfReport = projectsController.GenerateCheckListReport(tradeParticipationInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfReport, "CheckList");
        }
#endregion

    }
}
