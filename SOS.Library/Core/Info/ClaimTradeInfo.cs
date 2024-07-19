using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class ClaimTradeInfo : Info, IClaimDetail
    {

#region Private Members
        private Decimal? amount;

        private ClaimInfo claim;
        private ClientTradeInfo clientTrade;
#endregion

#region Constructors
        public ClaimTradeInfo() 
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

        public ClientTradeInfo ClientTrade
        {
            get { return clientTrade; }
            set { clientTrade = value; }
        }

        public String ClientTradeName
        {
            get
            {
                return ClientTrade != null ? ClientTrade.Name : null;
            }
        }

        public Decimal? ClientTradeAmount
        {
            get
            {
                return ClientTrade != null ? ClientTrade.Amount : null;
            }
        }

        public Decimal? ClientTradePercentaje
        {
            get
            {
                if (ClientTrade != null)
                    if (ClientTrade.Amount != null)
                        if (Amount != null)
                            if (ClientTrade.Amount != 0)
                                return (Amount) / ClientTrade.Amount;

                return null;
            }
        }
#endregion

    }
}
