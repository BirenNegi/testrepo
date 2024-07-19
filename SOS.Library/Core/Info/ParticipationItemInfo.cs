using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ParticipationItemInfo : Info
    {

#region Private Members
        private bool? isIncluded;
        private bool? confirmed;
        private decimal? amount;
        private String quantity;
        private String notes;
        private TradeParticipationInfo tradeParticipation;
        private TradeItemInfo tradeItem;
#endregion

#region Constructors
        public ParticipationItemInfo() 
        {
        }

        public ParticipationItemInfo(int? tradeParticipationId)
        {
            Id = tradeParticipationId;
        }
#endregion

#region Public properties
        public bool? IsIncluded
        {
            get { return isIncluded; }
            set { isIncluded = value; }
        }

        public bool? Confirmed
        {
            get { return confirmed; }
            set { confirmed = value; }
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

        public Boolean IsEmpty
        {
            get
            {
                return IsIncluded == null && Confirmed == null && Amount == null && Quantity == null && Notes == null;
            }
        }

        public TradeParticipationInfo TradeParticipation
        {
            get { return tradeParticipation; }
            set { tradeParticipation = value; }
        }

        public TradeItemInfo TradeItem
        {
            get { return tradeItem; }
            set { tradeItem = value; }
        }
#endregion

    }
}
 