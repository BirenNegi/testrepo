using System;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewMinutesPage : SOSPage
    {

#region Members
        private TradeInfo tradeInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeInfo == null)
                return null;

            tempNode.ParentNode.Title = tradeInfo.Name;
            tempNode.ParentNode.Url += "?TradeId=" + tradeInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeInfo.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeInfo.Project.IdStr;

            return currentNode;
        }

        private void BindMinutes()
        {
            //---#--26/20/22--- MinutesTemplateInfo minutesTemplateInfo = TradesController.GetInstance().GetMinutesTemplate(1);
            MinutesTemplateInfo minutesTemplateInfo;

            if (tradeInfo.Project.BusinessUnitName == "QLD2")
            {
                 minutesTemplateInfo = TradesController.GetInstance().GetMinutesTemplate(4);
            }
            else {
                 minutesTemplateInfo = TradesController.GetInstance().GetMinutesTemplate(5);

            }

            //---26/10/22---------
            ContractsController contractsController = ContractsController.GetInstance();

            XmlDocument xmlDocument = contractsController.CheckTradeForTemplate(tradeInfo, minutesTemplateInfo.Template);
            if (xmlDocument.DocumentElement != null)
            {
                Utils.AddNode(xmlDocument.DocumentElement, TreeView1.Nodes[0]);
                pnlErrors.Visible = true;
                lnkPrint.Visible = false;
            }
            else
            {
                lnkPrint.NavigateUrl = "~/Modules/Projects/ShowMinutes.aspx?TradeId=" + tradeInfo.IdStr;
                litMinutes.Text = contractsController.MergeTemplateView(tradeInfo, minutesTemplateInfo.Template);
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                String parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = TradesController.GetInstance().GetDeepTrade(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");
                tradeInfo.Project = ProjectsController.GetInstance().GetProjectWithDrawings(tradeInfo.Project.Id);

                if (!Page.IsPostBack)
                    BindMinutes();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
        
    }
}