using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using SmartOutlet.Outlet.EventSourcing.Events;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public class PlugStateReporter : IPlugStateReporter
    {
        private readonly IDocumentStore _documentStore;

        public PlugStateReporter(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public IEnumerable<Plug> GetStateReport(Guid[] plugIds)
        {
            using (var session = _documentStore.OpenSession())
            {
                return session.Query<Plug>()
                    .Where(x => x.Active)
                    .ToArray()
                    .Where(x => FilterPlugs(x, plugIds));
            }
        }

        private bool FilterPlugs(Plug plug, Guid[] plugIds)
        {
            if (plugIds.Any())
                return plugIds.Contains(plug.Id);
            return true;
        }
    }
}