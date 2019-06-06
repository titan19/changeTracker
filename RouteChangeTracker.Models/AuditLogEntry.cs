using System;
using System.Collections.Generic;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Models
{
    public class AuditLogEntry
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public DayOfWeekEnum AffectedDays;
        public string ChangeType;
        public object Original;
        public object NewValue;
        public bool IsPlanned;
        public List<Approval> Approvals;
    }
}