using System;
using System.Threading;
using System.Threading.Tasks;

namespace Windbg.Demo
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                ThreadPool.QueueUserWorkItem(obj => StupidMethod());

                Thread.Sleep(100);
            }
        }

        private static void StupidMethod()
        {
            new TaskCompletionSource<Boolean>().Task.Wait();
        }
    }
}
