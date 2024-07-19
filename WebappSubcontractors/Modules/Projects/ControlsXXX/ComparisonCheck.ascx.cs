using System;
using System.Xml;
using System.Web.UI.WebControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ComparisonCheckControl : System.Web.UI.UserControl
    {

#region Members
        private TradeInfo tradeInfo = null;
        private TradeParticipationInfo tradeParticipationInfo = null;
        private String tradeParticipationType = null;
        private Boolean comparisonOk = false;
        private Boolean ignoreRankAssignment = false;
#endregion

#region Public properties
        public TradeInfo Trade
        {
            get { return tradeInfo; }
            set { tradeInfo = value; }
        }

        public TradeParticipationInfo TradeParticipation
        {
            get { return tradeParticipationInfo; }
            set { tradeParticipationInfo = value; }
        }

        public String TradeParticipationType
        {
            get { return tradeParticipationType; }
            set { tradeParticipationType = value; }
        }

        public Boolean ComparisonOk
        {
            get { return comparisonOk; }
            set { comparisonOk = value; }
        }

        public Boolean IgnoreRankAssignment
        {
            get { return ignoreRankAssignment; }
            set { ignoreRankAssignment = value; }
        }
#endregion

#region Public Methods
        public void CheckComparision()
        {
            TreeView1.Nodes[0].ChildNodes.Clear();
            comparisonOk = false;

            if (Trade != null)
            {
                XmlDocument xmlDocument = TradesController.GetInstance().CheckTrade(Trade, TradeParticipation, IgnoreRankAssignment);

                if (xmlDocument.DocumentElement != null)
                    AddNode(xmlDocument.DocumentElement, TreeView1.Nodes[0]);
                else
                    comparisonOk = true;
            }
        }
#endregion

#region Private Methods
        private void AddNode(XmlNode xmlNode, TreeNode treeNode)
        {
            if (xmlNode.HasChildNodes)
            {
                switch (xmlNode.Name)
                {
                    case "errors":
                        treeNode.Text = "Comparison Check";
                        break;
                    case "column":
                        treeNode.Text = xmlNode.Attributes["name"].Value;
                        treeNode.NavigateUrl = "~/Modules/Projects/EditComparison.aspx?ParticipationId=" + xmlNode.Attributes["id"].Value;
                        treeNode.Expanded = false;
                        break;
                    case "error":
                        treeNode.Text = xmlNode.Attributes["text"].Value;
                        treeNode.Expanded = false;
                        treeNode.SelectAction = TreeNodeSelectAction.None;
                        break;
                }

                for (int i = 0; i <= xmlNode.ChildNodes.Count - 1; i++)
                {
                    treeNode.ChildNodes.Add(new TreeNode());
                    AddNode(xmlNode.ChildNodes[i], treeNode.ChildNodes[i]);
                }
            }
            else
            {
                switch (xmlNode.Name)
                {
                    case "errorBudget":
                        treeNode.Text = xmlNode.Attributes["text"].Value;
                        treeNode.NavigateUrl = "~/Modules/Projects/EditProjectTrade.aspx?TradeId=" + xmlNode.Attributes["id"].Value;
                        break;
                    case "error":
                        treeNode.Text = xmlNode.Attributes["text"].Value;
                        treeNode.NavigateUrl = "~/Modules/Projects/EditParticipation.aspx?ParticipationId=" + xmlNode.Attributes["id"].Value;
                        break;
                    case "item":
                        treeNode.Text = xmlNode.Attributes["name"].Value;
                        treeNode.NavigateUrl = "~/Modules/Projects/EditParticipationItem.aspx?ParticipationItemId=" + xmlNode.Attributes["id"].Value;
                        break;
                    case "errorGeneral":
                        treeNode.Text = xmlNode.Attributes["text"].Value;
                        treeNode.NavigateUrl = "~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeInfo.IdStr;
                        break;
                }
            }
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckComparision();
        }
#endregion

    }
}
