using System;
using System.Threading;

namespace Windbg.Demo
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            var thread = new Thread(Method1, 4096);

            thread.Start();

            thread.Join();

            Console.Out.WriteLine(field);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Method1()
        {
            field++;

            Method2();

            field++;
        }

        private static void Method2()
        {
            field--;

            Method3();

            field--;
        }

        private static void Method3()
        {
            field /= 2;

            Method1();

            field /= 2;
        }

        private static int field;
    }
}
