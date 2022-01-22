using System;
using Marten;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public class TimelineReporter : ITimelineReporter
    {
        private readonly IDocumentStore _documentStore;

        public TimelineReporter(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public TimeLine LoadTimeLine(Guid plugId)
        {
            using (var session = _documentStore.OpenSession())
            {
                return session.Load<TimeLine>(plugId);
            }
        }
    }
}