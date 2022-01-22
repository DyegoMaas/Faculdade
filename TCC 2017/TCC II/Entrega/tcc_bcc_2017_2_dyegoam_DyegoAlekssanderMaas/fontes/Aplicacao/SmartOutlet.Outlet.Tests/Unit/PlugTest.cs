using System;
using FluentAssertions;
using NUnit.Framework;
using SmartOutlet.Outlet.EventSourcing;
using SmartOutlet.Outlet.EventSourcing.Events;
using SmartOutlet.Outlet.EventSourcing.Projections;

namespace SmartOutlet.Outlet.Tests.Unit
{
    public class PlugTest
    {
        private Plug _plug;

        [SetUp]
        public void SetUp()
        {
            _plug = new Plug();
        }
        
        [Test]
        public void activating_a_plug()
        {
            var plugActivated = new PlugActivated(Guid.NewGuid(), "WONDROUS");
            
            _plug.Apply(plugActivated);

            _plug.Id.Should().Be(plugActivated.PlugId);
            _plug.Name.Should().Be(plugActivated.PlugName);
        }

        [Test]
        public void turning_a_plug_on()
        {
            _plug.Apply(new PlugTurnedOn());
            _plug.CurrentState.Should().Be(PlugState.On);
        }
        
        [Test]
        public void turning_a_plug_off()
        {
            _plug.Apply(new PlugTurnedOff());
            _plug.CurrentState.Should().Be(PlugState.Off);
        }

        [Test]
        public void reading_consumption()
        {
            var consumptionReadingReceived = new ConsumptionReadingReceived(5.2, 220, 20.5d);
            
            _plug.Apply(consumptionReadingReceived);

            _plug.LastConsumptionInWatts.Should().Be(consumptionReadingReceived.ConsumptionInWatts);
        }

        [Test]
        public void renaming_a_plug()
        {
            var plugRenamed = new PlugRenamed(newName: "ELECTRIC SHEEP");
            
            _plug.Apply(plugRenamed);

            _plug.Name.Should().Be(plugRenamed.NewName);
        }
    }
}