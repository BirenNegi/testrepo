using System;

namespace SOS.Reports
{
    public class BidsChart : IComparable
    {

#region Public properties
        public String ProjectName { get; set; }
        public String SubcontractorName { get; set; }
        public DateTime? ProjectsCommencementDate { get; set; }
        public int? SubcontractorId { get; set; }
        public int? Rank { get; set; }
        public int? StrikeRate { get; set; }
#endregion

#region Public Methods
        public int CompareTo(Object obj)
        {
            if (this.StrikeRate != null && ((BidsChart)obj).StrikeRate != null)
                return ((int)this.StrikeRate).CompareTo((int)((BidsChart)obj).StrikeRate);
            else
                throw new ArgumentException("BidsChart objects have null StrikeRate.");
        }
#endregion

    }
}
