using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class ViewClientTradesPage : SOSPage
    {

#region Members
        private ProjectInfo projectInfo = null;
        private ClientTradeInfo clientTradeInfo = null;
        private ClientTradeInfo newClientTradeInfo = null;
#endregion
        
#region Private Methods
        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (projectInfo == null)
                return null;

            tempNode.ParentNode.Title = projectInfo.Name;
            tempNode.ParentNode.Url += "?ProjectId=" + projectInfo.IdStr;

            return currentNode;
        }

        protected HtmlTableCell TextBoxCell(String style, String id, String width, Boolean withValidator)
        {
            HtmlTableCell cell = new HtmlTableCell();
            TextBox textBox = new TextBox();

            cell.Attributes.Add("class", style);

            textBox.ID = id;
            textBox.Width = new Unit(width);

            if (withValidator)
            {
                CompareValidator compareValidator = new CompareValidator();
                compareValidator.ControlToValidate = textBox.ClientID;
                compareValidator.ErrorMessage = "Invalid number!<br />";
                compareValidator.CssClass = "frmError";
                compareValidator.Display = ValidatorDisplay.Dynamic;
                compareValidator.Operator = ValidationCompareOperator.DataTypeCheck;
                compareValidator.Type = ValidationDataType.Currency;

                cell.Controls.Add(compareValidator);            
            }

            cell.Controls.Add(textBox);

            return cell;
        }

        protected HtmlTableCell EmptyCell()
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", "lstBlankItem");
            return cell;
        }
        
        protected void CreateForm()
        {
            HtmlTable table;
            HtmlTableRow row;
            HtmlTableCell cell;
            HyperLink hyperLink;
            HtmlAnchor htmlAnchor;
            TextBox textBox;
            Button button;
            DropDownList dropDownList;
            Image image;
            String currStyle = null;
            ClientTradeInfo previousClientTrade;
            ClientTradeInfo clientTrade;
            ClientTradeInfo nextClientTrade;
            List<ClientTradeTypeInfo> clientTradeTypeInfoList;

            if (projectInfo.ClientTrades != null)
            {
                for (int i = 0; i < projectInfo.ClientTrades.Count; i++)
                {
                    previousClientTrade = i > 0 ? projectInfo.ClientTrades[i - 1] : null;
                    clientTrade = projectInfo.ClientTrades[i];
                    nextClientTrade = i < projectInfo.ClientTrades.Count - 1 ? projectInfo.ClientTrades[i + 1] : null;

                    currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                    row = new HtmlTableRow();

                    if (clientTrade.Equals(clientTradeInfo))
                    {
                        if (newClientTradeInfo != null)
                        {
                            row.Cells.Add(EmptyCell());
                            row.Cells.Add(EmptyCell());
                        }

                        row.Cells.Add(TextBoxCell(currStyle, "txtName", "250px", false));
                        row.Cells.Add(TextBoxCell(currStyle, "txtPercentage", "50px", true));
                        row.Cells.Add(TextBoxCell(currStyle, "txtAmount", "100px", true));

                        cell = new HtmlTableCell();
                        cell.Attributes.Add("class", currStyle);
                        cell.NoWrap = true;
                        cell.Align = "Center";
                        cell.ColSpan = 3;

                        button = new Button();
                        button.ID = "cmdUpdate";
                        button.Text = "Update";
                        button.Click += new System.EventHandler(cmdUpdate_Click);
                        cell.Controls.Add(button);

                        image = new Image();
                        image.ImageUrl = "~/Images/1x1.gif";
                        image.Width = 8;
                        cell.Controls.Add(image);

                        button = new Button();
                        button.ID = "cmdCancel";
                        button.Text = "Cancel";
                        button.Click += new System.EventHandler(cmdCancel_Click);
                        cell.Controls.Add(button);

                        row.Cells.Add(cell);
                    }
                    else
                    {
                        if (newClientTradeInfo != null)
                        {
                            cellSort.Visible = true;
                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);

                            if (clientTrade.Claimed == null && previousClientTrade != null && previousClientTrade.Claimed == null)
                            {
                                htmlAnchor = new HtmlAnchor();
                                htmlAnchor.Attributes.Add("title", "Up");
                                htmlAnchor.Attributes.Add("href", clientTrade.IdStr);
                                htmlAnchor.ServerClick += new System.EventHandler(lnkUp_Click);
                                image = new Image();
                                image.ImageUrl = "~/Images/IconUp.gif";
                                htmlAnchor.Controls.Add(image);
                                cell.Controls.Add(htmlAnchor);
                            }
                            row.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);

                            if (clientTrade.Claimed == null && nextClientTrade != null && nextClientTrade.Claimed == null)
                            {
                                htmlAnchor = new HtmlAnchor();
                                htmlAnchor.Attributes.Add("title", "Down");
                                htmlAnchor.Attributes.Add("href", clientTrade.IdStr);
                                htmlAnchor.ServerClick += new System.EventHandler(lnkDown_Click);
                                image = new Image();
                                image.ImageUrl = "~/Images/IconDown.gif";
                                htmlAnchor.Controls.Add(image);
                                cell.Controls.Add(htmlAnchor);
                            }

                            row.Cells.Add(cell);
                        }

                        cell = new HtmlTableCell();
                        cell.Attributes.Add("class", currStyle);
                        cell.InnerText = clientTrade.Name;
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        cell.Align = "Right";
                        cell.Attributes.Add("class", currStyle);
                        cell.InnerText = UI.Utils.SetFormEditDecimal(clientTrade.ProjectPercentage);
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        cell.Align = "Right";
                        cell.Attributes.Add("class", currStyle);
                        cell.InnerText = UI.Utils.SetFormEditDecimal(clientTrade.Amount);
                        row.Cells.Add(cell);

                        cell = new HtmlTableCell();
                        cell.Align = "Right";
                        cell.Attributes.Add("class", currStyle);
                        cell.InnerText = UI.Utils.SetFormEditDecimal(clientTrade.Claimed);
                        row.Cells.Add(cell);

                        if (newClientTradeInfo != null)
                        {
                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);
                            hyperLink = new HyperLink();
                            hyperLink.CssClass = "frmLink";
                            hyperLink.ToolTip = "Edit";
                            hyperLink.ImageUrl = "~/Images/IconEdit.gif";
                            hyperLink.NavigateUrl = String.Format("~/Modules/Projects/ViewClientTrades.aspx?ProjectId={0}&ClientTradeId={1}", projectInfo.IdStr, clientTrade.IdStr);
                            cell.Controls.Add(hyperLink);
                            row.Cells.Add(cell);

                            cell = new HtmlTableCell();
                            cell.Attributes.Add("class", currStyle);

                            if (clientTrade.Claimed == null)
                            {
                                htmlAnchor = new HtmlAnchor();
                                htmlAnchor.Attributes.Add("class", "lstLink");
                                htmlAnchor.Attributes.Add("title", "Delete");
                                htmlAnchor.Attributes.Add("href", clientTrade.IdStr);
                                htmlAnchor.Attributes.Add("onclick", String.Format("javascript:return confirm('Delete Client Trade: {0}?');", clientTrade.Name));
                                htmlAnchor.ServerClick += new System.EventHandler(lnkDelete_Click);
                                image = new Image();
                                image.ImageUrl = "~/Images/IconDelete.gif";
                                htmlAnchor.Controls.Add(image);
                                cell.Controls.Add(htmlAnchor);
                            }

                            row.Cells.Add(cell);
                        }
                    }

                    tblTrades.Rows.Add(row);
                }
            }

            if (newClientTradeInfo != null)
            {
                cellSort.Visible = true;
                clientTradeTypeInfoList = TradesController.GetInstance().GetClientTradeTypes();

                dropDownList = new DropDownList();
                dropDownList.ID = "ddlNewName";
                dropDownList.Items.Add(new ListItem(String.Empty, String.Empty));
                foreach (ClientTradeTypeInfo clientTradeTypeInfo in clientTradeTypeInfoList)
                    dropDownList.Items.Add(new ListItem(clientTradeTypeInfo.Name, clientTradeTypeInfo.Name));

                table = new HtmlTable();

                row = new HtmlTableRow();

                cell = new HtmlTableCell();
                cell.Controls.Add(dropDownList);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                row = new HtmlTableRow();
                cell = new HtmlTableCell();
                textBox = new TextBox();
                textBox.ID = "txtNewName";
                textBox.Width = 250;
                cell.Controls.Add(textBox);
                row.Cells.Add(cell);

                table.Rows.Add(row);

                row = new HtmlTableRow();
                row.Cells.Add(EmptyCell());
                row.Cells.Add(EmptyCell());

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", "lstBlankItem");
                cell.Controls.Add(table);
                row.Cells.Add(cell);

                row.Cells.Add(TextBoxCell("lstBlankItem", "txtNewPercentage", "50px", true));
                row.Cells.Add(TextBoxCell("lstBlankItem", "txtNewAmount", "100px", true));

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", currStyle);
                cell.Align = "Center";
                cell.ColSpan = 3;
                button = new Button();
                button.ID = "cmdAdd";
                button.Text = "Add";
                button.Click += new System.EventHandler(cmdAdd_Click);
                cell.Controls.Add(button);
                row.Cells.Add(cell);

                tblTrades.Rows.Add(row);
            }
        }

        private void ObjectsToForm()
        {
            lblContractAmount.Text = UI.Utils.SetFormDecimal(projectInfo.ContractAmount);
            lblTradesAmount.Text = UI.Utils.SetFormDecimal(projectInfo.TotalClientTrades);
            lblDifference.Text = UI.Utils.SetFormDecimal(projectInfo.ContractAmountMinusClientTrades);

            if (projectInfo.ContractAmountMinusClientTrades != null)
                if (projectInfo.ContractAmountMinusClientTrades != 0)
                    tdDifference.Attributes.Add("class", "lstErrItem");

            if (clientTradeInfo != null)
            {
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtName")).Text = UI.Utils.SetFormString(clientTradeInfo.Name);
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtPercentage")).Text = UI.Utils.SetFormEditDecimal(clientTradeInfo.ProjectPercentage);
                ((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmount")).Text = UI.Utils.SetFormEditDecimal(clientTradeInfo.Amount);
            }
        }

        private void MoveUpDown(int? clientTradeId, Boolean isUp)
        {
            try
            {
                ProjectsController projectsController = ProjectsController.GetInstance();
                ClientTradeInfo clientTradeInfo = projectInfo.ClientTrades.Find(delegate(ClientTradeInfo clientTradeInfoInList) { return clientTradeInfoInList.Id.Equals(clientTradeId); });
                projectsController.ChangeDisplayOrderClientTrade(projectInfo.ClientTrades, clientTradeInfo, isUp);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientTrades.aspx?ProjectId=" + projectInfo.IdStr);
        }
#endregion
        
#region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            ClientTradeInfo tempClientTradeInfo;
            String parameterProjectId;
            String parameterClientTradeId;

            try
            {
                Security.CheckAccess(Security.userActions.ViewProject);
                parameterProjectId = Utils.CheckParameter("ProjectId");
                projectInfo = projectsController.GetProjectWithClientTrades(Int32.Parse(parameterProjectId));

                if (projectInfo.ClientTrades != null)
                    foreach (ClientTradeInfo clientTrade in projectInfo.ClientTrades)
                        clientTrade.Project = projectInfo;

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                if (Security.ViewAccess(Security.userActions.ViewProject))
                {
                    tempClientTradeInfo = new ClientTradeInfo();
                    tempClientTradeInfo.Project = projectInfo;

                    if (processController.AllowEditCurrentUser(tempClientTradeInfo))
                    {
                        newClientTradeInfo = new ClientTradeInfo();
                        newClientTradeInfo.Project = projectInfo;

                        parameterClientTradeId = Request.Params["ClientTradeId"];
                        if (parameterClientTradeId != null)
                        {
                            clientTradeInfo = projectsController.GetClientTrade(Int32.Parse(parameterClientTradeId));
                            Core.Utils.CheckNullObject(clientTradeInfo, parameterClientTradeId, "Client Trade");
                            clientTradeInfo.Project = projectInfo;
                        }

                    }
                }

                CreateForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                    ObjectsToForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectsController.GetInstance().DeleteClientTrade(new ClientTradeInfo(Int32.Parse(((HtmlAnchor)sender).HRef)));
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientTrades.aspx?ProjectId=" + projectInfo.IdStr);
        }

        protected void lnkUp_Click(object sender, EventArgs e)
        {
            MoveUpDown(Int32.Parse(((HtmlAnchor)sender).HRef), true);
        }

        protected void lnkDown_Click(object sender, EventArgs e)
        {
            MoveUpDown(Int32.Parse(((HtmlAnchor)sender).HRef), false);
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Decimal? percentage = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtPercentage")).Text);
                Decimal? amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtAmount")).Text);

                clientTradeInfo.Name = UI.Utils.GetFormString(((TextBox)Utils.FindControlRecursive(tblTrades, "txtName")).Text);

                if (clientTradeInfo.Name == null)
                    throw new Exception("The field Trade Name must have a value.");

                foreach (ClientTradeInfo clientTrade in projectInfo.ClientTrades)
                    if (!clientTrade.Equals(clientTradeInfo))
                        if (clientTrade.Name.Equals(clientTradeInfo.Name))
                            throw new Exception(String.Format("The Client Trade: {0} already exist in the project.", clientTrade.Name));

                if (amount == null)
                    if (percentage == null)
                        clientTradeInfo.Amount = null;
                    else
                        clientTradeInfo.Amount = Math.Round((Decimal)((clientTradeInfo.Project.ContractAmount / 100) * percentage), 2);
                else
                    clientTradeInfo.Amount = Math.Round((Decimal)amount, 2);

                ProjectsController.GetInstance().UpdateClientTrade(clientTradeInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientTrades.aspx?ProjectId=" + projectInfo.IdStr);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Projects/ViewClientTrades.aspx?ProjectId=" + projectInfo.IdStr);
        }

        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();

            try
            {
                Decimal? percentage = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtNewPercentage")).Text);
                Decimal? amount = UI.Utils.GetFormDecimal(((TextBox)Utils.FindControlRecursive(tblTrades, "txtNewAmount")).Text);
                String txtName = UI.Utils.GetFormString(((TextBox)Utils.FindControlRecursive(tblTrades, "txtNewName")).Text);
                String ddlName = UI.Utils.GetFormString(((DropDownList)Utils.FindControlRecursive(tblTrades, "ddlNewName")).Text);

                newClientTradeInfo.Name = txtName != null ? txtName : ddlName;

                if (newClientTradeInfo.Name == null)
                    throw new Exception("The field Trade Name must have a value.");

                foreach (ClientTradeInfo clientTrade in projectInfo.ClientTrades)
                    if (clientTrade.Name.Equals(newClientTradeInfo.Name))
                        throw new Exception(String.Format("The Client Trade: {0} already exist in the project.", clientTrade.Name));

                if (amount == null)
                    if (percentage == null)
                        newClientTradeInfo.Amount = null;
                    else
                        newClientTradeInfo.Amount = Math.Round((Decimal)((newClientTradeInfo.Project.ContractAmount / 100) * percentage), 2);
                else
                    newClientTradeInfo.Amount = Math.Round((Decimal)amount);

                projectsController.AddClientTrade(newClientTradeInfo);
                projectInfo.ClientTrades = projectsController.GetClientTrades(projectInfo);
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

            Response.Redirect("~/Modules/Projects/ViewClientTrades.aspx?ProjectId=" + projectInfo.IdStr);
        }
#endregion
    
    }
}
