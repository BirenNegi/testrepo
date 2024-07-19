using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class BusinessUnitInfo : Info
    {

#region Private Members
        private String name;
        private String projectNumberFormat;
        private Decimal? tradeOverbudgetApproval;
        private Decimal? tradeAmountApproval;
        //#--
        private Decimal? tradeComAmountApproval;
        private int? tradeComOverBudget;
        private Decimal? tradeDAAmountApproval;
        private Decimal? tradeUMOverbudgetApproval;
        private Decimal? variationUMDAOverApproval;
        private EmployeeInfo estimatingDirector;
        //#--
        private String claimSpecialNote;
        private Decimal? variationSepAccUMApproval; //DS20231005
        private Decimal? variationUMBOQVCVDVApproval; //DS20231124
        private EmployeeInfo unitManager;
        private List<ProcessTemplateInfo> processTemplates;
#endregion

#region Constructors
        public BusinessUnitInfo() 
        {
        }

        public BusinessUnitInfo(int? businessUnitId)
        {
            Id = businessUnitId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String ProjectNumberFormat
        {
            get { return projectNumberFormat; }
            set { projectNumberFormat = value; }
        }

        public Decimal? TradeOverbudgetApproval
        {
            get { return tradeOverbudgetApproval; }
            set { tradeOverbudgetApproval = value; }
        }

        public Decimal? TradeAmountApproval
        {
            get { return tradeAmountApproval; }
            set { tradeAmountApproval = value; }
        }

        //#--
        public Decimal? TradeComAmountApproval
        {
            get { return tradeComAmountApproval; }
            set { tradeComAmountApproval = value; }
        }

        public int? TradeComOverBudget
        {
            get { return tradeComOverBudget; }
            set { tradeComOverBudget = value; }
        }

        public Decimal? TradeDAAmountApproval
        {
            get { return tradeDAAmountApproval; }
            set { tradeDAAmountApproval = value; }
        }
        public EmployeeInfo EstimatingDirector
        {
            get { return estimatingDirector; }
            set { estimatingDirector = value; }
        }


        public Decimal? TradeUMOverbudgetApproval
        {
            get { return tradeUMOverbudgetApproval; }
            set { tradeUMOverbudgetApproval = value; }
        }

        
        public Decimal? VariationUMDAOverApproval
        {
            get { return variationUMDAOverApproval; }
            set { variationUMDAOverApproval = value; }
        }


        //#---



        public String ClaimSpecialNote
        {
            get { return claimSpecialNote; }
            set { claimSpecialNote = value; }
        }
        public Decimal? VariationSepAccUMApproval   // DS20231005
        {
            get { return variationSepAccUMApproval; }
            set { variationSepAccUMApproval = value; }
        }
        public Decimal? VariationUMBOQVCVDVApproval   // DS20231124
        {
            get { return variationUMBOQVCVDVApproval; }
            set { variationUMBOQVCVDVApproval = value; }
        }

        public EmployeeInfo UnitManager
        {
            get { return unitManager; }
            set { unitManager = value; }
        }

        public List<ProcessTemplateInfo> ProcessTemplates
        {
            get { return processTemplates; }
            set { processTemplates = value; }
        }

        public String UnitManagerName
        {
            get { return UnitManager != null ? UnitManager.Name : null; }
        }
#endregion
 
    }
}
