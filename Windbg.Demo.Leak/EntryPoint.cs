using System.Threading;

namespace Windbg.Demo
{
    internal class EntryPoint
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 50; i++)
            {
                var buffer = new byte[1024 * 1024];

                var timer = new Timer(_ =>
                {
                    buffer.ToString();
                });

                timer.Change(1000, 1000);
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
