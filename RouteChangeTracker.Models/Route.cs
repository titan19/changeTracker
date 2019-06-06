using System;
using System.Collections.Generic;
using System.Text;

namespace RouteChangeTracker.Models
{
    public class Route
    {
        public string Name;
        public Days ActiveDays;
        public List<Ride> Rides;
        public DateTime StartDay;
        public DateTime EndDay;
    }
}
