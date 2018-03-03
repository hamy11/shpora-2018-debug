using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MetricsCollector
{
    public class MetricsCollector
    {
        private SystemUptimeProvider uptimeProvider;
        public event EventHandler<MetricsCollection> MetricsAvailable = delegate { };

        public MetricsCollector()
        {
            uptimeProvider = new SystemUptimeProvider();
        }

        public void Run(TimeSpan period)
        {
            while (true)
            {
                Collect();
                Thread.Sleep(period);
            }
        }

        private void Collect()
        {
            var processes = Process.GetProcesses();
            var processMetrics = processes.Select(x => new ProcessMetrics(x)).ToArray();
            var upTime = uptimeProvider.GetValue();
            var metricsCollection = new MetricsCollection
            {
                ProcessMetricsCollection = processMetrics,
                Timestamp = DateTime.UtcNow,
                Uptime = upTime,
                MachineName = Environment.MachineName
            };
            Task.Run(() => MetricsAvailable(this, metricsCollection));
        }
    }
}