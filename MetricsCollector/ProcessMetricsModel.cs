namespace MetricsCollector
{
    public class ProcessMetricsModel
    {
        public ProcessMetricsModel(ProcessMetrics metrics)
        {
            ProcessId = metrics.ProcessId;
            Name = metrics.Name;
            WorkingSet = $"{metrics.WorkingSet / (1024.0 * 1024):F2} MB";
            PrivateBytes = $"{metrics.PrivateBytes / (1024.0 * 1024):F2} MB";
            ThreadsCount = metrics.ThreadsCount;
        }

        public int ProcessId { get; set; }
        
        public string Name { get; set; }
        
        public string WorkingSet { get; set; }
        
        public string PrivateBytes { get; set; }
        
        public int ThreadsCount { get; set; }
    }
}