using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ProcessTemplateInfo : Info
    {

#region Constants
        public const String ProcessTypeTrade = "T";
        public const String ProcessTypeContract = "C";
        public const String ProcessTypeClientVariation = "V";
        public const String ProcessTypeClaim = "M";
        public const String ProcessTypeSeparateAccounts = "S";
#endregion

#region Private Members
        private String name;
        private String processType;

        private JobTypeInfo jobType;
#endregion

#region Constructors
        public ProcessTemplateInfo() 
        {
        }

        public ProcessTemplateInfo(int? processInfoId)
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

        public String ProcessType
        {
            get { return processType; }
            set { processType = value; }
        }

        public JobTypeInfo JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }
#endregion
    
    }
}
