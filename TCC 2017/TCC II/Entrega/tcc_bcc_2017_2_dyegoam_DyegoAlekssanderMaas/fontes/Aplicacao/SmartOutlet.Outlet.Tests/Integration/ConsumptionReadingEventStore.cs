using System;
using System.Configuration;
using System.Linq;
using FluentAssertions;
using Marten;
using Marten.Events.Projections;
using NUnit.Framework;
using Remotion.Linq.Clauses.ResultOperators;
using SmartOutlet.Outlet.EventSourcing;
using SmartOutlet.Outlet.EventSourcing.Events;
using SmartOutlet.Outlet.EventSourcing.Projections;
using SmartOutlet.Outlet.EventSourcing.Reports;

namespace SmartOutlet.Outlet.Tests.Integration
{
    public class ConsumptionReadingEventStore
    {
        private DocumentStore _documentStore;

        [SetUp]
        public void SetUp()
        {
            _documentStore = NewEventSource();
        }

        private static DocumentStore NewEventSource()
        {
            return DocumentStoreForTests.NewEventSource(
                typeof(PlugActivated),
                typeof(PlugRenamed),
                typeof(PlugTurnedOn),
                typeof(PlugTurnedOff),
                typeof(OperationScheduled),
                typeof(ConsumptionReadingReceived)
            );
        }

        [Test]
        public void saving_consumption_readings_in_event_store()
        {
            var pinheiro = new PlugActivated(Guid.NewGuid(), "Pinheiro de Natal");
            var tv = new PlugActivated(Guid.NewGuid(), "TV Set");

            // activating two plugs
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(pinheiro.PlugId, pinheiro);
                session.Events.Append(tv.PlugId, tv);

                session.SaveChanges();
            }
            
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(pinheiro.PlugId, 
                    new PlugRenamed("New Name"),
                    new PlugTurnedOn(),
                    new ConsumptionReadingReceived(2.2, 220, 20),
                    new ConsumptionReadingReceived(2.2, 220, 20),
                    new ConsumptionReadingReceived(2.2, 220, 22),
                    new ConsumptionReadingReceived(2.2, 220, 23),
                    new ConsumptionReadingReceived(2.2, 220, 24),
                    new ConsumptionReadingReceived(2.2, 220, 20),
                    new OperationScheduled(CommandType.TurnOff, 30.Minutes()),
                    new ConsumptionReadingReceived(2.3, 220, 21),
                    new ConsumptionReadingReceived(2.3, 220, 22),
                    new ConsumptionReadingReceived(2.3, 220, 20),
                    new ConsumptionReadingReceived(2.3, 220, 21),
                    new PlugTurnedOff(), 
                    new PlugTurnedOn(),
                    new ConsumptionReadingReceived(2.3, 220, 21)
                );
                
                session.Events.Append(tv.PlugId, 
                    new PlugTurnedOn(), 
                    new PlugTurnedOff()
                );

                session.SaveChanges();
            }
            
            using (var session = _documentStore.LightweightSession())
            {
                var plugPinheiro = session.Load<Plug>(pinheiro.PlugId);
                var plugTv = session.Load<Plug>(tv.PlugId);

                plugPinheiro.IsOn().Should().BeTrue();
                plugPinheiro.Name.Should().Be("New Name");
                
                plugTv.IsOn().Should().BeFalse();
            }
        }
        
        [Test]
        public void reporting_a_plugs_consumption()
        {
            var pinheiro = new PlugActivated(Guid.NewGuid(), "Pinheiro de Natal");

            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(pinheiro.PlugId, pinheiro);
                session.SaveChanges();
            }
            
            using (var session = _documentStore.OpenSession())
            {
                session.Events.Append(pinheiro.PlugId, 
                    new PlugTurnedOn(),
                    new ConsumptionReadingReceived(2.3, 220, 20),
                    new ConsumptionReadingReceived(2.3, 220, 20),
                    new ConsumptionReadingReceived(2.3, 220, 22),
                    new ConsumptionReadingReceived(2.3, 220, 23),
                    new ConsumptionReadingReceived(2.3, 220, 24),
                    new ConsumptionReadingReceived(2.3, 220, 20),
                    new ConsumptionReadingReceived(2.3, 220, 21),
                    new ConsumptionReadingReceived(2.3, 220, 22),
                    new ConsumptionReadingReceived(2.3, 220, 20),
                    new ConsumptionReadingReceived(2.3, 220, 21),
                    new PlugTurnedOff(), 
                    new PlugTurnedOn(),
                    new ConsumptionReadingReceived(2.2, 220, 21)
                );
                session.SaveChanges();
            }

            var reporter = new ConsumptionReporter(_documentStore);
            var consumptionReport = reporter.GetConsumptionReport(pinheiro.PlugId);
            consumptionReport.Select(x => x.ConsumptionInWatts).Should().BeEquivalentTo(new []
            {
                20.0, 20.0, 22.0, 23.0, 24.0 ,20.0, 21.0, 22.0, 20.0, 21.0, 21.0
            });
            
            using (var session = _documentStore.OpenSession())
            {
                session.Load<Plug>(pinheiro.PlugId).LastConsumptionInWatts.Should().Be(21);
            }
        }
    }
}