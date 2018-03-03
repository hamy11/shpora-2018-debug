using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Benchmarks
{
    [DisassemblyDiagnoser, RyuJitX64Job, LegacyJitX86Job]
    public class TailCallBenchmark
    {
        private int[] intArr;
        private long[] longArr;

        [GlobalSetup]
        public void Setup()
        {
            intArr = new int[47];
            intArr[1] = 1;
            for (int i = 2; i < intArr.Length; ++i)
                intArr[i] = intArr[i - 1] + intArr[i - 2];
        
            longArr = new long[47];
            longArr[1] = 1;
            for (int i = 2; i < longArr.Length; ++i)
                longArr[i] = longArr[i - 1] + longArr[i - 2];
        }

        [Benchmark]
        public int GcdInt()
        {
            int x = 0;
            for (int i = 0; i < intArr.Length - 1; ++i)
                x ^= GcdInt(intArr[i], intArr[i + 1]);
            return x;
        }

        [Benchmark]
        public long GcdLong()
        {
            long x = 0;
            for (int i = 0; i < longArr.Length - 1; ++i)
                x ^= GcdLong(longArr[i], longArr[i + 1]);
            return x;
        }
        
        public static int GcdInt(int a, int b){
            return b == 0 ? a : GcdInt(b, a%b);
        }
        public static long GcdLong(long a, long b){
            return b == 0 ? a : GcdLong(b, a%b);
        }
    }
}