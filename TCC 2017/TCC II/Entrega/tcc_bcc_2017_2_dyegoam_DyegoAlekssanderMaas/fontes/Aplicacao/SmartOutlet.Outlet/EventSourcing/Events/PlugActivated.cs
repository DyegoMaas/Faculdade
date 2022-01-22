using System;

namespace SmartOutlet.Outlet.EventSourcing.Events
{
    public class PlugActivated : IPlugEvent
    {
        public Guid PlugId { get; set; }
        public string PlugName { get; set; }

        public PlugActivated(Guid plugId, string plugName)
        {
            PlugId = plugId;
            PlugName = plugName;
        }

        protected PlugActivated()
        {
        }
    }
}