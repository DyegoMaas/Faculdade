using System;

namespace SmartOutlet.Outlet
{
    public class ScheduleCommand
    {
        public DateTime EstimatedExecutionTime { get; }
        public CommandType Type { get; }
        public TimeSpan TimeInFuture { get; }
        public string Description => EstimatedExecutionTime.ToString("MM/dd/yyyy HH:mm:ss");

        public ScheduleCommand(CommandType type, TimeSpan timeInFuture)
        {
            Type = type;
            TimeInFuture = timeInFuture;
            EstimatedExecutionTime = DateTime.Now + timeInFuture;
        }
    }
}