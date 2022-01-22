using System;
using Marten;
using SmartOutlet.Outlet.EventSourcing.Events;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.EventSourcing
{
    public class PlugEventSequencer : IPlugEventSequencer
    {
        private readonly IDocumentStore _documentStore;

        public PlugEventSequencer(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void PlugActivated(Guid plugId, string name)
        {
            using (var session = _documentStore.OpenSession())
            {
                var @event = new PlugActivated(plugId, name);
                session.Events.StartStream<Plug>(plugId, @event);
                session.SaveChanges();
            }
        }

        public void PlugDeactivated(Guid plugId)
        {
            AppendEvent(plugId, new PlugDeactivated());
        }

        public void PlugReactivated(Guid plugId)
        {
            AppendEvent(plugId, new PlugReactivated());
        }

        public void ActionScheduled(ScheduleCommand command, Guid plugId)
        {
            AppendEvent(plugId, new OperationScheduled(command.Type, command.TimeInFuture));
        }

        public void PlugRenamed(string newName, Guid plugId)
        {
            AppendEvent(plugId, new PlugRenamed(newName));
        }

        public void PlugTurnedOn(Guid plugId)
        {
            AppendEvent(plugId, new PlugTurnedOn());
        }

        public void PlugTurnedOff(Guid plugId)
        {
            AppendEvent(plugId, new PlugTurnedOff());
        }

        public void NewConsumption(Guid plugId, double current, double voltage, double consumptionInWatts)
        {
            AppendEvent(plugId, new ConsumptionReadingReceived(current, voltage, consumptionInWatts));
        }

        private void AppendEvent<T>(Guid plugId, T @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(plugId, @event);
                session.SaveChanges();
            }
        }
    }
}