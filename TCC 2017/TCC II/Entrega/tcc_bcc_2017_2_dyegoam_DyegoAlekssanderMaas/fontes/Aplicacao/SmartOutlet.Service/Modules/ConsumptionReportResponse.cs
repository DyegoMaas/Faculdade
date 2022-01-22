using System.Collections.Generic;

namespace SmartOutlet.Service.Modules
{
    public class ConsumptionReportResponse
    {
        public IList<ConsumptionResponse> Data { get; set; }
        public double kWh { get; set; }
    }
}