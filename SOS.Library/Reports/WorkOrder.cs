using System;
using SOS.Core;

namespace SOS.Reports
{
    public class WorkOrder: IComparable
    {

#region Public properties
        public String ProjectName { get; set; }
        public String ProjectId { get; set; }
        public String TradeCode { get; set; }
        public String TradeName { get; set; }
        public String SubbyName { get; set; }
        public String WorkOrderNumber { get; set; }

        public Decimal? BOQBudget { get; set; }
        public Decimal? AllocatedBudget { get; set; }
        public Decimal? VariationsBOQBudget { get; set; }
        public Decimal? CVSABudget { get; set; }
        public Decimal? OriginalCommitment { get; set; }
        public Decimal? VariationsBOQ { get; set; }
        public Decimal? VariationsVaughans { get; set; }
        public Decimal? VariationsDesign { get; set; }
        public Decimal? VariationsCVSA { get; set; }
        //#--27/11/2019
        public Decimal? AllocatedVariationsCVSA { get; set; }
        public Decimal? AllocatedVariationsCVSATotal { get; set; }
        //#-- 27/11/2019
        public Decimal? UnapprovedVariationsVaughans { get; set; }
        public Decimal? UnapprovedVariationsCVSA { get; set; }
        public Decimal? AllocatedBudgetTotal { get; set; }
        public Decimal? VariationsBOQBudgetTotal { get; set; }
        public Decimal? OriginalCommitmentTotal { get; set; }
        public Decimal? VariationsBOQTotal { get; set; }
        public Decimal? VariationsVaughansTotal { get; set; }
        public Decimal? VariationsDesignTotal { get; set; }
        public Decimal? VariationsCVSATotal { get; set; }

        public Boolean Calculate { get; set; }

        private Boolean CalculateWinLoss
        {
            get
            {
                return TotalCommitmentTotal != null;
            }
        }

        private Decimal? TotalCommitmentTotal
        {
            get
            {
                return Utils.AddValues(Utils.AddValues(Utils.AddValues(Utils.AddValues(OriginalCommitmentTotal, VariationsBOQTotal), VariationsVaughansTotal), VariationsDesignTotal), VariationsCVSATotal);
            }
        }

        public Decimal? UnallocatedBudget
        {
            get
            {
                //#--1/11/2019-- Decimal? unallocatedBudget = Calculate ? Utils.AddValues(Utils.AddValues(BOQBudget, AllocatedBudgetTotal, true), VariationsBOQBudgetTotal) : null;

                Decimal? unallocatedBudget = Calculate ? Utils.AddValues(BOQBudget, AllocatedBudgetTotal, true) : null;   //#--1/11/2019--

                if (unallocatedBudget != null && unallocatedBudget.Value < 0)
                    unallocatedBudget = 0;

                return unallocatedBudget;
            }
        }

        public Decimal? TotalCommitment
        {
            get
            {
                return Utils.AddValues(Utils.AddValues(Utils.AddValues(Utils.AddValues(OriginalCommitment, VariationsBOQ), VariationsVaughans), VariationsDesign), VariationsCVSA);
            }
        }

        public Decimal? WinLossContract
        {
            get
            {
                //--#--30/10/2019    return CalculateWinLoss ? Utils.AddValues(AllocatedBudgetTotal, OriginalCommitmentTotal) : null;

                return CalculateWinLoss ? (OriginalCommitmentTotal== null? 0:Utils.AddValues(AllocatedBudgetTotal, OriginalCommitmentTotal)) : null;    //--#--30/10/2019
            }
        }

        public Decimal? WinLossCVSA
        {
            get
            {
                return CalculateWinLoss ? Utils.AddValues(CVSABudget, VariationsCVSATotal) : null;
            }
        }

        public Decimal? WinLossTotal
        {
            get
            {
                return CalculateWinLoss ? Utils.AddValues(Utils.AddValues(BOQBudget, CVSABudget), TotalCommitmentTotal) : null;
            }
        }


        //#--01/11/2019
        public Decimal? UnapprovedContracts { get; set; }
        //#--01/11/2019




        #endregion

        #region Public Methods
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            else
            {
                WorkOrder otherWorkOrder = (WorkOrder)obj;

                if (this.ProjectId == otherWorkOrder.ProjectId)
                    if (this.TradeCode == otherWorkOrder.TradeCode)
                        if (this.OriginalCommitment != null && otherWorkOrder.OriginalCommitment != null)
                            return Math.Abs(this.OriginalCommitment.Value).CompareTo(Math.Abs(otherWorkOrder.OriginalCommitment.Value));
                        else
                            //#-----02/12/2019-- return this.SubbyName.CompareTo(otherWorkOrder.SubbyName);
                            return 1;     //#---02/12/2019

                    else
                        return this.TradeCode.CompareTo(otherWorkOrder.TradeCode);
                else
                    return this.ProjectId.CompareTo(otherWorkOrder.ProjectId);
            }
        }
#endregion

    }
}
