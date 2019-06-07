using System;
using System.Collections.Generic;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Models
{
    public class Route
    {
        public string Name { get; set; }
        public DayOfWeekEnum ActiveDays { get; set; }
        public List<Ride> Rides { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
    }
}
