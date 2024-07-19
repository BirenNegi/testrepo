using System;

namespace SOS.Core
{
    [Serializable]
    public class ContractTemplateInfo : Info
    {

#region Constants
        public const String ContractTypeStandard = "ST";
        public const String ContractTypeSimplified = "SP";
#endregion

#region Private Members
        private String standardTemplate;
        private String simplifiedTemplate;
        private String variationTemplate;
        private BusinessUnitInfo businessUnit;
        private JobTypeInfo jobType;
#endregion

#region Constructors
        public ContractTemplateInfo() 
        {
        }

        public ContractTemplateInfo(int? contractTemplateId)
        {
            Id = contractTemplateId;
        }
#endregion

#region Public properties
        public String StandardTemplate
        {
            get { return standardTemplate; }
            set { standardTemplate = value; }
        }

        public String SimplifiedTemplate
        {
            get { return simplifiedTemplate; }
            set { simplifiedTemplate = value; }
        }

        public String VariationTemplate
        {
            get { return variationTemplate; }
            set { variationTemplate = value; }
        }

        public BusinessUnitInfo BusinessUnit
        {
            get { return businessUnit; }
            set { businessUnit = value; }
        }

        public JobTypeInfo JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }
#endregion

    }
}
