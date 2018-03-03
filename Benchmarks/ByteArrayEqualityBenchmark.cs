using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Benchmarks
{
    [DisassemblyDiagnoser]
    public class ByteArrayEqualityBenchmark
    {
        private byte[] aArr;
        private byte[] bArr;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(42);
            aArr = new byte[100255];
            bArr = new byte[100255];
            random.NextBytes(aArr);
            Array.Copy(aArr, bArr, aArr.Length);
            bArr[bArr.Length - 1] = (byte) (bArr[bArr.Length - 1] + 1);
        }

        [Benchmark]
        public bool Linq() => LinqCompare(aArr, bArr);

        [Benchmark]
        public bool Trivial() => TrivialCompare(aArr, bArr);

        [Benchmark]
        public bool ReadOnly() => TrivialCompareIReadOnly(aArr, bArr);
        
        [Benchmark]
        public bool Structural() => StructuralCompare(aArr, bArr);

        [Benchmark]
        public bool Unrolled() => TrivialCompareUnrolled(aArr, bArr);

        [Benchmark]
        public bool Long() => LongCompare(aArr, bArr);

        [Benchmark]
        public bool Span() => SpanCompare(aArr, bArr);

        [Benchmark]
        public bool Vectors() => VectorCompare(aArr, bArr);

        [Benchmark]
        public bool SequenceEqual() => SequenceCompare(aArr, bArr);
        
        [Benchmark]
        public bool LongUnrolled() => EqualBytesLongUnrolled(aArr, bArr);
        
        [Benchmark]
        public bool VectorsUnrolled() => EqualBytesVectorsUnrolled(aArr, bArr);

        private static bool SequenceCompare(byte[] bytes, byte[] bArr1)
        {
            return bytes.SequenceEqual(bArr1);
        }

        [Benchmark]
        public bool Native() => NativeCompare(aArr, bArr);


        private static bool TrivialCompare(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
                return false;
            for (var i = 0; i < x.Length; ++i)
                if(x[i] != y[i])
                    return false;
            
            return true;
        }

        private static bool TrivialCompareIReadOnly(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
        {
            if (x.Count != y.Count)
                return false;
            for (var i = 0; i < x.Count; ++i)
                if(x[i] != y[i])
                    return false;
            
            return true;
        }

        private static bool LinqCompare(IReadOnlyCollection<byte> x, IReadOnlyCollection<byte> y)
        {
            return x.Count == y.Count &&
                   x.Zip(y, (a, b) => a - b)
                    .All(a => a == 0);
        }
        
        private static bool StructuralCompare(byte[] x, byte[] y) 
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
        }


        private static bool TrivialCompareUnrolled(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
                return false;
            var i = 0;
            var length = x.Length - x.Length % 8;
            while(i < length)
            {
                if(
                    x[i] != y[i] ||
                    x[i+1] != y[i+1] ||
                    x[i+2] != y[i+2] ||
                    x[i+3] != y[i+3] ||
                    x[i+4] != y[i+4] ||
                    x[i+5] != y[i+5] ||
                    x[i+6] != y[i+6] ||
                    x[i+7] != y[i+7])
                    return false;
                i += 8;
            }
            for (; i < x.Length; ++i)
                if(x[i] != y[i])
                    return false;
            
            return true;
        }

        public static unsafe bool LongCompare(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
                return false;
            var N = x.Length - x.Length % 8;
            fixed(byte* p1 = x, p2 = y)
            {
                byte* x1 = p1, x2 = p2;
                for (var i = 0; i < N; ++i, x1 += 8, x2 += 8)
                    if (*(long*) x1 != *(long*) x2)
                        return false;
                for (var i = 0; i < x.Length % 8; ++i, ++x1, ++x2)
                    if (*x1 != *x2)
                        return false;
                return true;
            }
        }
        
        public static bool SpanCompare(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
                return false;
            var N = x.Length - x.Length % sizeof(long);

            var a = new Span<byte>(x, 0, N).NonPortableCast<byte, long>();
            var b = new Span<byte>(x, 0, N).NonPortableCast<byte, long>();

            for(var i = 0; i< a.Length; ++i)
                if (a[i] != b[i])
                    return false;
            for(var i = N; i< x.Length; ++i)
                if (x[i] != y[i])
                    return false;
            return true;
        }

        public static bool VectorCompare(byte[] x, byte[] y)
        {
            var offset = 0;
            for (var i = 0; i < x.Length / Vector<byte>.Count; ++i, offset += Vector<byte>.Count)
            {
                var a = new Vector<byte>(x, offset);
                var b = new Vector<byte>(y, offset);
                if (!Vector.EqualsAll(a, b))
                    return false;
            }

            for(var i = 0; i < x.Length % Vector<byte>.Count; i++, offset++)
                if(x[offset] != y[offset])
                    return false;

            return true;
        }
        public static unsafe bool NativeCompare(byte[] x, byte[] y)
        {
            fixed (byte* a = x)
            fixed (byte* b = y)
                return memcmp(a, b, (UIntPtr) x.Length) == 0;
        }
        
        [DllImport("msvcrt.dll")]
        public static extern unsafe int memcmp(byte* a, byte* b, UIntPtr length);
        
        static unsafe bool EqualBytesLongUnrolled (byte[] data1, byte[] data2)
        {
            if (data1 == data2)
                return true;
            if (data1.Length != data2.Length)
                return false;

            fixed (byte* bytes1 = data1, bytes2 = data2) {
                int len = data1.Length;
                int rem = len % (sizeof(long) * 16);
                long* b1 = (long*)bytes1;
                long* b2 = (long*)bytes2;
                long* e1 = (long*)(bytes1 + len - rem);

                while (b1 < e1) {
                    if (*(b1) != *(b2) || *(b1 + 1) != *(b2 + 1) || 
                        *(b1 + 2) != *(b2 + 2) || *(b1 + 3) != *(b2 + 3) ||
                        *(b1 + 4) != *(b2 + 4) || *(b1 + 5) != *(b2 + 5) || 
                        *(b1 + 6) != *(b2 + 6) || *(b1 + 7) != *(b2 + 7) ||
                        *(b1 + 8) != *(b2 + 8) || *(b1 + 9) != *(b2 + 9) || 
                        *(b1 + 10) != *(b2 + 10) || *(b1 + 11) != *(b2 + 11) ||
                        *(b1 + 12) != *(b2 + 12) || *(b1 + 13) != *(b2 + 13) || 
                        *(b1 + 14) != *(b2 + 14) || *(b1 + 15) != *(b2 + 15))
                        return false;
                    b1 += 16;
                    b2 += 16;
                }

                for (int i = 0; i < rem; i++)
                    if (data1 [len - 1 - i] != data2 [len - 1 - i])
                        return false;

                return true;
            }
        }
        
        static bool EqualBytesVectorsUnrolled (byte[] data1, byte[] data2)
        {
            if (data1 == data2)
                return true;
            if (data1.Length != data2.Length)
                return false;

                int len = data1.Length;
                int rem = len % (Vector<byte>.Count * 8);
                int end = len - rem;

                for (int i = 0; i < end; i += 8 * Vector<byte>.Count)
                {
                    if (
                        !Vector.EqualsAll(new Vector<byte>(data1, i), new Vector<byte>(data2, i)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + Vector<byte>.Count),
                            new Vector<byte>(data2, i + Vector<byte>.Count)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + 2 * Vector<byte>.Count),
                            new Vector<byte>(data2, i + 2 * Vector<byte>.Count)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + 3 * Vector<byte>.Count),
                            new Vector<byte>(data2, i + 3 * Vector<byte>.Count)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + 4 * Vector<byte>.Count),
                            new Vector<byte>(data2, i + 4 * Vector<byte>.Count)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + 5 * Vector<byte>.Count),
                            new Vector<byte>(data2, i + 5 * Vector<byte>.Count)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + 6 * Vector<byte>.Count),
                            new Vector<byte>(data2, i + 6 * Vector<byte>.Count)) ||
                        !Vector.EqualsAll(new Vector<byte>(data1, i + 7 * Vector<byte>.Count),
                            new Vector<byte>(data2, i + 7 * Vector<byte>.Count))
                    )
                        return false;
                }

                for (int i = 0; i < rem; i++)
                    if (data1[len - 1 - i] != data2 [len - 1 - i])
                        return false;

                return true;
        }
    }
}