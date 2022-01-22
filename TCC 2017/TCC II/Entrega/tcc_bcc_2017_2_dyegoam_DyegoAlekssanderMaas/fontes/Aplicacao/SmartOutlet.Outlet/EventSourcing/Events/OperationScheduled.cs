using System;

namespace SmartOutlet.Outlet.EventSourcing.Events
{
    public class OperationScheduled : IPlugEvent
    {
        public CommandType Type { get; set; }
        public TimeSpan TimeInFuture { get; set; }

        public OperationScheduled(CommandType type, TimeSpan timeInFuture)
        {
            Type = type;
            TimeInFuture = timeInFuture;
        }

        protected OperationScheduled()
        {
        }
    }
}