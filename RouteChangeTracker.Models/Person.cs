namespace RouteChangeTracker.Models
{
    public class Person
    {
        public string FirstName;
        public string LastName;

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}