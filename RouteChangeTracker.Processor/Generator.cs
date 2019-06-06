using System;
using System.Collections.Generic;
using System.Linq;
using RouteChangeTracker.Models;

namespace RouteChangeTracker.Processor
{
    public class Generator
    {
        private readonly Random random;

        public Generator(Random random = null)
        {
            this.random = random ?? new Random();
        }

        private static readonly string[] FirstRouteParts = "Funny Cool Real Nice Fast Smart".Split(" ");
        private static readonly string[] SecondRouteParts = "Jerry Block-chain Express Route".Split(" ");
        private static readonly string[] FirstNames = "Isolda Genry John Carl Nil Nikita".Split(" ");
        private static readonly string[] LastNames = "Schtain Libovskiy Barkov Levko".Split(" ");
        public static readonly string[] StationNames = "Lababa Warwolt Beralan Kebasa Urbani Limas".Split(" ");

        public IEnumerable<Route> GenerateSimpleRoutes(int count)
        {
            Random random = new Random();
            for (var i = 0; i < count; i++)
            {
                var route = new Route
                {
                    ActiveDays = (Days)random.Next(0, 128),
                    StartDay = DateTime.Now.AddDays(random.Next(-182, 183)),
                    Name = $"{ChooseRandom(FirstRouteParts)} {ChooseRandom(SecondRouteParts)}"
                };
                route.EndDay = route.StartDay.AddDays(random.Next(0, 365));
                route.Rides = GenerateSimpleRides(random.Next(1, 6), route).ToList();
                yield return route;
            }
        }

        public IEnumerable<Ride> GenerateSimpleRides(int count, Route route)
        {
            for (var i = 0; i < count; i++)
            {
                var ride = new Ride
                {
                    Date = route.StartDay.AddDays(random.Next(route.EndDay.Subtract(route.StartDay).Days)),
                    Driver = GenerateDriver(),
                    StartTime = TimeSpan.FromMinutes(random.Next(60 * 24)),
                    Canceled = false
                };
                ride.PlannedDriver = ride.Driver;
                ride.PlannedStartTime = ride.StartTime;
                ride.Stations = GenerateStations(random.Next(5)).ToList();
                yield return ride;
            }
        }

        public IEnumerable<Station> GenerateStations(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var station = GenerateStation();
                station.Passengers = GeneratePassengers(random.Next(1, 6)).ToList();
                yield return station;
            }
        }

        private Station GenerateStation()
        {
            var station = new Station
            {
                Name = ChooseRandom(StationNames),
                Address = ChooseRandom(StationNames) + $" {random.Next(333)}",
                Order = random.Next(20),
                IsActive = true
            };
            station.PlannedOrder = station.Order;
            return station;
        }

        public IEnumerable<Passenger> GeneratePassengers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var station = new Passenger
                {
                    Person = GeneratePerson(),
                    Destination = GenerateStation(),
                    IsActive = true
                };
                yield return station;
            }
        }

        public Driver GenerateDriver()
        {
            var driver = new Driver
            {
                LicenseNumber = string.Join("",
                    ChooseRandomSeveral("ABCDEF".ToCharArray(), 2).Concat(
                        ChooseRandomSeveral("0123456789".ToCharArray(), 6))
                ),
                Person = GeneratePerson()
            };
            return driver;
        }

        public Person GeneratePerson()
        {
            return new Person
            {
                FirstName = ChooseRandom(FirstNames),
                LastName = ChooseRandom(LastNames)
            };
        }

        private IEnumerable<T> ChooseRandomSeveral<T>(IReadOnlyList<T> array, int count)
        {
            for (var i = 0; i < count; i++)
                yield return ChooseRandom(array);
        }

        private T ChooseRandom<T>(IReadOnlyList<T> array)
        {
            return array[random.Next(0, array.Count)];
        }
    }
}