using System.Collections.Generic;

namespace RouteChangeTracker.Models
{
    public class Station : Identity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Passenger> Passengers { get; set; }
        public int Order { get; set; }
        public int PlannedOrder { get; set; }
        public bool IsActive { get; set; }
    }
}