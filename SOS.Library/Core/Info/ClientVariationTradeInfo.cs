using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClientVariationTradeInfo : Info, IBudget
    {

        #region Private Members
        private Decimal? amount;
        private String tradeCode;

        private ClientVariationInfo clientVariation;
        #endregion

        #region Constructors
        public ClientVariationTradeInfo()
        {
        }

        public ClientVariationTradeInfo(int? clientVariationTradeId)
        {
            Id = clientVariationTradeId;
        }

        public ClientVariationTradeInfo(ClientVariationInfo clientVariation)
        {
            ClientVariation = clientVariation;
        }
        #endregion

        #region Public properties
        public Decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public String TradeCode
        {
            get { return tradeCode; }
            set { tradeCode = value; }
        }

        public ClientVariationInfo ClientVariation
        {
            get { return clientVariation; }
            set { clientVariation = value; }
        }
        #endregion

        #region IBudget implementation
        public decimal? BudgetAmountInitial { get; set; }
        public decimal? BudgetAmountTradeInitial { get; set; }
        public decimal? BudgetAmountAllowance { get; set; }
        public decimal? BudgetUnallocated { get; set; }
        public IBudget BudgetProvider { get; set; }

        public Decimal BudgetAmount
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
                return clientVariation.ApprovalDate;
            }
        }

        public Boolean BudgetInclude
        {
            get
            {
                return BudgetDate != null && !clientVariation.IsCancel;
            }
        }

        public String BudgetName
        {
            get { return clientVariation.NumberAndRevisionName; }
        }

        public BudgetType BudgetType
        {
            //#---get { return clientVariation is SeparateAccountInfo ? BudgetType.SA : BudgetType.CV; } 
            get { return clientVariation is SeparateAccountInfo ? BudgetType.SA : (clientVariation is TenantVariationInfo) ? BudgetType.TV : BudgetType.CV; }

            //#---TV
        }

        public bool Equals(IBudget iBudget)
        {
            return base.EqualsType((Info)iBudget);
        }
        #endregion

    }
}
