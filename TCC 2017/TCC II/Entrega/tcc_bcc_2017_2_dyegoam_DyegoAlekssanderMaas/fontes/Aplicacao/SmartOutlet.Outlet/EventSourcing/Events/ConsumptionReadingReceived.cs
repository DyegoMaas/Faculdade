using System;

namespace SmartOutlet.Outlet.EventSourcing.Events
{
    public class ConsumptionReadingReceived
    {
        public double Current { get; set; }
        public double Voltage { get; set; }
        public double ConsumptionInWatts { get; set; }

        public ConsumptionReadingReceived(double current, double voltage, double consumptionInWatts)
        {
            Current = current;
            Voltage = voltage;
            ConsumptionInWatts = consumptionInWatts;
        }

        protected ConsumptionReadingReceived()
        {
        }
    }
}