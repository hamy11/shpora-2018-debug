using System;
using System.Diagnostics;

namespace MetricsCollector
{
    public class SystemUptimeProvider : IDisposable
    {
        private readonly PerformanceCounter upTime;

        public SystemUptimeProvider()
        {
            try
            {
                upTime = new PerformanceCounter("System", "System Up Time");
                upTime.NextValue();
            }
            catch
            {
            }
        }

        public TimeSpan GetValue()
        {
            try
            {
                return TimeSpan.FromSeconds(upTime.NextValue());
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        public void Dispose()
        {
            upTime?.Dispose();
        }
    }
}