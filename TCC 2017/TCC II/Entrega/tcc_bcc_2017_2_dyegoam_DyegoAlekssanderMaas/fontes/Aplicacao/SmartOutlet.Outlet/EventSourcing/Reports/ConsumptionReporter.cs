using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Marten.Events;
using SmartOutlet.Outlet.EventSourcing.Events;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public class ConsumptionReporter : IConsumptionReporter
    {
        private readonly IDocumentStore _documentStore;

        public ConsumptionReporter(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public IEnumerable<ConsumptionInTime> GetConsumptionReport(Guid plugId, DateTime? startTime = null)
        {
            using (var session = _documentStore.LightweightSession())
            {
                var consumption = session.Events
                    .FetchStream(plugId, timestamp:startTime)
                    .Where(e => 
                        e.StreamId == plugId &&
                        e.Data.GetType() == typeof(ConsumptionReadingReceived))
                    .OrderBy(e => e.Timestamp)
                    .Select(e =>
                    {
                        var reading = (ConsumptionReadingReceived) e.Data;
                        return new ConsumptionInTime(ConvertToRMS(reading), reading.Current, e.Timestamp);
                    });
                return consumption;
            }
        }

        private static double ConvertToRMS(ConsumptionReadingReceived reading)
        {
            return reading.ConsumptionInWatts * 0.707;
        }
    }
}