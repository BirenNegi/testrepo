using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using System.Xml;


namespace SOS.Web
{
    
    public partial class EditItemCategories_Items_ScopeOfWork :SOSPage
    {

        #region Members
        private TradeInfo tradeInfo = null;
        private TradeItemCategoryInfo tradeItemCategoryInfo = null;
        private TradeItemInfo tradeItemInfo = null;
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

        private void ObjectsToForm()
        {
            ProcessController processController = ProcessController.GetInstance();
            Boolean isEditable = false;
           // String deleteMessage;


            if (Security.ViewAccess(Security.userActions.EditTrade))
            {
                isEditable = processController.AllowEditCurrentUser(tradeInfo.Process);

            }
            if (processController.AllowEditCurrentUser(tradeInfo.Process))
            {
               // cmdDeleteTop.Attributes.Add("onClick", "javascript:return confirm('Delete Trade Item Category and All its Items?');");
                gvItemCategories.Columns[8].Visible = true;
            }


            // phAddItemCategory.Visible = isEditable;
            
            gvItemCategories.Columns[4].Visible = isEditable;
            gvItemCategories.Columns[5].Visible = isEditable;
            gvItemCategories.Columns[6].Visible = isEditable;
            gvItemCategories.Columns[7].Visible = isEditable;
            gvItemCategories.Columns[8].Visible = isEditable;   

            BindItemCategories();




        }

        private void BindItemCategories()
        {
            
            if (tradeInfo.ItemCategories != null)
            {
                gvItemCategories.DataSource = tradeInfo.ItemCategories;
                gvItemCategories.DataBind();

                if (gvItemCategories.Rows.Count == 0)
                {
                    TradeItemCategoryInfo dummyCategory = new TradeItemCategoryInfo();
                    dummyCategory.Name = "No records";
                    dummyCategory.ShortDescription = "";
                    dummyCategory.LongDescription = "";
                    tradeInfo.ItemCategories.Add(dummyCategory);
                    gvItemCategories.DataSource =tradeInfo.ItemCategories;
                    gvItemCategories.DataBind();
                    
                    gvItemCategories.Columns[4].Visible = false;
                    gvItemCategories.Columns[5].Visible = false;
                    gvItemCategories.Columns[6].Visible = false;
                    gvItemCategories.Columns[7].Visible = false;
                    gvItemCategories.Columns[8].Visible = false;
                }


            }



        }

        private void BindTradeItems()
        {
            if (tradeItemCategoryInfo.TradeItems != null)
            {       gvTradeItems.DataSource = tradeItemCategoryInfo.TradeItems;
                    gvTradeItems.DataBind();
            }
        }


        private void FormToObjects()
        {
            //tradeItemCategoryInfo.Name = UI.Utils.GetFormString(txtName.Text);
            //tradeItemCategoryInfo.ShortDescription = UI.Utils.GetFormString(txtShortDescription.Text);
            //tradeItemCategoryInfo.LongDescription = UI.Utils.GetFormString(txtLongDescription.Text);
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



        protected void Page_Load(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            TradesController tradesController = TradesController.GetInstance();
            String parameterTradeId = Request.Params["Traded"];

            try
            {
                Security.CheckAccess(Security.userActions.EditTrade);
                parameterTradeId = Utils.CheckParameter("TradeId");
                tradeInfo = tradesController.GetTrade(Int32.Parse(parameterTradeId));
                Core.Utils.CheckNullObject(tradeInfo, parameterTradeId, "Trade");

                tradeInfo.ItemCategories = tradesController.GetDeepTradeItemCategories(tradeInfo);
                tradeInfo.Project = projectsController.GetProject(tradeInfo.Project.Id);
                if (tradeInfo.Process != null)
                    tradeInfo.Process.Project = tradeInfo.Project;


                if (!Page.IsPostBack)
                {
                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

        }

        #region gvItemCategories Events
        protected void gvItemCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddCategory") {
                GridViewRow row = gvItemCategories.FooterRow;
                tradeItemCategoryInfo = new TradeItemCategoryInfo();
                tradeItemCategoryInfo.Trade = tradeInfo;

                tradeItemCategoryInfo.Name = UI.Utils.GetFormString(((TextBox)(row.Cells[1].FindControl("TxtNewName"))).Text).ToString();
                tradeItemCategoryInfo.ShortDescription = ((TextBox)(row.Cells[2].FindControl("TxtNewShortDesc"))).Text.ToString();
                tradeItemCategoryInfo.LongDescription = ((TextBox)(row.Cells[3].FindControl("TxtNewLongDesc"))).Text.ToString();

                tradeItemCategoryInfo.Id = TradesController.GetInstance().AddUpdateTradeItemCategory(tradeItemCategoryInfo);
                tradeInfo.ItemCategories = TradesController.GetInstance().GetTradeItemCategories(tradeInfo);
                ObjectsToForm();
               
            }
            else if (e.CommandName == "Select")
            {
                TradesController tradesController = TradesController.GetInstance();
                int? tradeItemCategoryId = (int?)gvItemCategories.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                tradeItemCategoryInfo = tradeInfo.ItemCategories.Find(delegate (TradeItemCategoryInfo tradeItemCategoryInfoInList) { return tradeItemCategoryInfoInList.Id == tradeItemCategoryId; });

                tradeItemCategoryInfo = tradesController.GetDeepTradeItemCategory(Int32.Parse(tradeItemCategoryInfo.IdStr));

                gvTradeItems.DataSource = tradeItemCategoryInfo.TradeItems;
                gvTradeItems.DataBind();
                gvTradeItems.Columns[6].Visible = true;
                gvTradeItems.Columns[7].Visible = true;

                LblTradeCategory.Text = tradeItemCategoryInfo.Name;
                LblTradeCategoryId.Text = tradeItemCategoryInfo.IdStr;

                if (gvTradeItems.Rows.Count == 0)
                {
                    TradeItemInfo dumItem = new TradeItemInfo();
                    dumItem.Name = "No records";
                    dumItem.Units = "";
                    tradeItemCategoryInfo.TradeItems.Add(dumItem);
                    gvTradeItems.DataSource = tradeItemCategoryInfo.TradeItems;
                    gvTradeItems.DataBind();
                    gvTradeItems.Columns[6].Visible = false;
                    gvTradeItems.Columns[7].Visible = false;

                }

            }
            else if (e.CommandName == "MoveUp" || e.CommandName == "MoveDown")
            {  
                    TradesController tradesController = TradesController.GetInstance();
                    int? tradeItemCategoryId = (int?)gvItemCategories.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                    TradeItemCategoryInfo tradeItemCategoryInfo = tradeInfo.ItemCategories.Find(delegate (TradeItemCategoryInfo tradeItemCategoryInfoInList) { return tradeItemCategoryInfoInList.Id == tradeItemCategoryId; });

                    if (e.CommandName == "MoveUp")
                        tradesController.ChangeDisplayOrderTradeItemCategory(tradeInfo.ItemCategories, tradeItemCategoryInfo, true);
                    else if (e.CommandName == "MoveDown")
                        tradesController.ChangeDisplayOrderTradeItemCategory(tradeInfo.ItemCategories, tradeItemCategoryInfo, false);
                   
                    //if (e.CommandName != "Edit"&& e.CommandName != "Update")
                    //{  } 
                    tradeInfo.ItemCategories = tradesController.GetTradeItemCategories(tradeInfo);
                    BindItemCategories();
                    
           }
        }

        protected void gvItemCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
           
            try
            {
                if (Page.IsValid)
                {
                    GridViewRow row = (GridViewRow)gvItemCategories.Rows[e.RowIndex];
                    int? tradeItemCategoryId = (int?)gvItemCategories.DataKeys[Convert.ToInt32(e.RowIndex)].Value;

                    tradeItemCategoryInfo = new TradeItemCategoryInfo(tradeItemCategoryId);


                    tradeItemCategoryInfo = TradesController.GetInstance().GetTradeItemCategory(tradeItemCategoryInfo.Id);

                    tradeItemCategoryInfo.Name = UI.Utils.GetFormString(((TextBox)(row.Cells[2].FindControl("TxtUpdateName"))).Text).ToString();
                    tradeItemCategoryInfo.ShortDescription =((TextBox)(row.Cells[3].FindControl("TxtUpdateShortDesc"))).Text.ToString();
                    tradeItemCategoryInfo.LongDescription =((TextBox)(row.Cells[4].FindControl("TxtUpdateLongDesc"))).Text.ToString();

                    tradeItemCategoryInfo.Id = TradesController.GetInstance().AddUpdateTradeItemCategory(tradeItemCategoryInfo);

                    ObjectsToForm();
                }
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
            //tradeInfo.ItemCategories = TradesController.GetInstance().GetTradeItemCategories(tradeInfo);
            //gvItemCategories.EditIndex = -1;
            //BindItemCategories();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);


        }
        protected void gvItemCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItemCategories.EditIndex = e.NewEditIndex;
            BindItemCategories();
        }

        protected void gvItemCategories_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            gvItemCategories.EditIndex = -1;
            BindItemCategories();
        }

       

        protected void gvItemCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int? tradeItemCategoryId = (int?)gvItemCategories.DataKeys[Convert.ToInt32(e.RowIndex)].Value;
            tradeItemCategoryInfo = new TradeItemCategoryInfo(tradeItemCategoryId);
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
            //tradeInfo.ItemCategories = TradesController.GetInstance().GetTradeItemCategories(tradeInfo);
            //gvItemCategories.EditIndex = -1;
            //BindItemCategories();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        #endregion GVItemCategories


        #region gvTradeItems
        protected void gvTradeItems_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            

                TradesController tradesController = TradesController.GetInstance();


            if (e.CommandName == "AddTradeItem")
            {
                try
                {
                    tradeItemCategoryInfo = tradesController.GetDeepTradeItemCategory((int?)int.Parse(LblTradeCategoryId.Text));
                    tradeItemCategoryInfo.Trade = tradesController.GetDeepTrade(tradeItemCategoryInfo.Trade.Id);
                    GridViewRow row = gvTradeItems.FooterRow;
                    tradeItemInfo = new TradeItemInfo();
                    tradeItemInfo.TradeItemCategory = tradeItemCategoryInfo;

                    tradeItemInfo.Name = UI.Utils.GetFormString(((TextBox)(row.Cells[2].FindControl("TxtItemName"))).Text).ToString();
                    tradeItemInfo.Units = UI.Utils.GetFormString(((TextBox)(row.Cells[3].FindControl("TxtItemUnit"))).Text.ToString());

                    if (((DropDownList)(row.Cells[4].FindControl("dpdItemQC"))).SelectedItem.Value.ToString() != "")
                        tradeItemInfo.RequiresQuantityCheck = ((bool?)(Boolean.Parse(((DropDownList)(row.Cells[4].FindControl("dpdItemQC"))).SelectedItem.Value.ToString())));

                    if (((DropDownList)(row.Cells[5].FindControl("dpdItemRP"))).SelectedItem.Value.ToString() != "")
                        tradeItemInfo.RequiredInProposal = ((bool?)(Boolean.Parse(((DropDownList)(row.Cells[5].FindControl("dpdItemRP"))).SelectedItem.Value.ToString())));

                    tradeItemInfo.Scope = ((TextBox)(row.Cells[6].FindControl("TxtItemScopeWork"))).Text.ToString();


                    tradeItemInfo.Id = TradesController.GetInstance().AddUpdateTradeItem(tradeItemInfo);
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }
                tradeItemCategoryInfo = TradesController.GetInstance().GetDeepTradeItemCategory((int?)int.Parse(LblTradeCategoryId.Text));
                gvTradeItems.EditIndex = -1;
                BindTradeItems();


            }
            
            else if (e.CommandName == "MoveUp" || e.CommandName == "MoveDown" || e.CommandName =="Edit"|| e.CommandName == "Cancel")
            {

                int? tradeItemId = (int?)gvTradeItems.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;

                tradeItemCategoryInfo = tradesController.GetDeepTradeItemCategory((int?)int.Parse(LblTradeCategoryId.Text));
                TradeItemInfo tradeItemInfo = tradeItemCategoryInfo.TradeItems.Find(delegate (TradeItemInfo tradeItemInfoInList) { return tradeItemInfoInList.Id == tradeItemId; });
                //tradeItemCategoryInfo
                if (e.CommandName == "MoveUp")
                    tradesController.ChangeDisplayOrderTradeItem(tradeItemCategoryInfo.TradeItems, tradeItemInfo, true);
                else if (e.CommandName == "MoveDown")
                    tradesController.ChangeDisplayOrderTradeItem(tradeItemCategoryInfo.TradeItems, tradeItemInfo, false);
                
                
                    tradeItemCategoryInfo.TradeItems = tradesController.GetTradeItems(tradeItemCategoryInfo);
                    BindTradeItems();
                
            }
        }
       

        protected void gvTradeItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            GridViewRow row = (GridViewRow)gvTradeItems.Rows[e.RowIndex];
            int? tradeItemId = (int?)gvTradeItems.DataKeys[Convert.ToInt32(e.RowIndex)].Value;

            tradeItemInfo = new TradeItemInfo(tradeItemId);

            TradesController tradesController = TradesController.GetInstance();

            Boolean deleteError = false;

                try
                {
                    XmlDocument xmlDocument = tradesController.CheckTradeItemForDelete(tradeItemInfo);

                    if (xmlDocument.DocumentElement != null)
                        deleteError = true;
                    else
                        tradesController.DeleteTradeItem(tradeItemInfo);

                    BindDeleteErrorsTree(xmlDocument, deleteError);
                }
                catch (Exception Ex)
                {
                    Utils.ProcessPageLoadException(this, Ex);
                }
            tradeItemCategoryInfo = tradesController.GetDeepTradeItemCategory((int?)int.Parse(LblTradeCategoryId.Text));
            tradeItemCategoryInfo.TradeItems = tradesController.GetTradeItems(tradeItemCategoryInfo);
            BindTradeItems();

        }

        protected void gvTradeItems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTradeItems.EditIndex = e.NewEditIndex;

            BindTradeItems();

          


        }

        protected void gvTradeItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    GridViewRow row = (GridViewRow)gvTradeItems.Rows[e.RowIndex];
                    int? tradeItemId = (int?)gvTradeItems.DataKeys[Convert.ToInt32(e.RowIndex)].Value;

                    tradeItemInfo = new TradeItemInfo(tradeItemId);


                    tradeItemInfo = TradesController.GetInstance().GetTradeItem(tradeItemInfo.Id);

                    tradeItemInfo.Name = UI.Utils.GetFormString(((TextBox)(row.Cells[2].FindControl("TxtName"))).Text).ToString();
                    tradeItemInfo.Units = ((TextBox)(row.Cells[3].FindControl("TxtUnit"))).Text.ToString();

                    if(((DropDownList)(row.Cells[4].FindControl("dpdQC"))).SelectedItem.Value.ToString()!="")
                    tradeItemInfo.RequiresQuantityCheck = ((bool?)(Boolean.Parse(((DropDownList)(row.Cells[4].FindControl("dpdQC"))).SelectedItem.Value.ToString())));


                    if (((DropDownList)(row.Cells[5].FindControl("dpdRP"))).SelectedItem.Value.ToString() != "")
                        tradeItemInfo.RequiredInProposal=((bool?)(Boolean.Parse(((DropDownList)(row.Cells[5].FindControl("dpdRP"))).SelectedItem.Value.ToString())));

                    tradeItemInfo.Scope = ((TextBox)(row.Cells[6].FindControl("TxtScopeHeader1"))).Text.ToString();


                    tradeItemInfo.Id = TradesController.GetInstance().AddUpdateTradeItem(tradeItemInfo);
                }

            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }


            tradeItemCategoryInfo = TradesController.GetInstance().GetDeepTradeItemCategory((int?)int.Parse(LblTradeCategoryId.Text)); 
            gvTradeItems.EditIndex = -1;
            BindTradeItems();

        }

       

        protected void gvTradeItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTradeItems.EditIndex = -1;
            BindTradeItems();
        }

        #endregion


    }
}