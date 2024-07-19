using System;

namespace SOS.Core
{
    [Serializable]
    public class TradeItemInfo : Info, ISortable
    {

#region Private Members
        private String name;
        private String units;
        private String scope;
        private int? displayOrder;
        private bool? isIncluded;
        private bool? requiresQuantityCheck;
        private bool? requiredInProposal;
        private decimal? amount;
        private String quantity;
        private String notes;
        private TradeItemCategoryInfo tradeItemCategory;
#endregion

#region Constructors
        public TradeItemInfo() 
        {
        }

        public TradeItemInfo(int? tradeItemId)
        {
            Id = tradeItemId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Units
        {
            get { return units; }
            set { units = value; }
        }

        public String Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        public int? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public bool? IsIncluded
        {
            get { return isIncluded; }
            set { isIncluded = value; }
        }

        public bool? RequiresQuantityCheck
        {
            get { return requiresQuantityCheck; }
            set { requiresQuantityCheck = value; }
        }

        public bool? RequiredInProposal
        {
            get { return requiredInProposal; }
            set { requiredInProposal = value; }
        }

        public decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public String Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public String Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public TradeItemCategoryInfo TradeItemCategory
        {
            get { return tradeItemCategory; }
            set { tradeItemCategory = value; }
        }

        public String TradeItemCategoryShortDescription
        {
            get { return tradeItemCategory != null ? tradeItemCategory.ShortDescription : String.Empty; }
        }

        public Boolean IsRequiredInProposal
        {
            get { return requiredInProposal == null ? false : (Boolean)requiredInProposal; }
        }
#endregion

    }
}
