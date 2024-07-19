using System;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewInvitationPage : SOSPage
    {

#region Members
        private TradeParticipationInfo tradeParticipationInfo = null;
#endregion

#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeParticipationInfo == null)
                return null;

            tempNode.ParentNode.Title = tradeParticipationInfo.SubContractor.Name;
            tempNode.ParentNode.Url += "?ParticipationId=" + tradeParticipationInfo.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeParticipationInfo.Trade.Name;
            tempNode.ParentNode.ParentNode.Url += "?TradeId=" + tradeParticipationInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.ParentNode.Title = tradeParticipationInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeParticipationInfo.Trade.Project.IdStr;

            return currentNode;
        }

        private void BindInvitation()
        {
            String template = TradesController.GetInstance().GetInvitationTemplate(1).Template;
            ContractsController contractsController = ContractsController.GetInstance();

            XmlDocument xmlDocument = contractsController.CheckTradeForTemplate(tradeParticipationInfo.Trade, template);
            if (xmlDocument.DocumentElement != null)
            {
                Utils.AddNode(xmlDocument.DocumentElement, TreeView1.Nodes[0]);
                pnlErrors.Visible = true;
                lnkPrint.Visible = false;
            }
            else
            {
                lnkPrint.NavigateUrl = "~/Modules/Projects/ShowInvitation.aspx?ParticipationId=" + tradeParticipationInfo.IdStr;
                litInvitation.Text = contractsController.MergeTemplateView(tradeParticipationInfo.Trade, template);
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewParticipation);
                String parameterParticipationId = Utils.CheckParameter("ParticipationId");
                tradeParticipationInfo = TradesController.GetInstance().GetTradeParticipationWithTradeAndProject(Int32.Parse(parameterParticipationId));                
                tradeParticipationInfo.Rank = 1;
                tradeParticipationInfo.PulledOut = false;                
                Core.Utils.CheckNullObject(tradeParticipationInfo, parameterParticipationId, "Trade Subcontractor");

                if (!Page.IsPostBack)
                    BindInvitation();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
#endregion
        
    }
}