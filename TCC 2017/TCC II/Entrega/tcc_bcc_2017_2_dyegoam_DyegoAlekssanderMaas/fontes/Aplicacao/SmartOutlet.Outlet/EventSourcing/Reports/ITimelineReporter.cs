using System;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public interface ITimelineReporter
    {
        TimeLine LoadTimeLine(Guid plugId);
    }
}