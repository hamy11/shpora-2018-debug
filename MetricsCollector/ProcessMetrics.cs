using System.Diagnostics;

namespace MetricsCollector
{
    public class ProcessMetrics
    {
        public ProcessMetrics(Process process)
        {
            ProcessId = process.Id;
            ThreadsCount = process.Threads.Count;
            PrivateBytes = process.PrivateMemorySize64;
            WorkingSet = process.WorkingSet64;
            Name = process.ProcessName;
            PeakWorkingSet = process.PeakWorkingSet64;
            PeakPrivateBytes = process.PeakPagedMemorySize64;
            HandlesCount = process.HandleCount;
            SessionId = process.SessionId;
            BasePriority = process.BasePriority;
        }

        public int BasePriority { get; set; }

        public int SessionId { get; set; }

        public int HandlesCount { get; set; }

        public long PeakPrivateBytes { get; set; }

        public long PeakWorkingSet { get; set; }

        public int ProcessId { get; set; }
        
        public string Name { get; set; }
        
        public long WorkingSet { get; set; }
        
        public long PrivateBytes { get; set; }
        
        public int ThreadsCount { get; set; }
    }
}