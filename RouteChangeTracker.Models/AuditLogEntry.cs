using System;
using System.Collections.Generic;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Models
{
    public class AuditLogEntry
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeekEnum AffectedDays { get; set; }
        public string ChangeType { get; set; }
        public object Original { get; set; }
        public object NewValue { get; set; }
        public bool IsPlanned { get; set; }
        public List<Approval> Approvals { get; set; }
    }
}