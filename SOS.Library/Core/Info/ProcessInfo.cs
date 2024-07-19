using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ProcessInfo : Info
    {

#region Constants
        public const String ProcessComparison = "Comparisons";
        public const String ProcessContract = "Contracts";
        public const String ProcessVariation = "Variation Orders";
        public const String ProcessClientVariation = "Client Variations";
        public const String ProcessTenantVariation = "Tenant Variations";  // DS20240213
        public const String ProcessSeparateAccount = "Separate Accounts";
        public const String ProcessClaim = "Claims";
        public const String ProcessCreateClaim = "Pending Claim";
        public const String ProcessTransmittal = "Transmittals";
        public const String ProcessRFI = "RFIs";
        public const String ProcessEOT = "EOTs";
        public const String ProcessParticipation = "Quotes reminders";
        public const String ProcessProject = "Project Files";
        public const String ProcessUnknown = "Unknown";
        //#------
        public const String ProcessMissingSignedContractFile = "Missing Signed Contract File";
        public const String ProcessMeetingMinutes = "Meeting Minutes";
        //#----

        #endregion

        #region Private Members
        private String name;
        private String stepComparisonApproval;
        private String stepContractApproval;

        private ProjectInfo project;

        private List<ProcessStepInfo> steps;
        private List<ReversalInfo> reversals;
#endregion

#region Constructors
        public ProcessInfo() 
        {
        }

        public ProcessInfo(int? processInfoId)
        {
            Id = processInfoId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String StepComparisonApproval
        {
            get { return stepComparisonApproval; }
            set { stepComparisonApproval = value; }
        }

        public String StepContractApproval
        {
            get { return stepContractApproval; }
            set { stepContractApproval = value; }
        }

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        public List<ProcessStepInfo> Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        public List<ReversalInfo> Reversals
        {
            get { return reversals; }
            set { reversals = value; }
        }

        public ProcessStepInfo LastExecutedStep
        {
            get
            {
                int i;

                if (Steps != null)
                {
                    i = Steps.Count - 1;
                    while (i >= 0)
                    {
                        if (Steps[i].ActualDate != null)
                            return Steps[i];
                        else
                            i--;
                    }
                }

                return null;
            }
        }
        public int? Hide { get; set; }
        #endregion

    }
}
