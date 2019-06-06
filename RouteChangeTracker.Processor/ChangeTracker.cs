using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual.Syntax;
using RouteChangeTracker.Models;
using RouteChangeTracker.Models.Enums;

namespace RouteChangeTracker.Processor
{
    public class ChangeTracker<T>
    {
        private readonly T _original;
        private readonly T _updated;
        public IEnumerable<AuditLogEntry> Logs { get; private set; } = Enumerable.Empty<AuditLogEntry>();

        public ChangeTracker(T original, T updated)
        {
            _original = original;
            _updated = updated;
        }

        private static readonly Dictionary<ChangeTypeEnum, string> ChangeTypeCodes = new Dictionary<ChangeTypeEnum, string>
        {
            {ChangeTypeEnum.RouteDays,"Route active days changed"},
            {ChangeTypeEnum.RouteEndDay,"Route end day changed"},
            {ChangeTypeEnum.RouteStartDay,"Route start day changed"},
            {ChangeTypeEnum.RouteName,"Route name changed"},
            {ChangeTypeEnum.RideStartTime,"Ride Start time changed"},
            {ChangeTypeEnum.RideDriver,"Ride driver changed"},
            {ChangeTypeEnum.RideStatus,"Ride status changed"},
            {ChangeTypeEnum.StationStatus,"Station status changed"},
            {ChangeTypeEnum.StationName,"Station name changed"},
            {ChangeTypeEnum.StationAddress,"Station address changed"},
            {ChangeTypeEnum.StationOrder,"Station order changed"},
            {ChangeTypeEnum.PassengerStatus,"Passenger status changed"},
            {ChangeTypeEnum.PassengerPerson,"Passenger person changed"},
            {ChangeTypeEnum.PassengerDestination,"Passenger destination changed"}
        };

        public ChangeTracker<T> CheckChangesInList<TS>(
            Func<T, List<TS>> selector,
            Func<TS, TS, IEnumerable<AuditLogEntry>> processor)
        {
            var original = selector(_original);
            var updated = selector(_updated);
            if (original.Count != updated.Count)
                throw new InvalidOperationException($"Different list size");

            for (var i = 0; i < original.Count; i++)
            {
                Logs = Logs.Concat(processor(original[i], updated[i]));
            }

            return this;
        }

        public ChangeTracker<T> CheckChangesWithPlanned(
            Func<T, object> selector,
            Func<T, object> plannedSelector,
            ChangeTypeEnum code)
        {
            if (selector(_original).IsDeepEqual(selector(_updated))) return this;
            bool planned = plannedSelector != null && !plannedSelector(_original).IsDeepEqual(plannedSelector(_updated));

            Logs = Logs.Append(LogAction(code, selector(_original), selector(_updated), planned));

            return this;
        }

        public ChangeTracker<T> CheckChanges(
            Func<T, object> selector,
            ChangeTypeEnum code)
        {
            CheckChangesWithPlanned(selector, null, code);
            return this;
        }

        private static AuditLogEntry LogAction(ChangeTypeEnum code, object origin, object updated, bool planned)
        {
            if (!ChangeTypeCodes.ContainsKey(code))
                throw new ArgumentException($"Change type {code} is not exist.");
            return new AuditLogEntry
            {
                Original = origin,
                NewValue = updated,
                ChangeType = $"{(int)code + 1} {ChangeTypeCodes[code]}",
                IsPlanned = planned
            };
        }
    }
}