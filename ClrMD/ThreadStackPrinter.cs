using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD
{
    public class ThreadStackPrinter
    {
        public static void PrintStack(ClrThread clrThread)
        {
            Console.WriteLine($"==== Thread: {clrThread.ManagedThreadId} - {clrThread.OSThreadId} ====");
            foreach (var stackFrame in clrThread.StackTrace)
            {
                if (stackFrame.Kind == ClrStackFrameType.Runtime)
                    continue;
                Console.WriteLine(stackFrame.DisplayString);
            }

            Console.WriteLine("====");
        }
    }
}