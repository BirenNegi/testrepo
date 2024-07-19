using System;

namespace SOS.Reports
{
    public class PendingTask
    {

#region Public properties
        public String BusinessUnitIdStr { get; set; }
        public String BusinessUnitName { get; set; }
        public String ProjectIdStr { get; set; }
        public String ProjectName { get; set; }
        public String EmployeeIdStr { get; set; }
        public String EmployeeName { get; set; }
        public String EntityType { get; set; }
        public String EntityName { get; set; }
        public String Task { get; set; }
        public DateTime? TargetDate { get; set; }
        public int? DueDays { get; set; }
#endregion

#region Public Methods
#endregion

    }
}
