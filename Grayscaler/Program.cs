using System;
using System.Diagnostics;
using System.IO;
using libbmp;

namespace Grayscaler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var image = new image(args[0]);
            image.grayscale();
            image.save("gray_" + Path.GetFileName(args[0]));
            Console.WriteLine(sw.Elapsed);
        }
    }
}