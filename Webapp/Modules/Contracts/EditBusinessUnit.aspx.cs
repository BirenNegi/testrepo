using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using SOS.Core;

namespace SOS.Web
{
    public partial class EditBusinessUnitPage : System.Web.UI.Page
    {

#region Members
        private BusinessUnitInfo businessUnitInfo = null;
        
#endregion

#region Private Methods
        private void CreateForm()
        {
            String rowStyle;
            HtmlTableRow row;
            HtmlTableCell cell;
            DropDownList dropDownListTrades;
            DropDownList dropDownListContracts;
            List<JobTypeInfo> jobTypeInfoList = ContractsController.GetInstance().GetJobTypes();
            List<ProcessTemplateInfo> processTemplates = ProcessController.GetInstance().GetProcessTemplates();

            rowStyle = "lstItem";
            foreach (JobTypeInfo jobTypeInfo in jobTypeInfoList)
            {
                row = new HtmlTableRow();
                
                cell = new HtmlTableCell();
                cell.Attributes.Add("class", rowStyle);
                cell.InnerText = jobTypeInfo.Name;
                row.Cells.Add(cell);

                dropDownListTrades = new DropDownList();
                dropDownListTrades.ID = ProcessTemplateInfo.ProcessTypeTrade + jobTypeInfo.IdStr;
                dropDownListTrades.Items.Add(new ListItem("None", String.Empty));

                dropDownListContracts = new DropDownList();
                dropDownListContracts.ID = ProcessTemplateInfo.ProcessTypeContract + jobTypeInfo.IdStr;
                dropDownListContracts.Items.Add(new ListItem("None", String.Empty));

                foreach (ProcessTemplateInfo processTemplateInfo in processTemplates)
                    switch (processTemplateInfo.ProcessType)
                    {
                        case ProcessTemplateInfo.ProcessTypeTrade:
                            dropDownListTrades.Items.Add(new ListItem(processTemplateInfo.Name, processTemplateInfo.IdStr));
                            break;
                        case ProcessTemplateInfo.ProcessTypeContract:
                            dropDownListContracts.Items.Add(new ListItem(processTemplateInfo.Name, processTemplateInfo.IdStr));
                            break;
                    }

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", rowStyle);
                cell.Controls.Add(dropDownListTrades);
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                cell.Attributes.Add("class", rowStyle);
                cell.Controls.Add(dropDownListContracts);
                row.Cells.Add(cell);

                rowStyle = rowStyle == "lstItem" ? "lstAltItem" : "lstItem";
                tblProcesses.Rows.Add(row);
            }
        }
        
        private void ObjectsToForm()
        {
            DropDownList dropDownList;
            List<JobTypeInfo> jobTypeInfoList = ContractsController.GetInstance().GetJobTypes();

            if (businessUnitInfo.Id == null)
            {
                TitleBar.Title = "Adding Business Unit";
                cmdUpdateTop.Text = "Save";
                cmdUpdateBottom.Text = "Save";
            }
            else
            {
                TitleBar.Title = "Updating Business Unit";
            }

            txtName.Text = UI.Utils.SetFormString(businessUnitInfo.Name);
            ddlProjectNumberFormat.SelectedValue = UI.Utils.SetFormString(businessUnitInfo.ProjectNumberFormat);
            txtTradeOverbudgetApproval.Text = UI.Utils.SetFormEditDecimal(businessUnitInfo.TradeOverbudgetApproval);
            txtTradeAmountApproval.Text = UI.Utils.SetFormEditDecimal(businessUnitInfo.TradeAmountApproval);
            //#----

            txtTradeComAmountApproval.Text = UI.Utils.SetFormEditDecimal(businessUnitInfo.TradeComAmountApproval);
            txtTradeComOverbudget.Text= UI.Utils.SetFormEditInteger(businessUnitInfo.TradeComOverBudget);
            txtTradeDAamountApproval.Text= UI.Utils.SetFormEditDecimal(businessUnitInfo.TradeDAAmountApproval);
            txtTradeUMOverbudgetApproval.Text= UI.Utils.SetFormEditDecimal(businessUnitInfo.TradeUMOverbudgetApproval);
            txtVariationAmtUMDAOverApproval.Text = UI.Utils.SetFormEditDecimal(businessUnitInfo.VariationUMDAOverApproval);
            if (businessUnitInfo.EstimatingDirector != null)
            {
                txtEDId.Value = businessUnitInfo.EstimatingDirector.IdStr;
                TxtED.Text = businessUnitInfo.EstimatingDirector.Name;
            }

            //#---

            txtClaimSpecialNote.Text = UI.Utils.SetFormString(businessUnitInfo.ClaimSpecialNote);
            txtVariationSepAccUMApproval.Text = UI.Utils.SetFormEditDecimal(businessUnitInfo.VariationSepAccUMApproval);  //DS20231005
            txtVariationUMBOQVCVDVApproval.Text = UI.Utils.SetFormEditDecimal(businessUnitInfo.VariationUMBOQVCVDVApproval);  //DS20231124
            if (businessUnitInfo.UnitManager != null)
            {
                txtUMId.Value = businessUnitInfo.UnitManager.IdStr;
                txtUM.Text = businessUnitInfo.UnitManager.Name;
            }
            

            cmdSelUM.NavigateUrl = Utils.PopupPeople(this, txtUMId.ClientID, txtUM.ClientID, PeopleInfo.PeopleTypeEmployee, businessUnitInfo);
            
            //#-----
                cmdSelED.NavigateUrl = Utils.PopupPeople(this, txtEDId.ClientID, TxtED.ClientID, PeopleInfo.PeopleTypeEmployee, businessUnitInfo);

            //#----

            if (businessUnitInfo.ProcessTemplates != null)
                foreach (ProcessTemplateInfo processTemplateInfo in businessUnitInfo.ProcessTemplates)
                {
                    dropDownList = (DropDownList)Utils.FindControlRecursive(tblProcesses, processTemplateInfo.ProcessType + processTemplateInfo.JobType.IdStr);
                    if (dropDownList != null)
                        dropDownList.SelectedValue = processTemplateInfo.IdStr;
                }
        }

        private void FormToObjects()
        {
            DropDownList dropDownList;
            ProcessTemplateInfo processTemplateInfo;
            List<JobTypeInfo> jobTypeInfoList = ContractsController.GetInstance().GetJobTypes();

            businessUnitInfo.Name = UI.Utils.GetFormString(txtName.Text);
            businessUnitInfo.ProjectNumberFormat = UI.Utils.GetFormString(ddlProjectNumberFormat.SelectedValue);
            businessUnitInfo.ProcessTemplates = new List<ProcessTemplateInfo>();
            businessUnitInfo.TradeOverbudgetApproval = UI.Utils.GetFormDecimal(txtTradeOverbudgetApproval.Text);
            businessUnitInfo.TradeAmountApproval = UI.Utils.GetFormDecimal(txtTradeAmountApproval.Text);
            //#---

            businessUnitInfo.TradeComAmountApproval = UI.Utils.GetFormDecimal(txtTradeComAmountApproval.Text);
            businessUnitInfo.TradeComOverBudget = UI.Utils.GetFormInteger(txtTradeComOverbudget.Text);
            businessUnitInfo.TradeDAAmountApproval = UI.Utils.GetFormDecimal(txtTradeDAamountApproval.Text);
            businessUnitInfo.EstimatingDirector = txtEDId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtEDId.Value)) : null;
            businessUnitInfo.TradeUMOverbudgetApproval = UI.Utils.GetFormDecimal(txtTradeUMOverbudgetApproval.Text);
            businessUnitInfo.VariationUMDAOverApproval = UI.Utils.GetFormDecimal(txtVariationAmtUMDAOverApproval.Text);
            businessUnitInfo.VariationSepAccUMApproval = UI.Utils.GetFormDecimal(txtVariationSepAccUMApproval.Text);  //DS20231005
            businessUnitInfo.VariationUMBOQVCVDVApproval = UI.Utils.GetFormDecimal(txtVariationUMBOQVCVDVApproval.Text);  //DS20231124
                                                                                                                      //#--
            businessUnitInfo.ClaimSpecialNote = UI.Utils.GetFormString(txtClaimSpecialNote.Text);

            businessUnitInfo.UnitManager = txtUMId.Value != "" ? new EmployeeInfo(Convert.ToInt32(txtUMId.Value)) : null;

            foreach (JobTypeInfo jobTypeInfo in jobTypeInfoList)
            {
                dropDownList = (DropDownList)Utils.FindControlRecursive(tblProcesses, ProcessTemplateInfo.ProcessTypeTrade + jobTypeInfo.IdStr);
                if (dropDownList.SelectedValue != String.Empty) {
                    processTemplateInfo = new ProcessTemplateInfo(Int32.Parse(dropDownList.SelectedValue));
                    processTemplateInfo.JobType = jobTypeInfo;
                    processTemplateInfo.ProcessType = ProcessTemplateInfo.ProcessTypeTrade;

                    businessUnitInfo.ProcessTemplates.Add(processTemplateInfo);
                }

                dropDownList = (DropDownList)Utils.FindControlRecursive(tblProcesses, ProcessTemplateInfo.ProcessTypeContract + jobTypeInfo.IdStr);
                if (dropDownList.SelectedValue != String.Empty)
                {
                    processTemplateInfo = new ProcessTemplateInfo(Int32.Parse(dropDownList.SelectedValue));
                    processTemplateInfo.JobType = jobTypeInfo;
                    processTemplateInfo.ProcessType = ProcessTemplateInfo.ProcessTypeContract;

                    businessUnitInfo.ProcessTemplates.Add(processTemplateInfo);
                }
            }
        }
#endregion

#region Event Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            String parameterBusinessUnitId = Request.Params["BusinessUnitId"];

            try
            {
                Security.CheckAccess(Security.userActions.EditBusinessUnit);

                if (parameterBusinessUnitId == null)
                    businessUnitInfo = new BusinessUnitInfo();
                else
                {
                    businessUnitInfo = ContractsController.GetInstance().GetDeepBusinessUnit(Int32.Parse(parameterBusinessUnitId));
                    Core.Utils.CheckNullObject(businessUnitInfo, parameterBusinessUnitId, "Business Unit");
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

            try {
                if (!Page.IsPostBack)
                    ObjectsToForm();
            }
            catch (Exception Ex)
            {
                Utils.ProcessPageLoadException(this, Ex);
            }
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try {
                if (Page.IsValid)
                {
                    FormToObjects();
                    businessUnitInfo.Id = ContractsController.GetInstance().AddUpdateBusinessUnit(businessUnitInfo);
                }
            }
            catch (Exception Ex) {
                Utils.ProcessPageLoadException(this, Ex);
            }

            if (Page.IsValid)
                Response.Redirect("~/Modules/Contracts/ListBusinessUnits.aspx");
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/ListBusinessUnits.aspx");
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Contract Amount";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Win";
                HeaderCell.ColumnSpan = 2;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Loss";
                HeaderCell.ColumnSpan = 3;
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderGridRow.Cells.Add(HeaderCell);

                GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            }
        }




        #endregion

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/Modules/Contracts/EditContractApprovalLimit.aspx");
        }
    }
}