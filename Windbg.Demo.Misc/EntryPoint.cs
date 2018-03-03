using System;
using System.Threading;

namespace Windbg.Demo
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            holder = new StateHolder
            {
                ClassState = Tuple.Create(123, "Hello"),
                StructState = (987, "WinDbg!")
            };

            Thread.Sleep(Timeout.Infinite);
        }

        private static StateHolder holder;
    }

    internal class StateHolder
    {
        public Tuple<Int32, string> ClassState;
        public (Int32, string) StructState;
    }
}
