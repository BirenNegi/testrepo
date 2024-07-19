using System;
using System.Xml;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using SOS.Core;

namespace SOS.Web
{
    public partial class ProcessManagerControl : System.Web.UI.UserControl
    {

#region Members
        private ProcessInfo processInfo = null;
        private String tradeIdStr = null;
        private Boolean enabled = true;

        public event EventHandler ApproveClicked;
        public event EventHandler ReverseClicked;
#endregion

#region Public properties
        public ProcessInfo Process
        {
            get { return processInfo; }
            set { processInfo = value; }
        }

        public String TradeIdStr
        {
            get { return tradeIdStr; }
            set { tradeIdStr = value; }
        }

        public int? StepId
        {
            get { return ViewState["StepId"] != null ? (int?)ViewState["StepId"] : null; }
            set { ViewState["StepId"] = value; }
        }

        public Boolean Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public String Comments
        {
            get { return UI.Utils.GetFormString(hidComments.Value); }
        }
#endregion

#region Private Methods
        private void AddCellHighlightedText(HtmlTableRow row, String text, String style)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.Attributes.Add("style", "font-weight:bold;");
            cell.InnerText = text;
            row.Cells.Add(cell);
        }

        private void AddLabelError(HtmlTableCell cell, String text)
        {
            Label label = new Label();
            label.Attributes.Add("class", "frmError");
            label.Text = text;
            cell.Controls.Add(label);
        }

        private void AddCellText(HtmlTableRow row, String text, String style)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.InnerText = text;
            row.Cells.Add(cell);
        }

        private void AddCellTextCenter(HtmlTableRow row, String text, String style)
        {
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("class", style);
            cell.InnerText = text;
            cell.Align = "Center";
            row.Cells.Add(cell);
        }

        private void AddRowStep(HtmlTable table, ProcessStepInfo processStepInfo, String stepNumberStr, String style, Boolean isCurrent)
        {
            HtmlTableRow row = new HtmlTableRow();

            AddCellTextCenter(row, UI.Utils.SetFormString(stepNumberStr), style);

            if (isCurrent && Enabled)
                AddCellHighlightedText(row, UI.Utils.SetFormString(processStepInfo.Name), style);
            else
                AddCellText(row, UI.Utils.SetFormString(processStepInfo.Name), style);

            AddCellText(row, UI.Utils.SetFormString(processStepInfo.Status), style);
            AddCellText(row, UI.Utils.SetFormString(processStepInfo.StepAssigneeName), style);
            AddCellText(row, UI.Utils.SetFormString(processStepInfo.ApprovedByName), style);
            AddCellTextCenter(row, UI.Utils.SetFormDate(processStepInfo.TargetDate), style);
            AddCellTextCenter(row, UI.Utils.SetFormDateTime(processStepInfo.ActualDate), style);

            table.Rows.Add(row);
        }

        private void AddRowHeader(HtmlTable table)
        {
            HtmlTableRow row = new HtmlTableRow();

            AddCellText(row, String.Empty, "lstHeader");
            AddCellText(row, "Step", "lstHeader");
            AddCellText(row, "Status", "lstHeader");
            AddCellText(row, "Assigned to", "lstHeader");
            AddCellText(row, "Approved by", "lstHeader");
            AddCellText(row, "Target Date", "lstHeader");
            AddCellText(row, "Actual Date/Time", "lstHeader");

            table.Rows.Add(row);
        }

        private void AddRowReversalsHeader(HtmlTable table)
        {
            HtmlTableRow row = new HtmlTableRow();

            AddCellText(row, "Reversed By", "lstSimpleTitle");
            AddCellText(row, "Date/Time", "lstSimpleTitle");
            AddCellText(row, "Comments", "lstSimpleTitle");
            AddCellText(row, "Approved By", "lstSimpleTitle");
            AddCellText(row, "Date/Time", "lstSimpleTitle");
            AddCellText(row, "Comments", "lstSimpleTitle");

            table.Rows.Add(row);
        }

        private void AddRowReversal(HtmlTable table, ReversalInfo reversalInfo)
        {
            HtmlTableRow row = new HtmlTableRow();

            AddCellText(row, UI.Utils.SetFormString(reversalInfo.ReversalByName), "lstSimpleNoWrap");
            AddCellTextCenter(row, UI.Utils.SetFormDateTime(reversalInfo.ReversalDate), "lstSimpleNoWrap");
            AddCellText(row, UI.Utils.SetFormString(reversalInfo.ReversalNote), "lstSimple");
            AddCellText(row, UI.Utils.SetFormString(reversalInfo.ReplyByName), "lstSimpleNoWrap");
            AddCellTextCenter(row, UI.Utils.SetFormDateTime(reversalInfo.ReplyDate), "lstSimpleNoWrap");
            AddCellText(row, UI.Utils.SetFormString(reversalInfo.ReplyNote), "lstSimple");

            table.Rows.Add(row);
        }

        private void AddRowReversals(HtmlTable tblApproval, ProcessStepInfo processStepInfo, String style)
        {
            HtmlTable table = new HtmlTable();
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();

            table.CellPadding = 3;
            table.CellSpacing = 0;
            table.Attributes.Add("style", "border:#333333 1px solid;");

            AddCellText(row, String.Empty, style);

            cell.ColSpan = 6;
            cell.Attributes.Add("class", style);

            AddRowReversalsHeader(table);

            foreach (ReversalInfo reversalInfo in processStepInfo.Reversals)
                AddRowReversal(table, reversalInfo);

            cell.Controls.Add(table);
            row.Cells.Add(cell);

            tblApproval.Rows.Add(row);
        }

        private void AddRowCreateContract(HtmlTable table, ProcessStepInfo currentStep, String style, String checkProcessStep)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            HyperLink lnkCreateContract;

            AddCellText(row, String.Empty, style);

            cell.ColSpan = 6;
            cell.Attributes.Add("class", style);

            if (checkProcessStep != null)
                AddLabelError(cell, checkProcessStep);
            else
            {
                lnkCreateContract = new HyperLink();
                lnkCreateContract.Text = "Create contract";
                lnkCreateContract.NavigateUrl = "~/Modules/Contracts/CreateContract.aspx?TradeId=" + TradeIdStr + "&ProcessStepId=" + currentStep.IdStr;
                lnkCreateContract.CssClass = "frmLink";
                cell.Controls.Add(lnkCreateContract);
            }

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        private void AddRowGenerateWorkOrder(HtmlTable table, ProcessStepInfo currentStep, String style, String checkProcessStep)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            Button cmdApprove;

            AddCellText(row, String.Empty, style);

            cell.ColSpan = 6;
            cell.Attributes.Add("class", style);

            if (checkProcessStep != null)
                AddLabelError(cell, checkProcessStep);
            else
            {
                cmdApprove = new Button();
                cmdApprove.ID = "cmdApprove";
                cmdApprove.Text = "Generate Work Order";
                cmdApprove.Click += new EventHandler(cmdApprove_Click);
                cmdApprove.OnClientClick = "return confirm('Execute step: " + currentStep.Name + "');";
                cell.Controls.Add(cmdApprove);
            }

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        private void AddRowSendInvoice(HtmlTable table, ProcessStepInfo currentStep, String style, String checkProcessStep)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            Button cmdApprove;

            AddCellText(row, String.Empty, style);

            cell.ColSpan = 6;
            cell.Attributes.Add("class", style);

            if (checkProcessStep != null)
                AddLabelError(cell, checkProcessStep);
            else
            {
                cmdApprove = new Button();
                cmdApprove.ID = "cmdApprove";
                cmdApprove.Text = "Send Invoice";
                cmdApprove.Click += new EventHandler(cmdApprove_Click);
                cmdApprove.OnClientClick = "return confirm('Execute step: " + currentStep.Name + "');";
                cell.Controls.Add(cmdApprove);
            }

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        private void AddRowApprove(HtmlTable table, ProcessStepInfo currentStep, String style, String checkProcessStep, String cmdText)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            Button cmdApprove;
            String strScript;
            String scriptName;

            AddCellText(row, String.Empty, style);

            cell.ColSpan = 6;
            cell.Attributes.Add("class", style);

            if (checkProcessStep != null)
                AddLabelError(cell, checkProcessStep);
            else
            {
                cmdApprove = new Button();
                cmdApprove.ID = "cmdApprove";
                cmdApprove.Text = cmdText;
                cmdApprove.Click += new EventHandler(cmdApprove_Click);

                if (currentStep.HasPendingReversal)
                {
                    scriptName = ClientID + "_ReApproveStepPrompt";

                    strScript = "" +
                    "<script language='JavaScript'>\r" +
                    "function " + scriptName + "() {\r" +
                    "   var reason = prompt('Approval comments: " + currentStep.Name + "?', '');\r" +
                    "   if (reason != null) {\r" +
                    "      if (reason.replace(/^\\s\\s*/, '').replace(/\\s\\s*$/, '') == '') {\r" +
                    "         alert('You must provide a comment.')\r" +
                    "         return false;\r" +
                    "      }\r" +
                    "      else {\r" +
                    "         document.getElementById('" + hidComments.ClientID + "').value = reason;\r" +
                    "         return true;\r" +
                    "      }\r" +
                    "   }\r" +
                    "   else {\r" +
                    "      return false;\r" +
                    "   }\r" +
                    "}\r" +
                    "</script>\r";

                    if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptName))
                        Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptName, strScript);

                    cmdApprove.OnClientClick = "return " + scriptName + "();";
                }

                else
                {

                    //#-- To check Target Date is <= currentdate for Claim Final payment approval


                    if ((currentStep.Type == ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByFC || currentStep.Type == ProcessStepInfo.StepTypeClaimFinalPaymentApprovalByPM) && currentStep.TargetDate.Value > DateTime.Now)
                    {
                        scriptName = ClientID + "_ApproveStepPrompt";

                        strScript = "" +
                        "<script language='JavaScript'>\r" +
                        "function " + scriptName + "() {\r" +
                        "         alert('You will be able to approve this on, or after the Target Date')\r" +
                        "         return false;\r" +
                        "}\r" +
                        "</script>\r";

                        if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptName))
                            Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptName, strScript);

                        cmdApprove.OnClientClick = "return " + scriptName + "();";

                    }

                    //#-- Tocheck Target Date is <= currentdate for Finalpayment approval

                    else
                    {
                        cmdApprove.OnClientClick = "return confirm('Execute step: " + currentStep.Name + "');";
                    }
                }



                cell.Controls.Add(cmdApprove);
            }

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

        private void AddRowReverse(HtmlTable table, ProcessStepInfo previousStep, String style)
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            String strScript;
            String scriptName;
            Button cmdReverse;

            AddCellText(row, String.Empty, style);

            cell.ColSpan = 6;
            cell.Attributes.Add("class", style);

            scriptName = ClientID + "_ReverseStepPrompt";

            strScript = "" +
            "<script language='JavaScript'>\r" +
            "function " + scriptName + "() {\r" +
            "   var reason = prompt('Reason to reverse step: " + previousStep.Name + "?', '');\r" +
            "   if (reason != null) {\r" +
            "      if (reason.replace(/^\\s\\s*/, '').replace(/\\s\\s*$/, '') == '') {\r" +
            "         alert('You must provide a comment.')\r" +
            "         return false;\r" +
            "      }\r" +
            "      else {\r" +
            "         document.getElementById('" + hidComments.ClientID + "').value = reason;\r" +
            "         return true;\r" +
            "      }\r" +
            "   }\r" +
            "   else {\r" +
            "      return false;\r" +
            "   }\r" +
            "}\r" +
            "</script>\r";

            if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptName))
                Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptName, strScript);

            cmdReverse = new Button();
            cmdReverse.ID = "cmdReverse";
            cmdReverse.Text = "Reverse";
            cmdReverse.Click += new EventHandler(cmdReverse_Click);
            cmdReverse.OnClientClick = "return " + scriptName + "();";

            cell.Controls.Add(cmdReverse);

            row.Cells.Add(cell);
            table.Rows.Add(row);
        }

            public void BindApproval()
        {
            ProcessController processController = ProcessController.GetInstance();
            ProcessStepInfo currentStep;
            ProcessStepInfo previousStep = null;
            ProcessStepInfo nextStep = null;   // DS20231211
            String checkProcessStep = null;
            String currStyle = "";
            Int16 stepNumber = 0;
            Boolean isCurrentStep;
            Boolean allowApprovalCurrentStep = false;
            Boolean permissionApprovalPreviousStep = false;
            bool isReverseAdded = false;   // DS20231220

            tblApproval.Rows.Clear();
            AddRowHeader(tblApproval);

            if (Process != null && Process.Steps != null)
            {
                if (StepId != null)
                    currentStep = Process.Steps.Find(delegate(ProcessStepInfo processStepInfoInList) { return processStepInfoInList.Id == StepId; });
                else
                    currentStep = processController.GetCurrentStep(Process);

                if (currentStep != null)
                {
                    StepId = currentStep.Id;
                    previousStep = processController.GetPreviousProcessStep(currentStep);
                    checkProcessStep = processController.CheckProcessStep(currentStep);
                    allowApprovalCurrentStep = processController.AllowApprovalCurrentUser(currentStep);

                    if (previousStep != null)
                        permissionApprovalPreviousStep = processController.PermissionApprovalCurrentUser(previousStep);
                }

                foreach (ProcessStepInfo processStepInfo in Process.Steps)
                {
                    if (!processStepInfo.SkipStep)
                    {
                        stepNumber++;
                        processStepInfo.StepAssigneeName = processController.GetStepAssigneeName(processStepInfo);
                        currStyle = currStyle == "lstItem" ? "lstAltItem" : "lstItem";

                        isCurrentStep = currentStep != null && processStepInfo.Equals(currentStep);
                        if (processStepInfo.Type == processStepInfo.Process.StepContractApproval)    // DS202312
                        {
                            if (processStepInfo.StepAssigneeName != null) { processStepInfo.StepAssigneeName = processStepInfo.StepAssigneeName + " *"; }
                        }
                        if (processStepInfo.Type == processStepInfo.Process.StepComparisonApproval)    // DS202405
                        {
                            if (processStepInfo.StepAssigneeName != null) { processStepInfo.StepAssigneeName = processStepInfo.StepAssigneeName + " *"; }
                        }
                        AddRowStep(tblApproval, processStepInfo, stepNumber.ToString(), currStyle, isCurrentStep);

                        if (processStepInfo.Reversals != null && processStepInfo.Reversals.Count > 0)
                            AddRowReversals(tblApproval, processStepInfo, currStyle);

                        //#---if (Enabled && previousStep != null && processStepInfo.Equals(previousStep) && allowApprovalCurrentStep && permissionApprovalPreviousStep && processController.IsReversible(previousStep))
                        nextStep = processController.GetNextProcessStep(processStepInfo);         // DS20231211
                        bool isStepUM = false;  // DS20231211 >>>
                        if (nextStep != null)
                        {
                            if (nextStep.Type == ProcessStepInfo.StepTypeClientVariationUM || nextStep.Type == ProcessStepInfo.StepTypeContractUM)
                            {
                                if (nextStep.Skip == false)
                                   isStepUM = true;
                            }
                        }
                        if (isStepUM == true)
                        {
                            if (previousStep != null && isCurrentStep == false)
                            {
                                if (currentStep.Type == ProcessStepInfo.StepTypeClientVariationUM || previousStep.Type == ProcessStepInfo.StepTypeContractCM)
                                {
                                    // if (ProcessController.GetInstance().IsReversible(previousStep, currentStep))
                                    if (isReverseAdded == false)
                                    { 
                                    if (ProcessController.GetInstance().AllowApprovalCurrentUser(currentStep))
                                    AddRowReverse(tblApproval, previousStep, currStyle);
                                    isReverseAdded = true;
                                    }
                                }
                                
                            }
                        }

                        //if (isCurrentStep == true)
                        //{
                        if (isReverseAdded == false)
                        {
                            if (previousStep != null && previousStep.Type != ProcessStepInfo.StepTypeClientVariationUM)    // removed comments
                            {


                                if (Enabled && previousStep != null && processStepInfo.Equals(previousStep) && allowApprovalCurrentStep && processController.IsReversible(previousStep, processStepInfo))  //DS20231212
                                    //DS20231004->PEEK
                                {
                                    AddRowReverse(tblApproval, previousStep, currStyle);
                                    isReverseAdded = true;
                                }
                            else
                                {

                                }  // DS20231211 <<<
                            }
                        }
                        //}
                        if (Enabled && isCurrentStep && allowApprovalCurrentStep)
                        {
                            switch (currentStep.Type)
                            {
                                case ProcessStepInfo.StepTypeCreateContract:
                                    AddRowCreateContract(tblApproval, currentStep, currStyle, checkProcessStep);
                                    break;
                                case ProcessStepInfo.StepTypeGenerateWorkOrder:
                                    AddRowGenerateWorkOrder(tblApproval, currentStep, currStyle, checkProcessStep);
                                    break;
                                case ProcessStepInfo.StepTypeClientVariationClientVerbalApproval:
                                    AddRowApprove(tblApproval, currentStep, currStyle, checkProcessStep, "Record Verbal Approval");
                                    break;
                                case ProcessStepInfo.StepTypeClientVariationClientFinalApproval:
                                    AddRowApprove(tblApproval, currentStep, currStyle, checkProcessStep, "Record Final Approval");
                                    break;
                                case ProcessStepInfo.StepTypeClientVariationSendInvoice:
                                    AddRowSendInvoice(tblApproval, currentStep, currStyle, checkProcessStep);
                                    break;
                                default:
                                    AddRowApprove(tblApproval, currentStep, currStyle, checkProcessStep, "Approve");
                                    break;
                            }
                        }
                    }
                }
            }
        }
#endregion

#region Public Methods

#endregion

#region Event Handlers
        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            if (ApproveClicked != null)
                ApproveClicked(this, new EventArgs());
        }

        protected void cmdReverse_Click(object sender, EventArgs e)
        {
            if (ReverseClicked != null)
                ReverseClicked(this, new EventArgs());
        }
#endregion

    }
}
