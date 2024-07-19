using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TradeItemCategoryInfo : Info, ISortable
    {

#region Private Members
        private String name;
        private String shortDescription;
        private String longDescription;
        private int? displayOrder;
        private TradeInfo trade;
        private List<TradeItemInfo> tradeItems;
#endregion

#region Constructors
        public TradeItemCategoryInfo() 
        {
        }

        public TradeItemCategoryInfo(int? tradeItemCategoryId)
        {
            Id = tradeItemCategoryId;
        }
#endregion

#region Public properties
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String ShortDescription
        {
            get { return shortDescription; }
            set { shortDescription = value; }
        }

        public String LongDescription
        {
            get { return longDescription; }
            set { longDescription = value; }
        }

        public int? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public TradeInfo Trade
        {
            get { return trade; }
            set { trade = value; }
        }

        public List<TradeItemInfo> TradeItems
        {
            get { return tradeItems; }
            set { tradeItems = value; }
        }
#endregion

    }
}
