using System;

namespace MetricsCollector
{
    public class MetricsCollection
    {
        public DateTime Timestamp { get; set; }
        public TimeSpan Uptime { get; set; }
        public ProcessMetrics[] ProcessMetricsCollection { get; set; }
        public string MachineName { get; set; }
    }
}