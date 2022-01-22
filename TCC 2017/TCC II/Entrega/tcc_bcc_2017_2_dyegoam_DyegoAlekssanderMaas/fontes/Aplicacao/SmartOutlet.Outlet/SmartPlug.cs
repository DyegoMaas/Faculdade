using System;
using SmartOutlet.Outlet.EventSourcing;
using SmartOutlet.Outlet.Mqtt;

namespace SmartOutlet.Outlet
{
    public class SmartPlug : ISmartPlug
    {
        private readonly IPublisher _publisher;
        private readonly IPlugEventSequencer _plugEventSequencer;

        public SmartPlug(IPublisher publisher, IPlugEventSequencer plugEventSequencer)
        {
            _publisher = publisher;
            _plugEventSequencer = plugEventSequencer;
        }

        public Guid Activate(string name)
        {
            var plugId = Guid.NewGuid();
            _plugEventSequencer.PlugActivated(plugId, name);
            
            var payload = $"{plugId}";
            _publisher.Publish("/smart-things/plug/activate", payload);
            
            return plugId;
        }

        public void Deactivate(Guid plugId)
        {
            _plugEventSequencer.PlugDeactivated(plugId);
        }

        public void Reactivate(Guid plugId)
        {
            _plugEventSequencer.PlugReactivated(plugId);
        }

        public void Calibrate(Guid plugId)
        {
            var payload = $"{plugId}|";
            _publisher.Publish("/smart-things/plug/calibrate", payload);
        }

        public void ResetCredentials()
        {
            _publisher.Publish("/smart-things/plug/clean-identity", string.Empty);
        }

        public void Rename(string newName, Guid plugId)
        {
            _plugEventSequencer.PlugRenamed(newName, plugId);
        }

        public void TryTurnOff(Guid plugId)
        {
            _publisher.Publish("/smart-things/plug/state", $"{plugId}|turn-off");
        }

        public void TryTurnOn(Guid plugId)
        {
            _publisher.Publish("/smart-things/plug/state", $"{plugId}|turn-on");
        }

        public void ScheduleTurnOn(ScheduleCommand command, Guid plugId)
        {
            _publisher.Publish("/smart-things/plug/schedule-on", $"{plugId}|{GetMilisecondsString(command.TimeInFuture)}");
            _plugEventSequencer.ActionScheduled(command, plugId);
        }

        public void ScheduleTurnOff(ScheduleCommand command, Guid plugId)
        {
            _publisher.Publish("/smart-things/plug/schedule-off", $"{plugId}|{GetMilisecondsString(command.TimeInFuture)}");
            _plugEventSequencer.ActionScheduled(command, plugId);
        }

        private static string GetMilisecondsString(TimeSpan timeInFuture)
        {
            return ((int)timeInFuture.TotalMilliseconds).ToString();
        }
    }
}