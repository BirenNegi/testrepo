using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SOS.Core;
//using Microsoft.Reporting.WebForms;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;

//using iTextSharp;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;


namespace SOS.Web
{
    public partial class ShowCVsSummary : SOSPage
    {

        #region Members
        private ProjectInfo projectInfo;
        private ClientVariationInfo clientVariationInfo;
        private ClientVariationItemInfo clientVariationItemInfo;
        private ClientVariationItemInfo newClientVariationItemInfo;
        private ClientVariationTradeInfo clientVariationTradeInfo;
        private ClientVariationTradeInfo newClientVariationTradeInfo;

        private  ProjectsController projectsController;
        private List<IBudget> projectBudget;
        private TradeTemplateInfo tradeTemplateInfo;
        private List<TradeTemplateInfo> tradeTemplateInfoList;
        #endregion
          


        #region Private Methods

        protected override SiteMapNode BindNavigation(SiteMapResolveEventArgs e)
        {
            SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
            SiteMapNode tempNode = currentNode;

            if (clientVariationInfo == null)
                return null;

            tempNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Account" : "Client Variation";

            tempNode.ParentNode.Title = clientVariationInfo is SeparateAccountInfo ? "Separate Accounts" : "Client Variations";
            tempNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr + "&Type=" + clientVariationInfo.Type;

            tempNode.ParentNode.ParentNode.Title = clientVariationInfo.Project.Name;
            tempNode.ParentNode.ParentNode.Url += "?ProjectId=" + clientVariationInfo.Project.IdStr;

            return currentNode;
        }




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

        //private void BindForm()
        //{
        //    BindProjectsList();
        //    BindTable();

        //}


        //private void BindTable()
        //{
        //    ProjectsController projectsController = ProjectsController.GetInstance();
        //    String currStyle = null;
        //    HtmlTableRow row;
        //    String tradeCodeAndName,Amount,Balance;
        //    ClientVariationTradeInfo clientVariationTradeInBudget;


        //    if (projectInfo.ClientVariations != null)
        //        foreach (ClientVariationInfo Cvinfo in projectInfo.ClientVariations)
        //        {

        //            clientVariationInfo = projectsController.GetClientVariationWithItemsAndTrades(Int32.Parse(Cvinfo.IdStr));
        //            Core.Utils.CheckNullObject(clientVariationInfo, Cvinfo.IdStr, "Client Variation");

        //            clientVariationInfo.Project = projectsController.GetProject(clientVariationInfo.Project.Id);
        //            clientVariationInfo.Project.ClientVariations = new List<ClientVariationInfo>();
        //            clientVariationInfo.Project.ClientVariations.Add(clientVariationInfo);


                    

        //                        foreach (ClientVariationTradeInfo CvTrade in clientVariationInfo.Trades)
        //                        {
        //                            currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
        //                            row = new HtmlTableRow();

        //                            clientVariationTradeInfo = clientVariationInfo.Trades.Find(delegate (ClientVariationTradeInfo clientVariationTradeInfoInList) { return clientVariationTradeInfoInList.Equals(new ClientVariationTradeInfo(Int32.Parse(CvTrade.IdStr))); });

        //                              tradeTemplateInfo = tradeTemplateInfoList.Find(delegate (TradeTemplateInfo tradeTemplateInfoInList) { return tradeTemplateInfoInList.TradeCode.Equals(CvTrade.TradeCode); });
        //                              tradeCodeAndName = tradeTemplateInfo != null ? tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName : null;
        //                              Amount = CvTrade.Amount.ToString();
        //                                 clientVariationTradeInBudget = (ClientVariationTradeInfo)projectBudget.Find(pb => pb.Equals(CvTrade));
        //                                    if (clientVariationTradeInBudget != null)
        //                                    {
        //                                        Balance= clientVariationTradeInBudget.BudgetAmountInitial.Value.ToString();
        //                                        //tradeInUse = projectBudget.Any(pb => clientVariationTrade.Equals(pb.BudgetProvider));
        //                                    }
        //                                    else
        //                                        Balance = "N/A";


        //                                } //-- foreach (ClientVariationTradeInfo CvTrade



        //        }  // --foreach (ClientVariationInfo Cvinfo 


        //}

        private void bindTradeTable()
        {  
            DataTable dt = new DataTable();
            dt.Columns.Add("TradeCode");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Balance");

        }






        private DataTable Getdata(string Id)
        {
            String tradeCodeAndName, Amount, Balance;
            ClientVariationTradeInfo clientVariationTradeInBudget;

            projectsController = ProjectsController.GetInstance();
            DataTable dt = new DataTable();
            dt.Columns.Add("TradeCode");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Balance");

            clientVariationInfo = projectsController.GetClientVariationWithItemsAndTrades(Int32.Parse(Id));
            Core.Utils.CheckNullObject(clientVariationInfo, Id, "Client Variation");


            foreach (ClientVariationTradeInfo CvTrade in clientVariationInfo.Trades)
            {
                //currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";
                //row = new HtmlTableRow();

                clientVariationTradeInfo = clientVariationInfo.Trades.Find(delegate (ClientVariationTradeInfo clientVariationTradeInfoInList) { return clientVariationTradeInfoInList.Equals(new ClientVariationTradeInfo(Int32.Parse(CvTrade.IdStr))); });

                tradeTemplateInfo = tradeTemplateInfoList.Find(delegate (TradeTemplateInfo tradeTemplateInfoInList) { return tradeTemplateInfoInList.TradeCode.Equals(CvTrade.TradeCode); });
                tradeCodeAndName = tradeTemplateInfo != null ? tradeTemplateInfo.TradeCode + " " + tradeTemplateInfo.TradeName : null;
                Amount = CvTrade.Amount.ToString();
                clientVariationTradeInBudget = (ClientVariationTradeInfo)projectBudget.Find(pb => pb.Equals(CvTrade));
                if (clientVariationTradeInBudget != null)
                {
                    Balance = clientVariationTradeInBudget.BudgetAmountInitial.Value.ToString();
                    //tradeInUse = projectBudget.Any(pb => clientVariationTrade.Equals(pb.BudgetProvider));
                }
                else
                    Balance = "N/A";

                dt.Rows.Add(tradeCodeAndName, Amount, Balance);


            } //-- foreach (ClientVariationTradeInfo CvTrade






            return dt;
        }



        //private void BindCVs()
        //{

        //    //#-----
        //    DateTime? dateissued;
        //    DataTable dt = new DataTable();

        //    //#-------


        //    ProjectsController projectsController = ProjectsController.GetInstance();
        //    List<ReportParameter> reportParameters = new List<ReportParameter>();
        //    ProjectInfo projectInfo;
        //    String projectId;

        //    if (ddlProjects.SelectedValue != String.Empty)
        //    {
        //        projectId = ddlProjects.SelectedValue;
        //        projectInfo = projectsController.GetProjectWithClientVariations(Int32.Parse(projectId));

        //        if (projectInfo != null)
        //        {
        //            reportParameters.Add(new ReportParameter("ProjectName", UI.Utils.SetFormString(projectInfo.Name)));
        //            reportParameters.Add(new ReportParameter("ProjectNumber", UI.Utils.SetFormString(projectInfo.FullNumber)));

        //            repCVsProject.LocalReport.ReportPath = Request.PhysicalApplicationPath + "Reports\\CVs.rdlc";
        //            repCVsProject.LocalReport.SetParameters(reportParameters);
        //            repCVsProject.LocalReport.DataSources.Clear();

        //            //San   //if (projectInfo.ClientVariations != null)
        //            //San  //    repCVsProject.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", projectInfo.ClientVariations));


        //            //#--------------------------------
        //            if (projectInfo.ClientVariations != null)
        //            {

        //                dt.Columns.Add("NumberAndRevisionName");
        //                dt.Columns.Add("Name");
        //                dt.Columns.Add("WriteDate");
        //                dt.Columns.Add("TotalAmount");
        //                dt.Columns.Add("Status");
        //                dt.Columns.Add("VerbalApprovalDate");
        //                dt.Columns.Add("ApprovalDate");

        //                dt.Columns["WriteDate"].DataType = typeof(DateTime);
        //                dt.Columns["VerbalApprovalDate"].DataType = typeof(DateTime);
        //                dt.Columns["ApprovalDate"].DataType = typeof(DateTime);
        //                dt.Columns["TotalAmount"].DataType = typeof(decimal);

        //                foreach (var item in projectInfo.ClientVariations)
        //                {

        //                    if (item.Process.Steps[2].Role == "CM" && item.Process.Steps[2].Status == "Approved")
        //                    {
        //                        dateissued = item.Process.Steps[2].ActualDate;
        //                    }
        //                    else { dateissued = null; }
        //                    dt.Rows.Add(item.NumberAndRevisionName, item.Name, dateissued, item.TotalAmount, item.Status, item.VerbalApprovalDate, item.ApprovalDate);

        //                }

        //                // repCVsProject.LocalReport.DataSources.Add(new ReportDataSource("SOS_Core_ClientVariationInfo", dt));


        //            }





        //            //#---------------------------------------------


        //            repCVsProject.DataBind();
        //            repCVsProject.Visible = true;
        //        }
        //    }
        //}


        protected String InfoStatus(ClientVariationInfo clientVariationInfo)
        {
            ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(clientVariationInfo.Process);
            return processStepInfo != null ? processStepInfo.Name : String.Empty;
        }

        protected String DateStatus(ClientVariationInfo clientVariationInfo)
        {
            ProcessStepInfo processStepInfo = ProcessController.GetInstance().GetLastStep(clientVariationInfo.Process);
            return processStepInfo != null ? UI.Utils.SetFormDate(processStepInfo.ActualDate) : String.Empty;
        }

        protected String LinkClientVariation(ClientVariationInfo clientVariationInfo)
        {
            if (clientVariationInfo.IsInternallyApproved)
                return "~/Modules/Projects/ShowClientVariation.aspx?ClientVariationId=" + clientVariationInfo.IdStr;
            else
                return null;
        }



        #endregion




        protected void Page_Load(object sender, EventArgs e)
        {
            //ProjectsController projectsController = ProjectsController.GetInstance();
            //ProcessController processController = ProcessController.GetInstance();

            try
            {
                //Security.CheckAccess(Security.userActions.ViewReports);
                /*String parameterProjectId = Utils.CheckParameter("ProjectId");

                projectInfo = projectsController.GetProjectWithClientVariations(Int32.Parse(parameterProjectId), "CV");

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                projectBudget = projectsController.GetProjectBudget(projectInfo, sbiTrades.IncludeAllCVSA, sbiTrades.IncludeAllOVO);

                tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();     */
               

                if (!Page.IsPostBack)
                {
                    BindProjectsList();
                }


                    //repCVsProject.Visible = false;
                    //BindForm();
                    //gvClientVariations.DataSource = projectInfo.ClientVariations;
                    //gvClientVariations.DataBind();
                

               
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



        protected void cmdGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {   if(ddlProjects.SelectedItem.Value!=String.Empty)
                BindCVs(ddlProjects.SelectedItem.Value.ToString());
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }


        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable DT = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string CVId = gvClientVariations.DataKeys[e.Row.RowIndex].Value.ToString();
                GridView gvTrade = e.Row.FindControl("gvTrades") as GridView; // (GridView)e.Row.Cells[9].Controls[0];  //
                
                DT= Getdata(CVId);
                if (DT.Rows.Count > 0)
                {  }
                    gvTrade.DataSource = DT;//string.Format("select top 3 * from Orders where CustomerId='{0}'", customerId));
                    gvTrade.DataBind();
                
                //else {
                //    string s="11";}
                
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (gvClientVariations.Rows.Count > 0)
            { ExportToExcel(); }
        }



        protected void BindCVs(string ProjectId)
        {

            ProjectsController projectsController = ProjectsController.GetInstance();
            ProcessController processController = ProcessController.GetInstance();

            try
            {
                //Security.CheckAccess(Security.userActions.ViewReports);
                String parameterProjectId = ProjectId;// Utils.CheckParameter("ProjectId");

                projectInfo = projectsController.GetProjectWithClientVariations(Int32.Parse(parameterProjectId), "CV");

                Core.Utils.CheckNullObject(projectInfo, parameterProjectId, "Project");

                projectBudget = projectsController.GetProjectBudget(projectInfo, sbiTrades.IncludeAllCVSA, sbiTrades.IncludeAllOVO);

                tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();


               
                //repCVsProject.Visible = false;
                //BindForm();
                gvClientVariations.DataSource = projectInfo.ClientVariations;
                gvClientVariations.DataBind();



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
            Response.AddHeader("content-disposition", "attachment;filename=ClientVariationsSummary.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvClientVariations.AllowPaging = false;
               // this.BindGrid();

                gvClientVariations.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvClientVariations.HeaderRow.Cells)
                {
                    cell.BackColor = gvClientVariations.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvClientVariations.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvClientVariations.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvClientVariations.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvClientVariations.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }


        //protected void ExportToPDF(object sender, EventArgs e)
        //{
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //        {
        //            //To Export all pages
        //            gvClientVariations.AllowPaging = false;
        //            //this.BindGrid();

        //            gvClientVariations.RenderControl(hw);
        //            StringReader sr = new StringReader(sw.ToString());
        //            Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
        //            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //            pdfDoc.Open();
        //            htmlparser.Parse(sr);
        //            pdfDoc.Close();

        //            Response.ContentType = "application/pdf";
        //            Response.AddHeader("content-disposition", "attachment;filename=ClientVariationsSummary.pdf");
        //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //            Response.Write(pdfDoc);
        //            Response.End();
        //        }
        //    }
        //}

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }



    }
}