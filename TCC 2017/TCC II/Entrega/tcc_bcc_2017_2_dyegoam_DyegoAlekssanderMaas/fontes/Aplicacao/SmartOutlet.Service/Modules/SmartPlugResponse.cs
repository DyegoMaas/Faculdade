using System;

namespace SmartOutlet.Service.Modules
{
    public class SmartPlugResponse
    {
        public Guid PlugId { get; set; }
        public bool IsOn { get; set; }
        public string Name { get; set; }
        public double LastConsumption { get; set; }
    }
}