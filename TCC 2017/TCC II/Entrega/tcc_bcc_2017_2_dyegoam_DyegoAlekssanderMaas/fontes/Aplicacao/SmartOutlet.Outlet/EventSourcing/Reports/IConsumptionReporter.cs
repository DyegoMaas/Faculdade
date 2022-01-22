using System;
using System.Collections.Generic;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public interface IConsumptionReporter
    {
        IEnumerable<ConsumptionInTime> GetConsumptionReport(Guid plugId, DateTime? startTime = null);
    }
}