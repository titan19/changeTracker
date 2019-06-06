using System.Collections.Generic;

namespace RouteChangeTracker.Models
{
    public class Station
    {
        public string Name;
        public string Address;
        public List<Passenger> Passengers;
        public int Order;
        public int PlannedOrder;
        public bool IsActive;
    }
}