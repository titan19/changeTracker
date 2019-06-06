using System;
using System.Collections.Generic;

namespace RouteChangeTracker.Models
{
    public class Ride
    {
        public DateTime Date;
        public TimeSpan StartTime;
        public TimeSpan PlannedStartTime;
        public List<Station> Stations;
        public Driver Driver;
        public Driver PlannedDriver;
        public bool Canceled;
    }
}