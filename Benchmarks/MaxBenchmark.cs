using System;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [DisassemblyDiagnoser]
    public class MaxBenchmark
    {
        private int[] numbers;
        
        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            numbers = Enumerable.Range(0, 10007).Select(_ => rnd.Next()).ToArray();
        }

        [Benchmark]
        public int SimpleMax()
        {
            return Max(numbers);
        }
        
        [Benchmark]
        public int LinqMax()
        {
            return numbers.Max();
        }

        [Benchmark]
        public int MathMax()
        {
            var max = int.MinValue;
            foreach (var number in numbers)
                max = Math.Max(max, number);
            return max;
        }

        [Benchmark]
        public int VectorsMax()
        {
            var mx = new Vector<int>(int.MinValue);
            var len = numbers.Length - numbers.Length % Vector<int>.Count;
            for (var i = 0; i < len; i += Vector<int>.Count)
                mx = Vector.Max(mx, new Vector<int>(numbers, i));
            var res = new int[Vector<int>.Count];
            mx.CopyTo(res);

            return Max(res);
        }

        private static int Max(int[] arr)
        {
            var max = int.MinValue;
            foreach (var number in arr)
                if (number > max)
                    max = number;
            return max;
        }
    }
}