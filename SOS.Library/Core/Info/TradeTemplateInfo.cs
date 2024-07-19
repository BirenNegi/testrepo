using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class TradeTemplateInfo : Info
    {

#region Private Members
        private bool? isStandard;
        private TradeInfo trade;
        private List<SubContractorInfo> defaultSubContractors;
#endregion

#region Constructors
        public TradeTemplateInfo()
        {
        }

        public TradeTemplateInfo(int? tradeTemplateId)
        {
            Id = tradeTemplateId;
        }
#endregion

#region Public properties
        public bool? IsStandard
        {
            get { return isStandard; }
            set { isStandard = value; }
        }

        public TradeInfo Trade
        {
            get { return trade; }
            set { trade = value; }
        }

        public List<SubContractorInfo> DefaultSubContractors
        {
            get { return defaultSubContractors; }
            set { defaultSubContractors = value; }
        }

        public String TradeName
        {
            get { return Trade != null ? Trade.Name : null; }
        }

        public String TradeCode
        {
            get { return Trade != null ? Trade.Code : null; }
        }

        public String TradeCodeAndName
        {
            get
            {
                if (TradeCode != null)
                    if (TradeName != null)
                        return TradeCode + " " + TradeName;
                    else
                        return TradeCode;
                else
                    if (TradeName != null)
                        return TradeName;
                    else
                        return null;
            }
        }

        public String TradeDescription
        {
            get { return Trade != null ? Trade.Description : null; }
        }

        public String TradeJobTypeName
        {
            get { return Trade != null ? Trade.JobTypeName : null; }
        }

        public int? TradeDaysFromPCD
        {
            get { return Trade != null ? Trade.DaysFromPCD : null; }
        }
#endregion

    }
}
