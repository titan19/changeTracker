using System.Collections.Generic;
using System.Linq;
using DeepEqual.Syntax;
using RouteChangeTracker.Models;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Processor
{
    public class ManualProcessor
    {
        public IEnumerable<AuditLogEntry> ProcessManual(Route[] original, Route[] updated)
        {
            return new ChangeTracker<Route[]>(original, updated)
                .CheckChangesInList(a => a.ToList(), ProcessRoute)
                .Logs;
        }

        private IEnumerable<AuditLogEntry> ProcessRoute(Route original, Route updated)
        {
            var tracker = new ChangeTracker<Route>(original, updated);
            tracker
                .CheckChanges(f => f.ActiveDays, ChangeTypeEnum.RouteDays)
                .CheckChanges(f => f.EndDay, ChangeTypeEnum.RouteEndDay)
                .CheckChanges(f => f.StartDay, ChangeTypeEnum.RouteStartDay)
                .CheckChanges(f => f.Name, ChangeTypeEnum.RouteName);
            var drivers = CreateDriversList(
                    original.Rides
                    .SelectMany(x => new[] { x.Driver, x.PlannedDriver })
                    .Concat(updated.Rides.SelectMany(x => new[] {x.Driver, x.PlannedDriver}))
                );
            foreach (var log in tracker.Logs)
            {
                log.AddApprovals(drivers);
            }

            return tracker
                .CheckChangesInList(a => a.Rides, ProcessRide)
                .Logs
                .GroupBy(x => new
                {
                    x.ChangeType,
                    x.IsPlanned,
                    x.Original,
                    x.NewValue
                })
                .Select(x => new AuditLogEntry
                    {
                        ChangeType = x.Key.ChangeType,
                        IsPlanned = x.Key.IsPlanned,
                        Original = x.Key.Original,
                        NewValue = x.Key.NewValue,
                        Approvals = x.SelectMany(a => a.Approvals).ToList(),
                        StartDate = x.Min(a => a.StartDate),
                        EndDate = x.Max(a => a.EndDate),
                        AffectedDays = x.Select(a => a.AffectedDays)
                            .Aggregate((current, next) => current | next)
                    }
                ); ;
        }

        private IEnumerable<AuditLogEntry> ProcessRide(Ride original, Ride updated)
        {
            var drivers = CreateDriversList(new[] {
                original.Driver,
                original.PlannedDriver,
                updated.Driver,
                updated.PlannedDriver
            });

            return new ChangeTracker<Ride>(original, updated)
                .CheckChangesWithPlanned(
                    f => f.StartTime,
                    f => f.PlannedStartTime,
                    ChangeTypeEnum.RideStartTime)
                .CheckChangesWithPlanned(
                    f => f.Driver,
                    f => f.PlannedDriver,
                    ChangeTypeEnum.RideDriver)
                .CheckChanges(f => f.Canceled, ChangeTypeEnum.RideStatus)
                .CheckChangesInIdentityList(
                    f => f.Stations,
                    ProcessStation,
                    ChangeTypeEnum.StationAdded,
                    ProcessStation)
                .Logs
                .Select(x => x.AddApprovals(drivers).SetOneDayDate(updated.Date));
        }

        private List<Driver> CreateDriversList(IEnumerable<Driver> drivers)
        {
            var result = new List<Driver>();
            foreach (var driver in drivers)
            {
                if (!result.Any(d => d.IsDeepEqual(driver)))
                {
                    result.Add(driver);
                }
            }

            return result;
        }

        private IEnumerable<AuditLogEntry> ProcessStation(Station newly)
        {
            return new ChangeTracker<Station>(null, newly)
                .CheckChangesInIdentityList(f => f.Passengers, ProcessPassenger, ChangeTypeEnum.PassengerAdded)
                .Logs;
        }

        private IEnumerable<AuditLogEntry> ProcessStation(Station original, Station updated)
        {
            return new ChangeTracker<Station>(original, updated)
                .CheckChanges(f => f.IsActive, ChangeTypeEnum.StationStatus)
                .CheckChanges(f => f.Name, ChangeTypeEnum.StationName)
                .CheckChanges(f => f.Address, ChangeTypeEnum.StationAddress)
                .CheckChangesWithPlanned(
                    f => f.Order,
                    f => f.PlannedOrder,
                    ChangeTypeEnum.StationOrder)
                .CheckChangesInIdentityList(
                    f => f.Passengers, 
                    ProcessPassenger, 
                    ChangeTypeEnum.PassengerAdded)
                .Logs;
        }

        private IEnumerable<AuditLogEntry> ProcessPassenger(Passenger original, Passenger updated)
        {
            return new ChangeTracker<Passenger>(original, updated)
                .CheckChanges(f => f.IsActive, ChangeTypeEnum.PassengerStatus)
                .CheckChanges(f => f.Person, ChangeTypeEnum.PassengerPerson)
                .CheckChanges(f => f.Destination, ChangeTypeEnum.PassengerDestination)
                .Logs;
        }
    }
}