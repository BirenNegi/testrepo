using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
using System.Collections.Specialized;
//using Microsoft.Reporting.WebForms;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;


namespace SOS.Web
{
    public partial class ShowBudgetTradeSummary : SOSPage
    {
        #region Members
        private ProjectInfo projectInfo = null;
        private BudgetInfo budgetInfo = null;
        private BudgetInfo newBudgetInfo = null;
        private List<IBudget> projectBudget = null;
        private List<IBudget> projectBudgetFull = null;
        private List<TradeTemplateInfo> tradeTemplateInfoList = null;
        private StringDictionary tradeNamesDictionary = null;
        private Boolean viewSummary = false;
        #endregion


           

        #region Private methods

        //protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        //{
        //    SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
        //    SiteMapNode tempNode = currentNode;

        //    //if (clientVariationInfo == null)
        //    //    return null;

        //    //tempNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Client Variation";

        //    //tempNode.ParentNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Accounts" : "Client Variations";
        //    //tempNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr + "&Type=" + clientVariationInfo.Type;

        //    //tempNode.ParentNode.ParentNode.Title = clientVariationInfo.Project.Name;
        //    //tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr;

        //    return currentNode;
        //}



        private void BindProjectsList()
        {
            List<ProjectInfo> projectInfoList = null;
            ProjectsController projectsController = ProjectsController.GetInstance();

            if (chkAll.Checked)
                projectInfoList = projectsController.ListProjects();
            else
                projectInfoList = projectsController.ListActiveProjects();

            ddlProjects.Items.Clear();
            ddlProjects.Items.Add(new ListItem(String.Empty, String.Empty));

            if (projectInfoList != null)
                foreach (ProjectInfo projectInfo in projectInfoList)
                    ddlProjects.Items.Add(new ListItem(projectInfo.Name, projectInfo.IdStr));
        }


        private DataTable CreateTradeReport(String tradeCode, Boolean includeAllCVSA, Boolean includeAllOVO)
        {
              DataTable dt = new DataTable();
                 dt.Columns.Add("Date");
                 dt.Columns.Add("Type");
                 dt.Columns.Add("TradeCode");
                 dt.Columns.Add("Amount");
                 dt.Columns.Add("Balance");
                 dt.Columns.Add("Allocation");
                 dt.Columns.Add("Contract");
                 dt.Columns.Add("WinLoss");
                
              
            if (tradeCode != String.Empty)
            {
                Decimal balance = 0;
                String currStyle = null;
              
                projectBudgetFull.Sort(new BudgetComparer());

                foreach (IBudget iBudget in projectBudgetFull)
                {
                    if (iBudget.TradeCode == tradeCode && (iBudget.BudgetInclude ||                         
                        ((iBudget.BudgetType == BudgetType.Contract || iBudget.BudgetType == BudgetType.Variation) && includeAllOVO)))
                    {
                        //currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                        balance += iBudget.BudgetAmount;


                        //if (iBudget.BudgetType != BudgetType.BOQ) //iBudget.BudgetType != BudgetType.CV && iBudget.BudgetType != BudgetType.SA && iBudget.BudgetType != BudgetType.BOQ
                        //{ }
                            dt.Rows.Add(
                            UI.Utils.SetFormDate(iBudget.BudgetDate),
                            iBudget.BudgetType,
                            iBudget.TradeCode + " " + iBudget.BudgetName,
                            iBudget.BudgetAmount!=0? UI.Utils.SetFormEditDecimal((Decimal?)iBudget.BudgetAmount):"0.00",
                            balance!=0? UI.Utils.SetFormEditDecimal((Decimal?)balance):"0.00",
                           iBudget.BudgetAmountAllowance!=0? UI.Utils.SetFormEditDecimal((Decimal?)iBudget.BudgetAmountAllowance):"0.00",//--Comparison--//iBudget.BudgetProvider==null? iBudget.BudgetAmountAllowance: iBudget.BudgetProvider.BudgetAmountAllowance,//==String.Empty? 0.00: iBudget.BudgetProvider.BudgetAmountAllowance,
                          - (iBudget.BudgetAmount),//--Contract amount--//iBudget.BudgetProvider == null ? -(iBudget.BudgetAmount): iBudget.BudgetProvider.BudgetAmount,
                                                   // iBudget.BudgetProvider == null ?0.0 : iBudget.BudgetProvider.BudgetUnallocated,
                            iBudget.BudgetWinLoss!=0? UI.Utils.SetFormEditDecimal((Decimal?)iBudget.BudgetWinLoss) : "0.00");
                                                       
                        
                        

                    }
                }

              }
               
            return dt;
        }


        protected void BindBudgets(String ProjectId)
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();
            String parameterProjectId;
            List<IBudget> BOQprojectBudgetFull = null;

            try
            {
                //Security.CheckAccess(Security.userActions.ViewProject);
                parameterProjectId = ddlProjects.SelectedItem.Value.ToString();  //Utils.CheckParameter("ProjectId");
                //parameterPanel = Utils.CheckParameterInt32("Panel");

                projectInfo = projectsController.GetProjectWithBudgets(Int32.Parse(parameterProjectId));
                tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
               
                    projectBudget = projectsController.GetProjectBudget(projectInfo, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO);
                    projectBudgetFull = projectsController.GetProjectBudget(projectInfo, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO, true);


                BOQprojectBudgetFull = projectBudgetFull.Where(x => x.BudgetType == BudgetType.BOQ  ).ToList();

                gvBOQTrade.DataSource = BOQprojectBudgetFull;
                gvBOQTrade.DataBind();

               
               
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }

        }



        protected void ExportToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=BOQSummary.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvBOQTrade.AllowPaging = false;
                // this.BindGrid();

                gvBOQTrade.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvBOQTrade.HeaderRow.Cells)
                {
                    cell.BackColor = Color.FromName("#ffeb9c"); //gvBOQTrade.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvBOQTrade.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = Color.LightGray; 
                                //gvBOQTrade.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = Color.FromName("#ccc9c"); //gvBOQTrade.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvBOQTrade.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }



        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }



        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {  
                if (!Page.IsPostBack)
                {
                    BindProjectsList();
                }
              
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }


        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlProjects.SelectedItem.Value != String.Empty)
                    BindBudgets(ddlProjects.SelectedItem.Value.ToString());
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }




        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            BindProjectsList();
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            if (gvBOQTrade.Rows.Count > 0)
            { ExportToExcel(); }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable DT = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Code = ((e.Row.Cells[0])).Text; //gvBOQTrade.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvTrade = e.Row.FindControl("gvTrades") as GridView; // (GridView)e.Row.Cells[9].Controls[0];  //

                DT = CreateTradeReport(Code, sbiBOQ.IncludeAllCVSA, sbiBOQ.IncludeAllOVO);
                if (DT.Rows.Count > 0)
                {
                    //DataView dv = new DataView(DT);
                    //dv.Sort = "Date";
                    //DT = dv.ToTable();
                }
                gvTrade.DataSource = DT;//string.Format("select top 3 * from Orders where CustomerId='{0}'", customerId));
                gvTrade.DataBind();
                

                

            }
        }


    }
}