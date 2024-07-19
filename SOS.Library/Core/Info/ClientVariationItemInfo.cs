using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClientVariationItemInfo : Info
    {

#region Private Members
        private Decimal? amount;
        private String description;
        private int? displayOrder;

        private ClientVariationInfo clientVariation;
#endregion

#region Constructors
        public ClientVariationItemInfo() 
        {
        }

        public ClientVariationItemInfo(int? clientVariationItemId)
        {
            Id = clientVariationItemId;
        }

        public ClientVariationItemInfo(ClientVariationInfo clientVariation)
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

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public Int32? DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public ClientVariationInfo ClientVariation
        {
            get { return clientVariation; }
            set { clientVariation = value; }
        }
#endregion

    }
}
