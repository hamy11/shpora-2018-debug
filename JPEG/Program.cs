using System;
using System.Diagnostics;
using Utilities;
namespace JPEG
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<CompressDecompressBenchmark>();

            //FFT.Test();
            //var fileName = @"..\..\Big_Black_River_Railroad_Bridge.bmp";
            var fileName = @"..\..\sample.bmp";
            var compressor = new JpegCompressor(fileName);

            var sw = Stopwatch.StartNew();

            compressor.Compress();

            sw.Stop();
            Console.WriteLine("Compression: " + sw.Elapsed);
            sw.Restart();

            compressor.Decompress();

            Console.WriteLine("Decompression: " + sw.Elapsed);
            Console.WriteLine($"Peak commit size: {MemoryMeter.PeakPrivateBytes() / (1024.0 * 1024):F2} MB");
            Console.WriteLine($"Peak working set: {MemoryMeter.PeakWorkingSet() / (1024.0 * 1024):F2} MB");
            //Console.ReadLine();
        }
    }
}