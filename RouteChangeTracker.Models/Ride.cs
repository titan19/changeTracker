using System;
using System.Collections.Generic;

namespace RouteChangeTracker.Models
{
    public class Ride
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan PlannedStartTime { get; set; }
        public List<Station> Stations { get; set; }
        public Driver Driver { get; set; }
        public Driver PlannedDriver { get; set; }
        public bool Canceled { get; set; }
    }
}