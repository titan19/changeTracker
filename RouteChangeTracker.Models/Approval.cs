namespace RouteChangeTracker.Models
{
    public class Approval : Identity
    {
        public Driver Driver;
        public bool IsApproved;
    }
}