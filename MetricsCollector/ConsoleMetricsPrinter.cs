using System;
using System.Linq;
using ConsoleTables;

namespace MetricsCollector
{
    public class ConsoleMetricsPrinter
    {
        private object sync = new object();
        
        public void Print(MetricsCollection metricsCollection, int top)
        {
            lock (sync)
            {
                PrintInternal(metricsCollection, top);
            }
        }

        private static void PrintInternal(MetricsCollection collection, int top)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Machine: {collection.MachineName}, Uptime: {collection.Uptime}, Timestamp: {collection.Timestamp}");
            ConsoleTable.From(
                    collection
                        .ProcessMetricsCollection
                        .OrderByDescending(x => x.WorkingSet)
                        .Take(top)
                        .Select(x => new ProcessMetricsModel(x)))
                .Write(Format.MarkDown);
        }
    }
}