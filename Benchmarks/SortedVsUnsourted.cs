using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class SortedVsUnsourted
    {
        private int[] sorted, unsorted;
        
        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            unsorted = Enumerable.Range(0, 10000).Select(x => rnd.Next(0, 1000)).ToArray();
            sorted = unsorted.OrderBy(x => x).ToArray();
        }

        [Benchmark]
        public int Sorted()
        {
            return CountLessThan(sorted, 500);
        }

        [Benchmark]
        public int Unsorted()
        {
            return CountLessThan(unsorted, 500);
        }

        private static int CountLessThan(int[] arr, int border)
        {
            var count = 0;
            foreach (var i in arr)
                if (i < border)
                    count++;
            return count;
        }
    }
}