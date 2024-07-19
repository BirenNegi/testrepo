using System;

namespace SOS.Core
{
    [Serializable]
    public class SiteOrderDetailInfo : Info
    {
        public int OrderID { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public decimal? Qty { get; set; }
        public DateTime ETA { get; set; }
        public string UM { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }

        public SiteOrderDetailInfo()
        {
        }

        public SiteOrderDetailInfo(int? OrderId)
        {
            Id = OrderId;
        }
    }
}
