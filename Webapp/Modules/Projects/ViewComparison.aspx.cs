using System;
using System.Xml;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;
using System.Linq;//#---

namespace SOS.Web
{
    public partial class ViewComparisonPage : SOSPage
    {

#region Members
        private TradeInfo tradeInfo = null;
        private Boolean allowEdit = false;
        private String comparisonType = null;
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

            tempNode.ParentNode.ParentNode.Title = tradeInfo.Project.Name + (comparisonType == Info.TypeProposal ? " (Proposal)" : "");
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeInfo.Project.IdStr;

            return currentNode;
        }

        private void BindTrade()
        {
            TradeParticipationInfo tradeParticipation = new TradeParticipationInfo();


            //#-----To move all pulled out subcontractors to rightend in comparison. It is sorted by IsPulledOut,ID

           // tradeInfo.Participations.Sort((x, y) => x.IsPulledOut.ToString().CompareTo(y.IsPulledOut.ToString()) + x.Id.Value.CompareTo(y.Id.Value)); //+  x.StatusName.CompareTo(y.StatusName)

            tradeInfo.Participations.Sort((x, y) =>
            {
                var ret = x.IsPulledOut.ToString().CompareTo(y.IsPulledOut.ToString());
                if (ret == 0) ret = x.Id.Value.CompareTo(y.Id.Value);
                return ret;
            });

            //#------




            tradeParticipation.Trade = tradeInfo;

            if (comparisonType == Info.TypeProposal)
                pnlProposal.CssClass = "PanelProposal";

            ddlParticipants.Items.Add(new ListItem("Budget/BOQ", tradeInfo.BudgetParticipation.IdStr));

            foreach (TradeParticipationInfo tradeParticipationInfo in tradeInfo.SubcontractorsParticipations)
                ddlParticipants.Items.Add(new ListItem(tradeParticipationInfo.SubContractor.Name, tradeParticipationInfo.IdStr));

            ViewComparison1.TradeParticipation = tradeParticipation;
            ViewComparison1.AllowEdit = allowEdit;
            ViewComparison1.ComparisonType = comparisonType;
            ViewComparison1.RePaint = true;

            CheckComparison1.Trade = tradeInfo;
            CheckComparison1.TradeParticipationType = comparisonType;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                comparisonType = Utils.CheckParameter("Type");
                parameterTradeId = Utils.CheckParameter("TradeId");

                if (comparisonType == Info.TypeActive)
                    tradeInfo = tradesController.GetTradeWithItemsAndParticipationsActive(Int32.Parse(parameterTradeId));
                else if (comparisonType == Info.TypeProposal)
                    tradeInfo = tradesController.GetTradeWithItemsAndParticipationsProposal(Int32.Parse(parameterTradeId));
                else
                    throw new Exception("Invalid participation type");

                tradeInfo.TradeBudgets = tradesController.GetTradeBudgets(tradeInfo);
                tradesController.SetBudgetParticipationAmount(tradeInfo);
 
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTrade))
                        allowEdit = processController.AllowEditCurrentUser(tradeInfo);
                        if (allowEdit)
                            pnlEdit.Visible = true;

                    BindTrade();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void ddlParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParticipants.SelectedValue != "")
                Response.Redirect(String.Format("~/Modules/Projects/EditComparison.aspx?ParticipationId={0}", ddlParticipants.SelectedValue));
        }
#endregion

    }
}
