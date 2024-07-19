using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ProcessStepInfo : Info
    {

#region Constants
        public const String StepTypeComparisonCA = "COMCA";
        public const String StepTypeComparisonPM = "COMPM";
        public const String StepTypeComparisonCM = "COMCM";
        //#---Added new role Commercial Manager on Comparison approval process
        public const String StepTypeComparisonCO = "COMCO";
        //#---

        public const String StepTypeComparisonDA = "COMDA";
        public const String StepTypeComparisonUM = "COMUM";
        public const String StepTypeCreateContract = "CRECO";
        public const String StepTypeGenerateWorkOrder = "GENWO";
        public const String StepTypeContractDC = "CONDC";
        public const String StepTypeContractDM = "CONDM";
        public const String StepTypeContractCA = "CONCA";
        //#----Added new role Commercial Manager  on contract  approval process
        public const String StepTypeContractCO = "CONCO";
        public const String StepTypeContractDA = "CONDA";
        public const String StepTypeContractUM = "CONUM";
        //#---Added new role Commercial Manager  on contract  approval process

        public const String StepTypeContractPM = "CONPM";
        public const String StepTypeContractCM = "CONCM";
        public const String StepTypeContractMD = "CONMD";


        public const String StepTypeClientVariationCA = "CLVCA";
        public const String StepTypeClientVariationPM = "CLVPM";
        public const String StepTypeClientVariationInternalApproval = "CLVIA";
        public const String StepTypeClientVariationClientVerbalApproval = "CLVVA";
        public const String StepTypeClientVariationClientFinalApproval = "CLVFA";
        public const String StepTypeClientVariationWorksCompleted = "CLVWC";
        public const String StepTypeClientVariationSendInvoice = "CLVSI";
        public const String StepTypeClientVariationInvoicePaid = "CLVIP";
        public const String StepTypeClaimDraftCA = "CLDCA";
        public const String StepTypeClaimDraftApproval = "CLDDA";
        public const String StepTypeClaimInvoiceCA = "CLICA";
        public const String StepTypeClaimInvoiceInternalApproval = "CLIIA";
        public const String StepTypeClaimInvoiceFinalApproval = "CLIFA";

        //#--New Steps added for Claim after Finay Approvalby FC

        public const String StepTypeClaimFinalPaymentApprovalByPM = "CLFPP";
        public const String StepTypeClaimFinalPaymentApprovalByFC = "CLFPF";

        //#--New Steps added for Claim after Finay Approvalby FC
        public const String StepTypeClientVariationUM = "CVSUM";    // DS20231208


        #endregion

        #region Private Members
        private String stepType;
        private String name;
        private String role;
        private int? numDays;
        private int? order;
        private DateTime? actualDate;
        private DateTime? targetDate;
        private String comments;
        private int? dueDays;
        private bool? skip;

        private PeopleInfo assignedTo;
        private PeopleInfo approvedBy;
        private ProcessInfo process;

        private String stepAssigneeName;

        private List<ReversalInfo> reversals;
#endregion

#region Constructors
        public ProcessStepInfo() 
        {
        }

        public ProcessStepInfo(int? processStepInfoId)
        {
            Id = processStepInfoId;
        }

        public ProcessStepInfo(String type, String name, String role, int? oreder)
        {
            Type = type;
            Name = name;
            Role = role;
            Order = order;
        }
#endregion

#region Public properties
        public String StepType
        {
            get { return stepType; }
            set { stepType = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Role
        {
            get { return role; }
            set { role = value; }
        }

        public int? NumDays
        {
            get { return numDays; }
            set { numDays = value; }
        }

        public int? Order
        {
            get { return order; }
            set { order = value; }
        }

        public DateTime? ActualDate
        {
            get { return actualDate; }
            set { actualDate = value; }
        }

        public DateTime? TargetDate
        {
            get { return targetDate; }
            set { targetDate = value; }
        }

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public int? DueDays
        {
            get { return dueDays; }
            set { dueDays = value; }
        }

        public bool? Skip
        {
            get { return skip; }
            set { skip = value; }
        }

        public PeopleInfo AssignedTo
        {
            get { return assignedTo; }
            set { assignedTo = value; }
        }

        public PeopleInfo ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        public ProcessInfo Process
        {
            get { return process; }
            set { process = value; }
        }

        public String StepAssigneeName
        {
            get { return stepAssigneeName; }
            set { stepAssigneeName = value; }
        }

        public List<ReversalInfo> Reversals
        {
            get { return reversals; }
            set { reversals = value; }
        }

        public String Status
        {
            get { return ActualDate != null ? "Approved" : "Pending"; }
        }

        public String ApprovedByName
        {
            get { return ApprovedBy != null ? ApprovedBy.Name : null; }
        }

        public Boolean HasPendingReversal
        {
            get
            {
                if (reversals != null)
                    foreach (ReversalInfo reversal in reversals)
                        if (reversal.IsPending)
                            return true;

                return false;
            }            
        }

        public ReversalInfo PendingReversal
        {
            get
            {
                if (reversals != null)
                    foreach (ReversalInfo reversal in reversals)
                        if (reversal.IsPending)
                            return reversal;

                return null;
            }            
        }

        public Boolean SkipStep
        {
            get { return Skip != null ? (Boolean)skip : false; }
        }

        public String ProcessName
        {
            get {
                if (Process.Project != null)
                {
                    if (Process.Project.Claims != null)
                        if (Process.Project.Claims[0].Id != null)
                            return ProcessInfo.ProcessClaim;
                        else
                            return ProcessInfo.ProcessCreateClaim;

                    if (Process.Project.ClientVariations != null)
                        if (Process.Project.ClientVariations[0] is SeparateAccountInfo)
                            return ProcessInfo.ProcessSeparateAccount;
                        else
                        if (Process.Project.ClientVariations[0] is TenantVariationInfo) // DS20240213
                            return ProcessInfo.ProcessTenantVariation; // DS20240213
                        else                                           // DS20240213
                            return ProcessInfo.ProcessClientVariation;


                    //#--------
                    if (Process.Project.Trades != null && Process.Project.Trades[0].Contract == null && Process.Name == "Missing Signed Contract File")
                        return ProcessInfo.ProcessMissingSignedContractFile;
                    //#---------



                    if (Process.Project.Trades != null)
                        if (Process.Project.Trades[0].Participations != null)
                            return ProcessInfo.ProcessParticipation;
                        else
                            if (Process.Project.Trades[0].Contract == null)
                                return ProcessInfo.ProcessComparison;
                            else
                                if (Process.Project.Trades[0].Contract.ParentContract == null)
                                    return ProcessInfo.ProcessContract;
                                else
                                    return ProcessInfo.ProcessVariation;

                    


                    if (Process.Project.Transmittals != null)
                        return ProcessInfo.ProcessTransmittal;

                    if (Process.Project.RFIs != null)
                        return ProcessInfo.ProcessRFI;

                    if (Process.Project.EOTs != null)
                        return ProcessInfo.ProcessEOT;

                    return ProcessInfo.ProcessProject;
                }
                else
                {
                    return ProcessInfo.ProcessUnknown;
                }
            }
        }

#endregion

    }
}
