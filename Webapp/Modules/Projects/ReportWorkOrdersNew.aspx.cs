using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Microsoft.Reporting.WebForms;

using SOS.Core;
using SOS.Reports;

namespace SOS.Web
{
    public partial class ReportWorkOrdersNewPage : System.Web.UI.Page
    {

#region Private Methods
        private WorkOrder AddWorkOrderSubby(Dictionary<String, Dictionary<String, WorkOrder>> workOrdersSubbyDictionary, Dictionary<String, String> tradeNamesDictionary, String subbyName, String tradeCode, String projectIdStr, String projectName, String workOrderNumber)
        {
            WorkOrder workOrderSubby = null;

            if (!workOrdersSubbyDictionary.ContainsKey(tradeCode))
                workOrdersSubbyDictionary.Add(tradeCode, new Dictionary<String, WorkOrder>());

            if (workOrdersSubbyDictionary[tradeCode].ContainsKey(subbyName))
                workOrderSubby = workOrdersSubbyDictionary[tradeCode][subbyName];
            else
            {
                workOrderSubby = new WorkOrder();

                workOrderSubby.ProjectId = projectIdStr;
                workOrderSubby.ProjectName = projectName;
                workOrderSubby.TradeCode = tradeCode;
                workOrderSubby.TradeName = tradeNamesDictionary.ContainsKey(tradeCode) ? tradeNamesDictionary[tradeCode] : "Unknown";
                workOrderSubby.SubbyName = subbyName;
                workOrderSubby.WorkOrderNumber = workOrderNumber;

                workOrdersSubbyDictionary[tradeCode].Add(subbyName, workOrderSubby);
            }

            return workOrderSubby;
        }
        
        private void BindReport()
        {
            ProjectsController projectsController = ProjectsController.GetInstance();
            Dictionary<String, Dictionary<String, WorkOrder>> workOrdersSubbyDictionary = new Dictionary<String, Dictionary<String, WorkOrder>>();
            Dictionary<String, String> tradeNamesDictionary = new Dictionary<String, String>();
            Dictionary<String, WorkOrder> workOrdersDictionary = new Dictionary<string, WorkOrder>();
            List<TradeTemplateInfo> tradeTemplateInfoList = TradesController.GetInstance().GetTradeTemplates();
            List<ProjectInfo> projectInfoList = sosFilterSelector.Projects;
            List<ReportParameter> reportParameterList = new List<ReportParameter>();
            List<WorkOrder> workOrders = new List<WorkOrder>();
            List<IBudget> projectBudget;
            WorkOrder workOrder;
            WorkOrder workOrderSubby;
            TradeBudgetInfo tradeBudgetInfo;
            VariationInfo variationInfo;
            int Cnt = 0;   //#--02/12/2019

            foreach (TradeTemplateInfo tradeTemplateInfo in tradeTemplateInfoList)
                tradeNamesDictionary.Add(tradeTemplateInfo.TradeCode, tradeTemplateInfo.TradeName);

            foreach (ProjectInfo projectInfo in projectInfoList)
            {
                workOrdersSubbyDictionary.Clear();
                workOrdersDictionary.Clear();

                    projectBudget = projectsController.GetProjectBudget(projectInfo, true, true, true).Where(b => b.TradeCode != TradeInfo.marginTradeCode).ToList();

                    if (sosFilterSelector.StartTrade != null)
                        if (sosFilterSelector.EndTrade != null)
                            projectBudget = projectBudget.Where(b => b.TradeCode.CompareTo(sosFilterSelector.StartTrade) >= 0 && b.TradeCode.CompareTo(sosFilterSelector.EndTrade) <= 0).ToList();
                        else
                            projectBudget = projectBudget.Where(b => b.TradeCode.CompareTo(sosFilterSelector.StartTrade) >= 0).ToList();
                    else
                        if (sosFilterSelector.EndTrade != null)
                            projectBudget = projectBudget.Where(b => b.TradeCode.CompareTo(sosFilterSelector.EndTrade) <= 0).ToList();

                    foreach (IBudget iBudget in projectBudget)
                    {
                       

                        if (workOrdersDictionary.ContainsKey(iBudget.TradeCode))
                            workOrder = workOrdersDictionary[iBudget.TradeCode];
                        else
                        {
                            workOrder = new WorkOrder();

                            workOrder.ProjectId = projectInfo.IdStr;
                            workOrder.ProjectName = projectInfo.Name;
                            workOrder.TradeCode = iBudget.TradeCode;
                            workOrder.TradeName = tradeNamesDictionary.ContainsKey(iBudget.TradeCode) ? tradeNamesDictionary[iBudget.TradeCode] : "Unknown";

                            workOrdersDictionary.Add(iBudget.TradeCode, workOrder);
                        }

                              #region  Old

                    //   if (iBudget.BudgetInclude)
                    //{
                    //switch (iBudget.BudgetType)
                    //{
                    //    case BudgetType.BOQ:
                    //        workOrder.BOQBudget = Core.Utils.AddValues(workOrder.BOQBudget, iBudget.BudgetAmount);
                    //        break;

                    //    case BudgetType.Contract:
                    //        tradeBudgetInfo = (TradeBudgetInfo)iBudget;

                    //        workOrderSubby = AddWorkOrderSubby(workOrdersSubbyDictionary, tradeNamesDictionary, tradeBudgetInfo.SubcontractorShortName, iBudget.TradeCode, projectInfo.IdStr, projectInfo.Name, tradeBudgetInfo.WorkOrderNumber);

                    //        if (iBudget.BudgetProvider.BudgetType == BudgetType.BOQ)
                    //        {
                    //            workOrderSubby.OriginalCommitment = Core.Utils.AddValues(workOrderSubby.OriginalCommitment, iBudget.BudgetAmount);
                    //            workOrderSubby.AllocatedBudget = Core.Utils.AddValues(workOrderSubby.AllocatedBudget, iBudget.BudgetAmountAllowance);

                    //        }
                    //        else
                    //        {
                    //            workOrderSubby.VariationsCVSA = Core.Utils.AddValues(workOrderSubby.VariationsCVSA, iBudget.BudgetAmount);
                    //        }

                    //        break;

                    //    case BudgetType.CV:
                    //    case BudgetType.SA:
                    //        workOrder.CVSABudget = Core.Utils.AddValues(workOrder.CVSABudget, iBudget.BudgetAmount);
                    //        break;

                    //    case BudgetType.Variation:
                    //        variationInfo = (VariationInfo)iBudget;

                    //        workOrderSubby = AddWorkOrderSubby(workOrdersSubbyDictionary, tradeNamesDictionary, variationInfo.SubcontractorShortName, iBudget.TradeCode, projectInfo.IdStr, projectInfo.Name, variationInfo.WorkOrderNumber);

                    //        switch (variationInfo.Type)
                    //        {
                    //            case VariationInfo.VariationTypeBillOfQuantities:
                    //                workOrderSubby.VariationsBOQ = Core.Utils.AddValues(workOrderSubby.VariationsBOQ, iBudget.BudgetAmount);
                    //                workOrderSubby.VariationsBOQBudget = Core.Utils.AddValues(workOrderSubby.VariationsBOQBudget, iBudget.BudgetAmount);
                    //                //#---30/10/2019
                    //                if (iBudget.BudgetProvider.BudgetType == BudgetType.BOQ)
                    //                {
                    //                    //workOrderSubby.OriginalCommitment = Core.Utils.AddValues(workOrderSubby.OriginalCommitment,iBudget.BudgetAmount);
                    //                    workOrderSubby.AllocatedBudget = Core.Utils.AddValues(workOrderSubby.AllocatedBudget, (iBudget.BudgetAmountInitial));
                    //                }

                    //                //#---30/10/2019
                    //                break;

                    //            case VariationInfo.VariationTypeCompany:
                    //            case VariationInfo.VariationTypeBackCharge:
                    //                workOrderSubby.VariationsVaughans = Core.Utils.AddValues(workOrderSubby.VariationsVaughans, iBudget.BudgetAmount);
                    //                break;

                    //            case VariationInfo.VariationTypeDesign:
                    //                workOrderSubby.VariationsDesign = Core.Utils.AddValues(workOrderSubby.VariationsDesign, iBudget.BudgetAmount);
                    //                break;

                    //            //this should be the same as checking the budget provider to be CV/SA but that would not work for old project
                    //            case VariationInfo.VariationTypeClient:
                    //            case VariationInfo.VariationTypeSeparateAccounts:
                    //                workOrderSubby.VariationsCVSA = Core.Utils.AddValues(workOrderSubby.VariationsCVSA, iBudget.BudgetAmount);
                    //                break;
                    //        }
                    //        break;
                    //}
                    //    }


                    //else
                    //    {
                    //        if (iBudget.BudgetType == BudgetType.Variation)
                    //        {
                    //            variationInfo = (VariationInfo)iBudget;

                    //            switch (variationInfo.Type)
                    //            {
                    //                case VariationInfo.VariationTypeCompany:
                    //                case VariationInfo.VariationTypeDesign:
                    //                //#---30/10/2019----
                    //                if (workOrdersDictionary.ContainsKey(iBudget.TradeCode))
                    //                    workOrder = workOrdersDictionary[iBudget.TradeCode];


                    //                //#---30/10/2019---
                    //                workOrder.UnapprovedVariationsVaughans = Core.Utils.AddValues(workOrder.UnapprovedVariationsVaughans, iBudget.BudgetAmount);
                    //                    break;

                    //                case VariationInfo.VariationTypeClient:
                    //                case VariationInfo.VariationTypeSeparateAccounts:
                    //                    workOrder.UnapprovedVariationsCVSA = Core.Utils.AddValues(workOrder.UnapprovedVariationsCVSA, iBudget.BudgetAmount);
                    //                    break;
                    //            }
                    //        }
                    //    }



               






                #endregion



                 //#---31/10/2019
               
                             switch (iBudget.BudgetType)
                                    {
                                      case BudgetType.BOQ:
                                         if (iBudget.BudgetInclude)
                                             workOrder.BOQBudget = Core.Utils.AddValues(workOrder.BOQBudget, iBudget.BudgetAmount);
                                             break;

                                       case BudgetType.Contract:
                                                if (iBudget.BudgetInclude)
                                                {
                                                        tradeBudgetInfo = (TradeBudgetInfo)iBudget;

                                                        workOrderSubby = AddWorkOrderSubby(workOrdersSubbyDictionary, tradeNamesDictionary, tradeBudgetInfo.SubcontractorShortName, iBudget.TradeCode, projectInfo.IdStr, projectInfo.Name, tradeBudgetInfo.WorkOrderNumber);

                                                        if (iBudget.BudgetProvider.BudgetType == BudgetType.BOQ)
                                                        {
                                                            workOrderSubby.OriginalCommitment = Core.Utils.AddValues(workOrderSubby.OriginalCommitment, iBudget.BudgetAmount);
                                                            workOrderSubby.AllocatedBudget = Core.Utils.AddValues(workOrderSubby.AllocatedBudget, iBudget.BudgetAmountAllowance);

                                                        }
                                                        else
                                                        {
                                                            workOrderSubby.VariationsCVSA = Core.Utils.AddValues(workOrderSubby.VariationsCVSA, iBudget.BudgetAmount);
                                                           //#--27/11/2019
                                                            workOrder.AllocatedVariationsCVSA = Core.Utils.AddValues(workOrder.AllocatedVariationsCVSA,iBudget.BudgetAmountInitial);
                                                           //#--27/11/2019
                                                        }
                                                }

                                                //#--02/12/2019
                                                else
                                                {
                                                    tradeBudgetInfo = (TradeBudgetInfo)iBudget;
                                                    workOrder = new WorkOrder();
                                                    workOrder.ProjectId = projectInfo.IdStr;
                                                    workOrder.ProjectName = projectInfo.Name;
                                                    workOrder.TradeCode = iBudget.TradeCode;
                                                    workOrder.TradeName = tradeNamesDictionary.ContainsKey(iBudget.TradeCode) ? tradeNamesDictionary[iBudget.TradeCode] : "Unknown";
                                                     workOrder.SubbyName = tradeBudgetInfo.SubcontractorShortName;
                                                    workOrder.UnapprovedContracts = Core.Utils.AddValues(workOrder.UnapprovedContracts, -(iBudget.BudgetAmount));

                                                    if (workOrdersDictionary.ContainsKey(iBudget.TradeCode))
                                                        {
                                                            workOrdersDictionary.Add(iBudget.TradeCode+"_"+ Cnt.ToString(), workOrder);
                                                            Cnt++;
                                                        }
                                                 }
                                                 //#--02/12/2019
                                                  break;

                                        case BudgetType.CV:
                                        case BudgetType.SA:
                                             //#-- 01/11/2019
                                            if (iBudget.BudgetInclude)
                                             {  
                                 
                                               if (workOrdersDictionary.ContainsKey(iBudget.TradeCode + " - " + iBudget.BudgetName))
                                                    workOrder = workOrdersDictionary[iBudget.TradeCode + " - " + iBudget.BudgetName];
                                                else
                                                {
                                                    workOrder = new WorkOrder();

                                                    workOrder.ProjectId = projectInfo.IdStr;
                                                    workOrder.ProjectName = projectInfo.Name;
                                                    workOrder.TradeCode = iBudget.TradeCode + " - " + iBudget.BudgetName;
                                                    workOrder.TradeName = tradeNamesDictionary.ContainsKey(iBudget.TradeCode) ? tradeNamesDictionary[iBudget.TradeCode] : "Unknown";

                                                    workOrdersDictionary.Add(iBudget.TradeCode + " - " + iBudget.BudgetName, workOrder);
                                                }  /**/
                                             //#-- 01/11/2019


                                             workOrder.CVSABudget = Core.Utils.AddValues(workOrder.CVSABudget, iBudget.BudgetAmount);
                                            }
                                             break;

                                        case BudgetType.Variation:
                                            variationInfo = (VariationInfo)iBudget;

                                            workOrderSubby = AddWorkOrderSubby(workOrdersSubbyDictionary, tradeNamesDictionary, variationInfo.SubcontractorShortName, iBudget.TradeCode, projectInfo.IdStr, projectInfo.Name, variationInfo.WorkOrderNumber);

                                        switch (variationInfo.Type)
                                        {
                                                    case VariationInfo.VariationTypeBillOfQuantities:
                                                        if (iBudget.BudgetInclude)
                                                        {
                                                            workOrderSubby.VariationsBOQ = Core.Utils.AddValues(workOrderSubby.VariationsBOQ, iBudget.BudgetAmount);
                                                            workOrderSubby.VariationsBOQBudget = Core.Utils.AddValues(workOrderSubby.VariationsBOQBudget, iBudget.BudgetAmount);
                                                            //#---30/10/2019
                                                            if (iBudget.BudgetProvider.BudgetType == BudgetType.BOQ)
                                                            {
                                                                //workOrderSubby.OriginalCommitment = Core.Utils.AddValues(workOrderSubby.OriginalCommitment,iBudget.BudgetAmount);
                                                                workOrderSubby.AllocatedBudget = Core.Utils.AddValues(workOrderSubby.AllocatedBudget, (iBudget.BudgetAmountInitial));
                                                                
                                                            }
                                                        }
                                                        //else {
                                                        //    string x = "";
                                                        //}


                                                            //#---30/10/2019
                                                    break;

                                                    case VariationInfo.VariationTypeCompany:
                                                    case VariationInfo.VariationTypeBackCharge:
                                                         if (iBudget.BudgetInclude)
                                                             workOrderSubby.VariationsVaughans = Core.Utils.AddValues(workOrderSubby.VariationsVaughans, iBudget.BudgetAmount);
                                                         else
                                                            workOrder.UnapprovedVariationsVaughans = Core.Utils.AddValues(workOrder.UnapprovedVariationsVaughans, iBudget.BudgetAmount);
                                                         break;

                                                    case VariationInfo.VariationTypeDesign:
                                                         if (iBudget.BudgetInclude)
                                                             workOrderSubby.VariationsDesign = Core.Utils.AddValues(workOrderSubby.VariationsDesign, iBudget.BudgetAmount);
                                                         else
                                                            workOrderSubby.UnapprovedVariationsVaughans = Core.Utils.AddValues(workOrderSubby.UnapprovedVariationsVaughans, iBudget.BudgetAmount);
                                                           
                                    break;

                                                    //this should be the same as checking the budget provider to be CV/SA but that would not work for old project
                                                    case VariationInfo.VariationTypeClient:
                                                    case VariationInfo.VariationTypeSeparateAccounts:
                                                    ////#-- 01/11/2019
                                                    if (workOrdersDictionary.ContainsKey(iBudget.TradeCode + " - " + iBudget.BudgetProvider.BudgetName))
                                                        workOrder = workOrdersDictionary[iBudget.TradeCode + " - " + iBudget.BudgetProvider.BudgetName];
                                                    ////#-- 01/11/2019
                                                    if (iBudget.BudgetInclude) {
                                                            workOrder.VariationsCVSA = Core.Utils.AddValues(workOrder.VariationsCVSA, iBudget.BudgetAmount);
                                                    //#--27/11/2019---
                                                          workOrder.AllocatedVariationsCVSA = Core.Utils.AddValues(workOrder.AllocatedVariationsCVSA,iBudget.BudgetAmountInitial);
                                                        }
                                                    //#--27/11/2019--
                                                       else
                                                            workOrder.UnapprovedVariationsCVSA = Core.Utils.AddValues(workOrder.UnapprovedVariationsCVSA, iBudget.BudgetAmount);
                                                       break;
                                             }
                                break;
                            }

                     }






                foreach (String key in workOrdersDictionary.Keys)
                    {
                        workOrder = workOrdersDictionary[key];
                        workOrder.Calculate = true;

                        if (workOrdersSubbyDictionary.ContainsKey(key))
                        {
                            workOrderSubby = null;

                            foreach (WorkOrder wo in workOrdersSubbyDictionary[key].Values)
                                if (workOrderSubby == null || Math.Abs(workOrderSubby.OriginalCommitment != null ? workOrderSubby.OriginalCommitment.Value : 0) < Math.Abs(wo.OriginalCommitment != null ? wo.OriginalCommitment.Value : 0))
                                    workOrderSubby = wo;

                            workOrder.AllocatedBudget = workOrderSubby.AllocatedBudget;
                            workOrder.VariationsBOQBudget = workOrderSubby.VariationsBOQBudget;
                            workOrder.OriginalCommitment = workOrderSubby.OriginalCommitment;
                            workOrder.VariationsBOQ = workOrderSubby.VariationsBOQ;
                            workOrder.VariationsVaughans = workOrderSubby.VariationsVaughans;
                            workOrder.VariationsDesign = workOrderSubby.VariationsDesign;
                            workOrder.VariationsCVSA = workOrderSubby.VariationsCVSA;
                            //#--27/11/22019
                            workOrder.AllocatedVariationsCVSA = workOrderSubby.AllocatedVariationsCVSA;

                            workOrder.AllocatedVariationsCVSATotal = workOrder.AllocatedVariationsCVSA;
                          //#--27/11/22019
                            workOrder.AllocatedBudgetTotal = workOrder.AllocatedBudget;
                            workOrder.VariationsBOQBudgetTotal = workOrder.VariationsBOQBudget;
                            workOrder.OriginalCommitmentTotal = workOrder.OriginalCommitment;
                            workOrder.VariationsBOQTotal = workOrder.VariationsBOQ;
                            workOrder.VariationsVaughansTotal = workOrder.VariationsVaughans;
                            workOrder.VariationsDesignTotal = workOrder.VariationsDesign;
                            workOrder.VariationsCVSATotal = workOrder.VariationsCVSA;

                        foreach (WorkOrder wo in workOrdersSubbyDictionary[key].Values)
                            if (wo != workOrderSubby)
                            {
                                workOrder.AllocatedBudgetTotal = Core.Utils.AddValues(workOrder.AllocatedBudgetTotal, wo.AllocatedBudget);
                                workOrder.VariationsBOQBudgetTotal = Core.Utils.AddValues(workOrder.VariationsBOQBudgetTotal, wo.VariationsBOQBudget);
                                workOrder.OriginalCommitmentTotal = Core.Utils.AddValues(workOrder.OriginalCommitmentTotal, wo.OriginalCommitment);
                                workOrder.VariationsBOQTotal = Core.Utils.AddValues(workOrder.VariationsBOQTotal, wo.VariationsBOQ);
                                workOrder.VariationsVaughansTotal = Core.Utils.AddValues(workOrder.VariationsVaughansTotal, wo.VariationsVaughans);
                                workOrder.VariationsDesignTotal = Core.Utils.AddValues(workOrder.VariationsDesignTotal, wo.VariationsDesign);
                                workOrder.VariationsCVSATotal = Core.Utils.AddValues(workOrder.VariationsCVSATotal, wo.VariationsCVSA);
                                //#--27/11/22019
                                workOrder.AllocatedVariationsCVSATotal = Core.Utils.AddValues(workOrder.AllocatedVariationsCVSATotal, wo.AllocatedVariationsCVSA);
                                //#--27/11/22019
                                workOrders.Add(wo);
                             }
                            //#---31/10/2019
                                else if (workOrdersSubbyDictionary[key].Count == 1)
                                {
                                    workOrder.UnapprovedVariationsVaughans = Core.Utils.AddValues(workOrder.UnapprovedVariationsVaughans, workOrderSubby.UnapprovedVariationsVaughans);
                                    workOrder.UnapprovedVariationsCVSA = Core.Utils.AddValues(workOrder.UnapprovedVariationsCVSA, workOrderSubby.UnapprovedVariationsCVSA);
                                }
                            //#---31/10/2019
                            workOrder.SubbyName = workOrderSubby.SubbyName;
                            workOrder.WorkOrderNumber = workOrderSubby.WorkOrderNumber;
                        }
                        else
                        {
                            workOrder.AllocatedBudgetTotal = workOrder.AllocatedBudget;
                            workOrder.VariationsBOQBudgetTotal = workOrder.VariationsBOQBudget;
                            workOrder.OriginalCommitmentTotal = workOrder.OriginalCommitment;
                            workOrder.VariationsBOQTotal = workOrder.VariationsBOQ;
                            workOrder.VariationsVaughansTotal = workOrder.VariationsVaughans;
                            workOrder.VariationsDesignTotal = workOrder.VariationsDesign;
                            workOrder.VariationsCVSATotal = workOrder.VariationsCVSA;
                            //#--27/11/2019
                            workOrder.AllocatedVariationsCVSATotal = workOrder.AllocatedVariationsCVSA;   //#--27/11/2019
                    }

                        workOrders.Add(workOrder);
                    }
                
            }

            workOrders.Sort();

            reportParameterList.Add(new ReportParameter("FilterInfo", sosFilterSelector.FilterInfo, false));

            //#--  repWorkOrders.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\WorkOrdersNew.rdlc"; 
            
            //#--- Replaced  new WorkOrders_San.rdlc
            repWorkOrders.LocalReport.ReportPath = Request.PhysicalApplicationPath + "\\Reports\\WorkOrders_San.rdlc";
            //#---
            repWorkOrders.LocalReport.SetParameters(reportParameterList);
            repWorkOrders.LocalReport.DataSources.Clear();

            repWorkOrders.LocalReport.DataSources.Add(new ReportDataSource("SOS_Reports_WorkOrder", workOrders));

            repWorkOrders.DataBind();
            repWorkOrders.Visible = true;
        }
#endregion

#region Event Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Security.CheckAccess(Security.userActions.ViewBudgetReports);

                if (!Page.IsPostBack)
                    repWorkOrders.Visible = false;

                sosFilterSelector.ActionControl = cmdGenerateReport;
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
                sosFilterSelector.ReBindLists();
                BindReport();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
             Response.Redirect("~/Modules/Core/ListReports.aspx");
        }
#endregion

    }
}
