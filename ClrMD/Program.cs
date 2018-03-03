using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Можно отлаживать уже запущенный процесс, не только разбирать дамп памяти:
            // using (var dt = DataTarget.AttachToProcess(pid, msTimeout, AttachFlag.Invasive))
            // using (var dt = DataTarget.AttachToProcess(pid, msTimeout, AttachFlag.NonInvasive))
            // using (var dt = DataTarget.AttachToProcess(pid, msTimeout, AttachFlag.Passive))
            using (var dt = DataTarget.LoadCrashDump(args[0]))
             {
                if (dt.PointerSize != IntPtr.Size)
                    throw new Exception();
            
                var runtime = dt.ClrVersions[0].CreateRuntime();
                
                var heap = runtime.Heap;
                
                Console.WriteLine(heap);

                foreach (var thread in runtime.Threads)
                {
                    ThreadStackPrinter.PrintStack(thread);
                }

                DictionariesPrinter.PrintDictionaries(heap);
                DictionariesPrinter.PrintConcurrentDictionaries(heap);
            }
        }
        
    }
}