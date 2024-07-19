using System;

namespace SOS.Core
{
    public interface IBudget
    {

#region Public properties
        int? Id { get; set; }
        String TradeCode { get; }
        Decimal BudgetAmount { get; }
        Decimal? BudgetAmountInitial { get; set; }
        Decimal? BudgetAmountTradeInitial { get; set; }
        Decimal? BudgetAmountAllowance { get; set; }
        Decimal? BudgetUnallocated { get; set; }
        Decimal? BudgetWinLoss { get; }
        DateTime? BudgetDate { get; }
        Boolean BudgetInclude { get; }
        String BudgetName { get; }
        BudgetType BudgetType { get; }

        IBudget BudgetProvider { get; set; }

        bool Equals(IBudget iBudget);
#endregion

    }
}
