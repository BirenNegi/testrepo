using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClaimVariationInfo : Info, IClaimDetail
    {

#region Private Members
        private Decimal? amount;

        private ClaimInfo claim;
        private ClientVariationInfo clientVariation;
#endregion

#region Constructors
        public ClaimVariationInfo() 
        {
        }
#endregion

#region Public properties
        public Decimal? Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public ClaimInfo Claim
        {
            get { return claim; }
            set { claim = value; }
        }

        public ClientVariationInfo ClientVariation
        {
            get { return clientVariation; }
            set { clientVariation = value; }
        }

        public int? ClientVariationNumber
        {
            get
            {
                return ClientVariation != null ? ClientVariation.Number : null;
            }
        }

        public String ClientVariationName
        {
            get
            {
                return ClientVariation != null ? ClientVariation.Name : null;
            }
        }

        public String ClientVariationNumberAndRevisionName
        {
            get
            {
                return ClientVariation != null ? ClientVariation.NumberAndRevisionName : null;
            }
        }

        public Decimal? ClientVariationAmount
        {
            get
            {
                return ClientVariation != null ? ClientVariation.TotalAmount : null;
            }
        }

        public Decimal? ClientVariationPercentaje
        {
            get
            {
                if (ClientVariation != null)
                    if (ClientVariation.TotalAmount != null)
                        if (Amount != null)
                            if (ClientVariation.TotalAmount != 0)
                                return (Amount) / ClientVariation.TotalAmount;

                return null;
            }
        }
#endregion

    }
}
