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
            TradeInfo tradeInfo = null;
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String comparisonType = null;
            String parameterTradeId;
            Byte[] pdfReport = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                comparisonType = Utils.CheckParameter("Type");
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetDeepTrade(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");
                tradeInfo.Project = projectsController.GetProject(tradeInfo.Project.Id);

                pdfReport = projectsController.GenerateCheckListReport(tradeInfo, comparisonType);
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
