using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TradeBudgetInfo : Info, IBudget
    {

#region Private properties
        private TradeInfo trade;
        private IBudget budgetProvider;
#endregion

#region Constructors
        public TradeBudgetInfo() 
        {
        }

        public TradeBudgetInfo(TradeInfo trade, IBudget budgetProvider)
        {
            this.trade = trade;
            this.budgetProvider = budgetProvider;
        }
#endregion

#region Public properties
        public int? TradeId
        {
            get { return trade.Id; }
        }

        public int? BudgetProviderId
        {
            get { return budgetProvider.Id; }
        }

        public String WorkOrderNumber
        {
            get
            {
                return trade.WorkOrderNumber;
            }
        }

        public String SubcontractorShortName
        {
            get
            {
                return trade.Participations[0].SubContractor.ShortName;
            }
        }
#endregion

#region IBudget implementation
        public decimal? BudgetAmountInitial { get; set; }
        public decimal? BudgetAmountTradeInitial { get; set; }
        public decimal? BudgetAmountAllowance { get; set; }
        public decimal? BudgetUnallocated { get; set; }
        public Decimal? Amount { get; set; }

        public String TradeCode
        {
            get { return budgetProvider.TradeCode; }
        }

        public Boolean BudgetInclude
        {
            get { return BudgetDate != null; }
        }

        public IBudget BudgetProvider
        {
            get { return budgetProvider; }
            set { budgetProvider = value; }
        }

        public Decimal BudgetAmount
        {
            get { return Amount.HasValue ? -Amount.Value : 0; }
        }

        public Decimal? BudgetWinLoss
        {
            get
            {
                if (BudgetAmountAllowance.HasValue && Amount.HasValue)
                    return BudgetAmountAllowance.Value - Amount.Value;
                else
                    return null;
            }
        }

        public String BudgetName
        {
            get { return "Main trade: " + trade.Name; }
        }

        public BudgetType BudgetType
        {
            get { return BudgetType.Contract; }
        }

        public DateTime? BudgetDate
        {
            get { return trade.Contract != null ? trade.Contract.ApprovalDate : null; }
        }

        public bool Equals(IBudget iBudget)
        {
            return base.EqualsType((Info)iBudget);
        }
#endregion

    }
}
