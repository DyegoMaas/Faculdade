using System;
using SmartOutlet.Outlet.EventSourcing.Events;

namespace SmartOutlet.Outlet.EventSourcing.Projections
{
    public class Plug
    {
        public Guid Id { get; set; } 
        public PlugState CurrentState { get; set; }
        public string Name { get; set; }
        public double LastConsumptionInWatts { get; set; }
        public bool Active { get; set; }
        public bool IsOn() => CurrentState == PlugState.On;

        public void Apply(PlugActivated activation)
        {
            Id = activation.PlugId;
            Name = activation.PlugName;
            Active = true;
        }
        
        public void Apply(PlugDeactivated deactivation)
        {
            Active = false;
        }
        
        public void Apply(PlugReactivated reactivation)
        {
            Active = true;
        }
        
        public void Apply(PlugTurnedOn plugTurnedOn)
        {
            CurrentState = PlugState.On;
        }
        
        public void Apply(PlugTurnedOff plugTurnedOff)
        {
            CurrentState = PlugState.Off;
        }

        public void Apply(ConsumptionReadingReceived consumptionReading)
        {
            LastConsumptionInWatts = consumptionReading.ConsumptionInWatts;
        }

        public void Apply(PlugRenamed plugRenamed)
        {
            Name = plugRenamed.NewName;
        }
    }
}