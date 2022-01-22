using System;

namespace SmartOutlet.Outlet.EventSourcing
{
    public interface IPlugEventSequencer
    {
        void NewConsumption(Guid plugId, double current, double voltage, double consumptionInWatts);
        void PlugTurnedOn(Guid plugId);
        void PlugTurnedOff(Guid plugId);
        void PlugRenamed(string newName, Guid plugId);
        void PlugActivated(Guid plugId, string name);
        void ActionScheduled(ScheduleCommand command, Guid plugId);
        void PlugDeactivated(Guid plugId);
        void PlugReactivated(Guid plugId);
    }
}