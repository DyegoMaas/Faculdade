using System;

namespace SmartOutlet.Outlet
{
    public interface ISmartPlug
    {
        void TryTurnOff(Guid plugId);
        void TryTurnOn(Guid plugId);
        void ScheduleTurnOn(ScheduleCommand command, Guid plugId);
        void ScheduleTurnOff(ScheduleCommand command, Guid plugId);
        void Rename(string newName, Guid plugId);
        Guid Activate(string name);
        void Deactivate(Guid plugId);
        void ResetCredentials();
        void Reactivate(Guid plugId);
        void Calibrate(Guid plugId);
    }
}