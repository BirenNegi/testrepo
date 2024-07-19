using System;

using SOS.Core;

namespace SOS.Web
{
    public partial class ShowMinutesPage : System.Web.UI.Page
    {

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ContractsController contractsController = ContractsController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            TradeInfo tradeInfo = null;
            String parameterTradeId;
            Byte[] pdfData = null;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetDeepTrade(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");
                tradeInfo.Project = projectsController.GetProjectWithDrawings(tradeInfo.Project.Id);

               //---#--26/10/2022--- pdfData = UI.Utils.HtmlToPDF(contractsController.MergeTemplatePrint(tradeInfo, tradesController.GetMinutesTemplate(1).Template), contractsController.GetTemplateFooterText(tradesController.GetMinutesTemplate(1).Template));


                if (tradeInfo.Project.BusinessUnitName == "QLD2")
                {
                    pdfData = UI.Utils.HtmlToPDF(contractsController.MergeTemplatePrint(tradeInfo, tradesController.GetMinutesTemplate(4).Template), contractsController.GetTemplateFooterText(tradesController.GetMinutesTemplate(4).Template));

                }
                else
                {
                    pdfData = UI.Utils.HtmlToPDF(contractsController.MergeTemplatePrint(tradeInfo, tradesController.GetMinutesTemplate(5).Template), contractsController.GetTemplateFooterText(tradesController.GetMinutesTemplate(5).Template));


                }

                //------#----26/10/2022----------------




            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Utils.SendPdfData(pdfData, "Minutes");
        }
#endregion
        
    }
}
