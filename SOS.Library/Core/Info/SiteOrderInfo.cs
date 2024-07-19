using System;
using System.Collections.Generic;

namespace SOS.Core
{
    [Serializable]
    public class SiteOrderInfo : Info
    {
        public string Title { get; set; }
        public int SubContractorId { get; set; }
        public int ProjectId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Status { get; set; }
        public string Typ { get; set; }
        public DateTime? DateStart { get; set; }
        public int ForemanID { get; set; }
        public DateTime? DateEnd { get; set; }
        // DS20230304 public Decimal? DeliveryFee { get; set; }
        public Decimal? ItemsTotal { get; set; }
        public Decimal? SubTotal { get; set; }
        // DS20230304 public Decimal? GST { get; set; }
        // DS20230304 public Decimal? Total { get; set; }
        public string Contact { get; set; }
        // DS20230304 public string ContactPhone { get; set; }
        public string PeriodType { get; set; }
        public string PaymentStatus { get; set; } //EmployeeInfo
        public int VariationID { get; set; }
        public string Email { get; set; }
        public int ContactPeopleId { get; set; }
        public string OrderCode { get; set; }
        public string ProjectName { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Locality { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        // DS20230304 public Decimal? GSTRate { get; set; }
        public string ABN { get; set; }
        public int isOrderApproved { get; set; }
        
        public string ForemanName { get; set; }
        public string SubContractorName { get; set; }
        public int isMobile { get; set; }                    // DS20230304
        public int GivenByPeopleId { get; set; }             // DS20230304
        public string GivenByName { get; set; }              // DS20230304
        public string Comments { get; set; }                 // DS20230304
        public string TypeInfo { get; set; }                 // DS20230304
        public string TradesCode { get; set; }               // DS20230306
        public string RowColor { get; set; }               // DS20230403
        public string BusinessUnitName { get; set; }               // DS20230403
        public int DocCount { get; set; }                    // DS20230710

        public List<SiteOrderDetailInfo> Items;

        public List<SiteOrderDocSearchInfo> Docs;

        public SiteOrderInfo()
        {
            Items = new List<SiteOrderDetailInfo>();
            Docs = new List<SiteOrderDocSearchInfo>();
        }
        public SiteOrderInfo(int? OrderId)
        {
            Id = OrderId;
            Items = new List<SiteOrderDetailInfo>();
            Docs = new List<SiteOrderDocSearchInfo>();
        }

        public string StatusDescription
        {
            get
            {
                if (Status == "AT")
                    return "Active";
                if (Status == "CN")      // DS20231102
                    return "Cancelled";
                else if (Status == "CP")
                    return "Complete";
                else if (Status == "RJ")
                    return "Rejected";
                else
                    return "";
            }
        }
        public string TypDescription
        {
            get
            {
                if (Typ == "Mat")
                    return "Material Order";
                else if (Typ == "Ins")
                    return "Site Instruction";
                else if (Typ == "Hir")
                    return "Equipment Hire";
                else
                    return "";
            }
        }
    }
}
