using System;
using System.Collections.Generic;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public interface IPlugStateReporter
    {
        IEnumerable<Plug> GetStateReport(params Guid[] plugIds);
    }
}