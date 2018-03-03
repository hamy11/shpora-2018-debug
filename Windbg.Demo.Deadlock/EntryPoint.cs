using System.Threading;

namespace Windbg.Demo
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            var lock1 = new object();
            var lock2 = new object();

            var barrier = new Barrier(2);

            ThreadPool.QueueUserWorkItem(obj =>
            {
                lock (lock1)
                {
                    barrier.SignalAndWait();

                    lock (lock2) { }
                }
            });

            ThreadPool.QueueUserWorkItem(obj =>
            {
                lock (lock2)
                {
                    barrier.SignalAndWait();

                    lock (lock1) { }
                }
            });

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
