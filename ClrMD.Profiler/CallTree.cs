using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Diagnostics.Runtime;

namespace ClrMD.Profiler
{
    public class CallTree
    {
        public CallTree(string methodName)
        {
            MethodName = methodName;
        }

        public void Add(IList<ClrStackFrame> stackTrace)
        {
            if (stackTrace.Count == 0)
                return;
            var names = new string[stackTrace.Count];
            for(var i = 0; i< names.Length; ++i)
                names[i] = stackTrace[i].DisplayString;
            Add(names, names.Length-1);
        }
        
        private void Add(string[] stackTrace, int index)
        {
            ++Hits;
            if (index == 0)
            {
                ++Tops;
                return;
            }
            var nextMethod = stackTrace[index - 1];
            if (!Nodes.ContainsKey(nextMethod))
                Nodes[nextMethod] = new CallTree(nextMethod);
            Nodes[nextMethod].Add(stackTrace, index - 1);
        }

        public void Print(int indent = 0)
        {
            Console.Write(new string('-', indent));
            Console.WriteLine(MethodName + " " + Tops + " " + Hits);
            var top = Nodes.OrderByDescending(x => x.Value.Hits);
            ++indent;
            foreach (var callee in top)
                callee.Value.Print(indent);
        }

        public string MethodName;
        public int Hits;
        public int Tops;
        public Dictionary<string, CallTree> Nodes = new Dictionary<string, CallTree>();
    }
}