using System;
using System.Collections.Generic;

namespace SOS.Core
{

    #region Constants
    public enum BudgetType
    {
        BOQ,
        CV,
        SA,
        TV,
        Contract,
        Variation,
    }
    #endregion

    [Serializable]
    public class BudgetInfo : Info, IBudget
    {

        #region Private Members
        private String code;
        private String name;
        private Decimal? amount;

        private ProjectInfo project;
        #endregion

        #region Constructors
        public BudgetInfo()
        {
        }

        public BudgetInfo(int? budgetInfoId)
        {
            Id = budgetInfoId;
        }
        #endregion

        #region Public properties
        public String Code
        {
            get { return code; }
            set { code = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String CodeAndName
        {
            get
            {
                if (Code != null)
                    if (Name != null)
                        return Code + " " + Name;
                    else
                        return Code;
                else
                    if (Name != null)
                    return Name;
                else
                    return null;
            }
        }

        public Decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public ProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        public Decimal? Adjustments { get; set; }
        #endregion

        #region IBudget implementation
        public decimal? BudgetAmountInitial { get; set; }
        public decimal? BudgetAmountTradeInitial { get; set; }
        public decimal? BudgetAmountAllowance { get; set; }
        public decimal? BudgetUnallocated { get; set; }
        public IBudget BudgetProvider { get; set; }

        public String TradeCode
        {
            get { return Code; }
        }

        public decimal BudgetAmount
        {
            get { return amount.HasValue ? amount.Value : 0; }
        }

        public Decimal? BudgetWinLoss
        {
            get
            {
                return null;
            }
        }

        public DateTime? BudgetDate
        {
            get
            {
                return CreatedDate;
            }
        }

        public Boolean BudgetInclude
        {
            get { return true; }
        }

        public String BudgetName
        {
            get { return Name; }
        }

        public BudgetType BudgetType
        {
            get { return BudgetType.BOQ; }
        }

        public bool Equals(IBudget iBudget)
        {
            return base.EqualsType((Info)iBudget);
        }
        #endregion

    }
}
