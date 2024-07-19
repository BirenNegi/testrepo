using System;

namespace SOS.Core
{
    [Serializable]
    public class ContractModificationInfo : Info
    {

#region Private Members
        private String sectionName;
        private String sectionText;
        private ContractInfo contract;
        private EmployeeInfo employee;
#endregion

#region Constructors
        public ContractModificationInfo() 
        {
        }

        public ContractModificationInfo(int? contractModificationId)
        {
            Id = contractModificationId;
        }
#endregion

#region Public properties
        public String SectionName
        {
            get { return sectionName; }
            set { sectionName = value; }
        }

        public String SectionText
        {
            get { return sectionText; }
            set { sectionText = value; }
        }

        public ContractInfo Contract
        {
            get { return contract; }
            set { contract = value; }
        }

        public EmployeeInfo Employee
        {
            get { return employee; }
            set { employee = value; }
        }
#endregion

    }
}
