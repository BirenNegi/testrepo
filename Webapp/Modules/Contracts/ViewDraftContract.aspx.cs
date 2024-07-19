using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SOS.Core;
using System.Text;

namespace SOS.Web
{
    public partial class ViewDraftContract :SOSPage
    {
        #region Members
        private TradeInfo tradeInfo = null;
        private ContractTemplateInfo contractTemplateInfo = null;
        #endregion

        #region Private Methods
        //protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        //{
        //    //SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
        //    //SiteMapNode tempNode = currentNode;

        //    //if (tradeInfo == null)
        //    //    return null;

        //    //tempNode.ParentNode.Title = tradeInfo.Name;
        //    //tempNode.ParentNode.Url = tempNode.ParentNode.Url + "?TradeId=" + tradeInfo.IdStr;

        //    //tempNode.ParentNode.ParentNode.Title = tradeInfo.Project.Name;
        //    //tempNode.ParentNode.ParentNode.Url = tempNode.ParentNode.ParentNode.Url + "?ProjectId=" + tradeInfo.Project.IdStr;

        //    //return currentNode;
        //}

        #endregion


        #region EventHandlers

        protected void Page_Load(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            ContractsController contractsController = ContractsController.GetInstance();
            String parameterTradeId;

            try
            {
                Security.CheckAccess(Security.userActions.EditContract);

                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetDeepTradeActive(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");

              
                tradeInfo.Project = projectsController.GetProjectWithDrawingsActive(tradeInfo.Project.Id);
                tradeInfo.Project.Trades = new List<TradeInfo>();
                tradeInfo.Project.Trades.Add(tradeInfo);
                tradeInfo.Process.Project = tradeInfo.Project;

                tradeInfo.Contract = new ContractInfo();
                tradeInfo.Contract.Trade = tradeInfo;
                tradeInfo.Contract.WriteDate = DateTime.Today;
                tradeInfo.Contract.GoodsServicesTax = Decimal.Parse(Web.Utils.GetConfigListItemValue("Global", "Settings", "GST"));
                contractTemplateInfo = contractsController.GetContractTemplate(tradeInfo);



                if (contractTemplateInfo != null)
                    tradeInfo.Contract.Template = contractTemplateInfo.StandardTemplate;// contractsController.ViewTemplate(contractTemplateInfo.StandardTemplate);// 

                tradeInfo.Contract.Template= contractsController.MergeDraftTemplate(tradeInfo, tradeInfo.Contract.Template, "View");

                tradeInfo.Contract.Template = contractsController.ViewTemplate(tradeInfo.Contract.Template);// 

                // StringBuilder stringBuilder = new StringBuilder(tradeInfo.Contract.Template);



                //String originalText = contractsController.GetTemplateFooterFull(tradeInfo.Contract.Template);
                //if (originalText != null)
                //    stringBuilder.Replace(originalText, "<span class='TemplateFooter'>" + contractsController.GetTemplateFooterText(tradeInfo.Contract.Template) + "</span>");

                //originalText = contractsController.GetTemplateFinancialFull(tradeInfo.Contract.Template);
                //if (originalText != null)
                //    stringBuilder.Replace(originalText, "<span class='TemplateFinancial'>" + contractsController.GetTemplateFinancialText(tradeInfo.Contract.Template) + "</span>");

                //originalText = contractsController.GetTemplateTermsFull(tradeInfo.Contract.Template);
                //if (originalText != null)
                //    stringBuilder.Replace(originalText, "<span class='TemplateTerms'>" + contractsController.GetTemplateTermsText(tradeInfo.Contract.Template) + "</span>");

                litContract.Text = tradeInfo.Contract.Template.Replace("Building Customers for Life", "Draft Contract Template <br/> <br/> Building Customers for Life");




                if (!Page.IsPostBack)
                {
                   // ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }





        }

        #endregion

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Byte[] Rptpdf = null;
            if (litContract.Text.Length > 0)
                Rptpdf = UI.Utils.HtmlToPDF(litContract.Text, "");
            Utils.SendPdfData(Rptpdf, "DraftContract");
        }
    }
}