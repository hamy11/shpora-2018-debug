using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD.Profiler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pid = 0; //TODO
            var seconds = 100; //TODO
            var sw = new Stopwatch();
            var tree = new CallTree("Process");
            using (var dt = DataTarget.AttachToProcess(pid, 10, AttachFlag.Passive))
            {
                var runtime = dt.ClrVersions[0].CreateRuntime();

                if (dt.PointerSize != IntPtr.Size)
                    throw new Exception();

                sw.Start();

                while (sw.Elapsed < TimeSpan.FromSeconds(seconds))
                {
                    foreach (var thread in runtime.Threads)
                        tree.Add(thread.StackTrace);

                    Thread.Sleep(10);
                    runtime.Flush();
                    ClrRuntimePatcher.Flush(runtime);
                }
            }

            tree.Print();
        }
    }

    public static class ClrRuntimePatcher
    {
        public static void Flush(ClrRuntime runtime)
        {
            var field = runtime.GetType().BaseType
                .GetField("_modules", BindingFlags.Instance | BindingFlags.NonPublic);
            var dict = field.GetValue(runtime);
            dict.GetType().GetMethod("Clear").Invoke(dict, new object[0]);

            var field2 = runtime.GetType().BaseType
                .GetField("_moduleFiles", BindingFlags.Instance | BindingFlags.NonPublic);
            var dict2 = field2.GetValue(runtime);
            dict2.GetType().GetMethod("Clear").Invoke(dict2, new object[0]);
        }
    }
}