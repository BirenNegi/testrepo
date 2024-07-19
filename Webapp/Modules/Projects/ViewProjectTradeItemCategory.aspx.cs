using System;
using System.Web;
using System.Xml;
using System.Web.UI.WebControls;
//#
using System.IO;
using System.Web.UI;
//#
using SOS.Core;


namespace SOS.Web
{
    public partial class ViewProjectTradeItemCategoryPage : SOSPage
    {

#region Members
        private TradeItemCategoryInfo tradeItemCategoryInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (tradeItemCategoryInfo == null)
                return null;

            tempNode.Title = tradeItemCategoryInfo.Name;
            tempNode.ParentNode.Title = tradeItemCategoryInfo.Trade.Name;
            tempNode.ParentNode.Url += "?TradeId=" + tradeItemCategoryInfo.Trade.IdStr;

            tempNode.ParentNode.ParentNode.Title = tradeItemCategoryInfo.Trade.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + tradeItemCategoryInfo.Trade.Project.IdStr;

            return currentNode;
        }

        private void BindTradeItems()
        {
            gvTradeItems.DataSource = tradeItemCategoryInfo.TradeItems;
            gvTradeItems.DataBind();
        }

        private void BindTradeItemCategory()
        {
            lnkAddTradeItem.NavigateUrl = "~/Modules/Projects/EditProjectTradeItem.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr;

            lblName.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.Name);
            lblShortDescription.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.ShortDescription);
            lblLongDescription.Text = UI.Utils.SetFormString(tradeItemCategoryInfo.LongDescription);

            BindTradeItems();
        }

        private void BindDeleteErrorsTree(XmlDocument xmlDocument, Boolean deleteError)
        {
            if (deleteError)
            {
                TreeViewDeleteErrors.Nodes.Clear();
                TreeViewDeleteErrors.Nodes.Add(new TreeNode());
                Utils.AddNode(xmlDocument.DocumentElement, TreeViewDeleteErrors.Nodes[0]);
                TreeViewDeleteErrors.ExpandAll();
                pnlDeleteErrors.Visible = true;
            }
            else
                pnlDeleteErrors.Visible = false;
        }
#endregion
        
#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeItemCategoryId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewTrade);
                parameterTradeItemCategoryId = Utils.CheckParameter("TradeItemCategoryId");
                tradeItemCategoryInfo = tradesController.GetDeepTradeItemCategory(Int32.Parse(parameterTradeItemCategoryId));
                Core.Utils.CheckNullObject(tradeItemCategoryInfo, parameterTradeItemCategoryId, "Trade Item Category");

                if (!Page.IsPostBack)
                {
                    if (Security.ViewAccess(Security.userActions.EditTrade))
                    {

                        if (processController.AllowEditCurrentUser(tradeItemCategoryInfo.Trade) || processController.AllowEditTradeItemCategoryCurrentUser(tradeItemCategoryInfo.Trade))  //DS20240206
                        {
                            cmdEditTop.Visible = true;
                            cmdDeleteTop.Visible = true;

                            cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Item Category and All its Items?');");

                            lnkAddTradeItem.Visible = true;   // ds20240207
                            lblMessageBrowse.Visible = true;  // ds20240207
                            ExcelUpload.Visible = true;  // ds20240207
                            btnImport.Visible = true;  // ds20240207

                            // phAddTradeItem.Visible = true;  // ds20240207

                            gvTradeItems.Columns[6].Visible = true;
                            gvTradeItems.Columns[7].Visible = true;
                        }
                    }
                    BindTradeItemCategory();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/EditProjectTradeItemCategory.aspx?TradeItemCategoryId=" + tradeItemCategoryInfo.IdStr);
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            Boolean deleteError = false;

            try
            {
                XmlDocument xmlDocument = tradesController.CheckTradeItemCateforyForDelete(tradeItemCategoryInfo);

                if (xmlDocument.DocumentElement != null)
                    deleteError = true;
                else
                    tradesController.DeleteTradeItemCategory(tradeItemCategoryInfo);

                BindDeleteErrorsTree(xmlDocument, deleteError);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (!deleteError)
                Response.Redirect("~/Modules/Projects/ViewProjectTrade.aspx?TradeId=" + tradeItemCategoryInfo.Trade.IdStr);
        }

        protected void gvTradeItems_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            TradesController tradesController = TradesController.GetInstance();
            int? tradeItemId = (int?)gvTradeItems.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
            TradeItemInfo tradeItemInfo = tradeItemCategoryInfo.TradeItems.Find(delegate(TradeItemInfo tradeItemInfoInList) { return tradeItemInfoInList.Id == tradeItemId; });

            if (e.CommandName == "MoveUp")
                tradesController.ChangeDisplayOrderTradeItem(tradeItemCategoryInfo.TradeItems, tradeItemInfo, true);
            else if (e.CommandName == "MoveDown")
                tradesController.ChangeDisplayOrderTradeItem(tradeItemCategoryInfo.TradeItems, tradeItemInfo, false);

            tradeItemCategoryInfo.TradeItems = tradesController.GetTradeItems(tradeItemCategoryInfo);
            BindTradeItems();
        }
        #endregion


        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }
        protected void btnImport_Click(object sender, EventArgs e)  //DS20231116
        {
            TradesController tradesController = TradesController.GetInstance();
            ProjectsController projectsController = ProjectsController.GetInstance();
            lblMessageResult.Text = projectsController.TradeItemTemplateUpload(tradeItemCategoryInfo,ExcelUpload);
            tradeItemCategoryInfo.TradeItems = tradesController.GetTradeItems(tradeItemCategoryInfo);
            BindTradeItems();

        }
            protected void btnExport_Click(object sender, EventArgs e)  //DS20231116
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            projectsController.TradeItemTemplateDownLoad(tradeItemCategoryInfo.TradeItems, tradeItemCategoryInfo.Trade.Project.Name, tradeItemCategoryInfo.Name);
            //Response.Clear();      DS20231115 >>>
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Charset = "";
            //string FileName = "Item Category" + DateTime.Now + ".xls";
            //StringWriter strwritter = new StringWriter();
            //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);

            //StreamReader sr = new StreamReader(Server.MapPath("../Styles/"));
            //string s = sr.ReadToEnd();

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            //gvTradeItems.GridLines = GridLines.Both;
            //gvTradeItems.HeaderStyle.Font.Bold = true;
            //gvTradeItems.RenderControl(htmltextwrtter);
            //Response.Write(strwritter.ToString());
            //Response.End();

        }
    }
}
