using System;
using System.Collections.Generic;
using System.Linq;
using RouteChangeTracker.Models;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Processor
{
    public class Generator
    {
        private static readonly string[] FirstRouteParts = "Funny Cool Real Nice Fast Smart".Split(" ");
        private static readonly string[] SecondRouteParts = "Jerry Block-chain Express Route".Split(" ");
        private static readonly string[] FirstNames = "Isolda Genry John Carl Nil Nikita".Split(" ");
        private static readonly string[] LastNames = "Schtain Libovskiy Barkov Levko".Split(" ");
        private static readonly string[] StationNames = "Lababa Warwolt Beralan Kebasa Urbani Limas".Split(" ");

        private readonly Random _random;

        private int _stationId;
        private int _passengerId;

        public Generator(Random random = null)
        {
            _random = random ?? new Random();
        }

        public IEnumerable<Route> GenerateSimpleRoutes(int count)
        {
            Random random = new Random();
            for (var i = 0; i < count; i++)
            {
                var route = new Route
                {
                    ActiveDays = (DayOfWeekEnum)Math.Pow(2, random.Next(0, 7)),
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
                    Date = route.StartDay.AddDays(_random.Next(route.EndDay.Subtract(route.StartDay).Days)),
                    Driver = GenerateDriver(),
                    StartTime = TimeSpan.FromMinutes(_random.Next(60 * 24)),
                    Canceled = false
                };
                ride.PlannedDriver = ride.Driver;
                ride.PlannedStartTime = ride.StartTime;
                ride.Stations = GenerateStations(_random.Next(5)).ToList();
                yield return ride;
            }
        }

        public IEnumerable<Station> GenerateStations(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var station = GenerateStation();
                station.Passengers = GeneratePassengers(_random.Next(1, 6)).ToList();
                yield return station;
            }
        }

        private Station GenerateStation()
        {
            var station = new Station
            {
                Id = ++_stationId,
                Name = ChooseRandom(StationNames),
                Address = ChooseRandom(StationNames) + $" {_random.Next(333)}",
                Order = _random.Next(20),
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
                    Id = ++_passengerId,
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
            return array[_random.Next(0, array.Count)];
        }
    }
}