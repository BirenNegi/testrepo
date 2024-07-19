using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class VariationInfo : Info, IBudget
    {

        #region Constants
        public const String VariationTypeCompany = "V";
        public const String VariationTypeDesign = "DV";
        public const String VariationTypeBackCharge = "BC";
        public const String VariationTypeBillOfQuantities = "BOQ";
        public const String VariationTypeClient = "CV";
        public const String VariationTypeSeparateAccounts = "SA";
        public const String VariationTypeTenant = "TV";
        #endregion

        #region Private Members
        private String header;
        private String description;
        private String tradeCode;
        private decimal? amount;
        private decimal? allowance;
        private decimal? cVAllowance;
        private String number;

        private IBudget budgetProvider;
        private ContractInfo contract;
        #endregion

        #region Constructors
        public VariationInfo()
        {
        }

        public VariationInfo(int? variationId)
        {
            Id = variationId;
        }

        public VariationInfo(int? variationId, IBudget budgetProvider)
        {
            Id = variationId;
            this.budgetProvider = budgetProvider;
        }
        #endregion

        #region Public properties
        public String Header
        {
            get { return header; }
            set { header = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public decimal? Allowance
        {
            get { return allowance; }
            set { allowance = value; }
        }

        public decimal? CVAllowance
        {
            get { return cVAllowance; }
            set { cVAllowance = value; }
        }

        public String Number
        {
            get { return number; }
            set { number = value; }
        }

        public decimal? AllowanceMinusAmount
        {
            get
            {
                return Allowance != null ? Amount != null ? Allowance - Amount : null : null;
            }
        }

        public decimal? CVAllowanceMinusAmount
        {
            get
            {
                return CVAllowance != null ? Amount != null ? CVAllowance - Amount : null : null;
            }
        }

        public decimal? AllowanceBudget
        {
            get
            {
                if (budgetProvider != null)
                {
                    return budgetProvider.BudgetAmountInitial;
                }
                else
                {
                    //---#---
                    if (this.Type == VariationInfo.VariationTypeClient || this.Type == VariationInfo.VariationTypeSeparateAccounts || this.Type == VariationInfo.VariationTypeTenant)
                    {
                        return CVAllowance;
                    }
                    else
                    {
                        return Allowance;
                    }
                }
            }
        }

        public Boolean IsBOQ
        {
            get
            {
                return Type == VariationTypeCompany || Type == VariationTypeDesign || Type == VariationTypeBillOfQuantities;
            }
        }

        public Boolean IsCV
        {
            get
            {
                return Type == VariationTypeClient;
            }
        }

        public Boolean IsSA
        {
            get
            {
                return Type == VariationTypeSeparateAccounts;
            }
        }
        //--#---
        public Boolean IsTV
        {
            get
            {
                return Type == VariationTypeTenant;
            }
        }
        //--#---



        public ContractInfo Contract
        {
            get { return contract; }
            set { contract = value; }
        }


        public String SubcontractorShortName
        {
            get
            {
                return Contract.SubcontractorShortName;
            }
        }

        public String WorkOrderNumber
        {
            get
            {
                return Contract.WorkOrderNumber;
            }
        }
        #endregion

        #region IBudget implementation
        public decimal? BudgetAmountTradeInitial { get; set; }
        public decimal? BudgetAmountAllowance { get; set; }
        public decimal? BudgetUnallocated { get; set; }

        public decimal? BudgetAmountInitial
        {
            get { return allowance; }
            set { allowance = value; }
        }

        public String TradeCode
        {
            get { return budgetProvider != null ? budgetProvider.TradeCode : tradeCode; }
            set { tradeCode = value; }
        }

        public IBudget BudgetProvider
        {
            get { return budgetProvider; }
            set { budgetProvider = value; }
        }

        public Decimal BudgetAmount
        {
            get { return amount.HasValue ? -amount.Value : 0; }
        }

        public Decimal? BudgetWinLoss
        {
            get
            {
                if (Allowance.HasValue && Amount.HasValue)
                    return Allowance.Value - Amount.Value;
                else
                    return null;
            }
        }

        public DateTime? BudgetDate
        {
            get
            {
                return contract.ApprovalDate;
            }
        }

        public Boolean BudgetInclude
        {
            get { return BudgetDate != null; }
        }

        public String BudgetName
        {
            get { return String.Format("{0}-{1}", Type, header); }
        }

        public BudgetType BudgetType
        {
            get { return BudgetType.Variation; }
        }

        public bool Equals(IBudget iBudget)
        {
            return base.EqualsType((Info)iBudget);
        }
        #endregion

    }
}
