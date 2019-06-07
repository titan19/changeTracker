namespace RouteChangeTracker.Models
{
    public class Passenger : Identity
    {
        public Person Person { get; set; }
        public Station Destination { get; set; }
        public bool IsActive { get; set; }
    }
}