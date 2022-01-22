using System;

namespace SmartOutlet.Outlet.EventSourcing.Reports
{
    public class ConsumptionInTime
    {
        public DateTimeOffset TimeStamp { get;  }
        public double ConsumptionInWatts { get; }
        public double Current { get; set; }


        public ConsumptionInTime(double consumptionInWatts, double current, DateTimeOffset timeStamp)
        {
            TimeStamp = timeStamp;
            ConsumptionInWatts = consumptionInWatts;
            Current = current;
        }
    }
}