using System;
using System.Collections.Generic;
using System.Text;
using RouteChangeTracker.Models;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Processor
{
    public static class Extensions
    {
        public static AuditLogEntry AddApprovals(this AuditLogEntry log, IEnumerable<Driver> drivers)
        {
            if (log.Approvals == null)
                log.Approvals = new List<Approval>();
            foreach (var driver in drivers)
            {
                log.Approvals.Add(new Approval { Driver = driver });
            }

            return log;
        }

        public static AuditLogEntry SetOneDayDate(this AuditLogEntry log, DateTime date)
        {
            log.StartDate = log.EndDate = date.Date;
            log.AffectedDays = (DayOfWeekEnum)date.DayOfWeek;

            return log;
        }
    }
}
