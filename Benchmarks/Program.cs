using System;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // BenchmarkRunner.Run<MemoryTraffic>();
            // BenchmarkRunner.Run<TailCallBenchmark>();
            // BenchmarkRunner.Run<StructVsClassBenchmark>();
            // BenchmarkRunner.Run<BitCountBenchmark>();
            BenchmarkRunner.Run<ByteArrayEqualityBenchmark>();
            // BenchmarkRunner.Run<SortedVsUnsourted>();
            // BenchmarkRunner.Run<NewConstraintBenchmark>();
            // BenchmarkRunner.Run<MaxBenchmark>();
        }
    }
}