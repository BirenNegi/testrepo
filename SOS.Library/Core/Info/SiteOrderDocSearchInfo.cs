using System;

namespace SOS.Core
{
    [Serializable]
    public class SiteOrderDocSearchInfo : Info
    {
        #region Public properties
        public string DocTitle { get; set; }
        public string DocName { get; set; }
        public string LastName { get; set; }
        public DateTime DocDate { get; set; }
        #endregion

        public SiteOrderDocSearchInfo()
        {
        }
#region Constructors
        public SiteOrderDocSearchInfo(int? SiteOrderDocId)
        {
            Id = SiteOrderDocId;
        }
#endregion
    }
}
