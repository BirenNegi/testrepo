using System;

namespace SOS.Core
{
    [Serializable]
    public class SiteOrderDocInfo : Info
    {
        #region Public properties
        public int ProjectId { get; set; }
        public int OrderId { get; set; }
        public string DocTitle { get; set; }
        public string DocName { get; set; }
        public string DocNameOrig { get; set; }
        public string Status { get; set; }
        public string SharePointID { get; set; }
        public int isMobileUpload { get; set; }
        #endregion
        public SiteOrderDocInfo()
        {
        }
#region Constructors
        public SiteOrderDocInfo(int? SiteOrderDocId)
        {
            Id = SiteOrderDocId;
        }
#endregion
    }
}
